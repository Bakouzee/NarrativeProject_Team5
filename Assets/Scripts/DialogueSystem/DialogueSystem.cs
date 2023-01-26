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
        [SerializeField] private ChoiceDatabase _choiceDatabase;
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
        public DialogueData GetSetDialogueData { get { return _dataToRead; } set { _dataToRead = value; } }
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
            if(dialogueMana.GetSetCurrentLanguage == DialogueManager.Language.FR)
            {
                _dialoguesToRead = dialogueLanguageToUse.dialogueFR;
            } 
            else if(dialogueMana.GetSetCurrentLanguage == DialogueManager.Language.EN)
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
                StopCoroutine(_readCoroutine);
                _readCoroutine = null;
                AudioManager.instance.Pause("SD_Text");
                return;
            }

            _dataToRead.indexDialogue++;
            if (_dataToRead.indexDialogue >= _dialoguesToRead.Count)
            {
                // Show Choices
                ShowDialogue(false);
                return;
            }

            ReadSentence(_dataToRead);
        }

        public void NextSheet(int indexSheet)
        {
            //Debug.Log("index sheet : " + indexSheet);
            _dataToRead = _databaseToRead.dialogueDatas[indexSheet];
            _dataToRead.indexDialogue = -1;
            _dialoguesToRead = _dataToRead.dialogueFR;
            ShowDialogue(true);

            NextSentence();
        }

        private void ReadSentence(DialogueData dialogueData)
        {
            int index = dialogueData.indexDialogue;

            string speakerName = dialogueData.speakersName[index];

            string id = dialogueData.speakersID[index];
            string[] idSplit = id.Split('_');

            // Character comes in scene
            if(idSplit.Length > 1)
            {
                if (idSplit[1] == "COMEIN")
                {
                    Debug.Log(speakerName + " comes in");
                    CharactersInScene(true, speakerName);
                } 
                else if (idSplit[1] == "COMEOUT")
                {
                    Debug.Log(speakerName + " comes out");
                    CharactersInScene(false, speakerName);
                }
            }

            // Show speaker's speaking or not
            CharactersSpeaking(speakerName);

            // Change sprite about character's feeling
            CharactersFeeling(id);

            if (_readCoroutine == null)
            {
                if(dialogueData.speakersID[index] == "SKIP")
                {
                    _readCoroutine = StartCoroutine(ReadCharByChar(_dialoguesToRead[index], _dialogueSpeed, true));
                }
                else
                {
                    // change sprite character
                    _readCoroutine = StartCoroutine(ReadCharByChar(_dialoguesToRead[index], _dialogueSpeed));
                }
                AudioManager.instance.Play("SD_Text");
            }
        }

        private IEnumerator ReadCharByChar(string sentence, float speed, bool willBeSkipped = false)
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

            if (willBeSkipped)
            {
                _readCoroutine = null;
                NextSentence();
                yield break;
            }
            AudioManager.instance.Pause("SD_Text");
        }
        #endregion

        #region Characters Settings
        private void CharactersInScene(bool comeIn, string character = null)
        {
            if (character == null || character == "Player")
            {
                for (int i = 0; i < _charactersImg.Count; i++)
                {
                    _charactersImg[i].gameObject.SetActive(false);
                }
                return;
            }

            if (comeIn)
            {
                switch (character)
                {
                    case "Medhiv":
                        // Display img
                        for(int i = 0; i < _charactersImg.Count; i++)
                        {
                            if (!_charactersImg[i].gameObject.activeSelf)
                            {
                                _charactersNames[i].gameObject.SetActive(true);
                                Animation.instance.FadeIN(_charactersImg[i]);
                                _charactersImg[i].gameObject.tag = character;
                                _charactersImg[i].sprite = Animation.instance.ChangeSprite(Animation.persoName.Medhiv, "MED_CALM");
                                return;
                            }
                        }
                        break;
                    case "Diya":
                        for (int i = 0; i < _charactersImg.Count; i++)
                        {
                            if (!_charactersImg[i].gameObject.activeSelf)
                            {
                                _charactersNames[i].gameObject.SetActive(true);
                                Animation.instance.FadeIN(_charactersImg[i]);
                                _charactersImg[i].gameObject.tag = character;
                                _charactersImg[i].sprite = Animation.instance.ChangeSprite(Animation.persoName.Medhiv, "DIY_CALM");
                                return;
                            }
                        }
                        break;
                    case "Syrdon":
                        for (int i = 0; i < _charactersImg.Count; i++)
                        {
                            if (!_charactersImg[i].gameObject.activeSelf)
                            {
                                _charactersNames[i].gameObject.SetActive(true);
                                Animation.instance.FadeIN(_charactersImg[i]);
                                _charactersImg[i].gameObject.tag = character;
                                _charactersImg[i].sprite = Animation.instance.ChangeSprite(Animation.persoName.Syrdon, "Nain.neutre");
                                return;
                            }
                        }
                        break;
                    default: break;
                }

            }
            else
            {
                for (int i = 0; i < _charactersImg.Count; i++)
                {
                    if (_charactersImg[i].gameObject.tag == character)
                    {
                        StartCoroutine(Animation.instance.FadeOut(_charactersImg[i]));
                        _charactersNames[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        private void CharactersSpeaking(string speakerName)
        {
            // show character speaking img
            for (int i = 0; i < _charactersImg.Count; i++)
            {
                if (_charactersImg[i].gameObject.activeSelf)
                {
                    if (_charactersImg[i].gameObject.tag == "Untagged" || _charactersImg[i].gameObject.tag == speakerName)
                    {
                        _charactersImg[i].gameObject.tag = speakerName;
                        Animation.instance.SpeakerSpeaking(_charactersImg[i]);
                        Animation.instance.ScaleIn(speakerName, _charactersImg[i]);

                        _charactersNames[i].text = speakerName;
                        Animation.instance.TextFadeIn(_charactersNames[i]);
                    }
                    else
                    {
                        Animation.instance.SpeakerNotSpeaking(_charactersImg[i]);
                        Animation.instance.ScaleOut(_charactersImg[i].gameObject.tag, _charactersImg[i]);
                        Animation.instance.TextFadeOut(_charactersNames[i]);
                    }
                }
            }
        }

        private void CharactersFeeling(string spriteID)
        {
            // Guarding Case
            if(spriteID == "" || spriteID == "PLAYER" || spriteID == "SKIP") return;

            string[] spriteIDSplit = spriteID.Split('_');
            string prefix = spriteIDSplit[0]; // MED, DIY, SYR
            string suffix = spriteIDSplit[1]; // FEELING

            if(suffix == "COMEIN" || suffix == "COMEOUT") return;

            switch (prefix)
            {
                case "MED":
                    for(int i = 0; i < _charactersImg.Count; i++)
                    {
                        if (_charactersImg[i].color.a >= 1 && _charactersImg[i].gameObject.tag == "Medhiv")
                        {
                            Debug.Log("Feeling Change for " + prefix + " : " + suffix);
                            _charactersImg[i].sprite = Animation.instance.ChangeSprite(Animation.persoName.Medhiv, spriteID);
                            return;
                        }
                    }
                    return;
                case "DIY":
                    for (int i = 0; i < _charactersImg.Count; i++)
                    {
                        if (_charactersImg[i].color.a >= 1 && _charactersImg[i].gameObject.tag == "Diya")
                        {
                            Debug.Log("Feeling Change for " + prefix + " : " + suffix);
                            _charactersImg[i].sprite = Animation.instance.ChangeSprite(Animation.persoName.Diya, spriteID);
                            return;
                        }
                    }
                    return;
                case "SYR":
                    for (int i = 0; i < _charactersImg.Count; i++)
                    {
                        if (_charactersImg[i].color.a >= 1 && _charactersImg[i].gameObject.tag == "Syrdon")
                        {
                            Debug.Log("Feeling Change for " + prefix + " : " + suffix);
                            _charactersImg[i].sprite = Animation.instance.ChangeSprite(Animation.persoName.Syrdon, spriteID);
                            return;
                        }
                    }
                    return;
            }
        }

        #endregion
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

                for(int i = 0; i < _choicesToDisplay.Count; i++)
                {
                    TextMeshProUGUI choiceTxt = _choicesToDisplay[i].GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();
                    choiceTxt.text = _choiceDatabase.choices[int.Parse(_choices) - 1].buttonFR[i];
                }
            }
        }
    }
}