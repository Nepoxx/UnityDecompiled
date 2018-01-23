// Decompiled with JetBrains decompiler
// Type: UnityEngine.ContactFilter2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Serializable]
  public struct ContactFilter2D
  {
    public bool useTriggers;
    public bool useLayerMask;
    public bool useDepth;
    public bool useOutsideDepth;
    public bool useNormalAngle;
    public bool useOutsideNormalAngle;
    public LayerMask layerMask;
    public float minDepth;
    public float maxDepth;
    public float minNormalAngle;
    public float maxNormalAngle;
    public const float NormalAngleUpperLimit = 359.9999f;

    public ContactFilter2D NoFilter()
    {
      this.useTriggers = true;
      this.useLayerMask = false;
      this.layerMask = (LayerMask) -1;
      this.useDepth = false;
      this.useOutsideDepth = false;
      this.minDepth = float.NegativeInfinity;
      this.maxDepth = float.PositiveInfinity;
      this.useNormalAngle = false;
      this.useOutsideNormalAngle = false;
      this.minNormalAngle = 0.0f;
      this.maxNormalAngle = 359.9999f;
      return this;
    }

    private void CheckConsistency()
    {
      this.minDepth = (double) this.minDepth == double.NegativeInfinity || (double) this.minDepth == double.PositiveInfinity || float.IsNaN(this.minDepth) ? float.MinValue : this.minDepth;
      this.maxDepth = (double) this.maxDepth == double.NegativeInfinity || (double) this.maxDepth == double.PositiveInfinity || float.IsNaN(this.maxDepth) ? float.MaxValue : this.maxDepth;
      if ((double) this.minDepth > (double) this.maxDepth)
      {
        float minDepth = this.minDepth;
        this.minDepth = this.maxDepth;
        this.maxDepth = minDepth;
      }
      this.minNormalAngle = !float.IsNaN(this.minNormalAngle) ? Mathf.Clamp(this.minNormalAngle, 0.0f, 359.9999f) : 0.0f;
      this.maxNormalAngle = !float.IsNaN(this.maxNormalAngle) ? Mathf.Clamp(this.maxNormalAngle, 0.0f, 359.9999f) : 359.9999f;
      if ((double) this.minNormalAngle <= (double) this.maxNormalAngle)
        return;
      float minNormalAngle = this.minNormalAngle;
      this.minNormalAngle = this.maxNormalAngle;
      this.maxNormalAngle = minNormalAngle;
    }

    public void ClearLayerMask()
    {
      this.useLayerMask = false;
    }

    public void SetLayerMask(LayerMask layerMask)
    {
      this.layerMask = layerMask;
      this.useLayerMask = true;
    }

    public void ClearDepth()
    {
      this.useDepth = false;
    }

    public void SetDepth(float minDepth, float maxDepth)
    {
      this.minDepth = minDepth;
      this.maxDepth = maxDepth;
      this.useDepth = true;
      this.CheckConsistency();
    }

    public void ClearNormalAngle()
    {
      this.useNormalAngle = false;
    }

    public void SetNormalAngle(float minNormalAngle, float maxNormalAngle)
    {
      this.minNormalAngle = minNormalAngle;
      this.maxNormalAngle = maxNormalAngle;
      this.useNormalAngle = true;
      this.CheckConsistency();
    }

    public bool isFiltering
    {
      get
      {
        return !this.useTriggers || this.useLayerMask || this.useDepth || this.useNormalAngle;
      }
    }

    public bool IsFilteringTrigger([Writable] Collider2D collider)
    {
      return !this.useTriggers && collider.isTrigger;
    }

    public bool IsFilteringLayerMask(GameObject obj)
    {
      return this.useLayerMask && ((int) this.layerMask & 1 << obj.layer) == 0;
    }

    public bool IsFilteringDepth(GameObject obj)
    {
      if (!this.useDepth)
        return false;
      if ((double) this.minDepth > (double) this.maxDepth)
      {
        float minDepth = this.minDepth;
        this.minDepth = this.maxDepth;
        this.maxDepth = minDepth;
      }
      float z = obj.transform.position.z;
      bool flag = (double) z < (double) this.minDepth || (double) z > (double) this.maxDepth;
      if (this.useOutsideDepth)
        return !flag;
      return flag;
    }

    public bool IsFilteringNormalAngle(Vector2 normal)
    {
      return this.IsFilteringNormalAngle(Mathf.Atan2(normal.y, normal.x) * 57.29578f);
    }

    public bool IsFilteringNormalAngle(float angle)
    {
      angle -= Mathf.Floor(angle / 359.9999f) * 359.9999f;
      float num1 = Mathf.Clamp(this.minNormalAngle, 0.0f, 359.9999f);
      float num2 = Mathf.Clamp(this.maxNormalAngle, 0.0f, 359.9999f);
      if ((double) num1 > (double) num2)
      {
        float num3 = num1;
        num1 = num2;
        num2 = num3;
      }
      bool flag = (double) angle < (double) num1 || (double) angle > (double) num2;
      if (this.useOutsideNormalAngle)
        return !flag;
      return flag;
    }

    internal static ContactFilter2D CreateLegacyFilter(int layerMask, float minDepth, float maxDepth)
    {
      ContactFilter2D contactFilter2D = new ContactFilter2D();
      contactFilter2D.useTriggers = Physics2D.queriesHitTriggers;
      contactFilter2D.SetLayerMask((LayerMask) layerMask);
      contactFilter2D.SetDepth(minDepth, maxDepth);
      return contactFilter2D;
    }
  }
}
