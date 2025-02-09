// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.MaintainQueriesDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class MaintainQueriesDialog : Form
  {
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private CampaignFrequencyNameProvider frequencyNames = new CampaignFrequencyNameProvider();
    private System.ComponentModel.Container components;
    private Panel pnlButtons;
    private Label lblSeparator;
    private Button btnCancel;
    private Button btnOK;
    private Panel panel1;
    private CheckBox cbAutoDeleteQuery;
    private CheckBox cbAutoAddQuery;
    private Label label1;
    private Label label2;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    private Button btnEditAddQuery;
    private Button btnEditDeleteQuery;
    private Label label3;
    private Label label4;
    private Label label7;
    private Label lblDays;
    private NumericUpDown nudFrequencyDays;
    private ComboBox cboFrequencyType;
    private Label lblFrequencyType;
    private Button btnRunQueries;
    private Label lblAddQueryStatus;
    private Label lblDeleteQueryStatus;

    public MaintainQueriesDialog(EllieMae.EMLite.Campaign.Campaign campaign)
    {
      this.campaign = campaign;
      campaign.BeginEdit();
      this.InitializeComponent();
      this.cboFrequencyType.Items.Clear();
      this.cboFrequencyType.Items.AddRange((object[]) this.frequencyNames.GetNames());
      this.cboFrequencyType.SelectedItem = (object) this.frequencyNames.GetName((object) campaign.FrequencyType);
      this.nudFrequencyDays.Value = (Decimal) campaign.FrequencyInterval;
      this.populateControls();
    }

    private void populateControls()
    {
      this.btnEditAddQuery.Enabled = true;
      if ((CampaignType.AutoAddQuery & this.campaign.CampaignType) == CampaignType.AutoAddQuery)
      {
        this.lblAddQueryStatus.Text = "Defined";
        this.cbAutoAddQuery.Checked = true;
        this.cbAutoAddQuery.Enabled = true;
      }
      else if (this.campaign.AddQueryId != 0)
      {
        this.lblAddQueryStatus.Text = "Defined";
        this.cbAutoAddQuery.Checked = false;
        this.cbAutoAddQuery.Enabled = true;
      }
      else
      {
        this.cbAutoAddQuery.Checked = false;
        this.cbAutoAddQuery.Enabled = false;
      }
      this.btnEditDeleteQuery.Enabled = true;
      if ((CampaignType.AutoDeleteQuery & this.campaign.CampaignType) == CampaignType.AutoDeleteQuery)
      {
        this.lblDeleteQueryStatus.Text = "Defined";
        this.cbAutoDeleteQuery.Checked = true;
        this.cbAutoDeleteQuery.Enabled = true;
      }
      else if (this.campaign.DeleteQueryId != 0)
      {
        this.lblDeleteQueryStatus.Text = "Defined";
        this.cbAutoDeleteQuery.Checked = false;
        this.cbAutoDeleteQuery.Enabled = true;
      }
      else
      {
        this.cbAutoDeleteQuery.Checked = false;
        this.cbAutoDeleteQuery.Enabled = false;
      }
      this.cboFrequencyType.Enabled = this.cbAutoAddQuery.Enabled || this.cbAutoDeleteQuery.Enabled;
      this.btnRunQueries.Enabled = this.campaign.Status == CampaignStatus.Running && (this.cbAutoAddQuery.Checked || this.cbAutoDeleteQuery.Checked);
    }

    private void editAddQuery()
    {
      this.campaign.BeginEdit();
      if (DialogResult.OK == new CampaignQueryDialog(QueryEditMode.AddQuery, this.campaign).ShowDialog())
      {
        if (string.Empty == this.campaign.AddQuery.XmlQueryString)
        {
          this.campaign.CampaignType &= ~CampaignType.AutoAddQuery;
          if (this.campaign.CampaignType == (CampaignType) 0)
            this.campaign.CampaignType = CampaignType.Manual;
        }
        else
        {
          this.campaign.CampaignType |= CampaignType.AutoAddQuery;
          this.campaign.CampaignType &= ~CampaignType.Manual;
        }
        this.campaign.ApplyEdit();
        this.populateControls();
      }
      else
        this.campaign.CancelEdit();
    }

    private void editDeleteQuery()
    {
      this.campaign.BeginEdit();
      if (DialogResult.OK == new CampaignQueryDialog(QueryEditMode.DeleteQuery, this.campaign).ShowDialog())
      {
        if (string.Empty == this.campaign.DeleteQuery.XmlQueryString)
        {
          this.campaign.CampaignType &= ~CampaignType.AutoDeleteQuery;
          if (this.campaign.CampaignType == (CampaignType) 0)
            this.campaign.CampaignType = CampaignType.Manual;
        }
        else
        {
          this.campaign.CampaignType |= CampaignType.AutoDeleteQuery;
          this.campaign.CampaignType &= ~CampaignType.Manual;
        }
        this.campaign.ApplyEdit();
        this.populateControls();
      }
      else
        this.campaign.CancelEdit();
    }

    private void saveCampaign()
    {
      this.campaign.FrequencyType = (CampaignFrequencyType) this.frequencyNames.GetValue(this.cboFrequencyType.SelectedItem.ToString());
      this.campaign.FrequencyInterval = CampaignFrequencyType.Custom == this.campaign.FrequencyType ? (int) this.nudFrequencyDays.Value : 0;
      this.campaign.ApplyEdit();
      this.campaign.Save();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void cbAutoAddQuery_CheckedChanged(object sender, EventArgs e)
    {
      if (this.cbAutoAddQuery.Checked)
      {
        this.campaign.CampaignType |= CampaignType.AutoAddQuery;
        this.campaign.CampaignType &= ~CampaignType.Manual;
      }
      else
      {
        this.campaign.CampaignType &= ~CampaignType.AutoAddQuery;
        if (this.campaign.CampaignType == (CampaignType) 0)
          this.campaign.CampaignType = CampaignType.Manual;
      }
      this.btnRunQueries.Enabled = this.campaign.Status == CampaignStatus.Running && (this.cbAutoAddQuery.Checked || this.cbAutoDeleteQuery.Checked);
    }

    private void cbAutoDeleteQuery_CheckedChanged(object sender, EventArgs e)
    {
      if (this.cbAutoDeleteQuery.Checked)
      {
        this.campaign.CampaignType |= CampaignType.AutoDeleteQuery;
        this.campaign.CampaignType &= ~CampaignType.Manual;
      }
      else
      {
        this.campaign.CampaignType &= ~CampaignType.AutoDeleteQuery;
        if (this.campaign.CampaignType == (CampaignType) 0)
          this.campaign.CampaignType = CampaignType.Manual;
      }
      this.btnRunQueries.Enabled = this.campaign.Status == CampaignStatus.Running && (this.cbAutoAddQuery.Checked || this.cbAutoDeleteQuery.Checked);
    }

    private void btnRunQueries_Click(object sender, EventArgs e)
    {
      this.saveCampaign();
      this.campaign.RunQueries();
      this.DialogResult = DialogResult.OK;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.saveCampaign();
      this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.campaign.CancelEdit();
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnEditDeleteQuery_Click(object sender, EventArgs e) => this.editDeleteQuery();

    private void btnEditAddQuery_Click(object sender, EventArgs e) => this.editAddQuery();

    private void cboFrequencyType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (CampaignFrequencyType.Custom == (CampaignFrequencyType) this.frequencyNames.GetValue(this.cboFrequencyType.SelectedItem.ToString()))
      {
        this.nudFrequencyDays.Minimum = 1M;
        this.nudFrequencyDays.Visible = true;
        this.nudFrequencyDays.Enabled = true;
        this.lblDays.Visible = true;
      }
      else
      {
        this.nudFrequencyDays.Minimum = 0M;
        this.nudFrequencyDays.Value = 0M;
        this.nudFrequencyDays.Visible = false;
        this.lblDays.Visible = false;
      }
    }

    private void InitializeComponent()
    {
      this.pnlButtons = new Panel();
      this.btnRunQueries = new Button();
      this.lblSeparator = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.panel1 = new Panel();
      this.lblDays = new Label();
      this.nudFrequencyDays = new NumericUpDown();
      this.cboFrequencyType = new ComboBox();
      this.lblFrequencyType = new Label();
      this.label7 = new Label();
      this.groupBox2 = new GroupBox();
      this.lblDeleteQueryStatus = new Label();
      this.label4 = new Label();
      this.btnEditDeleteQuery = new Button();
      this.cbAutoDeleteQuery = new CheckBox();
      this.groupBox1 = new GroupBox();
      this.lblAddQueryStatus = new Label();
      this.label3 = new Label();
      this.btnEditAddQuery = new Button();
      this.cbAutoAddQuery = new CheckBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.pnlButtons.SuspendLayout();
      this.panel1.SuspendLayout();
      this.nudFrequencyDays.BeginInit();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.pnlButtons.Controls.Add((Control) this.btnRunQueries);
      this.pnlButtons.Controls.Add((Control) this.lblSeparator);
      this.pnlButtons.Controls.Add((Control) this.btnCancel);
      this.pnlButtons.Controls.Add((Control) this.btnOK);
      this.pnlButtons.Dock = DockStyle.Bottom;
      this.pnlButtons.Location = new Point(0, 287);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(372, 32);
      this.pnlButtons.TabIndex = 2;
      this.btnRunQueries.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnRunQueries.Location = new Point(84, 5);
      this.btnRunQueries.Name = "btnRunQueries";
      this.btnRunQueries.Size = new Size(104, 23);
      this.btnRunQueries.TabIndex = 3;
      this.btnRunQueries.Text = "Run Queries Now";
      this.btnRunQueries.Click += new EventHandler(this.btnRunQueries_Click);
      this.lblSeparator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblSeparator.Location = new Point(7, 0);
      this.lblSeparator.Name = "lblSeparator";
      this.lblSeparator.Size = new Size(358, 1);
      this.lblSeparator.TabIndex = 2;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(292, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(192, 5);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(96, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "Apply Changes";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.panel1.Controls.Add((Control) this.lblDays);
      this.panel1.Controls.Add((Control) this.nudFrequencyDays);
      this.panel1.Controls.Add((Control) this.cboFrequencyType);
      this.panel1.Controls.Add((Control) this.lblFrequencyType);
      this.panel1.Controls.Add((Control) this.label7);
      this.panel1.Controls.Add((Control) this.groupBox2);
      this.panel1.Controls.Add((Control) this.groupBox1);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(372, 287);
      this.panel1.TabIndex = 5;
      this.lblDays.Location = new Point(260, 248);
      this.lblDays.Name = "lblDays";
      this.lblDays.Size = new Size(36, 20);
      this.lblDays.TabIndex = 17;
      this.lblDays.Text = "days";
      this.lblDays.TextAlign = ContentAlignment.MiddleLeft;
      this.nudFrequencyDays.Location = new Point(208, 248);
      this.nudFrequencyDays.Name = "nudFrequencyDays";
      this.nudFrequencyDays.Size = new Size(48, 20);
      this.nudFrequencyDays.TabIndex = 16;
      this.cboFrequencyType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFrequencyType.Location = new Point(116, 248);
      this.cboFrequencyType.Name = "cboFrequencyType";
      this.cboFrequencyType.Size = new Size(84, 21);
      this.cboFrequencyType.TabIndex = 15;
      this.cboFrequencyType.SelectedIndexChanged += new EventHandler(this.cboFrequencyType_SelectedIndexChanged);
      this.lblFrequencyType.Location = new Point(20, 248);
      this.lblFrequencyType.Name = "lblFrequencyType";
      this.lblFrequencyType.Size = new Size(96, 20);
      this.lblFrequencyType.TabIndex = 14;
      this.lblFrequencyType.Text = "Query Frequency:";
      this.lblFrequencyType.TextAlign = ContentAlignment.MiddleLeft;
      this.label7.Location = new Point(20, 40);
      this.label7.Name = "label7";
      this.label7.Size = new Size(336, 20);
      this.label7.TabIndex = 13;
      this.label7.Text = "Set up your campaign to automatically add and remove contacts.";
      this.groupBox2.Controls.Add((Control) this.lblDeleteQueryStatus);
      this.groupBox2.Controls.Add((Control) this.label4);
      this.groupBox2.Controls.Add((Control) this.btnEditDeleteQuery);
      this.groupBox2.Controls.Add((Control) this.cbAutoDeleteQuery);
      this.groupBox2.Location = new Point(12, 152);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(344, 80);
      this.groupBox2.TabIndex = 12;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Remove Contacts";
      this.lblDeleteQueryStatus.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblDeleteQueryStatus.Location = new Point(80, 24);
      this.lblDeleteQueryStatus.Name = "lblDeleteQueryStatus";
      this.lblDeleteQueryStatus.Size = new Size(96, 16);
      this.lblDeleteQueryStatus.TabIndex = 11;
      this.lblDeleteQueryStatus.Text = "Not Defined";
      this.label4.Location = new Point(8, 24);
      this.label4.Name = "label4";
      this.label4.Size = new Size(76, 16);
      this.label4.TabIndex = 10;
      this.label4.Text = "Query Status:";
      this.btnEditDeleteQuery.Location = new Point(224, 20);
      this.btnEditDeleteQuery.Name = "btnEditDeleteQuery";
      this.btnEditDeleteQuery.Size = new Size(112, 24);
      this.btnEditDeleteQuery.TabIndex = 9;
      this.btnEditDeleteQuery.Text = "Edit Search Query";
      this.btnEditDeleteQuery.Click += new EventHandler(this.btnEditDeleteQuery_Click);
      this.cbAutoDeleteQuery.Location = new Point(8, 48);
      this.cbAutoDeleteQuery.Name = "cbAutoDeleteQuery";
      this.cbAutoDeleteQuery.Size = new Size(324, 24);
      this.cbAutoDeleteQuery.TabIndex = 6;
      this.cbAutoDeleteQuery.Text = "Remove contacts automatically using a search query.";
      this.cbAutoDeleteQuery.CheckedChanged += new EventHandler(this.cbAutoDeleteQuery_CheckedChanged);
      this.groupBox1.Controls.Add((Control) this.lblAddQueryStatus);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.btnEditAddQuery);
      this.groupBox1.Controls.Add((Control) this.cbAutoAddQuery);
      this.groupBox1.Location = new Point(12, 64);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(344, 80);
      this.groupBox1.TabIndex = 11;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Add Contacts";
      this.lblAddQueryStatus.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblAddQueryStatus.Location = new Point(80, 24);
      this.lblAddQueryStatus.Name = "lblAddQueryStatus";
      this.lblAddQueryStatus.Size = new Size(96, 16);
      this.lblAddQueryStatus.TabIndex = 10;
      this.lblAddQueryStatus.Text = "Not Defined";
      this.label3.Location = new Point(8, 24);
      this.label3.Name = "label3";
      this.label3.Size = new Size(76, 16);
      this.label3.TabIndex = 9;
      this.label3.Text = "Query Status:";
      this.btnEditAddQuery.Location = new Point(224, 20);
      this.btnEditAddQuery.Name = "btnEditAddQuery";
      this.btnEditAddQuery.Size = new Size(112, 24);
      this.btnEditAddQuery.TabIndex = 8;
      this.btnEditAddQuery.Text = "Edit Search Query";
      this.btnEditAddQuery.Click += new EventHandler(this.btnEditAddQuery_Click);
      this.cbAutoAddQuery.Location = new Point(8, 48);
      this.cbAutoAddQuery.Name = "cbAutoAddQuery";
      this.cbAutoAddQuery.Size = new Size(328, 24);
      this.cbAutoAddQuery.TabIndex = 5;
      this.cbAutoAddQuery.Text = "Add contacts automatically using a search query.";
      this.cbAutoAddQuery.CheckedChanged += new EventHandler(this.cbAutoAddQuery_CheckedChanged);
      this.label2.BorderStyle = BorderStyle.Fixed3D;
      this.label2.Location = new Point(12, 30);
      this.label2.Name = "label2";
      this.label2.Size = new Size(348, 2);
      this.label2.TabIndex = 10;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(246, 18);
      this.label1.TabIndex = 9;
      this.label1.Text = "Automatic Search Queries";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(372, 319);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.pnlButtons);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MaintainQueriesDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Campaign Search Queries";
      this.KeyUp += new KeyEventHandler(this.MaintainQueriesDialog_KeyUp);
      this.pnlButtons.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.nudFrequencyDays.EndInit();
      this.groupBox2.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void MaintainQueriesDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }
  }
}
