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
        [SerializeField] private TextMeshProUGUI _dialogueTxt;
        [SerializeField] private List<TextMeshProUGUI> _charactersNames;
        [SerializeField] private List<Image> _charactersImg;
        [SerializeField] private float _dialogueSpeed = 0.1f;

        private DialogueManager dialogueMana;
        private DialogueData _dataToRead;
        private List<string> _dialoguesToRead = new();
        private string _choices;
        private bool _isDialogueDone;
        private Coroutine _readCoroutine;

        #region Properties
        public DialogueData GetDialogueData => _dataToRead;
        public TextMeshProUGUI GetDialogueTxt => _dialogueTxt;
        public List<string> GetDialoguesToRead => _dialoguesToRead;
        public string GetChoices => _choices;
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
            if (_readCoroutine != null)
            {
                _dialogueTxt.text = "";
                _dialogueTxt.text = _dialoguesToRead[_dataToRead.indexDialogue];
                _dataToRead.indexDialogue++;
                StopCoroutine(_readCoroutine);
                _readCoroutine = null;
                return;
            }

            if (_dataToRead.indexDialogue >= _dialoguesToRead.Count)
            {
                // Show Choices
                ShowDialogue(false);
                return;
            }

            _dataToRead.indexDialogue++;
            ReadSentence(_dataToRead);
        }

        private void ReadSentence(DialogueData dialogueData)
        {
            _charactersNames[0].text = dialogueData.speakersName[dialogueData.indexDialogue];
            _charactersImg[0].DOFade(1, 0.3f);
            if(_readCoroutine == null)
            {
                _readCoroutine = StartCoroutine(ReadCharByChar(_dialoguesToRead[dialogueData.indexDialogue], _dialogueSpeed));
            }
        }

        private IEnumerator ReadCharByChar(string sentence, float speed)
        {
            if (sentence == "")
            {
                _dialogueTxt.text = "";
                yield break;
            }

            _dialogueTxt.text = "";
            int i = 0;
            char letter = sentence[i];
            while (i < sentence.Length)
            {
                letter = sentence[i];
                _dialogueTxt.text += letter;
                i++;
                yield return new WaitForSeconds(speed);
            }
            _readCoroutine = null;
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

                _choicesToDisplay[0].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i < _dialogueToDisplay.Count; i++)
                {
                    _dialogueToDisplay[i].SetActive(false);
                }

                _choicesToDisplay[0].transform.parent.gameObject.SetActive(true);
                _choices = _dataToRead.playerChoice;
            }
        }
    }
}