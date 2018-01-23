// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShaderImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CustomEditor(typeof (ShaderImporter))]
  internal class ShaderImporterInspector : AssetImporterEditor
  {
    private List<string> propertyNames = new List<string>();
    private List<string> displayNames = new List<string>();
    private List<Texture> textures = new List<Texture>();
    private List<TextureDimension> dimensions = new List<TextureDimension>();

    internal override void OnHeaderControlsGUI()
    {
      Shader target = this.assetEditor.target as Shader;
      GUILayout.FlexibleSpace();
      if (!GUILayout.Button("Open...", EditorStyles.miniButton, new GUILayoutOption[0]))
        return;
      AssetDatabase.OpenAsset((UnityEngine.Object) target);
      GUIUtility.ExitGUI();
    }

    public override void OnEnable()
    {
      this.ResetValues();
    }

    private void ShowDefaultTextures()
    {
      if (this.propertyNames.Count == 0)
        return;
      EditorGUILayout.LabelField("Default Maps", EditorStyles.boldLabel, new GUILayoutOption[0]);
      for (int index = 0; index < this.propertyNames.Count; ++index)
      {
        Texture texture1 = this.textures[index];
        Texture texture2 = (Texture) null;
        EditorGUI.BeginChangeCheck();
        System.Type typeFromDimension = MaterialEditor.GetTextureTypeFromDimension(this.dimensions[index]);
        if (typeFromDimension != null)
          texture2 = EditorGUILayout.MiniThumbnailObjectField(GUIContent.Temp(!string.IsNullOrEmpty(this.displayNames[index]) ? this.displayNames[index] : ObjectNames.NicifyVariableName(this.propertyNames[index])), (UnityEngine.Object) texture1, typeFromDimension) as Texture;
        if (EditorGUI.EndChangeCheck())
          this.textures[index] = texture2;
      }
    }

    public override bool HasModified()
    {
      if (base.HasModified())
        return true;
      ShaderImporter target = this.target as ShaderImporter;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return false;
      Shader shader = target.GetShader();
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null)
        return false;
      int propertyCount = ShaderUtil.GetPropertyCount(shader);
      for (int propertyIdx = 0; propertyIdx < propertyCount; ++propertyIdx)
      {
        string propertyName = ShaderUtil.GetPropertyName(shader, propertyIdx);
        for (int index = 0; index < this.propertyNames.Count; ++index)
        {
          if (this.propertyNames[index] == propertyName && (UnityEngine.Object) this.textures[index] != (UnityEngine.Object) target.GetDefaultTexture(propertyName))
            return true;
        }
      }
      return false;
    }

    protected override void ResetValues()
    {
      base.ResetValues();
      this.propertyNames = new List<string>();
      this.displayNames = new List<string>();
      this.textures = new List<Texture>();
      this.dimensions = new List<TextureDimension>();
      ShaderImporter target = this.target as ShaderImporter;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      Shader shader = target.GetShader();
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null)
        return;
      int propertyCount = ShaderUtil.GetPropertyCount(shader);
      for (int propertyIdx = 0; propertyIdx < propertyCount; ++propertyIdx)
      {
        if (ShaderUtil.GetPropertyType(shader, propertyIdx) == ShaderUtil.ShaderPropertyType.TexEnv)
        {
          string propertyName = ShaderUtil.GetPropertyName(shader, propertyIdx);
          string propertyDescription = ShaderUtil.GetPropertyDescription(shader, propertyIdx);
          Texture defaultTexture = target.GetDefaultTexture(propertyName);
          this.propertyNames.Add(propertyName);
          this.displayNames.Add(propertyDescription);
          this.textures.Add(defaultTexture);
          this.dimensions.Add(ShaderUtil.GetTexDim(shader, propertyIdx));
        }
      }
    }

    protected override void Apply()
    {
      base.Apply();
      ShaderImporter target = this.target as ShaderImporter;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      target.SetDefaultTextures(this.propertyNames.ToArray(), this.textures.ToArray());
      AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((UnityEngine.Object) target));
    }

    private static int GetNumberOfTextures(Shader shader)
    {
      int num = 0;
      int propertyCount = ShaderUtil.GetPropertyCount(shader);
      for (int propertyIdx = 0; propertyIdx < propertyCount; ++propertyIdx)
      {
        if (ShaderUtil.GetPropertyType(shader, propertyIdx) == ShaderUtil.ShaderPropertyType.TexEnv)
          ++num;
      }
      return num;
    }

    public override void OnInspectorGUI()
    {
      ShaderImporter target = this.target as ShaderImporter;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      Shader shader = target.GetShader();
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null)
        return;
      if (ShaderImporterInspector.GetNumberOfTextures(shader) != this.propertyNames.Count)
        this.ResetValues();
      this.ShowDefaultTextures();
      this.ApplyRevertGUI();
    }
  }
}
