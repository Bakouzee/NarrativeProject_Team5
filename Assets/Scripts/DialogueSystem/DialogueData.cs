namespace TeamFive
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class DialogueData
    {
        public int indexDialogue;
        public List<string> speakersID = new();
        public List<string> speakersName = new();
        public List<string> dialogueFR = new();
        public List<string> dialogueEN = new();
        public List<string> sfx = new();
        public List<string> vfx = new();
        public string playerChoice;
        public float timerChoice;
    }
}

