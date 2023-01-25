using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongButton : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite songOn;
    [SerializeField] private Sprite songOff;

    public void SwitchSong()
    {
        if(image.sprite == songOn)
        {
            image.sprite = songOff; 
        }
        else
        {
            image.sprite = songOn;
        }
    }
}
