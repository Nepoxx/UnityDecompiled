// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightingStats
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  internal struct LightingStats
  {
    public uint realtimeLightsCount;
    public uint dynamicMeshesCount;
    public uint mixedLightsCount;
    public uint bakedLightsCount;
    public uint staticMeshesCount;
    public uint staticMeshesRealtimeEmissive;
    public uint staticMeshesBakedEmissive;
    public uint lightProbeGroupsCount;
    public uint reflectionProbesCount;

    internal void Reset()
    {
      this.realtimeLightsCount = 0U;
      this.dynamicMeshesCount = 0U;
      this.mixedLightsCount = 0U;
      this.bakedLightsCount = 0U;
      this.staticMeshesCount = 0U;
      this.staticMeshesRealtimeEmissive = 0U;
      this.staticMeshesBakedEmissive = 0U;
      this.lightProbeGroupsCount = 0U;
      this.reflectionProbesCount = 0U;
    }
  }
}
