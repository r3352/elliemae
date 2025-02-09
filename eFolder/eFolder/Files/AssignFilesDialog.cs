// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.AssignFilesDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Viewers;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class AssignFilesDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private DocumentLog doc;
    private GridViewDataManager gvFilesMgr;
    private ArrayList fileList = new ArrayList();
    private IContainer components;
    private GroupContainer gcFiles;
    private Button btnCancel;
    private Button btnAssign;
    private Panel pnlFooter;
    private CollapsibleSplitter csFiles;
    private GridView gvFiles;
    private BorderPanel pnlRight;
    private FileAttachmentViewerControl fileViewer;
    private ToolTip tooltip;

    public AssignFilesDialog(LoanDataMgr loanDataMgr, DocumentLog doc)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.loanDataMgr = loanDataMgr;
      this.doc = doc;
      this.initFileList();
      this.loadFileList();
    }

    public FileAttachment[] Files
    {
      get => (FileAttachment[]) this.fileList.ToArray(typeof (FileAttachment));
    }

    private void setWindowSize()
    {
      if (Form.ActiveForm != null)
      {
        Form form = Form.ActiveForm;
        while (form.Owner != null)
          form = form.Owner;
        this.Width = Convert.ToInt32((double) form.Width * 0.95);
        this.Height = Convert.ToInt32((double) form.Height * 0.95);
      }
      else
      {
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Width = Convert.ToInt32((double) workingArea.Width * 0.95);
        workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Height = Convert.ToInt32((double) workingArea.Height * 0.95);
      }
    }

    private void initFileList()
    {
      this.gvFilesMgr = new GridViewDataManager(this.gvFiles, this.loanDataMgr);
      this.gvFilesMgr.CreateLayout(new TableLayout.Column[2]
      {
        GridViewDataManager.NameWithIconColumn,
        GridViewDataManager.DateTimeColumn
      });
      this.gvFiles.Columns[0].ActivatedEditorType = GVActivatedEditorType.TextBox;
    }

    private void loadFileList()
    {
      this.gvFiles.Items.Clear();
      foreach (FileAttachment unassignedAttachment in this.loanDataMgr.FileAttachments.GetUnassignedAttachments())
        this.gvFilesMgr.AddItem(unassignedAttachment, (FileAttachmentReference) null);
      this.gvFiles.ReSort();
    }

    private void showFiles(FileAttachment[] fileList)
    {
      this.gvFiles.SelectedItems.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFiles.Items)
      {
        if (Array.IndexOf<object>((object[]) fileList, gvItem.Tag) >= 0)
          gvItem.Selected = true;
      }
      this.showFiles();
    }

    private void showFiles()
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gvFiles.SelectedItems)
        arrayList.Add(selectedItem.Tag);
      FileAttachment[] array = (FileAttachment[]) arrayList.ToArray(typeof (FileAttachment));
      if (array.Length != 0)
        this.fileViewer.LoadFiles(array);
      else
        this.fileViewer.CloseFile();
    }

    private void gvFiles_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      string str = e.EditorControl.Text.Trim();
      this.gvFiles.CancelEditing();
      if (!(str != e.SubItem.Text) || !(str != string.Empty))
        return;
      FileAttachment tag = (FileAttachment) e.SubItem.Item.Tag;
      tag.Title = str;
      this.gvFilesMgr.RefreshItem(e.SubItem.Item, tag, (FileAttachmentReference) null);
    }

    private void gvFiles_BeforeSelectedIndexCommitted(object sender, CancelEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
    }

    private void gvFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAssign.Enabled = this.gvFiles.SelectedItems.Count > 0;
    }

    private void gvFiles_SelectedIndexCommitted(object sender, EventArgs e) => this.showFiles();

    private void fileViewer_LoadAttachments(object source, EventArgs e)
    {
      this.loadFileList();
      this.showFiles(this.fileViewer.Attachments);
    }

    private void btnAssign_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.gvFiles.SelectedItems)
      {
        FileAttachment tag = (FileAttachment) selectedItem.Tag;
        this.doc.Files.Add(tag.ID, Session.UserID, true);
        this.fileList.Add((object) tag);
      }
      this.DialogResult = DialogResult.OK;
    }

    private void AssignFilesDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
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
      this.gcFiles = new GroupContainer();
      this.gvFiles = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.btnCancel = new Button();
      this.btnAssign = new Button();
      this.pnlFooter = new Panel();
      this.csFiles = new CollapsibleSplitter();
      this.pnlRight = new BorderPanel();
      this.fileViewer = new FileAttachmentViewerControl();
      this.gcFiles.SuspendLayout();
      this.pnlFooter.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.SuspendLayout();
      this.gcFiles.Controls.Add((Control) this.gvFiles);
      this.gcFiles.Dock = DockStyle.Left;
      this.gcFiles.HeaderForeColor = SystemColors.ControlText;
      this.gcFiles.Location = new Point(0, 0);
      this.gcFiles.Name = "gcFiles";
      this.gcFiles.Size = new Size(356, 521);
      this.gcFiles.TabIndex = 0;
      this.gcFiles.Text = "Files";
      this.gvFiles.BorderStyle = BorderStyle.None;
      this.gvFiles.ClearSelectionsOnEmptyRowClick = false;
      this.gvFiles.Dock = DockStyle.Fill;
      this.gvFiles.Location = new Point(1, 26);
      this.gvFiles.Name = "gvFiles";
      this.gvFiles.Size = new Size(354, 494);
      this.gvFiles.TabIndex = 1;
      this.gvFiles.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvFiles.UseCompatibleEditingBehavior = true;
      this.gvFiles.BeforeSelectedIndexCommitted += new CancelEventHandler(this.gvFiles_BeforeSelectedIndexCommitted);
      this.gvFiles.SelectedIndexChanged += new EventHandler(this.gvFiles_SelectedIndexChanged);
      this.gvFiles.SelectedIndexCommitted += new EventHandler(this.gvFiles_SelectedIndexCommitted);
      this.gvFiles.EditorClosing += new GVSubItemEditingEventHandler(this.gvFiles_EditorClosing);
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(692, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnAssign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAssign.Enabled = false;
      this.btnAssign.Location = new Point(616, 9);
      this.btnAssign.Name = "btnAssign";
      this.btnAssign.Size = new Size(75, 22);
      this.btnAssign.TabIndex = 6;
      this.btnAssign.Text = "Assign";
      this.btnAssign.UseVisualStyleBackColor = true;
      this.btnAssign.Click += new EventHandler(this.btnAssign_Click);
      this.pnlFooter.Controls.Add((Control) this.btnCancel);
      this.pnlFooter.Controls.Add((Control) this.btnAssign);
      this.pnlFooter.Dock = DockStyle.Bottom;
      this.pnlFooter.Location = new Point(0, 521);
      this.pnlFooter.Name = "pnlFooter";
      this.pnlFooter.Size = new Size(774, 40);
      this.pnlFooter.TabIndex = 5;
      this.csFiles.AnimationDelay = 20;
      this.csFiles.AnimationStep = 20;
      this.csFiles.BorderStyle3D = Border3DStyle.Flat;
      this.csFiles.ControlToHide = (Control) this.gcFiles;
      this.csFiles.ExpandParentForm = false;
      this.csFiles.Location = new Point(356, 0);
      this.csFiles.Name = "csLeft";
      this.csFiles.TabIndex = 2;
      this.csFiles.TabStop = false;
      this.csFiles.UseAnimations = false;
      this.csFiles.VisualStyle = VisualStyles.Encompass;
      this.pnlRight.Controls.Add((Control) this.fileViewer);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(363, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(411, 521);
      this.pnlRight.TabIndex = 14;
      this.fileViewer.Dock = DockStyle.Fill;
      this.fileViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fileViewer.Location = new Point(1, 1);
      this.fileViewer.Name = "fileViewer";
      this.fileViewer.Size = new Size(409, 519);
      this.fileViewer.TabIndex = 10;
      this.fileViewer.LoadAttachments += new EventHandler(this.fileViewer_LoadAttachments);
      this.AcceptButton = (IButtonControl) this.btnAssign;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(774, 561);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.csFiles);
      this.Controls.Add((Control) this.gcFiles);
      this.Controls.Add((Control) this.pnlFooter);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MinimizeBox = false;
      this.Name = nameof (AssignFilesDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Attach Unassigned Files";
      this.FormClosing += new FormClosingEventHandler(this.AssignFilesDialog_FormClosing);
      this.gcFiles.ResumeLayout(false);
      this.pnlFooter.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
