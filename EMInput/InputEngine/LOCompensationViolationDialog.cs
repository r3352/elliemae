// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LOCompensationViolationDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LOCompensationViolationDialog : Form
  {
    private LOCompensationSetting loCompensationSetting;
    private IHtmlInput dataObject;
    private PopupBusinessRules popupRules;
    private string currentFieldID = string.Empty;
    private string currentVal = string.Empty;
    private List<Panel> panelBoxes;
    private List<ComboBox> comboBoxes;
    private List<string> borrowerBoxes;
    private List<string> sellerBoxes;
    private bool hideCancelButton;
    private bool updateGFE = true;
    private IContainer components;
    private Button btnCancel;
    private Label label1;
    private Label label3;
    private RadioButton rdoToBorrower;
    private RadioButton rdoTo3rdParty;
    private RadioButton rdoTakeNoAction;
    private Panel panel1;
    private Button btnOK;
    private GroupContainer groupContainer1;
    private Label label4;
    private ComboBox cbo801k;
    private Label label25;
    private TextBox textBox11;
    private ComboBox cbo801j;
    private Label label23;
    private TextBox textBox10;
    private ComboBox cbo801i;
    private Label label21;
    private TextBox textBox9;
    private ComboBox cbo801h;
    private Label label19;
    private TextBox textBox8;
    private ComboBox cbo801g;
    private Label label17;
    private TextBox textBox7;
    private ComboBox cbo801f;
    private Label label15;
    private TextBox textBox6;
    private Label label16;
    private ComboBox cbo801e;
    private Label label13;
    private TextBox textBox5;
    private Label label14;
    private ComboBox cbo801d;
    private Label label11;
    private TextBox textBox4;
    private Label label12;
    private ComboBox cbo801c;
    private Label label9;
    private TextBox textBox3;
    private Label label10;
    private ComboBox cbo801b;
    private Label label7;
    private TextBox textBox2;
    private Label label8;
    private Label label6;
    private ComboBox cbo801a;
    private Label label5;
    private TextBox textBox1;
    private TextBox textBox16;
    private TextBox textBox15;
    private TextBox textBox14;
    private TextBox textBox13;
    private TextBox textBox12;
    private PictureBox pboxAsterisk;
    private PictureBox pboxDownArrow;
    private Panel panel801k;
    private Panel panel801j;
    private Panel panel801i;
    private Panel panel801h;
    private Panel panel801g;
    private Panel panel801f;
    private Panel panel801e;
    private Panel panel801d;
    private Panel panel801c;
    private Panel panel801b;
    private Panel panel801a;
    private Panel panel3;
    private TextBox textBox19;
    private Label label20;
    private TextBox textBox20;
    private Label label22;
    private TextBox textBox18;
    private Label label18;
    private TextBox textBox17;
    private Label label2;
    private TextBox textBox21;
    private Label label24;
    private PictureBox pictureBox1;
    private TextBox textBox32;
    private TextBox textBox31;
    private TextBox textBox30;
    private TextBox textBox29;
    private TextBox textBox28;
    private TextBox textBox27;
    private TextBox textBox26;
    private TextBox textBox25;
    private TextBox textBox24;
    private TextBox textBox23;
    private TextBox textBox22;
    private PictureBox pictureBox11;
    private PictureBox pictureBox10;
    private PictureBox pictureBox9;
    private PictureBox pictureBox8;
    private PictureBox pictureBox7;
    private PictureBox pictureBox6;
    private PictureBox pictureBox5;
    private PictureBox pictureBox4;
    private PictureBox pictureBox3;
    private PictureBox pictureBox2;
    private Panel panel801l;
    private TextBox textBox33;
    private TextBox textBox34;
    private Label label26;
    private TextBox textBox35;
    private Label label27;
    private TextBox textBox36;
    private Label label28;
    private Label label29;
    private PictureBox pictureBox19;
    private ComboBox cbo801s;
    private Label label31;
    private Label label30;
    private Panel panel801s;
    private Panel panel801r;
    private Panel panel801q;
    private Panel panel801p;
    private Panel panel801o;
    private Panel panel801n;
    private Panel panel801m;
    private PictureBox pictureBox12;
    private TextBox textBox37;
    private TextBox textBox38;
    private TextBox textBox39;
    private ComboBox cbo801l;
    private Label label32;
    private PictureBox pictureBox18;
    private TextBox textBox55;
    private TextBox textBox56;
    private TextBox textBox57;
    private ComboBox cbo801r;
    private Label label38;
    private PictureBox pictureBox17;
    private TextBox textBox52;
    private TextBox textBox53;
    private TextBox textBox54;
    private ComboBox cbo801q;
    private Label label37;
    private PictureBox pictureBox16;
    private TextBox textBox49;
    private TextBox textBox50;
    private TextBox textBox51;
    private ComboBox cbo801p;
    private Label label36;
    private PictureBox pictureBox15;
    private TextBox textBox46;
    private TextBox textBox47;
    private TextBox textBox48;
    private ComboBox cbo801o;
    private Label label35;
    private PictureBox pictureBox14;
    private TextBox textBox43;
    private TextBox textBox44;
    private TextBox textBox45;
    private ComboBox cbo801n;
    private Label label34;
    private PictureBox pictureBox13;
    private TextBox textBox40;
    private TextBox textBox41;
    private TextBox textBox42;
    private ComboBox cbo801m;
    private Label label33;

    public LOCompensationViolationDialog(
      IHtmlInput dataObject,
      LOCompensationSetting loCompensationSetting,
      bool hideCancelButton)
      : this(dataObject, (string) null, (string) null, loCompensationSetting, true)
    {
      this.hideCancelButton = hideCancelButton;
      if (!this.hideCancelButton)
        return;
      this.btnCancel.Visible = false;
      this.btnOK.Left = this.btnCancel.Left;
      this.FormClosing += new FormClosingEventHandler(this.LOCompensationViolationDialog_FormClosing);
    }

    private void LOCompensationViolationDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.hideCancelButton || this.rdoTakeNoAction.Visible || this.DialogResult == DialogResult.OK)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "In order to comply with the LO Compensation rule, you have to select an action before closing this form.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      e.Cancel = true;
    }

    public LOCompensationViolationDialog(
      IHtmlInput dataObject,
      string id,
      string val,
      LOCompensationSetting loCompensationSetting,
      bool updateGFE)
    {
      this.updateGFE = updateGFE;
      this.loCompensationSetting = loCompensationSetting;
      this.dataObject = dataObject;
      this.currentFieldID = id;
      this.currentVal = val;
      this.InitializeComponent();
      LoanData dataObject1 = this.dataObject as LoanData;
      this.initForm();
      if (this.loCompensationSetting.LOAction == LOCompensationSetting.LOActions.Fix)
      {
        this.rdoTakeNoAction.Visible = false;
        this.rdoToBorrower.Checked = this.rdoTo3rdParty.Checked = false;
      }
      else
        this.rdoTakeNoAction.Checked = true;
    }

    private void initForm()
    {
      this.populateFieldValues(this.Controls, (string) null, (string) null);
      this.panelBoxes = new List<Panel>();
      this.comboBoxes = new List<ComboBox>();
      this.borrowerBoxes = new List<string>();
      this.sellerBoxes = new List<string>();
      int num = 0;
      if (this.loCompensationSetting.IsLineItemEnabled("s", this.dataObject))
      {
        ++num;
        this.panel801s.Visible = true;
        this.panelBoxes.Add(this.panel801s);
        this.comboBoxes.Add(this.cbo801s);
        this.borrowerBoxes.Add("NEWHUD.X15");
        this.sellerBoxes.Add("NEWHUD.X788");
      }
      else
        this.panel801s.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("r", this.dataObject))
      {
        ++num;
        this.panel801r.Visible = true;
        this.panelBoxes.Add(this.panel801r);
        this.comboBoxes.Add(this.cbo801r);
        this.borrowerBoxes.Add("NEWHUD.X1237");
        this.sellerBoxes.Add("NEWHUD.X1238");
      }
      else
        this.panel801r.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("q", this.dataObject))
      {
        ++num;
        this.panel801q.Visible = true;
        this.panelBoxes.Add(this.panel801q);
        this.comboBoxes.Add(this.cbo801q);
        this.borrowerBoxes.Add("NEWHUD.X1245");
        this.sellerBoxes.Add("NEWHUD.X1246");
      }
      else
        this.panel801q.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("p", this.dataObject))
      {
        ++num;
        this.panel801p.Visible = true;
        this.panelBoxes.Add(this.panel801p);
        this.comboBoxes.Add(this.cbo801p);
        this.borrowerBoxes.Add("NEWHUD.X1253");
        this.sellerBoxes.Add("NEWHUD.X1254");
      }
      else
        this.panel801p.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("o", this.dataObject))
      {
        ++num;
        this.panel801o.Visible = true;
        this.panelBoxes.Add(this.panel801o);
        this.comboBoxes.Add(this.cbo801o);
        this.borrowerBoxes.Add("NEWHUD.X1261");
        this.sellerBoxes.Add("NEWHUD.X1262");
      }
      else
        this.panel801o.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("n", this.dataObject))
      {
        ++num;
        this.panel801n.Visible = true;
        this.panelBoxes.Add(this.panel801n);
        this.comboBoxes.Add(this.cbo801n);
        this.borrowerBoxes.Add("NEWHUD.X1269");
        this.sellerBoxes.Add("NEWHUD.X1270");
      }
      else
        this.panel801n.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("m", this.dataObject))
      {
        ++num;
        this.panel801m.Visible = true;
        this.panelBoxes.Add(this.panel801m);
        this.comboBoxes.Add(this.cbo801m);
        this.borrowerBoxes.Add("NEWHUD.X1277");
        this.sellerBoxes.Add("NEWHUD.X1278");
      }
      else
        this.panel801m.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("l", this.dataObject))
      {
        ++num;
        this.panel801l.Visible = true;
        this.panelBoxes.Add(this.panel801l);
        this.comboBoxes.Add(this.cbo801l);
        this.borrowerBoxes.Add("NEWHUD.X1285");
        this.sellerBoxes.Add("NEWHUD.X1286");
      }
      else
        this.panel801l.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("k", this.dataObject))
      {
        ++num;
        this.panel801k.Visible = true;
        this.panelBoxes.Add(this.panel801k);
        this.comboBoxes.Add(this.cbo801k);
        this.borrowerBoxes.Add("NEWHUD.X733");
        this.sellerBoxes.Add("NEWHUD.X779");
      }
      else
        this.panel801k.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("j", this.dataObject))
      {
        ++num;
        this.panel801j.Visible = true;
        this.panelBoxes.Add(this.panel801j);
        this.comboBoxes.Add(this.cbo801j);
        this.borrowerBoxes.Add("1842");
        this.sellerBoxes.Add("1843");
      }
      else
        this.panel801j.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("i", this.dataObject))
      {
        ++num;
        this.panel801i.Visible = true;
        this.panelBoxes.Add(this.panel801i);
        this.comboBoxes.Add(this.cbo801i);
        this.borrowerBoxes.Add("1839");
        this.sellerBoxes.Add("1840");
      }
      else
        this.panel801i.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("h", this.dataObject))
      {
        ++num;
        this.panel801h.Visible = true;
        this.panelBoxes.Add(this.panel801h);
        this.comboBoxes.Add(this.cbo801h);
        this.borrowerBoxes.Add("1625");
        this.sellerBoxes.Add("1626");
      }
      else
        this.panel801h.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("g", this.dataObject))
      {
        ++num;
        this.panel801g.Visible = true;
        this.panelBoxes.Add(this.panel801g);
        this.comboBoxes.Add(this.cbo801g);
        this.borrowerBoxes.Add("155");
        this.sellerBoxes.Add("200");
      }
      else
        this.panel801g.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("f", this.dataObject))
      {
        ++num;
        this.panel801f.Visible = true;
        this.panelBoxes.Add(this.panel801f);
        this.comboBoxes.Add(this.cbo801f);
        this.borrowerBoxes.Add("NEWHUD.X225");
        this.sellerBoxes.Add("NEWHUD.X226");
      }
      else
        this.panel801f.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("e", this.dataObject))
      {
        ++num;
        this.panel801e.Visible = true;
        this.panelBoxes.Add(this.panel801e);
        this.comboBoxes.Add(this.cbo801e);
        this.borrowerBoxes.Add("439");
        this.sellerBoxes.Add("572");
      }
      else
        this.panel801e.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("d", this.dataObject))
      {
        ++num;
        this.panel801d.Visible = true;
        this.panelBoxes.Add(this.panel801d);
        this.comboBoxes.Add(this.cbo801d);
        this.borrowerBoxes.Add("367");
        this.sellerBoxes.Add("569");
      }
      else
        this.panel801d.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("c", this.dataObject))
      {
        ++num;
        this.panel801c.Visible = true;
        this.panelBoxes.Add(this.panel801c);
        this.comboBoxes.Add(this.cbo801c);
        this.borrowerBoxes.Add("1621");
        this.sellerBoxes.Add("1622");
      }
      else
        this.panel801c.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("b", this.dataObject))
      {
        ++num;
        this.panel801b.Visible = true;
        this.panelBoxes.Add(this.panel801b);
        this.comboBoxes.Add(this.cbo801b);
        this.borrowerBoxes.Add("L228");
        this.sellerBoxes.Add("L229");
      }
      else
        this.panel801b.Visible = false;
      if (this.loCompensationSetting.IsLineItemEnabled("a", this.dataObject))
      {
        ++num;
        this.panel801a.Visible = true;
        this.panelBoxes.Add(this.panel801a);
        this.comboBoxes.Add(this.cbo801a);
        this.borrowerBoxes.Add("454");
        this.sellerBoxes.Add("559");
      }
      else
        this.panel801a.Visible = false;
      this.groupContainer1.Height = num * this.panel801a.Height + 40;
      this.btnOK.Top = this.btnCancel.Top = this.groupContainer1.Top + this.groupContainer1.Height + 10;
      this.Height = this.btnOK.Top + this.btnOK.Height + 40;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.isCompensationViolation())
      {
        if (this.rdoToBorrower.Checked)
          this.rdoToBorrower_Click((object) null, (EventArgs) null);
        else if (this.rdoTo3rdParty.Checked)
          this.rdoTo3rdParty_Click((object) null, (EventArgs) null);
        else if (!this.rdoTakeNoAction.Checked)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please select an action to apply LO Compensation rule.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      if (!this.rdoTakeNoAction.Checked)
      {
        if (this.isCompensationViolation())
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "All compensation fields must be paid by the borrower or non-borrowers, but not both.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        Cursor.Current = Cursors.WaitCursor;
        for (int index = 0; index < this.panelBoxes.Count; ++index)
        {
          if (this.panelBoxes[index].Visible)
            this.setLoanFieldValue(this.panelBoxes[index].Controls);
        }
        if (this.dataObject is LoanData)
        {
          LoanData dataObject = (LoanData) this.dataObject;
          if (dataObject.Calculator != null)
          {
            if (this.updateGFE)
            {
              dataObject.Calculator.CopyHUD2010ToGFE2010("389", false);
              dataObject.Calculator.CopyHUD2010ToGFE2010("1620", false);
              dataObject.Calculator.CopyHUD2010ToGFE2010("NEWHUD.X223", false);
              dataObject.Calculator.CopyHUD2010ToGFE2010("NEWHUD.X224", false);
            }
            if (this.rdoToBorrower.Checked)
            {
              dataObject.SetField("NEWHUD2.X466", "");
              dataObject.SetField("NEWHUD2.X468", "");
              for (int index = 482; index <= 484; ++index)
                dataObject.SetField("NEWHUD2.X" + (object) index, "");
              for (int index = 489; index <= 494; ++index)
                dataObject.SetField("NEWHUD2.X" + (object) index, "");
              dataObject.SetField("NEWHUD2.X109", "");
            }
            else if (this.rdoTo3rdParty.Checked)
            {
              for (int index = 432; index <= 461; ++index)
              {
                if (index != 436 && index != 437)
                  dataObject.SetField("NEWHUD2.X" + (object) index, "");
              }
              dataObject.SetField("NEWHUD2.X108", "");
            }
            dataObject.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
          }
        }
        else if (this.dataObject is ClosingCost)
        {
          if (this.dataObject.GetField("NEWHUD.X223") != string.Empty || this.dataObject.GetField("NEWHUD.X224") != string.Empty)
          {
            this.dataObject.SetCurrentField("NEWHUD.X227", "Lender");
            this.dataObject.SetCurrentField("NEWHUD.X230", "Broker");
          }
          else
          {
            this.dataObject.SetCurrentField("NEWHUD.X227", "");
            this.dataObject.SetCurrentField("NEWHUD.X230", "");
          }
          this.dataObject.SetCurrentField("SYS.X265", "");
          if (this.dataObject.GetField("389") != string.Empty || this.dataObject.GetField("1620") != string.Empty)
            this.dataObject.SetCurrentField("SYS.X266", "Broker");
          else
            this.dataObject.SetCurrentField("SYS.X266", "");
        }
        Cursor.Current = Cursors.Default;
      }
      this.DialogResult = DialogResult.OK;
    }

    private bool isCompensationViolation()
    {
      ClosingCost dataObject = new ClosingCost();
      this.createIHtmlInputTable(this.Controls, (IHtmlInput) dataObject);
      dataObject.SetCurrentField("SYS.X252", this.dataObject.GetField("SYS.X252"));
      dataObject.SetCurrentField("SYS.X262", this.dataObject.GetField("SYS.X262"));
      dataObject.SetCurrentField("SYS.X270", this.dataObject.GetField("SYS.X270"));
      dataObject.SetCurrentField("SYS.X272", this.dataObject.GetField("SYS.X272"));
      dataObject.SetCurrentField("SYS.X266", this.dataObject.GetField("SYS.X266"));
      dataObject.SetCurrentField("NEWHUD.X230", this.dataObject.GetField("NEWHUD.X230"));
      dataObject.SetCurrentField("SYS.X290", this.dataObject.GetField("SYS.X290"));
      dataObject.SetCurrentField("SYS.X292", this.dataObject.GetField("SYS.X292"));
      dataObject.SetCurrentField("SYS.X297", this.dataObject.GetField("SYS.X297"));
      dataObject.SetCurrentField("SYS.X302", this.dataObject.GetField("SYS.X302"));
      dataObject.SetCurrentField("NEWHUD.X690", this.dataObject.GetField("NEWHUD.X690"));
      dataObject.SetCurrentField("NEWHUD.X1242", this.dataObject.GetField("NEWHUD.X1242"));
      dataObject.SetCurrentField("NEWHUD.X1250", this.dataObject.GetField("NEWHUD.X1250"));
      dataObject.SetCurrentField("NEWHUD.X1258", this.dataObject.GetField("NEWHUD.X1258"));
      dataObject.SetCurrentField("NEWHUD.X1266", this.dataObject.GetField("NEWHUD.X1266"));
      dataObject.SetCurrentField("NEWHUD.X1274", this.dataObject.GetField("NEWHUD.X1274"));
      dataObject.SetCurrentField("NEWHUD.X1282", this.dataObject.GetField("NEWHUD.X1282"));
      dataObject.SetCurrentField("NEWHUD.X1290", this.dataObject.GetField("NEWHUD.X1290"));
      dataObject.SetCurrentField("NEWHUD.X627", this.dataObject.GetField("NEWHUD.X627"));
      dataObject.SetCurrentField("NEWHUD.X715", this.dataObject.GetField("NEWHUD.X715"));
      dataObject.SetCurrentField("NEWHUD.X713", this.dataObject.GetField("NEWHUD.X713"));
      return this.loCompensationSetting.HasViolation((IHtmlInput) dataObject);
    }

    private void rdoToBorrower_Click(object sender, EventArgs e)
    {
      this.populateFieldValues(this.Controls, (string) null, (string) null);
      if (this.dataObject.GetField("NEWHUD.X223") != string.Empty)
        this.populateFieldValues(this.Controls, "NEWHUD.X223", "");
      if (this.dataObject.GetField("NEWHUD.X224") != string.Empty)
        this.populateFieldValues(this.Controls, "NEWHUD.X224", "");
      if (this.dataObject.GetField("NEWHUD.X225") != string.Empty)
        this.populateFieldValues(this.Controls, "NEWHUD.X225", "");
      if (this.dataObject.GetField("NEWHUD.X226") != string.Empty)
        this.populateFieldValues(this.Controls, "NEWHUD.X226", "");
      if (this.dataObject.GetField("NEWHUD.X227") != string.Empty)
        this.populateFieldValues(this.Controls, "NEWHUD.X227", "");
      if (this.dataObject.GetField("NEWHUD.X230") != string.Empty)
        this.populateFieldValues(this.Controls, "NEWHUD.X230", "");
      for (int index = 0; index < this.panelBoxes.Count; ++index)
      {
        if (this.panelBoxes[index].Visible)
        {
          double num1 = Utils.ParseDouble((object) (this.getFieldValue(this.panelBoxes[index].Controls, this.sellerBoxes[index]) ?? ""));
          double num2 = Utils.ParseDouble((object) (this.getFieldValue(this.panelBoxes[index].Controls, this.borrowerBoxes[index]) ?? ""));
          if (num1 != 0.0)
          {
            double num3 = num2 + num1;
            this.populateFieldValues(this.panelBoxes[index].Controls, this.borrowerBoxes[index], num3.ToString("N2"));
            this.populateFieldValues(this.Controls, this.sellerBoxes[index], "");
          }
          if (this.comboBoxes[index].Text != string.Empty)
            this.comboBoxes[index].Text = "";
          if (this.comboBoxes[index] != this.cbo801f)
            this.comboBoxes[index].Enabled = false;
        }
      }
    }

    private void rdoTo3rdParty_Click(object sender, EventArgs e)
    {
      this.populateFieldValues(this.Controls, (string) null, (string) null);
      if (this.dataObject.GetField("389") != string.Empty)
        this.populateFieldValues(this.Controls, "389", "");
      if (this.dataObject.GetField("1620") != string.Empty)
        this.populateFieldValues(this.Controls, "1620", "");
      if (this.dataObject.GetField("439") != string.Empty)
        this.populateFieldValues(this.Controls, "439", "");
      if (this.dataObject.GetField("572") != string.Empty)
        this.populateFieldValues(this.Controls, "572", "");
      if (this.dataObject.GetField("SYS.X265") != string.Empty)
        this.populateFieldValues(this.Controls, "SYS.X265", "");
      if (this.dataObject.GetField("SYS.X266") != string.Empty)
        this.populateFieldValues(this.Controls, "SYS.X266", "");
      string str = "L";
      if (this.cbo801f.Text != "L" && this.cbo801e.Text != string.Empty)
        str = this.cbo801e.Text;
      for (int index = 0; index < this.panelBoxes.Count; ++index)
      {
        if (this.panelBoxes[index].Visible)
        {
          double num1 = Utils.ParseDouble((object) (this.getFieldValue(this.panelBoxes[index].Controls, this.sellerBoxes[index]) ?? ""));
          double num2 = Utils.ParseDouble((object) (this.getFieldValue(this.panelBoxes[index].Controls, this.borrowerBoxes[index]) ?? ""));
          if (num1 == 0.0 || num2 != 0.0 || !(this.comboBoxes[index].Text == string.Empty))
          {
            if (num1 != 0.0 && num2 != 0.0 && this.comboBoxes[index].Text == string.Empty)
              this.comboBoxes[index].Text = str;
            else if (this.comboBoxes[index].Text == string.Empty)
            {
              if (this.panelBoxes[index] == this.panel801f)
                this.comboBoxes[index].Text = "L";
              else
                this.comboBoxes[index].Text = str;
            }
          }
          if (this.comboBoxes[index] != this.cbo801e && this.comboBoxes[index] != this.cbo801f)
            this.comboBoxes[index].Enabled = true;
        }
      }
    }

    private void rdoTakeNoAction_CheckedChanged(object sender, EventArgs e)
    {
      this.populateFieldValues(this.Controls, (string) null, (string) null);
    }

    private void setLoanFieldValue(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
          case ComboBox _:
            if (c.Tag != null)
            {
              string str = c.Tag.ToString();
              if (!(str == string.Empty))
              {
                string empty = string.Empty;
                string val;
                if (c is ComboBox)
                {
                  switch (c.Text)
                  {
                    case "B":
                      val = "Broker";
                      break;
                    case "L":
                      val = "Lender";
                      break;
                    case "O":
                      val = "Other";
                      break;
                    default:
                      val = "";
                      break;
                  }
                }
                else
                  val = c.Text;
                if (this.dataObject.GetField(str) != val)
                {
                  if (str == "NEWHUD.X227" && val == string.Empty)
                  {
                    if (this.dataObject.GetField("NEWHUD.X1718") == "Y")
                    {
                      this.dataObject.SetCurrentField("NEWHUD.X1141", "");
                      this.dataObject.SetCurrentField("NEWHUD.X1225", "");
                      this.dataObject.SetCurrentField("NEWHUD.X1142", "");
                      this.dataObject.SetCurrentField("NEWHUD.X1167", "");
                      this.dataObject.SetCurrentField("NEWHUD.X1168", "");
                      bool flag = false;
                      if (this.dataObject is LoanData)
                      {
                        if (((LoanData) this.dataObject).Calculator.IsSyncGFERequired)
                          flag = true;
                      }
                      else
                        flag = true;
                      if (flag)
                      {
                        this.dataObject.SetCurrentField("NEWHUD.X1200", "");
                        this.dataObject.SetCurrentField("NEWHUD.X1228", "");
                        this.dataObject.SetCurrentField("NEWHUD.X1201", "");
                      }
                    }
                    this.dataObject.SetCurrentField("NEWHUD.X1718", "");
                    this.dataObject.SetCurrentField("NEWHUD.X223", "");
                    this.dataObject.SetCurrentField("NEWHUD.X224", "");
                    this.dataObject.SetCurrentField("NEWHUD.X230", "");
                    this.dataObject.SetCurrentField("NEWHUD.X225", "");
                    this.dataObject.SetCurrentField("NEWHUD.X229", "");
                  }
                  this.dataObject.SetField(str, val);
                }
                if (c is ComboBox && str != string.Empty)
                {
                  this.setPTCPOC(str);
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          default:
            this.setLoanFieldValue(c.Controls);
            continue;
        }
      }
    }

    private void setPTCPOC(string paidByID)
    {
      if (!HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) paidByID))
        return;
      string[] strArray = (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) paidByID];
      if (strArray == null || strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO] == string.Empty || this.dataObject.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]) != "Broker")
        return;
      string field1 = this.dataObject.GetField(paidByID);
      double num1 = strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] != string.Empty ? Utils.ParseDouble((object) this.dataObject.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID])) : 0.0;
      double num2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT] != string.Empty ? Utils.ParseDouble((object) this.dataObject.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT])) : 0.0;
      double num3 = strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT] != string.Empty ? Utils.ParseDouble((object) this.dataObject.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT])) : 0.0;
      string field2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY] != string.Empty ? this.dataObject.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]) : "";
      string field3 = strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY] != string.Empty ? this.dataObject.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]) : "";
      LoanData dataObject = (LoanData) this.dataObject;
      if (field1 == "")
      {
        if (field2 != string.Empty && num2 != 0.0)
          this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], field1);
        if (field3 != string.Empty && num3 != 0.0)
        {
          this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], "");
          this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], "");
          this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY], num3.ToString("N2"));
        }
        if (!dataObject.Use2015RESPA || !this.rdoToBorrower.Checked)
          return;
        this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT], "");
        this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT], "");
        this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], "");
        this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT], "");
        this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT], "");
        this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT], "");
        dataObject.Calculator.Calculate2015FeeDetails(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY]);
      }
      else
      {
        if (field2 == string.Empty && num2 != 0.0)
          this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], field1);
        double num4 = num1 - num2;
        this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], num4 != 0.0 ? num4.ToString("N2") : "");
        this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY], "");
        if (field3 != field1 && num4 != 0.0)
          this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], field1);
        if (!dataObject.Use2015RESPA || !this.rdoTo3rdParty.Checked)
          return;
        double num5 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != string.Empty ? Utils.ParseDouble((object) this.dataObject.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT])) : (0.0.ToString() + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != string.Empty ? Utils.ParseDouble((object) this.dataObject.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT])) : (0.0.ToString() + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != string.Empty ? Utils.ParseDouble((object) this.dataObject.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT])) : 0.0));
        double num6 = num1 - num5;
        if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT] != "")
          this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT], "");
        if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "")
          this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT], "");
        switch (field1)
        {
          case "Lender":
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT], "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT], "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT], "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT], "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT], num5 != 0.0 ? num5.ToString("N2") : "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "")
            {
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], num6 != 0.0 ? num6.ToString("N2") : "");
              break;
            }
            break;
          case "Broker":
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT], "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT], "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT], "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT], num5 != 0.0 ? num5.ToString("N2") : "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT] != "")
            {
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT], num6 != 0.0 ? num6.ToString("N2") : "");
              break;
            }
            break;
          case "Other":
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT], "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT], "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT], "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT], num5 != 0.0 ? num5.ToString("N2") : "");
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT] != "")
            {
              this.dataObject.SetCurrentField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT], num6 != 0.0 ? num6.ToString("N2") : "");
              break;
            }
            break;
        }
        dataObject.Calculator.Calculate2015FeeDetails("", strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY]);
      }
    }

    private void populateFieldValues(
      Control.ControlCollection cs,
      string setFieldID,
      string setValue)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
          case ComboBox _:
            if (c.Tag != null)
            {
              string str = c.Tag.ToString();
              if (!(str == string.Empty) && (setFieldID == null || !(str != setFieldID)))
              {
                if (setFieldID != null && str == setFieldID)
                {
                  c.Text = setValue;
                  return;
                }
                if (c is ComboBox)
                {
                  switch (this.dataObject.GetField(str))
                  {
                    case "Broker":
                      c.Text = "B";
                      break;
                    case "Lender":
                      c.Text = "L";
                      break;
                    case "Other":
                      c.Text = "O";
                      break;
                    default:
                      c.Text = string.Empty;
                      break;
                  }
                }
                else
                  c.Text = this.dataObject.GetField(str);
                if (setFieldID == null && this.popupRules != null)
                {
                  this.popupRules.SetBusinessRules((object) c, str);
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          default:
            this.populateFieldValues(c.Controls, setFieldID, setValue);
            continue;
        }
      }
    }

    private string getFieldValue(Control.ControlCollection cs, string fieldID)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
          case ComboBox _:
            if (c.Tag != null)
            {
              string str = c.Tag.ToString();
              if (!(str == string.Empty) && !(str != fieldID))
                return c.Text;
              continue;
            }
            continue;
          default:
            string fieldValue = this.getFieldValue(c.Controls, fieldID);
            if (fieldValue != null)
              return fieldValue;
            continue;
        }
      }
      return (string) null;
    }

    private void createIHtmlInputTable(Control.ControlCollection cs, IHtmlInput dataObject)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
          case ComboBox _:
            if (c.Tag != null)
            {
              string id = c.Tag.ToString();
              if (!(id == string.Empty))
              {
                if (c is ComboBox && c.Text != string.Empty)
                {
                  switch (c.Text)
                  {
                    case "B":
                      dataObject.SetCurrentField(id, "Broker");
                      continue;
                    case "L":
                      dataObject.SetCurrentField(id, "Lender");
                      continue;
                    case "O":
                      dataObject.SetCurrentField(id, "Other");
                      continue;
                    default:
                      continue;
                  }
                }
                else
                {
                  dataObject.SetCurrentField(id, c.Text);
                  continue;
                }
              }
              else
                continue;
            }
            else
              continue;
          default:
            this.createIHtmlInputTable(c.Controls, dataObject);
            continue;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LOCompensationViolationDialog));
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.label3 = new Label();
      this.rdoToBorrower = new RadioButton();
      this.rdoTo3rdParty = new RadioButton();
      this.rdoTakeNoAction = new RadioButton();
      this.btnOK = new Button();
      this.groupContainer1 = new GroupContainer();
      this.panel801s = new Panel();
      this.cbo801s = new ComboBox();
      this.textBox35 = new TextBox();
      this.pictureBox19 = new PictureBox();
      this.label26 = new Label();
      this.textBox33 = new TextBox();
      this.label29 = new Label();
      this.label28 = new Label();
      this.textBox34 = new TextBox();
      this.textBox36 = new TextBox();
      this.label27 = new Label();
      this.panel801r = new Panel();
      this.pictureBox18 = new PictureBox();
      this.textBox55 = new TextBox();
      this.textBox56 = new TextBox();
      this.textBox57 = new TextBox();
      this.cbo801r = new ComboBox();
      this.label38 = new Label();
      this.panel801q = new Panel();
      this.pictureBox17 = new PictureBox();
      this.textBox52 = new TextBox();
      this.textBox53 = new TextBox();
      this.textBox54 = new TextBox();
      this.cbo801q = new ComboBox();
      this.label37 = new Label();
      this.panel801p = new Panel();
      this.pictureBox16 = new PictureBox();
      this.textBox49 = new TextBox();
      this.textBox50 = new TextBox();
      this.textBox51 = new TextBox();
      this.cbo801p = new ComboBox();
      this.label36 = new Label();
      this.panel801o = new Panel();
      this.pictureBox15 = new PictureBox();
      this.textBox46 = new TextBox();
      this.textBox47 = new TextBox();
      this.textBox48 = new TextBox();
      this.cbo801o = new ComboBox();
      this.label35 = new Label();
      this.panel801n = new Panel();
      this.pictureBox14 = new PictureBox();
      this.textBox43 = new TextBox();
      this.textBox44 = new TextBox();
      this.textBox45 = new TextBox();
      this.cbo801n = new ComboBox();
      this.label34 = new Label();
      this.panel801m = new Panel();
      this.pictureBox13 = new PictureBox();
      this.textBox40 = new TextBox();
      this.textBox41 = new TextBox();
      this.textBox42 = new TextBox();
      this.cbo801m = new ComboBox();
      this.label33 = new Label();
      this.label31 = new Label();
      this.label30 = new Label();
      this.panel801l = new Panel();
      this.pictureBox12 = new PictureBox();
      this.textBox37 = new TextBox();
      this.textBox38 = new TextBox();
      this.textBox39 = new TextBox();
      this.cbo801l = new ComboBox();
      this.label32 = new Label();
      this.panel801k = new Panel();
      this.pictureBox11 = new PictureBox();
      this.textBox32 = new TextBox();
      this.textBox16 = new TextBox();
      this.textBox11 = new TextBox();
      this.cbo801k = new ComboBox();
      this.label25 = new Label();
      this.panel801j = new Panel();
      this.pictureBox10 = new PictureBox();
      this.textBox31 = new TextBox();
      this.textBox15 = new TextBox();
      this.textBox10 = new TextBox();
      this.cbo801j = new ComboBox();
      this.label23 = new Label();
      this.panel801i = new Panel();
      this.pictureBox9 = new PictureBox();
      this.textBox30 = new TextBox();
      this.textBox14 = new TextBox();
      this.textBox9 = new TextBox();
      this.cbo801i = new ComboBox();
      this.label21 = new Label();
      this.panel801h = new Panel();
      this.pictureBox8 = new PictureBox();
      this.textBox29 = new TextBox();
      this.textBox13 = new TextBox();
      this.textBox8 = new TextBox();
      this.cbo801h = new ComboBox();
      this.label19 = new Label();
      this.panel801g = new Panel();
      this.pictureBox7 = new PictureBox();
      this.textBox28 = new TextBox();
      this.textBox12 = new TextBox();
      this.textBox7 = new TextBox();
      this.cbo801g = new ComboBox();
      this.label17 = new Label();
      this.panel801f = new Panel();
      this.pictureBox6 = new PictureBox();
      this.textBox27 = new TextBox();
      this.textBox19 = new TextBox();
      this.label20 = new Label();
      this.textBox20 = new TextBox();
      this.label22 = new Label();
      this.cbo801f = new ComboBox();
      this.textBox6 = new TextBox();
      this.label15 = new Label();
      this.label16 = new Label();
      this.panel801e = new Panel();
      this.pictureBox5 = new PictureBox();
      this.textBox26 = new TextBox();
      this.textBox18 = new TextBox();
      this.label18 = new Label();
      this.textBox17 = new TextBox();
      this.label2 = new Label();
      this.cbo801e = new ComboBox();
      this.textBox5 = new TextBox();
      this.label13 = new Label();
      this.label14 = new Label();
      this.panel801d = new Panel();
      this.pictureBox4 = new PictureBox();
      this.textBox25 = new TextBox();
      this.cbo801d = new ComboBox();
      this.textBox4 = new TextBox();
      this.label11 = new Label();
      this.label12 = new Label();
      this.panel801c = new Panel();
      this.pictureBox3 = new PictureBox();
      this.textBox24 = new TextBox();
      this.cbo801c = new ComboBox();
      this.textBox3 = new TextBox();
      this.label9 = new Label();
      this.label10 = new Label();
      this.panel801b = new Panel();
      this.pictureBox2 = new PictureBox();
      this.textBox23 = new TextBox();
      this.cbo801b = new ComboBox();
      this.label8 = new Label();
      this.label7 = new Label();
      this.textBox2 = new TextBox();
      this.panel801a = new Panel();
      this.textBox22 = new TextBox();
      this.pictureBox1 = new PictureBox();
      this.textBox21 = new TextBox();
      this.label24 = new Label();
      this.cbo801a = new ComboBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.textBox1 = new TextBox();
      this.panel3 = new Panel();
      this.label6 = new Label();
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.panel1 = new Panel();
      this.groupContainer1.SuspendLayout();
      this.panel801s.SuspendLayout();
      ((ISupportInitialize) this.pictureBox19).BeginInit();
      this.panel801r.SuspendLayout();
      ((ISupportInitialize) this.pictureBox18).BeginInit();
      this.panel801q.SuspendLayout();
      ((ISupportInitialize) this.pictureBox17).BeginInit();
      this.panel801p.SuspendLayout();
      ((ISupportInitialize) this.pictureBox16).BeginInit();
      this.panel801o.SuspendLayout();
      ((ISupportInitialize) this.pictureBox15).BeginInit();
      this.panel801n.SuspendLayout();
      ((ISupportInitialize) this.pictureBox14).BeginInit();
      this.panel801m.SuspendLayout();
      ((ISupportInitialize) this.pictureBox13).BeginInit();
      this.panel801l.SuspendLayout();
      ((ISupportInitialize) this.pictureBox12).BeginInit();
      this.panel801k.SuspendLayout();
      ((ISupportInitialize) this.pictureBox11).BeginInit();
      this.panel801j.SuspendLayout();
      ((ISupportInitialize) this.pictureBox10).BeginInit();
      this.panel801i.SuspendLayout();
      ((ISupportInitialize) this.pictureBox9).BeginInit();
      this.panel801h.SuspendLayout();
      ((ISupportInitialize) this.pictureBox8).BeginInit();
      this.panel801g.SuspendLayout();
      ((ISupportInitialize) this.pictureBox7).BeginInit();
      this.panel801f.SuspendLayout();
      ((ISupportInitialize) this.pictureBox6).BeginInit();
      this.panel801e.SuspendLayout();
      ((ISupportInitialize) this.pictureBox5).BeginInit();
      this.panel801d.SuspendLayout();
      ((ISupportInitialize) this.pictureBox4).BeginInit();
      this.panel801c.SuspendLayout();
      ((ISupportInitialize) this.pictureBox3).BeginInit();
      this.panel801b.SuspendLayout();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      this.panel801a.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(709, 698);
      this.btnCancel.Margin = new Padding(4, 4, 4, 4);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(100, 28);
      this.btnCancel.TabIndex = 25;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label1.Location = new Point(93, 16);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(715, 60);
      this.label1.TabIndex = 1;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.label3.AutoSize = true;
      this.label3.Location = new Point(93, 76);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(195, 17);
      this.label3.TabIndex = 3;
      this.label3.Text = "How do you want to proceed?";
      this.rdoToBorrower.AutoSize = true;
      this.rdoToBorrower.Checked = true;
      this.rdoToBorrower.Location = new Point(97, 100);
      this.rdoToBorrower.Margin = new Padding(4, 4, 4, 4);
      this.rdoToBorrower.Name = "rdoToBorrower";
      this.rdoToBorrower.Size = new Size(193, 21);
      this.rdoToBorrower.TabIndex = 1;
      this.rdoToBorrower.TabStop = true;
      this.rdoToBorrower.Text = "Make all Paid by Borrower";
      this.rdoToBorrower.UseVisualStyleBackColor = true;
      this.rdoToBorrower.Click += new EventHandler(this.rdoToBorrower_Click);
      this.rdoTo3rdParty.AutoSize = true;
      this.rdoTo3rdParty.Location = new Point(304, 100);
      this.rdoTo3rdParty.Margin = new Padding(4, 4, 4, 4);
      this.rdoTo3rdParty.Name = "rdoTo3rdParty";
      this.rdoTo3rdParty.Size = new Size(194, 21);
      this.rdoTo3rdParty.TabIndex = 2;
      this.rdoTo3rdParty.Text = "Make all Paid by 3rd Party";
      this.rdoTo3rdParty.UseVisualStyleBackColor = true;
      this.rdoTo3rdParty.Click += new EventHandler(this.rdoTo3rdParty_Click);
      this.rdoTakeNoAction.AutoSize = true;
      this.rdoTakeNoAction.Location = new Point(536, 100);
      this.rdoTakeNoAction.Margin = new Padding(4, 4, 4, 4);
      this.rdoTakeNoAction.Name = "rdoTakeNoAction";
      this.rdoTakeNoAction.Size = new Size(123, 21);
      this.rdoTakeNoAction.TabIndex = 3;
      this.rdoTakeNoAction.Text = "Take no action";
      this.rdoTakeNoAction.UseVisualStyleBackColor = true;
      this.rdoTakeNoAction.CheckedChanged += new EventHandler(this.rdoTakeNoAction_CheckedChanged);
      this.btnOK.Location = new Point(601, 698);
      this.btnOK.Margin = new Padding(4, 4, 4, 4);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(100, 28);
      this.btnOK.TabIndex = 24;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.groupContainer1.Controls.Add((Control) this.panel801s);
      this.groupContainer1.Controls.Add((Control) this.panel801r);
      this.groupContainer1.Controls.Add((Control) this.panel801q);
      this.groupContainer1.Controls.Add((Control) this.panel801p);
      this.groupContainer1.Controls.Add((Control) this.panel801o);
      this.groupContainer1.Controls.Add((Control) this.panel801n);
      this.groupContainer1.Controls.Add((Control) this.panel801m);
      this.groupContainer1.Controls.Add((Control) this.label31);
      this.groupContainer1.Controls.Add((Control) this.label30);
      this.groupContainer1.Controls.Add((Control) this.panel801l);
      this.groupContainer1.Controls.Add((Control) this.panel801k);
      this.groupContainer1.Controls.Add((Control) this.panel801j);
      this.groupContainer1.Controls.Add((Control) this.panel801i);
      this.groupContainer1.Controls.Add((Control) this.panel801h);
      this.groupContainer1.Controls.Add((Control) this.panel801g);
      this.groupContainer1.Controls.Add((Control) this.panel801f);
      this.groupContainer1.Controls.Add((Control) this.panel801e);
      this.groupContainer1.Controls.Add((Control) this.panel801d);
      this.groupContainer1.Controls.Add((Control) this.panel801c);
      this.groupContainer1.Controls.Add((Control) this.panel801b);
      this.groupContainer1.Controls.Add((Control) this.panel801a);
      this.groupContainer1.Controls.Add((Control) this.panel3);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(97, 128);
      this.groupContainer1.Margin = new Padding(4, 4, 4, 4);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(712, 562);
      this.groupContainer1.TabIndex = 4;
      this.groupContainer1.Text = "Compensation Fields";
      this.panel801s.Controls.Add((Control) this.cbo801s);
      this.panel801s.Controls.Add((Control) this.textBox35);
      this.panel801s.Controls.Add((Control) this.pictureBox19);
      this.panel801s.Controls.Add((Control) this.label26);
      this.panel801s.Controls.Add((Control) this.textBox33);
      this.panel801s.Controls.Add((Control) this.label29);
      this.panel801s.Controls.Add((Control) this.label28);
      this.panel801s.Controls.Add((Control) this.textBox34);
      this.panel801s.Controls.Add((Control) this.textBox36);
      this.panel801s.Controls.Add((Control) this.label27);
      this.panel801s.Dock = DockStyle.Top;
      this.panel801s.Location = new Point(1, 524);
      this.panel801s.Margin = new Padding(4, 4, 4, 4);
      this.panel801s.Name = "panel801s";
      this.panel801s.Size = new Size(710, 27);
      this.panel801s.TabIndex = 23;
      this.cbo801s.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801s.FormattingEnabled = true;
      this.cbo801s.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801s.Location = new Point(633, 0);
      this.cbo801s.Margin = new Padding(4, 4, 4, 4);
      this.cbo801s.Name = "cbo801s";
      this.cbo801s.Size = new Size(60, 24);
      this.cbo801s.TabIndex = 23;
      this.cbo801s.Tag = (object) "NEWHUD.X749";
      this.textBox35.Location = new Point(220, 0);
      this.textBox35.Margin = new Padding(4, 4, 4, 4);
      this.textBox35.Name = "textBox35";
      this.textBox35.ReadOnly = true;
      this.textBox35.Size = new Size(55, 22);
      this.textBox35.TabIndex = 80;
      this.textBox35.TabStop = false;
      this.textBox35.Tag = (object) "1061";
      this.textBox35.TextAlign = HorizontalAlignment.Right;
      this.pictureBox19.Image = (Image) Resources.compensation;
      this.pictureBox19.Location = new Point(17, 2);
      this.pictureBox19.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox19.Name = "pictureBox19";
      this.pictureBox19.Size = new Size(16, 16);
      this.pictureBox19.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox19.TabIndex = 74;
      this.pictureBox19.TabStop = false;
      this.label26.AutoSize = true;
      this.label26.Location = new Point(307, 5);
      this.label26.Margin = new Padding(4, 0, 4, 0);
      this.label26.Name = "label26";
      this.label26.Size = new Size(16, 17);
      this.label26.TabIndex = 82;
      this.label26.Text = "$";
      this.textBox33.Location = new Point(525, 0);
      this.textBox33.Margin = new Padding(4, 4, 4, 4);
      this.textBox33.Name = "textBox33";
      this.textBox33.ReadOnly = true;
      this.textBox33.Size = new Size(89, 22);
      this.textBox33.TabIndex = 83;
      this.textBox33.TabStop = false;
      this.textBox33.Tag = (object) "NEWHUD.X788";
      this.textBox33.TextAlign = HorizontalAlignment.Right;
      this.label29.AutoSize = true;
      this.label29.Location = new Point(41, 5);
      this.label29.Margin = new Padding(4, 0, 4, 0);
      this.label29.Name = "label29";
      this.label29.Size = new Size(120, 17);
      this.label29.TabIndex = 75;
      this.label29.Text = "Origination Points";
      this.label28.AutoSize = true;
      this.label28.Location = new Point(409, 5);
      this.label28.Margin = new Padding(4, 0, 4, 0);
      this.label28.Name = "label28";
      this.label28.Size = new Size(16, 17);
      this.label28.TabIndex = 77;
      this.label28.Text = "$";
      this.textBox34.Location = new Point(324, 0);
      this.textBox34.Margin = new Padding(4, 4, 4, 4);
      this.textBox34.Name = "textBox34";
      this.textBox34.ReadOnly = true;
      this.textBox34.Size = new Size(72, 22);
      this.textBox34.TabIndex = 81;
      this.textBox34.TabStop = false;
      this.textBox34.Tag = (object) "436";
      this.textBox34.TextAlign = HorizontalAlignment.Right;
      this.textBox36.Location = new Point(429, 0);
      this.textBox36.Margin = new Padding(4, 4, 4, 4);
      this.textBox36.Name = "textBox36";
      this.textBox36.ReadOnly = true;
      this.textBox36.Size = new Size(89, 22);
      this.textBox36.TabIndex = 76;
      this.textBox36.TabStop = false;
      this.textBox36.Tag = (object) "NEWHUD.X15";
      this.textBox36.TextAlign = HorizontalAlignment.Right;
      this.label27.AutoSize = true;
      this.label27.Location = new Point(276, 5);
      this.label27.Margin = new Padding(4, 0, 4, 0);
      this.label27.Name = "label27";
      this.label27.Size = new Size(32, 17);
      this.label27.TabIndex = 79;
      this.label27.Text = "% +";
      this.panel801r.Controls.Add((Control) this.pictureBox18);
      this.panel801r.Controls.Add((Control) this.textBox55);
      this.panel801r.Controls.Add((Control) this.textBox56);
      this.panel801r.Controls.Add((Control) this.textBox57);
      this.panel801r.Controls.Add((Control) this.cbo801r);
      this.panel801r.Controls.Add((Control) this.label38);
      this.panel801r.Dock = DockStyle.Top;
      this.panel801r.Location = new Point(1, 497);
      this.panel801r.Margin = new Padding(4, 4, 4, 4);
      this.panel801r.Name = "panel801r";
      this.panel801r.Size = new Size(710, 27);
      this.panel801r.TabIndex = 22;
      this.pictureBox18.Image = (Image) Resources.compensation;
      this.pictureBox18.Location = new Point(17, 2);
      this.pictureBox18.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox18.Name = "pictureBox18";
      this.pictureBox18.Size = new Size(16, 16);
      this.pictureBox18.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox18.TabIndex = 79;
      this.pictureBox18.TabStop = false;
      this.textBox55.Location = new Point(525, 0);
      this.textBox55.Margin = new Padding(4, 4, 4, 4);
      this.textBox55.Name = "textBox55";
      this.textBox55.ReadOnly = true;
      this.textBox55.Size = new Size(89, 22);
      this.textBox55.TabIndex = 78;
      this.textBox55.TabStop = false;
      this.textBox55.Tag = (object) "NEWHUD.X1286";
      this.textBox55.TextAlign = HorizontalAlignment.Right;
      this.textBox56.Location = new Point(45, 0);
      this.textBox56.Margin = new Padding(4, 4, 4, 4);
      this.textBox56.Name = "textBox56";
      this.textBox56.ReadOnly = true;
      this.textBox56.Size = new Size(165, 22);
      this.textBox56.TabIndex = 76;
      this.textBox56.TabStop = false;
      this.textBox56.Tag = (object) "NEWHUD.X1283";
      this.textBox57.Location = new Point(429, 0);
      this.textBox57.Margin = new Padding(4, 4, 4, 4);
      this.textBox57.Name = "textBox57";
      this.textBox57.ReadOnly = true;
      this.textBox57.Size = new Size(89, 22);
      this.textBox57.TabIndex = 74;
      this.textBox57.TabStop = false;
      this.textBox57.Tag = (object) "NEWHUD.X1285";
      this.textBox57.TextAlign = HorizontalAlignment.Right;
      this.cbo801r.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801r.FormattingEnabled = true;
      this.cbo801r.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801r.Location = new Point(633, 0);
      this.cbo801r.Margin = new Padding(4, 4, 4, 4);
      this.cbo801r.Name = "cbo801r";
      this.cbo801r.Size = new Size(60, 24);
      this.cbo801r.TabIndex = 22;
      this.cbo801r.Tag = (object) "NEWHUD.X1287";
      this.label38.AutoSize = true;
      this.label38.Location = new Point(409, 5);
      this.label38.Margin = new Padding(4, 0, 4, 0);
      this.label38.Name = "label38";
      this.label38.Size = new Size(16, 17);
      this.label38.TabIndex = 75;
      this.label38.Text = "$";
      this.panel801q.Controls.Add((Control) this.pictureBox17);
      this.panel801q.Controls.Add((Control) this.textBox52);
      this.panel801q.Controls.Add((Control) this.textBox53);
      this.panel801q.Controls.Add((Control) this.textBox54);
      this.panel801q.Controls.Add((Control) this.cbo801q);
      this.panel801q.Controls.Add((Control) this.label37);
      this.panel801q.Dock = DockStyle.Top;
      this.panel801q.Location = new Point(1, 470);
      this.panel801q.Margin = new Padding(4, 4, 4, 4);
      this.panel801q.Name = "panel801q";
      this.panel801q.Size = new Size(710, 27);
      this.panel801q.TabIndex = 21;
      this.pictureBox17.Image = (Image) Resources.compensation;
      this.pictureBox17.Location = new Point(17, 2);
      this.pictureBox17.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox17.Name = "pictureBox17";
      this.pictureBox17.Size = new Size(16, 16);
      this.pictureBox17.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox17.TabIndex = 79;
      this.pictureBox17.TabStop = false;
      this.textBox52.Location = new Point(525, 0);
      this.textBox52.Margin = new Padding(4, 4, 4, 4);
      this.textBox52.Name = "textBox52";
      this.textBox52.ReadOnly = true;
      this.textBox52.Size = new Size(89, 22);
      this.textBox52.TabIndex = 78;
      this.textBox52.TabStop = false;
      this.textBox52.Tag = (object) "NEWHUD.X1278";
      this.textBox52.TextAlign = HorizontalAlignment.Right;
      this.textBox53.Location = new Point(45, 0);
      this.textBox53.Margin = new Padding(4, 4, 4, 4);
      this.textBox53.Name = "textBox53";
      this.textBox53.ReadOnly = true;
      this.textBox53.Size = new Size(165, 22);
      this.textBox53.TabIndex = 76;
      this.textBox53.TabStop = false;
      this.textBox53.Tag = (object) "NEWHUD.X1275";
      this.textBox54.Location = new Point(429, 0);
      this.textBox54.Margin = new Padding(4, 4, 4, 4);
      this.textBox54.Name = "textBox54";
      this.textBox54.ReadOnly = true;
      this.textBox54.Size = new Size(89, 22);
      this.textBox54.TabIndex = 74;
      this.textBox54.TabStop = false;
      this.textBox54.Tag = (object) "NEWHUD.X1277";
      this.textBox54.TextAlign = HorizontalAlignment.Right;
      this.cbo801q.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801q.FormattingEnabled = true;
      this.cbo801q.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801q.Location = new Point(633, 0);
      this.cbo801q.Margin = new Padding(4, 4, 4, 4);
      this.cbo801q.Name = "cbo801q";
      this.cbo801q.Size = new Size(60, 24);
      this.cbo801q.TabIndex = 21;
      this.cbo801q.Tag = (object) "NEWHUD.X1279";
      this.label37.AutoSize = true;
      this.label37.Location = new Point(409, 5);
      this.label37.Margin = new Padding(4, 0, 4, 0);
      this.label37.Name = "label37";
      this.label37.Size = new Size(16, 17);
      this.label37.TabIndex = 75;
      this.label37.Text = "$";
      this.panel801p.Controls.Add((Control) this.pictureBox16);
      this.panel801p.Controls.Add((Control) this.textBox49);
      this.panel801p.Controls.Add((Control) this.textBox50);
      this.panel801p.Controls.Add((Control) this.textBox51);
      this.panel801p.Controls.Add((Control) this.cbo801p);
      this.panel801p.Controls.Add((Control) this.label36);
      this.panel801p.Dock = DockStyle.Top;
      this.panel801p.Location = new Point(1, 443);
      this.panel801p.Margin = new Padding(4, 4, 4, 4);
      this.panel801p.Name = "panel801p";
      this.panel801p.Size = new Size(710, 27);
      this.panel801p.TabIndex = 20;
      this.pictureBox16.Image = (Image) Resources.compensation;
      this.pictureBox16.Location = new Point(17, 2);
      this.pictureBox16.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox16.Name = "pictureBox16";
      this.pictureBox16.Size = new Size(16, 16);
      this.pictureBox16.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox16.TabIndex = 79;
      this.pictureBox16.TabStop = false;
      this.textBox49.Location = new Point(525, 0);
      this.textBox49.Margin = new Padding(4, 4, 4, 4);
      this.textBox49.Name = "textBox49";
      this.textBox49.ReadOnly = true;
      this.textBox49.Size = new Size(89, 22);
      this.textBox49.TabIndex = 78;
      this.textBox49.TabStop = false;
      this.textBox49.Tag = (object) "NEWHUD.X1270";
      this.textBox49.TextAlign = HorizontalAlignment.Right;
      this.textBox50.Location = new Point(45, 0);
      this.textBox50.Margin = new Padding(4, 4, 4, 4);
      this.textBox50.Name = "textBox50";
      this.textBox50.ReadOnly = true;
      this.textBox50.Size = new Size(165, 22);
      this.textBox50.TabIndex = 76;
      this.textBox50.TabStop = false;
      this.textBox50.Tag = (object) "NEWHUD.X1267";
      this.textBox51.Location = new Point(429, 0);
      this.textBox51.Margin = new Padding(4, 4, 4, 4);
      this.textBox51.Name = "textBox51";
      this.textBox51.ReadOnly = true;
      this.textBox51.Size = new Size(89, 22);
      this.textBox51.TabIndex = 74;
      this.textBox51.TabStop = false;
      this.textBox51.Tag = (object) "NEWHUD.X1269";
      this.textBox51.TextAlign = HorizontalAlignment.Right;
      this.cbo801p.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801p.FormattingEnabled = true;
      this.cbo801p.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801p.Location = new Point(633, 0);
      this.cbo801p.Margin = new Padding(4, 4, 4, 4);
      this.cbo801p.Name = "cbo801p";
      this.cbo801p.Size = new Size(60, 24);
      this.cbo801p.TabIndex = 20;
      this.cbo801p.Tag = (object) "NEWHUD.X1271";
      this.label36.AutoSize = true;
      this.label36.Location = new Point(409, 5);
      this.label36.Margin = new Padding(4, 0, 4, 0);
      this.label36.Name = "label36";
      this.label36.Size = new Size(16, 17);
      this.label36.TabIndex = 75;
      this.label36.Text = "$";
      this.panel801o.Controls.Add((Control) this.pictureBox15);
      this.panel801o.Controls.Add((Control) this.textBox46);
      this.panel801o.Controls.Add((Control) this.textBox47);
      this.panel801o.Controls.Add((Control) this.textBox48);
      this.panel801o.Controls.Add((Control) this.cbo801o);
      this.panel801o.Controls.Add((Control) this.label35);
      this.panel801o.Dock = DockStyle.Top;
      this.panel801o.Location = new Point(1, 416);
      this.panel801o.Margin = new Padding(4, 4, 4, 4);
      this.panel801o.Name = "panel801o";
      this.panel801o.Size = new Size(710, 27);
      this.panel801o.TabIndex = 19;
      this.pictureBox15.Image = (Image) Resources.compensation;
      this.pictureBox15.Location = new Point(17, 2);
      this.pictureBox15.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox15.Name = "pictureBox15";
      this.pictureBox15.Size = new Size(16, 16);
      this.pictureBox15.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox15.TabIndex = 79;
      this.pictureBox15.TabStop = false;
      this.textBox46.Location = new Point(525, 0);
      this.textBox46.Margin = new Padding(4, 4, 4, 4);
      this.textBox46.Name = "textBox46";
      this.textBox46.ReadOnly = true;
      this.textBox46.Size = new Size(89, 22);
      this.textBox46.TabIndex = 78;
      this.textBox46.TabStop = false;
      this.textBox46.Tag = (object) "NEWHUD.X1262";
      this.textBox46.TextAlign = HorizontalAlignment.Right;
      this.textBox47.Location = new Point(45, 0);
      this.textBox47.Margin = new Padding(4, 4, 4, 4);
      this.textBox47.Name = "textBox47";
      this.textBox47.ReadOnly = true;
      this.textBox47.Size = new Size(165, 22);
      this.textBox47.TabIndex = 76;
      this.textBox47.TabStop = false;
      this.textBox47.Tag = (object) "NEWHUD.X1259";
      this.textBox48.Location = new Point(429, 0);
      this.textBox48.Margin = new Padding(4, 4, 4, 4);
      this.textBox48.Name = "textBox48";
      this.textBox48.ReadOnly = true;
      this.textBox48.Size = new Size(89, 22);
      this.textBox48.TabIndex = 74;
      this.textBox48.TabStop = false;
      this.textBox48.Tag = (object) "NEWHUD.X1261";
      this.textBox48.TextAlign = HorizontalAlignment.Right;
      this.cbo801o.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801o.FormattingEnabled = true;
      this.cbo801o.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801o.Location = new Point(633, 0);
      this.cbo801o.Margin = new Padding(4, 4, 4, 4);
      this.cbo801o.Name = "cbo801o";
      this.cbo801o.Size = new Size(60, 24);
      this.cbo801o.TabIndex = 19;
      this.cbo801o.Tag = (object) "NEWHUD.X1263";
      this.label35.AutoSize = true;
      this.label35.Location = new Point(409, 5);
      this.label35.Margin = new Padding(4, 0, 4, 0);
      this.label35.Name = "label35";
      this.label35.Size = new Size(16, 17);
      this.label35.TabIndex = 75;
      this.label35.Text = "$";
      this.panel801n.Controls.Add((Control) this.pictureBox14);
      this.panel801n.Controls.Add((Control) this.textBox43);
      this.panel801n.Controls.Add((Control) this.textBox44);
      this.panel801n.Controls.Add((Control) this.textBox45);
      this.panel801n.Controls.Add((Control) this.cbo801n);
      this.panel801n.Controls.Add((Control) this.label34);
      this.panel801n.Dock = DockStyle.Top;
      this.panel801n.Location = new Point(1, 389);
      this.panel801n.Margin = new Padding(4, 4, 4, 4);
      this.panel801n.Name = "panel801n";
      this.panel801n.Size = new Size(710, 27);
      this.panel801n.TabIndex = 18;
      this.pictureBox14.Image = (Image) Resources.compensation;
      this.pictureBox14.Location = new Point(17, 2);
      this.pictureBox14.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox14.Name = "pictureBox14";
      this.pictureBox14.Size = new Size(16, 16);
      this.pictureBox14.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox14.TabIndex = 79;
      this.pictureBox14.TabStop = false;
      this.textBox43.Location = new Point(525, 0);
      this.textBox43.Margin = new Padding(4, 4, 4, 4);
      this.textBox43.Name = "textBox43";
      this.textBox43.ReadOnly = true;
      this.textBox43.Size = new Size(89, 22);
      this.textBox43.TabIndex = 78;
      this.textBox43.TabStop = false;
      this.textBox43.Tag = (object) "NEWHUD.X1254";
      this.textBox43.TextAlign = HorizontalAlignment.Right;
      this.textBox44.Location = new Point(45, 0);
      this.textBox44.Margin = new Padding(4, 4, 4, 4);
      this.textBox44.Name = "textBox44";
      this.textBox44.ReadOnly = true;
      this.textBox44.Size = new Size(165, 22);
      this.textBox44.TabIndex = 76;
      this.textBox44.TabStop = false;
      this.textBox44.Tag = (object) "NEWHUD.X1251";
      this.textBox45.Location = new Point(429, 0);
      this.textBox45.Margin = new Padding(4, 4, 4, 4);
      this.textBox45.Name = "textBox45";
      this.textBox45.ReadOnly = true;
      this.textBox45.Size = new Size(89, 22);
      this.textBox45.TabIndex = 74;
      this.textBox45.TabStop = false;
      this.textBox45.Tag = (object) "NEWHUD.X1253";
      this.textBox45.TextAlign = HorizontalAlignment.Right;
      this.cbo801n.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801n.FormattingEnabled = true;
      this.cbo801n.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801n.Location = new Point(633, 0);
      this.cbo801n.Margin = new Padding(4, 4, 4, 4);
      this.cbo801n.Name = "cbo801n";
      this.cbo801n.Size = new Size(60, 24);
      this.cbo801n.TabIndex = 18;
      this.cbo801n.Tag = (object) "NEWHUD.X1255";
      this.label34.AutoSize = true;
      this.label34.Location = new Point(409, 5);
      this.label34.Margin = new Padding(4, 0, 4, 0);
      this.label34.Name = "label34";
      this.label34.Size = new Size(16, 17);
      this.label34.TabIndex = 75;
      this.label34.Text = "$";
      this.panel801m.Controls.Add((Control) this.pictureBox13);
      this.panel801m.Controls.Add((Control) this.textBox40);
      this.panel801m.Controls.Add((Control) this.textBox41);
      this.panel801m.Controls.Add((Control) this.textBox42);
      this.panel801m.Controls.Add((Control) this.cbo801m);
      this.panel801m.Controls.Add((Control) this.label33);
      this.panel801m.Dock = DockStyle.Top;
      this.panel801m.Location = new Point(1, 362);
      this.panel801m.Margin = new Padding(4, 4, 4, 4);
      this.panel801m.Name = "panel801m";
      this.panel801m.Size = new Size(710, 27);
      this.panel801m.TabIndex = 17;
      this.pictureBox13.Image = (Image) Resources.compensation;
      this.pictureBox13.Location = new Point(17, 2);
      this.pictureBox13.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox13.Name = "pictureBox13";
      this.pictureBox13.Size = new Size(16, 16);
      this.pictureBox13.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox13.TabIndex = 79;
      this.pictureBox13.TabStop = false;
      this.textBox40.Location = new Point(525, 0);
      this.textBox40.Margin = new Padding(4, 4, 4, 4);
      this.textBox40.Name = "textBox40";
      this.textBox40.ReadOnly = true;
      this.textBox40.Size = new Size(89, 22);
      this.textBox40.TabIndex = 78;
      this.textBox40.TabStop = false;
      this.textBox40.Tag = (object) "NEWHUD.X1246";
      this.textBox40.TextAlign = HorizontalAlignment.Right;
      this.textBox41.Location = new Point(45, 0);
      this.textBox41.Margin = new Padding(4, 4, 4, 4);
      this.textBox41.Name = "textBox41";
      this.textBox41.ReadOnly = true;
      this.textBox41.Size = new Size(165, 22);
      this.textBox41.TabIndex = 76;
      this.textBox41.TabStop = false;
      this.textBox41.Tag = (object) "NEWHUD.X1243";
      this.textBox42.Location = new Point(429, 0);
      this.textBox42.Margin = new Padding(4, 4, 4, 4);
      this.textBox42.Name = "textBox42";
      this.textBox42.ReadOnly = true;
      this.textBox42.Size = new Size(89, 22);
      this.textBox42.TabIndex = 74;
      this.textBox42.TabStop = false;
      this.textBox42.Tag = (object) "NEWHUD.X1245";
      this.textBox42.TextAlign = HorizontalAlignment.Right;
      this.cbo801m.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801m.FormattingEnabled = true;
      this.cbo801m.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801m.Location = new Point(633, 0);
      this.cbo801m.Margin = new Padding(4, 4, 4, 4);
      this.cbo801m.Name = "cbo801m";
      this.cbo801m.Size = new Size(60, 24);
      this.cbo801m.TabIndex = 17;
      this.cbo801m.Tag = (object) "NEWHUD.X1247";
      this.label33.AutoSize = true;
      this.label33.Location = new Point(409, 5);
      this.label33.Margin = new Padding(4, 0, 4, 0);
      this.label33.Name = "label33";
      this.label33.Size = new Size(16, 17);
      this.label33.TabIndex = 75;
      this.label33.Text = "$";
      this.label31.AutoSize = true;
      this.label31.BackColor = Color.Transparent;
      this.label31.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label31.Location = new Point(544, 7);
      this.label31.Margin = new Padding(4, 0, 4, 0);
      this.label31.Name = "label31";
      this.label31.Size = new Size(50, 17);
      this.label31.TabIndex = 16;
      this.label31.Text = "Seller";
      this.label30.AutoSize = true;
      this.label30.BackColor = Color.Transparent;
      this.label30.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label30.Location = new Point(436, 7);
      this.label30.Margin = new Padding(4, 0, 4, 0);
      this.label30.Name = "label30";
      this.label30.Size = new Size(73, 17);
      this.label30.TabIndex = 15;
      this.label30.Text = "Borrower";
      this.panel801l.Controls.Add((Control) this.pictureBox12);
      this.panel801l.Controls.Add((Control) this.textBox37);
      this.panel801l.Controls.Add((Control) this.textBox38);
      this.panel801l.Controls.Add((Control) this.textBox39);
      this.panel801l.Controls.Add((Control) this.cbo801l);
      this.panel801l.Controls.Add((Control) this.label32);
      this.panel801l.Dock = DockStyle.Top;
      this.panel801l.Location = new Point(1, 335);
      this.panel801l.Margin = new Padding(4, 4, 4, 4);
      this.panel801l.Name = "panel801l";
      this.panel801l.Size = new Size(710, 27);
      this.panel801l.TabIndex = 14;
      this.pictureBox12.Image = (Image) Resources.compensation;
      this.pictureBox12.Location = new Point(17, 2);
      this.pictureBox12.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox12.Name = "pictureBox12";
      this.pictureBox12.Size = new Size(16, 16);
      this.pictureBox12.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox12.TabIndex = 79;
      this.pictureBox12.TabStop = false;
      this.textBox37.Location = new Point(525, 0);
      this.textBox37.Margin = new Padding(4, 4, 4, 4);
      this.textBox37.Name = "textBox37";
      this.textBox37.ReadOnly = true;
      this.textBox37.Size = new Size(89, 22);
      this.textBox37.TabIndex = 78;
      this.textBox37.TabStop = false;
      this.textBox37.Tag = (object) "NEWHUD.X1238";
      this.textBox37.TextAlign = HorizontalAlignment.Right;
      this.textBox38.Location = new Point(45, 0);
      this.textBox38.Margin = new Padding(4, 4, 4, 4);
      this.textBox38.Name = "textBox38";
      this.textBox38.ReadOnly = true;
      this.textBox38.Size = new Size(165, 22);
      this.textBox38.TabIndex = 76;
      this.textBox38.TabStop = false;
      this.textBox38.Tag = (object) "NEWHUD.X1235";
      this.textBox39.Location = new Point(429, 0);
      this.textBox39.Margin = new Padding(4, 4, 4, 4);
      this.textBox39.Name = "textBox39";
      this.textBox39.ReadOnly = true;
      this.textBox39.Size = new Size(89, 22);
      this.textBox39.TabIndex = 74;
      this.textBox39.TabStop = false;
      this.textBox39.Tag = (object) "NEWHUD.X1237";
      this.textBox39.TextAlign = HorizontalAlignment.Right;
      this.cbo801l.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801l.FormattingEnabled = true;
      this.cbo801l.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801l.Location = new Point(633, 0);
      this.cbo801l.Margin = new Padding(4, 4, 4, 4);
      this.cbo801l.Name = "cbo801l";
      this.cbo801l.Size = new Size(60, 24);
      this.cbo801l.TabIndex = 16;
      this.cbo801l.Tag = (object) "NEWHUD.X1239";
      this.label32.AutoSize = true;
      this.label32.Location = new Point(409, 5);
      this.label32.Margin = new Padding(4, 0, 4, 0);
      this.label32.Name = "label32";
      this.label32.Size = new Size(16, 17);
      this.label32.TabIndex = 75;
      this.label32.Text = "$";
      this.panel801k.Controls.Add((Control) this.pictureBox11);
      this.panel801k.Controls.Add((Control) this.textBox32);
      this.panel801k.Controls.Add((Control) this.textBox16);
      this.panel801k.Controls.Add((Control) this.textBox11);
      this.panel801k.Controls.Add((Control) this.cbo801k);
      this.panel801k.Controls.Add((Control) this.label25);
      this.panel801k.Dock = DockStyle.Top;
      this.panel801k.Location = new Point(1, 308);
      this.panel801k.Margin = new Padding(4, 4, 4, 4);
      this.panel801k.Name = "panel801k";
      this.panel801k.Size = new Size(710, 27);
      this.panel801k.TabIndex = 13;
      this.pictureBox11.Image = (Image) Resources.compensation;
      this.pictureBox11.Location = new Point(17, 2);
      this.pictureBox11.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox11.Name = "pictureBox11";
      this.pictureBox11.Size = new Size(16, 16);
      this.pictureBox11.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox11.TabIndex = 73;
      this.pictureBox11.TabStop = false;
      this.textBox32.Location = new Point(525, 0);
      this.textBox32.Margin = new Padding(4, 4, 4, 4);
      this.textBox32.Name = "textBox32";
      this.textBox32.ReadOnly = true;
      this.textBox32.Size = new Size(89, 22);
      this.textBox32.TabIndex = 72;
      this.textBox32.TabStop = false;
      this.textBox32.Tag = (object) "NEWHUD.X779";
      this.textBox32.TextAlign = HorizontalAlignment.Right;
      this.textBox16.Location = new Point(45, 0);
      this.textBox16.Margin = new Padding(4, 4, 4, 4);
      this.textBox16.Name = "textBox16";
      this.textBox16.ReadOnly = true;
      this.textBox16.Size = new Size(165, 22);
      this.textBox16.TabIndex = 5;
      this.textBox16.TabStop = false;
      this.textBox16.Tag = (object) "NEWHUD.X732";
      this.textBox11.Location = new Point(429, 0);
      this.textBox11.Margin = new Padding(4, 4, 4, 4);
      this.textBox11.Name = "textBox11";
      this.textBox11.ReadOnly = true;
      this.textBox11.Size = new Size(89, 22);
      this.textBox11.TabIndex = 1;
      this.textBox11.TabStop = false;
      this.textBox11.Tag = (object) "NEWHUD.X733";
      this.textBox11.TextAlign = HorizontalAlignment.Right;
      this.cbo801k.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801k.FormattingEnabled = true;
      this.cbo801k.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801k.Location = new Point(633, 0);
      this.cbo801k.Margin = new Padding(4, 4, 4, 4);
      this.cbo801k.Name = "cbo801k";
      this.cbo801k.Size = new Size(60, 24);
      this.cbo801k.TabIndex = 15;
      this.cbo801k.Tag = (object) "NEWHUD.X748";
      this.label25.AutoSize = true;
      this.label25.Location = new Point(409, 5);
      this.label25.Margin = new Padding(4, 0, 4, 0);
      this.label25.Name = "label25";
      this.label25.Size = new Size(16, 17);
      this.label25.TabIndex = 2;
      this.label25.Text = "$";
      this.panel801j.Controls.Add((Control) this.pictureBox10);
      this.panel801j.Controls.Add((Control) this.textBox31);
      this.panel801j.Controls.Add((Control) this.textBox15);
      this.panel801j.Controls.Add((Control) this.textBox10);
      this.panel801j.Controls.Add((Control) this.cbo801j);
      this.panel801j.Controls.Add((Control) this.label23);
      this.panel801j.Dock = DockStyle.Top;
      this.panel801j.Location = new Point(1, 281);
      this.panel801j.Margin = new Padding(4, 4, 4, 4);
      this.panel801j.Name = "panel801j";
      this.panel801j.Size = new Size(710, 27);
      this.panel801j.TabIndex = 12;
      this.pictureBox10.Image = (Image) Resources.compensation;
      this.pictureBox10.Location = new Point(17, 2);
      this.pictureBox10.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox10.Name = "pictureBox10";
      this.pictureBox10.Size = new Size(16, 16);
      this.pictureBox10.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox10.TabIndex = 73;
      this.pictureBox10.TabStop = false;
      this.textBox31.Location = new Point(525, 0);
      this.textBox31.Margin = new Padding(4, 4, 4, 4);
      this.textBox31.Name = "textBox31";
      this.textBox31.ReadOnly = true;
      this.textBox31.Size = new Size(89, 22);
      this.textBox31.TabIndex = 72;
      this.textBox31.TabStop = false;
      this.textBox31.Tag = (object) "1843";
      this.textBox31.TextAlign = HorizontalAlignment.Right;
      this.textBox15.Location = new Point(45, 0);
      this.textBox15.Margin = new Padding(4, 4, 4, 4);
      this.textBox15.Name = "textBox15";
      this.textBox15.ReadOnly = true;
      this.textBox15.Size = new Size(165, 22);
      this.textBox15.TabIndex = 5;
      this.textBox15.TabStop = false;
      this.textBox15.Tag = (object) "1841";
      this.textBox10.Location = new Point(429, 0);
      this.textBox10.Margin = new Padding(4, 4, 4, 4);
      this.textBox10.Name = "textBox10";
      this.textBox10.ReadOnly = true;
      this.textBox10.Size = new Size(89, 22);
      this.textBox10.TabIndex = 1;
      this.textBox10.TabStop = false;
      this.textBox10.Tag = (object) "1842";
      this.textBox10.TextAlign = HorizontalAlignment.Right;
      this.cbo801j.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801j.FormattingEnabled = true;
      this.cbo801j.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801j.Location = new Point(633, 0);
      this.cbo801j.Margin = new Padding(4, 4, 4, 4);
      this.cbo801j.Name = "cbo801j";
      this.cbo801j.Size = new Size(60, 24);
      this.cbo801j.TabIndex = 14;
      this.cbo801j.Tag = (object) "SYS.X301";
      this.label23.AutoSize = true;
      this.label23.Location = new Point(409, 5);
      this.label23.Margin = new Padding(4, 0, 4, 0);
      this.label23.Name = "label23";
      this.label23.Size = new Size(16, 17);
      this.label23.TabIndex = 2;
      this.label23.Text = "$";
      this.panel801i.Controls.Add((Control) this.pictureBox9);
      this.panel801i.Controls.Add((Control) this.textBox30);
      this.panel801i.Controls.Add((Control) this.textBox14);
      this.panel801i.Controls.Add((Control) this.textBox9);
      this.panel801i.Controls.Add((Control) this.cbo801i);
      this.panel801i.Controls.Add((Control) this.label21);
      this.panel801i.Dock = DockStyle.Top;
      this.panel801i.Location = new Point(1, 254);
      this.panel801i.Margin = new Padding(4, 4, 4, 4);
      this.panel801i.Name = "panel801i";
      this.panel801i.Size = new Size(710, 27);
      this.panel801i.TabIndex = 11;
      this.pictureBox9.Image = (Image) Resources.compensation;
      this.pictureBox9.Location = new Point(17, 2);
      this.pictureBox9.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox9.Name = "pictureBox9";
      this.pictureBox9.Size = new Size(16, 16);
      this.pictureBox9.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox9.TabIndex = 73;
      this.pictureBox9.TabStop = false;
      this.textBox30.Location = new Point(525, 0);
      this.textBox30.Margin = new Padding(4, 4, 4, 4);
      this.textBox30.Name = "textBox30";
      this.textBox30.ReadOnly = true;
      this.textBox30.Size = new Size(89, 22);
      this.textBox30.TabIndex = 72;
      this.textBox30.TabStop = false;
      this.textBox30.Tag = (object) "1840";
      this.textBox30.TextAlign = HorizontalAlignment.Right;
      this.textBox14.Location = new Point(45, 0);
      this.textBox14.Margin = new Padding(4, 4, 4, 4);
      this.textBox14.Name = "textBox14";
      this.textBox14.ReadOnly = true;
      this.textBox14.Size = new Size(165, 22);
      this.textBox14.TabIndex = 5;
      this.textBox14.TabStop = false;
      this.textBox14.Tag = (object) "1838";
      this.textBox9.Location = new Point(429, 0);
      this.textBox9.Margin = new Padding(4, 4, 4, 4);
      this.textBox9.Name = "textBox9";
      this.textBox9.ReadOnly = true;
      this.textBox9.Size = new Size(89, 22);
      this.textBox9.TabIndex = 1;
      this.textBox9.TabStop = false;
      this.textBox9.Tag = (object) "1839";
      this.textBox9.TextAlign = HorizontalAlignment.Right;
      this.cbo801i.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801i.FormattingEnabled = true;
      this.cbo801i.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801i.Location = new Point(633, 0);
      this.cbo801i.Margin = new Padding(4, 4, 4, 4);
      this.cbo801i.Name = "cbo801i";
      this.cbo801i.Size = new Size(60, 24);
      this.cbo801i.TabIndex = 13;
      this.cbo801i.Tag = (object) "SYS.X296";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(409, 5);
      this.label21.Margin = new Padding(4, 0, 4, 0);
      this.label21.Name = "label21";
      this.label21.Size = new Size(16, 17);
      this.label21.TabIndex = 2;
      this.label21.Text = "$";
      this.panel801h.Controls.Add((Control) this.pictureBox8);
      this.panel801h.Controls.Add((Control) this.textBox29);
      this.panel801h.Controls.Add((Control) this.textBox13);
      this.panel801h.Controls.Add((Control) this.textBox8);
      this.panel801h.Controls.Add((Control) this.cbo801h);
      this.panel801h.Controls.Add((Control) this.label19);
      this.panel801h.Dock = DockStyle.Top;
      this.panel801h.Location = new Point(1, 227);
      this.panel801h.Margin = new Padding(4, 4, 4, 4);
      this.panel801h.Name = "panel801h";
      this.panel801h.Size = new Size(710, 27);
      this.panel801h.TabIndex = 10;
      this.pictureBox8.Image = (Image) Resources.compensation;
      this.pictureBox8.Location = new Point(17, 2);
      this.pictureBox8.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox8.Name = "pictureBox8";
      this.pictureBox8.Size = new Size(16, 16);
      this.pictureBox8.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox8.TabIndex = 73;
      this.pictureBox8.TabStop = false;
      this.textBox29.Location = new Point(525, 0);
      this.textBox29.Margin = new Padding(4, 4, 4, 4);
      this.textBox29.Name = "textBox29";
      this.textBox29.ReadOnly = true;
      this.textBox29.Size = new Size(89, 22);
      this.textBox29.TabIndex = 72;
      this.textBox29.TabStop = false;
      this.textBox29.Tag = (object) "1626";
      this.textBox29.TextAlign = HorizontalAlignment.Right;
      this.textBox13.Location = new Point(45, 0);
      this.textBox13.Margin = new Padding(4, 4, 4, 4);
      this.textBox13.Name = "textBox13";
      this.textBox13.ReadOnly = true;
      this.textBox13.Size = new Size(165, 22);
      this.textBox13.TabIndex = 5;
      this.textBox13.TabStop = false;
      this.textBox13.Tag = (object) "1627";
      this.textBox8.Location = new Point(429, 0);
      this.textBox8.Margin = new Padding(4, 4, 4, 4);
      this.textBox8.Name = "textBox8";
      this.textBox8.ReadOnly = true;
      this.textBox8.Size = new Size(89, 22);
      this.textBox8.TabIndex = 1;
      this.textBox8.TabStop = false;
      this.textBox8.Tag = (object) "1625";
      this.textBox8.TextAlign = HorizontalAlignment.Right;
      this.cbo801h.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801h.FormattingEnabled = true;
      this.cbo801h.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801h.Location = new Point(633, 0);
      this.cbo801h.Margin = new Padding(4, 4, 4, 4);
      this.cbo801h.Name = "cbo801h";
      this.cbo801h.Size = new Size(60, 24);
      this.cbo801h.TabIndex = 12;
      this.cbo801h.Tag = (object) "SYS.X291";
      this.label19.AutoSize = true;
      this.label19.Location = new Point(409, 5);
      this.label19.Margin = new Padding(4, 0, 4, 0);
      this.label19.Name = "label19";
      this.label19.Size = new Size(16, 17);
      this.label19.TabIndex = 2;
      this.label19.Text = "$";
      this.panel801g.Controls.Add((Control) this.pictureBox7);
      this.panel801g.Controls.Add((Control) this.textBox28);
      this.panel801g.Controls.Add((Control) this.textBox12);
      this.panel801g.Controls.Add((Control) this.textBox7);
      this.panel801g.Controls.Add((Control) this.cbo801g);
      this.panel801g.Controls.Add((Control) this.label17);
      this.panel801g.Dock = DockStyle.Top;
      this.panel801g.Location = new Point(1, 200);
      this.panel801g.Margin = new Padding(4, 4, 4, 4);
      this.panel801g.Name = "panel801g";
      this.panel801g.Size = new Size(710, 27);
      this.panel801g.TabIndex = 9;
      this.pictureBox7.Image = (Image) Resources.compensation;
      this.pictureBox7.Location = new Point(17, 2);
      this.pictureBox7.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox7.Name = "pictureBox7";
      this.pictureBox7.Size = new Size(16, 16);
      this.pictureBox7.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox7.TabIndex = 73;
      this.pictureBox7.TabStop = false;
      this.textBox28.Location = new Point(525, 0);
      this.textBox28.Margin = new Padding(4, 4, 4, 4);
      this.textBox28.Name = "textBox28";
      this.textBox28.ReadOnly = true;
      this.textBox28.Size = new Size(89, 22);
      this.textBox28.TabIndex = 72;
      this.textBox28.TabStop = false;
      this.textBox28.Tag = (object) "200";
      this.textBox28.TextAlign = HorizontalAlignment.Right;
      this.textBox12.Location = new Point(45, 0);
      this.textBox12.Margin = new Padding(4, 4, 4, 4);
      this.textBox12.Name = "textBox12";
      this.textBox12.ReadOnly = true;
      this.textBox12.Size = new Size(165, 22);
      this.textBox12.TabIndex = 4;
      this.textBox12.TabStop = false;
      this.textBox12.Tag = (object) "154";
      this.textBox7.Location = new Point(429, 0);
      this.textBox7.Margin = new Padding(4, 4, 4, 4);
      this.textBox7.Name = "textBox7";
      this.textBox7.ReadOnly = true;
      this.textBox7.Size = new Size(89, 22);
      this.textBox7.TabIndex = 1;
      this.textBox7.TabStop = false;
      this.textBox7.Tag = (object) "155";
      this.textBox7.TextAlign = HorizontalAlignment.Right;
      this.cbo801g.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801g.FormattingEnabled = true;
      this.cbo801g.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801g.Location = new Point(633, 0);
      this.cbo801g.Margin = new Padding(4, 4, 4, 4);
      this.cbo801g.Name = "cbo801g";
      this.cbo801g.Size = new Size(60, 24);
      this.cbo801g.TabIndex = 11;
      this.cbo801g.Tag = (object) "SYS.X289";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(409, 5);
      this.label17.Margin = new Padding(4, 0, 4, 0);
      this.label17.Name = "label17";
      this.label17.Size = new Size(16, 17);
      this.label17.TabIndex = 2;
      this.label17.Text = "$";
      this.panel801f.Controls.Add((Control) this.pictureBox6);
      this.panel801f.Controls.Add((Control) this.textBox27);
      this.panel801f.Controls.Add((Control) this.textBox19);
      this.panel801f.Controls.Add((Control) this.label20);
      this.panel801f.Controls.Add((Control) this.textBox20);
      this.panel801f.Controls.Add((Control) this.label22);
      this.panel801f.Controls.Add((Control) this.cbo801f);
      this.panel801f.Controls.Add((Control) this.textBox6);
      this.panel801f.Controls.Add((Control) this.label15);
      this.panel801f.Controls.Add((Control) this.label16);
      this.panel801f.Dock = DockStyle.Top;
      this.panel801f.Location = new Point(1, 173);
      this.panel801f.Margin = new Padding(4, 4, 4, 4);
      this.panel801f.Name = "panel801f";
      this.panel801f.Size = new Size(710, 27);
      this.panel801f.TabIndex = 8;
      this.pictureBox6.Image = (Image) Resources.compensation;
      this.pictureBox6.Location = new Point(17, 2);
      this.pictureBox6.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox6.Name = "pictureBox6";
      this.pictureBox6.Size = new Size(16, 16);
      this.pictureBox6.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox6.TabIndex = 73;
      this.pictureBox6.TabStop = false;
      this.textBox27.Location = new Point(525, 0);
      this.textBox27.Margin = new Padding(4, 4, 4, 4);
      this.textBox27.Name = "textBox27";
      this.textBox27.ReadOnly = true;
      this.textBox27.Size = new Size(89, 22);
      this.textBox27.TabIndex = 72;
      this.textBox27.TabStop = false;
      this.textBox27.Tag = (object) "NEWHUD.X226";
      this.textBox27.TextAlign = HorizontalAlignment.Right;
      this.textBox19.Location = new Point(324, 0);
      this.textBox19.Margin = new Padding(4, 4, 4, 4);
      this.textBox19.Name = "textBox19";
      this.textBox19.ReadOnly = true;
      this.textBox19.Size = new Size(72, 22);
      this.textBox19.TabIndex = 10;
      this.textBox19.TabStop = false;
      this.textBox19.Tag = (object) "NEWHUD.X224";
      this.textBox19.TextAlign = HorizontalAlignment.Right;
      this.label20.AutoSize = true;
      this.label20.Location = new Point(307, 5);
      this.label20.Margin = new Padding(4, 0, 4, 0);
      this.label20.Name = "label20";
      this.label20.Size = new Size(16, 17);
      this.label20.TabIndex = 11;
      this.label20.Text = "$";
      this.textBox20.Location = new Point(220, 0);
      this.textBox20.Margin = new Padding(4, 4, 4, 4);
      this.textBox20.Name = "textBox20";
      this.textBox20.ReadOnly = true;
      this.textBox20.Size = new Size(55, 22);
      this.textBox20.TabIndex = 9;
      this.textBox20.TabStop = false;
      this.textBox20.Tag = (object) "NEWHUD.X223";
      this.textBox20.TextAlign = HorizontalAlignment.Right;
      this.label22.AutoSize = true;
      this.label22.Location = new Point(276, 5);
      this.label22.Margin = new Padding(4, 0, 4, 0);
      this.label22.Name = "label22";
      this.label22.Size = new Size(32, 17);
      this.label22.TabIndex = 8;
      this.label22.Text = "% +";
      this.cbo801f.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801f.Enabled = false;
      this.cbo801f.FormattingEnabled = true;
      this.cbo801f.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801f.Location = new Point(633, 0);
      this.cbo801f.Margin = new Padding(4, 4, 4, 4);
      this.cbo801f.Name = "cbo801f";
      this.cbo801f.Size = new Size(60, 24);
      this.cbo801f.TabIndex = 10;
      this.cbo801f.Tag = (object) "NEWHUD.X227";
      this.textBox6.Location = new Point(429, 0);
      this.textBox6.Margin = new Padding(4, 4, 4, 4);
      this.textBox6.Name = "textBox6";
      this.textBox6.ReadOnly = true;
      this.textBox6.Size = new Size(89, 22);
      this.textBox6.TabIndex = 1;
      this.textBox6.TabStop = false;
      this.textBox6.Tag = (object) "NEWHUD.X225";
      this.textBox6.TextAlign = HorizontalAlignment.Right;
      this.label15.AutoSize = true;
      this.label15.Location = new Point(409, 5);
      this.label15.Margin = new Padding(4, 0, 4, 0);
      this.label15.Name = "label15";
      this.label15.Size = new Size(16, 17);
      this.label15.TabIndex = 2;
      this.label15.Text = "$";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(41, 5);
      this.label16.Margin = new Padding(4, 0, 4, 0);
      this.label16.Name = "label16";
      this.label16.Size = new Size(144, 17);
      this.label16.TabIndex = 0;
      this.label16.Text = "Broker Compensation";
      this.panel801e.Controls.Add((Control) this.pictureBox5);
      this.panel801e.Controls.Add((Control) this.textBox26);
      this.panel801e.Controls.Add((Control) this.textBox18);
      this.panel801e.Controls.Add((Control) this.label18);
      this.panel801e.Controls.Add((Control) this.textBox17);
      this.panel801e.Controls.Add((Control) this.label2);
      this.panel801e.Controls.Add((Control) this.cbo801e);
      this.panel801e.Controls.Add((Control) this.textBox5);
      this.panel801e.Controls.Add((Control) this.label13);
      this.panel801e.Controls.Add((Control) this.label14);
      this.panel801e.Dock = DockStyle.Top;
      this.panel801e.Location = new Point(1, 146);
      this.panel801e.Margin = new Padding(4, 4, 4, 4);
      this.panel801e.Name = "panel801e";
      this.panel801e.Size = new Size(710, 27);
      this.panel801e.TabIndex = 7;
      this.pictureBox5.Image = (Image) Resources.compensation;
      this.pictureBox5.Location = new Point(17, 2);
      this.pictureBox5.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox5.Name = "pictureBox5";
      this.pictureBox5.Size = new Size(16, 16);
      this.pictureBox5.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox5.TabIndex = 73;
      this.pictureBox5.TabStop = false;
      this.textBox26.Location = new Point(525, 0);
      this.textBox26.Margin = new Padding(4, 4, 4, 4);
      this.textBox26.Name = "textBox26";
      this.textBox26.ReadOnly = true;
      this.textBox26.Size = new Size(89, 22);
      this.textBox26.TabIndex = 72;
      this.textBox26.TabStop = false;
      this.textBox26.Tag = (object) "572";
      this.textBox26.TextAlign = HorizontalAlignment.Right;
      this.textBox18.Location = new Point(324, 1);
      this.textBox18.Margin = new Padding(4, 4, 4, 4);
      this.textBox18.Name = "textBox18";
      this.textBox18.ReadOnly = true;
      this.textBox18.Size = new Size(72, 22);
      this.textBox18.TabIndex = 6;
      this.textBox18.TabStop = false;
      this.textBox18.Tag = (object) "1620";
      this.textBox18.TextAlign = HorizontalAlignment.Right;
      this.label18.AutoSize = true;
      this.label18.Location = new Point(307, 6);
      this.label18.Margin = new Padding(4, 0, 4, 0);
      this.label18.Name = "label18";
      this.label18.Size = new Size(16, 17);
      this.label18.TabIndex = 7;
      this.label18.Text = "$";
      this.textBox17.Location = new Point(220, 0);
      this.textBox17.Margin = new Padding(4, 4, 4, 4);
      this.textBox17.Name = "textBox17";
      this.textBox17.ReadOnly = true;
      this.textBox17.Size = new Size(55, 22);
      this.textBox17.TabIndex = 5;
      this.textBox17.TabStop = false;
      this.textBox17.Tag = (object) "389";
      this.textBox17.TextAlign = HorizontalAlignment.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(276, 5);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(32, 17);
      this.label2.TabIndex = 4;
      this.label2.Text = "% +";
      this.cbo801e.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801e.FormattingEnabled = true;
      this.cbo801e.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801e.Location = new Point(633, 0);
      this.cbo801e.Margin = new Padding(4, 4, 4, 4);
      this.cbo801e.Name = "cbo801e";
      this.cbo801e.Size = new Size(60, 24);
      this.cbo801e.TabIndex = 9;
      this.cbo801e.Tag = (object) "SYS.X265";
      this.textBox5.Location = new Point(429, 0);
      this.textBox5.Margin = new Padding(4, 4, 4, 4);
      this.textBox5.Name = "textBox5";
      this.textBox5.ReadOnly = true;
      this.textBox5.Size = new Size(89, 22);
      this.textBox5.TabIndex = 1;
      this.textBox5.TabStop = false;
      this.textBox5.Tag = (object) "439";
      this.textBox5.TextAlign = HorizontalAlignment.Right;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(409, 5);
      this.label13.Margin = new Padding(4, 0, 4, 0);
      this.label13.Name = "label13";
      this.label13.Size = new Size(16, 17);
      this.label13.TabIndex = 2;
      this.label13.Text = "$";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(41, 5);
      this.label14.Margin = new Padding(4, 0, 4, 0);
      this.label14.Name = "label14";
      this.label14.Size = new Size(85, 17);
      this.label14.TabIndex = 0;
      this.label14.Text = "Broker Fees";
      this.panel801d.Controls.Add((Control) this.pictureBox4);
      this.panel801d.Controls.Add((Control) this.textBox25);
      this.panel801d.Controls.Add((Control) this.cbo801d);
      this.panel801d.Controls.Add((Control) this.textBox4);
      this.panel801d.Controls.Add((Control) this.label11);
      this.panel801d.Controls.Add((Control) this.label12);
      this.panel801d.Dock = DockStyle.Top;
      this.panel801d.Location = new Point(1, 119);
      this.panel801d.Margin = new Padding(4, 4, 4, 4);
      this.panel801d.Name = "panel801d";
      this.panel801d.Size = new Size(710, 27);
      this.panel801d.TabIndex = 6;
      this.pictureBox4.Image = (Image) Resources.compensation;
      this.pictureBox4.Location = new Point(17, 2);
      this.pictureBox4.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox4.Name = "pictureBox4";
      this.pictureBox4.Size = new Size(16, 16);
      this.pictureBox4.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox4.TabIndex = 73;
      this.pictureBox4.TabStop = false;
      this.textBox25.Location = new Point(525, 0);
      this.textBox25.Margin = new Padding(4, 4, 4, 4);
      this.textBox25.Name = "textBox25";
      this.textBox25.ReadOnly = true;
      this.textBox25.Size = new Size(89, 22);
      this.textBox25.TabIndex = 72;
      this.textBox25.TabStop = false;
      this.textBox25.Tag = (object) "569";
      this.textBox25.TextAlign = HorizontalAlignment.Right;
      this.cbo801d.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801d.FormattingEnabled = true;
      this.cbo801d.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801d.Location = new Point(633, 0);
      this.cbo801d.Margin = new Padding(4, 4, 4, 4);
      this.cbo801d.Name = "cbo801d";
      this.cbo801d.Size = new Size(60, 24);
      this.cbo801d.TabIndex = 8;
      this.cbo801d.Tag = (object) "SYS.X271";
      this.textBox4.Location = new Point(429, 0);
      this.textBox4.Margin = new Padding(4, 4, 4, 4);
      this.textBox4.Name = "textBox4";
      this.textBox4.ReadOnly = true;
      this.textBox4.Size = new Size(89, 22);
      this.textBox4.TabIndex = 1;
      this.textBox4.TabStop = false;
      this.textBox4.Tag = (object) "367";
      this.textBox4.TextAlign = HorizontalAlignment.Right;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(409, 5);
      this.label11.Margin = new Padding(4, 0, 4, 0);
      this.label11.Name = "label11";
      this.label11.Size = new Size(16, 17);
      this.label11.TabIndex = 2;
      this.label11.Text = "$";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(41, 5);
      this.label12.Margin = new Padding(4, 0, 4, 0);
      this.label12.Name = "label12";
      this.label12.Size = new Size(122, 17);
      this.label12.TabIndex = 0;
      this.label12.Text = "Underwriting Fees";
      this.panel801c.Controls.Add((Control) this.pictureBox3);
      this.panel801c.Controls.Add((Control) this.textBox24);
      this.panel801c.Controls.Add((Control) this.cbo801c);
      this.panel801c.Controls.Add((Control) this.textBox3);
      this.panel801c.Controls.Add((Control) this.label9);
      this.panel801c.Controls.Add((Control) this.label10);
      this.panel801c.Dock = DockStyle.Top;
      this.panel801c.Location = new Point(1, 92);
      this.panel801c.Margin = new Padding(4, 4, 4, 4);
      this.panel801c.Name = "panel801c";
      this.panel801c.Size = new Size(710, 27);
      this.panel801c.TabIndex = 5;
      this.pictureBox3.Image = (Image) Resources.compensation;
      this.pictureBox3.Location = new Point(17, 2);
      this.pictureBox3.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox3.Name = "pictureBox3";
      this.pictureBox3.Size = new Size(16, 16);
      this.pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox3.TabIndex = 73;
      this.pictureBox3.TabStop = false;
      this.textBox24.Location = new Point(525, 0);
      this.textBox24.Margin = new Padding(4, 4, 4, 4);
      this.textBox24.Name = "textBox24";
      this.textBox24.ReadOnly = true;
      this.textBox24.Size = new Size(89, 22);
      this.textBox24.TabIndex = 72;
      this.textBox24.TabStop = false;
      this.textBox24.Tag = (object) "1622";
      this.textBox24.TextAlign = HorizontalAlignment.Right;
      this.cbo801c.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801c.FormattingEnabled = true;
      this.cbo801c.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801c.Location = new Point(633, 0);
      this.cbo801c.Margin = new Padding(4, 4, 4, 4);
      this.cbo801c.Name = "cbo801c";
      this.cbo801c.Size = new Size(60, 24);
      this.cbo801c.TabIndex = 7;
      this.cbo801c.Tag = (object) "SYS.X269";
      this.textBox3.Location = new Point(429, 0);
      this.textBox3.Margin = new Padding(4, 4, 4, 4);
      this.textBox3.Name = "textBox3";
      this.textBox3.ReadOnly = true;
      this.textBox3.Size = new Size(89, 22);
      this.textBox3.TabIndex = 1;
      this.textBox3.TabStop = false;
      this.textBox3.Tag = (object) "1621";
      this.textBox3.TextAlign = HorizontalAlignment.Right;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(409, 5);
      this.label9.Margin = new Padding(4, 0, 4, 0);
      this.label9.Name = "label9";
      this.label9.Size = new Size(16, 17);
      this.label9.TabIndex = 2;
      this.label9.Text = "$";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(41, 5);
      this.label10.Margin = new Padding(4, 0, 4, 0);
      this.label10.Name = "label10";
      this.label10.Size = new Size(113, 17);
      this.label10.TabIndex = 0;
      this.label10.Text = "Processing Fees";
      this.panel801b.Controls.Add((Control) this.pictureBox2);
      this.panel801b.Controls.Add((Control) this.textBox23);
      this.panel801b.Controls.Add((Control) this.cbo801b);
      this.panel801b.Controls.Add((Control) this.label8);
      this.panel801b.Controls.Add((Control) this.label7);
      this.panel801b.Controls.Add((Control) this.textBox2);
      this.panel801b.Dock = DockStyle.Top;
      this.panel801b.Location = new Point(1, 65);
      this.panel801b.Margin = new Padding(4, 4, 4, 4);
      this.panel801b.Name = "panel801b";
      this.panel801b.Size = new Size(710, 27);
      this.panel801b.TabIndex = 4;
      this.pictureBox2.Image = (Image) Resources.compensation;
      this.pictureBox2.Location = new Point(17, 2);
      this.pictureBox2.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new Size(16, 16);
      this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox2.TabIndex = 73;
      this.pictureBox2.TabStop = false;
      this.textBox23.Location = new Point(525, 0);
      this.textBox23.Margin = new Padding(4, 4, 4, 4);
      this.textBox23.Name = "textBox23";
      this.textBox23.ReadOnly = true;
      this.textBox23.Size = new Size(89, 22);
      this.textBox23.TabIndex = 72;
      this.textBox23.TabStop = false;
      this.textBox23.Tag = (object) "L229";
      this.textBox23.TextAlign = HorizontalAlignment.Right;
      this.cbo801b.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbo801b.FormattingEnabled = true;
      this.cbo801b.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801b.Location = new Point(633, 0);
      this.cbo801b.Margin = new Padding(4, 4, 4, 4);
      this.cbo801b.Name = "cbo801b";
      this.cbo801b.Size = new Size(60, 24);
      this.cbo801b.TabIndex = 6;
      this.cbo801b.Tag = (object) "SYS.X261";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(41, 5);
      this.label8.Margin = new Padding(4, 0, 4, 0);
      this.label8.Name = "label8";
      this.label8.Size = new Size(112, 17);
      this.label8.TabIndex = 0;
      this.label8.Text = "Application Fees";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(409, 5);
      this.label7.Margin = new Padding(4, 0, 4, 0);
      this.label7.Name = "label7";
      this.label7.Size = new Size(16, 17);
      this.label7.TabIndex = 2;
      this.label7.Text = "$";
      this.textBox2.Location = new Point(429, 0);
      this.textBox2.Margin = new Padding(4, 4, 4, 4);
      this.textBox2.Name = "textBox2";
      this.textBox2.ReadOnly = true;
      this.textBox2.Size = new Size(89, 22);
      this.textBox2.TabIndex = 1;
      this.textBox2.TabStop = false;
      this.textBox2.Tag = (object) "L228";
      this.textBox2.TextAlign = HorizontalAlignment.Right;
      this.panel801a.Controls.Add((Control) this.textBox22);
      this.panel801a.Controls.Add((Control) this.pictureBox1);
      this.panel801a.Controls.Add((Control) this.textBox21);
      this.panel801a.Controls.Add((Control) this.label24);
      this.panel801a.Controls.Add((Control) this.cbo801a);
      this.panel801a.Controls.Add((Control) this.label4);
      this.panel801a.Controls.Add((Control) this.label5);
      this.panel801a.Controls.Add((Control) this.textBox1);
      this.panel801a.Dock = DockStyle.Top;
      this.panel801a.Location = new Point(1, 38);
      this.panel801a.Margin = new Padding(4, 4, 4, 4);
      this.panel801a.Name = "panel801a";
      this.panel801a.Size = new Size(710, 27);
      this.panel801a.TabIndex = 3;
      this.textBox22.Location = new Point(525, 0);
      this.textBox22.Margin = new Padding(4, 4, 4, 4);
      this.textBox22.Name = "textBox22";
      this.textBox22.ReadOnly = true;
      this.textBox22.Size = new Size(89, 22);
      this.textBox22.TabIndex = 71;
      this.textBox22.TabStop = false;
      this.textBox22.Tag = (object) "559";
      this.textBox22.TextAlign = HorizontalAlignment.Right;
      this.pictureBox1.Image = (Image) Resources.compensation;
      this.pictureBox1.Location = new Point(17, 2);
      this.pictureBox1.Margin = new Padding(4, 4, 4, 4);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(16, 16);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 70;
      this.pictureBox1.TabStop = false;
      this.textBox21.Location = new Point(220, 0);
      this.textBox21.Margin = new Padding(4, 4, 4, 4);
      this.textBox21.Name = "textBox21";
      this.textBox21.ReadOnly = true;
      this.textBox21.Size = new Size(55, 22);
      this.textBox21.TabIndex = 7;
      this.textBox21.TabStop = false;
      this.textBox21.Tag = (object) "388";
      this.textBox21.TextAlign = HorizontalAlignment.Right;
      this.label24.AutoSize = true;
      this.label24.Location = new Point(276, 5);
      this.label24.Margin = new Padding(4, 0, 4, 0);
      this.label24.Name = "label24";
      this.label24.Size = new Size(20, 17);
      this.label24.TabIndex = 6;
      this.label24.Text = "%";
      this.cbo801a.FormattingEnabled = true;
      this.cbo801a.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "B",
        (object) "L",
        (object) "O"
      });
      this.cbo801a.Location = new Point(633, 0);
      this.cbo801a.Margin = new Padding(4, 4, 4, 4);
      this.cbo801a.Name = "cbo801a";
      this.cbo801a.Size = new Size(60, 24);
      this.cbo801a.TabIndex = 5;
      this.cbo801a.Tag = (object) "SYS.X251";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(41, 5);
      this.label4.Margin = new Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(148, 17);
      this.label4.TabIndex = 0;
      this.label4.Text = "Loan Origination Fees";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(409, 5);
      this.label5.Margin = new Padding(4, 0, 4, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(16, 17);
      this.label5.TabIndex = 2;
      this.label5.Text = "$";
      this.textBox1.Location = new Point(429, 0);
      this.textBox1.Margin = new Padding(4, 4, 4, 4);
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new Size(89, 22);
      this.textBox1.TabIndex = 1;
      this.textBox1.TabStop = false;
      this.textBox1.Tag = (object) "454";
      this.textBox1.TextAlign = HorizontalAlignment.Right;
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(1, 26);
      this.panel3.Margin = new Padding(4, 4, 4, 4);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(710, 12);
      this.panel3.TabIndex = 2;
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(632, 7);
      this.label6.Margin = new Padding(4, 0, 4, 0);
      this.label6.Name = "label6";
      this.label6.Size = new Size(62, 17);
      this.label6.TabIndex = 1;
      this.label6.Text = "Paid by";
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(40, 242);
      this.pboxDownArrow.Margin = new Padding(4, 4, 4, 4);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(23, 21);
      this.pboxDownArrow.TabIndex = 69;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(40, 220);
      this.pboxAsterisk.Margin = new Padding(4, 4, 4, 4);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(32, 15);
      this.pboxAsterisk.TabIndex = 18;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.panel1.BackgroundImage = (Image) Resources.warning_32x32;
      this.panel1.BackgroundImageLayout = ImageLayout.Center;
      this.panel1.Location = new Point(16, 16);
      this.panel1.Margin = new Padding(4, 4, 4, 4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(56, 49);
      this.panel1.TabIndex = 7;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(828, 740);
      this.Controls.Add((Control) this.pboxDownArrow);
      this.Controls.Add((Control) this.pboxAsterisk);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.rdoTakeNoAction);
      this.Controls.Add((Control) this.rdoTo3rdParty);
      this.Controls.Add((Control) this.rdoToBorrower);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(4, 4, 4, 4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LOCompensationViolationDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "LO Compensation Violation";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.panel801s.ResumeLayout(false);
      this.panel801s.PerformLayout();
      ((ISupportInitialize) this.pictureBox19).EndInit();
      this.panel801r.ResumeLayout(false);
      this.panel801r.PerformLayout();
      ((ISupportInitialize) this.pictureBox18).EndInit();
      this.panel801q.ResumeLayout(false);
      this.panel801q.PerformLayout();
      ((ISupportInitialize) this.pictureBox17).EndInit();
      this.panel801p.ResumeLayout(false);
      this.panel801p.PerformLayout();
      ((ISupportInitialize) this.pictureBox16).EndInit();
      this.panel801o.ResumeLayout(false);
      this.panel801o.PerformLayout();
      ((ISupportInitialize) this.pictureBox15).EndInit();
      this.panel801n.ResumeLayout(false);
      this.panel801n.PerformLayout();
      ((ISupportInitialize) this.pictureBox14).EndInit();
      this.panel801m.ResumeLayout(false);
      this.panel801m.PerformLayout();
      ((ISupportInitialize) this.pictureBox13).EndInit();
      this.panel801l.ResumeLayout(false);
      this.panel801l.PerformLayout();
      ((ISupportInitialize) this.pictureBox12).EndInit();
      this.panel801k.ResumeLayout(false);
      this.panel801k.PerformLayout();
      ((ISupportInitialize) this.pictureBox11).EndInit();
      this.panel801j.ResumeLayout(false);
      this.panel801j.PerformLayout();
      ((ISupportInitialize) this.pictureBox10).EndInit();
      this.panel801i.ResumeLayout(false);
      this.panel801i.PerformLayout();
      ((ISupportInitialize) this.pictureBox9).EndInit();
      this.panel801h.ResumeLayout(false);
      this.panel801h.PerformLayout();
      ((ISupportInitialize) this.pictureBox8).EndInit();
      this.panel801g.ResumeLayout(false);
      this.panel801g.PerformLayout();
      ((ISupportInitialize) this.pictureBox7).EndInit();
      this.panel801f.ResumeLayout(false);
      this.panel801f.PerformLayout();
      ((ISupportInitialize) this.pictureBox6).EndInit();
      this.panel801e.ResumeLayout(false);
      this.panel801e.PerformLayout();
      ((ISupportInitialize) this.pictureBox5).EndInit();
      this.panel801d.ResumeLayout(false);
      this.panel801d.PerformLayout();
      ((ISupportInitialize) this.pictureBox4).EndInit();
      this.panel801c.ResumeLayout(false);
      this.panel801c.PerformLayout();
      ((ISupportInitialize) this.pictureBox3).EndInit();
      this.panel801b.ResumeLayout(false);
      this.panel801b.PerformLayout();
      ((ISupportInitialize) this.pictureBox2).EndInit();
      this.panel801a.ResumeLayout(false);
      this.panel801a.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
