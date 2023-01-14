using MusicInterface;
using System.Linq;
using UnityEngine;

public static class GameStateToControlsConverter
{
    public static ControlData Convert(GameState gameState)
    {
        return gameState.GameStage switch
        {
            GameState.Stage.Preparing => ConvertForPreparingStage(gameState),
            GameState.Stage.Playing => ConvertForPlayingStage(gameState),
            GameState.Stage.Won => ConvertForWonStage(gameState),
            GameState.Stage.Lost => ConvertForLostStage(gameState),
        };
    }

    private static ControlData ConvertForWonStage(GameState gameState)
    {
        var controlData = new ControlData
        {
            Mode = Vector.OneHot(Controls.Modes.Count, 0),
            AttackDensity = Vector.OneHot(Controls.AttackDensities.Count(), 1),
            AvgPitchesPlayed = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), 2),
            Entropy = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), 2),
            Reset = gameState.IsInitState,
            RequestedEventCount = 30
        };

        return controlData;
    }

    private static ControlData ConvertForLostStage(GameState gameState)
    {
        var controlData = new ControlData
        {
            Mode = Vector.OneHot(Controls.Modes.Count, 1),
            AttackDensity = Vector.OneHot(Controls.AttackDensities.Count(), 1),
            AvgPitchesPlayed = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), 2),
            Entropy = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), 2),
            Reset = gameState.IsInitState,
            RequestedEventCount = 30
        };

        return controlData;
    }

    private static ControlData ConvertForPreparingStage(GameState gameState)
    {
        var controlData = new ControlData
        {
            Mode = Vector.OneHot(Controls.Modes.Count, 0),
            AttackDensity = Vector.OneHot(Controls.AttackDensities.Count(), 0),
            AvgPitchesPlayed = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), 0),
            Entropy = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), 0),
            Reset = gameState.IsInitState,
            RequestedEventCount = 30
        };

        return controlData;
    }

    private static ControlData ConvertForPlayingStage(GameState gameState)
    {
        var troubleFactor = gameState.EnemiesCount - (gameState.DefendersCount - gameState.SunflowersCount);
        var levelProgess = gameState.LevelProgess;
        var enemiesCount = gameState.EnemiesCount;

        var nervousness = Mathf.Max(gameState.AverageMouseSpeed * 2f, gameState.AverageMouseClicks / 2f);

        var controlData = new ControlData
        {
            Mode = troubleFactor switch
            {
                < -1 => Vector.OneHot(Controls.Modes.Count, 0),
                > 1 => Vector.OneHot(Controls.Modes.Count, 1),
                _ => Vector.EqualDistribution(Controls.Modes.Count)
            },
            AttackDensity = Vector.OneHot(Controls.AttackDensities.Count(), CalculateBin(levelProgess, 1f, Controls.AttackDensities.Count())),
            AvgPitchesPlayed = Vector.OneHot(Controls.AvgPitchesPlayed.Count(), CalculateBin(enemiesCount, 6, Controls.AvgPitchesPlayed.Count())),
            Entropy = Vector.OneHot(Controls.Entropies.Count(), CalculateBin(nervousness, 1f, Controls.Entropies.Count())),
            Reset = gameState.IsInitState,
            RequestedEventCount = 50
        };

        return controlData;
    }

    private static int CalculateBin(float value, float valueForMaxBin, int binCount)
    {
        var valuePerBin = valueForMaxBin / (binCount - 1);
        var estimatedBin = (int)(value / valuePerBin);
        return Mathf.Clamp(estimatedBin, 0, binCount - 1);
    }

    private static int CalculateBin(int value, int valueForMaxBin, int binCount)
    {
        var valuePerBin = (float)valueForMaxBin / (binCount - 1);
        var estimatedBin = (int)(value / valuePerBin);
        return estimatedBin < binCount ? estimatedBin : binCount - 1;
    }
}
