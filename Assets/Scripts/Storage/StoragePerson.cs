using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoragePerson : MonoBehaviour {

    public static string _Ufo { get { return SaveLoadData.TypePrefabs.PrefabUfo.ToString(); } }
    public static string _Boss { get { return SaveLoadData.TypePrefabs.PrefabUfo.ToString(); } }

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
        var count1= Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).ToList().Count();
        Debug.Log("PERSON PAIR (" + field + ")  COUNT " + count1);

        var listT1=  Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).ToList();
        foreach(var pair in listT1)
        {
            foreach (var obj in pair.Value)
            {
                Debug.Log("PERSON PAIR (" + field + ") 1.: " + obj.name);
            }
        }

        foreach (var listM in listT1.Select(p => p.Value.Select(c=>c.name))) 
        {
            foreach (var obj in listM)
            {
                Debug.Log("PERSON  VALUES(" + field + ") 1.: " + obj);
            }
            
        }

        var listT2 = listT1.SelectMany(x => x.Value).ToList();

        foreach (var t3 in listT2)
        {
            Debug.Log("PERSON(" + field + ") 2.: " + t3);
        }

        var listT3 = listT2.Where(p => p.tag.ToString() == _Ufo || p.tag.ToString() == _Boss);

        foreach (var t3 in listT3)
        {
            Debug.Log("PERSON(" + field + ") 3.: " + t3);
        }

        return Storage.Instance.GamesObjectsReal.Where(p=> p.Key == field).
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
