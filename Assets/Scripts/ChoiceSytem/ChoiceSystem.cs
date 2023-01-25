namespace TeamFive
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TeamFive;
    using UnityEngine;

    public class ChoiceSystem : MonoBehaviour
    {
        [SerializeField] private ChoiceDatabase _choiceDatabase;
        [SerializeField] private DialogueSystem dialogueSystem;

        private ScriptableChoice _dataToRead;

        private int _indexSheet;

        public void ButtonChoice(int indexButton)
        {
            int choiceNumber = Convert.ToInt32(dialogueSystem.GetChoices);
            Debug.Log("Choice number : " + choiceNumber);
            _dataToRead = _choiceDatabase.choices[choiceNumber - 1];

            switch (indexButton)
            {
                case 1:
                    _indexSheet = int.Parse(_dataToRead.sheetNumber[0]);
                    _indexSheet--;
                    Debug.Log("index sheet : " + _indexSheet);
                    dialogueSystem.NextSheet(_indexSheet);
                    break;
                case 2:
                    _indexSheet = int.Parse(_dataToRead.sheetNumber[1]);
                    _indexSheet--;
                    dialogueSystem.NextSheet(_indexSheet);
                    break;
            }

        }

        //public void NextSheet(int indexSheet, bool isFollowingSheet)

    }

}