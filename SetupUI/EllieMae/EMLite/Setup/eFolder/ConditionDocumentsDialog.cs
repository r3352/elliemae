// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.ConditionDocumentsDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class ConditionDocumentsDialog : Form
  {
    private Sessions.Session session;
    private DocumentTrackingSetup docSetup;
    private GridViewDataManager gvDocumentsMgr;
    private DocumentTemplate[] docList;
    private bool multiSelect = true;
    private bool initFlag = true;
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private GridView gvDocuments;
    private ToolTip toolTip;

    public ConditionDocumentsDialog(
      Sessions.Session session,
      DocumentTrackingSetup docSetup,
      DocumentTemplate[] docList,
      bool multiSelect = true,
      DocumentTemplate defaultDoc = null)
    {
      this.InitializeComponent();
      this.session = session;
      this.docSetup = docSetup;
      this.gvDocuments.AllowMultiselect = multiSelect;
      this.multiSelect = multiSelect;
      this.initDocumentList();
      this.loadDocumentList(docList);
      if (!multiSelect && defaultDoc != null)
        this.gvDocuments.Items[this.gvDocuments.Items.GetItemByTag((object) defaultDoc).Index].Selected = true;
      else if (docList.Length != 0 && !multiSelect)
        this.gvDocuments.Items[this.gvDocuments.Items.GetItemByTag((object) docList[0]).Index].Selected = true;
      this.initFlag = false;
    }

    public DocumentTemplate[] Documents => this.docList;

    private void initDocumentList()
    {
      this.gvDocumentsMgr = new GridViewDataManager(this.session, this.gvDocuments, (LoanDataMgr) null);
      this.gvDocumentsMgr.CreateLayout(new TableLayout.Column[3]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DocTypeColumn,
        GridViewDataManager.DocSourceColumn
      });
      this.gvDocuments.Sort(0, SortOrder.Ascending);
      this.gvDocuments.Columns[0].CheckBoxes = this.multiSelect;
    }

    private void loadDocumentList(DocumentTemplate[] docList)
    {
      foreach (DocumentTemplate template in this.docSetup)
      {
        bool flag = false;
        if (Array.IndexOf<DocumentTemplate>(docList, template) >= 0)
          flag = true;
        this.gvDocumentsMgr.AddItem(template).Checked = flag;
      }
      this.gvDocuments.ReSort();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      if (this.multiSelect)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
        {
          if (gvItem.Checked)
            arrayList.Add(gvItem.Tag);
        }
      }
      else if (this.gvDocuments.SelectedItems.Count == 1)
        arrayList.Add(this.gvDocuments.SelectedItems[0].Tag);
      this.docList = (DocumentTemplate[]) arrayList.ToArray(typeof (DocumentTemplate));
      this.DialogResult = DialogResult.OK;
    }

    private void gvDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
      int hotItemIndex = this.gvDocuments.HotItemIndex;
      if (this.multiSelect)
      {
        this.gvDocuments.SelectedItems.Clear();
        this.gvDocuments.Items[hotItemIndex].Checked = !this.gvDocuments.Items[hotItemIndex].Checked;
      }
      else
      {
        if (this.initFlag)
          return;
        this.gvDocuments.SelectedIndexChanged -= new EventHandler(this.gvDocuments_SelectedIndexChanged);
        this.gvDocuments.SelectedItems.Clear();
        this.gvDocuments.Items[hotItemIndex].Selected = true;
        this.gvDocuments.SelectedIndexChanged += new EventHandler(this.gvDocuments_SelectedIndexChanged);
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
      this.components = (IContainer) new System.ComponentModel.Container();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.gvDocuments = new GridView();
      this.toolTip = new ToolTip(this.components);
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(387, 324);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 24);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(468, 324);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.gvDocuments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocuments.Location = new Point(12, 12);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(530, 304);
      this.gvDocuments.TabIndex = 0;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.SelectedIndexChanged += new EventHandler(this.gvDocuments_SelectedIndexChanged);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(554, 356);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.gvDocuments);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConditionDocumentsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Documents";
      this.ResumeLayout(false);
    }
  }
}
