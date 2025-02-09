// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ClosingCostSelect
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ClosingCostSelect : Form, IHelp
  {
    private const string className = "ClosingCostSelect";
    private static string sw = Tracing.SwInputEngine;
    private Button selectBtn;
    private Button cancelBtn;
    private System.ComponentModel.Container components;
    private FSExplorer tempExplorer;
    private const EllieMae.EMLite.ClientServer.TemplateSettingsType templateType = EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost;
    private LoanData loan;
    private CheckBox chkAppendCC;
    private bool isPublicOnly;
    private Sessions.Session session = Session.DefaultInstance;
    private ClosingCost cc;
    private string selectedFile = string.Empty;

    public ClosingCostSelect(IHtmlInput inputData)
    {
      this.loan = (LoanData) null;
      this.InitializeComponent();
      this.tempExplorer.AddNewHUDColumn();
      this.refreshList();
    }

    public ClosingCostSelect(IHtmlInput inputData, bool isPublicOnly)
      : this(inputData, isPublicOnly, Session.DefaultInstance)
    {
    }

    public ClosingCostSelect(IHtmlInput inputData, bool isPublicOnly, Sessions.Session session)
    {
      this.isPublicOnly = isPublicOnly;
      this.loan = (LoanData) null;
      this.session = session;
      this.InitializeComponent();
      this.tempExplorer.AddNewHUDColumn();
      this.refreshList();
    }

    public ClosingCostSelect(LoanData loan)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.tempExplorer.AddNewHUDColumn();
      this.tempExplorer.RESPAMode = this.loan == null ? FSExplorer.RESPAFilter.All : (this.loan.Use2015RESPA ? FSExplorer.RESPAFilter.Respa2015 : (this.loan.Use2010RESPA ? FSExplorer.RESPAFilter.Respa2010 : FSExplorer.RESPAFilter.Respa2009));
      this.refreshList();
    }

    private void refreshList()
    {
      LoanProgramIFSExplorer ifsExplorer = new LoanProgramIFSExplorer(EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost, this.session);
      this.tempExplorer.FileType = FSExplorer.FileTypes.ClosingCosts;
      this.tempExplorer.HideAllButtons = true;
      this.tempExplorer.SetProperties(false, false, true, 2, true);
      FileSystemEntry publicRoot = FileSystemEntry.PublicRoot;
      try
      {
        publicRoot = FileSystemEntry.Parse(this.session.GetPrivateProfileString("ClosingCostTemplate", "LastFolderViewed") ?? "");
        if (!ifsExplorer.EntryExists(publicRoot))
          publicRoot = FileSystemEntry.PublicRoot;
      }
      catch
      {
      }
      this.tempExplorer.Init((IFSExplorerBase) ifsExplorer, publicRoot, !this.getPersonalRight());
      this.chkAppendCC.Checked = (bool) this.session.StartupInfo.PolicySettings[(object) "Policies.AppendClosingCost"];
    }

    private bool getPersonalRight()
    {
      return UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas) || ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_Personal_ClosingCosts);
    }

    public ClosingCost SelectedCost => this.cc;

    public string SelectedFile => this.selectedFile;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.selectBtn = new Button();
      this.cancelBtn = new Button();
      this.tempExplorer = new FSExplorer();
      this.chkAppendCC = new CheckBox();
      this.SuspendLayout();
      this.selectBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.selectBtn.Location = new Point(523, 401);
      this.selectBtn.Name = "selectBtn";
      this.selectBtn.Size = new Size(75, 24);
      this.selectBtn.TabIndex = 2;
      this.selectBtn.Text = "&Select";
      this.selectBtn.Click += new EventHandler(this.selectBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(604, 401);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 3;
      this.cancelBtn.Text = "&Cancel";
      this.tempExplorer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tempExplorer.FolderComboSelectedIndex = -1;
      this.tempExplorer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.Location = new Point(8, 8);
      this.tempExplorer.Name = "tempExplorer";
      this.tempExplorer.RenameButtonSize = new Size(62, 22);
      this.tempExplorer.RESPAMode = FSExplorer.RESPAFilter.All;
      this.tempExplorer.setContactType = ContactType.BizPartner;
      this.tempExplorer.Size = new Size(671, 358);
      this.tempExplorer.TabIndex = 0;
      this.tempExplorer.SelectedCurrentFile += new EventHandler(this.tempExplorer_SelectedCurrentFile);
      this.chkAppendCC.Location = new Point(8, 372);
      this.chkAppendCC.Name = "chkAppendCC";
      this.chkAppendCC.Size = new Size(260, 20);
      this.chkAppendCC.TabIndex = 1;
      this.chkAppendCC.Text = "Only apply template fields that contain a value";
      this.AcceptButton = (IButtonControl) this.selectBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(691, 437);
      this.Controls.Add((Control) this.chkAppendCC);
      this.Controls.Add((Control) this.tempExplorer);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.selectBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ClosingCostSelect);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Closing Cost Template";
      this.Closed += new EventHandler(this.ClosingCostSelect_Closed);
      this.KeyDown += new KeyEventHandler(this.ClosingCostSelect_KeyDown);
      this.ResumeLayout(false);
    }

    private void selectBtn_Click(object sender, EventArgs e)
    {
      if (this.tempExplorer.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You need to select a closing cost template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FileSystemEntry tag = (FileSystemEntry) this.tempExplorer.SelectedItems[0].Tag;
        if (tag.Type != FileSystemEntry.Types.File)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You need to select a closing cost template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost, tag))
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The template has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          BinaryObject binaryObject = (BinaryObject) null;
          try
          {
            binaryObject = this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost, tag);
          }
          catch (Exception ex)
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, "Encompass can't load " + tag.Name + " closing cost template. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            Tracing.Log(ClosingCostSelect.sw, TraceLevel.Error, nameof (ClosingCostSelect), "Can't load " + (object) tag + " template. Error: " + ex.Message);
          }
          if (binaryObject == null)
            return;
          try
          {
            this.cc = (ClosingCost) binaryObject;
          }
          catch (Exception ex)
          {
            Tracing.Log(ClosingCostSelect.sw, TraceLevel.Error, nameof (ClosingCostSelect), "Can't load " + (object) this.cc + "template. Error: " + ex.Message);
            return;
          }
          if (this.cc == null)
            return;
          this.selectedFile = tag.ToDisplayString();
          if (this.loan != null)
          {
            if (this.cc.RESPAVersion == "2015" && !Utils.CheckIf2015RespaTila(this.loan.GetField("3969")) || this.cc.RESPAVersion == "2010" && this.loan.GetField("3969") != "RESPA 2010 GFE and HUD-1" || this.cc.RESPAVersion == "2009" && this.loan.GetField("3969") != "Old GFE and HUD-1")
            {
              int num5 = (int) Utils.Dialog((IWin32Window) this, "The Closing Cost Template you selected is for " + this.cc.RESPAVersion + " Itemization but current loan is not for " + this.cc.RESPAVersion + " Itemization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            if (this.cc.RESPAVersion == "" || this.loan.GetField("3969") == "")
            {
              if (this.loan.GetField("NEWHUD.X354") != "Y" && this.cc.For2010GFE)
              {
                int num6 = (int) Utils.Dialog((IWin32Window) this, "The Closing Cost Template you selected is for new Itemization 2010 but current loan is for old GFE Itemization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
              if (this.loan.GetField("NEWHUD.X354") == "Y" && !this.cc.For2010GFE)
              {
                int num7 = (int) Utils.Dialog((IWin32Window) this, "The Closing Cost Template you selected is for old GFE Itemization but current loan is for New Itemization 2010.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
            }
            if (!new BusinessRuleCheck().SkipReadOnlyFields((object) this.cc, this.loan))
              return;
            Cursor.Current = Cursors.WaitCursor;
            this.loan.VALoanValidation = true;
            this.loan.SelectClosingCostProgram(this.cc, this.chkAppendCC.Checked);
            string path = tag.Path;
            string str = !tag.IsPublic ? "Personal:" + path : "Public:" + path;
            if (!this.loan.IsFieldReadOnly("1785"))
              this.loan.SetCurrentField("1785", this.removeTemplatePath(str));
            this.loan.SetCurrentField("2862", str);
            if (this.session.LoanDataMgr.SystemConfiguration != null && this.session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting != null)
              LOCompensationInputHandler.CheckLOCompRuleConfliction(this.session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting, (IHtmlInput) this.loan, (string) null, (string) null, (string) null, true);
            this.loan.Calculator.CalculateAll();
            if (this.loan.GetField("CPA.RetainUserInputs") != "Y")
              this.loan.Calculator.FormCalculation("CPA_ESCROWDETAILS", (string) null, (string) null);
            if (this.loan != null && !this.loan.VALoanValidation)
            {
              int num8 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, this.loan.VALoanWarningMessage, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              this.loan.VALoanValidation = true;
            }
            Cursor.Current = Cursors.Default;
          }
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private string removeTemplatePath(string name)
    {
      int num = name.LastIndexOf("\\");
      return num == -1 ? name : name.Substring(num + 1);
    }

    private void tempExplorer_SelectedCurrentFile(object sender, EventArgs e)
    {
      this.selectBtn_Click((object) null, (EventArgs) null);
    }

    private void ClosingCostSelect_Closed(object sender, EventArgs e)
    {
      try
      {
        this.session.WritePrivateProfileString("ClosingCostTemplate", "LastFolderViewed", this.tempExplorer.CurrentFolder.ToString());
      }
      catch (Exception ex)
      {
      }
    }

    private void ClosingCostSelect_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Closing Costs");
    }
  }
}
