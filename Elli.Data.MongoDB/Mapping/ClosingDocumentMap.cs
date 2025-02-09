// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.ClosingDocumentMap
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.Mortgage;
using MongoDB.Bson.Serialization;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Elli.Data.MongoDB.Mapping
{
  public class ClosingDocumentMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (ClosingDocument)))
        return;
      BsonClassMap.RegisterClassMap<ClosingDocument>((Action<BsonClassMap<ClosingDocument>>) (cm =>
      {
        cm.AutoMap();
        cm.UnmapProperty<Loan>((Expression<Func<ClosingDocument, Loan>>) (c => c.Loan));
        cm.MapField("additionalStateDisclosures").SetElementName("AdditionalStateDisclosures");
        cm.MapField("antiSteeringLoanOptions").SetElementName("AntiSteeringLoanOptions");
        cm.MapField("stateLicenses").SetElementName("StateLicenses");
        cm.MapField("respaHudDetails").SetElementName("RespaHudDetails");
        cm.MapField("closingEntities").SetElementName("ClosingEntities");
      }));
      AdditionalStateDisclosureMap.Register();
      AntiSteeringLoanOptionMap.Register();
      ClosingEntityMap.Register();
      RespaHudDetailMap.Register();
      StateLicenseMap.Register();
    }
  }
}
