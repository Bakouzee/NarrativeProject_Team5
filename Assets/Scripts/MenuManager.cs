namespace TeamFive
{
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.UI;

    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject background;

        [SerializeField] private Image song;
        [SerializeField] private DialogueSystem dialogueSystem;

        [SerializeField] private GameObject buttonSong;
        [SerializeField] private GameObject buttonFR;
        [SerializeField] private GameObject buttonEN;

        [SerializeField] private Sprite songOn;
        [SerializeField] private Sprite songOff;

        private bool isDeploy = false;
        private Coroutine actualCorout;

        public void Deploy()
        {
            if(!isDeploy && actualCorout == null)
            {
                actualCorout = StartCoroutine(deployCorout());
            }
            else if(isDeploy && actualCorout == null)
            {
                actualCorout = StartCoroutine(reployCorout());
            }
        }

        public void ChangeSong()
        {

        }

        public void changeFR()
        {
            DialogueManager.Instance.GetSetCurrentLanguage = DialogueManager.Language.FR;
            dialogueSystem.ChangeLanguage(dialogueSystem.GetSetDialogueData);
            dialogueSystem.GetDialogueTxt.text = dialogueSystem.GetDialoguesToRead[dialogueSystem.GetSetDialogueData.indexDialogue];
        }

        public void changeEN()
        {
            DialogueManager.Instance.GetSetCurrentLanguage = DialogueManager.Language.EN;
            dialogueSystem.ChangeLanguage(dialogueSystem.GetSetDialogueData);
            dialogueSystem.GetDialogueTxt.text = dialogueSystem.GetDialoguesToRead[dialogueSystem.GetSetDialogueData.indexDialogue];
        }

        private IEnumerator deployCorout()
        {
            background.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            buttonSong.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            buttonFR.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            buttonEN.SetActive(true);
            isDeploy = true;
        }
        
        private IEnumerator reployCorout()
        {
            buttonEN.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            buttonFR.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            buttonSong.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            background.SetActive(false);
            isDeploy = false;
        }
    }

}