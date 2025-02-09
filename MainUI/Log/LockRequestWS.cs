// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LockRequestWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class LockRequestWS : UserControl
  {
    private Hashtable loanSnapshot;
    private LockRequestLog requestLog;
    private LockConfirmLog confirmLog;
    private bool isConfirmed;
    private LockConfirmLog[] confirmLocks;
    private LockDenialLog[] denialLocks;
    private LockCancellationLog[] cancellationLocks;
    private Sessions.Session session;
    private IContainer components;
    private Label label1;
    private Label label3;
    private Label label4;
    private Label label5;
    private TextBox textBox2;
    private TextBox textBox3;
    private Label label6;
    private Label label7;
    private TextBox textBox4;
    private TextBox textBox5;
    private Label label8;
    private TextBox textBox6;
    private TextBox textBox7;
    private Label label10;
    private TextBox textBox8;
    private TextBox textBox9;
    private TextBox boxRequestedBy;
    private TextBox boxDateRequested;
    private ToolTip toolTipField;
    private Label labelCommitment;
    private TextBox textBoxCommitment;
    private Label labelTotalPrice;
    private Label labelTotalRate;
    private TextBox textBoxRegistered;
    private Label labelRegistered;
    private TextBox textBox1;
    private Label label9;
    private TextBox textBox44;
    private Label label11;
    private Button clearAlertBtn;
    private Label label12;
    private TextBox textBox10;
    private TextBox textBox11;
    private Label label13;
    private Label label14;
    private TextBox textBox12;
    private TextBox textBox13;
    private Label label15;
    private GroupContainer groupContainerMiddle;
    private Label labelInfo;
    private GridView listViewRate;
    private GridView listViewPrice;
    private GridView listViewMargin;
    private GroupContainer groupContainerAll;
    private Panel panelInside;
    private BorderPanel pnlRequested;
    private TextBox textBox14;
    private Label label16;
    private Panel pnlDetail;
    private Panel pnlLockExtension;
    private Panel pnlRegular;
    private TextBox textBox15;
    private TextBox textBox16;
    private TextBox textBox17;
    private Label label2;
    private Label label17;
    private TextBox textBox18;
    private Label label18;
    private Label label19;
    private Label label20;
    private TextBox textBox22;
    private Label label23;
    private TextBox textBox21;
    private Label label22;
    private TextBox textBox20;
    private Label label21;
    private TextBox textBox19;
    private BorderPanel pnlConfirmed;
    private Label label24;
    private TextBox boxRequestedBy2;
    private TextBox boxDateRequested2;
    private TextBox textBoxRegistered2;
    private Label labelRegistered2;
    private Label label26;
    private TextBox txtConfirmedBy;
    private TextBox txtConfirmedDate;
    private Label label28;
    private Label label27;
    private Label label25;
    private TextBox textBox23;
    private TextBox textBox24;
    private Label label29;
    private Panel panelPriceConcession;

    public LockRequestWS(Sessions.Session session, LockConfirmLog confirmLog)
    {
      this.session = session;
      this.isConfirmed = true;
      this.confirmLog = confirmLog;
      this.Dock = DockStyle.Fill;
      this.InitializeComponent();
      this.requestLog = this.session.LoanDataMgr.LoanData.GetLogList().GetLockRequest(this.confirmLog.RequestGUID);
      this.labelInfo.Text = " Rate Lock ";
      this.labelCommitment.Visible = true;
      this.textBoxCommitment.Visible = true;
      this.labelTotalRate.Text = "Total Rate Locked:";
      this.labelTotalPrice.Text = "Total Price Locked:";
      this.labelRegistered.Visible = this.labelRegistered2.Visible = true;
      this.textBoxRegistered.Visible = this.textBoxRegistered2.Visible = true;
      if (this.requestLog.IsLockExtension)
      {
        this.pnlRegular.Visible = false;
        this.pnlLockExtension.Visible = true;
      }
      else
      {
        this.pnlLockExtension.Visible = false;
        this.pnlRegular.Visible = true;
      }
      this.panelPriceConcession.Visible = true;
      this.initForm();
      this.checkAleartStatus();
    }

    public LockRequestWS(Sessions.Session session, LockRequestLog requestLog)
    {
      this.session = session;
      this.isConfirmed = false;
      this.requestLog = requestLog;
      this.Dock = DockStyle.Fill;
      this.InitializeComponent();
      this.labelInfo.Text = " Rate Request ";
      this.clearAlertBtn.Visible = false;
      if (this.requestLog.IsLockExtension)
      {
        this.pnlRegular.Visible = false;
        this.pnlLockExtension.Visible = true;
        this.panelPriceConcession.Visible = true;
      }
      else
      {
        this.pnlLockExtension.Visible = false;
        this.pnlRegular.Visible = true;
        this.panelPriceConcession.Visible = false;
      }
      this.initForm();
    }

    private void initForm()
    {
      this.loanSnapshot = this.requestLog.GetLockRequestSnapshot();
      if (this.loanSnapshot != null)
        this.populateLoanSnapshot(this.Controls);
      this.populateToolTips(this.Controls);
      if (this.requestLog.Date != DateTime.MinValue)
      {
        TextBox boxDateRequested = this.boxDateRequested;
        TextBox boxDateRequested2 = this.boxDateRequested2;
        DateTime date = this.requestLog.Date;
        string str1;
        string str2 = str1 = date.ToString("MM/dd/yyyy") + " " + this.requestLog.TimeRequested;
        boxDateRequested2.Text = str1;
        string str3 = str2;
        boxDateRequested.Text = str3;
      }
      this.boxRequestedBy.Text = this.boxRequestedBy2.Text = this.requestLog.RequestedFullName;
      this.listViewRate.Items.Clear();
      this.listViewPrice.Items.Clear();
      this.listViewMargin.Items.Clear();
      this.listViewRate.BeginUpdate();
      if (this.isConfirmed)
      {
        this.populateListView(this.listViewRate, 2153, 2157);
        this.populateListView(this.listViewRate, 2448, 2480);
        this.populateListView(this.listViewPrice, 2162, 2200);
        LoanLockUtils.IncludeAdjustments(this.listViewPrice, this.loanSnapshot, 3474, "Extension #");
        LoanLockUtils.IncludeAdjustments(this.listViewPrice, this.loanSnapshot, 4276, "Re-Lock Fees #");
        LoanLockUtils.IncludeAdjustments(this.listViewPrice, this.loanSnapshot, 4356, "Custom Price Adjustments #");
        this.populateListView(this.listViewMargin, 2734, 2772);
        this.textBoxRegistered.Text = this.textBoxRegistered2.Text = this.confirmLog.ConfirmedByFullName;
        this.groupContainerAll.Text = this.requestLog.RateLockAction != RateLockAction.TradeExtension ? (this.requestLog.IsLockExtension ? "Extension Confirmed by " + this.confirmLog.ConfirmedByFullName : "Lock Confirmed by " + this.confirmLog.ConfirmedByFullName) : "Extension Confirmed from Correspondent Trade by " + this.confirmLog.ConfirmedByFullName;
        if (this.confirmLog.DateTimeConfirmed != DateTime.MinValue)
          this.txtConfirmedDate.Text = this.confirmLog.DateTimeConfirmed.ToString("MM/dd/yyyy h:mm:ss tt");
        this.txtConfirmedBy.Text = this.confirmLog.ConfirmedByFullName;
        this.pnlRequested.Visible = false;
        this.pnlConfirmed.Visible = true;
        this.pnlConfirmed.Top = 0;
        this.groupContainerMiddle.Top = this.pnlConfirmed.Top + this.pnlConfirmed.Height - 1;
      }
      else
      {
        this.populateListView(this.listViewRate, 2093, 2097);
        this.populateListView(this.listViewRate, 2414, 2446);
        this.populateListView(this.listViewPrice, 2102, 2140);
        LoanLockUtils.IncludeAdjustments(this.listViewPrice, this.loanSnapshot, 3454, "Extension #");
        LoanLockUtils.IncludeAdjustments(this.listViewPrice, this.loanSnapshot, 4256, "Re-Lock Fees #");
        LoanLockUtils.IncludeAdjustments(this.listViewPrice, this.loanSnapshot, 4336, "Custom Price Adjustments #");
        this.populateListView(this.listViewMargin, 2648, 2686);
        this.groupContainerAll.Text = this.requestLog.RateLockAction != RateLockAction.TradeExtension ? (this.requestLog.IsLockExtension ? "Extension Requested by " + this.requestLog.RequestedFullName : "Lock Requested by " + this.requestLog.RequestedFullName) : "Extension Requested from Correspondent Trade by " + this.requestLog.RequestedFullName;
        this.pnlConfirmed.Visible = false;
        this.pnlRequested.Visible = true;
        this.groupContainerMiddle.Top = this.pnlRequested.Top + this.pnlRequested.Height - 1;
      }
      this.listViewRate.EndUpdate();
      this.listViewPrice.EndUpdate();
      this.listViewMargin.EndUpdate();
      RequestSnapshotForm requestSnapshotForm = new RequestSnapshotForm(this.session, this.requestLog, this.isConfirmed, AppColors.GoneFishing, this.session.LoanDataMgr.LoanData);
      requestSnapshotForm.Top = this.groupContainerMiddle.Top + this.groupContainerMiddle.Height - 1;
      this.panelInside.Controls.Add((Control) requestSnapshotForm);
      this.panelInside.Height = requestSnapshotForm.Top + requestSnapshotForm.Height;
      requestSnapshotForm.RemoveLeftBorder();
      requestSnapshotForm.BringToFront();
    }

    private void populateListView(GridView lv, int sf, int ef)
    {
      if (this.loanSnapshot == null)
        return;
      for (int index = sf; index <= ef; index += 2)
      {
        int num = index + 1;
        if (this.loanSnapshot.ContainsKey((object) index.ToString()) || this.loanSnapshot.ContainsKey((object) num.ToString()))
        {
          string text = string.Empty;
          if (this.loanSnapshot.ContainsKey((object) index.ToString()))
            text = this.isFieldHiddenByRule(index.ToString()) ? "*" : this.loanSnapshot[(object) index.ToString()].ToString();
          string str = string.Empty;
          if (this.loanSnapshot.ContainsKey((object) num.ToString()))
            str = this.isFieldHiddenByRule(num.ToString()) ? "*" : this.loanSnapshot[(object) num.ToString()].ToString();
          lv.Items.Add(new GVItem(text)
          {
            SubItems = {
              (object) str
            }
          });
        }
      }
    }

    private void populateLoanSnapshot(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        if (c is TextBox)
        {
          TextBox textBox = (TextBox) c;
          if (textBox != null && textBox.Tag != null)
          {
            string str = textBox.Tag.ToString();
            if (!(str == string.Empty))
            {
              if (this.isConfirmed)
              {
                switch (str)
                {
                  case "2088":
                    str = "2148";
                    textBox.Tag = (object) str;
                    break;
                  case "2089":
                    str = "2149";
                    textBox.Tag = (object) str;
                    break;
                  case "2090":
                    str = "2150";
                    textBox.Tag = (object) str;
                    break;
                  case "2092":
                    str = "2152";
                    textBox.Tag = (object) str;
                    break;
                  case "2099":
                    str = "2159";
                    textBox.Tag = (object) str;
                    break;
                  case "2100":
                    str = "2160";
                    textBox.Tag = (object) str;
                    break;
                  case "2101":
                    str = "3420";
                    textBox.Tag = (object) str;
                    break;
                  case "2142":
                    str = "2202";
                    textBox.Tag = (object) str;
                    break;
                  case "2143":
                    str = "2203";
                    textBox.Tag = (object) str;
                    break;
                  case "2144":
                    str = "2204";
                    textBox.Tag = (object) str;
                    break;
                  case "2151":
                    if (c.Name == "textBox16")
                    {
                      str = "3358";
                      textBox.Tag = (object) str;
                      break;
                    }
                    break;
                  case "2647":
                    str = "2733";
                    textBox.Tag = (object) str;
                    break;
                  case "2688":
                    str = "2774";
                    textBox.Tag = (object) str;
                    break;
                  case "2689":
                    str = "2775";
                    textBox.Tag = (object) str;
                    break;
                  case "3360":
                    str = "3363";
                    textBox.Tag = (object) str;
                    break;
                  case "3361":
                    str = "3364";
                    textBox.Tag = (object) str;
                    break;
                  case "3362":
                    str = "3365";
                    textBox.Tag = (object) str;
                    break;
                  case "3369":
                    str = "2151";
                    textBox.Tag = (object) str;
                    break;
                }
              }
              else if (this.requestLog.IsLockExtension)
              {
                if (str == "2101")
                {
                  str = "3420";
                  textBox.Tag = (object) str;
                }
              }
              else if (str == "2151")
              {
                if (c.Name == "textBox5")
                  str = "2091";
                textBox.Tag = (object) "2091";
              }
              if (this.loanSnapshot.ContainsKey((object) str))
                textBox.Text = this.isFieldHiddenByRule(str) ? "*" : this.formatValue(str, this.loanSnapshot[(object) str].ToString());
            }
          }
        }
        else
          this.populateLoanSnapshot(c.Controls);
      }
    }

    private string formatValue(string id, string val)
    {
      if (!(id == "2216"))
        return val;
      if (string.Compare(val, "Y", true) == 0)
        return "Yes";
      return string.Compare(val, "N", true) == 0 ? "No" : "";
    }

    private bool isFieldHiddenByRule(string id)
    {
      Hashtable hiddenFields = this.session.LoanDataMgr.GetHiddenFields();
      return hiddenFields.ContainsKey((object) id) || hiddenFields.ContainsKey((object) ("LOCKRATE." + id));
    }

    private void populateToolTips(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        if (c is TextBox)
        {
          TextBox textBox = (TextBox) c;
          if (textBox != null && textBox.Tag != null)
          {
            string caption = textBox.Tag.ToString();
            if (!(caption == string.Empty))
              this.toolTipField.SetToolTip((Control) textBox, caption);
          }
        }
        else
          this.populateToolTips(c.Controls);
      }
    }

    private void clearAlertBtn_Click(object sender, EventArgs e)
    {
      LogList logList = this.session.LoanData.GetLogList();
      if (this.confirmLocks == null)
        this.confirmLocks = (LockConfirmLog[]) logList.GetAllRecordsOfType(typeof (LockConfirmLog));
      foreach (LockConfirmLog confirmLock in this.confirmLocks)
        confirmLock.AlertLoanOfficer = false;
      if (this.denialLocks == null)
        this.denialLocks = (LockDenialLog[]) logList.GetAllRecordsOfType(typeof (LockDenialLog));
      foreach (LockDenialLog denialLock in this.denialLocks)
        denialLock.AlertLoanOfficer = false;
      LockCancellationLog[] allRecordsOfType = (LockCancellationLog[]) logList.GetAllRecordsOfType(typeof (LockCancellationLog));
      foreach (LockCancellationLog lockCancellationLog in allRecordsOfType)
        lockCancellationLog.AlertLoanOfficer = false;
      if (allRecordsOfType == null)
        allRecordsOfType = (LockCancellationLog[]) logList.GetAllRecordsOfType(typeof (LockCancellationLog));
      foreach (LockCancellationLog lockCancellationLog in allRecordsOfType)
        lockCancellationLog.AlertLoanOfficer = false;
      this.checkAleartStatus();
      int num = (int) Utils.Dialog((IWin32Window) this, "All Lock Alerts have been cleared.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void checkAleartStatus()
    {
      LogList logList = this.session.LoanData.GetLogList();
      if (this.confirmLocks == null)
        this.confirmLocks = (LockConfirmLog[]) logList.GetAllRecordsOfType(typeof (LockConfirmLog));
      foreach (LockConfirmLog confirmLock in this.confirmLocks)
      {
        if (confirmLock.AlertLoanOfficer)
        {
          this.clearAlertBtn.Enabled = true;
          return;
        }
      }
      if (this.denialLocks == null)
        this.denialLocks = (LockDenialLog[]) logList.GetAllRecordsOfType(typeof (LockDenialLog));
      foreach (LockDenialLog denialLock in this.denialLocks)
      {
        if (denialLock.AlertLoanOfficer)
        {
          this.clearAlertBtn.Enabled = true;
          return;
        }
      }
      if (this.cancellationLocks == null)
        this.cancellationLocks = (LockCancellationLog[]) logList.GetAllRecordsOfType(typeof (LockCancellationLog));
      foreach (LockCancellationLog cancellationLock in this.cancellationLocks)
      {
        if (cancellationLock.AlertLoanOfficer)
        {
          this.clearAlertBtn.Enabled = true;
          return;
        }
      }
      this.clearAlertBtn.Enabled = false;
    }

    private void textBox16_TextChanged(object sender, EventArgs e)
    {
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
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.toolTipField = new ToolTip(this.components);
      this.groupContainerAll = new GroupContainer();
      this.panelInside = new Panel();
      this.groupContainerMiddle = new GroupContainer();
      this.pnlDetail = new Panel();
      this.panelPriceConcession = new Panel();
      this.textBox23 = new TextBox();
      this.label25 = new Label();
      this.label29 = new Label();
      this.textBox24 = new TextBox();
      this.label9 = new Label();
      this.label11 = new Label();
      this.label8 = new Label();
      this.textBox6 = new TextBox();
      this.listViewMargin = new GridView();
      this.labelTotalRate = new Label();
      this.listViewPrice = new GridView();
      this.textBox7 = new TextBox();
      this.listViewRate = new GridView();
      this.label10 = new Label();
      this.textBox8 = new TextBox();
      this.textBox11 = new TextBox();
      this.textBox9 = new TextBox();
      this.label13 = new Label();
      this.labelTotalPrice = new Label();
      this.label14 = new Label();
      this.textBox1 = new TextBox();
      this.textBox12 = new TextBox();
      this.textBox44 = new TextBox();
      this.textBox13 = new TextBox();
      this.label12 = new Label();
      this.label15 = new Label();
      this.textBox10 = new TextBox();
      this.pnlLockExtension = new Panel();
      this.textBox22 = new TextBox();
      this.label23 = new Label();
      this.textBox21 = new TextBox();
      this.label22 = new Label();
      this.textBox20 = new TextBox();
      this.label21 = new Label();
      this.textBox19 = new TextBox();
      this.label20 = new Label();
      this.textBox15 = new TextBox();
      this.textBox16 = new TextBox();
      this.textBox17 = new TextBox();
      this.label2 = new Label();
      this.label17 = new Label();
      this.textBox18 = new TextBox();
      this.label18 = new Label();
      this.label19 = new Label();
      this.pnlRegular = new Panel();
      this.textBox2 = new TextBox();
      this.textBox14 = new TextBox();
      this.textBox5 = new TextBox();
      this.label16 = new Label();
      this.textBox4 = new TextBox();
      this.label6 = new Label();
      this.label7 = new Label();
      this.textBox3 = new TextBox();
      this.label4 = new Label();
      this.labelCommitment = new Label();
      this.label5 = new Label();
      this.textBoxCommitment = new TextBox();
      this.labelInfo = new Label();
      this.pnlConfirmed = new BorderPanel();
      this.txtConfirmedBy = new TextBox();
      this.txtConfirmedDate = new TextBox();
      this.label28 = new Label();
      this.label27 = new Label();
      this.label24 = new Label();
      this.boxRequestedBy2 = new TextBox();
      this.boxDateRequested2 = new TextBox();
      this.textBoxRegistered2 = new TextBox();
      this.labelRegistered2 = new Label();
      this.label26 = new Label();
      this.pnlRequested = new BorderPanel();
      this.label1 = new Label();
      this.boxRequestedBy = new TextBox();
      this.boxDateRequested = new TextBox();
      this.textBoxRegistered = new TextBox();
      this.labelRegistered = new Label();
      this.label3 = new Label();
      this.clearAlertBtn = new Button();
      this.groupContainerAll.SuspendLayout();
      this.panelInside.SuspendLayout();
      this.groupContainerMiddle.SuspendLayout();
      this.pnlDetail.SuspendLayout();
      this.panelPriceConcession.SuspendLayout();
      this.pnlLockExtension.SuspendLayout();
      this.pnlRegular.SuspendLayout();
      this.pnlConfirmed.SuspendLayout();
      this.pnlRequested.SuspendLayout();
      this.SuspendLayout();
      this.groupContainerAll.Controls.Add((Control) this.panelInside);
      this.groupContainerAll.Controls.Add((Control) this.clearAlertBtn);
      this.groupContainerAll.Dock = DockStyle.Fill;
      this.groupContainerAll.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerAll.Location = new Point(0, 0);
      this.groupContainerAll.Name = "groupContainerAll";
      this.groupContainerAll.Size = new Size(673, 1242);
      this.groupContainerAll.TabIndex = 3;
      this.groupContainerAll.Text = "Lock Request";
      this.panelInside.AutoScroll = true;
      this.panelInside.Controls.Add((Control) this.groupContainerMiddle);
      this.panelInside.Controls.Add((Control) this.pnlConfirmed);
      this.panelInside.Controls.Add((Control) this.pnlRequested);
      this.panelInside.Dock = DockStyle.Fill;
      this.panelInside.Location = new Point(1, 26);
      this.panelInside.Name = "panelInside";
      this.panelInside.Size = new Size(671, 1215);
      this.panelInside.TabIndex = 4;
      this.groupContainerMiddle.Borders = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.groupContainerMiddle.Controls.Add((Control) this.pnlDetail);
      this.groupContainerMiddle.Controls.Add((Control) this.pnlLockExtension);
      this.groupContainerMiddle.Controls.Add((Control) this.pnlRegular);
      this.groupContainerMiddle.Controls.Add((Control) this.labelInfo);
      this.groupContainerMiddle.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerMiddle.Location = new Point(0, 144);
      this.groupContainerMiddle.Name = "groupContainerMiddle";
      this.groupContainerMiddle.Size = new Size(660, 946);
      this.groupContainerMiddle.TabIndex = 1;
      this.pnlDetail.Controls.Add((Control) this.panelPriceConcession);
      this.pnlDetail.Controls.Add((Control) this.label9);
      this.pnlDetail.Controls.Add((Control) this.label11);
      this.pnlDetail.Controls.Add((Control) this.label8);
      this.pnlDetail.Controls.Add((Control) this.textBox6);
      this.pnlDetail.Controls.Add((Control) this.listViewMargin);
      this.pnlDetail.Controls.Add((Control) this.labelTotalRate);
      this.pnlDetail.Controls.Add((Control) this.listViewPrice);
      this.pnlDetail.Controls.Add((Control) this.textBox7);
      this.pnlDetail.Controls.Add((Control) this.listViewRate);
      this.pnlDetail.Controls.Add((Control) this.label10);
      this.pnlDetail.Controls.Add((Control) this.textBox8);
      this.pnlDetail.Controls.Add((Control) this.textBox11);
      this.pnlDetail.Controls.Add((Control) this.textBox9);
      this.pnlDetail.Controls.Add((Control) this.label13);
      this.pnlDetail.Controls.Add((Control) this.labelTotalPrice);
      this.pnlDetail.Controls.Add((Control) this.label14);
      this.pnlDetail.Controls.Add((Control) this.textBox1);
      this.pnlDetail.Controls.Add((Control) this.textBox12);
      this.pnlDetail.Controls.Add((Control) this.textBox44);
      this.pnlDetail.Controls.Add((Control) this.textBox13);
      this.pnlDetail.Controls.Add((Control) this.label12);
      this.pnlDetail.Controls.Add((Control) this.label15);
      this.pnlDetail.Controls.Add((Control) this.textBox10);
      this.pnlDetail.Dock = DockStyle.Fill;
      this.pnlDetail.Location = new Point(0, 200);
      this.pnlDetail.Name = "pnlDetail";
      this.pnlDetail.Size = new Size(659, 745);
      this.pnlDetail.TabIndex = 54;
      this.panelPriceConcession.Controls.Add((Control) this.textBox23);
      this.panelPriceConcession.Controls.Add((Control) this.label25);
      this.panelPriceConcession.Controls.Add((Control) this.label29);
      this.panelPriceConcession.Controls.Add((Control) this.textBox24);
      this.panelPriceConcession.Location = new Point(326, 262);
      this.panelPriceConcession.Name = "panelPriceConcession";
      this.panelPriceConcession.Size = new Size(322, 43);
      this.panelPriceConcession.TabIndex = 52;
      this.textBox23.BackColor = Color.WhiteSmoke;
      this.textBox23.Location = new Point(218, 0);
      this.textBox23.Name = "textBox23";
      this.textBox23.ReadOnly = true;
      this.textBox23.Size = new Size(100, 20);
      this.textBox23.TabIndex = 49;
      this.textBox23.Tag = (object) "4858";
      this.textBox23.TextAlign = HorizontalAlignment.Right;
      this.label25.AutoSize = true;
      this.label25.Location = new Point(4, 4);
      this.label25.Name = "label25";
      this.label25.Size = new Size(165, 13);
      this.label25.TabIndex = 48;
      this.label25.Text = "Total Corporate Price Concession";
      this.label29.AutoSize = true;
      this.label29.Location = new Point(4, 26);
      this.label29.Name = "label29";
      this.label29.Size = new Size(153, 13);
      this.label29.TabIndex = 51;
      this.label29.Text = "Total Branch Price Concession";
      this.textBox24.BackColor = Color.WhiteSmoke;
      this.textBox24.Location = new Point(218, 22);
      this.textBox24.Name = "textBox24";
      this.textBox24.ReadOnly = true;
      this.textBox24.Size = new Size(100, 20);
      this.textBox24.TabIndex = 50;
      this.textBox24.Tag = (object) "4857";
      this.textBox24.TextAlign = HorizontalAlignment.Right;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(3, 10);
      this.label9.Name = "label9";
      this.label9.Size = new Size(57, 13);
      this.label9.TabIndex = 31;
      this.label9.Text = "Base Rate";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(333, 10);
      this.label11.Name = "label11";
      this.label11.Size = new Size(58, 13);
      this.label11.TabIndex = 33;
      this.label11.Text = "Base Price";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(3, 309);
      this.label8.Name = "label8";
      this.label8.Size = new Size(117, 13);
      this.label8.TabIndex = 20;
      this.label8.Text = "Total Rate Adjustments";
      this.textBox6.BackColor = Color.WhiteSmoke;
      this.textBox6.Location = new Point(214, 306);
      this.textBox6.Name = "textBox6";
      this.textBox6.ReadOnly = true;
      this.textBox6.Size = new Size(100, 20);
      this.textBox6.TabIndex = 21;
      this.textBox6.Tag = (object) "2099";
      this.textBox6.TextAlign = HorizontalAlignment.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Price Adjustment";
      gvColumn1.Width = 194;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Amount";
      gvColumn2.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn2.Width = 114;
      this.listViewMargin.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.listViewMargin.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewMargin.Location = new Point(4, 385);
      this.listViewMargin.Name = "listViewMargin";
      this.listViewMargin.Size = new Size(310, 224);
      this.listViewMargin.SortOption = GVSortOption.None;
      this.listViewMargin.TabIndex = 47;
      this.labelTotalRate.AutoSize = true;
      this.labelTotalRate.Location = new Point(3, 331);
      this.labelTotalRate.Name = "labelTotalRate";
      this.labelTotalRate.Size = new Size(112, 13);
      this.labelTotalRate.TabIndex = 22;
      this.labelTotalRate.Text = "Total Rate Requested";
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.Text = "Price Adjustment";
      gvColumn3.Width = 194;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Amount";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 114;
      this.listViewPrice.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.listViewPrice.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewPrice.Location = new Point(334, 32);
      this.listViewPrice.Name = "listViewPrice";
      this.listViewPrice.Size = new Size(310, 224);
      this.listViewPrice.SortOption = GVSortOption.None;
      this.listViewPrice.TabIndex = 46;
      this.textBox7.BackColor = Color.WhiteSmoke;
      this.textBox7.Location = new Point(214, 328);
      this.textBox7.Name = "textBox7";
      this.textBox7.ReadOnly = true;
      this.textBox7.Size = new Size(100, 20);
      this.textBox7.TabIndex = 23;
      this.textBox7.Tag = (object) "2100";
      this.textBox7.TextAlign = HorizontalAlignment.Right;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column1";
      gvColumn5.Text = "Rate Adjustment";
      gvColumn5.Width = 194;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column2";
      gvColumn6.SpringToFit = true;
      gvColumn6.Text = "Amount";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 114;
      this.listViewRate.Columns.AddRange(new GVColumn[2]
      {
        gvColumn5,
        gvColumn6
      });
      this.listViewRate.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewRate.Location = new Point(4, 32);
      this.listViewRate.Name = "listViewRate";
      this.listViewRate.Size = new Size(310, 224);
      this.listViewRate.SortOption = GVSortOption.None;
      this.listViewRate.TabIndex = 45;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(330, 310);
      this.label10.Name = "label10";
      this.label10.Size = new Size(118, 13);
      this.label10.TabIndex = 24;
      this.label10.Text = "Total Price Adjustments";
      this.textBox8.BackColor = Color.WhiteSmoke;
      this.textBox8.Location = new Point(544, 306);
      this.textBox8.Name = "textBox8";
      this.textBox8.ReadOnly = true;
      this.textBox8.Size = new Size(100, 20);
      this.textBox8.TabIndex = 25;
      this.textBox8.Tag = (object) "2142";
      this.textBox8.TextAlign = HorizontalAlignment.Right;
      this.textBox11.BackColor = Color.WhiteSmoke;
      this.textBox11.Location = new Point(214, 363);
      this.textBox11.Name = "textBox11";
      this.textBox11.ReadOnly = true;
      this.textBox11.Size = new Size(100, 20);
      this.textBox11.TabIndex = 43;
      this.textBox11.Tag = (object) "2647";
      this.textBox11.TextAlign = HorizontalAlignment.Right;
      this.textBox9.BackColor = Color.WhiteSmoke;
      this.textBox9.Location = new Point(544, 328);
      this.textBox9.Name = "textBox9";
      this.textBox9.ReadOnly = true;
      this.textBox9.Size = new Size(100, 20);
      this.textBox9.TabIndex = 26;
      this.textBox9.Tag = (object) "2143";
      this.textBox9.TextAlign = HorizontalAlignment.Right;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(3, 366);
      this.label13.Name = "label13";
      this.label13.Size = new Size(93, 13);
      this.label13.TabIndex = 42;
      this.label13.Text = "Base ARM Margin";
      this.labelTotalPrice.AutoSize = true;
      this.labelTotalPrice.Location = new Point(330, 332);
      this.labelTotalPrice.Name = "labelTotalPrice";
      this.labelTotalPrice.Size = new Size(113, 13);
      this.labelTotalPrice.TabIndex = 27;
      this.labelTotalPrice.Text = "Total Price Requested";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(3, 639);
      this.label14.Name = "label14";
      this.label14.Size = new Size(148, 13);
      this.label14.TabIndex = 41;
      this.label14.Text = "Total ARM Margin Requested";
      this.textBox1.BackColor = Color.WhiteSmoke;
      this.textBox1.Location = new Point(214, 7);
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new Size(100, 20);
      this.textBox1.TabIndex = 32;
      this.textBox1.Tag = (object) "2092";
      this.textBox1.TextAlign = HorizontalAlignment.Right;
      this.textBox12.BackColor = Color.WhiteSmoke;
      this.textBox12.Location = new Point(214, 636);
      this.textBox12.Name = "textBox12";
      this.textBox12.ReadOnly = true;
      this.textBox12.Size = new Size(100, 20);
      this.textBox12.TabIndex = 40;
      this.textBox12.Tag = (object) "2689";
      this.textBox12.TextAlign = HorizontalAlignment.Right;
      this.textBox44.BackColor = Color.WhiteSmoke;
      this.textBox44.Location = new Point(544, 7);
      this.textBox44.Name = "textBox44";
      this.textBox44.ReadOnly = true;
      this.textBox44.Size = new Size(100, 20);
      this.textBox44.TabIndex = 34;
      this.textBox44.Tag = (object) "2101";
      this.textBox44.TextAlign = HorizontalAlignment.Right;
      this.textBox13.BackColor = Color.WhiteSmoke;
      this.textBox13.Location = new Point(214, 614);
      this.textBox13.Name = "textBox13";
      this.textBox13.ReadOnly = true;
      this.textBox13.Size = new Size(100, 20);
      this.textBox13.TabIndex = 39;
      this.textBox13.Tag = (object) "2688";
      this.textBox13.TextAlign = HorizontalAlignment.Right;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(332, 369);
      this.label12.Name = "label12";
      this.label12.Size = new Size(56, 13);
      this.label12.TabIndex = 35;
      this.label12.Text = "Comments";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(3, 617);
      this.label15.Name = "label15";
      this.label15.Size = new Size(153, 13);
      this.label15.TabIndex = 38;
      this.label15.Text = "Total ARM Margin Adjustments";
      this.textBox10.BackColor = Color.WhiteSmoke;
      this.textBox10.Location = new Point(334, 387);
      this.textBox10.Multiline = true;
      this.textBox10.Name = "textBox10";
      this.textBox10.ReadOnly = true;
      this.textBox10.ScrollBars = ScrollBars.Both;
      this.textBox10.Size = new Size(310, 268);
      this.textBox10.TabIndex = 36;
      this.textBox10.Tag = (object) "2144";
      this.pnlLockExtension.Controls.Add((Control) this.textBox22);
      this.pnlLockExtension.Controls.Add((Control) this.label23);
      this.pnlLockExtension.Controls.Add((Control) this.textBox21);
      this.pnlLockExtension.Controls.Add((Control) this.label22);
      this.pnlLockExtension.Controls.Add((Control) this.textBox20);
      this.pnlLockExtension.Controls.Add((Control) this.label21);
      this.pnlLockExtension.Controls.Add((Control) this.textBox19);
      this.pnlLockExtension.Controls.Add((Control) this.label20);
      this.pnlLockExtension.Controls.Add((Control) this.textBox15);
      this.pnlLockExtension.Controls.Add((Control) this.textBox16);
      this.pnlLockExtension.Controls.Add((Control) this.textBox17);
      this.pnlLockExtension.Controls.Add((Control) this.label2);
      this.pnlLockExtension.Controls.Add((Control) this.label17);
      this.pnlLockExtension.Controls.Add((Control) this.textBox18);
      this.pnlLockExtension.Controls.Add((Control) this.label18);
      this.pnlLockExtension.Controls.Add((Control) this.label19);
      this.pnlLockExtension.Dock = DockStyle.Top;
      this.pnlLockExtension.Location = new Point(0, 100);
      this.pnlLockExtension.Name = "pnlLockExtension";
      this.pnlLockExtension.Size = new Size(659, 100);
      this.pnlLockExtension.TabIndex = 53;
      this.textBox22.BackColor = Color.WhiteSmoke;
      this.textBox22.Location = new Point(482, 73);
      this.textBox22.Name = "textBox22";
      this.textBox22.ReadOnly = true;
      this.textBox22.Size = new Size(100, 20);
      this.textBox22.TabIndex = 32;
      this.textBox22.Tag = (object) "3362";
      this.label23.AutoSize = true;
      this.label23.Location = new Point(333, 77);
      this.label23.Name = "label23";
      this.label23.Size = new Size(86, 13);
      this.label23.TabIndex = 31;
      this.label23.Text = "Price Adjustment";
      this.textBox21.BackColor = Color.WhiteSmoke;
      this.textBox21.Location = new Point(482, 50);
      this.textBox21.Name = "textBox21";
      this.textBox21.ReadOnly = true;
      this.textBox21.Size = new Size(100, 20);
      this.textBox21.TabIndex = 30;
      this.textBox21.Tag = (object) "3361";
      this.label22.AutoSize = true;
      this.label22.Location = new Point(333, 54);
      this.label22.Name = "label22";
      this.label22.Size = new Size(131, 13);
      this.label22.TabIndex = 29;
      this.label22.Text = "New Lock Expiration Date";
      this.textBox20.BackColor = Color.WhiteSmoke;
      this.textBox20.Location = new Point(482, 28);
      this.textBox20.Name = "textBox20";
      this.textBox20.ReadOnly = true;
      this.textBox20.Size = new Size(100, 20);
      this.textBox20.TabIndex = 28;
      this.textBox20.Tag = (object) "3360";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(333, 32);
      this.label21.Name = "label21";
      this.label21.Size = new Size(79, 13);
      this.label21.TabIndex = 27;
      this.label21.Text = "Days to Extend";
      this.textBox19.BackColor = Color.WhiteSmoke;
      this.textBox19.Location = new Point(482, 6);
      this.textBox19.Name = "textBox19";
      this.textBox19.ReadOnly = true;
      this.textBox19.Size = new Size(100, 20);
      this.textBox19.TabIndex = 26;
      this.textBox19.Tag = (object) "3369";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(333, 10);
      this.label20.Name = "label20";
      this.label20.Size = new Size(150, 13);
      this.label20.TabIndex = 25;
      this.label20.Text = "Previous Lock Expiration Date";
      this.textBox15.BackColor = Color.WhiteSmoke;
      this.textBox15.Location = new Point(155, 6);
      this.textBox15.Name = "textBox15";
      this.textBox15.ReadOnly = true;
      this.textBox15.Size = new Size(100, 20);
      this.textBox15.TabIndex = 19;
      this.textBox15.Tag = (object) "2088";
      this.textBox16.BackColor = Color.WhiteSmoke;
      this.textBox16.Location = new Point(155, 74);
      this.textBox16.Name = "textBox16";
      this.textBox16.ReadOnly = true;
      this.textBox16.Size = new Size(100, 20);
      this.textBox16.TabIndex = 24;
      this.textBox16.Tag = (object) "2091";
      this.textBox16.TextChanged += new EventHandler(this.textBox16_TextChanged);
      this.textBox17.BackColor = Color.WhiteSmoke;
      this.textBox17.Location = new Point(155, 51);
      this.textBox17.Name = "textBox17";
      this.textBox17.ReadOnly = true;
      this.textBox17.Size = new Size(100, 20);
      this.textBox17.TabIndex = 23;
      this.textBox17.Tag = (object) "2090";
      this.textBox17.TextAlign = HorizontalAlignment.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(5, 77);
      this.label2.Name = "label2";
      this.label2.Size = new Size(144, 13);
      this.label2.TabIndex = 22;
      this.label2.Text = "Original Lock Expiration Date";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(5, 54);
      this.label17.Name = "label17";
      this.label17.Size = new Size(106, 13);
      this.label17.TabIndex = 21;
      this.label17.Text = "Original Lock # Days";
      this.textBox18.BackColor = Color.WhiteSmoke;
      this.textBox18.Location = new Point(155, 29);
      this.textBox18.Name = "textBox18";
      this.textBox18.ReadOnly = true;
      this.textBox18.Size = new Size(100, 20);
      this.textBox18.TabIndex = 20;
      this.textBox18.Tag = (object) "2089";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(5, 32);
      this.label18.Name = "label18";
      this.label18.Size = new Size(95, 13);
      this.label18.TabIndex = 18;
      this.label18.Text = "Original Lock Date";
      this.label19.AutoSize = true;
      this.label19.Location = new Point(5, 10);
      this.label19.Name = "label19";
      this.label19.Size = new Size(75, 13);
      this.label19.TabIndex = 17;
      this.label19.Text = "Rate Sheet ID";
      this.pnlRegular.Controls.Add((Control) this.textBox2);
      this.pnlRegular.Controls.Add((Control) this.textBox14);
      this.pnlRegular.Controls.Add((Control) this.textBox5);
      this.pnlRegular.Controls.Add((Control) this.label16);
      this.pnlRegular.Controls.Add((Control) this.textBox4);
      this.pnlRegular.Controls.Add((Control) this.label6);
      this.pnlRegular.Controls.Add((Control) this.label7);
      this.pnlRegular.Controls.Add((Control) this.textBox3);
      this.pnlRegular.Controls.Add((Control) this.label4);
      this.pnlRegular.Controls.Add((Control) this.labelCommitment);
      this.pnlRegular.Controls.Add((Control) this.label5);
      this.pnlRegular.Controls.Add((Control) this.textBoxCommitment);
      this.pnlRegular.Dock = DockStyle.Top;
      this.pnlRegular.Location = new Point(0, 26);
      this.pnlRegular.Name = "pnlRegular";
      this.pnlRegular.Size = new Size(659, 74);
      this.pnlRegular.TabIndex = 52;
      this.textBox2.BackColor = Color.WhiteSmoke;
      this.textBox2.Location = new Point(98, 6);
      this.textBox2.Name = "textBox2";
      this.textBox2.ReadOnly = true;
      this.textBox2.Size = new Size(100, 20);
      this.textBox2.TabIndex = 11;
      this.textBox2.Tag = (object) "2088";
      this.textBox14.BackColor = Color.WhiteSmoke;
      this.textBox14.Location = new Point(459, 28);
      this.textBox14.Name = "textBox14";
      this.textBox14.ReadOnly = true;
      this.textBox14.Size = new Size(141, 20);
      this.textBox14.TabIndex = 50;
      this.textBox14.Tag = (object) "3256";
      this.textBox14.Visible = false;
      this.textBox5.BackColor = Color.WhiteSmoke;
      this.textBox5.Location = new Point(459, 6);
      this.textBox5.Name = "textBox5";
      this.textBox5.ReadOnly = true;
      this.textBox5.Size = new Size(141, 20);
      this.textBox5.TabIndex = 16;
      this.textBox5.Tag = (object) "2151";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(332, 32);
      this.label16.Name = "label16";
      this.label16.Size = new Size(98, 13);
      this.label16.TabIndex = 51;
      this.label16.Text = "Last Rate Set Date";
      this.label16.Visible = false;
      this.textBox4.BackColor = Color.WhiteSmoke;
      this.textBox4.Location = new Point(98, 50);
      this.textBox4.Name = "textBox4";
      this.textBox4.ReadOnly = true;
      this.textBox4.Size = new Size(100, 20);
      this.textBox4.TabIndex = 15;
      this.textBox4.Tag = (object) "2090";
      this.textBox4.TextAlign = HorizontalAlignment.Right;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(332, 10);
      this.label6.Name = "label6";
      this.label6.Size = new Size(106, 13);
      this.label6.TabIndex = 14;
      this.label6.Text = "Lock Expiration Date";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(5, 54);
      this.label7.Name = "label7";
      this.label7.Size = new Size(68, 13);
      this.label7.TabIndex = 13;
      this.label7.Text = "Lock # Days";
      this.textBox3.BackColor = Color.WhiteSmoke;
      this.textBox3.Location = new Point(98, 28);
      this.textBox3.Name = "textBox3";
      this.textBox3.ReadOnly = true;
      this.textBox3.Size = new Size(100, 20);
      this.textBox3.TabIndex = 12;
      this.textBox3.Tag = (object) "2089";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(5, 32);
      this.label4.Name = "label4";
      this.label4.Size = new Size(57, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "Lock Date";
      this.labelCommitment.AutoSize = true;
      this.labelCommitment.Location = new Point(332, 54);
      this.labelCommitment.Name = "labelCommitment";
      this.labelCommitment.Size = new Size(115, 13);
      this.labelCommitment.TabIndex = 30;
      this.labelCommitment.Text = "Investor Commitment #";
      this.labelCommitment.Visible = false;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(5, 10);
      this.label5.Name = "label5";
      this.label5.Size = new Size(75, 13);
      this.label5.TabIndex = 9;
      this.label5.Text = "Rate Sheet ID";
      this.textBoxCommitment.BackColor = Color.WhiteSmoke;
      this.textBoxCommitment.Location = new Point(459, 50);
      this.textBoxCommitment.Name = "textBoxCommitment";
      this.textBoxCommitment.ReadOnly = true;
      this.textBoxCommitment.Size = new Size(141, 20);
      this.textBoxCommitment.TabIndex = 29;
      this.textBoxCommitment.Tag = (object) "2215";
      this.textBoxCommitment.Visible = false;
      this.labelInfo.AutoSize = true;
      this.labelInfo.BackColor = Color.Transparent;
      this.labelInfo.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.labelInfo.Location = new Point(3, 7);
      this.labelInfo.Name = "labelInfo";
      this.labelInfo.Size = new Size(80, 14);
      this.labelInfo.TabIndex = 44;
      this.labelInfo.Text = "Rate Request";
      this.pnlConfirmed.Borders = AnchorStyles.Bottom | AnchorStyles.Right;
      this.pnlConfirmed.Controls.Add((Control) this.txtConfirmedBy);
      this.pnlConfirmed.Controls.Add((Control) this.txtConfirmedDate);
      this.pnlConfirmed.Controls.Add((Control) this.label28);
      this.pnlConfirmed.Controls.Add((Control) this.label27);
      this.pnlConfirmed.Controls.Add((Control) this.label24);
      this.pnlConfirmed.Controls.Add((Control) this.boxRequestedBy2);
      this.pnlConfirmed.Controls.Add((Control) this.boxDateRequested2);
      this.pnlConfirmed.Controls.Add((Control) this.textBoxRegistered2);
      this.pnlConfirmed.Controls.Add((Control) this.labelRegistered2);
      this.pnlConfirmed.Controls.Add((Control) this.label26);
      this.pnlConfirmed.Location = new Point(1, 60);
      this.pnlConfirmed.Name = "pnlConfirmed";
      this.pnlConfirmed.Size = new Size(659, 84);
      this.pnlConfirmed.TabIndex = 2;
      this.txtConfirmedBy.BackColor = Color.WhiteSmoke;
      this.txtConfirmedBy.Location = new Point(451, 31);
      this.txtConfirmedBy.Name = "txtConfirmedBy";
      this.txtConfirmedBy.ReadOnly = true;
      this.txtConfirmedBy.Size = new Size(152, 20);
      this.txtConfirmedBy.TabIndex = 18;
      this.txtConfirmedDate.BackColor = Color.WhiteSmoke;
      this.txtConfirmedDate.Location = new Point(451, 10);
      this.txtConfirmedDate.Name = "txtConfirmedDate";
      this.txtConfirmedDate.ReadOnly = true;
      this.txtConfirmedDate.Size = new Size(152, 20);
      this.txtConfirmedDate.TabIndex = 17;
      this.label28.AutoSize = true;
      this.label28.Location = new Point(337, 31);
      this.label28.Name = "label28";
      this.label28.Size = new Size(69, 13);
      this.label28.TabIndex = 16;
      this.label28.Text = "Confirmed By";
      this.label27.AutoSize = true;
      this.label27.Location = new Point(337, 10);
      this.label27.Name = "label27";
      this.label27.Size = new Size(108, 13);
      this.label27.TabIndex = 15;
      this.label27.Text = "Confirmed Date/Time";
      this.label24.AutoSize = true;
      this.label24.Location = new Point(10, 14);
      this.label24.Name = "label24";
      this.label24.Size = new Size(113, 13);
      this.label24.TabIndex = 7;
      this.label24.Text = "Requested Date/Time";
      this.boxRequestedBy2.BackColor = Color.WhiteSmoke;
      this.boxRequestedBy2.Location = new Point(128, 31);
      this.boxRequestedBy2.Name = "boxRequestedBy2";
      this.boxRequestedBy2.ReadOnly = true;
      this.boxRequestedBy2.Size = new Size(192, 20);
      this.boxRequestedBy2.TabIndex = 10;
      this.boxDateRequested2.BackColor = Color.WhiteSmoke;
      this.boxDateRequested2.Location = new Point(128, 10);
      this.boxDateRequested2.Name = "boxDateRequested2";
      this.boxDateRequested2.ReadOnly = true;
      this.boxDateRequested2.Size = new Size(192, 20);
      this.boxDateRequested2.TabIndex = 4;
      this.textBoxRegistered2.BackColor = Color.WhiteSmoke;
      this.textBoxRegistered2.Location = new Point(451, 52);
      this.textBoxRegistered2.Name = "textBoxRegistered2";
      this.textBoxRegistered2.ReadOnly = true;
      this.textBoxRegistered2.Size = new Size(152, 20);
      this.textBoxRegistered2.TabIndex = 14;
      this.textBoxRegistered2.Tag = (object) "";
      this.textBoxRegistered2.Visible = false;
      this.labelRegistered2.AutoSize = true;
      this.labelRegistered2.Location = new Point(337, 55);
      this.labelRegistered2.Name = "labelRegistered2";
      this.labelRegistered2.Size = new Size(73, 13);
      this.labelRegistered2.TabIndex = 13;
      this.labelRegistered2.Text = "Registered By";
      this.labelRegistered2.Visible = false;
      this.label26.AutoSize = true;
      this.label26.Location = new Point(10, 35);
      this.label26.Name = "label26";
      this.label26.Size = new Size(74, 13);
      this.label26.TabIndex = 9;
      this.label26.Text = "Requested By";
      this.pnlRequested.Borders = AnchorStyles.Bottom | AnchorStyles.Right;
      this.pnlRequested.Controls.Add((Control) this.label1);
      this.pnlRequested.Controls.Add((Control) this.boxRequestedBy);
      this.pnlRequested.Controls.Add((Control) this.boxDateRequested);
      this.pnlRequested.Controls.Add((Control) this.textBoxRegistered);
      this.pnlRequested.Controls.Add((Control) this.labelRegistered);
      this.pnlRequested.Controls.Add((Control) this.label3);
      this.pnlRequested.Location = new Point(0, 0);
      this.pnlRequested.Name = "pnlRequested";
      this.pnlRequested.Size = new Size(660, 60);
      this.pnlRequested.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(113, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Requested Date/Time";
      this.boxRequestedBy.BackColor = Color.WhiteSmoke;
      this.boxRequestedBy.Location = new Point(424, 10);
      this.boxRequestedBy.Name = "boxRequestedBy";
      this.boxRequestedBy.ReadOnly = true;
      this.boxRequestedBy.Size = new Size(180, 20);
      this.boxRequestedBy.TabIndex = 10;
      this.boxDateRequested.BackColor = Color.WhiteSmoke;
      this.boxDateRequested.Location = new Point(128, 10);
      this.boxDateRequested.Name = "boxDateRequested";
      this.boxDateRequested.ReadOnly = true;
      this.boxDateRequested.Size = new Size(192, 20);
      this.boxDateRequested.TabIndex = 4;
      this.textBoxRegistered.BackColor = Color.WhiteSmoke;
      this.textBoxRegistered.Location = new Point(424, 32);
      this.textBoxRegistered.Name = "textBoxRegistered";
      this.textBoxRegistered.ReadOnly = true;
      this.textBoxRegistered.Size = new Size(180, 20);
      this.textBoxRegistered.TabIndex = 14;
      this.textBoxRegistered.Tag = (object) "";
      this.textBoxRegistered.Visible = false;
      this.labelRegistered.AutoSize = true;
      this.labelRegistered.Location = new Point(337, 34);
      this.labelRegistered.Name = "labelRegistered";
      this.labelRegistered.Size = new Size(73, 13);
      this.labelRegistered.TabIndex = 13;
      this.labelRegistered.Text = "Registered By";
      this.labelRegistered.Visible = false;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(337, 13);
      this.label3.Name = "label3";
      this.label3.Size = new Size(74, 13);
      this.label3.TabIndex = 9;
      this.label3.Text = "Requested By";
      this.clearAlertBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.clearAlertBtn.Location = new Point(591, 2);
      this.clearAlertBtn.Name = "clearAlertBtn";
      this.clearAlertBtn.Size = new Size(76, 22);
      this.clearAlertBtn.TabIndex = 3;
      this.clearAlertBtn.Text = "Clear &Alert";
      this.clearAlertBtn.Click += new EventHandler(this.clearAlertBtn_Click);
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.groupContainerAll);
      this.Name = nameof (LockRequestWS);
      this.Size = new Size(673, 1242);
      this.groupContainerAll.ResumeLayout(false);
      this.panelInside.ResumeLayout(false);
      this.groupContainerMiddle.ResumeLayout(false);
      this.groupContainerMiddle.PerformLayout();
      this.pnlDetail.ResumeLayout(false);
      this.pnlDetail.PerformLayout();
      this.panelPriceConcession.ResumeLayout(false);
      this.panelPriceConcession.PerformLayout();
      this.pnlLockExtension.ResumeLayout(false);
      this.pnlLockExtension.PerformLayout();
      this.pnlRegular.ResumeLayout(false);
      this.pnlRegular.PerformLayout();
      this.pnlConfirmed.ResumeLayout(false);
      this.pnlConfirmed.PerformLayout();
      this.pnlRequested.ResumeLayout(false);
      this.pnlRequested.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
