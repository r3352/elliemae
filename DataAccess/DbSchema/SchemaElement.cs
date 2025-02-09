// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.SchemaElement
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.DataAccess.DbSchema
{
  public class SchemaElement
  {
    public SchemaElement(XElement xmlElement)
    {
      this.XmlElement = xmlElement;
      if (this.XmlElement.Attribute((XName) nameof (Name)) == null)
        return;
      this.Name = new QualifiedName((string) this.XmlElement.Attribute((XName) nameof (Name)));
    }

    public XElement XmlElement { get; private set; }

    public QualifiedName Name { get; protected set; }

    public string Type => (string) this.XmlElement.Attribute((XName) nameof (Type));

    public string GetProperty(string name)
    {
      XElement xelement = this.XmlElement.Elements().FirstOrDefault<XElement>((Func<XElement, bool>) (e => e.Name.LocalName == "Property" && e.Attribute((XName) "Name") != null && e.Attribute((XName) "Name").Value == name));
      if (xelement == null)
        return (string) null;
      XAttribute xattribute = xelement.Attribute((XName) "Value");
      if (xattribute != null)
        return xattribute.Value;
      return xelement.Elements().FirstOrDefault<XElement>((Func<XElement, bool>) (e => e.Name.LocalName == "Value"))?.Value;
    }

    public SchemaRelationship GetRelationship(string type)
    {
      XElement xmlElement = this.XmlElement.Elements().FirstOrDefault<XElement>((Func<XElement, bool>) (e => e.Name.LocalName == "Relationship" && e.Attribute((XName) "Name") != null && e.Attribute((XName) "Name").Value == type));
      return xmlElement != null ? new SchemaRelationship(xmlElement) : (SchemaRelationship) null;
    }
  }
}
