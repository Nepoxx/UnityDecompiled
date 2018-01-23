// Decompiled with JetBrains decompiler
// Type: UnityEngine.GridBrushBase
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  public abstract class GridBrushBase : ScriptableObject
  {
    public virtual void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
    }

    public virtual void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
    }

    public virtual void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
      for (int zMin = position.zMin; zMin < position.zMax; ++zMin)
      {
        for (int yMin = position.yMin; yMin < position.yMax; ++yMin)
        {
          for (int xMin = position.xMin; xMin < position.xMax; ++xMin)
            this.Paint(gridLayout, brushTarget, new Vector3Int(xMin, yMin, zMin));
        }
      }
    }

    public virtual void BoxErase(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
      for (int zMin = position.zMin; zMin < position.zMax; ++zMin)
      {
        for (int yMin = position.yMin; yMin < position.yMax; ++yMin)
        {
          for (int xMin = position.xMin; xMin < position.xMax; ++xMin)
            this.Erase(gridLayout, brushTarget, new Vector3Int(xMin, yMin, zMin));
        }
      }
    }

    public virtual void Select(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
    }

    public virtual void FloodFill(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
    }

    public virtual void Rotate(GridBrushBase.RotationDirection direction, GridLayout.CellLayout layout)
    {
    }

    public virtual void Flip(GridBrushBase.FlipAxis flip, GridLayout.CellLayout layout)
    {
    }

    public virtual void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pivot)
    {
    }

    public virtual void Move(GridLayout gridLayout, GameObject brushTarget, BoundsInt from, BoundsInt to)
    {
    }

    public virtual void MoveStart(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
    }

    public virtual void MoveEnd(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
    }

    public enum Tool
    {
      Select,
      Move,
      Paint,
      Box,
      Pick,
      Erase,
      FloodFill,
    }

    public enum RotationDirection
    {
      Clockwise,
      CounterClockwise,
    }

    public enum FlipAxis
    {
      X,
      Y,
    }
  }
}
