using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TrackPointsNavigator : MonoBehaviour {

    public Color ColorTrack = Color.blue;
    public LineTextureMode TextureMode = LineTextureMode.Stretch;
    private LineRenderer lineRenderer;
    public float sizeTrack = 0.5f;
    public Material MaterialTrack = null;
    public int MaxLenLine = 5;

    private List<Vector3> m_TrackPoints = new List<Vector3>();
    private int countPoints = -1;
    public List<Vector3> TrackPoints
    {
        get
        {
            return m_TrackPoints;
        }
        set
        {
            m_TrackPoints = value;
        }
    }


    // Use this for initialization
    void Start () {

        //Debug.Log("TrackPointsNavigator StartCoroutine   DrawTrackPoints");
        StartCoroutine(DrawTrackPoints());
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //void Update()
    //{
    //    lr.textureMode = textureMode;
    //    lr.material.SetTextureScale("_MainTex", new Vector2(tileAmount, 1.0f));
    //}

    void OnGUI()
    {
        //var textureMode = GUI.Toggle(new Rect(25, 25, 200, 30), TextureMode == LineTextureMode.Tile, "Tiled") ? LineTextureMode.Tile : LineTextureMode.Stretch;

        //if (textureMode == LineTextureMode.Tile)
        //{
        //    GUI.Label(new Rect(25, 60, 200, 30), "Tile Amount");
        //    tileAmount = GUI.HorizontalSlider(new Rect(125, 65, 200, 30), tileAmount, 0.1f, 4.0f);
        //}
    }

    IEnumerator DrawTrackPoints()
    {
        //Debug.Log("TrackPointsNavigator DrawTrackPoints..... start");

        while (true)
        {
            yield return new WaitForSeconds(1);

            //Debug.Log("TrackPointsNavigator DrawTrackPoints.....");

            if (countPoints!= m_TrackPoints.Count)
            {
                //Debug.Log("DrawTrackPoints (" + this.gameObject.name + ")....");

                countPoints = m_TrackPoints.Count;
                DrawTrack(m_TrackPoints, ColorTrack);
            }
        }

        Debug.Log("TrackPointsNavigator DrawTrackPoints..... END");

    }

    public void DrawTrack(List<Vector3> trackPoints, Color colorTrack)
    {
        //Debug.Log("TrackPointsNavigator DrawTrack ****** ");

        //LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startColor = colorTrack;
            lineRenderer.endColor = colorTrack;
            
            //lineRenderer.colorGradient = new Gradient();
            lineRenderer.textureMode = TextureMode;
            if(MaterialTrack==null)
                MaterialTrack = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Particle.mat");
            lineRenderer.material = MaterialTrack;

            lineRenderer.startWidth = sizeTrack;
            lineRenderer.endWidth = sizeTrack;
        }

        if (lineRenderer == null)
        {
            Debug.Log("LineRenderer is null !!!!");
            return;
        }

        int maxPoint = (trackPoints.Count > MaxLenLine) ? MaxLenLine : trackPoints.Count;
        lineRenderer.positionCount = maxPoint;
        int maxPointFor = trackPoints.Count() - maxPoint;

        string indErr = "";
        try
        {
            //for (int i = 0; i < trackPoints.Count(); i++)
            int indNext = 0;
            //for (int i = trackPoints.Count()-1; i >= 0; i--)
            //for (int i = maxPoint - 1; i >= 0; i--)
            //for (int i = trackPoints.Count() - 1; i >= maxPointFor; i--)
            for (int i = trackPoints.Count() - 1; i >= maxPointFor; i--)
            {
                indErr = i.ToString();
                //Vector3 posNext = new Vector3(trackPoints[i].x, trackPoints[i].y, -2);
                Vector3 posNext = new Vector3(trackPoints[i].x, trackPoints[i].y, -2);

                indErr += "pos1=" + posNext.x + "x" + posNext.y;
                if (lineRenderer != null)
                {
                    //Debug.Log("_____" + this.name + " DrawTrack lineRenderer SetPosition(" + indNext + ", " + posNext + ") I=" + i + "  minimum=" + maxPointFor + " maxPoint=" + maxPoint + "  trackPoints.Count:" + trackPoints.Count);
                    //Debug.Log("_____" + this.name + " DrawTrack lineRenderer SetPosition(" + indNext + ") I=" + i + "  minimum=" + maxPointFor + " maxPoint=" + maxPoint + "  trackPoints.Count:" + trackPoints.Count);
                    indErr = "_____" + this.name + " DrawTrack lineRenderer SetPosition(" + indNext + ") I=" + i + "  minimum=" + maxPointFor + " maxPoint=" + maxPoint + "  trackPoints.Count:" + trackPoints.Count;
                    lineRenderer.SetPosition(indNext, posNext);
                }
                else
                    Debug.Log("####### DrawTrack lineRenderer is null");

                //if(indNext > MaxLenLine)
                //{
                //    Debug.Log("####### DrawTrack IS LIMIT : " + MaxLenLine + ")   Points count =" + indNext);
                //    lineRenderer = null;
                //    GetComponent<LineRenderer>().SetPositions(new Vector3[0]);
                //    lineRenderer.positionCount = 0;
                //    break;
                //}
                indNext++;
            }

            indErr = "end for";
            if (trackPoints.Count > MaxLenLine)
            {
                indErr = "reset";
                //Debug.Log("####### DrawTrack IS LIMIT : " + MaxLenLine + ")   Points count =" + indNext);
                indErr = "reset lineRenderer null";
                //lineRenderer = null;
                indErr = "reset lineRenderer SetPositions in zero";
                //GetComponent<LineRenderer>().SetPositions(new Vector3[0]);
                indErr = "reset lineRenderer positionCount = 0";
                //lineRenderer.positionCount = 0;
                //break;
            }
            

            //-------------
            //for (int i = 0; i < trackPoints.Count(); i++)
            //{
            //    indErr = i.ToString();
            //    //Vector3 posNext = new Vector3(trackPoints[i].x, trackPoints[i].y, -2);
            //    Vector3 posNext = new Vector3(trackPoints[i].x, trackPoints[i].y, -2);

            //    indErr += "pos1=" + posNext.x + "x" + posNext.y;
            //    if (lineRenderer != null)
            //    {
            //        Debug.Log("_____DrawTrack lineRenderer SetPosition(" + i + ", " + posNext + ")");
            //        lineRenderer.SetPosition(i, posNext);
            //    }
            //    else
            //        Debug.Log("####### DrawTrack lineRenderer is null");

            //    if (indNext > MaxLenLine)
            //    {
            //        Debug.Log("####### DrawTrack IS LIMIT : " + MaxLenLine + ")   Points count =" + indNext);
            //        lineRenderer = null;
            //        GetComponent<LineRenderer>().SetPositions(new Vector3[0]);
            //        GetComponent<LineRenderer>().SetPositions(new Vector3[0]);
            //        lineRenderer.positionCount = 0;
            //        break;
            //    }
            //    indNext++;
            //}
        }
        catch (Exception x)
        {
            Debug.Log("####### Error DrawTrack (" + indErr + ") : " + x.Message);
        }

    }
}
