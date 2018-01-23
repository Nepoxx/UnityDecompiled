// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIAspectSizer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  internal sealed class GUIAspectSizer : GUILayoutEntry
  {
    private float aspect;

    public GUIAspectSizer(float aspect, GUILayoutOption[] options)
      : base(0.0f, 0.0f, 0.0f, 0.0f, GUIStyle.none)
    {
      this.aspect = aspect;
      this.ApplyOptions(options);
    }

    public override void CalcHeight()
    {
      this.minHeight = this.maxHeight = this.rect.width / this.aspect;
    }
  }
}
