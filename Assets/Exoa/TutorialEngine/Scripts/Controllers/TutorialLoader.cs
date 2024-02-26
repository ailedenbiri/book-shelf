using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Exoa.TutorialEngine
{
    public class TutorialLoader : MonoBehaviour
    {
        public static TutorialLoader instance;

        private TutorialController tc;

        [HideInInspector]
        public bool tutorialLoaded;

        [HideInInspector]
        public Tutorial currentTutorial;

        [HideInInspector]
        public string loadedTutorialName;



        protected virtual void Awake()
        {
            instance = this;
            tc = GetComponent<TutorialController>();
            tutorialLoaded = false;
        }

        public void Start()
        {

        }

#if UNITY_EDITOR
        void OnGUI()
        {
            if (Input.GetKeyDown(KeyCode.E) && Event.current != null && Event.current.control && Event.current.type == EventType.KeyDown)
                SceneManager.LoadScene("TutorialEditor");
        }
#endif

        /// <summary>
        /// Load a tutorial file and launch the tutorial
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sendLoadedEvent"></param>
        virtual public void Load(string name, bool sendLoadedEvent = true)
        {
            if (tc != null && tc.debug) Debug.Log("Load " + name);

            currentTutorial = LoadOffline(name);

            loadedTutorialName = name;
            ProcessTutorial(sendLoadedEvent);
        }

        protected virtual void ProcessTutorial(bool sendLoadedEvent)
        {
            if (currentTutorial == null) return;

            tutorialLoaded = true;

            if (sendLoadedEvent)
                TutorialEvents.OnTutorialLoaded?.Invoke();
            TutorialEvents.OnTutorialReady?.Invoke();
        }

        protected static Tutorial LoadOffline(string name)
        {
            TextAsset json = Resources.Load<TextAsset>("Tutorials/" + name);
            return JsonUtility.FromJson<Tutorial>(json.text);
        }

    }
}
