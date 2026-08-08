using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blendpoly.Character
{
    public class CharacterCustomization : MonoBehaviour
    {
        public GameObject Gender_ref;

        [Header("Male References")]
        public GameObject Male_Skin_ref, Male_Clothes_ref, Male_Eyes_ref, Male_Beard_ref,
                          Male_Glasses_ref, Male_Hair_ref, Male_Shoes_ref, Male_Body_ref;

        [Header("Female References")]
        public GameObject Female_Skin_ref, Female_Clothes_ref, Female_Eyes_ref,
                          Female_Glasses_ref, Female_Hair_ref, Female_Shoes_ref, Female_Body_ref;

        public int ClotheSkinIndex = 0;
        public int EyeColorIndex = 0;      // separated
        public int SkinColorIndex = 0;     // separated

        public Material[] ClothSkins;
        public Material[] EyeColor;
        public Material[] SkinColor;

        public bool IsMale()
        {
            int index = GetCurrentActiveAccessory(Gender_ref);
            if (index == -1)
            {
                Debug.LogWarning("No active gender child found. Defaulting to female.");
                return false;
            }
            return index == 0;
        }

        public void ChangeGender(int direction)
        {
            ChangeAccessory(Gender_ref, direction);
            // Reset all indices when gender switches
            ClotheSkinIndex = 0;
            EyeColorIndex = 0;
            SkinColorIndex = 0;
        }

        public void ChangeAccessory(GameObject BodyPart, int direction)
        {
            if (BodyPart == null) return;

            int ActiveChildIndex = GetCurrentActiveAccessory(BodyPart);
            if (ActiveChildIndex == -1) return;

            int newIndex = (ActiveChildIndex + direction) % BodyPart.transform.childCount;
            if (newIndex < 0) newIndex += BodyPart.transform.childCount;

            BodyPart.transform.GetChild(newIndex).gameObject.SetActive(true);
            BodyPart.transform.GetChild(ActiveChildIndex).gameObject.SetActive(false);
            ClotheSkinIndex = 0;
        }

        public void ChangeColor(GameObject BodyPart, int direction)
        {
            if (BodyPart == null) return;

            SkinnedMeshRenderer renderer = BodyPart.GetComponent<SkinnedMeshRenderer>();
            if (renderer == null)
            {
                Debug.LogWarning($"No SkinnedMeshRenderer found on {BodyPart.name}");
                return;
            }

            if (BodyPart.name.Contains("Eye"))
            {
                EyeColorIndex = (EyeColorIndex + direction) % EyeColor.Length;
                if (EyeColorIndex < 0) EyeColorIndex += EyeColor.Length;
                renderer.sharedMaterial = EyeColor[EyeColorIndex];
            }
            else
            {
                SkinColorIndex = (SkinColorIndex + direction) % SkinColor.Length;
                if (SkinColorIndex < 0) SkinColorIndex += SkinColor.Length;
                renderer.sharedMaterial = SkinColor[SkinColorIndex];
            }
        }

        public void ChangeClothesSkin(GameObject BodyPart, int direction)
        {
            if (BodyPart == null) return;

            int activeChildIndex = GetCurrentActiveAccessory(BodyPart);
            if (activeChildIndex == -1) return;

            ClotheSkinIndex = (ClotheSkinIndex + direction) % ClothSkins.Length;
            if (ClotheSkinIndex < 0) ClotheSkinIndex += ClothSkins.Length;

            Transform activeChild = BodyPart.transform.GetChild(activeChildIndex);
            SkinnedMeshRenderer renderer = activeChild.GetComponent<SkinnedMeshRenderer>();
            if (renderer == null) return;

            renderer.sharedMaterial = ClothSkins[ClotheSkinIndex];
        }

        private int GetCurrentActiveAccessory(GameObject Parent)
        {
            if (Parent == null)
            {
                Debug.LogError("Parent GameObject is null.");
                return -1;
            }

            for (int i = 0; i < Parent.transform.childCount; i++)
            {
                if (Parent.transform.GetChild(i).gameObject.activeSelf)
                    return i;
            }

            Debug.LogWarning($"No active child found in {Parent.name}");
            return -1;
        }
    }
}