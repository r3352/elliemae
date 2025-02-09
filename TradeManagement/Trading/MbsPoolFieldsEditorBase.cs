// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolFieldsEditorBase
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class MbsPoolFieldsEditorBase : UserControl, IMbsPoolFieldsEditor
  {
    protected const string ERR_InvestorRemittanceDay = "Valid entry for \"Investor Remittance Day\" is a number 1-31.";
    protected const string ERR_ScheduledRemittancePaymentDay = "Valid entry for \"Scheduled Remittance Payment Day\" is a number 1-31.";
    protected const string ERR_Percent24 = "Valid enter is a decimal less than 100";
    protected const string ERR_Percent34 = "Valid enter is a decimal less than 1000";
    protected bool loading;
    protected bool modified;
    protected bool readOnly;
    protected string originalTradeName = string.Empty;
    protected string investorTemplateName = string.Empty;
    protected bool loanUpdatableFieldChanged;
    protected MbsPoolInfo poolTrade;
    private IContainer components;

    public event ValueChangedEventHandler ValueChanged;

    public MbsPoolInfo PoolTrade
    {
      get => this.poolTrade;
      set => this.poolTrade = value;
    }

    public bool DataModified => this.modified;

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        this.readOnly = value;
        this.setReadOnly();
      }
    }

    public bool LoanUpdatableFieldChanged => this.loanUpdatableFieldChanged;

    public PipelineInfo[] CurrentInTradeLoans { get; set; }

    public MbsPoolFieldsEditorBase() => this.InitializeComponent();

    public MbsPoolFieldsEditorBase(PipelineInfo[] currentInTradeLoans)
      : this()
    {
      this.CurrentInTradeLoans = currentInTradeLoans;
    }

    public bool ValidateChanges(bool popupMgs = true) => this.queryCommitChanges(popupMgs);

    public void CommitChanges()
    {
      this.commitChanges();
      this.modified = false;
    }

    public void LoadTradeData()
    {
      this.loadTradeData();
      this.modified = false;
      this.loanUpdatableFieldChanged = false;
    }

    protected virtual void loadTradeData()
    {
    }

    protected virtual void setReadOnly()
    {
    }

    protected virtual bool queryCommitChanges(bool popupMgs) => true;

    protected virtual void commitChanges()
    {
    }

    protected virtual void LoadConfigurableFieldOptions()
    {
    }

    protected void OnFieldValueChanged(object sender, EventArgs e)
    {
      this.modified = true;
      if (!(sender is TextBox) || !(((Control) sender).Name == "txtRemittancePaymentDay") && !(((Control) sender).Name == "txtInvestorRemittanceDay"))
        return;
      TextBox textBox = (TextBox) sender;
      if (textBox.Text.Length <= 0 || Regex.IsMatch(textBox.Text, "^[0-9-]*$"))
        return;
      textBox.Text = textBox.Text.Substring(0, textBox.Text.Length - 1);
      textBox.Select(textBox.Text.Length, 0);
    }

    protected void OnLoanUpdatableFieldValueChanged(object sender, EventArgs e)
    {
      this.loanUpdatableFieldChanged = true;
      this.OnFieldValueChanged(sender, e);
    }

    private bool ValidateFormat(ValidateMode mode, string sDate)
    {
      return mode != ValidateMode.Day || sDate.Length == 0 || Regex.IsMatch(sDate, "^[0-9]{1,2}$") || Regex.IsMatch(sDate, "^---[0-9]{1,2}$");
    }

    private bool ValidateData(ValidateMode mode, string text)
    {
      if (text.Length == 0)
        return true;
      switch (mode)
      {
        case ValidateMode.Day:
          if (Regex.IsMatch(text, "^---[0-9]{1,2}$"))
            text = text.Substring(3);
          int num = Utils.ParseInt((object) text, 0);
          if (num < 1 || num > 31)
            return false;
          break;
        case ValidateMode.Percent24:
          if (Utils.ParseDecimal((object) text) >= 100M)
            return false;
          break;
        case ValidateMode.Percent34:
          if (Utils.ParseDecimal((object) text) >= 1000M)
            return false;
          break;
      }
      return true;
    }

    protected bool ValidateEditor(object obj, ValidateMode mode, string errMsg, bool needFocus = true)
    {
      bool flag = true;
      if (obj is TextBox textBox)
      {
        if (!this.ValidateFormat(mode, textBox.Text) || !this.ValidateData(mode, textBox.Text))
        {
          if (errMsg.Length > 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, errMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          flag = false;
        }
        if (!flag)
        {
          textBox.BackColor = Color.LightYellow;
          if (needFocus)
            textBox.Focus();
        }
        else
          textBox.BackColor = Color.White;
      }
      return flag;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() => this.components = (IContainer) new System.ComponentModel.Container();
  }
}
