using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNPC : MonoBehaviour {

    //Coroutine createObject;
    public GameObject prefabUfo;
    public Camera MainCamera;

    private GenerateGridFields _scriptGrid;
    private int m_LimitUfo = 100;

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

                //var pos = new Vector3(prefabUfo.transform.position.x, prefabUfo.transform.position.y - add, -1);
                var pos = new Vector3(prefabUfo.transform.position.x, prefabUfo.transform.position.y - 6, -1);
                //var _rb2d = newUfo.GetComponent<Rigidbody2D>();
                //if (_rb2d != null)
                //{
                //    Debug.Log("CreateObjectUfo Set position 3........");
                //    _rb2d.MovePosition(pos);
                //}
                //else
                //{
                //    Debug.Log("CreateObjectUfo Set position 3 original........");
                //    //@!@.3
                    newUfo.transform.position = pos; 
                //}

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
