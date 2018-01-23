// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.DefaultBuildWindowExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Modules
{
  internal class DefaultBuildWindowExtension : IBuildWindowExtension
  {
    public virtual void ShowPlatformBuildOptions()
    {
    }

    public virtual void ShowInternalPlatformBuildOptions()
    {
    }

    public virtual bool EnabledBuildButton()
    {
      return true;
    }

    public virtual bool EnabledBuildAndRunButton()
    {
      return true;
    }

    public virtual bool ShouldDrawScriptDebuggingCheckbox()
    {
      return true;
    }

    public virtual bool ShouldDrawProfilerCheckbox()
    {
      return true;
    }

    public virtual bool ShouldDrawDevelopmentPlayerCheckbox()
    {
      return true;
    }

    public virtual bool ShouldDrawExplicitNullCheckbox()
    {
      return false;
    }

    public virtual bool ShouldDrawExplicitDivideByZeroCheckbox()
    {
      return false;
    }

    public virtual bool ShouldDrawForceOptimizeScriptsCheckbox()
    {
      return false;
    }
  }
}
