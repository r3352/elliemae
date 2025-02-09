// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LoanActionLogControl
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class LoanActionLogControl : UserControl
  {
    private IContainer components;
    private GroupContainer gpcLoanActionLog;
    private StandardIconButton stdIconNew;
    private GridView lvwLogs;

    public LoanActionLogControl(Sessions.Session session, LoanData loan)
    {
      this.InitializeComponent();
      this.populateLoanActionLogs();
    }

    private void populateLoanActionLogs()
    {
      this.lvwLogs.Items.Clear();
      foreach (LogRecordBase allDatedRecord in Session.LoanData.GetLogList().GetAllDatedRecords())
      {
        if (allDatedRecord is LoanActionLog)
        {
          LoanActionLog loanActionLog = (LoanActionLog) allDatedRecord;
          this.lvwLogs.Items.Add(new GVItem()
          {
            SubItems = {
              [0] = {
                Value = (object) loanActionLog.Date
              },
              [1] = {
                Value = (object) loanActionLog.LoanActionType
              },
              [2] = {
                Value = (object) loanActionLog.TriggeredBy
              }
            },
            Tag = (object) allDatedRecord
          });
        }
      }
      this.refreshRecordCount();
    }

    private void refreshRecordCount()
    {
      this.gpcLoanActionLog.Text = "Loan Action Log (" + (object) this.lvwLogs.Items.Count + ")";
    }

    private void stdIconNew_Click(object sender, EventArgs e)
    {
      using (LoanActionLogDialog loanActionLogDialog = new LoanActionLogDialog())
      {
        int num = (int) loanActionLogDialog.ShowDialog();
        if (loanActionLogDialog.DialogResult != DialogResult.OK)
          return;
        this.populateLoanActionLogs();
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
      this.gpcLoanActionLog = new GroupContainer();
      this.stdIconNew = new StandardIconButton();
      this.lvwLogs = new GridView();
      this.gpcLoanActionLog.SuspendLayout();
      ((ISupportInitialize) this.stdIconNew).BeginInit();
      this.SuspendLayout();
      this.gpcLoanActionLog.BackColor = Color.White;
      this.gpcLoanActionLog.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gpcLoanActionLog.Controls.Add((Control) this.stdIconNew);
      this.gpcLoanActionLog.Controls.Add((Control) this.lvwLogs);
      this.gpcLoanActionLog.Dock = DockStyle.Fill;
      this.gpcLoanActionLog.HeaderForeColor = SystemColors.ControlText;
      this.gpcLoanActionLog.Location = new Point(0, 0);
      this.gpcLoanActionLog.Name = "gpcLoanActionLog";
      this.gpcLoanActionLog.Size = new Size(475, 378);
      this.gpcLoanActionLog.TabIndex = 3;
      this.gpcLoanActionLog.Text = "Loan Action Log";
      this.stdIconNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconNew.BackColor = Color.Transparent;
      this.stdIconNew.Location = new Point(448, 4);
      this.stdIconNew.MouseDownImage = (Image) null;
      this.stdIconNew.Name = "stdIconNew";
      this.stdIconNew.Size = new Size(16, 16);
      this.stdIconNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconNew.TabIndex = 45;
      this.stdIconNew.TabStop = false;
      this.stdIconNew.Click += new EventHandler(this.stdIconNew_Click);
      this.lvwLogs.AllowMultiselect = false;
      this.lvwLogs.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "gvDate";
      gvColumn1.Text = "Date";
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "gvActionType";
      gvColumn2.Text = "Loan Action Type";
      gvColumn2.Width = 180;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "gvBy";
      gvColumn3.Text = "Performed By";
      gvColumn3.Width = 120;
      this.lvwLogs.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.lvwLogs.Dock = DockStyle.Fill;
      this.lvwLogs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.lvwLogs.Location = new Point(1, 25);
      this.lvwLogs.Name = "lvwLogs";
      this.lvwLogs.Size = new Size(473, 352);
      this.lvwLogs.SortIconVisible = false;
      this.lvwLogs.SortOption = GVSortOption.None;
      this.lvwLogs.TabIndex = 43;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gpcLoanActionLog);
      this.Name = nameof (LoanActionLogControl);
      this.Size = new Size(475, 378);
      this.gpcLoanActionLog.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconNew).EndInit();
      this.ResumeLayout(false);
    }
  }
}
