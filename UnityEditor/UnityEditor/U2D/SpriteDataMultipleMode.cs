// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.SpriteDataMultipleMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.U2D
{
  internal class SpriteDataMultipleMode : SpriteDataSingleMode
  {
    public void Load(SerializedProperty sp)
    {
      this.rect = sp.FindPropertyRelative("m_Rect").rectValue;
      this.border = sp.FindPropertyRelative("m_Border").vector4Value;
      this.name = sp.FindPropertyRelative("m_Name").stringValue;
      this.alignment = (SpriteAlignment) sp.FindPropertyRelative("m_Alignment").intValue;
      this.pivot = SpriteEditorUtility.GetPivotValue(this.alignment, sp.FindPropertyRelative("m_Pivot").vector2Value);
      this.tessellationDetail = sp.FindPropertyRelative("m_TessellationDetail").floatValue;
      this.outline = SpriteDataSingleMode.AcquireOutline(sp.FindPropertyRelative("m_Outline"));
      this.physicsShape = SpriteDataSingleMode.AcquireOutline(sp.FindPropertyRelative("m_PhysicsShape"));
    }

    public void Apply(SerializedProperty sp)
    {
      sp.FindPropertyRelative("m_Rect").rectValue = this.rect;
      sp.FindPropertyRelative("m_Border").vector4Value = this.border;
      sp.FindPropertyRelative("m_Name").stringValue = this.name;
      sp.FindPropertyRelative("m_Alignment").intValue = (int) this.alignment;
      sp.FindPropertyRelative("m_Pivot").vector2Value = this.pivot;
      sp.FindPropertyRelative("m_TessellationDetail").floatValue = this.tessellationDetail;
      SerializedProperty propertyRelative1 = sp.FindPropertyRelative("m_Outline");
      propertyRelative1.ClearArray();
      if (this.outline != null)
        SpriteDataSingleMode.ApplyOutlineChanges(propertyRelative1, this.outline);
      SerializedProperty propertyRelative2 = sp.FindPropertyRelative("m_PhysicsShape");
      propertyRelative2.ClearArray();
      if (this.physicsShape == null)
        return;
      SpriteDataSingleMode.ApplyOutlineChanges(propertyRelative2, this.physicsShape);
    }
  }
}
