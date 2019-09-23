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


[CreateAssetMenu(menuName = "Custom Tool/Create/Container Fabrications", fileName = "ContainerFabrications")]
//[Serializable]
public class ContainerPortalFabrication : ScriptableObject
{
    [SerializeField]
    public PortalResourceFabrication[] CollectionPriorityFinder;
    [SerializeField]
    public string Tag;
}


