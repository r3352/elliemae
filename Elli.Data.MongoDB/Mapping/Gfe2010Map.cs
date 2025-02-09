// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.Gfe2010Map
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
  public class Gfe2010Map
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (Gfe2010)))
        return;
      BsonClassMap.RegisterClassMap<Gfe2010>((Action<BsonClassMap<Gfe2010>>) (cm =>
      {
        cm.AutoMap();
        cm.UnmapProperty<ClosingCost>((Expression<Func<Gfe2010, ClosingCost>>) (c => c.ClosingCost));
        cm.MapField("gfe2010Fees").SetElementName("Gfe2010Fees");
        cm.MapField("gfe2010WholePocs").SetElementName("Gfe2010WholePocs");
      }));
      Gfe2010FeeMap.Register();
      Gfe2010WholePocMap.Register();
    }
  }
}
