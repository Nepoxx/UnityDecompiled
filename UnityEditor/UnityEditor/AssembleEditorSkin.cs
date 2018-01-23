// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssembleEditorSkin
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditorInternal;

namespace UnityEditor
{
  internal class AssembleEditorSkin : EditorWindow
  {
    public static void DoIt()
    {
      EditorApplication.ExecuteMenuItem("Tools/Regenerate Editor Skins Now");
    }

    private static void RegenerateAllIconsWithMipLevels()
    {
      GenerateIconsWithMipLevels.GenerateAllIconsWithMipLevels();
    }

    private static void RegenerateSelectedIconsWithMipLevels()
    {
      GenerateIconsWithMipLevels.GenerateSelectedIconsWithMips();
    }
  }
}
