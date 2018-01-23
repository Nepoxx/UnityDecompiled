// Decompiled with JetBrains decompiler
// Type: UnityEditor.ClipAnimationInfoCurve
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Stores a curve and its name that will be used to create additionnal curves during the import process.</para>
  /// </summary>
  [NativeType(CodegenOptions = CodegenOptions.Custom, IntermediateScriptingStructName = "MonoClipAnimationInfoCurve")]
  [UsedByNativeCode]
  public struct ClipAnimationInfoCurve
  {
    /// <summary>
    ///   <para>The name of the animation curve.</para>
    /// </summary>
    public string name;
    /// <summary>
    ///   <para>The animation curve.</para>
    /// </summary>
    public AnimationCurve curve;
  }
}
