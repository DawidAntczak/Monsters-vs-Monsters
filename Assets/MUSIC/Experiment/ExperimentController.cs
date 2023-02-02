using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using MusicInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MUSIC.Experiment
{
    public class ExperimentController : MonoBehaviour
    {
        public static ExperimentController Instance { get; private set; }

        [SerializeField] private TestGroupData _testGroupData;
        [SerializeField] [Range(0, 127)] private int _composedInstrument = 3;

        private Dictionary<string, Action> _levelNameToMusicIndex;

        private MidiFile _composedMusicForStatic;
        private MidiFile _composedMusicForDynamic;

        private OutputDevice _outputDevice;
        private Playback _playback;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            _levelNameToMusicIndex = new()
            {
                { "LVL_001", PlayNoMusic },
                { "LVL_002", PlayNoMusic },
                { "LVL_003", PlayNoMusic },
                { "LVL_101", () => PlayMusicForIndex(0) },
                { "LVL_102", () => PlayMusicForIndex(1) },
                { "LVL_103", () => PlayMusicForIndex(2) },
                { "LVL_104", () => PlayMusicForIndex(3) },
                { "LVL_201", () => PlayMusicForIndex(3) },
            };

            var path = Path.Combine(Application.dataPath, _testGroupData._composedMidiDirPath, _testGroupData._composedMidiForStatic);
            _composedMusicForStatic = MidiFile.Read(Path.Combine(path));
            _composedMusicForStatic.OverrideInstrument(_composedInstrument);

            path = Path.Combine(Application.dataPath, _testGroupData._composedMidiDirPath, _testGroupData._composedMidiForDynamic);
            _composedMusicForDynamic = MidiFile.Read(Path.Combine(path));
            _composedMusicForDynamic.OverrideInstrument(_composedInstrument);

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void PlayMusicForIndex(int index)
        {
            var musicType = _testGroupData.MusicTypeOrder[index];
            switch (musicType)
            {
                case MusicTypes.Composed:
                    PlayComposedStaticMusic();
                    break;
                case MusicTypes.ComposedDynamic:
                    PlayComposedDynamicMusic();
                    break;
                case MusicTypes.Generated:
                    PlayGeneratedMusic();
                    break;
                case MusicTypes.GeneratedStatic:
                    PlayGeneratedStaticMusic();
                    break;
            }
        }

        private void PlayNoMusic()
        {
        }

        private void PlayComposedStaticMusic()
        {
            _outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth");

            _composedMusicForStatic.OverrideVelocity(95);
            _playback = _composedMusicForStatic.GetPlayback(_outputDevice);

            _playback.Loop = true;
            _playback.Start();
        }

        private void PlayComposedDynamicMusic()
        {
            _outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth");

            _composedMusicForDynamic.OverrideVelocity(63);
            _playback = _composedMusicForDynamic.GetPlayback(_outputDevice);
            _playback.Speed = 0.8;

            _playback.Start();
            StartCoroutine(AdjustDynamicMusic(_composedMusicForDynamic, _playback));
        }


        private void PlayGeneratedMusic()
        {
            AutoMusicSystem.Instance.StartPlaying();
        }

        private void PlayGeneratedStaticMusic()
        {
            AutoMusicSystem.Instance.StartPlaying(new StaticGameStateToControlsConverter());
        }

        private IEnumerator AdjustDynamicMusic(MidiFile midiFile, Playback playback)
        {
            while (_playback == playback)
            {
                while (playback.IsRunning)
                {
                    yield return null;
                    continue;
                }
                if (_playback == playback)
                {
                    var progess = GameStateCalculator.Instance.CalculateGameState().LevelProgess;

                    var speed = 0.8 + Mathf.Min(progess, 1f) / 5 * 2;
                    midiFile.OverrideVelocity(63 + (int)Math.Min(64 * progess, 64));

                    playback = midiFile.GetPlayback(_outputDevice);
                    _playback = playback;
                    _playback.Speed = speed;
                    _playback.Start();
                }
            }
        }

        void OnApplicationQuit()
        {
            Stop();
        }

        private void Stop()
        {
            try
            {
                _playback?.Stop();
                _playback?.Dispose();
                _outputDevice?.Dispose();
                _playback = null;
                _outputDevice = null;
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            Debug.Log($"Scene loaded: {arg0.name}. Stopping music.");

            Stop();

            AutoMusicSystem.Instance.StopPlaying();

            if (_levelNameToMusicIndex.TryGetValue(arg0.name, out var action))
            {
                action();
            }
        }
    }
}
