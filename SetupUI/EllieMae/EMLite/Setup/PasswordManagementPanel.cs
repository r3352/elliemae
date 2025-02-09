// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PasswordManagementPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PasswordManagementPanel : UserControl
  {
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private TextBox txtMinLength;
    private TextBox txtMinUpper;
    private Label label5;
    private TextBox txtMinLower;
    private Label label6;
    private TextBox txtMinDigits;
    private Label label7;
    private TextBox txtMinSpecial;
    private Label label8;
    private Label label9;
    private TextBox txtDaysToExpire;
    private CheckBox chkDaysToExpire;
    private CheckBox chkNumPrevious;
    private TextBox txtNumPrevious;
    private CheckBox chkDaysToReuse;
    private TextBox txtDaysToReuse;
    private CheckBox chkMaxAttempts;
    private TextBox txtMaxAttempts;
    private Button btnSave;
    private Button btnReset;
    private System.ComponentModel.Container components;
    private SetUpContainer setupContainer;

    public PasswordManagementPanel(SetUpContainer container)
    {
      this.InitializeComponent();
      this.setupContainer = container;
      this.resetForm();
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
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.txtMinLength = new TextBox();
      this.txtMinUpper = new TextBox();
      this.label5 = new Label();
      this.txtMinLower = new TextBox();
      this.label6 = new Label();
      this.txtMinDigits = new TextBox();
      this.label7 = new Label();
      this.txtMinSpecial = new TextBox();
      this.label8 = new Label();
      this.label9 = new Label();
      this.txtDaysToExpire = new TextBox();
      this.chkDaysToExpire = new CheckBox();
      this.chkNumPrevious = new CheckBox();
      this.txtNumPrevious = new TextBox();
      this.chkDaysToReuse = new CheckBox();
      this.txtDaysToReuse = new TextBox();
      this.chkMaxAttempts = new CheckBox();
      this.txtMaxAttempts = new TextBox();
      this.btnSave = new Button();
      this.btnReset = new Button();
      this.SuspendLayout();
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.Location = new Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(632, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Use this feature to specify rules for setting and maintaining user passwords.";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(0, 26);
      this.label2.Name = "label2";
      this.label2.Size = new Size(132, 16);
      this.label2.TabIndex = 1;
      this.label2.Text = "Password Requirements";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(0, 208);
      this.label3.Name = "label3";
      this.label3.Size = new Size(126, 16);
      this.label3.TabIndex = 2;
      this.label3.Text = "Password Maintenance";
      this.label4.Location = new Point(0, 54);
      this.label4.Name = "label4";
      this.label4.Size = new Size(246, 16);
      this.label4.TabIndex = 3;
      this.label4.Text = "Minimum length:";
      this.txtMinLength.Location = new Point(248, 50);
      this.txtMinLength.MaxLength = 4;
      this.txtMinLength.Name = "txtMinLength";
      this.txtMinLength.Size = new Size(52, 20);
      this.txtMinLength.TabIndex = 4;
      this.txtMinLength.Text = "";
      this.txtMinLength.TextAlign = HorizontalAlignment.Right;
      this.txtMinLength.KeyPress += new KeyPressEventHandler(this.onIntegerFieldKeyPress);
      this.txtMinLength.TextChanged += new EventHandler(this.onControlValueChanged);
      this.txtMinUpper.Location = new Point(248, 74);
      this.txtMinUpper.MaxLength = 4;
      this.txtMinUpper.Name = "txtMinUpper";
      this.txtMinUpper.Size = new Size(52, 20);
      this.txtMinUpper.TabIndex = 6;
      this.txtMinUpper.Text = "";
      this.txtMinUpper.TextAlign = HorizontalAlignment.Right;
      this.txtMinUpper.KeyPress += new KeyPressEventHandler(this.onIntegerFieldKeyPress);
      this.txtMinUpper.TextChanged += new EventHandler(this.onControlValueChanged);
      this.label5.Location = new Point(0, 78);
      this.label5.Name = "label5";
      this.label5.Size = new Size(246, 16);
      this.label5.TabIndex = 5;
      this.label5.Text = "Minimum number of upper case characters:";
      this.txtMinLower.Location = new Point(248, 98);
      this.txtMinLower.MaxLength = 4;
      this.txtMinLower.Name = "txtMinLower";
      this.txtMinLower.Size = new Size(52, 20);
      this.txtMinLower.TabIndex = 8;
      this.txtMinLower.Text = "";
      this.txtMinLower.TextAlign = HorizontalAlignment.Right;
      this.txtMinLower.KeyPress += new KeyPressEventHandler(this.onIntegerFieldKeyPress);
      this.txtMinLower.TextChanged += new EventHandler(this.onControlValueChanged);
      this.label6.Location = new Point(0, 102);
      this.label6.Name = "label6";
      this.label6.Size = new Size(246, 16);
      this.label6.TabIndex = 7;
      this.label6.Text = "Minimum number of lower case characters:";
      this.txtMinDigits.Location = new Point(248, 122);
      this.txtMinDigits.MaxLength = 4;
      this.txtMinDigits.Name = "txtMinDigits";
      this.txtMinDigits.Size = new Size(52, 20);
      this.txtMinDigits.TabIndex = 10;
      this.txtMinDigits.Text = "";
      this.txtMinDigits.TextAlign = HorizontalAlignment.Right;
      this.txtMinDigits.KeyPress += new KeyPressEventHandler(this.onIntegerFieldKeyPress);
      this.txtMinDigits.TextChanged += new EventHandler(this.onControlValueChanged);
      this.label7.Location = new Point(0, 126);
      this.label7.Name = "label7";
      this.label7.Size = new Size(246, 16);
      this.label7.TabIndex = 9;
      this.label7.Text = "Minimum number of numeric characters:";
      this.txtMinSpecial.Location = new Point(248, 146);
      this.txtMinSpecial.MaxLength = 4;
      this.txtMinSpecial.Name = "txtMinSpecial";
      this.txtMinSpecial.Size = new Size(52, 20);
      this.txtMinSpecial.TabIndex = 12;
      this.txtMinSpecial.Text = "";
      this.txtMinSpecial.TextAlign = HorizontalAlignment.Right;
      this.txtMinSpecial.KeyPress += new KeyPressEventHandler(this.onIntegerFieldKeyPress);
      this.txtMinSpecial.TextChanged += new EventHandler(this.onControlValueChanged);
      this.label8.Location = new Point(0, 150);
      this.label8.Name = "label8";
      this.label8.Size = new Size(246, 16);
      this.label8.TabIndex = 11;
      this.label8.Text = "Minimum number of special characters:";
      this.label9.Location = new Point(0, 166);
      this.label9.Name = "label9";
      this.label9.Size = new Size(246, 16);
      this.label9.TabIndex = 13;
      this.label9.Text = "(punctuation, symbols, etc.)";
      this.txtDaysToExpire.Location = new Point(348, 230);
      this.txtDaysToExpire.MaxLength = 4;
      this.txtDaysToExpire.Name = "txtDaysToExpire";
      this.txtDaysToExpire.Size = new Size(52, 20);
      this.txtDaysToExpire.TabIndex = 15;
      this.txtDaysToExpire.Text = "";
      this.txtDaysToExpire.TextAlign = HorizontalAlignment.Right;
      this.txtDaysToExpire.KeyPress += new KeyPressEventHandler(this.onIntegerFieldKeyPress);
      this.txtDaysToExpire.TextChanged += new EventHandler(this.onControlValueChanged);
      this.chkDaysToExpire.Location = new Point(0, 234);
      this.chkDaysToExpire.Name = "chkDaysToExpire";
      this.chkDaysToExpire.Size = new Size(342, 16);
      this.chkDaysToExpire.TabIndex = 16;
      this.chkDaysToExpire.Text = "Number of days before a password must be changed:";
      this.chkDaysToExpire.Click += new EventHandler(this.chkDaysToExpire_Click);
      this.chkNumPrevious.Location = new Point(0, 260);
      this.chkNumPrevious.Name = "chkNumPrevious";
      this.chkNumPrevious.Size = new Size(342, 16);
      this.chkNumPrevious.TabIndex = 18;
      this.chkNumPrevious.Text = "Number of previous passwords that cannot be reused:";
      this.chkNumPrevious.Click += new EventHandler(this.chkNumPrevious_Click);
      this.txtNumPrevious.Location = new Point(348, 256);
      this.txtNumPrevious.MaxLength = 4;
      this.txtNumPrevious.Name = "txtNumPrevious";
      this.txtNumPrevious.Size = new Size(52, 20);
      this.txtNumPrevious.TabIndex = 17;
      this.txtNumPrevious.Text = "";
      this.txtNumPrevious.TextAlign = HorizontalAlignment.Right;
      this.txtNumPrevious.KeyPress += new KeyPressEventHandler(this.onIntegerFieldKeyPress);
      this.txtNumPrevious.TextChanged += new EventHandler(this.onControlValueChanged);
      this.chkDaysToReuse.Location = new Point(0, 284);
      this.chkDaysToReuse.Name = "chkDaysToReuse";
      this.chkDaysToReuse.Size = new Size(342, 16);
      this.chkDaysToReuse.TabIndex = 20;
      this.chkDaysToReuse.Text = "Number of days before a password can be reused:";
      this.chkDaysToReuse.Click += new EventHandler(this.chkDaysToReuse_Click);
      this.txtDaysToReuse.Location = new Point(348, 280);
      this.txtDaysToReuse.MaxLength = 4;
      this.txtDaysToReuse.Name = "txtDaysToReuse";
      this.txtDaysToReuse.Size = new Size(52, 20);
      this.txtDaysToReuse.TabIndex = 19;
      this.txtDaysToReuse.Text = "";
      this.txtDaysToReuse.TextAlign = HorizontalAlignment.Right;
      this.txtDaysToReuse.KeyPress += new KeyPressEventHandler(this.onIntegerFieldKeyPress);
      this.txtDaysToReuse.TextChanged += new EventHandler(this.onControlValueChanged);
      this.chkMaxAttempts.Location = new Point(0, 308);
      this.chkMaxAttempts.Name = "chkMaxAttempts";
      this.chkMaxAttempts.Size = new Size(350, 16);
      this.chkMaxAttempts.TabIndex = 22;
      this.chkMaxAttempts.Text = "Number of failed login attempts before a user's account is locked:";
      this.chkMaxAttempts.Click += new EventHandler(this.chkMaxAttempts_Click);
      this.txtMaxAttempts.Location = new Point(348, 304);
      this.txtMaxAttempts.MaxLength = 4;
      this.txtMaxAttempts.Name = "txtMaxAttempts";
      this.txtMaxAttempts.Size = new Size(52, 20);
      this.txtMaxAttempts.TabIndex = 21;
      this.txtMaxAttempts.Text = "";
      this.txtMaxAttempts.TextAlign = HorizontalAlignment.Right;
      this.txtMaxAttempts.KeyPress += new KeyPressEventHandler(this.onIntegerFieldKeyPress);
      this.txtMaxAttempts.TextChanged += new EventHandler(this.onControlValueChanged);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnSave.Location = new Point(2, 360);
      this.btnSave.Name = "btnSave";
      this.btnSave.TabIndex = 23;
      this.btnSave.Text = "&Save";
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnReset.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnReset.Location = new Point(84, 360);
      this.btnReset.Name = "btnReset";
      this.btnReset.TabIndex = 24;
      this.btnReset.Text = "&Reset";
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.Controls.Add((Control) this.btnReset);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.txtMaxAttempts);
      this.Controls.Add((Control) this.chkMaxAttempts);
      this.Controls.Add((Control) this.chkDaysToReuse);
      this.Controls.Add((Control) this.txtDaysToReuse);
      this.Controls.Add((Control) this.chkNumPrevious);
      this.Controls.Add((Control) this.txtNumPrevious);
      this.Controls.Add((Control) this.chkDaysToExpire);
      this.Controls.Add((Control) this.txtDaysToExpire);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.txtMinSpecial);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.txtMinDigits);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.txtMinLower);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.txtMinUpper);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.txtMinLength);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (PasswordManagementPanel);
      this.Size = new Size(658, 396);
      this.ResumeLayout(false);
    }

    private void resetForm()
    {
      IDictionary serverSettings1 = Session.ServerManager.GetServerSettings("Password");
      IDictionary serverSettings2 = Session.ServerManager.GetServerSettings("Login");
      this.txtMinLength.Text = string.Concat(serverSettings1[(object) "Password.MinLength"]);
      this.txtMinUpper.Text = string.Concat(serverSettings1[(object) "Password.NumUpperCase"]);
      this.txtMinLower.Text = string.Concat(serverSettings1[(object) "Password.NumLowerCase"]);
      this.txtMinDigits.Text = string.Concat(serverSettings1[(object) "Password.NumDigits"]);
      this.txtMinSpecial.Text = string.Concat(serverSettings1[(object) "Password.NumSpecial"]);
      this.chkDaysToExpire.Checked = !serverSettings1[(object) "Password.Lifetime"].Equals((object) 0);
      this.txtDaysToExpire.Enabled = this.chkDaysToExpire.Checked;
      this.txtDaysToExpire.Text = this.chkDaysToExpire.Checked ? serverSettings1[(object) "Password.Lifetime"].ToString() : "";
      this.chkNumPrevious.Checked = !serverSettings1[(object) "Password.HistorySize"].Equals((object) 0);
      this.txtNumPrevious.Enabled = this.chkNumPrevious.Checked;
      this.txtNumPrevious.Text = this.chkNumPrevious.Checked ? serverSettings1[(object) "Password.HistorySize"].ToString() : "";
      this.chkDaysToReuse.Checked = !serverSettings1[(object) "Password.DaysToReuse"].Equals((object) 0);
      this.txtDaysToReuse.Enabled = this.chkDaysToReuse.Checked;
      this.txtDaysToReuse.Text = this.chkDaysToReuse.Checked ? serverSettings1[(object) "Password.DaysToReuse"].ToString() : "";
      this.chkMaxAttempts.Checked = !serverSettings2[(object) "Password.MaxLoginFailures"].Equals((object) 0);
      this.txtMaxAttempts.Enabled = this.chkMaxAttempts.Checked;
      this.txtMaxAttempts.Text = this.chkMaxAttempts.Checked ? serverSettings2[(object) "Password.MaxLoginFailures"].ToString() : "";
      this.btnSave.Enabled = false;
      this.btnReset.Enabled = false;
    }

    private void onIntegerFieldKeyPress(object sender, KeyPressEventArgs e)
    {
      e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
    }

    private void onControlValueChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
      this.btnReset.Enabled = true;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      Session.ServerManager.UpdateServerSettings((IDictionary) new Hashtable()
      {
        [(object) "Password.MinLength"] = (object) this.parseInt(this.txtMinLength.Text),
        [(object) "Password.NumUpperCase"] = (object) this.parseInt(this.txtMinUpper.Text),
        [(object) "Password.NumLowerCase"] = (object) this.parseInt(this.txtMinLower.Text),
        [(object) "Password.NumDigits"] = (object) this.parseInt(this.txtMinDigits.Text),
        [(object) "Password.NumSpecial"] = (object) this.parseInt(this.txtMinSpecial.Text),
        [(object) "Password.Lifetime"] = (object) this.parseInt(this.txtDaysToExpire.Text),
        [(object) "Password.HistorySize"] = (object) this.parseInt(this.txtNumPrevious.Text),
        [(object) "Password.DaysToReuse"] = (object) this.parseInt(this.txtDaysToReuse.Text),
        [(object) "Password.MaxLoginFailures"] = (object) this.parseInt(this.txtMaxAttempts.Text)
      });
    }

    private int parseInt(string text)
    {
      try
      {
        return int.Parse(text);
      }
      catch
      {
        return 0;
      }
    }

    private void chkDaysToExpire_Click(object sender, EventArgs e)
    {
      this.txtDaysToExpire.Enabled = this.chkDaysToExpire.Checked;
      this.txtDaysToExpire.Text = "";
      this.onControlValueChanged(sender, e);
    }

    private void chkNumPrevious_Click(object sender, EventArgs e)
    {
      this.txtNumPrevious.Enabled = this.chkNumPrevious.Checked;
      this.txtNumPrevious.Text = "";
      this.onControlValueChanged(sender, e);
    }

    private void chkDaysToReuse_Click(object sender, EventArgs e)
    {
      this.txtDaysToReuse.Enabled = this.chkDaysToReuse.Checked;
      this.txtDaysToReuse.Text = "";
      this.onControlValueChanged(sender, e);
    }

    private void chkMaxAttempts_Click(object sender, EventArgs e)
    {
      this.txtMaxAttempts.Enabled = this.chkMaxAttempts.Checked;
      this.txtMaxAttempts.Text = "";
      this.onControlValueChanged(sender, e);
    }

    private void btnReset_Click(object sender, EventArgs e) => this.resetForm();
  }
}
