// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.ePass.OTPEmailListDailog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.ePass
{
  public class OTPEmailListDailog : EmailListDialog
  {
    private LoanDataMgr loanDataMgr;

    public OTPEmailListDailog(
      LoanDataMgr loanDataMgr,
      List<ContactDetails> loanContacts,
      bool includeAllPairs = false)
      : base(loanDataMgr.LoanData, loanContacts, includeAllPairs)
    {
      this.loanDataMgr = loanDataMgr;
    }

    protected override ListViewItem AddListViewItem(
      Dictionary<string, string> contactDetails,
      string ContactType)
    {
      ListViewItem listViewItem = base.AddListViewItem(contactDetails, ContactType);
      listViewItem.SubItems.Add(contactDetails["CellPhoneNumber"]);
      listViewItem.SubItems.Add(contactDetails["HomePhoneNumber"]);
      listViewItem.SubItems.Add(contactDetails["WorkPhoneNumber"]);
      return listViewItem;
    }

    protected override void AddContactDetails()
    {
      using (OTPAddFileContactDialog dialog = new OTPAddFileContactDialog(this.fileContacts.ContactTypes.Where<KeyValuePair<string, int>>((Func<KeyValuePair<string, int>, bool>) (x => x.Value != 1 || !x.Key.ToLower().Contains("borrower"))).Select<KeyValuePair<string, int>, string>((Func<KeyValuePair<string, int>, string>) (pair => pair.Key)).ToList<string>()))
      {
        if (dialog.ShowDialog() != DialogResult.OK)
          return;
        if (this.fileContacts.ContactTypes.Where<KeyValuePair<string, int>>((Func<KeyValuePair<string, int>, bool>) (x => x.Value == 1)).Select<KeyValuePair<string, int>, string>((Func<KeyValuePair<string, int>, string>) (pair => pair.Key)).ToList<string>().Exists((Predicate<string>) (x => x.Equals(dialog.contactDetails["ContactType"].ToString()))))
        {
          if (string.IsNullOrEmpty(this.contacts.Find((Predicate<string>) (x =>
          {
            if (x.Contains(dialog.contactDetails["ContactType"]))
            {
              if (x.Split(';')[1] == dialog.contactDetails["Name"])
              {
                if (x.Split(';')[2] == dialog.contactDetails["Email"])
                  return x.Split(';')[4] == "1";
              }
            }
            return false;
          }))))
          {
            if (Utils.Dialog((IWin32Window) this, "You are updating the existing " + dialog.contactDetails["ContactType"].ToString() + " details.  Do you want to save?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
              if (!this.fileContacts.UpdateContactDetails(dialog.contactDetails))
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "Error occured while updating File contact. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
              string str = this.contacts.Find((Predicate<string>) (x =>
              {
                if (!x.Contains(dialog.contactDetails["ContactType"]))
                  return false;
                return x.Split(';')[4] == "1";
              }));
              this.contacts.Add(string.Join(";", dialog.contactDetails["ContactType"], dialog.contactDetails["Name"], dialog.contactDetails["Email"], dialog.contactDetails["CellPhoneNumber"], "1", "1"));
              if (string.IsNullOrEmpty(str))
                this.contacts.Add(string.Join(";", str.Split(';')[2], str.Split(';')[0], str.Split(';')[1], string.Empty, "0", "0"));
            }
            else
              this.contacts.Add(string.Join(";", dialog.contactDetails["ContactType"], dialog.contactDetails["Name"], dialog.contactDetails["Email"], dialog.contactDetails["CellPhoneNumber"], "0", "1"));
          }
        }
        else
        {
          if (!this.fileContacts.UpdateContactDetails(dialog.contactDetails))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Error occured while updating File contact. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          this.contacts.Add(string.Join(";", dialog.contactDetails["ContactType"], dialog.contactDetails["Name"], dialog.contactDetails["Email"], dialog.contactDetails["CellPhoneNumber"], "1", "1"));
        }
        if (!this.loanDataMgr.Save())
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "Unable to save the loan. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.CreateRecipient(dialog);
          this.loanDataMgr.refreshLoanFromServer();
          ListViewItem listViewItem = new ListViewItem(dialog.contactDetails["Name"]);
          listViewItem.SubItems.Add(dialog.contactDetails["Email"]);
          listViewItem.SubItems.Add(dialog.contactDetails["ContactType"]);
          listViewItem.SubItems.Add(dialog.contactDetails["CellPhoneNumber"]);
          listViewItem.SubItems.Add(string.Empty);
          listViewItem.SubItems.Add(string.Empty);
          if (dialog.contactDetails["ContactType"].ToLower().Contains("borrower"))
            listViewItem.Tag = dialog.contactDetails["ContactType"].ToLower() == "borrower" ? (object) this.loanData.CurrentBorrowerPair.Borrower.Id : (object) this.loanData.CurrentBorrowerPair.CoBorrower.Id;
          this.emailLvw.Items.Add(listViewItem);
          this.fileContacts.ContactTypes[dialog.contactDetails["ContactType"]] = 1;
        }
      }
    }

    private void CreateRecipient(OTPAddFileContactDialog dialog)
    {
      DMOSServiceClient dmosServiceClient = new DMOSServiceClient();
      GetDMOSRecipientURLRequest request = new GetDMOSRecipientURLRequest();
      request.loanGuid = this.loanData.GetField("GUID").Replace("{", string.Empty).Replace("}", string.Empty);
      request.caller = new Caller()
      {
        name = "Encompass",
        version = VersionInformation.CurrentVersion.DisplayVersion
      };
      OTPRecipient otpRecipient = new OTPRecipient();
      otpRecipient.PartyId = Guid.NewGuid().ToString();
      otpRecipient.fullName = dialog.contactDetails["Name"];
      otpRecipient.email = dialog.contactDetails["Email"];
      otpRecipient.PhoneNumber = dialog.contactDetails["CellPhoneNumber"];
      otpRecipient.PhoneType = PhoneType.Mobile;
      otpRecipient.role = dialog.contactDetails["ContactType"];
      otpRecipient.borrowerId = dialog.contactDetails["ContactType"].ToLower().Contains("borrower") ? (dialog.contactDetails["ContactType"].ToLower().StartsWith("borrower") ? this.loanData.CurrentBorrowerPair.Borrower.Id : this.loanData.CurrentBorrowerPair.CoBorrower.Id) : (string) null;
      OTPRecipient recipient = otpRecipient;
      List<Party> partyList = new List<Party>();
      Party party = dmosServiceClient.SetPartyDetails(recipient);
      if (party != null)
        partyList.Add(party);
      request.parties = partyList.ToArray();
      Task.WaitAll((Task) dmosServiceClient.GetRecipientURL(request));
    }
  }
}
