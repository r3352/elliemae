// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SelectVerificationDocumentDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SelectVerificationDocumentDialog : Form
  {
    private DocumentLog docLog;
    private IContainer components;
    private Button btnOK;
    private Button cancelBtn;
    private ComboBox cboDocs;

    public SelectVerificationDocumentDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      foreach (object allDocument in loanDataMgr.LoanData.GetLogList().GetAllDocuments())
        this.cboDocs.Items.Add(allDocument);
      if (this.cboDocs.Items.Count <= 0)
        return;
      this.cboDocs.SelectedIndex = 0;
    }

    public DocumentLog Document => this.docLog;

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.docLog = (DocumentLog) this.cboDocs.SelectedItem;
      if (this.docLog == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a document", MessageBoxButtons.OK, MessageBoxIcon.None);
      }
      else
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
      this.cancelBtn = new Button();
      this.cboDocs = new ComboBox();
      this.SuspendLayout();
      this.btnOK.Location = new Point(93, 71);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 17;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(173, 71);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 18;
      this.cancelBtn.Text = "Cancel";
      this.cboDocs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboDocs.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDocs.FormattingEnabled = true;
      this.cboDocs.Location = new Point(12, 24);
      this.cboDocs.Name = "cboDocs";
      this.cboDocs.Size = new Size(239, 21);
      this.cboDocs.Sorted = true;
      this.cboDocs.TabIndex = 20;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(260, 106);
      this.Controls.Add((Control) this.cboDocs);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.cancelBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectVerificationDocumentDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Document";
      this.ResumeLayout(false);
    }
  }
}
