// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.OTPRecipientControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class OTPRecipientControl : UserControl
  {
    private IContainer components;
    private Panel pnlRecipient;
    public TextBox txtName;
    public TextBox txtEmail;
    public CheckBox cbSelect;
    public ComboBox ddPhoneNumber;

    public OTPRecipientControl() => this.InitializeComponent();

    private void phoneField_KeyUp(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == '(' || e.KeyChar == ')' || e.KeyChar == '-')
        return;
      e.Handled = true;
    }

    private void formatFieldValue(object sender, FieldFormat format)
    {
      ComboBox comboBox = (ComboBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(comboBox.Text, format, ref needsUpdate);
      if (!needsUpdate)
        return;
      comboBox.Text = str;
      comboBox.SelectionStart = str.Length;
    }

    private void ddPhoneNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.ddPhoneNumber.Items.Count > 0 && this.ddPhoneNumber.SelectedValue.ToString() == "1000")
      {
        this.ddPhoneNumber.DropDownStyle = ComboBoxStyle.DropDown;
        this.ddPhoneNumber.Text = string.Empty;
        this.ddPhoneNumber.SelectedText = string.Empty;
      }
      else
        this.ddPhoneNumber.DropDownStyle = ComboBoxStyle.DropDownList;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlRecipient = new Panel();
      this.ddPhoneNumber = new ComboBox();
      this.txtName = new TextBox();
      this.txtEmail = new TextBox();
      this.cbSelect = new CheckBox();
      this.pnlRecipient.SuspendLayout();
      this.SuspendLayout();
      this.pnlRecipient.Controls.Add((Control) this.ddPhoneNumber);
      this.pnlRecipient.Controls.Add((Control) this.txtName);
      this.pnlRecipient.Controls.Add((Control) this.txtEmail);
      this.pnlRecipient.Controls.Add((Control) this.cbSelect);
      this.pnlRecipient.Dock = DockStyle.Fill;
      this.pnlRecipient.Location = new Point(0, 0);
      this.pnlRecipient.Name = "pnlRecipient";
      this.pnlRecipient.Size = new Size(600, 30);
      this.pnlRecipient.TabIndex = 1;
      this.ddPhoneNumber.Font = new Font("Arial", 7.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ddPhoneNumber.FormattingEnabled = true;
      this.ddPhoneNumber.Location = new Point(452, 5);
      this.ddPhoneNumber.Margin = new Padding(3, 1, 3, 1);
      this.ddPhoneNumber.MaxLength = 14;
      this.ddPhoneNumber.Name = "ddPhoneNumber";
      this.ddPhoneNumber.Size = new Size(135, 21);
      this.ddPhoneNumber.TabIndex = 5;
      this.ddPhoneNumber.SelectedIndexChanged += new EventHandler(this.ddPhoneNumber_SelectedIndexChanged);
      this.ddPhoneNumber.KeyPress += new KeyPressEventHandler(this.phoneField_KeyUp);
      this.txtName.Font = new Font("Arial", 8.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtName.Location = new Point(149, 5);
      this.txtName.Margin = new Padding(3, 2, 3, 2);
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(129, 21);
      this.txtName.TabIndex = 3;
      this.txtEmail.Font = new Font("Arial", 8.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtEmail.Location = new Point(284, 5);
      this.txtEmail.Margin = new Padding(3, 2, 3, 2);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.ReadOnly = true;
      this.txtEmail.Size = new Size(161, 21);
      this.txtEmail.TabIndex = 4;
      this.cbSelect.AutoSize = true;
      this.cbSelect.Checked = true;
      this.cbSelect.CheckState = CheckState.Checked;
      this.cbSelect.Location = new Point(3, 5);
      this.cbSelect.Margin = new Padding(3, 2, 3, 2);
      this.cbSelect.Name = "cbSelect";
      this.cbSelect.Size = new Size(93, 18);
      this.cbSelect.TabIndex = 2;
      this.cbSelect.Text = "RecipientType";
      this.cbSelect.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.pnlRecipient);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (OTPRecipientControl);
      this.Size = new Size(600, 30);
      this.pnlRecipient.ResumeLayout(false);
      this.pnlRecipient.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
