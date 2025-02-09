// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.PackageDetails
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class PackageDetails
  {
    public string id { get; set; }

    public string status { get; set; }

    public DateTime createdDate { get; set; }

    public Createdby createdBy { get; set; }

    public DateTime modifiedDate { get; set; }

    public string envelopeId { get; set; }

    public From from { get; set; }

    public bool notifyWhenViewed { get; set; }

    public RecipientList[] recipients { get; set; }

    public Document1[] documents { get; set; }

    public Custom custom { get; set; }
  }
}
