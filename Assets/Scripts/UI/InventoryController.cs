using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    private Dictionary<string, GameObject> collectionCases;
    private GameObject m_prefabCaseInventory;

    [NonSerialized]
    public int MaxCases = 10;

    //[NonSerialized]
    private string m_CurrentCaseInvName;
    public string CurrentCaseInvName
    {
        get { return m_CurrentCaseInvName; }
    }
       
    // Use this for initialization
    void Start () {
        LoadInventoryCase();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCurrentCaseInventoryIndex(int p_indexInv)
    {
        m_CurrentCaseInvName = GetNextNameCaseInv(p_indexInv);
    }
    
    private void LoadInventoryCase()
    {
        int index = 0;
        collectionCases = new Dictionary<string, GameObject>();
        //m_prefabCaseInventory = transform.Find("InvCase").gameObject;
        m_prefabCaseInventory = transform.GetChild(0).gameObject;
        m_prefabCaseInventory.SetActive(false);
        

        for (index = 0; index < MaxCases; index++)
        {
            GameObject caseGobj = Instantiate<GameObject>(m_prefabCaseInventory);
            caseGobj.SetActive(true);
            caseGobj.transform.SetParent(this.gameObject.transform);
            //caseGobj.name += "_" + index++;

            caseGobj.name = GetNextNameCaseInv(index);
            collectionCases.Add(caseGobj.name, caseGobj);
        }
        //SpritesInventory
    }

    private string GetNextNameCaseInv(int p_indexInv)
    {
        return m_prefabCaseInventory.name + "_" + p_indexInv;
    }

    // --- ADD
    //public void AddCase(SaveLoadData.TypeInventoryObjects inventoryObject) // << add Object in first empty case
    public void AddCase(DataObjectInventory inventoryObject) // << add Object in first empty case
    {
        //SaveLoadData.TypeInventoryObjects inventoryObjectType = 

        foreach (GameObject caseInv in collectionCases.Values) //<< find case
        {
            CaseInventoryData dataInvCase = caseInv.GetComponent<CaseInventoryData>(); //<< data case
            string nameCaseObject = dataInvCase.NameInventopyObject; //<< object
            if (string.IsNullOrEmpty(nameCaseObject)) //<< exit object
            {
                UpdateCase(caseInv.name, inventoryObject); //<< add new object
                break;
            }
        }
    }
        
    public DataObjectInventory GetObjectFromCurrentCase()
    {
        return ClearCase(CurrentCaseInvName);
    }

    public DataObjectInventory ClearCase(int p_indexInv = 0)
    {
        string caseInvName = GetNextNameCaseInv(p_indexInv);
        return ClearCase(caseInvName);
    }

    public DataObjectInventory ClearCase(string caseInvName)
    {
        return UpdateCase(caseInvName, null);
    }

    public void ClearFullCase(string caseInvName)
    {
        foreach (GameObject caseInv in collectionCases.Values) //<< find case
        {
            CaseInventoryData dataInvCase = caseInv.GetComponent<CaseInventoryData>(); //<< data case
            string nameCaseObject = dataInvCase.NameInventopyObject; //<< object
            if (!string.IsNullOrEmpty(nameCaseObject) && caseInvName == caseInv.name) //<< exit object
            {
                UpdateCase(caseInv.name, null); //<< clear object in case
                break;
            }
        }
    }

    private void SortingInventory()
    {
        var cases = collectionCases.Values.Select(p => p).ToList<GameObject>();
        int indexMove = cases.Count - 1;

        foreach (GameObject caseInv in collectionCases.Values) //<< find case
        {
            CaseInventoryData dataInvCase = caseInv.GetComponent<CaseInventoryData>(); //<< data case
            string nameCaseObject = dataInvCase.NameInventopyObject; //<< object
            if (string.IsNullOrEmpty(nameCaseObject)) //<< is empty case
            {
                //var cases = collectionCases.Values.Select(p=>p).ToList<GameObject>();

                //for (int i = cases.Count -1; i<0; i--) //<< finding full case
                //{
                //    GameObject caseInvFull = cases[i];
                //    CaseInventoryData dataInvCaseMove = caseInvFull.GetComponent<CaseInventoryData>(); //<< data case
                //    if(dataInvCaseMove == null)
                //    {
                //        Debug.Log("##### CaseInventoryData not found in caseInv (Full) = " + caseInvFull.name);
                //        continue;
                //    }
                //    DataObjectInventory caseObjectMove = dataInvCaseMove.DataObjectInv; //<< object
                //    if (false == string.IsNullOrEmpty(caseObjectMove.NameInventopyObject)) //<< is full case
                //    {
                //        //Fragment sorting  - move object full to empty
                //        UpdateCase(caseInv.name, caseObjectMove); //<< move object in empty case
                //        UpdateCase(caseInvFull.name, null); //<< clear object in full case
                //        break;
                //    }
                //}
                //for (int i = cases.Count - 1; i < 0; i--) //<< finding full case
                //{
                    GameObject caseInvFull = cases[indexMove--];
                    CaseInventoryData dataInvCaseMove = caseInvFull.GetComponent<CaseInventoryData>(); //<< data case
                    if (dataInvCaseMove == null)
                    {
                        Debug.Log("##### CaseInventoryData not found in caseInv (Full) = " + caseInvFull.name);
                        continue;
                    }
                    DataObjectInventory caseObjectMove = dataInvCaseMove.DataObjectInv; //<< object
                    if (false == string.IsNullOrEmpty(caseObjectMove.NameInventopyObject)) //<< is full case
                    {
                        //Fragment sorting  - move object full to empty
                        UpdateCase(caseInv.name, caseObjectMove); //<< move object in empty case
                        UpdateCase(caseInvFull.name, null); //<< clear object in full case
                        break;
                    }
                //}
                break;
            }
        }
    }

    //public void UpdateCase(string nameCase, DataObjectInventory inventoryObject)
    //{
    //    //string nameInvObj = inventoryObject.NameInventopyObject;
    //    UpdateCase(nameCase, inventoryObject);
    //}

    //public void UpdateCase(int p_indexInv, DataObjectInventory inventoryObject)
    //{
    //    //string nameInvObj = inventoryObject.NameInventopyObject;
    //    UpdateCase(p_indexInv, inventoryObject);
    //}

    public DataObjectInventory UpdateCase(int p_indexInv, DataObjectInventory inventoryObject)
    {
        string nameCase = GetNextNameCaseInv(p_indexInv);
        return UpdateCase(nameCase, inventoryObject);
    }

    public DataObjectInventory UpdateCase(string nameCase, DataObjectInventory inventoryObject)
    {
        if (inventoryObject == null)
            inventoryObject = new DataObjectInventory("");

        DataObjectInventory oldDataObjCase = new DataObjectInventory("");

        if (!collectionCases.ContainsKey(nameCase))
        {
            Debug.Log("####### Not in Inventory nameCase = " + nameCase);
            return oldDataObjCase;
        }
        GameObject caseInv = collectionCases[nameCase];
        GameObject invCaseIcon = caseInv.transform.GetChild(0).gameObject;
        CaseInventoryData dataInvCase = caseInv.GetComponent<CaseInventoryData>();
        //SaveLoadData.TypeInventoryObjects typeInvObj = dataInvCase.DataObjectInv.TypeInventopyObject;
        //string NameInventopyObject = dataInvCase.NameInventopyObject;
        //if (typeInvObj == SaveLoadData.TypeInventoryObjects.PrefabField)

        if (string.IsNullOrEmpty(inventoryObject.NameInventopyObject))
        {
            oldDataObjCase = dataInvCase.DataObjectInv;
            dataInvCase.DataObjectInv = new DataObjectInventory("");
            invCaseIcon.SetActive(false);
        }
        else
        {
            string striteInvName = inventoryObject.NameInventopyObject + "Inv";
            if (!Storage.Palette.SpritesInventory.ContainsKey(striteInvName))
            {
                Debug.Log("####### Not in SpritesInventory striteInvName = " + striteInvName);
                return oldDataObjCase;
            }
            
            Sprite spriteObjectInv = Storage.Palette.SpritesInventory[striteInvName];
            dataInvCase.DataObjectInv = inventoryObject;
            invCaseIcon.GetComponent<SpriteRenderer>().sprite = spriteObjectInv;
            invCaseIcon.SetActive(true);
        }
        return oldDataObjCase;
    }
}
