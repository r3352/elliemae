// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.SchemaIndexColumnSpec
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.DataAccess.DbSchema
{
  public class SchemaIndexColumnSpec : SchemaElement
  {
    public SchemaIndexColumnSpec(XElement xmlElement)
      : base(xmlElement)
    {
      ICollection<SchemaReference> source = (this.GetRelationship("Column") ?? throw new ArgumentException("Missing Column relationship for IndexColumnSpec: " + (string) this.Name)).EntriesOfType<SchemaReference>();
      this.ColumnName = source.Count != 0 ? source.First<SchemaReference>().Name : throw new ArgumentException("Missing Reference element for Column in index spec: " + (string) this.Name);
    }

    public QualifiedName ColumnName { get; private set; }
  }
}
