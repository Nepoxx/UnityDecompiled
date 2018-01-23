// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildPlayerSceneTreeViewItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;

namespace UnityEditor
{
  internal class BuildPlayerSceneTreeViewItem : TreeViewItem
  {
    public static int kInvalidCounter = -1;
    private const string kAssetsFolder = "Assets/";
    private const string kSceneExtension = ".unity";
    public bool active;
    public int counter;
    public string fullName;
    public GUID guid;

    public BuildPlayerSceneTreeViewItem(int id, int depth, string path, bool state)
      : base(id, depth)
    {
      this.active = state;
      this.counter = BuildPlayerSceneTreeViewItem.kInvalidCounter;
      this.guid = new GUID(AssetDatabase.AssetPathToGUID(path));
      this.fullName = "";
      this.displayName = path;
      this.UpdateName();
    }

    public void UpdateName()
    {
      string assetPath = AssetDatabase.GUIDToAssetPath(this.guid.ToString());
      if (!(assetPath != this.fullName))
        return;
      this.fullName = assetPath;
      this.displayName = this.fullName;
      if (this.displayName.StartsWith("Assets/"))
        this.displayName = this.displayName.Remove(0, "Assets/".Length);
      int length = this.displayName.LastIndexOf(".unity");
      if (length > 0)
        this.displayName = this.displayName.Substring(0, length);
    }
  }
}
