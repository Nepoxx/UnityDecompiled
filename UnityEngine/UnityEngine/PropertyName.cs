// Decompiled with JetBrains decompiler
// Type: UnityEngine.PropertyName
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Text;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  public struct PropertyName
  {
    internal int id;
    internal int conflictIndex;

    public PropertyName(string name)
    {
      this = new PropertyName(PropertyNameUtils.PropertyNameFromString(name));
    }

    public PropertyName(PropertyName other)
    {
      this.id = other.id;
      this.conflictIndex = other.conflictIndex;
    }

    public PropertyName(int id)
    {
      this.id = id;
      this.conflictIndex = 0;
    }

    public static bool IsNullOrEmpty(PropertyName prop)
    {
      return prop.id == 0;
    }

    public static bool operator ==(PropertyName lhs, PropertyName rhs)
    {
      return lhs.id == rhs.id;
    }

    public static bool operator !=(PropertyName lhs, PropertyName rhs)
    {
      return lhs.id != rhs.id;
    }

    public override int GetHashCode()
    {
      return this.id;
    }

    public override bool Equals(object other)
    {
      return other is PropertyName && this == (PropertyName) other;
    }

    public static implicit operator PropertyName(string name)
    {
      return new PropertyName(name);
    }

    public static implicit operator PropertyName(int id)
    {
      return new PropertyName(id);
    }

    public override string ToString()
    {
      int num = PropertyNameUtils.ConflictCountForID(this.id);
      string str = string.Format("{0}:{1}", (object) PropertyNameUtils.StringFromPropertyName(this), (object) this.id);
      if (num > 0)
      {
        StringBuilder stringBuilder = new StringBuilder(str);
        stringBuilder.Append(" conflicts with ");
        for (int index = 0; index < num; ++index)
        {
          if (index != this.conflictIndex)
            stringBuilder.AppendFormat("\"{0}\"", (object) PropertyNameUtils.StringFromPropertyName(new PropertyName(this.id)
            {
              conflictIndex = index
            }));
        }
        str = stringBuilder.ToString();
      }
      return str;
    }
  }
}
