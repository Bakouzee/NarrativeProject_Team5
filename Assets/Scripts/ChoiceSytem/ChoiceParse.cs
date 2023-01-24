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
                Debug.Log(data_values.ToString());

                for(int i = 1; i <= 4; i++)
                {
                    var second_split = data_values[i].Split('|');
                    choice.buttonFR.Add(second_split[0]);
                    choice.textFR.Add(second_split[1]);
                }

                for (int i = 5; i <= 8; i++)
                {
                    var second_split = data_values[i].Split('|');
                    choice.buttonEN.Add(second_split[0]);
                    choice.textEN.Add(second_split[1]);
                }

                for (int i = 9; i <= 13; i++)
                {
                    choice.sheetNumber.Add(data_values[i]);
                }

                data_String = strReader.ReadLine();

                choiceDatabase.choices.Add(choice);
            }

            endOfFile = false;
        }
    }

}
