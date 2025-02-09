// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.MilestoneTaskLogMap
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
  public class MilestoneTaskLogMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (MilestoneTaskLog)))
        return;
      BsonClassMap.RegisterClassMap<MilestoneTaskLog>((Action<BsonClassMap<MilestoneTaskLog>>) (cm =>
      {
        cm.AutoMap();
        cm.MapProperty<bool>((Expression<Func<MilestoneTaskLog, bool>>) (c => c.Completed));
        cm.MapProperty<int>((Expression<Func<MilestoneTaskLog, int>>) (c => c.ContactCount));
        cm.MapProperty<DateTime>((Expression<Func<MilestoneTaskLog, DateTime>>) (c => c.ExpectedDate));
        cm.MapProperty<DateTime>((Expression<Func<MilestoneTaskLog, DateTime>>) (c => c.AddDate));
        cm.MapField("contacts").SetElementName("Contacts");
      }));
      LogAlertMap.Register();
      LogCommentMap.Register();
      MilestoneTaskContactMap.Register();
    }
  }
}
