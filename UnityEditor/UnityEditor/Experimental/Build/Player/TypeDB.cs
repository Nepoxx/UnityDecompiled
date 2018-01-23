// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.Build.Player.TypeDB
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace UnityEditor.Experimental.Build.Player
{
  [Serializable]
  public class TypeDB : ISerializable, IDisposable
  {
    private IntPtr m_Ptr;

    internal TypeDB()
    {
      this.m_Ptr = TypeDB.Internal_Create();
    }

    protected TypeDB(SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
    {
      this.m_Ptr = TypeDB.Internal_Create();
      this.DeserializeNativeTypeDB((string) info.GetValue("typedb", typeof (string)));
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IntPtr Internal_Create();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Destroy(IntPtr ptr);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int GetHash();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern string SerializeNativeTypeDB();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void DeserializeNativeTypeDB(string data);

    ~TypeDB()
    {
      this.Dispose();
    }

    public void Dispose()
    {
      if (!(this.m_Ptr != IntPtr.Zero))
        return;
      TypeDB.Internal_Destroy(this.m_Ptr);
      this.m_Ptr = IntPtr.Zero;
    }

    public override bool Equals(object obj)
    {
      TypeDB typeDb = obj as TypeDB;
      if (typeDb == null)
        return false;
      return typeDb.GetHash() == this.GetHash();
    }

    public override int GetHashCode()
    {
      return this.GetHash();
    }

    public void GetObjectData(SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
    {
      string str = this.SerializeNativeTypeDB();
      info.AddValue("typedb", (object) str);
    }
  }
}
