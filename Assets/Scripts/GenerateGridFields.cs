using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGridFields : MonoBehaviour {

    public GameObject prefabField;
    public GameObject prefabPanel;
    public Dictionary<int, GameObject> Fields;

	// Use this for initialization
	void Start () {
        Fields = new Dictionary<int, GameObject>();
        //StartGenCircle();
        StartGenGrig();
    }


    void Awake()
    {
        
        //CreateFields();
        //StartCoroutine(CreateFieldsAsync());
        //StartGenGrig();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator CreateFieldsAsync_()
    {
        if (prefabField == null)
        {
            Debug.Log("prefabField == null");
            yield break;
        }
        if (prefabPanel == null)
        {
            Debug.Log("prefabPanel == null");
            yield break;
        }

        if (prefabField.transform == null)
        {
            Debug.Log("prefabField.transform == null");
            yield break;
        }

        //Debug.Log("SpriteRenderer init");
        Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
        Debug.Log("size Field==" + sizeSpriteRendererField);

        //GetComponent<Collider>().bounds.size
        //var t1 = prefabField.GetComponent<Renderer>().bounds.size;
        //Debug.Log("RectTransform prefabbFieldTransform complete");

        //float widthFiled = FieldTransform.rect.width; // .Size.Width;
        //float heightFiled = FieldTransform.rect.height;
        float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
        float heightFiled = sizeSpriteRendererField.y;


        //Debug.Log("RectTransform prefabbPanelTransform init");
        //RectTransform PanelTransform = (RectTransform)prefabPanel.transform;
        //float widthPanel = PanelTransform.rect.width; // .Size.Width;
        //float heightPanel = PanelTransform.rect.height;

        Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
        Debug.Log("size Panel==" + sizeSpriteRendererField);


        float widthPanel = sizeSpriteRendererprefabPanel.x; // .Size.Width;
        float heightPanel = sizeSpriteRendererprefabPanel.y;

        int widthLenght = (int)(widthPanel / widthFiled);
        int heightLenght = (int)(heightPanel / heightFiled);

        int maxLengthOfArray = widthLenght * heightLenght;
        Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
        int counter = 0;

        //Point panelLocation = prefabPanel.Location;
        //Vector2 panelLocation = PanelTransform.rect.position;
        //Vector2 panelLocation = PanelTransform.rect.position;
        //Debug.Log("prefabPanel.GetComponent<Renderer> init");
        Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
        Debug.Log("panelLocation =" + panelLocation.ToString());

        //prefabPanel.Visible = false;
        Debug.Log("visible=" + prefabPanel.GetComponent<Renderer>().enabled.ToString());
        prefabPanel.GetComponent<Renderer>().enabled = false;
        Debug.Log("visible=" + prefabPanel.GetComponent<Renderer>().enabled.ToString());

        //#TEST
        widthFiled = 50;
        heightFiled = 50;

        //var renderers = gameObject.GetComponentsInChildren.();
        //for (var r Renderer in renderers) {
        //    r.enabled = !r.enabled;
        //}
        //Vector2 offset = new Vector2(prefabField.transform.position.x, prefabField.transform.position.y);
        Vector2 offset = new Vector2();
        for (int heig = 0; heig < widthLenght; heig++)
        {
            for (int wid = 0; wid < heightLenght; wid++)
            {
                counter++;
                Vector2 newPos = new Vector2();
                //newPos.x = panelLocation.x + offset.x;
                //newPos.y = panelLocation.y + offset.y;
                newPos.x = offset.x;
                newPos.y = offset.y;

                Debug.Log("Instantiate(prefabField) " + counter.ToString());
                GameObject newFiled = (GameObject)Instantiate(prefabField);
                //Debug.Log("newFiled.transform.position init");
                newFiled.transform.position = newPos * Time.deltaTime;
                Debug.Log("newFiled.transform.position =" + newPos.ToString());

                //Fields.Add(counter, newFiled);
                //offset.y += widthFiled;
                offset.x -= widthFiled;
                yield return null;
            }
            //Debug.Log("");
            offset.x = 0;
            offset.y -= heightFiled;
        }

        ////foreach (GameObject b in cells)
        ////    Controls.Add(b.btn);
    }

    IEnumerator CreateFieldsAsync()
    {
        Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
        Debug.Log("size Field==" + sizeSpriteRendererField);

        float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
        float heightFiled = sizeSpriteRendererField.y;

        Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
        Debug.Log("size Panel==" + sizeSpriteRendererField);

        var scaleX = prefabPanel.transform.localScale.x;
        var scaleY = prefabPanel.transform.localScale.y;

        float widthPanel = sizeSpriteRendererprefabPanel.x * scaleX;
        float heightPanel = sizeSpriteRendererprefabPanel.y * scaleY;

        int widthLenght = (int)(widthPanel / widthFiled);
        int heightLenght = (int)(heightPanel / heightFiled);

        int maxLengthOfArray = widthLenght * heightLenght;
        Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
        int counter = 0;

        Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
        Debug.Log("panelLocation =" + panelLocation.ToString());

        prefabPanel.GetComponent<Renderer>().enabled = false;

        widthFiled = 1f;
        heightFiled = 1f;

        float offsetX = 1;
        float offsetY = 1;
        for (int heig = 0; heig < widthLenght; heig++)
        {
            for (int wid = 0; wid < heightLenght; wid++)
            {
                counter++;
                Vector2 newPos = new Vector2(offsetX, offsetY);
                GameObject newFiled = (GameObject)Instantiate(prefabField);
                newFiled.transform.position = new Vector2(offsetX, offsetY);
                Debug.Log("newFiled.transform.position =" + newFiled.transform.position.ToString());
                offsetX += widthFiled;

                yield return null;
            }
            offsetX = 0;
            offsetY -= heightFiled;
        }

    }


    public void CreateFields()
    {
        Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
        Debug.Log("size Field==" + sizeSpriteRendererField);

        float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
        float heightFiled = sizeSpriteRendererField.y;

        Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
        Debug.Log("size Panel==" + sizeSpriteRendererField);

        var scaleX = prefabPanel.transform.localScale.x;
        var scaleY = prefabPanel.transform.localScale.y;

        float widthPanel = sizeSpriteRendererprefabPanel.x * scaleX;
        float heightPanel = sizeSpriteRendererprefabPanel.y * scaleY;

        int widthLenght = (int)(widthPanel / widthFiled);
        int heightLenght = (int)(heightPanel / heightFiled);

        int maxLengthOfArray = widthLenght * heightLenght;
        Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
        int counter = 0;

        Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
        Debug.Log("panelLocation =" + panelLocation.ToString());

        prefabPanel.GetComponent<Renderer>().enabled = false;

        widthFiled = 1f;
        heightFiled = 1f;

        float offsetX = 1;
        float offsetY = 1;
        for (int heig = 0; heig < widthLenght; heig++)
        {
            for (int wid = 0; wid < heightLenght; wid++)
            {
                counter++;
                Vector2 newPos = new Vector2(offsetX, offsetY);
                GameObject newFiled = (GameObject)Instantiate(prefabField);
                newFiled.transform.position = new Vector2(offsetX, offsetY);
                Debug.Log("newFiled.transform.position =" + newFiled.transform.position.ToString());
                offsetX += widthFiled;

                //Fields.Add(counter, newFiled);
            }
            offsetX = 0;
            offsetY -= heightFiled;
        }
    }

    public void CreateFields_()
    {
        Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
        Debug.Log("size Field==" + sizeSpriteRendererField);


        float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
        float heightFiled = sizeSpriteRendererField.y;


        Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
        Debug.Log("size Panel==" + sizeSpriteRendererField);

        //Debug.Log("Panel. Scale.x =" + prefabPanel.transform.localScale.x.ToString());
        //Debug.Log("Panel. Scale.y =" + prefabPanel.transform.localScale.y.ToString());
        var scaleX = prefabPanel.transform.localScale.x;
        var scaleY = prefabPanel.transform.localScale.y;
        

        //float widthPanel = sizeSpriteRendererprefabPanel.x; // .Size.Width;
        //float heightPanel = sizeSpriteRendererprefabPanel.y;
        float widthPanel = sizeSpriteRendererprefabPanel.x * scaleX;
        float heightPanel = sizeSpriteRendererprefabPanel.y * scaleY;

        int widthLenght = (int)(widthPanel / widthFiled);
        int heightLenght = (int)(heightPanel / heightFiled);

        int maxLengthOfArray = widthLenght * heightLenght;
        Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
        int counter = 0;

        Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
        Debug.Log("panelLocation =" + panelLocation.ToString());

        //prefabPanel.Visible = false;
        //Debug.Log("visible=" + prefabPanel.GetComponent<Renderer>().enabled.ToString());
        prefabPanel.GetComponent<Renderer>().enabled = false;
        //Debug.Log("visible=" + prefabPanel.GetComponent<Renderer>().enabled.ToString());

        //#TEST
        //widthFiled = 35;
        //heightFiled = 35;

        widthFiled = 1f;
        heightFiled = 1f;
        //widthFiled = 0.5f;
        //heightFiled = 0.5f;


        //Vector2 offset = new Vector2();
        //Vector2 offset = new Vector2(prefabField.transform.position.x, prefabField.transform.position.y);
        //Vector2 offset = new Vector2(prefabField.transform.position.x, prefabField.transform.position.y);
        //float offsetX = prefabField.transform.position.x;
        //float offsetY = prefabField.transform.position.y;
        float offsetX = 1;
        float offsetY = 1;
        //Debug.Log("offset 1=" + offset.ToString());
        for (int heig = 0; heig < widthLenght; heig++)
        {
            for (int wid = 0; wid < heightLenght; wid++)
            {
                counter++;
                //Vector2 newPos = new Vector2();
                //newPos.x = panelLocation.x + offset.x;
                //newPos.y = panelLocation.y + offset.y;
                //Debug.Log("offset 2..=" + offset.ToString());
                //newPos.x = offset.x;
                //newPos.y = offset.y;
                Vector2 newPos = new Vector2(offsetX, offsetY);
                //newPos = new Vector2(offsetX + prefabField.transform.position.x, offsetY + prefabField.transform.position.y);
                

                //Debug.Log("Instantiate(prefabField) " + counter.ToString());
                Debug.Log("Inst Field => " + counter.ToString());
                
                //GameObject newFiled = (GameObject)Instantiate(prefabField);
                //GameObject newFiled = (GameObject)Instantiate(prefabField, newPos * Time.deltaTime);
                //GameObject newFiled = (GameObject)Instantiate(prefabField, prefabPanel.transform);
                GameObject newFiled = (GameObject)Instantiate(prefabField);
                //GameObject newFiled = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //SpriteRenderer sr = newFiled.AddComponent<SpriteRenderer>();
                //sr = prefabField.GetComponent<SpriteRenderer>();

                //Debug.Log("newFiled.transform.position init");
                //newFiled.transform.position = newPos * Time.deltaTime;
                //newFiled.transform.position = new Vector2(prefabField.transform.position.x + offset.x * Time.deltaTime, prefabField.transform.position.y + offset.y * Time.deltaTime);
                    //newFiled.transform.position = new Vector2(offset.x * Time.deltaTime, offset.y * Time.deltaTime);

                //newFiled.transform.position = newPos *Time.deltaTime;
                newFiled.transform.position = new Vector2(offsetX, offsetY);
                //newFiled.transform.position = newPos;
                //newFiled.transform.position = new Vector2(offsetX * Time.deltaTime, offsetY * Time.deltaTime);
                
                
                //newFiled.transform.position = new Vector2(prefabField.transform.position.x + offset.x, prefabField.transform.position.y + offset.y);
                Debug.Log("newFiled.transform.position =" + newFiled.transform.position.ToString());

                //Fields.Add(counter, newFiled);
                //offset.y += widthFiled;
                //offset.x += widthFiled;
                offsetX += widthFiled;
                
            }
            //Debug.Log("");
            //offset.x = 0;
            //offset.y -= heightFiled;
            offsetX = 0;
            offsetY -= heightFiled;
        }

        //for (int y = 0; y < 15; y++)
        //{
        //    for (int x = 0; x < 15; x++)
        //    {
        //        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //        //cube.AddComponent<Rigidbody>();
        //        cube.transform.position = new Vector3(x, y, 0);
        //    }
        //}


       //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
       // cube.transform.position = new Vector3(1, 1);
       // GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
       // cube2.transform.position = new Vector3(1, 0);
       // GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
       // cube3.transform.position = new Vector3(2, 1);


       
    }




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

    //---------------------

    public GameObject prefabCompas;
    public int numberOfObjects = 10;
    public float radius = 0.1f;

    void StartGenCircle()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            pos += new Vector3(10, -5, 0);

            Instantiate(prefabCompas, pos, Quaternion.identity);
        }
    }

    public float gridX = 5f;
    public float gridY = 5f;
    public float spacing = 1f;

    void StartGenGrig()
    {
        //for (int y = 0; y < gridY; y++)
        for (int y = 0; y > (gridY *-1); y--)
        {
            for (int x = 0; x < gridX; x++)
            {
                Vector3 pos = new Vector3(x, y, 1) * spacing;
                //Vector2 pos = new Vector2(x, y) * spacing;
                Instantiate(prefabField, pos, Quaternion.identity);
                //Debug.Log("pos=" + pos);
            }
        }
    } 
}
