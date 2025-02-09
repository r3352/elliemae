// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.ApplicationMap
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
  public class ApplicationMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (Application)))
        return;
      BsonClassMap.RegisterClassMap<Application>((Action<BsonClassMap<Application>>) (cm =>
      {
        cm.AutoMap();
        cm.UnmapProperty<Loan>((Expression<Func<Application, Loan>>) (c => c.Loan));
        cm.MapField("assets").SetElementName("Assets");
        cm.MapField("income").SetElementName("Income");
        cm.MapField("liabilities").SetElementName("Liabilities");
        cm.MapField("employment").SetElementName("Employment");
        cm.MapField("residences").SetElementName("Residences");
        cm.MapField("selfEmployedIncomes").SetElementName("SelfEmployedIncomes");
        cm.MapField("reoProperties").SetElementName("ReoProperties");
        cm.MapField("tax4506s").SetElementName("Tax4506s");
        cm.MapField("tqlReports").SetElementName("TQLReports");
        cm.MapField("ausTrackingLogs").SetElementName("AUSTrackingLogs");
        cm.MapField("atrQmBorrowers").SetElementName("ATRQMBorrowers");
      }));
      AssetMap.Register();
      ATRQMBorrowerMap.Register();
      AUSTrackingLogMap.Register();
      BorrowerMap.Register();
      EmploymentMap.Register();
      IncomeMap.Register();
      LiabilityMap.Register();
      ReoPropertyMap.Register();
      ResidenceMap.Register();
      SelfEmployedIncomeMap.Register();
      Tax4506Map.Register();
      TQLReportInformationMap.Register();
    }
  }
}
