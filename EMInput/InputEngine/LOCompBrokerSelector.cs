// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LOCompBrokerSelector
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LOCompBrokerSelector : Form
  {
    private GridViewFilterManager filterManager;
    private List<ExternalOriginatorManagementData> companyList;
    private const int RECORDSPERPAGE = 100;
    private bool forLender;
    private int sortedColumnNo = -1;
    private bool sortedDescending;
    private static readonly TableLayout CompanyLayout = new TableLayout(new TableLayout.Column[4]
    {
      new TableLayout.Column("CompanyLegalName", "Company Legal Name", HorizontalAlignment.Left, 200),
      new TableLayout.Column("DBA", "DBA", HorizontalAlignment.Left, 200),
      new TableLayout.Column("City", "City", HorizontalAlignment.Left, 200),
      new TableLayout.Column("State", "State", HorizontalAlignment.Left, 70)
    });
    private ExternalOriginatorManagementData selectedCompany;
    private IContainer components;
    private GroupContainer groupContainer1;
    private BorderPanel borderPanel1;
    private GridView gvCompanyList;
    private Panel panel1;
    private FlowLayoutPanel flowLayoutPanel2;
    private Button btnCancel;
    private Button btnSelect;
    private PageListNavigator navCompanys;
    private Label lblFilter;
    private Button btnClear;
    private Label label4;
    private GradientPanel gradientPanel3;
    private ToolTip toolTip1;
    private EMHelpLink emHelpLink1;

    public LOCompBrokerSelector(bool forLender)
    {
      this.forLender = forLender;
      this.InitializeComponent();
      this.sortedColumnNo = 0;
      this.sortedDescending = false;
      this.filterManager = new GridViewFilterManager(Session.DefaultInstance, this.gvCompanyList, true);
      int num = 0;
      foreach (TableLayout.Column column in LOCompBrokerSelector.CompanyLayout)
      {
        this.filterManager.CreateColumnFilter(num, FieldFormat.STRING);
        this.gvCompanyList.Columns[num].Tag = (object) column.ColumnID;
        ++num;
      }
      this.navCompanys.ItemsPerPage = 100;
      this.searchExternalCompanies(true);
      this.refreshFilterDescription();
      this.filterManager.FilterChanged += new EventHandler(this.onFilterChanged);
      this.gvCompanyList.DoubleClick += new EventHandler(this.btnSelect_Click);
      FieldFilterList fieldFilterList = this.filterManager.ToFieldFilterList();
      this.btnClear.Enabled = fieldFilterList != null && fieldFilterList.Count > 0;
      this.gvCompanyList.Sort(0, SortOrder.Ascending);
    }

    private void searchExternalCompanies(bool refreshRequired)
    {
      FieldFilterList fieldFilterList = this.filterManager.ToFieldFilterList();
      string legalName = string.Empty;
      string dbaName = string.Empty;
      string cityName = string.Empty;
      string stateName = string.Empty;
      if (fieldFilterList != null && fieldFilterList.Count > 0)
      {
        for (int index = 0; index < fieldFilterList.Count; ++index)
        {
          switch (fieldFilterList[index].FieldID)
          {
            case "CompanyLegalName":
              legalName = fieldFilterList[index].ValueFrom;
              break;
            case "DBA":
              dbaName = fieldFilterList[index].ValueFrom;
              break;
            case "City":
              cityName = fieldFilterList[index].ValueFrom;
              break;
            case "State":
              stateName = fieldFilterList[index].ValueFrom;
              break;
          }
        }
      }
      Cursor.Current = Cursors.WaitCursor;
      if (this.companyList != null)
        this.companyList.Clear();
      this.companyList = Session.ConfigurationManager.GetExternalOrganizations(this.forLender, ExternalOriginatorEntityType.Broker, legalName, dbaName, cityName, stateName, false, this.getSortedColumnName(), this.sortedDescending, Session.DefaultInstance.UserID);
      this.navCompanys.NumberOfItems = this.companyList != null ? this.companyList.Count : 0;
      if (refreshRequired)
        this.refreshGridView(1);
      Cursor.Current = Cursors.Default;
    }

    private void refreshGridView(int startNo)
    {
      this.gvCompanyList.Items.Clear();
      if (this.companyList == null || this.companyList.Count == 0)
        return;
      this.navCompanys.MoveFirstEvent -= new PageListNavigator.MoveFirstEventHandler(this.navCompanys_MoveFirstEvent);
      this.navCompanys.MovePreviousEvent -= new PageListNavigator.MovePreviousEventHandler(this.navCompanys_MovePreviousEvent);
      this.navCompanys.MoveNextEvent -= new PageListNavigator.MoveNextEventHandler(this.navCompanys_MoveNextEvent);
      this.navCompanys.MoveLastEvent -= new PageListNavigator.MoveLastEventHandler(this.navCompanys_MoveLastEvent);
      this.navCompanys.PageChangedEvent -= new PageListNavigator.PageChangedEventHandler(this.navCompanys_PageChangedEvent);
      --startNo;
      this.gvCompanyList.BeginUpdate();
      for (int index = 0; index < this.companyList.Count; ++index)
      {
        if (index >= startNo)
          this.gvCompanyList.Items.Add(this.createGVItem(this.companyList[index]));
        if (this.gvCompanyList.Items.Count >= 100)
          break;
      }
      this.gvCompanyList.EndUpdate();
      this.navCompanys.MoveFirstEvent += new PageListNavigator.MoveFirstEventHandler(this.navCompanys_MoveFirstEvent);
      this.navCompanys.MovePreviousEvent += new PageListNavigator.MovePreviousEventHandler(this.navCompanys_MovePreviousEvent);
      this.navCompanys.MoveNextEvent += new PageListNavigator.MoveNextEventHandler(this.navCompanys_MoveNextEvent);
      this.navCompanys.MoveLastEvent += new PageListNavigator.MoveLastEventHandler(this.navCompanys_MoveLastEvent);
      this.navCompanys.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navCompanys_PageChangedEvent);
      this.gvCompanyList_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.filterManager.ClearColumnFilters();
      this.onFilterChanged((object) this, EventArgs.Empty);
    }

    private GVItem createGVItem(ExternalOriginatorManagementData org)
    {
      return new GVItem()
      {
        Text = org.CompanyLegalName,
        SubItems = {
          (object) org.CompanyDBAName,
          (object) org.City,
          (object) org.State
        },
        Tag = (object) org
      };
    }

    private void onFilterChanged(object sender, EventArgs e)
    {
      this.refreshFilterDescription();
      this.searchExternalCompanies(true);
      this.btnClear.Enabled = this.filterManager.ToFieldFilterList().Count > 0;
    }

    private void refreshFilterDescription()
    {
      FieldFilterList fieldFilterList = this.filterManager.ToFieldFilterList();
      if (fieldFilterList.Count == 0)
        this.lblFilter.Text = "None";
      else
        this.lblFilter.Text = fieldFilterList.ToString(true);
    }

    private void navCompanys_MoveFirstEvent(object sender, EventArgs e) => this.refreshGridView(1);

    private void navCompanys_MoveLastEvent(object sender, EventArgs e)
    {
      this.navCompanys_PageChangedEvent((object) null, (PageChangedEventArgs) null);
    }

    private void navCompanys_MoveNextEvent(object sender, EventArgs e)
    {
      this.navCompanys_PageChangedEvent((object) null, (PageChangedEventArgs) null);
    }

    private void navCompanys_MovePreviousEvent(object sender, EventArgs e)
    {
      this.navCompanys_PageChangedEvent((object) null, (PageChangedEventArgs) null);
    }

    private void navCompanys_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      this.refreshGridView(this.navCompanys.CurrentPageIndex * 100 + 1);
    }

    private void gvCompanyList_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.gvCompanyList.SelectedItems.Count == 1;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.gvCompanyList.SelectedItems.Count == 0)
        return;
      this.selectedCompany = (ExternalOriginatorManagementData) this.gvCompanyList.SelectedItems[0].Tag;
      this.DialogResult = DialogResult.OK;
    }

    public ExternalOriginatorManagementData SelectedCompany => this.selectedCompany;

    private void gvCompanyList_ColumnClick(object source, GVColumnClickEventArgs e)
    {
      this.sortedDescending = this.sortedColumnNo == e.Column && !this.sortedDescending;
      this.sortedColumnNo = e.Column;
      this.gvCompanyList.SelectedItems.Clear();
      this.searchExternalCompanies(false);
      this.refreshGridView(this.navCompanys.CurrentPageIndex * 100 + 1);
    }

    private string getSortedColumnName()
    {
      switch (this.sortedColumnNo)
      {
        case 0:
          return "CompanyLegalName";
        case 1:
          return "CompanyDBAName";
        case 2:
          return "City";
        case 3:
          return "State";
        default:
          return "";
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
      GVColumn gvColumn4 = new GVColumn();
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1 = new GroupContainer();
      this.borderPanel1 = new BorderPanel();
      this.gvCompanyList = new GridView();
      this.panel1 = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnCancel = new Button();
      this.btnSelect = new Button();
      this.navCompanys = new PageListNavigator();
      this.gradientPanel3 = new GradientPanel();
      this.lblFilter = new Label();
      this.btnClear = new Button();
      this.label4 = new Label();
      this.groupContainer1.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.borderPanel1);
      this.groupContainer1.Controls.Add((Control) this.panel1);
      this.groupContainer1.Controls.Add((Control) this.navCompanys);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 30);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(664, 573);
      this.groupContainer1.TabIndex = 10;
      this.borderPanel1.Borders = AnchorStyles.Bottom;
      this.borderPanel1.Controls.Add((Control) this.gvCompanyList);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(1, 26);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(662, 509);
      this.borderPanel1.TabIndex = 36;
      this.gvCompanyList.AllowColumnReorder = true;
      this.gvCompanyList.AllowMultiselect = false;
      this.gvCompanyList.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Company Legal Name";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "DBA";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "City";
      gvColumn3.Width = 200;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "State";
      gvColumn4.Width = 62;
      this.gvCompanyList.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gvCompanyList.Dock = DockStyle.Fill;
      this.gvCompanyList.FilterVisible = true;
      this.gvCompanyList.Location = new Point(0, 0);
      this.gvCompanyList.Name = "gvCompanyList";
      this.gvCompanyList.Size = new Size(662, 508);
      this.gvCompanyList.SortOption = GVSortOption.Owner;
      this.gvCompanyList.TabIndex = 28;
      this.gvCompanyList.SelectedIndexChanged += new EventHandler(this.gvCompanyList_SelectedIndexChanged);
      this.gvCompanyList.ColumnClick += new GVColumnClickEventHandler(this.gvCompanyList_ColumnClick);
      this.panel1.Controls.Add((Control) this.emHelpLink1);
      this.panel1.Controls.Add((Control) this.flowLayoutPanel2);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(1, 535);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(662, 37);
      this.panel1.TabIndex = 33;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "LO Compensation#Lookup-wholesale";
      this.emHelpLink1.Location = new Point(11, 10);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 71;
      this.flowLayoutPanel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnCancel);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnSelect);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(339, 6);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Padding = new Padding(0, 0, 10, 0);
      this.flowLayoutPanel2.Size = new Size(324, 24);
      this.flowLayoutPanel2.TabIndex = 7;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(239, 0);
      this.btnCancel.Margin = new Padding(0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSelect.BackColor = SystemColors.Control;
      this.btnSelect.Location = new Point(164, 0);
      this.btnSelect.Margin = new Padding(0);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 24);
      this.btnSelect.TabIndex = 3;
      this.btnSelect.Text = "Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.navCompanys.BackColor = Color.Transparent;
      this.navCompanys.Font = new Font("Arial", 8f);
      this.navCompanys.ItemsPerPage = 100;
      this.navCompanys.Location = new Point(0, 2);
      this.navCompanys.Name = "navCompanys";
      this.navCompanys.NumberOfItems = 0;
      this.navCompanys.Size = new Size(254, 24);
      this.navCompanys.TabIndex = 26;
      this.gradientPanel3.Borders = AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel3.Controls.Add((Control) this.lblFilter);
      this.gradientPanel3.Controls.Add((Control) this.btnClear);
      this.gradientPanel3.Controls.Add((Control) this.label4);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(0, 0);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(664, 30);
      this.gradientPanel3.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel3.TabIndex = 9;
      this.lblFilter.AutoEllipsis = true;
      this.lblFilter.BackColor = Color.Transparent;
      this.lblFilter.Location = new Point(34, 8);
      this.lblFilter.Name = "lblFilter";
      this.lblFilter.Size = new Size(560, 14);
      this.lblFilter.TabIndex = 7;
      this.lblFilter.Text = "label1";
      this.btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClear.BackColor = SystemColors.Control;
      this.btnClear.Location = new Point(600, 4);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(56, 22);
      this.btnClear.TabIndex = 6;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(5, 8);
      this.label4.Name = "label4";
      this.label4.Size = new Size(32, 13);
      this.label4.TabIndex = 3;
      this.label4.Text = "Filter:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(664, 603);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.gradientPanel3);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (LOCompBrokerSelector);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Broker";
      this.groupContainer1.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
