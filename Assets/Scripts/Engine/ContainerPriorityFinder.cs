using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(menuName = "Custom Tool/Create/Container Priority", fileName = "ContainerPriorityFinder")]
//[Serializable]
public class ContainerPriorityFinder : ScriptableObject
{
    [SerializeField]
    public PriorityFinder[] CollectionPriorityFinder;
    [SerializeField]
    public string Tag;
}

