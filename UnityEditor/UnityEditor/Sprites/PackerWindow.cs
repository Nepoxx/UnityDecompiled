// Decompiled with JetBrains decompiler
// Type: UnityEditor.Sprites.PackerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor.Sprites
{
  internal class PackerWindow : SpriteUtilityWindow
  {
    private static string[] s_AtlasNamesEmpty = new string[1]{ "Sprite atlas cache is empty" };
    private static string[] s_PageNamesEmpty = new string[0];
    private string[] m_AtlasNames = PackerWindow.s_AtlasNamesEmpty;
    private int m_SelectedAtlas = 0;
    private string[] m_PageNames = PackerWindow.s_PageNamesEmpty;
    private int m_SelectedPage = 0;
    private Sprite m_SelectedSprite = (Sprite) null;

    private void OnEnable()
    {
      this.minSize = new Vector2(400f, 256f);
      this.titleContent = PackerWindow.PackerWindowStyle.windowTitle;
      this.Reset();
    }

    private void Reset()
    {
      this.RefreshAtlasNameList();
      this.RefreshAtlasPageList();
      this.m_SelectedAtlas = 0;
      this.m_SelectedPage = 0;
      this.m_SelectedSprite = (Sprite) null;
    }

    private void RefreshAtlasNameList()
    {
      this.m_AtlasNames = Packer.atlasNames;
      if (this.m_SelectedAtlas < this.m_AtlasNames.Length)
        return;
      this.m_SelectedAtlas = 0;
    }

    private void RefreshAtlasPageList()
    {
      if (this.m_AtlasNames.Length > 0)
      {
        UnityEngine.Texture2D[] texturesForAtlas = Packer.GetTexturesForAtlas(this.m_AtlasNames[this.m_SelectedAtlas]);
        this.m_PageNames = new string[texturesForAtlas.Length];
        for (int index = 0; index < texturesForAtlas.Length; ++index)
          this.m_PageNames[index] = string.Format(PackerWindow.PackerWindowStyle.pageContentLabel.text, (object) (index + 1));
      }
      else
        this.m_PageNames = PackerWindow.s_PageNamesEmpty;
      if (this.m_SelectedPage < this.m_PageNames.Length)
        return;
      this.m_SelectedPage = 0;
    }

    private void OnAtlasNameListChanged()
    {
      if (this.m_AtlasNames.Length > 0)
      {
        string[] atlasNames = Packer.atlasNames;
        if (this.m_AtlasNames[this.m_SelectedAtlas].Equals(atlasNames.Length > this.m_SelectedAtlas ? atlasNames[this.m_SelectedAtlas] : (string) null))
        {
          this.RefreshAtlasNameList();
          this.RefreshAtlasPageList();
          this.m_SelectedSprite = (Sprite) null;
          return;
        }
      }
      this.Reset();
    }

    private bool ValidateIsPackingEnabled()
    {
      if (EditorSettings.spritePackerMode == SpritePackerMode.BuildTimeOnly || EditorSettings.spritePackerMode == SpritePackerMode.AlwaysOn)
        return true;
      EditorGUILayout.BeginVertical();
      GUILayout.Label(PackerWindow.PackerWindowStyle.packingDisabledLabel);
      if (GUILayout.Button(PackerWindow.PackerWindowStyle.openProjectSettingButton))
        EditorApplication.ExecuteMenuItem("Edit/Project Settings/Editor");
      EditorGUILayout.EndVertical();
      return false;
    }

    private Rect DoToolbarGUI()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PackerWindow.\u003CDoToolbarGUI\u003Ec__AnonStorey0 toolbarGuiCAnonStorey0 = new PackerWindow.\u003CDoToolbarGUI\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      toolbarGuiCAnonStorey0.\u0024this = this;
      Rect rect = new Rect(0.0f, 0.0f, this.position.width, 17f);
      if (UnityEngine.Event.current.type == EventType.Repaint)
        EditorStyles.toolbar.Draw(rect, false, false, false, false);
      bool enabled = GUI.enabled;
      GUI.enabled = this.m_AtlasNames.Length > 0;
      Rect toolbarRect = this.DoAlphaZoomToolbarGUI(rect);
      GUI.enabled = enabled;
      Rect drawRect = new Rect(5f, 0.0f, 0.0f, 17f);
      toolbarRect.width -= drawRect.x;
      using (new EditorGUI.DisabledScope(Application.isPlaying))
      {
        drawRect.width = EditorStyles.toolbarButton.CalcSize(PackerWindow.PackerWindowStyle.packLabel).x;
        // ISSUE: reference to a compiler-generated method
        SpriteUtilityWindow.DrawToolBarWidget(ref drawRect, ref toolbarRect, new Action<Rect>(toolbarGuiCAnonStorey0.\u003C\u003Em__0));
        using (new EditorGUI.DisabledScope(Packer.SelectedPolicy == Packer.kDefaultPolicy))
        {
          drawRect.x += drawRect.width;
          drawRect.width = EditorStyles.toolbarButton.CalcSize(PackerWindow.PackerWindowStyle.repackLabel).x;
          // ISSUE: reference to a compiler-generated method
          SpriteUtilityWindow.DrawToolBarWidget(ref drawRect, ref toolbarRect, new Action<Rect>(toolbarGuiCAnonStorey0.\u003C\u003Em__1));
        }
      }
      float x = GUI.skin.label.CalcSize(PackerWindow.PackerWindowStyle.viewAtlasLabel).x;
      float num = (float) ((double) x + 100.0 + 70.0 + 100.0);
      drawRect.x += 5f;
      toolbarRect.width -= 5f;
      float width = toolbarRect.width;
      using (new EditorGUI.DisabledScope(this.m_AtlasNames.Length == 0))
      {
        drawRect.x += drawRect.width;
        drawRect.width = x / num * width;
        SpriteUtilityWindow.DrawToolBarWidget(ref drawRect, ref toolbarRect, (Action<Rect>) (adjustedDrawArea => GUI.Label(adjustedDrawArea, PackerWindow.PackerWindowStyle.viewAtlasLabel)));
        EditorGUI.BeginChangeCheck();
        drawRect.x += drawRect.width;
        drawRect.width = 100f / num * width;
        // ISSUE: reference to a compiler-generated method
        SpriteUtilityWindow.DrawToolBarWidget(ref drawRect, ref toolbarRect, new Action<Rect>(toolbarGuiCAnonStorey0.\u003C\u003Em__2));
        if (EditorGUI.EndChangeCheck())
        {
          this.RefreshAtlasPageList();
          this.m_SelectedSprite = (Sprite) null;
        }
        EditorGUI.BeginChangeCheck();
        drawRect.x += drawRect.width;
        drawRect.width = 70f / num * width;
        // ISSUE: reference to a compiler-generated method
        SpriteUtilityWindow.DrawToolBarWidget(ref drawRect, ref toolbarRect, new Action<Rect>(toolbarGuiCAnonStorey0.\u003C\u003Em__3));
        if (EditorGUI.EndChangeCheck())
          this.m_SelectedSprite = (Sprite) null;
      }
      EditorGUI.BeginChangeCheck();
      // ISSUE: reference to a compiler-generated field
      toolbarGuiCAnonStorey0.policies = Packer.Policies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      toolbarGuiCAnonStorey0.selectedPolicy = Array.IndexOf<string>(toolbarGuiCAnonStorey0.policies, Packer.SelectedPolicy);
      drawRect.x += drawRect.width;
      drawRect.width = 100f / num * width;
      // ISSUE: reference to a compiler-generated method
      SpriteUtilityWindow.DrawToolBarWidget(ref drawRect, ref toolbarRect, new Action<Rect>(toolbarGuiCAnonStorey0.\u003C\u003Em__4));
      if (EditorGUI.EndChangeCheck())
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Packer.SelectedPolicy = toolbarGuiCAnonStorey0.policies[toolbarGuiCAnonStorey0.selectedPolicy];
      }
      return toolbarRect;
    }

    private void OnSelectionChange()
    {
      if (Selection.activeObject == (UnityEngine.Object) null)
        return;
      Sprite activeObject = Selection.activeObject as Sprite;
      if (!((UnityEngine.Object) activeObject != (UnityEngine.Object) this.m_SelectedSprite))
        return;
      if ((UnityEngine.Object) activeObject != (UnityEngine.Object) null)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PackerWindow.\u003COnSelectionChange\u003Ec__AnonStorey1 changeCAnonStorey1 = new PackerWindow.\u003COnSelectionChange\u003Ec__AnonStorey1();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Packer.GetAtlasDataForSprite(activeObject, out changeCAnonStorey1.selAtlasName, out changeCAnonStorey1.selAtlasTexture);
        // ISSUE: reference to a compiler-generated method
        int index1 = ((IEnumerable<string>) this.m_AtlasNames).ToList<string>().FindIndex(new Predicate<string>(changeCAnonStorey1.\u003C\u003Em__0));
        if (index1 == -1)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        int index2 = ((IEnumerable<UnityEngine.Texture2D>) Packer.GetTexturesForAtlas(changeCAnonStorey1.selAtlasName)).ToList<UnityEngine.Texture2D>().FindIndex(new Predicate<UnityEngine.Texture2D>(changeCAnonStorey1.\u003C\u003Em__1));
        if (index2 == -1)
          return;
        this.m_SelectedAtlas = index1;
        this.m_SelectedPage = index2;
        this.RefreshAtlasPageList();
      }
      this.m_SelectedSprite = activeObject;
      this.Repaint();
    }

    private void RefreshState()
    {
      string[] atlasNames = Packer.atlasNames;
      if (!((IEnumerable<string>) atlasNames).SequenceEqual<string>((IEnumerable<string>) this.m_AtlasNames))
      {
        if (atlasNames.Length == 0)
        {
          this.Reset();
          return;
        }
        this.OnAtlasNameListChanged();
      }
      if (this.m_AtlasNames.Length == 0)
      {
        this.SetNewTexture((UnityEngine.Texture2D) null);
      }
      else
      {
        if (this.m_SelectedAtlas >= this.m_AtlasNames.Length)
          this.m_SelectedAtlas = 0;
        string atlasName = this.m_AtlasNames[this.m_SelectedAtlas];
        UnityEngine.Texture2D[] texturesForAtlas1 = Packer.GetTexturesForAtlas(atlasName);
        if (this.m_SelectedPage >= texturesForAtlas1.Length)
          this.m_SelectedPage = 0;
        this.SetNewTexture(texturesForAtlas1[this.m_SelectedPage]);
        UnityEngine.Texture2D[] texturesForAtlas2 = Packer.GetAlphaTexturesForAtlas(atlasName);
        this.SetAlphaTextureOverride(this.m_SelectedPage >= texturesForAtlas2.Length ? (UnityEngine.Texture2D) null : texturesForAtlas2[this.m_SelectedPage]);
      }
    }

    public void OnGUI()
    {
      if (!this.ValidateIsPackingEnabled())
        return;
      Matrix4x4 matrix = Handles.matrix;
      this.InitStyles();
      this.RefreshState();
      Rect rect = this.DoToolbarGUI();
      if (this.m_Texture == (ITexture2D) null)
        return;
      EditorGUILayout.BeginHorizontal();
      this.m_TextureViewRect = new Rect(0.0f, rect.yMax, this.position.width - 16f, this.position.height - 16f - rect.height);
      GUILayout.FlexibleSpace();
      this.DoTextureGUI();
      EditorGUI.DropShadowLabel(new Rect(this.m_TextureViewRect.x, this.m_TextureViewRect.y + 10f, this.m_TextureViewRect.width, 20f), string.Format("{1}x{2}, {0}", (object) TextureUtil.GetTextureFormatString(this.m_Texture.format), (object) this.m_Texture.width, (object) this.m_Texture.height));
      EditorGUILayout.EndHorizontal();
      Handles.matrix = matrix;
    }

    private void DrawLineUtility(Vector2 from, Vector2 to)
    {
      SpriteEditorUtility.DrawLine(new Vector3((float) ((double) from.x * (double) this.m_Texture.width + 1.0 / (double) this.m_Zoom), (float) ((double) from.y * (double) this.m_Texture.height + 1.0 / (double) this.m_Zoom), 0.0f), new Vector3((float) ((double) to.x * (double) this.m_Texture.width + 1.0 / (double) this.m_Zoom), (float) ((double) to.y * (double) this.m_Texture.height + 1.0 / (double) this.m_Zoom), 0.0f));
    }

    private PackerWindow.Edge[] FindUniqueEdges(ushort[] indices)
    {
      PackerWindow.Edge[] edgeArray = new PackerWindow.Edge[indices.Length];
      int num = indices.Length / 3;
      for (int index = 0; index < num; ++index)
      {
        edgeArray[index * 3] = new PackerWindow.Edge(indices[index * 3], indices[index * 3 + 1]);
        edgeArray[index * 3 + 1] = new PackerWindow.Edge(indices[index * 3 + 1], indices[index * 3 + 2]);
        edgeArray[index * 3 + 2] = new PackerWindow.Edge(indices[index * 3 + 2], indices[index * 3]);
      }
      return ((IEnumerable<PackerWindow.Edge>) edgeArray).GroupBy<PackerWindow.Edge, PackerWindow.Edge>((Func<PackerWindow.Edge, PackerWindow.Edge>) (x => x)).Where<IGrouping<PackerWindow.Edge, PackerWindow.Edge>>((Func<IGrouping<PackerWindow.Edge, PackerWindow.Edge>, bool>) (x => x.Count<PackerWindow.Edge>() == 1)).Select<IGrouping<PackerWindow.Edge, PackerWindow.Edge>, PackerWindow.Edge>((Func<IGrouping<PackerWindow.Edge, PackerWindow.Edge>, PackerWindow.Edge>) (x => x.First<PackerWindow.Edge>())).ToArray<PackerWindow.Edge>();
    }

    protected override void DrawGizmos()
    {
      if (!((UnityEngine.Object) this.m_SelectedSprite != (UnityEngine.Object) null) || !(this.m_Texture != (ITexture2D) null))
        return;
      Vector2[] spriteUvs = SpriteUtility.GetSpriteUVs(this.m_SelectedSprite, true);
      PackerWindow.Edge[] uniqueEdges = this.FindUniqueEdges(this.m_SelectedSprite.triangles);
      SpriteEditorUtility.BeginLines(new Color(0.3921f, 0.5843f, 0.9294f, 0.75f));
      foreach (PackerWindow.Edge edge in uniqueEdges)
        this.DrawLineUtility(spriteUvs[(int) edge.v0], spriteUvs[(int) edge.v1]);
      SpriteEditorUtility.EndLines();
    }

    private class PackerWindowStyle
    {
      public static readonly GUIContent packLabel = EditorGUIUtility.TextContent("Pack");
      public static readonly GUIContent repackLabel = EditorGUIUtility.TextContent("Repack");
      public static readonly GUIContent viewAtlasLabel = EditorGUIUtility.TextContent("View Atlas:");
      public static readonly GUIContent windowTitle = EditorGUIUtility.TextContent("Sprite Packer");
      public static readonly GUIContent pageContentLabel = EditorGUIUtility.TextContent("Page {0}");
      public static readonly GUIContent packingDisabledLabel = EditorGUIUtility.TextContent("Legacy sprite packing is disabled. Enable it in Edit > Project Settings > Editor.");
      public static readonly GUIContent openProjectSettingButton = EditorGUIUtility.TextContent("Open Project Editor Settings");
    }

    private struct Edge
    {
      public ushort v0;
      public ushort v1;

      public Edge(ushort a, ushort b)
      {
        this.v0 = a;
        this.v1 = b;
      }

      public override bool Equals(object obj)
      {
        PackerWindow.Edge edge = (PackerWindow.Edge) obj;
        return (int) this.v0 == (int) edge.v0 && (int) this.v1 == (int) edge.v1 || (int) this.v0 == (int) edge.v1 && (int) this.v1 == (int) edge.v0;
      }

      public override int GetHashCode()
      {
        return ((int) this.v0 << 16 | (int) this.v1) ^ ((int) this.v1 << 16 | (int) this.v0).GetHashCode();
      }
    }
  }
}
