// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.BlendTree
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>Blend trees are used to blend continuously animation between their childs. They can either be 1D or 2D.</para>
  /// </summary>
  public sealed class BlendTree : Motion
  {
    public BlendTree()
    {
      BlendTree.Internal_Create(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create(BlendTree mono);

    /// <summary>
    ///   <para>Parameter that is used to compute the blending weight of the childs in 1D blend trees or on the X axis of a 2D blend tree.</para>
    /// </summary>
    public extern string blendParameter { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Parameter that is used to compute the blending weight of the childs on the Y axis of a 2D blend tree.</para>
    /// </summary>
    public extern string blendParameterY { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The Blending type can be either 1D or different types of 2D.</para>
    /// </summary>
    public extern BlendTreeType blendType { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A copy of the list of the blend tree child motions.</para>
    /// </summary>
    public extern ChildMotion[] children { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetDirectBlendTreeParameter(int index, string parameter);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern string GetDirectBlendTreeParameter(int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern int GetChildCount();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern Motion GetChildMotion(int index);

    /// <summary>
    ///   <para>When active, the children's thresholds are automatically spread between 0 and 1.</para>
    /// </summary>
    public extern bool useAutomaticThresholds { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Sets the minimum threshold that will be used by the ChildMotion. Only used when useAutomaticThresholds is true.</para>
    /// </summary>
    public extern float minThreshold { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Sets the maximum threshold that will be used by the ChildMotion. Only used when useAutomaticThresholds is true.</para>
    /// </summary>
    public extern float maxThreshold { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SortChildren();

    internal extern int recursiveBlendParameterCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern string GetRecursiveBlendParameter(int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern float GetRecursiveBlendParameterMin(int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern float GetRecursiveBlendParameterMax(int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetInputBlendValue(string blendValueName, float value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern float GetInputBlendValue(string blendValueName);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern AnimationClip[] GetAnimationClipsFlattened();

    /// <summary>
    ///   <para>Utility function to add a child motion to a blend trees.</para>
    /// </summary>
    /// <param name="motion">The motion to add as child.</param>
    /// <param name="position">The position of the child. When using 2D blend trees.</param>
    /// <param name="threshold">The threshold of the child. When using 1D blend trees.</param>
    public void AddChild(Motion motion)
    {
      this.AddChild(motion, Vector2.zero, 0.0f);
    }

    /// <summary>
    ///   <para>Utility function to add a child motion to a blend trees.</para>
    /// </summary>
    /// <param name="motion">The motion to add as child.</param>
    /// <param name="position">The position of the child. When using 2D blend trees.</param>
    /// <param name="threshold">The threshold of the child. When using 1D blend trees.</param>
    public void AddChild(Motion motion, Vector2 position)
    {
      this.AddChild(motion, position, 0.0f);
    }

    /// <summary>
    ///   <para>Utility function to add a child motion to a blend trees.</para>
    /// </summary>
    /// <param name="motion">The motion to add as child.</param>
    /// <param name="position">The position of the child. When using 2D blend trees.</param>
    /// <param name="threshold">The threshold of the child. When using 1D blend trees.</param>
    public void AddChild(Motion motion, float threshold)
    {
      this.AddChild(motion, Vector2.zero, threshold);
    }

    /// <summary>
    ///   <para>Utility function to remove the child of a blend tree.</para>
    /// </summary>
    /// <param name="index">The index of the blend tree to remove.</param>
    public void RemoveChild(int index)
    {
      Undo.RecordObject((Object) this, "Remove Child");
      ChildMotion[] children = this.children;
      ArrayUtility.RemoveAt<ChildMotion>(ref children, index);
      this.children = children;
    }

    internal void AddChild(Motion motion, Vector2 position, float threshold)
    {
      Undo.RecordObject((Object) this, "Added BlendTree Child");
      ChildMotion[] children = this.children;
      ArrayUtility.Add<ChildMotion>(ref children, new ChildMotion()
      {
        timeScale = 1f,
        motion = motion,
        position = position,
        threshold = threshold,
        directBlendParameter = "Blend"
      });
      this.children = children;
    }

    /// <summary>
    ///   <para>Utility function to add a child blend tree to a blend tree.</para>
    /// </summary>
    /// <param name="position">The position of the child. When using 2D blend trees.</param>
    /// <param name="threshold">The threshold of the child. When using 1D blend trees.</param>
    public BlendTree CreateBlendTreeChild(float threshold)
    {
      return this.CreateBlendTreeChild(Vector2.zero, threshold);
    }

    /// <summary>
    ///   <para>Utility function to add a child blend tree to a blend tree.</para>
    /// </summary>
    /// <param name="position">The position of the child. When using 2D blend trees.</param>
    /// <param name="threshold">The threshold of the child. When using 1D blend trees.</param>
    public BlendTree CreateBlendTreeChild(Vector2 position)
    {
      return this.CreateBlendTreeChild(position, 0.0f);
    }

    internal bool HasChild(BlendTree childTree, bool recursive)
    {
      foreach (ChildMotion child in this.children)
      {
        if ((Object) child.motion == (Object) childTree || recursive && child.motion is BlendTree && (child.motion as BlendTree).HasChild(childTree, true))
          return true;
      }
      return false;
    }

    internal BlendTree CreateBlendTreeChild(Vector2 position, float threshold)
    {
      Undo.RecordObject((Object) this, "Created BlendTree Child");
      BlendTree blendTree = new BlendTree();
      blendTree.name = nameof (BlendTree);
      blendTree.hideFlags = HideFlags.HideInHierarchy;
      if (AssetDatabase.GetAssetPath((Object) this) != "")
        AssetDatabase.AddObjectToAsset((Object) blendTree, AssetDatabase.GetAssetPath((Object) this));
      this.AddChild((Motion) blendTree, position, threshold);
      return blendTree;
    }
  }
}
