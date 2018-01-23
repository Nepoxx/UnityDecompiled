// Decompiled with JetBrains decompiler
// Type: UnityEditor.TickStyle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class TickStyle
  {
    public EditorGUIUtility.SkinnedColor tickColor = new EditorGUIUtility.SkinnedColor(new Color(0.0f, 0.0f, 0.0f, 0.2f), new Color(0.45f, 0.45f, 0.45f, 0.2f));
    public EditorGUIUtility.SkinnedColor labelColor = new EditorGUIUtility.SkinnedColor(new Color(0.0f, 0.0f, 0.0f, 0.32f), new Color(0.8f, 0.8f, 0.8f, 0.32f));
    public int distMin = 10;
    public int distFull = 80;
    public int distLabel = 50;
    public bool stubs = false;
    public bool centerLabel = false;
    public string unit = "";
  }
}
