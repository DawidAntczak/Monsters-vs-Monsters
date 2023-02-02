using MusicInterface;
using System.Linq;
using UnityEngine;

public class GameStateToControlsConverter
{
    private readonly bool _interpolate;
    private ControlData _lastNotInterpolatedControlData;

    public GameStateToControlsConverter(bool interpolate)
    {
        _interpolate = interpolate;
    }

    public virtual ControlData Convert(GameState gameState, float temperature, float requestedTimeLength)
    {
        return gameState.GameStage switch
        {
            GameState.Stage.Preparing => ConvertForPreparingStage(gameState, temperature, requestedTimeLength),
            GameState.Stage.Playing => ConvertForPlayingStage(gameState, temperature, requestedTimeLength),
            GameState.Stage.Won => ConvertForWonStage(gameState, temperature, requestedTimeLength),
            GameState.Stage.Lost => ConvertForLostStage(gameState, temperature, requestedTimeLength),
        };
    }

    private ControlData ConvertForWonStage(GameState gameState, float temperature, float requestedTimeLength)
    {
        var controlData = new ControlData
        {
            Mode = Vector.OneHot(Controls.Modes.Count, 0),
            AttackDensity = Vector.OneHot(Controls.AttackDensities.Count(), 1),
            AvgPitchesPlayed = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), 2),
            Entropy = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), 2),
            Reset = gameState.IsInitState,
            Temperature = temperature,
            RequestedTimeLength = requestedTimeLength
        };

        return controlData;
    }

    private ControlData ConvertForLostStage(GameState gameState, float temperature, float requestedTimeLength)
    {
        var controlData = new ControlData
        {
            Mode = Vector.OneHot(Controls.Modes.Count, 1),
            AttackDensity = Vector.OneHot(Controls.AttackDensities.Count(), 1),
            AvgPitchesPlayed = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), 2),
            Entropy = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), 2),
            Reset = gameState.IsInitState,
            Temperature = temperature,
            RequestedTimeLength = requestedTimeLength
        };

        return controlData;
    }

    private ControlData ConvertForPreparingStage(GameState gameState, float temperature, float requestedTimeLength)
    {
        var controlData = new ControlData
        {
            Mode = Vector.OneHot(Controls.Modes.Count, 0),
            AttackDensity = Vector.OneHot(Controls.AttackDensities.Count(), 0),
            AvgPitchesPlayed = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), 0),
            Entropy = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), 0),
            Reset = gameState.IsInitState,
            Temperature = temperature,
            RequestedTimeLength = requestedTimeLength
        };

        return controlData;
    }

    private ControlData ConvertForPlayingStage(GameState gameState, float temperature, float requestedTimeLength)
    {
        var danger = Mathf.Pow(gameState.EnemiesCount, 2f) - gameState.DefendersCount;
        var enemiesCount = gameState.EnemiesCount;
        var levelProgess = gameState.LevelProgess * Mathf.Sqrt(enemiesCount) / 4f;

        var (p, q) = (1, 4);
        var nervousness = Mathf.Max(gameState.AverageMouseSpeed / p, gameState.AverageMouseClicks / q, Mathf.Sqrt(enemiesCount) / 4f);

        var controlData = new ControlData
        {
            Mode = danger switch
            {
                <= 0 => Vector.OneHot(Controls.Modes.Count, 0),
                > 0 => Vector.OneHot(Controls.Modes.Count, 1),
            },
            AttackDensity = Vector.NormalizedNormalDistribution(Controls.AttackDensities.Count(), levelProgess * (Controls.AttackDensities.Count()), 1),
            AvgPitchesPlayed = Vector.NormalizedNormalDistribution(Controls.AvgPitchesPlayed.Count(), Mathf.Sqrt(enemiesCount) / 4f * (Controls.AvgPitchesPlayed.Count()), 1),
            Entropy = Vector.NormalizedNormalDistribution(Controls.Entropies.Count(), nervousness * (Controls.Entropies.Count()), 1),
            Reset = gameState.IsInitState,
            Temperature = temperature,
            RequestedTimeLength = requestedTimeLength
        };

        if (_interpolate)
        {
            var interpolatedControlData = _lastNotInterpolatedControlData != null ? new ControlData
            {
                Mode = Interpolate(_lastNotInterpolatedControlData.Mode, controlData.Mode),
                AttackDensity = Interpolate(_lastNotInterpolatedControlData.AttackDensity, controlData.AttackDensity),
                AvgPitchesPlayed = Interpolate(_lastNotInterpolatedControlData.AvgPitchesPlayed, controlData.AvgPitchesPlayed),
                Entropy = Interpolate(_lastNotInterpolatedControlData.Entropy, controlData.Entropy),
                Reset = controlData.Reset,
                Temperature = controlData.Temperature,
                RequestedTimeLength = controlData.RequestedTimeLength
            } : controlData;

            _lastNotInterpolatedControlData = controlData;
            controlData = interpolatedControlData;
        }

        return controlData;
    }

    private Vector Interpolate(Vector lastValue, Vector newValue)
    {
        var newValueArray = newValue.ToEnumerable().ToArray();
        return Vector.FromArray(lastValue.ToEnumerable().Select((x, i) => (x + newValueArray[i]) / 2).ToArray());
    }

    private int CalculateBin(float value, float valueForMaxBin, int binCount)
    {
        var estimatedBin = CalculateContinousBin(value, valueForMaxBin, binCount);
        return Mathf.Clamp(estimatedBin, 0, binCount - 1);
    }

    private int CalculateContinousBin(float value, float valueForMaxBin, int binCount)
    {
        var valuePerBin = valueForMaxBin / (binCount - 1);
        return (int)(value / valuePerBin);
    }
}
