// Decompiled with JetBrains decompiler
// Type: UnityEngine.Analytics.CustomEventData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Analytics
{
  [StructLayout(LayoutKind.Sequential)]
  internal class CustomEventData : IDisposable
  {
    [NonSerialized]
    internal IntPtr m_Ptr;

    private CustomEventData()
    {
    }

    public CustomEventData(string name)
    {
      this.InternalCreate(name);
    }

    ~CustomEventData()
    {
      this.InternalDestroy();
    }

    public void Dispose()
    {
      this.InternalDestroy();
      GC.SuppressFinalize((object) this);
    }

    public bool Add(string key, string value)
    {
      return this.AddString(key, value);
    }

    public bool Add(string key, bool value)
    {
      return this.AddBool(key, value);
    }

    public bool Add(string key, char value)
    {
      return this.AddChar(key, value);
    }

    public bool Add(string key, byte value)
    {
      return this.AddByte(key, value);
    }

    public bool Add(string key, sbyte value)
    {
      return this.AddSByte(key, value);
    }

    public bool Add(string key, short value)
    {
      return this.AddInt16(key, value);
    }

    public bool Add(string key, ushort value)
    {
      return this.AddUInt16(key, value);
    }

    public bool Add(string key, int value)
    {
      return this.AddInt32(key, value);
    }

    public bool Add(string key, uint value)
    {
      return this.AddUInt32(key, value);
    }

    public bool Add(string key, long value)
    {
      return this.AddInt64(key, value);
    }

    public bool Add(string key, ulong value)
    {
      return this.AddUInt64(key, value);
    }

    public bool Add(string key, float value)
    {
      return this.AddDouble(key, (double) Convert.ToDecimal(value));
    }

    public bool Add(string key, double value)
    {
      return this.AddDouble(key, value);
    }

    public bool Add(string key, Decimal value)
    {
      return this.AddDouble(key, (double) Convert.ToDecimal(value));
    }

    public bool Add(IDictionary<string, object> eventData)
    {
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) eventData)
      {
        string key = keyValuePair.Key;
        object obj = keyValuePair.Value;
        if (obj == null)
        {
          this.Add(key, "null");
        }
        else
        {
          System.Type type = obj.GetType();
          if (type == typeof (string))
            this.Add(key, (string) obj);
          else if (type == typeof (char))
            this.Add(key, (char) obj);
          else if (type == typeof (sbyte))
            this.Add(key, (sbyte) obj);
          else if (type == typeof (byte))
            this.Add(key, (byte) obj);
          else if (type == typeof (short))
            this.Add(key, (short) obj);
          else if (type == typeof (ushort))
            this.Add(key, (ushort) obj);
          else if (type == typeof (int))
            this.Add(key, (int) obj);
          else if (type == typeof (uint))
            this.Add(keyValuePair.Key, (uint) obj);
          else if (type == typeof (long))
            this.Add(key, (long) obj);
          else if (type == typeof (ulong))
            this.Add(key, (ulong) obj);
          else if (type == typeof (bool))
            this.Add(key, (bool) obj);
          else if (type == typeof (float))
            this.Add(key, (float) obj);
          else if (type == typeof (double))
            this.Add(key, (double) obj);
          else if (type == typeof (Decimal))
          {
            this.Add(key, (Decimal) obj);
          }
          else
          {
            if (!type.IsValueType)
              throw new ArgumentException(string.Format("Invalid type: {0} passed", (object) type));
            this.Add(key, obj.ToString());
          }
        }
      }
      return true;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InternalCreate(string name);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void InternalDestroy();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddString(string key, string value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddBool(string key, bool value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddChar(string key, char value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddByte(string key, byte value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddSByte(string key, sbyte value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddInt16(string key, short value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddUInt16(string key, ushort value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddInt32(string key, int value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddUInt32(string key, uint value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddInt64(string key, long value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddUInt64(string key, ulong value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddDouble(string key, double value);
  }
}
