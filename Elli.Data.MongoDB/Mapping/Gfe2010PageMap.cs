// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.Gfe2010PageMap
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
  public class Gfe2010PageMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (Gfe2010Page)))
        return;
      BsonClassMap.RegisterClassMap<Gfe2010Page>((Action<BsonClassMap<Gfe2010Page>>) (cm =>
      {
        cm.AutoMap();
        cm.UnmapProperty<ClosingCost>((Expression<Func<Gfe2010Page, ClosingCost>>) (c => c.ClosingCost));
        cm.MapField("gfe2010GfeCharges").SetElementName("Gfe2010GfeCharges");
        cm.MapField("gfe2010FwbcFwscs").SetElementName("Gfe2010FwbcFwscs");
      }));
      Gfe2010FwbcFwscMap.Register();
      Gfe2010GfeChargeMap.Register();
    }
  }
}
