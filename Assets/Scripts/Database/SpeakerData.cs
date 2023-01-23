namespace TeamFive
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class SpeakerData
    {
        public string speakerID;
        public string speakerName;

        [System.Serializable]
        public struct Status 
        {
            public enum Feels
            {
                ANGRY,
                HAPPY,
                SAD,
            }

            public Feels speakerFeeling;
            public Sprite speakerSprite;
            public AudioClip speakerAudioClip;
        }

        public List<Status> speakerStatuses = new();
    }
}
