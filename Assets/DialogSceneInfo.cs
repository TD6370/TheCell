using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSceneInfo : MonoBehaviour {

    public enum ModeInfo { Person, Target }

    public SceneDebuger.CaseSceneDialogPerson CaseDialogPerson;
    public GameObject DialogIcon;
    public GameObject DialogIconTarget;
    public GameObject DialogIconAction;
    public GameObject BorderIconTarget;
    public GameObject BorderIconAction;
    public SceneDebuger.CaseSceneDialogPerson CaseDialogTarget;

    public string DialogModelViewTarget;

    //private GameObject m_targetDialog;
    [SerializeField]
    private ModeInfo ModeViewInfo = ModeInfo.Person;
    private string m_info = "";
    private SpriteRenderer m_renderer;

    private void Awake()
    {
        m_renderer = this.gameObject.GetComponent<SpriteRenderer>();
        if (m_renderer == null)
            Debug.Log(Storage.EventsUI.ListLogAdd = "#### DialogSceneInfo.Awake m_renderer is null ");
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
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
                //string nameVM = data.TypePrefab.ToString();

                //if (!Storage.Palette.SpritesPrefabs.ContainsKey(nameVM))
                //{
                //    Debug.Log(Storage.EventsUI.ListLogAdd = "#### InitDialogView modelView is Null >> " + data.NameObject);
                //    return;
                //}
                //p_caseDialogPerson.ID
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
            if (objectsGrid == null)
            {
                //Debug.Log(Storage.EventsUI.ListLogAdd = "#### objectsGrid is null from " + fieldTarget);
                return;
            }


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

                    CaseDialogTarget = Storage.SceneDebug.CreateTargetDialog(p_caseDialogPerson.Person, modelViewTarget);
                    break;
                }
            }
        }
        else
        {
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
                Debug.Log("TEST MSetMode : Person " + m_info);
                BorderIconTarget.SetActive(true);
                BorderIconAction.SetActive(true);
                m_renderer.enabled = true;
                m_renderer.color = Color.white;
                break;
            case ModeInfo.Target:
                //TEST
                Debug.Log("TEST MSetMode : Target " + m_info);
                m_renderer.enabled = false;
                m_renderer.color = Color.red;
                BorderIconTarget.SetActive(false);
                BorderIconAction.SetActive(false);
                break;
        }
    }

    //void OnGUI()
    //{
    //    // Make a text field that modifies stringToEdit.
    //    //GUI.TextField(new Rect(10, 10, 200, 20), m_info, 25);
    //}

    public float explosionRadius = 0.5f;
       
    

    //void OnDrawGizmosSelected()
    //{
    //    // Display the explosion radius when selected
    //    Gizmos.color = new Color(50, 0, 50, 0.5F);
    //    Gizmos.DrawSphere(transform.position, explosionRadius);
    //}

}
