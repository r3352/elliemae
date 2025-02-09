// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.LoanMap
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.Mortgage;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace Elli.Data.MongoDB.Mapping
{
  public class LoanMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (Loan)))
        return;
      BsonClassMap.RegisterClassMap<Loan>((Action<BsonClassMap<Loan>>) (cm =>
      {
        cm.AutoMap();
        cm.MapField("applications").SetElementName("Applications");
        cm.MapField("purchaseCredits").SetElementName("PurchaseCredits");
        cm.MapField("settlementServiceCharges").SetElementName("SettlementServiceCharges");
        cm.MapField("fees").SetElementName("Fees");
        cm.MapField("contacts").SetElementName("Contacts");
        cm.MapField("loanPrograms").SetElementName("LoanPrograms");
        cm.MapField("serviceProviderContacts").SetElementName("ServiceProviderContacts");
        cm.MapField("customFields").SetElementName("CustomFields");
        cm.MapField("logRecords").SetElementName("LogRecords");
        cm.MapField("forms").SetElementName("Forms");
        cm.MapField("_elliCustomAlerts").SetElementName("ElliCustomAlerts");
        cm.MapField("affiliatedBusinessArrangements").SetElementName("AffiliatedBusinessArrangements");
        cm.MapField("homeCounselingProviders").SetElementName("HomeCounselingProviders");
        cm.MapField("loanTags").SetElementName("LoanTags");
        cm.MapProperty<string>((Expression<Func<Loan, string>>) (c => c.AssemblyVersion));
        cm.UnmapProperty<Dictionary<object, bool>>((Expression<Func<Loan, Dictionary<object, bool>>>) (c => c.EmptyCollectionCache));
        cm.UnmapProperty<Dictionary<string, Tuple<object, object>>>((Expression<Func<Loan, Dictionary<string, Tuple<object, object>>>>) (c => c.CollectionModelPathCache));
        cm.UnmapProperty<Dictionary<object, Dictionary<string, object>>>((Expression<Func<Loan, Dictionary<object, Dictionary<string, object>>>>) (c => c.CollectionObjCache));
        cm.UnmapProperty<Dictionary<object, string>>((Expression<Func<Loan, Dictionary<object, string>>>) (c => c.ModelPathLookupCache));
      }));
      AdditionalRequestsMap.Register();
      AffiliatedBusinessArrangementMap.Register();
      ApplicationMap.Register();
      ATRQMCommonMap.Register();
      ClosingCostMap.Register();
      ClosingDocumentMap.Register();
      CommitmentTermsMap.Register();
      ComplianceTestLogMap.Register();
      ContactMap.Register();
      ConversationLogMap.Register();
      CrmLogMap.Register();
      CustomFieldMap.Register();
      DataTracLogMap.Register();
      DisclosureNoticesMap.Register();
      DisclosureTracking2015LogMap.Register();
      LogConditionMap.Register();
      DisclosureTrackingLogMap.Register();
      DocumentLogMap.Register();
      DocumentOrderLogMap.Register();
      DocumentSettingsMap.Register();
      DownloadLogMap.Register();
      DownPaymentMap.Register();
      EdmLogMap.Register();
      ElliCustomAlertMap.Register();
      ElliLOCompensationMap.Register();
      EmailTriggerLogMap.Register();
      EmDocumentMap.Register();
      EmDocumentInvestorMap.Register();
      EmDocumentLenderMap.Register();
      FannieMaeMap.Register();
      FeeMap.Register();
      FhaVaLoanMap.Register();
      FormMap.Register();
      FreddieMacMap.Register();
      FundingMap.Register();
      GfeMap.Register();
      HmdaMap.Register();
      HomeCounselingProviderMap.Register();
      HomeCounselingProviderSelectedMap.Register();
      HtmlEmailLogMap.Register();
      Hud1EsMap.Register();
      HudLoanDataMap.Register();
      InterimServicingMap.Register();
      LoanProductDataMap.Register();
      LoanProgramMap.Register();
      LoanSubmissionMap.Register();
      LockCancellationLogMap.Register();
      LockConfirmLogMap.Register();
      LockDenialLogMap.Register();
      LockRequestLogMap.Register();
      LogEntryLogMap.Register();
      LogRecordMap.Register();
      McawMap.Register();
      MilestoneFreeRoleLogMap.Register();
      MilestoneHistoryLogMap.Register();
      MilestoneLogMap.Register();
      MilestoneTaskLogMap.Register();
      MilestoneTemplateLogMap.Register();
      MiscellaneousMap.Register();
      NetTangibleBenefitMap.Register();
      PostClosingConditionLogMap.Register();
      PreliminaryConditionLogMap.Register();
      PrequalificationMap.Register();
      PrintLogMap.Register();
      PrivacyPolicyMap.Register();
      ProfitManagementMap.Register();
      PropertyMap.Register();
      PurchaseCreditMap.Register();
      RateLockMap.Register();
      RegistrationLogMap.Register();
      RegulationZMap.Register();
      Section32Map.Register();
      ServiceProviderContactMap.Register();
      ServicingDisclosureMap.Register();
      SettlementServiceChargeMap.Register();
      ShippingMap.Register();
      StateDisclosureMap.Register();
      StatementCreditDenialMap.Register();
      StatusOnlineLogMap.Register();
      TPOMap.Register();
      TQLMap.Register();
      TrustAccountMap.Register();
      TsumMap.Register();
      UlddMap.Register();
      UnderwriterSummaryMap.Register();
      UnderwritingConditionLogMap.Register();
      UsdaMap.Register();
      VaLoanDataMap.Register();
      VerificationLogMap.Register();
      CorrespondentMap.Register();
      MarginIndexLogMap.Register();
      DisclosureTrackingSnapshotMap.Register();
      LockRequestSnapshotMap.Register();
      DocumentOrderSnapshotMap.Register();
      OptimalBlueSnapshotMap.Register();
    }
  }
}
