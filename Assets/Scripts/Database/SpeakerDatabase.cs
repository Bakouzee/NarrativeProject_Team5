namespace TeamFive
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SpeakerDatabase", menuName = "Database/Create Speaker Database")]
    public class SpeakerDatabase : ScriptableObject
    {
        public List<SpeakerData> speakerData;
    }
}

