// Decompiled with JetBrains decompiler
// Type: UnityEditor.TakeInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>A Takeinfo object contains all the information needed to describe a take.</para>
  /// </summary>
  [UsedByNativeCode]
  [NativeType(Header = "Editor/Src/AssetPipeline/ModelImporting/ModelImporter.h")]
  public struct TakeInfo
  {
    /// <summary>
    ///   <para>Take name as define from imported file.</para>
    /// </summary>
    public string name;
    /// <summary>
    ///   <para>This is the default clip name for the clip generated for this take.</para>
    /// </summary>
    public string defaultClipName;
    /// <summary>
    ///   <para>Start time in second.</para>
    /// </summary>
    public float startTime;
    /// <summary>
    ///   <para>Stop time in second.</para>
    /// </summary>
    public float stopTime;
    /// <summary>
    ///   <para>Start time in second.</para>
    /// </summary>
    public float bakeStartTime;
    /// <summary>
    ///   <para>Stop time in second.</para>
    /// </summary>
    public float bakeStopTime;
    /// <summary>
    ///   <para>Sample rate of the take.</para>
    /// </summary>
    public float sampleRate;
  }
}
