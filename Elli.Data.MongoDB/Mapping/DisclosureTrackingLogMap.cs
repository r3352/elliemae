// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.DisclosureTrackingLogMap
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
  public class DisclosureTrackingLogMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (DisclosureTrackingLog)))
        return;
      BsonClassMap.RegisterClassMap<DisclosureTrackingLog>((Action<BsonClassMap<DisclosureTrackingLog>>) (cm =>
      {
        cm.AutoMap();
        cm.MapField("forms").SetElementName("Forms");
        cm.MapField("snapshotFields").SetElementName("SnapshotFields");
        cm.MapProperty<DateTime?>((Expression<Func<DisclosureTrackingLog, DateTime?>>) (c => c.ApplicationDate));
        cm.MapProperty<string>((Expression<Func<DisclosureTrackingLog, string>>) (c => c.DisclosedAPR));
        cm.MapProperty<string>((Expression<Func<DisclosureTrackingLog, string>>) (c => c.FinanceCharge));
        cm.MapProperty<string>((Expression<Func<DisclosureTrackingLog, string>>) (c => c.LoanProgram));
        cm.MapProperty<string>((Expression<Func<DisclosureTrackingLog, string>>) (c => c.LoanAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTrackingLog, string>>) (c => c.PropertyAddress));
        cm.MapProperty<string>((Expression<Func<DisclosureTrackingLog, string>>) (c => c.PropertyCity));
        cm.MapProperty<string>((Expression<Func<DisclosureTrackingLog, string>>) (c => c.PropertyState));
        cm.MapProperty<string>((Expression<Func<DisclosureTrackingLog, string>>) (c => c.PropertyZip));
        cm.UnmapProperty<string>((Expression<Func<DisclosureTrackingLog, string>>) (c => c.SnapshotXml));
      }));
      DisclosureFormMap.Register();
      LogAlertMap.Register();
      LogCommentMap.Register();
      LogSnapshotFieldMap.Register();
    }
  }
}
