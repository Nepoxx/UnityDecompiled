// Decompiled with JetBrains decompiler
// Type: UnityEditor.MovieImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>AssetImporter for importing MovieTextures.</para>
  /// </summary>
  public sealed class MovieImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Quality setting to use when importing the movie. This is a float value from 0 to 1.</para>
    /// </summary>
    public extern float quality { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is the movie texture storing non-color data?</para>
    /// </summary>
    public extern bool linearTexture { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Duration of the Movie to be imported in seconds.</para>
    /// </summary>
    public extern float duration { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
