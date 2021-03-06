﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementNPC : MonoBehaviour {

    public GameObject PrefabStarTrackPoint;
    public string MarkerDebug = "";

    protected Coroutine moveObject;
    //protected ModelNPC.GameDataNPC _dataNPC;
    protected ModelNPC.PersonData _dataNPC;
    protected string objID;

    private Material m_material;
    private SpriteRenderer m_spriteRenderer;
    private Rigidbody2D _rb2d;
    private bool m_isPause = false;
    private bool m_isTrack = false;
    private List<Vector3> m_TrackPoints = new List<Vector3>();
    private GameObject m_TrackPointsNavigator = null;
    private GameObjecDataController m_dataController;

    private string testId;
    private string _resName = "";
    private int countUpdate = 0;
    //private float realtimeMoving = 0f;
    public bool isRunning = false;

    void Awake()
    {
        //m_dataController = gameObject.GetComponent<GameObjecDataController>();
        m_dataController = gameObject.GetDataController();
    }

    // Use this for initialization
    public virtual void Start()
    {
        InitNPC();

        //realtimeMoving =  Time.time + 3f;
    }

    public void InitNPC()
    {
        _resName = "";

        objID = Helper.GetID(this.name);
        if (string.IsNullOrEmpty(objID))
        {
            objID = "Empty";
            return;
        }
        //p_Data.Data.NameObject
        InitData();

        isRunning = false;
        if (moveObject != null)
        {
            isRunning = false;
            StopCoroutine(moveObject);
        }
        StartMoving();

        if (Storage.EventsUI == null)
        {
            Debug.Log("############ Storage.Events == null");
            return;
        }
        m_isTrack = Storage.EventsUI.IsTrackPointsVisible;

        
        if (GameActionPersonController.IsGameActionPersons)
        {
            var actionGame = GetComponent<GameActionPersonController>();
            if (actionGame == null)
                Debug.Log("############ InitNPC().GameActionPersonController == null");
            else
            {
                //actionGame.IsStartInit = true;
                actionGame.StartInit();
                //actionGame.InitActions();
                //actionGame.UpdateMeModelView();
            }
        }
    }

    protected virtual void StartMoving()
    {
        moveObject = StartCoroutine(MoveObjectToPosition<ModelNPC.GameDataNPC>());
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    private void LateUpdate()
    {
    }

    private void OnMouseDown()
    {
        SelectedGameObject();
    }

    void OnGUI()
    {
        if (Storage.SceneDebug.SettingsScene.IsShowTittlePerson)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 13;
            style.normal.textColor = Color.black;

            IsSelectedMe();
            if (objID == Storage.Instance.SelectGameObjectID)
            {
                style.fontSize = 15;
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = Color.yellow;
            }
            else
            {
                style.fontSize = 13;
                style.fontStyle = FontStyle.Normal;
                if (style.normal.textColor == Color.black)
                    style.normal.textColor = "#b3fff0".ToColor();//  Color.black;
                else
                    style.normal.textColor = Color.black;
            }

            if (Camera.main == null)
                return;

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            Rect position1 = new Rect(screenPosition.x - 10, Screen.height - screenPosition.y - 30, 300, 100);
            GUI.Label(position1, objID, style);
        }
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    private void InitData()
    {
        m_material = this.GetComponent<Renderer>().material;
        m_spriteRenderer = this.GetComponent<SpriteRenderer>();
        _rb2d = this.GetComponent<Rigidbody2D>();
    }

    IEnumerator ChangeColor()
    {

        while (true)
        {
            ChangeRandomColor();
            yield return new WaitForSeconds(2f);
        }
    }

    private void ChangeRandomColor()
    {
        m_spriteRenderer.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
    }

    
    protected IEnumerator MoveObjectToPosition<T>() where T : ModelNPC.GameDataNPC
    {
        float timeLockOnTarget = 0f;
        //int countLockOnTarget = 0;
        int speed = 1;
        Vector2 veloccityStart = _rb2d.velocity;
        float step = 0;
        if (!Helper.IsDataInit(this.gameObject))
        {
            isRunning = false;
            Debug.Log("##################### STOP >>>>>>>>>>>>>>>>>>>>>");
            yield break;
        }

        string info = "MoveObjectToPosition Init";
        _dataNPC = GetUpdateData();
        if (_dataNPC != null)
        {
            speed = _dataNPC.SpeedCurrent = _dataNPC.Speed;
            step = speed * Time.deltaTime;
        }
        else
        {
            Debug.Log("############# MoveObjectToPosition Not FIND dataNPC " + this.gameObject.name + "     tag:" + this.gameObject.tag);
        }

        bool actionIsMove = true;

        while (true)
        {
            actionIsMove = true;
            if (_dataNPC != null && _dataNPC.CurrentAction != GameActionPersonController.NameActionsPerson.Move.ToString())
                actionIsMove = false;
            if(m_isPause)
                actionIsMove = false;
            if (actionIsMove)
            {
                isRunning = true;
                if (step == 0) //TEST
                {
                    step = speed * Time.deltaTime;
                    //!FIX MOVE TARGET!
                    //step = _dataNPC.SpeedCurrent * Time.deltaTime;
                    Storage.EventsUI.ListLogAdd = "Movement NPC step is zero!!!";
                }

                if (Storage.Instance.IsLoadingWorld)
                {
                    Debug.Log("_______________ LOADING WORLD ....._______________");
                    yield return null;
                }

                if (Storage.Instance.IsCorrectData)
                {
                    Debug.Log("_______________ RETURN CorrectData ON CORRECT_______________");
                    yield return null;
                }

                if (_dataNPC == null)
                {
                    Debug.Log("########################## UFO MoveObjectToPosition dataUfo is EMPTY");
                    //Storage.Fix.CorrectData(null, this.gameObject, "MoveObjectToPosition");

                    UpdateData("MoveObjectToPosition");
                    isRunning = false;
                    yield break;
                }

                //---------------TEST //FIX@@DUBLICATE
                //if (_dataNPC.NameObject != gameObject.name)
                //{
                //    Debug.Log(Storage.EventsUI.ListLogAdd = "#### MoveObjectToPosition ERROR Name " + _dataNPC.NameObject + " <> GO:" + gameObject.name);
                //}
                //-------------------------------

                //isRunning = true;
                if (_dataNPC.IsMoveValid())
                {
                    Vector3 targetPosition = _dataNPC.TargetPosition;

                    _dataNPC.StartTransfer("MoveObjectToPosition"); //FIX~~TRANSFER

                    //float stepMove = step;
                    Vector3 pos = Vector3.MoveTowards(transform.position, targetPosition, step);

                    //----
                    float distField = Vector2.Distance(new Vector2(targetPosition.x,
                                             targetPosition.y)
                                             , new Vector2(transform.position.x, transform.position.y));

                    //!FIX MOVE TARGET!
                    if (distField < 0.5)
                    {
                        if (timeLockOnTarget < Time.time)
                        {
                            timeLockOnTarget = Time.time + 0.5f;
                            if (targetPosition.x > transform.position.x)
                                pos.x = transform.position.x + step / 2;
                            else
                                pos.x = transform.position.x - step / 2;
                            if (targetPosition.y > transform.position.y)
                                pos.y = transform.position.y + step / 2;
                            else
                                pos.y = transform.position.y - step / 2;
                        }
                    }
                    //-----

                    if (_rb2d != null)
                    {
                        _rb2d.MovePosition(pos);
                    }
                    else
                    {
                        //Debug.Log("NPC MoveObjectToPosition Set position 2 original........");
                        transform.position = pos;
                    }

                    //+++++++++++ RESAVE Next Position ++++++++++++
                    bool res = ResavePositionData<T>();

                    if(_dataNPC != null)
                        _dataNPC.StopTransfer();
                    if (!res)
                    {
                        Debug.Log("########### STOP MOVE ON ERROR MOVE");
                        Destroy(this.gameObject);
                        isRunning = false;
                        yield break;
                    }
                }
            }
            yield return null;
            isRunning = false;
        }

        Debug.Log("##################### STOP >>>>>>>>>>>>>>>>>>>>>");
        isRunning = false;
    }

    private bool ResavePositionData<T>() where T : ModelNPC.GameDataNPC
    {
        if (!_dataNPC.IsReality)
        {
            _dataNPC.IsReality = true;
            Debug.Log(Storage.EventsUI.ListLogAdd = "### CheckComplitionMoveInDream dataNPC IsReality !!!");
        }

        if (Storage.Instance.IsCorrectData)
        {
            Debug.Log("_______________ResavePositionData     RETURN CorrectData ON CORRECT_______________");
            return true;
        }

        if (!string.IsNullOrEmpty(_resName) && _resName != this.gameObject.name)
        {
            Debug.Log("################## ERROR MoveObjectToPosition ===========PRED========= rael name: " + this.gameObject.name + "  new name: " + _resName);
            return true;
        }

        if(Storage.Data.IsUpdatingLocationPersonGlobal)
        {
            //#test
            if(!PoolGameObjects.IsUsePoolObjects)
                Debug.Log("_______________ResavePositionData  RETURN IsUpdatingLocationPerson________ ASYNC _______" + this.gameObject.name);
            //return true;
        }

        string oldName = this.gameObject.name;
        Vector3 oldPoint = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        _resName = _dataNPC.NextPosition(this.gameObject);

        if(_resName=="Update")
        {
            UpdateData("ResavePositionData");
            _resName = "";
            return true;
        }

        if (_resName == "Error")
            return false;

        if (!string.IsNullOrEmpty(_resName))
        {
            if (oldName != _resName)
            {
                string callInfo = "ResavePositionData >> oldName(" + oldName + ") != _resName(" + _resName + ")";
                _dataNPC = GetUpdateData(callInfo);
                if (_dataNPC == null)
                {
                    Debug.Log("################## ERROR MoveObjectToPosition dataNPC is Empty   GO:" + this.gameObject.name);
                    return false;
                }
                //TRACK ME
                if (m_isTrack)
                {
                    //Debug.Log("m_TrackPoints : " + m_TrackPoints.Count + "          " + oldPoint.x + "x" + oldPoint.y);
                    m_TrackPoints.Add(oldPoint);
                    CrateNavigatorTrackPoints();
                }
            }
            if (_resName != this.gameObject.name)
            {
                Debug.Log("################## ERROR MoveObjectToPosition ===========POST========= rael name: " + this.gameObject.name + "  new name: " + _resName);
                this.gameObject.name = _resName;
            }
        }

        return true;
        //+++++++++++++++++++++++
    }

    //#POLYLINE TRACK
    private void CrateNavigatorTrackPoints()
    {
        if (m_TrackPointsNavigator == null)
        {
            //Debug.Log("------------------ StartCoroutine -- CreateTrackPolyline " + this.name);
            StartCoroutine(CreateTrackPolyline());
        }
        else
        {
            //Debug.Log("------------------ scriptTrackPoints.TrackPoints get TrackPointsNavigator");
            TrackPointsNavigator scriptTrackPoints = m_TrackPointsNavigator.GetComponent<TrackPointsNavigator>();

            var tr1 = m_TrackPointsNavigator.transform;
            var tr2 = this.gameObject.transform;

            if (scriptTrackPoints == null)
            {
                Debug.Log("############ TrackPointsNavigator is Empty");
                return;
            }

            scriptTrackPoints.TrackPoints = m_TrackPoints;

            if (_dataNPC == null)
            {
                UpdateData("CrateNavigatorTrackPoints");
                if (_dataNPC == null)
                {
                    Debug.Log("################# CrateNavigatorTrackPoints dataNPC is Empty");
                    return;
                }
            }

            ModelNPC.GameDataBoss dataBoss = _dataNPC as ModelNPC.GameDataBoss;
            if (dataBoss != null)
            {
                scriptTrackPoints.ColorTrack = dataBoss.ColorRender;
            }
            //Debug.Log("------------------ scriptTrackPoints.TrackPoints =" + m_TrackPoints.Count + " points: " + scriptTrackPoints);
        }
    }

    //#POLYLINE TRACK
    IEnumerator CreateTrackPolyline()
    {
        if (PrefabStarTrackPoint==null)
        {
            PrefabStarTrackPoint = (GameObject)GameObject.Find("PrefabStarTrackPointT");
            if(PrefabStarTrackPoint==null)
            {
                Debug.Log("############ PrefabStarTrackPoint Find(PrefabStarTrackPointT) Not Found !!! " + this.gameObject.name);
                yield break;
            }
        }
        yield return new WaitForSeconds(0.1f);

        m_TrackPointsNavigator = Instantiate(PrefabStarTrackPoint);
        m_TrackPointsNavigator.name = "NavigatorTrackPoints_" + this.gameObject.name;
        m_TrackPointsNavigator.transform.SetParent(this.gameObject.transform);
        m_TrackPointsNavigator.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        TrackPointsNavigator scriptTrackPoints = m_TrackPointsNavigator.GetComponent<TrackPointsNavigator>();
        if(scriptTrackPoints==null)
        {
            Debug.Log("############ TrackPointsNavigator is Empty");
            yield break;
        }
        scriptTrackPoints.TrackPoints = m_TrackPoints;
        //Debug.Log("************ TrackPointsNavigator Created : " + m_TrackPointsNavigator.name);
    }

    public virtual void UpdateData(string callFunc)
    {
        _dataNPC = GetUpdateData(callFunc);
        objID = Helper.GetID(this.name);
    }

    public void SetTarget()
    {
        Debug.Log("^^^^^^^^ TARGET --- SetTarget NPC " + this.tag + "       " + this.name);
        if (_dataNPC != null)
        {
            //Vector3 setV = Storage.Person.PersonsTargetPosition;
            //_dataNPC.SetTargetPosition(setV);
            _dataNPC.SetTargetPosition(Storage.Person.PersonsTargetPosition);
            
        }
    }

    private void SelectedGameObject()
    {
        Storage.EventsUI.SetTestText(objID);

        FindPersonData person = Storage.Person.GetFindPersonsDataForName(this.gameObject.name);
        if(person == null)
        {
            Debug.Log("#### SelectedGameObject Not in Data go name: " + this.gameObject.name);
        }
        var res = GetUpdateData("SelectedGameObject");
        if (res == null)
        {
            Debug.Log("#### GetUpdateData: " + res.NameObject);
        }

        //#EXPAND
        Storage.EventsUI.AddMenuPerson(_dataNPC, this.gameObject);

     
    }



    private bool _isSelected = false;
    private bool IsSelectedMe()
    {
        if (objID == Storage.Instance.SelectGameObjectID)
        {
            if (!_isSelected)
            {
                _isSelected = true;
                SelectedMe();
                return true;
            }
        }
        else
        {
            _isSelected = false;
        }
        return false;
    }

    private void SelectedMe()
    {
        Storage.EventsUI.ListLogAdd = "Me: " + this.gameObject.name;
    }

    public void Pause()
    {
        m_isPause = !m_isPause;
        //Debug.Log("Pause = " + m_isPause + "   " + this.name);
    }

    public void TrackOn()
    {
        m_isTrack = !m_isTrack;
    }

    //protected virtual ModelNPC.GameDataNPC GetUpdateData(string callInfo = "GetInitData")
    //{
    //    var dataNPC = m_dataController.UpdateData(callInfo) as ModelNPC.GameDataNPC;
    //    return dataNPC;
    //}

    //public virtual ModelNPC.GameDataNPC GetData(string callInfo = "GetInitData")
    //{
    //    return _dataNPC;
    //}

    protected virtual ModelNPC.PersonData GetUpdateData(string callInfo = "GetInitData")
    {
        var dataNPC = m_dataController.UpdateData(callInfo) as ModelNPC.PersonData;
        return dataNPC;
    }

    public virtual ModelNPC.PersonData GetData(string callInfo = "GetInitData")
    {
        return _dataNPC as ModelNPC.PersonData;
    }

}
