// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.SelectFolderControl
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class SelectFolderControl : UserControl
  {
    private bool isDirty;
    private LoanFolderInfo[] allFolders;
    private bool suspendEvent;
    private Sessions.Session session;
    private IContainer components;
    private Button btnSelectAll;
    private Button btnSelectAllExcept;
    private Button btnUnselectAll;
    private GroupContainer grpFolders;
    private FlowLayoutPanel flowLayoutPanel2;
    private Button deselectAllBtn;
    internal GridView gvFolders;
    private FlowLayoutPanel flowLayoutPanel1;
    private Panel panel1;
    private RadioButton radSelectedFolders;
    private Label label9;
    private RadioButton radActiveFolders;
    private RadioButton radAllFolders;

    public bool IsDirty => this.isDirty;

    public SelectFolderControl(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      int num = this.DesignMode ? 1 : 0;
    }

    public List<string> GetFolderNameList()
    {
      List<string> folderNameList = new List<string>();
      if (this.radAllFolders.Checked)
        folderNameList.Add(DashboardTemplate.DashboardAllFolder);
      else if (this.radActiveFolders.Checked)
      {
        folderNameList.Add(DashboardTemplate.DashboardNoArchiveFolder);
      }
      else
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFolders.Items)
        {
          if (gvItem.Checked)
            folderNameList.Add(((LoanFolderInfo) gvItem.Tag).Name);
        }
      }
      return folderNameList;
    }

    public void SetFolderNameList(
      List<string> folderNameList,
      bool selectAll,
      bool selectAllButArchive)
    {
      this.suspendEvent = true;
      this.gvFolders.Items.Clear();
      if (selectAll | selectAllButArchive)
      {
        this.grpFolders.Visible = false;
        if (selectAll)
          this.radAllFolders.Checked = true;
        else
          this.radActiveFolders.Checked = true;
        this.displayFolderNames();
      }
      else
      {
        this.radSelectedFolders.Checked = true;
        this.grpFolders.Visible = true;
        if (folderNameList != null)
          this.displayFolderNames(folderNameList);
        else
          this.displayFolderNames();
      }
      this.suspendEvent = false;
    }

    private void displayFolderNames(List<string> folderNameList)
    {
      if (this.allFolders == null)
        this.allFolders = this.session.LoanManager.GetAllLoanFolderInfos(false);
      List<string> stringList = new List<string>((IEnumerable<string>) folderNameList);
      this.gvFolders.Items.Clear();
      for (int index = 0; index < this.allFolders.Length; ++index)
      {
        GVItem gvItem = new GVItem(this.allFolders[index].DisplayName);
        gvItem.SubItems[1].Text = this.allFolders[index].Type == LoanFolderInfo.LoanFolderType.Archive ? "Yes" : "";
        gvItem.Tag = (object) this.allFolders[index];
        if (stringList.Contains(this.allFolders[index].Name))
        {
          gvItem.Checked = true;
          stringList.Remove(this.allFolders[index].Name);
        }
        this.gvFolders.Items.Add(gvItem);
      }
      this.isDirty = stringList.Count != 0;
      if (!this.isDirty)
        return;
      this.OnDataChangedEvent(EventArgs.Empty);
    }

    private void displayFolderNames()
    {
      if (this.allFolders == null)
        this.allFolders = this.session.LoanManager.GetAllLoanFolderInfos(false);
      foreach (LoanFolderInfo allFolder in this.allFolders)
      {
        GVItem gvItem = new GVItem(allFolder.DisplayName);
        gvItem.Tag = (object) allFolder;
        this.gvFolders.Items.Add(gvItem);
        if (!this.isNonRegularLoanFolder(gvItem))
          gvItem.Checked = true;
      }
      this.isDirty = true;
      this.OnDataChangedEvent(EventArgs.Empty);
    }

    private bool isNonRegularLoanFolder(GVItem gvItem) => ((LoanFolderInfo) gvItem.Tag).Type != 0;

    private void selectFolders(bool setSelected, bool selectArchive)
    {
      if (this.gvFolders.Items.Count == 0)
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFolders.Items)
      {
        if (!selectArchive && this.isNonRegularLoanFolder(gvItem))
          gvItem.Checked = false;
        else if (gvItem.Text.ToLower() != SystemSettings.TrashFolder.ToLower())
          gvItem.Checked = setSelected;
      }
      this.isDirty = true;
      this.OnDataChangedEvent(EventArgs.Empty);
    }

    private void lvwFolders_Click(object sender, EventArgs e)
    {
      this.isDirty = true;
      this.OnDataChangedEvent(EventArgs.Empty);
    }

    private void btnSelectAll_Click(object sender, EventArgs e) => this.selectFolders(true, true);

    private void btnSelectAllExcept_Click(object sender, EventArgs e)
    {
      this.selectFolders(true, false);
    }

    private void btnUnselectAll_Click(object sender, EventArgs e)
    {
      this.selectFolders(false, true);
    }

    public event SelectFolderControl.DataChangedEventHandler DataChangedEvent;

    protected virtual void OnDataChangedEvent(EventArgs e)
    {
      if (this.suspendEvent || this.DataChangedEvent == null)
        return;
      this.DataChangedEvent((object) this, e);
    }

    private void gvFolders_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.isDirty = true;
      this.OnDataChangedEvent(EventArgs.Empty);
    }

    private void folderOption_Changed(object sender, EventArgs e)
    {
      this.isDirty = true;
      this.OnDataChangedEvent(EventArgs.Empty);
      if (!this.radSelectedFolders.Checked)
        this.grpFolders.Visible = false;
      else
        this.grpFolders.Visible = true;
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
      this.btnSelectAll = new Button();
      this.btnSelectAllExcept = new Button();
      this.btnUnselectAll = new Button();
      this.grpFolders = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.deselectAllBtn = new Button();
      this.gvFolders = new GridView();
      this.panel1 = new Panel();
      this.radSelectedFolders = new RadioButton();
      this.label9 = new Label();
      this.radActiveFolders = new RadioButton();
      this.radAllFolders = new RadioButton();
      this.grpFolders.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.btnSelectAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSelectAll.BackColor = SystemColors.Control;
      this.btnSelectAll.Location = new Point(17, 0);
      this.btnSelectAll.Margin = new Padding(0);
      this.btnSelectAll.Name = "btnSelectAll";
      this.btnSelectAll.Size = new Size(76, 22);
      this.btnSelectAll.TabIndex = 35;
      this.btnSelectAll.Text = "&Select All";
      this.btnSelectAll.UseVisualStyleBackColor = true;
      this.btnSelectAll.Click += new EventHandler(this.btnSelectAll_Click);
      this.btnSelectAllExcept.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSelectAllExcept.BackColor = SystemColors.Control;
      this.btnSelectAllExcept.Location = new Point(93, 0);
      this.btnSelectAllExcept.Margin = new Padding(0);
      this.btnSelectAllExcept.Name = "btnSelectAllExcept";
      this.btnSelectAllExcept.Size = new Size(141, 22);
      this.btnSelectAllExcept.TabIndex = 36;
      this.btnSelectAllExcept.Text = "Select All &Except Archive";
      this.btnSelectAllExcept.UseVisualStyleBackColor = true;
      this.btnSelectAllExcept.Click += new EventHandler(this.btnSelectAllExcept_Click);
      this.btnUnselectAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUnselectAll.BackColor = SystemColors.Control;
      this.btnUnselectAll.Location = new Point(234, 0);
      this.btnUnselectAll.Margin = new Padding(0);
      this.btnUnselectAll.Name = "btnUnselectAll";
      this.btnUnselectAll.Size = new Size(83, 22);
      this.btnUnselectAll.TabIndex = 37;
      this.btnUnselectAll.Text = "&Deselect All";
      this.btnUnselectAll.UseVisualStyleBackColor = true;
      this.btnUnselectAll.Click += new EventHandler(this.btnUnselectAll_Click);
      this.grpFolders.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpFolders.Controls.Add((Control) this.flowLayoutPanel2);
      this.grpFolders.Controls.Add((Control) this.gvFolders);
      this.grpFolders.Dock = DockStyle.Fill;
      this.grpFolders.Location = new Point(0, 96);
      this.grpFolders.Name = "grpFolders";
      this.grpFolders.Size = new Size(455, 216);
      this.grpFolders.TabIndex = 38;
      this.grpFolders.Text = "Folders";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnUnselectAll);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSelectAllExcept);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSelectAll);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(131, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Padding = new Padding(0, 0, 5, 0);
      this.flowLayoutPanel1.Size = new Size(322, 22);
      this.flowLayoutPanel1.TabIndex = 23;
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.deselectAllBtn);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(412, 2);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(174, 22);
      this.flowLayoutPanel2.TabIndex = 22;
      this.deselectAllBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.deselectAllBtn.Location = new Point(89, 0);
      this.deselectAllBtn.Margin = new Padding(0);
      this.deselectAllBtn.Name = "deselectAllBtn";
      this.deselectAllBtn.Padding = new Padding(2, 0, 0, 0);
      this.deselectAllBtn.Size = new Size(85, 22);
      this.deselectAllBtn.TabIndex = 24;
      this.deselectAllBtn.Text = "Dese&lect All";
      this.gvFolders.AllowMultiselect = false;
      this.gvFolders.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Folder Name";
      gvColumn1.Width = 353;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Archive";
      gvColumn2.Width = 100;
      this.gvFolders.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvFolders.Dock = DockStyle.Fill;
      this.gvFolders.Location = new Point(1, 26);
      this.gvFolders.Name = "gvFolders";
      this.gvFolders.Selectable = false;
      this.gvFolders.Size = new Size(453, 189);
      this.gvFolders.TabIndex = 21;
      this.gvFolders.SubItemCheck += new GVSubItemEventHandler(this.gvFolders_SubItemCheck);
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Controls.Add((Control) this.radSelectedFolders);
      this.panel1.Controls.Add((Control) this.label9);
      this.panel1.Controls.Add((Control) this.radActiveFolders);
      this.panel1.Controls.Add((Control) this.radAllFolders);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(455, 96);
      this.panel1.TabIndex = 39;
      this.radSelectedFolders.AutoSize = true;
      this.radSelectedFolders.BackColor = Color.Transparent;
      this.radSelectedFolders.Location = new Point(31, 68);
      this.radSelectedFolders.Name = "radSelectedFolders";
      this.radSelectedFolders.Size = new Size(159, 17);
      this.radSelectedFolders.TabIndex = 29;
      this.radSelectedFolders.Text = "Select loan folders manually.";
      this.radSelectedFolders.UseVisualStyleBackColor = false;
      this.radSelectedFolders.CheckedChanged += new EventHandler(this.folderOption_Changed);
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.Transparent;
      this.label9.Location = new Point(12, 8);
      this.label9.Name = "label9";
      this.label9.Size = new Size(239, 13);
      this.label9.TabIndex = 26;
      this.label9.Text = "Select the loan folders to include in the snapshot.";
      this.radActiveFolders.AutoSize = true;
      this.radActiveFolders.BackColor = Color.Transparent;
      this.radActiveFolders.Location = new Point(31, 48);
      this.radActiveFolders.Name = "radActiveFolders";
      this.radActiveFolders.Size = new Size(204, 17);
      this.radActiveFolders.TabIndex = 28;
      this.radActiveFolders.Text = "All loan folders except Archive folders and Archive loans.";
      this.radActiveFolders.UseVisualStyleBackColor = false;
      this.radActiveFolders.CheckedChanged += new EventHandler(this.folderOption_Changed);
      this.radAllFolders.AutoSize = true;
      this.radAllFolders.BackColor = Color.Transparent;
      this.radAllFolders.Location = new Point(31, 28);
      this.radAllFolders.Name = "radAllFolders";
      this.radAllFolders.Size = new Size(96, 17);
      this.radAllFolders.TabIndex = 27;
      this.radAllFolders.Text = "All loan folders.";
      this.radAllFolders.UseVisualStyleBackColor = false;
      this.radAllFolders.CheckedChanged += new EventHandler(this.folderOption_Changed);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.grpFolders);
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (SelectFolderControl);
      this.Size = new Size(455, 312);
      this.grpFolders.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }

    public delegate void DataChangedEventHandler(object sender, EventArgs e);
  }
}
