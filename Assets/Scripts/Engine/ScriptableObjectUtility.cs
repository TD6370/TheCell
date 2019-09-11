using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public static class ScriptableObjectUtility
{
    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    #if UNITY_EDITOR
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        //string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/Asset" + typeof(T).ToString() + ".asset");
        //string assetPathAndName = CreateAssetBundles.AssetBundleDirectory + "/" + path + "/Asset" + typeof(T).ToString() + ".asset";
        string assetPathAndName = CreateAssetBundles.AssetBundleDirectory +  "/Asset" + typeof(T).ToString() + ".asset";

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    #endif
}


public class CreateAssetBundles
{
    public static AssetBundle loadedAssetBundle;

    public static string AssetBundleDirectory = "Assets/AssetBundles";
    public static ContainerPriorityFinder LoadContainerPriorityFinder
    {
        get {
            string strErr = "0";
            try
            {
                ContainerPriorityFinder container = null;
                //string name = "ContainerPriorityFinder.asset";
                //string pathPriorityContainer = m_assetBundleDirectory + "/ScriptableData/" + name;
                //var loadedAssetBundle = AssetBundle.LoadFromFile(pathPriorityContainer);

                //var loadedAssetBundle = AssetBundle.LoadFromFile(m_assetBundleDirectory);
                //if (loadedAssetBundle != null)
                //    cpf = loadedAssetBundle.LoadAsset<ContainerPriorityFinder>("ContainerPriorityFinder");

                //--------------
                //string name = "ContainerPriorityFinder.asset";
                //string pathPriorityContainer = Path.Combine(AssetBundleDirectory, "ScriptableData");
                //string path1 = Path.Combine(pathPriorityContainer, name);
                //string path = "Assets/AssetBundles/ScriptableData/ContainerPriorityFinder.asset";

                //strErr = "1";
                //AssetBundle loadedAssetBundle = AssetBundle.LoadFromFile(path);
                //if (loadedAssetBundle == null)
                //{
                //    Debug.Log(Storage.EventsUI.ListLogAdd = "Failed to load AssetBundle! " + pathPriorityContainer);
                //    return container;
                //}
                //strErr = "2";
                //container = loadedAssetBundle.LoadAsset<ContainerPriorityFinder>("ContainerPriorityFinder");
                //if (container == null)
                //{
                //    Debug.Log(Storage.EventsUI.ListLogAdd = "Failed to load ContainerPriorityFinder!");
                //    return container;
                //}

                //-------------------
                strErr = "1";
                var findContainerPriorityFinder = Resources.FindObjectsOfTypeAll(typeof(ContainerPriorityFinder));
                strErr = "2";
                if (findContainerPriorityFinder == null)
                    Storage.EventsUI.ListLogAdd = "NOT Finded LoadPriorityPerson !!!";
                else
                    Storage.EventsUI.ListLogAdd = "Finded LoadPriorityPersons count =" + findContainerPriorityFinder.Length;
                strErr = "3";
                foreach (var obj in findContainerPriorityFinder)
                {
                    if(obj == null)
                        Storage.EventsUI.ListLogAdd = "Finded obj is null";
                    else
                        Storage.EventsUI.ListLogAdd = "Finded obj : " + obj.name;

                    container = obj as ContainerPriorityFinder;
                    if(container != null)
                        Storage.EventsUI.ListLogAdd = "Finded LoadPriorityPerson : " + container.name;
                    else
                        Storage.EventsUI.ListLogAdd = "Finded LoadPriorityPerson type NOT parse = " + obj.name;

                    if (container != null && container.CollectionPriorityFinder != null)
                        Storage.EventsUI.ListLogAdd = "Finded LoadPriorityPerson : " + container.name + "  count=" + container.CollectionPriorityFinder.Length;
                    else if(container != null && container.CollectionPriorityFinder == null)
                        Storage.EventsUI.ListLogAdd = "Finded LoadPriorityPerson : " + container.name + "  CollectionPriorityFinder is null";

                    if (container != null && container.CollectionPriorityFinder.Length > 0)
                        break;
                }
                return container;
            }
            catch(Exception ex)
            {
                Debug.Log(Storage.EventsUI.ListLogAdd = "###(" + strErr + ") : " + ex.Message);
            }
            return null;
        }
    }

#if UNITY_EDITOR

    public static void LoadContainerPriorityFinderEditor()
    {
        ContainerPriorityFinder cpf = null;
        try
        {
            string name = "ContainerPriorityFinder.asset";
            string pathPriorityContainer = AssetBundleDirectory + "/ScriptableData/" + name;
            cpf = (ContainerPriorityFinder)AssetDatabase.LoadAssetAtPath(pathPriorityContainer, typeof(ContainerPriorityFinder));
        }
        catch (Exception ex)
        {
            Debug.Log("### LoadContainerPriorityFinder : " + ex.Message);
        }
    }

    //
    [MenuItem("Assets/Data/Create Asset Prioritys finders")]
    static void CreateAssetContainerPriorityFinder()
    {
        ScriptableObjectUtility.CreateAsset<ContainerPriorityFinder>();
    }

    //[CreateAssetMenu(menuName = "Custom Tool/Create Container Priority", fileName = "ContainerPriorityFinder")]
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        if (!Directory.Exists(AssetBundleDirectory))
        {
            Directory.CreateDirectory(AssetBundleDirectory);
        }
        //BuildPipeline.BuildAssetBundles(m_assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        BuildPipeline.BuildAssetBundles(AssetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);
    }


    [MenuItem("Assets/Load Data/Load Prioritys finders")]
    static void LoadPriotitysFinders()
    {
        //Debug.ClearDeveloperConsole();

        //var findContainerPriorityFinder = AssetBundle.FindObjectsOfTypeAll(typeof(ContainerPriorityFinder));
        //foreach (var obj in findContainerPriorityFinder)
        //{
        //    Debug.Log("Finded container: " + obj.name);
        //    var containerFind = obj as ContainerPriorityFinder;
        //    if (containerFind != null)
        //    {
        //        Debug.Log("Finded container: " + obj.name + " count=" + containerFind.CollectionPriorityFinder.Length);
        //    }
        //}

        //return;
        //--------------------

        //string name1 = "ContainerPriorityFinder.asset";
        string name = "bundlecell";
        //string pathPriorityContainer = m_assetBundleDirectory + "/ScriptableData/" + name1;
        //string pathPriorityContainer = m_assetBundleDirectory + "/" + name1;


        //string str1 = Path.Combine(pathPriorityContainer, name);
        //string str = "Assets/AssetBundles/ScriptableData/ContainerPriorityFinder.asset";
        //string str = "Assets/AssetBundles/ContainerPriorityFinder.asset";
        //string str = AssetDatabase.GetAssetPath(Selection.activeObject);

        //AssetBundle.UnloadAllAssetBundles(true);
        if (loadedAssetBundle == null)
        {
            Debug.Log("load AssetBundle... " + name);
            AssetBundle.UnloadAllAssetBundles(false);

            ///AssetBundle loadedAssetBundle = null;
            loadedAssetBundle = AssetBundle.LoadFromFile(AssetBundleDirectory + "/" + name);
            //AssetBundle loadedAssetBundle = AssetBundle.LoadFromFile(str);

            //FIX----
            ///var findContainerPriorityFinder = AssetBundle.FindObjectsOfTypeAll(typeof(ContainerPriorityFinder));
            //----
            //var loadedAssetBundles = AssetBundle.FindObjectsOfTypeAll(typeof(AssetBundle));
        }
        //var loadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "ContainerPriorityFinder"));
        //if (loadedAssetBundle == null)
        //{
        //    Debug.Log("Failed to load AssetBundle!");
        //    return;
        //}

        //AssetBundle loadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/");

        if (loadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle! ");// + pathPriorityContainer);
            return;
        }
        string str1 = "assets/assetbundles/scriptabledata/containerpriorityfinder.asset";
        string str2 = "assets/assetbundles/scriptabledata/containerpriorityfinder";
        string str3 = "scriptabledata/containerpriorityfinder";
        string str4 = "containerpriorityfinder";
        string str5= "containerpriorityfinder.asset";
        string str6 = "ContainerPriorityFinder.asset";
        string str7 = "ContainerPriorityFinder";

        string pathPriorityFinder = str7;
        //string pathPriorityFinder = "Assets/AssetBundles/ScriptableData/ContainerPriorityFinder.asset";
        //string pathPriorityFinder = "Assets/AssetBundles/ScriptableData/ContainerPriorityFinder";
        //string pathPriorityFinder = "ScriptableData/ContainerPriorityFinder";
        //string pathPriorityFinder = "ContainerPriorityFinder.asset";
        //string pathPriorityFinder = AssetDatabase.GetAssetPath(Selection.activeObject);
        //ContainerPriorityFinder container = loadedAssetBundle.LoadAsset<ContainerPriorityFinder>("ContainerPriorityFinder");

        var namesAssets = loadedAssetBundle.AllAssetNames();
        if (namesAssets.Length == 0)
            Debug.Log("loadedAssetBundle.AllAssetNames = 0");
        string assetFirts = "";
        foreach (var nameAss in namesAssets)
        {
            assetFirts = nameAss;
            Debug.Log(nameAss);
            break;
        }
        if(loadedAssetBundle.Contains(assetFirts))
        {
            Debug.Log("Exist assetFirts = " + assetFirts);
            //pathPriorityFinder = assetFirts;
        }

        //var container = loadedAssetBundle.LoadAsset("ContainerPriorityFinder") ;
        //var containerObj = loadedAssetBundle.LoadAsset("ContainerPriorityFinder");
        //var containerObj = loadedAssetBundle.LoadAsset(pathPriorityFinder);
        var containerObj = loadedAssetBundle.LoadAsset<ContainerPriorityFinder>(pathPriorityFinder);
        //var containerObj = loadedAssetBundle.LoadAsset("ContainerPriorityFinder", typeof(ContainerPriorityFinder));
        //var containerObj = loadedAssetBundle.LoadAsset("ContainerPriorityFinder", typeof(ContainerPriorityFinder));
        //var containerObj = loadedAssetBundle.LoadAsset(pathPriorityFinder, typeof(ContainerPriorityFinder));

        //var t1 = loadedAssetBundle.LoadAsset(str1);
        //var t2 = loadedAssetBundle.LoadAsset(str2);
        //var t3 = loadedAssetBundle.LoadAsset(str3);
        //var t4 = loadedAssetBundle.LoadAsset(str4);
        //var t5 = loadedAssetBundle.LoadAsset(str5);
        //var t6 = loadedAssetBundle.LoadAsset(str6);
        //var t7 = loadedAssetBundle.LoadAsset(str7);

        if (containerObj != null)
            Debug.Log("ContainerPriorityFinder >> " + containerObj.name);

        ContainerPriorityFinder container = containerObj as ContainerPriorityFinder;


        //ContainerPriorityFinder container = loadedAssetBundle.LoadAsset<ContainerPriorityFinder>(pathPriorityFinder);
        if (container == null)
        {
            Debug.Log("Failed to load ContainerPriorityFinder ! " + pathPriorityFinder);
            return;
        }
        Debug.Log("Load ContainerPriorityFinder OK ! size :" + container.CollectionPriorityFinder.Length + "   =" + pathPriorityFinder);
    }

    [MenuItem("Assets/Data/Log Prioritys finders")]
    static void LogPriotitysFinders()
    {
        var selectionPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        Debug.Log(selectionPath);
    }

    [MenuItem("Assets/Save Data/Save Prioritys finders")]
    static void SavePriotitysFinders()
    {
        var selectionPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        ContainerPriorityFinder containerPriorityFinder = Selection.activeObject as ContainerPriorityFinder;

        //string assetBundleDirectory = m_assetBundleDirectory + "/ContainerPriorityFinder.asset";
        string assetBundleDirectory = AssetBundleDirectory + "/ContainerPriorityFinder";
        // Create a simple material asset

        //Material material = new Material(Shader.Find("Specular"));
        //ContainerPriorityFinder containerPriorityFinder = Storage.Person.ContainerPrioritys;

        AssetDatabase.CreateAsset(containerPriorityFinder, assetBundleDirectory);

        AssetDatabase.SaveAssets();

        // Add an animation clip to it
        //AnimationClip animationClip = new AnimationClip();
        //animationClip.name = "My Clip";
        //AssetDatabase.AddObjectToAsset(animationClip, material);

        // Reimport the asset after adding an object.
        // Otherwise the change only shows up when saving the project
        //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animationClip));

        // Print the path of the created asset
        //Debug.Log(AssetDatabase.GetAssetPath(material));

        //--------------
        //var asset = ScriptableObject.CreateInstance<LocalizationData>();

        //AssetDatabase.CreateAsset(asset, path);
        //AssetDatabase.SaveAssets();

        //EditorUtility.FocusProjectWindow();

        Selection.activeObject = containerPriorityFinder;
        //---------------------
    }
#endif
}
