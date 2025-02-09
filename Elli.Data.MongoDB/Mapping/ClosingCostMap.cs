// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.ClosingCostMap
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
  public class ClosingCostMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (ClosingCost)))
        return;
      BsonClassMap.RegisterClassMap<ClosingCost>((Action<BsonClassMap<ClosingCost>>) (cm =>
      {
        cm.AutoMap();
        cm.UnmapProperty<Loan>((Expression<Func<ClosingCost, Loan>>) (c => c.Loan));
        cm.MapField("feeVariances").SetElementName("FeeVariances");
      }));
      Gfe2010Map.Register();
      Gfe2010PageMap.Register();
      Gfe2010SectionMap.Register();
      ClosingDisclosure1Map.Register();
      ClosingDisclosure2Map.Register();
      ClosingDisclosure3Map.Register();
      ClosingDisclosure4Map.Register();
      ClosingDisclosure5Map.Register();
      LoanEstimate1Map.Register();
      LoanEstimate2Map.Register();
      LoanEstimate3Map.Register();
      FeeVarianceMap.Register();
      FeeVarianceOtherMap.Register();
    }
  }
}
