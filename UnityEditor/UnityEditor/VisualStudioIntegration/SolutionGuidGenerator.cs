// Decompiled with JetBrains decompiler
// Type: UnityEditor.VisualStudioIntegration.SolutionGuidGenerator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Security.Cryptography;
using System.Text;

namespace UnityEditor.VisualStudioIntegration
{
  public static class SolutionGuidGenerator
  {
    public static string GuidForProject(string projectName)
    {
      return SolutionGuidGenerator.ComputeGuidHashFor(projectName + "salt");
    }

    public static string GuidForSolution(string projectName, string sourceFileExtension)
    {
      if (sourceFileExtension.ToLower() == "cs")
        return "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
      return SolutionGuidGenerator.ComputeGuidHashFor(projectName);
    }

    private static string ComputeGuidHashFor(string input)
    {
      return SolutionGuidGenerator.HashAsGuid(SolutionGuidGenerator.HashToString(MD5.Create().ComputeHash(Encoding.Default.GetBytes(input))));
    }

    private static string HashAsGuid(string hash)
    {
      return (hash.Substring(0, 8) + "-" + hash.Substring(8, 4) + "-" + hash.Substring(12, 4) + "-" + hash.Substring(16, 4) + "-" + hash.Substring(20, 12)).ToUpper();
    }

    private static string HashToString(byte[] bs)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte b in bs)
        stringBuilder.Append(b.ToString("x2"));
      return stringBuilder.ToString();
    }
  }
}
