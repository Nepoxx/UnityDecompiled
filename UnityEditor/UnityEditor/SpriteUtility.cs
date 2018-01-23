// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.U2D;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor
{
  internal static class SpriteUtility
  {
    private static Material s_PreviewSpriteDefaultMaterial;
    private static List<UnityEngine.Object> s_SceneDragObjects;
    private static SpriteUtility.DragType s_DragType;

    internal static Material previewSpriteDefaultMaterial
    {
      get
      {
        if ((UnityEngine.Object) SpriteUtility.s_PreviewSpriteDefaultMaterial == (UnityEngine.Object) null)
          SpriteUtility.s_PreviewSpriteDefaultMaterial = new Material(Shader.Find("Sprites/Default"));
        return SpriteUtility.s_PreviewSpriteDefaultMaterial;
      }
    }

    public static void OnSceneDrag(SceneView sceneView)
    {
      SceneView sceneView1 = sceneView;
      UnityEngine.U2D.Interface.Event @event = new UnityEngine.U2D.Interface.Event();
      UnityEngine.Object[] objectReferences = DragAndDrop.objectReferences;
      string[] paths = DragAndDrop.paths;
      // ISSUE: reference to a compiler-generated field
      if (SpriteUtility.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SpriteUtility.\u003C\u003Ef__mg\u0024cache0 = new SpriteUtility.ShowFileDialogDelegate(EditorUtility.SaveFilePanelInProject);
      }
      // ISSUE: reference to a compiler-generated field
      SpriteUtility.ShowFileDialogDelegate fMgCache0 = SpriteUtility.\u003C\u003Ef__mg\u0024cache0;
      SpriteUtility.HandleSpriteSceneDrag(sceneView1, (IEvent) @event, objectReferences, paths, fMgCache0);
    }

    public static void HandleSpriteSceneDrag(SceneView sceneView, IEvent evt, UnityEngine.Object[] objectReferences, string[] paths, SpriteUtility.ShowFileDialogDelegate saveFileDialog)
    {
      if (evt.type != EventType.DragUpdated && evt.type != EventType.DragPerform && evt.type != EventType.DragExited || ((IEnumerable<UnityEngine.Object>) objectReferences).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (obj => obj == (UnityEngine.Object) null)))
        return;
      if (objectReferences.Length == 1 && (UnityEngine.Object) (objectReferences[0] as UnityEngine.Texture2D) != (UnityEngine.Object) null)
      {
        GameObject gameObject = HandleUtility.PickGameObject(evt.mousePosition, true);
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          Renderer component = gameObject.GetComponent<Renderer>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && !(component is SpriteRenderer))
          {
            SpriteUtility.CleanUp(true);
            return;
          }
        }
      }
      switch (evt.type)
      {
        case EventType.DragUpdated:
          SpriteUtility.DragType dragType = !evt.alt ? SpriteUtility.DragType.SpriteAnimation : SpriteUtility.DragType.CreateMultiple;
          if (SpriteUtility.s_DragType != dragType || SpriteUtility.s_SceneDragObjects == null)
          {
            if (!SpriteUtility.ExistingAssets(objectReferences) && SpriteUtility.PathsAreValidTextures(paths))
            {
              DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
              SpriteUtility.s_SceneDragObjects = new List<UnityEngine.Object>();
              SpriteUtility.s_DragType = dragType;
            }
            else
            {
              List<Sprite> fromPathsOrObjects = SpriteUtility.GetSpriteFromPathsOrObjects(objectReferences, paths, evt.type);
              if (fromPathsOrObjects.Count == 0)
                break;
              if (SpriteUtility.s_DragType != SpriteUtility.DragType.NotInitialized)
                SpriteUtility.CleanUp(true);
              SpriteUtility.s_DragType = dragType;
              SpriteUtility.CreateSceneDragObjects(fromPathsOrObjects);
              SpriteUtility.IgnoreForRaycasts(SpriteUtility.s_SceneDragObjects);
            }
          }
          SpriteUtility.PositionSceneDragObjects(SpriteUtility.s_SceneDragObjects, sceneView, evt.mousePosition);
          DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
          evt.Use();
          break;
        case EventType.DragPerform:
          List<Sprite> fromPathsOrObjects1 = SpriteUtility.GetSpriteFromPathsOrObjects(objectReferences, paths, evt.type);
          if (fromPathsOrObjects1.Count <= 0 || SpriteUtility.s_SceneDragObjects == null)
            break;
          int currentGroup = Undo.GetCurrentGroup();
          if (SpriteUtility.s_SceneDragObjects.Count == 0)
          {
            SpriteUtility.CreateSceneDragObjects(fromPathsOrObjects1);
            SpriteUtility.PositionSceneDragObjects(SpriteUtility.s_SceneDragObjects, sceneView, evt.mousePosition);
          }
          foreach (GameObject sceneDragObject in SpriteUtility.s_SceneDragObjects)
          {
            sceneDragObject.hideFlags = HideFlags.None;
            Undo.RegisterCreatedObjectUndo((UnityEngine.Object) sceneDragObject, "Create Sprite");
            EditorUtility.SetDirty((UnityEngine.Object) sceneDragObject);
          }
          bool flag = true;
          if (SpriteUtility.s_DragType == SpriteUtility.DragType.SpriteAnimation && fromPathsOrObjects1.Count > 1)
          {
            UsabilityAnalytics.Event("Sprite Drag and Drop", "Drop multiple sprites to scene", "null", 1);
            flag = SpriteUtility.AddAnimationToGO((GameObject) SpriteUtility.s_SceneDragObjects[0], fromPathsOrObjects1.ToArray(), saveFileDialog);
          }
          else
            UsabilityAnalytics.Event("Sprite Drag and Drop", "Drop single sprite to scene", "null", 1);
          if (flag)
            Selection.objects = SpriteUtility.s_SceneDragObjects.ToArray();
          else
            Undo.RevertAllDownToGroup(currentGroup);
          SpriteUtility.CleanUp(!flag);
          evt.Use();
          break;
        case EventType.DragExited:
          if (SpriteUtility.s_SceneDragObjects == null)
            break;
          SpriteUtility.CleanUp(true);
          evt.Use();
          break;
      }
    }

    private static void IgnoreForRaycasts(List<UnityEngine.Object> objects)
    {
      List<Transform> transformList = new List<Transform>();
      foreach (GameObject gameObject in objects)
        transformList.AddRange((IEnumerable<Transform>) gameObject.GetComponentsInChildren<Transform>());
      HandleUtility.ignoreRaySnapObjects = transformList.ToArray();
    }

    private static void PositionSceneDragObjects(List<UnityEngine.Object> objects, SceneView sceneView, Vector2 mousePosition)
    {
      Vector3 zero = Vector3.zero;
      Vector3 worldPosition = HandleUtility.GUIPointToWorldRay(mousePosition).GetPoint(10f);
      if (sceneView.in2DMode)
      {
        worldPosition.z = 0.0f;
      }
      else
      {
        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        object obj = HandleUtility.RaySnap(HandleUtility.GUIPointToWorldRay(mousePosition));
        if (obj != null)
          worldPosition = ((RaycastHit) obj).point;
      }
      if ((UnityEngine.Object) Selection.activeGameObject != (UnityEngine.Object) null)
      {
        Grid componentInParent = Selection.activeGameObject.GetComponentInParent<Grid>();
        if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null)
        {
          Vector3Int cell = componentInParent.WorldToCell(worldPosition);
          worldPosition = componentInParent.GetCellCenterWorld(cell);
        }
      }
      foreach (GameObject gameObject in objects)
        gameObject.transform.position = worldPosition;
    }

    private static void CreateSceneDragObjects(List<Sprite> sprites)
    {
      if (SpriteUtility.s_SceneDragObjects == null)
        SpriteUtility.s_SceneDragObjects = new List<UnityEngine.Object>();
      if (SpriteUtility.s_DragType == SpriteUtility.DragType.CreateMultiple)
      {
        foreach (Sprite sprite in sprites)
          SpriteUtility.s_SceneDragObjects.Add((UnityEngine.Object) SpriteUtility.CreateDragGO(sprite, Vector3.zero));
      }
      else
        SpriteUtility.s_SceneDragObjects.Add((UnityEngine.Object) SpriteUtility.CreateDragGO(sprites[0], Vector3.zero));
    }

    private static void CleanUp(bool deleteTempSceneObject)
    {
      if (SpriteUtility.s_SceneDragObjects != null)
      {
        if (deleteTempSceneObject)
        {
          foreach (UnityEngine.Object sceneDragObject in SpriteUtility.s_SceneDragObjects)
            UnityEngine.Object.DestroyImmediate(sceneDragObject, false);
        }
        SpriteUtility.s_SceneDragObjects.Clear();
        SpriteUtility.s_SceneDragObjects = (List<UnityEngine.Object>) null;
      }
      HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
      SpriteUtility.s_DragType = SpriteUtility.DragType.NotInitialized;
    }

    private static bool CreateAnimation(GameObject gameObject, UnityEngine.Object[] frames, SpriteUtility.ShowFileDialogDelegate saveFileDialog)
    {
      SpriteUtility.ShowFileDialogDelegate fileDialogDelegate = saveFileDialog;
      if (fileDialogDelegate == null)
      {
        // ISSUE: reference to a compiler-generated field
        if (SpriteUtility.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SpriteUtility.\u003C\u003Ef__mg\u0024cache1 = new SpriteUtility.ShowFileDialogDelegate(EditorUtility.SaveFilePanelInProject);
        }
        // ISSUE: reference to a compiler-generated field
        fileDialogDelegate = SpriteUtility.\u003C\u003Ef__mg\u0024cache1;
      }
      saveFileDialog = fileDialogDelegate;
      Array.Sort<UnityEngine.Object>(frames, (Comparison<UnityEngine.Object>) ((a, b) => EditorUtility.NaturalCompare(a.name, b.name)));
      Animator animator = !(bool) ((UnityEngine.Object) AnimationWindowUtility.EnsureActiveAnimationPlayer(gameObject)) ? (Animator) null : AnimationWindowUtility.GetClosestAnimatorInParents(gameObject.transform);
      bool flag = (UnityEngine.Object) animator != (UnityEngine.Object) null;
      if ((UnityEngine.Object) animator != (UnityEngine.Object) null)
      {
        string message = string.Format(SpriteUtility.SpriteUtilityStrings.saveAnimDialogMessage.text, (object) gameObject.name);
        string activeFolderPath = ProjectWindowUtil.GetActiveFolderPath();
        string clipPath = saveFileDialog(SpriteUtility.SpriteUtilityStrings.saveAnimDialogTitle.text, SpriteUtility.SpriteUtilityStrings.saveAnimDialogName.text, "anim", message, activeFolderPath);
        if (string.IsNullOrEmpty(clipPath))
        {
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) animator);
          return false;
        }
        AnimationClip newClipAtPath = AnimationWindowUtility.CreateNewClipAtPath(clipPath);
        if ((UnityEngine.Object) newClipAtPath != (UnityEngine.Object) null)
        {
          SpriteUtility.AddSpriteAnimationToClip(newClipAtPath, frames);
          flag = AnimationWindowUtility.AddClipToAnimatorComponent(animator, newClipAtPath);
        }
      }
      if (!flag)
        Debug.LogError((object) SpriteUtility.SpriteUtilityStrings.failedToCreateAnimationError.text);
      return flag;
    }

    private static void AddSpriteAnimationToClip(AnimationClip newClip, UnityEngine.Object[] frames)
    {
      newClip.frameRate = 12f;
      ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[frames.Length];
      for (int index = 0; index < keyframes.Length; ++index)
      {
        keyframes[index] = new ObjectReferenceKeyframe();
        keyframes[index].value = (UnityEngine.Object) SpriteUtility.RemapObjectToSprite(frames[index]);
        keyframes[index].time = (float) index / newClip.frameRate;
      }
      EditorCurveBinding binding = EditorCurveBinding.PPtrCurve("", typeof (SpriteRenderer), "m_Sprite");
      AnimationUtility.SetObjectReferenceCurve(newClip, binding, keyframes);
    }

    public static List<Sprite> GetSpriteFromPathsOrObjects(UnityEngine.Object[] objects, string[] paths, EventType currentEventType)
    {
      List<Sprite> result = new List<Sprite>();
      foreach (UnityEngine.Object @object in objects)
      {
        if (AssetDatabase.Contains(@object))
        {
          if (@object is Sprite)
            result.Add(@object as Sprite);
          else if (@object is UnityEngine.Texture2D)
            result.AddRange((IEnumerable<Sprite>) SpriteUtility.TextureToSprites(@object as UnityEngine.Texture2D));
        }
      }
      if (!SpriteUtility.ExistingAssets(objects) && currentEventType == EventType.DragPerform && EditorSettings.defaultBehaviorMode == EditorBehaviorMode.Mode2D)
        SpriteUtility.HandleExternalDrag(paths, true, ref result);
      return result;
    }

    public static bool ExistingAssets(UnityEngine.Object[] objects)
    {
      foreach (UnityEngine.Object @object in objects)
      {
        if (AssetDatabase.Contains(@object))
          return true;
      }
      return false;
    }

    private static void HandleExternalDrag(string[] paths, bool perform, ref List<Sprite> result)
    {
      foreach (string path in paths)
      {
        if (SpriteUtility.ValidPathForTextureAsset(path))
        {
          DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
          if (perform)
          {
            string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine("Assets", FileUtil.GetLastPathNameComponent(path)));
            if (uniqueAssetPath.Length > 0)
            {
              FileUtil.CopyFileOrDirectory(path, uniqueAssetPath);
              SpriteUtility.ForcedImportFor(uniqueAssetPath);
              Sprite defaultSprite = SpriteUtility.GenerateDefaultSprite(AssetDatabase.LoadMainAssetAtPath(uniqueAssetPath) as UnityEngine.Texture2D);
              if ((UnityEngine.Object) defaultSprite != (UnityEngine.Object) null)
                result.Add(defaultSprite);
            }
          }
        }
      }
    }

    private static bool PathsAreValidTextures(string[] paths)
    {
      if (paths == null || paths.Length == 0)
        return false;
      foreach (string path in paths)
      {
        if (!SpriteUtility.ValidPathForTextureAsset(path))
          return false;
      }
      return true;
    }

    private static void ForcedImportFor(string newPath)
    {
      try
      {
        AssetDatabase.StartAssetEditing();
        AssetDatabase.ImportAsset(newPath);
      }
      finally
      {
        AssetDatabase.StopAssetEditing();
      }
    }

    private static Sprite GenerateDefaultSprite(UnityEngine.Texture2D texture)
    {
      string assetPath = AssetDatabase.GetAssetPath((UnityEngine.Object) texture);
      TextureImporter atPath = AssetImporter.GetAtPath(assetPath) as TextureImporter;
      if ((UnityEngine.Object) atPath == (UnityEngine.Object) null || atPath.textureType != TextureImporterType.Sprite)
        return (Sprite) null;
      if (atPath.spriteImportMode == SpriteImportMode.None)
      {
        atPath.spriteImportMode = SpriteImportMode.Single;
        AssetDatabase.WriteImportSettingsIfDirty(assetPath);
        SpriteUtility.ForcedImportFor(assetPath);
      }
      return ((IEnumerable<UnityEngine.Object>) AssetDatabase.LoadAllAssetsAtPath(assetPath)).FirstOrDefault<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (t => t is Sprite)) as Sprite;
    }

    public static GameObject CreateDragGO(Sprite frame, Vector3 position)
    {
      GameObject gameObject = new GameObject(GameObjectUtility.GetUniqueNameForSibling((Transform) null, !string.IsNullOrEmpty(frame.name) ? frame.name : "Sprite"));
      gameObject.AddComponent<SpriteRenderer>().sprite = frame;
      gameObject.transform.position = position;
      gameObject.hideFlags = HideFlags.HideAndDontSave;
      return gameObject;
    }

    public static bool AddAnimationToGO(GameObject go, Sprite[] frames, SpriteUtility.ShowFileDialogDelegate saveFileDialog)
    {
      SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
      if ((UnityEngine.Object) spriteRenderer == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) SpriteUtility.SpriteUtilityStrings.unableToFindSpriteRendererWarning.text);
        spriteRenderer = go.AddComponent<SpriteRenderer>();
        if ((UnityEngine.Object) spriteRenderer == (UnityEngine.Object) null)
        {
          Debug.LogWarning((object) SpriteUtility.SpriteUtilityStrings.unableToAddSpriteRendererWarning.text);
          return false;
        }
      }
      spriteRenderer.sprite = frames[0];
      return SpriteUtility.CreateAnimation(go, (UnityEngine.Object[]) frames, saveFileDialog);
    }

    public static GameObject DropSpriteToSceneToCreateGO(Sprite sprite, Vector3 position)
    {
      GameObject gameObject = new GameObject(!string.IsNullOrEmpty(sprite.name) ? sprite.name : "Sprite");
      gameObject.AddComponent<SpriteRenderer>().sprite = sprite;
      gameObject.transform.position = position;
      Selection.activeObject = (UnityEngine.Object) gameObject;
      return gameObject;
    }

    public static Sprite RemapObjectToSprite(UnityEngine.Object obj)
    {
      if (obj is Sprite)
        return (Sprite) obj;
      if (obj is UnityEngine.Texture2D)
      {
        UnityEngine.Object[] objectArray = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(obj));
        for (int index = 0; index < objectArray.Length; ++index)
        {
          if (objectArray[index].GetType() == typeof (Sprite))
            return objectArray[index] as Sprite;
        }
      }
      return (Sprite) null;
    }

    public static List<Sprite> TextureToSprites(UnityEngine.Texture2D tex)
    {
      UnityEngine.Object[] objectArray = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) tex));
      List<Sprite> spriteList = new List<Sprite>();
      for (int index = 0; index < objectArray.Length; ++index)
      {
        if (objectArray[index].GetType() == typeof (Sprite))
          spriteList.Add(objectArray[index] as Sprite);
      }
      if (spriteList.Count > 0)
        return spriteList;
      Sprite defaultSprite = SpriteUtility.GenerateDefaultSprite(tex);
      if ((UnityEngine.Object) defaultSprite != (UnityEngine.Object) null)
        spriteList.Add(defaultSprite);
      return spriteList;
    }

    public static Sprite TextureToSprite(UnityEngine.Texture2D tex)
    {
      List<Sprite> sprites = SpriteUtility.TextureToSprites(tex);
      if (sprites.Count > 0)
        return sprites[0];
      return (Sprite) null;
    }

    private static bool ValidPathForTextureAsset(string path)
    {
      string lower = FileUtil.GetPathExtension(path).ToLower();
      return lower == "jpg" || lower == "jpeg" || (lower == "tif" || lower == "tiff") || (lower == "tga" || lower == "gif" || (lower == "png" || lower == "psd")) || (lower == "bmp" || lower == "iff" || (lower == "pict" || lower == "pic") || (lower == "pct" || lower == "exr")) || lower == "hdr";
    }

    public static UnityEngine.Texture2D RenderStaticPreview(Sprite sprite, Color color, int width, int height)
    {
      return SpriteUtility.RenderStaticPreview(sprite, color, width, height, Matrix4x4.identity);
    }

    public static UnityEngine.Texture2D RenderStaticPreview(Sprite sprite, Color color, int width, int height, Matrix4x4 transform)
    {
      if ((UnityEngine.Object) sprite == (UnityEngine.Object) null)
        return (UnityEngine.Texture2D) null;
      PreviewHelpers.AdjustWidthAndHeightForStaticPreview((int) sprite.rect.width, (int) sprite.rect.height, ref width, ref height);
      SavedRenderTargetState renderTargetState = new SavedRenderTargetState();
      RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
      RenderTexture.active = temporary;
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      GL.Clear(true, true, new Color(0.0f, 0.0f, 0.0f, 0.1f));
      SpriteUtility.previewSpriteDefaultMaterial.mainTexture = (Texture) sprite.texture;
      SpriteUtility.previewSpriteDefaultMaterial.SetPass(0);
      SpriteUtility.RenderSpriteImmediate(sprite, color, transform);
      GL.sRGBWrite = false;
      UnityEngine.Texture2D texture2D = new UnityEngine.Texture2D(width, height, TextureFormat.ARGB32, false);
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) width, (float) height), 0, 0);
      texture2D.Apply();
      RenderTexture.ReleaseTemporary(temporary);
      renderTargetState.Restore();
      return texture2D;
    }

    internal static void RenderSpriteImmediate(Sprite sprite, Color color, Matrix4x4 transform)
    {
      float width = sprite.rect.width;
      float height = sprite.rect.height;
      float num1 = sprite.rect.width / sprite.bounds.size.x;
      Vector2[] vertices = sprite.vertices;
      Vector2[] uv = sprite.uv;
      ushort[] triangles = sprite.triangles;
      Vector2 pivot = sprite.pivot;
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Begin(4);
      for (int index = 0; index < sprite.triangles.Length; ++index)
      {
        ushort num2 = triangles[index];
        Vector2 vector2_1 = vertices[(int) num2];
        Vector2 vector2_2 = uv[(int) num2];
        Vector3 point = new Vector3(vector2_1.x, vector2_1.y, 0.0f);
        point = transform.MultiplyPoint(point);
        point.x = (point.x * num1 + pivot.x) / width;
        point.y = (point.y * num1 + pivot.y) / height;
        GL.Color(color);
        GL.TexCoord(new Vector3(vector2_2.x, vector2_2.y, 0.0f));
        GL.Vertex3(point.x, point.y, point.z);
      }
      GL.End();
      GL.PopMatrix();
      GL.sRGBWrite = false;
    }

    public static UnityEngine.Texture2D CreateTemporaryDuplicate(UnityEngine.Texture2D original, int width, int height)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture || !(bool) ((UnityEngine.Object) original))
        return (UnityEngine.Texture2D) null;
      RenderTexture active = RenderTexture.active;
      Rect rawViewportRect = ShaderUtil.rawViewportRect;
      bool flag1 = !TextureUtil.GetLinearSampled((Texture) original);
      RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.Default, !flag1 ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB);
      GL.sRGBWrite = flag1 && QualitySettings.activeColorSpace == ColorSpace.Linear;
      Graphics.Blit((Texture) original, temporary);
      GL.sRGBWrite = false;
      RenderTexture.active = temporary;
      bool flag2 = width >= SystemInfo.maxTextureSize || height >= SystemInfo.maxTextureSize;
      UnityEngine.Texture2D texture2D = new UnityEngine.Texture2D(width, height, TextureFormat.RGBA32, original.mipmapCount > 1 || flag2);
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) width, (float) height), 0, 0);
      texture2D.Apply();
      RenderTexture.ReleaseTemporary(temporary);
      EditorGUIUtility.SetRenderTextureNoViewport(active);
      ShaderUtil.rawViewportRect = rawViewportRect;
      texture2D.alphaIsTransparency = original.alphaIsTransparency;
      return texture2D;
    }

    public static SpriteImportMode GetSpriteImportMode(ISpriteEditorDataProvider dataProvider)
    {
      return dataProvider != null ? dataProvider.spriteImportMode : SpriteImportMode.None;
    }

    private static class SpriteUtilityStrings
    {
      public static readonly GUIContent saveAnimDialogMessage = EditorGUIUtility.TextContent("Create a new animation for the game object '{0}':");
      public static readonly GUIContent saveAnimDialogTitle = EditorGUIUtility.TextContent("Create New Animation");
      public static readonly GUIContent saveAnimDialogName = EditorGUIUtility.TextContent("New Animation");
      public static readonly GUIContent unableToFindSpriteRendererWarning = EditorGUIUtility.TextContent("There should be a SpriteRenderer in dragged object");
      public static readonly GUIContent unableToAddSpriteRendererWarning = EditorGUIUtility.TextContent("Unable to add SpriteRenderer into Gameobject.");
      public static readonly GUIContent failedToCreateAnimationError = EditorGUIUtility.TextContent("Failed to create animation for dragged object");
    }

    private enum DragType
    {
      NotInitialized,
      SpriteAnimation,
      CreateMultiple,
    }

    public delegate string ShowFileDialogDelegate(string title, string defaultName, string extension, string message, string defaultPath);
  }
}
