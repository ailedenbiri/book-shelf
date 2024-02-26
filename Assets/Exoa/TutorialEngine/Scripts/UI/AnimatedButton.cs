using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Exoa.TutorialEngine
{
    [RequireComponent(typeof(AudioSource))]
    public class AnimatedButton : UIBehaviour, IPointerDownHandler, IPointerClickHandler
    {
        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        public bool interactable = true;
        public bool useClickInsteadDown;

        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

        private Animator m_animator;
        public AudioClip clickSound;

        override protected void Start()
        {
            base.Start();
            m_animator = GetComponent<Animator>();
        }

        public ButtonClickedEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (useClickInsteadDown)
                return;

            if (eventData.button != PointerEventData.InputButton.Left || !interactable)
                return;

            Press();
        }

        private void Press()
        {
            if (!IsActive())
                return;

            m_animator.SetTrigger("Pressed");
            Invoke("InvokeOnClickAction", 0.1f);
        }

        private void InvokeOnClickAction()
        {
            m_OnClick.Invoke();
            if (clickSound != null)
            {
                AudioSource source = GetComponent<AudioSource>();
                if (source == null) source = gameObject.AddComponent<AudioSource>();
                source.clip = clickSound;
                source.Play();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!useClickInsteadDown)
                return;

            if (eventData.button != PointerEventData.InputButton.Left || !interactable)
                return;

            Press();
        }
    }
}
