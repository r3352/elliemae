// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.IpAddressForm
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class IpAddressForm : Form
  {
    private Label label1;
    private Button btnOK;
    private Button btnCancel;
    private Label label12;
    private Label label11;
    private Label label8;
    private Label label9;
    private Label label10;
    private TextBox txtSMD;
    private TextBox txtSMC;
    private TextBox txtSMB;
    private TextBox txtSMA;
    private Button btnRemove;
    private Label label7;
    private Button btnAdd;
    private Label label6;
    private Label label5;
    private Label label4;
    private TextBox txtIPD;
    private TextBox txtIPC;
    private TextBox txtIPB;
    private Label label2;
    private TextBox txtIPA;
    private ListBox lstIPs;
    private System.ComponentModel.Container components;

    public IpAddressForm(IPAddressRange[] ips)
    {
      this.InitializeComponent();
      this.lstIPs.Items.Clear();
      for (int index = 0; index < ips.Length; ++index)
        this.lstIPs.Items.Add((object) ips[index]);
      if (this.lstIPs.Items.Count != 0)
        return;
      this.lstIPs.Items.Add((object) "(Allow all IP addresses)");
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
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.label12 = new Label();
      this.label11 = new Label();
      this.label8 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.txtSMD = new TextBox();
      this.txtSMC = new TextBox();
      this.txtSMB = new TextBox();
      this.txtSMA = new TextBox();
      this.btnRemove = new Button();
      this.label7 = new Label();
      this.btnAdd = new Button();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.txtIPD = new TextBox();
      this.txtIPC = new TextBox();
      this.txtIPB = new TextBox();
      this.label2 = new Label();
      this.txtIPA = new TextBox();
      this.lstIPs = new ListBox();
      this.SuspendLayout();
      this.label1.Location = new Point(14, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(302, 48);
      this.label1.TabIndex = 33;
      this.label1.Text = "You may select one or more IP addresses or subnets that you will permit to connect to your Encompass Server.";
      this.btnOK.Location = new Point(170, 250);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(72, 23);
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(246, 250);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(72, 23);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.label12.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(172, 118);
      this.label12.Name = "label12";
      this.label12.Size = new Size(142, 18);
      this.label12.TabIndex = 54;
      this.label12.Text = "(Subnet mask)";
      this.label12.TextAlign = ContentAlignment.TopCenter;
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(160, 100);
      this.label11.Name = "label11";
      this.label11.Size = new Size(11, 15);
      this.label11.TabIndex = 53;
      this.label11.Text = "/";
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(276, 100);
      this.label8.Name = "label8";
      this.label8.Size = new Size(11, 15);
      this.label8.TabIndex = 52;
      this.label8.Text = ".";
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(238, 100);
      this.label9.Name = "label9";
      this.label9.Size = new Size(11, 15);
      this.label9.TabIndex = 51;
      this.label9.Text = ".";
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(200, 100);
      this.label10.Name = "label10";
      this.label10.Size = new Size(11, 15);
      this.label10.TabIndex = 50;
      this.label10.Text = ".";
      this.txtSMD.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSMD.Location = new Point(284, 96);
      this.txtSMD.MaxLength = 3;
      this.txtSMD.Name = "txtSMD";
      this.txtSMD.Size = new Size(30, 20);
      this.txtSMD.TabIndex = 41;
      this.txtSMD.KeyPress += new KeyPressEventHandler(this.ipKeyPress);
      this.txtSMD.TextChanged += new EventHandler(this.ipTextChanged);
      this.txtSMC.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSMC.Location = new Point(246, 96);
      this.txtSMC.MaxLength = 3;
      this.txtSMC.Name = "txtSMC";
      this.txtSMC.Size = new Size(30, 20);
      this.txtSMC.TabIndex = 40;
      this.txtSMC.KeyPress += new KeyPressEventHandler(this.ipKeyPress);
      this.txtSMC.TextChanged += new EventHandler(this.ipTextChanged);
      this.txtSMB.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSMB.Location = new Point(208, 96);
      this.txtSMB.MaxLength = 3;
      this.txtSMB.Name = "txtSMB";
      this.txtSMB.Size = new Size(30, 20);
      this.txtSMB.TabIndex = 39;
      this.txtSMB.KeyPress += new KeyPressEventHandler(this.ipKeyPress);
      this.txtSMB.TextChanged += new EventHandler(this.ipTextChanged);
      this.txtSMA.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSMA.Location = new Point(170, 96);
      this.txtSMA.MaxLength = 3;
      this.txtSMA.Name = "txtSMA";
      this.txtSMA.Size = new Size(30, 20);
      this.txtSMA.TabIndex = 38;
      this.txtSMA.KeyPress += new KeyPressEventHandler(this.ipKeyPress);
      this.txtSMA.TextChanged += new EventHandler(this.ipTextChanged);
      this.btnRemove.Location = new Point(250, 166);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(64, 23);
      this.btnRemove.TabIndex = 43;
      this.btnRemove.Text = "Remove";
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(16, 116);
      this.label7.Name = "label7";
      this.label7.Size = new Size(142, 18);
      this.label7.TabIndex = 49;
      this.label7.Text = "(Address range)";
      this.label7.TextAlign = ContentAlignment.TopCenter;
      this.btnAdd.Location = new Point(250, 138);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(64, 23);
      this.btnAdd.TabIndex = 42;
      this.btnAdd.Text = "Add";
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(120, 100);
      this.label6.Name = "label6";
      this.label6.Size = new Size(11, 15);
      this.label6.TabIndex = 48;
      this.label6.Text = ".";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(82, 100);
      this.label5.Name = "label5";
      this.label5.Size = new Size(11, 15);
      this.label5.TabIndex = 47;
      this.label5.Text = ".";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(44, 100);
      this.label4.Name = "label4";
      this.label4.Size = new Size(11, 15);
      this.label4.TabIndex = 46;
      this.label4.Text = ".";
      this.txtIPD.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtIPD.Location = new Point(128, 96);
      this.txtIPD.MaxLength = 3;
      this.txtIPD.Name = "txtIPD";
      this.txtIPD.Size = new Size(30, 20);
      this.txtIPD.TabIndex = 37;
      this.txtIPD.KeyPress += new KeyPressEventHandler(this.ipKeyPress);
      this.txtIPD.TextChanged += new EventHandler(this.ipTextChanged);
      this.txtIPC.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtIPC.Location = new Point(90, 96);
      this.txtIPC.MaxLength = 3;
      this.txtIPC.Name = "txtIPC";
      this.txtIPC.Size = new Size(30, 20);
      this.txtIPC.TabIndex = 36;
      this.txtIPC.KeyPress += new KeyPressEventHandler(this.ipKeyPress);
      this.txtIPC.TextChanged += new EventHandler(this.ipTextChanged);
      this.txtIPB.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtIPB.Location = new Point(52, 96);
      this.txtIPB.MaxLength = 3;
      this.txtIPB.Name = "txtIPB";
      this.txtIPB.Size = new Size(30, 20);
      this.txtIPB.TabIndex = 35;
      this.txtIPB.KeyPress += new KeyPressEventHandler(this.ipKeyPress);
      this.txtIPB.TextChanged += new EventHandler(this.ipTextChanged);
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(14, 74);
      this.label2.Name = "label2";
      this.label2.Size = new Size(224, 20);
      this.label2.TabIndex = 45;
      this.label2.Text = "Only allow connections from:";
      this.label2.TextAlign = ContentAlignment.BottomLeft;
      this.txtIPA.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtIPA.Location = new Point(14, 96);
      this.txtIPA.MaxLength = 3;
      this.txtIPA.Name = "txtIPA";
      this.txtIPA.Size = new Size(30, 20);
      this.txtIPA.TabIndex = 34;
      this.txtIPA.KeyPress += new KeyPressEventHandler(this.ipKeyPress);
      this.txtIPA.TextChanged += new EventHandler(this.ipTextChanged);
      this.lstIPs.ItemHeight = 14;
      this.lstIPs.Location = new Point(14, 138);
      this.lstIPs.Name = "lstIPs";
      this.lstIPs.SelectionMode = SelectionMode.MultiExtended;
      this.lstIPs.Size = new Size(232, 88);
      this.lstIPs.TabIndex = 44;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(330, 282);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.txtSMD);
      this.Controls.Add((Control) this.txtSMC);
      this.Controls.Add((Control) this.txtSMB);
      this.Controls.Add((Control) this.txtSMA);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.txtIPD);
      this.Controls.Add((Control) this.txtIPC);
      this.Controls.Add((Control) this.txtIPB);
      this.Controls.Add((Control) this.txtIPA);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.lstIPs);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (IpAddressForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "IP Address Restrictions";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void ipTextChanged(object sender, EventArgs e)
    {
      TextBox ctl = (TextBox) sender;
      if (ctl.Text.Length != 3)
        return;
      this.SelectNextControl((Control) ctl, true, true, false, true);
    }

    private void ipKeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '.')
      {
        if (((Control) sender).Text != "")
          this.SelectNextControl((Control) sender, true, true, false, true);
        e.Handled = true;
      }
      else
      {
        if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
          return;
        e.Handled = true;
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      TextBox[] ipBoxes1 = new TextBox[4]
      {
        this.txtIPA,
        this.txtIPB,
        this.txtIPC,
        this.txtIPD
      };
      TextBox[] ipBoxes2 = new TextBox[4]
      {
        this.txtSMA,
        this.txtSMB,
        this.txtSMC,
        this.txtSMD
      };
      string addr = this.validateIP(ipBoxes1);
      if (addr == null)
        return;
      string subnet = this.validateIP(ipBoxes2);
      if (subnet == null)
        return;
      if (this.lstIPs.Items.Contains((object) addr))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified IP address is already listed.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (this.lstIPs.Items[0] is string)
          this.lstIPs.Items.Clear();
        this.lstIPs.Items.Add((object) new IPAddressRange(addr, subnet));
        this.clearIpAddresses();
      }
    }

    private void clearIpAddresses()
    {
      this.txtIPA.Text = this.txtIPB.Text = this.txtIPC.Text = this.txtIPD.Text = "";
      this.txtSMA.Text = this.txtSMB.Text = this.txtSMC.Text = this.txtSMD.Text = "";
    }

    private string validateIP(TextBox[] ipBoxes)
    {
      string str = "";
      for (int index = 0; index < ipBoxes.Length; ++index)
      {
        if (!this.isValidIPOctet(ipBoxes[index].Text))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "All four of the IP/Subnet address octets must lie in the range from 0 - 255, inclusive.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return (string) null;
        }
        str = str + (index != 0 ? "." : "") + ipBoxes[index].Text;
      }
      return str;
    }

    private bool isValidIPOctet(string value)
    {
      try
      {
        int num = int.Parse(value);
        return num >= 0 && num <= (int) byte.MaxValue;
      }
      catch
      {
        return false;
      }
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.lstIPs.Items[0] is string)
        return;
      for (int index = this.lstIPs.SelectedIndices.Count - 1; index >= 0; --index)
        this.lstIPs.Items.Remove(this.lstIPs.SelectedItems[index]);
      if (this.lstIPs.Items.Count != 0)
        return;
      this.lstIPs.Items.Add((object) "(Allow all IP addresses)");
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    public IPAddressRange[] GetIpRestrictions()
    {
      if (this.lstIPs.Items[0] is string)
        return new IPAddressRange[0];
      IPAddressRange[] destination = new IPAddressRange[this.lstIPs.Items.Count];
      this.lstIPs.Items.CopyTo((object[]) destination, 0);
      return destination;
    }
  }
}
