// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.MixerEffectDefinitionReloader
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Audio
{
  [InitializeOnLoad]
  internal static class MixerEffectDefinitionReloader
  {
    static MixerEffectDefinitionReloader()
    {
      MixerEffectDefinitions.Refresh();
      EditorApplication.CallbackFunction projectWindowChanged = EditorApplication.projectWindowChanged;
      // ISSUE: reference to a compiler-generated field
      if (MixerEffectDefinitionReloader.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MixerEffectDefinitionReloader.\u003C\u003Ef__mg\u0024cache0 = new EditorApplication.CallbackFunction(MixerEffectDefinitionReloader.OnProjectChanged);
      }
      // ISSUE: reference to a compiler-generated field
      EditorApplication.CallbackFunction fMgCache0 = MixerEffectDefinitionReloader.\u003C\u003Ef__mg\u0024cache0;
      EditorApplication.projectWindowChanged = projectWindowChanged + fMgCache0;
    }

    private static void OnProjectChanged()
    {
      MixerEffectDefinitions.Refresh();
    }
  }
}
