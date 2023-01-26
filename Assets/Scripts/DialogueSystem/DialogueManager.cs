namespace TeamFive
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public delegate void OnPlayerChoiceDelegate();
    public class DialogueManager : MonoBehaviour
    {
        #region Singleton
        public static DialogueManager Instance;
        private void Awake()
        {
            if(Instance == null) Instance = this;

            _currentLanguage = Language.FR;

        }
        #endregion

        public enum Language
        {
            FR,
            EN,
        }

        public OnPlayerChoiceDelegate OnPlayerChoice;

        [SerializeField] private Language _currentLanguage;
        [SerializeField] private DialogueSystem dialogueSystem;

        #region Properties
        public Language GetSetCurrentLanguage {  get { return _currentLanguage; } set { _currentLanguage = value; } }
        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _currentLanguage = Language.EN;
                dialogueSystem.ChangeLanguage(dialogueSystem.GetSetDialogueData);
                dialogueSystem.GetDialogueTxt.text = dialogueSystem.GetDialoguesToRead[dialogueSystem.GetSetDialogueData.indexDialogue];
            }
        }
    }
}
