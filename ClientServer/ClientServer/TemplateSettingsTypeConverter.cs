// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TemplateSettingsTypeConverter
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class TemplateSettingsTypeConverter
  {
    private TemplateSettingsTypeConverter()
    {
    }

    public static TemplateSettingsType FromConditionType(ConditionType conditionType)
    {
      if (conditionType == ConditionType.Underwriting)
        return TemplateSettingsType.UnderwritingConditionSet;
      if (conditionType == ConditionType.PostClosing)
        return TemplateSettingsType.PostClosingConditionSet;
      throw new ArgumentException("Cannot resolve template type for condition type '" + (object) conditionType + "'");
    }

    public static ConditionType ToConditionType(TemplateSettingsType templateType)
    {
      if (templateType == TemplateSettingsType.UnderwritingConditionSet)
        return ConditionType.Underwriting;
      if (templateType == TemplateSettingsType.PostClosingConditionSet)
        return ConditionType.PostClosing;
      throw new ArgumentException("Cannot resolve condition type for template type '" + (object) templateType + "'");
    }

    public static TemplateSettingsType GetConditionTrackingViewType(ConditionType conditionType)
    {
      switch (conditionType)
      {
        case ConditionType.Underwriting:
          return TemplateSettingsType.UnderwritingConditionTrackingView;
        case ConditionType.PostClosing:
          return TemplateSettingsType.PostClosingConditionTrackingView;
        case ConditionType.Preliminary:
          return TemplateSettingsType.PreliminaryConditionTrackingView;
        case ConditionType.Sell:
          return TemplateSettingsType.SellConditionTrackingView;
        case ConditionType.Enhanced:
          return TemplateSettingsType.EnhancedConditionTrackingView;
        default:
          throw new ArgumentException("Cannot resolve template type for condition type '" + (object) conditionType + "'");
      }
    }

    public static BinaryConvertibleObject ConvertToTemplateObject(
      TemplateSettingsType type,
      BinaryObject data)
    {
      switch (type)
      {
        case TemplateSettingsType.LoanProgram:
          return (BinaryConvertibleObject) (LoanProgram) data;
        case TemplateSettingsType.ClosingCost:
          return (BinaryConvertibleObject) (ClosingCost) data;
        case TemplateSettingsType.MiscData:
          return (BinaryConvertibleObject) (DataTemplate) data;
        case TemplateSettingsType.FormList:
          return (BinaryConvertibleObject) (FormTemplate) data;
        case TemplateSettingsType.DocumentSet:
          return (BinaryConvertibleObject) (DocumentSetTemplate) data;
        case TemplateSettingsType.LoanTemplate:
          return (BinaryConvertibleObject) (LoanTemplate) data;
        case TemplateSettingsType.StackingOrder:
          return (BinaryConvertibleObject) (StackingOrderSetTemplate) data;
        case TemplateSettingsType.UnderwritingConditionSet:
        case TemplateSettingsType.PostClosingConditionSet:
          return (BinaryConvertibleObject) (ConditionSetTemplate) data;
        case TemplateSettingsType.Campaign:
          return (BinaryConvertibleObject) (CampaignTemplate) data;
        case TemplateSettingsType.TradePriceAdjustment:
          return (BinaryConvertibleObject) (PriceAdjustmentTemplate) data;
        case TemplateSettingsType.SRPTable:
          return (BinaryConvertibleObject) (SRPTableTemplate) data;
        case TemplateSettingsType.TradeFilter:
          return (BinaryConvertibleObject) (TradeFilterTemplate) data;
        case TemplateSettingsType.Investor:
          return (BinaryConvertibleObject) (InvestorTemplate) data;
        case TemplateSettingsType.PurchaseAdvice:
          return (BinaryConvertibleObject) (PurchaseAdviceTemplate) data;
        case TemplateSettingsType.FundingTemplate:
          return (BinaryConvertibleObject) (FundingTemplate) data;
        case TemplateSettingsType.DashboardTemplate:
          return (BinaryConvertibleObject) (DashboardTemplate) data;
        case TemplateSettingsType.PipelineView:
          return (BinaryConvertibleObject) (PipelineView) data;
        case TemplateSettingsType.BorrowerContactView:
        case TemplateSettingsType.BizPartnerView:
        case TemplateSettingsType.PublicBizPartnerView:
          return (BinaryConvertibleObject) (ContactView) data;
        case TemplateSettingsType.DashboardViewTemplate:
          return (BinaryConvertibleObject) (DashboardViewTemplate) data;
        case TemplateSettingsType.TaskSet:
          return (BinaryConvertibleObject) (TaskSetTemplate) data;
        case TemplateSettingsType.ConditionalLetter:
          return (BinaryConvertibleObject) (ConditionalLetterPrintOption) data;
        case TemplateSettingsType.SettlementServiceProviders:
          return (BinaryConvertibleObject) (SettlementServiceTemplate) data;
        case TemplateSettingsType.DocumentTrackingView:
          return (BinaryConvertibleObject) (DocumentTrackingView) data;
        case TemplateSettingsType.PreliminaryConditionTrackingView:
        case TemplateSettingsType.UnderwritingConditionTrackingView:
        case TemplateSettingsType.PostClosingConditionTrackingView:
        case TemplateSettingsType.SellConditionTrackingView:
        case TemplateSettingsType.EnhancedConditionTrackingView:
          return (BinaryConvertibleObject) (ConditionTrackingView) data;
        case TemplateSettingsType.MasterContractView:
        case TemplateSettingsType.TradeView:
        case TemplateSettingsType.CorrespondentMasterView:
        case TemplateSettingsType.CorrespondentTradeView:
        case TemplateSettingsType.GSECommitmentView:
          return (BinaryConvertibleObject) (TradeView) data;
        case TemplateSettingsType.TradeAssignedLoanView:
          return (BinaryConvertibleObject) (TradeView) data;
        case TemplateSettingsType.TradeAssignedLoanSecurityView:
          return (BinaryConvertibleObject) (SecurityTradeView) data;
        case TemplateSettingsType.SecurityTradeView:
          return (BinaryConvertibleObject) (SecurityTradeView) data;
        case TemplateSettingsType.LoanDuplicationTemplate:
          return (BinaryConvertibleObject) (LoanDuplicationTemplate) data;
        case TemplateSettingsType.DocumentExportTemplate:
          return (BinaryConvertibleObject) (DocumentExportTemplate) data;
        case TemplateSettingsType.AffiliatedBusinessArrangements:
          return (BinaryConvertibleObject) (AffiliateTemplate) data;
        case TemplateSettingsType.MbsPoolView:
          return (BinaryConvertibleObject) (TradeView) data;
        case TemplateSettingsType.PurchaseConditionSet:
          return (BinaryConvertibleObject) (PurchaseConditionSetTemplate) data;
        default:
          throw new ArgumentException("Unknown template type '" + (object) type + "'");
      }
    }
  }
}
