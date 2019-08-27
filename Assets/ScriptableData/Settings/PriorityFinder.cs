using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using System;

//[CreateAssetMenu(fileName = "PriorityFinder", menuName = "Create Priority Finder...", order = 51)]

//[CreateAssetMenu(menuName = "My Assets/Create Priority Finder")]
[CreateAssetMenu(menuName = "Custom Tool/Create Container Priority", fileName = "ContainerPriorityFinder") ]
public class ContainerPriorityFinder : ScriptableObject
{
    //[MenuItem("Tools/CustomTool/TEST")]
    //static void DoIt()
    //{
    //    EditorUtility.DisplayDialog("TEST", "no OK", "Create", "");
    //}
    [SerializeField]
    public PriorityFinder[] CollectionPriorityFinder;
    [SerializeField]
    public int T2;
}

[CreateAssetMenu(menuName = "Custom Tool/Create Priority Finder", fileName = "Priority_")]
public class PriorityFinder : ScriptableObject
{
    [SerializeField]
    public SaveLoadData.TypePrefabs TypeObserver;

    [SerializeField]
    public List<SaveLoadData.TypePrefabs> PrioritysTypeModelAll;
    [SerializeField]
    public List<SaveLoadData.TypePrefabNPC> PrioritysTypeModelNPC;
    [SerializeField]
    public List<SaveLoadData.TypePrefabFloors> PrioritysTypeModelFloor;
    [SerializeField]
    public List<SaveLoadData.TypePrefabFlore> PrioritysTypeModelFlore;
    [SerializeField]
    public List<SaveLoadData.TypePrefabObjects> PrioritysTypeModelWood;
    [SerializeField]
    public List<SaveLoadData.TypePrefabWall> PrioritysTypeModelWall;
    [SerializeField]
    public List<PoolGameObjects.TypePoolPrefabs> PrioritysTypePool;
    [SerializeField]
    public List<TypesBiomNPC> PrioritysTypeBiomNPC;

    //public SaveLoadData.TypePrefabs CurrentTypeModel;
    //public PoolGameObjects.TypePoolPrefabs CurrentTypePool;
    //public TypesBiomNPC CurrentTypeNPC;

    public List<SaveLoadData.TypePrefabs> GetPrioritysTypeModel()
    {
        List<SaveLoadData.TypePrefabs> getPrioritysTypeModel = new List<SaveLoadData.TypePrefabs>();
        getPrioritysTypeModel.AddRange(PrioritysTypeModelAll);

        foreach (var item in PrioritysTypeModelNPC) {
            var prioriT = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), item.ToString());
            getPrioritysTypeModel.Add(prioriT);
        }
        foreach (var item in PrioritysTypeModelFloor)
        {
            var prioriT = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), item.ToString());
            getPrioritysTypeModel.Add(prioriT);
        }
        foreach (var item in PrioritysTypeModelFlore)
        {
            var prioriT = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), item.ToString());
            getPrioritysTypeModel.Add(prioriT);
        }
        foreach (var item in PrioritysTypeModelWood)
        {
            var prioriT = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), item.ToString());
            getPrioritysTypeModel.Add(prioriT);
        }
        foreach (var item in PrioritysTypeModelWall)
        {
            var prioriT = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), item.ToString());
            getPrioritysTypeModel.Add(prioriT);
        }
               

        return getPrioritysTypeModel;
    }

}

public enum TypesBiomNPC
{
    None,
    Red,
    Blue,
    Green,
    Violet,
}