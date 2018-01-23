// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.StyleSheets.StyleSheetExtensions
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
  internal static class StyleSheetExtensions
  {
    public static void Apply<T>(this StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<T> property, HandlesApplicatorFunction<T> applicatorFunc)
    {
      if (handles[0].valueType == StyleValueType.Keyword && handles[0].valueIndex == 2)
        StyleSheetApplicator.ApplyDefault<T>(specificity, ref property);
      else
        applicatorFunc(sheet, handles, specificity, ref property);
    }

    public static void ApplyShorthand(this StyleSheet sheet, StyleValueHandle[] handles, int specificity, VisualElementStylesData styleData, ShorthandApplicatorFunction applicatorFunc)
    {
      if (handles[0].valueType == StyleValueType.Keyword || handles[0].valueIndex == 2)
        return;
      applicatorFunc(sheet, handles, specificity, styleData);
    }

    public static string ReadAsString(this StyleSheet sheet, StyleValueHandle handle)
    {
      string empty = string.Empty;
      string str;
      switch (handle.valueType)
      {
        case StyleValueType.Keyword:
          str = sheet.ReadKeyword(handle).ToString();
          break;
        case StyleValueType.Float:
          str = sheet.ReadFloat(handle).ToString();
          break;
        case StyleValueType.Color:
          str = sheet.ReadColor(handle).ToString();
          break;
        case StyleValueType.ResourcePath:
          str = sheet.ReadResourcePath(handle);
          break;
        case StyleValueType.Enum:
          str = sheet.ReadEnum(handle);
          break;
        case StyleValueType.String:
          str = sheet.ReadString(handle);
          break;
        default:
          throw new ArgumentException("Unhandled type " + (object) handle.valueType);
      }
      return str;
    }
  }
}
