// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.AudioMixerController
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Scripting;

namespace UnityEditor.Audio
{
  internal sealed class AudioMixerController : AudioMixer
  {
    public static float kMinVolume = -80f;
    public static float kMaxEffect = 0.0f;
    public static float kVolumeWarp = 1.7f;
    public static string s_GroupEffectDisplaySeperator = "\\";
    [NonSerialized]
    public int m_HighlightEffectIndex = -1;
    [NonSerialized]
    private List<AudioMixerGroupController> m_CachedSelection = (List<AudioMixerGroupController>) null;
    [NonSerialized]
    private Dictionary<GUID, AudioParameterPath> m_ExposedParamPathCache;

    public AudioMixerController()
    {
      AudioMixerController.Internal_CreateAudioMixerController(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateAudioMixerController(AudioMixerController mono);

    private static void GetGroupsRecurse(AudioMixerGroupController group, List<AudioMixerGroupController> groups)
    {
      groups.Add(group);
      foreach (AudioMixerGroupController child in group.children)
        AudioMixerController.GetGroupsRecurse(child, groups);
    }

    public AudioMixerGroupController[] allGroups
    {
      get
      {
        List<AudioMixerGroupController> groups = new List<AudioMixerGroupController>();
        AudioMixerController.GetGroupsRecurse(this.masterGroup, groups);
        return groups.ToArray();
      }
    }

    public extern int numExposedParameters { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern ExposedAudioParameter[] exposedParameters { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern AudioMixerGroupController masterGroup { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern AudioMixerSnapshot startSnapshot { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern AudioMixerSnapshotController TargetSnapshot { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern AudioMixerSnapshotController[] snapshots { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int GetGroupVUInfo(GUID group, bool fader, ref float[] vuLevel, ref float[] vuPeak);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void UpdateMuteSolo();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void UpdateBypass();

    public List<AudioMixerGroupController> CachedSelection
    {
      get
      {
        if (this.m_CachedSelection == null)
          this.m_CachedSelection = new List<AudioMixerGroupController>();
        return this.m_CachedSelection;
      }
    }

    public extern int currentViewIndex { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool CurrentViewContainsGroup(GUID group);

    public extern MixerGroupView[] views { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CheckForCyclicReferences(AudioMixer mixer, AudioMixerGroup group);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern float GetMaxVolume();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern float GetVolumeSplitPoint();

    public extern bool isSuspended { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool EditingTargetSnapshot();

    public event ChangedExposedParameterHandler ChangedExposedParameter;

    public void OnChangedExposedParameter()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.ChangedExposedParameter == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.ChangedExposedParameter();
    }

    public void ClearEventHandlers()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.ChangedExposedParameter == null)
        return;
      // ISSUE: reference to a compiler-generated field
      foreach (ChangedExposedParameterHandler invocation in this.ChangedExposedParameter.GetInvocationList())
        this.ChangedExposedParameter -= invocation;
    }

    private Dictionary<GUID, AudioParameterPath> exposedParamCache
    {
      get
      {
        if (this.m_ExposedParamPathCache == null)
          this.m_ExposedParamPathCache = new Dictionary<GUID, AudioParameterPath>();
        return this.m_ExposedParamPathCache;
      }
    }

    private string FindUniqueParameterName(string template, ExposedAudioParameter[] parameters)
    {
      string str = template;
      int num = 1;
      for (int index = 0; index < parameters.Length; ++index)
      {
        if (str == parameters[index].name)
        {
          str = template + " " + (object) num++;
          index = -1;
        }
      }
      return str;
    }

    private int SortFuncForExposedParameters(ExposedAudioParameter p1, ExposedAudioParameter p2)
    {
      return string.CompareOrdinal(this.ResolveExposedParameterPath(p1.guid, true), this.ResolveExposedParameterPath(p2.guid, true));
    }

    public void AddExposedParameter(AudioParameterPath path)
    {
      if (!this.ContainsExposedParameter(path.parameter))
      {
        List<ExposedAudioParameter> exposedAudioParameterList = new List<ExposedAudioParameter>((IEnumerable<ExposedAudioParameter>) this.exposedParameters);
        exposedAudioParameterList.Add(new ExposedAudioParameter()
        {
          name = this.FindUniqueParameterName("MyExposedParam", this.exposedParameters),
          guid = path.parameter
        });
        exposedAudioParameterList.Sort(new Comparison<ExposedAudioParameter>(this.SortFuncForExposedParameters));
        this.exposedParameters = exposedAudioParameterList.ToArray();
        this.OnChangedExposedParameter();
        this.exposedParamCache[path.parameter] = path;
        AudioMixerUtility.RepaintAudioMixerAndInspectors();
      }
      else
        Debug.LogError((object) "Cannot expose the same parameter more than once!");
    }

    public bool ContainsExposedParameter(GUID parameter)
    {
      return ((IEnumerable<ExposedAudioParameter>) this.exposedParameters).Where<ExposedAudioParameter>((Func<ExposedAudioParameter, bool>) (val => val.guid == parameter)).ToArray<ExposedAudioParameter>().Length > 0;
    }

    public void RemoveExposedParameter(GUID parameter)
    {
      this.exposedParameters = ((IEnumerable<ExposedAudioParameter>) this.exposedParameters).Where<ExposedAudioParameter>((Func<ExposedAudioParameter, bool>) (val => val.guid != parameter)).ToArray<ExposedAudioParameter>();
      this.OnChangedExposedParameter();
      if (this.exposedParamCache.ContainsKey(parameter))
        this.exposedParamCache.Remove(parameter);
      AudioMixerUtility.RepaintAudioMixerAndInspectors();
    }

    public string ResolveExposedParameterPath(GUID parameter, bool getOnlyBasePath)
    {
      if (this.exposedParamCache.ContainsKey(parameter))
        return this.exposedParamCache[parameter].ResolveStringPath(getOnlyBasePath);
      foreach (AudioMixerGroupController group in this.GetAllAudioGroupsSlow())
      {
        if (group.GetGUIDForVolume() == parameter || group.GetGUIDForPitch() == parameter)
        {
          AudioGroupParameterPath groupParameterPath = new AudioGroupParameterPath(group, parameter);
          this.exposedParamCache[parameter] = (AudioParameterPath) groupParameterPath;
          return groupParameterPath.ResolveStringPath(getOnlyBasePath);
        }
        for (int index = 0; index < group.effects.Length; ++index)
        {
          AudioMixerEffectController effect = group.effects[index];
          foreach (MixerParameterDefinition effectParameter in MixerEffectDefinitions.GetEffectParameters(effect.effectName))
          {
            if (effect.GetGUIDForParameter(effectParameter.name) == parameter)
            {
              AudioEffectParameterPath effectParameterPath = new AudioEffectParameterPath(group, effect, parameter);
              this.exposedParamCache[parameter] = (AudioParameterPath) effectParameterPath;
              return effectParameterPath.ResolveStringPath(getOnlyBasePath);
            }
          }
        }
      }
      return "Error finding Parameter path";
    }

    public static AudioMixerController CreateMixerControllerAtPath(string path)
    {
      AudioMixerController audioMixerController = new AudioMixerController();
      audioMixerController.CreateDefaultAsset(path);
      return audioMixerController;
    }

    public void CreateDefaultAsset(string path)
    {
      this.masterGroup = new AudioMixerGroupController((AudioMixer) this);
      this.masterGroup.name = "Master";
      this.masterGroup.PreallocateGUIDs();
      AudioMixerEffectController effect = new AudioMixerEffectController("Attenuation");
      effect.PreallocateGUIDs();
      this.masterGroup.InsertEffect(effect, 0);
      AudioMixerSnapshotController snapshotController = new AudioMixerSnapshotController((AudioMixer) this);
      snapshotController.name = "Snapshot";
      this.snapshots = new AudioMixerSnapshotController[1]
      {
        snapshotController
      };
      this.startSnapshot = (AudioMixerSnapshot) snapshotController;
      AssetDatabase.CreateAssetFromObjects(new UnityEngine.Object[4]
      {
        (UnityEngine.Object) this,
        (UnityEngine.Object) this.masterGroup,
        (UnityEngine.Object) effect,
        (UnityEngine.Object) snapshotController
      }, path);
    }

    private void BuildTestSetup(System.Random r, AudioMixerGroupController parent, int minSpan, int maxSpan, int maxGroups, string prefix, ref int numGroups)
    {
      int num = numGroups != 0 ? r.Next(minSpan, maxSpan + 1) : maxSpan;
      for (int index = 0; index < num; ++index)
      {
        string str = prefix + (object) index;
        AudioMixerGroupController newGroup = this.CreateNewGroup(str, false);
        this.AddChildToParent(newGroup, parent);
        if (++numGroups >= maxGroups)
          break;
        this.BuildTestSetup(r, newGroup, minSpan, maxSpan <= minSpan ? minSpan : maxSpan - 1, maxGroups, str, ref numGroups);
      }
    }

    public void BuildTestSetup(int minSpan, int maxSpan, int maxGroups)
    {
      int numGroups = 0;
      this.DeleteGroups(this.masterGroup.children);
      this.BuildTestSetup(new System.Random(), this.masterGroup, minSpan, maxSpan, maxGroups, "G", ref numGroups);
    }

    public List<AudioMixerGroupController> GetAllAudioGroupsSlow()
    {
      List<AudioMixerGroupController> groups = new List<AudioMixerGroupController>();
      if ((UnityEngine.Object) this.masterGroup != (UnityEngine.Object) null)
        this.GetAllAudioGroupsSlowRecurse(this.masterGroup, ref groups);
      return groups;
    }

    private void GetAllAudioGroupsSlowRecurse(AudioMixerGroupController g, ref List<AudioMixerGroupController> groups)
    {
      groups.Add(g);
      foreach (AudioMixerGroupController child in g.children)
        this.GetAllAudioGroupsSlowRecurse(child, ref groups);
    }

    public bool HasMoreThanOneGroup()
    {
      return this.masterGroup.children.Length > 0;
    }

    private bool IsChildOf(AudioMixerGroupController child, List<AudioMixerGroupController> groups)
    {
      while ((UnityEngine.Object) child != (UnityEngine.Object) null)
      {
        child = this.FindParentGroup(this.masterGroup, child);
        if (groups.Contains(child))
          return true;
      }
      return false;
    }

    public bool AreAnyOfTheGroupsInTheListAncestors(List<AudioMixerGroupController> groups)
    {
      return groups.Any<AudioMixerGroupController>((Func<AudioMixerGroupController, bool>) (g => this.IsChildOf(g, groups)));
    }

    private void RemoveAncestorGroups(List<AudioMixerGroupController> groups)
    {
      groups.RemoveAll((Predicate<AudioMixerGroupController>) (g => this.IsChildOf(g, groups)));
      object.Equals((object) this.AreAnyOfTheGroupsInTheListAncestors(groups), (object) false);
    }

    private void DestroyExposedParametersContainedInEffect(AudioMixerEffectController effect)
    {
      Undo.RecordObject((UnityEngine.Object) this, "Changed Exposed Parameters");
      foreach (ExposedAudioParameter exposedParameter in this.exposedParameters)
      {
        if (effect.ContainsParameterGUID(exposedParameter.guid))
          this.RemoveExposedParameter(exposedParameter.guid);
      }
    }

    private void DestroyExposedParametersContainedInGroup(AudioMixerGroupController group)
    {
      Undo.RecordObject((UnityEngine.Object) this, "Remove Exposed Parameter");
      foreach (ExposedAudioParameter exposedParameter in this.exposedParameters)
      {
        if (group.GetGUIDForVolume() == exposedParameter.guid || group.GetGUIDForPitch() == exposedParameter.guid)
          this.RemoveExposedParameter(exposedParameter.guid);
      }
    }

    private void DeleteSubGroupRecursive(AudioMixerGroupController group)
    {
      foreach (AudioMixerGroupController child in group.children)
        this.DeleteSubGroupRecursive(child);
      foreach (AudioMixerEffectController effect in group.effects)
      {
        this.DestroyExposedParametersContainedInEffect(effect);
        Undo.DestroyObjectImmediate((UnityEngine.Object) effect);
      }
      this.DestroyExposedParametersContainedInGroup(group);
      Undo.DestroyObjectImmediate((UnityEngine.Object) group);
    }

    private void DeleteGroupsInternal(List<AudioMixerGroupController> groupsToDelete, List<AudioMixerGroupController> allGroups)
    {
      foreach (AudioMixerGroupController allGroup in allGroups)
      {
        IEnumerable<AudioMixerGroupController> mixerGroupControllers = groupsToDelete.Intersect<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) allGroup.children);
        if (mixerGroupControllers.Count<AudioMixerGroupController>() > 0)
        {
          Undo.RegisterCompleteObjectUndo((UnityEngine.Object) allGroup, "Delete Group(s)");
          allGroup.children = ((IEnumerable<AudioMixerGroupController>) allGroup.children).Except<AudioMixerGroupController>(mixerGroupControllers).ToArray<AudioMixerGroupController>();
        }
      }
      foreach (AudioMixerGroupController group in groupsToDelete)
        this.DeleteSubGroupRecursive(group);
    }

    public void DeleteGroups(AudioMixerGroupController[] groups)
    {
      List<AudioMixerGroupController> list = ((IEnumerable<AudioMixerGroupController>) groups).ToList<AudioMixerGroupController>();
      this.RemoveAncestorGroups(list);
      this.DeleteGroupsInternal(list, this.GetAllAudioGroupsSlow());
      this.OnUnitySelectionChanged();
    }

    public void RemoveEffect(AudioMixerEffectController effect, AudioMixerGroupController group)
    {
      Undo.RecordObject((UnityEngine.Object) group, "Delete Effect");
      List<AudioMixerEffectController> effectControllerList = new List<AudioMixerEffectController>((IEnumerable<AudioMixerEffectController>) group.effects);
      effectControllerList.Remove(effect);
      group.effects = effectControllerList.ToArray();
      this.DestroyExposedParametersContainedInEffect(effect);
      Undo.DestroyObjectImmediate((UnityEngine.Object) effect);
    }

    public void OnSubAssetChanged()
    {
      AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((UnityEngine.Object) this));
    }

    public void CloneNewSnapshotFromTarget(bool storeUndoState)
    {
      List<AudioMixerSnapshotController> snapshotControllerList = new List<AudioMixerSnapshotController>((IEnumerable<AudioMixerSnapshotController>) this.snapshots);
      AudioMixerSnapshotController snapshotController = UnityEngine.Object.Instantiate<AudioMixerSnapshotController>(this.TargetSnapshot);
      snapshotController.name = this.TargetSnapshot.name + " - Copy";
      snapshotControllerList.Add(snapshotController);
      this.snapshots = snapshotControllerList.ToArray();
      this.TargetSnapshot = snapshotControllerList[snapshotControllerList.Count - 1];
      AssetDatabase.AddObjectToAsset((UnityEngine.Object) snapshotController, (UnityEngine.Object) this);
      if (storeUndoState)
        Undo.RegisterCreatedObjectUndo((UnityEngine.Object) snapshotController, "");
      this.OnSubAssetChanged();
    }

    public void RemoveTargetSnapshot()
    {
      if (this.snapshots.Length < 2)
        return;
      AudioMixerSnapshotController targetSnapshot = this.TargetSnapshot;
      Undo.RecordObject((UnityEngine.Object) this, "Remove Snapshot");
      List<AudioMixerSnapshotController> snapshotControllerList = new List<AudioMixerSnapshotController>((IEnumerable<AudioMixerSnapshotController>) this.snapshots);
      snapshotControllerList.Remove(targetSnapshot);
      this.snapshots = snapshotControllerList.ToArray();
      Undo.DestroyObjectImmediate((UnityEngine.Object) targetSnapshot);
      this.OnSubAssetChanged();
    }

    public void RemoveSnapshot(AudioMixerSnapshotController snapshot)
    {
      if (this.snapshots.Length < 2)
        return;
      AudioMixerSnapshotController snapshotController = snapshot;
      Undo.RecordObject((UnityEngine.Object) this, "Remove Snapshot");
      List<AudioMixerSnapshotController> snapshotControllerList = new List<AudioMixerSnapshotController>((IEnumerable<AudioMixerSnapshotController>) this.snapshots);
      snapshotControllerList.Remove(snapshotController);
      this.snapshots = snapshotControllerList.ToArray();
      Undo.DestroyObjectImmediate((UnityEngine.Object) snapshotController);
      this.OnSubAssetChanged();
    }

    public AudioMixerGroupController CreateNewGroup(string name, bool storeUndoState)
    {
      AudioMixerGroupController mixerGroupController = new AudioMixerGroupController((AudioMixer) this);
      mixerGroupController.name = name;
      mixerGroupController.PreallocateGUIDs();
      AudioMixerEffectController effect = new AudioMixerEffectController("Attenuation");
      this.AddNewSubAsset((UnityEngine.Object) effect, storeUndoState);
      effect.PreallocateGUIDs();
      mixerGroupController.InsertEffect(effect, 0);
      this.AddNewSubAsset((UnityEngine.Object) mixerGroupController, storeUndoState);
      return mixerGroupController;
    }

    public void AddChildToParent(AudioMixerGroupController child, AudioMixerGroupController parent)
    {
      this.RemoveGroupsFromParent(new AudioMixerGroupController[1]
      {
        child
      }, false);
      parent.children = new List<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) parent.children)
      {
        child
      }.ToArray();
    }

    private void AddNewSubAsset(UnityEngine.Object obj, bool storeUndoState)
    {
      AssetDatabase.AddObjectToAsset(obj, (UnityEngine.Object) this);
      if (!storeUndoState)
        return;
      Undo.RegisterCreatedObjectUndo(obj, "");
    }

    public void RemoveGroupsFromParent(AudioMixerGroupController[] groups, bool storeUndoState)
    {
      List<AudioMixerGroupController> list = ((IEnumerable<AudioMixerGroupController>) groups).ToList<AudioMixerGroupController>();
      this.RemoveAncestorGroups(list);
      if (storeUndoState)
        Undo.RecordObject((UnityEngine.Object) this, "Remove group");
      foreach (AudioMixerGroupController mixerGroupController1 in list)
      {
        foreach (AudioMixerGroupController mixerGroupController2 in this.GetAllAudioGroupsSlow())
        {
          List<AudioMixerGroupController> mixerGroupControllerList = new List<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) mixerGroupController2.children);
          if (mixerGroupControllerList.Contains(mixerGroupController1))
            mixerGroupControllerList.Remove(mixerGroupController1);
          if (mixerGroupController2.children.Length != mixerGroupControllerList.Count)
            mixerGroupController2.children = mixerGroupControllerList.ToArray();
        }
      }
    }

    public AudioMixerGroupController FindParentGroup(AudioMixerGroupController node, AudioMixerGroupController group)
    {
      for (int index = 0; index < node.children.Length; ++index)
      {
        if ((UnityEngine.Object) node.children[index] == (UnityEngine.Object) group)
          return node;
        AudioMixerGroupController parentGroup = this.FindParentGroup(node.children[index], group);
        if ((UnityEngine.Object) parentGroup != (UnityEngine.Object) null)
          return parentGroup;
      }
      return (AudioMixerGroupController) null;
    }

    public AudioMixerEffectController CopyEffect(AudioMixerEffectController sourceEffect)
    {
      AudioMixerEffectController effectController = new AudioMixerEffectController(sourceEffect.effectName);
      effectController.name = sourceEffect.name;
      effectController.PreallocateGUIDs();
      MixerParameterDefinition[] effectParameters = MixerEffectDefinitions.GetEffectParameters(sourceEffect.effectName);
      foreach (AudioMixerSnapshotController snapshot in this.snapshots)
      {
        float num;
        if (snapshot.GetValue(sourceEffect.GetGUIDForMixLevel(), out num))
          snapshot.SetValue(effectController.GetGUIDForMixLevel(), num);
        foreach (MixerParameterDefinition parameterDefinition in effectParameters)
        {
          if (snapshot.GetValue(sourceEffect.GetGUIDForParameter(parameterDefinition.name), out num))
            snapshot.SetValue(effectController.GetGUIDForParameter(parameterDefinition.name), num);
        }
      }
      AssetDatabase.AddObjectToAsset((UnityEngine.Object) effectController, (UnityEngine.Object) this);
      return effectController;
    }

    private AudioMixerGroupController DuplicateGroupRecurse(AudioMixerGroupController sourceGroup, bool recordUndo)
    {
      AudioMixerGroupController group = new AudioMixerGroupController((AudioMixer) this);
      List<AudioMixerEffectController> effectControllerList = new List<AudioMixerEffectController>();
      foreach (AudioMixerEffectController effect in sourceGroup.effects)
        effectControllerList.Add(this.CopyEffect(effect));
      List<AudioMixerGroupController> mixerGroupControllerList = new List<AudioMixerGroupController>();
      foreach (AudioMixerGroupController child in sourceGroup.children)
        mixerGroupControllerList.Add(this.DuplicateGroupRecurse(child, recordUndo));
      group.name = sourceGroup.name + " - Copy";
      group.PreallocateGUIDs();
      group.effects = effectControllerList.ToArray();
      group.children = mixerGroupControllerList.ToArray();
      group.solo = sourceGroup.solo;
      group.mute = sourceGroup.mute;
      group.bypassEffects = sourceGroup.bypassEffects;
      foreach (AudioMixerSnapshotController snapshot in this.snapshots)
      {
        float num;
        if (snapshot.GetValue(sourceGroup.GetGUIDForVolume(), out num))
          snapshot.SetValue(group.GetGUIDForVolume(), num);
        if (snapshot.GetValue(sourceGroup.GetGUIDForPitch(), out num))
          snapshot.SetValue(group.GetGUIDForPitch(), num);
      }
      AssetDatabase.AddObjectToAsset((UnityEngine.Object) group, (UnityEngine.Object) this);
      if (recordUndo)
        Undo.RegisterCreatedObjectUndo((UnityEngine.Object) group, group.name);
      if (this.CurrentViewContainsGroup(sourceGroup.groupID))
        group.controller.AddGroupToCurrentView(group);
      return group;
    }

    public List<AudioMixerGroupController> DuplicateGroups(AudioMixerGroupController[] sourceGroups, bool recordUndo)
    {
      List<AudioMixerGroupController> list = ((IEnumerable<AudioMixerGroupController>) sourceGroups).ToList<AudioMixerGroupController>();
      this.RemoveAncestorGroups(list);
      List<AudioMixerGroupController> mixerGroupControllerList = new List<AudioMixerGroupController>();
      foreach (AudioMixerGroupController mixerGroupController1 in list)
      {
        AudioMixerGroupController parentGroup = this.FindParentGroup(this.masterGroup, mixerGroupController1);
        if ((UnityEngine.Object) parentGroup != (UnityEngine.Object) null && (UnityEngine.Object) mixerGroupController1 != (UnityEngine.Object) null)
        {
          AudioMixerGroupController mixerGroupController2 = this.DuplicateGroupRecurse(mixerGroupController1, recordUndo);
          parentGroup.children = new List<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) parentGroup.children)
          {
            mixerGroupController2
          }.ToArray();
          mixerGroupControllerList.Add(mixerGroupController2);
        }
      }
      return mixerGroupControllerList;
    }

    public void CopyEffectSettingsToAllSnapshots(AudioMixerGroupController group, int effectIndex, AudioMixerSnapshotController snapshot, bool includeWetParam)
    {
      AudioMixerSnapshotController[] snapshots = this.snapshots;
      for (int index = 0; index < snapshots.Length; ++index)
      {
        if (!((UnityEngine.Object) snapshots[index] == (UnityEngine.Object) snapshot))
        {
          AudioMixerEffectController effect = group.effects[effectIndex];
          MixerParameterDefinition[] effectParameters = MixerEffectDefinitions.GetEffectParameters(effect.effectName);
          float num;
          if (includeWetParam)
          {
            GUID guidForMixLevel = effect.GetGUIDForMixLevel();
            if (snapshot.GetValue(guidForMixLevel, out num))
              snapshots[index].SetValue(guidForMixLevel, num);
          }
          foreach (MixerParameterDefinition parameterDefinition in effectParameters)
          {
            GUID guidForParameter = effect.GetGUIDForParameter(parameterDefinition.name);
            if (snapshot.GetValue(guidForParameter, out num))
              snapshots[index].SetValue(guidForParameter, num);
          }
        }
      }
    }

    public void CopyAllSettingsToAllSnapshots(AudioMixerGroupController group, AudioMixerSnapshotController snapshot)
    {
      for (int effectIndex = 0; effectIndex < group.effects.Length; ++effectIndex)
        this.CopyEffectSettingsToAllSnapshots(group, effectIndex, snapshot, true);
      AudioMixerSnapshotController[] snapshots = this.snapshots;
      for (int index = 0; index < snapshots.Length; ++index)
      {
        if (!((UnityEngine.Object) snapshots[index] == (UnityEngine.Object) snapshot))
        {
          AudioMixerSnapshotController snapshot1 = snapshots[index];
          group.SetValueForVolume(this, snapshot1, group.GetValueForVolume(this, snapshot));
          group.SetValueForPitch(this, snapshot1, group.GetValueForPitch(this, snapshot));
        }
      }
    }

    public void CopyAttenuationToAllSnapshots(AudioMixerGroupController group, AudioMixerSnapshotController snapshot)
    {
      AudioMixerSnapshotController[] snapshots = this.snapshots;
      for (int index = 0; index < snapshots.Length; ++index)
      {
        if (!((UnityEngine.Object) snapshots[index] == (UnityEngine.Object) snapshot))
        {
          AudioMixerSnapshotController snapshot1 = snapshots[index];
          group.SetValueForVolume(this, snapshot1, group.GetValueForVolume(this, snapshot));
        }
      }
    }

    public void ReparentSelection(AudioMixerGroupController newParent, int insertionIndex, List<AudioMixerGroupController> selection)
    {
      if (insertionIndex >= 0)
        insertionIndex -= ((IEnumerable<AudioMixerGroupController>) newParent.children).ToList<AudioMixerGroupController>().GetRange(0, insertionIndex).Count<AudioMixerGroupController>(new Func<AudioMixerGroupController, bool>(selection.Contains));
      Undo.RecordObject((UnityEngine.Object) newParent, "Change Audio Mixer Group Parent");
      foreach (AudioMixerGroupController mixerGroupController1 in this.GetAllAudioGroupsSlow())
      {
        if (((IEnumerable<AudioMixerGroupController>) mixerGroupController1.children).Intersect<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) selection).Any<AudioMixerGroupController>())
        {
          Undo.RecordObject((UnityEngine.Object) mixerGroupController1, string.Empty);
          List<AudioMixerGroupController> mixerGroupControllerList = new List<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) mixerGroupController1.children);
          foreach (AudioMixerGroupController mixerGroupController2 in selection)
            mixerGroupControllerList.Remove(mixerGroupController2);
          mixerGroupController1.children = mixerGroupControllerList.ToArray();
        }
      }
      if (insertionIndex == -1)
        insertionIndex = 0;
      List<AudioMixerGroupController> mixerGroupControllerList1 = new List<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) newParent.children);
      mixerGroupControllerList1.InsertRange(insertionIndex, (IEnumerable<AudioMixerGroupController>) selection);
      newParent.children = mixerGroupControllerList1.ToArray();
    }

    public static bool InsertEffect(AudioMixerEffectController effect, ref List<AudioMixerEffectController> targetEffects, int targetIndex)
    {
      if (targetIndex < 0 || targetIndex > targetEffects.Count)
      {
        Debug.LogError((object) ("Inserting effect failed! size: " + (object) targetEffects.Count + " at index: " + (object) targetIndex));
        return false;
      }
      targetEffects.Insert(targetIndex, effect);
      return true;
    }

    public static bool MoveEffect(ref List<AudioMixerEffectController> sourceEffects, int sourceIndex, ref List<AudioMixerEffectController> targetEffects, int targetIndex)
    {
      if (sourceEffects == targetEffects)
      {
        if (targetIndex > sourceIndex)
          --targetIndex;
        if (sourceIndex == targetIndex)
          return false;
      }
      if (sourceIndex < 0 || sourceIndex >= sourceEffects.Count || (targetIndex < 0 || targetIndex > targetEffects.Count))
        return false;
      AudioMixerEffectController effectController = sourceEffects[sourceIndex];
      sourceEffects.RemoveAt(sourceIndex);
      targetEffects.Insert(targetIndex, effectController);
      return true;
    }

    public static string FixNameForPopupMenu(string s)
    {
      return s;
    }

    public void ClearSendConnectionsTo(AudioMixerEffectController sendTarget)
    {
      foreach (AudioMixerGroupController mixerGroupController in this.GetAllAudioGroupsSlow())
      {
        foreach (AudioMixerEffectController effect in mixerGroupController.effects)
        {
          if (effect.IsSend() && (UnityEngine.Object) effect.sendTarget == (UnityEngine.Object) sendTarget)
          {
            Undo.RecordObject((UnityEngine.Object) effect, "Clear Send target");
            effect.sendTarget = (AudioMixerEffectController) null;
          }
        }
      }
    }

    private static Dictionary<object, AudioMixerController.ConnectionNode> BuildTemporaryGraph(List<AudioMixerGroupController> allGroups, AudioMixerGroupController groupWhoseEffectIsChanged, AudioMixerEffectController effectWhoseTargetIsChanged, AudioMixerEffectController targetToTest, AudioMixerGroupController modifiedGroup1, List<AudioMixerEffectController> modifiedGroupEffects1, AudioMixerGroupController modifiedGroup2, List<AudioMixerEffectController> modifiedGroupEffects2)
    {
      Dictionary<object, AudioMixerController.ConnectionNode> dictionary = new Dictionary<object, AudioMixerController.ConnectionNode>();
      foreach (AudioMixerGroupController allGroup in allGroups)
      {
        dictionary[(object) allGroup] = new AudioMixerController.ConnectionNode()
        {
          group = allGroup,
          effect = (AudioMixerEffectController) null
        };
        object index = (object) allGroup;
        foreach (AudioMixerEffectController effectController1 in !((UnityEngine.Object) allGroup == (UnityEngine.Object) modifiedGroup1) ? (!((UnityEngine.Object) allGroup == (UnityEngine.Object) modifiedGroup2) ? ((IEnumerable<AudioMixerEffectController>) allGroup.effects).ToList<AudioMixerEffectController>() : modifiedGroupEffects2) : modifiedGroupEffects1)
        {
          if (!dictionary.ContainsKey((object) effectController1))
            dictionary[(object) effectController1] = new AudioMixerController.ConnectionNode();
          dictionary[(object) effectController1].group = allGroup;
          dictionary[(object) effectController1].effect = effectController1;
          if (!dictionary[index].targets.Contains((object) effectController1))
            dictionary[index].targets.Add((object) effectController1);
          AudioMixerEffectController effectController2 = !((UnityEngine.Object) allGroup == (UnityEngine.Object) groupWhoseEffectIsChanged) || !((UnityEngine.Object) effectWhoseTargetIsChanged == (UnityEngine.Object) effectController1) ? effectController1.sendTarget : targetToTest;
          if ((UnityEngine.Object) effectController2 != (UnityEngine.Object) null)
          {
            if (!dictionary.ContainsKey((object) effectController2))
            {
              dictionary[(object) effectController2] = new AudioMixerController.ConnectionNode();
              dictionary[(object) effectController2].group = allGroup;
              dictionary[(object) effectController2].effect = effectController2;
            }
            if (!dictionary[(object) effectController1].targets.Contains((object) effectController2))
              dictionary[(object) effectController1].targets.Add((object) effectController2);
          }
          index = (object) effectController1;
        }
        dictionary[(object) allGroup].groupTail = index;
      }
      return dictionary;
    }

    private static void ListTemporaryGraph(Dictionary<object, AudioMixerController.ConnectionNode> graph)
    {
      Debug.Log((object) "Listing temporary graph:");
      int num1 = 0;
      foreach (KeyValuePair<object, AudioMixerController.ConnectionNode> keyValuePair in graph)
      {
        Debug.Log((object) string.Format("Node {0}: {1}", (object) num1++, (object) keyValuePair.Value.GetDisplayString()));
        int num2 = 0;
        foreach (object target in keyValuePair.Value.targets)
          Debug.Log((object) string.Format("  Target {0}: {1}", (object) num2++, (object) graph[target].GetDisplayString()));
      }
    }

    private static bool CheckForCycle(object curr, Dictionary<object, AudioMixerController.ConnectionNode> graph, List<AudioMixerController.ConnectionNode> identifiedLoop)
    {
      AudioMixerController.ConnectionNode connectionNode = graph[curr];
      if (connectionNode.visited)
      {
        if (identifiedLoop != null)
        {
          identifiedLoop.Clear();
          identifiedLoop.Add(connectionNode);
        }
        return true;
      }
      connectionNode.visited = true;
      foreach (object target in connectionNode.targets)
      {
        if (AudioMixerController.CheckForCycle(target, graph, identifiedLoop))
        {
          connectionNode.visited = false;
          if (identifiedLoop != null)
            identifiedLoop.Add(connectionNode);
          return true;
        }
      }
      connectionNode.visited = false;
      return false;
    }

    public static bool DoesTheTemporaryGraphHaveAnyCycles(List<AudioMixerGroupController> allGroups, List<AudioMixerController.ConnectionNode> identifiedLoop, Dictionary<object, AudioMixerController.ConnectionNode> graph)
    {
      foreach (object allGroup in allGroups)
      {
        if (AudioMixerController.CheckForCycle(allGroup, graph, identifiedLoop))
        {
          if (identifiedLoop != null)
          {
            AudioMixerController.ConnectionNode connectionNode = identifiedLoop[0];
            int index = 1;
            do
              ;
            while (index < identifiedLoop.Count && identifiedLoop[index++] != connectionNode);
            identifiedLoop.RemoveRange(index, identifiedLoop.Count - index);
            identifiedLoop.Reverse();
          }
          return true;
        }
      }
      return false;
    }

    public static bool WillChangeOfEffectTargetCauseFeedback(List<AudioMixerGroupController> allGroups, AudioMixerGroupController groupWhoseEffectIsChanged, int effectWhoseTargetIsChanged, AudioMixerEffectController targetToTest, List<AudioMixerController.ConnectionNode> identifiedLoop)
    {
      Dictionary<object, AudioMixerController.ConnectionNode> graph = AudioMixerController.BuildTemporaryGraph(allGroups, groupWhoseEffectIsChanged, groupWhoseEffectIsChanged.effects[effectWhoseTargetIsChanged], targetToTest, (AudioMixerGroupController) null, (List<AudioMixerEffectController>) null, (AudioMixerGroupController) null, (List<AudioMixerEffectController>) null);
      foreach (AudioMixerGroupController allGroup in allGroups)
      {
        foreach (AudioMixerGroupController child in allGroup.children)
        {
          object groupTail = graph[(object) child].groupTail;
          if (!graph[groupTail].targets.Contains((object) allGroup))
            graph[groupTail].targets.Add((object) allGroup);
        }
      }
      return AudioMixerController.DoesTheTemporaryGraphHaveAnyCycles(allGroups, identifiedLoop, graph);
    }

    public static bool WillModificationOfTopologyCauseFeedback(List<AudioMixerGroupController> allGroups, List<AudioMixerGroupController> groupsToBeMoved, AudioMixerGroupController newParentForMovedGroups, List<AudioMixerController.ConnectionNode> identifiedLoop)
    {
      Dictionary<object, AudioMixerController.ConnectionNode> graph = AudioMixerController.BuildTemporaryGraph(allGroups, (AudioMixerGroupController) null, (AudioMixerEffectController) null, (AudioMixerEffectController) null, (AudioMixerGroupController) null, (List<AudioMixerEffectController>) null, (AudioMixerGroupController) null, (List<AudioMixerEffectController>) null);
      foreach (AudioMixerGroupController allGroup in allGroups)
      {
        foreach (AudioMixerGroupController child in allGroup.children)
        {
          AudioMixerGroupController mixerGroupController = !groupsToBeMoved.Contains(child) ? allGroup : newParentForMovedGroups;
          object groupTail = graph[(object) child].groupTail;
          if (!graph[groupTail].targets.Contains((object) mixerGroupController))
            graph[groupTail].targets.Add((object) mixerGroupController);
        }
      }
      return AudioMixerController.DoesTheTemporaryGraphHaveAnyCycles(allGroups, identifiedLoop, graph);
    }

    public static bool WillMovingEffectCauseFeedback(List<AudioMixerGroupController> allGroups, AudioMixerGroupController sourceGroup, int sourceIndex, AudioMixerGroupController targetGroup, int targetIndex, List<AudioMixerController.ConnectionNode> identifiedLoop)
    {
      Dictionary<object, AudioMixerController.ConnectionNode> graph;
      if ((UnityEngine.Object) sourceGroup == (UnityEngine.Object) targetGroup)
      {
        List<AudioMixerEffectController> list = ((IEnumerable<AudioMixerEffectController>) sourceGroup.effects).ToList<AudioMixerEffectController>();
        if (!AudioMixerController.MoveEffect(ref list, sourceIndex, ref list, targetIndex))
          return false;
        graph = AudioMixerController.BuildTemporaryGraph(allGroups, (AudioMixerGroupController) null, (AudioMixerEffectController) null, (AudioMixerEffectController) null, sourceGroup, list, (AudioMixerGroupController) null, (List<AudioMixerEffectController>) null);
      }
      else
      {
        List<AudioMixerEffectController> list1 = ((IEnumerable<AudioMixerEffectController>) sourceGroup.effects).ToList<AudioMixerEffectController>();
        List<AudioMixerEffectController> list2 = ((IEnumerable<AudioMixerEffectController>) targetGroup.effects).ToList<AudioMixerEffectController>();
        if (!AudioMixerController.MoveEffect(ref list1, sourceIndex, ref list2, targetIndex))
          return false;
        graph = AudioMixerController.BuildTemporaryGraph(allGroups, (AudioMixerGroupController) null, (AudioMixerEffectController) null, (AudioMixerEffectController) null, sourceGroup, list1, targetGroup, list2);
      }
      foreach (AudioMixerGroupController allGroup in allGroups)
      {
        foreach (AudioMixerGroupController child in allGroup.children)
        {
          object groupTail = graph[(object) child].groupTail;
          if (!graph[groupTail].targets.Contains((object) allGroup))
            graph[groupTail].targets.Add((object) allGroup);
        }
      }
      return AudioMixerController.DoesTheTemporaryGraphHaveAnyCycles(allGroups, identifiedLoop, graph);
    }

    public static float DbToLin(float x)
    {
      if ((double) x < (double) AudioMixerController.kMinVolume)
        return 0.0f;
      return Mathf.Pow(10f, x * 0.05f);
    }

    public void CloneViewFromCurrent()
    {
      Undo.RecordObject((UnityEngine.Object) this, "Create view");
      List<MixerGroupView> mixerGroupViewList = new List<MixerGroupView>((IEnumerable<MixerGroupView>) this.views);
      mixerGroupViewList.Add(new MixerGroupView()
      {
        name = this.views[this.currentViewIndex].name + " - Copy",
        guids = this.views[this.currentViewIndex].guids
      });
      this.views = mixerGroupViewList.ToArray();
      this.currentViewIndex = mixerGroupViewList.Count - 1;
    }

    public void DeleteView(int index)
    {
      Undo.RecordObject((UnityEngine.Object) this, "Delete view");
      List<MixerGroupView> mixerGroupViewList = new List<MixerGroupView>((IEnumerable<MixerGroupView>) this.views);
      mixerGroupViewList.RemoveAt(index);
      this.views = mixerGroupViewList.ToArray();
      this.ForceSetView(Mathf.Clamp(this.currentViewIndex, 0, mixerGroupViewList.Count - 1));
    }

    public void SetView(int index)
    {
      if (this.currentViewIndex == index)
        return;
      this.ForceSetView(index);
    }

    public void SanitizeGroupViews()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioMixerController.\u003CSanitizeGroupViews\u003Ec__AnonStorey4 viewsCAnonStorey4 = new AudioMixerController.\u003CSanitizeGroupViews\u003Ec__AnonStorey4();
      // ISSUE: reference to a compiler-generated field
      viewsCAnonStorey4.allGroups = this.GetAllAudioGroupsSlow();
      MixerGroupView[] views = this.views;
      for (int index = 0; index < views.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        views[index].guids = ((IEnumerable<GUID>) views[index].guids).SelectMany<GUID, AudioMixerGroupController, \u003C\u003E__AnonType0<GUID, AudioMixerGroupController>>(new Func<GUID, IEnumerable<AudioMixerGroupController>>(viewsCAnonStorey4.\u003C\u003Em__0), (Func<GUID, AudioMixerGroupController, \u003C\u003E__AnonType0<GUID, AudioMixerGroupController>>) ((x, y) =>
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          \u003C\u003E__AnonType0<GUID, AudioMixerGroupController> anonType0 = new \u003C\u003E__AnonType0<GUID, AudioMixerGroupController>(x, y);
          return anonType0;
        })).Where<\u003C\u003E__AnonType0<GUID, AudioMixerGroupController>>((Func<\u003C\u003E__AnonType0<GUID, AudioMixerGroupController>, bool>) (_param0 => _param0.y.groupID == _param0.x)).Select<\u003C\u003E__AnonType0<GUID, AudioMixerGroupController>, GUID>((Func<\u003C\u003E__AnonType0<GUID, AudioMixerGroupController>, GUID>) (_param0 => _param0.x)).ToArray<GUID>();
      }
      this.views = ((IEnumerable<MixerGroupView>) views).ToArray<MixerGroupView>();
    }

    public void ForceSetView(int index)
    {
      this.currentViewIndex = index;
      this.SanitizeGroupViews();
    }

    public void AddGroupToCurrentView(AudioMixerGroupController group)
    {
      MixerGroupView[] views = this.views;
      List<GUID> list = ((IEnumerable<GUID>) views[this.currentViewIndex].guids).ToList<GUID>();
      list.Add(group.groupID);
      views[this.currentViewIndex].guids = list.ToArray();
      this.views = ((IEnumerable<MixerGroupView>) views).ToArray<MixerGroupView>();
    }

    public void SetCurrentViewVisibility(GUID[] guids)
    {
      MixerGroupView[] views = this.views;
      views[this.currentViewIndex].guids = guids;
      this.views = ((IEnumerable<MixerGroupView>) views).ToArray<MixerGroupView>();
      this.SanitizeGroupViews();
    }

    public AudioMixerGroupController[] GetCurrentViewGroupList()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioMixerController.\u003CGetCurrentViewGroupList\u003Ec__AnonStorey5 listCAnonStorey5 = new AudioMixerController.\u003CGetCurrentViewGroupList\u003Ec__AnonStorey5();
      List<AudioMixerGroupController> allAudioGroupsSlow = this.GetAllAudioGroupsSlow();
      // ISSUE: reference to a compiler-generated field
      listCAnonStorey5.view = this.views[this.currentViewIndex];
      // ISSUE: reference to a compiler-generated method
      return allAudioGroupsSlow.Where<AudioMixerGroupController>(new Func<AudioMixerGroupController, bool>(listCAnonStorey5.\u003C\u003Em__0)).ToArray<AudioMixerGroupController>();
    }

    public static float VolumeToScreenMapping(float value, float screenRange, bool forward)
    {
      float num1 = AudioMixerController.GetVolumeSplitPoint() * screenRange;
      float num2 = screenRange - num1;
      if (forward)
        return (double) value <= 0.0 ? Mathf.Pow(value / AudioMixerController.kMinVolume, 1f / AudioMixerController.kVolumeWarp) * num2 + num1 : num1 - Mathf.Pow(value / AudioMixerController.GetMaxVolume(), 1f / AudioMixerController.kVolumeWarp) * num1;
      return (double) value >= (double) num1 ? Mathf.Pow((value - num1) / num2, AudioMixerController.kVolumeWarp) * AudioMixerController.kMinVolume : Mathf.Pow((float) (1.0 - (double) value / (double) num1), AudioMixerController.kVolumeWarp) * AudioMixerController.GetMaxVolume();
    }

    public void OnUnitySelectionChanged()
    {
      this.m_CachedSelection = this.GetAllAudioGroupsSlow().Intersect<AudioMixerGroupController>(((IEnumerable<UnityEngine.Object>) Selection.GetFiltered(typeof (AudioMixerGroupController), SelectionMode.Deep)).Select<UnityEngine.Object, AudioMixerGroupController>((Func<UnityEngine.Object, AudioMixerGroupController>) (g => (AudioMixerGroupController) g))).ToList<AudioMixerGroupController>();
    }

    public class ConnectionNode
    {
      public bool visited = false;
      public object groupTail = (object) null;
      public List<object> targets = new List<object>();
      public AudioMixerGroupController group = (AudioMixerGroupController) null;
      public AudioMixerEffectController effect = (AudioMixerEffectController) null;

      public string GetDisplayString()
      {
        string str = this.group.GetDisplayString();
        if ((UnityEngine.Object) this.effect != (UnityEngine.Object) null)
          str = str + AudioMixerController.s_GroupEffectDisplaySeperator + AudioMixerController.FixNameForPopupMenu(this.effect.effectName);
        return str;
      }
    }
  }
}
