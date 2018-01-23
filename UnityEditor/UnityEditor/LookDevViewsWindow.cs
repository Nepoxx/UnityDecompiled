// Decompiled with JetBrains decompiler
// Type: UnityEditor.LookDevViewsWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class LookDevViewsWindow : PopupWindowContent
  {
    private static LookDevViewsWindow.Styles s_Styles = new LookDevViewsWindow.Styles();
    private static float kIconSize = 32f;
    private static float kLabelWidth = 120f;
    private static float kSliderWidth = 100f;
    private static float kSliderFieldWidth = 30f;
    private static float kSliderFieldPadding = 5f;
    private static float kLineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    private float m_WindowHeight = 5f * LookDevViewsWindow.kLineHeight + EditorGUIUtility.standardVerticalSpacing;
    private float m_WindowWidth = (float) ((double) LookDevViewsWindow.kLabelWidth + (double) LookDevViewsWindow.kSliderWidth + (double) LookDevViewsWindow.kSliderFieldWidth + (double) LookDevViewsWindow.kSliderFieldPadding + 5.0);
    private readonly LookDevView m_LookDevView;

    public LookDevViewsWindow(LookDevView lookDevView)
    {
      this.m_LookDevView = lookDevView;
    }

    private GUIContent GetGUIContentLink(bool active)
    {
      return !active ? LookDevViewsWindow.styles.sLinkInactive : LookDevViewsWindow.styles.sLinkActive;
    }

    public static LookDevViewsWindow.Styles styles
    {
      get
      {
        return LookDevViewsWindow.s_Styles;
      }
    }

    private bool NeedLoD()
    {
      return this.m_LookDevView.config.GetObjectLoDCount(LookDevEditionContext.Left) > 1 || this.m_LookDevView.config.GetObjectLoDCount(LookDevEditionContext.Right) > 1;
    }

    private float GetHeight()
    {
      float windowHeight = this.m_WindowHeight;
      if (this.NeedLoD())
        windowHeight += LookDevViewsWindow.kLineHeight;
      return windowHeight;
    }

    public override Vector2 GetWindowSize()
    {
      return new Vector2(this.m_WindowWidth + (this.m_LookDevView.config.lookDevMode == LookDevMode.Single1 || this.m_LookDevView.config.lookDevMode == LookDevMode.Single2 ? 0.0f : this.m_WindowWidth + LookDevViewsWindow.kIconSize), this.GetHeight());
    }

    public override void OnGUI(Rect rect)
    {
      if ((UnityEngine.Object) this.m_LookDevView.config == (UnityEngine.Object) null)
        return;
      Rect drawPos = new Rect(0.0f, 0.0f, rect.width, this.GetHeight());
      this.DrawOneView(drawPos, this.m_LookDevView.config.lookDevMode != LookDevMode.Single2 ? LookDevEditionContext.Left : LookDevEditionContext.Right);
      drawPos.x += this.m_WindowWidth;
      drawPos.x += LookDevViewsWindow.kIconSize;
      this.DrawOneView(drawPos, LookDevEditionContext.Right);
      if (Event.current.type == EventType.MouseMove)
        Event.current.Use();
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape)
        return;
      this.editorWindow.Close();
      GUIUtility.ExitGUI();
    }

    private void DrawOneView(Rect drawPos, LookDevEditionContext context)
    {
      int index1 = (int) context;
      bool flag1 = this.m_LookDevView.config.lookDevMode != LookDevMode.Single1 && context == LookDevEditionContext.Left || this.m_LookDevView.config.lookDevMode != LookDevMode.Single2 && context == LookDevEditionContext.Right;
      GUILayout.BeginArea(drawPos);
      GUILayout.Label(LookDevViewsWindow.styles.sViewTitle[index1], LookDevViewsWindow.styles.sViewTitleStyles[index1], new GUILayoutOption[0]);
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical(GUILayout.Width(this.m_WindowWidth));
      GUILayout.BeginHorizontal(GUILayout.Height(LookDevViewsWindow.kLineHeight));
      GUILayout.Label(LookDevViewsWindow.styles.sExposure, LookDevViewsWindow.styles.sMenuItem, new GUILayoutOption[1]
      {
        GUILayout.Width(LookDevViewsWindow.kLabelWidth)
      });
      float floatProperty1 = this.m_LookDevView.config.GetFloatProperty(LookDevProperty.ExposureValue, context);
      EditorGUI.BeginChangeCheck();
      float num1 = Mathf.Round(this.m_LookDevView.config.exposureRange);
      float num2 = Mathf.Clamp(GUILayout.HorizontalSlider(floatProperty1, (float) -(double) num1, num1, GUILayout.Width(LookDevViewsWindow.kSliderWidth)), -num1, num1);
      float num3 = Mathf.Clamp(EditorGUILayout.FloatField((float) Math.Round((double) num2, (double) num2 >= 0.0 ? 2 : 1), GUILayout.Width(LookDevViewsWindow.kSliderFieldWidth)), -num1, num1);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_LookDevView.config.UpdateFocus(context);
        this.m_LookDevView.config.UpdateFloatProperty(LookDevProperty.ExposureValue, num3);
      }
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal(GUILayout.Height(LookDevViewsWindow.kLineHeight));
      int hdriCount = this.m_LookDevView.envLibrary.hdriCount;
      using (new EditorGUI.DisabledScope(hdriCount <= 1))
      {
        GUILayout.Label(LookDevViewsWindow.styles.sEnvironment, LookDevViewsWindow.styles.sMenuItem, new GUILayoutOption[1]
        {
          GUILayout.Width(LookDevViewsWindow.kLabelWidth)
        });
        if (hdriCount > 1)
        {
          int max = hdriCount - 1;
          int intProperty = this.m_LookDevView.config.GetIntProperty(LookDevProperty.HDRI, context);
          EditorGUI.BeginChangeCheck();
          int num4 = Mathf.Clamp(EditorGUILayout.IntField((int) GUILayout.HorizontalSlider((float) intProperty, 0.0f, (float) max, GUILayout.Width(LookDevViewsWindow.kSliderWidth)), GUILayout.Width(LookDevViewsWindow.kSliderFieldWidth)), 0, max);
          if (EditorGUI.EndChangeCheck())
          {
            this.m_LookDevView.config.UpdateFocus(context);
            this.m_LookDevView.config.UpdateIntProperty(LookDevProperty.HDRI, num4);
          }
        }
        else
        {
          double num4 = (double) GUILayout.HorizontalSlider(0.0f, 0.0f, 0.0f, GUILayout.Width(LookDevViewsWindow.kSliderWidth));
          GUILayout.Label(LookDevViewsWindow.styles.sZero, LookDevViewsWindow.styles.sMenuItem, new GUILayoutOption[0]);
        }
      }
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal(GUILayout.Height(LookDevViewsWindow.kLineHeight));
      GUILayout.Label(LookDevViewsWindow.styles.sShadingMode, LookDevViewsWindow.styles.sMenuItem, new GUILayoutOption[1]
      {
        GUILayout.Width(LookDevViewsWindow.kLabelWidth)
      });
      int intProperty1 = this.m_LookDevView.config.GetIntProperty(LookDevProperty.ShadingMode, context);
      EditorGUI.BeginChangeCheck();
      int num5 = EditorGUILayout.IntPopup("", intProperty1, LookDevViewsWindow.styles.sShadingModeStrings, LookDevViewsWindow.styles.sShadingModeValues, new GUILayoutOption[1]{ GUILayout.Width((float) ((double) LookDevViewsWindow.kSliderFieldWidth + (double) LookDevViewsWindow.kSliderWidth + 4.0)) });
      if (EditorGUI.EndChangeCheck())
      {
        this.m_LookDevView.config.UpdateFocus(context);
        this.m_LookDevView.config.UpdateIntProperty(LookDevProperty.ShadingMode, num5);
      }
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal(GUILayout.Height(LookDevViewsWindow.kLineHeight));
      GUILayout.Label(LookDevViewsWindow.styles.sRotation, LookDevViewsWindow.styles.sMenuItem, new GUILayoutOption[1]
      {
        GUILayout.Width(LookDevViewsWindow.kLabelWidth)
      });
      float floatProperty2 = this.m_LookDevView.config.GetFloatProperty(LookDevProperty.EnvRotation, context);
      EditorGUI.BeginChangeCheck();
      float num6 = Mathf.Clamp(EditorGUILayout.FloatField((float) Math.Round((double) GUILayout.HorizontalSlider(floatProperty2, 0.0f, 720f, GUILayout.Width(LookDevViewsWindow.kSliderWidth)), 0), GUILayout.Width(LookDevViewsWindow.kSliderFieldWidth)), 0.0f, 720f);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_LookDevView.config.UpdateFocus(context);
        this.m_LookDevView.config.UpdateFloatProperty(LookDevProperty.EnvRotation, num6);
      }
      GUILayout.EndHorizontal();
      if (this.NeedLoD())
      {
        GUILayout.BeginHorizontal(GUILayout.Height(LookDevViewsWindow.kLineHeight));
        if (this.m_LookDevView.config.GetObjectLoDCount(context) > 1)
        {
          int intProperty2 = this.m_LookDevView.config.GetIntProperty(LookDevProperty.LoDIndex, context);
          GUILayout.Label(intProperty2 != -1 ? LookDevViewsWindow.styles.sLoD : LookDevViewsWindow.styles.sLoDAuto, LookDevViewsWindow.styles.sMenuItem, new GUILayoutOption[1]
          {
            GUILayout.Width(LookDevViewsWindow.kLabelWidth)
          });
          EditorGUI.BeginChangeCheck();
          int max = this.m_LookDevView.config.GetObjectLoDCount(context) - 1;
          if (this.m_LookDevView.config.lookDevMode != LookDevMode.Single1 && this.m_LookDevView.config.lookDevMode != LookDevMode.Single2 && this.m_LookDevView.config.IsPropertyLinked(LookDevProperty.LoDIndex))
            max = Math.Min(this.m_LookDevView.config.GetObjectLoDCount(LookDevEditionContext.Left), this.m_LookDevView.config.GetObjectLoDCount(LookDevEditionContext.Right)) - 1;
          int num4 = EditorGUILayout.IntField((int) GUILayout.HorizontalSlider((float) Mathf.Clamp(intProperty2, -1, max), -1f, (float) max, GUILayout.Width(LookDevViewsWindow.kSliderWidth)), GUILayout.Width(LookDevViewsWindow.kSliderFieldWidth));
          if (EditorGUI.EndChangeCheck())
          {
            this.m_LookDevView.config.UpdateFocus(context);
            this.m_LookDevView.config.UpdateIntProperty(LookDevProperty.LoDIndex, num4);
          }
        }
        GUILayout.EndHorizontal();
      }
      GUILayout.EndVertical();
      if (flag1)
      {
        GUILayout.BeginVertical(GUILayout.Width(LookDevViewsWindow.kIconSize));
        LookDevProperty[] lookDevPropertyArray = new LookDevProperty[5]{ LookDevProperty.ExposureValue, LookDevProperty.HDRI, LookDevProperty.ShadingMode, LookDevProperty.EnvRotation, LookDevProperty.LoDIndex };
        int num4 = 4 + (!this.NeedLoD() ? 0 : 1);
        for (int index2 = 0; index2 < num4; ++index2)
        {
          EditorGUI.BeginChangeCheck();
          bool active = this.m_LookDevView.config.IsPropertyLinked(lookDevPropertyArray[index2]);
          bool flag2 = GUILayout.Toggle((active ? 1 : 0) != 0, this.GetGUIContentLink(active), LookDevViewsWindow.styles.sToolBarButton, new GUILayoutOption[1]{ GUILayout.Height(LookDevViewsWindow.kLineHeight) });
          if (EditorGUI.EndChangeCheck())
            this.m_LookDevView.config.UpdatePropertyLink(lookDevPropertyArray[index2], flag2);
        }
        GUILayout.EndVertical();
      }
      GUILayout.EndHorizontal();
      GUILayout.EndArea();
    }

    public class Styles
    {
      public readonly GUIStyle sMenuItem = (GUIStyle) "MenuItem";
      public readonly GUIStyle sHeaderStyle = EditorStyles.miniLabel;
      public readonly GUIStyle sToolBarButton = (GUIStyle) "toolbarbutton";
      public readonly GUIContent sTitle = EditorGUIUtility.TextContent("Views");
      public readonly GUIContent sExposure = EditorGUIUtility.TextContent("EV|Exposure value: control the brightness of the environment.");
      public readonly GUIContent sEnvironment = EditorGUIUtility.TextContent("Environment|Select an environment from the list of currently available environments");
      public readonly GUIContent sRotation = EditorGUIUtility.TextContent("Rotation|Change the rotation of the environment");
      public readonly GUIContent sZero = EditorGUIUtility.TextContent("0");
      public readonly GUIContent sLoD = EditorGUIUtility.TextContent("LoD|Choose displayed LoD");
      public readonly GUIContent sLoDAuto = EditorGUIUtility.TextContent("LoD (auto)|Choose displayed LoD");
      public readonly GUIContent sShadingMode = EditorGUIUtility.TextContent("Shading|Select shading mode");
      public readonly GUIContent[] sViewTitle = new GUIContent[2]{ EditorGUIUtility.TextContent("Main View (1)"), EditorGUIUtility.TextContent("Second View (2)") };
      public readonly GUIStyle[] sViewTitleStyles = new GUIStyle[2]{ new GUIStyle(EditorStyles.miniLabel), new GUIStyle(EditorStyles.miniLabel) };
      public readonly string[] sShadingModeStrings = new string[6]{ "Shaded", "Shaded Wireframe", "Albedo", "Specular", "Smoothness", "Normal" };
      public readonly int[] sShadingModeValues = new int[6]{ -1, 2, 8, 9, 10, 11 };
      public readonly GUIContent sLinkActive = EditorGUIUtility.IconContent("LookDevMirrorViewsActive", "Link|Links the property between the different views");
      public readonly GUIContent sLinkInactive = EditorGUIUtility.IconContent("LookDevMirrorViewsInactive", "Link|Links the property between the different views");

      public Styles()
      {
        this.sViewTitleStyles[0].normal.textColor = (Color) LookDevView.m_FirstViewGizmoColor;
        this.sViewTitleStyles[1].normal.textColor = (Color) LookDevView.m_SecondViewGizmoColor;
      }
    }
  }
}
