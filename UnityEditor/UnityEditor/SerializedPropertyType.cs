// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializedPropertyType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Type of a SerializedProperty.</para>
  /// </summary>
  public enum SerializedPropertyType
  {
    Generic = -1,
    Integer = 0,
    Boolean = 1,
    Float = 2,
    String = 3,
    Color = 4,
    ObjectReference = 5,
    LayerMask = 6,
    Enum = 7,
    Vector2 = 8,
    Vector3 = 9,
    Vector4 = 10, // 0x0000000A
    Rect = 11, // 0x0000000B
    ArraySize = 12, // 0x0000000C
    Character = 13, // 0x0000000D
    AnimationCurve = 14, // 0x0000000E
    Bounds = 15, // 0x0000000F
    Gradient = 16, // 0x00000010
    Quaternion = 17, // 0x00000011
    ExposedReference = 18, // 0x00000012
    FixedBufferSize = 19, // 0x00000013
    Vector2Int = 20, // 0x00000014
    Vector3Int = 21, // 0x00000015
    RectInt = 22, // 0x00000016
    BoundsInt = 23, // 0x00000017
  }
}
