// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.NuGetPackageResolver
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.Scripting.Compilers
{
  internal sealed class NuGetPackageResolver
  {
    public NuGetPackageResolver()
    {
      this.TargetMoniker = "UAP,Version=v10.0";
    }

    public string PackagesDirectory { get; set; }

    public string ProjectLockFile { get; set; }

    public string TargetMoniker { get; set; }

    public string[] ResolvedReferences { get; private set; }

    private string ConvertToWindowsPath(string path)
    {
      return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
    }

    public string[] Resolve()
    {
      Dictionary<string, object> uwpTarget = this.FindUWPTarget((Dictionary<string, object>) ((Dictionary<string, object>) Json.Deserialize(File.ReadAllText(this.ProjectLockFile)))["targets"]);
      List<string> stringList = new List<string>();
      string windowsPath = this.ConvertToWindowsPath(this.GetPackagesPath());
      foreach (KeyValuePair<string, object> keyValuePair in uwpTarget)
      {
        Dictionary<string, object> dictionary1 = (Dictionary<string, object>) keyValuePair.Value;
        object obj;
        if (dictionary1.TryGetValue("compile", out obj))
        {
          Dictionary<string, object> dictionary2 = (Dictionary<string, object>) obj;
          string[] strArray = keyValuePair.Key.Split('/');
          string path2_1 = strArray[0];
          string path2_2 = strArray[1];
          string str = Path.Combine(Path.Combine(windowsPath, path2_1), path2_2);
          if (!Directory.Exists(str))
            throw new Exception(string.Format("Package directory not found: \"{0}\".", (object) str));
          foreach (string key in dictionary2.Keys)
          {
            if (!string.Equals(Path.GetFileName(key), "_._", StringComparison.InvariantCultureIgnoreCase))
            {
              string path = Path.Combine(str, this.ConvertToWindowsPath(key));
              if (!File.Exists(path))
                throw new Exception(string.Format("Reference not found: \"{0}\".", (object) path));
              stringList.Add(path);
            }
          }
          if (dictionary1.ContainsKey("frameworkAssemblies"))
            throw new NotImplementedException("Support for \"frameworkAssemblies\" property has not been implemented yet.");
        }
      }
      this.ResolvedReferences = stringList.ToArray();
      return this.ResolvedReferences;
    }

    private Dictionary<string, object> FindUWPTarget(Dictionary<string, object> targets)
    {
      foreach (KeyValuePair<string, object> target in targets)
      {
        if (target.Key.StartsWith(this.TargetMoniker) && !target.Key.Contains("/"))
          return (Dictionary<string, object>) target.Value;
      }
      throw new InvalidOperationException("Could not find suitable target for " + this.TargetMoniker + " in project.lock.json file.");
    }

    private string GetPackagesPath()
    {
      string packagesDirectory = this.PackagesDirectory;
      if (!string.IsNullOrEmpty(packagesDirectory))
        return packagesDirectory;
      string environmentVariable = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
      if (!string.IsNullOrEmpty(environmentVariable))
        return environmentVariable;
      return Path.Combine(Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), ".nuget"), "packages");
    }
  }
}
