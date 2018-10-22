using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoragePerson : MonoBehaviour {

    public Color ColorSelectedCursorObject = Color.cyan;
    public Color ColorFindCursorObject = Color.magenta;

    public static string _Ufo { get { return SaveLoadData.TypePrefabs.PrefabUfo.ToString(); } }
    public static string _Boss { get { return SaveLoadData.TypePrefabs.PrefabBoss.ToString(); } }

    //public Vector3 PersonsTargetPosition { get; set; }

    private SaveLoadData.LevelData _personsData;
    public SaveLoadData.LevelData PersonsData
    {
        get { return _personsData; }
    }

    public void PersonsDataInit(SaveLoadData.LevelData _newData = null)
    {
        if (_newData == null)
            _personsData = new SaveLoadData.LevelData();
        else
            _personsData = _newData;
    }

    void Awake()
    {
        PersonsDataInit();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public IEnumerable<GameObject> GetAllRealPersons()
    {
        return Storage.Instance.GamesObjectsReal.
                SelectMany(x => x.Value).
                Where(p => p.tag == _Ufo || p.tag == _Boss).ToList();
    }

    public IEnumerable<GameObject> GetAllRealPersons(string field)
    {
        //var count1= Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).ToList().Count();
        //Debug.Log("PERSON PAIR (" + field + ")  COUNT " + count1);

        foreach (GameObject gobjItem in Storage.Instance.GamesObjectsReal.
            Where(p => p.Key == field).
            SelectMany(x => x.Value).ToList())
        {
            Debug.Log("OBJECT(" + field + ") : " + gobjItem);
        }

        return Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).
                SelectMany(x => x.Value).
                Where(p => p.tag == _Ufo || p.tag == _Boss).ToList();
    }

    public List<SaveLoadData.TypePrefabs> TypesPersons
    {
        get
        {
            return new List<SaveLoadData.TypePrefabs>()
                {
                     SaveLoadData.TypePrefabs.PrefabUfo,
                     SaveLoadData.TypePrefabs.PrefabBoss
                };
        }
    }

    public List<string> NamesPersons
    {
        get
        {
            return new List<string>()
                {
                     SaveLoadData.TypePrefabs.PrefabUfo.ToString(),
                     SaveLoadData.TypePrefabs.PrefabBoss.ToString()
                };
        }
    }

    public void SelectedID(string gobjID)
    {

    }

    public void VeiwCursorGameObjectData(string _fieldCursor)
    {
        //Storage.Events.ListLogClear();
        GameObject prefabFind = Storage.Instance.Fields[_fieldCursor];

        if (prefabFind != null)
        {
            prefabFind.gameObject.GetComponent<SpriteRenderer>().color = ColorSelectedCursorObject;
        }

        foreach (var gobj in Storage.Person.GetAllRealPersons(_fieldCursor))
        {
            Storage.Events.ListLogAdd = "FIND (" + _fieldCursor + "): " + gobj.name;

            gobj.GetComponent<SpriteRenderer>().color = ColorFindCursorObject;

            MovementNPC movement = gobj.GetComponent<MovementNPC>();
            SaveLoadData.ObjectData findData = movement.GetData();
            var objData = SaveLoadData.FindObjectData(gobj);
            if (findData != objData)
            {
                Storage.Events.ListLogAdd = "#### " + gobj.name + " conflict DATA";
                Debug.Log("#### " + gobj.name + " conflict DATA");
            }

            var dataNPC = findData as SaveLoadData.GameDataNPC;
            if (dataNPC != null)
            {
                Storage.Events.ListLogAdd = "VeiwCursorGameObjectData: " + gobj.name + " NPC Params: " + dataNPC.GetParamsString;
                Debug.Log("VeiwCursorGameObjectData: " + gobj.name + " NPC Params: " + dataNPC.GetParamsString);

                //#EXPAND
                Storage.Events.AddExpand(dataNPC.NameObject,
                    dataNPC.GetParams,
                    new List<string> { "Pause", "Kill", "StartTrack" },
                    gobjObservable: gobj);
            }
            else
            {
                Debug.Log("VeiwCursorGameObjectData: " + gobj.name + "  Not is NPC");
            }


            var dataBoss = findData as SaveLoadData.GameDataBoss;
            if (dataBoss != null)
            {
                Storage.Events.ListLogAdd = "YES GameDataBoss " + gobj.name + " SaveLoadData.GameDataBoss is EMPTY ";
                Debug.Log("YES GameDataBoss " + gobj.name + " SaveLoadData.GameDataBoss is EMPTY ");
                dataBoss.ColorRender = Color.magenta;

                //#EXPAND
                Storage.Events.AddExpand(dataBoss.NameObject,
                    dataBoss.GetParams,
                    new List<string> { "Pause", "Kill", "StartTrack" },
                    gobjObservable: gobj);
            }
            else
            {
                if (gobj.tag == _Boss)
                {
                    Storage.Events.ListLogAdd = "#### " + gobj.name + " SaveLoadData.GameDataBoss is EMPTY ";
                    Debug.Log("#### " + gobj.name + " SaveLoadData.GameDataBoss is EMPTY ");
                }
            }

        }
    }

    public void SetTartgetPositionAll(Vector2 posCursorToField)
    {
        Debug.Log("SetTartgetPositionAll : to " + posCursorToField.x + "" + posCursorToField.y);

        foreach (var gobj in Storage.Person.GetAllRealPersons())
        {
            Debug.Log("SetTartgetPositionAll : " + gobj.name + " to : " + posCursorToField.x + "" + posCursorToField.y);
            MovementNPC movem = gobj.GetComponent<MovementNPC>();
            SaveLoadData.GameDataNPC dataNPC = movem.GetData();
            dataNPC.SetTargetPosition(posCursorToField);
        }
    }
}

public static class PersonsExtensions
{
    public static bool IsPerson(this string typePrefab)
    {
        return Storage.Person.NamesPersons.Contains(typePrefab);
    }

    public static MovementUfo GetMoveUfo(this GameObject gobj)
    {
        var moveUfo = gobj.GetComponent<MovementUfo>();
        if (moveUfo != null)
            return moveUfo;
        return null;
    }

    public static MovementNPC GetMoveNPC(this GameObject gobj)
    {
        var moveNPC = gobj.GetComponent<MovementNPC>();
        if (moveNPC != null)
            return moveNPC;
        return null;
    }
}
