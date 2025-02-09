// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.StatusOnline.OTPStatusOnlineClientCC
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.eFolder.eDelivery;
using EllieMae.EMLite.eFolder.eDelivery.HelperMethods;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.StatusOnline
{
  public class OTPStatusOnlineClientCC(LoanDataMgr loanDataMgr) : StatusOnlineClientCC(loanDataMgr)
  {
    protected override string sendNotifications(StatusOnlineTrigger trigger)
    {
      List<Party> source1 = new List<Party>();
      List<PartyResponse> source2 = new List<PartyResponse>();
      bool flag = false;
      string str1 = string.Empty;
      string empty1 = string.Empty;
      string userid = string.Empty;
      string name = string.Empty;
      string email = string.Empty;
      string str2 = string.Empty;
      string str3 = string.Empty;
      string simpleField = this.loanDataMgr.LoanData.GetSimpleField("3239");
      if (simpleField != string.Empty)
      {
        UserInfo user = Session.OrganizationManager.GetUser(simpleField);
        if (user != (UserInfo) null)
        {
          string str4 = this.loanDataMgr.LoanData.GetSimpleField("1612");
          if (str4 == string.Empty)
            str4 = user.FullName;
          if (string.IsNullOrEmpty(str4))
            str2 = user.FirstName + " " + user.LastName;
          str3 = user.Email;
        }
      }
      if (string.IsNullOrEmpty(str3))
        return "No originator email address.";
      if (!this.loanDataMgr.Save())
        return "Unable to save the loan.";
      EBSServiceClient ebsServiceClient = new EBSServiceClient();
      Task<List<ContactDetails>> loanContacts = ebsServiceClient.GetLoanContacts(this.loanDataMgr.LoanData.GetField("GUID"));
      Task.WaitAll((Task) loanContacts);
      List<ContactDetails> contactDetailsList = loanContacts.Result == null ? new List<ContactDetails>() : loanContacts.Result;
      ContactDetails contactDetails = (ContactDetails) null;
      Party existingPartyContact = (Party) null;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      foreach (BorrowerPair borrowerPair in this.loanDataMgr.LoanData.GetBorrowerPairs())
      {
        this.loanDataMgr.LoanData.SetBorrowerPair(borrowerPair);
        foreach (string emailRecipient in trigger.EmailRecipients)
        {
          contactDetails = (ContactDetails) null;
          string userName1 = string.Empty;
          string field = this.loanDataMgr.LoanData.GetField(emailRecipient);
          if (!string.IsNullOrEmpty(field))
          {
            switch (emailRecipient)
            {
              case "1240":
                string userName2 = this.loanDataMgr.LoanData.GetSimpleField("1868");
                if (string.IsNullOrEmpty(userName2))
                  userName2 = string.Join(" ", ((IEnumerable<string>) new string[4]
                  {
                    this.loanDataMgr.LoanData.GetSimpleField("4000"),
                    this.loanDataMgr.LoanData.GetSimpleField("4001"),
                    this.loanDataMgr.LoanData.GetSimpleField("4002"),
                    this.loanDataMgr.LoanData.GetSimpleField("4003")
                  }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
                Tuple<string, string> phoneNumber1 = this.GetPhoneNumber("borrower");
                if (phoneNumber1 != null)
                {
                  source1.Add(this.CreateDMOSParty(userName2, field, this.loanDataMgr.LoanData.CurrentBorrowerPair.Borrower.Id, phoneNumber1.Item1, phoneNumber1.Item2));
                  continue;
                }
                continue;
              case "1268":
                string userName3 = this.loanDataMgr.LoanData.GetSimpleField("1873");
                if (string.IsNullOrEmpty(userName3))
                  userName3 = string.Join(" ", ((IEnumerable<string>) new string[4]
                  {
                    this.loanDataMgr.LoanData.GetSimpleField("4004"),
                    this.loanDataMgr.LoanData.GetSimpleField("4005"),
                    this.loanDataMgr.LoanData.GetSimpleField("4006"),
                    this.loanDataMgr.LoanData.GetSimpleField("4007")
                  }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
                Tuple<string, string> phoneNumber2 = this.GetPhoneNumber("coborrower");
                if (phoneNumber2 != null)
                {
                  source1.Add(this.CreateDMOSParty(userName3, field, this.loanDataMgr.LoanData.CurrentBorrowerPair.CoBorrower.Id, phoneNumber2.Item1, phoneNumber2.Item2, "coborrower"));
                  continue;
                }
                continue;
              case "VEND.X141":
                if (string.IsNullOrEmpty(userName1))
                  userName1 = this.loanDataMgr.LoanData.GetField("VEND.X139");
                Tuple<string, string> phoneNumber3 = this.GetPhoneNumber("buyer's agent");
                if (phoneNumber3 != null)
                {
                  source1.Add(this.CreateDMOSParty(userName1, field, (string) null, phoneNumber3.Item1, phoneNumber3.Item2, "buyer's agent"));
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
        if (((IEnumerable<string>) trigger.EmailRecipients).Contains<string>("NBO"))
        {
          List<NonBorrowingOwner> byBorrowerPairId = this.loanDataMgr.LoanData.GetNboByBorrowerPairId(this.loanDataMgr.LoanData.CurrentBorrowerPair.Id);
          int num = 0;
          foreach (NonBorrowingOwner nonBorrowingOwner in byBorrowerPairId)
          {
            NonBorrowingOwner nbo = nonBorrowingOwner;
            if (contactDetailsList != null)
              contactDetails = contactDetailsList.Find((Predicate<ContactDetails>) (x => x.borrowerId == nbo.NBOID));
            string str5;
            if (contactDetails == null || string.IsNullOrEmpty(contactDetails.name))
              str5 = string.Join(" ", ((IEnumerable<string>) new string[4]
              {
                nbo.FirstName,
                nbo.MiddleName,
                nbo.LastName,
                nbo.SuffixName
              }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
            else
              str5 = contactDetails.name;
            string userName = str5;
            Tuple<string, string> nboPhoneNumber = this.GetNBOPhoneNumber(nbo);
            if (nboPhoneNumber != null)
            {
              source1.Add(this.CreateDMOSParty(userName, nbo.EmailAddress, nbo.NBOID, nboPhoneNumber.Item1, nboPhoneNumber.Item2, eDeliveryEntityType.NonBorrowingOwner.ToString("g")));
              ++num;
            }
          }
        }
      }
      if (source1.Count<Party>() == 0)
        return "No valid recipients.";
      Task<GetDMOSRecipientURLResponse> recipientUrl = new DMOSServiceClient().GetRecipientURL(new GetDMOSRecipientURLRequest()
      {
        loanGuid = this.loanDataMgr.LoanData.GetField("GUID").Replace("{", string.Empty).Replace("}", string.Empty),
        parties = source1.GroupBy<Party, string>((Func<Party, string>) (o => o.loanEntity.entityId)).Select<IGrouping<string, Party>, Party>((Func<IGrouping<string, Party>, Party>) (x => x.First<Party>())).ToArray<Party>(),
        caller = new Caller()
        {
          name = "Encompass",
          version = VersionInformation.CurrentVersion.DisplayVersion,
          clientId = Session.CompanyInfo.ClientID
        }
      });
      Task.WaitAll((Task) recipientUrl);
      GetDMOSRecipientURLResponse result = recipientUrl.Result;
      this.loanDataMgr.refreshLoanFromServer();
      if (result != null && result.parties != null)
        source2 = ((IEnumerable<PartyResponse>) result.parties).ToList<PartyResponse>();
      StatusOnlineManager.GetTriggerSender(this.loanDataMgr, trigger.EmailFromType, trigger.OwnerID, out userid, out email, out name);
      string str6 = this.getTriggerEmailBody(trigger, userid);
      foreach (BorrowerPair borrowerPair in this.loanDataMgr.LoanData.GetBorrowerPairs())
      {
        this.loanDataMgr.LoanData.SetBorrowerPair(borrowerPair);
        foreach (string emailRecipient in trigger.EmailRecipients)
        {
          existingPartyContact = (Party) null;
          string field = this.loanDataMgr.LoanData.GetField(emailRecipient);
          if (!string.IsNullOrEmpty(field))
          {
            switch (emailRecipient)
            {
              case "1240":
                if (source1 != null)
                {
                  existingPartyContact = source1.Find((Predicate<Party>) (x => x.loanEntity.entityId == this.loanDataMgr.LoanData.CurrentBorrowerPair.Borrower.Id));
                  break;
                }
                break;
              case "1268":
                if (source1 != null)
                {
                  existingPartyContact = source1.Find((Predicate<Party>) (x => x.loanEntity.entityId == this.loanDataMgr.LoanData.CurrentBorrowerPair.CoBorrower.Id));
                  break;
                }
                break;
              case "VEND.X141":
                if (source1 != null)
                {
                  existingPartyContact = source1.Find((Predicate<Party>) (x => x.loanEntity.entityId.ToLower() == "buyeragent"));
                  break;
                }
                break;
            }
            if (existingPartyContact != null)
            {
              HtmlEmailTemplate htmlEmailTemplate = Session.ConfigurationManager.GetHtmlEmailTemplate(trigger.EmailTemplateOwner, trigger.EmailTemplate);
              RecipientInfo recipientInfo = this.CreateRecipientInfo(this.packageGUID, trigger.Guid, email, name, field, htmlEmailTemplate.Subject, str6, true);
              str6 = this.getTriggerEmailBody(trigger, userid);
              string str7 = this.ValidateRecipientInfo(recipientInfo);
              Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Validate Recipient result: " + str7);
              if (str7 == string.Empty)
              {
                if (source2 != null)
                  str6 = HtmlFieldMerge.MergeDynamicConsumerConnectContent(str6, source2.Where<PartyResponse>((Func<PartyResponse, bool>) (x => x.id == existingPartyContact.id)).FirstOrDefault<PartyResponse>()?.url, existingPartyContact.fullName);
                sendNotificationRequest notificationRequest = this.getSendNotificationRequest(recipientInfo.subject, userid, name, email, field, str6);
                do
                {
                  str1 = string.Empty;
                  try
                  {
                    Task.WaitAll(ebsServiceClient.SendNotification(notificationRequest));
                    break;
                  }
                  catch (SoapException ex)
                  {
                    MetricsFactory.IncrementErrorCounter((Exception) ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\OTPStatusOnlineClientCC.cs", nameof (sendNotifications), 322);
                    str1 = ex.Detail.InnerText;
                  }
                  catch (Exception ex)
                  {
                    MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\OTPStatusOnlineClientCC.cs", nameof (sendNotifications), 327);
                    str1 = ex.Message;
                  }
                  if (str1 != string.Empty)
                  {
                    DialogResult dialogResult = Utils.Dialog((IWin32Window) null, "The following error occurred when trying to send the loan status email:\n\n" + str1 + ". Do you wish to retry sending the email?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                    if (dialogResult == DialogResult.No)
                      flag = false;
                    if (dialogResult == DialogResult.Yes)
                      flag = true;
                  }
                }
                while (flag);
                if (str1 == string.Empty)
                  this.createLogEntry(recipientInfo);
              }
            }
          }
        }
        if (((IEnumerable<string>) trigger.EmailRecipients).Contains<string>("NBO"))
        {
          List<NonBorrowingOwner> byBorrowerPairId = this.loanDataMgr.LoanData.GetNboByBorrowerPairId(this.loanDataMgr.LoanData.CurrentBorrowerPair.Id);
          int num = 0;
          foreach (NonBorrowingOwner nonBorrowingOwner in byBorrowerPairId)
          {
            NonBorrowingOwner nbo = nonBorrowingOwner;
            existingPartyContact = (Party) null;
            if (source1 != null)
              existingPartyContact = source1.Find((Predicate<Party>) (x => x.loanEntity.entityId == nbo.NBOID));
            if (existingPartyContact != null)
            {
              RecipientInfo recipientInfo = this.CreateRecipientInfo(this.packageGUID, trigger.Guid, email, name, existingPartyContact.contact.emailAddress, trigger.Name, str6, true);
              str6 = this.getTriggerEmailBody(trigger, userid);
              string str8 = this.ValidateRecipientInfo(recipientInfo);
              Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Validate Recipient result: " + str8);
              if (str8 == string.Empty)
              {
                if (source2 != null)
                  str6 = HtmlFieldMerge.MergeDynamicConsumerConnectContent(str6, source2.Where<PartyResponse>((Func<PartyResponse, bool>) (x => x.id == existingPartyContact.id)).FirstOrDefault<PartyResponse>()?.url, existingPartyContact.fullName);
                sendNotificationRequest notificationRequest = this.getSendNotificationRequest(recipientInfo.subject, userid, name, email, existingPartyContact.contact.emailAddress, str6);
                do
                {
                  str1 = string.Empty;
                  try
                  {
                    Task.WaitAll(ebsServiceClient.SendNotification(notificationRequest));
                    break;
                  }
                  catch (SoapException ex)
                  {
                    MetricsFactory.IncrementErrorCounter((Exception) ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\OTPStatusOnlineClientCC.cs", nameof (sendNotifications), 401);
                    str1 = ex.Detail.InnerText;
                  }
                  catch (Exception ex)
                  {
                    MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\OTPStatusOnlineClientCC.cs", nameof (sendNotifications), 406);
                    str1 = ex.Message;
                  }
                  if (str1 != string.Empty)
                  {
                    DialogResult dialogResult = Utils.Dialog((IWin32Window) null, "The following error occurred when trying to send the loan status email:\n\n" + str1 + ". Do you wish to retry sending the email?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                    if (dialogResult == DialogResult.No)
                      flag = false;
                    if (dialogResult == DialogResult.Yes)
                      flag = true;
                  }
                }
                while (flag);
                if (str1 == string.Empty)
                  this.createLogEntry(recipientInfo);
              }
            }
            ++num;
          }
        }
      }
      return str1;
    }

    private Tuple<string, string> GetPhoneNumber(string role)
    {
      Tuple<string, string> phoneNumber = (Tuple<string, string>) null;
      SortedDictionary<int, string> phoneFields = new SortedDictionary<int, string>();
      switch (role)
      {
        case "borrower":
          phoneFields.Add(1, "1490");
          phoneFields.Add(2, "66");
          phoneFields.Add(3, "FE0117");
          phoneNumber = this.GetValidNumber(phoneFields);
          break;
        case "coborrower":
          phoneFields.Add(1, "1480");
          phoneFields.Add(2, "98");
          phoneFields.Add(3, "FE0217");
          phoneNumber = this.GetValidNumber(phoneFields);
          break;
        case "buyer's agent":
          phoneFields.Add(1, "VEND.X500");
          phoneFields.Add(2, "VEND.X140");
          phoneNumber = this.GetValidNumber(phoneFields);
          break;
      }
      return phoneNumber;
    }

    private Tuple<string, string> GetValidNumber(SortedDictionary<int, string> phoneFields)
    {
      Tuple<string, string> validNumber = (Tuple<string, string>) null;
      foreach (KeyValuePair<int, string> phoneField in phoneFields)
      {
        string simpleField = this.loanDataMgr.LoanData.GetSimpleField(phoneField.Value);
        if (!string.IsNullOrEmpty(simpleField) && this.IsValidPhoneNumber(simpleField))
        {
          validNumber = new Tuple<string, string>(simpleField, phoneField.Key.ToString());
          return validNumber;
        }
      }
      return validNumber;
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
      return phoneNumber.Trim().Where<char>((Func<char, bool>) (x => char.IsDigit(x))).Count<char>() == 10;
    }

    private Tuple<string, string> GetNBOPhoneNumber(NonBorrowingOwner nbo)
    {
      Tuple<string, string> nboPhoneNumber = (Tuple<string, string>) null;
      if (!string.IsNullOrEmpty(nbo.CellPhoneNumber) && this.IsValidPhoneNumber(nbo.CellPhoneNumber))
        nboPhoneNumber = new Tuple<string, string>(nbo.CellPhoneNumber, "1");
      else if (!string.IsNullOrEmpty(nbo.HomePhoneNumber) && this.IsValidPhoneNumber(nbo.HomePhoneNumber))
        nboPhoneNumber = new Tuple<string, string>(nbo.HomePhoneNumber, "2");
      else if (!string.IsNullOrEmpty(nbo.BusinessPhoneNumber) && this.IsValidPhoneNumber(nbo.BusinessPhoneNumber))
        nboPhoneNumber = new Tuple<string, string>(nbo.BusinessPhoneNumber, "3");
      return nboPhoneNumber;
    }

    private Party CreateDMOSParty(
      string userName,
      string userEmail,
      string borrowerId,
      string phoneNumber,
      string phoneType,
      string contactType = "Borrower")
    {
      Party party = new Party()
      {
        fullName = userName,
        id = Guid.NewGuid().ToString(),
        contact = new DMOSContact()
        {
          emailAddress = userEmail,
          phoneNumber = phoneNumber
        }
      };
      party.contact.phoneType = phoneType.ToLower() == "2" ? "Home" : "Mobile";
      DMOSRequestHelper.SetPartyEntityType(party, contactType, borrowerId, (string) null);
      return party;
    }
  }
}
