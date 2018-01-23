// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.DrawRendererSortSettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.Rendering
{
  public struct DrawRendererSortSettings
  {
    public Matrix4x4 worldToCameraMatrix;
    public Vector3 cameraPosition;
    public SortFlags flags;
    private int _sortOrthographic;
    private Matrix4x4 _previousVPMatrix;
    private Matrix4x4 _nonJitteredVPMatrix;

    public bool sortOrthographic
    {
      get
      {
        return this._sortOrthographic != 0;
      }
      set
      {
        this._sortOrthographic = !value ? 0 : 1;
      }
    }
  }
}
