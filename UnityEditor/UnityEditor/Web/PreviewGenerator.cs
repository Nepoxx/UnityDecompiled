// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.PreviewGenerator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Web
{
  internal class PreviewGenerator
  {
    protected static PreviewGenerator s_Instance = (PreviewGenerator) null;
    private const string kPreviewBuildFolder = "builds";

    public static PreviewGenerator GetInstance()
    {
      if (PreviewGenerator.s_Instance == null)
        return new PreviewGenerator();
      return PreviewGenerator.s_Instance;
    }

    public byte[] GeneratePreview(string assetPath, int width, int height)
    {
      Object targetObject = AssetDatabase.LoadMainAssetAtPath(assetPath);
      if (targetObject == (Object) null)
        return (byte[]) null;
      Editor editor = Editor.CreateEditor(targetObject);
      if ((Object) editor == (Object) null)
        return (byte[]) null;
      Texture2D tex = editor.RenderStaticPreview(assetPath, (Object[]) null, width, height);
      if ((Object) tex == (Object) null)
      {
        Object.DestroyImmediate((Object) editor);
        return (byte[]) null;
      }
      byte[] png = tex.EncodeToPNG();
      Object.DestroyImmediate((Object) tex);
      Object.DestroyImmediate((Object) editor);
      return png;
    }
  }
}
