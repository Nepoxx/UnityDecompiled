// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarPreviewSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class AvatarPreviewSelection : ScriptableSingleton<AvatarPreviewSelection>
  {
    [SerializeField]
    private GameObject[] m_PreviewModels;

    private void Awake()
    {
      int length = 4;
      if (this.m_PreviewModels != null && this.m_PreviewModels.Length == length)
        return;
      this.m_PreviewModels = new GameObject[length];
    }

    public static void SetPreview(ModelImporterAnimationType type, GameObject go)
    {
      if (!Enum.IsDefined(typeof (ModelImporterAnimationType), (object) type) || !((UnityEngine.Object) ScriptableSingleton<AvatarPreviewSelection>.instance.m_PreviewModels[(int) type] != (UnityEngine.Object) go))
        return;
      ScriptableSingleton<AvatarPreviewSelection>.instance.m_PreviewModels[(int) type] = go;
      ScriptableSingleton<AvatarPreviewSelection>.instance.Save(false);
    }

    public static GameObject GetPreview(ModelImporterAnimationType type)
    {
      if (!Enum.IsDefined(typeof (ModelImporterAnimationType), (object) type))
        return (GameObject) null;
      return ScriptableSingleton<AvatarPreviewSelection>.instance.m_PreviewModels[(int) type];
    }
  }
}
