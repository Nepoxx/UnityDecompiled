// Decompiled with JetBrains decompiler
// Type: UnityEditor.TransformMaskElement
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [UsedByNativeCode]
  [NativeType(CodegenOptions = CodegenOptions.Custom, Header = "Runtime/Animation/AvatarMask.h", IntermediateScriptingStructName = "MonoTransformMaskElement")]
  internal struct TransformMaskElement
  {
    public string path;
    public float weight;
  }
}
