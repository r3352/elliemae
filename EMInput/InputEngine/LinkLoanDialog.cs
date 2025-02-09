// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LinkLoanDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LinkLoanDialog : Form
  {
    private string currentGUID = string.Empty;
    private bool forLinkSync;
    private ICursor pipelineCursor;
    private LoanReportFieldDefs fieldDefs;
    private PipelineInfo selectedInfo;
    private LoanDataMgr selectedLinkLoanDataMgr;
    private IContainer components;
    private Label folderLabel;
    private ComboBox folderCombo;
    private Button cancelButton;
    private Button okbutton;
    private GridView detailListView;
    private GroupContainer groupContainer1;
    private PageListNavigator loanPageListNavigator;

    public LinkLoanDialog(string currentFolder, string currentGUID, bool forLinkSync)
    {
      this.currentGUID = currentGUID;
      this.forLinkSync = forLinkSync;
      this.InitializeComponent();
      this.initLoanFolders(currentFolder);
      this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.DatabaseFieldsNoAudit);
      this.initItemsWithPagination(currentFolder);
      this.folderCombo.SelectedIndexChanged += new EventHandler(this.folderCombo_SelectedIndexChanged);
    }

    private void initLoanFolders(string selectedFolder)
    {
      string[] allLoanFolderNames = Session.LoanManager.GetAllLoanFolderNames(false);
      this.folderCombo.Items.Clear();
      int num = 0;
      foreach (string strA in allLoanFolderNames)
      {
        this.folderCombo.Items.Add((object) strA);
        if (string.Compare(strA, selectedFolder, true) == 0)
          num = this.folderCombo.Items.Count - 1;
      }
      this.folderCombo.SelectedIndex = num;
    }

    private SortField[] getCurrentSortFields()
    {
      return this.getSortFieldsForColumnSort(this.detailListView.Columns.GetSortOrder());
    }

    private SortField[] getSortFieldsForColumnSort(GVColumnSort[] sortOrder)
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumnSort gvColumnSort in sortOrder)
      {
        string tag = (string) this.detailListView.Columns[gvColumnSort.Column].Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string str in tag.Split(chArray))
        {
          SortField sortFieldForColumn = this.getSortFieldForColumn(str.Trim(), gvColumnSort.SortOrder);
          if (sortFieldForColumn != null)
            sortFieldList.Add(sortFieldForColumn);
        }
      }
      return sortFieldList.ToArray();
    }

    private SortField getSortFieldForColumn(string columnID, SortOrder sortOrder)
    {
      LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(columnID);
      return fieldByCriterionName != null ? new SortField(fieldByCriterionName.SortTerm, sortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending, fieldByCriterionName.DataConversion) : new SortField(columnID, sortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending, DataConversion.None);
    }

    private bool retrievePipelineData(string selectedFolder, SortField[] sortFields = null)
    {
      try
      {
        if (sortFields == null)
          sortFields = this.getCurrentSortFields();
        if (this.pipelineCursor != null)
        {
          try
          {
            this.pipelineCursor.Dispose();
          }
          catch
          {
          }
          this.pipelineCursor = (ICursor) null;
        }
        this.pipelineCursor = Session.LoanManager.OpenPipeline(selectedFolder, LoanInfo.Right.Read, (string[]) null, PipelineData.All, sortFields, (QueryCriterion) null, false, 0);
        this.loanPageListNavigator.NumberOfItems = this.pipelineCursor.GetItemCount();
        return true;
      }
      catch
      {
        return false;
      }
    }

    private void initItemsWithPagination(string selectedFolder)
    {
      if (!this.retrievePipelineData(selectedFolder))
        return;
      this.displayCurrentPage(false, 0);
    }

    private GVItem createGVItemForPipelineInfo(PipelineInfo pInfo)
    {
      GVItem itemForPipelineInfo = new GVItem(pInfo.LoanNumber);
      Hashtable info = pInfo.Info;
      LockInfo lockInfo = pInfo.LockInfo;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      itemForPipelineInfo.SubItems.Add((object) pInfo.LastName);
      itemForPipelineInfo.SubItems.Add((object) pInfo.FirstName);
      itemForPipelineInfo.SubItems.Add((object) (string) info[(object) "LoanOfficerName"]);
      itemForPipelineInfo.SubItems.Add((object) (string) info[(object) "LoanProcessorName"]);
      itemForPipelineInfo.SubItems.Add((object) (string) info[(object) "LoanCloserName"]);
      string str1 = "Not Locked";
      if (lockInfo.LockedFor == LoanInfo.LockReason.Downloaded)
        str1 = "Downloaded by " + lockInfo.LockedBy;
      else if (lockInfo.LockedFor == LoanInfo.LockReason.OpenForWork)
        str1 = "Opened by " + lockInfo.LockedBy;
      itemForPipelineInfo.SubItems.Add((object) str1);
      itemForPipelineInfo.SubItems.Add((object) (string) info[(object) "Address1"]);
      double num = Utils.ParseDouble(info[(object) "LoanAmount"]);
      if (num != 0.0)
        itemForPipelineInfo.SubItems.Add((object) num.ToString("N2"));
      else
        itemForPipelineInfo.SubItems.Add((object) "");
      itemForPipelineInfo.SubItems.Add((object) (string) info[(object) "LoanType"]);
      itemForPipelineInfo.SubItems.Add((object) (string) info[(object) "LoanPurpose"]);
      itemForPipelineInfo.SubItems.Add((object) ((string) info[(object) "CoBorrowerFirstName"] + " " + (string) info[(object) "CoBorrowerLastName"]));
      itemForPipelineInfo.SubItems.Add((object) (string) info[(object) "LoanName"]);
      string str2 = (string) info[(object) "ActionTaken"];
      if (str2 == string.Empty)
        str2 = "Active Loan";
      itemForPipelineInfo.SubItems.Add((object) str2);
      itemForPipelineInfo.SubItems.Add((object) pInfo.GUID);
      itemForPipelineInfo.Tag = (object) pInfo;
      return itemForPipelineInfo;
    }

    private void initItems(string selectedFolder)
    {
      PipelineInfo[] pipeline = Session.LoanManager.GetPipeline(selectedFolder, false);
      this.detailListView.Items.Clear();
      if (pipeline == null || pipeline.Length == 0)
        return;
      this.detailListView.BeginUpdate();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 0; index < pipeline.Length; ++index)
      {
        Hashtable info = pipeline[index].Info;
        LockInfo lockInfo = pipeline[index].LockInfo;
        GVItem gvItem = new GVItem(pipeline[index].LoanNumber);
        gvItem.SubItems.Add((object) pipeline[index].LastName);
        gvItem.SubItems.Add((object) pipeline[index].FirstName);
        gvItem.SubItems.Add((object) (string) info[(object) "LoanOfficerName"]);
        gvItem.SubItems.Add((object) (string) info[(object) "LoanProcessorName"]);
        gvItem.SubItems.Add((object) (string) info[(object) "LoanCloserName"]);
        string str1 = "Not Locked";
        if (lockInfo.LockedFor == LoanInfo.LockReason.Downloaded)
          str1 = "Downloaded by " + lockInfo.LockedBy;
        else if (lockInfo.LockedFor == LoanInfo.LockReason.OpenForWork)
          str1 = "Opened by " + lockInfo.LockedBy;
        gvItem.SubItems.Add((object) str1);
        gvItem.SubItems.Add((object) (string) info[(object) "Address1"]);
        double num = Utils.ParseDouble(info[(object) "LoanAmount"]);
        if (num != 0.0)
          gvItem.SubItems.Add((object) num.ToString("N2"));
        else
          gvItem.SubItems.Add((object) "");
        gvItem.SubItems.Add((object) (string) info[(object) "LoanType"]);
        gvItem.SubItems.Add((object) (string) info[(object) "LoanPurpose"]);
        gvItem.SubItems.Add((object) ((string) info[(object) "CoBorrowerFirstName"] + " " + (string) info[(object) "CoBorrowerLastName"]));
        gvItem.SubItems.Add((object) (string) info[(object) "LoanName"]);
        string str2 = (string) info[(object) "ActionTaken"];
        if (str2 == string.Empty)
          str2 = "Active Loan";
        gvItem.SubItems.Add((object) str2);
        gvItem.SubItems.Add((object) pipeline[index].GUID);
        gvItem.Tag = (object) pipeline[index];
        this.detailListView.Items.Add(gvItem);
      }
      this.detailListView.EndUpdate();
    }

    private void folderCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.initItemsWithPagination(this.folderCombo.Text);
    }

    private void okbutton_Click(object sender, EventArgs e)
    {
      if (this.detailListView.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a loan to link.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.selectedInfo = (PipelineInfo) this.detailListView.SelectedItems[0].Tag;
        if (string.Compare(this.selectedInfo.GUID, this.currentGUID, true) == 0)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You cannot select current loan to link.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          if ((this.selectedInfo.LinkGuid ?? "") != string.Empty && Utils.Dialog((IWin32Window) this, "The loan you selected is currently linking to other loan. Do you want to remove this link?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            return;
          if (string.Compare(this.selectedInfo.LinkGuid, this.currentGUID, true) == 0)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "The loan you selected is already linked to current loan. Please select different loan.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            if (this.forLinkSync)
            {
              this.selectedLinkLoanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, this.SelectedInfo.LoanFolder, this.SelectedInfo.LoanName, false);
              if (this.selectedLinkLoanDataMgr.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(false).Length != 0)
              {
                int num4 = (int) Utils.Dialog((IWin32Window) this, "The selected loan has already been disclosed to the borrower.  It is not eligible for processing through Link and Sync.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
            }
            this.DialogResult = DialogResult.OK;
          }
        }
      }
    }

    public PipelineInfo SelectedInfo => this.selectedInfo;

    public LoanDataMgr SelectedLinkLoanDataMgr => this.selectedLinkLoanDataMgr;

    private void LinkLoanDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void loanPageListNavigator_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      using (CursorActivator.Wait())
        this.displayCurrentPage(false, 1);
    }

    private void displayCurrentPage(bool preserveSelections, int sqlRead)
    {
      this.detailListView.BeginUpdate();
      int currentPageItemIndex = this.loanPageListNavigator.CurrentPageItemIndex;
      int currentPageItemCount = this.loanPageListNavigator.CurrentPageItemCount;
      PipelineInfo[] pipelineInfoArray = new PipelineInfo[0];
      if (currentPageItemCount > 0)
        pipelineInfoArray = (PipelineInfo[]) this.pipelineCursor.GetItems(currentPageItemIndex, currentPageItemCount, false, sqlRead);
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      if (preserveSelections)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.detailListView.Items)
        {
          if (gvItem.Selected && gvItem.Tag != null)
            dictionary[((PipelineInfo) gvItem.Tag).GUID] = true;
        }
      }
      this.detailListView.Items.Clear();
      for (int index = 0; index < pipelineInfoArray.Length; ++index)
      {
        GVItem itemForPipelineInfo = this.createGVItemForPipelineInfo(pipelineInfoArray[index]);
        this.detailListView.Items.Add(itemForPipelineInfo);
        if (dictionary.ContainsKey(pipelineInfoArray[index].GUID))
          itemForPipelineInfo.Selected = true;
      }
      if (this.detailListView.Items.Count > 0 && this.detailListView.SelectedItems.Count == 0)
        this.detailListView.Items[0].Selected = true;
      this.detailListView.EndUpdate();
    }

    private void detailListView_SortItems(object source, GVColumnSortEventArgs e)
    {
      using (CursorActivator.Wait())
      {
        if (e.Column == 6 || !this.retrievePipelineData(this.folderCombo.Text, this.getSortFieldsForColumnSort(e.ColumnSorts)))
          e.Cancel = true;
        else
          this.displayCurrentPage(true, 1);
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
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      GVColumn gvColumn15 = new GVColumn();
      this.folderLabel = new Label();
      this.folderCombo = new ComboBox();
      this.cancelButton = new Button();
      this.okbutton = new Button();
      this.detailListView = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.loanPageListNavigator = new PageListNavigator();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.folderLabel.AutoSize = true;
      this.folderLabel.Location = new Point(12, 16);
      this.folderLabel.Name = "folderLabel";
      this.folderLabel.Size = new Size(63, 13);
      this.folderLabel.TabIndex = 5;
      this.folderLabel.Text = "Loan Folder";
      this.folderLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.folderCombo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.folderCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.folderCombo.Location = new Point(82, 13);
      this.folderCombo.Name = "folderCombo";
      this.folderCombo.Size = new Size(452, 21);
      this.folderCombo.TabIndex = 4;
      this.cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelButton.DialogResult = DialogResult.Cancel;
      this.cancelButton.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.cancelButton.Location = new Point(704, 396);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new Size(75, 24);
      this.cancelButton.TabIndex = 7;
      this.cancelButton.Text = "&Cancel";
      this.okbutton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okbutton.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.okbutton.Location = new Point(624, 396);
      this.okbutton.Name = "okbutton";
      this.okbutton.Size = new Size(75, 24);
      this.okbutton.TabIndex = 8;
      this.okbutton.Text = "&Link";
      this.okbutton.Click += new EventHandler(this.okbutton_Click);
      this.detailListView.AllowMultiselect = false;
      this.detailListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Tag = (object) "Loan.LoanNumber";
      gvColumn1.Text = "Loan Number";
      gvColumn1.Width = 92;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Tag = (object) "Loan.BorrowerLastName";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 80;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Tag = (object) "Loan.BorrowerFirstName";
      gvColumn3.Text = "First Name";
      gvColumn3.Width = 79;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Tag = (object) "Loan.LoanOfficerName";
      gvColumn4.Text = "LO";
      gvColumn4.Width = 60;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Tag = (object) "Loan.LoanProcessorName";
      gvColumn5.Text = "LP";
      gvColumn5.Width = 60;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Tag = (object) "Loan.LoanCloserName";
      gvColumn6.Text = "CL";
      gvColumn6.Width = 60;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "File Status";
      gvColumn7.Width = 93;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.Tag = (object) "Loan.Address1";
      gvColumn8.Text = "Address";
      gvColumn8.Width = 74;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column9";
      gvColumn9.SortMethod = GVSortMethod.Numeric;
      gvColumn9.Tag = (object) "Loan.LoanAmount";
      gvColumn9.Text = "Loan Amount";
      gvColumn9.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn9.Width = 95;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column10";
      gvColumn10.Tag = (object) "Loan.LoanType";
      gvColumn10.Text = "Loan Type";
      gvColumn10.Width = 84;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column11";
      gvColumn11.Tag = (object) "Loan.LoanPurpose";
      gvColumn11.Text = "Loan Purpose";
      gvColumn11.Width = 98;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column12";
      gvColumn12.Tag = (object) "Loan.CoBorrowerFirstName,Loan.CoBorrowerLastName";
      gvColumn12.Text = "Co-Borrower";
      gvColumn12.Width = 88;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column13";
      gvColumn13.Tag = (object) "Loan.LoanName";
      gvColumn13.Text = "Loan Name";
      gvColumn13.Width = 86;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column14";
      gvColumn14.Tag = (object) "Loan.ActionTaken";
      gvColumn14.Text = "Loan Status";
      gvColumn14.Width = 60;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "Column15";
      gvColumn15.Tag = (object) "Loan.Guid";
      gvColumn15.Text = "GUID";
      gvColumn15.Width = 60;
      this.detailListView.Columns.AddRange(new GVColumn[15]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13,
        gvColumn14,
        gvColumn15
      });
      this.detailListView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.detailListView.Location = new Point(0, 26);
      this.detailListView.Name = "detailListView";
      this.detailListView.Size = new Size(769, 302);
      this.detailListView.SortOption = GVSortOption.Owner;
      this.detailListView.TabIndex = 9;
      this.detailListView.SortItems += new GVColumnSortEventHandler(this.detailListView_SortItems);
      this.groupContainer1.AutoSize = true;
      this.groupContainer1.Controls.Add((Control) this.detailListView);
      this.groupContainer1.Controls.Add((Control) this.loanPageListNavigator);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(7, 45);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(769, 328);
      this.groupContainer1.TabIndex = 10;
      this.loanPageListNavigator.AutoSize = true;
      this.loanPageListNavigator.BackColor = Color.Transparent;
      this.loanPageListNavigator.Font = new Font("Arial", 8f);
      this.loanPageListNavigator.Location = new Point(0, 0);
      this.loanPageListNavigator.Name = "loanPageListNavigator";
      this.loanPageListNavigator.NumberOfItems = 0;
      this.loanPageListNavigator.Size = new Size(254, 25);
      this.loanPageListNavigator.TabIndex = 0;
      this.loanPageListNavigator.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.loanPageListNavigator_PageChangedEvent);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(788, 428);
      this.Controls.Add((Control) this.okbutton);
      this.Controls.Add((Control) this.cancelButton);
      this.Controls.Add((Control) this.folderLabel);
      this.Controls.Add((Control) this.folderCombo);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (LinkLoanDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Link to a Loan";
      this.KeyPress += new KeyPressEventHandler(this.LinkLoanDialog_KeyPress);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
