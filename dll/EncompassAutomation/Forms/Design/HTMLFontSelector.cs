// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.HTMLFontSelector
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class HTMLFontSelector : System.Windows.Forms.Form
  {
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private ComboBox cboSize;
    private System.Windows.Forms.CheckBox chkBold;
    private System.Windows.Forms.CheckBox chkItalics;
    private System.Windows.Forms.CheckBox chkUnderline;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label _sample;
    private ComboBox cboFont;
    private System.ComponentModel.Container components;

    public HTMLFontSelector(HTMLFont font)
    {
      this.InitializeComponent();
      this.initComboFont();
      this.cboFont.Text = font.Family;
      this.cboSize.Text = font.Size.ToString();
      this.chkBold.Checked = font.Bold;
      this.chkItalics.Checked = font.Italics;
      this.chkUnderline.Checked = font.Underline;
      this.setSampleFont(font);
      this.cboFont.SelectedIndexChanged += new EventHandler(this.currentFontChanged);
      this.cboSize.TextChanged += new EventHandler(this.currentFontChanged);
      this.chkBold.CheckStateChanged += new EventHandler(this.currentFontChanged);
      this.chkUnderline.CheckStateChanged += new EventHandler(this.currentFontChanged);
      this.chkItalics.CheckStateChanged += new EventHandler(this.currentFontChanged);
    }

    private void initComboFont()
    {
      FontFamily[] families = FontFamily.Families;
      this.cboFont.Items.Clear();
      for (int index = 0; index < families.Length; ++index)
      {
        if (families[index].IsStyleAvailable(FontStyle.Regular))
          this.cboFont.Items.Add((object) families[index].Name);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.cboSize = new ComboBox();
      this.chkBold = new System.Windows.Forms.CheckBox();
      this.chkItalics = new System.Windows.Forms.CheckBox();
      this.chkUnderline = new System.Windows.Forms.CheckBox();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.label7 = new System.Windows.Forms.Label();
      this._sample = new System.Windows.Forms.Label();
      this.cboFont = new ComboBox();
      this.SuspendLayout();
      this.label1.Location = new Point(14, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(88, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Font:";
      this.label2.Location = new Point(248, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(82, 18);
      this.label2.TabIndex = 1;
      this.label2.Text = "Size:";
      this.label3.Location = new Point(16, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(88, 18);
      this.label3.TabIndex = 2;
      this.label3.Text = "Effects:";
      this.cboSize.Items.AddRange(new object[24]
      {
        (object) "5",
        (object) "6",
        (object) "7",
        (object) "8",
        (object) "9",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "25",
        (object) "30",
        (object) "35",
        (object) "40",
        (object) "45",
        (object) "50",
        (object) "55",
        (object) "60"
      });
      this.cboSize.Location = new Point(248, 24);
      this.cboSize.Name = "cboSize";
      this.cboSize.Size = new Size(112, 21);
      this.cboSize.TabIndex = 4;
      this.chkBold.Location = new Point(16, 78);
      this.chkBold.Name = "chkBold";
      this.chkBold.Size = new Size(112, 17);
      this.chkBold.TabIndex = 5;
      this.chkBold.Text = "Bold";
      this.chkItalics.Location = new Point(130, 78);
      this.chkItalics.Name = "chkItalics";
      this.chkItalics.Size = new Size(114, 17);
      this.chkItalics.TabIndex = 6;
      this.chkItalics.Text = "Italics";
      this.chkUnderline.Location = new Point(250, 78);
      this.chkUnderline.Name = "chkUnderline";
      this.chkUnderline.Size = new Size(108, 17);
      this.chkUnderline.TabIndex = 7;
      this.chkUnderline.Text = "Underline";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(286, 186);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "&Cancel";
      this.btnOK.Location = new Point(206, 186);
      this.btnOK.Name = "btnOK";
      this.btnOK.TabIndex = 9;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.label7.Location = new Point(12, 112);
      this.label7.Name = "label7";
      this.label7.Size = new Size(100, 16);
      this.label7.TabIndex = 15;
      this.label7.Text = "Sample :";
      this._sample.BackColor = Color.White;
      this._sample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._sample.Location = new Point(12, 128);
      this._sample.Name = "_sample";
      this._sample.Size = new Size(348, 40);
      this._sample.TabIndex = 14;
      this._sample.Text = "AaBbCcXxYyZz";
      this._sample.TextAlign = ContentAlignment.MiddleCenter;
      this.cboFont.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFont.Location = new Point(14, 24);
      this.cboFont.Name = "cboFont";
      this.cboFont.Size = new Size(228, 21);
      this.cboFont.TabIndex = 22;
      this.cboFont.KeyPress += new KeyPressEventHandler(this.cboFont_KeyPress);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(368, 217);
      this.Controls.Add((System.Windows.Forms.Control) this.cboFont);
      this.Controls.Add((System.Windows.Forms.Control) this.label7);
      this.Controls.Add((System.Windows.Forms.Control) this._sample);
      this.Controls.Add((System.Windows.Forms.Control) this.btnOK);
      this.Controls.Add((System.Windows.Forms.Control) this.btnCancel);
      this.Controls.Add((System.Windows.Forms.Control) this.chkUnderline);
      this.Controls.Add((System.Windows.Forms.Control) this.chkItalics);
      this.Controls.Add((System.Windows.Forms.Control) this.chkBold);
      this.Controls.Add((System.Windows.Forms.Control) this.cboSize);
      this.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (HTMLFontSelector);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Font Editor";
      this.ResumeLayout(false);
    }

    private void cboFont_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.cboSize.Text.Length == 0)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "The indicated font size is invalid.", "HTML Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.cboSize.Focus();
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    public HTMLFont SelectedFont
    {
      get
      {
        return new HTMLFont(this.cboFont.Text, int.Parse(this.cboSize.Text), this.chkBold.Checked, this.chkItalics.Checked, this.chkUnderline.Checked);
      }
    }

    private void setSampleFont(HTMLFont font)
    {
      int style = 0;
      if (font.Bold)
        ++style;
      if (font.Underline)
        style += 4;
      if (font.Italics)
        style += 2;
      this._sample.Font = new Font(font.Family, (float) font.Size, (FontStyle) style, font.Unit);
    }

    private void currentFontChanged(object sender, EventArgs e)
    {
      int style = 0;
      if (this.chkBold.Checked)
        ++style;
      if (this.chkUnderline.Checked)
        style += 4;
      if (this.chkItalics.Checked)
        style += 2;
      Font font;
      try
      {
        font = new Font(this.cboFont.Text, (float) Convert.ToInt32(this.cboSize.Text), (FontStyle) style);
      }
      catch
      {
        font = new Font(this.cboFont.Text, (float) Convert.ToInt32(this.cboSize.Text));
      }
      this._sample.Font = font;
    }
  }
}
