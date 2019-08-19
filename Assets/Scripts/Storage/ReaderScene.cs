//using UnityEngine;
//using UnityEditor;

using System.Collections.Generic;

public static class ReaderScene 
{
    public static bool IsGridDataFieldExist(string field)
    {
        return Storage.Instance.GridDataG.FieldsD.ContainsKey(field);
    }

    public static List<ModelNPC.ObjectData> GetObjecsDataFromGrid(string nameField)
    {
        //if (!Storage.Instance.IsLoadingWorldThread)
        //{
        //    if (!IsGridDataFieldExist(nameField))
        //        Storage.Data.AddNewFieldInGrid(nameField, "GetObjecsDataFromGrid");
        //}
        return Storage.Instance.GridDataG.FieldsD[nameField].Objects;
    }

    public static ModelNPC.ObjectData GetObjecDataFromGrid(string nameField, int index)
    {
        //if (!Storage.Instance.IsLoadingWorldThread)
        //{
        //    if (!IsGridDataFieldExist(nameField))
        //        Storage.Data.AddNewFieldInGrid(nameField, "GetObjecDataFromGrid");
        //}
        return GetObjecsDataFromGrid(nameField)[index];
    }

    //: UpdateData.
    //[MenuItem("Tools/MyTool/Do It in C#")]
    //static void DoIt()
    //{
    //    EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    //}
}