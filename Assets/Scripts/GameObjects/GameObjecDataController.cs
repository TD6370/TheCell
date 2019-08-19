using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjecDataController : MonoBehaviour {

    protected ModelNPC.ObjectData _dataObject;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public ModelNPC.ObjectData UpdateData(string callFunc)
    {
        _dataObject = SaveLoadData.GetObjectDataByGobj(this.gameObject);
   
        if (_dataObject == null)
        {
            Debug.Log("#################### Error data game object is Empty !!!!    :" + callFunc);
            return null;
        }

        _dataObject.Init();

        if (_dataObject.NameObject != this.name)
        {
            Debug.Log("#################### Error data game object : " + _dataObject.NameObject + "  GO: " + this.name + "   :" + callFunc);
            return null;
        }

        //if (_dataObject.TargetPosition == new Vector3(0, 0, 0))
        //{
        //    Debug.Log("#################### Error UFO dataUfo.TargetPosition is zero !!!!   :" + callFunc);
        //    return null;
        //}
        return _dataObject;
    }

    public ModelNPC.ObjectData GetData()
    {
        return _dataObject;
    }
}
