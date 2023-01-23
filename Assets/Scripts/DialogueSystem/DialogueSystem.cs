namespace TeamFive
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using DG.Tweening;

    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField] private DialogueDatabase _databaseToRead;
        [SerializeField] private List<GameObject> _choicesToDisplay;
        [SerializeField] private List<GameObject> _dialogueToDisplay;
        [SerializeField] private TextMeshProUGUI dialogueTxt;
        [SerializeField] private List<TextMeshProUGUI> _charactersNames;
        [SerializeField] private List<Image> _charactersImg;

        private DialogueManager dialogueMana;
        private DialogueData _dataToRead;
        private List<string> _dialoguesToRead = new();
        private bool _isDialogueDone;

        #region Properties
        public DialogueData GetDialogueData => _dataToRead;
        public TextMeshProUGUI GetDialogueTxt => dialogueTxt;
        public List<string> GetDialoguesToRead => _dialoguesToRead;
        #endregion

        private void Start()
        {
            dialogueMana = DialogueManager.Instance;
            _dataToRead = _databaseToRead.dialogueDatas[0];
            ChangeLanguage(_dataToRead);
            ReadSentence(_dataToRead);
        }

        public void ChangeLanguage(DialogueData dialogueLanguageToUse)
        {
            if(dialogueMana.GetCurrentLanguage == DialogueManager.Language.FR)
            {
                _dialoguesToRead = dialogueLanguageToUse.dialogueFR;
            } 
            else if(dialogueMana.GetCurrentLanguage == DialogueManager.Language.EN)
            {
                _dialoguesToRead = dialogueLanguageToUse.dialogueEN;
            }
        }

        #region Read Sentence
        // Drag and drop in the BTN_Next
        public void NextSentence()
        {
            _dataToRead.indexDialogue++;
            ReadSentence(_dataToRead);
        }

        private void ReadSentence(DialogueData dialogueData)
        {
            if(dialogueData.indexDialogue >= _dialoguesToRead.Count)
            {
                return;
            }

            _charactersNames[0].text = dialogueData.speakersName[dialogueData.indexDialogue];
            _charactersImg[0].DOFade(1, 0.3f);
            dialogueTxt.text = _dialoguesToRead[dialogueData.indexDialogue];
        }
        #endregion

        private void CharactersInScene(int nberChar = 0, List<Sprite> charSprites = null)
        {
            if (nberChar <= 0)
            {
                for (int i = 0; i < _charactersImg.Count; i++)
                {
                    _charactersImg[i].gameObject.SetActive(false);
                }
                return;
            }

            for(int i = 0; i < nberChar; i++)
            {
                _charactersImg[i].gameObject.SetActive(true);
                _charactersImg[i].sprite = charSprites[i];
            }
        }

        private void ShowDialogue(bool isDialogue)
        {
            if (isDialogue)
            {
                for(int i = 0; i < _dialogueToDisplay.Count; i++)
                {
                    _dialogueToDisplay[i].SetActive(true);
                }

                for(int j = 0; j < _choicesToDisplay.Count; j++)
                {
                    _choicesToDisplay[j].SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < _dialogueToDisplay.Count; i++)
                {
                    _dialogueToDisplay[i].SetActive(false);
                }

                for (int j = 0; j < _choicesToDisplay.Count; j++)
                {
                    _choicesToDisplay[j].SetActive(true);
                }
            }
        }
    }
}
