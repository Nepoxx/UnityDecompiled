// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.VRDeviceInfoEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditorInternal.VR
{
  [NativeType(CodegenOptions = CodegenOptions.Custom)]
  [RequiredByNativeCode]
  public struct VRDeviceInfoEditor
  {
    public string deviceNameKey;
    public string deviceNameUI;
    public string externalPluginName;
    public bool supportsEditorMode;
    public bool inListByDefault;
  }
}
