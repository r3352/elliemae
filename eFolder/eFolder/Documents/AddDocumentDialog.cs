// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.AddDocumentDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class AddDocumentDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private DocumentLog[] docList;
    private IContainer components;
    private RadioButton rdoNew;
    private RadioButton rdoSet;
    private Button btnOK;
    private Button btnCancel;

    public AddDocumentDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
    }

    public DocumentLog[] Documents => this.docList;

    public bool ShowDocumentDetails => this.rdoNew.Checked;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.rdoNew.Checked)
      {
        this.addDocument();
      }
      else
      {
        if (!this.rdoSet.Checked)
          return;
        this.addDocumentSet();
      }
    }

    private void addDocument()
    {
      LoanData loanData = this.loanDataMgr.LoanData;
      LogList logList = loanData.GetLogList();
      DocumentLog rec = new DocumentLog(Session.UserID, loanData.PairId);
      rec.Title = "Untitled";
      rec.Stage = logList.NextStage;
      logList.AddRecord((LogRecordBase) rec);
      this.docList = new DocumentLog[1]{ rec };
      this.DialogResult = DialogResult.OK;
    }

    private void addDocumentSet()
    {
      this.docList = Session.Application.GetService<IEFolder>().AppendDocumentSet(this.loanDataMgr);
      if (this.docList != null)
        this.DialogResult = DialogResult.OK;
      else
        this.DialogResult = DialogResult.Cancel;
    }

    private void AddDocumentDialog_VisibleChanged(object sender, EventArgs e)
    {
      if (this.DialogResult != DialogResult.OK || this.Visible || !this.ShowDocumentDetails)
        return;
      DocumentDetailsDialog.ShowInstance(this.loanDataMgr, this.Documents[0]);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.rdoNew = new RadioButton();
      this.rdoSet = new RadioButton();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.rdoNew.AutoSize = true;
      this.rdoNew.Checked = true;
      this.rdoNew.Location = new Point(12, 12);
      this.rdoNew.Name = "rdoNew";
      this.rdoNew.Size = new Size(129, 18);
      this.rdoNew.TabIndex = 0;
      this.rdoNew.TabStop = true;
      this.rdoNew.Text = "Add a new document";
      this.rdoNew.UseVisualStyleBackColor = true;
      this.rdoSet.AutoSize = true;
      this.rdoSet.Location = new Point(12, 36);
      this.rdoSet.Name = "rdoSet";
      this.rdoSet.Size = new Size(202, 18);
      this.rdoSet.TabIndex = 1;
      this.rdoSet.Text = "Add documents from Document Sets";
      this.rdoSet.UseVisualStyleBackColor = true;
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
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(292, 146);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.rdoSet);
      this.Controls.Add((Control) this.rdoNew);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddDocumentDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Document";
      this.VisibleChanged += new EventHandler(this.AddDocumentDialog_VisibleChanged);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
