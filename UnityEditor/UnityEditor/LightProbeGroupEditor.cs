// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightProbeGroupEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace UnityEditor
{
  internal class LightProbeGroupEditor : IEditablePoint
  {
    private static readonly Color kCloudColor = new Color(0.7843137f, 0.7843137f, 0.07843138f, 0.85f);
    private static readonly Color kSelectedCloudColor = new Color(0.3f, 0.6f, 1f, 1f);
    private List<int> m_Selection = new List<int>();
    private Vector3 m_LastPosition = Vector3.zero;
    private Quaternion m_LastRotation = Quaternion.identity;
    private Vector3 m_LastScale = Vector3.one;
    private bool m_Editing;
    private List<Vector3> m_SourcePositions;
    private LightProbeGroupSelection m_SerializedSelectedProbes;
    private readonly LightProbeGroup m_Group;
    private bool m_ShouldRecalculateTetrahedra;
    private LightProbeGroupInspector m_Inspector;

    public LightProbeGroupEditor(LightProbeGroup group, LightProbeGroupInspector inspector)
    {
      this.m_Group = group;
      this.MarkTetrahedraDirty();
      this.m_SerializedSelectedProbes = ScriptableObject.CreateInstance<LightProbeGroupSelection>();
      this.m_SerializedSelectedProbes.hideFlags = HideFlags.HideAndDontSave;
      this.m_Inspector = inspector;
      this.drawTetrahedra = true;
    }

    public bool drawTetrahedra { get; set; }

    public void SetEditing(bool editing)
    {
      this.m_Editing = editing;
    }

    public void AddProbe(Vector3 position)
    {
      Undo.RegisterCompleteObjectUndo(new UnityEngine.Object[2]
      {
        (UnityEngine.Object) this.m_Group,
        (UnityEngine.Object) this.m_SerializedSelectedProbes
      }, "Add Probe");
      this.m_SourcePositions.Add(position);
      this.SelectProbe(this.m_SourcePositions.Count - 1);
      this.MarkTetrahedraDirty();
    }

    private void SelectProbe(int i)
    {
      if (this.m_Selection.Contains(i))
        return;
      this.m_Selection.Add(i);
    }

    public void SelectAllProbes()
    {
      this.DeselectProbes();
      int count = this.m_SourcePositions.Count;
      for (int index = 0; index < count; ++index)
        this.m_Selection.Add(index);
    }

    public void DeselectProbes()
    {
      this.m_Selection.Clear();
      this.m_SerializedSelectedProbes.m_Selection = this.m_Selection;
    }

    private IEnumerable<Vector3> SelectedProbePositions()
    {
      return (IEnumerable<Vector3>) this.m_Selection.Select<int, Vector3>((Func<int, Vector3>) (t => this.m_SourcePositions[t])).ToList<Vector3>();
    }

    public void DuplicateSelectedProbes()
    {
      if (this.m_Selection.Count == 0)
        return;
      Undo.RegisterCompleteObjectUndo(new UnityEngine.Object[2]
      {
        (UnityEngine.Object) this.m_Group,
        (UnityEngine.Object) this.m_SerializedSelectedProbes
      }, "Duplicate Probes");
      foreach (Vector3 selectedProbePosition in this.SelectedProbePositions())
        this.m_SourcePositions.Add(selectedProbePosition);
      this.MarkTetrahedraDirty();
    }

    private void CopySelectedProbes()
    {
      IEnumerable<Vector3> source = this.SelectedProbePositions();
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (Vector3[]));
      StringWriter stringWriter = new StringWriter();
      xmlSerializer.Serialize((TextWriter) stringWriter, (object) source.Select<Vector3, Vector3>((Func<Vector3, Vector3>) (pos => this.m_Group.transform.TransformPoint(pos))).ToArray<Vector3>());
      stringWriter.Close();
      GUIUtility.systemCopyBuffer = stringWriter.ToString();
    }

    private static bool CanPasteProbes()
    {
      try
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (Vector3[]));
        StringReader stringReader = new StringReader(GUIUtility.systemCopyBuffer);
        xmlSerializer.Deserialize((TextReader) stringReader);
        stringReader.Close();
        return true;
      }
      catch
      {
        return false;
      }
    }

    private bool PasteProbes()
    {
      try
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (Vector3[]));
        StringReader stringReader = new StringReader(GUIUtility.systemCopyBuffer);
        Vector3[] vector3Array = (Vector3[]) xmlSerializer.Deserialize((TextReader) stringReader);
        stringReader.Close();
        if (vector3Array.Length == 0)
          return false;
        Undo.RegisterCompleteObjectUndo(new UnityEngine.Object[2]
        {
          (UnityEngine.Object) this.m_Group,
          (UnityEngine.Object) this.m_SerializedSelectedProbes
        }, "Paste Probes");
        int count = this.m_SourcePositions.Count;
        foreach (Vector3 position in vector3Array)
          this.m_SourcePositions.Add(this.m_Group.transform.InverseTransformPoint(position));
        this.DeselectProbes();
        for (int i = count; i < count + vector3Array.Length; ++i)
          this.SelectProbe(i);
        this.MarkTetrahedraDirty();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public void RemoveSelectedProbes()
    {
      if (this.m_Selection.Count == 0)
        return;
      Undo.RegisterCompleteObjectUndo(new UnityEngine.Object[2]
      {
        (UnityEngine.Object) this.m_Group,
        (UnityEngine.Object) this.m_SerializedSelectedProbes
      }, "Delete Probes");
      foreach (int index in (IEnumerable<int>) this.m_Selection.OrderByDescending<int, int>((Func<int, int>) (x => x)))
        this.m_SourcePositions.RemoveAt(index);
      this.DeselectProbes();
      this.MarkTetrahedraDirty();
    }

    public void PullProbePositions()
    {
      if (!((UnityEngine.Object) this.m_Group != (UnityEngine.Object) null) || !((UnityEngine.Object) this.m_SerializedSelectedProbes != (UnityEngine.Object) null))
        return;
      this.m_SourcePositions = new List<Vector3>((IEnumerable<Vector3>) this.m_Group.probePositions);
      this.m_Selection = new List<int>((IEnumerable<int>) this.m_SerializedSelectedProbes.m_Selection);
    }

    public void PushProbePositions()
    {
      this.m_Group.probePositions = this.m_SourcePositions.ToArray();
      this.m_SerializedSelectedProbes.m_Selection = this.m_Selection;
    }

    private void DrawTetrahedra()
    {
      if (Event.current.type != EventType.Repaint || !(bool) ((UnityEngine.Object) SceneView.lastActiveSceneView))
        return;
      LightProbeVisualization.DrawTetrahedra(this.m_ShouldRecalculateTetrahedra, SceneView.lastActiveSceneView.camera.transform.position);
      this.m_ShouldRecalculateTetrahedra = false;
    }

    public void HandleEditMenuHotKeyCommands()
    {
      if (Event.current.type != EventType.ValidateCommand && Event.current.type != EventType.ExecuteCommand)
        return;
      bool flag = Event.current.type == EventType.ExecuteCommand;
      switch (Event.current.commandName)
      {
        case "SoftDelete":
        case "Delete":
          if (flag)
            this.RemoveSelectedProbes();
          Event.current.Use();
          break;
        case "Duplicate":
          if (flag)
            this.DuplicateSelectedProbes();
          Event.current.Use();
          break;
        case "SelectAll":
          if (flag)
            this.SelectAllProbes();
          Event.current.Use();
          break;
        case "Cut":
          if (flag)
          {
            this.CopySelectedProbes();
            this.RemoveSelectedProbes();
          }
          Event.current.Use();
          break;
        case "Copy":
          if (flag)
            this.CopySelectedProbes();
          Event.current.Use();
          break;
      }
    }

    public static void TetrahedralizeSceneProbes(out Vector3[] positions, out int[] indices)
    {
      LightProbeGroup[] objectsOfType = UnityEngine.Object.FindObjectsOfType(typeof (LightProbeGroup)) as LightProbeGroup[];
      if (objectsOfType == null)
      {
        positions = new Vector3[0];
        indices = new int[0];
      }
      else
      {
        List<Vector3> vector3List = new List<Vector3>();
        foreach (LightProbeGroup lightProbeGroup in objectsOfType)
        {
          foreach (Vector3 probePosition in lightProbeGroup.probePositions)
          {
            Vector3 vector3 = lightProbeGroup.transform.TransformPoint(probePosition);
            vector3List.Add(vector3);
          }
        }
        if (vector3List.Count == 0)
        {
          positions = new Vector3[0];
          indices = new int[0];
        }
        else
          Lightmapping.Tetrahedralize(vector3List.ToArray(), out indices, out positions);
      }
    }

    public bool OnSceneGUI(Transform transform)
    {
      if (!this.m_Group.enabled)
        return this.m_Editing;
      if (Event.current.type == EventType.Layout)
      {
        if (this.m_LastPosition != this.m_Group.transform.position || this.m_LastRotation != this.m_Group.transform.rotation || this.m_LastScale != this.m_Group.transform.localScale)
          this.MarkTetrahedraDirty();
        this.m_LastPosition = this.m_Group.transform.position;
        this.m_LastRotation = this.m_Group.transform.rotation;
        this.m_LastScale = this.m_Group.transform.localScale;
      }
      bool firstSelect = false;
      if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && (this.SelectedCount == 0 && PointEditor.FindNearest(Event.current.mousePosition, transform, (IEditablePoint) this) != -1) && !this.m_Editing)
      {
        this.m_Inspector.StartEditMode();
        this.m_Editing = true;
        firstSelect = true;
      }
      bool flag = Event.current.type == EventType.MouseUp;
      if (this.m_Editing && PointEditor.SelectPoints((IEditablePoint) this, transform, ref this.m_Selection, firstSelect))
        Undo.RegisterCompleteObjectUndo(new UnityEngine.Object[2]
        {
          (UnityEngine.Object) this.m_Group,
          (UnityEngine.Object) this.m_SerializedSelectedProbes
        }, "Select Probes");
      if ((Event.current.type == EventType.ValidateCommand || Event.current.type == EventType.ExecuteCommand) && Event.current.commandName == "Paste")
      {
        if (Event.current.type == EventType.ValidateCommand && LightProbeGroupEditor.CanPasteProbes())
          Event.current.Use();
        if (Event.current.type == EventType.ExecuteCommand && this.PasteProbes())
        {
          Event.current.Use();
          this.m_Editing = true;
        }
      }
      if (this.drawTetrahedra)
        this.DrawTetrahedra();
      PointEditor.Draw((IEditablePoint) this, transform, this.m_Selection, true);
      if (!this.m_Editing)
        return this.m_Editing;
      this.HandleEditMenuHotKeyCommands();
      if (this.m_Editing && PointEditor.MovePoints((IEditablePoint) this, transform, this.m_Selection))
      {
        Undo.RegisterCompleteObjectUndo(new UnityEngine.Object[2]
        {
          (UnityEngine.Object) this.m_Group,
          (UnityEngine.Object) this.m_SerializedSelectedProbes
        }, "Move Probes");
        if (LightProbeVisualization.dynamicUpdateLightProbes)
          this.MarkTetrahedraDirty();
      }
      if (this.m_Editing && flag && !LightProbeVisualization.dynamicUpdateLightProbes)
        this.MarkTetrahedraDirty();
      return this.m_Editing;
    }

    public void MarkTetrahedraDirty()
    {
      this.m_ShouldRecalculateTetrahedra = true;
    }

    public Bounds selectedProbeBounds
    {
      get
      {
        List<Vector3> positions = new List<Vector3>();
        foreach (int index in this.m_Selection)
          positions.Add(this.m_SourcePositions[index]);
        return this.GetBounds(positions);
      }
    }

    public Bounds bounds
    {
      get
      {
        return this.GetBounds(this.m_SourcePositions);
      }
    }

    private Bounds GetBounds(List<Vector3> positions)
    {
      if (positions.Count == 0)
        return new Bounds();
      if (positions.Count == 1)
        return new Bounds(this.m_Group.transform.TransformPoint(positions[0]), new Vector3(1f, 1f, 1f));
      return GeometryUtility.CalculateBounds(positions.ToArray(), this.m_Group.transform.localToWorldMatrix);
    }

    public Vector3 GetPosition(int idx)
    {
      return this.m_SourcePositions[idx];
    }

    public Vector3 GetWorldPosition(int idx)
    {
      return this.m_Group.transform.TransformPoint(this.m_SourcePositions[idx]);
    }

    public void SetPosition(int idx, Vector3 position)
    {
      if (this.m_SourcePositions[idx] == position)
        return;
      this.m_SourcePositions[idx] = position;
    }

    public Color GetDefaultColor()
    {
      return LightProbeGroupEditor.kCloudColor;
    }

    public Color GetSelectedColor()
    {
      return LightProbeGroupEditor.kSelectedCloudColor;
    }

    public float GetPointScale()
    {
      return 10f * AnnotationUtility.iconSize;
    }

    public Vector3[] GetSelectedPositions()
    {
      int selectedCount = this.SelectedCount;
      Vector3[] vector3Array = new Vector3[selectedCount];
      for (int index = 0; index < selectedCount; ++index)
        vector3Array[index] = this.m_SourcePositions[this.m_Selection[index]];
      return vector3Array;
    }

    public void UpdateSelectedPosition(int idx, Vector3 position)
    {
      if (idx > this.SelectedCount - 1)
        return;
      this.m_SourcePositions[this.m_Selection[idx]] = position;
    }

    public IEnumerable<Vector3> GetPositions()
    {
      return (IEnumerable<Vector3>) this.m_SourcePositions;
    }

    public Vector3[] GetUnselectedPositions()
    {
      int count = this.Count;
      int selectedCount = this.SelectedCount;
      if (selectedCount == count)
        return new Vector3[0];
      if (selectedCount == 0)
        return this.m_SourcePositions.ToArray();
      bool[] flagArray = new bool[count];
      for (int index = 0; index < count; ++index)
        flagArray[index] = false;
      for (int index = 0; index < selectedCount; ++index)
        flagArray[this.m_Selection[index]] = true;
      Vector3[] vector3Array = new Vector3[count - selectedCount];
      int num = 0;
      for (int index = 0; index < count; ++index)
      {
        if (!flagArray[index])
          vector3Array[num++] = this.m_SourcePositions[index];
      }
      return vector3Array;
    }

    public int Count
    {
      get
      {
        return this.m_SourcePositions.Count;
      }
    }

    public int SelectedCount
    {
      get
      {
        return this.m_Selection.Count;
      }
    }
  }
}
