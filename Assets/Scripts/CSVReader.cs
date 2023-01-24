namespace TeamFive
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using Unity.VisualScripting;
    using UnityEngine;

    public class CSVReader : MonoBehaviour
    {
        private string filePath;
        bool endOfFile = false;


        [SerializeField] private DialogueDatabase dialogueDatabase;

        [SerializeField] private int nbFile;
        [SerializeField] private int indexLine;

        private string data_String;

        private void Start()
        {
            dialogueDatabase.dialogueDatas.Clear();
            ReadAllSheets(nbFile);
        }

        public void ReadAllSheets(int nbFile)
        {

            for (int i = 1; i <= nbFile; i++)
            {
                DialogueData data = new DialogueData();
                filePath = "Assets/Ressources/Sheet_Dialogue/Feuille_" + i + ".csv";
                ReadSheet(data);
            }
        }

        #region ReadSheet
        private void ReadSheet(DialogueData data)
        {
            StreamReader strReader = new StreamReader(filePath);

            for (int i = 0; i <= indexLine; i++)
            {
                data_String = strReader.ReadLine();
            }

            while (!endOfFile) // Tant qu'il y a une ligne non vide
            {
                // Stocke les valeurs 
                var data_values = data_String.Split(';');

                if (data_values[0] == "")
                {
                    endOfFile = true;
                    break;
                }

                data.speakersID.Add(data_values[0]);
                data.speakersName.Add(data_values[1]);
                data.dialogueFR.Add(data_values[2]);
                data.dialogueEN.Add(data_values[3]);
                data.sfx.Add(data_values[4]);
                data.vfx.Add(data_values[5]);

                if (data_values[6] != "")
                {
                    data.playerChoice = data_values[6];
                }
                if (data_values[7] != "")
                {
                    data.timerChoice = (float) Convert.ToDouble(data_values[7]);
                }

                data_String = strReader.ReadLine();

            }
            Debug.Log("Data set");
            dialogueDatabase.dialogueDatas.Add(data);
            endOfFile = false;
        }
        #endregion
    }
}



