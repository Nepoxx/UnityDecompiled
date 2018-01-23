// Decompiled with JetBrains decompiler
// Type: UnityEditor.PhysicsVisualizationSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>This class contains the settings controlling the Physics Debug Visualization.</para>
  /// </summary>
  public static class PhysicsVisualizationSettings
  {
    /// <summary>
    ///   <para>Initializes the physics debug visualization system. The system must be initialized for any physics objects to be visualized. It is normally initialized by the PhysicsDebugWindow.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void InitDebugDraw();

    /// <summary>
    ///   <para>Deinitializes the physics debug visualization system and tracking of changes Colliders.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DeinitDebugDraw();

    /// <summary>
    ///   <para>Resets the visualization options to their default state.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Reset();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetShowStaticColliders(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShowStaticColliders(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, bool show);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetShowTriggers(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShowTriggers(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, bool show);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetShowRigidbodies(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShowRigidbodies(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, bool show);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetShowKinematicBodies(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShowKinematicBodies(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, bool show);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetShowSleepingBodies(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShowSleepingBodies(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, bool show);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetShowCollisionLayer(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, int layer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShowCollisionLayer(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, int layer, bool show);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetShowCollisionLayerMask(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShowCollisionLayerMask(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, int mask);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetShowBoxColliders(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShowBoxColliders(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, bool show);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetShowSphereColliders(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShowSphereColliders(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, bool show);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetShowCapsuleColliders(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShowCapsuleColliders(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, bool show);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetShowMeshColliders(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, PhysicsVisualizationSettings.MeshColliderType colliderType);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShowMeshColliders(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, PhysicsVisualizationSettings.MeshColliderType colliderType, bool show);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetShowTerrainColliders(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShowTerrainColliders(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, bool show);

    /// <summary>
    ///   <para>Shows extra options used to develop and debug the physics visualization.</para>
    /// </summary>
    public static extern bool devOptions { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Dirty marker used for refreshing the GUI.</para>
    /// </summary>
    public static extern int dirtyCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>See PhysicsVisualizationSettings.FilterWorkflow.</para>
    /// </summary>
    public static extern PhysicsVisualizationSettings.FilterWorkflow filterWorkflow { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should the PhysicsDebugWindow display the collision geometry.</para>
    /// </summary>
    public static extern bool showCollisionGeometry { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enables the mouse-over highlighting and mouse selection modes.</para>
    /// </summary>
    public static extern bool enableMouseSelect { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Controls whether the SceneView or the GameView camera is used. Not shown in the UI.</para>
    /// </summary>
    public static extern bool useSceneCam { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Colliders within this distance will be displayed.</para>
    /// </summary>
    public static extern float viewDistance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Maximum number of mesh tiles available to draw all Terrain Colliders.</para>
    /// </summary>
    public static extern int terrainTilesMax { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Forcing the drawing of Colliders on top of any other geometry, regardless of depth.</para>
    /// </summary>
    public static extern bool forceOverdraw { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Color for Colliders that do not have a Rigidbody component.</para>
    /// </summary>
    public static Color staticColor
    {
      get
      {
        Color color;
        PhysicsVisualizationSettings.INTERNAL_get_staticColor(out color);
        return color;
      }
      set
      {
        PhysicsVisualizationSettings.INTERNAL_set_staticColor(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_staticColor(out Color value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_staticColor(ref Color value);

    /// <summary>
    ///   <para>Color for Rigidbodies, primarily active ones.</para>
    /// </summary>
    public static Color rigidbodyColor
    {
      get
      {
        Color color;
        PhysicsVisualizationSettings.INTERNAL_get_rigidbodyColor(out color);
        return color;
      }
      set
      {
        PhysicsVisualizationSettings.INTERNAL_set_rigidbodyColor(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_rigidbodyColor(out Color value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_rigidbodyColor(ref Color value);

    /// <summary>
    ///   <para>Color for kinematic Rigidbodies.</para>
    /// </summary>
    public static Color kinematicColor
    {
      get
      {
        Color color;
        PhysicsVisualizationSettings.INTERNAL_get_kinematicColor(out color);
        return color;
      }
      set
      {
        PhysicsVisualizationSettings.INTERNAL_set_kinematicColor(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_kinematicColor(out Color value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_kinematicColor(ref Color value);

    /// <summary>
    ///   <para>Color for Colliders that are Triggers.</para>
    /// </summary>
    public static Color triggerColor
    {
      get
      {
        Color color;
        PhysicsVisualizationSettings.INTERNAL_get_triggerColor(out color);
        return color;
      }
      set
      {
        PhysicsVisualizationSettings.INTERNAL_set_triggerColor(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_triggerColor(out Color value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_triggerColor(ref Color value);

    /// <summary>
    ///   <para>Color for Rigidbodies that are controlled by the physics simulator, but are not currently being simulated.</para>
    /// </summary>
    public static Color sleepingBodyColor
    {
      get
      {
        Color color;
        PhysicsVisualizationSettings.INTERNAL_get_sleepingBodyColor(out color);
        return color;
      }
      set
      {
        PhysicsVisualizationSettings.INTERNAL_set_sleepingBodyColor(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_sleepingBodyColor(out Color value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_sleepingBodyColor(ref Color value);

    /// <summary>
    ///   <para>Alpha amount used for transparency blending.</para>
    /// </summary>
    public static extern float baseAlpha { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Used to disinguish neighboring Colliders.</para>
    /// </summary>
    public static extern float colorVariance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern float dotAlpha { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool forceDot { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static void SetShowForAllFilters(PhysicsVisualizationSettings.FilterWorkflow filterWorkflow, bool selected)
    {
      for (int layer = 0; layer < 32; ++layer)
        PhysicsVisualizationSettings.SetShowCollisionLayer(filterWorkflow, layer, selected);
      PhysicsVisualizationSettings.SetShowStaticColliders(filterWorkflow, selected);
      PhysicsVisualizationSettings.SetShowTriggers(filterWorkflow, selected);
      PhysicsVisualizationSettings.SetShowRigidbodies(filterWorkflow, selected);
      PhysicsVisualizationSettings.SetShowKinematicBodies(filterWorkflow, selected);
      PhysicsVisualizationSettings.SetShowSleepingBodies(filterWorkflow, selected);
      PhysicsVisualizationSettings.SetShowBoxColliders(filterWorkflow, selected);
      PhysicsVisualizationSettings.SetShowSphereColliders(filterWorkflow, selected);
      PhysicsVisualizationSettings.SetShowCapsuleColliders(filterWorkflow, selected);
      PhysicsVisualizationSettings.SetShowMeshColliders(filterWorkflow, PhysicsVisualizationSettings.MeshColliderType.Convex, selected);
      PhysicsVisualizationSettings.SetShowMeshColliders(filterWorkflow, PhysicsVisualizationSettings.MeshColliderType.NonConvex, selected);
      PhysicsVisualizationSettings.SetShowTerrainColliders(filterWorkflow, selected);
    }

    /// <summary>
    ///   <para>Updates the mouse-over highlight at the given mouse position in screen space.</para>
    /// </summary>
    /// <param name="pos"></param>
    public static void UpdateMouseHighlight(Vector2 pos)
    {
      PhysicsVisualizationSettings.INTERNAL_CALL_UpdateMouseHighlight(ref pos);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_UpdateMouseHighlight(ref Vector2 pos);

    /// <summary>
    ///   <para>Clears the highlighted Collider.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearMouseHighlight();

    /// <summary>
    ///   <para>Returns true if there currently is any kind of physics object highlighted.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasMouseHighlight();

    public static GameObject PickClosestGameObject(Camera cam, int layers, Vector2 position, GameObject[] ignore, GameObject[] filter, out int materialIndex)
    {
      if ((UnityEngine.Object) cam == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (cam));
      return PhysicsVisualizationSettings.Internal_PickClosestGameObject(cam, layers, position, ignore, filter, out materialIndex);
    }

    internal static GameObject Internal_PickClosestGameObject(Camera cam, int layers, Vector2 position, GameObject[] ignore, GameObject[] filter, out int materialIndex)
    {
      return PhysicsVisualizationSettings.INTERNAL_CALL_Internal_PickClosestGameObject(cam, layers, ref position, ignore, filter, out materialIndex);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern GameObject INTERNAL_CALL_Internal_PickClosestGameObject(Camera cam, int layers, ref Vector2 position, GameObject[] ignore, GameObject[] filter, out int materialIndex);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_CollectCollidersForDebugDraw(Camera cam, object colliderList);

    /// <summary>
    ///        <para>Decides whether the Workflow in the Physics Debug window should be inclusive
    /// (&lt;a href="PhysicsVisualizationSettings.FilterWorkflow.ShowSelectedItems.html"&gt;ShowSelectedItems&lt;a&gt;) or exclusive (&lt;a href="PhysicsVisualizationSettings.FilterWorkflow.HideSelectedItems.html"&gt;HideSelectedItems&lt;a&gt;).</para>
    ///      </summary>
    public enum FilterWorkflow
    {
      HideSelectedItems,
      ShowSelectedItems,
    }

    /// <summary>
    ///   <para>Is a MeshCollider convex.</para>
    /// </summary>
    public enum MeshColliderType
    {
      Convex,
      NonConvex,
    }
  }
}
