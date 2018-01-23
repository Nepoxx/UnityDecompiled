// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightingWindowLightmapPreviewTab
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class LightingWindowLightmapPreviewTab
  {
    private Vector2 m_ScrollPositionLightmaps = Vector2.zero;
    private Vector2 m_ScrollPositionMaps = Vector2.zero;
    private int m_SelectedLightmap = -1;
    private static LightingWindowLightmapPreviewTab.Styles s_Styles;

    private static void DrawHeader(Rect rect, bool showdrawDirectionalityHeader, bool showShadowMaskHeader, float maxLightmaps)
    {
      rect.width /= maxLightmaps;
      EditorGUI.DropShadowLabel(rect, "Intensity");
      rect.x += rect.width;
      if (showdrawDirectionalityHeader)
      {
        EditorGUI.DropShadowLabel(rect, "Directionality");
        rect.x += rect.width;
      }
      if (!showShadowMaskHeader)
        return;
      EditorGUI.DropShadowLabel(rect, "Shadowmask");
    }

    private void MenuSelectLightmapUsers(Rect rect, int lightmapIndex)
    {
      if (Event.current.type != EventType.ContextClick || !rect.Contains(Event.current.mousePosition))
        return;
      EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f), EditorGUIUtility.TempContent(new string[1]
      {
        "Select Lightmap Users"
      }), -1, new EditorUtility.SelectMenuItemFunction(this.SelectLightmapUsers), (object) lightmapIndex);
      Event.current.Use();
    }

    private void SelectLightmapUsers(object userData, string[] options, int selected)
    {
      int num = (int) userData;
      ArrayList arrayList = new ArrayList();
      foreach (MeshRenderer meshRenderer in UnityEngine.Object.FindObjectsOfType(typeof (MeshRenderer)) as MeshRenderer[])
      {
        if ((UnityEngine.Object) meshRenderer != (UnityEngine.Object) null && meshRenderer.lightmapIndex == num)
          arrayList.Add((object) meshRenderer.gameObject);
      }
      foreach (Terrain terrain in UnityEngine.Object.FindObjectsOfType(typeof (Terrain)) as Terrain[])
      {
        if ((UnityEngine.Object) terrain != (UnityEngine.Object) null && terrain.lightmapIndex == num)
          arrayList.Add((object) terrain.gameObject);
      }
      Selection.objects = arrayList.ToArray(typeof (UnityEngine.Object)) as UnityEngine.Object[];
    }

    public void LightmapPreview(Rect r)
    {
      if (LightingWindowLightmapPreviewTab.s_Styles == null)
        LightingWindowLightmapPreviewTab.s_Styles = new LightingWindowLightmapPreviewTab.Styles();
      GUI.Box(r, "", (GUIStyle) "PreBackground");
      this.m_ScrollPositionLightmaps = EditorGUILayout.BeginScrollView(this.m_ScrollPositionLightmaps, GUILayout.Height(r.height));
      int lightmapIndex = 0;
      bool showdrawDirectionalityHeader = false;
      bool showShadowMaskHeader = false;
      foreach (LightmapData lightmap in LightmapSettings.lightmaps)
      {
        if ((UnityEngine.Object) lightmap.lightmapDir != (UnityEngine.Object) null)
          showdrawDirectionalityHeader = true;
        if ((UnityEngine.Object) lightmap.shadowMask != (UnityEngine.Object) null)
          showShadowMaskHeader = true;
      }
      float num1 = 1f;
      if (showdrawDirectionalityHeader)
        ++num1;
      if (showShadowMaskHeader)
        ++num1;
      LightingWindowLightmapPreviewTab.DrawHeader(GUILayoutUtility.GetRect(r.width, r.width, 20f, 20f), showdrawDirectionalityHeader, showShadowMaskHeader, num1);
      foreach (LightmapData lightmap in LightmapSettings.lightmaps)
      {
        if ((UnityEngine.Object) lightmap.lightmapColor == (UnityEngine.Object) null && (UnityEngine.Object) lightmap.lightmapDir == (UnityEngine.Object) null && (UnityEngine.Object) lightmap.shadowMask == (UnityEngine.Object) null)
        {
          ++lightmapIndex;
        }
        else
        {
          int num2 = !(bool) ((UnityEngine.Object) lightmap.lightmapColor) ? -1 : Math.Max(lightmap.lightmapColor.width, lightmap.lightmapColor.height);
          int num3 = !(bool) ((UnityEngine.Object) lightmap.lightmapDir) ? -1 : Math.Max(lightmap.lightmapDir.width, lightmap.lightmapDir.height);
          int num4 = !(bool) ((UnityEngine.Object) lightmap.shadowMask) ? -1 : Math.Max(lightmap.shadowMask.width, lightmap.shadowMask.height);
          Texture2D texture2D = num2 <= num3 ? (num3 <= num4 ? lightmap.shadowMask : lightmap.lightmapDir) : (num2 <= num4 ? lightmap.shadowMask : lightmap.lightmapColor);
          GUILayoutOption[] guiLayoutOptionArray = new GUILayoutOption[2]{ GUILayout.MaxWidth(r.width), GUILayout.MaxHeight((float) texture2D.height) };
          Rect aspectRect = GUILayoutUtility.GetAspectRect(num1, guiLayoutOptionArray);
          float num5 = 5f;
          aspectRect.width /= num1;
          aspectRect.width -= num5;
          aspectRect.x += num5 / 2f;
          EditorGUI.DrawPreviewTexture(aspectRect, (Texture) lightmap.lightmapColor);
          this.MenuSelectLightmapUsers(aspectRect, lightmapIndex);
          if ((bool) ((UnityEngine.Object) lightmap.lightmapDir))
          {
            aspectRect.x += aspectRect.width + num5;
            EditorGUI.DrawPreviewTexture(aspectRect, (Texture) lightmap.lightmapDir);
            this.MenuSelectLightmapUsers(aspectRect, lightmapIndex);
          }
          if ((bool) ((UnityEngine.Object) lightmap.shadowMask))
          {
            aspectRect.x += aspectRect.width + num5;
            EditorGUI.DrawPreviewTexture(aspectRect, (Texture) lightmap.shadowMask);
            this.MenuSelectLightmapUsers(aspectRect, lightmapIndex);
          }
          GUILayout.Space(10f);
          ++lightmapIndex;
        }
      }
      EditorGUILayout.EndScrollView();
    }

    public void UpdateLightmapSelection()
    {
      Terrain terrain = (Terrain) null;
      MeshRenderer component;
      if ((UnityEngine.Object) Selection.activeGameObject == (UnityEngine.Object) null || (UnityEngine.Object) (component = Selection.activeGameObject.GetComponent<MeshRenderer>()) == (UnityEngine.Object) null && (UnityEngine.Object) (terrain = Selection.activeGameObject.GetComponent<Terrain>()) == (UnityEngine.Object) null)
        this.m_SelectedLightmap = -1;
      else
        this.m_SelectedLightmap = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? terrain.lightmapIndex : component.lightmapIndex;
    }

    public void Maps()
    {
      if (LightingWindowLightmapPreviewTab.s_Styles == null)
        LightingWindowLightmapPreviewTab.s_Styles = new LightingWindowLightmapPreviewTab.Styles();
      GUI.changed = false;
      if (Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.OnDemand)
      {
        SerializedObject serializedObject = new SerializedObject(LightmapEditorSettings.GetLightmapSettings());
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LightingDataAsset"), LightingWindowLightmapPreviewTab.s_Styles.LightingDataAsset, new GUILayoutOption[0]);
        serializedObject.ApplyModifiedProperties();
      }
      GUILayout.Space(10f);
      LightmapData[] lightmaps = LightmapSettings.lightmaps;
      this.m_ScrollPositionMaps = GUILayout.BeginScrollView(this.m_ScrollPositionMaps);
      using (new EditorGUI.DisabledScope(true))
      {
        bool flag1 = false;
        bool flag2 = false;
        foreach (LightmapData lightmapData in lightmaps)
        {
          if ((UnityEngine.Object) lightmapData.lightmapDir != (UnityEngine.Object) null)
            flag1 = true;
          if ((UnityEngine.Object) lightmapData.shadowMask != (UnityEngine.Object) null)
            flag2 = true;
        }
        for (int index = 0; index < lightmaps.Length; ++index)
        {
          GUILayout.BeginHorizontal();
          GUILayout.Space(5f);
          lightmaps[index].lightmapColor = this.LightmapField(lightmaps[index].lightmapColor, index);
          if (flag1)
          {
            GUILayout.Space(10f);
            lightmaps[index].lightmapDir = this.LightmapField(lightmaps[index].lightmapDir, index);
          }
          if (flag2)
          {
            GUILayout.Space(10f);
            lightmaps[index].shadowMask = this.LightmapField(lightmaps[index].shadowMask, index);
          }
          GUILayout.Space(5f);
          GUILayout.BeginVertical();
          GUILayout.Label("Index: " + (object) index, EditorStyles.miniBoldLabel, new GUILayoutOption[0]);
          if (LightmapEditorSettings.lightmapper == LightmapEditorSettings.Lightmapper.PathTracer)
          {
            LightmapConvergence lightmapConvergence = Lightmapping.GetLightmapConvergence(index);
            if (lightmapConvergence.IsValid())
            {
              GUILayout.Label("Occupied: " + InternalEditorUtility.CountToString((ulong) lightmapConvergence.occupiedTexelCount), EditorStyles.miniLabel, new GUILayoutOption[0]);
              GUILayout.Label(EditorGUIUtility.TextContent("Direct: " + (object) lightmapConvergence.minDirectSamples + " / " + (object) lightmapConvergence.maxDirectSamples + " / " + (object) lightmapConvergence.avgDirectSamples + "|min / max / avg samples per texel"), EditorStyles.miniLabel, new GUILayoutOption[0]);
              GUILayout.Label(EditorGUIUtility.TextContent("Global Illumination: " + (object) lightmapConvergence.minGISamples + " / " + (object) lightmapConvergence.maxGISamples + " / " + (object) lightmapConvergence.avgGISamples + "|min / max / avg samples per texel"), EditorStyles.miniLabel, new GUILayoutOption[0]);
            }
            else
            {
              GUILayout.Label("Occupied: N/A", EditorStyles.miniLabel, new GUILayoutOption[0]);
              GUILayout.Label("Direct: N/A", EditorStyles.miniLabel, new GUILayoutOption[0]);
              GUILayout.Label("Global Illumination: N/A", EditorStyles.miniLabel, new GUILayoutOption[0]);
            }
            float lightmapBakePerformance = Lightmapping.GetLightmapBakePerformance(index);
            if ((double) lightmapBakePerformance >= 0.0)
              GUILayout.Label(lightmapBakePerformance.ToString("0.00") + " mrays/sec", EditorStyles.miniLabel, new GUILayoutOption[0]);
            else
              GUILayout.Label("N/A mrays/sec", EditorStyles.miniLabel, new GUILayoutOption[0]);
          }
          GUILayout.EndVertical();
          GUILayout.FlexibleSpace();
          GUILayout.EndHorizontal();
        }
      }
      GUILayout.EndScrollView();
    }

    private Texture2D LightmapField(Texture2D lightmap, int index)
    {
      Rect rect = GUILayoutUtility.GetRect(100f, 100f, EditorStyles.objectField);
      this.MenuSelectLightmapUsers(rect, index);
      Texture2D texture2D = EditorGUI.ObjectField(rect, (UnityEngine.Object) lightmap, typeof (Texture2D), false) as Texture2D;
      if (index == this.m_SelectedLightmap && Event.current.type == EventType.Repaint)
        LightingWindowLightmapPreviewTab.s_Styles.selectedLightmapHighlight.Draw(rect, false, false, false, false);
      return texture2D;
    }

    private class Styles
    {
      public GUIStyle selectedLightmapHighlight = (GUIStyle) "LightmapEditorSelectedHighlight";
      public GUIContent LightProbes = EditorGUIUtility.TextContent("Light Probes|A different LightProbes.asset can be assigned here. These assets are generated by baking a scene containing light probes.");
      public GUIContent LightingDataAsset = EditorGUIUtility.TextContent("Lighting Data Asset|A different LightingData.asset can be assigned here. These assets are generated by baking a scene in the OnDemand mode.");
      public GUIContent MapsArraySize = EditorGUIUtility.TextContent("Array Size|The length of the array of lightmaps.");
    }
  }
}
