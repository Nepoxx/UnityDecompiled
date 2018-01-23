// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetPreviewUpdater
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
  internal static class AssetPreviewUpdater
  {
    public static Texture2D CreatePreviewForAsset(UnityEngine.Object obj, UnityEngine.Object[] subAssets, string assetPath)
    {
      if (obj == (UnityEngine.Object) null)
        return (Texture2D) null;
      System.Type customEditorType = CustomEditorAttributes.FindCustomEditorType(obj, false);
      if (customEditorType == null)
        return (Texture2D) null;
      MethodInfo method = customEditorType.GetMethod("RenderStaticPreview");
      if (method == null)
      {
        Debug.LogError((object) "Fail to find RenderStaticPreview base method");
        return (Texture2D) null;
      }
      if (method.DeclaringType == typeof (Editor))
        return (Texture2D) null;
      Editor editor = Editor.CreateEditor(obj);
      if ((UnityEngine.Object) editor == (UnityEngine.Object) null)
        return (Texture2D) null;
      Texture2D texture2D = editor.RenderStaticPreview(assetPath, subAssets, 128, 128);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) editor);
      return texture2D;
    }
  }
}
