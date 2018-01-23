// Decompiled with JetBrains decompiler
// Type: UnityEditor.TargetGlesGraphics
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  [Obsolete("TargetGlesGraphics is ignored, use SetGraphicsAPIs/GetGraphicsAPIs APIs", false)]
  public enum TargetGlesGraphics
  {
    Automatic = -1,
    OpenGLES_1_x = 0,
    OpenGLES_2_0 = 1,
    OpenGLES_3_0 = 2,
  }
}
