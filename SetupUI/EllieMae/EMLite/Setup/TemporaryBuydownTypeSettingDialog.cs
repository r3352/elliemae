// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TemporaryBuydownTypeSettingDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TemporaryBuydownTypeSettingDialog : Form
  {
    private TemporaryBuydown buydown;
    private Sessions.Session session;
    private bool isModified;
    private List<TemporaryBuydownTypeSettingDialog.RateTerm> list;
    private IContainer components;
    private Panel panel1;
    private Label label1;
    private Button okBtn;
    private Button cancelBtn;
    private Label label2;
    private TextBox buydownTypeDescription;
    private TextBox buydownTypeName;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private TextBox rate6;
    private TextBox rate5;
    private TextBox rate4;
    private TextBox rate3;
    private TextBox rate2;
    private TextBox rate1;
    private Label label9;
    private Label label14;
    private Label label13;
    private Label label12;
    private Label label11;
    private Label label10;
    private Label label15;
    private Label label20;
    private Label label19;
    private Label label18;
    private Label label17;
    private Label label16;
    private TextBox term6;
    private TextBox term5;
    private TextBox term4;
    private TextBox term3;
    private TextBox term2;
    private TextBox term1;
    private Label label21;
    private Label label23;
    private Label label22;
    private Label label26;
    private Label label25;
    private Label label24;

    public TemporaryBuydownTypeSettingDialog(Sessions.Session session, TemporaryBuydown buydown = null)
    {
      this.InitializeComponent();
      this.session = session;
      this.buydown = buydown;
      this.list = new List<TemporaryBuydownTypeSettingDialog.RateTerm>();
      if (buydown != null)
        this.loadTemporaryBuydown(buydown);
      this.buydownTypeName.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.buydownTypeDescription.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.rate1.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.term1.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.rate2.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.term2.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.rate3.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.term3.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.rate4.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.term4.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.rate5.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.term5.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.rate6.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.term6.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.rate1.KeyPress += new KeyPressEventHandler(this.rate_Valitation);
      this.rate1.LostFocus += new EventHandler(this.rate_LostFocus);
      this.rate1.GotFocus += new EventHandler(this.rateTerm_GotFocus);
      this.term1.KeyPress += new KeyPressEventHandler(this.term_Validate);
      this.term1.GotFocus += new EventHandler(this.rateTerm_GotFocus);
      this.rate2.KeyPress += new KeyPressEventHandler(this.rate_Valitation);
      this.rate2.LostFocus += new EventHandler(this.rate_LostFocus);
      this.rate2.GotFocus += new EventHandler(this.rateTerm_GotFocus);
      this.term2.KeyPress += new KeyPressEventHandler(this.term_Validate);
      this.term2.GotFocus += new EventHandler(this.rateTerm_GotFocus);
      this.rate3.KeyPress += new KeyPressEventHandler(this.rate_Valitation);
      this.rate3.LostFocus += new EventHandler(this.rate_LostFocus);
      this.rate3.GotFocus += new EventHandler(this.rateTerm_GotFocus);
      this.term3.KeyPress += new KeyPressEventHandler(this.term_Validate);
      this.term3.GotFocus += new EventHandler(this.rateTerm_GotFocus);
      this.rate4.KeyPress += new KeyPressEventHandler(this.rate_Valitation);
      this.rate4.LostFocus += new EventHandler(this.rate_LostFocus);
      this.rate4.GotFocus += new EventHandler(this.rateTerm_GotFocus);
      this.term4.KeyPress += new KeyPressEventHandler(this.term_Validate);
      this.term4.GotFocus += new EventHandler(this.rateTerm_GotFocus);
      this.rate5.KeyPress += new KeyPressEventHandler(this.rate_Valitation);
      this.rate5.LostFocus += new EventHandler(this.rate_LostFocus);
      this.rate5.GotFocus += new EventHandler(this.rateTerm_GotFocus);
      this.term5.KeyPress += new KeyPressEventHandler(this.term_Validate);
      this.term5.GotFocus += new EventHandler(this.rateTerm_GotFocus);
      this.rate6.KeyPress += new KeyPressEventHandler(this.rate_Valitation);
      this.rate6.LostFocus += new EventHandler(this.rate_LostFocus);
      this.rate6.GotFocus += new EventHandler(this.rateTerm_GotFocus);
      this.term6.KeyPress += new KeyPressEventHandler(this.term_Validate);
      this.term6.GotFocus += new EventHandler(this.rateTerm_GotFocus);
    }

    public TemporaryBuydown Buydown => this.buydown;

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (!this.isModified && this.buydownTypeName.Text != string.Empty)
      {
        this.Close();
      }
      else
      {
        if (this.buydown == null)
          this.buydown = new TemporaryBuydown();
        this.getRateAndTerm();
        this.buydown.Rate1 = "";
        this.buydown.Term1 = "";
        this.buydown.Rate2 = "";
        this.buydown.Term2 = "";
        this.buydown.Rate3 = "";
        this.buydown.Term3 = "";
        this.buydown.Rate4 = "";
        this.buydown.Term4 = "";
        this.buydown.Rate5 = "";
        this.buydown.Term5 = "";
        this.buydown.Rate6 = "";
        this.buydown.Term6 = "";
        if (this.buydownTypeName.Text == string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a Buydown Type.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.buydown.BuydownType = this.buydownTypeName.Text;
          this.buydown.Description = this.buydownTypeDescription.Text == string.Empty ? "" : this.buydownTypeDescription.Text;
          if (this.list.Count > 0)
          {
            TemporaryBuydownTypeSettingDialog.RateTerm rateTerm = this.list[0];
            this.buydown.Rate1 = rateTerm.Rate.Replace(",", "");
            this.buydown.Term1 = rateTerm.Term.Replace(",", "");
            this.list.RemoveAt(0);
          }
          if (this.list.Count > 0)
          {
            TemporaryBuydownTypeSettingDialog.RateTerm rateTerm = this.list[0];
            this.buydown.Rate2 = rateTerm.Rate.Replace(",", "");
            this.buydown.Term2 = rateTerm.Term.Replace(",", "");
            this.list.RemoveAt(0);
          }
          if (this.list.Count > 0)
          {
            TemporaryBuydownTypeSettingDialog.RateTerm rateTerm = this.list[0];
            this.buydown.Rate3 = rateTerm.Rate.Replace(",", "");
            this.buydown.Term3 = rateTerm.Term.Replace(",", "");
            this.list.RemoveAt(0);
          }
          if (this.list.Count > 0)
          {
            TemporaryBuydownTypeSettingDialog.RateTerm rateTerm = this.list[0];
            this.buydown.Rate4 = rateTerm.Rate.Replace(",", "");
            this.buydown.Term4 = rateTerm.Term.Replace(",", "");
            this.list.RemoveAt(0);
          }
          if (this.list.Count > 0)
          {
            TemporaryBuydownTypeSettingDialog.RateTerm rateTerm = this.list[0];
            this.buydown.Rate5 = rateTerm.Rate.Replace(",", "");
            this.buydown.Term5 = rateTerm.Term.Replace(",", "");
            this.list.RemoveAt(0);
          }
          if (this.list.Count > 0)
          {
            TemporaryBuydownTypeSettingDialog.RateTerm rateTerm = this.list[0];
            this.buydown.Rate6 = rateTerm.Rate.Replace(",", "");
            this.buydown.Term6 = rateTerm.Term.Replace(",", "");
            this.list.RemoveAt(0);
          }
          this.buydown.lastModifiedBy = this.session.UserID + " (" + this.session.UserInfo.FullName + ")";
          this.buydown.lastModifiedDateTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
          this.DialogResult = DialogResult.OK;
          this.Close();
        }
      }
    }

    private void textBox_TextChanged(object sender, EventArgs e) => this.isModified = true;

    private void getRateAndTerm()
    {
      this.list.Clear();
      if (this.rate1.Text != string.Empty || this.term1.Text != string.Empty)
        this.list.Add(new TemporaryBuydownTypeSettingDialog.RateTerm(this.rate1.Text, this.term1.Text));
      if (this.rate2.Text != string.Empty || this.term2.Text != string.Empty)
        this.list.Add(new TemporaryBuydownTypeSettingDialog.RateTerm(this.rate2.Text, this.term2.Text));
      if (this.rate3.Text != string.Empty || this.term3.Text != string.Empty)
        this.list.Add(new TemporaryBuydownTypeSettingDialog.RateTerm(this.rate3.Text, this.term3.Text));
      if (this.rate4.Text != string.Empty || this.term4.Text != string.Empty)
        this.list.Add(new TemporaryBuydownTypeSettingDialog.RateTerm(this.rate4.Text, this.term4.Text));
      if (this.rate5.Text != string.Empty || this.term5.Text != string.Empty)
        this.list.Add(new TemporaryBuydownTypeSettingDialog.RateTerm(this.rate5.Text, this.term5.Text));
      if (!(this.rate6.Text != string.Empty) && !(this.term6.Text != string.Empty))
        return;
      this.list.Add(new TemporaryBuydownTypeSettingDialog.RateTerm(this.rate6.Text, this.term6.Text));
    }

    private void cancelBtn_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void loadTemporaryBuydown(TemporaryBuydown buydown)
    {
      this.buydownTypeName.Text = buydown.BuydownType == null ? "" : buydown.BuydownType;
      this.buydownTypeDescription.Text = buydown.Description == null ? "" : buydown.Description;
      this.rate1.Text = string.IsNullOrEmpty(buydown.Rate1) ? "" : string.Format("{0:#,##0.000}", (object) Decimal.Parse(buydown.Rate1));
      this.term1.Text = string.IsNullOrEmpty(buydown.Term1) ? "" : string.Format("{0:#,##}", (object) int.Parse(buydown.Term1));
      this.rate2.Text = string.IsNullOrEmpty(buydown.Rate2) ? "" : string.Format("{0:#,##0.000}", (object) Decimal.Parse(buydown.Rate2));
      this.term2.Text = string.IsNullOrEmpty(buydown.Term2) ? "" : string.Format("{0:#,##}", (object) int.Parse(buydown.Term2));
      this.rate3.Text = string.IsNullOrEmpty(buydown.Rate3) ? "" : string.Format("{0:#,##0.000}", (object) Decimal.Parse(buydown.Rate3));
      this.term3.Text = string.IsNullOrEmpty(buydown.Term3) ? "" : string.Format("{0:#,##}", (object) int.Parse(buydown.Term3));
      this.rate4.Text = string.IsNullOrEmpty(buydown.Rate4) ? "" : string.Format("{0:#,##0.000}", (object) Decimal.Parse(buydown.Rate4));
      this.term4.Text = string.IsNullOrEmpty(buydown.Term4) ? "" : string.Format("{0:#,##}", (object) int.Parse(buydown.Term4));
      this.rate5.Text = string.IsNullOrEmpty(buydown.Rate5) ? "" : string.Format("{0:#,##0.000}", (object) Decimal.Parse(buydown.Rate5));
      this.term5.Text = string.IsNullOrEmpty(buydown.Term5) ? "" : string.Format("{0:#,##}", (object) int.Parse(buydown.Term5));
      this.rate6.Text = string.IsNullOrEmpty(buydown.Rate6) ? "" : string.Format("{0:#,##0.000}", (object) Decimal.Parse(buydown.Rate6));
      this.term6.Text = string.IsNullOrEmpty(buydown.Term6) ? "" : string.Format("{0:#,##}", (object) int.Parse(buydown.Term6));
    }

    private void rate_Valitation(object sender, KeyPressEventArgs e)
    {
      if (!char.IsControl(e.KeyChar) && (sender as TextBox).Text.Count<char>() >= 15 && (sender as TextBox).SelectionLength == 0)
        e.Handled = true;
      if (!char.IsControl(e.KeyChar) && ((sender as TextBox).Text == "0" && e.KeyChar != '.' || e.KeyChar == '.' && (sender as TextBox).Text.Count<char>() == 0))
        e.Handled = true;
      if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
        e.Handled = true;
      if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
        e.Handled = true;
      if (char.IsControl(e.KeyChar) || !Regex.IsMatch((sender as TextBox).Text, "\\.\\d\\d\\d") || (sender as TextBox).SelectionStart <= (sender as TextBox).Text.IndexOf('.'))
        return;
      e.Handled = true;
    }

    private void rate_LostFocus(object sender, EventArgs e)
    {
      string text = (sender as TextBox).Text;
      if (string.IsNullOrEmpty(text))
        return;
      if (text.IndexOf('.') == text.Length - 1)
      {
        string s = text.Substring(0, text.Length - 1);
        (sender as TextBox).Text = string.Format("{0:#,##0.000}", (object) Decimal.Parse(s));
      }
      else if (text.IndexOf('.') == -1)
        (sender as TextBox).Text = string.Format("{0:#,##0.000}", (object) Decimal.Parse(text + ".000"));
      else if (text.IndexOf('.') == text.Length - 2)
        (sender as TextBox).Text = string.Format("{0:#,##0.000}", (object) Decimal.Parse(text + "00"));
      else if (text.IndexOf('.') == text.Length - 3)
        (sender as TextBox).Text = string.Format("{0:#,##0.000}", (object) Decimal.Parse(text + "0"));
      else
        (sender as TextBox).Text = string.Format("{0:#,##0.000}", (object) Decimal.Parse(text));
    }

    private void term_Validate(object sender, KeyPressEventArgs e)
    {
      if (!char.IsControl(e.KeyChar) && (sender as TextBox).Text == string.Empty && e.KeyChar == '0')
        e.Handled = true;
      if (!char.IsControl(e.KeyChar) && (sender as TextBox).Text.Count<char>() >= 3 && (sender as TextBox).SelectionLength == 0)
        e.Handled = true;
      if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void rateTerm_GotFocus(object sender, EventArgs e)
    {
      (sender as TextBox).Text = (sender as TextBox).Text.Replace(",", "");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.label26 = new Label();
      this.label25 = new Label();
      this.label24 = new Label();
      this.label23 = new Label();
      this.label22 = new Label();
      this.label21 = new Label();
      this.term6 = new TextBox();
      this.term5 = new TextBox();
      this.term4 = new TextBox();
      this.term3 = new TextBox();
      this.term2 = new TextBox();
      this.term1 = new TextBox();
      this.label20 = new Label();
      this.label19 = new Label();
      this.label18 = new Label();
      this.label17 = new Label();
      this.label16 = new Label();
      this.label15 = new Label();
      this.label14 = new Label();
      this.label13 = new Label();
      this.label12 = new Label();
      this.label11 = new Label();
      this.label10 = new Label();
      this.label9 = new Label();
      this.rate6 = new TextBox();
      this.rate5 = new TextBox();
      this.rate4 = new TextBox();
      this.rate3 = new TextBox();
      this.rate2 = new TextBox();
      this.rate1 = new TextBox();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.buydownTypeDescription = new TextBox();
      this.buydownTypeName = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.label26);
      this.panel1.Controls.Add((Control) this.label25);
      this.panel1.Controls.Add((Control) this.label24);
      this.panel1.Controls.Add((Control) this.label23);
      this.panel1.Controls.Add((Control) this.label22);
      this.panel1.Controls.Add((Control) this.label21);
      this.panel1.Controls.Add((Control) this.term6);
      this.panel1.Controls.Add((Control) this.term5);
      this.panel1.Controls.Add((Control) this.term4);
      this.panel1.Controls.Add((Control) this.term3);
      this.panel1.Controls.Add((Control) this.term2);
      this.panel1.Controls.Add((Control) this.term1);
      this.panel1.Controls.Add((Control) this.label20);
      this.panel1.Controls.Add((Control) this.label19);
      this.panel1.Controls.Add((Control) this.label18);
      this.panel1.Controls.Add((Control) this.label17);
      this.panel1.Controls.Add((Control) this.label16);
      this.panel1.Controls.Add((Control) this.label15);
      this.panel1.Controls.Add((Control) this.label14);
      this.panel1.Controls.Add((Control) this.label13);
      this.panel1.Controls.Add((Control) this.label12);
      this.panel1.Controls.Add((Control) this.label11);
      this.panel1.Controls.Add((Control) this.label10);
      this.panel1.Controls.Add((Control) this.label9);
      this.panel1.Controls.Add((Control) this.rate6);
      this.panel1.Controls.Add((Control) this.rate5);
      this.panel1.Controls.Add((Control) this.rate4);
      this.panel1.Controls.Add((Control) this.rate3);
      this.panel1.Controls.Add((Control) this.rate2);
      this.panel1.Controls.Add((Control) this.rate1);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.label7);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.buydownTypeDescription);
      this.panel1.Controls.Add((Control) this.buydownTypeName);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Location = new Point(7, 9);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(338, 217);
      this.panel1.TabIndex = 0;
      this.label26.AutoSize = true;
      this.label26.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label26.Location = new Point(290, 187);
      this.label26.Name = "label26";
      this.label26.Size = new Size(29, 13);
      this.label26.TabIndex = 38;
      this.label26.Text = "mths";
      this.label25.AutoSize = true;
      this.label25.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label25.Location = new Point(290, 161);
      this.label25.Name = "label25";
      this.label25.Size = new Size(29, 13);
      this.label25.TabIndex = 32;
      this.label25.Text = "mths";
      this.label24.AutoSize = true;
      this.label24.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label24.Location = new Point(290, 136);
      this.label24.Name = "label24";
      this.label24.Size = new Size(29, 13);
      this.label24.TabIndex = 26;
      this.label24.Text = "mths";
      this.label23.AutoSize = true;
      this.label23.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label23.Location = new Point(290, 112);
      this.label23.Name = "label23";
      this.label23.Size = new Size(29, 13);
      this.label23.TabIndex = 20;
      this.label23.Text = "mths";
      this.label22.AutoSize = true;
      this.label22.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label22.Location = new Point(290, 87);
      this.label22.Name = "label22";
      this.label22.Size = new Size(29, 13);
      this.label22.TabIndex = 35;
      this.label22.Text = "mths";
      this.label21.AutoSize = true;
      this.label21.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label21.Location = new Point(290, 63);
      this.label21.Name = "label21";
      this.label21.Size = new Size(29, 13);
      this.label21.TabIndex = 9;
      this.label21.Text = "mths";
      this.term6.Location = new Point(215, 184);
      this.term6.Name = "term6";
      this.term6.Size = new Size(69, 20);
      this.term6.TabIndex = 37;
      this.term5.Location = new Point(215, 158);
      this.term5.Name = "term5";
      this.term5.Size = new Size(70, 20);
      this.term5.TabIndex = 31;
      this.term4.Location = new Point(215, 133);
      this.term4.Name = "term4";
      this.term4.Size = new Size(69, 20);
      this.term4.TabIndex = 25;
      this.term3.Location = new Point(215, 109);
      this.term3.Name = "term3";
      this.term3.Size = new Size(69, 20);
      this.term3.TabIndex = 19;
      this.term2.Location = new Point(215, 84);
      this.term2.Name = "term2";
      this.term2.Size = new Size(69, 20);
      this.term2.TabIndex = 14;
      this.term1.Location = new Point(215, 59);
      this.term1.Name = "term1";
      this.term1.Size = new Size(69, 20);
      this.term1.TabIndex = 8;
      this.label20.AutoSize = true;
      this.label20.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label20.Location = new Point(180, 187);
      this.label20.Name = "label20";
      this.label20.Size = new Size(31, 13);
      this.label20.TabIndex = 36;
      this.label20.Text = "Term";
      this.label19.AutoSize = true;
      this.label19.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label19.Location = new Point(180, 161);
      this.label19.Name = "label19";
      this.label19.Size = new Size(31, 13);
      this.label19.TabIndex = 30;
      this.label19.Text = "Term";
      this.label18.AutoSize = true;
      this.label18.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label18.Location = new Point(180, 136);
      this.label18.Name = "label18";
      this.label18.Size = new Size(31, 13);
      this.label18.TabIndex = 24;
      this.label18.Text = "Term";
      this.label17.AutoSize = true;
      this.label17.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label17.Location = new Point(180, 112);
      this.label17.Name = "label17";
      this.label17.Size = new Size(31, 13);
      this.label17.TabIndex = 18;
      this.label17.Text = "Term";
      this.label16.AutoSize = true;
      this.label16.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label16.Location = new Point(180, 87);
      this.label16.Name = "label16";
      this.label16.Size = new Size(31, 13);
      this.label16.TabIndex = 13;
      this.label16.Text = "Term";
      this.label15.AutoSize = true;
      this.label15.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label15.Location = new Point(180, 63);
      this.label15.Name = "label15";
      this.label15.Size = new Size(31, 13);
      this.label15.TabIndex = 7;
      this.label15.Text = "Term";
      this.label14.AutoSize = true;
      this.label14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label14.Location = new Point(146, 187);
      this.label14.Name = "label14";
      this.label14.Size = new Size(15, 13);
      this.label14.TabIndex = 35;
      this.label14.Text = "%";
      this.label13.AutoSize = true;
      this.label13.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(146, 161);
      this.label13.Name = "label13";
      this.label13.Size = new Size(15, 13);
      this.label13.TabIndex = 29;
      this.label13.Text = "%";
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(146, 136);
      this.label12.Name = "label12";
      this.label12.Size = new Size(15, 13);
      this.label12.TabIndex = 23;
      this.label12.Text = "%";
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(146, 112);
      this.label11.Name = "label11";
      this.label11.Size = new Size(15, 13);
      this.label11.TabIndex = 17;
      this.label11.Text = "%";
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(146, 87);
      this.label10.Name = "label10";
      this.label10.Size = new Size(15, 13);
      this.label10.TabIndex = 12;
      this.label10.Text = "%";
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(146, 63);
      this.label9.Name = "label9";
      this.label9.Size = new Size(15, 13);
      this.label9.TabIndex = 6;
      this.label9.Text = "%";
      this.rate6.Location = new Point(40, 184);
      this.rate6.Name = "rate6";
      this.rate6.Size = new Size(100, 20);
      this.rate6.TabIndex = 34;
      this.rate5.Location = new Point(40, 158);
      this.rate5.Name = "rate5";
      this.rate5.Size = new Size(100, 20);
      this.rate5.TabIndex = 28;
      this.rate4.Location = new Point(40, 133);
      this.rate4.Name = "rate4";
      this.rate4.Size = new Size(100, 20);
      this.rate4.TabIndex = 22;
      this.rate3.Location = new Point(40, 109);
      this.rate3.Name = "rate3";
      this.rate3.Size = new Size(100, 20);
      this.rate3.TabIndex = 16;
      this.rate2.Location = new Point(40, 84);
      this.rate2.Name = "rate2";
      this.rate2.Size = new Size(100, 20);
      this.rate2.TabIndex = 11;
      this.rate1.Location = new Point(40, 60);
      this.rate1.Name = "rate1";
      this.rate1.Size = new Size(100, 20);
      this.rate1.TabIndex = 5;
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(4, 187);
      this.label8.Name = "label8";
      this.label8.Size = new Size(30, 13);
      this.label8.TabIndex = 33;
      this.label8.Text = "Rate";
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(4, 161);
      this.label7.Name = "label7";
      this.label7.Size = new Size(30, 13);
      this.label7.TabIndex = 27;
      this.label7.Text = "Rate";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(4, 136);
      this.label6.Name = "label6";
      this.label6.Size = new Size(30, 13);
      this.label6.TabIndex = 21;
      this.label6.Text = "Rate";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(4, 112);
      this.label5.Name = "label5";
      this.label5.Size = new Size(30, 13);
      this.label5.TabIndex = 15;
      this.label5.Text = "Rate";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(4, 87);
      this.label4.Name = "label4";
      this.label4.Size = new Size(30, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "Rate";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(4, 63);
      this.label3.Name = "label3";
      this.label3.Size = new Size(30, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Rate";
      this.buydownTypeDescription.Location = new Point(88, 33);
      this.buydownTypeDescription.Name = "buydownTypeDescription";
      this.buydownTypeDescription.Size = new Size(231, 20);
      this.buydownTypeDescription.TabIndex = 3;
      this.buydownTypeName.Location = new Point(88, 8);
      this.buydownTypeName.Name = "buydownTypeName";
      this.buydownTypeName.Size = new Size(231, 20);
      this.buydownTypeName.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(4, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Description";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(4, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(78, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Buydown Type";
      this.okBtn.Location = new Point(189, 232);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 1;
      this.okBtn.Text = "OK";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Location = new Point(270, 232);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 2;
      this.cancelBtn.Text = "Cancel";
      this.cancelBtn.UseVisualStyleBackColor = true;
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(353, 261);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TemporaryBuydownTypeSettingDialog);
      this.ShowIcon = false;
      this.Text = "Buydown Type";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private class RateTerm
    {
      private string rate;
      private string term;

      public RateTerm(string rate, string term)
      {
        this.rate = rate;
        this.term = term;
      }

      public string Rate
      {
        get => this.rate;
        set => this.rate = value;
      }

      public string Term
      {
        get => this.term;
        set => this.term = value;
      }
    }
  }
}
