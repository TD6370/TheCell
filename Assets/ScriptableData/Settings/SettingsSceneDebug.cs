using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Custom Tool/SceneDebug/Create Settings Scene Debug", fileName = "SettingsSceneDebug")]
public class SettingsSceneDebug : ScriptableObject {

    [SerializeField, Tooltip("Show tittle scene perons")]
    public bool IsShowTittlePerson = false;
    [SerializeField, Tooltip("Write Debug in log list")]
    public bool IsLog = false;

    [SerializeField, Tooltip("Enable auto debug screen dialogs")]
    public bool AutoRefreshOn = false;
    [SerializeField, Tooltip("Include in debug screen Reals gobjects")]
    public bool RealDebugOn = false;
    
    //[Range(1, 10)]
    [SerializeField, Range(1, 10), Tooltip("Speed move persons in dream")]
    public float SpeedMovePersonInDream = 1f;
    //[Range(0, 1)]
    [SerializeField, Range(0, 1), Tooltip("Time delay searching info actions")]
    public float WaitTimeReaderScene = 0.5f;

    //[Range(0.3f, 10)]
    [SerializeField, Range(0.3f, 10), Tooltip("Time deley refresh draw screen")]
    public float TimeRefreshDebugScene = 1;

    [SerializeField, Tooltip("Enable clear templates")]
    public bool IsClearTemplate = false;
    //[Range(1, 10)]
    [SerializeField, Range(1, 10), Tooltip("Set delay time clear template debug dialogs")]
    public float TimeClearTemplate = 3f;

    //[SerializeField, Tooltip("Time deley refresh screen"]
    //public static bool IsDebugOn = false;
}


//[CreateAssetMenu(menuName = "Custom Tool/SceneDebug/Create Settings Scene Debug", fileName = "SettingsSceneDebug")]
/*
[System.Serializable]
public class SettingsSceneDebug //: ScriptableObject
{

    [SerializeField, Tooltip("Show tittle scene perons")]
    public bool IsShowTittlePerson = false;
    [SerializeField, Tooltip("Write Debug in log list")]
    public bool IsLog;

    //[SerializeField, Tooltip("Time deley refresh screen"]
    //public static bool IsDebugOn = false;
    [SerializeField, Tooltip("Enable auto debug screen dialogs")]
    public bool AutoRefreshOn = false;
    [SerializeField, Tooltip("Include in debug screen Reals gobjects")]
    public bool RealDebugOn = false;

    //[Range(1, 10)]
    [SerializeField, Range(1, 10), Tooltip("Speed move persons in dream")]
    public float SpeedMovePersonInDream = 1f;
    //[Range(0, 1)]
    [SerializeField, Range(0, 1), Tooltip("Time delay searching info actions")]
    public float WaitTimeReaderScene = 0.5f;

    //[Range(0.3f, 10)]
    [SerializeField, Range(0.3f, 10), Tooltip("Time deley refresh draw screen")]
    public float TimeRefreshDebugScene = 1;

    [SerializeField, Tooltip("Enable clear templates")]
    public bool IsClearTemplate = false;
    //[Range(1, 10)]
    [SerializeField, Range(1, 10), Tooltip("Set delay time clear template debug dialogs")]
    public float TimeClearTemplate = 3f;

}
*/
