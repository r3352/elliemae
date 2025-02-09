// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TemplateDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using AxSHDocVw;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine.Forms;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TemplateDialog : Form, IOnlineHelpTarget, IHelp
  {
    private AxWebBrowser axWebBrowser;
    private System.ComponentModel.Container components;
    private FrmBrowserHandler brwHandler;
    private static object nobj = (object) Missing.Value;
    private Panel webPanel;
    private Panel topPanel;
    private Label label2;
    private TextBox descTxt;
    private TextBox nameTxt;
    private Label label1;
    private Panel bottomPanel;
    private Button cancelBtn;
    private Button saveBtn;
    private System.Runtime.InteropServices.ComTypes.IConnectionPoint conPt;
    private int cookie;
    private FieldDataTemplate template;
    private TemplateSettingsType templateType;
    private EMHelpLink emHelpLink1;
    private CheckBox chkIgnore;
    private bool isPublic = true;
    private string priorName = string.Empty;
    private Sessions.Session session;

    public TemplateDialog(
      FieldDataTemplate template,
      TemplateSettingsType templateType,
      bool isPublic,
      bool readOnly,
      Sessions.Session session)
      : this(template, templateType, isPublic, session)
    {
      if (readOnly)
      {
        this.saveBtn.Enabled = false;
        this.cancelBtn.Text = "Close";
        this.AcceptButton = (IButtonControl) this.cancelBtn;
        this.nameTxt.ReadOnly = true;
        this.descTxt.ReadOnly = true;
        this.chkIgnore.Enabled = false;
      }
      this.chkIgnore.Visible = false;
      if (!this.isPublic || EnableDisableSetting.Enabled != (EnableDisableSetting) this.session.ServerManager.GetServerSetting("Components.DisplayBusinessRuleOption"))
        return;
      this.chkIgnore.Visible = true;
      if (this.template == null || !this.template.IgnoreBusinessRules)
        return;
      this.chkIgnore.Checked = true;
    }

    public TemplateDialog(
      FieldDataTemplate template,
      TemplateSettingsType templateType,
      bool isPublic,
      Sessions.Session session)
    {
      this.session = session;
      this.isPublic = isPublic;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      this.priorName = template.TemplateName;
      if (templateType == TemplateSettingsType.FundingTemplate)
      {
        this.nameTxt.ReadOnly = false;
        this.nameTxt.Leave += new EventHandler(this.nameTxt_Leave);
        this.nameTxt.TabIndex = 0;
        this.descTxt.TabIndex = 1;
      }
      this.templateType = templateType;
      this.template = template;
      this.chkIgnore.Visible = false;
      if (this.isPublic && EnableDisableSetting.Enabled == (EnableDisableSetting) this.session.ServerManager.GetServerSetting("Components.DisplayBusinessRuleOption"))
      {
        this.chkIgnore.Visible = true;
        if (this.template != null && this.template.IgnoreBusinessRules)
          this.chkIgnore.Checked = true;
      }
      this.setHelpLink(this.templateType);
      if (this.template != null)
      {
        this.nameTxt.Text = this.template.TemplateName;
        this.descTxt.Text = this.template.Description;
      }
      this.HookUpBrowserHandler();
      this.chkIgnore.CheckedChanged += new EventHandler(this.chkIgnore_CheckedChanged);
    }

    private void nameTxt_Leave(object sender, EventArgs e)
    {
      this.template.TemplateName = this.nameTxt.Text.Trim();
    }

    private void setHelpLink(TemplateSettingsType templateType)
    {
      this.emHelpLink1.HelpTag = string.Empty;
      this.emHelpLink1.Visible = false;
      switch (templateType)
      {
        case TemplateSettingsType.LoanProgram:
          this.emHelpLink1.HelpTag = "Loan Programs";
          break;
        case TemplateSettingsType.ClosingCost:
          this.emHelpLink1.HelpTag = "Closing Costs";
          break;
        case TemplateSettingsType.FundingTemplate:
          this.emHelpLink1.HelpTag = "Funding Templates";
          break;
      }
      if (!(string.Empty != this.emHelpLink1.HelpTag))
        return;
      this.emHelpLink1.Visible = true;
    }

    protected void HookUpBrowserHandler()
    {
      System.Runtime.InteropServices.ComTypes.IConnectionPointContainer ocx = (System.Runtime.InteropServices.ComTypes.IConnectionPointContainer) this.axWebBrowser.GetOcx();
      Guid guid = typeof (SHDocVw.DWebBrowserEvents2).GUID;
      ocx.FindConnectionPoint(ref guid, out this.conPt);
      this.brwHandler = new FrmBrowserHandler(this.session, (IWin32Window) this, (IHtmlInput) this.template);
      this.conPt.Advise((object) this.brwHandler, out this.cookie);
      this.brwHandler.SetHelpTarget((IOnlineHelpTarget) this);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.cookie != 0)
        {
          this.conPt.Unadvise(this.cookie);
          this.brwHandler.Dispose();
          this.cookie = 0;
        }
        if (this.components != null)
          this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    public void LoadForm(string title, string formId)
    {
      this.Text = title == "Closing Cost" ? title + " Template Details" : title + " Details";
      this.axWebBrowser.Navigate(FormStore.GetFormHTMLPath(this.session, new InputFormInfo(formId, title)), ref TemplateDialog.nobj, ref TemplateDialog.nobj, ref TemplateDialog.nobj, ref TemplateDialog.nobj);
    }

    public bool IsPublic => this.isPublic;

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TemplateDialog));
      this.webPanel = new Panel();
      this.axWebBrowser = new AxWebBrowser();
      this.topPanel = new Panel();
      this.label2 = new Label();
      this.descTxt = new TextBox();
      this.nameTxt = new TextBox();
      this.label1 = new Label();
      this.bottomPanel = new Panel();
      this.chkIgnore = new CheckBox();
      this.cancelBtn = new Button();
      this.saveBtn = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.webPanel.SuspendLayout();
      this.axWebBrowser.BeginInit();
      this.topPanel.SuspendLayout();
      this.bottomPanel.SuspendLayout();
      this.SuspendLayout();
      this.webPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.webPanel.AutoScroll = true;
      this.webPanel.Controls.Add((Control) this.axWebBrowser);
      this.webPanel.Location = new Point(0, 120);
      this.webPanel.Name = "webPanel";
      this.webPanel.Size = new Size(972, 444);
      this.webPanel.TabIndex = 3;
      this.webPanel.Click += new EventHandler(this.webPanel_Click);
      this.axWebBrowser.Dock = DockStyle.Fill;
      this.axWebBrowser.Enabled = true;
      this.axWebBrowser.Location = new Point(0, 0);
      this.axWebBrowser.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser.OcxState");
      this.axWebBrowser.Size = new Size(972, 444);
      this.axWebBrowser.TabIndex = 4;
      this.topPanel.Controls.Add((Control) this.label2);
      this.topPanel.Controls.Add((Control) this.descTxt);
      this.topPanel.Controls.Add((Control) this.nameTxt);
      this.topPanel.Controls.Add((Control) this.label1);
      this.topPanel.Dock = DockStyle.Top;
      this.topPanel.Location = new Point(0, 0);
      this.topPanel.Name = "topPanel";
      this.topPanel.Size = new Size(972, 116);
      this.topPanel.TabIndex = 0;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 42);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 7;
      this.label2.Text = "Description";
      this.descTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.descTxt.Location = new Point(94, 42);
      this.descTxt.Multiline = true;
      this.descTxt.Name = "descTxt";
      this.descTxt.ScrollBars = ScrollBars.Both;
      this.descTxt.Size = new Size(866, 68);
      this.descTxt.TabIndex = 0;
      this.nameTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.nameTxt.Location = new Point(94, 16);
      this.nameTxt.MaxLength = 256;
      this.nameTxt.Name = "nameTxt";
      this.nameTxt.ReadOnly = true;
      this.nameTxt.Size = new Size(865, 20);
      this.nameTxt.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(82, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Template Name";
      this.bottomPanel.Controls.Add((Control) this.chkIgnore);
      this.bottomPanel.Controls.Add((Control) this.emHelpLink1);
      this.bottomPanel.Controls.Add((Control) this.cancelBtn);
      this.bottomPanel.Controls.Add((Control) this.saveBtn);
      this.bottomPanel.Dock = DockStyle.Bottom;
      this.bottomPanel.Location = new Point(0, 570);
      this.bottomPanel.Name = "bottomPanel";
      this.bottomPanel.Size = new Size(972, 54);
      this.bottomPanel.TabIndex = 5;
      this.chkIgnore.AutoSize = true;
      this.chkIgnore.Location = new Point(216, 25);
      this.chkIgnore.Name = "chkIgnore";
      this.chkIgnore.Size = new Size(212, 17);
      this.chkIgnore.TabIndex = 9;
      this.chkIgnore.Text = "Template data will ignore business rules and fee management persona rights";
      this.chkIgnore.UseVisualStyleBackColor = true;
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(884, 20);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 7;
      this.cancelBtn.Text = "&Cancel";
      this.saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.saveBtn.Location = new Point(800, 20);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(75, 24);
      this.saveBtn.TabIndex = 6;
      this.saveBtn.Text = "&Save";
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.Location = new Point(20, 22);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 8;
      this.emHelpLink1.Visible = false;
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(972, 624);
      this.Controls.Add((Control) this.bottomPanel);
      this.Controls.Add((Control) this.topPanel);
      this.Controls.Add((Control) this.webPanel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (TemplateDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (TemplateDialog);
      this.Closing += new CancelEventHandler(this.TemplateDialog_Closing);
      this.webPanel.ResumeLayout(false);
      this.axWebBrowser.EndInit();
      this.topPanel.ResumeLayout(false);
      this.topPanel.PerformLayout();
      this.bottomPanel.ResumeLayout(false);
      this.bottomPanel.PerformLayout();
      this.ResumeLayout(false);
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      string str = "";
      if (this.template is ClosingCost)
      {
        IInputHandler inputHandler = this.brwHandler.GetInputHandler();
        if (inputHandler != null && inputHandler is REGZGFE_2010InputHandler)
          ((REGZGFE_2010InputHandler) inputHandler).SetTemplate();
      }
      if (this.template is LoanProgram || this.template is ClosingCost)
        str = this.template.GetSimpleField("LP97");
      if (this.template is LoanProgram && this.isPublic && (str.ToLower().StartsWith("private") || str.ToLower().StartsWith("personal")))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This public loan program contains a personal closing cost.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.template is ClosingCost)
        {
          ((ClosingCost) this.template).RemoveContactDetail();
          if (this.template.IsLocked("454") && string.IsNullOrEmpty(this.template.GetField("454")))
            this.template.RemoveLock("454");
        }
        if (this.template is FundingTemplate)
        {
          if (this.nameTxt.Text.Trim() == "")
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "A name must be provided for this template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.DialogResult = DialogResult.None;
            return;
          }
          if (string.Compare(this.priorName, this.nameTxt.Text.Trim(), true) != 0 && this.session.ConfigurationManager.TemplateSettingsObjectExists(TemplateSettingsType.FundingTemplate, new FileSystemEntry(FileSystemEntry.PublicRoot.Path, this.nameTxt.Text.Trim(), FileSystemEntry.Types.File, (string) null)))
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "The name '" + this.nameTxt.Text.Trim() + "' is already in use. Select a new name and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.nameTxt.Focus();
            return;
          }
        }
        this.template.Description = this.descTxt.Text.Trim();
        if (!this.isPublic)
          this.template.IgnoreBusinessRules = false;
        this.DialogResult = DialogResult.OK;
      }
    }

    public string GetHelpTargetName()
    {
      if (this.template is LoanProgram)
        return "Loan Programs";
      if (this.template is ClosingCost)
        return "Closing Costs";
      return this.template is FundingTemplate ? "Funding Templates" : "";
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    private void webPanel_Click(object sender, EventArgs e) => this.ShowHelp();

    private void TemplateDialog_Closing(object sender, CancelEventArgs e)
    {
      if (this.template is ClosingCost)
      {
        InputHandlerBase inputHandler = (InputHandlerBase) this.brwHandler.GetInputHandler();
        if (inputHandler is REGZGFE_2015InputHandler)
          ((REGZGFE_2015InputHandler) inputHandler).CloseAllPopupWindows();
      }
      if (this.cookie != 0)
      {
        this.conPt.Unadvise(this.cookie);
        this.brwHandler.Dispose();
        this.cookie = 0;
      }
      this.axWebBrowser.Dispose();
    }

    private void chkIgnore_CheckedChanged(object sender, EventArgs e)
    {
      this.template.IgnoreBusinessRules = this.chkIgnore.Checked;
    }
  }
}
