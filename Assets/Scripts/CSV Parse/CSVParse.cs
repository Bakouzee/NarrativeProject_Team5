using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVParse : MonoBehaviour
{
    private FileStream csvFile;

    private void Start()
    {
        string path = Application.streamingAssetsPath + "/Resources/TestDialogue/TEST IMPORT.csv";

        Debug.Log(path);

        csvFile = new FileStream(path, FileMode.Open, FileAccess.Read);

        //csvFile.
        
    }
}
