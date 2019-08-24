using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditPriorityFinderController : MonoBehaviour {


    //public PriorityFinder PriorityFinderCurrent;
    public ContainerPriorityFinder ContainerPriority;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


#if UNITY_EDITOR
    //[MenuItem("Assets/Create/Localization Data")]
    [MenuItem("Tools/Custom Tool/Create asset PriorityFinder")]
    public static void CreatePriorityFinderDataAsset()
    {
        SaveLoadData.TypePrefabs typeModel = SaveLoadData.TypePrefabs.PrefabField;

        var selectionPath = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(selectionPath))
        {
            selectionPath = Application.dataPath;
        }

        var path = EditorUtility.SaveFilePanelInProject(
                                         "Create Priority Finder Data",
                                         "PriorityFinder" + typeModel.ToString(),
                                         "Asset not create",
                                         string.Empty,
                                         selectionPath);

        if (path.Length > 0)
        {
            var asset = ScriptableObject.CreateInstance<ContainerPriorityFinder>();

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
#endif
}
