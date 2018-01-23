// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchyNode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace UnityEditorInternal
{
  internal class AnimationWindowHierarchyNode : TreeViewItem
  {
    public float? topPixel = new float?();
    public int indent = 0;
    public string path;
    public System.Type animatableObjectType;
    public string propertyName;
    public EditorCurveBinding? binding;
    public AnimationWindowCurve[] curves;

    public AnimationWindowHierarchyNode(int instanceID, int depth, TreeViewItem parent, System.Type animatableObjectType, string propertyName, string path, string displayName)
      : base(instanceID, depth, parent, displayName)
    {
      this.displayName = displayName;
      this.animatableObjectType = animatableObjectType;
      this.propertyName = propertyName;
      this.path = path;
    }
  }
}
