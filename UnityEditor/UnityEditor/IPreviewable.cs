// Decompiled with JetBrains decompiler
// Type: UnityEditor.IPreviewable
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal interface IPreviewable
  {
    void Initialize(Object[] targets);

    Object target { get; }

    bool MoveNextTarget();

    void ResetTarget();

    bool HasPreviewGUI();

    GUIContent GetPreviewTitle();

    void DrawPreview(Rect previewArea);

    void OnPreviewGUI(Rect r, GUIStyle background);

    void OnInteractivePreviewGUI(Rect r, GUIStyle background);

    void OnPreviewSettings();

    string GetInfoString();

    void ReloadPreviewInstances();
  }
}
