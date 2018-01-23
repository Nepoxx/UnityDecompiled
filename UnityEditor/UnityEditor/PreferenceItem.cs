// Decompiled with JetBrains decompiler
// Type: UnityEditor.PreferenceItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>The PreferenceItem attribute allows you to add preferences sections to the Preferences Window.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public sealed class PreferenceItem : Attribute
  {
    public string name;

    /// <summary>
    ///   <para>Creates a section in the Preferences Window called name and invokes the static function following it for the section's GUI.</para>
    /// </summary>
    /// <param name="name"></param>
    public PreferenceItem(string name)
    {
      this.name = name;
    }

    [RequiredSignature]
    private static void signature()
    {
    }
  }
}
