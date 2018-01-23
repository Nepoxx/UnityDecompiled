// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.Debugger.UxmlExporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.Debugger
{
  internal class UxmlExporter
  {
    private const string UIElementsNamespace = "UnityEngine.Experimental.UIElements";

    public static string Dump(VisualElement selectedElement, string templateId, UxmlExporter.ExportOptions options)
    {
      Dictionary<XNamespace, string> nsToPrefix = new Dictionary<XNamespace, string>() { { (XNamespace) "UnityEngine.Experimental.UIElements", "ui" } };
      HashSet<string> stringSet = new HashSet<string>();
      XDocument xdocument = new XDocument();
      XElement parent = new XElement((XName) "UXML");
      xdocument.Add((object) parent);
      UxmlExporter.Recurse(parent, nsToPrefix, stringSet, selectedElement, options);
      foreach (KeyValuePair<XNamespace, string> keyValuePair in nsToPrefix)
        parent.Add((object) new XAttribute(XNamespace.Xmlns + keyValuePair.Value, (object) keyValuePair.Key));
      foreach (string str in (IEnumerable<string>) stringSet.OrderByDescending<string, string>((Func<string, string>) (x => x)))
        parent.AddFirst((object) new XElement((XName) "Using", new object[2]
        {
          (object) new XAttribute((XName) "alias", (object) str),
          (object) new XAttribute((XName) "path", (object) str)
        }));
      XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, IndentChars = "  ", NewLineChars = "\n", OmitXmlDeclaration = true, NewLineOnAttributes = (options & UxmlExporter.ExportOptions.NewLineOnAttributes) == UxmlExporter.ExportOptions.NewLineOnAttributes, NewLineHandling = NewLineHandling.Replace };
      StringBuilder output = new StringBuilder();
      using (XmlWriter writer = XmlWriter.Create(output, settings))
        xdocument.Save(writer);
      return output.ToString();
    }

    private static void Recurse(XElement parent, Dictionary<XNamespace, string> nsToPrefix, HashSet<string> usings, VisualElement ve, UxmlExporter.ExportOptions options)
    {
      string str1 = ve.GetType().Namespace ?? "";
      string typeName = ve.typeName;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      XElement parent1;
      if (ve is TemplateContainer)
      {
        string templateId = ((TemplateContainer) ve).templateId;
        parent1 = new XElement((XName) templateId);
        usings.Add(templateId);
      }
      else
      {
        string str2;
        parent1 = !nsToPrefix.TryGetValue((XNamespace) str1, out str2) ? new XElement((XName) typeName) : new XElement((XNamespace) str1 + typeName);
      }
      parent.Add((object) parent1);
      foreach (KeyValuePair<string, string> keyValuePair in dictionary)
        parent1.SetAttributeValue((XName) keyValuePair.Key, (object) keyValuePair.Value);
      if (!string.IsNullOrEmpty(ve.name) && (int) ve.name[0] != 95)
        parent1.SetAttributeValue((XName) "name", (object) ve.name);
      else if ((options & UxmlExporter.ExportOptions.AutoNameElements) == UxmlExporter.ExportOptions.AutoNameElements)
      {
        string str2 = ve.GetType().Name + (ve.text == null ? "" : ve.text.Replace(" ", ""));
        parent1.SetAttributeValue((XName) "name", (object) str2);
      }
      if (!string.IsNullOrEmpty(ve.text))
        parent1.SetAttributeValue((XName) "text", (object) ve.text);
      IEnumerable<string> classes = ve.GetClasses();
      if (classes.Any<string>())
        parent1.SetAttributeValue((XName) "class", (object) string.Join(" ", classes.ToArray<string>()));
      if (ve is TemplateContainer)
        return;
      foreach (VisualElement child in ve.Children())
        UxmlExporter.Recurse(parent1, nsToPrefix, usings, child, options);
    }

    [System.Flags]
    public enum ExportOptions
    {
      None = 0,
      NewLineOnAttributes = 1,
      StyleFields = 2,
      AutoNameElements = 4,
    }
  }
}
