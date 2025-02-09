// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ConditionRulePanelContainer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ConditionRulePanelContainer : SettingsUserControl
  {
    private Sessions.Session session;
    private ConditionRulePanel conditionRulePanel;
    private IContainer components;
    private Panel panelBottom;
    private Panel panelTop;
    private GroupContainer groupContainer1;
    private StandardIconButton iconBtnSave;
    private StandardIconButton iconBtnReset;
    private CheckBox chkAllow;

    public ConditionRulePanelContainer(SetUpContainer setupContainer)
      : this(Session.DefaultInstance, setupContainer, false)
    {
    }

    public ConditionRulePanelContainer(
      Sessions.Session session,
      SetUpContainer setupContainer,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.Dock = DockStyle.Fill;
      this.InitializeComponent();
      this.session = session;
      this.conditionRulePanel = new ConditionRulePanel(session, allowMultiSelect);
      this.panelTop.Controls.Add((Control) this.conditionRulePanel);
      this.Reset();
    }

    public string[] SelectedRules
    {
      get => this.conditionRulePanel.SelectedRules;
      set => this.conditionRulePanel.SelectedRules = value;
    }

    private void iconBtnSave_Click(object sender, EventArgs e) => this.Save();

    private void iconBtnReset_Click(object sender, EventArgs e) => this.Reset();

    private void chkAllow_CheckedChanged(object sender, EventArgs e)
    {
      this.iconBtnSave.Enabled = this.iconBtnReset.Enabled = true;
      this.setDirtyFlag(true);
    }

    public override void Save()
    {
      this.session.ConfigurationManager.SetCompanySetting("RequiredFields", "Display", this.chkAllow.Checked ? "1" : "");
      this.setDirtyFlag(false);
      this.iconBtnSave.Enabled = this.iconBtnReset.Enabled = false;
    }

    public override void Reset()
    {
      this.chkAllow.Checked = this.session.ConfigurationManager.GetCompanySetting("RequiredFields", "Display") == "1";
      this.setDirtyFlag(false);
      this.iconBtnSave.Enabled = this.iconBtnReset.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelBottom = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.iconBtnSave = new StandardIconButton();
      this.iconBtnReset = new StandardIconButton();
      this.chkAllow = new CheckBox();
      this.panelTop = new Panel();
      this.panelBottom.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.iconBtnSave).BeginInit();
      ((ISupportInitialize) this.iconBtnReset).BeginInit();
      this.SuspendLayout();
      this.panelBottom.Controls.Add((Control) this.groupContainer1);
      this.panelBottom.Dock = DockStyle.Bottom;
      this.panelBottom.Location = new Point(0, 422);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(678, 63);
      this.panelBottom.TabIndex = 0;
      this.groupContainer1.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.iconBtnSave);
      this.groupContainer1.Controls.Add((Control) this.iconBtnReset);
      this.groupContainer1.Controls.Add((Control) this.chkAllow);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(678, 63);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Required Fields Rule Setting";
      this.iconBtnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnSave.BackColor = Color.Transparent;
      this.iconBtnSave.Location = new Point(633, 5);
      this.iconBtnSave.Name = "iconBtnSave";
      this.iconBtnSave.Size = new Size(16, 16);
      this.iconBtnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.iconBtnSave.TabIndex = 2;
      this.iconBtnSave.TabStop = false;
      this.iconBtnSave.Click += new EventHandler(this.iconBtnSave_Click);
      this.iconBtnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnReset.BackColor = Color.Transparent;
      this.iconBtnReset.Location = new Point(655, 5);
      this.iconBtnReset.Name = "iconBtnReset";
      this.iconBtnReset.Size = new Size(16, 16);
      this.iconBtnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.iconBtnReset.TabIndex = 1;
      this.iconBtnReset.TabStop = false;
      this.iconBtnReset.Click += new EventHandler(this.iconBtnReset_Click);
      this.chkAllow.AutoSize = true;
      this.chkAllow.Location = new Point(11, 35);
      this.chkAllow.Name = "chkAllow";
      this.chkAllow.Size = new Size(297, 17);
      this.chkAllow.TabIndex = 0;
      this.chkAllow.Text = "Allow users to complete fields on the Milestone worksheet";
      this.chkAllow.UseVisualStyleBackColor = true;
      this.chkAllow.CheckedChanged += new EventHandler(this.chkAllow_CheckedChanged);
      this.panelTop.Dock = DockStyle.Fill;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(678, 422);
      this.panelTop.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelTop);
      this.Controls.Add((Control) this.panelBottom);
      this.Name = nameof (ConditionRulePanelContainer);
      this.Size = new Size(678, 485);
      this.panelBottom.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      ((ISupportInitialize) this.iconBtnSave).EndInit();
      ((ISupportInitialize) this.iconBtnReset).EndInit();
      this.ResumeLayout(false);
    }
  }
}
