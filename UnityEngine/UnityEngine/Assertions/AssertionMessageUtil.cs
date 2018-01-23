// Decompiled with JetBrains decompiler
// Type: UnityEngine.Assertions.AssertionMessageUtil
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Assertions
{
  internal class AssertionMessageUtil
  {
    private const string k_Expected = "Expected:";
    private const string k_AssertionFailed = "Assertion failure.";

    public static string GetMessage(string failureMessage)
    {
      return UnityString.Format("{0} {1}", (object) "Assertion failure.", (object) failureMessage);
    }

    public static string GetMessage(string failureMessage, string expected)
    {
      return AssertionMessageUtil.GetMessage(UnityString.Format("{0}{1}{2} {3}", (object) failureMessage, (object) Environment.NewLine, (object) "Expected:", (object) expected));
    }

    public static string GetEqualityMessage(object actual, object expected, bool expectEqual)
    {
      return AssertionMessageUtil.GetMessage(UnityString.Format("Values are {0}equal.", !expectEqual ? (object) "" : (object) "not "), UnityString.Format("{0} {2} {1}", actual, expected, !expectEqual ? (object) "!=" : (object) "=="));
    }

    public static string NullFailureMessage(object value, bool expectNull)
    {
      return AssertionMessageUtil.GetMessage(UnityString.Format("Value was {0}Null", !expectNull ? (object) "" : (object) "not "), UnityString.Format("Value was {0}Null", !expectNull ? (object) "not " : (object) ""));
    }

    public static string BooleanFailureMessage(bool expected)
    {
      return AssertionMessageUtil.GetMessage("Value was " + (object) !expected, expected.ToString());
    }
  }
}
