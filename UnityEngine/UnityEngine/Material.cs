// Decompiled with JetBrains decompiler
// Type: UnityEngine.Material
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The material class.</para>
  /// </summary>
  public class Material : Object
  {
    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="contents"></param>
    [Obsolete("Creating materials from shader source string is no longer supported. Use Shader assets instead.")]
    public Material(string contents)
    {
      Material.Internal_CreateWithString(this, contents);
    }

    /// <summary>
    ///   <para>Create a temporary Material.</para>
    /// </summary>
    /// <param name="shader">Create a material with a given Shader.</param>
    /// <param name="source">Create a material by copying all properties from another material.</param>
    public Material(Shader shader)
    {
      Material.Internal_CreateWithShader(this, shader);
    }

    /// <summary>
    ///   <para>Create a temporary Material.</para>
    /// </summary>
    /// <param name="shader">Create a material with a given Shader.</param>
    /// <param name="source">Create a material by copying all properties from another material.</param>
    public Material(Material source)
    {
      Material.Internal_CreateWithMaterial(this, source);
    }

    /// <summary>
    ///   <para>The shader used by the material.</para>
    /// </summary>
    public extern Shader shader { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The main material's color.</para>
    /// </summary>
    public Color color
    {
      get
      {
        return this.GetColor("_Color");
      }
      set
      {
        this.SetColor("_Color", value);
      }
    }

    /// <summary>
    ///   <para>The material's texture.</para>
    /// </summary>
    public Texture mainTexture
    {
      get
      {
        return this.GetTexture("_MainTex");
      }
      set
      {
        this.SetTexture("_MainTex", value);
      }
    }

    /// <summary>
    ///   <para>The texture offset of the main texture.</para>
    /// </summary>
    public Vector2 mainTextureOffset
    {
      get
      {
        return this.GetTextureOffset("_MainTex");
      }
      set
      {
        this.SetTextureOffset("_MainTex", value);
      }
    }

    /// <summary>
    ///   <para>The texture scale of the main texture.</para>
    /// </summary>
    public Vector2 mainTextureScale
    {
      get
      {
        return this.GetTextureScale("_MainTex");
      }
      set
      {
        this.SetTextureScale("_MainTex", value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetFloatImpl(int nameID, float value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetIntImpl(int nameID, int value);

    private void SetColorImpl(int nameID, Color value)
    {
      Material.INTERNAL_CALL_SetColorImpl(this, nameID, ref value);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetColorImpl(Material self, int nameID, ref Color value);

    private void SetVectorImpl(int nameID, Vector4 value)
    {
      Material.INTERNAL_CALL_SetVectorImpl(this, nameID, ref value);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetVectorImpl(Material self, int nameID, ref Vector4 value);

    private void SetMatrixImpl(int nameID, Matrix4x4 value)
    {
      Material.INTERNAL_CALL_SetMatrixImpl(this, nameID, ref value);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetMatrixImpl(Material self, int nameID, ref Matrix4x4 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetTextureImpl(int nameID, Texture value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetBufferImpl(int nameID, ComputeBuffer value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetFloatArrayImpl(int nameID, float[] values, int count);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetVectorArrayImpl(int nameID, Vector4[] values, int count);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetMatrixArrayImpl(int nameID, Matrix4x4[] values, int count);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetColorArrayImpl(int nameID, Color[] values, int count);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern float GetFloatImpl(int nameID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int GetIntImpl(int nameID);

    private Color GetColorImpl(int nameID)
    {
      Color color;
      Material.INTERNAL_CALL_GetColorImpl(this, nameID, out color);
      return color;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetColorImpl(Material self, int nameID, out Color value);

    private Vector4 GetVectorImpl(int nameID)
    {
      Vector4 vector4;
      Material.INTERNAL_CALL_GetVectorImpl(this, nameID, out vector4);
      return vector4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetVectorImpl(Material self, int nameID, out Vector4 value);

    private Matrix4x4 GetMatrixImpl(int nameID)
    {
      Matrix4x4 matrix4x4;
      Material.INTERNAL_CALL_GetMatrixImpl(this, nameID, out matrix4x4);
      return matrix4x4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetMatrixImpl(Material self, int nameID, out Matrix4x4 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Texture GetTextureImpl(int nameID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern float[] GetFloatArrayImpl(int nameID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Vector4[] GetVectorArrayImpl(int nameID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Matrix4x4[] GetMatrixArrayImpl(int nameID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetFloatArrayImplList(int nameID, object list);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetVectorArrayImplList(int nameID, object list);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetMatrixArrayImplList(int nameID, object list);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Color[] GetColorArrayImpl(int nameID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetColorArrayImplList(int nameID, object list);

    private Vector4 GetTextureScaleAndOffsetImpl(int nameID)
    {
      Vector4 vector4;
      Material.INTERNAL_CALL_GetTextureScaleAndOffsetImpl(this, nameID, out vector4);
      return vector4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetTextureScaleAndOffsetImpl(Material self, int nameID, out Vector4 value);

    private void SetTextureOffsetImpl(int nameID, Vector2 offset)
    {
      Material.INTERNAL_CALL_SetTextureOffsetImpl(this, nameID, ref offset);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetTextureOffsetImpl(Material self, int nameID, ref Vector2 offset);

    private void SetTextureScaleImpl(int nameID, Vector2 scale)
    {
      Material.INTERNAL_CALL_SetTextureScaleImpl(this, nameID, ref scale);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetTextureScaleImpl(Material self, int nameID, ref Vector2 scale);

    /// <summary>
    ///   <para>Checks if material's shader has a property of a given name.</para>
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="nameID"></param>
    public bool HasProperty(string propertyName)
    {
      return this.HasProperty(Shader.PropertyToID(propertyName));
    }

    /// <summary>
    ///   <para>Checks if material's shader has a property of a given name.</para>
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="nameID"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool HasProperty(int nameID);

    /// <summary>
    ///   <para>Get the value of material's shader tag.</para>
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="searchFallbacks"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetTag(string tag, bool searchFallbacks, [DefaultValue("\"\"")] string defaultValue);

    /// <summary>
    ///   <para>Get the value of material's shader tag.</para>
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="searchFallbacks"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public string GetTag(string tag, bool searchFallbacks)
    {
      string defaultValue = "";
      return this.GetTag(tag, searchFallbacks, defaultValue);
    }

    /// <summary>
    ///   <para>Sets an override tag/value on the material.</para>
    /// </summary>
    /// <param name="tag">Name of the tag to set.</param>
    /// <param name="val">Name of the value to set. Empty string to clear the override flag.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetOverrideTag(string tag, string val);

    /// <summary>
    ///   <para>Enables or disables a Shader pass on a per-Material level.</para>
    /// </summary>
    /// <param name="passName">Shader pass name (case insensitive).</param>
    /// <param name="enabled">Flag indicating whether this Shader pass should be enabled.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetShaderPassEnabled(string passName, bool enabled);

    /// <summary>
    ///   <para>Checks whether a given Shader pass is enabled on this Material.</para>
    /// </summary>
    /// <param name="passName">Shader pass name (case insensitive).</param>
    /// <returns>
    ///   <para>True if the Shader pass is enabled.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetShaderPassEnabled(string passName);

    /// <summary>
    ///   <para>Interpolate properties between two materials.</para>
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="t"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Lerp(Material start, Material end, float t);

    /// <summary>
    ///   <para>How many passes are in this material (Read Only).</para>
    /// </summary>
    public extern int passCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Activate the given pass for rendering.</para>
    /// </summary>
    /// <param name="pass">Shader pass number to setup.</param>
    /// <returns>
    ///   <para>If false is returned, no rendering should be done.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool SetPass(int pass);

    /// <summary>
    ///   <para>Returns the name of the shader pass at index pass.</para>
    /// </summary>
    /// <param name="pass"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetPassName(int pass);

    /// <summary>
    ///   <para>Returns the index of the pass passName.</para>
    /// </summary>
    /// <param name="passName"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int FindPass(string passName);

    /// <summary>
    ///   <para>Render queue of this material.</para>
    /// </summary>
    public extern int renderQueue { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("Creating materials from shader source string will be removed in the future. Use Shader assets instead.")]
    public static Material Create(string scriptContents)
    {
      return new Material(scriptContents);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateWithString([Writable] Material mono, string contents);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateWithShader([Writable] Material mono, Shader shader);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateWithMaterial([Writable] Material mono, Material source);

    /// <summary>
    ///   <para>Copy properties from other material into this material.</para>
    /// </summary>
    /// <param name="mat"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void CopyPropertiesFromMaterial(Material mat);

    /// <summary>
    ///   <para>Sets a shader keyword that is enabled by this material.</para>
    /// </summary>
    /// <param name="keyword"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void EnableKeyword(string keyword);

    /// <summary>
    ///   <para>Unset a shader keyword.</para>
    /// </summary>
    /// <param name="keyword"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void DisableKeyword(string keyword);

    /// <summary>
    ///   <para>Is the shader keyword enabled on this material?</para>
    /// </summary>
    /// <param name="keyword"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool IsKeywordEnabled(string keyword);

    /// <summary>
    ///   <para>Additional shader keywords set by this material.</para>
    /// </summary>
    public extern string[] shaderKeywords { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Defines how the material should interact with lightmaps and lightprobes.</para>
    /// </summary>
    public extern MaterialGlobalIlluminationFlags globalIlluminationFlags { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets whether GPU instancing is enabled for this material.</para>
    /// </summary>
    public extern bool enableInstancing { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets whether the Double Sided Global Illumination setting is enabled for this material.</para>
    /// </summary>
    public extern bool doubleSidedGI { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Sets a named float value.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="value">Float value to set.</param>
    /// <param name="name">Property name, e.g. "_Glossiness".</param>
    public void SetFloat(string name, float value)
    {
      this.SetFloat(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a named float value.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="value">Float value to set.</param>
    /// <param name="name">Property name, e.g. "_Glossiness".</param>
    public void SetFloat(int nameID, float value)
    {
      this.SetFloatImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets a named integer value.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="value">Integer value to set.</param>
    /// <param name="name">Property name, e.g. "_SrcBlend".</param>
    public void SetInt(string name, int value)
    {
      this.SetInt(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a named integer value.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="value">Integer value to set.</param>
    /// <param name="name">Property name, e.g. "_SrcBlend".</param>
    public void SetInt(int nameID, int value)
    {
      this.SetIntImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets a named color value.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name, e.g. "_Color".</param>
    /// <param name="value">Color value to set.</param>
    public void SetColor(string name, Color value)
    {
      this.SetColor(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a named color value.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name, e.g. "_Color".</param>
    /// <param name="value">Color value to set.</param>
    public void SetColor(int nameID, Color value)
    {
      this.SetColorImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets a named vector value.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name, e.g. "_WaveAndDistance".</param>
    /// <param name="value">Vector value to set.</param>
    public void SetVector(string name, Vector4 value)
    {
      this.SetVector(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a named vector value.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name, e.g. "_WaveAndDistance".</param>
    /// <param name="value">Vector value to set.</param>
    public void SetVector(int nameID, Vector4 value)
    {
      this.SetVectorImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets a named matrix for the shader.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name, e.g. "_CubemapRotation".</param>
    /// <param name="value">Matrix value to set.</param>
    public void SetMatrix(string name, Matrix4x4 value)
    {
      this.SetMatrix(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a named matrix for the shader.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name, e.g. "_CubemapRotation".</param>
    /// <param name="value">Matrix value to set.</param>
    public void SetMatrix(int nameID, Matrix4x4 value)
    {
      this.SetMatrixImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets a named texture.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name, e.g. "_MainTex".</param>
    /// <param name="value">Texture to set.</param>
    public void SetTexture(string name, Texture value)
    {
      this.SetTexture(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a named texture.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name, e.g. "_MainTex".</param>
    /// <param name="value">Texture to set.</param>
    public void SetTexture(int nameID, Texture value)
    {
      this.SetTextureImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets a named ComputeBuffer value.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name.</param>
    /// <param name="value">The ComputeBuffer value to set.</param>
    public void SetBuffer(string name, ComputeBuffer value)
    {
      this.SetBuffer(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets a named ComputeBuffer value.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name.</param>
    /// <param name="value">The ComputeBuffer value to set.</param>
    public void SetBuffer(int nameID, ComputeBuffer value)
    {
      this.SetBufferImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets the placement offset of texture propertyName.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name, for example: "_MainTex".</param>
    /// <param name="value">Texture placement offset.</param>
    public void SetTextureOffset(string name, Vector2 value)
    {
      this.SetTextureOffset(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets the placement offset of texture propertyName.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name, for example: "_MainTex".</param>
    /// <param name="value">Texture placement offset.</param>
    public void SetTextureOffset(int nameID, Vector2 value)
    {
      this.SetTextureOffsetImpl(nameID, value);
    }

    /// <summary>
    ///   <para>Sets the placement scale of texture propertyName.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name, e.g. "_MainTex".</param>
    /// <param name="value">Texture placement scale.</param>
    public void SetTextureScale(string name, Vector2 value)
    {
      this.SetTextureScale(Shader.PropertyToID(name), value);
    }

    /// <summary>
    ///   <para>Sets the placement scale of texture propertyName.</para>
    /// </summary>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="name">Property name, e.g. "_MainTex".</param>
    /// <param name="value">Texture placement scale.</param>
    public void SetTextureScale(int nameID, Vector2 value)
    {
      this.SetTextureScaleImpl(nameID, value);
    }

    public void SetFloatArray(string name, List<float> values)
    {
      this.SetFloatArray(Shader.PropertyToID(name), values);
    }

    public void SetFloatArray(int nameID, List<float> values)
    {
      this.SetFloatArray(nameID, (float[]) NoAllocHelpers.ExtractArrayFromList((object) values), values.Count);
    }

    /// <summary>
    ///   <para>Sets a float array property.</para>
    /// </summary>
    /// <param name="name">Property name.</param>
    /// <param name="nameID">Property name ID. Use Shader.PropertyToID to get this ID.</param>
    /// <param name="values">Array of values to set.</param>
    public void SetFloatArray(string name, float[] values)
    {
      this.SetFloatArray(Shader.PropertyToID(name), values);
    }

    /// <summary>
    ///   <para>Sets a float array property.</para>
    /// </summary>
    /// <param name="name">Property name.</param>
    /// <param name="nameID">Property name ID. Use Shader.PropertyToID to get this ID.</param>
    /// <param name="values">Array of values to set.</param>
    public void SetFloatArray(int nameID, float[] values)
    {
      this.SetFloatArray(nameID, values, values.Length);
    }

    private void SetFloatArray(int nameID, float[] values, int count)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      if (values.Length == 0)
        throw new ArgumentException("Zero-sized array is not allowed.");
      if (values.Length < count)
        throw new ArgumentException("array has less elements than passed count.");
      this.SetFloatArrayImpl(nameID, values, count);
    }

    public void SetColorArray(string name, List<Color> values)
    {
      this.SetColorArray(Shader.PropertyToID(name), values);
    }

    public void SetColorArray(int nameID, List<Color> values)
    {
      this.SetColorArray(nameID, (Color[]) NoAllocHelpers.ExtractArrayFromList((object) values), values.Count);
    }

    /// <summary>
    ///   <para>Sets a color array property.</para>
    /// </summary>
    /// <param name="name">Property name.</param>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="values">Array of values to set.</param>
    public void SetColorArray(string name, Color[] values)
    {
      this.SetColorArray(Shader.PropertyToID(name), values);
    }

    /// <summary>
    ///   <para>Sets a color array property.</para>
    /// </summary>
    /// <param name="name">Property name.</param>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    /// <param name="values">Array of values to set.</param>
    public void SetColorArray(int nameID, Color[] values)
    {
      this.SetColorArray(nameID, values, values.Length);
    }

    private void SetColorArray(int nameID, Color[] values, int count)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      if (values.Length == 0)
        throw new ArgumentException("Zero-sized array is not allowed.");
      if (values.Length < count)
        throw new ArgumentException("array has less elements than passed count.");
      this.SetColorArrayImpl(nameID, values, count);
    }

    public void SetVectorArray(string name, List<Vector4> values)
    {
      this.SetVectorArray(Shader.PropertyToID(name), values);
    }

    public void SetVectorArray(int nameID, List<Vector4> values)
    {
      this.SetVectorArray(nameID, (Vector4[]) NoAllocHelpers.ExtractArrayFromList((object) values), values.Count);
    }

    /// <summary>
    ///   <para>Sets a vector array property.</para>
    /// </summary>
    /// <param name="name">Property name.</param>
    /// <param name="values">Array of values to set.</param>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    public void SetVectorArray(string name, Vector4[] values)
    {
      this.SetVectorArray(Shader.PropertyToID(name), values);
    }

    /// <summary>
    ///   <para>Sets a vector array property.</para>
    /// </summary>
    /// <param name="name">Property name.</param>
    /// <param name="values">Array of values to set.</param>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    public void SetVectorArray(int nameID, Vector4[] values)
    {
      this.SetVectorArray(nameID, values, values.Length);
    }

    private void SetVectorArray(int nameID, Vector4[] values, int count)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      if (values.Length == 0)
        throw new ArgumentException("Zero-sized array is not allowed.");
      if (values.Length < count)
        throw new ArgumentException("array has less elements than passed count.");
      this.SetVectorArrayImpl(nameID, values, count);
    }

    public void SetMatrixArray(string name, List<Matrix4x4> values)
    {
      this.SetMatrixArray(Shader.PropertyToID(name), values);
    }

    public void SetMatrixArray(int nameID, List<Matrix4x4> values)
    {
      this.SetMatrixArray(nameID, (Matrix4x4[]) NoAllocHelpers.ExtractArrayFromList((object) values), values.Count);
    }

    /// <summary>
    ///   <para>Sets a matrix array property.</para>
    /// </summary>
    /// <param name="name">Property name.</param>
    /// <param name="values">Array of values to set.</param>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    public void SetMatrixArray(string name, Matrix4x4[] values)
    {
      this.SetMatrixArray(Shader.PropertyToID(name), values);
    }

    /// <summary>
    ///   <para>Sets a matrix array property.</para>
    /// </summary>
    /// <param name="name">Property name.</param>
    /// <param name="values">Array of values to set.</param>
    /// <param name="nameID">Property name ID, use Shader.PropertyToID to get it.</param>
    public void SetMatrixArray(int nameID, Matrix4x4[] values)
    {
      this.SetMatrixArray(nameID, values, values.Length);
    }

    private void SetMatrixArray(int nameID, Matrix4x4[] values, int count)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      if (values.Length == 0)
        throw new ArgumentException("Zero-sized array is not allowed.");
      if (values.Length < count)
        throw new ArgumentException("array has less elements than passed count.");
      this.SetMatrixArrayImpl(nameID, values, count);
    }

    /// <summary>
    ///   <para>Get a named float value.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public float GetFloat(string name)
    {
      return this.GetFloat(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Get a named float value.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public float GetFloat(int nameID)
    {
      return this.GetFloatImpl(nameID);
    }

    /// <summary>
    ///   <para>Get a named integer value.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public int GetInt(string name)
    {
      return this.GetInt(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Get a named integer value.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public int GetInt(int nameID)
    {
      return this.GetIntImpl(nameID);
    }

    /// <summary>
    ///   <para>Get a named color value.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public Color GetColor(string name)
    {
      return this.GetColor(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Get a named color value.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public Color GetColor(int nameID)
    {
      return this.GetColorImpl(nameID);
    }

    /// <summary>
    ///   <para>Get a named vector value.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public Vector4 GetVector(string name)
    {
      return this.GetVector(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Get a named vector value.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public Vector4 GetVector(int nameID)
    {
      return this.GetVectorImpl(nameID);
    }

    /// <summary>
    ///   <para>Get a named matrix value from the shader.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public Matrix4x4 GetMatrix(string name)
    {
      return this.GetMatrix(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Get a named matrix value from the shader.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public Matrix4x4 GetMatrix(int nameID)
    {
      return this.GetMatrixImpl(nameID);
    }

    public void GetFloatArray(string name, List<float> values)
    {
      this.GetFloatArray(Shader.PropertyToID(name), values);
    }

    public void GetFloatArray(int nameID, List<float> values)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      this.GetFloatArrayImplList(nameID, (object) values);
    }

    /// <summary>
    ///   <para>Get a named float array.</para>
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    public float[] GetFloatArray(string name)
    {
      return this.GetFloatArray(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Get a named float array.</para>
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    public float[] GetFloatArray(int nameID)
    {
      return this.GetFloatArrayImpl(nameID);
    }

    public void GetVectorArray(string name, List<Vector4> values)
    {
      this.GetVectorArray(Shader.PropertyToID(name), values);
    }

    public void GetVectorArray(int nameID, List<Vector4> values)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      this.GetVectorArrayImplList(nameID, (object) values);
    }

    /// <summary>
    ///   <para>Get a named color array.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public Color[] GetColorArray(string name)
    {
      return this.GetColorArray(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Get a named color array.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public Color[] GetColorArray(int nameID)
    {
      return this.GetColorArrayImpl(nameID);
    }

    public void GetColorArray(string name, List<Color> values)
    {
      this.GetColorArray(Shader.PropertyToID(name), values);
    }

    public void GetColorArray(int nameID, List<Color> values)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      this.GetColorArrayImplList(nameID, (object) values);
    }

    /// <summary>
    ///   <para>Get a named vector array.</para>
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    public Vector4[] GetVectorArray(string name)
    {
      return this.GetVectorArray(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Get a named vector array.</para>
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    public Vector4[] GetVectorArray(int nameID)
    {
      return this.GetVectorArrayImpl(nameID);
    }

    public void GetMatrixArray(string name, List<Matrix4x4> values)
    {
      this.GetMatrixArray(Shader.PropertyToID(name), values);
    }

    public void GetMatrixArray(int nameID, List<Matrix4x4> values)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      this.GetMatrixArrayImplList(nameID, (object) values);
    }

    /// <summary>
    ///   <para>Get a named matrix array.</para>
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    public Matrix4x4[] GetMatrixArray(string name)
    {
      return this.GetMatrixArray(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Get a named matrix array.</para>
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    public Matrix4x4[] GetMatrixArray(int nameID)
    {
      return this.GetMatrixArrayImpl(nameID);
    }

    /// <summary>
    ///   <para>Get a named texture.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public Texture GetTexture(string name)
    {
      return this.GetTexture(Shader.PropertyToID(name));
    }

    /// <summary>
    ///   <para>Get a named texture.</para>
    /// </summary>
    /// <param name="nameID">The name ID of the property retrieved by Shader.PropertyToID.</param>
    /// <param name="name">The name of the property.</param>
    public Texture GetTexture(int nameID)
    {
      return this.GetTextureImpl(nameID);
    }

    /// <summary>
    ///   <para>Gets the placement offset of texture propertyName.</para>
    /// </summary>
    /// <param name="name">The name of the property.</param>
    public Vector2 GetTextureOffset(string name)
    {
      return this.GetTextureOffset(Shader.PropertyToID(name));
    }

    public Vector2 GetTextureOffset(int nameID)
    {
      Vector4 scaleAndOffsetImpl = this.GetTextureScaleAndOffsetImpl(nameID);
      return new Vector2(scaleAndOffsetImpl.z, scaleAndOffsetImpl.w);
    }

    /// <summary>
    ///   <para>Gets the placement scale of texture propertyName.</para>
    /// </summary>
    /// <param name="name">The name of the property.</param>
    public Vector2 GetTextureScale(string name)
    {
      return this.GetTextureScale(Shader.PropertyToID(name));
    }

    public Vector2 GetTextureScale(int nameID)
    {
      Vector4 scaleAndOffsetImpl = this.GetTextureScaleAndOffsetImpl(nameID);
      return new Vector2(scaleAndOffsetImpl.x, scaleAndOffsetImpl.y);
    }
  }
}
