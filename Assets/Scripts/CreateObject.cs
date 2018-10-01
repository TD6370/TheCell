using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour {

    //Coroutine createObject;
    public GameObject prefabUfo;

	// Use this for initialization
	void Start () {
        StartCoroutine(CreateObjectUfo());
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
                newUfo.transform.position = new Vector2(prefabUfo.transform.position.x, prefabUfo.transform.position.y - add);
                //newUfo.MovePosition(newUfo.position + movement * speed * Time.deltaTime);

                //print(newUfo.transform.position.ToString()); //Консоль
                Debug.Log("UFO pos=" + newUfo.transform.position.ToString());//Дебаг
                //Debug.Log("Count Ufo Real =" + coutUfoReal.ToString());//Дебаг
            }
            yield return new WaitForSeconds(3);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
