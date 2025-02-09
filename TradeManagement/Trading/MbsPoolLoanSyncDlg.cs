// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolLoanSyncDlg
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class MbsPoolLoanSyncDlg : Form
  {
    private Dictionary<string, DisplayItem> mapping = new Dictionary<string, DisplayItem>();
    private MbsPoolMortgageType poolMortgageType;
    private IContainer components;
    private GridView gvFieldList;
    private Button btnOK;
    private CheckBox chBCheckAll;
    private Button btnCancel;
    private CheckBox chbApplyAll;

    public MbsPoolLoanSyncDlg(MbsPoolMortgageType type)
    {
      this.InitializeComponent();
      this.poolMortgageType = type;
      bool flag = true;
      this.initMapNEW();
      foreach (string key in this.mapping.Keys)
      {
        GVItem gvItem = new GVItem("");
        gvItem.SubItems[0].Checked = true;
        if (this.mapping[key].IsCommentItem)
        {
          gvItem.SubItems[0].CheckBoxVisible = false;
        }
        else
        {
          gvItem.SubItems[0].Checked = this.mapping[key].IsChecked;
          if (!this.mapping[key].IsChecked)
            flag = false;
        }
        gvItem.SubItems.Add((object) this.mapping[key].TradeFieldDisplayName);
        gvItem.SubItems.Add((object) this.mapping[key].LoanFieldDisplayName);
        gvItem.Tag = (object) key;
        this.gvFieldList.Items.Add(gvItem);
      }
      this.chBCheckAll.CheckedChanged -= new EventHandler(this.chBCheckAll_CheckedChanged);
      this.chBCheckAll.Checked = flag;
      this.chBCheckAll.CheckedChanged += new EventHandler(this.chBCheckAll_CheckedChanged);
      this.chbApplyAll.Checked = true;
    }

    private void initMapNEW()
    {
      this.mapping.Clear();
      DataTable synchronizationFields = Session.ConfigurationManager.GetLoanSynchronizationFields();
      if (synchronizationFields == null)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) synchronizationFields.Rows)
      {
        if (!(row["TradeType"].ToString() != "3") && (!(row["FieldId"].ToString() == "4093") && !(row["FieldId"].ToString() == "4094") || this.poolMortgageType == MbsPoolMortgageType.FannieMaePE))
          this.mapping.Add(row["FieldId"].ToString(), new DisplayItem(row["SourceFieldDesc"].ToString(), row["DestinationFieldDesc"].ToString(), !Utils.ParseBoolean(row["Editable"]), Utils.ParseBoolean(row["Value"])));
      }
    }

    public List<string> SkipFieldList
    {
      get
      {
        List<string> skipFieldList = new List<string>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFieldList.Items)
        {
          if (!this.mapping[(string) gvItem.Tag].IsCommentItem && !gvItem.Checked)
            skipFieldList.Add(string.Concat(gvItem.Tag));
        }
        if (skipFieldList.Contains("Investor Information"))
          skipFieldList.Add("1397");
        return skipFieldList;
      }
    }

    private void chBCheckAll_CheckedChanged(object sender, EventArgs e)
    {
      bool flag = false;
      if (this.chBCheckAll.Checked)
        flag = true;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFieldList.Items)
        gvItem.SubItems[0].Checked = flag;
    }

    public bool ApplyToAll => this.chbApplyAll.Checked;

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
      this.btnOK = new Button();
      this.gvFieldList = new GridView();
      this.chBCheckAll = new CheckBox();
      this.chbApplyAll = new CheckBox();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(459, 343);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(100, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "Start Update";
      this.btnOK.UseVisualStyleBackColor = true;
      this.gvFieldList.AllowMultiselect = false;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "";
      gvColumn1.Width = 30;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Trade Field";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Loan Field";
      gvColumn3.Width = 315;
      this.gvFieldList.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvFieldList.FullRowSelect = false;
      this.gvFieldList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvFieldList.Location = new Point(12, 12);
      this.gvFieldList.Name = "gvFieldList";
      this.gvFieldList.Selectable = false;
      this.gvFieldList.Size = new Size(547, 319);
      this.gvFieldList.SortOption = GVSortOption.None;
      this.gvFieldList.TabIndex = 0;
      this.chBCheckAll.AutoSize = true;
      this.chBCheckAll.Checked = true;
      this.chBCheckAll.CheckState = CheckState.Checked;
      this.chBCheckAll.Location = new Point(19, 16);
      this.chBCheckAll.Name = "chBCheckAll";
      this.chBCheckAll.Size = new Size(15, 14);
      this.chBCheckAll.TabIndex = 3;
      this.chBCheckAll.UseVisualStyleBackColor = true;
      this.chBCheckAll.CheckedChanged += new EventHandler(this.chBCheckAll_CheckedChanged);
      this.chbApplyAll.AutoSize = true;
      this.chbApplyAll.Checked = true;
      this.chbApplyAll.CheckState = CheckState.Checked;
      this.chbApplyAll.Enabled = false;
      this.chbApplyAll.Location = new Point(12, 347);
      this.chbApplyAll.Name = "chbApplyAll";
      this.chbApplyAll.Size = new Size(110, 17);
      this.chbApplyAll.TabIndex = 5;
      this.chbApplyAll.Text = "Apply to All Loans";
      this.chbApplyAll.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(353, 343);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(100, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(571, 378);
      this.ControlBox = false;
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.chbApplyAll);
      this.Controls.Add((Control) this.chBCheckAll);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.gvFieldList);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MbsPoolLoanSyncDlg);
      this.Text = "Loan Data Synchronization";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
