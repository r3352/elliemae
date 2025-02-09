// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.IPAddressesForm
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class IPAddressesForm : Form
  {
    private IPRange ipRange;
    private const string everyUser = "<Everyone>";
    private Sessions.Session session;
    private IContainer components;
    private ComboBox comboBoxUserID;
    private Label label9;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private TextBox textBoxTo4;
    private TextBox textBoxTo3;
    private TextBox textBoxTo2;
    private TextBox textBoxTo1;
    private TextBox textBoxFrom4;
    private TextBox textBoxFrom3;
    private TextBox textBoxFrom2;
    private Label label2;
    private Label label1;
    private TextBox textBoxFrom1;
    private RadioButton rBtnRange;
    private RadioButton rBtnIP;
    private Label label10;
    private Label label11;
    private Label label12;
    private TextBox textBoxIP4;
    private TextBox textBoxIP3;
    private TextBox textBoxIP2;
    private TextBox textBoxIP1;
    private Label label14;
    private Button btnSave;
    private Button btnCancel;

    public IPAddressesForm(IPRange ipRange)
      : this(ipRange, Session.DefaultInstance)
    {
    }

    public IPAddressesForm(IPRange ipRange, Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.ipRange = ipRange;
      this.loadUsers();
      this.loadIPRange();
    }

    private void loadUsers()
    {
      string strB = "<Everyone>";
      if (this.ipRange != null && !this.ipRange.ForEveryone())
        strB = this.ipRange.Userid;
      this.comboBoxUserID.Items.Clear();
      this.comboBoxUserID.Items.Add((object) "<Everyone>");
      UserInfo[] allUsers = this.session.OrganizationManager.GetAllUsers();
      int num1 = 0;
      foreach (UserInfo userInfo in allUsers)
      {
        int num2 = this.comboBoxUserID.Items.Add((object) userInfo.Userid);
        if (string.Compare(userInfo.Userid, strB, StringComparison.OrdinalIgnoreCase) == 0)
          num1 = num2;
      }
      this.comboBoxUserID.SelectedIndex = num1;
    }

    private void loadIPRange()
    {
      if (this.ipRange == null)
        this.rBtnRange.Checked = true;
      else if (this.ipRange.StartIP == this.ipRange.EndIP)
      {
        this.rBtnIP.Checked = true;
        string[] strArray = this.ipRange.StartIP.Split('.');
        this.textBoxIP1.Text = strArray[0];
        this.textBoxIP2.Text = strArray[1];
        this.textBoxIP3.Text = strArray[2];
        this.textBoxIP4.Text = strArray[3];
      }
      else
      {
        this.rBtnRange.Checked = true;
        string[] strArray1 = this.ipRange.StartIP.Split('.');
        this.textBoxFrom1.Text = strArray1[0];
        this.textBoxFrom2.Text = strArray1[1];
        this.textBoxFrom3.Text = strArray1[2];
        this.textBoxFrom4.Text = strArray1[3];
        string[] strArray2 = this.ipRange.EndIP.Split('.');
        this.textBoxTo1.Text = strArray2[0];
        this.textBoxTo2.Text = strArray2[1];
        this.textBoxTo3.Text = strArray2[2];
        this.textBoxTo4.Text = strArray2[3];
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      string startIP;
      string endIP;
      if (this.rBtnRange.Checked)
      {
        startIP = this.textBoxFrom1.Text + "." + this.textBoxFrom2.Text + "." + this.textBoxFrom3.Text + "." + this.textBoxFrom4.Text;
        endIP = this.textBoxTo1.Text + "." + this.textBoxTo2.Text + "." + this.textBoxTo3.Text + "." + this.textBoxTo4.Text;
      }
      else
      {
        startIP = this.textBoxIP1.Text + "." + this.textBoxIP2.Text + "." + this.textBoxIP3.Text + "." + this.textBoxIP4.Text;
        endIP = startIP;
      }
      string userid = this.comboBoxUserID.SelectedItem.ToString();
      if (userid == "<Everyone>")
        userid = (string) null;
      IPRange ipRange;
      try
      {
        ipRange = new IPRange(this.ipRange == null ? 0 : this.ipRange.OID, userid, startIP, endIP);
        if (this.canBlockSelfIP(ipRange))
        {
          this.DialogResult = Utils.Dialog((IWin32Window) this, "Based on your current IP address, the new IP restriction you are setting up will now prevent you from being able to log into Encompass. Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
          if (this.DialogResult == DialogResult.No)
            return;
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        this.DialogResult = DialogResult.None;
        return;
      }
      if (ipRange.OID <= 0)
        this.session.ConfigurationManager.AddAllowedIPRange(ipRange);
      else
        this.session.ConfigurationManager.UpdateAllowedIPRange(ipRange);
    }

    private void textBoxIPComponent_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void textBoxIPComponent_TextChanged(object sender, EventArgs e)
    {
      TextBox textBox = sender as TextBox;
      if (string.IsNullOrEmpty(textBox.Text) || this.validateIPComponent(textBox.Text))
        return;
      textBox.Text = "";
    }

    private bool validateIPComponent(string ipString)
    {
      try
      {
        int int32 = Convert.ToInt32(ipString);
        if (int32 >= 0)
        {
          if (int32 <= (int) byte.MaxValue)
            goto label_4;
        }
        int num = (int) Utils.Dialog((IWin32Window) this, "Value must be between 0 and 255.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        return false;
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid IP address format.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        return false;
      }
label_4:
      return true;
    }

    private bool canBlockSelfIP(IPRange ips)
    {
      bool flag = true;
      try
      {
        string str = this.comboBoxUserID.SelectedItem.ToString();
        if (str == "<Everyone>" || str.ToLower() == this.session.UserID.ToLower())
        {
          IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
          IPAddress ipAddress = hostAddresses[hostAddresses.Length - 1];
          foreach (string ipString in new IPRangeFinder().GetIPRange(IPAddress.Parse(ips.StartIP), IPAddress.Parse(ips.EndIP)))
          {
            if (IPAddress.Parse(ipString).Equals((object) ipAddress))
            {
              flag = false;
              break;
            }
          }
        }
        else
          flag = false;
      }
      catch
      {
      }
      return flag;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IPAddressesForm));
      this.comboBoxUserID = new ComboBox();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.textBoxTo4 = new TextBox();
      this.textBoxTo3 = new TextBox();
      this.textBoxTo2 = new TextBox();
      this.textBoxTo1 = new TextBox();
      this.textBoxFrom4 = new TextBox();
      this.textBoxFrom3 = new TextBox();
      this.textBoxFrom2 = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.textBoxFrom1 = new TextBox();
      this.rBtnRange = new RadioButton();
      this.rBtnIP = new RadioButton();
      this.label10 = new Label();
      this.label11 = new Label();
      this.label12 = new Label();
      this.textBoxIP4 = new TextBox();
      this.textBoxIP3 = new TextBox();
      this.textBoxIP2 = new TextBox();
      this.textBoxIP1 = new TextBox();
      this.label14 = new Label();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.comboBoxUserID.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxUserID.FormattingEnabled = true;
      this.comboBoxUserID.Location = new Point(58, 14);
      this.comboBoxUserID.Name = "comboBoxUserID";
      this.comboBoxUserID.Size = new Size(213, 21);
      this.comboBoxUserID.TabIndex = 14;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(14, 17);
      this.label9.Name = "label9";
      this.label9.Size = new Size(29, 13);
      this.label9.TabIndex = 77;
      this.label9.Text = "User";
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(209, 102);
      this.label8.Name = "label8";
      this.label8.Size = new Size(13, 17);
      this.label8.TabIndex = 76;
      this.label8.Text = ".";
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(273, 102);
      this.label7.Name = "label7";
      this.label7.Size = new Size(13, 17);
      this.label7.TabIndex = 75;
      this.label7.Text = ".";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(146, 128);
      this.label6.Name = "label6";
      this.label6.Size = new Size(13, 17);
      this.label6.TabIndex = 74;
      this.label6.Text = ".";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(209, 128);
      this.label5.Name = "label5";
      this.label5.Size = new Size(13, 17);
      this.label5.TabIndex = 73;
      this.label5.Text = ".";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(273, 129);
      this.label4.Name = "label4";
      this.label4.Size = new Size(13, 17);
      this.label4.TabIndex = 72;
      this.label4.Text = ".";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(146, 102);
      this.label3.Name = "label3";
      this.label3.Size = new Size(13, 17);
      this.label3.TabIndex = 71;
      this.label3.Text = ".";
      this.textBoxTo4.Location = new Point(287, 128);
      this.textBoxTo4.Name = "textBoxTo4";
      this.textBoxTo4.Size = new Size(47, 20);
      this.textBoxTo4.TabIndex = 7;
      this.textBoxTo4.TextChanged += new EventHandler(this.textBoxIPComponent_TextChanged);
      this.textBoxTo4.KeyPress += new KeyPressEventHandler(this.textBoxIPComponent_KeyPress);
      this.textBoxTo3.Location = new Point(224, 128);
      this.textBoxTo3.Name = "textBoxTo3";
      this.textBoxTo3.Size = new Size(47, 20);
      this.textBoxTo3.TabIndex = 6;
      this.textBoxTo3.TextChanged += new EventHandler(this.textBoxIPComponent_TextChanged);
      this.textBoxTo3.KeyPress += new KeyPressEventHandler(this.textBoxIPComponent_KeyPress);
      this.textBoxTo2.Location = new Point(161, (int) sbyte.MaxValue);
      this.textBoxTo2.Name = "textBoxTo2";
      this.textBoxTo2.Size = new Size(47, 20);
      this.textBoxTo2.TabIndex = 5;
      this.textBoxTo2.TextChanged += new EventHandler(this.textBoxIPComponent_TextChanged);
      this.textBoxTo2.KeyPress += new KeyPressEventHandler(this.textBoxIPComponent_KeyPress);
      this.textBoxTo1.Location = new Point(98, (int) sbyte.MaxValue);
      this.textBoxTo1.Name = "textBoxTo1";
      this.textBoxTo1.Size = new Size(47, 20);
      this.textBoxTo1.TabIndex = 4;
      this.textBoxTo1.TextChanged += new EventHandler(this.textBoxIPComponent_TextChanged);
      this.textBoxTo1.KeyPress += new KeyPressEventHandler(this.textBoxIPComponent_KeyPress);
      this.textBoxFrom4.Location = new Point(287, 102);
      this.textBoxFrom4.Name = "textBoxFrom4";
      this.textBoxFrom4.Size = new Size(47, 20);
      this.textBoxFrom4.TabIndex = 3;
      this.textBoxFrom4.TextChanged += new EventHandler(this.textBoxIPComponent_TextChanged);
      this.textBoxFrom4.KeyPress += new KeyPressEventHandler(this.textBoxIPComponent_KeyPress);
      this.textBoxFrom3.Location = new Point(224, 102);
      this.textBoxFrom3.Name = "textBoxFrom3";
      this.textBoxFrom3.Size = new Size(47, 20);
      this.textBoxFrom3.TabIndex = 2;
      this.textBoxFrom3.TextChanged += new EventHandler(this.textBoxIPComponent_TextChanged);
      this.textBoxFrom3.KeyPress += new KeyPressEventHandler(this.textBoxIPComponent_KeyPress);
      this.textBoxFrom2.Location = new Point(161, 101);
      this.textBoxFrom2.Name = "textBoxFrom2";
      this.textBoxFrom2.Size = new Size(47, 20);
      this.textBoxFrom2.TabIndex = 1;
      this.textBoxFrom2.TextChanged += new EventHandler(this.textBoxIPComponent_TextChanged);
      this.textBoxFrom2.KeyPress += new KeyPressEventHandler(this.textBoxIPComponent_KeyPress);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(37, 131);
      this.label2.Name = "label2";
      this.label2.Size = new Size(20, 13);
      this.label2.TabIndex = 63;
      this.label2.Text = "To";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(37, 104);
      this.label1.Name = "label1";
      this.label1.Size = new Size(30, 13);
      this.label1.TabIndex = 62;
      this.label1.Text = "From";
      this.textBoxFrom1.Location = new Point(98, 101);
      this.textBoxFrom1.Name = "textBoxFrom1";
      this.textBoxFrom1.Size = new Size(47, 20);
      this.textBoxFrom1.TabIndex = 0;
      this.textBoxFrom1.TextChanged += new EventHandler(this.textBoxIPComponent_TextChanged);
      this.textBoxFrom1.KeyPress += new KeyPressEventHandler(this.textBoxIPComponent_KeyPress);
      this.rBtnRange.AutoSize = true;
      this.rBtnRange.Checked = true;
      this.rBtnRange.Location = new Point(12, 74);
      this.rBtnRange.Name = "rBtnRange";
      this.rBtnRange.Size = new Size(111, 17);
      this.rBtnRange.TabIndex = 15;
      this.rBtnRange.TabStop = true;
      this.rBtnRange.Text = "IP Address Range";
      this.rBtnRange.UseVisualStyleBackColor = true;
      this.rBtnIP.AutoSize = true;
      this.rBtnIP.Location = new Point(12, 159);
      this.rBtnIP.Name = "rBtnIP";
      this.rBtnIP.Size = new Size(76, 17);
      this.rBtnIP.TabIndex = 16;
      this.rBtnIP.Text = "IP Address";
      this.rBtnIP.UseVisualStyleBackColor = true;
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(146, 185);
      this.label10.Name = "label10";
      this.label10.Size = new Size(13, 17);
      this.label10.TabIndex = 88;
      this.label10.Text = ".";
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(209, 185);
      this.label11.Name = "label11";
      this.label11.Size = new Size(13, 17);
      this.label11.TabIndex = 87;
      this.label11.Text = ".";
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(273, 186);
      this.label12.Name = "label12";
      this.label12.Size = new Size(13, 17);
      this.label12.TabIndex = 86;
      this.label12.Text = ".";
      this.textBoxIP4.Location = new Point(287, 185);
      this.textBoxIP4.Name = "textBoxIP4";
      this.textBoxIP4.Size = new Size(47, 20);
      this.textBoxIP4.TabIndex = 11;
      this.textBoxIP4.TextChanged += new EventHandler(this.textBoxIPComponent_TextChanged);
      this.textBoxIP4.KeyPress += new KeyPressEventHandler(this.textBoxIPComponent_KeyPress);
      this.textBoxIP3.Location = new Point(224, 185);
      this.textBoxIP3.Name = "textBoxIP3";
      this.textBoxIP3.Size = new Size(47, 20);
      this.textBoxIP3.TabIndex = 10;
      this.textBoxIP3.TextChanged += new EventHandler(this.textBoxIPComponent_TextChanged);
      this.textBoxIP3.KeyPress += new KeyPressEventHandler(this.textBoxIPComponent_KeyPress);
      this.textBoxIP2.Location = new Point(161, 184);
      this.textBoxIP2.Name = "textBoxIP2";
      this.textBoxIP2.Size = new Size(47, 20);
      this.textBoxIP2.TabIndex = 9;
      this.textBoxIP2.TextChanged += new EventHandler(this.textBoxIPComponent_TextChanged);
      this.textBoxIP2.KeyPress += new KeyPressEventHandler(this.textBoxIPComponent_KeyPress);
      this.textBoxIP1.Location = new Point(98, 184);
      this.textBoxIP1.Name = "textBoxIP1";
      this.textBoxIP1.Size = new Size(47, 20);
      this.textBoxIP1.TabIndex = 8;
      this.textBoxIP1.TextChanged += new EventHandler(this.textBoxIPComponent_TextChanged);
      this.textBoxIP1.KeyPress += new KeyPressEventHandler(this.textBoxIPComponent_KeyPress);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(20, 45);
      this.label14.Name = "label14";
      this.label14.Size = new Size(318, 13);
      this.label14.TabIndex = 89;
      this.label14.Text = "is allowed to access Encompass from the following IP address(es):";
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(178, 224);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 12;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(259, 224);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 13;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(353, 262);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.label14);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.textBoxIP4);
      this.Controls.Add((Control) this.textBoxIP3);
      this.Controls.Add((Control) this.textBoxIP2);
      this.Controls.Add((Control) this.textBoxIP1);
      this.Controls.Add((Control) this.rBtnIP);
      this.Controls.Add((Control) this.rBtnRange);
      this.Controls.Add((Control) this.comboBoxUserID);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.textBoxTo4);
      this.Controls.Add((Control) this.textBoxTo3);
      this.Controls.Add((Control) this.textBoxTo2);
      this.Controls.Add((Control) this.textBoxTo1);
      this.Controls.Add((Control) this.textBoxFrom4);
      this.Controls.Add((Control) this.textBoxFrom3);
      this.Controls.Add((Control) this.textBoxFrom2);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxFrom1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (IPAddressesForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Allowed IP Addresses";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
