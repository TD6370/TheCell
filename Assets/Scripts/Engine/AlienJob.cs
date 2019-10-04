using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Custom Tool/Create Alien Job %&j", fileName = "AlienJob_")]
[SerializeField]
public class AlienJob : ScriptableObject
{
    [SerializeField]
    public TypesJobs Job;
    [SerializeField]
    public SaveLoadData.TypePrefabs TargetResource = SaveLoadData.TypePrefabs.PrefabField;
    [SerializeField]
    public SaveLoadData.TypePrefabs ResourceResult = SaveLoadData.TypePrefabs.PrefabField;
    public TypesJobTo JobTo;
    [SerializeField]
    public int LimitResourceCount;
}