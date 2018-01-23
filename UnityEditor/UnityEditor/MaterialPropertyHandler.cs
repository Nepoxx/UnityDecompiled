// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialPropertyHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor
{
  internal class MaterialPropertyHandler
  {
    private static Dictionary<string, MaterialPropertyHandler> s_PropertyHandlers = new Dictionary<string, MaterialPropertyHandler>();
    private MaterialPropertyDrawer m_PropertyDrawer;
    private List<MaterialPropertyDrawer> m_DecoratorDrawers;

    public MaterialPropertyDrawer propertyDrawer
    {
      get
      {
        return this.m_PropertyDrawer;
      }
    }

    public bool IsEmpty()
    {
      return this.m_PropertyDrawer == null && (this.m_DecoratorDrawers == null || this.m_DecoratorDrawers.Count == 0);
    }

    public void OnGUI(ref Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
    {
      float height = position.height;
      position.height = 0.0f;
      if (this.m_DecoratorDrawers != null)
      {
        foreach (MaterialPropertyDrawer decoratorDrawer in this.m_DecoratorDrawers)
        {
          position.height = decoratorDrawer.GetPropertyHeight(prop, label.text, editor);
          float labelWidth = EditorGUIUtility.labelWidth;
          float fieldWidth = EditorGUIUtility.fieldWidth;
          decoratorDrawer.OnGUI(position, prop, label, editor);
          EditorGUIUtility.labelWidth = labelWidth;
          EditorGUIUtility.fieldWidth = fieldWidth;
          position.y += position.height;
          height -= position.height;
        }
      }
      position.height = height;
      if (this.m_PropertyDrawer == null)
        return;
      float labelWidth1 = EditorGUIUtility.labelWidth;
      float fieldWidth1 = EditorGUIUtility.fieldWidth;
      this.m_PropertyDrawer.OnGUI(position, prop, label, editor);
      EditorGUIUtility.labelWidth = labelWidth1;
      EditorGUIUtility.fieldWidth = fieldWidth1;
    }

    public float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
      float num = 0.0f;
      if (this.m_DecoratorDrawers != null)
      {
        foreach (MaterialPropertyDrawer decoratorDrawer in this.m_DecoratorDrawers)
          num += decoratorDrawer.GetPropertyHeight(prop, label, editor);
      }
      if (this.m_PropertyDrawer != null)
        num += this.m_PropertyDrawer.GetPropertyHeight(prop, label, editor);
      return num;
    }

    private static string GetPropertyString(Shader shader, string name)
    {
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null)
        return string.Empty;
      return shader.GetInstanceID().ToString() + "_" + name;
    }

    internal static void InvalidatePropertyCache(Shader shader)
    {
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null)
        return;
      string str = shader.GetInstanceID().ToString() + "_";
      List<string> stringList = new List<string>();
      foreach (string key in MaterialPropertyHandler.s_PropertyHandlers.Keys)
      {
        if (key.StartsWith(str))
          stringList.Add(key);
      }
      foreach (string key in stringList)
        MaterialPropertyHandler.s_PropertyHandlers.Remove(key);
    }

    private static MaterialPropertyDrawer CreatePropertyDrawer(System.Type klass, string argsText)
    {
      if (string.IsNullOrEmpty(argsText))
        return Activator.CreateInstance(klass) as MaterialPropertyDrawer;
      string[] strArray = argsText.Split(',');
      object[] objArray = new object[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
      {
        string s = strArray[index].Trim();
        float result;
        objArray[index] = !float.TryParse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result) ? (object) s : (object) result;
      }
      return Activator.CreateInstance(klass, objArray) as MaterialPropertyDrawer;
    }

    private static MaterialPropertyDrawer GetShaderPropertyDrawer(string attrib, out bool isDecorator)
    {
      isDecorator = false;
      string str = attrib;
      string argsText = string.Empty;
      Match match = Regex.Match(attrib, "(\\w+)\\s*\\((.*)\\)");
      if (match.Success)
      {
        str = match.Groups[1].Value;
        argsText = match.Groups[2].Value.Trim();
      }
      foreach (System.Type klass in EditorAssemblies.SubclassesOf(typeof (MaterialPropertyDrawer)))
      {
        if (klass.Name == str || klass.Name == str + "Drawer" || (klass.Name == "Material" + str + "Drawer" || klass.Name == str + "Decorator") || klass.Name == "Material" + str + "Decorator")
        {
          try
          {
            isDecorator = klass.Name.EndsWith("Decorator");
            return MaterialPropertyHandler.CreatePropertyDrawer(klass, argsText);
          }
          catch (Exception ex)
          {
            Debug.LogWarningFormat("Failed to create material drawer {0} with arguments '{1}'", new object[2]
            {
              (object) str,
              (object) argsText
            });
            return (MaterialPropertyDrawer) null;
          }
        }
      }
      return (MaterialPropertyDrawer) null;
    }

    private static MaterialPropertyHandler GetShaderPropertyHandler(Shader shader, string name)
    {
      string[] propertyAttributes = ShaderUtil.GetShaderPropertyAttributes(shader, name);
      if (propertyAttributes == null || propertyAttributes.Length == 0)
        return (MaterialPropertyHandler) null;
      MaterialPropertyHandler materialPropertyHandler = new MaterialPropertyHandler();
      foreach (string attrib in propertyAttributes)
      {
        bool isDecorator;
        MaterialPropertyDrawer shaderPropertyDrawer = MaterialPropertyHandler.GetShaderPropertyDrawer(attrib, out isDecorator);
        if (shaderPropertyDrawer != null)
        {
          if (isDecorator)
          {
            if (materialPropertyHandler.m_DecoratorDrawers == null)
              materialPropertyHandler.m_DecoratorDrawers = new List<MaterialPropertyDrawer>();
            materialPropertyHandler.m_DecoratorDrawers.Add(shaderPropertyDrawer);
          }
          else
          {
            if (materialPropertyHandler.m_PropertyDrawer != null)
              Debug.LogWarning((object) string.Format("Shader property {0} already has a property drawer", (object) name), (UnityEngine.Object) shader);
            materialPropertyHandler.m_PropertyDrawer = shaderPropertyDrawer;
          }
        }
      }
      return materialPropertyHandler;
    }

    internal static MaterialPropertyHandler GetHandler(Shader shader, string name)
    {
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null)
        return (MaterialPropertyHandler) null;
      string propertyString = MaterialPropertyHandler.GetPropertyString(shader, name);
      MaterialPropertyHandler materialPropertyHandler;
      if (MaterialPropertyHandler.s_PropertyHandlers.TryGetValue(propertyString, out materialPropertyHandler))
        return materialPropertyHandler;
      materialPropertyHandler = MaterialPropertyHandler.GetShaderPropertyHandler(shader, name);
      if (materialPropertyHandler != null && materialPropertyHandler.IsEmpty())
        materialPropertyHandler = (MaterialPropertyHandler) null;
      MaterialPropertyHandler.s_PropertyHandlers[propertyString] = materialPropertyHandler;
      return materialPropertyHandler;
    }
  }
}
