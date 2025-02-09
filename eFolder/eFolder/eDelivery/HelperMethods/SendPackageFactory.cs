// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.HelperMethods.SendPackageFactory
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.eDelivery.ePass;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.HelperMethods
{
  public static class SendPackageFactory
  {
    public static SendPackageDialog CreateSendPackageDialog(
      eDeliveryMessageType packageType,
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      string[] pdfList)
    {
      return Session.StartupInfo.OtpSupport ? (SendPackageDialog) new OTPSendPackageDialog(packageType, loanDataMgr, coversheetFile, signList, neededList, pdfList) : new SendPackageDialog(packageType, loanDataMgr, coversheetFile, signList, neededList, pdfList);
    }

    public static SendPackageDialog CreateSendPackageDialog(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      string[] pdfList,
      HtmlEmailTemplateType emailType)
    {
      return Session.StartupInfo.OtpSupport ? (SendPackageDialog) new OTPSendPackageDialog(loanDataMgr, coversheetFile, signList, neededList, pdfList, emailType) : new SendPackageDialog(loanDataMgr, coversheetFile, signList, neededList, pdfList, emailType);
    }

    public static SendPackageDialog CreateSendPackageDialog(eDeliveryMessage msg)
    {
      return Session.StartupInfo.OtpSupport ? (SendPackageDialog) new OTPSendPackageDialog(msg) : new SendPackageDialog(msg);
    }

    public static EmailListDialog CreateEmailListDialog(
      LoanDataMgr loanDataMgr,
      List<ContactDetails> loanContacts,
      bool includeAllPairs = false)
    {
      return Session.StartupInfo.OtpSupport ? (EmailListDialog) new OTPEmailListDailog(loanDataMgr, loanContacts, includeAllPairs) : new EmailListDialog(loanDataMgr.LoanData, loanContacts, includeAllPairs);
    }

    public static FileContacts CreateFileContact(LoanData loanData)
    {
      return Session.StartupInfo.OtpSupport ? (FileContacts) new OTPFileContacts(loanData) : new FileContacts(loanData);
    }

    public static eDeliveryMessage CreateEDeliveryMessage(
      LoanDataMgr loanDataMgr,
      eDeliveryMessageType msgType)
    {
      return Session.StartupInfo.OtpSupport ? (eDeliveryMessage) new OTPEdeliveryMessage(loanDataMgr, msgType) : new eDeliveryMessage(loanDataMgr, msgType);
    }

    public static eDeliveryClient CreateEDeliveryClient(LoanDataMgr loanDataMgr)
    {
      if (!Session.StartupInfo.OtpSupport)
        return new eDeliveryClient();
      if (loanDataMgr.Dirty && !loanDataMgr.Save())
        throw new Exception("Unable to save the loan.");
      return (eDeliveryClient) new OTPEdeliveryClient();
    }
  }
}
