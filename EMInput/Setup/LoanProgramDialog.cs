// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanProgramDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using AxSHDocVw;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
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
  public class LoanProgramDialog : Form
  {
    private Sessions.Session session;
    private LoanProgram loanProgram;
    private FrmBrowserHandler browserHandler;
    private bool isPublic;
    private IContainer components;
    private Panel pnlBody;
    private Panel pnlNameDesc;
    private Label label2;
    private Label label1;
    private TextBox txtName;
    private TextBox txtDescription;
    private Panel pnlClosingCost;
    private ComboBox cboDocTypeCode;
    private TextBox txtClosingCostProgram;
    private Label label5;
    private Label label6;
    private StandardIconButton btnClearClosingCost;
    private StandardIconButton btnSelectClosingCost;
    private DialogButtons dlgButtons;
    private AxWebBrowser webBrowser;
    private ToolTip toolTip1;
    private Label label3;
    private EMHelpLink emHelpLink1;
    private CheckBox chkIgnore;

    public LoanProgramDialog(Sessions.Session session, LoanProgram loanProgram, bool isPublic)
    {
      this.session = session;
      this.loanProgram = loanProgram;
      this.isPublic = isPublic;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      this.loadDocTypeCodes();
      this.populateProgramInfo();
      this.createBrowserHandler();
      this.chkIgnore.Visible = false;
      if (!this.isPublic || EnableDisableSetting.Enabled != (EnableDisableSetting) this.session.ServerManager.GetServerSetting("Components.DisplayBusinessRuleOption"))
        return;
      this.chkIgnore.Visible = true;
      if (this.loanProgram == null || !this.loanProgram.IgnoreBusinessRules)
        return;
      this.chkIgnore.Checked = true;
    }

    private void loadDocTypeCodes()
    {
      this.cboDocTypeCode.Items.Clear();
      foreach (FieldOption option in StandardFields.GetField("MORNET.X67").Options)
        this.cboDocTypeCode.Items.Add((object) option);
    }

    private void populateProgramInfo()
    {
      this.txtName.Text = this.loanProgram.TemplateName;
      this.txtDescription.Text = this.loanProgram.Description;
      if (this.loanProgram.ClosingCostPath != "")
      {
        this.txtClosingCostProgram.Text = FileSystemEntry.Parse(this.loanProgram.ClosingCostPath, this.session.UserID).ToDisplayString();
        this.btnClearClosingCost.Enabled = true;
      }
      this.cboDocTypeCode.Text = "";
      string simpleField = this.loanProgram.GetSimpleField("MORNET.X67");
      foreach (FieldOption fieldOption in this.cboDocTypeCode.Items)
      {
        if (fieldOption.Value == simpleField)
        {
          this.cboDocTypeCode.SelectedItem = (object) fieldOption;
          break;
        }
      }
      if (!(simpleField != "") || !(this.cboDocTypeCode.Text == ""))
        return;
      this.cboDocTypeCode.Text = simpleField;
    }

    private void createBrowserHandler()
    {
      this.browserHandler = new FrmBrowserHandler(this.session, (IWin32Window) this, (IHtmlInput) this.loanProgram);
      this.browserHandler.AttachToBrowser(this.webBrowser);
      this.browserHandler.OpenForm(new InputFormInfo("LOANPROG", "Loan Program", InputFormType.Standard));
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      if (this.cboDocTypeCode.SelectedItem != null)
        this.loanProgram.SetField("MORNET.X67", ((FieldOption) this.cboDocTypeCode.SelectedItem).Value);
      else
        this.loanProgram.SetField("MORNET.X67", this.cboDocTypeCode.Text);
      this.loanProgram.Description = this.txtDescription.Text;
      this.loanProgram.IgnoreBusinessRules = this.isPublic && this.chkIgnore.Checked;
      this.DialogResult = DialogResult.OK;
    }

    private void btnSelectClosingCost_Click(object sender, EventArgs e)
    {
      FileSystemEntry defaultFolder = FileSystemEntry.PublicRoot;
      if (this.loanProgram.ClosingCostPath != "")
        defaultFolder = FileSystemEntry.Parse(this.loanProgram.ClosingCostPath, this.session.UserID).ParentFolder;
      using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, TemplateSettingsType.ClosingCost, defaultFolder))
      {
        if (templateSelectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.loanProgram.ClosingCostPath = templateSelectionDialog.SelectedItem.ToString();
        this.txtClosingCostProgram.Text = templateSelectionDialog.SelectedItem.ToDisplayString();
        this.btnClearClosingCost.Enabled = true;
      }
    }

    private void btnClearClosingCost_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to remove the selected Closing Cost template from the Loan Program?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.loanProgram.ClosingCostPath = "";
      this.txtClosingCostProgram.Text = "";
      this.btnClearClosingCost.Enabled = false;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanProgramDialog));
      this.pnlBody = new Panel();
      this.webBrowser = new AxWebBrowser();
      this.pnlClosingCost = new Panel();
      this.btnClearClosingCost = new StandardIconButton();
      this.btnSelectClosingCost = new StandardIconButton();
      this.cboDocTypeCode = new ComboBox();
      this.txtClosingCostProgram = new TextBox();
      this.label5 = new Label();
      this.label6 = new Label();
      this.pnlNameDesc = new Panel();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.txtName = new TextBox();
      this.txtDescription = new TextBox();
      this.toolTip1 = new ToolTip(this.components);
      this.dlgButtons = new DialogButtons();
      this.emHelpLink1 = new EMHelpLink();
      this.chkIgnore = new CheckBox();
      this.pnlBody.SuspendLayout();
      this.webBrowser.BeginInit();
      this.pnlClosingCost.SuspendLayout();
      ((ISupportInitialize) this.btnClearClosingCost).BeginInit();
      ((ISupportInitialize) this.btnSelectClosingCost).BeginInit();
      this.pnlNameDesc.SuspendLayout();
      this.SuspendLayout();
      this.pnlBody.Controls.Add((Control) this.webBrowser);
      this.pnlBody.Controls.Add((Control) this.pnlClosingCost);
      this.pnlBody.Controls.Add((Control) this.pnlNameDesc);
      this.pnlBody.Dock = DockStyle.Fill;
      this.pnlBody.Location = new Point(0, 0);
      this.pnlBody.Name = "pnlBody";
      this.pnlBody.Padding = new Padding(2);
      this.pnlBody.Size = new Size(714, 568);
      this.pnlBody.TabIndex = 1;
      this.webBrowser.Dock = DockStyle.Fill;
      this.webBrowser.Enabled = true;
      this.webBrowser.Location = new Point(2, 168);
      this.webBrowser.OcxState = (AxHost.State) componentResourceManager.GetObject("webBrowser.OcxState");
      this.webBrowser.Size = new Size(710, 398);
      this.webBrowser.TabIndex = 2;
      this.pnlClosingCost.Controls.Add((Control) this.btnClearClosingCost);
      this.pnlClosingCost.Controls.Add((Control) this.btnSelectClosingCost);
      this.pnlClosingCost.Controls.Add((Control) this.cboDocTypeCode);
      this.pnlClosingCost.Controls.Add((Control) this.txtClosingCostProgram);
      this.pnlClosingCost.Controls.Add((Control) this.label5);
      this.pnlClosingCost.Controls.Add((Control) this.label6);
      this.pnlClosingCost.Dock = DockStyle.Top;
      this.pnlClosingCost.Location = new Point(2, 118);
      this.pnlClosingCost.Name = "pnlClosingCost";
      this.pnlClosingCost.Size = new Size(710, 50);
      this.pnlClosingCost.TabIndex = 11;
      this.btnClearClosingCost.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClearClosingCost.BackColor = Color.Transparent;
      this.btnClearClosingCost.Enabled = false;
      this.btnClearClosingCost.Location = new Point(687, 3);
      this.btnClearClosingCost.MouseDownImage = (Image) null;
      this.btnClearClosingCost.Name = "btnClearClosingCost";
      this.btnClearClosingCost.Size = new Size(16, 15);
      this.btnClearClosingCost.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnClearClosingCost.TabIndex = 19;
      this.btnClearClosingCost.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnClearClosingCost, "Remove Closing Cost Template");
      this.btnClearClosingCost.Click += new EventHandler(this.btnClearClosingCost_Click);
      this.btnSelectClosingCost.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSelectClosingCost.BackColor = Color.Transparent;
      this.btnSelectClosingCost.Location = new Point(668, 3);
      this.btnSelectClosingCost.MouseDownImage = (Image) null;
      this.btnSelectClosingCost.Name = "btnSelectClosingCost";
      this.btnSelectClosingCost.Size = new Size(16, 15);
      this.btnSelectClosingCost.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelectClosingCost.TabIndex = 18;
      this.btnSelectClosingCost.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSelectClosingCost, "Select Closing Cost Template");
      this.btnSelectClosingCost.Click += new EventHandler(this.btnSelectClosingCost_Click);
      this.cboDocTypeCode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboDocTypeCode.FormattingEnabled = true;
      this.cboDocTypeCode.Location = new Point(121, 24);
      this.cboDocTypeCode.Name = "cboDocTypeCode";
      this.cboDocTypeCode.Size = new Size(542, 21);
      this.cboDocTypeCode.TabIndex = 14;
      this.txtClosingCostProgram.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtClosingCostProgram.Location = new Point(121, 2);
      this.txtClosingCostProgram.Name = "txtClosingCostProgram";
      this.txtClosingCostProgram.ReadOnly = true;
      this.txtClosingCostProgram.Size = new Size(542, 20);
      this.txtClosingCostProgram.TabIndex = 13;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 27);
      this.label5.Name = "label5";
      this.label5.Size = new Size(112, 13);
      this.label5.TabIndex = 12;
      this.label5.Text = "Loan Doc. Type Code";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 6);
      this.label6.Name = "label6";
      this.label6.Size = new Size(107, 13);
      this.label6.TabIndex = 11;
      this.label6.Text = "Closing Cost Program";
      this.pnlNameDesc.Controls.Add((Control) this.label3);
      this.pnlNameDesc.Controls.Add((Control) this.label2);
      this.pnlNameDesc.Controls.Add((Control) this.label1);
      this.pnlNameDesc.Controls.Add((Control) this.txtName);
      this.pnlNameDesc.Controls.Add((Control) this.txtDescription);
      this.pnlNameDesc.Dock = DockStyle.Top;
      this.pnlNameDesc.Location = new Point(2, 2);
      this.pnlNameDesc.Name = "pnlNameDesc";
      this.pnlNameDesc.Size = new Size(710, 116);
      this.pnlNameDesc.TabIndex = 9;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(569, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "Enter the desired values in the fields below. Values added from a plan code are read-only and cannot be changed here.";
      this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 58);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 7;
      this.label2.Text = "Description";
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 35);
      this.label1.Name = "label1";
      this.label1.Size = new Size(82, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Template Name";
      this.txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtName.Location = new Point(121, 33);
      this.txtName.MaxLength = 256;
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(542, 20);
      this.txtName.TabIndex = 1;
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Location = new Point(121, 56);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Both;
      this.txtDescription.Size = new Size(542, 57);
      this.txtDescription.TabIndex = 0;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 568);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.OKText = "&Save";
      this.dlgButtons.Size = new Size(714, 38);
      this.dlgButtons.TabIndex = 4;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Loan Programs";
      this.emHelpLink1.Location = new Point(11, 580);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 15);
      this.emHelpLink1.TabIndex = 12;
      this.chkIgnore.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkIgnore.AutoSize = true;
      this.chkIgnore.Location = new Point(128, 580);
      this.chkIgnore.Name = "chkIgnore";
      this.chkIgnore.Size = new Size(384, 17);
      this.chkIgnore.TabIndex = 13;
      this.chkIgnore.Text = "Template data will ignore business rules and fee management persona rights";
      this.chkIgnore.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(714, 606);
      this.Controls.Add((Control) this.chkIgnore);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.pnlBody);
      this.Controls.Add((Control) this.dlgButtons);
      this.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.MinimumSize = new Size(715, 253);
      this.Name = nameof (LoanProgramDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Loan Program Details";
      this.pnlBody.ResumeLayout(false);
      this.webBrowser.EndInit();
      this.pnlClosingCost.ResumeLayout(false);
      this.pnlClosingCost.PerformLayout();
      ((ISupportInitialize) this.btnClearClosingCost).EndInit();
      ((ISupportInitialize) this.btnSelectClosingCost).EndInit();
      this.pnlNameDesc.ResumeLayout(false);
      this.pnlNameDesc.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
