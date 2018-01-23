// Decompiled with JetBrains decompiler
// Type: UnityEditor.LookDevResources
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  internal class LookDevResources
  {
    public static Material m_SkyboxMaterial = (Material) null;
    public static Material m_GBufferPatchMaterial = (Material) null;
    public static Material m_DrawBallsMaterial = (Material) null;
    public static Mesh m_ScreenQuadMesh = (Mesh) null;
    public static Material m_LookDevCompositing = (Material) null;
    public static Material m_DeferredOverlayMaterial = (Material) null;
    public static Cubemap m_DefaultHDRI = (Cubemap) null;
    public static Material m_LookDevCubeToLatlong = (Material) null;
    public static RenderTexture m_SelectionTexture = (RenderTexture) null;
    public static RenderTexture m_BrightestPointRT = (RenderTexture) null;
    public static Texture2D m_BrightestPointTexture = (Texture2D) null;
    public static SphericalHarmonicsL2 m_ZeroAmbientProbe;

    public static void Initialize()
    {
      LookDevResources.m_ZeroAmbientProbe.Clear();
      if ((UnityEngine.Object) LookDevResources.m_SkyboxMaterial == (UnityEngine.Object) null)
        LookDevResources.m_SkyboxMaterial = new Material(Shader.Find("Skybox/Cubemap"));
      if ((UnityEngine.Object) LookDevResources.m_ScreenQuadMesh == (UnityEngine.Object) null)
      {
        LookDevResources.m_ScreenQuadMesh = new Mesh();
        LookDevResources.m_ScreenQuadMesh.vertices = new Vector3[4]
        {
          new Vector3(-1f, -1f, 0.0f),
          new Vector3(1f, 1f, 0.0f),
          new Vector3(1f, -1f, 0.0f),
          new Vector3(-1f, 1f, 0.0f)
        };
        LookDevResources.m_ScreenQuadMesh.triangles = new int[6]
        {
          0,
          1,
          2,
          1,
          0,
          3
        };
      }
      if ((UnityEngine.Object) LookDevResources.m_GBufferPatchMaterial == (UnityEngine.Object) null)
      {
        LookDevResources.m_GBufferPatchMaterial = new Material(EditorGUIUtility.LoadRequired("LookDevView/GBufferWhitePatch.shader") as Shader);
        LookDevResources.m_DrawBallsMaterial = new Material(EditorGUIUtility.LoadRequired("LookDevView/GBufferBalls.shader") as Shader);
      }
      if ((UnityEngine.Object) LookDevResources.m_LookDevCompositing == (UnityEngine.Object) null)
        LookDevResources.m_LookDevCompositing = new Material(EditorGUIUtility.LoadRequired("LookDevView/LookDevCompositing.shader") as Shader);
      if ((UnityEngine.Object) LookDevResources.m_DeferredOverlayMaterial == (UnityEngine.Object) null)
        LookDevResources.m_DeferredOverlayMaterial = EditorGUIUtility.LoadRequired("SceneView/SceneViewDeferredMaterial.mat") as Material;
      if ((UnityEngine.Object) LookDevResources.m_DefaultHDRI == (UnityEngine.Object) null)
      {
        LookDevResources.m_DefaultHDRI = EditorGUIUtility.Load("LookDevView/DefaultHDRI.exr") as Cubemap;
        if ((UnityEngine.Object) LookDevResources.m_DefaultHDRI == (UnityEngine.Object) null)
          LookDevResources.m_DefaultHDRI = EditorGUIUtility.Load("LookDevView/DefaultHDRI.asset") as Cubemap;
      }
      if ((UnityEngine.Object) LookDevResources.m_LookDevCubeToLatlong == (UnityEngine.Object) null)
        LookDevResources.m_LookDevCubeToLatlong = new Material(EditorGUIUtility.LoadRequired("LookDevView/LookDevCubeToLatlong.shader") as Shader);
      if ((UnityEngine.Object) LookDevResources.m_SelectionTexture == (UnityEngine.Object) null)
        LookDevResources.m_SelectionTexture = new RenderTexture(250, 125, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
      if ((UnityEngine.Object) LookDevResources.m_BrightestPointRT == (UnityEngine.Object) null)
        LookDevResources.m_BrightestPointRT = new RenderTexture(250, 125, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Default);
      if (!((UnityEngine.Object) LookDevResources.m_BrightestPointTexture == (UnityEngine.Object) null))
        return;
      LookDevResources.m_BrightestPointTexture = new Texture2D(250, 125, TextureFormat.RGBAHalf, false);
    }

    public static void Cleanup()
    {
      LookDevResources.m_SkyboxMaterial = (Material) null;
      if (!(bool) ((UnityEngine.Object) LookDevResources.m_LookDevCompositing))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) LookDevResources.m_LookDevCompositing);
      LookDevResources.m_LookDevCompositing = (Material) null;
    }

    public static void UpdateShadowInfoWithBrightestSpot(CubemapInfo cubemapInfo)
    {
      LookDevResources.m_LookDevCubeToLatlong.SetTexture("_MainTex", (Texture) cubemapInfo.cubemap);
      LookDevResources.m_LookDevCubeToLatlong.SetVector("_WindowParams", new Vector4(10000f, -1000f, 2f, 0.0f));
      LookDevResources.m_LookDevCubeToLatlong.SetVector("_CubeToLatLongParams", new Vector4((float) Math.PI / 180f * cubemapInfo.angleOffset, 0.5f, 1f, 3f));
      LookDevResources.m_LookDevCubeToLatlong.SetPass(0);
      int num1 = 250;
      int num2 = 125;
      Graphics.Blit((Texture) cubemapInfo.cubemap, LookDevResources.m_BrightestPointRT, LookDevResources.m_LookDevCubeToLatlong);
      LookDevResources.m_BrightestPointTexture.ReadPixels(new Rect(0.0f, 0.0f, (float) num1, (float) num2), 0, 0, false);
      LookDevResources.m_BrightestPointTexture.Apply();
      Color[] pixels = LookDevResources.m_BrightestPointTexture.GetPixels();
      float num3 = 0.0f;
      for (int index1 = 0; index1 < num2; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
        {
          Vector3 vector3 = new Vector3(pixels[index1 * num1 + index2].r, pixels[index1 * num1 + index2].g, pixels[index1 * num1 + index2].b);
          float num4 = (float) ((double) vector3.x * 0.212672904133797 + (double) vector3.y * 0.715152204036713 + (double) vector3.z * 0.0721750035881996);
          if ((double) num3 < (double) num4)
          {
            Vector2 latLong = LookDevEnvironmentWindow.PositionToLatLong(new Vector2((float) ((double) index2 / (double) (num1 - 1) * 2.0 - 1.0), (float) ((double) index1 / (double) (num2 - 1) * 2.0 - 1.0)));
            cubemapInfo.shadowInfo.latitude = latLong.x;
            cubemapInfo.shadowInfo.longitude = latLong.y - cubemapInfo.angleOffset;
            num3 = num4;
          }
        }
      }
    }
  }
}
