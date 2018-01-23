// Decompiled with JetBrains decompiler
// Type: UnityEditor.PreviewRenderUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  public class PreviewRenderUtility
  {
    private readonly PreviewScene m_PreviewScene;
    private RenderTexture m_RenderTexture;
    private Rect m_TargetRect;
    private SavedRenderTargetState m_SavedState;
    private bool m_PixelPerfect;
    private Material m_InvisibleMaterial;
    [Obsolete("Use the property camera instead (UnityUpgradable) -> camera", false)]
    public Camera m_Camera;
    [Obsolete("Use the property cameraFieldOfView (UnityUpgradable) -> cameraFieldOfView", false)]
    public float m_CameraFieldOfView;
    [Obsolete("Use the property lights (UnityUpgradable) -> lights", false)]
    public Light[] m_Light;

    public PreviewRenderUtility(bool renderFullScene)
      : this()
    {
    }

    public PreviewRenderUtility(bool renderFullScene, bool pixelPerfect)
      : this()
    {
      this.m_PixelPerfect = pixelPerfect;
    }

    public PreviewRenderUtility()
    {
      this.m_PreviewScene = new PreviewScene("Preview Scene");
      GameObject light1 = PreviewRenderUtility.CreateLight();
      this.previewScene.AddGameObject(light1);
      this.Light0 = light1.GetComponent<Light>();
      GameObject light2 = PreviewRenderUtility.CreateLight();
      this.previewScene.AddGameObject(light2);
      this.Light1 = light2.GetComponent<Light>();
      this.Light0.color = SceneView.kSceneViewFrontLight;
      this.Light1.transform.rotation = Quaternion.Euler(340f, 218f, 177f);
      this.Light1.color = new Color(0.4f, 0.4f, 0.45f, 0.0f) * 0.7f;
      this.m_PixelPerfect = false;
    }

    internal static void SetEnabledRecursive(GameObject go, bool enabled)
    {
      foreach (Renderer componentsInChild in go.GetComponentsInChildren<Renderer>())
        componentsInChild.enabled = enabled;
    }

    public Camera camera
    {
      get
      {
        return this.previewScene.camera;
      }
    }

    public float cameraFieldOfView
    {
      get
      {
        return this.camera.fieldOfView;
      }
      set
      {
        this.camera.fieldOfView = value;
      }
    }

    public Color ambientColor { get; set; }

    public Light[] lights
    {
      get
      {
        return new Light[2]{ this.Light0, this.Light1 };
      }
    }

    private Light Light0 { get; set; }

    private Light Light1 { get; set; }

    internal RenderTexture renderTexture
    {
      get
      {
        return this.m_RenderTexture;
      }
    }

    internal PreviewScene previewScene
    {
      get
      {
        return this.m_PreviewScene;
      }
    }

    public void Cleanup()
    {
      if ((bool) ((UnityEngine.Object) this.m_RenderTexture))
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_RenderTexture);
        this.m_RenderTexture = (RenderTexture) null;
      }
      if ((UnityEngine.Object) this.m_InvisibleMaterial != (UnityEngine.Object) null)
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_InvisibleMaterial);
        this.m_InvisibleMaterial = (Material) null;
      }
      this.previewScene.Dispose();
    }

    public void BeginPreview(Rect r, GUIStyle previewBackground)
    {
      this.InitPreview(r);
      if (previewBackground == null || previewBackground == GUIStyle.none)
        return;
      Graphics.DrawTexture(previewBackground.overflow.Add(new Rect(0.0f, 0.0f, (float) this.m_RenderTexture.width, (float) this.m_RenderTexture.height)), (Texture) previewBackground.normal.background, new Rect(0.0f, 0.0f, 1f, 1f), previewBackground.border.left, previewBackground.border.right, previewBackground.border.top, previewBackground.border.bottom, new Color(0.5f, 0.5f, 0.5f, 1f), (Material) null);
      if (!Unsupported.SetOverrideRenderSettings(this.previewScene.scene))
        return;
      RenderSettings.ambientMode = AmbientMode.Flat;
      RenderSettings.ambientLight = this.ambientColor;
    }

    public void BeginStaticPreview(Rect r)
    {
      this.InitPreview(r);
      Color color = new Color(0.3215686f, 0.3215686f, 0.3215686f, 1f);
      Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, true, true);
      texture2D.SetPixel(0, 0, color);
      texture2D.Apply();
      Graphics.DrawTexture(new Rect(0.0f, 0.0f, (float) this.m_RenderTexture.width, (float) this.m_RenderTexture.height), (Texture) texture2D);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) texture2D);
    }

    private void InitPreview(Rect r)
    {
      this.m_TargetRect = r;
      float scaleFactor = this.GetScaleFactor(r.width, r.height);
      int width = (int) ((double) r.width * (double) scaleFactor);
      int height = (int) ((double) r.height * (double) scaleFactor);
      if (!(bool) ((UnityEngine.Object) this.m_RenderTexture) || this.m_RenderTexture.width != width || this.m_RenderTexture.height != height)
      {
        if ((bool) ((UnityEngine.Object) this.m_RenderTexture))
        {
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_RenderTexture);
          this.m_RenderTexture = (RenderTexture) null;
        }
        RenderTextureFormat format = !this.camera.allowHDR ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGBHalf;
        this.m_RenderTexture = new RenderTexture(width, height, 16, format, RenderTextureReadWrite.Default);
        this.m_RenderTexture.hideFlags = HideFlags.HideAndDontSave;
        this.camera.targetTexture = this.m_RenderTexture;
        foreach (Behaviour light in this.lights)
          light.enabled = true;
      }
      this.m_SavedState = new SavedRenderTargetState();
      EditorGUIUtility.SetRenderTextureNoViewport(this.m_RenderTexture);
      GL.LoadOrtho();
      GL.LoadPixelMatrix(0.0f, (float) this.m_RenderTexture.width, (float) this.m_RenderTexture.height, 0.0f);
      ShaderUtil.rawViewportRect = new Rect(0.0f, 0.0f, (float) this.m_RenderTexture.width, (float) this.m_RenderTexture.height);
      ShaderUtil.rawScissorRect = new Rect(0.0f, 0.0f, (float) this.m_RenderTexture.width, (float) this.m_RenderTexture.height);
      GL.Clear(true, true, this.camera.backgroundColor);
      foreach (Behaviour light in this.lights)
        light.enabled = true;
      SphericalHarmonicsL2 ambientProbe = RenderSettings.ambientProbe;
      Unsupported.SetOverrideRenderSettings(this.previewScene.scene);
      RenderSettings.ambientProbe = ambientProbe;
    }

    public float GetScaleFactor(float width, float height)
    {
      float f = Mathf.Min(Mathf.Max(Mathf.Min(width * 2f, 1024f), width) / width, Mathf.Max(Mathf.Min(height * 2f, 1024f), height) / height) * EditorGUIUtility.pixelsPerPoint;
      if (this.m_PixelPerfect)
        f = Mathf.Max(Mathf.Round(f), 1f);
      return f;
    }

    [Obsolete("This method has been marked obsolete, use BeginStaticPreview() instead (UnityUpgradable) -> BeginStaticPreview(*)", false)]
    public void BeginStaticPreviewHDR(Rect r)
    {
      this.BeginStaticPreview(r);
    }

    [Obsolete("This method has been marked obsolete, use BeginPreview() instead (UnityUpgradable) -> BeginPreview(*)", false)]
    public void BeginPreviewHDR(Rect r, GUIStyle previewBackground)
    {
      this.BeginPreview(r, previewBackground);
    }

    public Texture EndPreview()
    {
      Unsupported.RestoreOverrideRenderSettings();
      this.m_SavedState.Restore();
      this.FinishFrame();
      return (Texture) this.m_RenderTexture;
    }

    private void FinishFrame()
    {
      Unsupported.RestoreOverrideRenderSettings();
      foreach (Behaviour light in this.lights)
        light.enabled = false;
    }

    public void EndAndDrawPreview(Rect r)
    {
      Texture image = this.EndPreview();
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      GUI.DrawTexture(r, image, ScaleMode.StretchToFill, false);
      GL.sRGBWrite = false;
    }

    public Texture2D EndStaticPreview()
    {
      Unsupported.RestoreOverrideRenderSettings();
      RenderTexture temporary = RenderTexture.GetTemporary((int) this.m_TargetRect.width, (int) this.m_TargetRect.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      Graphics.Blit((Texture) this.m_RenderTexture, temporary);
      GL.sRGBWrite = false;
      RenderTexture.active = temporary;
      Texture2D texture2D = new Texture2D((int) this.m_TargetRect.width, (int) this.m_TargetRect.height, TextureFormat.RGB24, false, true);
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, this.m_TargetRect.width, this.m_TargetRect.height), 0, 0);
      texture2D.Apply();
      RenderTexture.ReleaseTemporary(temporary);
      this.m_SavedState.Restore();
      this.FinishFrame();
      return texture2D;
    }

    [Obsolete("AddSingleGO(GameObject go, bool instantiateAtZero) has been deprecated, use AddSingleGo(GameObject go) instead. instantiateAtZero has no effect and is not supported.")]
    public void AddSingleGO(GameObject go, bool instantiateAtZero)
    {
      this.AddSingleGO(go);
    }

    public void AddSingleGO(GameObject go)
    {
      this.previewScene.AddGameObject(go);
    }

    public GameObject InstantiatePrefabInScene(GameObject prefab)
    {
      return (GameObject) PrefabUtility.InstantiatePrefab((UnityEngine.Object) prefab, this.m_PreviewScene.scene);
    }

    private Material GetInvisibleMaterial()
    {
      if ((UnityEngine.Object) this.m_InvisibleMaterial == (UnityEngine.Object) null)
      {
        this.m_InvisibleMaterial = new Material(Shader.FindBuiltin("Internal-Colored.shader"));
        this.m_InvisibleMaterial.hideFlags = HideFlags.HideAndDontSave;
        this.m_InvisibleMaterial.SetColor("_Color", Color.clear);
        this.m_InvisibleMaterial.SetInt("_ZWrite", 0);
      }
      return this.m_InvisibleMaterial;
    }

    internal void AddManagedGO(GameObject go)
    {
      this.m_PreviewScene.AddManagedGO(go);
    }

    public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material mat, int subMeshIndex)
    {
      this.DrawMesh(mesh, matrix, mat, subMeshIndex, (MaterialPropertyBlock) null, (Transform) null, false);
    }

    public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material mat, int subMeshIndex, MaterialPropertyBlock customProperties)
    {
      this.DrawMesh(mesh, matrix, mat, subMeshIndex, customProperties, (Transform) null, false);
    }

    public void DrawMesh(Mesh mesh, Matrix4x4 m, Material mat, int subMeshIndex, MaterialPropertyBlock customProperties, Transform probeAnchor, bool useLightProbe)
    {
      Quaternion rot = Quaternion.LookRotation((Vector3) m.GetColumn(2), (Vector3) m.GetColumn(1));
      Vector4 column = m.GetColumn(3);
      Vector3 scale = new Vector3(m.GetColumn(0).magnitude, m.GetColumn(1).magnitude, m.GetColumn(2).magnitude);
      this.DrawMesh(mesh, (Vector3) column, scale, rot, mat, subMeshIndex, customProperties, probeAnchor, useLightProbe);
    }

    public void DrawMesh(Mesh mesh, Vector3 pos, Quaternion rot, Material mat, int subMeshIndex)
    {
      this.DrawMesh(mesh, pos, rot, mat, subMeshIndex, (MaterialPropertyBlock) null, (Transform) null, false);
    }

    public void DrawMesh(Mesh mesh, Vector3 pos, Quaternion rot, Material mat, int subMeshIndex, MaterialPropertyBlock customProperties)
    {
      this.DrawMesh(mesh, pos, rot, mat, subMeshIndex, customProperties, (Transform) null, false);
    }

    public void DrawMesh(Mesh mesh, Vector3 pos, Quaternion rot, Material mat, int subMeshIndex, MaterialPropertyBlock customProperties, Transform probeAnchor)
    {
      this.DrawMesh(mesh, pos, rot, mat, subMeshIndex, customProperties, probeAnchor, false);
    }

    public void DrawMesh(Mesh mesh, Vector3 pos, Quaternion rot, Material mat, int subMeshIndex, MaterialPropertyBlock customProperties, Transform probeAnchor, bool useLightProbe)
    {
      this.DrawMesh(mesh, pos, Vector3.one, rot, mat, subMeshIndex, customProperties, probeAnchor, useLightProbe);
    }

    public void DrawMesh(Mesh mesh, Vector3 pos, Vector3 scale, Quaternion rot, Material mat, int subMeshIndex, MaterialPropertyBlock customProperties, Transform probeAnchor, bool useLightProbe)
    {
      Graphics.DrawMesh(mesh, Matrix4x4.TRS(pos, rot, scale), mat, 1, this.camera, subMeshIndex, customProperties, ShadowCastingMode.Off, false, probeAnchor, useLightProbe);
    }

    internal static Mesh GetPreviewSphere()
    {
      GameObject gameObject = (GameObject) EditorGUIUtility.LoadRequired("Previews/PreviewMaterials.fbx");
      gameObject.SetActive(false);
      IEnumerator enumerator = gameObject.transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          if (current.name == "sphere")
            return current.GetComponent<MeshFilter>().sharedMesh;
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      return (Mesh) null;
    }

    protected static GameObject CreateLight()
    {
      GameObject objectWithHideFlags = EditorUtility.CreateGameObjectWithHideFlags("PreRenderLight", HideFlags.HideAndDontSave, typeof (Light));
      Light component = objectWithHideFlags.GetComponent<Light>();
      component.type = LightType.Directional;
      component.intensity = 1f;
      component.enabled = false;
      return objectWithHideFlags;
    }

    public void Render(bool allowScriptableRenderPipeline = false, bool updatefov = true)
    {
      foreach (Behaviour light in this.lights)
        light.enabled = true;
      bool scriptableRenderPipeline = Unsupported.useScriptableRenderPipeline;
      Unsupported.useScriptableRenderPipeline = allowScriptableRenderPipeline;
      float fieldOfView = this.camera.fieldOfView;
      if (updatefov)
        this.camera.fieldOfView = (float) ((double) Mathf.Atan((this.m_RenderTexture.width > 0 ? Mathf.Max(1f, (float) this.m_RenderTexture.height / (float) this.m_RenderTexture.width) : 1f) * Mathf.Tan((float) ((double) this.camera.fieldOfView * 0.5 * (Math.PI / 180.0)))) * 57.2957801818848 * 2.0);
      this.camera.Render();
      this.camera.fieldOfView = fieldOfView;
      Unsupported.useScriptableRenderPipeline = scriptableRenderPipeline;
    }
  }
}
