﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEmp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //public void GenGrigLook(Vector2 _movement, int p_PosHeroX = 0, int p_limitHorizontalLook = 0, int p_PosHeroY = 0, int p_limitVerticalLook = 0)
    //{
    //    int gridWidth = 100;
    //    int gridHeight = 100;
    //    //gridWidth = (int)GridX;
    //    //gridHeight = (int)GridY;

    //    int maxVertical = (int)p_limitVerticalLook + 1;// *-1;
    //    int maxHorizontal = (int)p_limitHorizontalLook + 1;

    //    if (Fields.Count != _counter || _counter == 0)
    //    {
    //        //DebugLogT("--------- Cancel GenGrigLook");
    //        return;
    //    }
    //    else
    //    {
    //        //Debug.Log("Start GenGrigLook count: " + Fields.Count);
    //    }

    //    if (_movement.x != 0)
    //    {
    //        int p_startPosY = p_PosHeroY - (p_limitVerticalLook / 2);
    //        //Validate
    //        if (p_startPosY < 0)
    //        {
    //            //Debug.Log("GenGrigLook --!!!--- not validate startPosY=" + p_startPosY + 
    //            //    " p_PosHeroY=" + p_PosHeroY  +
    //            //    " p_limitVerticalLook=" + p_limitVerticalLook +
    //            //    "        startPosY = p_PosHeroY + (p_limitVerticalLook / 2);");
    //            p_startPosY = 0;
    //        }
    //        int limitVertical = p_startPosY + maxVertical;
    //        if (limitVertical > gridHeight)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate limitVertical=" + limitVertical);
    //            limitVertical = gridHeight;
    //        }

    //        bool isRemove = true;
    //        bool isAdded = true;
    //        int x = 0;
    //        int LeftX = p_PosHeroX - (p_limitHorizontalLook / 2);
    //        int RightX = p_PosHeroX + (p_limitHorizontalLook / 2);
    //        int LeftRemoveX = LeftX - 1;
    //        int RightRemoveX = RightX + 1;
    //        //Validate
    //        if (LeftRemoveX < 0)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate LeftRemoveX=" + LeftRemoveX);
    //            if (_movement.x > 0)
    //                isRemove = false;
    //        }
    //        if (LeftX < 0)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate LeftX=" + LeftX);
    //            LeftX = 0;
    //            if (_movement.x < 0)
    //                isAdded = false;
    //        }
    //        if (RightRemoveX > gridWidth)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate RightRemoveX=" + RightRemoveX);
    //            if (_movement.x < 0)
    //                isRemove = false;
    //        }
    //        if (RightX > gridWidth)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate RightX=" + RightX);
    //            RightX = gridWidth;
    //            if (_movement.x > 0)
    //                isAdded = false;
    //        }

    //        //DebugLogT("GenGrigLook movement X");

    //        //Debug.Log("GenGrigLook >> Remove Vertical  p_startPosY=" + p_startPosY + "  >>>>>  limitVertical=" + limitVertical);
    //        //Debug.Log("p_startPosY = p_PosHeroY - (p_limitVerticalLook / 2)" +
    //        //    "  p_startPosY =" + p_startPosY +
    //        //    "  p_PosHeroY =" + p_PosHeroY +
    //        //    "  p_limitVerticalLook =" + p_limitVerticalLook);
    //        //Debug.Log("limitVertical = p_startPosY + maxVertical" +
    //        //    "  limitVertical =" + limitVertical +
    //        //    "  p_startPosY=" + p_startPosY +
    //        //    "  maxVertical=" + maxVertical);

    //        if (!isRemove)
    //        {
    //            //Debug.Log("GenGrigLook Not Remove Vertical ");
    //        }
    //        else
    //        {
    //            //x = _movement.x > 0 ?
    //            //    //Remove Vertical
    //            //LeftX :
    //            //RightX;
    //            x = _movement.x > 0 ?
    //                //Remove Vertical
    //            LeftRemoveX :
    //            RightRemoveX;

    //            string _nameFiled = "";
    //            for (int y = p_startPosY; y < limitVertical; y++)
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                _nameFiled = nameFiled;
    //                //Debug.Log("GenGrigLook Remove Vertical --- " + nameFiled);
    //                //Find
    //                if (!Fields.ContainsKey(nameFiled))
    //                {
    //                    //DebugLogT("GenGrigLook Remove Vertical not Field : " + nameFiled);
    //                    continue;
    //                }
    //                GameObject findFiled = Fields[nameFiled];

    //                //Destroy !!!
    //                // Kills the game object in 5 seconds after loading the object
    //                //Debug.Log("GenGrigLook Destroy");
    //                Destroy(findFiled, 0.5f);
    //                //Debug.Log("GenGrigLook Fields.Remove");
    //                Fields.Remove(nameFiled);
    //                _counter--;
    //                //Debug.Log("GenGrigLook Remove Vertical +++ " + nameFiled);
    //            }
    //            //DebugLogT("GenGrigLook Remove Vertical +++ " + _nameFiled);
    //        }

    //        if (!isAdded)
    //        {
    //            //DebugLogT("GenGrigLook Not Added Vertical ");
    //        }
    //        else
    //        {
    //            x = _movement.x > 0 ?
    //                //Added Vertical
    //                RightX :
    //                LeftX;

    //            string _nameFiled = "";
    //            for (int y = p_startPosY; y < limitVertical; y++)
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                _nameFiled = nameFiled;
    //                //Debug.Log("GenGrigLook Added Vertical --- " + nameFiled);

    //                if (Fields.ContainsKey(nameFiled))
    //                {
    //                    // Debug.Log("GenGrigLook Added Vertical YES Field ))) : " + nameFiled);
    //                    continue;
    //                }
    //                Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
    //                pos.z = 0;
    //                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
    //                //DebugLogT("Field Added  name init : " + nameFiled);
    //                newFiled.name = nameFiled;
    //                Fields.Add(nameFiled, newFiled);
    //                _counter++;
    //                //Debug.Log("GenGrigLook Added Vertical +++ " + nameFiled);
    //            }
    //            //DebugLogT("GenGrigLook Added Vertical +++ " + _nameFiled);
    //        }
    //    }

    //    if (_movement.y != 0)
    //    {
    //        //DebugLogT("GenGrigLook movement Y");

    //        int p_startPosX = p_PosHeroX - (p_limitHorizontalLook / 2); //#
    //        //Validate
    //        if (p_startPosX < 0)
    //        {
    //            //Debug.Log("GenGrigLook --!!!--- not validate startPosX=" + p_startPosX);
    //            p_startPosX = 0;
    //        }

    //        int limitHorizontal = p_startPosX + maxHorizontal;
    //        if (limitHorizontal > gridWidth)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate Horizontal=" + limitHorizontal);
    //            limitHorizontal = gridWidth;
    //        }

    //        bool isRemove = true;
    //        bool isAdded = true;
    //        int y = 0;
    //        //int TopY = p_PosHeroY - (p_limitVerticalLook / 2); //#
    //        //int DownY = p_PosHeroY + (p_limitVerticalLook / 2); //#
    //        int TopY = p_PosHeroY - (p_limitVerticalLook / 2); //#
    //        int DownY = p_PosHeroY + (p_limitVerticalLook / 2); //#
    //        int TopRemoveY = TopY - 1;
    //        int DownRemoveY = DownY + 1;
    //        //Debug.Log("GenGrigLook PosHeroY=" + p_PosHeroY);
    //        //Debug.Log("GenGrigLook TopY=" + TopY);
    //        //Debug.Log("GenGrigLook DownY=" + DownY);

    //        //Validate
    //        if (TopRemoveY < 0)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate TopRemoveY=" + TopRemoveY);
    //            if (_movement.y < 0)
    //                isRemove = false;
    //        }
    //        if (TopY < 0)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate TopY=" + TopY);
    //            TopY = 0;
    //            if (_movement.y > 0)
    //                isAdded = false;
    //        }
    //        if (DownRemoveY > gridHeight)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate DownRemoveY=" + DownRemoveY);
    //            if (_movement.y > 0)
    //                isRemove = false;
    //        }
    //        if (DownY > gridHeight)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate DownY=" + DownY);
    //            DownY = gridHeight;
    //            if (_movement.y < 0)
    //                isAdded = false;
    //        }

    //        if (!isRemove)
    //        {
    //            //DebugLogT("GenGrigLook Not Remove Horizontal ");
    //        }
    //        else
    //        {
    //            //    //Remove Horizontal //#
    //            //    TopY :
    //            //    DownY; //#
    //            y = _movement.y < 0 ?
    //                //Remove Horizontal //#
    //                TopRemoveY :
    //                DownRemoveY; //#

    //            for (int x = p_startPosX; x < limitHorizontal; x++) //#
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                //Debug.Log("GenGrigLook Remove Horizontal --- " + nameFiled);
    //                //Find
    //                if (!Fields.ContainsKey(nameFiled))
    //                {
    //                    //DebugLogT("GenGrigLook Remove Vertical not Field : " + nameFiled + "   Fields cpont: " + Fields.Count);
    //                    continue;
    //                }
    //                GameObject findFiled = Fields[nameFiled];

    //                //Destroy !!!
    //                // Kills the game object in 5 seconds after loading the object
    //                //Debug.Log("GenGrigLook Destroy");
    //                Destroy(findFiled, 0.5f);
    //                //Debug.Log("GenGrigLook Fields.Remove");
    //                Fields.Remove(nameFiled);
    //                _counter--;
    //                //DebugLogT("GenGrigLook Removed Horizontal +++ " + nameFiled);
    //            }
    //        }

    //        if (!isAdded)
    //        {
    //            //DebugLogT("GenGrigLook Not Added Horizontal ");
    //        }
    //        else
    //        {
    //            //y = _movement.y > 0 ?
    //            y = _movement.y < 0 ?
    //                //Added Horizontal
    //                DownY :
    //                TopY; //#
    //            for (int x = p_startPosX; x < limitHorizontal; x++) //#
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                //Debug.Log("GenGrigLook Added Horizontal --- " + nameFiled);

    //                if (Fields.ContainsKey(nameFiled))
    //                {
    //                    // DebugLogT("GenGrigLook Added Vertical YES Field ))) : " + nameFiled + "   Fields cpont: " + Fields.Count);
    //                    continue;
    //                }
    //                Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
    //                pos.z = 0;
    //                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
    //                //Debug.Log("Field Added name init : " + nameFiled);
    //                newFiled.name = nameFiled;
    //                Fields.Add(nameFiled, newFiled);
    //                _counter++;
    //                //DebugLogT("Field Added name +++ " + nameFiled);
    //            }
    //        }
    //    }
    //}

    //-------------------------

    //public void CreateFields_()
    //{
    //    Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Field==" + sizeSpriteRendererField);


    //    float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
    //    float heightFiled = sizeSpriteRendererField.y;


    //    Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Panel==" + sizeSpriteRendererField);

    //    //Debug.Log("Panel. Scale.x =" + prefabPanel.transform.localScale.x.ToString());
    //    //Debug.Log("Panel. Scale.y =" + prefabPanel.transform.localScale.y.ToString());
    //    var scaleX = prefabPanel.transform.localScale.x;
    //    var scaleY = prefabPanel.transform.localScale.y;


    //    //float widthPanel = sizeSpriteRendererprefabPanel.x; // .Size.Width;
    //    //float heightPanel = sizeSpriteRendererprefabPanel.y;
    //    float widthPanel = sizeSpriteRendererprefabPanel.x * scaleX;
    //    float heightPanel = sizeSpriteRendererprefabPanel.y * scaleY;

    //    int widthLenght = (int)(widthPanel / widthFiled);
    //    int heightLenght = (int)(heightPanel / heightFiled);

    //    int maxLengthOfArray = widthLenght * heightLenght;
    //    Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
    //    int counter = 0;

    //    Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
    //    Debug.Log("panelLocation =" + panelLocation.ToString());

    //    //prefabPanel.Visible = false;
    //    //Debug.Log("visible=" + prefabPanel.GetComponent<Renderer>().enabled.ToString());
    //    prefabPanel.GetComponent<Renderer>().enabled = false;
    //    //Debug.Log("visible=" + prefabPanel.GetComponent<Renderer>().enabled.ToString());

    //    //#TEST
    //    //widthFiled = 35;
    //    //heightFiled = 35;

    //    widthFiled = 1f;
    //    heightFiled = 1f;
    //    //widthFiled = 0.5f;
    //    //heightFiled = 0.5f;


    //    //Vector2 offset = new Vector2();
    //    //Vector2 offset = new Vector2(prefabField.transform.position.x, prefabField.transform.position.y);
    //    //Vector2 offset = new Vector2(prefabField.transform.position.x, prefabField.transform.position.y);
    //    //float offsetX = prefabField.transform.position.x;
    //    //float offsetY = prefabField.transform.position.y;
    //    float offsetX = 1;
    //    float offsetY = 1;
    //    //Debug.Log("offset 1=" + offset.ToString());
    //    for (int heig = 0; heig < widthLenght; heig++)
    //    {
    //        for (int wid = 0; wid < heightLenght; wid++)
    //        {
    //            counter++;
    //            //Vector2 newPos = new Vector2();
    //            //newPos.x = panelLocation.x + offset.x;
    //            //newPos.y = panelLocation.y + offset.y;
    //            //Debug.Log("offset 2..=" + offset.ToString());
    //            //newPos.x = offset.x;
    //            //newPos.y = offset.y;
    //            Vector2 newPos = new Vector2(offsetX, offsetY);
    //            //newPos = new Vector2(offsetX + prefabField.transform.position.x, offsetY + prefabField.transform.position.y);


    //            //Debug.Log("Instantiate(prefabField) " + counter.ToString());
    //            Debug.Log("Inst Field => " + counter.ToString());

    //            //GameObject newFiled = (GameObject)Instantiate(prefabField);
    //            //GameObject newFiled = (GameObject)Instantiate(prefabField, newPos * Time.deltaTime);
    //            //GameObject newFiled = (GameObject)Instantiate(prefabField, prefabPanel.transform);
    //            GameObject newFiled = (GameObject)Instantiate(prefabField);
    //            //GameObject newFiled = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //            //SpriteRenderer sr = newFiled.AddComponent<SpriteRenderer>();
    //            //sr = prefabField.GetComponent<SpriteRenderer>();

    //            //Debug.Log("newFiled.transform.position init");
    //            //newFiled.transform.position = newPos * Time.deltaTime;
    //            //newFiled.transform.position = new Vector2(prefabField.transform.position.x + offset.x * Time.deltaTime, prefabField.transform.position.y + offset.y * Time.deltaTime);
    //            //newFiled.transform.position = new Vector2(offset.x * Time.deltaTime, offset.y * Time.deltaTime);
    //            //newFiled.transform.position = newPos *Time.deltaTime;
    //            newFiled.transform.position = new Vector2(offsetX, offsetY);
    //            //newFiled.transform.position = newPos;
    //            //newFiled.transform.position = new Vector2(offsetX * Time.deltaTime, offsetY * Time.deltaTime);
    //            //newFiled.transform.position = new Vector2(prefabField.transform.position.x + offset.x, prefabField.transform.position.y + offset.y);
    //            Debug.Log("newFiled.transform.position =" + newFiled.transform.position.ToString());

    //            //Fields.Add(counter, newFiled);
    //            //offset.y += widthFiled;
    //            //offset.x += widthFiled;
    //            offsetX += widthFiled;

    //        }
    //        //Debug.Log("");
    //        //offset.x = 0;
    //        //offset.y -= heightFiled;
    //        offsetX = 0;
    //        offsetY -= heightFiled;
    //    }
    //    //for (int y = 0; y < 15; y++)
    //    //{
    //    //    for (int x = 0; x < 15; x++)
    //    //    {
    //    //        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    //        //cube.AddComponent<Rigidbody>();
    //    //        cube.transform.position = new Vector3(x, y, 0);
    //    //    }
    //    //}
    //    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    // cube.transform.position = new Vector3(1, 1);
    //    // GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    // cube2.transform.position = new Vector3(1, 0);
    //    // GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    // cube3.transform.position = new Vector3(2, 1);
    //}

    //--------------

    //public Rigidbody rocket;
    //public float speed = 10f;

    //void FireRocket()
    //{
    //    Rigidbody rocketClone = (Rigidbody)Instantiate(rocket, transform.position, transform.rotation);
    //    rocketClone.velocity = transform.forward * speed;

    //    // You can also access other components / scripts of the clone
    //    //rocketClone.GetComponent<MyRocketScript>().DoSomething();
    //}

    //// Calls the fire method when holding down ctrl or mouse
    //void Update()
    //{
    //    if (Input.GetButtonDown("Fire1"))
    //    {
    //        FireRocket();
    //    }
    //}

    //------------------

    //IEnumerator CreateFieldsAsync_()
    //{
    //    if (prefabField == null)
    //    {
    //        Debug.Log("prefabField == null");
    //        yield break;
    //    }
    //    if (prefabPanel == null)
    //    {
    //        Debug.Log("prefabPanel == null");
    //        yield break;
    //    }

    //    if (prefabField.transform == null)
    //    {
    //        Debug.Log("prefabField.transform == null");
    //        yield break;
    //    }

    //    //Debug.Log("SpriteRenderer init");
    //    Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Field==" + sizeSpriteRendererField);

    //    //GetComponent<Collider>().bounds.size
    //    //var t1 = prefabField.GetComponent<Renderer>().bounds.size;
    //    //Debug.Log("RectTransform prefabbFieldTransform complete");

    //    //float widthFiled = FieldTransform.rect.width; // .Size.Width;
    //    //float heightFiled = FieldTransform.rect.height;
    //    float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
    //    float heightFiled = sizeSpriteRendererField.y;


    //    //Debug.Log("RectTransform prefabbPanelTransform init");
    //    //RectTransform PanelTransform = (RectTransform)prefabPanel.transform;
    //    //float widthPanel = PanelTransform.rect.width; // .Size.Width;
    //    //float heightPanel = PanelTransform.rect.height;

    //    Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Panel==" + sizeSpriteRendererField);


    //    float widthPanel = sizeSpriteRendererprefabPanel.x; // .Size.Width;
    //    float heightPanel = sizeSpriteRendererprefabPanel.y;

    //    int widthLenght = (int)(widthPanel / widthFiled);
    //    int heightLenght = (int)(heightPanel / heightFiled);

    //    int maxLengthOfArray = widthLenght * heightLenght;
    //    Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
    //    int counter = 0;

    //    //Point panelLocation = prefabPanel.Location;
    //    //Vector2 panelLocation = PanelTransform.rect.position;
    //    //Vector2 panelLocation = PanelTransform.rect.position;
    //    //Debug.Log("prefabPanel.GetComponent<Renderer> init");
    //    Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
    //    Debug.Log("panelLocation =" + panelLocation.ToString());

    //    //prefabPanel.Visible = false;
    //    Debug.Log("visible=" + prefabPanel.GetComponent<Renderer>().enabled.ToString());
    //    prefabPanel.GetComponent<Renderer>().enabled = false;
    //    Debug.Log("visible=" + prefabPanel.GetComponent<Renderer>().enabled.ToString());

    //    //#TEST
    //    widthFiled = 50;
    //    heightFiled = 50;

    //    //var renderers = gameObject.GetComponentsInChildren.();
    //    //for (var r Renderer in renderers) {
    //    //    r.enabled = !r.enabled;
    //    //}
    //    //Vector2 offset = new Vector2(prefabField.transform.position.x, prefabField.transform.position.y);
    //    Vector2 offset = new Vector2();
    //    for (int heig = 0; heig < widthLenght; heig++)
    //    {
    //        for (int wid = 0; wid < heightLenght; wid++)
    //        {
    //            counter++;
    //            Vector2 newPos = new Vector2();
    //            //newPos.x = panelLocation.x + offset.x;
    //            //newPos.y = panelLocation.y + offset.y;
    //            newPos.x = offset.x;
    //            newPos.y = offset.y;

    //            Debug.Log("Instantiate(prefabField) " + counter.ToString());
    //            GameObject newFiled = (GameObject)Instantiate(prefabField);
    //            //Debug.Log("newFiled.transform.position init");
    //            newFiled.transform.position = newPos * Time.deltaTime;
    //            Debug.Log("newFiled.transform.position =" + newPos.ToString());

    //            //Fields.Add(counter, newFiled);
    //            //offset.y += widthFiled;
    //            offset.x -= widthFiled;
    //            yield return null;
    //        }
    //        //Debug.Log("");
    //        offset.x = 0;
    //        offset.y -= heightFiled;
    //    }

    //    ////foreach (GameObject b in cells)
    //    ////    Controls.Add(b.btn);
    //}

    //IEnumerator CreateFieldsAsync()
    //{
    //    Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Field==" + sizeSpriteRendererField);

    //    float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
    //    float heightFiled = sizeSpriteRendererField.y;

    //    Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Panel==" + sizeSpriteRendererField);

    //    var scaleX = prefabPanel.transform.localScale.x;
    //    var scaleY = prefabPanel.transform.localScale.y;

    //    float widthPanel = sizeSpriteRendererprefabPanel.x * scaleX;
    //    float heightPanel = sizeSpriteRendererprefabPanel.y * scaleY;

    //    int widthLenght = (int)(widthPanel / widthFiled);
    //    int heightLenght = (int)(heightPanel / heightFiled);

    //    int maxLengthOfArray = widthLenght * heightLenght;
    //    Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
    //    int counter = 0;

    //    Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
    //    Debug.Log("panelLocation =" + panelLocation.ToString());

    //    prefabPanel.GetComponent<Renderer>().enabled = false;

    //    widthFiled = 1f;
    //    heightFiled = 1f;

    //    float offsetX = 1;
    //    float offsetY = 1;
    //    for (int heig = 0; heig < widthLenght; heig++)
    //    {
    //        for (int wid = 0; wid < heightLenght; wid++)
    //        {
    //            counter++;
    //            Vector2 newPos = new Vector2(offsetX, offsetY);
    //            GameObject newFiled = (GameObject)Instantiate(prefabField);
    //            newFiled.transform.position = new Vector2(offsetX, offsetY);
    //            Debug.Log("newFiled.transform.position =" + newFiled.transform.position.ToString());
    //            offsetX += widthFiled;

    //            yield return null;
    //        }
    //        offsetX = 0;
    //        offsetY -= heightFiled;
    //    }

    //}

    //-------------------------------------

    //public IEnumerator GenGrigLookAsync(Vector2 _movement, int p_PosHeroX = 0, int p_limitHorizontalLook = 0, int p_PosHeroY = 0, int p_limitVerticalLook = 0)
    //{
    //    int gridWidth = 100;
    //    int gridHeight = 100;
    //    //gridWidth = (int)GridX;
    //    //gridHeight = (int)GridY;

    //    int maxVertical = (int)p_limitVerticalLook + 1;// *-1;
    //    int maxHorizontal = (int)p_limitHorizontalLook + 1;

    //    if (Fields.Count != _counter || _counter == 0)
    //        yield break;

    //    if (_movement.x != 0)
    //    {
    //        int p_startPosY = p_PosHeroY - (p_limitVerticalLook / 2);
    //        //Validate
    //        if (p_startPosY < 0)
    //            p_startPosY = 0;

    //        int limitVertical = p_startPosY + maxVertical;
    //        if (limitVertical > gridHeight)
    //            limitVertical = gridHeight;

    //        bool isRemove = true;
    //        bool isAdded = true;
    //        int x = 0;
    //        int LeftX = p_PosHeroX - (p_limitHorizontalLook / 2);
    //        int RightX = p_PosHeroX + (p_limitHorizontalLook / 2);
    //        int LeftRemoveX = LeftX - 1;
    //        int RightRemoveX = RightX + 1;
    //        //Validate
    //        if (LeftRemoveX < 0)
    //        {
    //            if (_movement.x > 0)
    //                isRemove = false;
    //        }
    //        if (LeftX < 0)
    //        {
    //            LeftX = 0;
    //            if (_movement.x < 0)
    //                isAdded = false;
    //        }
    //        if (RightRemoveX > gridWidth)
    //        {
    //            if (_movement.x < 0)
    //                isRemove = false;
    //        }
    //        if (RightX > gridWidth)
    //        {
    //            RightX = gridWidth;
    //            if (_movement.x > 0)
    //                isAdded = false;
    //        }

    //        if (isRemove)
    //        {
    //            x = _movement.x > 0 ?
    //                //Remove Vertical
    //            LeftRemoveX :
    //            RightRemoveX;

    //            string _nameFiled = "";
    //            for (int y = p_startPosY; y < limitVertical; y++)
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                _nameFiled = nameFiled;
    //                //Find
    //                if (!Fields.ContainsKey(nameFiled))
    //                    continue;

    //                GameObject findFiled = Fields[nameFiled];
    //                //Destroy !!!
    //                //Destroy(findFiled, 0.5f);
    //                Destroy(findFiled);
    //                Fields.Remove(nameFiled);
    //                _counter--;
    //                //yield return null;
    //            }
    //            yield return null;
    //        }

    //        if (isAdded)
    //        {
    //            x = _movement.x > 0 ?
    //                //Added Vertical
    //                RightX :
    //                LeftX;

    //            string _nameFiled = "";
    //            for (int y = p_startPosY; y < limitVertical; y++)
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                _nameFiled = nameFiled;

    //                if (Fields.ContainsKey(nameFiled))
    //                    continue;

    //                Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
    //                pos.z = 0;
    //                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
    //                newFiled.name = nameFiled;
    //                Fields.Add(nameFiled, newFiled);
    //                _counter++;
    //                //yield return null;
    //            }
    //            yield return null;
    //        }
    //    }

    //    if (_movement.y != 0)
    //    {
    //        int p_startPosX = p_PosHeroX - (p_limitHorizontalLook / 2); //#
    //        //Validate
    //        if (p_startPosX < 0)
    //            p_startPosX = 0;

    //        int limitHorizontal = p_startPosX + maxHorizontal;
    //        if (limitHorizontal > gridWidth)
    //            limitHorizontal = gridWidth;

    //        bool isRemove = true;
    //        bool isAdded = true;
    //        int y = 0;
    //        int TopY = p_PosHeroY - (p_limitVerticalLook / 2); //#
    //        int DownY = p_PosHeroY + (p_limitVerticalLook / 2); //#
    //        int TopRemoveY = TopY - 1;
    //        int DownRemoveY = DownY + 1;

    //        //Validate
    //        if (TopRemoveY < 0 && _movement.y < 0)
    //            isRemove = false;
    //        if (TopY < 0 && _movement.y > 0)
    //        {
    //            TopY = 0;
    //            //_movement.y > 0
    //            isAdded = false;
    //        }
    //        if (DownRemoveY > gridHeight && _movement.y > 0)
    //            isRemove = false;

    //        if (DownY > gridHeight && _movement.y < 0)
    //        {
    //            DownY = gridHeight;
    //            //if (_movement.y < 0)
    //            isAdded = false;
    //        }

    //        if (isRemove)
    //        {
    //            y = _movement.y < 0 ?
    //                //Remove Horizontal //#
    //                TopRemoveY :
    //                DownRemoveY; //#

    //            for (int x = p_startPosX; x < limitHorizontal; x++) //#
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                //Find
    //                if (!Fields.ContainsKey(nameFiled))
    //                    continue;

    //                GameObject findFiled = Fields[nameFiled];
    //                //Destroy !!!
    //                //Destroy(findFiled, 0.5f);
    //                Destroy(findFiled);
    //                Fields.Remove(nameFiled);
    //                _counter--;
    //                //yield return null;
    //            }
    //            yield return null;
    //        }

    //        if (isAdded)
    //        {
    //            y = _movement.y < 0 ?
    //                //Added Horizontal
    //                DownY :
    //                TopY; //#
    //            for (int x = p_startPosX; x < limitHorizontal; x++) //#
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);

    //                if (Fields.ContainsKey(nameFiled))
    //                    continue;

    //                Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
    //                pos.z = 0;
    //                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
    //                newFiled.name = nameFiled;
    //                Fields.Add(nameFiled, newFiled);
    //                _counter++;
    //                //yield return null;
    //            }
    //            yield return null;
    //        }
    //    }
    //}

    //--------------

    //public void GenGrigLook(Vector2 _movement, int p_PosHeroX = 0, int p_limitHorizontalLook = 0, int p_PosHeroY = 0, int p_limitVerticalLook = 0)
    //{
    //    int gridWidth = 100;
    //    int gridHeight = 100;
    //    //gridWidth = (int)GridX;
    //    //gridHeight = (int)GridY;

    //    int maxVertical = (int)p_limitVerticalLook + 1;// *-1;
    //    int maxHorizontal = (int)p_limitHorizontalLook + 1;

    //    if (Fields.Count != _counter || _counter == 0)
    //        return;

    //    if (_movement.x != 0)
    //    {
    //        int p_startPosY = p_PosHeroY - (p_limitVerticalLook / 2);
    //        //Validate
    //        if (p_startPosY < 0)
    //            p_startPosY = 0;

    //        int limitVertical = p_startPosY + maxVertical;
    //        if (limitVertical > gridHeight)
    //            limitVertical = gridHeight;

    //        bool isRemove = true;
    //        bool isAdded = true;
    //        int x = 0;
    //        int LeftX = p_PosHeroX - (p_limitHorizontalLook / 2);
    //        int RightX = p_PosHeroX + (p_limitHorizontalLook / 2);
    //        int LeftRemoveX = LeftX - 1;
    //        int RightRemoveX = RightX + 1;
    //        //Validate ValidateRemoveX
    //        if (LeftRemoveX < 0)
    //        {
    //            if (_movement.x > 0)
    //                isRemove = false;
    //        }
    //        if (RightRemoveX > gridWidth)
    //        {
    //            if (_movement.x < 0)
    //                isRemove = false;
    //        }

    //        if (RightX > gridWidth)
    //        {
    //            RightX = gridWidth;
    //            if (_movement.x > 0)
    //                isAdded = false;
    //        }
    //        if (LeftX < 0)
    //        {
    //            LeftX = 0;
    //            if (_movement.x < 0)
    //                isAdded = false;
    //        }



    //        if (isRemove)
    //        {
    //            x = _movement.x > 0 ?
    //                //Remove Vertical
    //            LeftRemoveX :
    //            RightRemoveX;

    //            string _nameFiled = "";
    //            for (int y = p_startPosY; y < limitVertical; y++)
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                _nameFiled = nameFiled;
    //                //Find
    //                if (!Fields.ContainsKey(nameFiled))
    //                    continue;

    //                GameObject findFiled = Fields[nameFiled];
    //                //Destroy !!!
    //                //Destroy(findFiled, 0.5f);
    //                Destroy(findFiled);
    //                Fields.Remove(nameFiled);
    //                _counter--;
    //            }
    //        }

    //        if (isAdded)
    //        {
    //            x = _movement.x > 0 ?
    //                //Added Vertical
    //                RightX :
    //                LeftX;

    //            string _nameFiled = "";
    //            for (int y = p_startPosY; y < limitVertical; y++)
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                _nameFiled = nameFiled;

    //                if (Fields.ContainsKey(nameFiled))
    //                    continue;

    //                Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
    //                pos.z = 0;
    //                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
    //                newFiled.name = nameFiled;
    //                Fields.Add(nameFiled, newFiled);
    //                _counter++;
    //            }
    //        }
    //    }

    //    if (_movement.y != 0)
    //    {
    //        int p_startPosX = p_PosHeroX - (p_limitHorizontalLook / 2); //#
    //        //Validate
    //        if (p_startPosX < 0)
    //            p_startPosX = 0;

    //        int limitHorizontal = p_startPosX + maxHorizontal;
    //        if (limitHorizontal > gridWidth)
    //            limitHorizontal = gridWidth;

    //        bool isRemove = true;
    //        bool isAdded = true;
    //        int y = 0;
    //        int TopY = p_PosHeroY - (p_limitVerticalLook / 2); //#
    //        int DownY = p_PosHeroY + (p_limitVerticalLook / 2); //#
    //        int TopRemoveY = TopY - 1;
    //        int DownRemoveY = DownY + 1;

    //        //Validate
    //        if (TopRemoveY < 0 && _movement.y < 0)
    //            isRemove = false;
    //        if (TopY < 0 && _movement.y > 0)
    //        {
    //            TopY = 0;
    //            //_movement.y > 0
    //            isAdded = false;
    //        }
    //        if (DownRemoveY > gridHeight && _movement.y > 0)
    //            isRemove = false;

    //        if (DownY > gridHeight && _movement.y < 0)
    //        {
    //            DownY = gridHeight;
    //            //if (_movement.y < 0)
    //            isAdded = false;
    //        }

    //        if (isRemove)
    //        {
    //            y = _movement.y < 0 ?
    //                //Remove Horizontal //#
    //                TopRemoveY :
    //                DownRemoveY; //#

    //            for (int x = p_startPosX; x < limitHorizontal; x++) //#
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                //Find
    //                if (!Fields.ContainsKey(nameFiled))
    //                    continue;

    //                GameObject findFiled = Fields[nameFiled];
    //                //Destroy !!!
    //                //Destroy(findFiled, 0.5f);
    //                Destroy(findFiled);
    //                Fields.Remove(nameFiled);
    //                _counter--;
    //            }
    //        }

    //        if (isAdded)
    //        {
    //            y = _movement.y < 0 ?
    //                //Added Horizontal
    //                DownY :
    //                TopY; //#
    //            for (int x = p_startPosX; x < limitHorizontal; x++) //#
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);

    //                if (Fields.ContainsKey(nameFiled))
    //                    continue;

    //                Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
    //                pos.z = 0;
    //                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
    //                newFiled.name = nameFiled;
    //                Fields.Add(nameFiled, newFiled);
    //                _counter++;
    //            }
    //        }
    //    }
    //}

    //---------------------

    //public void CreateFields()
    //{
    //    Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Field==" + sizeSpriteRendererField);

    //    float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
    //    float heightFiled = sizeSpriteRendererField.y;

    //    Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Panel==" + sizeSpriteRendererField);

    //    var scaleX = prefabPanel.transform.localScale.x;
    //    var scaleY = prefabPanel.transform.localScale.y;

    //    float widthPanel = sizeSpriteRendererprefabPanel.x * scaleX;
    //    float heightPanel = sizeSpriteRendererprefabPanel.y * scaleY;

    //    int widthLenght = (int)(widthPanel / widthFiled);
    //    int heightLenght = (int)(heightPanel / heightFiled);

    //    int maxLengthOfArray = widthLenght * heightLenght;
    //    Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
    //    int counter = 0;

    //    Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
    //    Debug.Log("panelLocation =" + panelLocation.ToString());

    //    prefabPanel.GetComponent<Renderer>().enabled = false;

    //    widthFiled = 1f;
    //    heightFiled = 1f;

    //    float offsetX = 1;
    //    float offsetY = 1;
    //    for (int heig = 0; heig < widthLenght; heig++)
    //    {
    //        for (int wid = 0; wid < heightLenght; wid++)
    //        {
    //            counter++;
    //            Vector2 newPos = new Vector2(offsetX, offsetY);
    //            GameObject newFiled = (GameObject)Instantiate(prefabField);
    //            newFiled.transform.position = new Vector2(offsetX, offsetY);
    //            Debug.Log("newFiled.transform.position =" + newFiled.transform.position.ToString());
    //            offsetX += widthFiled;

    //            //Fields.Add(counter, newFiled);
    //        }
    //        offsetX = 0;
    //        offsetY -= heightFiled;
    //    }
    //}
    //----------------------
    //#.D
    //private void RemoveRealObjectD(string p_nameFiled)
    //{
    //    if (!GamesObjectsReal.ContainsKey(p_nameFiled))
    //    {
    //        //Debug.Log("RemoveRealObject Not in field : " + p_nameFiled);
    //        return;
    //    }
    //    else
    //    {
    //        //---------------------------
    //        if (GridData != null)
    //        {
    //            Debug.Log("RemoveRealObjectD GridData -- EMPTY !!!!!!!");
    //            return;
    //        }
    //        if (GridData.Fields.Find(p => p.NameField == p_nameFiled) == null)
    //            return;

    //        int indexFieldData = GridData.Fields.FindIndex(p => p.NameField == p_nameFiled);
    //        if (indexFieldData == -1)
    //        {
    //            Debug.Log("RemoveRealObjectD Not field in Data: " + p_nameFiled);
    //            return;
    //        }
    //        List<SaveLoadData.ObjectData> dataObjects = GridData.Fields[indexFieldData].Objects;

    //        //int indexObjectData = listDataObjectInField.FindIndex(p => p.NameObject == p_saveObject.name);
    //        //SaveLoadData.ObjectData objDataOld = listDataObjectInField[indexObjectData];

    //        //var dataObjectSave = SaveLoadData.CreateObjectData(p_saveObject);
    //        //int indexNN = dataObjects.Count + 1;

    //        //if (objDataOld == null)
    //        //{
    //        //    dataObjectSave.NameObject += indexNN;
    //        //    dataObjects.Add(dataObjectSave);
    //        //}
    //        //else
    //        //{
    //        //    objDataOld = dataObjectSave;
    //        //}
    //        //-------------------------

    //        //List<GameObject> activeObjects = GamesObjectsActive[p_nameFiled];
    //        List<GameObject> realObjects = GamesObjectsReal[p_nameFiled];


    //        for (int i = activeObjects.Count - 1; i >= 0; i--)
    //        {


    //            activeObjects[i].SetActive(false); //#

    //            if (realObjects.Count <= i)
    //                continue;
    //            if (realObjects[i] == null)
    //                continue;



    //            var pos1 = activeObjects[i].transform.position;
    //            var pos2 = realObjects[i].transform.position;

    //            var f = pos1.y;
    //            string posFieldOld = GetNameFiled(pos1.x, pos1.y);
    //            string posFieldReal = GetNameFiled(pos2.x, pos2.y);

    //            //---------------------------------------------
    //            if (posFieldOld != posFieldReal)
    //            {
    //                Debug.Log("RemoveRealObject posFieldOld(" + posFieldOld + ") != posFieldReal(" + posFieldReal + ")      " + activeObjects[i].name + "    " + realObjects[i].name);

    //                activeObjects.RemoveAt(i);
    //                if (!GamesObjectsActive.ContainsKey(posFieldReal))
    //                {
    //                    Debug.Log("RemoveRealObject Not new posFieldReal =" + posFieldReal);
    //                }
    //                else
    //                {
    //                    //Add in new Filed
    //                    List<GameObject> activeObjectsNew = GamesObjectsActive[posFieldReal];

    //                    var realObj = realObjects[i];
    //                    var coyObj = CreatePrefabByName(realObj.tag, realObj.name, realObj.transform.position);

    //                    activeObjectsNew.Add(coyObj);

    //                }
    //            }
    //            else
    //            {
    //                //---------------------------------------------
    //                //Save Real value in memory
    //                activeObjects[i] = Instantiate(realObjects[i]); //#
    //                activeObjects[i].SetActive(false); //#
    //            }
    //        }

    //        SaveToDataObject(p_nameFiled);

    //        foreach (var obj in realObjects)
    //        {
    //            _counter--;
    //            Destroy(obj);
    //            //obj.SetActive(false);
    //        }
    //        GamesObjectsReal.Remove(p_nameFiled);

    //        //DebugLogT("RemoveRealObject objects in field ++++ " + p_nameFiled);


    //    }
    //}

    //---------------------

    //private void DeactivateGameObjectForLook(string p_nameFiled)
    //{
    //    //DebugLog("# DeactivateGameObjectForLook");

    //    if (!GamesObjectsActive.ContainsKey(p_nameFiled))
    //    {
    //        //DebugLog("DeactivateGameObjectForLook Not in field : " + p_nameFiled);
    //        return;
    //    }

    //    Debug.Log("# CreateGameObjectActiveForLook : " + p_nameFiled);

    //    List<GameObject> listGameObjectInField = GamesObjectsActive[p_nameFiled];
    //    foreach (var gameObj in listGameObjectInField)
    //    {
    //        //Destroy();// gameObj.SetActive(false);
    //        _counter--;
    //        DebugLog("# CreateGameObjectActiveForLook " + newFiled.name + " " + newFiled.tag + "  in  " + p_nameFiled);
    //    }
    //}

    //private void CreateGameObjectActive(string p_nameFiled)
    //{

    //}


    //-----------------------------

//NotSupportedException: The type System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] is not supported because it implements IDictionary.
//System.Xml.Serialization.TypeData.get_ListItemType ()
//System.Xml.Serialization.TypeData.get_ListItemTypeData ()
//System.Xml.Serialization.TypeData..ctor (System.Type type, System.String elementName, Boolean isPrimitive, System.Xml.Serialization.TypeData mappedType, System.Xml.Schema.XmlSchemaPatternFacet facet)
//System.Xml.Serialization.TypeData..ctor (System.Type type, System.String elementName, Boolean isPrimitive)
//System.Xml.Serialization.TypeTranslator.GetTypeData (System.Type runtimeType, System.String xmlDataType)
//System.Xml.Serialization.TypeTranslator.GetTypeData (System.Type type)
//System.Xml.Serialization.XmlReflectionImporter.CreateMapMember (System.Type declaringType, System.Xml.Serialization.XmlReflectionMember rmember, System.String defaultNamespace)
//System.Xml.Serialization.XmlReflectionImporter.ImportClassMapping (System.Xml.Serialization.TypeData typeData, System.Xml.Serialization.XmlRootAttribute root, System.String defaultNamespace)
//Rethrow as InvalidOperationException: There was an error reflecting field 'FieldsD'.
//System.Xml.Serialization.XmlReflectionImporter.ImportClassMapping (System.Xml.Serialization.TypeData typeData, System.Xml.Serialization.XmlRootAttribute root, System.String defaultNamespace)
//System.Xml.Serialization.XmlReflectionImporter.ImportTypeMapping (System.Xml.Serialization.TypeData typeData, System.Xml.Serialization.XmlRootAttribute root, System.String defaultNamespace)
//Rethrow as InvalidOperationException: There was an error reflecting type 'SaveLoadData+GridData'.
//System.Xml.Serialization.XmlReflectionImporter.ImportTypeMapping (System.Xml.Serialization.TypeData typeData, System.Xml.Serialization.XmlRootAttribute root, System.String defaultNamespace)
//System.Xml.Serialization.XmlReflectionImporter.ImportTypeMapping (System.Type type, System.Xml.Serialization.XmlRootAttribute root, System.String defaultNamespace)
//System.Xml.Serialization.XmlSerializer..ctor (System.Type type, System.Xml.Serialization.XmlAttributeOverrides overrides, System.Type[] extraTypes, System.Xml.Serialization.XmlRootAttribute root, System.String defaultNamespace)
//System.Xml.Serialization.XmlSerializer..ctor (System.Type type, System.Type[] extraTypes)
//SaveLoadData+Serializator.DeXml (System.String datapath) (at Assets/Scripts/SaveLoadData.cs:363)
//SaveLoadData.LoadPathData () (at Assets/Scripts/SaveLoadData.cs:67)
//SaveLoadData.Start () (at Assets/Scripts/SaveLoadData.cs:44)

    //---------------------------------

    ////public Dictionary(IDictionary<TKey, TValue> dictionary);
    //        //public Dictionary(IEqualityComparer<TKey> comparer);
    //        //public Dictionary(int capacity);
    //        //public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer);
    //        //public Dictionary(int capacity, IEqualityComparer<TKey> comparer);
    //        //protected Dictionary(SerializationInfo info, StreamingContext context);

    //        //public class Dictionary<TKey, TValue> : IEnumerable, ISerializable, ICollection, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, IDictionary, IDeserializationCallback
    //        //Dictionary<string, FieldData>
    //        IEnumerable<KeyValuePair<string, FieldData>> listXML = state.FieldsXML;
    //        state.FieldsD = new Dictionary<string, FieldData>();
    //        //IEnumerable<FieldData> source = listXML.Select(x => x.Value);
    //        //Func<FieldData, string> selector = (fd) => {
    //        //    FieldData data = fd;
    //        //    //data
    //        //    return "";
    //        //};
    //        //state.FieldsD = ToDict(state.FieldsXML).ToDictionary(x => x.Key, x => x.Value);
    //        state.FieldsD = state.FieldsXML.ToDictionary(x => x.Key, x => x.Value); 
    //        //-----------------------

    //            //Dictionary<TKey, TSource>
    //            //(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);
    //        //state.FieldsD = state.FieldsXML.ToDictionary<string, FieldData>(source, selector);
    //        //state.FieldsD = state.FieldsXML.ToDictionary<string, FieldData>(source, new Func<TSource, TKey>);

    //        //state.FieldsD = state.FieldsXML.ToDictionary<string, FieldData>(x => x.Key, y => y.Value);

    //        //public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>
    //        //            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);
    //        //public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>
    //        //          (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector);
    //        //public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>
    //        //              (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer);
    //        //public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>
    //        //          (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer);
            

    //        //Dictionary<TKey, TSource>
    //        //(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);

    //        IEnumerable<KeyValuePair<int, string>> listT = new List<KeyValuePair<int, string>>();
    //        Dictionary<int, string> d1 = new Dictionary<int, string>();
    //        //d1 = listT.ToDictionary<int, string>(x => x.Key, x => x.Value);
    //        //d1 = listT.ToDictionary<int, string>(listT);
    //        //d1 = listT.ToDictionary<int, string>(x => x.Value, x => x.Key);


    //        IEnumerable<string> source2 = listT.Select(x => x.Value);
    //        Func<string, string> selectorTest = str => str.ToUpper();
    //        Func<string, int> selector2 = str => str.Length;
    //        Func<int, string> selector3 = key => key.ToString();

    //        // Dictionary<TKey, TSource> 
    //        // ToDictionary<TSource, TKey>
    //        //(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);
    //        //d1 = listT.ToDictionary<int, string>(source2, selector2);
    //        //d1 = listT.ToDictionary<int, string>(source2, selector2);
    //        //d1 = listT.ToDictionary<int, string>(x => x.ToString(), x => x.Value.Length);
    //        //d1 = listT.ToDictionary<int, string>(x => x.Value.ToString(), c=>c.Key.ToString().Length);
    //        //d1 = listT.ToDictionary<int, string>(x => x.Value.ToString(), x => x.Key.ToString());
    //        //d1 = listT.ToDictionary<string, int>(x => x.Value.ToString(), x => x.Key.ToString());
    //        //var ress = listT.ToDictionary<string, int>(key => key.ToString().Length); //
    //        //var ress = listT.ToDictionary<string, int>(source2, selector2); //
            
    //        //var ress = listT.ToDictionary<string, int>(x => x.Key, ); //
            
    //        //d1 = listT.ToDictionary<int, string>(x => x.Value, x => x.Key);
    //        //d1 = listT.ToDictionary<int, string>(key => key.ToString());
    //        //var _d77 = listT.ToDictionary<int, string>(source2, selector2);
    //        //d1 = listT.GetComponents().ToDictionary(x => x.Key, x => x.Value);
    //        //d1 = listT.GetComponents().ToDictionary(x => x.Key, x => x.Value);
    //        //d1 = GetComponents2().ToDictionary(x => x.Key, x => x.Value);
    //        d1 = GetComponents3(listT).ToDictionary(x => x.Key, x => x.Value);
    //        //var tt= (IEnumerable<KeyValuePair<int, string>>)listT;
    //        //d1 = tt;


    //XmlSerializer serializer = new XmlSerializer(typeof(GridData), extraTypes);
    ////XmlSerializer serializer2 = new XmlSerializer(typeof(item[]),
    ////                     new XmlRootAttribute() { ElementName = "items" });

    ////List<KeyValuePair<K,V>>а затем (re) создайте ее в хэш-таблицу.

    //FileStream fs = new FileStream(datapath, FileMode.Create);

    ////serializer.Serialize(fs,
    ////  state.FieldsD.Select(kv => new item() { id = kv.Key, value = kv.Value }).ToArray());

    //----------------------------------
}