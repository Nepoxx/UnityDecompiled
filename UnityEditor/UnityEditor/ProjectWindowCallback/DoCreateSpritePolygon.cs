// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowCallback.DoCreateSpritePolygon
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.ProjectWindowCallback
{
  internal class DoCreateSpritePolygon : EndNameEditAction
  {
    public int sides;

    public override void Action(int instanceId, string pathName, string resourceFile)
    {
      bool flag = false;
      if (this.sides < 0)
      {
        this.sides = 5;
        flag = true;
      }
      UnityEditor.Sprites.SpriteUtility.CreateSpritePolygonAssetAtPath(pathName, this.sides);
      if (!flag)
        return;
      Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(pathName);
      SpriteEditorWindow.GetWindow();
    }
  }
}
