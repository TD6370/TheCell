using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUfo : MonoBehaviour {

    public GameObject DataStorage;
    //private Storage m_scriptStorage;

    private Coroutine moveObject;
    private Material m_material;
    private SpriteRenderer m_spriteRenderer;
    //@PD@ private PersonalData m_scriptPersonal;
    private Rigidbody2D _rb2d;
    //private int _lmitHorizontalLook = 0;
    //private int _limitVerticalLook = 0;

    string testId;

    void Awake()
    {
        //@44@
        ///testId = Guid.NewGuid().ToString().Substring(1, 4);
        //Debug.Log("UFO create ID ++++++++++++++++++++++ " + testId);
        //this.name = "PrefabUfo_" + testId;
    }

	// Use this for initialization
	void Start () {

        InitData();
        StartCoroutine(MoveObjectToPosition());
        
	}

    // Update is called once per frame
    void Update()
    {
    }

    private void InitData()
    {
        m_material = this.GetComponent<Renderer>().material;
        m_spriteRenderer = this.GetComponent<SpriteRenderer>();
        //@PD@ m_scriptPersonal = this.GetComponent<PersonalData>();
        _rb2d = this.GetComponent<Rigidbody2D>();

        var storage = DataStorage;
        if (storage == null)
        {
            Debug.Log("DataStorage null");
            return;
        }
        //m_scriptStorage = storage.GetComponent<Storage>();
        //if (m_scriptStorage == null)
        //{
        //    Debug.Log("Error scriptStorage is null !!!");
        //    return;
        //}
        //_lmitHorizontalLook = m_scriptStorage.LimitHorizontalLook;
        //_limitVerticalLook = m_scriptStorage.LimitVerticalLook;
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
        m_spriteRenderer.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
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
        Vector3 lastPositionForLock = transform.position;
        Vector3 lastPositionForMoveField = transform.position;

        string newName = "";

        int stepTest = 0;
        int stepLimitTest = 10;
        float minDist = 0.005f;  //0.01f;

        int speed = 2;
        float step = speed * Time.deltaTime;

        //int start = this.gameObject.name.IndexOf("Field");
        //if (start == -1)
        //{
        //    Debug.Log("# GetNameFieldByName " + this.gameObject.name + " key 'Field' not found!");
        //    yield return null;
        //}
        if (!Storage.IsDataInit(this.gameObject))
        {
            //yield return null;
            yield break;
        }

        //@PD@ var dataUfo = m_scriptPersonal.PersonalObjectData as SaveLoadData.GameDataUfo;
        //Debug.Log("______________________________________CALL CreateObjectData 5._________________________" + this.gameObject.name);
        var dataUfo = SaveLoadData.CreateObjectData(this.gameObject) as SaveLoadData.GameDataUfo; ;

        if (dataUfo == null)
        {
            Debug.Log("Error UFO MoveObjectToPosition objUfo is Empty !!!!");
            yield break;
        }

        if (dataUfo.TargetPosition == new Vector3(0, 0, 0))
        {
            Debug.Log("Error UFO objUfo.TargetPosition is zero !!!!");
            yield break;
        }

        while (true)
        {
            stepTest++;
            if (stepTest > stepLimitTest)
            {
                float distLock = Vector3.Distance(lastPositionForLock, transform.position);
                if (distLock < minDist)
                {
                    //Debug.Log("MoveObjectToPosition ------ UFO LOCK !!!!  > " + distLock);
                    //objUfo.SetTargetPosition(_lmitHorizontalLook, _limitVerticalLook);
                    //SetTargetPosition(objUfo);
                    dataUfo.SetTargetPosition();
                }
                lastPositionForLock = transform.position;
                stepTest = 0;
            }

            Vector3 targetPosition  = dataUfo.TargetPosition;
            Vector3 pos = Vector3.MoveTowards(transform.position, targetPosition, step); 


            // Move our position a step closer to the target. @!@
            //var _rb2d = GetComponent<Rigidbody2D>();
            if (_rb2d != null)
            {
                //Debug.Log("UFO MoveObjectToPosition Set position 2........");
                _rb2d.MovePosition(pos);
            }
            else
            {
                //@!@.2
                Debug.Log("UFO MoveObjectToPosition Set position 2 original........");
                transform.position = pos; 
            }

            if (!string.IsNullOrEmpty(newName) && newName != this.gameObject.name)
            {
                Debug.Log("ERROR =================================== MoveObjectToPosition ===========PRED========= rael name: " + this.gameObject.name + "  new name: " + newName);
            }

            //if (!string.IsNullOrEmpty(newName))
            //{
            //    Debug.Log("TEST ==== MoveObjectToPosition PRED ==== rael name: " + this.gameObject.name + "  last new name: " + newName);
            //}

            string oldName = this.name;


            //string resName = dataUfo.TestNextPosition(this.gameObject, newName);
            //+++++++++++++++++++++++
            string resName = dataUfo.NextPosition(this.gameObject);
            //+++++++++++++++++++++++

            if (!string.IsNullOrEmpty(resName))
                newName = resName;

            if (!string.IsNullOrEmpty(resName))
            {
                if (this.gameObject.name != newName)
                {
                    Debug.Log("ERROR **** MoveObjectToPosition POST ==== rael name: " + this.gameObject.name + "  new name: " + newName + "      old name:" + oldName);
                    this.gameObject.name = newName;
                }
                else
                {
                    //@POS@ Debug.Log("TEST ==== MoveObjectToPosition POST ==== rael name: " + this.gameObject.name + "  new name: " + newName + "      old name:" + oldName);
                }
            }
            if (!string.IsNullOrEmpty(resName) && resName != this.gameObject.name)
            {
                Debug.Log("ERROR =================================== MoveObjectToPosition ===========POST========= rael name: " + this.gameObject.name + "  new name: " + newName);
            }
            //if (!string.IsNullOrEmpty(newName))
            //{
            //    Debug.Log("TEST ==== MoveObjectToPosition POST ==== rael name: " + this.gameObject.name + "  new name: " + newName + "      old name:" + oldName);
            //}
            //if (!string.IsNullOrEmpty(newName) && newName != this.gameObject.name)
            //{
            //    Debug.Log("ERROR =================================== MoveObjectToPosition ===========POST========= rael name: " + this.gameObject.name + "  new name: " + newName);
            //}

            float dist = Vector3.Distance(targetPosition, transform.position);
            //Debug.Log("MoveObjectToPosition ------ UFO distance to point  > " + dist);
            if (dist < minDist)
            {
                //Debug.Log("MoveObjectToPosition ------ UFO IN POINT  > " + dist);
                //objUfo.SetTargetPosition();
                //SetTargetPosition(objUfo);
                dataUfo.SetTargetPosition();
            }

            yield return null;
        }
    }

    //private void SetTargetPosition(SaveLoadData.GameDataUfo objUfo) 
    //{
    //    objUfo.SetTargetPosition(m_scriptStorage.ZonaField);
    //}
	



}
