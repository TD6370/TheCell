﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNPC : MonoBehaviour {

    //Coroutine createObject;
    public GameObject prefabUfo;
    //@ST@ public Camera MainCamera;

    private GenerateGridFields _scriptGrid;
    private int m_LimitUfo = 0;//100;
    private float _periodCreateNPC = 2;//3;

    private Coroutine coroutineCreateObjectUfo;

    void Start()
    {
        //StartCoroutine(CreateObjectUfo());

        _scriptGrid = GetComponent<GenerateGridFields>();
    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SartCrateNPC()
    {

        //Debug.Log(".............SartCrateNPC -- CreateObjectUfo()");

        //#TEST STOP
        //coroutineCreateObjectUfo = StartCoroutine(CreateObjectUfo());
    }

    public void StopCrateNPC()
    {
        if (coroutineCreateObjectUfo != null)
            StopCoroutine(coroutineCreateObjectUfo);
    }

    [ExecuteInEditMode]
    IEnumerator CreateObjectUfo()
    {
        int coutUfoReal = 0;

        bool isTest = false;

        while (true)
        {

            if (Storage.Instance.IsLoadingWorld)
            {
                Debug.Log("_______________ LOADING WORLD ....._______________");
                yield return null;
            }

            if (coutUfoReal < m_LimitUfo && !isTest)
            {
                if (coutUfoReal == 0) coutUfoReal = 2;

                coutUfoReal++; //TEST

                var pos = new Vector3(prefabUfo.transform.position.x, prefabUfo.transform.position.y - 6, -1);
                if (Storage.Instance.ZonaReal == null)
                {
                    Debug.Log("CreateObjectUfo not create Ufo ! ZonaReal not init....");
                    yield return null;
                }

                if (Helper.IsValidPiontInZona(pos.x, pos.y))
                {
                    GameObject newUfo = (GameObject)Instantiate(prefabUfo);
                    //int add = (coutUfoReal * 1);

                    string id = System.Guid.NewGuid().ToString().Substring(1, 4);
                    newUfo.name = "PrefabUfo_" + id;

                    Debug.Log("CREATE NEW UFO :" + newUfo.name);


                    newUfo.transform.position = pos;

                    _scriptGrid.ActiveGameObject_lagacy(newUfo);
                }
                else
                {
                    //Debug.Log("CreateObjectUfo not create Ufo in zona....");
                }
            }
            yield return new WaitForSeconds(_periodCreateNPC);
        }
    }

    //private void TestCreateObjectUfo()
    //{
    //    GameObject newUfo = (GameObject)Instantiate(prefabUfo);
        
    //    string id = System.Guid.NewGuid().ToString().Substring(1, 4);
    //    newUfo.name = "PrefabUfo_" + id;
    //    Debug.Log("CREATE NEW UFO :" + newUfo.name);
    //    var pos = new Vector3(prefabUfo.transform.position.x, prefabUfo.transform.position.y - 6, -1);
    //    newUfo.transform.position = pos;

    //    _scriptGrid.ActiveGameObject(newUfo);
    //}
	

}
