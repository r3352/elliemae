// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.LoanProductDataMap
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
  public class LoanProductDataMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (LoanProductData)))
        return;
      BsonClassMap.RegisterClassMap<LoanProductData>((Action<BsonClassMap<LoanProductData>>) (cm =>
      {
        cm.AutoMap();
        cm.UnmapProperty<Loan>((Expression<Func<LoanProductData, Loan>>) (c => c.Loan));
        cm.MapField("buydowns").SetElementName("Buydowns");
        cm.MapField("helocRepaymentDrawPeriods").SetElementName("HelocRepaymentDrawPeriods");
        cm.MapField("prepaymentPenalties").SetElementName("PrepaymentPenalties");
      }));
      BuydownMap.Register();
      HelocRepaymentDrawPeriodMap.Register();
      PrepaymentPenaltyMap.Register();
    }
  }
}
