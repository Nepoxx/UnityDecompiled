// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Outline
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System.Collections.Generic;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Adds an outline to a graphic using IVertexModifier.</para>
  /// </summary>
  [AddComponentMenu("UI/Effects/Outline", 15)]
  public class Outline : Shadow
  {
    protected Outline()
    {
    }

    public override void ModifyMesh(VertexHelper vh)
    {
      if (!this.IsActive())
        return;
      List<UIVertex> uiVertexList = ListPool<UIVertex>.Get();
      vh.GetUIVertexStream(uiVertexList);
      int num = uiVertexList.Count * 5;
      if (uiVertexList.Capacity < num)
        uiVertexList.Capacity = num;
      int start1 = 0;
      int count1 = uiVertexList.Count;
      this.ApplyShadowZeroAlloc(uiVertexList, (Color32) this.effectColor, start1, uiVertexList.Count, this.effectDistance.x, this.effectDistance.y);
      int start2 = count1;
      int count2 = uiVertexList.Count;
      this.ApplyShadowZeroAlloc(uiVertexList, (Color32) this.effectColor, start2, uiVertexList.Count, this.effectDistance.x, -this.effectDistance.y);
      int start3 = count2;
      int count3 = uiVertexList.Count;
      this.ApplyShadowZeroAlloc(uiVertexList, (Color32) this.effectColor, start3, uiVertexList.Count, -this.effectDistance.x, this.effectDistance.y);
      int start4 = count3;
      int count4 = uiVertexList.Count;
      this.ApplyShadowZeroAlloc(uiVertexList, (Color32) this.effectColor, start4, uiVertexList.Count, -this.effectDistance.x, -this.effectDistance.y);
      vh.Clear();
      vh.AddUIVertexTriangleStream(uiVertexList);
      ListPool<UIVertex>.Release(uiVertexList);
    }
  }
}
