// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.BamObjects.FulfillmentRecipient
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Export.BamObjects
{
  public class FulfillmentRecipient
  {
    public string Id { get; set; }

    public DateTime PresumedDate { get; set; }

    public DateTime ActualDate { get; set; }

    public string Comment { get; set; }

    public FulfillmentRecipient(
      string id,
      DateTime presumedDate,
      DateTime actualDate,
      string comment)
    {
      this.Id = id;
      this.PresumedDate = presumedDate;
      this.ActualDate = actualDate;
      this.Comment = comment;
    }
  }
}
