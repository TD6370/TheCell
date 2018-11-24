using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsObject : MonoBehaviour {

    public bool IsMeTerra = false;
    private bool m_isAlpha = false;
    private float m_LevelAlpha = 0;
    private string m_OldFieldHero = "";

    public PoolGameObject PoolCase { get; set; }

    private void Awake()
    {
        string typePrefub = this.gameObject.tag.ToString();

        if (this.gameObject == null)
            return;

        if (this.gameObject == null)
            return;

        SaveLoadData.TypePrefabs typePrefab = Helper.GetTypePrefab(this.gameObject);
        if (Helper.IsTerraAlpha(typePrefab))
        {
            IsMeTerra = true;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
    void Update()
    {
        AlphaTerra();
    }

    private void AlphaTerra()
    {
        if (IsMeTerra)
        {
            int offsetTopHero = 0;

            if (m_OldFieldHero == Storage.Instance.SelectFieldPosHero)
                return;
            m_OldFieldHero = Storage.Instance.SelectFieldPosHero;

            float posHeroY = Storage.PlayerController.transform.position.y + offsetTopHero;
            float gobjY = this.transform.position.y;
            float posHeroX = Storage.PlayerController.transform.position.x;
            float gobjX = this.transform.position.x;
            float dist = Vector3.Distance(Storage.PlayerController.transform.position, this.transform.position);
            float distX = Math.Abs(Storage.PlayerController.transform.position.x - this.transform.position.x);
            bool isNear = false;
            float maxDist = 10;
            float maxDistX = 3;
            ///if (dist < maxDist)
            if (dist < maxDist && distX < maxDistX)
                isNear = true;
            

            string field = Helper.GetNameFieldObject(this.gameObject);

            //if (gobjY - offsetTopHero < posHeroY && isNear )
            if (gobjY < posHeroY && isNear)
            {
                float _alphaKof = (dist / maxDist);
                float LevAlpha = _alphaKof;
                if (LevAlpha != m_LevelAlpha)
                {
                    m_LevelAlpha = LevAlpha;
                    //Single _alpha = 1 - Math.Abs(_alphaKof) + 0.3f;
                    Single _alpha = _alphaKof;

                    if (_alpha < 0.6f)
                        _alpha = 0.6f;
                    if (_alpha > 0.8f)
                        _alpha = 0.8f;

                    //---------------
                    if (field == Storage.Instance.SelectFieldCursor) // Storage.Instance.SelectFieldPosHero)
                    {
                       // Debug.Log("----------" + this.gameObject.name + "--- Dist: " + dist + "    Alpha: " + _alpha + " " + this.transform.position + " H> " + Storage.PlayerController.transform.position);
                    }
                    //-----------------

                    this.gameObject.SetAlpha(_alpha);
                    m_isAlpha = true;
                }
            }
            else
            {
                if (m_isAlpha)
                {
                    //---------------
                    if (field == Storage.Instance.SelectFieldCursor) // Storage.Instance.SelectFieldPosHero)
                    {
                        Debug.Log("----------" + this.gameObject.name + "--- Dist: " + dist + "    Alpha: reset !! " + this.transform.position + " H> " + Storage.PlayerController.transform.position);
                    }
                    //-----------------
                    
                    this.gameObject.SetAlpha(1f);
                    m_LevelAlpha = -1;
                    m_isAlpha = false;
                }
            }

        }
    }

    //private void AlphaTerra()
    //{
    //    if (IsMeTerra)
    //    {
    //        if (m_OldFieldHero == Storage.Instance.SelectField)
    //            return;
    //        m_OldFieldHero = Storage.Instance.SelectField;

    //        float posHeroY = Storage.PlayerController.transform.position.y;
    //        float gobjY = this.transform.position.y;
    //        float posHeroX = Storage.PlayerController.transform.position.x;
    //        float gobjX = this.transform.position.x;
    //        float dist = Vector3.Distance(Storage.PlayerController.transform.position, this.transform.position);
    //        bool isNear = false;
    //        float maxDist = 10;
    //        if (dist < maxDist)
    //            isNear = true;

    //        int offsetTopHero = 0;

    //        //if (gobjY - offsetTopHero < posHeroY && isNear )
    //        if (gobjY  < posHeroY && isNear)
    //        {
    //            float _alphaKof = (dist/ maxDist);
    //            float LevAlpha = _alphaKof;
    //            if (LevAlpha != m_LevelAlpha)
    //            {
    //                m_LevelAlpha = LevAlpha;
    //                Single _alpha = 1 - Math.Abs(_alphaKof) + 0.3f;
    //                this.gameObject.SetAlpha(_alpha);
    //                m_isAlpha = true;
    //            }
    //        }
    //        else
    //        {
    //            if (m_isAlpha)
    //            {
    //                this.gameObject.SetAlpha(1f);
    //                m_LevelAlpha = -1;
    //                m_isAlpha = false;
    //            }
    //        }

    //    }
    //}

    private void OnMouseDown()
    {
        //Debug.Log("&&&& EventsObject OnMouseDown");
        SelectIdFromTextBox();
    }

    public void Kill()
    {
        Storage.Instance.AddDestroyGameObject(this.gameObject);
    }

    private void SelectIdFromTextBox()
    {
        var gobj = this.gameObject;
        if (gobj.tag.ToString().IsPerson())
        {
            string objID = Helper.GetID(this.gameObject.name);
            Storage.Instance.SelectGameObjectID = objID;
            //Debug.Log("&&&& EventsObject Select " + objID + "   " + this.gameObject.name);
            Storage.Events.SetTestText(objID);
        }
        else
        {
            //Debug.Log("&&&& EventsObject Select " + gobj.tag.ToString() + "     " + this.gameObject.name);
            Storage.Events.SetTestText(gobj.tag.ToString());
        }
    }
}
