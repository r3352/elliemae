// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.Schema
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.DataAccess.DbSchema
{
  public class Schema
  {
    private readonly Dictionary<QualifiedName, SchemaElement> _elements = new Dictionary<QualifiedName, SchemaElement>();

    public Schema(TextReader schemaReader)
    {
      XDocument schemaDoc = XDocument.Load(schemaReader);
      this.ReadTables(schemaDoc);
      this.ReadIndexes((XContainer) schemaDoc);
      this.ReadFKeys((XContainer) schemaDoc);
      this.ReadViews((XContainer) schemaDoc);
    }

    public IEnumerable<SchemaElement> Elements()
    {
      return (IEnumerable<SchemaElement>) this._elements.Values;
    }

    public SchemaElement Element(QualifiedName qn)
    {
      return !this._elements.ContainsKey(qn) ? (SchemaElement) null : this._elements[qn];
    }

    public IEnumerable<T> Elements<T>() where T : SchemaElement => this.Elements().OfType<T>();

    public void Publish(IScriptWriter writer)
    {
      foreach (SchemaTable element in this.Elements<SchemaTable>())
        element.Publish(writer);
      foreach (SchemaIndex element in this.Elements<SchemaIndex>())
        element.Publish(writer);
      foreach (SchemaForeignKeyConstraint element in this.Elements<SchemaForeignKeyConstraint>())
        element.Publish(writer);
      foreach (SchemaView element in this.Elements<SchemaView>())
        element.Publish(writer);
    }

    private void ReadTables(XDocument schemaDoc)
    {
      foreach (SchemaTable schemaTable in schemaDoc.Descendants().Where<XElement>((Func<XElement, bool>) (e => e.NodeType == XmlNodeType.Element)).Where<XElement>((Func<XElement, bool>) (e => (string) e.Attribute((XName) "Type") == "SqlTable")).Select<XElement, SchemaTable>((Func<XElement, SchemaTable>) (e => new SchemaTable(e))))
        this._elements.Add(schemaTable.Name, (SchemaElement) schemaTable);
      foreach (SchemaPrimaryKeyConstraint primaryKeyConstraint in schemaDoc.Descendants().Where<XElement>((Func<XElement, bool>) (e => e.NodeType == XmlNodeType.Element)).Where<XElement>((Func<XElement, bool>) (e => (string) e.Attribute((XName) "Type") == "SqlPrimaryKeyConstraint")).Select<XElement, SchemaPrimaryKeyConstraint>((Func<XElement, SchemaPrimaryKeyConstraint>) (e => new SchemaPrimaryKeyConstraint(e))))
      {
        if (!this._elements.ContainsKey(primaryKeyConstraint.TableName))
          throw new Exception("Primary key exists for unknown table " + (string) primaryKeyConstraint.TableName);
        ((SchemaTable) this._elements[primaryKeyConstraint.TableName]).PrimaryKeyConstraint = primaryKeyConstraint;
      }
      foreach (SchemaDefaultConstraint defaultConstraint in schemaDoc.Descendants().Where<XElement>((Func<XElement, bool>) (e => e.NodeType == XmlNodeType.Element)).Where<XElement>((Func<XElement, bool>) (e => (string) e.Attribute((XName) "Type") == "SqlDefaultConstraint")).Select<XElement, SchemaDefaultConstraint>((Func<XElement, SchemaDefaultConstraint>) (e => new SchemaDefaultConstraint(e))))
      {
        if (!this._elements.ContainsKey(defaultConstraint.TableName))
          throw new Exception("Default constraint exists for unknown table " + (string) defaultConstraint.TableName);
        SchemaColumn schemaColumn = ((SchemaTable) this._elements[defaultConstraint.TableName]).Column(defaultConstraint.ColumnName);
        if (schemaColumn == null)
          throw new Exception("Default constraint exists for unknown column " + (string) defaultConstraint.ColumnName + " in table " + (string) defaultConstraint.TableName);
        schemaColumn.DefaultConstraint = defaultConstraint;
      }
    }

    private void ReadIndexes(XContainer schemaDoc)
    {
      foreach (SchemaIndex schemaIndex in schemaDoc.Descendants().Where<XElement>((Func<XElement, bool>) (e => e.NodeType == XmlNodeType.Element)).Where<XElement>((Func<XElement, bool>) (e => (string) e.Attribute((XName) "Type") == "SqlIndex")).Select<XElement, SchemaIndex>((Func<XElement, SchemaIndex>) (e => new SchemaIndex(e))))
        this._elements.Add(schemaIndex.Name, (SchemaElement) schemaIndex);
    }

    private void ReadFKeys(XContainer schemaDoc)
    {
      foreach (SchemaForeignKeyConstraint foreignKeyConstraint in schemaDoc.Descendants().Where<XElement>((Func<XElement, bool>) (e => e.NodeType == XmlNodeType.Element)).Where<XElement>((Func<XElement, bool>) (e => (string) e.Attribute((XName) "Type") == "SqlForeignKeyConstraint")).Select<XElement, SchemaForeignKeyConstraint>((Func<XElement, SchemaForeignKeyConstraint>) (e => new SchemaForeignKeyConstraint(e))))
        this._elements.Add(foreignKeyConstraint.Name, (SchemaElement) foreignKeyConstraint);
    }

    private void ReadViews(XContainer schemaDoc)
    {
      foreach (SchemaView schemaView in schemaDoc.Descendants().Where<XElement>((Func<XElement, bool>) (e => e.NodeType == XmlNodeType.Element)).Where<XElement>((Func<XElement, bool>) (e => (string) e.Attribute((XName) "Type") == "SqlView")).Select<XElement, SchemaView>((Func<XElement, SchemaView>) (e => new SchemaView(e))))
        this._elements.Add(schemaView.Name, (SchemaElement) schemaView);
    }
  }
}
