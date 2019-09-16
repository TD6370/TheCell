using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRenderSorting : MonoBehaviour {

    public bool IsHero;
    public GameObject BoneRoorAnimation;

    private bool Disabled = false;

    private int SortingBase = 5000;
    private Renderer rendererSort;
    private Renderer m_rendererSortOther;
    string m_OldFieldHero = "";

    private List<Renderer> renderersSort;
    [SerializeField]
    private int Offset = 0;
    [SerializeField]
    private bool IsOverlapSprite = false;

    private bool m_IsCheckOverlap = false;
    private int m_offsetOverlap = 0;
    [SerializeField]
    public bool IsMeTerra = false;

    private void Awake()
    {
        if (Disabled)
            return;

        renderersSort = new List<Renderer>();
        if (IsHero)
        {
            
            rendererSort = Storage.Instance.HeroModel.GetComponent<Renderer>();
            renderersSort.Add(rendererSort);
        }
        else
        {
            rendererSort = gameObject.GetComponent<Renderer>();
            renderersSort.Add(rendererSort);
        }
        FixedOverlapSprites();
        InitLayersDetailsAnimation();
    }

    private void InitLayersDetailsAnimation()
    {
        if(BoneRoorAnimation!=null)
        {
            var rootRenderer = BoneRoorAnimation.GetComponent<Renderer>();

            renderersSort = new List<Renderer>();
            renderersSort.Add(rootRenderer);

            foreach (Transform child in BoneRoorAnimation.transform)
            {
                GameObject modelAnimation = child.gameObject;
                if (modelAnimation.tag == "BoneModelAnimation")
                {
                    var renderNext = modelAnimation.GetComponent<SpriteRenderer>();
                    renderersSort.Add(renderNext);
                }
            }
        }
    }

    void FixedOverlapSprites()
    {
        m_offsetOverlap = Random.Range(0, 50);
    }
    //void FixedOverlapSprites_()
    //{
    //    Vector2 fieldPos = Helper.NormalizPosToField(gameObject.transform.position.x, gameObject.transform.position.y);
    //    int row = (int)fieldPos.y + 1;
    //    int column = (int)fieldPos.x + 1;
    //    bool RowNotEvenColumnIsEven = (column & 1) == 0 && (row & 1) != 0;
    //    bool RowIsEvenColumnNotEven = (column & 1) != 0 && (row & 1) == 0;
    //    if (RowNotEvenColumnIsEven || RowIsEvenColumnNotEven)
    //    {
    //        if (Helper.IsTerra(gameObject.tag.ToString()))
    //        {
    //            //gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 2f);
    //            IsOverlapSprite = true;
    //            //Offset++;
    //            m_offsetOverlap = 1;
    //        }
    //    }
    //}

    bool isInit = false;
    private void Start()
    {

        
    }

    private string m_lastName = "";

    // Update is called once per frame
    void Update () {

        //if(m_lastName != this.gameObject.name)
        //{
        //    isInit = false;
        //    m_lastName = this.gameObject.name;
        //}

        //if (!isInit)
        //{
        //    var data = this.gameObject.GetData();
        //    if (data != null)
        //    {
        //        if (Helper.IsTerraLayer(data.TypePoolPrefab))
        //        {
        //            IsMeTerra = true;
        //        }
        //    }
        //    else
        //    {
        //        IsMeTerra = true;
        //    }
        //    isInit = true;
        //}
    }
        
    private void OnDisable()
    {
        isInit = false;
    }

    private void FixedUpdate()
    {
        if (Disabled)
            return;

        if (IsMeTerra)
        {
            if (m_OldFieldHero == Storage.Instance.SelectFieldPosHero)
                return;
            m_OldFieldHero = Storage.Instance.SelectFieldPosHero;
        }

        float offsetCalculate = SortingBase - gameObject.transform.position.y; // - Offset; //@@+ fix
        offsetCalculate = (float)System.Math.Round(offsetCalculate, 2);
        offsetCalculate *= 100;
        
        //rendererSort.sortingOrder = (int)offsetCalculate + m_offsetOverlap + (Offset*100);
        ////Legacy code
        //if (m_rendererSortOther != null)
        //    m_rendererSortOther.sortingOrder = rendererSort.sortingOrder + 1;

        int order = (int)offsetCalculate + m_offsetOverlap + (Offset * 100);

        foreach(Renderer renderer in renderersSort)
        {
            if(renderer!=null)
                renderer.sortingOrder = order++;
        }
    }

    public void UpdateOrderingLayer(Renderer rend = null) 
    {
        if (rend != null)
        {
            //rendererSort = rend;
            m_rendererSortOther = gameObject.GetComponent<Renderer>();
            renderersSort = new List<Renderer>();
            renderersSort.Add(rend);

            if(m_rendererSortOther!=null)
                renderersSort.Add(m_rendererSortOther);
        }
        //if (rend != null && !rend.isVisible)
        //{
        //    this.enabled = false;
        //    Debug.Log("## UpdateOrderingLayer DISABLED >> " + this.gameObject.name);
        //}
    }

    public void UpdateOrderingLayer(Renderer rendBack = null, Renderer rendFront = null)
    {
        if (rendBack != null)
        {
            //rendererSort = rendBack;
            //m_rendererSortOther = rendFront;

            renderersSort = new List<Renderer>();
            renderersSort.Add(rendBack);
            renderersSort.Add(rendFront);
        }
    }
}
