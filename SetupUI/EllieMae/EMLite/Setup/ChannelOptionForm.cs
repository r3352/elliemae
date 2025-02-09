// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ChannelOptionForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ChannelOptionForm : SettingsUserControl
  {
    private CheckBox[] listChannel;
    private NMLSReportSettings nmlsSettings;
    private IContainer components;
    private GroupContainer groupContainer1;
    private CheckBox chkBoxCorrespondent;
    private CheckBox chkBoxBrokered;
    private CheckBox chkBoxBankedWholesale;
    private CheckBox chkBoxBankedRetail;
    private ComboBox cboNMLSCorrespondent;
    private Label label1;
    private Label label2;
    private Label label3;
    private ComboBox cboNMLSNoChannel;
    private GradientPanel gradientPanel1;
    private Label label4;
    private CheckBox chkBoxDisableCorrespondentChannelEmailPopup;

    public ChannelOptionForm(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.listChannel = new CheckBox[4];
      this.listChannel[0] = this.chkBoxBankedRetail;
      this.listChannel[1] = this.chkBoxBankedWholesale;
      this.listChannel[2] = this.chkBoxBrokered;
      this.listChannel[3] = this.chkBoxCorrespondent;
      this.chkBoxDisableCorrespondentChannelEmailPopup.CheckedChanged += new EventHandler(this.listChannel_CheckChanged);
      foreach (CheckBox checkBox in this.listChannel)
        checkBox.CheckedChanged += new EventHandler(this.listChannel_CheckChanged);
      this.nmlsSettings = new NMLSReportSettings(Session.SessionObjects);
      this.Reset();
    }

    public override void Save()
    {
      int num1 = 0;
      foreach (CheckBox checkBox in this.listChannel)
      {
        if (checkBox.Checked)
          ++num1;
      }
      if (num1 == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to display at least one option.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string empty = string.Empty;
        for (int index = 0; index < this.listChannel.Length; ++index)
        {
          if (this.listChannel[index].Checked)
          {
            if (empty != string.Empty)
              empty += ",";
            empty += index.ToString();
          }
        }
        Session.ConfigurationManager.SetCompanySetting("CHANNELOPTION", "DISPLAY", empty);
        Session.ConfigurationManager.SetCompanySetting("Policies", "DisableEmailPopup", this.chkBoxDisableCorrespondentChannelEmailPopup.Checked.ToString());
        this.nmlsSettings.SetChannelMapping("Correspondent", this.cboNMLSCorrespondent.SelectedItem.ToString());
        this.nmlsSettings.SetChannelMapping("", this.cboNMLSNoChannel.SelectedItem.ToString());
        this.setDirtyFlag(false);
      }
    }

    public override void Reset()
    {
      for (int index = 0; index < this.listChannel.Length; ++index)
        this.listChannel[index].Checked = false;
      string companySetting = Session.ConfigurationManager.GetCompanySetting("CHANNELOPTION", "DISPLAY");
      if ((companySetting ?? "") != string.Empty)
      {
        string str = companySetting;
        char[] chArray = new char[1]{ ',' };
        foreach (object obj in str.Split(chArray))
        {
          int index = Utils.ParseInt(obj);
          if (index + 1 <= this.listChannel.Length)
            this.listChannel[index].Checked = true;
        }
      }
      else
      {
        for (int index = 0; index < this.listChannel.Length; ++index)
          this.listChannel[index].Checked = true;
      }
      bool result;
      bool.TryParse(Session.ConfigurationManager.GetCompanySetting("Policies", "DisableEmailPopup"), out result);
      this.chkBoxDisableCorrespondentChannelEmailPopup.Checked = result;
      this.chkBoxDisableCorrespondentChannelEmailPopup.Enabled = this.chkBoxCorrespondent.Checked;
      this.cboNMLSCorrespondent.SelectedItem = (object) this.nmlsSettings.GetChannelMapping("Correspondent");
      this.cboNMLSNoChannel.SelectedItem = (object) this.nmlsSettings.GetChannelMapping("");
      this.setDirtyFlag(false);
    }

    private void listChannel_CheckChanged(object sender, EventArgs e)
    {
      if (sender is CheckBox checkBox && checkBox.Name == "chkBoxCorrespondent")
      {
        this.chkBoxDisableCorrespondentChannelEmailPopup.Enabled = this.chkBoxCorrespondent.Checked;
        if (!this.chkBoxDisableCorrespondentChannelEmailPopup.Enabled)
          this.chkBoxDisableCorrespondentChannelEmailPopup.Checked = false;
      }
      this.setDirtyFlag(true);
    }

    private void onChannelOptionChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.chkBoxDisableCorrespondentChannelEmailPopup = new CheckBox();
      this.label4 = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.label3 = new Label();
      this.cboNMLSNoChannel = new ComboBox();
      this.label2 = new Label();
      this.cboNMLSCorrespondent = new ComboBox();
      this.chkBoxCorrespondent = new CheckBox();
      this.chkBoxBrokered = new CheckBox();
      this.chkBoxBankedWholesale = new CheckBox();
      this.chkBoxBankedRetail = new CheckBox();
      this.groupContainer1.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.chkBoxDisableCorrespondentChannelEmailPopup);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.gradientPanel1);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.cboNMLSNoChannel);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.cboNMLSCorrespondent);
      this.groupContainer1.Controls.Add((Control) this.chkBoxCorrespondent);
      this.groupContainer1.Controls.Add((Control) this.chkBoxBrokered);
      this.groupContainer1.Controls.Add((Control) this.chkBoxBankedWholesale);
      this.groupContainer1.Controls.Add((Control) this.chkBoxBankedRetail);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(675, 491);
      this.groupContainer1.TabIndex = 14;
      this.groupContainer1.Text = "Channel Options";
      this.chkBoxDisableCorrespondentChannelEmailPopup.AutoSize = true;
      this.chkBoxDisableCorrespondentChannelEmailPopup.Location = new Point(32, 135);
      this.chkBoxDisableCorrespondentChannelEmailPopup.Name = "chkBoxDisableCorrespondentChannelEmailPopup";
      this.chkBoxDisableCorrespondentChannelEmailPopup.Size = new Size(153, 18);
      this.chkBoxDisableCorrespondentChannelEmailPopup.TabIndex = 4;
      this.chkBoxDisableCorrespondentChannelEmailPopup.Text = "Disable email check popup";
      this.chkBoxDisableCorrespondentChannelEmailPopup.UseVisualStyleBackColor = true;
      this.label4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label4.AutoEllipsis = true;
      this.label4.Location = new Point(9, 208);
      this.label4.Name = "label4";
      this.label4.Size = new Size(653, 18);
      this.label4.TabIndex = 10;
      this.label4.Text = "Configure how Correspondent loans and loans with no channel selection are treated when generating NMLS Call Reports.";
      this.gradientPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Location = new Point(1, 166);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(673, 28);
      this.gradientPanel1.TabIndex = 9;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(8, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(139, 14);
      this.label1.TabIndex = 4;
      this.label1.Text = "NMLS Channels Options";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 261);
      this.label3.Name = "label3";
      this.label3.Size = new Size(247, 14);
      this.label3.TabIndex = 8;
      this.label3.Text = "Loans with no channel selected will be treated as";
      this.cboNMLSNoChannel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboNMLSNoChannel.FormattingEnabled = true;
      this.cboNMLSNoChannel.Items.AddRange(new object[3]
      {
        (object) "Banked - Retail",
        (object) "Banked - Wholesale",
        (object) "Brokered"
      });
      this.cboNMLSNoChannel.Location = new Point(265, 257);
      this.cboNMLSNoChannel.Name = "cboNMLSNoChannel";
      this.cboNMLSNoChannel.Size = new Size(121, 22);
      this.cboNMLSNoChannel.TabIndex = 7;
      this.cboNMLSNoChannel.SelectedIndexChanged += new EventHandler(this.onChannelOptionChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 237);
      this.label2.Name = "label2";
      this.label2.Size = new Size(254, 14);
      this.label2.TabIndex = 6;
      this.label2.Text = "Loans marked as Correspondent will be treated as ";
      this.cboNMLSCorrespondent.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboNMLSCorrespondent.FormattingEnabled = true;
      this.cboNMLSCorrespondent.Items.AddRange(new object[3]
      {
        (object) "Banked - Retail",
        (object) "Banked - Wholesale",
        (object) "Brokered"
      });
      this.cboNMLSCorrespondent.Location = new Point(265, 233);
      this.cboNMLSCorrespondent.Name = "cboNMLSCorrespondent";
      this.cboNMLSCorrespondent.Size = new Size(121, 22);
      this.cboNMLSCorrespondent.TabIndex = 5;
      this.cboNMLSCorrespondent.SelectedIndexChanged += new EventHandler(this.onChannelOptionChanged);
      this.chkBoxCorrespondent.AutoSize = true;
      this.chkBoxCorrespondent.Location = new Point(11, 109);
      this.chkBoxCorrespondent.Name = "chkBoxCorrespondent";
      this.chkBoxCorrespondent.Size = new Size(98, 18);
      this.chkBoxCorrespondent.TabIndex = 3;
      this.chkBoxCorrespondent.Text = "Correspondent";
      this.chkBoxCorrespondent.UseVisualStyleBackColor = true;
      this.chkBoxBrokered.AutoSize = true;
      this.chkBoxBrokered.Location = new Point(11, 84);
      this.chkBoxBrokered.Name = "chkBoxBrokered";
      this.chkBoxBrokered.Size = new Size(70, 18);
      this.chkBoxBrokered.TabIndex = 2;
      this.chkBoxBrokered.Text = "Brokered";
      this.chkBoxBrokered.UseVisualStyleBackColor = true;
      this.chkBoxBankedWholesale.AutoSize = true;
      this.chkBoxBankedWholesale.Location = new Point(11, 58);
      this.chkBoxBankedWholesale.Name = "chkBoxBankedWholesale";
      this.chkBoxBankedWholesale.Size = new Size(122, 18);
      this.chkBoxBankedWholesale.TabIndex = 1;
      this.chkBoxBankedWholesale.Text = "Banked - Wholesale";
      this.chkBoxBankedWholesale.UseVisualStyleBackColor = true;
      this.chkBoxBankedRetail.AutoSize = true;
      this.chkBoxBankedRetail.Location = new Point(11, 33);
      this.chkBoxBankedRetail.Name = "chkBoxBankedRetail";
      this.chkBoxBankedRetail.Size = new Size(98, 18);
      this.chkBoxBankedRetail.TabIndex = 0;
      this.chkBoxBankedRetail.Text = "Banked - Retail";
      this.chkBoxBankedRetail.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (ChannelOptionForm);
      this.Size = new Size(675, 491);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
