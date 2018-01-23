// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightTableColumns
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class LightTableColumns
  {
    private static ColorPickerHDRConfig s_ColorPickerHDRConfig = new ColorPickerHDRConfig(0.0f, 65536f, 1.525879E-05f, 3f);
    private const float kMaxfp16 = 65536f;

    private static SerializedPropertyTreeView.Column[] FinalizeColumns(SerializedPropertyTreeView.Column[] columns, out string[] propNames)
    {
      propNames = new string[columns.Length];
      for (int index = 0; index < columns.Length; ++index)
        propNames[index] = columns[index].propertyName;
      return columns;
    }

    private static bool IsEditable(Object target)
    {
      return (target.hideFlags & HideFlags.NotEditable) == HideFlags.None;
    }

    public static SerializedPropertyTreeView.Column[] CreateLightColumns(out string[] propNames)
    {
      SerializedPropertyTreeView.Column[] columns = new SerializedPropertyTreeView.Column[8];
      int index1 = 0;
      SerializedPropertyTreeView.Column column1 = new SerializedPropertyTreeView.Column();
      column1.headerContent = LightTableColumns.Styles.Name;
      column1.headerTextAlignment = TextAlignment.Left;
      column1.sortedAscending = true;
      column1.sortingArrowAlignment = TextAlignment.Center;
      column1.width = 200f;
      column1.minWidth = 100f;
      column1.autoResize = false;
      column1.allowToggleVisibility = true;
      column1.propertyName = (string) null;
      column1.dependencyIndices = (int[]) null;
      column1.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareName;
      column1.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawName;
      column1.filter = (SerializedPropertyFilters.IFilter) new SerializedPropertyFilters.Name();
      SerializedPropertyTreeView.Column column2 = column1;
      columns[index1] = column2;
      int index2 = 1;
      SerializedPropertyTreeView.Column column3 = new SerializedPropertyTreeView.Column();
      column3.headerContent = LightTableColumns.Styles.On;
      column3.headerTextAlignment = TextAlignment.Center;
      column3.sortedAscending = true;
      column3.sortingArrowAlignment = TextAlignment.Center;
      column3.width = 25f;
      column3.minWidth = 25f;
      column3.maxWidth = 25f;
      column3.autoResize = false;
      column3.allowToggleVisibility = true;
      column3.propertyName = "m_Enabled";
      column3.dependencyIndices = (int[]) null;
      column3.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareCheckbox;
      column3.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawCheckbox;
      SerializedPropertyTreeView.Column column4 = column3;
      columns[index2] = column4;
      int index3 = 2;
      SerializedPropertyTreeView.Column column5 = new SerializedPropertyTreeView.Column();
      column5.headerContent = LightTableColumns.Styles.Type;
      column5.headerTextAlignment = TextAlignment.Left;
      column5.sortedAscending = true;
      column5.sortingArrowAlignment = TextAlignment.Center;
      column5.width = 120f;
      column5.minWidth = 60f;
      column5.autoResize = false;
      column5.allowToggleVisibility = true;
      column5.propertyName = "m_Type";
      column5.dependencyIndices = (int[]) null;
      column5.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareEnum;
      column5.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawDefault;
      SerializedPropertyTreeView.Column column6 = column5;
      columns[index3] = column6;
      int index4 = 3;
      SerializedPropertyTreeView.Column column7 = new SerializedPropertyTreeView.Column();
      column7.headerContent = LightTableColumns.Styles.Mode;
      column7.headerTextAlignment = TextAlignment.Left;
      column7.sortedAscending = true;
      column7.sortingArrowAlignment = TextAlignment.Center;
      column7.width = 70f;
      column7.minWidth = 40f;
      column7.maxWidth = 70f;
      column7.autoResize = false;
      column7.allowToggleVisibility = true;
      column7.propertyName = "m_Lightmapping";
      column7.dependencyIndices = new int[1]{ 2 };
      column7.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareEnum;
      column7.drawDelegate = (SerializedPropertyTreeView.Column.DrawEntry) ((r, prop, dep) =>
      {
        using (new EditorGUI.DisabledScope(dep.Length > 1 && dep[0].enumValueIndex == 3))
        {
          EditorGUI.BeginChangeCheck();
          int num = EditorGUI.IntPopup(r, prop.intValue, LightTableColumns.Styles.LightmapBakeTypeTitles, LightTableColumns.Styles.LightmapBakeTypeValues);
          if (!EditorGUI.EndChangeCheck())
            return;
          prop.intValue = num;
        }
      });
      SerializedPropertyTreeView.Column column8 = column7;
      columns[index4] = column8;
      int index5 = 4;
      SerializedPropertyTreeView.Column column9 = new SerializedPropertyTreeView.Column();
      column9.headerContent = LightTableColumns.Styles.Color;
      column9.headerTextAlignment = TextAlignment.Left;
      column9.sortedAscending = true;
      column9.sortingArrowAlignment = TextAlignment.Center;
      column9.width = 70f;
      column9.minWidth = 40f;
      column9.autoResize = false;
      column9.allowToggleVisibility = true;
      column9.propertyName = "m_Color";
      column9.dependencyIndices = (int[]) null;
      column9.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareColor;
      column9.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawDefault;
      SerializedPropertyTreeView.Column column10 = column9;
      columns[index5] = column10;
      int index6 = 5;
      SerializedPropertyTreeView.Column column11 = new SerializedPropertyTreeView.Column();
      column11.headerContent = LightTableColumns.Styles.Intensity;
      column11.headerTextAlignment = TextAlignment.Left;
      column11.sortedAscending = true;
      column11.sortingArrowAlignment = TextAlignment.Center;
      column11.width = 60f;
      column11.minWidth = 30f;
      column11.autoResize = false;
      column11.allowToggleVisibility = true;
      column11.propertyName = "m_Intensity";
      column11.dependencyIndices = (int[]) null;
      column11.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareFloat;
      column11.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawDefault;
      SerializedPropertyTreeView.Column column12 = column11;
      columns[index6] = column12;
      int index7 = 6;
      SerializedPropertyTreeView.Column column13 = new SerializedPropertyTreeView.Column();
      column13.headerContent = LightTableColumns.Styles.IndirectMultiplier;
      column13.headerTextAlignment = TextAlignment.Left;
      column13.sortedAscending = true;
      column13.sortingArrowAlignment = TextAlignment.Center;
      column13.width = 110f;
      column13.minWidth = 60f;
      column13.autoResize = false;
      column13.allowToggleVisibility = true;
      column13.propertyName = "m_BounceIntensity";
      column13.dependencyIndices = (int[]) null;
      column13.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareFloat;
      column13.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawDefault;
      SerializedPropertyTreeView.Column column14 = column13;
      columns[index7] = column14;
      int index8 = 7;
      SerializedPropertyTreeView.Column column15 = new SerializedPropertyTreeView.Column();
      column15.headerContent = LightTableColumns.Styles.ShadowType;
      column15.headerTextAlignment = TextAlignment.Left;
      column15.sortedAscending = true;
      column15.sortingArrowAlignment = TextAlignment.Center;
      column15.width = 100f;
      column15.minWidth = 60f;
      column15.autoResize = false;
      column15.allowToggleVisibility = true;
      column15.propertyName = "m_Shadows.m_Type";
      column15.dependencyIndices = (int[]) null;
      column15.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareEnum;
      column15.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawDefault;
      SerializedPropertyTreeView.Column column16 = column15;
      columns[index8] = column16;
      return LightTableColumns.FinalizeColumns(columns, out propNames);
    }

    public static SerializedPropertyTreeView.Column[] CreateReflectionColumns(out string[] propNames)
    {
      SerializedPropertyTreeView.Column[] columns = new SerializedPropertyTreeView.Column[8];
      int index1 = 0;
      SerializedPropertyTreeView.Column column1 = new SerializedPropertyTreeView.Column();
      column1.headerContent = LightTableColumns.Styles.Name;
      column1.headerTextAlignment = TextAlignment.Left;
      column1.sortedAscending = true;
      column1.sortingArrowAlignment = TextAlignment.Center;
      column1.width = 200f;
      column1.minWidth = 100f;
      column1.autoResize = false;
      column1.allowToggleVisibility = true;
      column1.propertyName = (string) null;
      column1.dependencyIndices = (int[]) null;
      column1.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareName;
      column1.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawName;
      column1.filter = (SerializedPropertyFilters.IFilter) new SerializedPropertyFilters.Name();
      SerializedPropertyTreeView.Column column2 = column1;
      columns[index1] = column2;
      int index2 = 1;
      SerializedPropertyTreeView.Column column3 = new SerializedPropertyTreeView.Column();
      column3.headerContent = LightTableColumns.Styles.On;
      column3.headerTextAlignment = TextAlignment.Center;
      column3.sortedAscending = true;
      column3.sortingArrowAlignment = TextAlignment.Center;
      column3.width = 25f;
      column3.minWidth = 25f;
      column3.maxWidth = 25f;
      column3.autoResize = false;
      column3.allowToggleVisibility = true;
      column3.propertyName = "m_Enabled";
      column3.dependencyIndices = (int[]) null;
      column3.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareCheckbox;
      column3.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawCheckbox;
      SerializedPropertyTreeView.Column column4 = column3;
      columns[index2] = column4;
      int index3 = 2;
      SerializedPropertyTreeView.Column column5 = new SerializedPropertyTreeView.Column();
      column5.headerContent = LightTableColumns.Styles.Mode;
      column5.headerTextAlignment = TextAlignment.Left;
      column5.sortedAscending = true;
      column5.sortingArrowAlignment = TextAlignment.Center;
      column5.width = 70f;
      column5.minWidth = 40f;
      column5.autoResize = false;
      column5.allowToggleVisibility = true;
      column5.propertyName = "m_Mode";
      column5.dependencyIndices = (int[]) null;
      column5.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareInt;
      column5.drawDelegate = (SerializedPropertyTreeView.Column.DrawEntry) ((r, prop, dep) => EditorGUI.IntPopup(r, prop, ReflectionProbeEditor.Styles.reflectionProbeMode, ReflectionProbeEditor.Styles.reflectionProbeModeValues, GUIContent.none));
      SerializedPropertyTreeView.Column column6 = column5;
      columns[index3] = column6;
      int index4 = 3;
      SerializedPropertyTreeView.Column column7 = new SerializedPropertyTreeView.Column();
      column7.headerContent = LightTableColumns.Styles.Projection;
      column7.headerTextAlignment = TextAlignment.Left;
      column7.sortedAscending = true;
      column7.sortingArrowAlignment = TextAlignment.Center;
      column7.width = 80f;
      column7.minWidth = 40f;
      column7.autoResize = false;
      column7.allowToggleVisibility = true;
      column7.propertyName = "m_BoxProjection";
      column7.dependencyIndices = (int[]) null;
      column7.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareCheckbox;
      column7.drawDelegate = (SerializedPropertyTreeView.Column.DrawEntry) ((r, prop, dep) =>
      {
        int[] optionValues = new int[2]{ 0, 1 };
        prop.boolValue = EditorGUI.IntPopup(r, !prop.boolValue ? 0 : 1, LightTableColumns.Styles.ProjectionStrings, optionValues) == 1;
      });
      SerializedPropertyTreeView.Column column8 = column7;
      columns[index4] = column8;
      int index5 = 4;
      SerializedPropertyTreeView.Column column9 = new SerializedPropertyTreeView.Column();
      column9.headerContent = LightTableColumns.Styles.HDR;
      column9.headerTextAlignment = TextAlignment.Center;
      column9.sortedAscending = true;
      column9.sortingArrowAlignment = TextAlignment.Center;
      column9.width = 35f;
      column9.minWidth = 35f;
      column9.maxWidth = 35f;
      column9.autoResize = false;
      column9.allowToggleVisibility = true;
      column9.propertyName = "m_HDR";
      column9.dependencyIndices = (int[]) null;
      column9.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareCheckbox;
      column9.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawCheckbox;
      SerializedPropertyTreeView.Column column10 = column9;
      columns[index5] = column10;
      int index6 = 5;
      SerializedPropertyTreeView.Column column11 = new SerializedPropertyTreeView.Column();
      column11.headerContent = LightTableColumns.Styles.ShadowDistance;
      column11.headerTextAlignment = TextAlignment.Left;
      column11.sortedAscending = true;
      column11.sortingArrowAlignment = TextAlignment.Center;
      column11.width = 110f;
      column11.minWidth = 50f;
      column11.autoResize = false;
      column11.allowToggleVisibility = true;
      column11.propertyName = "m_ShadowDistance";
      column11.dependencyIndices = (int[]) null;
      column11.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareFloat;
      column11.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawDefault;
      SerializedPropertyTreeView.Column column12 = column11;
      columns[index6] = column12;
      int index7 = 6;
      SerializedPropertyTreeView.Column column13 = new SerializedPropertyTreeView.Column();
      column13.headerContent = LightTableColumns.Styles.NearPlane;
      column13.headerTextAlignment = TextAlignment.Left;
      column13.sortedAscending = true;
      column13.sortingArrowAlignment = TextAlignment.Center;
      column13.width = 70f;
      column13.minWidth = 30f;
      column13.autoResize = false;
      column13.allowToggleVisibility = true;
      column13.propertyName = "m_NearClip";
      column13.dependencyIndices = (int[]) null;
      column13.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareFloat;
      column13.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawDefault;
      SerializedPropertyTreeView.Column column14 = column13;
      columns[index7] = column14;
      int index8 = 7;
      SerializedPropertyTreeView.Column column15 = new SerializedPropertyTreeView.Column();
      column15.headerContent = LightTableColumns.Styles.FarPlane;
      column15.headerTextAlignment = TextAlignment.Left;
      column15.sortedAscending = true;
      column15.sortingArrowAlignment = TextAlignment.Center;
      column15.width = 70f;
      column15.minWidth = 30f;
      column15.autoResize = false;
      column15.allowToggleVisibility = true;
      column15.propertyName = "m_FarClip";
      column15.dependencyIndices = (int[]) null;
      column15.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareFloat;
      column15.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawDefault;
      SerializedPropertyTreeView.Column column16 = column15;
      columns[index8] = column16;
      return LightTableColumns.FinalizeColumns(columns, out propNames);
    }

    public static SerializedPropertyTreeView.Column[] CreateLightProbeColumns(out string[] propNames)
    {
      SerializedPropertyTreeView.Column[] columns = new SerializedPropertyTreeView.Column[2];
      int index1 = 0;
      SerializedPropertyTreeView.Column column1 = new SerializedPropertyTreeView.Column();
      column1.headerContent = LightTableColumns.Styles.Name;
      column1.headerTextAlignment = TextAlignment.Left;
      column1.sortedAscending = true;
      column1.sortingArrowAlignment = TextAlignment.Center;
      column1.width = 200f;
      column1.minWidth = 100f;
      column1.autoResize = false;
      column1.allowToggleVisibility = true;
      column1.propertyName = (string) null;
      column1.dependencyIndices = (int[]) null;
      column1.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareName;
      column1.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawName;
      column1.filter = (SerializedPropertyFilters.IFilter) new SerializedPropertyFilters.Name();
      SerializedPropertyTreeView.Column column2 = column1;
      columns[index1] = column2;
      int index2 = 1;
      SerializedPropertyTreeView.Column column3 = new SerializedPropertyTreeView.Column();
      column3.headerContent = LightTableColumns.Styles.On;
      column3.headerTextAlignment = TextAlignment.Center;
      column3.sortedAscending = true;
      column3.sortingArrowAlignment = TextAlignment.Center;
      column3.width = 25f;
      column3.minWidth = 25f;
      column3.maxWidth = 25f;
      column3.autoResize = false;
      column3.allowToggleVisibility = true;
      column3.propertyName = "m_Enabled";
      column3.dependencyIndices = (int[]) null;
      column3.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareCheckbox;
      column3.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawCheckbox;
      SerializedPropertyTreeView.Column column4 = column3;
      columns[index2] = column4;
      return LightTableColumns.FinalizeColumns(columns, out propNames);
    }

    public static SerializedPropertyTreeView.Column[] CreateEmissivesColumns(out string[] propNames)
    {
      SerializedPropertyTreeView.Column[] columns = new SerializedPropertyTreeView.Column[4];
      int index1 = 0;
      SerializedPropertyTreeView.Column column1 = new SerializedPropertyTreeView.Column();
      column1.headerContent = LightTableColumns.Styles.SelectObjects;
      column1.headerTextAlignment = TextAlignment.Left;
      column1.sortedAscending = true;
      column1.sortingArrowAlignment = TextAlignment.Center;
      column1.width = 20f;
      column1.minWidth = 20f;
      column1.maxWidth = 20f;
      column1.autoResize = false;
      column1.allowToggleVisibility = true;
      column1.propertyName = "m_LightmapFlags";
      column1.dependencyIndices = (int[]) null;
      column1.compareDelegate = (SerializedPropertyTreeView.Column.CompareEntry) null;
      column1.drawDelegate = (SerializedPropertyTreeView.Column.DrawEntry) ((r, prop, dep) =>
      {
        if (!GUI.Button(r, LightTableColumns.Styles.SelectObjectsButton, (GUIStyle) "label"))
          return;
        SearchableEditorWindow.SearchForReferencesToInstanceID(prop.serializedObject.targetObject.GetInstanceID());
      });
      SerializedPropertyTreeView.Column column2 = column1;
      columns[index1] = column2;
      int index2 = 1;
      SerializedPropertyTreeView.Column column3 = new SerializedPropertyTreeView.Column();
      column3.headerContent = LightTableColumns.Styles.Name;
      column3.headerTextAlignment = TextAlignment.Left;
      column3.sortedAscending = true;
      column3.sortingArrowAlignment = TextAlignment.Center;
      column3.width = 200f;
      column3.minWidth = 100f;
      column3.autoResize = false;
      column3.allowToggleVisibility = true;
      column3.propertyName = (string) null;
      column3.dependencyIndices = (int[]) null;
      column3.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareName;
      column3.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.s_DrawName;
      column3.filter = (SerializedPropertyFilters.IFilter) new SerializedPropertyFilters.Name();
      SerializedPropertyTreeView.Column column4 = column3;
      columns[index2] = column4;
      int index3 = 2;
      SerializedPropertyTreeView.Column column5 = new SerializedPropertyTreeView.Column();
      column5.headerContent = LightTableColumns.Styles.GlobalIllumination;
      column5.headerTextAlignment = TextAlignment.Left;
      column5.sortedAscending = true;
      column5.sortingArrowAlignment = TextAlignment.Center;
      column5.width = 120f;
      column5.minWidth = 70f;
      column5.autoResize = false;
      column5.allowToggleVisibility = true;
      column5.propertyName = "m_LightmapFlags";
      column5.dependencyIndices = (int[]) null;
      column5.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.s_CompareInt;
      column5.drawDelegate = (SerializedPropertyTreeView.Column.DrawEntry) ((r, prop, dep) =>
      {
        if (!prop.serializedObject.targetObject.GetType().Equals(typeof (Material)))
          return;
        using (new EditorGUI.DisabledScope(!LightTableColumns.IsEditable(prop.serializedObject.targetObject)))
        {
          MaterialGlobalIlluminationFlags illuminationFlags1 = (prop.intValue & 2) == 0 ? MaterialGlobalIlluminationFlags.RealtimeEmissive : MaterialGlobalIlluminationFlags.BakedEmissive;
          int[] optionValues = new int[2]{ 1, 2 };
          EditorGUI.BeginChangeCheck();
          MaterialGlobalIlluminationFlags illuminationFlags2 = (MaterialGlobalIlluminationFlags) EditorGUI.IntPopup(r, (int) illuminationFlags1, LightTableColumns.Styles.LightmapEmissiveStrings, optionValues);
          if (!EditorGUI.EndChangeCheck())
            return;
          Material targetObject = (Material) prop.serializedObject.targetObject;
          Undo.RecordObjects((Object[]) new Material[1]
          {
            targetObject
          }, "Modify GI Settings of " + targetObject.name);
          targetObject.globalIlluminationFlags = illuminationFlags2;
          prop.serializedObject.Update();
        }
      });
      SerializedPropertyTreeView.Column column6 = column5;
      columns[index3] = column6;
      int index4 = 3;
      SerializedPropertyTreeView.Column column7 = new SerializedPropertyTreeView.Column();
      column7.headerContent = LightTableColumns.Styles.Intensity;
      column7.headerTextAlignment = TextAlignment.Left;
      column7.sortedAscending = true;
      column7.sortingArrowAlignment = TextAlignment.Center;
      column7.width = 70f;
      column7.minWidth = 40f;
      column7.autoResize = false;
      column7.allowToggleVisibility = true;
      column7.propertyName = "m_Shader";
      column7.dependencyIndices = (int[]) null;
      column7.compareDelegate = (SerializedPropertyTreeView.Column.CompareEntry) ((lhs, rhs) =>
      {
        float H1;
        float S1;
        float V1;
        Color.RGBToHSV(((Material) lhs.serializedObject.targetObject).GetColor("_EmissionColor"), out H1, out S1, out V1);
        float H2;
        float S2;
        float V2;
        Color.RGBToHSV(((Material) rhs.serializedObject.targetObject).GetColor("_EmissionColor"), out H2, out S2, out V2);
        return V1.CompareTo(V2);
      });
      column7.drawDelegate = (SerializedPropertyTreeView.Column.DrawEntry) ((r, prop, dep) =>
      {
        if (!prop.serializedObject.targetObject.GetType().Equals(typeof (Material)))
          return;
        using (new EditorGUI.DisabledScope(!LightTableColumns.IsEditable(prop.serializedObject.targetObject)))
        {
          Material targetObject = (Material) prop.serializedObject.targetObject;
          Color color1 = targetObject.GetColor("_EmissionColor");
          ColorPickerHDRConfig colorPickerHdrConfig = LightTableColumns.s_ColorPickerHDRConfig ?? ColorPicker.defaultHDRConfig;
          EditorGUI.BeginChangeCheck();
          Color color2 = EditorGUI.ColorBrightnessField(r, GUIContent.Temp(""), color1, colorPickerHdrConfig.minBrightness, colorPickerHdrConfig.maxBrightness);
          if (!EditorGUI.EndChangeCheck())
            return;
          Undo.RecordObjects((Object[]) new Material[1]
          {
            targetObject
          }, "Modify Color of " + targetObject.name);
          targetObject.SetColor("_EmissionColor", color2);
        }
      });
      column7.copyDelegate = (SerializedPropertyTreeView.Column.CopyDelegate) ((target, source) =>
      {
        Color color = ((Material) source.serializedObject.targetObject).GetColor("_EmissionColor");
        ((Material) target.serializedObject.targetObject).SetColor("_EmissionColor", color);
      });
      SerializedPropertyTreeView.Column column8 = column7;
      columns[index4] = column8;
      return LightTableColumns.FinalizeColumns(columns, out propNames);
    }

    private static class Styles
    {
      public static readonly GUIContent[] ProjectionStrings = new GUIContent[2]{ EditorGUIUtility.TextContent("Infinite"), EditorGUIUtility.TextContent("Box") };
      public static readonly GUIContent[] LightmapEmissiveStrings = new GUIContent[2]{ EditorGUIUtility.TextContent("Realtime"), EditorGUIUtility.TextContent("Baked") };
      public static readonly GUIContent Name = EditorGUIUtility.TextContent(nameof (Name));
      public static readonly GUIContent On = EditorGUIUtility.TextContent(nameof (On));
      public static readonly GUIContent Type = EditorGUIUtility.TextContent(nameof (Type));
      public static readonly GUIContent Mode = EditorGUIUtility.TextContent(nameof (Mode));
      public static readonly GUIContent Color = EditorGUIUtility.TextContent(nameof (Color));
      public static readonly GUIContent Intensity = EditorGUIUtility.TextContent(nameof (Intensity));
      public static readonly GUIContent IndirectMultiplier = EditorGUIUtility.TextContent("Indirect Multiplier");
      public static readonly GUIContent ShadowType = EditorGUIUtility.TextContent("Shadow Type");
      public static readonly GUIContent Projection = EditorGUIUtility.TextContent(nameof (Projection));
      public static readonly GUIContent HDR = EditorGUIUtility.TextContent(nameof (HDR));
      public static readonly GUIContent ShadowDistance = EditorGUIUtility.TextContent("Shadow Distance");
      public static readonly GUIContent NearPlane = EditorGUIUtility.TextContent("Near Plane");
      public static readonly GUIContent FarPlane = EditorGUIUtility.TextContent("Far Plane");
      public static readonly GUIContent GlobalIllumination = EditorGUIUtility.TextContent("Global Illumination");
      public static readonly GUIContent SelectObjects = EditorGUIUtility.TextContent("");
      public static readonly GUIContent SelectObjectsButton = EditorGUIUtility.TextContentWithIcon("|Find References in Scene", "UnityEditor.FindDependencies");
      public static readonly GUIContent[] LightmapBakeTypeTitles = new GUIContent[3]{ new GUIContent("Realtime"), new GUIContent("Mixed"), new GUIContent("Baked") };
      public static readonly int[] LightmapBakeTypeValues = new int[3]{ 4, 1, 2 };
    }
  }
}
