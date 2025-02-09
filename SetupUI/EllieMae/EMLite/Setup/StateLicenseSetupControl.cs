// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.StateLicenseSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class StateLicenseSetupControl : UserControl
  {
    public EventHandler EditorClosing;
    public EventHandler EditorOpening;
    public EventHandler SubitemChecked;
    private string[] statesAbbrevation;
    private Hashtable stateTable;
    private List<StateLicenseExtType> licenses;
    private DateTime minValue = DateTime.Parse("1/1/1900");
    private DateTime maxValue = DateTime.Parse("01/01/2079");
    private bool useParentInfo;
    private bool readOnly;
    private bool includeUT;
    private IContainer components;
    private GroupContainer grpLicenseList;
    private GridView gridViewStates;
    private FlowLayoutPanel flowLayoutButtons;
    private Button notAllBtn;
    private Button allBtn;

    public StateLicenseSetupControl(
      List<StateLicenseExtType> licenses,
      bool useParentInfo,
      bool readOnly,
      bool includeUT)
    {
      this.licenses = licenses;
      this.useParentInfo = useParentInfo;
      this.readOnly = readOnly;
      this.includeUT = includeUT;
      this.InitializeComponent();
      if (readOnly)
        this.gridViewStates.Columns[0].CheckBoxes = this.allBtn.Visible = this.notAllBtn.Visible = false;
      this.Dock = DockStyle.Fill;
      this.statesAbbrevation = Utils.GetStates(true);
      string[] additionalTerritories = Utils.GetAdditionalTerritories();
      List<string> stringList = new List<string>((IEnumerable<string>) this.statesAbbrevation);
      if (includeUT)
      {
        if (!string.Equals(Session.ConfigurationManager.GetCompanySetting("Policies", "ShowAdditionalTerritories"), "true", StringComparison.CurrentCultureIgnoreCase))
        {
          foreach (string str in additionalTerritories)
            stringList.Remove(str);
        }
      }
      else
      {
        stringList.Remove("PR");
        stringList.Remove("GU");
        stringList.Remove("VI");
        foreach (string str in additionalTerritories)
          stringList.Remove(str);
        stringList.Sort();
      }
      this.statesAbbrevation = stringList.ToArray();
      this.stateTable = new Hashtable();
      this.initForm();
      if (includeUT)
        return;
      this.gridViewStates.Sort(1, SortOrder.Ascending);
    }

    private void initForm()
    {
      if (this.licenses != null)
      {
        for (int index = 0; index < this.licenses.Count; ++index)
        {
          if ((this.includeUT || !(this.licenses[index].StateAbbrevation == "GU") && !(this.licenses[index].StateAbbrevation == "PR") && !(this.licenses[index].StateAbbrevation == "VI")) && !this.stateTable.ContainsKey((object) this.licenses[index].StateAbbrevation))
            this.stateTable.Add((object) this.licenses[index].StateAbbrevation, (object) this.licenses[index]);
        }
      }
      this.refreshLicenseTable();
    }

    private void refreshLicenseTable()
    {
      this.gridViewStates.BeginUpdate();
      this.gridViewStates.Items.Clear();
      for (int index = 0; index < this.statesAbbrevation.Length; ++index)
      {
        if (this.includeUT || string.Compare(this.statesAbbrevation[index], "GU", true) != 0 && string.Compare(this.statesAbbrevation[index], "VI", true) != 0 && string.Compare(this.statesAbbrevation[index], "PR", true) != 0)
        {
          MaventLicenseTypesUtils.GetLicenseTypes(this.statesAbbrevation[index]);
          GVItem gvItem = new GVItem();
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems[0].Value = (object) null;
          gvItem.SubItems[1].Value = (object) Utils.GetFullStateName(this.statesAbbrevation[index]);
          if (this.stateTable.ContainsKey((object) this.statesAbbrevation[index]))
          {
            StateLicenseExtType stateLicenseExtType = (StateLicenseExtType) this.stateTable[(object) this.statesAbbrevation[index]];
            gvItem.SubItems[0].Checked = stateLicenseExtType.Approved;
            gvItem.SubItems[2].Text = stateLicenseExtType.LicenseNo;
            DateTime dateTime;
            if (stateLicenseExtType.IssueDate != DateTime.MinValue)
            {
              GVSubItem subItem = gvItem.SubItems[3];
              dateTime = stateLicenseExtType.IssueDate;
              string str = dateTime.ToString("MM/dd/yyyy");
              subItem.Value = (object) str;
            }
            if (stateLicenseExtType.StartDate != DateTime.MinValue)
            {
              GVSubItem subItem = gvItem.SubItems[4];
              dateTime = stateLicenseExtType.StartDate;
              string str = dateTime.ToString("MM/dd/yyyy");
              subItem.Value = (object) str;
            }
            if (stateLicenseExtType.EndDate != DateTime.MinValue)
            {
              GVSubItem subItem = gvItem.SubItems[5];
              string str;
              if (!(stateLicenseExtType.EndDate != DateTime.MaxValue) || !(stateLicenseExtType.EndDate != DateTime.MinValue))
              {
                str = "";
              }
              else
              {
                dateTime = stateLicenseExtType.EndDate;
                str = dateTime.ToString("MM/dd/yyyy");
              }
              subItem.Value = (object) str;
            }
            gvItem.SubItems[6].Value = (object) stateLicenseExtType.LicenseStatus;
            if (stateLicenseExtType.StatusDate != DateTime.MinValue)
            {
              GVSubItem subItem = gvItem.SubItems[7];
              dateTime = stateLicenseExtType.StatusDate;
              string str = dateTime.ToString("MM/dd/yyyy");
              subItem.Value = (object) str;
            }
            if (stateLicenseExtType.LastChecked != DateTime.MinValue)
            {
              GVSubItem subItem = gvItem.SubItems[8];
              dateTime = stateLicenseExtType.LastChecked;
              string str = dateTime.ToString("MM/dd/yyyy");
              subItem.Value = (object) str;
            }
            if (this.readOnly && stateLicenseExtType.Approved)
              gvItem.SubItems[0].Text = "Yes";
            gvItem.Tag = (object) false;
          }
          this.gridViewStates.Items.Add(gvItem);
        }
      }
      this.gridViewStates.EndUpdate();
      this.refreshButtonStatus();
    }

    public void SetUseParentInfo(bool useParentInfo)
    {
      this.useParentInfo = useParentInfo;
      this.refreshLicenseTable();
    }

    private void gridViewStates_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Index != 0)
        return;
      string str = e.SubItem.Item.Text.Trim();
      string licenseKey = MaventLicenseTypesUtils.GetLicenseKey(e.SubItem.Item.Text.Trim(), e.SubItem.Item.SubItems[2].Text.Trim());
      if (!this.stateTable.ContainsKey((object) str))
        this.stateTable.Add((object) str, (object) new StateLicenseExtType(str, licenseKey, string.Empty, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, string.Empty, DateTime.MinValue, false, false, DateTime.MinValue, 0));
      StateLicenseExtType stateLicenseExtType = (StateLicenseExtType) this.stateTable[(object) str];
      if (e.SubItem.Index == 0)
        stateLicenseExtType.Approved = e.SubItem.Checked;
      e.SubItem.Item.Tag = (object) true;
      this.refreshButtonStatus();
      if (this.SubitemChecked == null)
        return;
      this.SubitemChecked((object) this, new EventArgs());
    }

    private void gridViewStates_EditorClosing(object sender, GVSubItemEditingEventArgs e)
    {
      e.SubItem.Item.Tag = (object) true;
      if (this.EditorClosing == null)
        return;
      this.EditorClosing(sender, new EventArgs());
    }

    private void gridViewStates_EditorOpening(object sender, GVSubItemEditingEventArgs e)
    {
      if (this.EditorOpening != null)
        this.EditorOpening((object) e, new EventArgs());
      if (e.SubItem.Index == 6)
      {
        ComboBox editorControl = (ComboBox) e.EditorControl;
        editorControl.Items.Clear();
        editorControl.DropDownStyle = ComboBoxStyle.DropDownList;
        editorControl.Items.AddRange((object[]) Utils.GetStateLicenseStatus());
        editorControl.Text = e.SubItem.Text;
      }
      else
      {
        if (e.SubItem.Index != 2)
          return;
        ((TextBoxBase) e.EditorControl).MaxLength = 50;
      }
    }

    private void allBtn_Click(object sender, EventArgs e) => this.checkStates(true);

    private void checkStates(bool selected)
    {
      for (int nItemIndex = 0; nItemIndex < this.gridViewStates.Items.Count; ++nItemIndex)
        this.gridViewStates.Items[nItemIndex].SubItems[0].Checked = selected;
    }

    private void notAllBtn_Click(object sender, EventArgs e) => this.checkStates(false);

    public List<StateLicenseExtType> GetLicenses(bool includeAll)
    {
      List<StateLicenseExtType> licenses = new List<StateLicenseExtType>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewStates.Items)
      {
        if (gvItem.Tag != null && (includeAll || gvItem.Tag.ToString().ToLower().Equals("true")))
        {
          StateLicenseExtType stateLicenseExtType = new StateLicenseExtType(Utils.GetStateAbbr(gvItem.SubItems[1].Text), "", gvItem.SubItems[2].Text, gvItem.SubItems[3].Text != "" ? Utils.ParseDate((object) gvItem.SubItems[3].Text) : DateTime.MinValue, gvItem.SubItems[4].Text != "" ? Utils.ParseDate((object) gvItem.SubItems[4].Text) : DateTime.MinValue, gvItem.SubItems[5].Text != "" ? Utils.ParseDate((object) gvItem.SubItems[5].Text) : DateTime.MinValue, gvItem.SubItems[6].Text, gvItem.SubItems[7].Text != "" ? Utils.ParseDate((object) gvItem.SubItems[7].Text) : DateTime.MinValue, gvItem.SubItems[0].Checked, false, gvItem.SubItems[8].Text != "" ? Utils.ParseDate((object) gvItem.SubItems[8].Text) : DateTime.MinValue, 0);
          licenses.Add(stateLicenseExtType);
        }
      }
      return licenses;
    }

    public bool DataValidated()
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewStates.Items)
      {
        if (gvItem.Tag != null && gvItem.Tag.ToString().ToLower().Equals("true"))
        {
          if (gvItem.SubItems[3].Text.Trim() != "")
          {
            DateTime date = Utils.ParseDate((object) gvItem.SubItems[3].Text.Trim());
            if (date > this.maxValue || date < this.minValue)
            {
              int num = (int) MessageBox.Show((IWin32Window) this, "The value of Issue Date '" + date.ToString("MM/dd/yyyy") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.gridViewStates.EnsureVisible(gvItem.Index);
              gvItem.SubItems[3].BeginEdit();
              return false;
            }
          }
          if (gvItem.SubItems[4].Text.Trim() != "")
          {
            DateTime date = Utils.ParseDate((object) gvItem.SubItems[4].Text.Trim());
            if (date > this.maxValue || date < this.minValue)
            {
              int num = (int) MessageBox.Show((IWin32Window) this, "The value of Start Date '" + date.ToString("MM/dd/yyyy") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.gridViewStates.EnsureVisible(gvItem.Index);
              gvItem.SubItems[4].BeginEdit();
              return false;
            }
          }
          if (gvItem.SubItems[5].Text.Trim() != "")
          {
            DateTime date = Utils.ParseDate((object) gvItem.SubItems[5].Text.Trim());
            if (date > this.maxValue || date < this.minValue)
            {
              int num = (int) MessageBox.Show((IWin32Window) this, "The value of End Date '" + date.ToString("MM/dd/yyyy") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.gridViewStates.EnsureVisible(gvItem.Index);
              gvItem.SubItems[5].BeginEdit();
              return false;
            }
          }
          if (gvItem.SubItems[7].Text.Trim() != "")
          {
            DateTime date = Utils.ParseDate((object) gvItem.SubItems[7].Text.Trim());
            if (date > this.maxValue || date < this.minValue)
            {
              int num = (int) MessageBox.Show((IWin32Window) this, "The value of Status Date '" + date.ToString("MM/dd/yyyy") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.gridViewStates.EnsureVisible(gvItem.Index);
              gvItem.SubItems[7].BeginEdit();
              return false;
            }
          }
          if (gvItem.SubItems[8].Text.Trim() != "")
          {
            DateTime date = Utils.ParseDate((object) gvItem.SubItems[8].Text.Trim());
            if (date > this.maxValue || date < this.minValue)
            {
              int num = (int) MessageBox.Show((IWin32Window) this, "The value of Last Checked Date '" + date.ToString("MM/dd/yyyy") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.gridViewStates.EnsureVisible(gvItem.Index);
              gvItem.SubItems[8].BeginEdit();
              return false;
            }
          }
        }
      }
      return true;
    }

    private void refreshButtonStatus()
    {
      if (!this.flowLayoutButtons.Visible || this.readOnly)
        return;
      this.notAllBtn.Enabled = this.gridViewStates.GetCheckedItems(0).Length != 0;
      this.allBtn.Enabled = this.gridViewStates.GetCheckedItems(0).Length != this.gridViewStates.Items.Count;
    }

    public bool HasActiveItem => this.gridViewStates.GetCheckedItems(0).Length != 0;

    public bool StopEditing() => this.gridViewStates.StopEditing();

    public bool BorderVisible
    {
      set
      {
        this.grpLicenseList.Borders = value ? AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right : AnchorStyles.None;
      }
    }

    public bool ButtonVisible
    {
      set => this.flowLayoutButtons.Visible = value;
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
      GVColumn gvColumn9 = new GVColumn();
      this.grpLicenseList = new GroupContainer();
      this.flowLayoutButtons = new FlowLayoutPanel();
      this.notAllBtn = new Button();
      this.allBtn = new Button();
      this.gridViewStates = new GridView();
      this.grpLicenseList.SuspendLayout();
      this.flowLayoutButtons.SuspendLayout();
      this.SuspendLayout();
      this.grpLicenseList.Controls.Add((Control) this.flowLayoutButtons);
      this.grpLicenseList.Controls.Add((Control) this.gridViewStates);
      this.grpLicenseList.Dock = DockStyle.Fill;
      this.grpLicenseList.HeaderForeColor = SystemColors.ControlText;
      this.grpLicenseList.Location = new Point(0, 0);
      this.grpLicenseList.Name = "grpLicenseList";
      this.grpLicenseList.Size = new Size(951, 468);
      this.grpLicenseList.TabIndex = 21;
      this.grpLicenseList.Text = "Active Licenses";
      this.flowLayoutButtons.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutButtons.BackColor = Color.Transparent;
      this.flowLayoutButtons.Controls.Add((Control) this.notAllBtn);
      this.flowLayoutButtons.Controls.Add((Control) this.allBtn);
      this.flowLayoutButtons.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutButtons.Location = new Point(751, 1);
      this.flowLayoutButtons.Name = "flowLayoutButtons";
      this.flowLayoutButtons.Size = new Size(196, 22);
      this.flowLayoutButtons.TabIndex = 41;
      this.notAllBtn.Location = new Point(112, 0);
      this.notAllBtn.Margin = new Padding(0);
      this.notAllBtn.Name = "notAllBtn";
      this.notAllBtn.Size = new Size(84, 22);
      this.notAllBtn.TabIndex = 2;
      this.notAllBtn.Text = "&Deselect All";
      this.notAllBtn.Click += new EventHandler(this.notAllBtn_Click);
      this.allBtn.Location = new Point(40, 0);
      this.allBtn.Margin = new Padding(0);
      this.allBtn.Name = "allBtn";
      this.allBtn.Size = new Size(72, 22);
      this.allBtn.TabIndex = 3;
      this.allBtn.Text = "&Select All";
      this.allBtn.Click += new EventHandler(this.allBtn_Click);
      this.gridViewStates.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnApproved";
      gvColumn1.Text = "Active";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 60;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnState";
      gvColumn2.Text = "State";
      gvColumn2.Width = 120;
      gvColumn3.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnLicenseNo";
      gvColumn3.Text = "License #";
      gvColumn3.Width = 150;
      gvColumn4.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnIssueDate";
      gvColumn4.SortMethod = GVSortMethod.Date;
      gvColumn4.Text = "Issue Date";
      gvColumn4.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn4.Width = 80;
      gvColumn5.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnStartDate";
      gvColumn5.SortMethod = GVSortMethod.Date;
      gvColumn5.Text = "Start Date";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 80;
      gvColumn6.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnEndDate";
      gvColumn6.SortMethod = GVSortMethod.Date;
      gvColumn6.Text = "End Date";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 80;
      gvColumn7.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnStatus";
      gvColumn7.Text = "Status";
      gvColumn7.Width = 250;
      gvColumn8.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColumnStatusDate";
      gvColumn8.SortMethod = GVSortMethod.Date;
      gvColumn8.Text = "Status Date";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 80;
      gvColumn9.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "ColumnLastCheck";
      gvColumn9.SortMethod = GVSortMethod.Date;
      gvColumn9.Text = "Last Checked";
      gvColumn9.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn9.Width = 80;
      this.gridViewStates.Columns.AddRange(new GVColumn[9]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gridViewStates.Dock = DockStyle.Fill;
      this.gridViewStates.Location = new Point(1, 26);
      this.gridViewStates.Name = "gridViewStates";
      this.gridViewStates.Size = new Size(949, 441);
      this.gridViewStates.TabIndex = 40;
      this.gridViewStates.SubItemCheck += new GVSubItemEventHandler(this.gridViewStates_SubItemCheck);
      this.gridViewStates.EditorOpening += new GVSubItemEditingEventHandler(this.gridViewStates_EditorOpening);
      this.gridViewStates.EditorClosing += new GVSubItemEditingEventHandler(this.gridViewStates_EditorClosing);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpLicenseList);
      this.Name = nameof (StateLicenseSetupControl);
      this.Size = new Size(951, 468);
      this.grpLicenseList.ResumeLayout(false);
      this.flowLayoutButtons.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
