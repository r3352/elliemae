// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.PreClosingCriteriaDialog
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class PreClosingCriteriaDialog : Form
  {
    private PreClosingCriteria criteria;
    private Sessions.Session session;
    private IContainer components;
    private ListView lvwPropertyStates;
    private ListView lvwAmortTypes;
    private ListView lvwLienTypes;
    private ListView lvwLoanTypes;
    private ListView lvwOccupancyTypes;
    private ListView lvwLoanPurposes;
    private CheckBox chkAmortType;
    private CheckBox chkOccupancyType;
    private CheckBox chkLoanType;
    private CheckBox chkLoanPurpose;
    private CheckBox chkLienType;
    private CheckBox chkPropertyState;
    private Label lblCriteria;
    private Button btnCancel;
    private Button btnOK;
    private ListView lvwEntityTypes;
    private ListView lvwChannelTypes;
    private CheckBox chkEntityType;
    private CheckBox chkChannelType;
    private ListView lvwLoanLock;
    private CheckBox chkLoanLock;
    private CheckBox chkChangedCircumstance;
    private ListView lvwChangedCircumstance;
    private ColumnHeader colChangedCircum;
    private ColumnHeader colChannel;
    private ColumnHeader colLoanType;
    private ColumnHeader colEntityType;
    private ColumnHeader colPropState;
    private ColumnHeader colAmortType;
    private ColumnHeader colLienPos;
    private ColumnHeader colPropWillBe;
    private ColumnHeader colPurposeOfLoan;
    private ColumnHeader colLockState;
    private ListView lvwPlanCode;
    private CheckBox chkPlanCode;
    private ToolTip toolTip1;

    public PreClosingCriteriaDialog(PreClosingCriteria criteria, Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.loadPlanCodes();
      this.loadChangeOfCircumstance();
      this.criteria = criteria;
      this.loadCriteria();
    }

    public PreClosingCriteria Criteria => this.criteria;

    private void loadPlanCodes()
    {
      if (!this.session.Application.GetService<ILoanServices>().IsEncompassDocServiceAvailable(DocumentOrderType.Closing))
      {
        this.chkPlanCode.Visible = false;
        this.lvwPlanCode.Visible = false;
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

    private void loadChangeOfCircumstance()
    {
      this.lvwChangedCircumstance.Items.Clear();
      List<ChangeCircumstanceSettings> circumstanceSettings1 = this.session.ConfigurationManager.GetAllChangeCircumstanceSettings();
      List<string[]> strArrayList = new List<string[]>();
      foreach (ChangeCircumstanceSettings circumstanceSettings2 in circumstanceSettings1)
      {
        string[] strArray = new string[4]
        {
          circumstanceSettings2.Code,
          circumstanceSettings2.Description,
          circumstanceSettings2.Comment,
          circumstanceSettings2.Reason.ToString()
        };
        strArrayList.Add(strArray);
      }
      foreach (string[] strArray in strArrayList)
        this.lvwChangedCircumstance.Items.Add(new ListViewItem(strArray[1])
        {
          Tag = (object) strArray[0],
          ToolTipText = strArray[2]
        });
    }

    private void loadCriteria()
    {
      if (this.criteria == null)
        return;
      this.chkAmortType.Checked = this.criteria.AmortTypeValues != null;
      if (this.chkAmortType.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwAmortTypes.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.AmortTypeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkLienType.Checked = this.criteria.LienTypeValues != null;
      if (this.chkLienType.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwLienTypes.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.LienTypeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkLoanPurpose.Checked = this.criteria.LoanPurposeValues != null;
      if (this.chkLoanPurpose.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwLoanPurposes.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.LoanPurposeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkLoanType.Checked = this.criteria.LoanTypeValues != null;
      if (this.chkLoanType.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwLoanTypes.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.LoanTypeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkOccupancyType.Checked = this.criteria.OccupancyTypeValues != null;
      if (this.chkOccupancyType.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwOccupancyTypes.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.OccupancyTypeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkPropertyState.Checked = this.criteria.PropertyStateValues != null;
      if (this.chkPropertyState.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwPropertyStates.Items)
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
      this.chkChannelType.Checked = this.criteria.ChannelTypeValues != null;
      if (this.chkChannelType.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwChannelTypes.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.ChannelTypeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkEntityType.Checked = this.criteria.EntityTypeValues != null;
      if (this.chkEntityType.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwEntityTypes.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.EntityTypeValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkLoanLock.Checked = this.criteria.LoanLockValues != null;
      if (this.chkLoanLock.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwLoanLock.Items)
        {
          if (Array.IndexOf<object>((object[]) this.criteria.LoanLockValues, listViewItem.Tag) >= 0)
            listViewItem.Checked = true;
        }
      }
      this.chkChangedCircumstance.Checked = this.criteria.ChangedCircumstance != null;
      if (!this.chkChangedCircumstance.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwChangedCircumstance.Items)
      {
        if (Array.IndexOf<object>((object[]) this.criteria.ChangedCircumstance, listViewItem.Tag) >= 0)
          listViewItem.Checked = true;
      }
    }

    private bool saveCriteria()
    {
      if (!this.chkAmortType.Checked && !this.chkLienType.Checked && !this.chkLoanPurpose.Checked && !this.chkLoanType.Checked && !this.chkOccupancyType.Checked && !this.chkPropertyState.Checked && !this.chkChannelType.Checked && !this.chkEntityType.Checked && !this.chkPlanCode.Checked && !this.chkChangedCircumstance.Checked && !this.chkLoanLock.Checked)
      {
        this.criteria = (PreClosingCriteria) null;
        return true;
      }
      string empty = string.Empty;
      string[] amortTypeValues = (string[]) null;
      if (this.chkAmortType.Checked)
      {
        List<string> stringList = new List<string>();
        foreach (ListViewItem checkedItem in this.lvwAmortTypes.CheckedItems)
          stringList.Add(checkedItem.Tag as string);
        if (stringList.Count > 0)
          amortTypeValues = stringList.ToArray();
        else
          empty += "Amortization Type\r\n";
      }
      string[] lienTypeValues = (string[]) null;
      if (this.chkLienType.Checked)
      {
        List<string> stringList = new List<string>();
        foreach (ListViewItem checkedItem in this.lvwLienTypes.CheckedItems)
          stringList.Add(checkedItem.Tag as string);
        if (stringList.Count > 0)
          lienTypeValues = stringList.ToArray();
        else
          empty += "Lien Position\r\n";
      }
      string[] loanPurposeValues = (string[]) null;
      if (this.chkLoanPurpose.Checked)
      {
        List<string> stringList = new List<string>();
        foreach (ListViewItem checkedItem in this.lvwLoanPurposes.CheckedItems)
          stringList.Add(checkedItem.Tag as string);
        if (stringList.Count > 0)
          loanPurposeValues = stringList.ToArray();
        else
          empty += "Purpose of Loan\r\n";
      }
      string[] loanTypeValues = (string[]) null;
      if (this.chkLoanType.Checked)
      {
        List<string> stringList = new List<string>();
        foreach (ListViewItem checkedItem in this.lvwLoanTypes.CheckedItems)
          stringList.Add(checkedItem.Tag as string);
        if (stringList.Count > 0)
          loanTypeValues = stringList.ToArray();
        else
          empty += "Loan Type\r\n";
      }
      string[] occupancyTypeValues = (string[]) null;
      if (this.chkOccupancyType.Checked)
      {
        List<string> stringList = new List<string>();
        foreach (ListViewItem checkedItem in this.lvwOccupancyTypes.CheckedItems)
          stringList.Add(checkedItem.Tag as string);
        if (stringList.Count > 0)
          occupancyTypeValues = stringList.ToArray();
        else
          empty += "Property Will Be\r\n";
      }
      string[] propertyStateValues = (string[]) null;
      if (this.chkPropertyState.Checked)
      {
        List<string> stringList = new List<string>();
        foreach (ListViewItem checkedItem in this.lvwPropertyStates.CheckedItems)
          stringList.Add(checkedItem.Tag as string);
        if (stringList.Count > 0)
          propertyStateValues = stringList.ToArray();
        else
          empty += "Property State\r\n";
      }
      string[] planCodeValues = (string[]) null;
      if (this.chkPlanCode.Checked)
      {
        List<string> stringList = new List<string>();
        foreach (ListViewItem checkedItem in this.lvwPlanCode.CheckedItems)
          stringList.Add(checkedItem.Tag as string);
        if (stringList.Count > 0)
          planCodeValues = stringList.ToArray();
        else
          empty += "Plan Code\r\n";
      }
      string[] channelTypeValues = (string[]) null;
      if (this.chkChannelType.Checked)
      {
        List<string> stringList = new List<string>();
        foreach (ListViewItem checkedItem in this.lvwChannelTypes.CheckedItems)
          stringList.Add(checkedItem.Tag as string);
        if (stringList.Count > 0)
          channelTypeValues = stringList.ToArray();
        else
          empty += "Loan Channel\r\n";
      }
      string[] entityTypeValues = (string[]) null;
      if (this.chkEntityType.Checked)
      {
        List<string> stringList = new List<string>();
        foreach (ListViewItem checkedItem in this.lvwEntityTypes.CheckedItems)
          stringList.Add(checkedItem.Tag as string);
        if (stringList.Count > 0)
          entityTypeValues = stringList.ToArray();
        else
          empty += "Entity Type\r\n";
      }
      string[] loanLockValues = (string[]) null;
      if (this.chkLoanLock.Checked)
      {
        List<string> stringList = new List<string>();
        foreach (ListViewItem checkedItem in this.lvwLoanLock.CheckedItems)
          stringList.Add(checkedItem.Tag as string);
        if (stringList.Count > 0)
          loanLockValues = stringList.ToArray();
        else
          empty += "Loan Lock\r\n";
      }
      string[] changedCircumstance = (string[]) null;
      if (this.chkChangedCircumstance.Checked)
      {
        List<string> stringList = new List<string>();
        foreach (ListViewItem checkedItem in this.lvwChangedCircumstance.CheckedItems)
          stringList.Add(checkedItem.Tag as string);
        if (stringList.Count > 0)
          changedCircumstance = stringList.ToArray();
        else
          empty += "Change Circumstance\r\n";
      }
      if (empty != string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select at least one value for each of the following categories:\r\n\r\n" + empty, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      this.criteria = new PreClosingCriteria(amortTypeValues, lienTypeValues, loanPurposeValues, loanTypeValues, occupancyTypeValues, propertyStateValues, planCodeValues, channelTypeValues, entityTypeValues, loanLockValues, changedCircumstance);
      return true;
    }

    private void chkAmortType_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwAmortTypes.Enabled = this.chkAmortType.Checked;
      if (this.chkAmortType.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwAmortTypes.Items)
        listViewItem.Checked = false;
    }

    private void chkLienType_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwLienTypes.Enabled = this.chkLienType.Checked;
      if (this.chkLienType.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwLienTypes.Items)
        listViewItem.Checked = false;
    }

    private void chkLoanPurpose_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwLoanPurposes.Enabled = this.chkLoanPurpose.Checked;
      if (this.chkLoanPurpose.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwLoanPurposes.Items)
        listViewItem.Checked = false;
    }

    private void chkLoanType_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwLoanTypes.Enabled = this.chkLoanType.Checked;
      if (this.chkLoanType.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwLoanTypes.Items)
        listViewItem.Checked = false;
    }

    private void chkOccupancyType_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwOccupancyTypes.Enabled = this.chkOccupancyType.Checked;
      if (this.chkOccupancyType.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwOccupancyTypes.Items)
        listViewItem.Checked = false;
    }

    private void chkPropertyState_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwPropertyStates.Enabled = this.chkPropertyState.Checked;
      if (this.chkPropertyState.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwPropertyStates.Items)
        listViewItem.Checked = false;
    }

    private void chkChannelType_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwChannelTypes.Enabled = this.chkChannelType.Checked;
      if (this.chkChannelType.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwChannelTypes.Items)
        listViewItem.Checked = false;
    }

    private void chkEntityType_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwEntityTypes.Enabled = this.chkEntityType.Checked;
      if (this.chkEntityType.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwEntityTypes.Items)
        listViewItem.Checked = false;
    }

    private void chkLoanLock_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwLoanLock.Enabled = this.chkLoanLock.Checked;
      if (this.chkLoanLock.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwLoanLock.Items)
        listViewItem.Checked = false;
    }

    private void chkChangedCircumstance_CheckedChanged(object sender, EventArgs e)
    {
      this.lvwChangedCircumstance.Enabled = this.chkChangedCircumstance.Checked;
      if (this.chkChangedCircumstance.Checked)
        return;
      foreach (ListViewItem listViewItem in this.lvwChangedCircumstance.Items)
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

    private void okBtn_Click(object sender, EventArgs e)
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
      this.components = (IContainer) new System.ComponentModel.Container();
      ListViewItem listViewItem1 = new ListViewItem("AK");
      ListViewItem listViewItem2 = new ListViewItem("AL");
      ListViewItem listViewItem3 = new ListViewItem("AZ");
      ListViewItem listViewItem4 = new ListViewItem("AR");
      ListViewItem listViewItem5 = new ListViewItem("CA");
      ListViewItem listViewItem6 = new ListViewItem("CO");
      ListViewItem listViewItem7 = new ListViewItem("CT");
      ListViewItem listViewItem8 = new ListViewItem("DE");
      ListViewItem listViewItem9 = new ListViewItem("DC");
      ListViewItem listViewItem10 = new ListViewItem("FL");
      ListViewItem listViewItem11 = new ListViewItem("GA");
      ListViewItem listViewItem12 = new ListViewItem("HI");
      ListViewItem listViewItem13 = new ListViewItem("ID");
      ListViewItem listViewItem14 = new ListViewItem("IL");
      ListViewItem listViewItem15 = new ListViewItem("IN");
      ListViewItem listViewItem16 = new ListViewItem("IA");
      ListViewItem listViewItem17 = new ListViewItem("KS");
      ListViewItem listViewItem18 = new ListViewItem("KY");
      ListViewItem listViewItem19 = new ListViewItem("LA");
      ListViewItem listViewItem20 = new ListViewItem("ME");
      ListViewItem listViewItem21 = new ListViewItem("MD");
      ListViewItem listViewItem22 = new ListViewItem("MA");
      ListViewItem listViewItem23 = new ListViewItem("MI");
      ListViewItem listViewItem24 = new ListViewItem("MN");
      ListViewItem listViewItem25 = new ListViewItem("MS");
      ListViewItem listViewItem26 = new ListViewItem("MO");
      ListViewItem listViewItem27 = new ListViewItem("MT");
      ListViewItem listViewItem28 = new ListViewItem("NE");
      ListViewItem listViewItem29 = new ListViewItem("NV");
      ListViewItem listViewItem30 = new ListViewItem("NH");
      ListViewItem listViewItem31 = new ListViewItem("NJ");
      ListViewItem listViewItem32 = new ListViewItem("NM");
      ListViewItem listViewItem33 = new ListViewItem("NY");
      ListViewItem listViewItem34 = new ListViewItem("NC");
      ListViewItem listViewItem35 = new ListViewItem("ND");
      ListViewItem listViewItem36 = new ListViewItem("OH");
      ListViewItem listViewItem37 = new ListViewItem("OK");
      ListViewItem listViewItem38 = new ListViewItem("OR");
      ListViewItem listViewItem39 = new ListViewItem("PA");
      ListViewItem listViewItem40 = new ListViewItem("RI");
      ListViewItem listViewItem41 = new ListViewItem("SC");
      ListViewItem listViewItem42 = new ListViewItem("SD");
      ListViewItem listViewItem43 = new ListViewItem("TN");
      ListViewItem listViewItem44 = new ListViewItem("TX");
      ListViewItem listViewItem45 = new ListViewItem("UT");
      ListViewItem listViewItem46 = new ListViewItem("VT");
      ListViewItem listViewItem47 = new ListViewItem("VA");
      ListViewItem listViewItem48 = new ListViewItem("WA");
      ListViewItem listViewItem49 = new ListViewItem("WV");
      ListViewItem listViewItem50 = new ListViewItem("WI");
      ListViewItem listViewItem51 = new ListViewItem("WY");
      ListViewItem listViewItem52 = new ListViewItem("Fixed");
      ListViewItem listViewItem53 = new ListViewItem("GPM");
      ListViewItem listViewItem54 = new ListViewItem("ARM");
      ListViewItem listViewItem55 = new ListViewItem("Other");
      ListViewItem listViewItem56 = new ListViewItem("First");
      ListViewItem listViewItem57 = new ListViewItem("Second");
      ListViewItem listViewItem58 = new ListViewItem("Conv");
      ListViewItem listViewItem59 = new ListViewItem("FHA");
      ListViewItem listViewItem60 = new ListViewItem("VA");
      ListViewItem listViewItem61 = new ListViewItem("USDA-RHS");
      ListViewItem listViewItem62 = new ListViewItem("Other");
      ListViewItem listViewItem63 = new ListViewItem("HELOC");
      ListViewItem listViewItem64 = new ListViewItem("Primary");
      ListViewItem listViewItem65 = new ListViewItem("Secondary");
      ListViewItem listViewItem66 = new ListViewItem("Investment");
      ListViewItem listViewItem67 = new ListViewItem("Purchase");
      ListViewItem listViewItem68 = new ListViewItem("Cash-Out Refi");
      ListViewItem listViewItem69 = new ListViewItem("No Cash-Out Refi");
      ListViewItem listViewItem70 = new ListViewItem("Construction");
      ListViewItem listViewItem71 = new ListViewItem("Construction-Perm");
      ListViewItem listViewItem72 = new ListViewItem("Other");
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PreClosingCriteriaDialog));
      ListViewItem listViewItem73 = new ListViewItem("Broker");
      ListViewItem listViewItem74 = new ListViewItem("Lender");
      ListViewItem listViewItem75 = new ListViewItem("Banker - Retail");
      ListViewItem listViewItem76 = new ListViewItem("Banker - Wholesale");
      ListViewItem listViewItem77 = new ListViewItem("Brokered");
      ListViewItem listViewItem78 = new ListViewItem("Correspondent");
      ListViewItem listViewItem79 = new ListViewItem("Locked");
      ListViewItem listViewItem80 = new ListViewItem("Not Locked");
      this.lvwPropertyStates = new ListView();
      this.colPropState = new ColumnHeader();
      this.lvwAmortTypes = new ListView();
      this.colAmortType = new ColumnHeader();
      this.lvwLienTypes = new ListView();
      this.colLienPos = new ColumnHeader();
      this.lvwLoanTypes = new ListView();
      this.colLoanType = new ColumnHeader();
      this.lvwOccupancyTypes = new ListView();
      this.colPropWillBe = new ColumnHeader();
      this.lvwLoanPurposes = new ListView();
      this.colPurposeOfLoan = new ColumnHeader();
      this.chkAmortType = new CheckBox();
      this.chkOccupancyType = new CheckBox();
      this.chkLoanType = new CheckBox();
      this.chkLoanPurpose = new CheckBox();
      this.chkLienType = new CheckBox();
      this.chkPropertyState = new CheckBox();
      this.lblCriteria = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.lvwEntityTypes = new ListView();
      this.colEntityType = new ColumnHeader();
      this.lvwChannelTypes = new ListView();
      this.colChannel = new ColumnHeader();
      this.chkEntityType = new CheckBox();
      this.chkChannelType = new CheckBox();
      this.lvwLoanLock = new ListView();
      this.colLockState = new ColumnHeader();
      this.chkLoanLock = new CheckBox();
      this.chkChangedCircumstance = new CheckBox();
      this.lvwChangedCircumstance = new ListView();
      this.colChangedCircum = new ColumnHeader();
      this.lvwPlanCode = new ListView();
      this.chkPlanCode = new CheckBox();
      this.toolTip1 = new ToolTip(this.components);
      this.SuspendLayout();
      this.lvwPropertyStates.AutoArrange = false;
      this.lvwPropertyStates.CheckBoxes = true;
      this.lvwPropertyStates.Columns.AddRange(new ColumnHeader[1]
      {
        this.colPropState
      });
      this.lvwPropertyStates.Enabled = false;
      this.lvwPropertyStates.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem1.StateImageIndex = 0;
      listViewItem1.Tag = (object) "AK";
      listViewItem2.StateImageIndex = 0;
      listViewItem2.Tag = (object) "AL";
      listViewItem3.StateImageIndex = 0;
      listViewItem3.Tag = (object) "AZ";
      listViewItem4.StateImageIndex = 0;
      listViewItem4.Tag = (object) "AR";
      listViewItem5.StateImageIndex = 0;
      listViewItem5.Tag = (object) "CA";
      listViewItem6.StateImageIndex = 0;
      listViewItem6.Tag = (object) "CO";
      listViewItem7.StateImageIndex = 0;
      listViewItem7.Tag = (object) "CT";
      listViewItem8.StateImageIndex = 0;
      listViewItem8.Tag = (object) "DE";
      listViewItem9.StateImageIndex = 0;
      listViewItem9.Tag = (object) "DC";
      listViewItem10.StateImageIndex = 0;
      listViewItem10.Tag = (object) "FL";
      listViewItem11.StateImageIndex = 0;
      listViewItem11.Tag = (object) "GA";
      listViewItem12.StateImageIndex = 0;
      listViewItem12.Tag = (object) "HI";
      listViewItem13.StateImageIndex = 0;
      listViewItem13.Tag = (object) "ID";
      listViewItem14.StateImageIndex = 0;
      listViewItem14.Tag = (object) "IL";
      listViewItem15.StateImageIndex = 0;
      listViewItem15.Tag = (object) "IN";
      listViewItem16.StateImageIndex = 0;
      listViewItem16.Tag = (object) "IA";
      listViewItem17.StateImageIndex = 0;
      listViewItem17.Tag = (object) "KS";
      listViewItem18.StateImageIndex = 0;
      listViewItem18.Tag = (object) "KY";
      listViewItem19.StateImageIndex = 0;
      listViewItem19.Tag = (object) "LA";
      listViewItem20.StateImageIndex = 0;
      listViewItem20.Tag = (object) "ME";
      listViewItem21.StateImageIndex = 0;
      listViewItem21.Tag = (object) "MD";
      listViewItem22.StateImageIndex = 0;
      listViewItem22.Tag = (object) "MA";
      listViewItem23.StateImageIndex = 0;
      listViewItem23.Tag = (object) "MI";
      listViewItem24.StateImageIndex = 0;
      listViewItem24.Tag = (object) "MN";
      listViewItem25.StateImageIndex = 0;
      listViewItem25.Tag = (object) "MS";
      listViewItem26.StateImageIndex = 0;
      listViewItem26.Tag = (object) "MO";
      listViewItem27.StateImageIndex = 0;
      listViewItem27.Tag = (object) "MT";
      listViewItem28.StateImageIndex = 0;
      listViewItem28.Tag = (object) "NE";
      listViewItem29.StateImageIndex = 0;
      listViewItem29.Tag = (object) "NV";
      listViewItem30.StateImageIndex = 0;
      listViewItem30.Tag = (object) "NH";
      listViewItem31.StateImageIndex = 0;
      listViewItem31.Tag = (object) "NJ";
      listViewItem32.StateImageIndex = 0;
      listViewItem32.Tag = (object) "NM";
      listViewItem33.StateImageIndex = 0;
      listViewItem33.Tag = (object) "NY";
      listViewItem34.StateImageIndex = 0;
      listViewItem34.Tag = (object) "NC";
      listViewItem35.StateImageIndex = 0;
      listViewItem35.Tag = (object) "ND";
      listViewItem36.StateImageIndex = 0;
      listViewItem36.Tag = (object) "OH";
      listViewItem37.StateImageIndex = 0;
      listViewItem37.Tag = (object) "OK";
      listViewItem38.StateImageIndex = 0;
      listViewItem38.Tag = (object) "OR";
      listViewItem39.StateImageIndex = 0;
      listViewItem39.Tag = (object) "PA";
      listViewItem40.StateImageIndex = 0;
      listViewItem40.Tag = (object) "RI";
      listViewItem41.StateImageIndex = 0;
      listViewItem41.Tag = (object) "SC";
      listViewItem42.StateImageIndex = 0;
      listViewItem42.Tag = (object) "SD";
      listViewItem43.StateImageIndex = 0;
      listViewItem43.Tag = (object) "TN";
      listViewItem44.StateImageIndex = 0;
      listViewItem44.Tag = (object) "TX";
      listViewItem45.StateImageIndex = 0;
      listViewItem45.Tag = (object) "UT";
      listViewItem46.StateImageIndex = 0;
      listViewItem46.Tag = (object) "VT";
      listViewItem47.StateImageIndex = 0;
      listViewItem47.Tag = (object) "VA";
      listViewItem48.StateImageIndex = 0;
      listViewItem48.Tag = (object) "WA";
      listViewItem49.StateImageIndex = 0;
      listViewItem49.Tag = (object) "WV";
      listViewItem50.StateImageIndex = 0;
      listViewItem50.Tag = (object) "WI";
      listViewItem51.StateImageIndex = 0;
      listViewItem51.Tag = (object) "WY";
      this.lvwPropertyStates.Items.AddRange(new ListViewItem[51]
      {
        listViewItem1,
        listViewItem2,
        listViewItem3,
        listViewItem4,
        listViewItem5,
        listViewItem6,
        listViewItem7,
        listViewItem8,
        listViewItem9,
        listViewItem10,
        listViewItem11,
        listViewItem12,
        listViewItem13,
        listViewItem14,
        listViewItem15,
        listViewItem16,
        listViewItem17,
        listViewItem18,
        listViewItem19,
        listViewItem20,
        listViewItem21,
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
        listViewItem51
      });
      this.lvwPropertyStates.LabelWrap = false;
      this.lvwPropertyStates.Location = new Point(422, 84);
      this.lvwPropertyStates.MultiSelect = false;
      this.lvwPropertyStates.Name = "lvwPropertyStates";
      this.lvwPropertyStates.ShowGroups = false;
      this.lvwPropertyStates.Size = new Size(120, 116);
      this.lvwPropertyStates.TabIndex = 12;
      this.lvwPropertyStates.UseCompatibleStateImageBehavior = false;
      this.lvwPropertyStates.View = View.Details;
      this.lvwAmortTypes.AutoArrange = false;
      this.lvwAmortTypes.CheckBoxes = true;
      this.lvwAmortTypes.Columns.AddRange(new ColumnHeader[1]
      {
        this.colAmortType
      });
      this.lvwAmortTypes.Enabled = false;
      this.lvwAmortTypes.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem52.StateImageIndex = 0;
      listViewItem52.Tag = (object) "Fixed";
      listViewItem53.StateImageIndex = 0;
      listViewItem53.Tag = (object) "GraduatedPaymentMortgage";
      listViewItem54.StateImageIndex = 0;
      listViewItem54.Tag = (object) "AdjustableRate";
      listViewItem55.StateImageIndex = 0;
      listViewItem55.Tag = (object) "OtherAmortizationType";
      this.lvwAmortTypes.Items.AddRange(new ListViewItem[4]
      {
        listViewItem52,
        listViewItem53,
        listViewItem54,
        listViewItem55
      });
      this.lvwAmortTypes.Location = new Point(17, 229);
      this.lvwAmortTypes.MultiSelect = false;
      this.lvwAmortTypes.Name = "lvwAmortTypes";
      this.lvwAmortTypes.Scrollable = false;
      this.lvwAmortTypes.ShowGroups = false;
      this.lvwAmortTypes.Size = new Size(120, 116);
      this.lvwAmortTypes.TabIndex = 10;
      this.lvwAmortTypes.UseCompatibleStateImageBehavior = false;
      this.lvwAmortTypes.View = View.Details;
      this.colAmortType.Width = 120;
      this.lvwLienTypes.AutoArrange = false;
      this.lvwLienTypes.CheckBoxes = true;
      this.lvwLienTypes.Columns.AddRange(new ColumnHeader[1]
      {
        this.colLienPos
      });
      this.lvwLienTypes.Enabled = false;
      this.lvwLienTypes.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem56.StateImageIndex = 0;
      listViewItem56.Tag = (object) "FirstLien";
      listViewItem57.StateImageIndex = 0;
      listViewItem57.Tag = (object) "SecondLien";
      this.lvwLienTypes.Items.AddRange(new ListViewItem[2]
      {
        listViewItem56,
        listViewItem57
      });
      this.lvwLienTypes.Location = new Point(287, 229);
      this.lvwLienTypes.MultiSelect = false;
      this.lvwLienTypes.Name = "lvwLienTypes";
      this.lvwLienTypes.Scrollable = false;
      this.lvwLienTypes.ShowGroups = false;
      this.lvwLienTypes.Size = new Size(120, 116);
      this.lvwLienTypes.TabIndex = 8;
      this.lvwLienTypes.UseCompatibleStateImageBehavior = false;
      this.lvwLienTypes.View = View.Details;
      this.colLienPos.Width = 120;
      this.lvwLoanTypes.AutoArrange = false;
      this.lvwLoanTypes.CheckBoxes = true;
      this.lvwLoanTypes.Columns.AddRange(new ColumnHeader[1]
      {
        this.colLoanType
      });
      this.lvwLoanTypes.Enabled = false;
      this.lvwLoanTypes.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem58.StateImageIndex = 0;
      listViewItem58.Tag = (object) "Conventional";
      listViewItem59.StateImageIndex = 0;
      listViewItem59.Tag = (object) "FHA";
      listViewItem60.StateImageIndex = 0;
      listViewItem60.Tag = (object) "VA";
      listViewItem61.StateImageIndex = 0;
      listViewItem61.Tag = (object) "FarmersHomeAdministration";
      listViewItem62.StateImageIndex = 0;
      listViewItem62.Tag = (object) "Other";
      listViewItem63.StateImageIndex = 0;
      listViewItem63.Tag = (object) "HELOC";
      this.lvwLoanTypes.Items.AddRange(new ListViewItem[6]
      {
        listViewItem58,
        listViewItem59,
        listViewItem60,
        listViewItem61,
        listViewItem62,
        listViewItem63
      });
      this.lvwLoanTypes.Location = new Point(287, 84);
      this.lvwLoanTypes.MultiSelect = false;
      this.lvwLoanTypes.Name = "lvwLoanTypes";
      this.lvwLoanTypes.Scrollable = false;
      this.lvwLoanTypes.ShowGroups = false;
      this.lvwLoanTypes.Size = new Size(120, 116);
      this.lvwLoanTypes.TabIndex = 6;
      this.lvwLoanTypes.UseCompatibleStateImageBehavior = false;
      this.lvwLoanTypes.View = View.Details;
      this.colLoanType.Width = 120;
      this.lvwOccupancyTypes.AutoArrange = false;
      this.lvwOccupancyTypes.CheckBoxes = true;
      this.lvwOccupancyTypes.Columns.AddRange(new ColumnHeader[1]
      {
        this.colPropWillBe
      });
      this.lvwOccupancyTypes.Enabled = false;
      this.lvwOccupancyTypes.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem64.StateImageIndex = 0;
      listViewItem64.Tag = (object) "PrimaryResidence";
      listViewItem65.StateImageIndex = 0;
      listViewItem65.Tag = (object) "SecondHome";
      listViewItem66.StateImageIndex = 0;
      listViewItem66.Tag = (object) "Investor";
      this.lvwOccupancyTypes.Items.AddRange(new ListViewItem[3]
      {
        listViewItem64,
        listViewItem65,
        listViewItem66
      });
      this.lvwOccupancyTypes.Location = new Point(152, 229);
      this.lvwOccupancyTypes.MultiSelect = false;
      this.lvwOccupancyTypes.Name = "lvwOccupancyTypes";
      this.lvwOccupancyTypes.Scrollable = false;
      this.lvwOccupancyTypes.ShowGroups = false;
      this.lvwOccupancyTypes.Size = new Size(120, 116);
      this.lvwOccupancyTypes.TabIndex = 4;
      this.lvwOccupancyTypes.UseCompatibleStateImageBehavior = false;
      this.lvwOccupancyTypes.View = View.Details;
      this.colPropWillBe.Width = 120;
      this.lvwLoanPurposes.AutoArrange = false;
      this.lvwLoanPurposes.CheckBoxes = true;
      this.lvwLoanPurposes.Columns.AddRange(new ColumnHeader[1]
      {
        this.colPurposeOfLoan
      });
      this.lvwLoanPurposes.Enabled = false;
      this.lvwLoanPurposes.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem67.StateImageIndex = 0;
      listViewItem67.Tag = (object) "Purchase";
      listViewItem68.StateImageIndex = 0;
      listViewItem68.Tag = (object) "Cash-Out Refinance";
      listViewItem69.StateImageIndex = 0;
      listViewItem69.Tag = (object) "NoCash-Out Refinance";
      listViewItem70.StateImageIndex = 0;
      listViewItem70.Tag = (object) "ConstructionOnly";
      listViewItem71.StateImageIndex = 0;
      listViewItem71.Tag = (object) "ConstructionToPermanent";
      listViewItem72.StateImageIndex = 0;
      listViewItem72.Tag = (object) "Other";
      this.lvwLoanPurposes.Items.AddRange(new ListViewItem[6]
      {
        listViewItem67,
        listViewItem68,
        listViewItem69,
        listViewItem70,
        listViewItem71,
        listViewItem72
      });
      this.lvwLoanPurposes.Location = new Point(557, 84);
      this.lvwLoanPurposes.MultiSelect = false;
      this.lvwLoanPurposes.Name = "lvwLoanPurposes";
      this.lvwLoanPurposes.Scrollable = false;
      this.lvwLoanPurposes.ShowGroups = false;
      this.lvwLoanPurposes.Size = new Size(120, 116);
      this.lvwLoanPurposes.TabIndex = 2;
      this.lvwLoanPurposes.UseCompatibleStateImageBehavior = false;
      this.lvwLoanPurposes.View = View.Details;
      this.colPurposeOfLoan.Width = 120;
      this.chkAmortType.AutoSize = true;
      this.chkAmortType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkAmortType.Location = new Point(12, 209);
      this.chkAmortType.Name = "chkAmortType";
      this.chkAmortType.Size = new Size(126, 18);
      this.chkAmortType.TabIndex = 9;
      this.chkAmortType.Text = "&Amortization Type";
      this.chkAmortType.UseVisualStyleBackColor = true;
      this.chkAmortType.CheckedChanged += new EventHandler(this.chkAmortType_CheckedChanged);
      this.chkOccupancyType.AutoSize = true;
      this.chkOccupancyType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkOccupancyType.Location = new Point(146, 209);
      this.chkOccupancyType.Name = "chkOccupancyType";
      this.chkOccupancyType.Size = new Size(113, 18);
      this.chkOccupancyType.TabIndex = 3;
      this.chkOccupancyType.Text = "Property &Will Be";
      this.chkOccupancyType.UseVisualStyleBackColor = true;
      this.chkOccupancyType.CheckedChanged += new EventHandler(this.chkOccupancyType_CheckedChanged);
      this.chkLoanType.AutoSize = true;
      this.chkLoanType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkLoanType.Location = new Point(280, 64);
      this.chkLoanType.Name = "chkLoanType";
      this.chkLoanType.Size = new Size(82, 18);
      this.chkLoanType.TabIndex = 5;
      this.chkLoanType.Text = "Loan &Type";
      this.chkLoanType.UseVisualStyleBackColor = true;
      this.chkLoanType.CheckedChanged += new EventHandler(this.chkLoanType_CheckedChanged);
      this.chkLoanPurpose.AutoSize = true;
      this.chkLoanPurpose.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkLoanPurpose.Location = new Point(550, 64);
      this.chkLoanPurpose.Name = "chkLoanPurpose";
      this.chkLoanPurpose.Size = new Size(117, 18);
      this.chkLoanPurpose.TabIndex = 1;
      this.chkLoanPurpose.Text = "&Purpose of Loan";
      this.chkLoanPurpose.UseVisualStyleBackColor = true;
      this.chkLoanPurpose.CheckedChanged += new EventHandler(this.chkLoanPurpose_CheckedChanged);
      this.chkLienType.AutoSize = true;
      this.chkLienType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkLienType.Location = new Point(280, 209);
      this.chkLienType.Name = "chkLienType";
      this.chkLienType.Size = new Size(98, 18);
      this.chkLienType.TabIndex = 7;
      this.chkLienType.Text = "&Lien Position";
      this.chkLienType.UseVisualStyleBackColor = true;
      this.chkLienType.CheckedChanged += new EventHandler(this.chkLienType_CheckedChanged);
      this.chkPropertyState.AutoSize = true;
      this.chkPropertyState.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkPropertyState.Location = new Point(414, 64);
      this.chkPropertyState.Name = "chkPropertyState";
      this.chkPropertyState.Size = new Size(105, 18);
      this.chkPropertyState.TabIndex = 11;
      this.chkPropertyState.Text = "P&roperty State";
      this.chkPropertyState.UseVisualStyleBackColor = true;
      this.chkPropertyState.CheckedChanged += new EventHandler(this.chkPropertyState_CheckedChanged);
      this.lblCriteria.Location = new Point(12, 9);
      this.lblCriteria.Name = "lblCriteria";
      this.lblCriteria.Size = new Size(664, 43);
      this.lblCriteria.TabIndex = 0;
      this.lblCriteria.Text = componentResourceManager.GetString("lblCriteria.Text");
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(596, 472);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(76, 24);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Location = new Point(510, 472);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(76, 24);
      this.btnOK.TabIndex = 19;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.okBtn_Click);
      this.lvwEntityTypes.AutoArrange = false;
      this.lvwEntityTypes.CheckBoxes = true;
      this.lvwEntityTypes.Columns.AddRange(new ColumnHeader[1]
      {
        this.colEntityType
      });
      this.lvwEntityTypes.Enabled = false;
      this.lvwEntityTypes.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem73.StateImageIndex = 0;
      listViewItem73.Tag = (object) "Broker";
      listViewItem74.StateImageIndex = 0;
      listViewItem74.Tag = (object) "Lender";
      this.lvwEntityTypes.Items.AddRange(new ListViewItem[2]
      {
        listViewItem73,
        listViewItem74
      });
      this.lvwEntityTypes.Location = new Point(152, 84);
      this.lvwEntityTypes.MultiSelect = false;
      this.lvwEntityTypes.Name = "lvwEntityTypes";
      this.lvwEntityTypes.Scrollable = false;
      this.lvwEntityTypes.ShowGroups = false;
      this.lvwEntityTypes.Size = new Size(120, 116);
      this.lvwEntityTypes.TabIndex = 16;
      this.lvwEntityTypes.UseCompatibleStateImageBehavior = false;
      this.lvwEntityTypes.View = View.Details;
      this.colEntityType.Width = 120;
      this.lvwChannelTypes.AutoArrange = false;
      this.lvwChannelTypes.CheckBoxes = true;
      this.lvwChannelTypes.Columns.AddRange(new ColumnHeader[1]
      {
        this.colChannel
      });
      this.lvwChannelTypes.Enabled = false;
      this.lvwChannelTypes.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem75.StateImageIndex = 0;
      listViewItem75.Tag = (object) "Banker - Retail";
      listViewItem76.StateImageIndex = 0;
      listViewItem76.Tag = (object) "Banker - Wholesale";
      listViewItem77.StateImageIndex = 0;
      listViewItem77.Tag = (object) "Brokered";
      listViewItem78.StateImageIndex = 0;
      listViewItem78.Tag = (object) "Correspondent";
      this.lvwChannelTypes.Items.AddRange(new ListViewItem[4]
      {
        listViewItem75,
        listViewItem76,
        listViewItem77,
        listViewItem78
      });
      this.lvwChannelTypes.Location = new Point(17, 84);
      this.lvwChannelTypes.MultiSelect = false;
      this.lvwChannelTypes.Name = "lvwChannelTypes";
      this.lvwChannelTypes.Scrollable = false;
      this.lvwChannelTypes.ShowGroups = false;
      this.lvwChannelTypes.Size = new Size(120, 116);
      this.lvwChannelTypes.TabIndex = 14;
      this.lvwChannelTypes.UseCompatibleStateImageBehavior = false;
      this.lvwChannelTypes.View = View.Details;
      this.colChannel.Width = 125;
      this.chkEntityType.AutoSize = true;
      this.chkEntityType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkEntityType.Location = new Point(146, 64);
      this.chkEntityType.Name = "chkEntityType";
      this.chkEntityType.Size = new Size(85, 18);
      this.chkEntityType.TabIndex = 15;
      this.chkEntityType.Text = "&Entity Type";
      this.chkEntityType.UseVisualStyleBackColor = true;
      this.chkEntityType.CheckedChanged += new EventHandler(this.chkEntityType_CheckedChanged);
      this.chkChannelType.AutoSize = true;
      this.chkChannelType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkChannelType.Location = new Point(12, 64);
      this.chkChannelType.Name = "chkChannelType";
      this.chkChannelType.Size = new Size(71, 18);
      this.chkChannelType.TabIndex = 13;
      this.chkChannelType.Text = "&Channel";
      this.chkChannelType.UseVisualStyleBackColor = true;
      this.chkChannelType.CheckedChanged += new EventHandler(this.chkChannelType_CheckedChanged);
      this.lvwLoanLock.AutoArrange = false;
      this.lvwLoanLock.CheckBoxes = true;
      this.lvwLoanLock.Columns.AddRange(new ColumnHeader[1]
      {
        this.colLockState
      });
      this.lvwLoanLock.Enabled = false;
      this.lvwLoanLock.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem79.StateImageIndex = 0;
      listViewItem79.Tag = (object) "Locked";
      listViewItem80.StateImageIndex = 0;
      listViewItem80.Tag = (object) "Not Locked";
      this.lvwLoanLock.Items.AddRange(new ListViewItem[2]
      {
        listViewItem79,
        listViewItem80
      });
      this.lvwLoanLock.Location = new Point(422, 229);
      this.lvwLoanLock.MultiSelect = false;
      this.lvwLoanLock.Name = "lvwLoanLock";
      this.lvwLoanLock.Scrollable = false;
      this.lvwLoanLock.ShowGroups = false;
      this.lvwLoanLock.Size = new Size(120, 116);
      this.lvwLoanLock.TabIndex = 23;
      this.lvwLoanLock.UseCompatibleStateImageBehavior = false;
      this.lvwLoanLock.View = View.Details;
      this.colLockState.Width = 120;
      this.chkLoanLock.AutoSize = true;
      this.chkLoanLock.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkLoanLock.Location = new Point(414, 209);
      this.chkLoanLock.Name = "chkLoanLock";
      this.chkLoanLock.Size = new Size(114, 18);
      this.chkLoanLock.TabIndex = 22;
      this.chkLoanLock.Text = "L&oan Lock State";
      this.chkLoanLock.UseVisualStyleBackColor = true;
      this.chkLoanLock.CheckedChanged += new EventHandler(this.chkLoanLock_CheckedChanged);
      this.chkChangedCircumstance.AutoSize = true;
      this.chkChangedCircumstance.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkChangedCircumstance.Location = new Point(12, 351);
      this.chkChangedCircumstance.Name = "chkChangedCircumstance";
      this.chkChangedCircumstance.Size = new Size(155, 18);
      this.chkChangedCircumstance.TabIndex = 25;
      this.chkChangedCircumstance.Text = "Changed Circumstance";
      this.chkChangedCircumstance.UseVisualStyleBackColor = true;
      this.chkChangedCircumstance.CheckedChanged += new EventHandler(this.chkChangedCircumstance_CheckedChanged);
      this.lvwChangedCircumstance.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwChangedCircumstance.CheckBoxes = true;
      this.lvwChangedCircumstance.Columns.AddRange(new ColumnHeader[1]
      {
        this.colChangedCircum
      });
      this.lvwChangedCircumstance.Enabled = false;
      this.lvwChangedCircumstance.HeaderStyle = ColumnHeaderStyle.None;
      this.lvwChangedCircumstance.LabelWrap = false;
      this.lvwChangedCircumstance.Location = new Point(16, 371);
      this.lvwChangedCircumstance.MultiSelect = false;
      this.lvwChangedCircumstance.Name = "lvwChangedCircumstance";
      this.lvwChangedCircumstance.ShowGroups = false;
      this.lvwChangedCircumstance.ShowItemToolTips = true;
      this.lvwChangedCircumstance.Size = new Size(388, 89);
      this.lvwChangedCircumstance.TabIndex = 26;
      this.lvwChangedCircumstance.UseCompatibleStateImageBehavior = false;
      this.lvwChangedCircumstance.View = View.Details;
      this.colChangedCircum.Width = 500;
      this.lvwPlanCode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lvwPlanCode.AutoArrange = false;
      this.lvwPlanCode.CheckBoxes = true;
      this.lvwPlanCode.Enabled = false;
      this.lvwPlanCode.HeaderStyle = ColumnHeaderStyle.None;
      this.lvwPlanCode.Location = new Point(421, 371);
      this.lvwPlanCode.MultiSelect = false;
      this.lvwPlanCode.Name = "lvwPlanCode";
      this.lvwPlanCode.ShowGroups = false;
      this.lvwPlanCode.Size = new Size(256, 89);
      this.lvwPlanCode.TabIndex = 28;
      this.lvwPlanCode.UseCompatibleStateImageBehavior = false;
      this.lvwPlanCode.View = View.SmallIcon;
      this.chkPlanCode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkPlanCode.AutoSize = true;
      this.chkPlanCode.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkPlanCode.Location = new Point(414, 351);
      this.chkPlanCode.Name = "chkPlanCode";
      this.chkPlanCode.Size = new Size(81, 18);
      this.chkPlanCode.TabIndex = 27;
      this.chkPlanCode.Text = "Plan &Code";
      this.chkPlanCode.UseVisualStyleBackColor = true;
      this.chkPlanCode.CheckedChanged += new EventHandler(this.chkPlanCode_CheckedChanged);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(691, 506);
      this.Controls.Add((Control) this.lvwPlanCode);
      this.Controls.Add((Control) this.chkPlanCode);
      this.Controls.Add((Control) this.lvwChangedCircumstance);
      this.Controls.Add((Control) this.chkChangedCircumstance);
      this.Controls.Add((Control) this.lvwLoanLock);
      this.Controls.Add((Control) this.chkLoanLock);
      this.Controls.Add((Control) this.lvwEntityTypes);
      this.Controls.Add((Control) this.lvwChannelTypes);
      this.Controls.Add((Control) this.chkEntityType);
      this.Controls.Add((Control) this.chkChannelType);
      this.Controls.Add((Control) this.lvwPropertyStates);
      this.Controls.Add((Control) this.lvwAmortTypes);
      this.Controls.Add((Control) this.lvwLienTypes);
      this.Controls.Add((Control) this.lvwLoanTypes);
      this.Controls.Add((Control) this.lvwOccupancyTypes);
      this.Controls.Add((Control) this.lvwLoanPurposes);
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
      this.Name = nameof (PreClosingCriteriaDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Document Inclusion";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
