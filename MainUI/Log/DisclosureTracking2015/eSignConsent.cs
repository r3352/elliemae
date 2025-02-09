// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.DisclosureTracking2015.eSignConsent
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.LoanUtils.EDelivery;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Log.DisclosureTracking2015
{
  public class eSignConsent : Form
  {
    private const string className = "eSignConsent";
    protected static string sw = Tracing.SwOutsideLoan;
    private const string EXTERNAL_eCONSENT_FILENAME = "FullExternalEConsent.pdf";
    private LoanData loanData;
    private Sessions.Session session;
    private BorrowerPair[] pairs;
    private Dictionary<string, bool> locks = new Dictionary<string, bool>();
    private Dictionary<string, string> lockValues = new Dictionary<string, string>();
    private bool IsEveryNBOAccepted = true;
    private DateTime latestNBOAcceptedDate;
    private Dictionary<string, string[]> eConsentsToBeWithdrawn = new Dictionary<string, string[]>();
    public static Dictionary<string, string> borrowerCoborrower_recipientIDFields = new Dictionary<string, string>()
    {
      {
        "b1",
        "4956"
      },
      {
        "c1",
        "4957"
      },
      {
        "b2",
        "4958"
      },
      {
        "c2",
        "4959"
      },
      {
        "b3",
        "4960"
      },
      {
        "c3",
        "4961"
      },
      {
        "b4",
        "4962"
      },
      {
        "c4",
        "4963"
      },
      {
        "b5",
        "4964"
      },
      {
        "c5",
        "4965"
      },
      {
        "b6",
        "4966"
      },
      {
        "c6",
        "4967"
      }
    };
    private IContainer components;
    private GroupContainer groupContainer1;
    private TextBox txtConsentDate;
    private Label label1;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private TextBox txtCrEmail1;
    private TextBox txtBrEmail1;
    private TextBox txtCrName1;
    private TextBox txtBrName1;
    private Label label9;
    private Label label8;
    private ComboBox cmbBrStatus1;
    private FieldLockButton lBtnCrStatus1;
    private ComboBox cmbCrStatus1;
    private FieldLockButton lBtnBrStatus1;
    private CalendarButton clBrDate1;
    private FieldLockButton lBtnCrDate1;
    private FieldLockButton lBtnBrDate1;
    private TextBox txtCrDate1;
    private CalendarButton clCrDate1;
    private TextBox txtBrDate1;
    private TextBox txtCrSource1;
    private TextBox txtBrSource1;
    private FieldLockButton lBtnCrSource1;
    private FieldLockButton lBtnBrSource1;
    private TextBox txtCrIPAddress1;
    private TextBox txtBrIPAddress1;
    private FieldLockButton lBtnCrIPAddress1;
    private FieldLockButton lBtnBrIPAddress1;
    private TextBox txtCrSource5;
    private TextBox txtBrSource5;
    private FieldLockButton lBtnCrSource5;
    private FieldLockButton lBtnBrSource5;
    private TextBox txtCrIPAddress5;
    private TextBox txtBrIPAddress5;
    private FieldLockButton lBtnCrIPAddress5;
    private FieldLockButton lBtnBrIPAddress5;
    private TextBox txtCrDate5;
    private CalendarButton clCrDate5;
    private TextBox txtBrDate5;
    private CalendarButton clBrDate5;
    private FieldLockButton lBtnCrDate5;
    private FieldLockButton lBtnBrDate5;
    private FieldLockButton lBtnCrStatus5;
    private ComboBox cmbCrStatus5;
    private FieldLockButton lBtnBrStatus5;
    private ComboBox cmbBrStatus5;
    private TextBox txtCrEmail5;
    private TextBox txtBrEmail5;
    private TextBox txtCrName5;
    private TextBox txtBrName5;
    private Label label16;
    private Label label17;
    private TextBox txtCrSource4;
    private TextBox txtBrSource4;
    private FieldLockButton lBtnCrSource4;
    private FieldLockButton lBtnBrSource4;
    private TextBox txtCrIPAddress4;
    private TextBox txtBrIPAddress4;
    private FieldLockButton lBtnCrIPAddress4;
    private FieldLockButton lBtnBrIPAddress4;
    private TextBox txtCrDate4;
    private CalendarButton clCrDate4;
    private TextBox txtBrDate4;
    private CalendarButton clBrDate4;
    private FieldLockButton lBtnCrDate4;
    private FieldLockButton lBtnBrDate4;
    private FieldLockButton lBtnCrStatus4;
    private ComboBox cmbCrStatus4;
    private FieldLockButton lBtnBrStatus4;
    private ComboBox cmbBrStatus4;
    private TextBox txtCrEmail4;
    private TextBox txtBrEmail4;
    private TextBox txtCrName4;
    private TextBox txtBrName4;
    private Label label14;
    private Label label15;
    private TextBox txtCrSource3;
    private TextBox txtBrSource3;
    private FieldLockButton lBtnCrSource3;
    private FieldLockButton lBtnBrSource3;
    private TextBox txtCrIPAddress3;
    private TextBox txtBrIPAddress3;
    private FieldLockButton lBtnCrIPAddress3;
    private FieldLockButton lBtnBrIPAddress3;
    private TextBox txtCrDate3;
    private CalendarButton clCrDate3;
    private TextBox txtBrDate3;
    private CalendarButton clBrDate3;
    private FieldLockButton lBtnCrDate3;
    private FieldLockButton lBtnBrDate3;
    private FieldLockButton lBtnCrStatus3;
    private ComboBox cmbCrStatus3;
    private FieldLockButton lBtnBrStatus3;
    private ComboBox cmbBrStatus3;
    private TextBox txtCrEmail3;
    private TextBox txtBrEmail3;
    private TextBox txtCrName3;
    private TextBox txtBrName3;
    private Label label12;
    private Label label13;
    private TextBox txtCrSource2;
    private TextBox txtBrSource2;
    private FieldLockButton lBtnCrSource2;
    private FieldLockButton lBtnBrSource2;
    private TextBox txtCrIPAddress2;
    private TextBox txtBrIPAddress2;
    private FieldLockButton lBtnCrIPAddress2;
    private FieldLockButton lBtnBrIPAddress2;
    private TextBox txtCrDate2;
    private CalendarButton clCrDate2;
    private TextBox txtBrDate2;
    private CalendarButton clBrDate2;
    private FieldLockButton lBtnCrDate2;
    private FieldLockButton lBtnBrDate2;
    private FieldLockButton lBtnCrStatus2;
    private ComboBox cmbCrStatus2;
    private FieldLockButton lBtnBrStatus2;
    private ComboBox cmbBrStatus2;
    private TextBox txtCrEmail2;
    private TextBox txtBrEmail2;
    private TextBox txtCrName2;
    private TextBox txtBrName2;
    private Label label10;
    private Label label11;
    private TextBox txtCrSource6;
    private TextBox txtBrSource6;
    private FieldLockButton lBtnCrSource6;
    private FieldLockButton lBtnBrSource6;
    private TextBox txtCrIPAddress6;
    private TextBox txtBrIPAddress6;
    private FieldLockButton lBtnCrIPAddress6;
    private FieldLockButton lBtnBrIPAddress6;
    private TextBox txtCrDate6;
    private CalendarButton clCrDate6;
    private TextBox txtBrDate6;
    private CalendarButton clBrDate6;
    private FieldLockButton lBtnCrDate6;
    private FieldLockButton lBtnBrDate6;
    private FieldLockButton lBtnCrStatus6;
    private ComboBox cmbCrStatus6;
    private FieldLockButton lBtnBrStatus6;
    private ComboBox cmbBrStatus6;
    private TextBox txtCrEmail6;
    private TextBox txtBrEmail6;
    private TextBox txtCrName6;
    private TextBox txtBrName6;
    private Label label18;
    private Label label19;
    private Button btnOK;
    private Button btnCancel;
    private Panel panel6;
    private Panel panel5;
    private Panel panel4;
    private Panel panel3;
    private Panel panel2;
    private Panel panel1;
    private System.Windows.Forms.LinkLabel viewFormLink;
    private Panel pnlCr1;
    private Panel pnlCr2;
    private Panel pnlCr3;
    private Panel pnlCr4;
    private Panel pnlCr6;
    private Panel pnlCr5;
    private Panel panel7;
    private Panel panel8;
    private Button btnBrWithdraw6;
    private TextBox txtBrWithdrawnBy6;
    private Button btnCrWithdraw6;
    private TextBox txtCrWithdrawnBy6;
    private Button btnBrWithdraw5;
    private TextBox txtBrWithdrawnBy5;
    private Button btnCrWithdraw5;
    private TextBox txtCrWithdrawnBy5;
    private Button btnBrWithdraw4;
    private TextBox txtBrWithdrawnBy4;
    private Button btnCrWithdraw4;
    private TextBox txtCrWithdrawnBy4;
    private Button btnBrWithdraw3;
    private TextBox txtBrWithdrawnBy3;
    private Button btnCrWithdraw3;
    private TextBox txtCrWithdrawnBy3;
    private Button btnBrWithdraw2;
    private TextBox txtBrWithdrawnBy2;
    private Button btnCrWithdraw2;
    private TextBox txtCrWithdrawnBy2;
    private Button btnBrWithdraw1;
    private TextBox txtBrWithdrawnBy1;
    private TextBox txtCrWithdrawnBy1;
    private Label label20;
    private Button btnCrWithdraw1;

    public eSignConsent()
    {
      this.loanData = Session.LoanData;
      this.session = Session.DefaultInstance;
      this.InitializeComponent();
      this.FormClosing += new FormClosingEventHandler(this.formClose);
      this.session.LoanDataMgr.SyncESignConsentData();
      this.loadBorrowerPairs();
      this.loadNBOs();
      this.getLocks(this.panel8);
      this.applyPolicyPersonaSettings();
    }

    private void applyPolicyPersonaSettings()
    {
      try
      {
        bool flag1 = false;
        bool flag2;
        if ("Enabled" != Session.SessionObjects.GetCompanySettingFromCache("Policies", "AllowEConsentWithdrawal"))
        {
          flag2 = false;
        }
        else
        {
          bool flag3 = this.session.UserInfo.IsSuperAdministrator();
          flag1 = !this.session.LoanDataMgr.LockLoanWithExclusive(false);
          if (flag3 && !flag1)
            return;
          flag2 = ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).CheckPermission(AclFeature.ToolsTab_DT_WithdrawEConsent, this.session.UserID);
        }
        if (!(!flag2 | flag1))
          return;
        Panel panel8 = this.panel8;
        foreach (Control control1 in (ArrangedElementCollection) this.panel8.Controls)
        {
          if (control1 is Panel)
          {
            foreach (Control control2 in (ArrangedElementCollection) control1.Controls)
            {
              if (control2 is Button && control2.Name.ToLower().Contains("withdraw"))
              {
                if (!flag2)
                  control2.Visible = false;
                else
                  control2.Enabled = false;
              }
              if (control2 is Panel)
              {
                foreach (Control control3 in (ArrangedElementCollection) control2.Controls)
                {
                  if (control3 is Button && control3.Name.ToLower().Contains("withdraw"))
                  {
                    if (!flag2)
                      control3.Visible = false;
                    else
                      control3.Enabled = false;
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(eSignConsent.sw, nameof (eSignConsent), TraceLevel.Error, ex.ToString());
      }
    }

    private void getLocks(Panel pnl)
    {
      foreach (Control control in (ArrangedElementCollection) pnl.Controls)
      {
        if (control is Panel)
          this.getLocks((Panel) control);
        if (control.Tag != null && ((string) control.Tag).Length > 4)
        {
          this.locks.Add(((string) control.Tag).Split('_')[1], (this.loanData.IsLocked(((string) control.Tag).Split('_')[1]) ? 1 : 0) != 0);
          this.lockValues.Add(((string) control.Tag).Split('_')[1], this.loanData.GetField(((string) control.Tag).Split('_')[1]));
        }
      }
    }

    private void loadBorrowerPairs()
    {
      int num1 = 1;
      int num2 = 0;
      this.pairs = this.loanData.GetBorrowerPairs();
      foreach (BorrowerPair pair in this.pairs)
      {
        Control control1 = this.panel8.Controls["panel" + (object) num1];
        control1.Controls["txtBrName" + (object) num1].Text = pair.Borrower.FirstName + " " + pair.Borrower.LastName;
        control1.Controls["txtBrEmail" + (object) num1].Text = this.loanData.GetSimpleField("1240", pair);
        Control control2 = control1.Controls["pnlCr" + (object) num1];
        Control control3 = control2.Controls["txtCrName" + (object) num1];
        control3.Text = pair.CoBorrower.FirstName + " " + pair.CoBorrower.LastName;
        if (control3.Text.Trim() == "")
        {
          control2.Visible = false;
          control2.Parent.Size = new Size(control2.Parent.Size.Width, control2.Parent.Size.Height - 26);
        }
        else
        {
          control2.Controls["txtCrEmail" + (object) num1].Text = this.loanData.GetSimpleField("1268", pair);
          ++num2;
        }
        ++num1;
        ++num2;
      }
      foreach (Control control4 in (ArrangedElementCollection) this.panel8.Controls)
      {
        if (control4 is Panel)
        {
          foreach (Control control5 in (ArrangedElementCollection) control4.Controls)
          {
            if (control5.Tag != null)
            {
              if (((string) control5.Tag).Length == 4)
              {
                control5.Text = this.loanData.GetField((string) control5.Tag);
                if (((IEnumerable<object>) new string[12]
                {
                  "3985",
                  "3989",
                  "3993",
                  "3997",
                  "4024",
                  "4028",
                  "4032",
                  "4036",
                  "4040",
                  "4044",
                  "4048",
                  "4052"
                }).Contains<object>(control5.Tag) && control5.Text == "//")
                  control5.Text = "";
              }
              else if (this.loanData.IsLocked(((string) control5.Tag).Split('_')[1]))
                this.lock_Clicked((object) control5, (EventArgs) null);
            }
            if (control5 is Panel)
            {
              foreach (Control control6 in (ArrangedElementCollection) control5.Controls)
              {
                if (control6.Tag != null)
                {
                  if (((string) control6.Tag).Length == 4)
                  {
                    control6.Text = this.loanData.GetField((string) control6.Tag);
                    if (((IEnumerable<object>) new string[12]
                    {
                      "3985",
                      "3989",
                      "3993",
                      "3997",
                      "4024",
                      "4028",
                      "4032",
                      "4036",
                      "4040",
                      "4044",
                      "4048",
                      "4052"
                    }).Contains<object>(control6.Tag) && control6.Text == "//")
                      control6.Text = "";
                  }
                  else if (this.loanData.IsLocked(((string) control6.Tag).Split('_')[1]))
                    this.lock_Clicked((object) control6, (EventArgs) null);
                }
              }
            }
          }
        }
      }
      for (int index = num1; index <= 6; ++index)
        this.panel8.Controls["panel" + (object) index].Visible = false;
      if (num2 <= 12)
      {
        int num3 = 25 * Math.Max(0, 12 - num2) + 40;
        this.groupContainer1.Size = new Size(this.groupContainer1.Width, 430 - num3);
        this.ClientSize = new Size(this.ClientSize.Width, 518 - num3 - 30);
      }
      this.txtConsentDate.Text = this.loanData.GetField("3983");
    }

    private void loadConsentDate()
    {
      DateTime dateTime = new DateTime();
      bool flag = true;
      for (int index = 1; index <= 6; ++index)
      {
        Control control1 = this.panel8.Controls["panel" + (object) index];
        Control control2 = control1.Controls["cmbBrStatus" + (object) index];
        if (control2.Visible)
        {
          if (control2.Text == "Accepted")
          {
            flag = true;
          }
          else
          {
            flag = false;
            break;
          }
        }
        Control control3 = control1.Controls["pnlCr" + (object) index].Controls["cmbCrStatus" + (object) index];
        if (control3.Visible)
        {
          if (control3.Text == "Accepted")
          {
            flag = true;
          }
          else
          {
            flag = false;
            break;
          }
        }
      }
      if (flag && this.IsEveryNBOAccepted)
      {
        for (int index = 1; index <= 6; ++index)
        {
          Control control = this.panel8.Controls["panel" + (object) index];
          DateTime date1 = Utils.ParseDate((object) control.Controls["txtBrDate" + (object) index].Text);
          if (date1 > DateTime.MinValue && date1 > dateTime)
            dateTime = date1;
          DateTime date2 = Utils.ParseDate((object) control.Controls["pnlCr" + (object) index].Controls["txtCrDate" + (object) index].Text);
          if (date2 > DateTime.MinValue && date2 > dateTime)
            dateTime = date2;
        }
        if (this.latestNBOAcceptedDate > DateTime.MinValue && this.latestNBOAcceptedDate > dateTime)
          dateTime = this.latestNBOAcceptedDate;
      }
      if (dateTime == DateTime.MinValue)
        this.txtConsentDate.Text = "";
      else
        this.txtConsentDate.Text = dateTime.ToString("d");
    }

    private void loadNBOs()
    {
      List<int> linkedVestingIdxList = this.loanData.GetNBOLinkedVestingIdxList();
      int num1 = 0;
      if (linkedVestingIdxList.Count<int>() == 0)
        return;
      linkedVestingIdxList.Reverse();
      foreach (int num2 in linkedVestingIdxList)
      {
        string str1 = "NBOC";
        string formattedIdx = num2.ToString("00");
        Panel child = this.constructNBOPanel(num1, formattedIdx);
        string str2 = this.loanData.GetField(str1 + formattedIdx + "01") + " " + this.loanData.GetField(str1 + formattedIdx + "02");
        string str3 = this.loanData.GetField(str1 + formattedIdx + "03") + " " + this.loanData.GetField(str1 + formattedIdx + "04");
        child.Controls[1].Text = str2.Trim() + " " + str3.Trim();
        child.Controls[2].Text = this.loanData.GetField(str1 + formattedIdx + "11");
        child.Controls[5].Text = this.loanData.GetField(str1 + formattedIdx + "19");
        child.Controls[3].Text = this.loanData.GetField(str1 + formattedIdx + "20");
        string field1 = this.loanData.GetField(str1 + formattedIdx + "18");
        child.Controls[6].Text = field1 == "//" ? "" : field1;
        DateTime date = Utils.ParseDate((object) field1);
        if (date > DateTime.MinValue && date > this.latestNBOAcceptedDate)
          this.latestNBOAcceptedDate = date;
        child.Controls[7].Text = this.loanData.GetField(str1 + formattedIdx + "39");
        string field2 = this.loanData.GetField(str1 + formattedIdx + "17");
        int num3 = 0;
        switch (field2)
        {
          case "Accepted":
            num3 = 1;
            break;
          case "Rejected":
            num3 = 2;
            break;
          case "Pending":
            num3 = 3;
            break;
        }
        ((ListControl) child.Controls[4]).SelectedIndex = num3;
        if (this.IsEveryNBOAccepted && field2 != "Accepted")
          this.IsEveryNBOAccepted = false;
        this.panel8.Controls.Add((Control) child);
        this.panel8.Controls.SetChildIndex((Control) child, num1);
        ++num1;
      }
      if (!this.IsEveryNBOAccepted)
        this.latestNBOAcceptedDate = new DateTime();
      this.panel8.Controls.SetChildIndex((Control) this.panel6, num1);
      this.panel8.Controls.SetChildIndex((Control) this.panel5, num1 + 1);
      this.panel8.Controls.SetChildIndex((Control) this.panel4, num1 + 2);
      this.panel8.Controls.SetChildIndex((Control) this.panel3, num1 + 3);
      this.panel8.Controls.SetChildIndex((Control) this.panel2, num1 + 4);
      this.panel8.Controls.SetChildIndex((Control) this.panel1, num1 + 5);
      this.panel8.Size = new Size(this.panel8.Size.Width, this.panel8.Size.Height + 25 * num1);
      Size size1 = this.groupContainer1.Size;
      if (size1.Height + 25 * num1 > 390)
      {
        this.groupContainer1.AutoScroll = true;
        GroupContainer groupContainer1 = this.groupContainer1;
        size1 = this.groupContainer1.Size;
        Size size2 = new Size(size1.Width, 390);
        groupContainer1.Size = size2;
        size1 = this.ClientSize;
        this.ClientSize = new Size(size1.Width, 448);
      }
      else
      {
        GroupContainer groupContainer1 = this.groupContainer1;
        size1 = this.groupContainer1.Size;
        int width1 = size1.Width;
        size1 = this.groupContainer1.Size;
        int height1 = size1.Height + 30 * num1;
        Size size3 = new Size(width1, height1);
        groupContainer1.Size = size3;
        size1 = this.ClientSize;
        int width2 = size1.Width;
        size1 = this.ClientSize;
        int height2 = size1.Height + 30 * num1;
        this.ClientSize = new Size(width2, height2);
      }
    }

    private Panel constructNBOPanel(int count, string formattedIdx)
    {
      Panel panel = new Panel();
      Label label = new Label();
      label.AutoSize = true;
      label.Location = new Point(3, 7);
      label.Name = "label" + formattedIdx;
      label.Size = new Size(58, 13);
      label.Text = "Non-Borrowing Owner";
      TextBox textBox1 = new TextBox();
      textBox1.Enabled = false;
      textBox1.Location = new Point(135, 4);
      textBox1.Name = "txtnboName" + formattedIdx;
      textBox1.Size = new Size(149, 20);
      TextBox textBox2 = new TextBox();
      textBox2.Enabled = false;
      textBox2.Location = new Point(288, 4);
      textBox2.Name = "txtnboEmail" + formattedIdx;
      textBox2.Size = new Size(149, 20);
      TextBox textBox3 = new TextBox();
      textBox3.Enabled = false;
      textBox3.Location = new Point(813, 3);
      textBox3.Name = "txtnboSource" + formattedIdx;
      textBox3.Size = new Size(115, 20);
      ComboBox comboBox = new ComboBox();
      comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBox.Enabled = false;
      comboBox.FormattingEnabled = true;
      comboBox.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      comboBox.Location = new Point(442, 3);
      comboBox.Name = "cmbnboStatus" + formattedIdx;
      comboBox.Size = new Size(115, 21);
      TextBox textBox4 = new TextBox();
      textBox4.Enabled = false;
      textBox4.Location = new Point(690, 3);
      textBox4.Name = "txtnboIPAddress" + formattedIdx;
      textBox4.Size = new Size(115, 20);
      TextBox textBox5 = new TextBox();
      textBox5.Enabled = false;
      textBox5.Location = new Point(565, 4);
      textBox5.Name = "txtnboDate" + formattedIdx;
      textBox5.Size = new Size(115, 20);
      TextBox textBox6 = new TextBox();
      textBox6.Enabled = false;
      textBox6.Location = new Point(936, 3);
      textBox6.Name = "txtnboWithdrawnBy" + formattedIdx;
      textBox6.Size = new Size(115, 20);
      Button button = new Button();
      button.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      button.Location = new Point(1060, 3);
      button.Name = "btnnboWithdraw" + formattedIdx;
      button.Size = new Size(120, 20);
      button.Text = "Withdraw Consent";
      button.UseVisualStyleBackColor = true;
      button.Click += new EventHandler(this.btnWithdraw_Click);
      panel.Controls.Add((Control) label);
      panel.Controls.Add((Control) textBox1);
      panel.Controls.Add((Control) textBox2);
      panel.Controls.Add((Control) textBox3);
      panel.Controls.Add((Control) comboBox);
      panel.Controls.Add((Control) textBox4);
      panel.Controls.Add((Control) textBox5);
      panel.Controls.Add((Control) textBox6);
      panel.Controls.Add((Control) button);
      panel.Dock = DockStyle.Top;
      panel.Location = new Point(0, 300 + 25 * count);
      panel.Name = "nboPanel" + formattedIdx;
      panel.Size = new Size(1201, 25);
      return panel;
    }

    private void btnWithdraw_Click(object sender, EventArgs e)
    {
      Button button = (Button) sender;
      string name = button.Name;
      if (name.Contains("btnnbo"))
      {
        this.withdraw_nbo((Panel) button.Parent, name.Substring(14));
      }
      else
      {
        string bOrC = "Br";
        string index = name.Substring(name.Length - 1);
        Panel control = (Panel) this.panel8.Controls["panel" + index];
        if (name.Contains("Cr"))
        {
          control = (Panel) control.Controls["pnlCr" + index];
          bOrC = "Cr";
        }
        this.withdraw_borrowerCoborrower(control, index, bOrC);
      }
    }

    private void withdraw_borrowerCoborrower(Panel parentPanel, string index, string bOrC)
    {
      Control control1 = parentPanel.Controls["cmb" + bOrC + "Status" + index];
      if (control1.Text == "Rejected")
        return;
      string str1;
      switch (bOrC)
      {
        case "Br":
          str1 = "b";
          break;
        case "Cr":
          str1 = "c";
          break;
        default:
          str1 = "";
          break;
      }
      string key = str1 + index;
      string str2 = string.Empty;
      if (eSignConsent.borrowerCoborrower_recipientIDFields.ContainsKey(key))
        str2 = this.loanData.GetSimpleField(eSignConsent.borrowerCoborrower_recipientIDFields[key]);
      if (string.IsNullOrEmpty(str2))
        return;
      Control control2 = parentPanel.Controls["txt" + bOrC + "Date" + index];
      Control control3 = parentPanel.Controls["txt" + bOrC + "Source" + index];
      Control control4 = parentPanel.Controls["txt" + bOrC + "IPAddress" + index];
      Control control5 = parentPanel.Controls["txt" + bOrC + "WithdrawnBy" + index];
      Control control6 = parentPanel.Controls["txt" + bOrC + "Name" + index];
      Control control7 = parentPanel.Controls["txt" + bOrC + "Email" + index];
      control1.Text = "Rejected";
      Control control8 = control2;
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      string str3 = dateTime.ToString("MM/dd/yyyy");
      control8.Text = str3;
      control3.Text = "SmartClient";
      control4.Text = "";
      control5.Text = Session.UserID;
      this.loanData.SetField((string) control1.Tag, control1.Text);
      this.loanData.SetField((string) control2.Tag, control2.Text);
      this.loanData.SetField((string) control3.Tag, control3.Text);
      this.loanData.SetField((string) control4.Tag, control4.Text);
      this.loanData.SetField((string) control5.Tag, control5.Text);
      this.eConsentsToBeWithdrawn.Add(key, new string[3]
      {
        str2,
        control6.Text,
        control7.Text
      });
    }

    private void withdraw_nbo(Panel parentPanel, string formattedIndex)
    {
      Control control1 = parentPanel.Controls["cmbnboStatus" + formattedIndex];
      if (control1.Text == "Rejected")
        return;
      string str1 = "nboc" + formattedIndex + "40";
      string simpleField = this.loanData.GetSimpleField(str1);
      if (string.IsNullOrEmpty(simpleField))
        return;
      Control control2 = parentPanel.Controls["txtnboDate" + formattedIndex];
      Control control3 = parentPanel.Controls["txtnboSource" + formattedIndex];
      Control control4 = parentPanel.Controls["txtnboIPAddress" + formattedIndex];
      Control control5 = parentPanel.Controls["txtnboWithdrawnBy" + formattedIndex];
      Control control6 = parentPanel.Controls["txtnboName" + formattedIndex];
      Control control7 = parentPanel.Controls["txtnboEmail" + formattedIndex];
      control1.Text = "Rejected";
      Control control8 = control2;
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      string str2 = dateTime.ToString("MM/dd/yyyy");
      control8.Text = str2;
      control3.Text = "SmartClient";
      control4.Text = "";
      control5.Text = Session.UserID;
      this.loanData.SetField("NBOC" + formattedIndex + "17", control1.Text);
      this.loanData.SetField("NBOC" + formattedIndex + "18", control2.Text);
      this.loanData.SetField("NBOC" + formattedIndex + "20", control3.Text);
      this.loanData.SetField("NBOC" + formattedIndex + "19", control4.Text);
      this.loanData.SetField("NBOC" + formattedIndex + "39", control5.Text);
      this.eConsentsToBeWithdrawn.Add(str1, new string[3]
      {
        simpleField,
        control6.Text,
        control7.Text
      });
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.loanData.Calculator != null)
        this.loanData.Calculator.LoadeSignConsentDate();
      if (!EDeliveryLoanSync.Withdraw_eConsents(this.session.LoanDataMgr, this.eConsentsToBeWithdrawn))
      {
        int num = (int) MessageBox.Show("Not all requests to withdraw eConsent have been committed successfully.");
      }
      this.DialogResult = DialogResult.OK;
    }

    private void lock_Clicked(object sender, EventArgs e)
    {
      FieldLockButton fieldLockButton = (FieldLockButton) sender;
      string id = fieldLockButton.Tag.ToString().Split('_')[1];
      fieldLockButton.Locked = !fieldLockButton.Locked;
      string key = !fieldLockButton.Name.Contains("Status") ? "txt" + fieldLockButton.Name.Substring(4) : "cmb" + fieldLockButton.Name.Substring(4);
      Panel control1 = (Panel) this.panel8.Controls["panel" + fieldLockButton.Name.Substring(fieldLockButton.Name.Length - 1)];
      if (key.Contains("Cr"))
        control1 = (Panel) control1.Controls["pnlCr" + fieldLockButton.Name.Substring(fieldLockButton.Name.Length - 1)];
      Control control2 = control1.Controls[key];
      control2.Enabled = fieldLockButton.Locked;
      if (fieldLockButton.Name.Contains("Date"))
        control1.Controls["cl" + fieldLockButton.Name.Substring(4)].Enabled = fieldLockButton.Locked;
      if (e == null)
        return;
      if (fieldLockButton.Locked)
      {
        this.loanData.AddLock(id);
      }
      else
      {
        this.loanData.RemoveLock(id);
        control2.Text = this.loanData.GetField(id);
        if (!((IEnumerable<object>) new string[12]
        {
          "3985",
          "3989",
          "3993",
          "3997",
          "4024",
          "4028",
          "4032",
          "4036",
          "4040",
          "4044",
          "4048",
          "4052"
        }).Contains<object>(control2.Tag) || !(control2.Text == "//"))
          return;
        control2.Text = "";
      }
    }

    private void DateField_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      try
      {
        if (textBox.Text == "")
          return;
        this.loanData.GetFieldDefinition(textBox.Tag.ToString()).ValidateFormat(textBox.Text);
      }
      catch (Exception ex)
      {
        Tracing.Log(eSignConsent.sw, nameof (eSignConsent), TraceLevel.Error, ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + textBox.Text + "' is invalid for field '" + textBox.Tag + "'. Date format is invalid.");
        textBox.Text = "";
        textBox.Focus();
      }
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      LoanDataMgr loanDataMgr = this.loanData.LinkSyncType == LinkSyncType.ConstructionLinked ? this.session.LoanDataMgr.LinkedLoan : this.session.LoanDataMgr;
      BinaryObject binaryObject = (BinaryObject) null;
      if (this.loanData.eConsentType == eConsentTypes.FullexternaleConsent)
        binaryObject = loanDataMgr.GetSupportingData("FullExternalEConsent.pdf");
      else if (this.session.LoanDataMgr.IsPlatformLoan())
      {
        binaryObject = new EDeliveryRestClient(loanDataMgr).GetPackageGroupConsentPdf().Result;
      }
      else
      {
        string consentPdf = loanDataMgr.GetConsentPDF();
        if (!string.IsNullOrEmpty(consentPdf))
          binaryObject = new BinaryObject(Convert.FromBase64String(consentPdf));
      }
      if (binaryObject == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Consent form is not available at this time.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string nameWithExtension = SystemSettings.GetTempFileNameWithExtension("pdf");
        binaryObject.Write(nameWithExtension);
        using (PdfPreviewDialog pdfPreviewDialog = new PdfPreviewDialog(nameWithExtension, true, true, false))
        {
          int num2 = (int) pdfPreviewDialog.ShowDialog((IWin32Window) Form.ActiveForm);
        }
      }
    }

    private void IPAddress_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      try
      {
        if (textBox.Text == "")
          return;
        IPAddress.Parse(textBox.Text);
      }
      catch (Exception ex)
      {
        Tracing.Log(eSignConsent.sw, nameof (eSignConsent), TraceLevel.Error, ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + textBox.Text + "' is invalid for field '" + textBox.Tag + "'. IP Address format is invalid.");
        textBox.Text = "";
        textBox.Focus();
      }
    }

    private void formClose(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().RefreshContents();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (eSignConsent));
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.groupContainer1 = new GroupContainer();
      this.panel8 = new Panel();
      this.panel6 = new Panel();
      this.btnBrWithdraw6 = new Button();
      this.txtBrWithdrawnBy6 = new TextBox();
      this.pnlCr6 = new Panel();
      this.btnCrWithdraw6 = new Button();
      this.txtCrWithdrawnBy6 = new TextBox();
      this.label18 = new Label();
      this.clCrDate6 = new CalendarButton();
      this.txtCrDate6 = new TextBox();
      this.txtCrName6 = new TextBox();
      this.lBtnCrDate6 = new FieldLockButton();
      this.lBtnCrIPAddress6 = new FieldLockButton();
      this.txtCrSource6 = new TextBox();
      this.lBtnCrStatus6 = new FieldLockButton();
      this.txtCrEmail6 = new TextBox();
      this.txtCrIPAddress6 = new TextBox();
      this.cmbCrStatus6 = new ComboBox();
      this.lBtnCrSource6 = new FieldLockButton();
      this.label19 = new Label();
      this.txtBrName6 = new TextBox();
      this.txtBrEmail6 = new TextBox();
      this.txtBrSource6 = new TextBox();
      this.cmbBrStatus6 = new ComboBox();
      this.lBtnBrStatus6 = new FieldLockButton();
      this.lBtnBrSource6 = new FieldLockButton();
      this.txtBrIPAddress6 = new TextBox();
      this.lBtnBrDate6 = new FieldLockButton();
      this.lBtnBrIPAddress6 = new FieldLockButton();
      this.clBrDate6 = new CalendarButton();
      this.txtBrDate6 = new TextBox();
      this.panel5 = new Panel();
      this.btnBrWithdraw5 = new Button();
      this.txtBrWithdrawnBy5 = new TextBox();
      this.pnlCr5 = new Panel();
      this.btnCrWithdraw5 = new Button();
      this.txtCrWithdrawnBy5 = new TextBox();
      this.label16 = new Label();
      this.txtCrSource5 = new TextBox();
      this.lBtnCrSource5 = new FieldLockButton();
      this.txtCrName5 = new TextBox();
      this.txtCrIPAddress5 = new TextBox();
      this.lBtnCrIPAddress5 = new FieldLockButton();
      this.txtCrEmail5 = new TextBox();
      this.txtCrDate5 = new TextBox();
      this.clCrDate5 = new CalendarButton();
      this.lBtnCrDate5 = new FieldLockButton();
      this.cmbCrStatus5 = new ComboBox();
      this.lBtnCrStatus5 = new FieldLockButton();
      this.label17 = new Label();
      this.txtBrName5 = new TextBox();
      this.txtBrEmail5 = new TextBox();
      this.cmbBrStatus5 = new ComboBox();
      this.lBtnBrStatus5 = new FieldLockButton();
      this.lBtnBrDate5 = new FieldLockButton();
      this.clBrDate5 = new CalendarButton();
      this.txtBrDate5 = new TextBox();
      this.lBtnBrIPAddress5 = new FieldLockButton();
      this.txtBrIPAddress5 = new TextBox();
      this.lBtnBrSource5 = new FieldLockButton();
      this.txtBrSource5 = new TextBox();
      this.panel4 = new Panel();
      this.btnBrWithdraw4 = new Button();
      this.txtBrWithdrawnBy4 = new TextBox();
      this.label15 = new Label();
      this.txtBrName4 = new TextBox();
      this.pnlCr4 = new Panel();
      this.btnCrWithdraw4 = new Button();
      this.txtCrWithdrawnBy4 = new TextBox();
      this.label14 = new Label();
      this.txtCrSource4 = new TextBox();
      this.lBtnCrSource4 = new FieldLockButton();
      this.txtCrName4 = new TextBox();
      this.txtCrIPAddress4 = new TextBox();
      this.lBtnCrIPAddress4 = new FieldLockButton();
      this.txtCrEmail4 = new TextBox();
      this.txtCrDate4 = new TextBox();
      this.clCrDate4 = new CalendarButton();
      this.lBtnCrDate4 = new FieldLockButton();
      this.cmbCrStatus4 = new ComboBox();
      this.lBtnCrStatus4 = new FieldLockButton();
      this.txtBrEmail4 = new TextBox();
      this.cmbBrStatus4 = new ComboBox();
      this.lBtnBrStatus4 = new FieldLockButton();
      this.lBtnBrDate4 = new FieldLockButton();
      this.clBrDate4 = new CalendarButton();
      this.txtBrDate4 = new TextBox();
      this.lBtnBrIPAddress4 = new FieldLockButton();
      this.txtBrIPAddress4 = new TextBox();
      this.lBtnBrSource4 = new FieldLockButton();
      this.txtBrSource4 = new TextBox();
      this.panel3 = new Panel();
      this.btnBrWithdraw3 = new Button();
      this.txtBrWithdrawnBy3 = new TextBox();
      this.label13 = new Label();
      this.pnlCr3 = new Panel();
      this.btnCrWithdraw3 = new Button();
      this.txtCrWithdrawnBy3 = new TextBox();
      this.label12 = new Label();
      this.txtCrSource3 = new TextBox();
      this.lBtnCrSource3 = new FieldLockButton();
      this.txtCrName3 = new TextBox();
      this.txtCrIPAddress3 = new TextBox();
      this.lBtnCrIPAddress3 = new FieldLockButton();
      this.txtCrEmail3 = new TextBox();
      this.txtCrDate3 = new TextBox();
      this.clCrDate3 = new CalendarButton();
      this.lBtnCrDate3 = new FieldLockButton();
      this.cmbCrStatus3 = new ComboBox();
      this.lBtnCrStatus3 = new FieldLockButton();
      this.txtBrName3 = new TextBox();
      this.txtBrEmail3 = new TextBox();
      this.cmbBrStatus3 = new ComboBox();
      this.lBtnBrStatus3 = new FieldLockButton();
      this.lBtnBrDate3 = new FieldLockButton();
      this.clBrDate3 = new CalendarButton();
      this.txtBrDate3 = new TextBox();
      this.lBtnBrIPAddress3 = new FieldLockButton();
      this.txtBrIPAddress3 = new TextBox();
      this.lBtnBrSource3 = new FieldLockButton();
      this.txtBrSource3 = new TextBox();
      this.panel2 = new Panel();
      this.btnBrWithdraw2 = new Button();
      this.txtBrWithdrawnBy2 = new TextBox();
      this.pnlCr2 = new Panel();
      this.btnCrWithdraw2 = new Button();
      this.txtCrWithdrawnBy2 = new TextBox();
      this.label10 = new Label();
      this.txtCrSource2 = new TextBox();
      this.lBtnCrSource2 = new FieldLockButton();
      this.txtCrName2 = new TextBox();
      this.txtCrIPAddress2 = new TextBox();
      this.lBtnCrIPAddress2 = new FieldLockButton();
      this.txtCrEmail2 = new TextBox();
      this.txtCrDate2 = new TextBox();
      this.clCrDate2 = new CalendarButton();
      this.lBtnCrDate2 = new FieldLockButton();
      this.cmbCrStatus2 = new ComboBox();
      this.lBtnCrStatus2 = new FieldLockButton();
      this.label11 = new Label();
      this.txtBrName2 = new TextBox();
      this.txtBrEmail2 = new TextBox();
      this.cmbBrStatus2 = new ComboBox();
      this.lBtnBrStatus2 = new FieldLockButton();
      this.lBtnBrDate2 = new FieldLockButton();
      this.clBrDate2 = new CalendarButton();
      this.txtBrDate2 = new TextBox();
      this.lBtnBrIPAddress2 = new FieldLockButton();
      this.txtBrIPAddress2 = new TextBox();
      this.lBtnBrSource2 = new FieldLockButton();
      this.txtBrSource2 = new TextBox();
      this.panel1 = new Panel();
      this.btnBrWithdraw1 = new Button();
      this.txtBrWithdrawnBy1 = new TextBox();
      this.pnlCr1 = new Panel();
      this.btnCrWithdraw1 = new Button();
      this.txtCrWithdrawnBy1 = new TextBox();
      this.label9 = new Label();
      this.txtCrSource1 = new TextBox();
      this.lBtnCrSource1 = new FieldLockButton();
      this.txtCrName1 = new TextBox();
      this.txtCrIPAddress1 = new TextBox();
      this.lBtnCrIPAddress1 = new FieldLockButton();
      this.txtCrEmail1 = new TextBox();
      this.txtCrDate1 = new TextBox();
      this.clCrDate1 = new CalendarButton();
      this.lBtnCrDate1 = new FieldLockButton();
      this.cmbCrStatus1 = new ComboBox();
      this.lBtnCrStatus1 = new FieldLockButton();
      this.label8 = new Label();
      this.txtBrName1 = new TextBox();
      this.txtBrEmail1 = new TextBox();
      this.cmbBrStatus1 = new ComboBox();
      this.lBtnBrStatus1 = new FieldLockButton();
      this.lBtnBrDate1 = new FieldLockButton();
      this.clBrDate1 = new CalendarButton();
      this.txtBrDate1 = new TextBox();
      this.lBtnBrIPAddress1 = new FieldLockButton();
      this.txtBrIPAddress1 = new TextBox();
      this.lBtnBrSource1 = new FieldLockButton();
      this.txtBrSource1 = new TextBox();
      this.panel7 = new Panel();
      this.label20 = new Label();
      this.label1 = new Label();
      this.txtConsentDate = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.viewFormLink = new System.Windows.Forms.LinkLabel();
      this.groupContainer1.SuspendLayout();
      this.panel8.SuspendLayout();
      this.panel6.SuspendLayout();
      this.pnlCr6.SuspendLayout();
      ((ISupportInitialize) this.clCrDate6).BeginInit();
      ((ISupportInitialize) this.clBrDate6).BeginInit();
      this.panel5.SuspendLayout();
      this.pnlCr5.SuspendLayout();
      ((ISupportInitialize) this.clCrDate5).BeginInit();
      ((ISupportInitialize) this.clBrDate5).BeginInit();
      this.panel4.SuspendLayout();
      this.pnlCr4.SuspendLayout();
      ((ISupportInitialize) this.clCrDate4).BeginInit();
      ((ISupportInitialize) this.clBrDate4).BeginInit();
      this.panel3.SuspendLayout();
      this.pnlCr3.SuspendLayout();
      ((ISupportInitialize) this.clCrDate3).BeginInit();
      ((ISupportInitialize) this.clBrDate3).BeginInit();
      this.panel2.SuspendLayout();
      this.pnlCr2.SuspendLayout();
      ((ISupportInitialize) this.clCrDate2).BeginInit();
      ((ISupportInitialize) this.clBrDate2).BeginInit();
      this.panel1.SuspendLayout();
      this.pnlCr1.SuspendLayout();
      ((ISupportInitialize) this.clCrDate1).BeginInit();
      ((ISupportInitialize) this.clBrDate1).BeginInit();
      this.panel7.SuspendLayout();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(1034, 401);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 130;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(1115, 401);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 132;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.groupContainer1.Controls.Add((Control) this.panel8);
      this.groupContainer1.Controls.Add((Control) this.panel7);
      this.groupContainer1.Controls.Add((Control) this.viewFormLink);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(1201, 362);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "eConsent Details";
      this.panel8.Controls.Add((Control) this.panel6);
      this.panel8.Controls.Add((Control) this.panel5);
      this.panel8.Controls.Add((Control) this.panel4);
      this.panel8.Controls.Add((Control) this.panel3);
      this.panel8.Controls.Add((Control) this.panel2);
      this.panel8.Controls.Add((Control) this.panel1);
      this.panel8.Dock = DockStyle.Top;
      this.panel8.Location = new Point(1, 83);
      this.panel8.Name = "panel8";
      this.panel8.Size = new Size(1199, 305);
      this.panel8.TabIndex = 133;
      this.panel6.Controls.Add((Control) this.btnBrWithdraw6);
      this.panel6.Controls.Add((Control) this.txtBrWithdrawnBy6);
      this.panel6.Controls.Add((Control) this.pnlCr6);
      this.panel6.Controls.Add((Control) this.label19);
      this.panel6.Controls.Add((Control) this.txtBrName6);
      this.panel6.Controls.Add((Control) this.txtBrEmail6);
      this.panel6.Controls.Add((Control) this.txtBrSource6);
      this.panel6.Controls.Add((Control) this.cmbBrStatus6);
      this.panel6.Controls.Add((Control) this.lBtnBrStatus6);
      this.panel6.Controls.Add((Control) this.lBtnBrSource6);
      this.panel6.Controls.Add((Control) this.txtBrIPAddress6);
      this.panel6.Controls.Add((Control) this.lBtnBrDate6);
      this.panel6.Controls.Add((Control) this.lBtnBrIPAddress6);
      this.panel6.Controls.Add((Control) this.clBrDate6);
      this.panel6.Controls.Add((Control) this.txtBrDate6);
      this.panel6.Dock = DockStyle.Top;
      this.panel6.Location = new Point(0, 250);
      this.panel6.Name = "panel6";
      this.panel6.Size = new Size(1199, 50);
      this.panel6.TabIndex = 6;
      this.btnBrWithdraw6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnBrWithdraw6.Location = new Point(1060, 3);
      this.btnBrWithdraw6.Name = "btnBrWithdraw6";
      this.btnBrWithdraw6.Size = new Size(120, 20);
      this.btnBrWithdraw6.TabIndex = 0;
      this.btnBrWithdraw6.Text = "Withdraw Consent";
      this.btnBrWithdraw6.UseVisualStyleBackColor = true;
      this.btnBrWithdraw6.Click += new EventHandler(this.btnWithdraw_Click);
      this.txtBrWithdrawnBy6.Enabled = false;
      this.txtBrWithdrawnBy6.Location = new Point(936, 3);
      this.txtBrWithdrawnBy6.Name = "txtBrWithdrawnBy6";
      this.txtBrWithdrawnBy6.Size = new Size(115, 20);
      this.txtBrWithdrawnBy6.TabIndex = 147;
      this.txtBrWithdrawnBy6.Tag = (object) "4999";
      this.pnlCr6.Controls.Add((Control) this.btnCrWithdraw6);
      this.pnlCr6.Controls.Add((Control) this.txtCrWithdrawnBy6);
      this.pnlCr6.Controls.Add((Control) this.label18);
      this.pnlCr6.Controls.Add((Control) this.clCrDate6);
      this.pnlCr6.Controls.Add((Control) this.txtCrDate6);
      this.pnlCr6.Controls.Add((Control) this.txtCrName6);
      this.pnlCr6.Controls.Add((Control) this.lBtnCrDate6);
      this.pnlCr6.Controls.Add((Control) this.lBtnCrIPAddress6);
      this.pnlCr6.Controls.Add((Control) this.txtCrSource6);
      this.pnlCr6.Controls.Add((Control) this.lBtnCrStatus6);
      this.pnlCr6.Controls.Add((Control) this.txtCrEmail6);
      this.pnlCr6.Controls.Add((Control) this.txtCrIPAddress6);
      this.pnlCr6.Controls.Add((Control) this.cmbCrStatus6);
      this.pnlCr6.Controls.Add((Control) this.lBtnCrSource6);
      this.pnlCr6.Location = new Point(0, 25);
      this.pnlCr6.Name = "pnlCr6";
      this.pnlCr6.Size = new Size(1196, 27);
      this.pnlCr6.TabIndex = 133;
      this.btnCrWithdraw6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnCrWithdraw6.Location = new Point(1060, 3);
      this.btnCrWithdraw6.Name = "btnCrWithdraw6";
      this.btnCrWithdraw6.Size = new Size(120, 20);
      this.btnCrWithdraw6.TabIndex = 0;
      this.btnCrWithdraw6.Text = "Withdraw Consent";
      this.btnCrWithdraw6.UseVisualStyleBackColor = true;
      this.btnCrWithdraw6.Click += new EventHandler(this.btnWithdraw_Click);
      this.txtCrWithdrawnBy6.Enabled = false;
      this.txtCrWithdrawnBy6.Location = new Point(936, 3);
      this.txtCrWithdrawnBy6.Name = "txtCrWithdrawnBy6";
      this.txtCrWithdrawnBy6.Size = new Size(115, 20);
      this.txtCrWithdrawnBy6.TabIndex = 169;
      this.txtCrWithdrawnBy6.Tag = (object) "5000";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(3, 7);
      this.label18.Name = "label18";
      this.label18.Size = new Size(74, 13);
      this.label18.TabIndex = 157;
      this.label18.Text = "Co-Borrower 6";
      this.clCrDate6.DateControl = (Control) this.txtCrDate6;
      this.clCrDate6.Enabled = false;
      ((IconButton) this.clCrDate6).Image = (Image) componentResourceManager.GetObject("clCrDate6.Image");
      this.clCrDate6.Location = new Point(686, 3);
      this.clCrDate6.MouseDownImage = (Image) null;
      this.clCrDate6.Name = "clCrDate6";
      this.clCrDate6.Size = new Size(16, 16);
      this.clCrDate6.SizeMode = PictureBoxSizeMode.AutoSize;
      this.clCrDate6.TabIndex = 170;
      this.clCrDate6.TabStop = false;
      this.clCrDate6.Visible = false;
      this.txtCrDate6.Enabled = false;
      this.txtCrDate6.Location = new Point(565, 3);
      this.txtCrDate6.Name = "txtCrDate6";
      this.txtCrDate6.Size = new Size(115, 20);
      this.txtCrDate6.TabIndex = 125;
      this.txtCrDate6.Tag = (object) "4052";
      this.txtCrDate6.Leave += new EventHandler(this.DateField_Leave);
      this.txtCrName6.Enabled = false;
      this.txtCrName6.Location = new Point(135, 4);
      this.txtCrName6.Name = "txtCrName6";
      this.txtCrName6.Size = new Size(149, 20);
      this.txtCrName6.TabIndex = 120;
      this.lBtnCrDate6.Location = new Point(592, 5);
      this.lBtnCrDate6.LockedStateToolTip = "Use Default Value";
      this.lBtnCrDate6.MaximumSize = new Size(16, 17);
      this.lBtnCrDate6.MinimumSize = new Size(16, 17);
      this.lBtnCrDate6.Name = "lBtnCrDate6";
      this.lBtnCrDate6.Size = new Size(16, 17);
      this.lBtnCrDate6.TabIndex = 124;
      this.lBtnCrDate6.Tag = (object) "LOCKBUTTON_4052";
      this.lBtnCrDate6.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrDate6.Visible = false;
      this.lBtnCrDate6.Click += new EventHandler(this.lock_Clicked);
      this.lBtnCrIPAddress6.Location = new Point(765, 4);
      this.lBtnCrIPAddress6.LockedStateToolTip = "Use Default Value";
      this.lBtnCrIPAddress6.MaximumSize = new Size(16, 17);
      this.lBtnCrIPAddress6.MinimumSize = new Size(16, 17);
      this.lBtnCrIPAddress6.Name = "lBtnCrIPAddress6";
      this.lBtnCrIPAddress6.Size = new Size(16, 17);
      this.lBtnCrIPAddress6.TabIndex = 126;
      this.lBtnCrIPAddress6.Tag = (object) "LOCKBUTTON_4053";
      this.lBtnCrIPAddress6.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrIPAddress6.Visible = false;
      this.lBtnCrIPAddress6.Click += new EventHandler(this.lock_Clicked);
      this.txtCrSource6.Enabled = false;
      this.txtCrSource6.Location = new Point(813, 3);
      this.txtCrSource6.Name = "txtCrSource6";
      this.txtCrSource6.Size = new Size(115, 20);
      this.txtCrSource6.TabIndex = 129;
      this.txtCrSource6.Tag = (object) "4054";
      this.lBtnCrStatus6.Location = new Point(420, 5);
      this.lBtnCrStatus6.LockedStateToolTip = "Use Default Value";
      this.lBtnCrStatus6.MaximumSize = new Size(16, 17);
      this.lBtnCrStatus6.MinimumSize = new Size(16, 17);
      this.lBtnCrStatus6.Name = "lBtnCrStatus6";
      this.lBtnCrStatus6.Size = new Size(16, 17);
      this.lBtnCrStatus6.TabIndex = 122;
      this.lBtnCrStatus6.Tag = (object) "LOCKBUTTON_4051";
      this.lBtnCrStatus6.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrStatus6.Visible = false;
      this.lBtnCrStatus6.Click += new EventHandler(this.lock_Clicked);
      this.txtCrEmail6.Enabled = false;
      this.txtCrEmail6.Location = new Point(288, 4);
      this.txtCrEmail6.Name = "txtCrEmail6";
      this.txtCrEmail6.Size = new Size(149, 20);
      this.txtCrEmail6.TabIndex = 121;
      this.txtCrIPAddress6.Enabled = false;
      this.txtCrIPAddress6.Location = new Point(690, 3);
      this.txtCrIPAddress6.Name = "txtCrIPAddress6";
      this.txtCrIPAddress6.Size = new Size(115, 20);
      this.txtCrIPAddress6.TabIndex = (int) sbyte.MaxValue;
      this.txtCrIPAddress6.Tag = (object) "4053";
      this.txtCrIPAddress6.Leave += new EventHandler(this.IPAddress_Leave);
      this.cmbCrStatus6.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCrStatus6.Enabled = false;
      this.cmbCrStatus6.FormattingEnabled = true;
      this.cmbCrStatus6.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      this.cmbCrStatus6.Location = new Point(442, 3);
      this.cmbCrStatus6.Name = "cmbCrStatus6";
      this.cmbCrStatus6.Size = new Size(115, 21);
      this.cmbCrStatus6.TabIndex = 123;
      this.cmbCrStatus6.Tag = (object) "4051";
      this.lBtnCrSource6.Location = new Point(914, 4);
      this.lBtnCrSource6.LockedStateToolTip = "Use Default Value";
      this.lBtnCrSource6.MaximumSize = new Size(16, 17);
      this.lBtnCrSource6.MinimumSize = new Size(16, 17);
      this.lBtnCrSource6.Name = "lBtnCrSource6";
      this.lBtnCrSource6.Size = new Size(16, 17);
      this.lBtnCrSource6.TabIndex = 128;
      this.lBtnCrSource6.Tag = (object) "LOCKBUTTON_4054";
      this.lBtnCrSource6.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrSource6.Visible = false;
      this.lBtnCrSource6.Click += new EventHandler(this.lock_Clicked);
      this.label19.AutoSize = true;
      this.label19.Location = new Point(3, 7);
      this.label19.Name = "label19";
      this.label19.Size = new Size(58, 13);
      this.label19.TabIndex = 156;
      this.label19.Text = "Borrower 6";
      this.txtBrName6.Enabled = false;
      this.txtBrName6.Location = new Point(135, 4);
      this.txtBrName6.Name = "txtBrName6";
      this.txtBrName6.Size = new Size(149, 20);
      this.txtBrName6.TabIndex = 110;
      this.txtBrEmail6.Enabled = false;
      this.txtBrEmail6.Location = new Point(288, 4);
      this.txtBrEmail6.Name = "txtBrEmail6";
      this.txtBrEmail6.Size = new Size(149, 20);
      this.txtBrEmail6.TabIndex = 111;
      this.txtBrSource6.Enabled = false;
      this.txtBrSource6.Location = new Point(813, 3);
      this.txtBrSource6.Name = "txtBrSource6";
      this.txtBrSource6.Size = new Size(115, 20);
      this.txtBrSource6.TabIndex = 119;
      this.txtBrSource6.Tag = (object) "4050";
      this.cmbBrStatus6.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrStatus6.Enabled = false;
      this.cmbBrStatus6.FormattingEnabled = true;
      this.cmbBrStatus6.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      this.cmbBrStatus6.Location = new Point(442, 3);
      this.cmbBrStatus6.Name = "cmbBrStatus6";
      this.cmbBrStatus6.Size = new Size(115, 21);
      this.cmbBrStatus6.TabIndex = 113;
      this.cmbBrStatus6.Tag = (object) "4047";
      this.lBtnBrStatus6.Location = new Point(420, 4);
      this.lBtnBrStatus6.LockedStateToolTip = "Use Default Value";
      this.lBtnBrStatus6.MaximumSize = new Size(16, 17);
      this.lBtnBrStatus6.MinimumSize = new Size(16, 17);
      this.lBtnBrStatus6.Name = "lBtnBrStatus6";
      this.lBtnBrStatus6.Size = new Size(16, 17);
      this.lBtnBrStatus6.TabIndex = 112;
      this.lBtnBrStatus6.Tag = (object) "LOCKBUTTON_4047";
      this.lBtnBrStatus6.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrStatus6.Visible = false;
      this.lBtnBrStatus6.Click += new EventHandler(this.lock_Clicked);
      this.lBtnBrSource6.Location = new Point(914, 3);
      this.lBtnBrSource6.LockedStateToolTip = "Use Default Value";
      this.lBtnBrSource6.MaximumSize = new Size(16, 17);
      this.lBtnBrSource6.MinimumSize = new Size(16, 17);
      this.lBtnBrSource6.Name = "lBtnBrSource6";
      this.lBtnBrSource6.Size = new Size(16, 17);
      this.lBtnBrSource6.TabIndex = 118;
      this.lBtnBrSource6.Tag = (object) "LOCKBUTTON_4050";
      this.lBtnBrSource6.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrSource6.Visible = false;
      this.lBtnBrSource6.Click += new EventHandler(this.lock_Clicked);
      this.txtBrIPAddress6.Enabled = false;
      this.txtBrIPAddress6.Location = new Point(690, 3);
      this.txtBrIPAddress6.Name = "txtBrIPAddress6";
      this.txtBrIPAddress6.Size = new Size(115, 20);
      this.txtBrIPAddress6.TabIndex = 117;
      this.txtBrIPAddress6.Tag = (object) "4049";
      this.txtBrIPAddress6.Leave += new EventHandler(this.IPAddress_Leave);
      this.lBtnBrDate6.Location = new Point(592, 4);
      this.lBtnBrDate6.LockedStateToolTip = "Use Default Value";
      this.lBtnBrDate6.MaximumSize = new Size(16, 17);
      this.lBtnBrDate6.MinimumSize = new Size(16, 17);
      this.lBtnBrDate6.Name = "lBtnBrDate6";
      this.lBtnBrDate6.Size = new Size(16, 17);
      this.lBtnBrDate6.TabIndex = 114;
      this.lBtnBrDate6.Tag = (object) "LOCKBUTTON_4048";
      this.lBtnBrDate6.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrDate6.Visible = false;
      this.lBtnBrDate6.Click += new EventHandler(this.lock_Clicked);
      this.lBtnBrIPAddress6.Location = new Point(765, 3);
      this.lBtnBrIPAddress6.LockedStateToolTip = "Use Default Value";
      this.lBtnBrIPAddress6.MaximumSize = new Size(16, 17);
      this.lBtnBrIPAddress6.MinimumSize = new Size(16, 17);
      this.lBtnBrIPAddress6.Name = "lBtnBrIPAddress6";
      this.lBtnBrIPAddress6.Size = new Size(16, 17);
      this.lBtnBrIPAddress6.TabIndex = 116;
      this.lBtnBrIPAddress6.Tag = (object) "LOCKBUTTON_4049";
      this.lBtnBrIPAddress6.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrIPAddress6.Visible = false;
      this.lBtnBrIPAddress6.Click += new EventHandler(this.lock_Clicked);
      this.clBrDate6.DateControl = (Control) this.txtBrDate6;
      this.clBrDate6.Enabled = false;
      ((IconButton) this.clBrDate6).Image = (Image) componentResourceManager.GetObject("clBrDate6.Image");
      this.clBrDate6.Location = new Point(686, 4);
      this.clBrDate6.MouseDownImage = (Image) null;
      this.clBrDate6.Name = "clBrDate6";
      this.clBrDate6.Size = new Size(16, 16);
      this.clBrDate6.SizeMode = PictureBoxSizeMode.AutoSize;
      this.clBrDate6.TabIndex = 168;
      this.clBrDate6.TabStop = false;
      this.clBrDate6.Visible = false;
      this.txtBrDate6.Enabled = false;
      this.txtBrDate6.Location = new Point(565, 4);
      this.txtBrDate6.Name = "txtBrDate6";
      this.txtBrDate6.Size = new Size(115, 20);
      this.txtBrDate6.TabIndex = 115;
      this.txtBrDate6.Tag = (object) "4048";
      this.txtBrDate6.Leave += new EventHandler(this.DateField_Leave);
      this.panel5.Controls.Add((Control) this.btnBrWithdraw5);
      this.panel5.Controls.Add((Control) this.txtBrWithdrawnBy5);
      this.panel5.Controls.Add((Control) this.pnlCr5);
      this.panel5.Controls.Add((Control) this.label17);
      this.panel5.Controls.Add((Control) this.txtBrName5);
      this.panel5.Controls.Add((Control) this.txtBrEmail5);
      this.panel5.Controls.Add((Control) this.cmbBrStatus5);
      this.panel5.Controls.Add((Control) this.lBtnBrStatus5);
      this.panel5.Controls.Add((Control) this.lBtnBrDate5);
      this.panel5.Controls.Add((Control) this.clBrDate5);
      this.panel5.Controls.Add((Control) this.txtBrDate5);
      this.panel5.Controls.Add((Control) this.lBtnBrIPAddress5);
      this.panel5.Controls.Add((Control) this.txtBrIPAddress5);
      this.panel5.Controls.Add((Control) this.lBtnBrSource5);
      this.panel5.Controls.Add((Control) this.txtBrSource5);
      this.panel5.Dock = DockStyle.Top;
      this.panel5.Location = new Point(0, 200);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(1199, 50);
      this.panel5.TabIndex = 5;
      this.btnBrWithdraw5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnBrWithdraw5.Location = new Point(1060, 3);
      this.btnBrWithdraw5.Name = "btnBrWithdraw5";
      this.btnBrWithdraw5.Size = new Size(120, 20);
      this.btnBrWithdraw5.TabIndex = 0;
      this.btnBrWithdraw5.Text = "Withdraw Consent";
      this.btnBrWithdraw5.UseVisualStyleBackColor = true;
      this.btnBrWithdraw5.Click += new EventHandler(this.btnWithdraw_Click);
      this.txtBrWithdrawnBy5.Enabled = false;
      this.txtBrWithdrawnBy5.Location = new Point(936, 3);
      this.txtBrWithdrawnBy5.Name = "txtBrWithdrawnBy5";
      this.txtBrWithdrawnBy5.Size = new Size(115, 20);
      this.txtBrWithdrawnBy5.TabIndex = 141;
      this.txtBrWithdrawnBy5.Tag = (object) "4997";
      this.pnlCr5.Controls.Add((Control) this.btnCrWithdraw5);
      this.pnlCr5.Controls.Add((Control) this.txtCrWithdrawnBy5);
      this.pnlCr5.Controls.Add((Control) this.label16);
      this.pnlCr5.Controls.Add((Control) this.txtCrSource5);
      this.pnlCr5.Controls.Add((Control) this.lBtnCrSource5);
      this.pnlCr5.Controls.Add((Control) this.txtCrName5);
      this.pnlCr5.Controls.Add((Control) this.txtCrIPAddress5);
      this.pnlCr5.Controls.Add((Control) this.lBtnCrIPAddress5);
      this.pnlCr5.Controls.Add((Control) this.txtCrEmail5);
      this.pnlCr5.Controls.Add((Control) this.txtCrDate5);
      this.pnlCr5.Controls.Add((Control) this.clCrDate5);
      this.pnlCr5.Controls.Add((Control) this.lBtnCrDate5);
      this.pnlCr5.Controls.Add((Control) this.cmbCrStatus5);
      this.pnlCr5.Controls.Add((Control) this.lBtnCrStatus5);
      this.pnlCr5.Location = new Point(0, 25);
      this.pnlCr5.Name = "pnlCr5";
      this.pnlCr5.Size = new Size(1196, 28);
      this.pnlCr5.TabIndex = 133;
      this.btnCrWithdraw5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnCrWithdraw5.Location = new Point(1060, 3);
      this.btnCrWithdraw5.Name = "btnCrWithdraw5";
      this.btnCrWithdraw5.Size = new Size(120, 20);
      this.btnCrWithdraw5.TabIndex = 0;
      this.btnCrWithdraw5.Text = "Withdraw Consent";
      this.btnCrWithdraw5.UseVisualStyleBackColor = true;
      this.btnCrWithdraw5.Click += new EventHandler(this.btnWithdraw_Click);
      this.txtCrWithdrawnBy5.Enabled = false;
      this.txtCrWithdrawnBy5.Location = new Point(936, 3);
      this.txtCrWithdrawnBy5.Name = "txtCrWithdrawnBy5";
      this.txtCrWithdrawnBy5.Size = new Size(115, 20);
      this.txtCrWithdrawnBy5.TabIndex = 145;
      this.txtCrWithdrawnBy5.Tag = (object) "4998";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(3, 7);
      this.label16.Name = "label16";
      this.label16.Size = new Size(74, 13);
      this.label16.TabIndex = 133;
      this.label16.Text = "Co-Borrower 5";
      this.txtCrSource5.Enabled = false;
      this.txtCrSource5.Location = new Point(814, 4);
      this.txtCrSource5.Name = "txtCrSource5";
      this.txtCrSource5.Size = new Size(115, 20);
      this.txtCrSource5.TabIndex = 109;
      this.txtCrSource5.Tag = (object) "4046";
      this.lBtnCrSource5.Location = new Point(914, 4);
      this.lBtnCrSource5.LockedStateToolTip = "Use Default Value";
      this.lBtnCrSource5.MaximumSize = new Size(16, 17);
      this.lBtnCrSource5.MinimumSize = new Size(16, 17);
      this.lBtnCrSource5.Name = "lBtnCrSource5";
      this.lBtnCrSource5.Size = new Size(16, 17);
      this.lBtnCrSource5.TabIndex = 108;
      this.lBtnCrSource5.Tag = (object) "LOCKBUTTON_4046";
      this.lBtnCrSource5.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrSource5.Visible = false;
      this.lBtnCrSource5.Click += new EventHandler(this.lock_Clicked);
      this.txtCrName5.Enabled = false;
      this.txtCrName5.Location = new Point(135, 4);
      this.txtCrName5.Name = "txtCrName5";
      this.txtCrName5.Size = new Size(149, 20);
      this.txtCrName5.TabIndex = 100;
      this.txtCrIPAddress5.Enabled = false;
      this.txtCrIPAddress5.Location = new Point(691, 4);
      this.txtCrIPAddress5.Name = "txtCrIPAddress5";
      this.txtCrIPAddress5.Size = new Size(115, 20);
      this.txtCrIPAddress5.TabIndex = 107;
      this.txtCrIPAddress5.Tag = (object) "4045";
      this.txtCrIPAddress5.Leave += new EventHandler(this.IPAddress_Leave);
      this.lBtnCrIPAddress5.Location = new Point(765, 4);
      this.lBtnCrIPAddress5.LockedStateToolTip = "Use Default Value";
      this.lBtnCrIPAddress5.MaximumSize = new Size(16, 17);
      this.lBtnCrIPAddress5.MinimumSize = new Size(16, 17);
      this.lBtnCrIPAddress5.Name = "lBtnCrIPAddress5";
      this.lBtnCrIPAddress5.Size = new Size(16, 17);
      this.lBtnCrIPAddress5.TabIndex = 106;
      this.lBtnCrIPAddress5.Tag = (object) "LOCKBUTTON_4045";
      this.lBtnCrIPAddress5.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrIPAddress5.Visible = false;
      this.lBtnCrIPAddress5.Click += new EventHandler(this.lock_Clicked);
      this.txtCrEmail5.Enabled = false;
      this.txtCrEmail5.Location = new Point(288, 4);
      this.txtCrEmail5.Name = "txtCrEmail5";
      this.txtCrEmail5.Size = new Size(149, 20);
      this.txtCrEmail5.TabIndex = 101;
      this.txtCrDate5.Enabled = false;
      this.txtCrDate5.Location = new Point(565, 3);
      this.txtCrDate5.Name = "txtCrDate5";
      this.txtCrDate5.Size = new Size(115, 20);
      this.txtCrDate5.TabIndex = 105;
      this.txtCrDate5.Tag = (object) "4044";
      this.txtCrDate5.Leave += new EventHandler(this.DateField_Leave);
      this.clCrDate5.DateControl = (Control) this.txtCrDate5;
      this.clCrDate5.Enabled = false;
      ((IconButton) this.clCrDate5).Image = (Image) componentResourceManager.GetObject("clCrDate5.Image");
      this.clCrDate5.Location = new Point(686, 3);
      this.clCrDate5.MouseDownImage = (Image) null;
      this.clCrDate5.Name = "clCrDate5";
      this.clCrDate5.Size = new Size(16, 16);
      this.clCrDate5.SizeMode = PictureBoxSizeMode.AutoSize;
      this.clCrDate5.TabIndex = 146;
      this.clCrDate5.TabStop = false;
      this.clCrDate5.Visible = false;
      this.lBtnCrDate5.Location = new Point(592, 5);
      this.lBtnCrDate5.LockedStateToolTip = "Use Default Value";
      this.lBtnCrDate5.MaximumSize = new Size(16, 17);
      this.lBtnCrDate5.MinimumSize = new Size(16, 17);
      this.lBtnCrDate5.Name = "lBtnCrDate5";
      this.lBtnCrDate5.Size = new Size(16, 17);
      this.lBtnCrDate5.TabIndex = 104;
      this.lBtnCrDate5.Tag = (object) "LOCKBUTTON_4044";
      this.lBtnCrDate5.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrDate5.Visible = false;
      this.lBtnCrDate5.Click += new EventHandler(this.lock_Clicked);
      this.cmbCrStatus5.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCrStatus5.Enabled = false;
      this.cmbCrStatus5.FormattingEnabled = true;
      this.cmbCrStatus5.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      this.cmbCrStatus5.Location = new Point(442, 3);
      this.cmbCrStatus5.Name = "cmbCrStatus5";
      this.cmbCrStatus5.Size = new Size(115, 21);
      this.cmbCrStatus5.TabIndex = 103;
      this.cmbCrStatus5.Tag = (object) "4043";
      this.lBtnCrStatus5.Location = new Point(420, 5);
      this.lBtnCrStatus5.LockedStateToolTip = "Use Default Value";
      this.lBtnCrStatus5.MaximumSize = new Size(16, 17);
      this.lBtnCrStatus5.MinimumSize = new Size(16, 17);
      this.lBtnCrStatus5.Name = "lBtnCrStatus5";
      this.lBtnCrStatus5.Size = new Size(16, 17);
      this.lBtnCrStatus5.TabIndex = 102;
      this.lBtnCrStatus5.Tag = (object) "LOCKBUTTON_4043";
      this.lBtnCrStatus5.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrStatus5.Visible = false;
      this.lBtnCrStatus5.Click += new EventHandler(this.lock_Clicked);
      this.label17.AutoSize = true;
      this.label17.Location = new Point(3, 7);
      this.label17.Name = "label17";
      this.label17.Size = new Size(58, 13);
      this.label17.TabIndex = 132;
      this.label17.Text = "Borrower 5";
      this.txtBrName5.Enabled = false;
      this.txtBrName5.Location = new Point(135, 4);
      this.txtBrName5.Name = "txtBrName5";
      this.txtBrName5.Size = new Size(149, 20);
      this.txtBrName5.TabIndex = 90;
      this.txtBrEmail5.Enabled = false;
      this.txtBrEmail5.Location = new Point(288, 4);
      this.txtBrEmail5.Name = "txtBrEmail5";
      this.txtBrEmail5.Size = new Size(149, 20);
      this.txtBrEmail5.TabIndex = 91;
      this.cmbBrStatus5.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrStatus5.Enabled = false;
      this.cmbBrStatus5.FormattingEnabled = true;
      this.cmbBrStatus5.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      this.cmbBrStatus5.Location = new Point(442, 3);
      this.cmbBrStatus5.Name = "cmbBrStatus5";
      this.cmbBrStatus5.Size = new Size(115, 21);
      this.cmbBrStatus5.TabIndex = 93;
      this.cmbBrStatus5.Tag = (object) "4039";
      this.lBtnBrStatus5.Location = new Point(420, 4);
      this.lBtnBrStatus5.LockedStateToolTip = "Use Default Value";
      this.lBtnBrStatus5.MaximumSize = new Size(16, 17);
      this.lBtnBrStatus5.MinimumSize = new Size(16, 17);
      this.lBtnBrStatus5.Name = "lBtnBrStatus5";
      this.lBtnBrStatus5.Size = new Size(16, 17);
      this.lBtnBrStatus5.TabIndex = 92;
      this.lBtnBrStatus5.Tag = (object) "LOCKBUTTON_4039";
      this.lBtnBrStatus5.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrStatus5.Visible = false;
      this.lBtnBrStatus5.Click += new EventHandler(this.lock_Clicked);
      this.lBtnBrDate5.Location = new Point(592, 4);
      this.lBtnBrDate5.LockedStateToolTip = "Use Default Value";
      this.lBtnBrDate5.MaximumSize = new Size(16, 17);
      this.lBtnBrDate5.MinimumSize = new Size(16, 17);
      this.lBtnBrDate5.Name = "lBtnBrDate5";
      this.lBtnBrDate5.Size = new Size(16, 17);
      this.lBtnBrDate5.TabIndex = 94;
      this.lBtnBrDate5.Tag = (object) "LOCKBUTTON_4040";
      this.lBtnBrDate5.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrDate5.Visible = false;
      this.lBtnBrDate5.Click += new EventHandler(this.lock_Clicked);
      this.clBrDate5.DateControl = (Control) this.txtBrDate5;
      this.clBrDate5.Enabled = false;
      ((IconButton) this.clBrDate5).Image = (Image) componentResourceManager.GetObject("clBrDate5.Image");
      this.clBrDate5.Location = new Point(686, 4);
      this.clBrDate5.MouseDownImage = (Image) null;
      this.clBrDate5.Name = "clBrDate5";
      this.clBrDate5.Size = new Size(16, 16);
      this.clBrDate5.SizeMode = PictureBoxSizeMode.AutoSize;
      this.clBrDate5.TabIndex = 144;
      this.clBrDate5.TabStop = false;
      this.clBrDate5.Visible = false;
      this.txtBrDate5.Enabled = false;
      this.txtBrDate5.Location = new Point(565, 4);
      this.txtBrDate5.Name = "txtBrDate5";
      this.txtBrDate5.Size = new Size(115, 20);
      this.txtBrDate5.TabIndex = 95;
      this.txtBrDate5.Tag = (object) "4040";
      this.txtBrDate5.Leave += new EventHandler(this.DateField_Leave);
      this.lBtnBrIPAddress5.Location = new Point(765, 3);
      this.lBtnBrIPAddress5.LockedStateToolTip = "Use Default Value";
      this.lBtnBrIPAddress5.MaximumSize = new Size(16, 17);
      this.lBtnBrIPAddress5.MinimumSize = new Size(16, 17);
      this.lBtnBrIPAddress5.Name = "lBtnBrIPAddress5";
      this.lBtnBrIPAddress5.Size = new Size(16, 17);
      this.lBtnBrIPAddress5.TabIndex = 96;
      this.lBtnBrIPAddress5.Tag = (object) "LOCKBUTTON_4041";
      this.lBtnBrIPAddress5.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrIPAddress5.Visible = false;
      this.lBtnBrIPAddress5.Click += new EventHandler(this.lock_Clicked);
      this.txtBrIPAddress5.Enabled = false;
      this.txtBrIPAddress5.Location = new Point(690, 3);
      this.txtBrIPAddress5.Name = "txtBrIPAddress5";
      this.txtBrIPAddress5.Size = new Size(115, 20);
      this.txtBrIPAddress5.TabIndex = 97;
      this.txtBrIPAddress5.Tag = (object) "4041";
      this.txtBrIPAddress5.Leave += new EventHandler(this.IPAddress_Leave);
      this.lBtnBrSource5.Location = new Point(914, 3);
      this.lBtnBrSource5.LockedStateToolTip = "Use Default Value";
      this.lBtnBrSource5.MaximumSize = new Size(16, 17);
      this.lBtnBrSource5.MinimumSize = new Size(16, 17);
      this.lBtnBrSource5.Name = "lBtnBrSource5";
      this.lBtnBrSource5.Size = new Size(16, 17);
      this.lBtnBrSource5.TabIndex = 98;
      this.lBtnBrSource5.Tag = (object) "LOCKBUTTON_4042";
      this.lBtnBrSource5.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrSource5.Visible = false;
      this.lBtnBrSource5.Click += new EventHandler(this.lock_Clicked);
      this.txtBrSource5.Enabled = false;
      this.txtBrSource5.Location = new Point(814, 3);
      this.txtBrSource5.Name = "txtBrSource5";
      this.txtBrSource5.Size = new Size(115, 20);
      this.txtBrSource5.TabIndex = 99;
      this.txtBrSource5.Tag = (object) "4042";
      this.panel4.Controls.Add((Control) this.btnBrWithdraw4);
      this.panel4.Controls.Add((Control) this.txtBrWithdrawnBy4);
      this.panel4.Controls.Add((Control) this.label15);
      this.panel4.Controls.Add((Control) this.txtBrName4);
      this.panel4.Controls.Add((Control) this.pnlCr4);
      this.panel4.Controls.Add((Control) this.txtBrEmail4);
      this.panel4.Controls.Add((Control) this.cmbBrStatus4);
      this.panel4.Controls.Add((Control) this.lBtnBrStatus4);
      this.panel4.Controls.Add((Control) this.lBtnBrDate4);
      this.panel4.Controls.Add((Control) this.clBrDate4);
      this.panel4.Controls.Add((Control) this.txtBrDate4);
      this.panel4.Controls.Add((Control) this.lBtnBrIPAddress4);
      this.panel4.Controls.Add((Control) this.txtBrIPAddress4);
      this.panel4.Controls.Add((Control) this.lBtnBrSource4);
      this.panel4.Controls.Add((Control) this.txtBrSource4);
      this.panel4.Dock = DockStyle.Top;
      this.panel4.Location = new Point(0, 150);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(1199, 50);
      this.panel4.TabIndex = 4;
      this.btnBrWithdraw4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnBrWithdraw4.Location = new Point(1060, 3);
      this.btnBrWithdraw4.Name = "btnBrWithdraw4";
      this.btnBrWithdraw4.Size = new Size(120, 20);
      this.btnBrWithdraw4.TabIndex = 0;
      this.btnBrWithdraw4.Text = "Withdraw Consent";
      this.btnBrWithdraw4.UseVisualStyleBackColor = true;
      this.btnBrWithdraw4.Click += new EventHandler(this.btnWithdraw_Click);
      this.txtBrWithdrawnBy4.Enabled = false;
      this.txtBrWithdrawnBy4.Location = new Point(936, 3);
      this.txtBrWithdrawnBy4.Name = "txtBrWithdrawnBy4";
      this.txtBrWithdrawnBy4.Size = new Size(115, 20);
      this.txtBrWithdrawnBy4.TabIndex = 139;
      this.txtBrWithdrawnBy4.Tag = (object) "4995";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(3, 7);
      this.label15.Name = "label15";
      this.label15.Size = new Size(58, 13);
      this.label15.TabIndex = 108;
      this.label15.Text = "Borrower 4";
      this.txtBrName4.Enabled = false;
      this.txtBrName4.Location = new Point(135, 4);
      this.txtBrName4.Name = "txtBrName4";
      this.txtBrName4.Size = new Size(149, 20);
      this.txtBrName4.TabIndex = 70;
      this.pnlCr4.Controls.Add((Control) this.btnCrWithdraw4);
      this.pnlCr4.Controls.Add((Control) this.txtCrWithdrawnBy4);
      this.pnlCr4.Controls.Add((Control) this.label14);
      this.pnlCr4.Controls.Add((Control) this.txtCrSource4);
      this.pnlCr4.Controls.Add((Control) this.lBtnCrSource4);
      this.pnlCr4.Controls.Add((Control) this.txtCrName4);
      this.pnlCr4.Controls.Add((Control) this.txtCrIPAddress4);
      this.pnlCr4.Controls.Add((Control) this.lBtnCrIPAddress4);
      this.pnlCr4.Controls.Add((Control) this.txtCrEmail4);
      this.pnlCr4.Controls.Add((Control) this.txtCrDate4);
      this.pnlCr4.Controls.Add((Control) this.clCrDate4);
      this.pnlCr4.Controls.Add((Control) this.lBtnCrDate4);
      this.pnlCr4.Controls.Add((Control) this.cmbCrStatus4);
      this.pnlCr4.Controls.Add((Control) this.lBtnCrStatus4);
      this.pnlCr4.Location = new Point(0, 25);
      this.pnlCr4.Name = "pnlCr4";
      this.pnlCr4.Size = new Size(1196, 28);
      this.pnlCr4.TabIndex = 133;
      this.btnCrWithdraw4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnCrWithdraw4.Location = new Point(1060, 3);
      this.btnCrWithdraw4.Name = "btnCrWithdraw4";
      this.btnCrWithdraw4.Size = new Size(120, 20);
      this.btnCrWithdraw4.TabIndex = 0;
      this.btnCrWithdraw4.Text = "Withdraw Consent";
      this.btnCrWithdraw4.UseVisualStyleBackColor = true;
      this.btnCrWithdraw4.Click += new EventHandler(this.btnWithdraw_Click);
      this.txtCrWithdrawnBy4.Enabled = false;
      this.txtCrWithdrawnBy4.Location = new Point(936, 3);
      this.txtCrWithdrawnBy4.Name = "txtCrWithdrawnBy4";
      this.txtCrWithdrawnBy4.Size = new Size(115, 20);
      this.txtCrWithdrawnBy4.TabIndex = 140;
      this.txtCrWithdrawnBy4.Tag = (object) "4996";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(3, 6);
      this.label14.Name = "label14";
      this.label14.Size = new Size(74, 13);
      this.label14.TabIndex = 109;
      this.label14.Text = "Co-Borrower 4";
      this.txtCrSource4.Enabled = false;
      this.txtCrSource4.Location = new Point(813, 4);
      this.txtCrSource4.Name = "txtCrSource4";
      this.txtCrSource4.Size = new Size(115, 20);
      this.txtCrSource4.TabIndex = 89;
      this.txtCrSource4.Tag = (object) "4038";
      this.lBtnCrSource4.Location = new Point(914, 4);
      this.lBtnCrSource4.LockedStateToolTip = "Use Default Value";
      this.lBtnCrSource4.MaximumSize = new Size(16, 17);
      this.lBtnCrSource4.MinimumSize = new Size(16, 17);
      this.lBtnCrSource4.Name = "lBtnCrSource4";
      this.lBtnCrSource4.Size = new Size(16, 17);
      this.lBtnCrSource4.TabIndex = 88;
      this.lBtnCrSource4.Tag = (object) "LOCKBUTTON_4038";
      this.lBtnCrSource4.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrSource4.Visible = false;
      this.lBtnCrSource4.Click += new EventHandler(this.lock_Clicked);
      this.txtCrName4.Enabled = false;
      this.txtCrName4.Location = new Point(135, 4);
      this.txtCrName4.Name = "txtCrName4";
      this.txtCrName4.Size = new Size(149, 20);
      this.txtCrName4.TabIndex = 80;
      this.txtCrIPAddress4.Enabled = false;
      this.txtCrIPAddress4.Location = new Point(690, 4);
      this.txtCrIPAddress4.Name = "txtCrIPAddress4";
      this.txtCrIPAddress4.Size = new Size(115, 20);
      this.txtCrIPAddress4.TabIndex = 87;
      this.txtCrIPAddress4.Tag = (object) "4037";
      this.txtCrIPAddress4.Leave += new EventHandler(this.IPAddress_Leave);
      this.lBtnCrIPAddress4.Location = new Point(765, 4);
      this.lBtnCrIPAddress4.LockedStateToolTip = "Use Default Value";
      this.lBtnCrIPAddress4.MaximumSize = new Size(16, 17);
      this.lBtnCrIPAddress4.MinimumSize = new Size(16, 17);
      this.lBtnCrIPAddress4.Name = "lBtnCrIPAddress4";
      this.lBtnCrIPAddress4.Size = new Size(16, 17);
      this.lBtnCrIPAddress4.TabIndex = 86;
      this.lBtnCrIPAddress4.Tag = (object) "LOCKBUTTON_4037";
      this.lBtnCrIPAddress4.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrIPAddress4.Visible = false;
      this.lBtnCrIPAddress4.Click += new EventHandler(this.lock_Clicked);
      this.txtCrEmail4.Enabled = false;
      this.txtCrEmail4.Location = new Point(288, 4);
      this.txtCrEmail4.Name = "txtCrEmail4";
      this.txtCrEmail4.Size = new Size(149, 20);
      this.txtCrEmail4.TabIndex = 81;
      this.txtCrDate4.Enabled = false;
      this.txtCrDate4.Location = new Point(565, 3);
      this.txtCrDate4.Name = "txtCrDate4";
      this.txtCrDate4.Size = new Size(115, 20);
      this.txtCrDate4.TabIndex = 85;
      this.txtCrDate4.Tag = (object) "4036";
      this.txtCrDate4.Leave += new EventHandler(this.DateField_Leave);
      this.clCrDate4.DateControl = (Control) this.txtCrDate4;
      this.clCrDate4.Enabled = false;
      ((IconButton) this.clCrDate4).Image = (Image) componentResourceManager.GetObject("clCrDate4.Image");
      this.clCrDate4.Location = new Point(686, 3);
      this.clCrDate4.MouseDownImage = (Image) null;
      this.clCrDate4.Name = "clCrDate4";
      this.clCrDate4.Size = new Size(16, 16);
      this.clCrDate4.SizeMode = PictureBoxSizeMode.AutoSize;
      this.clCrDate4.TabIndex = 122;
      this.clCrDate4.TabStop = false;
      this.clCrDate4.Visible = false;
      this.lBtnCrDate4.Location = new Point(592, 5);
      this.lBtnCrDate4.LockedStateToolTip = "Use Default Value";
      this.lBtnCrDate4.MaximumSize = new Size(16, 17);
      this.lBtnCrDate4.MinimumSize = new Size(16, 17);
      this.lBtnCrDate4.Name = "lBtnCrDate4";
      this.lBtnCrDate4.Size = new Size(16, 17);
      this.lBtnCrDate4.TabIndex = 84;
      this.lBtnCrDate4.Tag = (object) "LOCKBUTTON_4036";
      this.lBtnCrDate4.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrDate4.Visible = false;
      this.lBtnCrDate4.Click += new EventHandler(this.lock_Clicked);
      this.cmbCrStatus4.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCrStatus4.Enabled = false;
      this.cmbCrStatus4.FormattingEnabled = true;
      this.cmbCrStatus4.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      this.cmbCrStatus4.Location = new Point(442, 3);
      this.cmbCrStatus4.Name = "cmbCrStatus4";
      this.cmbCrStatus4.Size = new Size(115, 21);
      this.cmbCrStatus4.TabIndex = 83;
      this.cmbCrStatus4.Tag = (object) "4035";
      this.lBtnCrStatus4.Location = new Point(420, 5);
      this.lBtnCrStatus4.LockedStateToolTip = "Use Default Value";
      this.lBtnCrStatus4.MaximumSize = new Size(16, 17);
      this.lBtnCrStatus4.MinimumSize = new Size(16, 17);
      this.lBtnCrStatus4.Name = "lBtnCrStatus4";
      this.lBtnCrStatus4.Size = new Size(16, 17);
      this.lBtnCrStatus4.TabIndex = 82;
      this.lBtnCrStatus4.Tag = (object) "LOCKBUTTON_4035";
      this.lBtnCrStatus4.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrStatus4.Visible = false;
      this.lBtnCrStatus4.Click += new EventHandler(this.lock_Clicked);
      this.txtBrEmail4.Enabled = false;
      this.txtBrEmail4.Location = new Point(288, 4);
      this.txtBrEmail4.Name = "txtBrEmail4";
      this.txtBrEmail4.Size = new Size(149, 20);
      this.txtBrEmail4.TabIndex = 71;
      this.cmbBrStatus4.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrStatus4.Enabled = false;
      this.cmbBrStatus4.FormattingEnabled = true;
      this.cmbBrStatus4.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      this.cmbBrStatus4.Location = new Point(442, 3);
      this.cmbBrStatus4.Name = "cmbBrStatus4";
      this.cmbBrStatus4.Size = new Size(115, 21);
      this.cmbBrStatus4.TabIndex = 73;
      this.cmbBrStatus4.Tag = (object) "4031";
      this.lBtnBrStatus4.Location = new Point(420, 4);
      this.lBtnBrStatus4.LockedStateToolTip = "Use Default Value";
      this.lBtnBrStatus4.MaximumSize = new Size(16, 17);
      this.lBtnBrStatus4.MinimumSize = new Size(16, 17);
      this.lBtnBrStatus4.Name = "lBtnBrStatus4";
      this.lBtnBrStatus4.Size = new Size(16, 17);
      this.lBtnBrStatus4.TabIndex = 72;
      this.lBtnBrStatus4.Tag = (object) "LOCKBUTTON_4031";
      this.lBtnBrStatus4.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrStatus4.Visible = false;
      this.lBtnBrStatus4.Click += new EventHandler(this.lock_Clicked);
      this.lBtnBrDate4.Location = new Point(592, 4);
      this.lBtnBrDate4.LockedStateToolTip = "Use Default Value";
      this.lBtnBrDate4.MaximumSize = new Size(16, 17);
      this.lBtnBrDate4.MinimumSize = new Size(16, 17);
      this.lBtnBrDate4.Name = "lBtnBrDate4";
      this.lBtnBrDate4.Size = new Size(16, 17);
      this.lBtnBrDate4.TabIndex = 74;
      this.lBtnBrDate4.Tag = (object) "LOCKBUTTON_4032";
      this.lBtnBrDate4.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrDate4.Visible = false;
      this.lBtnBrDate4.Click += new EventHandler(this.lock_Clicked);
      this.clBrDate4.DateControl = (Control) this.txtBrDate4;
      this.clBrDate4.Enabled = false;
      ((IconButton) this.clBrDate4).Image = (Image) componentResourceManager.GetObject("clBrDate4.Image");
      this.clBrDate4.Location = new Point(686, 4);
      this.clBrDate4.MouseDownImage = (Image) null;
      this.clBrDate4.Name = "clBrDate4";
      this.clBrDate4.Size = new Size(16, 16);
      this.clBrDate4.SizeMode = PictureBoxSizeMode.AutoSize;
      this.clBrDate4.TabIndex = 120;
      this.clBrDate4.TabStop = false;
      this.clBrDate4.Visible = false;
      this.txtBrDate4.Enabled = false;
      this.txtBrDate4.Location = new Point(565, 4);
      this.txtBrDate4.Name = "txtBrDate4";
      this.txtBrDate4.Size = new Size(115, 20);
      this.txtBrDate4.TabIndex = 75;
      this.txtBrDate4.Tag = (object) "4032";
      this.txtBrDate4.Leave += new EventHandler(this.DateField_Leave);
      this.lBtnBrIPAddress4.Location = new Point(765, 3);
      this.lBtnBrIPAddress4.LockedStateToolTip = "Use Default Value";
      this.lBtnBrIPAddress4.MaximumSize = new Size(16, 17);
      this.lBtnBrIPAddress4.MinimumSize = new Size(16, 17);
      this.lBtnBrIPAddress4.Name = "lBtnBrIPAddress4";
      this.lBtnBrIPAddress4.Size = new Size(16, 17);
      this.lBtnBrIPAddress4.TabIndex = 76;
      this.lBtnBrIPAddress4.Tag = (object) "LOCKBUTTON_4033";
      this.lBtnBrIPAddress4.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrIPAddress4.Visible = false;
      this.lBtnBrIPAddress4.Click += new EventHandler(this.lock_Clicked);
      this.txtBrIPAddress4.Enabled = false;
      this.txtBrIPAddress4.Location = new Point(690, 3);
      this.txtBrIPAddress4.Name = "txtBrIPAddress4";
      this.txtBrIPAddress4.Size = new Size(115, 20);
      this.txtBrIPAddress4.TabIndex = 77;
      this.txtBrIPAddress4.Tag = (object) "4033";
      this.txtBrIPAddress4.Leave += new EventHandler(this.IPAddress_Leave);
      this.lBtnBrSource4.Location = new Point(914, 3);
      this.lBtnBrSource4.LockedStateToolTip = "Use Default Value";
      this.lBtnBrSource4.MaximumSize = new Size(16, 17);
      this.lBtnBrSource4.MinimumSize = new Size(16, 17);
      this.lBtnBrSource4.Name = "lBtnBrSource4";
      this.lBtnBrSource4.Size = new Size(16, 17);
      this.lBtnBrSource4.TabIndex = 78;
      this.lBtnBrSource4.Tag = (object) "LOCKBUTTON_4034";
      this.lBtnBrSource4.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrSource4.Visible = false;
      this.lBtnBrSource4.Click += new EventHandler(this.lock_Clicked);
      this.txtBrSource4.Enabled = false;
      this.txtBrSource4.Location = new Point(813, 3);
      this.txtBrSource4.Name = "txtBrSource4";
      this.txtBrSource4.Size = new Size(115, 20);
      this.txtBrSource4.TabIndex = 79;
      this.txtBrSource4.Tag = (object) "4034";
      this.panel3.Controls.Add((Control) this.btnBrWithdraw3);
      this.panel3.Controls.Add((Control) this.txtBrWithdrawnBy3);
      this.panel3.Controls.Add((Control) this.label13);
      this.panel3.Controls.Add((Control) this.pnlCr3);
      this.panel3.Controls.Add((Control) this.txtBrName3);
      this.panel3.Controls.Add((Control) this.txtBrEmail3);
      this.panel3.Controls.Add((Control) this.cmbBrStatus3);
      this.panel3.Controls.Add((Control) this.lBtnBrStatus3);
      this.panel3.Controls.Add((Control) this.lBtnBrDate3);
      this.panel3.Controls.Add((Control) this.clBrDate3);
      this.panel3.Controls.Add((Control) this.txtBrDate3);
      this.panel3.Controls.Add((Control) this.lBtnBrIPAddress3);
      this.panel3.Controls.Add((Control) this.txtBrIPAddress3);
      this.panel3.Controls.Add((Control) this.lBtnBrSource3);
      this.panel3.Controls.Add((Control) this.txtBrSource3);
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(0, 100);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(1199, 50);
      this.panel3.TabIndex = 3;
      this.btnBrWithdraw3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnBrWithdraw3.Location = new Point(1060, 3);
      this.btnBrWithdraw3.Name = "btnBrWithdraw3";
      this.btnBrWithdraw3.Size = new Size(120, 20);
      this.btnBrWithdraw3.TabIndex = 0;
      this.btnBrWithdraw3.Text = "Withdraw Consent";
      this.btnBrWithdraw3.UseVisualStyleBackColor = true;
      this.btnBrWithdraw3.Click += new EventHandler(this.btnWithdraw_Click);
      this.txtBrWithdrawnBy3.Enabled = false;
      this.txtBrWithdrawnBy3.Location = new Point(936, 3);
      this.txtBrWithdrawnBy3.Name = "txtBrWithdrawnBy3";
      this.txtBrWithdrawnBy3.Size = new Size(115, 20);
      this.txtBrWithdrawnBy3.TabIndex = 137;
      this.txtBrWithdrawnBy3.Tag = (object) "4993";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(3, 7);
      this.label13.Name = "label13";
      this.label13.Size = new Size(58, 13);
      this.label13.TabIndex = 84;
      this.label13.Text = "Borrower 3";
      this.pnlCr3.Controls.Add((Control) this.btnCrWithdraw3);
      this.pnlCr3.Controls.Add((Control) this.txtCrWithdrawnBy3);
      this.pnlCr3.Controls.Add((Control) this.label12);
      this.pnlCr3.Controls.Add((Control) this.txtCrSource3);
      this.pnlCr3.Controls.Add((Control) this.lBtnCrSource3);
      this.pnlCr3.Controls.Add((Control) this.txtCrName3);
      this.pnlCr3.Controls.Add((Control) this.txtCrIPAddress3);
      this.pnlCr3.Controls.Add((Control) this.lBtnCrIPAddress3);
      this.pnlCr3.Controls.Add((Control) this.txtCrEmail3);
      this.pnlCr3.Controls.Add((Control) this.txtCrDate3);
      this.pnlCr3.Controls.Add((Control) this.clCrDate3);
      this.pnlCr3.Controls.Add((Control) this.lBtnCrDate3);
      this.pnlCr3.Controls.Add((Control) this.cmbCrStatus3);
      this.pnlCr3.Controls.Add((Control) this.lBtnCrStatus3);
      this.pnlCr3.Location = new Point(0, 25);
      this.pnlCr3.Name = "pnlCr3";
      this.pnlCr3.Size = new Size(1196, 28);
      this.pnlCr3.TabIndex = 133;
      this.btnCrWithdraw3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnCrWithdraw3.Location = new Point(1060, 3);
      this.btnCrWithdraw3.Name = "btnCrWithdraw3";
      this.btnCrWithdraw3.Size = new Size(120, 20);
      this.btnCrWithdraw3.TabIndex = 0;
      this.btnCrWithdraw3.Text = "Withdraw Consent";
      this.btnCrWithdraw3.UseVisualStyleBackColor = true;
      this.btnCrWithdraw3.Click += new EventHandler(this.btnWithdraw_Click);
      this.txtCrWithdrawnBy3.Enabled = false;
      this.txtCrWithdrawnBy3.Location = new Point(936, 3);
      this.txtCrWithdrawnBy3.Name = "txtCrWithdrawnBy3";
      this.txtCrWithdrawnBy3.Size = new Size(115, 20);
      this.txtCrWithdrawnBy3.TabIndex = 138;
      this.txtCrWithdrawnBy3.Tag = (object) "4994";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(3, 7);
      this.label12.Name = "label12";
      this.label12.Size = new Size(74, 13);
      this.label12.TabIndex = 85;
      this.label12.Text = "Co-Borrower 3";
      this.txtCrSource3.Enabled = false;
      this.txtCrSource3.Location = new Point(813, 4);
      this.txtCrSource3.Name = "txtCrSource3";
      this.txtCrSource3.Size = new Size(115, 20);
      this.txtCrSource3.TabIndex = 69;
      this.txtCrSource3.Tag = (object) "4030";
      this.lBtnCrSource3.Location = new Point(914, 4);
      this.lBtnCrSource3.LockedStateToolTip = "Use Default Value";
      this.lBtnCrSource3.MaximumSize = new Size(16, 17);
      this.lBtnCrSource3.MinimumSize = new Size(16, 17);
      this.lBtnCrSource3.Name = "lBtnCrSource3";
      this.lBtnCrSource3.Size = new Size(16, 17);
      this.lBtnCrSource3.TabIndex = 68;
      this.lBtnCrSource3.Tag = (object) "LOCKBUTTON_4030";
      this.lBtnCrSource3.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrSource3.Visible = false;
      this.lBtnCrSource3.Click += new EventHandler(this.lock_Clicked);
      this.txtCrName3.Enabled = false;
      this.txtCrName3.Location = new Point(135, 4);
      this.txtCrName3.Name = "txtCrName3";
      this.txtCrName3.Size = new Size(149, 20);
      this.txtCrName3.TabIndex = 60;
      this.txtCrIPAddress3.Enabled = false;
      this.txtCrIPAddress3.Location = new Point(691, 3);
      this.txtCrIPAddress3.Name = "txtCrIPAddress3";
      this.txtCrIPAddress3.Size = new Size(115, 20);
      this.txtCrIPAddress3.TabIndex = 67;
      this.txtCrIPAddress3.Tag = (object) "4029";
      this.txtCrIPAddress3.Leave += new EventHandler(this.IPAddress_Leave);
      this.lBtnCrIPAddress3.Location = new Point(765, 4);
      this.lBtnCrIPAddress3.LockedStateToolTip = "Use Default Value";
      this.lBtnCrIPAddress3.MaximumSize = new Size(16, 17);
      this.lBtnCrIPAddress3.MinimumSize = new Size(16, 17);
      this.lBtnCrIPAddress3.Name = "lBtnCrIPAddress3";
      this.lBtnCrIPAddress3.Size = new Size(16, 17);
      this.lBtnCrIPAddress3.TabIndex = 66;
      this.lBtnCrIPAddress3.Tag = (object) "LOCKBUTTON_4029";
      this.lBtnCrIPAddress3.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrIPAddress3.Visible = false;
      this.lBtnCrIPAddress3.Click += new EventHandler(this.lock_Clicked);
      this.txtCrEmail3.Enabled = false;
      this.txtCrEmail3.Location = new Point(288, 4);
      this.txtCrEmail3.Name = "txtCrEmail3";
      this.txtCrEmail3.Size = new Size(149, 20);
      this.txtCrEmail3.TabIndex = 61;
      this.txtCrDate3.Enabled = false;
      this.txtCrDate3.Location = new Point(565, 3);
      this.txtCrDate3.Name = "txtCrDate3";
      this.txtCrDate3.Size = new Size(115, 20);
      this.txtCrDate3.TabIndex = 65;
      this.txtCrDate3.Tag = (object) "4028";
      this.txtCrDate3.Leave += new EventHandler(this.DateField_Leave);
      this.clCrDate3.DateControl = (Control) this.txtCrDate3;
      this.clCrDate3.Enabled = false;
      ((IconButton) this.clCrDate3).Image = (Image) componentResourceManager.GetObject("clCrDate3.Image");
      this.clCrDate3.Location = new Point(686, 3);
      this.clCrDate3.MouseDownImage = (Image) null;
      this.clCrDate3.Name = "clCrDate3";
      this.clCrDate3.Size = new Size(16, 16);
      this.clCrDate3.SizeMode = PictureBoxSizeMode.AutoSize;
      this.clCrDate3.TabIndex = 98;
      this.clCrDate3.TabStop = false;
      this.clCrDate3.Visible = false;
      this.lBtnCrDate3.Location = new Point(592, 5);
      this.lBtnCrDate3.LockedStateToolTip = "Use Default Value";
      this.lBtnCrDate3.MaximumSize = new Size(16, 17);
      this.lBtnCrDate3.MinimumSize = new Size(16, 17);
      this.lBtnCrDate3.Name = "lBtnCrDate3";
      this.lBtnCrDate3.Size = new Size(16, 17);
      this.lBtnCrDate3.TabIndex = 64;
      this.lBtnCrDate3.Tag = (object) "LOCKBUTTON_4028";
      this.lBtnCrDate3.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrDate3.Visible = false;
      this.lBtnCrDate3.Click += new EventHandler(this.lock_Clicked);
      this.cmbCrStatus3.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCrStatus3.Enabled = false;
      this.cmbCrStatus3.FormattingEnabled = true;
      this.cmbCrStatus3.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      this.cmbCrStatus3.Location = new Point(442, 3);
      this.cmbCrStatus3.Name = "cmbCrStatus3";
      this.cmbCrStatus3.Size = new Size(115, 21);
      this.cmbCrStatus3.TabIndex = 63;
      this.cmbCrStatus3.Tag = (object) "4027";
      this.lBtnCrStatus3.Location = new Point(420, 5);
      this.lBtnCrStatus3.LockedStateToolTip = "Use Default Value";
      this.lBtnCrStatus3.MaximumSize = new Size(16, 17);
      this.lBtnCrStatus3.MinimumSize = new Size(16, 17);
      this.lBtnCrStatus3.Name = "lBtnCrStatus3";
      this.lBtnCrStatus3.Size = new Size(16, 17);
      this.lBtnCrStatus3.TabIndex = 62;
      this.lBtnCrStatus3.Tag = (object) "LOCKBUTTON_4027";
      this.lBtnCrStatus3.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrStatus3.Visible = false;
      this.lBtnCrStatus3.Click += new EventHandler(this.lock_Clicked);
      this.txtBrName3.Enabled = false;
      this.txtBrName3.Location = new Point(135, 4);
      this.txtBrName3.Name = "txtBrName3";
      this.txtBrName3.Size = new Size(149, 20);
      this.txtBrName3.TabIndex = 50;
      this.txtBrEmail3.Enabled = false;
      this.txtBrEmail3.Location = new Point(288, 4);
      this.txtBrEmail3.Name = "txtBrEmail3";
      this.txtBrEmail3.Size = new Size(149, 20);
      this.txtBrEmail3.TabIndex = 51;
      this.cmbBrStatus3.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrStatus3.Enabled = false;
      this.cmbBrStatus3.FormattingEnabled = true;
      this.cmbBrStatus3.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      this.cmbBrStatus3.Location = new Point(442, 3);
      this.cmbBrStatus3.Name = "cmbBrStatus3";
      this.cmbBrStatus3.Size = new Size(115, 21);
      this.cmbBrStatus3.TabIndex = 53;
      this.cmbBrStatus3.Tag = (object) "4023";
      this.lBtnBrStatus3.Location = new Point(420, 4);
      this.lBtnBrStatus3.LockedStateToolTip = "Use Default Value";
      this.lBtnBrStatus3.MaximumSize = new Size(16, 17);
      this.lBtnBrStatus3.MinimumSize = new Size(16, 17);
      this.lBtnBrStatus3.Name = "lBtnBrStatus3";
      this.lBtnBrStatus3.Size = new Size(16, 17);
      this.lBtnBrStatus3.TabIndex = 52;
      this.lBtnBrStatus3.Tag = (object) "LOCKBUTTON_4023";
      this.lBtnBrStatus3.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrStatus3.Visible = false;
      this.lBtnBrStatus3.Click += new EventHandler(this.lock_Clicked);
      this.lBtnBrDate3.Location = new Point(592, 4);
      this.lBtnBrDate3.LockedStateToolTip = "Use Default Value";
      this.lBtnBrDate3.MaximumSize = new Size(16, 17);
      this.lBtnBrDate3.MinimumSize = new Size(16, 17);
      this.lBtnBrDate3.Name = "lBtnBrDate3";
      this.lBtnBrDate3.Size = new Size(16, 17);
      this.lBtnBrDate3.TabIndex = 54;
      this.lBtnBrDate3.Tag = (object) "LOCKBUTTON_4024";
      this.lBtnBrDate3.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrDate3.Visible = false;
      this.lBtnBrDate3.Click += new EventHandler(this.lock_Clicked);
      this.clBrDate3.DateControl = (Control) this.txtBrDate3;
      this.clBrDate3.Enabled = false;
      ((IconButton) this.clBrDate3).Image = (Image) componentResourceManager.GetObject("clBrDate3.Image");
      this.clBrDate3.Location = new Point(686, 4);
      this.clBrDate3.MouseDownImage = (Image) null;
      this.clBrDate3.Name = "clBrDate3";
      this.clBrDate3.Size = new Size(16, 16);
      this.clBrDate3.SizeMode = PictureBoxSizeMode.AutoSize;
      this.clBrDate3.TabIndex = 96;
      this.clBrDate3.TabStop = false;
      this.clBrDate3.Visible = false;
      this.txtBrDate3.Enabled = false;
      this.txtBrDate3.Location = new Point(565, 4);
      this.txtBrDate3.Name = "txtBrDate3";
      this.txtBrDate3.Size = new Size(115, 20);
      this.txtBrDate3.TabIndex = 55;
      this.txtBrDate3.Tag = (object) "4024";
      this.txtBrDate3.Leave += new EventHandler(this.DateField_Leave);
      this.lBtnBrIPAddress3.Location = new Point(765, 3);
      this.lBtnBrIPAddress3.LockedStateToolTip = "Use Default Value";
      this.lBtnBrIPAddress3.MaximumSize = new Size(16, 17);
      this.lBtnBrIPAddress3.MinimumSize = new Size(16, 17);
      this.lBtnBrIPAddress3.Name = "lBtnBrIPAddress3";
      this.lBtnBrIPAddress3.Size = new Size(16, 17);
      this.lBtnBrIPAddress3.TabIndex = 56;
      this.lBtnBrIPAddress3.Tag = (object) "LOCKBUTTON_4025";
      this.lBtnBrIPAddress3.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrIPAddress3.Visible = false;
      this.lBtnBrIPAddress3.Click += new EventHandler(this.lock_Clicked);
      this.txtBrIPAddress3.Enabled = false;
      this.txtBrIPAddress3.Location = new Point(690, 3);
      this.txtBrIPAddress3.Name = "txtBrIPAddress3";
      this.txtBrIPAddress3.Size = new Size(115, 20);
      this.txtBrIPAddress3.TabIndex = 57;
      this.txtBrIPAddress3.Tag = (object) "4025";
      this.txtBrIPAddress3.Leave += new EventHandler(this.IPAddress_Leave);
      this.lBtnBrSource3.Location = new Point(914, 3);
      this.lBtnBrSource3.LockedStateToolTip = "Use Default Value";
      this.lBtnBrSource3.MaximumSize = new Size(16, 17);
      this.lBtnBrSource3.MinimumSize = new Size(16, 17);
      this.lBtnBrSource3.Name = "lBtnBrSource3";
      this.lBtnBrSource3.Size = new Size(16, 17);
      this.lBtnBrSource3.TabIndex = 58;
      this.lBtnBrSource3.Tag = (object) "LOCKBUTTON_4026";
      this.lBtnBrSource3.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrSource3.Visible = false;
      this.lBtnBrSource3.Click += new EventHandler(this.lock_Clicked);
      this.txtBrSource3.Enabled = false;
      this.txtBrSource3.Location = new Point(813, 3);
      this.txtBrSource3.Name = "txtBrSource3";
      this.txtBrSource3.Size = new Size(115, 20);
      this.txtBrSource3.TabIndex = 59;
      this.txtBrSource3.Tag = (object) "4026";
      this.panel2.Controls.Add((Control) this.btnBrWithdraw2);
      this.panel2.Controls.Add((Control) this.txtBrWithdrawnBy2);
      this.panel2.Controls.Add((Control) this.pnlCr2);
      this.panel2.Controls.Add((Control) this.label11);
      this.panel2.Controls.Add((Control) this.txtBrName2);
      this.panel2.Controls.Add((Control) this.txtBrEmail2);
      this.panel2.Controls.Add((Control) this.cmbBrStatus2);
      this.panel2.Controls.Add((Control) this.lBtnBrStatus2);
      this.panel2.Controls.Add((Control) this.lBtnBrDate2);
      this.panel2.Controls.Add((Control) this.clBrDate2);
      this.panel2.Controls.Add((Control) this.txtBrDate2);
      this.panel2.Controls.Add((Control) this.lBtnBrIPAddress2);
      this.panel2.Controls.Add((Control) this.txtBrIPAddress2);
      this.panel2.Controls.Add((Control) this.lBtnBrSource2);
      this.panel2.Controls.Add((Control) this.txtBrSource2);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(0, 50);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(1199, 50);
      this.panel2.TabIndex = 2;
      this.btnBrWithdraw2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnBrWithdraw2.Location = new Point(1060, 3);
      this.btnBrWithdraw2.Name = "btnBrWithdraw2";
      this.btnBrWithdraw2.Size = new Size(120, 20);
      this.btnBrWithdraw2.TabIndex = 0;
      this.btnBrWithdraw2.Text = "Withdraw Consent";
      this.btnBrWithdraw2.UseVisualStyleBackColor = true;
      this.btnBrWithdraw2.Click += new EventHandler(this.btnWithdraw_Click);
      this.txtBrWithdrawnBy2.Enabled = false;
      this.txtBrWithdrawnBy2.Location = new Point(936, 3);
      this.txtBrWithdrawnBy2.Name = "txtBrWithdrawnBy2";
      this.txtBrWithdrawnBy2.Size = new Size(115, 20);
      this.txtBrWithdrawnBy2.TabIndex = 135;
      this.txtBrWithdrawnBy2.Tag = (object) "4991";
      this.pnlCr2.Controls.Add((Control) this.btnCrWithdraw2);
      this.pnlCr2.Controls.Add((Control) this.txtCrWithdrawnBy2);
      this.pnlCr2.Controls.Add((Control) this.label10);
      this.pnlCr2.Controls.Add((Control) this.txtCrSource2);
      this.pnlCr2.Controls.Add((Control) this.lBtnCrSource2);
      this.pnlCr2.Controls.Add((Control) this.txtCrName2);
      this.pnlCr2.Controls.Add((Control) this.txtCrIPAddress2);
      this.pnlCr2.Controls.Add((Control) this.lBtnCrIPAddress2);
      this.pnlCr2.Controls.Add((Control) this.txtCrEmail2);
      this.pnlCr2.Controls.Add((Control) this.txtCrDate2);
      this.pnlCr2.Controls.Add((Control) this.clCrDate2);
      this.pnlCr2.Controls.Add((Control) this.lBtnCrDate2);
      this.pnlCr2.Controls.Add((Control) this.cmbCrStatus2);
      this.pnlCr2.Controls.Add((Control) this.lBtnCrStatus2);
      this.pnlCr2.Location = new Point(0, 25);
      this.pnlCr2.Name = "pnlCr2";
      this.pnlCr2.Size = new Size(1196, 28);
      this.pnlCr2.TabIndex = 133;
      this.btnCrWithdraw2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnCrWithdraw2.Location = new Point(1060, 3);
      this.btnCrWithdraw2.Name = "btnCrWithdraw2";
      this.btnCrWithdraw2.Size = new Size(120, 20);
      this.btnCrWithdraw2.TabIndex = 0;
      this.btnCrWithdraw2.Text = "Withdraw Consent";
      this.btnCrWithdraw2.UseVisualStyleBackColor = true;
      this.btnCrWithdraw2.Click += new EventHandler(this.btnWithdraw_Click);
      this.txtCrWithdrawnBy2.Enabled = false;
      this.txtCrWithdrawnBy2.Location = new Point(936, 3);
      this.txtCrWithdrawnBy2.Name = "txtCrWithdrawnBy2";
      this.txtCrWithdrawnBy2.Size = new Size(115, 20);
      this.txtCrWithdrawnBy2.TabIndex = 136;
      this.txtCrWithdrawnBy2.Tag = (object) "4992";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(3, 8);
      this.label10.Name = "label10";
      this.label10.Size = new Size(74, 13);
      this.label10.TabIndex = 61;
      this.label10.Text = "Co-Borrower 2";
      this.txtCrSource2.Enabled = false;
      this.txtCrSource2.Location = new Point(813, 3);
      this.txtCrSource2.Name = "txtCrSource2";
      this.txtCrSource2.Size = new Size(115, 20);
      this.txtCrSource2.TabIndex = 42;
      this.txtCrSource2.Tag = (object) "3999";
      this.lBtnCrSource2.Location = new Point(914, 4);
      this.lBtnCrSource2.LockedStateToolTip = "Use Default Value";
      this.lBtnCrSource2.MaximumSize = new Size(16, 17);
      this.lBtnCrSource2.MinimumSize = new Size(16, 17);
      this.lBtnCrSource2.Name = "lBtnCrSource2";
      this.lBtnCrSource2.Size = new Size(16, 17);
      this.lBtnCrSource2.TabIndex = 41;
      this.lBtnCrSource2.Tag = (object) "LOCKBUTTON_3999";
      this.lBtnCrSource2.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrSource2.Visible = false;
      this.lBtnCrSource2.Click += new EventHandler(this.lock_Clicked);
      this.txtCrName2.Enabled = false;
      this.txtCrName2.Location = new Point(135, 4);
      this.txtCrName2.Name = "txtCrName2";
      this.txtCrName2.Size = new Size(149, 20);
      this.txtCrName2.TabIndex = 33;
      this.txtCrIPAddress2.Enabled = false;
      this.txtCrIPAddress2.Location = new Point(690, 3);
      this.txtCrIPAddress2.Name = "txtCrIPAddress2";
      this.txtCrIPAddress2.Size = new Size(115, 20);
      this.txtCrIPAddress2.TabIndex = 40;
      this.txtCrIPAddress2.Tag = (object) "3998";
      this.txtCrIPAddress2.Leave += new EventHandler(this.IPAddress_Leave);
      this.lBtnCrIPAddress2.Location = new Point(765, 4);
      this.lBtnCrIPAddress2.LockedStateToolTip = "Use Default Value";
      this.lBtnCrIPAddress2.MaximumSize = new Size(16, 17);
      this.lBtnCrIPAddress2.MinimumSize = new Size(16, 17);
      this.lBtnCrIPAddress2.Name = "lBtnCrIPAddress2";
      this.lBtnCrIPAddress2.Size = new Size(16, 17);
      this.lBtnCrIPAddress2.TabIndex = 39;
      this.lBtnCrIPAddress2.Tag = (object) "LOCKBUTTON_3998";
      this.lBtnCrIPAddress2.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrIPAddress2.Visible = false;
      this.lBtnCrIPAddress2.Click += new EventHandler(this.lock_Clicked);
      this.txtCrEmail2.Enabled = false;
      this.txtCrEmail2.Location = new Point(288, 4);
      this.txtCrEmail2.Name = "txtCrEmail2";
      this.txtCrEmail2.Size = new Size(149, 20);
      this.txtCrEmail2.TabIndex = 34;
      this.txtCrDate2.Enabled = false;
      this.txtCrDate2.Location = new Point(565, 3);
      this.txtCrDate2.Name = "txtCrDate2";
      this.txtCrDate2.Size = new Size(115, 20);
      this.txtCrDate2.TabIndex = 38;
      this.txtCrDate2.Tag = (object) "3997";
      this.txtCrDate2.Leave += new EventHandler(this.DateField_Leave);
      this.clCrDate2.DateControl = (Control) this.txtCrDate2;
      this.clCrDate2.Enabled = false;
      ((IconButton) this.clCrDate2).Image = (Image) componentResourceManager.GetObject("clCrDate2.Image");
      this.clCrDate2.Location = new Point(686, 3);
      this.clCrDate2.MouseDownImage = (Image) null;
      this.clCrDate2.Name = "clCrDate2";
      this.clCrDate2.Size = new Size(16, 16);
      this.clCrDate2.SizeMode = PictureBoxSizeMode.AutoSize;
      this.clCrDate2.TabIndex = 74;
      this.clCrDate2.TabStop = false;
      this.clCrDate2.Visible = false;
      this.lBtnCrDate2.Location = new Point(592, 5);
      this.lBtnCrDate2.LockedStateToolTip = "Use Default Value";
      this.lBtnCrDate2.MaximumSize = new Size(16, 17);
      this.lBtnCrDate2.MinimumSize = new Size(16, 17);
      this.lBtnCrDate2.Name = "lBtnCrDate2";
      this.lBtnCrDate2.Size = new Size(16, 17);
      this.lBtnCrDate2.TabIndex = 37;
      this.lBtnCrDate2.Tag = (object) "LOCKBUTTON_3997";
      this.lBtnCrDate2.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrDate2.Visible = false;
      this.lBtnCrDate2.Click += new EventHandler(this.lock_Clicked);
      this.cmbCrStatus2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCrStatus2.Enabled = false;
      this.cmbCrStatus2.FormattingEnabled = true;
      this.cmbCrStatus2.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      this.cmbCrStatus2.Location = new Point(442, 3);
      this.cmbCrStatus2.Name = "cmbCrStatus2";
      this.cmbCrStatus2.Size = new Size(115, 21);
      this.cmbCrStatus2.TabIndex = 36;
      this.cmbCrStatus2.Tag = (object) "3996";
      this.lBtnCrStatus2.Location = new Point(420, 5);
      this.lBtnCrStatus2.LockedStateToolTip = "Use Default Value";
      this.lBtnCrStatus2.MaximumSize = new Size(16, 17);
      this.lBtnCrStatus2.MinimumSize = new Size(16, 17);
      this.lBtnCrStatus2.Name = "lBtnCrStatus2";
      this.lBtnCrStatus2.Size = new Size(16, 17);
      this.lBtnCrStatus2.TabIndex = 35;
      this.lBtnCrStatus2.Tag = (object) "LOCKBUTTON_3996";
      this.lBtnCrStatus2.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrStatus2.Visible = false;
      this.lBtnCrStatus2.Click += new EventHandler(this.lock_Clicked);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(4, 7);
      this.label11.Name = "label11";
      this.label11.Size = new Size(58, 13);
      this.label11.TabIndex = 60;
      this.label11.Text = "Borrower 2";
      this.txtBrName2.Enabled = false;
      this.txtBrName2.Location = new Point(135, 4);
      this.txtBrName2.Name = "txtBrName2";
      this.txtBrName2.Size = new Size(149, 20);
      this.txtBrName2.TabIndex = 22;
      this.txtBrEmail2.Enabled = false;
      this.txtBrEmail2.Location = new Point(288, 4);
      this.txtBrEmail2.Name = "txtBrEmail2";
      this.txtBrEmail2.Size = new Size(149, 20);
      this.txtBrEmail2.TabIndex = 23;
      this.cmbBrStatus2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrStatus2.Enabled = false;
      this.cmbBrStatus2.FormattingEnabled = true;
      this.cmbBrStatus2.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      this.cmbBrStatus2.Location = new Point(442, 3);
      this.cmbBrStatus2.Name = "cmbBrStatus2";
      this.cmbBrStatus2.Size = new Size(115, 21);
      this.cmbBrStatus2.TabIndex = 25;
      this.cmbBrStatus2.Tag = (object) "3992";
      this.lBtnBrStatus2.Location = new Point(420, 4);
      this.lBtnBrStatus2.LockedStateToolTip = "Use Default Value";
      this.lBtnBrStatus2.MaximumSize = new Size(16, 17);
      this.lBtnBrStatus2.MinimumSize = new Size(16, 17);
      this.lBtnBrStatus2.Name = "lBtnBrStatus2";
      this.lBtnBrStatus2.Size = new Size(16, 17);
      this.lBtnBrStatus2.TabIndex = 24;
      this.lBtnBrStatus2.Tag = (object) "LOCKBUTTON_3992";
      this.lBtnBrStatus2.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrStatus2.Visible = false;
      this.lBtnBrStatus2.Click += new EventHandler(this.lock_Clicked);
      this.lBtnBrDate2.Location = new Point(592, 4);
      this.lBtnBrDate2.LockedStateToolTip = "Use Default Value";
      this.lBtnBrDate2.MaximumSize = new Size(16, 17);
      this.lBtnBrDate2.MinimumSize = new Size(16, 17);
      this.lBtnBrDate2.Name = "lBtnBrDate2";
      this.lBtnBrDate2.Size = new Size(16, 17);
      this.lBtnBrDate2.TabIndex = 26;
      this.lBtnBrDate2.Tag = (object) "LOCKBUTTON_3993";
      this.lBtnBrDate2.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrDate2.Visible = false;
      this.lBtnBrDate2.Click += new EventHandler(this.lock_Clicked);
      this.clBrDate2.DateControl = (Control) this.txtBrDate2;
      this.clBrDate2.Enabled = false;
      ((IconButton) this.clBrDate2).Image = (Image) componentResourceManager.GetObject("clBrDate2.Image");
      this.clBrDate2.Location = new Point(686, 4);
      this.clBrDate2.MouseDownImage = (Image) null;
      this.clBrDate2.Name = "clBrDate2";
      this.clBrDate2.Size = new Size(16, 16);
      this.clBrDate2.SizeMode = PictureBoxSizeMode.AutoSize;
      this.clBrDate2.TabIndex = 72;
      this.clBrDate2.TabStop = false;
      this.clBrDate2.Visible = false;
      this.txtBrDate2.Enabled = false;
      this.txtBrDate2.Location = new Point(565, 4);
      this.txtBrDate2.Name = "txtBrDate2";
      this.txtBrDate2.Size = new Size(115, 20);
      this.txtBrDate2.TabIndex = 27;
      this.txtBrDate2.Tag = (object) "3993";
      this.txtBrDate2.Leave += new EventHandler(this.DateField_Leave);
      this.lBtnBrIPAddress2.Location = new Point(765, 3);
      this.lBtnBrIPAddress2.LockedStateToolTip = "Use Default Value";
      this.lBtnBrIPAddress2.MaximumSize = new Size(16, 17);
      this.lBtnBrIPAddress2.MinimumSize = new Size(16, 17);
      this.lBtnBrIPAddress2.Name = "lBtnBrIPAddress2";
      this.lBtnBrIPAddress2.Size = new Size(16, 17);
      this.lBtnBrIPAddress2.TabIndex = 28;
      this.lBtnBrIPAddress2.Tag = (object) "LOCKBUTTON_3994";
      this.lBtnBrIPAddress2.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrIPAddress2.Visible = false;
      this.lBtnBrIPAddress2.Click += new EventHandler(this.lock_Clicked);
      this.txtBrIPAddress2.Enabled = false;
      this.txtBrIPAddress2.Location = new Point(690, 3);
      this.txtBrIPAddress2.Name = "txtBrIPAddress2";
      this.txtBrIPAddress2.Size = new Size(115, 20);
      this.txtBrIPAddress2.TabIndex = 29;
      this.txtBrIPAddress2.Tag = (object) "3994";
      this.txtBrIPAddress2.Leave += new EventHandler(this.IPAddress_Leave);
      this.lBtnBrSource2.Location = new Point(914, 3);
      this.lBtnBrSource2.LockedStateToolTip = "Use Default Value";
      this.lBtnBrSource2.MaximumSize = new Size(16, 17);
      this.lBtnBrSource2.MinimumSize = new Size(16, 17);
      this.lBtnBrSource2.Name = "lBtnBrSource2";
      this.lBtnBrSource2.Size = new Size(16, 17);
      this.lBtnBrSource2.TabIndex = 31;
      this.lBtnBrSource2.Tag = (object) "LOCKBUTTON_3995";
      this.lBtnBrSource2.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrSource2.Visible = false;
      this.lBtnBrSource2.Click += new EventHandler(this.lock_Clicked);
      this.txtBrSource2.Enabled = false;
      this.txtBrSource2.Location = new Point(813, 3);
      this.txtBrSource2.Name = "txtBrSource2";
      this.txtBrSource2.Size = new Size(115, 20);
      this.txtBrSource2.TabIndex = 32;
      this.txtBrSource2.Tag = (object) "3995";
      this.panel1.Controls.Add((Control) this.btnBrWithdraw1);
      this.panel1.Controls.Add((Control) this.txtBrWithdrawnBy1);
      this.panel1.Controls.Add((Control) this.pnlCr1);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.txtBrName1);
      this.panel1.Controls.Add((Control) this.txtBrEmail1);
      this.panel1.Controls.Add((Control) this.cmbBrStatus1);
      this.panel1.Controls.Add((Control) this.lBtnBrStatus1);
      this.panel1.Controls.Add((Control) this.lBtnBrDate1);
      this.panel1.Controls.Add((Control) this.clBrDate1);
      this.panel1.Controls.Add((Control) this.txtBrDate1);
      this.panel1.Controls.Add((Control) this.lBtnBrIPAddress1);
      this.panel1.Controls.Add((Control) this.txtBrIPAddress1);
      this.panel1.Controls.Add((Control) this.lBtnBrSource1);
      this.panel1.Controls.Add((Control) this.txtBrSource1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1199, 50);
      this.panel1.TabIndex = 1;
      this.btnBrWithdraw1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnBrWithdraw1.Location = new Point(1060, 3);
      this.btnBrWithdraw1.Name = "btnBrWithdraw1";
      this.btnBrWithdraw1.Size = new Size(120, 20);
      this.btnBrWithdraw1.TabIndex = 0;
      this.btnBrWithdraw1.Text = "Withdraw Consent";
      this.btnBrWithdraw1.UseVisualStyleBackColor = true;
      this.btnBrWithdraw1.Click += new EventHandler(this.btnWithdraw_Click);
      this.txtBrWithdrawnBy1.Enabled = false;
      this.txtBrWithdrawnBy1.Location = new Point(936, 3);
      this.txtBrWithdrawnBy1.Name = "txtBrWithdrawnBy1";
      this.txtBrWithdrawnBy1.Size = new Size(115, 20);
      this.txtBrWithdrawnBy1.TabIndex = 12;
      this.txtBrWithdrawnBy1.Tag = (object) "4989";
      this.pnlCr1.Controls.Add((Control) this.btnCrWithdraw1);
      this.pnlCr1.Controls.Add((Control) this.txtCrWithdrawnBy1);
      this.pnlCr1.Controls.Add((Control) this.label9);
      this.pnlCr1.Controls.Add((Control) this.txtCrSource1);
      this.pnlCr1.Controls.Add((Control) this.lBtnCrSource1);
      this.pnlCr1.Controls.Add((Control) this.txtCrName1);
      this.pnlCr1.Controls.Add((Control) this.txtCrIPAddress1);
      this.pnlCr1.Controls.Add((Control) this.lBtnCrIPAddress1);
      this.pnlCr1.Controls.Add((Control) this.txtCrEmail1);
      this.pnlCr1.Controls.Add((Control) this.txtCrDate1);
      this.pnlCr1.Controls.Add((Control) this.clCrDate1);
      this.pnlCr1.Controls.Add((Control) this.lBtnCrDate1);
      this.pnlCr1.Controls.Add((Control) this.cmbCrStatus1);
      this.pnlCr1.Controls.Add((Control) this.lBtnCrStatus1);
      this.pnlCr1.Location = new Point(0, 26);
      this.pnlCr1.Name = "pnlCr1";
      this.pnlCr1.Size = new Size(1196, 28);
      this.pnlCr1.TabIndex = 133;
      this.btnCrWithdraw1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnCrWithdraw1.Location = new Point(1060, 3);
      this.btnCrWithdraw1.Name = "btnCrWithdraw1";
      this.btnCrWithdraw1.Size = new Size(120, 20);
      this.btnCrWithdraw1.TabIndex = 0;
      this.btnCrWithdraw1.Text = "Withdraw Consent";
      this.btnCrWithdraw1.UseVisualStyleBackColor = true;
      this.btnCrWithdraw1.Click += new EventHandler(this.btnWithdraw_Click);
      this.txtCrWithdrawnBy1.Enabled = false;
      this.txtCrWithdrawnBy1.Location = new Point(936, 3);
      this.txtCrWithdrawnBy1.Name = "txtCrWithdrawnBy1";
      this.txtCrWithdrawnBy1.Size = new Size(115, 20);
      this.txtCrWithdrawnBy1.TabIndex = 22;
      this.txtCrWithdrawnBy1.Tag = (object) "4990";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(4, 6);
      this.label9.Name = "label9";
      this.label9.Size = new Size(74, 13);
      this.label9.TabIndex = 10;
      this.label9.Text = "Co-Borrower 1";
      this.txtCrSource1.Enabled = false;
      this.txtCrSource1.Location = new Point(812, 3);
      this.txtCrSource1.Name = "txtCrSource1";
      this.txtCrSource1.Size = new Size(115, 20);
      this.txtCrSource1.TabIndex = 21;
      this.txtCrSource1.Tag = (object) "3991";
      this.lBtnCrSource1.Location = new Point(914, 4);
      this.lBtnCrSource1.LockedStateToolTip = "Use Default Value";
      this.lBtnCrSource1.MaximumSize = new Size(16, 17);
      this.lBtnCrSource1.MinimumSize = new Size(16, 17);
      this.lBtnCrSource1.Name = "lBtnCrSource1";
      this.lBtnCrSource1.Size = new Size(16, 17);
      this.lBtnCrSource1.TabIndex = 20;
      this.lBtnCrSource1.Tag = (object) "LOCKBUTTON_3991";
      this.lBtnCrSource1.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrSource1.Visible = false;
      this.lBtnCrSource1.Click += new EventHandler(this.lock_Clicked);
      this.txtCrName1.Enabled = false;
      this.txtCrName1.Location = new Point(135, 4);
      this.txtCrName1.Name = "txtCrName1";
      this.txtCrName1.Size = new Size(149, 20);
      this.txtCrName1.TabIndex = 12;
      this.txtCrIPAddress1.Enabled = false;
      this.txtCrIPAddress1.Location = new Point(689, 3);
      this.txtCrIPAddress1.Name = "txtCrIPAddress1";
      this.txtCrIPAddress1.Size = new Size(115, 20);
      this.txtCrIPAddress1.TabIndex = 19;
      this.txtCrIPAddress1.Tag = (object) "3990";
      this.txtCrIPAddress1.Leave += new EventHandler(this.IPAddress_Leave);
      this.lBtnCrIPAddress1.Location = new Point(765, 4);
      this.lBtnCrIPAddress1.LockedStateToolTip = "Use Default Value";
      this.lBtnCrIPAddress1.MaximumSize = new Size(16, 17);
      this.lBtnCrIPAddress1.MinimumSize = new Size(16, 17);
      this.lBtnCrIPAddress1.Name = "lBtnCrIPAddress1";
      this.lBtnCrIPAddress1.Size = new Size(16, 17);
      this.lBtnCrIPAddress1.TabIndex = 18;
      this.lBtnCrIPAddress1.Tag = (object) "LOCKBUTTON_3990";
      this.lBtnCrIPAddress1.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrIPAddress1.Visible = false;
      this.lBtnCrIPAddress1.Click += new EventHandler(this.lock_Clicked);
      this.txtCrEmail1.Enabled = false;
      this.txtCrEmail1.Location = new Point(288, 4);
      this.txtCrEmail1.Name = "txtCrEmail1";
      this.txtCrEmail1.Size = new Size(149, 20);
      this.txtCrEmail1.TabIndex = 13;
      this.txtCrDate1.Enabled = false;
      this.txtCrDate1.Location = new Point(565, 3);
      this.txtCrDate1.Name = "txtCrDate1";
      this.txtCrDate1.Size = new Size(115, 20);
      this.txtCrDate1.TabIndex = 17;
      this.txtCrDate1.Tag = (object) "3989";
      this.txtCrDate1.Leave += new EventHandler(this.DateField_Leave);
      this.clCrDate1.DateControl = (Control) this.txtCrDate1;
      this.clCrDate1.Enabled = false;
      ((IconButton) this.clCrDate1).Image = (Image) componentResourceManager.GetObject("clCrDate1.Image");
      this.clCrDate1.Location = new Point(686, 3);
      this.clCrDate1.MouseDownImage = (Image) null;
      this.clCrDate1.Name = "clCrDate1";
      this.clCrDate1.Size = new Size(16, 16);
      this.clCrDate1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.clCrDate1.TabIndex = 50;
      this.clCrDate1.TabStop = false;
      this.clCrDate1.Visible = false;
      this.lBtnCrDate1.Location = new Point(592, 5);
      this.lBtnCrDate1.LockedStateToolTip = "Use Default Value";
      this.lBtnCrDate1.MaximumSize = new Size(16, 17);
      this.lBtnCrDate1.MinimumSize = new Size(16, 17);
      this.lBtnCrDate1.Name = "lBtnCrDate1";
      this.lBtnCrDate1.Size = new Size(16, 17);
      this.lBtnCrDate1.TabIndex = 16;
      this.lBtnCrDate1.Tag = (object) "LOCKBUTTON_3989";
      this.lBtnCrDate1.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrDate1.Visible = false;
      this.lBtnCrDate1.Click += new EventHandler(this.lock_Clicked);
      this.cmbCrStatus1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCrStatus1.Enabled = false;
      this.cmbCrStatus1.FormattingEnabled = true;
      this.cmbCrStatus1.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      this.cmbCrStatus1.Location = new Point(442, 3);
      this.cmbCrStatus1.Name = "cmbCrStatus1";
      this.cmbCrStatus1.Size = new Size(115, 21);
      this.cmbCrStatus1.TabIndex = 15;
      this.cmbCrStatus1.Tag = (object) "3988";
      this.lBtnCrStatus1.Location = new Point(420, 5);
      this.lBtnCrStatus1.LockedStateToolTip = "Use Default Value";
      this.lBtnCrStatus1.MaximumSize = new Size(16, 17);
      this.lBtnCrStatus1.MinimumSize = new Size(16, 17);
      this.lBtnCrStatus1.Name = "lBtnCrStatus1";
      this.lBtnCrStatus1.Size = new Size(16, 17);
      this.lBtnCrStatus1.TabIndex = 14;
      this.lBtnCrStatus1.Tag = (object) "LOCKBUTTON_3988";
      this.lBtnCrStatus1.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCrStatus1.Visible = false;
      this.lBtnCrStatus1.Click += new EventHandler(this.lock_Clicked);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(3, 7);
      this.label8.Name = "label8";
      this.label8.Size = new Size(58, 13);
      this.label8.TabIndex = 9;
      this.label8.Text = "Borrower 1";
      this.txtBrName1.Enabled = false;
      this.txtBrName1.Location = new Point(135, 4);
      this.txtBrName1.Name = "txtBrName1";
      this.txtBrName1.Size = new Size(149, 20);
      this.txtBrName1.TabIndex = 2;
      this.txtBrEmail1.Enabled = false;
      this.txtBrEmail1.Location = new Point(288, 4);
      this.txtBrEmail1.Name = "txtBrEmail1";
      this.txtBrEmail1.Size = new Size(149, 20);
      this.txtBrEmail1.TabIndex = 3;
      this.cmbBrStatus1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrStatus1.Enabled = false;
      this.cmbBrStatus1.FormattingEnabled = true;
      this.cmbBrStatus1.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Accepted",
        (object) "Rejected",
        (object) "Pending"
      });
      this.cmbBrStatus1.Location = new Point(442, 3);
      this.cmbBrStatus1.Name = "cmbBrStatus1";
      this.cmbBrStatus1.Size = new Size(115, 21);
      this.cmbBrStatus1.TabIndex = 5;
      this.cmbBrStatus1.Tag = (object) "3984";
      this.lBtnBrStatus1.Location = new Point(420, 4);
      this.lBtnBrStatus1.LockedStateToolTip = "Use Default Value";
      this.lBtnBrStatus1.MaximumSize = new Size(16, 17);
      this.lBtnBrStatus1.MinimumSize = new Size(16, 17);
      this.lBtnBrStatus1.Name = "lBtnBrStatus1";
      this.lBtnBrStatus1.Size = new Size(16, 17);
      this.lBtnBrStatus1.TabIndex = 4;
      this.lBtnBrStatus1.Tag = (object) "LOCKBUTTON_3984";
      this.lBtnBrStatus1.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrStatus1.Visible = false;
      this.lBtnBrStatus1.Click += new EventHandler(this.lock_Clicked);
      this.lBtnBrDate1.Location = new Point(592, 4);
      this.lBtnBrDate1.LockedStateToolTip = "Use Default Value";
      this.lBtnBrDate1.MaximumSize = new Size(16, 17);
      this.lBtnBrDate1.MinimumSize = new Size(16, 17);
      this.lBtnBrDate1.Name = "lBtnBrDate1";
      this.lBtnBrDate1.Size = new Size(16, 17);
      this.lBtnBrDate1.TabIndex = 6;
      this.lBtnBrDate1.Tag = (object) "LOCKBUTTON_3985";
      this.lBtnBrDate1.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrDate1.Visible = false;
      this.lBtnBrDate1.Click += new EventHandler(this.lock_Clicked);
      this.clBrDate1.DateControl = (Control) this.txtBrDate1;
      this.clBrDate1.Enabled = false;
      ((IconButton) this.clBrDate1).Image = (Image) componentResourceManager.GetObject("clBrDate1.Image");
      this.clBrDate1.Location = new Point(686, 4);
      this.clBrDate1.MouseDownImage = (Image) null;
      this.clBrDate1.Name = "clBrDate1";
      this.clBrDate1.Size = new Size(16, 16);
      this.clBrDate1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.clBrDate1.TabIndex = 48;
      this.clBrDate1.TabStop = false;
      this.clBrDate1.Visible = false;
      this.txtBrDate1.Enabled = false;
      this.txtBrDate1.Location = new Point(565, 4);
      this.txtBrDate1.Name = "txtBrDate1";
      this.txtBrDate1.Size = new Size(115, 20);
      this.txtBrDate1.TabIndex = 7;
      this.txtBrDate1.Tag = (object) "3985";
      this.txtBrDate1.Leave += new EventHandler(this.DateField_Leave);
      this.lBtnBrIPAddress1.Location = new Point(765, 3);
      this.lBtnBrIPAddress1.LockedStateToolTip = "Use Default Value";
      this.lBtnBrIPAddress1.MaximumSize = new Size(16, 17);
      this.lBtnBrIPAddress1.MinimumSize = new Size(16, 17);
      this.lBtnBrIPAddress1.Name = "lBtnBrIPAddress1";
      this.lBtnBrIPAddress1.Size = new Size(16, 17);
      this.lBtnBrIPAddress1.TabIndex = 8;
      this.lBtnBrIPAddress1.Tag = (object) "LOCKBUTTON_3986";
      this.lBtnBrIPAddress1.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrIPAddress1.Visible = false;
      this.lBtnBrIPAddress1.Click += new EventHandler(this.lock_Clicked);
      this.txtBrIPAddress1.Enabled = false;
      this.txtBrIPAddress1.Location = new Point(690, 3);
      this.txtBrIPAddress1.Name = "txtBrIPAddress1";
      this.txtBrIPAddress1.Size = new Size(115, 20);
      this.txtBrIPAddress1.TabIndex = 9;
      this.txtBrIPAddress1.Tag = (object) "3986";
      this.txtBrIPAddress1.Leave += new EventHandler(this.IPAddress_Leave);
      this.lBtnBrSource1.Location = new Point(914, 3);
      this.lBtnBrSource1.LockedStateToolTip = "Use Default Value";
      this.lBtnBrSource1.MaximumSize = new Size(16, 17);
      this.lBtnBrSource1.MinimumSize = new Size(16, 17);
      this.lBtnBrSource1.Name = "lBtnBrSource1";
      this.lBtnBrSource1.Size = new Size(16, 17);
      this.lBtnBrSource1.TabIndex = 10;
      this.lBtnBrSource1.Tag = (object) "LOCKBUTTON_3987";
      this.lBtnBrSource1.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBrSource1.Visible = false;
      this.lBtnBrSource1.Click += new EventHandler(this.lock_Clicked);
      this.txtBrSource1.Enabled = false;
      this.txtBrSource1.Location = new Point(813, 3);
      this.txtBrSource1.Name = "txtBrSource1";
      this.txtBrSource1.Size = new Size(115, 20);
      this.txtBrSource1.TabIndex = 11;
      this.txtBrSource1.Tag = (object) "3987";
      this.panel7.Controls.Add((Control) this.label20);
      this.panel7.Controls.Add((Control) this.label1);
      this.panel7.Controls.Add((Control) this.txtConsentDate);
      this.panel7.Controls.Add((Control) this.label2);
      this.panel7.Controls.Add((Control) this.label3);
      this.panel7.Controls.Add((Control) this.label4);
      this.panel7.Controls.Add((Control) this.label5);
      this.panel7.Controls.Add((Control) this.label6);
      this.panel7.Controls.Add((Control) this.label7);
      this.panel7.Dock = DockStyle.Top;
      this.panel7.Location = new Point(1, 26);
      this.panel7.Name = "panel7";
      this.panel7.Size = new Size(1199, 57);
      this.panel7.TabIndex = 133;
      this.label20.AutoSize = true;
      this.label20.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
      this.label20.Location = new Point(935, 39);
      this.label20.Name = "label20";
      this.label20.Size = new Size(85, 13);
      this.label20.TabIndex = 9;
      this.label20.Text = "Withdrawn By";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(78, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "eConsent Date";
      this.txtConsentDate.Enabled = false;
      this.txtConsentDate.Location = new Point(115, 7);
      this.txtConsentDate.Name = "txtConsentDate";
      this.txtConsentDate.Size = new Size(100, 20);
      this.txtConsentDate.TabIndex = 1;
      this.txtConsentDate.Tag = (object) "\"3983\"";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(133, 39);
      this.label2.Name = "label2";
      this.label2.Size = new Size(39, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Name";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(285, 39);
      this.label3.Name = "label3";
      this.label3.Size = new Size(37, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Email";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
      this.label4.Location = new Point(439, 39);
      this.label4.Name = "label4";
      this.label4.Size = new Size(100, 13);
      this.label4.TabIndex = 5;
      this.label4.Text = "eConsent Status";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
      this.label5.Location = new Point(564, 39);
      this.label5.Name = "label5";
      this.label5.Size = new Size(74, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Status Date";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
      this.label6.Location = new Point(689, 39);
      this.label6.Name = "label6";
      this.label6.Size = new Size(68, 13);
      this.label6.TabIndex = 7;
      this.label6.Text = "IP Address";
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
      this.label7.Location = new Point(812, 39);
      this.label7.Name = "label7";
      this.label7.Size = new Size(47, 13);
      this.label7.TabIndex = 8;
      this.label7.Text = "Source";
      this.viewFormLink.AutoSize = true;
      this.viewFormLink.BackColor = Color.Transparent;
      this.viewFormLink.CausesValidation = false;
      this.viewFormLink.Location = new Point(1089, 6);
      this.viewFormLink.Name = "viewFormLink";
      this.viewFormLink.Size = new Size(56, 13);
      this.viewFormLink.TabIndex = 9;
      this.viewFormLink.TabStop = true;
      this.viewFormLink.Text = "View Form";
      this.viewFormLink.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(1201, 440);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (eSignConsent);
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "eConsent Status";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.panel8.ResumeLayout(false);
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.pnlCr6.ResumeLayout(false);
      this.pnlCr6.PerformLayout();
      ((ISupportInitialize) this.clCrDate6).EndInit();
      ((ISupportInitialize) this.clBrDate6).EndInit();
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      this.pnlCr5.ResumeLayout(false);
      this.pnlCr5.PerformLayout();
      ((ISupportInitialize) this.clCrDate5).EndInit();
      ((ISupportInitialize) this.clBrDate5).EndInit();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.pnlCr4.ResumeLayout(false);
      this.pnlCr4.PerformLayout();
      ((ISupportInitialize) this.clCrDate4).EndInit();
      ((ISupportInitialize) this.clBrDate4).EndInit();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.pnlCr3.ResumeLayout(false);
      this.pnlCr3.PerformLayout();
      ((ISupportInitialize) this.clCrDate3).EndInit();
      ((ISupportInitialize) this.clBrDate3).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.pnlCr2.ResumeLayout(false);
      this.pnlCr2.PerformLayout();
      ((ISupportInitialize) this.clCrDate2).EndInit();
      ((ISupportInitialize) this.clBrDate2).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.pnlCr1.ResumeLayout(false);
      this.pnlCr1.PerformLayout();
      ((ISupportInitialize) this.clCrDate1).EndInit();
      ((ISupportInitialize) this.clBrDate1).EndInit();
      this.panel7.ResumeLayout(false);
      this.panel7.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
