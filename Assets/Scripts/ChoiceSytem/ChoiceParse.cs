namespace TeamFive
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class ChoiceParse : MonoBehaviour
    {
        [SerializeField] private ChoiceDatabase choiceDatabase;
        ScriptableChoice choice;

        string filePath = "Assets/Ressources/Sheet_Dialogue/Choix.csv";
        bool endOfFile = false;

        [SerializeField] private int indexLine;

        private string data_String;

        // Start is called before the first frame update
        void Start()
        {
            choiceDatabase.choices.Clear();
            ReadChoice();
        }

        private void ReadChoice()
        {
            StreamReader strReader = new StreamReader(filePath);

            for (int i = 0; i <= indexLine; i++)
            {
                data_String = strReader.ReadLine();
            }

            while (!endOfFile) // Tant qu'il y a une ligne non vide
            {
                choice = new ScriptableChoice();
                if (data_String == null)
                {
                    endOfFile = true;
                    break;
                }

                // Stocke les valeurs 
                var data_values = data_String.Split(';');

                for(int i = 1; i <= 2; i++)
                {
                    choice.buttonFR.Add(data_values[i]);
                }

                for (int i = 6; i <= 7; i++)
                {
                    choice.buttonEN.Add(data_values[i]);
                }

                for (int i = 10; i <= 11; i++)
                {
                    choice.sheetNumber.Add(data_values[i]);
                }

                data_String = strReader.ReadLine();

                Debug.Log("Choice added");
                choiceDatabase.choices.Add(choice);
            }

            endOfFile = false;
        }
    }

}
