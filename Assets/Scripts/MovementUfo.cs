using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUfo : MonoBehaviour {

    Coroutine moveObject;

	// Use this for initialization
	void Start () {
	    //Coroutine moveObject =StartCoroutine(Move

        StartCoroutine(ChangeColor());
        
        moveObject = StartCoroutine(MoveObject());
	}

    IEnumerator ChangeColor(){
        Material material = this.GetComponent<Renderer>().material;
        while(true){
                material.color = new Color(Random.value, Random.value, Random.value, 1);
                yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator MoveObject()
    {
        int distantion = 0;
        float x = transform.position.x;
        int compas=1;

        while (true)
        {
            distantion++;
            if (distantion > 200)
            {
                distantion = 0;
                compas *= -1;
                //yield break;
            }

            transform.Translate(compas * Time.deltaTime, 0, 0);
            //transform.Translate(1, 0, 0);
            yield return null;
            //yield return new WaitForSeconds(1);
        }
    }
	
	// Update is called once per frame
	void Update () {
	}


}
