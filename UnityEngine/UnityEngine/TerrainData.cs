// Decompiled with JetBrains decompiler
// Type: UnityEngine.TerrainData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
  public sealed class TerrainData : Object
  {
    private static readonly int k_MaximumResolution = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MaxHeightmapRes);
    private static readonly int k_MinimumDetailResolutionPerPatch = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MinDetailResPerPatch);
    private static readonly int k_MaximumDetailResolutionPerPatch = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MaxDetailResPerPatch);
    private static readonly int k_MaximumDetailPatchCount = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MaxDetailPatchCount);
    private static readonly int k_MinimumAlphamapResolution = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MinAlphamapRes);
    private static readonly int k_MaximumAlphamapResolution = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MaxAlphamapRes);
    private static readonly int k_MinimumBaseMapResolution = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MinBaseMapRes);
    private static readonly int k_MaximumBaseMapResolution = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MaxBaseMapRes);
    private const string k_ScriptingInterfaceName = "TerrainDataScriptingInterface";
    private const string k_ScriptingInterfacePrefix = "TerrainDataScriptingInterface::";
    private const string k_HeightmapPrefix = "GetHeightmap().";
    private const string k_DetailDatabasePrefix = "GetDetailDatabase().";
    private const string k_TreeDatabasePrefix = "GetTreeDatabase().";
    private const string k_SplatDatabasePrefix = "GetSplatDatabase().";

    public TerrainData()
    {
      TerrainData.Internal_Create(this);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetBoundaryValue(TerrainData.BoundaryValueType type);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] TerrainData terrainData);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool HasUser(GameObject user);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void AddUser(GameObject user);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void RemoveUser(GameObject user);

    public extern int heightmapWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern int heightmapHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int heightmapResolution
    {
      get
      {
        return this.internalHeightmapResolution;
      }
      set
      {
        int num = value;
        if (value < 0 || value > TerrainData.k_MaximumResolution)
        {
          Debug.LogWarning((object) ("heightmapResolution is clamped to the range of [0, " + (object) TerrainData.k_MaximumResolution + "]."));
          num = Math.Min(TerrainData.k_MaximumResolution, Math.Max(value, 0));
        }
        this.internalHeightmapResolution = num;
      }
    }

    private extern int internalHeightmapResolution { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public Vector3 heightmapScale
    {
      get
      {
        Vector3 ret;
        this.get_heightmapScale_Injected(out ret);
        return ret;
      }
    }

    public Vector3 size
    {
      get
      {
        Vector3 ret;
        this.get_size_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_size_Injected(ref value);
      }
    }

    public Bounds bounds
    {
      get
      {
        Bounds ret;
        this.get_bounds_Injected(out ret);
        return ret;
      }
    }

    public extern float thickness { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetHeight(int x, int y);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetInterpolatedHeight(float x, float y);

    public float[,] GetHeights(int xBase, int yBase, int width, int height)
    {
      if (xBase < 0 || yBase < 0 || (xBase + width < 0 || yBase + height < 0) || (xBase + width > this.heightmapWidth || yBase + height > this.heightmapHeight))
        throw new ArgumentException("Trying to access out-of-bounds terrain height information.");
      return this.Internal_GetHeights(xBase, yBase, width, height);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern float[,] Internal_GetHeights(int xBase, int yBase, int width, int height);

    public void SetHeights(int xBase, int yBase, float[,] heights)
    {
      if (heights == null)
        throw new NullReferenceException();
      if (xBase + heights.GetLength(1) > this.heightmapWidth || xBase + heights.GetLength(1) < 0 || (yBase + heights.GetLength(0) < 0 || xBase < 0) || (yBase < 0 || yBase + heights.GetLength(0) > this.heightmapHeight))
        throw new ArgumentException(UnityString.Format("X or Y base out of bounds. Setting up to {0}x{1} while map size is {2}x{3}", (object) (xBase + heights.GetLength(1)), (object) (yBase + heights.GetLength(0)), (object) this.heightmapWidth, (object) this.heightmapHeight));
      this.Internal_SetHeights(xBase, yBase, heights.GetLength(1), heights.GetLength(0), heights);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetHeights(int xBase, int yBase, int width, int height, float[,] heights);

    public void SetHeightsDelayLOD(int xBase, int yBase, float[,] heights)
    {
      if (heights == null)
        throw new ArgumentNullException(nameof (heights));
      int length1 = heights.GetLength(0);
      int length2 = heights.GetLength(1);
      if (xBase < 0 || xBase + length2 < 0 || xBase + length2 > this.heightmapWidth)
        throw new ArgumentException(UnityString.Format("X out of bounds - trying to set {0}-{1} but the terrain ranges from 0-{2}", (object) xBase, (object) (xBase + length2), (object) this.heightmapWidth));
      if (yBase < 0 || yBase + length1 < 0 || yBase + length1 > this.heightmapHeight)
        throw new ArgumentException(UnityString.Format("Y out of bounds - trying to set {0}-{1} but the terrain ranges from 0-{2}", (object) yBase, (object) (yBase + length1), (object) this.heightmapHeight));
      this.Internal_SetHeightsDelayLOD(xBase, yBase, length2, length1, heights);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetHeightsDelayLOD(int xBase, int yBase, int width, int height, float[,] heights);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetSteepness(float x, float y);

    public Vector3 GetInterpolatedNormal(float x, float y)
    {
      Vector3 ret;
      this.GetInterpolatedNormal_Injected(x, y, out ret);
      return ret;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern int GetAdjustedSize(int size);

    public extern float wavingGrassStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern float wavingGrassAmount { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern float wavingGrassSpeed { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public Color wavingGrassTint
    {
      get
      {
        Color ret;
        this.get_wavingGrassTint_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_wavingGrassTint_Injected(ref value);
      }
    }

    public extern int detailWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern int detailHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public void SetDetailResolution(int detailResolution, int resolutionPerPatch)
    {
      if (detailResolution < 0)
      {
        Debug.LogWarning((object) "detailResolution must not be negative.");
        detailResolution = 0;
      }
      if (resolutionPerPatch < TerrainData.k_MinimumDetailResolutionPerPatch || resolutionPerPatch > TerrainData.k_MaximumDetailResolutionPerPatch)
      {
        Debug.LogWarning((object) ("resolutionPerPatch is clamped to the range of [" + (object) TerrainData.k_MinimumDetailResolutionPerPatch + ", " + (object) TerrainData.k_MaximumDetailResolutionPerPatch + "]."));
        resolutionPerPatch = Math.Min(TerrainData.k_MaximumDetailResolutionPerPatch, Math.Max(resolutionPerPatch, TerrainData.k_MinimumDetailResolutionPerPatch));
      }
      int num = detailResolution / resolutionPerPatch;
      if (num > TerrainData.k_MaximumDetailPatchCount)
      {
        Debug.LogWarning((object) ("Patch count (detailResolution / resolutionPerPatch) is clamped to the range of [0, " + (object) TerrainData.k_MaximumDetailPatchCount + "]."));
        num = Math.Min(TerrainData.k_MaximumDetailPatchCount, Math.Max(num, 0));
      }
      this.Internal_SetDetailResolution(num, resolutionPerPatch);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetDetailResolution(int patchCount, int resolutionPerPatch);

    public extern int detailResolution { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal extern int detailResolutionPerPatch { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void ResetDirtyDetails();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void RefreshPrototypes();

    public extern DetailPrototype[] detailPrototypes { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int[] GetSupportedLayers(int xBase, int yBase, int totalWidth, int totalHeight);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int[,] GetDetailLayer(int xBase, int yBase, int width, int height, int layer);

    public void SetDetailLayer(int xBase, int yBase, int layer, int[,] details)
    {
      this.Internal_SetDetailLayer(xBase, yBase, details.GetLength(1), details.GetLength(0), layer, details);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetDetailLayer(int xBase, int yBase, int totalWidth, int totalHeight, int detailIndex, int[,] data);

    public TreeInstance[] treeInstances
    {
      get
      {
        return this.Internal_GetTreeInstances();
      }
      set
      {
        this.Internal_SetTreeInstances(value);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern TreeInstance[] Internal_GetTreeInstances();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetTreeInstances([NotNull] TreeInstance[] instances);

    public TreeInstance GetTreeInstance(int index)
    {
      if (index < 0 || index >= this.treeInstanceCount)
        throw new ArgumentOutOfRangeException(nameof (index));
      return this.Internal_GetTreeInstance(index);
    }

    private TreeInstance Internal_GetTreeInstance(int index)
    {
      TreeInstance ret;
      this.Internal_GetTreeInstance_Injected(index, out ret);
      return ret;
    }

    public void SetTreeInstance(int index, TreeInstance instance)
    {
      this.SetTreeInstance_Injected(index, ref instance);
    }

    public extern int treeInstanceCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern TreePrototype[] treePrototypes { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void RemoveTreePrototype(int index);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void RecalculateTreePositions();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void RemoveDetailPrototype(int index);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool NeedUpgradeScaledTreePrototypes();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void UpgradeScaledTreePrototype();

    public extern int alphamapLayers { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public float[,,] GetAlphamaps(int x, int y, int width, int height)
    {
      if (x < 0 || y < 0 || (width < 0 || height < 0))
        throw new ArgumentException("Invalid argument for GetAlphaMaps");
      return this.Internal_GetAlphamaps(x, y, width, height);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern float[,,] Internal_GetAlphamaps(int x, int y, int width, int height);

    public int alphamapResolution
    {
      get
      {
        return this.Internal_alphamapResolution;
      }
      set
      {
        int num = value;
        if (value < TerrainData.k_MinimumAlphamapResolution || value > TerrainData.k_MaximumAlphamapResolution)
        {
          Debug.LogWarning((object) ("alphamapResolution is clamped to the range of [" + (object) TerrainData.k_MinimumAlphamapResolution + ", " + (object) TerrainData.k_MaximumAlphamapResolution + "]."));
          num = Math.Min(TerrainData.k_MaximumAlphamapResolution, Math.Max(value, TerrainData.k_MinimumAlphamapResolution));
        }
        this.Internal_alphamapResolution = num;
      }
    }

    [RequiredByNativeCode]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern float GetAlphamapResolutionInternal();

    private extern int Internal_alphamapResolution { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public int alphamapWidth
    {
      get
      {
        return this.alphamapResolution;
      }
    }

    public int alphamapHeight
    {
      get
      {
        return this.alphamapResolution;
      }
    }

    public int baseMapResolution
    {
      get
      {
        return this.Internal_baseMapResolution;
      }
      set
      {
        int num = value;
        if (value < TerrainData.k_MinimumBaseMapResolution || value > TerrainData.k_MaximumBaseMapResolution)
        {
          Debug.LogWarning((object) ("baseMapResolution is clamped to the range of [" + (object) TerrainData.k_MinimumBaseMapResolution + ", " + (object) TerrainData.k_MaximumBaseMapResolution + "]."));
          num = Math.Min(TerrainData.k_MaximumBaseMapResolution, Math.Max(value, TerrainData.k_MinimumBaseMapResolution));
        }
        this.Internal_baseMapResolution = num;
      }
    }

    private extern int Internal_baseMapResolution { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public void SetAlphamaps(int x, int y, float[,,] map)
    {
      if (map.GetLength(2) != this.alphamapLayers)
        throw new Exception(UnityString.Format("Float array size wrong (layers should be {0})", (object) this.alphamapLayers));
      this.Internal_SetAlphamaps(x, y, map.GetLength(1), map.GetLength(0), map);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetAlphamaps(int x, int y, int width, int height, float[,,] map);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void RecalculateBasemapIfDirty();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetBasemapDirty(bool dirty);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Texture2D GetAlphamapTexture(int index);

    private extern int alphamapTextureCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public Texture2D[] alphamapTextures
    {
      get
      {
        Texture2D[] texture2DArray = new Texture2D[this.alphamapTextureCount];
        for (int index = 0; index < texture2DArray.Length; ++index)
          texture2DArray[index] = this.GetAlphamapTexture(index);
        return texture2DArray;
      }
    }

    public extern SplatPrototype[] splatPrototypes { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void AddTree(ref TreeInstance tree);

    internal int RemoveTrees(Vector2 position, float radius, int prototypeIndex)
    {
      return this.RemoveTrees_Injected(ref position, radius, prototypeIndex);
    }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_heightmapScale_Injected(out Vector3 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_size_Injected(out Vector3 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_size_Injected(ref Vector3 value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_bounds_Injected(out Bounds ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetInterpolatedNormal_Injected(float x, float y, out Vector3 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_wavingGrassTint_Injected(out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_wavingGrassTint_Injected(ref Color value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_GetTreeInstance_Injected(int index, out TreeInstance ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetTreeInstance_Injected(int index, ref TreeInstance instance);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int RemoveTrees_Injected(ref Vector2 position, float radius, int prototypeIndex);

    private enum BoundaryValueType
    {
      MaxHeightmapRes,
      MinDetailResPerPatch,
      MaxDetailResPerPatch,
      MaxDetailPatchCount,
      MinAlphamapRes,
      MaxAlphamapRes,
      MinBaseMapRes,
      MaxBaseMapRes,
    }
  }
}
