// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.AssetImporters.AssetImporterEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Experimental.AssetImporters
{
  /// <summary>
  ///   <para>Default editor for all asset importer settings.</para>
  /// </summary>
  public abstract class AssetImporterEditor : Editor
  {
    private ulong m_AssetTimeStamp = 0;
    private bool m_MightHaveModified = false;
    private Editor m_AssetEditor;

    protected internal virtual Editor assetEditor
    {
      get
      {
        return this.m_AssetEditor;
      }
      internal set
      {
        this.m_AssetEditor = value;
      }
    }

    internal override string targetTitle
    {
      get
      {
        string str = string.Empty;
        if ((Object) this.assetEditor != (Object) null && this.assetEditor.target == (Object) null)
          this.assetEditor.InternalSetTargets(this.assetEditor.serializedObject.targetObjects);
        if ((Object) this.assetEditor == (Object) null || this.assetEditor.target == (Object) null)
          Debug.LogError((object) "AssetImporterEditor: assetEditor has null targets!");
        else
          str = this.assetEditor.targetTitle;
        return string.Format("{0} Import Settings", (object) str);
      }
    }

    internal override int referenceTargetIndex
    {
      get
      {
        return base.referenceTargetIndex;
      }
      set
      {
        base.referenceTargetIndex = value;
        if (!((Object) this.assetEditor != (Object) null))
          return;
        this.assetEditor.referenceTargetIndex = value;
      }
    }

    internal override IPreviewable preview
    {
      get
      {
        if (this.useAssetDrawPreview && (Object) this.assetEditor != (Object) null)
          return (IPreviewable) this.assetEditor;
        return base.preview;
      }
    }

    /// <summary>
    ///   <para>Determines if the asset preview is handled by the AssetEditor or the Importer DrawPreview</para>
    /// </summary>
    protected virtual bool useAssetDrawPreview
    {
      get
      {
        return true;
      }
    }

    internal override void OnHeaderIconGUI(Rect iconRect)
    {
      if (!((Object) this.assetEditor != (Object) null))
        return;
      this.assetEditor.OnHeaderIconGUI(iconRect);
    }

    /// <summary>
    ///   <para>Should imported object be shown as a separate editor?</para>
    /// </summary>
    public virtual bool showImportedObject
    {
      get
      {
        return true;
      }
    }

    internal override SerializedObject GetSerializedObjectInternal()
    {
      if (this.m_SerializedObject == null)
        this.m_SerializedObject = SerializedObject.LoadFromCache(this.GetInstanceID());
      if (this.m_SerializedObject == null)
        this.m_SerializedObject = new SerializedObject(this.targets);
      return this.m_SerializedObject;
    }

    /// <summary>
    ///   <para>This function is called when the object is loaded.</para>
    /// </summary>
    public virtual void OnEnable()
    {
    }

    /// <summary>
    ///   <para>This function is called when the editor object goes out of scope.</para>
    /// </summary>
    public virtual void OnDisable()
    {
      AssetImporter target = this.target as AssetImporter;
      if (Unsupported.IsDestroyScriptableObject((ScriptableObject) this) && this.m_MightHaveModified && ((Object) target != (Object) null && !InternalEditorUtility.ignoreInspectorChanges) && (this.HasModified() && !this.AssetWasUpdated()))
      {
        string message = "Unapplied import settings for '" + target.assetPath + "'";
        if (this.targets.Length > 1)
          message = "Unapplied import settings for '" + (object) this.targets.Length + "' files";
        if (EditorUtility.DisplayDialog("Unapplied import settings", message, "Apply", "Revert"))
        {
          this.Apply();
          this.m_MightHaveModified = false;
          this.ImportAssets(this.GetAssetPaths());
        }
      }
      if (this.m_SerializedObject == null || !this.m_SerializedObject.hasModifiedProperties)
        return;
      this.m_SerializedObject.Cache(this.GetInstanceID());
      this.m_SerializedObject = (SerializedObject) null;
    }

    /// <summary>
    ///   <para>This function is called when the Editor script is started.</para>
    /// </summary>
    protected virtual void Awake()
    {
      this.ResetTimeStamp();
      this.ResetValues();
    }

    private string[] GetAssetPaths()
    {
      Object[] targets = this.targets;
      string[] strArray = new string[targets.Length];
      for (int index = 0; index < targets.Length; ++index)
      {
        AssetImporter assetImporter = targets[index] as AssetImporter;
        strArray[index] = assetImporter.assetPath;
      }
      return strArray;
    }

    /// <summary>
    ///   <para>Reset the import settings to their last saved values.</para>
    /// </summary>
    protected virtual void ResetValues()
    {
      this.serializedObject.SetIsDifferentCacheDirty();
      this.serializedObject.Update();
    }

    /// <summary>
    ///   <para>Determine if the import settings have been modified.</para>
    /// </summary>
    public virtual bool HasModified()
    {
      return this.serializedObject.hasModifiedProperties;
    }

    /// <summary>
    ///   <para>Saves any changes from the Editor's control into the asset's import settings object.</para>
    /// </summary>
    protected virtual void Apply()
    {
      this.serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    internal bool AssetWasUpdated()
    {
      AssetImporter target = this.target as AssetImporter;
      if ((long) this.m_AssetTimeStamp == 0L)
        this.ResetTimeStamp();
      return (Object) target != (Object) null && (long) this.m_AssetTimeStamp != (long) target.assetTimeStamp;
    }

    internal void ResetTimeStamp()
    {
      AssetImporter target = this.target as AssetImporter;
      if (!((Object) target != (Object) null))
        return;
      this.m_AssetTimeStamp = target.assetTimeStamp;
    }

    protected internal void ApplyAndImport()
    {
      this.Apply();
      this.m_MightHaveModified = false;
      this.ImportAssets(this.GetAssetPaths());
      this.ResetValues();
    }

    private void ImportAssets(string[] paths)
    {
      foreach (string path in paths)
        AssetDatabase.WriteImportSettingsIfDirty(path);
      try
      {
        AssetDatabase.StartAssetEditing();
        foreach (string path in paths)
          AssetDatabase.ImportAsset(path);
      }
      finally
      {
        AssetDatabase.StopAssetEditing();
      }
      this.OnAssetImportDone();
    }

    internal virtual void OnAssetImportDone()
    {
    }

    /// <summary>
    ///   <para>Implements the 'Revert' button of the inspector.</para>
    /// </summary>
    /// <param name="buttonText">Text to display on button.</param>
    protected void RevertButton()
    {
      this.RevertButton("Revert");
    }

    /// <summary>
    ///   <para>Implements the 'Revert' button of the inspector.</para>
    /// </summary>
    /// <param name="buttonText">Text to display on button.</param>
    protected void RevertButton(string buttonText)
    {
      if (!GUILayout.Button(buttonText))
        return;
      this.m_MightHaveModified = false;
      this.ResetTimeStamp();
      this.ResetValues();
      if (this.HasModified())
        Debug.LogError((object) "Importer reports modified values after reset.");
    }

    /// <summary>
    ///   <para>Implements the 'Apply' button of the inspector.</para>
    /// </summary>
    /// <param name="buttonText">Text to display on button.</param>
    /// <returns>
    ///   <para>Returns true if the new settings were successfully applied</para>
    /// </returns>
    protected bool ApplyButton()
    {
      return this.ApplyButton("Apply");
    }

    /// <summary>
    ///   <para>Implements the 'Apply' button of the inspector.</para>
    /// </summary>
    /// <param name="buttonText">Text to display on button.</param>
    /// <returns>
    ///   <para>Returns true if the new settings were successfully applied</para>
    /// </returns>
    protected bool ApplyButton(string buttonText)
    {
      if (!GUILayout.Button(buttonText))
        return false;
      this.ApplyAndImport();
      return true;
    }

    /// <summary>
    ///   <para>Process the 'Apply' and 'Revert' buttons.</para>
    /// </summary>
    /// <returns>
    ///   <para>Returns true if the new settings were successfully applied.</para>
    /// </returns>
    protected virtual bool OnApplyRevertGUI()
    {
      using (new EditorGUI.DisabledScope(!this.HasModified()))
      {
        this.RevertButton();
        return this.ApplyButton();
      }
    }

    /// <summary>
    ///   <para>Add's the 'Apply' and 'Revert' buttons to the editor.</para>
    /// </summary>
    protected void ApplyRevertGUI()
    {
      this.m_MightHaveModified = true;
      EditorGUILayout.Space();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      bool flag = this.OnApplyRevertGUI();
      if (this.AssetWasUpdated() && Event.current.type != EventType.Layout)
      {
        IPreviewable preview = this.preview;
        if (preview != null)
          preview.ReloadPreviewInstances();
        this.ResetTimeStamp();
        this.ResetValues();
        this.Repaint();
      }
      GUILayout.EndHorizontal();
      if (!flag)
        return;
      GUIUtility.ExitGUI();
    }
  }
}
