// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CountyLimitEditDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CountyLimitEditDialog : Form
  {
    private IContainer components;
    private TextBox txtLimit4;
    private TextBox txtLimit3;
    private TextBox txtLimit2;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label lblLimit1;
    private TextBox txtLimit1;
    private Button btnSave;
    private Button btnCancel;

    public CountyLimitEditDialog(string limit1, string limit2, string limit3, string limit4)
    {
      this.InitializeComponent();
      this.txtLimit1.Text = limit1;
      this.txtLimit2.Text = limit2;
      this.txtLimit3.Text = limit3;
      this.txtLimit4.Text = limit4;
      this.validateInput();
    }

    private bool validateInput()
    {
      bool flag = true;
      if (!Utils.IsInt((object) this.txtLimit1.Text.Trim()))
      {
        this.txtLimit1.Focus();
        int num = (int) Utils.Dialog((IWin32Window) this, "Invald value type for Limits for 1 Living Unit", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      else if (!Utils.IsInt((object) this.txtLimit2.Text.Trim()))
      {
        this.txtLimit2.Focus();
        int num = (int) Utils.Dialog((IWin32Window) this, "Invald value type for Limits for 2 Living Units", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      else if (!Utils.IsInt((object) this.txtLimit3.Text.Trim()))
      {
        this.txtLimit3.Focus();
        int num = (int) Utils.Dialog((IWin32Window) this, "Invald value type for Limits for 3 Living Units", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      else if (!Utils.IsInt((object) this.txtLimit4.Text.Trim()))
      {
        this.txtLimit4.Focus();
        int num = (int) Utils.Dialog((IWin32Window) this, "Invald value type for Limits for 4 Living Units", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      return flag;
    }

    private void leave(object sender, EventArgs e)
    {
      if (sender == null || !(sender is TextBox))
        return;
      TextBox textBox = (TextBox) sender;
      if (!Utils.IsDouble((object) textBox.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, textBox.Text + " is not a valid county limit.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        textBox.Text = "0";
        textBox.Focus();
      }
      else
        textBox.Text = CountyLimitEditDialog.ToDoubleString(textBox.Text);
      this.validateInput();
    }

    private void enter(object sender, EventArgs e)
    {
      if (sender == null || !(sender is TextBox))
        return;
      TextBox textBox = (TextBox) sender;
      textBox.Text = textBox.Text.Replace(",", "");
    }

    internal static string ToDoubleString(string strValue)
    {
      if (strValue == string.Empty || strValue == null)
        return "0";
      string str = double.Parse(strValue.Replace(",", string.Empty)).ToString("N2");
      return str.Substring(0, str.IndexOf('.'));
    }

    public string Limit1Text => this.txtLimit1.Text;

    public string Limit2Text => this.txtLimit2.Text;

    public string Limit3Text => this.txtLimit3.Text;

    public string Limit4Text => this.txtLimit4.Text;

    public int Limit1 => Utils.ParseInt((object) this.txtLimit1.Text.Replace(",", string.Empty));

    public int Limit2 => Utils.ParseInt((object) this.txtLimit2.Text.Replace(",", string.Empty));

    public int Limit3 => Utils.ParseInt((object) this.txtLimit3.Text.Replace(",", string.Empty));

    public int Limit4 => Utils.ParseInt((object) this.txtLimit4.Text.Replace(",", string.Empty));

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtLimit4 = new TextBox();
      this.txtLimit3 = new TextBox();
      this.txtLimit2 = new TextBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.lblLimit1 = new Label();
      this.txtLimit1 = new TextBox();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.txtLimit4.Location = new Point(357, 42);
      this.txtLimit4.Name = "txtLimit4";
      this.txtLimit4.Size = new Size(178, 20);
      this.txtLimit4.TabIndex = 26;
      this.txtLimit4.Leave += new EventHandler(this.leave);
      this.txtLimit4.Enter += new EventHandler(this.enter);
      this.txtLimit3.Location = new Point(88, 42);
      this.txtLimit3.Name = "txtLimit3";
      this.txtLimit3.Size = new Size(180, 20);
      this.txtLimit3.TabIndex = 25;
      this.txtLimit3.Leave += new EventHandler(this.leave);
      this.txtLimit3.Enter += new EventHandler(this.enter);
      this.txtLimit2.Location = new Point(357, 11);
      this.txtLimit2.Name = "txtLimit2";
      this.txtLimit2.Size = new Size(178, 20);
      this.txtLimit2.TabIndex = 24;
      this.txtLimit2.Leave += new EventHandler(this.leave);
      this.txtLimit2.Enter += new EventHandler(this.enter);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(273, 45);
      this.label5.Name = "label5";
      this.label5.Size = new Size(79, 13);
      this.label5.TabIndex = 23;
      this.label5.Text = "Limit for 4 Units";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 45);
      this.label4.Name = "label4";
      this.label4.Size = new Size(79, 13);
      this.label4.TabIndex = 22;
      this.label4.Text = "Limit for 3 Units";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(273, 15);
      this.label3.Name = "label3";
      this.label3.Size = new Size(79, 13);
      this.label3.TabIndex = 21;
      this.label3.Text = "Limit for 2 Units";
      this.lblLimit1.AutoSize = true;
      this.lblLimit1.Location = new Point(8, 15);
      this.lblLimit1.Name = "lblLimit1";
      this.lblLimit1.Size = new Size(74, 13);
      this.lblLimit1.TabIndex = 20;
      this.lblLimit1.Text = "Limit for 1 Unit";
      this.txtLimit1.Location = new Point(88, 12);
      this.txtLimit1.Name = "txtLimit1";
      this.txtLimit1.Size = new Size(180, 20);
      this.txtLimit1.TabIndex = 19;
      this.txtLimit1.Leave += new EventHandler(this.leave);
      this.txtLimit1.Enter += new EventHandler(this.enter);
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(386, 78);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(71, 23);
      this.btnSave.TabIndex = 27;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(463, 78);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(71, 23);
      this.btnCancel.TabIndex = 28;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(546, 113);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.txtLimit4);
      this.Controls.Add((Control) this.txtLimit3);
      this.Controls.Add((Control) this.txtLimit2);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.lblLimit1);
      this.Controls.Add((Control) this.txtLimit1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CountyLimitEditDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "FHA County Limits";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
