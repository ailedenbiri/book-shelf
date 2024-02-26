using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Exoa.TutorialEngine
{
    public class TutorialPopup : Popup
    {
        static public TutorialPopup instance;
        private void Awake()
        {
            instance = this;
        }

        public AnimatedButton nextBtn;
        public Button closeBtn;
        public Text contentText;
        public UnityEvent OnClickNext;
        public Transform bg;
        private RectTransform popupRt;

        public RectTransform PopupRt { get => popupRt; set => popupRt = value; }

        /// <summary>
        /// Initialize the popup
        /// </summary>
        public void Init()
        {
            popupRt = GetComponent<RectTransform>();

            nextBtn.onClick.RemoveAllListeners();
            nextBtn.onClick.AddListener(OnClickNextHandler);
        }

        private void OnClickNextHandler()
        {
            OnClickNext.Invoke();
            CloseNextBtn(true);
        }

        /// <summary>
        /// Set step details in the popup
        /// </summary>
        /// <param name="s"></param>
        public void SetStep(TutorialSession.TutorialStep s)
        {
            contentText.text = s.text;
            UpdateHGroup();
        }

        /// <summary>
        /// Calculate the popup's position regarding the object hightlighted
        /// </summary>
        /// <param name="newRect"></param>
        /// <returns></returns>
        public Vector2 CalculatePopupPosition(Rect newRect)
        {
            Vector2 maskPosition = newRect.position;
            float maskDistanceFromCenter = maskPosition.magnitude;

            float maskDiagonal = Mathf.Max(newRect.width, newRect.height);


            float popupDiagonal = Mathf.Sqrt(Mathf.Pow(popupRt.rect.width, 2) + Mathf.Pow(popupRt.rect.height, 2));
            Vector2 popupPostion = (maskPosition.magnitude - (maskDiagonal * .5f) - (popupDiagonal * .5f)) * maskPosition.normalized;

            RectTransform parent = popupRt.parent as RectTransform;
            float maxX = parent.rect.width * .5f - popupRt.rect.width * .5f;
            float maxY = parent.rect.height * .5f - popupRt.rect.height * .5f;

            popupPostion.x = Mathf.Clamp(popupPostion.x, -maxX, maxX);
            popupPostion.y = Mathf.Clamp(popupPostion.y, -maxY, maxY);

            return popupPostion;
        }

        private void UpdateHGroup()
        {
            ContentSizeFitter csf = bg.GetComponent<ContentSizeFitter>();
            csf.enabled = false;
            csf.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)bg);
            csf.enabled = true;
        }

        public void OpenNextBtn()
        {
            nextBtn.interactable = true;
            nextBtn.transform.DOScale(Vector3.one, 0.3f);
        }

        public void CloseNextBtn(bool openAfter)
        {
            nextBtn.interactable = false;
            nextBtn.transform.DOScale(Vector3.zero, 0.3f);
            if (openAfter)
            {
                DOVirtual.DelayedCall(3f, () => OpenNextBtn());
            }
        }
    }
}
