// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.ImportMain
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using ImportGenesisData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class ImportMain : Form, IHelp
  {
    private const string className = "ImportMain";
    private static string sw = Tracing.SwImportExport;
    private const string AUTO_LO_ASSIGNMENT_KEY = "Import.AutoLOAssignment";
    private bool autoLoAssignment;
    private string loanOfficerId = string.Empty;
    private RadioButton rbFannieMae34;
    private List<ImportMain.LoanOfficer> loanOfficers;
    private System.ComponentModel.Container components;
    private Panel pnlButtons;
    private Button btnCancel;
    private Button btnContinue;
    private Panel pnlAssignment;
    private GroupBox grpAssignment;
    private ComboBox cboLoanOfficers;
    private RadioButton rbAssign;
    private RadioButton rbLeaveBlank;
    private Label lbAssignment1;
    private Panel pnlSource;
    private GroupBox grpSource;
    private RadioButton rbCalyxPoint;
    private RadioButton rbGenesis;
    private RadioButton rbFannieMae;
    private RadioButton rbContour;

    public event EventHandler LoanImported;

    public ImportMain()
    {
      this.InitializeComponent();
      this.initializeDialog();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (ImportMain));
    }

    private void initializeDialog()
    {
      foreach (LoanFolderAclInfo accessibleLoanFolder in ((LoanFoldersAclManager) Session.ACL.GetAclManager(AclCategory.LoanFolderMove)).GetAccessibleLoanFolders(AclFeature.LoanMgmt_Import, Session.UserID, Session.UserInfo.UserPersonas))
      {
        switch (accessibleLoanFolder.FolderName)
        {
          case "Calyx Point":
            this.rbCalyxPoint.Enabled = accessibleLoanFolder.MoveFromAccess == 1;
            break;
          case "Fannie Mae 3.x":
            this.rbFannieMae.Enabled = accessibleLoanFolder.MoveFromAccess == 1;
            break;
          case "ULAD":
            this.rbFannieMae34.Enabled = accessibleLoanFolder.MoveFromAccess == 1;
            break;
          case "Contour - TLH 5.3":
            this.rbContour.Enabled = accessibleLoanFolder.MoveFromAccess == 1;
            break;
          case "Genesis":
            this.rbGenesis.Enabled = accessibleLoanFolder.MoveFromAccess == 1;
            break;
        }
      }
      if (EnConfigurationSettings.GlobalSettings.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        this.rbGenesis.Enabled = false;
      if (Session.UserInfo.IsAdministrator() && (this.autoLoAssignment = this.getImportValue("Import.AutoLOAssignment")))
        this.rbLeaveBlank.Checked = true;
      if (!this.autoLoAssignment)
      {
        this.pnlAssignment.Visible = false;
        this.Height -= this.pnlAssignment.Height;
      }
      if (this.rbFannieMae34.Enabled)
        this.rbFannieMae34.Checked = true;
      else if (this.rbFannieMae.Enabled)
        this.rbFannieMae.Checked = true;
      else if (this.rbFannieMae34.Enabled)
        this.rbFannieMae34.Checked = true;
      else if (this.rbContour.Enabled)
        this.rbContour.Checked = true;
      else if (this.rbGenesis.Enabled)
        this.rbGenesis.Checked = true;
      this.btnContinue.Enabled = this.rbGenesis.Enabled || this.rbContour.Enabled || this.rbFannieMae.Enabled || this.rbFannieMae34.Enabled || this.rbCalyxPoint.Enabled;
      if (Session.StartupInfo.AllowURLA2020)
        return;
      this.rbFannieMae34.Visible = false;
    }

    private bool getImportValue(string importKey)
    {
      return EnableDisableSetting.Enabled == (EnableDisableSetting) Session.ServerManager.GetServerSetting(importKey);
    }

    private void getLoanOfficerList()
    {
      this.loanOfficers = new List<ImportMain.LoanOfficer>();
      RolesMappingInfo roleMappingInfo = ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetRoleMappingInfo(RealWorldRoleID.LoanOfficer);
      if (roleMappingInfo.RoleIDList == null || roleMappingInfo.RoleIDList.Length == 0)
        return;
      foreach (int roleId in roleMappingInfo.RoleIDList)
      {
        foreach (UserInfo userInfo in Session.OrganizationManager.GetScopedUsersWithRole(roleId))
        {
          ImportMain.LoanOfficer loanOfficer = new ImportMain.LoanOfficer(userInfo.Userid, userInfo.FirstName, userInfo.LastName);
          if (!this.loanOfficers.Contains(loanOfficer))
            this.loanOfficers.Add(loanOfficer);
        }
      }
      this.loanOfficers.Sort();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void rbLoanAssignment_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rbLeaveBlank.Checked)
      {
        this.cboLoanOfficers.Enabled = false;
      }
      else
      {
        if (this.loanOfficers == null)
        {
          this.getLoanOfficerList();
          this.cboLoanOfficers.DataSource = (object) this.loanOfficers;
        }
        if (this.loanOfficers.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "There are currently no Loan Officers to select from.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.rbLeaveBlank.Checked = true;
        }
        else
        {
          this.cboLoanOfficers.Enabled = true;
          if (string.Empty != this.loanOfficerId)
          {
            ImportMain.LoanOfficer loanOfficer = new ImportMain.LoanOfficer(this.loanOfficerId);
            if (this.loanOfficers.Contains(loanOfficer))
            {
              this.cboLoanOfficers.SelectedItem = (object) loanOfficer;
            }
            else
            {
              this.cboLoanOfficers.SelectedIndex = 0;
              this.loanOfficerId = string.Empty;
            }
          }
          else
            this.cboLoanOfficers.SelectedIndex = 0;
          this.cboLoanOfficers.Focus();
        }
      }
    }

    private void cboLoanOfficers_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.loanOfficerId = ((ImportMain.LoanOfficer) this.cboLoanOfficers.SelectedItem).UserId;
    }

    private void btnContinue_Click(object sender, EventArgs e)
    {
      if (string.Empty != this.loanOfficerId && this.rbLeaveBlank.Checked)
        this.loanOfficerId = string.Empty;
      this.DialogResult = DialogResult.OK;
      if (this.rbCalyxPoint.Checked)
      {
        int num1 = (int) new ImportPoint(this.loanOfficerId).ShowDialog((IWin32Window) this);
      }
      else if (this.rbFannieMae.Checked || this.rbFannieMae34.Checked)
      {
        using (ImportFannie importFannie = new ImportFannie(this.loanOfficerId, this.rbFannieMae34.Checked))
        {
          importFannie.LoanImported += new EventHandler(this.importFannieForm_LoanImported);
          int num2 = (int) importFannie.ShowDialog((IWin32Window) this);
        }
      }
      else if (this.rbContour.Checked)
      {
        int num3 = (int) new ImportContour(this.loanOfficerId).ShowDialog((IWin32Window) this);
      }
      else
      {
        if (!this.rbGenesis.Checked)
          return;
        int num4 = (int) new GenesisImportDialog(this.loanOfficerId).ShowDialog((IWin32Window) this);
      }
    }

    private void importFannieForm_LoanImported(object sender, EventArgs e)
    {
      if (this.LoanImported == null)
        return;
      this.LoanImported(sender, e);
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (Keys.F1 != e.KeyCode)
        return;
      this.ShowHelp();
    }

    private void InitializeComponent()
    {
      this.pnlButtons = new Panel();
      this.btnCancel = new Button();
      this.btnContinue = new Button();
      this.pnlAssignment = new Panel();
      this.grpAssignment = new GroupBox();
      this.cboLoanOfficers = new ComboBox();
      this.rbAssign = new RadioButton();
      this.rbLeaveBlank = new RadioButton();
      this.lbAssignment1 = new Label();
      this.pnlSource = new Panel();
      this.grpSource = new GroupBox();
      this.rbFannieMae34 = new RadioButton();
      this.rbCalyxPoint = new RadioButton();
      this.rbGenesis = new RadioButton();
      this.rbFannieMae = new RadioButton();
      this.rbContour = new RadioButton();
      this.pnlButtons.SuspendLayout();
      this.pnlAssignment.SuspendLayout();
      this.grpAssignment.SuspendLayout();
      this.pnlSource.SuspendLayout();
      this.grpSource.SuspendLayout();
      this.SuspendLayout();
      this.pnlButtons.Controls.Add((Control) this.btnCancel);
      this.pnlButtons.Controls.Add((Control) this.btnContinue);
      this.pnlButtons.Dock = DockStyle.Top;
      this.pnlButtons.Location = new Point(0, 263);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(500, 36);
      this.pnlButtons.TabIndex = 25;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(411, 6);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 14;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = false;
      this.btnContinue.BackColor = SystemColors.Control;
      this.btnContinue.Location = new Point(330, 6);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(75, 24);
      this.btnContinue.TabIndex = 15;
      this.btnContinue.Text = "C&ontinue";
      this.btnContinue.UseVisualStyleBackColor = false;
      this.btnContinue.Click += new EventHandler(this.btnContinue_Click);
      this.pnlAssignment.Controls.Add((Control) this.grpAssignment);
      this.pnlAssignment.Dock = DockStyle.Top;
      this.pnlAssignment.Location = new Point(0, 138);
      this.pnlAssignment.Name = "pnlAssignment";
      this.pnlAssignment.Size = new Size(500, 125);
      this.pnlAssignment.TabIndex = 24;
      this.grpAssignment.Controls.Add((Control) this.cboLoanOfficers);
      this.grpAssignment.Controls.Add((Control) this.rbAssign);
      this.grpAssignment.Controls.Add((Control) this.rbLeaveBlank);
      this.grpAssignment.Controls.Add((Control) this.lbAssignment1);
      this.grpAssignment.Location = new Point(12, 12);
      this.grpAssignment.Name = "grpAssignment";
      this.grpAssignment.Size = new Size(474, 113);
      this.grpAssignment.TabIndex = 3;
      this.grpAssignment.TabStop = false;
      this.grpAssignment.Text = "Loan Officer Assignment";
      this.cboLoanOfficers.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLoanOfficers.FormattingEnabled = true;
      this.cboLoanOfficers.Location = new Point(197, 78);
      this.cboLoanOfficers.Name = "cboLoanOfficers";
      this.cboLoanOfficers.Size = new Size(265, 21);
      this.cboLoanOfficers.TabIndex = 3;
      this.cboLoanOfficers.SelectedIndexChanged += new EventHandler(this.cboLoanOfficers_SelectedIndexChanged);
      this.rbAssign.AutoSize = true;
      this.rbAssign.Location = new Point(9, 80);
      this.rbAssign.Name = "rbAssign";
      this.rbAssign.Size = new Size(182, 17);
      this.rbAssign.TabIndex = 2;
      this.rbAssign.TabStop = true;
      this.rbAssign.Text = "Assign the following Loan Officer:";
      this.rbAssign.UseVisualStyleBackColor = true;
      this.rbAssign.CheckedChanged += new EventHandler(this.rbLoanAssignment_CheckedChanged);
      this.rbLeaveBlank.AutoSize = true;
      this.rbLeaveBlank.Location = new Point(9, 57);
      this.rbLeaveBlank.Name = "rbLeaveBlank";
      this.rbLeaveBlank.Size = new Size(163, 17);
      this.rbLeaveBlank.TabIndex = 1;
      this.rbLeaveBlank.TabStop = true;
      this.rbLeaveBlank.Text = "Do not assign a Loan Officer.";
      this.rbLeaveBlank.UseVisualStyleBackColor = true;
      this.rbLeaveBlank.CheckedChanged += new EventHandler(this.rbLoanAssignment_CheckedChanged);
      this.lbAssignment1.Location = new Point(6, 16);
      this.lbAssignment1.Name = "lbAssignment1";
      this.lbAssignment1.Size = new Size(460, 38);
      this.lbAssignment1.TabIndex = 0;
      this.lbAssignment1.Text = "If the name of the Loan Officer in the imported file does not exactly match an Encompass Loan Officer, then:";
      this.lbAssignment1.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlSource.Controls.Add((Control) this.grpSource);
      this.pnlSource.Dock = DockStyle.Top;
      this.pnlSource.Location = new Point(0, 0);
      this.pnlSource.Name = "pnlSource";
      this.pnlSource.Size = new Size(500, 138);
      this.pnlSource.TabIndex = 23;
      this.grpSource.Controls.Add((Control) this.rbFannieMae34);
      this.grpSource.Controls.Add((Control) this.rbCalyxPoint);
      this.grpSource.Controls.Add((Control) this.rbGenesis);
      this.grpSource.Controls.Add((Control) this.rbFannieMae);
      this.grpSource.Controls.Add((Control) this.rbContour);
      this.grpSource.Location = new Point(12, 12);
      this.grpSource.Name = "grpSource";
      this.grpSource.Size = new Size(474, 126);
      this.grpSource.TabIndex = 19;
      this.grpSource.TabStop = false;
      this.grpSource.Text = "Select Import File Source";
      this.rbFannieMae34.AutoSize = true;
      this.rbFannieMae34.Location = new Point(9, 69);
      this.rbFannieMae34.Name = "rbFannieMae34";
      this.rbFannieMae34.Size = new Size(117, 17);
      this.rbFannieMae34.TabIndex = 18;
      this.rbFannieMae34.TabStop = true;
      this.rbFannieMae34.Text = "ULAD / iLAD (MISMO 3.4)";
      this.rbFannieMae34.UseVisualStyleBackColor = true;
      this.rbCalyxPoint.ForeColor = SystemColors.ControlText;
      this.rbCalyxPoint.Location = new Point(9, 19);
      this.rbCalyxPoint.Name = "rbCalyxPoint";
      this.rbCalyxPoint.Size = new Size(104, 24);
      this.rbCalyxPoint.TabIndex = 13;
      this.rbCalyxPoint.Text = "Calyx Point";
      this.rbGenesis.ForeColor = SystemColors.ControlText;
      this.rbGenesis.Location = new Point(166, 88);
      this.rbGenesis.Name = "rbGenesis";
      this.rbGenesis.Size = new Size(112, 24);
      this.rbGenesis.TabIndex = 17;
      this.rbGenesis.Text = "Genesis";
      this.rbGenesis.Visible = false;
      this.rbFannieMae.ForeColor = SystemColors.ControlText;
      this.rbFannieMae.Location = new Point(9, 42);
      this.rbFannieMae.Name = "rbFannieMae";
      this.rbFannieMae.Size = new Size(151, 24);
      this.rbFannieMae.TabIndex = 14;
      this.rbFannieMae.Text = "Fannie Mae 3.0/3.2";
      this.rbContour.ForeColor = SystemColors.ControlText;
      this.rbContour.Location = new Point(166, 65);
      this.rbContour.Name = "rbContour";
      this.rbContour.Size = new Size(112, 24);
      this.rbContour.TabIndex = 16;
      this.rbContour.Text = "Contour - TLH 5.3";
      this.rbContour.Visible = false;
      this.AcceptButton = (IButtonControl) this.btnContinue;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(500, 298);
      this.Controls.Add((Control) this.pnlButtons);
      this.Controls.Add((Control) this.pnlAssignment);
      this.Controls.Add((Control) this.pnlSource);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportMain);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.pnlButtons.ResumeLayout(false);
      this.pnlAssignment.ResumeLayout(false);
      this.grpAssignment.ResumeLayout(false);
      this.grpAssignment.PerformLayout();
      this.pnlSource.ResumeLayout(false);
      this.grpSource.ResumeLayout(false);
      this.grpSource.PerformLayout();
      this.ResumeLayout(false);
    }

    public class LoanOfficer : IComparable
    {
      private string userId = string.Empty;
      private string name = string.Empty;

      public string UserId => this.userId;

      public string Name => this.name;

      public LoanOfficer(string userId) => this.userId = userId != null ? userId : string.Empty;

      public LoanOfficer(string userId, string firstName, string lastName)
      {
        this.userId = userId != null ? userId : string.Empty;
        firstName = firstName != null ? firstName : string.Empty;
        lastName = lastName != null ? lastName : string.Empty;
        if (string.Empty != firstName)
          this.name = firstName;
        if (string.Empty != firstName && string.Empty != lastName)
          this.name += " ";
        if (!(string.Empty != lastName))
          return;
        this.name += lastName;
      }

      public override string ToString() => this.Name;

      public override bool Equals(object obj)
      {
        bool flag = false;
        if (obj is ImportMain.LoanOfficer loanOfficer)
          flag = this.userId == loanOfficer.userId;
        return flag;
      }

      public override int GetHashCode() => this.userId.GetHashCode();

      public int CompareTo(object obj)
      {
        int num = 0;
        if (obj is ImportMain.LoanOfficer loanOfficer)
          num = this.name.CompareTo(loanOfficer.name);
        return num;
      }
    }
  }
}
