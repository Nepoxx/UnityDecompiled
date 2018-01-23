// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.Interface.IHandles
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor.U2D.Interface
{
  internal interface IHandles
  {
    Color color { get; set; }

    Matrix4x4 matrix { get; set; }

    Vector3[] MakeBezierPoints(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, int division);

    void DrawAAPolyLine(ITexture2D lineTex, float width, params Vector3[] points);

    void DrawAAPolyLine(ITexture2D lineTex, params Vector3[] points);

    void DrawLine(Vector3 p1, Vector3 p2);

    void SetDiscSectionPoints(Vector3[] dest, Vector3 center, Vector3 normal, Vector3 from, float angle, float radius);
  }
}
