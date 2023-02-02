using MusicInterface;
using System.Linq;

namespace Assets.MUSIC
{
    public class StaticGameStateToControlsConverter : GameStateToControlsConverter
    {
        public StaticGameStateToControlsConverter() : base(false)
        {
        }

        public override ControlData Convert(GameState gameState, float temperature, float requestedTimeLength)
        {
            return new ControlData
            {
                Mode = Vector.EqualDistribution(Controls.Modes.Count),//Vector.OneHot(Controls.Modes.Count, 0),
                AttackDensity = Vector.EqualDistribution(Controls.AttackDensities.Count()),
                AvgPitchesPlayed = Vector.EqualDistribution(Controls.AvgPitchesPlayed.Count()),
                Entropy = Vector.EqualDistribution(Controls.Entropies.Count()),
                Reset = gameState.IsInitState,
                Temperature = temperature,
                RequestedTimeLength = requestedTimeLength
            };
        }
    }
}
