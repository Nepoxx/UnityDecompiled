// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpritePolygonModeModule
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.U2D.Interface;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor
{
  internal class SpritePolygonModeModule : SpriteFrameModuleBase, ISpriteEditorModule
  {
    private Rect m_PolygonChangeShapeWindowRect = new Rect(0.0f, 17f, 150f, 45f);
    private const int k_PolygonChangeShapeWindowMargin = 17;
    private const int k_PolygonChangeShapeWindowWidth = 150;
    private const int k_PolygonChangeShapeWindowHeight = 45;
    private const int k_PolygonChangeShapeWindowWarningHeight = 65;

    public SpritePolygonModeModule(ISpriteEditor sw, IEventSystem es, IUndoSystem us, IAssetDatabase ad)
      : base("Sprite Polygon Mode Editor", sw, es, us, ad)
    {
    }

    public override void OnModuleActivate()
    {
      base.OnModuleActivate();
      this.m_RectsCache = this.spriteEditor.spriteRects;
      this.showChangeShapeWindow = this.polygonSprite;
      if (!this.polygonSprite)
        return;
      this.DeterminePolygonSides();
    }

    public override void OnModuleDeactivate()
    {
      this.m_RectsCache = (ISpriteRectCache) null;
    }

    public override bool CanBeActivated()
    {
      return SpriteUtility.GetSpriteImportMode(this.spriteEditor.spriteEditorDataProvider) == SpriteImportMode.Polygon;
    }

    private bool polygonSprite
    {
      get
      {
        return this.spriteImportMode == SpriteImportMode.Polygon;
      }
    }

    private void DeterminePolygonSides()
    {
      if (this.polygonSprite && this.m_RectsCache.Count == 1)
      {
        SpriteRect spriteRect = this.m_RectsCache.RectAt(0);
        if (spriteRect.outline.Count != 1)
          return;
        this.polygonSides = spriteRect.outline[0].Count;
      }
      else
        this.polygonSides = 0;
    }

    public int GetPolygonSideCount()
    {
      this.DeterminePolygonSides();
      return this.polygonSides;
    }

    public int polygonSides { get; set; }

    public void GeneratePolygonOutline()
    {
      for (int i = 0; i < this.m_RectsCache.Count; ++i)
      {
        SpriteRect spriteRect = this.m_RectsCache.RectAt(i);
        SpriteOutline spriteOutline = new SpriteOutline();
        spriteOutline.AddRange((IEnumerable<Vector2>) UnityEditor.Sprites.SpriteUtility.GeneratePolygonOutlineVerticesOfSize(this.polygonSides, (int) spriteRect.rect.width, (int) spriteRect.rect.height));
        spriteRect.outline.Clear();
        spriteRect.outline.Add(spriteOutline);
        this.spriteEditor.SetDataModified();
      }
      this.Repaint();
    }

    public override void OnPostGUI()
    {
      this.DoPolygonChangeShapeWindow();
      base.OnPostGUI();
    }

    public override void DoTextureGUI()
    {
      base.DoTextureGUI();
      this.DrawGizmos();
      this.HandleGizmoMode();
      this.HandleBorderCornerScalingHandles();
      this.HandleBorderSidePointScalingSliders();
      this.HandleBorderSideScalingHandles();
      this.HandlePivotHandle();
      if (this.MouseOnTopOfInspector())
        return;
      this.spriteEditor.HandleSpriteSelection();
    }

    public override void DrawToolbarGUI(Rect toolbarRect)
    {
      using (new EditorGUI.DisabledScope(this.spriteEditor.editingDisabled))
      {
        GUIStyle toolbarPopup = EditorStyles.toolbarPopup;
        Rect drawRect = toolbarRect;
        drawRect.width = toolbarPopup.CalcSize(SpritePolygonModeModule.SpritePolygonModeStyles.changeShapeLabel).x;
        SpriteUtilityWindow.DrawToolBarWidget(ref drawRect, ref toolbarRect, (Action<Rect>) (adjustedDrawArea => this.showChangeShapeWindow = GUI.Toggle(adjustedDrawArea, this.showChangeShapeWindow, SpritePolygonModeModule.SpritePolygonModeStyles.changeShapeLabel, EditorStyles.toolbarButton)));
      }
    }

    private void DrawGizmos()
    {
      if (this.eventSystem.current.type != EventType.Repaint)
        return;
      for (int i = 0; i < this.spriteCount; ++i)
      {
        List<SpriteOutline> spriteOutlineAt = this.GetSpriteOutlineAt(i);
        Vector2 vector2 = this.GetSpriteRectAt(i).size * 0.5f;
        if (spriteOutlineAt.Count > 0)
        {
          SpriteEditorUtility.BeginLines(new Color(0.75f, 0.75f, 0.75f, 0.75f));
          for (int index1 = 0; index1 < spriteOutlineAt.Count; ++index1)
          {
            int index2 = 0;
            int index3 = spriteOutlineAt[index1].Count - 1;
            for (; index2 < spriteOutlineAt[index1].Count; ++index2)
            {
              SpriteEditorUtility.DrawLine((Vector3) (spriteOutlineAt[index1][index3] + vector2), (Vector3) (spriteOutlineAt[index1][index2] + vector2));
              index3 = index2;
            }
          }
          SpriteEditorUtility.EndLines();
        }
      }
      this.DrawSpriteRectGizmos();
    }

    private void DoPolygonChangeShapeWindow()
    {
      if (!this.showChangeShapeWindow || this.spriteEditor.editingDisabled)
        return;
      bool flag = false;
      float labelWidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = 45f;
      GUILayout.BeginArea(this.m_PolygonChangeShapeWindowRect);
      GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
      IEvent current = this.eventSystem.current;
      if (this.isSidesValid && current.type == EventType.KeyDown && current.keyCode == KeyCode.Return)
      {
        flag = true;
        current.Use();
      }
      EditorGUI.BeginChangeCheck();
      this.polygonSides = EditorGUILayout.IntField(SpritePolygonModeModule.SpritePolygonModeStyles.sidesLabel, this.polygonSides, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.m_PolygonChangeShapeWindowRect.height = !this.isSidesValid ? 65f : 45f;
      GUILayout.FlexibleSpace();
      if (!this.isSidesValid)
      {
        EditorGUILayout.HelpBox(SpritePolygonModeModule.SpritePolygonModeStyles.polygonChangeShapeHelpBoxContent.text, MessageType.Warning, true);
      }
      else
      {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUI.BeginDisabledGroup(!this.isSidesValid);
        if (GUILayout.Button(SpritePolygonModeModule.SpritePolygonModeStyles.changeButtonLabel))
          flag = true;
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();
      }
      GUILayout.EndVertical();
      if (flag)
      {
        if (this.isSidesValid)
          this.GeneratePolygonOutline();
        this.showChangeShapeWindow = false;
        GUIUtility.hotControl = 0;
        GUIUtility.keyboardControl = 0;
      }
      EditorGUIUtility.labelWidth = labelWidth;
      GUILayout.EndArea();
    }

    private bool isSidesValid
    {
      get
      {
        return this.polygonSides == 0 || this.polygonSides >= 3 && this.polygonSides <= 128;
      }
    }

    public bool showChangeShapeWindow { get; set; }

    private static class SpritePolygonModeStyles
    {
      public static readonly GUIContent changeShapeLabel = EditorGUIUtility.TextContent("Change Shape");
      public static readonly GUIContent sidesLabel = EditorGUIUtility.TextContent("Sides");
      public static readonly GUIContent polygonChangeShapeHelpBoxContent = EditorGUIUtility.TextContent("Sides can only be either 0 or anything between 3 and 128");
      public static readonly GUIContent changeButtonLabel = EditorGUIUtility.TextContent("Change|Change to the new number of sides");
    }
  }
}
