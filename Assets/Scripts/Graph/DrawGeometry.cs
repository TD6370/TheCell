﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Rendering.PostProcessing;
using UnityEngine.PostProcessing;

//[ExecuteInEditMode]
public class DrawGeometry : MonoBehaviour
{
    public bool IsParallaxOn = false;
    
    private LineRenderer m_lineRenderer;
    //private PostProcessVolume m_PostProcess;
    //private SpriteRenderer m_SpriteRenderer;
      

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    


    

    //ColorBlock cb = buttonCommand.colors;
    //cb.normalColor = Color.yellow;
    //cb.disabledColor = Color.yellow;
    //cb.highlightedColor = Color.yellow;
    //cb.pressedColor = Color.yellow;
    //buttonCommand.colors = cb;

    //string colorStr = "#" + ColorUtility.ToHtmlStringRGB(m_ColorRender); 
    //ColorUtility.TryParseHtmlString(color, out outColor);

    public void DrawClear()
    {
        m_lineRenderer.positionCount = 0;
    }
            
    public void DrawRect(float x, float y, float x2, float y2, Color color, float width = 0.2f )
    {
        //m_lineRenderer = GetComponent<LineRenderer>();
        if (m_lineRenderer == null)
        {
            Debug.Log("LineRenderer is null !!!!");
            return;
        }

        //float alpha = 1.0f;
        //Gradient gradient = new Gradient();
        //gradient.SetKeys(
        //    new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
        //    new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        //    );
        //m_lineRenderer.colorGradient = gradient;

        //color = Color.green;
        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        //lineRenderer.SetColors(c1, c2);
        m_lineRenderer.SetColors(color, color);
        //m_lineRenderer.startColor = Color.red;
        m_lineRenderer.SetWidth(width, width);
        int size = 5;
        m_lineRenderer.SetVertexCount(size);

        Vector3 pos1 = new Vector3(x, y, -2);
        m_lineRenderer.SetPosition(0, pos1);
        Vector3 pos2 = new Vector3(x2, y, -2);
        m_lineRenderer.SetPosition(1, pos2);
        Vector3 pos3 = new Vector3(x2, y2, -2);
        m_lineRenderer.SetPosition(2, pos3);
        Vector3 pos4 = new Vector3(x, y2, -2);
        m_lineRenderer.SetPosition(3, pos4);
        Vector3 pos5 = new Vector3(x, y, -2);
        m_lineRenderer.SetPosition(4, pos5);
    }

    #region DrawLine
    /*
    public static void DrawPolyLine(params Vector3[] points)
    {
        if (!Handles.BeginLineDrawing(Handles.matrix, false))
            return;
        for (int index = 1; index < points.Length; ++index)
        {
            GL.Vertex(points[index]);
            GL.Vertex(points[index - 1]);
        }
        Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a line from p1 to p2.</para>
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    public static void DrawLine(Vector3 p1, Vector3 p2)
    {
        if (!Handles.BeginLineDrawing(Handles.matrix, false))
            return;
        GL.Vertex(p1);
        GL.Vertex(p2);
        Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a list of line segments.</para>
    /// </summary>
    /// <param name="lineSegments">A list of pairs of points that represent the start and end of line segments.</param>
    public static void DrawLines(Vector3[] lineSegments)
    {
        if (!Handles.BeginLineDrawing(Handles.matrix, false))
            return;
        int index = 0;
        while (index < lineSegments.Length)
        {
            Vector3 lineSegment1 = lineSegments[index];
            Vector3 lineSegment2 = lineSegments[index + 1];
            GL.Vertex(lineSegment1);
            GL.Vertex(lineSegment2);
            index += 2;
        }
        Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a list of indexed line segments.</para>
    /// </summary>
    /// <param name="points">A list of points.</param>
    /// <param name="segmentIndices">A list of pairs of indices to the start and end points of the line segments.</param>
    public static void DrawLines(Vector3[] points, int[] segmentIndices)
    {
        if (!Handles.BeginLineDrawing(Handles.matrix, false))
            return;
        int index = 0;
        while (index < segmentIndices.Length)
        {
            Vector3 point1 = points[segmentIndices[index]];
            Vector3 point2 = points[segmentIndices[index + 1]];
            GL.Vertex(point1);
            GL.Vertex(point2);
            index += 2;
        }
        Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a dotted line from p1 to p2.</para>
    /// </summary>
    /// <param name="p1">The start point.</param>
    /// <param name="p2">The end point.</param>
    /// <param name="screenSpaceSize">The size in pixels for the lengths of the line segments and the gaps between them.</param>
    public static void DrawDottedLine(Vector3 p1, Vector3 p2, float screenSpaceSize)
    {
        if (!Handles.BeginLineDrawing(Handles.matrix, true))
            return;
        float x = screenSpaceSize * EditorGUIUtility.pixelsPerPoint;
        GL.MultiTexCoord(1, p1);
        GL.MultiTexCoord2(2, x, 0.0f);
        GL.Vertex(p1);
        GL.MultiTexCoord(1, p1);
        GL.MultiTexCoord2(2, x, 0.0f);
        GL.Vertex(p2);
        Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a list of dotted line segments.</para>
    /// </summary>
    /// <param name="lineSegments">A list of pairs of points that represent the start and end of line segments.</param>
    /// <param name="screenSpaceSize">The size in pixels for the lengths of the line segments and the gaps between them.</param>
    public static void DrawDottedLines(Vector3[] lineSegments, float screenSpaceSize)
    {
        if (!Handles.BeginLineDrawing(Handles.matrix, true))
            return;
        float x = screenSpaceSize * EditorGUIUtility.pixelsPerPoint;
        int index = 0;
        while (index < lineSegments.Length)
        {
            Vector3 lineSegment1 = lineSegments[index];
            Vector3 lineSegment2 = lineSegments[index + 1];
            GL.MultiTexCoord(1, lineSegment1);
            GL.MultiTexCoord2(2, x, 0.0f);
            GL.Vertex(lineSegment1);
            GL.MultiTexCoord(1, lineSegment1);
            GL.MultiTexCoord2(2, x, 0.0f);
            GL.Vertex(lineSegment2);
            index += 2;
        }
        Handles.EndLineDrawing();
    }

    public static void DrawDottedLines(Vector3[] points, int[] segmentIndices, float screenSpaceSize)
    {
        if (!Handles.BeginLineDrawing(Handles.matrix, true))
            return;
        float x = screenSpaceSize * EditorGUIUtility.pixelsPerPoint;
        int index = 0;
        while (index < segmentIndices.Length)
        {
            Vector3 point1 = points[segmentIndices[index]];
            Vector3 point2 = points[segmentIndices[index + 1]];
            GL.MultiTexCoord(1, point1);
            GL.MultiTexCoord2(2, x, 0.0f);
            GL.Vertex(point1);
            GL.MultiTexCoord(1, point1);
            GL.MultiTexCoord2(2, x, 0.0f);
            GL.Vertex(point2);
            index += 2;
        }
        Handles.EndLineDrawing();
    }
    */
    //============================
    #endregion

    IEnumerator AminateAlphaColor(GameObject obj)
    {
        while (true)
        {
            var color = obj.GetComponent<Renderer>().material.color;
            for (float i = 1; i >= 0; i -= 0.1f)
            {
                color.a = i;
                obj.GetComponent<Renderer>().material.color = color;
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            for (float i = 0; i < 1; i += 0.1f)
            {
                color.a = i;
                obj.GetComponent<Renderer>().material.color = color;
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private bool IsNotFloorAndFlore(GameObject gobj)
    {
        bool isTrue;
        if (PoolGameObjects.IsUseTypePoolPrefabs)
        {
            isTrue = gobj.IsPoolFloor() || gobj.IsPoolFlore();
        }
        else
        {
            isTrue = gobj.tag == SaveLoadData.TypePrefabs.PrefabField.ToString();
        }
        return isTrue == false;
    }

    public void ParallaxOn()
    {
        Camera cam = Storage.Instance.MainCamera;

        IsParallaxOn = !IsParallaxOn;
        //float angleParallax = 35f;
        //float angleParallax = 15f;
        float angleParallax = 20f;
        if (IsParallaxOn)
        {
            cam.orthographic = false;

            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y -5f, cam.transform.position.z - 20f);
            cam.transform.rotation = Quaternion.Euler(-10f, 0f, 0f);

           // Storage.PlayerController.transform.rotation = Quaternion.Euler(-6f, 0f, 0f);

            float anglePrlx = angleParallax * (-1f);
            Storage.GridData.PrefabVood.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
            Storage.GridData.PrefabElka.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
            Storage.GridData.PrefabRock.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
            Storage.GridData.PrefabBoss.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
            Storage.GridData.PrefabWallRock.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
            Storage.GridData.PrefabWallWood.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
            foreach(GameObject itemObj in Storage.Instance.GamesObjectsReal.SelectMany(p=>p.Value))
            {

                bool isNotFloorAndFlore = IsNotFloorAndFlore(itemObj);
                if (isNotFloorAndFlore)
                {
                    itemObj.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
                }
            }
            foreach (PoolGameObject itemPoolObj in Storage.Pool.PoolGamesObjectsStack.SelectMany(p => p.Value))
            {
                if (itemPoolObj.GameObjectNext != null)
                {
                    bool isNotFloorAndFlore = IsNotFloorAndFlore(itemPoolObj.GameObjectNext);
                    if (isNotFloorAndFlore)
                    {
                        itemPoolObj.GameObjectNext.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
                    }
                }
            }
        }
        else
        {
            cam.orthographic = true;
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + 5f, cam.transform.position.z + 20f);
            cam.transform.rotation = Quaternion.Euler(10f, 0f, 0f);

            //cam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            //Storage.PlayerController.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            float anglePrlx = angleParallax;
            Storage.GridData.PrefabVood.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
            Storage.GridData.PrefabElka.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
            Storage.GridData.PrefabRock.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
            Storage.GridData.PrefabBoss.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
            Storage.GridData.PrefabWallRock.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
            Storage.GridData.PrefabWallWood.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);

            foreach (GameObject itemObj in Storage.Instance.GamesObjectsReal.SelectMany(p => p.Value))
            {
                bool isNotFloorAndFlore = IsNotFloorAndFlore(itemObj);
                if (isNotFloorAndFlore)
                {
                    itemObj.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
                }
            }
            foreach (PoolGameObject itemObj in Storage.Pool.PoolGamesObjectsStack.SelectMany(p => p.Value))
            {
                if (itemObj.GameObjectNext != null)
                {

                    if (itemObj.GameObjectNext.tag != SaveLoadData.TypePrefabs.PrefabField.ToString())
                    {
                        itemObj.GameObjectNext.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
                    }

                    //var tagPrefab = Storage.GridData.GetTypePool(itemObj.Tag);
                    //if (tagPrefab != SaveLoadData.TypePrefabs.PrefabField.ToString())
                    //{

                    //    itemObj.GameObjectNext.transform.rotation = Quaternion.Euler(anglePrlx, 0f, 0f);
                    //}
                }
            }
        }
               
    }

    //public void DrawTrack2(List<Vector3> trackPoints, Color colorTrack)
    //{
    //    foreach(var point in trackPoints)
    //    {
    //        Debug.Log("DrawPolyLine Point : " + point);
    //    }
    //    Handles.DrawPolyLine(trackPoints.ToArray());
    //}

    //public static Vector3 PositionHandle(Vector3 position, Quaternion rotation)
    //{
    //    float handleSize = HandleUtility.GetHandleSize(position);
    //    Color color = Handles.color;
    //    Handles.color = Handles.xAxisColor;
    //    position = Handles.Slider(position, rotation * Vector3.right, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), SnapSettings.move.x);
    //    Handles.color = Handles.yAxisColor;
    //    position = Handles.Slider(position, rotation * Vector3.up, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), SnapSettings.move.y);
    //    Handles.color = Handles.zAxisColor;
    //    position = Handles.Slider(position, rotation * Vector3.forward, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), SnapSettings.move.z);
    //    Handles.color = Handles.centerColor;
    //    position = Handles.FreeMoveHandle(position, rotation, handleSize * 0.15f, SnapSettings.move, new Handles.DrawCapFunction(Handles.RectangleCap));
    //    Handles.color = color;
    //    return position;
    //}

    private void DrawRect(float x, float y, float x2, float y2)
    {
        //return;
        //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.Log("LineRenderer is null !!!!");
            return;
        }

        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        //lineRenderer.SetColors(c1, c2);
        lineRenderer.SetColors(Color.green, Color.green);
        lineRenderer.SetWidth(0.2F, 0.2F);
        int size = 5;
        lineRenderer.SetVertexCount(size);

        Vector3 pos1 = new Vector3(x, y, -2);
        lineRenderer.SetPosition(0, pos1);
        Vector3 pos2 = new Vector3(x2, y, -2);
        lineRenderer.SetPosition(1, pos2);
        Vector3 pos3 = new Vector3(x2, y2, -2);
        lineRenderer.SetPosition(2, pos3);
        Vector3 pos4 = new Vector3(x, y2, -2);
        lineRenderer.SetPosition(3, pos4);
        Vector3 pos5 = new Vector3(x, y, -2);
        lineRenderer.SetPosition(4, pos5);
    }

    void StartGenCircle(GameObject prefabCompas, int numberOfObjects = 10, float radius = 0.1f)
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;

            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            pos += new Vector3(10, -5, 0);

            Instantiate(prefabCompas, pos, Quaternion.identity);
        }
    }

    public Color GetRandomColor()
    {
        System.Random rnd = new System.Random();
        float r = rnd.Next(1, 255);
        float g = rnd.Next(1, 255);
        float b = rnd.Next(1, 255);
        return new Color(r, g, b, 1);
    }
}

public static class DrawExtensions
{
    public static void OnDrawRect(this LineRenderer sender, float x, float y, float x2, float y2, Color color, float width = 0.2f)
    {
        //m_lineRenderer = GetComponent<LineRenderer>();
        if (sender == null)
        {
            Debug.Log("LineRenderer is null !!!!");
            return;
        }

        //float alpha = 1.0f;
        //Gradient gradient = new Gradient();
        //gradient.SetKeys(
        //    new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
        //    new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        //    );
        //m_lineRenderer.colorGradient = gradient;

        //color = Color.green;
        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        //lineRenderer.SetColors(c1, c2);
        sender.SetColors(color, color);
        //m_lineRenderer.startColor = Color.red;
        sender.SetWidth(width, width);
        int size = 5;
        sender.SetVertexCount(size);

        Vector3 pos1 = new Vector3(x, y, -2);
        sender.SetPosition(0, pos1);
        Vector3 pos2 = new Vector3(x2, y, -2);
        sender.SetPosition(1, pos2);
        Vector3 pos3 = new Vector3(x2, y2, -2);
        sender.SetPosition(2, pos3);
        Vector3 pos4 = new Vector3(x, y2, -2);
        sender.SetPosition(3, pos4);
        Vector3 pos5 = new Vector3(x, y, -2);
        sender.SetPosition(4, pos5);
    }

    public static void SetColor(this Button btn, string strColorBack ="", string strColorText="")
    {
        if (!String.IsNullOrEmpty(strColorBack))
        {
            btn.GetComponent<Image>().color = strColorBack.ToColor();
        }

        if (!String.IsNullOrEmpty(strColorText))
        {
            var colorText = btn.GetComponentInChildren<Text>().color;
            if (colorText == null)
            {
                Debug.Log("########### Button.SetColor GetComponentInChildren<Text>() is null");
                return;
            }
            btn.GetComponentInChildren<Text>().color = strColorText.ToColor();
            //colorText = strColorText.ToColor();
        }
    }

    public static Color ToColor(this string strColor)
    {
        Color outColor = Color.clear;
        try
        {
            ColorUtility.TryParseHtmlString(strColor, out outColor);
        }
        catch(Exception x)
        {
            Debug.Log("##################### ToColor: " + strColor  + " > " + x.Message);
        }
        return outColor;
    }

    public static Color ToColor(this string color, Color oldColor)
    {
        Color resColor = Color.clear;

        if (!string.IsNullOrEmpty(color))
        {
            string indErr = "srart";
            try
            {
                if (color.IndexOf("#") != 0)
                    color = "#" + color;

                //Debug.Log("ToColor parse");

                indErr = "6";
                string parseColor = "#" + ColorUtility.ToHtmlStringRGB(oldColor);
                if (parseColor != color)
                {
                    Color outColor = Color.clear;
                    indErr = "3";
                    ColorUtility.TryParseHtmlString(color, out outColor);

                    indErr = "9";
                    //Debug.Log("ColorRender SET " + _ColorLevel + "      ColorRender=" + ColorRender.ToString() + "      outColor=" + outColor.ToString() +  " RGB:" + testStr1 + "  RGBA:" + testStr2);
                    //Debug.Log("ColorRender SET    PARSE OLD COLOR: " + parseColor + "  VALUE: " + outColor);
                    indErr = "10";
                    resColor = outColor;
                }
                else
                {
                    return oldColor;
                }
            }
            catch (Exception x)
            {
                Debug.Log("############ Error GameDataBoss.ColorLevel (" + indErr + ") : " + x.Message);
            }
        }
        else
        {
            Debug.Log("############ Error GameDataBoss.ColorLevel (" + color + ") is null");
        }
        return resColor;
    }

    public static void SetAlpha(this GameObject gobj, Single _alpha = .5f)
    {
        gobj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, _alpha);
    }

    public static void SetAlpha(this SpriteRenderer spriteRender, Single _alpha = .5f)
    {
        spriteRender.color = new Color(1f, 1f, 1f, _alpha);
        //return;

        Material material = spriteRender.material;
        if (_alpha == 1f)
        {
            //material.SetFloat("_Mode", 4f);
            Color32 col = material.GetColor("_Color");
            col.a = 255;
            spriteRender.material.SetColor("_Color", col);
        }
        else
        {
            //material.SetFloat("_Mode", 4f);
            //material = new Material(Shader.Find("Standard"));

            //material.SetFloat("_Mode", 2f);
            //material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            //material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            //material.SetInt("_ZWrite", 0);
            //material.DisableKeyword("_ALPHATEST_ON");
            //material.EnableKeyword("_ALPHABLEND_ON");
            //material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            //material.renderQueue = 3000;

            Color32 col = material.GetColor("_Color");
            col.a = (byte)(255 * _alpha);
            spriteRender.material.SetColor("_Color", col);
        }

        //0f - opacity
        //1f - cutout
        //2f - fade
        //3f - transparent

    }

}

public static class TextureExtension
{
    public static void DrawPixeles(this Texture2D texture, int startX, int startY, int addSize, int sizeDraw, Color colorCell)
    {
        for (int x2 = startX; x2 < startX + addSize; x2++)
        {
            for (int y2 = startY; y2 < startY + addSize; y2++)
            {
                texture.SetPixel(x2, sizeDraw - y2, colorCell);
            }
        }
    }
}
