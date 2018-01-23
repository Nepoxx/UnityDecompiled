// Decompiled with JetBrains decompiler
// Type: UnityEngine.U2D.Interface.EventSystem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEngine.U2D.Interface
{
  internal class EventSystem : IEventSystem
  {
    public IEvent current
    {
      get
      {
        return (IEvent) new Event();
      }
    }
  }
}
