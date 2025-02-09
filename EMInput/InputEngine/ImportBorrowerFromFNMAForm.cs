// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ImportBorrowerFromFNMAForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.RemotingServices;
using Microsoft.VisualBasic.Compatibility.VB6;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ImportBorrowerFromFNMAForm : Form
  {
    private bool importedFromCoborrower;
    private bool importFromMISMO34;
    private LoanData importedLoan;
    private IContainer components;
    private Button cancelBtn;
    private Button btnImport;
    private Label label1;
    private DriveListBox driveListBox;
    private DirListBox dirListBox;
    private FileListBox fileListBox;

    public ImportBorrowerFromFNMAForm(bool importedFromCoborrower, bool importFromMISMO34)
    {
      this.importedFromCoborrower = importedFromCoborrower;
      this.importFromMISMO34 = importFromMISMO34;
      this.InitializeComponent();
    }

    private void driveListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.dirListBox.Path = this.driveListBox.Drive;
      }
      catch (IOException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Device " + this.driveListBox.Drive + " unavailable.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        string path = this.dirListBox.Path;
        this.driveListBox.Drive = this.dirListBox.Path.Substring(0, this.dirListBox.Path.IndexOf("\\"));
        this.dirListBox.Path = path;
      }
      this.dirListBox_MouseDoubleClick((object) null, (MouseEventArgs) null);
    }

    private void dirListBox_Change(object sender, EventArgs e)
    {
      this.fileListBox.Path = this.dirListBox.Path;
      this.fileListBox.Refresh();
    }

    private void fileListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnImport.Enabled = this.fileListBox.SelectedItems.Count > 0;
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      if (this.dirListBox.SelectedItems.Count == 0 || this.fileListBox.SelectedItems.Count == 0)
        return;
      Cursor.Current = Cursors.WaitCursor;
      FannieImport fannieImport = new FannieImport(Session.SessionObjects, this.importFromMISMO34);
      fannieImport.ImportBorrowerOnly = true;
      LoanDataMgr loanDataMgr = fannieImport.Convert(this.fileListBox.Path + "\\" + this.fileListBox.SelectedItems[0].ToString(), Session.SessionObjects, (string) null);
      Cursor.Current = Cursors.Default;
      if (loanDataMgr == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "File is not in " + (this.importFromMISMO34 ? "ULAD / iLAD (MISMO 3.4)" : "Fannie Mae") + " format.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (fannieImport.ErrorMessage != string.Empty)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, fannieImport.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.importedFromCoborrower && loanDataMgr.LoanData.GetField("68") == "" && loanDataMgr.LoanData.GetField("69") == "")
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "The selected " + (this.importFromMISMO34 ? "ULAD / iLAD (MISMO 3.4)" : "Fannie Mae") + " file doesn't have co-borrower. Please select another Fannie Mae file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.importedLoan = loanDataMgr.LoanData;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void dirListBox_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.fileListBox.Path = this.dirListBox.Path;
      this.fileListBox.Refresh();
    }

    public LoanData ImportedLoan => this.importedLoan;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cancelBtn = new Button();
      this.btnImport = new Button();
      this.label1 = new Label();
      this.driveListBox = new DriveListBox();
      this.dirListBox = new DirListBox();
      this.fileListBox = new FileListBox();
      this.SuspendLayout();
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(441, 364);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 29;
      this.cancelBtn.Text = "&Cancel";
      this.btnImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnImport.Location = new Point(361, 364);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(75, 24);
      this.btnImport.TabIndex = 28;
      this.btnImport.Text = "&Import";
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(152, 16);
      this.label1.TabIndex = 21;
      this.label1.Text = "Fannie Mae Folder:";
      this.driveListBox.FormattingEnabled = true;
      this.driveListBox.Location = new Point(12, 27);
      this.driveListBox.Name = "driveListBox";
      this.driveListBox.Size = new Size(248, 21);
      this.driveListBox.TabIndex = 20;
      this.driveListBox.SelectedIndexChanged += new EventHandler(this.driveListBox_SelectedIndexChanged);
      this.dirListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dirListBox.FormattingEnabled = true;
      this.dirListBox.IntegralHeight = false;
      this.dirListBox.Location = new Point(12, 59);
      this.dirListBox.Name = "dirListBox";
      this.dirListBox.Size = new Size(248, 288);
      this.dirListBox.TabIndex = 22;
      this.dirListBox.MouseDoubleClick += new MouseEventHandler(this.dirListBox_MouseDoubleClick);
      this.fileListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.fileListBox.FormattingEnabled = true;
      this.fileListBox.Location = new Point(273, 59);
      this.fileListBox.Name = "fileListBox";
      this.fileListBox.Pattern = "*.*";
      this.fileListBox.Size = new Size(243, 290);
      this.fileListBox.TabIndex = 23;
      this.fileListBox.SelectedIndexChanged += new EventHandler(this.fileListBox_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(533, 395);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.btnImport);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.driveListBox);
      this.Controls.Add((Control) this.dirListBox);
      this.Controls.Add((Control) this.fileListBox);
      this.MinimizeBox = false;
      this.Name = nameof (ImportBorrowerFromFNMAForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Import Borrower From Fannie Mae";
      this.ResumeLayout(false);
    }
  }
}
