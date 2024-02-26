
//using Exoa.Json;
using Exoa.TutorialEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.UIElements.Expansions;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Expansions;

namespace Exoa.Designer
{

    public class TutorialsEditor : EditorWindow
    {
        private Main mainPage;
        private H2 pageTitle;
        private PopupField<string> tutorialList;
        private Button createModuleBtn2;
        private List<string> meshesInSelction;
        private Div popupDiv, popupDiv2;
        private TextField newTutorialName;
        private SerializedObject serializedObject;
        private MyObject obj;
        private ReorderableList tutorialsList;



        public class MyObject : ScriptableObject
        {
            [SerializeField] public List<TutorialSession.TutorialStep> currentTutorialStepsList;
            [SerializeField] public List<string> myTutorialsNameList;
            [SerializeField] public string selectedTutorial;
        }


        /// <summary>
        /// Show the EditorWindow window.
        /// </summary>

        [MenuItem("Tools/Exoa/Tutorials Manager")]
        public new static void Show()
        {
            TutorialsEditor wnd = GetWindow<TutorialsEditor>();
            wnd.titleContent = new GUIContent("Tutorials Manager");
        }

        [MenuItem("Tools/Exoa/Tutorial Engine Help/Manual")]
        public new static void Help()
        {
            Application.OpenURL("http://monitor.exoa.fr/te-manual");
        }
        [MenuItem("Tools/Exoa/Tutorial Engine Help/Forum")]
        public new static void Forum()
        {
            Application.OpenURL("http://monitor.exoa.fr/te-forum");
        }

        private void OnEnable()
        {
            this.ApplyStyle();
            tutorialsList = this.rootVisualElement.Q<ReorderableList>("tutorialsList");

            obj = ScriptableObject.CreateInstance<MyObject>();

            obj.currentTutorialStepsList = new List<TutorialSession.TutorialStep>();

            pageTitle = this.rootVisualElement.Q<H2>("pageTitle");
            mainPage = this.rootVisualElement.Q<Main>("tabsPage");

            newTutorialName = this.rootVisualElement.Q<TextField>("newTutorialName");


            Button saveTutorialBtn = this.rootVisualElement.Q<Button>("saveTutorialBtn");
            Button addNewCategoryItemBtn = this.rootVisualElement.Q<Button>("addNewStepBtn");
            //Button removeSelectedCategoryItemBtn = this.rootVisualElement.Q<Button>("removeSelectedStepBtn");
            Button editBtn = this.rootVisualElement.Q<Button>("editBtn");
            Button createTutorialBtn = this.rootVisualElement.Q<Button>("createTutorialBtn");

            createTutorialBtn.clickable.clicked += OnClickCreateNew;
            saveTutorialBtn.clickable.clicked += OnClickSaveTutorials;
            editBtn.clickable.clicked += OnClickEditTutorial;

            addNewCategoryItemBtn.clickable.clicked += OnClickAddNewStepBtn;
            //removeSelectedCategoryItemBtn.clickable.clicked += OnClickRemoveSelectedStepBtn;

            popupDiv = this.rootVisualElement.Q<Div>("popupDiv");

            // Create a new field and assign it its value.

            UpdateListOfTutorialFiles();


        }

        private void UpdateListOfTutorialFiles()
        {
            TextAsset[] ta = Resources.LoadAll<TextAsset>("Tutorials/");

            obj.myTutorialsNameList = new List<string>();
            for (int i = 0; i < ta.Length; i++)
            {
                obj.myTutorialsNameList.Add(ta[i].name);
            }

            BindListsToObjects();
            CreateTutorialsPopups();
        }

        private void OnClickCreateNew()
        {
            string filename = newTutorialName.text;
            string folderPath = GetFilePath("Tutorials t:Folder", "Tutorials", "folder", true, "Resources/");
            if (string.IsNullOrEmpty(filename))
            {
                EditorUtility.DisplayDialog("Error", "Please enter a file name", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(folderPath))
                return;

            string filePath = folderPath + "/" + filename + ".json";

            // Make sure the file name is unique, in case an existing Prefab has the same name.
            filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);

            File.WriteAllText(filePath, "{}", Encoding.UTF8);

            EditorUtility.DisplayDialog("Done!", "File created at: " + filePath, "Ok");

            AssetDatabase.Refresh();


            obj.selectedTutorial = filename;
            obj.currentTutorialStepsList = new List<TutorialSession.TutorialStep>();
            obj.currentTutorialStepsList.Add(new TutorialSession.TutorialStep() { text = "Hello!" });

            UpdateListOfTutorialFiles();
        }

        private void OnClickEditTutorial()
        {
            Debug.Log(tutorialList.value);

            //Debug.Log(obj.myTutorialsNameList.Count);

            TextAsset ta = Resources.Load<TextAsset>("Tutorials/" + tutorialList.value);
            //Tutorial t = JsonConvert.DeserializeObject<Tutorial>(ta.text);
            Tutorial t = JsonUtility.FromJson<Tutorial>(ta.text);

            obj.selectedTutorial = tutorialList.value;
            obj.currentTutorialStepsList = new List<TutorialSession.TutorialStep>();
            obj.currentTutorialStepsList.AddRange(t.tutorial_steps);


            BindListsToObjects();
        }

        private void OnClickRemoveSelectedStepBtn()
        {
            int index = GetSelectedIndex(tutorialsList);
            if (index > -1 && obj.currentTutorialStepsList.Count > index)
            {
                obj.currentTutorialStepsList.RemoveAt(index);
                BindListsToObjects();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Could not find the element " + index + " to remove!", "Ok");
                return;
            }
        }

        private int GetSelectedIndex(ReorderableList list)
        {
            int index = -1;
            VisualElement selected = list.Q<VisualElement>(null, "unity-reorderable-list__item_selected");
            if (selected != null)
            {
                VisualElement child = selected;
                do
                {
                    string name = child.name;
                    int startIndex = name.LastIndexOf('[');
                    if (startIndex > 0)
                    {
                        string subStr = name.Substring(startIndex + 1, 1);
                        index = int.Parse(subStr);
                    }
                    child = child.Children().ElementAt(0);

                } while (index == -1 && child != null);
            }
            return index;
        }

        private void OnClickAddNewStepBtn()
        {
            obj.currentTutorialStepsList.Add(new TutorialSession.TutorialStep());
            BindListsToObjects();
        }

        private void BindListsToObjects()
        {
            serializedObject = new UnityEditor.SerializedObject(obj);
            tutorialsList.BindProperty(serializedObject.FindProperty("currentTutorialStepsList"));
        }

        private void CreateTutorialsPopups()
        {
            popupDiv.Clear();
            tutorialList = new PopupField<string>("Select a tutorial", obj.myTutorialsNameList, 0);

            popupDiv.Add(tutorialList);

        }


        private void OnClickSaveTutorials()
        {
            string filePath = GetFilePath(obj.selectedTutorial + " t:TextAsset", obj.selectedTutorial + ".json", "file", true, "Resources/Tutorials/");
            if (string.IsNullOrEmpty(filePath))
                return;

            Tutorial t = new Tutorial();
            t.tutorial_steps = obj.currentTutorialStepsList.ToArray();
            //string content = JsonConvert.SerializeObject(t, Formatting.Indented);
            string content = JsonUtility.ToJson(t, true);
            Debug.Log("obj.currentTutorialStepsList:" + obj.currentTutorialStepsList.Count);
            Debug.Log("content:" + content);

            if (content != null && content.Length > 2)
            {
                File.WriteAllText(filePath, content, Encoding.UTF8);
                EditorUtility.DisplayDialog("Saved!", "File saved to:" + filePath, "Ok");

            }

            AssetDatabase.Refresh();

        }

        private string GetFilePath(string searchStr, string endsWith, string fileOrFolder = "file", bool alertIfNotFound = true, string resourcesFolder = "Resources/")
        {
            List<string> guids = new List<string>(AssetDatabase.FindAssets(searchStr, new[] { "Assets" }));

            for (int i = 0; i < guids.Count; i++)
            {
                guids[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (!guids[i].EndsWith(endsWith))
                {
                    guids.RemoveAt(i);
                    i--;
                }
            }
            if (alertIfNotFound)
            {
                if (guids == null || guids.Count == 0)
                {
                    string resolution = resourcesFolder == null ? ", did you removed it ?" : ", please create it in your " + resourcesFolder + " folder!";
                    EditorUtility.DisplayDialog("Error", "Could not find the " + fileOrFolder + " " + endsWith + " in the project" + resolution, "Ok");
                    return null;
                }
                if (guids.Count > 1)
                {
                    EditorUtility.DisplayDialog("Error", "You have multiple " + fileOrFolder + "s in your project called " + endsWith + ", please keep only a single one!", "Ok");
                    return null;
                }
            }
            if (guids == null || guids.Count == 0)
                return null;
            return guids[0];
        }

    }
}