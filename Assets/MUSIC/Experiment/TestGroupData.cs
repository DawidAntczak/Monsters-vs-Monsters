using System;
using UnityEngine;

namespace Assets.MUSIC.Experiment
{
    [Serializable]
    public class TestGroupData
    {
        public MusicTypes[] _musicTypeOrder;
        public AudioClip _composedMusicClip;

        public MusicTypes[] MusicTypeOrder { get => _musicTypeOrder; set => _musicTypeOrder = value; }
        public AudioClip ComposedMusicClip { get => _composedMusicClip; set => _composedMusicClip = value; }
    }
}
