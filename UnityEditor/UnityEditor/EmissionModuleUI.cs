// Decompiled with JetBrains decompiler
// Type: UnityEditor.EmissionModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class EmissionModuleUI : ModuleUI
  {
    private List<SerializedMinMaxCurve> m_BurstCountCurves = new List<SerializedMinMaxCurve>();
    public SerializedMinMaxCurve m_Time;
    public SerializedMinMaxCurve m_Distance;
    private const int k_MaxNumBursts = 8;
    private const float k_BurstDragWidth = 15f;
    private SerializedProperty m_BurstCount;
    private SerializedProperty m_Bursts;
    private ReorderableList m_BurstList;
    private static EmissionModuleUI.Texts s_Texts;

    public EmissionModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "EmissionModule", displayName)
    {
      this.m_ToolTip = "Emission of the emitter. This controls the rate at which particles are emitted as well as burst emissions.";
    }

    protected override void Init()
    {
      if (this.m_BurstCount != null)
        return;
      if (EmissionModuleUI.s_Texts == null)
        EmissionModuleUI.s_Texts = new EmissionModuleUI.Texts();
      this.m_Time = new SerializedMinMaxCurve((ModuleUI) this, EmissionModuleUI.s_Texts.rateOverTime, "rateOverTime");
      this.m_Distance = new SerializedMinMaxCurve((ModuleUI) this, EmissionModuleUI.s_Texts.rateOverDistance, "rateOverDistance");
      this.m_BurstCount = this.GetProperty("m_BurstCount");
      this.m_Bursts = this.GetProperty("m_Bursts");
      this.m_BurstList = new ReorderableList(this.serializedObject, this.m_Bursts, false, true, true, true);
      this.m_BurstList.elementHeight = 16f;
      this.m_BurstList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.OnBurstListAddCallback);
      this.m_BurstList.onCanAddCallback = new ReorderableList.CanAddCallbackDelegate(this.OnBurstListCanAddCallback);
      this.m_BurstList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.OnBurstListRemoveCallback);
      this.m_BurstList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawBurstListHeaderCallback);
      this.m_BurstList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawBurstListElementCallback);
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      ModuleUI.GUIMinMaxCurve(EmissionModuleUI.s_Texts.rateOverTime, this.m_Time);
      ModuleUI.GUIMinMaxCurve(EmissionModuleUI.s_Texts.rateOverDistance, this.m_Distance);
      this.DoBurstGUI(initial);
    }

    private void DoBurstGUI(InitialModuleUI initial)
    {
      while (this.m_BurstList.count > this.m_BurstCountCurves.Count)
      {
        SerializedProperty arrayElementAtIndex = this.m_Bursts.GetArrayElementAtIndex(this.m_BurstCountCurves.Count);
        this.m_BurstCountCurves.Add(new SerializedMinMaxCurve((ModuleUI) this, EmissionModuleUI.s_Texts.burstCount, arrayElementAtIndex.propertyPath + ".countCurve", false, true));
      }
      EditorGUILayout.Space();
      GUI.Label(ModuleUI.GetControlRect(13), EmissionModuleUI.s_Texts.burst, ParticleSystemStyles.Get().label);
      this.m_BurstList.displayAdd = this.m_Bursts.arraySize < 8;
      this.m_BurstList.DoLayoutList();
    }

    private void OnBurstListAddCallback(ReorderableList list)
    {
      ReorderableList.defaultBehaviours.DoAddButton(list);
      ++this.m_BurstCount.intValue;
      SerializedProperty arrayElementAtIndex = this.m_Bursts.GetArrayElementAtIndex(list.index);
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("countCurve.minMaxState");
      SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("countCurve.scalar");
      SerializedProperty propertyRelative3 = arrayElementAtIndex.FindPropertyRelative("cycleCount");
      propertyRelative1.intValue = 0;
      propertyRelative2.floatValue = 30f;
      propertyRelative3.intValue = 1;
      SerializedProperty propertyRelative4 = arrayElementAtIndex.FindPropertyRelative("countCurve.minCurve");
      SerializedProperty propertyRelative5 = arrayElementAtIndex.FindPropertyRelative("countCurve.maxCurve");
      propertyRelative4.animationCurveValue = AnimationCurve.Linear(0.0f, 1f, 1f, 1f);
      propertyRelative5.animationCurveValue = AnimationCurve.Linear(0.0f, 1f, 1f, 1f);
      this.m_BurstCountCurves.Add(new SerializedMinMaxCurve((ModuleUI) this, EmissionModuleUI.s_Texts.burstCount, arrayElementAtIndex.propertyPath + ".countCurve", false, true));
    }

    private bool OnBurstListCanAddCallback(ReorderableList list)
    {
      return !this.m_ParticleSystemUI.multiEdit;
    }

    private void OnBurstListRemoveCallback(ReorderableList list)
    {
      for (int index = list.index; index < this.m_BurstCountCurves.Count; ++index)
        this.m_BurstCountCurves[index].RemoveCurveFromEditor();
      this.m_BurstCountCurves.RemoveRange(list.index, this.m_BurstCountCurves.Count - list.index);
      AnimationCurvePreviewCache.ClearCache();
      ReorderableList.defaultBehaviours.DoRemoveButton(list);
      --this.m_BurstCount.intValue;
    }

    private void DrawBurstListHeaderCallback(Rect rect)
    {
      rect.width -= 15f;
      rect.width /= 4f;
      rect.x += 15f;
      EditorGUI.LabelField(rect, EmissionModuleUI.s_Texts.burstTime, ParticleSystemStyles.Get().label);
      rect.x += rect.width;
      EditorGUI.LabelField(rect, EmissionModuleUI.s_Texts.burstCount, ParticleSystemStyles.Get().label);
      rect.x += rect.width;
      EditorGUI.LabelField(rect, EmissionModuleUI.s_Texts.burstCycleCount, ParticleSystemStyles.Get().label);
      rect.x += rect.width;
      EditorGUI.LabelField(rect, EmissionModuleUI.s_Texts.burstRepeatInterval, ParticleSystemStyles.Get().label);
      rect.x += rect.width;
    }

    private void DrawBurstListElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
      SerializedProperty arrayElementAtIndex = this.m_Bursts.GetArrayElementAtIndex(index);
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("time");
      SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("cycleCount");
      SerializedProperty propertyRelative3 = arrayElementAtIndex.FindPropertyRelative("repeatInterval");
      rect.width -= 45f;
      rect.width /= 4f;
      double num1 = (double) ModuleUI.FloatDraggable(rect, propertyRelative1, 1f, 15f, "n3");
      rect.x += rect.width;
      rect = ModuleUI.GUIMinMaxCurveInline(rect, this.m_BurstCountCurves[index], 15f);
      rect.x += rect.width;
      rect.width -= 13f;
      if (!propertyRelative2.hasMultipleDifferentValues && propertyRelative2.intValue == 0)
      {
        rect.x += 15f;
        rect.width -= 15f;
        EditorGUI.LabelField(rect, EmissionModuleUI.s_Texts.burstCycleCountInfinite, ParticleSystemStyles.Get().label);
      }
      else
        ModuleUI.IntDraggable(rect, (GUIContent) null, propertyRelative2, 15f);
      rect.width += 13f;
      EmissionModuleUI.GUIMMModePopUp(ModuleUI.GetPopupRect(rect), propertyRelative2);
      rect.x += rect.width;
      double num2 = (double) ModuleUI.FloatDraggable(rect, propertyRelative3, 1f, 15f, "n3");
      rect.x += rect.width;
    }

    private static void SelectModeCallback(object obj)
    {
      EmissionModuleUI.ModeCallbackData modeCallbackData = (EmissionModuleUI.ModeCallbackData) obj;
      modeCallbackData.modeProp.intValue = modeCallbackData.selectedState;
    }

    private static void GUIMMModePopUp(Rect rect, SerializedProperty modeProp)
    {
      if (!EditorGUI.DropdownButton(rect, GUIContent.none, FocusType.Passive, ParticleSystemStyles.Get().minMaxCurveStateDropDown))
        return;
      GUIContent[] guiContentArray = new GUIContent[2]{ new GUIContent("Infinite"), new GUIContent("Count") };
      GenericMenu genericMenu1 = new GenericMenu();
      for (int i = 0; i < guiContentArray.Length; ++i)
      {
        GenericMenu genericMenu2 = genericMenu1;
        GUIContent content = guiContentArray[i];
        int num = modeProp.intValue == i ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        if (EmissionModuleUI.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EmissionModuleUI.\u003C\u003Ef__mg\u0024cache0 = new GenericMenu.MenuFunction2(EmissionModuleUI.SelectModeCallback);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache0 = EmissionModuleUI.\u003C\u003Ef__mg\u0024cache0;
        EmissionModuleUI.ModeCallbackData modeCallbackData = new EmissionModuleUI.ModeCallbackData(i, modeProp);
        genericMenu2.AddItem(content, num != 0, fMgCache0, (object) modeCallbackData);
      }
      genericMenu1.ShowAsContext();
      Event.current.Use();
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      this.Init();
      if (!this.m_Distance.scalar.hasMultipleDifferentValues && (double) this.m_Distance.scalar.floatValue <= 0.0)
        return;
      text += "\nDistance-based emission is being used in the Emission module.";
    }

    private class Texts
    {
      public GUIContent rateOverTime = EditorGUIUtility.TextContent("Rate over Time|The number of particles emitted per second.");
      public GUIContent rateOverDistance = EditorGUIUtility.TextContent("Rate over Distance|The number of particles emitted per distance unit.");
      public GUIContent burst = EditorGUIUtility.TextContent("Bursts|Emission of extra particles at specific times during the duration of the system.");
      public GUIContent burstTime = EditorGUIUtility.TextContent("Time|When the burst will trigger.");
      public GUIContent burstCount = EditorGUIUtility.TextContent("Count|The number of particles to emit.");
      public GUIContent burstCycleCount = EditorGUIUtility.TextContent("Cycles|How many times to emit the burst. Use the dropdown to repeat infinitely.");
      public GUIContent burstCycleCountInfinite = EditorGUIUtility.TextContent("Infinite");
      public GUIContent burstRepeatInterval = EditorGUIUtility.TextContent("Interval|Repeat the burst every N seconds.");
    }

    private class ModeCallbackData
    {
      public SerializedProperty modeProp;
      public int selectedState;

      public ModeCallbackData(int i, SerializedProperty p)
      {
        this.modeProp = p;
        this.selectedState = i;
      }
    }
  }
}
