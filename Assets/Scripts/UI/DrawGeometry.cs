using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DrawGeometry : MonoBehaviour
{

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



}

public static class DrawExtensions
{
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
        ColorUtility.TryParseHtmlString(strColor, out outColor);

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

}
