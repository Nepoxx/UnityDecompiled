// Decompiled with JetBrains decompiler
// Type: UnityEngine.UnityEventQueueSystem
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  public sealed class UnityEventQueueSystem
  {
    public static string GenerateEventIdForPayload(string eventPayloadName)
    {
      byte[] byteArray = Guid.NewGuid().ToByteArray();
      return string.Format("REGISTER_EVENT_ID(0x{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}{6:X2}{7:X2}ULL,0x{8:X2}{9:X2}{10:X2}{11:X2}{12:X2}{13:X2}{14:X2}{15:X2}ULL,{16})", (object) byteArray[0], (object) byteArray[1], (object) byteArray[2], (object) byteArray[3], (object) byteArray[4], (object) byteArray[5], (object) byteArray[6], (object) byteArray[7], (object) byteArray[8], (object) byteArray[9], (object) byteArray[10], (object) byteArray[11], (object) byteArray[12], (object) byteArray[13], (object) byteArray[14], (object) byteArray[15], (object) eventPayloadName);
    }

    public static IntPtr GetGlobalEventQueue()
    {
      IntPtr num;
      UnityEventQueueSystem.INTERNAL_CALL_GetGlobalEventQueue(out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetGlobalEventQueue(out IntPtr value);
  }
}
