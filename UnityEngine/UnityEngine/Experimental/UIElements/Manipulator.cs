// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.Manipulator
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  public abstract class Manipulator : IManipulator
  {
    private VisualElement m_Target;

    protected abstract void RegisterCallbacksOnTarget();

    protected abstract void UnregisterCallbacksFromTarget();

    public VisualElement target
    {
      get
      {
        return this.m_Target;
      }
      set
      {
        if (this.target != null)
          this.UnregisterCallbacksFromTarget();
        this.m_Target = value;
        if (this.target == null)
          return;
        this.RegisterCallbacksOnTarget();
      }
    }
  }
}
