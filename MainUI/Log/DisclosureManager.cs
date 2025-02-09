// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.DisclosureManager
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class DisclosureManager : IDisclosureManager
  {
    public string[] DiscloseMethod { get; } = new string[8]
    {
      "U.S. Mail",
      "In Person",
      "Email",
      "eFolder eDisclosures",
      "Fax",
      "Other",
      "Closing Docs Order",
      "eClose"
    };

    public string[] SentMethod { get; } = new string[6]
    {
      "In Person",
      "Phone",
      "Email",
      "Signature",
      "Other",
      "eFolder eDisclosures"
    };

    public string[] BorrowerType { get; } = new string[11]
    {
      "",
      "Individual",
      "Co-signer",
      "Title Only",
      "Non Title Spouse",
      "Trustee",
      "Title Only Trustee",
      "Settlor Trustee",
      "Settlor",
      "Title Only Settlor Trustee",
      "Officer"
    };

    public string[] NboType { get; } = new string[4]
    {
      "Title only",
      "Non Title Spouse",
      "Title Only Trustee",
      "Title Only Settlor Trustee"
    };

    public bool CanEditSentDateAndExternalField { get; set; }

    public bool HasManualFulfillmentPermission { get; set; }

    public bool IsFulfillmentServiceEnabled { get; set; }

    public bool IsReasonsEnabled { get; set; }

    public bool FulfillmentUpdated { get; set; }

    public bool IntermediateData { get; set; }

    public bool HasAccessRight { get; }

    public bool FeeLevelIndicator { get; }

    public bool IsPlatformLoan { get; } = Session.LoanDataMgr.IsPlatformLoan();

    public bool IsEsignEnabled { get; }

    public IDisclosureTracking2015Log DisclosureTrackingLog { get; set; }

    public Dictionary<string, INonBorrowerOwnerItem> NBOItems { get; }

    public LoanData LoanData { get; } = Session.LoanData;

    public string eDisclosureManuallyFulfilledBy { get; set; }

    public DateTime eDisclosureManualFulfillmentDate { get; set; }

    public string AutomaticFullfillmentServiceName { get; } = "Encompass Fulfillment Service";

    public DateTime eDisclosurePresumedDate { get; set; }

    public DateTime eDisclosureActualDate { get; set; }

    public DisclosureTrackingBase.DisclosedMethod eDisclosureManualFulfillmentMethod { get; set; }

    public string eDisclosureManualFulfillmentComment { get; set; } = "";

    public Dictionary<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient> RecipientItems { get; }

    public string NboInstance { get; set; } = "";

    public string SelectedRecipientID { get; set; } = "";

    public bool IsLinkedLoan { get; }

    public bool IsConstructionPrimaryLoan
    {
      get
      {
        return this.LoanData != null && this.LoanData.LinkSyncType == LinkSyncType.ConstructionPrimary;
      }
    }

    public DateTime BorrowerReceivedDate { get; set; }

    public DateTime BorrowerActualReceivedDate { get; set; }

    public string[] eClosingDisclosedMethod
    {
      get
      {
        return ((IEnumerable<string>) this.DiscloseMethod).Except<string>((IEnumerable<string>) new string[3]
        {
          "eFolder eDisclosures",
          "Closing Docs Order",
          "eClose"
        }).ToArray<string>();
      }
    }

    public DateTime DisclosedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log disclosureTrackingLog = (EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog;
        if (!this.DisclosureTrackingLog.IsLocked)
          return disclosureTrackingLog.DisclosedDate.ComputedValue.DateTime;
        return !(disclosureTrackingLog.DisclosedDate.UserValue == DateTime.MinValue) ? disclosureTrackingLog.DisclosedDate.UserValue.Date : DateTime.MinValue;
      }
      set
      {
        EnhancedDisclosureTracking2015Log disclosureTrackingLog = (EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog;
        if (!this.DisclosureTrackingLog.IsLocked)
          return;
        disclosureTrackingLog.DisclosedDate.UserValue = value;
      }
    }

    public string TimezoneString { get; }

    public DisclosureManager(IDisclosureTracking2015Log disclosureTrackingLog, bool hasAccessRight)
    {
      this.DisclosureTrackingLog = disclosureTrackingLog;
      this.HasAccessRight = hasAccessRight;
      this.IsLinkedLoan = this.LoanData != null && this.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked;
      this.FeeLevelIndicator = this.LoanData.GetField("4461") == "Y";
      if (((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey("XCOCFeeLevelIndicator"))
        this.FeeLevelIndicator = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes["XCOCFeeLevelIndicator"] == "Y";
      if (this.NBOItems == null)
        this.NBOItems = new Dictionary<string, INonBorrowerOwnerItem>();
      if (disclosureTrackingLog.IsNboExist)
        this.InitNBOItems();
      this.InitSecurityFields();
      EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient1 = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).DisclosureRecipients.Where<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (r => r.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate && r.UserId == Session.UserInfo.Userid)).FirstOrDefault<EnhancedDisclosureTracking2015Log.DisclosureRecipient>();
      if (disclosureRecipient1 != null)
        this.IsEsignEnabled = this.IsPlatformLoan && disclosureRecipient1.Tracking.ESignedDate.DateTime == DateTime.MinValue;
      this.InitReceivedDateAndRevisedDueDate();
      this.RecipientItems = new Dictionary<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient>();
      foreach (EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient2 in (IEnumerable<EnhancedDisclosureTracking2015Log.DisclosureRecipient>) ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).DisclosureRecipients)
      {
        if (!string.IsNullOrEmpty(disclosureRecipient2.Name))
          this.RecipientItems.Add(disclosureRecipient2.Id, disclosureRecipient2.Clone());
      }
      string str = this.LoanData.GetField("LE1.XG9") == "" ? this.LoanData.GetField("LE1.X9") : this.LoanData.GetField("LE1.XG9");
      this.TimezoneString = EnhancedDisclosureTracking2015Log.UseLE1X9ForTimeZone(this.LoanData) ? str : "PST";
    }

    public string GetTimeZoneString(DateTime dt)
    {
      if (!EnhancedDisclosureTracking2015Log.UseLE1X9ForTimeZone(this.LoanData))
        return "PST";
      string field = this.LoanData.GetField("LE1.XG9");
      if (field == "")
        return this.LoanData.GetField("LE1.X9");
      bool isDaylightSavingTime = false;
      if (dt != DateTime.MinValue)
        isDaylightSavingTime = System.TimeZoneInfo.Local.IsDaylightSavingTime(dt);
      return Utils.TransformTimezoneToStandardTimezone(field, isDaylightSavingTime);
    }

    private void InitReceivedDateAndRevisedDueDate()
    {
      string str1 = "";
      string str2 = "";
      if (this.DisclosureTrackingLog.DisclosedForLE)
      {
        if (((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey("ChangesLEReceivedDate"))
          str1 = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes["ChangesLEReceivedDate"];
        if (((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey("RevisedLEDueDate"))
          str2 = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes["RevisedLEDueDate"];
        if (Utils.IsDate((object) str1))
          ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).LoanEstimate.ChangesReceivedDate = Convert.ToDateTime(str1);
        if (Utils.IsDate((object) str2))
          ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).LoanEstimate.RevisedDueDate = Convert.ToDateTime(str2);
      }
      if (!this.DisclosureTrackingLog.DisclosedForCD)
        return;
      if (((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey("ChangesCDReceivedDate"))
        str1 = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes["ChangesCDReceivedDate"];
      if (((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey("RevisedCDDueDate"))
        str2 = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes["RevisedCDDueDate"];
      if (Utils.IsDate((object) str1))
        ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).ClosingDisclosure.ChangesReceivedDate = Convert.ToDateTime(str1);
      if (!Utils.IsDate((object) str2))
        return;
      ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).ClosingDisclosure.RevisedDueDate = Convert.ToDateTime(str2);
    }

    private void InitNBOItems()
    {
      foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in this.DisclosureTrackingLog.GetAllnboItems())
        this.NBOItems.Add(allnboItem.Key, allnboItem.Value.CloneForDuplicate());
    }

    public void SetReceivedDateForNBOs(DateTime disclosedDate)
    {
      Dictionary<string, object> disclosureNboReceivedDate = this.LoanData.Calculator.CalculateNew2015DisclosureNBOReceivedDate(this.DisclosureTrackingLog, disclosedDate);
      foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in this.DisclosureTrackingLog.GetAllnboItems())
      {
        string key = allnboItem.Key + "_PresumedReceiveDate";
        if (Utils.ParseDate(disclosureNboReceivedDate[key]) != DateTime.MinValue)
          allnboItem.Value.PresumedReceivedDate = Utils.ParseDate(disclosureNboReceivedDate[key]);
      }
    }

    private void InitSecurityFields()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.CanEditSentDateAndExternalField = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_DT_ChangeSentDate);
      this.HasManualFulfillmentPermission = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_DT_ManualFulfillment);
      this.IsFulfillmentServiceEnabled = Session.ConfigurationManager.GetCompanySetting("Fulfillment", "ServiceEnabled") == "Y";
      this.IsReasonsEnabled = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_DT_ChangeReasons);
    }

    public DateTime GetReceivedDateByAdding3BusinessDays(DateTime disclosedDate)
    {
      return Session.SessionObjects.GetBusinessCalendar(CalendarType.RegZ).AddBusinessDays(disclosedDate, 3, true);
    }

    public void GetReceivedDateForNBOs(DateTime disclosedDate)
    {
      foreach (KeyValuePair<string, INonBorrowerOwnerItem> keyValuePair in this.DisclosureTrackingLog.GetAllnboItems().Where<KeyValuePair<string, INonBorrowerOwnerItem>>((Func<KeyValuePair<string, INonBorrowerOwnerItem>, bool>) (item => !item.Value.isPresumedDateLocked)).Where<KeyValuePair<string, INonBorrowerOwnerItem>>((Func<KeyValuePair<string, INonBorrowerOwnerItem>, bool>) (item => item.Value.eDisclosureNBOLoanLevelConsent == "Accepted")))
        this.DisclosureTrackingLog.SetnboAttributeValue(keyValuePair.Key, (object) this.GetReceivedDateByAdding3BusinessDays(disclosedDate), DisclosureTracking2015Log.NBOUpdatableFields.PresumedReceivedDate);
    }

    public DisclosureTrackingBase.DisclosedMethod GetDisclosedMethod(string method)
    {
      return !string.IsNullOrEmpty(method) ? (!(method == this.DiscloseMethod[0]) ? (!(method == this.DiscloseMethod[1]) ? (!(method == this.DiscloseMethod[3]) ? (!(method == this.DiscloseMethod[2]) ? (!(method == this.DiscloseMethod[4]) ? (!(method == this.DiscloseMethod[5]) ? DisclosureTrackingBase.DisclosedMethod.eDisclosure : DisclosureTrackingBase.DisclosedMethod.Other) : DisclosureTrackingBase.DisclosedMethod.Fax) : DisclosureTrackingBase.DisclosedMethod.Email) : DisclosureTrackingBase.DisclosedMethod.eDisclosure) : DisclosureTrackingBase.DisclosedMethod.InPerson) : DisclosureTrackingBase.DisclosedMethod.ByMail) : DisclosureTrackingBase.DisclosedMethod.None;
    }

    public DateTime GetNextClosestBusinessDay(DateTime forDate)
    {
      return Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Postal, forDate, forDate.AddMonths(1)).GetNextClosestBusinessDay(forDate);
    }

    public void UpdateLePage1IntendToProceed(bool intendChanged)
    {
      if (this.DisclosureTrackingLog.IntentToProceed)
      {
        Dictionary<IDisclosureTracking2015Log, bool> log = new Dictionary<IDisclosureTracking2015Log, bool>()
        {
          {
            this.DisclosureTrackingLog,
            false
          }
        };
        this.LoanData.SetField("3164", intendChanged ? "Y" : "N");
        if (this.DisclosedDate != DateTime.MinValue)
          this.LoanData.SetField("3972", this.DisclosedDate.ToString("d"));
        if (((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).IntentToProceed.Date != DateTime.MinValue)
          this.LoanData.SetField("3197", ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).IntentToProceed.Date.ToString("d"));
        if (((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).IntentToProceed.ReceivedBy.UseUserValue)
        {
          this.LoanData.AddLock("3973");
          this.LoanData.SetField("3973", ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).IntentToProceed.ReceivedBy.UserValue);
        }
        else
        {
          if (this.LoanData.IsLocked("3973"))
            this.LoanData.RemoveLock("3973");
          this.LoanData.SetField("3973", ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).IntentToProceed.ReceivedBy.ComputedValue);
        }
        this.LoanData.SetField("3974", ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).IntentToProceed.ReceivedMethod.ToString());
        this.LoanData.SetField("3975", ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).IntentToProceed.ReceivedMethod == DisclosureTrackingBase.DisclosedMethod.Other ? ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).IntentToProceed.ReceivedMethodOther : "");
        this.LoanData.SetField("3976", ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).IntentToProceed.Comments);
        this.LoanData.Calculator.GetIntentToProceedIDisclosureTracking2015Log(log);
      }
      else
      {
        if (this.DisclosureTrackingLog.IntentToProceed == intendChanged)
          return;
        this.LoanData.SetField("3164", "N");
        this.LoanData.SetField("3972", "//");
        this.LoanData.Calculator.FormCalculation("3164", (string) null, (string) null);
      }
    }

    public void UpdateFulfillment(
      string fulfillmentMethod,
      DateTime actualFulfillmentDate,
      DateTime sentDate,
      DateTime presumedFulfillmentDate,
      DateTime recipientActualReceivedDate)
    {
      if (this.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
      {
        if (this.FulfillmentUpdated)
        {
          this.DisclosureTrackingLog.eDisclosureManuallyFulfilledBy = this.eDisclosureManuallyFulfilledBy;
          this.DisclosureTrackingLog.eDisclosureManualFulfillmentDate = this.eDisclosureManualFulfillmentDate;
          this.DisclosureTrackingLog.eDisclosureManualFulfillmentMethod = this.eDisclosureManualFulfillmentMethod;
          this.DisclosureTrackingLog.eDisclosureManualFulfillmentComment = this.eDisclosureManualFulfillmentComment;
          this.DisclosureTrackingLog.PresumedFulfillmentDate = this.eDisclosurePresumedDate;
        }
        this.DisclosureTrackingLog.ActualFulfillmentDate = actualFulfillmentDate;
        if (this.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.None)
        {
          if (fulfillmentMethod == this.DiscloseMethod[1])
            this.eDisclosureManualFulfillmentMethod = DisclosureTrackingBase.DisclosedMethod.InPerson;
          else if (fulfillmentMethod == this.DiscloseMethod[0])
            this.eDisclosureManualFulfillmentMethod = DisclosureTrackingBase.DisclosedMethod.ByMail;
        }
        Dictionary<string, object> disclosureReceivedDate = this.LoanData.Calculator.CalculateNew2015DisclosureReceivedDate(this.DisclosureTrackingLog, sentDate, presumedFulfillmentDate, actualFulfillmentDate, this.eDisclosureManualFulfillmentMethod);
        this.DisclosureTrackingLog.BorrowerFulfillmentMethodDescription = disclosureReceivedDate["BorrowerFulfillmentMethodDescription"].ToString();
        this.DisclosureTrackingLog.CoBorrowerFulfillmentMethodDescription = disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"].ToString();
        if (recipientActualReceivedDate == DateTime.MinValue || recipientActualReceivedDate != DateTime.MinValue && Utils.ParseDate(disclosureReceivedDate["BorrowerActualReceivedDate"]) != DateTime.MinValue)
          this.DisclosureTrackingLog.BorrowerActualReceivedDate = Utils.ParseDate(disclosureReceivedDate["BorrowerActualReceivedDate"]);
        this.DisclosureTrackingLog.BorrowerDisclosedMethod = (DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"];
        this.DisclosureTrackingLog.CoBorrowerDisclosedMethod = (DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["CoBorrowerDisclosedMethod"];
        this.DisclosureTrackingLog.BorrowerPresumedReceivedDate = Utils.ParseDate(disclosureReceivedDate["BorrowerPresumedReceivedDate"]);
        this.DisclosureTrackingLog.CoBorrowerPresumedReceivedDate = Utils.ParseDate(disclosureReceivedDate["CoBorrowerPresumedReceivedDate"]);
      }
      if (this.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder && this.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose)
        this.LoanData.Calculator.CalculateLastDisclosedCDorLE(this.DisclosureTrackingLog);
      double appliedCureAmount = 0.0;
      Hashtable triggerFields = (Hashtable) null;
      string cureLogComment = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).GetCureLogComment();
      if (this.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder && this.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose && (this.DisclosureTrackingLog.DisclosedForLE && this.DisclosureTrackingLog.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Revised || this.DisclosureTrackingLog.DisclosedForCD) && cureLogComment != string.Empty && RegulationAlerts.GetGoodFaithFeeVarianceViolationAlert(this.LoanData) != null)
      {
        appliedCureAmount = this.LoanData.Calculator.GetRequiredVarianceCureAmount();
        triggerFields = this.LoanData.Calculator.GetGFFVarianceAlertDetails();
      }
      if (this.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || this.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose)
        return;
      this.LoanData.Calculator.UpdateLogs();
      ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).CreateCureLog(appliedCureAmount, cureLogComment, triggerFields, Session.UserID, Session.UserInfo.FullName, false);
    }

    public void UpdateCocFields(bool feeLevelIndicator, bool changedCircumstances)
    {
      if (((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey("XCOCFeeLevelIndicator") && (((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes["XCOCFeeLevelIndicator"] == "N" || ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes["XCOCFeeLevelIndicator"] == ""))
        ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes["XCOCFeeLevelIndicator"] = feeLevelIndicator ? "Y" : "N";
      ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes["XCOCChangedCircumstances"] = changedCircumstances ? "Y" : "N";
    }

    public void UpdateNboRecievedDates(DateTime sentDate)
    {
      if (this.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.InPerson)
        return;
      if ((this.DisclosureTrackingLog.DisclosedForLE || this.DisclosureTrackingLog.DisclosedForCD) && this.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure)
      {
        if (this.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || this.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose)
          return;
        foreach (KeyValuePair<string, INonBorrowerOwnerItem> nboItem in this.NBOItems)
          nboItem.Value.PresumedReceivedDate = this.GetReceivedDateByAdding3BusinessDays(sentDate);
      }
      else
      {
        if (this.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure)
          return;
        Dictionary<string, object> disclosureNboReceivedDate = this.LoanData.Calculator.CalculateNew2015DisclosureNBOReceivedDate(this.DisclosureTrackingLog, sentDate);
        foreach (KeyValuePair<string, INonBorrowerOwnerItem> nboItem in this.NBOItems)
        {
          string key1 = nboItem.Key + "_PresumedReceiveDate";
          string key2 = nboItem.Key + "_ActualReceivedDate";
          if (Utils.ParseDate(disclosureNboReceivedDate[key1]) != DateTime.MinValue)
            nboItem.Value.PresumedReceivedDate = Utils.ParseDate(disclosureNboReceivedDate[key1]);
          if (nboItem.Value.ActualReceivedDate == DateTime.MinValue || nboItem.Value.ActualReceivedDate != DateTime.MinValue && Utils.ParseDate(disclosureNboReceivedDate[key2]) != DateTime.MinValue)
            nboItem.Value.ActualReceivedDate = Utils.ParseDate(disclosureNboReceivedDate[key2]);
        }
      }
    }

    public void UpdateFulfillmentFields(
      string RecipientId,
      string FulfillmentOrderBy,
      string DateFulfillOrder,
      DateTime dpPresumedFulfillmentDate,
      DateTime dpActualFulfillmentDate,
      string txtFulfillmentComments,
      string txtFulfillmentMethod)
    {
      if (this.FulfillmentUpdated)
        return;
      EnhancedDisclosureTracking2015Log disclosureTrackingLog = (EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog;
      IList<EnhancedDisclosureTracking2015Log.FulfillmentFields> fulfillments = disclosureTrackingLog.Fulfillments;
      EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillmentFields1;
      if (fulfillments == null)
      {
        fulfillmentFields1 = (EnhancedDisclosureTracking2015Log.FulfillmentFields) null;
      }
      else
      {
        IEnumerable<EnhancedDisclosureTracking2015Log.FulfillmentFields> source = fulfillments.Where<EnhancedDisclosureTracking2015Log.FulfillmentFields>((Func<EnhancedDisclosureTracking2015Log.FulfillmentFields, bool>) (f => f.Recipients.Any<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>((Func<EnhancedDisclosureTracking2015Log.FulfillmentRecipient, bool>) (fr => fr.Id == RecipientId))));
        fulfillmentFields1 = source != null ? source.FirstOrDefault<EnhancedDisclosureTracking2015Log.FulfillmentFields>() : (EnhancedDisclosureTracking2015Log.FulfillmentFields) null;
      }
      EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillmentFields2 = fulfillmentFields1;
      if (fulfillmentFields2 == null)
      {
        EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillmentFields3;
        if (((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Fulfillments.Count > 0)
        {
          fulfillmentFields3 = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Fulfillments[0];
          fulfillmentFields3.IsManual = true;
        }
        else
        {
          fulfillmentFields3 = new EnhancedDisclosureTracking2015Log.FulfillmentFields((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog)
          {
            Id = new Guid().ToString(),
            IsManual = true
          };
          ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Fulfillments.Add(fulfillmentFields3);
        }
        fulfillmentFields3.OrderedBy = FulfillmentOrderBy;
        DateTime result;
        fulfillmentFields3.ProcessedDate = disclosureTrackingLog.CreateDateTimeWithZone(DateTime.TryParse(DateFulfillOrder, out result) ? result : DateTime.MinValue);
        EnhancedDisclosureTracking2015Log.FulfillmentRecipient fulfillmentRecipient = new EnhancedDisclosureTracking2015Log.FulfillmentRecipient(disclosureTrackingLog)
        {
          Id = RecipientId,
          PresumedDate = disclosureTrackingLog.CreateDateTimeWithZone(dpPresumedFulfillmentDate)
        };
        fulfillmentRecipient.ActualDate = disclosureTrackingLog.CreateDateTimeWithZone(dpActualFulfillmentDate);
        fulfillmentRecipient.Comments = txtFulfillmentComments;
        fulfillmentFields3.Recipients.Add(fulfillmentRecipient);
      }
      else
      {
        fulfillmentFields2.OrderedBy = FulfillmentOrderBy;
        DateTime result;
        fulfillmentFields2.ProcessedDate = disclosureTrackingLog.CreateDateTimeWithZone(DateTime.TryParse(DateFulfillOrder, out result) ? result : DateTime.MinValue);
        EnhancedDisclosureTracking2015Log.FulfillmentRecipient fulfillmentRecipient = fulfillmentFields2.Recipients.FirstOrDefault<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>((Func<EnhancedDisclosureTracking2015Log.FulfillmentRecipient, bool>) (fr => fr.Id == RecipientId));
        fulfillmentRecipient.PresumedDate = disclosureTrackingLog.CreateDateTimeWithZone(dpPresumedFulfillmentDate);
        fulfillmentRecipient.ActualDate = disclosureTrackingLog.CreateDateTimeWithZone(dpActualFulfillmentDate);
        fulfillmentRecipient.Comments = txtFulfillmentComments;
      }
      switch (txtFulfillmentMethod)
      {
        case "U.S.Mail":
          if (fulfillmentFields2 == null)
            break;
          fulfillmentFields2.DisclosedMethod = DisclosureTrackingBase.DisclosedMethod.ByMail;
          break;
        case "In Person":
          if (fulfillmentFields2 == null)
            break;
          fulfillmentFields2.DisclosedMethod = DisclosureTrackingBase.DisclosedMethod.InPerson;
          break;
        default:
          if (fulfillmentFields2 == null)
            break;
          fulfillmentFields2.DisclosedMethod = DisclosureTrackingBase.DisclosedMethod.None;
          break;
      }
    }

    public Dictionary<string, object> CalculateNew2015DisclosureReceivedDate(
      DateTime sentDate,
      DateTime PresumedFulfillmentDate,
      DateTime ActualFulfillmentDate,
      DisclosureTrackingBase.DisclosedMethod method)
    {
      return this.LoanData.Calculator.CalculateNew2015DisclosureReceivedDate(this.DisclosureTrackingLog, sentDate, PresumedFulfillmentDate, ActualFulfillmentDate, method);
    }

    public DateTime? GetRecipientPresumedReceivedDate(bool isLocked, DateTime SentDate)
    {
      if (!this.DisclosureTrackingLog.DisclosedForLE && !this.DisclosureTrackingLog.DisclosedForCD || this.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || this.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose)
        return new DateTime?();
      if (!Utils.IsDate((object) this.LoanData.GetField("3983")))
        return new DateTime?();
      return !isLocked ? new DateTime?(this.GetReceivedDateByAdding3BusinessDays(SentDate)) : new DateTime?();
    }

    public List<Tuple<string, string, string, string, string, string>> GetXcocFields()
    {
      int num = Utils.ParseInt(((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey("XCOCcount") ? (object) ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes["XCOCcount"] : (object) "0");
      if (num <= 0)
        return (List<Tuple<string, string, string, string, string, string>>) null;
      List<Tuple<string, string, string, string, string, string>> xcocFields = new List<Tuple<string, string, string, string, string, string>>();
      for (int index = 1; index <= num; ++index)
      {
        string key1 = "XCOC" + index.ToString("00") + "01";
        string key2 = "XCOC" + index.ToString("00") + "_Description";
        string key3 = "XCOC" + index.ToString("00") + "05";
        string key4 = "XCOC" + index.ToString("00") + "06";
        string key5 = "XCOC" + index.ToString("00") + "_Amount";
        string key6 = "XCOC" + index.ToString("00") + "07";
        string key7 = "XCOC" + index.ToString("00") + "08";
        string key8 = "XCOC" + index.ToString("00") + "03";
        if (((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey(key1) && !(((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes[key1] == ""))
        {
          string attribute1 = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey(key2) ? ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes[key2] : "";
          string attribute2 = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey(key5) ? ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes[key5] : "";
          string attribute3 = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey(key3) ? ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes[key3] : "";
          string attribute4 = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey(key4) ? ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes[key4] : "";
          string str1 = "";
          string str2 = "";
          if (((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey(key6))
          {
            str1 = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes[key6];
            if (string.Compare(str1.ToLowerInvariant(), "other") == 0 && ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes.ContainsKey(key7))
              str1 = str1 + " - " + ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes[key7];
            str2 = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).Attributes[key8];
          }
          xcocFields.Add(Tuple.Create<string, string, string, string, string, string>(attribute1, attribute2, attribute3, attribute4, str1, str2));
        }
      }
      return xcocFields;
    }

    public void UpdateRecipients(
      Dictionary<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient> localRecipients)
    {
      if (localRecipients == null)
        return;
      foreach (KeyValuePair<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient> localRecipient1 in localRecipients)
      {
        KeyValuePair<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient> localRecipient = localRecipient1;
        EnhancedDisclosureTracking2015Log.DisclosureRecipient other = ((EnhancedDisclosureTracking2015Log) this.DisclosureTrackingLog).DisclosureRecipients.First<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (rec => rec.Id == localRecipient.Key));
        if (other != null)
          localRecipient.Value.CopyPropertiesTo(other);
      }
    }
  }
}
