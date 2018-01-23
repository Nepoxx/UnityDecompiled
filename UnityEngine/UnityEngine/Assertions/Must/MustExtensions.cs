// Decompiled with JetBrains decompiler
// Type: UnityEngine.Assertions.Must.MustExtensions
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Diagnostics;

namespace UnityEngine.Assertions.Must
{
  [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
  [DebuggerStepThrough]
  public static class MustExtensions
  {
    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustBeTrue(this bool value)
    {
      UnityEngine.Assertions.Assert.IsTrue(value);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustBeTrue(this bool value, string message)
    {
      UnityEngine.Assertions.Assert.IsTrue(value, message);
    }

    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    [Conditional("UNITY_ASSERTIONS")]
    public static void MustBeFalse(this bool value)
    {
      UnityEngine.Assertions.Assert.IsFalse(value);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustBeFalse(this bool value, string message)
    {
      UnityEngine.Assertions.Assert.IsFalse(value, message);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustBeApproximatelyEqual(this float actual, float expected)
    {
      UnityEngine.Assertions.Assert.AreApproximatelyEqual(actual, expected);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustBeApproximatelyEqual(this float actual, float expected, string message)
    {
      UnityEngine.Assertions.Assert.AreApproximatelyEqual(actual, expected, message);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustBeApproximatelyEqual(this float actual, float expected, float tolerance)
    {
      UnityEngine.Assertions.Assert.AreApproximatelyEqual(actual, expected, tolerance);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustBeApproximatelyEqual(this float actual, float expected, float tolerance, string message)
    {
      UnityEngine.Assertions.Assert.AreApproximatelyEqual(expected, actual, tolerance, message);
    }

    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    [Conditional("UNITY_ASSERTIONS")]
    public static void MustNotBeApproximatelyEqual(this float actual, float expected)
    {
      UnityEngine.Assertions.Assert.AreNotApproximatelyEqual(expected, actual);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustNotBeApproximatelyEqual(this float actual, float expected, string message)
    {
      UnityEngine.Assertions.Assert.AreNotApproximatelyEqual(expected, actual, message);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustNotBeApproximatelyEqual(this float actual, float expected, float tolerance)
    {
      UnityEngine.Assertions.Assert.AreNotApproximatelyEqual(expected, actual, tolerance);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustNotBeApproximatelyEqual(this float actual, float expected, float tolerance, string message)
    {
      UnityEngine.Assertions.Assert.AreNotApproximatelyEqual(expected, actual, tolerance, message);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustBeEqual<T>(this T actual, T expected)
    {
      UnityEngine.Assertions.Assert.AreEqual<T>(actual, expected);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustBeEqual<T>(this T actual, T expected, string message)
    {
      UnityEngine.Assertions.Assert.AreEqual<T>(expected, actual, message);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustNotBeEqual<T>(this T actual, T expected)
    {
      UnityEngine.Assertions.Assert.AreNotEqual<T>(actual, expected);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustNotBeEqual<T>(this T actual, T expected, string message)
    {
      UnityEngine.Assertions.Assert.AreNotEqual<T>(expected, actual, message);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustBeNull<T>(this T expected) where T : class
    {
      UnityEngine.Assertions.Assert.IsNull<T>(expected);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustBeNull<T>(this T expected, string message) where T : class
    {
      UnityEngine.Assertions.Assert.IsNull<T>(expected, message);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustNotBeNull<T>(this T expected) where T : class
    {
      UnityEngine.Assertions.Assert.IsNotNull<T>(expected);
    }

    [Conditional("UNITY_ASSERTIONS")]
    [Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
    public static void MustNotBeNull<T>(this T expected, string message) where T : class
    {
      UnityEngine.Assertions.Assert.IsNotNull<T>(expected, message);
    }
  }
}
