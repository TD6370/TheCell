using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNPC : MonoBehaviour {

    //Coroutine createObject;
    public GameObject prefabUfo;
    public Camera MainCamera;

    private GenerateGridFields _scriptGrid;
    private int m_LimitUfo = 10;

    void Start()
    {
        //StartCoroutine(CreateObjectUfo());
    }

    void Awake()
    {
        var camera = MainCamera;
        if (camera == null)
        {
            Debug.Log("MainCamera null");
            return;
        }
        _scriptGrid = MainCamera.GetComponent<GenerateGridFields>();
    }

    public void SartCrateNPC()
    {
        //Debug.Log(".............SartCrateNPC -- CreateObjectUfo()");
        StartCoroutine(CreateObjectUfo());
    }

    [ExecuteInEditMode]
    IEnumerator CreateObjectUfo()
    {
        int coutUfoReal = 0;

        bool isTest = false;

        while (true)
        {
            GameObject[] listPrefabUfo = GameObject.FindGameObjectsWithTag("PrefabUfo");
            coutUfoReal = listPrefabUfo.Length;
            //Debug.Log("Count Ufo Real =" + coutUfoReal.ToString() + " < " + m_LimitUfo);//Дебаг

            if (coutUfoReal < m_LimitUfo && !isTest)
            {
                if (coutUfoReal == 0) coutUfoReal = 2;

                GameObject newUfo = (GameObject)Instantiate(prefabUfo);
                int add = (coutUfoReal * 1);
                newUfo.name = "PrefabUfo";
                newUfo.transform.position = new Vector3(prefabUfo.transform.position.x, prefabUfo.transform.position.y - add, -1);

                _scriptGrid.ActiveGameObject(newUfo);
            }
            else
            {
                //Debug.Log("Count Ufo Real =" + coutUfoReal.ToString());
            }
            yield return new WaitForSeconds(3);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
