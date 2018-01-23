// Decompiled with JetBrains decompiler
// Type: UnityEngine.Shader
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Shader scripts used for all rendering.</para>
  /// </summary>
  public sealed class Shader : Object
  {
    /// <summary>
    ///   <para>Finds a shader with the given name.</para>
    /// </summary>
    /// <param name="name"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Shader Find(string name);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Shader FindBuiltin(string name);

    /// <summary>
    ///   <para>Can this shader run on the end-users graphics card? (Read Only)</para>
    /// </summary>
    public extern bool isSupported { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal extern string customEditor { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Set a global shader keyword.</para>
    /// </summary>
    /// <param name="keyword"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void EnableKeyword(string keyword);

    /// <summary>
    ///   <para>Unset a global shader keyword.</para>
    /// </summary>
    /// <param name="keyword"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DisableKeyword(string keyword);

    /// <summary>
    ///   <para>Is global shader keyword enabled?</para>
    /// </summary>
    /// <param name="keyword"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsKeywordEnabled(string keyword);

    /// <summary>
    ///   <para>Shader LOD level for this shader.</para>
    /// </summary>
    public extern int maximumLOD { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Shader LOD level for all shaders.</para>
    /// </summary>
    public static extern int globalMaximumLOD { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Render pipeline currently in use.</para>
    /// </summary>
    public static extern string globalRenderPipeline { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Render queue of this shader. (Read Only)</para>
    /// </summary>
    public extern int renderQueue { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal extern DisableBatchingType disableBatching { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetGlobalFloatImpl(int nameID, float value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetGlobalIntImpl(int nameID, int value);

    private static void SetGlobalVectorImpl(int nameID, Vector4 value)
    {
      Shader.INTERNAL_CALL_SetGlobalVectorImpl(nameID, ref value);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetGlobalVectorImpl(int nameID, ref Vector4 value);

    private static void SetGlobalColorImpl(int nameID, Color value)
    {
      Shader.INTERNAL_CALL_SetGlobalColorImpl(nameID, ref value);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetGlobalColorImpl(int nameID, ref Color value);

    private static void SetGlobalMatrixImpl(int nameID, Matrix4x4 value)
    {
      Shader.INTERNAL_CALL_SetGlobalMatrixImpl(nameID, ref value);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetGlobalMatrixImpl(int nameID, ref Matrix4x4 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetGlobalTextureImpl(int nameID, Texture value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetGlobalFloatArrayImpl(int nameID, float[] values, int count);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetGlobalVectorArrayImpl(int nameID, Vector4[] values, int count);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetGlobalMatrixArrayImpl(int nameID, Matrix4x4[] values, int count);

    /// <summary>
    ///   <para>Sets a global compute buffer property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="buffer"></param>
    /// <param name="nameID"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetGlobalBuffer(int nameID, ComputeBuffer buffer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float GetGlobalFloatImpl(int nameID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetGlobalIntImpl(int nameID);

    private static Vector4 GetGlobalVectorImpl(int nameID)
    {
      Vector4 vector4;
      Shader.INTERNAL_CALL_GetGlobalVectorImpl(nameID, out vector4);
      return vector4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetGlobalVectorImpl(int nameID, out Vector4 value);

    private static Color GetGlobalColorImpl(int nameID)
    {
      Color color;
      Shader.INTERNAL_CALL_GetGlobalColorImpl(nameID, out color);
      return color;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetGlobalColorImpl(int nameID, out Color value);

    private static Matrix4x4 GetGlobalMatrixImpl(int nameID)
    {
      Matrix4x4 matrix4x4;
      Shader.INTERNAL_CALL_GetGlobalMatrixImpl(nameID, out matrix4x4);
      return matrix4x4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetGlobalMatrixImpl(int nameID, out Matrix4x4 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Texture GetGlobalTextureImpl(int nameID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float[] GetGlobalFloatArrayImpl(int nameID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Vector4[] GetGlobalVectorArrayImpl(int nameID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Matrix4x4[] GetGlobalMatrixArrayImpl(int nameID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetGlobalFloatArrayImplList(int nameID, object list);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetGlobalVectorArrayImplList(int nameID, object list);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetGlobalMatrixArrayImplList(int nameID, object list);

    /// <summary>
    ///   <para>Gets unique identifier for a shader property name.</para>
    /// </summary>
    /// <param name="name">Shader property name.</param>
    /// <returns>
    ///   <para>Unique integer for the name.</para>
    /// </returns>
    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int PropertyToID(string name);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string IDToProperty(int id);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int TagToID(string name);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string IDToTag(int id);

    /// <summary>
    ///   <para>Fully load all shaders to prevent future performance hiccups.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void WarmupAllShaders();

    /// <summary>
    ///   <para>Sets a global float property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalFloat(string name, float value)
    {
      Shader.SetGlobalFloat(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a global float property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalFloat(int nameID, float value)
    {
      Shader.SetGlobalFloatImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets a global int property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalInt(string name, int value)
    {
      Shader.SetGlobalInt(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a global int property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalInt(int nameID, int value)
    {
      Shader.SetGlobalIntImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets a global vector property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalVector(string name, Vector4 value)
    {
      Shader.SetGlobalVector(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a global vector property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalVector(int nameID, Vector4 value)
    {
      Shader.SetGlobalVectorImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets a global color property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalColor(string name, Color value)
    {
      Shader.SetGlobalColor(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a global color property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalColor(int nameID, Color value)
    {
      Shader.SetGlobalColorImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets a global matrix property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalMatrix(string name, Matrix4x4 value)
    {
      Shader.SetGlobalMatrix(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a global matrix property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalMatrix(int nameID, Matrix4x4 value)
    {
      Shader.SetGlobalMatrixImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets a global texture property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalTexture(string name, Texture value)
    {
      Shader.SetGlobalTexture(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a global texture property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalTexture(int nameID, Texture value)
    {
      Shader.SetGlobalTextureImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets a global compute buffer property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="buffer"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalBuffer(string name, ComputeBuffer buffer)
    {
      Shader.SetGlobalBuffer(Shader.PropertyToID(name), buffer);
    }

    public static void SetGlobalFloatArray(string name, List<float> values)
    {
      Shader.SetGlobalFloatArray(Shader.PropertyToID(name), values);
    }

    public static void SetGlobalFloatArray(int nameID, List<float> values)
    {
      Shader.SetGlobalFloatArray(nameID, (float[]) NoAllocHelpers.ExtractArrayFromList((object) values), values.Count);
    }

    /// <summary>
    ///   <para>Sets a global float array property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="values"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalFloatArray(string name, float[] values)
    {
      Shader.SetGlobalFloatArray(Shader.PropertyToID(name), values);
    }

    /// <summary>
    ///   <para>Sets a global float array property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="values"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalFloatArray(int nameID, float[] values)
    {
      Shader.SetGlobalFloatArray(nameID, values, values.Length);
    }

    private static void SetGlobalFloatArray(int nameID, float[] values, int count)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      if (values.Length == 0)
        throw new ArgumentException("Zero-sized array is not allowed.");
      if (values.Length < count)
        throw new ArgumentException("array has less elements than passed count.");
      Shader.SetGlobalFloatArrayImpl(nameID, values, count);
    }

    public static void SetGlobalVectorArray(string name, List<Vector4> values)
    {
      Shader.SetGlobalVectorArray(Shader.PropertyToID(name), values);
    }

    public static void SetGlobalVectorArray(int nameID, List<Vector4> values)
    {
      Shader.SetGlobalVectorArray(nameID, (Vector4[]) NoAllocHelpers.ExtractArrayFromList((object) values), values.Count);
    }

    /// <summary>
    ///   <para>Sets a global vector array property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="values"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalVectorArray(string name, Vector4[] values)
    {
      Shader.SetGlobalVectorArray(Shader.PropertyToID(name), values);
    }

    /// <summary>
    ///   <para>Sets a global vector array property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="values"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalVectorArray(int nameID, Vector4[] values)
    {
      Shader.SetGlobalVectorArray(nameID, values, values.Length);
    }

    private static void SetGlobalVectorArray(int nameID, Vector4[] values, int count)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      if (values.Length == 0)
        throw new ArgumentException("Zero-sized array is not allowed.");
      if (values.Length < count)
        throw new ArgumentException("array has less elements than passed count.");
      Shader.SetGlobalVectorArrayImpl(nameID, values, count);
    }

    public static void SetGlobalMatrixArray(string name, List<Matrix4x4> values)
    {
      Shader.SetGlobalMatrixArray(Shader.PropertyToID(name), values);
    }

    public static void SetGlobalMatrixArray(int nameID, List<Matrix4x4> values)
    {
      Shader.SetGlobalMatrixArray(nameID, (Matrix4x4[]) NoAllocHelpers.ExtractArrayFromList((object) values), values.Count);
    }

    /// <summary>
    ///   <para>Sets a global matrix array property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="values"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalMatrixArray(string name, Matrix4x4[] values)
    {
      Shader.SetGlobalMatrixArray(Shader.PropertyToID(name), values);
    }

    /// <summary>
    ///   <para>Sets a global matrix array property for all shaders.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="values"></param>
    /// <param name="nameID"></param>
    public static void SetGlobalMatrixArray(int nameID, Matrix4x4[] values)
    {
      Shader.SetGlobalMatrixArray(nameID, values, values.Length);
    }

    private static void SetGlobalMatrixArray(int nameID, Matrix4x4[] values, int count)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      if (values.Length == 0)
        throw new ArgumentException("Zero-sized array is not allowed.");
      if (values.Length < count)
        throw new ArgumentException("array has less elements than passed count.");
      Shader.SetGlobalMatrixArrayImpl(nameID, values, count);
    }

    /// <summary>
    ///   <para>Gets a global float property for all shaders previously set using SetGlobalFloat.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static float GetGlobalFloat(string name)
    {
      return Shader.GetGlobalFloat(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Gets a global float property for all shaders previously set using SetGlobalFloat.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static float GetGlobalFloat(int nameID)
    {
      return Shader.GetGlobalFloatImpl(nameID);
    }

    /// <summary>
    ///   <para>Gets a global int property for all shaders previously set using SetGlobalInt.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static int GetGlobalInt(string name)
    {
      return Shader.GetGlobalInt(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Gets a global int property for all shaders previously set using SetGlobalInt.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static int GetGlobalInt(int nameID)
    {
      return Shader.GetGlobalIntImpl(nameID);
    }

    /// <summary>
    ///   <para>Gets a global vector property for all shaders previously set using SetGlobalVector.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static Vector4 GetGlobalVector(string name)
    {
      return Shader.GetGlobalVector(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Gets a global vector property for all shaders previously set using SetGlobalVector.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static Vector4 GetGlobalVector(int nameID)
    {
      return Shader.GetGlobalVectorImpl(nameID);
    }

    /// <summary>
    ///   <para>Gets a global color property for all shaders previously set using SetGlobalColor.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static Color GetGlobalColor(string name)
    {
      return Shader.GetGlobalColor(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Gets a global color property for all shaders previously set using SetGlobalColor.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static Color GetGlobalColor(int nameID)
    {
      return Shader.GetGlobalColorImpl(nameID);
    }

    /// <summary>
    ///   <para>Gets a global matrix property for all shaders previously set using SetGlobalMatrix.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static Matrix4x4 GetGlobalMatrix(string name)
    {
      return Shader.GetGlobalMatrix(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Gets a global matrix property for all shaders previously set using SetGlobalMatrix.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static Matrix4x4 GetGlobalMatrix(int nameID)
    {
      return Shader.GetGlobalMatrixImpl(nameID);
    }

    /// <summary>
    ///   <para>Gets a global texture property for all shaders previously set using SetGlobalTexture.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static Texture GetGlobalTexture(string name)
    {
      return Shader.GetGlobalTexture(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Gets a global texture property for all shaders previously set using SetGlobalTexture.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static Texture GetGlobalTexture(int nameID)
    {
      return Shader.GetGlobalTextureImpl(nameID);
    }

    public static void GetGlobalFloatArray(string name, List<float> values)
    {
      Shader.GetGlobalFloatArray(Shader.PropertyToID(name), values);
    }

    public static void GetGlobalFloatArray(int nameID, List<float> values)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      Shader.GetGlobalFloatArrayImplList(nameID, (object) values);
    }

    /// <summary>
    ///   <para>Gets a global float array for all shaders previously set using SetGlobalFloatArray.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static float[] GetGlobalFloatArray(string name)
    {
      return Shader.GetGlobalFloatArray(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Gets a global float array for all shaders previously set using SetGlobalFloatArray.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static float[] GetGlobalFloatArray(int nameID)
    {
      return Shader.GetGlobalFloatArrayImpl(nameID);
    }

    public static void GetGlobalVectorArray(string name, List<Vector4> values)
    {
      Shader.GetGlobalVectorArray(Shader.PropertyToID(name), values);
    }

    public static void GetGlobalVectorArray(int nameID, List<Vector4> values)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      Shader.GetGlobalVectorArrayImplList(nameID, (object) values);
    }

    /// <summary>
    ///   <para>Gets a global vector array for all shaders previously set using SetGlobalVectorArray.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static Vector4[] GetGlobalVectorArray(string name)
    {
      return Shader.GetGlobalVectorArray(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Gets a global vector array for all shaders previously set using SetGlobalVectorArray.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static Vector4[] GetGlobalVectorArray(int nameID)
    {
      return Shader.GetGlobalVectorArrayImpl(nameID);
    }

    public static void GetGlobalMatrixArray(string name, List<Matrix4x4> values)
    {
      Shader.GetGlobalMatrixArray(Shader.PropertyToID(name), values);
    }

    public static void GetGlobalMatrixArray(int nameID, List<Matrix4x4> values)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      Shader.GetGlobalMatrixArrayImplList(nameID, (object) values);
    }

    /// <summary>
    ///   <para>Gets a global matrix array for all shaders previously set using SetGlobalMatrixArray.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static Matrix4x4[] GetGlobalMatrixArray(string name)
    {
      return Shader.GetGlobalMatrixArray(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Gets a global matrix array for all shaders previously set using SetGlobalMatrixArray.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameID"></param>
    public static Matrix4x4[] GetGlobalMatrixArray(int nameID)
    {
      return Shader.GetGlobalMatrixArrayImpl(nameID);
    }

    [Obsolete("SetGlobalTexGenMode is not supported anymore. Use programmable shaders to achieve the same effect.", true)]
    public static void SetGlobalTexGenMode(string propertyName, TexGenMode mode)
    {
    }

    [Obsolete("SetGlobalTextureMatrixName is not supported anymore. Use programmable shaders to achieve the same effect.", true)]
    public static void SetGlobalTextureMatrixName(string propertyName, string matrixName)
    {
    }

    /// <summary>
    ///   <para>Shader hardware tier classification for current device.</para>
    /// </summary>
    [Obsolete("Use Graphics.activeTier instead (UnityUpgradable) -> UnityEngine.Graphics.activeTier", false)]
    public static ShaderHardwareTier globalShaderHardwareTier
    {
      get
      {
        return (ShaderHardwareTier) Graphics.activeTier;
      }
      set
      {
        Graphics.activeTier = (GraphicsTier) value;
      }
    }
  }
}
