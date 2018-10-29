using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRenderSorting : MonoBehaviour {


    private int SortingBase = 5000;
    private Renderer rendererSort;
    [SerializeField]
    private int Offset = 0;

    private void Awake()
    {
        rendererSort = gameObject.GetComponent<Renderer>();
    }

	
	// Update is called once per frame
	void Update () {
        //rendererSort.sortingOrder = (int)(SortingBase - gameObject.transform.position.y);
        //rendererSort.sortingOrder = (int)(SortingBase - Mathf.Abs(gameObject.transform.position.y - Offset));
        rendererSort.sortingOrder = (int)(SortingBase - gameObject.transform.position.y - Offset);
        //rendererSort.sortingOrder = (int)gameObject.transform.position.y;

    }
}
