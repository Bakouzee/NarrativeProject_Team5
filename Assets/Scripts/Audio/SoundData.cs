namespace TeamFive
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    [CreateAssetMenu(fileName = "Database", menuName = "Database/Sound Database")]
    public class SoundData : ScriptableObject
    {
        public List<Sound> dialogueDatas = new();
    }
}
