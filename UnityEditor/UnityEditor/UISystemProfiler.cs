// Decompiled with JetBrains decompiler
// Type: UnityEditor.UISystemProfiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class UISystemProfiler
  {
    private readonly SplitterState m_TreePreviewHorizontalSplitState = new SplitterState(new float[2]{ 70f, 30f }, new int[2]{ 100, 100 }, (int[]) null);
    private int currentFrame = 0;
    private Material m_CompositeOverdrawMaterial;
    private MultiColumnHeaderState m_MulticolumnHeaderState;
    private UISystemProfilerRenderService m_RenderService;
    private UISystemProfilerTreeView m_TreeViewControl;
    private UISystemProfilerTreeView.State m_UGUIProfilerTreeViewState;
    private ZoomableArea m_ZoomablePreview;
    private UISystemPreviewWindow m_DetachedPreview;

    internal void DrawUIPane(ProfilerWindow win, ProfilerArea profilerArea, UISystemProfilerChart detailsChart)
    {
      this.InitIfNeeded(win);
      EditorGUILayout.BeginVertical();
      if ((UnityEngine.Object) this.m_DetachedPreview != (UnityEngine.Object) null && !(bool) ((UnityEngine.Object) this.m_DetachedPreview))
        this.m_DetachedPreview = (UISystemPreviewWindow) null;
      bool detachedPreview = (bool) ((UnityEngine.Object) this.m_DetachedPreview);
      if (!detachedPreview)
      {
        GUILayout.BeginHorizontal();
        SplitterGUILayout.BeginHorizontalSplit(this.m_TreePreviewHorizontalSplitState);
      }
      Rect controlRect = EditorGUILayout.GetControlRect(new GUILayoutOption[2]{ GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) });
      controlRect.yMin -= EditorGUIUtility.standardVerticalSpacing;
      this.m_TreeViewControl.property = win.CreateProperty(ProfilerColumn.DontSort);
      if (!this.m_TreeViewControl.property.frameDataReady)
      {
        this.m_TreeViewControl.property.Cleanup();
        this.m_TreeViewControl.property = (ProfilerProperty) null;
        GUI.Label(controlRect, UISystemProfiler.Styles.noData);
      }
      else
      {
        int visibleFrameIndex = win.GetActiveVisibleFrameIndex();
        if (this.m_UGUIProfilerTreeViewState != null && this.m_UGUIProfilerTreeViewState.lastFrame != visibleFrameIndex)
        {
          this.currentFrame = ProfilerDriver.lastFrameIndex - visibleFrameIndex;
          this.m_TreeViewControl.Reload();
        }
        this.m_TreeViewControl.OnGUI(controlRect);
        this.m_TreeViewControl.property.Cleanup();
      }
      if (!detachedPreview)
      {
        using (new EditorGUILayout.VerticalScope(new GUILayoutOption[0]))
        {
          using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar, new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) }))
          {
            if (GUILayout.Button(UISystemProfiler.Styles.contentDetachRender, EditorStyles.toolbarButton, new GUILayoutOption[1]{ GUILayout.Width(75f) }))
            {
              this.m_DetachedPreview = EditorWindow.GetWindow<UISystemPreviewWindow>();
              this.m_DetachedPreview.profiler = this;
              this.m_DetachedPreview.Show();
            }
            UISystemProfiler.DrawPreviewToolbarButtons();
          }
          this.DrawRenderUI();
        }
        GUILayout.EndHorizontal();
        SplitterGUILayout.EndHorizontalSplit();
        EditorGUI.DrawRect(new Rect((float) this.m_TreePreviewHorizontalSplitState.realSizes[0] + controlRect.xMin, controlRect.y, 1f, controlRect.height), UISystemProfiler.Styles.separatorColor);
      }
      EditorGUILayout.EndVertical();
      if (!(bool) ((UnityEngine.Object) this.m_DetachedPreview))
        return;
      this.m_DetachedPreview.Repaint();
    }

    internal static void DrawPreviewToolbarButtons()
    {
      UISystemProfiler.PreviewBackground = (UISystemProfiler.Styles.PreviewBackgroundType) EditorGUILayout.IntPopup(GUIContent.none, (int) UISystemProfiler.PreviewBackground, UISystemProfiler.Styles.backgroundOptions, UISystemProfiler.Styles.backgroundValues, EditorStyles.toolbarDropDown, new GUILayoutOption[1]
      {
        GUILayout.MaxWidth(100f)
      });
      UISystemProfiler.PreviewRenderMode = (UISystemProfiler.Styles.RenderMode) EditorGUILayout.IntPopup(GUIContent.none, (int) UISystemProfiler.PreviewRenderMode, UISystemProfiler.Styles.rendermodeOptions, UISystemProfiler.Styles.rendermodeValues, EditorStyles.toolbarDropDown, new GUILayoutOption[1]
      {
        GUILayout.MaxWidth(100f)
      });
    }

    private static UISystemProfiler.Styles.RenderMode PreviewRenderMode
    {
      get
      {
        return (UISystemProfiler.Styles.RenderMode) EditorPrefs.GetInt("UGUIProfiler.Overdraw", 0);
      }
      set
      {
        EditorPrefs.SetInt("UGUIProfiler.Overdraw", (int) value);
      }
    }

    private static UISystemProfiler.Styles.PreviewBackgroundType PreviewBackground
    {
      get
      {
        return (UISystemProfiler.Styles.PreviewBackgroundType) EditorPrefs.GetInt("UGUIProfiler.CheckerBoard", 0);
      }
      set
      {
        EditorPrefs.SetInt("UGUIProfiler.CheckerBoard", (int) value);
      }
    }

    internal void DrawRenderUI()
    {
      Rect controlRect = EditorGUILayout.GetControlRect(new GUILayoutOption[2]{ GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) });
      GUI.Box(controlRect, GUIContent.none);
      this.m_ZoomablePreview.BeginViewGUI();
      bool flag = true;
      if (this.m_UGUIProfilerTreeViewState != null && Event.current.type == EventType.Repaint)
      {
        IList<int> selection = this.m_TreeViewControl.GetSelection();
        if (selection.Count > 0)
        {
          foreach (TreeViewItem rowsFromId in (IEnumerable<TreeViewItem>) this.m_TreeViewControl.GetRowsFromIDs(selection))
          {
            Texture2D texture2D = (Texture2D) null;
            UISystemProfilerTreeView.BatchTreeViewItem batchTreeViewItem = rowsFromId as UISystemProfilerTreeView.BatchTreeViewItem;
            UISystemProfiler.Styles.RenderMode previewRenderMode = UISystemProfiler.PreviewRenderMode;
            if (this.m_RenderService == null)
              this.m_RenderService = new UISystemProfilerRenderService();
            if (batchTreeViewItem != null)
              texture2D = this.m_RenderService.GetThumbnail(this.currentFrame, batchTreeViewItem.renderDataIndex, 1, previewRenderMode != UISystemProfiler.Styles.RenderMode.Standard);
            UISystemProfilerTreeView.CanvasTreeViewItem canvasTreeViewItem = rowsFromId as UISystemProfilerTreeView.CanvasTreeViewItem;
            if (canvasTreeViewItem != null)
              texture2D = this.m_RenderService.GetThumbnail(this.currentFrame, canvasTreeViewItem.info.renderDataIndex, canvasTreeViewItem.info.renderDataCount, previewRenderMode != UISystemProfiler.Styles.RenderMode.Standard);
            if (previewRenderMode == UISystemProfiler.Styles.RenderMode.CompositeOverdraw && (UnityEngine.Object) this.m_CompositeOverdrawMaterial == (UnityEngine.Object) null)
            {
              Shader shader = Shader.Find("Hidden/UI/CompositeOverdraw");
              if ((bool) ((UnityEngine.Object) shader))
                this.m_CompositeOverdrawMaterial = new Material(shader);
            }
            if ((bool) ((UnityEngine.Object) texture2D))
            {
              float width1 = (float) texture2D.width;
              float height1 = (float) texture2D.height;
              float num = Math.Min(controlRect.width / width1, controlRect.height / height1);
              float width2 = width1 * num;
              float height2 = height1 * num;
              Rect rect = new Rect(controlRect.x + (float) (((double) controlRect.width - (double) width2) / 2.0), controlRect.y + (float) (((double) controlRect.height - (double) height2) / 2.0), width2, height2);
              if (flag)
              {
                flag = false;
                this.m_ZoomablePreview.rect = rect;
                UISystemProfiler.Styles.PreviewBackgroundType previewBackground = UISystemProfiler.PreviewBackground;
                if (previewBackground == UISystemProfiler.Styles.PreviewBackgroundType.Checkerboard)
                  EditorGUI.DrawTransparencyCheckerTexture(this.m_ZoomablePreview.drawRect, ScaleMode.ScaleAndCrop, 0.0f);
                else
                  EditorGUI.DrawRect(this.m_ZoomablePreview.drawRect, previewBackground != UISystemProfiler.Styles.PreviewBackgroundType.Black ? Color.white : Color.black);
              }
              Graphics.DrawTexture(this.m_ZoomablePreview.drawRect, (Texture) texture2D, this.m_ZoomablePreview.shownArea, 0, 0, 0, 0, previewRenderMode != UISystemProfiler.Styles.RenderMode.CompositeOverdraw ? EditorGUI.transparentMaterial : this.m_CompositeOverdrawMaterial);
            }
            if (previewRenderMode != UISystemProfiler.Styles.RenderMode.Standard)
              break;
          }
        }
      }
      if (flag && Event.current.type == EventType.Repaint)
        this.m_ZoomablePreview.rect = controlRect;
      this.m_ZoomablePreview.EndViewGUI();
    }

    private void InitIfNeeded(ProfilerWindow win)
    {
      if (this.m_ZoomablePreview != null)
        return;
      this.m_ZoomablePreview = new ZoomableArea(true, false)
      {
        hRangeMin = 0.0f,
        vRangeMin = 0.0f,
        hRangeMax = 1f,
        vRangeMax = 1f
      };
      this.m_ZoomablePreview.SetShownHRange(0.0f, 1f);
      this.m_ZoomablePreview.SetShownVRange(0.0f, 1f);
      this.m_ZoomablePreview.uniformScale = true;
      this.m_ZoomablePreview.scaleWithWindow = true;
      int num1 = 100;
      int num2 = 200;
      this.m_MulticolumnHeaderState = new MultiColumnHeaderState(new MultiColumnHeaderState.Column[8]
      {
        new MultiColumnHeaderState.Column()
        {
          headerContent = EditorGUIUtility.TextContent("Object"),
          width = 220f,
          maxWidth = 400f,
          canSort = true
        },
        new MultiColumnHeaderState.Column()
        {
          headerContent = EditorGUIUtility.TextContent("Self Batch Count"),
          width = (float) num1,
          maxWidth = (float) num2
        },
        new MultiColumnHeaderState.Column()
        {
          headerContent = EditorGUIUtility.TextContent("Cumulative Batch Count"),
          width = (float) num1,
          maxWidth = (float) num2
        },
        new MultiColumnHeaderState.Column()
        {
          headerContent = EditorGUIUtility.TextContent("Self Vertex Count"),
          width = (float) num1,
          maxWidth = (float) num2
        },
        new MultiColumnHeaderState.Column()
        {
          headerContent = EditorGUIUtility.TextContent("Cumulative Vertex Count"),
          width = (float) num1,
          maxWidth = (float) num2
        },
        new MultiColumnHeaderState.Column()
        {
          headerContent = EditorGUIUtility.TextContent("Batch Breaking Reason"),
          width = 220f,
          maxWidth = 400f,
          canSort = false
        },
        new MultiColumnHeaderState.Column()
        {
          headerContent = EditorGUIUtility.TextContent("GameObject Count"),
          width = (float) num1,
          maxWidth = 400f
        },
        new MultiColumnHeaderState.Column()
        {
          headerContent = EditorGUIUtility.TextContent("GameObjects"),
          width = 150f,
          maxWidth = 400f,
          canSort = false
        }
      });
      foreach (MultiColumnHeaderState.Column column in this.m_MulticolumnHeaderState.columns)
        column.sortingArrowAlignment = TextAlignment.Right;
      this.m_UGUIProfilerTreeViewState = new UISystemProfilerTreeView.State()
      {
        profilerWindow = win
      };
      UISystemProfiler.Headers headers1 = new UISystemProfiler.Headers(this.m_MulticolumnHeaderState);
      headers1.canSort = true;
      headers1.height = 21f;
      UISystemProfiler.Headers headers2 = headers1;
      headers2.sortingChanged += (MultiColumnHeader.HeaderCallback) (header => this.m_TreeViewControl.Reload());
      this.m_TreeViewControl = new UISystemProfilerTreeView(this.m_UGUIProfilerTreeViewState, (MultiColumnHeader) headers2);
      this.m_TreeViewControl.Reload();
    }

    public void CurrentAreaChanged(ProfilerArea profilerArea)
    {
      if (profilerArea == ProfilerArea.UI || profilerArea == ProfilerArea.UIDetails)
        return;
      if ((bool) ((UnityEngine.Object) this.m_DetachedPreview))
      {
        this.m_DetachedPreview.Close();
        this.m_DetachedPreview = (UISystemPreviewWindow) null;
      }
      if (this.m_RenderService != null)
      {
        this.m_RenderService.Dispose();
        this.m_RenderService = (UISystemProfilerRenderService) null;
      }
    }

    internal class Headers : MultiColumnHeader
    {
      public Headers(MultiColumnHeaderState state)
        : base(state)
      {
      }

      protected override void ColumnHeaderGUI(MultiColumnHeaderState.Column column, Rect headerRect, int columnIndex)
      {
        GUIStyle styleWrapped = this.GetStyleWrapped(column.headerTextAlignment);
        float num = styleWrapped.CalcHeight(column.headerContent, headerRect.width);
        Rect position = headerRect;
        position.yMin += (float) ((double) position.height - (double) num - 1.0);
        GUI.Label(position, column.headerContent, styleWrapped);
        if (!this.canSort || !column.canSort)
          return;
        this.SortingButton(column, headerRect, columnIndex);
      }

      internal override void DrawDivider(Rect dividerRect, MultiColumnHeaderState.Column column)
      {
      }

      internal override Rect GetArrowRect(MultiColumnHeaderState.Column column, Rect headerRect)
      {
        return new Rect(headerRect.xMax - MultiColumnHeader.DefaultStyles.arrowStyle.fixedWidth, headerRect.y + 5f, MultiColumnHeader.DefaultStyles.arrowStyle.fixedWidth, headerRect.height - 10f);
      }

      private GUIStyle GetStyleWrapped(TextAlignment alignment)
      {
        switch (alignment)
        {
          case TextAlignment.Left:
            return UISystemProfiler.Styles.columnHeader;
          case TextAlignment.Center:
            return UISystemProfiler.Styles.columnHeaderCenterAligned;
          case TextAlignment.Right:
            return UISystemProfiler.Styles.columnHeaderRightAligned;
          default:
            return UISystemProfiler.Styles.columnHeader;
        }
      }
    }

    internal static class Styles
    {
      public static readonly GUIStyle entryOdd = (GUIStyle) "OL EntryBackOdd";
      public static readonly GUIStyle entryEven = (GUIStyle) "OL EntryBackEven";
      public static readonly GUIStyle rightHeader = (GUIStyle) "OL title TextRight";
      public static readonly GUIStyle columnHeader = (GUIStyle) "OL title";
      public static readonly GUIStyle columnHeaderCenterAligned = new GUIStyle(UISystemProfiler.Styles.columnHeader) { alignment = TextAnchor.MiddleCenter };
      public static readonly GUIStyle columnHeaderRightAligned = new GUIStyle(UISystemProfiler.Styles.columnHeader) { alignment = TextAnchor.MiddleRight };
      public static readonly GUIStyle background = (GUIStyle) "OL Box";
      public static readonly GUIStyle header = (GUIStyle) "OL title";
      internal const string PrefCheckerBoard = "UGUIProfiler.CheckerBoard";
      internal const string PrefOverdraw = "UGUIProfiler.Overdraw";
      public static readonly GUIContent noData;
      public static GUIContent[] backgroundOptions;
      public static int[] backgroundValues;
      public static GUIContent contentDetachRender;
      public static GUIContent[] rendermodeOptions;
      public static int[] rendermodeValues;
      private static readonly Color m_SeparatorColorPro;
      private static readonly Color m_SeparatorColorNonPro;

      static Styles()
      {
        UISystemProfiler.Styles.header.alignment = TextAnchor.MiddleLeft;
        UISystemProfiler.Styles.noData = EditorGUIUtility.TextContent("No frame data available - UI profiling is only available when profiling in the editor");
        UISystemProfiler.Styles.contentDetachRender = new GUIContent("Detach");
        UISystemProfiler.Styles.backgroundOptions = new GUIContent[3]
        {
          new GUIContent("Checkerboard"),
          new GUIContent("Black"),
          new GUIContent("White")
        };
        UISystemProfiler.Styles.backgroundValues = new int[3]
        {
          0,
          1,
          2
        };
        UISystemProfiler.Styles.rendermodeOptions = new GUIContent[3]
        {
          new GUIContent("Standard"),
          new GUIContent("Overdraw"),
          new GUIContent("Composite overdraw")
        };
        UISystemProfiler.Styles.rendermodeValues = new int[3]
        {
          0,
          1,
          2
        };
        UISystemProfiler.Styles.m_SeparatorColorPro = new Color(0.15f, 0.15f, 0.15f);
        UISystemProfiler.Styles.m_SeparatorColorNonPro = new Color(0.6f, 0.6f, 0.6f);
      }

      public static Color separatorColor
      {
        get
        {
          return !EditorGUIUtility.isProSkin ? UISystemProfiler.Styles.m_SeparatorColorNonPro : UISystemProfiler.Styles.m_SeparatorColorPro;
        }
      }

      internal enum RenderMode
      {
        Standard,
        Overdraw,
        CompositeOverdraw,
      }

      internal enum PreviewBackgroundType
      {
        Checkerboard,
        Black,
        White,
      }
    }
  }
}
