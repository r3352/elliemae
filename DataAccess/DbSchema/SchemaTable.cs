// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.SchemaTable
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.DataAccess.DbSchema
{
  public class SchemaTable : SchemaElement
  {
    private List<SchemaColumn> columns = new List<SchemaColumn>();

    public SchemaTable(XElement xmlElement)
      : base(xmlElement)
    {
      this.columns.AddRange((IEnumerable<SchemaColumn>) (this.GetRelationship(nameof (Columns)) ?? throw new ArgumentException("Cannot locate Columns relationship for table " + (string) this.Name)).EntriesOfType<SchemaColumn>());
    }

    public IList<SchemaColumn> Columns => (IList<SchemaColumn>) this.columns;

    public SchemaColumn Column(QualifiedName qn)
    {
      return this.columns.First<SchemaColumn>((Func<SchemaColumn, bool>) (col => col.Name.Equals((object) qn)));
    }

    public SchemaPrimaryKeyConstraint PrimaryKeyConstraint { get; set; }

    public void Publish(IScriptWriter writer)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("if not exists (select 1 from sysobjects where id = object_id(" + SQL.EncodeString((string) this.Name) + "))");
      stringBuilder.AppendLine("begin");
      stringBuilder.AppendLine("    print 'Creating table " + (string) this.Name + "'");
      stringBuilder.AppendLine("    create table " + (string) this.Name);
      stringBuilder.AppendLine("    (");
      for (int index = 0; index < this.Columns.Count; ++index)
        stringBuilder.AppendLine("        " + (object) this.Columns[index] + (index == this.Columns.Count - 1 ? (object) "" : (object) ","));
      stringBuilder.AppendLine("    )");
      stringBuilder.AppendLine("end");
      writer.WriteTransaction(stringBuilder.ToString());
      foreach (SchemaColumn column in (IEnumerable<SchemaColumn>) this.Columns)
        column.Publish(writer);
      if (this.PrimaryKeyConstraint == null)
        return;
      this.PrimaryKeyConstraint.Publish(writer);
    }
  }
}
