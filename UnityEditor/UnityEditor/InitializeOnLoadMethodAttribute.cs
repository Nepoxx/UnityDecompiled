// Decompiled with JetBrains decompiler
// Type: UnityEditor.InitializeOnLoadMethodAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Allow an editor class method to be initialized when Unity loads without action from the user.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class InitializeOnLoadMethodAttribute : Attribute
  {
  }
}
