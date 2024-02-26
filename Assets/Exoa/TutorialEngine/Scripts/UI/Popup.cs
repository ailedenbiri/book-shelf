using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Exoa.TutorialEngine
{
    public class Popup : MonoBehaviour
    {
        public Color backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);
        public bool createBackground = true;
        private GameObject m_background;

        /// <summary>
        /// Recenter the popup
        /// </summary>
        public void Center()
        {
            RectTransform rt = transform as RectTransform;
            rt.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// Open the popup
        /// </summary>
        public void Open()
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Close"))
                animator.Play("Open");

            AddBackground();
        }

        /// <summary>
        /// Hide the popup
        /// </summary>
        public void Hide()
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                animator.Play("Close");

            RemoveBackground();
        }

        /// <summary>
        /// Close the popup
        /// </summary>
        public void Close()
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                animator.Play("Close");

            RemoveBackground();
            StartCoroutine(RunPopupDestroy());
        }

        private IEnumerator RunPopupDestroy()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(m_background);
            Destroy(gameObject);
        }

        private void AddBackground()
        {
            if (!createBackground)
                return;

            var bgTex = new Texture2D(1, 1);
            bgTex.SetPixel(0, 0, backgroundColor);
            bgTex.Apply();

            m_background = new GameObject("PopupBackground");
            var image = m_background.AddComponent<Image>();
            var rect = new Rect(0, 0, bgTex.width, bgTex.height);
            var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
            image.material.mainTexture = bgTex;
            image.sprite = sprite;
            var newColor = image.color;
            image.color = newColor;
            image.canvasRenderer.SetAlpha(0.0f);
            image.CrossFadeAlpha(1.0f, 0.4f, false);

            var canvas = transform.parent;
            m_background.transform.localScale = new Vector3(1, 1, 1);
            m_background.transform.SetParent(canvas.transform, false);
            m_background.transform.SetSiblingIndex(transform.GetSiblingIndex());

            RectTransform bgRect = m_background.GetComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0, 0);
            bgRect.anchorMax = new Vector2(1, 1);
        }

        private void RemoveBackground()
        {
            if (m_background != null)
            {
                var image = m_background.GetComponent<Image>();
                if (image != null)
                    image.CrossFadeAlpha(0.0f, 0.2f, false);
            }
        }
    }
}
