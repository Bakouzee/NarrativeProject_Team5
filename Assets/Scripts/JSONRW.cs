using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONRW : MonoBehaviour
{
    string filePath = "Assets/Feuille_2.csv";
    bool endOfFile = false;

    [SerializeField] private int indexLine;

    private string data_String;

    List<string> id = new List<string>();
    List<string> character = new List<string>();
    List<string> fr = new List<string>();
    List<string> en = new List<string>();
    List<string> ja = new List<string>();
    List<string> audio = new List<string>();
    string choix = "";
    string timer = "";

    private void Start()
    {
        SetFilePath("Feuille_2");
        Debug.Log(filePath);
        ReadSheet();
        for(int i = 0; i < id.Count; i++)
            {
            Debug.Log("id numéro : " + i + " = " + id[i]);
            Debug.Log(character[i]);
            Debug.Log(fr[i]);
            Debug.Log(en[i]);
            Debug.Log(ja[i]);
            Debug.Log(audio[i]);
        }
        Debug.Log(choix);
        Debug.Log(timer);

    }

    public void SetFilePath(string feuille)
    {
        filePath = "Assets/" + feuille + ".csv";
    }

    private void ReadSheet()
    {
        StreamReader strReader = new StreamReader(filePath);
        int index = 0;

        for (int i = 0; i <= indexLine; i++)
        {
            data_String = strReader.ReadLine();
        }

        while (!endOfFile) // Tant qu'il y a une ligne non vide
        {

            if (data_String == null)
            {
                endOfFile = true;
                break;
            }

            // Stocke les valeurs 
            var data_values = data_String.Split(';');
            List<string> actualSentence = new List<string>();

            id.Add(data_values[0]);
            character.Add(data_values[1]);
            fr.Add(data_values[2]);
            en.Add(data_values[3]);
            ja.Add(data_values[4]);
            audio.Add(data_values[5]);

            if (data_values[6] != "")
            {
                choix = data_values[6];
                Debug.Log("choix pendant tri = " + choix);
            }
            if (data_values[7] != "")
            {
                timer = data_values[7];
                Debug.Log("timer pendant tri = " + timer);
            }

            data_String = strReader.ReadLine();

            //List<List<string>> stringData = new List<List<string>>();
        }
    }

}


