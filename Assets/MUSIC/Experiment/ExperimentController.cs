using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using MusicInterface;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MUSIC.Experiment
{
    public class ExperimentController : MonoBehaviour
    {
        public static ExperimentController Instance { get; private set; }

        private ExperimentConfiguration _experimentConfiguration;
        [SerializeField] [Range(0, 127)] private int _instrument = 3;

        private OutputDevice _outputDevice;
        private Playback _playback;

        private readonly Dictionary<string, MidiFile> _midis = new();

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
        }

        public void StartExperiment()
        {
            try
            {
                try
                {
                    var configJson = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "ExperimentConfiguration.json"));
                    _experimentConfiguration = JsonConvert.DeserializeObject<ExperimentConfiguration>(configJson);
                }
                catch (Exception e)
                {
                    throw new Exception($"Could not load configuration!{Environment.NewLine}{e.Message}");
                }


                foreach (var levelMusicType in _experimentConfiguration.LevelNameToMusicType.Where(x => x.Value == MusicTypes.Composed || x.Value == MusicTypes.ComposedDynamic))
                {
                    _experimentConfiguration.LevelNameToMidiClipName.TryGetValue(levelMusicType.Key, out var midiClipName);
                    if (midiClipName == null)
                        throw new Exception($"No MIDI set for level {levelMusicType.Key}!");
                }

                _midis.Clear();
                foreach (var midiClipName in _experimentConfiguration.LevelNameToMidiClipName.Values.Where(x => !string.IsNullOrEmpty(x)))
                {
                    MidiFile midi;
                    try
                    {
                        midi = LoadMidi(midiClipName);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Could not load MIDI {midiClipName}!{Environment.NewLine}{e.Message}");
                    }
                    _midis.Add(midiClipName, midi);
                }

                SceneManager.sceneLoaded += SceneManager_sceneLoaded;

                SceneManager.LoadScene("LVL_001");
            }
            catch (Exception e)
            {
                ErrorPanel.Instance?.Show(e);
            }
        }

        private void PlayMusicForScene(string sceneName)
        {
            if (!_experimentConfiguration.LevelNameToMusicType.TryGetValue(sceneName, out var musicType))
            {
                return;
            }
            _experimentConfiguration.LevelNameToMidiClipName.TryGetValue(sceneName, out var musicMidiName);

            switch (musicType)
            {
                case MusicTypes.Composed:
                    PlayComposedStaticMusic(_midis[musicMidiName]);
                    break;
                case MusicTypes.ComposedDynamic:
                    PlayComposedDynamicMusic(_midis[musicMidiName]);
                    break;
                case MusicTypes.Generated:
                    PlayGeneratedMusic();
                    break;
                case MusicTypes.GeneratedStatic:
                    PlayGeneratedStaticMusic();
                    break;
            }
        }

        private MidiFile LoadMidi(string name)
        {
            var path = Path.Combine(Application.streamingAssetsPath, name);
            var midi = MidiFile.Read(Path.Combine(path));
            midi.OverrideInstrument(_instrument);
            return midi;
        }

        private void PlayComposedStaticMusic(MidiFile midi)
        {
            _outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth");

            midi.OverrideVelocity(95);
            _playback = midi.GetPlayback(_outputDevice);

            _playback.Loop = true;
            _playback.Start();
        }

        private void PlayComposedDynamicMusic(MidiFile midi)
        {
            _outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth");

            midi.OverrideVelocity(63);
            _playback = midi.GetPlayback(_outputDevice);
            _playback.Speed = 0.8;

            _playback.Start();
            StartCoroutine(AdjustDynamicMusic(midi, _playback));
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

            PlayMusicForScene(arg0.name);
        }
    }
}
