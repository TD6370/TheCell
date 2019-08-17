using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    private Dictionary<string, GameObject> collectionCases;

    [NonSerialized]
    public int MaxCases = 10;

    private string m_CurrentCaseInvName;
    public string CurrentCaseInvName {
        get { return m_CurrentCaseInvName; }
    }

    // Use this for initialization
    void Start () {
        LoadInventoryCase();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private GameObject m_prefabCaseInventory;

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
    public void AddCase(SaveLoadData.TypePrefabs inventoryObject) // << add Object in first empty case
    {
        foreach(GameObject caseInv in collectionCases.Values) //<< find case
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
        
    public void ClearCurrentCase()
    {
        ClearCase(m_CurrentCaseInvName);
    }

    public void ClearCase(int p_indexInv = 0)
    {
        string caseInvName = GetNextNameCaseInv(p_indexInv);
        ClearCase(caseInvName);
    }

    public void ClearCase(string caseInvName)
    {
        UpdateCase(caseInvName, "");
    }

    public void ClearFullCase(string caseInvName)
    {
        foreach (GameObject caseInv in collectionCases.Values) //<< find case
        {
            CaseInventoryData dataInvCase = caseInv.GetComponent<CaseInventoryData>(); //<< data case
            string nameCaseObject = dataInvCase.NameInventopyObject; //<< object
            if (!string.IsNullOrEmpty(nameCaseObject) && caseInvName == caseInv.name) //<< exit object
            {
                UpdateCase(caseInv.name, ""); //<< clear object in case
                break;
            }
        }
    }

    private void SortingInventory()
    {
        foreach (GameObject caseInv in collectionCases.Values) //<< find case
        {
            CaseInventoryData dataInvCase = caseInv.GetComponent<CaseInventoryData>(); //<< data case
            string nameCaseObject = dataInvCase.NameInventopyObject; //<< object
            if (string.IsNullOrEmpty(nameCaseObject)) //<< is empty case
            {
                var cases = collectionCases.Values.Select(p=>p).ToList<GameObject>();

                for (int i = cases.Count -1; i<0; i--) //<< finding full case
                {
                    GameObject caseInvFull = cases[i];
                    CaseInventoryData dataInvCaseMove = caseInvFull.GetComponent<CaseInventoryData>(); //<< data case
                    if(dataInvCaseMove == null)
                    {
                        Debug.Log("##### CaseInventoryData not found in caseInv (Full) = " + caseInvFull.name);
                        continue;
                    }
                    string nameCaseObjectMove = dataInvCaseMove.NameInventopyObject; //<< object
                    if (false == string.IsNullOrEmpty(nameCaseObjectMove)) //<< is full case
                    {
                        //Fragment sorting  - move object full to empty
                        UpdateCase(caseInv.name, nameCaseObjectMove); //<< move object in empty case
                        UpdateCase(caseInvFull.name, ""); //<< clear object in full case
                        break;
                    }
                }
                break;
            }
        }
    }

    public void UpdateCase(string nameCase, SaveLoadData.TypePrefabs inventoryObject)
    {
        string nameInvObj = inventoryObject.ToString() + "Inv";
        UpdateCase(nameCase, nameInvObj);
    }

    public void UpdateCase(int p_indexInv, SaveLoadData.TypePrefabs inventoryObject)
    {
        string nameInvObj = inventoryObject.ToString()+ "Inv";
        UpdateCase(p_indexInv, nameInvObj);
    }

    public void UpdateCase(int p_indexInv, string striteInvName = "")
    {
        string nameCase = GetNextNameCaseInv(p_indexInv);
        UpdateCase(nameCase, striteInvName);
    }

    public void UpdateCase(string nameCase, string striteInvName = "")
    {
        if (!collectionCases.ContainsKey(nameCase))
        {
            Debug.Log("####### Not in Inventory nameCase = " + nameCase);
            return;
        }
        GameObject caseInv = collectionCases[nameCase];
        GameObject invCaseIcon = caseInv.transform.GetChild(0).gameObject;
        CaseInventoryData dataInvCase = caseInv.GetComponent<CaseInventoryData>();
        SaveLoadData.TypePrefabs typeInvObj = dataInvCase.TypeInventopyObject;
        string NameInventopyObject = dataInvCase.NameInventopyObject;
        if (typeInvObj == SaveLoadData.TypePrefabs.PrefabField)

            if (string.IsNullOrEmpty(striteInvName))
            {
                //invCaseIcon.GetComponent<SpriteRenderer>().sprite = null;
                //TypeInventopyObject
                dataInvCase.NameInventopyObject = string.Empty;
                invCaseIcon.SetActive(false);
            }
            else
            {
                if (!Storage.Palette.SpritesInventory.ContainsKey(striteInvName))
                {
                    Debug.Log("####### Not in SpritesInventory striteInvName = " + striteInvName);
                    return;
                }
                Sprite spriteObjectInv = Storage.Palette.SpritesInventory[striteInvName];
                dataInvCase.NameInventopyObject = striteInvName.Replace("Inv", "");
                invCaseIcon.GetComponent<SpriteRenderer>().sprite = spriteObjectInv;
                invCaseIcon.SetActive(true);
            }
    }
}
