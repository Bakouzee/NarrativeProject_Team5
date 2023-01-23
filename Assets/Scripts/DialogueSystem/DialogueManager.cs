namespace TeamFive
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private DialogueDatabase _dialogueToRead;
        [SerializeField] private List<GameObject> _btnChoices;

        private void Start()
        {
            
        }
    }
}
