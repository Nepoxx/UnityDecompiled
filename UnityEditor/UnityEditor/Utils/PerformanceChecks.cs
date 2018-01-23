// Decompiled with JetBrains decompiler
// Type: UnityEditor.Utils.PerformanceChecks
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.Utils
{
  internal class PerformanceChecks
  {
    private static readonly string[] kShadersWithMobileVariants = new string[9]{ "VertexLit", "Diffuse", "Bumped Diffuse", "Bumped Specular", "Particles/Additive", "Particles/VertexLit Blended", "Particles/Alpha Blended", "Particles/Multiply", "RenderFX/Skybox" };

    private static bool IsMobileBuildTarget(BuildTarget target)
    {
      return target == BuildTarget.iOS || target == BuildTarget.Android || target == BuildTarget.Tizen;
    }

    private static string FormattedTextContent(string localeString, params object[] args)
    {
      return string.Format(EditorGUIUtility.TextContent(localeString).text, args);
    }

    public static string CheckMaterial(Material mat, BuildTarget buildTarget)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PerformanceChecks.\u003CCheckMaterial\u003Ec__AnonStorey0 materialCAnonStorey0 = new PerformanceChecks.\u003CCheckMaterial\u003Ec__AnonStorey0();
      if ((UnityEngine.Object) mat == (UnityEngine.Object) null || (UnityEngine.Object) mat.shader == (UnityEngine.Object) null)
        return (string) null;
      // ISSUE: reference to a compiler-generated field
      materialCAnonStorey0.shaderName = mat.shader.name;
      int lod = ShaderUtil.GetLOD(mat.shader);
      // ISSUE: reference to a compiler-generated method
      bool flag1 = Array.Exists<string>(PerformanceChecks.kShadersWithMobileVariants, new Predicate<string>(materialCAnonStorey0.\u003C\u003Em__0));
      bool flag2 = PerformanceChecks.IsMobileBuildTarget(buildTarget);
      if (!(mat.GetTag(nameof (PerformanceChecks), true).ToLower() == "false"))
      {
        if (flag1)
        {
          if (flag2 && mat.HasProperty("_Color") && mat.GetColor("_Color") == new Color(1f, 1f, 1f, 1f))
          {
            // ISSUE: reference to a compiler-generated field
            return PerformanceChecks.FormattedTextContent("Shader is using white color which does nothing; Consider using {0} shader for performance.", (object) ("Mobile/" + materialCAnonStorey0.shaderName));
          }
          // ISSUE: reference to a compiler-generated field
          if (flag2 && materialCAnonStorey0.shaderName.StartsWith("Particles/"))
          {
            // ISSUE: reference to a compiler-generated field
            return PerformanceChecks.FormattedTextContent("Consider using {0} shader on this platform for performance.", (object) ("Mobile/" + materialCAnonStorey0.shaderName));
          }
          // ISSUE: reference to a compiler-generated field
          if (materialCAnonStorey0.shaderName == "RenderFX/Skybox" && mat.HasProperty("_Tint") && mat.GetColor("_Tint") == new Color(0.5f, 0.5f, 0.5f, 0.5f))
            return PerformanceChecks.FormattedTextContent("Skybox shader is using gray color which does nothing; Consider using {0} shader for performance.", (object) "Mobile/Skybox");
        }
        // ISSUE: reference to a compiler-generated field
        if (lod >= 300 && flag2 && !materialCAnonStorey0.shaderName.StartsWith("Mobile/"))
          return PerformanceChecks.FormattedTextContent("Shader might be expensive on this platform. Consider switching to a simpler shader; look under Mobile shaders.");
        // ISSUE: reference to a compiler-generated field
        if (materialCAnonStorey0.shaderName.Contains("VertexLit") && mat.HasProperty("_Emission"))
        {
          bool flag3 = false;
          Shader shader = mat.shader;
          int propertyCount = ShaderUtil.GetPropertyCount(shader);
          for (int propertyIdx = 0; propertyIdx < propertyCount; ++propertyIdx)
          {
            if (ShaderUtil.GetPropertyName(shader, propertyIdx) == "_Emission")
            {
              flag3 = ShaderUtil.GetPropertyType(shader, propertyIdx) == ShaderUtil.ShaderPropertyType.Color;
              break;
            }
          }
          if (flag3)
          {
            Color color = mat.GetColor("_Emission");
            if ((double) color.r >= 0.5 && (double) color.g >= 0.5 && (double) color.b >= 0.5)
              return PerformanceChecks.FormattedTextContent("Looks like you're using VertexLit shader to simulate an unlit object (white emissive). Use one of Unlit shaders instead for performance.");
          }
        }
        if (mat.HasProperty("_BumpMap") && (UnityEngine.Object) mat.GetTexture("_BumpMap") == (UnityEngine.Object) null)
          return PerformanceChecks.FormattedTextContent("Normal mapped shader without a normal map. Consider using a non-normal mapped shader for performance.");
      }
      return (string) null;
    }
  }
}
