// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarMaskInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace UnityEditor
{
  [CustomEditor(typeof (AvatarMask))]
  internal class AvatarMaskInspector : Editor
  {
    private bool m_ShowBodyMask = true;
    private bool m_BodyMaskFoldout = false;
    private bool m_CanImport = true;
    private SerializedProperty m_BodyMask = (SerializedProperty) null;
    private SerializedProperty m_TransformMask = (SerializedProperty) null;
    private SerializedProperty m_AnimationType = (SerializedProperty) null;
    private AnimationClipInfoProperties m_ClipInfo = (AnimationClipInfoProperties) null;
    private string[] m_TransformPaths = (string[]) null;
    private bool m_TransformMaskFoldout = false;
    private string[] m_HumanTransform = (string[]) null;
    private AvatarMaskInspector.NodeInfo[] m_NodeInfos;
    private Avatar m_RefAvatar;
    private ModelImporter m_RefImporter;

    public bool canImport
    {
      get
      {
        return this.m_CanImport;
      }
      set
      {
        this.m_CanImport = value;
      }
    }

    public AnimationClipInfoProperties clipInfo
    {
      get
      {
        return this.m_ClipInfo;
      }
      set
      {
        this.m_ClipInfo = value;
        if (this.m_ClipInfo != null)
        {
          this.m_ClipInfo.MaskFromClip(this.target as AvatarMask);
          SerializedObject serializedObject = this.m_ClipInfo.maskTypeProperty.serializedObject;
          this.m_AnimationType = serializedObject.FindProperty("m_AnimationType");
          this.m_TransformPaths = (serializedObject.targetObject as ModelImporter).transformPaths;
        }
        else
        {
          this.m_TransformPaths = (string[]) null;
          this.m_AnimationType = (SerializedProperty) null;
        }
        this.InitializeSerializedProperties();
      }
    }

    private ModelImporterAnimationType animationType
    {
      get
      {
        if (this.m_AnimationType != null)
          return (ModelImporterAnimationType) this.m_AnimationType.intValue;
        return ModelImporterAnimationType.None;
      }
    }

    private void InitializeSerializedProperties()
    {
      if (this.clipInfo != null)
      {
        this.m_BodyMask = this.clipInfo.bodyMaskProperty;
        this.m_TransformMask = this.clipInfo.transformMaskProperty;
      }
      else
      {
        this.m_BodyMask = this.serializedObject.FindProperty("m_Mask");
        this.m_TransformMask = this.serializedObject.FindProperty("m_Elements");
      }
      this.FillNodeInfos();
    }

    private void OnEnable()
    {
      if (this.target == (UnityEngine.Object) null)
        return;
      this.InitializeSerializedProperties();
    }

    public bool showBody
    {
      get
      {
        return this.m_ShowBodyMask;
      }
      set
      {
        this.m_ShowBodyMask = value;
      }
    }

    public string[] humanTransforms
    {
      get
      {
        if (this.animationType == ModelImporterAnimationType.Human && this.clipInfo != null)
        {
          if (this.m_HumanTransform == null)
          {
            SerializedObject serializedObject = this.clipInfo.maskTypeProperty.serializedObject;
            ModelImporter targetObject = serializedObject.targetObject as ModelImporter;
            this.m_HumanTransform = AvatarMaskUtility.GetAvatarHumanTransform(serializedObject, targetObject.transformPaths);
          }
        }
        else
          this.m_HumanTransform = (string[]) null;
        return this.m_HumanTransform;
      }
    }

    private ClipAnimationMaskType IndexToMaskType(int index)
    {
      return index == 2 ? ClipAnimationMaskType.None : (ClipAnimationMaskType) index;
    }

    private int MaskTypeToIndex(ClipAnimationMaskType maskType)
    {
      return maskType == ClipAnimationMaskType.None ? 2 : (int) maskType;
    }

    public override void OnInspectorGUI()
    {
      Profiler.BeginSample("AvatarMaskInspector.OnInspectorGUI()");
      if (this.clipInfo == null)
        this.serializedObject.Update();
      bool flag = false;
      if (this.clipInfo != null)
      {
        EditorGUI.BeginChangeCheck();
        int index1 = this.MaskTypeToIndex(this.clipInfo.maskType);
        EditorGUI.showMixedValue = this.clipInfo.maskTypeProperty.hasMultipleDifferentValues;
        int index2 = EditorGUILayout.Popup(AvatarMaskInspector.Styles.MaskDefinition, index1, AvatarMaskInspector.Styles.MaskDefinitionOpt, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
          this.clipInfo.maskType = this.IndexToMaskType(index2);
          this.UpdateMask(this.clipInfo.maskType);
        }
        flag = this.clipInfo.maskType == ClipAnimationMaskType.CopyFromOther;
      }
      if (flag)
        this.CopyFromOtherGUI();
      bool enabled = GUI.enabled;
      GUI.enabled = !flag;
      EditorGUI.BeginChangeCheck();
      this.OnBodyInspectorGUI();
      this.OnTransformInspectorGUI();
      if (this.clipInfo != null && EditorGUI.EndChangeCheck())
        this.clipInfo.MaskFromClip(this.target as AvatarMask);
      GUI.enabled = enabled;
      if (this.clipInfo == null)
        this.serializedObject.ApplyModifiedProperties();
      Profiler.EndSample();
    }

    protected void CopyFromOtherGUI()
    {
      if (this.clipInfo == null)
        return;
      EditorGUILayout.BeginHorizontal();
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.clipInfo.maskSourceProperty, GUIContent.Temp("Source"), new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck() && (UnityEngine.Object) (this.clipInfo.maskSourceProperty.objectReferenceValue as AvatarMask) != (UnityEngine.Object) null)
        this.UpdateMask(this.clipInfo.maskType);
      EditorGUILayout.EndHorizontal();
    }

    public bool IsMaskEmpty()
    {
      return this.m_NodeInfos.Length == 0;
    }

    public bool IsMaskUpToDate()
    {
      if (this.clipInfo == null || this.m_NodeInfos.Length != this.m_TransformPaths.Length)
        return false;
      if (this.m_TransformMask.arraySize > 0)
      {
        SerializedProperty arrayElementAtIndex = this.m_TransformMask.GetArrayElementAtIndex(0);
        for (int index = 1; index < this.m_NodeInfos.Length; ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated method
          if (ArrayUtility.FindIndex<string>(this.m_TransformPaths, new Predicate<string>(new AvatarMaskInspector.\u003CIsMaskUpToDate\u003Ec__AnonStorey0() { path = this.m_NodeInfos[index].m_Path.stringValue }.\u003C\u003Em__0)) == -1)
            return false;
          arrayElementAtIndex.Next(false);
        }
      }
      return true;
    }

    private void UpdateMask(ClipAnimationMaskType maskType)
    {
      if (this.clipInfo == null)
        return;
      switch (maskType)
      {
        case ClipAnimationMaskType.CreateFromThisModel:
          AvatarMaskUtility.UpdateTransformMask(this.m_TransformMask, (this.clipInfo.maskTypeProperty.serializedObject.targetObject as ModelImporter).transformPaths, this.humanTransforms, true);
          this.FillNodeInfos();
          break;
        case ClipAnimationMaskType.CopyFromOther:
          AvatarMask objectReferenceValue = this.clipInfo.maskSourceProperty.objectReferenceValue as AvatarMask;
          if ((UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null)
          {
            AvatarMask target = this.target as AvatarMask;
            target.Copy(objectReferenceValue);
            if (this.humanTransforms != null)
              AvatarMaskUtility.SetActiveHumanTransforms(target, this.humanTransforms);
            this.clipInfo.MaskToClip(target);
            this.FillNodeInfos();
            break;
          }
          break;
        case ClipAnimationMaskType.None:
          ModelImporter.UpdateTransformMask(new AvatarMask(), this.clipInfo.transformMaskProperty);
          break;
      }
      this.clipInfo.MaskFromClip(this.target as AvatarMask);
    }

    public void OnBodyInspectorGUI()
    {
      if (!this.m_ShowBodyMask)
        return;
      bool changed = GUI.changed;
      this.m_BodyMaskFoldout = EditorGUILayout.Foldout(this.m_BodyMaskFoldout, AvatarMaskInspector.Styles.BodyMask, true);
      GUI.changed = changed;
      if (this.m_BodyMaskFoldout)
        BodyMaskEditor.Show(this.m_BodyMask, 13);
    }

    public void OnTransformInspectorGUI()
    {
      float xmin = 0.0f;
      float ymin = 0.0f;
      float num = 0.0f;
      float ymax = 0.0f;
      bool changed = GUI.changed;
      this.m_TransformMaskFoldout = EditorGUILayout.Foldout(this.m_TransformMaskFoldout, AvatarMaskInspector.Styles.TransformMask, true);
      GUI.changed = changed;
      if (this.m_TransformMaskFoldout)
      {
        if (this.canImport)
          this.ImportAvatarReference();
        if (this.m_NodeInfos == null || this.m_TransformMask.arraySize != this.m_NodeInfos.Length)
          this.FillNodeInfos();
        if (this.IsMaskEmpty())
        {
          GUILayout.BeginVertical();
          GUILayout.BeginHorizontal(EditorStyles.helpBox, new GUILayoutOption[0]);
          GUILayout.Label(this.animationType != ModelImporterAnimationType.Generic ? (this.animationType != ModelImporterAnimationType.Human ? "No transform mask defined" : "No transform mask defined, only human curves will be imported") : "No transform mask defined, everything will be imported", EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
          GUILayout.EndHorizontal();
          if (!this.canImport && this.clipInfo.maskType == ClipAnimationMaskType.CreateFromThisModel)
          {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create Mask"))
              this.UpdateMask(this.clipInfo.maskType);
            GUILayout.EndHorizontal();
          }
          GUILayout.EndVertical();
        }
        else
        {
          this.ComputeShownElements();
          GUILayout.Space(1f);
          int indentLevel = EditorGUI.indentLevel;
          int arraySize = this.m_TransformMask.arraySize;
          for (int index = 1; index < arraySize; ++index)
          {
            if (this.m_NodeInfos[index].m_Show)
            {
              GUILayout.BeginHorizontal();
              EditorGUI.indentLevel = this.m_NodeInfos[index].m_Depth + 1;
              EditorGUI.BeginChangeCheck();
              Rect rect = GUILayoutUtility.GetRect(15f, 15f, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) });
              GUILayoutUtility.GetRect(10f, 15f, new GUILayoutOption[1]
              {
                GUILayout.ExpandWidth(false)
              });
              rect.x += 15f;
              EditorGUI.BeginDisabledGroup(this.m_NodeInfos[index].m_State == AvatarMaskInspector.NodeInfo.State.disabled || this.m_NodeInfos[index].m_State == AvatarMaskInspector.NodeInfo.State.invalid);
              bool flag1 = Event.current.button == 1;
              bool flag2 = (double) this.m_NodeInfos[index].m_Weight.floatValue > 0.0;
              bool flag3 = GUI.Toggle(rect, flag2, "");
              if (EditorGUI.EndChangeCheck())
              {
                this.m_NodeInfos[index].m_Weight.floatValue = !flag3 ? 0.0f : 1f;
                if (!flag1)
                  this.CheckChildren(index, flag3);
              }
              string str = this.m_NodeInfos[index].m_State != AvatarMaskInspector.NodeInfo.State.invalid ? this.m_NodeInfos[index].m_Name : "<color=#FF0000AA>" + this.m_NodeInfos[index].m_Name + "</color>";
              if (this.m_NodeInfos[index].m_ChildIndices.Count > 0)
                this.m_NodeInfos[index].m_Expanded = EditorGUILayout.Foldout(this.m_NodeInfos[index].m_Expanded, str, true, AvatarMaskInspector.Styles.foldoutStyle);
              else
                EditorGUILayout.LabelField(str, AvatarMaskInspector.Styles.labelStyle, new GUILayoutOption[0]);
              EditorGUI.EndDisabledGroup();
              if (index == 1)
              {
                ymin = rect.yMin;
                xmin = rect.xMin;
              }
              else if (index == arraySize - 1)
                ymax = rect.yMax;
              num = Mathf.Max(num, GUILayoutUtility.GetLastRect().xMax);
              GUILayout.EndHorizontal();
            }
          }
          EditorGUI.indentLevel = indentLevel;
        }
      }
      if (Event.current == null || Event.current.type != EventType.MouseUp || (Event.current.button != 1 || !Rect.MinMaxRect(xmin, ymin, num, ymax).Contains(Event.current.mousePosition)))
        return;
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(new GUIContent("Select all"), false, new GenericMenu.MenuFunction(this.SelectAll));
      genericMenu.AddItem(new GUIContent("Deselect all"), false, new GenericMenu.MenuFunction(this.DeselectAll));
      genericMenu.ShowAsContext();
      Event.current.Use();
    }

    private void SetAllTransformActive(bool active)
    {
      for (int index = 0; index < this.m_NodeInfos.Length; ++index)
      {
        if (this.m_NodeInfos[index].m_State == AvatarMaskInspector.NodeInfo.State.enabled)
          this.m_NodeInfos[index].m_Weight.floatValue = !active ? 0.0f : 1f;
      }
    }

    private void SelectAll()
    {
      this.SetAllTransformActive(true);
    }

    private void DeselectAll()
    {
      this.SetAllTransformActive(false);
    }

    private void ImportAvatarReference()
    {
      EditorGUI.BeginChangeCheck();
      this.m_RefAvatar = EditorGUILayout.ObjectField("Use skeleton from", (UnityEngine.Object) this.m_RefAvatar, typeof (Avatar), true, new GUILayoutOption[0]) as Avatar;
      if (EditorGUI.EndChangeCheck())
        this.m_RefImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) this.m_RefAvatar)) as ModelImporter;
      if (!((UnityEngine.Object) this.m_RefImporter != (UnityEngine.Object) null) || !GUILayout.Button("Import skeleton"))
        return;
      AvatarMaskUtility.UpdateTransformMask(this.m_TransformMask, this.m_RefImporter.transformPaths, (string[]) null, true);
    }

    public void FillNodeInfos()
    {
      this.m_NodeInfos = new AvatarMaskInspector.NodeInfo[this.m_TransformMask.arraySize];
      if (this.m_TransformMask.arraySize == 0)
        return;
      string[] strArray = new string[this.m_TransformMask.arraySize];
      SerializedProperty arrayElementAtIndex = this.m_TransformMask.GetArrayElementAtIndex(0);
      arrayElementAtIndex.Next(false);
      for (int index1 = 1; index1 < this.m_NodeInfos.Length; ++index1)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AvatarMaskInspector.\u003CFillNodeInfos\u003Ec__AnonStorey1 infosCAnonStorey1 = new AvatarMaskInspector.\u003CFillNodeInfos\u003Ec__AnonStorey1();
        this.m_NodeInfos[index1].m_Path = arrayElementAtIndex.FindPropertyRelative("m_Path");
        this.m_NodeInfos[index1].m_Weight = arrayElementAtIndex.FindPropertyRelative("m_Weight");
        strArray[index1] = this.m_NodeInfos[index1].m_Path.stringValue;
        // ISSUE: reference to a compiler-generated field
        infosCAnonStorey1.fullPath = strArray[index1];
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.m_NodeInfos[index1].m_State = !this.m_CanImport ? (this.humanTransforms == null ? (this.m_TransformPaths == null || ArrayUtility.FindIndex<string>(this.m_TransformPaths, new Predicate<string>(infosCAnonStorey1.\u003C\u003Em__2)) != -1 ? AvatarMaskInspector.NodeInfo.State.enabled : AvatarMaskInspector.NodeInfo.State.invalid) : (ArrayUtility.FindIndex<string>(this.humanTransforms, new Predicate<string>(infosCAnonStorey1.\u003C\u003Em__0)) != -1 ? AvatarMaskInspector.NodeInfo.State.disabled : (this.m_TransformPaths == null || ArrayUtility.FindIndex<string>(this.m_TransformPaths, new Predicate<string>(infosCAnonStorey1.\u003C\u003Em__1)) != -1 ? AvatarMaskInspector.NodeInfo.State.enabled : AvatarMaskInspector.NodeInfo.State.invalid))) : AvatarMaskInspector.NodeInfo.State.enabled;
        this.m_NodeInfos[index1].m_Expanded = true;
        this.m_NodeInfos[index1].m_ParentIndex = -1;
        this.m_NodeInfos[index1].m_ChildIndices = new List<int>();
        // ISSUE: reference to a compiler-generated field
        this.m_NodeInfos[index1].m_Depth = index1 != 0 ? infosCAnonStorey1.fullPath.Count<char>((Func<char, bool>) (f => (int) f == 47)) : 0;
        string str1 = "";
        // ISSUE: reference to a compiler-generated field
        int length = infosCAnonStorey1.fullPath.LastIndexOf('/');
        if (length > 0)
        {
          // ISSUE: reference to a compiler-generated field
          str1 = infosCAnonStorey1.fullPath.Substring(0, length);
        }
        int startIndex = length != -1 ? length + 1 : 0;
        // ISSUE: reference to a compiler-generated field
        this.m_NodeInfos[index1].m_Name = infosCAnonStorey1.fullPath.Substring(startIndex);
        for (int index2 = 1; index2 < index1; ++index2)
        {
          string str2 = strArray[index2];
          if (str1 != "" && str2 == str1)
          {
            this.m_NodeInfos[index1].m_ParentIndex = index2;
            this.m_NodeInfos[index2].m_ChildIndices.Add(index1);
          }
        }
        arrayElementAtIndex.Next(false);
      }
    }

    private void ComputeShownElements()
    {
      for (int currentIndex = 0; currentIndex < this.m_NodeInfos.Length; ++currentIndex)
      {
        if (this.m_NodeInfos[currentIndex].m_ParentIndex == -1)
          this.ComputeShownElements(currentIndex, true);
      }
    }

    private void ComputeShownElements(int currentIndex, bool show)
    {
      this.m_NodeInfos[currentIndex].m_Show = show;
      bool show1 = show && this.m_NodeInfos[currentIndex].m_Expanded;
      foreach (int childIndex in this.m_NodeInfos[currentIndex].m_ChildIndices)
        this.ComputeShownElements(childIndex, show1);
    }

    private void CheckChildren(int index, bool value)
    {
      foreach (int childIndex in this.m_NodeInfos[index].m_ChildIndices)
      {
        if (this.m_NodeInfos[childIndex].m_State == AvatarMaskInspector.NodeInfo.State.enabled)
          this.m_NodeInfos[childIndex].m_Weight.floatValue = !value ? 0.0f : 1f;
        this.CheckChildren(childIndex, value);
      }
    }

    private static class Styles
    {
      public static GUIContent MaskDefinition = EditorGUIUtility.TextContent("Definition|Choose between Create From This Model, Copy From Other Avatar. The first one create a Mask for this file and the second one use a Mask from another file to import animation.");
      public static GUIContent[] MaskDefinitionOpt = new GUIContent[3]{ EditorGUIUtility.TextContent("Create From This Model|Create a Mask based on the model from this file. For Humanoid rig all the human transform are always imported and converted to muscle curve, thus they cannot be unchecked."), EditorGUIUtility.TextContent("Copy From Other Mask|Copy a Mask from another file to import animation clip."), EditorGUIUtility.TextContent("None | Import Everything") };
      public static GUIContent BodyMask = EditorGUIUtility.TextContent("Humanoid|Define which body part are active. Also define which animation curves will be imported for an Animation Clip.");
      public static GUIContent TransformMask = EditorGUIUtility.TextContent("Transform|Define which transform are active. Also define which animation curves will be imported for an Animation Clip.");
      public static GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
      public static GUIStyle labelStyle = new GUIStyle(EditorStyles.label);

      static Styles()
      {
        GUIStyle foldoutStyle = AvatarMaskInspector.Styles.foldoutStyle;
        bool flag = true;
        AvatarMaskInspector.Styles.labelStyle.richText = flag;
        int num = flag ? 1 : 0;
        foldoutStyle.richText = num != 0;
      }
    }

    private struct NodeInfo
    {
      public bool m_Expanded;
      public bool m_Show;
      public AvatarMaskInspector.NodeInfo.State m_State;
      public int m_ParentIndex;
      public List<int> m_ChildIndices;
      public int m_Depth;
      public SerializedProperty m_Path;
      public SerializedProperty m_Weight;
      public string m_Name;

      public enum State
      {
        disabled,
        enabled,
        invalid,
      }
    }
  }
}
