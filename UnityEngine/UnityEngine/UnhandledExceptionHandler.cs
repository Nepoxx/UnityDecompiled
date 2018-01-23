// Decompiled with JetBrains decompiler
// Type: UnityEngine.UnhandledExceptionHandler
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  internal sealed class UnhandledExceptionHandler
  {
    [RequiredByNativeCode]
    private static void RegisterUECatcher()
    {
      AppDomain currentDomain = AppDomain.CurrentDomain;
      // ISSUE: reference to a compiler-generated field
      if (UnhandledExceptionHandler.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        UnhandledExceptionHandler.\u003C\u003Ef__mg\u0024cache0 = new UnhandledExceptionEventHandler(UnhandledExceptionHandler.HandleUnhandledException);
      }
      // ISSUE: reference to a compiler-generated field
      UnhandledExceptionEventHandler fMgCache0 = UnhandledExceptionHandler.\u003C\u003Ef__mg\u0024cache0;
      currentDomain.UnhandledException += fMgCache0;
    }

    private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
      Exception exceptionObject = args.ExceptionObject as Exception;
      if (exceptionObject != null)
      {
        UnhandledExceptionHandler.PrintException("Unhandled Exception: ", exceptionObject);
        UnhandledExceptionHandler.NativeUnhandledExceptionHandler(exceptionObject.GetType().Name, exceptionObject.Message, exceptionObject.StackTrace);
      }
      else
        UnhandledExceptionHandler.NativeUnhandledExceptionHandler((string) null, (string) null, (string) null);
    }

    private static void PrintException(string title, Exception e)
    {
      Debug.LogException(e);
      if (e.InnerException == null)
        return;
      UnhandledExceptionHandler.PrintException("Inner Exception: ", e.InnerException);
    }

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void NativeUnhandledExceptionHandler(string managedExceptionType, string managedExceptionMessage, string managedExceptionStack);
  }
}
