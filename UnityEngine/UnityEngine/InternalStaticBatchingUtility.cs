// Decompiled with JetBrains decompiler
// Type: UnityEngine.InternalStaticBatchingUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
  internal class InternalStaticBatchingUtility
  {
    private const int MaxVerticesInBatch = 64000;
    private const string CombinedMeshPrefix = "Combined Mesh";

    public static void CombineRoot(GameObject staticBatchRoot)
    {
      InternalStaticBatchingUtility.Combine(staticBatchRoot, false, false);
    }

    public static void Combine(GameObject staticBatchRoot, bool combineOnlyStatic, bool isEditorPostprocessScene)
    {
      GameObject[] objectsOfType = (GameObject[]) Object.FindObjectsOfType(typeof (GameObject));
      List<GameObject> gameObjectList = new List<GameObject>();
      foreach (GameObject gameObject in objectsOfType)
      {
        if ((!((Object) staticBatchRoot != (Object) null) || gameObject.transform.IsChildOf(staticBatchRoot.transform)) && (!combineOnlyStatic || gameObject.isStaticBatchable))
          gameObjectList.Add(gameObject);
      }
      InternalStaticBatchingUtility.CombineGameObjects(gameObjectList.ToArray(), staticBatchRoot, isEditorPostprocessScene);
    }

    public static void CombineGameObjects(GameObject[] gos, GameObject staticBatchRoot, bool isEditorPostprocessScene)
    {
      Matrix4x4 matrix4x4 = Matrix4x4.identity;
      Transform staticBatchRootTransform = (Transform) null;
      if ((bool) ((Object) staticBatchRoot))
      {
        matrix4x4 = staticBatchRoot.transform.worldToLocalMatrix;
        staticBatchRootTransform = staticBatchRoot.transform;
      }
      int batchIndex = 0;
      int num = 0;
      List<MeshSubsetCombineUtility.MeshContainer> meshes = new List<MeshSubsetCombineUtility.MeshContainer>();
      Array.Sort((Array) gos, (IComparer) new InternalStaticBatchingUtility.SortGO());
      foreach (GameObject go in gos)
      {
        MeshFilter component1 = go.GetComponent(typeof (MeshFilter)) as MeshFilter;
        if (!((Object) component1 == (Object) null))
        {
          Mesh sharedMesh = component1.sharedMesh;
          if (!((Object) sharedMesh == (Object) null) && (isEditorPostprocessScene || sharedMesh.canAccess))
          {
            Renderer component2 = component1.GetComponent<Renderer>();
            if (!((Object) component2 == (Object) null) && component2.enabled && component2.staticBatchIndex == 0)
            {
              Material[] materialArray1 = component2.sharedMaterials;
              if (!((IEnumerable<Material>) materialArray1).Any<Material>((Func<Material, bool>) (m => (Object) m != (Object) null && (Object) m.shader != (Object) null && m.shader.disableBatching != DisableBatchingType.False)))
              {
                int vertexCount = sharedMesh.vertexCount;
                if (vertexCount != 0)
                {
                  MeshRenderer meshRenderer = component2 as MeshRenderer;
                  if (!((Object) meshRenderer != (Object) null) || !((Object) meshRenderer.additionalVertexStreams != (Object) null) || vertexCount == meshRenderer.additionalVertexStreams.vertexCount)
                  {
                    if (num + vertexCount > 64000)
                    {
                      InternalStaticBatchingUtility.MakeBatch(meshes, staticBatchRootTransform, batchIndex++);
                      meshes.Clear();
                      num = 0;
                    }
                    MeshSubsetCombineUtility.MeshInstance meshInstance = new MeshSubsetCombineUtility.MeshInstance();
                    meshInstance.meshInstanceID = sharedMesh.GetInstanceID();
                    meshInstance.rendererInstanceID = component2.GetInstanceID();
                    if ((Object) meshRenderer != (Object) null && (Object) meshRenderer.additionalVertexStreams != (Object) null)
                      meshInstance.additionalVertexStreamsMeshInstanceID = meshRenderer.additionalVertexStreams.GetInstanceID();
                    meshInstance.transform = matrix4x4 * component1.transform.localToWorldMatrix;
                    meshInstance.lightmapScaleOffset = component2.lightmapScaleOffset;
                    meshInstance.realtimeLightmapScaleOffset = component2.realtimeLightmapScaleOffset;
                    MeshSubsetCombineUtility.MeshContainer meshContainer = new MeshSubsetCombineUtility.MeshContainer();
                    meshContainer.gameObject = go;
                    meshContainer.instance = meshInstance;
                    meshContainer.subMeshInstances = new List<MeshSubsetCombineUtility.SubMeshInstance>();
                    meshes.Add(meshContainer);
                    if (materialArray1.Length > sharedMesh.subMeshCount)
                    {
                      Debug.LogWarning((object) ("Mesh '" + sharedMesh.name + "' has more materials (" + (object) materialArray1.Length + ") than subsets (" + (object) sharedMesh.subMeshCount + ")"), (Object) component2);
                      Material[] materialArray2 = new Material[sharedMesh.subMeshCount];
                      for (int index = 0; index < sharedMesh.subMeshCount; ++index)
                        materialArray2[index] = component2.sharedMaterials[index];
                      component2.sharedMaterials = materialArray2;
                      materialArray1 = materialArray2;
                    }
                    for (int index = 0; index < Math.Min(materialArray1.Length, sharedMesh.subMeshCount); ++index)
                      meshContainer.subMeshInstances.Add(new MeshSubsetCombineUtility.SubMeshInstance()
                      {
                        meshInstanceID = component1.sharedMesh.GetInstanceID(),
                        vertexOffset = num,
                        subMeshIndex = index,
                        gameObjectInstanceID = go.GetInstanceID(),
                        transform = meshInstance.transform
                      });
                    num += sharedMesh.vertexCount;
                  }
                }
              }
            }
          }
        }
      }
      InternalStaticBatchingUtility.MakeBatch(meshes, staticBatchRootTransform, batchIndex);
    }

    private static void MakeBatch(List<MeshSubsetCombineUtility.MeshContainer> meshes, Transform staticBatchRootTransform, int batchIndex)
    {
      if (meshes.Count < 2)
        return;
      List<MeshSubsetCombineUtility.MeshInstance> meshInstanceList = new List<MeshSubsetCombineUtility.MeshInstance>();
      List<MeshSubsetCombineUtility.SubMeshInstance> subMeshInstanceList = new List<MeshSubsetCombineUtility.SubMeshInstance>();
      foreach (MeshSubsetCombineUtility.MeshContainer mesh in meshes)
      {
        meshInstanceList.Add(mesh.instance);
        subMeshInstanceList.AddRange((IEnumerable<MeshSubsetCombineUtility.SubMeshInstance>) mesh.subMeshInstances);
      }
      string meshName = "Combined Mesh" + " (root: " + (!((Object) staticBatchRootTransform != (Object) null) ? "scene" : staticBatchRootTransform.name) + ")";
      if (batchIndex > 0)
        meshName = meshName + " " + (object) (batchIndex + 1);
      Mesh combinedMesh = StaticBatchingHelper.InternalCombineVertices(meshInstanceList.ToArray(), meshName);
      StaticBatchingHelper.InternalCombineIndices(subMeshInstanceList.ToArray(), combinedMesh);
      int firstSubMesh = 0;
      foreach (MeshSubsetCombineUtility.MeshContainer mesh in meshes)
      {
        ((MeshFilter) mesh.gameObject.GetComponent(typeof (MeshFilter))).sharedMesh = combinedMesh;
        int subMeshCount = mesh.subMeshInstances.Count<MeshSubsetCombineUtility.SubMeshInstance>();
        Renderer component = mesh.gameObject.GetComponent<Renderer>();
        component.SetStaticBatchInfo(firstSubMesh, subMeshCount);
        component.staticBatchRootTransform = staticBatchRootTransform;
        component.enabled = false;
        component.enabled = true;
        MeshRenderer meshRenderer = component as MeshRenderer;
        if ((Object) meshRenderer != (Object) null)
          meshRenderer.additionalVertexStreams = (Mesh) null;
        firstSubMesh += subMeshCount;
      }
    }

    internal class SortGO : IComparer
    {
      int IComparer.Compare(object a, object b)
      {
        if (a == b)
          return 0;
        Renderer renderer1 = InternalStaticBatchingUtility.SortGO.GetRenderer(a as GameObject);
        Renderer renderer2 = InternalStaticBatchingUtility.SortGO.GetRenderer(b as GameObject);
        int num = InternalStaticBatchingUtility.SortGO.GetMaterialId(renderer1).CompareTo(InternalStaticBatchingUtility.SortGO.GetMaterialId(renderer2));
        if (num == 0)
          num = InternalStaticBatchingUtility.SortGO.GetLightmapIndex(renderer1).CompareTo(InternalStaticBatchingUtility.SortGO.GetLightmapIndex(renderer2));
        return num;
      }

      private static int GetMaterialId(Renderer renderer)
      {
        if ((Object) renderer == (Object) null || (Object) renderer.sharedMaterial == (Object) null)
          return 0;
        return renderer.sharedMaterial.GetInstanceID();
      }

      private static int GetLightmapIndex(Renderer renderer)
      {
        if ((Object) renderer == (Object) null)
          return -1;
        return renderer.lightmapIndex;
      }

      private static Renderer GetRenderer(GameObject go)
      {
        if ((Object) go == (Object) null)
          return (Renderer) null;
        MeshFilter component = go.GetComponent(typeof (MeshFilter)) as MeshFilter;
        if ((Object) component == (Object) null)
          return (Renderer) null;
        return component.GetComponent<Renderer>();
      }
    }
  }
}
