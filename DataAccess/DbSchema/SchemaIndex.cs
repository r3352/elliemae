// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.SchemaIndex
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
  public class SchemaIndex : SchemaElement
  {
    private List<SchemaIndexColumnSpec> columns = new List<SchemaIndexColumnSpec>();

    public SchemaIndex(XElement xmlElement)
      : base(xmlElement)
    {
      this.columns.AddRange((IEnumerable<SchemaIndexColumnSpec>) (this.GetRelationship("ColumnSpecifications") ?? throw new ArgumentException("Missing ColumnSpecification relationship for index " + (string) this.Name)).EntriesOfType<SchemaIndexColumnSpec>());
      ICollection<SchemaReference> source = (this.GetRelationship("IndexedObject") ?? throw new ArgumentException("Missing IndexedObject relationship for Index: " + (string) this.Name)).EntriesOfType<SchemaReference>();
      this.TableName = source.Count != 0 ? source.First<SchemaReference>().Name : throw new ArgumentException("Missing Reference element for Column in index spec: " + (string) this.Name);
    }

    public QualifiedName TableName { get; private set; }

    public ICollection<SchemaIndexColumnSpec> Columns
    {
      get => (ICollection<SchemaIndexColumnSpec>) this.columns;
    }

    public bool IsClustered => this.GetProperty(nameof (IsClustered)) == "True";

    public bool IsUnique => this.GetProperty(nameof (IsUnique)) == "True";

    public void Publish(IScriptWriter writer)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("if not exists (select 1 from sysindexes where id = object_id(" + SQL.EncodeString(this.TableName.ObjectName) + ") and name = " + SQL.EncodeString(this.Name.GetUnbracketedObjectName()) + ")");
      stringBuilder.AppendLine("begin");
      stringBuilder.AppendLine("    print 'Adding index " + (string) this.Name + " to table " + (string) this.TableName + "'");
      stringBuilder.Append("    create");
      if (this.IsClustered)
        stringBuilder.Append(" clustered");
      if (this.IsUnique)
        stringBuilder.Append(" unique");
      stringBuilder.Append(" index " + (string) this.Name + " on " + (string) this.TableName);
      List<string> values = new List<string>();
      foreach (SchemaIndexColumnSpec column in (IEnumerable<SchemaIndexColumnSpec>) this.Columns)
        values.Add((string) column.ColumnName);
      stringBuilder.Append(" (" + string.Join(",", (IEnumerable<string>) values) + ")");
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("end");
      writer.WriteTransaction(stringBuilder.ToString());
    }
  }
}
