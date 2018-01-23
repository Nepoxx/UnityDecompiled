// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.Build.AssetBundle.BundleBuildInterface
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEditor.Experimental.Build.Player;

namespace UnityEditor.Experimental.Build.AssetBundle
{
  public class BundleBuildInterface
  {
    public static BuildInput GenerateBuildInput()
    {
      BuildInput ret;
      BundleBuildInterface.GenerateBuildInput_Injected(out ret);
      return ret;
    }

    public static SceneLoadInfo PrepareScene(string scenePath, BuildSettings settings, string outputFolder)
    {
      SceneLoadInfo ret;
      BundleBuildInterface.PrepareScene_Injected(scenePath, ref settings, outputFolder, out ret);
      return ret;
    }

    public static ObjectIdentifier[] GetPlayerObjectIdentifiersInAsset(GUID asset, BuildTarget target)
    {
      return BundleBuildInterface.GetPlayerObjectIdentifiersInAsset_Injected(ref asset, target);
    }

    public static ObjectIdentifier[] GetPlayerDependenciesForObject(ObjectIdentifier objectID, BuildTarget target, TypeDB typeDB)
    {
      return BundleBuildInterface.GetPlayerDependenciesForObject_Injected(ref objectID, target, typeDB);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ObjectIdentifier[] GetPlayerDependenciesForObjects(ObjectIdentifier[] objectIDs, BuildTarget target, TypeDB typeDB);

    public static System.Type GetTypeForObject(ObjectIdentifier objectID)
    {
      return BundleBuildInterface.GetTypeForObject_Injected(ref objectID);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern System.Type[] GetTypeForObjects(ObjectIdentifier[] objectIDs);

    public static BuildOutput WriteResourceFilesForBundle(BuildCommandSet commands, string bundleName, BuildSettings settings, string outputFolder)
    {
      BuildOutput ret;
      BundleBuildInterface.WriteResourceFilesForBundle_Injected(ref commands, bundleName, ref settings, outputFolder, out ret);
      return ret;
    }

    public static BuildOutput WriteResourceFilesForBundles(BuildCommandSet commands, string[] bundleNames, BuildSettings settings, string outputFolder)
    {
      BuildOutput ret;
      BundleBuildInterface.WriteResourceFilesForBundles_Injected(ref commands, bundleNames, ref settings, outputFolder, out ret);
      return ret;
    }

    public static BuildOutput WriteAllResourceFiles(BuildCommandSet commands, BuildSettings settings, string outputFolder)
    {
      BuildOutput ret;
      BundleBuildInterface.WriteAllResourceFiles_Injected(ref commands, ref settings, outputFolder, out ret);
      return ret;
    }

    public static uint ArchiveAndCompress(ResourceFile[] resourceFiles, string outputBundlePath, BuildCompression compression)
    {
      return BundleBuildInterface.ArchiveAndCompress_Injected(resourceFiles, outputBundlePath, ref compression);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GenerateBuildInput_Injected(out BuildInput ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void PrepareScene_Injected(string scenePath, ref BuildSettings settings, string outputFolder, out SceneLoadInfo ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern ObjectIdentifier[] GetPlayerObjectIdentifiersInAsset_Injected(ref GUID asset, BuildTarget target);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern ObjectIdentifier[] GetPlayerDependenciesForObject_Injected(ref ObjectIdentifier objectID, BuildTarget target, TypeDB typeDB);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern System.Type GetTypeForObject_Injected(ref ObjectIdentifier objectID);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void WriteResourceFilesForBundle_Injected(ref BuildCommandSet commands, string bundleName, ref BuildSettings settings, string outputFolder, out BuildOutput ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void WriteResourceFilesForBundles_Injected(ref BuildCommandSet commands, string[] bundleNames, ref BuildSettings settings, string outputFolder, out BuildOutput ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void WriteAllResourceFiles_Injected(ref BuildCommandSet commands, ref BuildSettings settings, string outputFolder, out BuildOutput ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern uint ArchiveAndCompress_Injected(ResourceFile[] resourceFiles, string outputBundlePath, ref BuildCompression compression);
  }
}
