namespace TeamFive
{
    using System;
    using UnityEngine;
    using DG.Tweening;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;

    public class Animation : MonoBehaviour
    {
        public static Animation instance;


        [Header("------Shaking scene------")]
        public GameObject backGround;
        public GameObject perso;

        [Header("------CameraShake values------")]
        public float duration;
        public float intensity;
        public int strength;


        [Header("------FadeOut Fade in Time duration------")]
        public float fadeDuration;

        [Header("------Sprite Medhiv------")]
        [SerializeField] private List<Sprite> medhivSprite;
        [Header("------Sprite Diya------")]
        [SerializeField] private List<Sprite> diyaSprite;
        [Header("------Sprite Syrdon------")]
        [SerializeField] private List<Sprite> syrdonSprite;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }


        public void CameraShake()
        {

            backGround.transform.DOShakePosition(duration, intensity, strength, 2f, true);
            perso.transform.DOShakePosition(duration, intensity, strength, 2f, true);
        }



        public void FadeIN(Image imageToFadeIn)
        {
            imageToFadeIn.gameObject.SetActive(true);
            imageToFadeIn.DOFade(1f, fadeDuration);
        }



        public IEnumerator FadeOut(Image imageToFadeOut)
        {
            imageToFadeOut.DOFade(0f, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
            imageToFadeOut.gameObject.SetActive(false);

        }
        public enum persoName
        {
            Medhiv,
            Diya,
            Syrdon,
        };

        public Sprite GetSprite(persoName perso, string spriteName)
        {

            switch (perso)
            {
                case persoName.Medhiv:
                    for (int i = 0; i < medhivSprite.Count; i++)
                    {
                        if (medhivSprite[i].name == spriteName)
                        {
                            return medhivSprite[i];
                        }

                    }
                    break;
                case persoName.Diya:
                    for (int i = 0; i < diyaSprite.Count; i++)
                    {
                        if (diyaSprite[i].name == spriteName)
                        {
                            return diyaSprite[i];
                        }

                    }
                    break;
                case persoName.Syrdon:
                    for (int i = 0; i < syrdonSprite.Count; i++)
                    {
                        if (syrdonSprite[i].name == spriteName)
                        {
                            return syrdonSprite[i];
                        }
                    }
                    break;
            }
            
            Debug.LogWarning("Le nom du sprite n'existe pas : " + spriteName);
            return null;

        }
    }

    
}
