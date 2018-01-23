// Decompiled with JetBrains decompiler
// Type: UnityEditor.PlayerSettingsSplashScreenEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEditor.AnimatedValues;
using UnityEditor.Build;
using UnityEditor.Modules;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal sealed class PlayerSettingsSplashScreenEditor
  {
    private static readonly float k_MinLogoTime = 2f;
    private static readonly float k_MaxLogoTime = 10f;
    private static readonly float k_DefaultLogoTime = 2f;
    private static readonly float k_LogoListElementHeight = 72f;
    private static readonly float k_LogoListLogoFieldHeight = 64f;
    private static readonly float k_LogoListFooterHeight = 20f;
    private static readonly float k_LogoListUnityLogoMinWidth = 64f;
    private static readonly float k_LogoListUnityLogoMaxWidth = 220f;
    private static readonly float k_LogoListPropertyMinWidth = 230f;
    private static readonly float k_LogoListPropertyLabelWidth = 100f;
    private static readonly float k_MinPersonalEditionOverlayOpacity = 0.5f;
    private static readonly float k_MinProEditionOverlayOpacity = 0.0f;
    private static readonly PlayerSettingsSplashScreenEditor.Texts k_Texts = new PlayerSettingsSplashScreenEditor.Texts();
    private readonly AnimBool m_ShowAnimationControlsAnimator = new AnimBool();
    private readonly AnimBool m_ShowBackgroundColorAnimator = new AnimBool();
    private readonly AnimBool m_ShowLogoControlsAnimator = new AnimBool();
    private PlayerSettingsEditor m_Owner;
    private SerializedProperty m_ResolutionDialogBanner;
    private SerializedProperty m_ShowUnitySplashLogo;
    private SerializedProperty m_ShowUnitySplashScreen;
    private SerializedProperty m_SplashScreenAnimation;
    private SerializedProperty m_SplashScreenBackgroundAnimationZoom;
    private SerializedProperty m_SplashScreenBackgroundColor;
    private SerializedProperty m_SplashScreenBackgroundLandscape;
    private SerializedProperty m_SplashScreenBackgroundPortrait;
    private SerializedProperty m_SplashScreenDrawMode;
    private SerializedProperty m_SplashScreenLogoAnimationZoom;
    private SerializedProperty m_SplashScreenLogos;
    private SerializedProperty m_SplashScreenLogoStyle;
    private SerializedProperty m_SplashScreenOverlayOpacity;
    private SerializedProperty m_VirtualRealitySplashScreen;
    private ReorderableList m_LogoList;
    private float m_TotalLogosDuration;
    private static Sprite s_UnityLogo;

    public PlayerSettingsSplashScreenEditor(PlayerSettingsEditor owner)
    {
      this.m_Owner = owner;
    }

    public static extern bool licenseAllowsDisabling { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static Color GetSplashScreenActualBackgroundColor()
    {
      Color color;
      PlayerSettingsSplashScreenEditor.INTERNAL_CALL_GetSplashScreenActualBackgroundColor(out color);
      return color;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSplashScreenActualBackgroundColor(out Color value);

    internal static Texture2D GetSplashScreenActualBackgroundImage(Rect windowRect)
    {
      return PlayerSettingsSplashScreenEditor.INTERNAL_CALL_GetSplashScreenActualBackgroundImage(ref windowRect);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Texture2D INTERNAL_CALL_GetSplashScreenActualBackgroundImage(ref Rect windowRect);

    internal static Rect GetSplashScreenActualUVs(Rect windowRect)
    {
      Rect rect;
      PlayerSettingsSplashScreenEditor.INTERNAL_CALL_GetSplashScreenActualUVs(ref windowRect, out rect);
      return rect;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSplashScreenActualUVs(ref Rect windowRect, out Rect value);

    public void OnEnable()
    {
      this.m_ResolutionDialogBanner = this.m_Owner.FindPropertyAssert("resolutionDialogBanner");
      this.m_ShowUnitySplashLogo = this.m_Owner.FindPropertyAssert("m_ShowUnitySplashLogo");
      this.m_ShowUnitySplashScreen = this.m_Owner.FindPropertyAssert("m_ShowUnitySplashScreen");
      this.m_SplashScreenAnimation = this.m_Owner.FindPropertyAssert("m_SplashScreenAnimation");
      this.m_SplashScreenBackgroundAnimationZoom = this.m_Owner.FindPropertyAssert("m_SplashScreenBackgroundAnimationZoom");
      this.m_SplashScreenBackgroundColor = this.m_Owner.FindPropertyAssert("m_SplashScreenBackgroundColor");
      this.m_SplashScreenBackgroundLandscape = this.m_Owner.FindPropertyAssert("splashScreenBackgroundSourceLandscape");
      this.m_SplashScreenBackgroundPortrait = this.m_Owner.FindPropertyAssert("splashScreenBackgroundSourcePortrait");
      this.m_SplashScreenDrawMode = this.m_Owner.FindPropertyAssert("m_SplashScreenDrawMode");
      this.m_SplashScreenLogoAnimationZoom = this.m_Owner.FindPropertyAssert("m_SplashScreenLogoAnimationZoom");
      this.m_SplashScreenLogos = this.m_Owner.FindPropertyAssert("m_SplashScreenLogos");
      this.m_SplashScreenLogoStyle = this.m_Owner.FindPropertyAssert("m_SplashScreenLogoStyle");
      this.m_SplashScreenOverlayOpacity = this.m_Owner.FindPropertyAssert("m_SplashScreenOverlayOpacity");
      this.m_VirtualRealitySplashScreen = this.m_Owner.FindPropertyAssert("m_VirtualRealitySplashScreen");
      this.m_LogoList = new ReorderableList(this.m_Owner.serializedObject, this.m_SplashScreenLogos, true, true, true, true);
      this.m_LogoList.elementHeight = PlayerSettingsSplashScreenEditor.k_LogoListElementHeight;
      this.m_LogoList.footerHeight = PlayerSettingsSplashScreenEditor.k_LogoListFooterHeight;
      this.m_LogoList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.OnLogoListAddCallback);
      this.m_LogoList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawLogoListHeaderCallback);
      ReorderableList logoList = this.m_LogoList;
      // ISSUE: reference to a compiler-generated field
      if (PlayerSettingsSplashScreenEditor.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PlayerSettingsSplashScreenEditor.\u003C\u003Ef__mg\u0024cache0 = new ReorderableList.CanRemoveCallbackDelegate(PlayerSettingsSplashScreenEditor.OnLogoListCanRemoveCallback);
      }
      // ISSUE: reference to a compiler-generated field
      ReorderableList.CanRemoveCallbackDelegate fMgCache0 = PlayerSettingsSplashScreenEditor.\u003C\u003Ef__mg\u0024cache0;
      logoList.onCanRemoveCallback = fMgCache0;
      this.m_LogoList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawLogoListElementCallback);
      this.m_LogoList.drawFooterCallback = new ReorderableList.FooterCallbackDelegate(this.DrawLogoListFooterCallback);
      this.m_ShowAnimationControlsAnimator.value = this.m_SplashScreenAnimation.intValue == 2;
      this.m_ShowAnimationControlsAnimator.valueChanged.AddListener(new UnityAction(((Editor) this.m_Owner).Repaint));
      this.m_ShowBackgroundColorAnimator.value = this.m_SplashScreenBackgroundLandscape.objectReferenceValue == (Object) null;
      this.m_ShowBackgroundColorAnimator.valueChanged.AddListener(new UnityAction(((Editor) this.m_Owner).Repaint));
      this.m_ShowLogoControlsAnimator.value = this.m_ShowUnitySplashLogo.boolValue;
      this.m_ShowLogoControlsAnimator.valueChanged.AddListener(new UnityAction(((Editor) this.m_Owner).Repaint));
      if (!((Object) PlayerSettingsSplashScreenEditor.s_UnityLogo == (Object) null))
        return;
      PlayerSettingsSplashScreenEditor.s_UnityLogo = UnityEngine.Resources.GetBuiltinResource<Sprite>("UnitySplash-cube.png");
    }

    private void DrawLogoListHeaderCallback(Rect rect)
    {
      this.m_TotalLogosDuration = 0.0f;
      EditorGUI.LabelField(rect, "Logos");
    }

    private void DrawElementUnityLogo(Rect rect, int index, bool isActive, bool isFocused)
    {
      SerializedProperty propertyRelative = this.m_SplashScreenLogos.GetArrayElementAtIndex(index).FindPropertyRelative("duration");
      float num1 = Mathf.Clamp(rect.width - PlayerSettingsSplashScreenEditor.k_LogoListPropertyMinWidth, PlayerSettingsSplashScreenEditor.k_LogoListUnityLogoMinWidth, PlayerSettingsSplashScreenEditor.k_LogoListUnityLogoMaxWidth);
      float height = num1 / ((float) PlayerSettingsSplashScreenEditor.s_UnityLogo.texture.width / (float) PlayerSettingsSplashScreenEditor.s_UnityLogo.texture.height);
      Rect position = new Rect(rect.x, rect.y + (float) (((double) rect.height - (double) height) / 2.0), PlayerSettingsSplashScreenEditor.k_LogoListUnityLogoMaxWidth, height);
      Color color = GUI.color;
      GUI.color = this.m_SplashScreenLogoStyle.intValue != 0 ? Color.white : Color.black;
      GUI.Label(position, (Texture) PlayerSettingsSplashScreenEditor.s_UnityLogo.texture);
      GUI.color = color;
      float labelWidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = PlayerSettingsSplashScreenEditor.k_LogoListPropertyLabelWidth;
      Rect rect1 = new Rect(rect.x + num1, rect.y + EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight, rect.width - num1, EditorGUIUtility.singleLineHeight);
      EditorGUI.BeginChangeCheck();
      GUIContent label = EditorGUI.BeginProperty(rect1, PlayerSettingsSplashScreenEditor.k_Texts.logoDuration, propertyRelative);
      float num2 = EditorGUI.Slider(rect1, label, propertyRelative.floatValue, PlayerSettingsSplashScreenEditor.k_MinLogoTime, PlayerSettingsSplashScreenEditor.k_MaxLogoTime);
      if (EditorGUI.EndChangeCheck())
        propertyRelative.floatValue = num2;
      EditorGUI.EndProperty();
      EditorGUIUtility.labelWidth = labelWidth;
      this.m_TotalLogosDuration += propertyRelative.floatValue;
    }

    private void DrawLogoListElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
      rect.height -= EditorGUIUtility.standardVerticalSpacing;
      SerializedProperty arrayElementAtIndex = this.m_SplashScreenLogos.GetArrayElementAtIndex(index);
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("logo");
      if (propertyRelative1.objectReferenceValue == (Object) PlayerSettingsSplashScreenEditor.s_UnityLogo)
      {
        this.DrawElementUnityLogo(rect, index, isActive, isFocused);
      }
      else
      {
        float num1 = Mathf.Clamp(rect.width - PlayerSettingsSplashScreenEditor.k_LogoListPropertyMinWidth, PlayerSettingsSplashScreenEditor.k_LogoListUnityLogoMinWidth, PlayerSettingsSplashScreenEditor.k_LogoListUnityLogoMaxWidth);
        Rect position1 = new Rect(rect.x, rect.y + (float) (((double) rect.height - (double) PlayerSettingsSplashScreenEditor.k_LogoListLogoFieldHeight) / 2.0), PlayerSettingsSplashScreenEditor.k_LogoListUnityLogoMinWidth, PlayerSettingsSplashScreenEditor.k_LogoListLogoFieldHeight);
        EditorGUI.BeginChangeCheck();
        Object @object = EditorGUI.ObjectField(position1, GUIContent.none, propertyRelative1.objectReferenceValue, typeof (Sprite), false);
        if (EditorGUI.EndChangeCheck())
          propertyRelative1.objectReferenceValue = @object;
        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = PlayerSettingsSplashScreenEditor.k_LogoListPropertyLabelWidth;
        Rect position2 = new Rect(rect.x + num1, rect.y + EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight, rect.width - num1, EditorGUIUtility.singleLineHeight);
        EditorGUI.BeginChangeCheck();
        SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("duration");
        float num2 = EditorGUI.Slider(position2, PlayerSettingsSplashScreenEditor.k_Texts.logoDuration, propertyRelative2.floatValue, PlayerSettingsSplashScreenEditor.k_MinLogoTime, PlayerSettingsSplashScreenEditor.k_MaxLogoTime);
        if (EditorGUI.EndChangeCheck())
          propertyRelative2.floatValue = num2;
        EditorGUIUtility.labelWidth = labelWidth;
        this.m_TotalLogosDuration += propertyRelative2.floatValue;
      }
    }

    private void DrawLogoListFooterCallback(Rect rect)
    {
      float num = Mathf.Max(PlayerSettingsSplashScreenEditor.k_MinLogoTime, this.m_TotalLogosDuration);
      EditorGUI.LabelField(rect, "Splash Screen Duration: " + num.ToString(), EditorStyles.miniBoldLabel);
      ReorderableList.defaultBehaviours.DrawFooter(rect, this.m_LogoList);
    }

    private void OnLogoListAddCallback(ReorderableList list)
    {
      int arraySize = this.m_SplashScreenLogos.arraySize;
      this.m_SplashScreenLogos.InsertArrayElementAtIndex(this.m_SplashScreenLogos.arraySize);
      SerializedProperty arrayElementAtIndex = this.m_SplashScreenLogos.GetArrayElementAtIndex(arraySize);
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("logo");
      SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("duration");
      propertyRelative1.objectReferenceValue = (Object) null;
      propertyRelative2.floatValue = PlayerSettingsSplashScreenEditor.k_DefaultLogoTime;
    }

    private static bool OnLogoListCanRemoveCallback(ReorderableList list)
    {
      return list.serializedProperty.GetArrayElementAtIndex(list.index).FindPropertyRelative("logo").objectReferenceValue != (Object) PlayerSettingsSplashScreenEditor.s_UnityLogo;
    }

    private void AddUnityLogoToLogosList()
    {
      for (int index = 0; index < this.m_SplashScreenLogos.arraySize; ++index)
      {
        if (this.m_SplashScreenLogos.GetArrayElementAtIndex(index).FindPropertyRelative("logo").objectReferenceValue == (Object) PlayerSettingsSplashScreenEditor.s_UnityLogo)
          return;
      }
      this.m_SplashScreenLogos.InsertArrayElementAtIndex(0);
      SerializedProperty arrayElementAtIndex = this.m_SplashScreenLogos.GetArrayElementAtIndex(0);
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("logo");
      SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("duration");
      propertyRelative1.objectReferenceValue = (Object) PlayerSettingsSplashScreenEditor.s_UnityLogo;
      propertyRelative2.floatValue = PlayerSettingsSplashScreenEditor.k_DefaultLogoTime;
    }

    private void RemoveUnityLogoFromLogosList()
    {
      for (int index = 0; index < this.m_SplashScreenLogos.arraySize; ++index)
      {
        if (this.m_SplashScreenLogos.GetArrayElementAtIndex(index).FindPropertyRelative("logo").objectReferenceValue == (Object) PlayerSettingsSplashScreenEditor.s_UnityLogo)
        {
          this.m_SplashScreenLogos.DeleteArrayElementAtIndex(index);
          --index;
        }
      }
    }

    private static bool TargetSupportsOptionalBuiltinSplashScreen(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
    {
      if (settingsExtension != null)
        return settingsExtension.CanShowUnitySplashScreen();
      return targetGroup == BuildTargetGroup.Standalone;
    }

    private static void ObjectReferencePropertyField<T>(SerializedProperty property, GUIContent label) where T : Object
    {
      EditorGUI.BeginChangeCheck();
      Rect controlRect = EditorGUILayout.GetControlRect(true, 64f, EditorStyles.objectFieldThumb, new GUILayoutOption[0]);
      label = EditorGUI.BeginProperty(controlRect, label, property);
      Object @object = EditorGUI.ObjectField(controlRect, label, property.objectReferenceValue, typeof (T), false);
      if (EditorGUI.EndChangeCheck())
      {
        property.objectReferenceValue = @object;
        GUI.changed = true;
      }
      EditorGUI.EndProperty();
    }

    public void SplashSectionGUI(BuildPlatform platform, BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension, int sectionIndex = 2)
    {
      GUI.changed = false;
      if (this.m_Owner.BeginSettingsBox(sectionIndex, PlayerSettingsSplashScreenEditor.k_Texts.title))
      {
        if (targetGroup == BuildTargetGroup.Standalone)
        {
          PlayerSettingsSplashScreenEditor.ObjectReferencePropertyField<Texture2D>(this.m_ResolutionDialogBanner, PlayerSettingsSplashScreenEditor.k_Texts.configDialogBanner);
          EditorGUILayout.Space();
        }
        if (this.m_Owner.m_VRSettings.TargetGroupSupportsVirtualReality(targetGroup))
          PlayerSettingsSplashScreenEditor.ObjectReferencePropertyField<Texture2D>(this.m_VirtualRealitySplashScreen, PlayerSettingsSplashScreenEditor.k_Texts.vrSplashScreen);
        if (PlayerSettingsSplashScreenEditor.TargetSupportsOptionalBuiltinSplashScreen(targetGroup, settingsExtension))
          this.BuiltinCustomSplashScreenGUI();
        if (settingsExtension != null)
          settingsExtension.SplashSectionGUI();
        if (this.m_ShowUnitySplashScreen.boolValue)
          this.m_Owner.ShowSharedNote();
      }
      this.m_Owner.EndSettingsBox();
    }

    private void BuiltinCustomSplashScreenGUI()
    {
      EditorGUILayout.LabelField(PlayerSettingsSplashScreenEditor.k_Texts.splashTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
      using (new EditorGUI.DisabledScope(!PlayerSettingsSplashScreenEditor.licenseAllowsDisabling))
      {
        EditorGUILayout.PropertyField(this.m_ShowUnitySplashScreen, PlayerSettingsSplashScreenEditor.k_Texts.showSplash, new GUILayoutOption[0]);
        if (!this.m_ShowUnitySplashScreen.boolValue)
          return;
      }
      if (GUI.Button(EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(PlayerSettingsSplashScreenEditor.k_Texts.previewSplash, (GUIStyle) "button"), new GUIContent(" ")), PlayerSettingsSplashScreenEditor.k_Texts.previewSplash))
      {
        UnityEngine.Rendering.SplashScreen.Begin();
        GameView mainGameView = GameView.GetMainGameView();
        if ((bool) ((Object) mainGameView))
          mainGameView.Focus();
        GameView.RepaintAll();
      }
      EditorGUILayout.PropertyField(this.m_SplashScreenLogoStyle, PlayerSettingsSplashScreenEditor.k_Texts.splashStyle, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_SplashScreenAnimation, PlayerSettingsSplashScreenEditor.k_Texts.animate, new GUILayoutOption[0]);
      this.m_ShowAnimationControlsAnimator.target = this.m_SplashScreenAnimation.intValue == 2;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowAnimationControlsAnimator.faded))
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.Slider(this.m_SplashScreenLogoAnimationZoom, 0.0f, 1f, PlayerSettingsSplashScreenEditor.k_Texts.logoZoom, new GUILayoutOption[0]);
        EditorGUILayout.Slider(this.m_SplashScreenBackgroundAnimationZoom, 0.0f, 1f, PlayerSettingsSplashScreenEditor.k_Texts.backgroundZoom, new GUILayoutOption[0]);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
      EditorGUILayout.Space();
      EditorGUILayout.LabelField(PlayerSettingsSplashScreenEditor.k_Texts.logosTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
      using (new EditorGUI.DisabledScope(!Application.HasProLicense()))
      {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(this.m_ShowUnitySplashLogo, PlayerSettingsSplashScreenEditor.k_Texts.showLogo, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          if (!this.m_ShowUnitySplashLogo.boolValue)
            this.RemoveUnityLogoFromLogosList();
          else if (this.m_SplashScreenDrawMode.intValue == 1)
            this.AddUnityLogoToLogosList();
        }
        this.m_ShowLogoControlsAnimator.target = this.m_ShowUnitySplashLogo.boolValue;
      }
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowLogoControlsAnimator.faded))
      {
        ++EditorGUI.indentLevel;
        EditorGUI.BeginChangeCheck();
        int intValue = this.m_SplashScreenDrawMode.intValue;
        EditorGUILayout.PropertyField(this.m_SplashScreenDrawMode, PlayerSettingsSplashScreenEditor.k_Texts.drawMode, new GUILayoutOption[0]);
        if (intValue != this.m_SplashScreenDrawMode.intValue)
        {
          if (this.m_SplashScreenDrawMode.intValue == 0)
            this.RemoveUnityLogoFromLogosList();
          else
            this.AddUnityLogoToLogosList();
        }
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
      this.m_LogoList.DoLayoutList();
      EditorGUILayout.Space();
      EditorGUILayout.LabelField(PlayerSettingsSplashScreenEditor.k_Texts.backgroundTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.Slider(this.m_SplashScreenOverlayOpacity, !Application.HasProLicense() ? PlayerSettingsSplashScreenEditor.k_MinPersonalEditionOverlayOpacity : PlayerSettingsSplashScreenEditor.k_MinProEditionOverlayOpacity, 1f, PlayerSettingsSplashScreenEditor.k_Texts.overlayOpacity, new GUILayoutOption[0]);
      this.m_ShowBackgroundColorAnimator.target = this.m_SplashScreenBackgroundLandscape.objectReferenceValue == (Object) null;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowBackgroundColorAnimator.faded))
        EditorGUILayout.PropertyField(this.m_SplashScreenBackgroundColor, PlayerSettingsSplashScreenEditor.k_Texts.backgroundColor, new GUILayoutOption[0]);
      EditorGUILayout.EndFadeGroup();
      PlayerSettingsSplashScreenEditor.ObjectReferencePropertyField<Sprite>(this.m_SplashScreenBackgroundLandscape, PlayerSettingsSplashScreenEditor.k_Texts.backgroundImage);
      if (GUI.changed && this.m_SplashScreenBackgroundLandscape.objectReferenceValue == (Object) null)
        this.m_SplashScreenBackgroundPortrait.objectReferenceValue = (Object) null;
      using (new EditorGUI.DisabledScope(this.m_SplashScreenBackgroundLandscape.objectReferenceValue == (Object) null))
        PlayerSettingsSplashScreenEditor.ObjectReferencePropertyField<Sprite>(this.m_SplashScreenBackgroundPortrait, PlayerSettingsSplashScreenEditor.k_Texts.backgroundPortraitImage);
    }

    private class Texts
    {
      public GUIContent animate = EditorGUIUtility.TextContent("Animation");
      public GUIContent backgroundColor = EditorGUIUtility.TextContent("Background Color|Background color when no background image is used.");
      public GUIContent backgroundImage = EditorGUIUtility.TextContent("Background Image|Image to be used in landscape and portrait(when portrait image is not set).");
      public GUIContent backgroundPortraitImage = EditorGUIUtility.TextContent("Alternate Portrait Image*|Optional image to be used in portrait mode.");
      public GUIContent backgroundTitle = EditorGUIUtility.TextContent("Background*");
      public GUIContent backgroundZoom = EditorGUIUtility.TextContent("Background Zoom");
      public GUIContent configDialogBanner = EditorGUIUtility.TextContent("Application Config Dialog Banner");
      public GUIContent drawMode = EditorGUIUtility.TextContent("Draw Mode");
      public GUIContent logoDuration = EditorGUIUtility.TextContent("Logo Duration|The time the logo will be shown for.");
      public GUIContent logosTitle = EditorGUIUtility.TextContent("Logos*");
      public GUIContent logoZoom = EditorGUIUtility.TextContent("Logo Zoom");
      public GUIContent overlayOpacity = EditorGUIUtility.TextContent("Overlay Opacity|Overlay strength applied to improve logo visibility.");
      public GUIContent previewSplash = EditorGUIUtility.TextContent("Preview|Preview the splash screen in the game view.");
      public GUIContent showLogo = EditorGUIUtility.TextContent("Show Unity Logo");
      public GUIContent showSplash = EditorGUIUtility.TextContent("Show Splash Screen");
      public GUIContent splashStyle = EditorGUIUtility.TextContent("Splash Style");
      public GUIContent splashTitle = EditorGUIUtility.TextContent("Splash Screen");
      public GUIContent title = EditorGUIUtility.TextContent("Splash Image");
      public GUIContent vrSplashScreen = EditorGUIUtility.TextContent("Virtual Reality Splash Image");
    }
  }
}
