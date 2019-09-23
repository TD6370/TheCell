using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Custom Tool/Create/Settings portals", fileName = "SettingsPortals")]
public class SettingsPortals : ScriptableObject
{
    [SerializeField]
    public int StartLimitNPC;
    [SerializeField]
    public float PeriodIncubation;
}