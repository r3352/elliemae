// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SwapBorrowerPairForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SwapBorrowerPairForm : Form, IOnlineHelpTarget
  {
    private const string className = "CoMortgagerWS";
    private LoanData loan;
    private BorrowerPair[] pairs;
    private BorrowerPair currentPair;
    private string[] piggyBackFieldSync;
    private PopupBusinessRules popupRules;
    private int targetBorrowerPairNo = -1;
    private bool importedFromCoborrower;
    private bool targetCoBorrower;
    private bool isFormClosed;
    private static string[] importBorrowerFields = new string[120]
    {
      "4000",
      "4001",
      "4002",
      "4003",
      "65",
      "1402",
      "66",
      "1240",
      "1178",
      "1490",
      "52",
      "169",
      "265",
      "170",
      "172",
      "1057",
      "463",
      "173",
      "174",
      "171",
      "965",
      "466",
      "418",
      "403",
      "981",
      "1069",
      "188",
      "1523",
      "1524",
      "1525",
      "1526",
      "1527",
      "1528",
      "1529",
      "1530",
      "471",
      "101",
      "102",
      "103",
      "104",
      "105",
      "106",
      "107",
      "108",
      "910",
      "67",
      "1450",
      "1414",
      "1484",
      "1416",
      "1417",
      "1418",
      "1419",
      "URLA.X1",
      "URLA.X263",
      "URLA.X111",
      "URLA.X113",
      "URLA.X115",
      "URLA.X117",
      "53",
      "54",
      "66",
      "4533",
      "1819",
      "URLA.X267",
      "URLA.X197",
      "URLA.X7",
      "URLA.X8",
      "URLA.X269",
      "URLA.X13",
      "URLA.X123",
      "URLA.X124",
      "URLA.X125",
      "URLA.X19",
      "URLA.X17",
      "URLA.X21",
      "URLA.X35",
      "URLA.X195",
      "URLA.X199",
      "1145",
      "1815",
      "1169",
      "1758",
      "URLA.X84",
      "URLA.X86",
      "URLA.X88",
      "URLA.X90",
      "URLA.X92",
      "URLA.X94",
      "URLA.X96",
      "URLA.X98",
      "URLA.X100",
      "URLA.X102",
      "URLA.X104",
      "URLA.X106",
      "URLA.X174",
      "URLA.X175",
      "URLA.X176",
      "URLA.X177",
      "URLA.X213",
      "URLA.X153",
      "URLA.X154",
      "URLA.X155",
      "URLA.X232",
      "URLA.X233",
      "4008",
      "3249",
      "3250",
      "3860",
      "4715",
      "3861",
      "3862",
      "3864",
      "3863",
      "4717",
      "3865",
      "4719",
      "4073",
      "4074",
      "4075"
    };
    private static string[] importHMDABorrowerFields = new string[35]
    {
      "4210",
      "4211",
      "4212",
      "4205",
      "4243",
      "4125",
      "4144",
      "4145",
      "4146",
      "4147",
      "4126",
      "4128",
      "4130",
      "4252",
      "4244",
      "4148",
      "4149",
      "4150",
      "4151",
      "4152",
      "4153",
      "4154",
      "4155",
      "4156",
      "4157",
      "4158",
      "4193",
      "4194",
      "4195",
      "4196",
      "4245",
      "4143",
      "4121",
      "4122",
      "4123"
    };
    private static string[] importCoBorrowerFields = new string[120]
    {
      "4004",
      "4005",
      "4006",
      "4007",
      "97",
      "1403",
      "98",
      "1268",
      "1179",
      "1480",
      "84",
      "175",
      "266",
      "176",
      "178",
      "1197",
      "464",
      "179",
      "180",
      "177",
      "985",
      "467",
      "1343",
      "1108",
      "1015",
      "1070",
      "189",
      "1531",
      "1532",
      "1533",
      "1534",
      "1535",
      "1536",
      "1537",
      "1538",
      "478",
      "110",
      "111",
      "112",
      "113",
      "114",
      "115",
      "116",
      "117",
      "911",
      "60",
      "1452",
      "1415",
      "1502",
      "1519",
      "1520",
      "1521",
      "1522",
      "URLA.X2",
      "URLA.X264",
      "URLA.X112",
      "URLA.X114",
      "URLA.X116",
      "URLA.X118",
      "85",
      "86",
      "98",
      "4534",
      "1820",
      "URLA.X268",
      "URLA.X198",
      "URLA.X9",
      "URLA.X10",
      "URLA.X270",
      "URLA.X14",
      "URLA.X126",
      "URLA.X127",
      "URLA.X128",
      "URLA.X20",
      "URLA.X18",
      "URLA.X22",
      "URLA.X36",
      "URLA.X196",
      "URLA.X200",
      "1146",
      "1816",
      "1170",
      "1759",
      "URLA.X85",
      "URLA.X87",
      "URLA.X89",
      "URLA.X91",
      "URLA.X93",
      "URLA.X95",
      "URLA.X97",
      "URLA.X99",
      "URLA.X101",
      "URLA.X103",
      "URLA.X105",
      "URLA.X107",
      "URLA.X178",
      "URLA.X179",
      "URLA.X180",
      "URLA.X181",
      "URLA.X214",
      "URLA.X159",
      "URLA.X160",
      "URLA.X161",
      "URLA.X244",
      "URLA.X243",
      "4009",
      "3251",
      "3252",
      "3866",
      "4716",
      "3867",
      "3868",
      "3870",
      "3869",
      "4718",
      "3871",
      "4720",
      "4076",
      "4077",
      "4078"
    };
    private static string[] importHMDACoBorrowerFields = new string[35]
    {
      "4213",
      "4214",
      "4215",
      "4206",
      "4246",
      "4136",
      "4159",
      "4160",
      "4161",
      "4162",
      "4137",
      "4139",
      "4141",
      "4253",
      "4247",
      "4163",
      "4164",
      "4165",
      "4166",
      "4167",
      "4168",
      "4169",
      "4170",
      "4171",
      "4172",
      "4173",
      "4197",
      "4198",
      "4199",
      "4200",
      "4248",
      "4131",
      "4132",
      "4133",
      "4134"
    };
    private static string[] vomFields = new string[41]
    {
      "46",
      "36",
      "98",
      "37",
      "44",
      "45",
      "38",
      "04",
      "06",
      "07",
      "08",
      "28",
      "41",
      "19",
      "17",
      "16",
      "24",
      "18",
      "23",
      "22",
      "20",
      "21",
      "26",
      "14",
      "32",
      "25",
      "64",
      "47",
      "48",
      "49",
      "50",
      "51",
      "52",
      "53",
      "54",
      "55",
      "56",
      "57",
      "58",
      "59",
      "98"
    };
    private IContainer components;
    private Button btnOK;
    private Button btnDeletePair;
    private Label label4;
    private TextBox txtCobSSN;
    private TextBox txtCobLast;
    private Label label6;
    private TextBox txtCobFirst;
    private Label label3;
    private TextBox txtBorSSN;
    private Label label2;
    private TextBox txtBorLast;
    private Label label1;
    private TextBox txtBorFirst;
    private Label label9;
    private Label label8;
    private TextBox txtBorCredit1;
    private TextBox txtBorCredit3;
    private TextBox txtBorCredit2;
    private Label label7;
    private Label label10;
    private Label label11;
    private TextBox txtCobCredit1;
    private TextBox txtCobCredit3;
    private TextBox txtCobCredit2;
    private Label label12;
    private TextBox txtBorMiddle;
    private Label label18;
    private Label label19;
    private TextBox txtCobSuffix;
    private TextBox txtCobMiddle;
    private Label label16;
    private Label label15;
    private Label label14;
    private TextBox txtBorSuffix;
    private GroupContainer groupContainer1;
    private GroupContainer groupContainer2;
    private GroupContainer groupContainer3;
    private PictureBox pboxDownArrow;
    private PictureBox pboxAsterisk;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnAdd;
    private GridView gridViewPairs;
    private ToolTip toolTip1;
    private EMHelpLink emHelpLink1;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnMoveDown;
    private StandardIconButton btnMoveUp;
    private Button btnDeleteCoborrower;
    private Button btnMoveBorrower;
    private Button btnImport;
    private Button btnMoveCoborrower;
    private Label label5;
    private CheckBox chkBorrSelfEmployed;
    private CheckBox chkCoBorrSelfEmployed;
    private Label label13;
    private Label label17;
    private Label label20;
    private ComboBox cboBorrowerType;
    private ComboBox cboCoborrowerType;

    public event EventHandler ImportFromLoanClicked;

    public SwapBorrowerPairForm(LoanData loan)
    {
      this.loan = loan;
      this.pairs = this.loan.GetBorrowerPairs();
      foreach (BorrowerPair pair in this.pairs)
      {
        if (pair.Id == this.loan.CurrentBorrowerPair.Id)
          this.currentPair = pair;
      }
      this.InitializeComponent();
      this.initForm();
      this.applyPersonaAccess();
    }

    private void applyPersonaAccess()
    {
      if (!((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanTab_Other_DeleteBorrowers))
        this.btnDeletePair.Visible = this.btnDeleteCoborrower.Visible = false;
      Hashtable hashtable = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).CheckPermissions(FeatureSets.LoanOtherFeatures, Session.UserInfo);
      if (!(bool) hashtable[(object) AclFeature.LoanTab_Other_MoveBorrowers])
      {
        this.btnMoveBorrower.Visible = this.btnMoveCoborrower.Visible = false;
        this.btnMoveUp.Visible = this.btnMoveDown.Visible = false;
      }
      if (!(bool) hashtable[(object) AclFeature.LoanTab_Other_ImportBorrowers])
        this.btnImport.Visible = false;
      if (!(bool) hashtable[(object) AclFeature.LoanTab_Other_ManageBorrowers])
      {
        this.btnImport.Visible = this.btnMoveBorrower.Visible = this.btnMoveCoborrower.Visible = this.btnDeleteCoborrower.Visible = false;
        this.btnDeletePair.Visible = this.btnMoveDown.Visible = this.btnMoveUp.Visible = false;
        this.verticalSeparator1.Visible = false;
      }
      if (!this.btnDeletePair.Visible && this.btnMoveUp.Visible || !this.btnDeletePair.Visible && !this.btnMoveUp.Visible)
        this.verticalSeparator1.Visible = false;
      if (this.btnDeleteCoborrower.Visible || !this.btnMoveCoborrower.Visible)
        return;
      this.btnMoveCoborrower.Left = this.btnMoveBorrower.Left;
    }

    private void setBusinessRule(Control.ControlCollection cs)
    {
      if (this.popupRules == null)
      {
        ResourceManager resources = new ResourceManager(typeof (SwapBorrowerPairForm));
        this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
      }
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
            TextBox ctrl1 = (TextBox) c;
            if (ctrl1 != null && ctrl1.Tag != null)
            {
              string fieldID = ctrl1.Tag.ToString();
              if (!(fieldID == string.Empty))
              {
                this.popupRules.SetBusinessRules((object) ctrl1, fieldID);
                if (ctrl1.ReadOnly)
                {
                  ctrl1.BackColor = SystemColors.Control;
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          case ComboBox _:
            ComboBox ctrl2 = (ComboBox) c;
            if (ctrl2 != null && ctrl2.Tag != null)
            {
              string fieldID = ctrl2.Tag.ToString();
              if (!(fieldID == string.Empty))
              {
                this.popupRules.SetBusinessRules((object) ctrl2, fieldID);
                continue;
              }
              continue;
            }
            continue;
          default:
            this.setBusinessRule(c.Controls);
            continue;
        }
      }
    }

    private void loadFields(Control.ControlCollection cs)
    {
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
                textBox.Text = this.loan.GetField(id);
                continue;
              }
              continue;
            }
            continue;
          case CheckBox _:
            CheckBox checkBox = (CheckBox) c;
            if (checkBox != null && checkBox.Tag != null)
            {
              string id = checkBox.Tag.ToString();
              if (!(id == string.Empty))
              {
                checkBox.Checked = this.loan.GetField(id).ToUpper() == "Y";
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
                comboBox.Text = this.loan.GetField(id);
                continue;
              }
              continue;
            }
            continue;
          default:
            this.loadFields(c.Controls);
            continue;
        }
      }
    }

    private void SwapBorrowerPairForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      bool flag = true;
      this.pairs = this.loan.GetBorrowerPairs();
      foreach (BorrowerPair pair in this.pairs)
      {
        if (pair.Id == this.currentPair.Id)
        {
          flag = false;
          break;
        }
      }
      if (!flag && this.currentPair != null)
        this.loan.SetBorrowerPair(this.currentPair);
      else
        this.loan.SetBorrowerPair(this.pairs[0]);
      Session.Application.GetService<ILoanEditor>().RefreshContents();
      this.isFormClosed = true;
    }

    private void initForm()
    {
      this.pairs = this.loan.GetBorrowerPairs();
      this.gridViewPairs.Items.Clear();
      this.gridViewPairs.BeginUpdate();
      int pairNo = 1;
      foreach (BorrowerPair pair in this.pairs)
      {
        this.gridViewPairs.Items.Add(this.buildGridViewItem(pair, pairNo));
        ++pairNo;
      }
      this.gridViewPairs.EndUpdate();
      this.gridViewPairs.Items[0].Selected = true;
      this.setBusinessRule(this.Controls);
    }

    private GVItem buildGridViewItem(BorrowerPair p, int pairNo)
    {
      GVItem gvItem = new GVItem(string.Concat((object) pairNo));
      gvItem.SubItems.Add((object) (p.Borrower.LastName + ", " + p.Borrower.FirstName));
      if (p.CoBorrower.LastName != "" || p.CoBorrower.FirstName != "")
        gvItem.SubItems.Add((object) (p.CoBorrower.LastName + ", " + p.CoBorrower.FirstName));
      gvItem.Tag = (object) pairNo.ToString();
      return gvItem;
    }

    private void setCurrentPair(int pairIndex)
    {
      if (pairIndex < 1 || pairIndex > this.gridViewPairs.Items.Count)
        return;
      this.gridViewPairs.Items[pairIndex - 1].Selected = true;
    }

    private void btnMoveUp_Click(object sender, EventArgs e)
    {
      if (this.gridViewPairs.SelectedItems.Count != 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select a borrower pair to be moved.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int pair2 = Utils.ParseInt(this.gridViewPairs.SelectedItems[0].Tag);
        if (pair2 <= 1)
          return;
        this.swapBorrowerPairs(pair2 - 1, pair2);
        this.setCurrentPair(pair2 - 1);
      }
    }

    private void btnMoveDown_Click(object sender, EventArgs e)
    {
      if (this.gridViewPairs.SelectedItems.Count != 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select a borrower pair to be moved.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int pair1 = Utils.ParseInt(this.gridViewPairs.SelectedItems[0].Tag);
        if (pair1 >= this.pairs.Length)
          return;
        this.swapBorrowerPairs(pair1, pair1 + 1);
        this.setCurrentPair(pair1 + 1);
      }
    }

    private void swapBorrowerPairs(int pair1, int pair2)
    {
      using (CursorActivator.Wait())
      {
        if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
          return;
        try
        {
          this.loan.SwapBorrowerPairs(new BorrowerPair[2]
          {
            this.pairs[pair1 - 1],
            this.pairs[pair2 - 1]
          });
          this.loan.TriggerCalculation("36", this.loan.GetField("36"));
          this.loan.Calculator.FormCalculation("CALCAUTOMATEDDISCLOSURES");
          this.loan.BusinessRuleTrigger |= BusinessRuleOnDemandEnum.FieldAccessRule;
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Borrower Pairs cannot be swapped due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        this.initForm();
      }
    }

    private void btnSwapCoborrower_Click(object sender, EventArgs e)
    {
      this.moveBorrowerConfirmation(false);
    }

    private void moveBorrowerConfirmation(bool forCoborrower)
    {
      int num = Utils.ParseInt((object) (string) this.gridViewPairs.SelectedItems[0].Tag);
      int targetPair = -1;
      bool targetForCoborrower = false;
      using (MoveBorrowerForm moveBorrowerForm = new MoveBorrowerForm(this.loan, num, forCoborrower))
      {
        if (moveBorrowerForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        targetPair = moveBorrowerForm.TargetBorrowerPairNo;
        targetForCoborrower = moveBorrowerForm.TargetCoBorrower;
      }
      string text = "Swap the borrower and co-borrower information for this borrower pair?";
      if (targetPair != this.gridViewPairs.SelectedItems[0].Index)
        text = "Are you sure you want to swap selected borrower information to another borrower?";
      if (Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.swapBorrowerAndCoborrower(num, forCoborrower, targetPair, targetForCoborrower);
    }

    private void swapBorrowerAndCoborrower(
      int sourcePair,
      bool sourceForCoborrower,
      int targetPair,
      bool targetForCoborrower)
    {
      using (CursorActivator.Wait())
      {
        if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
          return;
        try
        {
          if (sourcePair == targetPair && sourceForCoborrower != targetForCoborrower)
            this.loan.SwapBorrowers(new BorrowerPair[1]
            {
              this.pairs[sourcePair - 1]
            });
          else
            this.loan.SwapBorrowers(sourcePair, sourceForCoborrower, targetPair, targetForCoborrower);
          this.loan.TriggerCalculation("4000", this.loan.GetField("4000"));
          if (this.loan.Calculator != null)
            this.loan.Calculator.FormCalculation("CALCAUTOMATEDDISCLOSURES");
          this.loan.BusinessRuleTrigger |= BusinessRuleOnDemandEnum.FieldAccessRule;
          ILoanEditor service = Session.Application.GetService<ILoanEditor>();
          service.ApplyOnDemandBusinessRules();
          service.RefreshContents();
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Borrowers cannot be swapped due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        this.initForm();
        this.setCurrentPair(targetPair);
      }
      string text = "The borrower and co-borrower have been swapped successfully.";
      if (sourcePair != targetPair)
        text = "The borrower has been moved to another borrower pair successfully.";
      int num1 = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void gridViewPairs_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gridViewPairs.SelectedItems.Count;
      this.btnMoveUp.Enabled = count == 1 && this.gridViewPairs.SelectedItems[0].Index != 0;
      this.btnMoveDown.Enabled = count == 1 && this.gridViewPairs.SelectedItems[0].Index != this.gridViewPairs.Items.Count - 1;
      this.btnDeletePair.Enabled = count > 0 && this.gridViewPairs.Items.Count > 1;
      this.btnDeleteCoborrower.Enabled = count == 1;
      this.btnMoveBorrower.Enabled = this.btnMoveCoborrower.Enabled = count == 1;
      if (count == 0)
        return;
      int num1 = Utils.ParseInt((object) (string) this.gridViewPairs.SelectedItems[0].Tag);
      if (num1 < 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Borrower Pair information is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.txtBorFirst.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorMiddle.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorLast.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorSuffix.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorSSN.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobFirst.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobMiddle.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobLast.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobSuffix.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobSSN.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorCredit1.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorCredit2.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorCredit3.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobCredit1.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobCredit2.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobCredit3.LostFocus -= new EventHandler(this.borrowerInfo_LostFocus);
        this.cboBorrowerType.SelectedIndexChanged -= new EventHandler(this.borrowerInfo_SelectedIndexChanged);
        this.cboCoborrowerType.SelectedIndexChanged -= new EventHandler(this.borrowerInfo_SelectedIndexChanged);
        this.chkBorrSelfEmployed.CheckedChanged -= new EventHandler(this.borrowerInfo_CheckChanged);
        this.chkCoBorrSelfEmployed.CheckedChanged -= new EventHandler(this.borrowerInfo_CheckChanged);
        this.loan.SetBorrowerPair(this.pairs[num1 - 1]);
        this.loadFields(this.Controls);
        this.txtBorFirst.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorMiddle.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorLast.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorSuffix.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorSSN.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobFirst.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobMiddle.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobLast.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobSuffix.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobSSN.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorCredit1.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorCredit2.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtBorCredit3.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobCredit1.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobCredit2.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.txtCobCredit3.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
        this.cboBorrowerType.SelectedIndexChanged += new EventHandler(this.borrowerInfo_SelectedIndexChanged);
        this.cboCoborrowerType.SelectedIndexChanged += new EventHandler(this.borrowerInfo_SelectedIndexChanged);
        this.chkBorrSelfEmployed.CheckedChanged += new EventHandler(this.borrowerInfo_CheckChanged);
        this.chkCoBorrSelfEmployed.CheckedChanged += new EventHandler(this.borrowerInfo_CheckChanged);
        this.txtBorFirst.Focus();
      }
    }

    private void borrowerInfo_LostFocus(object sender, EventArgs e)
    {
      if (this.isFormClosed || this.gridViewPairs.SelectedItems.Count == 0 || !(sender is TextBox textBox) || textBox.Tag == null)
        return;
      string id = textBox.Tag.ToString();
      if (id == string.Empty)
        return;
      if (!textBox.ReadOnly)
        this.loan.SetField(id, textBox.Text.Trim(), true);
      this.gridViewPairs.SelectedItems[0].SubItems[1].Text = ((this.txtBorLast.Text.Trim() + " " + this.txtBorSuffix.Text.Trim()).Trim() + ", " + this.txtBorFirst.Text.Trim() + " " + this.txtBorMiddle.Text.Trim()).Trim();
      this.gridViewPairs.SelectedItems[0].SubItems[2].Text = ((this.txtCobLast.Text.Trim() + " " + this.txtCobSuffix.Text.Trim()).Trim() + ", " + this.txtCobFirst.Text.Trim() + " " + this.txtCobMiddle.Text.Trim()).Trim();
    }

    private void borrowerInfo_CheckChanged(object sender, EventArgs e)
    {
      if (this.gridViewPairs.SelectedItems.Count == 0 || !(sender is CheckBox checkBox) || checkBox.Tag == null)
        return;
      string id = checkBox.Tag.ToString();
      if (id == string.Empty)
        return;
      this.loan.SetField(id, checkBox.Checked ? "Y" : "N", true);
      this.loan.Calculator.FormCalculation(checkBox.Tag.ToString(), (string) null, (string) null);
    }

    private void borrowerInfo_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridViewPairs.SelectedItems.Count == 0 || !(sender is ComboBox comboBox) || comboBox.Tag == null)
        return;
      string id = comboBox.Tag.ToString();
      if (id == string.Empty)
        return;
      this.loan.SetField(id, comboBox.Text, true);
      this.loan.Calculator.FormCalculation(comboBox.Tag.ToString(), (string) null, (string) null);
    }

    public int AddBorrowerPair()
    {
      if (Session.SessionObjects.AllowConcurrentEditing && !Session.LoanDataMgr.LockLoanWithExclusiveA())
        return -1;
      this.loan.Calculator.UpdateAccountName("4000");
      if (this.gridViewPairs.Items.Count > 5)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "You only can have 6 borrower pairs.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      BorrowerPair borrowerPair = this.loan.CreateBorrowerPair();
      if (borrowerPair == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "Borrower pair cannot be created due to a problem.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      Cursor.Current = Cursors.WaitCursor;
      this.pairs = this.loan.GetBorrowerPairs();
      this.gridViewPairs.Items.Add(this.buildGridViewItem(borrowerPair, this.gridViewPairs.Items.Count + 1));
      this.gridViewPairs.SelectedItems.Clear();
      this.gridViewPairs.Items[this.gridViewPairs.Items.Count - 1].Selected = true;
      Cursor.Current = Cursors.Default;
      return this.gridViewPairs.Items.Count - 1;
    }

    private void btnAdd_Click(object sender, EventArgs e) => this.AddBorrowerPair();

    private void btnDeletePair_Click(object sender, EventArgs e)
    {
      if (this.gridViewPairs.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select the borrower pair to be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to remove borrower pair " + (object) (this.gridViewPairs.SelectedItems[0].Index + 1) + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes || !Session.LoanDataMgr.LockLoanWithExclusiveA())
          return;
        using (new CursorActivator(Cursors.WaitCursor))
        {
          int pair = Utils.ParseInt((object) (string) this.gridViewPairs.SelectedItems[0].Tag);
          if (pair < 1)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "Borrower Pair information is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            this.loan.eSignConsentDeleteBorrowerPair(pair, this.gridViewPairs.Items.Count);
            string id = this.pairs[pair - 1].Id;
            this.loan.RemoveBorrowerPair(this.pairs[pair - 1]);
            this.pairs = this.loan.GetBorrowerPairs();
            VestingPartyFields[] vestingPartyFields = this.loan.GetVestingPartyFields(false);
            for (int index = 1; index <= vestingPartyFields.Length; ++index)
            {
              string simpleField = this.loan.GetSimpleField("TR" + index.ToString("00") + "05");
              if (id.Trim() == simpleField.Trim())
                this.loan.SetField("TR" + index.ToString("00") + "05", this.pairs[0].Id);
            }
            this.loan.TriggerCalculation("4000", this.loan.GetField("4000"));
            if (this.loan.LinkedData != null)
              this.loan.SyncPiggyBackFiles(this.getPiggybackSyncFields(), false, true, (string) null, (string) null, false);
            this.initForm();
          }
        }
      }
    }

    private void btnDeleteCoborrower_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "The coborrower's data in the selected pairs will be permanently deleted from the loan. Are you sure you want to delete the coborrower?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      using (new CursorActivator(Cursors.WaitCursor))
      {
        int pair = Utils.ParseInt((object) (string) this.gridViewPairs.SelectedItems[0].Tag);
        if (this.loan.RemoveCoborrowers(new BorrowerPair[1]
        {
          this.pairs[pair - 1]
        }))
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The coborrower(s) have been removed from this loan successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The coborrower(s) cannot be removed from this loan.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        if (this.loan.LinkedData != null)
          this.loan.SyncPiggyBackFiles(this.getPiggybackSyncFields(), false, true, (string) null, (string) null, false);
        this.loan.eSignConsentDeleteCoBorrower(pair);
        this.initForm();
      }
    }

    private void txtBorSSN_KeyUp(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.SSN;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void SwapBorrowerPairForm_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.btnOK_Click((object) null, (EventArgs) null);
    }

    private string[] getPiggybackSyncFields()
    {
      if (this.piggyBackFieldSync == null)
        this.piggyBackFieldSync = Session.LoanDataMgr.SystemConfiguration.PiggybackSyncFields.GetSyncFields();
      return this.piggyBackFieldSync;
    }

    public string GetHelpTargetName() => "CoMortgagerWS";

    private void nameField_Leave(object sender, EventArgs e)
    {
      if (this.gridViewPairs.SelectedItems.Count == 0)
        return;
      this.loan.Calculator.UpdateAccountName(((Control) sender).Tag.ToString());
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp(this.emHelpLink1.HelpTag);
    }

    private void btnMoveCoborrower_Click(object sender, EventArgs e)
    {
      this.moveBorrowerConfirmation(true);
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      string str = string.Empty;
      using (ImportBorrowerForm importBorrowerForm = new ImportBorrowerForm(this.loan))
      {
        if (importBorrowerForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        str = importBorrowerForm.SelectedSource;
        this.importedFromCoborrower = importBorrowerForm.ImportedFromCoborrower;
        this.targetBorrowerPairNo = importBorrowerForm.TargetBorrowerPairNo;
        this.targetCoBorrower = importBorrowerForm.TargetCoBorrower;
      }
      switch (str)
      {
        case "ULAD / iLAD (MISMO 3.4) file":
          this.importBorrowerFromFannieMae(true);
          break;
        case "FNMA 3.2 file":
          this.importBorrowerFromFannieMae(false);
          break;
        case "Another loan file":
          this.selectBorrowerFromAnotherLoan();
          break;
        case "Contacts":
          this.importBorrowerFromContact();
          break;
      }
    }

    private void importBorrowerFromFannieMae(bool importFromMISMO34)
    {
      LoanData importedLoan = (LoanData) null;
      using (ImportBorrowerFromFNMAForm borrowerFromFnmaForm = new ImportBorrowerFromFNMAForm(this.importedFromCoborrower, importFromMISMO34))
      {
        if (borrowerFromFnmaForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
          importedLoan = borrowerFromFnmaForm.ImportedLoan;
      }
      if (importedLoan == null)
        return;
      this.importBorrower(importedLoan);
    }

    private void selectBorrowerFromAnotherLoan()
    {
      if (this.ImportFromLoanClicked == null)
        return;
      this.ImportFromLoanClicked((object) this, new EventArgs());
    }

    public void ImportBorrowerFromAnotherLoan(PipelineInfo selectedPipelineInfo)
    {
      if (selectedPipelineInfo == null)
        return;
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        LoanDataMgr loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, selectedPipelineInfo.GUID, false);
        if (loanDataMgr != null && loanDataMgr.LoanData != null)
        {
          if (this.importedFromCoborrower && loanDataMgr.LoanData.GetField("68") == "" && loanDataMgr.LoanData.GetField("69") == "")
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The selected loan doesn't have co-borrower. Please select another loan file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
            this.importBorrower(loanDataMgr.LoanData);
          loanDataMgr.Close();
        }
        else
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The selected loan cannot be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Cannot import borrower from the selected loan due to this error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Cursor.Current = Cursors.Default;
    }

    private void importBorrowerFromContact()
    {
      BorrowerInfo borrowerInfo = (BorrowerInfo) null;
      using (RxBorrowerContact rxBorrowerContact = new RxBorrowerContact(this.targetCoBorrower, false, false, Session.SessionObjects.UserID))
      {
        rxBorrowerContact.SetCurrentFilter((FieldFilterList) null);
        if (!rxBorrowerContact.ForcedClose)
        {
          if (rxBorrowerContact.ShowDialog((IWin32Window) this) == DialogResult.OK)
            borrowerInfo = rxBorrowerContact.BorrowerObj;
        }
      }
      if (borrowerInfo == null)
        return;
      Opportunity opportunityByBorrowerId = Session.SessionObjects.ContactManager.GetOpportunityByBorrowerId(borrowerInfo.ContactID);
      Cursor.Current = Cursors.WaitCursor;
      LoanDataMgr loanDataMgr = LoanDataMgr.BlankLoan(Session.SessionObjects);
      loanDataMgr.LoanData.IgnoreValidationErrors = true;
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "4004" : "4000", borrowerInfo.FirstName);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "4005" : "4001", borrowerInfo.MiddleName);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "4006" : "4002", borrowerInfo.LastName);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "4007" : "4003", borrowerInfo.SuffixName);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "97" : "65", borrowerInfo.SSN);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "1403" : "1402", borrowerInfo.Birthdate.ToString("MM/dd/yyyy"));
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "98" : "66", borrowerInfo.HomePhone);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "1179" : "1178", borrowerInfo.BizEmail);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "1268" : "1240", borrowerInfo.PersonalEmail);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "1480" : "1490", borrowerInfo.MobilePhone);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "84" : "52", borrowerInfo.Married ? "Married" : "");
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "FR0204" : "FR0104", borrowerInfo.HomeAddress.Street1 + (borrowerInfo.HomeAddress.Street2 != string.Empty ? " " + borrowerInfo.HomeAddress.Street2 : ""));
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "FR0206" : "FR0106", borrowerInfo.HomeAddress.City);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "FR0207" : "FR0107", borrowerInfo.HomeAddress.State);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "FR0208" : "FR0108", borrowerInfo.HomeAddress.Zip);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "FE0202" : "FE0102", borrowerInfo.EmployerName);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "FE0210" : "FE0110", borrowerInfo.JobTitle);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "FE0204" : "FE0104", borrowerInfo.BizAddress.Street1 + (borrowerInfo.BizAddress.Street2 != string.Empty ? " " + borrowerInfo.BizAddress.Street2 : ""));
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "FE0205" : "FE0105", borrowerInfo.BizAddress.City);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "FE0206" : "FE0106", borrowerInfo.BizAddress.State);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "FE0207" : "FE0107", borrowerInfo.BizAddress.Zip);
      loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "FE0217" : "FE0117", borrowerInfo.WorkPhone);
      if (opportunityByBorrowerId != null && opportunityByBorrowerId.Employment == EmploymentStatus.SelfEmployed)
        loanDataMgr.LoanData.SetField(this.importedFromCoborrower ? "FE0215" : "FE0115", "Y");
      this.importBorrower(loanDataMgr.LoanData);
      Cursor.Current = Cursors.Default;
    }

    private void importBorrower(LoanData importedLoan)
    {
      if (importedLoan == null)
        return;
      using (CursorActivator.Wait())
      {
        if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
          return;
        Cursor.Current = Cursors.WaitCursor;
        BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
        try
        {
          if (borrowerPairs.Length < this.targetBorrowerPairNo)
          {
            for (int index = borrowerPairs.Length + 1; index <= this.targetBorrowerPairNo; ++index)
              this.loan.CreateBorrowerPair();
            borrowerPairs = this.loan.GetBorrowerPairs();
          }
          this.loan.SetBorrowerPair(borrowerPairs[this.targetBorrowerPairNo - 1]);
        }
        catch (Exception ex)
        {
          Cursor.Current = Cursors.Default;
          int num = (int) Utils.Dialog((IWin32Window) this, "Borrower cannot be imported to this loan due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        string[] importFields1;
        string[] importFields2;
        if (this.targetCoBorrower)
        {
          importFields1 = SwapBorrowerPairForm.importCoBorrowerFields;
          importFields2 = SwapBorrowerPairForm.importHMDACoBorrowerFields;
        }
        else
        {
          importFields1 = SwapBorrowerPairForm.importBorrowerFields;
          importFields2 = SwapBorrowerPairForm.importHMDABorrowerFields;
        }
        string[] exportFields1;
        string[] exportFields2;
        if (this.importedFromCoborrower)
        {
          exportFields1 = SwapBorrowerPairForm.importCoBorrowerFields;
          exportFields2 = SwapBorrowerPairForm.importHMDACoBorrowerFields;
        }
        else
        {
          exportFields1 = SwapBorrowerPairForm.importBorrowerFields;
          exportFields2 = SwapBorrowerPairForm.importHMDABorrowerFields;
        }
        importedLoan.SetBorrowerPair(0);
        string empty = string.Empty;
        this.setImportedFields(importedLoan, importFields1, exportFields1, ref empty);
        if (importedLoan.GetField("4142") == "Y")
        {
          this.setImportedFields(importedLoan, importFields2, exportFields2, ref empty);
          this.loan.Calculator.FormCalculation("HMDASORTING", (string) null, (string) null);
        }
        this.importVerifications("FL", new string[60]
        {
          "15",
          "36",
          "02",
          "03",
          "04",
          "05",
          "06",
          "20",
          "21",
          "22",
          "23",
          "08",
          "09",
          "10",
          "37",
          "38",
          "13",
          "12",
          "11",
          "39",
          "16",
          "07",
          "14",
          "17",
          "18",
          "26",
          "27",
          "28",
          "29",
          "30",
          "31",
          "32",
          "64",
          "43",
          "44",
          "45",
          "46",
          "47",
          "48",
          "49",
          "50",
          "51",
          "52",
          "53",
          "54",
          "55",
          "56",
          "57",
          "58",
          "59",
          "60",
          "61",
          "62",
          "63",
          "65",
          "66",
          "67",
          "68",
          "69",
          "98"
        }, importedLoan);
        this.importVerifications(this.importedFromCoborrower ? "CR" : "BR", new string[37]
        {
          "13",
          "02",
          "03",
          "05",
          "09",
          "10",
          "11",
          "18",
          "19",
          "20",
          "21",
          "37",
          "38",
          "04",
          "06",
          "07",
          "08",
          "22",
          "12",
          "24",
          "16",
          "15",
          "14",
          "23",
          "25",
          "26",
          "27",
          "28",
          "29",
          "30",
          "36",
          "39",
          "40",
          "64",
          "44",
          "45",
          "98"
        }, importedLoan);
        this.importVerifications(this.importedFromCoborrower ? "CE" : "BE", new string[54]
        {
          "08",
          "36",
          "02",
          "03",
          "04",
          "05",
          "06",
          "07",
          "08",
          "17",
          "29",
          "30",
          "31",
          "37",
          "38",
          "32",
          "28",
          "10",
          "09",
          "11",
          "14",
          "13",
          "33",
          "16",
          "15",
          "27",
          "19",
          "20",
          "21",
          "22",
          "23",
          "12",
          "37",
          "38",
          "39",
          "64",
          "44",
          "45",
          "98",
          "52",
          "54",
          "55",
          "56",
          "58",
          "59",
          "60",
          "61",
          "62",
          "63",
          "75",
          "76",
          "78",
          "79",
          "80"
        }, importedLoan);
        this.importVerifications("DD", new string[41]
        {
          "24",
          "36",
          "02",
          "03",
          "04",
          "05",
          "06",
          "07",
          "26",
          "27",
          "28",
          "29",
          "37",
          "38",
          "08",
          "09",
          "10",
          "11",
          "12",
          "13",
          "14",
          "15",
          "16",
          "17",
          "18",
          "19",
          "20",
          "21",
          "22",
          "23",
          "34",
          "39",
          "40",
          "64",
          "44",
          "45",
          "48",
          "49",
          "50",
          "51",
          "98"
        }, importedLoan);
        this.importVerifications("FM", new string[25]
        {
          "36",
          "98",
          "37",
          "44",
          "45",
          "38",
          "04",
          "06",
          "07",
          "08",
          "28",
          "41",
          "19",
          "17",
          "16",
          "24",
          "18",
          "23",
          "22",
          "20",
          "21",
          "26",
          "14",
          "32",
          "25"
        }, importedLoan);
        if (this.loan.Use2020URLA && importedLoan.Use2020URLA)
        {
          this.importVerifications("URLAROA", new string[20]
          {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"
          }, importedLoan);
          this.importVerifications("URLARGG", new string[22]
          {
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "64"
          }, importedLoan);
          this.importVerifications("URLAROIS", new string[22]
          {
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "64"
          }, importedLoan);
          this.importVerifications("URLARAL", new string[34]
          {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "64"
          }, importedLoan);
          this.importVerifications("URLAROL", new string[25]
          {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "64",
            "98"
          }, importedLoan);
          this.importVerifications(this.importedFromCoborrower ? "URLACAKA" : "URLABAKA", new string[4]
          {
            "01",
            "02",
            "03",
            "04"
          }, importedLoan);
        }
        List<int> intList = new List<int>();
        for (int index = 144; index <= 150; index += 3)
        {
          if (this.loan.GetField(string.Concat((object) index)) == "" || this.targetCoBorrower && this.loan.GetField(string.Concat((object) index)) == "C" || !this.targetCoBorrower && this.loan.GetField(string.Concat((object) index)) == "B")
          {
            this.loan.SetField(string.Concat((object) index), "");
            this.loan.SetField(string.Concat((object) (index + 1)), "");
            this.loan.SetField(string.Concat((object) (index + 2)), "");
            intList.Add(index);
          }
        }
        if (intList.Count > 0)
        {
          int index1 = 0;
          for (int index2 = 144; index2 <= 150; index2 += 3)
          {
            if (this.importedFromCoborrower && importedLoan.GetField(string.Concat((object) index2)) == "C" || !this.importedFromCoborrower && importedLoan.GetField(string.Concat((object) index2)) == "B")
            {
              this.loan.SetField(string.Concat((object) intList[index1]), this.targetCoBorrower ? "C" : "B");
              this.loan.SetField(string.Concat((object) (intList[index1] + 1)), importedLoan.GetField(string.Concat((object) (index2 + 1))));
              this.loan.SetField(string.Concat((object) (intList[index1] + 2)), importedLoan.GetField(string.Concat((object) (index2 + 2))));
              ++index1;
            }
            if (index1 >= intList.Count)
              break;
          }
        }
        this.initForm();
        this.setCurrentPair(this.targetBorrowerPairNo);
        if (empty != string.Empty)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The following field(s) cannot be imported:\r\n" + empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        Cursor.Current = Cursors.Default;
      }
    }

    private void setImportedFields(
      LoanData importedLoan,
      string[] importFields,
      string[] exportFields,
      ref string msg)
    {
      for (int index = 0; index < importFields.Length; ++index)
      {
        try
        {
          this.loan.SetField(importFields[index], importedLoan.GetField(exportFields[index]));
        }
        catch (Exception ex)
        {
          msg = msg + (msg != string.Empty ? "," : "") + importFields[index];
        }
      }
    }

    private void importVerifications(
      string fieldPrefix,
      string[] verifiFields,
      LoanData importedLoan)
    {
      int num1 = 0;
      int num2 = 0;
      string str1 = string.Empty;
      switch (fieldPrefix)
      {
        case "BE":
          num1 = importedLoan.GetNumberOfEmployer(true);
          str1 = "08";
          break;
        case "BR":
          num1 = importedLoan.GetNumberOfResidence(true);
          str1 = "13";
          break;
        case "CE":
          num1 = importedLoan.GetNumberOfEmployer(false);
          str1 = "08";
          break;
        case "CR":
          num1 = importedLoan.GetNumberOfResidence(false);
          str1 = "13";
          break;
        case "DD":
          num1 = importedLoan.GetNumberOfDeposits();
          str1 = "24";
          break;
        case "FL":
          num1 = importedLoan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
          num2 = importedLoan.GetNumberOfMortgages();
          str1 = "15";
          break;
        case "URLABAKA":
          num1 = importedLoan.GetNumberOfURLAAlternateNames(true);
          break;
        case "URLACAKA":
          num1 = importedLoan.GetNumberOfURLAAlternateNames(false);
          break;
        case "URLARAL":
          num1 = importedLoan.GetNumberOfAdditionalLoans();
          str1 = "01";
          break;
        case "URLARGG":
          num1 = importedLoan.GetNumberOfGiftsAndGrants();
          str1 = "02";
          break;
        case "URLAROA":
          num1 = importedLoan.GetNumberOfOtherAssets();
          str1 = "01";
          break;
        case "URLAROIS":
          num1 = importedLoan.GetNumberOfOtherIncomeSources();
          str1 = "02";
          break;
        case "URLAROL":
          num1 = importedLoan.GetNumberOfOtherLiability();
          str1 = "01";
          break;
      }
      if (num1 == 0)
        return;
      string empty1 = string.Empty;
      string str2 = string.Empty;
      string empty2 = string.Empty;
      int num3 = 0;
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      List<string> stringList = new List<string>();
      for (int index1 = 1; index1 <= num1; ++index1)
      {
        string str3 = fieldPrefix + index1.ToString("00");
        if (!(fieldPrefix != "URLACAKA") || !(fieldPrefix != "URLABAKA") || (!this.importedFromCoborrower || string.Compare(importedLoan.GetField(str3 + str1), "coborrower", true) == 0) && (this.importedFromCoborrower || string.Compare(importedLoan.GetField(str3 + str1), "borrower", true) == 0) || string.Compare(importedLoan.GetField(str3 + str1), "both", true) == 0)
        {
          switch (fieldPrefix)
          {
            case "BE":
            case "CE":
              int num4 = this.loan.NewEmployer(!this.targetCoBorrower, string.Compare(importedLoan.GetField(str3 + "23"), "current", true) == 0) + 1;
              str2 = (this.targetCoBorrower ? "CE" : "BE") + num4.ToString("00");
              break;
            case "BR":
            case "CR":
              int num5 = this.loan.NewResidence(!this.targetCoBorrower, string.Compare(importedLoan.GetField(str3 + "23"), "current", true) == 0) + 1;
              str2 = (this.targetCoBorrower ? "CR" : "BR") + num5.ToString("00");
              break;
            case "DD":
              int num6 = this.loan.NewDeposit() + 1;
              str2 = fieldPrefix + num6.ToString("00");
              break;
            case "FL":
              int num7 = this.loan.NewLiability() + 1;
              str2 = fieldPrefix + num7.ToString("00");
              break;
            case "URLABAKA":
            case "URLACAKA":
              bool borrower = !this.importedFromCoborrower;
              num3 = this.loan.GetNumberOfURLAAlternateNames(!this.importedFromCoborrower) + 1;
              IList<URLAAlternateName> urlaAlternames = this.loan.GetURLAAlternames(borrower);
              URLAAlternateName urlaAlternateName = new URLAAlternateName("", importedLoan.GetField(str3 + "01"), importedLoan.GetField(str3 + "02"), importedLoan.GetField(str3 + "03"), importedLoan.GetField(str3 + "04"), importedLoan.GetField(str3 + "05"));
              urlaAlternames.Add(urlaAlternateName);
              this.loan.UpdateURLAAlternateNames(!this.targetCoBorrower, urlaAlternames);
              this.loan.Calculator.FormCalculation(this.targetCoBorrower ? "URLACAKA0101" : "URLABAKA0101");
              continue;
            case "URLARAL":
              int num8 = this.loan.NewAdditionalLoan() + 1;
              str2 = fieldPrefix + num8.ToString("00");
              break;
            case "URLARGG":
              int num9 = this.loan.NewGiftGrant() + 1;
              str2 = fieldPrefix + num9.ToString("00");
              break;
            case "URLAROA":
              int num10 = this.loan.NewOtherAsset() + 1;
              str2 = fieldPrefix + num10.ToString("00");
              break;
            case "URLAROIS":
              int num11 = this.loan.NewOtherIncomeSource() + 1;
              str2 = fieldPrefix + num11.ToString("00");
              break;
            case "URLAROL":
              int num12 = this.loan.NewOtherLiability() + 1;
              str2 = fieldPrefix + num12.ToString("00");
              break;
          }
          for (int index2 = 0; index2 < verifiFields.Length; ++index2)
          {
            try
            {
              string val = !(fieldPrefix == "BE") && !(fieldPrefix == "CE") || !(verifiFields[index2] == "11") || !importedLoan.Use2020URLA ? importedLoan.GetField(str3 + verifiFields[index2]) : importedLoan.GetField(str3 + "51");
              if (verifiFields[index2] == str1)
                val = this.targetCoBorrower ? "CoBorrower" : "Borrower";
              if ((fieldPrefix == "BE" || fieldPrefix == "CE") && verifiFields[index2] == "11" && this.loan.Use2020URLA)
                this.loan.SetField(str2 + "51", val);
              else
                this.loan.SetField(str2 + verifiFields[index2], val);
            }
            catch (Exception ex)
            {
            }
          }
          switch (fieldPrefix)
          {
            case "FL":
              string field1 = importedLoan.GetField(str3 + "25");
              if (field1 != string.Empty)
              {
                if (!insensitiveHashtable.ContainsKey((object) field1))
                {
                  for (int index3 = 1; index3 <= num2; ++index3)
                  {
                    string field2 = importedLoan.GetField("FM" + index3.ToString("00") + "46");
                    string field3 = importedLoan.GetField("FM" + index3.ToString("00") + "43");
                    bool flag1 = this.loan.Use2020URLA && importedLoan.Use2020URLA;
                    bool flag2 = !this.importedFromCoborrower && string.Compare(field2, "borrower", true) == 0;
                    bool flag3 = this.importedFromCoborrower && string.Compare(field2, "coborrower", true) == 0;
                    bool flag4 = string.Compare(field2, "both", true) == 0;
                    if (flag1 && string.Compare(field1, field3, true) == 0 && flag2 | flag3 | flag4 || !flag1 && string.Compare(field1, field3, true) == 0)
                    {
                      int num13 = this.loan.NewMortgage(string.Empty) + 1;
                      if (field1 != string.Empty && !stringList.Contains(field1))
                        stringList.Add(field1);
                      for (int index4 = 0; index4 < SwapBorrowerPairForm.vomFields.Length; ++index4)
                      {
                        try
                        {
                          this.loan.SetField("FM" + num13.ToString("00") + SwapBorrowerPairForm.vomFields[index4], importedLoan.GetField("FM" + index3.ToString("00") + SwapBorrowerPairForm.vomFields[index4]));
                        }
                        catch (Exception ex)
                        {
                        }
                      }
                      this.loan.SetField(str2 + "25", this.loan.GetField("FM" + num13.ToString("00") + "43"));
                      insensitiveHashtable.Add((object) field1, (object) this.loan.GetField("FM" + num13.ToString("00") + "43"));
                      break;
                    }
                  }
                  continue;
                }
                this.loan.SetField(str2 + "25", insensitiveHashtable[(object) field1].ToString());
                continue;
              }
              continue;
            case "BE":
            case "CE":
              string[] strArray = new string[11]
              {
                "77",
                "65",
                "66",
                "67",
                "68",
                "69",
                "70",
                "71",
                "72",
                "74",
                "53"
              };
              if (importedLoan.Use2020URLA && this.loan.Use2020URLA)
              {
                for (int index5 = 0; index5 < strArray.Length; ++index5)
                  this.loan.SetField(str2 + strArray[index5], importedLoan.GetField(str3 + strArray[index5]));
                continue;
              }
              if (importedLoan.Use2020URLA && !this.loan.Use2020URLA)
              {
                double num14 = Utils.ArithmeticRounding(Utils.ParseDouble((object) importedLoan.GetField(str3 + "53"), 0.0) + Utils.ParseDouble((object) importedLoan.GetField(str3 + "23"), 0.0), 2);
                this.loan.SetField(str2 + "23", num14 != 0.0 ? num14.ToString("N2") : "");
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
      if (!(fieldPrefix == "FL"))
        return;
      string empty3 = string.Empty;
      for (int index = 1; index <= num1; ++index)
      {
        string field = importedLoan.GetField("FL" + index.ToString("00") + "25");
        if (field != string.Empty && !stringList.Contains(field))
          stringList.Add(field);
      }
      for (int index6 = 1; index6 <= num2; ++index6)
      {
        string field4 = importedLoan.GetField("FM" + index6.ToString("00") + "43");
        string field5 = importedLoan.GetField("FM" + index6.ToString("00") + "46");
        bool flag5 = this.loan.Use2020URLA && importedLoan.Use2020URLA;
        bool flag6 = !this.importedFromCoborrower && string.Compare(field5, "borrower", true) == 0;
        bool flag7 = this.importedFromCoborrower && string.Compare(field5, "coborrower", true) == 0;
        bool flag8 = string.Compare(field5, "both", true) == 0;
        if (field4 != string.Empty && !stringList.Contains(field4) && (flag5 && flag6 | flag7 | flag8 || !flag5))
        {
          int num15 = this.loan.NewMortgage(string.Empty) + 1;
          for (int index7 = 0; index7 < SwapBorrowerPairForm.vomFields.Length; ++index7)
          {
            try
            {
              this.loan.SetField("FM" + num15.ToString("00") + SwapBorrowerPairForm.vomFields[index7], importedLoan.GetField("FM" + index6.ToString("00") + SwapBorrowerPairForm.vomFields[index7]));
            }
            catch (Exception ex)
            {
            }
          }
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
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SwapBorrowerPairForm));
      this.btnOK = new Button();
      this.toolTip1 = new ToolTip(this.components);
      this.btnMoveDown = new StandardIconButton();
      this.btnMoveUp = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.groupContainer2 = new GroupContainer();
      this.cboBorrowerType = new ComboBox();
      this.label17 = new Label();
      this.chkBorrSelfEmployed = new CheckBox();
      this.label5 = new Label();
      this.btnMoveBorrower = new Button();
      this.label1 = new Label();
      this.txtBorFirst = new TextBox();
      this.txtBorLast = new TextBox();
      this.label2 = new Label();
      this.txtBorSSN = new TextBox();
      this.label3 = new Label();
      this.label15 = new Label();
      this.label7 = new Label();
      this.label14 = new Label();
      this.txtBorCredit2 = new TextBox();
      this.txtBorSuffix = new TextBox();
      this.txtBorCredit3 = new TextBox();
      this.txtBorMiddle = new TextBox();
      this.txtBorCredit1 = new TextBox();
      this.label8 = new Label();
      this.label9 = new Label();
      this.groupContainer3 = new GroupContainer();
      this.cboCoborrowerType = new ComboBox();
      this.label20 = new Label();
      this.chkCoBorrSelfEmployed = new CheckBox();
      this.label13 = new Label();
      this.btnMoveCoborrower = new Button();
      this.btnDeleteCoborrower = new Button();
      this.label18 = new Label();
      this.label6 = new Label();
      this.label19 = new Label();
      this.txtCobFirst = new TextBox();
      this.txtCobSuffix = new TextBox();
      this.txtCobLast = new TextBox();
      this.txtCobMiddle = new TextBox();
      this.txtCobSSN = new TextBox();
      this.label16 = new Label();
      this.label4 = new Label();
      this.label12 = new Label();
      this.label10 = new Label();
      this.txtCobCredit2 = new TextBox();
      this.label11 = new Label();
      this.txtCobCredit3 = new TextBox();
      this.txtCobCredit1 = new TextBox();
      this.groupContainer1 = new GroupContainer();
      this.btnImport = new Button();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnDeletePair = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.gridViewPairs = new GridView();
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.emHelpLink1 = new EMHelpLink();
      ((ISupportInitialize) this.btnMoveDown).BeginInit();
      ((ISupportInitialize) this.btnMoveUp).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.groupContainer2.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(576, 498);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "&Close";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnMoveDown.BackColor = Color.Transparent;
      this.btnMoveDown.Location = new Point(56, 3);
      this.btnMoveDown.Margin = new Padding(3, 3, 2, 3);
      this.btnMoveDown.MouseDownImage = (Image) null;
      this.btnMoveDown.Name = "btnMoveDown";
      this.btnMoveDown.Size = new Size(16, 16);
      this.btnMoveDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveDown.TabIndex = 11;
      this.btnMoveDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveDown, "Move Down");
      this.btnMoveDown.Click += new EventHandler(this.btnMoveDown_Click);
      this.btnMoveUp.BackColor = Color.Transparent;
      this.btnMoveUp.Location = new Point(35, 3);
      this.btnMoveUp.Margin = new Padding(3, 3, 2, 3);
      this.btnMoveUp.MouseDownImage = (Image) null;
      this.btnMoveUp.Name = "btnMoveUp";
      this.btnMoveUp.Size = new Size(16, 16);
      this.btnMoveUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveUp.TabIndex = 12;
      this.btnMoveUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveUp, "Move Up");
      this.btnMoveUp.Click += new EventHandler(this.btnMoveUp_Click);
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(14, 3);
      this.btnAdd.Margin = new Padding(3, 3, 2, 3);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 8;
      this.btnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAdd, "Add Borrower Pair");
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.groupContainer2.Controls.Add((Control) this.cboBorrowerType);
      this.groupContainer2.Controls.Add((Control) this.label17);
      this.groupContainer2.Controls.Add((Control) this.chkBorrSelfEmployed);
      this.groupContainer2.Controls.Add((Control) this.label5);
      this.groupContainer2.Controls.Add((Control) this.btnMoveBorrower);
      this.groupContainer2.Controls.Add((Control) this.label1);
      this.groupContainer2.Controls.Add((Control) this.txtBorFirst);
      this.groupContainer2.Controls.Add((Control) this.txtBorLast);
      this.groupContainer2.Controls.Add((Control) this.label2);
      this.groupContainer2.Controls.Add((Control) this.txtBorSSN);
      this.groupContainer2.Controls.Add((Control) this.label3);
      this.groupContainer2.Controls.Add((Control) this.label15);
      this.groupContainer2.Controls.Add((Control) this.label7);
      this.groupContainer2.Controls.Add((Control) this.label14);
      this.groupContainer2.Controls.Add((Control) this.txtBorCredit2);
      this.groupContainer2.Controls.Add((Control) this.txtBorSuffix);
      this.groupContainer2.Controls.Add((Control) this.txtBorCredit3);
      this.groupContainer2.Controls.Add((Control) this.txtBorMiddle);
      this.groupContainer2.Controls.Add((Control) this.txtBorCredit1);
      this.groupContainer2.Controls.Add((Control) this.label8);
      this.groupContainer2.Controls.Add((Control) this.label9);
      this.groupContainer2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(12, (int) byte.MaxValue);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(318, 235);
      this.groupContainer2.TabIndex = 1;
      this.groupContainer2.Text = "Borrower";
      this.cboBorrowerType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrowerType.FormattingEnabled = true;
      this.cboBorrowerType.Items.AddRange(new object[11]
      {
        (object) "",
        (object) "Individual",
        (object) "Co-signer",
        (object) "Title only",
        (object) "Non Title Spouse",
        (object) "Trustee",
        (object) "Title Only Trustee",
        (object) "Settlor Trustee",
        (object) "Settlor",
        (object) "Title Only Settlor Trustee",
        (object) "Officer"
      });
      this.cboBorrowerType.Location = new Point(117, 31);
      this.cboBorrowerType.Name = "cboBorrowerType";
      this.cboBorrowerType.Size = new Size(189, 22);
      this.cboBorrowerType.TabIndex = 5;
      this.cboBorrowerType.Tag = (object) "4008";
      this.cboBorrowerType.SelectedIndexChanged += new EventHandler(this.borrowerInfo_SelectedIndexChanged);
      this.label17.AutoSize = true;
      this.label17.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label17.Location = new Point(6, 36);
      this.label17.Name = "label17";
      this.label17.Size = new Size(30, 14);
      this.label17.TabIndex = 39;
      this.label17.Text = "Vesting Type";
      this.chkBorrSelfEmployed.AutoSize = true;
      this.chkBorrSelfEmployed.Location = new Point(117, 213);
      this.chkBorrSelfEmployed.Name = "chkBorrSelfEmployed";
      this.chkBorrSelfEmployed.Size = new Size(15, 14);
      this.chkBorrSelfEmployed.TabIndex = 38;
      this.chkBorrSelfEmployed.Tag = (object) "FE0115";
      this.chkBorrSelfEmployed.UseVisualStyleBackColor = true;
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(6, 212);
      this.label5.Name = "label5";
      this.label5.Size = new Size(76, 14);
      this.label5.TabIndex = 37;
      this.label5.Text = "Self-Employed";
      this.btnMoveBorrower.BackColor = SystemColors.Control;
      this.btnMoveBorrower.Location = new Point(236, 2);
      this.btnMoveBorrower.Margin = new Padding(3, 0, 0, 0);
      this.btnMoveBorrower.Name = "btnMoveBorrower";
      this.btnMoveBorrower.Padding = new Padding(2, 0, 0, 0);
      this.btnMoveBorrower.Size = new Size(75, 22);
      this.btnMoveBorrower.TabIndex = 36;
      this.btnMoveBorrower.Text = "Move";
      this.btnMoveBorrower.UseVisualStyleBackColor = true;
      this.btnMoveBorrower.Click += new EventHandler(this.btnSwapCoborrower_Click);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(6, 58);
      this.label1.Name = "label1";
      this.label1.Size = new Size(58, 14);
      this.label1.TabIndex = 1;
      this.label1.Text = "First Name";
      this.txtBorFirst.Location = new Point(117, 55);
      this.txtBorFirst.Name = "txtBorFirst";
      this.txtBorFirst.Size = new Size(189, 20);
      this.txtBorFirst.TabIndex = 6;
      this.txtBorFirst.Tag = (object) "4000";
      this.txtBorFirst.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtBorFirst.Leave += new EventHandler(this.nameField_Leave);
      this.txtBorLast.Location = new Point(117, 99);
      this.txtBorLast.Name = "txtBorLast";
      this.txtBorLast.Size = new Size(104, 20);
      this.txtBorLast.TabIndex = 8;
      this.txtBorLast.Tag = (object) "4002";
      this.txtBorLast.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtBorLast.Leave += new EventHandler(this.nameField_Leave);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(6, 102);
      this.label2.Name = "label2";
      this.label2.Size = new Size(58, 14);
      this.label2.TabIndex = 3;
      this.label2.Text = "Last Name";
      this.txtBorSSN.Location = new Point(117, 121);
      this.txtBorSSN.Name = "txtBorSSN";
      this.txtBorSSN.Size = new Size(104, 20);
      this.txtBorSSN.TabIndex = 10;
      this.txtBorSSN.Tag = (object) "65";
      this.txtBorSSN.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtBorSSN.KeyUp += new KeyEventHandler(this.txtBorSSN_KeyUp);
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(6, 124);
      this.label3.Name = "label3";
      this.label3.Size = new Size(28, 14);
      this.label3.TabIndex = 5;
      this.label3.Text = "SSN";
      this.label15.AutoSize = true;
      this.label15.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label15.Location = new Point(230, 102);
      this.label15.Name = "label15";
      this.label15.Size = new Size(36, 14);
      this.label15.TabIndex = 28;
      this.label15.Text = "Suffix";
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(6, 146);
      this.label7.Name = "label7";
      this.label7.Size = new Size(75, 14);
      this.label7.TabIndex = 12;
      this.label7.Text = "Experian/FICO";
      this.label14.AutoSize = true;
      this.label14.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label14.Location = new Point(6, 80);
      this.label14.Name = "label14";
      this.label14.Size = new Size(37, 14);
      this.label14.TabIndex = 27;
      this.label14.Text = "Middle";
      this.txtBorCredit2.Location = new Point(117, 165);
      this.txtBorCredit2.Name = "txtBorCredit2";
      this.txtBorCredit2.Size = new Size(104, 20);
      this.txtBorCredit2.TabIndex = 12;
      this.txtBorCredit2.Tag = (object) "1450";
      this.txtBorCredit2.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtBorSuffix.Location = new Point(269, 99);
      this.txtBorSuffix.Name = "txtBorSuffix";
      this.txtBorSuffix.Size = new Size(37, 20);
      this.txtBorSuffix.TabIndex = 9;
      this.txtBorSuffix.Tag = (object) "4003";
      this.txtBorSuffix.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtBorSuffix.Leave += new EventHandler(this.nameField_Leave);
      this.txtBorCredit3.Location = new Point(117, 187);
      this.txtBorCredit3.Name = "txtBorCredit3";
      this.txtBorCredit3.Size = new Size(104, 20);
      this.txtBorCredit3.TabIndex = 13;
      this.txtBorCredit3.Tag = (object) "1414";
      this.txtBorCredit3.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtBorMiddle.Location = new Point(117, 77);
      this.txtBorMiddle.Name = "txtBorMiddle";
      this.txtBorMiddle.Size = new Size(189, 20);
      this.txtBorMiddle.TabIndex = 7;
      this.txtBorMiddle.Tag = (object) "4001";
      this.txtBorMiddle.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtBorMiddle.Leave += new EventHandler(this.nameField_Leave);
      this.txtBorCredit1.Location = new Point(117, 143);
      this.txtBorCredit1.Name = "txtBorCredit1";
      this.txtBorCredit1.Size = new Size(104, 20);
      this.txtBorCredit1.TabIndex = 11;
      this.txtBorCredit1.Tag = (object) "67";
      this.txtBorCredit1.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(6, 168);
      this.label8.Name = "label8";
      this.label8.Size = new Size(108, 14);
      this.label8.TabIndex = 16;
      this.label8.Text = "Trans Union/Empirica";
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(6, 190);
      this.label9.Name = "label9";
      this.label9.Size = new Size(91, 14);
      this.label9.TabIndex = 17;
      this.label9.Text = "EquiFax/BEACON";
      this.groupContainer3.Controls.Add((Control) this.cboCoborrowerType);
      this.groupContainer3.Controls.Add((Control) this.label20);
      this.groupContainer3.Controls.Add((Control) this.chkCoBorrSelfEmployed);
      this.groupContainer3.Controls.Add((Control) this.label13);
      this.groupContainer3.Controls.Add((Control) this.btnMoveCoborrower);
      this.groupContainer3.Controls.Add((Control) this.btnDeleteCoborrower);
      this.groupContainer3.Controls.Add((Control) this.label18);
      this.groupContainer3.Controls.Add((Control) this.label6);
      this.groupContainer3.Controls.Add((Control) this.label19);
      this.groupContainer3.Controls.Add((Control) this.txtCobFirst);
      this.groupContainer3.Controls.Add((Control) this.txtCobSuffix);
      this.groupContainer3.Controls.Add((Control) this.txtCobLast);
      this.groupContainer3.Controls.Add((Control) this.txtCobMiddle);
      this.groupContainer3.Controls.Add((Control) this.txtCobSSN);
      this.groupContainer3.Controls.Add((Control) this.label16);
      this.groupContainer3.Controls.Add((Control) this.label4);
      this.groupContainer3.Controls.Add((Control) this.label12);
      this.groupContainer3.Controls.Add((Control) this.label10);
      this.groupContainer3.Controls.Add((Control) this.txtCobCredit2);
      this.groupContainer3.Controls.Add((Control) this.label11);
      this.groupContainer3.Controls.Add((Control) this.txtCobCredit3);
      this.groupContainer3.Controls.Add((Control) this.txtCobCredit1);
      this.groupContainer3.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(334, (int) byte.MaxValue);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(317, 235);
      this.groupContainer3.TabIndex = 2;
      this.groupContainer3.Text = "Co-Borrower";
      this.cboCoborrowerType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCoborrowerType.FormattingEnabled = true;
      this.cboCoborrowerType.Items.AddRange(new object[11]
      {
        (object) "",
        (object) "Individual",
        (object) "Co-signer",
        (object) "Title only",
        (object) "Non Title Spouse",
        (object) "Trustee",
        (object) "Title Only Trustee",
        (object) "Settlor Trustee",
        (object) "Settlor",
        (object) "Title Only Settlor Trustee",
        (object) "Officer"
      });
      this.cboCoborrowerType.Location = new Point(117, 31);
      this.cboCoborrowerType.Name = "cboCoborrowerType";
      this.cboCoborrowerType.Size = new Size(189, 22);
      this.cboCoborrowerType.TabIndex = 14;
      this.cboCoborrowerType.Tag = (object) "4009";
      this.cboCoborrowerType.SelectedIndexChanged += new EventHandler(this.borrowerInfo_SelectedIndexChanged);
      this.label20.AutoSize = true;
      this.label20.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label20.Location = new Point(6, 36);
      this.label20.Name = "label20";
      this.label20.Size = new Size(30, 14);
      this.label20.TabIndex = 41;
      this.label20.Text = "Vesting Type";
      this.chkCoBorrSelfEmployed.AutoSize = true;
      this.chkCoBorrSelfEmployed.Location = new Point(118, 213);
      this.chkCoBorrSelfEmployed.Name = "chkCoBorrSelfEmployed";
      this.chkCoBorrSelfEmployed.Size = new Size(15, 14);
      this.chkCoBorrSelfEmployed.TabIndex = 40;
      this.chkCoBorrSelfEmployed.Tag = (object) "FE0215";
      this.chkCoBorrSelfEmployed.UseVisualStyleBackColor = true;
      this.label13.AutoSize = true;
      this.label13.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(6, 212);
      this.label13.Name = "label13";
      this.label13.Size = new Size(76, 14);
      this.label13.TabIndex = 39;
      this.label13.Text = "Self-Employed";
      this.btnMoveCoborrower.Location = new Point(118, 2);
      this.btnMoveCoborrower.Name = "btnMoveCoborrower";
      this.btnMoveCoborrower.Size = new Size(75, 22);
      this.btnMoveCoborrower.TabIndex = 36;
      this.btnMoveCoborrower.Text = "Move";
      this.btnMoveCoborrower.UseVisualStyleBackColor = true;
      this.btnMoveCoborrower.Click += new EventHandler(this.btnMoveCoborrower_Click);
      this.btnDeleteCoborrower.BackColor = SystemColors.Control;
      this.btnDeleteCoborrower.Location = new Point(196, 2);
      this.btnDeleteCoborrower.Margin = new Padding(3, 0, 0, 0);
      this.btnDeleteCoborrower.Name = "btnDeleteCoborrower";
      this.btnDeleteCoborrower.Padding = new Padding(2, 0, 0, 0);
      this.btnDeleteCoborrower.Size = new Size(116, 22);
      this.btnDeleteCoborrower.TabIndex = 35;
      this.btnDeleteCoborrower.Text = "Delete &Co-Borrower";
      this.btnDeleteCoborrower.UseVisualStyleBackColor = true;
      this.btnDeleteCoborrower.Click += new EventHandler(this.btnDeleteCoborrower_Click);
      this.label18.AutoSize = true;
      this.label18.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label18.Location = new Point(230, 102);
      this.label18.Name = "label18";
      this.label18.Size = new Size(36, 14);
      this.label18.TabIndex = 34;
      this.label18.Text = "Suffix";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(6, 58);
      this.label6.Name = "label6";
      this.label6.Size = new Size(58, 14);
      this.label6.TabIndex = 7;
      this.label6.Text = "First Name";
      this.label19.AutoSize = true;
      this.label19.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label19.Location = new Point(6, 102);
      this.label19.Name = "label19";
      this.label19.Size = new Size(58, 14);
      this.label19.TabIndex = 33;
      this.label19.Text = "Last Name";
      this.txtCobFirst.Location = new Point(117, 55);
      this.txtCobFirst.Name = "txtCobFirst";
      this.txtCobFirst.Size = new Size(189, 20);
      this.txtCobFirst.TabIndex = 15;
      this.txtCobFirst.Tag = (object) "4004";
      this.txtCobFirst.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtCobFirst.Leave += new EventHandler(this.nameField_Leave);
      this.txtCobSuffix.Location = new Point(269, 99);
      this.txtCobSuffix.Name = "txtCobSuffix";
      this.txtCobSuffix.Size = new Size(37, 20);
      this.txtCobSuffix.TabIndex = 18;
      this.txtCobSuffix.Tag = (object) "4007";
      this.txtCobSuffix.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtCobSuffix.Leave += new EventHandler(this.nameField_Leave);
      this.txtCobLast.Location = new Point(117, 99);
      this.txtCobLast.Name = "txtCobLast";
      this.txtCobLast.Size = new Size(104, 20);
      this.txtCobLast.TabIndex = 17;
      this.txtCobLast.Tag = (object) "4006";
      this.txtCobLast.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtCobLast.Leave += new EventHandler(this.nameField_Leave);
      this.txtCobMiddle.Location = new Point(117, 77);
      this.txtCobMiddle.Name = "txtCobMiddle";
      this.txtCobMiddle.Size = new Size(189, 20);
      this.txtCobMiddle.TabIndex = 16;
      this.txtCobMiddle.Tag = (object) "4005";
      this.txtCobMiddle.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtCobMiddle.Leave += new EventHandler(this.nameField_Leave);
      this.txtCobSSN.Location = new Point(117, 121);
      this.txtCobSSN.Name = "txtCobSSN";
      this.txtCobSSN.Size = new Size(104, 20);
      this.txtCobSSN.TabIndex = 19;
      this.txtCobSSN.Tag = (object) "97";
      this.txtCobSSN.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtCobSSN.KeyUp += new KeyEventHandler(this.txtBorSSN_KeyUp);
      this.label16.AutoSize = true;
      this.label16.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label16.Location = new Point(6, 80);
      this.label16.Name = "label16";
      this.label16.Size = new Size(37, 14);
      this.label16.TabIndex = 30;
      this.label16.Text = "Middle";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(6, 124);
      this.label4.Name = "label4";
      this.label4.Size = new Size(28, 14);
      this.label4.TabIndex = 11;
      this.label4.Text = "SSN";
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(6, 146);
      this.label12.Name = "label12";
      this.label12.Size = new Size(75, 14);
      this.label12.TabIndex = 18;
      this.label12.Text = "Experian/FICO";
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(6, 190);
      this.label10.Name = "label10";
      this.label10.Size = new Size(91, 14);
      this.label10.TabIndex = 23;
      this.label10.Text = "EquiFax/BEACON";
      this.txtCobCredit2.Location = new Point(117, 165);
      this.txtCobCredit2.Name = "txtCobCredit2";
      this.txtCobCredit2.Size = new Size(104, 20);
      this.txtCobCredit2.TabIndex = 21;
      this.txtCobCredit2.Tag = (object) "1452";
      this.txtCobCredit2.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(6, 168);
      this.label11.Name = "label11";
      this.label11.Size = new Size(108, 14);
      this.label11.TabIndex = 22;
      this.label11.Text = "Trans Union/Empirica";
      this.txtCobCredit3.Location = new Point(117, 187);
      this.txtCobCredit3.Name = "txtCobCredit3";
      this.txtCobCredit3.Size = new Size(104, 20);
      this.txtCobCredit3.TabIndex = 22;
      this.txtCobCredit3.Tag = (object) "1415";
      this.txtCobCredit3.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.txtCobCredit1.Location = new Point(117, 143);
      this.txtCobCredit1.Name = "txtCobCredit1";
      this.txtCobCredit1.Size = new Size(104, 20);
      this.txtCobCredit1.TabIndex = 20;
      this.txtCobCredit1.Tag = (object) "60";
      this.txtCobCredit1.LostFocus += new EventHandler(this.borrowerInfo_LostFocus);
      this.groupContainer1.Controls.Add((Control) this.btnImport);
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Controls.Add((Control) this.gridViewPairs);
      this.groupContainer1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(639, 239);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Borrower Pairs";
      this.btnImport.Location = new Point(349, 2);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(75, 22);
      this.btnImport.TabIndex = 37;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDeletePair);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMoveDown);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMoveUp);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAdd);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(430, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(204, 22);
      this.flowLayoutPanel1.TabIndex = 12;
      this.btnDeletePair.BackColor = SystemColors.Control;
      this.btnDeletePair.Location = new Point(84, 0);
      this.btnDeletePair.Margin = new Padding(3, 0, 0, 0);
      this.btnDeletePair.Name = "btnDeletePair";
      this.btnDeletePair.Padding = new Padding(2, 0, 0, 0);
      this.btnDeletePair.Size = new Size(120, 22);
      this.btnDeletePair.TabIndex = 5;
      this.btnDeletePair.Text = "&Delete Borrower Pair";
      this.btnDeletePair.UseVisualStyleBackColor = true;
      this.btnDeletePair.Click += new EventHandler(this.btnDeletePair_Click);
      this.verticalSeparator1.Location = new Point(77, 3);
      this.verticalSeparator1.Margin = new Padding(3, 3, 2, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 10;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.gridViewPairs.AllowMultiselect = false;
      this.gridViewPairs.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Pair";
      gvColumn1.Width = 30;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Borrower";
      gvColumn2.Width = 300;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Co-Borrower";
      gvColumn3.Width = 300;
      this.gridViewPairs.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridViewPairs.Dock = DockStyle.Fill;
      this.gridViewPairs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewPairs.Location = new Point(1, 26);
      this.gridViewPairs.Name = "gridViewPairs";
      this.gridViewPairs.Size = new Size(637, 212);
      this.gridViewPairs.SortOption = GVSortOption.None;
      this.gridViewPairs.TabIndex = 11;
      this.gridViewPairs.SelectedIndexChanged += new EventHandler(this.gridViewPairs_SelectedIndexChanged);
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(470, 506);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 70;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(436, 508);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 69;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "CoMortgagerWS";
      this.emHelpLink1.Location = new Point(12, 501);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 71;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.ClientSize = new Size(663, 529);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.pboxDownArrow);
      this.Controls.Add((Control) this.pboxAsterisk);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.groupContainer3);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SwapBorrowerPairForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Manage Borrowers";
      this.FormClosing += new FormClosingEventHandler(this.SwapBorrowerPairForm_FormClosing);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.KeyPress += new KeyPressEventHandler(this.SwapBorrowerPairForm_KeyPress);
      ((ISupportInitialize) this.btnMoveDown).EndInit();
      ((ISupportInitialize) this.btnMoveUp).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      this.ResumeLayout(false);
    }
  }
}
