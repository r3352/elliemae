// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.SchemaTypeSpecifier
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.DataAccess.DbSchema
{
  public class SchemaTypeSpecifier : SchemaElement
  {
    public SchemaTypeSpecifier(XElement xmlElement)
      : base(xmlElement)
    {
      this.TypeName = (string) (this.GetRelationship("Type") ?? throw new Exception("Cannot locate Type relationship for TypeSpecifier")).EntriesOfType<SchemaReference>().First<SchemaReference>().Name;
    }

    public string TypeName { get; private set; }

    public int MaxLength
    {
      get
      {
        try
        {
          return int.Parse(this.GetProperty("Length"));
        }
        catch
        {
          return -1;
        }
      }
    }

    public bool IsMaxLengthString => this.GetProperty("IsMax") == "True";

    public bool IsStringType
    {
      get
      {
        return string.Compare(this.TypeName, "[varchar]", true) == 0 || string.Compare(this.TypeName, "varchar", true) == 0 || string.Compare(this.TypeName, "[char]", true) == 0 || string.Compare(this.TypeName, "char", true) == 0;
      }
    }

    public override string ToString()
    {
      string str = this.TypeName;
      string property1 = this.GetProperty("Length");
      if (this.GetProperty("IsMax") == "True")
        str += "(max)";
      else if (!string.IsNullOrEmpty(property1))
      {
        str = str + "(" + property1 + ")";
      }
      else
      {
        string property2 = this.GetProperty("Precision");
        string property3 = this.GetProperty("Scale");
        if (!string.IsNullOrEmpty(property2) && !string.IsNullOrEmpty(property3))
          str = str + "(" + property2 + "," + property3 + ")";
        else if (!string.IsNullOrEmpty(property2))
          str = str + "(" + property2 + ")";
      }
      return str;
    }
  }
}
