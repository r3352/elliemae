// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactLoanCRMSearch
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactLoanCRMSearch : Form
  {
    private ContactType contactType;
    private int currentContactId;
    private const string className = "BorrowerLoansForm";
    private TableLayout currentLayout;
    private GridViewLayoutManager layoutManager;
    private bool suspendRefresh;
    private static readonly string sw = Tracing.SwOutsideLoan;
    private LoanReportFieldDefs fieldDefs;
    private PipelineInfo[] currentAssociatedLoans;
    private string layoutFileName = "BorrowerPipelineView";
    private string contactSSN = "";
    private IContainer components;
    private GroupContainer groupContainer1;
    private GridView gvLoans;
    private Panel pnlButton;
    private Button btnCancel;
    private Button btnOK;

    public ContactLoanCRMSearch(
      ContactType contactType,
      int contactID,
      string contactSSN,
      PipelineInfo[] currentAssociatedLoans)
    {
      this.InitializeComponent();
      this.contactType = contactType;
      this.currentContactId = contactID;
      this.contactSSN = contactSSN;
      this.currentAssociatedLoans = currentAssociatedLoans;
      this.loadPersonalLayout();
    }

    private void loadPersonalLayout()
    {
      try
      {
        BinaryObject userSettings = Session.User.GetUserSettings(this.layoutFileName);
        this.setLayout(this.addSpecialColumn(userSettings == null ? this.getDemoTableLayout() : userSettings.ToObject<TableLayout>()));
      }
      catch (Exception ex)
      {
        Tracing.Log(ContactLoanCRMSearch.sw, "BorrowerLoansForm", TraceLevel.Error, "Error loading layout: " + (object) ex);
      }
    }

    private TableLayout addSpecialColumn(TableLayout layout)
    {
      if (layout.GetColumnByID("BorrowerAssociatesCount.AssociatedCount") == null)
        layout.InsertColumn(0, new TableLayout.Column("BorrowerAssociatesCount.AssociatedCount", "Currently Associated", HorizontalAlignment.Center, 120));
      return layout;
    }

    private void setLayout(TableLayout layout)
    {
      this.currentLayout = layout;
      this.applyTableLayout(this.currentLayout);
      this.RefreshPipeline();
    }

    private void applyTableLayout(TableLayout layout)
    {
      if (this.layoutManager == null)
        this.layoutManager = this.createLayoutManager();
      this.layoutManager.ApplyLayout(layout);
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.AllDatabaseFields);
      foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) this.fieldDefs)
      {
        if (fullTableLayout.GetColumnByID(fieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(new TableLayout.Column(fieldDef.CriterionFieldName, fieldDef.Name, fieldDef.Description, fieldDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric ? HorizontalAlignment.Right : HorizontalAlignment.Left, 100));
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private GridViewLayoutManager createLayoutManager()
    {
      return new GridViewLayoutManager(this.gvLoans, this.getFullTableLayout());
    }

    public void RefreshPipeline()
    {
      if (this.suspendRefresh)
        return;
      this.loadLoanList();
    }

    private TableLayout getDemoTableLayout()
    {
      TableLayout demoTableLayout = new TableLayout();
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.DateFileOpened", "Date File Opened", "Date File Opened", "", HorizontalAlignment.Center, 140, true));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanNumber", "Loan Number", "Loan Number", "", HorizontalAlignment.Left, 103, true));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.CurrentMilestoneName", "Current Milestone Name", "Current Milestone Name", "", HorizontalAlignment.Left, 180, true));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.CurrentMilestoneName", "Current Milestone Name", "Current Milestone Name", "", HorizontalAlignment.Left, 180, true));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LastModified", "Last Modified", HorizontalAlignment.Left, 140)
      {
        SortOrder = SortOrder.Descending
      });
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.CurrentMilestoneDate", "Current Milestone Date", HorizontalAlignment.Left, 140));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanPurpose", "Loan Purpose", HorizontalAlignment.Left, 90));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanAmount", "Loan Amount", HorizontalAlignment.Right, 103));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanRate", "Loan Rate", HorizontalAlignment.Left, 40));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LTV", "LTV", HorizontalAlignment.Left, 40));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.Amortization", "Amortization", HorizontalAlignment.Left, 80));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanType", "Loan Type", HorizontalAlignment.Left, 90));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LienPosition", "Lien Position", HorizontalAlignment.Left, 90));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.DateCompleted", "Date Completed", HorizontalAlignment.Left, 140));
      return demoTableLayout;
    }

    private void loadLoanList()
    {
      this.gvLoans.Items.Clear();
      string[] fieldList = this.generateFieldList();
      QueryCriterion filter = (QueryCriterion) new StringValueCriterion("borrower.ssn", this.contactSSN, StringMatchType.Exact);
      foreach (PipelineInfo loanItem in Session.LoanManager.GetPipeline(LoanInfo.Right.Access, fieldList, PipelineData.CurrentUserAccessRightsOnly, (SortField[]) null, filter, false))
      {
        bool flag = false;
        foreach (PipelineInfo currentAssociatedLoan in this.currentAssociatedLoans)
        {
          if (currentAssociatedLoan.GUID == loanItem.GUID)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          this.addPipelineInfoToGV(loanItem, fieldList);
      }
    }

    private string[] generateFieldList()
    {
      List<string> stringList = new List<string>();
      foreach (TableLayout.Column column in this.layoutManager.GetCurrentLayout())
      {
        LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(column.ColumnID);
        if (!stringList.Contains(column.ColumnID))
          stringList.Add(column.ColumnID);
        if (fieldByCriterionName != null)
        {
          foreach (string relatedField in fieldByCriterionName.RelatedFields)
          {
            if (!stringList.Contains(relatedField))
              stringList.Add(relatedField);
          }
        }
      }
      return stringList.ToArray();
    }

    private void addPipelineInfoToGV(PipelineInfo loanItem, string[] fields)
    {
      List<string> stringList = new List<string>();
      bool flag = true;
      if (!loanItem.Rights.ContainsKey((object) Session.UserID))
        flag = false;
      IEnumerator<TableLayout.Column> enumerator = this.layoutManager.GetCurrentLayout().GetEnumerator();
      while (enumerator.MoveNext())
      {
        TableLayout.Column current = enumerator.Current;
        switch (current.ColumnID)
        {
          case "Loan.DateFileOpened":
          case "Loan.LoanAmount":
          case "Loan.CurrentMilestoneName":
          case "Loan.LoanNumber":
            stringList.Add(string.Concat(loanItem.GetField(current.ColumnID)));
            continue;
          case "BorrowerAssociatesCount.AssociatedCount":
            if (string.Concat(loanItem.GetField(current.ColumnID)) != "")
            {
              stringList.Add("Yes");
              continue;
            }
            stringList.Add("No");
            continue;
          default:
            if (!flag)
            {
              stringList.Add("");
              continue;
            }
            stringList.Add(string.Concat(loanItem.GetField(current.ColumnID)));
            continue;
        }
      }
      this.gvLoans.Items.Add(new GVItem(stringList.ToArray())
      {
        Tag = (object) loanItem
      });
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a loan file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.DialogResult = DialogResult.OK;
        this.Cursor = Cursors.WaitCursor;
        foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
        {
          PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
          if (!Session.UserInfo.IsSuperAdministrator())
          {
            if (!tag.Rights.ContainsKey((object) Session.UserID))
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "You do not have access right to loan '" + tag.LoanNumber + "'.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            if ((LoanInfo.Right) tag.Rights[(object) Session.UserID] == LoanInfo.Right.Read)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "You do not have Edit access right to loan '" + tag.LoanNumber + "'.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
          }
          ILoan loan = (ILoan) null;
          try
          {
            loan = Session.LoanManager.OpenLoan(tag.GUID);
            loan.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
            BorrowerInfo borrower1 = Session.ContactManager.GetBorrower(this.currentContactId);
            if (borrower1 == null)
              throw new Exception("Error retrieving borrower info object.");
            LoanData loanData = loan.GetLoanData(false);
            if (loanData == null)
              throw new Exception("Error retrieving loanData object.");
            PipelineInfo pipelineInfo = loanData.GetLogList() != null ? loanData.ToPipelineInfo() : throw new Exception("Error retrieving loanData.GetLogList() object.");
            if (pipelineInfo == null)
              throw new Exception("Error retrieving pipeLineInfo object.");
            BorrowerPair[] borrowerPairs = loanData.GetBorrowerPairs();
            if (borrowerPairs == null || borrowerPairs.Length == 0)
              throw new Exception("Error retrieving borPairs object.");
            if (pipelineInfo.Borrowers == null || pipelineInfo.Borrowers.Length == 0)
              throw new Exception("Error retrieving pipeLineInfo.Borrowers object.");
            foreach (PipelineInfo.Borrower borrower2 in pipelineInfo.Borrowers)
            {
              if (borrower2.SSN == borrower1.SSN && borrower2.PairIndex <= borrowerPairs.Length)
              {
                BorrowerPair borrowerPair = borrowerPairs[borrower2.PairIndex - 1];
                if (borrower2.BorrowerType == LoanBorrowerType.Borrower)
                  loanData.GetLogList().AddCRMMapping(borrowerPair.Borrower.Id, CRMLogType.BorrowerContact, borrower1.ContactGuid.ToString(), CRMRoleType.Borrower);
                else
                  loanData.GetLogList().AddCRMMapping(borrowerPair.CoBorrower.Id, CRMLogType.BorrowerContact, borrower1.ContactGuid.ToString(), CRMRoleType.Coborrower);
              }
            }
            loan.Save(loanData, "");
            loan.Unlock();
          }
          catch (LockException ex)
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, "Application can not modify loan '" + tag.LoanNumber + "' due to: " + ex.Message);
            loan.Unlock();
          }
          catch (Exception ex)
          {
            int num5 = (int) Utils.Dialog((IWin32Window) this, "Application can not modify loan '" + tag.LoanNumber + "' due to: " + ex.Message);
            loan.Unlock();
          }
        }
        this.Cursor = Cursors.Default;
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
      this.groupContainer1 = new GroupContainer();
      this.gvLoans = new GridView();
      this.pnlButton = new Panel();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.groupContainer1.SuspendLayout();
      this.pnlButton.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.gvLoans);
      this.groupContainer1.Controls.Add((Control) this.pnlButton);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(620, 480);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Loans";
      this.gvLoans.Dock = DockStyle.Fill;
      this.gvLoans.Location = new Point(1, 26);
      this.gvLoans.Name = "gvLoans";
      this.gvLoans.Size = new Size(618, 414);
      this.gvLoans.TabIndex = 1;
      this.pnlButton.Controls.Add((Control) this.btnCancel);
      this.pnlButton.Controls.Add((Control) this.btnOK);
      this.pnlButton.Dock = DockStyle.Bottom;
      this.pnlButton.Location = new Point(1, 440);
      this.pnlButton.Name = "pnlButton";
      this.pnlButton.Size = new Size(618, 39);
      this.pnlButton.TabIndex = 2;
      this.btnOK.Location = new Point(451, 6);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(532, 6);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(620, 480);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactLoanCRMSearch);
      this.Text = "Unassigned Loans";
      this.groupContainer1.ResumeLayout(false);
      this.pnlButton.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
