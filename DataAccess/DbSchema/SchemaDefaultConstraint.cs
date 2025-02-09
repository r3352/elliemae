// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.SchemaDefaultConstraint
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.DataAccess.DbSchema
{
  public class SchemaDefaultConstraint : SchemaElement
  {
    public SchemaDefaultConstraint(XElement xmlElement)
      : base(xmlElement)
    {
      this.TableName = ((this.GetRelationship("DefiningTable") ?? throw new ArgumentException("Missing DefiningTable relationship for Default constraint: " + (string) this.Name)).EntriesOfType<SchemaReference>().FirstOrDefault<SchemaReference>() ?? throw new ArgumentException("Missing reference for DefiningTable relationship in Default constraint: " + (string) this.Name)).Name;
      this.ColumnName = ((this.GetRelationship("ForColumn") ?? throw new ArgumentException("Missing ForColumn relationship for Default constraint: " + (string) this.Name)).EntriesOfType<SchemaReference>().FirstOrDefault<SchemaReference>() ?? throw new ArgumentException("Missing reference for ForColumn relationship in Default constraint: " + (string) this.Name)).Name;
    }

    public QualifiedName TableName { get; private set; }

    public QualifiedName ColumnName { get; private set; }

    public string Expression => this.GetProperty("DefaultExpressionScript");

    public override string ToString() => this.Expression;
  }
}
