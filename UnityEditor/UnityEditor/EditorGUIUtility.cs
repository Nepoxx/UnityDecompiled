// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorGUIUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Miscellaneous helper stuff for EditorGUI.</para>
  /// </summary>
  public sealed class EditorGUIUtility : GUIUtility
  {
    internal static int s_FontIsBold = -1;
    internal static SliderLabels sliderLabels = new SliderLabels();
    internal static Color kDarkViewBackground = new Color(0.22f, 0.22f, 0.22f, 0.0f);
    private static GUIContent s_ObjectContent = new GUIContent();
    private static GUIContent s_Text = new GUIContent();
    private static GUIContent s_Image = new GUIContent();
    private static GUIContent s_TextImage = new GUIContent();
    private static GUIContent s_BlankContent = new GUIContent(" ");
    private static Hashtable s_TextGUIContents = new Hashtable();
    private static Hashtable s_GUIContents = new Hashtable();
    private static Hashtable s_IconGUIContents = new Hashtable();
    internal static int s_LastControlID = 0;
    private static bool s_HierarchyMode = false;
    internal static bool s_WideMode = false;
    private static float s_ContextWidth = 0.0f;
    private static float s_LabelWidth = 0.0f;
    private static float s_FieldWidth = 0.0f;
    [Obsolete("This field is no longer used by any builtin controls. If passing this field to GetControlID, explicitly use the FocusType enum instead.", false)]
    public static FocusType native = FocusType.Keyboard;
    private static List<EditorGUIUtility.HeaderItemDelegate> s_EditorHeaderItemsMethods = (List<EditorGUIUtility.HeaderItemDelegate>) null;
    private static Texture2D s_InfoIcon;
    private static Texture2D s_WarningIcon;
    private static Texture2D s_ErrorIcon;
    private static GUIStyle s_WhiteTextureStyle;
    private static GUIStyle s_BasicTextureStyle;
    internal static Material s_GUITextureBlitColorspaceMaterial;

    static EditorGUIUtility()
    {
      GUISkin.SkinChangedDelegate skinChanged = GUISkin.m_SkinChanged;
      // ISSUE: reference to a compiler-generated field
      if (EditorGUIUtility.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditorGUIUtility.\u003C\u003Ef__mg\u0024cache0 = new GUISkin.SkinChangedDelegate(EditorGUIUtility.SkinChanged);
      }
      // ISSUE: reference to a compiler-generated field
      GUISkin.SkinChangedDelegate fMgCache0 = EditorGUIUtility.\u003C\u003Ef__mg\u0024cache0;
      GUISkin.m_SkinChanged = skinChanged + fMgCache0;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string SerializeMainMenuToString();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetMenuLocalizationTestMode(bool onoff);

    /// <summary>
    ///   <para>Get the height used for a single Editor control such as a one-line EditorGUI.TextField or EditorGUI.Popup.</para>
    /// </summary>
    public static float singleLineHeight
    {
      get
      {
        return 16f;
      }
    }

    /// <summary>
    ///   <para>Get the height used by default for vertical spacing between controls.</para>
    /// </summary>
    public static float standardVerticalSpacing
    {
      get
      {
        return 2f;
      }
    }

    internal static GUIContent TextContent(string textAndTooltip)
    {
      if (textAndTooltip == null)
        textAndTooltip = "";
      string str = textAndTooltip;
      GUIContent guiContent = (GUIContent) EditorGUIUtility.s_TextGUIContents[(object) str];
      if (guiContent == null)
      {
        string[] andTooltipString = EditorGUIUtility.GetNameAndTooltipString(textAndTooltip);
        guiContent = new GUIContent(andTooltipString[1]);
        if (andTooltipString[2] != null)
          guiContent.tooltip = andTooltipString[2];
        EditorGUIUtility.s_TextGUIContents[(object) str] = (object) guiContent;
      }
      return guiContent;
    }

    internal static GUIContent TextContentWithIcon(string textAndTooltip, string icon)
    {
      if (textAndTooltip == null)
        textAndTooltip = "";
      if (icon == null)
        icon = "";
      string str = string.Format("{0}|{1}", (object) textAndTooltip, (object) icon);
      GUIContent guiContent = (GUIContent) EditorGUIUtility.s_TextGUIContents[(object) str];
      if (guiContent == null)
      {
        string[] andTooltipString = EditorGUIUtility.GetNameAndTooltipString(textAndTooltip);
        guiContent = new GUIContent(andTooltipString[1]);
        guiContent.image = (Texture) EditorGUIUtility.LoadIconRequired(icon);
        if (andTooltipString[2] != null)
          guiContent.tooltip = andTooltipString[2];
        EditorGUIUtility.s_TextGUIContents[(object) str] = (object) guiContent;
      }
      return guiContent;
    }

    [ExcludeFromDocs]
    internal static GUIContent TrTextContent(string text, string tooltip)
    {
      Texture icon = (Texture) null;
      return EditorGUIUtility.TrTextContent(text, tooltip, icon);
    }

    [ExcludeFromDocs]
    internal static GUIContent TrTextContent(string text)
    {
      Texture icon = (Texture) null;
      string tooltip = (string) null;
      return EditorGUIUtility.TrTextContent(text, tooltip, icon);
    }

    internal static GUIContent TrTextContent(string text, [DefaultValue("null")] string tooltip, [DefaultValue("null")] Texture icon)
    {
      string str = string.Format("{0}|{1}", text == null ? (object) "" : (object) text, tooltip == null ? (object) "" : (object) tooltip);
      GUIContent guiContent = (GUIContent) EditorGUIUtility.s_GUIContents[(object) str];
      if (guiContent == null)
      {
        guiContent = new GUIContent(L10n.Tr(text));
        if (tooltip != null)
          guiContent.tooltip = L10n.Tr(tooltip);
        if ((UnityEngine.Object) icon != (UnityEngine.Object) null)
          guiContent.image = icon;
        EditorGUIUtility.s_GUIContents[(object) str] = (object) guiContent;
      }
      return guiContent;
    }

    internal static GUIContent TrTextContent(string text, string tooltip, string iconName)
    {
      string str = string.Format("{0}|{1}", text == null ? (object) "" : (object) text, tooltip == null ? (object) "" : (object) tooltip);
      GUIContent guiContent = (GUIContent) EditorGUIUtility.s_GUIContents[(object) str];
      if (guiContent == null)
      {
        guiContent = new GUIContent(L10n.Tr(text));
        if (tooltip != null)
          guiContent.tooltip = L10n.Tr(tooltip);
        if (iconName != null)
        {
          Texture texture = (Texture) EditorGUIUtility.LoadIconRequired(iconName);
          guiContent.image = texture;
        }
        EditorGUIUtility.s_GUIContents[(object) str] = (object) guiContent;
      }
      return guiContent;
    }

    internal static GUIContent TrTextContent(string text, Texture icon)
    {
      return EditorGUIUtility.TrTextContent(text, (string) null, icon);
    }

    internal static GUIContent TrTextContentWithIcon(string text, Texture icon)
    {
      return EditorGUIUtility.TrTextContent(text, (string) null, icon);
    }

    internal static GUIContent TrTextContentWithIcon(string text, string iconName)
    {
      return EditorGUIUtility.TrTextContent(text, (string) null, iconName);
    }

    internal static GUIContent TrTextContentWithIcon(string text, string tooltip, string iconName)
    {
      return EditorGUIUtility.TrTextContent(text, tooltip, iconName);
    }

    internal static GUIContent TrTextContentWithIcon(string text, string tooltip, Texture icon)
    {
      return EditorGUIUtility.TrTextContent(text, tooltip, icon);
    }

    [ExcludeFromDocs]
    internal static GUIContent TrIconContent(string name)
    {
      string tooltip = (string) null;
      return EditorGUIUtility.TrIconContent(name, tooltip);
    }

    internal static GUIContent TrIconContent(string name, [DefaultValue("null")] string tooltip)
    {
      GUIContent iconGuiContent = (GUIContent) EditorGUIUtility.s_IconGUIContents[(object) name];
      if (iconGuiContent != null)
        return iconGuiContent;
      GUIContent guiContent = new GUIContent();
      if (tooltip != null)
        guiContent.tooltip = L10n.Tr(tooltip);
      guiContent.image = (Texture) EditorGUIUtility.LoadIconRequired(name);
      EditorGUIUtility.s_IconGUIContents[(object) name] = (object) guiContent;
      return guiContent;
    }

    [ExcludeFromDocs]
    internal static GUIContent TrIconContent(Texture icon)
    {
      string tooltip = (string) null;
      return EditorGUIUtility.TrIconContent(icon, tooltip);
    }

    internal static GUIContent TrIconContent(Texture icon, [DefaultValue("null")] string tooltip)
    {
      GUIContent iconGuiContent = (GUIContent) EditorGUIUtility.s_IconGUIContents[(object) tooltip];
      if (iconGuiContent != null)
        return iconGuiContent;
      GUIContent guiContent = new GUIContent();
      if (tooltip != null)
        guiContent.tooltip = L10n.Tr(tooltip);
      guiContent.image = icon;
      EditorGUIUtility.s_IconGUIContents[(object) tooltip] = (object) guiContent;
      return guiContent;
    }

    internal static string[] GetNameAndTooltipString(string nameAndTooltip)
    {
      string[] strArray1 = new string[3];
      string[] strArray2 = nameAndTooltip.Split('|');
      switch (strArray2.Length)
      {
        case 0:
          strArray1[0] = "";
          strArray1[1] = "";
          break;
        case 1:
          strArray1[0] = strArray2[0].Trim();
          strArray1[1] = strArray1[0];
          break;
        case 2:
          strArray1[0] = strArray2[0].Trim();
          strArray1[1] = strArray1[0];
          strArray1[2] = strArray2[1].Trim();
          break;
        default:
          Debug.LogError((object) ("Error in Tooltips: Too many strings in line beginning with '" + strArray2[0] + "'"));
          break;
      }
      return strArray1;
    }

    internal static Texture2D LoadIconRequired(string name)
    {
      Texture2D texture2D = EditorGUIUtility.LoadIcon(name);
      if (!(bool) ((UnityEngine.Object) texture2D))
        Debug.LogErrorFormat("Unable to load the icon: '{0}'.\nNote that either full project path should be used (with extension) or just the icon name if the icon is located in the following location: '{1}' (without extension, since png is assumed)", new object[2]
        {
          (object) name,
          (object) ("Assets/Editor Default Resources/" + EditorResourcesUtility.iconsPath)
        });
      return texture2D;
    }

    internal static Texture2D LoadIcon(string name)
    {
      return EditorGUIUtility.LoadIconForSkin(name, EditorGUIUtility.skinIndex);
    }

    private static Texture2D LoadGeneratedIconOrNormalIcon(string name)
    {
      Texture2D texture2D = EditorGUIUtility.Load(EditorResourcesUtility.generatedIconsPath + name + ".asset") as Texture2D;
      if (!(bool) ((UnityEngine.Object) texture2D))
        texture2D = EditorGUIUtility.Load(EditorResourcesUtility.iconsPath + name + ".png") as Texture2D;
      if (!(bool) ((UnityEngine.Object) texture2D))
        texture2D = EditorGUIUtility.Load(name) as Texture2D;
      return texture2D;
    }

    internal static Texture2D LoadIconForSkin(string name, int skinIndex)
    {
      if (string.IsNullOrEmpty(name))
        return (Texture2D) null;
      if (skinIndex == 0)
        return EditorGUIUtility.LoadGeneratedIconOrNormalIcon(name);
      string name1 = "d_" + Path.GetFileName(name);
      string directoryName = Path.GetDirectoryName(name);
      if (!string.IsNullOrEmpty(directoryName))
        name1 = string.Format("{0}/{1}", (object) directoryName, (object) name1);
      Texture2D texture2D = EditorGUIUtility.LoadGeneratedIconOrNormalIcon(name1);
      if (!(bool) ((UnityEngine.Object) texture2D))
        texture2D = EditorGUIUtility.LoadGeneratedIconOrNormalIcon(name);
      return texture2D;
    }

    /// <summary>
    ///   <para>Fetch the GUIContent from the Unity builtin resources with the given name.</para>
    /// </summary>
    /// <param name="name">Name of the desired icon.</param>
    /// <param name="text">Tooltip for hovering over the icon.</param>
    [ExcludeFromDocs]
    public static GUIContent IconContent(string name)
    {
      string text = (string) null;
      return EditorGUIUtility.IconContent(name, text);
    }

    /// <summary>
    ///   <para>Fetch the GUIContent from the Unity builtin resources with the given name.</para>
    /// </summary>
    /// <param name="name">Name of the desired icon.</param>
    /// <param name="text">Tooltip for hovering over the icon.</param>
    public static GUIContent IconContent(string name, [DefaultValue("null")] string text)
    {
      GUIContent iconGuiContent = (GUIContent) EditorGUIUtility.s_IconGUIContents[(object) name];
      if (iconGuiContent != null)
        return iconGuiContent;
      GUIContent guiContent = new GUIContent();
      if (text != null)
      {
        string[] andTooltipString = EditorGUIUtility.GetNameAndTooltipString(text);
        if (andTooltipString[2] != null)
          guiContent.tooltip = andTooltipString[2];
      }
      guiContent.image = (Texture) EditorGUIUtility.LoadIconRequired(name);
      EditorGUIUtility.s_IconGUIContents[(object) name] = (object) guiContent;
      return guiContent;
    }

    /// <summary>
    ///   <para>Is the user currently using the pro skin? (Read Only)</para>
    /// </summary>
    public static bool isProSkin
    {
      get
      {
        return EditorGUIUtility.skinIndex == 1;
      }
    }

    internal static extern int skinIndex { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static void Internal_SwitchSkin()
    {
      EditorGUIUtility.skinIndex = 1 - EditorGUIUtility.skinIndex;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetObjectNameWithInfo(UnityEngine.Object obj);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetTypeNameWithInfo(string typeName);

    /// <summary>
    ///   <para>Return a GUIContent object with the name and icon of an Object.</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    public static GUIContent ObjectContent(UnityEngine.Object obj, System.Type type)
    {
      if ((bool) obj)
      {
        EditorGUIUtility.s_ObjectContent.text = EditorGUIUtility.GetObjectNameWithInfo(obj);
        EditorGUIUtility.s_ObjectContent.image = (Texture) AssetPreview.GetMiniThumbnail(obj);
      }
      else if (type != null)
      {
        EditorGUIUtility.s_ObjectContent.text = EditorGUIUtility.GetTypeNameWithInfo(type.Name);
        EditorGUIUtility.s_ObjectContent.image = (Texture) AssetPreview.GetMiniTypeThumbnail(type);
      }
      else
      {
        EditorGUIUtility.s_ObjectContent.text = "<no type>";
        EditorGUIUtility.s_ObjectContent.image = (Texture) null;
      }
      return EditorGUIUtility.s_ObjectContent;
    }

    internal static GUIContent TempContent(string t)
    {
      EditorGUIUtility.s_Text.text = t;
      return EditorGUIUtility.s_Text;
    }

    internal static GUIContent TempContent(Texture i)
    {
      EditorGUIUtility.s_Image.image = i;
      return EditorGUIUtility.s_Image;
    }

    internal static GUIContent TempContent(string t, Texture i)
    {
      EditorGUIUtility.s_TextImage.image = i;
      EditorGUIUtility.s_TextImage.text = t;
      return EditorGUIUtility.s_TextImage;
    }

    internal static GUIContent[] TempContent(string[] texts)
    {
      GUIContent[] guiContentArray = new GUIContent[texts.Length];
      for (int index = 0; index < texts.Length; ++index)
        guiContentArray[index] = new GUIContent(texts[index]);
      return guiContentArray;
    }

    internal static GUIContent TrTempContent(string t)
    {
      return EditorGUIUtility.TempContent(L10n.Tr(t));
    }

    internal static bool HasHolddownKeyModifiers(Event evt)
    {
      return evt.shift | evt.control | evt.alt | evt.command;
    }

    /// <summary>
    ///   <para>Does a given class have per-object thumbnails?</para>
    /// </summary>
    /// <param name="objType"></param>
    public static bool HasObjectThumbnail(System.Type objType)
    {
      return objType != null && (objType.IsSubclassOf(typeof (Texture)) || objType == typeof (Texture) || objType == typeof (Sprite));
    }

    /// <summary>
    ///   <para>Set icons rendered as part of GUIContent to be rendered at a specific size.</para>
    /// </summary>
    /// <param name="size"></param>
    public static void SetIconSize(Vector2 size)
    {
      EditorGUIUtility.INTERNAL_CALL_SetIconSize(ref size);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetIconSize(ref Vector2 size);

    /// <summary>
    ///   <para>Get the size that has been set using SetIconSize.</para>
    /// </summary>
    public static Vector2 GetIconSize()
    {
      Vector2 size;
      EditorGUIUtility.Internal_GetIconSize(out size);
      return size;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_GetIconSize(out Vector2 size);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern UnityEngine.Object GetScript(string scriptClass);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetIconForObject(UnityEngine.Object obj, Texture2D icon);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Texture2D GetIconForObject(UnityEngine.Object obj);

    /// <summary>
    ///   <para>Get a white texture.</para>
    /// </summary>
    public static extern Texture2D whiteTexture { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static Texture2D infoIcon
    {
      get
      {
        if ((UnityEngine.Object) EditorGUIUtility.s_InfoIcon == (UnityEngine.Object) null)
          EditorGUIUtility.s_InfoIcon = EditorGUIUtility.LoadIcon("console.infoicon");
        return EditorGUIUtility.s_InfoIcon;
      }
    }

    internal static Texture2D warningIcon
    {
      get
      {
        if ((UnityEngine.Object) EditorGUIUtility.s_WarningIcon == (UnityEngine.Object) null)
          EditorGUIUtility.s_WarningIcon = EditorGUIUtility.LoadIcon("console.warnicon");
        return EditorGUIUtility.s_WarningIcon;
      }
    }

    internal static Texture2D errorIcon
    {
      get
      {
        if ((UnityEngine.Object) EditorGUIUtility.s_ErrorIcon == (UnityEngine.Object) null)
          EditorGUIUtility.s_ErrorIcon = EditorGUIUtility.LoadIcon("console.erroricon");
        return EditorGUIUtility.s_ErrorIcon;
      }
    }

    internal static Texture2D GetHelpIcon(MessageType type)
    {
      switch (type)
      {
        case MessageType.Info:
          return EditorGUIUtility.infoIcon;
        case MessageType.Warning:
          return EditorGUIUtility.warningIcon;
        case MessageType.Error:
          return EditorGUIUtility.errorIcon;
        default:
          return (Texture2D) null;
      }
    }

    internal static GUIContent blankContent
    {
      get
      {
        return EditorGUIUtility.s_BlankContent;
      }
    }

    internal static GUIStyle whiteTextureStyle
    {
      get
      {
        if (EditorGUIUtility.s_WhiteTextureStyle == null)
        {
          EditorGUIUtility.s_WhiteTextureStyle = new GUIStyle();
          EditorGUIUtility.s_WhiteTextureStyle.normal.background = EditorGUIUtility.whiteTexture;
        }
        return EditorGUIUtility.s_WhiteTextureStyle;
      }
    }

    internal static GUIStyle GetBasicTextureStyle(Texture2D tex)
    {
      if (EditorGUIUtility.s_BasicTextureStyle == null)
        EditorGUIUtility.s_BasicTextureStyle = new GUIStyle();
      EditorGUIUtility.s_BasicTextureStyle.normal.background = tex;
      return EditorGUIUtility.s_BasicTextureStyle;
    }

    internal static void NotifyLanguageChanged(SystemLanguage newLanguage)
    {
      EditorGUIUtility.s_TextGUIContents = new Hashtable();
      EditorGUIUtility.s_GUIContents = new Hashtable();
      EditorGUIUtility.s_IconGUIContents = new Hashtable();
      EditorUtility.Internal_UpdateMenuTitleForLanguage(newLanguage);
      LocalizationDatabase.SetCurrentEditorLanguage(newLanguage);
      EditorApplication.RequestRepaintAllViews();
    }

    /// <summary>
    ///   <para>Get a texture from its source filename.</para>
    /// </summary>
    /// <param name="name"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture2D FindTexture(string name);

    /// <summary>
    ///   <para>Get one of the built-in GUI skins, which can be the game view, inspector or scene view skin as chosen by the parameter.</para>
    /// </summary>
    /// <param name="skin"></param>
    public static GUISkin GetBuiltinSkin(EditorSkin skin)
    {
      return GUIUtility.GetBuiltinSkin((int) skin);
    }

    /// <summary>
    ///   <para>Load a required built-in resource.</para>
    /// </summary>
    /// <param name="path"></param>
    public static UnityEngine.Object LoadRequired(string path)
    {
      UnityEngine.Object @object = EditorGUIUtility.Load(path, typeof (UnityEngine.Object));
      if (!(bool) @object)
        Debug.LogError((object) ("Unable to find required resource at 'Editor Default Resources/" + path + "'"));
      return @object;
    }

    /// <summary>
    ///   <para>Load a built-in resource.</para>
    /// </summary>
    /// <param name="path"></param>
    public static UnityEngine.Object Load(string path)
    {
      return EditorGUIUtility.Load(path, typeof (UnityEngine.Object));
    }

    [TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
    private static UnityEngine.Object Load(string filename, System.Type type)
    {
      UnityEngine.Object object1 = AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/" + filename, type);
      if (object1 != (UnityEngine.Object) null)
        return object1;
      AssetBundle editorAssetBundle = EditorGUIUtility.GetEditorAssetBundle();
      if ((UnityEngine.Object) editorAssetBundle == (UnityEngine.Object) null)
      {
        if (Application.isBatchmode)
          return (UnityEngine.Object) null;
        throw new NullReferenceException("Failure to load editor resource asset bundle.");
      }
      UnityEngine.Object object2 = editorAssetBundle.LoadAsset(filename, type);
      if (object2 != (UnityEngine.Object) null)
        return object2;
      return AssetDatabase.LoadAssetAtPath(filename, type);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern UnityEngine.Object GetBuiltinExtraResource(System.Type type, string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern BuiltinResource[] GetBuiltinResourceList(int classID);

    /// <summary>
    ///   <para>Ping an object in the Scene like clicking it in an inspector.</para>
    /// </summary>
    /// <param name="obj">The object to be pinged.</param>
    /// <param name="targetInstanceID"></param>
    public static void PingObject(UnityEngine.Object obj)
    {
      if (!(obj != (UnityEngine.Object) null))
        return;
      EditorGUIUtility.PingObject(obj.GetInstanceID());
    }

    /// <summary>
    ///   <para>Ping an object in the Scene like clicking it in an inspector.</para>
    /// </summary>
    /// <param name="obj">The object to be pinged.</param>
    /// <param name="targetInstanceID"></param>
    public static void PingObject(int targetInstanceID)
    {
      foreach (SceneHierarchyWindow sceneHierarchyWindow in SceneHierarchyWindow.GetAllSceneHierarchyWindows())
      {
        bool ping = true;
        sceneHierarchyWindow.FrameObject(targetInstanceID, ping);
      }
      foreach (ProjectBrowser allProjectBrowser in ProjectBrowser.GetAllProjectBrowsers())
      {
        bool ping = true;
        allProjectBrowser.FrameObject(targetInstanceID, ping);
      }
    }

    internal static void MoveFocusAndScroll(bool forward)
    {
      int keyboardControl = GUIUtility.keyboardControl;
      EditorGUIUtility.Internal_MoveKeyboardFocus(forward);
      if (keyboardControl == GUIUtility.keyboardControl)
        return;
      EditorGUIUtility.RefreshScrollPosition();
    }

    internal static void RefreshScrollPosition()
    {
      Rect rect;
      if (!EditorGUIUtility.Internal_GetKeyboardRect(GUIUtility.keyboardControl, out rect))
        return;
      GUI.ScrollTo(rect);
    }

    internal static void ScrollForTabbing(bool forward)
    {
      Rect rect;
      if (!EditorGUIUtility.Internal_GetKeyboardRect(EditorGUIUtility.Internal_GetNextKeyboardControlID(forward), out rect))
        return;
      GUI.ScrollTo(rect);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_GetKeyboardRect(int id, out Rect rect);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_MoveKeyboardFocus(bool forward);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetNextKeyboardControlID(bool forward);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern AssetBundle GetEditorAssetBundle();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetRenderTextureNoViewport(RenderTexture rt);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetVisibleLayers(int layers);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetLockedLayers(int layers);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsGizmosAllowedForObject(UnityEngine.Object obj);

    internal static void ResetGUIState()
    {
      GUI.skin = (GUISkin) null;
      Color white = Color.white;
      GUI.contentColor = white;
      GUI.backgroundColor = white;
      GUI.color = !EditorApplication.isPlayingOrWillChangePlaymode ? Color.white : (Color) HostView.kPlayModeDarken;
      GUI.enabled = true;
      GUI.changed = false;
      EditorGUI.indentLevel = 0;
      EditorGUI.ClearStacks();
      EditorGUIUtility.fieldWidth = 0.0f;
      EditorGUIUtility.labelWidth = 0.0f;
      EditorGUIUtility.SetBoldDefaultFont(false);
      EditorGUIUtility.UnlockContextWidth();
      EditorGUIUtility.hierarchyMode = false;
      EditorGUIUtility.wideMode = false;
      ScriptAttributeUtility.propertyHandlerCache = (PropertyHandlerCache) null;
    }

    internal static void RenderGameViewCamerasInternal(RenderTexture target, int targetDisplay, Rect screenRect, Vector2 mousePosition, bool gizmos)
    {
      EditorGUIUtility.INTERNAL_CALL_RenderGameViewCamerasInternal(target, targetDisplay, ref screenRect, ref mousePosition, gizmos);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_RenderGameViewCamerasInternal(RenderTexture target, int targetDisplay, ref Rect screenRect, ref Vector2 mousePosition, bool gizmos);

    [Obsolete("RenderGameViewCameras is no longer supported. Consider rendering cameras manually.", true)]
    public static void RenderGameViewCameras(RenderTexture target, int targetDisplay, Rect screenRect, Vector2 mousePosition, bool gizmos)
    {
      EditorGUIUtility.INTERNAL_CALL_RenderGameViewCameras(target, targetDisplay, ref screenRect, ref mousePosition, gizmos);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_RenderGameViewCameras(RenderTexture target, int targetDisplay, ref Rect screenRect, ref Vector2 mousePosition, bool gizmos);

    /// <summary>
    ///   <para>Render all ingame cameras.</para>
    /// </summary>
    /// <param name="cameraRect">The device coordinates to render all game cameras into.</param>
    /// <param name="gizmos">Show gizmos as well.</param>
    /// <param name="gui"></param>
    /// <param name="statsRect"></param>
    [Obsolete("RenderGameViewCameras is no longer supported. Consider rendering cameras manually.", true)]
    public static void RenderGameViewCameras(Rect cameraRect, bool gizmos, bool gui)
    {
    }

    /// <summary>
    ///   <para>Render all ingame cameras.</para>
    /// </summary>
    /// <param name="cameraRect">The device coordinates to render all game cameras into.</param>
    /// <param name="gizmos">Show gizmos as well.</param>
    /// <param name="gui"></param>
    /// <param name="statsRect"></param>
    [Obsolete("RenderGameViewCameras is no longer supported. Consider rendering cameras manually.", true)]
    public static void RenderGameViewCameras(Rect cameraRect, Rect statsRect, bool gizmos, bool gui)
    {
    }

    /// <summary>
    ///   <para>Check if any enabled camera can render to a particular display.</para>
    /// </summary>
    /// <param name="displayIndex">Display index.</param>
    /// <returns>
    ///   <para>True if a camera will render to the display.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsDisplayReferencedByCameras(int displayIndex);

    /// <summary>
    ///   <para>Send an input event into the game.</para>
    /// </summary>
    /// <param name="evt"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void QueueGameViewInputEvent(Event evt);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetDefaultFont(Font font);

    private static GUIStyle GetStyle(string styleName)
    {
      GUIStyle guiStyle = GUI.skin.FindStyle(styleName) ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);
      if (guiStyle == null)
      {
        Debug.Log((object) ("Missing built-in guistyle " + styleName));
        guiStyle = GUISkin.error;
      }
      return guiStyle;
    }

    [RequiredByNativeCode]
    internal static void HandleControlID(int id)
    {
      EditorGUIUtility.s_LastControlID = id;
      EditorGUI.PrepareCurrentPrefixLabel(EditorGUIUtility.s_LastControlID);
    }

    /// <summary>
    ///   <para>Is a text field currently editing text?</para>
    /// </summary>
    public static bool editingTextField
    {
      get
      {
        return EditorGUI.RecycledTextEditor.s_ActuallyEditing;
      }
      set
      {
        EditorGUI.RecycledTextEditor.s_ActuallyEditing = value;
      }
    }

    /// <summary>
    ///   <para>Is the Editor GUI is hierarchy mode?</para>
    /// </summary>
    public static bool hierarchyMode
    {
      get
      {
        return EditorGUIUtility.s_HierarchyMode;
      }
      set
      {
        EditorGUIUtility.s_HierarchyMode = value;
      }
    }

    /// <summary>
    ///   <para>Is the Editor GUI currently in wide mode?</para>
    /// </summary>
    public static bool wideMode
    {
      get
      {
        return EditorGUIUtility.s_WideMode;
      }
      set
      {
        EditorGUIUtility.s_WideMode = value;
      }
    }

    private static float CalcContextWidth()
    {
      float num = GUIClip.GetTopRect().width;
      if ((double) num < 1.0 || (double) num >= 40000.0)
        num = EditorGUIUtility.currentViewWidth;
      return num;
    }

    internal static void LockContextWidth()
    {
      EditorGUIUtility.s_ContextWidth = EditorGUIUtility.CalcContextWidth();
    }

    internal static void UnlockContextWidth()
    {
      EditorGUIUtility.s_ContextWidth = 0.0f;
    }

    internal static float contextWidth
    {
      get
      {
        if ((double) EditorGUIUtility.s_ContextWidth > 0.0)
          return EditorGUIUtility.s_ContextWidth;
        return EditorGUIUtility.CalcContextWidth();
      }
    }

    /// <summary>
    ///   <para>The width of the GUI area for the current EditorWindow or other view.</para>
    /// </summary>
    public static float currentViewWidth
    {
      get
      {
        return GUIView.current.position.width;
      }
    }

    /// <summary>
    ///   <para>The width in pixels reserved for labels of Editor GUI controls.</para>
    /// </summary>
    public static float labelWidth
    {
      get
      {
        if ((double) EditorGUIUtility.s_LabelWidth > 0.0)
          return EditorGUIUtility.s_LabelWidth;
        if (EditorGUIUtility.s_HierarchyMode)
          return Mathf.Max((float) ((double) EditorGUIUtility.contextWidth * 0.449999988079071 - 40.0), 120f);
        return 150f;
      }
      set
      {
        EditorGUIUtility.s_LabelWidth = value;
      }
    }

    /// <summary>
    ///   <para>The minimum width in pixels reserved for the fields of Editor GUI controls.</para>
    /// </summary>
    public static float fieldWidth
    {
      get
      {
        if ((double) EditorGUIUtility.s_FieldWidth > 0.0)
          return EditorGUIUtility.s_FieldWidth;
        return 50f;
      }
      set
      {
        EditorGUIUtility.s_FieldWidth = value;
      }
    }

    /// <summary>
    ///   <para>Make all EditorGUI look like regular controls.</para>
    /// </summary>
    /// <param name="labelWidth">Width to use for prefixed labels.</param>
    /// <param name="fieldWidth">Width of text entries.</param>
    [ExcludeFromDocs]
    [Obsolete("LookLikeControls and LookLikeInspector modes are deprecated. Use EditorGUIUtility.labelWidth and EditorGUIUtility.fieldWidth to control label and field widths.")]
    public static void LookLikeControls(float labelWidth)
    {
      float fieldWidth = 0.0f;
      EditorGUIUtility.LookLikeControls(labelWidth, fieldWidth);
    }

    [ExcludeFromDocs]
    [Obsolete("LookLikeControls and LookLikeInspector modes are deprecated. Use EditorGUIUtility.labelWidth and EditorGUIUtility.fieldWidth to control label and field widths.")]
    public static void LookLikeControls()
    {
      EditorGUIUtility.LookLikeControls(0.0f, 0.0f);
    }

    /// <summary>
    ///   <para>Make all EditorGUI look like regular controls.</para>
    /// </summary>
    /// <param name="labelWidth">Width to use for prefixed labels.</param>
    /// <param name="fieldWidth">Width of text entries.</param>
    [Obsolete("LookLikeControls and LookLikeInspector modes are deprecated. Use EditorGUIUtility.labelWidth and EditorGUIUtility.fieldWidth to control label and field widths.")]
    public static void LookLikeControls([DefaultValue("0")] float labelWidth, [DefaultValue("0")] float fieldWidth)
    {
      EditorGUIUtility.fieldWidth = fieldWidth;
      EditorGUIUtility.labelWidth = labelWidth;
    }

    /// <summary>
    ///   <para>Make all EditorGUI look like simplified outline view controls.</para>
    /// </summary>
    [Obsolete("LookLikeControls and LookLikeInspector modes are deprecated.")]
    public static void LookLikeInspector()
    {
      EditorGUIUtility.fieldWidth = 0.0f;
      EditorGUIUtility.labelWidth = 0.0f;
    }

    internal static void SkinChanged()
    {
      EditorStyles.UpdateSkinCache();
    }

    internal static Rect DragZoneRect(Rect position)
    {
      return new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
    }

    internal static void SetBoldDefaultFont(bool isBold)
    {
      int num = !isBold ? 0 : 1;
      if (num == EditorGUIUtility.s_FontIsBold)
        return;
      EditorGUIUtility.SetDefaultFont(!isBold ? EditorStyles.standardFont : EditorStyles.boldFont);
      EditorGUIUtility.s_FontIsBold = num;
    }

    internal static bool GetBoldDefaultFont()
    {
      return EditorGUIUtility.s_FontIsBold == 1;
    }

    /// <summary>
    ///   <para>Creates an event that can be sent to another window.</para>
    /// </summary>
    /// <param name="commandName">The command to be sent.</param>
    public static Event CommandEvent(string commandName)
    {
      Event @event = new Event();
      EditorGUIUtility.Internal_SetupEventValues((object) @event);
      @event.type = EventType.ExecuteCommand;
      @event.commandName = commandName;
      return @event;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetupEventValues(object evt);

    /// <summary>
    ///   <para>Draw a color swatch.</para>
    /// </summary>
    /// <param name="position">The rectangle to draw the color swatch within.</param>
    /// <param name="color">The color to draw.</param>
    public static void DrawColorSwatch(Rect position, Color color)
    {
      EditorGUIUtility.DrawColorSwatch(position, color, true);
    }

    internal static void DrawColorSwatch(Rect position, Color color, bool showAlpha)
    {
      EditorGUIUtility.DrawColorSwatch(position, color, showAlpha, false);
    }

    internal static void DrawColorSwatch(Rect position, Color color, bool showAlpha, bool hdr)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color color1 = GUI.color;
      Color backgroundColor = GUI.backgroundColor;
      float a = !GUI.enabled ? 2f : 1f;
      GUI.color = !EditorGUI.showMixedValue ? new Color(color.r, color.g, color.b, a) : new Color(0.82f, 0.82f, 0.82f, a) * color1;
      GUI.backgroundColor = Color.white;
      GUIStyle whiteTextureStyle = EditorGUIUtility.whiteTextureStyle;
      whiteTextureStyle.Draw(position, false, false, false, false);
      float maxColorComponent = GUI.color.maxColorComponent;
      if (hdr && (double) maxColorComponent > 1.0)
      {
        float width = position.width / 3f;
        Rect position1 = new Rect(position.x, position.y, width, position.height);
        Rect position2 = new Rect(position.xMax - width, position.y, width, position.height);
        Color color2 = GUI.color.RGBMultiplied(1f / maxColorComponent);
        Color color3 = GUI.color;
        GUI.color = color2;
        GUIStyle basicTextureStyle = EditorGUIUtility.GetBasicTextureStyle(EditorGUIUtility.whiteTexture);
        basicTextureStyle.Draw(position1, false, false, false, false);
        basicTextureStyle.Draw(position2, false, false, false, false);
        GUI.color = color3;
        EditorGUIUtility.GetBasicTextureStyle(ColorPicker.GetGradientTextureWithAlpha0To1()).Draw(position1, false, false, false, false);
        EditorGUIUtility.GetBasicTextureStyle(ColorPicker.GetGradientTextureWithAlpha1To0()).Draw(position2, false, false, false, false);
      }
      if (!EditorGUI.showMixedValue)
      {
        if (showAlpha)
        {
          GUI.color = new Color(0.0f, 0.0f, 0.0f, a);
          float height = Mathf.Clamp(position.height * 0.2f, 2f, 20f);
          Rect position1 = new Rect(position.x, position.yMax - height, position.width, height);
          whiteTextureStyle.Draw(position1, false, false, false, false);
          GUI.color = new Color(1f, 1f, 1f, a);
          position1.width *= Mathf.Clamp01(color.a);
          whiteTextureStyle.Draw(position1, false, false, false, false);
        }
      }
      else
      {
        EditorGUI.BeginHandleMixedValueContentColor();
        whiteTextureStyle.Draw(position, EditorGUI.mixedValueContent, false, false, false, false);
        EditorGUI.EndHandleMixedValueContentColor();
      }
      GUI.color = color1;
      GUI.backgroundColor = backgroundColor;
      if (!hdr || (double) maxColorComponent <= 1.0)
        return;
      GUI.Label(new Rect(position.x, position.y, position.width - 3f, position.height), "HDR", EditorStyles.centeredGreyMiniLabel);
    }

    /// <summary>
    ///   <para>Draw a curve swatch.</para>
    /// </summary>
    /// <param name="position">The rectangle to draw the color swatch within.</param>
    /// <param name="curve">The curve to draw.</param>
    /// <param name="property">The curve to draw as a SerializedProperty.</param>
    /// <param name="color">The color to draw the curve with.</param>
    /// <param name="bgColor">The color to draw the background with.</param>
    /// <param name="curveRanges">Optional parameter to specify the range of the curve which should be included in swatch.</param>
    public static void DrawCurveSwatch(Rect position, AnimationCurve curve, SerializedProperty property, Color color, Color bgColor)
    {
      EditorGUIUtility.DrawCurveSwatchInternal(position, curve, (AnimationCurve) null, property, (SerializedProperty) null, color, bgColor, false, new Rect(), Color.clear, Color.clear);
    }

    public static void DrawCurveSwatch(Rect position, AnimationCurve curve, SerializedProperty property, Color color, Color bgColor, Color topFillColor, Color bottomFillColor)
    {
      EditorGUIUtility.DrawCurveSwatchInternal(position, curve, (AnimationCurve) null, property, (SerializedProperty) null, color, bgColor, false, new Rect(), topFillColor, bottomFillColor);
    }

    public static void DrawCurveSwatch(Rect position, AnimationCurve curve, SerializedProperty property, Color color, Color bgColor, Color topFillColor, Color bottomFillColor, Rect curveRanges)
    {
      EditorGUIUtility.DrawCurveSwatchInternal(position, curve, (AnimationCurve) null, property, (SerializedProperty) null, color, bgColor, true, curveRanges, topFillColor, bottomFillColor);
    }

    /// <summary>
    ///   <para>Draw a curve swatch.</para>
    /// </summary>
    /// <param name="position">The rectangle to draw the color swatch within.</param>
    /// <param name="curve">The curve to draw.</param>
    /// <param name="property">The curve to draw as a SerializedProperty.</param>
    /// <param name="color">The color to draw the curve with.</param>
    /// <param name="bgColor">The color to draw the background with.</param>
    /// <param name="curveRanges">Optional parameter to specify the range of the curve which should be included in swatch.</param>
    public static void DrawCurveSwatch(Rect position, AnimationCurve curve, SerializedProperty property, Color color, Color bgColor, Rect curveRanges)
    {
      EditorGUIUtility.DrawCurveSwatchInternal(position, curve, (AnimationCurve) null, property, (SerializedProperty) null, color, bgColor, true, curveRanges, Color.clear, Color.clear);
    }

    /// <summary>
    ///   <para>Draw swatch with a filled region between two SerializedProperty curves.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="property2"></param>
    /// <param name="color"></param>
    /// <param name="bgColor"></param>
    /// <param name="curveRanges"></param>
    public static void DrawRegionSwatch(Rect position, SerializedProperty property, SerializedProperty property2, Color color, Color bgColor, Rect curveRanges)
    {
      EditorGUIUtility.DrawCurveSwatchInternal(position, (AnimationCurve) null, (AnimationCurve) null, property, property2, color, bgColor, true, curveRanges, Color.clear, Color.clear);
    }

    /// <summary>
    ///   <para>Draw swatch with a filled region between two curves.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="curve"></param>
    /// <param name="curve2"></param>
    /// <param name="color"></param>
    /// <param name="bgColor"></param>
    /// <param name="curveRanges"></param>
    public static void DrawRegionSwatch(Rect position, AnimationCurve curve, AnimationCurve curve2, Color color, Color bgColor, Rect curveRanges)
    {
      EditorGUIUtility.DrawCurveSwatchInternal(position, curve, curve2, (SerializedProperty) null, (SerializedProperty) null, color, bgColor, true, curveRanges, Color.clear, Color.clear);
    }

    private static void DrawCurveSwatchInternal(Rect position, AnimationCurve curve, AnimationCurve curve2, SerializedProperty property, SerializedProperty property2, Color color, Color bgColor, bool useCurveRanges, Rect curveRanges, Color topFillColor, Color bottomFillColor)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      int num1 = (int) position.width;
      int num2 = (int) position.height;
      int maxTextureSize = SystemInfo.maxTextureSize;
      bool flag1 = num1 > maxTextureSize;
      bool flag2 = num2 > maxTextureSize;
      if (flag1)
        num1 = Mathf.Min(num1, maxTextureSize);
      if (flag2)
        num2 = Mathf.Min(num2, maxTextureSize);
      Color color1 = GUI.color;
      GUI.color = bgColor;
      EditorGUIUtility.whiteTextureStyle.Draw(position, false, false, false, false);
      GUI.color = color1;
      if (property != null && property.hasMultipleDifferentValues)
      {
        EditorGUI.BeginHandleMixedValueContentColor();
        GUI.Label(position, EditorGUI.mixedValueContent, (GUIStyle) "PreOverlayLabel");
        EditorGUI.EndHandleMixedValueContentColor();
      }
      else
      {
        Texture2D tex = (Texture2D) null;
        if (property != null)
          tex = property2 != null ? (!useCurveRanges ? AnimationCurvePreviewCache.GetPreview(num1, num2, property, property2, color, topFillColor, bottomFillColor) : AnimationCurvePreviewCache.GetPreview(num1, num2, property, property2, color, topFillColor, bottomFillColor, curveRanges)) : (!useCurveRanges ? AnimationCurvePreviewCache.GetPreview(num1, num2, property, color, topFillColor, bottomFillColor) : AnimationCurvePreviewCache.GetPreview(num1, num2, property, color, topFillColor, bottomFillColor, curveRanges));
        else if (curve != null)
          tex = curve2 != null ? (!useCurveRanges ? AnimationCurvePreviewCache.GetPreview(num1, num2, curve, curve2, color, topFillColor, bottomFillColor) : AnimationCurvePreviewCache.GetPreview(num1, num2, curve, curve2, color, topFillColor, bottomFillColor, curveRanges)) : (!useCurveRanges ? AnimationCurvePreviewCache.GetPreview(num1, num2, curve, color, topFillColor, bottomFillColor) : AnimationCurvePreviewCache.GetPreview(num1, num2, curve, color, topFillColor, bottomFillColor, curveRanges));
        GUIStyle basicTextureStyle = EditorGUIUtility.GetBasicTextureStyle(tex);
        if (!flag1)
          position.width = (float) tex.width;
        if (!flag2)
          position.height = (float) tex.height;
        basicTextureStyle.Draw(position, false, false, false, false);
      }
    }

    [Obsolete("EditorGUIUtility.RGBToHSV is obsolete. Use Color.RGBToHSV instead (UnityUpgradable) -> [UnityEngine] UnityEngine.Color.RGBToHSV(*)", true)]
    public static void RGBToHSV(Color rgbColor, out float H, out float S, out float V)
    {
      Color.RGBToHSV(rgbColor, out H, out S, out V);
    }

    [Obsolete("EditorGUIUtility.HSVToRGB is obsolete. Use Color.HSVToRGB instead (UnityUpgradable) -> [UnityEngine] UnityEngine.Color.HSVToRGB(*)", true)]
    public static Color HSVToRGB(float H, float S, float V)
    {
      return Color.HSVToRGB(H, S, V);
    }

    [Obsolete("EditorGUIUtility.HSVToRGB is obsolete. Use Color.HSVToRGB instead (UnityUpgradable) -> [UnityEngine] UnityEngine.Color.HSVToRGB(*)", true)]
    public static Color HSVToRGB(float H, float S, float V, bool hdr)
    {
      return Color.HSVToRGB(H, S, V, hdr);
    }

    /// <summary>
    ///   <para>The system copy buffer.</para>
    /// </summary>
    public new static extern string systemCopyBuffer { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static void SetPasteboardColor(Color color)
    {
      EditorGUIUtility.INTERNAL_CALL_SetPasteboardColor(ref color);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetPasteboardColor(ref Color color);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasPasteboardColor();

    internal static Color GetPasteboardColor()
    {
      Color color;
      EditorGUIUtility.INTERNAL_CALL_GetPasteboardColor(out color);
      return color;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetPasteboardColor(out Color value);

    /// <summary>
    ///   <para>Add a custom mouse pointer to a control.</para>
    /// </summary>
    /// <param name="position">The rectangle the control should be shown within.</param>
    /// <param name="mouse">The mouse cursor to use.</param>
    /// <param name="controlID">ID of a target control.</param>
    public static void AddCursorRect(Rect position, MouseCursor mouse)
    {
      EditorGUIUtility.AddCursorRect(position, mouse, 0);
    }

    /// <summary>
    ///   <para>Add a custom mouse pointer to a control.</para>
    /// </summary>
    /// <param name="position">The rectangle the control should be shown within.</param>
    /// <param name="mouse">The mouse cursor to use.</param>
    /// <param name="controlID">ID of a target control.</param>
    public static void AddCursorRect(Rect position, MouseCursor mouse, int controlID)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Rect rect = GUIClip.Unclip(position);
      Rect topmostRect = GUIClip.topmostRect;
      Rect r = Rect.MinMaxRect(Mathf.Max(rect.x, topmostRect.x), Mathf.Max(rect.y, topmostRect.y), Mathf.Min(rect.xMax, topmostRect.xMax), Mathf.Min(rect.yMax, topmostRect.yMax));
      if ((double) r.width <= 0.0 || (double) r.height <= 0.0)
        return;
      EditorGUIUtility.Internal_AddCursorRect(r, mouse, controlID);
    }

    private static void Internal_AddCursorRect(Rect r, MouseCursor m, int controlID)
    {
      EditorGUIUtility.INTERNAL_CALL_Internal_AddCursorRect(ref r, m, controlID);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_AddCursorRect(ref Rect r, MouseCursor m, int controlID);

    internal static Rect HandleHorizontalSplitter(Rect dragRect, float width, float minLeftSide, float minRightSide)
    {
      if (Event.current.type == EventType.Repaint)
        EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.SplitResizeLeftRight);
      float num = 0.0f;
      float x = EditorGUI.MouseDeltaReader(dragRect, true).x;
      if ((double) x != 0.0)
      {
        dragRect.x += x;
        num = Mathf.Clamp(dragRect.x, minLeftSide, width - minRightSide);
      }
      if ((double) dragRect.x > (double) width - (double) minRightSide)
        num = width - minRightSide;
      if ((double) num > 0.0)
        dragRect.x = num;
      return dragRect;
    }

    internal static void DrawHorizontalSplitter(Rect dragRect)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color color = GUI.color;
      GUI.color *= !EditorGUIUtility.isProSkin ? new Color(0.6f, 0.6f, 0.6f, 1.333f) : new Color(0.12f, 0.12f, 0.12f, 1.333f);
      GUI.DrawTexture(new Rect(dragRect.x - 1f, dragRect.y, 1f, dragRect.height), (Texture) EditorGUIUtility.whiteTexture);
      GUI.color = color;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void CleanCache(string text);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetSearchIndexOfControlIDList(int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetSearchIndexOfControlIDList();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CanHaveKeyboardFocus(int id);

    internal static EventType magnifyGestureEventType
    {
      get
      {
        return (EventType) 1000;
      }
    }

    internal static EventType swipeGestureEventType
    {
      get
      {
        return (EventType) 1001;
      }
    }

    internal static EventType rotateGestureEventType
    {
      get
      {
        return (EventType) 1002;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetWantsMouseJumping(int wantz);

    public static void ShowObjectPicker<T>(UnityEngine.Object obj, bool allowSceneObjects, string searchFilter, int controlID) where T : UnityEngine.Object
    {
      System.Type requiredType = typeof (T);
      ObjectSelector.get.Show(obj, requiredType, (SerializedProperty) null, allowSceneObjects);
      ObjectSelector.get.objectSelectorID = controlID;
      ObjectSelector.get.searchFilter = searchFilter;
    }

    /// <summary>
    ///   <para>The object currently selected in the object picker.</para>
    /// </summary>
    public static UnityEngine.Object GetObjectPickerObject()
    {
      return ObjectSelector.GetCurrentObject();
    }

    /// <summary>
    ///   <para>The controlID of the currently showing object picker.</para>
    /// </summary>
    public static int GetObjectPickerControlID()
    {
      return ObjectSelector.get.objectSelectorID;
    }

    internal static void RepaintCurrentWindow()
    {
      GUIUtility.CheckOnGUI();
      GUIView.current.Repaint();
    }

    internal static bool HasCurrentWindowKeyFocus()
    {
      GUIUtility.CheckOnGUI();
      return GUIView.current.hasFocus;
    }

    internal static Material GUITextureBlitColorspaceMaterial
    {
      get
      {
        if (!(bool) ((UnityEngine.Object) EditorGUIUtility.s_GUITextureBlitColorspaceMaterial))
        {
          EditorGUIUtility.s_GUITextureBlitColorspaceMaterial = new Material(EditorGUIUtility.LoadRequired("SceneView/GUITextureBlitColorspace.shader") as Shader);
          EditorGUIUtility.SetGUITextureBlitColorspaceSettings(EditorGUIUtility.s_GUITextureBlitColorspaceMaterial);
        }
        return EditorGUIUtility.s_GUITextureBlitColorspaceMaterial;
      }
    }

    internal static void SetGUITextureBlitColorspaceSettings(Material mat)
    {
      if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal && QualitySettings.activeColorSpace == ColorSpace.Linear)
        mat.SetFloat("_ConvertToGamma", 1f);
      else
        mat.SetFloat("_ConvertToGamma", 0.0f);
    }

    /// <summary>
    ///         <para>The scale of GUI points relative to screen pixels for the current view
    /// 
    /// This value is the number of screen pixels per point of interface space. For instance, 2.0 on retina displays. Note that the value may differ from one view to the next if the views are on monitors with different UI scales.</para>
    ///       </summary>
    public new static float pixelsPerPoint
    {
      get
      {
        return GUIUtility.pixelsPerPoint;
      }
    }

    /// <summary>
    ///   <para>Converts a position from point to pixel space.</para>
    /// </summary>
    /// <param name="rect">A GUI position in point space.</param>
    /// <returns>
    ///   <para>The same position in pixel space.</para>
    /// </returns>
    public static Rect PointsToPixels(Rect rect)
    {
      float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
      rect.x *= pixelsPerPoint;
      rect.y *= pixelsPerPoint;
      rect.width *= pixelsPerPoint;
      rect.height *= pixelsPerPoint;
      return rect;
    }

    /// <summary>
    ///   <para>Convert a Rect from pixel space to point space.</para>
    /// </summary>
    /// <param name="rect">A GUI rect measured in pixels.</param>
    /// <returns>
    ///   <para>A rect representing the same area in points.</para>
    /// </returns>
    public static Rect PixelsToPoints(Rect rect)
    {
      float num = 1f / EditorGUIUtility.pixelsPerPoint;
      rect.x *= num;
      rect.y *= num;
      rect.width *= num;
      rect.height *= num;
      return rect;
    }

    /// <summary>
    ///   <para>Convert a Rect from point space to pixel space.</para>
    /// </summary>
    /// <param name="position">A GUI rect measured in points.</param>
    /// <returns>
    ///   <para>A rect representing the same area in pixels.</para>
    /// </returns>
    public static Vector2 PointsToPixels(Vector2 position)
    {
      float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
      position.x *= pixelsPerPoint;
      position.y *= pixelsPerPoint;
      return position;
    }

    /// <summary>
    ///   <para>Convert a position from pixel to point space.</para>
    /// </summary>
    /// <param name="position">A GUI position in pixel space.</param>
    /// <returns>
    ///   <para>A vector representing the same position in point space.</para>
    /// </returns>
    public static Vector2 PixelsToPoints(Vector2 position)
    {
      float num = 1f / EditorGUIUtility.pixelsPerPoint;
      position.x *= num;
      position.y *= num;
      return position;
    }

    public static List<Rect> GetFlowLayoutedRects(Rect rect, GUIStyle style, float horizontalSpacing, float verticalSpacing, List<string> items)
    {
      List<Rect> rectList = new List<Rect>(items.Count);
      Vector2 position = rect.position;
      for (int index = 0; index < items.Count; ++index)
      {
        GUIContent content = EditorGUIUtility.TempContent(items[index]);
        Vector2 size = style.CalcSize(content);
        Rect rect1 = new Rect(position, size);
        if ((double) position.x + (double) size.x + (double) horizontalSpacing >= (double) rect.xMax)
        {
          position.x = rect.x;
          position.y += size.y + verticalSpacing;
          rect1.position = position;
        }
        rectList.Add(rect1);
        position.x += size.x + horizontalSpacing;
      }
      return rectList;
    }

    internal static void ShowObjectPicker<T>(UnityEngine.Object obj, bool allowSceneObjects, string searchFilter, ObjectSelectorReceiver objectSelectorReceiver) where T : UnityEngine.Object
    {
      System.Type requiredType = typeof (T);
      ObjectSelector.get.Show(obj, requiredType, (SerializedProperty) null, allowSceneObjects);
      ObjectSelector.get.objectSelectorReceiver = objectSelectorReceiver;
      ObjectSelector.get.searchFilter = searchFilter;
    }

    internal static Rect DrawEditorHeaderItems(Rect rectangle, UnityEngine.Object[] targetObjs)
    {
      if (targetObjs.Length == 0 || targetObjs.Length == 1 && targetObjs[0].GetType() == typeof (object))
        return rectangle;
      if (EditorGUIUtility.s_EditorHeaderItemsMethods == null)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EditorGUIUtility.\u003CDrawEditorHeaderItems\u003Ec__AnonStorey0 itemsCAnonStorey0 = new EditorGUIUtility.\u003CDrawEditorHeaderItems\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        itemsCAnonStorey0.targetObjTypes = new List<System.Type>();
        for (System.Type type = targetObjs[0].GetType(); type.BaseType != null; type = type.BaseType)
        {
          // ISSUE: reference to a compiler-generated field
          itemsCAnonStorey0.targetObjTypes.Add(type);
        }
        // ISSUE: reference to a compiler-generated method
        IEnumerable<MethodInfo> methodInfos = AttributeHelper.GetMethodsWithAttribute<EditorHeaderItemAttribute>(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic).FilterAndSortOnAttribute<EditorHeaderItemAttribute>(new Func<EditorHeaderItemAttribute, bool>(itemsCAnonStorey0.\u003C\u003Em__0), (Func<EditorHeaderItemAttribute, IComparable>) (a => (IComparable) a.callbackOrder));
        EditorGUIUtility.s_EditorHeaderItemsMethods = new List<EditorGUIUtility.HeaderItemDelegate>();
        foreach (MethodInfo method in methodInfos)
          EditorGUIUtility.s_EditorHeaderItemsMethods.Add((EditorGUIUtility.HeaderItemDelegate) Delegate.CreateDelegate(typeof (EditorGUIUtility.HeaderItemDelegate), method));
      }
      for (int index = 0; index < EditorGUIUtility.s_EditorHeaderItemsMethods.Count; ++index)
      {
        if (EditorGUIUtility.s_EditorHeaderItemsMethods[index](rectangle, targetObjs))
          rectangle.x -= rectangle.width;
      }
      return rectangle;
    }

    /// <summary>
    ///   <para>Disposable scope helper for GetIconSize / SetIconSize.</para>
    /// </summary>
    public class IconSizeScope : GUI.Scope
    {
      private Vector2 m_OriginalIconSize;

      /// <summary>
      ///   <para>Begin an IconSizeScope.</para>
      /// </summary>
      /// <param name="iconSizeWithinScope">Size to be used for icons rendered as GUIContent within this scope.</param>
      public IconSizeScope(Vector2 iconSizeWithinScope)
      {
        this.m_OriginalIconSize = EditorGUIUtility.GetIconSize();
        EditorGUIUtility.SetIconSize(iconSizeWithinScope);
      }

      protected override void CloseScope()
      {
        EditorGUIUtility.SetIconSize(this.m_OriginalIconSize);
      }
    }

    internal class SkinnedColor
    {
      private Color normalColor;
      private Color proColor;

      public SkinnedColor(Color color, Color proColor)
      {
        this.normalColor = color;
        this.proColor = proColor;
      }

      public SkinnedColor(Color color)
      {
        this.normalColor = color;
        this.proColor = color;
      }

      public Color color
      {
        get
        {
          if (EditorGUIUtility.isProSkin)
            return this.proColor;
          return this.normalColor;
        }
        set
        {
          if (EditorGUIUtility.isProSkin)
            this.proColor = value;
          else
            this.normalColor = value;
        }
      }

      public static implicit operator Color(EditorGUIUtility.SkinnedColor colorSkin)
      {
        return colorSkin.color;
      }
    }

    private delegate bool HeaderItemDelegate(Rect rectangle, UnityEngine.Object[] targets);
  }
}
