// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.ShippingContactMap
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
  public class ShippingContactMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (ShippingContact)))
        return;
      BsonClassMap.RegisterClassMap<ShippingContact>((Action<BsonClassMap<ShippingContact>>) (cm =>
      {
        cm.AutoMap();
        cm.UnmapProperty<Shipping>((Expression<Func<ShippingContact, Shipping>>) (c => c.Shipping));
      }));
    }
  }
}
