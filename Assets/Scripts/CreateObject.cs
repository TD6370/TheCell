using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour {

    //Coroutine createObject;
    public GameObject prefabUfo;
    public Camera MainCamera;

    private GenerateGridFields _scriptGrid;

    void Start()
    {
        StartCoroutine(CreateObjectUfo());

        var camera = MainCamera;
        if (camera == null)
        {
            Debug.Log("MainCamera null");
            return;
        }
        _scriptGrid = MainCamera.GetComponent<GenerateGridFields>();
    }

    [ExecuteInEditMode]
    IEnumerator CreateObjectUfo()
    {
        int coutUfoReal = 0;

        while (true)
        {   
            GameObject[] listPrefabUfo = GameObject.FindGameObjectsWithTag("Ufak") ;//  = gameObject.CompareTag ("Ufak")) ;
            coutUfoReal = listPrefabUfo.Length;
            if(coutUfoReal<10)
            {
                if (coutUfoReal == 0) coutUfoReal = 2;
                //if(listPrefabUfo.Length>0){
                //    otherUfo = listPrefabUfo[0];
                //}
                
                GameObject newUfo = (GameObject)Instantiate(prefabUfo);
                int add = (coutUfoReal * 1);
                //newUfo.transform.position = new Vector3(prefabUfo.transform.position.x, prefabUfo.transform.position.y - add, -1);
                newUfo.transform.position = new Vector3(prefabUfo.transform.position.x, prefabUfo.transform.position.y - add);
                //newUfo.MovePosition(newUfo.position + movement * speed * Time.deltaTime);

                //!!! _scriptGrid.ActiveGameObject(newUfo);

                //print(newUfo.transform.position.ToString()); //Консоль
                //Debug.Log("UFO pos=" + newUfo.transform.position.ToString());//Дебаг
                //Debug.Log("Count Ufo Real =" + coutUfoReal.ToString());//Дебаг
            }
            yield return new WaitForSeconds(3);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
