// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildReporting.StrippingInfoWithSizeAnalysis
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityEditor.BuildReporting
{
  internal abstract class StrippingInfoWithSizeAnalysis : StrippingInfo
  {
    protected Dictionary<string, int> folderSizes = new Dictionary<string, int>();
    protected Dictionary<string, int> moduleSizes = new Dictionary<string, int>();
    protected Dictionary<string, int> assemblySizes = new Dictionary<string, int>();
    protected Dictionary<string, int> objectSizes = new Dictionary<string, int>();

    protected abstract Dictionary<string, string> GetModuleArtifacts();

    protected abstract Dictionary<string, string> GetSymbolArtifacts();

    protected abstract Dictionary<string, int> GetFunctionSizes();

    protected abstract void AddPlatformSpecificCodeOutputModules();

    private static Dictionary<string, string> GetIl2CPPAssemblyMapArtifacts(string path)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (string readAllLine in File.ReadAllLines(path))
      {
        char[] chArray = new char[1]{ '\t' };
        string[] strArray = readAllLine.Split(chArray);
        if (strArray.Length == 3)
          dictionary[strArray[0]] = strArray[2];
      }
      return dictionary;
    }

    private static void OutputSizes(Dictionary<string, int> sizes, int totalLines)
    {
      List<KeyValuePair<string, int>> list = sizes.ToList<KeyValuePair<string, int>>();
      list.Sort((Comparison<KeyValuePair<string, int>>) ((firstPair, nextPair) => nextPair.Value.CompareTo(firstPair.Value)));
      foreach (KeyValuePair<string, int> keyValuePair in list)
      {
        if (keyValuePair.Value < 10000)
          break;
        Console.WriteLine(keyValuePair.Value.ToString("D6") + " " + ((double) keyValuePair.Value * 100.0 / (double) totalLines).ToString("F2") + "% " + keyValuePair.Key);
      }
    }

    private static void PrintSizesDictionary(Dictionary<string, int> sizes, int maxSize)
    {
      List<KeyValuePair<string, int>> list = sizes.ToList<KeyValuePair<string, int>>();
      list.Sort((Comparison<KeyValuePair<string, int>>) ((pair1, pair2) => pair2.Value.CompareTo(pair1.Value)));
      for (int index = 0; index < maxSize && index < list.Count; ++index)
        Console.WriteLine(list[index].Value.ToString("D8") + " " + list[index].Key);
    }

    private static void PrintSizesDictionary(Dictionary<string, int> sizes)
    {
      StrippingInfoWithSizeAnalysis.PrintSizesDictionary(sizes, 500);
    }

    public void Analyze()
    {
      bool flag = Unsupported.IsDeveloperBuild();
      Dictionary<string, string> symbolArtifacts = this.GetSymbolArtifacts();
      Dictionary<string, string> moduleArtifacts = this.GetModuleArtifacts();
      Dictionary<string, string> assemblyMapArtifacts = StrippingInfoWithSizeAnalysis.GetIl2CPPAssemblyMapArtifacts("Temp/StagingArea/Data/il2cppOutput/Symbols/MethodMap.tsv");
      int num1 = 0;
      Dictionary<string, int> functionSizes = this.GetFunctionSizes();
      foreach (KeyValuePair<string, int> keyValuePair in functionSizes)
      {
        if (symbolArtifacts.ContainsKey(keyValuePair.Key))
        {
          string key1 = symbolArtifacts[keyValuePair.Key].Replace('\\', '/');
          if (flag)
          {
            if (!this.objectSizes.ContainsKey(key1))
              this.objectSizes[key1] = 0;
            Dictionary<string, int> objectSizes;
            string index;
            (objectSizes = this.objectSizes)[index = key1] = objectSizes[index] + keyValuePair.Value;
          }
          if (key1.LastIndexOf('/') != -1)
          {
            string key2 = key1.Substring(0, key1.LastIndexOf('/'));
            if (!this.folderSizes.ContainsKey(key2))
              this.folderSizes[key2] = 0;
            Dictionary<string, int> folderSizes;
            string index;
            (folderSizes = this.folderSizes)[index = key2] = folderSizes[index] + keyValuePair.Value;
          }
        }
        if (moduleArtifacts.ContainsKey(keyValuePair.Key))
        {
          string str = moduleArtifacts[keyValuePair.Key];
          string key = StrippingInfo.ModuleName(str.Substring(0, str.Length - "Module_Dynamic.bc".Length));
          if (!this.moduleSizes.ContainsKey(key))
            this.moduleSizes[key] = 0;
          Dictionary<string, int> moduleSizes;
          string index;
          (moduleSizes = this.moduleSizes)[index = key] = moduleSizes[index] + keyValuePair.Value;
          num1 += keyValuePair.Value;
        }
        if (assemblyMapArtifacts.ContainsKey(keyValuePair.Key))
        {
          string key = assemblyMapArtifacts[keyValuePair.Key];
          if (!this.assemblySizes.ContainsKey(key))
            this.assemblySizes[key] = 0;
          Dictionary<string, int> assemblySizes;
          string index;
          (assemblySizes = this.assemblySizes)[index = key] = assemblySizes[index] + keyValuePair.Value;
        }
      }
      this.AddPlatformSpecificCodeOutputModules();
      int totalSize = this.totalSize;
      foreach (KeyValuePair<string, int> moduleSize in this.moduleSizes)
      {
        if (this.modules.Contains(moduleSize.Key))
          totalSize -= moduleSize.Value;
      }
      this.moduleSizes["Unaccounted"] = totalSize;
      this.AddModule("Unaccounted");
      foreach (KeyValuePair<string, int> moduleSize in this.moduleSizes)
        this.AddModuleSize(moduleSize.Key, moduleSize.Value);
      int num2 = 0;
      foreach (KeyValuePair<string, int> assemblySize in this.assemblySizes)
      {
        this.RegisterDependency("IL2CPP Generated", assemblySize.Key);
        this.sizes[assemblySize.Key] = assemblySize.Value;
        num2 += assemblySize.Value;
        this.SetIcon(assemblySize.Key, "class/DefaultAsset");
      }
      this.RegisterDependency("IL2CPP Generated", "IL2CPP Unaccounted");
      this.sizes["IL2CPP Unaccounted"] = this.moduleSizes["IL2CPP Generated"] - num2;
      this.SetIcon("IL2CPP Unaccounted", "class/DefaultAsset");
      if (!flag)
        return;
      Console.WriteLine("Code size per module: ");
      StrippingInfoWithSizeAnalysis.PrintSizesDictionary(this.moduleSizes);
      Console.WriteLine("\n\n");
      Console.WriteLine("Code size per source folder: ");
      StrippingInfoWithSizeAnalysis.PrintSizesDictionary(this.folderSizes);
      Console.WriteLine("\n\n");
      Console.WriteLine("Code size per object file: ");
      StrippingInfoWithSizeAnalysis.PrintSizesDictionary(this.objectSizes);
      Console.WriteLine("\n\n");
      Console.WriteLine("Code size per function: ");
      StrippingInfoWithSizeAnalysis.PrintSizesDictionary(functionSizes);
      Console.WriteLine("\n\n");
    }
  }
}
