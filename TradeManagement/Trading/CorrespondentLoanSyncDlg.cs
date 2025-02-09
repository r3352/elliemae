// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentLoanSyncDlg
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CorrespondentLoanSyncDlg : Form
  {
    private Dictionary<string, CorrespondentLoanSyncDlg.DisplayItem> mapping = new Dictionary<string, CorrespondentLoanSyncDlg.DisplayItem>();
    private IContainer components;
    private GridView gvFieldList;
    private Button btnOK;
    private CheckBox chbApplyAll;
    private CheckBox chBCheckAll;

    public CorrespondentLoanSyncDlg()
    {
      this.InitializeComponent();
      this.initMapNEW();
      foreach (string key in this.mapping.Keys)
      {
        GVItem gvItem = new GVItem("");
        gvItem.SubItems[0].Checked = true;
        if (this.mapping[key].IsCommentItem)
          gvItem.SubItems[0].CheckBoxVisible = false;
        gvItem.SubItems.Add((object) this.mapping[key].TradeFieldDisplayName);
        gvItem.SubItems.Add((object) this.mapping[key].LoanFieldDisplayName);
        gvItem.Tag = (object) key;
        this.gvFieldList.Items.Add(gvItem);
      }
    }

    private void initMapNEW()
    {
      string str = "       ";
      this.mapping.Add("2841", new CorrespondentLoanSyncDlg.DisplayItem("Contract Number", "Master Contract # (2841)"));
      this.mapping.Add("2297", new CorrespondentLoanSyncDlg.DisplayItem("Investor Delivery Date", "Sell Side Investor Delivery Date (2297)"));
      this.mapping.Add("2206", new CorrespondentLoanSyncDlg.DisplayItem("Target Delivery Date", "Sell Side Target Delivery Date (2206)"));
      this.mapping.Add("2232", new CorrespondentLoanSyncDlg.DisplayItem("Base Price", "Sell Side Base Price (2232)"));
      this.mapping.Add("Price Adjustments", new CorrespondentLoanSyncDlg.DisplayItem("Price Adjustments", "Sell Side Price Adjustments (2234)"));
      this.mapping.Add("comment2273", new CorrespondentLoanSyncDlg.DisplayItem(str + "Total Price Adjustment", "Sell Side Total Price Adjustment (2273)", true));
      this.mapping.Add("3890", new CorrespondentLoanSyncDlg.DisplayItem("Pool Number", "Sell Side Pool Number (3890)"));
      this.mapping.Add("Investor Information", new CorrespondentLoanSyncDlg.DisplayItem("Investor Information", ""));
      this.mapping.Add("comment2278", new CorrespondentLoanSyncDlg.DisplayItem(str + "Investor Name", "Sell Side Investor Name (2278)", true));
      this.mapping.Add("comment2279", new CorrespondentLoanSyncDlg.DisplayItem(str + "Investor Contact Name", "Sell Side Investor Contact (2279)", true));
      this.mapping.Add("comment2280", new CorrespondentLoanSyncDlg.DisplayItem(str + "Investor Phone Number", "Sell Side Investor Contact Phone (2280)", true));
      this.mapping.Add("comment3055", new CorrespondentLoanSyncDlg.DisplayItem(str + "Investor Email Address", "Sell Side Investor Email Address (3055)", true));
      this.mapping.Add("comment2281", new CorrespondentLoanSyncDlg.DisplayItem(str + "Investor Address (Street Address)", "Sell Side Investor Address (Street Address) (2281)", true));
      this.mapping.Add("comment2282", new CorrespondentLoanSyncDlg.DisplayItem(str + "Investor Address (City)", "Sell Side Investor Address (City) (2282)", true));
      this.mapping.Add("comment2283", new CorrespondentLoanSyncDlg.DisplayItem(str + "Investor Address (State)", "Sell Side Investor Address (State) (2283)", true));
      this.mapping.Add("comment2284", new CorrespondentLoanSyncDlg.DisplayItem(str + "Investor Address (Zip)", "Sell Side Investor Address (Zip) (2284)", true));
      this.mapping.Add("comment2285", new CorrespondentLoanSyncDlg.DisplayItem(str + "Investor Website", "Sell Side Investor Website (2285)", true));
      this.mapping.Add("1397", new CorrespondentLoanSyncDlg.DisplayItem(str + "HMDA Information", "Type of Purchaser (1397)", true));
      this.mapping.Add("comment3534", new CorrespondentLoanSyncDlg.DisplayItem(str + "Servicing Type", "Sell Side Servicing Type (3534)", true));
      this.mapping.Add("comment3535", new CorrespondentLoanSyncDlg.DisplayItem(str + "Servicer", "Sell Side Servicer (3535)", true));
      this.mapping.Add("comment3889", new CorrespondentLoanSyncDlg.DisplayItem(str + "Guarantee Fee", "Sell Side Guarantee Fee (3889) (map to ULDD.X39)", true));
      this.mapping.Add("comment3888", new CorrespondentLoanSyncDlg.DisplayItem(str + "Servicing Fee", "Sell Side Servicing Fee (3888) (map to ULDD.X85)", true));
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
      this.chbApplyAll = new CheckBox();
      this.chBCheckAll = new CheckBox();
      this.SuspendLayout();
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(484, 343);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
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
      this.gvFieldList.Location = new Point(12, 12);
      this.gvFieldList.Name = "gvFieldList";
      this.gvFieldList.Selectable = false;
      this.gvFieldList.Size = new Size(547, 319);
      this.gvFieldList.SortOption = GVSortOption.None;
      this.gvFieldList.TabIndex = 0;
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
      this.chBCheckAll.AutoSize = true;
      this.chBCheckAll.Checked = true;
      this.chBCheckAll.CheckState = CheckState.Checked;
      this.chBCheckAll.Location = new Point(19, 16);
      this.chBCheckAll.Name = "chBCheckAll";
      this.chBCheckAll.Size = new Size(15, 14);
      this.chBCheckAll.TabIndex = 3;
      this.chBCheckAll.UseVisualStyleBackColor = true;
      this.chBCheckAll.CheckedChanged += new EventHandler(this.chBCheckAll_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(571, 378);
      this.ControlBox = false;
      this.Controls.Add((Control) this.chBCheckAll);
      this.Controls.Add((Control) this.chbApplyAll);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.gvFieldList);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "MbsPoolLoanSyncDlg";
      this.Text = "Loan Data Synchronization";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private class DisplayItem
    {
      public string TradeFieldDisplayName;
      public string LoanFieldDisplayName;
      public bool IsCommentItem;

      public DisplayItem(string tradeFieldDisplayName, string loanFieldDisplayName)
        : this(tradeFieldDisplayName, loanFieldDisplayName, false)
      {
      }

      public DisplayItem(
        string tradeFieldDisplayName,
        string loanFieldDisplayName,
        bool isCommentItem)
      {
        this.TradeFieldDisplayName = tradeFieldDisplayName;
        this.LoanFieldDisplayName = loanFieldDisplayName;
        this.IsCommentItem = isCommentItem;
      }
    }
  }
}
