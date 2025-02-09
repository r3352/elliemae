// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.UlddMap
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
  public class UlddMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (Uldd)))
        return;
      BsonClassMap.RegisterClassMap<Uldd>((Action<BsonClassMap<Uldd>>) (cm =>
      {
        cm.AutoMap();
        cm.UnmapProperty<Loan>((Expression<Func<Uldd, Loan>>) (c => c.Loan));
      }));
    }
  }
}
