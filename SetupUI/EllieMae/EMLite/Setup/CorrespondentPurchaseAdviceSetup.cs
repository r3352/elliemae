// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CorrespondentPurchaseAdviceSetup
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CorrespondentPurchaseAdviceSetup : SettingsUserControl
  {
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer gcWorksheetTabSettings;
    private GroupContainer gcEnableWorksheetTab;
    private CheckBox chkEnablePaymentHistoryTab;
    private CheckBox chkEnableEscrowDetailsTab;
    private TextBox txtCutOffCalendar;
    private TextBox txtNumberOfMonths;
    private ComboBox cmbDaysPerYear;
    private ComboBox cmbRounding;
    private Label label1;
    private Label label2;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private Label label10;
    private Label label11;
    private Label label3;

    public CorrespondentPurchaseAdviceSetup(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance, false)
    {
    }

    public CorrespondentPurchaseAdviceSetup(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.LoadDefaultSettings();
      this.setDirtyFlag(false);
    }

    private void LoadDefaultSettings()
    {
      this.chkEnablePaymentHistoryTab.Checked = (bool) this.session.ServerManager.GetServerSetting("Policies.EnablePaymentHistoryAndCalc");
      if (this.chkEnablePaymentHistoryTab.Checked)
      {
        this.chkEnableEscrowDetailsTab.Checked = (bool) this.session.ServerManager.GetServerSetting("Policies.EnableEscrowDetailsAndCalc");
      }
      else
      {
        this.chkEnableEscrowDetailsTab.Enabled = false;
        this.chkEnableEscrowDetailsTab.Checked = false;
      }
      this.txtCutOffCalendar.Text = this.session.ConfigurationManager.GetCompanySetting("Policies", "CutoffCalendarDay");
      this.txtNumberOfMonths.Text = this.session.ConfigurationManager.GetCompanySetting("Policies", "NumberOfMonths");
      this.cmbDaysPerYear.SelectedItem = (object) this.session.ConfigurationManager.GetCompanySetting("Policies", "PerDiemInterestDaysPerYear");
      this.cmbRounding.SelectedItem = (object) this.session.ConfigurationManager.GetCompanySetting("Policies", "PerDiemInterestRounding");
    }

    public override void Reset()
    {
      this.LoadDefaultSettings();
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      using (CursorActivator.Wait())
      {
        if (this.txtNumberOfMonths.Text.Trim() == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Number of Months is required.");
          this.txtNumberOfMonths.Focus();
          return;
        }
        if (this.txtCutOffCalendar.Text.Trim() == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Cutoff Calendar Day is required.");
          this.txtCutOffCalendar.Focus();
          return;
        }
        if (Convert.ToInt32(this.txtCutOffCalendar.Text) < 1 || Convert.ToInt32(this.txtCutOffCalendar.Text) > 28)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Cutoff Calendar Day should be between 1 and 28.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.txtCutOffCalendar.Text = "";
          this.txtCutOffCalendar.Focus();
          return;
        }
        if (Convert.ToInt32(this.txtNumberOfMonths.Text) < 6 || Convert.ToInt32(this.txtNumberOfMonths.Text) > 24)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Number of Months should be between 6 and 24.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.txtNumberOfMonths.Text = "";
          this.txtNumberOfMonths.Focus();
          return;
        }
        Session.ServerManager.UpdateServerSettings((IDictionary) new Dictionary<string, object>()
        {
          {
            "Policies.EnablePaymentHistoryAndCalc",
            (object) this.chkEnablePaymentHistoryTab.Checked
          },
          {
            "Policies.EnableEscrowDetailsAndCalc",
            (object) this.chkEnableEscrowDetailsTab.Checked
          },
          {
            "Policies.CutoffCalendarDay",
            (object) this.txtCutOffCalendar.Text
          },
          {
            "Policies.NumberOfMonths",
            (object) this.txtNumberOfMonths.Text
          },
          {
            "Policies.PerDiemInterestDaysPerYear",
            (object) this.cmbDaysPerYear.SelectedItem.ToString().Trim()
          },
          {
            "Policies.PerDiemInterestRounding",
            (object) this.cmbRounding.SelectedItem.ToString().Trim()
          }
        }, true, false);
        this.Reset();
      }
      this.setDirtyFlag(false);
    }

    private void txtCutOffCalendar_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar) || e.KeyChar.Equals((object) Keys.Space))
      {
        this.setDirtyFlag(true);
      }
      else
      {
        if (!char.IsNumber(e.KeyChar))
          e.Handled = true;
        this.setDirtyFlag(true);
      }
    }

    private void txtNumberOfMonths_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar) || e.KeyChar.Equals((object) Keys.Space))
      {
        this.setDirtyFlag(true);
      }
      else
      {
        if (!char.IsNumber(e.KeyChar))
          e.Handled = true;
        this.setDirtyFlag(true);
      }
    }

    private void chkEnablePaymentHistoryTab_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkEnablePaymentHistoryTab.Checked)
      {
        this.chkEnableEscrowDetailsTab.Enabled = true;
      }
      else
      {
        this.chkEnableEscrowDetailsTab.Enabled = false;
        this.chkEnableEscrowDetailsTab.Checked = false;
      }
      this.setDirtyFlag(true);
    }

    private void chkEnableEscrowDetailsTab_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void cmbDaysPerYear_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void cmbRounding_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcWorksheetTabSettings = new GroupContainer();
      this.gcEnableWorksheetTab = new GroupContainer();
      this.label3 = new Label();
      this.label8 = new Label();
      this.cmbRounding = new ComboBox();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label11 = new Label();
      this.txtNumberOfMonths = new TextBox();
      this.chkEnablePaymentHistoryTab = new CheckBox();
      this.chkEnableEscrowDetailsTab = new CheckBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.txtCutOffCalendar = new TextBox();
      this.cmbDaysPerYear = new ComboBox();
      this.gcWorksheetTabSettings.SuspendLayout();
      this.gcEnableWorksheetTab.SuspendLayout();
      this.SuspendLayout();
      this.gcWorksheetTabSettings.Controls.Add((Control) this.gcEnableWorksheetTab);
      this.gcWorksheetTabSettings.Dock = DockStyle.Top;
      this.gcWorksheetTabSettings.HeaderForeColor = SystemColors.ControlText;
      this.gcWorksheetTabSettings.Location = new Point(0, 0);
      this.gcWorksheetTabSettings.Name = "gcWorksheetTabSettings";
      this.gcWorksheetTabSettings.Size = new Size(1008, 325);
      this.gcWorksheetTabSettings.TabIndex = 2;
      this.gcWorksheetTabSettings.Text = "Correspondent Purchase Advice - Worksheet Tab Settings";
      this.gcEnableWorksheetTab.Controls.Add((Control) this.label3);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.label8);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.cmbRounding);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.label7);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.label6);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.label5);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.label9);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.label10);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.label11);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.txtNumberOfMonths);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.chkEnablePaymentHistoryTab);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.chkEnableEscrowDetailsTab);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.label1);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.label2);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.txtCutOffCalendar);
      this.gcEnableWorksheetTab.Controls.Add((Control) this.cmbDaysPerYear);
      this.gcEnableWorksheetTab.Dock = DockStyle.Top;
      this.gcEnableWorksheetTab.HeaderForeColor = SystemColors.ControlText;
      this.gcEnableWorksheetTab.Location = new Point(1, 26);
      this.gcEnableWorksheetTab.Name = "gcEnableWorksheetTab";
      this.gcEnableWorksheetTab.Size = new Size(1006, 300);
      this.gcEnableWorksheetTab.TabIndex = 3;
      this.gcEnableWorksheetTab.Text = "Enable worksheet tabs, set key values for calculations.";
      this.label3.Font = new Font("Microsoft Sans Serif", 9.25f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(518, 116);
      this.label3.Name = "label3";
      this.label3.Size = new Size(58, 23);
      this.label3.TabIndex = 34;
      this.label3.Text = "that day.";
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(188, 229);
      this.label8.Name = "label8";
      this.label8.Size = new Size(171, 13);
      this.label8.TabIndex = 30;
      this.label8.Text = "Per Diem Interest - Rounding";
      this.cmbRounding.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRounding.Font = new Font("Microsoft Sans Serif", 9.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.cmbRounding.FormattingEnabled = true;
      this.cmbRounding.Items.AddRange(new object[3]
      {
        (object) "2 Decimals",
        (object) "4 Decimals",
        (object) "No Rounding"
      });
      this.cmbRounding.Location = new Point(47, 226);
      this.cmbRounding.Name = "cmbRounding";
      this.cmbRounding.Size = new Size(126, 23);
      this.cmbRounding.TabIndex = 29;
      this.cmbRounding.SelectedIndexChanged += new EventHandler(this.cmbRounding_SelectedIndexChanged);
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(123, 186);
      this.label7.Name = "label7";
      this.label7.Size = new Size(195, 13);
      this.label7.TabIndex = 28;
      this.label7.Text = "Per Diem Interest - Days per year";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(393, 144);
      this.label6.Name = "label6";
      this.label6.Size = new Size(126, 15);
      this.label6.TabIndex = 24;
      this.label6.Text = "(24 month maximum)";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(92, 146);
      this.label5.Name = "label5";
      this.label5.Size = new Size(301, 13);
      this.label5.TabIndex = 23;
      this.label5.Text = "Number of Months - Payment Amortization to display";
      this.label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
      this.label9.Location = new Point(341, 117);
      this.label9.Name = "label9";
      this.label9.Size = new Size(44, 23);
      this.label9.TabIndex = 31;
      this.label9.Text = "before";
      this.label10.Font = new Font("Microsoft Sans Serif", 9.25f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(387, 116);
      this.label10.Name = "label10";
      this.label10.Size = new Size(70, 23);
      this.label10.TabIndex = 32;
      this.label10.Text = "that day or";
      this.label11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
      this.label11.Location = new Point(461, 117);
      this.label11.Name = "label11";
      this.label11.Size = new Size(55, 23);
      this.label11.TabIndex = 33;
      this.label11.Text = "on/after";
      this.txtNumberOfMonths.Font = new Font("Microsoft Sans Serif", 9.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.txtNumberOfMonths.Location = new Point(47, 144);
      this.txtNumberOfMonths.MaxLength = 2;
      this.txtNumberOfMonths.Name = "txtNumberOfMonths";
      this.txtNumberOfMonths.Size = new Size(39, 21);
      this.txtNumberOfMonths.TabIndex = 22;
      this.txtNumberOfMonths.TextAlign = HorizontalAlignment.Center;
      this.txtNumberOfMonths.KeyPress += new KeyPressEventHandler(this.txtNumberOfMonths_KeyPress);
      this.chkEnablePaymentHistoryTab.AutoSize = true;
      this.chkEnablePaymentHistoryTab.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkEnablePaymentHistoryTab.Location = new Point(30, 35);
      this.chkEnablePaymentHistoryTab.Name = "chkEnablePaymentHistoryTab";
      this.chkEnablePaymentHistoryTab.Size = new Size(279, 17);
      this.chkEnablePaymentHistoryTab.TabIndex = 5;
      this.chkEnablePaymentHistoryTab.Text = "Enable Payment History tab and calculations";
      this.chkEnablePaymentHistoryTab.UseVisualStyleBackColor = true;
      this.chkEnablePaymentHistoryTab.CheckedChanged += new EventHandler(this.chkEnablePaymentHistoryTab_CheckedChanged);
      this.chkEnableEscrowDetailsTab.AutoSize = true;
      this.chkEnableEscrowDetailsTab.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkEnableEscrowDetailsTab.Location = new Point(30, 58);
      this.chkEnableEscrowDetailsTab.Name = "chkEnableEscrowDetailsTab";
      this.chkEnableEscrowDetailsTab.Size = new Size(272, 17);
      this.chkEnableEscrowDetailsTab.TabIndex = 4;
      this.chkEnableEscrowDetailsTab.Text = "Enable Escrow Details tab and calculations";
      this.chkEnableEscrowDetailsTab.UseVisualStyleBackColor = true;
      this.chkEnableEscrowDetailsTab.CheckedChanged += new EventHandler(this.chkEnableEscrowDetailsTab_CheckedChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(92, 103);
      this.label1.Name = "label1";
      this.label1.Size = new Size(228, 13);
      this.label1.TabIndex = 17;
      this.label1.Text = "Cutoff Calendar Day - day of the month";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9.25f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(92, 116);
      this.label2.Name = "label2";
      this.label2.Size = new Size(247, 16);
      this.label2.TabIndex = 19;
      this.label2.Text = "Calculations will use dates that are either";
      this.txtCutOffCalendar.Font = new Font("Microsoft Sans Serif", 9.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.txtCutOffCalendar.Location = new Point(47, 100);
      this.txtCutOffCalendar.MaxLength = 2;
      this.txtCutOffCalendar.Name = "txtCutOffCalendar";
      this.txtCutOffCalendar.Size = new Size(39, 21);
      this.txtCutOffCalendar.TabIndex = 21;
      this.txtCutOffCalendar.TextAlign = HorizontalAlignment.Center;
      this.txtCutOffCalendar.KeyPress += new KeyPressEventHandler(this.txtCutOffCalendar_KeyPress);
      this.cmbDaysPerYear.DisplayMember = "(none)";
      this.cmbDaysPerYear.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbDaysPerYear.Font = new Font("Microsoft Sans Serif", 9.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.cmbDaysPerYear.FormattingEnabled = true;
      this.cmbDaysPerYear.Items.AddRange(new object[3]
      {
        (object) "365",
        (object) "364",
        (object) "360"
      });
      this.cmbDaysPerYear.Location = new Point(47, 183);
      this.cmbDaysPerYear.Name = "cmbDaysPerYear";
      this.cmbDaysPerYear.Size = new Size(60, 23);
      this.cmbDaysPerYear.TabIndex = 27;
      this.cmbDaysPerYear.SelectedIndexChanged += new EventHandler(this.cmbDaysPerYear_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.gcWorksheetTabSettings);
      this.Name = nameof (CorrespondentPurchaseAdviceSetup);
      this.Size = new Size(1008, 1000);
      this.gcWorksheetTabSettings.ResumeLayout(false);
      this.gcEnableWorksheetTab.ResumeLayout(false);
      this.gcEnableWorksheetTab.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
