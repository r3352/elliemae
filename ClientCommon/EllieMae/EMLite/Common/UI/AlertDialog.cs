// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.AlertDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class AlertDialog : Form
  {
    private string guid = "";
    private PipelineInfo.Alert[] alertList;
    private GVItem divItem = new GVItem("-----Dismissed or Snoozed Alerts-----");
    private PipelineInfo pipelineInfo;
    private bool editEnabled = true;
    private int filterAlertId = -1;
    private bool allowClearComplianceAlerts;
    private IContainer components;
    private Label label1;
    private GridView gvAlerts;
    private Button btnDismiss;
    private Button btnSnooze;
    private Label label2;
    private ComboBox cmbSnoozeDuration;
    private ContextMenuStrip mnuContext;
    private ToolStripMenuItem dismissAlertToolStripMenuItem;
    private ToolStripMenuItem snoozeToolStripMenuItem;
    private ToolStripMenuItem reactivateToolStripMenuItem;
    private Button btnReactivate;

    public AlertDialog(PipelineInfo pipelineInfo)
      : this(pipelineInfo, -1)
    {
    }

    public AlertDialog(PipelineInfo pipelineInfo, int filterAlertId)
    {
      this.InitializeComponent();
      this.pipelineInfo = pipelineInfo;
      this.filterAlertId = filterAlertId;
      using (CursorActivator.Wait())
      {
        if (pipelineInfo.Alerts == null)
        {
          PipelineInfo[] pipeline = Session.LoanManager.GetPipeline(new string[1]
          {
            pipelineInfo.GUID
          }, (string[]) null, PipelineData.Alerts | PipelineData.Milestones | PipelineData.LoanAssociates, false);
          if (pipeline.Length != 0)
            this.pipelineInfo = pipeline[0];
        }
        if (this.pipelineInfo.Alerts != null)
        {
          AlertSetupData alertSetupData = Session.LoanManager.GetAlertSetupData();
          this.pipelineInfo.UpdateAlerts(Session.UserInfo, Session.SessionObjects.AclGroupManager.GetGroupsOfUser(Session.UserID), alertSetupData);
        }
        this.guid = this.pipelineInfo.GUID;
        this.alertList = this.pipelineInfo.Alerts;
        if (this.alertList == null)
          this.alertList = new PipelineInfo.Alert[0];
        this.initialPageValue();
        this.enforceSecurity();
      }
    }

    private void enforceSecurity()
    {
      if (Session.UserInfo.IsAdministrator())
        return;
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      PipelineInfo.LoanAssociateInfo currentLoanAssociate = this.pipelineInfo.GetCurrentLoanAssociate();
      if (!aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_Pipeline_Alert))
        this.disableAlertActions();
      else if (currentLoanAssociate == null)
        this.disableAlertActions();
      else if (currentLoanAssociate.AssociateType == LoanAssociateType.User && currentLoanAssociate.UserID != Session.UserID)
      {
        this.disableAlertActions();
      }
      else
      {
        if (currentLoanAssociate.AssociateType != LoanAssociateType.Group || this.isUserInGroup(currentLoanAssociate.GroupID))
          return;
        this.disableAlertActions();
      }
    }

    private void disableAlertActions()
    {
      this.btnDismiss.Enabled = false;
      this.btnReactivate.Enabled = false;
      this.btnSnooze.Enabled = false;
      this.editEnabled = false;
    }

    private bool isUserInGroup(int groupId) => Session.User.IsMemberOfAclGroup(groupId);

    private void initialPageValue()
    {
      this.gvAlerts.Items.Clear();
      List<PipelineInfo.Alert> alertList1 = new List<PipelineInfo.Alert>();
      List<PipelineInfo.Alert> alertList2 = new List<PipelineInfo.Alert>();
      for (int index = 0; index < this.alertList.Length; ++index)
      {
        PipelineInfo.Alert alert = this.updateStatus(this.alertList[index]);
        if (this.filterAlertId == -1 || alert.AlertID == this.filterAlertId)
        {
          if (alert.DisplayStatus != 1)
            alertList2.Add(alert);
          else
            alertList1.Add(alert);
        }
      }
      alertList1.Sort(new Comparison<PipelineInfo.Alert>(this.sortItemsByAlertDate));
      alertList2.Sort(new Comparison<PipelineInfo.Alert>(this.sortItemsByAlertDate));
      foreach (PipelineInfo.Alert alert in alertList1)
        this.gvAlerts.Items.Add(new GVItem(alert.AlertMessage)
        {
          SubItems = {
            (object) alert.Date.ToString("M/d/yy")
          },
          Tag = (object) alert
        });
      if (alertList2.Count > 0)
      {
        this.gvAlerts.Items.Add(this.divItem);
        foreach (PipelineInfo.Alert alert in alertList2)
          this.gvAlerts.Items.Add(new GVItem(this.getInactiveAlertMessage(alert))
          {
            SubItems = {
              (object) alert.Date.ToString("M/d/yy")
            },
            Tag = (object) alert
          });
      }
      this.Text = alertList1.Count.ToString() + " Loan Alerts";
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAlerts.Items)
      {
        if (gvItem.Tag is PipelineInfo.Alert tag && tag.AlertMessage.StartsWith("Compliance Review"))
        {
          ILoanServices service = Session.Application.GetService<ILoanServices>();
          if (service == null)
            break;
          this.allowClearComplianceAlerts = service.AllowClearComplianceAlerts();
          break;
        }
      }
    }

    private int sortItemsByAlertDate(PipelineInfo.Alert alertA, PipelineInfo.Alert alertB)
    {
      return alertA.Date == alertB.Date ? string.Compare(alertA.AlertMessage, alertB.AlertMessage) : alertA.Date.CompareTo(alertB.Date);
    }

    private string getInactiveAlertMessage(PipelineInfo.Alert alert)
    {
      string inactiveAlertMessage;
      if (alert.DisplayStatus == 3)
      {
        inactiveAlertMessage = alert.AlertMessage + " (Dismissed)";
      }
      else
      {
        string str = alert.AlertMessage + " (Snoozed for ";
        switch (alert.SnoozeDuration)
        {
          case 10:
            str += "10 minutes";
            break;
          case 60:
            str += "1 hour";
            break;
          case 240:
            str += "4 hours";
            break;
          case 1440:
            str += "1 day";
            break;
          case 2880:
            str += "2 days";
            break;
          case 4320:
            str += "3 days";
            break;
          case 5760:
            str += "4 days";
            break;
          case 10080:
            str += "1 week";
            break;
          case 20160:
            str += "2 weeks";
            break;
          case 43200:
            str += "1 month";
            break;
        }
        inactiveAlertMessage = str + " from " + (object) alert.SnoozeStartDTTM + ")";
      }
      return inactiveAlertMessage;
    }

    private PipelineInfo.Alert updateStatus(PipelineInfo.Alert alert)
    {
      if (alert.DisplayStatus == 2 && alert.SnoozeDuration != 0)
      {
        DateTime dateTime = alert.SnoozeStartDTTM;
        dateTime = dateTime.AddMinutes((double) alert.SnoozeDuration);
        if (dateTime < DateTime.Now)
        {
          alert.DisplayStatus = 1;
          alert.SnoozeDuration = 0;
          alert.SnoozeStartDTTM = DateTime.MinValue;
          Session.LoanManager.UpdateLoanAlerts(this.guid, new PipelineInfo.Alert[1]
          {
            alert
          });
        }
      }
      return alert;
    }

    private void btnDismiss_Click(object sender, EventArgs e)
    {
      if (this.gvAlerts.SelectedItems == null || this.gvAlerts.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
        foreach (GVItem selectedItem in this.gvAlerts.SelectedItems)
        {
          if (selectedItem.Tag is PipelineInfo.Alert tag)
          {
            tag.DisplayStatus = 3;
            tag.SnoozeStartDTTM = DateTime.MinValue;
            tag.SnoozeDuration = 0;
            alertList.Add(tag);
          }
        }
        Session.LoanManager.UpdateLoanAlerts(this.guid, alertList.ToArray());
        this.updateAlertListState(alertList.ToArray());
        this.initialPageValue();
      }
    }

    private void updateAlertListState(PipelineInfo.Alert[] updatedAlerts)
    {
      foreach (PipelineInfo.Alert updatedAlert in updatedAlerts)
      {
        for (int index = 0; index < this.alertList.Length; ++index)
        {
          if (updatedAlert.LoanAlertID != null && updatedAlert.LoanAlertID != "" && this.alertList[index].LoanAlertID != "" && this.alertList[index].LoanAlertID != null && updatedAlert.LoanAlertID == this.alertList[index].LoanAlertID)
          {
            this.alertList[index] = updatedAlert;
            break;
          }
          if (updatedAlert.AlertID == this.alertList[index].AlertID && updatedAlert.AlertTargetID == this.alertList[index].AlertTargetID)
          {
            this.alertList[index] = updatedAlert;
            break;
          }
        }
      }
    }

    private void btnSnooze_Click(object sender, EventArgs e)
    {
      if (this.gvAlerts.SelectedItems == null || this.gvAlerts.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select an alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.cmbSnoozeDuration.Text == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select a snooze duration time", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
        foreach (GVItem selectedItem in this.gvAlerts.SelectedItems)
        {
          if (selectedItem.Tag is PipelineInfo.Alert tag)
          {
            tag.DisplayStatus = 2;
            tag.SnoozeStartDTTM = DateTime.Now;
            tag.SnoozeDuration = this.getDuration(this.cmbSnoozeDuration.Text);
            alertList.Add(tag);
          }
        }
        Session.LoanManager.UpdateLoanAlerts(this.guid, alertList.ToArray());
        this.updateAlertListState(alertList.ToArray());
        this.initialPageValue();
      }
    }

    private int getDuration(string timePeriod) => PipelineInfo.Alert.GetDuration(timePeriod);

    private void gvAlerts_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      foreach (GVItem selectedItem in this.gvAlerts.SelectedItems)
      {
        if (selectedItem.Tag is PipelineInfo.Alert tag)
        {
          if (tag.DisplayStatus == 3)
            flag2 = true;
          else if (tag.DisplayStatus == 1)
            flag1 = true;
          flag3 = true;
          if (tag.AlertMessage.StartsWith("Compliance Review"))
            flag4 = true;
        }
      }
      this.btnDismiss.Enabled = this.editEnabled & flag3 && !flag2 && (!flag4 || this.allowClearComplianceAlerts);
      this.btnReactivate.Enabled = this.editEnabled & flag3 && !flag1 && (!flag4 || this.allowClearComplianceAlerts);
      this.btnSnooze.Enabled = this.editEnabled & flag3 && (!flag4 || this.allowClearComplianceAlerts);
    }

    private void mnuContext_Opening(object sender, CancelEventArgs e)
    {
      if (!this.btnSnooze.Enabled)
      {
        e.Cancel = true;
      }
      else
      {
        this.dismissAlertToolStripMenuItem.Enabled = this.btnDismiss.Enabled;
        this.reactivateToolStripMenuItem.Enabled = this.btnReactivate.Enabled;
      }
    }

    private void dismissAlertToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.btnDismiss_Click((object) null, (EventArgs) null);
    }

    private void snoozeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.btnSnooze_Click((object) null, (EventArgs) null);
    }

    private void reactivateToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.gvAlerts.SelectedItems == null || this.gvAlerts.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
        foreach (GVItem selectedItem in this.gvAlerts.SelectedItems)
        {
          if (selectedItem.Tag is PipelineInfo.Alert tag)
          {
            tag.DisplayStatus = 1;
            tag.SnoozeStartDTTM = DateTime.MinValue;
            tag.SnoozeDuration = 0;
            alertList.Add(tag);
          }
        }
        Session.LoanManager.UpdateLoanAlerts(this.guid, alertList.ToArray());
        this.updateAlertListState(alertList.ToArray());
        this.initialPageValue();
      }
    }

    private void AlertDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.Close();
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.label1 = new Label();
      this.gvAlerts = new GridView();
      this.mnuContext = new ContextMenuStrip(this.components);
      this.dismissAlertToolStripMenuItem = new ToolStripMenuItem();
      this.snoozeToolStripMenuItem = new ToolStripMenuItem();
      this.reactivateToolStripMenuItem = new ToolStripMenuItem();
      this.btnDismiss = new Button();
      this.btnSnooze = new Button();
      this.label2 = new Label();
      this.cmbSnoozeDuration = new ComboBox();
      this.btnReactivate = new Button();
      this.mnuContext.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(294, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select loan alerts to clear or to snooze to be reminded later.";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Alert";
      gvColumn1.Width = 481;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Date Expected";
      gvColumn2.Width = 88;
      this.gvAlerts.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvAlerts.ContextMenuStrip = this.mnuContext;
      this.gvAlerts.Location = new Point(10, 27);
      this.gvAlerts.Name = "gvAlerts";
      this.gvAlerts.Size = new Size(571, 240);
      this.gvAlerts.SortOption = GVSortOption.None;
      this.gvAlerts.TabIndex = 1;
      this.gvAlerts.SelectedIndexChanged += new EventHandler(this.gvAlerts_SelectedIndexChanged);
      this.mnuContext.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.dismissAlertToolStripMenuItem,
        (ToolStripItem) this.snoozeToolStripMenuItem,
        (ToolStripItem) this.reactivateToolStripMenuItem
      });
      this.mnuContext.Name = "contextMenuStrip1";
      this.mnuContext.ShowImageMargin = false;
      this.mnuContext.Size = new Size(121, 70);
      this.mnuContext.Opening += new CancelEventHandler(this.mnuContext_Opening);
      this.dismissAlertToolStripMenuItem.Name = "dismissAlertToolStripMenuItem";
      this.dismissAlertToolStripMenuItem.Size = new Size(120, 22);
      this.dismissAlertToolStripMenuItem.Text = "Dismiss Alert";
      this.dismissAlertToolStripMenuItem.Click += new EventHandler(this.dismissAlertToolStripMenuItem_Click);
      this.snoozeToolStripMenuItem.Name = "snoozeToolStripMenuItem";
      this.snoozeToolStripMenuItem.Size = new Size(120, 22);
      this.snoozeToolStripMenuItem.Text = "Snooze";
      this.snoozeToolStripMenuItem.Click += new EventHandler(this.snoozeToolStripMenuItem_Click);
      this.reactivateToolStripMenuItem.Name = "reactivateToolStripMenuItem";
      this.reactivateToolStripMenuItem.Size = new Size(120, 22);
      this.reactivateToolStripMenuItem.Text = "Reactivate";
      this.reactivateToolStripMenuItem.Click += new EventHandler(this.reactivateToolStripMenuItem_Click);
      this.btnDismiss.Location = new Point(10, 276);
      this.btnDismiss.Name = "btnDismiss";
      this.btnDismiss.Size = new Size(87, 22);
      this.btnDismiss.TabIndex = 2;
      this.btnDismiss.Text = "Dismiss Alert";
      this.btnDismiss.UseVisualStyleBackColor = true;
      this.btnDismiss.Click += new EventHandler(this.btnDismiss_Click);
      this.btnSnooze.Location = new Point(506, 276);
      this.btnSnooze.Name = "btnSnooze";
      this.btnSnooze.Size = new Size(75, 22);
      this.btnSnooze.TabIndex = 3;
      this.btnSnooze.Text = "Snooze";
      this.btnSnooze.UseVisualStyleBackColor = true;
      this.btnSnooze.Click += new EventHandler(this.btnSnooze_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(291, 280);
      this.label2.Name = "label2";
      this.label2.Size = new Size(85, 14);
      this.label2.TabIndex = 4;
      this.label2.Text = "Snooze alert for";
      this.cmbSnoozeDuration.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSnoozeDuration.FormattingEnabled = true;
      this.cmbSnoozeDuration.Items.AddRange(new object[10]
      {
        (object) "10 minutes",
        (object) "1 hour",
        (object) "4 hours",
        (object) "1 day",
        (object) "2 days",
        (object) "3 days",
        (object) "4 days",
        (object) "1 week",
        (object) "2 weeks",
        (object) "1 month"
      });
      this.cmbSnoozeDuration.Location = new Point(378, 277);
      this.cmbSnoozeDuration.Name = "cmbSnoozeDuration";
      this.cmbSnoozeDuration.Size = new Size(121, 22);
      this.cmbSnoozeDuration.TabIndex = 5;
      this.btnReactivate.Location = new Point(102, 276);
      this.btnReactivate.Name = "btnReactivate";
      this.btnReactivate.Size = new Size(87, 22);
      this.btnReactivate.TabIndex = 6;
      this.btnReactivate.Text = "Reactivate";
      this.btnReactivate.UseVisualStyleBackColor = true;
      this.btnReactivate.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(592, 308);
      this.Controls.Add((Control) this.btnReactivate);
      this.Controls.Add((Control) this.btnDismiss);
      this.Controls.Add((Control) this.cmbSnoozeDuration);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.gvAlerts);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnSnooze);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AlertDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Loan Alerts";
      this.KeyDown += new KeyEventHandler(this.AlertDialog_KeyDown);
      this.mnuContext.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
