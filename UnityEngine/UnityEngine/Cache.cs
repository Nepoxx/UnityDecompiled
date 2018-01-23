// Decompiled with JetBrains decompiler
// Type: UnityEngine.Cache
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public struct Cache
  {
    private int m_Handle;

    internal int handle
    {
      get
      {
        return this.m_Handle;
      }
    }

    public static bool operator ==(Cache lhs, Cache rhs)
    {
      return lhs.handle == rhs.handle;
    }

    public static bool operator !=(Cache lhs, Cache rhs)
    {
      return lhs.handle != rhs.handle;
    }

    public override int GetHashCode()
    {
      return this.m_Handle;
    }

    public override bool Equals(object other)
    {
      if (!(other is Cache))
        return false;
      return this.handle == ((Cache) other).handle;
    }

    public bool valid
    {
      get
      {
        return Cache.Cache_IsValid(this.m_Handle);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool Cache_IsValid(int handle);

    public bool ready
    {
      get
      {
        return Cache.Cache_IsReady(this.m_Handle);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool Cache_IsReady(int handle);

    public bool readOnly
    {
      get
      {
        return Cache.Cache_IsReadonly(this.m_Handle);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool Cache_IsReadonly(int handle);

    public string path
    {
      get
      {
        return Cache.Cache_GetPath(this.m_Handle);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string Cache_GetPath(int handle);

    public int index
    {
      get
      {
        return Cache.Cache_GetIndex(this.m_Handle);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int Cache_GetIndex(int handle);

    public long spaceFree
    {
      get
      {
        return Cache.Cache_GetSpaceFree(this.m_Handle);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern long Cache_GetSpaceFree(int handle);

    public long maximumAvailableStorageSpace
    {
      get
      {
        return Cache.Cache_GetMaximumDiskSpaceAvailable(this.m_Handle);
      }
      set
      {
        Cache.Cache_SetMaximumDiskSpaceAvailable(this.m_Handle, value);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern long Cache_GetMaximumDiskSpaceAvailable(int handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Cache_SetMaximumDiskSpaceAvailable(int handle, long value);

    public long spaceOccupied
    {
      get
      {
        return Cache.Cache_GetCachingDiskSpaceUsed(this.m_Handle);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern long Cache_GetCachingDiskSpaceUsed(int handle);

    public int expirationDelay
    {
      get
      {
        return Cache.Cache_GetExpirationDelay(this.m_Handle);
      }
      set
      {
        Cache.Cache_SetExpirationDelay(this.m_Handle, value);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int Cache_GetExpirationDelay(int handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Cache_SetExpirationDelay(int handle, int value);

    public bool ClearCache()
    {
      return Cache.Cache_ClearCache(this.m_Handle);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool Cache_ClearCache(int handle);

    public bool ClearCache(int expiration)
    {
      return Cache.Cache_ClearCache_Expiration(this.m_Handle, expiration);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool Cache_ClearCache_Expiration(int handle, int expiration);
  }
}
