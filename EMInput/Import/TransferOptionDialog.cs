// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.TransferOptionDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class TransferOptionDialog : Form
  {
    private GroupBox grpOption;
    private const int radioX = 24;
    private const int radioHeight = 24;
    private const int opt2Y = 48;
    private System.ComponentModel.Container components;
    private Label lblMessage;
    private int selectedOption;
    private Button btnOK;
    private Button btnCancel;
    private Panel pnlButton;
    private CheckBox chkAll;
    private bool applyToAll;

    public int SelectedOption => this.selectedOption;

    public bool AppyToAll => this.applyToAll;

    public TransferOptionDialog(
      string caption,
      string text,
      string[] options,
      int defaultOption,
      bool showCheckBox)
      : this(caption, text, options, showCheckBox)
    {
    }

    public TransferOptionDialog(string caption, string text, string[] options, int defaultOption)
      : this(caption, text, options)
    {
      ((RadioButton) this.grpOption.Controls[defaultOption]).Checked = true;
    }

    public TransferOptionDialog(string caption, string text, string[] options)
      : this(caption, text, options, false)
    {
    }

    public TransferOptionDialog(string caption, string text, string[] options, bool showCheckBox)
    {
      this.InitializeComponent();
      this.Text = caption;
      this.lblMessage.Text = text;
      this.InitRadioButtons(options, showCheckBox);
    }

    private void InitRadioButtons(string[] options, bool showCheckBox)
    {
      for (int ordinal = 0; ordinal < options.Length; ++ordinal)
        this.grpOption.Controls.Add((Control) new TransferOptionDialog.userRadioButton(ordinal, options[ordinal]));
      this.chkAll.Visible = showCheckBox;
      if (options.Length <= 2)
        return;
      int addedHeight = TransferOptionDialog.userRadioButton.GetAddedHeight(options.Length - 2);
      this.pnlButton.Location = new Point(this.pnlButton.Location.X, this.pnlButton.Location.Y + addedHeight);
      GroupBox grpOption = this.grpOption;
      Size size1 = this.grpOption.Size;
      int width1 = size1.Width;
      size1 = this.grpOption.Size;
      int height1 = size1.Height + addedHeight;
      Size size2 = new Size(width1, height1);
      grpOption.Size = size2;
      size1 = this.Size;
      int width2 = size1.Width;
      size1 = this.Size;
      int height2 = size1.Height + addedHeight;
      this.Size = new Size(width2, height2);
    }

    private void chkAll_CheckedChanged(object sender, EventArgs e)
    {
      this.applyToAll = this.chkAll.Checked;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.grpOption = new GroupBox();
      this.lblMessage = new Label();
      this.pnlButton = new Panel();
      this.chkAll = new CheckBox();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.pnlButton.SuspendLayout();
      this.SuspendLayout();
      this.grpOption.Location = new Point(56, 88);
      this.grpOption.Name = "grpOption";
      this.grpOption.Size = new Size(232, 80);
      this.grpOption.TabIndex = 1;
      this.grpOption.TabStop = false;
      this.grpOption.Text = "Options";
      this.lblMessage.Location = new Point(48, 24);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(280, 56);
      this.lblMessage.TabIndex = 3;
      this.pnlButton.Controls.Add((Control) this.chkAll);
      this.pnlButton.Controls.Add((Control) this.btnCancel);
      this.pnlButton.Controls.Add((Control) this.btnOK);
      this.pnlButton.Location = new Point(56, 184);
      this.pnlButton.Name = "pnlButton";
      this.pnlButton.Size = new Size(232, 56);
      this.pnlButton.TabIndex = 7;
      this.chkAll.Location = new Point(64, 24);
      this.chkAll.Name = "chkAll";
      this.chkAll.Size = new Size(96, 24);
      this.chkAll.TabIndex = 9;
      this.chkAll.Text = "Apply to All";
      this.chkAll.Visible = false;
      this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(112, 0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(24, 0);
      this.btnOK.Name = "btnOK";
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(370, 248);
      this.Controls.Add((Control) this.pnlButton);
      this.Controls.Add((Control) this.lblMessage);
      this.Controls.Add((Control) this.grpOption);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TransferOptionDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Choice";
      this.pnlButton.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.grpOption.Controls.Count; ++index)
      {
        if (((RadioButton) this.grpOption.Controls[index]).Checked)
        {
          this.selectedOption = index;
          break;
        }
      }
    }

    private class userRadioButton : RadioButton
    {
      private const int posX = 24;
      private const int posY = 24;
      private const int sizeX = 180;
      private const int sizeY = 24;

      public userRadioButton(int ordinal, string text)
      {
        this.Location = new Point(24, 24 + TransferOptionDialog.userRadioButton.GetAddedHeight(ordinal));
        this.Size = new Size(180, 24);
        this.TabIndex = ordinal;
        this.Text = text;
        if (ordinal != 1)
          return;
        this.Checked = true;
      }

      public static int GetAddedHeight(int ordinal) => ordinal * 24;
    }
  }
}
