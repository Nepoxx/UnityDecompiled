// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.UxmlFactory`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>Base class for all user-defined UXML Element factories.</para>
  /// </summary>
  public abstract class UxmlFactory<T> : IUxmlFactory where T : VisualElement
  {
    public System.Type CreatesType
    {
      get
      {
        return typeof (T);
      }
    }

    public VisualElement Create(IUxmlAttributes bag, CreationContext cc)
    {
      return (VisualElement) this.DoCreate(bag, cc);
    }

    protected abstract T DoCreate(IUxmlAttributes bag, CreationContext cc);
  }
}
