using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private SerializedProperty IsShowTittleInfoPerson;
    private SerializedProperty IsShowTargetRayPerson;

    private SerializedObject newSO;

    private float m_timeUpdateUI = 3f;
    private int m_xOut = 0;
    private int m_yOut = 0;
    private string m_childInfo = string.Empty;

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
        IsShowTittleInfoPerson = newSO.FindProperty("IsShowTittleInfoPerson");
        IsShowTargetRayPerson = newSO.FindProperty("IsShowTargetRayPerson");

        ResetPropertyEditor();
    }

    private void ResetPropertyEditor()
    {
        //SettingsEditor = Editor.CreateEditor(SettingsScene.objectReferenceValue);

    }

    private int m_portalIndexInfo;
    private ModelNPC.PortalData m_portalNext;

    public override void OnInspectorGUI()
    {
        var sceneDebug = (SceneDebuger)target;
        if (sceneDebug == null)
        {
            EditorGUILayout.HelpBox("Property: SceneDebuger is empty! 1", MessageType.Error);
            return;
        }
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
        IsShowTittleInfoPerson.boolValue = EditorGUILayout.Toggle("Show tittle Person Info action", IsShowTittleInfoPerson.boolValue);
        IsShowTargetRayPerson.boolValue = EditorGUILayout.Toggle("Show target ray Person", IsShowTargetRayPerson.boolValue);

        EditorGUILayout.Space();
        //@@$$
        //AutoRefreshOn.boolValue = EditorGUILayout.Toggle("Auto Refresh", AutoRefreshOn.boolValue);

        EditorGUI.BeginChangeCheck(); //@@$$
        AutoRefreshOn.boolValue = EditorGUILayout.Toggle("Auto Refresh", AutoRefreshOn.boolValue);
        if (EditorGUI.EndChangeCheck()) {
            Storage.SceneDebug.DialogsClear();
        }

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
        EditorGUILayout.LabelField("Count DreamWork's: ",
            string.Format("{0}/{1}/{2}", sceneDebug.InfoCount, sceneDebug.LivePersonsCount, sceneDebug.LivePersonsStartCount));

        EditorGUILayout.Space();

        
        if (Storage.Instance != null && Storage.PortalsManager != null && Storage.PortalsManager.Portals.Count >0)
        {
            if (m_timeUpdateUI < Time.time || m_portalNext == null)
            {
                m_timeUpdateUI = Time.time + 3f;
                if (m_portalIndexInfo >= Storage.PortalsManager.Portals.Count())
                    m_portalIndexInfo = 0;
                m_portalNext = Storage.PortalsManager.Portals[m_portalIndexInfo];
                m_portalIndexInfo++;
            }

            Helper.GetFieldPositByWorldPosit(ref m_xOut, ref m_yOut, m_portalNext.Position);
            m_childInfo = (m_portalNext.ChildrensId == null) ? "..." : m_portalNext.ChildrensId.Count.ToString();
            EditorGUILayout.LabelField("PORTAL: ", m_portalNext.TypeBiom.ToString());
            //GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("   field: ", String.Format("{0}x{1}", m_xOut, m_yOut));
            EditorGUILayout.LabelField("   NPC: ", m_childInfo);
            //GUILayout.EndHorizontal();
            EditorGUILayout.LabelField("RESOURCES: ");
            if (m_portalNext.Resources != null) {
                foreach(var res in m_portalNext.Resources)  {
                    EditorGUILayout.LabelField("   " + res.NameInventopyObject + ":", res.Count.ToString());
                }
            }
        }
        //}
        
        
        //EditorGUI.BeginDisabledGroup(true);
        //SettingsEditor.OnInspectorGUI(); //<<<<
        //EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndVertical();

        newSO.ApplyModifiedProperties();

        //@@$$
        //if(!AutoRefreshOn.boolValue && stateAutoRefresh != AutoRefreshOn.boolValue)
        //    Storage.SceneDebug.DialogsClear();
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
