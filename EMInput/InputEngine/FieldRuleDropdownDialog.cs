// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FieldRuleDropdownDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.Encompass.Forms;
using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FieldRuleDropdownDialog : System.Windows.Forms.Form
  {
    private ListBox listBoxValues;
    private System.Windows.Forms.Panel panelTop;
    private PictureBox pictureBox1;
    private System.Windows.Forms.Panel panelShadow;
    private System.Windows.Forms.Panel panelMain;
    private System.Windows.Forms.Label labelFieldID;
    private System.ComponentModel.Container components;
    private DropdownOption[] options;
    private DropdownOption selectedItem;

    public FieldRuleDropdownDialog(DropdownOption[] options, string title)
    {
      this.InitializeComponent();
      this.ShowInTaskbar = false;
      this.options = options;
      this.Size = new Size(this.listBoxValues.Width, this.listBoxValues.Height);
      this.listBoxValues.Items.Clear();
      for (int index = 0; index < options.Length; ++index)
      {
        if (options[index].Text == "")
          this.listBoxValues.Items.Add((object) "<Clear>");
        else
          this.listBoxValues.Items.Add((object) options[index].Text);
      }
      this.labelFieldID.Text = title;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public DropdownOption SelectedItem => this.selectedItem;

    private void InitializeComponent()
    {
      ResourceManager resourceManager = new ResourceManager(typeof (FieldRuleDropdownDialog));
      this.listBoxValues = new ListBox();
      this.panelTop = new System.Windows.Forms.Panel();
      this.pictureBox1 = new PictureBox();
      this.panelShadow = new System.Windows.Forms.Panel();
      this.panelMain = new System.Windows.Forms.Panel();
      this.labelFieldID = new System.Windows.Forms.Label();
      this.panelTop.SuspendLayout();
      this.panelMain.SuspendLayout();
      this.SuspendLayout();
      this.listBoxValues.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listBoxValues.BackColor = Color.White;
      this.listBoxValues.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.listBoxValues.HorizontalScrollbar = true;
      this.listBoxValues.Location = new Point(1, 22);
      this.listBoxValues.Name = "listBoxValues";
      this.listBoxValues.Size = new Size(154, 169);
      this.listBoxValues.TabIndex = 0;
      this.listBoxValues.KeyPress += new KeyPressEventHandler(this.listBoxValues_KeyPress);
      this.listBoxValues.MouseUp += new MouseEventHandler(this.listBoxValues_MouseUp);
      this.panelTop.BackgroundImage = (System.Drawing.Image) resourceManager.GetObject("panelTop.BackgroundImage");
      this.panelTop.Controls.Add((System.Windows.Forms.Control) this.labelFieldID);
      this.panelTop.Controls.Add((System.Windows.Forms.Control) this.pictureBox1);
      this.panelTop.Dock = DockStyle.Top;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(154, 20);
      this.panelTop.TabIndex = 1;
      this.pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pictureBox1.Image = (System.Drawing.Image) resourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(139, 3);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(13, 13);
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.Click += new EventHandler(this.pictureBox1_Click);
      this.panelShadow.BackColor = Color.DarkGray;
      this.panelShadow.Location = new Point(4, 3);
      this.panelShadow.Name = "panelShadow";
      this.panelShadow.Size = new Size(156, 195);
      this.panelShadow.TabIndex = 3;
      this.panelMain.BackColor = Color.White;
      this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelMain.Controls.Add((System.Windows.Forms.Control) this.panelTop);
      this.panelMain.Controls.Add((System.Windows.Forms.Control) this.listBoxValues);
      this.panelMain.Location = new Point(0, 0);
      this.panelMain.Name = "panelMain";
      this.panelMain.Size = new Size(156, 195);
      this.panelMain.TabIndex = 4;
      this.labelFieldID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.labelFieldID.BackColor = Color.Transparent;
      this.labelFieldID.Location = new Point(2, 2);
      this.labelFieldID.Name = "labelFieldID";
      this.labelFieldID.Size = new Size(134, 16);
      this.labelFieldID.TabIndex = 0;
      this.labelFieldID.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.Gainsboro;
      this.ClientSize = new Size(168, 216);
      this.Controls.Add((System.Windows.Forms.Control) this.panelMain);
      this.Controls.Add((System.Windows.Forms.Control) this.panelShadow);
      this.FormBorderStyle = FormBorderStyle.None;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FieldRuleDropdownDialog);
      this.StartPosition = FormStartPosition.Manual;
      this.Text = nameof (FieldRuleDropdownDialog);
      this.TransparencyKey = Color.Gainsboro;
      this.panelTop.ResumeLayout(false);
      this.panelMain.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void listBoxValues_MouseUp(object sender, MouseEventArgs e)
    {
      if (this.listBoxValues.SelectedItems.Count == 0)
      {
        this.selectedItem = (DropdownOption) null;
      }
      else
      {
        this.selectedItem = this.options[this.listBoxValues.SelectedIndex];
        this.DialogResult = DialogResult.OK;
      }
    }

    private void listBoxValues_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    public void SetListBoxWidth(int w)
    {
      if (w < 120)
        w = 120;
      if (w < this.labelFieldID.Left + this.labelFieldID.Width)
        w = this.labelFieldID.Left + this.labelFieldID.Width + 20;
      this.panelMain.Width = w;
      this.panelMain.Height = this.listBoxValues.Top + this.listBoxValues.Items.Count > 8 ? 8 * this.listBoxValues.ItemHeight + 30 : this.listBoxValues.Items.Count * this.listBoxValues.ItemHeight + 30;
      this.panelShadow.Width = w;
      this.panelShadow.Height = this.panelMain.Height;
      this.Size = new Size(this.panelShadow.Left + w, this.panelShadow.Height + this.panelShadow.Top);
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
      this.selectedItem = (DropdownOption) null;
      this.DialogResult = DialogResult.Cancel;
    }
  }
}
