// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ChangeMilestoneDates
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ChangeMilestoneDates : Form
  {
    private const string className = "ChangeMilestoneDates";
    protected static string sw = Tracing.SwInputEngine;
    private Sessions.Session session;
    private DateTime previousDate;
    private List<LogRecordBase> logRecords;
    private Dictionary<EllieMae.EMLite.Workflow.Milestone, DateTime> msd = new Dictionary<EllieMae.EMLite.Workflow.Milestone, DateTime>();
    private Dictionary<string, AclTriState> permissions = new Dictionary<string, AclTriState>();
    private DateTimeType dateTime;
    private AutoDayCountSetting setting;
    private ILoanEditor editor;
    private int i;
    private IContainer components;
    private GroupContainer grpTemplateMilestones;
    private BorderPanel pnlSequentialMilestones;
    private GridView gvMilestones;
    private Button btnApply;
    private Button btnCancel;
    private CheckBox chkManual;
    private ErrorProvider errorPro;
    private EMHelpLink emHelpLink1;
    private PictureBox pictureBox1;
    private Label lblDesc;
    private GradientPanel gradientPanel1;

    public ChangeMilestoneDates(Sessions.Session session, int index, DateTime date)
      : this(session, false)
    {
      ((DateTimePicker) this.gvMilestones.Items[index].SubItems[2].Value).Value = date;
      this.resetErrors();
    }

    public ChangeMilestoneDates(Sessions.Session session, bool mustResolve)
    {
      this.session = session;
      List<string> stringList = new List<string>();
      foreach (MilestoneLog allMilestone in this.session.LoanData.GetLogList().GetAllMilestones())
        stringList.Add(allMilestone.MilestoneID);
      this.permissions = ChangeMilestoneDates.GetPermissionFromUser(AclMilestone.ChangeExpectedDate, this.session, stringList.ToArray());
      this.logRecords = ((IEnumerable<LogRecordBase>) this.session.LoanData.GetLogList().GetAllDatedRecords()).ToList<LogRecordBase>();
      this.editor = Session.Application.GetService<ILoanEditor>();
      this.InitializeComponent();
      this.setting = (AutoDayCountSetting) Enum.Parse(typeof (AutoDayCountSetting), string.Concat(this.session.ServerManager.GetServerSetting("Policies.MilestoneExpDayCount")), true);
      if (this.setting == AutoDayCountSetting.BusinessDays)
        this.dateTime = DateTimeType.Business;
      else if (this.setting == AutoDayCountSetting.CalendarDays)
        this.dateTime = DateTimeType.Calendar;
      else if (this.setting == AutoDayCountSetting.CompanyDays)
        this.dateTime = DateTimeType.Company;
      this.chkManual.Checked = !this.session.LoanData.GetLogList().MSDateLock;
      this.PopulateData();
      this.resetErrors();
      this.btnCancel.Enabled = !mustResolve;
    }

    public void PopulateData()
    {
      MilestoneTemplatesBpmManager mileTempBpmMgr = (MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones);
      List<LogRecordBase> remove = new List<LogRecordBase>();
      this.logRecords.ToList<LogRecordBase>().ForEach((Action<LogRecordBase>) (item =>
      {
        if (item.GetType() == typeof (MilestoneLog))
        {
          MilestoneLog milestoneLog = (MilestoneLog) item;
          if (milestoneLog.Done)
          {
            this.previousDate = item.Date;
            this.gvMilestones.Items.Add(this.createGVItem(mileTempBpmMgr.GetMilestoneByID(milestoneLog.MilestoneID, milestoneLog.Stage, false, milestoneLog.Days, milestoneLog.DoneText, milestoneLog.ExpText), item.Date, true));
          }
          else
          {
            this.previousDate = !(item.Date.Date != DateTime.MaxValue.Date) || !(item.Date.Date != DateTime.MinValue.Date) ? this.editor.AddDays(this.previousDate, milestoneLog.Days) : item.Date;
            this.gvMilestones.Items.Add(this.createGVItem(mileTempBpmMgr.GetMilestoneByID(milestoneLog.MilestoneID, milestoneLog.Stage, false, milestoneLog.Days, milestoneLog.DoneText, milestoneLog.ExpText), this.previousDate, false));
          }
          this.msd.Add((EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[this.gvMilestones.Items.Count - 1].Tag, this.previousDate);
        }
        else
          remove.Add(item);
      }));
      remove.ForEach((Action<LogRecordBase>) (item => this.logRecords.Remove(item)));
      int num = this.gvMilestones.GetItemBounds(0).Height * (this.session.LoanData.GetLogList().GetNumberOfMilestones() + 2);
      if (num > 500)
        num = 500;
      this.gvMilestones.Height = num;
      this.pnlSequentialMilestones.Height = num + 1;
      this.grpTemplateMilestones.Height = num + 28;
      this.ClientSize = new Size(this.ClientSize.Width, num + 150);
    }

    public GVItem createGVItem(EllieMae.EMLite.Workflow.Milestone msItem, DateTime date, bool done)
    {
      Font font = new Font(this.gvMilestones.Font, FontStyle.Bold);
      GVItem gvItem = new GVItem();
      gvItem.SubItems.Add((object) new MilestoneLabel(msItem)
      {
        DisplayName = (done ? msItem.DescTextAfter : msItem.DescTextBefore)
      });
      gvItem.SubItems.Add((object) this.getRoleName(msItem.RoleID));
      gvItem.Tag = (object) msItem;
      DateTimePicker dateTimePicker = new DateTimePicker();
      dateTimePicker.Value = date;
      dateTimePicker.Name = "datepicker:" + this.i.ToString();
      ++this.i;
      dateTimePicker.CloseUp += new EventHandler(this.dp_CloseUp);
      dateTimePicker.KeyDown += new KeyEventHandler(this.dp_KeyDown);
      dateTimePicker.CalendarMonthBackground = Color.WhiteSmoke;
      dateTimePicker.CalendarTitleBackColor = SystemColors.GradientInactiveCaption;
      dateTimePicker.CustomFormat = "MM/dd/yyyy";
      dateTimePicker.Format = DateTimePickerFormat.Custom;
      if (this.permissions.ContainsKey(msItem.MilestoneID) && this.permissions[msItem.MilestoneID] == AclTriState.True)
        dateTimePicker.Enabled = true;
      else
        dateTimePicker.Enabled = false;
      if (done)
        dateTimePicker.Font = font;
      gvItem.SubItems.Add((object) dateTimePicker);
      if (done)
        gvItem.SubItems.ToList<GVSubItem>().ForEach((Action<GVSubItem>) (em => em.Font = font));
      return gvItem;
    }

    private void dp_KeyDown(object sender, KeyEventArgs e) => e.SuppressKeyPress = true;

    private void btnApply_Click(object sender, EventArgs e)
    {
      DateTime dateTime1 = new DateTime();
      bool flag = false;
      for (int nItemIndex = 0; nItemIndex < this.gvMilestones.Items.Count; ++nItemIndex)
      {
        DateTime dateTime2 = ((DateTimePicker) this.gvMilestones.Items[nItemIndex].SubItems[2].Value).Value;
        if (nItemIndex != 0 && ((DateTimePicker) this.gvMilestones.Items[nItemIndex - 1].SubItems[2].Value).Value.Date > dateTime2.Date || nItemIndex != this.gvMilestones.Items.Count - 1 && ((DateTimePicker) this.gvMilestones.Items[nItemIndex + 1].SubItems[2].Value).Value.Date < dateTime2.Date)
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "One or more date conflicts have occurred. To ensure a logical milestone workflow, these expected milestone completion dates must be in chronological order.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        MilestoneLog[] allMilestones = this.session.LoanData.GetLogList().GetAllMilestones();
        for (int nItemIndex = 0; nItemIndex < this.gvMilestones.Items.Count; ++nItemIndex)
          allMilestones[nItemIndex].AdjustAllDates(((DateTimePicker) this.gvMilestones.Items[nItemIndex].SubItems[2].Value).Value);
        if (allMilestones[0].Date != ((DateTimePicker) this.gvMilestones.Items[0].SubItems[2].Value).Value)
          allMilestones[0].AdjustAllDates(((DateTimePicker) this.gvMilestones.Items[0].SubItems[2].Value).Value);
        this.session.LoanData.GetLogList().ShowDatesInLog = true;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void dp_CloseUp(object sender, EventArgs e)
    {
      MilestoneLog[] allMilestones = this.session.LoanData.GetLogList().GetAllMilestones();
      DateTime date1 = ((DateTimePicker) sender).Value;
      int int32 = Convert.ToInt32(((Control) sender).Name.Split(':')[1]);
      DateTime date2;
      if (this.msd.ContainsKey((EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[int32].Tag))
      {
        date2 = this.msd[(EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[int32].Tag];
        if (date2.Date == date1.Date)
          return;
      }
      else
      {
        date2 = allMilestones[int32].Date;
        if (date2.Date == date1.Date)
          return;
      }
      DateTimePicker dateTimePicker1 = (DateTimePicker) this.gvMilestones.Items[int32].SubItems[2].Value;
      if (int32 != 0)
      {
        date2 = ((DateTimePicker) this.gvMilestones.Items[int32 - 1].SubItems[2].Value).Value;
        if (date2.Date > date1.Date)
        {
          this.errorPro.SetError((Control) dateTimePicker1, "The date is earlier than the previous milestone date. Change the date, or update the date for the previous milestone.");
          goto label_11;
        }
      }
      if (!this.chkManual.Checked && int32 != this.gvMilestones.Items.Count - 1)
      {
        date2 = ((DateTimePicker) this.gvMilestones.Items[int32 + 1].SubItems[2].Value).Value;
        if (date2.Date < date1.Date)
        {
          this.errorPro.SetError((Control) dateTimePicker1, "The date is later than the next milestone date. Change the date, or update the date for the next milestone.");
          goto label_11;
        }
      }
      this.errorPro.SetError((Control) dateTimePicker1, "");
label_11:
      if (this.chkManual.Checked)
      {
        Dictionary<EllieMae.EMLite.Workflow.Milestone, DateTime> dictionary = new Dictionary<EllieMae.EMLite.Workflow.Milestone, DateTime>();
        for (int nItemIndex = int32 + 1; nItemIndex < this.gvMilestones.Items.Count; ++nItemIndex)
        {
          int dayCount;
          try
          {
            DatetimeUtils datetimeUtils = !this.msd.ContainsKey((EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[nItemIndex - 1].Tag) ? new DatetimeUtils(allMilestones[nItemIndex - 1].Date, this.dateTime) : new DatetimeUtils(this.msd[(EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[nItemIndex - 1].Tag], this.dateTime);
            dayCount = !this.msd.ContainsKey((EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[nItemIndex].Tag) ? datetimeUtils.NumberOfDaysFrom(allMilestones[nItemIndex].Date) : datetimeUtils.NumberOfDaysFrom(this.msd[(EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[nItemIndex].Tag]);
            if (this.dateTime == DateTimeType.Company)
            {
              int num = this.editor.MinusBusinessDays(!this.msd.ContainsKey((EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[nItemIndex - 1].Tag) ? allMilestones[nItemIndex - 1].Date : this.msd[(EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[nItemIndex - 1].Tag], !this.msd.ContainsKey((EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[nItemIndex].Tag) ? allMilestones[nItemIndex].Date : this.msd[(EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[nItemIndex].Tag]);
              if (dayCount < 0)
                dayCount -= num;
              else if (dayCount > 0)
                dayCount += num;
            }
          }
          catch (Exception ex)
          {
            Tracing.Log(ChangeMilestoneDates.sw, nameof (ChangeMilestoneDates), TraceLevel.Error, ex.ToString());
            dayCount = allMilestones[nItemIndex].Days;
          }
          date1 = this.editor.AddDays(date1, dayCount);
          dictionary.Add((EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[nItemIndex].Tag, date1);
          DateTimePicker dateTimePicker2 = (DateTimePicker) this.gvMilestones.Items[nItemIndex].SubItems[2].Value;
          dateTimePicker2.Value = date1;
          this.errorPro.SetError((Control) dateTimePicker2, "");
        }
        foreach (KeyValuePair<EllieMae.EMLite.Workflow.Milestone, DateTime> keyValuePair in dictionary)
          this.msd[keyValuePair.Key] = keyValuePair.Value;
        if (this.msd.ContainsKey((EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[int32].Tag))
          this.msd[(EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[int32].Tag] = ((DateTimePicker) sender).Value;
      }
      else if (this.msd.ContainsKey((EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[int32].Tag))
        this.msd[(EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[int32].Tag] = date1;
      this.resetErrors();
    }

    private void resetErrors()
    {
      for (int index = 0; index < this.gvMilestones.Items.Count; ++index)
      {
        if (this.logRecords[index] is MilestoneLog)
        {
          DateTimePicker dateTimePicker = (DateTimePicker) this.gvMilestones.Items[index].SubItems[2].Value;
          DateTime dateTime;
          if (index != 0)
          {
            dateTime = ((DateTimePicker) this.gvMilestones.Items[index - 1].SubItems[2].Value).Value;
            DateTime date1 = dateTime.Date;
            dateTime = dateTimePicker.Value;
            DateTime date2 = dateTime.Date;
            if (date1 > date2)
            {
              this.errorPro.SetError((Control) dateTimePicker, "The date is earlier than the previous milestone date. Change the date, or update the date for the previous milestone.");
              continue;
            }
          }
          if (index != this.gvMilestones.Items.Count - 1)
          {
            dateTime = ((DateTimePicker) this.gvMilestones.Items[index + 1].SubItems[2].Value).Value;
            DateTime date3 = dateTime.Date;
            dateTime = dateTimePicker.Value;
            DateTime date4 = dateTime.Date;
            if (date3 < date4)
            {
              this.errorPro.SetError((Control) dateTimePicker, "The date is later than the next milestone date. Change the date, or update the date for the next milestone.");
              continue;
            }
          }
          this.errorPro.SetError((Control) dateTimePicker, "");
        }
      }
    }

    private string getRoleName(int roleId)
    {
      RoleInfo roleInfo = ((IEnumerable<RoleInfo>) this.session.SessionObjects.BpmManager.GetAllRoleFunctions()).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleID == roleId));
      return roleInfo != null ? roleInfo.RoleName : "";
    }

    public static Dictionary<string, AclTriState> GetPermissionFromUser(
      AclMilestone feature,
      Sessions.Session session,
      string[] milestoneIDs)
    {
      return ((MilestonesAclManager) session.ACL.GetAclManager(AclCategory.Milestones)).CheckApplicationPermissions(feature, milestoneIDs, session.UserInfo);
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
      GVColumn gvColumn3 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ChangeMilestoneDates));
      this.grpTemplateMilestones = new GroupContainer();
      this.pnlSequentialMilestones = new BorderPanel();
      this.gvMilestones = new GridView();
      this.btnApply = new Button();
      this.btnCancel = new Button();
      this.chkManual = new CheckBox();
      this.errorPro = new ErrorProvider(this.components);
      this.emHelpLink1 = new EMHelpLink();
      this.pictureBox1 = new PictureBox();
      this.lblDesc = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.grpTemplateMilestones.SuspendLayout();
      this.pnlSequentialMilestones.SuspendLayout();
      ((ISupportInitialize) this.errorPro).BeginInit();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.grpTemplateMilestones.Controls.Add((Control) this.pnlSequentialMilestones);
      this.grpTemplateMilestones.HeaderForeColor = SystemColors.ControlText;
      this.grpTemplateMilestones.Location = new Point(-1, 92);
      this.grpTemplateMilestones.Name = "grpTemplateMilestones";
      this.grpTemplateMilestones.Size = new Size(463, 315);
      this.grpTemplateMilestones.TabIndex = 1;
      this.grpTemplateMilestones.Text = "Log";
      this.pnlSequentialMilestones.Borders = AnchorStyles.Bottom;
      this.pnlSequentialMilestones.Controls.Add((Control) this.gvMilestones);
      this.pnlSequentialMilestones.Dock = DockStyle.Fill;
      this.pnlSequentialMilestones.Location = new Point(1, 26);
      this.pnlSequentialMilestones.Name = "pnlSequentialMilestones";
      this.pnlSequentialMilestones.Size = new Size(461, 288);
      this.pnlSequentialMilestones.TabIndex = 1;
      this.gvMilestones.AllowDrop = true;
      this.gvMilestones.AllowMultiselect = false;
      this.gvMilestones.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Milestone";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Role";
      gvColumn2.Width = 120;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Date";
      gvColumn3.Width = 120;
      this.gvMilestones.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvMilestones.Dock = DockStyle.Fill;
      this.gvMilestones.DropTarget = GVDropTarget.BetweenItems;
      this.gvMilestones.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvMilestones.Location = new Point(0, 0);
      this.gvMilestones.Name = "gvMilestones";
      this.gvMilestones.Size = new Size(461, 287);
      this.gvMilestones.SortOption = GVSortOption.None;
      this.gvMilestones.TabIndex = 1;
      this.btnApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnApply.Location = new Point(299, 419);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new Size(75, 23);
      this.btnApply.TabIndex = 2;
      this.btnApply.Text = "Apply";
      this.btnApply.UseVisualStyleBackColor = true;
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(380, 420);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.chkManual.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkManual.AutoSize = true;
      this.chkManual.BackColor = Color.Transparent;
      this.chkManual.Checked = true;
      this.chkManual.CheckState = CheckState.Checked;
      this.chkManual.Location = new Point(4, 7);
      this.chkManual.Name = "chkManual";
      this.chkManual.Size = new Size(333, 17);
      this.chkManual.TabIndex = 4;
      this.chkManual.Text = "If date is changed, automatically recalculate all subsequent dates";
      this.chkManual.UseVisualStyleBackColor = false;
      this.errorPro.ContainerControl = (ContainerControl) this;
      this.errorPro.DataMember = "";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = nameof (ChangeMilestoneDates);
      this.emHelpLink1.Location = new Point(4, 426);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 42;
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(12, 14);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 43;
      this.pictureBox1.TabStop = false;
      this.lblDesc.Location = new Point(50, 12);
      this.lblDesc.Name = "lblDesc";
      this.lblDesc.Size = new Size(326, 43);
      this.lblDesc.TabIndex = 44;
      this.lblDesc.Text = "Click a Calendar icon to set a new milestone expected completion date. Dates must progress in chronological order and will be applied to the current loan.";
      this.gradientPanel1.Controls.Add((Control) this.chkManual);
      this.gradientPanel1.GradientColor1 = Color.White;
      this.gradientPanel1.GradientColor2 = Color.White;
      this.gradientPanel1.Location = new Point(-2, 61);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(464, 31);
      this.gradientPanel1.TabIndex = 45;
      this.AcceptButton = (IButtonControl) this.btnApply;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(463, 448);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Controls.Add((Control) this.lblDesc);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnApply);
      this.Controls.Add((Control) this.grpTemplateMilestones);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ChangeMilestoneDates);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Change Milestone Dates";
      this.grpTemplateMilestones.ResumeLayout(false);
      this.pnlSequentialMilestones.ResumeLayout(false);
      ((ISupportInitialize) this.errorPro).EndInit();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
