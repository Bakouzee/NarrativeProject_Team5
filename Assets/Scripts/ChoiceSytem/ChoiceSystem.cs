using System.Collections;
using System.Collections.Generic;
using TeamFive;
using UnityEngine;

public class ChoiceSystem : MonoBehaviour
{
    [SerializeField] private ChoiceDatabase _choiceDatabase;
    private int _indexSheet;

    public void ButtonChoice(int indexButton)
    {
        /*switch (indexButton) 
        {
            case 1:
                NextSheet();
        }*/
    }

    //public void NextSheet(int indexSheet, bool isFollowingSheet)
    //private void NextSheet(int indexSheet)
    //{
    //    _indexSheet = indexSheet - 1;
    //    if (_indexSheet < _databaseToRead.dialogueDatas.Count)
    //    {
    //        _dataToRead = _databaseToRead.dialogueDatas[_indexSheet];
    //    }
    //    ReadSentence(_dataToRead);
    //}
}
