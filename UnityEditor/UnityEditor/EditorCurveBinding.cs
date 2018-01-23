// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorCurveBinding
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Bindings;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Defines how a curve is attached to an object that it controls.</para>
  /// </summary>
  [NativeType(CodegenOptions.Custom, "MonoEditorCurveBinding")]
  public struct EditorCurveBinding
  {
    /// <summary>
    ///   <para>The transform path of the object that is animated.</para>
    /// </summary>
    public string path;
    private System.Type m_type;
    /// <summary>
    ///   <para>The property of the object that is animated.</para>
    /// </summary>
    public string propertyName;
    private int m_isPPtrCurve;
    private int m_isDiscreteCurve;
    private int m_isPhantom;
    internal int m_ClassID;
    internal int m_ScriptInstanceID;

    public bool isPPtrCurve
    {
      get
      {
        return this.m_isPPtrCurve != 0;
      }
    }

    public bool isDiscreteCurve
    {
      get
      {
        return this.m_isDiscreteCurve != 0;
      }
    }

    internal bool isPhantom
    {
      get
      {
        return this.m_isPhantom != 0;
      }
      set
      {
        this.m_isPhantom = !value ? 0 : 1;
      }
    }

    public static bool operator ==(EditorCurveBinding lhs, EditorCurveBinding rhs)
    {
      if (lhs.m_ClassID != 0 && rhs.m_ClassID != 0 && (lhs.m_ClassID != rhs.m_ClassID || lhs.m_ScriptInstanceID != rhs.m_ScriptInstanceID))
        return false;
      return lhs.m_isPPtrCurve == rhs.m_isPPtrCurve && lhs.m_isDiscreteCurve == rhs.m_isDiscreteCurve && (lhs.path == rhs.path && lhs.type == rhs.type) && lhs.propertyName == rhs.propertyName;
    }

    public static bool operator !=(EditorCurveBinding lhs, EditorCurveBinding rhs)
    {
      return !(lhs == rhs);
    }

    public override int GetHashCode()
    {
      return string.Format("{0}:{1}:{2}", (object) this.path, (object) this.type.Name, (object) this.propertyName).GetHashCode();
    }

    public override bool Equals(object other)
    {
      if (!(other is EditorCurveBinding))
        return false;
      return this == (EditorCurveBinding) other;
    }

    public System.Type type
    {
      get
      {
        return this.m_type;
      }
      set
      {
        this.m_type = value;
        this.m_ClassID = 0;
        this.m_ScriptInstanceID = 0;
      }
    }

    public static EditorCurveBinding FloatCurve(string inPath, System.Type inType, string inPropertyName)
    {
      return new EditorCurveBinding() { path = inPath, type = inType, propertyName = inPropertyName, m_isPPtrCurve = 0, m_isDiscreteCurve = 0, m_isPhantom = 0 };
    }

    public static EditorCurveBinding PPtrCurve(string inPath, System.Type inType, string inPropertyName)
    {
      return new EditorCurveBinding() { path = inPath, type = inType, propertyName = inPropertyName, m_isPPtrCurve = 1, m_isDiscreteCurve = 1, m_isPhantom = 0 };
    }
  }
}
