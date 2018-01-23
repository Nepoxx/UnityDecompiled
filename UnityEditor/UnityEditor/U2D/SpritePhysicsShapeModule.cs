// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.SpritePhysicsShapeModule
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.U2D.Interface;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor.U2D
{
  internal class SpritePhysicsShapeModule : SpriteOutlineModule
  {
    private readonly float kDefaultPhysicsTessellationDetail = 0.25f;
    private readonly byte kDefaultPhysicsAlphaTolerance = 200;

    public SpritePhysicsShapeModule(ISpriteEditor sem, IEventSystem ege, IUndoSystem us, IAssetDatabase ad, IGUIUtility gu, IShapeEditorFactory sef, ITexture2D outlineTexture)
      : base(sem, ege, us, ad, gu, sef, outlineTexture)
    {
      this.spriteEditorWindow = sem;
    }

    public override string moduleName
    {
      get
      {
        return "Edit Physics Shape";
      }
    }

    private ISpriteEditor spriteEditorWindow { get; set; }

    protected override List<SpriteOutline> selectedShapeOutline
    {
      get
      {
        return this.m_Selected.physicsShape;
      }
      set
      {
        this.m_Selected.physicsShape = value;
      }
    }

    protected override bool HasShapeOutline(SpriteRect spriteRect)
    {
      return spriteRect.physicsShape != null && spriteRect.physicsShape.Count > 0;
    }

    protected override void SetupShapeEditorOutline(SpriteRect spriteRect)
    {
      if (spriteRect.physicsShape != null && spriteRect.physicsShape.Count != 0)
        return;
      spriteRect.physicsShape = SpriteOutlineModule.GenerateSpriteRectOutline(spriteRect.rect, this.spriteEditorWindow.selectedTexture, (double) Math.Abs(spriteRect.tessellationDetail - -1f) >= (double) Mathf.Epsilon ? spriteRect.tessellationDetail : this.kDefaultPhysicsTessellationDetail, this.kDefaultPhysicsAlphaTolerance, this.spriteEditorWindow.spriteEditorDataProvider);
      this.spriteEditorWindow.SetDataModified();
    }
  }
}
