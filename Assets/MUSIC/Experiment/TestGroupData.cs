using System;

namespace Assets.MUSIC.Experiment
{
    [Serializable]
    public class TestGroupData
    {
        public MusicTypes[] _musicTypeOrder;
        public string _composedMidiDirPath;
        public string _composedMidiForStatic;
        public string _composedMidiForDynamic;

        public MusicTypes[] MusicTypeOrder { get => _musicTypeOrder; set => _musicTypeOrder = value; }
        public string ComposedMidiDirPath { get => _composedMidiDirPath; set => _composedMidiDirPath = value; }
        public string ComposedMidiForStatic { get => _composedMidiForStatic; set => _composedMidiForStatic = value; }
        public string ComposedMidiForDynamic { get => _composedMidiForDynamic; set => _composedMidiForDynamic = value; }
    }
}
