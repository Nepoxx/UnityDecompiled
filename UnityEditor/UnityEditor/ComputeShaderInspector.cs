// Decompiled with JetBrains decompiler
// Type: UnityEditor.ComputeShaderInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CustomEditor(typeof (ComputeShader))]
  internal class ComputeShaderInspector : Editor
  {
    private Vector2 m_ScrollPosition = Vector2.zero;
    private const float kSpace = 5f;

    private static List<ComputeShaderInspector.KernelInfo> GetKernelDisplayInfo(ComputeShader cs)
    {
      List<ComputeShaderInspector.KernelInfo> kernelInfoList = new List<ComputeShaderInspector.KernelInfo>();
      int shaderPlatformCount = ShaderUtil.GetComputeShaderPlatformCount(cs);
      for (int platformIndex = 0; platformIndex < shaderPlatformCount; ++platformIndex)
      {
        GraphicsDeviceType shaderPlatformType = ShaderUtil.GetComputeShaderPlatformType(cs, platformIndex);
        int platformKernelCount = ShaderUtil.GetComputeShaderPlatformKernelCount(cs, platformIndex);
        for (int kernelIndex = 0; kernelIndex < platformKernelCount; ++kernelIndex)
        {
          string platformKernelName = ShaderUtil.GetComputeShaderPlatformKernelName(cs, platformIndex, kernelIndex);
          bool flag = false;
          foreach (ComputeShaderInspector.KernelInfo kernelInfo in kernelInfoList)
          {
            if (kernelInfo.name == platformKernelName)
            {
              kernelInfo.platforms += (string) (object) ' ';
              kernelInfo.platforms += shaderPlatformType.ToString();
              flag = true;
            }
          }
          if (!flag)
            kernelInfoList.Add(new ComputeShaderInspector.KernelInfo()
            {
              name = platformKernelName,
              platforms = shaderPlatformType.ToString()
            });
        }
      }
      return kernelInfoList;
    }

    public override void OnInspectorGUI()
    {
      ComputeShader target = this.target as ComputeShader;
      if ((Object) target == (Object) null)
        return;
      GUI.enabled = true;
      EditorGUI.indentLevel = 0;
      this.ShowKernelInfoSection(target);
      this.ShowCompiledCodeSection(target);
      this.ShowShaderErrors(target);
    }

    private void ShowKernelInfoSection(ComputeShader cs)
    {
      GUILayout.Label(ComputeShaderInspector.Styles.kernelsHeading, EditorStyles.boldLabel, new GUILayoutOption[0]);
      foreach (ComputeShaderInspector.KernelInfo kernelInfo in ComputeShaderInspector.GetKernelDisplayInfo(cs))
        EditorGUILayout.LabelField(kernelInfo.name, kernelInfo.platforms, new GUILayoutOption[0]);
    }

    private void ShowCompiledCodeSection(ComputeShader cs)
    {
      GUILayout.Space(5f);
      if (!GUILayout.Button(ComputeShaderInspector.Styles.showCompiled, EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
        return;
      ShaderUtil.OpenCompiledComputeShader(cs, true);
      GUIUtility.ExitGUI();
    }

    private void ShowShaderErrors(ComputeShader s)
    {
      if (ShaderUtil.GetComputeShaderErrorCount(s) < 1)
        return;
      ShaderInspector.ShaderErrorListUI((Object) s, ShaderUtil.GetComputeShaderErrors(s), ref this.m_ScrollPosition);
    }

    private class KernelInfo
    {
      internal string name;
      internal string platforms;
    }

    internal class Styles
    {
      public static GUIContent showCompiled = EditorGUIUtility.TextContent("Show compiled code");
      public static GUIContent kernelsHeading = EditorGUIUtility.TextContent("Kernels:");
    }
  }
}
