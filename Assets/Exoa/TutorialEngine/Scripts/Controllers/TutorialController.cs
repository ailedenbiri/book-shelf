using Exoa.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Exoa.TutorialEngine
{
    public class TutorialController : MonoBehaviour
    {
        private enum State { Inactive, Loading, Playing, FadingOut, Delaying, Completed };
        private static State tutorialState;
        private static bool isSkippable;
        private static bool autoNext = true;
        private static float delay = 0;

        private TutorialSession session;
        private List<TutorialSession.TutorialStep> steps;

        [Header("UI")]
        public TutorialPopup popup;

        public Button hiddenBtn;
        public RectTransform hiddenBtnRt;
        public RectTransform mask;
        private int currentStep = -1;
        public float maskScale = 1.2f;
        public float nextTime;

        public Image bg;

        public static TutorialController instance;
        private bool retried;

        private float size1Duration = 0.2f;

        private GameObject currentTarget;
        private Vector2 currentEndPositionValue;
        private float currentEndSizeValue;
        private float currentEndPositionChangeTime;

        private GameObject lastFocusedObject;
        private Transform lastFocusedObjectParent;
        private int lastFocusedObjectSibling;

        [Header("BACKGROUND")]
        public Color initBGColor = new Color(0, 0, 0, .7f);
        public Color normalBGColor = new Color(0, 0, 0, .7f);
        private Color currentBgColor;


        [Header("ANIMATION")]
        public Springs popupMoveSettings;
        private Vector2Spring popupMoveSpring;
        public Springs maskMoveSettings;
        private Vector2Spring maskMoveSpring;
        public Springs maskSize1Settings;
        public Springs maskSize2Settings;
        private Vector2Spring maskSizeSpring;
        public Springs bgColorSettings;
        private Vector4Spring bgColorSpring;

        private RenderMode canvasRenderMode;
        private Camera canvasRenderCamera;

        [Header("DEBUG MODE")]
        public bool debug;

        /// <summary>
        /// Option to display the close button or not, to skip the tutorial
        /// </summary>
        public static bool IsSkippable { get { return isSkippable; } set { isSkippable = value; } }

        /// <summary>
        /// Getter to check if the tutorial is still playing
        /// </summary>
        public static bool IsTutorialActive { get { return tutorialState == State.Playing; } }

        /// <summary>
        /// Getter to check if the tutorial is still playing
        /// </summary>
        public static bool IsTutorialComplete { get { return tutorialState == State.Completed; } }

        /// <summary>
        /// Automatically go to the next step after a step is done
        /// If false, the tutorial will hide until you call NextStep()
        /// </summary>
        public static bool AutoNext { get => autoNext; set => autoNext = value; }

        /// <summary>
        /// Add a delay between steps
        /// </summary>
        public static float Delay { get => delay; set => delay = value; }

        private void OnDestroy()
        {
            TutorialEvents.OnTutorialLoaded -= OnTutorialLoeaded;
        }
        void Awake()
        {
            if (instance != null) throw new Exception("TutorialController alraedy creaeted");

            instance = this;
            canvasRenderCamera = GetCamera();
            popup.gameObject.SetActive(false);
            popup.closeBtn.onClick.AddListener(OnClosePopup);
        }

        private void OnClosePopup()
        {
            HideTutorial();
        }

        void Start()
        {
            bg.material.color = initBGColor;

            if (TutorialLoader.instance.tutorialLoaded)
                OnTutorialLoeaded();

            TutorialEvents.OnTutorialLoaded += OnTutorialLoeaded;

        }

        private void OnTutorialLoeaded()
        {
            steps = new List<TutorialSession.TutorialStep>();
            if (TutorialLoader.instance != null && TutorialLoader.instance.currentTutorial.tutorial_steps != null)
                steps.AddRange(TutorialLoader.instance.currentTutorial.tutorial_steps);

            tutorialState = State.Playing;
            currentStep = -1;

            if (steps.Count > 0)
            {

                ShowTutorial();
                ForceNext();
            }
            else
            {
                HideTutorial();
            }
        }

        private void OnClickNext()
        {
            Next();
        }

        private void Update()
        {
            if (debug) Debug.Log("tutorialState:" + tutorialState);

            if (tutorialState == State.Inactive || tutorialState == State.Completed)
            {
                return;
            }
            if (tutorialState == State.Delaying)
            {
                if (nextTime < Time.time - delay)
                {
                    ShowTutorial();
                    ForceNext();
                }
            }
            if (tutorialState == State.FadingOut)
            {
                Vector2 targetOutSize = new Vector2(7000, 7000);
                mask.sizeDelta = maskSize1Settings.UpdateSpring(ref maskSizeSpring, targetOutSize);
                popup.PopupRt.anchoredPosition = popupMoveSettings.UpdateSpring(ref popupMoveSpring, targetOutSize);
                if (Vector2.Distance(targetOutSize, mask.sizeDelta) < 100)
                {
                    if (steps == null || currentStep >= steps.Count - 1)
                    {
                        if (debug) Debug.Log("Invoke OnTutorialComplete");
                        tutorialState = State.Completed;
                        TutorialEvents.OnTutorialComplete?.Invoke();
                    }
                    else
                        tutorialState = State.Inactive;

                    bg.gameObject.SetActive(false);
                    mask.gameObject.SetActive(false);
                    popup.Hide();


                    return;
                }
            }
            if (tutorialState == State.Playing)
            {
                if (currentTarget != null)
                {
                    Rect targetRect2D = GetObjectCanvasRect(currentTarget);
                    float targetSize = Mathf.Max(targetRect2D.width, targetRect2D.height);

                    if (currentEndSizeValue != targetSize)
                    {
                        currentEndSizeValue = targetSize;
                    }

                    if (currentEndPositionValue != targetRect2D.position)
                    {
                        currentEndPositionValue = targetRect2D.position;
                    }

                    mask.anchoredPosition = maskMoveSettings.UpdateSpring(ref maskMoveSpring, currentEndPositionValue);
                    Vector2 targetMaskSize = Vector2.one * currentEndSizeValue * maskScale;

                    if (currentEndPositionChangeTime > Time.time - size1Duration)
                    {
                        targetMaskSize = new Vector2(.3f, 2f) * currentEndSizeValue * maskScale;
                        mask.sizeDelta = maskSize1Settings.UpdateSpring(ref maskSizeSpring, targetMaskSize);
                    }
                    else
                    {
                        mask.sizeDelta = maskSize2Settings.UpdateSpring(ref maskSizeSpring, targetMaskSize);
                    }
                    Vector2 popupPositon = popup.CalculatePopupPosition(targetRect2D);
                    popup.PopupRt.anchoredPosition = popupMoveSettings.UpdateSpring(ref popupMoveSpring, popupPositon);
                    hiddenBtnRt.anchoredPosition = currentEndPositionValue;
                    hiddenBtnRt.sizeDelta = targetMaskSize;
                }
                else
                {
                    popup.PopupRt.anchoredPosition = popupMoveSettings.UpdateSpring(ref popupMoveSpring, Vector2.zero);
                }
                Vector4 newColorV = bgColorSettings.UpdateSpring(ref bgColorSpring, currentBgColor.ToVector4());
                bg.color = newColorV.ToColor();
            }
        }

        /// <summary>
        /// Jump to the next tutorial step
        /// </summary>
        public void Next()
        {
            InternalNext(false);
        }

        /// <summary>
        /// Jump to the next tutorial step
        /// </summary>
        public void ForceNext()
        {
            InternalNext(true);
        }

        private void InternalNext(bool force = false)
        {
            if (debug) Debug.Log("Next force:" + force + " autoNext:" + autoNext + " delay:" + delay);
            if (!autoNext && !IsTutorialActive)
            {
                ShowTutorial();
            }
            if (!autoNext && !force)
            {
                HideTutorial();

                return;
            }
            else if (delay > 0 && !force)
            {
                HideTutorial();
                nextTime = Time.time;
                tutorialState = State.Delaying;
                popup.Hide();
                return;
            }
            if (debug) print("Next Tutorial Step currentStep:" + currentStep + " steps.Count:" + steps.Count);
            currentStep++;
            if (steps == null || currentStep >= steps.Count)
            {
                HideTutorial();
                //tutorialState = State.Completed;
                //TutorialEvents.OnTutorialComplete?.Invoke();
                return;
            }
            TutorialSession.TutorialStep s = steps[currentStep];
            popup.SetStep(s);
            currentTarget = null;


            if (s.target_obj != "")
                currentTarget = GameObject.Find(s.target_obj);

            bool differentObject = currentTarget != lastFocusedObject;

            if (currentTarget != null)
            {
                lastFocusedObject = currentTarget;
                lastFocusedObjectParent = currentTarget.transform.parent;
                lastFocusedObjectSibling = currentTarget.transform.GetSiblingIndex();

                Rect rect2D = GetObjectCanvasRect(currentTarget);

                float size = Mathf.Max(rect2D.width, rect2D.height);
                Vector3 dir = new Vector3(rect2D.position.x, rect2D.position.y, 0) - mask.anchoredPosition3D;

                Debug.DrawRay(mask.position, dir, Color.yellow, 10);
                mask.rotation = Quaternion.LookRotation(mask.forward, dir);

                currentEndPositionValue = rect2D.position;
                currentEndPositionChangeTime = differentObject ? Time.time : Time.time - size1Duration;

                currentEndSizeValue = size;
                RectTransform rt = currentTarget.GetComponent<RectTransform>();
                Button btn = currentTarget.GetComponent<Button>();
                hiddenBtn.onClick.RemoveAllListeners();

                if (s.isClickable && rt != null && btn != null)
                {
                    hiddenBtn.gameObject.SetActive(true);
                    hiddenBtn.onClick.AddListener(btn.onClick.Invoke);
                }
                else
                {
                    hiddenBtn.gameObject.SetActive(false);
                }

                if (s.isReplacingNextButton && rt != null && btn != null)
                {
                    popup.nextBtn.gameObject.SetActive(false);


                    hiddenBtn.onClick.AddListener(popup.nextBtn.onClick.Invoke);
                }
                else
                {
                    popup.nextBtn.gameObject.SetActive(true);
                }
                TutorialEvents.OnTutorialFocus?.Invoke(s.target_obj, rect2D.center);
                TutorialEvents.OnTutorialProgress?.Invoke(currentStep, steps.Count);
            }
            else if (!retried && !string.IsNullOrEmpty(s.target_obj))
            {
                retried = true;
                if (debug) Debug.Log("RETRYING CANNOT FIND " + s.target_obj);
                // Retry in .2f seconds
                currentStep--;
                Invoke("Next", .2f);
            }
            else
            {
                if (!string.IsNullOrEmpty(s.target_obj) && debug)
                    Debug.Log("CANNOT FIND " + s.target_obj);

                if (!string.IsNullOrEmpty(s.sendMessage) && debug)
                {
                    Debug.Log("Calling " + s.sendMessage + " on popup");
                    popup.SendMessage(s.sendMessage, SendMessageOptions.DontRequireReceiver);
                }
                currentEndPositionValue = Vector2.zero;
                currentEndPositionChangeTime = Time.time;
                popup.nextBtn.gameObject.SetActive(true);
                hiddenBtn.gameObject.SetActive(false);
                mask.sizeDelta = (Vector2.zero);
                mask.anchoredPosition = (Vector2.zero);
                TutorialEvents.OnTutorialProgress?.Invoke(currentStep, steps.Count);
            }
            currentBgColor = currentStep == 0 ? initBGColor : normalBGColor;

        }

        private void ShowTutorial()
        {
            popup.OnClickNext.RemoveAllListeners();
            popup.OnClickNext.AddListener(OnClickNext);
            popup.closeBtn.gameObject.SetActive(IsSkippable);
            popup.gameObject.SetActive(true);
            popup.createBackground = false;
            popup.Init();
            popup.Center();
            popup.Open();

            bg.gameObject.SetActive(true);

            mask.gameObject.SetActive(true);
            mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
            mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
            mask.anchoredPosition = new Vector2(0, 1000);
            hiddenBtn.gameObject.SetActive(false);

            tutorialState = State.Playing;

        }
        /// <summary>
        /// Close the tutorial
        /// </summary>
        public void HideTutorial()
        {
            //popup.OnClickNext.RemoveAllListeners();

            tutorialState = State.FadingOut;

        }


        /**
        * Get Object's bounds in the Canvas space
        **/
        private Rect GetObjectCanvasRect(GameObject obj)
        {
            RectTransform objRect = obj.GetComponent<RectTransform>();
            Renderer objRenderer = obj.GetComponent<Renderer>();
            Collider objCollider = obj.GetComponentInChildren<Collider>();

            Rect newRect = new Rect();
            if (objRect != null)
            {
                newRect = objRect.GetRectFromOtherParent(mask.parent as RectTransform);

            }
            else if (objRenderer != null)
            {
                newRect = objRenderer.GetScreenRect(canvasRenderCamera);
                newRect = (mask.parent as RectTransform).ScreenRectToRectTransform(newRect,
                    canvasRenderMode == RenderMode.ScreenSpaceOverlay ? null : canvasRenderCamera);
            }
            else if (objCollider != null)
            {
                newRect = objCollider.GetScreenRect(canvasRenderCamera);
                newRect = (mask.parent as RectTransform).ScreenRectToRectTransform(newRect,
                    canvasRenderMode == RenderMode.ScreenSpaceOverlay ? null : canvasRenderCamera);

            }
            return newRect;
        }

        private Camera GetCamera()
        {
            Camera cam = Camera.main;
            // first find the canvas
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                canvas = transform.root.GetComponent<Canvas>();
            }
            if (canvas != null)
            {
                canvasRenderMode = canvas.renderMode;
                if (canvas.worldCamera != null)
                    cam = canvas.worldCamera;
            }
            return cam;
        }
    }
}
