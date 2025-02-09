// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ContactAdvancedSearchControl
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Reporting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ContactAdvancedSearchControl : AdvancedSearchControl
  {
    private RadioButton radLastClosed;
    private RadioButton radLastOriginated;
    private RadioButton radAnyLoan;
    private Label label1;
    private RadioButton radAnyClosed;
    private Panel pnlRelatedLoans;

    public ContactAdvancedSearchControl()
    {
      this.InitializeComponent();
      this.pnlRelatedLoans.Parent = (Control) null;
      this.FooterControls.Add((Control) this.pnlRelatedLoans);
      this.pnlRelatedLoans.Dock = DockStyle.Fill;
    }

    private void InitializeComponent()
    {
      this.pnlRelatedLoans = new Panel();
      this.radLastClosed = new RadioButton();
      this.radLastOriginated = new RadioButton();
      this.radAnyClosed = new RadioButton();
      this.radAnyLoan = new RadioButton();
      this.label1 = new Label();
      this.pnlRelatedLoans.SuspendLayout();
      this.SuspendLayout();
      this.pnlRelatedLoans.BackColor = Color.Transparent;
      this.pnlRelatedLoans.Controls.Add((Control) this.radLastClosed);
      this.pnlRelatedLoans.Controls.Add((Control) this.radLastOriginated);
      this.pnlRelatedLoans.Controls.Add((Control) this.radAnyClosed);
      this.pnlRelatedLoans.Controls.Add((Control) this.radAnyLoan);
      this.pnlRelatedLoans.Controls.Add((Control) this.label1);
      this.pnlRelatedLoans.Location = new Point(0, 314);
      this.pnlRelatedLoans.Name = "pnlRelatedLoans";
      this.pnlRelatedLoans.Size = new Size(636, 26);
      this.pnlRelatedLoans.TabIndex = 10;
      this.radLastClosed.AutoSize = true;
      this.radLastClosed.Location = new Point(424, 4);
      this.radLastClosed.Name = "radLastClosed";
      this.radLastClosed.Size = new Size(126, 18);
      this.radLastClosed.TabIndex = 3;
      this.radLastClosed.TabStop = true;
      this.radLastClosed.Text = "Last Completed Loan";
      this.radLastClosed.UseVisualStyleBackColor = true;
      this.radLastClosed.Click += new EventHandler(this.onLoanMatchTypeChange);
      this.radLastOriginated.AutoSize = true;
      this.radLastOriginated.Location = new Point(296, 4);
      this.radLastOriginated.Name = "radLastOriginated";
      this.radLastOriginated.Size = new Size(125, 18);
      this.radLastOriginated.TabIndex = 2;
      this.radLastOriginated.TabStop = true;
      this.radLastOriginated.Text = "Last Originated Loan";
      this.radLastOriginated.UseVisualStyleBackColor = true;
      this.radLastOriginated.Click += new EventHandler(this.onLoanMatchTypeChange);
      this.radAnyClosed.AutoSize = true;
      this.radAnyClosed.Location = new Point(168, 4);
      this.radAnyClosed.Name = "radAnyClosed";
      this.radAnyClosed.Size = new Size(125, 18);
      this.radAnyClosed.TabIndex = 4;
      this.radAnyClosed.TabStop = true;
      this.radAnyClosed.Text = "Any Completed Loan";
      this.radAnyClosed.UseVisualStyleBackColor = true;
      this.radAnyClosed.Click += new EventHandler(this.onLoanMatchTypeChange);
      this.radAnyLoan.AutoSize = true;
      this.radAnyLoan.Location = new Point(93, 4);
      this.radAnyLoan.Name = "radAnyLoan";
      this.radAnyLoan.Size = new Size(72, 18);
      this.radAnyLoan.TabIndex = 1;
      this.radAnyLoan.TabStop = true;
      this.radAnyLoan.Text = "Any Loan";
      this.radAnyLoan.UseVisualStyleBackColor = true;
      this.radAnyLoan.Click += new EventHandler(this.onLoanMatchTypeChange);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(81, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Loan Selection:";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.Controls.Add((Control) this.pnlRelatedLoans);
      this.Name = nameof (ContactAdvancedSearchControl);
      this.Controls.SetChildIndex((Control) this.pnlRelatedLoans, 0);
      this.pnlRelatedLoans.ResumeLayout(false);
      this.pnlRelatedLoans.PerformLayout();
      this.ResumeLayout(false);
    }

    protected override void OnFilterAdded(FieldFilterEventArgs e)
    {
      if (e.Filter.DataSource != FilterDataSource.Loan || this.FooterVisible)
        return;
      using (RelatedLoanMatchDialog relatedLoanMatchDialog = new RelatedLoanMatchDialog())
      {
        int num = (int) relatedLoanMatchDialog.ShowDialog((IWin32Window) this);
        this.setRelatedLoanMatchType(relatedLoanMatchDialog.SelectedMatchType);
      }
    }

    public override void AddFilters(FieldFilterList filters)
    {
      base.AddFilters(filters);
      bool flag = false;
      foreach (FieldFilter filter in (List<FieldFilter>) filters)
      {
        if (filter.DataSource == FilterDataSource.Loan)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.setRelatedLoanMatchType(RelatedLoanMatchType.None);
      else if (filters.RelatedLoanMatchType == RelatedLoanMatchType.None)
        this.setRelatedLoanMatchType(RelatedLoanMatchType.LastClosed);
      else
        this.setRelatedLoanMatchType(filters.RelatedLoanMatchType);
    }

    public override FieldFilterList GetCurrentFilter()
    {
      FieldFilterList currentFilter = base.GetCurrentFilter();
      if (this.FooterVisible)
        currentFilter.RelatedLoanMatchType = !this.radAnyLoan.Checked ? (!this.radAnyClosed.Checked ? (!this.radLastOriginated.Checked ? RelatedLoanMatchType.LastClosed : RelatedLoanMatchType.LastOriginated) : RelatedLoanMatchType.AnyClosed) : RelatedLoanMatchType.AnyOriginated;
      return currentFilter;
    }

    private void setRelatedLoanMatchType(RelatedLoanMatchType matchType)
    {
      if (matchType == RelatedLoanMatchType.None)
      {
        this.FooterVisible = false;
      }
      else
      {
        this.FooterVisible = true;
        if (matchType == RelatedLoanMatchType.AnyOriginated)
          this.radAnyLoan.Checked = true;
        else if (matchType == RelatedLoanMatchType.AnyClosed)
          this.radAnyClosed.Checked = true;
        else if (matchType == RelatedLoanMatchType.LastOriginated)
          this.radLastOriginated.Checked = true;
        else
          this.radLastClosed.Checked = true;
      }
    }

    private void onLoanMatchTypeChange(object sender, EventArgs e) => this.OnDataChange();
  }
}
