using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sound {
    public class SoundManager : MonoBehaviour {
        [SerializeField] AudioSource musicSource = null;
        [SerializeField] AudioSource jingleSource = null;
        [SerializeField] AudioSource [] seSources = null;

        static SoundManager s_instance = null;

        public static SoundManager Instance {
            get { return s_instance; }
        }

        [SerializeField] private AudioClip [] bgmList = null;
        [SerializeField] private AudioClip [] jingleList = null;
        [SerializeField] private AudioClip [] seList = null;

        double fadeTime = 1.0;
        double fadeDeltaTime = 0;
        float aimVolume = 0.0f;
        float baseVolume = 1.0f;
        bool isFadeLeap = false;
        int idxSESource = 0;

        // Use this for initialization
        void Awake ()
        {
            if (s_instance == null) {
                s_instance = this;
                GameObject.DontDestroyOnLoad (gameObject);
                musicSource.clip = null;
                foreach (var item in seSources) {
                    item.clip = null;
                }
            } else if (s_instance != this) {
                Destroy (gameObject);
                return;
            }
            idxSESource = 0;
            fadeDeltaTime = 0.0;
            //			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        /// <summary>
        /// ゲームステートによってBGMを変えたい場合...
        /// </summary>
        void Update ()
        {
            if (isFadeLeap == true) {
                fadeDeltaTime += Time.deltaTime;
                if (fadeTime > 0.0f) {
                    if (aimVolume > musicSource.volume) {
                        // Fade In
                        musicSource.volume = (float)(fadeDeltaTime / fadeTime) * baseVolume;
                    } else {
                        // Fade Out
                        musicSource.volume = (float)(1.0f - fadeDeltaTime / fadeTime) * baseVolume;
                    }
                }
                if (fadeDeltaTime >= fadeTime || fadeTime == 0.0) {
                    isFadeLeap = false;
                    fadeDeltaTime = 0.0;
                    musicSource.volume = aimVolume;
                    if (aimVolume == 0.0f) {
                        musicSource.Stop ();
                    }
                }
            }
        }

        public void FadeInBGM (float time = 1.0f)
        {
            fadeTime = time;
            aimVolume = 1.0f;
            musicSource.volume = 0.0f;
            isFadeLeap = true;
        }

        public void FadeOutBGM (float time = 1.0f)
        {
            fadeTime = time;
            aimVolume = 0.0f;
            isFadeLeap = true;
        }

        public void PlayBGM (SoundList.BGM bgmNo, bool isfadein = false)
        {
            if (musicSource.isPlaying == false || musicSource.clip != bgmList [(int)bgmNo]) {
                musicSource.clip = bgmList [(int)bgmNo];
                musicSource.loop = true;
                musicSource.Play ();
                if (isfadein == true) {
                    FadeInBGM ();
                } else {
                    musicSource.volume = 1.0f;
                }
            }
        }

        public void PlayJingle (SoundList.JINGLE no)
        {
            if (jingleSource.isPlaying == false || jingleSource.clip != jingleList [(int)no]) {
                jingleSource.clip = jingleList [(int)no];
                jingleSource.loop = false;
                jingleSource.volume = 1.0f;
                jingleSource.Play ();
            }
        }

        public void PlaySE (SoundList.SE seNo)
        {
            if (++idxSESource >= seSources.Length) {
                idxSESource = 0;
            }
            var curItem = seSources [idxSESource];
            curItem.clip = seList [(int)seNo];
            curItem.Play ();
        }
    }

}
