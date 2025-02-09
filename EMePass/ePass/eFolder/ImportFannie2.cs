// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.eFolder.ImportFannie2
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass.eFolder
{
  public class ImportFannie2 : Form
  {
    private const string className = "ImportFannie2";
    private static readonly string sw = Tracing.SwImportExport;
    private string filepath;
    private LoanTemplateSelection loanTemplate;
    private LoanDataMgr loanDataMgr;
    private IContainer components;
    private StandardIconButton btnClear;
    private StandardIconButton btnBrowse;
    private Label lblTemplate;
    private TextBox txtTemplate;
    private Button btnCancel;
    private Button btnImport;
    private ComboBox cboFolder;
    private Label lblFolder;

    public ImportFannie2(string filepath)
    {
      this.InitializeComponent();
      this.filepath = filepath;
      this.loadFolderList();
    }

    public LoanDataMgr LoanDataMgr => this.loanDataMgr;

    private void loadFolderList()
    {
      this.cboFolder.Items.AddRange((object[]) ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetLoanFoldersForAction(LoanFolderAction.Import));
      this.cboFolder.SelectedItem = (object) Session.UserInfo.WorkingFolder;
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      using (LoanTemplateSelectDialog templateSelectDialog = new LoanTemplateSelectDialog(Session.DefaultInstance, false, aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateBlank), aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateFromTmpl)))
      {
        if (templateSelectDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        FileSystemEntry selectedItem = templateSelectDialog.SelectedItem;
        if (selectedItem != null)
        {
          this.loanTemplate = new LoanTemplateSelection(selectedItem, templateSelectDialog.AppendData);
          this.txtTemplate.Text = this.loanTemplate.TemplateEntry.Name;
        }
        else
        {
          this.loanTemplate = (LoanTemplateSelection) null;
          this.txtTemplate.Text = string.Empty;
        }
      }
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.loanTemplate = (LoanTemplateSelection) null;
      this.txtTemplate.Text = string.Empty;
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      string selectedItem = (string) this.cboFolder.SelectedItem;
      if (selectedItem == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to choose a destination folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FannieImport fannieImport = this.loanTemplate == null ? new FannieImport(Session.SessionObjects) : new FannieImport(this.loanTemplate);
        try
        {
          fannieImport.UseEMLoanNumbering = (EnableDisableSetting) Session.ServerManager.GetServerSetting("Import.LoanNumbering") == EnableDisableSetting.Enabled;
        }
        catch (Exception ex)
        {
          Tracing.Log(ImportFannie2.sw, TraceLevel.Error, nameof (ImportFannie2), "Problem reading system setting \"Import.LoanNumbering\", exception: " + ex.Message);
        }
        try
        {
          fannieImport.EnforceApplicationDate = (EnableDisableSetting) Session.ServerManager.GetServerSetting("Import.EnforceApplicationDate") == EnableDisableSetting.Enabled;
        }
        catch (Exception ex)
        {
          Tracing.Log(ImportFannie2.sw, TraceLevel.Error, nameof (ImportFannie2), "Problem reading system setting \"Import.EnforceApplicationDate\", exception: " + ex.Message);
        }
        this.loanDataMgr = fannieImport.Convert(this.filepath, Session.SessionObjects, string.Empty);
        if (this.loanDataMgr == null)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Your web application failed to be imported.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.loanDataMgr.FromLoanImport = true;
          this.loanDataMgr.Create(selectedItem, string.Empty);
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnClear = new StandardIconButton();
      this.btnBrowse = new StandardIconButton();
      this.lblTemplate = new Label();
      this.txtTemplate = new TextBox();
      this.btnCancel = new Button();
      this.btnImport = new Button();
      this.cboFolder = new ComboBox();
      this.lblFolder = new Label();
      ((ISupportInitialize) this.btnClear).BeginInit();
      ((ISupportInitialize) this.btnBrowse).BeginInit();
      this.SuspendLayout();
      this.btnClear.BackColor = Color.Transparent;
      this.btnClear.Location = new Point(328, 82);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(16, 16);
      this.btnClear.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnClear.TabIndex = 33;
      this.btnClear.TabStop = false;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.btnBrowse.BackColor = Color.Transparent;
      this.btnBrowse.Location = new Point(308, 82);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new Size(16, 16);
      this.btnBrowse.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnBrowse.TabIndex = 32;
      this.btnBrowse.TabStop = false;
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      this.lblTemplate.AutoSize = true;
      this.lblTemplate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblTemplate.Location = new Point(12, 63);
      this.lblTemplate.Name = "lblTemplate";
      this.lblTemplate.Size = new Size(74, 14);
      this.lblTemplate.TabIndex = 2;
      this.lblTemplate.Text = "Loan template";
      this.txtTemplate.Location = new Point(12, 80);
      this.txtTemplate.Name = "txtTemplate";
      this.txtTemplate.ReadOnly = true;
      this.txtTemplate.Size = new Size(292, 20);
      this.txtTemplate.TabIndex = 3;
      this.txtTemplate.Tag = (object) "";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnCancel.Location = new Point(272, 116);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnImport.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnImport.Location = new Point(196, 116);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(75, 22);
      this.btnImport.TabIndex = 4;
      this.btnImport.Text = "Import";
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.cboFolder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFolder.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboFolder.Location = new Point(12, 28);
      this.cboFolder.Name = "cboFolder";
      this.cboFolder.Size = new Size(333, 22);
      this.cboFolder.TabIndex = 1;
      this.lblFolder.AutoSize = true;
      this.lblFolder.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblFolder.Location = new Point(12, 12);
      this.lblFolder.Name = "lblFolder";
      this.lblFolder.Size = new Size(311, 14);
      this.lblFolder.TabIndex = 0;
      this.lblFolder.Text = "Select the loan folder where you want to import the Application";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(358, 149);
      this.Controls.Add((Control) this.btnClear);
      this.Controls.Add((Control) this.btnBrowse);
      this.Controls.Add((Control) this.lblTemplate);
      this.Controls.Add((Control) this.txtTemplate);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnImport);
      this.Controls.Add((Control) this.cboFolder);
      this.Controls.Add((Control) this.lblFolder);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportFannie2);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import";
      ((ISupportInitialize) this.btnClear).EndInit();
      ((ISupportInitialize) this.btnBrowse).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
