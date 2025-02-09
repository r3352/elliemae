// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditZipcodeForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditZipcodeForm : Form
  {
    private ZipcodeInfoUserDefined zipcodeInfo;
    private Sessions.Session session;
    private ZipcodeInfoUserDefined newZipcodeInfo;
    private IContainer components;
    private Label label1;
    private TextBox txtZip;
    private Label label2;
    private ComboBox cboState;
    private TextBox txtCity;
    private Label label3;
    private TextBox txtCounty;
    private Label label4;
    private Button btnCancel;
    private Button btnOK;

    public EditZipcodeForm(ZipcodeInfoUserDefined zipcodeInfo)
      : this(zipcodeInfo, Session.DefaultInstance)
    {
    }

    public EditZipcodeForm(ZipcodeInfoUserDefined zipcodeInfo, Sessions.Session session)
    {
      this.zipcodeInfo = zipcodeInfo;
      this.session = session;
      this.InitializeComponent();
      this.cboState.Items.AddRange((object[]) Utils.GetStates());
      if (this.zipcodeInfo != null)
      {
        this.txtZip.Text = this.zipcodeInfo.Zipcode + (this.zipcodeInfo.ZipcodeExtension != string.Empty ? "-" + this.zipcodeInfo.ZipcodeExtension : "");
        this.txtCity.Text = this.zipcodeInfo.ZipInfo.City;
        this.cboState.Text = this.zipcodeInfo.ZipInfo.State;
        this.txtCounty.Text = this.zipcodeInfo.ZipInfo.County;
      }
      this.enableOKButton();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      string str = this.txtZip.Text.Trim();
      string empty = string.Empty;
      if (str.IndexOf("-") > -1)
      {
        string[] strArray = str.Split('-');
        str = strArray[0];
        empty = strArray[1];
      }
      if ((this.zipcodeInfo == null || string.Compare(this.zipcodeInfo.Zipcode, str, true) != 0 || string.Compare(this.zipcodeInfo.ZipcodeExtension, empty, true) != 0 || string.Compare(this.zipcodeInfo.ZipInfo.State, this.cboState.Text.Trim(), true) != 0 || string.Compare(this.zipcodeInfo.ZipInfo.City, this.txtCity.Text.Trim(), true) != 0) && this.session.ConfigurationManager.IsZipcodeUserDefinedDuplicated(str, empty, this.cboState.Text, this.txtCity.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Zipcode database already contains this zipcode information.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.newZipcodeInfo = new ZipcodeInfoUserDefined(str, empty, this.txtCity.Text.Trim(), this.cboState.Text, this.txtCounty.Text.Trim());
        this.DialogResult = DialogResult.OK;
      }
    }

    private void textField_Changed(object sender, EventArgs e) => this.enableOKButton();

    private void cboState_SelectedIndexChanged(object sender, EventArgs e) => this.enableOKButton();

    private void enableOKButton()
    {
      this.btnOK.Enabled = (this.txtZip.Text.Trim().Length == 5 || this.txtZip.Text.Trim().Length == 10) && this.txtCity.Text.Trim().Length > 0 && this.cboState.Text.Trim().Length > 0;
    }

    public ZipcodeInfoUserDefined NewZipcodeInfo => this.newZipcodeInfo;

    private void txtZip_KeyUp(object sender, KeyEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, FieldFormat.ZIPCODE, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
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
      this.txtZip = new TextBox();
      this.label2 = new Label();
      this.cboState = new ComboBox();
      this.txtCity = new TextBox();
      this.label3 = new Label();
      this.txtCounty = new TextBox();
      this.label4 = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(24, 22);
      this.label1.Name = "label1";
      this.label1.Size = new Size(22, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Zip";
      this.txtZip.Location = new Point(94, 19);
      this.txtZip.MaxLength = 10;
      this.txtZip.Name = "txtZip";
      this.txtZip.Size = new Size(178, 20);
      this.txtZip.TabIndex = 0;
      this.txtZip.TextChanged += new EventHandler(this.textField_Changed);
      this.txtZip.KeyUp += new KeyEventHandler(this.txtZip_KeyUp);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(24, 74);
      this.label2.Name = "label2";
      this.label2.Size = new Size(32, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "State";
      this.cboState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboState.FormattingEnabled = true;
      this.cboState.Location = new Point(94, 71);
      this.cboState.Name = "cboState";
      this.cboState.Size = new Size(91, 21);
      this.cboState.TabIndex = 2;
      this.cboState.SelectedIndexChanged += new EventHandler(this.cboState_SelectedIndexChanged);
      this.txtCity.Location = new Point(94, 45);
      this.txtCity.MaxLength = 64;
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new Size(178, 20);
      this.txtCity.TabIndex = 1;
      this.txtCity.TextChanged += new EventHandler(this.textField_Changed);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(24, 48);
      this.label3.Name = "label3";
      this.label3.Size = new Size(24, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "City";
      this.txtCounty.Location = new Point(94, 98);
      this.txtCounty.MaxLength = 64;
      this.txtCounty.Name = "txtCounty";
      this.txtCounty.Size = new Size(178, 20);
      this.txtCounty.TabIndex = 3;
      this.txtCounty.TextChanged += new EventHandler(this.textField_Changed);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(24, 101);
      this.label4.Name = "label4";
      this.label4.Size = new Size(40, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "County";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(197, 138);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Location = new Point(116, 138);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(301, 173);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtCounty);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.txtCity);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.cboState);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtZip);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EditZipcodeForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Zipcode";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
