// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.JSProxyMgr
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEditor.Collaboration;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class JSProxyMgr
  {
    private Queue<JSProxyMgr.TaskCallback> m_TaskList = (Queue<JSProxyMgr.TaskCallback>) null;
    private Dictionary<string, object> m_GlobalObjects = (Dictionary<string, object>) null;
    private static JSProxyMgr s_Instance = (JSProxyMgr) null;
    private static readonly string[] s_IgnoredMethods = new string[4]{ "Equals", "GetHashCode", "GetType", "ToString" };
    public const double kProtocolVersion = 1.0;
    public const long kInvalidMessageID = -1;
    public const int kErrNone = 0;
    public const int kErrInvalidMessageFormat = -1000;
    public const int kErrUnknownObject = -1001;
    public const int kErrUnknownMethod = -1002;
    public const int kErrInvocationFailed = -1003;
    public const int kErrUnsupportedProtocol = -1004;
    public const int kErrUnknownEvent = -1005;
    public const string kTypeInvoke = "INVOKE";
    public const string kTypeGetStubInfo = "GETSTUBINFO";
    public const string kTypeOnEvent = "ONEVENT";

    static JSProxyMgr()
    {
      WebView.OnDomainReload();
    }

    protected JSProxyMgr()
    {
      this.m_TaskList = new Queue<JSProxyMgr.TaskCallback>();
      this.m_GlobalObjects = new Dictionary<string, object>();
      this.AddGlobalObject("unity/collab", (object) Collab.instance);
    }

    public static JSProxyMgr GetInstance()
    {
      if (JSProxyMgr.s_Instance == null)
        JSProxyMgr.s_Instance = new JSProxyMgr();
      return JSProxyMgr.s_Instance;
    }

    public static void DoTasks()
    {
      JSProxyMgr.GetInstance().ProcessTasks();
    }

    ~JSProxyMgr()
    {
      this.m_GlobalObjects.Clear();
      this.m_GlobalObjects = (Dictionary<string, object>) null;
    }

    public void AddGlobalObject(string referenceName, object obj)
    {
      if (this.m_GlobalObjects == null)
        this.m_GlobalObjects = new Dictionary<string, object>();
      this.RemoveGlobalObject(referenceName);
      this.m_GlobalObjects.Add(referenceName, obj);
    }

    public void RemoveGlobalObject(string referenceName)
    {
      if (this.m_GlobalObjects == null || !this.m_GlobalObjects.ContainsKey(referenceName))
        return;
      this.m_GlobalObjects.Remove(referenceName);
    }

    private void AddTask(JSProxyMgr.TaskCallback task)
    {
      if (this.m_TaskList == null)
        this.m_TaskList = new Queue<JSProxyMgr.TaskCallback>();
      this.m_TaskList.Enqueue(task);
    }

    private void ProcessTasks()
    {
      if (this.m_TaskList == null || this.m_TaskList.Count == 0)
        return;
      for (int index = 10; this.m_TaskList.Count > 0 && index > 0; --index)
        this.m_TaskList.Dequeue()();
    }

    public bool DoMessage(string jsonRequest, JSProxyMgr.ExecCallback callback, WebView webView)
    {
      long messageID = -1;
      try
      {
        Dictionary<string, object> jsonData = Json.Deserialize(jsonRequest) as Dictionary<string, object>;
        if (jsonData == null || !jsonData.ContainsKey("messageID") || (!jsonData.ContainsKey("version") || !jsonData.ContainsKey("type")))
        {
          callback((object) JSProxyMgr.FormatError(messageID, -1000, "errInvalidMessageFormat", jsonRequest));
          return false;
        }
        messageID = (long) jsonData["messageID"];
        double num = double.Parse((string) jsonData["version"], (IFormatProvider) CultureInfo.InvariantCulture);
        string str = (string) jsonData["type"];
        if (num > 1.0)
        {
          callback((object) JSProxyMgr.FormatError(messageID, -1004, "errUnsupportedProtocol", "The protocol version <" + (object) num + "> is not supported by this verison of the code"));
          return false;
        }
        if (str == "INVOKE")
          return this.DoInvokeMessage(messageID, callback, jsonData);
        if (str == "GETSTUBINFO")
          return this.DoGetStubInfoMessage(messageID, callback, jsonData);
        if (str == "ONEVENT")
          return this.DoOnEventMessage(messageID, callback, jsonData, webView);
      }
      catch (Exception ex)
      {
        callback((object) JSProxyMgr.FormatError(messageID, -1000, "errInvalidMessageFormat", ex.Message));
      }
      return false;
    }

    private bool DoGetStubInfoMessage(long messageID, JSProxyMgr.ExecCallback callback, Dictionary<string, object> jsonData)
    {
      if (!jsonData.ContainsKey("reference"))
      {
        callback((object) JSProxyMgr.FormatError(messageID, -1001, "errUnknownObject", "object reference missing"));
        return false;
      }
      string reference = (string) jsonData["reference"];
      object destinationObject = this.GetDestinationObject(reference);
      if (destinationObject == null)
      {
        callback((object) JSProxyMgr.FormatError(messageID, -1001, "errUnknownObject", "cannot find object with reference <" + reference + ">"));
        return false;
      }
      List<MethodInfo> list1 = ((IEnumerable<MethodInfo>) destinationObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)).ToList<MethodInfo>();
      list1.AddRange((IEnumerable<MethodInfo>) ((IEnumerable<MethodInfo>) destinationObject.GetType().GetMethods(BindingFlags.Static | BindingFlags.Public)).ToList<MethodInfo>());
      ArrayList arrayList1 = new ArrayList();
      foreach (MethodInfo methodInfo in list1)
      {
        if (Array.IndexOf<string>(JSProxyMgr.s_IgnoredMethods, methodInfo.Name) < 0 && (!methodInfo.IsSpecialName || !methodInfo.Name.StartsWith("set_") && !methodInfo.Name.StartsWith("get_")))
        {
          System.Reflection.ParameterInfo[] parameters = methodInfo.GetParameters();
          ArrayList arrayList2 = new ArrayList();
          foreach (System.Reflection.ParameterInfo parameterInfo in parameters)
            arrayList2.Add((object) parameterInfo.Name);
          JspmMethodInfo jspmMethodInfo = new JspmMethodInfo(methodInfo.Name, (string[]) arrayList2.ToArray(typeof (string)));
          arrayList1.Add((object) jspmMethodInfo);
        }
      }
      List<PropertyInfo> list2 = ((IEnumerable<PropertyInfo>) destinationObject.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)).ToList<PropertyInfo>();
      ArrayList arrayList3 = new ArrayList();
      foreach (PropertyInfo propertyInfo in list2)
        arrayList3.Add((object) new JspmPropertyInfo(propertyInfo.Name, propertyInfo.GetValue(destinationObject, (object[]) null)));
      foreach (System.Reflection.FieldInfo fieldInfo in ((IEnumerable<System.Reflection.FieldInfo>) destinationObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)).ToList<System.Reflection.FieldInfo>())
        arrayList3.Add((object) new JspmPropertyInfo(fieldInfo.Name, fieldInfo.GetValue(destinationObject)));
      List<EventInfo> list3 = ((IEnumerable<EventInfo>) destinationObject.GetType().GetEvents(BindingFlags.Instance | BindingFlags.Public)).ToList<EventInfo>();
      ArrayList arrayList4 = new ArrayList();
      foreach (EventInfo eventInfo in list3)
        arrayList4.Add((object) eventInfo.Name);
      callback((object) new JspmStubInfoSuccess(messageID, reference, (JspmPropertyInfo[]) arrayList3.ToArray(typeof (JspmPropertyInfo)), (JspmMethodInfo[]) arrayList1.ToArray(typeof (JspmMethodInfo)), (string[]) arrayList4.ToArray(typeof (string))));
      return true;
    }

    private bool DoOnEventMessage(long messageID, JSProxyMgr.ExecCallback callback, Dictionary<string, object> jsonData, WebView webView)
    {
      callback((object) JSProxyMgr.FormatError(messageID, -1002, "errUnknownMethod", "method DoOnEventMessage is deprecated"));
      return false;
    }

    private bool DoInvokeMessage(long messageID, JSProxyMgr.ExecCallback callback, Dictionary<string, object> jsonData)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      JSProxyMgr.\u003CDoInvokeMessage\u003Ec__AnonStorey0 messageCAnonStorey0 = new JSProxyMgr.\u003CDoInvokeMessage\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      messageCAnonStorey0.callback = callback;
      // ISSUE: reference to a compiler-generated field
      messageCAnonStorey0.messageID = messageID;
      if (!jsonData.ContainsKey("destination") || !jsonData.ContainsKey("method") || !jsonData.ContainsKey("params"))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        messageCAnonStorey0.callback((object) JSProxyMgr.FormatError(messageCAnonStorey0.messageID, -1001, "errUnknownObject", "object reference, method name or parameters missing"));
        return false;
      }
      string reference = (string) jsonData["destination"];
      string str1 = (string) jsonData["method"];
      List<object> data = (List<object>) jsonData["params"];
      // ISSUE: reference to a compiler-generated field
      messageCAnonStorey0.destObject = this.GetDestinationObject(reference);
      // ISSUE: reference to a compiler-generated field
      if (messageCAnonStorey0.destObject == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        messageCAnonStorey0.callback((object) JSProxyMgr.FormatError(messageCAnonStorey0.messageID, -1001, "errUnknownObject", "cannot find object with reference <" + reference + ">"));
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      MethodInfo[] methods = messageCAnonStorey0.destObject.GetType().GetMethods();
      // ISSUE: reference to a compiler-generated field
      messageCAnonStorey0.foundMethod = (MethodInfo) null;
      // ISSUE: reference to a compiler-generated field
      messageCAnonStorey0.parameters = (object[]) null;
      string str2 = "";
      foreach (MethodInfo method in methods)
      {
        if (!(method.Name != str1))
        {
          try
          {
            // ISSUE: reference to a compiler-generated field
            messageCAnonStorey0.parameters = this.ParseParams(method, data);
            // ISSUE: reference to a compiler-generated field
            messageCAnonStorey0.foundMethod = method;
            break;
          }
          catch (Exception ex)
          {
            str2 = ex.Message;
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (messageCAnonStorey0.foundMethod == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        messageCAnonStorey0.callback((object) JSProxyMgr.FormatError(messageCAnonStorey0.messageID, -1002, "errUnknownMethod", "cannot find method <" + str1 + "> for object <" + reference + ">, reason:" + str2));
        return false;
      }
      // ISSUE: reference to a compiler-generated method
      this.AddTask(new JSProxyMgr.TaskCallback(messageCAnonStorey0.\u003C\u003Em__0));
      return true;
    }

    public static JspmError FormatError(long messageID, int status, string errorClass, string message)
    {
      return new JspmError(messageID, status, errorClass, message);
    }

    public static JspmSuccess FormatSuccess(long messageID, object result)
    {
      return new JspmSuccess(messageID, result, "INVOKE");
    }

    public object GetDestinationObject(string reference)
    {
      object obj;
      this.m_GlobalObjects.TryGetValue(reference, out obj);
      return obj;
    }

    public object[] ParseParams(MethodInfo method, List<object> data)
    {
      System.Reflection.ParameterInfo[] parameters = method.GetParameters();
      if (parameters.Length != data.Count)
        return (object[]) null;
      List<object> objectList = new List<object>(data.Count);
      for (int index = 0; index < data.Count; ++index)
      {
        object obj = this.InternalParseParam(parameters[index].ParameterType, data[index]);
        objectList.Add(obj);
      }
      return objectList.ToArray();
    }

    private object InternalParseParam(System.Type type, object data)
    {
      if (data == null)
        return (object) null;
      IList list;
      if ((list = data as IList) != null)
      {
        if (!type.IsArray)
          throw new InvalidOperationException("Not an array " + type.FullName);
        System.Type elementType = type.GetElementType();
        ArrayList arrayList = new ArrayList();
        for (int index = 0; index < list.Count; ++index)
        {
          object obj = this.InternalParseParam(elementType, list[index]);
          arrayList.Add(obj);
        }
        return (object) arrayList.ToArray(elementType);
      }
      IDictionary dictionary;
      if ((dictionary = data as IDictionary) != null)
      {
        if (!type.IsClass)
          throw new InvalidOperationException("Not a class " + type.FullName);
        ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, (Binder) null, CallingConventions.Any, new System.Type[0], (ParameterModifier[]) null);
        if (constructor == null)
          throw new InvalidOperationException("Cannot find a default constructor for " + type.FullName);
        object obj1 = constructor.Invoke(new object[0]);
        foreach (System.Reflection.FieldInfo fieldInfo in ((IEnumerable<System.Reflection.FieldInfo>) type.GetFields(BindingFlags.Instance | BindingFlags.Public)).ToList<System.Reflection.FieldInfo>())
        {
          try
          {
            object obj2 = this.InternalParseParam(fieldInfo.FieldType, dictionary[(object) fieldInfo.Name]);
            fieldInfo.SetValue(obj1, obj2);
          }
          catch (KeyNotFoundException ex)
          {
          }
        }
        foreach (PropertyInfo propertyInfo in ((IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.Instance | BindingFlags.Public)).ToList<PropertyInfo>())
        {
          try
          {
            object obj2 = this.InternalParseParam(propertyInfo.PropertyType, dictionary[(object) propertyInfo.Name]);
            MethodInfo setMethod = propertyInfo.GetSetMethod();
            if (setMethod != null)
              setMethod.Invoke(obj1, new object[1]{ obj2 });
          }
          catch (KeyNotFoundException ex)
          {
          }
          catch (TargetInvocationException ex)
          {
          }
        }
        return Convert.ChangeType(obj1, type);
      }
      string str;
      if ((str = data as string) != null)
        return (object) str;
      if (data is bool)
        return (object) (bool) data;
      if (data is double)
        return (object) (double) data;
      if (data is int || data is short || (data is int || data is long) || data is long)
        return Convert.ChangeType(data, type);
      throw new InvalidOperationException("Cannot parse " + Json.Serialize(data));
    }

    public string Stringify(object target)
    {
      return Json.Serialize(target);
    }

    protected object GetMemberValue(MemberInfo member, object target)
    {
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          return ((System.Reflection.FieldInfo) member).GetValue(target);
        case MemberTypes.Property:
          try
          {
            return ((PropertyInfo) member).GetValue(target, (object[]) null);
          }
          catch (TargetParameterCountException ex)
          {
            throw new ArgumentException("MemberInfo has index parameters", nameof (member), (Exception) ex);
          }
        default:
          throw new ArgumentException("MemberInfo is not of type FieldInfo or PropertyInfo", nameof (member));
      }
    }

    protected delegate void TaskCallback();

    public delegate void ExecCallback(object result);
  }
}
