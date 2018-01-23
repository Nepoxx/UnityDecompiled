// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationWindowEvent
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class AnimationWindowEvent : ScriptableObject
  {
    public GameObject root;
    public AnimationClip clip;
    public AnimationClipInfoProperties clipInfo;
    public int eventIndex;

    public static AnimationWindowEvent CreateAndEdit(GameObject root, AnimationClip clip, float time)
    {
      AnimationEvent evt = new AnimationEvent();
      evt.time = time;
      AnimationEvent[] animationEvents = AnimationUtility.GetAnimationEvents(clip);
      int num = AnimationWindowEvent.InsertAnimationEvent(ref animationEvents, clip, evt);
      AnimationWindowEvent instance = ScriptableObject.CreateInstance<AnimationWindowEvent>();
      instance.hideFlags = HideFlags.HideInHierarchy;
      instance.name = "Animation Event";
      instance.root = root;
      instance.clip = clip;
      instance.clipInfo = (AnimationClipInfoProperties) null;
      instance.eventIndex = num;
      return instance;
    }

    public static AnimationWindowEvent Edit(GameObject root, AnimationClip clip, int eventIndex)
    {
      AnimationWindowEvent instance = ScriptableObject.CreateInstance<AnimationWindowEvent>();
      instance.hideFlags = HideFlags.HideInHierarchy;
      instance.name = "Animation Event";
      instance.root = root;
      instance.clip = clip;
      instance.clipInfo = (AnimationClipInfoProperties) null;
      instance.eventIndex = eventIndex;
      return instance;
    }

    public static AnimationWindowEvent Edit(AnimationClipInfoProperties clipInfo, int eventIndex)
    {
      AnimationWindowEvent instance = ScriptableObject.CreateInstance<AnimationWindowEvent>();
      instance.hideFlags = HideFlags.HideInHierarchy;
      instance.name = "Animation Event";
      instance.root = (GameObject) null;
      instance.clip = (AnimationClip) null;
      instance.clipInfo = clipInfo;
      instance.eventIndex = eventIndex;
      return instance;
    }

    private static int InsertAnimationEvent(ref AnimationEvent[] events, AnimationClip clip, AnimationEvent evt)
    {
      Undo.RegisterCompleteObjectUndo((Object) clip, "Add Event");
      int index1 = events.Length;
      for (int index2 = 0; index2 < events.Length; ++index2)
      {
        if ((double) events[index2].time > (double) evt.time)
        {
          index1 = index2;
          break;
        }
      }
      ArrayUtility.Insert<AnimationEvent>(ref events, index1, evt);
      AnimationUtility.SetAnimationEvents(clip, events);
      events = AnimationUtility.GetAnimationEvents(clip);
      if ((double) events[index1].time != (double) evt.time || events[index1].functionName != evt.functionName)
        Debug.LogError((object) "Failed insertion");
      return index1;
    }
  }
}
