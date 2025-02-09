// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SyncTemplateSetupDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SyncTemplateSetupDialog : Form
  {
    private SyncTemplate syncTemplate;
    private Sessions.Session session;
    private PiggybackSetupPanel piggybackSetupPanel;
    private IContainer components;
    private Panel panelBottom;
    private Panel panelTop;
    private Panel panelMiddle;
    private ToolTip toolTip1;
    private Button btnSave;
    private Button btnCancel;
    private TextBox txtDescription;
    private TextBox txtName;
    private Label label2;
    private Label label1;

    public SyncTemplateSetupDialog(
      Sessions.Session session,
      FieldSettings fieldSettings,
      SyncTemplate syncTemplate)
    {
      this.session = session;
      this.syncTemplate = syncTemplate;
      this.InitializeComponent();
      if (syncTemplate != null && (syncTemplate.SyncFields == null || syncTemplate.SyncFields.Count == 0))
        syncTemplate.AddFields(Utils.LoadPiggybackDefaultSyncFields((IWin32Window) Session.MainScreen, AssemblyResolver.GetResourceFileFullPath(SystemSettings.DocDirRelPath + "PiggybackDefaultList.xml", SystemSettings.LocalAppDir)));
      this.piggybackSetupPanel = new PiggybackSetupPanel(this.session, this.syncTemplate, fieldSettings);
      this.piggybackSetupPanel.OnStatusChanged += new EventHandler(this.piggybackSetupPanel_OnStatusChanged);
      this.panelMiddle.Controls.Add((Control) this.piggybackSetupPanel);
      this.txtName.Text = this.syncTemplate != null ? this.syncTemplate.TemplateName : "";
      this.txtDescription.Text = this.syncTemplate != null ? this.syncTemplate.TemplateDescription : "";
      this.piggybackSetupPanel_OnStatusChanged((object) null, (EventArgs) null);
    }

    private void piggybackSetupPanel_OnStatusChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = this.piggybackSetupPanel.SyncTemplateValidation() && this.txtName.Text.Trim() != "";
    }

    public SyncTemplate NewSyncTemplate
    {
      get
      {
        if (this.syncTemplate == null)
        {
          this.syncTemplate = new SyncTemplate(this.txtName.Text, this.txtDescription.Text);
        }
        else
        {
          this.syncTemplate.TemplateName = this.txtName.Text;
          this.syncTemplate.TemplateDescription = this.txtDescription.Text;
        }
        this.piggybackSetupPanel.SetSyncTemplate(this.syncTemplate);
        return this.syncTemplate;
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.txtName.Text.Trim() == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Sync Template name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (!this.piggybackSetupPanel.SyncTemplateValidation())
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please add at least one field for synchronization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if ((this.syncTemplate == null || this.syncTemplate.TemplateID == -1 || this.syncTemplate != null && string.Compare(this.syncTemplate.TemplateName, this.txtName.Text, true) != 0) && this.session.ConfigurationManager.SyncTemplateNameExist(this.txtName.Text))
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "The Sync Template list already contains a template named '" + this.txtName.Text + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void txtName_TextChanged(object sender, EventArgs e)
    {
      this.piggybackSetupPanel_OnStatusChanged((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.panelBottom = new Panel();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.panelTop = new Panel();
      this.txtDescription = new TextBox();
      this.txtName = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panelMiddle = new Panel();
      this.toolTip1 = new ToolTip(this.components);
      this.panelBottom.SuspendLayout();
      this.panelTop.SuspendLayout();
      this.SuspendLayout();
      this.panelBottom.Controls.Add((Control) this.btnSave);
      this.panelBottom.Controls.Add((Control) this.btnCancel);
      this.panelBottom.Dock = DockStyle.Bottom;
      this.panelBottom.Location = new Point(0, 681);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(993, 49);
      this.panelBottom.TabIndex = 0;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(825, 14);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 3;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(906, 14);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.panelTop.Controls.Add((Control) this.txtDescription);
      this.panelTop.Controls.Add((Control) this.txtName);
      this.panelTop.Controls.Add((Control) this.label2);
      this.panelTop.Controls.Add((Control) this.label1);
      this.panelTop.Dock = DockStyle.Top;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(993, 104);
      this.panelTop.TabIndex = 1;
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Location = new Point(100, 33);
      this.txtDescription.MaxLength = 4096;
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Both;
      this.txtDescription.Size = new Size(881, 65);
      this.txtDescription.TabIndex = 1;
      this.txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtName.Location = new Point(100, 9);
      this.txtName.MaxLength = (int) byte.MaxValue;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(881, 20);
      this.txtName.TabIndex = 0;
      this.txtName.TextChanged += new EventHandler(this.txtName_TextChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Description";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(82, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Template Name";
      this.panelMiddle.Dock = DockStyle.Fill;
      this.panelMiddle.Location = new Point(0, 104);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new Size(993, 577);
      this.panelMiddle.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(993, 730);
      this.Controls.Add((Control) this.panelMiddle);
      this.Controls.Add((Control) this.panelTop);
      this.Controls.Add((Control) this.panelBottom);
      this.Name = nameof (SyncTemplateSetupDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Sync Template Details";
      this.panelBottom.ResumeLayout(false);
      this.panelTop.ResumeLayout(false);
      this.panelTop.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
