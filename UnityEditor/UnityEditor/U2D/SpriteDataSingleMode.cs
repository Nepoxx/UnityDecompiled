// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.SpriteDataSingleMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.Experimental.U2D;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.U2D
{
  internal class SpriteDataSingleMode : SpriteDataBase
  {
    public void Apply(SerializedObject so)
    {
      so.FindProperty("m_Alignment").intValue = (int) this.alignment;
      so.FindProperty("m_SpriteBorder").vector4Value = this.border;
      so.FindProperty("m_SpritePivot").vector2Value = this.pivot;
      so.FindProperty("m_SpriteTessellationDetail").floatValue = this.tessellationDetail;
      SerializedProperty property1 = so.FindProperty("m_SpriteSheet.m_Outline");
      if (this.outline != null)
        SpriteDataSingleMode.ApplyOutlineChanges(property1, this.outline);
      else
        property1.ClearArray();
      SerializedProperty property2 = so.FindProperty("m_SpriteSheet.m_PhysicsShape");
      if (this.physicsShape != null)
        SpriteDataSingleMode.ApplyOutlineChanges(property2, this.physicsShape);
      else
        property2.ClearArray();
    }

    public void Load(SerializedObject so)
    {
      TextureImporter targetObject = so.targetObject as TextureImporter;
      this.name = targetObject.name;
      this.alignment = (SpriteAlignment) so.FindProperty("m_Alignment").intValue;
      this.border = targetObject.spriteBorder;
      this.pivot = SpriteEditorUtility.GetPivotValue(this.alignment, targetObject.spritePivot);
      this.tessellationDetail = so.FindProperty("m_SpriteTessellationDetail").floatValue;
      this.outline = SpriteDataSingleMode.AcquireOutline(so.FindProperty("m_SpriteSheet.m_Outline"));
      this.physicsShape = SpriteDataSingleMode.AcquireOutline(so.FindProperty("m_SpriteSheet.m_PhysicsShape"));
      Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(targetObject.assetPath);
      this.rect = new Rect(0.0f, 0.0f, (float) texture2D.width, (float) texture2D.height);
    }

    protected static List<Vector2[]> AcquireOutline(SerializedProperty outlineSP)
    {
      List<Vector2[]> vector2ArrayList = new List<Vector2[]>();
      for (int index1 = 0; index1 < outlineSP.arraySize; ++index1)
      {
        SerializedProperty arrayElementAtIndex = outlineSP.GetArrayElementAtIndex(index1);
        Vector2[] vector2Array = new Vector2[arrayElementAtIndex.arraySize];
        for (int index2 = 0; index2 < arrayElementAtIndex.arraySize; ++index2)
          vector2Array[index2] = arrayElementAtIndex.GetArrayElementAtIndex(index2).vector2Value;
        vector2ArrayList.Add(vector2Array);
      }
      return vector2ArrayList;
    }

    protected static void ApplyOutlineChanges(SerializedProperty outlineSP, List<Vector2[]> outline)
    {
      outlineSP.ClearArray();
      for (int index1 = 0; index1 < outline.Count; ++index1)
      {
        outlineSP.InsertArrayElementAtIndex(index1);
        Vector2[] vector2Array = outline[index1];
        SerializedProperty arrayElementAtIndex = outlineSP.GetArrayElementAtIndex(index1);
        arrayElementAtIndex.ClearArray();
        for (int index2 = 0; index2 < vector2Array.Length; ++index2)
        {
          arrayElementAtIndex.InsertArrayElementAtIndex(index2);
          arrayElementAtIndex.GetArrayElementAtIndex(index2).vector2Value = vector2Array[index2];
        }
      }
    }

    public override SpriteAlignment alignment { get; set; }

    public override Vector4 border { get; set; }

    public override string name { get; set; }

    public override List<Vector2[]> outline { get; set; }

    public override List<Vector2[]> physicsShape { get; set; }

    public override Vector2 pivot { get; set; }

    public override Rect rect { get; set; }

    public override float tessellationDetail { get; set; }
  }
}
