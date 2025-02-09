// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MSWorksheet.GlobalSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.MSWorksheet
{
  public class GlobalSettings : Form
  {
    private Sessions.Session session;
    private const string className = "GlobalSettings";
    private bool suspendEvent;
    private IContainer components;
    private Panel panel1;
    private Label label1;
    private CheckBox chkShowNonMatching;
    private BorderPanel pnlTempSettings;
    private CheckBox chkConditions;
    private GradientPanel gradientPanel1;
    private Label label2;
    private CheckBox chkManual;
    private RadioButton radCompliance;
    private Label label4;
    private RadioButton radCalendar;
    private RadioButton radBusiness;
    private GradientPanel gradientPanel3;
    private Label label5;
    private EMHelpLink emHelpLink1;
    private Button btnSave;
    private Button btnCancel;
    private Panel panel2;
    private BorderPanel borderPanel2;
    private CheckBox chkEmailNotification;
    private GradientPanel gradientPanel5;
    private Label label6;
    private Panel panel3;
    private GradientPanel gradientPanel4;

    public GlobalSettings(Sessions.Session session)
    {
      this.suspendEvent = true;
      this.InitializeComponent();
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
      switch ((AutoDayCountSetting) this.session.ServerManager.GetServerSetting("Policies.MilestoneExpDayCount"))
      {
        case AutoDayCountSetting.CalendarDays:
          this.radCalendar.Checked = true;
          break;
        case AutoDayCountSetting.BusinessDays:
          this.radBusiness.Checked = true;
          break;
        default:
          this.radCompliance.Checked = true;
          break;
      }
      if (this.session.EncompassEdition == EncompassEdition.Broker)
      {
        this.chkConditions.Enabled = this.chkManual.Enabled = this.chkEmailNotification.Enabled = this.chkShowNonMatching.Enabled = false;
      }
      else
      {
        this.setTemplateSettings((MilestoneTemplatesSetting) this.session.ServerManager.GetServerSetting("Policies.MilestoneTemplateSettings"));
        this.chkShowNonMatching.Checked = (bool) this.session.ServerManager.GetServerSetting("Policies.ShowNonMatchingMilestoneTemplate");
        this.chkEmailNotification.Checked = string.IsNullOrWhiteSpace(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "MilestoneTemplateChangeNotification")) ? (bool) this.session.ServerManager.GetServerSetting("Policies.MilestoneTemplateChangeNotification") : bool.Parse(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "MilestoneTemplateChangeNotification"));
        this.suspendEvent = false;
      }
    }

    private void setTemplateSettings(MilestoneTemplatesSetting setting)
    {
      switch (setting)
      {
        case MilestoneTemplatesSetting.Manual:
          this.chkManual.Checked = true;
          this.chkConditions.Checked = false;
          break;
        case MilestoneTemplatesSetting.Automatic:
          this.chkManual.Checked = false;
          this.chkConditions.Checked = true;
          break;
        case MilestoneTemplatesSetting.Both:
          this.chkManual.Checked = this.chkConditions.Checked = true;
          break;
        default:
          this.chkManual.Checked = false;
          this.chkConditions.Checked = false;
          break;
      }
    }

    private AutoDayCountSetting getSelectedMilestoneExpDayCount()
    {
      if (this.radBusiness.Checked)
        return AutoDayCountSetting.BusinessDays;
      return this.radCalendar.Checked ? AutoDayCountSetting.CalendarDays : AutoDayCountSetting.CompanyDays;
    }

    private MilestoneTemplatesSetting getMilestoneTemplatesSetting()
    {
      if (this.chkManual.Checked && this.chkConditions.Checked)
        return MilestoneTemplatesSetting.Both;
      if (this.chkManual.Checked)
        return MilestoneTemplatesSetting.Manual;
      return this.chkConditions.Checked ? MilestoneTemplatesSetting.Automatic : MilestoneTemplatesSetting.None;
    }

    private void saveSettings()
    {
      this.session.ServerManager.UpdateServerSetting("Policies.MilestoneExpDayCount", (object) this.getSelectedMilestoneExpDayCount());
      this.session.StartupInfo.PolicySettings[(object) "Policies.MilestoneExpDayCount"] = (object) this.getSelectedMilestoneExpDayCount();
      this.session.ServerManager.UpdateServerSetting("Policies.MilestoneTemplateSettings", (object) this.getMilestoneTemplatesSetting());
      this.session.StartupInfo.PolicySettings[(object) "Policies.MilestoneTemplateSettings"] = (object) this.getMilestoneTemplatesSetting();
      this.session.ServerManager.UpdateServerSetting("Policies.ShowNonMatchingMilestoneTemplate", (object) this.chkShowNonMatching.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.ShowNonMatchingMilestoneTemplate"] = (object) this.chkShowNonMatching.Checked;
      this.session.ServerManager.UpdateServerSetting("Policies.MilestoneTemplateChangeNotification", (object) this.chkEmailNotification.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.MilestoneTemplateChangeNotification"] = (object) this.chkEmailNotification.Checked;
    }

    private void GlobalSettings_CheckedChanged(object sender, EventArgs e)
    {
      this.chkShowNonMatching.Enabled = this.chkManual.Checked;
      this.setDirtyFlag(true);
    }

    private void setDirtyFlag(bool val)
    {
      if (this.suspendEvent)
        return;
      this.btnSave.Enabled = val;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.saveSettings();
      this.setDirtyFlag(false);
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (GlobalSettings));
      this.panel1 = new Panel();
      this.panel3 = new Panel();
      this.gradientPanel3 = new GradientPanel();
      this.label5 = new Label();
      this.radCompliance = new RadioButton();
      this.radCalendar = new RadioButton();
      this.label4 = new Label();
      this.radBusiness = new RadioButton();
      this.panel2 = new Panel();
      this.borderPanel2 = new BorderPanel();
      this.chkEmailNotification = new CheckBox();
      this.gradientPanel5 = new GradientPanel();
      this.label6 = new Label();
      this.pnlTempSettings = new BorderPanel();
      this.chkConditions = new CheckBox();
      this.chkShowNonMatching = new CheckBox();
      this.gradientPanel1 = new GradientPanel();
      this.label2 = new Label();
      this.chkManual = new CheckBox();
      this.gradientPanel4 = new GradientPanel();
      this.label1 = new Label();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.panel1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.panel2.SuspendLayout();
      this.borderPanel2.SuspendLayout();
      this.gradientPanel5.SuspendLayout();
      this.pnlTempSettings.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.gradientPanel4.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.panel3);
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Controls.Add((Control) this.pnlTempSettings);
      this.panel1.Controls.Add((Control) this.gradientPanel4);
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(695, 310);
      this.panel1.TabIndex = 0;
      this.panel3.Controls.Add((Control) this.gradientPanel3);
      this.panel3.Controls.Add((Control) this.radCompliance);
      this.panel3.Controls.Add((Control) this.radCalendar);
      this.panel3.Controls.Add((Control) this.label4);
      this.panel3.Controls.Add((Control) this.radBusiness);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 239);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(693, 69);
      this.panel3.TabIndex = 55;
      this.gradientPanel3.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanel3.Controls.Add((Control) this.label5);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(0, 0);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(693, 32);
      this.gradientPanel3.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel3.TabIndex = 53;
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(12, 8);
      this.label5.Name = "label5";
      this.label5.Size = new Size(91, 14);
      this.label5.TabIndex = 0;
      this.label5.Text = "Milestone Days";
      this.radCompliance.AutoSize = true;
      this.radCompliance.Location = new Point(273, 45);
      this.radCompliance.Name = "radCompliance";
      this.radCompliance.Size = new Size(114, 17);
      this.radCompliance.TabIndex = 11;
      this.radCompliance.TabStop = true;
      this.radCompliance.Text = "Company Calendar";
      this.radCompliance.UseVisualStyleBackColor = true;
      this.radCompliance.CheckedChanged += new EventHandler(this.GlobalSettings_CheckedChanged);
      this.radCalendar.AutoSize = true;
      this.radCalendar.Location = new Point(173, 45);
      this.radCalendar.Name = "radCalendar";
      this.radCalendar.Size = new Size(94, 17);
      this.radCalendar.TabIndex = 10;
      this.radCalendar.TabStop = true;
      this.radCalendar.Text = "Calendar Days";
      this.radCalendar.UseVisualStyleBackColor = true;
      this.radCalendar.CheckedChanged += new EventHandler(this.GlobalSettings_CheckedChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 47);
      this.label4.Name = "label4";
      this.label4.Size = new Size(77, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Days to Count:";
      this.radBusiness.AutoSize = true;
      this.radBusiness.Location = new Point(90, 45);
      this.radBusiness.Name = "radBusiness";
      this.radBusiness.Size = new Size(81, 17);
      this.radBusiness.TabIndex = 9;
      this.radBusiness.TabStop = true;
      this.radBusiness.Text = "Week Days";
      this.radBusiness.UseVisualStyleBackColor = true;
      this.radBusiness.CheckedChanged += new EventHandler(this.GlobalSettings_CheckedChanged);
      this.panel2.Controls.Add((Control) this.borderPanel2);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(0, 163);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(693, 76);
      this.panel2.TabIndex = 56;
      this.borderPanel2.Controls.Add((Control) this.chkEmailNotification);
      this.borderPanel2.Controls.Add((Control) this.gradientPanel5);
      this.borderPanel2.Dock = DockStyle.Fill;
      this.borderPanel2.Location = new Point(0, 0);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(693, 76);
      this.borderPanel2.TabIndex = 1;
      this.chkEmailNotification.AutoSize = true;
      this.chkEmailNotification.Location = new Point(14, 44);
      this.chkEmailNotification.Name = "chkEmailNotification";
      this.chkEmailNotification.Size = new Size(256, 17);
      this.chkEmailNotification.TabIndex = 46;
      this.chkEmailNotification.Text = "Notify user when no longer assigned to milestone";
      this.chkEmailNotification.UseVisualStyleBackColor = true;
      this.chkEmailNotification.CheckedChanged += new EventHandler(this.GlobalSettings_CheckedChanged);
      this.gradientPanel5.Borders = AnchorStyles.Bottom;
      this.gradientPanel5.Controls.Add((Control) this.label6);
      this.gradientPanel5.Dock = DockStyle.Top;
      this.gradientPanel5.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel5.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel5.Location = new Point(1, 1);
      this.gradientPanel5.Name = "gradientPanel5";
      this.gradientPanel5.Size = new Size(691, 31);
      this.gradientPanel5.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel5.TabIndex = 1;
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(10, 9);
      this.label6.Name = "label6";
      this.label6.Size = new Size(172, 14);
      this.label6.TabIndex = 0;
      this.label6.Text = "Email Notification Preferences";
      this.pnlTempSettings.Borders = AnchorStyles.Right;
      this.pnlTempSettings.Controls.Add((Control) this.chkConditions);
      this.pnlTempSettings.Controls.Add((Control) this.chkShowNonMatching);
      this.pnlTempSettings.Controls.Add((Control) this.gradientPanel1);
      this.pnlTempSettings.Controls.Add((Control) this.chkManual);
      this.pnlTempSettings.Dock = DockStyle.Top;
      this.pnlTempSettings.Location = new Point(0, 49);
      this.pnlTempSettings.Name = "pnlTempSettings";
      this.pnlTempSettings.Size = new Size(693, 114);
      this.pnlTempSettings.TabIndex = 51;
      this.chkConditions.AutoSize = true;
      this.chkConditions.Location = new Point(16, 44);
      this.chkConditions.Name = "chkConditions";
      this.chkConditions.Size = new Size(671, 17);
      this.chkConditions.TabIndex = 47;
      this.chkConditions.Text = "Automatic Mode (The system applies a milestone template to a loan when the loan’s data matches that template’s conditions and criteria.)";
      this.chkConditions.UseVisualStyleBackColor = true;
      this.chkConditions.CheckedChanged += new EventHandler(this.GlobalSettings_CheckedChanged);
      this.chkShowNonMatching.AutoSize = true;
      this.chkShowNonMatching.Location = new Point(45, 85);
      this.chkShowNonMatching.Name = "chkShowNonMatching";
      this.chkShowNonMatching.Size = new Size(337, 17);
      this.chkShowNonMatching.TabIndex = 45;
      this.chkShowNonMatching.Text = "Allow matching and non-matching templates to be applied to loans";
      this.chkShowNonMatching.UseVisualStyleBackColor = true;
      this.chkShowNonMatching.CheckedChanged += new EventHandler(this.GlobalSettings_CheckedChanged);
      this.gradientPanel1.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.label2);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(692, 32);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 0;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(10, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(161, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Template Selection Settings";
      this.chkManual.AutoSize = true;
      this.chkManual.Location = new Point(16, 64);
      this.chkManual.Name = "chkManual";
      this.chkManual.Size = new Size(497, 17);
      this.chkManual.TabIndex = 45;
      this.chkManual.Text = "Manual Mode (Allow authorized users to manually apply milestone templates to loan files at any time.)";
      this.chkManual.UseVisualStyleBackColor = true;
      this.chkManual.CheckedChanged += new EventHandler(this.GlobalSettings_CheckedChanged);
      this.gradientPanel4.Borders = AnchorStyles.None;
      this.gradientPanel4.Controls.Add((Control) this.label1);
      this.gradientPanel4.Dock = DockStyle.Top;
      this.gradientPanel4.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel4.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel4.Location = new Point(0, 0);
      this.gradientPanel4.Name = "gradientPanel4";
      this.gradientPanel4.Size = new Size(693, 49);
      this.gradientPanel4.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(12, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(387, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "These global settings apply to all milestone templates. Select your settings below.";
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(522, 332);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 60;
      this.btnSave.Text = "&OK";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(605, 332);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 61;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = nameof (GlobalSettings);
      this.emHelpLink1.Location = new Point(6, 338);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 62;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.ControlLightLight;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(694, 365);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (GlobalSettings);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Global Template Settings";
      this.panel1.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.borderPanel2.ResumeLayout(false);
      this.borderPanel2.PerformLayout();
      this.gradientPanel5.ResumeLayout(false);
      this.gradientPanel5.PerformLayout();
      this.pnlTempSettings.ResumeLayout(false);
      this.pnlTempSettings.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.gradientPanel4.ResumeLayout(false);
      this.gradientPanel4.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
