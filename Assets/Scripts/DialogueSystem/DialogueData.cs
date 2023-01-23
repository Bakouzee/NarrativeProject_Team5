namespace TeamFive
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class DialogueData
    {
        public int indexDialogue;
        public List<Sprite> speakersID;
        public List<string> speakersName;
        public List<string> dialogueFR;
        public List<string> dialogueEN;
        public List<string> sfx;
        public List<string> vfx;
        public bool playerChoice;
        public float timerChoice;
    }
}

