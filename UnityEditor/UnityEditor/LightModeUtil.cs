// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightModeUtil
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class LightModeUtil
  {
    public static readonly GUIContent s_enableBaked = EditorGUIUtility.TextContent("Baked Global Illumination|Controls whether Mixed and Baked lights will use baked Global Illumination. If enabled, Mixed lights are baked using the specified Lighting Mode and Baked lights will be completely baked and not adjustable at runtime.");
    private static LightModeUtil gs_ptr = (LightModeUtil) null;
    private int[] m_modeVals = new int[3];
    private Object m_cachedObject = (Object) null;
    private SerializedObject m_so = (SerializedObject) null;
    private SerializedProperty m_enableRealtimeGI = (SerializedProperty) null;
    private SerializedProperty m_mixedBakeMode = (SerializedProperty) null;
    private SerializedProperty m_useShadowmask = (SerializedProperty) null;
    private SerializedProperty m_enabledBakedGI = (SerializedProperty) null;
    private SerializedProperty m_workflowMode = (SerializedProperty) null;
    private SerializedProperty m_environmentMode = (SerializedProperty) null;

    private LightModeUtil()
    {
      this.Load();
    }

    public static LightModeUtil Get()
    {
      if (LightModeUtil.gs_ptr == null)
        LightModeUtil.gs_ptr = new LightModeUtil();
      return LightModeUtil.gs_ptr;
    }

    public void GetModes(out int realtimeMode, out int mixedMode)
    {
      realtimeMode = this.m_modeVals[0];
      mixedMode = this.m_modeVals[1];
    }

    public bool AreBakedLightmapsEnabled()
    {
      return this.m_enabledBakedGI != null && this.m_enabledBakedGI.boolValue;
    }

    public bool IsRealtimeGIEnabled()
    {
      return this.m_enableRealtimeGI != null && this.m_enableRealtimeGI.boolValue;
    }

    public bool IsAnyGIEnabled()
    {
      return this.IsRealtimeGIEnabled() || this.AreBakedLightmapsEnabled();
    }

    public bool GetAmbientLightingMode(out int mode)
    {
      if (this.AreBakedLightmapsEnabled() && this.IsRealtimeGIEnabled())
      {
        mode = this.m_environmentMode.intValue;
        return true;
      }
      mode = !this.AreBakedLightmapsEnabled() ? 0 : 1;
      return false;
    }

    public int GetAmbientLightingMode()
    {
      int mode;
      this.GetAmbientLightingMode(out mode);
      return mode;
    }

    public bool IsSubtractiveModeEnabled()
    {
      return this.m_modeVals[1] == 1;
    }

    public bool IsWorkflowAuto()
    {
      return this.m_workflowMode.intValue == 0;
    }

    public void SetWorkflow(bool bAutoEnabled)
    {
      this.m_workflowMode.intValue = !bAutoEnabled ? 1 : 0;
    }

    public void GetProps(out SerializedProperty o_enableRealtimeGI, out SerializedProperty o_enableBakedGI, out SerializedProperty o_mixedBakeMode, out SerializedProperty o_useShadowMask)
    {
      o_enableRealtimeGI = this.m_enableRealtimeGI;
      o_enableBakedGI = this.m_enabledBakedGI;
      o_mixedBakeMode = this.m_mixedBakeMode;
      o_useShadowMask = this.m_useShadowmask;
    }

    public bool Load()
    {
      if (!this.CheckCachedObject())
        return false;
      this.Update(!this.m_enableRealtimeGI.boolValue ? 1 : 0, this.m_mixedBakeMode.intValue);
      return true;
    }

    public void Store(int realtimeMode, int mixedMode)
    {
      this.Update(realtimeMode, mixedMode);
      if (!this.CheckCachedObject())
        return;
      this.m_enableRealtimeGI.boolValue = this.m_modeVals[0] == 0;
      this.m_mixedBakeMode.intValue = this.m_modeVals[1];
      this.m_useShadowmask.boolValue = this.m_modeVals[1] == 2;
    }

    public bool Flush()
    {
      return this.m_so.ApplyModifiedProperties();
    }

    public void DrawBakedGIElement()
    {
      EditorGUILayout.PropertyField(this.m_enabledBakedGI, LightModeUtil.s_enableBaked, new GUILayoutOption[0]);
    }

    public void AnalyzeScene(ref LightModeValidator.Stats stats)
    {
      LightModeValidator.AnalyzeScene(this.m_modeVals[0], this.m_modeVals[1], this.m_modeVals[2], this.GetAmbientLightingMode(), ref stats);
    }

    private bool CheckCachedObject()
    {
      Object lightmapSettings = LightmapEditorSettings.GetLightmapSettings();
      if (lightmapSettings == (Object) null)
        return false;
      if (lightmapSettings == this.m_cachedObject)
      {
        this.m_so.UpdateIfRequiredOrScript();
        return true;
      }
      this.m_cachedObject = lightmapSettings;
      this.m_so = new SerializedObject(lightmapSettings);
      this.m_enableRealtimeGI = this.m_so.FindProperty("m_GISettings.m_EnableRealtimeLightmaps");
      this.m_mixedBakeMode = this.m_so.FindProperty("m_LightmapEditorSettings.m_MixedBakeMode");
      this.m_useShadowmask = this.m_so.FindProperty("m_UseShadowmask");
      this.m_enabledBakedGI = this.m_so.FindProperty("m_GISettings.m_EnableBakedLightmaps");
      this.m_workflowMode = this.m_so.FindProperty("m_GIWorkflowMode");
      this.m_environmentMode = this.m_so.FindProperty("m_GISettings.m_EnvironmentLightingMode");
      return true;
    }

    private void Update(int realtimeMode, int mixedMode)
    {
      this.m_modeVals[0] = realtimeMode;
      this.m_modeVals[1] = mixedMode;
      this.m_modeVals[2] = 0;
    }

    internal enum LightmapMixedBakeMode
    {
      IndirectOnly,
      LightmapsWithSubtractiveShadows,
      ShadowmaskAndIndirect,
    }
  }
}
