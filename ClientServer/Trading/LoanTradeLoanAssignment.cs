// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanTradeLoanAssignment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.TradeSynchronization;
using EllieMae.EMLite.Common;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Script.Serialization;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class LoanTradeLoanAssignment : TradeAssignment
  {
    private TradePriceAdjustments priceAdjustments;
    private SRPTable srpTable;
    private Investor investor;
    private ContactInformation assignee;
    private TradePricingInfo pricing;
    private LoanTradePairOffs loanTradePairOffs;

    [ScriptIgnore]
    public LoanTradeInfo LoanTradeInfo { get; set; }

    public string CommitmentNumber { get; set; }

    public DateTime CommitmentDate { get; set; }

    public string CommitmentType { get; set; }

    public string TradeDescription { get; set; }

    public string ContractNumber { get; set; }

    public Decimal TradeAmount { get; set; }

    public DateTime TargetDeliveryDate { get; set; }

    public DateTime ActualDeliveryDate { get; set; }

    public DateTime EarlyDeliveryDate { get; set; }

    public Decimal TotalPairoffAmount { get; set; }

    public Decimal Tolerance { get; set; }

    public bool Locked { get; set; }

    public string InvestorName { get; set; }

    public string InvestorCommitmentNumber { get; set; }

    public DateTime InvestorDeliveryDate { get; set; }

    public string InvestorTradeNumber { get; set; }

    public DateTime LastModified { get; set; }

    public int LoanCount { get; set; }

    public Decimal MiscFee { get; set; }

    public Decimal NetProfit { get; set; }

    public DateTime PurchaseDate { get; set; }

    public Decimal RateAdjustment { get; set; }

    public Decimal BuyUp { get; set; }

    public Decimal BuyDown { get; set; }

    public string Servicer { get; set; }

    public ServicingType ServicingType { get; set; }

    public Decimal TotalProfit { get; set; }

    public DateTime ShipmentDate { get; set; }

    public DateTime AssignedDate { get; set; }

    public string AssignedSecurityId { get; set; }

    public bool IsWeightedAvgBulkPriceLocked { get; set; }

    public Decimal WeightedAvgBulkPrice { get; set; }

    public bool IsBulkDelivery { get; set; }

    public TradePriceAdjustments PriceAdjustments
    {
      get => this.priceAdjustments;
      set => this.priceAdjustments = value;
    }

    public SRPTable SRPTable
    {
      get => this.srpTable;
      set => this.srpTable = value;
    }

    public Investor Investor
    {
      get => this.investor;
      set => this.investor = value;
    }

    public ContactInformation Assignee
    {
      get => this.assignee;
      set => this.assignee = value;
    }

    public TradePricingInfo Pricing
    {
      get => this.pricing;
      set => this.pricing = value;
    }

    public LoanTradePairOffs LoanTradePairOffs
    {
      get => this.loanTradePairOffs;
      set => this.loanTradePairOffs = value;
    }

    public int SecurityTradeID { get; set; } = -1;

    public int Id { get; private set; }

    public string Guid { get; set; }

    public Decimal OpenAmount { get; private set; }

    public Decimal CompletionPercentage { get; private set; }

    public Decimal MaxAmount { get; private set; }

    public Decimal MinAmount { get; private set; }

    public Decimal AssignedAmount { get; private set; }

    public Decimal GainLossAmount { get; private set; }

    public bool HasPendingLoan { get; private set; }

    public TradeStatus Status { get; private set; }

    public string StatusText => this.Status.ToDescription();

    public string ServicingTypeText => this.ServicingType.ToDescription();

    public LoanTradeLoanAssignment()
    {
    }

    public LoanTradeLoanAssignment(LoanTradeInfo tradeInfo, List<string> skipFieldList)
      : base(tradeInfo.TradeID, TradeType.LoanTrade, skipFieldList)
    {
      this.ConvertFromTradeInfoToTradeAssignment(tradeInfo);
      this.SkipFieldList = skipFieldList;
    }

    public LoanTradeLoanAssignment(string jsonString)
    {
      this.ConvertFromTradeAssignmentToTradeInfo(jsonString);
    }

    private void ConvertFromTradeInfoToTradeAssignment(LoanTradeInfo info)
    {
      this.LoanTradeInfo = info;
      this.Id = info.TradeID;
      this.TradeID = info.TradeID;
      this.Guid = info.Guid;
      this.CommitmentDate = info.CommitmentDate;
      this.CommitmentType = info.CommitmentType;
      this.CommitmentNumber = info.Name;
      this.ContractNumber = info.ContractNumber;
      this.LastModified = info.LastModified;
      this.Status = info.Status;
      this.TradeDescription = info.TradeDescription;
      this.HasPendingLoan = info.HasPendingLoan();
      this.LoanCount = info.LoanCount;
      this.InvestorName = info.Investor.Name;
      this.InvestorCommitmentNumber = info.InvestorCommitmentNumber;
      this.InvestorTradeNumber = info.InvestorTradeNumber;
      this.Servicer = info.Servicer;
      this.ServicingType = info.ServicingType;
      this.TargetDeliveryDate = info.TargetDeliveryDate;
      this.EarlyDeliveryDate = info.EarlyDeliveryDate;
      this.InvestorDeliveryDate = info.InvestorDeliveryDate;
      this.PurchaseDate = info.PurchaseDate;
      this.ShipmentDate = info.ShipmentDate;
      this.ActualDeliveryDate = info.ShipmentDate;
      this.BuyDown = info.BuyDownAmount;
      this.BuyUp = info.BuyUpAmount;
      this.RateAdjustment = info.RateAdjustment;
      this.TradeAmount = info.TradeAmount;
      this.Tolerance = info.Tolerance;
      this.MinAmount = (Decimal) (int) Math.Round(info.MinAmount, 0);
      this.MaxAmount = (Decimal) (int) Math.Round(info.MaxAmount, 0);
      this.OpenAmount = info.OpenAmount;
      this.CompletionPercentage = info.CompletionPercent;
      this.TotalPairoffAmount = info.PairOffAmount;
      this.GainLossAmount = info.GainLossAmount;
      this.MiscFee = info.MiscAdjustment;
      this.NetProfit = info.NetProfit;
      this.IsBulkDelivery = info.IsBulkDelivery;
      this.WeightedAvgBulkPrice = info.WeightedAvgBulkPrice;
      this.IsWeightedAvgBulkPriceLocked = info.IsWeightedAvgBulkPriceLocked;
      this.SecurityTradeID = info.SecurityTradeID;
      this.LoanTradePairOffs = info.LoanTradePairOffs;
      this.PriceAdjustments = info.PriceAdjustments;
      this.SRPTable = info.SRPTable;
      this.Investor = info.Investor;
      this.Assignee = info.Assignee;
      this.Pricing = info.Pricing;
    }

    private void ConvertFromTradeAssignmentToTradeInfo(string jsonString)
    {
      object obj1 = JsonConvert.DeserializeObject<object>(jsonString);
      this.LoanTradeInfo = new LoanTradeInfo();
      LoanTradeInfo loanTradeInfo1 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target1 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p1 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TradeID", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__0.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__0, obj1);
      int num1 = target1((CallSite) p1, obj2);
      loanTradeInfo1.TradeID = num1;
      LoanTradeInfo loanTradeInfo2 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target2 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p3 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Guid", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__2.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__2, obj1);
      string str1 = target2((CallSite) p3, obj3);
      loanTradeInfo2.Guid = str1;
      this.LoanTradeInfo.Archived = false;
      LoanTradeInfo loanTradeInfo3 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target3 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p5 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BuyDown", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__4.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__4, obj1);
      Decimal num2 = target3((CallSite) p5, obj4);
      loanTradeInfo3.BuyDownAmount = num2;
      LoanTradeInfo loanTradeInfo4 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target4 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p7 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BuyUp", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__6.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__6, obj1);
      Decimal num3 = target4((CallSite) p7, obj5);
      loanTradeInfo4.BuyUpAmount = num3;
      LoanTradeInfo loanTradeInfo5 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target5 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p11 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target6 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p10 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__10;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__9 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target7 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p9 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__9;
      Type type1 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CommitmentDate", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__8.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__8, obj1);
      object obj7 = target7((CallSite) p9, type1, obj6);
      object obj8 = target6((CallSite) p10, obj7);
      DateTime dateTime1 = target5((CallSite) p11, obj8);
      loanTradeInfo5.CommitmentDate = dateTime1;
      LoanTradeInfo loanTradeInfo6 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target8 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p13 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CommitmentType", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj9 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__12.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__12, obj1);
      string str2 = target8((CallSite) p13, obj9);
      loanTradeInfo6.CommitmentType = str2;
      LoanTradeInfo loanTradeInfo7 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target9 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__15.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p15 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__15;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CommitmentNumber", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__14.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__14, obj1);
      string str3 = target9((CallSite) p15, obj10);
      loanTradeInfo7.Name = str3;
      LoanTradeInfo loanTradeInfo8 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__19 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target10 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__19.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p19 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__19;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target11 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__18.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p18 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__18;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__17 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target12 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__17.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p17 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__17;
      Type type2 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "EarlyDeliveryDate", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj11 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__16.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__16, obj1);
      object obj12 = target12((CallSite) p17, type2, obj11);
      object obj13 = target11((CallSite) p18, obj12);
      DateTime dateTime2 = target10((CallSite) p19, obj13);
      loanTradeInfo8.EarlyDeliveryDate = dateTime2;
      LoanTradeInfo loanTradeInfo9 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__21 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target13 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__21.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p21 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__21;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InvestorCommitmentNumber", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj14 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__20.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__20, obj1);
      string str4 = target13((CallSite) p21, obj14);
      loanTradeInfo9.InvestorCommitmentNumber = str4;
      LoanTradeInfo loanTradeInfo10 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__25 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target14 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__25.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p25 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__25;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__24 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target15 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__24.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p24 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__24;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__23 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__23 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target16 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__23.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p23 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__23;
      Type type3 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__22 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InvestorDeliveryDate", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj15 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__22.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__22, obj1);
      object obj16 = target16((CallSite) p23, type3, obj15);
      object obj17 = target15((CallSite) p24, obj16);
      DateTime dateTime3 = target14((CallSite) p25, obj17);
      loanTradeInfo10.InvestorDeliveryDate = dateTime3;
      LoanTradeInfo loanTradeInfo11 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__27 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target17 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__27.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p27 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__27;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__26 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InvestorName", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj18 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__26.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__26, obj1);
      string str5 = target17((CallSite) p27, obj18);
      loanTradeInfo11.InvestorName = str5;
      LoanTradeInfo loanTradeInfo12 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__29 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target18 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__29.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p29 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__29;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__28 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InvestorTradeNumber", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj19 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__28.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__28, obj1);
      string str6 = target18((CallSite) p29, obj19);
      loanTradeInfo12.InvestorTradeNumber = str6;
      LoanTradeInfo loanTradeInfo13 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__31 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__31 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target19 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__31.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p31 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__31;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__30 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IsBulkDelivery", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj20 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__30.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__30, obj1);
      int num4 = target19((CallSite) p31, obj20) ? 1 : 0;
      loanTradeInfo13.IsBulkDelivery = num4 != 0;
      LoanTradeInfo loanTradeInfo14 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__33 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__33 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target20 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__33.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p33 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__33;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__32 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IsWeightedAvgBulkPriceLocked", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj21 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__32.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__32, obj1);
      int num5 = target20((CallSite) p33, obj21) ? 1 : 0;
      loanTradeInfo14.IsWeightedAvgBulkPriceLocked = num5 != 0;
      LoanTradeInfo loanTradeInfo15 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__35 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__35 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target21 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__35.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p35 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__35;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__34 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastModified", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj22 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__34.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__34, obj1);
      DateTime dateTime4 = target21((CallSite) p35, obj22);
      loanTradeInfo15.LastModified = dateTime4;
      LoanTradeInfo loanTradeInfo16 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__37 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__37 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target22 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__37.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p37 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__37;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__36 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ContractNumber", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj23 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__36.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__36, obj1);
      string str7 = target22((CallSite) p37, obj23);
      loanTradeInfo16.ContractNumber = str7;
      LoanTradeInfo loanTradeInfo17 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__39 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__39 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target23 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__39.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p39 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__39;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__38 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MiscFee", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj24 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__38.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__38, obj1);
      Decimal num6 = target23((CallSite) p39, obj24);
      loanTradeInfo17.MiscAdjustment = num6;
      LoanTradeInfo loanTradeInfo18 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__41 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__41 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target24 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__41.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p41 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__41;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__40 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__40 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "NetProfit", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj25 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__40.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__40, obj1);
      Decimal num7 = target24((CallSite) p41, obj25);
      loanTradeInfo18.NetProfit = num7;
      LoanTradeInfo loanTradeInfo19 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__43 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__43 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target25 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__43.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p43 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__43;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__42 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "RateAdjustment", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj26 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__42.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__42, obj1);
      Decimal num8 = target25((CallSite) p43, obj26);
      loanTradeInfo19.RateAdjustment = num8;
      LoanTradeInfo loanTradeInfo20 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__45 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__45 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target26 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__45.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p45 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__45;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__44 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Servicer", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj27 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__44.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__44, obj1);
      string str8 = target26((CallSite) p45, obj27);
      loanTradeInfo20.Servicer = str8;
      LoanTradeInfo loanTradeInfo21 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__47 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__47 = CallSite<Func<CallSite, object, ServicingType>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (ServicingType), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, ServicingType> target27 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__47.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, ServicingType>> p47 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__47;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__46 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__46 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ServicingType", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj28 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__46.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__46, obj1);
      int num9 = (int) target27((CallSite) p47, obj28);
      loanTradeInfo21.ServicingType = (ServicingType) num9;
      LoanTradeInfo loanTradeInfo22 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__51 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__51 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target28 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__51.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p51 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__51;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__50 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target29 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__50.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p50 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__50;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__49 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__49 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target30 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__49.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p49 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__49;
      Type type4 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__48 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ActualDeliveryDate", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj29 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__48.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__48, obj1);
      object obj30 = target30((CallSite) p49, type4, obj29);
      object obj31 = target29((CallSite) p50, obj30);
      DateTime dateTime5 = target28((CallSite) p51, obj31);
      loanTradeInfo22.ShipmentDate = dateTime5;
      LoanTradeInfo loanTradeInfo23 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__53 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__53 = CallSite<Func<CallSite, object, TradeStatus>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (TradeStatus), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, TradeStatus> target31 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__53.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, TradeStatus>> p53 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__53;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__52 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__52 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Status", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj32 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__52.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__52, obj1);
      int num10 = (int) target31((CallSite) p53, obj32);
      loanTradeInfo23.Status = (TradeStatus) num10;
      LoanTradeInfo loanTradeInfo24 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__57 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__57 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target32 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__57.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p57 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__57;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__56 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__56 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target33 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__56.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p56 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__56;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__55 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__55 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target34 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__55.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p55 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__55;
      Type type5 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__54 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TargetDeliveryDate", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj33 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__54.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__54, obj1);
      object obj34 = target34((CallSite) p55, type5, obj33);
      object obj35 = target33((CallSite) p56, obj34);
      DateTime dateTime6 = target32((CallSite) p57, obj35);
      loanTradeInfo24.TargetDeliveryDate = dateTime6;
      LoanTradeInfo loanTradeInfo25 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__59 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__59 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target35 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__59.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p59 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__59;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__58 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__58 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Tolerance", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj36 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__58.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__58, obj1);
      Decimal num11 = target35((CallSite) p59, obj36);
      loanTradeInfo25.Tolerance = num11;
      LoanTradeInfo loanTradeInfo26 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__61 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__61 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target36 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__61.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p61 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__61;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__60 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__60 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TotalPairoffAmount", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj37 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__60.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__60, obj1);
      Decimal num12 = target36((CallSite) p61, obj37);
      loanTradeInfo26.PairOffAmount = num12;
      LoanTradeInfo loanTradeInfo27 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__65 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__65 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target37 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__65.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p65 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__65;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__64 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__64 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target38 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__64.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p64 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__64;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__63 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__63 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target39 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__63.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p63 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__63;
      Type type6 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__62 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PurchaseDate", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj38 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__62.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__62, obj1);
      object obj39 = target39((CallSite) p63, type6, obj38);
      object obj40 = target38((CallSite) p64, obj39);
      DateTime dateTime7 = target37((CallSite) p65, obj40);
      loanTradeInfo27.PurchaseDate = dateTime7;
      LoanTradeInfo loanTradeInfo28 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__67 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__67 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target40 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__67.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p67 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__67;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__66 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__66 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TotalProfit", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj41 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__66.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__66, obj1);
      Decimal num13 = target40((CallSite) p67, obj41);
      loanTradeInfo28.TotalPairOffGainLoss = num13;
      LoanTradeInfo loanTradeInfo29 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__69 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__69 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target41 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__69.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p69 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__69;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__68 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__68 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TradeAmount", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj42 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__68.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__68, obj1);
      Decimal num14 = target41((CallSite) p69, obj42);
      loanTradeInfo29.TradeAmount = num14;
      LoanTradeInfo loanTradeInfo30 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__71 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__71 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target42 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__71.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p71 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__71;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__70 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__70 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TradeDescription", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj43 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__70.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__70, obj1);
      string str9 = target42((CallSite) p71, obj43);
      loanTradeInfo30.TradeDescription = str9;
      LoanTradeInfo loanTradeInfo31 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__73 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__73 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target43 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__73.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p73 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__73;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__72 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__72 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "WeightedAvgBulkPrice", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj44 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__72.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__72, obj1);
      Decimal num15 = target43((CallSite) p73, obj44);
      loanTradeInfo31.WeightedAvgBulkPrice = num15;
      LoanTradeInfo loanTradeInfo32 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__75 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__75 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target44 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__75.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p75 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__75;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__74 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__74 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SecurityTradeID", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj45 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__74.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__74, obj1);
      int num16 = target44((CallSite) p75, obj45);
      loanTradeInfo32.SecurityTradeID = num16;
      LoanTradeInfo loanTradeInfo33 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__78 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__78 = CallSite<Func<CallSite, object, LoanTradePairOffs>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (LoanTradePairOffs), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, LoanTradePairOffs> target45 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__78.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, LoanTradePairOffs>> p78 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__78;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__77 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__77 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "BuildLoanTradePairOffs", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target46 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__77.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p77 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__77;
      Type type7 = typeof (TradeLoanUpdateUtils);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__76 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__76 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LoanTradePairOffs", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj46 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__76.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__76, obj1);
      object obj47 = target46((CallSite) p77, type7, obj46);
      LoanTradePairOffs loanTradePairOffs = target45((CallSite) p78, obj47);
      loanTradeInfo33.LoanTradePairOffs = loanTradePairOffs;
      LoanTradeInfo loanTradeInfo34 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__81 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__81 = CallSite<Func<CallSite, object, TradePriceAdjustments>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (TradePriceAdjustments), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, TradePriceAdjustments> target47 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__81.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, TradePriceAdjustments>> p81 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__81;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__80 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__80 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "BuildPriceAdjustments", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target48 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__80.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p80 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__80;
      Type type8 = typeof (TradeLoanUpdateUtils);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__79 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__79 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PriceAdjustments", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj48 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__79.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__79, obj1);
      object obj49 = target48((CallSite) p80, type8, obj48);
      TradePriceAdjustments priceAdjustments = target47((CallSite) p81, obj49);
      loanTradeInfo34.PriceAdjustments = priceAdjustments;
      LoanTradeInfo loanTradeInfo35 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__84 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__84 = CallSite<Func<CallSite, object, SRPTable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (SRPTable), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, SRPTable> target49 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__84.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, SRPTable>> p84 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__84;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__83 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__83 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "BuildSrpTable", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target50 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__83.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p83 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__83;
      Type type9 = typeof (TradeLoanUpdateUtils);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__82 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__82 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SRPTable", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj50 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__82.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__82, obj1);
      object obj51 = target50((CallSite) p83, type9, obj50);
      SRPTable srpTable = target49((CallSite) p84, obj51);
      loanTradeInfo35.SRPTable = srpTable;
      LoanTradeInfo loanTradeInfo36 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__87 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__87 = CallSite<Func<CallSite, object, Investor>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Investor), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Investor> target51 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__87.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Investor>> p87 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__87;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__86 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__86 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "BuildInvestors", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target52 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__86.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p86 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__86;
      Type type10 = typeof (TradeLoanUpdateUtils);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__85 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__85 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Investor", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj52 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__85.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__85, obj1);
      object obj53 = target52((CallSite) p86, type10, obj52);
      Investor investor = target51((CallSite) p87, obj53);
      loanTradeInfo36.Investor = investor;
      LoanTradeInfo loanTradeInfo37 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__91 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__91 = CallSite<Func<CallSite, object, ContactInformation>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (ContactInformation), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, ContactInformation> target53 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__91.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, ContactInformation>> p91 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__91;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__90 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__90 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", (IEnumerable<Type>) new Type[1]
        {
          typeof (ContactInformation)
        }, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target54 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__90.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p90 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__90;
      Type type11 = typeof (JsonConvert);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__89 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__89 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target55 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__89.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p89 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__89;
      Type type12 = typeof (JsonConvert);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__88 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__88 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Assignee", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj54 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__88.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__88, obj1);
      object obj55 = target55((CallSite) p89, type12, obj54);
      object obj56 = target54((CallSite) p90, type11, obj55);
      ContactInformation contactInformation = target53((CallSite) p91, obj56);
      loanTradeInfo37.Assignee = contactInformation;
      LoanTradeInfo loanTradeInfo38 = this.LoanTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__94 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__94 = CallSite<Func<CallSite, object, TradePricingInfo>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (TradePricingInfo), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, TradePricingInfo> target56 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__94.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, TradePricingInfo>> p94 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__94;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__93 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__93 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "BuildPricingInfo", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target57 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__93.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p93 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__93;
      Type type13 = typeof (TradeLoanUpdateUtils);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__92 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__92 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Pricing", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj57 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__92.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__92, obj1);
      object obj58 = target57((CallSite) p93, type13, obj57);
      TradePricingInfo tradePricingInfo = target56((CallSite) p94, obj58);
      loanTradeInfo38.Pricing = tradePricingInfo;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__97 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__97 = CallSite<Func<CallSite, object, List<string>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (List<string>), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, List<string>> target58 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__97.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, List<string>>> p97 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__97;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__96 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__96 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "BuildSkipFieldList", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target59 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__96.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p96 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__96;
      Type type14 = typeof (TradeLoanUpdateUtils);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__95 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__95 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SkipFieldList", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj59 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__95.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__95, obj1);
      object obj60 = target59((CallSite) p96, type14, obj59);
      this.SkipFieldList = target58((CallSite) p97, obj60);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__99 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__99 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target60 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__99.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p99 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__99;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__98 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__98 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Id", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj61 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__98.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__98, obj1);
      this.Id = target60((CallSite) p99, obj61);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__101 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__101 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target61 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__101.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p101 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__101;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__100 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__100 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TradeID", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj62 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__100.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__100, obj1);
      this.TradeID = target61((CallSite) p101, obj62);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__103 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__103 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target62 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__103.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p103 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__103;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__102 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__102 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SecurityTradeID", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj63 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__102.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__102, obj1);
      this.SecurityTradeID = target62((CallSite) p103, obj63);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__105 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__105 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target63 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__105.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p105 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__105;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__104 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__104 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Guid", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj64 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__104.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__104, obj1);
      this.Guid = target63((CallSite) p105, obj64);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__107 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__107 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target64 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__107.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p107 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__107;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__106 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__106 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TradeDescription", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj65 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__106.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__106, obj1);
      this.TradeDescription = target64((CallSite) p107, obj65);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__111 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__111 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target65 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__111.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p111 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__111;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__110 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__110 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target66 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__110.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p110 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__110;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__109 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__109 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target67 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__109.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p109 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__109;
      Type type15 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__108 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__108 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ActualDeliveryDate", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj66 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__108.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__108, obj1);
      object obj67 = target67((CallSite) p109, type15, obj66);
      object obj68 = target66((CallSite) p110, obj67);
      this.ActualDeliveryDate = target65((CallSite) p111, obj68);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__113 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__113 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target68 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__113.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p113 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__113;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__112 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__112 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LoanSyncOption", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj69 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__112.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__112, obj1);
      this.LoanSyncOption = target68((CallSite) p113, obj69);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__115 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__115 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target69 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__115.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p115 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__115;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__114 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__114 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "FinalStatus", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj70 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__114.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__114, obj1);
      this.FinalStatus = target69((CallSite) p115, obj70);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__117 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__117 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target70 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__117.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p117 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__117;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__116 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__116 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SessionId", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj71 = LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__116.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__212.\u003C\u003Ep__116, obj1);
      this.SessionId = target70((CallSite) p117, obj71);
    }

    private SRPTable BuildSrpTable(string jsonString)
    {
      object obj1 = JsonConvert.DeserializeObject<object>(jsonString);
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p1 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SRPTable", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__0.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__0, obj1);
      object obj3 = target2((CallSite) p1, obj2, (object) null);
      if (target1((CallSite) p2, obj3))
        return (SRPTable) null;
      SRPTable.SRPPricingItems source = new SRPTable.SRPPricingItems();
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__31 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__31 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (LoanTradeLoanAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, IEnumerable> target3 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__31.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, IEnumerable>> p31 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__31;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PricingItems", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target4 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p4 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SRPTable", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__3.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__3, obj1);
      object obj5 = target4((CallSite) p4, obj4);
      foreach (object obj6 in target3((CallSite) p31, obj5))
      {
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__11 = CallSite<Func<CallSite, Type, object, object, Range<Decimal>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object, Range<Decimal>> target5 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__11.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object, Range<Decimal>>> p11 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__11;
        Type type1 = typeof (Range<Decimal>);
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__7 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDecimal", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target6 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__7.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p7 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__7;
        Type type2 = typeof (Convert);
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Minimum", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target7 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__6.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p6 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__6;
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LoanAmount", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__5.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__5, obj6);
        object obj8 = target7((CallSite) p6, obj7);
        object obj9 = target6((CallSite) p7, type2, obj8);
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__10 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDecimal", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target8 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__10.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p10 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__10;
        Type type3 = typeof (Convert);
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Maximum", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target9 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p9 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__9;
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LoanAmount", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj10 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__8.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__8, obj6);
        object obj11 = target9((CallSite) p9, obj10);
        object obj12 = target8((CallSite) p10, type3, obj11);
        Range<Decimal> loanAmount = target5((CallSite) p11, type1, obj9, obj12);
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (LoanTradeLoanAssignment)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, Decimal> target10 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__13.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, Decimal>> p13 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__13;
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BaseAdjustment", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj13 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__12.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__12, obj6);
        Decimal baseAdjustment = target10((CallSite) p13, obj13);
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (LoanTradeLoanAssignment)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, Decimal> target11 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__15.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, Decimal>> p15 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__15;
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ImpoundsAdjustment", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj14 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__14.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__14, obj6);
        Decimal impoundsAdjustment = target11((CallSite) p15, obj14);
        SRPTable.SRPStateAdjustments stateAdjustments1 = new SRPTable.SRPStateAdjustments();
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__27 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (LoanTradeLoanAssignment)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, IEnumerable> target12 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__27.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, IEnumerable>> p27 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__27;
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__16 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "StateAdjustments", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj15 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__16.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__16, obj6);
        foreach (object obj16 in target12((CallSite) p27, obj15))
        {
          SRPTable.SRPStateAdjustments stateAdjustments2 = stateAdjustments1;
          // ISSUE: reference to a compiler-generated field
          if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__26 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__26 = CallSite<Func<CallSite, Type, System.Guid, object, object, object, SRPTable.StateAdjustment>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, System.Guid, object, object, object, SRPTable.StateAdjustment> target13 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__26.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, System.Guid, object, object, object, SRPTable.StateAdjustment>> p26 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__26;
          Type type4 = typeof (SRPTable.StateAdjustment);
          // ISSUE: reference to a compiler-generated field
          if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__19 = CallSite<Func<CallSite, Type, object, System.Guid>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, System.Guid> target14 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__19.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, System.Guid>> p19 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__19;
          Type type5 = typeof (System.Guid);
          // ISSUE: reference to a compiler-generated field
          if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__18 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__18 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target15 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__18.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p18 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__18;
          Type type6 = typeof (Convert);
          // ISSUE: reference to a compiler-generated field
          if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Id", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj17 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__17.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__17, obj16);
          object obj18 = target15((CallSite) p18, type6, obj17);
          System.Guid guid = target14((CallSite) p19, type5, obj18);
          // ISSUE: reference to a compiler-generated field
          if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__21 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__21 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target16 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__21.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p21 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__21;
          Type type7 = typeof (Convert);
          // ISSUE: reference to a compiler-generated field
          if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__20 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "State", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj19 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__20.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__20, obj16);
          object obj20 = target16((CallSite) p21, type7, obj19);
          // ISSUE: reference to a compiler-generated field
          if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__23 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__23 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDecimal", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target17 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__23.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p23 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__23;
          Type type8 = typeof (Convert);
          // ISSUE: reference to a compiler-generated field
          if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__22 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Adjustment", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj21 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__22.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__22, obj16);
          object obj22 = target17((CallSite) p23, type8, obj21);
          // ISSUE: reference to a compiler-generated field
          if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__25 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__25 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDecimal", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target18 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__25.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p25 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__25;
          Type type9 = typeof (Convert);
          // ISSUE: reference to a compiler-generated field
          if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__24 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ImpoundAdjustment", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj23 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__24.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__24, obj16);
          object obj24 = target18((CallSite) p25, type9, obj23);
          SRPTable.StateAdjustment adj = target13((CallSite) p26, type4, guid, obj20, obj22, obj24);
          stateAdjustments2.Add(adj);
        }
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__30 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__30 = CallSite<Func<CallSite, Type, object, System.Guid>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, System.Guid> target19 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__30.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, System.Guid>> p30 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__30;
        Type type10 = typeof (System.Guid);
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__29 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__29 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target20 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__29.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p29 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__29;
        Type type11 = typeof (Convert);
        // ISSUE: reference to a compiler-generated field
        if (LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__28 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Id", typeof (LoanTradeLoanAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj25 = LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__28.Target((CallSite) LoanTradeLoanAssignment.\u003C\u003Eo__213.\u003C\u003Ep__28, obj6);
        object obj26 = target20((CallSite) p29, type11, obj25);
        SRPTable.PricingItem pricingItem = new SRPTable.PricingItem(target19((CallSite) p30, type10, obj26), loanAmount, baseAdjustment, impoundsAdjustment, stateAdjustments1);
        source.Add(pricingItem);
      }
      return new SRPTable(source);
    }

    [CLSCompliant(false)]
    public static string SerializeToJson(LoanTradeInfo info)
    {
      return new JavaScriptSerializer().Serialize((object) info);
    }
  }
}
