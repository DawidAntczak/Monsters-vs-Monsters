using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MUSIC.Experiment
{
    public class ExperimentController : MonoBehaviour
    {
        public static ExperimentController Instance { get; private set; }

        [SerializeField] private TestGroup _currentTestGroup;
        [SerializeField] private TestGroupsContainer testGroupsContainer;

        private Dictionary<string, Action> _levelNameToMusicIndex;

        private AudioSource _audioSource;

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
                { "LVL_201", PlayGeneratedMusic },
            };
            _audioSource = GetComponent<AudioSource>();

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void PlayMusicForIndex(int index)
        {
            var data = GetTestGroupDataForCurrentUser();
            var musicType = data.MusicTypeOrder[index];
            switch (musicType)
            {
                case MusicTypes.Composed:
                    PlayComposedStaticMusic(data.ComposedMusicClip);
                    break;
                case MusicTypes.ComposedDynamic:
                    PlayComposedDynamicMusic(data.ComposedMusicClip);
                    break;
                case MusicTypes.Generated:
                    PlayGeneratedMusic();
                    break;
            }
        }

        private void PlayNoMusic()
        {
        }

        private void PlayComposedStaticMusic(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.pitch = 1f;
            _audioSource.volume = 0.9f;
            _audioSource.Play();
        }

        private void PlayComposedDynamicMusic(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.pitch = 1f;
            _audioSource.volume = 0.5f;
            _audioSource.Play();
            StartCoroutine(AdjustDynamicMusic());
        }


        private void PlayGeneratedMusic()
        {
            AutoMusicSystem.Instance.StartPlaying();
        }

        private IEnumerator AdjustDynamicMusic()
        {
            yield return new WaitForSeconds(_audioSource.clip.length / _audioSource.pitch);
            while (_audioSource.isPlaying)
            {
                var progess = GameStateCalculator.Instance.CalculateGameState().LevelProgess;
                if (progess <= 0.5f)
                {
                    yield return new WaitForSeconds(_audioSource.clip.length / _audioSource.pitch);
                    continue;
                }

                progess -= 0.5f;
                progess *= 2f;

                _audioSource.pitch = 1f + Mathf.Min(progess, 1f) / 2f;
                _audioSource.volume = 0.5f + Mathf.Min(progess, 1f) / 2;

                var toEndOfClip = _audioSource.clip.length / _audioSource.pitch;
                yield return new WaitForSeconds(toEndOfClip);
            }
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            Debug.Log($"Scene loaded: {arg0.name}. Stopping music.");

            _audioSource.Stop();
            AutoMusicSystem.Instance.StopPlaying();

            if (_levelNameToMusicIndex.TryGetValue(arg0.name, out var action))
            {
                action();
            }
        }

        private TestGroupData GetTestGroupDataForCurrentUser()
        {
            //return testGroupsContainer[_currentTestGroup];
            return testGroupsContainer.First().Value;
        }
    }
}
