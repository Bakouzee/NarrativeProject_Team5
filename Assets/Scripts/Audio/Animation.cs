namespace TeamFive
{
    using System;
    using UnityEngine;
    using DG.Tweening;
    using UnityEngine.UI;

    public class Animation : MonoBehaviour
    {

        public GameObject backGround;
        public GameObject perso;

        public Image imageLeft;
        public Image imageMiddle;
        public Image imageRight;
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                CameraShake();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                FadeIN();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                FadeOut();
            }
            
        }

        void CameraShake()
        {
            backGround.transform.DOShakePosition(5f, 8f, 10, 2f, true);
            perso.transform.DOShakePosition(3f, 8f, 10, 2f, true);
        }

        

        void FadeIN()
        {
            imageLeft.DOFade(1f, 5f);
        }

        void FadeOut()
        {
            imageLeft.DOFade(0f, 5f);

        }
    }
}
