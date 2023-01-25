namespace TeamFive
{
    using System.Collections;
    using System;
    using UnityEngine;
    using DG.Tweening;
    using JetBrains.Annotations;
    using UnityEngine.Rendering;

    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;

        public static AudioManager instance;

        bool resetSound = false;


        [SerializeField] private string currentSound;
        public string CurrentSound { get => currentSound; private set => currentSound = value; }


        bool bool1 = false;
        bool bool2 = false;
        bool bool3 = false;
        bool bool4 = false;        

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
 
        private void Update()
        {
            if (bool1 && bool2 && bool3 && bool4)
            {
                bool1 = false;
                bool2 = false;
                bool3 = false;
                bool4 = false;
            }
        }

        private void Start()
        {
            StartCoroutine(MusicRandom());
        }


        public IEnumerator MusicRandom()
        {
            string musique1 = "SD_Musique01";
            string musique2 = "SD_Musique02";
            string musique3 = "SD_Musique03";
            string musique4 = "SD_Musique04";

            float duration1 = 207f;
            float duration2 = 158f;
            float duration3 = 148f;
            float duration4 = 228f;

            int xcount = UnityEngine.Random.Range(0, 4);
            
            if (xcount == 0 && bool1 == false)
            {
                Play(musique1);
                currentSound = musique1;
                bool1 = true;
                yield return new WaitForSeconds(duration1);
                StopFade();

            }
            if (xcount == 1 && bool2 == false)
            {
                Play(musique2);
                currentSound = musique2;
                bool2 = true;
                yield return new WaitForSeconds(duration2);
                StopFade();


            }
            if (xcount == 2 && bool3 == false)
            {
                Play(musique3);
                currentSound = musique3;
                bool3 = true;
                yield return new WaitForSeconds(duration3);
                StopFade();


            }
            if (xcount == 3 && bool4 == false)
            {
                Play(musique4);
                currentSound = musique4;
                bool4 = true;
                yield return new WaitForSeconds(duration4);
                StopFade();

            }
            StartCoroutine(MusicRandom());


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