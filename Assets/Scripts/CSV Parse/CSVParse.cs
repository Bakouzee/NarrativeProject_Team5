namespace TeamFive
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class CSVParse : MonoBehaviour
    {
        [SerializeField] private SpeakerDatabase speakerDatabase;
        //[SerializeField] private Dialogue;

        private void Start()
        {
            string path = Application.dataPath + "/Resources/TestDialogue/TEST_IMPORT.csv";
            Debug.Log(path);

            string[] csvFile = File.ReadAllLines(path);

            // Start at line 2
            for(int i = 1; i < csvFile.Length; i++)
            {
                string line = csvFile[i];
                string[] columns = line.Split(";");
                for(int j = 0; j < columns.Length; j++)
                {
                    // get prefix && suffixe in ID
                    string prefix = columns[0].Split('_')[0]; // ANT
                    string suffix = columns[0].Split('_')[1]; // SAD
                    Debug.Log("Character : " + prefix);
                    Debug.Log("Feeling : " + suffix);

                    // get the id to change audio && sprite
                    for (int k = 0; k < speakerDatabase.speakerData.Count; k++)
                    {
                        SpeakerData speaker = speakerDatabase.speakerData[k];
                        if(speaker.speakerID == columns[0])
                        {
                            switch (suffix)
                            {
                                case "SAD":

                                    break;
                                case "HAPPY":

                                    break;
                                case "ANGRY":

                                    break;
                                default: break;
                            }
                        }
                    }
                }
            }
        }
    }
}

