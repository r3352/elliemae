// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.StatusOnline.LoanStatusCollection
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.StatusOnline
{
  internal class LoanStatusCollection : IXmlSerializable
  {
    private LoanStatusItem[] itemList;
    private string borFirstName;
    private string borLastName;
    private string coborFirstName;
    private string coborLastName;
    private string borSSN;
    private string coborSSN;
    private Address propAddr;
    private string loFirstName;
    private string loLastName;
    private string loCompanyName;
    private Address loAddr;
    private string loPhoneNo;
    private string loFaxNo;
    private string loEmail;
    private string realtorFirstName;
    private string realtorLastName;
    private string realtorCompanyName;
    private Address realtorAddr;
    private string realtorPhoneNo;
    private string realtorFaxNo;
    private string realtorEmail;
    private string loanNumber;
    private Decimal loanAmount;
    private Decimal appraisedValue;
    private Decimal interestRate;
    private EllieMae.EMLite.Common.Contact.LoanPurpose loanPurpose;
    private AmortizationType loanProgram;
    private string autoEmailToList;
    private string autoEmailCCList;
    private string lastEmailToList;
    private string lastEmailCCList;

    public LoanStatusCollection() => this.itemList = new LoanStatusItem[0];

    public LoanStatusCollection(XmlSerializationInfo info)
    {
      this.itemList = (LoanStatusItem[]) info.GetValue("Items", typeof (LoanStatusItem[]));
      if (this.itemList == null)
        this.itemList = new LoanStatusItem[0];
      this.borFirstName = info.GetString(nameof (borFirstName));
      this.borLastName = info.GetString(nameof (borLastName));
      this.coborFirstName = info.GetString(nameof (coborFirstName));
      this.coborLastName = info.GetString(nameof (coborLastName));
      this.borSSN = info.GetString(nameof (borSSN));
      this.coborSSN = info.GetString(nameof (coborSSN));
      this.propAddr = (Address) info.GetValue(nameof (propAddr), typeof (Address));
      this.loFirstName = info.GetString(nameof (loFirstName));
      this.loLastName = info.GetString(nameof (loLastName));
      this.loCompanyName = info.GetString(nameof (loCompanyName));
      this.loAddr = (Address) info.GetValue(nameof (loAddr), typeof (Address));
      this.loPhoneNo = info.GetString(nameof (loPhoneNo));
      this.loFaxNo = info.GetString(nameof (loFaxNo));
      this.loEmail = info.GetString(nameof (loEmail));
      this.realtorFirstName = info.GetString(nameof (realtorFirstName));
      this.realtorLastName = info.GetString(nameof (realtorLastName));
      this.realtorCompanyName = info.GetString(nameof (realtorCompanyName));
      this.realtorAddr = (Address) info.GetValue(nameof (realtorAddr), typeof (Address));
      this.realtorPhoneNo = info.GetString(nameof (realtorPhoneNo));
      this.realtorFaxNo = info.GetString(nameof (realtorFaxNo));
      this.realtorEmail = info.GetString(nameof (realtorEmail));
      this.loanNumber = info.GetString(nameof (loanNumber), "0504EM00001");
      this.loanAmount = (Decimal) info.GetValue(nameof (loanAmount), typeof (Decimal), (object) 80000.00M);
      this.appraisedValue = (Decimal) info.GetValue(nameof (appraisedValue), typeof (Decimal), (object) 223000.00M);
      this.interestRate = (Decimal) info.GetValue(nameof (interestRate), typeof (Decimal), (object) 8.25M);
      this.loanPurpose = (EllieMae.EMLite.Common.Contact.LoanPurpose) info.GetValue(nameof (loanPurpose), typeof (EllieMae.EMLite.Common.Contact.LoanPurpose), (object) EllieMae.EMLite.Common.Contact.LoanPurpose.CashOutRefi);
      this.loanProgram = (AmortizationType) info.GetValue(nameof (loanProgram), typeof (AmortizationType), (object) AmortizationType.ARM);
      this.autoEmailToList = info.GetString(nameof (autoEmailToList), string.Empty);
      this.autoEmailCCList = info.GetString(nameof (autoEmailCCList), string.Empty);
      this.lastEmailToList = info.GetString(nameof (lastEmailToList), string.Empty);
      this.lastEmailCCList = info.GetString(nameof (lastEmailCCList), string.Empty);
    }

    public StatusOnlineSetup MigrateData(string ownerID)
    {
      StatusOnlineSetup statusOnlineSetup = new StatusOnlineSetup();
      statusOnlineSetup.PersonalTriggersType = ApplyPersonalTriggersType.FileStarter;
      this.migrateTriggers(statusOnlineSetup.Triggers, ownerID, (LoanData) null);
      return statusOnlineSetup;
    }

    public StatusOnlineLoanSetup MigrateData(
      string ownerID,
      LoanStatusSettingsForLoan loanSettings,
      LoanData loanData)
    {
      StatusOnlineLoanSetup statusOnlineLoanSetup = new StatusOnlineLoanSetup();
      statusOnlineLoanSetup.AppliedPersonalTriggers = true;
      statusOnlineLoanSetup.ShowPrompt = !loanSettings.NoPrompt;
      this.migrateTriggers(statusOnlineLoanSetup.Triggers, ownerID, loanData);
      return statusOnlineLoanSetup;
    }

    private void migrateTriggers(
      StatusOnlineTriggerCollection collection,
      string ownerID,
      LoanData loanData)
    {
      string[] recipientList = new string[2]
      {
        "1240",
        "1268"
      };
      if (!string.IsNullOrEmpty(this.lastEmailToList))
      {
        List<string> stringList = new List<string>();
        string lastEmailToList = this.lastEmailToList;
        char[] chArray1 = new char[1]{ ';' };
        foreach (string emailAddresses in lastEmailToList.Split(chArray1))
        {
          if (Utils.ValidateEmail(emailAddresses))
            stringList.Add(emailAddresses.Trim());
        }
        if (!string.IsNullOrEmpty(this.lastEmailCCList))
        {
          string lastEmailCcList = this.lastEmailCCList;
          char[] chArray2 = new char[1]{ ';' };
          foreach (string emailAddresses in lastEmailCcList.Split(chArray2))
          {
            if (Utils.ValidateEmail(emailAddresses))
              stringList.Add(emailAddresses.Trim());
          }
        }
        recipientList = stringList.ToArray();
      }
      foreach (LoanStatusItem loanStatusItem in this.itemList)
      {
        StatusOnlineTrigger trigger = loanStatusItem.MigrateData(ownerID, loanData, recipientList);
        collection.Add(trigger);
      }
    }

    public void GetXmlObjectData(XmlSerializationInfo info) => throw new NotSupportedException();
  }
}
