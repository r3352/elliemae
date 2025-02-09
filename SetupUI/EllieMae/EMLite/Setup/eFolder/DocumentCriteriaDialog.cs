// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.DocumentCriteriaDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class DocumentCriteriaDialog : Form
  {
    private DocumentCriteria criteria;
    private Sessions.Session session;
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private Label lblCriteria;
    private CheckBox chkLoanType;
    private CheckBox chkLoanPurpose;
    private CheckBox chkLienType;
    private CheckBox chkOccupancyType;
    private CheckBox chkAmortType;
    private CheckBox chkPropertyState;
    private ListView lvwLoanPurpose;
    private ListView lvwOccupancyType;
    private ListView lvwLoanType;
    private ListView lvwLienType;
    private ListView lvwAmortType;
    private ListView lvwPropertyState;
    private ListView lvwPlanCode;
    private CheckBox chkPlanCode;
    private ListView lvwFormVersion;
    private CheckBox chkFormVersion;

    public DocumentCriteriaDialog(DocumentCriteria criteria, Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.loadPlanCodes();
      this.criteria = criteria;
      this.loadCriteria();
    }

    public DocumentCriteria Criteria => this.criteria;

    private void loadPlanCodes()
    {
      if (!this.session.Application.GetService<ILoanServices>().IsEncompassDocServiceAvailable(DocumentOrderType.Closing))
      {
        this.lvwPlanCode.Visible = false;
        this.chkPlanCode.Visible = false;
      }
      else
      {
        foreach (PlanCodeInfo companyPlanCode in this.session.ConfigurationManager.GetCompanyPlanCodes(DocumentOrderType.Closing))
        {
          if (!string.IsNullOrEmpty(companyPlanCode.PlanCode))
            this.lvwPlanCode.Items.Add(companyPlanCode.PlanCode).Tag = (object) companyPlanCode.PlanCode;
        }
      }
    }

    private void loadCriteria()
    {
      if (this.criteria == null)
        return;
      this.chkAmortType.Checked = this.criteria.AmortTypeValues != null;
      if (this.chkAmortType.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwAmortType.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.AmortTypeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkLienType.Checked = this.criteria.LienTypeValues != null;
      if (this.chkLienType.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwLienType.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.LienTypeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkLoanPurpose.Checked = this.criteria.LoanPurposeValues != null;
      if (this.chkLoanPurpose.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwLoanPurpose.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.LoanPurposeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkLoanType.Checked = this.criteria.LoanTypeValues != null;
      if (this.chkLoanType.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwLoanType.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.LoanTypeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkOccupancyType.Checked = this.criteria.OccupancyTypeValues != null;
      if (this.chkOccupancyType.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwOccupancyType.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.OccupancyTypeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkPropertyState.Checked = this.criteria.PropertyStateValues != null;
      if (this.chkPropertyState.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwPropertyState.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.PropertyStateValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkPlanCode.Checked = this.criteria.PlanCodeValues != null;
      if (this.chkPlanCode.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwPlanCode.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.PlanCodeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkFormVersion.Checked = this.criteria.FormVersionValues != null;
      if (!this.chkFormVersion.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwFormVersion.Items)
      {
        if (Array.IndexOf<object>((object[]) this.criteria.FormVersionValues, listViewItem.Tag) >= 0)
          listViewItem.Checked = true;
      }
    }

    private bool saveCriteria()
    {
      if (!this.chkAmortType.Checked && !this.chkLienType.Checked && !this.chkLoanPurpose.Checked && !this.chkLoanType.Checked && !this.chkOccupancyType.Checked && !this.chkPropertyState.Checked && !this.chkPlanCode.Checked && !this.chkFormVersion.Checked)
      {
        this.criteria = (DocumentCriteria) null;
        return true;
      }
      string empty = string.Empty;
      string[] amortTypeValues = (string[]) null;
      if (this.chkAmortType.Checked)
      {
        ArrayList arrayList = new ArrayList();
        foreach (ListViewItem checkedItem in this.lvwAmortType.CheckedItems)
          arrayList.Add(checkedItem.Tag);
        if (arrayList.Count > 0)
          amortTypeValues = (string[]) arrayList.ToArray(typeof (string));
        else
          empty += "Amortization Type\r\n";
      }
      string[] lienTypeValues = (string[]) null;
      if (this.chkLienType.Checked)
      {
        ArrayList arrayList = new ArrayList();
        foreach (ListViewItem checkedItem in this.lvwLienType.CheckedItems)
          arrayList.Add(checkedItem.Tag);
        if (arrayList.Count > 0)
          lienTypeValues = (string[]) arrayList.ToArray(typeof (string));
        else
          empty += "Lien Position\r\n";
      }
      string[] loanPurposeValues = (string[]) null;
      if (this.chkLoanPurpose.Checked)
      {
        ArrayList arrayList = new ArrayList();
        foreach (ListViewItem checkedItem in this.lvwLoanPurpose.CheckedItems)
          arrayList.Add(checkedItem.Tag);
        if (arrayList.Count > 0)
          loanPurposeValues = (string[]) arrayList.ToArray(typeof (string));
        else
          empty += "Purpose of Loan\r\n";
      }
      string[] loanTypeValues = (string[]) null;
      if (this.chkLoanType.Checked)
      {
        ArrayList arrayList = new ArrayList();
        foreach (ListViewItem checkedItem in this.lvwLoanType.CheckedItems)
          arrayList.Add(checkedItem.Tag);
        if (arrayList.Count > 0)
          loanTypeValues = (string[]) arrayList.ToArray(typeof (string));
        else
          empty += "Loan Type\r\n";
      }
      string[] occupancyTypeValues = (string[]) null;
      if (this.chkOccupancyType.Checked)
      {
        ArrayList arrayList = new ArrayList();
        foreach (ListViewItem checkedItem in this.lvwOccupancyType.CheckedItems)
          arrayList.Add(checkedItem.Tag);
        if (arrayList.Count > 0)
          occupancyTypeValues = (string[]) arrayList.ToArray(typeof (string));
        else
          empty += "Property Will Be\r\n";
      }
      string[] propertyStateValues = (string[]) null;
      if (this.chkPropertyState.Checked)
      {
        ArrayList arrayList = new ArrayList();
        foreach (ListViewItem checkedItem in this.lvwPropertyState.CheckedItems)
          arrayList.Add(checkedItem.Tag);
        if (arrayList.Count > 0)
          propertyStateValues = (string[]) arrayList.ToArray(typeof (string));
        else
          empty += "Property State\r\n";
      }
      string[] planCodeValues = (string[]) null;
      if (this.chkPlanCode.Checked)
      {
        ArrayList arrayList = new ArrayList();
        foreach (ListViewItem checkedItem in this.lvwPlanCode.CheckedItems)
          arrayList.Add(checkedItem.Tag);
        if (arrayList.Count > 0)
          planCodeValues = (string[]) arrayList.ToArray(typeof (string));
        else
          empty += "Plan Code\r\n";
      }
      string[] formVersionValues = (string[]) null;
      if (this.chkFormVersion.Checked)
      {
        ArrayList arrayList = new ArrayList();
        foreach (ListViewItem checkedItem in this.lvwFormVersion.CheckedItems)
          arrayList.Add(checkedItem.Tag);
        if (arrayList.Count > 0)
          formVersionValues = (string[]) arrayList.ToArray(typeof (string));
        else
          empty += "Form Version\r\n";
      }
      if (empty != string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select at least one value for each of the following categories:\r\n\r\n" + empty, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      this.criteria = new DocumentCriteria(amortTypeValues, lienTypeValues, loanPurposeValues, loanTypeValues, occupancyTypeValues, propertyStateValues, planCodeValues, formVersionValues);
      return true;
    }

    private void chkAmortType_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwAmortType.Enabled = this.chkAmortType.Checked;
      if (this.chkAmortType.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwAmortType.Items)
        listViewItem.Checked = false;
    }

    private void chkLienType_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwLienType.Enabled = this.chkLienType.Checked;
      if (this.chkLienType.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwLienType.Items)
        listViewItem.Checked = false;
    }

    private void chkLoanPurpose_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwLoanPurpose.Enabled = this.chkLoanPurpose.Checked;
      if (this.chkLoanPurpose.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwLoanPurpose.Items)
        listViewItem.Checked = false;
    }

    private void chkLoanType_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwLoanType.Enabled = this.chkLoanType.Checked;
      if (this.chkLoanType.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwLoanType.Items)
        listViewItem.Checked = false;
    }

    private void chkOccupancyType_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwOccupancyType.Enabled = this.chkOccupancyType.Checked;
      if (this.chkOccupancyType.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwOccupancyType.Items)
        listViewItem.Checked = false;
    }

    private void chkPropertyState_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwPropertyState.Enabled = this.chkPropertyState.Checked;
      if (this.chkPropertyState.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwPropertyState.Items)
        listViewItem.Checked = false;
    }

    private void chkPlanCode_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwPlanCode.Enabled = this.chkPlanCode.Checked;
      if (this.chkPlanCode.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwPlanCode.Items)
        listViewItem.Checked = false;
    }

    private void chkFormVersion_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwFormVersion.Enabled = this.chkFormVersion.Checked;
      if (this.chkFormVersion.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwFormVersion.Items)
        listViewItem.Checked = false;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.saveCriteria())
        return;
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DocumentCriteriaDialog));
      ListViewItem listViewItem1 = new ListViewItem("Purchase");
      ListViewItem listViewItem2 = new ListViewItem("Cash-Out Refi");
      ListViewItem listViewItem3 = new ListViewItem("No Cash-Out Refi");
      ListViewItem listViewItem4 = new ListViewItem("Construction");
      ListViewItem listViewItem5 = new ListViewItem("Construction-Perm");
      ListViewItem listViewItem6 = new ListViewItem("Other");
      ListViewItem listViewItem7 = new ListViewItem("Primary");
      ListViewItem listViewItem8 = new ListViewItem("Secondary");
      ListViewItem listViewItem9 = new ListViewItem("Investment");
      ListViewItem listViewItem10 = new ListViewItem("Conv");
      ListViewItem listViewItem11 = new ListViewItem("FHA");
      ListViewItem listViewItem12 = new ListViewItem("VA");
      ListViewItem listViewItem13 = new ListViewItem("USDA-RHS");
      ListViewItem listViewItem14 = new ListViewItem("Other");
      ListViewItem listViewItem15 = new ListViewItem("HELOC");
      ListViewItem listViewItem16 = new ListViewItem("First");
      ListViewItem listViewItem17 = new ListViewItem("Second");
      ListViewItem listViewItem18 = new ListViewItem("Fixed");
      ListViewItem listViewItem19 = new ListViewItem("GPM");
      ListViewItem listViewItem20 = new ListViewItem("ARM");
      ListViewItem listViewItem21 = new ListViewItem("Other");
      ListViewItem listViewItem22 = new ListViewItem("AK");
      ListViewItem listViewItem23 = new ListViewItem("AL");
      ListViewItem listViewItem24 = new ListViewItem("AZ");
      ListViewItem listViewItem25 = new ListViewItem("AR");
      ListViewItem listViewItem26 = new ListViewItem("CA");
      ListViewItem listViewItem27 = new ListViewItem("CO");
      ListViewItem listViewItem28 = new ListViewItem("CT");
      ListViewItem listViewItem29 = new ListViewItem("DE");
      ListViewItem listViewItem30 = new ListViewItem("DC");
      ListViewItem listViewItem31 = new ListViewItem("FL");
      ListViewItem listViewItem32 = new ListViewItem("GA");
      ListViewItem listViewItem33 = new ListViewItem("HI");
      ListViewItem listViewItem34 = new ListViewItem("ID");
      ListViewItem listViewItem35 = new ListViewItem("IL");
      ListViewItem listViewItem36 = new ListViewItem("IN");
      ListViewItem listViewItem37 = new ListViewItem("IA");
      ListViewItem listViewItem38 = new ListViewItem("KS");
      ListViewItem listViewItem39 = new ListViewItem("KY");
      ListViewItem listViewItem40 = new ListViewItem("LA");
      ListViewItem listViewItem41 = new ListViewItem("ME");
      ListViewItem listViewItem42 = new ListViewItem("MD");
      ListViewItem listViewItem43 = new ListViewItem("MA");
      ListViewItem listViewItem44 = new ListViewItem("MI");
      ListViewItem listViewItem45 = new ListViewItem("MN");
      ListViewItem listViewItem46 = new ListViewItem("MS");
      ListViewItem listViewItem47 = new ListViewItem("MO");
      ListViewItem listViewItem48 = new ListViewItem("MT");
      ListViewItem listViewItem49 = new ListViewItem("NE");
      ListViewItem listViewItem50 = new ListViewItem("NV");
      ListViewItem listViewItem51 = new ListViewItem("NH");
      ListViewItem listViewItem52 = new ListViewItem("NJ");
      ListViewItem listViewItem53 = new ListViewItem("NM");
      ListViewItem listViewItem54 = new ListViewItem("NY");
      ListViewItem listViewItem55 = new ListViewItem("NC");
      ListViewItem listViewItem56 = new ListViewItem("ND");
      ListViewItem listViewItem57 = new ListViewItem("OH");
      ListViewItem listViewItem58 = new ListViewItem("OK");
      ListViewItem listViewItem59 = new ListViewItem("OR");
      ListViewItem listViewItem60 = new ListViewItem("PA");
      ListViewItem listViewItem61 = new ListViewItem("RI");
      ListViewItem listViewItem62 = new ListViewItem("SC");
      ListViewItem listViewItem63 = new ListViewItem("SD");
      ListViewItem listViewItem64 = new ListViewItem("TN");
      ListViewItem listViewItem65 = new ListViewItem("TX");
      ListViewItem listViewItem66 = new ListViewItem("UT");
      ListViewItem listViewItem67 = new ListViewItem("VT");
      ListViewItem listViewItem68 = new ListViewItem("VA");
      ListViewItem listViewItem69 = new ListViewItem("WA");
      ListViewItem listViewItem70 = new ListViewItem("WV");
      ListViewItem listViewItem71 = new ListViewItem("WI");
      ListViewItem listViewItem72 = new ListViewItem("WY");
      ListViewItem listViewItem73 = new ListViewItem("RESPA-TILA 2015 LE and CD");
      ListViewItem listViewItem74 = new ListViewItem("2010 GFE and HUD-1");
      ListViewItem listViewItem75 = new ListViewItem("Old GFE and HUD-1");
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lblCriteria = new Label();
      this.chkLoanType = new CheckBox();
      this.chkLoanPurpose = new CheckBox();
      this.chkLienType = new CheckBox();
      this.chkOccupancyType = new CheckBox();
      this.chkAmortType = new CheckBox();
      this.chkPropertyState = new CheckBox();
      this.lvwLoanPurpose = new ListView();
      this.lvwOccupancyType = new ListView();
      this.lvwLoanType = new ListView();
      this.lvwLienType = new ListView();
      this.lvwAmortType = new ListView();
      this.lvwPropertyState = new ListView();
      this.lvwPlanCode = new ListView();
      this.chkPlanCode = new CheckBox();
      this.lvwFormVersion = new ListView();
      this.chkFormVersion = new CheckBox();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(524, 370);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(76, 24);
      this.btnOK.TabIndex = 17;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(606, 370);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(76, 24);
      this.btnCancel.TabIndex = 18;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.lblCriteria.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblCriteria.Location = new Point(12, 12);
      this.lblCriteria.Name = "lblCriteria";
      this.lblCriteria.Size = new Size(670, 60);
      this.lblCriteria.TabIndex = 0;
      this.lblCriteria.Text = componentResourceManager.GetString("lblCriteria.Text");
      this.chkLoanType.AutoSize = true;
      this.chkLoanType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkLoanType.Location = new Point(284, 80);
      this.chkLoanType.Name = "chkLoanType";
      this.chkLoanType.Size = new Size(82, 18);
      this.chkLoanType.TabIndex = 5;
      this.chkLoanType.Text = "Loan &Type";
      this.chkLoanType.UseVisualStyleBackColor = true;
      this.chkLoanType.CheckedChanged += new EventHandler(this.chkLoanType_CheckedChanged);
      this.chkLoanPurpose.AutoSize = true;
      this.chkLoanPurpose.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkLoanPurpose.Location = new Point(12, 80);
      this.chkLoanPurpose.Name = "chkLoanPurpose";
      this.chkLoanPurpose.Size = new Size(117, 18);
      this.chkLoanPurpose.TabIndex = 1;
      this.chkLoanPurpose.Text = "&Purpose of Loan";
      this.chkLoanPurpose.UseVisualStyleBackColor = true;
      this.chkLoanPurpose.CheckedChanged += new EventHandler(this.chkLoanPurpose_CheckedChanged);
      this.chkLienType.AutoSize = true;
      this.chkLienType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkLienType.Location = new Point(12, 224);
      this.chkLienType.Name = "chkLienType";
      this.chkLienType.Size = new Size(98, 18);
      this.chkLienType.TabIndex = 9;
      this.chkLienType.Text = "&Lien Position";
      this.chkLienType.UseVisualStyleBackColor = true;
      this.chkLienType.CheckedChanged += new EventHandler(this.chkLienType_CheckedChanged);
      this.chkOccupancyType.AutoSize = true;
      this.chkOccupancyType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkOccupancyType.Location = new Point(148, 80);
      this.chkOccupancyType.Name = "chkOccupancyType";
      this.chkOccupancyType.Size = new Size(113, 18);
      this.chkOccupancyType.TabIndex = 3;
      this.chkOccupancyType.Text = "Property &Will Be";
      this.chkOccupancyType.UseVisualStyleBackColor = true;
      this.chkOccupancyType.CheckedChanged += new EventHandler(this.chkOccupancyType_CheckedChanged);
      this.chkAmortType.AutoSize = true;
      this.chkAmortType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkAmortType.Location = new Point(148, 224);
      this.chkAmortType.Name = "chkAmortType";
      this.chkAmortType.Size = new Size(126, 18);
      this.chkAmortType.TabIndex = 11;
      this.chkAmortType.Text = "&Amortization Type";
      this.chkAmortType.UseVisualStyleBackColor = true;
      this.chkAmortType.CheckedChanged += new EventHandler(this.chkAmortType_CheckedChanged);
      this.chkPropertyState.AutoSize = true;
      this.chkPropertyState.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkPropertyState.Location = new Point(284, 224);
      this.chkPropertyState.Name = "chkPropertyState";
      this.chkPropertyState.Size = new Size(105, 18);
      this.chkPropertyState.TabIndex = 13;
      this.chkPropertyState.Text = "P&roperty State";
      this.chkPropertyState.UseVisualStyleBackColor = true;
      this.chkPropertyState.CheckedChanged += new EventHandler(this.chkPropertyState_CheckedChanged);
      this.lvwLoanPurpose.AutoArrange = false;
      this.lvwLoanPurpose.CheckBoxes = true;
      this.lvwLoanPurpose.Enabled = false;
      this.lvwLoanPurpose.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem1.StateImageIndex = 0;
      listViewItem1.Tag = (object) "Purchase";
      listViewItem2.StateImageIndex = 0;
      listViewItem2.Tag = (object) "Cash-Out Refinance";
      listViewItem3.StateImageIndex = 0;
      listViewItem3.Tag = (object) "NoCash-Out Refinance";
      listViewItem4.StateImageIndex = 0;
      listViewItem4.Tag = (object) "ConstructionOnly";
      listViewItem5.StateImageIndex = 0;
      listViewItem5.Tag = (object) "ConstructionToPermanent";
      listViewItem6.StateImageIndex = 0;
      listViewItem6.Tag = (object) "Other";
      this.lvwLoanPurpose.Items.AddRange(new ListViewItem[6]
      {
        listViewItem1,
        listViewItem2,
        listViewItem3,
        listViewItem4,
        listViewItem5,
        listViewItem6
      });
      this.lvwLoanPurpose.Location = new Point(16, 100);
      this.lvwLoanPurpose.MultiSelect = false;
      this.lvwLoanPurpose.Name = "lvwLoanPurpose";
      this.lvwLoanPurpose.Scrollable = false;
      this.lvwLoanPurpose.ShowGroups = false;
      this.lvwLoanPurpose.Size = new Size(120, 116);
      this.lvwLoanPurpose.TabIndex = 2;
      this.lvwLoanPurpose.UseCompatibleStateImageBehavior = false;
      this.lvwLoanPurpose.View = View.SmallIcon;
      this.lvwOccupancyType.AutoArrange = false;
      this.lvwOccupancyType.CheckBoxes = true;
      this.lvwOccupancyType.Enabled = false;
      this.lvwOccupancyType.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem7.StateImageIndex = 0;
      listViewItem7.Tag = (object) "PrimaryResidence";
      listViewItem8.StateImageIndex = 0;
      listViewItem8.Tag = (object) "SecondHome";
      listViewItem9.StateImageIndex = 0;
      listViewItem9.Tag = (object) "Investor";
      this.lvwOccupancyType.Items.AddRange(new ListViewItem[3]
      {
        listViewItem7,
        listViewItem8,
        listViewItem9
      });
      this.lvwOccupancyType.Location = new Point(152, 100);
      this.lvwOccupancyType.MultiSelect = false;
      this.lvwOccupancyType.Name = "lvwOccupancyType";
      this.lvwOccupancyType.Scrollable = false;
      this.lvwOccupancyType.ShowGroups = false;
      this.lvwOccupancyType.Size = new Size(120, 116);
      this.lvwOccupancyType.TabIndex = 4;
      this.lvwOccupancyType.UseCompatibleStateImageBehavior = false;
      this.lvwOccupancyType.View = View.SmallIcon;
      this.lvwLoanType.AutoArrange = false;
      this.lvwLoanType.CheckBoxes = true;
      this.lvwLoanType.Enabled = false;
      this.lvwLoanType.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem10.StateImageIndex = 0;
      listViewItem10.Tag = (object) "Conventional";
      listViewItem11.StateImageIndex = 0;
      listViewItem11.Tag = (object) "FHA";
      listViewItem12.StateImageIndex = 0;
      listViewItem12.Tag = (object) "VA";
      listViewItem13.StateImageIndex = 0;
      listViewItem13.Tag = (object) "FarmersHomeAdministration";
      listViewItem14.StateImageIndex = 0;
      listViewItem14.Tag = (object) "Other";
      listViewItem15.StateImageIndex = 0;
      listViewItem15.Tag = (object) "HELOC";
      this.lvwLoanType.Items.AddRange(new ListViewItem[6]
      {
        listViewItem10,
        listViewItem11,
        listViewItem12,
        listViewItem13,
        listViewItem14,
        listViewItem15
      });
      this.lvwLoanType.Location = new Point(288, 100);
      this.lvwLoanType.MultiSelect = false;
      this.lvwLoanType.Name = "lvwLoanType";
      this.lvwLoanType.Scrollable = false;
      this.lvwLoanType.ShowGroups = false;
      this.lvwLoanType.Size = new Size(120, 116);
      this.lvwLoanType.TabIndex = 6;
      this.lvwLoanType.UseCompatibleStateImageBehavior = false;
      this.lvwLoanType.View = View.SmallIcon;
      this.lvwLienType.AutoArrange = false;
      this.lvwLienType.CheckBoxes = true;
      this.lvwLienType.Enabled = false;
      this.lvwLienType.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem16.StateImageIndex = 0;
      listViewItem16.Tag = (object) "FirstLien";
      listViewItem17.StateImageIndex = 0;
      listViewItem17.Tag = (object) "SecondLien";
      this.lvwLienType.Items.AddRange(new ListViewItem[2]
      {
        listViewItem16,
        listViewItem17
      });
      this.lvwLienType.Location = new Point(16, 244);
      this.lvwLienType.MultiSelect = false;
      this.lvwLienType.Name = "lvwLienType";
      this.lvwLienType.Scrollable = false;
      this.lvwLienType.ShowGroups = false;
      this.lvwLienType.Size = new Size(120, 116);
      this.lvwLienType.TabIndex = 10;
      this.lvwLienType.UseCompatibleStateImageBehavior = false;
      this.lvwLienType.View = View.SmallIcon;
      this.lvwAmortType.AutoArrange = false;
      this.lvwAmortType.CheckBoxes = true;
      this.lvwAmortType.Enabled = false;
      this.lvwAmortType.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem18.StateImageIndex = 0;
      listViewItem18.Tag = (object) "Fixed";
      listViewItem19.StateImageIndex = 0;
      listViewItem19.Tag = (object) "GraduatedPaymentMortgage";
      listViewItem20.StateImageIndex = 0;
      listViewItem20.Tag = (object) "AdjustableRate";
      listViewItem21.StateImageIndex = 0;
      listViewItem21.Tag = (object) "OtherAmortizationType";
      this.lvwAmortType.Items.AddRange(new ListViewItem[4]
      {
        listViewItem18,
        listViewItem19,
        listViewItem20,
        listViewItem21
      });
      this.lvwAmortType.Location = new Point(152, 244);
      this.lvwAmortType.MultiSelect = false;
      this.lvwAmortType.Name = "lvwAmortType";
      this.lvwAmortType.Scrollable = false;
      this.lvwAmortType.ShowGroups = false;
      this.lvwAmortType.Size = new Size(120, 116);
      this.lvwAmortType.TabIndex = 12;
      this.lvwAmortType.UseCompatibleStateImageBehavior = false;
      this.lvwAmortType.View = View.SmallIcon;
      this.lvwPropertyState.AutoArrange = false;
      this.lvwPropertyState.CheckBoxes = true;
      this.lvwPropertyState.Enabled = false;
      this.lvwPropertyState.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem22.StateImageIndex = 0;
      listViewItem22.Tag = (object) "AK";
      listViewItem23.StateImageIndex = 0;
      listViewItem23.Tag = (object) "AL";
      listViewItem24.StateImageIndex = 0;
      listViewItem24.Tag = (object) "AZ";
      listViewItem25.StateImageIndex = 0;
      listViewItem25.Tag = (object) "AR";
      listViewItem26.StateImageIndex = 0;
      listViewItem26.Tag = (object) "CA";
      listViewItem27.StateImageIndex = 0;
      listViewItem27.Tag = (object) "CO";
      listViewItem28.StateImageIndex = 0;
      listViewItem28.Tag = (object) "CT";
      listViewItem29.StateImageIndex = 0;
      listViewItem29.Tag = (object) "DE";
      listViewItem30.StateImageIndex = 0;
      listViewItem30.Tag = (object) "DC";
      listViewItem31.StateImageIndex = 0;
      listViewItem31.Tag = (object) "FL";
      listViewItem32.StateImageIndex = 0;
      listViewItem32.Tag = (object) "GA";
      listViewItem33.StateImageIndex = 0;
      listViewItem33.Tag = (object) "HI";
      listViewItem34.StateImageIndex = 0;
      listViewItem34.Tag = (object) "ID";
      listViewItem35.StateImageIndex = 0;
      listViewItem35.Tag = (object) "IL";
      listViewItem36.StateImageIndex = 0;
      listViewItem36.Tag = (object) "IN";
      listViewItem37.StateImageIndex = 0;
      listViewItem37.Tag = (object) "IA";
      listViewItem38.StateImageIndex = 0;
      listViewItem38.Tag = (object) "KS";
      listViewItem39.StateImageIndex = 0;
      listViewItem39.Tag = (object) "KY";
      listViewItem40.StateImageIndex = 0;
      listViewItem40.Tag = (object) "LA";
      listViewItem41.StateImageIndex = 0;
      listViewItem41.Tag = (object) "ME";
      listViewItem42.StateImageIndex = 0;
      listViewItem42.Tag = (object) "MD";
      listViewItem43.StateImageIndex = 0;
      listViewItem43.Tag = (object) "MA";
      listViewItem44.StateImageIndex = 0;
      listViewItem44.Tag = (object) "MI";
      listViewItem45.StateImageIndex = 0;
      listViewItem45.Tag = (object) "MN";
      listViewItem46.StateImageIndex = 0;
      listViewItem46.Tag = (object) "MS";
      listViewItem47.StateImageIndex = 0;
      listViewItem47.Tag = (object) "MO";
      listViewItem48.StateImageIndex = 0;
      listViewItem48.Tag = (object) "MT";
      listViewItem49.StateImageIndex = 0;
      listViewItem49.Tag = (object) "NE";
      listViewItem50.StateImageIndex = 0;
      listViewItem50.Tag = (object) "NV";
      listViewItem51.StateImageIndex = 0;
      listViewItem51.Tag = (object) "NH";
      listViewItem52.StateImageIndex = 0;
      listViewItem52.Tag = (object) "NJ";
      listViewItem53.StateImageIndex = 0;
      listViewItem53.Tag = (object) "NM";
      listViewItem54.StateImageIndex = 0;
      listViewItem54.Tag = (object) "NY";
      listViewItem55.StateImageIndex = 0;
      listViewItem55.Tag = (object) "NC";
      listViewItem56.StateImageIndex = 0;
      listViewItem56.Tag = (object) "ND";
      listViewItem57.StateImageIndex = 0;
      listViewItem57.Tag = (object) "OH";
      listViewItem58.StateImageIndex = 0;
      listViewItem58.Tag = (object) "OK";
      listViewItem59.StateImageIndex = 0;
      listViewItem59.Tag = (object) "OR";
      listViewItem60.StateImageIndex = 0;
      listViewItem60.Tag = (object) "PA";
      listViewItem61.StateImageIndex = 0;
      listViewItem61.Tag = (object) "RI";
      listViewItem62.StateImageIndex = 0;
      listViewItem62.Tag = (object) "SC";
      listViewItem63.StateImageIndex = 0;
      listViewItem63.Tag = (object) "SD";
      listViewItem64.StateImageIndex = 0;
      listViewItem64.Tag = (object) "TN";
      listViewItem65.StateImageIndex = 0;
      listViewItem65.Tag = (object) "TX";
      listViewItem66.StateImageIndex = 0;
      listViewItem66.Tag = (object) "UT";
      listViewItem67.StateImageIndex = 0;
      listViewItem67.Tag = (object) "VT";
      listViewItem68.StateImageIndex = 0;
      listViewItem68.Tag = (object) "VA";
      listViewItem69.StateImageIndex = 0;
      listViewItem69.Tag = (object) "WA";
      listViewItem70.StateImageIndex = 0;
      listViewItem70.Tag = (object) "WV";
      listViewItem71.StateImageIndex = 0;
      listViewItem71.Tag = (object) "WI";
      listViewItem72.StateImageIndex = 0;
      listViewItem72.Tag = (object) "WY";
      this.lvwPropertyState.Items.AddRange(new ListViewItem[51]
      {
        listViewItem22,
        listViewItem23,
        listViewItem24,
        listViewItem25,
        listViewItem26,
        listViewItem27,
        listViewItem28,
        listViewItem29,
        listViewItem30,
        listViewItem31,
        listViewItem32,
        listViewItem33,
        listViewItem34,
        listViewItem35,
        listViewItem36,
        listViewItem37,
        listViewItem38,
        listViewItem39,
        listViewItem40,
        listViewItem41,
        listViewItem42,
        listViewItem43,
        listViewItem44,
        listViewItem45,
        listViewItem46,
        listViewItem47,
        listViewItem48,
        listViewItem49,
        listViewItem50,
        listViewItem51,
        listViewItem52,
        listViewItem53,
        listViewItem54,
        listViewItem55,
        listViewItem56,
        listViewItem57,
        listViewItem58,
        listViewItem59,
        listViewItem60,
        listViewItem61,
        listViewItem62,
        listViewItem63,
        listViewItem64,
        listViewItem65,
        listViewItem66,
        listViewItem67,
        listViewItem68,
        listViewItem69,
        listViewItem70,
        listViewItem71,
        listViewItem72
      });
      this.lvwPropertyState.LabelWrap = false;
      this.lvwPropertyState.Location = new Point(288, 244);
      this.lvwPropertyState.MultiSelect = false;
      this.lvwPropertyState.Name = "lvwPropertyState";
      this.lvwPropertyState.ShowGroups = false;
      this.lvwPropertyState.Size = new Size(120, 116);
      this.lvwPropertyState.TabIndex = 14;
      this.lvwPropertyState.UseCompatibleStateImageBehavior = false;
      this.lvwPropertyState.View = View.SmallIcon;
      this.lvwPlanCode.AutoArrange = false;
      this.lvwPlanCode.CheckBoxes = true;
      this.lvwPlanCode.Enabled = false;
      this.lvwPlanCode.HeaderStyle = ColumnHeaderStyle.None;
      this.lvwPlanCode.Location = new Point(424, 244);
      this.lvwPlanCode.MultiSelect = false;
      this.lvwPlanCode.Name = "lvwPlanCode";
      this.lvwPlanCode.ShowGroups = false;
      this.lvwPlanCode.Size = new Size(256, 116);
      this.lvwPlanCode.TabIndex = 16;
      this.lvwPlanCode.UseCompatibleStateImageBehavior = false;
      this.lvwPlanCode.View = View.SmallIcon;
      this.chkPlanCode.AutoSize = true;
      this.chkPlanCode.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkPlanCode.Location = new Point(420, 224);
      this.chkPlanCode.Name = "chkPlanCode";
      this.chkPlanCode.Size = new Size(81, 18);
      this.chkPlanCode.TabIndex = 15;
      this.chkPlanCode.Text = "Plan &Code";
      this.chkPlanCode.UseVisualStyleBackColor = true;
      this.chkPlanCode.CheckedChanged += new EventHandler(this.chkPlanCode_CheckedChanged);
      this.lvwFormVersion.AutoArrange = false;
      this.lvwFormVersion.CheckBoxes = true;
      this.lvwFormVersion.Enabled = false;
      this.lvwFormVersion.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem73.StateImageIndex = 0;
      listViewItem73.Tag = (object) "RESPA-TILA 2015 LE and CD";
      listViewItem74.StateImageIndex = 0;
      listViewItem74.Tag = (object) "RESPA 2010 GFE and HUD-1";
      listViewItem75.StateImageIndex = 0;
      listViewItem75.Tag = (object) "Old GFE and HUD-1";
      this.lvwFormVersion.Items.AddRange(new ListViewItem[3]
      {
        listViewItem73,
        listViewItem74,
        listViewItem75
      });
      this.lvwFormVersion.LabelWrap = false;
      this.lvwFormVersion.Location = new Point(424, 100);
      this.lvwFormVersion.MultiSelect = false;
      this.lvwFormVersion.Name = "lvwFormVersion";
      this.lvwFormVersion.Scrollable = false;
      this.lvwFormVersion.ShowGroups = false;
      this.lvwFormVersion.Size = new Size(256, 116);
      this.lvwFormVersion.TabIndex = 8;
      this.lvwFormVersion.UseCompatibleStateImageBehavior = false;
      this.lvwFormVersion.View = View.SmallIcon;
      this.chkFormVersion.AutoSize = true;
      this.chkFormVersion.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkFormVersion.Location = new Point(420, 80);
      this.chkFormVersion.Name = "chkFormVersion";
      this.chkFormVersion.Size = new Size(101, 18);
      this.chkFormVersion.TabIndex = 7;
      this.chkFormVersion.Text = "&Form Version";
      this.chkFormVersion.UseVisualStyleBackColor = true;
      this.chkFormVersion.CheckedChanged += new EventHandler(this.chkFormVersion_CheckedChanged);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(695, 405);
      this.Controls.Add((Control) this.lvwFormVersion);
      this.Controls.Add((Control) this.chkFormVersion);
      this.Controls.Add((Control) this.lvwPlanCode);
      this.Controls.Add((Control) this.chkPlanCode);
      this.Controls.Add((Control) this.lvwPropertyState);
      this.Controls.Add((Control) this.lvwAmortType);
      this.Controls.Add((Control) this.lvwLienType);
      this.Controls.Add((Control) this.lvwLoanType);
      this.Controls.Add((Control) this.lvwOccupancyType);
      this.Controls.Add((Control) this.lvwLoanPurpose);
      this.Controls.Add((Control) this.chkAmortType);
      this.Controls.Add((Control) this.chkOccupancyType);
      this.Controls.Add((Control) this.chkLoanType);
      this.Controls.Add((Control) this.chkLoanPurpose);
      this.Controls.Add((Control) this.chkLienType);
      this.Controls.Add((Control) this.chkPropertyState);
      this.Controls.Add((Control) this.lblCriteria);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DocumentCriteriaDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Document Inclusion";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
