// Decompiled with JetBrains decompiler
// Type: UnityEditor.TerrainInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CustomEditor(typeof (Terrain))]
  internal class TerrainInspector : Editor
  {
    internal static PrefKey[] s_ToolKeys = new PrefKey[6]{ new PrefKey("Terrain/Raise Height", "f1"), new PrefKey("Terrain/Set Height", "f2"), new PrefKey("Terrain/Smooth Height", "f3"), new PrefKey("Terrain/Texture Paint", "f4"), new PrefKey("Terrain/Tree Brush", "f5"), new PrefKey("Terrain/Detail Brush", "f6") };
    internal static PrefKey s_PrevBrush = new PrefKey("Terrain/Previous Brush", ",");
    internal static PrefKey s_NextBrush = new PrefKey("Terrain/Next Brush", ".");
    internal static PrefKey s_PrevTexture = new PrefKey("Terrain/Previous Detail", "#,");
    internal static PrefKey s_NextTexture = new PrefKey("Terrain/Next Detail", "#.");
    private static Texture2D[] s_BrushTextures = (Texture2D[]) null;
    private static int s_TerrainEditorHash = "TerrainEditor".GetHashCode();
    private static int s_activeTerrainInspector = 0;
    private Texture2D[] m_SplatIcons = (Texture2D[]) null;
    private GUIContent[] m_TreeContents = (GUIContent[]) null;
    private GUIContent[] m_DetailContents = (GUIContent[]) null;
    private int m_SelectedBrush = 0;
    private int m_SelectedSplat = 0;
    private int m_SelectedDetail = 0;
    private List<ReflectionProbeBlendInfo> m_BlendInfoList = new List<ReflectionProbeBlendInfo>();
    private AnimBool m_ShowBuiltinSpecularSettings = new AnimBool();
    private AnimBool m_ShowCustomMaterialSettings = new AnimBool();
    private AnimBool m_ShowReflectionProbesGUI = new AnimBool();
    private bool m_LODTreePrototypePresent = false;
    private SavedInt m_SelectedTool = new SavedInt("TerrainSelectedTool", 0);
    private static TerrainInspector.Styles styles;
    private const string kDisplayLightingKey = "TerrainInspector.Lighting.ShowSettings";
    private Terrain m_Terrain;
    private TerrainCollider m_TerrainCollider;
    private float m_TargetHeight;
    private float m_Strength;
    private int m_Size;
    private float m_SplatAlpha;
    private float m_DetailOpacity;
    private float m_DetailStrength;
    private const float kHeightmapBrushScale = 0.01f;
    private const float kMinBrushStrength = 0.001678493f;
    private Brush m_CachedBrush;
    private LightingSettingsInspector m_Lighting;

    private static float PercentSlider(GUIContent content, float valueInPercent, float minVal, float maxVal)
    {
      EditorGUI.BeginChangeCheck();
      float num = EditorGUILayout.Slider(content, Mathf.Round(valueInPercent * 100f), minVal * 100f, maxVal * 100f, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        return num / 100f;
      return valueInPercent;
    }

    private void CheckKeys()
    {
      if (TerrainInspector.s_activeTerrainInspector != 0 && TerrainInspector.s_activeTerrainInspector != this.GetInstanceID())
        return;
      for (int index = 0; index < TerrainInspector.s_ToolKeys.Length; ++index)
      {
        if (TerrainInspector.s_ToolKeys[index].activated)
        {
          this.selectedTool = (TerrainTool) index;
          this.Repaint();
          Event.current.Use();
        }
      }
      if (TerrainInspector.s_PrevBrush.activated)
      {
        --this.m_SelectedBrush;
        if (this.m_SelectedBrush < 0)
          this.m_SelectedBrush = TerrainInspector.s_BrushTextures.Length - 1;
        this.Repaint();
        Event.current.Use();
      }
      if (TerrainInspector.s_NextBrush.activated)
      {
        ++this.m_SelectedBrush;
        if (this.m_SelectedBrush >= TerrainInspector.s_BrushTextures.Length)
          this.m_SelectedBrush = 0;
        this.Repaint();
        Event.current.Use();
      }
      int num = 0;
      if (TerrainInspector.s_NextTexture.activated)
        num = 1;
      if (TerrainInspector.s_PrevTexture.activated)
        num = -1;
      if (num == 0)
        return;
      switch (this.selectedTool)
      {
        case TerrainTool.PaintTexture:
          this.m_SelectedSplat = (int) Mathf.Repeat((float) (this.m_SelectedSplat + num), (float) this.m_Terrain.terrainData.splatPrototypes.Length);
          Event.current.Use();
          this.Repaint();
          break;
        case TerrainTool.PlaceTree:
          if (TreePainter.selectedTree >= 0)
            TreePainter.selectedTree = (int) Mathf.Repeat((float) (TreePainter.selectedTree + num), (float) this.m_TreeContents.Length);
          else if (num == -1 && this.m_TreeContents.Length > 0)
            TreePainter.selectedTree = this.m_TreeContents.Length - 1;
          else if (num == 1 && this.m_TreeContents.Length > 0)
            TreePainter.selectedTree = 0;
          Event.current.Use();
          this.Repaint();
          break;
        case TerrainTool.PaintDetail:
          this.m_SelectedDetail = (int) Mathf.Repeat((float) (this.m_SelectedDetail + num), (float) this.m_Terrain.terrainData.detailPrototypes.Length);
          Event.current.Use();
          this.Repaint();
          break;
      }
    }

    private void LoadBrushIcons()
    {
      ArrayList arrayList = new ArrayList();
      int num1 = 1;
      Texture texture1;
      do
      {
        texture1 = (Texture) EditorGUIUtility.Load(EditorResourcesUtility.brushesPath + "builtin_brush_" + (object) num1 + ".png");
        if ((bool) ((UnityEngine.Object) texture1))
          arrayList.Add((object) texture1);
        ++num1;
      }
      while ((bool) ((UnityEngine.Object) texture1));
      int num2 = 0;
      Texture texture2;
      do
      {
        texture2 = (Texture) EditorGUIUtility.FindTexture("brush_" + (object) num2 + ".png");
        if ((bool) ((UnityEngine.Object) texture2))
          arrayList.Add((object) texture2);
        ++num2;
      }
      while ((bool) ((UnityEngine.Object) texture2));
      TerrainInspector.s_BrushTextures = arrayList.ToArray(typeof (Texture2D)) as Texture2D[];
    }

    private void Initialize()
    {
      this.m_Terrain = this.target as Terrain;
      if (TerrainInspector.s_BrushTextures != null)
        return;
      this.LoadBrushIcons();
    }

    private void LoadInspectorSettings()
    {
      this.m_TargetHeight = EditorPrefs.GetFloat("TerrainBrushTargetHeight", 0.2f);
      this.m_Strength = EditorPrefs.GetFloat("TerrainBrushStrength", 0.5f);
      this.m_Size = EditorPrefs.GetInt("TerrainBrushSize", 25);
      this.m_SplatAlpha = EditorPrefs.GetFloat("TerrainBrushSplatAlpha", 1f);
      this.m_DetailOpacity = EditorPrefs.GetFloat("TerrainDetailOpacity", 1f);
      this.m_DetailStrength = EditorPrefs.GetFloat("TerrainDetailStrength", 0.8f);
      this.m_SelectedBrush = EditorPrefs.GetInt("TerrainSelectedBrush", 0);
      this.m_SelectedSplat = EditorPrefs.GetInt("TerrainSelectedSplat", 0);
      this.m_SelectedDetail = EditorPrefs.GetInt("TerrainSelectedDetail", 0);
    }

    private void SaveInspectorSettings()
    {
      EditorPrefs.SetInt("TerrainSelectedDetail", this.m_SelectedDetail);
      EditorPrefs.SetInt("TerrainSelectedSplat", this.m_SelectedSplat);
      EditorPrefs.SetInt("TerrainSelectedBrush", this.m_SelectedBrush);
      EditorPrefs.SetFloat("TerrainDetailStrength", this.m_DetailStrength);
      EditorPrefs.SetFloat("TerrainDetailOpacity", this.m_DetailOpacity);
      EditorPrefs.SetFloat("TerrainBrushSplatAlpha", this.m_SplatAlpha);
      EditorPrefs.SetInt("TerrainBrushSize", this.m_Size);
      EditorPrefs.SetFloat("TerrainBrushStrength", this.m_Strength);
      EditorPrefs.SetFloat("TerrainBrushTargetHeight", this.m_TargetHeight);
    }

    public void OnEnable()
    {
      if (TerrainInspector.s_activeTerrainInspector == 0)
        TerrainInspector.s_activeTerrainInspector = this.GetInstanceID();
      this.m_ShowBuiltinSpecularSettings.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowCustomMaterialSettings.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowReflectionProbesGUI.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      Terrain target = this.target as Terrain;
      if ((UnityEngine.Object) target != (UnityEngine.Object) null)
      {
        this.m_ShowBuiltinSpecularSettings.value = target.materialType == Terrain.MaterialType.BuiltInLegacySpecular;
        this.m_ShowCustomMaterialSettings.value = target.materialType == Terrain.MaterialType.Custom;
        this.m_ShowReflectionProbesGUI.value = target.materialType == Terrain.MaterialType.BuiltInStandard || target.materialType == Terrain.MaterialType.Custom;
      }
      this.LoadInspectorSettings();
      SceneView.onPreSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnPreSceneGUICallback);
      SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnSceneGUICallback);
      this.InitializeLightingFields();
    }

    public void OnDisable()
    {
      SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnSceneGUICallback);
      SceneView.onPreSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnPreSceneGUICallback);
      this.SaveInspectorSettings();
      this.m_ShowReflectionProbesGUI.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowCustomMaterialSettings.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowBuiltinSpecularSettings.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      if (this.m_CachedBrush != null)
        this.m_CachedBrush.Dispose();
      if (TerrainInspector.s_activeTerrainInspector != this.GetInstanceID())
        return;
      TerrainInspector.s_activeTerrainInspector = 0;
    }

    private TerrainTool selectedTool
    {
      get
      {
        if (Tools.current == Tool.None && this.GetInstanceID() == TerrainInspector.s_activeTerrainInspector)
          return (TerrainTool) this.m_SelectedTool.value;
        return TerrainTool.None;
      }
      set
      {
        if (value != TerrainTool.None)
          Tools.current = Tool.None;
        this.m_SelectedTool.value = (int) value;
        TerrainInspector.s_activeTerrainInspector = this.GetInstanceID();
      }
    }

    public void MenuButton(GUIContent title, string menuName, int userData)
    {
      GUIContent content = new GUIContent(title.text, TerrainInspector.styles.settingsIcon, title.tooltip);
      Rect rect = GUILayoutUtility.GetRect(content, TerrainInspector.styles.largeSquare);
      if (!GUI.Button(rect, content, TerrainInspector.styles.largeSquare))
        return;
      MenuCommand command = new MenuCommand((UnityEngine.Object) this.m_Terrain, userData);
      EditorUtility.DisplayPopupMenu(new Rect(rect.x, rect.y, 0.0f, 0.0f), menuName, command);
    }

    public static int AspectSelectionGrid(int selected, Texture[] textures, int approxSize, GUIStyle style, string emptyString, out bool doubleClick)
    {
      GUILayout.BeginVertical((GUIStyle) "box", GUILayout.MinHeight(10f));
      int num1 = 0;
      doubleClick = false;
      if (textures.Length != 0)
      {
        float num2 = (EditorGUIUtility.currentViewWidth - 20f) / (float) approxSize;
        int num3 = (int) Mathf.Ceil((float) textures.Length / num2);
        Rect aspectRect = GUILayoutUtility.GetAspectRect(num2 / (float) num3);
        Event current = Event.current;
        if (current.type == EventType.MouseDown && current.clickCount == 2 && aspectRect.Contains(current.mousePosition))
        {
          doubleClick = true;
          current.Use();
        }
        num1 = GUI.SelectionGrid(aspectRect, Math.Min(selected, textures.Length - 1), textures, Mathf.RoundToInt(EditorGUIUtility.currentViewWidth - 20f) / approxSize, style);
      }
      else
        GUILayout.Label(emptyString);
      GUILayout.EndVertical();
      return num1;
    }

    private static Rect GetBrushAspectRect(int elementCount, int approxSize, int extraLineHeight, out int xCount)
    {
      xCount = (int) Mathf.Ceil((EditorGUIUtility.currentViewWidth - 20f) / (float) approxSize);
      int num = elementCount / xCount;
      if (elementCount % xCount != 0)
        ++num;
      Rect aspectRect = GUILayoutUtility.GetAspectRect((float) xCount / (float) num);
      Rect rect = GUILayoutUtility.GetRect(10f, (float) (extraLineHeight * num));
      aspectRect.height += rect.height;
      return aspectRect;
    }

    public static int AspectSelectionGridImageAndText(int selected, GUIContent[] textures, int approxSize, GUIStyle style, string emptyString, out bool doubleClick)
    {
      EditorGUILayout.BeginVertical(GUIContent.none, EditorStyles.helpBox, GUILayout.MinHeight(10f));
      int num = 0;
      doubleClick = false;
      if (textures.Length != 0)
      {
        int xCount = 0;
        Rect brushAspectRect = TerrainInspector.GetBrushAspectRect(textures.Length, approxSize, 12, out xCount);
        Event current = Event.current;
        if (current.type == EventType.MouseDown && current.clickCount == 2 && brushAspectRect.Contains(current.mousePosition))
        {
          doubleClick = true;
          current.Use();
        }
        num = GUI.SelectionGrid(brushAspectRect, Math.Min(selected, textures.Length - 1), textures, xCount, style);
      }
      else
        GUILayout.Label(emptyString);
      GUILayout.EndVertical();
      return num;
    }

    private void LoadSplatIcons()
    {
      SplatPrototype[] splatPrototypes = this.m_Terrain.terrainData.splatPrototypes;
      this.m_SplatIcons = new Texture2D[splatPrototypes.Length];
      for (int index = 0; index < this.m_SplatIcons.Length; ++index)
        this.m_SplatIcons[index] = AssetPreview.GetAssetPreview((UnityEngine.Object) splatPrototypes[index].texture) ?? splatPrototypes[index].texture;
    }

    private void LoadTreeIcons()
    {
      TreePrototype[] treePrototypes = this.m_Terrain.terrainData.treePrototypes;
      this.m_TreeContents = new GUIContent[treePrototypes.Length];
      for (int index = 0; index < this.m_TreeContents.Length; ++index)
      {
        this.m_TreeContents[index] = new GUIContent();
        Texture assetPreview = (Texture) AssetPreview.GetAssetPreview((UnityEngine.Object) treePrototypes[index].prefab);
        if ((UnityEngine.Object) assetPreview != (UnityEngine.Object) null)
          this.m_TreeContents[index].image = assetPreview;
        if ((UnityEngine.Object) treePrototypes[index].prefab != (UnityEngine.Object) null)
        {
          this.m_TreeContents[index].text = treePrototypes[index].prefab.name;
          this.m_TreeContents[index].tooltip = this.m_TreeContents[index].text;
        }
        else
          this.m_TreeContents[index].text = "Missing";
      }
    }

    private void LoadDetailIcons()
    {
      DetailPrototype[] detailPrototypes = this.m_Terrain.terrainData.detailPrototypes;
      this.m_DetailContents = new GUIContent[detailPrototypes.Length];
      for (int index = 0; index < this.m_DetailContents.Length; ++index)
      {
        this.m_DetailContents[index] = new GUIContent();
        if (detailPrototypes[index].usePrototypeMesh)
        {
          Texture assetPreview = (Texture) AssetPreview.GetAssetPreview((UnityEngine.Object) detailPrototypes[index].prototype);
          if ((UnityEngine.Object) assetPreview != (UnityEngine.Object) null)
            this.m_DetailContents[index].image = assetPreview;
          this.m_DetailContents[index].text = !((UnityEngine.Object) detailPrototypes[index].prototype != (UnityEngine.Object) null) ? "Missing" : detailPrototypes[index].prototype.name;
        }
        else
        {
          Texture prototypeTexture = (Texture) detailPrototypes[index].prototypeTexture;
          if ((UnityEngine.Object) prototypeTexture != (UnityEngine.Object) null)
            this.m_DetailContents[index].image = prototypeTexture;
          this.m_DetailContents[index].text = !((UnityEngine.Object) prototypeTexture != (UnityEngine.Object) null) ? "Missing" : prototypeTexture.name;
        }
      }
    }

    public void ShowTrees()
    {
      this.LoadTreeIcons();
      GUI.changed = false;
      this.ShowUpgradeTreePrototypeScaleUI();
      GUILayout.Label(TerrainInspector.styles.trees, EditorStyles.boldLabel, new GUILayoutOption[0]);
      bool doubleClick;
      TreePainter.selectedTree = TerrainInspector.AspectSelectionGridImageAndText(TreePainter.selectedTree, this.m_TreeContents, 64, TerrainInspector.styles.gridListText, "No trees defined", out doubleClick);
      if (TreePainter.selectedTree >= this.m_TreeContents.Length)
        TreePainter.selectedTree = -1;
      if (doubleClick)
      {
        TerrainTreeContextMenus.EditTree(new MenuCommand((UnityEngine.Object) this.m_Terrain, TreePainter.selectedTree));
        GUIUtility.ExitGUI();
      }
      GUILayout.BeginHorizontal();
      this.ShowMassPlaceTrees();
      GUILayout.FlexibleSpace();
      this.MenuButton(TerrainInspector.styles.editTrees, "CONTEXT/TerrainEngineTrees", TreePainter.selectedTree);
      this.ShowRefreshPrototypes();
      GUILayout.EndHorizontal();
      if (TreePainter.selectedTree == -1)
        return;
      GUILayout.Label(TerrainInspector.styles.settings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      TreePainter.brushSize = EditorGUILayout.Slider(TerrainInspector.styles.brushSize, TreePainter.brushSize, 1f, 100f, new GUILayoutOption[0]);
      float valueInPercent = (float) ((3.29999995231628 - (double) TreePainter.spacing) / 3.0);
      float num = TerrainInspector.PercentSlider(TerrainInspector.styles.treeDensity, valueInPercent, 0.1f, 1f);
      if ((double) num != (double) valueInPercent)
        TreePainter.spacing = (float) ((1.10000002384186 - (double) num) * 3.0);
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal();
      GUILayout.Label(TerrainInspector.styles.treeHeight, new GUILayoutOption[1]
      {
        GUILayout.Width(EditorGUIUtility.labelWidth - 6f)
      });
      GUILayout.Label(TerrainInspector.styles.treeHeightRandomLabel, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      TreePainter.allowHeightVar = GUILayout.Toggle((TreePainter.allowHeightVar ? 1 : 0) != 0, TerrainInspector.styles.treeHeightRandomToggle, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      if (TreePainter.allowHeightVar)
      {
        EditorGUI.BeginChangeCheck();
        float minValue = TreePainter.treeHeight * (1f - TreePainter.treeHeightVariation);
        float maxValue = TreePainter.treeHeight * (1f + TreePainter.treeHeightVariation);
        EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, 0.01f, 2f);
        if (EditorGUI.EndChangeCheck())
        {
          TreePainter.treeHeight = (float) (((double) minValue + (double) maxValue) * 0.5);
          TreePainter.treeHeightVariation = (float) (((double) maxValue - (double) minValue) / ((double) minValue + (double) maxValue));
        }
      }
      else
      {
        TreePainter.treeHeight = EditorGUILayout.Slider(TreePainter.treeHeight, 0.01f, 2f);
        TreePainter.treeHeightVariation = 0.0f;
      }
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      TreePainter.lockWidthToHeight = EditorGUILayout.Toggle(TerrainInspector.styles.lockWidth, TreePainter.lockWidthToHeight, new GUILayoutOption[0]);
      if (TreePainter.lockWidthToHeight)
      {
        TreePainter.treeWidth = TreePainter.treeHeight;
        TreePainter.treeWidthVariation = TreePainter.treeHeightVariation;
        TreePainter.allowWidthVar = TreePainter.allowHeightVar;
      }
      GUILayout.Space(5f);
      using (new EditorGUI.DisabledScope(TreePainter.lockWidthToHeight))
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label(TerrainInspector.styles.treeWidth, new GUILayoutOption[1]
        {
          GUILayout.Width(EditorGUIUtility.labelWidth - 6f)
        });
        GUILayout.Label(TerrainInspector.styles.treeWidthRandomLabel, new GUILayoutOption[1]
        {
          GUILayout.ExpandWidth(false)
        });
        TreePainter.allowWidthVar = GUILayout.Toggle((TreePainter.allowWidthVar ? 1 : 0) != 0, TerrainInspector.styles.treeWidthRandomToggle, new GUILayoutOption[1]
        {
          GUILayout.ExpandWidth(false)
        });
        if (TreePainter.allowWidthVar)
        {
          EditorGUI.BeginChangeCheck();
          float minValue = TreePainter.treeWidth * (1f - TreePainter.treeWidthVariation);
          float maxValue = TreePainter.treeWidth * (1f + TreePainter.treeWidthVariation);
          EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, 0.01f, 2f);
          if (EditorGUI.EndChangeCheck())
          {
            TreePainter.treeWidth = (float) (((double) minValue + (double) maxValue) * 0.5);
            TreePainter.treeWidthVariation = (float) (((double) maxValue - (double) minValue) / ((double) minValue + (double) maxValue));
          }
        }
        else
        {
          TreePainter.treeWidth = EditorGUILayout.Slider(TreePainter.treeWidth, 0.01f, 2f);
          TreePainter.treeWidthVariation = 0.0f;
        }
        GUILayout.EndHorizontal();
      }
      GUILayout.Space(5f);
      if (TerrainEditorUtility.IsLODTreePrototype(this.m_Terrain.terrainData.treePrototypes[TreePainter.selectedTree].m_Prefab))
        TreePainter.randomRotation = EditorGUILayout.Toggle(TerrainInspector.styles.treeRotation, TreePainter.randomRotation, new GUILayoutOption[0]);
      else
        TreePainter.treeColorAdjustment = EditorGUILayout.Slider(TerrainInspector.styles.treeColorVar, TreePainter.treeColorAdjustment, 0.0f, 1f, new GUILayoutOption[0]);
      GameObject prefab = this.m_Terrain.terrainData.treePrototypes[TreePainter.selectedTree].m_Prefab;
      if (!((UnityEngine.Object) prefab != (UnityEngine.Object) null))
        return;
      bool flag = (GameObjectUtility.GetStaticEditorFlags(prefab) & StaticEditorFlags.LightmapStatic) != (StaticEditorFlags) 0;
      using (new EditorGUI.DisabledScope(true))
        EditorGUILayout.Toggle(TerrainInspector.styles.treeLightmapStatic, flag, new GUILayoutOption[0]);
    }

    public void ShowDetails()
    {
      this.LoadDetailIcons();
      this.ShowBrushes();
      GUI.changed = false;
      GUILayout.Label(TerrainInspector.styles.details, EditorStyles.boldLabel, new GUILayoutOption[0]);
      bool doubleClick;
      this.m_SelectedDetail = TerrainInspector.AspectSelectionGridImageAndText(this.m_SelectedDetail, this.m_DetailContents, 64, TerrainInspector.styles.gridListText, "No Detail Objects defined", out doubleClick);
      if (doubleClick)
      {
        TerrainDetailContextMenus.EditDetail(new MenuCommand((UnityEngine.Object) this.m_Terrain, this.m_SelectedDetail));
        GUIUtility.ExitGUI();
      }
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      this.MenuButton(TerrainInspector.styles.editDetails, "CONTEXT/TerrainEngineDetails", this.m_SelectedDetail);
      this.ShowRefreshPrototypes();
      GUILayout.EndHorizontal();
      GUILayout.Label(TerrainInspector.styles.settings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.m_Size = Mathf.RoundToInt(EditorGUILayout.Slider(TerrainInspector.styles.brushSize, (float) this.m_Size, 1f, 100f, new GUILayoutOption[0]));
      this.m_DetailOpacity = EditorGUILayout.Slider(TerrainInspector.styles.opacity, this.m_DetailOpacity, 0.0f, 1f, new GUILayoutOption[0]);
      this.m_DetailStrength = EditorGUILayout.Slider(TerrainInspector.styles.detailTargetStrength, this.m_DetailStrength, 0.0f, 1f, new GUILayoutOption[0]);
      this.m_DetailStrength = Mathf.Round(this.m_DetailStrength * 16f) / 16f;
    }

    public void ShowSettings()
    {
      TerrainData terrainData = this.m_Terrain.terrainData;
      EditorGUI.BeginChangeCheck();
      GUILayout.Label("Base Terrain", EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.m_Terrain.drawHeightmap = EditorGUILayout.Toggle(TerrainInspector.styles.drawTerrain, this.m_Terrain.drawHeightmap, new GUILayoutOption[0]);
      this.m_Terrain.heightmapPixelError = EditorGUILayout.Slider(TerrainInspector.styles.pixelError, this.m_Terrain.heightmapPixelError, 1f, 200f, new GUILayoutOption[0]);
      this.m_Terrain.basemapDistance = EditorGUILayout.Slider(TerrainInspector.styles.baseMapDist, this.m_Terrain.basemapDistance, 0.0f, 2000f, new GUILayoutOption[0]);
      this.m_Terrain.castShadows = EditorGUILayout.Toggle(TerrainInspector.styles.castShadows, this.m_Terrain.castShadows, new GUILayoutOption[0]);
      this.m_Terrain.materialType = (Terrain.MaterialType) EditorGUILayout.EnumPopup(TerrainInspector.styles.material, (Enum) this.m_Terrain.materialType, new GUILayoutOption[0]);
      if (this.m_Terrain.materialType != Terrain.MaterialType.Custom)
        this.m_Terrain.materialTemplate = (Material) null;
      this.m_ShowBuiltinSpecularSettings.target = this.m_Terrain.materialType == Terrain.MaterialType.BuiltInLegacySpecular;
      this.m_ShowCustomMaterialSettings.target = this.m_Terrain.materialType == Terrain.MaterialType.Custom;
      this.m_ShowReflectionProbesGUI.target = this.m_Terrain.materialType == Terrain.MaterialType.BuiltInStandard || this.m_Terrain.materialType == Terrain.MaterialType.Custom;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowBuiltinSpecularSettings.faded))
      {
        ++EditorGUI.indentLevel;
        this.m_Terrain.legacySpecular = EditorGUILayout.ColorField("Specular Color", this.m_Terrain.legacySpecular, new GUILayoutOption[0]);
        this.m_Terrain.legacyShininess = EditorGUILayout.Slider("Shininess", this.m_Terrain.legacyShininess, 0.03f, 1f, new GUILayoutOption[0]);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowCustomMaterialSettings.faded))
      {
        ++EditorGUI.indentLevel;
        this.m_Terrain.materialTemplate = EditorGUILayout.ObjectField("Custom Material", (UnityEngine.Object) this.m_Terrain.materialTemplate, typeof (Material), false, new GUILayoutOption[0]) as Material;
        if ((UnityEngine.Object) this.m_Terrain.materialTemplate != (UnityEngine.Object) null && ShaderUtil.HasTangentChannel(this.m_Terrain.materialTemplate.shader))
          EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("Can't use materials with shaders which need tangent geometry on terrain, use shaders in Nature/Terrain instead.").text, MessageType.Warning, false);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowReflectionProbesGUI.faded))
      {
        this.m_Terrain.reflectionProbeUsage = (ReflectionProbeUsage) EditorGUILayout.EnumPopup(TerrainInspector.styles.reflectionProbes, (Enum) this.m_Terrain.reflectionProbeUsage, new GUILayoutOption[0]);
        if (this.m_Terrain.reflectionProbeUsage != ReflectionProbeUsage.Off)
        {
          ++EditorGUI.indentLevel;
          this.m_Terrain.GetClosestReflectionProbes(this.m_BlendInfoList);
          RendererEditorBase.Probes.ShowClosestReflectionProbes(this.m_BlendInfoList);
          --EditorGUI.indentLevel;
        }
      }
      EditorGUILayout.EndFadeGroup();
      terrainData.thickness = EditorGUILayout.FloatField(TerrainInspector.styles.thickness, terrainData.thickness, new GUILayoutOption[0]);
      GUILayout.Label("Tree & Detail Objects", EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.m_Terrain.drawTreesAndFoliage = EditorGUILayout.Toggle(TerrainInspector.styles.drawTrees, this.m_Terrain.drawTreesAndFoliage, new GUILayoutOption[0]);
      this.m_Terrain.bakeLightProbesForTrees = EditorGUILayout.Toggle(TerrainInspector.styles.bakeLightProbesForTrees, this.m_Terrain.bakeLightProbesForTrees, new GUILayoutOption[0]);
      if (this.m_Terrain.bakeLightProbesForTrees)
        EditorGUILayout.HelpBox("GPU instancing is disabled for trees if light probes are used. Performance may be affected.", MessageType.Info);
      this.m_Terrain.detailObjectDistance = EditorGUILayout.Slider(TerrainInspector.styles.detailObjectDistance, this.m_Terrain.detailObjectDistance, 0.0f, 250f, new GUILayoutOption[0]);
      this.m_Terrain.collectDetailPatches = EditorGUILayout.Toggle(TerrainInspector.styles.collectDetailPatches, this.m_Terrain.collectDetailPatches, new GUILayoutOption[0]);
      this.m_Terrain.detailObjectDensity = EditorGUILayout.Slider(TerrainInspector.styles.detailObjectDensity, this.m_Terrain.detailObjectDensity, 0.0f, 1f, new GUILayoutOption[0]);
      this.m_Terrain.treeDistance = EditorGUILayout.Slider(TerrainInspector.styles.treeDistance, this.m_Terrain.treeDistance, 0.0f, 2000f, new GUILayoutOption[0]);
      this.m_Terrain.treeBillboardDistance = EditorGUILayout.Slider(TerrainInspector.styles.treeBillboardDistance, this.m_Terrain.treeBillboardDistance, 5f, 2000f, new GUILayoutOption[0]);
      this.m_Terrain.treeCrossFadeLength = EditorGUILayout.Slider(TerrainInspector.styles.treeCrossFadeLength, this.m_Terrain.treeCrossFadeLength, 0.0f, 200f, new GUILayoutOption[0]);
      this.m_Terrain.treeMaximumFullLODCount = EditorGUILayout.IntSlider(TerrainInspector.styles.treeMaximumFullLODCount, this.m_Terrain.treeMaximumFullLODCount, 0, 10000, new GUILayoutOption[0]);
      if (Event.current.type == EventType.Layout)
      {
        this.m_LODTreePrototypePresent = false;
        for (int index = 0; index < terrainData.treePrototypes.Length; ++index)
        {
          if (TerrainEditorUtility.IsLODTreePrototype(terrainData.treePrototypes[index].prefab))
          {
            this.m_LODTreePrototypePresent = true;
            break;
          }
        }
      }
      if (this.m_LODTreePrototypePresent)
        EditorGUILayout.HelpBox("Tree Distance, Billboard Start, Fade Length and Max Mesh Trees have no effect on SpeedTree trees. Please use the LOD Group component on the tree prefab to control LOD settings.", MessageType.Info);
      if (EditorGUI.EndChangeCheck())
      {
        EditorApplication.SetSceneRepaintDirty();
        EditorUtility.SetDirty((UnityEngine.Object) this.m_Terrain);
        if (!EditorApplication.isPlaying)
          EditorSceneManager.MarkSceneDirty(this.m_Terrain.gameObject.scene);
      }
      EditorGUI.BeginChangeCheck();
      GUILayout.Label("Wind Settings for Grass", EditorStyles.boldLabel, new GUILayoutOption[0]);
      float num1 = EditorGUILayout.Slider(TerrainInspector.styles.wavingGrassStrength, terrainData.wavingGrassStrength, 0.0f, 1f, new GUILayoutOption[0]);
      float num2 = EditorGUILayout.Slider(TerrainInspector.styles.wavingGrassSpeed, terrainData.wavingGrassSpeed, 0.0f, 1f, new GUILayoutOption[0]);
      float num3 = EditorGUILayout.Slider(TerrainInspector.styles.wavingGrassAmount, terrainData.wavingGrassAmount, 0.0f, 1f, new GUILayoutOption[0]);
      Color color = EditorGUILayout.ColorField(TerrainInspector.styles.wavingGrassTint, terrainData.wavingGrassTint, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        terrainData.wavingGrassStrength = num1;
        terrainData.wavingGrassSpeed = num2;
        terrainData.wavingGrassAmount = num3;
        terrainData.wavingGrassTint = color;
        if (!EditorUtility.IsPersistent((UnityEngine.Object) terrainData) && !EditorApplication.isPlaying)
          EditorSceneManager.MarkSceneDirty(this.m_Terrain.gameObject.scene);
      }
      this.ShowResolution();
      this.ShowHeightmaps();
    }

    public void ShowRaiseHeight()
    {
      this.ShowBrushes();
      GUILayout.Label(TerrainInspector.styles.settings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.ShowBrushSettings();
    }

    public void ShowSmoothHeight()
    {
      this.ShowBrushes();
      GUILayout.Label(TerrainInspector.styles.settings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.ShowBrushSettings();
    }

    public void ShowTextures()
    {
      this.LoadSplatIcons();
      this.ShowBrushes();
      GUILayout.Label(TerrainInspector.styles.textures, EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUI.changed = false;
      bool doubleClick;
      this.m_SelectedSplat = TerrainInspector.AspectSelectionGrid(this.m_SelectedSplat, (Texture[]) this.m_SplatIcons, 64, TerrainInspector.styles.gridList, "No terrain textures defined.", out doubleClick);
      if (doubleClick)
      {
        TerrainSplatContextMenus.EditSplat(new MenuCommand((UnityEngine.Object) this.m_Terrain, this.m_SelectedSplat));
        GUIUtility.ExitGUI();
      }
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      this.MenuButton(TerrainInspector.styles.editTextures, "CONTEXT/TerrainEngineSplats", this.m_SelectedSplat);
      GUILayout.EndHorizontal();
      GUILayout.Label(TerrainInspector.styles.settings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.ShowBrushSettings();
      this.m_SplatAlpha = EditorGUILayout.Slider(TerrainInspector.styles.targetStrength, this.m_SplatAlpha, 0.0f, 1f, new GUILayoutOption[0]);
    }

    public void ShowBrushes()
    {
      GUILayout.Label(TerrainInspector.styles.brushes, EditorStyles.boldLabel, new GUILayoutOption[0]);
      bool doubleClick;
      this.m_SelectedBrush = TerrainInspector.AspectSelectionGrid(this.m_SelectedBrush, (Texture[]) TerrainInspector.s_BrushTextures, 32, TerrainInspector.styles.gridList, "No brushes defined.", out doubleClick);
    }

    public void ShowHeightmaps()
    {
      GUILayout.Label(TerrainInspector.styles.heightmap, EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(TerrainInspector.styles.importRaw))
        TerrainMenus.ImportRaw();
      if (GUILayout.Button(TerrainInspector.styles.exportRaw))
        TerrainMenus.ExportHeightmapRaw();
      GUILayout.EndHorizontal();
    }

    public void ShowResolution()
    {
      GUILayout.Label("Resolution", EditorStyles.boldLabel, new GUILayoutOption[0]);
      float x1 = this.m_Terrain.terrainData.size.x;
      float y1 = this.m_Terrain.terrainData.size.y;
      float z1 = this.m_Terrain.terrainData.size.z;
      int heightmapResolution = this.m_Terrain.terrainData.heightmapResolution;
      int detailResolution = this.m_Terrain.terrainData.detailResolution;
      int resolutionPerPatch1 = this.m_Terrain.terrainData.detailResolutionPerPatch;
      int alphamapResolution = this.m_Terrain.terrainData.alphamapResolution;
      int baseMapResolution = this.m_Terrain.terrainData.baseMapResolution;
      EditorGUI.BeginChangeCheck();
      float x2 = EditorGUILayout.DelayedFloatField(EditorGUIUtility.TextContent("Terrain Width|Size of the terrain object in its X axis (in world units)."), x1, new GUILayoutOption[0]);
      if ((double) x2 <= 0.0)
        x2 = 1f;
      if ((double) x2 > 100000.0)
        x2 = 100000f;
      float z2 = EditorGUILayout.DelayedFloatField(EditorGUIUtility.TextContent("Terrain Length|Size of the terrain object in its Z axis (in world units)."), z1, new GUILayoutOption[0]);
      if ((double) z2 <= 0.0)
        z2 = 1f;
      if ((double) z2 > 100000.0)
        z2 = 100000f;
      float y2 = EditorGUILayout.DelayedFloatField(EditorGUIUtility.TextContent("Terrain Height|Difference in Y coordinate between the lowest possible heightmap value and the highest (in world units)."), y1, new GUILayoutOption[0]);
      if ((double) y2 <= 0.0)
        y2 = 1f;
      if ((double) y2 > 10000.0)
        y2 = 10000f;
      int adjustedSize = this.m_Terrain.terrainData.GetAdjustedSize(Mathf.Clamp(EditorGUILayout.DelayedIntField(EditorGUIUtility.TextContent("Heightmap Resolution|Pixel resolution of the terrain’s heightmap (should be a power of two plus one, eg, 513 = 512 + 1)."), heightmapResolution, new GUILayoutOption[0]), 33, 4097));
      int resolution = Mathf.Clamp(EditorGUILayout.DelayedIntField(EditorGUIUtility.TextContent("Detail Resolution|Resolution of the map that determines the separate patches of details/grass. Higher resolution gives smaller and more detailed patches."), detailResolution, new GUILayoutOption[0]), 0, 4048);
      int resolutionPerPatch2 = Mathf.Clamp(EditorGUILayout.DelayedIntField(EditorGUIUtility.TextContent("Detail Resolution Per Patch|Length/width of the square of patches renderered with a single draw call."), resolutionPerPatch1, new GUILayoutOption[0]), 8, 128);
      int num1 = Mathf.Clamp(Mathf.ClosestPowerOfTwo(EditorGUILayout.DelayedIntField(EditorGUIUtility.TextContent("Control Texture Resolution|Resolution of the “splatmap” that controls the blending of the different terrain textures."), alphamapResolution, new GUILayoutOption[0])), 16, 2048);
      int num2 = Mathf.Clamp(Mathf.ClosestPowerOfTwo(EditorGUILayout.DelayedIntField(EditorGUIUtility.TextContent("Base Texture Resolution|Resolution of the composite texture used on the terrain when viewed from a distance greater than the Basemap Distance."), baseMapResolution, new GUILayoutOption[0])), 16, 2048);
      if (EditorGUI.EndChangeCheck())
      {
        ArrayList arrayList = new ArrayList();
        arrayList.Add((object) this.m_Terrain.terrainData);
        arrayList.AddRange((ICollection) this.m_Terrain.terrainData.alphamapTextures);
        Undo.RegisterCompleteObjectUndo(arrayList.ToArray(typeof (UnityEngine.Object)) as UnityEngine.Object[], "Set Resolution");
        if (this.m_Terrain.terrainData.heightmapResolution != adjustedSize)
          this.m_Terrain.terrainData.heightmapResolution = adjustedSize;
        this.m_Terrain.terrainData.size = new Vector3(x2, y2, z2);
        if (this.m_Terrain.terrainData.detailResolution != resolution || resolutionPerPatch2 != this.m_Terrain.terrainData.detailResolutionPerPatch)
          this.ResizeDetailResolution(this.m_Terrain.terrainData, resolution, resolutionPerPatch2);
        if (this.m_Terrain.terrainData.alphamapResolution != num1)
          this.m_Terrain.terrainData.alphamapResolution = num1;
        if (this.m_Terrain.terrainData.baseMapResolution != num2)
          this.m_Terrain.terrainData.baseMapResolution = num2;
        this.m_Terrain.Flush();
      }
      EditorGUILayout.HelpBox("Please note that modifying the resolution of the heightmap, detail map and control texture will clear their contents, respectively.", MessageType.Warning);
    }

    private void ResizeDetailResolution(TerrainData terrainData, int resolution, int resolutionPerPatch)
    {
      if (resolution == terrainData.detailResolution)
      {
        List<int[,]> numArrayList = new List<int[,]>();
        for (int layer = 0; layer < terrainData.detailPrototypes.Length; ++layer)
          numArrayList.Add(terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, layer));
        terrainData.SetDetailResolution(resolution, resolutionPerPatch);
        for (int layer = 0; layer < numArrayList.Count; ++layer)
          terrainData.SetDetailLayer(0, 0, layer, numArrayList[layer]);
      }
      else
        terrainData.SetDetailResolution(resolution, resolutionPerPatch);
    }

    public void ShowUpgradeTreePrototypeScaleUI()
    {
      if (!((UnityEngine.Object) this.m_Terrain.terrainData != (UnityEngine.Object) null) || !this.m_Terrain.terrainData.NeedUpgradeScaledTreePrototypes())
        return;
      GUIContent content = EditorGUIUtility.TempContent("Some of your prototypes have scaling values on the prefab. Since Unity 5.2 these scalings will be applied to terrain tree instances. Do you want to upgrade to this behaviour?", (Texture) EditorGUIUtility.GetHelpIcon(MessageType.Warning));
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      GUILayout.Label(content, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
      GUILayout.Space(3f);
      if (GUILayout.Button("Upgrade", new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
      {
        this.m_Terrain.terrainData.UpgradeScaledTreePrototype();
        TerrainMenus.RefreshPrototypes();
      }
      GUILayout.Space(3f);
      GUILayout.EndVertical();
    }

    public void ShowRefreshPrototypes()
    {
      if (!GUILayout.Button(TerrainInspector.styles.refresh))
        return;
      TerrainMenus.RefreshPrototypes();
    }

    public void ShowMassPlaceTrees()
    {
      using (new EditorGUI.DisabledScope(TreePainter.selectedTree == -1))
      {
        if (!GUILayout.Button(TerrainInspector.styles.massPlaceTrees))
          return;
        TerrainMenus.MassPlaceTrees();
      }
    }

    public void ShowBrushSettings()
    {
      this.m_Size = Mathf.RoundToInt(EditorGUILayout.Slider(TerrainInspector.styles.brushSize, (float) this.m_Size, 1f, 100f, new GUILayoutOption[0]));
      this.m_Strength = TerrainInspector.PercentSlider(TerrainInspector.styles.opacity, this.m_Strength, 0.001678493f, 1f);
    }

    public void ShowSetHeight()
    {
      this.ShowBrushes();
      GUILayout.Label(TerrainInspector.styles.settings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.ShowBrushSettings();
      GUILayout.BeginHorizontal();
      GUI.changed = false;
      float num1 = this.m_TargetHeight * this.m_Terrain.terrainData.size.y;
      float num2 = EditorGUILayout.Slider(TerrainInspector.styles.height, num1, 0.0f, this.m_Terrain.terrainData.size.y, new GUILayoutOption[0]);
      if (GUI.changed)
        this.m_TargetHeight = num2 / this.m_Terrain.terrainData.size.y;
      if (GUILayout.Button(TerrainInspector.styles.flatten, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_Terrain.terrainData, "Flatten Heightmap");
        HeightmapFilters.Flatten(this.m_Terrain.terrainData, this.m_TargetHeight);
      }
      GUILayout.EndHorizontal();
    }

    public void InitializeLightingFields()
    {
      this.m_Lighting = new LightingSettingsInspector(this.serializedObject);
      this.m_Lighting.showSettings = EditorPrefs.GetBool("TerrainInspector.Lighting.ShowSettings", false);
    }

    public void RenderLightingFields()
    {
      bool showSettings = this.m_Lighting.showSettings;
      if (this.m_Lighting.Begin())
        this.m_Lighting.RenderTerrainSettings();
      this.m_Lighting.End();
      if (this.m_Lighting.showSettings == showSettings)
        return;
      EditorPrefs.SetBool("TerrainInspector.Lighting.ShowSettings", this.m_Lighting.showSettings);
    }

    private void OnInspectorUpdate()
    {
      if (!AssetPreview.HasAnyNewPreviewTexturesAvailable())
        return;
      this.Repaint();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.Initialize();
      if (TerrainInspector.styles == null)
        TerrainInspector.styles = new TerrainInspector.Styles();
      if (!(bool) ((UnityEngine.Object) this.m_Terrain.terrainData))
      {
        GUI.enabled = false;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Toolbar(-1, TerrainInspector.styles.toolIcons, TerrainInspector.styles.command, new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUI.enabled = true;
        GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
        GUILayout.Label("Terrain Asset Missing");
        this.m_Terrain.terrainData = EditorGUILayout.ObjectField("Assign:", (UnityEngine.Object) this.m_Terrain.terrainData, typeof (TerrainData), false, new GUILayoutOption[0]) as TerrainData;
        GUILayout.EndVertical();
      }
      else
      {
        if (Event.current.type == EventType.Layout)
          this.m_TerrainCollider = this.m_Terrain.gameObject.GetComponent<TerrainCollider>();
        if ((bool) ((UnityEngine.Object) this.m_TerrainCollider) && (UnityEngine.Object) this.m_TerrainCollider.terrainData != (UnityEngine.Object) this.m_Terrain.terrainData)
        {
          GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
          GUILayout.Label(TerrainInspector.styles.mismatchedTerrainData, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
          GUILayout.Space(3f);
          if (GUILayout.Button(TerrainInspector.styles.assign, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
          {
            Undo.RecordObject((UnityEngine.Object) this.m_TerrainCollider, "Assign TerrainData");
            this.m_TerrainCollider.terrainData = this.m_Terrain.terrainData;
          }
          GUILayout.Space(3f);
          GUILayout.EndVertical();
        }
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUI.changed = false;
        int selectedTool = (int) this.selectedTool;
        int num = GUILayout.Toolbar(selectedTool, TerrainInspector.styles.toolIcons, TerrainInspector.styles.command, new GUILayoutOption[0]);
        if (num != selectedTool)
        {
          this.selectedTool = (TerrainTool) num;
          InspectorWindow.RepaintAllInspectors();
          if ((UnityEngine.Object) Toolbar.get != (UnityEngine.Object) null)
            Toolbar.get.Repaint();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        this.CheckKeys();
        GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
        if (selectedTool >= 0 && selectedTool < TerrainInspector.styles.toolIcons.Length)
        {
          GUILayout.Label(TerrainInspector.styles.toolNames[selectedTool].text);
          GUILayout.Label(TerrainInspector.styles.toolNames[selectedTool].tooltip, EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
        }
        else
        {
          GUILayout.Label("No tool selected");
          GUILayout.Label("Please select a tool", EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
        }
        GUILayout.EndVertical();
        switch (selectedTool)
        {
          case 0:
            this.ShowRaiseHeight();
            break;
          case 1:
            this.ShowSetHeight();
            break;
          case 2:
            this.ShowSmoothHeight();
            break;
          case 3:
            this.ShowTextures();
            break;
          case 4:
            this.ShowTrees();
            break;
          case 5:
            this.ShowDetails();
            break;
          case 6:
            this.ShowSettings();
            break;
        }
        this.RenderLightingFields();
        GUILayout.Space(5f);
      }
    }

    private Brush GetActiveBrush(int size)
    {
      if (this.m_CachedBrush == null)
        this.m_CachedBrush = new Brush();
      this.m_CachedBrush.Load(TerrainInspector.s_BrushTextures[this.m_SelectedBrush], size);
      return this.m_CachedBrush;
    }

    public bool Raycast(out Vector2 uv, out Vector3 pos)
    {
      RaycastHit hitInfo;
      if (this.m_Terrain.GetComponent<Collider>().Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hitInfo, float.PositiveInfinity))
      {
        uv = hitInfo.textureCoord;
        pos = hitInfo.point;
        return true;
      }
      uv = Vector2.zero;
      pos = Vector3.zero;
      return false;
    }

    public bool HasFrameBounds()
    {
      return (UnityEngine.Object) this.m_Terrain != (UnityEngine.Object) null;
    }

    public Bounds OnGetFrameBounds()
    {
      Vector2 uv;
      Vector3 pos;
      if ((bool) ((UnityEngine.Object) Camera.current) && (bool) ((UnityEngine.Object) this.m_Terrain.terrainData) && this.Raycast(out uv, out pos))
      {
        if ((UnityEngine.Object) SceneView.lastActiveSceneView != (UnityEngine.Object) null)
          SceneView.lastActiveSceneView.viewIsLockedToObject = false;
        Bounds bounds = new Bounds();
        float num = this.selectedTool != TerrainTool.PlaceTree ? (float) this.m_Size : TreePainter.brushSize;
        Vector3 vector3;
        vector3.x = num / (float) this.m_Terrain.terrainData.heightmapWidth * this.m_Terrain.terrainData.size.x;
        vector3.z = num / (float) this.m_Terrain.terrainData.heightmapHeight * this.m_Terrain.terrainData.size.z;
        vector3.y = (float) (((double) vector3.x + (double) vector3.z) * 0.5);
        bounds.center = pos;
        bounds.size = vector3;
        if (this.selectedTool == TerrainTool.PaintDetail && this.m_Terrain.terrainData.detailWidth != 0)
        {
          vector3.x = (float) ((double) num / (double) this.m_Terrain.terrainData.detailWidth * (double) this.m_Terrain.terrainData.size.x * 0.699999988079071);
          vector3.z = (float) ((double) num / (double) this.m_Terrain.terrainData.detailHeight * (double) this.m_Terrain.terrainData.size.z * 0.699999988079071);
          vector3.y = 0.0f;
          bounds.size = vector3;
        }
        return bounds;
      }
      Vector3 position = this.m_Terrain.transform.position;
      if ((UnityEngine.Object) this.m_Terrain.terrainData == (UnityEngine.Object) null)
        return new Bounds(position, Vector3.zero);
      Vector3 size = this.m_Terrain.terrainData.size;
      float[,] heights = this.m_Terrain.terrainData.GetHeights(0, 0, this.m_Terrain.terrainData.heightmapWidth, this.m_Terrain.terrainData.heightmapHeight);
      float a = float.MinValue;
      for (int index1 = 0; index1 < this.m_Terrain.terrainData.heightmapHeight; ++index1)
      {
        for (int index2 = 0; index2 < this.m_Terrain.terrainData.heightmapWidth; ++index2)
          a = Mathf.Max(a, heights[index2, index1]);
      }
      size.y = a * size.y;
      return new Bounds(position + size * 0.5f, size);
    }

    private bool IsModificationToolActive()
    {
      if (!(bool) ((UnityEngine.Object) this.m_Terrain))
        return false;
      TerrainTool selectedTool = this.selectedTool;
      return selectedTool != TerrainTool.TerrainSettings && (selectedTool >= TerrainTool.PaintHeight && selectedTool < TerrainTool.TerrainToolCount);
    }

    private bool IsBrushPreviewVisible()
    {
      if (!this.IsModificationToolActive())
        return false;
      Vector2 uv;
      Vector3 pos;
      return this.Raycast(out uv, out pos);
    }

    private void DisableProjector()
    {
      if (this.m_CachedBrush == null)
        return;
      this.m_CachedBrush.GetPreviewProjector().enabled = false;
    }

    private void UpdatePreviewBrush()
    {
      if (!this.IsModificationToolActive() || (UnityEngine.Object) this.m_Terrain.terrainData == (UnityEngine.Object) null)
      {
        this.DisableProjector();
      }
      else
      {
        Projector previewProjector = this.GetActiveBrush(this.m_Size).GetPreviewProjector();
        float num1 = 1f;
        float num2 = this.m_Terrain.terrainData.size.x / this.m_Terrain.terrainData.size.z;
        Transform transform = previewProjector.transform;
        bool flag = true;
        Vector2 uv;
        Vector3 pos;
        if (this.Raycast(out uv, out pos))
        {
          if (this.selectedTool == TerrainTool.PlaceTree)
          {
            previewProjector.material.mainTexture = (Texture) EditorGUIUtility.Load(EditorResourcesUtility.brushesPath + "builtin_brush_4.png");
            num1 = TreePainter.brushSize / 0.8f;
            num2 = 1f;
          }
          else if (this.selectedTool == TerrainTool.PaintHeight || this.selectedTool == TerrainTool.SetHeight || this.selectedTool == TerrainTool.SmoothHeight)
          {
            if (this.m_Size % 2 == 0)
            {
              float num3 = 0.5f;
              uv.x = (Mathf.Floor(uv.x * (float) (this.m_Terrain.terrainData.heightmapWidth - 1)) + num3) / (float) (this.m_Terrain.terrainData.heightmapWidth - 1);
              uv.y = (Mathf.Floor(uv.y * (float) (this.m_Terrain.terrainData.heightmapHeight - 1)) + num3) / (float) (this.m_Terrain.terrainData.heightmapHeight - 1);
            }
            else
            {
              uv.x = Mathf.Round(uv.x * (float) (this.m_Terrain.terrainData.heightmapWidth - 1)) / (float) (this.m_Terrain.terrainData.heightmapWidth - 1);
              uv.y = Mathf.Round(uv.y * (float) (this.m_Terrain.terrainData.heightmapHeight - 1)) / (float) (this.m_Terrain.terrainData.heightmapHeight - 1);
            }
            pos.x = uv.x * this.m_Terrain.terrainData.size.x;
            pos.z = uv.y * this.m_Terrain.terrainData.size.z;
            pos += this.m_Terrain.transform.position;
            num1 = (float) this.m_Size * 0.5f / (float) this.m_Terrain.terrainData.heightmapWidth * this.m_Terrain.terrainData.size.x;
          }
          else if (this.selectedTool == TerrainTool.PaintTexture || this.selectedTool == TerrainTool.PaintDetail)
          {
            float num3 = this.m_Size % 2 != 0 ? 0.5f : 0.0f;
            int num4;
            int num5;
            if (this.selectedTool == TerrainTool.PaintTexture)
            {
              num4 = this.m_Terrain.terrainData.alphamapWidth;
              num5 = this.m_Terrain.terrainData.alphamapHeight;
            }
            else
            {
              num4 = this.m_Terrain.terrainData.detailWidth;
              num5 = this.m_Terrain.terrainData.detailHeight;
            }
            if (num4 == 0 || num5 == 0)
              flag = false;
            uv.x = (Mathf.Floor(uv.x * (float) num4) + num3) / (float) num4;
            uv.y = (Mathf.Floor(uv.y * (float) num5) + num3) / (float) num5;
            pos.x = uv.x * this.m_Terrain.terrainData.size.x;
            pos.z = uv.y * this.m_Terrain.terrainData.size.z;
            pos += this.m_Terrain.transform.position;
            num1 = (float) this.m_Size * 0.5f / (float) num4 * this.m_Terrain.terrainData.size.x;
            num2 = (float) num4 / (float) num5;
          }
        }
        else
          flag = false;
        previewProjector.enabled = flag;
        if (flag)
        {
          pos.y = this.m_Terrain.transform.position.y + this.m_Terrain.SampleHeight(pos);
          transform.position = pos + new Vector3(0.0f, 50f, 0.0f);
        }
        previewProjector.orthographicSize = num1 / num2;
        previewProjector.aspectRatio = num2;
      }
    }

    public void OnSceneGUICallback(SceneView sceneView)
    {
      this.Initialize();
      if ((UnityEngine.Object) this.m_Terrain == (UnityEngine.Object) null || (UnityEngine.Object) this.m_Terrain.terrainData == (UnityEngine.Object) null)
        return;
      Event current = Event.current;
      this.CheckKeys();
      int controlId = GUIUtility.GetControlID(TerrainInspector.s_TerrainEditorHash, FocusType.Passive);
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != 0 && GUIUtility.hotControl != controlId || current.GetTypeForControl(controlId) == EventType.MouseDrag && GUIUtility.hotControl != controlId || (Event.current.alt || current.button != 0 || (!this.IsModificationToolActive() || HandleUtility.nearestControl != controlId)))
            break;
          if (current.type == EventType.MouseDown)
            GUIUtility.hotControl = controlId;
          Vector2 uv;
          Vector3 pos;
          if (!this.Raycast(out uv, out pos))
            break;
          if (this.selectedTool == TerrainTool.SetHeight && Event.current.shift)
          {
            this.m_TargetHeight = this.m_Terrain.terrainData.GetInterpolatedHeight(uv.x, uv.y) / this.m_Terrain.terrainData.size.y;
            InspectorWindow.RepaintAllInspectors();
          }
          else if (this.selectedTool == TerrainTool.PlaceTree)
          {
            if (current.type == EventType.MouseDown)
              Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_Terrain.terrainData, "Place Tree");
            if (!Event.current.shift && !Event.current.control)
              TreePainter.PlaceTrees(this.m_Terrain, uv.x, uv.y);
            else
              TreePainter.RemoveTrees(this.m_Terrain, uv.x, uv.y, Event.current.control);
          }
          else if (this.selectedTool == TerrainTool.PaintTexture)
          {
            if (current.type == EventType.MouseDown)
            {
              List<UnityEngine.Object> objectList = new List<UnityEngine.Object>();
              objectList.Add((UnityEngine.Object) this.m_Terrain.terrainData);
              objectList.AddRange((IEnumerable<UnityEngine.Object>) this.m_Terrain.terrainData.alphamapTextures);
              Undo.RegisterCompleteObjectUndo(objectList.ToArray(), "Detail Edit");
            }
            SplatPainter splatPainter = new SplatPainter();
            splatPainter.size = this.m_Size;
            splatPainter.strength = this.m_Strength;
            splatPainter.terrainData = this.m_Terrain.terrainData;
            splatPainter.brush = this.GetActiveBrush(splatPainter.size);
            splatPainter.target = this.m_SplatAlpha;
            splatPainter.tool = this.selectedTool;
            this.m_Terrain.editorRenderFlags = TerrainRenderFlags.heightmap;
            splatPainter.Paint(uv.x, uv.y, this.m_SelectedSplat);
            this.m_Terrain.terrainData.SetBasemapDirty(false);
          }
          else if (this.selectedTool == TerrainTool.PaintDetail)
          {
            if (current.type == EventType.MouseDown)
              Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_Terrain.terrainData, "Detail Edit");
            DetailPainter detailPainter = new DetailPainter();
            detailPainter.size = this.m_Size;
            detailPainter.targetStrength = this.m_DetailStrength * 16f;
            detailPainter.opacity = this.m_DetailOpacity;
            if (Event.current.shift || Event.current.control)
              detailPainter.targetStrength *= -1f;
            detailPainter.clearSelectedOnly = Event.current.control;
            detailPainter.terrainData = this.m_Terrain.terrainData;
            detailPainter.brush = this.GetActiveBrush(detailPainter.size);
            detailPainter.tool = this.selectedTool;
            detailPainter.randomizeDetails = true;
            detailPainter.Paint(uv.x, uv.y, this.m_SelectedDetail);
          }
          else
          {
            if (current.type == EventType.MouseDown)
              Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_Terrain.terrainData, "Heightmap Edit");
            HeightmapPainter heightmapPainter = new HeightmapPainter();
            heightmapPainter.size = this.m_Size;
            heightmapPainter.strength = this.m_Strength * 0.01f;
            if (this.selectedTool == TerrainTool.SmoothHeight)
              heightmapPainter.strength = this.m_Strength;
            heightmapPainter.terrainData = this.m_Terrain.terrainData;
            heightmapPainter.brush = this.GetActiveBrush(this.m_Size);
            heightmapPainter.targetHeight = this.m_TargetHeight;
            heightmapPainter.tool = this.selectedTool;
            this.m_Terrain.editorRenderFlags = TerrainRenderFlags.heightmap;
            if (this.selectedTool == TerrainTool.PaintHeight && Event.current.shift)
              heightmapPainter.strength = -heightmapPainter.strength;
            heightmapPainter.PaintHeight(uv.x, uv.y);
          }
          current.Use();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          GUIUtility.hotControl = 0;
          if (!this.IsModificationToolActive())
            break;
          if (this.selectedTool == TerrainTool.PaintTexture)
            this.m_Terrain.terrainData.SetBasemapDirty(true);
          this.m_Terrain.editorRenderFlags = TerrainRenderFlags.all;
          this.m_Terrain.ApplyDelayedHeightmapModification();
          current.Use();
          break;
        case EventType.MouseMove:
          if (!this.IsBrushPreviewVisible())
            break;
          HandleUtility.Repaint();
          break;
        case EventType.Repaint:
          this.DisableProjector();
          break;
        case EventType.Layout:
          if (!this.IsModificationToolActive())
            break;
          HandleUtility.AddDefaultControl(controlId);
          break;
      }
    }

    private void OnPreSceneGUICallback(SceneView sceneView)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      this.UpdatePreviewBrush();
    }

    private class Styles
    {
      public GUIStyle gridList = (GUIStyle) "GridList";
      public GUIStyle gridListText = (GUIStyle) "GridListText";
      public GUIStyle largeSquare = (GUIStyle) "Button";
      public GUIStyle command = (GUIStyle) "Command";
      public Texture settingsIcon = EditorGUIUtility.IconContent("SettingsIcon").image;
      public GUIContent[] toolIcons = new GUIContent[7]{ EditorGUIUtility.IconContent("TerrainInspector.TerrainToolRaise", "|Raise/Lower Terrain)"), EditorGUIUtility.IconContent("TerrainInspector.TerrainToolSetHeight", "|Paint Height"), EditorGUIUtility.IconContent("TerrainInspector.TerrainToolSmoothHeight", "|Smooth Height"), EditorGUIUtility.IconContent("TerrainInspector.TerrainToolSplat", "|Paint Texture"), EditorGUIUtility.IconContent("TerrainInspector.TerrainToolTrees", "|Paint Trees"), EditorGUIUtility.IconContent("TerrainInspector.TerrainToolPlants", "|Paint Details"), EditorGUIUtility.IconContent("TerrainInspector.TerrainToolSettings", "|Terrain Settings") };
      public GUIContent[] toolNames = new GUIContent[7]{ EditorGUIUtility.TextContent("Raise/Lower Terrain|Click to raise.\n\nHold shift and click to lower."), EditorGUIUtility.TextContent("Paint Height|Click to paint height.\n\nHold shift and click to sample target height."), EditorGUIUtility.TextContent("Smooth Height|Click to average out height."), EditorGUIUtility.TextContent("Paint Texture|Select a texture below, then click to paint."), EditorGUIUtility.TextContent("Paint Trees|Click to paint trees.\n\nHold shift and click to erase trees.\n\nHold Ctrl and click to erase only trees of the selected type."), EditorGUIUtility.TextContent("Paint Details|Click to paint details.\n\nHold shift and click to erase details.\n\nHold Ctrl and click to erase only details of the selected type."), EditorGUIUtility.TextContent("Terrain Settings") };
      public GUIContent brushSize = EditorGUIUtility.TextContent("Brush Size|Size of the brush used to paint.");
      public GUIContent opacity = EditorGUIUtility.TextContent("Opacity|Strength of the applied effect.");
      public GUIContent targetStrength = EditorGUIUtility.TextContent("Target Strength|Maximum opacity you can reach by painting continuously.");
      public GUIContent settings = EditorGUIUtility.TextContent("Settings");
      public GUIContent brushes = EditorGUIUtility.TextContent("Brushes");
      public GUIContent mismatchedTerrainData = EditorGUIUtility.TextContentWithIcon("The TerrainData used by the TerrainCollider component is different from this terrain. Would you like to assign the same TerrainData to the TerrainCollider component?", "console.warnicon");
      public GUIContent assign = EditorGUIUtility.TextContent("Assign");
      public GUIContent textures = EditorGUIUtility.TextContent("Textures");
      public GUIContent editTextures = EditorGUIUtility.TextContent("Edit Textures...");
      public GUIContent trees = EditorGUIUtility.TextContent("Trees");
      public GUIContent noTrees = EditorGUIUtility.TextContent("No Trees defined|Use edit button below to add new tree types.");
      public GUIContent editTrees = EditorGUIUtility.TextContent("Edit Trees...|Add/remove tree types.");
      public GUIContent treeDensity = EditorGUIUtility.TextContent("Tree Density|How dense trees are you painting");
      public GUIContent treeHeight = EditorGUIUtility.TextContent("Tree Height|Height of the planted trees");
      public GUIContent treeHeightRandomLabel = EditorGUIUtility.TextContent("Random?|Enable random variation in tree height (variation)");
      public GUIContent treeHeightRandomToggle = EditorGUIUtility.TextContent("|Enable random variation in tree height (variation)");
      public GUIContent lockWidth = EditorGUIUtility.TextContent("Lock Width to Height|Let the tree width be the same with height");
      public GUIContent treeWidth = EditorGUIUtility.TextContent("Tree Width|Width of the planted trees");
      public GUIContent treeWidthRandomLabel = EditorGUIUtility.TextContent("Random?|Enable random variation in tree width (variation)");
      public GUIContent treeWidthRandomToggle = EditorGUIUtility.TextContent("|Enable random variation in tree width (variation)");
      public GUIContent treeColorVar = EditorGUIUtility.TextContent("Color Variation|Amount of random shading applied to trees");
      public GUIContent treeRotation = EditorGUIUtility.TextContent("Random Tree Rotation|Enable?");
      public GUIContent massPlaceTrees = EditorGUIUtility.TextContent("Mass Place Trees|The Mass Place Trees button is a very useful way to create an overall covering of trees without painting over the whole landscape. Following a mass placement, you can still use painting to add or remove trees to create denser or sparser areas.");
      public GUIContent treeLightmapStatic = EditorGUIUtility.TextContent("Tree Lightmap Static|The state of the Lightmap Static flag for the tree prefab root GameObject. The flag can be changed on the prefab. When disabled, this tree will not be visible to the lightmapper. When enabled, any child GameObjects which also have the static flag enabled, will be present in lightmap calculations. Regardless of the Static flag, each tree instance receives its own light probe and no lightmap texels.");
      public GUIContent details = EditorGUIUtility.TextContent("Details");
      public GUIContent editDetails = EditorGUIUtility.TextContent("Edit Details...|Add/remove detail meshes");
      public GUIContent detailTargetStrength = EditorGUIUtility.TextContent("Target Strength|Target amount");
      public GUIContent height = EditorGUIUtility.TextContent("Height|You can set the Height property manually or you can shift-click on the terrain to sample the height at the mouse position (rather like the “eyedropper” tool in an image editor).");
      public GUIContent heightmap = EditorGUIUtility.TextContent("Heightmap");
      public GUIContent importRaw = EditorGUIUtility.TextContent("Import Raw...|The Import Raw button allows you to set the terrain’s heightmap from an image file in the RAW grayscale format. RAW format can be generated by third party terrain editing tools (such as Bryce) and can also be opened, edited and saved by Photoshop. This allows for sophisticated generation and editing of terrains outside Unity.");
      public GUIContent exportRaw = EditorGUIUtility.TextContent("Export Raw...|The Export Raw button allows you to save the terrain’s heightmap to an image file in the RAW grayscale format. RAW format can be generated by third party terrain editing tools (such as Bryce) and can also be opened, edited and saved by Photoshop. This allows for sophisticated generation and editing of terrains outside Unity.");
      public GUIContent flatten = EditorGUIUtility.TextContent("Flatten|The Flatten button levels the whole terrain to the chosen height.");
      public GUIContent bakeLightProbesForTrees = EditorGUIUtility.TextContent("Bake Light Probes For Trees|If the option is enabled, Unity will create internal light probes at the position of each tree (these probes are internal and will not affect other renderers in the scene) and apply them to tree renderers for lighting. Otherwise trees are still affected by LightProbeGroups. The option is only effective for trees that have LightProbe enabled on their prototype prefab.");
      public GUIContent refresh = EditorGUIUtility.TextContent("Refresh|When you save a tree asset from the modelling app, you will need to click the Refresh button (shown in the inspector when the tree painting tool is selected) in order to see the updated trees on your terrain.");
      public GUIContent drawTerrain = EditorGUIUtility.TextContent("Draw|Toggle the rendering of terrain");
      public GUIContent pixelError = EditorGUIUtility.TextContent("Pixel Error|The accuracy of the mapping between the terrain maps (heightmap, textures, etc) and the generated terrain; higher values indicate lower accuracy but lower rendering overhead.");
      public GUIContent baseMapDist = EditorGUIUtility.TextContent("Base Map Dist.|The maximum distance at which terrain textures will be displayed at full resolution. Beyond this distance, a lower resolution composite image will be used for efficiency.");
      public GUIContent castShadows = EditorGUIUtility.TextContent("Cast Shadows|Does the terrain cast shadows?");
      public GUIContent material = EditorGUIUtility.TextContent("Material|The material used to render the terrain. This will affect how the color channels of a terrain texture are interpreted.");
      public GUIContent reflectionProbes = EditorGUIUtility.TextContent("Reflection Probes|How reflection probes are used on terrain. Only effective when using built-in standard material or a custom material which supports rendering with reflection.");
      public GUIContent thickness = EditorGUIUtility.TextContent("Thickness|How much the terrain collision volume should extend along the negative Y-axis. Objects are considered colliding with the terrain from the surface to a depth equal to the thickness. This helps prevent high-speed moving objects from penetrating into the terrain without using expensive continuous collision detection.");
      public GUIContent drawTrees = EditorGUIUtility.TextContent("Draw|Should trees, grass and details be drawn?");
      public GUIContent detailObjectDistance = EditorGUIUtility.TextContent("Detail Distance|The distance (from camera) beyond which details will be culled.");
      public GUIContent collectDetailPatches = EditorGUIUtility.TextContent("Collect Detail Patches|Should detail patches in the Terrain be removed from memory when not visible?");
      public GUIContent detailObjectDensity = EditorGUIUtility.TextContent("Detail Density|The number of detail/grass objects in a given unit of area. The value can be set lower to reduce rendering overhead.");
      public GUIContent treeDistance = EditorGUIUtility.TextContent("Tree Distance|The distance (from camera) beyond which trees will be culled.");
      public GUIContent treeBillboardDistance = EditorGUIUtility.TextContent("Billboard Start|The distance (from camera) at which 3D tree objects will be replaced by billboard images.");
      public GUIContent treeCrossFadeLength = EditorGUIUtility.TextContent("Fade Length|Distance over which trees will transition between 3D objects and billboards.");
      public GUIContent treeMaximumFullLODCount = EditorGUIUtility.TextContent("Max Mesh Trees|The maximum number of visible trees that will be represented as solid 3D meshes. Beyond this limit, trees will be replaced with billboards.");
      public GUIContent wavingGrassStrength = EditorGUIUtility.TextContent("Speed|The speed of the wind as it blows grass.");
      public GUIContent wavingGrassSpeed = EditorGUIUtility.TextContent("Size|The size of the “ripples” on grassy areas as the wind blows over them.");
      public GUIContent wavingGrassAmount = EditorGUIUtility.TextContent("Bending|The degree to which grass objects are bent over by the wind.");
      public GUIContent wavingGrassTint = EditorGUIUtility.TextContent("Grass Tint|Overall color tint applied to grass objects.");
    }
  }
}
