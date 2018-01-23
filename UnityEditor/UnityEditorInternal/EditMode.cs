// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.EditMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  [InitializeOnLoad]
  public class EditMode
  {
    private static bool s_Debug = false;
    private static UnityEditor.Tool s_ToolBeforeEnteringEditMode = UnityEditor.Tool.Move;
    private const string kEditModeStringKey = "EditModeState";
    private const string kPrevToolStringKey = "EditModePrevTool";
    private const string kOwnerStringKey = "EditModeOwner";
    private const float k_EditColliderbuttonWidth = 33f;
    private const float k_EditColliderbuttonHeight = 23f;
    private const float k_SpaceBetweenLabelAndButton = 5f;
    public static EditMode.OnEditModeStopFunc onEditModeEndDelegate;
    public static EditMode.OnEditModeStartFunc onEditModeStartDelegate;
    private static int s_OwnerID;
    private static EditMode.SceneViewEditMode s_EditMode;

    static EditMode()
    {
      EditMode.ownerID = SessionState.GetInt("EditModeOwner", EditMode.ownerID);
      EditMode.editMode = (EditMode.SceneViewEditMode) SessionState.GetInt("EditModeState", (int) EditMode.editMode);
      EditMode.toolBeforeEnteringEditMode = (UnityEditor.Tool) SessionState.GetInt("EditModePrevTool", (int) EditMode.toolBeforeEnteringEditMode);
      Action selectionChanged = Selection.selectionChanged;
      // ISSUE: reference to a compiler-generated field
      if (EditMode.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditMode.\u003C\u003Ef__mg\u0024cache0 = new Action(EditMode.OnSelectionChange);
      }
      // ISSUE: reference to a compiler-generated field
      Action fMgCache0 = EditMode.\u003C\u003Ef__mg\u0024cache0;
      Selection.selectionChanged = selectionChanged + fMgCache0;
      if (!EditMode.s_Debug)
        return;
      Debug.Log((object) ("EditMode static constructor: " + (object) EditMode.ownerID + " " + (object) EditMode.editMode + " " + (object) EditMode.toolBeforeEnteringEditMode));
    }

    internal static event Action<IToolModeOwner> editModeEnded;

    internal static event Action<IToolModeOwner, EditMode.SceneViewEditMode> editModeStarted;

    private static UnityEditor.Tool toolBeforeEnteringEditMode
    {
      get
      {
        return EditMode.s_ToolBeforeEnteringEditMode;
      }
      set
      {
        EditMode.s_ToolBeforeEnteringEditMode = value;
        SessionState.SetInt("EditModePrevTool", (int) EditMode.s_ToolBeforeEnteringEditMode);
        if (!EditMode.s_Debug)
          return;
        Debug.Log((object) ("Set toolBeforeEnteringEditMode " + (object) value));
      }
    }

    public static bool IsOwner(Editor editor)
    {
      return EditMode.IsOwner((IToolModeOwner) editor);
    }

    internal static bool IsOwner(IToolModeOwner owner)
    {
      return owner.GetInstanceID() == EditMode.s_OwnerID;
    }

    private static int ownerID
    {
      get
      {
        return EditMode.s_OwnerID;
      }
      set
      {
        EditMode.s_OwnerID = value;
        SessionState.SetInt("EditModeOwner", EditMode.s_OwnerID);
        if (!EditMode.s_Debug)
          return;
        Debug.Log((object) ("Set ownerID " + (object) value));
      }
    }

    public static EditMode.SceneViewEditMode editMode
    {
      get
      {
        return EditMode.s_EditMode;
      }
      private set
      {
        if (EditMode.s_EditMode == EditMode.SceneViewEditMode.None && value != EditMode.SceneViewEditMode.None)
        {
          EditMode.toolBeforeEnteringEditMode = Tools.current == UnityEditor.Tool.None ? UnityEditor.Tool.Move : Tools.current;
          Tools.current = UnityEditor.Tool.None;
        }
        else if (EditMode.s_EditMode != EditMode.SceneViewEditMode.None && value == EditMode.SceneViewEditMode.None)
          EditMode.ResetToolToPrevious();
        EditMode.s_EditMode = value;
        SessionState.SetInt("EditModeState", (int) EditMode.s_EditMode);
        if (!EditMode.s_Debug)
          return;
        Debug.Log((object) ("Set editMode " + (object) EditMode.s_EditMode));
      }
    }

    public static void ResetToolToPrevious()
    {
      if (Tools.current != UnityEditor.Tool.None)
        return;
      Tools.current = EditMode.toolBeforeEnteringEditMode;
    }

    private static void EndSceneViewEditing()
    {
      EditMode.ChangeEditMode(EditMode.SceneViewEditMode.None, new Bounds(Vector3.zero, Vector3.positiveInfinity), (Editor) null);
    }

    public static void OnSelectionChange()
    {
      IToolModeOwner objectFromInstanceId = InternalEditorUtility.GetObjectFromInstanceID(EditMode.ownerID) as IToolModeOwner;
      if (objectFromInstanceId != null && objectFromInstanceId.ModeSurvivesSelectionChange((int) EditMode.s_EditMode))
        return;
      EditMode.QuitEditMode();
    }

    public static void QuitEditMode()
    {
      if (Tools.current == UnityEditor.Tool.None && EditMode.editMode != EditMode.SceneViewEditMode.None)
        EditMode.ResetToolToPrevious();
      EditMode.EndSceneViewEditing();
    }

    private static void DetectMainToolChange()
    {
      if (Tools.current == UnityEditor.Tool.None || EditMode.editMode == EditMode.SceneViewEditMode.None)
        return;
      EditMode.EndSceneViewEditing();
    }

    [Obsolete("Use signature passing Func<Bounds> rather than Bounds.")]
    public static void DoEditModeInspectorModeButton(EditMode.SceneViewEditMode mode, string label, GUIContent icon, Bounds bounds, Editor caller)
    {
      EditMode.DoEditModeInspectorModeButton(mode, label, icon, (Func<Bounds>) (() => bounds), (IToolModeOwner) caller);
    }

    public static void DoEditModeInspectorModeButton(EditMode.SceneViewEditMode mode, string label, GUIContent icon, Func<Bounds> getBoundsOfTargets, Editor caller)
    {
      EditMode.DoEditModeInspectorModeButton(mode, label, icon, getBoundsOfTargets, (IToolModeOwner) caller);
    }

    internal static void DoEditModeInspectorModeButton(EditMode.SceneViewEditMode mode, string label, GUIContent icon, IToolModeOwner owner)
    {
      EditMode.DoEditModeInspectorModeButton(mode, label, icon, (Func<Bounds>) null, owner);
    }

    private static void DoEditModeInspectorModeButton(EditMode.SceneViewEditMode mode, string label, GUIContent icon, Func<Bounds> getBoundsOfTargets, IToolModeOwner owner)
    {
      EditMode.DetectMainToolChange();
      Rect controlRect = EditorGUILayout.GetControlRect(true, 23f, EditMode.Styles.singleButtonStyle, new GUILayoutOption[0]);
      Rect position1 = new Rect(controlRect.xMin + EditorGUIUtility.labelWidth, controlRect.yMin, 33f, 23f);
      Vector2 vector2 = GUI.skin.label.CalcSize(new GUIContent(label));
      Rect position2 = new Rect(position1.xMax + 5f, controlRect.yMin + (float) (((double) controlRect.height - (double) vector2.y) * 0.5), vector2.x, controlRect.height);
      int instanceId = owner.GetInstanceID();
      bool flag1 = EditMode.editMode == mode && EditMode.ownerID == instanceId;
      EditorGUI.BeginChangeCheck();
      bool flag2 = false;
      using (new EditorGUI.DisabledScope(!owner.areToolModesAvailable))
      {
        flag2 = GUI.Toggle(position1, flag1, icon, EditMode.Styles.singleButtonStyle);
        GUI.Label(position2, label);
      }
      if (!EditorGUI.EndChangeCheck())
        return;
      EditMode.ChangeEditMode(!flag2 ? EditMode.SceneViewEditMode.None : mode, getBoundsOfTargets != null ? getBoundsOfTargets() : owner.GetWorldBoundsOfTargets(), owner);
    }

    [Obsolete("Use signature passing Func<Bounds> rather than Bounds.")]
    public static void DoInspectorToolbar(EditMode.SceneViewEditMode[] modes, GUIContent[] guiContents, Bounds bounds, Editor caller)
    {
      EditMode.DoInspectorToolbar(modes, guiContents, (Func<Bounds>) (() => bounds), (IToolModeOwner) caller);
    }

    public static void DoInspectorToolbar(EditMode.SceneViewEditMode[] modes, GUIContent[] guiContents, Func<Bounds> getBoundsOfTargets, Editor caller)
    {
      EditMode.DoInspectorToolbar(modes, guiContents, getBoundsOfTargets, (IToolModeOwner) caller);
    }

    internal static void DoInspectorToolbar(EditMode.SceneViewEditMode[] modes, GUIContent[] guiContents, IToolModeOwner owner)
    {
      EditMode.DoInspectorToolbar(modes, guiContents, (Func<Bounds>) null, owner);
    }

    private static void DoInspectorToolbar(EditMode.SceneViewEditMode[] modes, GUIContent[] guiContents, Func<Bounds> getBoundsOfTargets, IToolModeOwner owner)
    {
      EditMode.DetectMainToolChange();
      int instanceId = owner.GetInstanceID();
      int selected = ArrayUtility.IndexOf<EditMode.SceneViewEditMode>(modes, EditMode.editMode);
      if (EditMode.ownerID != instanceId)
        selected = -1;
      EditorGUI.BeginChangeCheck();
      int index = selected;
      using (new EditorGUI.DisabledScope(!owner.areToolModesAvailable))
        index = GUILayout.Toolbar(selected, guiContents, EditMode.Styles.multiButtonStyle, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      EditMode.ChangeEditMode(index != selected ? modes[index] : EditMode.SceneViewEditMode.None, getBoundsOfTargets != null ? getBoundsOfTargets() : owner.GetWorldBoundsOfTargets(), owner);
    }

    public static void ChangeEditMode(EditMode.SceneViewEditMode mode, Bounds bounds, Editor caller)
    {
      EditMode.ChangeEditMode(mode, bounds, (IToolModeOwner) caller);
    }

    internal static void ChangeEditMode(EditMode.SceneViewEditMode mode, IToolModeOwner owner)
    {
      EditMode.ChangeEditMode(mode, owner.GetWorldBoundsOfTargets(), owner);
    }

    internal static void ChangeEditMode(EditMode.SceneViewEditMode mode, Bounds bounds, IToolModeOwner owner)
    {
      IToolModeOwner objectFromInstanceId = InternalEditorUtility.GetObjectFromInstanceID(EditMode.ownerID) as IToolModeOwner;
      EditMode.editMode = mode;
      EditMode.ownerID = mode == EditMode.SceneViewEditMode.None ? 0 : owner.GetInstanceID();
      if (EditMode.onEditModeEndDelegate != null && objectFromInstanceId is Editor)
        EditMode.onEditModeEndDelegate(objectFromInstanceId as Editor);
      // ISSUE: reference to a compiler-generated field
      if (EditMode.editModeEnded != null)
      {
        // ISSUE: reference to a compiler-generated field
        EditMode.editModeEnded(objectFromInstanceId);
      }
      if (EditMode.editMode != EditMode.SceneViewEditMode.None)
      {
        if (EditMode.onEditModeStartDelegate != null && owner is Editor)
          EditMode.onEditModeStartDelegate(owner as Editor, EditMode.editMode);
        // ISSUE: reference to a compiler-generated field
        if (EditMode.editModeStarted != null)
        {
          // ISSUE: reference to a compiler-generated field
          EditMode.editModeStarted(owner, EditMode.editMode);
        }
      }
      EditMode.EditModeChanged(bounds);
      InspectorWindow.RepaintAllInspectors();
    }

    private static void EditModeChanged(Bounds bounds)
    {
      if (EditMode.editMode != EditMode.SceneViewEditMode.None && (UnityEngine.Object) SceneView.lastActiveSceneView != (UnityEngine.Object) null && ((UnityEngine.Object) SceneView.lastActiveSceneView.camera != (UnityEngine.Object) null && !EditMode.SeenByCamera(SceneView.lastActiveSceneView.camera, bounds)))
        SceneView.lastActiveSceneView.Frame(bounds, EditorApplication.isPlaying);
      SceneView.RepaintAll();
    }

    private static bool SeenByCamera(Camera camera, Bounds bounds)
    {
      return EditMode.AnyPointSeenByCamera(camera, EditMode.GetPoints(bounds));
    }

    private static Vector3[] GetPoints(Bounds bounds)
    {
      return EditMode.BoundsToPoints(bounds);
    }

    private static Vector3[] BoundsToPoints(Bounds bounds)
    {
      return new Vector3[8]{ new Vector3(bounds.min.x, bounds.min.y, bounds.min.z), new Vector3(bounds.min.x, bounds.min.y, bounds.max.z), new Vector3(bounds.min.x, bounds.max.y, bounds.min.z), new Vector3(bounds.min.x, bounds.max.y, bounds.max.z), new Vector3(bounds.max.x, bounds.min.y, bounds.min.z), new Vector3(bounds.max.x, bounds.min.y, bounds.max.z), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z), new Vector3(bounds.max.x, bounds.max.y, bounds.max.z) };
    }

    private static bool AnyPointSeenByCamera(Camera camera, Vector3[] points)
    {
      foreach (Vector3 point in points)
      {
        if (EditMode.PointSeenByCamera(camera, point))
          return true;
      }
      return false;
    }

    private static bool PointSeenByCamera(Camera camera, Vector3 point)
    {
      Vector3 viewportPoint = camera.WorldToViewportPoint(point);
      return (double) viewportPoint.x > 0.0 && (double) viewportPoint.x < 1.0 && (double) viewportPoint.y > 0.0 && (double) viewportPoint.y < 1.0;
    }

    private static class Styles
    {
      public static readonly GUIStyle multiButtonStyle = (GUIStyle) "Command";
      public static readonly GUIStyle singleButtonStyle = new GUIStyle((GUIStyle) "Button");

      static Styles()
      {
        EditMode.Styles.singleButtonStyle.padding = EditMode.Styles.multiButtonStyle.padding;
        EditMode.Styles.singleButtonStyle.margin = EditMode.Styles.multiButtonStyle.margin;
      }
    }

    public delegate void OnEditModeStopFunc(Editor editor);

    public delegate void OnEditModeStartFunc(Editor editor, EditMode.SceneViewEditMode mode);

    public enum SceneViewEditMode
    {
      None,
      Collider,
      ClothConstraints,
      ClothSelfAndInterCollisionParticles,
      ReflectionProbeBox,
      ReflectionProbeOrigin,
      LightProbeProxyVolumeBox,
      LightProbeProxyVolumeOrigin,
      LightProbeGroup,
      ParticleSystemCollisionModulePlanesMove,
      ParticleSystemCollisionModulePlanesRotate,
      JointAngularLimits,
      GridPainting,
      GridPicking,
      GridEraser,
      GridFloodFill,
      GridBox,
      GridSelect,
      GridMove,
    }
  }
}
