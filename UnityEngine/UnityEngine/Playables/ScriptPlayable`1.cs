// Decompiled with JetBrains decompiler
// Type: UnityEngine.Playables.ScriptPlayable`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Playables
{
  public struct ScriptPlayable<T> : IPlayable, IEquatable<ScriptPlayable<T>> where T : class, IPlayableBehaviour, new()
  {
    private static readonly ScriptPlayable<T> m_NullPlayable = new ScriptPlayable<T>(PlayableHandle.Null);
    private PlayableHandle m_Handle;

    internal ScriptPlayable(PlayableHandle handle)
    {
      if (handle.IsValid() && !typeof (T).IsAssignableFrom(handle.GetPlayableType()))
        throw new InvalidCastException(string.Format("Incompatible handle: Trying to assign a playable data of type `{0}` that is not compatible with the PlayableBehaviour of type `{1}`.", (object) handle.GetPlayableType(), (object) typeof (T)));
      this.m_Handle = handle;
    }

    public static ScriptPlayable<T> Null
    {
      get
      {
        return ScriptPlayable<T>.m_NullPlayable;
      }
    }

    public static ScriptPlayable<T> Create(PlayableGraph graph, int inputCount = 0)
    {
      return new ScriptPlayable<T>(ScriptPlayable<T>.CreateHandle(graph, (T) null, inputCount));
    }

    public static ScriptPlayable<T> Create(PlayableGraph graph, T template, int inputCount = 0)
    {
      return new ScriptPlayable<T>(ScriptPlayable<T>.CreateHandle(graph, template, inputCount));
    }

    private static PlayableHandle CreateHandle(PlayableGraph graph, T template, int inputCount)
    {
      object scriptInstance = (object) template != null ? ScriptPlayable<T>.CloneScriptInstance((IPlayableBehaviour) template) : ScriptPlayable<T>.CreateScriptInstance();
      if (scriptInstance == null)
      {
        Debug.LogError((object) ("Could not create a ScriptPlayable of Type " + typeof (T).ToString()));
        return PlayableHandle.Null;
      }
      PlayableHandle playableHandle = graph.CreatePlayableHandle();
      if (!playableHandle.IsValid())
        return PlayableHandle.Null;
      playableHandle.SetInputCount(inputCount);
      playableHandle.SetScriptInstance(scriptInstance);
      return playableHandle;
    }

    private static object CreateScriptInstance()
    {
      return !typeof (ScriptableObject).IsAssignableFrom(typeof (T)) ? (object) Activator.CreateInstance<T>() : (object) ((object) ScriptableObject.CreateInstance(typeof (T)) as T);
    }

    private static object CloneScriptInstance(IPlayableBehaviour source)
    {
      UnityEngine.Object source1 = source as UnityEngine.Object;
      if (source1 != (UnityEngine.Object) null)
        return ScriptPlayable<T>.CloneScriptInstanceFromEngineObject(source1);
      ICloneable source2 = source as ICloneable;
      if (source2 != null)
        return ScriptPlayable<T>.CloneScriptInstanceFromIClonable(source2);
      return (object) null;
    }

    private static object CloneScriptInstanceFromEngineObject(UnityEngine.Object source)
    {
      UnityEngine.Object @object = UnityEngine.Object.Instantiate(source);
      if (@object != (UnityEngine.Object) null)
        @object.hideFlags |= HideFlags.DontSave;
      return (object) @object;
    }

    private static object CloneScriptInstanceFromIClonable(ICloneable source)
    {
      return source.Clone();
    }

    public PlayableHandle GetHandle()
    {
      return this.m_Handle;
    }

    public T GetBehaviour()
    {
      return this.m_Handle.GetObject<T>();
    }

    public static implicit operator Playable(ScriptPlayable<T> playable)
    {
      return new Playable(playable.GetHandle());
    }

    public static explicit operator ScriptPlayable<T>(Playable playable)
    {
      return new ScriptPlayable<T>(playable.GetHandle());
    }

    public bool Equals(ScriptPlayable<T> other)
    {
      return this.GetHandle() == other.GetHandle();
    }
  }
}
