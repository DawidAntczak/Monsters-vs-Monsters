using System;
using UnityEngine;

namespace Assets.MUSIC.Experiment
{
    [Serializable]
    public enum MusicTypeOrderGroups
    {
        A,
        B,
        C,
        D,
        E,
        F
    }

    [Serializable]
    public enum MusicClipGroups
    {
        I,
        II
    }

    [Serializable]
    public class TestGroup
    {
        [SerializeField] private MusicTypeOrderGroups _musicTypeOrderGroup;
        [SerializeField] private MusicClipGroups _musicClipGroup;

        public MusicTypeOrderGroups MusicTypeOrderGroup { get => _musicTypeOrderGroup; set => _musicTypeOrderGroup = value; }
        public MusicClipGroups MusicClipGroup { get => _musicClipGroup; set => _musicClipGroup = value; }

        public override bool Equals(object obj)
        {
            return obj is TestGroup group &&
                   _musicTypeOrderGroup == group._musicTypeOrderGroup &&
                   _musicClipGroup == group._musicClipGroup;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_musicTypeOrderGroup, _musicClipGroup);
        }

        public override string ToString()
        {
            return $"{_musicTypeOrderGroup} {_musicClipGroup}";
        }
    }
}
