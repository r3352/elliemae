// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.PrintDocumentsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.JedScriptEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class PrintDocumentsDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private DocumentLog[] docList;
    private FileSystemEntry defaultStackingEntry;
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private RadioButton rdoDetails;
    private RadioButton rdoFiles;

    public PrintDocumentsDialog(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      FileSystemEntry defaultStackingEntry)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.docList = docList;
      this.defaultStackingEntry = defaultStackingEntry;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.rdoFiles.Checked)
      {
        this.printFiles();
      }
      else
      {
        if (!this.rdoDetails.Checked)
          return;
        this.printDetails();
      }
    }

    private void printFiles()
    {
      using (SelectDocumentsDialog selectDocumentsDialog = new SelectDocumentsDialog(this.loanDataMgr, this.loanDataMgr.LoanData.GetLogList().GetAllDocuments(), this.docList, this.defaultStackingEntry, SelectDocumentsReasonType.PrintSave))
      {
        if (selectDocumentsDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(this.loanDataMgr))
        {
          string file = pdfFileBuilder.CreateFile(selectDocumentsDialog.Documents);
          if (file == null)
            return;
          using (PdfPrintDialog pdfPrintDialog = new PdfPrintDialog(file))
          {
            if (pdfPrintDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
              return;
          }
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    private void printDetails()
    {
      string filepath = new FormExport(this.loanDataMgr).ExportDetails(this.docList);
      if (filepath == null)
        return;
      using (PdfPreviewDialog pdfPreviewDialog = new PdfPreviewDialog(filepath, true, true, false))
      {
        if (pdfPreviewDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
      }
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.rdoDetails = new RadioButton();
      this.rdoFiles = new RadioButton();
      this.SuspendLayout();
      this.btnOK.Location = new Point(132, 116);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(208, 116);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.rdoDetails.AutoSize = true;
      this.rdoDetails.Location = new Point(12, 36);
      this.rdoDetails.Name = "rdoDetails";
      this.rdoDetails.Size = new Size(130, 18);
      this.rdoDetails.TabIndex = 1;
      this.rdoDetails.Text = "Print document details";
      this.rdoDetails.UseVisualStyleBackColor = true;
      this.rdoFiles.AutoSize = true;
      this.rdoFiles.Checked = true;
      this.rdoFiles.Location = new Point(12, 12);
      this.rdoFiles.Name = "rdoFiles";
      this.rdoFiles.Size = new Size(114, 18);
      this.rdoFiles.TabIndex = 0;
      this.rdoFiles.TabStop = true;
      this.rdoFiles.Text = "Print attached files";
      this.rdoFiles.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(292, 146);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.rdoDetails);
      this.Controls.Add((Control) this.rdoFiles);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PrintDocumentsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Print Documents";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
