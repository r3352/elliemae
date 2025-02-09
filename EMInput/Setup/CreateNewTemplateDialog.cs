// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CreateNewTemplateDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CreateNewTemplateDialog : Form
  {
    private IContainer components;
    private Button btnOK;
    private Label labelQuestion;
    private RadioButton rdo2015;
    private RadioButton rdo2010;
    private Button btnCancel;

    public CreateNewTemplateDialog(bool isForFunding)
    {
      this.InitializeComponent();
      if (isForFunding)
      {
        this.labelQuestion.Text = "The Funding Template will apply to:";
        this.Text = "Funding Template";
      }
      this.rdo2015.Checked = true;
    }

    public bool Use2010 => this.rdo2010.Checked;

    public bool Use2015 => this.rdo2015.Checked;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.labelQuestion = new Label();
      this.rdo2015 = new RadioButton();
      this.rdo2010 = new RadioButton();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(76, 109);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.labelQuestion.AutoSize = true;
      this.labelQuestion.Location = new Point(24, 20);
      this.labelQuestion.Name = "labelQuestion";
      this.labelQuestion.Size = new Size(194, 13);
      this.labelQuestion.TabIndex = 2;
      this.labelQuestion.Text = "The Closing Cost Template will apply to:";
      this.rdo2015.AutoSize = true;
      this.rdo2015.Location = new Point(27, 45);
      this.rdo2015.Name = "rdo2015";
      this.rdo2015.Size = new Size(105, 17);
      this.rdo2015.TabIndex = 3;
      this.rdo2015.Text = "2015 Itemization ";
      this.rdo2015.UseVisualStyleBackColor = true;
      this.rdo2010.AutoSize = true;
      this.rdo2010.Checked = true;
      this.rdo2010.Location = new Point(27, 68);
      this.rdo2010.Name = "rdo2010";
      this.rdo2010.Size = new Size(102, 17);
      this.rdo2010.TabIndex = 4;
      this.rdo2010.TabStop = true;
      this.rdo2010.Text = "2010 Itemization";
      this.rdo2010.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(157, 109);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(246, 144);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.rdo2010);
      this.Controls.Add((Control) this.rdo2015);
      this.Controls.Add((Control) this.labelQuestion);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CreateNewTemplateDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Closing Cost";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
