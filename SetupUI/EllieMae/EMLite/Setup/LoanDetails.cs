// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanDetails
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanDetails : Form
  {
    private const int ROLE_COLUMN_WIDTH = 80;
    private PipelineInfo[] pipelineInfos;
    private RoleInfo[] roles;
    private ListViewSortManager sortMngr;
    private System.ComponentModel.Container components;
    private ColumnHeader loanNumberChdr;
    private ColumnHeader lastNameChdr;
    private ColumnHeader firstNameChdr;
    private ColumnHeader lockChdr;
    private ColumnHeader addressChdr;
    private ColumnHeader amountChdr;
    private ColumnHeader typeChdr;
    private ColumnHeader purposeChdr;
    private ColumnHeader coBorChdr;
    private ColumnHeader loanNameChdr;
    private ColumnHeader guidChdr;
    private ColumnHeader lockRequestChdr;
    private ColumnHeader loanStatusChdr;
    private ListView lvwDetails;
    private Button btnClose;

    public LoanDetails(PipelineInfo pipelineInfo)
      : this(new PipelineInfo[1]{ pipelineInfo })
    {
    }

    public LoanDetails(PipelineInfo[] pipelineInfos)
    {
      this.pipelineInfos = pipelineInfos;
      this.InitializeComponent();
      this.initializeControls();
      this.populateControls();
      if (1 >= pipelineInfos.Length)
        return;
      this.setSortManager();
    }

    private void initializeControls()
    {
      this.roles = ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      ColumnHeader[] values = new ColumnHeader[this.roles.Length];
      int num = 0;
      foreach (RoleInfo role in this.roles)
        values[num++] = new ColumnHeader()
        {
          Text = role.RoleAbbr,
          Width = 80
        };
      this.lvwDetails.Columns.AddRange(values);
    }

    private void populateControls()
    {
      this.lvwDetails.Items.Clear();
      if (this.pipelineInfos == null || this.pipelineInfos.Length == 0)
        return;
      this.lvwDetails.BeginUpdate();
      foreach (PipelineInfo pipelineInfo in this.pipelineInfos)
      {
        ListViewItem listViewItem = new ListViewItem(pipelineInfo.LoanNumber);
        listViewItem.SubItems.Add((string) pipelineInfo.Info[(object) "Loan.BorrowerLastName"]);
        listViewItem.SubItems.Add((string) pipelineInfo.Info[(object) "Loan.BorrowerFirstName"]);
        listViewItem.SubItems.Add((string) pipelineInfo.Info[(object) "Loan.Address1"]);
        listViewItem.SubItems.Add(Utils.ParseDecimal(pipelineInfo.Info[(object) "Loan.TotalLoanAmount"]) > 0M ? Utils.ParseDecimal(pipelineInfo.Info[(object) "Loan.TotalLoanAmount"]).ToString("N2") : "");
        listViewItem.SubItems.Add(pipelineInfo.LoanType);
        listViewItem.SubItems.Add(pipelineInfo.LoanPurpose);
        listViewItem.SubItems.Add((string) pipelineInfo.Info[(object) "Loan.CoBorrowerName"]);
        listViewItem.SubItems.Add(string.Empty == (string) pipelineInfo.Info[(object) "Loan.ActionTaken"] ? "Active Loan" : (string) pipelineInfo.Info[(object) "Loan.ActionTaken"]);
        DateTime date = Utils.ParseDate(pipelineInfo.Info[(object) "Loan.LockRequestDate"]);
        if (date != DateTime.MinValue)
          listViewItem.SubItems.Add(date.ToString("MM/dd/yyyy hh:mm tt"));
        else
          listViewItem.SubItems.Add("");
        listViewItem.SubItems.Add(pipelineInfo.LoanName);
        listViewItem.SubItems.Add(pipelineInfo.GUID);
        listViewItem.SubItems.Add(this.getLockReason(pipelineInfo));
        foreach (RoleInfo role in this.roles)
          listViewItem.SubItems.Add(this.getLoanAssociate(role.RoleID, pipelineInfo));
        this.lvwDetails.Items.Add(listViewItem);
      }
      this.lvwDetails.EndUpdate();
    }

    private string getLockReason(PipelineInfo loan)
    {
      if (LoanInfo.LockReason.Downloaded == loan.LockInfo.LockedFor)
        return "Downloaded by " + loan.LockInfo.LockedBy;
      return LoanInfo.LockReason.OpenForWork == loan.LockInfo.LockedFor ? "Opened by " + loan.LockInfo.LockedBy : "Not Locked";
    }

    private string getLoanAssociate(int roleId, PipelineInfo loan)
    {
      if (loan.LoanAssociates != null)
      {
        foreach (PipelineInfo.LoanAssociateInfo loanAssociate in loan.LoanAssociates)
        {
          if (roleId == loanAssociate.RoleID)
            return loanAssociate.UserID;
        }
      }
      return string.Empty;
    }

    private void setSortManager()
    {
      ArrayList arrayList = new ArrayList((ICollection) new System.Type[13]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewCurrencySort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewDateSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort)
      });
      arrayList.AddRange((ICollection) ArrayList.Repeat((object) typeof (ListViewTextCaseInsensitiveSort), this.roles.Length));
      this.sortMngr = new ListViewSortManager(this.lvwDetails, (System.Type[]) arrayList.ToArray(typeof (System.Type)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void closeBtn_Click(object sender, EventArgs e) => this.Close();

    private void InitializeComponent()
    {
      this.lvwDetails = new ListView();
      this.loanNumberChdr = new ColumnHeader();
      this.lastNameChdr = new ColumnHeader();
      this.firstNameChdr = new ColumnHeader();
      this.addressChdr = new ColumnHeader();
      this.amountChdr = new ColumnHeader();
      this.typeChdr = new ColumnHeader();
      this.purposeChdr = new ColumnHeader();
      this.coBorChdr = new ColumnHeader();
      this.loanStatusChdr = new ColumnHeader();
      this.lockRequestChdr = new ColumnHeader();
      this.loanNameChdr = new ColumnHeader();
      this.guidChdr = new ColumnHeader();
      this.lockChdr = new ColumnHeader();
      this.btnClose = new Button();
      this.SuspendLayout();
      this.lvwDetails.AllowColumnReorder = true;
      this.lvwDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwDetails.Columns.AddRange(new ColumnHeader[13]
      {
        this.loanNumberChdr,
        this.lastNameChdr,
        this.firstNameChdr,
        this.addressChdr,
        this.amountChdr,
        this.typeChdr,
        this.purposeChdr,
        this.coBorChdr,
        this.loanStatusChdr,
        this.lockRequestChdr,
        this.loanNameChdr,
        this.guidChdr,
        this.lockChdr
      });
      this.lvwDetails.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lvwDetails.GridLines = true;
      this.lvwDetails.Location = new Point(0, 0);
      this.lvwDetails.Name = "lvwDetails";
      this.lvwDetails.Size = new Size(772, 336);
      this.lvwDetails.TabIndex = 0;
      this.lvwDetails.UseCompatibleStateImageBehavior = false;
      this.lvwDetails.View = View.Details;
      this.loanNumberChdr.Text = "Loan Number";
      this.loanNumberChdr.Width = 92;
      this.lastNameChdr.Text = "Last Name";
      this.lastNameChdr.Width = 80;
      this.firstNameChdr.Text = "First Name";
      this.firstNameChdr.Width = 79;
      this.addressChdr.DisplayIndex = 4;
      this.addressChdr.Text = "Address";
      this.addressChdr.Width = 74;
      this.amountChdr.DisplayIndex = 5;
      this.amountChdr.Text = "Total Loan Amount";
      this.amountChdr.Width = 108;
      this.typeChdr.DisplayIndex = 6;
      this.typeChdr.Text = "Loan Type";
      this.typeChdr.Width = 84;
      this.purposeChdr.DisplayIndex = 7;
      this.purposeChdr.Text = "Loan Purpose";
      this.purposeChdr.Width = 98;
      this.coBorChdr.DisplayIndex = 8;
      this.coBorChdr.Text = "Co-Borrower";
      this.coBorChdr.Width = 88;
      this.loanStatusChdr.DisplayIndex = 10;
      this.loanStatusChdr.Text = "Loan Status";
      this.lockRequestChdr.DisplayIndex = 12;
      this.lockRequestChdr.Text = "Lock Request";
      this.loanNameChdr.DisplayIndex = 9;
      this.loanNameChdr.Text = "Loan Name";
      this.loanNameChdr.Width = 86;
      this.guidChdr.Text = "GUID";
      this.lockChdr.DisplayIndex = 3;
      this.lockChdr.Text = "File Status";
      this.lockChdr.Width = 93;
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.btnClose.Location = new Point(689, 344);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Close";
      this.btnClose.Click += new EventHandler(this.closeBtn_Click);
      this.AcceptButton = (IButtonControl) this.btnClose;
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(772, 374);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.lvwDetails);
      this.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MinimizeBox = false;
      this.Name = nameof (LoanDetails);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Loan Details";
      this.ResumeLayout(false);
    }
  }
}
