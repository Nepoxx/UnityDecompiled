// Decompiled with JetBrains decompiler
// Type: UnityEditor.WebGLExceptionSupport
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Options for Exception support in WebGL.</para>
  /// </summary>
  public enum WebGLExceptionSupport
  {
    None,
    ExplicitlyThrownExceptionsOnly,
    FullWithoutStacktrace,
    FullWithStacktrace,
  }
}
