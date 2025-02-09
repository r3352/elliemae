// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.POCDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class POCDialog : Form
  {
    private LoanData loan;
    private string[] lineItemFields;
    private double borPaidAmt;
    private string borPaidBy = string.Empty;
    private bool isPOC;
    private bool isPTC;
    private PopupBusinessRules popupRules;
    private bool paidByFieldEditable = true;
    private bool useField1663Only;
    private IHtmlInput inputData;
    private Dictionary<string, string> dirtyFields;
    private IContainer components;
    private CheckBox chkPOC;
    private Label label1;
    private TextBox txtPOCAmount;
    private Label label2;
    private ComboBox cboPaidBy;
    private Button btnOK;
    private PictureBox pictureBox1;
    private Panel panel1;
    private EMHelpLink emHelpLink1;
    private PictureBox pboxDownArrow;
    private PictureBox pboxAsterisk;
    private ToolTip fieldToolTip;
    private CheckBox chkPTC;
    private Label label3;
    private ComboBox cboPTCPaidBy;
    private TextBox txtPTCAmount;
    private Label label4;
    private Label label5;
    private Label label6;
    private TextBox txtBorAmount;
    private Label label9;
    private Label label8;
    private CheckBox chkFinanced;
    private CheckBox chkAPR;
    private TextBox txtTotalAmount;
    private Label label10;
    private Label label11;
    private TextBox txtAffiliateAmount;
    private Label label12;
    private ComboBox cboPaidTo;
    private Label label7;
    private Panel panelAffiliate;

    public POCDialog(string pocID, IHtmlInput inputData)
    {
      this.inputData = inputData;
      if (this.inputData is LoanData)
        this.loan = (LoanData) this.inputData;
      this.InitializeComponent();
      if (pocID == "POPT.X12" && this.getField("NEWHUD.X1139") != "Y" && this.getField("1663") != string.Empty)
      {
        this.useField1663Only = true;
        this.txtPOCAmount.ReadOnly = true;
        this.txtPOCAmount.Enabled = false;
        this.cboPaidBy.Enabled = false;
      }
      if (pocID != "POPT.X54" && pocID != "POPT.X55")
        this.panelAffiliate.Visible = false;
      this.panel1.BackColor = this.label1.BackColor = this.label2.BackColor = Color.FromArgb(216, (int) byte.MaxValue, (int) byte.MaxValue);
      this.TransparencyKey = this.pictureBox1.BackColor = Color.FromArgb(114, 195, 248);
      this.lineItemFields = (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) pocID];
      this.chkPOC.Tag = (object) this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POC];
      this.txtPOCAmount.Tag = (object) this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POCAMT];
      this.cboPaidBy.Tag = (object) this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY];
      this.chkPTC.Tag = (object) this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTC];
      this.txtPTCAmount.Tag = (object) this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT];
      this.cboPTCPaidBy.Tag = (object) this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY];
      this.borPaidAmt = Utils.ParseDouble((object) this.getField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]));
      if (this.useField1663Only)
        this.borPaidAmt = Utils.ParseDouble((object) this.getField("1663")) * -1.0;
      this.borPaidBy = this.getField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY]);
      bool flag = false;
      if (this.loan != null && Session.LoanDataMgr != null && Session.LoanDataMgr.SystemConfiguration != null)
        flag = Session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting.EnableLOCompensationRule((IHtmlInput) this.loan);
      this.paidByFieldEditable = !flag || Session.LoanDataMgr != null && !Session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting.PaidByFieldSyncIsRequired(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], (IHtmlInput) this.loan);
      this.chkPOC.Checked = this.isPOC = this.getField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POC]) == "Y";
      this.txtPOCAmount.Text = this.getField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]);
      this.cboPaidBy.Text = this.paidByToUIValue(this.getField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]));
      this.fieldToolTip.SetToolTip((Control) this.chkPOC, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POC]);
      this.fieldToolTip.SetToolTip((Control) this.txtPOCAmount, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]);
      this.fieldToolTip.SetToolTip((Control) this.cboPaidBy, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]);
      this.chkPTC.Checked = this.isPTC = this.getField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTC]) == "Y";
      this.txtPTCAmount.Text = this.getField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]);
      this.cboPTCPaidBy.Text = this.paidByToUIValue(this.getField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]));
      this.txtBorAmount.Text = this.getField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY]);
      this.chkFinanced.Checked = this.getField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_FINANCED]) == "Y";
      if (this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X225")
        this.chkAPR.Enabled = false;
      else
        this.chkAPR.Checked = this.getField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_APR]) == "Y";
      this.txtTotalAmount.Text = this.borPaidAmt != 0.0 ? this.borPaidAmt.ToString("N2") : "";
      this.fieldToolTip.SetToolTip((Control) this.chkPTC, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTC]);
      this.fieldToolTip.SetToolTip((Control) this.txtPTCAmount, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]);
      this.fieldToolTip.SetToolTip((Control) this.cboPTCPaidBy, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]);
      this.fieldToolTip.SetToolTip((Control) this.txtBorAmount, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY]);
      this.fieldToolTip.SetToolTip((Control) this.chkFinanced, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_FINANCED]);
      this.fieldToolTip.SetToolTip((Control) this.chkAPR, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_APR]);
      this.fieldToolTip.SetToolTip((Control) this.txtTotalAmount, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]);
      if (pocID == "POPT.X54" || pocID == "POPT.X55")
      {
        this.cboPaidTo.Tag = pocID == "POPT.X54" ? (object) "NEWHUD.X804" : (object) "NEWHUD.X805";
        this.txtAffiliateAmount.Tag = pocID == "POPT.X54" ? (object) "NEWHUD.X1724" : (object) "NEWHUD.X1725";
        this.fieldToolTip.SetToolTip((Control) this.cboPaidTo, pocID == "POPT.X54" ? "NEWHUD.X804" : "NEWHUD.X805");
        this.fieldToolTip.SetToolTip((Control) this.txtAffiliateAmount, pocID == "POPT.X54" ? "NEWHUD.X1724" : "NEWHUD.X1725");
        switch (this.getField(pocID == "POPT.X54" ? "NEWHUD.X804" : "NEWHUD.X805"))
        {
          case "Affiliate":
            this.cboPaidTo.Text = "A";
            break;
          case "Broker":
            this.cboPaidTo.Text = "B";
            break;
          case "Lender":
            this.cboPaidTo.Text = "L";
            break;
          case "Seller":
            this.cboPaidTo.Text = "S";
            break;
          case "Investor":
            this.cboPaidTo.Text = "I";
            break;
          case "Other":
            this.cboPaidTo.Text = "O";
            break;
          default:
            this.cboPaidTo.Text = "";
            break;
        }
        if (this.cboPaidTo.Text == "A")
        {
          this.txtAffiliateAmount.ReadOnly = false;
          this.txtAffiliateAmount.Enabled = true;
          this.txtAffiliateAmount.Text = this.getField(pocID == "POPT.X54" ? "NEWHUD.X1724" : "NEWHUD.X1725");
        }
        else
        {
          this.txtAffiliateAmount.Text = "";
          this.txtAffiliateAmount.Enabled = false;
          this.txtAffiliateAmount.ReadOnly = true;
        }
      }
      this.setBusinessRule(pocID);
      if (!this.paidByFieldEditable)
        this.cboPaidBy.Enabled = this.cboPTCPaidBy.Enabled = false;
      else if (flag && this.borPaidBy != "" && Session.LoanDataMgr != null && Session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting.IsPaidByFieldCompensationEnabled(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], (IHtmlInput) this.loan))
        this.cboPaidBy.Items.RemoveAt(0);
      this.txtPOCAmount.TextChanged += new EventHandler(this.txtPOCAmount_TextChanged);
      this.txtPTCAmount.TextChanged += new EventHandler(this.txtPTCAmount_TextChanged);
      this.calculateBorPaidAmount(false);
    }

    private void setBusinessRule(string pocID)
    {
      if (this.loan == null || this.loan.IsTemplate)
        return;
      ResourceManager resources = new ResourceManager(typeof (POCDialog));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
      this.popupRules.SetBusinessRules((object) this.chkPOC, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POC]);
      this.popupRules.SetBusinessRules((object) this.txtPOCAmount, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]);
      this.popupRules.SetBusinessRules((object) this.cboPaidBy, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]);
      this.popupRules.SetBusinessRules((object) this.chkPTC, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTC]);
      this.popupRules.SetBusinessRules((object) this.txtPTCAmount, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]);
      this.popupRules.SetBusinessRules((object) this.cboPTCPaidBy, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]);
      this.popupRules.SetBusinessRules((object) this.chkAPR, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_APR]);
      this.popupRules.SetBusinessRules((object) this.chkFinanced, this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_FINANCED]);
      if (!(pocID == "POPT.X54") && !(pocID == "POPT.X55"))
        return;
      this.popupRules.SetBusinessRules((object) this.txtAffiliateAmount, pocID == "POPT.X54" ? "NEWHUD.X804" : "NEWHUD.X805");
    }

    private void chkPOC_Click(object sender, EventArgs e) => this.calculateBorPaidAmount(true);

    private void chkPTC_Click(object sender, EventArgs e) => this.calculateBorPaidAmount(false);

    private void txtPOCAmount_TextChanged(object sender, EventArgs e)
    {
      this.calculateBorPaidAmount(true);
    }

    private void txtPTCAmount_TextChanged(object sender, EventArgs e)
    {
      this.calculateBorPaidAmount(false);
    }

    private void calculateBorPaidAmount(bool pocInFocus)
    {
      this.txtPOCAmount.TextChanged -= new EventHandler(this.txtPOCAmount_TextChanged);
      this.txtPTCAmount.TextChanged -= new EventHandler(this.txtPTCAmount_TextChanged);
      double num1 = Utils.ParseDouble((object) this.txtPOCAmount.Text.Trim());
      double num2 = Utils.ParseDouble((object) this.txtPTCAmount.Text.Trim());
      if (this.cboPaidBy.Tag.ToString() == "NEWHUD.X232")
      {
        if (pocInFocus)
        {
          num2 = this.borPaidAmt - num1;
          this.txtPTCAmount.Text = num2 != 0.0 ? num2.ToString("N2") : "";
        }
        else
        {
          num1 = this.borPaidAmt - num2;
          this.txtPOCAmount.Text = num1 != 0.0 ? num1.ToString("N2") : "";
        }
      }
      if (num1 == 0.0)
      {
        this.chkPOC.Checked = false;
        this.cboPaidBy.SelectedIndex = -1;
      }
      if (num2 == 0.0)
      {
        this.chkPTC.Checked = false;
        this.cboPTCPaidBy.SelectedIndex = -1;
      }
      if (num1 + num2 > this.borPaidAmt && this.borPaidAmt > 0.0)
      {
        if (pocInFocus)
        {
          if (num1 > this.borPaidAmt)
          {
            num1 = this.borPaidAmt;
            this.txtPOCAmount.Text = num1 != 0.0 ? num1.ToString("N2") : "";
            this.txtPOCAmount.SelectionStart = 0;
            this.txtPOCAmount.SelectionLength = this.txtPOCAmount.Text.Trim().Length;
            num2 = 0.0;
          }
          else
            num2 = this.borPaidAmt - num1;
          this.txtPTCAmount.Text = num2 != 0.0 ? num2.ToString("N2") : "";
        }
        else
        {
          if (num2 > this.borPaidAmt)
          {
            num2 = this.borPaidAmt;
            this.txtPTCAmount.Text = num2 != 0.0 ? num2.ToString("N2") : "";
            this.txtPTCAmount.SelectionStart = 0;
            this.txtPTCAmount.SelectionLength = this.txtPTCAmount.Text.Trim().Length;
            num1 = 0.0;
          }
          else
            num1 = this.borPaidAmt - num2;
          this.txtPOCAmount.Text = num1 != 0.0 ? num1.ToString("N2") : "";
        }
      }
      if (!this.paidByFieldEditable)
      {
        num2 = 0.0;
        this.txtPTCAmount.Text = string.Empty;
        this.txtPTCAmount.ReadOnly = true;
        this.txtPTCAmount.Enabled = false;
        this.cboPTCPaidBy.SelectedIndex = -1;
      }
      double num3 = this.borPaidAmt - num1 - num2;
      this.txtBorAmount.Text = num3 != 0.0 ? num3.ToString("N2") : "";
      if (num3 <= 0.0)
        this.chkFinanced.Enabled = this.chkFinanced.Checked = false;
      else
        this.chkFinanced.Enabled = true;
      this.chkPTC.Checked = num2 != 0.0;
      this.chkPOC.Checked = num1 != 0.0;
      if (num2 == this.borPaidAmt)
      {
        this.cboPTCPaidBy.Text = this.paidByToUIValue(this.borPaidBy);
        this.chkPOC.Checked = this.cboPaidBy.Enabled = false;
      }
      else if (num1 == this.borPaidAmt)
      {
        this.cboPaidBy.Text = this.paidByToUIValue(this.borPaidBy);
        this.chkPTC.Checked = this.cboPTCPaidBy.Enabled = false;
      }
      else
      {
        if (num2 > 0.0 && this.cboPTCPaidBy.Text == string.Empty)
          this.cboPTCPaidBy.Text = this.paidByToUIValue("Lender");
        if (!this.useField1663Only)
          this.cboPaidBy.Enabled = true;
        this.cboPTCPaidBy.Enabled = true;
      }
      if (this.cboPaidBy.Tag.ToString() == "NEWHUD.X232")
      {
        this.cboPaidBy.Enabled = this.cboPTCPaidBy.Enabled = false;
        if (num1 > 0.0)
          this.cboPaidBy.Text = "L";
        if (num2 > 0.0)
          this.cboPTCPaidBy.Text = "L";
      }
      else if (this.cboPaidBy.Tag.ToString() == "NEWHUD.X888")
      {
        this.cboPTCPaidBy.Enabled = false;
        this.cboPaidBy.SelectedIndex = this.cboPTCPaidBy.SelectedIndex = -1;
      }
      if (!this.paidByFieldEditable || this.cboPaidBy.Tag.ToString() == "NEWHUD.X888")
        this.cboPaidBy.Enabled = this.cboPTCPaidBy.Enabled = false;
      if (string.Concat(this.cboPaidBy.Tag) == "")
        this.cboPaidBy.Enabled = false;
      if (string.Concat(this.txtPOCAmount.Tag) == "")
      {
        this.txtPOCAmount.ReadOnly = true;
        this.txtPOCAmount.Enabled = false;
      }
      this.txtPOCAmount.TextChanged += new EventHandler(this.txtPOCAmount_TextChanged);
      this.txtPTCAmount.TextChanged += new EventHandler(this.txtPTCAmount_TextChanged);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      double num1 = Utils.ParseDouble((object) this.txtPOCAmount.Text);
      double num2 = Utils.ParseDouble((object) this.txtPTCAmount.Text);
      double num3 = Utils.ParseDouble((object) this.txtTotalAmount.Text);
      double num4 = Utils.ParseDouble((object) this.txtAffiliateAmount.Text);
      if (num1 < 0.0)
      {
        int num5 = (int) Utils.Dialog((IWin32Window) this, "The POC fee can not be less than 0.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (num1 != 0.0 && this.borPaidAmt < num1)
      {
        int num6 = (int) Utils.Dialog((IWin32Window) this, "The POC fee can not exceed borrower paid fee.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.useField1663Only)
        {
          if (this.borPaidAmt < 0.0 && (num2 > 0.0 || num2 < this.borPaidAmt) || this.borPaidAmt > 0.0 && (num2 < 0.0 || num2 > this.borPaidAmt))
          {
            int num7 = (int) Utils.Dialog((IWin32Window) this, "The PTC fee can not exceed borrower paid fee.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        else if (num2 != 0.0 && this.borPaidAmt < num2 || this.borPaidAmt < Utils.ArithmeticRounding(num1 + num2, 2))
        {
          int num8 = (int) Utils.Dialog((IWin32Window) this, "The PTC fee can not exceed borrower paid fee.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (num4 != 0.0 && num4 > num3)
        {
          int num9 = (int) Utils.Dialog((IWin32Window) this, "The Amount Retained by Affiliate can not exceed total fee.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtAffiliateAmount.Focus();
        }
        else
        {
          if (this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POC] != string.Empty)
          {
            this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POC], this.chkPOC.Checked ? "Y" : "");
            this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POCAMT], this.txtPOCAmount.Text);
            this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], this.txtPOCAmount.Text.Trim() != string.Empty ? this.paidByToFieldValue(this.cboPaidBy.Text) : "");
          }
          this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_APR], this.chkAPR.Checked ? "Y" : "");
          this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTC], this.chkPTC.Checked ? "Y" : "");
          this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], this.txtPTCAmount.Text);
          this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], this.txtPTCAmount.Text.Trim() != string.Empty ? this.paidByToFieldValue(this.cboPTCPaidBy.Text) : "");
          this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY], this.txtBorAmount.Text);
          this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_FINANCED], this.chkFinanced.Checked ? "Y" : "");
          this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPOC], this.chkPTC.Checked || this.chkPOC.Checked ? "Y" : "");
          if (!this.chkPOC.Checked && !this.chkPTC.Checked || this.txtBorAmount.Text.Trim() != string.Empty)
            this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "");
          if (this.chkPOC.Checked && num1 != 0.0 && this.borPaidAmt == num1)
            this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], this.paidByToFieldValue(this.cboPaidBy.Text));
          else if (this.chkPTC.Checked && num2 != 0.0 && this.borPaidAmt == num2)
            this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], this.paidByToFieldValue(this.cboPTCPaidBy.Text));
          else if (this.borPaidAmt != 0.0 && this.txtBorAmount.Text.Trim() == string.Empty)
            this.setField(this.lineItemFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], this.paidByToFieldValue(this.cboPTCPaidBy.Text));
          if (this.panelAffiliate.Visible)
            this.setField(this.txtAffiliateAmount.Tag.ToString(), this.txtAffiliateAmount.Text);
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void numeric_KeyUp(object sender, KeyEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, FieldFormat.DECIMAL_2, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void POCDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }

    private void txtPOCAmount_Enter(object sender, EventArgs e)
    {
      this.displayFieldID(((Control) sender).Tag.ToString());
    }

    private void cboPaidBy_Enter(object sender, EventArgs e)
    {
      this.displayFieldID(((Control) sender).Tag.ToString());
    }

    private void chkPOC_Enter(object sender, EventArgs e)
    {
      this.displayFieldID(((Control) sender).Tag.ToString());
    }

    private void cboPaidBy_Leave(object sender, EventArgs e)
    {
      ComboBox ctrl = (ComboBox) sender;
      if (!ctrl.Enabled || this.loan == null || this.loan.IsTemplate)
        return;
      this.popupRules.RuleValidate((object) ctrl, (string) ctrl.Tag);
    }

    private string paidByToUIValue(string val)
    {
      switch (val)
      {
        case "Broker":
          return "B";
        case "Lender":
          return "L";
        case "Other":
          return "O";
        default:
          return string.Empty;
      }
    }

    private string paidByToFieldValue(string val)
    {
      switch (val)
      {
        case "B":
          return "Broker";
        case "L":
          return "Lender";
        case "O":
          return "Other";
        default:
          return string.Empty;
      }
    }

    private void displayFieldID(string id)
    {
      if (this.loan == null || this.loan.IsTemplate)
        return;
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(id);
    }

    private void setField(string id, string val)
    {
      if (this.dirtyFields == null)
        this.dirtyFields = new Dictionary<string, string>();
      this.inputData.SetCurrentField(id, val);
      if (this.dirtyFields.ContainsKey(id))
        this.dirtyFields[id] = val;
      else
        this.dirtyFields.Add(id, val);
    }

    public Dictionary<string, string> DirtyFields => this.dirtyFields;

    private string getField(string id) => this.inputData.GetField(id);

    private void numeric_Leave(object sender, EventArgs e)
    {
      TextBox ctrl = (TextBox) sender;
      if (ctrl.Enabled && this.loan != null && !this.loan.IsTemplate && !this.popupRules.RuleValidate((object) ctrl, (string) ctrl.Tag))
        this.calculateBorPaidAmount(ctrl.Name == "txtPOCAmount");
      double num = Utils.ParseDouble((object) ctrl.Text);
      ctrl.Text = num != 0.0 ? num.ToString("N2") : "";
    }

    private void txtAffiliateAmount_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == '.')
        return;
      e.Handled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (POCDialog));
      this.chkPOC = new CheckBox();
      this.label1 = new Label();
      this.txtPOCAmount = new TextBox();
      this.label2 = new Label();
      this.cboPaidBy = new ComboBox();
      this.btnOK = new Button();
      this.panel1 = new Panel();
      this.panelAffiliate = new Panel();
      this.cboPaidTo = new ComboBox();
      this.txtAffiliateAmount = new TextBox();
      this.label7 = new Label();
      this.label12 = new Label();
      this.label11 = new Label();
      this.chkFinanced = new CheckBox();
      this.chkAPR = new CheckBox();
      this.txtTotalAmount = new TextBox();
      this.label10 = new Label();
      this.txtBorAmount = new TextBox();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.chkPTC = new CheckBox();
      this.label3 = new Label();
      this.cboPTCPaidBy = new ComboBox();
      this.txtPTCAmount = new TextBox();
      this.label4 = new Label();
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.emHelpLink1 = new EMHelpLink();
      this.fieldToolTip = new ToolTip(this.components);
      this.pictureBox1 = new PictureBox();
      this.panel1.SuspendLayout();
      this.panelAffiliate.SuspendLayout();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.chkPOC.AutoSize = true;
      this.chkPOC.BackColor = Color.Transparent;
      this.chkPOC.Enabled = false;
      this.chkPOC.Location = new Point(6, 28);
      this.chkPOC.Name = "chkPOC";
      this.chkPOC.Size = new Size(48, 17);
      this.chkPOC.TabIndex = 0;
      this.chkPOC.Text = "POC";
      this.chkPOC.UseVisualStyleBackColor = false;
      this.chkPOC.Enter += new EventHandler(this.chkPOC_Enter);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(152, 29);
      this.label1.Name = "label1";
      this.label1.Size = new Size(13, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "$";
      this.txtPOCAmount.BackColor = SystemColors.Window;
      this.txtPOCAmount.Location = new Point(165, 26);
      this.txtPOCAmount.Name = "txtPOCAmount";
      this.txtPOCAmount.Size = new Size(74, 20);
      this.txtPOCAmount.TabIndex = 2;
      this.txtPOCAmount.TextAlign = HorizontalAlignment.Right;
      this.txtPOCAmount.Enter += new EventHandler(this.txtPOCAmount_Enter);
      this.txtPOCAmount.KeyUp += new KeyEventHandler(this.numeric_KeyUp);
      this.txtPOCAmount.Leave += new EventHandler(this.numeric_Leave);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(54, 29);
      this.label2.Name = "label2";
      this.label2.Size = new Size(42, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Paid by";
      this.cboPaidBy.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPaidBy.FormattingEnabled = true;
      this.cboPaidBy.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cboPaidBy.Location = new Point(102, 25);
      this.cboPaidBy.Name = "cboPaidBy";
      this.cboPaidBy.Size = new Size(45, 21);
      this.cboPaidBy.TabIndex = 1;
      this.cboPaidBy.Enter += new EventHandler(this.cboPaidBy_Enter);
      this.cboPaidBy.Leave += new EventHandler(this.cboPaidBy_Leave);
      this.btnOK.BackColor = SystemColors.Window;
      this.btnOK.Location = new Point(287, 150);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 8;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = false;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.panel1.Controls.Add((Control) this.panelAffiliate);
      this.panel1.Controls.Add((Control) this.label11);
      this.panel1.Controls.Add((Control) this.chkFinanced);
      this.panel1.Controls.Add((Control) this.chkAPR);
      this.panel1.Controls.Add((Control) this.txtTotalAmount);
      this.panel1.Controls.Add((Control) this.label10);
      this.panel1.Controls.Add((Control) this.txtBorAmount);
      this.panel1.Controls.Add((Control) this.label9);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.chkPTC);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.cboPTCPaidBy);
      this.panel1.Controls.Add((Control) this.txtPTCAmount);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.pboxDownArrow);
      this.panel1.Controls.Add((Control) this.pboxAsterisk);
      this.panel1.Controls.Add((Control) this.emHelpLink1);
      this.panel1.Controls.Add((Control) this.chkPOC);
      this.panel1.Controls.Add((Control) this.btnOK);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.cboPaidBy);
      this.panel1.Controls.Add((Control) this.txtPOCAmount);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Location = new Point(2, 2);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(372, 177);
      this.panel1.TabIndex = 9;
      this.panelAffiliate.Controls.Add((Control) this.cboPaidTo);
      this.panelAffiliate.Controls.Add((Control) this.txtAffiliateAmount);
      this.panelAffiliate.Controls.Add((Control) this.label7);
      this.panelAffiliate.Controls.Add((Control) this.label12);
      this.panelAffiliate.Location = new Point(6, 121);
      this.panelAffiliate.Name = "panelAffiliate";
      this.panelAffiliate.Size = new Size(331, 22);
      this.panelAffiliate.TabIndex = 89;
      this.cboPaidTo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPaidTo.Enabled = false;
      this.cboPaidTo.FormattingEnabled = true;
      this.cboPaidTo.Items.AddRange(new object[7]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "S",
        (object) "I",
        (object) "A",
        (object) "O"
      });
      this.cboPaidTo.Location = new Point(48, 0);
      this.cboPaidTo.Name = "cboPaidTo";
      this.cboPaidTo.Size = new Size(45, 21);
      this.cboPaidTo.TabIndex = 85;
      this.txtAffiliateAmount.BackColor = SystemColors.Window;
      this.txtAffiliateAmount.Location = new Point(257, 0);
      this.txtAffiliateAmount.Name = "txtAffiliateAmount";
      this.txtAffiliateAmount.Size = new Size(74, 20);
      this.txtAffiliateAmount.TabIndex = 8;
      this.txtAffiliateAmount.TextAlign = HorizontalAlignment.Right;
      this.txtAffiliateAmount.Enter += new EventHandler(this.txtPOCAmount_Enter);
      this.txtAffiliateAmount.KeyPress += new KeyPressEventHandler(this.txtAffiliateAmount_KeyPress);
      this.txtAffiliateAmount.KeyUp += new KeyEventHandler(this.numeric_KeyUp);
      this.txtAffiliateAmount.Leave += new EventHandler(this.numeric_Leave);
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Location = new Point(3, 3);
      this.label7.Name = "label7";
      this.label7.Size = new Size(44, 13);
      this.label7.TabIndex = 86;
      this.label7.Text = "Paid To";
      this.label12.AutoSize = true;
      this.label12.BackColor = Color.Transparent;
      this.label12.Location = new Point(99, 3);
      this.label12.Name = "label12";
      this.label12.Size = new Size(149, 13);
      this.label12.TabIndex = 87;
      this.label12.Text = "Amount Retained by Affiliate $";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(245, 53);
      this.label11.Name = "label11";
      this.label11.Size = new Size(79, 13);
      this.label11.TabIndex = 84;
      this.label11.Text = "(3rd Party PTC)";
      this.chkFinanced.AutoSize = true;
      this.chkFinanced.BackColor = Color.Transparent;
      this.chkFinanced.Location = new Point(297, 77);
      this.chkFinanced.Name = "chkFinanced";
      this.chkFinanced.Size = new Size(70, 17);
      this.chkFinanced.TabIndex = 7;
      this.chkFinanced.Text = "Financed";
      this.chkFinanced.UseVisualStyleBackColor = false;
      this.chkAPR.AutoSize = true;
      this.chkAPR.BackColor = Color.Transparent;
      this.chkAPR.Location = new Point(246, 77);
      this.chkAPR.Name = "chkAPR";
      this.chkAPR.Size = new Size(48, 17);
      this.chkAPR.TabIndex = 6;
      this.chkAPR.Text = "APR";
      this.chkAPR.UseVisualStyleBackColor = false;
      this.txtTotalAmount.BackColor = SystemColors.Window;
      this.txtTotalAmount.Enabled = false;
      this.txtTotalAmount.Location = new Point(165, 98);
      this.txtTotalAmount.Name = "txtTotalAmount";
      this.txtTotalAmount.Size = new Size(74, 20);
      this.txtTotalAmount.TabIndex = 83;
      this.txtTotalAmount.TextAlign = HorizontalAlignment.Right;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(151, 101);
      this.label10.Name = "label10";
      this.label10.Size = new Size(13, 13);
      this.label10.TabIndex = 82;
      this.label10.Text = "$";
      this.txtBorAmount.BackColor = SystemColors.Window;
      this.txtBorAmount.Enabled = false;
      this.txtBorAmount.Location = new Point(165, 74);
      this.txtBorAmount.Name = "txtBorAmount";
      this.txtBorAmount.Size = new Size(74, 20);
      this.txtBorAmount.TabIndex = 81;
      this.txtBorAmount.TextAlign = HorizontalAlignment.Right;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(152, 77);
      this.label9.Name = "label9";
      this.label9.Size = new Size(13, 13);
      this.label9.TabIndex = 80;
      this.label9.Text = "$";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(66, 77);
      this.label8.Name = "label8";
      this.label8.Size = new Size(82, 13);
      this.label8.TabIndex = 79;
      this.label8.Text = "Borrower to Pay";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(3, 7);
      this.label6.Name = "label6";
      this.label6.Size = new Size(128, 13);
      this.label6.TabIndex = 77;
      this.label6.Text = "Define how fees are paid.";
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(111, 101);
      this.label5.Name = "label5";
      this.label5.Size = new Size(36, 13);
      this.label5.TabIndex = 76;
      this.label5.Text = "Total";
      this.chkPTC.AutoSize = true;
      this.chkPTC.BackColor = Color.Transparent;
      this.chkPTC.Enabled = false;
      this.chkPTC.Location = new Point(6, 53);
      this.chkPTC.Name = "chkPTC";
      this.chkPTC.Size = new Size(47, 17);
      this.chkPTC.TabIndex = 3;
      this.chkPTC.Text = "PTC";
      this.chkPTC.UseVisualStyleBackColor = false;
      this.chkPTC.Enter += new EventHandler(this.chkPOC_Enter);
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(152, 53);
      this.label3.Name = "label3";
      this.label3.Size = new Size(13, 13);
      this.label3.TabIndex = 73;
      this.label3.Text = "$";
      this.cboPTCPaidBy.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPTCPaidBy.FormattingEnabled = true;
      this.cboPTCPaidBy.Items.AddRange(new object[3]
      {
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cboPTCPaidBy.Location = new Point(102, 50);
      this.cboPTCPaidBy.Name = "cboPTCPaidBy";
      this.cboPTCPaidBy.Size = new Size(45, 21);
      this.cboPTCPaidBy.TabIndex = 4;
      this.cboPTCPaidBy.Enter += new EventHandler(this.cboPaidBy_Enter);
      this.cboPTCPaidBy.Leave += new EventHandler(this.cboPaidBy_Leave);
      this.txtPTCAmount.BackColor = SystemColors.Window;
      this.txtPTCAmount.Location = new Point(165, 50);
      this.txtPTCAmount.Name = "txtPTCAmount";
      this.txtPTCAmount.Size = new Size(74, 20);
      this.txtPTCAmount.TabIndex = 5;
      this.txtPTCAmount.TextAlign = HorizontalAlignment.Right;
      this.txtPTCAmount.Enter += new EventHandler(this.txtPOCAmount_Enter);
      this.txtPTCAmount.KeyUp += new KeyEventHandler(this.numeric_KeyUp);
      this.txtPTCAmount.Leave += new EventHandler(this.numeric_Leave);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(54, 54);
      this.label4.Name = "label4";
      this.label4.Size = new Size(42, 13);
      this.label4.TabIndex = 75;
      this.label4.Text = "Paid by";
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(332, 10);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 70;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(297, 12);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 69;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "paidoutsideclosing";
      this.emHelpLink1.Location = new Point(6, 147);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 25;
      this.pictureBox1.BackColor = Color.WhiteSmoke;
      this.pictureBox1.Dock = DockStyle.Fill;
      this.pictureBox1.Image = (Image) EllieMae.EMLite.Properties.Resources.imagePOCEntry;
      this.pictureBox1.Location = new Point(0, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(388, 195);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 8;
      this.pictureBox1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.BackgroundImageLayout = ImageLayout.Center;
      this.ClientSize = new Size(388, 195);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.pictureBox1);
      this.DoubleBuffered = true;
      this.FormBorderStyle = FormBorderStyle.None;
      this.KeyPreview = true;
      this.Name = nameof (POCDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.Manual;
      this.Text = nameof (POCDialog);
      this.TransparencyKey = Color.WhiteSmoke;
      this.KeyPress += new KeyPressEventHandler(this.POCDialog_KeyPress);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panelAffiliate.ResumeLayout(false);
      this.panelAffiliate.PerformLayout();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
