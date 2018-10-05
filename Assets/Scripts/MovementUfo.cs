using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUfo : MonoBehaviour {


    Coroutine moveObject;

    Material m_material;
    SpriteRenderer m_spriteRenderer;
    PersonalData m_scriptPersonal;

	// Use this for initialization
	void Start () {
	    //Coroutine moveObject =StartCoroutine(Move
        m_material = this.GetComponent<Renderer>().material;
        m_spriteRenderer = this.GetComponent<SpriteRenderer>();
        m_scriptPersonal = this.GetComponent<PersonalData>();

        //ChangeRandomColor();
        //StartCoroutine(ChangeColor());
        
        //moveObject = StartCoroutine(MoveObject());
        StartCoroutine(MoveObjectToPosition());
        
	}

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator ChangeColor(){
        
        while(true){

            ChangeRandomColor();

            yield return new WaitForSeconds(2f);
        }
    }

    private void ChangeRandomColor()
    {
        //material.color = new Color(Random.value, Random.value, Random.value, 1);
        m_spriteRenderer.color = new Color(Random.value, Random.value, Random.value, 1);
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

    IEnumerator MoveObjectToPosition()
    {
        Vector3 lastPosition = transform.position;
        int stepTest = 0;
        int stepLimitTest = 10;
        float minDist = 0.001f;  //0.01f;

        int speed = 2;
        float step = speed * Time.deltaTime;
        var objUfo = m_scriptPersonal.PersonalObjectData as SaveLoadData.ObjectDataUfo;

        if (objUfo == null)
        {
            Debug.Log("Error UFO MoveObjectToPosition objUfo is Empty !!!!");
            yield break;
        }

        if (objUfo.TargetPosition == new Vector3(0, 0, 0))
        {
            Debug.Log("Error UFO objUfo.TargetPosition is zero !!!!");
            yield break;
        }

        while (true)
        {
            stepTest++;
            if (stepTest > stepLimitTest)
            {
                float distLock = Vector3.Distance(lastPosition, transform.position);
                if (distLock < minDist)
                {
                    //Debug.Log("MoveObjectToPosition ------ UFO LOCK !!!!  > " + distLock);
                    objUfo.SetTargetPosition();
                }
                lastPosition = transform.position;
                stepTest = 0;
            }

            Vector3 targetPosition  = objUfo.TargetPosition;

            // Move our position a step closer to the target.
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);


            float dist = Vector3.Distance(targetPosition, transform.position);
            //Debug.Log("MoveObjectToPosition ------ UFO distance to point  > " + dist);
            if (dist < minDist)
            {
                //Debug.Log("MoveObjectToPosition ------ UFO IN POINT  > " + dist);
                objUfo.SetTargetPosition();
            }

            yield return null;
        }
    }
	



}
