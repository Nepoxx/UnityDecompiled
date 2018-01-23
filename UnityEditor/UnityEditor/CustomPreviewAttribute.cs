// Decompiled with JetBrains decompiler
// Type: UnityEditor.CustomPreviewAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Adds an extra preview in the Inspector for the specified type.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class CustomPreviewAttribute : Attribute
  {
    internal System.Type m_Type;

    /// <summary>
    ///   <para>Tells a DefaultPreview which class it's a preview for.</para>
    /// </summary>
    /// <param name="type">The type you want to create a custom preview for.</param>
    public CustomPreviewAttribute(System.Type type)
    {
      this.m_Type = type;
    }
  }
}
