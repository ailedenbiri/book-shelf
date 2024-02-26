using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Maskable.Editor
{
	public static class ConvertMaskMenu
	{

		public static void Convert()
		{
			Assert.IsTrue(CanConvert());
			Undo.IncrementCurrentGroup();
			Undo.SetCurrentGroupName("Convert Mask to Maskable");
			var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable);
			foreach (var transform in selectedTransforms)
				Convert(transform.gameObject);
		}

		public static bool CanConvert()
		{
			var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable);
			return selectedTransforms.Any()
				&& selectedTransforms.All(t => IsConvertibleMask(t.gameObject));
		}

		static bool IsConvertibleMask(GameObject gameObject)
		{
			var mask = gameObject.GetComponent<Mask>();
			var graphic = gameObject.GetComponent<Graphic>();
			return mask
				&& graphic
				&& (graphic is Image || graphic is RawImage);
		}

		static void Convert(GameObject gameObject)
		{
			Assert.IsTrue(IsConvertibleMask(gameObject));
			DeepCheckConvertibility(gameObject);
			var mask = gameObject.GetComponent<Mask>();
			var Masking = Undo.AddComponent<Masking>(gameObject);
			var mayUseGraphic = MayUseGraphicSource(mask);
			if (mayUseGraphic)
			{
				Masking.source = Masking.MaskSource.Graphic;
				Undo.DestroyObjectImmediate(mask);
			}
			else
			{
				var graphic = gameObject.GetComponent<Graphic>();
				if (graphic is Image)
					SetUpFromImage(Masking, (Image)graphic);
				else if (graphic is RawImage)
					SetUpFromRawImage(Masking, (RawImage)graphic);
				else
					Debug.LogAssertionFormat("Converted Game Object should have an Image or Raw Image component");
				Undo.DestroyObjectImmediate(mask);
				if (!mask.showMaskGraphic)
					Undo.DestroyObjectImmediate(graphic);
			}
		}

		static void DeepCheckConvertibility(GameObject gameObject)
		{
			var rawImage = gameObject.GetComponent<RawImage>();
			if (rawImage)
			{
				var texture = rawImage.texture;
				if (texture && !(texture is Texture2D) && !(texture is RenderTexture))
					throw new UnsupportedRawImageTextureType(gameObject, texture);
			}
			var image = gameObject.GetComponent<Image>();
			if (image && !Masking.IsImageTypeSupported(image.type))
				throw new UnsupportedImageType(image.gameObject, image.type);
		}

		public class UnsupportedImageType : Exception
		{
			public UnsupportedImageType(GameObject objectBeingConverted, Image.Type unsupportedType)
			{
				this.objectBeingConverted = objectBeingConverted;
				this.unsupportedType = unsupportedType;
			}
			public GameObject objectBeingConverted { get; private set; }
			public Image.Type unsupportedType { get; private set; }
		}

		public class UnsupportedRawImageTextureType : Exception
		{
			public UnsupportedRawImageTextureType(GameObject objectBeingConverted, Texture unsupportedTexture)
			{
				this.objectBeingConverted = objectBeingConverted;
				this.unsupportedTexture = unsupportedTexture;
			}
			public GameObject objectBeingConverted { get; private set; }
			public Texture unsupportedTexture { get; private set; }
		}

		static bool MayUseGraphicSource(Mask mask)
		{
			var image = mask.GetComponent<Image>();
			var usesStandardUIMaskSprite = image && IsStandardUIMaskSprite(image.sprite);
			return mask.showMaskGraphic
				&& !usesStandardUIMaskSprite;
		}

		static bool IsStandardUIMaskSprite(Sprite sprite)
		{
			return sprite == standardUIMaskSprite;
		}

		static Sprite MaskingCompatibleVersionOf(Sprite original)
		{
			return IsStandardUIMaskSprite(original)
				? adaptedUIMaskSprite
				: original;
		}

		static void SetUpFromImage(Masking Masking, Image image)
		{
			Assert.IsTrue(Masking.IsImageTypeSupported(image.type));
			Masking.source = Masking.MaskSource.Sprite;
			Masking.sprite = MaskingCompatibleVersionOf(image.sprite);
			Masking.spriteBorderMode = Masking.ImageTypeToBorderMode(image.type);
#if UNITY_2019_2_OR_NEWER
			Masking.spritePixelsPerUnitMultiplier = image.pixelsPerUnitMultiplier;
#endif
		}

		static void SetUpFromRawImage(Masking Masking, RawImage rawImage)
		{
			Masking.source = Masking.MaskSource.Texture;
			var texture = rawImage.texture;
			if (texture)
				if (texture is Texture2D)
					Masking.texture = (Texture2D)texture;
				else if (texture is RenderTexture)
					Masking.renderTexture = (RenderTexture)texture;
				else
					Debug.LogAssertionFormat("Unsupported RawImage texture type: {0}", texture);
			Masking.textureUVRect = rawImage.uvRect;
		}

		static Sprite _standardUIMaskSprite;
		public static Sprite standardUIMaskSprite
		{
			get
			{
				if (!_standardUIMaskSprite)
					_standardUIMaskSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");
				return _standardUIMaskSprite;
			}
		}

		static Sprite _adaptedUIMaskSprite;
		public static Sprite adaptedUIMaskSprite
		{
			get
			{
				if (!_adaptedUIMaskSprite)
					_adaptedUIMaskSprite =
						AssetDatabase.LoadAssetAtPath<Sprite>(
							Path.Combine(PackageResources.packagePath, "Sprites/UIMask-FullAlpha.png"));
				return _adaptedUIMaskSprite;
			}
		}
	}
}
