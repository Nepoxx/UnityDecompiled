// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.XmlExtensions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Xml.Linq;

namespace UnityEditor.Experimental.UIElements
{
  internal static class XmlExtensions
  {
    public static string AttributeValue(this XElement elt, string attributeName)
    {
      XAttribute xattribute = elt.Attribute((XName) attributeName);
      return xattribute != null ? xattribute.Value : (string) null;
    }
  }
}
