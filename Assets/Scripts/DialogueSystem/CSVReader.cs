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

        [SerializeField] private DialogueDatabase dialogueDatabase;

        [SerializeField] private int nbFile;
        [SerializeField] private int indexLine;

        private void Awake()
        {
            dialogueDatabase.dialogueDatas.Clear();
            ReadAllSheets(nbFile);
        }

        public void ReadAllSheets(int nbFile)
        {
            TextAsset fileAsset = null;
            for (int i = 1; i <= nbFile; i++)
            {
                if (i == 15)
                {
                    DialogueData data = new();
                    dialogueDatabase.dialogueDatas.Add(data);
                    continue;
                }
                fileAsset = (TextAsset)Resources.Load("Sheet_Dialogue/Feuille_" + i, typeof(TextAsset));
                filePath = fileAsset.text;
                ReadSheet(filePath);
                Debug.Log("Data set");
            }
        }

        #region ReadSheet
        private void ReadSheet(string csvFile)
        {
            DialogueData data = new DialogueData();

            var data_values = csvFile.Split(';');

            for(int i = 21; i < data_values.Length; i++) // 21 -> nber of columns
            {
                string value = data_values[i];

                switch (i % 21)
                {
                    case 0:
                        if(value.Length > 2)
                        {
                            if (value.Contains('\n'))
                            {
                                string[] realVal = value.Split('\n');
                                data.speakersID.Add(realVal[1]);
                            }
                            else
                            {
                                data.speakersID.Add(value);
                            }
                        }
                        break;
                    case 1:
                        data.speakersName.Add(value);
                        break;
                    case 2:
                        data.dialogueFR.Add(value);
                        break;
                    case 3:
                        data.dialogueEN.Add(value);
                        break;
                    case 6:
                        if (int.TryParse(value, out int realValue))
                        {
                            Debug.Log("End Sheet int");
                            Debug.Log(realValue);
                            data.playerChoice = realValue.ToString();
                            dialogueDatabase.dialogueDatas.Add(data);
                            return;
                        }
                        else
                        {
                            if(value == "fin n1" || value == "x")
                            {
                                Debug.Log("End Story");
                                dialogueDatabase.dialogueDatas.Add(data);
                                return;
                            }
                            int actualI = i;
                            while(i % 21 != 0)
                            {
                                i++;
                            }
                            if (data_values[i].Length < 3)
                            {
                                Debug.Log(data_values[i]);
                                Debug.Log("End Sheet next");
                                dialogueDatabase.dialogueDatas.Add(data);
                                return;
                            }
                            i = actualI;
                            break;
                        }
                    default: break;
                }
            }
        }
        #endregion
    }
}



