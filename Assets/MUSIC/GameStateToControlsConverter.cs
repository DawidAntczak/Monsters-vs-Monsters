using MusicInterface;
using System;
using System.Linq;
using UnityEngine;

public static class GameStateToControlsConverter
{
    public static ControlData Convert(GameState gameState, float temperature, float requestedTimeLength)
    {
        return gameState.GameStage switch
        {
            GameState.Stage.Preparing => ConvertForPreparingStage(gameState, temperature, requestedTimeLength),
            GameState.Stage.Playing => ConvertForPlayingStage(gameState, temperature, requestedTimeLength),
            GameState.Stage.Won => ConvertForWonStage(gameState, temperature, requestedTimeLength),
            GameState.Stage.Lost => ConvertForLostStage(gameState, temperature, requestedTimeLength),
        };
    }

    private static ControlData ConvertForWonStage(GameState gameState, float temperature, float requestedTimeLength)
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

    private static ControlData ConvertForLostStage(GameState gameState, float temperature, float requestedTimeLength)
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

    private static ControlData ConvertForPreparingStage(GameState gameState, float temperature, float requestedTimeLength)
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

    private static ControlData ConvertForPlayingStage(GameState gameState, float temperature, float requestedTimeLength)
    {
        var troubleFactor = gameState.EnemiesCount - (gameState.DefendersCount - gameState.SunflowersCount);
        var levelProgess = gameState.LevelProgess;
        var enemiesCount = gameState.EnemiesCount;

        var (p, q) = (1, 4);
        var nervousness = Mathf.Max(gameState.AverageMouseSpeed / p, gameState.AverageMouseClicks / q);

        var controlData = new ControlData
        {
            Mode = troubleFactor switch
            {
                >= 0 => Vector.OneHot(Controls.Modes.Count, 0),
                < -1 => Vector.OneHot(Controls.Modes.Count, 1),
                _ => Vector.EqualDistribution(Controls.Modes.Count)
            },
            AttackDensity = Vector.OneHot(Controls.AttackDensities.Count(), CalculateBin(levelProgess, 1f, Controls.AttackDensities.Count())),
            AvgPitchesPlayed = Fuzzy3Vector(CalculateBin(enemiesCount, 6, Controls.AvgPitchesPlayed.Count())),
            Entropy = Fuzzy3Vector(CalculateBin(nervousness, 1f, Controls.Entropies.Count())),
            Reset = gameState.IsInitState,
            Temperature = temperature,
            RequestedTimeLength = requestedTimeLength
        };

        return controlData;
    }

    private static Vector Fuzzy3Vector(int tendencyBin)
    {
        if (tendencyBin == 1)
        {
            return Vector.FromArray(new double[] { 0.25f, 0.5f, 0.25f });
        }
        if (tendencyBin == 0)
        {
            return Vector.FromArray(new double[] { 2/3f, 1/3f, 0f});
        }
        if (tendencyBin == 2)
        {
            return Vector.FromArray(new double[] { 0f, 1/3f, 2/3f });
        }
        throw new ArgumentException();
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
