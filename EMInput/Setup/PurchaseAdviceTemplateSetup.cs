// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PurchaseAdviceTemplateSetup
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PurchaseAdviceTemplateSetup : Form
  {
    private string[] payoutOptions;
    private PurchaseAdviceTemplate purchaseTemplate;
    private Sessions.Session session;
    private IContainer components;
    private ComboBox comboBox12;
    private ComboBox comboBox11;
    private ComboBox comboBox10;
    private ComboBox comboBox9;
    private ComboBox comboBox8;
    private ComboBox comboBox7;
    private ComboBox comboBox6;
    private ComboBox comboBox5;
    private ComboBox comboBox4;
    private ComboBox comboBox3;
    private ComboBox comboBox2;
    private ComboBox comboBox1;
    private TextBox textBox10;
    private TextBox textBox11;
    private TextBox textBox17;
    private TextBox textBox18;
    private TextBox textBox19;
    private TextBox textBox20;
    private TextBox textBox21;
    private TextBox textBox22;
    private TextBox textBox23;
    private TextBox textBox24;
    private TextBox textBox25;
    private TextBox textBox26;
    private TextBox descTxt;
    private TextBox nameTxt;
    private Label label2;
    private Label label1;
    private Button buttonSave;
    private Button buttonCancel;

    public PurchaseAdviceTemplateSetup(PurchaseAdviceTemplate purchaseTemplate)
      : this(purchaseTemplate, Session.DefaultInstance)
    {
    }

    public PurchaseAdviceTemplateSetup(
      PurchaseAdviceTemplate purchaseTemplate,
      Sessions.Session session)
    {
      this.session = session;
      this.purchaseTemplate = purchaseTemplate;
      this.InitializeComponent();
      this.initForm();
      this.enforceSecurity();
      this.ShowInTaskbar = false;
    }

    private void enforceSecurity()
    {
      if (((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ToolsTab_PA_CreateEditTemplate))
        return;
      this.buttonSave.Enabled = false;
    }

    private void initForm()
    {
      ArrayList secondaryFields = this.session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.Payouts);
      this.payoutOptions = secondaryFields != null ? (string[]) secondaryFields.ToArray(typeof (string)) : new string[0];
      this.setFieldValues(this.Controls);
      if (this.purchaseTemplate == null)
        return;
      this.nameTxt.Text = this.purchaseTemplate.TemplateName;
      this.descTxt.Text = this.purchaseTemplate.Description;
    }

    private void setFieldValues(Control.ControlCollection cs)
    {
      this.nameTxt.Text = this.purchaseTemplate.TemplateName;
      this.descTxt.Text = this.purchaseTemplate.Description;
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
            TextBox textBox = (TextBox) c;
            textBox.Text = "";
            if (textBox != null && !(string.Concat(textBox.Tag) == ""))
            {
              string id = textBox.Tag.ToString();
              textBox.Text = this.purchaseTemplate.GetField(id);
              continue;
            }
            continue;
          case ComboBox _:
            ComboBox comboBox = (ComboBox) c;
            comboBox.Items.Clear();
            comboBox.Items.AddRange((object[]) this.payoutOptions);
            comboBox.Text = "";
            if (comboBox != null && comboBox.Tag != null)
            {
              string id = comboBox.Tag.ToString();
              if (!(id == string.Empty))
              {
                comboBox.Text = this.purchaseTemplate.GetField(id);
                continue;
              }
              continue;
            }
            continue;
          default:
            this.setFieldValues(c.Controls);
            continue;
        }
      }
    }

    private void getFieldValues(Control.ControlCollection cs)
    {
      this.purchaseTemplate.TemplateName = this.nameTxt.Text.Trim();
      this.purchaseTemplate.Description = this.descTxt.Text.Trim();
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
            TextBox textBox = (TextBox) c;
            if (textBox != null && textBox.Tag != null)
            {
              string id = textBox.Tag.ToString();
              if (!(id == string.Empty))
              {
                this.purchaseTemplate.SetCurrentField(id, textBox.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          case ComboBox _:
            ComboBox comboBox = (ComboBox) c;
            if (comboBox != null && comboBox.Tag != null)
            {
              string id = comboBox.Tag.ToString();
              if (!(id == string.Empty))
              {
                this.purchaseTemplate.SetCurrentField(id, comboBox.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          default:
            this.getFieldValues(c.Controls);
            continue;
        }
      }
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      string str = this.nameTxt.Text.Trim();
      if (str == string.Empty || str == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a Purchase Advice template name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.nameTxt.Focus();
      }
      else
      {
        if (this.purchaseTemplate == null)
          this.purchaseTemplate = new PurchaseAdviceTemplate();
        this.purchaseTemplate.MarkAsClean();
        this.getFieldValues(this.Controls);
        this.purchaseTemplate.TemplateName = this.nameTxt.Text.Trim();
        this.purchaseTemplate.Description = this.descTxt.Text.Trim();
        this.DialogResult = DialogResult.OK;
      }
    }

    private void decimalField_Keyup(object sender, KeyEventArgs e)
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

    private void decimalField_Keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsDigit(e.KeyChar))
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('.'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('-'))
          {
            e.Handled = true;
            return;
          }
        }
      }
      e.Handled = false;
    }

    private void decimalField_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      double num = Utils.ParseDouble((object) textBox.Text);
      if (num == 0.0)
        textBox.Text = "";
      else
        textBox.Text = num.ToString("N2");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.comboBox12 = new ComboBox();
      this.comboBox11 = new ComboBox();
      this.comboBox10 = new ComboBox();
      this.comboBox9 = new ComboBox();
      this.comboBox8 = new ComboBox();
      this.comboBox7 = new ComboBox();
      this.comboBox6 = new ComboBox();
      this.comboBox5 = new ComboBox();
      this.comboBox4 = new ComboBox();
      this.comboBox3 = new ComboBox();
      this.comboBox2 = new ComboBox();
      this.comboBox1 = new ComboBox();
      this.textBox10 = new TextBox();
      this.textBox11 = new TextBox();
      this.textBox17 = new TextBox();
      this.textBox18 = new TextBox();
      this.textBox19 = new TextBox();
      this.textBox20 = new TextBox();
      this.textBox21 = new TextBox();
      this.textBox22 = new TextBox();
      this.textBox23 = new TextBox();
      this.textBox24 = new TextBox();
      this.textBox25 = new TextBox();
      this.textBox26 = new TextBox();
      this.descTxt = new TextBox();
      this.nameTxt = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.buttonSave = new Button();
      this.buttonCancel = new Button();
      this.SuspendLayout();
      this.comboBox12.FormattingEnabled = true;
      this.comboBox12.Location = new Point(12, 388);
      this.comboBox12.Name = "comboBox12";
      this.comboBox12.Size = new Size(204, 21);
      this.comboBox12.TabIndex = 24;
      this.comboBox12.Tag = (object) "2394";
      this.comboBox11.FormattingEnabled = true;
      this.comboBox11.Location = new Point(12, 364);
      this.comboBox11.Name = "comboBox11";
      this.comboBox11.Size = new Size(204, 21);
      this.comboBox11.TabIndex = 22;
      this.comboBox11.Tag = (object) "2392";
      this.comboBox10.FormattingEnabled = true;
      this.comboBox10.Location = new Point(12, 340);
      this.comboBox10.Name = "comboBox10";
      this.comboBox10.Size = new Size(204, 21);
      this.comboBox10.TabIndex = 20;
      this.comboBox10.Tag = (object) "2390";
      this.comboBox9.FormattingEnabled = true;
      this.comboBox9.Location = new Point(12, 316);
      this.comboBox9.Name = "comboBox9";
      this.comboBox9.Size = new Size(204, 21);
      this.comboBox9.TabIndex = 18;
      this.comboBox9.Tag = (object) "2388";
      this.comboBox8.FormattingEnabled = true;
      this.comboBox8.Location = new Point(12, 292);
      this.comboBox8.Name = "comboBox8";
      this.comboBox8.Size = new Size(204, 21);
      this.comboBox8.TabIndex = 16;
      this.comboBox8.Tag = (object) "2386";
      this.comboBox7.FormattingEnabled = true;
      this.comboBox7.Location = new Point(12, 268);
      this.comboBox7.Name = "comboBox7";
      this.comboBox7.Size = new Size(204, 21);
      this.comboBox7.TabIndex = 14;
      this.comboBox7.Tag = (object) "2384";
      this.comboBox6.FormattingEnabled = true;
      this.comboBox6.Location = new Point(12, 244);
      this.comboBox6.Name = "comboBox6";
      this.comboBox6.Size = new Size(204, 21);
      this.comboBox6.TabIndex = 12;
      this.comboBox6.Tag = (object) "2382";
      this.comboBox5.FormattingEnabled = true;
      this.comboBox5.Location = new Point(12, 220);
      this.comboBox5.Name = "comboBox5";
      this.comboBox5.Size = new Size(204, 21);
      this.comboBox5.TabIndex = 10;
      this.comboBox5.Tag = (object) "2380";
      this.comboBox4.FormattingEnabled = true;
      this.comboBox4.Location = new Point(12, 196);
      this.comboBox4.Name = "comboBox4";
      this.comboBox4.Size = new Size(204, 21);
      this.comboBox4.TabIndex = 8;
      this.comboBox4.Tag = (object) "2378";
      this.comboBox3.FormattingEnabled = true;
      this.comboBox3.Location = new Point(12, 172);
      this.comboBox3.Name = "comboBox3";
      this.comboBox3.Size = new Size(204, 21);
      this.comboBox3.TabIndex = 6;
      this.comboBox3.Tag = (object) "2376";
      this.comboBox2.FormattingEnabled = true;
      this.comboBox2.Location = new Point(12, 148);
      this.comboBox2.Name = "comboBox2";
      this.comboBox2.Size = new Size(204, 21);
      this.comboBox2.TabIndex = 4;
      this.comboBox2.Tag = (object) "2374";
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new Point(12, 124);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new Size(204, 21);
      this.comboBox1.TabIndex = 2;
      this.comboBox1.Tag = (object) "2372";
      this.textBox10.Location = new Point(224, 388);
      this.textBox10.MaxLength = 10;
      this.textBox10.Name = "textBox10";
      this.textBox10.Size = new Size(132, 20);
      this.textBox10.TabIndex = 25;
      this.textBox10.Tag = (object) "2607";
      this.textBox10.TextAlign = HorizontalAlignment.Right;
      this.textBox10.Leave += new EventHandler(this.decimalField_Leave);
      this.textBox10.KeyUp += new KeyEventHandler(this.decimalField_Keyup);
      this.textBox10.KeyPress += new KeyPressEventHandler(this.decimalField_Keypress);
      this.textBox11.Location = new Point(224, 364);
      this.textBox11.MaxLength = 10;
      this.textBox11.Name = "textBox11";
      this.textBox11.Size = new Size(132, 20);
      this.textBox11.TabIndex = 23;
      this.textBox11.Tag = (object) "2606";
      this.textBox11.TextAlign = HorizontalAlignment.Right;
      this.textBox11.Leave += new EventHandler(this.decimalField_Leave);
      this.textBox11.KeyUp += new KeyEventHandler(this.decimalField_Keyup);
      this.textBox11.KeyPress += new KeyPressEventHandler(this.decimalField_Keypress);
      this.textBox17.Location = new Point(224, 340);
      this.textBox17.MaxLength = 10;
      this.textBox17.Name = "textBox17";
      this.textBox17.Size = new Size(132, 20);
      this.textBox17.TabIndex = 21;
      this.textBox17.Tag = (object) "2605";
      this.textBox17.TextAlign = HorizontalAlignment.Right;
      this.textBox17.Leave += new EventHandler(this.decimalField_Leave);
      this.textBox17.KeyUp += new KeyEventHandler(this.decimalField_Keyup);
      this.textBox17.KeyPress += new KeyPressEventHandler(this.decimalField_Keypress);
      this.textBox18.Location = new Point(224, 316);
      this.textBox18.MaxLength = 10;
      this.textBox18.Name = "textBox18";
      this.textBox18.Size = new Size(132, 20);
      this.textBox18.TabIndex = 19;
      this.textBox18.Tag = (object) "2604";
      this.textBox18.TextAlign = HorizontalAlignment.Right;
      this.textBox18.Leave += new EventHandler(this.decimalField_Leave);
      this.textBox18.KeyUp += new KeyEventHandler(this.decimalField_Keyup);
      this.textBox18.KeyPress += new KeyPressEventHandler(this.decimalField_Keypress);
      this.textBox19.Location = new Point(224, 292);
      this.textBox19.MaxLength = 10;
      this.textBox19.Name = "textBox19";
      this.textBox19.Size = new Size(132, 20);
      this.textBox19.TabIndex = 17;
      this.textBox19.Tag = (object) "2603";
      this.textBox19.TextAlign = HorizontalAlignment.Right;
      this.textBox19.Leave += new EventHandler(this.decimalField_Leave);
      this.textBox19.KeyUp += new KeyEventHandler(this.decimalField_Keyup);
      this.textBox19.KeyPress += new KeyPressEventHandler(this.decimalField_Keypress);
      this.textBox20.Location = new Point(224, 268);
      this.textBox20.MaxLength = 10;
      this.textBox20.Name = "textBox20";
      this.textBox20.Size = new Size(132, 20);
      this.textBox20.TabIndex = 15;
      this.textBox20.Tag = (object) "2602";
      this.textBox20.TextAlign = HorizontalAlignment.Right;
      this.textBox20.Leave += new EventHandler(this.decimalField_Leave);
      this.textBox20.KeyUp += new KeyEventHandler(this.decimalField_Keyup);
      this.textBox20.KeyPress += new KeyPressEventHandler(this.decimalField_Keypress);
      this.textBox21.Location = new Point(224, 244);
      this.textBox21.MaxLength = 10;
      this.textBox21.Name = "textBox21";
      this.textBox21.Size = new Size(132, 20);
      this.textBox21.TabIndex = 13;
      this.textBox21.Tag = (object) "2601";
      this.textBox21.TextAlign = HorizontalAlignment.Right;
      this.textBox21.Leave += new EventHandler(this.decimalField_Leave);
      this.textBox21.KeyUp += new KeyEventHandler(this.decimalField_Keyup);
      this.textBox21.KeyPress += new KeyPressEventHandler(this.decimalField_Keypress);
      this.textBox22.Location = new Point(224, 220);
      this.textBox22.MaxLength = 10;
      this.textBox22.Name = "textBox22";
      this.textBox22.Size = new Size(132, 20);
      this.textBox22.TabIndex = 11;
      this.textBox22.Tag = (object) "2600";
      this.textBox22.TextAlign = HorizontalAlignment.Right;
      this.textBox22.Leave += new EventHandler(this.decimalField_Leave);
      this.textBox22.KeyUp += new KeyEventHandler(this.decimalField_Keyup);
      this.textBox22.KeyPress += new KeyPressEventHandler(this.decimalField_Keypress);
      this.textBox23.Location = new Point(224, 196);
      this.textBox23.MaxLength = 10;
      this.textBox23.Name = "textBox23";
      this.textBox23.Size = new Size(132, 20);
      this.textBox23.TabIndex = 9;
      this.textBox23.Tag = (object) "2599";
      this.textBox23.TextAlign = HorizontalAlignment.Right;
      this.textBox23.Leave += new EventHandler(this.decimalField_Leave);
      this.textBox23.KeyUp += new KeyEventHandler(this.decimalField_Keyup);
      this.textBox23.KeyPress += new KeyPressEventHandler(this.decimalField_Keypress);
      this.textBox24.Location = new Point(224, 172);
      this.textBox24.MaxLength = 10;
      this.textBox24.Name = "textBox24";
      this.textBox24.Size = new Size(132, 20);
      this.textBox24.TabIndex = 7;
      this.textBox24.Tag = (object) "2598";
      this.textBox24.TextAlign = HorizontalAlignment.Right;
      this.textBox24.Leave += new EventHandler(this.decimalField_Leave);
      this.textBox24.KeyUp += new KeyEventHandler(this.decimalField_Keyup);
      this.textBox24.KeyPress += new KeyPressEventHandler(this.decimalField_Keypress);
      this.textBox25.Location = new Point(224, 148);
      this.textBox25.MaxLength = 10;
      this.textBox25.Name = "textBox25";
      this.textBox25.Size = new Size(132, 20);
      this.textBox25.TabIndex = 5;
      this.textBox25.Tag = (object) "2597";
      this.textBox25.TextAlign = HorizontalAlignment.Right;
      this.textBox25.Leave += new EventHandler(this.decimalField_Leave);
      this.textBox25.KeyUp += new KeyEventHandler(this.decimalField_Keyup);
      this.textBox25.KeyPress += new KeyPressEventHandler(this.decimalField_Keypress);
      this.textBox26.Location = new Point(224, 124);
      this.textBox26.MaxLength = 10;
      this.textBox26.Name = "textBox26";
      this.textBox26.Size = new Size(132, 20);
      this.textBox26.TabIndex = 3;
      this.textBox26.Tag = (object) "2596";
      this.textBox26.TextAlign = HorizontalAlignment.Right;
      this.textBox26.Leave += new EventHandler(this.decimalField_Leave);
      this.textBox26.KeyUp += new KeyEventHandler(this.decimalField_Keyup);
      this.textBox26.KeyPress += new KeyPressEventHandler(this.decimalField_Keypress);
      this.descTxt.Location = new Point(92, 34);
      this.descTxt.Multiline = true;
      this.descTxt.Name = "descTxt";
      this.descTxt.ScrollBars = ScrollBars.Both;
      this.descTxt.Size = new Size(264, 84);
      this.descTxt.TabIndex = 1;
      this.descTxt.Tag = (object) "";
      this.nameTxt.Location = new Point(92, 8);
      this.nameTxt.Name = "nameTxt";
      this.nameTxt.ReadOnly = true;
      this.nameTxt.Size = new Size(264, 20);
      this.nameTxt.TabIndex = 0;
      this.nameTxt.Tag = (object) "";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 34);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 62;
      this.label2.Text = "Description";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(82, 13);
      this.label1.TabIndex = 61;
      this.label1.Text = "Template Name";
      this.buttonSave.Location = new Point(196, 424);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new Size(75, 24);
      this.buttonSave.TabIndex = 26;
      this.buttonSave.Text = "Save";
      this.buttonSave.Click += new EventHandler(this.buttonSave_Click);
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(280, 424);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 24);
      this.buttonCancel.TabIndex = 27;
      this.buttonCancel.Text = "Cancel";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(368, 456);
      this.Controls.Add((Control) this.comboBox12);
      this.Controls.Add((Control) this.comboBox11);
      this.Controls.Add((Control) this.comboBox10);
      this.Controls.Add((Control) this.comboBox9);
      this.Controls.Add((Control) this.comboBox8);
      this.Controls.Add((Control) this.comboBox7);
      this.Controls.Add((Control) this.comboBox6);
      this.Controls.Add((Control) this.comboBox5);
      this.Controls.Add((Control) this.comboBox4);
      this.Controls.Add((Control) this.comboBox3);
      this.Controls.Add((Control) this.comboBox2);
      this.Controls.Add((Control) this.comboBox1);
      this.Controls.Add((Control) this.textBox10);
      this.Controls.Add((Control) this.textBox11);
      this.Controls.Add((Control) this.textBox17);
      this.Controls.Add((Control) this.textBox18);
      this.Controls.Add((Control) this.textBox19);
      this.Controls.Add((Control) this.textBox20);
      this.Controls.Add((Control) this.textBox21);
      this.Controls.Add((Control) this.textBox22);
      this.Controls.Add((Control) this.textBox23);
      this.Controls.Add((Control) this.textBox24);
      this.Controls.Add((Control) this.textBox25);
      this.Controls.Add((Control) this.textBox26);
      this.Controls.Add((Control) this.descTxt);
      this.Controls.Add((Control) this.nameTxt);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.buttonSave);
      this.Controls.Add((Control) this.buttonCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PurchaseAdviceTemplateSetup);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Create/Edit Template";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
