// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarMappingEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class AvatarMappingEditor : AvatarSubEditor
  {
    internal static int s_SelectedBoneIndex = -1;
    internal static bool s_DirtySelection = false;
    internal static int s_KeyboardControl = 0;
    protected int m_BodyView = 0;
    protected int[][] m_BodyPartHumanBone = new int[9][]{ new int[1]{ -1 }, new int[4]{ 0, 7, 8, 54 }, new int[5]{ 9, 10, 21, 22, 23 }, new int[4]{ 11, 13, 15, 17 }, new int[15]{ 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38 }, new int[4]{ 12, 14, 16, 18 }, new int[15]{ 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53 }, new int[4]{ 1, 3, 5, 19 }, new int[4]{ 2, 4, 6, 20 } };
    private Vector2 m_FoldoutScroll = Vector2.zero;
    private static AvatarMappingEditor.Styles s_Styles;
    protected bool[] m_BodyPartToggle;
    protected bool[] m_BodyPartFoldout;
    [SerializeField]
    protected AvatarSetupTool.BoneWrapper[] m_Bones;
    protected bool m_HasSkinnedMesh;
    private bool m_IsBiped;
    private Editor m_CurrentTransformEditor;
    private bool m_CurrentTransformEditorFoldout;

    public AvatarMappingEditor()
    {
      this.m_BodyPartToggle = new bool[9];
      this.m_BodyPartFoldout = new bool[9];
      for (int index = 0; index < 9; ++index)
      {
        this.m_BodyPartToggle[index] = false;
        this.m_BodyPartFoldout[index] = true;
      }
    }

    internal static AvatarMappingEditor.Styles styles
    {
      get
      {
        if (AvatarMappingEditor.s_Styles == null)
          AvatarMappingEditor.s_Styles = new AvatarMappingEditor.Styles();
        return AvatarMappingEditor.s_Styles;
      }
    }

    public override void Enable(AvatarEditor inspector)
    {
      base.Enable(inspector);
      this.Init();
    }

    public override void Disable()
    {
      if ((UnityEngine.Object) this.m_CurrentTransformEditor != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_CurrentTransformEditor);
      base.Disable();
    }

    public override void OnDestroy()
    {
      if ((UnityEngine.Object) this.m_CurrentTransformEditor != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_CurrentTransformEditor);
      base.OnDestroy();
    }

    protected void Init()
    {
      if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
        return;
      this.m_IsBiped = AvatarBipedMapper.IsBiped(this.gameObject.transform, (List<string>) null);
      if (this.m_Bones == null)
        this.m_Bones = AvatarSetupTool.GetHumanBones(this.serializedObject, this.modelBones);
      this.ValidateMapping();
      if ((UnityEngine.Object) this.m_CurrentTransformEditor != (UnityEngine.Object) null)
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_CurrentTransformEditor);
        this.m_CurrentTransformEditor = (Editor) null;
      }
      this.m_CurrentTransformEditorFoldout = true;
      this.m_HasSkinnedMesh = (UnityEngine.Object) this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>() != (UnityEngine.Object) null;
      this.InitPose();
      SceneView.RepaintAll();
    }

    protected override void ResetValues()
    {
      base.ResetValues();
      this.ResetBones();
      this.Init();
    }

    protected void ResetBones()
    {
      for (int index = 0; index < this.m_Bones.Length; ++index)
        this.m_Bones[index].Reset(this.serializedObject, this.modelBones);
    }

    protected bool IsValidHuman()
    {
      Animator component = this.gameObject.GetComponent<Animator>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return false;
      Avatar avatar = component.avatar;
      return (UnityEngine.Object) avatar != (UnityEngine.Object) null && avatar.isHuman;
    }

    protected void InitPose()
    {
      if (!this.IsValidHuman())
        return;
      this.gameObject.GetComponent<Animator>().WriteDefaultPose();
      AvatarSetupTool.TransferDescriptionToPose(this.serializedObject, this.root);
    }

    protected void ValidateMapping()
    {
      for (int i = 0; i < this.m_Bones.Length; ++i)
      {
        string error;
        this.m_Bones[i].state = this.GetBoneState(i, out error);
        this.m_Bones[i].error = error;
      }
    }

    private void EnableBodyParts(bool[] toggles, params int[] parts)
    {
      for (int index = 0; index < this.m_BodyPartToggle.Length; ++index)
        toggles[index] = false;
      foreach (int part in parts)
        toggles[part] = true;
    }

    private void HandleBodyView(int bodyView)
    {
      if (bodyView == 0)
        this.EnableBodyParts(this.m_BodyPartToggle, 1, 3, 5, 7, 8);
      if (bodyView == 1)
        this.EnableBodyParts(this.m_BodyPartToggle, 2);
      if (bodyView == 2)
        this.EnableBodyParts(this.m_BodyPartToggle, 4);
      if (bodyView != 3)
        return;
      this.EnableBodyParts(this.m_BodyPartToggle, 6);
    }

    public override void OnInspectorGUI()
    {
      if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed")
      {
        AvatarSetupTool.TransferPoseToDescription(this.serializedObject, this.root);
        for (int index = 0; index < this.m_Bones.Length; ++index)
          this.m_Bones[index].Serialize(this.serializedObject);
      }
      this.UpdateSelectedBone();
      if (AvatarMappingEditor.s_KeyboardControl != 0)
      {
        GUIUtility.keyboardControl = AvatarMappingEditor.s_KeyboardControl;
        AvatarMappingEditor.s_KeyboardControl = 0;
      }
      GUILayout.BeginVertical();
      EditorGUI.BeginChangeCheck();
      GUILayout.BeginVertical("", (GUIStyle) "TE NodeBackground", new GUILayoutOption[0]);
      this.m_BodyView = AvatarControl.ShowBoneMapping(this.m_BodyView, new AvatarControl.BodyPartFeedback(this.IsValidBodyPart), this.m_Bones, this.serializedObject, this);
      this.HandleBodyView(this.m_BodyView);
      GUILayout.EndVertical();
      this.m_FoldoutScroll = GUILayout.BeginScrollView(this.m_FoldoutScroll, AvatarMappingEditor.styles.box, GUILayout.MinHeight(80f), GUILayout.MaxHeight(500f), GUILayout.ExpandHeight(true));
      this.DisplayFoldout();
      GUILayout.FlexibleSpace();
      GUILayout.EndScrollView();
      if (EditorGUI.EndChangeCheck())
      {
        this.ValidateMapping();
        SceneView.RepaintAll();
      }
      this.DisplayMappingButtons();
      GUILayout.EndVertical();
      if (GUIUtility.hotControl == 0)
        this.TransferPoseIfChanged();
      this.ApplyRevertGUI();
      if ((UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.m_CurrentTransformEditor != (UnityEngine.Object) null && this.m_CurrentTransformEditor.target != (UnityEngine.Object) Selection.activeTransform)
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_CurrentTransformEditor);
        if ((UnityEngine.Object) this.m_CurrentTransformEditor == (UnityEngine.Object) null)
          this.m_CurrentTransformEditor = Editor.CreateEditor((UnityEngine.Object) Selection.activeTransform);
        EditorGUILayout.Space();
        this.m_CurrentTransformEditorFoldout = EditorGUILayout.InspectorTitlebar(this.m_CurrentTransformEditorFoldout, (UnityEngine.Object) Selection.activeTransform, true);
        if (!this.m_CurrentTransformEditorFoldout || !((UnityEngine.Object) this.m_CurrentTransformEditor != (UnityEngine.Object) null))
          return;
        this.m_CurrentTransformEditor.OnInspectorGUI();
      }
      else
      {
        if (!((UnityEngine.Object) this.m_CurrentTransformEditor != (UnityEngine.Object) null))
          return;
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_CurrentTransformEditor);
        this.m_CurrentTransformEditor = (Editor) null;
      }
    }

    protected void DebugPoseButtons()
    {
      if (GUILayout.Button("Default Pose") && this.IsValidHuman())
        this.gameObject.GetComponent<Animator>().WriteDefaultPose();
      if (!GUILayout.Button("Description Pose"))
        return;
      AvatarSetupTool.TransferDescriptionToPose(this.serializedObject, this.root);
    }

    protected void TransferPoseIfChanged()
    {
      foreach (GameObject gameObject in Selection.gameObjects)
      {
        if (this.TransformChanged(gameObject.transform))
        {
          AvatarSetupTool.TransferPoseToDescription(this.serializedObject, this.root);
          this.m_Inspector.Repaint();
          break;
        }
      }
    }

    protected void DisplayMappingButtons()
    {
      GUILayout.BeginHorizontal("", AvatarMappingEditor.styles.toolbar, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(true)
      });
      Rect rect1 = GUILayoutUtility.GetRect(AvatarMappingEditor.styles.mapping, AvatarMappingEditor.styles.toolbarDropDown);
      if (GUI.Button(rect1, AvatarMappingEditor.styles.mapping, AvatarMappingEditor.styles.toolbarDropDown))
      {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(AvatarMappingEditor.styles.clearMapping, false, new GenericMenu.MenuFunction(this.ClearMapping));
        if (this.m_IsBiped)
          genericMenu.AddItem(AvatarMappingEditor.styles.bipedMapping, false, new GenericMenu.MenuFunction(this.PerformBipedMapping));
        genericMenu.AddItem(AvatarMappingEditor.styles.autoMapping, false, new GenericMenu.MenuFunction(this.PerformAutoMapping));
        genericMenu.AddItem(AvatarMappingEditor.styles.loadMapping, false, new GenericMenu.MenuFunction(this.ApplyTemplate));
        genericMenu.AddItem(AvatarMappingEditor.styles.saveMapping, false, new GenericMenu.MenuFunction(this.SaveHumanTemplate));
        genericMenu.DropDown(rect1);
      }
      Rect rect2 = GUILayoutUtility.GetRect(AvatarMappingEditor.styles.pose, AvatarMappingEditor.styles.toolbarDropDown);
      if (GUI.Button(rect2, AvatarMappingEditor.styles.pose, AvatarMappingEditor.styles.toolbarDropDown))
      {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(AvatarMappingEditor.styles.resetPose, false, new GenericMenu.MenuFunction(this.CopyPrefabPose));
        if (this.m_IsBiped)
          genericMenu.AddItem(AvatarMappingEditor.styles.bipedPose, false, new GenericMenu.MenuFunction(this.BipedPose));
        if (this.m_HasSkinnedMesh)
          genericMenu.AddItem(AvatarMappingEditor.styles.sampleBindPose, false, new GenericMenu.MenuFunction(this.SampleBindPose));
        else
          genericMenu.AddItem(AvatarMappingEditor.styles.sampleBindPose, false, (GenericMenu.MenuFunction) null);
        genericMenu.AddItem(AvatarMappingEditor.styles.enforceTPose, false, new GenericMenu.MenuFunction(this.MakePoseValid));
        genericMenu.DropDown(rect2);
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
    }

    protected void CopyPrefabPose()
    {
      AvatarSetupTool.CopyPose(this.gameObject, this.prefab);
      AvatarSetupTool.TransferPoseToDescription(this.serializedObject, this.root);
      this.m_Inspector.Repaint();
    }

    protected void SampleBindPose()
    {
      AvatarSetupTool.SampleBindPose(this.gameObject);
      AvatarSetupTool.TransferPoseToDescription(this.serializedObject, this.root);
      this.m_Inspector.Repaint();
    }

    protected void BipedPose()
    {
      AvatarBipedMapper.BipedPose(this.gameObject, this.m_Bones);
      AvatarSetupTool.TransferPoseToDescription(this.serializedObject, this.root);
      this.m_Inspector.Repaint();
    }

    protected void MakePoseValid()
    {
      AvatarSetupTool.MakePoseValid(this.m_Bones);
      AvatarSetupTool.TransferPoseToDescription(this.serializedObject, this.root);
      this.m_Inspector.Repaint();
    }

    protected void PerformAutoMapping()
    {
      this.AutoMapping();
      this.ValidateMapping();
      SceneView.RepaintAll();
    }

    protected void PerformBipedMapping()
    {
      this.BipedMapping();
      this.ValidateMapping();
      SceneView.RepaintAll();
    }

    protected void AutoMapping()
    {
      foreach (KeyValuePair<int, Transform> mapBone in AvatarAutoMapper.MapBones(this.gameObject.transform, this.modelBones))
      {
        AvatarSetupTool.BoneWrapper bone = this.m_Bones[mapBone.Key];
        bone.bone = mapBone.Value;
        bone.Serialize(this.serializedObject);
      }
    }

    protected void BipedMapping()
    {
      foreach (KeyValuePair<int, Transform> mapBone in AvatarBipedMapper.MapBones(this.gameObject.transform))
      {
        AvatarSetupTool.BoneWrapper bone = this.m_Bones[mapBone.Key];
        bone.bone = mapBone.Value;
        bone.Serialize(this.serializedObject);
      }
    }

    protected void ClearMapping()
    {
      if (this.serializedObject == null)
        return;
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Clear Mapping");
      AvatarSetupTool.ClearHumanBoneArray(this.serializedObject);
      this.ResetBones();
      this.ValidateMapping();
      SceneView.RepaintAll();
    }

    protected Vector4 QuaternionToVector4(Quaternion rot)
    {
      return new Vector4(rot.x, rot.y, rot.z, rot.w);
    }

    protected Quaternion Vector4ToQuaternion(Vector4 rot)
    {
      return new Quaternion(rot.x, rot.y, rot.z, rot.w);
    }

    protected bool IsAnyBodyPartActive()
    {
      for (int index = 1; index < this.m_BodyPartToggle.Length; ++index)
      {
        if (this.m_BodyPartToggle[index])
          return true;
      }
      return false;
    }

    private void UpdateSelectedBone()
    {
      int selectedBoneIndex = AvatarMappingEditor.s_SelectedBoneIndex;
      if (AvatarMappingEditor.s_SelectedBoneIndex < 0 || AvatarMappingEditor.s_SelectedBoneIndex >= this.m_Bones.Length || (UnityEngine.Object) this.m_Bones[AvatarMappingEditor.s_SelectedBoneIndex].bone != (UnityEngine.Object) Selection.activeTransform)
      {
        AvatarMappingEditor.s_SelectedBoneIndex = -1;
        if ((UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null)
        {
          for (int index = 0; index < this.m_Bones.Length; ++index)
          {
            if ((UnityEngine.Object) this.m_Bones[index].bone == (UnityEngine.Object) Selection.activeTransform)
            {
              AvatarMappingEditor.s_SelectedBoneIndex = index;
              break;
            }
          }
        }
      }
      if (AvatarMappingEditor.s_SelectedBoneIndex == selectedBoneIndex)
        return;
      List<int> viewsThatContainBone = AvatarControl.GetViewsThatContainBone(AvatarMappingEditor.s_SelectedBoneIndex);
      if (viewsThatContainBone.Count > 0 && !viewsThatContainBone.Contains(this.m_BodyView))
        this.m_BodyView = viewsThatContainBone[0];
    }

    protected void DisplayFoldout()
    {
      Dictionary<Transform, bool> modelBones = this.modelBones;
      EditorGUIUtility.SetIconSize(Vector2.one * 16f);
      EditorGUILayout.BeginHorizontal();
      GUI.color = Color.grey;
      GUILayout.Label(AvatarMappingEditor.styles.dotFrameDotted.image, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      GUI.color = Color.white;
      GUILayout.Label("Optional Bone", new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(true)
      });
      EditorGUILayout.EndHorizontal();
      for (int index1 = 1; index1 < this.m_BodyPartToggle.Length; ++index1)
      {
        if (this.m_BodyPartToggle[index1])
        {
          if (AvatarMappingEditor.s_DirtySelection && !this.m_BodyPartFoldout[index1])
          {
            for (int index2 = 0; index2 < this.m_BodyPartHumanBone[index1].Length; ++index2)
            {
              int num = this.m_BodyPartHumanBone[index1][index2];
              if (AvatarMappingEditor.s_SelectedBoneIndex == num)
                this.m_BodyPartFoldout[index1] = true;
            }
          }
          this.m_BodyPartFoldout[index1] = GUILayout.Toggle(this.m_BodyPartFoldout[index1], AvatarMappingEditor.styles.BodyPartMapping[index1], EditorStyles.foldout, new GUILayoutOption[0]);
          ++EditorGUI.indentLevel;
          if (this.m_BodyPartFoldout[index1])
          {
            for (int index2 = 0; index2 < this.m_BodyPartHumanBone[index1].Length; ++index2)
            {
              int boneIndex = this.m_BodyPartHumanBone[index1][index2];
              if (boneIndex != -1)
              {
                AvatarSetupTool.BoneWrapper bone = this.m_Bones[boneIndex];
                string name = bone.humanBoneName;
                if (index1 == 5 || index1 == 6 || index1 == 8)
                  name = name.Replace("Right", "");
                if (index1 == 3 || index1 == 4 || index1 == 7)
                  name = name.Replace("Left", "");
                string text = ObjectNames.NicifyVariableName(name);
                Rect controlRect = EditorGUILayout.GetControlRect();
                Rect selectRect = controlRect;
                selectRect.width -= 15f;
                Rect rect = new Rect(controlRect.x + EditorGUI.indent, controlRect.y - 1f, 19f, 19f);
                bone.BoneDotGUI(rect, selectRect, boneIndex, true, false, true, this.serializedObject, this);
                controlRect.xMin += 19f;
                Transform key = EditorGUI.ObjectField(controlRect, new GUIContent(text), (UnityEngine.Object) bone.bone, typeof (Transform), true) as Transform;
                if ((UnityEngine.Object) key != (UnityEngine.Object) bone.bone)
                {
                  Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Avatar mapping modified");
                  bone.bone = key;
                  bone.Serialize(this.serializedObject);
                  if ((UnityEngine.Object) key != (UnityEngine.Object) null && !modelBones.ContainsKey(key))
                    modelBones[key] = true;
                }
                if (!string.IsNullOrEmpty(bone.error))
                {
                  GUILayout.BeginHorizontal();
                  GUILayout.Space((float) ((double) EditorGUI.indent + 19.0 + 4.0));
                  GUILayout.Label(bone.error, AvatarMappingEditor.s_Styles.errorLabel, new GUILayoutOption[0]);
                  GUILayout.EndHorizontal();
                }
              }
            }
          }
          --EditorGUI.indentLevel;
        }
      }
      AvatarMappingEditor.s_DirtySelection = false;
      EditorGUIUtility.SetIconSize(Vector2.zero);
    }

    private bool TransformChanged(Transform tr)
    {
      SerializedProperty skeletonBone = AvatarSetupTool.FindSkeletonBone(this.serializedObject, tr, false, false);
      if (skeletonBone != null)
      {
        SerializedProperty propertyRelative1 = skeletonBone.FindPropertyRelative(AvatarSetupTool.sPosition);
        if (propertyRelative1 != null && propertyRelative1.vector3Value != tr.localPosition)
          return true;
        SerializedProperty propertyRelative2 = skeletonBone.FindPropertyRelative(AvatarSetupTool.sRotation);
        if (propertyRelative2 != null && propertyRelative2.quaternionValue != tr.localRotation)
          return true;
        SerializedProperty propertyRelative3 = skeletonBone.FindPropertyRelative(AvatarSetupTool.sScale);
        if (propertyRelative3 != null && propertyRelative3.vector3Value != tr.localScale)
          return true;
      }
      return false;
    }

    protected BoneState GetBoneState(int i, out string error)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMappingEditor.\u003CGetBoneState\u003Ec__AnonStorey0 stateCAnonStorey0 = new AvatarMappingEditor.\u003CGetBoneState\u003Ec__AnonStorey0();
      error = string.Empty;
      // ISSUE: reference to a compiler-generated field
      stateCAnonStorey0.bone = this.m_Bones[i];
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) stateCAnonStorey0.bone.bone == (UnityEngine.Object) null)
        return BoneState.None;
      int humanBoneAncestor = AvatarSetupTool.GetFirstHumanBoneAncestor(this.m_Bones, i);
      AvatarSetupTool.BoneWrapper bone = this.m_Bones[humanBoneAncestor <= 0 ? 0 : humanBoneAncestor];
      // ISSUE: reference to a compiler-generated field
      if (i == 0 && (UnityEngine.Object) stateCAnonStorey0.bone.bone.parent == (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        error = stateCAnonStorey0.bone.messageName + " cannot be the root transform";
        return BoneState.InvalidHierarchy;
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) bone.bone != (UnityEngine.Object) null && !stateCAnonStorey0.bone.bone.IsChildOf(bone.bone))
      {
        // ISSUE: reference to a compiler-generated field
        error = stateCAnonStorey0.bone.messageName + " is not a child of " + bone.messageName + ".";
        return BoneState.InvalidHierarchy;
      }
      if (i == 54 && (UnityEngine.Object) this.m_Bones[8].bone == (UnityEngine.Object) null)
      {
        error = "Chest must be assigned before assigning UpperChest.";
        return BoneState.InvalidHierarchy;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (i != 23 && (UnityEngine.Object) bone.bone != (UnityEngine.Object) null && (UnityEngine.Object) bone.bone != (UnityEngine.Object) stateCAnonStorey0.bone.bone && (double) (stateCAnonStorey0.bone.bone.position - bone.bone.position).sqrMagnitude < (double) Mathf.Epsilon)
      {
        // ISSUE: reference to a compiler-generated field
        error = stateCAnonStorey0.bone.messageName + " has bone length of zero.";
        return BoneState.BoneLenghtIsZero;
      }
      // ISSUE: reference to a compiler-generated method
      if (((IEnumerable<AvatarSetupTool.BoneWrapper>) this.m_Bones).Where<AvatarSetupTool.BoneWrapper>(new Func<AvatarSetupTool.BoneWrapper, bool>(stateCAnonStorey0.\u003C\u003Em__0)).Count<AvatarSetupTool.BoneWrapper>() <= 1)
        return BoneState.Valid;
      // ISSUE: reference to a compiler-generated field
      error = stateCAnonStorey0.bone.messageName + " is also assigned to ";
      bool flag = true;
      for (int index = 0; index < this.m_Bones.Length; ++index)
      {
        if (i != index && (UnityEngine.Object) this.m_Bones[i].bone == (UnityEngine.Object) this.m_Bones[index].bone)
        {
          if (flag)
            flag = false;
          else
            error += ", ";
          error += ObjectNames.NicifyVariableName(this.m_Bones[index].humanBoneName);
        }
      }
      error += ".";
      return BoneState.Duplicate;
    }

    protected AvatarControl.BodyPartColor IsValidBodyPart(BodyPart bodyPart)
    {
      AvatarControl.BodyPartColor bodyPartColor = AvatarControl.BodyPartColor.Off;
      bool flag1 = false;
      int index1 = (int) bodyPart;
      if (bodyPart != BodyPart.LeftFingers && bodyPart != BodyPart.RightFingers)
      {
        for (int index2 = 0; index2 < this.m_BodyPartHumanBone[index1].Length; ++index2)
        {
          if (this.m_BodyPartHumanBone[index1][index2] != -1)
          {
            BoneState state = this.m_Bones[this.m_BodyPartHumanBone[index1][index2]].state;
            flag1 |= state == BoneState.Valid;
            if (HumanTrait.RequiredBone(this.m_BodyPartHumanBone[index1][index2]) && state == BoneState.None || state != BoneState.Valid && state != BoneState.None)
              return AvatarControl.BodyPartColor.Red;
          }
        }
      }
      else
      {
        bool flag2 = true;
        int num1 = 3;
        for (int index2 = 0; index2 < this.m_BodyPartHumanBone[index1].Length / num1; ++index2)
        {
          bool flag3 = false;
          int num2 = index2 * num1;
          for (int index3 = num1 - 1; index3 >= 0; --index3)
          {
            bool flag4 = this.m_Bones[this.m_BodyPartHumanBone[index1][num2 + index3]].state == BoneState.Valid;
            flag2 &= flag4;
            if (flag3)
            {
              if (!flag4)
                return AvatarControl.BodyPartColor.Red | AvatarControl.BodyPartColor.IKRed;
            }
            else
              flag1 |= flag3 = !flag3 && flag4;
          }
        }
        bodyPartColor = !flag2 ? AvatarControl.BodyPartColor.IKRed : AvatarControl.BodyPartColor.IKGreen;
      }
      if (!flag1)
        return AvatarControl.BodyPartColor.IKRed;
      return AvatarControl.BodyPartColor.Green | bodyPartColor;
    }

    private HumanTemplate OpenHumanTemplate()
    {
      string directory = "Assets/";
      string str = EditorUtility.OpenFilePanel("Open Human Template", directory, "ht");
      if (str == "")
        return (HumanTemplate) null;
      HumanTemplate humanTemplate = AssetDatabase.LoadMainAssetAtPath(FileUtil.GetProjectRelativePath(str)) as HumanTemplate;
      if ((UnityEngine.Object) humanTemplate == (UnityEngine.Object) null && EditorUtility.DisplayDialog("Human Template not found in project", "Import asset '" + str + "' into project", "Yes", "No"))
      {
        string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(directory + FileUtil.GetLastPathNameComponent(str));
        FileUtil.CopyFileOrDirectory(str, uniqueAssetPath);
        AssetDatabase.Refresh();
        humanTemplate = AssetDatabase.LoadMainAssetAtPath(uniqueAssetPath) as HumanTemplate;
        if ((UnityEngine.Object) humanTemplate == (UnityEngine.Object) null)
          Debug.Log((object) ("Failed importing file '" + str + "' to '" + uniqueAssetPath + "'"));
      }
      return humanTemplate;
    }

    public static bool MatchName(string transformName, string boneName)
    {
      char[] charArray = ":".ToCharArray();
      string[] strArray1 = transformName.Split(charArray);
      string[] strArray2 = boneName.Split(charArray);
      return transformName == boneName || strArray1.Length > 1 && strArray1[1] == boneName || strArray2.Length > 1 && transformName == strArray2[1] || strArray1.Length > 1 && strArray2.Length > 1 && strArray1[1] == strArray2[1];
    }

    protected void ApplyTemplate()
    {
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Apply Template");
      HumanTemplate humanTemplate = this.OpenHumanTemplate();
      if ((UnityEngine.Object) humanTemplate == (UnityEngine.Object) null)
        return;
      for (int index = 0; index < this.m_Bones.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AvatarMappingEditor.\u003CApplyTemplate\u003Ec__AnonStorey1 templateCAnonStorey1 = new AvatarMappingEditor.\u003CApplyTemplate\u003Ec__AnonStorey1();
        // ISSUE: reference to a compiler-generated field
        templateCAnonStorey1.boneName = humanTemplate.Find(this.m_Bones[index].humanBoneName);
        // ISSUE: reference to a compiler-generated field
        if (templateCAnonStorey1.boneName.Length > 0)
        {
          // ISSUE: reference to a compiler-generated method
          Transform transform = this.modelBones.Keys.FirstOrDefault<Transform>(new Func<Transform, bool>(templateCAnonStorey1.\u003C\u003Em__0));
          this.m_Bones[index].bone = transform;
        }
        else
          this.m_Bones[index].bone = (Transform) null;
        this.m_Bones[index].Serialize(this.serializedObject);
      }
      this.ValidateMapping();
      SceneView.RepaintAll();
    }

    private void SaveHumanTemplate()
    {
      string path = EditorUtility.SaveFilePanelInProject("Create New Human Template", "New Human Template", "ht", "Create a new human template");
      if (path == "")
        return;
      HumanTemplate humanTemplate = new HumanTemplate();
      humanTemplate.ClearTemplate();
      for (int index = 0; index < this.m_Bones.Length; ++index)
      {
        if ((UnityEngine.Object) this.m_Bones[index].bone != (UnityEngine.Object) null)
          humanTemplate.Insert(this.m_Bones[index].humanBoneName, this.m_Bones[index].bone.name);
      }
      AssetDatabase.CreateAsset((UnityEngine.Object) humanTemplate, path);
    }

    public override void OnSceneGUI()
    {
      if (AvatarMappingEditor.s_Styles == null)
        return;
      AvatarSkeletonDrawer.DrawSkeleton(this.root, this.modelBones, this.m_Bones);
      if (GUIUtility.hotControl != 0)
        return;
      this.TransferPoseIfChanged();
    }

    internal class Styles
    {
      public GUIContent[] BodyPartMapping = new GUIContent[9]{ EditorGUIUtility.TextContent("Avatar"), EditorGUIUtility.TextContent("Body"), EditorGUIUtility.TextContent("Head"), EditorGUIUtility.TextContent("Left Arm"), EditorGUIUtility.TextContent("Left Fingers"), EditorGUIUtility.TextContent("Right Arm"), EditorGUIUtility.TextContent("Right Fingers"), EditorGUIUtility.TextContent("Left Leg"), EditorGUIUtility.TextContent("Right Leg") };
      public GUIContent RequiredBone = EditorGUIUtility.TextContent("Optional Bones");
      public GUIContent DoneCharacter = EditorGUIUtility.TextContent("Done");
      public GUIContent mapping = EditorGUIUtility.TextContent("Mapping");
      public GUIContent clearMapping = EditorGUIUtility.TextContent("Clear");
      public GUIContent autoMapping = EditorGUIUtility.TextContent("Automap");
      public GUIContent bipedMapping = EditorGUIUtility.TextContent("Biped");
      public GUIContent loadMapping = EditorGUIUtility.TextContent("Load");
      public GUIContent saveMapping = EditorGUIUtility.TextContent("Save");
      public GUIContent pose = EditorGUIUtility.TextContent("Pose");
      public GUIContent resetPose = EditorGUIUtility.TextContent("Reset");
      public GUIContent sampleBindPose = EditorGUIUtility.TextContent("Sample Bind-Pose");
      public GUIContent enforceTPose = EditorGUIUtility.TextContent("Enforce T-Pose");
      public GUIContent bipedPose = EditorGUIUtility.TextContent("Biped Pose");
      public GUIContent ShowError = EditorGUIUtility.TextContent("Show Error (s)...");
      public GUIContent CloseError = EditorGUIUtility.TextContent("Close Error (s)");
      public GUIContent dotFill = EditorGUIUtility.IconContent("AvatarInspector/DotFill");
      public GUIContent dotFrame = EditorGUIUtility.IconContent("AvatarInspector/DotFrame");
      public GUIContent dotFrameDotted = EditorGUIUtility.IconContent("AvatarInspector/DotFrameDotted");
      public GUIContent dotSelection = EditorGUIUtility.IconContent("AvatarInspector/DotSelection");
      public GUIStyle box = new GUIStyle((GUIStyle) nameof (box));
      public GUIStyle toolbar = (GUIStyle) "TE Toolbar";
      public GUIStyle toolbarDropDown = (GUIStyle) "TE ToolbarDropDown";
      public GUIStyle errorLabel = new GUIStyle(EditorStyles.wordWrappedMiniLabel);

      public Styles()
      {
        this.box.padding = new RectOffset(0, 0, 0, 0);
        this.box.margin = new RectOffset(0, 0, 0, 0);
        this.errorLabel.normal.textColor = new Color(0.6f, 0.0f, 0.0f, 1f);
      }
    }
  }
}
