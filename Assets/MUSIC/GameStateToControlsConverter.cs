using MusicInterface;

public static class GameStateToControlsConverter
{
    public static ControlData Convert(GameState gameState)
    {
        var controlData = new ControlData
        {
            Mode = Vector.FromArray(new double[3]),
            AttackDensity = Vector.FromArray(new double[6]),
            AvgPitchesPlayed = Vector.FromArray(new double[3]),
            Entropy = Vector.FromArray(new double[3]),
            Reset = false,
            RequestedEventCount = 100
        };

        return controlData;
    }
}
