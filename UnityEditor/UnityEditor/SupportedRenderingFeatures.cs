// Decompiled with JetBrains decompiler
// Type: UnityEditor.SupportedRenderingFeatures
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Describes the rendering features supported by a given renderloop.</para>
  /// </summary>
  public struct SupportedRenderingFeatures
  {
    private static SupportedRenderingFeatures s_Active = new SupportedRenderingFeatures();
    /// <summary>
    ///   <para>Supported reflection probe rendering features.</para>
    /// </summary>
    public SupportedRenderingFeatures.ReflectionProbe reflectionProbe;

    /// <summary>
    ///   <para>The rendering features supported by the active renderloop.</para>
    /// </summary>
    public static SupportedRenderingFeatures active
    {
      get
      {
        return SupportedRenderingFeatures.s_Active;
      }
      set
      {
        SupportedRenderingFeatures.s_Active = value;
      }
    }

    /// <summary>
    ///   <para>Default rendering features (Read Only).</para>
    /// </summary>
    public static SupportedRenderingFeatures Default
    {
      get
      {
        return new SupportedRenderingFeatures();
      }
    }

    /// <summary>
    ///   <para>Reflection probe features.</para>
    /// </summary>
    [System.Flags]
    public enum ReflectionProbe
    {
      None = 0,
      Rotation = 1,
    }
  }
}
