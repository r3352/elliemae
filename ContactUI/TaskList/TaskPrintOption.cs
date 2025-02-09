// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.TaskList.TaskPrintOption
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.TaskList
{
  public class TaskPrintOption : Form
  {
    private TaskPrintOption.PrintOption selectedOption = TaskPrintOption.PrintOption.Cancel;
    private IContainer components;
    private Label label1;
    private RadioButton rdoPrintList;
    private RadioButton rdoPrintDetail;
    private Button btnPrint;
    private Button btnPreview;
    private Button btnCancel;

    public TaskPrintOption() => this.InitializeComponent();

    public TaskPrintOption.PrintOption SelectedOption => this.selectedOption;

    private void btnPrint_Click(object sender, EventArgs e)
    {
      this.selectedOption = !this.rdoPrintDetail.Checked ? TaskPrintOption.PrintOption.PrintList : TaskPrintOption.PrintOption.PrintDetail;
      this.Close();
    }

    private void btnPreview_Click(object sender, EventArgs e)
    {
      this.selectedOption = !this.rdoPrintDetail.Checked ? TaskPrintOption.PrintOption.PreviewList : TaskPrintOption.PrintOption.PreviewDetail;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.selectedOption = TaskPrintOption.PrintOption.Cancel;
      this.Close();
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
      this.rdoPrintList = new RadioButton();
      this.rdoPrintDetail = new RadioButton();
      this.btnPrint = new Button();
      this.btnPreview = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(228, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Choose a printing option for the selected tasks.";
      this.rdoPrintList.AutoSize = true;
      this.rdoPrintList.Checked = true;
      this.rdoPrintList.Location = new Point(24, 35);
      this.rdoPrintList.Name = "rdoPrintList";
      this.rdoPrintList.Size = new Size(65, 17);
      this.rdoPrintList.TabIndex = 1;
      this.rdoPrintList.TabStop = true;
      this.rdoPrintList.Text = "Print List";
      this.rdoPrintList.UseVisualStyleBackColor = true;
      this.rdoPrintDetail.AutoSize = true;
      this.rdoPrintDetail.Location = new Point(24, 59);
      this.rdoPrintDetail.Name = "rdoPrintDetail";
      this.rdoPrintDetail.Size = new Size(81, 17);
      this.rdoPrintDetail.TabIndex = 2;
      this.rdoPrintDetail.Text = "Print Details";
      this.rdoPrintDetail.UseVisualStyleBackColor = true;
      this.btnPrint.Location = new Point(89, 94);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(75, 23);
      this.btnPrint.TabIndex = 3;
      this.btnPrint.Text = "Print";
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnPreview.Location = new Point(170, 94);
      this.btnPreview.Name = "btnPreview";
      this.btnPreview.Size = new Size(75, 23);
      this.btnPreview.TabIndex = 4;
      this.btnPreview.Text = "Preview";
      this.btnPreview.UseVisualStyleBackColor = true;
      this.btnPreview.Click += new EventHandler(this.btnPreview_Click);
      this.btnCancel.Location = new Point(251, 94);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(341, (int) sbyte.MaxValue);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnPreview);
      this.Controls.Add((Control) this.btnPrint);
      this.Controls.Add((Control) this.rdoPrintDetail);
      this.Controls.Add((Control) this.rdoPrintList);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TaskPrintOption);
      this.Text = "Print";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public enum PrintOption
    {
      PrintDetail,
      PrintList,
      PreviewDetail,
      PreviewList,
      Cancel,
    }
  }
}
