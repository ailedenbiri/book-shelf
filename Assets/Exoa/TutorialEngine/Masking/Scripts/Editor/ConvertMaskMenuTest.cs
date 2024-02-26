using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using NUnit.Framework;
using System;
using System.Linq;
using Object = UnityEngine.Object;

namespace Maskable.Editor {
    public class ConvertMaskMenuTest {
        [Test] public void WhenNoObjectSelected_ShouldBeNotAvailable() {
            SelectObjects();
            Assert.IsFalse(ConvertMaskMenu.CanConvert());
        }

        [Test] public void WhenEmptyObjectSelected_ShouldBeNotAvailable() {
            AssertObjectIsNotConvertible();
        }

        [Test] public void WhenObjectWithMaskButWithoutGraphicSelected_ShouldBeNotAvailable() {
            AssertObjectIsNotConvertible(typeof(Mask));
        }

        [Test] public void WhenObjectWithGraphicButWithoutMaskSelected_ShouldBeNotAvailable() {
            AssertObjectIsNotConvertible(typeof(Image));
        }

        [Test] public void WhenObjectWithWrongTypeOfGraphicSelected_ShouldBeNotAvailable() {
            AssertObjectIsNotConvertible(typeof(Text), typeof(Mask));
        }
        
        void AssertObjectIsNotConvertible(params Type[] componentTypes) {
            var go = CreateGameObject(componentTypes);
            SelectObjects(go);
            Assert.IsFalse(ConvertMaskMenu.CanConvert());
        }
        
        GameObject CreateGameObject(params Type[] componentTypes) {
            var go = new GameObject("TestObject", componentTypes);
            Undo.RegisterCreatedObjectUndo(go, "Undo TestObject creation");
            return go;
        }

        void SelectObjects(params GameObject[] objects) {
            Selection.objects = objects.Cast<Object>().ToArray();
        }

        [Test] public void WhenNotAllOfSelectedObjectsConvertible_ShouldBeNotAvailable() {
            var good = CreateGameObject(typeof(Mask), typeof(Image));
            var bad = CreateGameObject(typeof(Mask), typeof(Text));
            SelectObjects(good, bad);
            Assert.IsFalse(ConvertMaskMenu.CanConvert());
        }

        [Test] public void WhenConvertibleObjectsSelected_ShouldBeAvailable() {
            var go = CreateGameObject(typeof(Mask), typeof(Image));
            SelectObjects(go);
            Assert.IsTrue(ConvertMaskMenu.CanConvert());
        }
        
        [Test] public void WhenInvokedOnSeveralObjects_TheyAllShouldBeConverted() {
            var gos = new [] {
                CreateObjectWithImageMask(renderable: true),
                CreateObjectWithImageMask(renderable: false),
                CreateObjectWithRawImageMask(renderable: true),
                CreateObjectWithRawImageMask(renderable: false),
            };
            SelectAndConvertObjects(gos);
            AssertConvertedProperly(gos[0], renderable: true, raw: false);
            AssertConvertedProperly(gos[1], renderable: false, raw: false);
            AssertConvertedProperly(gos[2], renderable: true, raw: true);
            AssertConvertedProperly(gos[3], renderable: false, raw: true);
        }

        void SelectAndConvertObjects(params GameObject[] objects) {
            SelectObjects(objects);
            ConvertMaskMenu.Convert();
        }

        void AssertConvertedProperly(GameObject go, bool renderable, bool raw) {
            var Masking = go.GetComponent<Masking>();
            Assert.IsNotNull(Masking);
            Assert.IsNull(go.GetComponent<Mask>());
            if (renderable) {
                Assert.AreEqual(Masking.MaskSource.Graphic, Masking.source);
                if (raw)
                    AssertHasComponent<RawImage>(go);
                else
                    AssertHasComponent<Image>(go);
            } else {
                if (raw) {
                    AssertHasNoComponent<RawImage>(go);
                    AssertRawImageConvertedProperly(Masking);
                } else {
                    AssertHasNoComponent<Image>(go);
                    AssertImageConvertedProperly(Masking);
                }
            }
        }

        void AssertHasComponent<T>(GameObject go) where T : Component {
            Assert.IsNotNull(go.GetComponent<T>());
        }

        void AssertHasNoComponent<T>(GameObject go) where T : Component {
            Assert.IsNull(go.GetComponent<T>());
        }

        static void AssertRawImageConvertedProperly(Masking Masking) {
            Assert.AreEqual(standardUISprite.texture, Masking.texture);
            Assert.AreEqual(standardRect, Masking.textureUVRect);
        }
        
        static void AssertImageConvertedProperly(Masking Masking) {
            Assert.AreEqual(standardUISprite, Masking.sprite);
            Assert.AreEqual(Masking.BorderMode.Sliced, Masking.spriteBorderMode);
        #if UNITY_2019_2_OR_NEWER
            Assert.AreEqual(120, Masking.spritePixelsPerUnitMultiplier);
        #endif
        }

        GameObject CreateObjectWithImageMask(bool renderable, Sprite sprite = null) {
            var go = CreateGameObject();
            var image = go.AddComponent<Image>();
            image.sprite = sprite ? sprite : standardUISprite;
            image.type = Image.Type.Sliced;
        #if UNITY_2019_2_OR_NEWER
            image.pixelsPerUnitMultiplier = 120;
        #endif
            var mask = go.AddComponent<Mask>();
            mask.showMaskGraphic = renderable;
            return go;
        }

        static Sprite _standardUISprite;
        static Sprite standardUISprite {
            get {
                return _standardUISprite
                    ? _standardUISprite
                    : (_standardUISprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd"));
            }
        }

        GameObject CreateObjectWithRawImageMask(bool renderable) {
            var go = CreateGameObject();
            var image = go.AddComponent<RawImage>();
            image.texture = standardUISprite.texture;
            image.uvRect = standardRect;
            var mask = go.AddComponent<Mask>();
            mask.showMaskGraphic = renderable;
            return go;
        }

        static readonly Rect standardRect = new Rect(0.2f, 0.1f, 0.7f, 0.6f);

        [Test] public void WhenImageWithStandardUIMaskSpriteConverted_MaskingShouldHaveAdaptedSprite() {
            foreach (var renderable in trueAndFalse) {
                var go = CreateAndConvertObjectWithImageMask(renderable, sprite: ConvertMaskMenu.standardUIMaskSprite);
                AssertMaskHaveAdaptedSprite(go);
            }
        }

        static readonly bool[] trueAndFalse = new[] { true, false };

        GameObject CreateAndConvertObjectWithImageMask(bool renderable, Sprite sprite = null) {
            var go = CreateObjectWithImageMask(renderable, sprite: sprite);
            SelectAndConvertObjects(go);
            return go;
        }
        
        void AssertMaskHaveAdaptedSprite(GameObject go) {
            var Masking = go.GetComponent<Masking>();
            Assert.AreEqual(ConvertMaskMenu.adaptedUIMaskSprite, Masking.sprite);
        }

        [Test] public void WhenRenderableImageWithStandardUIMaskSpriteConverted_ImageShouldSkillHaveStandardSprite() {
            var go = CreateAndConvertObjectWithImageMask(renderable: true, sprite: ConvertMaskMenu.standardUIMaskSprite);
            var image = go.GetComponent<Image>();
            Assert.AreEqual(ConvertMaskMenu.standardUIMaskSprite, image.sprite);
        }

        [Test] public void AfterConversion_AllTheChangesMayBeUndoneInSingleStep() {
            foreach (var renderable in trueAndFalse) {
                Undo.IncrementCurrentGroup();
                var go = CreateAndConvertObjectWithImageMask(renderable);
                Undo.PerformUndo();
                AssertHasComponent<Mask>(go);
                AssertHasComponent<Image>(go);
                AssertHasNoComponent<Masking>(go);
            }
        }

        [Test] public void AfterUndo_AllTheChangesMayBeReappliedInSingleStep() {
            foreach (var renderable in trueAndFalse) {
                var go = CreateAndConvertObjectWithImageMask(renderable);
                Undo.PerformUndo();
                Undo.PerformRedo();
                AssertConvertedProperly(go, renderable, raw: false);
            }
        }

        [Test] public void WhenRawImageWithUnsupportedTextureTypeConverted_ShouldThrow() {
            var unsupportedTexture = new Texture3D(4, 4, 4, TextureFormat.Alpha8, false);
            try {
                foreach (var renderable in trueAndFalse) {
                    var go = CreateObjectWithRawImageMask(renderable);
                    go.GetComponent<RawImage>().texture = unsupportedTexture;
                    SelectObjects(go);
                    Assert.Throws(typeof(ConvertMaskMenu.UnsupportedRawImageTextureType), ConvertMaskMenu.Convert);
                }
            } finally {
                Object.DestroyImmediate(unsupportedTexture);
            }
        }

        [Test] public void WhenImageWithoutSpriteConverted_ShouldConvertToMaskingWithoutSprite() {
            var go = CreateObjectWithImageMask(renderable: false);
            go.GetComponent<Image>().sprite = null;
            SelectAndConvertObjects(go);
            AssertHasComponent<Masking>(go);
            AssertHasNoComponent<Image>(go);
            Assert.IsNull(go.GetComponent<Masking>().sprite);
        }

        [Test] public void WhenRawImageWithoutTextureConverted_ShouldConvertToMaskingWithoutTexture() {
            var go = CreateObjectWithRawImageMask(renderable: false);
            go.GetComponent<RawImage>().texture = null;
            SelectAndConvertObjects(go);
            AssertHasComponent<Masking>(go);
            AssertHasNoComponent<RawImage>(go);
            Assert.IsNull(go.GetComponent<Masking>().texture);
        }

        [Test] public void WhenImageOfUnsupportedTypeConverted_ShouldThrow() {
            foreach (var renderable in trueAndFalse) {
                var go = CreateObjectWithImageMask(renderable);
                go.GetComponent<Image>().type = Image.Type.Filled;
                SelectObjects(go);
                Assert.Throws(typeof(ConvertMaskMenu.UnsupportedImageType), ConvertMaskMenu.Convert);
            }
        }
    }
}
