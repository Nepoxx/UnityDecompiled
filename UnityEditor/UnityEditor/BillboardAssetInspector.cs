// Decompiled with JetBrains decompiler
// Type: UnityEditor.BillboardAssetInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (BillboardAsset))]
  [CanEditMultipleObjects]
  internal class BillboardAssetInspector : Editor
  {
    private static BillboardAssetInspector.GUIStyles s_Styles = (BillboardAssetInspector.GUIStyles) null;
    private bool m_PreviewShaded = true;
    private Vector2 m_PreviewDir = new Vector2(-120f, 20f);
    private SerializedProperty m_Width;
    private SerializedProperty m_Height;
    private SerializedProperty m_Bottom;
    private SerializedProperty m_Images;
    private SerializedProperty m_Vertices;
    private SerializedProperty m_Indices;
    private SerializedProperty m_Material;
    private PreviewRenderUtility m_PreviewUtility;
    private Mesh m_ShadedMesh;
    private Mesh m_GeometryMesh;
    private MaterialPropertyBlock m_ShadedMaterialProperties;
    private Material m_GeometryMaterial;
    private Material m_WireframeMaterial;

    private static BillboardAssetInspector.GUIStyles Styles
    {
      get
      {
        if (BillboardAssetInspector.s_Styles == null)
          BillboardAssetInspector.s_Styles = new BillboardAssetInspector.GUIStyles();
        return BillboardAssetInspector.s_Styles;
      }
    }

    private void OnEnable()
    {
      this.m_Width = this.serializedObject.FindProperty("width");
      this.m_Height = this.serializedObject.FindProperty("height");
      this.m_Bottom = this.serializedObject.FindProperty("bottom");
      this.m_Images = this.serializedObject.FindProperty("imageTexCoords");
      this.m_Vertices = this.serializedObject.FindProperty("vertices");
      this.m_Indices = this.serializedObject.FindProperty("indices");
      this.m_Material = this.serializedObject.FindProperty("material");
    }

    private void OnDisable()
    {
      if (this.m_PreviewUtility == null)
        return;
      this.m_PreviewUtility.Cleanup();
      this.m_PreviewUtility = (PreviewRenderUtility) null;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ShadedMesh, true);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_GeometryMesh, true);
      this.m_GeometryMaterial = (Material) null;
      if ((UnityEngine.Object) this.m_WireframeMaterial != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_WireframeMaterial, true);
    }

    private void InitPreview()
    {
      if (this.m_PreviewUtility != null)
        return;
      this.m_PreviewUtility = new PreviewRenderUtility();
      this.m_ShadedMesh = new Mesh();
      this.m_ShadedMesh.hideFlags = HideFlags.HideAndDontSave;
      this.m_ShadedMesh.MarkDynamic();
      this.m_GeometryMesh = new Mesh();
      this.m_GeometryMesh.hideFlags = HideFlags.HideAndDontSave;
      this.m_GeometryMesh.MarkDynamic();
      this.m_ShadedMaterialProperties = new MaterialPropertyBlock();
      this.m_GeometryMaterial = EditorGUIUtility.GetBuiltinExtraResource(typeof (Material), "Default-Material.mat") as Material;
      this.m_WireframeMaterial = ModelInspector.CreateWireframeMaterial();
      EditorUtility.SetCameraAnimateMaterials(this.m_PreviewUtility.camera, true);
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Width);
      EditorGUILayout.PropertyField(this.m_Height);
      EditorGUILayout.PropertyField(this.m_Bottom);
      EditorGUILayout.PropertyField(this.m_Material);
      this.serializedObject.ApplyModifiedProperties();
    }

    public override bool HasPreviewGUI()
    {
      return this.target != (UnityEngine.Object) null;
    }

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return (Texture2D) null;
      this.InitPreview();
      this.m_PreviewUtility.BeginStaticPreview(new Rect(0.0f, 0.0f, (float) width, (float) height));
      this.DoRenderPreview(true);
      return this.m_PreviewUtility.EndStaticPreview();
    }

    public override void OnPreviewSettings()
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return;
      bool flag = this.m_Material.objectReferenceValue != (UnityEngine.Object) null;
      GUI.enabled = flag;
      if (!flag)
        this.m_PreviewShaded = false;
      GUIContent content = !this.m_PreviewShaded ? BillboardAssetInspector.Styles.m_Geometry : BillboardAssetInspector.Styles.m_Shaded;
      Rect rect = GUILayoutUtility.GetRect(content, BillboardAssetInspector.Styles.m_DropdownButton, new GUILayoutOption[1]{ GUILayout.Width(75f) });
      if (!EditorGUI.DropdownButton(rect, content, FocusType.Passive, BillboardAssetInspector.Styles.m_DropdownButton))
        return;
      GUIUtility.hotControl = 0;
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(BillboardAssetInspector.Styles.m_Shaded, this.m_PreviewShaded, (GenericMenu.MenuFunction) (() => this.m_PreviewShaded = true));
      genericMenu.AddItem(BillboardAssetInspector.Styles.m_Geometry, !this.m_PreviewShaded, (GenericMenu.MenuFunction) (() => this.m_PreviewShaded = false));
      genericMenu.DropDown(rect);
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y, r.width, 40f), "Preview requires\nrender texture support");
      }
      else
      {
        this.InitPreview();
        this.m_PreviewDir = PreviewGUI.Drag2D(this.m_PreviewDir, r);
        if (Event.current.type != EventType.Repaint)
          return;
        this.m_PreviewUtility.BeginPreview(r, background);
        this.DoRenderPreview(this.m_PreviewShaded);
        this.m_PreviewUtility.EndAndDrawPreview(r);
      }
    }

    public override string GetInfoString()
    {
      return string.Format("{0} verts, {1} tris, {2} images", (object) this.m_Vertices.arraySize, (object) (this.m_Indices.arraySize / 3), (object) this.m_Images.arraySize);
    }

    private void MakeRenderMesh(Mesh mesh, BillboardAsset billboard)
    {
      mesh.SetVertices(Enumerable.Repeat<Vector3>(Vector3.zero, billboard.vertexCount).ToList<Vector3>());
      mesh.SetColors(Enumerable.Repeat<Color>(Color.black, billboard.vertexCount).ToList<Color>());
      mesh.SetUVs(0, ((IEnumerable<Vector2>) billboard.GetVertices()).ToList<Vector2>());
      mesh.SetUVs(1, Enumerable.Repeat<Vector4>(new Vector4(1f, 1f, 0.0f, 0.0f), billboard.vertexCount).ToList<Vector4>());
      mesh.SetTriangles(((IEnumerable<ushort>) billboard.GetIndices()).Select<ushort, int>((Func<ushort, int>) (v => (int) v)).ToList<int>(), 0);
    }

    private void MakePreviewMesh(Mesh mesh, BillboardAsset billboard)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      mesh.SetVertices(Enumerable.Repeat<IEnumerable<Vector3>>(((IEnumerable<Vector2>) billboard.GetVertices()).Select<Vector2, Vector3>(new Func<Vector2, Vector3>(new BillboardAssetInspector.\u003CMakePreviewMesh\u003Ec__AnonStorey0()
      {
        width = billboard.width,
        height = billboard.height,
        bottom = billboard.bottom
      }.\u003C\u003Em__0)), 2).SelectMany<IEnumerable<Vector3>, Vector3>((Func<IEnumerable<Vector3>, IEnumerable<Vector3>>) (s => s)).ToList<Vector3>());
      mesh.SetNormals(Enumerable.Repeat<Vector3>(Vector3.forward, billboard.vertexCount).Concat<Vector3>(Enumerable.Repeat<Vector3>(-Vector3.forward, billboard.vertexCount)).ToList<Vector3>());
      int[] triangles = new int[billboard.indexCount * 2];
      ushort[] indices = billboard.GetIndices();
      for (int index = 0; index < billboard.indexCount / 3; ++index)
      {
        triangles[index * 3] = (int) indices[index * 3];
        triangles[index * 3 + 1] = (int) indices[index * 3 + 1];
        triangles[index * 3 + 2] = (int) indices[index * 3 + 2];
        triangles[index * 3 + billboard.indexCount] = (int) indices[index * 3 + 2];
        triangles[index * 3 + 1 + billboard.indexCount] = (int) indices[index * 3 + 1];
        triangles[index * 3 + 2 + billboard.indexCount] = (int) indices[index * 3];
      }
      mesh.SetTriangles(triangles, 0);
    }

    private void DoRenderPreview(bool shaded)
    {
      BillboardAsset target = this.target as BillboardAsset;
      Bounds bounds = new Bounds(new Vector3(0.0f, (float) (((double) this.m_Height.floatValue + (double) this.m_Bottom.floatValue) * 0.5), 0.0f), new Vector3(this.m_Width.floatValue, this.m_Height.floatValue, this.m_Width.floatValue));
      float magnitude = bounds.extents.magnitude;
      float num = 8f * magnitude;
      Quaternion quaternion = Quaternion.Euler(-this.m_PreviewDir.y, -this.m_PreviewDir.x, 0.0f);
      this.m_PreviewUtility.camera.transform.rotation = quaternion;
      this.m_PreviewUtility.camera.transform.position = quaternion * (-Vector3.forward * num);
      this.m_PreviewUtility.camera.nearClipPlane = num - magnitude * 1.1f;
      this.m_PreviewUtility.camera.farClipPlane = num + magnitude * 1.1f;
      this.m_PreviewUtility.lights[0].intensity = 1.4f;
      this.m_PreviewUtility.lights[0].transform.rotation = quaternion * Quaternion.Euler(40f, 40f, 0.0f);
      this.m_PreviewUtility.lights[1].intensity = 1.4f;
      this.m_PreviewUtility.ambientColor = new Color(0.1f, 0.1f, 0.1f, 0.0f);
      if (shaded)
      {
        this.MakeRenderMesh(this.m_ShadedMesh, target);
        target.MakeMaterialProperties(this.m_ShadedMaterialProperties, this.m_PreviewUtility.camera);
        ModelInspector.RenderMeshPreviewSkipCameraAndLighting(this.m_ShadedMesh, bounds, this.m_PreviewUtility, target.material, (Material) null, this.m_ShadedMaterialProperties, new Vector2(0.0f, 0.0f), -1);
      }
      else
      {
        this.MakePreviewMesh(this.m_GeometryMesh, target);
        ModelInspector.RenderMeshPreviewSkipCameraAndLighting(this.m_GeometryMesh, bounds, this.m_PreviewUtility, this.m_GeometryMaterial, this.m_WireframeMaterial, (MaterialPropertyBlock) null, new Vector2(0.0f, 0.0f), -1);
      }
    }

    private class GUIStyles
    {
      public readonly GUIContent m_Shaded = new GUIContent("Shaded");
      public readonly GUIContent m_Geometry = new GUIContent("Geometry");
      public readonly GUIStyle m_DropdownButton = new GUIStyle((GUIStyle) "MiniPopup");
    }
  }
}
