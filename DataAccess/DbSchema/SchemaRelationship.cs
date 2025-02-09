// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.SchemaRelationship
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.DataAccess.DbSchema
{
  public class SchemaRelationship : SchemaElement
  {
    public const string TypeSpecifier = "TypeSpecifier";
    public const string DefiningTable = "DefiningTable";
    public const string ForColumn = "ForColumn";
    public const string Columns = "Columns";
    public const string Column = "Column";
    public const string DataType = "Type";
    public const string IndexedObject = "IndexedObject";
    public const string ColumnSpecifications = "ColumnSpecifications";
    public const string ForeignColumns = "ForeignColumns";
    public const string ForeignTable = "ForeignTable";
    private readonly List<SchemaElement> _entries = new List<SchemaElement>();

    public SchemaRelationship(XElement xmlElement)
      : base(xmlElement)
    {
      foreach (XElement xmlElement1 in xmlElement.Elements().Where<XElement>((Func<XElement, bool>) (e => e.Name.LocalName == "Entry")))
        this._entries.Add(SchemaRelationship.ParseEntry(xmlElement1));
    }

    public IList<SchemaElement> Entries => (IList<SchemaElement>) this._entries;

    public ICollection<T> EntriesOfType<T>() where T : SchemaElement
    {
      return (ICollection<T>) this.Entries.Where<SchemaElement>((Func<SchemaElement, bool>) (e => e.GetType() == typeof (T))).Cast<T>().ToList<T>();
    }

    public static SchemaElement ParseEntry(XElement xmlElement)
    {
      XElement xmlElement1 = xmlElement.Elements().FirstOrDefault<XElement>((Func<XElement, bool>) (e => e.Name.LocalName == "References"));
      if (xmlElement1 != null)
        return (SchemaElement) new SchemaReference(xmlElement1);
      XElement xmlElement2 = xmlElement.Elements().FirstOrDefault<XElement>((Func<XElement, bool>) (e => e.NodeType == XmlNodeType.Element));
      if (xmlElement2 == null)
        throw new Exception("Cannot locate Element for relationship '" + (object) xmlElement.Parent.Attribute((XName) "Name") + "'");
      switch ((string) xmlElement2.Attribute((XName) "Type"))
      {
        case "SqlTypeSpecifier":
          return (SchemaElement) new SchemaTypeSpecifier(xmlElement2);
        case "SqlXmlTypeSpecifier":
          return (SchemaElement) new SchemaTypeSpecifier(xmlElement2);
        case "SqlSimpleColumn":
          return (SchemaElement) new SchemaColumn(xmlElement2);
        case "SqlIndexedColumnSpecification":
          return (SchemaElement) new SchemaIndexColumnSpec(xmlElement2);
        default:
          return new SchemaElement(xmlElement);
      }
    }
  }
}
