// Decompiled with JetBrains decompiler
// Type: UnityEditor.SubstanceImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (SubstanceArchive))]
  internal class SubstanceImporterInspector : Editor
  {
    private static SubstanceArchive s_LastSelectedPackage = (SubstanceArchive) null;
    private static string s_CachedSelectedMaterialInstanceName = (string) null;
    private static Mesh[] s_Meshes = new Mesh[4];
    private static GUIContent[] s_MeshIcons = new GUIContent[4];
    private static GUIContent[] s_LightIcons = new GUIContent[2];
    private static int previewNoDragDropHash = "PreviewWithoutDragAndDrop".GetHashCode();
    private string m_SelectedMaterialInstanceName = (string) null;
    private Vector2 m_ListScroll = Vector2.zero;
    [NonSerialized]
    private string[] m_PrototypeNames = (string[]) null;
    protected bool m_IsVisible = false;
    public Vector2 previewDir = new Vector2(0.0f, -20f);
    public int selectedMesh = 0;
    public int lightMode = 1;
    private const float kPreviewWidth = 60f;
    private const float kPreviewHeight = 76f;
    private const int kMaxRows = 2;
    private const string kDeprecationWarning = "Built-in support for Substance Designer materials has been deprecated and will be removed in Unity 2018.1. To continue using Substance Designer materials in Unity 2018.1, you will need to install a suitable third-party external importer from the Asset Store.";
    private EditorCache m_EditorCache;
    private Editor m_MaterialInspector;
    private PreviewRenderUtility m_PreviewUtility;
    private SubstanceImporterInspector.SubstanceStyles m_SubstanceStyles;

    public void OnEnable()
    {
      if (this.target == (UnityEngine.Object) SubstanceImporterInspector.s_LastSelectedPackage)
        this.m_SelectedMaterialInstanceName = SubstanceImporterInspector.s_CachedSelectedMaterialInstanceName;
      else
        SubstanceImporterInspector.s_LastSelectedPackage = this.target as SubstanceArchive;
    }

    public void OnDisable()
    {
      if (this.m_EditorCache != null)
        this.m_EditorCache.Dispose();
      if ((UnityEngine.Object) this.m_MaterialInspector != (UnityEngine.Object) null)
      {
        ((ProceduralMaterialInspector) this.m_MaterialInspector).ReimportSubstancesIfNeeded();
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_MaterialInspector);
      }
      SubstanceImporterInspector.s_CachedSelectedMaterialInstanceName = this.m_SelectedMaterialInstanceName;
      if (this.m_PreviewUtility == null)
        return;
      this.m_PreviewUtility.Cleanup();
      this.m_PreviewUtility = (PreviewRenderUtility) null;
    }

    private ProceduralMaterial GetSelectedMaterial()
    {
      if ((UnityEngine.Object) this.GetImporter() == (UnityEngine.Object) null)
        return (ProceduralMaterial) null;
      ProceduralMaterial[] sortedMaterials = this.GetSortedMaterials();
      ProceduralMaterial proceduralMaterial = Array.Find<ProceduralMaterial>(sortedMaterials, (Predicate<ProceduralMaterial>) (element => element.name == this.m_SelectedMaterialInstanceName));
      if ((this.m_SelectedMaterialInstanceName == null || (UnityEngine.Object) proceduralMaterial == (UnityEngine.Object) null) && sortedMaterials.Length > 0)
      {
        proceduralMaterial = sortedMaterials[0];
        this.m_SelectedMaterialInstanceName = proceduralMaterial.name;
      }
      return proceduralMaterial;
    }

    private void SelectNextMaterial()
    {
      if ((UnityEngine.Object) this.GetImporter() == (UnityEngine.Object) null)
        return;
      string str = (string) null;
      ProceduralMaterial[] sortedMaterials = this.GetSortedMaterials();
      for (int index1 = 0; index1 < sortedMaterials.Length; ++index1)
      {
        if (sortedMaterials[index1].name == this.m_SelectedMaterialInstanceName)
        {
          int index2 = Math.Min(index1 + 1, sortedMaterials.Length - 1);
          if (index2 == index1)
            --index2;
          if (index2 >= 0)
          {
            str = sortedMaterials[index2].name;
            break;
          }
          break;
        }
      }
      this.m_SelectedMaterialInstanceName = str;
    }

    private Editor GetSelectedMaterialInspector()
    {
      ProceduralMaterial selectedMaterial = this.GetSelectedMaterial();
      if ((bool) ((UnityEngine.Object) selectedMaterial) && (UnityEngine.Object) this.m_MaterialInspector != (UnityEngine.Object) null && this.m_MaterialInspector.target == (UnityEngine.Object) selectedMaterial)
        return this.m_MaterialInspector;
      EditorGUI.EndEditingActiveTextField();
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_MaterialInspector);
      this.m_MaterialInspector = (Editor) null;
      if ((bool) ((UnityEngine.Object) selectedMaterial))
      {
        this.m_MaterialInspector = Editor.CreateEditor((UnityEngine.Object) selectedMaterial);
        if (!(this.m_MaterialInspector is ProceduralMaterialInspector) && (UnityEngine.Object) this.m_MaterialInspector != (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) selectedMaterial.shader != (UnityEngine.Object) null)
            Debug.LogError((object) ("The shader: '" + selectedMaterial.shader.name + "' is using a custom editor deriving from MaterialEditor, please derive from ShaderGUI instead. Only the ShaderGUI approach works with Procedural Materials. Search the docs for 'ShaderGUI'"));
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_MaterialInspector);
          this.m_MaterialInspector = Editor.CreateEditor((UnityEngine.Object) selectedMaterial, typeof (ProceduralMaterialInspector));
        }
        ((ProceduralMaterialInspector) this.m_MaterialInspector).DisableReimportOnDisable();
      }
      return this.m_MaterialInspector;
    }

    public override void OnInspectorGUI()
    {
      if (this.m_SubstanceStyles == null)
        this.m_SubstanceStyles = new SubstanceImporterInspector.SubstanceStyles();
      EditorGUILayout.Space();
      EditorGUILayout.BeginVertical();
      this.MaterialListing();
      this.MaterialManagement();
      EditorGUILayout.EndVertical();
      Editor materialInspector = this.GetSelectedMaterialInspector();
      if (!(bool) ((UnityEngine.Object) materialInspector))
        return;
      materialInspector.DrawHeader();
      EditorGUILayout.HelpBox("Built-in support for Substance Designer materials has been deprecated and will be removed in Unity 2018.1. To continue using Substance Designer materials in Unity 2018.1, you will need to install a suitable third-party external importer from the Asset Store.", MessageType.Warning);
      materialInspector.OnInspectorGUI();
    }

    private SubstanceImporter GetImporter()
    {
      return AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(this.target)) as SubstanceImporter;
    }

    private void MaterialListing()
    {
      ProceduralMaterial[] sortedMaterials = this.GetSortedMaterials();
      foreach (ProceduralMaterial proceduralMaterial in sortedMaterials)
      {
        if (proceduralMaterial.isProcessing)
        {
          this.Repaint();
          SceneView.RepaintAll();
          GameView.RepaintAll();
          break;
        }
      }
      int length = sortedMaterials.Length;
      float num1 = (float) ((double) GUIView.current.position.width - 16.0 - 18.0 - 2.0);
      if ((double) num1 * 2.0 < (double) length * 60.0)
        num1 -= 16f;
      int num2 = Mathf.Max(1, Mathf.FloorToInt(num1 / 60f));
      int num3 = Mathf.CeilToInt((float) length / (float) num2);
      Rect viewRect = new Rect(0.0f, 0.0f, (float) num2 * 60f, (float) num3 * 76f);
      Rect rect = GUILayoutUtility.GetRect(viewRect.width, Mathf.Clamp(viewRect.height, 76f, 152f) + 1f);
      Rect position1 = new Rect(rect.x + 1f, rect.y + 1f, rect.width - 2f, rect.height - 1f);
      GUI.Box(rect, GUIContent.none, this.m_SubstanceStyles.gridBackground);
      GUI.Box(position1, GUIContent.none, this.m_SubstanceStyles.background);
      this.m_ListScroll = GUI.BeginScrollView(position1, this.m_ListScroll, viewRect, false, false);
      if (this.m_EditorCache == null)
        this.m_EditorCache = new EditorCache(EditorFeatures.PreviewGUI);
      for (int index = 0; index < sortedMaterials.Length; ++index)
      {
        ProceduralMaterial proceduralMaterial = sortedMaterials[index];
        if (!((UnityEngine.Object) proceduralMaterial == (UnityEngine.Object) null))
        {
          Rect position2 = new Rect((float) (index % num2) * 60f, (float) (index / num2) * 76f, 60f, 76f);
          bool flag = proceduralMaterial.name == this.m_SelectedMaterialInstanceName;
          Event current = Event.current;
          int controlId = GUIUtility.GetControlID(SubstanceImporterInspector.previewNoDragDropHash, FocusType.Passive, position2);
          switch (current.GetTypeForControl(controlId))
          {
            case EventType.MouseDown:
              if (current.button == 0 && position2.Contains(current.mousePosition))
              {
                if (current.clickCount == 1)
                {
                  this.m_SelectedMaterialInstanceName = proceduralMaterial.name;
                  current.Use();
                }
                else if (current.clickCount == 2)
                {
                  AssetDatabase.OpenAsset((UnityEngine.Object) proceduralMaterial);
                  GUIUtility.ExitGUI();
                  current.Use();
                }
                break;
              }
              break;
            case EventType.Repaint:
              Rect position3 = position2;
              position3.y = position2.yMax - 16f;
              position3.height = 16f;
              this.m_SubstanceStyles.resultsGridLabel.Draw(position3, EditorGUIUtility.TempContent(proceduralMaterial.name), false, false, flag, flag);
              break;
          }
          position2.height -= 16f;
          this.m_EditorCache[(UnityEngine.Object) proceduralMaterial].OnPreviewGUI(position2, this.m_SubstanceStyles.background);
        }
      }
      GUI.EndScrollView();
    }

    public override bool HasPreviewGUI()
    {
      return (UnityEngine.Object) this.GetSelectedMaterialInspector() != (UnityEngine.Object) null;
    }

    public override void OnPreviewGUI(Rect position, GUIStyle style)
    {
      Editor materialInspector = this.GetSelectedMaterialInspector();
      if (!(bool) ((UnityEngine.Object) materialInspector))
        return;
      materialInspector.OnPreviewGUI(position, style);
    }

    public override string GetInfoString()
    {
      Editor materialInspector = this.GetSelectedMaterialInspector();
      if ((bool) ((UnityEngine.Object) materialInspector))
        return materialInspector.targetTitle + "\n" + materialInspector.GetInfoString();
      return string.Empty;
    }

    public override void OnPreviewSettings()
    {
      Editor materialInspector = this.GetSelectedMaterialInspector();
      if (!(bool) ((UnityEngine.Object) materialInspector))
        return;
      materialInspector.OnPreviewSettings();
    }

    public void InstanciatePrototype(object prototypeName)
    {
      this.m_SelectedMaterialInstanceName = this.GetImporter().InstantiateMaterial(prototypeName as string);
      this.ApplyAndRefresh(false);
    }

    private ProceduralMaterial[] GetSortedMaterials()
    {
      ProceduralMaterial[] materials = this.GetImporter().GetMaterials();
      Array.Sort((Array) materials, (IComparer) new SubstanceImporterInspector.SubstanceNameComparer());
      return materials;
    }

    private void MaterialManagement()
    {
      SubstanceImporter importer = this.GetImporter();
      if (this.m_PrototypeNames == null)
        this.m_PrototypeNames = importer.GetPrototypeNames();
      ProceduralMaterial selectedMaterial = this.GetSelectedMaterial();
      GUILayout.BeginHorizontal(this.m_SubstanceStyles.toolbar, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      using (new EditorGUI.DisabledScope(EditorApplication.isPlaying))
      {
        if (this.m_PrototypeNames.Length > 1)
        {
          Rect rect = GUILayoutUtility.GetRect(this.m_SubstanceStyles.iconToolbarPlus, this.m_SubstanceStyles.toolbarDropDown);
          if (EditorGUI.DropdownButton(rect, this.m_SubstanceStyles.iconToolbarPlus, FocusType.Passive, this.m_SubstanceStyles.toolbarDropDown))
          {
            GenericMenu genericMenu = new GenericMenu();
            for (int index = 0; index < this.m_PrototypeNames.Length; ++index)
              genericMenu.AddItem(new GUIContent(this.m_PrototypeNames[index]), false, new GenericMenu.MenuFunction2(this.InstanciatePrototype), (object) this.m_PrototypeNames[index]);
            genericMenu.DropDown(rect);
          }
        }
        else if (this.m_PrototypeNames.Length == 1 && GUILayout.Button(this.m_SubstanceStyles.iconToolbarPlus, this.m_SubstanceStyles.toolbarButton, new GUILayoutOption[0]))
        {
          this.m_SelectedMaterialInstanceName = this.GetImporter().InstantiateMaterial(this.m_PrototypeNames[0]);
          this.ApplyAndRefresh(true);
        }
        using (new EditorGUI.DisabledScope((UnityEngine.Object) selectedMaterial == (UnityEngine.Object) null))
        {
          if (GUILayout.Button(this.m_SubstanceStyles.iconToolbarMinus, this.m_SubstanceStyles.toolbarButton, new GUILayoutOption[0]) && this.GetSortedMaterials().Length > 1)
          {
            this.SelectNextMaterial();
            importer.DestroyMaterial(selectedMaterial);
            this.ApplyAndRefresh(true);
          }
          if (GUILayout.Button(this.m_SubstanceStyles.iconDuplicate, this.m_SubstanceStyles.toolbarButton, new GUILayoutOption[0]))
          {
            string str = importer.CloneMaterial(selectedMaterial);
            if (str != "")
            {
              this.m_SelectedMaterialInstanceName = str;
              this.ApplyAndRefresh(true);
            }
          }
        }
      }
      EditorGUILayout.EndHorizontal();
    }

    private void ApplyAndRefresh(bool exitGUI)
    {
      AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this.target), ImportAssetOptions.ForceUncompressedImport);
      if (exitGUI)
        GUIUtility.ExitGUI();
      this.Repaint();
    }

    private void Init()
    {
      if (this.m_PreviewUtility == null)
        this.m_PreviewUtility = new PreviewRenderUtility();
      if (!((UnityEngine.Object) SubstanceImporterInspector.s_Meshes[0] == (UnityEngine.Object) null))
        return;
      GameObject gameObject = (GameObject) EditorGUIUtility.LoadRequired("Previews/PreviewMaterials.fbx");
      gameObject.SetActive(false);
      IEnumerator enumerator = gameObject.transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          MeshFilter component = current.GetComponent<MeshFilter>();
          switch (current.name)
          {
            case "sphere":
              SubstanceImporterInspector.s_Meshes[0] = component.sharedMesh;
              break;
            case "cube":
              SubstanceImporterInspector.s_Meshes[1] = component.sharedMesh;
              break;
            case "cylinder":
              SubstanceImporterInspector.s_Meshes[2] = component.sharedMesh;
              break;
            case "torus":
              SubstanceImporterInspector.s_Meshes[3] = component.sharedMesh;
              break;
            default:
              Debug.Log((object) ("Something is wrong, weird object found: " + current.name));
              break;
          }
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      SubstanceImporterInspector.s_MeshIcons[0] = EditorGUIUtility.IconContent("PreMatSphere");
      SubstanceImporterInspector.s_MeshIcons[1] = EditorGUIUtility.IconContent("PreMatCube");
      SubstanceImporterInspector.s_MeshIcons[2] = EditorGUIUtility.IconContent("PreMatCylinder");
      SubstanceImporterInspector.s_MeshIcons[3] = EditorGUIUtility.IconContent("PreMatTorus");
      SubstanceImporterInspector.s_LightIcons[0] = EditorGUIUtility.IconContent("PreMatLight0");
      SubstanceImporterInspector.s_LightIcons[1] = EditorGUIUtility.IconContent("PreMatLight1");
    }

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return (Texture2D) null;
      this.Init();
      this.m_PreviewUtility.BeginStaticPreview(new Rect(0.0f, 0.0f, (float) width, (float) height));
      this.DoRenderPreview(subAssets);
      return this.m_PreviewUtility.EndStaticPreview();
    }

    protected void DoRenderPreview(UnityEngine.Object[] subAssets)
    {
      if (this.m_PreviewUtility.renderTexture.width <= 0 || this.m_PreviewUtility.renderTexture.height <= 0)
        return;
      List<ProceduralMaterial> proceduralMaterialList = new List<ProceduralMaterial>();
      foreach (UnityEngine.Object subAsset in subAssets)
      {
        if (subAsset is ProceduralMaterial)
          proceduralMaterialList.Add(subAsset as ProceduralMaterial);
      }
      int num1 = 1;
      while (num1 * num1 < proceduralMaterialList.Count)
        ++num1;
      int num2 = Mathf.CeilToInt((float) proceduralMaterialList.Count / (float) num1);
      this.m_PreviewUtility.camera.transform.position = -Vector3.forward * 5f * (float) num1;
      this.m_PreviewUtility.camera.transform.rotation = Quaternion.identity;
      this.m_PreviewUtility.camera.farClipPlane = (float) (5 * num1) + 5f;
      this.m_PreviewUtility.camera.nearClipPlane = (float) (5 * num1) - 3f;
      this.m_PreviewUtility.ambientColor = new Color(0.2f, 0.2f, 0.2f, 0.0f);
      if (this.lightMode == 0)
      {
        this.m_PreviewUtility.lights[0].intensity = 1f;
        this.m_PreviewUtility.lights[0].transform.rotation = Quaternion.Euler(30f, 30f, 0.0f);
        this.m_PreviewUtility.lights[1].intensity = 0.0f;
      }
      else
      {
        this.m_PreviewUtility.lights[0].intensity = 1f;
        this.m_PreviewUtility.lights[0].transform.rotation = Quaternion.Euler(50f, 50f, 0.0f);
        this.m_PreviewUtility.lights[1].intensity = 1f;
      }
      for (int index = 0; index < proceduralMaterialList.Count; ++index)
      {
        ProceduralMaterial proceduralMaterial = proceduralMaterialList[index];
        Vector3 pos = new Vector3((float) (index % num1) - (float) (num1 - 1) * 0.5f, (float) (-index / num1) + (float) (num2 - 1) * 0.5f, 0.0f);
        pos *= (float) ((double) Mathf.Tan((float) ((double) this.m_PreviewUtility.camera.fieldOfView * 0.5 * (Math.PI / 180.0))) * 5.0 * 2.0);
        this.m_PreviewUtility.DrawMesh(SubstanceImporterInspector.s_Meshes[this.selectedMesh], pos, Quaternion.Euler(this.previewDir.y, 0.0f, 0.0f) * Quaternion.Euler(0.0f, this.previewDir.x, 0.0f), (Material) proceduralMaterial, 0);
      }
      this.m_PreviewUtility.Render(false, true);
    }

    private class SubstanceStyles
    {
      public GUIContent iconToolbarPlus = EditorGUIUtility.IconContent("Toolbar Plus", "|Add substance from prototype.");
      public GUIContent iconToolbarMinus = EditorGUIUtility.IconContent("Toolbar Minus", "|Remove selected substance.");
      public GUIContent iconDuplicate = EditorGUIUtility.IconContent("TreeEditor.Duplicate", "|Duplicate selected substance.");
      public GUIStyle resultsGridLabel = (GUIStyle) "ObjectPickerResultsGridLabel";
      public GUIStyle resultsGrid = (GUIStyle) "ObjectPickerResultsGrid";
      public GUIStyle gridBackground = (GUIStyle) "TE NodeBackground";
      public GUIStyle background = (GUIStyle) "ObjectPickerBackground";
      public GUIStyle toolbar = (GUIStyle) "TE Toolbar";
      public GUIStyle toolbarButton = (GUIStyle) "TE toolbarbutton";
      public GUIStyle toolbarDropDown = (GUIStyle) "TE toolbarDropDown";
    }

    public class SubstanceNameComparer : IComparer
    {
      public int Compare(object o1, object o2)
      {
        return EditorUtility.NaturalCompare((o1 as UnityEngine.Object).name, (o2 as UnityEngine.Object).name);
      }
    }
  }
}
