using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(SODataBehaviour))]
//public class SODataBehaviourEditor : Editor {

#if UNITY_EDITOR

[CustomEditor(typeof(SceneDebuger))]
[CanEditMultipleObjects]
public class SceneDebugerEditor : Editor
{
    private SerializedProperty SettingsScene;
    private Editor SettingsEditor;

    private SerializedProperty IsShowTittlePerson;
    public SerializedProperty IsLog;
    private SerializedProperty AutoRefreshOn;
    private SerializedProperty RealDebugOn;
    private SerializedProperty SpeedMovePersonInDream;
    private SerializedProperty TimeRelax;
    private SerializedProperty TimeRefreshDebugScene;
    private SerializedProperty IsClearTemplate;
    private SerializedProperty TimeClearTemplate;
    private SerializedProperty LivePersonsCount;
    private SerializedProperty TimeWorkAction;
    private SerializedProperty TimeLimitResetNavigator;

    private SerializedObject newSO;

    void OnEnable()
    {
        LivePersonsCount = serializedObject.FindProperty("LivePersonsCount");

        SettingsScene = serializedObject.FindProperty("SettingsScene");
        newSO = new SerializedObject(SettingsScene.objectReferenceValue);
        newSO.ApplyModifiedProperties();

        IsShowTittlePerson = newSO.FindProperty("IsShowTittlePerson");
        IsLog = newSO.FindProperty("IsLog");
        AutoRefreshOn = newSO.FindProperty("AutoRefreshOn");
        RealDebugOn = newSO.FindProperty("RealDebugOn");
        SpeedMovePersonInDream = newSO.FindProperty("SpeedMovePersonInDream");
        TimeRelax = newSO.FindProperty("TimeRelax");
        TimeRefreshDebugScene = newSO.FindProperty("TimeRefreshDebugScene");
        IsClearTemplate = newSO.FindProperty("IsClearTemplate");
        TimeClearTemplate = newSO.FindProperty("TimeClearTemplate");
        TimeWorkAction = newSO.FindProperty("TimeWorkAction");
        TimeLimitResetNavigator = newSO.FindProperty("TimeLimitResetNavigator");

        ResetPropertyEditor();
    }

    private void ResetPropertyEditor()
    {
        //SettingsEditor = Editor.CreateEditor(SettingsScene.objectReferenceValue);

    }

    public override void OnInspectorGUI()
    {
        var sceneDebug = (SceneDebuger)target;
        if (sceneDebug == null)
        {
            EditorGUILayout.HelpBox("Property: SceneDebuger is empty! 1", MessageType.Error);
            return;
        }


        bool stateAutoRefresh = AutoRefreshOn.boolValue;

        newSO.Update();

        //EditorGUI.BeginChangeCheck();
        //DrawDefaultInspector();
        //if (EditorGUI.EndChangeCheck()) {
        //    serializedObject.ApplyModifiedProperties(); // поскольку изменили значение PropertyField (scriptableObjectData) 
        //                                                // нужно применить изменения и только после этого
        //                                                // перезагрузить Editor scriptableObjectData
        //    ResetPropertyEditor();
        //}

        GUILayout.BeginVertical();
        EditorGUILayout.Space();

        if (IsLog == null)
        {
            EditorGUILayout.HelpBox("Property: IsLog is empty! 1", MessageType.Error);
            return;
        }
        if (LivePersonsCount == null)
        {
            EditorGUILayout.HelpBox("Property: LivePersonsCount is empty!", MessageType.Error);
            return;
        }


        IsLog.boolValue = EditorGUILayout.Toggle("Show log actions", IsLog.boolValue);
        IsShowTittlePerson.boolValue = EditorGUILayout.Toggle("Show tittle Person", IsShowTittlePerson.boolValue);

        EditorGUILayout.Space();
        AutoRefreshOn.boolValue = EditorGUILayout.Toggle("Auto Refresh", AutoRefreshOn.boolValue);
        RealDebugOn.boolValue = EditorGUILayout.Toggle("Real actions debug", RealDebugOn.boolValue);

        EditorGUILayout.Space();
        SpeedMovePersonInDream.floatValue = EditorGUILayout.Slider("Speed move Person in Dream",SpeedMovePersonInDream.floatValue, 1, 10);
        TimeRelax.floatValue = EditorGUILayout.Slider("Time of Relax", TimeRelax.floatValue, 0, 1);
        TimeRefreshDebugScene.floatValue = EditorGUILayout.Slider("Delay update screen actions", TimeRefreshDebugScene.floatValue, 0.1f, 10);

        EditorGUILayout.Space();
        TimeWorkAction.floatValue = EditorGUILayout.Slider("Time working", TimeWorkAction.floatValue, 1, 60);
        TimeLimitResetNavigator.floatValue = EditorGUILayout.Slider("Dispatcher time reset", TimeLimitResetNavigator.floatValue, 1, 60);

        EditorGUILayout.Space();
        IsClearTemplate.boolValue = EditorGUILayout.Toggle("Clear template", IsClearTemplate.boolValue);
        TimeClearTemplate.floatValue = EditorGUILayout.Slider("Delay clear template dialogs", TimeClearTemplate.floatValue, 1, 10);

        EditorGUILayout.Space();

        //EditorGUILayout.IntField("Count DreamWork's: ", LivePersonsCount.intValue);
        //EditorGUILayout.IntField("Count DreamWork's: ", sceneDebug.LivePersonsCount);
        EditorGUILayout.LabelField("Count DreamWork's: ", sceneDebug.LivePersonsCount.ToString());

        //EditorGUI.BeginDisabledGroup(true);
        //SettingsEditor.OnInspectorGUI(); //<<<<
        //EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndVertical();

        newSO.ApplyModifiedProperties();

        if(!AutoRefreshOn.boolValue && stateAutoRefresh != AutoRefreshOn.boolValue)
            Storage.SceneDebug.DialogsClear();
    }
}

#endif

//IsShowTittlePerson
//        IsLog
//        AutoRefreshOn
//        RealDebugOn
//        SpeedMovePersonInDream
//        WaitTimeReaderScene
//        TimeRefreshDebugScene
//        IsClearTemplate
//        TimeClearTemplate

/*
   public override void OnInspectorGUI()
   {
       //---------------------- 2.
       //var point = PositionsListProp.GetArrayElementAtIndex(i);
       //SettingsScene.GetEndProperty();
       var sceneDebug = (SceneDebuger)target;
       //var settingsSceneData = (SettingsSceneDebug)target;
       //--------------------------


       serializedObject.Update();
       EditorGUI.BeginChangeCheck();
       DrawDefaultInspector();
       if (EditorGUI.EndChangeCheck())
       {
           serializedObject.ApplyModifiedProperties(); // поскольку изменили значение PropertyField (scriptableObjectData) 
                                                       // нужно применить изменения и только после этого
                                                       // перезагрузить Editor scriptableObjectData
           ResetPropertyEditor();
       }

       // Рисуем SODataEditor
       if (SettingsEditor != null)
       {
           GUILayout.BeginVertical(EditorStyles.helpBox);
           //EditorGUI.BeginDisabledGroup(true);
           SettingsEditor.OnInspectorGUI();
           //EditorGUI.EndDisabledGroup();
           EditorGUILayout.EndVertical();
       }
       else
       {
           EditorGUILayout.Separator();
           EditorGUILayout.HelpBox("Settings is empty!", MessageType.Error);
       }

       serializedObject.ApplyModifiedProperties();

   }
   */
