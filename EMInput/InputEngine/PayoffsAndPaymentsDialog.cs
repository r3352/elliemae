// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PayoffsAndPaymentsDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PayoffsAndPaymentsDialog : Form
  {
    private LoanData loan;
    private IMainScreen mainScreen;
    private FieldDefinition fl0062Options;
    private FieldDefinition unfl0001Options;
    private bool isAlternate;
    private string lastUCDType = string.Empty;
    private int totalnonvol;
    public bool ByPassCopyCondition;
    public string PocOrgAmount = string.Empty;
    public string PocNewAmount = string.Empty;
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Dictionary<int, List<string>> nonVolCollection = new Dictionary<int, List<string>>();
    private IContainer components;
    private TableContainer tableContainer1;
    private GridView listViewLiabs;
    private TextBox txtTotal;
    private Label label11;
    private GroupContainer groupContainer1;
    private TextBox txtLoanAmount;
    private TextBox txtTotalCostJ;
    private TextBox txtClosingCostBeforeClosing;
    private TextBox txtTotalPayoffsK;
    private TextBox txtCashToClose;
    private Label label9;
    private Label label2;
    private Label label3;
    private Label label7;
    private Label label5;
    private Button okBtn;
    private Button cancelBtn;
    private TextBox txtToFromBorrower;
    private Label label4;
    private Label label1;
    private StandardIconButton deleteButton;
    private StandardIconButton btnAdd;

    public PayoffsAndPaymentsDialog(LoanData loan, IMainScreen mainScreen)
    {
      this.loan = loan;
      this.mainScreen = mainScreen;
      this.InitializeComponent();
      this.fl0062Options = EncompassFields.GetField("FL0062");
      this.unfl0001Options = EncompassFields.GetField("UNFL0001");
      this.isAlternate = loan.GetField("LE2.X28") == "Y";
    }

    private string calculateNonVOlDescription(string pocAmount, string paidBy)
    {
      string nonVolDescription;
      if (this.loan.GetField("LE2.X28") == "Y")
        nonVolDescription = "$" + pocAmount + " Principal Reduction to Borrower (for exceeding legal limits P.O.C. " + paidBy + ")";
      else
        nonVolDescription = "$" + pocAmount + " Principal Reduction for exceeding legal limits P.O.C. " + paidBy;
      return nonVolDescription;
    }

    private string calculatePrincipalReductionDescription(
      bool pocIndicator,
      bool isAlt,
      string pocAmount,
      string paidBy)
    {
      if (!pocIndicator)
        return "Principal Reduction to Borrower";
      return isAlt ? string.Format("${0} Principal Reduction to Borrower (P.O.C. {1})", (object) pocAmount, (object) paidBy) : string.Format("${0} Principal Reduction P.O.C. {1}", (object) pocAmount, (object) paidBy);
    }

    private string parseAdjustmentAmount(string adjustmentDescription)
    {
      int startIndex = adjustmentDescription.IndexOf("$");
      int num = startIndex > -1 ? adjustmentDescription.IndexOf(" ", startIndex) : -1;
      if (num > -1)
      {
        if (startIndex > -1)
        {
          try
          {
            return Utils.ParseDecimal((object) adjustmentDescription.Substring(startIndex + 1, num - startIndex - 1)).ToString();
          }
          catch (Exception ex)
          {
            Tracing.Log(PayoffsAndPaymentsDialog.sw, TraceLevel.Error, "PayoffAndPaymentsDialog", "Unable to parse the Principal Reduction description " + ex.Message);
          }
        }
      }
      return string.Empty;
    }

    private void initForm()
    {
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      int num = 0;
      this.listViewLiabs.Items.Clear();
      this.listViewLiabs.BeginUpdate();
      for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index1]);
        int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
        for (int index2 = 1; index2 <= exlcudingAlimonyJobExp; ++index2)
        {
          ++num;
          string str = "FL" + index2.ToString("00");
          GVItem gvItem = new GVItem(num.ToString());
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) string.Concat((object) (index1 + 1)));
          gvItem.SubItems.Add((object) this.loan.GetSimpleField(str + "02"));
          gvItem.SubItems.Add((object) this.loan.GetSimpleField(str + "43"));
          gvItem.SubItems.Add((object) this.loan.GetField(str + "13"));
          gvItem.SubItems.Add((object) this.loan.GetField(str + "16"));
          gvItem.SubItems.Add((object) this.mapUCDPayoffTypeValue(this.loan.GetField(str + "62"), true));
          gvItem.SubItems.Add((object) index2);
          if (this.loan.GetSimpleField(str + "18") == "Y")
            gvItem.SubItems[0].Checked = true;
          if (this.loan.GetSimpleField(str + "63") == "Y")
            gvItem.SubItems[1].Checked = true;
          gvItem.Tag = (object) (borrowerPairs[index1].Id + "," + str);
          this.listViewLiabs.Items.Add(gvItem);
        }
      }
      this.totalnonvol = this.loan.GetNumberOfNonVols() + 1;
      if (string.IsNullOrEmpty(this.PocOrgAmount) && !string.IsNullOrEmpty(this.PocNewAmount))
      {
        ++num;
        GVItem gvItem = new GVItem(num.ToString());
        gvItem.SubItems.Add((object) "");
        gvItem.SubItems.Add((object) "");
        gvItem.SubItems.Add((object) "");
        string nonVolDescription = this.calculateNonVOlDescription(this.PocNewAmount, "Lender");
        gvItem.SubItems.Add((object) nonVolDescription);
        gvItem.SubItems.Add((object) "");
        gvItem.SubItems.Add((object) "0.00");
        gvItem.SubItems.Add((object) this.mapNonUCDPayoffTypeValue("Other", true));
        gvItem.SubItems[0].CheckBoxEnabled = false;
        gvItem.Tag = (object) ("UNFL" + this.totalnonvol.ToString("00") + "|" + nonVolDescription + "|PrincipalReductionCure");
        gvItem.SubItems.Add((object) "Y");
        gvItem.SubItems.Add((object) "Lender");
        gvItem.SubItems.Add((object) ("$ " + this.PocNewAmount + " Principal Reduction to Borrower (for exceeding legal limits P.O.C. Lender)"));
        this.listViewLiabs.Items.Add(gvItem);
      }
      for (int index = 1; index < this.totalnonvol; ++index)
      {
        string str1 = "UNFL" + index.ToString("00");
        string field1 = this.loan.GetField(str1 + "01");
        string adjustmentDescription = this.loan.GetField(str1 + "02");
        string field2 = this.loan.GetField(str1 + "03");
        string empty = string.Empty;
        if (string.IsNullOrEmpty(this.PocOrgAmount) || !string.IsNullOrEmpty(this.PocNewAmount) || !(field2 == "PrincipalReductionCure"))
        {
          ++num;
          bool boolean = Utils.ParseBoolean((object) this.loan.GetField(str1 + "06"));
          string field3 = this.loan.GetField(str1 + "07");
          string field4 = this.loan.GetField(str1 + "08");
          string str2 = Utils.ParseDecimal((object) this.loan.GetField(str1 + "04"), false, 2).ToString("N2");
          GVItem gvItem = new GVItem(num.ToString());
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) "");
          if (field1 == "Other")
          {
            string str3;
            switch (field2)
            {
              case "PrincipalReduction":
                str3 = this.calculatePrincipalReductionDescription(boolean, this.isAlternate, boolean ? this.parseAdjustmentAmount(adjustmentDescription) : str2, field3);
                break;
              case "PrincipalReductionCure":
                str3 = this.calculateNonVOlDescription(this.ByPassCopyCondition ? this.PocNewAmount : this.loan.GetField("FV.X397"), field3);
                break;
              case "StandardOther":
                str3 = adjustmentDescription;
                break;
              default:
                if (!this.isAlternate || !(field1 == "Other"))
                {
                  str3 = field2;
                  break;
                }
                goto case "StandardOther";
            }
            adjustmentDescription = str3;
            gvItem.SubItems.Add(!this.isAlternate || !(field1 == "Other") || !(field2 != "StandardOther") ? (object) str3 : (object) field2);
          }
          else
            gvItem.SubItems.Add((object) adjustmentDescription);
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) str2);
          gvItem.SubItems.Add((object) this.mapNonUCDPayoffTypeValue(field1, true));
          gvItem.SubItems[0].CheckBoxEnabled = false;
          if (this.loan.GetSimpleField(str1 + "05") == "Y")
            gvItem.SubItems[1].Checked = true;
          gvItem.Tag = (object) (str1 + "|" + adjustmentDescription + "|" + field2);
          gvItem.SubItems.Add((object) boolean);
          gvItem.SubItems.Add((object) field3);
          gvItem.SubItems.Add((object) field4);
          this.listViewLiabs.Items.Add(gvItem);
        }
      }
      this.listViewLiabs.EndUpdate();
      if (currentBorrowerPair != null && (this.loan.CurrentBorrowerPair == null || currentBorrowerPair.Id != this.loan.CurrentBorrowerPair.Id))
        this.loan.SetBorrowerPair(currentBorrowerPair);
      this.populateTextBox();
    }

    private void populateTextBox()
    {
      this.txtTotal.Text = Utils.ParseDouble((object) this.loan.GetSimpleField("CD3.X80")).ToString("N2");
      this.txtLoanAmount.Text = Utils.ParseDouble((object) this.loan.GetSimpleField("CD3.X81")).ToString("N2");
      this.txtClosingCostBeforeClosing.Text = Utils.ParseDouble((object) this.loan.GetSimpleField("CD3.X83")).ToString("N2");
      this.txtTotalPayoffsK.Text = Utils.ParseDouble((object) this.loan.GetSimpleField("CD3.X84")).ToString("N2");
      this.txtToFromBorrower.Text = this.loan.GetSimpleField("CD3.X86").ToString();
      this.txtCashToClose.Text = (Utils.ParseDouble((object) this.loan.GetSimpleField("CD3.X85")) * (this.txtToFromBorrower.Text == "From Borrower" ? -1.0 : 1.0)).ToString("N2");
      this.txtTotalCostJ.Text = Utils.ParseDouble((object) this.loan.GetSimpleField("CD3.X82")).ToString("N2");
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      List<GVItem> gvItemList = new List<GVItem>();
      int num1 = 1;
      for (int index = 1; index <= this.listViewLiabs.Items.Count; ++index)
      {
        if (this.listViewLiabs.Items[index - 1].Tag.ToString().StartsWith("UNFL"))
        {
          string[] strArray = this.listViewLiabs.Items[index - 1].Tag.ToString().Split('|');
          this.nonVolCollection.Add(num1++, new List<string>()
          {
            this.listViewLiabs.Items[index - 1].SubItems[7].Text,
            strArray[1],
            strArray[2],
            this.listViewLiabs.Items[index - 1].SubItems[6].Text,
            this.listViewLiabs.Items[index - 1].SubItems[1].Checked.ToString(),
            this.listViewLiabs.Items[index - 1].SubItems[10].Text,
            this.listViewLiabs.Items[index - 1].SubItems[9].Text
          });
        }
      }
      this.loan.ClearNonVols();
      this.loan.CreateNonVols(this.nonVolCollection);
      int num2 = 0;
      List<int> intList = new List<int>();
      for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
      {
        for (int index2 = 1; index2 <= this.listViewLiabs.Items.Count; ++index2)
        {
          string str1 = this.listViewLiabs.Items[index2 - 1].Tag.ToString();
          string[] strArray1;
          if (str1.StartsWith("UNFL"))
            strArray1 = new string[1]{ str1 };
          else
            strArray1 = str1.Split(',');
          if (strArray1 == null || strArray1.Length != 2 || strArray1[0] != borrowerPairs[index1].Id)
          {
            if (strArray1[0].Contains("exceeding legal limits P.O.C.") && this.listViewLiabs.Items[index2 - 1].SubItems[1].Checked)
              num2 = index2;
            string[] strArray2 = strArray1[0].Split('|');
            if (strArray2.Length >= 3 && index1 == 0 && (strArray2[2].Equals("PrincipalReduction") || strArray2[2].Equals("StandardOther")) && this.listViewLiabs.Items[index2 - 1].SubItems[1].Checked)
              intList.Add(index2);
          }
          else
          {
            if (this.loan.CurrentBorrowerPair.Id != borrowerPairs[index1].Id)
              this.loan.SetBorrowerPair(borrowerPairs[index1]);
            string str2 = strArray1[1];
            if (this.listViewLiabs.Items[index2 - 1].SubItems[0].Checked)
            {
              this.loan.SetCurrentField(str2 + "18", "Y");
              this.loan.SetCurrentField(str2 + "16", this.listViewLiabs.Items[index2 - 1].SubItems[6].Text);
              this.loan.SetCurrentField(str2 + "62", this.mapUCDPayoffTypeValue(this.listViewLiabs.Items[index2 - 1].SubItems[7].Text, false));
            }
            else
            {
              this.loan.SetCurrentField(str2 + "16", "");
              this.loan.SetCurrentField(str2 + "18", "");
              this.loan.SetCurrentField(str2 + "62", "");
            }
            if (this.listViewLiabs.Items[index2 - 1].SubItems[1].Checked)
            {
              this.loan.SetCurrentField(str2 + "63", "Y");
              this.txtTotal.Text += (string) (object) Utils.ParseDouble((object) this.listViewLiabs.Items[index2 - 1].SubItems[0]);
              gvItemList.Add(this.listViewLiabs.Items[index2 - 1]);
            }
            else
              this.loan.SetCurrentField(str2 + "63", "");
            this.loan.SetField(str2 + "43", this.listViewLiabs.Items[index2 - 1].SubItems[4].Text);
          }
        }
      }
      this.loan.SetBorrowerPair(currentBorrowerPair);
      this.loan.Calculator.FormCalculation("CLOSINGDISCLOSUREPAGE3", "", "");
      this.loan.Calculator.FormCalculation("26", "", "");
      if ((this.ByPassCopyCondition || this.loan.GetField("LE2.X28") != "Y") && (this.ByPassCopyCondition || Utils.Dialog((IWin32Window) this, "Do you want to copy Total Payoffs and Payment (K) to line K-04 in the Summaries of Transactions on the CD page 3?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes))
      {
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        int num6 = 0;
        int num7 = 0;
        int num8 = 21;
        Decimal num9 = 0M;
        if (num2 > 0)
          gvItemList.Insert(num7++, this.listViewLiabs.Items[num2 - 1]);
        if (intList.Count > 0)
        {
          foreach (int num10 in intList)
            gvItemList.Insert(num7++, this.listViewLiabs.Items[num10 - 1]);
        }
        for (int index = 0; index < gvItemList.Count || index < num8; ++index)
        {
          if (index >= num8)
          {
            GVItem gvItem = gvItemList[index];
            if (index == num8)
            {
              this.loan.SetField("CD3.X" + (object) num3, "Additional Liabilities");
              this.loan.SetField("CD3.X" + (object) num4, "Additional Liabilities");
              this.loan.SetField("CD3.X" + (object) num6, this.mapUCDPayoffTypeValue("Other", false));
            }
            num9 += Utils.ParseDecimal((object) gvItem.SubItems[6].Text, 0.00M);
            this.loan.SetField("CD3.X" + (object) num5, num9.ToString());
          }
          else
          {
            int num11 = index * 4;
            num3 = 139 + num11;
            num5 = 140 + num11;
            num4 = 141 + num11;
            num6 = 142 + num11;
            if (index < gvItemList.Count)
            {
              GVItem gvItem = gvItemList[index];
              string str3;
              if (gvItem.Tag != null)
              {
                if (gvItem.Tag.ToString().Split('|').Length >= 3)
                {
                  str3 = gvItem.Tag.ToString().Split('|')[2];
                  goto label_47;
                }
              }
              str3 = "";
label_47:
              string str4 = str3;
              this.loan.SetField("CD3.X" + (object) num3, str4 == "StandardOther" ? str4 : gvItem.SubItems[4].Text);
              this.loan.SetField("CD3.X" + (object) num5, gvItem.SubItems[6].Text);
              this.loan.SetField("CD3.X" + (object) num4, num2 <= 0 || index != 0 ? gvItem.SubItems[3].Text : "Borrower");
              this.loan.SetField("CD3.X" + (object) num6, this.mapUCDPayoffTypeValue(gvItem.SubItems[7].Text, false));
              if (index == num8 - 1)
                num9 = Utils.ParseDecimal((object) gvItem.SubItems[6].Text, 0.00M);
            }
            else
            {
              this.loan.SetField("CD3.X" + (object) num3, "");
              this.loan.SetField("CD3.X" + (object) num5, "");
              this.loan.SetField("CD3.X" + (object) num4, "");
              this.loan.SetField("CD3.X" + (object) num6, "");
            }
          }
        }
      }
      Cursor.Current = Cursors.Default;
      this.ByPassCopyCondition = false;
      this.DialogResult = DialogResult.OK;
    }

    private void PayoffsAndPaymentsDialog_Load(object sender, EventArgs e)
    {
      if (this.isAlternate)
        this.btnAdd.Click += new EventHandler(this.btnAddNonVOL_Click);
      else
        this.btnAdd.Click += new EventHandler(this.btnAddPrincipalReduction_Click);
      this.initForm();
    }

    private void listViewLiabs_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.listViewLiabs.SubItemCheck -= new GVSubItemEventHandler(this.listViewLiabs_SubItemCheck);
      bool flag = false;
      if (!e.SubItem.Item.Tag.ToString().StartsWith("UNFL"))
      {
        if (e.SubItem.Index == 0)
        {
          if (e.SubItem.Checked)
          {
            e.SubItem.Item.SubItems[6].Text = e.SubItem.Item.SubItems[5].Text;
            if (!e.SubItem.Item.SubItems[1].Checked)
              e.SubItem.Item.SubItems[1].Checked = true;
            flag = true;
          }
          else
          {
            e.SubItem.Item.SubItems[6].Text = "";
            if (e.SubItem.Item.SubItems[1].Checked)
              e.SubItem.Item.SubItems[1].Checked = false;
            e.SubItem.Item.SubItems[7].Text = "";
          }
        }
        else if (e.SubItem.Index == 1 && e.SubItem.Checked && !e.SubItem.Item.SubItems[0].Checked)
        {
          e.SubItem.Item.SubItems[0].Checked = true;
          e.SubItem.Item.SubItems[6].Text = e.SubItem.Item.SubItems[5].Text;
          flag = true;
        }
        if (flag)
        {
          switch (this.loan.GetField("FL" + (e.SubItem.Item.Index + 1).ToString("00") + "08"))
          {
            case "CollectionsJudgementsAndLiens":
              e.SubItem.Item.SubItems[7].Text = this.mapUCDPayoffTypeValue("CollectionsJudgmentsAndLiens", true);
              break;
            case "HELOC":
              e.SubItem.Item.SubItems[7].Text = this.mapUCDPayoffTypeValue("HELOC", true);
              break;
            case "Installment":
              e.SubItem.Item.SubItems[7].Text = this.mapUCDPayoffTypeValue("Installment", true);
              break;
            case "LeasePayments":
              e.SubItem.Item.SubItems[7].Text = this.mapUCDPayoffTypeValue("LeasePayment", true);
              break;
            case "MortgageLoan":
              e.SubItem.Item.SubItems[7].Text = this.mapUCDPayoffTypeValue("MortgageLoan", true);
              break;
            case "Open30DayChargeAccount":
              e.SubItem.Item.SubItems[7].Text = this.mapUCDPayoffTypeValue("Open30DayChargeAccount", true);
              break;
            case "OtherExpense":
            case "OtherLiability":
              e.SubItem.Item.SubItems[7].Text = this.mapUCDPayoffTypeValue("Other", true);
              break;
            case "Revolving":
              e.SubItem.Item.SubItems[7].Text = this.mapUCDPayoffTypeValue("Revolving", true);
              break;
            case "Taxes":
              e.SubItem.Item.SubItems[7].Text = this.mapUCDPayoffTypeValue("Taxes", true);
              break;
            default:
              e.SubItem.Item.SubItems[7].Text = "";
              break;
          }
        }
      }
      this.calculateTotal();
      this.listViewLiabs.SubItemCheck += new GVSubItemEventHandler(this.listViewLiabs_SubItemCheck);
    }

    private void listViewLiabs_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      bool flag = e.SubItem.Item.Tag != null && e.SubItem.Item.Tag.ToString().StartsWith("UNFL");
      if (e.SubItem.Index == 7 && (e.SubItem.Item.SubItems[0].Checked && this.fl0062Options != null && this.fl0062Options.Options != null && this.fl0062Options.Options.Count > 0 || flag && this.unfl0001Options != null && this.unfl0001Options.Options != null && this.unfl0001Options.Options.Count > 0))
      {
        ComboBox editorControl = (ComboBox) e.EditorControl;
        editorControl.Visible = true;
        editorControl.Items.Clear();
        editorControl.DropDownStyle = ComboBoxStyle.DropDownList;
        if (flag)
        {
          if (this.isAlternate)
          {
            for (int index = 0; index < this.unfl0001Options.Options.Count; ++index)
              editorControl.Items.Add((object) this.unfl0001Options.Options[index].Text);
          }
          else
            editorControl.Items.Add((object) "Other");
          editorControl.Text = e.SubItem.Text;
          this.lastUCDType = editorControl.Text;
          editorControl.SelectedIndexChanged += new EventHandler(this.ComboBox_SelectedIndexChanged);
        }
        else
        {
          for (int index = 0; index < this.fl0062Options.Options.Count; ++index)
            editorControl.Items.Add((object) this.fl0062Options.Options[index].Text);
          editorControl.Text = e.SubItem.Text;
        }
      }
      else
        e.Cancel = true;
    }

    private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listViewLiabs.SelectedItems.Count == 0 || this.lastUCDType == ((Control) sender).Text || this.listViewLiabs.SelectedItems[0].Tag == null || !this.listViewLiabs.SelectedItems[0].Tag.ToString().StartsWith("UNFL"))
        return;
      string[] strArray = this.listViewLiabs.SelectedItems[0].Tag.ToString().Split('|');
      this.listViewLiabs.SelectedItems[0].Tag = (object) (strArray[0] + "||");
      double num = Utils.ToDouble(this.listViewLiabs.SelectedItems[0].SubItems[6].Text);
      if (((Control) sender).Text == "Other")
      {
        this.listViewLiabs.SelectedItems[0].SubItems[4].Text = "";
        this.listViewLiabs.SelectedItems[0].SubItems[6].Text = Math.Abs(num).ToString("N2");
      }
      else
      {
        this.listViewLiabs.SelectedItems[0].SubItems[4].Text = "";
        if (strArray[2] == "PrincipalReduction")
        {
          double amount = this.parseAmount(strArray[1]);
          num = amount != 0.0 ? amount : num;
        }
        this.listViewLiabs.SelectedItems[0].SubItems[6].Text = (num < 0.0 ? num : num * -1.0).ToString("N2");
      }
      this.calculateTotal();
    }

    private double parseAmount(string description)
    {
      int num1 = description.IndexOf("$");
      int num2 = description.IndexOf("P.O.C.");
      double amount = 0.0;
      if (num2 > -1 && num1 > -1)
        amount = Utils.ToDouble(description.Substring(num1 + 1, num2 - num1 - 1));
      return amount;
    }

    private string mapUCDPayoffTypeValue(string value, bool mapToUi)
    {
      if (this.fl0062Options == null || this.fl0062Options.Options == null)
        return value;
      for (int index = 0; index < this.fl0062Options.Options.Count; ++index)
      {
        if (mapToUi)
        {
          if (value == this.fl0062Options.Options[index].Value)
            return this.fl0062Options.Options[index].Text;
        }
        else if (value == this.fl0062Options.Options[index].Text)
          return this.fl0062Options.Options[index].Value;
      }
      return value;
    }

    private string mapNonUCDPayoffTypeValue(string value, bool mapToUi)
    {
      if (this.unfl0001Options == null || this.unfl0001Options.Options == null)
        return value;
      for (int index = 0; index < this.unfl0001Options.Options.Count; ++index)
      {
        if (mapToUi)
        {
          if (value == this.unfl0001Options.Options[index].Value)
            return this.unfl0001Options.Options[index].Text;
        }
        else if (value == this.unfl0001Options.Options[index].Text)
          return this.unfl0001Options.Options[index].Value;
      }
      return value;
    }

    private void calcPaymentsAndPayoffs(NonVOLEntryDialog paymentsAndPayoffs)
    {
      if (paymentsAndPayoffs.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      string str1 = "UNFL" + this.totalnonvol.ToString("00");
      ++this.totalnonvol;
      GVItem gvItem = new GVItem(this.totalnonvol.ToString());
      string str2 = !(paymentsAndPayoffs.adjustmentType == "Other") ? paymentsAndPayoffs.adjustmentDescription : (paymentsAndPayoffs.adjustmentOtherDescription == "PrincipalReduction" || !this.isAlternate ? paymentsAndPayoffs.adjustmentDescription : paymentsAndPayoffs.adjustmentOtherDescription);
      gvItem.SubItems.Add((object) "");
      gvItem.SubItems.Add((object) "");
      gvItem.SubItems.Add((object) "");
      gvItem.SubItems.Add((object) str2);
      gvItem.SubItems.Add((object) "");
      if (paymentsAndPayoffs.adjustmentAmount == 0M && paymentsAndPayoffs.adjustmentOtherDescription == "PrincipalReduction")
        gvItem.SubItems.Add((object) string.Empty);
      else
        gvItem.SubItems.Add((object) paymentsAndPayoffs.adjustmentAmount.ToString("N2"));
      gvItem.SubItems.Add((object) this.mapNonUCDPayoffTypeValue(paymentsAndPayoffs.adjustmentType, true));
      gvItem.SubItems[0].CheckBoxEnabled = false;
      gvItem.SubItems[1].Checked = true;
      gvItem.SubItems.Add((object) paymentsAndPayoffs.pocIndicator);
      gvItem.SubItems.Add((object) paymentsAndPayoffs.paidBy);
      gvItem.SubItems.Add((object) paymentsAndPayoffs.principalCureAddendum);
      gvItem.Tag = (object) (str1 + "|" + paymentsAndPayoffs.adjustmentDescription + "|" + paymentsAndPayoffs.adjustmentOtherDescription);
      this.listViewLiabs.Items.Add(gvItem);
      this.calculateTotal();
    }

    private void btnAddNonVOL_Click(object sender, EventArgs e)
    {
      using (NonVOLEntryDialog paymentsAndPayoffs = new NonVOLEntryDialog(isAlternate: this.isAlternate))
        this.calcPaymentsAndPayoffs(paymentsAndPayoffs);
    }

    private void calculateTotal()
    {
      double num1 = 0.0;
      double num2 = Utils.ParseDouble((object) this.txtTotal.Text);
      double num3 = Utils.ParseDouble((object) this.txtCashToClose.Text);
      for (int nItemIndex = 0; nItemIndex < this.listViewLiabs.Items.Count; ++nItemIndex)
      {
        if (this.listViewLiabs.Items[nItemIndex].SubItems[1].Checked)
          num1 += Utils.ParseDouble((object) this.listViewLiabs.Items[nItemIndex].SubItems[6].Text.Replace(",", ""));
      }
      this.txtTotal.Text = this.txtTotalPayoffsK.Text = num1.ToString("N2");
      double num4 = num3 + num2 - num1;
      this.txtCashToClose.Text = num4.ToString("N2");
      this.txtToFromBorrower.Text = num4 <= 0.0 ? "From Borrower" : "To Borrower";
    }

    private void listViewLiabs_DoubleClick(object sender, EventArgs e)
    {
      if (this.listViewLiabs.SelectedItems.Count == 0 || this.listViewLiabs.SelectedItems[0].Tag == null || !this.listViewLiabs.SelectedItems[0].Tag.ToString().StartsWith("UNFL"))
        return;
      int fieldId = int.Parse(this.listViewLiabs.SelectedItems[0].Tag.ToString().Substring(4, 2));
      string[] strArray = this.listViewLiabs.SelectedItems[0].Tag.ToString().Split('|');
      Decimal adjAmount = Utils.ParseDecimal((object) this.listViewLiabs.SelectedItems[0].SubItems[6].Text, 0M);
      using (NonVOLEntryDialog nonVolEntryDialog = new NonVOLEntryDialog(fieldId, this.listViewLiabs.SelectedItems[0].SubItems[7].Text, strArray[1], strArray[2], adjAmount, this.isAlternate, Utils.ParseBoolean((object) this.listViewLiabs.SelectedItems[0].SubItems[8].Text), this.listViewLiabs.SelectedItems[0].SubItems[9].Text, this.listViewLiabs.SelectedItems[0].SubItems[10].Text))
      {
        if (nonVolEntryDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.listViewLiabs.SelectedItems[0].SubItems[4].Text = !(nonVolEntryDialog.adjustmentType == "Other") ? nonVolEntryDialog.adjustmentDescription : (nonVolEntryDialog.adjustmentOtherDescription == "PrincipalReduction" || nonVolEntryDialog.adjustmentOtherDescription == "PrincipalReductionCure" || !this.isAlternate && nonVolEntryDialog.adjustmentOtherDescription == "StandardOther" ? nonVolEntryDialog.adjustmentDescription : nonVolEntryDialog.adjustmentOtherDescription);
        this.listViewLiabs.SelectedItems[0].SubItems[6].Text = !(nonVolEntryDialog.adjustmentAmount == 0M) || !(nonVolEntryDialog.adjustmentOtherDescription == "PrincipalReduction") ? nonVolEntryDialog.adjustmentAmount.ToString("N2") : string.Empty;
        this.listViewLiabs.SelectedItems[0].SubItems[7].Text = this.mapNonUCDPayoffTypeValue(nonVolEntryDialog.adjustmentType, true);
        this.listViewLiabs.SelectedItems[0].Tag = (object) (strArray[0] + "|" + nonVolEntryDialog.adjustmentDescription + "|" + nonVolEntryDialog.adjustmentOtherDescription);
        this.listViewLiabs.SelectedItems[0].SubItems[8].Text = nonVolEntryDialog.pocIndicator.ToString();
        this.listViewLiabs.SelectedItems[0].SubItems[9].Text = nonVolEntryDialog.paidBy;
        this.listViewLiabs.SelectedItems[0].SubItems[10].Text = nonVolEntryDialog.principalCureAddendum;
        this.calculateTotal();
      }
    }

    private void btnAddPrincipalReduction_Click(object sender, EventArgs e)
    {
      using (NonVOLEntryDialog paymentsAndPayoffs = new NonVOLEntryDialog(isAlternate: this.isAlternate))
        this.calcPaymentsAndPayoffs(paymentsAndPayoffs);
    }

    private void deleteButton_Click(object sender, EventArgs e)
    {
      if (DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "Permanently delete VOL/Non VOL adjustment?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) || this.listViewLiabs.SelectedItems == null)
        return;
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      foreach (GVItem selectedItem in this.listViewLiabs.SelectedItems)
      {
        string[] strArray = selectedItem.Tag.ToString().Split('|');
        if (strArray.Length < 3 || !strArray[2].Equals("PrincipalReductionCure"))
        {
          if (strArray[0].StartsWith("UNFL"))
            this.loan.RemoveNonVolAt(Utils.ParseInt((object) strArray[0].Substring(strArray[0].Length - 2)) - 1);
          else if (Utils.ParseInt((object) selectedItem.SubItems[2].Text) >= 1)
            this.RemoveVol(selectedItem);
          this.listViewLiabs.Items.Remove(selectedItem);
        }
      }
      this.initForm();
      this.calculateTotal();
      if (!(currentBorrowerPair.Id != this.loan.CurrentBorrowerPair.Id))
        return;
      this.loan.SetBorrowerPair(currentBorrowerPair);
    }

    private void RemoveVol(GVItem item)
    {
      this.loan.SetBorrowerPair(this.loan.GetBorrowerPairs()[Utils.ParseInt((object) item.SubItems[2].Text) - 1]);
      this.loan.RemoveLiabilityAt(Utils.ParseInt((object) item.SubItems[8].Text) - 1);
    }

    private void listViewLiabs_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.deleteButton.Enabled = false;
      foreach (GVItem selectedItem in this.listViewLiabs.SelectedItems)
      {
        string[] strArray = selectedItem.Tag.ToString().Split('|');
        if (strArray.Length < 3 || !strArray[2].Equals("PrincipalReductionCure"))
        {
          this.deleteButton.Enabled = true;
          break;
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      this.tableContainer1 = new TableContainer();
      this.deleteButton = new StandardIconButton();
      this.listViewLiabs = new GridView();
      this.txtTotal = new TextBox();
      this.label11 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.label4 = new Label();
      this.label1 = new Label();
      this.txtToFromBorrower = new TextBox();
      this.txtLoanAmount = new TextBox();
      this.txtTotalCostJ = new TextBox();
      this.txtClosingCostBeforeClosing = new TextBox();
      this.txtTotalPayoffsK = new TextBox();
      this.txtCashToClose = new TextBox();
      this.label9 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label7 = new Label();
      this.label5 = new Label();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.btnAdd = new StandardIconButton();
      this.tableContainer1.SuspendLayout();
      ((ISupportInitialize) this.deleteButton).BeginInit();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.SuspendLayout();
      this.tableContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tableContainer1.Controls.Add((Control) this.btnAdd);
      this.tableContainer1.Controls.Add((Control) this.deleteButton);
      this.tableContainer1.Controls.Add((Control) this.listViewLiabs);
      this.tableContainer1.Controls.Add((Control) this.txtTotal);
      this.tableContainer1.Controls.Add((Control) this.label11);
      this.tableContainer1.Location = new Point(5, 2);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(913, 292);
      this.tableContainer1.TabIndex = 70;
      this.tableContainer1.Text = "Disbursement to Others";
      this.deleteButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.deleteButton.BackColor = Color.Transparent;
      this.deleteButton.Enabled = false;
      this.deleteButton.Location = new Point(890, 4);
      this.deleteButton.MouseDownImage = (Image) null;
      this.deleteButton.Name = "deleteButton";
      this.deleteButton.Size = new Size(16, 16);
      this.deleteButton.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.deleteButton.TabIndex = 34;
      this.deleteButton.TabStop = false;
      this.deleteButton.Click += new EventHandler(this.deleteButton_Click);
      this.listViewLiabs.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Paid Off";
      gvColumn1.Width = 57;
      gvColumn2.CheckBoxes = true;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnInclude";
      gvColumn2.Text = "Include";
      gvColumn2.Width = 57;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnPair";
      gvColumn3.Text = "Pair";
      gvColumn3.Width = 30;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.Text = "Creditor Name";
      gvColumn4.Width = 200;
      gvColumn5.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Description of Purpose";
      gvColumn5.Width = 200;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column3";
      gvColumn6.Text = "Balance";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 86;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column4";
      gvColumn7.SortMethod = GVSortMethod.Numeric;
      gvColumn7.Text = "Payoff Amount";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 85;
      gvColumn8.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column6";
      gvColumn8.SpringToFit = true;
      gvColumn8.Text = "UCD Type";
      gvColumn8.Width = 196;
      this.listViewLiabs.Columns.AddRange(new GVColumn[8]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.listViewLiabs.Dock = DockStyle.Fill;
      this.listViewLiabs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewLiabs.Location = new Point(1, 26);
      this.listViewLiabs.Name = "listViewLiabs";
      this.listViewLiabs.Size = new Size(911, 240);
      this.listViewLiabs.SortOption = GVSortOption.None;
      this.listViewLiabs.TabIndex = 0;
      this.listViewLiabs.SelectedIndexChanged += new EventHandler(this.listViewLiabs_SelectedIndexChanged);
      this.listViewLiabs.SubItemCheck += new GVSubItemEventHandler(this.listViewLiabs_SubItemCheck);
      this.listViewLiabs.EditorOpening += new GVSubItemEditingEventHandler(this.listViewLiabs_EditorOpening);
      this.listViewLiabs.DoubleClick += new EventHandler(this.listViewLiabs_DoubleClick);
      this.txtTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtTotal.BackColor = Color.WhiteSmoke;
      this.txtTotal.Location = new Point(522, 269);
      this.txtTotal.Name = "txtTotal";
      this.txtTotal.ReadOnly = true;
      this.txtTotal.Size = new Size(108, 20);
      this.txtTotal.TabIndex = 2;
      this.txtTotal.TabStop = false;
      this.txtTotal.Tag = (object) "HUD1A.X31";
      this.txtTotal.TextAlign = HorizontalAlignment.Right;
      this.label11.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label11.AutoSize = true;
      this.label11.BackColor = Color.Transparent;
      this.label11.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(353, 271);
      this.label11.Name = "label11";
      this.label11.Size = new Size(168, 14);
      this.label11.TabIndex = 31;
      this.label11.Text = "Total Payoffs and Payments       $";
      this.groupContainer1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.txtToFromBorrower);
      this.groupContainer1.Controls.Add((Control) this.txtLoanAmount);
      this.groupContainer1.Controls.Add((Control) this.txtTotalCostJ);
      this.groupContainer1.Controls.Add((Control) this.txtClosingCostBeforeClosing);
      this.groupContainer1.Controls.Add((Control) this.txtTotalPayoffsK);
      this.groupContainer1.Controls.Add((Control) this.txtCashToClose);
      this.groupContainer1.Controls.Add((Control) this.label9);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(5, 300);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(913, 158);
      this.groupContainer1.TabIndex = 71;
      this.groupContainer1.Text = "Calculating Cash to Close";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(506, 105);
      this.label4.Name = "label4";
      this.label4.Size = new Size(14, 20);
      this.label4.TabIndex = 23;
      this.label4.Text = "-";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(506, 56);
      this.label1.Name = "label1";
      this.label1.Size = new Size(14, 20);
      this.label1.TabIndex = 22;
      this.label1.Text = "-";
      this.txtToFromBorrower.BackColor = Color.WhiteSmoke;
      this.txtToFromBorrower.Location = new Point(344, 129);
      this.txtToFromBorrower.Name = "txtToFromBorrower";
      this.txtToFromBorrower.ReadOnly = true;
      this.txtToFromBorrower.Size = new Size(134, 20);
      this.txtToFromBorrower.TabIndex = 21;
      this.txtToFromBorrower.TabStop = false;
      this.txtToFromBorrower.Tag = (object) "HUD1A.X32";
      this.txtLoanAmount.BackColor = Color.WhiteSmoke;
      this.txtLoanAmount.Location = new Point(523, 33);
      this.txtLoanAmount.Name = "txtLoanAmount";
      this.txtLoanAmount.ReadOnly = true;
      this.txtLoanAmount.Size = new Size(108, 20);
      this.txtLoanAmount.TabIndex = 3;
      this.txtLoanAmount.TabStop = false;
      this.txtLoanAmount.Tag = (object) "2";
      this.txtLoanAmount.TextAlign = HorizontalAlignment.Right;
      this.txtTotalCostJ.Location = new Point(523, 57);
      this.txtTotalCostJ.Name = "txtTotalCostJ";
      this.txtTotalCostJ.ReadOnly = true;
      this.txtTotalCostJ.Size = new Size(108, 20);
      this.txtTotalCostJ.TabIndex = 4;
      this.txtTotalCostJ.Tag = (object) "HUD1A.X33";
      this.txtTotalCostJ.TextAlign = HorizontalAlignment.Right;
      this.txtClosingCostBeforeClosing.BackColor = Color.WhiteSmoke;
      this.txtClosingCostBeforeClosing.Location = new Point(523, 81);
      this.txtClosingCostBeforeClosing.Name = "txtClosingCostBeforeClosing";
      this.txtClosingCostBeforeClosing.ReadOnly = true;
      this.txtClosingCostBeforeClosing.Size = new Size(108, 20);
      this.txtClosingCostBeforeClosing.TabIndex = 5;
      this.txtClosingCostBeforeClosing.TabStop = false;
      this.txtClosingCostBeforeClosing.Tag = (object) "L351";
      this.txtClosingCostBeforeClosing.TextAlign = HorizontalAlignment.Right;
      this.txtTotalPayoffsK.BackColor = Color.WhiteSmoke;
      this.txtTotalPayoffsK.Location = new Point(523, 105);
      this.txtTotalPayoffsK.Name = "txtTotalPayoffsK";
      this.txtTotalPayoffsK.ReadOnly = true;
      this.txtTotalPayoffsK.Size = new Size(108, 20);
      this.txtTotalPayoffsK.TabIndex = 6;
      this.txtTotalPayoffsK.TabStop = false;
      this.txtTotalPayoffsK.Tag = (object) "HUD1A.X31";
      this.txtTotalPayoffsK.TextAlign = HorizontalAlignment.Right;
      this.txtCashToClose.BackColor = Color.WhiteSmoke;
      this.txtCashToClose.Location = new Point(523, 129);
      this.txtCashToClose.Name = "txtCashToClose";
      this.txtCashToClose.ReadOnly = true;
      this.txtCashToClose.Size = new Size(108, 20);
      this.txtCashToClose.TabIndex = 7;
      this.txtCashToClose.TabStop = false;
      this.txtCashToClose.Tag = (object) "HUD1A.X32";
      this.txtCashToClose.TextAlign = HorizontalAlignment.Right;
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.Transparent;
      this.label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(214, 132);
      this.label9.Name = "label9";
      this.label9.Size = new Size(85, 13);
      this.label9.TabIndex = 20;
      this.label9.Text = "Cash to Close";
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(214, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(70, 13);
      this.label2.TabIndex = 12;
      this.label2.Text = "Loan Amount";
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(214, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(111, 13);
      this.label3.TabIndex = 14;
      this.label3.Text = "Total Closing Costs (J)";
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Location = new Point(214, 108);
      this.label7.Name = "label7";
      this.label7.Size = new Size(155, 13);
      this.label7.TabIndex = 18;
      this.label7.Text = "Total Payoffs and Payments (K)";
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Location = new Point(214, 84);
      this.label5.Name = "label5";
      this.label5.Size = new Size(165, 13);
      this.label5.TabIndex = 16;
      this.label5.Text = "Closing Costs Paid Before Closing";
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.BackColor = SystemColors.Control;
      this.okBtn.Location = new Point(758, 464);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 72;
      this.okBtn.Text = "&OK";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(842, 464);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 73;
      this.cancelBtn.Text = "&Cancel";
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(868, 4);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 35;
      this.btnAdd.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(923, 498);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.tableContainer1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PayoffsAndPaymentsDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Payoffs and Payments";
      this.Load += new EventHandler(this.PayoffsAndPaymentsDialog_Load);
      this.tableContainer1.ResumeLayout(false);
      this.tableContainer1.PerformLayout();
      ((ISupportInitialize) this.deleteButton).EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.ResumeLayout(false);
    }
  }
}
