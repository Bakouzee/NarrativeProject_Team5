namespace TeamFive
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    [CreateAssetMenu(fileName = "Choice database", menuName = "Database/Choice Database")]
    public class ChoiceDatabase : ScriptableObject
    {
        public List<ScriptableChoice> choices = new();

    }

}