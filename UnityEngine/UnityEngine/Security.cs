// Decompiled with JetBrains decompiler
// Type: UnityEngine.Security
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Webplayer security related class. Not supported from 5.4.0 onwards.</para>
  /// </summary>
  public sealed class Security
  {
    private static readonly string kSignatureExtension = ".signature";

    /// <summary>
    ///   <para>Prefetch the webplayer socket security policy from a non-default port number.</para>
    /// </summary>
    /// <param name="ip">IP address of server.</param>
    /// <param name="atPort">Port from where socket policy is read.</param>
    /// <param name="timeout">Time to wait for response.</param>
    [Obsolete("Security.PrefetchSocketPolicy is no longer supported, since the Unity Web Player is no longer supported by Unity.", true)]
    [ExcludeFromDocs]
    public static bool PrefetchSocketPolicy(string ip, int atPort)
    {
      int timeout = 3000;
      return UnityEngine.Security.PrefetchSocketPolicy(ip, atPort, timeout);
    }

    /// <summary>
    ///   <para>Prefetch the webplayer socket security policy from a non-default port number.</para>
    /// </summary>
    /// <param name="ip">IP address of server.</param>
    /// <param name="atPort">Port from where socket policy is read.</param>
    /// <param name="timeout">Time to wait for response.</param>
    [Obsolete("Security.PrefetchSocketPolicy is no longer supported, since the Unity Web Player is no longer supported by Unity.", true)]
    public static bool PrefetchSocketPolicy(string ip, int atPort, [DefaultValue("3000")] int timeout)
    {
      return false;
    }

    [RequiredByNativeCode]
    internal static bool VerifySignature(string file, byte[] publicKey)
    {
      try
      {
        string path = file + UnityEngine.Security.kSignatureExtension;
        if (!File.Exists(path))
          return false;
        using (RSACryptoServiceProvider cryptoServiceProvider1 = new RSACryptoServiceProvider())
        {
          cryptoServiceProvider1.ImportCspBlob(publicKey);
          using (SHA1CryptoServiceProvider cryptoServiceProvider2 = new SHA1CryptoServiceProvider())
            return cryptoServiceProvider1.VerifyData(File.ReadAllBytes(file), (object) cryptoServiceProvider2, File.ReadAllBytes(path));
        }
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
      return false;
    }

    /// <summary>
    ///   <para>Loads an assembly and checks that it is allowed to be used in the webplayer. (Web Player is no Longer Supported).</para>
    /// </summary>
    /// <param name="assemblyData">Assembly to verify.</param>
    /// <param name="authorizationKey">Public key used to verify assembly.</param>
    /// <returns>
    ///   <para>Loaded, verified, assembly, or null if the assembly cannot be verfied.</para>
    /// </returns>
    [Obsolete("This was an internal method which is no longer used", true)]
    public static Assembly LoadAndVerifyAssembly(byte[] assemblyData, string authorizationKey)
    {
      return (Assembly) null;
    }

    /// <summary>
    ///   <para>Loads an assembly and checks that it is allowed to be used in the webplayer. (Web Player is no Longer Supported).</para>
    /// </summary>
    /// <param name="assemblyData">Assembly to verify.</param>
    /// <param name="authorizationKey">Public key used to verify assembly.</param>
    /// <returns>
    ///   <para>Loaded, verified, assembly, or null if the assembly cannot be verfied.</para>
    /// </returns>
    [Obsolete("This was an internal method which is no longer used", true)]
    public static Assembly LoadAndVerifyAssembly(byte[] assemblyData)
    {
      return (Assembly) null;
    }
  }
}
