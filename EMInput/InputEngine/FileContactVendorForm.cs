// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FileContactVendorForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FileContactVendorForm : UserControl
  {
    private FileContactRecord.ContactTypes contactType;
    private LoanData loan;
    private Sessions.Session session = Session.DefaultInstance;
    private bool isSuperAdmin;
    private Hashtable fieldRights;
    private static Color ExistingFieldColor = Color.FromArgb(200, 227, 254);
    private static Color SelectedFieldColor = Color.FromArgb(254, 232, 208);
    private string fieldId_CopyProvider = "CopyProvider";
    private string fieldId_SettlementAgentCopy = "SettlementAgentCopy";
    private IContainer components;
    private GroupContainer groupContainer1;
    private Button btnCopyProvider;
    private Button btnSettlementAgentCopy;

    public event EventHandler CopyProviderButtonClick;

    public event EventHandler CopySettlementAgentButtonClick;

    public FileContactVendorForm(
      LoanScreen freeScreen,
      FileContactRecord.ContactFields contactFields,
      LoanData loanData)
    {
      this.InitializeComponent();
      if (this.session.UserInfo.IsSuperAdministrator())
        this.isSuperAdmin = true;
      this.Dock = DockStyle.Fill;
      this.contactType = contactFields.ContactType;
      this.loan = loanData;
      this.btnSettlementAgentCopy.Visible = false;
      switch (this.contactType)
      {
        case FileContactRecord.ContactTypes.Borrower:
          this.groupContainer1.Text = "Borrower";
          break;
        case FileContactRecord.ContactTypes.Coborrower:
          this.groupContainer1.Text = "Co-Borrower";
          break;
        case FileContactRecord.ContactTypes.Lender:
          this.groupContainer1.Text = "Lender";
          break;
        case FileContactRecord.ContactTypes.Appraiser:
          this.groupContainer1.Text = "Appraiser";
          break;
        case FileContactRecord.ContactTypes.Escrow:
          this.groupContainer1.Text = "Escrow Company";
          break;
        case FileContactRecord.ContactTypes.Title:
          this.groupContainer1.Text = "Title Insurance Company";
          break;
        case FileContactRecord.ContactTypes.BuyerAttorney:
          this.groupContainer1.Text = "Buyer's Attorney";
          break;
        case FileContactRecord.ContactTypes.SellerAttorney:
          this.groupContainer1.Text = "Seller's Attorney";
          break;
        case FileContactRecord.ContactTypes.BuyerAgent:
          this.groupContainer1.Text = "Buyer's Agent";
          break;
        case FileContactRecord.ContactTypes.SellerAgent:
          this.groupContainer1.Text = "Seller's Agent";
          break;
        case FileContactRecord.ContactTypes.Seller:
          this.groupContainer1.Text = "Seller 1";
          break;
        case FileContactRecord.ContactTypes.Seller2:
          this.groupContainer1.Text = "Seller 2";
          break;
        case FileContactRecord.ContactTypes.Notary:
          this.groupContainer1.Text = "Notary";
          break;
        case FileContactRecord.ContactTypes.Builder:
          this.groupContainer1.Text = "Builder";
          break;
        case FileContactRecord.ContactTypes.HazardInsurance:
          this.groupContainer1.Text = "Hazard Insurance";
          break;
        case FileContactRecord.ContactTypes.MortgageInsurance:
          this.groupContainer1.Text = "Mortgage Insurance";
          break;
        case FileContactRecord.ContactTypes.Surveyor:
          this.groupContainer1.Text = "Surveyor";
          break;
        case FileContactRecord.ContactTypes.FloodInsurance:
          this.groupContainer1.Text = "Flood Insurance";
          break;
        case FileContactRecord.ContactTypes.CreditCompany:
          this.groupContainer1.Text = "Credit Company";
          break;
        case FileContactRecord.ContactTypes.Underwriter:
          this.groupContainer1.Text = "Underwriter";
          break;
        case FileContactRecord.ContactTypes.Servicing:
          this.groupContainer1.Text = "Servicing";
          break;
        case FileContactRecord.ContactTypes.DocSigning:
          this.groupContainer1.Text = "Doc Signing";
          break;
        case FileContactRecord.ContactTypes.Warehouse:
          this.groupContainer1.Text = "Warehouse";
          break;
        case FileContactRecord.ContactTypes.FinancialPlanner:
          this.groupContainer1.Text = "Financial Planner";
          break;
        case FileContactRecord.ContactTypes.Investor:
          this.groupContainer1.Text = "Investor";
          break;
        case FileContactRecord.ContactTypes.AssignTo:
          this.groupContainer1.Text = "Assign To";
          break;
        case FileContactRecord.ContactTypes.Broker:
          this.groupContainer1.Text = "Broker";
          break;
        case FileContactRecord.ContactTypes.DocsPrepared:
          this.groupContainer1.Text = "Docs Prepared By";
          break;
        case FileContactRecord.ContactTypes.Custom1:
          this.groupContainer1.Text = contactFields.Category == string.Empty ? "Custom1" : contactFields.Category;
          break;
        case FileContactRecord.ContactTypes.Custom2:
          this.groupContainer1.Text = contactFields.Category == string.Empty ? "Custom2" : contactFields.Category;
          break;
        case FileContactRecord.ContactTypes.Custom3:
          this.groupContainer1.Text = contactFields.Category == string.Empty ? "Custom3" : contactFields.Category;
          break;
        case FileContactRecord.ContactTypes.Custom4:
          this.groupContainer1.Text = contactFields.Category == string.Empty ? "Custom4" : contactFields.Category;
          break;
        case FileContactRecord.ContactTypes.Seller3:
          this.groupContainer1.Text = "Seller 3";
          break;
        case FileContactRecord.ContactTypes.Seller4:
          this.groupContainer1.Text = "Seller 4";
          break;
        case FileContactRecord.ContactTypes.SettlementAgent:
          this.groupContainer1.Text = "Settlement Agent";
          break;
        case FileContactRecord.ContactTypes.NonBorrowingOwnerContact:
          this.groupContainer1.Text = contactFields.Category == string.Empty ? "None-Borrowing Owner File Contact" : contactFields.Category;
          break;
        case FileContactRecord.ContactTypes.SellerCorporationOfficer:
          this.groupContainer1.Text = "Seller Corporation Officers";
          break;
        default:
          this.groupContainer1.Text = "";
          break;
      }
      this.groupContainer1.Controls.Add((Control) freeScreen);
      if (contactFields.ContactType == FileContactRecord.ContactTypes.Borrower || contactFields.ContactType == FileContactRecord.ContactTypes.Coborrower || contactFields.ContactType == FileContactRecord.ContactTypes.NonBorrowingOwnerContact)
        this.btnCopyProvider.Visible = false;
      if (contactFields.ContactType == FileContactRecord.ContactTypes.Title || contactFields.ContactType == FileContactRecord.ContactTypes.Escrow || contactFields.ContactType == FileContactRecord.ContactTypes.BuyerAttorney)
        this.btnSettlementAgentCopy.Visible = true;
      if (this.loan != null && this.loan.IsInFindFieldForm && this.loan.ButtonSelectionEnabled)
      {
        this.refreshControl(this.btnCopyProvider, "Button_" + this.fieldId_CopyProvider);
        this.refreshControl(this.btnSettlementAgentCopy, "Button_" + this.fieldId_SettlementAgentCopy);
      }
      else
      {
        this.fieldRights = !this.loan.IsTemplate ? this.session.LoanDataMgr.GetFieldAccessList() : (Hashtable) null;
        this.SetButtonAccessMode(this.btnCopyProvider, "Button_" + this.fieldId_CopyProvider);
        this.SetButtonAccessMode(this.btnSettlementAgentCopy, "Button_" + this.fieldId_SettlementAgentCopy);
      }
    }

    private void refreshControl(Button b, string fieldId)
    {
      switch (this.loan.SelectedFieldType(fieldId))
      {
        case LoanData.FindFieldTypes.NewSelect:
          b.BackColor = FileContactVendorForm.SelectedFieldColor;
          break;
        case LoanData.FindFieldTypes.Existing:
          b.BackColor = FileContactVendorForm.ExistingFieldColor;
          break;
        default:
          b.BackColor = Color.White;
          break;
      }
    }

    private void update(Button b, string fieldId)
    {
      switch (this.loan.SelectedFieldType(fieldId))
      {
        case LoanData.FindFieldTypes.None:
          this.loan.AddSelectedField(fieldId);
          break;
        case LoanData.FindFieldTypes.NewSelect:
          this.loan.RemoveSelectedField(fieldId);
          break;
        case LoanData.FindFieldTypes.Existing:
          int num = (int) Utils.Dialog((IWin32Window) null, "You can't remove existing selected field in current list. Please use 'Remove' button to remove existing fields.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
      }
      this.refreshControl(b, fieldId);
    }

    public void SetButtonAccessMode(Button button, string actionID)
    {
      if (this.isSuperAdmin || this.loan.IsInFindFieldForm)
        return;
      if (!actionID.ToLower().StartsWith("button_"))
        actionID = "Button_" + actionID;
      if (this.fieldRights == null || !this.fieldRights.ContainsKey((object) actionID))
        return;
      switch (this.session.LoanDataMgr.GetFieldAccessRights(actionID))
      {
        case BizRule.FieldAccessRight.Hide:
          button.Visible = false;
          break;
        case BizRule.FieldAccessRight.ViewOnly:
          button.Enabled = false;
          break;
      }
    }

    private void btnCopyProvider_Click(object sender, MouseEventArgs e)
    {
      switch (e.Button)
      {
        case MouseButtons.Left:
          if (this.CopyProviderButtonClick == null || this.loan != null && this.loan.IsInFindFieldForm && this.loan.ButtonSelectionEnabled)
            break;
          this.CopyProviderButtonClick((object) this.contactType, (EventArgs) e);
          break;
        case MouseButtons.Right:
          if (this.loan == null || !this.loan.IsInFindFieldForm || !this.loan.ButtonSelectionEnabled)
            break;
          this.update(this.btnCopyProvider, "Button_" + this.fieldId_CopyProvider);
          break;
      }
    }

    private void btnSettlementAgentCopy_Click(object sender, MouseEventArgs e)
    {
      switch (e.Button)
      {
        case MouseButtons.Left:
          if (this.CopySettlementAgentButtonClick == null || this.loan != null && this.loan.IsInFindFieldForm && this.loan.ButtonSelectionEnabled)
            break;
          this.CopySettlementAgentButtonClick((object) this.contactType, (EventArgs) e);
          break;
        case MouseButtons.Right:
          if (this.loan == null || !this.loan.IsInFindFieldForm || !this.loan.ButtonSelectionEnabled)
            break;
          this.update(this.btnSettlementAgentCopy, "Button_" + this.fieldId_SettlementAgentCopy);
          break;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.btnCopyProvider = new Button();
      this.btnSettlementAgentCopy = new Button();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Borders = AnchorStyles.Top;
      this.groupContainer1.Controls.Add((Control) this.btnSettlementAgentCopy);
      this.groupContainer1.Controls.Add((Control) this.btnCopyProvider);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(654, 220);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "groupContainer1";
      this.btnCopyProvider.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCopyProvider.Location = new Point(423, 2);
      this.btnCopyProvider.Name = "btnCopyProvider";
      this.btnCopyProvider.Size = new Size(226, 22);
      this.btnCopyProvider.TabIndex = 0;
      this.btnCopyProvider.Text = "Copy to Settlement Service Provider List";
      this.btnCopyProvider.UseVisualStyleBackColor = true;
      this.btnCopyProvider.MouseUp += new MouseEventHandler(this.btnCopyProvider_Click);
      this.btnSettlementAgentCopy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSettlementAgentCopy.Location = new Point(268, 2);
      this.btnSettlementAgentCopy.Name = "btnSettlementAgentCopy";
      this.btnSettlementAgentCopy.Size = new Size(149, 22);
      this.btnSettlementAgentCopy.TabIndex = 1;
      this.btnSettlementAgentCopy.Text = "Copy to Settlement Agent";
      this.btnSettlementAgentCopy.UseVisualStyleBackColor = true;
      this.btnSettlementAgentCopy.MouseUp += new MouseEventHandler(this.btnSettlementAgentCopy_Click);
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (FileContactVendorForm);
      this.Size = new Size(654, 220);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
