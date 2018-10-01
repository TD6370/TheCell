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
        //CreateFields();
        //StartCoroutine(CreateFieldsAsync());
        
    }


    void Awake()
    {
        CreateFields();
        //StartCoroutine(CreateFieldsAsync());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator CreateFieldsAsync()
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

    public void CreateFields()
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
        widthFiled = 35;
        heightFiled = 35;


        //Vector2 offset = new Vector2();
        //Vector2 offset = new Vector2(prefabField.transform.position.x, prefabField.transform.position.y);
        //Vector2 offset = new Vector2(prefabField.transform.position.x, prefabField.transform.position.y);
        //float offsetX = prefabField.transform.position.x;
        //float offsetY = prefabField.transform.position.y;
        float offsetX = 0;
        float offsetY = 0;
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
                GameObject newFiled = (GameObject)Instantiate(prefabField);
                //Debug.Log("newFiled.transform.position init");
                //newFiled.transform.position = newPos * Time.deltaTime;
                //newFiled.transform.position = new Vector2(prefabField.transform.position.x + offset.x * Time.deltaTime, prefabField.transform.position.y + offset.y * Time.deltaTime);
                    //newFiled.transform.position = new Vector2(offset.x * Time.deltaTime, offset.y * Time.deltaTime);
                newFiled.transform.position = newPos * Time.deltaTime;
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

       
    }
}
