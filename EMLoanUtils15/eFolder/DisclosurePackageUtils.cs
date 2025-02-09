// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.DisclosurePackageUtils
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DataEngine.Util;
using EllieMae.EMLite.LoanUtils.EDelivery;
using EllieMae.EMLite.WebServices.LoanUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.eFolder
{
  public class DisclosurePackageUtils
  {
    private const string className = "DisclosurePackageUtils�";
    private static readonly string sw = Tracing.SwEFolder;

    private static string getLoanDisclosurePackageInfos(
      string clientID,
      string userID,
      string userPassword,
      string loanGuid,
      bool clearNotification)
    {
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml("<DisclosureInfo/>");
      DisclosurePackageUtils.setNodeText(xmlDoc, "FROM/@ClientID", clientID);
      DisclosurePackageUtils.setNodeText(xmlDoc, "FROM/@UserID", userID);
      DisclosurePackageUtils.setNodeText(xmlDoc, "FROM/@UserPassword", userPassword);
      DisclosurePackageUtils.setNodeText(xmlDoc, "FROM/@loanguid", loanGuid);
      if (clearNotification)
        DisclosurePackageUtils.setNodeText(xmlDoc, "FROM/@ClearNotifications", "Y");
      else
        DisclosurePackageUtils.setNodeText(xmlDoc, "FROM/@ClearNotifications", "N");
      return xmlDoc.OuterXml;
    }

    public static List<DisclosurePackage> GetDisclosurePackageDetails(
      LoanDataMgr loanDataMgr,
      bool clearNotification,
      List<DisclosureTracking2015Log> logList,
      System.TimeZoneInfo tz,
      bool isPlatformLoan = false)
    {
      SessionObjects sessionObjects = loanDataMgr.SessionObjects;
      string disclosurePackageInfos = DisclosurePackageUtils.getLoanDisclosurePackageInfos(sessionObjects.CompanyInfo.ClientID, sessionObjects.UserID, sessionObjects.UserPassword, loanDataMgr.LoanData.GUID, clearNotification);
      string ssoToken = (string) null;
      Dictionary<string, bool> dictionary = logList.Where<DisclosureTracking2015Log>((Func<DisclosureTracking2015Log, bool>) (x => !string.IsNullOrWhiteSpace(x.eDisclosurePackageID))).ToDictionary<DisclosureTracking2015Log, string, bool>((Func<DisclosureTracking2015Log, string>) (x => x.eDisclosurePackageID.ToLower()), (Func<DisclosureTracking2015Log, bool>) (x => x.IsWetSigned));
      if (sessionObjects.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
      {
        int result = 5;
        int.TryParse(sessionObjects.ConfigurationManager.GetSsoTokenExpirationTimeForEdm(), out result);
        ssoToken = sessionObjects.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
        {
          "Elli.Edm"
        }, result);
      }
      string empty = string.Empty;
      try
      {
        return !isPlatformLoan ? DisclosurePackageUtils.parsePackagesXml(new ePackage(ssoToken, (string) null, sessionObjects?.StartupInfo?.ServiceUrls?.EPackageServiceUrl).GetLoanDisclosureTrackingDetails(disclosurePackageInfos), dictionary, tz) : DisclosurePackageUtils.parsePackagesJson(new EDeliveryRestClient(loanDataMgr).GetDisclosureTrackingDetails().Result, loanDataMgr.LoanData, logList);
      }
      catch (Exception ex)
      {
        Tracing.Log(DisclosurePackageUtils.sw, TraceLevel.Warning, nameof (DisclosurePackageUtils), "Failed to retrieve Disclosure Package Details:" + ex.Message);
        return new List<DisclosurePackage>();
      }
    }

    public static List<DisclosurePackage> GetDisclosurePackageDetails(
      LoanDataMgr loanDataMgr,
      bool clearNotification,
      List<DisclosureTrackingLog> logList,
      bool isPlatformLoan = false)
    {
      SessionObjects sessionObjects = loanDataMgr.SessionObjects;
      string disclosurePackageInfos = DisclosurePackageUtils.getLoanDisclosurePackageInfos(sessionObjects.CompanyInfo.ClientID, sessionObjects.UserID, sessionObjects.UserPassword, loanDataMgr.LoanData.GUID, clearNotification);
      string ssoToken = (string) null;
      Dictionary<string, bool> dictionary = logList.Where<DisclosureTrackingLog>((Func<DisclosureTrackingLog, bool>) (x => !string.IsNullOrWhiteSpace(x.eDisclosurePackageID))).ToDictionary<DisclosureTrackingLog, string, bool>((Func<DisclosureTrackingLog, string>) (x => x.eDisclosurePackageID.ToLower()), (Func<DisclosureTrackingLog, bool>) (x => x.IsWetSigned));
      if (sessionObjects.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
      {
        int result = 5;
        int.TryParse(sessionObjects.ConfigurationManager.GetSsoTokenExpirationTimeForEdm(), out result);
        ssoToken = sessionObjects.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
        {
          "Elli.Edm"
        }, result);
      }
      try
      {
        return !isPlatformLoan ? DisclosurePackageUtils.parsePackagesXml(new ePackage(ssoToken, (string) null, sessionObjects?.StartupInfo?.ServiceUrls?.EPackageServiceUrl).GetLoanDisclosureTrackingDetails(disclosurePackageInfos), dictionary, (System.TimeZoneInfo) null) : DisclosurePackageUtils.parsePackagesJson(new EDeliveryRestClient(loanDataMgr).GetDisclosureTrackingDetails().Result, loanDataMgr.LoanData, logList);
      }
      catch (Exception ex)
      {
        Tracing.Log(DisclosurePackageUtils.sw, TraceLevel.Warning, nameof (DisclosurePackageUtils), "Failed to retrieve Disclosure Package Details:" + ex.Message);
        return new List<DisclosurePackage>();
      }
    }

    private static List<DisclosurePackage> parsePackagesJson(
      EDeliveryPackageTrackingDetail packageTrackingDetail,
      LoanData loanData,
      List<DisclosureTracking2015Log> logList)
    {
      List<DisclosurePackage> packagesJson = new List<DisclosurePackage>();
      Dictionary<string, Dictionary<string, string>> tracking2015Logs = loanData.GetSnapshotDataForAllDisclosureTracking2015Logs(false);
      foreach (EDeliveryTrackingDetail trackingDetail1 in packageTrackingDetail.trackingDetails)
      {
        EDeliveryTrackingDetail trackingDetail = trackingDetail1;
        DisclosurePackage disclosurePackage = new DisclosurePackage();
        disclosurePackage.SetPackageLevelFields(trackingDetail.packageId, trackingDetail.GetNotificationDate(), trackingDetail.createdInformation.GetCreatedDate(), (string) null, (string) null, (string) null, (string) null);
        if (trackingDetail.fulfillmentDetails != null)
        {
          string pdf = trackingDetail.consentPdf == null ? (string) null : (trackingDetail.consentPdf.pdf == null ? (string) null : trackingDetail.consentPdf.pdf);
          disclosurePackage.SetPackageLevelFields(trackingDetail.packageId, trackingDetail.GetNotificationDate(), trackingDetail.createdInformation.GetCreatedDate(), trackingDetail.fulfillmentDetails.senderName, trackingDetail.fulfillmentDetails.GetProcessedDate(), trackingDetail.fulfillmentDetails.GetScheduleDate(), pdf);
        }
        if (trackingDetail.consentPdf != null)
          disclosurePackage.SetPackageLevelFields((string) null, (string) null, (string) null, (string) null, (string) null, (string) null, trackingDetail.consentPdf.pdf);
        DisclosureTracking2015Log dtlog = logList.Find((Predicate<DisclosureTracking2015Log>) (x => x.eDisclosurePackageID.ToLower() == trackingDetail.packageId.ToLower()));
        if (dtlog != null)
        {
          Dictionary<string, INonBorrowerOwnerItem> allnboItems = dtlog.GetAllnboItems();
          for (int index = 0; index < trackingDetail.userDetails.Count; ++index)
          {
            bool flag = false;
            EDeliveryUserDetail userDetail1 = trackingDetail.userDetails[index];
            List<Event> eventList = new List<Event>();
            foreach (EDeliveryEvent edeliveryEvent in userDetail1.events)
            {
              Event @event = new Event()
              {
                Data = edeliveryEvent.data,
                date = edeliveryEvent.date,
                EventType = edeliveryEvent.eventType,
                IPAddress = edeliveryEvent.ipAddress
              };
              eventList.Add(@event);
            }
            UserDetail userDetail2 = new UserDetail()
            {
              Events = eventList,
              RecipientId = userDetail1.recipientId,
              UserEmail = userDetail1.userEmail,
              UserName = userDetail1.userName,
              UserType = userDetail1.userType
            };
            if (DisclosurePackageUtils.IsUserBorrower(tracking2015Logs, dtlog, userDetail2.UserName))
            {
              flag = true;
              DisclosurePackageUtils.FillBorrowerFields(userDetail2, disclosurePackage);
            }
            if (DisclosurePackageUtils.IsUserCoBorrower(tracking2015Logs, dtlog, userDetail2.UserName))
            {
              flag = true;
              DisclosurePackageUtils.FillCoBorrowerFields(userDetail2, disclosurePackage);
            }
            if (DisclosurePackageUtils.IsUserLoanOfficer(userDetail2, loanData))
            {
              flag = true;
              DisclosurePackageUtils.FillLoanOfficerFields(userDetail2, disclosurePackage);
            }
            if (!flag)
            {
              foreach (KeyValuePair<string, INonBorrowerOwnerItem> keyValuePair in allnboItems)
              {
                string strA = string.Join(" ", ((IEnumerable<string>) new string[4]
                {
                  keyValuePair.Value.FirstName,
                  keyValuePair.Value.MidName,
                  keyValuePair.Value.LastName,
                  keyValuePair.Value.Suffix
                }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
                if (string.Compare(strA, userDetail2.UserName, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(keyValuePair.Value.Email, userDetail2.UserEmail, StringComparison.OrdinalIgnoreCase) == 0)
                {
                  string nboNameEmail = strA + " " + keyValuePair.Value.Email;
                  flag = true;
                  DisclosurePackageUtils.FillNBOFields(nboNameEmail, userDetail2, disclosurePackage);
                  break;
                }
              }
            }
            if (!flag && !Guid.TryParse(userDetail2.RecipientId, out Guid _))
              DisclosurePackageUtils.FillLoanOfficerFields(userDetail2, disclosurePackage);
          }
          packagesJson.Add(disclosurePackage);
        }
      }
      return packagesJson;
    }

    private static bool IsUserBorrower(
      Dictionary<string, Dictionary<string, string>> snapshotValues,
      DisclosureTracking2015Log dtlog,
      string packageBorrowerName)
    {
      string empty = string.Empty;
      string strA = string.Empty;
      if (snapshotValues != null)
      {
        if (snapshotValues.Any<KeyValuePair<string, Dictionary<string, string>>>())
        {
          try
          {
            Dictionary<string, string> snapshotValue = snapshotValues[dtlog.Guid];
            empty = snapshotValue["1868"];
            strA = dtlog.FormatName(snapshotValue.ContainsKey("4000") ? snapshotValue["4000"] : "", snapshotValue.ContainsKey("4001") ? snapshotValue["4001"] : "", snapshotValue.ContainsKey("4002") ? snapshotValue["4002"] : "", snapshotValue.ContainsKey("4003") ? snapshotValue["4003"] : "");
          }
          catch (Exception ex)
          {
          }
        }
      }
      return !string.IsNullOrWhiteSpace(strA) && string.Compare(strA, packageBorrowerName, true) == 0 || !string.IsNullOrWhiteSpace(empty) && string.Compare(empty, packageBorrowerName, true) == 0 || !string.IsNullOrWhiteSpace(dtlog.BorrowerName) && string.Compare(dtlog.BorrowerName, packageBorrowerName, true) == 0;
    }

    private static bool IsUserCoBorrower(
      Dictionary<string, Dictionary<string, string>> snapshotValues,
      DisclosureTracking2015Log dtlog,
      string packageCoBorrowerName)
    {
      string empty = string.Empty;
      string strA = string.Empty;
      if (snapshotValues != null)
      {
        if (snapshotValues.Any<KeyValuePair<string, Dictionary<string, string>>>())
        {
          try
          {
            Dictionary<string, string> snapshotValue = snapshotValues[dtlog.Guid];
            empty = snapshotValue["1873"];
            strA = dtlog.FormatName(snapshotValue.ContainsKey("4004") ? snapshotValue["4004"] : "", snapshotValue.ContainsKey("4005") ? snapshotValue["4005"] : "", snapshotValue.ContainsKey("4006") ? snapshotValue["4006"] : "", snapshotValue.ContainsKey("4007") ? snapshotValue["4007"] : "");
          }
          catch (Exception ex)
          {
          }
        }
      }
      return !string.IsNullOrWhiteSpace(strA) && string.Compare(strA, packageCoBorrowerName, true) == 0 || !string.IsNullOrWhiteSpace(empty) && string.Compare(empty, packageCoBorrowerName, true) == 0 || !string.IsNullOrWhiteSpace(dtlog.CoBorrowerName) && string.Compare(dtlog.CoBorrowerName, packageCoBorrowerName, true) == 0;
    }

    private static List<DisclosurePackage> parsePackagesJson(
      EDeliveryPackageTrackingDetail packageTrackingDetail,
      LoanData loanData,
      List<DisclosureTrackingLog> logList)
    {
      List<DisclosurePackage> packagesJson = new List<DisclosurePackage>();
      foreach (EDeliveryTrackingDetail trackingDetail1 in packageTrackingDetail.trackingDetails)
      {
        EDeliveryTrackingDetail trackingDetail = trackingDetail1;
        DisclosurePackage disclosurePackage = new DisclosurePackage();
        disclosurePackage.SetPackageLevelFields(trackingDetail.packageId, trackingDetail.GetNotificationDate(), trackingDetail.createdInformation.GetCreatedDate(), (string) null, (string) null, (string) null, (string) null);
        if (trackingDetail.fulfillmentDetails != null)
        {
          string pdf = trackingDetail.consentPdf == null ? (string) null : (trackingDetail.consentPdf.pdf == null ? (string) null : trackingDetail.consentPdf.pdf);
          disclosurePackage.SetPackageLevelFields(trackingDetail.packageId, trackingDetail.GetNotificationDate(), trackingDetail.createdInformation.GetCreatedDate(), trackingDetail.fulfillmentDetails.senderName, trackingDetail.fulfillmentDetails.GetProcessedDate(), trackingDetail.fulfillmentDetails.GetScheduleDate(), pdf);
        }
        if (trackingDetail.consentPdf != null)
          disclosurePackage.SetPackageLevelFields((string) null, (string) null, (string) null, (string) null, (string) null, (string) null, trackingDetail.consentPdf.pdf);
        DisclosureTrackingLog disclosureTrackingLog = logList.Find((Predicate<DisclosureTrackingLog>) (x => x.eDisclosurePackageID.ToLower() == trackingDetail.packageId.ToLower()));
        if (disclosureTrackingLog != null)
        {
          for (int index = 0; index < trackingDetail.userDetails.Count; ++index)
          {
            bool flag = false;
            EDeliveryUserDetail userDetail1 = trackingDetail.userDetails[index];
            List<Event> eventList = new List<Event>();
            foreach (EDeliveryEvent edeliveryEvent in userDetail1.events)
            {
              Event @event = new Event()
              {
                Data = edeliveryEvent.data,
                date = edeliveryEvent.date,
                EventType = edeliveryEvent.eventType,
                IPAddress = edeliveryEvent.ipAddress
              };
              eventList.Add(@event);
            }
            UserDetail userDetail2 = new UserDetail()
            {
              Events = eventList,
              RecipientId = userDetail1.recipientId,
              UserEmail = userDetail1.userEmail,
              UserName = userDetail1.userName,
              UserType = userDetail1.userType
            };
            if (string.Compare(disclosureTrackingLog.BorrowerName, userDetail2.UserName, StringComparison.OrdinalIgnoreCase) == 0)
            {
              flag = true;
              DisclosurePackageUtils.FillBorrowerFields(userDetail2, disclosurePackage);
            }
            if (string.Compare(disclosureTrackingLog.CoBorrowerName, userDetail2.UserName, StringComparison.OrdinalIgnoreCase) == 0)
            {
              flag = true;
              DisclosurePackageUtils.FillCoBorrowerFields(userDetail2, disclosurePackage);
            }
            if (DisclosurePackageUtils.IsUserLoanOfficer(userDetail2, loanData))
            {
              flag = true;
              DisclosurePackageUtils.FillLoanOfficerFields(userDetail2, disclosurePackage);
            }
            if (!flag && !Guid.TryParse(userDetail2.RecipientId, out Guid _))
              DisclosurePackageUtils.FillLoanOfficerFields(userDetail2, disclosurePackage);
          }
          packagesJson.Add(disclosurePackage);
        }
      }
      return packagesJson;
    }

    private static bool IsUserLoanOfficer(UserDetail userDetail, LoanData loanData)
    {
      string strA1 = loanData.GetField("317");
      if (string.IsNullOrEmpty(strA1))
        strA1 = loanData.GetSimpleField("1612");
      string strA2 = loanData.GetField("1407");
      if (string.IsNullOrEmpty(strA2))
        strA2 = loanData.GetSimpleField("3968");
      return string.Compare(strA1, userDetail.UserName, true) == 0 && string.Compare(strA2, userDetail.UserEmail, true) == 0;
    }

    private static void FillLoanOfficerFields(
      UserDetail userDetail,
      DisclosurePackage disclosurePackage)
    {
      string viewedDateLO = string.Empty;
      string eSignedDateLO = string.Empty;
      string eSignedIPLO = string.Empty;
      foreach (Event @event in userDetail.Events)
      {
        switch (@event.EventType)
        {
          case "PackageRecipientPackageAccessed":
            viewedDateLO = @event.Date;
            continue;
          case "PackageRecipientSigned":
            eSignedDateLO = @event.Date;
            eSignedIPLO = @event.IPAddress;
            continue;
          default:
            continue;
        }
      }
      disclosurePackage.SetLoanOfficerFields(userDetail.UserName, userDetail.RecipientId, viewedDateLO, eSignedDateLO, eSignedIPLO);
    }

    private static bool IsUserCoBorrower(UserDetail userDetail, LoanData loanData)
    {
      foreach (BorrowerPair borrowerPair in loanData.GetBorrowerPairs())
      {
        string str = string.Join(" ", ((IEnumerable<string>) new string[4]
        {
          loanData.GetSimpleField("4004", borrowerPair),
          loanData.GetSimpleField("4005", borrowerPair),
          loanData.GetSimpleField("4006", borrowerPair),
          loanData.GetSimpleField("4007", borrowerPair)
        }).Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x))));
        string simpleField1 = loanData.GetSimpleField("1268", borrowerPair);
        string simpleField2 = loanData.GetSimpleField("1179", borrowerPair);
        if (string.Compare(str.Trim(), userDetail.UserName, true) == 0 && (string.Compare(simpleField1, userDetail.UserEmail, true) == 0 || string.Compare(simpleField2, userDetail.UserEmail, true) == 0))
          return true;
      }
      return false;
    }

    private static void FillCoBorrowerFields(
      UserDetail userDetail,
      DisclosurePackage disclosurePackage)
    {
      disclosurePackage.SetCoBorrowerFields(userDetail.UserName, userDetail.UserEmail);
      if (userDetail.Events == null)
        return;
      foreach (Event @event in userDetail.Events)
      {
        switch (@event.EventType)
        {
          case "PackageGroupConsented":
            disclosurePackage.SetEDisclosureLoanLevelConsentFields(eDisclosureCoBorrowerLoanLevelConsent: @event.Data);
            continue;
          case "PackageRecipientAuthenticated":
            disclosurePackage.SetCoBorrowerFields(userDetail.UserName, userDetail.UserEmail, @event.Date, @event.IPAddress);
            continue;
          case "PackageRecipientConsented":
            if (@event.Data == "Accepted")
              disclosurePackage.SetCoBorrowerFields(userDetail.UserName, userDetail.UserEmail, consentAccpetedDateCoBorrower: @event.Date, consentIPCoBorrower: @event.IPAddress);
            if (@event.Data == "Declined")
            {
              disclosurePackage.SetCoBorrowerFields(userDetail.UserName, userDetail.UserEmail, consentRejectedDateCoBorrower: @event.Date, consentIPCoBorrower: @event.IPAddress);
              continue;
            }
            continue;
          case "PackageRecipientPackageAccessed":
            disclosurePackage.SetCoBorrowerFields(userDetail.UserName, userDetail.UserEmail, viewedDateCoBorrower: @event.Date);
            disclosurePackage.SetDocumentViewedDateFields(documentViewedDateCoBorrower: @event.Date);
            continue;
          case "PackageRecipientSigned":
            disclosurePackage.SetCoBorrowerFields(userDetail.UserName, userDetail.UserEmail, eSignedDateCoBorrower: @event.Date, eSignedIPCoBorrower: @event.IPAddress);
            continue;
          case "PackageRecipientViewedSigned":
            disclosurePackage.ToggleMakedForeSignatures("1");
            continue;
          case "PackageRecipientViewedWetSigned":
            disclosurePackage.ToggleMakedForeSignatures("");
            continue;
          default:
            continue;
        }
      }
    }

    private static bool IsUserBorrower(UserDetail userDetail, LoanData loanData)
    {
      foreach (BorrowerPair borrowerPair in loanData.GetBorrowerPairs())
      {
        string str = string.Join(" ", ((IEnumerable<string>) new string[4]
        {
          loanData.GetSimpleField("4000", borrowerPair),
          loanData.GetSimpleField("4001", borrowerPair),
          loanData.GetSimpleField("4002", borrowerPair),
          loanData.GetSimpleField("4003", borrowerPair)
        }).Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x))));
        string simpleField1 = loanData.GetSimpleField("1240", borrowerPair);
        string simpleField2 = loanData.GetSimpleField("1178", borrowerPair);
        if (string.Compare(str.Trim(), userDetail.UserName, true) == 0 && (string.Compare(simpleField1, userDetail.UserEmail, true) == 0 || string.Compare(simpleField2, userDetail.UserEmail, true) == 0))
          return true;
      }
      return false;
    }

    private static void FillBorrowerFields(
      UserDetail userDetail,
      DisclosurePackage disclosurePackage)
    {
      disclosurePackage.SetBorrowerFields(userDetail.UserName, userDetail.UserEmail);
      if (userDetail.Events == null)
        return;
      foreach (Event @event in userDetail.Events)
      {
        switch (@event.EventType)
        {
          case "PackageGroupConsented":
            disclosurePackage.SetEDisclosureLoanLevelConsentFields(@event.Data);
            continue;
          case "PackageRecipientAuthenticated":
            disclosurePackage.SetBorrowerFields(userDetail.UserName, userDetail.UserEmail, @event.Date, @event.IPAddress);
            continue;
          case "PackageRecipientConsented":
            if (@event.Data == "Accepted")
              disclosurePackage.SetBorrowerFields(userDetail.UserName, userDetail.UserEmail, consentAccpetedDateBorrower: @event.Date, consentIPBorrower: @event.IPAddress);
            if (@event.Data == "Declined")
            {
              disclosurePackage.SetBorrowerFields(userDetail.UserName, userDetail.UserEmail, consentRejectedDateBorrower: @event.Date, consentIPBorrower: @event.IPAddress);
              continue;
            }
            continue;
          case "PackageRecipientPackageAccessed":
            disclosurePackage.SetBorrowerFields(userDetail.UserName, userDetail.UserEmail, viewedDateBorrower: @event.Date);
            disclosurePackage.SetDocumentViewedDateFields(@event.Date);
            continue;
          case "PackageRecipientSigned":
            disclosurePackage.SetBorrowerFields(userDetail.UserName, userDetail.UserEmail, eSignedDateBorrower: @event.Date, eSignedIPBorrower: @event.IPAddress);
            continue;
          case "PackageRecipientViewedSigned":
            disclosurePackage.ToggleMakedForeSignatures("1");
            continue;
          case "PackageRecipientViewedWetSigned":
            disclosurePackage.ToggleMakedForeSignatures("");
            continue;
          default:
            continue;
        }
      }
    }

    private static void FillNBOFields(
      string nboNameEmail,
      UserDetail userDetail,
      DisclosurePackage disclosurePackage)
    {
      disclosurePackage.SetNBOFields(nboNameEmail, userDetail.UserEmail);
      if (userDetail.Events == null)
        return;
      foreach (Event @event in userDetail.Events)
      {
        switch (@event.EventType)
        {
          case "PackageGroupConsented":
            disclosurePackage.SetNBOEDisclosureLoanLevelConsentFields(nboNameEmail, @event.Data);
            continue;
          case "PackageRecipientAuthenticated":
            disclosurePackage.SetNBOFields(nboNameEmail, userDetail.UserEmail, @event.Date, @event.IPAddress);
            continue;
          case "PackageRecipientConsented":
            if (@event.Data == "Accepted")
              disclosurePackage.SetNBOFields(nboNameEmail, userDetail.UserEmail, consentAcceptedDateNBO: @event.Date, consentIPNBO: @event.IPAddress);
            if (@event.Data == "Declined")
            {
              disclosurePackage.SetNBOFields(nboNameEmail, userDetail.UserEmail, consentRejectedDateNBO: @event.Date, consentIPNBO: @event.IPAddress);
              continue;
            }
            continue;
          case "PackageRecipientPackageAccessed":
            disclosurePackage.SetNBOFields(nboNameEmail, userDetail.UserEmail, viewedDateNBO: @event.Date);
            disclosurePackage.SetNBODocumentViewedDateFields(nboNameEmail, @event.Date);
            continue;
          case "PackageRecipientSigned":
            disclosurePackage.SetNBOFields(nboNameEmail, userDetail.UserEmail, eSignedDateNBO: @event.Date, eSignedIPNBO: @event.IPAddress);
            continue;
          case "PackageRecipientViewedSigned":
            disclosurePackage.ToggleMakedForNBOeSignatures(nboNameEmail, "1");
            continue;
          case "PackageRecipientViewedWetSigned":
            disclosurePackage.ToggleMakedForNBOeSignatures(nboNameEmail, "");
            continue;
          default:
            continue;
        }
      }
    }

    private static List<DisclosurePackage> parsePackagesXml(
      string xmlContent,
      Dictionary<string, bool> guidToWetSignFlag,
      System.TimeZoneInfo timeZoneInfo)
    {
      List<DisclosurePackage> packagesXml = new List<DisclosurePackage>();
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlContent);
      foreach (XmlNode selectNode in xmlDocument.SelectNodes("PackageDetails/package"))
        packagesXml.Add(new DisclosurePackage(selectNode, guidToWetSignFlag, timeZoneInfo));
      return packagesXml;
    }

    private static void setNodeText(XmlDocument xmlDoc, string xpath, string val)
    {
      if (val == null || val == string.Empty)
        return;
      XmlElement xmlElement = xmlDoc.DocumentElement;
      string str1 = xpath;
      char[] chArray = new char[1]{ '/' };
      foreach (string xpath1 in str1.Split(chArray))
      {
        if (xpath1.StartsWith("@"))
        {
          xmlElement.SetAttribute(xpath1.Substring(1), val);
          return;
        }
        if (xpath1 == "!CDATA")
        {
          xmlElement.AppendChild((XmlNode) xmlDoc.CreateCDataSection(val));
          return;
        }
        string name1 = xpath1;
        if (name1.EndsWith("]"))
          name1 = name1.Substring(0, name1.IndexOf("["));
        XmlNode xmlNode = xmlElement.SelectSingleNode(xpath1);
        while (xmlNode == null)
        {
          xmlNode = xmlElement.AppendChild((XmlNode) xmlDoc.CreateElement(name1));
          if (xpath1.IndexOf("[@") > 0)
          {
            string str2 = xpath1.Substring(xpath1.IndexOf("[@") + 2);
            string str3 = str2.Substring(0, str2.LastIndexOf("]"));
            string name2 = str3.Substring(0, str3.IndexOf("=")).Trim();
            string str4 = str3.Substring(str3.IndexOf("\"") + 1);
            string str5 = str4.Substring(0, str4.LastIndexOf("\""));
            ((XmlElement) xmlNode).SetAttribute(name2, str5);
          }
          else
            xmlNode = xmlElement.SelectSingleNode(xpath1);
        }
        xmlElement = (XmlElement) xmlNode;
      }
      xmlElement.InnerText = val;
    }
  }
}
