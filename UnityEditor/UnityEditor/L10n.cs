// Decompiled with JetBrains decompiler
// Type: UnityEditor.L10n
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  internal class L10n
  {
    private L10n()
    {
    }

    public static string Tr(string str)
    {
      return LocalizationDatabase.GetLocalizedString(str);
    }
  }
}
