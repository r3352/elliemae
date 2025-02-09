// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.VerifLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class VerifLog : DocumentLog
  {
    public new static readonly string XmlType = "Verif";
    private string id = "";

    public VerifLog(string verifId, string addedBy, string pairId)
      : base(addedBy, pairId)
    {
      this.id = verifId;
    }

    public VerifLog(LogList log, XmlElement e)
      : base(log, e)
    {
      this.id = new AttributeReader(e).GetString(nameof (Id));
    }

    public string Id => this.id;

    public override string ToString() => this.Title + " - " + this.RequestedFrom;

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) VerifLog.XmlType);
      attributeWriter.Write("Id", (object) this.Id);
    }
  }
}
