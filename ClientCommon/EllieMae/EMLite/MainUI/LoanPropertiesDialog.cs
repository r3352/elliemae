// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.LoanPropertiesDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class LoanPropertiesDialog : Form
  {
    private LoanProperties props;
    private IContainer components;
    private Label label1;
    private TextBox txtGuid;
    private Label label2;
    private Label label3;
    private TextBox txtName;
    private TextBox txtFolder;
    private TextBox txtSize;
    private Label label4;
    private Button btnOK;
    private Button btnRebuild;
    private Button btnExport;
    private SaveFileDialog saveFileDialog1;
    private TextBox txtStorage;
    private Label label5;

    public LoanPropertiesDialog(LoanProperties props)
    {
      this.InitializeComponent();
      this.props = props;
      this.txtGuid.Text = props.Identity.Guid;
      this.txtFolder.Text = props.Identity.LoanFolder;
      this.txtName.Text = props.Identity.LoanName;
      this.txtSize.Text = props.Size.ToString("#,##0");
      if (string.IsNullOrEmpty(props.Storage))
      {
        using (ILoan loan = Session.LoanManager.OpenLoan(props.Identity))
        {
          LoanProperty loanProperty = ((IEnumerable<LoanProperty>) loan.GetLoanPropertySettings()).FirstOrDefault<LoanProperty>((Func<LoanProperty, bool>) (x => x.Category.Equals("LoanStorage", StringComparison.OrdinalIgnoreCase) && x.Attribute.Equals("SupportingData", StringComparison.OrdinalIgnoreCase)));
          props.Storage = loanProperty == null ? "CIFS" : loanProperty.Value;
        }
      }
      switch (props.Storage.ToLower())
      {
        case "skydrive":
          this.txtStorage.Text = "SD";
          break;
        case "skydrivelite":
          this.txtStorage.Text = "SDL";
          break;
        case "skydriveclassic":
          this.txtStorage.Text = "SDC";
          break;
        default:
          this.txtStorage.Text = "CIFS";
          break;
      }
      this.btnRebuild.Visible = Session.UserInfo.IsAdministrator();
      this.checkExportStatus();
    }

    private void btnRebuild_Click(object sender, EventArgs e)
    {
      try
      {
        using (CursorActivator.Wait())
          Session.LoanManager.RebuildLoan(this.props.Identity.LoanFolder, this.props.Identity.LoanName, DatabaseToRebuild.Both);
        int num = (int) Utils.Dialog((IWin32Window) this, "The loan has been rebuilt successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The attempt to rebuild this loan has failed due to error (" + ex.Message + "). View the Encompass Server log file for additional information.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void checkExportStatus()
    {
      try
      {
        RegistryKey registryKey1 = (RegistryKey) null;
        RegistryKey registryKey2 = (RegistryKey) null;
        RegistryKey registryKey3 = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass");
        RegistryKey registryKey4 = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass");
        object obj1 = registryKey3.GetValue("AllowExport");
        object obj2 = registryKey4.GetValue("AllowExport");
        if (obj1 != null)
          this.btnExport.Visible = true;
        else if (obj2 != null)
          this.btnExport.Visible = true;
        else
          this.btnExport.Visible = false;
        registryKey1 = (RegistryKey) null;
        registryKey2 = (RegistryKey) null;
      }
      catch
      {
      }
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      if (this.saveFileDialog1.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      ILoanConsole service = Session.Application.GetService<ILoanConsole>();
      if (!service.OpenLoan(this.props.Identity.Guid, false))
        return;
      Session.LoanData.IncludeSnapshotInXML = true;
      Session.LoanData.GetSnapshotDataForAllDisclosureTracking2015LogsForLoan();
      using (FileStream destination = File.Open(this.saveFileDialog1.FileName, FileMode.Create, FileAccess.ReadWrite))
      {
        Session.LoanData.Dirty = true;
        using (Stream stream = Session.LoanData.ToStream())
          stream.CopyTo((Stream) destination);
        Session.LoanData.Dirty = false;
      }
      Session.LoanData.IncludeSnapshotInXML = false;
      service.CloseLoan(false);
      int num = (int) Utils.Dialog((IWin32Window) this, "Done");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.txtGuid = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.txtName = new TextBox();
      this.txtFolder = new TextBox();
      this.txtSize = new TextBox();
      this.label4 = new Label();
      this.btnOK = new Button();
      this.btnRebuild = new Button();
      this.btnExport = new Button();
      this.saveFileDialog1 = new SaveFileDialog();
      this.txtStorage = new TextBox();
      this.label5 = new Label();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(34, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "GUID:";
      this.txtGuid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtGuid.BackColor = System.Drawing.Color.WhiteSmoke;
      this.txtGuid.BorderStyle = BorderStyle.None;
      this.txtGuid.Location = new Point(87, 14);
      this.txtGuid.Name = "txtGuid";
      this.txtGuid.ReadOnly = true;
      this.txtGuid.Size = new Size(227, 13);
      this.txtGuid.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(67, 14);
      this.label2.TabIndex = 2;
      this.label2.Text = "Loan Folder:";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(7, 58);
      this.label3.Name = "label3";
      this.label3.Size = new Size(64, 14);
      this.label3.TabIndex = 4;
      this.label3.Text = "Loan Name:";
      this.txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtName.BackColor = System.Drawing.Color.WhiteSmoke;
      this.txtName.BorderStyle = BorderStyle.None;
      this.txtName.Location = new Point(87, 58);
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(227, 13);
      this.txtName.TabIndex = 5;
      this.txtFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFolder.BackColor = System.Drawing.Color.WhiteSmoke;
      this.txtFolder.BorderStyle = BorderStyle.None;
      this.txtFolder.Location = new Point(87, 36);
      this.txtFolder.Name = "txtFolder";
      this.txtFolder.ReadOnly = true;
      this.txtFolder.Size = new Size(227, 13);
      this.txtFolder.TabIndex = 3;
      this.txtSize.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSize.BackColor = System.Drawing.Color.WhiteSmoke;
      this.txtSize.BorderStyle = BorderStyle.None;
      this.txtSize.Location = new Point(87, 80);
      this.txtSize.Name = "txtSize";
      this.txtSize.ReadOnly = true;
      this.txtSize.Size = new Size(227, 13);
      this.txtSize.TabIndex = 7;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(7, 80);
      this.label4.Name = "label4";
      this.label4.Size = new Size(49, 14);
      this.label4.TabIndex = 6;
      this.label4.Text = "Size (B):";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(237, 142);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 12;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnRebuild.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnRebuild.DialogResult = DialogResult.OK;
      this.btnRebuild.Location = new Point(9, 142);
      this.btnRebuild.Name = "btnRebuild";
      this.btnRebuild.Size = new Size(75, 22);
      this.btnRebuild.TabIndex = 10;
      this.btnRebuild.Text = "&Rebuild";
      this.btnRebuild.UseVisualStyleBackColor = true;
      this.btnRebuild.Click += new EventHandler(this.btnRebuild_Click);
      this.btnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnExport.Location = new Point(90, 142);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(75, 23);
      this.btnExport.TabIndex = 11;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.saveFileDialog1.Filter = "All file|*.xml";
      this.txtStorage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtStorage.BackColor = System.Drawing.Color.WhiteSmoke;
      this.txtStorage.BorderStyle = BorderStyle.None;
      this.txtStorage.Location = new Point(87, 104);
      this.txtStorage.Name = "txtStorage";
      this.txtStorage.ReadOnly = true;
      this.txtStorage.Size = new Size(227, 13);
      this.txtStorage.TabIndex = 9;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(7, 104);
      this.label5.Name = "label5";
      this.label5.Size = new Size(48, 14);
      this.label5.TabIndex = 8;
      this.label5.Text = "Storage:";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnOK;
      this.ClientSize = new Size(325, 176);
      this.Controls.Add((Control) this.txtStorage);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.btnExport);
      this.Controls.Add((Control) this.btnRebuild);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtSize);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.txtFolder);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtGuid);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanPropertiesDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Loan Properties";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
