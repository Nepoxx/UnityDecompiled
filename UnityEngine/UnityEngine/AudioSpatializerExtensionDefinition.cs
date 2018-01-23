// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioSpatializerExtensionDefinition
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  internal class AudioSpatializerExtensionDefinition
  {
    public PropertyName spatializerName;
    public AudioExtensionDefinition definition;
    public AudioExtensionDefinition editorDefinition;

    public AudioSpatializerExtensionDefinition(string spatializerNameIn, AudioExtensionDefinition definitionIn, AudioExtensionDefinition editorDefinitionIn)
    {
      this.spatializerName = (PropertyName) spatializerNameIn;
      this.definition = definitionIn;
      this.editorDefinition = editorDefinitionIn;
    }
  }
}
