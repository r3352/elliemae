// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.NewMortgageDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class NewMortgageDialog : Form
  {
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private Button cancelBtn;
    private Button okBtn;
    private System.ComponentModel.Container components;
    private ListView liabListView;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private Label label1;
    private LoanData loan;
    private string reoID = string.Empty;
    private ListViewSortManager sortMngr;
    private string selectedVOL;

    public NewMortgageDialog(LoanData loan, string reoID)
    {
      this.loan = loan;
      this.reoID = reoID;
      this.InitializeComponent();
      this.sortMngr = new ListViewSortManager(this.liabListView, new System.Type[5]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewCurrencySort),
        typeof (ListViewCurrencySort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort)
      });
      this.sortMngr.Sort(0);
      this.initialListView();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string SelectedVOL => this.selectedVOL;

    private void InitializeComponent()
    {
      this.liabListView = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.columnHeader5 = new ColumnHeader();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.label1 = new Label();
      this.SuspendLayout();
      this.liabListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.liabListView.CheckBoxes = true;
      this.liabListView.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4,
        this.columnHeader5
      });
      this.liabListView.FullRowSelect = true;
      this.liabListView.HideSelection = false;
      this.liabListView.Location = new Point(8, 8);
      this.liabListView.MultiSelect = false;
      this.liabListView.Name = "liabListView";
      this.liabListView.Size = new Size(456, 300);
      this.liabListView.TabIndex = 2;
      this.liabListView.View = View.Details;
      this.columnHeader1.Text = "Lien Holder";
      this.columnHeader1.Width = 200;
      this.columnHeader2.Text = "Balance";
      this.columnHeader2.TextAlign = HorizontalAlignment.Right;
      this.columnHeader2.Width = 85;
      this.columnHeader3.Text = "Payment";
      this.columnHeader3.TextAlign = HorizontalAlignment.Right;
      this.columnHeader3.Width = 85;
      this.columnHeader4.Text = "ID";
      this.columnHeader4.Width = 0;
      this.columnHeader5.Text = "Type";
      this.columnHeader5.Width = 250;
      this.cancelBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(476, 42);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 6;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(476, 10);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 5;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.Location = new Point(4, 316);
      this.label1.Name = "label1";
      this.label1.Size = new Size(464, 28);
      this.label1.TabIndex = 7;
      this.label1.Text = "If you do not see the liability for this mortgage, add the liability to the VOL worksheet first. Or, if this property is owned free and clear, click OK without selecting a liability.";
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(562, 351);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.liabListView);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (NewMortgageDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Import Mortgage From Liability";
      this.ResumeLayout(false);
    }

    private void initialListView()
    {
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      string empty = string.Empty;
      for (int rec = 1; rec <= exlcudingAlimonyJobExp; ++rec)
      {
        string str = "FL" + rec.ToString("00");
        string field1 = this.loan.GetField(str + "25");
        string field2 = this.loan.GetField(str + "08");
        if ((field2 == "MortgageLoan" || field2 == "HELOC") && (field1 == string.Empty || this.reoID == field1 && this.reoID != string.Empty))
          this.PopulateListView(rec, field1);
      }
      for (int rec = 1; rec <= exlcudingAlimonyJobExp; ++rec)
      {
        string str = "FL" + rec.ToString("00");
        string field3 = this.loan.GetField(str + "25");
        string field4 = this.loan.GetField(str + "08");
        if (field4 != "MortgageLoan" && field4 != "HELOC" && (field3 == string.Empty || this.reoID == field3 && this.reoID != string.Empty))
          this.PopulateListView(rec, field3);
      }
    }

    private void PopulateListView(int rec, string reo)
    {
      string str = "FL" + rec.ToString("00");
      ListViewItem listViewItem = new ListViewItem(this.loan.GetField(str + "02"));
      string field1 = this.loan.GetField(str + "13");
      listViewItem.SubItems.Add(field1);
      string field2 = this.loan.GetField(str + "11");
      listViewItem.SubItems.Add(field2);
      listViewItem.SubItems.Add(rec.ToString());
      string text = this.loan.GetField(str + "08");
      if (text == "MortgageLoan")
        text = "Mortgage";
      listViewItem.SubItems.Add(text);
      listViewItem.Checked = reo == this.reoID && this.reoID != string.Empty;
      this.liabListView.Items.Add(listViewItem);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      int count = this.liabListView.CheckedItems.Count;
      this.selectedVOL = string.Empty;
      string empty = string.Empty;
      for (int index = 0; index < count; ++index)
      {
        string text = this.liabListView.CheckedItems[index].SubItems[3].Text;
        this.selectedVOL = !(this.selectedVOL == string.Empty) ? this.selectedVOL + "|" + text : text;
      }
    }

    private double DoubleValue(string strValue)
    {
      return strValue == string.Empty || strValue == null ? 0.0 : double.Parse(strValue.Replace(",", string.Empty));
    }
  }
}
