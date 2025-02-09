// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CheckDetailForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CheckDetailForm : UserControl, IPaymentMethod
  {
    private Hashtable fieldTable;
    private IContainer components;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    public TextBox textBox5;
    public TextBox textBoxAmount;
    public TextBox textBox3;
    public TextBox textBox2;
    public TextBox textBox4;
    private Label label7;
    public DatePicker textBox6;
    public TextBox textBox1;
    private Label label8;
    private Label label9;

    public CheckDetailForm() => this.InitializeComponent();

    private void decimal_KeyUp(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_2;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void decimal_FieldLeave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      double num = Utils.ParseDouble((object) textBox.Text);
      textBox.Text = num.ToString("N2");
    }

    public void UpdateAmount(double amount)
    {
      if (amount != 0.0)
        this.textBoxAmount.Text = amount.ToString("N2");
      else
        this.textBoxAmount.Text = string.Empty;
    }

    public void GetFieldValues(Hashtable fieldTable)
    {
      this.fieldTable = fieldTable;
      this.getFormFields(this.Controls);
    }

    private void getFormFields(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
            TextBox textBox = (TextBox) c;
            if (textBox != null && textBox.Tag != null)
            {
              string key = textBox.Tag.ToString();
              if (!(key == string.Empty))
              {
                if (this.fieldTable.ContainsKey((object) key))
                {
                  this.fieldTable[(object) key] = (object) textBox.Text.Trim();
                  continue;
                }
                this.fieldTable.Add((object) key, (object) textBox.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          case ComboBox _:
            ComboBox comboBox = (ComboBox) c;
            if (comboBox != null && comboBox.Tag != null)
            {
              string key = comboBox.Tag.ToString();
              if (!(key == string.Empty))
              {
                if (this.fieldTable.ContainsKey((object) key))
                {
                  this.fieldTable[(object) key] = (object) comboBox.Text.Trim();
                  continue;
                }
                this.fieldTable.Add((object) key, (object) comboBox.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          case DatePicker _:
            DatePicker datePicker = (DatePicker) c;
            if (datePicker != null && datePicker.Tag != null)
            {
              string key = datePicker.Tag.ToString();
              if (!(key == string.Empty))
              {
                if (this.fieldTable.ContainsKey((object) key))
                {
                  this.fieldTable[(object) key] = (object) datePicker.Text.Trim();
                  continue;
                }
                this.fieldTable.Add((object) key, (object) datePicker.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          case Label _:
            continue;
          default:
            this.getFormFields(c.Controls);
            continue;
        }
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
      this.textBox5 = new TextBox();
      this.textBoxAmount = new TextBox();
      this.textBox3 = new TextBox();
      this.textBox2 = new TextBox();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.textBox4 = new TextBox();
      this.label7 = new Label();
      this.textBox6 = new DatePicker();
      this.textBox1 = new TextBox();
      this.label8 = new Label();
      this.label9 = new Label();
      this.SuspendLayout();
      this.textBox5.Location = new Point(156, 88);
      this.textBox5.Name = "textBox5";
      this.textBox5.Size = new Size(112, 20);
      this.textBox5.TabIndex = 4;
      this.textBox5.Tag = (object) "CheckNumber";
      this.textBoxAmount.Location = new Point(156, 66);
      this.textBoxAmount.Name = "textBoxAmount";
      this.textBoxAmount.Size = new Size(112, 20);
      this.textBoxAmount.TabIndex = 3;
      this.textBoxAmount.Tag = (object) "CheckAmount";
      this.textBoxAmount.TextAlign = HorizontalAlignment.Right;
      this.textBoxAmount.Leave += new EventHandler(this.decimal_FieldLeave);
      this.textBoxAmount.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBox3.Location = new Point(156, 22);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new Size(112, 20);
      this.textBox3.TabIndex = 1;
      this.textBox3.Tag = (object) "AccountNumber";
      this.textBox2.Location = new Point(156, 0);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(112, 20);
      this.textBox2.TabIndex = 0;
      this.textBox2.Tag = (object) "InstitutionRouting";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(-1, 115);
      this.label6.Name = "label6";
      this.label6.Size = new Size(64, 13);
      this.label6.TabIndex = 31;
      this.label6.Text = "Check Date";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(-1, 93);
      this.label5.Name = "label5";
      this.label5.Size = new Size(48, 13);
      this.label5.TabIndex = 30;
      this.label5.Text = "Check #";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(-1, 71);
      this.label4.Name = "label4";
      this.label4.Size = new Size(77, 13);
      this.label4.TabIndex = 29;
      this.label4.Text = "Check Amount";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(-1, 27);
      this.label3.Name = "label3";
      this.label3.Size = new Size(57, 13);
      this.label3.TabIndex = 28;
      this.label3.Text = "Account #";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(-1, 5);
      this.label2.Name = "label2";
      this.label2.Size = new Size(102, 13);
      this.label2.TabIndex = 27;
      this.label2.Text = "Institution Routing #";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(140, 71);
      this.label1.Name = "label1";
      this.label1.Size = new Size(13, 13);
      this.label1.TabIndex = 37;
      this.label1.Text = "$";
      this.textBox4.Location = new Point(156, 44);
      this.textBox4.Name = "textBox4";
      this.textBox4.Size = new Size(112, 20);
      this.textBox4.TabIndex = 2;
      this.textBox4.Tag = (object) "AccountHolder";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(-1, 49);
      this.label7.Name = "label7";
      this.label7.Size = new Size(81, 13);
      this.label7.TabIndex = 42;
      this.label7.Text = "Account Holder";
      this.textBox6.BackColor = SystemColors.Window;
      this.textBox6.Location = new Point(156, 110);
      this.textBox6.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.textBox6.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.textBox6.Name = "textBox6";
      this.textBox6.Size = new Size(112, 21);
      this.textBox6.TabIndex = 5;
      this.textBox6.Tag = (object) "CheckDate";
      this.textBox6.Value = new DateTime(0L);
      this.textBox1.Location = new Point(156, 133);
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new Size(112, 20);
      this.textBox1.TabIndex = 52;
      this.textBox1.Tag = (object) "BuydownSubsidyAmount";
      this.textBox1.TextAlign = HorizontalAlignment.Right;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(-1, 137);
      this.label8.Name = "label8";
      this.label8.Size = new Size(130, 13);
      this.label8.TabIndex = 53;
      this.label8.Text = "Buydown Subsidy Amount";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(140, 137);
      this.label9.Name = "label9";
      this.label9.Size = new Size(13, 13);
      this.label9.TabIndex = 62;
      this.label9.Text = "$";
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.textBox1);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.textBox6);
      this.Controls.Add((Control) this.textBox4);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBox5);
      this.Controls.Add((Control) this.textBoxAmount);
      this.Controls.Add((Control) this.textBox3);
      this.Controls.Add((Control) this.textBox2);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Name = nameof (CheckDetailForm);
      this.Size = new Size(268, 156);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
