// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.EDelivery.EDeliveryLoanSync
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.EDelivery
{
  public class EDeliveryLoanSync
  {
    private const string className = "EDeliveryLoanSync�";
    private static readonly string sw = Tracing.SwEFolder;
    private BusinessCalendar _calendar;

    public EDeliveryLoanSync(BusinessCalendar calendar) => this._calendar = calendar;

    public static bool SyncLoan(LoanDataMgr loanDataMgr)
    {
      Tracing.Log(EDeliveryLoanSync.sw, TraceLevel.Verbose, nameof (EDeliveryLoanSync), "Entering SyncLoan");
      try
      {
        LoanDataMgr loanDataMgr1 = loanDataMgr;
        if (loanDataMgr.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked && loanDataMgr.LinkedLoan != null)
          loanDataMgr1 = loanDataMgr.LinkedLoan;
        Tracing.Log(EDeliveryLoanSync.sw, TraceLevel.Verbose, nameof (EDeliveryLoanSync), "Creating EDeliveryRestClient");
        EDeliveryRestClient edeliveryRestClient = new EDeliveryRestClient(loanDataMgr1);
        Tracing.Log(EDeliveryLoanSync.sw, TraceLevel.Verbose, nameof (EDeliveryLoanSync), "Calling GetPackageGroup");
        Task<PackageGroup> packageGroup = edeliveryRestClient.GetPackageGroup();
        Tracing.Log(EDeliveryLoanSync.sw, TraceLevel.Verbose, nameof (EDeliveryLoanSync), "Waiting for GetPackageGroup Task");
        Task.WaitAll((Task) packageGroup);
        if (packageGroup.Result != null)
        {
          Tracing.Log(EDeliveryLoanSync.sw, TraceLevel.Verbose, nameof (EDeliveryLoanSync), "Calling ConfigurationManager.GetBusinessCalendar");
          new EDeliveryLoanSync(loanDataMgr.ConfigurationManager.GetBusinessCalendar(CalendarType.RegZ)).SyncLoan(loanDataMgr.LoanData, packageGroup.Result);
        }
        else if (loanDataMgr.LoanData.Calculator != null)
        {
          if (loanDataMgr.LoanData.eConsentType != eConsentTypes.FullexternaleConsent)
          {
            loanDataMgr.LoanData.Calculator.LoadeSignConsentDate();
            return false;
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(EDeliveryLoanSync.sw, TraceLevel.Error, nameof (EDeliveryLoanSync), "Error in GetPackageGroup. Ex: " + (object) ex);
        Tracing.SendDTLogErrorMessageToServer(TraceLevel.Warning, "Disclosure Tracking Error during eDelivery Data Sync to Loan - LoanGuid: " + loanDataMgr.LoanData.GUID + " SessionID:" + loanDataMgr.SessionObjects.SessionID + " UserID:" + loanDataMgr.SessionObjects.UserID + " Error in GetPackageGroup. Ex: " + (object) ex);
        return false;
      }
      return true;
    }

    public void SyncLoan(LoanData loanData, PackageGroup packageGroup)
    {
      if (loanData.eConsentType != eConsentTypes.FullexternaleConsent)
      {
        if (this.packageContainsConsentsFromCC(packageGroup))
        {
          this.populateBorrowerConsentFields(loanData, packageGroup);
          this.populateNBOConsentFields(loanData, packageGroup);
        }
        if (loanData.Calculator != null)
          loanData.Calculator.LoadeSignConsentDate();
      }
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in loanData.GetLogList().GetAllIDisclosureTracking2015Log(false))
      {
        bool flag = false;
        if (disclosureTracking2015Log is DisclosureTracking2015Log)
          flag = this.populateDisclosureTrackingLog((DisclosureTracking2015Log) disclosureTracking2015Log, packageGroup);
        else if (disclosureTracking2015Log is EnhancedDisclosureTracking2015Log)
          flag = this.populateDisclosureTrackingLog((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log, packageGroup);
        if (loanData.Calculator != null & flag)
        {
          loanData.Calculator.CalculateNew2015DisclosureReceivedDate(disclosureTracking2015Log, true);
          loanData.Calculator.CalculateLatestDisclosure2015(disclosureTracking2015Log);
        }
      }
      this.populateECloseFields(loanData, packageGroup);
    }

    private void populateBorrowerConsentFields(LoanData loanData, PackageGroup packageGroup)
    {
      BorrowerPair[] borrowerPairs = loanData.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length && index <= 5; ++index)
      {
        BorrowerPair borrowerPair = borrowerPairs[index];
        string fullName1 = string.Join(" ", ((IEnumerable<string>) new string[4]
        {
          loanData.GetSimpleField("4000", borrowerPair),
          loanData.GetSimpleField("4001", borrowerPair),
          loanData.GetSimpleField("4002", borrowerPair),
          loanData.GetSimpleField("4003", borrowerPair)
        }).Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x))));
        string simpleField = loanData.GetSimpleField("1240", borrowerPair);
        string fullName2 = string.Join(" ", ((IEnumerable<string>) new string[4]
        {
          loanData.GetSimpleField("4004", borrowerPair),
          loanData.GetSimpleField("4005", borrowerPair),
          loanData.GetSimpleField("4006", borrowerPair),
          loanData.GetSimpleField("4007", borrowerPair)
        }).Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x))));
        string email = loanData.GetSimpleField("1268", borrowerPair);
        if (email == string.Empty)
          email = simpleField;
        RecipientConsentWithRecipientID loanLevelConsent1 = this.getLoanLevelConsent(packageGroup, fullName1, simpleField);
        RecipientConsentWithRecipientID loanLevelConsent2 = this.getLoanLevelConsent(packageGroup, fullName2, email);
        string[] fields1 = (string[]) null;
        string[] fields2 = (string[]) null;
        switch (index)
        {
          case 0:
            fields1 = new string[6]
            {
              "3984",
              "3985",
              "3986",
              "3987",
              "4989",
              "4956"
            };
            fields2 = new string[6]
            {
              "3988",
              "3989",
              "3990",
              "3991",
              "4990",
              "4957"
            };
            break;
          case 1:
            fields1 = new string[6]
            {
              "3992",
              "3993",
              "3994",
              "3995",
              "4991",
              "4958"
            };
            fields2 = new string[6]
            {
              "3996",
              "3997",
              "3998",
              "3999",
              "4992",
              "4959"
            };
            break;
          case 2:
            fields1 = new string[6]
            {
              "4023",
              "4024",
              "4025",
              "4026",
              "4993",
              "4960"
            };
            fields2 = new string[6]
            {
              "4027",
              "4028",
              "4029",
              "4030",
              "4994",
              "4961"
            };
            break;
          case 3:
            fields1 = new string[6]
            {
              "4031",
              "4032",
              "4033",
              "4034",
              "4995",
              "4962"
            };
            fields2 = new string[6]
            {
              "4035",
              "4036",
              "4037",
              "4038",
              "4996",
              "4963"
            };
            break;
          case 4:
            fields1 = new string[6]
            {
              "4039",
              "4040",
              "4041",
              "4042",
              "4997",
              "4964"
            };
            fields2 = new string[6]
            {
              "4043",
              "4044",
              "4045",
              "4046",
              "4998",
              "4965"
            };
            break;
          case 5:
            fields1 = new string[6]
            {
              "4047",
              "4048",
              "4049",
              "4050",
              "4999",
              "4966"
            };
            fields2 = new string[6]
            {
              "4051",
              "4052",
              "4053",
              "4054",
              "5000",
              "4967"
            };
            break;
        }
        this.populateConsentFields(loanData, loanLevelConsent1, fields1);
        this.populateConsentFields(loanData, loanLevelConsent2, fields2);
      }
    }

    private void populateNBOConsentFields(LoanData loanData, PackageGroup packageGroup)
    {
      int additionalVestingParties = loanData.GetNumberOfAdditionalVestingParties();
      for (int index = 1; index <= additionalVestingParties; ++index)
      {
        string field = loanData.GetField("TR" + index.ToString("00") + "99");
        int nboLinkedVesting = loanData.GetNBOLinkedVesting(field);
        if (nboLinkedVesting != 0)
        {
          string str = "NBOC" + nboLinkedVesting.ToString("00");
          string fullName = string.Join(" ", ((IEnumerable<string>) new string[4]
          {
            loanData.GetSimpleField(str + "01"),
            loanData.GetSimpleField(str + "02"),
            loanData.GetSimpleField(str + "03"),
            loanData.GetSimpleField(str + "04")
          }).Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x))));
          string simpleField = loanData.GetSimpleField(str + "11");
          RecipientConsentWithRecipientID loanLevelConsent = this.getLoanLevelConsent(packageGroup, fullName, simpleField);
          string[] fields = new string[6]
          {
            str + "17",
            str + "18",
            str + "19",
            str + "20",
            str + "39",
            str + "40"
          };
          this.populateConsentFields(loanData, loanLevelConsent, fields);
        }
      }
    }

    private bool packageContainsConsentsFromCC(PackageGroup packageGroup)
    {
      foreach (PackageGroupRecipient recipient in packageGroup.recipients)
      {
        if (recipient.consents != null && recipient.consents.Count > 0)
          return true;
      }
      return false;
    }

    private RecipientConsentWithRecipientID getLoanLevelConsent(
      PackageGroup packageGroup,
      string fullName,
      string email)
    {
      if (packageGroup.recipients == null)
        return (RecipientConsentWithRecipientID) null;
      List<RecipientConsentWithRecipientID> source = new List<RecipientConsentWithRecipientID>();
      string empty = string.Empty;
      foreach (PackageGroupRecipient recipient in packageGroup.recipients)
      {
        string id = recipient.id;
        if (recipient.consents != null)
        {
          foreach (RecipientConsent consent in recipient.consents)
            source.Add(new RecipientConsentWithRecipientID(id, consent));
        }
      }
      return source.Where<RecipientConsentWithRecipientID>((Func<RecipientConsentWithRecipientID, bool>) (x => string.Equals(x.consent.fullName.Trim(), fullName.Trim(), StringComparison.OrdinalIgnoreCase))).Where<RecipientConsentWithRecipientID>((Func<RecipientConsentWithRecipientID, bool>) (x => string.Equals(x.consent.email?.Trim() ?? "", email.Trim(), StringComparison.OrdinalIgnoreCase))).OrderByDescending<RecipientConsentWithRecipientID, DateTime?>((Func<RecipientConsentWithRecipientID, DateTime?>) (x => x.consent.date)).FirstOrDefault<RecipientConsentWithRecipientID>();
    }

    private DateTime? parseDate(
      DateTime? consentDate,
      System.TimeZoneInfo timeZoneInfo,
      LoanData loanData)
    {
      if (!consentDate.HasValue)
        return new DateTime?(DateTime.MinValue);
      DateTime dateTime = consentDate.Value;
      if (timeZoneInfo != null)
        return DateTimeKind.Utc == dateTime.Kind ? new DateTime?(System.TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo)) : new DateTime?(System.TimeZoneInfo.ConvertTime(dateTime, Utils.GetTimeZoneInfo("PST"), timeZoneInfo));
      string timeZoneCode = loanData.GetField("LE1.XG9") == "" ? loanData.GetField("LE1.X9") : loanData.GetField("LE1.XG9");
      return DateTimeKind.Utc == dateTime.Kind ? new DateTime?(System.TimeZoneInfo.ConvertTimeFromUtc(dateTime, Utils.GetTimeZoneInfo(timeZoneCode))) : new DateTime?(System.TimeZoneInfo.ConvertTime(dateTime, Utils.GetTimeZoneInfo("PST"), Utils.GetTimeZoneInfo(timeZoneCode)));
    }

    private void populateConsentFields(
      LoanData loanData,
      RecipientConsentWithRecipientID consentWithrecipientID,
      string[] fields)
    {
      string[] strArray = new string[6]
      {
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty
      };
      if (consentWithrecipientID != null && consentWithrecipientID.consent != null)
      {
        System.TimeZoneInfo timeZoneInfo = (System.TimeZoneInfo) null;
        foreach (IDisclosureTracking2015Log disclosureTracking2015Log in ((IEnumerable<IDisclosureTracking2015Log>) loanData.GetLogList().GetAllIDisclosureTracking2015Log(false)).ToArray<IDisclosureTracking2015Log>())
        {
          if (disclosureTracking2015Log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure && disclosureTracking2015Log.IsDisclosed)
          {
            timeZoneInfo = disclosureTracking2015Log.TimeZoneInfo;
            break;
          }
        }
        switch (consentWithrecipientID.consent.status)
        {
          case "Accepted":
            strArray[0] = "Accepted";
            break;
          case "Declined":
            strArray[0] = "Rejected";
            break;
          default:
            strArray[0] = "Pending";
            break;
        }
        strArray[1] = string.Format("{0:MM/dd/yyyy}", (object) this.parseDate(consentWithrecipientID.consent.date, timeZoneInfo, loanData));
        strArray[2] = consentWithrecipientID.consent.ipAddress;
        strArray[3] = consentWithrecipientID.consent.source;
        if (string.IsNullOrEmpty(strArray[3]))
          strArray[3] = "Consumer Connect";
        string str = "";
        if (consentWithrecipientID.consent.actingUserOnBehalfOfRecipient != null && !string.IsNullOrEmpty(consentWithrecipientID.consent.actingUserOnBehalfOfRecipient.userIdentity))
        {
          string userIdentity = consentWithrecipientID.consent.actingUserOnBehalfOfRecipient.userIdentity;
          str = userIdentity.Substring(userIdentity.LastIndexOf("\\") + 1);
        }
        strArray[4] = str;
        strArray[5] = consentWithrecipientID.recipientID;
      }
      for (int index = 0; index <= 5; ++index)
      {
        if (!loanData.IsLocked(fields[index]))
          loanData.SetField(fields[index], strArray[index]);
      }
    }

    private DateTime ConvertToLoanTimeZone(DisclosureTracking2015Log log, DateTime dateTime)
    {
      return log.TimeZoneInfo == null ? System.TimeZoneInfo.ConvertTimeFromUtc(dateTime, Utils.GetTimeZoneInfo("PST")) : log.ConvertToLoanTimeZone(dateTime);
    }

    private bool populateDisclosureTrackingLog(
      DisclosureTracking2015Log log,
      PackageGroup packageGroup)
    {
      if (string.IsNullOrWhiteSpace(log.eDisclosurePackageID))
        return false;
      Package package = this.getPackage(packageGroup, log.eDisclosurePackageID, log.Guid);
      if (package == null)
        return false;
      if (package.createdDate.HasValue)
        log.eDisclosurePackageCreatedDate = this.ConvertToLoanTimeZone(log, package.createdDate.Value);
      if (package.fulfillment != null)
      {
        if (package.fulfillment.from?.name != null)
          log.FulfillmentOrderedBy = package.fulfillment.from.name;
        if (package.fulfillment.processedDate.HasValue)
        {
          log.FullfillmentProcessedDate = this.ConvertToLoanTimeZone(log, package.fulfillment.processedDate.Value);
          log.PresumedFulfillmentDate = this._calendar.AddBusinessDays(log.FullfillmentProcessedDate, 3, true);
        }
        if (package.fulfillment.shipping != null && string.Equals(package.fulfillment.shipping.priority, "Overnight", StringComparison.OrdinalIgnoreCase))
          log.eDisclosureAutomatedFulfillmentMethod = DisclosureTrackingBase.DisclosedMethod.OvernightShipping;
        if (package.fulfillment.tracking != null && !string.IsNullOrWhiteSpace(package.fulfillment.tracking.number))
          log.FulfillmentTrackingNumber = package.fulfillment.tracking.number;
      }
      foreach (PackageRecipient recipient in package.recipients)
      {
        RecipientAuthentication recipientAuthentication = this.getPackageRecipientAuthentication(packageGroup, recipient.id);
        RecipientConsent recipientConsent1 = this.getPackageRecipientConsent(recipient, "Accepted");
        RecipientConsent recipientConsent2 = this.getPackageRecipientConsent(recipient, "Declined");
        PackageRecipientTask recipientViewedTask = this.getPackageRecipientViewedTask(recipient);
        PackageRecipientTask packageRecipientTask = this.getPackageRecipientTask(recipient, "ESign");
        string str = (string) null;
        switch (recipient.consentWhenPackageCreated?.status)
        {
          case "Accepted":
            str = "Accepted";
            break;
          case "Declined":
            str = "Rejected";
            break;
        }
        if (string.Equals(recipient.role, "Borrower", StringComparison.OrdinalIgnoreCase))
        {
          log.eDisclosureBorrowerName = recipient.fullName;
          log.eDisclosureBorrowerEmail = recipient.email;
          if (recipientAuthentication != null && recipientAuthentication.date.HasValue)
            log.eDisclosureBorrowerAuthenticatedDate = this.ConvertToLoanTimeZone(log, recipientAuthentication.date.Value);
          if (recipientAuthentication != null && recipientAuthentication.ipAddress != null)
            log.eDisclosureBorrowerAuthenticatedIP = recipientAuthentication.ipAddress;
          if (recipientConsent1 != null && recipientConsent1.date.HasValue)
            log.eDisclosureBorrowerAcceptConsentDate = this.ConvertToLoanTimeZone(log, recipientConsent1.date.Value);
          if (recipientConsent1 != null && recipientConsent1.ipAddress != null)
            log.eDisclosureBorrowerAcceptConsentIP = recipientConsent1.ipAddress;
          if (recipientConsent2 != null && recipientConsent2.date.HasValue)
            log.eDisclosureBorrowerRejectConsentDate = this.ConvertToLoanTimeZone(log, recipientConsent2.date.Value);
          if (recipientConsent2 != null && recipientConsent2.ipAddress != null)
            log.eDisclosureBorrowerRejectConsentIP = recipientConsent2.ipAddress;
          if (recipientViewedTask != null && recipientViewedTask.viewedDate.HasValue)
            log.EDisclosureBorrowerDocumentViewedDate = this.ConvertToLoanTimeZone(log, recipientViewedTask.viewedDate.Value);
          if (recipientViewedTask != null && recipientViewedTask.viewedDate.HasValue)
            log.eDisclosureBorrowerViewMessageDate = this.ConvertToLoanTimeZone(log, recipientViewedTask.viewedDate.Value);
          if (packageRecipientTask != null && packageRecipientTask.completedDate.HasValue)
            log.eDisclosureBorrowereSignedDate = this.ConvertToLoanTimeZone(log, packageRecipientTask.completedDate.Value);
          if (packageRecipientTask != null && packageRecipientTask.completedIpAddress != null)
            log.eDisclosureBorrowereSignedIP = packageRecipientTask.completedIpAddress;
          if (str != null)
            log.EDisclosureBorrowerLoanLevelConsent = str;
        }
        else if (string.Equals(recipient.role, "Coborrower", StringComparison.OrdinalIgnoreCase))
        {
          log.eDisclosureCoBorrowerName = recipient.fullName;
          log.eDisclosureCoBorrowerEmail = recipient.email;
          if (recipientAuthentication != null && recipientAuthentication.date.HasValue)
            log.eDisclosureCoBorrowerAuthenticatedDate = this.ConvertToLoanTimeZone(log, recipientAuthentication.date.Value);
          if (recipientAuthentication != null && recipientAuthentication.ipAddress != null)
            log.eDisclosureCoBorrowerAuthenticatedIP = recipientAuthentication.ipAddress;
          if (recipientConsent1 != null && recipientConsent1.date.HasValue)
            log.eDisclosureCoBorrowerAcceptConsentDate = this.ConvertToLoanTimeZone(log, recipientConsent1.date.Value);
          if (recipientConsent1 != null && recipientConsent1.ipAddress != null)
            log.eDisclosureCoBorrowerAcceptConsentIP = recipientConsent1.ipAddress;
          if (recipientConsent2 != null && recipientConsent2.date.HasValue)
            log.eDisclosureCoBorrowerRejectConsentDate = this.ConvertToLoanTimeZone(log, recipientConsent2.date.Value);
          if (recipientConsent2 != null && recipientConsent2.ipAddress != null)
            log.eDisclosureCoBorrowerRejectConsentIP = recipientConsent2.ipAddress;
          if (recipientViewedTask != null && recipientViewedTask.viewedDate.HasValue)
            log.EDisclosureCoborrowerDocumentViewedDate = this.ConvertToLoanTimeZone(log, recipientViewedTask.viewedDate.Value);
          if (recipientViewedTask != null && recipientViewedTask.viewedDate.HasValue)
            log.eDisclosureCoBorrowerViewMessageDate = this.ConvertToLoanTimeZone(log, recipientViewedTask.viewedDate.Value);
          if (packageRecipientTask != null && packageRecipientTask.completedDate.HasValue)
            log.eDisclosureCoBorrowereSignedDate = this.ConvertToLoanTimeZone(log, packageRecipientTask.completedDate.Value);
          if (packageRecipientTask != null && packageRecipientTask.completedIpAddress != null)
            log.eDisclosureCoBorrowereSignedIP = packageRecipientTask.completedIpAddress;
          if (str != null)
            log.EDisclosureCoBorrowerLoanLevelConsent = str;
        }
        else if (string.Equals(recipient.role, "Originator", StringComparison.OrdinalIgnoreCase))
        {
          log.eDisclosureLOName = recipient.fullName;
          log.eDisclosureLOUserId = recipient.id;
          if (recipientViewedTask != null && recipientViewedTask.viewedDate.HasValue)
            log.eDisclosureLOViewMessageDate = this.ConvertToLoanTimeZone(log, recipientViewedTask.viewedDate.Value);
          if (packageRecipientTask != null && packageRecipientTask.completedDate.HasValue)
            log.eDisclosureLOeSignedDate = this.ConvertToLoanTimeZone(log, packageRecipientTask.completedDate.Value);
          if (packageRecipientTask != null && packageRecipientTask.completedIpAddress != null)
            log.eDisclosureLOeSignedIP = packageRecipientTask.completedIpAddress;
        }
        else if (string.Equals(recipient.role, "NonBorrowingOwner", StringComparison.OrdinalIgnoreCase))
        {
          foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in log.GetAllnboItems())
          {
            string b = string.Join(" ", ((IEnumerable<string>) new string[4]
            {
              allnboItem.Value.FirstName,
              allnboItem.Value.MidName,
              allnboItem.Value.LastName,
              allnboItem.Value.Suffix
            }).Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x))));
            string email = allnboItem.Value.Email;
            if (string.Equals(recipient.fullName, b, StringComparison.OrdinalIgnoreCase) && string.Equals(recipient.email, email, StringComparison.OrdinalIgnoreCase))
            {
              if (recipientAuthentication != null && recipientAuthentication.date.HasValue)
                log.SetnboAttributeValue(allnboItem.Key, (object) this.ConvertToLoanTimeZone(log, recipientAuthentication.date.Value), DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAuthenticatedDate);
              if (recipientAuthentication != null && recipientAuthentication.ipAddress != null)
                log.SetnboAttributeValue(allnboItem.Key, (object) recipientAuthentication.ipAddress, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAuthenticatedIP);
              if (recipientConsent1 != null && recipientConsent1.date.HasValue)
                log.SetnboAttributeValue(allnboItem.Key, (object) this.ConvertToLoanTimeZone(log, recipientConsent1.date.Value), DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentDate);
              if (recipientConsent1 != null && recipientConsent1.ipAddress != null)
                log.SetnboAttributeValue(allnboItem.Key, (object) recipientConsent1.ipAddress, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentIP);
              if (recipientConsent2 != null && recipientConsent2.date.HasValue)
                log.SetnboAttributeValue(allnboItem.Key, (object) this.ConvertToLoanTimeZone(log, recipientConsent2.date.Value), DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBORejectConsentDate);
              if (recipientConsent2 != null && recipientConsent2.ipAddress != null)
                log.SetnboAttributeValue(allnboItem.Key, (object) recipientConsent2.ipAddress, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBORejectConsentIP);
              if (recipientViewedTask != null && recipientViewedTask.viewedDate.HasValue)
                log.SetnboAttributeValue(allnboItem.Key, (object) this.ConvertToLoanTimeZone(log, recipientViewedTask.viewedDate.Value), DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBODocumentViewedDate);
              if (recipientViewedTask != null && recipientViewedTask.viewedDate.HasValue)
                log.SetnboAttributeValue(allnboItem.Key, (object) this.ConvertToLoanTimeZone(log, recipientViewedTask.viewedDate.Value), DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOViewMessageDate);
              if (packageRecipientTask != null && packageRecipientTask.completedDate.HasValue)
                log.SetnboAttributeValue(allnboItem.Key, (object) this.ConvertToLoanTimeZone(log, packageRecipientTask.completedDate.Value), DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOSignedDate);
              if (packageRecipientTask != null && packageRecipientTask.completedIpAddress != null)
                log.SetnboAttributeValue(allnboItem.Key, (object) packageRecipientTask.completedIpAddress, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOeSignedIP);
              if (str != null)
                log.SetnboAttributeValue(allnboItem.Key, (object) str, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOLoanLevelConsent);
              log.SetnboAttributeValue(allnboItem.Key, packageRecipientTask != null ? (object) "1" : (object) "0", DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOeSignatures);
              break;
            }
          }
        }
      }
      return true;
    }

    private bool populateDisclosureTrackingLog(
      EnhancedDisclosureTracking2015Log log,
      PackageGroup packageGroup)
    {
      Package package = this.getPackage(packageGroup, log.Tracking.PackageId, log.Guid);
      if (package == null)
        return false;
      log.Tracking.PackageId = package.id;
      if (package.createdDate.HasValue)
        log.Tracking.PackageCreatedDate = log.ConvertToDateTimeWithZone(package.createdDate.Value);
      if (log.Status == DisclosureTracking2015Log.TrackingLogStatus.Pending)
      {
        log.Status = DisclosureTracking2015Log.TrackingLogStatus.Active;
        log.IncludedInTimeline = true;
      }
      if (package.fulfillment != null && log.Fulfillments.Count == 1)
      {
        if (package.fulfillment.from?.name != null)
          log.Fulfillments[0].OrderedBy = package.fulfillment.from.name;
        if (package.fulfillment.processedDate.HasValue)
        {
          log.Fulfillments[0].ProcessedDate = log.ConvertToDateTimeWithZone(package.fulfillment.processedDate.Value);
          DateTime srcDate = this._calendar.AddBusinessDays(log.Fulfillments[0].ProcessedDate.DateTime, 3, true);
          foreach (EnhancedDisclosureTracking2015Log.FulfillmentRecipient recipient in (IEnumerable<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>) log.Fulfillments[0].Recipients)
            recipient.PresumedDate = log.CreateDateTimeWithZone(srcDate);
        }
        if (package.fulfillment.shipping != null && string.Equals(package.fulfillment.shipping.priority, "Overnight", StringComparison.OrdinalIgnoreCase))
          log.Fulfillments[0].DisclosedMethod = DisclosureTrackingBase.DisclosedMethod.OvernightShipping;
        if (package.fulfillment.tracking != null && !string.IsNullOrWhiteSpace(package.fulfillment.tracking.number))
          log.Fulfillments[0].TrackingNumber = package.fulfillment.tracking.number;
      }
      foreach (PackageRecipient recipient in package.recipients)
      {
        PackageRecipient packageRecipient = recipient;
        EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient = log.DisclosureRecipients.FirstOrDefault<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (x => string.Equals(x.Id, packageRecipient.id, StringComparison.OrdinalIgnoreCase)));
        if (disclosureRecipient != null)
        {
          RecipientAuthentication recipientAuthentication = this.getPackageRecipientAuthentication(packageGroup, packageRecipient.id);
          RecipientConsent recipientConsent1 = this.getPackageRecipientConsent(packageRecipient, "Accepted");
          RecipientConsent recipientConsent2 = this.getPackageRecipientConsent(packageRecipient, "Declined");
          PackageRecipientTask recipientViewedTask = this.getPackageRecipientViewedTask(packageRecipient);
          PackageRecipientTask packageRecipientTask1 = this.getPackageRecipientTask(packageRecipient, "Review");
          PackageRecipientTask packageRecipientTask2 = this.getPackageRecipientTask(packageRecipient, "ESign");
          PackageRecipientTask packageRecipientTask3 = this.getPackageRecipientTask(packageRecipient, "WetSign");
          if (recipientAuthentication != null && recipientAuthentication.date.HasValue)
            disclosureRecipient.Tracking.AuthenticatedDate = log.ConvertToDateTimeWithZone(recipientAuthentication.date.Value);
          if (recipientAuthentication != null && recipientAuthentication.ipAddress != null)
            disclosureRecipient.Tracking.AuthenticatedIP = recipientAuthentication.ipAddress;
          if (recipientConsent1 != null && recipientConsent1.date.HasValue)
            disclosureRecipient.Tracking.AcceptConsentDate = log.ConvertToDateTimeWithZone(recipientConsent1.date.Value);
          if (recipientConsent1 != null && recipientConsent1.ipAddress != null)
            disclosureRecipient.Tracking.AcceptConsentIP = recipientConsent1.ipAddress;
          if (recipientConsent2 != null && recipientConsent2.date.HasValue)
            disclosureRecipient.Tracking.RejectConsentDate = log.ConvertToDateTimeWithZone(recipientConsent2.date.Value);
          if (recipientConsent2 != null && recipientConsent2.ipAddress != null)
            disclosureRecipient.Tracking.RejectConsentIP = recipientConsent2.ipAddress;
          if (recipientViewedTask != null && recipientViewedTask.viewedDate.HasValue)
            disclosureRecipient.Tracking.ViewMessageDate = log.ConvertToDateTimeWithZone(recipientViewedTask.viewedDate.Value);
          if (packageRecipientTask2 != null && packageRecipientTask2.viewedDate.HasValue)
            disclosureRecipient.Tracking.ViewESignedDate = log.ConvertToDateTimeWithZone(packageRecipientTask2.viewedDate.Value);
          if (packageRecipientTask3 != null && packageRecipientTask3.viewedDate.HasValue)
            disclosureRecipient.Tracking.ViewWetSignedDate = log.ConvertToDateTimeWithZone(packageRecipientTask3.viewedDate.Value);
          if (packageRecipientTask2 != null && packageRecipientTask2.completedDate.HasValue)
            disclosureRecipient.Tracking.ESignedDate = log.ConvertToDateTimeWithZone(packageRecipientTask2.completedDate.Value);
          if (packageRecipientTask2 != null && packageRecipientTask2.completedIpAddress != null)
            disclosureRecipient.Tracking.ESignedIP = packageRecipientTask2.completedIpAddress;
          if (packageRecipientTask1 != null && packageRecipientTask1.viewedDate.HasValue)
            disclosureRecipient.Tracking.InformationalViewedDate = log.ConvertToDateTimeWithZone(packageRecipientTask1.viewedDate.Value);
          if (packageRecipientTask1 != null && packageRecipientTask1.viewedIpAddress != null)
            disclosureRecipient.Tracking.InformationalViewedIP = packageRecipientTask1.viewedIpAddress;
          if (packageRecipientTask1 != null && packageRecipientTask1.completedDate.HasValue)
            disclosureRecipient.Tracking.InformationalCompletedDate = log.ConvertToDateTimeWithZone(packageRecipientTask1.completedDate.Value);
          if (packageRecipientTask1 != null && packageRecipientTask1.completedIpAddress != null)
            disclosureRecipient.Tracking.InformationalCompletedIP = packageRecipientTask1.completedIpAddress;
          switch (packageRecipient.consentWhenPackageCreated?.status)
          {
            case "Accepted":
              disclosureRecipient.Tracking.LoanLevelConsent = "Accepted";
              continue;
            case "Declined":
              disclosureRecipient.Tracking.LoanLevelConsent = "Rejected";
              continue;
            default:
              continue;
          }
        }
      }
      return true;
    }

    private Package getPackage(PackageGroup packageGroup, string packageId, string logId)
    {
      if (packageGroup.packages != null)
      {
        foreach (Package package in packageGroup.packages)
        {
          if (string.Equals(package.id, packageId, StringComparison.OrdinalIgnoreCase) || package.custom?.SynchronizationId != null && string.Equals(package.custom.SynchronizationId, logId, StringComparison.OrdinalIgnoreCase))
            return package;
        }
      }
      return (Package) null;
    }

    private RecipientAuthentication getPackageRecipientAuthentication(
      PackageGroup packageGroup,
      string recipientId)
    {
      PackageGroupRecipient packageGroupRecipient = packageGroup.recipients.FirstOrDefault<PackageGroupRecipient>((Func<PackageGroupRecipient, bool>) (x => string.Equals(x.id, recipientId, StringComparison.OrdinalIgnoreCase)));
      return packageGroupRecipient != null && packageGroupRecipient.authentications != null ? packageGroupRecipient.authentications.OrderBy<RecipientAuthentication, DateTime?>((Func<RecipientAuthentication, DateTime?>) (x => x.date)).FirstOrDefault<RecipientAuthentication>() : (RecipientAuthentication) null;
    }

    private RecipientConsent getPackageRecipientConsent(PackageRecipient recipient, string status)
    {
      return recipient.consents != null ? recipient.consents.Where<RecipientConsent>((Func<RecipientConsent, bool>) (x => string.Equals(x.status, status, StringComparison.OrdinalIgnoreCase))).OrderByDescending<RecipientConsent, DateTime?>((Func<RecipientConsent, DateTime?>) (x => x.date)).FirstOrDefault<RecipientConsent>() : (RecipientConsent) null;
    }

    private PackageRecipientTask getPackageRecipientViewedTask(PackageRecipient recipient)
    {
      return recipient.taskStatuses != null ? recipient.taskStatuses.Where<PackageRecipientTask>((Func<PackageRecipientTask, bool>) (x => !string.Equals(x.taskType, "Upload", StringComparison.OrdinalIgnoreCase) && x.viewedDate.HasValue)).OrderBy<PackageRecipientTask, DateTime?>((Func<PackageRecipientTask, DateTime?>) (x => x.viewedDate)).FirstOrDefault<PackageRecipientTask>() : (PackageRecipientTask) null;
    }

    private PackageRecipientTask getPackageRecipientTask(
      PackageRecipient recipient,
      string taskType)
    {
      return recipient.taskStatuses != null ? recipient.taskStatuses.FirstOrDefault<PackageRecipientTask>((Func<PackageRecipientTask, bool>) (x => string.Equals(x.taskType, taskType, StringComparison.OrdinalIgnoreCase))) : (PackageRecipientTask) null;
    }

    private void populateECloseFields(LoanData loanData, PackageGroup packageGroup)
    {
      Package package1 = (Package) null;
      if (packageGroup.packages != null)
      {
        foreach (Package package2 in packageGroup.packages)
        {
          if (string.Equals(package2.custom?.package?.type, "eClosing", StringComparison.OrdinalIgnoreCase) && (!package2.cancelled.HasValue || !package2.cancelled.Value) && (!package2.replaced.HasValue || !package2.replaced.Value) && (!package2.voided.HasValue || !package2.voided.Value))
            package1 = package2;
        }
      }
      string str = string.Empty;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      if (package1 != null)
      {
        DateTime? nullable = package1.createdDate;
        foreach (PackageRecipient recipient in package1.recipients)
        {
          if (recipient.taskStatuses != null)
          {
            IEnumerable<PackageRecipientTask> source = recipient.taskStatuses.Where<PackageRecipientTask>((Func<PackageRecipientTask, bool>) (x => string.Equals(x.taskType, "ESign", StringComparison.OrdinalIgnoreCase)));
            if (source.Count<PackageRecipientTask>() > 0)
            {
              bool flag = source.All<PackageRecipientTask>((Func<PackageRecipientTask, bool>) (x => x.completedDate.HasValue));
              str = ((string.IsNullOrEmpty(str) ? 1 : (string.Equals(str, "Y", StringComparison.OrdinalIgnoreCase) ? 1 : 0)) & (flag ? 1 : 0)) == 0 ? "N" : "Y";
            }
          }
          PackageRecipientRelease release = recipient.release;
          if ((release != null ? (release.scheduledDate.HasValue ? 1 : 0) : 0) != 0)
          {
            if (nullable.HasValue)
            {
              DateTime dateTime = nullable.Value;
              DateTime? scheduledDate = recipient.release.scheduledDate;
              if ((scheduledDate.HasValue ? (dateTime < scheduledDate.GetValueOrDefault() ? 1 : 0) : 0) == 0)
                continue;
            }
            nullable = recipient.release.scheduledDate;
          }
        }
        DateTime dateTime1;
        if (package1.createdDate.HasValue)
        {
          dateTime1 = package1.createdDate.Value;
          empty1 = dateTime1.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
        if (nullable.HasValue)
        {
          dateTime1 = nullable.Value;
          empty2 = dateTime1.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
        if (package1.expirationDates != null && package1.expirationDates.eSign.HasValue)
        {
          dateTime1 = package1.expirationDates.eSign.Value;
          empty3 = dateTime1.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
      }
      loanData.SetField("4797", str);
      loanData.SetField("4798", empty1);
      loanData.SetField("4799", empty2);
      loanData.SetField("4800", empty3);
    }

    public static bool Withdraw_eConsents(
      LoanDataMgr loanDataMgr,
      Dictionary<string, string[]> eConsents)
    {
      bool flag = true;
      if (eConsents != null)
      {
        if (eConsents.Count != 0)
        {
          try
          {
            LoanDataMgr loanDataMgr1 = loanDataMgr;
            if (loanDataMgr.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked && loanDataMgr.LinkedLoan != null)
              loanDataMgr1 = loanDataMgr.LinkedLoan;
            EDeliveryRestClient edeliveryRestClient = new EDeliveryRestClient(loanDataMgr1);
            List<Task> taskList = new List<Task>();
            foreach (KeyValuePair<string, string[]> eConsent in eConsents)
            {
              Task<string> task = edeliveryRestClient.AddGroupLevelConsent(eConsent.Value[0], eConsent.Value[1], eConsent.Value[2]);
              taskList.Add((Task) task);
            }
            Task.WaitAll(taskList.ToArray());
          }
          catch (Exception ex)
          {
            Tracing.Log(EDeliveryLoanSync.sw, TraceLevel.Error, nameof (EDeliveryLoanSync), "Error in Withdraw_eConsents. Exception: " + (object) ex);
            flag = false;
          }
          return flag;
        }
      }
      return flag;
    }
  }
}
