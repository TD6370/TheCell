using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalData : MonoBehaviour {

    private SaveLoadData.ObjectData m_personalObjectData = null;
    public SaveLoadData.ObjectData PersonalObjectData
    {
        get {
            if (m_personalObjectData != null)
            {
                //#+FIX ---------------------
                //Debug.Log(" GET :: old pos" + this.transform.position);
                //m_personalObjectData.Position = this.transform.position;
                //Debug.Log(" GET :: old pos FIXED" + this.transform.position);
            }


            //string log = (m_personalObjectData==null) ? "null" : m_personalObjectData.ToString();
            //Debug.Log("........ PersonalObjectData .... GET " + log);
            return m_personalObjectData; 
        }
        set {
            //string log = (value == null) ? "null" : value.ToString();
            //Debug.Log("........ PersonalObjectData .... SET " + log);
            m_personalObjectData = value; 
        }
    }

	// Use this for initialization
	void Start () {

        //string log = (m_personalObjectData == null) ? "null" : PersonalObjectData.ToString();
        //Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++");
        //Debug.Log("+++++ NEW PersonalObjectData init " + this.name + "    PersonalObjectData: " + log);
        if (PersonalObjectData == null)
        {
            //Debug.Log("CREATE new PersonalObjectData " + this.name);
            PersonalObjectData = new SaveLoadData.ObjectData()
            {
                NameObject = "Empty",
                TagObject = SaveLoadData.TypePrefabs.PrefabField.ToString()
            };
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
}
