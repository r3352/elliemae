// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.CustomFormImportDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class CustomFormImportDialog : Form
  {
    private RichTextBox richTextBox1;
    private Button btnOK;
    private Label label1;
    private System.ComponentModel.Container components;

    public CustomFormImportDialog() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public void SetViewContent(string[] lines) => this.richTextBox1.Lines = lines;

    private void InitializeComponent()
    {
      this.richTextBox1 = new RichTextBox();
      this.btnOK = new Button();
      this.label1 = new Label();
      this.SuspendLayout();
      this.richTextBox1.Location = new Point(20, 32);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.ReadOnly = true;
      this.richTextBox1.Size = new Size(576, 224);
      this.richTextBox1.TabIndex = 0;
      this.richTextBox1.Text = "";
      this.richTextBox1.WordWrap = false;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(520, 264);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(76, 24);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.label1.Location = new Point(20, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(412, 20);
      this.label1.TabIndex = 2;
      this.label1.Text = "Logs of custom form import.";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(610, 299);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.richTextBox1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (CustomFormImportDialog);
      this.Text = "Custom Form Import Logs";
      this.ResumeLayout(false);
    }
  }
}
