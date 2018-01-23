// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowCallback.DoCreateAudioMixer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Audio;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityEditor.ProjectWindowCallback
{
  internal class DoCreateAudioMixer : EndNameEditAction
  {
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
      AudioMixerController controllerAtPath = AudioMixerController.CreateMixerControllerAtPath(pathName);
      int result;
      if (!string.IsNullOrEmpty(resourceFile) && int.TryParse(resourceFile, out result))
      {
        AudioMixerGroupController objectFromInstanceId = InternalEditorUtility.GetObjectFromInstanceID(result) as AudioMixerGroupController;
        if ((Object) objectFromInstanceId != (Object) null)
          controllerAtPath.outputAudioMixerGroup = (AudioMixerGroup) objectFromInstanceId;
      }
      ProjectWindowUtil.ShowCreatedAsset((Object) controllerAtPath);
    }
  }
}
