// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BorrowerLoansForm
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
using EllieMae.EMLite.Serialization;
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
  public class BorrowerLoansForm : Form, IBindingForm
  {
    private bool isReadOnly;
    private int currentContactId;
    private BorrowerInfo currentContact;
    private const string className = "BorrowerLoansForm";
    private TableLayout currentLayout;
    private GridViewLayoutManager layoutManager;
    private bool suspendEvents;
    private bool suspendRefresh;
    private static readonly string sw = Tracing.SwOutsideLoan;
    private LoanReportFieldDefs fieldDefs;
    private string layoutFileName = "BorrowerPipelineView";
    private PipelineInfo[] currentLoanList;
    private IContainer components;
    private GridView gvLoanList;
    private GroupContainer gcLoans;
    private Panel panel1;
    private Button siBtnSearchLoans;
    private Button siBtnDisconnect;

    public BorrowerLoansForm()
    {
      this.InitializeComponent();
      this.gcLoans.Text = "Associated Loans";
      this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.AllDatabaseFields);
    }

    public bool IsReadOnly
    {
      get => this.isReadOnly;
      set => this.isReadOnly = value;
    }

    public int CurrentContactID
    {
      get => this.currentContactId;
      set
      {
        this.currentContactId = -1;
        this.gvLoanList.Items.Clear();
        this.gcLoans.Text = "Associated Loans (" + (object) this.gvLoanList.Items.Count + ")";
        if (value >= 0)
        {
          this.currentContactId = value;
          this.setButtonState(true);
          this.loadPersonalLayout();
        }
        else
          this.setButtonState(false);
      }
    }

    public object CurrentContact
    {
      get => (object) this.currentContact;
      set
      {
        this.currentContactId = -1;
        this.gvLoanList.Items.Clear();
        this.currentContact = (BorrowerInfo) null;
        this.gcLoans.Text = "Associated Loans (" + (object) this.gvLoanList.Items.Count + ")";
        if (value != null)
        {
          this.currentContact = (BorrowerInfo) value;
          this.currentContactId = this.currentContact.ContactID;
          this.setButtonState(true);
          this.loadPersonalLayout();
        }
        else
          this.setButtonState(false);
      }
    }

    private void setButtonState(bool enable)
    {
      if (enable)
      {
        if (this.currentContact.SSN != "")
          this.siBtnSearchLoans.Enabled = true;
        else
          this.siBtnSearchLoans.Enabled = false;
      }
      else
        this.siBtnSearchLoans.Enabled = enable;
    }

    public bool isDirty() => false;

    private void loadPersonalLayout()
    {
      try
      {
        BinaryObject userSettings = Session.User.GetUserSettings(this.layoutFileName);
        this.setLayout(userSettings == null ? this.getDemoTableLayout() : userSettings.ToObject<TableLayout>());
      }
      catch (Exception ex)
      {
        Tracing.Log(BorrowerLoansForm.sw, nameof (BorrowerLoansForm), TraceLevel.Error, "Error loading layout: " + (object) ex);
      }
    }

    private void setLayout(TableLayout layout)
    {
      this.currentLayout = layout;
      this.suspendEvents = true;
      this.applyTableLayout(this.currentLayout);
      this.suspendEvents = false;
      this.RefreshPipeline();
    }

    private void applyTableLayout(TableLayout layout)
    {
      if (this.layoutManager == null)
        this.layoutManager = this.createLayoutManager();
      this.validateTableLayout(layout);
      this.layoutManager.ApplyLayout(layout, false);
    }

    private void validateTableLayout(TableLayout layout)
    {
      List<TableLayout.Column> columnList = new List<TableLayout.Column>();
      foreach (TableLayout.Column column in layout)
      {
        ReportFieldDef fieldByCriterionName = (ReportFieldDef) this.fieldDefs.GetFieldByCriterionName(column.ColumnID);
        if (fieldByCriterionName != null)
          column.Title = fieldByCriterionName.Description;
        else
          columnList.Add(column);
      }
      foreach (TableLayout.Column column in columnList)
        layout.Remove(column);
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) this.fieldDefs)
      {
        if (fullTableLayout.GetColumnByID(fieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(ReportFieldClientExtension.ToTableLayoutColumn(fieldDef));
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private GridViewLayoutManager createLayoutManager()
    {
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.gvLoanList, this.getFullTableLayout(), this.getDemoTableLayout());
      layoutManager.LayoutChanged += new EventHandler(this.onLayoutChanged);
      return layoutManager;
    }

    private void onLayoutChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.RefreshPipeline();
      this.currentLayout = this.layoutManager.GetCurrentLayout();
      using (BinaryObject data = new BinaryObject((IXmlSerializable) this.currentLayout))
        Session.User.SaveUserSettings(this.layoutFileName, data);
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
      if (this.fieldDefs.GetFieldByCriterionName("Loan.DateFileOpened") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.DateFileOpened", "Date File Started", "Date File Started", "", HorizontalAlignment.Left, 140, true));
      if (this.fieldDefs.GetFieldByCriterionName("Loan.LoanNumber") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanNumber", "Loan Number", "Loan Number", "", HorizontalAlignment.Left, 103, true));
      if (this.fieldDefs.GetFieldByCriterionName("Loan.CurrentMilestoneName") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.CurrentMilestoneName", "Current Milestone Name", "Current Milestone Name", "", HorizontalAlignment.Left, 180, true));
      if (this.fieldDefs.GetFieldByCriterionName("Loan.CurrentMilestoneName") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.CurrentMilestoneName", "Current Milestone Name", "Current Milestone Name", "", HorizontalAlignment.Left, 180, true));
      if (this.fieldDefs.GetFieldByCriterionName("Loan.LastModified") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.LastModified", "Last Modified", HorizontalAlignment.Left, 140)
        {
          SortOrder = SortOrder.Descending
        });
      if (this.fieldDefs.GetFieldByCriterionName("Loan.CurrentMilestoneDate") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.CurrentMilestoneDate", "Current Milestone Date", HorizontalAlignment.Left, 140));
      if (this.fieldDefs.GetFieldByCriterionName("Loan.LoanPurpose") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanPurpose", "Loan Purpose", HorizontalAlignment.Left, 90));
      if (this.fieldDefs.GetFieldByCriterionName("Loan.LoanAmount") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanAmount", "Loan Amount", HorizontalAlignment.Right, 103));
      if (this.fieldDefs.GetFieldByCriterionName("Loan.LoanRate") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanRate", "Loan Rate", HorizontalAlignment.Left, 40));
      if (this.fieldDefs.GetFieldByCriterionName("Loan.LTV") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.LTV", "LTV", HorizontalAlignment.Left, 40));
      if (this.fieldDefs.GetFieldByCriterionName("Loan.Amortization") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.Amortization", "Amortization", HorizontalAlignment.Left, 80));
      if (this.fieldDefs.GetFieldByCriterionName("Loan.LoanType") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanType", "Loan Type", HorizontalAlignment.Left, 90));
      if (this.fieldDefs.GetFieldByCriterionName("Loan.LienPosition") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.LienPosition", "Lien Position", HorizontalAlignment.Left, 90));
      if (this.fieldDefs.GetFieldByCriterionName("Loan.DateCompleted") != null)
        demoTableLayout.AddColumn(new TableLayout.Column("Loan.DateCompleted", "Date Completed", HorizontalAlignment.Left, 140));
      return demoTableLayout;
    }

    private void loadLoanList()
    {
      this.gvLoanList.Items.Clear();
      string[] fieldList = this.generateFieldList();
      this.currentLoanList = new PipelineInfo[0];
      this.gcLoans.Text = "Associated Loans (" + (object) this.gvLoanList.Items.Count + ")";
      ICursor cursor = Session.LoanManager.OpenContactPipeline(fieldList, (SortField[]) null, this.currentContactId, CRMLogType.BorrowerContact, false);
      if (cursor == null || cursor.GetItemCount() == 0)
        return;
      this.currentLoanList = (PipelineInfo[]) cursor.GetItems(0, cursor.GetItemCount(), false);
      LoanInfo.Right[] effectiveRightsForLoans = ((LoanAccessBpmManager) Session.BPM.GetBpmManager(BizRuleType.LoanAccess)).GetEffectiveRightsForLoans(this.currentLoanList);
      for (int index = 0; index < this.currentLoanList.Length; ++index)
        this.addPipelineInfoToGV(this.currentLoanList[index], fieldList, effectiveRightsForLoans[index]);
      this.gcLoans.Text = "Associated Loans (" + (object) this.gvLoanList.Items.Count + ")";
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

    private bool isLoanAccessible(PipelineInfo pinfo, LoanInfo.Right effectiveRights)
    {
      return Session.UserInfo.IsSuperAdministrator() || effectiveRights != LoanInfo.Right.NoRight;
    }

    private void addPipelineInfoToGV(
      PipelineInfo loanItem,
      string[] fields,
      LoanInfo.Right effectiveRights)
    {
      List<string> stringList = new List<string>();
      bool flag = true;
      if (!this.isLoanAccessible(loanItem, effectiveRights))
        flag = false;
      foreach (GVColumn column in this.gvLoanList.Columns)
      {
        TableLayout.Column tag = (TableLayout.Column) column.Tag;
        LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(tag.ColumnID);
        object displayElement = loanItem.Info[(object) tag.ColumnID];
        if (fieldByCriterionName != null)
          displayElement = fieldByCriterionName.ToDisplayElement(tag.ColumnID, loanItem, (Control) this.gvLoanList);
        string columnId = tag.ColumnID;
        if (columnId == "Loan.DateFileOpened" || columnId == "Loan.LoanAmount" || columnId == "Loan.CurrentMilestoneName" || columnId == "Loan.LoanNumber")
          stringList.Add(string.Concat(displayElement));
        else if (!flag)
          stringList.Add("");
        else
          stringList.Add(string.Concat(displayElement));
      }
      this.gvLoanList.Items.Add(new GVItem(stringList.ToArray())
      {
        Tag = (object) loanItem
      });
    }

    public bool SaveChanges()
    {
      this.setButtonState(true);
      return true;
    }

    private void siBtnDisconnect_Click(object sender, EventArgs e)
    {
      if (this.gvLoanList.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a loan file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to disconnect the link between the borrower and the selected loan(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          return;
        bool flag1 = false;
        this.Cursor = Cursors.WaitCursor;
        foreach (GVItem selectedItem in this.gvLoanList.SelectedItems)
        {
          PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
          bool flag2 = false;
          if (!Session.UserInfo.IsSuperAdministrator() && !tag.Rights.ContainsKey((object) Session.UserID))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "You do not have access right to loan (LoanNumber:" + tag.LoanNumber + ").", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            flag2 = true;
          }
          else if (!Session.UserInfo.IsSuperAdministrator())
          {
            LoanInfo.Right right = (LoanInfo.Right) tag.Rights[(object) Session.UserID];
            if (!Session.UserInfo.IsSuperAdministrator() && right == LoanInfo.Right.Read)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "You do not have Edit access right to the loan (LoanNumber:" + tag.LoanNumber + ").", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              flag2 = true;
            }
          }
          if (!flag2)
          {
            ILoan loan = (ILoan) null;
            try
            {
              LoanData loanData;
              if (Session.LoanData == null || Session.LoanData.GUID != tag.GUID)
              {
                loan = Session.LoanManager.OpenLoan(tag.GUID);
                loan.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
                loanData = loan.GetLoanData(false);
              }
              else
              {
                if (Session.SessionObjects.AllowConcurrentEditing && !Session.LoanDataMgr.LockLoanWithExclusiveA(true))
                  return;
                loanData = Session.LoanData;
              }
              loanData.GetLogList().RemoveCRMMapping(this.currentContact.ContactGuid.ToString());
              if (Session.LoanData == null || Session.LoanData.GUID != tag.GUID)
              {
                loan.Save(loanData, "");
                loan.Close(true);
              }
              else
              {
                Session.Application.GetService<ILoanEditor>().SaveLoan();
                Session.Application.GetService<ILoanEditor>().RefreshLoanContents();
              }
              flag1 = true;
            }
            catch (ExclusiveLockException ex)
            {
              int num4 = (int) Utils.Dialog((IWin32Window) this, "Failed to obtain exclusive lock for loan '" + tag.LoanNumber + "'.");
              loan.Close(true);
            }
            catch (LockException ex)
            {
              int num5 = (int) Utils.Dialog((IWin32Window) this, "Application can not modify loan '" + tag.LoanNumber + "' due to: " + ex.Message);
              loan.Close(true);
            }
            catch (Exception ex)
            {
              int num6 = (int) Utils.Dialog((IWin32Window) this, "Application can not modify loan '" + tag.LoanNumber + "' due to: " + ex.Message);
              loan.Close(true);
            }
          }
        }
        if (flag1)
          this.loadLoanList();
        this.Cursor = Cursors.Default;
      }
    }

    private void siBtnRefresh_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to disgard your customized setting and use the default setting?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      this.currentLayout = this.getDemoTableLayout();
      this.setLayout(this.currentLayout);
      this.onLayoutChanged((object) null, (EventArgs) null);
    }

    private void gvLoanList_DoubleClick(object sender, EventArgs e)
    {
      if (this.gvLoanList.SelectedItems.Count == 0)
        return;
      PipelineInfo tag = (PipelineInfo) this.gvLoanList.SelectedItems[0].Tag;
      if (DialogResult.No == Utils.Dialog((IWin32Window) this, "Do you want to open the selected loan file?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
        return;
      ILoanConsole service = Session.Application.GetService<ILoanConsole>();
      if (!service.HasAccessToLoanTab)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You do not have access to the Pipeline/Loan tab.");
      }
      else
      {
        if (service.HasOpenLoan)
        {
          if (Session.LoanDataMgr.Writable)
          {
            switch (Utils.Dialog((IWin32Window) this, "Do you want to save the currently opened loan first before opening this loan file?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
            {
              case DialogResult.Yes:
                service.CloseLoanWithoutPrompts(true);
                break;
              case DialogResult.No:
                service.CloseLoanWithoutPrompts(false);
                break;
              default:
                return;
            }
          }
          else
            service.CloseLoanWithoutPrompts(false);
        }
        if (service.OpenLoan(tag.GUID, true))
          return;
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Failed to open the selected loan file.");
      }
    }

    private void siBtnSearchLoans_Click(object sender, EventArgs e)
    {
      if (this.currentContact.SSN == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "To search loans, social security number must be provided.");
      }
      else
      {
        this.Cursor = Cursors.WaitCursor;
        ContactLoanCRMSearch contactLoanCrmSearch = new ContactLoanCRMSearch(ContactType.Borrower, this.currentContactId, this.currentContact.SSN, this.currentLoanList);
        this.Cursor = Cursors.Default;
        if (contactLoanCrmSearch.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.loadLoanList();
      }
    }

    private void gvLoanList_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvLoanList.SelectedItems.Count == 0)
        this.siBtnDisconnect.Enabled = false;
      else
        this.siBtnDisconnect.Enabled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.gcLoans = new GroupContainer();
      this.siBtnSearchLoans = new Button();
      this.siBtnDisconnect = new Button();
      this.gvLoanList = new GridView();
      this.panel1.SuspendLayout();
      this.gcLoans.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.gcLoans);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(0, 2, 2, 2);
      this.panel1.Size = new Size(713, 440);
      this.panel1.TabIndex = 2;
      this.gcLoans.Controls.Add((Control) this.siBtnSearchLoans);
      this.gcLoans.Controls.Add((Control) this.siBtnDisconnect);
      this.gcLoans.Controls.Add((Control) this.gvLoanList);
      this.gcLoans.Dock = DockStyle.Fill;
      this.gcLoans.Location = new Point(0, 2);
      this.gcLoans.Name = "gcLoans";
      this.gcLoans.Size = new Size(711, 436);
      this.gcLoans.TabIndex = 1;
      this.siBtnSearchLoans.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnSearchLoans.BackColor = SystemColors.Control;
      this.siBtnSearchLoans.Location = new Point(388, 2);
      this.siBtnSearchLoans.Margin = new Padding(0);
      this.siBtnSearchLoans.Name = "siBtnSearchLoans";
      this.siBtnSearchLoans.Size = new Size(140, 22);
      this.siBtnSearchLoans.TabIndex = 5;
      this.siBtnSearchLoans.Text = "Link to Loans Using SSN";
      this.siBtnSearchLoans.UseVisualStyleBackColor = true;
      this.siBtnSearchLoans.Click += new EventHandler(this.siBtnSearchLoans_Click);
      this.siBtnDisconnect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnDisconnect.BackColor = SystemColors.Control;
      this.siBtnDisconnect.Enabled = false;
      this.siBtnDisconnect.Location = new Point(528, 2);
      this.siBtnDisconnect.Margin = new Padding(0);
      this.siBtnDisconnect.Name = "siBtnDisconnect";
      this.siBtnDisconnect.Size = new Size(178, 22);
      this.siBtnDisconnect.TabIndex = 4;
      this.siBtnDisconnect.Text = "Break Link and Remove from List";
      this.siBtnDisconnect.UseVisualStyleBackColor = true;
      this.siBtnDisconnect.Click += new EventHandler(this.siBtnDisconnect_Click);
      this.gvLoanList.AllowColumnReorder = true;
      this.gvLoanList.BorderStyle = BorderStyle.None;
      this.gvLoanList.Dock = DockStyle.Fill;
      this.gvLoanList.Location = new Point(1, 26);
      this.gvLoanList.Name = "gvLoanList";
      this.gvLoanList.Size = new Size(709, 409);
      this.gvLoanList.TabIndex = 0;
      this.gvLoanList.SelectedIndexChanged += new EventHandler(this.gvLoanList_SelectedIndexChanged);
      this.gvLoanList.DoubleClick += new EventHandler(this.gvLoanList_DoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(713, 440);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BorrowerLoansForm);
      this.Text = "Contact Loans";
      this.panel1.ResumeLayout(false);
      this.gcLoans.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
