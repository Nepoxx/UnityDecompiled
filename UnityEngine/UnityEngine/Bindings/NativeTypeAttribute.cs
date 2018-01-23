// Decompiled with JetBrains decompiler
// Type: UnityEngine.Bindings.NativeTypeAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Bindings
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
  internal class NativeTypeAttribute : Attribute, IBindingsHeaderProviderAttribute, IBindingsGenerateMarshallingTypeAttribute, IBindingsAttribute
  {
    public NativeTypeAttribute()
    {
      this.CodegenOptions = CodegenOptions.Auto;
    }

    public NativeTypeAttribute(CodegenOptions codegenOptions)
    {
      this.CodegenOptions = codegenOptions;
    }

    public NativeTypeAttribute(string header)
    {
      if (header == null)
        throw new ArgumentNullException(nameof (header));
      if (header == "")
        throw new ArgumentException("header cannot be empty", nameof (header));
      this.CodegenOptions = CodegenOptions.Auto;
      this.Header = header;
    }

    public NativeTypeAttribute(string header, CodegenOptions codegenOptions)
      : this(header)
    {
      this.CodegenOptions = codegenOptions;
    }

    public NativeTypeAttribute(CodegenOptions codegenOptions, string intermediateStructName)
      : this(codegenOptions)
    {
      this.IntermediateScriptingStructName = intermediateStructName;
    }

    public string Header { get; set; }

    public string IntermediateScriptingStructName { get; set; }

    public CodegenOptions CodegenOptions { get; set; }
  }
}
