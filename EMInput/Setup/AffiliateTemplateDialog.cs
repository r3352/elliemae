// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AffiliateTemplateDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AffiliateTemplateDialog : Form, IOnlineHelpTarget, IHelp
  {
    private AffiliatedBusinessArrangementForm serviceForm;
    private AffiliateTemplate template;
    private bool isPublic;
    private Sessions.Session session;
    private IContainer components;
    private Label label2;
    private TextBox descTxt;
    private TextBox nameTxt;
    private Label label1;
    private PanelEx panelExDetail;
    private Button btnCancel;
    private Button btnSave;
    private CheckBox chkIgnore;
    private EMHelpLink emHelpLink1;

    public AffiliateTemplateDialog(
      AffiliateTemplate template,
      bool isPublic,
      bool readOnly,
      Sessions.Session session)
    {
      this.template = template;
      this.isPublic = isPublic;
      this.session = session;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      this.serviceForm = new AffiliatedBusinessArrangementForm((IHtmlInput) this.template, readOnly, this.session);
      this.panelExDetail.Controls.Add((Control) this.serviceForm);
      this.serviceForm.BringToFront();
      this.initForm();
      if (!readOnly)
        return;
      this.descTxt.ReadOnly = true;
      this.btnCancel.Text = "&Close";
      this.btnSave.Visible = false;
      this.chkIgnore.Enabled = false;
    }

    private void initForm()
    {
      this.nameTxt.Text = this.template.TemplateName;
      this.descTxt.Text = this.template.Description;
      this.chkIgnore.Checked = this.template.IgnoreBusinessRules;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.template.TemplateName = this.nameTxt.Text;
      this.template.Description = this.descTxt.Text.Trim();
      this.template.IgnoreBusinessRules = this.chkIgnore.Checked;
      if (!this.serviceForm.SetTemplate(this.template))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please add a Affiliate first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    public string GetHelpTargetName() => "Setup\\Affiliates";

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    private void SettlementServiceTemplateDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label2 = new Label();
      this.descTxt = new TextBox();
      this.nameTxt = new TextBox();
      this.label1 = new Label();
      this.panelExDetail = new PanelEx();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.chkIgnore = new CheckBox();
      this.emHelpLink1 = new EMHelpLink();
      this.SuspendLayout();
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 12;
      this.label2.Text = "Description";
      this.descTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.descTxt.Location = new Point(101, 38);
      this.descTxt.Multiline = true;
      this.descTxt.Name = "descTxt";
      this.descTxt.ScrollBars = ScrollBars.Both;
      this.descTxt.Size = new Size(749, 66);
      this.descTxt.TabIndex = 10;
      this.nameTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.nameTxt.Location = new Point(101, 12);
      this.nameTxt.MaxLength = 256;
      this.nameTxt.Name = "nameTxt";
      this.nameTxt.ReadOnly = true;
      this.nameTxt.Size = new Size(749, 20);
      this.nameTxt.TabIndex = 13;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(82, 13);
      this.label1.TabIndex = 11;
      this.label1.Text = "Template Name";
      this.panelExDetail.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelExDetail.Location = new Point(3, 110);
      this.panelExDetail.Name = "panelExDetail";
      this.panelExDetail.Size = new Size(847, 503);
      this.panelExDetail.TabIndex = 14;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(767, 619);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 15;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(686, 619);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 16;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.chkIgnore.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkIgnore.AutoSize = true;
      this.chkIgnore.Location = new Point(215, 623);
      this.chkIgnore.Name = "chkIgnore";
      this.chkIgnore.Size = new Size(212, 17);
      this.chkIgnore.TabIndex = 17;
      this.chkIgnore.Text = "Template data will ignore business rules";
      this.chkIgnore.UseVisualStyleBackColor = true;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Affiliates";
      this.emHelpLink1.Location = new Point(3, 624);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 34;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(854, 654);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.chkIgnore);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.panelExDetail);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.descTxt);
      this.Controls.Add((Control) this.nameTxt);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (AffiliateTemplateDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Affiliated Business Arrangement Details";
      this.KeyUp += new KeyEventHandler(this.SettlementServiceTemplateDialog_KeyUp);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
