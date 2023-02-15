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
                fileAsset = (TextAsset)Resources.Load("Sheet_Dialogue/Feuille_" + i, typeof(TextAsset));
                filePath = fileAsset.text;//"Assets/Resources/Sheet_Dialogue/Feuille_" + i + ".csv";
                ReadSheet(filePath);
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
                            // Debug.Log(value);
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
                            Debug.Log("Choice length : " + value.Length);
                            Debug.Log("Choice : " + value);
                            data.playerChoice = realValue.ToString();
                            dialogueDatabase.dialogueDatas.Add(data);
                            return;
                        }
                        break;
                    default: break;
                }
            }

        }
        #endregion
    }
}



