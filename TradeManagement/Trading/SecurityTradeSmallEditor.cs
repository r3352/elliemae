// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SecurityTradeSmallEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class SecurityTradeSmallEditor : UserControl
  {
    private string className = nameof (SecurityTradeSmallEditor);
    private static string sw = Tracing.SwOutsideLoan;
    private SecurityTradeInfo trade;
    private SecurityTradeAssignment[] assignments;
    private SecurityTradeAssignment assignment;
    private LoanTradeInfo assigneeTrade;
    private int term1 = -1;
    private int term2 = -1;
    private bool modified;
    private bool ignoreEvent;
    private bool isLoanTradeReadonly;
    private IContainer components;
    private ToolTip toolTips;
    private GroupContainer grpTradeInfo;
    private Label label6;
    private DatePicker dpNotificationDate;
    private Label label5;
    private DatePicker dpSettlementDate;
    private DatePicker dpConfirmDate;
    private Label label2;
    private Label label21;
    private TextBox txtPrice;
    private Label label17;
    private TextBox txtCoupon;
    private Label label14;
    private ComboBox cboSecurityType;
    private DatePicker dpAssignedDate;
    private Label label24;
    private Label label1;
    private ComboBox cboName;
    private Panel panel1;
    private DealerEditorControl ctlDealer;
    private TabControl tabSecurityTrade;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private Label label15;
    private ComboBox cboProgramType;

    public event EventHandler<TermMonthsUpdatedEventArgs> TermMonthsUpdated;

    public event EventHandler SecurityPriceUpdated;

    public event EventHandler SecurityCouponUpdated;

    public SecurityTradeSmallEditor()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtCoupon, TextBoxContentRule.NonNegativeDecimal, "#,##0.00000;;\"\"");
      TextBoxFormatter.Attach(this.txtPrice, TextBoxContentRule.NonNegativeDecimal, "#,##0.0000000;;\"\"");
      this.refreshConfigurableFieldOptions();
    }

    public SecurityTradeInfo SecurityTrade => this.trade;

    public bool DataModified
    {
      get
      {
        return this.modified && !this.isEmpty() || this.ctlDealer.DataModified && !this.ctlDealer.isEmpty();
      }
    }

    public bool ReadOnly
    {
      get
      {
        if (this.isLoanTradeReadonly)
          return true;
        return this.trade != null && this.trade.TradeID >= 0;
      }
      set => this.isLoanTradeReadonly = value;
    }

    public void Init(SecurityTradeInfo trade, LoanTradeInfo assigneeTrade)
    {
      this.ignoreEvent = true;
      this.trade = trade;
      this.assigneeTrade = assigneeTrade;
      this.loadTradeData();
      this.ignoreEvent = false;
    }

    public bool SaveTrade()
    {
      return this.assigneeTrade != null && this.modified && this.SaveTrade(this.assigneeTrade);
    }

    public bool SaveTrade(LoanTradeInfo assigneeTrade)
    {
      bool flag = true;
      if (!this.DataModified)
        return true;
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (assigneeTrade != null)
          this.assigneeTrade = assigneeTrade;
        this.CommitChanges();
        if (!this.ValidateTradeData())
          return false;
        this.saveTradeInfo();
        this.saveAssignment();
        this.loadTradeData();
        return flag;
      }
      catch (ObjectNotFoundException ex)
      {
        Tracing.Log(SecurityTradeSmallEditor.sw, this.className, TraceLevel.Error, ex.ToString());
        if (ex.ObjectType == ObjectType.Trade)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The current trade has been deleted and cannot be saved. All changes made to this trade will be lost.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The security trade could not be saved due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.modified = false;
        return false;
      }
      catch (Exception ex)
      {
        Tracing.Log(SecurityTradeSmallEditor.sw, this.className, TraceLevel.Error, ex.ToString());
        int num = !ex.Message.Contains("The loan trade has been allocated to security trade") ? (int) Utils.Dialog((IWin32Window) this, "The security trade could not be saved due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation) : throw ex;
        this.modified = false;
        return false;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    public void RefreshContents()
    {
      this.loadTradeData();
      this.setControlsState();
    }

    private void setControlsState()
    {
      if (this.ReadOnly)
      {
        if (this.isLoanTradeReadonly)
          this.cboName.Enabled = false;
        else if (this.assignment == null || this.assigneeTrade == null)
          this.cboName.Enabled = true;
        else
          this.cboName.Enabled = false;
      }
      else
        this.cboName.Enabled = true;
      this.cboSecurityType.Enabled = !this.ReadOnly;
      this.cboProgramType.Enabled = !this.ReadOnly;
      this.txtCoupon.ReadOnly = this.ReadOnly;
      this.txtPrice.ReadOnly = this.ReadOnly;
      this.ctlDealer.ReadOnly = this.ReadOnly;
      this.dpAssignedDate.ReadOnly = this.ReadOnly;
      this.dpConfirmDate.ReadOnly = this.ReadOnly;
      this.dpSettlementDate.ReadOnly = this.ReadOnly;
      this.dpNotificationDate.ReadOnly = this.ReadOnly;
    }

    public void CommitChanges()
    {
      this.modified = this.DataModified;
      if (!this.modified)
        return;
      this.onFieldValueChanged((object) this.cboName, (EventArgs) null);
      if (this.trade == null)
      {
        this.trade = new SecurityTradeInfo();
        this.ctlDealer.CommitChanges();
        this.trade.Name = this.cboName.Text.Trim();
        this.trade.SecurityType = this.cboSecurityType.Text;
        this.trade.ProgramType = this.cboProgramType.Text;
        this.trade.Term1 = this.term1;
        this.trade.Term2 = this.term2;
        this.trade.Coupon = Utils.ParseDecimal((object) this.txtCoupon.Text);
        this.trade.Price = Utils.ParseDecimal((object) this.txtPrice.Text);
        this.trade.DealerName = this.ctlDealer.Dealer.EntityName;
        this.trade.Dealer = this.ctlDealer.Dealer;
        this.trade.ConfirmDate = this.dpConfirmDate.Value;
        this.trade.SettlementDate = this.dpSettlementDate.Value;
        this.trade.NotificationDate = this.dpNotificationDate.Value;
      }
      if (this.assignment == null)
        return;
      if (this.trade != null && this.trade.Name != string.Empty && this.dpAssignedDate.Text.Trim() == "")
        this.dpAssignedDate.Value = DateTime.Today;
      this.assignment = new SecurityTradeAssignment(this.trade, this.assigneeTrade, SecurityLoanTradeStatus.Assigned, DateTime.MinValue);
      this.assignment.AssignedStatusDate = this.dpAssignedDate.Value;
    }

    private void saveTradeInfo()
    {
      int tradeId = this.trade.TradeID;
      if (tradeId < 0)
      {
        this.trade.IsHidden = true;
        tradeId = Session.SecurityTradeManager.CreateTrade(this.trade);
      }
      this.trade = Session.SecurityTradeManager.GetTrade(tradeId);
      if (this.assignment == null)
      {
        this.assignment = new SecurityTradeAssignment(this.trade, this.assigneeTrade);
        if (this.dpAssignedDate.Text.Trim() != "")
          this.assignment.AssignedStatusDate = this.dpAssignedDate.Value;
      }
      this.setControlsState();
    }

    private void saveAssignment()
    {
      if (this.assignment == null)
        return;
      if (this.dpAssignedDate.Text.Trim() != "")
        this.assignment.AssignedStatusDate = DateTime.Parse(this.dpAssignedDate.Text);
      Session.SecurityTradeManager.AssignLoanTradeToTrade(this.trade.TradeID, this.assigneeTrade.TradeID, SecurityLoanTradeStatus.Assigned, this.assignment.AssignedStatusDate);
    }

    public bool isEmpty()
    {
      return this.cboName.Text.Trim() == "" && this.cboSecurityType.Text.Trim() == "" && this.cboProgramType.Text.Trim() == "" && this.txtCoupon.Text.Trim() == "" && this.txtPrice.Text.Trim() == "" && this.dpAssignedDate.Text.Trim() == "" && this.dpSettlementDate.Text.Trim() == "" && this.dpNotificationDate.Text.Trim() == "" && this.dpConfirmDate.Text.Trim() == "";
    }

    public bool ValidateTradeData()
    {
      if (this.ReadOnly || !this.modified || this.isEmpty() && this.ctlDealer.isEmpty() || this.cboName.Text.Trim().Length != 0)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a name/number for this trade before saving.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }

    private void cboName_TextChanged(object sender, EventArgs e)
    {
      if (this.ignoreEvent)
        return;
      if (this.trade != null && this.trade.Name != this.cboName.Text)
      {
        this.clearFields();
        this.trade = (SecurityTradeInfo) null;
        this.ReadOnly = false;
      }
      SecurityTradeInfo tradeByName = Session.SecurityTradeManager.GetTradeByName(this.cboName.Text);
      if (tradeByName != null)
      {
        this.trade = tradeByName;
        this.loadTradeData();
        this.OnTermMonthsUpdated(new TermMonthsUpdatedEventArgs(this.term1, this.term2));
      }
      this.setControlsState();
      this.OnSecurityPriceUpdated();
      this.OnSecurityCouponUpdated();
      this.modified = true;
    }

    private void onFieldValueChanged(object sender, EventArgs e)
    {
      if (this.ignoreEvent)
        return;
      if (sender == this.cboName)
        this.cboName_TextChanged(sender, e);
      else if (sender == this.txtPrice)
        this.OnSecurityPriceUpdated();
      else if (sender == this.txtCoupon)
        this.OnSecurityCouponUpdated();
      this.modified = true;
    }

    private void cboSecurityType_SelectedValueChanged(object sender, EventArgs e)
    {
      int term1 = this.term1;
      int term2 = this.term2;
      if (this.ignoreEvent)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) Session.ConfigurationManager.GetSecondarySecurityTypes().Rows)
      {
        if (this.cboSecurityType.Text == (string) row["Name"])
        {
          this.cboProgramType.Text = (string) row["ProgramType"];
          int num1 = (int) row["Term1"];
          int num2 = (int) row["Term2"];
          if (num1 != this.term1 || num2 != this.term2)
          {
            this.term1 = num1;
            this.term2 = num2;
            this.OnTermMonthsUpdated(new TermMonthsUpdatedEventArgs(this.term1, this.term2));
          }
        }
      }
    }

    private void dpSettlementDate_Load(object sender, EventArgs e)
    {
    }

    private void loadTradeData()
    {
      if (this.trade == null)
      {
        this.clearFields();
        this.cboName.Items.Clear();
        SecurityTradeInfo[] activeTrades = Session.SecurityTradeManager.GetActiveTrades();
        if (activeTrades != null)
        {
          foreach (SecurityTradeInfo securityTradeInfo in activeTrades)
          {
            if (!securityTradeInfo.Locked)
              this.cboName.Items.Add((object) securityTradeInfo.Name);
          }
        }
        this.cboName.Text = "";
        this.assignment = (SecurityTradeAssignment) null;
      }
      else
      {
        SecurityTradeEditorScreenData editorScreenData = Session.SecurityTradeManager.GetTradeEditorScreenData(this.trade.TradeID);
        if (editorScreenData.Trade != null)
          this.trade = editorScreenData.Trade;
        if (editorScreenData.Assignments != null)
          this.assignments = editorScreenData.Assignments;
        this.cboName.Text = this.trade.Name;
        this.cboSecurityType.Text = this.trade.SecurityType;
        this.cboProgramType.Text = this.trade.ProgramType;
        this.term1 = this.trade.Term1;
        this.term2 = this.trade.Term2;
        this.txtCoupon.Text = this.trade.Coupon.ToString("#,##0.00000;;\"\"");
        this.txtPrice.Text = this.trade.Price.ToString("#,##0.0000000;;\"\"");
        if (this.dpAssignedDate.Text.Trim() == "")
          this.dpAssignedDate.Value = DateTime.Today;
        if (this.assignments != null && this.assigneeTrade != null)
        {
          foreach (SecurityTradeAssignment assignment in this.assignments)
          {
            if (assignment.AssigneeTrade.TradeID == this.assigneeTrade.TradeID)
            {
              this.assignment = assignment;
              this.dpAssignedDate.Value = this.assignment.AssignedStatusDate;
              break;
            }
          }
        }
        this.dpConfirmDate.Value = this.trade.ConfirmDate;
        this.dpSettlementDate.Value = this.trade.SettlementDate;
        this.dpNotificationDate.Value = this.trade.NotificationDate;
        this.loadDealerData();
        this.setControlsState();
        this.modified = this.trade.TradeID <= 0;
      }
    }

    private void loadDealerData() => this.ctlDealer.Init(this.trade.Dealer);

    private void clearFields()
    {
      this.cboSecurityType.Text = "";
      this.cboProgramType.Text = "";
      this.txtCoupon.Text = "";
      this.txtPrice.Text = "";
      this.dpAssignedDate.Text = "";
      this.dpSettlementDate.Text = "";
      this.dpNotificationDate.Text = "";
      this.dpConfirmDate.Text = "";
      this.ctlDealer.Clear();
    }

    private void refreshConfigurableFieldOptions()
    {
      this.cboSecurityType.Items.Clear();
      if (!Session.IsConnected)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) Session.ConfigurationManager.GetSecondarySecurityTypes().Rows)
        this.cboSecurityType.Items.Add(row["Name"]);
    }

    protected virtual void OnTermMonthsUpdated(TermMonthsUpdatedEventArgs e)
    {
      EventHandler<TermMonthsUpdatedEventArgs> termMonthsUpdated = this.TermMonthsUpdated;
      if (termMonthsUpdated == null)
        return;
      termMonthsUpdated((object) this, e);
    }

    protected virtual void OnSecurityPriceUpdated()
    {
      if (this.SecurityPriceUpdated == null)
        return;
      this.SecurityPriceUpdated((object) this.txtPrice.Text, (EventArgs) null);
    }

    protected virtual void OnSecurityCouponUpdated()
    {
      if (this.SecurityCouponUpdated == null)
        return;
      this.SecurityCouponUpdated((object) this.txtCoupon.Text, (EventArgs) null);
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
      this.toolTips = new ToolTip(this.components);
      this.grpTradeInfo = new GroupContainer();
      this.tabSecurityTrade = new TabControl();
      this.tabPage1 = new TabPage();
      this.panel1 = new Panel();
      this.label15 = new Label();
      this.cboProgramType = new ComboBox();
      this.label1 = new Label();
      this.dpConfirmDate = new DatePicker();
      this.label2 = new Label();
      this.cboName = new ComboBox();
      this.label6 = new Label();
      this.label24 = new Label();
      this.dpNotificationDate = new DatePicker();
      this.dpAssignedDate = new DatePicker();
      this.label5 = new Label();
      this.cboSecurityType = new ComboBox();
      this.dpSettlementDate = new DatePicker();
      this.label14 = new Label();
      this.txtCoupon = new TextBox();
      this.label17 = new Label();
      this.label21 = new Label();
      this.txtPrice = new TextBox();
      this.tabPage2 = new TabPage();
      this.ctlDealer = new DealerEditorControl();
      this.grpTradeInfo.SuspendLayout();
      this.tabSecurityTrade.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.SuspendLayout();
      this.grpTradeInfo.Controls.Add((Control) this.tabSecurityTrade);
      this.grpTradeInfo.Dock = DockStyle.Fill;
      this.grpTradeInfo.HeaderForeColor = SystemColors.ControlText;
      this.grpTradeInfo.Location = new Point(0, 0);
      this.grpTradeInfo.Name = "grpTradeInfo";
      this.grpTradeInfo.Size = new Size(309, 296);
      this.grpTradeInfo.TabIndex = 1;
      this.grpTradeInfo.Text = "Security Trade Information";
      this.tabSecurityTrade.Controls.Add((Control) this.tabPage1);
      this.tabSecurityTrade.Controls.Add((Control) this.tabPage2);
      this.tabSecurityTrade.Dock = DockStyle.Fill;
      this.tabSecurityTrade.Location = new Point(1, 26);
      this.tabSecurityTrade.Margin = new Padding(0);
      this.tabSecurityTrade.Name = "tabSecurityTrade";
      this.tabSecurityTrade.SelectedIndex = 0;
      this.tabSecurityTrade.Size = new Size(307, 269);
      this.tabSecurityTrade.TabIndex = 184;
      this.tabPage1.Controls.Add((Control) this.panel1);
      this.tabPage1.Location = new Point(4, 23);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(299, 242);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Details";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.panel1.AutoScroll = true;
      this.panel1.BackColor = Color.WhiteSmoke;
      this.panel1.Controls.Add((Control) this.label15);
      this.panel1.Controls.Add((Control) this.cboProgramType);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.dpConfirmDate);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.cboName);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.label24);
      this.panel1.Controls.Add((Control) this.dpNotificationDate);
      this.panel1.Controls.Add((Control) this.dpAssignedDate);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.cboSecurityType);
      this.panel1.Controls.Add((Control) this.dpSettlementDate);
      this.panel1.Controls.Add((Control) this.label14);
      this.panel1.Controls.Add((Control) this.txtCoupon);
      this.panel1.Controls.Add((Control) this.label17);
      this.panel1.Controls.Add((Control) this.label21);
      this.panel1.Controls.Add((Control) this.txtPrice);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(293, 236);
      this.panel1.TabIndex = 182;
      this.label15.AutoSize = true;
      this.label15.Location = new Point(7, 56);
      this.label15.Name = "label15";
      this.label15.Size = new Size(73, 14);
      this.label15.TabIndex = 109;
      this.label15.Text = "Program Type";
      this.cboProgramType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboProgramType.FormattingEnabled = true;
      this.cboProgramType.Items.AddRange(new object[2]
      {
        (object) "Fixed",
        (object) "Adjustable"
      });
      this.cboProgramType.Location = new Point(130, 54);
      this.cboProgramType.Name = "cboProgramType";
      this.cboProgramType.Size = new Size(132, 22);
      this.cboProgramType.TabIndex = 102;
      this.cboProgramType.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(59, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Security ID";
      this.dpConfirmDate.BackColor = SystemColors.Window;
      this.dpConfirmDate.Location = new Point(130, 195);
      this.dpConfirmDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpConfirmDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpConfirmDate.Name = "dpConfirmDate";
      this.dpConfirmDate.Size = new Size(104, 22);
      this.dpConfirmDate.TabIndex = 108;
      this.dpConfirmDate.ToolTip = "";
      this.dpConfirmDate.Value = new DateTime(0L);
      this.dpConfirmDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 196);
      this.label2.Name = "label2";
      this.label2.Size = new Size(69, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Confirm Date";
      this.cboName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboName.FormattingEnabled = true;
      this.cboName.ItemHeight = 14;
      this.cboName.Location = new Point(130, 5);
      this.cboName.Name = "cboName";
      this.cboName.Size = new Size(132, 22);
      this.cboName.TabIndex = 100;
      this.cboName.TextChanged += new EventHandler(this.cboName_TextChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(7, 173);
      this.label6.Name = "label6";
      this.label6.Size = new Size(85, 14);
      this.label6.TabIndex = 0;
      this.label6.Text = "Notification Date";
      this.label24.AutoSize = true;
      this.label24.Location = new Point(7, 125);
      this.label24.Name = "label24";
      this.label24.Size = new Size(78, 14);
      this.label24.TabIndex = 0;
      this.label24.Text = "Assigned Date";
      this.dpNotificationDate.BackColor = SystemColors.Window;
      this.dpNotificationDate.Location = new Point(130, 172);
      this.dpNotificationDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpNotificationDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpNotificationDate.Name = "dpNotificationDate";
      this.dpNotificationDate.Size = new Size(104, 22);
      this.dpNotificationDate.TabIndex = 107;
      this.dpNotificationDate.ToolTip = "";
      this.dpNotificationDate.Value = new DateTime(0L);
      this.dpNotificationDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.dpAssignedDate.BackColor = SystemColors.Window;
      this.dpAssignedDate.Location = new Point(130, 124);
      this.dpAssignedDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpAssignedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpAssignedDate.Name = "dpAssignedDate";
      this.dpAssignedDate.Size = new Size(104, 22);
      this.dpAssignedDate.TabIndex = 105;
      this.dpAssignedDate.ToolTip = "";
      this.dpAssignedDate.Value = new DateTime(0L);
      this.dpAssignedDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(7, 150);
      this.label5.Name = "label5";
      this.label5.Size = new Size(82, 14);
      this.label5.TabIndex = 0;
      this.label5.Text = "Settlement Date";
      this.cboSecurityType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSecurityType.FormattingEnabled = true;
      this.cboSecurityType.Items.AddRange(new object[4]
      {
        (object) "FNMA",
        (object) "FHLMC",
        (object) "GNMA I",
        (object) "GNMA II"
      });
      this.cboSecurityType.Location = new Point(130, 30);
      this.cboSecurityType.Name = "cboSecurityType";
      this.cboSecurityType.Size = new Size(132, 22);
      this.cboSecurityType.TabIndex = 101;
      this.cboSecurityType.SelectedValueChanged += new EventHandler(this.cboSecurityType_SelectedValueChanged);
      this.dpSettlementDate.BackColor = SystemColors.Window;
      this.dpSettlementDate.Location = new Point(130, 149);
      this.dpSettlementDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpSettlementDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpSettlementDate.Name = "dpSettlementDate";
      this.dpSettlementDate.Size = new Size(104, 22);
      this.dpSettlementDate.TabIndex = 106;
      this.dpSettlementDate.ToolTip = "";
      this.dpSettlementDate.Value = new DateTime(0L);
      this.dpSettlementDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.dpSettlementDate.Load += new EventHandler(this.dpSettlementDate_Load);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(7, 32);
      this.label14.Name = "label14";
      this.label14.Size = new Size(73, 14);
      this.label14.TabIndex = 0;
      this.label14.Text = "Security Type";
      this.txtCoupon.Location = new Point(130, 78);
      this.txtCoupon.MaxLength = 12;
      this.txtCoupon.Name = "txtCoupon";
      this.txtCoupon.Size = new Size(93, 20);
      this.txtCoupon.TabIndex = 103;
      this.txtCoupon.TextAlign = HorizontalAlignment.Right;
      this.txtCoupon.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label17.AutoSize = true;
      this.label17.Location = new Point(7, 79);
      this.label17.Name = "label17";
      this.label17.Size = new Size(44, 14);
      this.label17.TabIndex = 0;
      this.label17.Text = "Coupon";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(7, 102);
      this.label21.Name = "label21";
      this.label21.Size = new Size(31, 14);
      this.label21.TabIndex = 0;
      this.label21.Text = "Price";
      this.txtPrice.Location = new Point(130, 101);
      this.txtPrice.MaxLength = 12;
      this.txtPrice.Name = "txtPrice";
      this.txtPrice.Size = new Size(93, 20);
      this.txtPrice.TabIndex = 104;
      this.txtPrice.TextAlign = HorizontalAlignment.Right;
      this.txtPrice.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.tabPage2.Controls.Add((Control) this.ctlDealer);
      this.tabPage2.Location = new Point(4, 23);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new Padding(3);
      this.tabPage2.Size = new Size(299, 242);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Dealer";
      this.tabPage2.UseVisualStyleBackColor = true;
      this.ctlDealer.BackColor = Color.WhiteSmoke;
      this.ctlDealer.Dock = DockStyle.Fill;
      this.ctlDealer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlDealer.Location = new Point(3, 3);
      this.ctlDealer.Name = "ctlDealer";
      this.ctlDealer.ReadOnly = false;
      this.ctlDealer.Size = new Size(293, 236);
      this.ctlDealer.TabIndex = 183;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.grpTradeInfo);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (SecurityTradeSmallEditor);
      this.Size = new Size(309, 296);
      this.grpTradeInfo.ResumeLayout(false);
      this.tabSecurityTrade.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
