using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSceneInfo : MonoBehaviour {

    public enum ModeInfo { Person, Target, None }

    public SceneDebuger.CaseSceneDialogPerson CaseDialogPerson;
    public GameObject DialogIcon;
    public GameObject DialogIconTarget;
    public GameObject DialogIconAction;
    public GameObject BorderIconTarget;
    public GameObject BorderIconAction;
    public SceneDebuger.CaseSceneDialogPerson CaseDialogTarget;

    public string DialogModelViewTarget;

    [SerializeField]
    private ModeInfo ModeViewInfo = ModeInfo.Person;
    private string m_info = "";
    private SpriteRenderer m_renderer;
    private LineRenderer m_lineRenderer;

    private Vector3 targetPositionRayDrawTest = new Vector3();

    private void Awake()
    {
        targetPositionRayDrawTest = Vector3.zero;
        m_renderer = this.gameObject.GetComponent<SpriteRenderer>();
        if (m_renderer == null)
            Debug.Log(Storage.EventsUI.ListLogAdd = "#### DialogSceneInfo.Awake m_renderer is null ");

        m_lineRenderer = GetComponent<LineRenderer>();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {


        //TEST
        if (CaseDialogTarget != null && CaseDialogTarget.IsLock && CaseDialogTarget.Dialog != null && CaseDialogTarget.Dialog.activeSelf)
        {
            if (CaseDialogPerson.IsLock && CaseDialogPerson.Person != null && ModeViewInfo == ModeInfo.Person)
            {

                // && CaseDialogTarget.Person.TargetPosition
                //Vector3 targetPosition = CaseDialogTarget.Dialog.transform.position;// .TargetPosition;    step = speed * Time.deltaTime;
                Vector3 targetPosition = targetPositionRayDrawTest = CaseDialogPerson.Person.TargetPosition;
                Vector3 pos = Vector3.MoveTowards(transform.position, targetPosition, 1 * Time.deltaTime);
                transform.position = pos;

                //CaseDialogTarget.Dialog.GetComponent<SpriteRenderer>().enabled = true;
                //DrawRayTarget();
            }
        }

        //if (CaseDialogTarget !=null && CaseDialogTarget.IsLock &&  CaseDialogPerson.IsLock && CaseDialogPerson.Person != null && ModeViewInfo == ModeInfo.Target)
        //{
        //    Vector3 targetPosition = CaseDialogTarget.Person.Position;
        //    //DrawRayTarget();
        //    RayTargetClear();
        //    var pointsRay = new Vector2[]{
        //        transform.position, targetPosition };

        //    DrawPolyline(pointsRay, new Color(20, 100, 40), 0.3f);
        //}
        //-------------

    }


    public void InitDialogView(SceneDebuger.CaseSceneDialogPerson p_caseDialogPerson, ModeInfo p_mode = ModeInfo.Person)
    {
        if (DialogIcon == null)
            return;

        var data = p_caseDialogPerson.Person.Data;

        if (p_mode == ModeInfo.Person)
        {
            CaseDialogPerson = p_caseDialogPerson;
            string modelView = data.ModelView;
            if (modelView == null)
            {
                modelView = data.TypePrefab.ToString();
            }

            if (!Storage.Palette.SpritesPrefabs.ContainsKey(modelView))
            {
                Debug.Log(Storage.EventsUI.ListLogAdd = "#### InitDialogView Not found modelView = " + modelView);
                DialogIcon.GetComponent<SpriteRenderer>().sprite = null;
            }
            else
            {
                Sprite spriteGobject = Storage.Palette.SpritesPrefabs[modelView];
                DialogIcon.GetComponent<SpriteRenderer>().sprite = spriteGobject;
            }

            string spriteNameAction = "ActionIcon" + data.CurrentAction.ToString();
            //"ActionIconMove"
            if (!Storage.Palette.SpritesUI.ContainsKey(spriteNameAction))
            {
                Debug.Log(Storage.EventsUI.ListLogAdd = "#### InitDialogView Not found spriteNameAction = " + spriteNameAction);
                DialogIconAction.GetComponent<SpriteRenderer>().sprite = null;
            }
            else
            {
                Sprite spriteAction = Storage.Palette.SpritesUI[spriteNameAction];
                DialogIconAction.GetComponent<SpriteRenderer>().sprite = spriteAction;
            }

            string fieldTarget = Helper.GetNameFieldPosit(data.TargetPosition.x, data.TargetPosition.y);
            if (fieldTarget == null)
            {
                Debug.Log(Storage.EventsUI.ListLogAdd = "#### fieldTarget is null ");
                return;
            }
            var objectsGrid = ReaderScene.GetObjectsDataFromGridContinue(fieldTarget);
            if (objectsGrid != null)
            {
                //Debug.Log(Storage.EventsUI.ListLogAdd = "#### objectsGrid is null from " + fieldTarget);
                //return;
                foreach (var objData in objectsGrid)
                {
                    if (objData == null)
                    {
                        Storage.EventsUI.ListLogAdd = "### TARGET ReaderScene NOT FIELD: " + fieldTarget;
                        continue;
                    }

                    string modelViewTarget = objData.ModelView;
                    if (modelViewTarget == null)
                        modelViewTarget = objData.TypePrefabName;

                    if (modelViewTarget == null)
                    {
                        Debug.Log(Storage.EventsUI.ListLogAdd = "#### InitDialogView Not found modelViewTarget is null >> " + objData.NameObject);
                        continue;
                    }

                    if (!Storage.Palette.SpritesPrefabs.ContainsKey(modelViewTarget))
                    {
                        Debug.Log(Storage.EventsUI.ListLogAdd = "#### InitDialogView Not found modelViewTarget = " + modelViewTarget);
                        DialogIconTarget.GetComponent<SpriteRenderer>().sprite = null;
                    }
                    else
                    {
                        Sprite spriteTarget = Storage.Palette.SpritesPrefabs[modelViewTarget];
                        DialogIconTarget.GetComponent<SpriteRenderer>().sprite = spriteTarget;

                        if (CaseDialogTarget != null && CaseDialogTarget.IsLock && CaseDialogTarget.Person != null && CaseDialogTarget.Person.ID == p_caseDialogPerson.Person.ID)
                        {
                            Storage.SceneDebug.UpdateTargetDialog(CaseDialogTarget, p_caseDialogPerson.Person, modelViewTarget);
                        }
                        else
                            CaseDialogTarget = Storage.SceneDebug.CreateTargetDialog(p_caseDialogPerson.Person, modelViewTarget);

                        DrawRayTarget(); //!!!!!!!!!!!!!!!!!!!!!!
                        break;
                    }
                }
            }
        }
        else
        {
            CaseDialogPerson = p_caseDialogPerson;
            DialogIcon.GetComponent<SpriteRenderer>().sprite = Storage.Palette.SpritesPrefabs[DialogModelViewTarget]; 
        }
        m_info = data.NameObject;
        SetMode(p_mode);
    }

    private void SetMode(ModeInfo mode)
    {
        //Debug.Log("################# TEST MSetMode :" + mode.ToString());
        ModeViewInfo = mode;
        switch (ModeViewInfo)
        {
            case ModeInfo.Person:
                //Debug.Log("TEST MSetMode : Person " + m_info);
                BorderIconTarget.SetActive(true);
                BorderIconAction.SetActive(true);
                //m_renderer.enabled = true;
                //m_renderer.color = Color.white;
                BorderIconAction.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case ModeInfo.Target:
                //TEST
                RayTargetClear();
                //Debug.Log("TEST MSetMode : Target " + m_info);
                //m_renderer.enabled = false;
                //m_renderer.color = "#9900ff".ToColor();
                BorderIconAction.GetComponent<SpriteRenderer>().color = Color.yellow;
                BorderIconTarget.SetActive(false);
                BorderIconAction.SetActive(false);
                break;
        }
    }

    public void Deactivate()
    {
        DialogIcon.GetComponent<SpriteRenderer>().sprite = null;
        DialogIconTarget.GetComponent<SpriteRenderer>().sprite = null;
        BorderIconAction.GetComponent<SpriteRenderer>().color = Color.white;
        //m_renderer.enabled = false;
        //m_renderer.color = "#9900ff".ToColor();
        BorderIconTarget.SetActive(false);
        BorderIconAction.SetActive(false);

        //ModeViewInfo = ModeInfo.None;
        RayTargetClear();
        targetPositionRayDrawTest = Vector3.zero;
    }

    public void RayTargetClear()
    {
        m_lineRenderer.positionCount = 0;
    }

    private void DrawRayTarget()
    {
        RayTargetClear();

        var target = CaseDialogTarget.Person;

        var pointsRay = new Vector2[]{
            transform.position,
            target.TargetPosition
        };
        //DrawPolyline(pointsRay, "#0000ff".ToColor(), 1f);
        DrawPolyline(pointsRay, new Color(20,100,40), 0.3f);
    }

    public void DrawPolyline(Vector2[] points, Color color, float width = 0.2f)
    {
        //return;

        if (m_lineRenderer == null)
        {
            Debug.Log("LineRenderer is null !!!!");
            return;
        }

        var colorKeys = new GradientColorKey[] { new GradientColorKey() { color = color } };
        //m_lineRenderer.SetColors(color, color);
        m_lineRenderer.colorGradient = new Gradient() { colorKeys = colorKeys };
        m_lineRenderer.SetWidth(width, width);
        int size = points.Length;
        m_lineRenderer.SetVertexCount(size);
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 posPoint = new Vector3(points[i].x, points[i].y, -2);
            m_lineRenderer.SetPosition(i, posPoint);
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.yellow;

        if (Camera.main == null)
            return;

        //m_info = "@";

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Rect position1 = new Rect(screenPosition.x - 10, Screen.height - screenPosition.y - 30, 300, 100);
        GUI.Label(position1, m_info, style);

        //Rect position2 = new Rect(screenPosition.x - 10, Screen.height - screenPosition.y - 70, 300, 100);
        //GUI.TextField(position2, m_info, 25);
        //Handles.DrawLine(center, t.GameObjects[i].transform.position);
    }


    private void DrawGizmosLine(Vector3 pos1, Vector3 pos2, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(pos1, pos2);
    }

    private void OnDrawGizmos()
    {
        //TEST
        //if (ModeViewInfo == ModeInfo.Person &&
        //    CaseDialogTarget != null &&
        //    CaseDialogTarget.Person != null &&
        //    m_lineRenderer.positionCount > 0)
        //{
        //    var targetPosition = CaseDialogTarget.Person.TargetPosition;
        //    //var targetPosition = CaseDialogTarget.Person.Position;
        //    //var targetPosition = CaseDialogTarget.Dialog.transform.position;
        //    //Gizmos.DrawLine(transform.position, targetPosition);
        //    DrawGizmosLine(transform.position, targetPosition, "#8533ff".ToColor());
        //}

        if(targetPositionRayDrawTest != Vector3.zero)
        {
            DrawGizmosLine(transform.position, targetPositionRayDrawTest, "#8533ff".ToColor());
        }
    }

    public float explosionRadius = 0.5f;

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        //Gizmos.color = new Color(50, 0, 50, 0.5F);
        //Gizmos.DrawSphere(transform.position, explosionRadius);

        //if (ModeViewInfo ==  ModeInfo.Person && CaseDialogTarget != null && CaseDialogTarget.Person != null)
        //{
        //    // Draws a blue line from this transform to the target
        //    var target = CaseDialogTarget.Person;
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawLine(transform.position, target.Position);
        //}
    }

    private void OnDisable()
    {
        targetPositionRayDrawTest = Vector3.zero;
    }

}
