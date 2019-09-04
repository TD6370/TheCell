using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Text;


#region Serialize Helper

public class Serializator
{
    public static Type[] extraTypes = {
        typeof(ModelNPC.FieldData),
        typeof(ModelNPC.ObjectData),

        typeof(ModelNPC.GameDataUfo),

        typeof(ModelNPC.GameDataNPC),
        typeof(ModelNPC.PersonData),

        typeof(ModelNPC.PersonDataBoss),
        typeof(ModelNPC.GameDataBoss),
        typeof(TilesData),
        typeof(DataTile),
        typeof(DataConstructionTiles),
        typeof(ModelNPC.WallData),
        typeof(ModelNPC.TerraData),
        typeof(ModelNPC.FloorData),
        typeof(ModelNPC.FloreData),
            

        typeof(ModelNPC.Swamp),
        typeof(ModelNPC.Chip),
        typeof(ModelNPC.Gecsagon),
        typeof(ModelNPC.Kamish),

        typeof(ModelNPC.Berry),
        typeof(ModelNPC.Mashrooms),
        typeof(ModelNPC.Weed),
        typeof(ModelNPC.Weedflower),

        typeof(ModelNPC.Kishka),
        typeof(ModelNPC.Nerv),
        typeof(ModelNPC.Orbits),
        typeof(ModelNPC.Shampinion),

        typeof(ModelNPC.Corals),
        typeof(ModelNPC.Desert),
        typeof(ModelNPC.Diods),
        typeof(ModelNPC.Parket),

        typeof(ModelNPC.Ground05), typeof(ModelNPC.Ground04), typeof(ModelNPC.Ground03), typeof(ModelNPC.Ground02), typeof(ModelNPC.Ground),
        typeof(ModelNPC.GrassSmall), typeof(ModelNPC.GrassMedium), typeof(ModelNPC.Grass),
        typeof(ModelNPC.Iris), typeof(ModelNPC.Osoka), typeof(ModelNPC.Tussok),

        typeof(ModelNPC.GameDataAlienInspector),
        typeof(ModelNPC.GameDataAlienMachinetool),
        typeof(ModelNPC.GameDataAlienMecha),

        typeof(ModelNPC.GameDataAlienDendroid),
        typeof(ModelNPC.GameDataAlienGarry),
        typeof(ModelNPC.GameDataAlienLollipop),

        typeof(ModelNPC.GameDataAlienBlastarr),
        typeof(ModelNPC.GameDataAlienHydragon),
        typeof(ModelNPC.GameDataAlienPavuk),
        typeof(ModelNPC.GameDataAlienSkvid),

        typeof(ModelNPC.GameDataAlienFantom),
        typeof(ModelNPC.GameDataAlienMask),
        typeof(ModelNPC.GameDataAlienVhailor),

        typeof(ModelNPC.GameDataAlienEj),

        typeof(ModelNPC.Kolba),
        typeof(ModelNPC.Lantern),

        typeof(ModelNPC.Bananas),
        typeof(ModelNPC.Cluben),

        typeof(ModelNPC.Chpok),
        typeof(ModelNPC.Pandora),

        typeof(ModelNPC.Nadmozg),
        typeof(ModelNPC.Triffid),
        typeof(ModelNPC.Aracul),
        typeof(ModelNPC.Cloudwood),

        typeof(ModelNPC.Elka),
        typeof(ModelNPC.Vood),
        typeof(ModelNPC.Rock),
        typeof(ModelNPC.WallRock),
        typeof(ModelNPC.WallWood),
                       
        typeof(ModelNPC.RockDark),
        typeof(ModelNPC.RockValun),
        typeof(ModelNPC.RockBrow),
        typeof(ModelNPC.Klen),
        typeof(ModelNPC.Iva),
        typeof(ModelNPC.Sosna),
        typeof(ModelNPC.BlueBerry) 
    };

    static public void SaveGridXml(ModelNPC.GridData state, string datapath, bool isNewWorld = false)
    {
        string indErr = "start";

        //------- Save parts
        //SaveGridPartsXml(state, datapath,isNewWorld);
        //------- Save cash and big
        SaveGridCashXml(state, datapath, isNewWorld);
        return;
        //----------------


        if (isNewWorld)
        {
            if (File.Exists(datapath))
            {
                try
                {
                    indErr = "delete";
                    File.Delete(datapath);
                }
                catch (Exception x)
                {
                    Debug.Log("############# Error SaveGridXml NOT File Delete: " + datapath + " : " + x.Message);
                }
            }
        }

        try
        {
            indErr = "1";
            //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
            //## 
            state.FieldsXML = state.FieldsD.ToList();
            indErr = "2";
            //## 
            Debug.Log("SaveXml GridData D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);

            indErr = "3";
            XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);

            indErr = "5";
            FileStream fs = new FileStream(datapath, FileMode.Create);

            indErr = "6";
            serializer.Serialize(fs, state);

            indErr = "7";
            fs.Close();

            indErr = "8";
            state.FieldsXML = null;
            //Debug.Log("Saved Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
        }
        catch (Exception x)
        {
            Debug.Log("######### SaveGridXml: " + x.Message + "     to :" + datapath);
        }
    }

    /*
    static public void SaveGridPartsXml(ModelNPC.GridData state, string datapath, bool isNewWorld = false)
    {

        string indErr = "start";

        if (isNewWorld)
        {
            for (int partX = 0; partX < Helper.SizePart; partX++)
            {
                for (int partY = 0; partY < Helper.SizePart; partY++)
                {
                    indErr = "c.1";
                    string nameFileXML = +(partX + 1) + "x" + (partY + 1);
                    string datapathPart = Application.dataPath + "/Levels/LevelDataPart" + nameFileXML + ".xml";
                    datapath = datapathPart;
                    if (File.Exists(datapathPart))
                    {
                        try
                        {
                            indErr = "delete";
                            File.Delete(datapathPart);
                        }
                        catch (Exception x)
                        {
                            Debug.Log("############# Error SaveGridXml NOT File Delete: " + datapathPart + " : " + x.Message);
                        }
                    }

                }
            }


        }

        try
        {
            Dictionary<string, ModelNPC.FieldData> FieldsPart = new Dictionary<string, ModelNPC.FieldData>();
            int SizePart = Helper.WidthLevel / Helper.SizePart;

            Dictionary<string, Dictionary<string, ModelNPC.FieldData>> PartsGrids = new Dictionary<string, Dictionary<string, ModelNPC.FieldData>>();
            for (int partX = 0; partX < Helper.SizePart; partX++)
            {
                for (int partY = 0; partY < Helper.SizePart; partY++)
                {
                    FieldsPart = new Dictionary<string, ModelNPC.FieldData>();
                    string nameFileXML = +(partX + 1) + "x" + (partY + 1);
                    int startX = partX * SizePart;
                    int startY = partY * SizePart;
                    int widthX = startX + SizePart;
                    int widthY = startY + SizePart;
                    for (int x = startX; x < widthX; x++)
                    {
                        for (int y = startY; y < widthY; y++)
                        {
                            indErr = "d.1";
                            string fieldName = Helper.GetNameField(x, y);
                            indErr = "d.2";
                            if (state.FieldsD.ContainsKey(fieldName))
                            {
                                ModelNPC.FieldData copyFields = state.FieldsD[fieldName];

                                FieldsPart.Add(fieldName, copyFields);
                            }
                        }
                    }
                    indErr = "d.3";
                    PartsGrids.Add(nameFileXML, FieldsPart);
                }
            }

            foreach (var partGrid in PartsGrids)
            {
                indErr = "1";
                //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
                //## 
                Dictionary<string, ModelNPC.FieldData> partGridFields = partGrid.Value;
                indErr = "1.1.";
                string nameFileXML = partGrid.Key;
                indErr = "1.2.";
                string datapathPart = Application.dataPath + "/Levels/LevelDataPart" + nameFileXML + ".xml";
                datapath = datapathPart;

                indErr = "1.3.";

                state.FieldsXML = partGridFields.ToList();
                indErr = "2";
                //## 
                Debug.Log("SaveXml GridData D:" + partGridFields.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapathPart);

                indErr = "3";
                XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);

                indErr = "5";
                //FileStream fs = new FileStream(datapathPart, FileMode.CreateNew);

                //indErr = "6";
                //serializer.Serialize(fs, state);

                //indErr = "7";
                //fs.Close();
                using (FileStream fs = new FileStream(datapathPart, FileMode.CreateNew))
                {
                    indErr = "6";
                    serializer.Serialize(fs, state);

                    indErr = "7";
                    fs.Close();
                }
            }
            indErr = "8";
            state.FieldsXML = null;
            //Debug.Log("Saved Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
        }
        catch (Exception x)
        {
            Debug.Log("######### SaveGridXml: " + x.Message + "     to :" + datapath + "    #" + indErr);
        }
    }
    */

    static public void SaveGridCashXml(ModelNPC.GridData state, string datapath, bool isNewWorld = false)
    {
        string indErr = "start";
        try
        {
            Dictionary<string, ModelNPC.FieldData> dataSave = new Dictionary<string, ModelNPC.FieldData>(state.FieldsD);

            Dictionary<string, ModelNPC.FieldData> FieldsPart = new Dictionary<string, ModelNPC.FieldData>();
            int SizePart = Helper.WidthLevel / Helper.SizePart;

            Dictionary<string, Dictionary<string, ModelNPC.FieldData>> PartsGrids = new Dictionary<string, Dictionary<string, ModelNPC.FieldData>>();

            int partX = 0;
            int partY = 0;
            FieldsPart = new Dictionary<string, ModelNPC.FieldData>();
            string nameFileXML = +(partX + 1) + "x" + (partY + 1);
            int startX = partX * SizePart;
            int startY = partY * SizePart;
            int widthX = startX + SizePart;
            int widthY = startY + SizePart;

            Helper.StartControlTime();

            for (int x = startX; x < widthX; x++)
            {
                for (int y = startY; y < widthY; y++)
                {
                    indErr = "d.1";
                    string fieldName = Helper.GetNameField(x, y);
                    indErr = "d.2";
                    //if (state.FieldsD.ContainsKey(fieldName))
                    if (dataSave.ContainsKey(fieldName))
                    {
                        //ModelNPC.FieldData copyFields = state.FieldsD[fieldName];
                        ModelNPC.FieldData copyFields = dataSave[fieldName];

                        FieldsPart.Add(fieldName, copyFields);
                        //state.FieldsD.Remove(fieldName);
                        dataSave.Remove(fieldName);
                    }
                }
            }
            indErr = "d.3";

            Helper.StopControlTime("...time: SaveGridCashXml FieldsPart: ");

            //Dictionary<string, ModelNPC.FieldData> cashPart = PartsGrids["1x1"];
            Dictionary<string, ModelNPC.FieldData> cashPart = FieldsPart;
            Dictionary<string, ModelNPC.FieldData> bigPart = dataSave;

            Helper.StartControlTime();
            ModelNPC.GridData stateCash = new ModelNPC.GridData() { FieldsD = cashPart };
            SavePartXML(stateCash, "1x1");
            Helper.StopControlTime("...time: SaveGridCashXml Save Cash");

            Helper.StartControlTime();
            ModelNPC.GridData stateBig = new ModelNPC.GridData() { FieldsD = bigPart };
            SavePartXML(stateBig, "1x2");
            Helper.StopControlTime("...time: SaveGridCashXml Save Big");


            indErr = "8";
            state.FieldsXML = null;
            //Debug.Log("Saved Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
        }
        catch (Exception x)
        {
            Debug.Log("######### SaveGridXml: " + x.Message + "     to :" + datapath + "    #" + indErr);
        }
    }

    //public static SavePartXML(ModelNPC.GridData state, string nameFileXML = "")
    public static void SavePartXML(ModelNPC.GridData state, string nameFileXML = "")
    {
        string indErr = "1";
        //Dictionary<string, ModelNPC.FieldData> partGridFields = partGrid;
        indErr = "1.1.";
        string datapathPart = Application.dataPath + "/Levels/LevelDataPart" + nameFileXML + ".xml";
        if (File.Exists(datapathPart))
        {
            try
            {
                indErr = "delete";
                File.Delete(datapathPart);
            }
            catch (Exception x)
            {
                Debug.Log("############# Error SavePartXML NOT File Delete: " + datapathPart + " : " + x.Message);
            }
        }

        indErr = "1.3.";

        state.FieldsXML = state.FieldsD.ToList();
        indErr = "2";
        //## 
        Debug.Log("SaveXml GridData D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapathPart);

        indErr = "3";
        //XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);
        XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);


        System.Text.UTF8Encoding encodingT = new System.Text.UTF8Encoding();
        indErr = "5";
        using (FileStream fs = new FileStream(datapathPart, FileMode.CreateNew))
        {
            indErr = "6";

            //-----------
            //var memoryStream = new MemoryStream();
            //var streamWriter = new StreamWriter(memoryStream, System.Text.Encoding.UTF8);
            //serializer.Serialize(streamWriter, state);
            //serializer.Serialize(fs, streamWriter);
            ////----------------
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    //XmlSerializer s = new XmlSerializer(typeof(T));
            //    //Console.WriteLine("Testing for type: {0}", typeof(T));
            //    serializer.Serialize(System.Xml.XmlWriter.Create(stream), state);
            //    stream.Flush();
            //    stream.Seek(0, SeekOrigin.Begin);
            //    object o = s.Deserialize(XmlReader.Create(stream));
            //    Console.WriteLine("  Deserialized type: {0}", o.GetType());
            //}
            //--------------------

            //
            //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            //inputString = encoding.GetString(inputString);
            //serializer.Serialize(fs, state, encodingStyle: "UTF8Encoding");
            //var encoding = Encoding.GetEncoding("ISO-8859-1");
            //using (StreamWriter sw = new StreamWriter(fname, appendMode, encoding))

            //----------------
            serializer.Serialize(fs, state);
            //----------------

            indErr = "7";
            fs.Close();
        }

        //FileStream fs = new FileStream(datapathPart, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        //StreamWriter swFromFileTrueUTF8 = new StreamWriter(datapathPart, true, System.Text.Encoding.UTF8);
        //swFromFileTrueUTF8.Write(textToAdd);
        //swFromFileTrueUTF8.Flush();
        //swFromFileTrueUTF8.Close();
    }

    /*
    static public ModelNPC.GridData LoadGridPartsXml()
    {
        string datapath;

        string stepErr = "start";

        ModelNPC.GridData result = new ModelNPC.GridData();
        try
        {
            Debug.Log("Loaded Xml GridData start...");
            for (int partX = 0; partX < 3; partX++)
            {
                for (int partY = 0; partY < 3; partY++)
                {
                    stepErr = "c.1";
                    string nameFileXML = +(partX + 1) + "x" + (partY + 1);
                    string datapathPart = Application.dataPath + "/Levels/LevelDataPart" + nameFileXML + ".xml";
                    datapath = datapathPart;
                    if (File.Exists(datapathPart))
                    {
                        try
                        {
                            ModelNPC.GridData itemGridData = null;
                            stepErr = ".1";
                            stepErr = ".2";
                            XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);
                            stepErr = ".3";
                            using (FileStream fs = new FileStream(datapathPart, FileMode.Open))
                            {
                                stepErr = ".4";
                                itemGridData = (ModelNPC.GridData)serializer.Deserialize(fs);
                                stepErr = ".5";
                                fs.Close();
                            }
                            stepErr = ".6";
                            itemGridData.FieldsD = itemGridData.FieldsXML.ToDictionary(x => x.Key, x => x.Value);
                            stepErr = ".7";
                            Debug.Log("Loaded Xml GridData D:" + itemGridData.FieldsD.Count() + "   XML:" + result.FieldsXML.Count() + "     datapath=" + datapathPart);
                            //## 
                            itemGridData.FieldsXML = null;
                            stepErr = ".8";
                            result.FieldsD = result.FieldsD.Concat(itemGridData.FieldsD)
                                .ToDictionary(x => x.Key, x => x.Value);

                        }
                        catch (Exception x)
                        {
                            Debug.Log("############ #" + stepErr + " LoadGridPartsXml : " + datapathPart + " : " + x.Message);
                        }
                    }

                }
            }


        }
        catch (Exception x)
        {
            result = null;
            Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        }

        return result;
    }
    */

    static public ModelNPC.GridData LoadGridXml_1(string datapath)
    {
        //--Load parts
        //return LoadGridPartsXml();
        //return null;
        //-------------------

        string stepErr = "start";
        ModelNPC.GridData state = null;
        try
        {
            Debug.Log("Loaded Xml GridData start...");

            stepErr = ".1";
            //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
            stepErr = ".2";
            XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);
            stepErr = ".3";
            FileStream fs = new FileStream(datapath, FileMode.Open);
            stepErr = ".4";
            state = (ModelNPC.GridData)serializer.Deserialize(fs);
            stepErr = ".5";
            fs.Close();

            stepErr = ".6";
            state.FieldsD = state.FieldsXML.ToDictionary(x => x.Key, x => x.Value);
            stepErr = ".7";
            Debug.Log("Loaded Xml GridData D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
            //## 
            state.FieldsXML = null;
        }
        catch (Exception x)
        {
            Storage.EventsUI.SetTittle = "Error LoadGridXml : " + datapath;
            Storage.EventsUI.ListLogAdd = "########### Error LoadGridXml";
            Storage.EventsUI.ListLogAdd = "########### Error LoadGridXml : " + datapath;
            Storage.EventsUI.ListLogAdd = "########### Error LoadGridXml";
            Storage.EventsUI.ListLogAdd = "########### Error LoadGridXml stepErr: " + stepErr;
            Storage.EventsUI.ListLogAdd = "Error LoadGridXml : " + x.Message; 

            state = null;
            Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        }

        if(state == null)
        {
            Storage.EventsUI.SetTittle = "LoadGridXml data is empty: " + datapath;
            Storage.EventsUI.ListLogAdd = "LoadGridXml data is empty";
            Storage.EventsUI.ListLogAdd = "LoadGridXml data is empty: " + datapath;
            Storage.EventsUI.ListLogAdd = "LoadGridXml data is empty";
        }

        return state;
    }

    static public ModelNPC.GridData LoadGridXml(string datapath)
    {
        //--Load parts
        //return LoadGridPartsXml();
        //return null;
        //-------------------

        string stepErr = "start";
        ModelNPC.GridData state = null;
        try
        {
            Debug.Log("Loaded Xml GridData start...");

            var encoding = Encoding.GetEncoding("UTF-8");
            XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);
            using (StreamReader reader = new StreamReader(datapath, Encoding.UTF8, true))
            {
                state = (ModelNPC.GridData)serializer.Deserialize(reader);
            }

            stepErr = ".6";
            state.FieldsD = state.FieldsXML.ToDictionary(x => x.Key, x => x.Value);
            stepErr = ".7";
            Debug.Log("Loaded Xml GridData D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
            //## 
            state.FieldsXML = null;
        }
        catch (Exception x)
        {
            Storage.EventsUI.SetTittle = "Error LoadGridXml : " + datapath;
            Storage.EventsUI.ListLogAdd = "########### Error LoadGridXml";
            Storage.EventsUI.ListLogAdd = "########### Error LoadGridXml : " + datapath;
            Storage.EventsUI.ListLogAdd = "########### Error LoadGridXml stepErr: " + stepErr;
            Storage.EventsUI.ListLogAdd = "Error LoadGridXml : " + x.Message;

            state = null;
            Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        }

        if (state == null)
        {
            Storage.EventsUI.SetTittle = "LoadGridXml data is empty: " + datapath;
            Storage.EventsUI.ListLogAdd = "LoadGridXml data is empty";
            Storage.EventsUI.ListLogAdd = "LoadGridXml data is empty: " + datapath;
            Storage.EventsUI.ListLogAdd = "LoadGridXml data is empty";
        }

        return state;
    }

    static public ModelNPC.LevelData LoadPersonXml(string datapath)
    {
        string stepErr = "start";
        ModelNPC.LevelData state = null;
        try
        {
            Debug.Log("Loaded Xml GridData start...");

            stepErr = ".1";
            
            //XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.LevelData), extraTypes);
            //stepErr = ".3";
            //FileStream fs = new FileStream(datapath, FileMode.Open);
            //stepErr = ".4";
            //state = (ModelNPC.LevelData)serializer.Deserialize(fs);
            //stepErr = ".5";
            //fs.Close();
            var encoding = Encoding.GetEncoding("UTF-8");
            stepErr = ".2";
            XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.LevelData), extraTypes);
            stepErr = ".3";
            using (StreamReader reader = new StreamReader(datapath, Encoding.UTF8, true))
            {
                stepErr = ".4";
                state = (ModelNPC.LevelData)serializer.Deserialize(reader);
            }
            stepErr = ".6";
            state.Persons = state.PersonsXML.ToDictionary(x => x.Key, x => x.Value);
            stepErr = ".7";
            Debug.Log("Loaded Xml CasePersonData :" + state.Persons.Count() + "   XML:" + state.PersonsXML.Count() + "     datapath=" + datapath);
            state.PersonsXML = null;
        }
        catch (Exception x)
        {
            state = null;
            Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        }

        return state;
    }

    static public TilesData LoadTilesXml(string datapath)
    {
        string stepErr = "start";
        TilesData state = null;
        try
        {
            Debug.Log("Loaded Xml GridData start...");

            stepErr = ".1";
            stepErr = ".2";
            //XmlSerializer serializer = new XmlSerializer(typeof(TilesData), extraTypes);
            //stepErr = ".3";
            //FileStream fs = new FileStream(datapath, FileMode.Open);
            //stepErr = ".4";

            //state = (TilesData)serializer.Deserialize(fs);
            //stepErr = ".5";
            //fs.Close();
            var encoding = Encoding.GetEncoding("UTF-8");
            stepErr = ".3";
            XmlSerializer serializer = new XmlSerializer(typeof(TilesData), extraTypes);
            stepErr = ".4";
            using (StreamReader reader = new StreamReader(datapath, Encoding.UTF8, true))
            {
                stepErr = ".5";
                state = (TilesData)serializer.Deserialize(reader);
            }
            stepErr = ".6";
            state.TilesD = state.TilesXML.ToDictionary(x => x.Key, x => x.Value);
            stepErr = ".7";
            Debug.Log("Loaded Xml CasePersonData :" + state.TilesD.Count() + "     datapath=" + datapath);
            state.TilesXML = null;
        }
        catch (Exception x)
        {
            state = null;
            Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        }

        return state;
    }

    static public void SaveTilesDataXml(TilesData state, string datapath, bool isNewWorld = false)
    {

        if (isNewWorld)
        {
            if (File.Exists(datapath))
            {
                try
                {
                    File.Delete(datapath);
                }
                catch (Exception x)
                {
                    Debug.Log("############# Error TilesData NOT File Delete: " + datapath + " : " + x.Message);
                }
            }
        }

        //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
        //## 
        state.TilesXML = state.TilesD.ToList();

        //## 
        Debug.Log("SaveXml GridData D:" + state.TilesD.Count() + "   XML:" + state.TilesXML.Count() + "     datapath=" + datapath);

        XmlSerializer serializer = new XmlSerializer(typeof(TilesData), extraTypes);

        FileStream fs = new FileStream(datapath, FileMode.Create);

        serializer.Serialize(fs, state);
        fs.Close();

        state.TilesXML = null;
        //Debug.Log("Saved Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
    }


    static public T LoadXml<T>(string datapath, Type[] p_extraTypes = null) where T : class
    {
        string stepErr = "start";
        T state = null;
        try
        {
            //Debug.Log("Loaded Xml GridData start...");

            if (p_extraTypes == null)
                p_extraTypes = extraTypes;

            stepErr = ".1";
            stepErr = ".2";
            XmlSerializer serializer = new XmlSerializer(typeof(T), p_extraTypes);
            stepErr = ".3";
            FileStream fs = new FileStream(datapath, FileMode.Open);
            stepErr = ".4";
            state = (T)serializer.Deserialize(fs);
            stepErr = ".5";
            fs.Close();
            stepErr = ".6";
            stepErr = ".7";
        }
        catch (Exception x)
        {
            state = null;
            Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        }

        return state;
    }

    static public void SaveXml<T>(T state, string datapath, bool isResave = false, System.Type[] p_extraTypes = null) where T : class
    {

        if (isResave)
        {
            if (File.Exists(datapath))
            {
                try
                {
                    File.Delete(datapath);
                }
                catch (Exception x)
                {
                    Debug.Log("############# Error SaveGridXml NOT File Delete: " + datapath + " : " + x.Message);
                }
            }
        }

        if (p_extraTypes == null)
            p_extraTypes = extraTypes;

        XmlSerializer serializer = new XmlSerializer(typeof(T), p_extraTypes);

        FileStream fs = new FileStream(datapath, FileMode.Create);

        serializer.Serialize(fs, state);
        fs.Close();

    }
}

#endregion
