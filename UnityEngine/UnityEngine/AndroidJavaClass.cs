// Decompiled with JetBrains decompiler
// Type: UnityEngine.AndroidJavaClass
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>AndroidJavaClass is the Unity representation of a generic instance of java.lang.Class.</para>
  /// </summary>
  public class AndroidJavaClass : AndroidJavaObject
  {
    /// <summary>
    ///   <para>Construct an AndroidJavaClass from the class name.</para>
    /// </summary>
    /// <param name="className">Specifies the Java class name (e.g. &lt;tt&gt;java.lang.String&lt;/tt&gt;).</param>
    public AndroidJavaClass(string className)
    {
      this._AndroidJavaClass(className);
    }

    internal AndroidJavaClass(IntPtr jclass)
    {
      if (jclass == IntPtr.Zero)
        throw new Exception("JNI: Init'd AndroidJavaClass with null ptr!");
      this.m_jclass = new GlobalJavaObjectRef(jclass);
      this.m_jobject = new GlobalJavaObjectRef(IntPtr.Zero);
    }

    private void _AndroidJavaClass(string className)
    {
      this.DebugPrint("Creating AndroidJavaClass from " + className);
      using (AndroidJavaObject androidJavaObject = AndroidJavaObject.FindClass(className))
      {
        this.m_jclass = new GlobalJavaObjectRef(androidJavaObject.GetRawObject());
        this.m_jobject = new GlobalJavaObjectRef(IntPtr.Zero);
      }
    }
  }
}
