// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.Bam
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.SimpleCache;
using EllieMae.EMLite.Common.TimeZones;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Export.BamEnums;
using EllieMae.EMLite.Export.BamObjects;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Export
{
  public class Bam : IBam
  {
    private const string className = "Bam";
    private string sw = Tracing.SwEpass;
    private bool debug = EnConfigurationSettings.GlobalSettings.Debug;
    private LoanData loanData;

    public Bam(LoanData loanData) => this.loanData = loanData;

    public int GetCurrentBorrowerPair() => this.loanData.GetPairIndex(this.loanData.PairId);

    public string GetField(string id) => this.loanData.GetField(id);

    public int GetNumberOfDeposits() => this.loanData.GetNumberOfDeposits();

    public int GetNumberOfEmployer(bool borrower) => this.loanData.GetNumberOfEmployer(borrower);

    public int GetNumberOfLiabilitesExlcudingAlimonyJobExp()
    {
      return this.loanData.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
    }

    public int GetNumberOfMortgages() => this.loanData.GetNumberOfMortgages();

    public int GetNumberOfResidence(bool borrower) => this.loanData.GetNumberOfResidence(borrower);

    public int GetNumberOfBorrowerPairs() => this.loanData.GetNumberOfBorrowerPairs();

    public int GetNumberOfSettlementServiceProviders()
    {
      return this.loanData.GetNumberOfSettlementServiceProviders();
    }

    public string GetSimpleField(string id) => this.loanData.GetSimpleField(id);

    public string GetSimpleField(string id, int pairIndex)
    {
      return this.loanData.GetSimpleField(id, pairIndex);
    }

    public bool IsLocked(string id) => this.loanData.IsLocked(id);

    public void SetBorrowerPair(int pairIndex) => this.loanData.SetBorrowerPair(pairIndex);

    public string GetUserSetting(string section, string key)
    {
      return Session.ISession.GetUser().GetPrivateProfileString(section, key);
    }

    public string GetVersion()
    {
      return VersionInformation.CurrentVersion.GetExtendedVersion(Session.EncompassEdition);
    }

    public string GetParentOranizationNMLSID(string loanOfficerID)
    {
      if (loanOfficerID == string.Empty)
        return string.Empty;
      IOrganizationManager organizationManager = (IOrganizationManager) Session.ISession.GetObject("OrganizationManager");
      UserInfo user = organizationManager.GetUser(loanOfficerID);
      if (user == (UserInfo) null)
        return string.Empty;
      OrgInfo organization = organizationManager.GetOrganization(user.OrgId);
      if (organization == null)
        return string.Empty;
      OrgInfo organizationWithNmls = organizationManager.GetFirstOrganizationWithNMLS(organization.Parent);
      return organizationWithNmls == null ? string.Empty : organizationWithNmls.NMLSCode;
    }

    public string GetCompanySetting(string section, string key)
    {
      return ((IConfigurationManager) Session.ISession.GetObject("ConfigurationManager")).GetCompanySetting(section, key);
    }

    public string GetUserID() => Session.ISession.UserID;

    public string GetClientID() => Session.CompanyInfo.ClientID;

    public bool IsSuperAdministrator() => Session.UserInfo.IsSuperAdministrator();

    public bool IsAdministrator() => Session.UserInfo.IsAdministrator();

    public bool IsTopLevelAdministrator() => Session.UserInfo.IsTopLevelAdministrator();

    public string ExportData(string format)
    {
      try
      {
        return new EllieMae.EMLite.Export.ExportData(Session.LoanDataMgr, this.loanData).Export(format);
      }
      catch
      {
        return string.Empty;
      }
    }

    public bool ValidateData(string format, bool allowContinue)
    {
      return new EllieMae.EMLite.Export.ExportData(Session.LoanDataMgr, this.loanData).Validate(format, allowContinue);
    }

    public string ToXml() => this.loanData.ToXml();

    public void GoToField(string fieldID, string exportType)
    {
      this.writeMethodCall(nameof (GoToField), (object) fieldID, (object) exportType);
      this.GoToField(fieldID, false, exportType);
    }

    public void GoToField(string fieldID, bool findNext, string exportType)
    {
      this.writeMethodCall(nameof (GoToField), (object) fieldID, (object) findNext, (object) exportType);
      Session.Application.GetService<ILoanEditor>().BAMGoToField(fieldID, findNext);
    }

    private void writeMethodCall(string methodName, params object[] parms)
    {
      try
      {
        if (!this.debug)
          return;
        Tracing.Log(true, "DEBUG", nameof (Bam), "BAM/" + (object) Thread.CurrentThread.GetHashCode() + ": " + methodName + "(" + Bam.paramsToString(parms) + ")");
      }
      catch
      {
      }
    }

    private static string paramsToString(object[] parms)
    {
      if (parms == null || parms.Length == 0)
        return "";
      string str1 = "";
      for (int index = 0; index < parms.Length; ++index)
      {
        object parm = parms[index];
        string str2;
        switch (parm)
        {
          case null:
            str2 = str1 + "null";
            break;
          case string _:
            str2 = str1 + "\"" + parm + "\"";
            break;
          case Enum _:
            str2 = str1 + parm.GetType().Name + "." + parm;
            break;
          case ValueType _:
            str2 = str1 + parm.ToString();
            break;
          default:
            if ((object) (parm as LoanIdentity) != null)
            {
              str2 = str1 + "<LoanIdentity(" + parm.ToString() + ")>";
              break;
            }
            if (parm is Array)
            {
              Array array = (Array) parm;
              Type elementType = array.GetType().GetElementType();
              str2 = str1 + "<" + elementType.Name + "[" + (object) array.Length + "]>";
              break;
            }
            str2 = str1 + "<" + parm.GetType().Name + ">";
            break;
        }
        str1 = str2 + (index < parms.Length - 1 ? ", " : "");
      }
      return str1;
    }

    public string GetFullVersion()
    {
      this.writeMethodCall(nameof (GetFullVersion));
      return VersionInformation.CurrentVersion.GetExtendedVersionWithHotfix(Session.EncompassEdition);
    }

    public DisclosureTrackingRecord2015[] GetDisclosureTracking2015Records()
    {
      this.writeMethodCall(nameof (GetDisclosureTracking2015Records));
      this.loanData.GetSnapshotDataForAllDisclosureTracking2015LogsForLoan();
      DisclosureTracking2015Log[] disclosureTracking2015Log = this.loanData.GetLogList().GetAllDisclosureTracking2015Log(true);
      List<DisclosureTrackingRecord2015> trackingRecord2015List = new List<DisclosureTrackingRecord2015>();
      foreach (DisclosureTracking2015Log log in disclosureTracking2015Log)
        trackingRecord2015List.Add(this.TransformToBam2015Log(log));
      return trackingRecord2015List.ToArray();
    }

    private DisclosedMethodEnum TranformToBam2015Method(
      DisclosureTrackingBase.DisclosedMethod disclosedMethod)
    {
      DisclosedMethodEnum bam2015Method = DisclosedMethodEnum.ByMail;
      switch (disclosedMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.None:
          bam2015Method = DisclosedMethodEnum.None;
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          bam2015Method = DisclosedMethodEnum.eDisclosure;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Fax:
          bam2015Method = DisclosedMethodEnum.Fax;
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          bam2015Method = DisclosedMethodEnum.InPerson;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          bam2015Method = DisclosedMethodEnum.Other;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Email:
          bam2015Method = DisclosedMethodEnum.Email;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Phone:
          bam2015Method = DisclosedMethodEnum.Phone;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          bam2015Method = DisclosedMethodEnum.Signature;
          break;
      }
      return bam2015Method;
    }

    private DisclosureTrackingRecord2015 TransformToBam2015Log(DisclosureTracking2015Log log)
    {
      DisclosureTrackingRecord2015 bam2015Log = new DisclosureTrackingRecord2015();
      bam2015Log.LEDisclosedByBroker = log.LEDisclosedByBroker;
      bam2015Log.DateAdded = log.DateAdded;
      bam2015Log.DisclosureTypeName = log.DisclosureTypeName;
      bam2015Log.LockedDisclosedDateField = log.LockedDisclosedDateField;
      bam2015Log.OriginalDisclosedDate = log.OriginalDisclosedDate;
      bam2015Log.DisclosedDate = log.DisclosedDate;
      bam2015Log.ProviderListSent = log.ProviderListSent;
      bam2015Log.DisclosedBy = log.DisclosedBy;
      bam2015Log.DisclosedByFullName = log.DisclosedByFullName;
      bam2015Log.LockedDisclosedByField = log.LockedDisclosedByField;
      bam2015Log.DisclosureMethod = this.TranformToBam2015Method(log.DisclosureMethod);
      bam2015Log.DisclosedMethodOther = log.DisclosedMethodOther;
      bam2015Log.BorrowerPairID = log.BorrowerPairID;
      bam2015Log.DisclosedMethodName = log.DisclosedMethodName;
      bam2015Log.ReceivedDate = log.ReceivedDate;
      bam2015Log.Received = log.Received;
      bam2015Log.IsDisclosedReceivedDateLocked = log.IsDisclosedReceivedDateLocked;
      bam2015Log.DisclosedAPR = log.DisclosedAPR;
      bam2015Log.FinanceCharge = log.FinanceCharge;
      bam2015Log.DisclosedDailyInterest = log.DisclosedDailyInterest;
      bam2015Log.ApplicationDate = log.ApplicationDate;
      bam2015Log.BorrowerName = log.BorrowerName;
      bam2015Log.CoBorrowerName = log.CoBorrowerName;
      bam2015Log.IsDisclosed = log.IsDisclosed;
      bam2015Log.IsLocked = log.IsLocked;
      bam2015Log.PropertyAddress = log.PropertyAddress;
      bam2015Log.PropertyCity = log.PropertyCity;
      bam2015Log.PropertyState = log.PropertyState;
      bam2015Log.PropertyZip = log.PropertyZip;
      bam2015Log.LoanProgram = log.LoanProgram;
      bam2015Log.LoanAmount = log.LoanAmount;
      bam2015Log.IsManuallyCreated = log.IsManuallyCreated;
      bam2015Log.IsDisclosedByLocked = log.IsDisclosedByLocked;
      bam2015Log.DisclosedForCD = log.DisclosedForCD;
      bam2015Log.DisclosedForLE = log.DisclosedForLE;
      bam2015Log.DisclosedForSafeHarbor = log.DisclosedForSafeHarbor;
      bam2015Log.IsDisclosedAPRLocked = log.IsDisclosedAPRLocked;
      bam2015Log.IsDisclosedFinanceChargeLocked = log.IsDisclosedFinanceChargeLocked;
      bam2015Log.IsDisclosedDailyInterestLocked = log.IsDisclosedDailyInterestLocked;
      bam2015Log.eDisclosureManualFulfillmentDate = log.eDisclosureManualFulfillmentDate;
      bam2015Log.eDisclosureManualFulfillmentMethod = this.TranformToBam2015Method(log.eDisclosureManualFulfillmentMethod);
      bam2015Log.LEReasonIsChangedCircumstanceSettlementCharges = log.LEReasonIsChangedCircumstanceSettlementCharges;
      bam2015Log.LEReasonIsChangedCircumstanceEligibility = log.LEReasonIsChangedCircumstanceEligibility;
      bam2015Log.LEReasonIsRevisionsRequestedByConsumer = log.LEReasonIsRevisionsRequestedByConsumer;
      bam2015Log.LEReasonIsInterestRateDependentCharges = log.LEReasonIsInterestRateDependentCharges;
      bam2015Log.LEReasonIsExpiration = log.LEReasonIsExpiration;
      bam2015Log.LEReasonIsDelayedSettlementOnConstructionLoans = log.LEReasonIsDelayedSettlementOnConstructionLoans;
      bam2015Log.LEReasonIsOther = log.LEReasonIsOther;
      bam2015Log.CDReasonIsChangeInAPR = log.CDReasonIsChangeInAPR;
      bam2015Log.CDReasonIsChangeInLoanProduct = log.CDReasonIsChangeInLoanProduct;
      bam2015Log.CDReasonIsPrepaymentPenaltyAdded = log.CDReasonIsPrepaymentPenaltyAdded;
      bam2015Log.CDReasonIsChangeInSettlementCharges = log.CDReasonIsChangeInSettlementCharges;
      bam2015Log.CDReasonIs24HourAdvancePreview = log.CDReasonIs24HourAdvancePreview;
      bam2015Log.CDReasonIsToleranceCure = log.CDReasonIsToleranceCure;
      bam2015Log.CDReasonIsClericalErrorCorrection = log.CDReasonIsClericalErrorCorrection;
      bam2015Log.CDReasonIsChangedCircumstanceEligibility = log.CDReasonIsChangedCircumstanceEligibility;
      bam2015Log.CDReasonIsRevisionsRequestedByConsumer = log.CDReasonIsRevisionsRequestedByConsumer;
      bam2015Log.CDReasonIsInterestRateDependentCharges = log.CDReasonIsInterestRateDependentCharges;
      bam2015Log.CDReasonIsOther = log.CDReasonIsOther;
      bam2015Log.LEReasonOther = log.LEReasonOther;
      bam2015Log.CDReasonOther = log.CDReasonOther;
      bam2015Log.ChangeInCircumstance = log.ChangeInCircumstance;
      bam2015Log.ChangeInCircumstanceComments = log.ChangeInCircumstanceComments;
      bam2015Log.IntentToProceed = log.IntentToProceed;
      bam2015Log.IntentToProceedDate = log.IntentToProceedDate;
      bam2015Log.IntentToProceedReceivedBy = log.IntentToProceedReceivedBy;
      bam2015Log.LockedIntentReceivedByField = log.LockedIntentReceivedByField;
      bam2015Log.IntentToProceedReceivedMethod = this.TranformToBam2015Method(log.IntentToProceedReceivedMethod);
      bam2015Log.IntentToProceedReceivedMethodOther = log.IntentToProceedReceivedMethodOther;
      bam2015Log.IntentToProceedComments = log.IntentToProceedComments;
      bam2015Log.IsIntentReceivedByLocked = log.IsIntentReceivedByLocked;
      bam2015Log.BorrowerDisclosedMethod = this.TranformToBam2015Method(log.BorrowerDisclosedMethod);
      bam2015Log.BorrowerDisclosedMethodOther = log.BorrowerDisclosedMethodOther;
      bam2015Log.IsBorrowerPresumedDateLocked = log.IsBorrowerPresumedDateLocked;
      bam2015Log.LockedBorrowerPresumedReceivedDate = log.LockedBorrowerPresumedReceivedDate;
      bam2015Log.BorrowerPresumedReceivedDate = log.BorrowerPresumedReceivedDate;
      bam2015Log.BorrowerActualReceivedDate = log.BorrowerActualReceivedDate;
      bam2015Log.BorrowerType = log.BorrowerType;
      bam2015Log.CoBorrowerDisclosedMethod = this.TranformToBam2015Method(log.CoBorrowerDisclosedMethod);
      bam2015Log.CoBorrowerDisclosedMethodOther = log.CoBorrowerDisclosedMethodOther;
      bam2015Log.IsCoBorrowerPresumedDateLocked = log.IsCoBorrowerPresumedDateLocked;
      bam2015Log.LockedCoBorrowerPresumedReceivedDate = log.LockedCoBorrowerPresumedReceivedDate;
      bam2015Log.CoBorrowerPresumedReceivedDate = log.CoBorrowerPresumedReceivedDate;
      bam2015Log.CoBorrowerActualReceivedDate = log.CoBorrowerActualReceivedDate;
      bam2015Log.CoBorrowerType = log.CoBorrowerType;
      bam2015Log.UCD = log.UCD;
      bam2015Log.UseForUCDExport = log.UseForUCDExport;
      bam2015Log.DisclosedFields = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (DisclosedLoanItem disclosedLoanItem in log.DisclosedData)
      {
        if (!bam2015Log.DisclosedFields.ContainsKey(disclosedLoanItem.FieldID))
          bam2015Log.DisclosedFields.Add(disclosedLoanItem.FieldID, disclosedLoanItem.FieldValue);
      }
      if (log.UCD != string.Empty)
      {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(log.UCD);
        foreach (KeyValuePair<string, string> keyValuePair in new UCDXmlParser(doc).ParseXml())
        {
          if (!bam2015Log.DisclosedFields.ContainsKey(keyValuePair.Key))
            bam2015Log.DisclosedFields.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      return bam2015Log;
    }

    public List<Dictionary<string, string>> GetFeeManagementSettings()
    {
      this.writeMethodCall(nameof (GetFeeManagementSettings));
      FeeManagementSetting feeManagement = Session.ConfigurationManager.GetFeeManagement();
      if (feeManagement == null)
        return (List<Dictionary<string, string>>) null;
      FeeManagementRecord[] allFees = feeManagement.GetAllFees();
      if (allFees == null)
        return (List<Dictionary<string, string>>) null;
      List<Dictionary<string, string>> managementSettings = new List<Dictionary<string, string>>();
      for (int index = 0; index < allFees.Length; ++index)
        managementSettings.Add(this.InsertFeeMngtIntoList(allFees[index]));
      return managementSettings;
    }

    private Dictionary<string, string> InsertFeeMngtIntoList(FeeManagementRecord feeManagementRecord)
    {
      return new Dictionary<string, string>()
      {
        {
          "Encompass Fee Name",
          feeManagementRecord.FeeName
        },
        {
          "Fee Name In Mavent",
          feeManagementRecord.FeeNameInMavent
        },
        {
          "Fee ID In Mavent",
          feeManagementRecord.FeeIDInMavent
        },
        {
          "FeeName In UCD",
          feeManagementRecord.FeeNameInUCD
        },
        {
          "FeeSource",
          feeManagementRecord.FeeSource
        },
        {
          "Section 700",
          this.ConvertYesNoNullBool(new bool?(feeManagementRecord.For700))
        },
        {
          "Section 800",
          this.ConvertYesNoNullBool(new bool?(feeManagementRecord.For800))
        },
        {
          "Section 900",
          this.ConvertYesNoNullBool(new bool?(feeManagementRecord.For900))
        },
        {
          "Section 1000",
          this.ConvertYesNoNullBool(new bool?(feeManagementRecord.For1000))
        },
        {
          "Section 1100",
          this.ConvertYesNoNullBool(new bool?(feeManagementRecord.For1100))
        },
        {
          "Section 1200",
          this.ConvertYesNoNullBool(new bool?(feeManagementRecord.For1200))
        },
        {
          "Section 1300",
          this.ConvertYesNoNullBool(new bool?(feeManagementRecord.For1300))
        },
        {
          "Section PC",
          this.ConvertYesNoNullBool(new bool?(feeManagementRecord.ForPC))
        }
      };
    }

    public Dictionary<string, string> GetFeeManagementPersonaSettings(int personaID)
    {
      this.writeMethodCall(nameof (GetFeeManagementPersonaSettings), (object) personaID);
      FieldAccessAclManager aclManager = (FieldAccessAclManager) Session.ACL.GetAclManager(AclCategory.FieldAccess);
      if (aclManager == null)
        return (Dictionary<string, string>) null;
      FeeManagementPersonaInfo managementPermission = aclManager.GetFeeManagementPermission((int[]) null);
      if (managementPermission == null)
        return (Dictionary<string, string>) null;
      Dictionary<string, string> managementPersonaSettings = new Dictionary<string, string>();
      if (personaID != 0)
      {
        FeeManagementPersonaRights personaRights = managementPermission.GetPersonaRights(personaID);
        managementPersonaSettings.Add("Persona ID", personaRights.PersonaID.ToString());
        managementPersonaSettings.Add("Section 700", this.ConvertYesNoNullBool(new bool?(personaRights.Overwrite700)));
        managementPersonaSettings.Add("Section 800", this.ConvertYesNoNullBool(new bool?(personaRights.Overwrite800)));
        managementPersonaSettings.Add("Section 900", this.ConvertYesNoNullBool(new bool?(personaRights.Overwrite900)));
        managementPersonaSettings.Add("Section 1000", this.ConvertYesNoNullBool(new bool?(personaRights.Overwrite1000)));
        managementPersonaSettings.Add("Section 1100", this.ConvertYesNoNullBool(new bool?(personaRights.Overwrite1100)));
        managementPersonaSettings.Add("Section 1200", this.ConvertYesNoNullBool(new bool?(personaRights.Overwrite1200)));
        managementPersonaSettings.Add("Section 1300", this.ConvertYesNoNullBool(new bool?(personaRights.Overwrite1300)));
        managementPersonaSettings.Add("Section PC", this.ConvertYesNoNullBool(new bool?(personaRights.OverwritePC)));
      }
      return managementPersonaSettings;
    }

    public List<Dictionary<string, string>> GetFeeManagementPersonaSettings()
    {
      this.writeMethodCall(nameof (GetFeeManagementPersonaSettings));
      FieldAccessAclManager aclManager = (FieldAccessAclManager) Session.ACL.GetAclManager(AclCategory.FieldAccess);
      if (aclManager == null)
        return (List<Dictionary<string, string>>) null;
      FeeManagementPersonaInfo managementPermission = aclManager.GetFeeManagementPermission((int[]) null);
      if (managementPermission == null)
        return (List<Dictionary<string, string>>) null;
      FeeManagementPersonaRights[] allPersonaRights = managementPermission.GetAllPersonaRights();
      if (allPersonaRights == null)
        return (List<Dictionary<string, string>>) null;
      List<Dictionary<string, string>> managementPersonaSettings = new List<Dictionary<string, string>>();
      for (int index = 0; index < allPersonaRights.Length; ++index)
        managementPersonaSettings.Add(this.InsertFeeMngtPersonaIntoList(allPersonaRights[index]));
      return managementPersonaSettings;
    }

    private Dictionary<string, string> InsertFeeMngtPersonaIntoList(
      FeeManagementPersonaRights feeManagementPersonaRights)
    {
      return new Dictionary<string, string>()
      {
        {
          "Persona Id",
          feeManagementPersonaRights.PersonaID.ToString()
        },
        {
          "Section 700",
          this.ConvertYesNoNullBool(new bool?(feeManagementPersonaRights.Overwrite700))
        },
        {
          "Section 800",
          this.ConvertYesNoNullBool(new bool?(feeManagementPersonaRights.Overwrite800))
        },
        {
          "Section 900",
          this.ConvertYesNoNullBool(new bool?(feeManagementPersonaRights.Overwrite900))
        },
        {
          "Section 1000",
          this.ConvertYesNoNullBool(new bool?(feeManagementPersonaRights.Overwrite1000))
        },
        {
          "Section 1100",
          this.ConvertYesNoNullBool(new bool?(feeManagementPersonaRights.Overwrite1100))
        },
        {
          "Section 1200",
          this.ConvertYesNoNullBool(new bool?(feeManagementPersonaRights.Overwrite1200))
        },
        {
          "Section 1300",
          this.ConvertYesNoNullBool(new bool?(feeManagementPersonaRights.Overwrite1300))
        },
        {
          "Section PC",
          this.ConvertYesNoNullBool(new bool?(feeManagementPersonaRights.OverwritePC))
        }
      };
    }

    private string ConvertYesNoNullBool(bool? type)
    {
      bool? nullable = type;
      if (!nullable.HasValue)
        return string.Empty;
      bool valueOrDefault = nullable.GetValueOrDefault();
      if (!valueOrDefault)
        return "N";
      return valueOrDefault ? "Y" : string.Empty;
    }

    public int GetNumberOfAdditionalLoans()
    {
      this.writeMethodCall(nameof (GetNumberOfAdditionalLoans));
      return this.loanData.GetNumberOfAdditionalLoans();
    }

    public int GetNumberOfGiftsAndGrants()
    {
      this.writeMethodCall(nameof (GetNumberOfGiftsAndGrants));
      return this.loanData.GetNumberOfGiftsAndGrants();
    }

    public int GetNumberOfOtherIncomeSources()
    {
      this.writeMethodCall(nameof (GetNumberOfOtherIncomeSources));
      return this.loanData.GetNumberOfOtherIncomeSources();
    }

    public int GetNumberOfOtherLiabilities()
    {
      this.writeMethodCall(nameof (GetNumberOfOtherLiabilities));
      return this.loanData.GetNumberOfOtherLiability();
    }

    public int GetNumberOfOtherAssets()
    {
      this.writeMethodCall(nameof (GetNumberOfOtherAssets));
      return this.loanData.GetNumberOfOtherAssets();
    }

    public int GetNumberOfAlternateNames(bool borrower)
    {
      this.writeMethodCall(nameof (GetNumberOfAlternateNames), (object) borrower);
      return this.loanData.GetNumberOfURLAAlternateNames(borrower);
    }

    public string GetAccessToken(string scope)
    {
      try
      {
        return new OAuth2(Session.DefaultInstance.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects), CacheItemRetentionPolicy.NoRetention).GetAccessToken(Session.DefaultInstance.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, scope).TypeAndToken;
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, TraceLevel.Verbose, nameof (Bam), "GetAccessToken: Error: " + ex.Message);
        return (string) null;
      }
    }

    public string GetUserPassword()
    {
      this.writeMethodCall(nameof (GetUserPassword));
      if (!string.IsNullOrWhiteSpace(Session.Password))
        return Session.Password;
      ICache simpleCache = CacheManager.GetSimpleCache("SsoTokenCache");
      string key = Session.DefaultInstance.SessionID + "_Elli.Emn";
      string userPassword = simpleCache.Get(key) as string;
      if (!string.IsNullOrWhiteSpace(userPassword))
        return userPassword;
      string ssoToken = Session.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
      {
        "Elli.Emn"
      }, 15);
      simpleCache.Put(key, new CacheItem((object) ssoToken));
      return ssoToken;
    }

    public EnhancedDisclosureTrackingRecord2015[] GetDisclosureTracking2015EnhancedRecords()
    {
      this.writeMethodCall(nameof (GetDisclosureTracking2015EnhancedRecords));
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = Session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(true);
      List<EnhancedDisclosureTrackingRecord2015> trackingRecord2015List = new List<EnhancedDisclosureTrackingRecord2015>();
      foreach (IDisclosureTracking2015Log log in idisclosureTracking2015Log)
        trackingRecord2015List.Add(this.transformToBam2015Enhanced(log));
      return trackingRecord2015List.ToArray();
    }

    public EnhancedDisclosureTrackingRecord2015 transformToBam2015Enhanced(
      IDisclosureTracking2015Log log)
    {
      EnhancedDisclosureTrackingRecord2015 bam2015Enhanced = new EnhancedDisclosureTrackingRecord2015();
      bam2015Enhanced.LEDisclosedByBroker = log.LEDisclosedByBroker;
      bam2015Enhanced.DisclosureTypeName = log.DisclosureTypeName;
      bam2015Enhanced.LockedDisclosedDateField = log.LockedDisclosedDateField;
      bam2015Enhanced.OriginalDisclosedDate = log.OriginalDisclosedDate;
      bam2015Enhanced.DisclosedDate = log.DisclosedDate;
      bam2015Enhanced.ProviderListSent = log.ProviderListSent;
      bam2015Enhanced.DisclosedBy = log.DisclosedBy;
      bam2015Enhanced.DisclosedByFullName = log.DisclosedByFullName;
      bam2015Enhanced.LockedDisclosedByField = log.LockedDisclosedByField;
      bam2015Enhanced.DisclosureMethod = this.tranformToBam2015Method(log.DisclosureMethod);
      bam2015Enhanced.DisclosedMethodOther = log.DisclosedMethodOther;
      bam2015Enhanced.BorrowerPairID = log.BorrowerPairID;
      bam2015Enhanced.DisclosedMethodName = log.DisclosedMethodName;
      bam2015Enhanced.ReceivedDate = log.ReceivedDate;
      bam2015Enhanced.Received = log.Received;
      bam2015Enhanced.IsDisclosedReceivedDateLocked = log.IsDisclosedReceivedDateLocked;
      bam2015Enhanced.DisclosedAPR = log.DisclosedAPR;
      bam2015Enhanced.FinanceCharge = log.FinanceCharge;
      bam2015Enhanced.DisclosedDailyInterest = log.DisclosedDailyInterest;
      bam2015Enhanced.IsDisclosed = log.IsDisclosed;
      bam2015Enhanced.IsLocked = log.IsLocked;
      bam2015Enhanced.IsDisclosedByLocked = log.IsDisclosedByLocked;
      bam2015Enhanced.IsDisclosedAPRLocked = log.IsDisclosedAPRLocked;
      bam2015Enhanced.DisclosedFields = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (DisclosedLoanItem disclosedLoanItem in log.DisclosedData)
      {
        if (!bam2015Enhanced.DisclosedFields.ContainsKey(disclosedLoanItem.FieldID))
          bam2015Enhanced.DisclosedFields.Add(disclosedLoanItem.FieldID, disclosedLoanItem.FieldValue);
      }
      bam2015Enhanced.UseForUCDExport = log.UseForUCDExport;
      if (log is EnhancedDisclosureTracking2015Log)
      {
        EnhancedDisclosureTracking2015Log disclosureTracking2015Log = (EnhancedDisclosureTracking2015Log) log;
        bam2015Enhanced.Status = disclosureTracking2015Log.Status.ToString();
        bam2015Enhanced.Provider = disclosureTracking2015Log.Provider;
        bam2015Enhanced.LoanAmount = disclosureTracking2015Log.LoanAmount.ToString();
        bam2015Enhanced.LoanProgram = disclosureTracking2015Log.LoanProgram;
        bam2015Enhanced.Contents = disclosureTracking2015Log.Contents.Select<EnhancedDisclosureTracking2015Log.DisclosureContentType, EllieMae.EMLite.Export.BamObjects.DisclosureContentType>((Func<EnhancedDisclosureTracking2015Log.DisclosureContentType, EllieMae.EMLite.Export.BamObjects.DisclosureContentType>) (c => (EllieMae.EMLite.Export.BamObjects.DisclosureContentType) c)).ToList<EllieMae.EMLite.Export.BamObjects.DisclosureContentType>();
        bam2015Enhanced.ApplicationDate = disclosureTracking2015Log.ApplicationDate;
        bam2015Enhanced.IsDisclosedFinanceChargeLocked = disclosureTracking2015Log.DisclosedFinanceCharge.UseUserValue;
        bam2015Enhanced.IsDisclosedDailyInterestLocked = disclosureTracking2015Log.DisclosedDailyInterest.UseUserValue;
        bam2015Enhanced.IsManuallyCreated = disclosureTracking2015Log.Provider == "Manual";
        bam2015Enhanced.DisclosureRecipients = new List<EllieMae.EMLite.Export.BamObjects.DisclosureRecipient>();
        foreach (EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient1 in (IEnumerable<EnhancedDisclosureTracking2015Log.DisclosureRecipient>) disclosureTracking2015Log.DisclosureRecipients)
        {
          EnhancedDisclosureTracking2015Log.DisclosureRecipientType role1 = disclosureRecipient1.Role;
          DisclosureTrackingBase.DisclosedMethod disclosedMethod1;
          DateTimeWithZone dateTimeWithZone;
          switch (role1)
          {
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower:
              EnhancedDisclosureTracking2015Log.BorrowerRecipient borrowerRecipient1 = (EnhancedDisclosureTracking2015Log.BorrowerRecipient) disclosureRecipient1;
              List<EllieMae.EMLite.Export.BamObjects.DisclosureRecipient> disclosureRecipients1 = bam2015Enhanced.DisclosureRecipients;
              string id1 = borrowerRecipient1.Id;
              string borrowerPairId1 = borrowerRecipient1.BorrowerPairId;
              string name1 = borrowerRecipient1.Name;
              string email1 = borrowerRecipient1.Email;
              role1 = borrowerRecipient1.Role;
              string role2 = role1.ToString();
              string roleDescription1 = borrowerRecipient1.RoleDescription;
              disclosedMethod1 = borrowerRecipient1.DisclosedMethod;
              string disclosedMethod2 = disclosedMethod1.ToString();
              string methodDescription1 = borrowerRecipient1.DisclosedMethodDescription;
              string borrowerType1 = borrowerRecipient1.BorrowerType.UseUserValue ? borrowerRecipient1.BorrowerType.UserValue : borrowerRecipient1.BorrowerType.ComputedValue;
              DateTime presumedReceivedDate1 = borrowerRecipient1.PresumedReceivedDate.UseUserValue ? borrowerRecipient1.PresumedReceivedDate.UserValue : borrowerRecipient1.PresumedReceivedDate.ComputedValue;
              DateTime actualReceivedDate1 = borrowerRecipient1.ActualReceivedDate;
              dateTimeWithZone = borrowerRecipient1.Tracking.AcceptConsentDate;
              DateTime dateTime1 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.ESignedDate;
              DateTime dateTime2 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.WetSignedDate;
              DateTime dateTime3 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.RejectConsentDate;
              DateTime dateTime4 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.ViewConsentDate;
              DateTime dateTime5 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.ViewMessageDate;
              DateTime dateTime6 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.AuthenticatedDate;
              DateTime dateTime7 = dateTimeWithZone.DateTime;
              string authenticatedIp1 = borrowerRecipient1.Tracking.AuthenticatedIP;
              string acceptConsentIp1 = borrowerRecipient1.Tracking.AcceptConsentIP;
              string rejectConsentIp1 = borrowerRecipient1.Tracking.RejectConsentIP;
              string esignedIp1 = borrowerRecipient1.Tracking.ESignedIP;
              string loanLevelConsent1 = borrowerRecipient1.Tracking.LoanLevelConsent;
              dateTimeWithZone = borrowerRecipient1.Tracking.ViewESignedDate;
              DateTime dateTime8 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.ViewWetSignedDate;
              DateTime dateTime9 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.InformationalViewedDate;
              DateTime dateTime10 = dateTimeWithZone.DateTime;
              string informationalViewedIp1 = borrowerRecipient1.Tracking.InformationalViewedIP;
              dateTimeWithZone = borrowerRecipient1.Tracking.InformationalCompletedDate;
              DateTime dateTime11 = dateTimeWithZone.DateTime;
              string informationalCompletedIp1 = borrowerRecipient1.Tracking.InformationalCompletedIP;
              EllieMae.EMLite.Export.BamObjects.DisclosureRecipientTracking tracking1 = new EllieMae.EMLite.Export.BamObjects.DisclosureRecipientTracking(dateTime1, dateTime2, dateTime3, dateTime4, dateTime5, dateTime6, dateTime7, authenticatedIp1, acceptConsentIp1, rejectConsentIp1, esignedIp1, loanLevelConsent1, dateTime8, dateTime9, dateTime10, informationalViewedIp1, dateTime11, informationalCompletedIp1);
              EllieMae.EMLite.Export.BamObjects.BorrowerRecipient borrowerRecipient2 = new EllieMae.EMLite.Export.BamObjects.BorrowerRecipient(id1, borrowerPairId1, name1, email1, role2, roleDescription1, disclosedMethod2, methodDescription1, borrowerType1, presumedReceivedDate1, actualReceivedDate1, tracking1);
              disclosureRecipients1.Add((EllieMae.EMLite.Export.BamObjects.DisclosureRecipient) borrowerRecipient2);
              continue;
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower:
              EnhancedDisclosureTracking2015Log.CoborrowerRecipient coborrowerRecipient = (EnhancedDisclosureTracking2015Log.CoborrowerRecipient) disclosureRecipient1;
              List<EllieMae.EMLite.Export.BamObjects.DisclosureRecipient> disclosureRecipients2 = bam2015Enhanced.DisclosureRecipients;
              string id2 = coborrowerRecipient.Id;
              string borrowerPairId2 = coborrowerRecipient.BorrowerPairId;
              string name2 = coborrowerRecipient.Name;
              string email2 = coborrowerRecipient.Email;
              role1 = coborrowerRecipient.Role;
              string role3 = role1.ToString();
              string roleDescription2 = coborrowerRecipient.RoleDescription;
              disclosedMethod1 = coborrowerRecipient.DisclosedMethod;
              string disclosedMethod3 = disclosedMethod1.ToString();
              string methodDescription2 = coborrowerRecipient.DisclosedMethodDescription;
              string borrowerType2 = coborrowerRecipient.BorrowerType.UseUserValue ? coborrowerRecipient.BorrowerType.UserValue : coborrowerRecipient.BorrowerType.ComputedValue;
              DateTime presumedReceivedDate2 = coborrowerRecipient.PresumedReceivedDate.UseUserValue ? coborrowerRecipient.PresumedReceivedDate.UserValue : coborrowerRecipient.PresumedReceivedDate.ComputedValue;
              DateTime actualReceivedDate2 = coborrowerRecipient.ActualReceivedDate;
              dateTimeWithZone = coborrowerRecipient.Tracking.AcceptConsentDate;
              DateTime dateTime12 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.ESignedDate;
              DateTime dateTime13 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.WetSignedDate;
              DateTime dateTime14 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.RejectConsentDate;
              DateTime dateTime15 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.ViewConsentDate;
              DateTime dateTime16 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.ViewMessageDate;
              DateTime dateTime17 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.AuthenticatedDate;
              DateTime dateTime18 = dateTimeWithZone.DateTime;
              string authenticatedIp2 = coborrowerRecipient.Tracking.AuthenticatedIP;
              string acceptConsentIp2 = coborrowerRecipient.Tracking.AcceptConsentIP;
              string rejectConsentIp2 = coborrowerRecipient.Tracking.RejectConsentIP;
              string esignedIp2 = coborrowerRecipient.Tracking.ESignedIP;
              string loanLevelConsent2 = coborrowerRecipient.Tracking.LoanLevelConsent;
              dateTimeWithZone = coborrowerRecipient.Tracking.ViewESignedDate;
              DateTime dateTime19 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.ViewWetSignedDate;
              DateTime dateTime20 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.InformationalViewedDate;
              DateTime dateTime21 = dateTimeWithZone.DateTime;
              string informationalViewedIp2 = coborrowerRecipient.Tracking.InformationalViewedIP;
              dateTimeWithZone = coborrowerRecipient.Tracking.InformationalCompletedDate;
              DateTime dateTime22 = dateTimeWithZone.DateTime;
              string informationalCompletedIp2 = coborrowerRecipient.Tracking.InformationalCompletedIP;
              EllieMae.EMLite.Export.BamObjects.DisclosureRecipientTracking tracking2 = new EllieMae.EMLite.Export.BamObjects.DisclosureRecipientTracking(dateTime12, dateTime13, dateTime14, dateTime15, dateTime16, dateTime17, dateTime18, authenticatedIp2, acceptConsentIp2, rejectConsentIp2, esignedIp2, loanLevelConsent2, dateTime19, dateTime20, dateTime21, informationalViewedIp2, dateTime22, informationalCompletedIp2);
              CoBorrowerRecipient borrowerRecipient3 = new CoBorrowerRecipient(id2, borrowerPairId2, name2, email2, role3, roleDescription2, disclosedMethod3, methodDescription2, borrowerType2, presumedReceivedDate2, actualReceivedDate2, tracking2);
              disclosureRecipients2.Add((EllieMae.EMLite.Export.BamObjects.DisclosureRecipient) borrowerRecipient3);
              continue;
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate:
              EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient2 = disclosureRecipient1;
              List<EllieMae.EMLite.Export.BamObjects.DisclosureRecipient> disclosureRecipients3 = bam2015Enhanced.DisclosureRecipients;
              string id3 = disclosureRecipient2.Id;
              string name3 = disclosureRecipient2.Name;
              string email3 = disclosureRecipient2.Email;
              role1 = disclosureRecipient2.Role;
              string role4 = role1.ToString();
              string roleDescription3 = disclosureRecipient2.RoleDescription;
              disclosedMethod1 = disclosureRecipient2.DisclosedMethod;
              string disclosedMethod4 = disclosedMethod1.ToString();
              string methodDescription3 = disclosureRecipient2.DisclosedMethodDescription;
              string borrowerType3 = disclosureRecipient2.BorrowerType.UseUserValue ? disclosureRecipient2.BorrowerType.UserValue : disclosureRecipient2.BorrowerType.ComputedValue;
              DateTime presumedReceivedDate3 = disclosureRecipient2.PresumedReceivedDate.UseUserValue ? disclosureRecipient2.PresumedReceivedDate.UserValue : disclosureRecipient2.PresumedReceivedDate.ComputedValue;
              DateTime actualReceivedDate3 = disclosureRecipient2.ActualReceivedDate;
              dateTimeWithZone = disclosureRecipient2.Tracking.AcceptConsentDate;
              DateTime dateTime23 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.ESignedDate;
              DateTime dateTime24 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.WetSignedDate;
              DateTime dateTime25 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.RejectConsentDate;
              DateTime dateTime26 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.ViewConsentDate;
              DateTime dateTime27 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.ViewMessageDate;
              DateTime dateTime28 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.AuthenticatedDate;
              DateTime dateTime29 = dateTimeWithZone.DateTime;
              string authenticatedIp3 = disclosureRecipient2.Tracking.AuthenticatedIP;
              string acceptConsentIp3 = disclosureRecipient2.Tracking.AcceptConsentIP;
              string rejectConsentIp3 = disclosureRecipient2.Tracking.RejectConsentIP;
              string esignedIp3 = disclosureRecipient2.Tracking.ESignedIP;
              string loanLevelConsent3 = disclosureRecipient2.Tracking.LoanLevelConsent;
              dateTimeWithZone = disclosureRecipient2.Tracking.ViewESignedDate;
              DateTime dateTime30 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.ViewWetSignedDate;
              DateTime dateTime31 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.InformationalViewedDate;
              DateTime dateTime32 = dateTimeWithZone.DateTime;
              string informationalViewedIp3 = disclosureRecipient2.Tracking.InformationalViewedIP;
              dateTimeWithZone = disclosureRecipient2.Tracking.InformationalCompletedDate;
              DateTime dateTime33 = dateTimeWithZone.DateTime;
              string informationalCompletedIp3 = disclosureRecipient2.Tracking.InformationalCompletedIP;
              EllieMae.EMLite.Export.BamObjects.DisclosureRecipientTracking tracking3 = new EllieMae.EMLite.Export.BamObjects.DisclosureRecipientTracking(dateTime23, dateTime24, dateTime25, dateTime26, dateTime27, dateTime28, dateTime29, authenticatedIp3, acceptConsentIp3, rejectConsentIp3, esignedIp3, loanLevelConsent3, dateTime30, dateTime31, dateTime32, informationalViewedIp3, dateTime33, informationalCompletedIp3);
              string userId = disclosureRecipient2.UserId;
              EllieMae.EMLite.Export.BamObjects.DisclosureRecipient disclosureRecipient3 = new EllieMae.EMLite.Export.BamObjects.DisclosureRecipient(id3, name3, email3, role4, roleDescription3, disclosedMethod4, methodDescription3, borrowerType3, presumedReceivedDate3, actualReceivedDate3, tracking3, userId);
              disclosureRecipients3.Add(disclosureRecipient3);
              continue;
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Other:
              EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient4 = disclosureRecipient1;
              List<EllieMae.EMLite.Export.BamObjects.DisclosureRecipient> disclosureRecipients4 = bam2015Enhanced.DisclosureRecipients;
              string id4 = disclosureRecipient4.Id;
              string name4 = disclosureRecipient4.Name;
              string email4 = disclosureRecipient4.Email;
              role1 = disclosureRecipient4.Role;
              string role5 = role1.ToString();
              string roleDescription4 = disclosureRecipient4.RoleDescription;
              disclosedMethod1 = disclosureRecipient4.DisclosedMethod;
              string disclosedMethod5 = disclosedMethod1.ToString();
              string methodDescription4 = disclosureRecipient4.DisclosedMethodDescription;
              string borrowerType4 = disclosureRecipient4.BorrowerType.UseUserValue ? disclosureRecipient4.BorrowerType.UserValue : disclosureRecipient4.BorrowerType.ComputedValue;
              DateTime presumedReceivedDate4 = disclosureRecipient4.PresumedReceivedDate.UseUserValue ? disclosureRecipient4.PresumedReceivedDate.UserValue : disclosureRecipient4.PresumedReceivedDate.ComputedValue;
              DateTime actualReceivedDate4 = disclosureRecipient4.ActualReceivedDate;
              dateTimeWithZone = disclosureRecipient4.Tracking.AcceptConsentDate;
              DateTime dateTime34 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.ESignedDate;
              DateTime dateTime35 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.WetSignedDate;
              DateTime dateTime36 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.RejectConsentDate;
              DateTime dateTime37 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.ViewConsentDate;
              DateTime dateTime38 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.ViewMessageDate;
              DateTime dateTime39 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.AuthenticatedDate;
              DateTime dateTime40 = dateTimeWithZone.DateTime;
              string authenticatedIp4 = disclosureRecipient4.Tracking.AuthenticatedIP;
              string acceptConsentIp4 = disclosureRecipient4.Tracking.AcceptConsentIP;
              string rejectConsentIp4 = disclosureRecipient4.Tracking.RejectConsentIP;
              string esignedIp4 = disclosureRecipient4.Tracking.ESignedIP;
              string loanLevelConsent4 = disclosureRecipient4.Tracking.LoanLevelConsent;
              dateTimeWithZone = disclosureRecipient4.Tracking.ViewESignedDate;
              DateTime dateTime41 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.ViewWetSignedDate;
              DateTime dateTime42 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.InformationalViewedDate;
              DateTime dateTime43 = dateTimeWithZone.DateTime;
              string informationalViewedIp4 = disclosureRecipient4.Tracking.InformationalViewedIP;
              dateTimeWithZone = disclosureRecipient4.Tracking.InformationalCompletedDate;
              DateTime dateTime44 = dateTimeWithZone.DateTime;
              string informationalCompletedIp4 = disclosureRecipient4.Tracking.InformationalCompletedIP;
              EllieMae.EMLite.Export.BamObjects.DisclosureRecipientTracking tracking4 = new EllieMae.EMLite.Export.BamObjects.DisclosureRecipientTracking(dateTime34, dateTime35, dateTime36, dateTime37, dateTime38, dateTime39, dateTime40, authenticatedIp4, acceptConsentIp4, rejectConsentIp4, esignedIp4, loanLevelConsent4, dateTime41, dateTime42, dateTime43, informationalViewedIp4, dateTime44, informationalCompletedIp4);
              EllieMae.EMLite.Export.BamObjects.DisclosureRecipient disclosureRecipient5 = new EllieMae.EMLite.Export.BamObjects.DisclosureRecipient(id4, name4, email4, role5, roleDescription4, disclosedMethod5, methodDescription4, borrowerType4, presumedReceivedDate4, actualReceivedDate4, tracking4);
              disclosureRecipients4.Add(disclosureRecipient5);
              continue;
            default:
              continue;
          }
        }
        bam2015Enhanced.Fulfillments = new List<EllieMae.EMLite.Export.BamObjects.FulfillmentFields>();
        foreach (EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillment in (IEnumerable<EnhancedDisclosureTracking2015Log.FulfillmentFields>) disclosureTracking2015Log.Fulfillments)
        {
          List<EllieMae.EMLite.Export.BamObjects.FulfillmentRecipient> recipients = new List<EllieMae.EMLite.Export.BamObjects.FulfillmentRecipient>();
          foreach (EnhancedDisclosureTracking2015Log.FulfillmentRecipient recipient in (IEnumerable<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>) fulfillment.Recipients)
          {
            List<EllieMae.EMLite.Export.BamObjects.FulfillmentRecipient> fulfillmentRecipientList = recipients;
            string id = recipient.Id;
            DateTimeWithZone dateTimeWithZone = recipient.PresumedDate;
            DateTime dateTime45 = dateTimeWithZone.DateTime;
            dateTimeWithZone = recipient.ActualDate;
            DateTime dateTime46 = dateTimeWithZone.DateTime;
            string comments = recipient.Comments;
            EllieMae.EMLite.Export.BamObjects.FulfillmentRecipient fulfillmentRecipient = new EllieMae.EMLite.Export.BamObjects.FulfillmentRecipient(id, dateTime45, dateTime46, comments);
            fulfillmentRecipientList.Add(fulfillmentRecipient);
          }
          bam2015Enhanced.Fulfillments.Add(new EllieMae.EMLite.Export.BamObjects.FulfillmentFields(fulfillment.IsManual, fulfillment.Id, this.tranformToBam2015Method(fulfillment.DisclosedMethod), recipients, fulfillment.OrderedBy, fulfillment.ProcessedDate.DateTime, fulfillment.TrackingNumber));
        }
        bam2015Enhanced.LoanEstimate = new EllieMae.EMLite.Export.BamObjects.LoanEstimateFields(disclosureTracking2015Log.LoanEstimate.IsDisclosedByBroker, disclosureTracking2015Log.LoanEstimate.IsChangedCircumstanceSettlementCharges, disclosureTracking2015Log.LoanEstimate.IsChangedCircumstanceEligibility, disclosureTracking2015Log.LoanEstimate.IsRevisionsRequestedByConsumer, disclosureTracking2015Log.LoanEstimate.IsInterestRateDependentCharges, disclosureTracking2015Log.LoanEstimate.IsExpiration, disclosureTracking2015Log.LoanEstimate.IsDelayedSettlementOnConstructionLoans, disclosureTracking2015Log.LoanEstimate.IsOther, disclosureTracking2015Log.LoanEstimate.OtherDescription, disclosureTracking2015Log.LoanEstimate.ChangesReceivedDate, disclosureTracking2015Log.LoanEstimate.RevisedDueDate);
        bam2015Enhanced.ClosingDisclosure = new EllieMae.EMLite.Export.BamObjects.ClosingDisclosureFields(disclosureTracking2015Log.ClosingDisclosure.IsChangeInAPR, disclosureTracking2015Log.ClosingDisclosure.IsChangeInLoanProduct, disclosureTracking2015Log.ClosingDisclosure.IsPrepaymentPenaltyAdded, disclosureTracking2015Log.ClosingDisclosure.IsChangeInSettlementCharges, disclosureTracking2015Log.ClosingDisclosure.Is24HourAdvancePreview, disclosureTracking2015Log.ClosingDisclosure.IsToleranceCure, disclosureTracking2015Log.ClosingDisclosure.IsClericalErrorCorrection, disclosureTracking2015Log.ClosingDisclosure.IsChangedCircumstanceEligibility, disclosureTracking2015Log.ClosingDisclosure.IsInterestRateDependentCharges, disclosureTracking2015Log.ClosingDisclosure.IsRevisionsRequestedByConsumer, disclosureTracking2015Log.ClosingDisclosure.IsOther, disclosureTracking2015Log.ClosingDisclosure.OtherDescription, disclosureTracking2015Log.ClosingDisclosure.ChangesReceivedDate, disclosureTracking2015Log.ClosingDisclosure.RevisedDueDate);
        bam2015Enhanced.IntentToProceed = new EllieMae.EMLite.Export.BamObjects.IntentToProceedFields(disclosureTracking2015Log.IntentToProceed.Intent, disclosureTracking2015Log.IntentToProceed.Date, disclosureTracking2015Log.IntentToProceed.ReceivedBy.UseUserValue ? disclosureTracking2015Log.IntentToProceed.ReceivedBy.UserValue : disclosureTracking2015Log.IntentToProceed.ReceivedBy.ComputedValue, this.tranformToBam2015Method(disclosureTracking2015Log.IntentToProceed.ReceivedMethod), disclosureTracking2015Log.IntentToProceed.ReceivedMethodOther, disclosureTracking2015Log.IntentToProceed.Comments);
        bam2015Enhanced.PropertyAddress = new EllieMae.EMLite.Export.BamObjects.Address(disclosureTracking2015Log.PropertyAddress.City, disclosureTracking2015Log.PropertyAddress.State, disclosureTracking2015Log.PropertyAddress.Street1, disclosureTracking2015Log.PropertyAddress.Street2, disclosureTracking2015Log.PropertyAddress.Zip);
        bam2015Enhanced.ChangeInCircumstance = disclosureTracking2015Log.ChangeInCircumstance;
        bam2015Enhanced.ChangeInCircumstanceComments = disclosureTracking2015Log.ChangeInCircumstanceComments;
        bam2015Enhanced.UCD = disclosureTracking2015Log.UCD;
        bam2015Enhanced.Tracking = new EllieMae.EMLite.Export.BamObjects.TrackingFields(this.transformToBAM2015TrackingIndicators(disclosureTracking2015Log.Tracking.Indicators), log.eDisclosureDisclosedMessage, log.eDisclosurePackageCreatedDate, log.eDisclosurePackageID, "");
        if (disclosureTracking2015Log.UCD != string.Empty)
        {
          XmlDocument doc = new XmlDocument();
          doc.LoadXml(disclosureTracking2015Log.UCD);
          foreach (KeyValuePair<string, string> keyValuePair in new UCDXmlParser(doc).ParseXml())
          {
            if (!bam2015Enhanced.DisclosedFields.ContainsKey(keyValuePair.Key))
              bam2015Enhanced.DisclosedFields.Add(keyValuePair.Key, keyValuePair.Value);
          }
        }
      }
      else
      {
        DisclosureTracking2015Log dt2015Log = (DisclosureTracking2015Log) log;
        bam2015Enhanced.Status = "Active";
        bam2015Enhanced.Provider = "Encompass";
        bam2015Enhanced.DisclosedDate = dt2015Log.DateAdded;
        bam2015Enhanced.LoanAmount = dt2015Log.LoanAmount.ToString();
        bam2015Enhanced.LoanProgram = dt2015Log.LoanProgram;
        bam2015Enhanced.ApplicationDate = dt2015Log.ApplicationDate;
        bam2015Enhanced.Contents = this.getDisclosureContentTypeforDT2015Log(dt2015Log);
        bam2015Enhanced.IsManuallyCreated = dt2015Log.IsManuallyCreated;
        bam2015Enhanced.DisclosureRecipients = new List<EllieMae.EMLite.Export.BamObjects.DisclosureRecipient>();
        string id5 = dt2015Log.Guid + "_borrower";
        string name5 = string.IsNullOrWhiteSpace(dt2015Log.eDisclosureBorrowerName) ? dt2015Log.BorrowerName : dt2015Log.eDisclosureBorrowerName;
        bam2015Enhanced.DisclosureRecipients.Add((EllieMae.EMLite.Export.BamObjects.DisclosureRecipient) new EllieMae.EMLite.Export.BamObjects.BorrowerRecipient(id5, dt2015Log.BorrowerPairID, name5, dt2015Log.eDisclosureBorrowerEmail, EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower.ToString(), string.Empty, dt2015Log.BorrowerDisclosedMethod.ToString(), dt2015Log.BorrowerDisclosedMethodOther, dt2015Log.IsBorrowerTypeLocked ? dt2015Log.LockedBorrowerType : dt2015Log.BorrowerType, dt2015Log.IsBorrowerPresumedDateLocked ? dt2015Log.BorrowerPresumedLockedReceivedDate : dt2015Log.BorrowerPresumedReceivedDate, dt2015Log.BorrowerActualReceivedDate, new EllieMae.EMLite.Export.BamObjects.DisclosureRecipientTracking(dt2015Log.eDisclosureBorrowerAcceptConsentDate, dt2015Log.eDisclosureBorrowereSignedDate, dt2015Log.eDisclosureBorrowerWetSignedDate, dt2015Log.eDisclosureBorrowerRejectConsentDate, dt2015Log.eDisclosureBorrowerViewConsentDate, dt2015Log.eDisclosureBorrowerViewMessageDate, dt2015Log.eDisclosureBorrowerAuthenticatedDate, dt2015Log.eDisclosureBorrowerAuthenticatedIP, dt2015Log.eDisclosureBorrowerAcceptConsentIP, dt2015Log.eDisclosureBorrowerRejectConsentIP, dt2015Log.eDisclosureBorrowereSignedIP, dt2015Log.EDisclosureBorrowerLoanLevelConsent, DateTime.MinValue, DateTime.MinValue)));
        string id6 = dt2015Log.Guid + "_coBorrower";
        string name6 = string.IsNullOrWhiteSpace(dt2015Log.eDisclosureCoBorrowerName) ? dt2015Log.CoBorrowerName : dt2015Log.eDisclosureCoBorrowerName;
        if (!string.IsNullOrWhiteSpace(name6))
          bam2015Enhanced.DisclosureRecipients.Add((EllieMae.EMLite.Export.BamObjects.DisclosureRecipient) new CoBorrowerRecipient(id6, dt2015Log.BorrowerPairID, name6, dt2015Log.eDisclosureCoBorrowerEmail, EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower.ToString(), string.Empty, dt2015Log.CoBorrowerDisclosedMethod.ToString(), dt2015Log.CoBorrowerDisclosedMethodOther, dt2015Log.IsCoBorrowerTypeLocked ? dt2015Log.LockedCoBorrowerType : dt2015Log.CoBorrowerType, dt2015Log.IsCoBorrowerPresumedDateLocked ? dt2015Log.CoBorrowerPresumedLockedReceivedDate : dt2015Log.CoBorrowerPresumedReceivedDate, dt2015Log.CoBorrowerActualReceivedDate, new EllieMae.EMLite.Export.BamObjects.DisclosureRecipientTracking(dt2015Log.eDisclosureCoBorrowerAcceptConsentDate, dt2015Log.eDisclosureCoBorrowereSignedDate, DateTime.MinValue, dt2015Log.eDisclosureCoBorrowerRejectConsentDate, dt2015Log.eDisclosureCoBorrowerViewConsentDate, dt2015Log.eDisclosureCoBorrowerViewMessageDate, dt2015Log.eDisclosureCoBorrowerAuthenticatedDate, dt2015Log.eDisclosureCoBorrowerAuthenticatedIP, dt2015Log.eDisclosureCoBorrowerAcceptConsentIP, dt2015Log.eDisclosureCoBorrowerRejectConsentIP, dt2015Log.eDisclosureCoBorrowereSignedIP, dt2015Log.EDisclosureCoBorrowerLoanLevelConsent, DateTime.MinValue, DateTime.MinValue)));
        if (!string.IsNullOrWhiteSpace(dt2015Log.eDisclosureLOName))
        {
          string id7 = dt2015Log.Guid + "_LoanOriginator";
          string disclosureLoName = dt2015Log.eDisclosureLOName;
          bam2015Enhanced.DisclosureRecipients.Add(new EllieMae.EMLite.Export.BamObjects.DisclosureRecipient(id7, disclosureLoName, "", EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate.ToString(), "Originator", string.Empty, string.Empty, string.Empty, DateTime.MinValue, DateTime.MinValue, new EllieMae.EMLite.Export.BamObjects.DisclosureRecipientTracking(DateTime.MinValue, dt2015Log.eDisclosureLOeSignedDate, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, dt2015Log.eDisclosureLOViewMessageDate, DateTime.MinValue, string.Empty, string.Empty, string.Empty, dt2015Log.eDisclosureLOeSignedIP, string.Empty, DateTime.MinValue, DateTime.MinValue), dt2015Log.eDisclosureLOUserId));
        }
        List<EllieMae.EMLite.Export.BamObjects.FulfillmentRecipient> recipients = new List<EllieMae.EMLite.Export.BamObjects.FulfillmentRecipient>();
        bam2015Enhanced.Fulfillments = new List<EllieMae.EMLite.Export.BamObjects.FulfillmentFields>();
        EllieMae.EMLite.Export.BamObjects.FulfillmentRecipient fulfillmentRecipient1 = new EllieMae.EMLite.Export.BamObjects.FulfillmentRecipient(id5, dt2015Log.PresumedFulfillmentDate, dt2015Log.ActualFulfillmentDate, dt2015Log.eDisclosureManualFulfillmentComment);
        recipients.Add(fulfillmentRecipient1);
        if (!string.IsNullOrWhiteSpace(name6))
        {
          EllieMae.EMLite.Export.BamObjects.FulfillmentRecipient fulfillmentRecipient2 = new EllieMae.EMLite.Export.BamObjects.FulfillmentRecipient(id5, dt2015Log.PresumedFulfillmentDate, dt2015Log.ActualFulfillmentDate, dt2015Log.eDisclosureManualFulfillmentComment);
          recipients.Add(fulfillmentRecipient2);
        }
        Guid guid;
        if (dt2015Log.eDisclosureManualFulfillmentDate == DateTime.MinValue)
        {
          List<EllieMae.EMLite.Export.BamObjects.FulfillmentFields> fulfillments = bam2015Enhanced.Fulfillments;
          guid = new Guid();
          EllieMae.EMLite.Export.BamObjects.FulfillmentFields fulfillmentFields = new EllieMae.EMLite.Export.BamObjects.FulfillmentFields(false, guid.ToString(), DisclosedMethodEnum.None, recipients, dt2015Log.FulfillmentOrderedBy, dt2015Log.FullfillmentProcessedDate, dt2015Log.FulfillmentTrackingNumber);
          fulfillments.Add(fulfillmentFields);
        }
        else
        {
          List<EllieMae.EMLite.Export.BamObjects.FulfillmentFields> fulfillments = bam2015Enhanced.Fulfillments;
          guid = new Guid();
          EllieMae.EMLite.Export.BamObjects.FulfillmentFields fulfillmentFields = new EllieMae.EMLite.Export.BamObjects.FulfillmentFields(false, guid.ToString(), this.tranformToBam2015Method(dt2015Log.eDisclosureManualFulfillmentMethod), recipients, dt2015Log.FulfillmentOrderedBy, dt2015Log.FullfillmentProcessedDate, dt2015Log.FulfillmentTrackingNumber);
          fulfillments.Add(fulfillmentFields);
        }
        bam2015Enhanced.LoanEstimate = new EllieMae.EMLite.Export.BamObjects.LoanEstimateFields(dt2015Log.LEDisclosedByBroker, dt2015Log.LEReasonIsChangedCircumstanceSettlementCharges, dt2015Log.LEReasonIsChangedCircumstanceEligibility, dt2015Log.LEReasonIsRevisionsRequestedByConsumer, dt2015Log.LEReasonIsInterestRateDependentCharges, dt2015Log.LEReasonIsExpiration, dt2015Log.LEReasonIsDelayedSettlementOnConstructionLoans, dt2015Log.LEReasonIsOther, dt2015Log.LEReasonOther, dt2015Log.ChangesReceivedDate, dt2015Log.RevisedDueDate);
        bam2015Enhanced.ClosingDisclosure = new EllieMae.EMLite.Export.BamObjects.ClosingDisclosureFields(dt2015Log.CDReasonIsChangeInAPR, dt2015Log.CDReasonIsChangeInLoanProduct, dt2015Log.CDReasonIsPrepaymentPenaltyAdded, dt2015Log.CDReasonIsChangeInSettlementCharges, dt2015Log.CDReasonIs24HourAdvancePreview, dt2015Log.CDReasonIsToleranceCure, dt2015Log.CDReasonIsClericalErrorCorrection, dt2015Log.CDReasonIsChangedCircumstanceEligibility, dt2015Log.CDReasonIsInterestRateDependentCharges, dt2015Log.CDReasonIsRevisionsRequestedByConsumer, dt2015Log.CDReasonIsOther, dt2015Log.CDReasonOther, dt2015Log.ChangesReceivedDate, dt2015Log.RevisedDueDate);
        bam2015Enhanced.IntentToProceed = new EllieMae.EMLite.Export.BamObjects.IntentToProceedFields(dt2015Log.IntentToProceed, dt2015Log.IntentToProceedDate, dt2015Log.IsIntentReceivedByLocked ? dt2015Log.LockedIntentReceivedByField : dt2015Log.IntentToProceedReceivedBy, this.tranformToBam2015Method(dt2015Log.IntentToProceedReceivedMethod), dt2015Log.IntentToProceedReceivedMethodOther, dt2015Log.IntentToProceedComments);
        bam2015Enhanced.Tracking = new EllieMae.EMLite.Export.BamObjects.TrackingFields(this.getTrackingIndicatorsforDT2015Log(dt2015Log), dt2015Log.eDisclosureDisclosedMessage, dt2015Log.eDisclosurePackageCreatedDate, dt2015Log.eDisclosurePackageID, dt2015Log.EDSRequestGuid);
        bam2015Enhanced.PropertyAddress = new EllieMae.EMLite.Export.BamObjects.Address(dt2015Log.PropertyCity, dt2015Log.PropertyState, dt2015Log.PropertyAddress, string.Empty, dt2015Log.PropertyZip);
        bam2015Enhanced.ChangeInCircumstance = dt2015Log.ChangeInCircumstance;
        bam2015Enhanced.ChangeInCircumstanceComments = dt2015Log.ChangeInCircumstanceComments;
        bam2015Enhanced.UCD = dt2015Log.UCD;
        if (dt2015Log.UCD != string.Empty)
        {
          XmlDocument doc = new XmlDocument();
          doc.LoadXml(dt2015Log.UCD);
          foreach (KeyValuePair<string, string> keyValuePair in new UCDXmlParser(doc).ParseXml())
          {
            if (!bam2015Enhanced.DisclosedFields.ContainsKey(keyValuePair.Key))
              bam2015Enhanced.DisclosedFields.Add(keyValuePair.Key, keyValuePair.Value);
          }
        }
      }
      return bam2015Enhanced;
    }

    private List<EllieMae.EMLite.Export.BamObjects.DisclosureContentType> getDisclosureContentTypeforDT2015Log(
      DisclosureTracking2015Log dt2015Log)
    {
      List<EllieMae.EMLite.Export.BamObjects.DisclosureContentType> typeforDt2015Log = new List<EllieMae.EMLite.Export.BamObjects.DisclosureContentType>();
      if (dt2015Log.DisclosedForLE)
        typeforDt2015Log.Add(EllieMae.EMLite.Export.BamObjects.DisclosureContentType.LE);
      if (dt2015Log.DisclosedForCD)
        typeforDt2015Log.Add(EllieMae.EMLite.Export.BamObjects.DisclosureContentType.CD);
      if (dt2015Log.DisclosedForSafeHarbor)
        typeforDt2015Log.Add(EllieMae.EMLite.Export.BamObjects.DisclosureContentType.SafeHarbor);
      if (dt2015Log.ProviderListSent)
        typeforDt2015Log.Add(EllieMae.EMLite.Export.BamObjects.DisclosureContentType.ServiceProviderList);
      if (dt2015Log.ProviderListNoFeeSent)
        typeforDt2015Log.Add(EllieMae.EMLite.Export.BamObjects.DisclosureContentType.ServiceProviderListNoFee);
      return typeforDt2015Log;
    }

    private DisclosedMethodEnum tranformToBam2015Method(
      DisclosureTrackingBase.DisclosedMethod disclosedMethod)
    {
      DisclosedMethodEnum bam2015Method = DisclosedMethodEnum.ByMail;
      switch (disclosedMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.None:
          bam2015Method = DisclosedMethodEnum.None;
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          bam2015Method = DisclosedMethodEnum.eDisclosure;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Fax:
          bam2015Method = DisclosedMethodEnum.Fax;
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          bam2015Method = DisclosedMethodEnum.InPerson;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          bam2015Method = DisclosedMethodEnum.Other;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Email:
          bam2015Method = DisclosedMethodEnum.Email;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Phone:
          bam2015Method = DisclosedMethodEnum.Phone;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          bam2015Method = DisclosedMethodEnum.Signature;
          break;
      }
      return bam2015Method;
    }

    private List<EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators> transformToBAM2015TrackingIndicators(
      IList<EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators> indicators)
    {
      List<EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators> trackingIndicators = new List<EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators>();
      if (indicators == null)
        return trackingIndicators;
      foreach (int indicator in (IEnumerable<EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators>) indicators)
      {
        switch (indicator)
        {
          case 0:
            trackingIndicators.Add(EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators.ApplicationPackage);
            continue;
          case 1:
            trackingIndicators.Add(EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators.ApprovalPackage);
            continue;
          case 2:
            trackingIndicators.Add(EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators.LockPackage);
            continue;
          case 3:
            trackingIndicators.Add(EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators.ThreeDayPackage);
            continue;
          default:
            continue;
        }
      }
      return trackingIndicators;
    }

    private List<EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators> getTrackingIndicatorsforDT2015Log(
      DisclosureTracking2015Log dt2015Log)
    {
      List<EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators> indicatorsforDt2015Log = new List<EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators>();
      if (dt2015Log.eDisclosureApplicationPackage)
        indicatorsforDt2015Log.Add(EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators.ApplicationPackage);
      if (dt2015Log.eDisclosureThreeDayPackage)
        indicatorsforDt2015Log.Add(EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators.ThreeDayPackage);
      if (dt2015Log.eDisclosureApprovalPackage)
        indicatorsforDt2015Log.Add(EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators.ApprovalPackage);
      if (dt2015Log.eDisclosureLockPackage)
        indicatorsforDt2015Log.Add(EllieMae.EMLite.Export.BamObjects.DT2015TrackingIndicators.LockPackage);
      return indicatorsforDt2015Log;
    }
  }
}
