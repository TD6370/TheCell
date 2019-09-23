using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "Custom Tool/Create/Priority Finder %&h", fileName = "Priority_" )]
//[Serializable]
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
}

public enum TypesBiomNPC
{
    None,
    Red,
    Blue,
    Green,
    Violet,
    Gray
}