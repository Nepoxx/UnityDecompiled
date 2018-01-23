// Decompiled with JetBrains decompiler
// Type: UnityEditor.PropertyAndTargetHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class PropertyAndTargetHandler
  {
    public SerializedProperty property;
    public Object target;
    public TargetChoiceHandler.TargetChoiceMenuFunction function;

    public PropertyAndTargetHandler(SerializedProperty property, Object target, TargetChoiceHandler.TargetChoiceMenuFunction function)
    {
      this.property = property;
      this.target = target;
      this.function = function;
    }
  }
}
