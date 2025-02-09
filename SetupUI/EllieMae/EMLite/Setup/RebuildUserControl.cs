// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.RebuildUserControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
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
  public class RebuildUserControl : UserControl
  {
    private Button rebuildBtn;
    private IContainer components;
    private GroupContainer gcRebuildPipeline;
    private StandardIconButton stdIconBtnRefresh;
    private ToolTip toolTip1;
    private ContextMenuStrip contextMenuStripFolders;
    private ToolStripMenuItem selectAllToolStripMenuItem;
    private ToolStripMenuItem clearAllToolStripMenuItem;
    private ToolStripMenuItem rebuildAllToolStripMenuItem;
    private GridView gvFolder;
    private VerticalSeparator verticalSeparator1;
    private RadioButton rBtnInternal;
    private RadioButton rBtnExternal;
    private RadioButton rBtnBoth;
    private SetUpContainer setupContainer;

    public RebuildUserControl(SetUpContainer setupContainer)
    {
      this.InitializeComponent();
      this.setupContainer = setupContainer;
      this.refreshFolders();
      if ((bool) Session.ServerManager.GetServerSetting("Components.UseERDB"))
      {
        this.rBtnInternal.Visible = this.rBtnExternal.Visible = this.rBtnBoth.Visible = true;
        this.rBtnBoth.Checked = true;
      }
      else
      {
        this.rBtnInternal.Visible = this.rBtnExternal.Visible = this.rBtnBoth.Visible = false;
        this.rBtnInternal.Checked = true;
      }
    }

    private void refreshFolders()
    {
      this.gvFolder.Items.Clear();
      LoanFolderInfo[] allLoanFolderInfos = Session.LoanManager.GetAllLoanFolderInfos(true);
      foreach (object obj in allLoanFolderInfos)
        this.gvFolder.Items.Add(new GVItem(obj));
      this.gcRebuildPipeline.Text = "Loan Folders (" + (object) allLoanFolderInfos.Length + ")";
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
      GVColumn gvColumn = new GVColumn();
      this.gcRebuildPipeline = new GroupContainer();
      this.verticalSeparator1 = new VerticalSeparator();
      this.gvFolder = new GridView();
      this.contextMenuStripFolders = new ContextMenuStrip(this.components);
      this.rebuildAllToolStripMenuItem = new ToolStripMenuItem();
      this.selectAllToolStripMenuItem = new ToolStripMenuItem();
      this.clearAllToolStripMenuItem = new ToolStripMenuItem();
      this.stdIconBtnRefresh = new StandardIconButton();
      this.rebuildBtn = new Button();
      this.toolTip1 = new ToolTip(this.components);
      this.rBtnBoth = new RadioButton();
      this.rBtnExternal = new RadioButton();
      this.rBtnInternal = new RadioButton();
      this.gcRebuildPipeline.SuspendLayout();
      this.contextMenuStripFolders.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnRefresh).BeginInit();
      this.SuspendLayout();
      this.gcRebuildPipeline.Controls.Add((Control) this.rBtnInternal);
      this.gcRebuildPipeline.Controls.Add((Control) this.rBtnExternal);
      this.gcRebuildPipeline.Controls.Add((Control) this.rBtnBoth);
      this.gcRebuildPipeline.Controls.Add((Control) this.verticalSeparator1);
      this.gcRebuildPipeline.Controls.Add((Control) this.gvFolder);
      this.gcRebuildPipeline.Controls.Add((Control) this.stdIconBtnRefresh);
      this.gcRebuildPipeline.Controls.Add((Control) this.rebuildBtn);
      this.gcRebuildPipeline.Dock = DockStyle.Fill;
      this.gcRebuildPipeline.HeaderForeColor = SystemColors.ControlText;
      this.gcRebuildPipeline.Location = new Point(0, 0);
      this.gcRebuildPipeline.Name = "gcRebuildPipeline";
      this.gcRebuildPipeline.Size = new Size(560, 364);
      this.gcRebuildPipeline.TabIndex = 6;
      this.gcRebuildPipeline.Text = "Loan Folders";
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(448, 6);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 4;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.gvFolder.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "colFolder";
      gvColumn.Text = "FolderName";
      gvColumn.Width = 500;
      this.gvFolder.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvFolder.ContextMenuStrip = this.contextMenuStripFolders;
      this.gvFolder.Dock = DockStyle.Fill;
      this.gvFolder.HeaderHeight = 0;
      this.gvFolder.HeaderVisible = false;
      this.gvFolder.Location = new Point(1, 26);
      this.gvFolder.Name = "gvFolder";
      this.gvFolder.Size = new Size(558, 337);
      this.gvFolder.TabIndex = 3;
      this.gvFolder.SizeChanged += new EventHandler(this.gvFolder_SizeChanged);
      this.contextMenuStripFolders.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.rebuildAllToolStripMenuItem,
        (ToolStripItem) this.selectAllToolStripMenuItem,
        (ToolStripItem) this.clearAllToolStripMenuItem
      });
      this.contextMenuStripFolders.Name = "contextMenuStripFolders";
      this.contextMenuStripFolders.Size = new Size(124, 70);
      this.rebuildAllToolStripMenuItem.Name = "rebuildAllToolStripMenuItem";
      this.rebuildAllToolStripMenuItem.Size = new Size(123, 22);
      this.rebuildAllToolStripMenuItem.Text = "Rebuild All";
      this.rebuildAllToolStripMenuItem.Click += new EventHandler(this.rebuildBtn_Click);
      this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
      this.selectAllToolStripMenuItem.Size = new Size(123, 22);
      this.selectAllToolStripMenuItem.Text = "Select All";
      this.selectAllToolStripMenuItem.Click += new EventHandler(this.selectAllBtn_Click);
      this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
      this.clearAllToolStripMenuItem.Size = new Size(123, 22);
      this.clearAllToolStripMenuItem.Text = "Clear All";
      this.clearAllToolStripMenuItem.Click += new EventHandler(this.clearAllBtn_Click);
      this.stdIconBtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnRefresh.BackColor = Color.Transparent;
      this.stdIconBtnRefresh.Location = new Point(426, 6);
      this.stdIconBtnRefresh.Name = "stdIconBtnRefresh";
      this.stdIconBtnRefresh.Size = new Size(16, 16);
      this.stdIconBtnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.stdIconBtnRefresh.TabIndex = 2;
      this.stdIconBtnRefresh.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnRefresh, "Refresh");
      this.stdIconBtnRefresh.Click += new EventHandler(this.refreshBtn_Click);
      this.rebuildBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.rebuildBtn.BackColor = SystemColors.Control;
      this.rebuildBtn.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rebuildBtn.Location = new Point(453, 2);
      this.rebuildBtn.Name = "rebuildBtn";
      this.rebuildBtn.Size = new Size(103, 22);
      this.rebuildBtn.TabIndex = 1;
      this.rebuildBtn.Text = "Rebuild Folder(s)";
      this.rebuildBtn.UseVisualStyleBackColor = true;
      this.rebuildBtn.Click += new EventHandler(this.rebuildBtn_Click);
      this.rBtnBoth.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.rBtnBoth.AutoSize = true;
      this.rBtnBoth.BackColor = Color.Transparent;
      this.rBtnBoth.Location = new Point(373, 4);
      this.rBtnBoth.Name = "rBtnBoth";
      this.rBtnBoth.Size = new Size(47, 17);
      this.rBtnBoth.TabIndex = 5;
      this.rBtnBoth.Text = "Both";
      this.rBtnBoth.UseVisualStyleBackColor = false;
      this.rBtnExternal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.rBtnExternal.AutoSize = true;
      this.rBtnExternal.BackColor = Color.Transparent;
      this.rBtnExternal.Location = new Point((int) byte.MaxValue, 4);
      this.rBtnExternal.Name = "rBtnExternal";
      this.rBtnExternal.Size = new Size(112, 17);
      this.rBtnExternal.TabIndex = 6;
      this.rBtnExternal.Text = "External Database";
      this.rBtnExternal.UseVisualStyleBackColor = false;
      this.rBtnInternal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.rBtnInternal.AutoSize = true;
      this.rBtnInternal.BackColor = Color.Transparent;
      this.rBtnInternal.Checked = true;
      this.rBtnInternal.Location = new Point(140, 4);
      this.rBtnInternal.Name = "rBtnInternal";
      this.rBtnInternal.Size = new Size(109, 17);
      this.rBtnInternal.TabIndex = 7;
      this.rBtnInternal.TabStop = true;
      this.rBtnInternal.Text = "Internal Database";
      this.rBtnInternal.UseVisualStyleBackColor = false;
      this.Controls.Add((Control) this.gcRebuildPipeline);
      this.Name = nameof (RebuildUserControl);
      this.Size = new Size(560, 364);
      this.gcRebuildPipeline.ResumeLayout(false);
      this.gcRebuildPipeline.PerformLayout();
      this.contextMenuStripFolders.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnRefresh).EndInit();
      this.ResumeLayout(false);
    }

    private string[] getSelectedFolderNames()
    {
      int count = this.gvFolder.SelectedItems.Count;
      if (count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one folder.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return (string[]) null;
      }
      string[] selectedFolderNames = new string[count];
      for (int index = 0; index < count; ++index)
        selectedFolderNames[index] = ((LoanFolderInfo) this.gvFolder.SelectedItems[index].Value).Name;
      return selectedFolderNames;
    }

    private void rebuildBtn_Click(object sender, EventArgs e)
    {
      object state = (object) null;
      if (sender == this.rebuildBtn)
      {
        state = (object) this.getSelectedFolderNames();
        if (state == null)
          return;
      }
      using (ProgressDialog progressDialog = new ProgressDialog("Rebuild Pipeline", new AsynchronousProcess(this.rebuildPipeline), state, true))
        progressDialog.ShowDialog();
    }

    private DialogResult rebuildPipeline(object state, IProgressFeedback feedback)
    {
      try
      {
        DatabaseToRebuild dbToRebuild = DatabaseToRebuild.Both;
        if (this.rBtnInternal.Checked)
          dbToRebuild = DatabaseToRebuild.Internal;
        else if (this.rBtnExternal.Checked)
          dbToRebuild = DatabaseToRebuild.External;
        if (state is Array)
          Session.LoanManager.RebuildPipeline((string[]) state, (IServerProgressFeedback) feedback, dbToRebuild);
        else
          Session.LoanManager.RebuildPipeline((IServerProgressFeedback) feedback, dbToRebuild);
        return feedback.Cancel ? DialogResult.Cancel : DialogResult.OK;
      }
      catch
      {
        return DialogResult.Abort;
      }
    }

    private void selectAllBtn_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = 0; nItemIndex < this.gvFolder.Items.Count; ++nItemIndex)
        this.gvFolder.Items[nItemIndex].Selected = true;
    }

    private void clearAllBtn_Click(object sender, EventArgs e)
    {
      this.gvFolder.SelectedItems.Clear();
    }

    private void refreshBtn_Click(object sender, EventArgs e)
    {
      this.refreshFolders();
      SaveConfirmScreen.Show((IWin32Window) this.setupContainer, "Data refreshed.");
    }

    private void gvFolder_SizeChanged(object sender, EventArgs e)
    {
      this.gvFolder.Columns[0].Width = this.gvFolder.Width - 5;
    }
  }
}
