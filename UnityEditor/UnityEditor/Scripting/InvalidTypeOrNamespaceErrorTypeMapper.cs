// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.InvalidTypeOrNamespaceErrorTypeMapper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace UnityEditor.Scripting
{
  internal class InvalidTypeOrNamespaceErrorTypeMapper : AbstractAstVisitor
  {
    private readonly int _line;
    private readonly int _column;

    private InvalidTypeOrNamespaceErrorTypeMapper(int line, int column)
    {
      this._line = line;
      this._column = column;
    }

    public static string IsTypeMovedToNamespaceError(CompilationUnit cu, int line, int column)
    {
      InvalidTypeOrNamespaceErrorTypeMapper namespaceErrorTypeMapper = new InvalidTypeOrNamespaceErrorTypeMapper(line, column);
      cu.AcceptVisitor((IAstVisitor) namespaceErrorTypeMapper, (object) null);
      return namespaceErrorTypeMapper.Found;
    }

    public string Found { get; private set; }

    public override object VisitTypeReference(TypeReference typeReference, object data)
    {
      bool flag = this._column >= typeReference.StartLocation.Column && this._column < typeReference.StartLocation.Column + typeReference.Type.Length;
      if (typeReference.StartLocation.Line != this._line || !flag)
        return base.VisitTypeReference(typeReference, data);
      this.Found = typeReference.Type;
      return (object) true;
    }
  }
}
