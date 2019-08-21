using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsObject : MonoBehaviour {

    public bool IsMeTerra = false;
    //public Color ColorOutline;

    private bool m_isAlpha = false;
    private float m_LevelAlpha = 0;
    private string m_OldFieldHero = "";
    private SpriteRenderer m_spriteRenderer;
    private GameObjecDataController m_DataController;

    public PoolGameObject PoolCase { get; set; }

    private void Awake()
    {
        string typePrefub = this.gameObject.tag.ToString();

        if (this.gameObject == null)
            return;

        if (this.gameObject == null)
            return;

        //if (PoolGameObjects.IsUseTypePoolPrefabs) //$$$
        //{
            //---------- $$$FLC
            PoolGameObjects.TypePoolPrefabs typePool = PoolGameObjects.TypePoolPrefabs.PoolFloor;
            bool isValid = System.Enum.IsDefined(typeof(PoolGameObjects.TypePoolPrefabs), this.gameObject.tag);
            if (isValid == false)
            {
                //Debug.Log("!!!!!!!!!!!!! ERROR TAG OLD " + this.gameObject.tag); //$$$
                if (this.gameObject.tag == "PrefabUfo")
                    typePool = PoolGameObjects.TypePoolPrefabs.PoolPersonUFO;
                else if (this.gameObject.tag == "PrefabBoss")
                    typePool = PoolGameObjects.TypePoolPrefabs.PoolPersonBoss;
                else
                {
                    Debug.Log("!!!!!!!!!!!!! LEGACY CODE " + this.gameObject.tag); //$$$
                    typePool = PoolGameObjects.TypePoolPrefabs.PoolWall;
                }
            }
            //---------- 
            else
            {
                typePool = (PoolGameObjects.TypePoolPrefabs)Enum.Parse(typeof(PoolGameObjects.TypePoolPrefabs), this.gameObject.tag);
            }
            if (Helper.IsTerraAlpha(typePool))
            {
                IsMeTerra = true;
            }
        //}
        //else
        //{
        //    SaveLoadData.TypePrefabs typePrefab = Helper.GetTypePrefab(this.gameObject);
        //    if (Helper.IsTerraAlpha(typePrefab))
        //    {
        //        IsMeTerra = true;
        //    }
        //}
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_DataController = GetComponent<GameObjecDataController>();
        
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        AlphaTerra();
    }

    private void LightingPosition(float posHeroY, float gobjY )
    //private void LightingPosition()
    {
        //float posHeroY = Storage.PlayerController.transform.position.y;
        //float gobjY = this.transform.position.y;
        int offsetTopHero = -1;

        if (gobjY < posHeroY + offsetTopHero)
        {
            gameObject.layer = Helper.LayerDark;
            //UpdateOutline(true);
        }
        else
        {
            gameObject.layer = Helper.LayerDefault;
            //UpdateOutline(false);
        }
    }

    /*
    void UpdateOutline(bool outline)
    {
        var data = m_DataController.GetData();
        if(data != null && data.ModelView == "PrefabElka")
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            m_spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Outline", outline ? 1f : 0);
            mpb.SetColor("_OutlineColor", ColorOutline);
            m_spriteRenderer.SetPropertyBlock(mpb);
        }
    }
    */

    private void AlphaTerra()
    {
        if (IsMeTerra)
        {
            //LightingPosition();

            int offsetTopHero = 0;

            if (m_OldFieldHero == Storage.Instance.SelectFieldPosHero)
                return;
            m_OldFieldHero = Storage.Instance.SelectFieldPosHero;


            //var t = this.gameObject.name;

            float posHeroY = Storage.PlayerController.transform.position.y + offsetTopHero;
            float gobjY = this.transform.position.y;

            LightingPosition(posHeroY, gobjY);

            float posHeroX = Storage.PlayerController.transform.position.x;
            float gobjX = this.transform.position.x;
            float dist = Vector3.Distance(Storage.PlayerController.transform.position, this.transform.position);
            float distX = Math.Abs(Storage.PlayerController.transform.position.x - this.transform.position.x);
            bool isNear = false;
            float maxDist = 10;
            float maxDistX = 1; // 3;
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

                    //if (_alpha < 0.6f)
                    //    _alpha = 0.6f;
                    if (_alpha < 0.8f)
                        _alpha = 0.75f;
                    if (_alpha > 0.8f)
                        _alpha = 0.9f;
                    //_alpha = 0.9f;

                    //---------------
                    if (field == Storage.Instance.SelectFieldCursor) // Storage.Instance.SelectFieldPosHero)
                    {
                       // Debug.Log("----------" + this.gameObject.name + "--- Dist: " + dist + "    Alpha: " + _alpha + " " + this.transform.position + " H> " + Storage.PlayerController.transform.position);
                    }
                    //-----------------

                    m_spriteRenderer.SetAlpha(_alpha);
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

                    m_spriteRenderer.SetAlpha(1f);
                    m_LevelAlpha = -1;
                    m_isAlpha = false;
                }
            }

        }
    }

    

    private void OnMouseDown()
    {
        //Debug.Log("&&&& EventsObject OnMouseDown");
        SelectIdFromTextBox();
    }
       

    private void SelectIdFromTextBox()
    {
        var gobj = this.gameObject;
        if (gobj.tag.ToString().IsPerson())
        {
            string objID = Helper.GetID(this.gameObject.name);
            Storage.Instance.SelectGameObjectID = objID;
            //Debug.Log("&&&& EventsObject Select " + objID + "   " + this.gameObject.name);
            Storage.EventsUI.SetTestText(objID);
        }
        else
        {
            //Debug.Log("&&&& EventsObject Select " + gobj.tag.ToString() + "     " + this.gameObject.name);
            Storage.EventsUI.SetTestText(gobj.tag.ToString());
        }
    }
}
