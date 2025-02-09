// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FeeDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FeeDialog : Form, IHelp
  {
    private const string className = "FeeDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private Label label1;
    private Label label2;
    private ComboBox calcOnCombo;
    private Label label3;
    private Label label4;
    private Label label5;
    private GroupBox groupBox1;
    private Button cancelBtn;
    private Button addBtn;
    private System.ComponentModel.Container components;
    private TextBox feeNameTxt;
    private TextBox rateTxt;
    private TextBox additionalTxt;
    private bool newTable;
    private EMHelpLink emHelpLink1;
    private FeePanel.FeeType feeType;
    private Sessions.Session session;
    public string FeeName;
    public string CalcBasedOn;
    public string Rate;
    public string Additional;

    public FeeDialog(
      FeePanel.FeeType feeType,
      string feeName,
      string calcBasedOn,
      string rate,
      string additional,
      Sessions.Session session)
    {
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(session);
      this.session = session;
      this.feeType = feeType;
      switch (feeType)
      {
        case FeePanel.FeeType.City:
          this.Text = "City/County Tax/Stamps List";
          this.emHelpLink1.HelpTag = "Setup\\City Tax";
          break;
        case FeePanel.FeeType.State:
          this.Text = "State Tax/Stamps List";
          this.emHelpLink1.HelpTag = "Setup\\State Tax";
          break;
        case FeePanel.FeeType.UserDefined:
          this.Text = "User Defined List";
          this.emHelpLink1.HelpTag = "Setup\\User Defined Fee";
          break;
      }
      this.newTable = feeName == string.Empty;
      if (this.newTable)
        return;
      this.feeNameTxt.Text = feeName;
      this.feeNameTxt.ReadOnly = true;
      this.feeNameTxt.TabStop = false;
      for (int index = 0; index < this.calcOnCombo.Items.Count; ++index)
      {
        if (this.calcOnCombo.Items[index].ToString() == calcBasedOn)
        {
          this.calcOnCombo.SelectedIndex = index;
          break;
        }
      }
      this.rateTxt.Text = rate;
      this.additionalTxt.Text = additional;
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
      this.feeNameTxt = new TextBox();
      this.label2 = new Label();
      this.calcOnCombo = new ComboBox();
      this.label3 = new Label();
      this.rateTxt = new TextBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.additionalTxt = new TextBox();
      this.groupBox1 = new GroupBox();
      this.cancelBtn = new Button();
      this.addBtn = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 28);
      this.label1.Name = "label1";
      this.label1.Size = new Size(81, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Fee Description";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.feeNameTxt.Location = new Point(117, 24);
      this.feeNameTxt.MaxLength = 60;
      this.feeNameTxt.Name = "feeNameTxt";
      this.feeNameTxt.Size = new Size(220, 20);
      this.feeNameTxt.TabIndex = 1;
      this.feeNameTxt.Leave += new EventHandler(this.leave);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 56);
      this.label2.Name = "label2";
      this.label2.Size = new Size(98, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Calculate based on";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.calcOnCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.calcOnCombo.Items.AddRange(new object[2]
      {
        (object) "Loan Amount",
        (object) "Purchase Price"
      });
      this.calcOnCombo.Location = new Point(117, 52);
      this.calcOnCombo.Name = "calcOnCombo";
      this.calcOnCombo.Size = new Size(152, 21);
      this.calcOnCombo.TabIndex = 2;
      this.calcOnCombo.Leave += new EventHandler(this.leave);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 84);
      this.label3.Name = "label3";
      this.label3.Size = new Size(30, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Rate";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.rateTxt.Location = new Point(117, 80);
      this.rateTxt.MaxLength = 8;
      this.rateTxt.Name = "rateTxt";
      this.rateTxt.Size = new Size(80, 20);
      this.rateTxt.TabIndex = 3;
      this.rateTxt.Leave += new EventHandler(this.leave);
      this.rateTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.rateTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label4.Location = new Point(196, 84);
      this.label4.Name = "label4";
      this.label4.Size = new Size(20, 16);
      this.label4.TabIndex = 6;
      this.label4.Text = "%";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 112);
      this.label5.Name = "label5";
      this.label5.Size = new Size(71, 13);
      this.label5.TabIndex = 7;
      this.label5.Text = "+ Additional $";
      this.label5.TextAlign = ContentAlignment.MiddleLeft;
      this.additionalTxt.Location = new Point(117, 108);
      this.additionalTxt.MaxLength = 10;
      this.additionalTxt.Name = "additionalTxt";
      this.additionalTxt.Size = new Size(80, 20);
      this.additionalTxt.TabIndex = 4;
      this.additionalTxt.Leave += new EventHandler(this.leave);
      this.additionalTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.additionalTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.groupBox1.Location = new Point(14, 134);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(324, 10);
      this.groupBox1.TabIndex = 9;
      this.groupBox1.TabStop = false;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.cancelBtn.Location = new Point(263, 152);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 6;
      this.cancelBtn.Text = "&Cancel";
      this.addBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.addBtn.Location = new Point(181, 152);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(75, 24);
      this.addBtn.TabIndex = 5;
      this.addBtn.Text = "&OK";
      this.addBtn.Click += new EventHandler(this.okBtn_Click);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "";
      this.emHelpLink1.Location = new Point(14, 154);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 10;
      this.AcceptButton = (IButtonControl) this.addBtn;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(353, 192);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.addBtn);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.additionalTxt);
      this.Controls.Add((Control) this.rateTxt);
      this.Controls.Add((Control) this.feeNameTxt);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.calcOnCombo);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FeeDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Fee Detail";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void leave(object sender, EventArgs e)
    {
      if (sender == null || !(sender is TextBox))
        return;
      TextBox textBox = (TextBox) sender;
      if (textBox.Name == "additionalTxt")
      {
        double num = Utils.ParseDouble((object) textBox.Text);
        textBox.Text = num.ToString("N2");
      }
      if (!(textBox.Name == "rateTxt"))
        return;
      double num1 = Utils.ParseDouble((object) textBox.Text);
      textBox.Text = num1.ToString("N5");
    }

    private void keypress(object sender, KeyPressEventArgs e)
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

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.FeeName = this.feeNameTxt.Text;
      if (this.newTable)
      {
        if (this.FeeName == string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a fee description.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        FeeListBase feeListBase = (FeeListBase) null;
        switch (this.feeType)
        {
          case FeePanel.FeeType.City:
            feeListBase = (FeeListBase) this.session.GetSystemSettings(typeof (FeeCityList));
            break;
          case FeePanel.FeeType.State:
            feeListBase = (FeeListBase) this.session.GetSystemSettings(typeof (FeeStateList));
            break;
          case FeePanel.FeeType.UserDefined:
            feeListBase = (FeeListBase) this.session.GetSystemSettings(typeof (FeeUserList));
            break;
        }
        if (feeListBase.TableNameExists(this.FeeName))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The fee description that you entered is already in use. Please try a different description.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.feeNameTxt.Focus();
          return;
        }
      }
      this.CalcBasedOn = this.calcOnCombo.Text;
      this.Rate = this.rateTxt.Text;
      this.Additional = this.additionalTxt.Text;
      this.DialogResult = DialogResult.OK;
    }

    private void keyup(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp() => JedHelp.ShowHelp(this.emHelpLink1.HelpTag);
  }
}
