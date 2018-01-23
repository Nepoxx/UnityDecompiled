// Decompiled with JetBrains decompiler
// Type: UnityEditor.StyleSheets.StyleSheetImportErrors
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.StyleSheets
{
  internal class StyleSheetImportErrors
  {
    private List<StyleSheetImportErrors.Error> m_Errors = new List<StyleSheetImportErrors.Error>();

    public void AddSyntaxError(string context)
    {
      this.m_Errors.Add(new StyleSheetImportErrors.Error(StyleSheetImportErrorType.Syntax, StyleSheetImportErrorCode.None, context));
    }

    public void AddSemanticError(StyleSheetImportErrorCode code, string context)
    {
      this.m_Errors.Add(new StyleSheetImportErrors.Error(StyleSheetImportErrorType.Semantic, code, context));
    }

    public void AddInternalError(string context)
    {
      this.m_Errors.Add(new StyleSheetImportErrors.Error(StyleSheetImportErrorType.Internal, StyleSheetImportErrorCode.None, context));
    }

    public bool hasErrors
    {
      get
      {
        return this.m_Errors.Count > 0;
      }
    }

    public IEnumerable<string> FormatErrors()
    {
      return this.m_Errors.Select<StyleSheetImportErrors.Error, string>((Func<StyleSheetImportErrors.Error, string>) (e => e.ToString()));
    }

    private struct Error
    {
      public readonly StyleSheetImportErrorType error;
      public readonly StyleSheetImportErrorCode code;
      public readonly string context;

      public Error(StyleSheetImportErrorType error, StyleSheetImportErrorCode code, string context)
      {
        this.error = error;
        this.code = code;
        this.context = context;
      }

      public override string ToString()
      {
        return string.Format("[StyleSheetImportError: error={0}, code={1}, context={2}]", (object) this.error, (object) this.code, (object) this.context);
      }
    }
  }
}
