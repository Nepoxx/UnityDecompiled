// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationWindowEventInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (AnimationWindowEvent))]
  internal class AnimationWindowEventInspector : Editor
  {
    private const string kNotSupportedPostFix = " (Function Not Supported)";
    private const string kNoneSelected = "(No Function Selected)";

    public override void OnInspectorGUI()
    {
      AnimationWindowEventInspector.OnEditAnimationEvents(((IEnumerable<UnityEngine.Object>) this.targets).Select<UnityEngine.Object, AnimationWindowEvent>((Func<UnityEngine.Object, AnimationWindowEvent>) (o => o as AnimationWindowEvent)).ToArray<AnimationWindowEvent>());
    }

    protected override void OnHeaderGUI()
    {
      Editor.DrawHeaderGUI((Editor) this, this.targets.Length != 1 ? this.targets.Length.ToString() + " Animation Events" : "Animation Event");
    }

    public static void OnEditAnimationEvent(AnimationWindowEvent awe)
    {
      AnimationWindowEventInspector.OnEditAnimationEvents(new AnimationWindowEvent[1]
      {
        awe
      });
    }

    public static void OnEditAnimationEvents(AnimationWindowEvent[] awEvents)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimationWindowEventInspector.\u003COnEditAnimationEvents\u003Ec__AnonStorey0 eventsCAnonStorey0 = new AnimationWindowEventInspector.\u003COnEditAnimationEvents\u003Ec__AnonStorey0();
      AnimationWindowEventInspector.AnimationWindowEventData data = AnimationWindowEventInspector.GetData(awEvents);
      if (data.events == null || data.selectedEvents == null || data.selectedEvents.Length == 0)
        return;
      // ISSUE: reference to a compiler-generated field
      eventsCAnonStorey0.firstEvent = data.selectedEvents[0];
      // ISSUE: reference to a compiler-generated method
      bool flag = Array.TrueForAll<AnimationEvent>(data.selectedEvents, new Predicate<AnimationEvent>(eventsCAnonStorey0.\u003C\u003Em__0));
      GUI.changed = false;
      if ((UnityEngine.Object) data.root != (UnityEngine.Object) null)
      {
        List<AnimationWindowEventMethod> windowEventMethodList = AnimationWindowEventInspector.CollectSupportedMethods(data.root);
        List<string> stringList = new List<string>(windowEventMethodList.Count);
        for (int index = 0; index < windowEventMethodList.Count; ++index)
        {
          AnimationWindowEventMethod windowEventMethod = windowEventMethodList[index];
          string str = " ( )";
          if (windowEventMethod.parameterType != null)
            str = windowEventMethod.parameterType != typeof (float) ? (windowEventMethod.parameterType != typeof (int) ? string.Format(" ( {0} )", (object) windowEventMethod.parameterType.Name) : " ( int )") : " ( float )";
          stringList.Add(windowEventMethod.name + str);
        }
        int count = windowEventMethodList.Count;
        // ISSUE: reference to a compiler-generated method
        int selectedIndex = windowEventMethodList.FindIndex(new Predicate<AnimationWindowEventMethod>(eventsCAnonStorey0.\u003C\u003Em__1));
        if (selectedIndex == -1)
        {
          selectedIndex = windowEventMethodList.Count;
          // ISSUE: reference to a compiler-generated field
          windowEventMethodList.Add(new AnimationWindowEventMethod()
          {
            name = eventsCAnonStorey0.firstEvent.functionName,
            parameterType = (System.Type) null
          });
          // ISSUE: reference to a compiler-generated field
          if (string.IsNullOrEmpty(eventsCAnonStorey0.firstEvent.functionName))
          {
            stringList.Add("(No Function Selected)");
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            stringList.Add(eventsCAnonStorey0.firstEvent.functionName + " (Function Not Supported)");
          }
        }
        EditorGUIUtility.labelWidth = 130f;
        EditorGUI.showMixedValue = !flag;
        int num = !flag ? -1 : selectedIndex;
        int index1 = EditorGUILayout.Popup("Function: ", selectedIndex, stringList.ToArray(), new GUILayoutOption[0]);
        if (num != index1 && index1 != -1 && index1 != count)
        {
          foreach (AnimationEvent selectedEvent in data.selectedEvents)
          {
            selectedEvent.functionName = windowEventMethodList[index1].name;
            selectedEvent.stringParameter = string.Empty;
          }
        }
        EditorGUI.showMixedValue = false;
        System.Type parameterType = windowEventMethodList[index1].parameterType;
        if (flag && parameterType != null)
        {
          EditorGUILayout.Space();
          if (parameterType == typeof (AnimationEvent))
            EditorGUILayout.PrefixLabel("Event Data");
          else
            EditorGUILayout.PrefixLabel("Parameters");
          AnimationWindowEventInspector.DoEditRegularParameters(data.selectedEvents, parameterType);
        }
      }
      else
      {
        EditorGUI.showMixedValue = !flag;
        // ISSUE: reference to a compiler-generated field
        string text = !flag ? "" : eventsCAnonStorey0.firstEvent.functionName;
        string str = EditorGUILayout.TextField(new GUIContent("Function"), text, new GUILayoutOption[0]);
        if (str != text)
        {
          foreach (AnimationEvent selectedEvent in data.selectedEvents)
          {
            selectedEvent.functionName = str;
            selectedEvent.stringParameter = string.Empty;
          }
        }
        EditorGUI.showMixedValue = false;
        if (flag)
        {
          AnimationWindowEventInspector.DoEditRegularParameters(data.selectedEvents, typeof (AnimationEvent));
        }
        else
        {
          using (new EditorGUI.DisabledScope(true))
            AnimationWindowEventInspector.DoEditRegularParameters(new AnimationEvent[1]
            {
              new AnimationEvent()
            }, typeof (AnimationEvent));
        }
      }
      if (!GUI.changed)
        return;
      AnimationWindowEventInspector.SetData(awEvents, data);
    }

    public static void OnDisabledAnimationEvent()
    {
      AnimationEvent animationEvent = new AnimationEvent();
      using (new EditorGUI.DisabledScope(true))
      {
        animationEvent.functionName = EditorGUILayout.TextField(new GUIContent("Function"), animationEvent.functionName, new GUILayoutOption[0]);
        AnimationWindowEventInspector.DoEditRegularParameters(new AnimationEvent[1]
        {
          animationEvent
        }, typeof (AnimationEvent));
      }
    }

    public static List<AnimationWindowEventMethod> CollectSupportedMethods(GameObject gameObject)
    {
      List<AnimationWindowEventMethod> windowEventMethodList = new List<AnimationWindowEventMethod>();
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        return windowEventMethodList;
      MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
      HashSet<string> stringSet = new HashSet<string>();
      foreach (MonoBehaviour monoBehaviour in components)
      {
        if (!((UnityEngine.Object) monoBehaviour == (UnityEngine.Object) null))
        {
          for (System.Type type1 = monoBehaviour.GetType(); type1 != typeof (MonoBehaviour) && type1 != null; type1 = type1.BaseType)
          {
            foreach (MethodInfo method in type1.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              AnimationWindowEventInspector.\u003CCollectSupportedMethods\u003Ec__AnonStorey1 methodsCAnonStorey1 = new AnimationWindowEventInspector.\u003CCollectSupportedMethods\u003Ec__AnonStorey1();
              MethodInfo methodInfo = method;
              // ISSUE: reference to a compiler-generated field
              methodsCAnonStorey1.name = methodInfo.Name;
              // ISSUE: reference to a compiler-generated field
              if (AnimationWindowEventInspector.IsSupportedMethodName(methodsCAnonStorey1.name))
              {
                System.Reflection.ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length <= 1)
                {
                  System.Type type2 = (System.Type) null;
                  if (parameters.Length == 1)
                  {
                    type2 = parameters[0].ParameterType;
                    if (type2 != typeof (string) && type2 != typeof (float) && (type2 != typeof (int) && type2 != typeof (AnimationEvent)) && (type2 != typeof (UnityEngine.Object) && !type2.IsSubclassOf(typeof (UnityEngine.Object)) && !type2.IsEnum))
                      continue;
                  }
                  AnimationWindowEventMethod windowEventMethod = new AnimationWindowEventMethod();
                  windowEventMethod.name = methodInfo.Name;
                  windowEventMethod.parameterType = type2;
                  // ISSUE: reference to a compiler-generated method
                  int index = windowEventMethodList.FindIndex(new Predicate<AnimationWindowEventMethod>(methodsCAnonStorey1.\u003C\u003Em__0));
                  if (index != -1 && windowEventMethodList[index].parameterType != type2)
                  {
                    // ISSUE: reference to a compiler-generated field
                    stringSet.Add(methodsCAnonStorey1.name);
                  }
                  windowEventMethodList.Add(windowEventMethod);
                }
              }
            }
          }
        }
      }
      foreach (string str in stringSet)
      {
        for (int index = windowEventMethodList.Count - 1; index >= 0; --index)
        {
          if (windowEventMethodList[index].name.Equals(str))
            windowEventMethodList.RemoveAt(index);
        }
      }
      return windowEventMethodList;
    }

    public static string FormatEvent(GameObject root, AnimationEvent evt)
    {
      if (string.IsNullOrEmpty(evt.functionName))
        return "(No Function Selected)";
      if (!AnimationWindowEventInspector.IsSupportedMethodName(evt.functionName) || (UnityEngine.Object) root == (UnityEngine.Object) null)
        return evt.functionName + " (Function Not Supported)";
      foreach (MonoBehaviour component in root.GetComponents<MonoBehaviour>())
      {
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
        {
          System.Type type = component.GetType();
          if (type != typeof (MonoBehaviour) && (type.BaseType == null || !(type.BaseType.Name == "GraphBehaviour")))
          {
            MethodInfo methodInfo = (MethodInfo) null;
            try
            {
              methodInfo = type.GetMethod(evt.functionName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            }
            catch (AmbiguousMatchException ex)
            {
            }
            if (methodInfo != null)
            {
              IEnumerable<System.Type> paramTypes = ((IEnumerable<System.Reflection.ParameterInfo>) methodInfo.GetParameters()).Select<System.Reflection.ParameterInfo, System.Type>((Func<System.Reflection.ParameterInfo, System.Type>) (p => p.ParameterType));
              return evt.functionName + AnimationWindowEventInspector.FormatEventArguments(paramTypes, evt);
            }
          }
        }
      }
      return evt.functionName + " (Function Not Supported)";
    }

    private static void DoEditRegularParameters(AnimationEvent[] events, System.Type selectedParameter)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimationWindowEventInspector.\u003CDoEditRegularParameters\u003Ec__AnonStorey2 parametersCAnonStorey2 = new AnimationWindowEventInspector.\u003CDoEditRegularParameters\u003Ec__AnonStorey2();
      // ISSUE: reference to a compiler-generated field
      parametersCAnonStorey2.firstEvent = events[0];
      if (selectedParameter == typeof (AnimationEvent) || selectedParameter == typeof (float))
      {
        // ISSUE: reference to a compiler-generated method
        bool flag = Array.TrueForAll<AnimationEvent>(events, new Predicate<AnimationEvent>(parametersCAnonStorey2.\u003C\u003Em__0));
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = !flag;
        // ISSUE: reference to a compiler-generated field
        float num = EditorGUILayout.FloatField("Float", parametersCAnonStorey2.firstEvent.floatParameter, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
          foreach (AnimationEvent animationEvent in events)
            animationEvent.floatParameter = num;
        }
      }
      if (selectedParameter == typeof (AnimationEvent) || selectedParameter == typeof (int) || selectedParameter.IsEnum)
      {
        // ISSUE: reference to a compiler-generated method
        bool flag = Array.TrueForAll<AnimationEvent>(events, new Predicate<AnimationEvent>(parametersCAnonStorey2.\u003C\u003Em__1));
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = !flag;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = !selectedParameter.IsEnum ? EditorGUILayout.IntField("Int", parametersCAnonStorey2.firstEvent.intParameter, new GUILayoutOption[0]) : AnimationWindowEventInspector.EnumPopup("Enum", selectedParameter, parametersCAnonStorey2.firstEvent.intParameter);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
          foreach (AnimationEvent animationEvent in events)
            animationEvent.intParameter = num;
        }
      }
      if (selectedParameter == typeof (AnimationEvent) || selectedParameter == typeof (string))
      {
        // ISSUE: reference to a compiler-generated method
        bool flag = Array.TrueForAll<AnimationEvent>(events, new Predicate<AnimationEvent>(parametersCAnonStorey2.\u003C\u003Em__2));
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = !flag;
        // ISSUE: reference to a compiler-generated field
        string str = EditorGUILayout.TextField("String", parametersCAnonStorey2.firstEvent.stringParameter, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
          foreach (AnimationEvent animationEvent in events)
            animationEvent.stringParameter = str;
        }
      }
      if (selectedParameter != typeof (AnimationEvent) && !selectedParameter.IsSubclassOf(typeof (UnityEngine.Object)) && selectedParameter != typeof (UnityEngine.Object))
        return;
      // ISSUE: reference to a compiler-generated method
      bool flag1 = Array.TrueForAll<AnimationEvent>(events, new Predicate<AnimationEvent>(parametersCAnonStorey2.\u003C\u003Em__3));
      EditorGUI.BeginChangeCheck();
      System.Type objType = typeof (UnityEngine.Object);
      if (selectedParameter != typeof (AnimationEvent))
        objType = selectedParameter;
      EditorGUI.showMixedValue = !flag1;
      bool allowSceneObjects = false;
      // ISSUE: reference to a compiler-generated field
      UnityEngine.Object @object = EditorGUILayout.ObjectField(ObjectNames.NicifyVariableName(objType.Name), parametersCAnonStorey2.firstEvent.objectReferenceParameter, objType, allowSceneObjects, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        foreach (AnimationEvent animationEvent in events)
          animationEvent.objectReferenceParameter = @object;
      }
    }

    private static int EnumPopup(string label, System.Type enumType, int selected)
    {
      if (!enumType.IsEnum)
        throw new Exception("parameter _enum must be of type System.Enum");
      string[] names = Enum.GetNames(enumType);
      int selectedIndex = Array.IndexOf<string>(names, Enum.GetName(enumType, (object) selected));
      int index = EditorGUILayout.Popup(label, selectedIndex, names, EditorStyles.popup, new GUILayoutOption[0]);
      if (index == -1)
        return selected;
      return Convert.ToInt32((object) (Enum) Enum.Parse(enumType, names[index]));
    }

    private static bool IsSupportedMethodName(string name)
    {
      return !(name == "Main") && !(name == "Start") && (!(name == "Awake") && !(name == "Update"));
    }

    private static string FormatEventArguments(IEnumerable<System.Type> paramTypes, AnimationEvent evt)
    {
      if (!paramTypes.Any<System.Type>())
        return " ( )";
      if (paramTypes.Count<System.Type>() > 1)
        return " (Function Not Supported)";
      System.Type enumType = paramTypes.First<System.Type>();
      if (enumType == typeof (string))
        return " ( \"" + evt.stringParameter + "\" )";
      if (enumType == typeof (float))
        return " ( " + (object) evt.floatParameter + " )";
      if (enumType == typeof (int))
        return " ( " + (object) evt.intParameter + " )";
      if (enumType.IsEnum)
        return " ( " + enumType.Name + "." + Enum.GetName(enumType, (object) evt.intParameter) + " )";
      if (enumType == typeof (AnimationEvent))
        return " ( " + (object) evt.floatParameter + " / " + (object) evt.intParameter + " / \"" + evt.stringParameter + "\" / " + (!(evt.objectReferenceParameter == (UnityEngine.Object) null) ? (object) evt.objectReferenceParameter.name : (object) "null") + " )";
      if (enumType.IsSubclassOf(typeof (UnityEngine.Object)) || enumType == typeof (UnityEngine.Object))
        return " ( " + (!(evt.objectReferenceParameter == (UnityEngine.Object) null) ? evt.objectReferenceParameter.name : "null") + " )";
      return " (Function Not Supported)";
    }

    private static AnimationWindowEventInspector.AnimationWindowEventData GetData(AnimationWindowEvent[] awEvents)
    {
      AnimationWindowEventInspector.AnimationWindowEventData animationWindowEventData = new AnimationWindowEventInspector.AnimationWindowEventData();
      if (awEvents.Length == 0)
        return animationWindowEventData;
      AnimationWindowEvent awEvent1 = awEvents[0];
      animationWindowEventData.root = awEvent1.root;
      animationWindowEventData.clip = awEvent1.clip;
      animationWindowEventData.clipInfo = awEvent1.clipInfo;
      if ((UnityEngine.Object) animationWindowEventData.clip != (UnityEngine.Object) null)
        animationWindowEventData.events = AnimationUtility.GetAnimationEvents(animationWindowEventData.clip);
      else if (animationWindowEventData.clipInfo != null)
        animationWindowEventData.events = animationWindowEventData.clipInfo.GetEvents();
      if (animationWindowEventData.events != null)
      {
        List<AnimationEvent> animationEventList = new List<AnimationEvent>();
        foreach (AnimationWindowEvent awEvent2 in awEvents)
        {
          if (awEvent2.eventIndex >= 0 && awEvent2.eventIndex < animationWindowEventData.events.Length)
            animationEventList.Add(animationWindowEventData.events[awEvent2.eventIndex]);
        }
        animationWindowEventData.selectedEvents = animationEventList.ToArray();
      }
      return animationWindowEventData;
    }

    private static void SetData(AnimationWindowEvent[] awEvents, AnimationWindowEventInspector.AnimationWindowEventData data)
    {
      if (data.events == null)
        return;
      if ((UnityEngine.Object) data.clip != (UnityEngine.Object) null)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) data.clip, "Animation Event Change");
        AnimationUtility.SetAnimationEvents(data.clip, data.events);
      }
      else
      {
        if (data.clipInfo == null)
          return;
        foreach (AnimationWindowEvent awEvent in awEvents)
        {
          if (awEvent.eventIndex >= 0 && awEvent.eventIndex < data.events.Length)
            data.clipInfo.SetEvent(awEvent.eventIndex, data.events[awEvent.eventIndex]);
        }
      }
    }

    [MenuItem("CONTEXT/AnimationWindowEvent/Reset")]
    private static void ResetValues(MenuCommand command)
    {
      AnimationWindowEvent[] awEvents = new AnimationWindowEvent[1]{ command.context as AnimationWindowEvent };
      AnimationWindowEventInspector.AnimationWindowEventData data = AnimationWindowEventInspector.GetData(awEvents);
      if (data.events == null || data.selectedEvents == null || data.selectedEvents.Length == 0)
        return;
      foreach (AnimationEvent selectedEvent in data.selectedEvents)
      {
        selectedEvent.functionName = "";
        selectedEvent.stringParameter = string.Empty;
        selectedEvent.floatParameter = 0.0f;
        selectedEvent.intParameter = 0;
        selectedEvent.objectReferenceParameter = (UnityEngine.Object) null;
      }
      AnimationWindowEventInspector.SetData(awEvents, data);
    }

    private struct AnimationWindowEventData
    {
      public GameObject root;
      public AnimationClip clip;
      public AnimationClipInfoProperties clipInfo;
      public AnimationEvent[] events;
      public AnimationEvent[] selectedEvents;
    }
  }
}
