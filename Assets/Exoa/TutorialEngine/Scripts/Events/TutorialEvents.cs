using UnityEngine;
using UnityEngine.Events;

namespace Exoa.TutorialEngine
{
    public class TutorialEvents
    {
        public delegate void OnTutorialEventHandler();
        public delegate void OnTutorialFocusHandler(string objectName, Vector3 rectCenterPosition);
        public delegate void OnTutorialProgressHandler(int currentStep, int totalSteps);

        /// <summary>
        /// Triggered when the tutorial is completed
        /// </summary>
        public static OnTutorialEventHandler OnTutorialComplete;

        /// <summary>
        /// Triggered when the step has changed
        /// </summary>
        public static OnTutorialProgressHandler OnTutorialProgress;

        /// <summary>
        /// Triggered when a game object is hightlighted
        /// </summary>
        public static OnTutorialFocusHandler OnTutorialFocus;

        /// <summary>
        /// Triggered when a tutorial file has been loaded
        /// </summary>
        public static OnTutorialEventHandler OnTutorialLoaded;

        /// <summary>
        /// Triggered when a tutorial is ready to play
        /// </summary>
        public static OnTutorialEventHandler OnTutorialReady;
    }
}
