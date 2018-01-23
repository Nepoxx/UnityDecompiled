// Decompiled with JetBrains decompiler
// Type: UnityEditor.TimeHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  internal struct TimeHelper
  {
    public float deltaTime;
    private long lastTime;

    public void Begin()
    {
      this.lastTime = DateTime.Now.Ticks;
    }

    public float Update()
    {
      this.deltaTime = (float) (DateTime.Now.Ticks - this.lastTime) / 1E+07f;
      this.lastTime = DateTime.Now.Ticks;
      return this.deltaTime;
    }
  }
}
