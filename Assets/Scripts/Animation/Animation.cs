namespace TeamFive
{
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
        public float textFadeDuration = 0.3f;
        public float textFadeOut = 0.25f;

        [Header("------Darken color when speaker not speaking------")]
        public Color darkenColor;
        public float darkenDuration;
        public float scaleInDuration;
        public float scaleOutNumber;
        public float scaleOutDuration;

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

        #region Speakers Speaking or not
        public void SpeakerNotSpeaking(Image imageToFadeIn)
        {
            imageToFadeIn.DOColor(darkenColor, darkenDuration);
        }

        public void SpeakerSpeaking(Image imageToFadeIn)
        {
            imageToFadeIn.DOColor(Color.white, darkenDuration);
        }

        public void ScaleIn(string speaker, Image speakerImg)
        {
            switch (speaker)
            {
                case "Medhiv":
                    speakerImg.transform.DOScale(4, scaleInDuration);
                    return;
                case "Diya":
                    speakerImg.transform.DOScale(4, scaleInDuration);
                    return;
                case "Syrdon":
                    speakerImg.transform.DOScale(3.5f, scaleInDuration);
                    return;
            }
        }

        public void ScaleOut(string nonSpeakerName, Image speakerImg)
        {
            switch (nonSpeakerName)
            {
                case "Medhiv":
                    float scaleOutMed = speakerImg.transform.localScale.x - scaleOutNumber;
                    scaleOutMed = Mathf.Clamp(scaleOutMed, 3.9f, 4);
                    speakerImg.transform.DOScale(scaleOutMed, scaleOutDuration);
                    return;
                case "Diya":
                    float scaleOutDiy = speakerImg.transform.localScale.x - scaleOutNumber;
                    scaleOutDiy = Mathf.Clamp(scaleOutDiy, 3.9f, 4);
                    speakerImg.transform.DOScale(scaleOutDiy, scaleOutDuration);
                    return;
                case "Syrdon":
                    float scaleOutSyr = speakerImg.transform.localScale.x - scaleOutNumber;
                    scaleOutSyr = Mathf.Clamp(scaleOutSyr, 3.4f, 3.5f);
                    speakerImg.transform.DOScale(scaleOutSyr, scaleOutDuration);
                    return;
            }
        }
        #endregion

        #region Speakers Comes -> Fade In/Out
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

        #endregion

        #region Text Fade In/Out
        public void TextFadeIn(TextMeshProUGUI speaker)
        {
            speaker.DOFade(1f, textFadeDuration);
        }

        public void TextFadeOut(TextMeshProUGUI speaker)
        {
            speaker.DOFade(textFadeOut, textFadeDuration);
        }
        #endregion

        #region Change characters sprite
        public enum persoName
        {
            Medhiv,
            Diya,
            Syrdon,
        };

        public Sprite ChangeSprite(persoName perso, string spriteName)
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
        #endregion

        public void Vibrate(long strength)
        {
            Vibration.Init();
            Vibration.Vibrate(strength);
        }
    }
}