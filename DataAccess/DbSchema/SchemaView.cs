// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.SchemaView
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System.Text;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.DataAccess.DbSchema
{
  public class SchemaView(XElement xmlElement) : SchemaElement(xmlElement)
  {
    public string Body => this.GetProperty("QueryScript");

    public void Publish(IScriptWriter writer)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("if exists (select 1 from sysobjects where id = object_id(" + SQL.EncodeString((string) this.Name) + "))");
      stringBuilder.AppendLine("begin");
      stringBuilder.AppendLine("    print 'Dropping view " + (string) this.Name + "'");
      stringBuilder.AppendLine("    drop view " + (string) this.Name);
      stringBuilder.AppendLine("end");
      writer.WriteTransaction(stringBuilder.ToString());
      writer.WriteTransaction("print 'Creating view " + (string) this.Name + "'");
      stringBuilder.Clear();
      stringBuilder.AppendLine("create view " + (string) this.Name);
      stringBuilder.AppendLine("as");
      stringBuilder.Append(this.Body.Replace("\n", "\r\n"));
      stringBuilder.AppendLine();
      writer.WriteTransaction(stringBuilder.ToString());
    }
  }
}
