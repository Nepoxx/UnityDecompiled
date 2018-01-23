// Decompiled with JetBrains decompiler
// Type: UnityEngine.TextGenerator
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class that can be used to generate text for rendering.</para>
  /// </summary>
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class TextGenerator : IDisposable
  {
    private static int s_NextId = 0;
    private static readonly Dictionary<int, WeakReference> s_Instances = new Dictionary<int, WeakReference>();
    internal IntPtr m_Ptr;
    private string m_LastString;
    private TextGenerationSettings m_LastSettings;
    private bool m_HasGenerated;
    private TextGenerationError m_LastValid;
    private readonly List<UIVertex> m_Verts;
    private readonly List<UICharInfo> m_Characters;
    private readonly List<UILineInfo> m_Lines;
    private bool m_CachedVerts;
    private bool m_CachedCharacters;
    private bool m_CachedLines;
    private int m_Id;

    /// <summary>
    ///   <para>Create a TextGenerator.</para>
    /// </summary>
    /// <param name="initialCapacity"></param>
    public TextGenerator()
      : this(50)
    {
    }

    /// <summary>
    ///   <para>Create a TextGenerator.</para>
    /// </summary>
    /// <param name="initialCapacity"></param>
    public TextGenerator(int initialCapacity)
    {
      this.m_Verts = new List<UIVertex>((initialCapacity + 1) * 4);
      this.m_Characters = new List<UICharInfo>(initialCapacity + 1);
      this.m_Lines = new List<UILineInfo>(20);
      this.Init();
      lock ((object) TextGenerator.s_Instances)
      {
        this.m_Id = TextGenerator.s_NextId++;
        TextGenerator.s_Instances.Add(this.m_Id, new WeakReference((object) this));
      }
    }

    ~TextGenerator()
    {
      ((IDisposable) this).Dispose();
    }

    void IDisposable.Dispose()
    {
      lock ((object) TextGenerator.s_Instances)
        TextGenerator.s_Instances.Remove(this.m_Id);
      this.Dispose_cpp();
    }

    [RequiredByNativeCode]
    internal static void InvalidateAll()
    {
      lock ((object) TextGenerator.s_Instances)
      {
        foreach (KeyValuePair<int, WeakReference> instance in TextGenerator.s_Instances)
        {
          WeakReference weakReference = instance.Value;
          if (weakReference.IsAlive)
            (weakReference.Target as TextGenerator).Invalidate();
        }
      }
    }

    private TextGenerationSettings ValidatedSettings(TextGenerationSettings settings)
    {
      if ((Object) settings.font != (Object) null && settings.font.dynamic)
        return settings;
      if (settings.fontSize != 0 || settings.fontStyle != FontStyle.Normal)
      {
        if ((Object) settings.font != (Object) null)
          Debug.LogWarningFormat((Object) settings.font, "Font size and style overrides are only supported for dynamic fonts. Font '{0}' is not dynamic.", (object) settings.font.name);
        settings.fontSize = 0;
        settings.fontStyle = FontStyle.Normal;
      }
      if (settings.resizeTextForBestFit)
      {
        if ((Object) settings.font != (Object) null)
          Debug.LogWarningFormat((Object) settings.font, "BestFit is only supported for dynamic fonts. Font '{0}' is not dynamic.", (object) settings.font.name);
        settings.resizeTextForBestFit = false;
      }
      return settings;
    }

    /// <summary>
    ///   <para>Mark the text generator as invalid. This will force a full text generation the next time Populate is called.</para>
    /// </summary>
    public void Invalidate()
    {
      this.m_HasGenerated = false;
    }

    public void GetCharacters(List<UICharInfo> characters)
    {
      this.GetCharactersInternal((object) characters);
    }

    public void GetLines(List<UILineInfo> lines)
    {
      this.GetLinesInternal((object) lines);
    }

    public void GetVertices(List<UIVertex> vertices)
    {
      this.GetVerticesInternal((object) vertices);
    }

    /// <summary>
    ///   <para>Given a string and settings, returns the preferred width for a container that would hold this text.</para>
    /// </summary>
    /// <param name="str">Generation text.</param>
    /// <param name="settings">Settings for generation.</param>
    /// <returns>
    ///   <para>Preferred width.</para>
    /// </returns>
    public float GetPreferredWidth(string str, TextGenerationSettings settings)
    {
      settings.horizontalOverflow = HorizontalWrapMode.Overflow;
      settings.verticalOverflow = VerticalWrapMode.Overflow;
      settings.updateBounds = true;
      this.Populate(str, settings);
      return this.rectExtents.width;
    }

    /// <summary>
    ///   <para>Given a string and settings, returns the preferred height for a container that would hold this text.</para>
    /// </summary>
    /// <param name="str">Generation text.</param>
    /// <param name="settings">Settings for generation.</param>
    /// <returns>
    ///   <para>Preferred height.</para>
    /// </returns>
    public float GetPreferredHeight(string str, TextGenerationSettings settings)
    {
      settings.verticalOverflow = VerticalWrapMode.Overflow;
      settings.updateBounds = true;
      this.Populate(str, settings);
      return this.rectExtents.height;
    }

    /// <summary>
    ///   <para>Will generate the vertices and other data for the given string with the given settings.</para>
    /// </summary>
    /// <param name="str">String to generate.</param>
    /// <param name="settings">Generation settings.</param>
    /// <param name="context">The object used as context of the error log message, if necessary.</param>
    /// <returns>
    ///   <para>True if the generation is a success, false otherwise.</para>
    /// </returns>
    public bool PopulateWithErrors(string str, TextGenerationSettings settings, GameObject context)
    {
      TextGenerationError textGenerationError = this.PopulateWithError(str, settings);
      if (textGenerationError == TextGenerationError.None)
        return true;
      if ((textGenerationError & TextGenerationError.CustomSizeOnNonDynamicFont) != TextGenerationError.None)
        Debug.LogErrorFormat((Object) context, "Font '{0}' is not dynamic, which is required to override its size", (object) settings.font);
      if ((textGenerationError & TextGenerationError.CustomStyleOnNonDynamicFont) != TextGenerationError.None)
        Debug.LogErrorFormat((Object) context, "Font '{0}' is not dynamic, which is required to override its style", (object) settings.font);
      return false;
    }

    /// <summary>
    ///   <para>Will generate the vertices and other data for the given string with the given settings.</para>
    /// </summary>
    /// <param name="str">String to generate.</param>
    /// <param name="settings">Settings.</param>
    public bool Populate(string str, TextGenerationSettings settings)
    {
      return this.PopulateWithError(str, settings) == TextGenerationError.None;
    }

    private TextGenerationError PopulateWithError(string str, TextGenerationSettings settings)
    {
      if (this.m_HasGenerated && str == this.m_LastString && settings.Equals(this.m_LastSettings))
        return this.m_LastValid;
      this.m_LastValid = this.PopulateAlways(str, settings);
      return this.m_LastValid;
    }

    private TextGenerationError PopulateAlways(string str, TextGenerationSettings settings)
    {
      this.m_LastString = str;
      this.m_HasGenerated = true;
      this.m_CachedVerts = false;
      this.m_CachedCharacters = false;
      this.m_CachedLines = false;
      this.m_LastSettings = settings;
      TextGenerationSettings generationSettings = this.ValidatedSettings(settings);
      TextGenerationError error;
      this.Populate_Internal(str, generationSettings.font, generationSettings.color, generationSettings.fontSize, generationSettings.scaleFactor, generationSettings.lineSpacing, generationSettings.fontStyle, generationSettings.richText, generationSettings.resizeTextForBestFit, generationSettings.resizeTextMinSize, generationSettings.resizeTextMaxSize, generationSettings.verticalOverflow, generationSettings.horizontalOverflow, generationSettings.updateBounds, generationSettings.textAnchor, generationSettings.generationExtents, generationSettings.pivot, generationSettings.generateOutOfBounds, generationSettings.alignByGeometry, out error);
      this.m_LastValid = error;
      return error;
    }

    /// <summary>
    ///   <para>Array of generated vertices.</para>
    /// </summary>
    public IList<UIVertex> verts
    {
      get
      {
        if (!this.m_CachedVerts)
        {
          this.GetVertices(this.m_Verts);
          this.m_CachedVerts = true;
        }
        return (IList<UIVertex>) this.m_Verts;
      }
    }

    /// <summary>
    ///   <para>Array of generated characters.</para>
    /// </summary>
    public IList<UICharInfo> characters
    {
      get
      {
        if (!this.m_CachedCharacters)
        {
          this.GetCharacters(this.m_Characters);
          this.m_CachedCharacters = true;
        }
        return (IList<UICharInfo>) this.m_Characters;
      }
    }

    /// <summary>
    ///   <para>Information about each generated text line.</para>
    /// </summary>
    public IList<UILineInfo> lines
    {
      get
      {
        if (!this.m_CachedLines)
        {
          this.GetLines(this.m_Lines);
          this.m_CachedLines = true;
        }
        return (IList<UILineInfo>) this.m_Lines;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Init();

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Dispose_cpp();

    internal bool Populate_Internal(string str, Font font, Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, VerticalWrapMode verticalOverFlow, HorizontalWrapMode horizontalOverflow, bool updateBounds, TextAnchor anchor, Vector2 extents, Vector2 pivot, bool generateOutOfBounds, bool alignByGeometry, out TextGenerationError error)
    {
      uint error1 = 0;
      if ((Object) font == (Object) null)
      {
        error = TextGenerationError.NoFont;
        return false;
      }
      bool flag = this.Populate_Internal_cpp(str, font, color, fontSize, scaleFactor, lineSpacing, style, richText, resizeTextForBestFit, resizeTextMinSize, resizeTextMaxSize, (int) verticalOverFlow, (int) horizontalOverflow, updateBounds, anchor, extents.x, extents.y, pivot.x, pivot.y, generateOutOfBounds, alignByGeometry, out error1);
      error = (TextGenerationError) error1;
      return flag;
    }

    internal bool Populate_Internal_cpp(string str, Font font, Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, int verticalOverFlow, int horizontalOverflow, bool updateBounds, TextAnchor anchor, float extentsX, float extentsY, float pivotX, float pivotY, bool generateOutOfBounds, bool alignByGeometry, out uint error)
    {
      return TextGenerator.INTERNAL_CALL_Populate_Internal_cpp(this, str, font, ref color, fontSize, scaleFactor, lineSpacing, style, richText, resizeTextForBestFit, resizeTextMinSize, resizeTextMaxSize, verticalOverFlow, horizontalOverflow, updateBounds, anchor, extentsX, extentsY, pivotX, pivotY, generateOutOfBounds, alignByGeometry, out error);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_Populate_Internal_cpp(TextGenerator self, string str, Font font, ref Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, int verticalOverFlow, int horizontalOverflow, bool updateBounds, TextAnchor anchor, float extentsX, float extentsY, float pivotX, float pivotY, bool generateOutOfBounds, bool alignByGeometry, out uint error);

    /// <summary>
    ///   <para>Extents of the generated text in rect format.</para>
    /// </summary>
    public Rect rectExtents
    {
      get
      {
        Rect rect;
        this.INTERNAL_get_rectExtents(out rect);
        return rect;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_rectExtents(out Rect value);

    /// <summary>
    ///   <para>Number of vertices generated.</para>
    /// </summary>
    public extern int vertexCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetVerticesInternal(object vertices);

    /// <summary>
    ///   <para>Returns the current UILineInfo.</para>
    /// </summary>
    /// <returns>
    ///   <para>Vertices.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern UIVertex[] GetVerticesArray();

    /// <summary>
    ///   <para>The number of characters that have been generated.</para>
    /// </summary>
    public extern int characterCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The number of characters that have been generated and are included in the visible lines.</para>
    /// </summary>
    public int characterCountVisible
    {
      get
      {
        return this.characterCount - 1;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetCharactersInternal(object characters);

    /// <summary>
    ///   <para>Returns the current UICharInfo.</para>
    /// </summary>
    /// <returns>
    ///   <para>Character information.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern UICharInfo[] GetCharactersArray();

    /// <summary>
    ///   <para>Number of text lines generated.</para>
    /// </summary>
    public extern int lineCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetLinesInternal(object lines);

    /// <summary>
    ///   <para>Returns the current UILineInfo.</para>
    /// </summary>
    /// <returns>
    ///   <para>Line information.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern UILineInfo[] GetLinesArray();

    /// <summary>
    ///   <para>The size of the font that was found if using best fit mode.</para>
    /// </summary>
    public extern int fontSizeUsedForBestFit { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
