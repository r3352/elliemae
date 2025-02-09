// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.AddRequestDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class AddRequestDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private GridViewDataManager gvTemplatesMgr;
    private List<DocumentLog> docList = new List<DocumentLog>();
    private string borrowerPairID;
    private IContainer components;
    private Button btnAdd;
    private Button btnCancel;
    private GridView gvTemplates;
    private ToolTip tooltip;

    public AddRequestDialog(LoanDataMgr loanDataMgr, string borrowerPairID)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.borrowerPairID = borrowerPairID;
      this.initTemplateList();
      this.loadTemplateList();
    }

    public DocumentLog[] Documents => this.docList.ToArray();

    private void initTemplateList()
    {
      this.gvTemplatesMgr = new GridViewDataManager(this.gvTemplates, this.loanDataMgr);
      this.gvTemplatesMgr.CreateLayout(new TableLayout.Column[3]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DocTypeColumn,
        GridViewDataManager.DocSourceColumn
      });
    }

    private void loadTemplateList()
    {
      foreach (DocumentTemplate template in this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup)
      {
        if (!Epass.IsEpassDoc(template.Name))
          this.gvTemplatesMgr.AddItem(template);
      }
      this.gvTemplates.Sort(0, SortOrder.Ascending);
    }

    private void gvTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = this.gvTemplates.SelectedItems.Count > 0;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      foreach (GVItem selectedItem in this.gvTemplates.SelectedItems)
      {
        DocumentLog logEntry = ((DocumentTemplate) selectedItem.Tag).CreateLogEntry(Session.UserID, this.borrowerPairID);
        logEntry.Stage = logList.NextStage;
        this.docList.Add(logEntry);
      }
      this.DialogResult = DialogResult.OK;
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
      this.btnAdd = new Button();
      this.btnCancel = new Button();
      this.gvTemplates = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.SuspendLayout();
      this.btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new Point(392, 299);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 22);
      this.btnAdd.TabIndex = 3;
      this.btnAdd.Text = "Add";
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(468, 299);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.gvTemplates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvTemplates.ClearSelectionsOnEmptyRowClick = false;
      this.gvTemplates.Location = new Point(12, 17);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(530, 276);
      this.gvTemplates.TabIndex = 2;
      this.gvTemplates.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
      this.AcceptButton = (IButtonControl) this.btnAdd;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(554, 336);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.gvTemplates);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddRequestDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Document";
      this.ResumeLayout(false);
    }
  }
}
