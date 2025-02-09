// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.RateLockMap
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
  public class RateLockMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (RateLock)))
        return;
      CustomDecimalSerializer customDecimalSerializer = new CustomDecimalSerializer(DecimalPlace.Ten);
      BsonClassMap.RegisterClassMap<RateLock>((Action<BsonClassMap<RateLock>>) (cm =>
      {
        cm.AutoMap();
        cm.UnmapProperty<Loan>((Expression<Func<RateLock, Loan>>) (c => c.Loan));
        cm.MapProperty<string>((Expression<Func<RateLock, string>>) (c => c.IsCancelled));
        cm.MapProperty<string>((Expression<Func<RateLock, string>>) (c => c.RateStatus));
        cm.MapProperty<string>((Expression<Func<RateLock, string>>) (c => c.RequestType));
        cm.MapProperty<string>((Expression<Func<RateLock, string>>) (c => c.RequestPending));
        cm.MapProperty<string>((Expression<Func<RateLock, string>>) (c => c.ExtensionRequestPending));
        cm.MapProperty<string>((Expression<Func<RateLock, string>>) (c => c.CancellationRequestPending));
        cm.MapProperty<string>((Expression<Func<RateLock, string>>) (c => c.ReLockRequestPending));
        cm.MapField("priceAdjustments").SetElementName("PriceAdjustments");
        cm.MapField("purchaseAdvicePayouts").SetElementName("PurchaseAdvicePayouts");
        cm.MapField("lockRequestBorrowers").SetElementName("LockRequestBorrowers");
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CurrentPriceRate)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CurrentPriceTotalAdjustment)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CurrentPriceRateRequested)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.RequestPriceRate)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.RequestPriceTotalAdjustment)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.RequestPriceRateRequested)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.BuySidePriceRate)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.BuySidePriceTotalAdjustment)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.BuySideRateNetBuyRate)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.TotalBuyPrice)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.SellSidePriceRate)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.SellSidePriceTotalAdjustment)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.SellSideNetSellPrice)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.GainLossTotalBuyPrice)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.GainLossPercentage)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CorrespondentFinalBuyPrice)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CorrespondentAdditionalLineAmount1)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CorrespondentAdditionalLineAmount2)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CorrespondentAdditionalLineAmount3)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.ProfitMarginAdjustedBuyPrice)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CompGainLossPercentage)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CompGainLossTotalCompPrice)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CompSideNetCompPrice)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CompSidePriceTotalAdjustment)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CompSidePriceRate)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CorrespondentLateFeePriceAdjustment)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CorrespondentAdjusterAmount1)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CorrespondentAdjusterAmount2)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
        cm.MapProperty<Decimal?>((Expression<Func<RateLock, Decimal?>>) (c => c.CorrespondentAdjusterAmount3)).SetIgnoreIfDefault(true).SetSerializer((IBsonSerializer) customDecimalSerializer);
      }));
      LockRequestBorrowerMap.Register();
      PriceAdjustmentMap.Register();
      PurchaseAdvicePayoutMap.Register();
    }
  }
}
