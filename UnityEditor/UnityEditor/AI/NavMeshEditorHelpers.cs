// Decompiled with JetBrains decompiler
// Type: UnityEditor.AI.NavMeshEditorHelpers
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor.AI
{
  /// <summary>
  ///   <para>NavMesh utility functionality effective only in the Editor.</para>
  /// </summary>
  public static class NavMeshEditorHelpers
  {
    /// <summary>
    ///   <para>Displays in the Editor the precise intermediate data used during the build process of the specified NavMesh.</para>
    /// </summary>
    /// <param name="navMeshData">NavMesh object for which debug data has been deliberately collected during the build process.</param>
    /// <param name="flags">Bitmask that designates the types of data to show at one time.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DrawBuildDebug(NavMeshData navMeshData, [DefaultValue("NavMeshBuildDebugFlags.All")] NavMeshBuildDebugFlags flags);

    [ExcludeFromDocs]
    public static void DrawBuildDebug(NavMeshData navMeshData)
    {
      NavMeshBuildDebugFlags flags = NavMeshBuildDebugFlags.All;
      NavMeshEditorHelpers.DrawBuildDebug(navMeshData, flags);
    }

    public static void OpenAgentSettings(int agentTypeID)
    {
      NavMeshEditorWindow.OpenAgentSettings(agentTypeID);
    }

    public static void OpenAreaSettings()
    {
      NavMeshEditorWindow.OpenAreaSettings();
    }

    public static void DrawAgentDiagram(Rect rect, float agentRadius, float agentHeight, float agentClimb, float agentSlope)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      float num1 = agentRadius;
      float num2 = agentHeight;
      float num3 = agentClimb;
      float num4 = 0.35f;
      float num5 = 20f;
      float num6 = 10f;
      float b = rect.height - (num5 + num6);
      float num7 = Mathf.Min(b / (num2 + num1 * 2f * num4), b / (num1 * 2f));
      float num8 = num2 * num7;
      float num9 = num1 * num7;
      float num10 = Mathf.Min(num3 * num7, b - num9 * num4);
      float x1 = rect.xMin + rect.width * 0.5f;
      float y1 = (float) ((double) rect.yMax - (double) num6 - (double) num9 * (double) num4);
      Vector3[] vector3Array1 = new Vector3[40];
      Vector3[] vector3Array2 = new Vector3[20];
      Vector3[] vector3Array3 = new Vector3[20];
      for (int index = 0; index < 20; ++index)
      {
        float f = (float) ((double) index / 19.0 * 3.14159274101257);
        float num11 = Mathf.Cos(f);
        float num12 = Mathf.Sin(f);
        vector3Array1[index] = new Vector3(x1 + num11 * num9, (float) ((double) y1 - (double) num8 - (double) num12 * (double) num9 * (double) num4), 0.0f);
        vector3Array1[index + 20] = new Vector3(x1 - num11 * num9, y1 + num12 * num9 * num4, 0.0f);
        vector3Array2[index] = new Vector3(x1 - num11 * num9, (float) ((double) y1 - (double) num8 + (double) num12 * (double) num9 * (double) num4), 0.0f);
        vector3Array3[index] = new Vector3(x1 - num11 * num9, (float) ((double) y1 - (double) num10 + (double) num12 * (double) num9 * (double) num4), 0.0f);
      }
      Color color = Handles.color;
      float xMin = rect.xMin;
      float y2 = y1 - num10;
      float x2 = x1 - b * 0.75f;
      float y3 = y1;
      float x3 = x1 + b * 0.75f;
      float y4 = y1;
      float num13 = x3;
      float num14 = y4;
      float num15 = Mathf.Min(rect.xMax - x3, b);
      float x4 = num13 + Mathf.Cos(agentSlope * ((float) Math.PI / 180f)) * num15;
      float y5 = num14 - Mathf.Sin(agentSlope * ((float) Math.PI / 180f)) * num15;
      Vector3[] vector3Array4 = new Vector3[2]{ new Vector3(xMin, y1, 0.0f), new Vector3(x3 + num15, y1, 0.0f) };
      Vector3[] vector3Array5 = new Vector3[5]{ new Vector3(xMin, y2, 0.0f), new Vector3(x2, y2, 0.0f), new Vector3(x2, y3, 0.0f), new Vector3(x3, y4, 0.0f), new Vector3(x4, y5, 0.0f) };
      Handles.color = !EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.5f) : new Color(0.0f, 0.0f, 0.0f, 0.5f);
      Handles.DrawAAPolyLine(2f, vector3Array4);
      Handles.color = !EditorGUIUtility.isProSkin ? new Color(0.0f, 0.0f, 0.0f, 0.5f) : new Color(1f, 1f, 1f, 0.5f);
      Handles.DrawAAPolyLine(3f, vector3Array5);
      Handles.color = Color.Lerp(new Color(0.0f, 0.75f, 1f, 1f), new Color(0.5f, 0.5f, 0.5f, 0.5f), 0.2f);
      Handles.DrawAAConvexPolygon(vector3Array1);
      if ((double) agentClimb <= (double) agentHeight)
      {
        Handles.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        Handles.DrawAAPolyLine(2f, vector3Array3);
      }
      Handles.color = new Color(1f, 1f, 1f, 0.4f);
      Handles.DrawAAPolyLine(2f, vector3Array2);
      Vector3[] vector3Array6 = new Vector3[2]{ new Vector3(x1, y1 - num8, 0.0f), new Vector3(x1 + num9, y1 - num8, 0.0f) };
      Handles.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
      Handles.DrawAAPolyLine(2f, vector3Array6);
      GUI.Label(new Rect((float) ((double) x1 + (double) num9 + 5.0), (float) ((double) y1 - (double) num8 * 0.5 - 10.0), 150f, 20f), string.Format("H = {0}", (object) agentHeight));
      GUI.Label(new Rect(x1, (float) ((double) y1 - (double) num8 - (double) num9 * (double) num4 - 15.0), 150f, 20f), string.Format("R = {0}", (object) agentRadius));
      GUI.Label(new Rect((float) (((double) xMin + (double) x2) * 0.5 - 20.0), y2 - 15f, 150f, 20f), string.Format("{0}", (object) agentClimb));
      GUI.Label(new Rect(x3 + 20f, y4 - 15f, 150f, 20f), string.Format("{0}°", (object) agentSlope));
      Handles.color = color;
    }
  }
}
