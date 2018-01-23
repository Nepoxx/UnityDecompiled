// Decompiled with JetBrains decompiler
// Type: UnityEditor.PreviewScene
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  internal class PreviewScene : IDisposable
  {
    private readonly List<GameObject> m_GameObjects = new List<GameObject>();
    private readonly Scene m_Scene;
    private readonly Camera m_Camera;

    public PreviewScene(string sceneName)
    {
      this.m_Scene = EditorSceneManager.NewPreviewScene();
      this.m_Scene.name = sceneName;
      GameObject objectWithHideFlags = EditorUtility.CreateGameObjectWithHideFlags("Preview Scene Camera", HideFlags.HideAndDontSave, typeof (Camera));
      this.AddGameObject(objectWithHideFlags);
      this.m_Camera = objectWithHideFlags.GetComponent<Camera>();
      this.camera.cameraType = CameraType.Preview;
      this.camera.enabled = false;
      this.camera.clearFlags = CameraClearFlags.Depth;
      this.camera.fieldOfView = 15f;
      this.camera.farClipPlane = 10f;
      this.camera.nearClipPlane = 2f;
      this.camera.backgroundColor = new Color(0.1921569f, 0.1921569f, 0.1921569f, 1f);
      this.camera.renderingPath = RenderingPath.Forward;
      this.camera.useOcclusionCulling = false;
      this.camera.scene = this.m_Scene;
    }

    public Camera camera
    {
      get
      {
        return this.m_Camera;
      }
    }

    public Scene scene
    {
      get
      {
        return this.m_Scene;
      }
    }

    public void AddGameObject(GameObject go)
    {
      if (this.m_GameObjects.Contains(go))
        return;
      SceneManager.MoveGameObjectToScene(go, this.m_Scene);
      this.m_GameObjects.Add(go);
    }

    public void AddManagedGO(GameObject go)
    {
      SceneManager.MoveGameObjectToScene(go, this.m_Scene);
    }

    public void Dispose()
    {
      EditorSceneManager.ClosePreviewScene(this.m_Scene);
      foreach (UnityEngine.Object gameObject in this.m_GameObjects)
        UnityEngine.Object.DestroyImmediate(gameObject);
      this.m_GameObjects.Clear();
    }
  }
}
