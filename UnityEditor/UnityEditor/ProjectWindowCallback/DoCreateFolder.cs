// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowCallback.DoCreateFolder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.IO;
using UnityEngine;

namespace UnityEditor.ProjectWindowCallback
{
  internal class DoCreateFolder : EndNameEditAction
  {
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
      ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(Path.GetDirectoryName(pathName), Path.GetFileName(pathName))), typeof (Object)));
    }
  }
}
