// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactUtils
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.CustomLetters;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using Microsoft.Win32;
using Outlook;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactUtils
  {
    private static string className = nameof (ContactUtils);
    private static string sw = Tracing.SwContact;
    public static string emailAddressOption_Home = "Home";
    public static string emailAddressOption_Work = "Work";
    public static int[] ContactIDs = (int[]) null;
    public static ContactType TypeOfContacts = ContactType.Borrower;
    public static FileSystemEntry LetterEntry = (FileSystemEntry) null;
    public static bool IsPrintPreview = false;
    public static bool IsEmailMerge = false;
    public static string EmailSubject = string.Empty;
    public static string[] EmailAddressOption = (string[]) null;
    public static string SenderUserID = (string) null;
    private static object ProgressDlgState = (object) null;
    private static IProgressFeedback Feedback = (IProgressFeedback) null;
    private static object progressSyncRoot = new object();
    private static int progressCounter = 0;
    private static bool autoCount = false;

    public static event MailMergeEventHandler MailMerge;

    private static EMPrintSettings ShowPrintDialog()
    {
      PrintDialog printDialog = new PrintDialog();
      printDialog.Document = new PrintDocument();
      printDialog.AllowPrintToFile = false;
      printDialog.AllowSomePages = false;
      printDialog.AllowSelection = false;
      return printDialog.ShowDialog() == DialogResult.Cancel ? (EMPrintSettings) null : new EMPrintSettings(printDialog.PrinterSettings.PrinterName, (int) printDialog.PrinterSettings.Copies, printDialog.PrinterSettings.Collate);
    }

    public static DialogResult DoAsyncMailMerge(object state, IProgressFeedback feedback)
    {
      ContactUtils.ProgressDlgState = state;
      ContactUtils.Feedback = feedback;
      ContactUtils.progressCounter = 0;
      ContactUtils.autoCount = false;
      new Thread(new ThreadStart(ContactUtils.ProgressBarThreadProc)).Start();
      bool flag = ContactUtils.DoMailMerge(ContactUtils.ContactIDs, ContactUtils.TypeOfContacts, ContactUtils.LetterEntry, ContactUtils.IsPrintPreview, ContactUtils.IsEmailMerge, ContactUtils.EmailSubject, ContactUtils.EmailAddressOption, ContactUtils.SenderUserID);
      lock (ContactUtils.progressSyncRoot)
        ContactUtils.progressCounter = ContactUtils.ContactIDs.Length;
      return !flag ? DialogResult.No : DialogResult.OK;
    }

    private static void ProgressBarThreadProc()
    {
      ContactUtils.Feedback.ResetCounter(ContactUtils.ContactIDs.Length);
      string str = !ContactUtils.IsPrintPreview ? "Encompass is sending emails to the contacts you selected. This action might take a while. Please wait..." : "Please wait while the preview is generated.";
      ContactUtils.Feedback.Status = str;
      int num1 = 0;
      while (num1 < ContactUtils.ContactIDs.Length)
      {
        Thread.Sleep(ContactUtils.autoCount ? (num1 == 0 ? 10000 : 7500) : 5000);
        int num2 = 0;
        lock (ContactUtils.progressSyncRoot)
        {
          if (ContactUtils.autoCount)
            ++ContactUtils.progressCounter;
          num2 = ContactUtils.progressCounter;
        }
        if (!ContactUtils.autoCount && num2 > 0)
          ContactUtils.Feedback.Details = "Sent " + (object) num2 + " of " + (object) ContactUtils.ContactIDs.Length + " messages...";
        if (num2 > num1)
        {
          ContactUtils.Feedback.Increment(num2 - num1);
          num1 = num2;
        }
      }
    }

    public static bool SubmitMailMergeJob(
      int[] contactIds,
      ContactType contactType,
      FileSystemEntry letterEntry,
      string emailSubject,
      string[] emailAddressOption,
      string senderUserID)
    {
      if (contactIds.Length == 0)
        return false;
      ArrayList arrayList1 = new ArrayList((ICollection) contactIds);
      int val2 = ContactUtils.parseInt(string.Concat(EnConfigurationSettings.GlobalSettings["MaxMailMergeJobSize", (object) ""]), -1);
      while (arrayList1.Count > 0)
      {
        ArrayList arrayList2 = val2 <= 0 ? arrayList1 : arrayList1.GetRange(0, Math.Min(arrayList1.Count, val2));
        MailMergeJobParameters mergeJobParameters = new MailMergeJobParameters(Session.UserID, Session.Password, Session.RemoteServer);
        mergeJobParameters.ContactIDs = (int[]) arrayList2.ToArray(typeof (int));
        mergeJobParameters.ContactType = contactType;
        mergeJobParameters.TemplatePath = letterEntry.ToString();
        mergeJobParameters.Subject = emailSubject;
        mergeJobParameters.EmailAddressOption = emailAddressOption;
        mergeJobParameters.SenderUserID = senderUserID;
        using (JobService jobService = new JobService())
          jobService.SubmitJob(Session.CompanyInfo.ClientID, Session.UserID, "MailMerge", mergeJobParameters.ContactIDs.Length, mergeJobParameters.ToString());
        arrayList1.RemoveRange(0, arrayList2.Count);
      }
      return true;
    }

    public static bool DoMailMerge(
      int[] contactIds,
      ContactType contactType,
      FileSystemEntry letterEntry,
      bool isPrintPreview,
      bool bEmailMerge,
      string emailSubject,
      string[] emailAddressOption,
      string senderUserID)
    {
      Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Info, "Starting Mail Merge process for " + (object) contactIds.Length + " " + (object) contactType + "s using template " + (object) letterEntry);
      if (contactIds.Length == 0)
        return false;
      bool flag = true;
      string tempFileName = SystemSettings.GetTempFileName(letterEntry.GetEncodedPath());
      string str = ContactUtils.DownloadServerFile(letterEntry, tempFileName, contactType);
      if (str == null)
        throw new ApplicationException("Cannot download " + letterEntry.GetEncodedPath() + " from Encompass server. Check if the file exits.");
      EmailDeliveryMethod emailDeliveryMethod = EmailDeliveryMethod.NotSpecified;
      SMTPMailSettings smtpSettings = (SMTPMailSettings) null;
      if (bEmailMerge)
        emailDeliveryMethod = ContactUtils.GetCurrentMailDeliveryMethod();
      if (emailDeliveryMethod == EmailDeliveryMethod.SMTP)
        smtpSettings = ContactUtils.getSMTPMailSettings();
      WordHandler wordHandler = (WordHandler) null;
      try
      {
        if (bEmailMerge && emailDeliveryMethod == EmailDeliveryMethod.SMTP)
          return ContactUtils.performSmtpMailMerge(contactIds, contactType, str, emailSubject, smtpSettings, emailAddressOption, senderUserID);
        wordHandler = new WordHandler();
        wordHandler.SetWordAppVisibility(false);
        wordHandler.SetWordAppWindowStateMax();
        lock (ContactUtils.progressSyncRoot)
          ContactUtils.autoCount = true;
        wordHandler.MergeContactLetter(str, contactType, contactIds, bEmailMerge, emailSubject, emailAddressOption, senderUserID);
        if (!isPrintPreview && !bEmailMerge)
        {
          EMPrintSettings emPrintSettings = ContactUtils.ShowPrintDialog();
          if (emPrintSettings != null)
          {
            wordHandler.SetPrinter(emPrintSettings.PrinterName);
            wordHandler.PrintDoc(emPrintSettings.NumCopy, emPrintSettings.Collate, true);
          }
          else
            flag = false;
        }
        if (isPrintPreview)
        {
          wordHandler.SetWordAppVisibility(true);
          wordHandler.ActivateDocWindow();
        }
      }
      catch (System.Exception ex)
      {
        Tracing.Log(ContactUtils.sw, TraceLevel.Error, ContactUtils.className, "Error occurred in MailMerge: " + ex.Message);
        Tracing.Log(ContactUtils.sw, TraceLevel.Error, ContactUtils.className, ex.StackTrace);
        if (wordHandler != null && !bEmailMerge)
          wordHandler.ShutDown();
        throw new ApplicationException(ex.Message, ex);
      }
      finally
      {
        if (wordHandler != null && (bEmailMerge || !isPrintPreview))
          wordHandler.ShutDown();
        try
        {
          System.IO.File.Delete(str);
        }
        catch
        {
        }
      }
      return flag;
    }

    public static void SendMail(MailMessage message)
    {
      if (ContactUtils.GetCurrentMailDeliveryMethod() == EmailDeliveryMethod.SMTP)
        ContactUtils.SendSMTPMail(message);
      else
        ContactUtils.SendOutlookMail(message);
    }

    public static void SendSMTPMail(MailMessage msg)
    {
      SMTPMailSettings smtpMailSettings = ContactUtils.getSMTPMailSettings();
      if (smtpMailSettings == null)
        throw new System.Exception("No valid SMTP settings detected");
      SmtpClient smtpClient = new SmtpClient();
      smtpClient.Host = smtpMailSettings.Server;
      smtpClient.Port = smtpMailSettings.Port;
      smtpClient.EnableSsl = smtpMailSettings.UseSSL;
      if (smtpMailSettings.UserName != "")
        smtpClient.Credentials = (ICredentialsByHost) new NetworkCredential(smtpMailSettings.UserName, smtpMailSettings.Password);
      try
      {
        smtpClient.Send(msg);
      }
      catch (System.Exception ex)
      {
        Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Error, "Failed to send message via SMTP: " + (object) ex);
        throw;
      }
    }

    public static void SendOutlookMail(MailMessage msg)
    {
      using (OutlookServices outlookServices = new OutlookServices())
      {
        _MailItem mailItem;
        try
        {
          Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Verbose, "Creating Outlook MailItem object...");
          mailItem = (_MailItem) outlookServices.ApplicationObject.CreateItem(OlItemType.olMailItem);
          Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Verbose, "Setting Outlook message body (type = " + (msg.IsBodyHtml ? "html" : "text") + "):" + Environment.NewLine + msg.Body);
          if (msg.IsBodyHtml)
            mailItem.HTMLBody = msg.Body;
          else
            mailItem.Body = msg.Body;
          Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Verbose, "Setting Outlook message subject:" + msg.Subject);
          mailItem.Subject = msg.Subject;
          string str = "";
          foreach (MailAddress mailAddress in (Collection<MailAddress>) msg.To)
            str = str + mailAddress.Address + ";";
          Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Verbose, "Addressing Outlook mail item to: " + str);
          mailItem.To = str;
        }
        catch (System.Exception ex)
        {
          Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Error, "Failed to create message in Outlook: " + (object) ex);
          throw;
        }
        try
        {
          mailItem.Send();
          Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Verbose, "Outlook mail item sent successfully.");
        }
        catch (System.Exception ex)
        {
          Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Error, "Failed to send message via Outlook: " + (object) ex);
          throw;
        }
      }
    }

    public static string DownloadServerFile(
      FileSystemEntry letterEntry,
      string localFileName,
      ContactType contactType)
    {
      try
      {
        BinaryObject binaryObject = (BinaryObject) null;
        try
        {
          binaryObject = Session.ConfigurationManager.GetCustomLetter((CustomLetterType) contactType, letterEntry);
        }
        catch
        {
        }
        if (binaryObject == null)
        {
          if (!Session.HideUI)
          {
            int num = (int) Utils.Dialog((IWin32Window) null, "Cannot open the custom letter '" + letterEntry.Name + "'", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          return (string) null;
        }
        binaryObject.Write(localFileName);
        return localFileName;
      }
      catch (IOException ex)
      {
        if (!Session.HideUI)
        {
          int num = (int) Utils.Dialog((IWin32Window) null, "Encompass client cannot access the file " + localFileName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Error, "Encompass client cannot access the file " + localFileName);
        return (string) null;
      }
      catch (System.Exception ex)
      {
        if (!Session.HideUI)
        {
          int num = (int) Utils.Dialog((IWin32Window) null, "Custom letter cannot be downloaded.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Error, "Custom letter '" + letterEntry.ToDisplayString() + "' cannot be downloaded.");
        return (string) null;
      }
    }

    public static EmailDeliveryMethod GetCurrentMailDeliveryMethod()
    {
      EmailDeliveryMethod serverSetting;
      EmailDeliveryMethod mailDeliveryMethod1 = serverSetting = (EmailDeliveryMethod) Session.ServerManager.GetServerSetting("Mail.DeliveryMethod");
      if (mailDeliveryMethod1 != EmailDeliveryMethod.NotSpecified)
        return mailDeliveryMethod1;
      try
      {
        EmailDeliveryMethod mailDeliveryMethod2 = (EmailDeliveryMethod) System.Enum.Parse(typeof (EmailDeliveryMethod), Session.GetPrivateProfileString("Mail", "DeliveryMethod"), true);
        if (mailDeliveryMethod2 != EmailDeliveryMethod.NotSpecified)
          return mailDeliveryMethod2;
      }
      catch
      {
      }
      return EmailDeliveryMethod.Outlook;
    }

    private static SMTPMailSettings getSMTPMailSettings()
    {
      SMTPMailSettings smtpMailSettings = new SMTPMailSettings();
      if ((bool) Session.ServerManager.GetServerSetting("Mail.SMTPAllowOverride") & (Session.GetPrivateProfileString("Mail", "SMTPOverride") ?? "").ToLower() == "true")
      {
        smtpMailSettings.Server = Session.GetPrivateProfileString("Mail", "SMTPServer") ?? "";
        smtpMailSettings.Port = ContactUtils.parseInt(Session.GetPrivateProfileString("Mail", "SMTPPort"), 25);
        smtpMailSettings.UserName = Session.GetPrivateProfileString("Mail", "SMTPUserName") ?? "";
        smtpMailSettings.Password = Session.GetPrivateProfileString("Mail", "SMTPPassword") ?? "";
        smtpMailSettings.UseSSL = Session.GetPrivateProfileString("Mail", "SMTPUseSSL") == "True";
      }
      else
      {
        smtpMailSettings.Server = string.Concat(Session.ServerManager.GetServerSetting("Mail.SMTPServer"));
        smtpMailSettings.Port = (int) Session.ServerManager.GetServerSetting("Mail.SMTPPort");
        smtpMailSettings.UseSSL = Session.SessionObjects.GetCompanySettingFromCache("Mail", "SMTPUseSSL") == "True";
        if ((bool) Session.ServerManager.GetServerSetting("Mail.SMTPIndividualLogin"))
        {
          smtpMailSettings.RequireUserName = true;
          smtpMailSettings.UserName = Session.GetPrivateProfileString("Mail", "SMTPUserName") ?? "";
          smtpMailSettings.Password = Session.GetPrivateProfileString("Mail", "SMTPPassword") ?? "";
        }
        else
        {
          smtpMailSettings.UserName = string.Concat(Session.ServerManager.GetServerSetting("Mail.SMTPUserName"));
          smtpMailSettings.Password = string.Concat(Session.ServerManager.GetServerSetting("Mail.SMTPPassword"));
        }
      }
      return smtpMailSettings;
    }

    private static int parseInt(string value, int defaultValue)
    {
      try
      {
        return int.Parse(value);
      }
      catch
      {
        return defaultValue;
      }
    }

    private static bool performSmtpMailMerge(
      int[] contactIds,
      ContactType contactType,
      string sourceFileName,
      string emailSubject,
      SMTPMailSettings smtpSettings,
      string[] emailAddressOption,
      string senderUserID)
    {
      Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Verbose, "Performing SMTP merging for " + (object) contactIds.Length + " contacts");
      if (smtpSettings.Server == "")
      {
        Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Error, "The SMTP server information is not properly configured.");
        if (Session.HideUI)
          throw new System.Exception("The SMTP server information is not properly configured.");
        int num = (int) Utils.Dialog((IWin32Window) null, "The SMTP server information is not properly configured.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (smtpSettings.RequireUserName && smtpSettings.UserName == "")
      {
        Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Error, "The SMTP login information is not properly configured.");
        if (Session.HideUI)
          throw new System.Exception("Your SMTP login information must be provided on the Settings tab before email can be sent.");
        int num = (int) Utils.Dialog((IWin32Window) null, "Your SMTP login information must be provided on the Settings tab before email can be sent.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      WordHandler wordHandler = (WordHandler) null;
      try
      {
        for (int index = 0; index < contactIds.Length; ++index)
        {
          if (index % 250 == 0)
          {
            if (wordHandler != null)
            {
              wordHandler.ShutDown();
              wordHandler = (WordHandler) null;
            }
            wordHandler = new WordHandler();
            wordHandler.SetWordAppVisibility(false);
            wordHandler.SetWordAppWindowStateMax();
          }
          string emailAddressOption1 = "";
          if (emailAddressOption != null && emailAddressOption.Length > index)
            emailAddressOption1 = emailAddressOption[index];
          if (!ContactUtils.performSmtpMailMerge(wordHandler, contactIds[index], contactType, sourceFileName, emailSubject, smtpSettings, emailAddressOption1, senderUserID))
            return false;
          lock (ContactUtils.progressSyncRoot)
            ++ContactUtils.progressCounter;
        }
      }
      finally
      {
        wordHandler?.ShutDown();
      }
      return true;
    }

    private static bool performSmtpMailMerge(
      WordHandler wordHandler,
      int contactId,
      ContactType contactType,
      string sourceFileName,
      string emailSubject,
      SMTPMailSettings smtpSettings,
      string emailAddressOption,
      string senderUserID)
    {
      Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Verbose, "Performing SMTP merging for contact " + (object) contactId);
      string str1 = Path.Combine(SystemSettings.TempFolderRoot, "MailMerge\\" + Guid.NewGuid().ToString("N") + ".doc");
      Directory.CreateDirectory(Path.GetDirectoryName(str1));
      System.IO.File.Copy(sourceFileName, str1, true);
      string directoryName = Path.GetDirectoryName(str1);
      string withoutExtension = Path.GetFileNameWithoutExtension(str1);
      string str2 = Path.Combine(directoryName, withoutExtension + ".htm");
      string path2 = withoutExtension + "_files";
      string path = Path.Combine(directoryName, path2);
      try
      {
        wordHandler.MergeContactLetter(str1, contactType, new int[1]
        {
          contactId
        }, false, "", new string[1]{ emailAddressOption }, senderUserID);
      }
      catch (System.Exception ex)
      {
        Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Warning, "Mail merge failed for contact " + (object) contactId + " (" + ex.Message + ") -- contact will be skipped");
        return true;
      }
      try
      {
        wordHandler.SaveAs(str2, 10);
        wordHandler.CloseDoc();
        string content = (string) null;
        using (BinaryObject binaryObject = new BinaryObject(str2))
          content = binaryObject.ToString(Encoding.Default);
        ArrayList arrayList = new ArrayList();
        if (Directory.Exists(path))
        {
          foreach (string file in Directory.GetFiles(path))
          {
            if (Path.GetFileName(file).ToLower() != "filelist.xml")
            {
              LinkedResource linkedResource = new LinkedResource(file, ContactUtils.getContentTypeFromExtension(Path.GetExtension(file)));
              arrayList.Add((object) linkedResource);
              string str3 = path2 + "/" + Path.GetFileName(file);
              content = content.Replace("\"" + str3 + "\"", "\"cid:" + linkedResource.ContentId + "\"");
            }
          }
        }
        AlternateView alternateViewFromString = AlternateView.CreateAlternateViewFromString(content, Encoding.Default, "text/html");
        foreach (LinkedResource linkedResource in arrayList)
          alternateViewFromString.LinkedResources.Add(linkedResource);
        MailMessage message = new MailMessage();
        message.IsBodyHtml = true;
        message.AlternateViews.Add(alternateViewFromString);
        message.From = new MailAddress(Session.UserInfo.Email, Session.UserInfo.FullName);
        message.Subject = emailSubject;
        IPropertyDictionary propertyDictionary = contactType != ContactType.Borrower ? (IPropertyDictionary) Session.ContactManager.GetBizPartner(contactId) : (IPropertyDictionary) Session.ContactManager.GetBorrower(contactId);
        if (propertyDictionary == null)
        {
          Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Warning, "The contact '" + (object) contactId + "' could not be found and is being skipped.");
          return true;
        }
        try
        {
          if (emailAddressOption == "")
            message.To.Add(new MailAddress(string.Concat(propertyDictionary["DefaultEmail"]), string.Concat(propertyDictionary["FullName"])));
          else if (emailAddressOption == ContactUtils.emailAddressOption_Home)
            message.To.Add(new MailAddress(string.Concat(propertyDictionary["PersonalEmail"]), string.Concat(propertyDictionary["FullName"])));
          else
            message.To.Add(new MailAddress(string.Concat(propertyDictionary["BizEmail"]), string.Concat(propertyDictionary["FullName"])));
        }
        catch (System.Exception ex)
        {
          Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Warning, "Invalid email address '" + propertyDictionary["DefaultEmail"] + "' detected for contact '" + (object) contactId + "' (" + ex.Message + "). Contact will be skipped.");
          return true;
        }
        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Host = smtpSettings.Server;
        smtpClient.Port = smtpSettings.Port;
        smtpClient.EnableSsl = smtpSettings.UseSSL;
        if (smtpSettings.UserName != "")
          smtpClient.Credentials = (ICredentialsByHost) new NetworkCredential(smtpSettings.UserName, smtpSettings.Password);
        smtpClient.Send(message);
        if (ContactUtils.MailMerge != null)
          ContactUtils.MailMerge(new MailMergeEventArgs(contactId, contactType));
        Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Info, "SMTP message sent successfully to '" + propertyDictionary["FullName"] + " <" + propertyDictionary["DefaultEmail"] + ">'");
        return true;
      }
      catch (SmtpException ex)
      {
        if (ex.StatusCode == SmtpStatusCode.GeneralFailure)
        {
          Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Error, "Error sending mail message: " + (object) ex);
          if (Session.HideUI)
          {
            throw;
          }
          else
          {
            int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Mail merge failed due to an error communicating with your SMTP server. Contact your system administrator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
        }
        else
        {
          Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Info, "SMTP error attempting to send message to contact '" + (object) contactId + "': " + ex.Message);
          return true;
        }
      }
      catch (System.Exception ex)
      {
        Tracing.Log(ContactUtils.sw, ContactUtils.className, TraceLevel.Error, "Error sending mail message: " + (object) ex);
        if (Session.HideUI)
        {
          throw;
        }
        else
        {
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Mail merge failed due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      finally
      {
        try
        {
          System.IO.File.Delete(str1);
        }
        catch
        {
        }
        try
        {
          System.IO.File.Delete(str2);
        }
        catch
        {
        }
        try
        {
          Directory.Delete(path, true);
        }
        catch
        {
        }
      }
    }

    private static string getContentTypeFromExtension(string extension)
    {
      if ((extension ?? "") == "")
        return "application/octet-stream";
      if (!extension.StartsWith("."))
        extension = "." + extension;
      using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(extension))
      {
        if (registryKey != null)
        {
          string typeFromExtension = string.Concat(registryKey.GetValue("Content Type"));
          if (typeFromExtension != "")
            return typeFromExtension;
        }
      }
      return "application/octet-stream";
    }

    public static void addCreditOrderingHistory(int contactId, ContactCreditScores[] creditResult)
    {
      ContactHistoryItem contactHistoryItem = new ContactHistoryItem(-1, "Credit Received", DateTime.Now, -1, string.Empty, Session.UserID, string.Empty);
      int historyId = Session.ContactManager.AddHistoryItemForContact(contactId, ContactType.Borrower, contactHistoryItem);
      Session.ContactManager.AddCreditScoresForHistoryItem(contactId, historyId, creditResult);
    }

    public static void addProductAndPricingHistory(int contactId)
    {
      ContactHistoryItem contactHistoryItem = new ContactHistoryItem(-1, "Product & Pricing", DateTime.Now, -1, string.Empty, Session.UserID, string.Empty);
      Session.ContactManager.AddHistoryItemForContact(contactId, ContactType.Borrower, contactHistoryItem);
    }

    public static void addLoanOriginationHistory(BorrowerInfo contact, BorrowerInfo coborrower)
    {
      Opportunity opportunityByBorrowerId = Session.ContactManager.GetOpportunityByBorrowerId(contact.ContactID);
      ContactLoanInfo info = new ContactLoanInfo();
      info.BorrowerID = contact.ContactID;
      if (opportunityByBorrowerId != null)
      {
        info.AppraisedValue = opportunityByBorrowerId.PropertyValue;
        info.LoanAmount = opportunityByBorrowerId.LoanAmount;
        info.InterestRate = opportunityByBorrowerId.MortgageRate;
        info.Term = opportunityByBorrowerId.Term;
        info.Purpose = opportunityByBorrowerId.Purpose;
        info.DownPayment = opportunityByBorrowerId.DownPayment;
        info.Amortization = opportunityByBorrowerId.Amortization;
      }
      else
      {
        info.AppraisedValue = 0M;
        info.LoanAmount = 0M;
        info.InterestRate = 0M;
        info.Term = 0;
        info.Purpose = EllieMae.EMLite.Common.Contact.LoanPurpose.Blank;
        info.DownPayment = 0M;
        info.Amortization = AmortizationType.Blank;
      }
      info.LTV = 0M;
      info.LoanType = LoanTypeEnum.Blank;
      info.LienPosition = LienEnum.Blank;
      info.LoanStatus = "Started";
      info.DateCompleted = DateTime.Now;
      int loanId = Session.ContactManager.AddContactLoan(info);
      ContactHistoryItem contactHistoryItem = new ContactHistoryItem(-1, "Loan Origination", info.DateCompleted, loanId, string.Empty, Session.UserID, string.Empty);
      Session.ContactManager.AddHistoryItemForContact(contact.ContactID, ContactType.Borrower, contactHistoryItem);
      if (coborrower == null)
        return;
      Session.ContactManager.AddHistoryItemForContact(coborrower.ContactID, ContactType.Borrower, contactHistoryItem);
    }

    public static void addBorrowerContactLoanRelationship(
      BorrowerInfo contact,
      string borrowerRandomID,
      BorrowerInfo coborrower,
      string coBorRandomID,
      string loanGuid)
    {
      Session.LoanData.GetLogList().AddCRMMapping(borrowerRandomID, CRMLogType.BorrowerContact, contact.ContactGuid.ToString(), CRMRoleType.Borrower);
      if (coborrower == null)
        return;
      Session.LoanData.GetLogList().AddCRMMapping(coBorRandomID, CRMLogType.BorrowerContact, coborrower.ContactGuid.ToString(), CRMRoleType.Coborrower);
    }

    public static void addEmailHistory(int[] contactIds, ContactType contactType, int noteId)
    {
      foreach (int contactId in contactIds)
      {
        ContactHistoryItem contactHistoryItem = new ContactHistoryItem(-1, "Emailed", DateTime.Now, noteId, string.Empty, Session.UserID, string.Empty);
        Session.ContactManager.AddHistoryItemForContact(contactId, contactType, contactHistoryItem);
      }
    }

    public static void addCallHistory(int[] contactIds, ContactType contactType, int noteId)
    {
      foreach (int contactId in contactIds)
      {
        ContactHistoryItem contactHistoryItem = new ContactHistoryItem(-1, "Called", DateTime.Now, noteId, string.Empty, Session.UserID, string.Empty);
        Session.ContactManager.AddHistoryItemForContact(contactId, contactType, contactHistoryItem);
      }
    }

    public static void addFaxHistory(int[] contactIds, ContactType contactType, int noteId)
    {
      foreach (int contactId in contactIds)
      {
        ContactHistoryItem contactHistoryItem = new ContactHistoryItem(-1, "Faxed", DateTime.Now, noteId, string.Empty, Session.UserID, string.Empty);
        Session.ContactManager.AddHistoryItemForContact(contactId, contactType, contactHistoryItem);
      }
    }

    public static void addMailMergeHistory(
      int[] contactIds,
      ContactType contactType,
      string letterFileName)
    {
      foreach (int contactId in contactIds)
      {
        ContactHistoryItem contactHistoryItem = new ContactHistoryItem(-1, "Mail Merge", DateTime.Now, -1, letterFileName, Session.UserID, string.Empty);
        Session.ContactManager.AddHistoryItemForContact(contactId, contactType, contactHistoryItem);
      }
    }

    public static void addEmailMergeHistory(
      int[] contactIds,
      ContactType contactType,
      string letterFileName,
      string subject)
    {
      foreach (int contactId in contactIds)
      {
        ContactHistoryItem contactHistoryItem = new ContactHistoryItem(-1, "Email Merge", DateTime.Now, -1, letterFileName, Session.UserID, subject);
        Session.ContactManager.AddHistoryItemForContact(contactId, contactType, contactHistoryItem);
      }
    }
  }
}
