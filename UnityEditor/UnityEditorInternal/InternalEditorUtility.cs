// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.InternalEditorUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Scripting;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngine.Tilemaps;

namespace UnityEditorInternal
{
  public sealed class InternalEditorUtility
  {
    public static extern bool isApplicationActive { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool inBatchMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool isHumanControllingUs { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void BumpMapSettingsFixingWindowReportResult(int result);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool BumpMapTextureNeedsFixingInternal(Material material, string propName, bool flaggedAsNormal);

    internal static bool BumpMapTextureNeedsFixing(MaterialProperty prop)
    {
      if (prop.type != MaterialProperty.PropType.Texture)
        return false;
      bool flaggedAsNormal = (prop.flags & MaterialProperty.PropFlags.Normal) != MaterialProperty.PropFlags.None;
      foreach (Material target in prop.targets)
      {
        if (InternalEditorUtility.BumpMapTextureNeedsFixingInternal(target, prop.name, flaggedAsNormal))
          return true;
      }
      return false;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void FixNormalmapTextureInternal(Material material, string propName);

    internal static void FixNormalmapTexture(MaterialProperty prop)
    {
      foreach (Material target in prop.targets)
        InternalEditorUtility.FixNormalmapTextureInternal(target, prop.name);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetEditorAssemblyPath();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetEngineAssemblyPath();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetEngineCoreModuleAssemblyPath();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string CalculateHashForObjectsAndDependencies(UnityEngine.Object[] objects);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ExecuteCommandOnKeyWindow(string commandName);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Material[] InstantiateMaterialsInEditMode(Renderer renderer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern CanAppendBuild BuildCanBeAppended(BuildTarget target, string location);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void RegisterExtensionDll(string dllLocation, string guid);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void RegisterPrecompiledAssembly(string dllName, string dllLocation);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Assembly LoadAssemblyWrapper(string dllName, string dllLocation);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetPlatformPath(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int AddScriptComponentUncheckedUndoable(GameObject gameObject, MonoScript script);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int CreateScriptableObjectUnchecked(MonoScript script);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RequestScriptReload();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SwitchSkinAndRepaintAllViews();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RepaintAllViews();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetIsInspectorExpanded(UnityEngine.Object obj);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetIsInspectorExpanded(UnityEngine.Object obj, bool isExpanded);

    public static extern int[] expandedProjectWindowItems { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SaveToSerializedFileAndForget(UnityEngine.Object[] obj, string path, bool allowTextSerialization);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object[] LoadSerializedFileAndForget(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern DragAndDropVisualMode ProjectWindowDrag(HierarchyProperty property, bool perform);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern DragAndDropVisualMode HierarchyWindowDrag(HierarchyProperty property, bool perform, InternalEditorUtility.HierarchyDropMode dropMode);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern DragAndDropVisualMode InspectorWindowDrag(UnityEngine.Object[] targets, bool perform);

    public static DragAndDropVisualMode SceneViewDrag(UnityEngine.Object dropUpon, Vector3 worldPosition, Vector2 viewportPosition, bool perform)
    {
      return InternalEditorUtility.INTERNAL_CALL_SceneViewDrag(dropUpon, ref worldPosition, ref viewportPosition, perform);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern DragAndDropVisualMode INTERNAL_CALL_SceneViewDrag(UnityEngine.Object dropUpon, ref Vector3 worldPosition, ref Vector2 viewportPosition, bool perform);

    public static void SetRectTransformTemporaryRect(RectTransform rectTransform, Rect rect)
    {
      InternalEditorUtility.INTERNAL_CALL_SetRectTransformTemporaryRect(rectTransform, ref rect);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetRectTransformTemporaryRect(RectTransform rectTransform, ref Rect rect);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasTeamLicense();

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasPro();

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasFreeLicense();

    [GeneratedByOldBindingsGenerator]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasEduLicense();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasAdvancedLicenseOnBuildTarget(BuildTarget target);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsMobilePlatform(BuildTarget target);

    public static Rect GetBoundsOfDesktopAtPoint(Vector2 pos)
    {
      Rect rect;
      InternalEditorUtility.INTERNAL_CALL_GetBoundsOfDesktopAtPoint(ref pos, out rect);
      return rect;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetBoundsOfDesktopAtPoint(ref Vector2 pos, out Rect value);

    public static extern string[] tags { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RemoveTag(string tag);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void AddTag(string tag);

    public static extern string[] layers { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string[] GetLayersWithId();

    public static LayerMask ConcatenatedLayersMaskToLayerMask(int concatenatedLayersMask)
    {
      LayerMask layerMask;
      InternalEditorUtility.INTERNAL_CALL_ConcatenatedLayersMaskToLayerMask(concatenatedLayersMask, out layerMask);
      return layerMask;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_ConcatenatedLayersMaskToLayerMask(int concatenatedLayersMask, out LayerMask value);

    public static int LayerMaskToConcatenatedLayersMask(LayerMask mask)
    {
      return InternalEditorUtility.INTERNAL_CALL_LayerMaskToConcatenatedLayersMask(ref mask);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int INTERNAL_CALL_LayerMaskToConcatenatedLayersMask(ref LayerMask mask);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetSortingLayerName(int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetSortingLayerUniqueID(int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetSortingLayerNameFromUniqueID(int id);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetSortingLayerCount();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetSortingLayerName(int index, string name);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetSortingLayerLocked(int index, bool locked);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetSortingLayerLocked(int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsSortingLayerDefault(int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void AddSortingLayer();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateSortingLayersOrder();

    internal static extern string[] sortingLayerNames { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern int[] sortingLayerUniqueIDs { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static Vector4 GetSpriteOuterUV(Sprite sprite, bool getAtlasData)
    {
      Vector4 vector4;
      InternalEditorUtility.INTERNAL_CALL_GetSpriteOuterUV(sprite, getAtlasData, out vector4);
      return vector4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSpriteOuterUV(Sprite sprite, bool getAtlasData, out Vector4 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object GetObjectFromInstanceID(int instanceID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern System.Type GetTypeWithoutLoadingObject(int instanceID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object GetLoadedObjectFromInstanceID(int instanceID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetLayerName(int layer);

    public static extern string unityPreferencesFolder { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetAssetsFolder();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetEditorFolder();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsInEditorFolder(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ReloadWindowLayoutMenu();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RevertFactoryLayoutSettings(bool quitOnCancel);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void LoadDefaultLayout();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void CalculateAmbientProbeFromSkybox();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetupShaderMenu(Material material);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetFullUnityVersion();

    public static Version GetUnityVersion()
    {
      Version version = new Version(InternalEditorUtility.GetUnityVersionDigits());
      return new Version(version.Major, version.Minor, version.Build, InternalEditorUtility.GetUnityRevision());
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetUnityVersionDigits();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetUnityBuildBranch();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetUnityVersionDate();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetUnityRevision();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsUnityBeta();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetUnityCopyright();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetLicenseInfo();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int[] GetLicenseFlags();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetAuthToken();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void OpenEditorConsole();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetGameObjectInstanceIDFromComponent(int instanceID);

    public static Color[] ReadScreenPixel(Vector2 pixelPos, int sizex, int sizey)
    {
      return InternalEditorUtility.INTERNAL_CALL_ReadScreenPixel(ref pixelPos, sizex, sizey);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Color[] INTERNAL_CALL_ReadScreenPixel(ref Vector2 pixelPos, int sizex, int sizey);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetGpuDeviceAndRecreateGraphics(int index, string name);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsGpuDeviceSelectionSupported();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetGpuDevices();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void OpenPlayerConsole();

    public static string TextifyEvent(Event evt)
    {
      if (evt == null)
        return "none";
      KeyCode keyCode = evt.keyCode;
      string str;
      switch (keyCode)
      {
        case KeyCode.Keypad0:
          str = "[0]";
          break;
        case KeyCode.Keypad1:
          str = "[1]";
          break;
        case KeyCode.Keypad2:
          str = "[2]";
          break;
        case KeyCode.Keypad3:
          str = "[3]";
          break;
        case KeyCode.Keypad4:
          str = "[4]";
          break;
        case KeyCode.Keypad5:
          str = "[5]";
          break;
        case KeyCode.Keypad6:
          str = "[6]";
          break;
        case KeyCode.Keypad7:
          str = "[7]";
          break;
        case KeyCode.Keypad8:
          str = "[8]";
          break;
        case KeyCode.Keypad9:
          str = "[9]";
          break;
        case KeyCode.KeypadPeriod:
          str = "[.]";
          break;
        case KeyCode.KeypadDivide:
          str = "[/]";
          break;
        case KeyCode.KeypadMinus:
          str = "[-]";
          break;
        case KeyCode.KeypadPlus:
          str = "[+]";
          break;
        case KeyCode.KeypadEnter:
          str = "enter";
          break;
        case KeyCode.KeypadEquals:
          str = "[=]";
          break;
        case KeyCode.UpArrow:
          str = "up";
          break;
        case KeyCode.DownArrow:
          str = "down";
          break;
        case KeyCode.RightArrow:
          str = "right";
          break;
        case KeyCode.LeftArrow:
          str = "left";
          break;
        case KeyCode.Insert:
          str = "insert";
          break;
        case KeyCode.Home:
          str = "home";
          break;
        case KeyCode.End:
          str = "end";
          break;
        case KeyCode.PageUp:
          str = "page up";
          break;
        case KeyCode.PageDown:
          str = "page down";
          break;
        case KeyCode.F1:
          str = "F1";
          break;
        case KeyCode.F2:
          str = "F2";
          break;
        case KeyCode.F3:
          str = "F3";
          break;
        case KeyCode.F4:
          str = "F4";
          break;
        case KeyCode.F5:
          str = "F5";
          break;
        case KeyCode.F6:
          str = "F6";
          break;
        case KeyCode.F7:
          str = "F7";
          break;
        case KeyCode.F8:
          str = "F8";
          break;
        case KeyCode.F9:
          str = "F9";
          break;
        case KeyCode.F10:
          str = "F10";
          break;
        case KeyCode.F11:
          str = "F11";
          break;
        case KeyCode.F12:
          str = "F12";
          break;
        case KeyCode.F13:
          str = "F13";
          break;
        case KeyCode.F14:
          str = "F14";
          break;
        case KeyCode.F15:
          str = "F15";
          break;
        default:
          str = keyCode == KeyCode.Backspace ? "backspace" : (keyCode == KeyCode.Return ? "return" : (keyCode == KeyCode.Escape ? "[esc]" : (keyCode == KeyCode.Delete ? "delete" : "" + (object) evt.keyCode)));
          break;
      }
      string empty = string.Empty;
      if (evt.alt)
        empty += "Alt+";
      if (evt.command)
        empty += Application.platform != RuntimePlatform.OSXEditor ? "Ctrl+" : "Cmd+";
      if (evt.control)
        empty += "Ctrl+";
      if (evt.shift)
        empty += "Shift+";
      return empty + str;
    }

    public static extern float defaultScreenWidth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern float defaultScreenHeight { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern float defaultWebScreenWidth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern float defaultWebScreenHeight { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern float remoteScreenWidth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern float remoteScreenHeight { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetAvailableDiffTools();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetNoDiffToolsDetectedMessage();

    public static Bounds TransformBounds(Bounds b, Transform t)
    {
      Bounds bounds;
      InternalEditorUtility.INTERNAL_CALL_TransformBounds(ref b, t, out bounds);
      return bounds;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_TransformBounds(ref Bounds b, Transform t, out Bounds value);

    public static void SetCustomLightingInternal(Light[] lights, Color ambient)
    {
      InternalEditorUtility.INTERNAL_CALL_SetCustomLightingInternal(lights, ref ambient);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetCustomLightingInternal(Light[] lights, ref Color ambient);

    public static void SetCustomLighting(Light[] lights, Color ambient)
    {
      if (lights == null)
        throw new ArgumentNullException(nameof (lights));
      InternalEditorUtility.SetCustomLightingInternal(lights, ambient);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ClearSceneLighting();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RemoveCustomLighting();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void DrawSkyboxMaterial(Material mat, Camera cam);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasFullscreenCamera();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ResetCursor();

    public static Bounds CalculateSelectionBounds(bool usePivotOnlyForParticles, bool onlyUseActiveSelection)
    {
      Bounds bounds;
      InternalEditorUtility.INTERNAL_CALL_CalculateSelectionBounds(usePivotOnlyForParticles, onlyUseActiveSelection, out bounds);
      return bounds;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_CalculateSelectionBounds(bool usePivotOnlyForParticles, bool onlyUseActiveSelection, out Bounds value);

    internal static Bounds CalculateSelectionBoundsInSpace(Vector3 position, Quaternion rotation, bool rectBlueprintMode)
    {
      Quaternion quaternion = Quaternion.Inverse(rotation);
      Vector3 vector3_1 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
      Vector3 vector3_2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
      Vector3[] vector3Array = new Vector3[2];
      foreach (GameObject gameObject in Selection.gameObjects)
      {
        Bounds localBounds = InternalEditorUtility.GetLocalBounds(gameObject);
        vector3Array[0] = localBounds.min;
        vector3Array[1] = localBounds.max;
        for (int index1 = 0; index1 < 2; ++index1)
        {
          for (int index2 = 0; index2 < 2; ++index2)
          {
            for (int index3 = 0; index3 < 2; ++index3)
            {
              Vector3 position1 = new Vector3(vector3Array[index1].x, vector3Array[index2].y, vector3Array[index3].z);
              if (rectBlueprintMode && InternalEditorUtility.SupportsRectLayout(gameObject.transform))
              {
                Vector3 localPosition = gameObject.transform.localPosition;
                localPosition.z = 0.0f;
                position1 = gameObject.transform.parent.TransformPoint(position1 + localPosition);
              }
              else
                position1 = gameObject.transform.TransformPoint(position1);
              position1 = quaternion * (position1 - position);
              for (int index4 = 0; index4 < 3; ++index4)
              {
                vector3_1[index4] = Mathf.Min(vector3_1[index4], position1[index4]);
                vector3_2[index4] = Mathf.Max(vector3_2[index4], position1[index4]);
              }
            }
          }
        }
      }
      return new Bounds((vector3_1 + vector3_2) * 0.5f, vector3_2 - vector3_1);
    }

    internal static bool SupportsRectLayout(Transform tr)
    {
      return !((UnityEngine.Object) tr == (UnityEngine.Object) null) && !((UnityEngine.Object) tr.parent == (UnityEngine.Object) null) && (!((UnityEngine.Object) tr.GetComponent<RectTransform>() == (UnityEngine.Object) null) && !((UnityEngine.Object) tr.parent.GetComponent<RectTransform>() == (UnityEngine.Object) null));
    }

    private static Bounds GetLocalBounds(GameObject gameObject)
    {
      RectTransform component1 = gameObject.GetComponent<RectTransform>();
      if ((bool) ((UnityEngine.Object) component1))
        return new Bounds((Vector3) component1.rect.center, (Vector3) component1.rect.size);
      Renderer component2 = gameObject.GetComponent<Renderer>();
      if (component2 is MeshRenderer)
      {
        MeshFilter component3 = component2.GetComponent<MeshFilter>();
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && (UnityEngine.Object) component3.sharedMesh != (UnityEngine.Object) null)
          return component3.sharedMesh.bounds;
      }
      if (component2 is SpriteRenderer)
        return ((SpriteRenderer) component2).GetSpriteBounds();
      if (component2 is SpriteMask)
        return ((SpriteMask) component2).GetSpriteBounds();
      if (component2 is TilemapRenderer)
      {
        Tilemap component3 = component2.GetComponent<Tilemap>();
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
          return component3.localBounds;
      }
      return new Bounds(Vector3.zero, Vector3.zero);
    }

    public static extern bool ignoreInspectorChanges { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void OnGameViewFocus(bool focus);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool OpenFileAtLineExternal(string filename, int line);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool WiiUSaveStartupScreenToFile(Texture2D image, string path, int outputWidth, int outputHeight);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool CanConnectToCacheServer();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ulong VerifyCacheServerIntegrity();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ulong FixCacheServerIntegrityErrors();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern DllType DetectDotNetDll(string path);

    public static bool IsDotNet4Dll(string path)
    {
      DllType dllType = InternalEditorUtility.DetectDotNetDll(path);
      switch (dllType)
      {
        case DllType.Unknown:
        case DllType.Native:
        case DllType.UnknownManaged:
        case DllType.ManagedNET35:
          return false;
        case DllType.ManagedNET40:
        case DllType.WinMDNative:
        case DllType.WinMDNET40:
          return true;
        default:
          throw new Exception(string.Format("Unknown dll type: {0}", (object) dllType));
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetCrashReportFolder();

    [ExcludeFromDocs]
    internal static bool RunningUnderWindows8()
    {
      return InternalEditorUtility.RunningUnderWindows8(true);
    }

    internal static bool RunningUnderWindows8([DefaultValue("true")] bool orHigher)
    {
      if (Application.platform != RuntimePlatform.WindowsEditor)
        return false;
      OperatingSystem osVersion = Environment.OSVersion;
      int major = osVersion.Version.Major;
      int minor = osVersion.Version.Minor;
      if (orHigher)
        return major > 6 || major == 6 && minor >= 2;
      return major == 6 && minor == 2;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int DetermineDepthOrder(Transform lhs, Transform rhs);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ShowPackageManagerWindow();

    public static Vector2 PassAndReturnVector2(Vector2 v)
    {
      Vector2 vector2;
      InternalEditorUtility.INTERNAL_CALL_PassAndReturnVector2(ref v, out vector2);
      return vector2;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_PassAndReturnVector2(ref Vector2 v, out Vector2 value);

    public static Color32 PassAndReturnColor32(Color32 c)
    {
      Color32 color32;
      InternalEditorUtility.INTERNAL_CALL_PassAndReturnColor32(ref c, out color32);
      return color32;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_PassAndReturnColor32(ref Color32 c, out Color32 value);

    public static string CountToString(ulong count)
    {
      string[] strArray = new string[4]{ "g", "m", "k", "" };
      float[] numArray = new float[4]{ 1E+09f, 1000000f, 1000f, 1f };
      int index = 0;
      while (index < 3 && (double) count < (double) numArray[index] / 2.0)
        ++index;
      return ((float) count / numArray[index]).ToString("0.0") + strArray[index];
    }

    [Obsolete("use EditorSceneManager.EnsureUntitledSceneHasBeenSaved")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool EnsureSceneHasBeenSaved(string operation);

    internal static void PrepareDragAndDropTesting(EditorWindow editorWindow)
    {
      if (!((UnityEngine.Object) editorWindow.m_Parent != (UnityEngine.Object) null))
        return;
      InternalEditorUtility.PrepareDragAndDropTestingInternal((GUIView) editorWindow.m_Parent);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void PrepareDragAndDropTestingInternal(GUIView guiView);

    public static bool SaveCursorToFile(string path, Texture2D image, Vector2 hotSpot)
    {
      return InternalEditorUtility.INTERNAL_CALL_SaveCursorToFile(path, image, ref hotSpot);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_SaveCursorToFile(string path, Texture2D image, ref Vector2 hotSpot);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool LaunchApplication(string path, string[] arguments);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string[] GetCompilationDefines(EditorScriptCompilationOptions options, BuildTargetGroup targetGroup, BuildTarget target);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern PrecompiledAssembly[] GetUnityAssemblies(bool buildingForEditor, BuildTargetGroup buildTargetGroup, BuildTarget target);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern PrecompiledAssembly[] GetPrecompiledAssemblies(bool buildingForEditor, BuildTargetGroup buildTargetGroup, BuildTarget target);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetEditorProfile();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsUnityExtensionsInitialized();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsValidUnityExtensionPath(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsUnityExtensionRegistered(string filename);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsUnityExtensionCompatibleWithEditor(BuildTargetGroup targetGroup, BuildTarget target, string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string[] GetEditorModuleDllNames();

    public static Texture2D GetIconForFile(string fileName)
    {
      int num1 = fileName.LastIndexOf('.');
      string key = num1 != -1 ? fileName.Substring(num1 + 1).ToLower() : "";
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (InternalEditorUtility.\u003C\u003Ef__switch\u0024map0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InternalEditorUtility.\u003C\u003Ef__switch\u0024map0 = new Dictionary<string, int>(131)
          {
            {
              "boo",
              0
            },
            {
              "cginc",
              1
            },
            {
              "cs",
              2
            },
            {
              "guiskin",
              3
            },
            {
              "js",
              4
            },
            {
              "dll",
              5
            },
            {
              "asmdef",
              6
            },
            {
              "mat",
              7
            },
            {
              "physicmaterial",
              8
            },
            {
              "prefab",
              9
            },
            {
              "shader",
              10
            },
            {
              "txt",
              11
            },
            {
              "unity",
              12
            },
            {
              "asset",
              13
            },
            {
              "prefs",
              13
            },
            {
              "anim",
              14
            },
            {
              "meta",
              15
            },
            {
              "mixer",
              16
            },
            {
              "ttf",
              17
            },
            {
              "otf",
              17
            },
            {
              "fon",
              17
            },
            {
              "fnt",
              17
            },
            {
              "aac",
              18
            },
            {
              "aif",
              18
            },
            {
              "aiff",
              18
            },
            {
              "au",
              18
            },
            {
              "mid",
              18
            },
            {
              "midi",
              18
            },
            {
              "mp3",
              18
            },
            {
              "mpa",
              18
            },
            {
              "ra",
              18
            },
            {
              "ram",
              18
            },
            {
              "wma",
              18
            },
            {
              "wav",
              18
            },
            {
              "wave",
              18
            },
            {
              "ogg",
              18
            },
            {
              "ai",
              19
            },
            {
              "apng",
              19
            },
            {
              "png",
              19
            },
            {
              "bmp",
              19
            },
            {
              "cdr",
              19
            },
            {
              "dib",
              19
            },
            {
              "eps",
              19
            },
            {
              "exif",
              19
            },
            {
              "gif",
              19
            },
            {
              "ico",
              19
            },
            {
              "icon",
              19
            },
            {
              "j",
              19
            },
            {
              "j2c",
              19
            },
            {
              "j2k",
              19
            },
            {
              "jas",
              19
            },
            {
              "jiff",
              19
            },
            {
              "jng",
              19
            },
            {
              "jp2",
              19
            },
            {
              "jpc",
              19
            },
            {
              "jpe",
              19
            },
            {
              "jpeg",
              19
            },
            {
              "jpf",
              19
            },
            {
              "jpg",
              19
            },
            {
              "jpw",
              19
            },
            {
              "jpx",
              19
            },
            {
              "jtf",
              19
            },
            {
              "mac",
              19
            },
            {
              "omf",
              19
            },
            {
              "qif",
              19
            },
            {
              "qti",
              19
            },
            {
              "qtif",
              19
            },
            {
              "tex",
              19
            },
            {
              "tfw",
              19
            },
            {
              "tga",
              19
            },
            {
              "tif",
              19
            },
            {
              "tiff",
              19
            },
            {
              "wmf",
              19
            },
            {
              "psd",
              19
            },
            {
              "exr",
              19
            },
            {
              "hdr",
              19
            },
            {
              "3df",
              20
            },
            {
              "3dm",
              20
            },
            {
              "3dmf",
              20
            },
            {
              "3ds",
              20
            },
            {
              "3dv",
              20
            },
            {
              "3dx",
              20
            },
            {
              "blend",
              20
            },
            {
              "c4d",
              20
            },
            {
              "lwo",
              20
            },
            {
              "lws",
              20
            },
            {
              "ma",
              20
            },
            {
              "max",
              20
            },
            {
              "mb",
              20
            },
            {
              "mesh",
              20
            },
            {
              "obj",
              20
            },
            {
              "vrl",
              20
            },
            {
              "wrl",
              20
            },
            {
              "wrz",
              20
            },
            {
              "fbx",
              20
            },
            {
              "asf",
              21
            },
            {
              "asx",
              21
            },
            {
              "avi",
              21
            },
            {
              "dat",
              21
            },
            {
              "divx",
              21
            },
            {
              "dvx",
              21
            },
            {
              "mlv",
              21
            },
            {
              "m2l",
              21
            },
            {
              "m2t",
              21
            },
            {
              "m2ts",
              21
            },
            {
              "m2v",
              21
            },
            {
              "m4e",
              21
            },
            {
              "m4v",
              21
            },
            {
              "mjp",
              21
            },
            {
              "mov",
              21
            },
            {
              "movie",
              21
            },
            {
              "mp21",
              21
            },
            {
              "mp4",
              21
            },
            {
              "mpe",
              21
            },
            {
              "mpeg",
              21
            },
            {
              "mpg",
              21
            },
            {
              "mpv2",
              21
            },
            {
              "ogm",
              21
            },
            {
              "qt",
              21
            },
            {
              "rm",
              21
            },
            {
              "rmvb",
              21
            },
            {
              "wmw",
              21
            },
            {
              "xvid",
              21
            },
            {
              "colors",
              22
            },
            {
              "gradients",
              22
            },
            {
              "curves",
              22
            },
            {
              "curvesnormalized",
              22
            },
            {
              "particlecurves",
              22
            },
            {
              "particlecurvessigned",
              22
            },
            {
              "particledoublecurves",
              22
            },
            {
              "particledoublecurvessigned",
              22
            }
          };
        }
        int num2;
        // ISSUE: reference to a compiler-generated field
        if (InternalEditorUtility.\u003C\u003Ef__switch\u0024map0.TryGetValue(key, out num2))
        {
          switch (num2)
          {
            case 0:
              return EditorGUIUtility.FindTexture("boo Script Icon");
            case 1:
              return EditorGUIUtility.FindTexture("CGProgram Icon");
            case 2:
              return EditorGUIUtility.FindTexture("cs Script Icon");
            case 3:
              return EditorGUIUtility.FindTexture("GUISkin Icon");
            case 4:
              return EditorGUIUtility.FindTexture("Js Script Icon");
            case 5:
              return EditorGUIUtility.FindTexture("Assembly Icon");
            case 6:
              return EditorGUIUtility.FindTexture("AssemblyDefinitionAsset Icon");
            case 7:
              return EditorGUIUtility.FindTexture("Material Icon");
            case 8:
              return EditorGUIUtility.FindTexture("PhysicMaterial Icon");
            case 9:
              return EditorGUIUtility.FindTexture("PrefabNormal Icon");
            case 10:
              return EditorGUIUtility.FindTexture("Shader Icon");
            case 11:
              return EditorGUIUtility.FindTexture("TextAsset Icon");
            case 12:
              return EditorGUIUtility.FindTexture("SceneAsset Icon");
            case 13:
              return EditorGUIUtility.FindTexture("GameManager Icon");
            case 14:
              return EditorGUIUtility.FindTexture("Animation Icon");
            case 15:
              return EditorGUIUtility.FindTexture("MetaFile Icon");
            case 16:
              return EditorGUIUtility.FindTexture("AudioMixerController Icon");
            case 17:
              return EditorGUIUtility.FindTexture("Font Icon");
            case 18:
              return EditorGUIUtility.FindTexture("AudioClip Icon");
            case 19:
              return EditorGUIUtility.FindTexture("Texture Icon");
            case 20:
              return EditorGUIUtility.FindTexture("Mesh Icon");
            case 21:
              return EditorGUIUtility.FindTexture("MovieTexture Icon");
            case 22:
              return EditorGUIUtility.FindTexture("ScriptableObject Icon");
          }
        }
      }
      return EditorGUIUtility.FindTexture("DefaultAsset Icon");
    }

    public static string[] GetEditorSettingsList(string prefix, int count)
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 1; index <= count; ++index)
      {
        string str = EditorPrefs.GetString(prefix + (object) index, "defaultValue");
        if (!(str == "defaultValue"))
          arrayList.Add((object) str);
        else
          break;
      }
      return arrayList.ToArray(typeof (string)) as string[];
    }

    public static void SaveEditorSettingsList(string prefix, string[] aList, int count)
    {
      for (int index = 0; index < aList.Length; ++index)
        EditorPrefs.SetString(prefix + (object) (index + 1), aList[index]);
      for (int index = aList.Length + 1; index <= count; ++index)
        EditorPrefs.DeleteKey(prefix + (object) index);
    }

    public static string TextAreaForDocBrowser(Rect position, string text, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID("TextAreaWithTabHandling".GetHashCode(), FocusType.Keyboard, position);
      EditorGUI.RecycledTextEditor recycledEditor = EditorGUI.s_RecycledEditor;
      Event current = Event.current;
      if (recycledEditor.IsEditingControl(controlId) && current.type == EventType.KeyDown)
      {
        if ((int) current.character == 9)
        {
          recycledEditor.Insert('\t');
          current.Use();
          GUI.changed = true;
          text = recycledEditor.text;
        }
        if ((int) current.character == 10)
        {
          recycledEditor.Insert('\n');
          current.Use();
          GUI.changed = true;
          text = recycledEditor.text;
        }
      }
      bool changed;
      text = EditorGUI.DoTextField(recycledEditor, controlId, EditorGUI.IndentedRect(position), text, style, (string) null, out changed, false, true, false);
      return text;
    }

    public static Camera[] GetSceneViewCameras()
    {
      return SceneView.GetAllSceneCameras();
    }

    public static void ShowGameView()
    {
      WindowLayout.ShowAppropriateViewOnEnterExitPlaymode(true);
    }

    public static List<int> GetNewSelection(int clickedInstanceID, List<int> allInstanceIDs, List<int> selectedInstanceIDs, int lastClickedInstanceID, bool keepMultiSelection, bool useShiftAsActionKey, bool allowMultiSelection)
    {
      List<int> intList = new List<int>();
      bool flag1 = Event.current.shift || EditorGUI.actionKey && useShiftAsActionKey;
      bool flag2 = EditorGUI.actionKey && !useShiftAsActionKey;
      if (!allowMultiSelection)
        flag1 = flag2 = false;
      if (flag2)
      {
        intList.AddRange((IEnumerable<int>) selectedInstanceIDs);
        if (intList.Contains(clickedInstanceID))
          intList.Remove(clickedInstanceID);
        else
          intList.Add(clickedInstanceID);
      }
      else if (flag1)
      {
        if (clickedInstanceID == lastClickedInstanceID)
        {
          intList.AddRange((IEnumerable<int>) selectedInstanceIDs);
          return intList;
        }
        int firstIndex;
        int lastIndex;
        if (!InternalEditorUtility.GetFirstAndLastSelected(allInstanceIDs, selectedInstanceIDs, out firstIndex, out lastIndex))
        {
          intList.Add(clickedInstanceID);
          return intList;
        }
        int num1 = -1;
        int num2 = -1;
        for (int index = 0; index < allInstanceIDs.Count; ++index)
        {
          if (allInstanceIDs[index] == clickedInstanceID)
            num1 = index;
          if (lastClickedInstanceID != 0 && allInstanceIDs[index] == lastClickedInstanceID)
            num2 = index;
        }
        int num3 = 0;
        if (num2 != -1)
          num3 = num1 <= num2 ? -1 : 1;
        int num4;
        int num5;
        if (num1 > lastIndex)
        {
          num4 = firstIndex;
          num5 = num1;
        }
        else if (num1 >= firstIndex && num1 < lastIndex)
        {
          if (num3 > 0)
          {
            num4 = num1;
            num5 = lastIndex;
          }
          else
          {
            num4 = firstIndex;
            num5 = num1;
          }
        }
        else
        {
          num4 = num1;
          num5 = lastIndex;
        }
        for (int index = num4; index <= num5; ++index)
          intList.Add(allInstanceIDs[index]);
      }
      else
      {
        if (keepMultiSelection && selectedInstanceIDs.Contains(clickedInstanceID))
        {
          intList.AddRange((IEnumerable<int>) selectedInstanceIDs);
          return intList;
        }
        intList.Add(clickedInstanceID);
      }
      return intList;
    }

    private static bool GetFirstAndLastSelected(List<int> allInstanceIDs, List<int> selectedInstanceIDs, out int firstIndex, out int lastIndex)
    {
      firstIndex = -1;
      lastIndex = -1;
      for (int index = 0; index < allInstanceIDs.Count; ++index)
      {
        if (selectedInstanceIDs.Contains(allInstanceIDs[index]))
        {
          if (firstIndex == -1)
            firstIndex = index;
          lastIndex = index;
        }
      }
      return firstIndex != -1 && lastIndex != -1;
    }

    internal static string GetApplicationExtensionForRuntimePlatform(RuntimePlatform platform)
    {
      if (platform == RuntimePlatform.OSXEditor)
        return "app";
      if (platform == RuntimePlatform.WindowsEditor)
        return "exe";
      return string.Empty;
    }

    public static bool IsValidFileName(string filename)
    {
      string str = InternalEditorUtility.RemoveInvalidCharsFromFileName(filename, false);
      return !(str != filename) && !string.IsNullOrEmpty(str);
    }

    public static string RemoveInvalidCharsFromFileName(string filename, bool logIfInvalidChars)
    {
      if (string.IsNullOrEmpty(filename))
        return filename;
      filename = filename.Trim();
      if (string.IsNullOrEmpty(filename))
        return filename;
      string str1 = new string(Path.GetInvalidFileNameChars());
      string str2 = "";
      bool flag = false;
      foreach (char ch in filename)
      {
        if (str1.IndexOf(ch) == -1)
          str2 += (string) (object) ch;
        else
          flag = true;
      }
      if (flag && logIfInvalidChars)
      {
        string invalidCharsOfFileName = InternalEditorUtility.GetDisplayStringOfInvalidCharsOfFileName(filename);
        if (invalidCharsOfFileName.Length > 0)
          Debug.LogWarningFormat("A filename cannot contain the following character{0}:  {1}", new object[2]
          {
            invalidCharsOfFileName.Length <= 1 ? (object) "" : (object) "s",
            (object) invalidCharsOfFileName
          });
      }
      return str2;
    }

    public static string GetDisplayStringOfInvalidCharsOfFileName(string filename)
    {
      if (string.IsNullOrEmpty(filename))
        return "";
      string str1 = new string(Path.GetInvalidFileNameChars());
      string str2 = "";
      foreach (char ch in filename)
      {
        if (str1.IndexOf(ch) >= 0 && str2.IndexOf(ch) == -1)
        {
          if (str2.Length > 0)
            str2 += " ";
          str2 += (string) (object) ch;
        }
      }
      return str2;
    }

    internal static bool IsScriptOrAssembly(string filename)
    {
      if (string.IsNullOrEmpty(filename))
        return false;
      switch (Path.GetExtension(filename).ToLower())
      {
        case ".cs":
        case ".js":
        case ".boo":
          return true;
        case ".dll":
        case ".exe":
          return AssemblyHelper.IsManagedAssembly(filename);
        default:
          return false;
      }
    }

    internal static T ParentHasComponent<T>(Transform trans) where T : Component
    {
      if (!((UnityEngine.Object) trans != (UnityEngine.Object) null))
        return (T) null;
      T component = trans.GetComponent<T>();
      if ((bool) ((UnityEngine.Object) component))
        return component;
      return InternalEditorUtility.ParentHasComponent<T>(trans.parent);
    }

    internal static IEnumerable<string> GetAllScriptGUIDs()
    {
      return ((IEnumerable<string>) AssetDatabase.GetAllAssetPaths()).Where<string>((Func<string, bool>) (asset => InternalEditorUtility.IsScriptOrAssembly(asset) && !AssetDatabase.IsPackagedAssetPath(asset))).Select<string, string>((Func<string, string>) (asset => AssetDatabase.AssetPathToGUID(asset)));
    }

    internal static MonoIsland[] GetMonoIslandsForPlayer()
    {
      BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.activeBuildTargetGroup;
      BuildTarget activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
      return EditorCompilationInterface.Instance.GetAllMonoIslands(InternalEditorUtility.GetUnityAssemblies(false, buildTargetGroup, activeBuildTarget), InternalEditorUtility.GetPrecompiledAssemblies(false, buildTargetGroup, activeBuildTarget), EditorScriptCompilationOptions.BuildingEmpty);
    }

    internal static MonoIsland[] GetMonoIslands()
    {
      return EditorCompilationInterface.GetAllMonoIslands();
    }

    internal static string[] GetCompilationDefinesForPlayer()
    {
      return InternalEditorUtility.GetCompilationDefines(EditorScriptCompilationOptions.BuildingEmpty, EditorUserBuildSettings.activeBuildTargetGroup, EditorUserBuildSettings.activeBuildTarget);
    }

    internal static string GetMonolithicEngineAssemblyPath()
    {
      return Path.Combine(Path.GetDirectoryName(InternalEditorUtility.GetEditorAssemblyPath()), "UnityEngine.dll");
    }

    public enum HierarchyDropMode
    {
      kHierarchyDragNormal = 0,
      kHierarchyDropUpon = 1,
      kHierarchyDropBetween = 2,
      kHierarchyDropAfterParent = 4,
      kHierarchySearchActive = 8,
    }
  }
}
