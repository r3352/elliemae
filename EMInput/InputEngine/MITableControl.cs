// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MITableControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.RemotingServices;
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
  public class MITableControl : UserControl
  {
    private const string className = "MITableControl";
    private static string sw = Tracing.SwOutsideLoan;
    private LoanTypeEnum loanType;
    private string tabName = string.Empty;
    private bool isForDownload;
    private bool isSelectOnly;
    private Sessions.Session session;
    private IContainer components;
    private GridView listViewMI;

    public event GVItemEventHandler RecordDoubleClick;

    public MITableControl(LoanTypeEnum loanType, MIRecord[] records, Sessions.Session session)
    {
      this.session = session;
      this.isSelectOnly = true;
      this.loanType = loanType;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      if (this.loanType == LoanTypeEnum.Conventional)
        this.listViewMI.Columns.Insert(1, new GVColumn()
        {
          Text = "Tab Name",
          Width = 80
        });
      this.setListViewHeaders(loanType);
      this.listViewMI.BeginUpdate();
      if (records != null)
      {
        for (int index = 0; index < records.Length; ++index)
          this.listViewMI.Items.Add(this.buildItem(records[index], false));
      }
      this.listViewMI.EndUpdate();
      this.listViewMI.Sort(0, SortOrder.Ascending);
    }

    private void sor(object source, GVColumnEventArgs e)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public MITableControl(
      LoanTypeEnum loanType,
      string tabName,
      bool isForDownload,
      Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.Dock = DockStyle.Fill;
      this.setListViewHeaders(loanType);
      this.ReloadRecords(loanType, tabName, isForDownload);
      this.listViewMI.Sort(0, SortOrder.Ascending);
    }

    private void setListViewHeaders(LoanTypeEnum loanType)
    {
      if (loanType != LoanTypeEnum.VA)
        this.listViewMI.Columns.Remove(this.listViewMI.Columns.GetColumnByName("colSubsequentPremium"));
      if (loanType == LoanTypeEnum.Other)
      {
        this.listViewMI.Columns.Remove(this.listViewMI.Columns.GetColumnByName("col1stPremium"));
      }
      else
      {
        if (loanType != LoanTypeEnum.USDA)
          return;
        this.listViewMI.Columns.GetColumnByName("col1stPremium").Text = "Guarantee Fee";
        GVColumn columnByName1 = this.listViewMI.Columns.GetColumnByName("Column4");
        columnByName1.Text = "1st Annual Fee %";
        columnByName1.Width = 130;
        GVColumn columnByName2 = this.listViewMI.Columns.GetColumnByName("Column5");
        columnByName2.Text = "1st Annual Fee Months";
        columnByName2.Width = 130;
        GVColumn columnByName3 = this.listViewMI.Columns.GetColumnByName("Column6");
        columnByName3.Text = "2nd Annual Fee %";
        columnByName3.Width = 130;
        GVColumn columnByName4 = this.listViewMI.Columns.GetColumnByName("Column7");
        columnByName4.Text = "2nd Annual Fee Months";
        columnByName4.Width = 130;
      }
    }

    public void ReloadRecords(LoanTypeEnum loanType, string tabName, bool isForDownload)
    {
      this.loanType = loanType;
      this.isForDownload = isForDownload;
      this.tabName = tabName;
      this.listViewMI.BeginUpdate();
      this.listViewMI.Items.Clear();
      MIRecord[] miRecords = this.session.ConfigurationManager.GetMIRecords(this.loanType, this.tabName, this.isForDownload);
      if (miRecords != null)
      {
        for (int index = 0; index < miRecords.Length; ++index)
          this.listViewMI.Items.Add(this.buildItem(miRecords[index], false));
      }
      this.listViewMI.EndUpdate();
    }

    public void AddRecord()
    {
      using (MIRecordSetupForm miRecordSetupForm = new MIRecordSetupForm(this.session, (MIRecord) null, this.loanType, this.tabName))
      {
        if (miRecordSetupForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        try
        {
          int id = this.session.ConfigurationManager.AddMIRecord(miRecordSetupForm.NewMIRecord, this.loanType, this.tabName, this.isForDownload);
          this.ReloadRecords(this.loanType, this.tabName, this.isForDownload);
          this.selectRecord(id);
        }
        catch (Exception ex)
        {
          Tracing.Log(MITableControl.sw, TraceLevel.Error, nameof (MITableControl), "exception in adding MI record: " + ex.Message);
        }
      }
    }

    private void selectRecord(int id)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewMI.Items)
      {
        if (((MIRecord) gvItem.Tag).Id == id)
        {
          gvItem.Selected = true;
          break;
        }
      }
    }

    public void DuplicateRecord()
    {
      if (this.listViewMI.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a MI record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        MIRecord miRecord = (MIRecord) ((MIRecord) this.listViewMI.SelectedItems[0].Tag).Clone();
        miRecord.Id = -1;
        using (MIRecordSetupForm miRecordSetupForm = new MIRecordSetupForm(this.session, miRecord, this.loanType, this.tabName))
        {
          if (miRecordSetupForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          try
          {
            int id = this.session.ConfigurationManager.AddMIRecord(miRecordSetupForm.NewMIRecord, this.loanType, this.tabName, this.isForDownload);
            this.ReloadRecords(this.loanType, this.tabName, this.isForDownload);
            this.selectRecord(id);
          }
          catch (Exception ex)
          {
            Tracing.Log(MITableControl.sw, TraceLevel.Error, nameof (MITableControl), "exception in adding MI record: " + ex.Message);
          }
        }
      }
    }

    public void UpdateRecord()
    {
      if (this.listViewMI.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a MI record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (MIRecordSetupForm miRecordSetupForm = new MIRecordSetupForm(this.session, (MIRecord) this.listViewMI.SelectedItems[0].Tag, this.loanType, this.tabName))
        {
          if (miRecordSetupForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          try
          {
            MIRecord miRecord = this.session.ConfigurationManager.UpdateMIRecord(miRecordSetupForm.NewMIRecord, this.loanType, this.tabName, this.isForDownload);
            this.listViewMI.SelectedItems[0].Text = miRecord.ScenarioKeyForUI;
            int num2 = 1;
            if (this.loanType != LoanTypeEnum.Other)
              this.listViewMI.SelectedItems[0].SubItems[num2++].Text = miRecord.Premium1st.ToString("N3");
            if (this.loanType == LoanTypeEnum.VA)
              this.listViewMI.SelectedItems[0].SubItems[num2++].Text = miRecord.SubsequentPremium.ToString("N3");
            GVSubItemCollection subItems1 = this.listViewMI.SelectedItems[0].SubItems;
            int nItemIndex1 = num2;
            int num3 = nItemIndex1 + 1;
            subItems1[nItemIndex1].Text = miRecord.Monthly1st.ToString("N3");
            GVSubItemCollection subItems2 = this.listViewMI.SelectedItems[0].SubItems;
            int nItemIndex2 = num3;
            int num4 = nItemIndex2 + 1;
            subItems2[nItemIndex2].Text = miRecord.Months1st.ToString("0");
            GVSubItemCollection subItems3 = this.listViewMI.SelectedItems[0].SubItems;
            int nItemIndex3 = num4;
            int num5 = nItemIndex3 + 1;
            subItems3[nItemIndex3].Text = miRecord.Monthly2st.ToString("N3");
            GVSubItemCollection subItems4 = this.listViewMI.SelectedItems[0].SubItems;
            int nItemIndex4 = num5;
            int num6 = nItemIndex4 + 1;
            subItems4[nItemIndex4].Text = miRecord.Months2st.ToString("0");
            GVSubItemCollection subItems5 = this.listViewMI.SelectedItems[0].SubItems;
            int nItemIndex5 = num6;
            int num7 = nItemIndex5 + 1;
            subItems5[nItemIndex5].Text = miRecord.Cutoff.ToString("N3");
            this.listViewMI.SelectedItems[0].Tag = (object) miRecord;
          }
          catch (Exception ex)
          {
            Tracing.Log(MITableControl.sw, TraceLevel.Error, nameof (MITableControl), "exception in updating MI record: " + ex.Message);
          }
        }
      }
    }

    public void ViewRecord()
    {
      if (this.listViewMI.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a MI record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (MIRecordSetupForm miRecordSetupForm = new MIRecordSetupForm(this.session, (MIRecord) this.listViewMI.SelectedItems[0].Tag, this.loanType, true))
        {
          int num2 = (int) miRecordSetupForm.ShowDialog((IWin32Window) this);
        }
      }
    }

    public void DeleteRecord()
    {
      if (this.listViewMI.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        MIRecord tag = (MIRecord) this.listViewMI.SelectedItems[0].Tag;
        try
        {
          this.session.ConfigurationManager.DeleteMIRecord(tag.Id, this.loanType, this.tabName, this.isForDownload);
          int nItemIndex = this.listViewMI.SelectedItems[0].Index;
          this.listViewMI.Items.Remove(this.listViewMI.SelectedItems[0]);
          if (this.listViewMI.Items.Count == 0)
            return;
          if (nItemIndex >= this.listViewMI.Items.Count)
            nItemIndex = this.listViewMI.Items.Count - 1;
          this.listViewMI.Items[nItemIndex].Selected = true;
        }
        catch (Exception ex)
        {
          Tracing.Log(MITableControl.sw, TraceLevel.Error, nameof (MITableControl), "exception in deleting MI record: " + ex.Message);
        }
      }
    }

    private static void moveListViewItemUp(GridView list, int selected)
    {
      if (selected == 0)
        return;
      list.BeginUpdate();
      GVItem gvItem = list.Items[selected];
      list.Items.RemoveAt(selected);
      list.Items.Insert(selected - 1, gvItem);
      gvItem.Selected = true;
      list.EndUpdate();
    }

    private static void moveListViewItemDown(GridView list, int selected)
    {
      if (selected + 1 >= list.Items.Count)
        return;
      list.BeginUpdate();
      GVItem gvItem = list.Items[selected];
      list.Items.RemoveAt(selected);
      list.Items.Insert(selected + 1, gvItem);
      gvItem.Selected = true;
      list.EndUpdate();
    }

    private GVItem buildItem(MIRecord miRecord, bool selected)
    {
      GVItem gvItem = new GVItem(miRecord.ScenarioKeyForUI);
      if (this.isSelectOnly && this.loanType == LoanTypeEnum.Conventional)
      {
        if (miRecord.TabName == string.Empty)
          gvItem.SubItems.Add((object) "General");
        else
          gvItem.SubItems.Add((object) miRecord.TabName);
      }
      if (this.loanType != LoanTypeEnum.Other)
        gvItem.SubItems.Add((object) miRecord.Premium1st.ToString("N3"));
      if (this.loanType == LoanTypeEnum.VA)
        gvItem.SubItems.Add((object) miRecord.SubsequentPremium.ToString("N3"));
      gvItem.SubItems.Add((object) miRecord.Monthly1st.ToString("N3"));
      GVSubItemCollection subItems1 = gvItem.SubItems;
      int num = miRecord.Months1st;
      string str1 = num.ToString("0");
      subItems1.Add((object) str1);
      gvItem.SubItems.Add((object) miRecord.Monthly2st.ToString("N3"));
      GVSubItemCollection subItems2 = gvItem.SubItems;
      num = miRecord.Months2st;
      string str2 = num.ToString("0");
      subItems2.Add((object) str2);
      gvItem.SubItems.Add((object) miRecord.Cutoff.ToString("N3"));
      gvItem.Tag = (object) miRecord;
      gvItem.Selected = selected;
      return gvItem;
    }

    public string[] SelectedItems
    {
      get
      {
        List<string> stringList = new List<string>();
        foreach (GVItem selectedItem in this.listViewMI.SelectedItems)
        {
          MIRecord tag = (MIRecord) selectedItem.Tag;
          stringList.Add(tag.UIKey);
        }
        return stringList.ToArray();
      }
      set
      {
        if (value != null && value.Length == 0)
        {
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewMI.Items)
            gvItem.Selected = false;
        }
        else
        {
          List<string> stringList = new List<string>((IEnumerable<string>) value);
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewMI.Items)
          {
            MIRecord tag = (MIRecord) gvItem.Tag;
            gvItem.Selected = stringList.Contains(tag.UIKey);
          }
        }
      }
    }

    private void listViewMI_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (this.RecordDoubleClick != null)
        this.RecordDoubleClick(source, e);
      if (this.isForDownload)
      {
        this.ViewRecord();
      }
      else
      {
        if (this.isSelectOnly)
          return;
        this.UpdateRecord();
      }
    }

    public MIRecord CurrentSelectMI
    {
      get
      {
        return this.listViewMI.SelectedItems.Count == 0 ? (MIRecord) null : (MIRecord) this.listViewMI.SelectedItems[0].Tag;
      }
    }

    public int ListViewItemCount => this.listViewMI.Items.Count;

    public int ListViewSelectedItemCount => this.listViewMI.SelectedItems.Count;

    public event EventHandler ListViewSelectedIndexChanged
    {
      add
      {
        lock (this.listViewMI)
          this.listViewMI.SelectedIndexChanged += value;
      }
      remove
      {
        lock (this.listViewMI)
          this.listViewMI.SelectedIndexChanged -= value;
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
      this.listViewMI = new GridView();
      this.SuspendLayout();
      this.listViewMI.AllowMultiselect = false;
      this.listViewMI.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Scenario";
      gvColumn1.Width = 205;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "col1stPremium";
      gvColumn2.Text = "1st Premium %";
      gvColumn2.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn2.Width = 90;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colSubsequentPremium";
      gvColumn3.Text = "Sub. Premium %";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 93;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "1st Monthly MI %";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 98;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "1st MI Months";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 81;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "2nd Monthly MI %";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 98;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "2nd MI Months";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 133;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.Text = "Cutoff %";
      gvColumn8.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn8.Width = 100;
      this.listViewMI.Columns.AddRange(new GVColumn[8]
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
      this.listViewMI.Dock = DockStyle.Fill;
      this.listViewMI.ForeColor = SystemColors.WindowText;
      this.listViewMI.Location = new Point(0, 0);
      this.listViewMI.Name = "listViewMI";
      this.listViewMI.Size = new Size(674, 389);
      this.listViewMI.TabIndex = 6;
      this.listViewMI.ItemDoubleClick += new GVItemEventHandler(this.listViewMI_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.listViewMI);
      this.Name = nameof (MITableControl);
      this.Size = new Size(674, 389);
      this.ResumeLayout(false);
    }
  }
}
