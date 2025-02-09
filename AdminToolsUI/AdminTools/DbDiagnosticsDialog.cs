// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.DbDiagnosticsDialog
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using Elli.Server.Remoting;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class DbDiagnosticsDialog : Form
  {
    private IContainer components;
    private Button btnClose;
    private GroupContainer groupContainer1;
    private GridView gvStats;
    private GroupContainer groupContainer2;
    private GridView gvHealth;
    private StandardIconButton btnRefresh;

    public DbDiagnosticsDialog() => this.InitializeComponent();

    private void DbDiagnosticsDialog_Shown(object sender, EventArgs e)
    {
      this.startDiagnostics(false);
    }

    private void startDiagnostics(bool healthOnly)
    {
      this.gvHealth.Items.Clear();
      if (!healthOnly)
        this.gvStats.Items.Clear();
      using (ProgressDialog progressDialog = new ProgressDialog("Encompass Database Diagnostics", new AsynchronousProcess(this.execDiagnostics), (object) healthOnly, true))
      {
        int num = (int) progressDialog.ShowDialog((IWin32Window) this);
      }
    }

    private DialogResult execDiagnostics(object healthOnlyObj, IProgressFeedback feedback)
    {
      try
      {
        bool flag = (bool) healthOnlyObj;
        int num1 = 0;
        feedback.ResetCounter(flag ? 4 : 6);
        feedback.SetFeedback("Gathering database statistics...", "Reading basic usage statistics...", 0);
        using (InProcConnection inProcConnection = new InProcConnection())
        {
          inProcConnection.OpenTrusted();
          IServerManager serverManager = (IServerManager) inProcConnection.Session.GetObject("ServerManager");
          if (!flag)
          {
            this.populateUsageInfo((object) serverManager.GetDbUsageInfo());
            int num2;
            if (!feedback.SetFeedback((string) null, "Reading database sizing details...", num2 = num1 + 1))
              return DialogResult.Cancel;
            this.populateSizeInfo((object) serverManager.GetDbSize());
            if (!feedback.SetFeedback((string) null, "Checking for consistency errors...", num1 = num2 + 1))
              return DialogResult.Cancel;
          }
          this.populateConsistencyInfo((object) serverManager.GetDbConsistencyErrorCount());
          int num3 = num1 + 2;
          if (!feedback.SetFeedback((string) null, "Analyzing logical data fragmentation...", num3))
            return DialogResult.Cancel;
          this.populateFragmentationInfo((object) serverManager.GetDbFragmentationLevel());
          feedback.Increment(2);
        }
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error running diagnostics: " + ex.Message);
        return DialogResult.Abort;
      }
    }

    private void populateUsageInfo(object usageInfoObj)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new WaitCallback(this.populateUsageInfo), usageInfoObj);
      }
      else
      {
        DbUsageInfo dbUsageInfo = (DbUsageInfo) usageInfoObj;
        this.addStatistic("# of Loans", (object) dbUsageInfo.LoanCount.ToString("#,##0"));
        this.addStatistic("# of Alerts", (object) dbUsageInfo.AlertCount.ToString("#,##0"));
        this.addStatistic("# of Borrower Contacts", (object) dbUsageInfo.BorrowerCount.ToString("#,##0"));
        this.addStatistic("# of Business Contacts", (object) dbUsageInfo.BizContactCount.ToString("#,##0"));
        this.addStatistic("# of Users", (object) dbUsageInfo.UserCount.ToString("#,##0"));
        this.addStatistic("# of Organizations", (object) dbUsageInfo.OrgCount.ToString("#,##0"));
        this.addStatistic("Max Organization Depth", (object) dbUsageInfo.OrgMaxDepth.ToString("#,##0"));
        this.addStatistic("# of Personas", (object) dbUsageInfo.PersonaCount.ToString("#,##0"));
        this.addStatistic("# of User Groups", (object) dbUsageInfo.GroupCount.ToString("#,##0"));
      }
    }

    private void populateSizeInfo(object sizeObj)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new WaitCallback(this.populateSizeInfo), sizeObj);
      }
      else
      {
        DbSize dbSize = (DbSize) sizeObj;
        this.addStatistic("Physical DB Size (KB)", (object) dbSize.PhysicalSize.ToString("#,##0"));
        this.addStatistic("Reserved Data Size (KB)", (object) dbSize.LogicalSize.ToString("#,##0"));
        this.addStatistic("Actual Data Size (KB)", (object) dbSize.DataSize.ToString("#,##0"));
        this.addStatistic("Actual Index Size (KB)", (object) dbSize.IndexSize.ToString("#,##0"));
      }
    }

    private void populateConsistencyInfo(object errCountObj)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new WaitCallback(this.populateConsistencyInfo), errCountObj);
      }
      else
      {
        int num = (int) errCountObj;
        GVItem gvItem = new GVItem("Database Consistency Errors");
        if (num == 0)
        {
          gvItem.SubItems[1].Value = (object) new Hyperlink("More info", new EventHandler(this.onNoConsistencyErrorClick));
        }
        else
        {
          gvItem.SubItems[2].ForeColor = Color.Red;
          gvItem.SubItems[1].Value = (object) new Hyperlink("More info", new EventHandler(this.onConsistencyErrorClick));
        }
        gvItem.SubItems[2].Value = (object) num;
        this.gvHealth.Items.Add(gvItem);
      }
    }

    private void populateFragmentationInfo(object fragLevelObj)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new WaitCallback(this.populateFragmentationInfo), fragLevelObj);
      }
      else
      {
        int num = (int) fragLevelObj;
        GVItem gvItem = new GVItem("Database Fragmentation");
        gvItem.SubItems[1].Value = (object) new Hyperlink("Defragment", new EventHandler(this.onDefragmentClick));
        gvItem.SubItems[2].Value = num >= 10 ? (num >= 25 ? (num >= 50 ? (num >= 75 ? (num >= 90 ? (object) (fragLevelObj.ToString() + "% (Extreme)") : (object) (fragLevelObj.ToString() + "% (Very High)")) : (object) (fragLevelObj.ToString() + "% (High)")) : (object) (fragLevelObj.ToString() + "% (Normal)")) : (object) (fragLevelObj.ToString() + "% (Low)")) : (object) (fragLevelObj.ToString() + "% (Very Low)");
        if (num >= 75)
          gvItem.SubItems[2].ForeColor = Color.Red;
        this.gvHealth.Items.Add(gvItem);
      }
    }

    private void addStatistic(string text, object value)
    {
      this.gvStats.Items.Add(new GVItem(text)
      {
        SubItems = {
          [1] = {
            Value = value
          }
        }
      });
    }

    private void onConsistencyErrorClick(object sender, EventArgs e)
    {
      int num = (int) Utils.Dialog((IWin32Window) this, "The database consistency check indicated that one or more consistency errors is present in your database. Consistency errors can result in data corruption or data loss and must be addressed immediately." + Environment.NewLine + Environment.NewLine + "Contact ICE Mortgage Technology Customer Support for assistance at 800-777-1718.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void onNoConsistencyErrorClick(object sender, EventArgs e)
    {
      int num = (int) Utils.Dialog((IWin32Window) this, "The database consistency check found no problems in your database. You should perform this check periodically to ensure errors are detected and corrected as soon as possible.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void onDefragmentClick(object sender, EventArgs e) => this.defragmentDatabase();

    private bool defragmentDatabase()
    {
      if (Utils.Dialog((IWin32Window) this, "Defragmenting the Encompass database can take an hour or more on a large, highly fragmented database. During that time, database performance will be severely degraded and users logging into Encompass may find the system difficult to use." + Environment.NewLine + Environment.NewLine + "Are you sure you want to proceed with the defragmentation process?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return false;
      this.startDefragmentation();
      this.startDiagnostics(true);
      return true;
    }

    private void btnRefresh_Click(object sender, EventArgs e) => this.startDiagnostics(true);

    private void startDefragmentation()
    {
      using (ProgressDialog progressDialog = new ProgressDialog("Encompass Database Diagnostics", new AsynchronousProcess(this.execDefragmentation), (object) null, true))
      {
        int num = (int) progressDialog.ShowDialog((IWin32Window) this);
      }
    }

    private DialogResult execDefragmentation(object state, IProgressFeedback feedback)
    {
      try
      {
        feedback.Status = "Defragmenting database...";
        using (InProcConnection inProcConnection = new InProcConnection())
        {
          inProcConnection.OpenTrusted();
          ((IServerManager) inProcConnection.Session.GetObject("ServerManager")).DefragmentDatabase((IServerProgressFeedback) feedback);
        }
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error running diagnostics: " + ex.Message);
        return DialogResult.Abort;
      }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DbDiagnosticsDialog));
      this.btnClose = new Button();
      this.groupContainer2 = new GroupContainer();
      this.btnRefresh = new StandardIconButton();
      this.gvHealth = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.gvStats = new GridView();
      this.groupContainer2.SuspendLayout();
      ((ISupportInitialize) this.btnRefresh).BeginInit();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(382, 367);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.groupContainer2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.btnRefresh);
      this.groupContainer2.Controls.Add((Control) this.gvHealth);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(10, 277);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(446, 83);
      this.groupContainer2.TabIndex = 3;
      this.groupContainer2.Text = "Database Health";
      this.btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRefresh.BackColor = Color.Transparent;
      this.btnRefresh.Location = new Point(424, 4);
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new Size(16, 16);
      this.btnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.btnRefresh.TabIndex = 1;
      this.btnRefresh.TabStop = false;
      this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
      this.gvHealth.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Column";
      gvColumn1.Width = 244;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Column";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.Numeric;
      gvColumn3.Text = "Column";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 100;
      this.gvHealth.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvHealth.Dock = DockStyle.Fill;
      this.gvHealth.HeaderHeight = 0;
      this.gvHealth.HeaderVisible = false;
      this.gvHealth.Location = new Point(1, 26);
      this.gvHealth.Name = "gvHealth";
      this.gvHealth.Selectable = false;
      this.gvHealth.Size = new Size(444, 56);
      this.gvHealth.TabIndex = 0;
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.gvStats);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(10, 10);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(446, 262);
      this.groupContainer1.TabIndex = 2;
      this.groupContainer1.Text = "Database Statistics";
      this.gvStats.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Column";
      gvColumn4.Width = 344;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.SortMethod = GVSortMethod.Numeric;
      gvColumn5.Text = "Column";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 100;
      this.gvStats.Columns.AddRange(new GVColumn[2]
      {
        gvColumn4,
        gvColumn5
      });
      this.gvStats.Dock = DockStyle.Fill;
      this.gvStats.HeaderHeight = 0;
      this.gvStats.HeaderVisible = false;
      this.gvStats.Location = new Point(1, 26);
      this.gvStats.Name = "gvStats";
      this.gvStats.Selectable = false;
      this.gvStats.Size = new Size(444, 235);
      this.gvStats.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(466, 398);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.btnClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (DbDiagnosticsDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass Database Diagnostics";
      this.Shown += new EventHandler(this.DbDiagnosticsDialog_Shown);
      this.groupContainer2.ResumeLayout(false);
      ((ISupportInitialize) this.btnRefresh).EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
