using System;
using System.IO;
using System.Reflection;
using System.Text;
using Assets.Scripts.Core.Editor.Code;
using Assets.Scripts.Core.Enums;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Core.Editor.Test
{
    [InitializeOnLoad]
    public class CreateViewTemplateListener
    {
        static CreateViewTemplateListener()
        {
            if (EditorPrefs.GetBool("CreateViewTemplate"))
            {
                EditorPrefs.SetBool("CreateViewTemplate", false);

                string name = EditorPrefs.GetString("Name");
                string ttype = EditorPrefs.GetString("Type");
                EditorPrefs.DeleteKey("Name");
                EditorPrefs.DeleteKey("Type");
                EditorPrefs.DeleteKey("ScriptRootPath");

                GameObject scenePrefab = new GameObject(name);
                scenePrefab.AddComponent<RectTransform>();
                scenePrefab.GetComponent<RectTransform>().localScale = Vector3.one;

                Type componentType = GetTypeFromName(name + ttype + "View");
                scenePrefab.AddComponent(componentType);

                Object root = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Core/RootTemplate.prefab", typeof(GameObject));
                root.name = "Root";
                PrefabUtility.InstantiatePrefab(root);

                var newRoot = GameObject.Find("Root");

                string scenePrefabPath = "Assets/Resources/Screens/" + name + ".prefab";
                PrefabUtility.SaveAsPrefabAsset(scenePrefab, scenePrefabPath);
                Object.DestroyImmediate(scenePrefab);

                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(scenePrefabPath);

                var newScene = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                GameObject screenContainer = GameObject.Find("Layer0");
                if (screenContainer.transform.childCount > 0)
                    Object.DestroyImmediate(screenContainer.transform.GetChild(0).gameObject);

                if (newScene != null)
                {
                    newScene.transform.SetParent(screenContainer.transform);
                    newScene.GetComponent<RectTransform>().localScale = Vector3.one;
                }

                Type type = GetTypeFromName(name + ttype + "TestRoot");
                newRoot.AddComponent(type);

                EditorSceneManager.MarkAllScenesDirty();
            }
        }

        private static Type GetTypeFromName(string classNameWithNameSpace)
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = a.GetTypes();
                foreach (Type type in types)
                {
                    if (type.AssemblyQualifiedName != null && type.AssemblyQualifiedName.Contains(classNameWithNameSpace))
                    {
                        return type;
                    }
                }
            }

            return null;
        }
    }


    public class CreateViewTemplate : ScriptableWizard
    {
        private const string NamespacePlaceholder = "%TemplateNS%";
        private const string ClassnamePlaceholder = "%Template%";
        private const string FunctionPlaceholder = "%ListenerFunction%";
        private const string AddListenerPlaceholder = "%AddListener%";
        private const string RemoveListenerPlaceholder = "%RemoveListener%";
        private const string EventPlaceholder = "%EventEnum%";

        private const string ViewFunctionTemplate =
          "public void On{0}Click()\r\t\t{{\r\t\t\tdispatcher.Dispatch({1}{2}Event.{0});\r\t\t}}";

        private const string MediatorFunctionTemplate = "public void On{0}()\r\t\t{{\r\t\t\t\r\t\t}}";
        private const string AddListenerTemplate = "view.dispatcher.AddListener({1}{2}Event.{0},On{0});";
        private const string RemoveListenerTemplate = "view.dispatcher.RemoveListener({1}{2}Event.{0},On{0});";
        private const string TemplatesPath = "Assets/Scripts/Core/Editor/Test/Templates/Template";
        public const string ViewPath = "Assets/Scripts/Project/View";
        public string Type = "Screen";
        private const string Path = "Assets/Tests";

        public string Name = string.Empty;

        public string[] EventList;

        private string _scriptRootPath;

        private static string _appScreensPath = Application.dataPath + "\\Scripts\\Core\\Enums\\AppScreens.cs";
        private static string _appStatusPath = Application.dataPath + "\\Scripts\\Core\\Enums\\AppStatus.cs";


        [MenuItem("Tools/Core/Create Screen")]
        [UsedImplicitly]
        private static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Panel", typeof(CreateViewTemplate), "Create");
        }

        private static void UpdateAppScreens(string name)
        {
            string newValue = "public const string " + name + " = " + "\"" + name + "\"" + ";" + "\r\t\t" + "/*EDITOR-BREAKPOINT*/";

            StreamReader r = new StreamReader(_appScreensPath);
            string text = r.ReadToEnd().Replace("/*EDITOR-BREAKPOINT*/", newValue);
            r.Close();

            StreamWriter w = new StreamWriter(_appScreensPath);
            w.Write(text);
            w.Flush();
            w.Close();
        }

        private static void UpdateAppStatus(string name)
        {
            StreamReader r = new StreamReader(_appStatusPath);
            string text = r.ReadToEnd();
            int index = text.IndexOf("/*EDITOR-NUMPOINT*/") - 1;

            string lastNumberValue = "";
            while (!text[index].Equals(' '))
            {
                lastNumberValue = text[index] + lastNumberValue;
                index--;
            }

            int lastNumber = int.Parse(lastNumberValue);
            int newNumber = lastNumber + 1;
            text = text.Replace(lastNumberValue, newNumber.ToString());

            string newValue = name + " = 1 << " + lastNumber + "," + "\r\t\t" + "/*EDITOR-BREAKPOINT*/ ";
            text = text.Replace("/*EDITOR-BREAKPOINT*/", newValue);

            r.Close();

            StreamWriter w = new StreamWriter(_appStatusPath);
            w.Write(text);
            w.Flush();
            w.Close();
        }

        [UsedImplicitly]
        private void OnWizardCreate()
        {
            if (string.IsNullOrEmpty(Name))
                return;

            CreateFolders();

            CreateTestScripts(_scriptRootPath);

            CreateScripts(ViewPath);

            EditorPrefs.SetBool("CreateViewTemplate", true);
            EditorPrefs.SetString("Name", Name);
            EditorPrefs.SetString("Type", Type);

            AssetDatabase.Refresh();
        }

        private void CreateFolders()
        {
            if (!AssetDatabase.IsValidFolder(Path + "/" + "Screen"))
                AssetDatabase.CreateFolder(Path, "Screen");
            var rootPath = Path + "/" + "Screen" + "/" + Name;

            if (!AssetDatabase.IsValidFolder(Path + "/" + "Screen" + "/" + Name))
                AssetDatabase.CreateFolder(Path + "/" + "Screen", Name);
            if (!AssetDatabase.IsValidFolder(rootPath + "/" + "Scripts"))
                AssetDatabase.CreateFolder(rootPath, "Scripts");
            _scriptRootPath = rootPath + "/" + "Scripts";
            if (!AssetDatabase.IsValidFolder(_scriptRootPath + "/" + "Controller"))
                AssetDatabase.CreateFolder(_scriptRootPath, "Controller");

            // create scene
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(newScene, rootPath + "/" + Name + Type + "Test.unity");
            UnityEngine.Object sceneAsset =
              AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(rootPath + "/" + Name + Type + "Test.unity");
            AssetDatabase.SetLabels(sceneAsset, new string[] { "Exclude" });
        }

        private void CreateTestScripts(string scriptRootPath)
        {
            AddScript(scriptRootPath, Name, TemplateType.Root, Type);
            AddScript(scriptRootPath, Name, TemplateType.Context, Type);
            AddScript(scriptRootPath + "/Controller", Name, TemplateType.Command, Type);
        }

        private void CreateScripts(string scriptRootPath)
        {
            AssetDatabase.CreateFolder(scriptRootPath, Name);
            var data = LoadTemplate(TemplateType.View);
            string namespaceValue = scriptRootPath.Replace("/", ".") + "." + Name;
            data = data.Replace(NamespacePlaceholder, namespaceValue);
            data = data.Replace(ClassnamePlaceholder, Name + Type);
            if (EventList.Length == 0)
                data = data.Replace(FunctionPlaceholder, "");
            else
            {
                string functions = "";
                for (int i = 0; i < EventList.Length; i++)
                {
                    functions += string.Format(ViewFunctionTemplate, EventList[i], Name, Type);
                    if (i < EventList.Length - 1)
                        functions += "\r\t\t\r\t\t";
                }

                data = data.Replace(FunctionPlaceholder, functions);
            }

            CodeUtilities.SaveFile(data, scriptRootPath + "/" + Name + "/" + Name + Type + "View.cs");

            data = LoadTemplate(TemplateType.Mediator);
            data = data.Replace(NamespacePlaceholder, namespaceValue);
            data = data.Replace(ClassnamePlaceholder, Name + Type);
            if (EventList.Length == 0)
            {
                data = data.Replace(FunctionPlaceholder, "");
                data = data.Replace(EventPlaceholder, "");
                data = data.Replace(AddListenerPlaceholder, "");
                data = data.Replace(RemoveListenerPlaceholder, "");
            }
            else
            {
                string events = "";
                for (int i = 0; i < EventList.Length; i++)
                {
                    events += EventList[i];
                    if (i < EventList.Length - 1)
                        events += ",\r\t\t";
                }

                data = data.Replace(EventPlaceholder, events);

                string addListeners = "";
                for (int i = 0; i < EventList.Length; i++)
                {
                    addListeners += string.Format(AddListenerTemplate, EventList[i], Name, Type);
                    if (i < EventList.Length - 1)
                        addListeners += "\r\t\t\t";
                }

                data = data.Replace(AddListenerPlaceholder, addListeners);

                string functions = "";
                for (int i = 0; i < EventList.Length; i++)
                {
                    functions += string.Format(MediatorFunctionTemplate, EventList[i]);
                    if (i < EventList.Length - 1)
                        functions += "\r\t\t\r\t\t";
                }

                data = data.Replace(FunctionPlaceholder, functions);

                string removeListeners = "";
                for (int i = 0; i < EventList.Length; i++)
                {
                    removeListeners += string.Format(RemoveListenerTemplate, EventList[i], Name, Type);
                    if (i < EventList.Length - 1)
                        removeListeners += "\r\t\t\t";
                }

                data = data.Replace(RemoveListenerPlaceholder, removeListeners);
            }

            UpdateAppScreens(Name);
            UpdateAppStatus(Name);

            CodeUtilities.SaveFile(data, scriptRootPath + "/" + Name + "/" + Name + Type + "Mediator.cs");
        }

        private static void AddScript(string path, string name, TemplateType type, string ttype)
        {
            var data = LoadTemplate(type);
            data = data.Replace(NamespacePlaceholder, path.Replace("/", "."));
            data = data.Replace(ClassnamePlaceholder, name + ttype + "Test");
            data = data.Replace("%Name%", name);
            data = data.Replace("%Type%", ttype);
            CodeUtilities.SaveFile(data, path + "/" + name + ttype + "Test" + type + ".cs");

            Object scneneAsset =
              AssetDatabase.LoadAssetAtPath<Object>(path + "/" + name + ttype + "Test" + type + ".cs");
            AssetDatabase.SetLabels(scneneAsset, new string[] { "Exclude" });
        }

        private static string LoadTemplate(TemplateType type)
        {
            try
            {
                string data = string.Empty;
                string path = TemplatesPath + type + ".txt";
                StreamReader theReader = new StreamReader(path, Encoding.Default);
                using (theReader)
                {
                    data = theReader.ReadToEnd();
                    theReader.Close();
                }

                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}\n", e.Message);
                return string.Empty;
            }
        }
    }
}