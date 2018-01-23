// Decompiled with JetBrains decompiler
// Type: UnityEngine.StackTraceUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Text;
using UnityEngine.Scripting;

namespace UnityEngine
{
  public static class StackTraceUtility
  {
    private static string projectFolder = "";

    [RequiredByNativeCode]
    internal static void SetProjectFolder(string folder)
    {
      StackTraceUtility.projectFolder = folder;
      if (string.IsNullOrEmpty(StackTraceUtility.projectFolder))
        return;
      StackTraceUtility.projectFolder = StackTraceUtility.projectFolder.Replace("\\", "/");
    }

    [SecuritySafeCritical]
    [RequiredByNativeCode]
    public static string ExtractStackTrace()
    {
      return StackTraceUtility.ExtractFormattedStackTrace(new StackTrace(1, true)).ToString();
    }

    private static bool IsSystemStacktraceType(object name)
    {
      string str = (string) name;
      return str.StartsWith("UnityEditor.") || str.StartsWith("UnityEngine.") || (str.StartsWith("System.") || str.StartsWith("UnityScript.Lang.")) || str.StartsWith("Boo.Lang.") || str.StartsWith("UnityEngine.SetupCoroutine");
    }

    public static string ExtractStringFromException(object exception)
    {
      string message = "";
      string stackTrace = "";
      StackTraceUtility.ExtractStringFromExceptionInternal(exception, out message, out stackTrace);
      return message + "\n" + stackTrace;
    }

    [RequiredByNativeCode]
    [SecuritySafeCritical]
    internal static void ExtractStringFromExceptionInternal(object exceptiono, out string message, out string stackTrace)
    {
      if (exceptiono == null)
        throw new ArgumentException("ExtractStringFromExceptionInternal called with null exception");
      Exception exception = exceptiono as Exception;
      if (exception == null)
        throw new ArgumentException("ExtractStringFromExceptionInternal called with an exceptoin that was not of type System.Exception");
      StringBuilder stringBuilder = new StringBuilder(exception.StackTrace != null ? exception.StackTrace.Length * 2 : 512);
      message = "";
      string str1 = "";
      for (; exception != null; exception = exception.InnerException)
      {
        str1 = str1.Length != 0 ? exception.StackTrace + "\n" + str1 : exception.StackTrace;
        string str2 = exception.GetType().Name;
        string str3 = "";
        if (exception.Message != null)
          str3 = exception.Message;
        if (str3.Trim().Length != 0)
          str2 = str2 + ": " + str3;
        message = str2;
        if (exception.InnerException != null)
          str1 = "Rethrow as " + str2 + "\n" + str1;
      }
      stringBuilder.Append(str1 + "\n");
      StackTrace stackTrace1 = new StackTrace(1, true);
      stringBuilder.Append(StackTraceUtility.ExtractFormattedStackTrace(stackTrace1));
      stackTrace = stringBuilder.ToString();
    }

    [RequiredByNativeCode]
    internal static string PostprocessStacktrace(string oldString, bool stripEngineInternalInformation)
    {
      if (oldString == null)
        return string.Empty;
      string[] strArray = oldString.Split('\n');
      StringBuilder stringBuilder = new StringBuilder(oldString.Length);
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = strArray[index].Trim();
      for (int index = 0; index < strArray.Length; ++index)
      {
        string str1 = strArray[index];
        if (str1.Length != 0 && (int) str1[0] != 10 && !str1.StartsWith("in (unmanaged)"))
        {
          if (!stripEngineInternalInformation || !str1.StartsWith("UnityEditor.EditorGUIUtility:RenderGameViewCameras"))
          {
            if (stripEngineInternalInformation && index < strArray.Length - 1 && StackTraceUtility.IsSystemStacktraceType((object) str1))
            {
              if (!StackTraceUtility.IsSystemStacktraceType((object) strArray[index + 1]))
              {
                int length = str1.IndexOf(" (at");
                if (length != -1)
                  str1 = str1.Substring(0, length);
              }
              else
                continue;
            }
            if (str1.IndexOf("(wrapper managed-to-native)") == -1 && str1.IndexOf("(wrapper delegate-invoke)") == -1 && str1.IndexOf("at <0x00000> <unknown method>") == -1 && (!stripEngineInternalInformation || !str1.StartsWith("[") || !str1.EndsWith("]")))
            {
              if (str1.StartsWith("at "))
                str1 = str1.Remove(0, 3);
              int startIndex1 = str1.IndexOf("[0x");
              int num = -1;
              if (startIndex1 != -1)
                num = str1.IndexOf("]", startIndex1);
              if (startIndex1 != -1 && num > startIndex1)
                str1 = str1.Remove(startIndex1, num - startIndex1 + 1);
              string str2 = str1.Replace("  in <filename unknown>:0", "").Replace("\\", "/");
              if (!string.IsNullOrEmpty(StackTraceUtility.projectFolder))
                str2 = str2.Replace(StackTraceUtility.projectFolder, "");
              string str3 = str2.Replace('\\', '/');
              int startIndex2 = str3.LastIndexOf("  in ");
              if (startIndex2 != -1)
              {
                string str4 = str3.Remove(startIndex2, 5).Insert(startIndex2, " (at ");
                str3 = str4.Insert(str4.Length, ")");
              }
              stringBuilder.Append(str3 + "\n");
            }
          }
          else
            break;
        }
      }
      return stringBuilder.ToString();
    }

    [SecuritySafeCritical]
    internal static string ExtractFormattedStackTrace(StackTrace stackTrace)
    {
      StringBuilder stringBuilder = new StringBuilder((int) byte.MaxValue);
      for (int index1 = 0; index1 < stackTrace.FrameCount; ++index1)
      {
        StackFrame frame = stackTrace.GetFrame(index1);
        MethodBase method = frame.GetMethod();
        if (method != null)
        {
          System.Type declaringType = method.DeclaringType;
          if (declaringType != null)
          {
            string str1 = declaringType.Namespace;
            if (str1 != null && str1.Length != 0)
            {
              stringBuilder.Append(str1);
              stringBuilder.Append(".");
            }
            stringBuilder.Append(declaringType.Name);
            stringBuilder.Append(":");
            stringBuilder.Append(method.Name);
            stringBuilder.Append("(");
            int index2 = 0;
            ParameterInfo[] parameters = method.GetParameters();
            bool flag = true;
            for (; index2 < parameters.Length; ++index2)
            {
              if (!flag)
                stringBuilder.Append(", ");
              else
                flag = false;
              stringBuilder.Append(parameters[index2].ParameterType.Name);
            }
            stringBuilder.Append(")");
            string str2 = frame.GetFileName();
            if (str2 != null && ((!(declaringType.Name == "Debug") || !(declaringType.Namespace == "UnityEngine")) && (!(declaringType.Name == "Logger") || !(declaringType.Namespace == "UnityEngine")) && ((!(declaringType.Name == "DebugLogHandler") || !(declaringType.Namespace == "UnityEngine")) && (!(declaringType.Name == "Assert") || !(declaringType.Namespace == "UnityEngine.Assertions"))) && (!(method.Name == "print") || !(declaringType.Name == "MonoBehaviour") || !(declaringType.Namespace == "UnityEngine"))))
            {
              stringBuilder.Append(" (at ");
              if (!string.IsNullOrEmpty(StackTraceUtility.projectFolder) && str2.Replace("\\", "/").StartsWith(StackTraceUtility.projectFolder))
                str2 = str2.Substring(StackTraceUtility.projectFolder.Length, str2.Length - StackTraceUtility.projectFolder.Length);
              stringBuilder.Append(str2);
              stringBuilder.Append(":");
              stringBuilder.Append(frame.GetFileLineNumber().ToString());
              stringBuilder.Append(")");
            }
            stringBuilder.Append("\n");
          }
        }
      }
      return stringBuilder.ToString();
    }
  }
}
