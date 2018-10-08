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
    private PersonalData m_scriptPersonal;
    private Rigidbody2D _rb2d;
    //private int _lmitHorizontalLook = 0;
    //private int _limitVerticalLook = 0;

    string testId;

    void Awake()
    {
        testId = Guid.NewGuid().ToString().Substring(1, 4);
        Debug.Log("UFO create ID ++++++++++++++++++++++ " + testId);
        this.name = "PrefabUfo_" + testId;
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
        m_scriptPersonal = this.GetComponent<PersonalData>();
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

        int stepTest = 0;
        int stepLimitTest = 10;
        float minDist = 0.005f;  //0.01f;

        int speed = 2;
        float step = speed * Time.deltaTime;
        var objUfo = m_scriptPersonal.PersonalObjectData as SaveLoadData.GameDataUfo;

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
                float distLock = Vector3.Distance(lastPositionForLock, transform.position);
                if (distLock < minDist)
                {
                    //Debug.Log("MoveObjectToPosition ------ UFO LOCK !!!!  > " + distLock);
                    //objUfo.SetTargetPosition(_lmitHorizontalLook, _limitVerticalLook);
                    //SetTargetPosition(objUfo);
                    objUfo.SetTargetPosition();
                }
                lastPositionForLock = transform.position;
                stepTest = 0;
            }

            Vector3 targetPosition  = objUfo.TargetPosition;
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

            //+FIX ----------------------------
            //string posFieldOld = Storage.GetNameFieldPosit(lastPositionForMoveField.x, lastPositionForMoveField.y);
            //string posFieldReal = Storage.GetNameFieldPosit(pos.x, pos.y);

            ////!!!!!!!!!!!! p_nameField === posFieldOld
            //if (posFieldOld != posFieldReal)
            //{
            //    lastPositionForMoveField = pos;
                //Debug.Log("UFO  MoveObjectToPosition " + posFieldOld + " >> " + posFieldReal + "   PersonalObjectData=" + objUfo.NameObject + "   testId=" + testId);

                //var lastNewePos = ((SaveLoadData.GameDataNPC)objUfo).NewPosition;
                //var lastNewePosUfo = ((SaveLoadData.GameDataUfo)objUfo).NewPosition;
                //123456789
                //Debug.Log("UFO  MoveObjectToPosition lastNewePos=" + lastNewePos + "  New pos:" + pos + "       lastNewePosUfo=" + lastNewePosUfo);
                //if (lastNewePos != pos)
                //{
                    
                    //objUfo.NextPosition(pos);
                    objUfo.NextPosition(transform.position); //NOT NEW POSITION, YES REAL POSITION
                    


                    //123456789
                    //((SaveLoadData.GameDataNPC)objUfo).NewPosition = Storage.ConvVector3(pos);;
                    //((SaveLoadData.GameDataUfo)objUfo).NewPosition = Storage.ConvVector3(pos); ;
                //}
            
            //}
            //-----------------------------------


            float dist = Vector3.Distance(targetPosition, transform.position);
            //Debug.Log("MoveObjectToPosition ------ UFO distance to point  > " + dist);
            if (dist < minDist)
            {
                //Debug.Log("MoveObjectToPosition ------ UFO IN POINT  > " + dist);
                //objUfo.SetTargetPosition();
                //SetTargetPosition(objUfo);
                objUfo.SetTargetPosition();
            }

            yield return null;
        }
    }

    //private void SetTargetPosition(SaveLoadData.GameDataUfo objUfo) 
    //{
    //    objUfo.SetTargetPosition(m_scriptStorage.ZonaField);
    //}
	



}
