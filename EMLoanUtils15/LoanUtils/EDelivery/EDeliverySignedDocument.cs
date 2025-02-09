// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.EDelivery.EDeliverySignedDocument
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.EDelivery
{
  public class EDeliverySignedDocument
  {
    public string title { get; set; }

    public string packageId { get; set; }

    public string signerName { get; set; }

    public string documentId { get; set; }

    public DateTime signedDate { get; set; }

    public string signedDocumentId { get; set; }
  }
}
