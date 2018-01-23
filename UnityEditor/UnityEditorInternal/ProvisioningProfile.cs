// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProvisioningProfile
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.IO;
using System.Text.RegularExpressions;

namespace UnityEditorInternal
{
  internal class ProvisioningProfile
  {
    private static readonly string s_FirstLinePattern = "<key>UUID<\\/key>";
    private static readonly string s_SecondLinePattern = "<string>((\\w*\\-?){5})";
    private string m_UUID = string.Empty;

    internal ProvisioningProfile()
    {
    }

    internal ProvisioningProfile(string UUID)
    {
      this.m_UUID = UUID;
    }

    public string UUID
    {
      get
      {
        return this.m_UUID;
      }
      set
      {
        this.m_UUID = value;
      }
    }

    internal static ProvisioningProfile ParseProvisioningProfileAtPath(string pathToFile)
    {
      ProvisioningProfile profile = new ProvisioningProfile();
      ProvisioningProfile.parseFile(pathToFile, profile);
      return profile;
    }

    private static void parseFile(string filePath, ProvisioningProfile profile)
    {
      StreamReader streamReader = new StreamReader(filePath);
      string input1;
      while ((input1 = streamReader.ReadLine()) != null)
      {
        string input2;
        if (Regex.Match(input1, ProvisioningProfile.s_FirstLinePattern).Success && (input2 = streamReader.ReadLine()) != null)
        {
          Match match = Regex.Match(input2, ProvisioningProfile.s_SecondLinePattern);
          if (match.Success)
          {
            profile.UUID = match.Groups[1].Value;
            break;
          }
        }
        if (!string.IsNullOrEmpty(profile.UUID))
          break;
      }
      streamReader.Close();
    }
  }
}
