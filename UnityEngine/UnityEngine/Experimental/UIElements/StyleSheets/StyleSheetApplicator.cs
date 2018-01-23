// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.StyleSheets.StyleSheetApplicator
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
  internal static class StyleSheetApplicator
  {
    private static void Apply<T>(T val, int specificity, ref StyleValue<T> property)
    {
      property.Apply(new StyleValue<T>(val, specificity), StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity);
    }

    public static void ApplyDefault<T>(int specificity, ref StyleValue<T> property)
    {
      StyleSheetApplicator.Apply<T>(default (T), specificity, ref property);
    }

    public static void ApplyBool(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<bool> property)
    {
      StyleSheetApplicator.Apply<bool>(sheet.ReadKeyword(handles[0]) == StyleValueKeyword.True, specificity, ref property);
    }

    public static void ApplyFloat(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<float> property)
    {
      StyleSheetApplicator.Apply<float>(sheet.ReadFloat(handles[0]), specificity, ref property);
    }

    public static void ApplyInt(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<int> property)
    {
      StyleSheetApplicator.Apply<int>((int) sheet.ReadFloat(handles[0]), specificity, ref property);
    }

    public static void ApplyEnum<T>(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<int> property)
    {
      StyleSheetApplicator.Apply<int>(StyleSheetCache.GetEnumValue<T>(sheet, handles[0]), specificity, ref property);
    }

    public static void ApplyColor(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<Color> property)
    {
      StyleSheetApplicator.Apply<Color>(sheet.ReadColor(handles[0]), specificity, ref property);
    }

    public static void ApplyResource<T>(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<T> property) where T : Object
    {
      StyleValueHandle handle = handles[0];
      if (handle.valueType == StyleValueType.Keyword && handle.valueIndex == 5)
      {
        StyleSheetApplicator.Apply<T>((T) null, specificity, ref property);
      }
      else
      {
        T obj = (T) null;
        string pathName = sheet.ReadResourcePath(handle);
        if (string.IsNullOrEmpty(pathName))
          return;
        T val = Panel.loadResourceFunc(pathName, typeof (T)) as T;
        if ((Object) val != (Object) null)
        {
          StyleSheetApplicator.Apply<T>(val, specificity, ref property);
        }
        else
        {
          if (typeof (T) == typeof (Texture2D))
            StyleSheetApplicator.Apply<T>(Panel.loadResourceFunc("d_console.warnicon", typeof (T)) as T, specificity, ref property);
          Debug.LogWarning((object) string.Format("{0} resource/file not found for path: {1}", (object) typeof (T).Name, (object) pathName));
        }
      }
    }

    public static class Shorthand
    {
      private static void ReadFourSidesArea(StyleSheet sheet, StyleValueHandle[] handles, out float top, out float right, out float bottom, out float left)
      {
        top = 0.0f;
        right = 0.0f;
        bottom = 0.0f;
        left = 0.0f;
        switch (handles.Length)
        {
          case 0:
            break;
          case 1:
            top = right = bottom = left = sheet.ReadFloat(handles[0]);
            break;
          case 2:
            top = bottom = sheet.ReadFloat(handles[0]);
            left = right = sheet.ReadFloat(handles[1]);
            break;
          case 3:
            top = sheet.ReadFloat(handles[0]);
            left = right = sheet.ReadFloat(handles[1]);
            bottom = sheet.ReadFloat(handles[2]);
            break;
          default:
            top = sheet.ReadFloat(handles[0]);
            right = sheet.ReadFloat(handles[1]);
            bottom = sheet.ReadFloat(handles[2]);
            left = sheet.ReadFloat(handles[3]);
            break;
        }
      }

      public static void ApplyBorderRadius(StyleSheet sheet, StyleValueHandle[] handles, int specificity, VisualElementStylesData styleData)
      {
        float top;
        float right;
        float bottom;
        float left;
        StyleSheetApplicator.Shorthand.ReadFourSidesArea(sheet, handles, out top, out right, out bottom, out left);
        StyleSheetApplicator.Apply<float>(top, specificity, ref styleData.borderTopLeftRadius);
        StyleSheetApplicator.Apply<float>(right, specificity, ref styleData.borderTopRightRadius);
        StyleSheetApplicator.Apply<float>(left, specificity, ref styleData.borderBottomLeftRadius);
        StyleSheetApplicator.Apply<float>(bottom, specificity, ref styleData.borderBottomRightRadius);
      }

      public static void ApplyMargin(StyleSheet sheet, StyleValueHandle[] handles, int specificity, VisualElementStylesData styleData)
      {
        float top;
        float right;
        float bottom;
        float left;
        StyleSheetApplicator.Shorthand.ReadFourSidesArea(sheet, handles, out top, out right, out bottom, out left);
        StyleSheetApplicator.Apply<float>(top, specificity, ref styleData.marginTop);
        StyleSheetApplicator.Apply<float>(right, specificity, ref styleData.marginRight);
        StyleSheetApplicator.Apply<float>(bottom, specificity, ref styleData.marginBottom);
        StyleSheetApplicator.Apply<float>(left, specificity, ref styleData.marginLeft);
      }

      public static void ApplyPadding(StyleSheet sheet, StyleValueHandle[] handles, int specificity, VisualElementStylesData styleData)
      {
        float top;
        float right;
        float bottom;
        float left;
        StyleSheetApplicator.Shorthand.ReadFourSidesArea(sheet, handles, out top, out right, out bottom, out left);
        StyleSheetApplicator.Apply<float>(top, specificity, ref styleData.paddingTop);
        StyleSheetApplicator.Apply<float>(right, specificity, ref styleData.paddingRight);
        StyleSheetApplicator.Apply<float>(bottom, specificity, ref styleData.paddingBottom);
        StyleSheetApplicator.Apply<float>(left, specificity, ref styleData.paddingLeft);
      }
    }
  }
}
