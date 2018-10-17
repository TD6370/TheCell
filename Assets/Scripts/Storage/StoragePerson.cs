using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoragePerson : MonoBehaviour {

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
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
