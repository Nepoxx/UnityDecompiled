// Decompiled with JetBrains decompiler
// Type: UnityEngine.AndroidJavaProxy
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Reflection;

namespace UnityEngine
{
  public class AndroidJavaProxy
  {
    private static readonly GlobalJavaObjectRef s_JavaLangSystemClass = new GlobalJavaObjectRef(AndroidJNISafe.FindClass("java/lang/System"));
    private static readonly IntPtr s_HashCodeMethodID = AndroidJNIHelper.GetMethodID((IntPtr) AndroidJavaProxy.s_JavaLangSystemClass, "identityHashCode", "(Ljava/lang/Object;)I", true);
    public readonly AndroidJavaClass javaInterface;
    internal AndroidJavaObject proxyObject;

    public AndroidJavaProxy(string javaInterface)
      : this(new AndroidJavaClass(javaInterface))
    {
    }

    public AndroidJavaProxy(AndroidJavaClass javaInterface)
    {
      this.javaInterface = javaInterface;
    }

    public virtual AndroidJavaObject Invoke(string methodName, object[] args)
    {
      Exception inner = (Exception) null;
      BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
      System.Type[] types = new System.Type[args.Length];
      for (int index = 0; index < args.Length; ++index)
        types[index] = args[index] != null ? args[index].GetType() : typeof (AndroidJavaObject);
      try
      {
        MethodInfo method = this.GetType().GetMethod(methodName, bindingAttr, (Binder) null, types, (ParameterModifier[]) null);
        if (method != null)
          return _AndroidJNIHelper.Box(method.Invoke((object) this, args));
      }
      catch (TargetInvocationException ex)
      {
        inner = ex.InnerException;
      }
      catch (Exception ex)
      {
        inner = ex;
      }
      string[] strArray = new string[args.Length];
      for (int index = 0; index < types.Length; ++index)
        strArray[index] = types[index].ToString();
      if (inner != null)
        throw new TargetInvocationException(this.GetType().ToString() + "." + methodName + "(" + string.Join(",", strArray) + ")", inner);
      throw new Exception("No such proxy method: " + (object) this.GetType() + "." + methodName + "(" + string.Join(",", strArray) + ")");
    }

    public virtual AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
    {
      object[] args = new object[javaArgs.Length];
      for (int index = 0; index < javaArgs.Length; ++index)
        args[index] = _AndroidJNIHelper.Unbox(javaArgs[index]);
      return this.Invoke(methodName, args);
    }

    public virtual bool equals(AndroidJavaObject obj)
    {
      return AndroidJNI.IsSameObject(this.GetProxy().GetRawObject(), obj != null ? obj.GetRawObject() : IntPtr.Zero);
    }

    public virtual int hashCode()
    {
      jvalue[] args = new jvalue[1];
      args[0].l = this.GetProxy().GetRawObject();
      return AndroidJNISafe.CallStaticIntMethod((IntPtr) AndroidJavaProxy.s_JavaLangSystemClass, AndroidJavaProxy.s_HashCodeMethodID, args);
    }

    public virtual string toString()
    {
      return this.ToString() + " <c# proxy java object>";
    }

    internal AndroidJavaObject GetProxy()
    {
      if (this.proxyObject == null)
        this.proxyObject = AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(AndroidJNIHelper.CreateJavaProxy(this));
      return this.proxyObject;
    }
  }
}
