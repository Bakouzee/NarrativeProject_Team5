namespace TeamFive
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    [CreateAssetMenu(fileName = "Database", menuName = "Database/Dialogue Database")]
    public class DialogueDatabase : ScriptableObject
    {
        public List<DialogueData> dialogueDatas = new();
    }
}
