// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.SchemaColumn
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
  public class SchemaColumn : SchemaElement
  {
    public SchemaColumn(XElement xmlElement)
      : base(xmlElement)
    {
      this.TableName = new QualifiedName(this.Name.ParentName);
      ICollection<SchemaTypeSpecifier> source = (this.GetRelationship(nameof (TypeSpecifier)) ?? throw new Exception("TypeSpecifier relationship missing for column '" + (string) this.Name + "'")).EntriesOfType<SchemaTypeSpecifier>();
      this.TypeSpecifier = source.Count != 0 ? source.First<SchemaTypeSpecifier>() : throw new Exception("TypeSpecifier collection empty for column '" + (string) this.Name + "'");
    }

    public QualifiedName TableName { get; private set; }

    public SchemaDefaultConstraint DefaultConstraint { get; set; }

    public SchemaTypeSpecifier TypeSpecifier { get; private set; }

    public bool IsNullable => this.GetProperty(nameof (IsNullable)) != "False";

    public override string ToString() => this.ToString(true);

    public string GetDefaultConstraintName()
    {
      return "CN_" + SQL.EncodeSysName(this.TableName.GetUnbracketedObjectName() + "_" + this.Name.GetUnbracketedObjectName());
    }

    public string ToString(bool includeConstraint)
    {
      string str = (string) this.Name + " " + (object) this.TypeSpecifier + " " + (this.IsNullable ? "" : "NOT ") + "NULL";
      if (includeConstraint && this.DefaultConstraint != null)
        str = str + " CONSTRAINT [" + this.GetDefaultConstraintName() + "] DEFAULT (" + (object) this.DefaultConstraint + ")";
      return str;
    }

    public void Publish(IScriptWriter writer)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("if not exists (select 1 from sys.columns where object_id = object_id(" + SQL.EncodeString(this.TableName.ObjectName) + ") and name = " + SQL.EncodeString(this.Name.GetUnbracketedObjectName()) + ")");
      stringBuilder.AppendLine("begin");
      stringBuilder.AppendLine("    print 'Adding column " + this.Name.ObjectName + " to table " + this.TableName.ObjectName + "'");
      stringBuilder.AppendLine("    alter table " + this.TableName.ObjectName + " add " + this.ToString());
      stringBuilder.AppendLine("end");
      writer.WriteTransaction(stringBuilder.ToString());
      if (this.TypeSpecifier.IsStringType)
      {
        stringBuilder.AppendLine("if not exists (select 1 from sys.columns where object_id = object_id(" + SQL.EncodeString(this.TableName.ObjectName) + ") and name = " + SQL.EncodeString(this.Name.GetUnbracketedObjectName()) + " and max_length = " + (object) this.TypeSpecifier.MaxLength + ")");
        stringBuilder.AppendLine("begin");
        stringBuilder.AppendLine("    print 'Altering column " + this.Name.ObjectName + " in table " + this.TableName.ObjectName + ": MaxLength set to " + (this.TypeSpecifier.MaxLength == -1 ? "max" : this.TypeSpecifier.MaxLength.ToString()) + "'");
        stringBuilder.AppendLine("    alter table " + this.TableName.ObjectName + " alter column " + this.ToString(false));
        stringBuilder.AppendLine("end");
      }
      if (this.IsNullable)
      {
        stringBuilder.AppendLine("if (select is_nullable from sys.columns where Object_id = Object_id(" + SQL.EncodeString(this.TableName.ObjectName) + ") and name = " + SQL.EncodeString(this.Name.GetUnbracketedObjectName()) + ") = 0");
        stringBuilder.AppendLine("begin");
        stringBuilder.AppendLine("    print 'Altering column " + this.Name.ObjectName + " in table " + this.TableName.ObjectName + ": Nullable constraint set to " + (this.IsNullable ? "NULL" : "NOT NULL") + "'");
        stringBuilder.AppendLine("    alter table " + this.TableName.ObjectName + " alter column " + this.ToString(false));
        stringBuilder.AppendLine("end");
      }
      writer.WriteTransaction(stringBuilder.ToString());
    }
  }
}
