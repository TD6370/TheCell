using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

public static class ScriptableObjectUtility
{
    public static AssetBundle loadedAssetBundle;

    private static string m_name = "bundlecell";
    public static string AssetBundleDirectory = "Assets/AssetBundles";

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

        string assetPathAndName = AssetBundleDirectory +  "/Asset" + typeof(T).ToString() + ".asset";

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif

    public static ContainerPriorityFinder LoadContainerPriorityFinderByResource
    {
        get {
            string strErr = "0";
            try
            {
                ContainerPriorityFinder container = null;
                
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


    public static void LoadAssetBundleCell()
    {
      
        string strErr = "0";
        try
        {
            ContainerPriorityFinder container = null;

            if (loadedAssetBundle == null)
            {
                Debug.Log("load AssetBundle... " + m_name);
                AssetBundle.UnloadAllAssetBundles(false);
                loadedAssetBundle = AssetBundle.LoadFromFile(AssetBundleDirectory + "/" + m_name);
            }
            if (loadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle! ");
            }
        }
        catch (Exception ex)
        {
            Debug.Log(Storage.EventsUI.ListLogAdd = "###(" + strErr + ") : " + ex.Message);
        }
    }

    public static ContainerPriorityFinder LoadContainerPriorityFinder
    {
        get
        {
            string strErr = "0";
            try
            {
                ContainerPriorityFinder container = LoadContainerPriorityFinderByTag("NPC");
                return container;
            }
            catch (Exception ex)
            {
                Debug.Log(Storage.EventsUI.ListLogAdd = "###(" + strErr + ") : " + ex.Message);
            }
            return null;
        }
    }

    //Legacy code
    public static ContainerPriorityFinder LoadContainerPriorityFinder_
    {
        get
        {
            string strErr = "0";
            try
            {
                ContainerPriorityFinder container = null;

                if (loadedAssetBundle == null)
                {
                    Debug.Log("load AssetBundle... " + m_name);
                    AssetBundle.UnloadAllAssetBundles(false);
                    loadedAssetBundle = AssetBundle.LoadFromFile(AssetBundleDirectory + "/" + m_name);
                }
                if (loadedAssetBundle == null)
                {
                    Debug.Log("Failed to load AssetBundle! ");
                    return null;
                }
                var bundle = loadedAssetBundle;

                ContainerPriorityFinder containerObj = null;
                ContainerPriorityFinder[] containerObjs = bundle.LoadAllAssets<ContainerPriorityFinder>();

                if (containerObjs == null)
                    Debug.Log("containerObjs - null");
                if (containerObjs != null && containerObjs.Length == 0)
                    Debug.Log("containerObjs count = 0");

                if (containerObjs != null && containerObjs.Length != 0)
                    containerObj = containerObjs[0];

                if (containerObj != null)
                    Debug.Log("ContainerPriorityFinder >> " + containerObj.name);

                container = containerObj as ContainerPriorityFinder;
                if (container == null)
                {
                    Debug.Log("Failed to load ContainerPriorityFinder ! ");
                }

                return container;
            }
            catch (Exception ex)
            {
                Debug.Log(Storage.EventsUI.ListLogAdd = "###(" + strErr + ") : " + ex.Message);
            }
            return null;
        }
    }

    public static ContainerPriorityFinder LoadContainerPriorityFinderByTag(string tag)
    {

        string strErr = "0";
        try
        {
            ContainerPriorityFinder container = null;

            if (loadedAssetBundle == null)
            {
                Debug.Log("load AssetBundle... " + m_name);
                AssetBundle.UnloadAllAssetBundles(false);
                loadedAssetBundle = AssetBundle.LoadFromFile(AssetBundleDirectory + "/" + m_name);
            }
            if (loadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle! ");
                return null;
            }
            var bundle = loadedAssetBundle;

            ContainerPriorityFinder containerObj = null;
            ContainerPriorityFinder[] containerObjs = bundle.LoadAllAssets<ContainerPriorityFinder>();

            if (containerObjs == null)
                Debug.Log("containerObjs - null");
            if (containerObjs != null && containerObjs.Length == 0)
                Debug.Log("containerObjs count = 0");

            if (containerObjs != null && containerObjs.Length != 0)
            {
                containerObj = containerObjs.Where(p => p.Tag == tag).FirstOrDefault();
            }

            if (containerObj != null)
                Debug.Log("ContainerPriorityFinder >> " + containerObj.name + " by Tag = " + tag);

            container = containerObj as ContainerPriorityFinder;
            if (container == null)
            {
                Debug.Log("Failed to load ContainerPriorityFinder ! ");
            }

            return container;
        }
        catch (Exception ex)
        {
            Debug.Log(Storage.EventsUI.ListLogAdd = "###(" + strErr + ") : " + ex.Message);
        }
        return null;
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
        CreateAsset<ContainerPriorityFinder>();
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
        
        if (ScriptableObjectUtility.loadedAssetBundle == null)
        {
            Debug.Log("load AssetBundle... " + m_name);
            AssetBundle.UnloadAllAssetBundles(false);
            loadedAssetBundle = AssetBundle.LoadFromFile(AssetBundleDirectory + "/" + m_name);
        }
        if (ScriptableObjectUtility.loadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle! ");
            return;
        }
        var bundle = loadedAssetBundle;
        var namesAssets = bundle.GetAllAssetNames();
        if (namesAssets.Length == 0)
            Debug.Log("loadedAssetBundle.AllAssetNames = 0");
        //string assetFirts = "";
        //foreach (var nameAss in namesAssets)
        //{
        //    assetFirts = nameAss;
        //    Debug.Log(nameAss);
        //    break;
        //}
        //if(bundle.Contains(assetFirts))
        //{
        //    Debug.Log("Exist assetFirts = " + assetFirts);
        //}

        ContainerPriorityFinder containerObj = null;
        ContainerPriorityFinder[] containerObjs = bundle.LoadAllAssets<ContainerPriorityFinder>();

        if (containerObjs == null)
            Debug.Log("containerObjs - null");
        if (containerObjs != null && containerObjs.Length == 0)
            Debug.Log("containerObjs count = 0");

        if (containerObjs != null && containerObjs.Length != 0)
            containerObj = containerObjs[0];

        if (containerObj != null)
            Debug.Log("ContainerPriorityFinder >> " + containerObj.name);

        ContainerPriorityFinder container = containerObj as ContainerPriorityFinder;
        if (container == null)
        {
            Debug.Log("Failed to load ContainerPriorityFinder ! ");
            return;
        }
        Debug.Log("Load ContainerPriorityFinder OK ! size :" + container.CollectionPriorityFinder.Length);
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
