// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FeeListDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FeeListDialog : Form, IHelp
  {
    private Button cancelBtn;
    private Button okBtn;
    private System.ComponentModel.Container components;
    private FeeListBase feeList;
    private string feeType = string.Empty;
    private LoanData loanData;
    private GridView gridFeeList;
    private Sessions.Session session = Session.DefaultInstance;
    private string feeDescription;
    private string feeTotal;

    internal FeeListDialog(string feeType, LoanData loanData, Sessions.Session session)
    {
      this.loanData = loanData;
      this.session = session;
      this.feeType = feeType;
      this.InitializeComponent();
      this.initFeeList();
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
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.gridFeeList = new GridView();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(476, 40);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 3;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(476, 12);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 2;
      this.okBtn.Text = "&Select";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Fee Name";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Based On";
      gvColumn2.Width = 85;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.Numeric;
      gvColumn3.Text = "Rate %";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 60;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "$Additional";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 109;
      this.gridFeeList.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gridFeeList.Location = new Point(8, 12);
      this.gridFeeList.Name = "gridFeeList";
      this.gridFeeList.Size = new Size(456, 324);
      this.gridFeeList.TabIndex = 4;
      this.gridFeeList.DoubleClick += new EventHandler(this.feeListview_DoubleClick);
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(562, 347);
      this.Controls.Add((Control) this.gridFeeList);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FeeListDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Fee List";
      this.KeyUp += new KeyEventHandler(this.FeeListDialog_KeyUp);
      this.ResumeLayout(false);
    }

    internal string FeeDescription => this.feeDescription;

    internal string FeeTotal => this.feeTotal;

    private void initFeeList()
    {
      if (this.feeType == "city")
      {
        this.Text = "City/County Tax/Stamps List";
        this.feeList = (FeeListBase) this.session.GetSystemSettings(typeof (FeeCityList));
      }
      else if (this.feeType == "state")
      {
        this.Text = "State Tax/Stamps List";
        this.feeList = (FeeListBase) this.session.GetSystemSettings(typeof (FeeStateList));
      }
      else
      {
        this.Text = "User Defined List";
        this.feeList = (FeeListBase) this.session.GetSystemSettings(typeof (FeeUserList));
      }
      this.gridFeeList.Items.Clear();
      this.gridFeeList.BeginUpdate();
      for (int i = 0; i < this.feeList.Count; ++i)
      {
        FeeListBase.FeeTable tableAt = this.feeList.GetTableAt(i);
        this.addListView(tableAt.FeeName, tableAt.CalcBasedOn, tableAt.Rate, tableAt.Additional);
      }
      this.gridFeeList.EndUpdate();
      if (this.gridFeeList.Items.Count < 1)
        this.okBtn.Enabled = false;
      if (this.gridFeeList.Items.Count == 0)
        return;
      this.gridFeeList.Items[0].Selected = true;
    }

    private void addListView(string description, string basedOn, string rate, string additional)
    {
      this.gridFeeList.Items.Add(new GVItem(description)
      {
        SubItems = {
          (object) basedOn,
          (object) rate,
          (object) additional
        }
      });
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.gridFeeList.SelectedItems.Count <= 0)
        return;
      GVItem selectedItem = this.gridFeeList.SelectedItems[0];
      this.feeDescription = selectedItem.Text;
      string text = selectedItem.SubItems[1].Text;
      double rate = Utils.ParseDouble((object) selectedItem.SubItems[2].Text);
      double additional = Utils.ParseDouble((object) selectedItem.SubItems[3].Text);
      if (this.loanData != null)
        this.feeTotal = this.calculateFee(text, rate, additional);
      else
        this.feeTotal = additional.ToString("N2");
    }

    private string calculateFee(string basedOn, double rate, double additional)
    {
      return ((!(basedOn == "Loan Amount") ? Utils.ParseDouble((object) this.loanData.GetField("136")) : Utils.ParseDouble((object) this.loanData.GetField("2"))) * (rate / 100.0) + additional).ToString("N2");
    }

    private void feeListview_DoubleClick(object sender, EventArgs e)
    {
      this.okBtn_Click((object) null, (EventArgs) null);
      this.DialogResult = DialogResult.OK;
    }

    private void FeeListDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp() => JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "City Tax");
  }
}
