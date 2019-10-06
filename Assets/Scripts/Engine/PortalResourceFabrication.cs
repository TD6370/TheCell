using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Custom Tool/Create/Portal Resource Fabrication %&f", fileName = "Fabrication_")]
public class PortalResourceFabrication : ScriptableObject
{
    [SerializeField]
    public TypesBiomNPC TypeBiom;
    [SerializeField]
    public SaveLoadData.TypeInventoryObjects ResouceInventory;
    [SerializeField]
    public int LimitToBeginProcess;
    [SerializeField]
    public ManagerPortals.TypeResourceProcess BeginProcess;
    [SerializeField]
    public SaveLoadData.TypeInventoryObjects SpawnResourceName;
    [SerializeField]
    public int LimitStorage;

}