// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.CompileErrorDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Compiler;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms
{
  public class CompileErrorDialog : Form
  {
    private Label label1;
    private PictureBox pictureBox1;
    private Button btnClose;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private Button btnDetails;
    private RichTextBox rtfRegion;
    private TextBox txtMessage;
    private TextBox txtRegion;
    private TextBox txtLineNumber;
    private System.ComponentModel.Container components;

    public CompileErrorDialog(CompileException e)
    {
      this.InitializeComponent();
      CompilerError error = e.Errors[0];
      this.txtMessage.Text = error.Message;
      this.txtRegion.Text = error.Region == null ? "Unknown" : error.Region.Name;
      this.txtLineNumber.Text = error.Region == null ? error.Line.ToString() : error.LineIndexOfRegion.ToString();
      this.createRegionCodeDisplay(error);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CompileErrorDialog));
      this.label1 = new Label();
      this.pictureBox1 = new PictureBox();
      this.btnClose = new Button();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.btnDetails = new Button();
      this.rtfRegion = new RichTextBox();
      this.txtMessage = new TextBox();
      this.txtRegion = new TextBox();
      this.txtLineNumber = new TextBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.label1.Location = new Point(78, 39);
      this.label1.Name = "label1";
      this.label1.Size = new Size(440, 32);
      this.label1.TabIndex = 0;
      this.label1.Text = "An error has occurred attempting to compile the custom code on this input screen. This screen cannot be loaded due to this error.";
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(22, 15);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(444, 160);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 6;
      this.btnClose.Text = "&Close";
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(78, 79);
      this.label4.Name = "label4";
      this.label4.Size = new Size(72, 17);
      this.label4.TabIndex = 7;
      this.label4.Text = "Message:";
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(78, 101);
      this.label5.Name = "label5";
      this.label5.Size = new Size(72, 17);
      this.label5.TabIndex = 8;
      this.label5.Text = "Region:";
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(78, 124);
      this.label6.Name = "label6";
      this.label6.Size = new Size(72, 17);
      this.label6.TabIndex = 9;
      this.label6.Text = "Line #:";
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(78, 15);
      this.label7.Name = "label7";
      this.label7.Size = new Size(190, 18);
      this.label7.TabIndex = 10;
      this.label7.Text = "Form Compilation Error!";
      this.btnDetails.Location = new Point(78, 160);
      this.btnDetails.Name = "btnDetails";
      this.btnDetails.Size = new Size(100, 23);
      this.btnDetails.TabIndex = 14;
      this.btnDetails.Text = "Show &Details >>";
      this.btnDetails.Click += new EventHandler(this.btnDetails_Click);
      this.rtfRegion.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rtfRegion.Location = new Point(78, 194);
      this.rtfRegion.Name = "rtfRegion";
      this.rtfRegion.ReadOnly = true;
      this.rtfRegion.Size = new Size(440, 141);
      this.rtfRegion.TabIndex = 15;
      this.rtfRegion.Text = "";
      this.rtfRegion.WordWrap = false;
      this.txtMessage.Location = new Point(152, 77);
      this.txtMessage.Name = "txtMessage";
      this.txtMessage.ReadOnly = true;
      this.txtMessage.Size = new Size(366, 20);
      this.txtMessage.TabIndex = 16;
      this.txtRegion.Location = new Point(152, 99);
      this.txtRegion.Name = "txtRegion";
      this.txtRegion.ReadOnly = true;
      this.txtRegion.Size = new Size(366, 20);
      this.txtRegion.TabIndex = 17;
      this.txtLineNumber.Location = new Point(152, 121);
      this.txtLineNumber.Name = "txtLineNumber";
      this.txtLineNumber.ReadOnly = true;
      this.txtLineNumber.Size = new Size(366, 20);
      this.txtLineNumber.TabIndex = 18;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(538, 193);
      this.Controls.Add((Control) this.txtLineNumber);
      this.Controls.Add((Control) this.txtRegion);
      this.Controls.Add((Control) this.txtMessage);
      this.Controls.Add((Control) this.rtfRegion);
      this.Controls.Add((Control) this.btnDetails);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CompileErrorDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass Form Compilation Error";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void createRegionCodeDisplay(CompilerError error)
    {
      if (error.Region == null)
      {
        this.rtfRegion.AppendText("(Source code not available)");
      }
      else
      {
        string[] strArray = CompileErrorDialog.splitIntoLines(error.Region.SourceCode);
        for (int index = 0; index < strArray.Length; ++index)
        {
          this.rtfRegion.SelectionStart = this.rtfRegion.TextLength;
          if (index == error.LineIndexOfRegion - 1)
          {
            this.rtfRegion.SelectionFont = new Font(this.rtfRegion.Font.FontFamily, this.rtfRegion.Font.Size, FontStyle.Bold);
            this.rtfRegion.SelectionColor = Color.Red;
          }
          else
          {
            this.rtfRegion.SelectionFont = new Font(this.rtfRegion.Font.FontFamily, this.rtfRegion.Font.Size, FontStyle.Regular);
            this.rtfRegion.SelectionColor = Color.Black;
          }
          this.rtfRegion.AppendText(strArray[index] + Environment.NewLine);
        }
      }
    }

    private static string[] splitIntoLines(string text)
    {
      ArrayList arrayList = new ArrayList();
      StringReader stringReader = new StringReader(text);
      string str;
      while ((str = stringReader.ReadLine()) != null)
        arrayList.Add((object) str);
      return (string[]) arrayList.ToArray(typeof (string));
    }

    private void btnDetails_Click(object sender, EventArgs e)
    {
      if (this.btnDetails.Text.StartsWith("Show"))
      {
        this.Height += this.rtfRegion.Height + 10;
        this.btnDetails.Text = "Hide &Details <<";
      }
      else
      {
        this.Height -= this.rtfRegion.Height + 10;
        this.btnDetails.Text = "Show &Details >>";
      }
    }
  }
}
