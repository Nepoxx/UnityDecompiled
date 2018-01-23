// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioExtensionDefinition
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  internal class AudioExtensionDefinition
  {
    private string assemblyName;
    private string extensionNamespace;
    private string extensionTypeName;
    private System.Type extensionType;

    public AudioExtensionDefinition(AudioExtensionDefinition definition)
    {
      this.assemblyName = definition.assemblyName;
      this.extensionNamespace = definition.extensionNamespace;
      this.extensionTypeName = definition.extensionTypeName;
      this.extensionType = this.GetExtensionType();
    }

    public AudioExtensionDefinition(string assemblyNameIn, string extensionNamespaceIn, string extensionTypeNameIn)
    {
      this.assemblyName = assemblyNameIn;
      this.extensionNamespace = extensionNamespaceIn;
      this.extensionTypeName = extensionTypeNameIn;
      this.extensionType = this.GetExtensionType();
    }

    public System.Type GetExtensionType()
    {
      if (this.extensionType == null)
        this.extensionType = System.Type.GetType(this.extensionNamespace + "." + this.extensionTypeName + ", " + this.assemblyName);
      return this.extensionType;
    }
  }
}
