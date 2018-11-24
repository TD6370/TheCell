using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DrawGeometry : MonoBehaviour
{
    public bool IsParallaxOn = false;

    private LineRenderer m_lineRenderer;

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
                if (itemObj.tag != SaveLoadData.TypePrefabs.PrefabField.ToString())
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
                if (itemObj.tag != SaveLoadData.TypePrefabs.PrefabField.ToString())
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
