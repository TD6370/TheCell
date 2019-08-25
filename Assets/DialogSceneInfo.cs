using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSceneInfo : MonoBehaviour {

    public SceneDebuger.CaseSceneDialogPerson CaseDialogPerson;
    public GameObject DialogIcon;
    public GameObject DialogIconTarget;
    public GameObject DialogIconAction;

    private string m_info = "";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitDialogView(SceneDebuger.CaseSceneDialogPerson p_caseDialogPerson)
    {
        if (DialogIcon == null)
            return;

        var data = p_caseDialogPerson.Person.Data;
        CaseDialogPerson = p_caseDialogPerson;
        string modelView = data.ModelView;
        if (!Storage.Palette.SpritesPrefabs.ContainsKey(modelView))
        {
            Debug.Log("########## InitDialogView Not found modelView = " + modelView);
            DialogIcon.GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            Sprite spriteGobject = Storage.Palette.SpritesPrefabs[modelView];
            DialogIcon.GetComponent<SpriteRenderer>().sprite = spriteGobject;
        }

        string spriteNameAction = "ActionIcon" + data.CurrentAction.ToString();
        if (!Storage.Palette.SpritesUI.ContainsKey(spriteNameAction))
        {
            Debug.Log("########## InitDialogView Not found spriteNameAction = " + spriteNameAction);
            DialogIconAction.GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            Sprite spriteAction = Storage.Palette.SpritesUI[spriteNameAction];
            DialogIconAction.GetComponent<SpriteRenderer>().sprite = spriteAction;
        }

        string fieldTarget = Helper.GetNameFieldPosit(data.TargetPosition.x, data.TargetPosition.y);
        foreach (var objData in ReaderScene.GetObjectsDataFromGrid(fieldTarget))
        {
            string modelViewTarget = objData.ModelView;
            if (!Storage.Palette.SpritesPrefabs.ContainsKey(modelViewTarget))
            {
                Debug.Log("########## InitDialogView Not found modelViewTarget = " + modelViewTarget);
                DialogIconTarget.GetComponent<SpriteRenderer>().sprite = null;
            }
            else
            {
                Sprite spriteTarget = Storage.Palette.SpritesPrefabs[modelViewTarget];
                DialogIconTarget.GetComponent<SpriteRenderer>().sprite = spriteTarget;
                break;
            }
        }
        m_info = data.NameObject;
    }

    void OnGUI()
    {
        // Make a text field that modifies stringToEdit.
        GUI.TextField(new Rect(10, 10, 200, 20), m_info, 25);
    }

}
