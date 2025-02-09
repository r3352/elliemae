// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.NewCorrespondentTrade
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class NewCorrespondentTrade : Form
  {
    private int TpoId;
    private bool setDefault;
    private IContainer components;
    private TableContainer grpLoans;
    private Button btnCancel;
    private Button btnSave;
    private GroupContainer groupContainer2;
    private StandardIconButton btnDelete;
    private Label label6;
    private TextBox txtOrgId;
    private Label label8;
    private TextBox txtTPOId;
    private StandardIconButton btnSelector;
    private Label label10;
    private TextBox txtCompanyName;
    private RadioButton rdoBestEfforts;
    private RadioButton rdoMandatory;

    public CorrespondentTradeInfo info { get; set; }

    public NewCorrespondentTrade()
    {
      this.InitializeComponent();
      this.info = new CorrespondentTradeInfo();
    }

    public void Refresh(CorrespondentTradeInfo info)
    {
      this.txtCompanyName.Text = info.CompanyName;
      this.txtTPOId.Text = info.TPOID;
      this.txtOrgId.Text = info.OrganizationID;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.ValidateTPOSelection())
      {
        this.DialogResult = DialogResult.None;
      }
      else
      {
        this.info.CompanyName = this.txtCompanyName.Text;
        this.info.TPOID = this.txtTPOId.Text;
        this.info.OrganizationID = this.txtOrgId.Text;
        this.info.ExternalOriginatorManagementID = this.TpoId;
        if (this.rdoBestEfforts.Checked)
          this.info.CommitmentType = CorrespondentTradeCommitmentType.BestEfforts;
        else
          this.info.CommitmentType = CorrespondentTradeCommitmentType.Mandatory;
      }
    }

    private bool ValidateTPOSelection()
    {
      if (this.txtTPOId.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A TPO company selection is required.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (this.rdoBestEfforts.Checked || this.rdoMandatory.Checked)
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "At least one Trade commitment type is required.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      this.txtCompanyName.Text = "";
      this.txtTPOId.Text = "";
      this.txtOrgId.Text = "";
      this.rdoMandatory.Enabled = false;
      this.rdoMandatory.Checked = false;
      this.rdoBestEfforts.Checked = false;
      this.rdoBestEfforts.Enabled = false;
    }

    private void btnSelector_Click(object sender, EventArgs e)
    {
      using (TPOCompanyMasterSelectorForm masterSelectorForm = new TPOCompanyMasterSelectorForm(Session.ConfigurationManager.GetExternalOrganizationsWithoutExtension(Session.UserID, (string) null).Where<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => !x.DisabledLogin)).ToList<ExternalOriginatorManagementData>(), Session.DefaultInstance, false))
      {
        if (masterSelectorForm.ShowDialog((IWin32Window) Session.DefaultInstance.MainForm) != DialogResult.OK)
          return;
        this.ApplyTPOSettingsToControl(masterSelectorForm.SelectedOrganization);
      }
    }

    private void ApplyTPOSettingsToControl(ExternalOriginatorManagementData tpoSettings)
    {
      bool boolean = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.AllowBestEfforts"]);
      this.txtTPOId.Text = tpoSettings.ExternalID;
      this.txtOrgId.Text = tpoSettings.OrgID;
      this.txtCompanyName.Text = tpoSettings.OrganizationName;
      this.TpoId = tpoSettings.oid;
      this.btnDelete.Enabled = true;
      this.rdoMandatory.Enabled = tpoSettings.CommitmentMandatory;
      this.rdoBestEfforts.Enabled = boolean && (tpoSettings.CommitmentUseBestEffort || !tpoSettings.CommitmentMandatory);
      this.rdoMandatory.Checked = this.rdoMandatory.Enabled && !this.rdoBestEfforts.Enabled;
      this.rdoBestEfforts.Checked = !this.rdoMandatory.Enabled && this.rdoBestEfforts.Enabled;
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void NewCorrespondentTrade_Paint(object sender, PaintEventArgs e)
    {
      if (this.setDefault)
        return;
      this.rdoMandatory.Checked = false;
      this.setDefault = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.grpLoans = new TableContainer();
      this.rdoBestEfforts = new RadioButton();
      this.rdoMandatory = new RadioButton();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.groupContainer2 = new GroupContainer();
      this.btnDelete = new StandardIconButton();
      this.label6 = new Label();
      this.txtOrgId = new TextBox();
      this.label8 = new Label();
      this.txtTPOId = new TextBox();
      this.btnSelector = new StandardIconButton();
      this.label10 = new Label();
      this.txtCompanyName = new TextBox();
      this.grpLoans.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnSelector).BeginInit();
      this.SuspendLayout();
      this.grpLoans.Controls.Add((Control) this.rdoBestEfforts);
      this.grpLoans.Controls.Add((Control) this.rdoMandatory);
      this.grpLoans.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.grpLoans.Location = new Point(12, 139);
      this.grpLoans.Margin = new Padding(0);
      this.grpLoans.Name = "grpLoans";
      this.grpLoans.Size = new Size(384, 90);
      this.grpLoans.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.grpLoans.TabIndex = 5;
      this.grpLoans.Text = "Commitment Type";
      this.rdoBestEfforts.AutoSize = true;
      this.rdoBestEfforts.Enabled = false;
      this.rdoBestEfforts.Location = new Point(229, 48);
      this.rdoBestEfforts.Name = "rdoBestEfforts";
      this.rdoBestEfforts.Size = new Size(83, 18);
      this.rdoBestEfforts.TabIndex = 1;
      this.rdoBestEfforts.TabStop = true;
      this.rdoBestEfforts.Text = "Best Efforts";
      this.rdoBestEfforts.UseVisualStyleBackColor = true;
      this.rdoMandatory.AutoSize = true;
      this.rdoMandatory.Enabled = false;
      this.rdoMandatory.Location = new Point(39, 48);
      this.rdoMandatory.Name = "rdoMandatory";
      this.rdoMandatory.Size = new Size(76, 18);
      this.rdoMandatory.TabIndex = 0;
      this.rdoMandatory.TabStop = true;
      this.rdoMandatory.Text = "Mandatory";
      this.rdoMandatory.UseVisualStyleBackColor = true;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(322, 241);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.BackColor = SystemColors.Control;
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(241, 241);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 21;
      this.btnSave.Text = "&OK";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.groupContainer2.Controls.Add((Control) this.btnDelete);
      this.groupContainer2.Controls.Add((Control) this.label6);
      this.groupContainer2.Controls.Add((Control) this.txtOrgId);
      this.groupContainer2.Controls.Add((Control) this.label8);
      this.groupContainer2.Controls.Add((Control) this.txtTPOId);
      this.groupContainer2.Controls.Add((Control) this.btnSelector);
      this.groupContainer2.Controls.Add((Control) this.label10);
      this.groupContainer2.Controls.Add((Control) this.txtCompanyName);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(12, 12);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(384, 119);
      this.groupContainer2.TabIndex = 6;
      this.groupContainer2.Text = "TPO Info";
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(326, 36);
      this.btnDelete.Margin = new Padding(3, 4, 2, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 51;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(7, 85);
      this.label6.Name = "label6";
      this.label6.Size = new Size(80, 13);
      this.label6.TabIndex = 50;
      this.label6.Text = "Organization ID";
      this.txtOrgId.BackColor = SystemColors.Control;
      this.txtOrgId.Enabled = false;
      this.txtOrgId.Location = new Point(159, 79);
      this.txtOrgId.MaxLength = 64;
      this.txtOrgId.Name = "txtOrgId";
      this.txtOrgId.Size = new Size(146, 20);
      this.txtOrgId.TabIndex = 49;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(8, 63);
      this.label8.Name = "label8";
      this.label8.Size = new Size(43, 13);
      this.label8.TabIndex = 48;
      this.label8.Text = "TPO ID";
      this.txtTPOId.BackColor = SystemColors.Control;
      this.txtTPOId.Enabled = false;
      this.txtTPOId.Location = new Point(159, 57);
      this.txtTPOId.MaxLength = 64;
      this.txtTPOId.Name = "txtTPOId";
      this.txtTPOId.Size = new Size(146, 20);
      this.txtTPOId.TabIndex = 47;
      this.btnSelector.BackColor = Color.Transparent;
      this.btnSelector.Location = new Point(307, 36);
      this.btnSelector.MouseDownImage = (Image) null;
      this.btnSelector.Name = "btnSelector";
      this.btnSelector.Size = new Size(16, 16);
      this.btnSelector.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelector.TabIndex = 40;
      this.btnSelector.TabStop = false;
      this.btnSelector.Click += new EventHandler(this.btnSelector_Click);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(8, 41);
      this.label10.Name = "label10";
      this.label10.Size = new Size(82, 13);
      this.label10.TabIndex = 39;
      this.label10.Text = "Company Name";
      this.txtCompanyName.BackColor = SystemColors.Control;
      this.txtCompanyName.Enabled = false;
      this.txtCompanyName.Location = new Point(159, 35);
      this.txtCompanyName.MaxLength = 64;
      this.txtCompanyName.Name = "txtCompanyName";
      this.txtCompanyName.Size = new Size(146, 20);
      this.txtCompanyName.TabIndex = 26;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(408, 275);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.grpLoans);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (NewCorrespondentTrade);
      this.ShowIcon = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "New Correspondent Trade";
      this.Paint += new PaintEventHandler(this.NewCorrespondentTrade_Paint);
      this.grpLoans.ResumeLayout(false);
      this.grpLoans.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnSelector).EndInit();
      this.ResumeLayout(false);
    }
  }
}
