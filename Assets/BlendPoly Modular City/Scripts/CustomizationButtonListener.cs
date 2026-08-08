using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Blendpoly.Character;

namespace Blendpoly.UI
{
	public class CustomizationButtonListener : MonoBehaviour
	{
		[Header("Gender Buttons")]
		[SerializeField] private Button GenderButton_Previous;
		[SerializeField] private Button GenderButton_Next;

		[Header("Hair Buttons (Mesh Changer)")]
		[SerializeField] private Button HairButton_Previous;
		[SerializeField] private Button HairButton_Next;
		[Header("Hair Buttons (Color Changer)")]
		[SerializeField] private Button HairColorButton_Previous;
		[SerializeField] private Button HairColorButton_Next;

		[Header("Eyes Buttons")]
		[SerializeField] private Button EyesButton_Previous;
		[SerializeField] private Button EyesButton_Next;

		[Header("Beard Buttons (Mesh Changer)")]
		[SerializeField] private Button BeardButton_Previous;
		[SerializeField] private Button BeardButton_Next;
		[Header("Beard Buttons (Color Changer)")]
		[SerializeField] private Button BeardColorButton_Previous;
		[SerializeField] private Button BeardColorButton_Next;

		[Header("Clothes Buttons (Mesh Changer)")]
		[SerializeField] private Button ClothesButton_Previous;
		[SerializeField] private Button ClothesButton_Next;
		[Header("Clothes Buttons (Color Changer)")]
		[SerializeField] private Button ClothesColorButton_Previous;
		[SerializeField] private Button ClothesColorButton_Next;

		[Header("Glasses Buttons (Mesh Changer)")]
		[SerializeField] private Button GlassesButton_Previous;
		[SerializeField] private Button GlassesButton_Next;

		[Header("Shoes Buttons (Mesh Changer)")]
		[SerializeField] private Button ShoesButton_Previous;
		[SerializeField] private Button ShoesButton_Next;
		[Header("Shoes Buttons (Color Changer)")]
		[SerializeField] private Button ShoesColorButton_Previous;
		[SerializeField] private Button ShoesColorButton_Next;

		[Header("Body Buttons (Color Changer)")]
		[SerializeField] private Button BodyColorButton_Previous;
		[SerializeField] private Button BodyColorButton_Next;

		[SerializeField] private CharacterCustomization CharacterCustom_Ref;

		private void Awake()
		{
			//-- Gender --//
			GenderButton_Previous.onClick.AddListener(() => { CharacterCustom_Ref.ChangeGender(-1); });
			GenderButton_Next.onClick.AddListener(() => { CharacterCustom_Ref.ChangeGender(1); });

			//-- Hair --//
			HairButton_Previous.onClick.AddListener(() => { CharacterCustom_Ref.ChangeAccessory(GetHairRef(), -1); });
			HairButton_Next.onClick.AddListener(() => { CharacterCustom_Ref.ChangeAccessory(GetHairRef(), 1); });

			HairColorButton_Previous.onClick.AddListener(() => { CharacterCustom_Ref.ChangeClothesSkin(GetHairRef(), -1); });
			HairColorButton_Next.onClick.AddListener(() => { CharacterCustom_Ref.ChangeClothesSkin(GetHairRef(), 1); });

			//-- Eyes --//
			EyesButton_Previous.onClick.AddListener(() => { CharacterCustom_Ref.ChangeColor(GetEyesRef(), -1); });
			EyesButton_Next.onClick.AddListener(() => { CharacterCustom_Ref.ChangeColor(GetEyesRef(), 1); });

			//-- Beard (Male only) --//
			BeardButton_Previous.onClick.AddListener(() =>
			{
				if (CharacterCustom_Ref.IsMale() && CharacterCustom_Ref.Male_Beard_ref != null)
					CharacterCustom_Ref.ChangeAccessory(CharacterCustom_Ref.Male_Beard_ref, -1);
			});
			BeardButton_Next.onClick.AddListener(() =>
			{
				if (CharacterCustom_Ref.IsMale() && CharacterCustom_Ref.Male_Beard_ref != null)
					CharacterCustom_Ref.ChangeAccessory(CharacterCustom_Ref.Male_Beard_ref, 1);
			});

			BeardColorButton_Previous.onClick.AddListener(() =>
			{
				if (CharacterCustom_Ref.IsMale() && CharacterCustom_Ref.Male_Beard_ref != null)
					CharacterCustom_Ref.ChangeClothesSkin(CharacterCustom_Ref.Male_Beard_ref, -1);
			});
			BeardColorButton_Next.onClick.AddListener(() =>
			{
				if (CharacterCustom_Ref.IsMale() && CharacterCustom_Ref.Male_Beard_ref != null)
					CharacterCustom_Ref.ChangeClothesSkin(CharacterCustom_Ref.Male_Beard_ref, 1);
			});

			//-- Clothes --//
			ClothesButton_Previous.onClick.AddListener(() => { CharacterCustom_Ref.ChangeAccessory(GetClothesRef(), -1); });
			ClothesButton_Next.onClick.AddListener(() => { CharacterCustom_Ref.ChangeAccessory(GetClothesRef(), 1); });

			ClothesColorButton_Previous.onClick.AddListener(() => { CharacterCustom_Ref.ChangeClothesSkin(GetClothesRef(), -1); });
			ClothesColorButton_Next.onClick.AddListener(() => { CharacterCustom_Ref.ChangeClothesSkin(GetClothesRef(), 1); });

			//-- Glasses --//
			GlassesButton_Previous.onClick.AddListener(() => { CharacterCustom_Ref.ChangeAccessory(GetGlassesRef(), -1); });
			GlassesButton_Next.onClick.AddListener(() => { CharacterCustom_Ref.ChangeAccessory(GetGlassesRef(), 1); });

			//-- Shoes --//
			ShoesButton_Previous.onClick.AddListener(() => { CharacterCustom_Ref.ChangeAccessory(GetShoesRef(), -1); });
			ShoesButton_Next.onClick.AddListener(() => { CharacterCustom_Ref.ChangeAccessory(GetShoesRef(), 1); });

			ShoesColorButton_Previous.onClick.AddListener(() => { CharacterCustom_Ref.ChangeClothesSkin(GetShoesRef(), -1); });
			ShoesColorButton_Next.onClick.AddListener(() => { CharacterCustom_Ref.ChangeClothesSkin(GetShoesRef(), 1); });

			//-- Body --//
			BodyColorButton_Previous.onClick.AddListener(() => { CharacterCustom_Ref.ChangeColor(GetBodyRef(), -1); });
			BodyColorButton_Next.onClick.AddListener(() => { CharacterCustom_Ref.ChangeColor(GetBodyRef(), 1); });
		}

		private GameObject GetHairRef() => CharacterCustom_Ref.IsMale() ? CharacterCustom_Ref.Male_Hair_ref : CharacterCustom_Ref.Female_Hair_ref;
		private GameObject GetEyesRef() => CharacterCustom_Ref.IsMale() ? CharacterCustom_Ref.Male_Eyes_ref : CharacterCustom_Ref.Female_Eyes_ref;
		private GameObject GetClothesRef() => CharacterCustom_Ref.IsMale() ? CharacterCustom_Ref.Male_Clothes_ref : CharacterCustom_Ref.Female_Clothes_ref;
		private GameObject GetGlassesRef() => CharacterCustom_Ref.IsMale() ? CharacterCustom_Ref.Male_Glasses_ref : CharacterCustom_Ref.Female_Glasses_ref;
		private GameObject GetShoesRef() => CharacterCustom_Ref.IsMale() ? CharacterCustom_Ref.Male_Shoes_ref : CharacterCustom_Ref.Female_Shoes_ref;
		private GameObject GetBodyRef() => CharacterCustom_Ref.IsMale() ? CharacterCustom_Ref.Male_Body_ref : CharacterCustom_Ref.Female_Body_ref;
	}
}