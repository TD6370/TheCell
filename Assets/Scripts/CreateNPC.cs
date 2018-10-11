using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNPC : MonoBehaviour {

    //Coroutine createObject;
    public GameObject prefabUfo;
    //@ST@ public Camera MainCamera;

    private GenerateGridFields _scriptGrid;
    private int m_LimitUfo = 0;//100;
    private float _periodCreateNPC = 2;//3;

    void Start()
    {
        //StartCoroutine(CreateObjectUfo());

        _scriptGrid = GetComponent<GenerateGridFields>();
    }

    void Awake()
    {
        //@ST@
        //var camera = MainCamera;
        //if (camera == null)
        //{
        //    Debug.Log("MainCamera null");
        //    return;
        //}
        //_scriptGrid = MainCamera.GetComponent<GenerateGridFields>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SartCrateNPC()
    {
        //Debug.Log(".............SartCrateNPC -- CreateObjectUfo()");
        StartCoroutine(CreateObjectUfo());
        //TestCreateObjectUfo();
    }

    [ExecuteInEditMode]
    IEnumerator CreateObjectUfo()
    {
        int coutUfoReal = 0;

        bool isTest = false;

        while (true)
        {
            //GameObject[] listPrefabUfo = GameObject.FindGameObjectsWithTag("PrefabUfo");
            //coutUfoReal = listPrefabUfo.Length;
            //Debug.Log("Count Ufo Real =" + coutUfoReal.ToString() + " < " + m_LimitUfo);//Дебаг

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

                if (Storage.Instance.IsValidPiontInZona(pos.x, pos.y))
                {
                    GameObject newUfo = (GameObject)Instantiate(prefabUfo);
                    int add = (coutUfoReal * 1);

                    string id = System.Guid.NewGuid().ToString().Substring(1, 4);
                    newUfo.name = "PrefabUfo_" + id;

                    Debug.Log("CREATE NEW UFO :" + newUfo.name);


                    newUfo.transform.position = pos;

                    _scriptGrid.ActiveGameObject(newUfo);
                }
                else
                {
                    //Debug.Log("CreateObjectUfo not create Ufo in zona....");
                }
            }
            yield return new WaitForSeconds(_periodCreateNPC);
        }
    }

    private void TestCreateObjectUfo()
    {
        GameObject newUfo = (GameObject)Instantiate(prefabUfo);
        
        string id = System.Guid.NewGuid().ToString().Substring(1, 4);
        newUfo.name = "PrefabUfo_" + id;
        Debug.Log("CREATE NEW UFO :" + newUfo.name);
        var pos = new Vector3(prefabUfo.transform.position.x, prefabUfo.transform.position.y - 6, -1);
        newUfo.transform.position = pos;

        _scriptGrid.ActiveGameObject(newUfo);
    }
	

}
