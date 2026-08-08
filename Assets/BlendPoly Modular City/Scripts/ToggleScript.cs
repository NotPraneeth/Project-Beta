using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blendpoly.Character;

namespace Blendpoly.ToggleScript
{
    public class ToggleScript : MonoBehaviour
    {
        [SerializeField] private GameObject onButton;
        [SerializeField] private GameObject offButton;
        [SerializeField] private GameObject Male_BodyPart, Female_BodyPart;

        [Header("Gender Check")]
        [SerializeField] private CharacterCustomization characterCustom;

        private int toggleIndex;

        void Start()
        {
            toggleIndex = PlayerPrefs.GetInt("ToggleState", 0);
        }

        private GameObject GetCurrentBodyPart()
        {
            if (characterCustom == null) return Male_BodyPart;
            return characterCustom.IsMale() ? Male_BodyPart : Female_BodyPart;
        }

        public void ToggleOn()
        {
            GameObject bodyPart = GetCurrentBodyPart();
            if (bodyPart == null) return;

            toggleIndex = 1;
            onButton.SetActive(true);
            offButton.SetActive(false);
            bodyPart.SetActive(true);

            PlayerPrefs.SetInt("ToggleState", toggleIndex);
        }

        public void ToggleOff()
        {
            GameObject bodyPart = GetCurrentBodyPart();
            if (bodyPart == null) return;

            toggleIndex = 0;
            onButton.SetActive(false);
            offButton.SetActive(true);
            bodyPart.SetActive(false);

            PlayerPrefs.SetInt("ToggleState", toggleIndex);
        }
    }
}