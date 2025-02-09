// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.SchemaForeignKeyConstraint
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
  public class SchemaForeignKeyConstraint : SchemaElement
  {
    public SchemaForeignKeyConstraint(XElement xmlElement)
      : base(xmlElement)
    {
      this.Column = this.getQualifiedName("Columns");
      this.Table = this.getQualifiedName("DefiningTable");
      this.ForeignColumn = this.getQualifiedName("ForeignColumns");
      this.ForeignTable = this.getQualifiedName(nameof (ForeignTable));
    }

    public QualifiedName Column { get; private set; }

    public QualifiedName Table { get; private set; }

    public QualifiedName ForeignColumn { get; private set; }

    public QualifiedName ForeignTable { get; private set; }

    public bool CascadeDelete => this.GetProperty("OnDeleteAction") == "1";

    private QualifiedName getQualifiedName(string relType)
    {
      ICollection<SchemaReference> source = (this.GetRelationship(relType) ?? throw new ArgumentException("Cannot locate " + relType + " Relationship for foreign key constraint: " + (string) this.Name)).EntriesOfType<SchemaReference>();
      return source.Count != 0 ? source.First<SchemaReference>().Name : throw new ArgumentException("Cannot locate Reference for " + relType + " Relationship for foreign key constraint: " + (string) this.Name);
    }

    public void Publish(IScriptWriter writer)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("if not exists (select 1 from sysobjects where id = object_id(" + SQL.EncodeString((string) this.Name) + "))");
      stringBuilder.AppendLine("begin");
      stringBuilder.AppendLine("    print 'Adding foreign key constraint " + (string) this.Name + " to table " + (string) this.Table + "'");
      stringBuilder.Append("    alter table " + (string) this.Table + " add constraint " + (string) this.Name + " foreign key (" + (string) this.Column + ") references " + (string) this.ForeignTable + " (" + (string) this.ForeignColumn + ")");
      if (this.CascadeDelete)
        stringBuilder.Append(" on delete cascade");
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("end");
      writer.WriteTransaction(stringBuilder.ToString());
    }
  }
}
