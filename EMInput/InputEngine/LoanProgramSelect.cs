// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LoanProgramSelect
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LoanProgramSelect : Form, IHelp
  {
    private const string className = "LoanProgramSelect";
    private static string sw = Tracing.SwInputEngine;
    private Button cancelBtn;
    protected Button selectBtn;
    private System.ComponentModel.Container components;
    private const EllieMae.EMLite.ClientServer.TemplateSettingsType templateType = EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram;
    private LoanDataMgr loanMgr;
    private LoanData loan;
    private FileSystemEntry selectedEntry;
    private LoanProgram lp;
    private ClosingCost closingCostTemplate;
    private Sessions.Session session;
    private string lpName = string.Empty;
    private string lpFullName = string.Empty;
    private CheckBox chkAppendLP;
    private CheckBox chkAppendCC;
    private Panel panelBottom;
    private Panel panelLeft;
    private Panel panelRight;
    private GroupContainer groupDetail;
    private CollapsibleSplitter collapsibleSplitter1;
    private BorderPanel borderPanelLeft;
    private GradientPanel gbOptions;
    private FileSystemExplorer fsExplorer;
    private LoanProgramSelect.SelectTypes selectType;
    private FormBrowser fbBrowser;
    private LoanProgramFileSystem fileSystem;

    public event EventHandler TemplateApplied;

    public LoanProgramSelect(Sessions.Session session)
      : this((LoanData) null, session)
    {
    }

    public LoanProgramSelect(LoanData loan, Sessions.Session session)
      : this(loan, LoanProgramSelect.SelectTypes.ForLoan, session)
    {
    }

    public LoanProgramSelect(
      LoanData loan,
      LoanProgramSelect.SelectTypes selectType,
      Sessions.Session session)
    {
      this.session = session;
      this.loan = loan;
      if (this.loan != null)
        this.loanMgr = this.loan.DataMgr != null ? (LoanDataMgr) this.loan.DataMgr : throw new ArgumentException("The specified LoanData object is not associated with a LoanDataMgr");
      this.selectType = selectType;
      this.InitializeComponent();
      if (this.selectType == LoanProgramSelect.SelectTypes.ForLockRequest)
        this.gbOptions.Visible = false;
      else if (this.selectType == LoanProgramSelect.SelectTypes.ForTrade)
      {
        this.panelRight.Visible = false;
        this.collapsibleSplitter1.Visible = false;
        this.Width = this.panelLeft.Width;
        this.panelLeft.Dock = DockStyle.Fill;
      }
      this.chkAppendCC.Checked = false;
      this.chkAppendCC.Enabled = false;
      this.chkAppendLP.Checked = (bool) this.session.StartupInfo.PolicySettings[(object) "Policies.AppendLoanProgram"];
      this.fileSystem = new LoanProgramFileSystem(this.session);
      this.fsExplorer.AttachFileSystem((IFileSystem) this.fileSystem);
      this.fsExplorer.AllowMultiselect = this.selectType == LoanProgramSelect.SelectTypes.ForTrade;
      FileSystemEntry lastVisitedFolder = this.fileSystem.GetLastVisitedFolder();
      try
      {
        if (lastVisitedFolder != null)
          this.fsExplorer.CurrentFolder = lastVisitedFolder;
      }
      catch
      {
      }
      if (this.loanMgr == null)
        this.gbOptions.Visible = false;
      if (this.selectType == LoanProgramSelect.SelectTypes.ForLoan)
        this.fsExplorer.SetColumnSortOrder(new ExplorerColumnSortOrder[2]
        {
          new ExplorerColumnSortOrder(2, SortOrder.Ascending),
          new ExplorerColumnSortOrder(0, SortOrder.Ascending)
        });
      this.fsExplorer.SelectFirstItemOfType(FileSystemEntry.Types.File);
      if (Screen.PrimaryScreen.Bounds.Size.Width <= 1300)
        this.Width = 1024;
      else
        this.Width = 1300;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.chkAppendLP = new CheckBox();
      this.chkAppendCC = new CheckBox();
      this.cancelBtn = new Button();
      this.selectBtn = new Button();
      this.panelBottom = new Panel();
      this.panelLeft = new Panel();
      this.borderPanelLeft = new BorderPanel();
      this.fsExplorer = new FileSystemExplorer();
      this.gbOptions = new GradientPanel();
      this.panelRight = new Panel();
      this.groupDetail = new GroupContainer();
      this.fbBrowser = new FormBrowser();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panelBottom.SuspendLayout();
      this.panelLeft.SuspendLayout();
      this.borderPanelLeft.SuspendLayout();
      this.gbOptions.SuspendLayout();
      this.panelRight.SuspendLayout();
      this.groupDetail.SuspendLayout();
      this.SuspendLayout();
      this.chkAppendLP.BackColor = Color.Transparent;
      this.chkAppendLP.Location = new Point(10, 8);
      this.chkAppendLP.Name = "chkAppendLP";
      this.chkAppendLP.Size = new Size(318, 22);
      this.chkAppendLP.TabIndex = 3;
      this.chkAppendLP.Text = "Only apply Loan Program template fields that contain a value";
      this.chkAppendLP.UseVisualStyleBackColor = false;
      this.chkAppendCC.BackColor = Color.Transparent;
      this.chkAppendCC.Location = new Point(10, 27);
      this.chkAppendCC.Name = "chkAppendCC";
      this.chkAppendCC.Size = new Size(318, 22);
      this.chkAppendCC.TabIndex = 4;
      this.chkAppendCC.Text = "Only apply Closing Cost template fields that contain a value";
      this.chkAppendCC.UseVisualStyleBackColor = false;
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.BackColor = SystemColors.Control;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(919, 9);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 22);
      this.cancelBtn.TabIndex = 7;
      this.cancelBtn.Text = "&Cancel";
      this.cancelBtn.UseVisualStyleBackColor = true;
      this.selectBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.selectBtn.BackColor = SystemColors.Control;
      this.selectBtn.Location = new Point(836, 9);
      this.selectBtn.Name = "selectBtn";
      this.selectBtn.Size = new Size(75, 22);
      this.selectBtn.TabIndex = 6;
      this.selectBtn.Text = "&Select";
      this.selectBtn.UseVisualStyleBackColor = true;
      this.selectBtn.Click += new EventHandler(this.selectBtn_Click);
      this.panelBottom.Controls.Add((Control) this.selectBtn);
      this.panelBottom.Controls.Add((Control) this.cancelBtn);
      this.panelBottom.Dock = DockStyle.Bottom;
      this.panelBottom.Location = new Point(0, 515);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(1004, 40);
      this.panelBottom.TabIndex = 8;
      this.panelLeft.Controls.Add((Control) this.borderPanelLeft);
      this.panelLeft.Controls.Add((Control) this.gbOptions);
      this.panelLeft.Dock = DockStyle.Fill;
      this.panelLeft.Location = new Point(0, 0);
      this.panelLeft.Name = "panelLeft";
      this.panelLeft.Size = new Size(258, 515);
      this.panelLeft.TabIndex = 9;
      this.borderPanelLeft.Borders = AnchorStyles.None;
      this.borderPanelLeft.Controls.Add((Control) this.fsExplorer);
      this.borderPanelLeft.Dock = DockStyle.Fill;
      this.borderPanelLeft.Location = new Point(0, 0);
      this.borderPanelLeft.Name = "borderPanelLeft";
      this.borderPanelLeft.Size = new Size(258, 460);
      this.borderPanelLeft.TabIndex = 4;
      this.fsExplorer.ActionBarVisible = false;
      this.fsExplorer.AllowMultiselect = false;
      this.fsExplorer.Dock = DockStyle.Fill;
      this.fsExplorer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fsExplorer.Location = new Point(0, 0);
      this.fsExplorer.Name = "fsExplorer";
      this.fsExplorer.Size = new Size(258, 460);
      this.fsExplorer.TabIndex = 1;
      this.fsExplorer.Title = "Loan Programs";
      this.fsExplorer.SelectedEntryChanged += new EventHandler(this.fsExplorer_SelectedEntryChanged);
      this.fsExplorer.EntryDoubleClick += new FileSystemEventHandler(this.fsExplorer_EntryDoubleClick);
      this.gbOptions.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gbOptions.Controls.Add((Control) this.chkAppendLP);
      this.gbOptions.Controls.Add((Control) this.chkAppendCC);
      this.gbOptions.Dock = DockStyle.Bottom;
      this.gbOptions.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gbOptions.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gbOptions.Location = new Point(0, 460);
      this.gbOptions.Name = "gbOptions";
      this.gbOptions.Size = new Size(258, 55);
      this.gbOptions.TabIndex = 0;
      this.panelRight.Controls.Add((Control) this.groupDetail);
      this.panelRight.Dock = DockStyle.Right;
      this.panelRight.Location = new Point(265, 0);
      this.panelRight.Name = "panelRight";
      this.panelRight.Size = new Size(739, 515);
      this.panelRight.TabIndex = 11;
      this.groupDetail.Controls.Add((Control) this.fbBrowser);
      this.groupDetail.Dock = DockStyle.Fill;
      this.groupDetail.HeaderForeColor = SystemColors.ControlText;
      this.groupDetail.Location = new Point(0, 0);
      this.groupDetail.Name = "groupDetail";
      this.groupDetail.Size = new Size(739, 515);
      this.groupDetail.TabIndex = 3;
      this.groupDetail.Text = "Details";
      this.fbBrowser.DataSource = (IHtmlInput) null;
      this.fbBrowser.Dock = DockStyle.Fill;
      this.fbBrowser.Location = new Point(1, 26);
      this.fbBrowser.Name = "fbBrowser";
      this.fbBrowser.Size = new Size(737, 488);
      this.fbBrowser.TabIndex = 0;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelRight;
      this.collapsibleSplitter1.Dock = DockStyle.Right;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(258, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 12;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.AcceptButton = (IButtonControl) this.selectBtn;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(1004, 555);
      this.Controls.Add((Control) this.panelLeft);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.panelRight);
      this.Controls.Add((Control) this.panelBottom);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (LoanProgramSelect);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Loan Program Template";
      this.Closed += new EventHandler(this.LoanProgramSelect_Closed);
      this.SizeChanged += new EventHandler(this.LoanProgramSelect_SizeChanged);
      this.KeyDown += new KeyEventHandler(this.LoanProgramSelect_KeyDown);
      this.panelBottom.ResumeLayout(false);
      this.panelLeft.ResumeLayout(false);
      this.borderPanelLeft.ResumeLayout(false);
      this.gbOptions.ResumeLayout(false);
      this.panelRight.ResumeLayout(false);
      this.groupDetail.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public LoanProgram SelectedProgram => this.lp;

    public FileSystemEntry SelectedEntry => this.selectedEntry;

    public FileSystemEntry[] SelectedEntries
    {
      get
      {
        List<FileSystemEntry> fileSystemEntryList = new List<FileSystemEntry>();
        foreach (ExplorerListItem selectedItem in this.fsExplorer.GetSelectedItems())
        {
          if (selectedItem.FileFolderEntry.Type == FileSystemEntry.Types.File)
            fileSystemEntryList.Add(selectedItem.FileFolderEntry);
        }
        return fileSystemEntryList.ToArray();
      }
    }

    protected void selectBtn_Click(object sender, EventArgs e)
    {
      if (this.selectType == LoanProgramSelect.SelectTypes.ForTrade)
        this.processSelectionForTrade();
      else
        this.processSelectionForLoan();
    }

    private void processSelectionForTrade()
    {
      foreach (ExplorerListItem selectedItem in this.fsExplorer.GetSelectedItems())
      {
        if (selectedItem.FileFolderEntry.Type == FileSystemEntry.Types.File)
        {
          this.DialogResult = DialogResult.OK;
          return;
        }
      }
      int num = (int) Utils.Dialog((IWin32Window) this, "You must select a loan program.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void processSelectionForLoan()
    {
      if (this.lp == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a loan program.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string simpleField = this.loanMgr.LoanData.GetSimpleField("1172");
        using (CursorActivator.Wait())
        {
          this.loanMgr.LoanData.VALoanValidation = true;
          if (this.lp.ClosingCostPath != "" && !this.validateClosingCostTemplate(this.lp.ClosingCostPath))
            return;
          if (!this.applyLoanProgram())
            return;
        }
        if (this.TemplateApplied != null)
          this.TemplateApplied((object) new object[4]
          {
            (object) simpleField,
            (object) this.loanMgr.LoanData.GetSimpleField("1172"),
            (object) this.loanMgr.LoanData.GetSimpleField("3894"),
            (object) this.lp
          }, EventArgs.Empty);
        Cursor.Current = Cursors.Default;
        this.DialogResult = DialogResult.OK;
      }
    }

    private bool applyLoanProgram()
    {
      if (this.selectType != LoanProgramSelect.SelectTypes.ForLockRequest && !new BusinessRuleCheck().SkipReadOnlyFields((object) this.lp, this.loanMgr.LoanData))
        return false;
      if (this.selectType == LoanProgramSelect.SelectTypes.ForLockRequest)
      {
        if (this.closingCostTemplate != null)
          this.loanMgr.LoanData.CopyClosingCostToLockRequest(this.closingCostTemplate, this.chkAppendCC.Checked);
        this.loanMgr.LoanData.CopyLoanProgramToLockRequest(this.lp, this.chkAppendLP.Checked);
        this.loanMgr.LoanData.SetField("2866", this.removeTemplatePath(this.lpName));
        this.loanMgr.LoanData.SetField("2967", this.lpFullName);
      }
      else
      {
        this.loanMgr.ApplyLoanProgram(this.selectedEntry, this.lp, this.chkAppendLP.Checked, this.chkAppendCC.Checked, false);
        if (this.loanMgr != null && !this.loanMgr.LoanData.VALoanValidation)
        {
          int num = (int) Utils.Dialog((IWin32Window) this.session.Application, this.loanMgr.LoanData.VALoanWarningMessage, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.loanMgr.LoanData.VALoanValidation = true;
        }
      }
      return true;
    }

    private bool validateClosingCostTemplate(string path)
    {
      FileSystemEntry fileEntry = FileSystemEntry.Parse(path, this.session.UserID);
      if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost, fileEntry))
        return Utils.Dialog((IWin32Window) this, "The Closing Cost Template associated with this Loan Program, " + fileEntry.Name + ", cannot be found. Would you like to apply the Loan Program without the Closing Cost data?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
      this.closingCostTemplate = (ClosingCost) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost, fileEntry);
      if (this.closingCostTemplate.RESPAVersion == "2015" && !Utils.CheckIf2015RespaTila(Session.LoanData.GetField("3969")))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Closing Cost Template attached to loan Program is for 2015 Itemization but current loan is for 2010 Itemization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.closingCostTemplate.RESPAVersion == "2010" && Utils.CheckIf2015RespaTila(Session.LoanData.GetField("3969")))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Closing Cost Template attached to loan Program is for 2010 Itemization but current loan is for 2015 Itemization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.closingCostTemplate == null)
        return Utils.Dialog((IWin32Window) this, "The Closing Cost Template associated with this Loan Program, " + fileEntry.Name + ", cannot be found. Would you like to apply the Loan Program without the Closing Cost data?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
      return new BusinessRuleCheck().SkipReadOnlyFields((object) this.closingCostTemplate, this.loanMgr.LoanData) && (this.selectType == LoanProgramSelect.SelectTypes.ForLockRequest || this.loanMgr.SystemConfiguration == null || this.loanMgr.SystemConfiguration.LoanOfficerCompensationSetting == null || LOCompensationInputHandler.CheckLOCompRuleConfliction(this.loanMgr.SystemConfiguration.LoanOfficerCompensationSetting, (IHtmlInput) this.closingCostTemplate, (string) null, (string) null, (string) null, false));
    }

    private string removeTemplatePath(string name)
    {
      int num = name.LastIndexOf("\\");
      return num == -1 ? name : name.Substring(num + 1);
    }

    private void clearDetailsPanel() => this.fbBrowser.Visible = false;

    protected void fsExplorer_SelectedEntryChanged(object sender, EventArgs e)
    {
      this.selectedEntry = (FileSystemEntry) null;
      this.lp = (LoanProgram) null;
      this.closingCostTemplate = (ClosingCost) null;
      ExplorerListItem[] selectedItems = this.fsExplorer.GetSelectedItems();
      if (selectedItems.Length == 0)
        return;
      FileSystemEntry fileFolderEntry = selectedItems[0].FileFolderEntry;
      if (selectedItems.Length > 1 || fileFolderEntry.Type != FileSystemEntry.Types.File)
      {
        this.clearDetailsPanel();
      }
      else
      {
        this.selectedEntry = fileFolderEntry;
        if (fileFolderEntry.IsPublic)
        {
          this.lpName = "Public:" + fileFolderEntry.Path;
          this.lpFullName = this.lpName;
        }
        else
        {
          this.lpName = "Personal:" + fileFolderEntry.Path;
          this.lpFullName = fileFolderEntry.ParentFolder.ToString() + fileFolderEntry.Name;
        }
        if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram, fileFolderEntry))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The template has been deleted or template file format is not correct.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.lp = (LoanProgram) ((LoanProgram) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram, fileFolderEntry)).Clone();
          try
          {
            Plan.Synchronize(Session.SessionObjects, this.lp);
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanProgramSelect.sw, nameof (LoanProgramSelect), TraceLevel.Warning, "Failed to sync plan code for loan program '" + this.lp.TemplateName + "': " + (object) ex);
          }
          if (this.lp.ClosingCostPath == "")
          {
            this.chkAppendCC.Checked = false;
            this.chkAppendCC.Enabled = false;
          }
          else
          {
            this.chkAppendCC.Checked = (bool) this.session.StartupInfo.PolicySettings[(object) "Policies.AppendClosingCost"];
            this.chkAppendCC.Enabled = true;
          }
          InputFormInfo formInfo = new InputFormInfo("LPDETAILS", "Loan Program Details", InputFormType.Standard);
          this.fbBrowser.Visible = true;
          this.fbBrowser.DataSource = (IHtmlInput) this.lp;
          this.fbBrowser.OpenForm(formInfo);
        }
      }
    }

    private void fsExplorer_EntryDoubleClick(object sender, FileSystemEventArgs e)
    {
      if (e.FileSystemEntry.Type != FileSystemEntry.Types.File)
        return;
      this.selectBtn_Click((object) null, (EventArgs) null);
    }

    private void LoanProgramSelect_Closed(object sender, EventArgs e)
    {
      try
      {
        this.fileSystem.SetLastVisitedFolder(this.fsExplorer.CurrentFolder);
      }
      catch
      {
      }
    }

    private void LoanProgramSelect_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Loan Programs");
    }

    private void LoanProgramSelect_SizeChanged(object sender, EventArgs e)
    {
      if (this.selectType == LoanProgramSelect.SelectTypes.ForTrade)
        return;
      if (this.WindowState == FormWindowState.Maximized)
        this.panelLeft.Width = (int) ((double) this.Width * 0.3);
      else
        this.panelLeft.Width = (int) ((double) this.Width * 0.75);
    }

    public enum SelectTypes
    {
      ForLoan,
      ForLockRequest,
      ForTrade,
    }
  }
}
