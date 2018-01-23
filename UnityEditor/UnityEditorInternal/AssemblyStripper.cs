// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AssemblyStripper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Scripting.Compilers;
using UnityEditor.Utils;

namespace UnityEditorInternal
{
  internal class AssemblyStripper
  {
    private static bool debugUnstripped
    {
      get
      {
        return false;
      }
    }

    private static string[] Il2CppBlacklistPaths
    {
      get
      {
        return new string[1]{ Path.Combine("..", "platform_native_link.xml") };
      }
    }

    private static string MonoLinker2Path
    {
      get
      {
        return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "il2cpp/build/UnityLinker.exe");
      }
    }

    private static string GetModuleWhitelist(string module, string moduleStrippingInformationFolder)
    {
      return Paths.Combine(moduleStrippingInformationFolder, module + ".xml");
    }

    private static bool StripAssembliesTo(string[] assemblies, string[] searchDirs, string outputFolder, string workingDirectory, out string output, out string error, string linkerPath, IIl2CppPlatformProvider platformProvider, IEnumerable<string> additionalBlacklist)
    {
      if (!Directory.Exists(outputFolder))
        Directory.CreateDirectory(outputFolder);
      IEnumerable<string> source = additionalBlacklist.Select<string, string>((Func<string, string>) (s => !Path.IsPathRooted(s) ? Path.Combine(workingDirectory, s) : s));
      // ISSUE: reference to a compiler-generated field
      if (AssemblyStripper.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AssemblyStripper.\u003C\u003Ef__mg\u0024cache0 = new Func<string, bool>(File.Exists);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, bool> fMgCache0 = AssemblyStripper.\u003C\u003Ef__mg\u0024cache0;
      additionalBlacklist = source.Where<string>(fMgCache0);
      IEnumerable<string> userBlacklistFiles = AssemblyStripper.GetUserBlacklistFiles();
      foreach (string str in userBlacklistFiles)
        Console.WriteLine("UserBlackList: " + str);
      additionalBlacklist = additionalBlacklist.Concat<string>(userBlacklistFiles);
      List<string> stringList = new List<string>() { "--api=" + PlayerSettings.GetApiCompatibilityLevel(EditorUserBuildSettings.activeBuildTargetGroup).ToString(), "-out=\"" + outputFolder + "\"", "-l=none", "-c=link", "--link-symbols", "-x=\"" + AssemblyStripper.GetModuleWhitelist("Core", platformProvider.moduleStrippingInformationFolder) + "\"", "-f=\"" + Path.Combine(platformProvider.il2CppFolder, "LinkerDescriptors") + "\"" };
      stringList.AddRange(additionalBlacklist.Select<string, string>((Func<string, string>) (path => "-x \"" + path + "\"")));
      stringList.AddRange(((IEnumerable<string>) searchDirs).Select<string, string>((Func<string, string>) (d => "-d \"" + d + "\"")));
      stringList.AddRange(((IEnumerable<string>) assemblies).Select<string, string>((Func<string, string>) (assembly => "-a  \"" + Path.GetFullPath(assembly) + "\"")));
      return AssemblyStripper.RunAssemblyLinker((IEnumerable<string>) stringList, out output, out error, linkerPath, workingDirectory);
    }

    private static bool RunAssemblyLinker(IEnumerable<string> args, out string @out, out string err, string linkerPath, string workingDirectory)
    {
      string args1 = args.Aggregate<string>((Func<string, string, string>) ((buff, s) => buff + " " + s));
      Console.WriteLine("Invoking UnityLinker with arguments: " + args1);
      Runner.RunManagedProgram(linkerPath, args1, workingDirectory, (CompilerOutputParserBase) null, (Action<ProcessStartInfo>) null);
      @out = "";
      err = "";
      return true;
    }

    private static List<string> GetUserAssemblies(RuntimeClassRegistry rcr, string managedDir)
    {
      return ((IEnumerable<string>) rcr.GetUserAssemblies()).Where<string>((Func<string, bool>) (s => rcr.IsDLLUsed(s))).Select<string, string>((Func<string, string>) (s => Path.Combine(managedDir, s))).ToList<string>();
    }

    internal static void StripAssemblies(string stagingAreaData, IIl2CppPlatformProvider platformProvider, RuntimeClassRegistry rcr)
    {
      string fullPath = Path.GetFullPath(Path.Combine(stagingAreaData, "Managed"));
      List<string> userAssemblies = AssemblyStripper.GetUserAssemblies(rcr, fullPath);
      userAssemblies.AddRange((IEnumerable<string>) Directory.GetFiles(fullPath, "I18N*.dll", SearchOption.TopDirectoryOnly));
      string[] array = userAssemblies.ToArray();
      string[] searchDirs = new string[1]{ fullPath };
      AssemblyStripper.RunAssemblyStripper(stagingAreaData, (IEnumerable) userAssemblies, fullPath, array, searchDirs, AssemblyStripper.MonoLinker2Path, platformProvider, rcr);
    }

    internal static void GenerateInternalCallSummaryFile(string icallSummaryPath, string managedAssemblyFolderPath, string strippedDLLPath)
    {
      string exe = Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Tools/InternalCallRegistrationWriter/InternalCallRegistrationWriter.exe");
      IEnumerable<string> source = ((IEnumerable<string>) Directory.GetFiles(strippedDLLPath, "UnityEngine.*Module.dll")).Concat<string>((IEnumerable<string>) new string[1]{ Path.Combine(strippedDLLPath, "UnityEngine.dll") });
      string args = string.Format("-output=\"{0}\" -summary=\"{1}\" -assembly=\"{2}\"", (object) Path.Combine(managedAssemblyFolderPath, "UnityICallRegistration.cpp"), (object) icallSummaryPath, (object) source.Aggregate<string>((Func<string, string, string>) ((dllArg, next) => dllArg + ";" + next)));
      Runner.RunManagedProgram(exe, args);
    }

    internal static IEnumerable<string> GetUserBlacklistFiles()
    {
      return ((IEnumerable<string>) Directory.GetFiles("Assets", "link.xml", SearchOption.AllDirectories)).Select<string, string>((Func<string, string>) (s => Path.Combine(Directory.GetCurrentDirectory(), s)));
    }

    private static bool AddWhiteListsForModules(IEnumerable<string> nativeModules, ref IEnumerable<string> blacklists, string moduleStrippingInformationFolder)
    {
      bool flag = false;
      foreach (string nativeModule in nativeModules)
      {
        string moduleWhitelist = AssemblyStripper.GetModuleWhitelist(nativeModule, moduleStrippingInformationFolder);
        if (File.Exists(moduleWhitelist) && !blacklists.Contains<string>(moduleWhitelist))
        {
          blacklists = blacklists.Concat<string>((IEnumerable<string>) new string[1]
          {
            moduleWhitelist
          });
          flag = true;
        }
      }
      return flag;
    }

    private static void RunAssemblyStripper(string stagingAreaData, IEnumerable assemblies, string managedAssemblyFolderPath, string[] assembliesToStrip, string[] searchDirs, string monoLinkerPath, IIl2CppPlatformProvider platformProvider, RuntimeClassRegistry rcr)
    {
      BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(platformProvider.target);
      bool flag1 = PlayerSettings.GetScriptingBackend(buildTargetGroup) == ScriptingImplementation.Mono2x;
      bool doStripping = rcr != null && PlayerSettings.stripEngineCode && platformProvider.supportsEngineStripping;
      IEnumerable<string> blacklists = (IEnumerable<string>) AssemblyStripper.Il2CppBlacklistPaths;
      if (rcr != null)
        blacklists = blacklists.Concat<string>((IEnumerable<string>) new string[3]
        {
          AssemblyStripper.WriteMethodsToPreserveBlackList(stagingAreaData, rcr, platformProvider.target),
          AssemblyStripper.WriteUnityEngineBlackList(stagingAreaData),
          MonoAssemblyStripping.GenerateLinkXmlToPreserveDerivedTypes(stagingAreaData, managedAssemblyFolderPath, rcr)
        });
      if (PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup) == ApiCompatibilityLevel.NET_4_6)
      {
        string path = Path.Combine(platformProvider.il2CppFolder, "LinkerDescriptors");
        blacklists = blacklists.Concat<string>((IEnumerable<string>) Directory.GetFiles(path, "*45.xml"));
      }
      if (flag1)
      {
        string path1 = Path.Combine(platformProvider.il2CppFolder, "LinkerDescriptors");
        blacklists = blacklists.Concat<string>((IEnumerable<string>) Directory.GetFiles(path1, "*_mono.xml"));
        string path2 = Path.Combine(BuildPipeline.GetBuildToolsDirectory(platformProvider.target), "link.xml");
        if (File.Exists(path2))
          blacklists = blacklists.Concat<string>((IEnumerable<string>) new string[1]
          {
            path2
          });
      }
      if (!doStripping)
      {
        foreach (string file in Directory.GetFiles(platformProvider.moduleStrippingInformationFolder, "*.xml"))
          blacklists = blacklists.Concat<string>((IEnumerable<string>) new string[1]
          {
            file
          });
      }
      string fullPath1 = Path.GetFullPath(Path.Combine(managedAssemblyFolderPath, "tempStrip"));
      bool flag2;
      do
      {
        flag2 = false;
        if (EditorUtility.DisplayCancelableProgressBar("Building Player", "Stripping assemblies", 0.0f))
          throw new OperationCanceledException();
        string output;
        string error;
        if (!AssemblyStripper.StripAssembliesTo(assembliesToStrip, searchDirs, fullPath1, managedAssemblyFolderPath, out output, out error, monoLinkerPath, platformProvider, blacklists))
          throw new Exception("Error in stripping assemblies: " + (object) assemblies + ", " + error);
        if (platformProvider.supportsEngineStripping)
        {
          string str = Path.Combine(managedAssemblyFolderPath, "ICallSummary.txt");
          AssemblyStripper.GenerateInternalCallSummaryFile(str, managedAssemblyFolderPath, fullPath1);
          if (doStripping)
          {
            HashSet<UnityType> nativeClasses;
            HashSet<string> nativeModules;
            CodeStrippingUtils.GenerateDependencies(fullPath1, str, rcr, doStripping, out nativeClasses, out nativeModules, platformProvider);
            flag2 = AssemblyStripper.AddWhiteListsForModules((IEnumerable<string>) nativeModules, ref blacklists, platformProvider.moduleStrippingInformationFolder);
          }
        }
      }
      while (flag2);
      string fullPath2 = Path.GetFullPath(Path.Combine(managedAssemblyFolderPath, "tempUnstripped"));
      if (AssemblyStripper.debugUnstripped)
        Directory.CreateDirectory(fullPath2);
      foreach (string file in Directory.GetFiles(managedAssemblyFolderPath))
      {
        string extension = Path.GetExtension(file);
        if (string.Equals(extension, ".dll", StringComparison.InvariantCultureIgnoreCase) || string.Equals(extension, ".winmd", StringComparison.InvariantCultureIgnoreCase) || (string.Equals(extension, ".mdb", StringComparison.InvariantCultureIgnoreCase) || string.Equals(extension, ".pdb", StringComparison.InvariantCultureIgnoreCase)))
        {
          if (AssemblyStripper.debugUnstripped)
            File.Move(file, Path.Combine(fullPath2, Path.GetFileName(file)));
          else
            File.Delete(file);
        }
      }
      foreach (string file in Directory.GetFiles(fullPath1))
        File.Move(file, Path.Combine(managedAssemblyFolderPath, Path.GetFileName(file)));
      Directory.Delete(fullPath1);
    }

    private static string WriteMethodsToPreserveBlackList(string stagingAreaData, RuntimeClassRegistry rcr, BuildTarget target)
    {
      string path = (!Path.IsPathRooted(stagingAreaData) ? Directory.GetCurrentDirectory() + "/" : "") + stagingAreaData + "/methods_pointedto_by_uievents.xml";
      File.WriteAllText(path, AssemblyStripper.GetMethodPreserveBlacklistContents(rcr, target));
      return path;
    }

    private static string WriteUnityEngineBlackList(string stagingAreaData)
    {
      string path = (!Path.IsPathRooted(stagingAreaData) ? Directory.GetCurrentDirectory() + "/" : "") + stagingAreaData + "/UnityEngine.xml";
      File.WriteAllText(path, "<linker><assembly fullname=\"UnityEngine\" preserve=\"nothing\"/></linker>");
      return path;
    }

    private static string GetMethodPreserveBlacklistContents(RuntimeClassRegistry rcr, BuildTarget target)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("<linker>");
      foreach (IGrouping<string, RuntimeClassRegistry.MethodDescription> source in rcr.GetMethodsToPreserve().GroupBy<RuntimeClassRegistry.MethodDescription, string>((Func<RuntimeClassRegistry.MethodDescription, string>) (m => m.assembly)))
      {
        string key = source.Key;
        if (AssemblyHelper.IsUnityEngineModule(key) && !BuildPipeline.IsFeatureSupported("ENABLE_MODULAR_UNITYENGINE_ASSEMBLIES", target))
          stringBuilder.AppendLine(string.Format("\t<assembly fullname=\"{0}\">", (object) "UnityEngine"));
        else
          stringBuilder.AppendLine(string.Format("\t<assembly fullname=\"{0}\">", (object) key));
        foreach (IGrouping<string, RuntimeClassRegistry.MethodDescription> grouping in source.GroupBy<RuntimeClassRegistry.MethodDescription, string>((Func<RuntimeClassRegistry.MethodDescription, string>) (m => m.fullTypeName)))
        {
          stringBuilder.AppendLine(string.Format("\t\t<type fullname=\"{0}\">", (object) grouping.Key));
          foreach (RuntimeClassRegistry.MethodDescription methodDescription in (IEnumerable<RuntimeClassRegistry.MethodDescription>) grouping)
            stringBuilder.AppendLine(string.Format("\t\t\t<method name=\"{0}\"/>", (object) methodDescription.methodName));
          stringBuilder.AppendLine("\t\t</type>");
        }
        stringBuilder.AppendLine("\t</assembly>");
      }
      stringBuilder.AppendLine("</linker>");
      return stringBuilder.ToString();
    }

    public static void InvokeFromBuildPlayer(BuildTarget buildTarget, RuntimeClassRegistry usedClasses)
    {
      string str = Paths.Combine("Temp", "StagingArea", "Data");
      BaseIl2CppPlatformProvider platformProvider = new BaseIl2CppPlatformProvider(buildTarget, Path.Combine(str, "Libraries"));
      AssemblyStripper.StripAssemblies(str, (IIl2CppPlatformProvider) platformProvider, usedClasses);
    }
  }
}
