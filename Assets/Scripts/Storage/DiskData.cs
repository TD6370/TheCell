using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class DiskData : MonoBehaviour 
{

    private float LoadingWordlTimer = 0f;

    //public DiskData() { }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadDataBigXML()
    {
        //--load parts 
        //StartCoroutine(StartLoadDataPartsXML());
        //--load asinc
        //StartCoroutine(StartLoadDataBigXML());

        //-- Load in thread
        //StartCoroutine(StartBackgroundLoadDataBigXML());

        //-- load old style
        //StartCoroutine(StartInGameLoadDataBigXML());

        //-- Load in thread full
        StartCoroutine(StartThreadLoadDataBigXML());
    }

    public void SaveLevel()
    {
        SaveLevelCash();
    }

    /*
    public void SaveLevelParts()
    {

        Storage.GenGrid.SaveAllRealGameObjects();
        if (Storage.Instance.GridDataG == null)
        {
            Debug.Log("Error SaveLevel gridData is null !!!");
            return;
        }
        Serializator.SaveGridPartsXml(Storage.Instance.GridDataG, Storage.Instance.DataPathLevel, true);
    }
    */

    public void SaveLevelCash()
    {
        Storage.GenGrid.SaveAllRealGameObjects();
        if (Storage.Instance.GridDataG == null)
        {
            Debug.Log("Error SaveLevel gridData is null !!!");
            return;
        }
        Serializator.SaveGridCashXml(Storage.Instance.GridDataG, Storage.Instance.DataPathLevel, true);
    }

    /*
    IEnumerator StartInGameLoadDataBigXML()
    {
        yield return new WaitForSeconds(2f);

        Storage.Events.SetMessage("Подожди говнюк...");

        yield return null;

        LoadingWordlTimer = Time.time;
        var fieldsD_Temp = Serializator.LoadGridXml(Storage.Data.DataPathBigPart);
        //yield return null;
        Storage.Data.SetGridDatatBig = fieldsD_Temp.FieldsD;

        //yield return null;
        Storage.Data.CompletedLoadWorld();//fieldsD_Temp

        float loadingTime = Time.time - LoadingWordlTimer;
        Storage.Events.SetMessage("Ты ждал: " + loadingTime);

        yield return new WaitForSeconds(0.5f);

        Storage.Events.HideMessage();

        yield break;
    }
    */

    /*
    IEnumerator StartBackgroundLoadDataBigXML()
    {
        yield return null;

        LoadingWordlTimer = Time.time;

        Storage.Data.LoadBigWorldThread();

        yield return new WaitForSeconds(1f);

        while (!Storage.Data.IsThreadLoadWorldCompleted)
        {

            yield return new WaitForSeconds(2f);
        }

        Storage.Data.CompletedLoadWorld();

        float loadingTime = Time.time - LoadingWordlTimer;
        Storage.Events.SetTittle = "Loaded:" + loadingTime;
        Storage.Events.ListLogAdd = "Loaded:" + loadingTime;
        Debug.Log("*********************** Time loding World: " + loadingTime);

        yield break;
    }
    */

    IEnumerator StartThreadLoadDataBigXML()
    {
        Storage.EventsUI.SetTittle = "StartThreadLoadDataBigXML";
        Storage.EventsUI.ListLogAdd = "StartThreadLoadDataBigXML";

        yield return null;

        LoadingWordlTimer = Time.time;

        Storage.Data.LoadBigWorldThread(true);

        //yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0.5f);

        while (!Storage.Data.IsThreadLoadWorldCompleted)
        {

            yield return new WaitForSeconds(0.2f);
            //yield return new WaitForSeconds(0.3f);
        }

        Storage.EventsUI.ListLogAdd = "....CompletedLoadWorld:";
        Storage.Data.CompletedLoadWorld();

        float loadingTime = Time.time - LoadingWordlTimer;
        Storage.EventsUI.SetTittle = "Loaded:" + loadingTime;
        Storage.EventsUI.ListLogAdd = "Loaded:" + loadingTime;
        Debug.Log("*********************** Time loding World: " + loadingTime);

        yield return null;
        System.GC.Collect();
        yield return null;

        Storage.Instance.IsLoadingWorldThread = false;
        Storage.Instance.InitCollectionID();

        yield return null;
        System.GC.Collect();
        yield break;
    }

    IEnumerator StartLoadDataBigXML()
    {
        string stepErr = "start";
        Debug.Log("Loaded Xml GridData start...");

        Dictionary<string, ModelNPC.FieldData> fieldsD_Test = new Dictionary<string, ModelNPC.FieldData>();

        yield return null;

        LoadingWordlTimer = Time.time;

        string nameField = "";

        stepErr = "c.1";
        string datapathPart = Application.dataPath + "/Levels/LevelDataPart1x2.xml";
        if (File.Exists(datapathPart))
        {

            int indProgress = 0;
            int limitUpdate = 20;

            //using (XmlReader xml = XmlReader.Create(stReader))
            using (XmlReader xml = XmlReader.Create(datapathPart))
            //using (XmlReader xml = XmlReader.Create(new StreamReader(datapathPart, System.Text.Encoding.UTF8)))
            {
                while (xml.Read())
                {
                    switch (xml.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (xml.Name == "Key")
                            {
                                XElement el = XElement.ReadFrom(xml) as XElement;
                                nameField = el.Value;
                                //nameField = xml.Value.Clone().ToString();
                                break;
                            }
                            //if (xml.Name == "Objects")
                            if (xml.Name == "ObjectData") //WWW
                            {
                                indProgress++;
                                if (indProgress > limitUpdate)
                                {
                                    indProgress = 0;
                                    yield return null;
                                }

                                XElement el = XElement.ReadFrom(xml) as XElement;
                                string inputString = el.ToString();

                                //---------------
                                //XmlSerializer serializer = new XmlSerializer(typeof(List<ModelNPC.ObjectData>), Serializator.extraTypes);
                                //WWW
                                XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.ObjectData), Serializator.extraTypes);
                                //--------------
                                StringReader stringReader = new StringReader(inputString);
                                //stringReader.Read(); // skip BOM
                                //--------------

                                //List<KeyValuePair<string, ModelNPC.FieldData>> dataResult = (List<KeyValuePair<string, ModelNPC.FieldData>>)serializer.Deserialize(rdr);
                                //Debug.Log("! " + inputString);
                                //List<ModelNPC.ObjectData> dataResult;
                                ModelNPC.ObjectData dataResult;
                                try
                                {
                                    dataResult = (ModelNPC.ObjectData)serializer.Deserialize(stringReader);
                                }
                                catch (Exception x)
                                {
                                    Debug.Log("############# " + x.Message);
                                    yield break;
                                }
                                //-------------------------
                                if (ReaderScene.IsGridDataFieldExist(nameField))
                                {
                                    fieldsD_Test[nameField].Objects.Add(dataResult);
                                }
                                else
                                {
                                    //_GridDataG.FieldsD.Add(nameField, new ModelNPC.FieldData()
                                    //{
                                    //    NameField = nameField,
                                    //    Objects = new List<ModelNPC.ObjectData>() { dataResult }
                                    //});
                                    fieldsD_Test.Add(nameField, new ModelNPC.FieldData()
                                    {
                                        NameField = nameField,
                                        Objects = new List<ModelNPC.ObjectData>() { dataResult }
                                    });
                                }
                            }
                            break;
                    }
                }
            }

            //xml.Close();
            //stReader.Close();
        }

        //------------
        Storage.Data.SetGridDatatBig = fieldsD_Test;

        yield return null;
        Storage.Data.CompletedLoadWorld();//fieldsD_Temp
        //--------------

        float loadingTime = Time.time - LoadingWordlTimer;
        Storage.EventsUI.SetMessageBox = "Ты ждал: " + loadingTime;

        yield return new WaitForSeconds(4f);

        Storage.EventsUI.HideMessage();
    }

    IEnumerator StartLoadDataPartsXML()
    {
        string datapath;
        string stepErr = "start";
        Debug.Log("Loaded Xml GridData start...");

        yield return null;

        for (int partX = 0; partX < Helper.SizePart; partX++)
        {
            for (int partY = 0; partY < Helper.SizePart; partY++)
            {
                //--- skip first part
                if (partX == 0 && partY == 0)
                    continue;

                //yield return null;

                stepErr = "c.1";
                string nameFileXML = +(partX + 1) + "x" + (partY + 1);
                string datapathPart = Application.dataPath + "/Levels/LevelDataPart" + nameFileXML + ".xml";
                datapath = datapathPart;
                if (File.Exists(datapathPart))
                {
                    //yield return new WaitForSeconds(0.3f);
                    yield return null;
                    yield return StartCoroutine(StartLoadPartXML(datapathPart));
                }

            }
        }
    }

    IEnumerator StartLoadPartXML(string datapathPart)
    {
        string stepErr = "start";

        //yield return null;
        ModelNPC.GridData itemGridData = null;
        stepErr = ".1";
        stepErr = ".2";
        XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), Serializator.extraTypes);
        stepErr = ".3";
        using (FileStream fs = new FileStream(datapathPart, FileMode.Open))
        {
            stepErr = ".4";
            itemGridData = (ModelNPC.GridData)serializer.Deserialize(fs);
            stepErr = ".5";
            fs.Close();
        }
        yield return null;
        stepErr = ".6";
        itemGridData.FieldsD = itemGridData.FieldsXML.ToDictionary(x => x.Key, x => x.Value);
        stepErr = ".7";
        Debug.Log("Loaded Xml GridData D:" + itemGridData.FieldsD.Count() + "     datapath=" + datapathPart);
        //## 
        itemGridData.FieldsXML = null;
        stepErr = ".8";
        //yield return null;
        Storage.Instance.GridDataG.FieldsD = Storage.Instance.GridDataG.FieldsD.Concat(itemGridData.FieldsD)
            .ToDictionary(x => x.Key, x => x.Value);

    }
}
