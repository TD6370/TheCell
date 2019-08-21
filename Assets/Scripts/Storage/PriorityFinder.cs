using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

//[CreateAssetMenu(fileName = "PriorityFinder", menuName = "Create Priority Finder...", order = 51)]
public class PriorityFinder : ScriptableObject
{
    [MenuItem("Tools/CustomTool/Create Priority Finder")]
    static void DoIt()
    {
        EditorUtility.DisplayDialog("Create Priority Finder", "no OK", "Create", "");
    }
    [SerializeField]
    public SaveLoadData.TypePrefabs TypeObserver;

    [SerializeField]
    public List<SaveLoadData.TypePrefabs> PrioritysTypeModel;
    [SerializeField]
    public List<PoolGameObjects.TypePoolPrefabs> PrioritysTypePool;
    [SerializeField]
    public List<TypesBiomNPC> PrioritysTypeBiomNPC;

    public SaveLoadData.TypePrefabs CurrentTypeModel;
    public PoolGameObjects.TypePoolPrefabs CurrentTypePool;
    public TypesBiomNPC CurrentTypeNPC;
    //public PriorityFinder() { }
}

public class ContainerPrioritys
{
    public List<PriorityFinder> PrioritysNPC;

    public ContainerPrioritys() { }
}


public enum TypesBiomNPC
{
    None,
    Red,
    Blue,
    Green,
    Violet,
}