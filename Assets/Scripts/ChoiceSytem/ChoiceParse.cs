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
        private ScriptableChoice choice;

        private string filePath;

        [SerializeField] private int indexLine;

        // Start is called before the first frame update
        void Awake()
        {
            choiceDatabase.choices.Clear();
            ReadChoice();
        }

        private void ReadChoice()
        {

            TextAsset fileAsset = (TextAsset)Resources.Load("Sheet_Dialogue/Choix", typeof(TextAsset));
            filePath = fileAsset.text;

            // Stocke les valeurs 
            var data_values = filePath.Split(';');//15

            choice = new ScriptableChoice();
            for(int i = 14; i < data_values.Length; i++)
            {
                string value = data_values[i];

                if (value.Length < 1) continue;

                switch (i % 14)
                {
                    case 0:
                        if (value.Length < 1)
                        {
                            choiceDatabase.choices.Add(choice);
                            return;
                        }
                        break;
                    case 1:
                        choice.buttonFR.Add(value);
                        break;
                    case 2:
                        choice.buttonFR.Add(value);
                        break;
                    case 6:
                        choice.buttonEN.Add(value);
                        break;
                    case 7:
                        choice.buttonEN.Add(value);
                        break;
                    case 10:
                        choice.sheetNumber.Add(value);
                        break;
                    case 11:
                        choice.sheetNumber.Add(value);
                        choiceDatabase.choices.Add(choice);
                        choice = new();
                        break;
                    default: break;
                }
            }
        }
    }
}
