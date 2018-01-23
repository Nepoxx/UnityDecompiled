// Decompiled with JetBrains decompiler
// Type: UnityEditor.GradientEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class GradientEditor
  {
    private static GradientEditor.Styles s_Styles;
    private static Texture2D s_BackgroundTexture;
    private const int k_MaxNumKeys = 8;
    private List<GradientEditor.Swatch> m_RGBSwatches;
    private List<GradientEditor.Swatch> m_AlphaSwatches;
    private GradientMode m_GradientMode;
    [NonSerialized]
    private GradientEditor.Swatch m_SelectedSwatch;
    private Gradient m_Gradient;
    private int m_NumSteps;
    private bool m_HDR;

    public void Init(Gradient gradient, int numSteps, bool hdr)
    {
      this.m_Gradient = gradient;
      this.m_NumSteps = numSteps;
      this.m_HDR = hdr;
      this.BuildArrays();
      if (this.m_RGBSwatches.Count <= 0)
        return;
      this.m_SelectedSwatch = this.m_RGBSwatches[0];
    }

    public Gradient target
    {
      get
      {
        return this.m_Gradient;
      }
    }

    private float GetTime(float actualTime)
    {
      actualTime = Mathf.Clamp01(actualTime);
      if (this.m_NumSteps <= 1)
        return actualTime;
      float num = 1f / (float) (this.m_NumSteps - 1);
      return (float) Mathf.RoundToInt(actualTime / num) / (float) (this.m_NumSteps - 1);
    }

    private void BuildArrays()
    {
      if (this.m_Gradient == null)
        return;
      GradientColorKey[] colorKeys = this.m_Gradient.colorKeys;
      this.m_RGBSwatches = new List<GradientEditor.Swatch>(colorKeys.Length);
      for (int index = 0; index < colorKeys.Length; ++index)
      {
        Color color = colorKeys[index].color;
        color.a = 1f;
        this.m_RGBSwatches.Add(new GradientEditor.Swatch(colorKeys[index].time, color, false));
      }
      GradientAlphaKey[] alphaKeys = this.m_Gradient.alphaKeys;
      this.m_AlphaSwatches = new List<GradientEditor.Swatch>(alphaKeys.Length);
      for (int index = 0; index < alphaKeys.Length; ++index)
      {
        float alpha = alphaKeys[index].alpha;
        this.m_AlphaSwatches.Add(new GradientEditor.Swatch(alphaKeys[index].time, new Color(alpha, alpha, alpha, 1f), true));
      }
      this.m_GradientMode = this.m_Gradient.mode;
    }

    public static void DrawGradientWithBackground(Rect position, Gradient gradient)
    {
      Texture2D gradientPreview = GradientPreviewCache.GetGradientPreview(gradient);
      Rect position1 = new Rect(position.x + 1f, position.y + 1f, position.width - 2f, position.height - 2f);
      Texture2D backgroundTexture = GradientEditor.GetBackgroundTexture();
      Rect texCoords = new Rect(0.0f, 0.0f, position1.width / (float) backgroundTexture.width, position1.height / (float) backgroundTexture.height);
      GUI.DrawTextureWithTexCoords(position1, (Texture) backgroundTexture, texCoords, false);
      if ((UnityEngine.Object) gradientPreview != (UnityEngine.Object) null)
        GUI.DrawTexture(position1, (Texture) gradientPreview, ScaleMode.StretchToFill, true);
      GUI.Label(position, GUIContent.none, EditorStyles.colorPickerBox);
      if ((double) GradientEditor.GetMaxColorComponent(gradient) <= 1.0)
        return;
      GUI.Label(new Rect(position.x, position.y, position.width - 3f, position.height), "HDR", EditorStyles.centeredGreyMiniLabel);
    }

    public void OnGUI(Rect position)
    {
      if (GradientEditor.s_Styles == null)
        GradientEditor.s_Styles = new GradientEditor.Styles();
      float num1 = 24f;
      float num2 = 16f;
      float num3 = 26f;
      float num4 = position.height - 2f * num2 - num3 - num1;
      position.height = num1;
      this.m_GradientMode = (GradientMode) EditorGUI.EnumPopup(position, GradientEditor.s_Styles.modeText, (Enum) this.m_GradientMode);
      if (this.m_GradientMode != this.m_Gradient.mode)
        this.AssignBack();
      position.y += num1;
      position.height = num2;
      this.ShowSwatchArray(position, this.m_AlphaSwatches, true);
      position.y += num2;
      if (Event.current.type == EventType.Repaint)
      {
        position.height = num4;
        GradientEditor.DrawGradientWithBackground(position, this.m_Gradient);
      }
      position.y += num4;
      position.height = num2;
      this.ShowSwatchArray(position, this.m_RGBSwatches, false);
      if (this.m_SelectedSwatch == null)
        return;
      position.y += num2;
      position.height = num3;
      position.y += 10f;
      float num5 = 45f;
      float num6 = 60f;
      float num7 = 20f;
      float num8 = 50f;
      float num9 = num6 + num7 + num6 + num5;
      Rect position1 = position;
      position1.height = 18f;
      position1.x += 17f;
      position1.width -= num9;
      EditorGUIUtility.labelWidth = num8;
      if (this.m_SelectedSwatch.m_IsAlpha)
      {
        EditorGUIUtility.fieldWidth = 30f;
        EditorGUI.BeginChangeCheck();
        float num10 = (float) EditorGUI.IntSlider(position1, GradientEditor.s_Styles.alphaText, (int) ((double) this.m_SelectedSwatch.m_Value.r * (double) byte.MaxValue), 0, (int) byte.MaxValue) / (float) byte.MaxValue;
        if (EditorGUI.EndChangeCheck())
        {
          this.m_SelectedSwatch.m_Value.r = this.m_SelectedSwatch.m_Value.g = this.m_SelectedSwatch.m_Value.b = Mathf.Clamp01(num10);
          this.AssignBack();
          HandleUtility.Repaint();
        }
      }
      else
      {
        EditorGUI.BeginChangeCheck();
        this.m_SelectedSwatch.m_Value = EditorGUI.ColorField(position1, GradientEditor.s_Styles.colorText, this.m_SelectedSwatch.m_Value, true, false, this.m_HDR, ColorPicker.defaultHDRConfig);
        if (EditorGUI.EndChangeCheck())
        {
          this.AssignBack();
          HandleUtility.Repaint();
        }
      }
      position1.x += position1.width + num7;
      position1.width = num5 + num6;
      EditorGUIUtility.labelWidth = num6;
      string fieldFormatString = EditorGUI.kFloatFieldFormatString;
      EditorGUI.kFloatFieldFormatString = "f1";
      EditorGUI.BeginChangeCheck();
      float num11 = EditorGUI.FloatField(position1, GradientEditor.s_Styles.locationText, this.m_SelectedSwatch.m_Time * 100f) / 100f;
      if (EditorGUI.EndChangeCheck())
      {
        this.m_SelectedSwatch.m_Time = Mathf.Clamp(num11, 0.0f, 1f);
        this.AssignBack();
      }
      EditorGUI.kFloatFieldFormatString = fieldFormatString;
      position1.x += position1.width;
      position1.width = 20f;
      GUI.Label(position1, GradientEditor.s_Styles.percentText);
    }

    private void ShowSwatchArray(Rect position, List<GradientEditor.Swatch> swatches, bool isAlpha)
    {
      int controlId = GUIUtility.GetControlID(652347689, FocusType.Passive);
      Event current = Event.current;
      float time = this.GetTime((Event.current.mousePosition.x - position.x) / position.width);
      Vector2 point = (Vector2) new Vector3(position.x + time * position.width, Event.current.mousePosition.y);
      EventType typeForControl = current.GetTypeForControl(controlId);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          Rect rect = position;
          rect.xMin -= 10f;
          rect.xMax += 10f;
          if (!rect.Contains(current.mousePosition))
            break;
          GUIUtility.hotControl = controlId;
          current.Use();
          if (swatches.Contains(this.m_SelectedSwatch) && !this.m_SelectedSwatch.m_IsAlpha && this.CalcSwatchRect(position, this.m_SelectedSwatch).Contains(current.mousePosition))
          {
            if (current.clickCount != 2)
              break;
            GUIUtility.keyboardControl = controlId;
            ColorPicker.Show(GUIView.current, this.m_SelectedSwatch.m_Value, false, this.m_HDR, ColorPicker.defaultHDRConfig);
            GUIUtility.ExitGUI();
            break;
          }
          bool flag1 = false;
          foreach (GradientEditor.Swatch swatch in swatches)
          {
            if (this.CalcSwatchRect(position, swatch).Contains(point))
            {
              flag1 = true;
              this.m_SelectedSwatch = swatch;
              break;
            }
          }
          if (!flag1)
          {
            if (swatches.Count < 8)
            {
              Color color = this.m_Gradient.Evaluate(time);
              if (isAlpha)
                color = new Color(color.a, color.a, color.a, 1f);
              else
                color.a = 1f;
              this.m_SelectedSwatch = new GradientEditor.Swatch(time, color, isAlpha);
              swatches.Add(this.m_SelectedSwatch);
              this.AssignBack();
            }
            else
              Debug.LogWarning((object) ("Max " + (object) 8 + " color keys and " + (object) 8 + " alpha keys are allowed in a gradient."));
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          GUIUtility.hotControl = 0;
          current.Use();
          if (!swatches.Contains(this.m_SelectedSwatch))
            this.m_SelectedSwatch = (GradientEditor.Swatch) null;
          this.RemoveDuplicateOverlappingSwatches();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != controlId || this.m_SelectedSwatch == null)
            break;
          current.Use();
          if ((double) current.mousePosition.y + 5.0 < (double) position.y || (double) current.mousePosition.y - 5.0 > (double) position.yMax)
          {
            if (swatches.Count > 1)
            {
              swatches.Remove(this.m_SelectedSwatch);
              this.AssignBack();
              break;
            }
          }
          else if (!swatches.Contains(this.m_SelectedSwatch))
            swatches.Add(this.m_SelectedSwatch);
          this.m_SelectedSwatch.m_Time = time;
          this.AssignBack();
          break;
        case EventType.KeyDown:
          if (current.keyCode != KeyCode.Delete)
            break;
          if (this.m_SelectedSwatch != null)
          {
            List<GradientEditor.Swatch> swatchList = !this.m_SelectedSwatch.m_IsAlpha ? this.m_RGBSwatches : this.m_AlphaSwatches;
            if (swatchList.Count > 1)
            {
              swatchList.Remove(this.m_SelectedSwatch);
              this.AssignBack();
              HandleUtility.Repaint();
            }
          }
          current.Use();
          break;
        case EventType.Repaint:
          bool flag2 = false;
          foreach (GradientEditor.Swatch swatch in swatches)
          {
            if (this.m_SelectedSwatch == swatch)
              flag2 = true;
            else
              this.DrawSwatch(position, swatch, !isAlpha);
          }
          if (!flag2 || this.m_SelectedSwatch == null)
            break;
          this.DrawSwatch(position, this.m_SelectedSwatch, !isAlpha);
          break;
        default:
          if (typeForControl != EventType.ValidateCommand)
          {
            if (typeForControl != EventType.ExecuteCommand)
              break;
            if (current.commandName == "ColorPickerChanged")
            {
              GUI.changed = true;
              this.m_SelectedSwatch.m_Value = ColorPicker.color;
              this.AssignBack();
              HandleUtility.Repaint();
              break;
            }
            if (!(current.commandName == "Delete") || swatches.Count <= 1)
              break;
            swatches.Remove(this.m_SelectedSwatch);
            this.AssignBack();
            HandleUtility.Repaint();
            break;
          }
          if (!(current.commandName == "Delete"))
            break;
          Event.current.Use();
          break;
      }
    }

    private void DrawSwatch(Rect totalPos, GradientEditor.Swatch s, bool upwards)
    {
      Color backgroundColor = GUI.backgroundColor;
      Rect position = this.CalcSwatchRect(totalPos, s);
      GUI.backgroundColor = s.m_Value;
      GUIStyle guiStyle1 = !upwards ? GradientEditor.s_Styles.downSwatch : GradientEditor.s_Styles.upSwatch;
      GUIStyle guiStyle2 = !upwards ? GradientEditor.s_Styles.downSwatchOverlay : GradientEditor.s_Styles.upSwatchOverlay;
      guiStyle1.Draw(position, false, false, this.m_SelectedSwatch == s, false);
      GUI.backgroundColor = backgroundColor;
      guiStyle2.Draw(position, false, false, this.m_SelectedSwatch == s, false);
    }

    private Rect CalcSwatchRect(Rect totalRect, GradientEditor.Swatch s)
    {
      float time = s.m_Time;
      return new Rect((float) ((double) totalRect.x + (double) Mathf.Round(totalRect.width * time) - 5.0), totalRect.y, 10f, totalRect.height);
    }

    private int SwatchSort(GradientEditor.Swatch lhs, GradientEditor.Swatch rhs)
    {
      if ((double) lhs.m_Time == (double) rhs.m_Time && lhs == this.m_SelectedSwatch)
        return -1;
      if ((double) lhs.m_Time == (double) rhs.m_Time && rhs == this.m_SelectedSwatch)
        return 1;
      return lhs.m_Time.CompareTo(rhs.m_Time);
    }

    private void AssignBack()
    {
      this.m_RGBSwatches.Sort((Comparison<GradientEditor.Swatch>) ((a, b) => this.SwatchSort(a, b)));
      GradientColorKey[] gradientColorKeyArray = new GradientColorKey[this.m_RGBSwatches.Count];
      for (int index = 0; index < this.m_RGBSwatches.Count; ++index)
      {
        gradientColorKeyArray[index].color = this.m_RGBSwatches[index].m_Value;
        gradientColorKeyArray[index].time = this.m_RGBSwatches[index].m_Time;
      }
      this.m_AlphaSwatches.Sort((Comparison<GradientEditor.Swatch>) ((a, b) => this.SwatchSort(a, b)));
      GradientAlphaKey[] gradientAlphaKeyArray = new GradientAlphaKey[this.m_AlphaSwatches.Count];
      for (int index = 0; index < this.m_AlphaSwatches.Count; ++index)
      {
        gradientAlphaKeyArray[index].alpha = this.m_AlphaSwatches[index].m_Value.r;
        gradientAlphaKeyArray[index].time = this.m_AlphaSwatches[index].m_Time;
      }
      this.m_Gradient.colorKeys = gradientColorKeyArray;
      this.m_Gradient.alphaKeys = gradientAlphaKeyArray;
      this.m_Gradient.mode = this.m_GradientMode;
      GUI.changed = true;
    }

    private void RemoveDuplicateOverlappingSwatches()
    {
      bool flag = false;
      for (int index = 1; index < this.m_RGBSwatches.Count; ++index)
      {
        if (Mathf.Approximately(this.m_RGBSwatches[index - 1].m_Time, this.m_RGBSwatches[index].m_Time))
        {
          this.m_RGBSwatches.RemoveAt(index);
          --index;
          flag = true;
        }
      }
      for (int index = 1; index < this.m_AlphaSwatches.Count; ++index)
      {
        if (Mathf.Approximately(this.m_AlphaSwatches[index - 1].m_Time, this.m_AlphaSwatches[index].m_Time))
        {
          this.m_AlphaSwatches.RemoveAt(index);
          --index;
          flag = true;
        }
      }
      if (!flag)
        return;
      this.AssignBack();
    }

    public static Texture2D GetBackgroundTexture()
    {
      if ((UnityEngine.Object) GradientEditor.s_BackgroundTexture == (UnityEngine.Object) null)
        GradientEditor.s_BackgroundTexture = GradientEditor.CreateCheckerTexture(32, 4, 4, Color.white, new Color(0.7f, 0.7f, 0.7f));
      return GradientEditor.s_BackgroundTexture;
    }

    public static Texture2D CreateCheckerTexture(int numCols, int numRows, int cellPixelWidth, Color col1, Color col2)
    {
      int height = numRows * cellPixelWidth;
      int width = numCols * cellPixelWidth;
      Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      Color[] colors = new Color[width * height];
      for (int index1 = 0; index1 < numRows; ++index1)
      {
        for (int index2 = 0; index2 < numCols; ++index2)
        {
          for (int index3 = 0; index3 < cellPixelWidth; ++index3)
          {
            for (int index4 = 0; index4 < cellPixelWidth; ++index4)
              colors[(index1 * cellPixelWidth + index3) * width + index2 * cellPixelWidth + index4] = (index1 + index2) % 2 != 0 ? col2 : col1;
          }
        }
      }
      texture2D.SetPixels(colors);
      texture2D.Apply();
      return texture2D;
    }

    public static void DrawGradientSwatch(Rect position, Gradient gradient, Color bgColor)
    {
      GradientEditor.DrawGradientSwatchInternal(position, gradient, (SerializedProperty) null, bgColor);
    }

    public static void DrawGradientSwatch(Rect position, SerializedProperty property, Color bgColor)
    {
      GradientEditor.DrawGradientSwatchInternal(position, (Gradient) null, property, bgColor);
    }

    private static void DrawGradientSwatchInternal(Rect position, Gradient gradient, SerializedProperty property, Color bgColor)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if (EditorGUI.showMixedValue)
      {
        Color color = GUI.color;
        GUI.color = new Color(0.82f, 0.82f, 0.82f, !GUI.enabled ? 2f : 1f) * bgColor;
        GUIStyle whiteTextureStyle = EditorGUIUtility.whiteTextureStyle;
        whiteTextureStyle.Draw(position, false, false, false, false);
        EditorGUI.BeginHandleMixedValueContentColor();
        whiteTextureStyle.Draw(position, EditorGUI.mixedValueContent, false, false, false, false);
        EditorGUI.EndHandleMixedValueContentColor();
        GUI.color = color;
      }
      else
      {
        Texture2D backgroundTexture = GradientEditor.GetBackgroundTexture();
        if ((UnityEngine.Object) backgroundTexture != (UnityEngine.Object) null)
        {
          Color color = GUI.color;
          GUI.color = bgColor;
          EditorGUIUtility.GetBasicTextureStyle(backgroundTexture).Draw(position, false, false, false, false);
          GUI.color = color;
        }
        Texture2D tex;
        float maxColorComponent;
        if (property != null)
        {
          tex = GradientPreviewCache.GetPropertyPreview(property);
          maxColorComponent = GradientEditor.GetMaxColorComponent(property.gradientValue);
        }
        else
        {
          tex = GradientPreviewCache.GetGradientPreview(gradient);
          maxColorComponent = GradientEditor.GetMaxColorComponent(gradient);
        }
        if ((UnityEngine.Object) tex == (UnityEngine.Object) null)
        {
          Debug.Log((object) "Warning: Could not create preview for gradient");
        }
        else
        {
          EditorGUIUtility.GetBasicTextureStyle(tex).Draw(position, false, false, false, false);
          if ((double) maxColorComponent <= 1.0)
            return;
          GUI.Label(new Rect(position.x, position.y - 1f, position.width - 3f, position.height + 2f), "HDR", EditorStyles.centeredGreyMiniLabel);
        }
      }
    }

    private static float GetMaxColorComponent(Gradient gradient)
    {
      float a = 0.0f;
      foreach (GradientColorKey colorKey in gradient.colorKeys)
        a = Mathf.Max(a, colorKey.color.maxColorComponent);
      return a;
    }

    private class Styles
    {
      public GUIStyle upSwatch = (GUIStyle) "Grad Up Swatch";
      public GUIStyle upSwatchOverlay = (GUIStyle) "Grad Up Swatch Overlay";
      public GUIStyle downSwatch = (GUIStyle) "Grad Down Swatch";
      public GUIStyle downSwatchOverlay = (GUIStyle) "Grad Down Swatch Overlay";
      public GUIContent modeText = new GUIContent("Mode");
      public GUIContent alphaText = new GUIContent("Alpha");
      public GUIContent colorText = new GUIContent("Color");
      public GUIContent locationText = new GUIContent("Location");
      public GUIContent percentText = new GUIContent("%");

      private static GUIStyle GetStyle(string name)
      {
        return ((GUISkin) EditorGUIUtility.LoadRequired("GradientEditor.GUISkin")).GetStyle(name);
      }
    }

    public class Swatch
    {
      public float m_Time;
      public Color m_Value;
      public bool m_IsAlpha;

      public Swatch(float time, Color value, bool isAlpha)
      {
        this.m_Time = time;
        this.m_Value = value;
        this.m_IsAlpha = isAlpha;
      }
    }
  }
}
