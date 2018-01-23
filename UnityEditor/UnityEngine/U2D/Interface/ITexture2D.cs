// Decompiled with JetBrains decompiler
// Type: UnityEngine.U2D.Interface.ITexture2D
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEngine.U2D.Interface
{
  internal abstract class ITexture2D
  {
    public abstract int width { get; }

    public abstract int height { get; }

    public abstract TextureFormat format { get; }

    public abstract Color32[] GetPixels32();

    public abstract FilterMode filterMode { get; set; }

    public abstract string name { get; }

    public abstract void SetPixels(Color[] c);

    public abstract void Apply();

    public abstract float mipMapBias { get; }

    public static bool operator ==(ITexture2D t1, ITexture2D t2)
    {
      if (object.ReferenceEquals((object) t1, (object) null))
        return object.ReferenceEquals((object) t2, (object) null) || t2 == (ITexture2D) null;
      return t1.Equals((object) t2);
    }

    public static bool operator !=(ITexture2D t1, ITexture2D t2)
    {
      if (object.ReferenceEquals((object) t1, (object) null))
        return !object.ReferenceEquals((object) t2, (object) null) && t2 != (ITexture2D) null;
      return !t1.Equals((object) t2);
    }

    public override bool Equals(object other)
    {
      throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
      throw new NotImplementedException();
    }

    public static implicit operator UnityEngine.Object(ITexture2D t)
    {
      return !object.ReferenceEquals((object) t, (object) null) ? t.ToUnityObject() : (UnityEngine.Object) null;
    }

    public static implicit operator UnityEngine.Texture2D(ITexture2D t)
    {
      return !object.ReferenceEquals((object) t, (object) null) ? t.ToUnityTexture() : (UnityEngine.Texture2D) null;
    }

    protected abstract UnityEngine.Object ToUnityObject();

    protected abstract UnityEngine.Texture2D ToUnityTexture();
  }
}
