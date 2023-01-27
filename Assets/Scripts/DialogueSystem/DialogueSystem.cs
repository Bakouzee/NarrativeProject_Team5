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
        [Header("----Databases----")]
        [SerializeField] private DialogueDatabase _databaseToRead;
        [SerializeField] private ChoiceDatabase _choiceDatabase;

        [Header("----UI----")]
        [SerializeField] private List<GameObject> _choicesToDisplay;
        [SerializeField] private List<GameObject> _dialogueToDisplay;
        [SerializeField] private TextMeshProUGUI _dialogueTxt;
        [SerializeField] private List<TextMeshProUGUI> _charactersNames;
        [SerializeField] private List<Image> _charactersImg;
        [SerializeField] private float _dialogueSpeed = 0.1f;

        [Header("----First Line----")]
        [SerializeField] private Image _blackScreen;
        [SerializeField] private TextMeshProUGUI _textBlackScreen;
        [SerializeField] private float _startDialogueDuration = 3f;
        [SerializeField] private float _endDialogueDuration = 3f;
        [SerializeField] private string _thanksForPlayingDemo;

        [Header("----Time Goes By----")]
        [SerializeField] private float _timeBlackScreenDuration = 3f;
        [SerializeField] private float _timeTextAppearsDuration = 3f;
        [SerializeField] private float _timeDialogueAppearsDuration = 3f;
        
        private DialogueManager dialogueMana;
        private DialogueData _dataToRead;
        private ScriptableChoice _choicesToRead;
        private List<string> _dialoguesToRead = new();
        private List<int> _endGameSheets = new();
        private string _choices;
        private Coroutine _readCoroutine;

        private bool _isEnding;

        public Animator m_Animator;

        #region Properties
        public DialogueData GetSetDialogueData { get { return _dataToRead; } set { _dataToRead = value; } }
        public TextMeshProUGUI GetDialogueTxt => _dialogueTxt;
        public List<string> GetDialoguesToRead => _dialoguesToRead;
        public string GetChoices => _choices;
        #endregion

        private void Start()
        {
            _endGameSheets.Add(12);
            _endGameSheets.Add(13);
            _endGameSheets.Add(19);
            _endGameSheets.Add(20);

            dialogueMana = DialogueManager.Instance;
            _dataToRead = _databaseToRead.dialogueDatas[0];
            ChangeLanguage(_dataToRead);
            if(DialogueManager.Instance.GetSetCurrentLanguage == DialogueManager.Language.FR)
            {
                StartCoroutine(SceneStart(_dataToRead.dialogueFR[0]));
            } else
            {
                StartCoroutine(SceneStart(_dataToRead.dialogueEN[0]));
            }
        }

        #region Start / End Demo
        private IEnumerator SceneStart(string firstLine)
        {
            // Display first line
            Animation.instance.TextFadeInAll(_textBlackScreen);
            _textBlackScreen.text = firstLine;

            yield return new WaitForSeconds(_startDialogueDuration);

            Animation.instance.TextFadeOutAll(_textBlackScreen);
            StartCoroutine(Animation.instance.FadeOut(_blackScreen));

            _dataToRead.indexDialogue++;
            ReadSentence(_dataToRead);
        }

        private IEnumerator DemoEnd(string endLine)
        {
            // Display first line
            Animation.instance.FadeIN(_blackScreen);

            yield return new WaitForSeconds(1f);

            Animation.instance.TextFadeInAll(_textBlackScreen);
            _textBlackScreen.text = endLine;

            yield return new WaitForSeconds(_endDialogueDuration);

            Animation.instance.TextFadeOutAll(_textBlackScreen);

            yield return new WaitForSeconds(0.5f);

            Animation.instance.TextFadeInAll(_textBlackScreen);
            _textBlackScreen.text = _thanksForPlayingDemo;
        }
        #endregion

        #region Read Sentence
        // Drag and drop in the BTN_Next
        public void NextSentence()
        {
            if (_dataToRead.indexDialogue == _dialoguesToRead.Count - 2 && _isEnding)
            {
                _dataToRead.indexDialogue++;
                string lastLine = _dialoguesToRead[_dataToRead.indexDialogue];

                StartCoroutine(DemoEnd(lastLine));

                return;
            }

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
            for(int i = 0; i < _choicesToDisplay.Count; i++)
            {
                _choicesToDisplay[i].SetActive(false);
            }

            // Check if it's the last sheet to read
            for(int i = 0; i < _endGameSheets.Count; i++)
            {
                if(indexSheet == _endGameSheets[i])
                {
                    _isEnding = true;
                    break;
                }
            }

            _dataToRead = _databaseToRead.dialogueDatas[indexSheet];
            _dataToRead.indexDialogue = -1;
            if (DialogueManager.Instance.GetSetCurrentLanguage == DialogueManager.Language.FR)
            {
                _dialoguesToRead = _dataToRead.dialogueFR;
            }
            else
            {
                _dialoguesToRead = _dataToRead.dialogueEN;
            }
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
                    CharactersInScene(true, idSplit[0], speakerName);
                    CharactersSpeaking(speakerName);
                    NextSentence();
                    return;
                }
                else if (idSplit[1] == "COMEOUT")
                {
                    CharactersInScene(false, idSplit[0], speakerName);
                    //CharactersSpeaking(speakerName);
                    NextSentence();
                    return;
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
                else if(dialogueData.speakersID[index] == "WAIT")
                {
                    _dialogueTxt.text = "";
                    StartCoroutine(TimeGoesBy(_dialoguesToRead[index]));
                }
                else
                {
                    // change sprite character
                    _readCoroutine = StartCoroutine(ReadCharByChar(_dialoguesToRead[index], _dialogueSpeed));
                }
            }
        }

        private IEnumerator ReadCharByChar(string sentence, float speed, bool willBeSkipped = false)
        {
            if (sentence == "")
            {
                _dialogueTxt.text = "";
                yield break;
            }

            AudioManager.instance.Play("SD_Text");

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
            if (willBeSkipped)
            {
                NextSentence();
                yield break;
            }
            AudioManager.instance.Pause("SD_Text");
        }

        private IEnumerator TimeGoesBy(string textToDisplay)
        {
            Animation.instance.FadeIN(_blackScreen);

            yield return new WaitForSeconds(_timeBlackScreenDuration);

            _textBlackScreen.text = textToDisplay;
            Animation.instance.TextFadeInAll(_textBlackScreen);

            yield return new WaitForSeconds(_timeTextAppearsDuration);

            Animation.instance.TextFadeOutAll(_textBlackScreen);

            yield return new WaitForSeconds(_timeBlackScreenDuration);

            StartCoroutine(Animation.instance.FadeOut(_blackScreen));

            yield return new WaitForSeconds(_timeDialogueAppearsDuration);

            NextSentence();
        }
        #endregion

        #region Characters Settings
        private void CharactersInScene(bool comeIn, string characterID = null, string character = null)
        {
            if (characterID == null) return;

            if (comeIn)
            {
                switch (characterID)
                {
                    case "MED":
                        // Display img and name
                        for(int i = 0; i < _charactersImg.Count; i++)
                        {
                            if (!_charactersImg[i].gameObject.activeSelf)
                            {
                                _charactersNames[i].gameObject.SetActive(true);
                                _charactersNames[i].text = character;

                                Animation.instance.FadeIN(_charactersImg[i]);
                                _charactersImg[i].gameObject.tag = character;
                                _charactersImg[i].sprite = Animation.instance.ChangeSprite(Animation.persoName.Medhiv, "MED_CALM");
                                return;
                            }
                        }
                        break;
                    case "DIY":
                        for (int i = 0; i < _charactersImg.Count; i++)
                        {
                            if (!_charactersImg[i].gameObject.activeSelf)
                            {
                                _charactersNames[i].gameObject.SetActive(true);
                                _charactersNames[i].text = character;

                                Animation.instance.FadeIN(_charactersImg[i]);
                                _charactersImg[i].gameObject.tag = character;
                                _charactersImg[i].sprite = Animation.instance.ChangeSprite(Animation.persoName.Diya, "DIY_CALM");
                                return;
                            }
                        }
                        break;
                    case "SYR":
                        for (int i = 0; i < _charactersImg.Count; i++)
                        {
                            if (!_charactersImg[i].gameObject.activeSelf)
                            {
                                _charactersNames[i].gameObject.SetActive(true);
                                _charactersNames[i].text = character;

                                Animation.instance.FadeIN(_charactersImg[i]);
                                _charactersImg[i].gameObject.tag = character;
                                _charactersImg[i].sprite = Animation.instance.ChangeSprite(Animation.persoName.Syrdon, "SYR_CALM");
                                return;
                            }
                        }
                        break;
                    default: return;
                }
            }
            else
            {
                for (int i = 0; i < _charactersImg.Count; i++)
                {
                    switch (characterID)
                    {
                        case "MED":
                            if (_charactersImg[i].gameObject.tag == "Varnas")
                            {
                                Debug.Log(characterID + " comes out.");
                                StartCoroutine(Animation.instance.FadeOut(_charactersImg[i]));
                                _charactersNames[i].gameObject.SetActive(false);
                            }
                            return;
                        case "DIY":
                            if (_charactersImg[i].gameObject.tag == "Diya")
                            {
                                Debug.Log(characterID + " comes out.");

                                StartCoroutine(Animation.instance.FadeOut(_charactersImg[i]));
                                _charactersNames[i].gameObject.SetActive(false);
                            }
                            return;
                        case "SYR":
                            if (_charactersImg[i].gameObject.tag == "Syrdon")
                            {
                                Debug.Log(characterID + " comes out.");

                                StartCoroutine(Animation.instance.FadeOut(_charactersImg[i]));
                                _charactersNames[i].gameObject.SetActive(false);
                            }
                            return;
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
            if(spriteID == "" || spriteID == "PLAYER" || spriteID == "SKIP" || spriteID == "WAIT") return;

            string[] spriteIDSplit = spriteID.Split('_');
            string prefix = spriteIDSplit[0]; // MED, DIY, SYR
            string suffix = spriteIDSplit[1]; // FEELING

            if(suffix == "COMEIN" || suffix == "COMEOUT") return;

            if(suffix == "ANGRY" || suffix == "UPSET")
            {
                Animation.instance.CameraShake();
            }

            switch (prefix)
            {
                case "MED":
                    for(int i = 0; i < _charactersImg.Count; i++)
                    {
                        if (_charactersImg[i].color.a >= 1 && _charactersImg[i].gameObject.tag == "Varnas")
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

        #region Dialogue Settings
        private void ShowDialogue(bool isDialogue)
        {
            if (isDialogue)
            {
                for (int i = 0; i < _dialogueToDisplay.Count; i++)
                {
                    _dialogueToDisplay[i].SetActive(true);
                    m_Animator.SetBool("DoAnim", false);

                }

                for (int i = 0; i < _choicesToDisplay.Count; i++)
                {
                    _choicesToDisplay[i].SetActive(false);
                    m_Animator.SetBool("DoAnim", false);
                }
            }
            else
            {
                for (int i = 0; i < _dialogueToDisplay.Count; i++)
                {
                    _dialogueToDisplay[i].SetActive(false);
                    m_Animator.SetBool("DoAnim", true);
                }

                _choices = _dataToRead.playerChoice;

                for (int i = 0; i < _choicesToDisplay.Count; i++)
                {
                    _choicesToDisplay[i].SetActive(true);
                    TextMeshProUGUI choiceTxt = _choicesToDisplay[i].GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();
                    if (DialogueManager.Instance.GetSetCurrentLanguage == DialogueManager.Language.FR)
                    {
                        choiceTxt.text = _choiceDatabase.choices[int.Parse(_choices) - 1].buttonFR[i];
                    }
                    else
                    {
                        choiceTxt.text = _choiceDatabase.choices[int.Parse(_choices) - 1].buttonEN[i];
                    }
                }
            }
        }
        public void ChangeLanguage(DialogueData dialogueLanguageToUse)
        {
            if(dialogueMana.GetSetCurrentLanguage == DialogueManager.Language.FR)
            {
                _dialoguesToRead = dialogueLanguageToUse.dialogueFR;
                for (int i = 0; i < _choicesToDisplay.Count; i++)
                {
                    if (!_choicesToDisplay[i].activeSelf) return;

                    TextMeshProUGUI choiceTxt = _choicesToDisplay[i].GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();
                    choiceTxt.text = _choiceDatabase.choices[int.Parse(_choices) - 1].buttonFR[i];
                }
            } 
            else if(dialogueMana.GetSetCurrentLanguage == DialogueManager.Language.EN)
            {
                _dialoguesToRead = dialogueLanguageToUse.dialogueEN;
                for (int i = 0; i < _choicesToDisplay.Count; i++)
                {
                    if (!_choicesToDisplay[i].activeSelf) return;
                    
                    TextMeshProUGUI choiceTxt = _choicesToDisplay[i].GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();
                    choiceTxt.text = _choiceDatabase.choices[int.Parse(_choices) - 1].buttonEN[i];
                }
            }
        }
        #endregion
    }
}