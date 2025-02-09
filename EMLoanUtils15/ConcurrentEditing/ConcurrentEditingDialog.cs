// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ConcurrentEditing.ConcurrentEditingDialog
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ConcurrentEditing
{
  public class ConcurrentEditingDialog : Form
  {
    private ConcurrentEditingDialog.Actions action;
    private IContainer components;
    private Label label1;
    private Button btnCancel;
    private Button btnMergeSave;
    private Button btnMergeAndShowResult;
    private Button btnMerge;

    public ConcurrentEditingDialog() => this.InitializeComponent();

    public ConcurrentEditingDialog.Actions Action => this.action;

    private void btnMergeSave_Click(object sender, EventArgs e)
    {
      this.action = ConcurrentEditingDialog.Actions.MergeAndSave;
    }

    private void btnMerge_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show("Not implemented yet.");
      this.DialogResult = DialogResult.None;
    }

    private void btnOverwrite_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show("Not implemented yet.");
      this.DialogResult = DialogResult.None;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.action = ConcurrentEditingDialog.Actions.Cancel;
    }

    private void btnMergeAndShowResult_Click(object sender, EventArgs e)
    {
      this.action = ConcurrentEditingDialog.Actions.MergeAndShowResult;
    }

    private void btnMerge_Click_1(object sender, EventArgs e)
    {
      this.action = ConcurrentEditingDialog.Actions.Merge;
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
      this.btnCancel = new Button();
      this.btnMergeSave = new Button();
      this.btnMergeAndShowResult = new Button();
      this.btnMerge = new Button();
      this.SuspendLayout();
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(359, 24);
      this.label1.TabIndex = 0;
      this.label1.Text = "The loan has been modified on the server.  Please select an action below.";
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(293, 36);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnMergeSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMergeSave.DialogResult = DialogResult.OK;
      this.btnMergeSave.Location = new Point(196, 36);
      this.btnMergeSave.Name = "btnMergeSave";
      this.btnMergeSave.Size = new Size(96, 23);
      this.btnMergeSave.TabIndex = 2;
      this.btnMergeSave.Text = "Merge && Save";
      this.btnMergeSave.UseVisualStyleBackColor = true;
      this.btnMergeSave.Click += new EventHandler(this.btnMergeSave_Click);
      this.btnMergeAndShowResult.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMergeAndShowResult.DialogResult = DialogResult.OK;
      this.btnMergeAndShowResult.Location = new Point(65, 36);
      this.btnMergeAndShowResult.Name = "btnMergeAndShowResult";
      this.btnMergeAndShowResult.Size = new Size(130, 23);
      this.btnMergeAndShowResult.TabIndex = 1;
      this.btnMergeAndShowResult.Text = "Merge && Show Result";
      this.btnMergeAndShowResult.UseVisualStyleBackColor = true;
      this.btnMergeAndShowResult.Click += new EventHandler(this.btnMergeAndShowResult_Click);
      this.btnMerge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMerge.DialogResult = DialogResult.OK;
      this.btnMerge.Location = new Point(11, 36);
      this.btnMerge.Name = "btnMerge";
      this.btnMerge.Size = new Size(53, 23);
      this.btnMerge.TabIndex = 3;
      this.btnMerge.Text = "Merge";
      this.btnMerge.UseVisualStyleBackColor = true;
      this.btnMerge.Visible = false;
      this.btnMerge.Click += new EventHandler(this.btnMerge_Click_1);
      this.AcceptButton = (IButtonControl) this.btnMergeAndShowResult;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(383, 69);
      this.Controls.Add((Control) this.btnMerge);
      this.Controls.Add((Control) this.btnMergeAndShowResult);
      this.Controls.Add((Control) this.btnMergeSave);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Name = nameof (ConcurrentEditingDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Merge Loan Files";
      this.ResumeLayout(false);
    }

    public enum Actions
    {
      Cancel,
      MergeAndShowResult,
      MergeAndSave,
      Merge,
    }
  }
}
