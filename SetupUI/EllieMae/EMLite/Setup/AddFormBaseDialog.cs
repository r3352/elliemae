// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddFormBaseDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddFormBaseDialog : Form
  {
    private const string className = "AddFormBaseDialog";
    private static readonly string sw = Tracing.SwOutsideLoan;
    protected Panel panelForAll;
    protected RadioButton radioAll;
    protected RadioButton radioTemplate;
    protected Button cancelBtn;
    protected Button okBtn;
    protected FSExplorer fsExplorerTemplate;
    protected TemplateIFSExplorer ifsExplorer;
    protected System.ComponentModel.Container components;
    protected Label label1;
    protected ComboBox cboRuleValues;
    protected CheckBox chkAttached;
    private Panel panelForDocs;
    private Panel panelDetail;
    protected GridView gridViewAll;
    protected GridView gridViewTemplate;
    private bool forDocs;
    protected Sessions.Session session;

    public AddFormBaseDialog(Sessions.Session session, bool forDocs)
    {
      this.session = session;
      this.forDocs = forDocs;
      this.InitializeComponent();
      this.radioAll.Checked = true;
      this.gridViewAll.Dock = DockStyle.Fill;
      if (forDocs)
        this.gridViewTemplate.Columns[0].Text = "Documents";
      else
        this.panelForDocs.Visible = false;
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
      this.fsExplorerTemplate = new FSExplorer(this.session);
      this.panelForAll = new Panel();
      this.panelDetail = new Panel();
      this.gridViewAll = new GridView();
      this.gridViewTemplate = new GridView();
      this.panelForDocs = new Panel();
      this.label1 = new Label();
      this.chkAttached = new CheckBox();
      this.cboRuleValues = new ComboBox();
      this.radioAll = new RadioButton();
      this.radioTemplate = new RadioButton();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.panelForAll.SuspendLayout();
      this.panelDetail.SuspendLayout();
      this.panelForDocs.SuspendLayout();
      this.SuspendLayout();
      this.fsExplorerTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.fsExplorerTemplate.FolderComboSelectedIndex = -1;
      this.fsExplorerTemplate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fsExplorerTemplate.HasPublicRight = true;
      this.fsExplorerTemplate.Location = new Point(3, 3);
      this.fsExplorerTemplate.Name = "fsExplorerTemplate";
      this.fsExplorerTemplate.RenameButtonSize = new Size(62, 22);
      this.fsExplorerTemplate.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.fsExplorerTemplate.Size = new Size(312, 316);
      this.fsExplorerTemplate.TabIndex = 0;
      this.panelForAll.Controls.Add((Control) this.panelDetail);
      this.panelForAll.Controls.Add((Control) this.panelForDocs);
      this.panelForAll.Location = new Point(8, 48);
      this.panelForAll.Name = "panelForAll";
      this.panelForAll.Size = new Size(600, 361);
      this.panelForAll.TabIndex = 1;
      this.panelDetail.Controls.Add((Control) this.gridViewAll);
      this.panelDetail.Controls.Add((Control) this.fsExplorerTemplate);
      this.panelDetail.Controls.Add((Control) this.gridViewTemplate);
      this.panelDetail.Dock = DockStyle.Fill;
      this.panelDetail.Location = new Point(0, 0);
      this.panelDetail.Name = "panelDetail";
      this.panelDetail.Size = new Size(600, 324);
      this.panelDetail.TabIndex = 6;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Form";
      gvColumn1.Width = 322;
      this.gridViewAll.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gridViewAll.HeaderHeight = 0;
      this.gridViewAll.HeaderVisible = false;
      this.gridViewAll.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewAll.Location = new Point(144, 116);
      this.gridViewAll.Name = "gridViewAll";
      this.gridViewAll.Size = new Size(326, 118);
      this.gridViewAll.TabIndex = 8;
      this.gridViewTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "headerList";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Form";
      gvColumn2.Width = 146;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "milestoneList";
      gvColumn3.Text = "Milestone";
      gvColumn3.Width = 126;
      this.gridViewTemplate.Columns.AddRange(new GVColumn[2]
      {
        gvColumn2,
        gvColumn3
      });
      this.gridViewTemplate.HeaderHeight = 22;
      this.gridViewTemplate.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewTemplate.Location = new Point(321, 3);
      this.gridViewTemplate.Name = "gridViewTemplate";
      this.gridViewTemplate.Size = new Size(276, 316);
      this.gridViewTemplate.TabIndex = 9;
      this.gridViewTemplate.SizeChanged += new EventHandler(this.gridViewTemplate_SizeChanged);
      this.panelForDocs.Controls.Add((Control) this.label1);
      this.panelForDocs.Controls.Add((Control) this.chkAttached);
      this.panelForDocs.Controls.Add((Control) this.cboRuleValues);
      this.panelForDocs.Dock = DockStyle.Bottom;
      this.panelForDocs.Location = new Point(0, 324);
      this.panelForDocs.Name = "panelForDocs";
      this.panelForDocs.Size = new Size(600, 37);
      this.panelForDocs.TabIndex = 5;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(70, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "For Milestone";
      this.chkAttached.AutoSize = true;
      this.chkAttached.Location = new Point(308, 10);
      this.chkAttached.Name = "chkAttached";
      this.chkAttached.Size = new Size(219, 17);
      this.chkAttached.TabIndex = 2;
      this.chkAttached.Text = "Requires an attachment to the document";
      this.chkAttached.UseVisualStyleBackColor = true;
      this.cboRuleValues.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRuleValues.FormattingEnabled = true;
      this.cboRuleValues.Location = new Point(102, 8);
      this.cboRuleValues.Name = "cboRuleValues";
      this.cboRuleValues.Size = new Size(180, 21);
      this.cboRuleValues.TabIndex = 0;
      this.radioAll.Location = new Point(12, 16);
      this.radioAll.Name = "radioAll";
      this.radioAll.Size = new Size(160, 20);
      this.radioAll.TabIndex = 2;
      this.radioAll.Text = "All Input Forms";
      this.radioAll.CheckedChanged += new EventHandler(this.formType_CheckedChanged);
      this.radioTemplate.Location = new Point(166, 16);
      this.radioTemplate.Name = "radioTemplate";
      this.radioTemplate.Size = new Size(300, 20);
      this.radioTemplate.TabIndex = 3;
      this.radioTemplate.Text = "Input Form Set Templates";
      this.radioTemplate.CheckedChanged += new EventHandler(this.formType_CheckedChanged);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(533, 415);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 7;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(449, 415);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 6;
      this.okBtn.Text = "&Add";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(620, 450);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.radioTemplate);
      this.Controls.Add((Control) this.radioAll);
      this.Controls.Add((Control) this.panelForAll);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddFormBaseDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Input Forms";
      this.Closing += new CancelEventHandler(this.AddFormDialog_Closing);
      this.KeyPress += new KeyPressEventHandler(this.AddFormBaseDialog_KeyPress);
      this.panelForAll.ResumeLayout(false);
      this.panelDetail.ResumeLayout(false);
      this.panelForDocs.ResumeLayout(false);
      this.panelForDocs.PerformLayout();
      this.ResumeLayout(false);
    }

    protected virtual void initTemplates(string templateType)
    {
      FileSystemEntry fileSystemEntry;
      try
      {
        fileSystemEntry = FileSystemEntry.Parse(this.session.GetPrivateProfileString(templateType, "LastFolderViewed") ?? "");
        if (!this.ifsExplorer.EntryExists(fileSystemEntry))
          fileSystemEntry = (FileSystemEntry) null;
      }
      catch
      {
        fileSystemEntry = (FileSystemEntry) null;
      }
      if (fileSystemEntry != FileSystemEntry.PublicRoot)
        fileSystemEntry = FileSystemEntry.PublicRoot;
      this.fsExplorerTemplate.Init((IFSExplorerBase) this.ifsExplorer, fileSystemEntry, true);
      this.fsExplorerTemplate.SelectedEntryChanged += new EventHandler(this.fsExplorerTemplate_SelectedEntryChanged);
      this.fsExplorerTemplate.FolderChanged += new EventHandler(this.fsExplorerTemplate_FolderChanged);
    }

    protected virtual void formType_CheckedChanged(object sender, EventArgs e)
    {
      if (this.radioTemplate.Checked)
      {
        this.fsExplorerTemplate.Visible = true;
        this.gridViewTemplate.Visible = true;
        this.gridViewAll.Visible = this.cboRuleValues.Visible = this.label1.Visible = false;
        this.chkAttached.Left = this.label1.Left;
      }
      else
      {
        this.fsExplorerTemplate.Visible = this.gridViewTemplate.Visible = false;
        this.gridViewAll.Visible = this.cboRuleValues.Visible = this.label1.Visible = true;
        this.chkAttached.Left = this.cboRuleValues.Left + this.cboRuleValues.Width + 20;
      }
    }

    protected virtual void okBtn_Click(object sender, EventArgs e)
    {
    }

    protected virtual void fsExplorerTemplate_FolderChanged(object sender, EventArgs e)
    {
      if (this.fsExplorerTemplate.SelectedItems.Count != 0)
        return;
      this.gridViewTemplate.Items.Clear();
    }

    protected virtual void fsExplorerTemplate_SelectedEntryChanged(object sender, EventArgs e)
    {
      this.gridViewTemplate.Items.Clear();
      if (this.fsExplorerTemplate.SelectedItems.Count == 0)
        return;
      int num = this.fsExplorerTemplate.SelectedItems[0].Tag.ToString() == "" ? 1 : 0;
    }

    protected virtual void AddFormDialog_Closing(object sender, CancelEventArgs e)
    {
      this.ifsExplorer.Dispose();
      this.fsExplorerTemplate.Dispose();
    }

    protected virtual void gridViewTemplate_SizeChanged(object sender, EventArgs e)
    {
      int width = this.gridViewTemplate.ClientSize.Width;
      if (!this.forDocs)
      {
        this.gridViewTemplate.Columns[0].Width = width;
      }
      else
      {
        this.gridViewTemplate.Columns[0].Width = (int) ((double) width * 0.5);
        this.gridViewTemplate.Columns[1].SpringToFit = true;
      }
    }

    private void AddFormBaseDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }
  }
}
