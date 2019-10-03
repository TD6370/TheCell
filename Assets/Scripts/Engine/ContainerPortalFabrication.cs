using UnityEngine;
using UnityEditor;
[CreateAssetMenu(menuName = "Custom Tool/Create/Container Fabrications", fileName = "ContainerFabrications")]
//[Serializable]
public class ContainerPortalFabrication : ScriptableObject
{
    [SerializeField]
    public PortalResourceFabrication[] CollectionResourceFabrication;
    [SerializeField]
    public string Tag;
}

