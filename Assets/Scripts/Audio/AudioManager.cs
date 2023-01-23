namespace TeamFive
{
    using System.Collections;
    using System;
    using UnityEngine;
    using DG.Tweening;


    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;

        public static AudioManager instance;

        bool resetSound = false;


        [SerializeField] private string currentSound;
        public string CurrentSound { get => currentSound; private set => currentSound = value; }


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.outputAudioMixerGroup = s.audioMixer;
            }
        }


        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found (surement mal ecrit entre le script et sur Unity)");
                return;
            }
            s.source.Play();
        }
        public void PlayFade(string name)
        {
            StopFade();
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found (surement mal ecrit entre le script et sur Unity)");
                return;
            }
            StartCoroutine(FadeIn(s));
            currentSound = name;
        }
        public void Stop()
        {
            Sound s = Array.Find(sounds, sound => sound.name == currentSound);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + currentSound + " not found (surement mal ecrit entre le script et sur Unity)");
                return;
            }
            s.source.Stop();
        }
        public void StopFade()
        {
            Sound s = Array.Find(sounds, sound => sound.name == currentSound);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + currentSound + " not found (surement mal ecrit entre le script et sur Unity)");
                return;
            }
            StartCoroutine(FadeOut(s));
        }
        public void Pause(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found (surement mal ecrit entre le script et sur Unity)");
                return;
            }
            s.source.Pause();
        }
        public void UnPause(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found (surement mal ecrit entre le script et sur Unity)");
                return;
            }
            s.source.UnPause();

        }

        public IEnumerator FadeOut(Sound s)
        {
            s.source.DOFade(0f, 2f);
            yield return new WaitForSeconds(2f);
            resetSound = true;

            if (resetSound == true)
            {
                s.source.Stop();
                s.source.volume = s.volume;
                Debug.Log("Le son est reset");
            }
            yield return null;
        }
        public IEnumerator FadeIn(Sound s)
        {
            yield return new WaitForSeconds(0.5f);
            s.source.volume = 0f;
            s.source.Play();
            s.source.DOFade(s.volume, 2f);
            yield return null;
        }
    }
}