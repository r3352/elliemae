// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.RequiredDocumentsControl
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.eFolder.Documents;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class RequiredDocumentsControl : UserControl, IRefreshContents
  {
    private const string className = "RequiredDocumentsControl";
    private static readonly string sw = Tracing.SwDataEngine;
    private MilestoneLog msToCheck;
    private DocMilestonePair[] reqDocs;
    private LogList logList;
    private LoanData loan;
    private Hashtable reqDocsTitle = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private bool editable = true;
    private DocumentTrackingSetup docSetting;
    private bool inChecked;
    private Hashtable loanMilestones;
    private bool includePrevious;
    private bool refreshDocList;
    private IContainer components;
    private Button btnEFolder;
    private GroupContainer groupContainer1;
    private GridView gridViewDocs;
    private ImageList imageList1;
    private ToolTip toolTip1;

    public RequiredDocumentsControl(MilestoneLog msToCheck, Hashtable loanMilestones)
    {
      this.loan = Session.LoanData;
      this.msToCheck = msToCheck;
      this.loanMilestones = loanMilestones;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      try
      {
        if (!new eFolderAccessRights(Session.LoanDataMgr).CanAccessDocumentTab)
          this.editable = false;
      }
      catch (Exception ex)
      {
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "Cannot check access rights: Error: " + ex.Message);
      }
      this.docSetting = Session.LoanDataMgr.SystemConfiguration.DocumentTrackingSetup;
      this.btnEFolder.Click += new EventHandler(this.btnEFolder_Click);
      this.ParentChanged += new EventHandler(this.RequiredDocumentsControl_ParentChanged);
    }

    public RequiredDocumentsControl(MilestoneLog currentLog)
    {
      this.loan = Session.LoanData;
      this.msToCheck = currentLog;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
    }

    private void loan_LogRecordChanged(object source, LogRecordEventArgs e)
    {
      Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Verbose, nameof (RequiredDocumentsControl), "Checking InvokeRequired For LogRecordChanged");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.loan_LogRecordChanged);
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Verbose, nameof (RequiredDocumentsControl), "Calling BeginInvoke For LogRecordChanged");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else if (Session.MainForm == Form.ActiveForm)
        this.RefreshDocumentList((DocMilestonePair[]) null, false);
      else
        this.refreshDocList = true;
    }

    private void btnEFolder_Click(object sender, EventArgs e)
    {
      eFolderDialog.ShowInstance(Session.DefaultInstance);
      this.RefreshDocumentList((DocMilestonePair[]) null, false);
    }

    public void SetButtonStatus(bool enabled) => this.btnEFolder.Enabled = enabled;

    public void RefreshContents() => this.RefreshDocumentList((DocMilestonePair[]) null, false);

    public void RefreshLoanContents() => this.RefreshContents();

    public void RefreshHistoryDocumentList(List<DocumentLog> docs)
    {
      this.gridViewDocs.SubItemCheck -= new GVSubItemEventHandler(this.gridViewDocs_SubItemCheck);
      this.gridViewDocs.Items.Clear();
      this.gridViewDocs.BeginUpdate();
      string empty = string.Empty;
      for (int index = 0; index < docs.Count; ++index)
      {
        GVItem gvItem = new GVItem("");
        if (this.reqDocsTitle != null && this.reqDocsTitle.ContainsKey((object) docs[index].Title))
          gvItem.ImageIndex = !docs[index].IsAttachedToLog ? 0 : 1;
        gvItem.SubItems.Add((object) this.buildDocumentStatus(docs[index]));
        gvItem.Tag = (object) docs[index];
        if (docs[index].Received)
          gvItem.SubItems[1].Checked = true;
        this.gridViewDocs.Items.Add(gvItem);
      }
      this.gridViewDocs.EndUpdate();
      this.gridViewDocs.SubItemCheck += new GVSubItemEventHandler(this.gridViewDocs_SubItemCheck);
    }

    public void RefreshDocumentList(DocMilestonePair[] reqDocs, bool includePrevious)
    {
      if (this.loan == null)
        return;
      if (reqDocs != null)
        this.reqDocs = reqDocs;
      this.includePrevious = includePrevious;
      this.logList = this.loan.GetLogList();
      if (this.logList == null)
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: logList is null!");
      MilestoneLog[] allMilestones1 = this.logList.GetAllMilestones();
      if (allMilestones1 == null)
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: msLogs is null!");
      this.loan.LogRecordChanged -= new LogRecordEventHandler(this.loan_LogRecordChanged);
      this.loan.LogRecordAdded -= new LogRecordEventHandler(this.loan_LogRecordAdded);
      this.loan.LogRecordRemoved -= new LogRecordEventHandler(this.loan_LogRecordRemoved);
      this.addRequiredDocuments(includePrevious);
      this.loan.LogRecordChanged += new LogRecordEventHandler(this.loan_LogRecordChanged);
      this.loan.LogRecordAdded += new LogRecordEventHandler(this.loan_LogRecordAdded);
      this.loan.LogRecordRemoved += new LogRecordEventHandler(this.loan_LogRecordRemoved);
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      if (this.msToCheck == null)
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: this.msToCheck is null!");
      else if (this.msToCheck.Stage == null)
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: this.msToCheck.Stage is null!");
      if (Session.BPM == null)
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: Session.BPM is null!");
      if (Session.BPM.GetBpmManager(BpmCategory.Milestones) == null)
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: Session.BPM.GetBpmManager(BpmCategory.Milestones) is null!");
      if (Session.LoanDataMgr == null)
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: Session.LoanDataMgr is null!");
      else if (Session.LoanDataMgr.FileAttachments == null)
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: Session.LoanDataMgr.FileAttachments is null!");
      if (this.reqDocsTitle == null)
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: this.reqDocsTitle is null!");
      for (int index1 = 0; index1 < allMilestones1.Length; ++index1)
      {
        IEnumerable<EllieMae.EMLite.Workflow.Milestone> allMilestones2 = ((BpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestones();
        if (allMilestones2 == null)
          Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: ms is null!");
        if (allMilestones1[index1] == null)
          Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: msLogs[" + (object) index1 + "] is null!");
        else if (allMilestones1[index1].Stage == null)
          Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: msLogs[" + (object) index1 + "].Stage is null!");
        DocumentLog[] documentsByStage = this.logList.GetDocumentsByStage(allMilestones1[index1].Stage, allMilestones2);
        if (documentsByStage != null && documentsByStage.Length != 0)
        {
          if (this.msToCheck.Stage == allMilestones1[index1].Stage)
            documentLogList.InsertRange(documentLogList.Count, (IEnumerable<DocumentLog>) documentsByStage);
          else if (this.reqDocsTitle != null)
          {
            for (int index2 = 0; index2 < documentsByStage.Length; ++index2)
            {
              if (this.reqDocsTitle.ContainsKey((object) documentsByStage[index2].Title))
              {
                DocMilestonePair docMilestonePair = (DocMilestonePair) this.reqDocsTitle[(object) documentsByStage[index2].Title];
                if (docMilestonePair == null)
                  Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: regMSInfo is null!");
                if (docMilestonePair.AttachedRequired)
                {
                  if (Session.LoanDataMgr.FileAttachments.ContainsAttachment(documentsByStage[index2]))
                    continue;
                }
                else if (documentsByStage[index2].Received)
                  continue;
                documentLogList.Add(documentsByStage[index2]);
              }
            }
          }
        }
        if (this.msToCheck.Stage == allMilestones1[index1].Stage && !includePrevious)
        {
          documentLogList = new List<DocumentLog>();
          documentLogList.AddRange((IEnumerable<DocumentLog>) documentsByStage);
        }
      }
      if (documentLogList == null)
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: allDocs is null!");
      DocumentLog[] array = documentLogList.ToArray();
      if (array == null)
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: docs is null!");
      this.gridViewDocs.SubItemCheck -= new GVSubItemEventHandler(this.gridViewDocs_SubItemCheck);
      this.gridViewDocs.Items.Clear();
      this.gridViewDocs.BeginUpdate();
      string empty = string.Empty;
      for (int index = 0; index < array.Length; ++index)
      {
        GVItem gvItem = new GVItem("");
        if (this.reqDocsTitle != null && this.reqDocsTitle.ContainsKey((object) array[index].Title))
        {
          DocMilestonePair docMilestonePair = (DocMilestonePair) this.reqDocsTitle[(object) array[index].Title];
          if (docMilestonePair == null)
            Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "RefreshDocumentList: regMSInfo2 is null!");
          gvItem.ImageIndex = !docMilestonePair.AttachedRequired ? 0 : 1;
        }
        gvItem.SubItems.Add((object) this.buildDocumentStatus(array[index]));
        gvItem.Tag = (object) array[index];
        if (array[index].Received)
          gvItem.SubItems[1].Checked = true;
        this.gridViewDocs.Items.Add(gvItem);
      }
      this.gridViewDocs.EndUpdate();
      this.gridViewDocs.SubItemCheck += new GVSubItemEventHandler(this.gridViewDocs_SubItemCheck);
    }

    private void loan_LogRecordAdded(object source, LogRecordEventArgs e)
    {
      Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Verbose, nameof (RequiredDocumentsControl), "Checking InvokeRequired For LogRecordAdded");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.loan_LogRecordAdded);
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Verbose, nameof (RequiredDocumentsControl), "Calling BeginInvoke For LogRecordAdded");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        if (e == null || !(e.LogRecord is DocumentLog))
          return;
        this.RefreshDocumentList((DocMilestonePair[]) null, false);
      }
    }

    private void loan_LogRecordRemoved(object source, LogRecordEventArgs e)
    {
      Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Verbose, nameof (RequiredDocumentsControl), "Checking InvokeRequired For LogRecordRemoved");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.loan_LogRecordRemoved);
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Verbose, nameof (RequiredDocumentsControl), "Calling BeginInvoke For LogRecordRemoved");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        if (e == null || !(e.LogRecord is DocumentLog))
          return;
        this.RefreshDocumentList((DocMilestonePair[]) null, false);
      }
    }

    private string buildDocumentStatus(DocumentLog log)
    {
      string str = log.Title;
      if (log is VerifLog)
        str = str + "-" + log.RequestedFrom;
      if (log.Status != "needed" && log.Status != "")
        str = str + "  " + log.Status + " : " + log.Date.ToString("MM/dd/yy");
      return str;
    }

    public bool VerifyRequiredDocuments()
    {
      if (this.reqDocs == null || this.reqDocs.Length == 0)
        return true;
      foreach (DictionaryEntry dictionaryEntry in this.reqDocsTitle)
      {
        bool flag1 = false;
        bool flag2 = false;
        object key = dictionaryEntry.Key;
        object obj = dictionaryEntry.Value;
        if (this.gridViewDocs.Items.Count == 0)
          return true;
        for (int nItemIndex = 0; nItemIndex < this.gridViewDocs.Items.Count; ++nItemIndex)
        {
          DocumentLog tag = (DocumentLog) this.gridViewDocs.Items[nItemIndex].Tag;
          if (tag.Title == dictionaryEntry.Key.ToString())
          {
            flag2 = true;
            if (this.gridViewDocs.Items[nItemIndex].SubItems[1].Checked)
            {
              if (((DocMilestonePair) dictionaryEntry.Value).AttachedRequired)
              {
                if (Session.LoanDataMgr.FileAttachments.ContainsAttachment(tag))
                {
                  flag1 = true;
                  break;
                }
              }
              else
              {
                flag1 = true;
                break;
              }
            }
          }
        }
        if (!flag2)
          flag1 = true;
        if (!flag1)
          return false;
      }
      return true;
    }

    private void gridViewDocs_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.loan.LogRecordChanged -= new LogRecordEventHandler(this.loan_LogRecordChanged);
      this.gridViewDocs.SubItemCheck -= new GVSubItemEventHandler(this.gridViewDocs_SubItemCheck);
      if (!this.editable)
      {
        e.SubItem.Checked = !e.SubItem.Checked;
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to edit document.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.gridViewDocs.SubItemCheck += new GVSubItemEventHandler(this.gridViewDocs_SubItemCheck);
        this.loan.LogRecordChanged += new LogRecordEventHandler(this.loan_LogRecordChanged);
      }
      else
      {
        this.inChecked = true;
        DocumentLog tag = (DocumentLog) this.gridViewDocs.Items[e.SubItem.Item.Index].Tag;
        if (e.SubItem.Checked)
          tag.MarkAsReceived(DateTime.Now, Session.UserID);
        else
          tag.UnmarkAsReceived();
        this.gridViewDocs.Items[e.SubItem.Item.Index].SubItems[1].Text = this.buildDocumentStatus(tag);
        Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
        this.gridViewDocs.SubItemCheck += new GVSubItemEventHandler(this.gridViewDocs_SubItemCheck);
        this.loan.LogRecordChanged += new LogRecordEventHandler(this.loan_LogRecordChanged);
      }
    }

    private void addRequiredDocuments(bool includePrevious)
    {
      if (this.reqDocs == null || this.reqDocs.Length == 0)
        return;
      DocumentLog[] allDocuments = this.logList.GetAllDocuments(false);
      if (allDocuments == null)
        Tracing.Log(RequiredDocumentsControl.sw, nameof (RequiredDocumentsControl), TraceLevel.Error, "addRequiredDocuments: allDocs is null!");
      Tracing.Log(RequiredDocumentsControl.sw, nameof (RequiredDocumentsControl), TraceLevel.Verbose, "Checking the loan for the presence of " + (object) this.reqDocs.Length + " required documents. The loan contains " + (object) allDocuments.Length + " DocumentLog entries.");
      for (int index = 0; index < this.reqDocs.Length; ++index)
      {
        DocumentTemplate byId = this.docSetting.GetByID(this.reqDocs[index].DocGuid);
        if (byId == null)
          Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "addRequiredDocuments: can't find document with GUID '" + this.reqDocs[index].DocGuid + "'.");
        else if (this.logList.GetMilestoneByID(this.reqDocs[index].MilestoneID) == null)
        {
          Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Info, nameof (RequiredDocumentsControl), "addRequiredDocuments: can't find milestone with ID '" + this.reqDocs[index].MilestoneID + "'.");
        }
        else
        {
          MilestoneLog milestoneById = this.logList.GetMilestoneByID(this.reqDocs[index].MilestoneID);
          Tracing.Log(RequiredDocumentsControl.sw, nameof (RequiredDocumentsControl), TraceLevel.Verbose, "Checking for required document '" + byId.Name + "' at milestone '" + milestoneById.Stage + "'.");
          if (this.reqDocsTitle == null)
            Tracing.Log(RequiredDocumentsControl.sw, nameof (RequiredDocumentsControl), TraceLevel.Error, "addRequiredDocuments: this.reqDocsTitle is null!");
          if (!this.reqDocsTitle.ContainsKey((object) byId.Name))
            this.reqDocsTitle.Add((object) byId.Name, (object) this.reqDocs[index]);
          DocumentLog documentLog1 = (DocumentLog) null;
          foreach (DocumentLog documentLog2 in allDocuments)
          {
            if (this.logList.GetMilestone(documentLog2.Stage) != null && string.Compare(documentLog2.Title, byId.Name, true) == 0)
            {
              documentLog1 = documentLog2;
              break;
            }
          }
          if (documentLog1 == null)
          {
            this.addDocumentToLoan(byId, this.reqDocs[index].MilestoneID);
            Tracing.Log(RequiredDocumentsControl.sw, nameof (RequiredDocumentsControl), TraceLevel.Info, "Added required document '" + byId.Name + "' at milestone '" + milestoneById.Stage + "'.");
          }
        }
      }
    }

    private DocumentLog addDocumentToLoan(DocumentTemplate cond, string milestoneID)
    {
      DocumentLog rec = new DocumentLog(cond, Session.UserID, this.loan.PairId);
      MilestoneLog milestoneById = this.logList.GetMilestoneByID(milestoneID);
      rec.Stage = milestoneById == null ? string.Empty : milestoneById.Stage;
      try
      {
        this.loan.GetLogList().AddRecord((LogRecordBase) rec);
      }
      catch (Exception ex)
      {
        Tracing.Log(RequiredDocumentsControl.sw, TraceLevel.Error, nameof (RequiredDocumentsControl), "addDocumentToLoan: can't add document set to loan file. " + cond.Name + "Error: " + ex.Message);
        return (DocumentLog) null;
      }
      return rec;
    }

    private void gridViewDocs_SubItemClick(object source, GVSubItemMouseEventArgs e)
    {
      if (this.inChecked)
      {
        this.inChecked = false;
      }
      else
      {
        this.inChecked = false;
        if (e.SubItem.Index != 1)
          return;
        DocumentLog tag = (DocumentLog) this.gridViewDocs.Items[e.SubItem.Item.Index].Tag;
        if (tag == null)
          return;
        DocumentDetailsDialog.ShowInstance(Session.LoanDataMgr, tag);
      }
    }

    private void gridViewDocs_SubItemEnter(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Index != 0)
        return;
      if (e.SubItem.ImageIndex == 0)
      {
        this.toolTip1.Show("Document is required!", (IWin32Window) this, new Point(20, e.SubItem.SelectionRegion.Y + 50));
      }
      else
      {
        if (e.SubItem.ImageIndex != 1)
          return;
        this.toolTip1.Show("Document attachment is required!", (IWin32Window) this, new Point(20, e.SubItem.SelectionRegion.Y + 50));
      }
    }

    private void gridViewDocs_SubItemLeave(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Index != 0)
        return;
      this.toolTip1.RemoveAll();
    }

    private void RequiredDocumentsControl_ParentChanged(object sender, EventArgs e)
    {
      if (this.Parent == null)
        Session.MainForm.Activated -= new EventHandler(this.MainForm_Activated);
      else
        Session.MainForm.Activated += new EventHandler(this.MainForm_Activated);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.loan != null)
        {
          this.loan.LogRecordChanged -= new LogRecordEventHandler(this.loan_LogRecordChanged);
          this.loan = (LoanData) null;
        }
        Session.MainForm.Activated -= new EventHandler(this.MainForm_Activated);
      }
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void MainForm_Activated(object sender, EventArgs e)
    {
      if (!this.refreshDocList)
        return;
      this.RefreshDocumentList((DocMilestonePair[]) null, false);
      this.refreshDocList = false;
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RequiredDocumentsControl));
      this.btnEFolder = new Button();
      this.groupContainer1 = new GroupContainer();
      this.gridViewDocs = new GridView();
      this.imageList1 = new ImageList(this.components);
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.btnEFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEFolder.Location = new Point(355, 2);
      this.btnEFolder.Name = "btnEFolder";
      this.btnEFolder.Size = new Size(73, 22);
      this.btnEFolder.TabIndex = 3;
      this.btnEFolder.Text = "eFolder";
      this.btnEFolder.UseVisualStyleBackColor = true;
      this.groupContainer1.Controls.Add((Control) this.gridViewDocs);
      this.groupContainer1.Controls.Add((Control) this.btnEFolder);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(433, 292);
      this.groupContainer1.TabIndex = 7;
      this.groupContainer1.Text = "Documents";
      this.gridViewDocs.AllowColumnResize = false;
      this.gridViewDocs.AllowMultiselect = false;
      this.gridViewDocs.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Task";
      gvColumn1.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn1.Width = 20;
      gvColumn2.CheckBoxes = true;
      gvColumn2.Cursor = Cursors.Hand;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Column";
      gvColumn2.Width = 411;
      this.gridViewDocs.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewDocs.Dock = DockStyle.Fill;
      this.gridViewDocs.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gridViewDocs.HeaderHeight = 0;
      this.gridViewDocs.HeaderVisible = false;
      this.gridViewDocs.ImageList = this.imageList1;
      this.gridViewDocs.Location = new Point(1, 26);
      this.gridViewDocs.Name = "gridViewDocs";
      this.gridViewDocs.Selectable = false;
      this.gridViewDocs.Size = new Size(431, 265);
      this.gridViewDocs.TabIndex = 9;
      this.gridViewDocs.SubItemEnter += new GVSubItemEventHandler(this.gridViewDocs_SubItemEnter);
      this.gridViewDocs.SubItemClick += new GVSubItemMouseEventHandler(this.gridViewDocs_SubItemClick);
      this.gridViewDocs.SubItemLeave += new GVSubItemEventHandler(this.gridViewDocs_SubItemLeave);
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "required.png");
      this.imageList1.Images.SetKeyName(1, "attachment-required.png");
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (RequiredDocumentsControl);
      this.Size = new Size(433, 292);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
