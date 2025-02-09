// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.BorrowerLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class BorrowerLink : PipelineImageLink
  {
    private bool isBorrower = true;

    public BorrowerLink(Control parentControl, PipelineElementData data)
      : base(parentControl, data, BorrowerLink.getDisplayValue(data), (Image) Resources.borrower_contact, (Image) Resources.borrower_contact_over)
    {
      this.isBorrower = string.Compare(this.CurrentData.FieldName, "Loan.BorrowerName", true) == 0;
    }

    public BorrowerLink(Control parentControl)
      : base(parentControl, (Image) Resources.borrower_contact, (Image) Resources.borrower_contact_over)
    {
    }

    protected override void OnLinkClicked(object sender, EventArgs e)
    {
      Point position = Cursor.Position;
      LoanData loanData = this.GetLoanData();
      if (loanData == null)
        return;
      this.createContextMenu(loanData)?.Show(position);
    }

    private ContextMenuStrip createContextMenu(LoanData loanData)
    {
      ContextMenuStrip contextMenu = new ContextMenuStrip();
      contextMenu.ShowImageMargin = false;
      ObjectWithImage dataSource = new ObjectWithImage(this.getBorrowerName(loanData), (Image) Resources.borrower_contact_icon_menu);
      contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) dataSource, ToolStripMenuItemEx.ToolStripItemType.Header));
      string phoneNumber1 = this.isBorrower ? loanData.GetField("66") : loanData.GetField("98");
      string phoneNumber2 = this.isBorrower ? loanData.GetField("FE0117") : loanData.GetField("FE0217");
      string phoneNumber3 = this.isBorrower ? loanData.GetField("1490") : loanData.GetField("1480");
      string emailAddr = this.isBorrower ? loanData.GetField("1240") : loanData.GetField("1268");
      if (phoneNumber1 != "")
        contextMenu.Items.Add((ToolStripItem) this.createPhoneLinkMenuItem(phoneNumber1, PhoneLink.ContactType.HomePhone));
      if (phoneNumber2 != "")
        contextMenu.Items.Add((ToolStripItem) this.createPhoneLinkMenuItem(phoneNumber2, PhoneLink.ContactType.WorkPhone));
      if (phoneNumber3 != "")
        contextMenu.Items.Add((ToolStripItem) this.createPhoneLinkMenuItem(phoneNumber3, PhoneLink.ContactType.CellPhone));
      if (emailAddr != "")
        contextMenu.Items.Add((ToolStripItem) this.createEmailLinkMenuItem(emailAddr));
      if (contextMenu.Items.Count > 1)
        contextMenu.Items.Add((ToolStripItem) new ToolStripSeparatorEx());
      if (((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.GlobalTab_Contacts))
      {
        string crmContactGuid = this.getCRMContactGuid(loanData);
        if (crmContactGuid != null)
        {
          ToolStripMenuItem actionMenuItem = this.createActionMenuItem("Create Appointment", new EventHandler(this.onCreateAppointment));
          actionMenuItem.Tag = (object) crmContactGuid;
          contextMenu.Items.Add((ToolStripItem) actionMenuItem);
        }
      }
      DocumentLog reportDocumentLog = this.getCreditReportDocumentLog(loanData);
      if (reportDocumentLog == null)
        contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Order Credit", new EventHandler(this.onOrderCredit)));
      else if ((reportDocumentLog.Guid ?? "") != "")
        contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("View Credit", new EventHandler(this.onViewCredit)));
      return contextMenu;
    }

    private string getCRMContactGuid(LoanData loanData)
    {
      BorrowerPair borrowerPair = loanData.GetBorrowerPairs()[0];
      string str = this.isBorrower ? borrowerPair.Borrower.Id : borrowerPair.CoBorrower.Id;
      foreach (CRMLog crmLog in loanData.GetLogList().GetAllCRMMapping())
      {
        if (crmLog.MappingType == CRMLogType.BorrowerContact && crmLog.MappingID == str)
          return crmLog.ContactGuid;
      }
      return (string) null;
    }

    private string getBorrowerName(LoanData loanData)
    {
      return loanData.CurrentBorrowerPair.Borrower.LastName + ", " + loanData.CurrentBorrowerPair.Borrower.FirstName;
    }

    private DocumentLog getCreditReportDocumentLog(LoanData loanData)
    {
      foreach (DocumentLog reportDocumentLog in loanData.GetLogList().GetDocumentsByTitle(Epass.Credit.FullName))
      {
        if (reportDocumentLog.IsePASS)
          return reportDocumentLog;
      }
      return (DocumentLog) null;
    }

    private ToolStripMenuItem createActionMenuItem(string text, EventHandler clickHandler)
    {
      ToolStripMenuItemEx actionMenuItem = new ToolStripMenuItemEx((object) text, ToolStripMenuItemEx.ToolStripItemType.Clickable);
      actionMenuItem.Click += clickHandler;
      return (ToolStripMenuItem) actionMenuItem;
    }

    private ToolStripMenuItem createEmailLinkMenuItem(string emailAddr)
    {
      ToolStripMenuItemEx emailLinkMenuItem = new ToolStripMenuItemEx((object) new ObjectWithImage(emailAddr, (Image) Resources.email_icon_menu), ToolStripMenuItemEx.ToolStripItemType.Clickable);
      emailLinkMenuItem.Click += new EventHandler(this.onEmailMenuClick);
      return (ToolStripMenuItem) emailLinkMenuItem;
    }

    private ToolStripMenuItem createPhoneLinkMenuItem(
      string phoneNumber,
      PhoneLink.ContactType phoneType)
    {
      ToolStripMenuItemEx phoneLinkMenuItem = new ToolStripMenuItemEx((object) new PhoneLink(phoneNumber, phoneType, false), ToolStripMenuItemEx.ToolStripItemType.Clickable);
      phoneLinkMenuItem.Click += new EventHandler(this.onPhoneMenuClick);
      return (ToolStripMenuItem) phoneLinkMenuItem;
    }

    private void onOrderCredit(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<IEPass>().ProcessURL(Epass.Credit.Url);
    }

    private void onViewCredit(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<IEPass>().View(Epass.Credit.FullName);
    }

    private void onCreateAppointment(object sender, EventArgs e)
    {
      BorrowerInfo borrower = Session.ContactManager.GetBorrower(((ToolStripItem) sender).Tag.ToString());
      if (borrower == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentControl, "The linked contact record could not be found and may have been deleted. You will need to re-link the borrower to the correct contact to use this feature.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        ContactInfo contactInfo = new ContactInfo(borrower.FullName, borrower.ContactID.ToString(), CategoryType.Borrower);
        Session.Application.GetService<ICalendar>().AddAppointment(new ContactInfo[1]
        {
          contactInfo
        });
      }
    }

    private void onEmailMenuClick(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<ILoanEditor>().StartConversation(new ConversationLog(DateTime.Now, Session.UserID)
      {
        IsEmail = true,
        Email = string.Concat(((ToolStripMenuItemEx) sender).DataSource),
        Name = !this.isBorrower ? Session.LoanData.GetField("68") + " " + Session.LoanData.GetField("69") : Session.LoanData.GetField("36") + " " + Session.LoanData.GetField("37")
      });
    }

    private void onPhoneMenuClick(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<ILoanEditor>().StartConversation(new ConversationLog(DateTime.Now, Session.UserID)
      {
        IsEmail = false,
        Phone = ((ToolStripMenuItemEx) sender).DataSource.ToString() ?? "",
        Name = !this.isBorrower ? Session.LoanData.GetField("68") + " " + Session.LoanData.GetField("69") : Session.LoanData.GetField("36") + " " + Session.LoanData.GetField("37")
      });
    }

    private static Element getDisplayValue(PipelineElementData pdata)
    {
      string str1;
      string str2;
      if (string.Compare(pdata.FieldName, "Loan.BorrowerName", true) == 0)
      {
        str1 = (string) pdata.PipelineInfo.Info[(object) "Loan.BorrowerFirstName"];
        str2 = (string) pdata.PipelineInfo.Info[(object) "Loan.BorrowerLastName"];
      }
      else
      {
        str1 = (string) pdata.PipelineInfo.Info[(object) "Loan.CoBorrowerFirstName"];
        str2 = (string) pdata.PipelineInfo.Info[(object) "Loan.CoBorrowerLastName"];
      }
      if (str1 == null || str2 == null)
        return (Element) new TextElement(string.Concat(pdata.GetValue()));
      return str1 == "" && str2 == "" ? (Element) null : (Element) new FormattedText("<B>" + str2 + ",</B> " + str1);
    }
  }
}
