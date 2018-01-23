// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.UIElementsViewImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using ExCSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.StyleSheets;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
  [ScriptedImporter(3, "uxml", 0)]
  internal class UIElementsViewImporter : ScriptedImporter
  {
    internal static UIElementsViewImporter.DefaultLogger logger = new UIElementsViewImporter.DefaultLogger();
    private const string k_XmlTemplate = "<UXML xmlns:ui=\"UnityEngine.Experimental.UIElements\">\n  <ui:Label text=\"New UXML\" />\n</UXML>";
    private const StringComparison k_Comparison = StringComparison.InvariantCulture;
    private const string k_TemplateNode = "UXML";
    private const string k_UsingNode = "Using";
    private const string k_UsingAliasAttr = "alias";
    private const string k_UsingPathAttr = "path";
    private const string k_StyleReferenceNode = "Style";
    private const string k_StylePathAttr = "path";
    private const string k_SlotDefinitionAttr = "slot-name";
    private const string k_SlotUsageAttr = "slot";

    [MenuItem("Assets/Create/UIElements View")]
    public static void CreateTemplateMenuItem()
    {
      ProjectWindowUtil.CreateAssetWithContent("New UXML.uxml", "<UXML xmlns:ui=\"UnityEngine.Experimental.UIElements\">\n  <ui:Label text=\"New UXML\" />\n</UXML>");
    }

    public override void OnImportAsset(AssetImportContext args)
    {
      UIElementsViewImporter.logger.BeginImport(args.assetPath);
      VisualTreeAsset vta;
      UIElementsViewImporter.ImportXml(args.assetPath, out vta);
      args.AddObjectToAsset("tree", (UnityEngine.Object) vta);
      args.SetMainObject((UnityEngine.Object) vta);
      if (!(bool) ((UnityEngine.Object) vta.inlineSheet))
        vta.inlineSheet = ScriptableObject.CreateInstance<UnityEngine.StyleSheets.StyleSheet>();
      args.AddObjectToAsset("inlineStyle", (UnityEngine.Object) vta.inlineSheet);
    }

    internal static void ImportXml(string xmlPath, out VisualTreeAsset vta)
    {
      vta = ScriptableObject.CreateInstance<VisualTreeAsset>();
      vta.visualElementAssets = new List<VisualElementAsset>();
      vta.templateAssets = new List<TemplateAsset>();
      XDocument doc;
      try
      {
        doc = XDocument.Load(xmlPath, LoadOptions.SetLineInfo);
      }
      catch (Exception ex)
      {
        UIElementsViewImporter.logger.LogError(ImportErrorType.Syntax, ImportErrorCode.InvalidXml, (object) ex, UIElementsViewImporter.Error.Level.Fatal, (IXmlLineInfo) null);
        return;
      }
      StyleSheetBuilder ssb = new StyleSheetBuilder();
      UIElementsViewImporter.LoadXmlRoot(doc, vta, ssb);
      UnityEngine.StyleSheets.StyleSheet instance = ScriptableObject.CreateInstance<UnityEngine.StyleSheets.StyleSheet>();
      instance.name = "inlineStyle";
      ssb.BuildTo(instance);
      vta.inlineSheet = instance;
    }

    private static void LoadXmlRoot(XDocument doc, VisualTreeAsset vta, StyleSheetBuilder ssb)
    {
      XElement root = doc.Root;
      if (!string.Equals(root.Name.LocalName, "UXML", StringComparison.InvariantCulture))
      {
        UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.InvalidRootElement, (object) root.Name, UIElementsViewImporter.Error.Level.Fatal, (IXmlLineInfo) root);
      }
      else
      {
        foreach (XElement element in root.Elements())
        {
          switch (element.Name.LocalName)
          {
            case "Using":
              UIElementsViewImporter.LoadUsingNode(vta, root, element);
              continue;
            default:
              UIElementsViewImporter.LoadXml(element, (VisualElementAsset) null, vta, ssb);
              continue;
          }
        }
      }
    }

    private static void LoadUsingNode(VisualTreeAsset vta, XElement elt, XElement child)
    {
      bool flag = false;
      string str = (string) null;
      string path = (string) null;
      foreach (XAttribute attribute in child.Attributes())
      {
        switch (attribute.Name.LocalName)
        {
          case "path":
            flag = true;
            path = attribute.Value;
            break;
          case "alias":
            str = attribute.Value;
            if (str == string.Empty)
            {
              UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.UsingHasEmptyAlias, (object) child, UIElementsViewImporter.Error.Level.Fatal, (IXmlLineInfo) child);
              break;
            }
            break;
          default:
            UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.UnknownAttribute, (object) attribute.Name.LocalName, UIElementsViewImporter.Error.Level.Fatal, (IXmlLineInfo) child);
            break;
        }
      }
      if (!flag)
      {
        UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.MissingPathAttributeOnUsing, (object) null, UIElementsViewImporter.Error.Level.Fatal, (IXmlLineInfo) elt);
      }
      else
      {
        if (string.IsNullOrEmpty(str))
          str = Path.GetFileNameWithoutExtension(path);
        if (vta.AliasExists(str))
          UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.DuplicateUsingAlias, (object) str, UIElementsViewImporter.Error.Level.Fatal, (IXmlLineInfo) elt);
        else
          vta.RegisterUsing(str, path);
      }
    }

    private static void LoadXml(XElement elt, VisualElementAsset parent, VisualTreeAsset vta, StyleSheetBuilder ssb)
    {
      VisualElementAsset vea;
      if (!UIElementsViewImporter.ResolveType(elt, vta, out vea))
      {
        UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.UnknownElement, (object) elt.Name.LocalName, UIElementsViewImporter.Error.Level.Fatal, (IXmlLineInfo) elt);
      }
      else
      {
        int num1 = parent != null ? parent.id : 0;
        int num2 = num1 << 1 ^ vea.GetHashCode();
        vea.parentId = num1;
        vea.id = num2;
        bool attributes = UIElementsViewImporter.ParseAttributes(elt, vea, ssb, vta, parent);
        vea.ruleIndex = !attributes ? -1 : ssb.EndRule();
        if (vea is TemplateAsset)
          vta.templateAssets.Add((TemplateAsset) vea);
        else
          vta.visualElementAssets.Add(vea);
        if (!elt.HasElements)
          return;
        foreach (XElement element in elt.Elements())
        {
          if (element.Name.LocalName == "Style")
            UIElementsViewImporter.LoadStyleReferenceNode(vea, element);
          else
            UIElementsViewImporter.LoadXml(element, vea, vta, ssb);
        }
      }
    }

    private static void LoadStyleReferenceNode(VisualElementAsset vea, XElement styleElt)
    {
      XAttribute xattribute = styleElt.Attribute((XName) "path");
      if (xattribute == null || string.IsNullOrEmpty(xattribute.Value))
        UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.StyleReferenceEmptyOrMissingPathAttr, (object) null, UIElementsViewImporter.Error.Level.Warning, (IXmlLineInfo) styleElt);
      else
        vea.stylesheets.Add(xattribute.Value);
    }

    private static bool ResolveType(XElement elt, VisualTreeAsset visualTreeAsset, out VisualElementAsset vea)
    {
      if (visualTreeAsset.AliasExists(elt.Name.LocalName))
      {
        vea = (VisualElementAsset) new TemplateAsset(elt.Name.LocalName);
      }
      else
      {
        string fullTypeName = !string.IsNullOrEmpty(elt.Name.NamespaceName) ? elt.Name.NamespaceName + "." + elt.Name.LocalName : elt.Name.LocalName;
        if (fullTypeName == typeof (VisualElement).FullName)
          fullTypeName = typeof (VisualContainer).FullName;
        vea = new VisualElementAsset(fullTypeName);
      }
      return true;
    }

    private static bool ParseAttributes(XElement elt, VisualElementAsset res, StyleSheetBuilder ssb, VisualTreeAsset vta, VisualElementAsset parent)
    {
      res.name = "_" + res.GetType().Name;
      bool flag = false;
      foreach (XAttribute attribute in elt.Attributes())
      {
        string localName = attribute.Name.LocalName;
        if (localName != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (UIElementsViewImporter.\u003C\u003Ef__switch\u0024map3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            UIElementsViewImporter.\u003C\u003Ef__switch\u0024map3 = new Dictionary<string, int>(8)
            {
              {
                "name",
                0
              },
              {
                "text",
                1
              },
              {
                "class",
                2
              },
              {
                "contentContainer",
                3
              },
              {
                "slot-name",
                4
              },
              {
                "slot",
                5
              },
              {
                "style",
                6
              },
              {
                "pickingMode",
                7
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (UIElementsViewImporter.\u003C\u003Ef__switch\u0024map3.TryGetValue(localName, out num))
          {
            switch (num)
            {
              case 0:
                res.name = attribute.Value;
                continue;
              case 1:
                res.text = attribute.Value;
                continue;
              case 2:
                res.classes = attribute.Value.Split(' ');
                continue;
              case 3:
                if (vta.contentContainerId != 0)
                {
                  UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.DuplicateContentContainer, (object) null, UIElementsViewImporter.Error.Level.Fatal, (IXmlLineInfo) elt);
                  continue;
                }
                vta.contentContainerId = res.id;
                continue;
              case 4:
                if (string.IsNullOrEmpty(attribute.Value))
                {
                  UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.SlotDefinitionHasEmptyName, (object) null, UIElementsViewImporter.Error.Level.Fatal, (IXmlLineInfo) elt);
                  continue;
                }
                if (!vta.AddSlotDefinition(attribute.Value, res.id))
                {
                  UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.DuplicateSlotDefinition, (object) attribute.Value, UIElementsViewImporter.Error.Level.Fatal, (IXmlLineInfo) elt);
                  continue;
                }
                continue;
              case 5:
                TemplateAsset templateAsset = parent as TemplateAsset;
                if (templateAsset == null)
                {
                  UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.SlotUsageInNonTemplate, (object) parent, UIElementsViewImporter.Error.Level.Fatal, (IXmlLineInfo) elt);
                  continue;
                }
                if (string.IsNullOrEmpty(attribute.Value))
                {
                  UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.SlotUsageHasEmptyName, (object) null, UIElementsViewImporter.Error.Level.Fatal, (IXmlLineInfo) elt);
                  continue;
                }
                templateAsset.AddSlotUsage(attribute.Value, res.id);
                continue;
              case 6:
                ExCSS.StyleSheet styleSheet = new ExCSS.Parser().Parse("* { " + attribute.Value + " }");
                if (styleSheet.Errors.Count != 0)
                {
                  UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.InvalidCssInStyleAttribute, (object) styleSheet.Errors.Aggregate<StylesheetParseError, string>("", (Func<string, StylesheetParseError, string>) ((s, error) => s + error.ToString() + "\n")), UIElementsViewImporter.Error.Level.Warning, (IXmlLineInfo) attribute);
                  continue;
                }
                if (styleSheet.StyleRules.Count != 1)
                {
                  UIElementsViewImporter.logger.LogError(ImportErrorType.Semantic, ImportErrorCode.InvalidCssInStyleAttribute, (object) ("Expected one style rule, found " + (object) styleSheet.StyleRules.Count), UIElementsViewImporter.Error.Level.Warning, (IXmlLineInfo) attribute);
                  continue;
                }
                ssb.BeginRule(-1);
                flag = true;
                StyleSheetImportErrors errors = new StyleSheetImportErrors();
                using (IEnumerator<Property> enumerator = styleSheet.StyleRules[0].Declarations.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    Property current = enumerator.Current;
                    ssb.BeginProperty(current.Name);
                    StyleSheetImporter.VisitValue(errors, ssb, current.Term);
                    ssb.EndProperty();
                  }
                  continue;
                }
              case 7:
                if (!Enum.IsDefined(typeof (PickingMode), (object) attribute.Value))
                {
                  Debug.LogErrorFormat("Could not parse value of '{0}', because it isn't defined in the PickingMode enum.", (object) attribute.Value);
                  continue;
                }
                res.pickingMode = (PickingMode) Enum.Parse(typeof (PickingMode), attribute.Value);
                continue;
            }
          }
        }
        res.AddProperty(attribute.Name.LocalName, attribute.Value);
      }
      return flag;
    }

    internal struct Error
    {
      public readonly UIElementsViewImporter.Error.Level level;
      public readonly ImportErrorType error;
      public readonly ImportErrorCode code;
      public readonly object context;
      public readonly string filePath;
      public readonly IXmlLineInfo xmlLineInfo;

      public Error(ImportErrorType error, ImportErrorCode code, object context, UIElementsViewImporter.Error.Level level, string filePath, IXmlLineInfo xmlLineInfo)
      {
        this.xmlLineInfo = xmlLineInfo;
        this.error = error;
        this.code = code;
        this.context = context;
        this.level = level;
        this.filePath = filePath;
      }

      private static string ErrorMessage(ImportErrorCode errorCode)
      {
        switch (errorCode)
        {
          case ImportErrorCode.InvalidRootElement:
            return "Expected the XML Root element name to be 'UXML', found '{0}'";
          case ImportErrorCode.DuplicateUsingAlias:
            return "Duplicate alias '{0}'";
          case ImportErrorCode.UnknownElement:
            return "Unknown element name '{0}'";
          case ImportErrorCode.UnknownAttribute:
            return "Unknown attribute: '{0}'";
          case ImportErrorCode.InvalidXml:
            return "Xml is not valid, exception during parsing: {0}";
          case ImportErrorCode.InvalidCssInStyleAttribute:
            return "USS in 'style' attribute is invalid: {0}";
          case ImportErrorCode.MissingPathAttributeOnUsing:
            return "'Using' declaration requires a 'path' attribute referencing another uxml file";
          case ImportErrorCode.UsingHasEmptyAlias:
            return "'Using' declaration requires a non-empty 'alias' attribute";
          case ImportErrorCode.StyleReferenceEmptyOrMissingPathAttr:
            return "USS in 'style' attribute is invalid: {0}";
          case ImportErrorCode.DuplicateSlotDefinition:
            return "Slot definition '{0}' is defined more than once";
          case ImportErrorCode.SlotUsageInNonTemplate:
            return "Element has an assigned slot, but its parent '{0}' is not a template reference";
          case ImportErrorCode.SlotDefinitionHasEmptyName:
            return "Slot definition has an empty name";
          case ImportErrorCode.SlotUsageHasEmptyName:
            return "Slot usage has an empty name";
          case ImportErrorCode.DuplicateContentContainer:
            return "'contentContainer' attribute must be defined once at most";
          default:
            throw new ArgumentOutOfRangeException("Unhandled error code " + (object) errorCode);
        }
      }

      public override string ToString()
      {
        return string.Format("{0}{1}: {2} - {3}", (object) this.filePath, (object) (this.xmlLineInfo != null ? string.Format(" ({0},{1})", (object) this.xmlLineInfo.LineNumber, (object) this.xmlLineInfo.LinePosition) : ""), (object) this.error, (object) string.Format(UIElementsViewImporter.Error.ErrorMessage(this.code), this.context != null ? (object) this.context.ToString() : (object) "<null>"));
      }

      public enum Level
      {
        Info,
        Warning,
        Fatal,
      }
    }

    internal class DefaultLogger
    {
      protected List<UIElementsViewImporter.Error> m_Errors = new List<UIElementsViewImporter.Error>();
      protected string m_Path;

      internal virtual void LogError(ImportErrorType error, ImportErrorCode code, object context, UIElementsViewImporter.Error.Level level, IXmlLineInfo xmlLineInfo)
      {
        this.m_Errors.Add(new UIElementsViewImporter.Error(error, code, context, level, this.m_Path, xmlLineInfo));
      }

      internal virtual void BeginImport(string path)
      {
        this.m_Path = path;
      }

      private void LogError(VisualTreeAsset obj, UIElementsViewImporter.Error error)
      {
        try
        {
          switch (error.level)
          {
            case UIElementsViewImporter.Error.Level.Info:
              Debug.LogFormat((UnityEngine.Object) obj, error.ToString(), new object[0]);
              break;
            case UIElementsViewImporter.Error.Level.Warning:
              Debug.LogWarningFormat((UnityEngine.Object) obj, error.ToString(), new object[0]);
              break;
            case UIElementsViewImporter.Error.Level.Fatal:
              Debug.LogErrorFormat((UnityEngine.Object) obj, error.ToString(), new object[0]);
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
        catch (FormatException ex)
        {
          switch (error.level)
          {
            case UIElementsViewImporter.Error.Level.Info:
              Debug.Log((object) error.ToString());
              break;
            case UIElementsViewImporter.Error.Level.Warning:
              Debug.LogWarning((object) error.ToString());
              break;
            case UIElementsViewImporter.Error.Level.Fatal:
              Debug.LogError((object) error.ToString());
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }

      internal virtual void FinishImport()
      {
        Dictionary<string, VisualTreeAsset> dictionary = new Dictionary<string, VisualTreeAsset>();
        foreach (UIElementsViewImporter.Error error in this.m_Errors)
        {
          VisualTreeAsset visualTreeAsset;
          if (!dictionary.TryGetValue(error.filePath, out visualTreeAsset))
            dictionary.Add(error.filePath, visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(error.filePath));
          this.LogError(visualTreeAsset, error);
        }
        this.m_Errors.Clear();
      }
    }
  }
}
