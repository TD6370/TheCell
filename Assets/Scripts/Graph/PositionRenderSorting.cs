using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRenderSorting : MonoBehaviour {

    public bool IsHero;
    private int SortingBase = 5000;
    private Renderer rendererSort;
    [SerializeField]
    private int Offset = 0;
    [SerializeField]
    private bool IsOverlapSprite = false;

    private bool m_IsCheckOverlap = false;
    private int m_offsetOverlap = 0;

    private void Awake()
    {
        if (IsHero)
        {
            rendererSort = Storage.Instance.HeroModel.GetComponent<Renderer>();
        }
        else
        {
            rendererSort = gameObject.GetComponent<Renderer>();
        }
        FixedOverlapSprites();
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

    private void Start()
    {
    }
    // Update is called once per frame
    void Update () {
        
        //rendererSort.sortingOrder = (int)(SortingBase - gameObject.transform.position.y - Offset);

        //if(!m_IsCheckOverlap)
        //{
        //    FixedOverlapSprites();
        //    m_IsCheckOverlap = true;
        //}

    }

    private void FixedUpdate()
    {
        //rendererSort.sortingOrder = (int)(SortingBase - gameObject.transform.position.y - Offset);
        //rendererSort.sortingOrder = ((int)(SortingBase - gameObject.transform.position.y - Offset)) + m_offsetOverlap;
        float offsetCalculate = SortingBase - gameObject.transform.position.y - Offset;
        offsetCalculate = (float)System.Math.Round(offsetCalculate, 2);
        offsetCalculate *= 100;
        rendererSort.sortingOrder = (int)offsetCalculate + m_offsetOverlap;
    }
}
