using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNPC : MonoBehaviour {

    public GameObject PrefabStarTrackPoint;

    public string MarkerDebug = "";

    protected Coroutine moveObject;
    protected ModelNPC.GameDataNPC _dataNPC;

    private Material m_material;
    private SpriteRenderer m_spriteRenderer;
    private Rigidbody2D _rb2d;
    private string objID;
    //private UIEvents _scriptUIEvents;
    private bool m_isPause = false;
    private bool m_isTrack = false;
    //private bool m_isTrack = true;
    private List<Vector3> m_TrackPoints = new List<Vector3>();
    private GameObject m_TrackPointsNavigator = null;

    string testId;
    string _resName = "";

    
    public bool isRunning = false;

    void Awake()
    {
    }

    // Use this for initialization
    public virtual void Start()
    {
        InitNPC();

        realtimeMoving =  Time.time + 3f;
    }

    public void InitNPC()
    {
        _resName = "";

        objID = Helper.GetID(this.name);
        if (string.IsNullOrEmpty(objID))
        {
            objID = "Empty";
            //this.gameObject.SetActive(false);
            return;
        }

        InitData();

        isRunning = false;
        if (moveObject != null)
        {
            isRunning = false;
            StopCoroutine(moveObject);
        }

        StartMoving();

        //GameObject UIcontroller = GameObject.FindWithTag("UI");
        //if (UIcontroller == null)
        //    Debug.Log("########### MovementUfo UIcontroller is Empty");
        //_scriptUIEvents = UIcontroller.GetComponent<UIEvents>();
        //if (_scriptUIEvents == null)
        //    Debug.Log("########### MovementUfo scriptUIEvents is Empty");
        if (Storage.Events == null)
        {
            Debug.Log("############ Storage.Events == null");
            return;
        }


        m_isTrack = Storage.Events.IsTrackPointsVisible;
    }

    protected virtual void StartMoving()
    {
        moveObject = StartCoroutine(MoveObjectToPosition<ModelNPC.GameDataNPC>());
    }

    bool isInfoStop = false;

    // Update is called once per frame
    public virtual void Update()
    {
        if(PoolGameObjects.IsUsePoolObjects)
            Refresh();
    }

    private void LateUpdate()
    {
        //Refresh();
    }

    private void OnMouseDown()
    {
        SelectedGameObject();
    }

    void OnGUI()
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
            if(style.normal.textColor== Color.black)
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

    void OnEnable()
    {
        //InitNPC();

        //if (PoolGameObjects.IsUsePoolObjects)
        //{
        //    if (Helper.IsDataInit(this.gameObject))
        //        UpdateData("Update");
        //}

        //StartMoving();
        //Debug.Log("*********************** MovementNPC  is OnEnable " + this.gameObject.name);
    }

    void OnDisable()
    {
        //isRunning = false;
        //StopCoroutine(moveObject);

        //Debug.Log("*********************** MovementNPC  is OnDisable " + this.gameObject.name);
    }

    private int countUpdate = 0;

    public void Refresh()
    {
        return;

        //if (!isInfoStop)
        //{
        //------------------------
        if (Time.time > realtimeMoving)
        {
            //bool isStartPrefab = this.gameObject.name.IndexOf("Field") == -1;
            bool IsDataInit = Helper.IsDataInit(this.gameObject);
            

            //isInfoStop = true;
            //Debug.Log("######## ME STOP : " + this.gameObject.name + @" isStartPrefab \\\");
            if (IsDataInit)
            {
                //UpdateData("Update");
                Debug.Log("######## ME STOP : " + this.gameObject.name + "  isRunning=" + isRunning);
                //if (!isRunning)
                //{
                //UpdateData("Update");

                isRunning = false;
                if (moveObject != null)
                {
                    isRunning = false;
                    //StopCoroutine(moveObject);
                }

                //UpdateData("Update");

                countUpdate = 10;

                //StartMoving();
            }
        }
        //}
        //-----------------
        //if (!isRunning)
        //{
        //    if (moveObject!=null)
        //        StopCoroutine(moveObject);

        //    StartMoving();
        //}
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

    //IEnumerator MoveObject()
    //{
    //    int distantion = 0;
    //    float x = transform.position.x;
    //    int compas = 1;

    //    while (true)
    //    {
    //        distantion++;
    //        if (distantion > 200)
    //        {
    //            distantion = 0;
    //            compas *= -1;
    //        }

    //        transform.Translate(compas * Time.deltaTime, 0, 0);
    //        yield return null;
    //    }
    //}


    private float realtimeMoving = 0f;

    //if (Input.GetKey("m") && Time.time > DelayTimer)
    //    {
    //        Storage.Map.Create();

    //        DelayTimer = Time.time + ActionRate;
    //    }

    protected IEnumerator MoveObjectToPosition<T>() where T : ModelNPC.GameDataNPC
    {
        //if(isRunning)
        //{
        //    Debug.Log("######### MoveObjectToPosition IS PROGRESS......");
        //    yield break;
        //}

        //isRunning = true;

        Vector3 lastPositionForLock = transform.position;
        Vector3 lastPositionForMoveField = transform.position;

        int stepTest = 0;
        int stepLimitTest = 10;
        float minDist = 0.005f;

        int speed = 1;
        float step = 0;


        if (!Helper.IsDataInit(this.gameObject))
        {
            isRunning = false;
            Debug.Log("##################### STOP >>>>>>>>>>>>>>>>>>>>>");
            yield break;
        }

        string info = "MoveObjectToPosition Init";
        //_dataNPC = FindObjectData(info) as SaveLoadData.GameDataNPC;
        _dataNPC = FindObjectData<T>(info);
        if (_dataNPC != null)
        {
            speed = _dataNPC.Speed;
            //Debug.Log("Speed (" + this.name + ") : " + speed);
        }

        step = speed * Time.deltaTime;

        while (true)
        {
            realtimeMoving = Time.time + 0.5f;
            //#TEST
            //if (!isRunning && PoolGameObjects.IsUsePoolObjects)
            //{
            //    Debug.Log("-------------- Update isRunning : : : " + this.gameObject.name + ", " + _dataNPC.NameObject);
            //    UpdateData("Update");
            //}

            //if (PoolGameObjects.IsUsePoolObjects)
            //{
            //    if(this.gameObject.name != _dataNPC.NameObject)
            //    {
            //        Debug.Log("-------------- Update >>>>> " + this.gameObject.name + " <> " + _dataNPC.NameObject);
            //        UpdateData("Update");
            //    }
            //if (PoolGameObjects.IsUsePoolObjects)
            //{
            //    if (countUpdate > 0)
            //    {
            //        countUpdate--;
            //        objID = Helper.GetID(this.name);
            //        UpdateData("MoveObjectToPosition");
            //    }
            //}
            //}
            isRunning = true;

            //realtimeMoving = Time.time + 2f;
            //realtimeMoving = Time.time + 0.2f;

            if (this.name == MarkerDebug)
            {
                Debug.Log("OK");
            }

            if (m_isPause)
            {
                //Debug.Log("_______________ PAUSE ME (" + this.gameObject.name + ") ....._______________");
                _rb2d.velocity = Vector3.zero;
                while (m_isPause)
                {
                    //Debug.Log("##################### STOP >>>>>>>>>>>>>>>>>>>>>");
                    yield return null;
                }
                Debug.Log("_______________ PAUSE ME (" + this.gameObject.name + ") END ....._______________");
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
                isRunning = false;
                yield break;
            }

            stepTest++;
            if (stepTest > stepLimitTest)
            {
                float distLock = Vector3.Distance(lastPositionForLock, transform.position);
                if (distLock < minDist)
                {
                    _dataNPC.SetTargetPosition();
                }
                lastPositionForLock = transform.position;
                stepTest = 0;
            }

            Vector3 targetPosition = _dataNPC.TargetPosition;
            Vector3 pos = Vector3.MoveTowards(transform.position, targetPosition, step);

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

            if (!res)
            {
                Debug.Log("########### STOP MOVE ON ERROR MOVE");
                Destroy(this.gameObject);
                isRunning = false;
                yield break;
            }

            float dist = Vector3.Distance(targetPosition, transform.position);
            if (dist < minDist)
            {
                _dataNPC.SetTargetPosition();
            }

            realtimeMoving = Time.time + 1f;

            yield return null;
            isRunning = false;
        }

        Debug.Log("##################### STOP >>>>>>>>>>>>>>>>>>>>>");
        isRunning = false;
    }

    private bool ResavePositionData<T>() where T : ModelNPC.GameDataNPC
    {
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
                _dataNPC = FindObjectData<T>(callInfo);// as SaveLoadData.GameDataUfo;
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



    protected T FindObjectData<T>(string callFunc) where T : ModelNPC.GameDataNPC
    {
        //Debug.Log("***************  FindObjectData GO: " + gameObject.name + "  " + gameObject.tag + "    T: " + typeof(T) + "      GO Type: " + this.gameObject.GetType());

        T _dataNPC = SaveLoadData.FindObjectData(this.gameObject) as T;


        if (_dataNPC == null)
        {
            Debug.Log("#################### Error UFO MoveObjectToPosition dataUfo is Empty !!!!    :" + callFunc);
            return null;
        }

        if (_dataNPC.NameObject != this.name)
        {
            Debug.Log("#################### Error UFO MoveObjectToPosition dataUfo: " + _dataNPC.NameObject + "  GO: " + this.name + "   :" + callFunc);
            return null;
        }

        if (_dataNPC.TargetPosition == new Vector3(0, 0, 0))
        {
            Debug.Log("#################### Error UFO dataUfo.TargetPosition is zero !!!!   :" + callFunc);
            return null;
        }

        return _dataNPC;
    }

    //#POLYLINE TRACK
    private void CrateNavigatorTrackPoints()
    {
        // Debug.Log("CrateNavigatorTrackPoints................");

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
        //PrefabStarTrackPointT

        if (PrefabStarTrackPoint==null)
        {
            PrefabStarTrackPoint = (GameObject)GameObject.Find("PrefabStarTrackPointT");
            if(PrefabStarTrackPoint==null)
            {
                Debug.Log("############ PrefabStarTrackPoint Find(PrefabStarTrackPointT) Not Found !!! " + this.gameObject.name);
                yield break;
            }

            //Debug.Log("############ PrefabStarTrackPoint is Empty" + this.gameObject.name);
            //yield break;
        }
        yield return new WaitForSeconds(0.1f);

        //m_TrackPointsNavigator = Instantiate(PrefabStarTrackPoint, transform.position, Quaternion.identity);
        //m_TrackPointsNavigator = Instantiate(PrefabStarTrackPoint, new Vector3(0, 0, -10), Quaternion.identity);
        m_TrackPointsNavigator = Instantiate(PrefabStarTrackPoint);
        m_TrackPointsNavigator.name = "NavigatorTrackPoints_" + this.gameObject.name;
        //m_TrackPointsNavigator.transform.SetParent(this.gameObject.transform, true);
        m_TrackPointsNavigator.transform.SetParent(this.gameObject.transform);
        //m_TrackPointsNavigator.transform.position = new Vector3(0, 0, -10);
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

    //private SaveLoadData.GameDataUfo FindObjectData(string callFunc)
    //{
    //    var dataUfo = SaveLoadData.FindObjectData(this.gameObject) as SaveLoadData.GameDataUfo;


    //    if (dataUfo == null)
    //    {
    //        Debug.Log("#################### Error UFO MoveObjectToPosition dataUfo is Empty !!!!    :" + callFunc);
    //        return null;
    //    }

    //    if (dataUfo.NameObject != this.name)
    //    {
    //        Debug.Log("#################### Error UFO MoveObjectToPosition dataUfo: " + dataUfo.NameObject + "  GO: " + this.name + "   :" + callFunc);
    //        return null;
    //    }

    //    if (dataUfo.TargetPosition == new Vector3(0, 0, 0))
    //    {
    //        Debug.Log("#################### Error UFO dataUfo.TargetPosition is zero !!!!   :" + callFunc);
    //        return null;
    //    }

    //    return dataUfo;
    //}

    public virtual void UpdateData(string callFunc)
    {
        _dataNPC = FindObjectData<ModelNPC.GameDataNPC>(callFunc);// as SaveLoadData.GameDataNPC;
        objID = Helper.GetID(this.name);
    }

    public virtual ModelNPC.GameDataNPC GetUpdateData(string callFunc)
    {
        _dataNPC = FindObjectData<ModelNPC.GameDataNPC>(callFunc);// as SaveLoadData.GameDataNPC;
        return _dataNPC;
    }

    public void SetTarget()
    {
        Debug.Log("^^^^^^^^ TARGET --- SetTarget NPC " + this.tag + "       " + this.name);
        if (_dataNPC != null)
        {
            //Vector3 setV = Storage.Person.PersonsTargetPosition;
            //_dataNPC.SetTargetPosition(setV);
            _dataNPC.SetTargetPosition(Storage.Instance.PersonsTargetPosition);
        }
    }

  

    private void SelectedGameObject()
    {
        

        //_scriptUIEvents.SetTestText(objID);
        Storage.Events.SetTestText(objID);

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

        if (Storage.Player.HeroExtremal == true)
        {
            //Debug.Log("*********** INIT NPC **************");
            //InitNPC();
        }

        //#EXPAND
        Storage.Events.AddMenuPerson(_dataNPC, this.gameObject);

        //Storage.Log.GetHistory(_dataNPC.NameObject);
        //Debug.Log("Select: Game " + this.gameObject.name + "  OLD D: " + _dataNPC.NameObject);
        //Debug.Log("FindPersonData:  D: " + person.DataObj.NameObject);
        //Storage.Events.ListLogAdd = "------------------------";
        //Storage.Events.ListLogAdd = "Select: Game " + this.gameObject.name;
        //Storage.Events.ListLogAdd = "  OLD D: " + _dataNPC.NameObject;
        //Storage.Events.ListLogAdd = "  FindPersonData: " + person.DataObj.NameObject;

        //UpdateData("SelectedGameObject");
        //ModelNPC.GameDataNPC dataNPC = GetData();
        //if (person.DataObj.NameObject != _dataNPC.NameObject)
        //{
        //    ModelNPC.GameDataNPC persNPC = person.DataObj as ModelNPC.GameDataNPC;

        //    if (persNPC != null)
        //    {
        //        var paramsInfo = persNPC.GetParams;
        //        paramsInfo.Add("FindPersonData!!!!");

        //        Storage.Events.AddExpandPerson(persNPC.NameObject,
        //        paramsInfo,
        //        new List<string> { "Pause", "Kill", "StartTrack" },
        //        gobjObservable: this.gameObject);
        //    }
        //}

        //Storage.Events.AddExpandPerson(_dataNPC.NameObject,
        //    _dataNPC.GetParams,
        //    new List<string> { "Pause", "Kill", "StartTrack", "GoTo" },
        //    gobjObservable: this.gameObject);
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
        Storage.Events.ListLogAdd = "Me: " + this.gameObject.name;
    }

    public void SaveData()
    {
        //string _nameField = Helper.GetNameFieldByName(_dataNPC.NameObject);
        _dataNPC.Update(this.gameObject);

        if (this.gameObject == null)
        {
            Debug.Log("############# SaveData ++ This GameObject is null");
            return;
        }
    }

    public ModelNPC.GameDataNPC GetData()
    {
        return _dataNPC;
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

   

    //public void DrawTrack(Vector2 posTrack)
    //{

    //}

}
