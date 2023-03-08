using System.Collections.Generic;
using Assets.MUSIC.Experiment;

public class ExperimentConfiguration
{
    public Dictionary<string, MusicTypes> LevelNameToMusicType { get; set; }
    public Dictionary<string, string> LevelNameToMidiClipName { get; set; }
}
