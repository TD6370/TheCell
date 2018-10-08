using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNPC : MonoBehaviour {

    public GameObject DataStorage;

    private Material m_material;
    private SpriteRenderer m_spriteRenderer;
    private PersonalData m_scriptPersonal;
    private Rigidbody2D _rb2d;

    int Level = 0;
    int Life = 0;

    protected void Start()
    {
        InitData();
        StartCoroutine(MoveObjectToPosition());
    }

    protected void Update()
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
    }

    IEnumerator MoveObjectToPosition()
    {
        Vector3 lastPosition = transform.position;
        int stepTest = 0;
        int stepLimitTest = 10;
        float minDist = 0.005f;  //0.01f;

        int speed = 2;
        float step = speed * Time.deltaTime;
        var objNPC = m_scriptPersonal.PersonalObjectData as SaveLoadData.GameDataNPC;

        if (objNPC == null)
        {
            Debug.Log("Error NPC MoveObjectToPosition objUfo is Empty !!!!");
            yield break;
        }

        if (objNPC.TargetPosition == new Vector3(0, 0, 0))
        {
            Debug.Log("Error NPC objUfo.TargetPosition is zero !!!!");
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
                    objNPC.SetTargetPosition();
                }
                lastPosition = transform.position;
                stepTest = 0;
            }

            Vector3 targetPosition = objNPC.TargetPosition;
            Vector3 pos = Vector3.MoveTowards(transform.position, targetPosition, step);


            // Move our position a step closer to the target. @!@
            if (_rb2d != null)
            {
                //Debug.Log("UFO MoveObjectToPosition Set position 2........");
                _rb2d.MovePosition(pos);
            }
            else
            {
                //@!@.2
                Debug.Log("NPC MoveObjectToPosition Set position 2 original........");
                transform.position = pos;
            }

            float dist = Vector3.Distance(targetPosition, transform.position);
            if (dist < minDist)
            {
                objNPC.SetTargetPosition();
            }

            yield return null;
        }
    }
}
