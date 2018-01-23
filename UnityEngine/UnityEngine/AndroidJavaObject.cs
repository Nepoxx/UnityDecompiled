// Decompiled with JetBrains decompiler
// Type: UnityEngine.AndroidJavaObject
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Text;

namespace UnityEngine
{
  /// <summary>
  ///   <para>AndroidJavaObject is the Unity representation of a generic instance of java.lang.Object.</para>
  /// </summary>
  public class AndroidJavaObject : IDisposable
  {
    private static bool enableDebugPrints = false;
    internal GlobalJavaObjectRef m_jobject;
    internal GlobalJavaObjectRef m_jclass;
    private static AndroidJavaClass s_JavaLangClass;

    /// <summary>
    ///   <para>Construct an AndroidJavaObject based on the name of the class.</para>
    /// </summary>
    /// <param name="className">Specifies the Java class name (e.g. "&lt;tt&gt;java.lang.String&lt;tt&gt;" or "&lt;tt&gt;javalangString&lt;tt&gt;").</param>
    /// <param name="args">An array of parameters passed to the constructor.</param>
    public AndroidJavaObject(string className, params object[] args)
      : this()
    {
      this._AndroidJavaObject(className, args);
    }

    internal AndroidJavaObject(IntPtr jobject)
      : this()
    {
      if (jobject == IntPtr.Zero)
        throw new Exception("JNI: Init'd AndroidJavaObject with null ptr!");
      IntPtr objectClass = AndroidJNISafe.GetObjectClass(jobject);
      this.m_jobject = new GlobalJavaObjectRef(jobject);
      this.m_jclass = new GlobalJavaObjectRef(objectClass);
      AndroidJNISafe.DeleteLocalRef(objectClass);
    }

    internal AndroidJavaObject()
    {
    }

    /// <summary>
    ///   <para>IDisposable callback.</para>
    /// </summary>
    public void Dispose()
    {
      this._Dispose();
    }

    /// <summary>
    ///   <para>Call a Java method on an object.</para>
    /// </summary>
    /// <param name="methodName">Specifies which method to call.</param>
    /// <param name="args">An array of parameters passed to the method.</param>
    public void Call(string methodName, params object[] args)
    {
      this._Call(methodName, args);
    }

    /// <summary>
    ///   <para>Call a static Java method on a class.</para>
    /// </summary>
    /// <param name="methodName">Specifies which method to call.</param>
    /// <param name="args">An array of parameters passed to the method.</param>
    public void CallStatic(string methodName, params object[] args)
    {
      this._CallStatic(methodName, args);
    }

    public FieldType Get<FieldType>(string fieldName)
    {
      return this._Get<FieldType>(fieldName);
    }

    public void Set<FieldType>(string fieldName, FieldType val)
    {
      this._Set<FieldType>(fieldName, val);
    }

    public FieldType GetStatic<FieldType>(string fieldName)
    {
      return this._GetStatic<FieldType>(fieldName);
    }

    public void SetStatic<FieldType>(string fieldName, FieldType val)
    {
      this._SetStatic<FieldType>(fieldName, val);
    }

    /// <summary>
    ///         <para>Retrieves the raw &lt;tt&gt;jobject&lt;/tt&gt; pointer to the Java object.
    /// 
    /// Note: Using raw JNI functions requires advanced knowledge of the Android Java Native Interface (JNI). Please take note.</para>
    ///       </summary>
    public IntPtr GetRawObject()
    {
      return this._GetRawObject();
    }

    /// <summary>
    ///         <para>Retrieves the raw &lt;tt&gt;jclass&lt;/tt&gt; pointer to the Java class.
    /// 
    /// Note: Using raw JNI functions requires advanced knowledge of the Android Java Native Interface (JNI). Please take note.</para>
    ///       </summary>
    public IntPtr GetRawClass()
    {
      return this._GetRawClass();
    }

    public ReturnType Call<ReturnType>(string methodName, params object[] args)
    {
      return this._Call<ReturnType>(methodName, args);
    }

    public ReturnType CallStatic<ReturnType>(string methodName, params object[] args)
    {
      return this._CallStatic<ReturnType>(methodName, args);
    }

    protected void DebugPrint(string msg)
    {
      if (!AndroidJavaObject.enableDebugPrints)
        return;
      Debug.Log((object) msg);
    }

    protected void DebugPrint(string call, string methodName, string signature, object[] args)
    {
      if (!AndroidJavaObject.enableDebugPrints)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (object obj in args)
      {
        stringBuilder.Append(", ");
        stringBuilder.Append(obj != null ? obj.GetType().ToString() : "<null>");
      }
      Debug.Log((object) (call + "(\"" + methodName + "\"" + stringBuilder.ToString() + ") = " + signature));
    }

    private void _AndroidJavaObject(string className, params object[] args)
    {
      this.DebugPrint("Creating AndroidJavaObject from " + className);
      if (args == null)
        args = new object[1];
      using (AndroidJavaObject androidJavaObject = AndroidJavaObject.FindClass(className))
      {
        this.m_jclass = new GlobalJavaObjectRef(androidJavaObject.GetRawObject());
        jvalue[] jniArgArray = AndroidJNIHelper.CreateJNIArgArray(args);
        try
        {
          IntPtr num = AndroidJNISafe.NewObject((IntPtr) this.m_jclass, AndroidJNIHelper.GetConstructorID((IntPtr) this.m_jclass, args), jniArgArray);
          this.m_jobject = new GlobalJavaObjectRef(num);
          AndroidJNISafe.DeleteLocalRef(num);
        }
        finally
        {
          AndroidJNIHelper.DeleteJNIArgArray(args, jniArgArray);
        }
      }
    }

    ~AndroidJavaObject()
    {
      this.Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
      this.m_jobject.Dispose();
      this.m_jclass.Dispose();
    }

    protected void _Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected void _Call(string methodName, params object[] args)
    {
      if (args == null)
        args = new object[1];
      IntPtr methodId = AndroidJNIHelper.GetMethodID((IntPtr) this.m_jclass, methodName, args, false);
      jvalue[] jniArgArray = AndroidJNIHelper.CreateJNIArgArray(args);
      try
      {
        AndroidJNISafe.CallVoidMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
      }
      finally
      {
        AndroidJNIHelper.DeleteJNIArgArray(args, jniArgArray);
      }
    }

    protected ReturnType _Call<ReturnType>(string methodName, params object[] args)
    {
      if (args == null)
        args = new object[1];
      IntPtr methodId = AndroidJNIHelper.GetMethodID<ReturnType>((IntPtr) this.m_jclass, methodName, args, false);
      jvalue[] jniArgArray = AndroidJNIHelper.CreateJNIArgArray(args);
      try
      {
        if (AndroidReflection.IsPrimitive(typeof (ReturnType)))
        {
          if (typeof (ReturnType) == typeof (int))
            return (ReturnType) (ValueType) AndroidJNISafe.CallIntMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (bool))
            return (ReturnType) (ValueType) AndroidJNISafe.CallBooleanMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (byte))
            return (ReturnType) (ValueType) AndroidJNISafe.CallByteMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (short))
            return (ReturnType) (ValueType) AndroidJNISafe.CallShortMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (long))
            return (ReturnType) (ValueType) AndroidJNISafe.CallLongMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (float))
            return (ReturnType) (ValueType) AndroidJNISafe.CallFloatMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (double))
            return (ReturnType) (ValueType) AndroidJNISafe.CallDoubleMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (char))
            return (ReturnType) (ValueType) AndroidJNISafe.CallCharMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
          return default (ReturnType);
        }
        if (typeof (ReturnType) == typeof (string))
          return (ReturnType) AndroidJNISafe.CallStringMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
        if (typeof (ReturnType) == typeof (AndroidJavaClass))
        {
          IntPtr jclass = AndroidJNISafe.CallObjectMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
          return !(jclass == IntPtr.Zero) ? (ReturnType) AndroidJavaObject.AndroidJavaClassDeleteLocalRef(jclass) : default (ReturnType);
        }
        if (typeof (ReturnType) == typeof (AndroidJavaObject))
        {
          IntPtr jobject = AndroidJNISafe.CallObjectMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
          return !(jobject == IntPtr.Zero) ? (ReturnType) AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(jobject) : default (ReturnType);
        }
        if (!AndroidReflection.IsAssignableFrom(typeof (Array), typeof (ReturnType)))
          throw new Exception("JNI: Unknown return type '" + (object) typeof (ReturnType) + "'");
        IntPtr array = AndroidJNISafe.CallObjectMethod((IntPtr) this.m_jobject, methodId, jniArgArray);
        return !(array == IntPtr.Zero) ? AndroidJNIHelper.ConvertFromJNIArray<ReturnType>(array) : default (ReturnType);
      }
      finally
      {
        AndroidJNIHelper.DeleteJNIArgArray(args, jniArgArray);
      }
    }

    protected FieldType _Get<FieldType>(string fieldName)
    {
      IntPtr fieldId = AndroidJNIHelper.GetFieldID<FieldType>((IntPtr) this.m_jclass, fieldName, false);
      if (AndroidReflection.IsPrimitive(typeof (FieldType)))
      {
        if (typeof (FieldType) == typeof (int))
          return (FieldType) (ValueType) AndroidJNISafe.GetIntField((IntPtr) this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (bool))
          return (FieldType) (ValueType) AndroidJNISafe.GetBooleanField((IntPtr) this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (byte))
          return (FieldType) (ValueType) AndroidJNISafe.GetByteField((IntPtr) this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (short))
          return (FieldType) (ValueType) AndroidJNISafe.GetShortField((IntPtr) this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (long))
          return (FieldType) (ValueType) AndroidJNISafe.GetLongField((IntPtr) this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (float))
          return (FieldType) (ValueType) AndroidJNISafe.GetFloatField((IntPtr) this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (double))
          return (FieldType) (ValueType) AndroidJNISafe.GetDoubleField((IntPtr) this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (char))
          return (FieldType) (ValueType) AndroidJNISafe.GetCharField((IntPtr) this.m_jobject, fieldId);
        return default (FieldType);
      }
      if (typeof (FieldType) == typeof (string))
        return (FieldType) AndroidJNISafe.GetStringField((IntPtr) this.m_jobject, fieldId);
      if (typeof (FieldType) == typeof (AndroidJavaClass))
      {
        IntPtr objectField = AndroidJNISafe.GetObjectField((IntPtr) this.m_jobject, fieldId);
        return !(objectField == IntPtr.Zero) ? (FieldType) AndroidJavaObject.AndroidJavaClassDeleteLocalRef(objectField) : default (FieldType);
      }
      if (typeof (FieldType) == typeof (AndroidJavaObject))
      {
        IntPtr objectField = AndroidJNISafe.GetObjectField((IntPtr) this.m_jobject, fieldId);
        return !(objectField == IntPtr.Zero) ? (FieldType) AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(objectField) : default (FieldType);
      }
      if (!AndroidReflection.IsAssignableFrom(typeof (Array), typeof (FieldType)))
        throw new Exception("JNI: Unknown field type '" + (object) typeof (FieldType) + "'");
      IntPtr objectField1 = AndroidJNISafe.GetObjectField((IntPtr) this.m_jobject, fieldId);
      return !(objectField1 == IntPtr.Zero) ? AndroidJNIHelper.ConvertFromJNIArray<FieldType>(objectField1) : default (FieldType);
    }

    protected void _Set<FieldType>(string fieldName, FieldType val)
    {
      IntPtr fieldId = AndroidJNIHelper.GetFieldID<FieldType>((IntPtr) this.m_jclass, fieldName, false);
      if (AndroidReflection.IsPrimitive(typeof (FieldType)))
      {
        if (typeof (FieldType) == typeof (int))
          AndroidJNISafe.SetIntField((IntPtr) this.m_jobject, fieldId, (int) (object) val);
        else if (typeof (FieldType) == typeof (bool))
          AndroidJNISafe.SetBooleanField((IntPtr) this.m_jobject, fieldId, (bool) (object) val);
        else if (typeof (FieldType) == typeof (byte))
          AndroidJNISafe.SetByteField((IntPtr) this.m_jobject, fieldId, (byte) (object) val);
        else if (typeof (FieldType) == typeof (short))
          AndroidJNISafe.SetShortField((IntPtr) this.m_jobject, fieldId, (short) (object) val);
        else if (typeof (FieldType) == typeof (long))
          AndroidJNISafe.SetLongField((IntPtr) this.m_jobject, fieldId, (long) (object) val);
        else if (typeof (FieldType) == typeof (float))
          AndroidJNISafe.SetFloatField((IntPtr) this.m_jobject, fieldId, (float) (object) val);
        else if (typeof (FieldType) == typeof (double))
        {
          AndroidJNISafe.SetDoubleField((IntPtr) this.m_jobject, fieldId, (double) (object) val);
        }
        else
        {
          if (typeof (FieldType) != typeof (char))
            return;
          AndroidJNISafe.SetCharField((IntPtr) this.m_jobject, fieldId, (char) (object) val);
        }
      }
      else if (typeof (FieldType) == typeof (string))
        AndroidJNISafe.SetStringField((IntPtr) this.m_jobject, fieldId, (string) (object) val);
      else if (typeof (FieldType) == typeof (AndroidJavaClass))
        AndroidJNISafe.SetObjectField((IntPtr) this.m_jobject, fieldId, (IntPtr) ((AndroidJavaObject) (object) val).m_jclass);
      else if (typeof (FieldType) == typeof (AndroidJavaObject))
      {
        AndroidJNISafe.SetObjectField((IntPtr) this.m_jobject, fieldId, (IntPtr) ((AndroidJavaObject) (object) val).m_jobject);
      }
      else
      {
        if (!AndroidReflection.IsAssignableFrom(typeof (Array), typeof (FieldType)))
          throw new Exception("JNI: Unknown field type '" + (object) typeof (FieldType) + "'");
        IntPtr jniArray = AndroidJNIHelper.ConvertToJNIArray((Array) (object) val);
        AndroidJNISafe.SetObjectField((IntPtr) this.m_jclass, fieldId, jniArray);
      }
    }

    protected void _CallStatic(string methodName, params object[] args)
    {
      if (args == null)
        args = new object[1];
      IntPtr methodId = AndroidJNIHelper.GetMethodID((IntPtr) this.m_jclass, methodName, args, true);
      jvalue[] jniArgArray = AndroidJNIHelper.CreateJNIArgArray(args);
      try
      {
        AndroidJNISafe.CallStaticVoidMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
      }
      finally
      {
        AndroidJNIHelper.DeleteJNIArgArray(args, jniArgArray);
      }
    }

    protected ReturnType _CallStatic<ReturnType>(string methodName, params object[] args)
    {
      if (args == null)
        args = new object[1];
      IntPtr methodId = AndroidJNIHelper.GetMethodID<ReturnType>((IntPtr) this.m_jclass, methodName, args, true);
      jvalue[] jniArgArray = AndroidJNIHelper.CreateJNIArgArray(args);
      try
      {
        if (AndroidReflection.IsPrimitive(typeof (ReturnType)))
        {
          if (typeof (ReturnType) == typeof (int))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticIntMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (bool))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticBooleanMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (byte))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticByteMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (short))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticShortMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (long))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticLongMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (float))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticFloatMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (double))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticDoubleMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (char))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticCharMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
          return default (ReturnType);
        }
        if (typeof (ReturnType) == typeof (string))
          return (ReturnType) AndroidJNISafe.CallStaticStringMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
        if (typeof (ReturnType) == typeof (AndroidJavaClass))
        {
          IntPtr jclass = AndroidJNISafe.CallStaticObjectMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
          return !(jclass == IntPtr.Zero) ? (ReturnType) AndroidJavaObject.AndroidJavaClassDeleteLocalRef(jclass) : default (ReturnType);
        }
        if (typeof (ReturnType) == typeof (AndroidJavaObject))
        {
          IntPtr jobject = AndroidJNISafe.CallStaticObjectMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
          return !(jobject == IntPtr.Zero) ? (ReturnType) AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(jobject) : default (ReturnType);
        }
        if (!AndroidReflection.IsAssignableFrom(typeof (Array), typeof (ReturnType)))
          throw new Exception("JNI: Unknown return type '" + (object) typeof (ReturnType) + "'");
        IntPtr array = AndroidJNISafe.CallStaticObjectMethod((IntPtr) this.m_jclass, methodId, jniArgArray);
        return !(array == IntPtr.Zero) ? AndroidJNIHelper.ConvertFromJNIArray<ReturnType>(array) : default (ReturnType);
      }
      finally
      {
        AndroidJNIHelper.DeleteJNIArgArray(args, jniArgArray);
      }
    }

    protected FieldType _GetStatic<FieldType>(string fieldName)
    {
      IntPtr fieldId = AndroidJNIHelper.GetFieldID<FieldType>((IntPtr) this.m_jclass, fieldName, true);
      if (AndroidReflection.IsPrimitive(typeof (FieldType)))
      {
        if (typeof (FieldType) == typeof (int))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticIntField((IntPtr) this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (bool))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticBooleanField((IntPtr) this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (byte))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticByteField((IntPtr) this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (short))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticShortField((IntPtr) this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (long))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticLongField((IntPtr) this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (float))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticFloatField((IntPtr) this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (double))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticDoubleField((IntPtr) this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (char))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticCharField((IntPtr) this.m_jclass, fieldId);
        return default (FieldType);
      }
      if (typeof (FieldType) == typeof (string))
        return (FieldType) AndroidJNISafe.GetStaticStringField((IntPtr) this.m_jclass, fieldId);
      if (typeof (FieldType) == typeof (AndroidJavaClass))
      {
        IntPtr staticObjectField = AndroidJNISafe.GetStaticObjectField((IntPtr) this.m_jclass, fieldId);
        return !(staticObjectField == IntPtr.Zero) ? (FieldType) AndroidJavaObject.AndroidJavaClassDeleteLocalRef(staticObjectField) : default (FieldType);
      }
      if (typeof (FieldType) == typeof (AndroidJavaObject))
      {
        IntPtr staticObjectField = AndroidJNISafe.GetStaticObjectField((IntPtr) this.m_jclass, fieldId);
        return !(staticObjectField == IntPtr.Zero) ? (FieldType) AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(staticObjectField) : default (FieldType);
      }
      if (!AndroidReflection.IsAssignableFrom(typeof (Array), typeof (FieldType)))
        throw new Exception("JNI: Unknown field type '" + (object) typeof (FieldType) + "'");
      IntPtr staticObjectField1 = AndroidJNISafe.GetStaticObjectField((IntPtr) this.m_jclass, fieldId);
      return !(staticObjectField1 == IntPtr.Zero) ? AndroidJNIHelper.ConvertFromJNIArray<FieldType>(staticObjectField1) : default (FieldType);
    }

    protected void _SetStatic<FieldType>(string fieldName, FieldType val)
    {
      IntPtr fieldId = AndroidJNIHelper.GetFieldID<FieldType>((IntPtr) this.m_jclass, fieldName, true);
      if (AndroidReflection.IsPrimitive(typeof (FieldType)))
      {
        if (typeof (FieldType) == typeof (int))
          AndroidJNISafe.SetStaticIntField((IntPtr) this.m_jclass, fieldId, (int) (object) val);
        else if (typeof (FieldType) == typeof (bool))
          AndroidJNISafe.SetStaticBooleanField((IntPtr) this.m_jclass, fieldId, (bool) (object) val);
        else if (typeof (FieldType) == typeof (byte))
          AndroidJNISafe.SetStaticByteField((IntPtr) this.m_jclass, fieldId, (byte) (object) val);
        else if (typeof (FieldType) == typeof (short))
          AndroidJNISafe.SetStaticShortField((IntPtr) this.m_jclass, fieldId, (short) (object) val);
        else if (typeof (FieldType) == typeof (long))
          AndroidJNISafe.SetStaticLongField((IntPtr) this.m_jclass, fieldId, (long) (object) val);
        else if (typeof (FieldType) == typeof (float))
          AndroidJNISafe.SetStaticFloatField((IntPtr) this.m_jclass, fieldId, (float) (object) val);
        else if (typeof (FieldType) == typeof (double))
        {
          AndroidJNISafe.SetStaticDoubleField((IntPtr) this.m_jclass, fieldId, (double) (object) val);
        }
        else
        {
          if (typeof (FieldType) != typeof (char))
            return;
          AndroidJNISafe.SetStaticCharField((IntPtr) this.m_jclass, fieldId, (char) (object) val);
        }
      }
      else if (typeof (FieldType) == typeof (string))
        AndroidJNISafe.SetStaticStringField((IntPtr) this.m_jclass, fieldId, (string) (object) val);
      else if (typeof (FieldType) == typeof (AndroidJavaClass))
        AndroidJNISafe.SetStaticObjectField((IntPtr) this.m_jclass, fieldId, (IntPtr) ((AndroidJavaObject) (object) val).m_jclass);
      else if (typeof (FieldType) == typeof (AndroidJavaObject))
      {
        AndroidJNISafe.SetStaticObjectField((IntPtr) this.m_jclass, fieldId, (IntPtr) ((AndroidJavaObject) (object) val).m_jobject);
      }
      else
      {
        if (!AndroidReflection.IsAssignableFrom(typeof (Array), typeof (FieldType)))
          throw new Exception("JNI: Unknown field type '" + (object) typeof (FieldType) + "'");
        IntPtr jniArray = AndroidJNIHelper.ConvertToJNIArray((Array) (object) val);
        AndroidJNISafe.SetStaticObjectField((IntPtr) this.m_jclass, fieldId, jniArray);
      }
    }

    internal static AndroidJavaObject AndroidJavaObjectDeleteLocalRef(IntPtr jobject)
    {
      try
      {
        return new AndroidJavaObject(jobject);
      }
      finally
      {
        AndroidJNISafe.DeleteLocalRef(jobject);
      }
    }

    internal static AndroidJavaClass AndroidJavaClassDeleteLocalRef(IntPtr jclass)
    {
      try
      {
        return new AndroidJavaClass(jclass);
      }
      finally
      {
        AndroidJNISafe.DeleteLocalRef(jclass);
      }
    }

    protected IntPtr _GetRawObject()
    {
      return (IntPtr) this.m_jobject;
    }

    protected IntPtr _GetRawClass()
    {
      return (IntPtr) this.m_jclass;
    }

    protected static AndroidJavaObject FindClass(string name)
    {
      return AndroidJavaObject.JavaLangClass.CallStatic<AndroidJavaObject>("forName", new object[1]{ (object) name.Replace('/', '.') });
    }

    protected static AndroidJavaClass JavaLangClass
    {
      get
      {
        if (AndroidJavaObject.s_JavaLangClass == null)
          AndroidJavaObject.s_JavaLangClass = new AndroidJavaClass(AndroidJNISafe.FindClass("java/lang/Class"));
        return AndroidJavaObject.s_JavaLangClass;
      }
    }
  }
}
