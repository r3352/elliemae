// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.AddSellDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class AddSellDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private ConditionLog[] condList;
    private bool isImportCondtions;
    private bool displayingImportCondition;
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private RadioButton rdoImport;
    private RadioButton rdoNew;
    private RadioButton rdImportAll;

    public bool IsImportConditions
    {
      get => this.isImportCondtions;
      set => this.isImportCondtions = value;
    }

    public bool SetImportOnly()
    {
      bool enhancedConditions = this.loanDataMgr.LoanData.EnableEnhancedConditions;
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
      bool flag1;
      bool flag2;
      if (enhancedConditions)
      {
        flag1 = folderAccessRights.CanImportAllEnhancedConditions();
        flag2 = folderAccessRights.CanReviewAndImportEnhancedConditions();
      }
      else
      {
        flag1 = folderAccessRights.CanUseDeliveryConditionImportAllCondition;
        flag2 = folderAccessRights.CanUseSellConditionImportCondition;
      }
      if (!enhancedConditions && (!folderAccessRights.CanAddSellConditions || !flag2 && !flag1) || enhancedConditions && !flag2 && !flag1)
        return false;
      this.Text = "Import Delivery Conditions";
      this.rdoNew.Checked = false;
      this.rdoNew.Visible = false;
      if (flag2)
        this.rdoImport.Checked = true;
      else if (flag1)
        this.rdImportAll.Checked = true;
      if (flag1)
      {
        this.rdoImport.Top = this.rdImportAll.Top;
        this.rdImportAll.Top = this.rdoNew.Top;
      }
      else if (flag2)
        this.rdoImport.Top = this.rdoNew.Top;
      return true;
    }

    public AddSellDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
      if (this.loanDataMgr.LoanData.EnableEnhancedConditions)
      {
        if (!folderAccessRights.CanReviewAndImportEnhancedConditions())
          this.rdoImport.Visible = false;
        if (folderAccessRights.CanImportAllEnhancedConditions())
          return;
        this.rdImportAll.Visible = false;
        this.rdoImport.Top = this.rdImportAll.Top;
      }
      else
      {
        if (!folderAccessRights.CanUseSellConditionImportCondition)
          this.rdoImport.Visible = false;
        if (folderAccessRights.CanUseDeliveryConditionImportAllCondition)
          return;
        this.rdImportAll.Visible = false;
        this.rdoImport.Top = this.rdImportAll.Top;
      }
    }

    public ConditionLog[] Conditions => this.condList;

    public bool ShowSellConditions
    {
      get
      {
        if (this.rdoNew.Checked)
          return true;
        return (this.rdoImport.Checked || this.rdImportAll.Checked) && this.condList != null && ((IEnumerable<ConditionLog>) this.condList).Count<ConditionLog>() == 1;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.rdoNew.Checked)
        this.addCondition();
      else if (this.rdoImport.Checked)
      {
        if (this.loanDataMgr.LoanData.EnableEnhancedConditions)
          this.importEnhancedCondition(false);
        else
          this.importCondition(false);
      }
      else
      {
        if (!this.rdImportAll.Checked)
          return;
        if (this.loanDataMgr.LoanData.EnableEnhancedConditions)
          this.importEnhancedCondition(true);
        else
          this.importCondition(true);
      }
    }

    private void addCondition()
    {
      LoanData loanData = this.loanDataMgr.LoanData;
      LogList logList = loanData.GetLogList();
      SellConditionLog rec = new SellConditionLog(Session.UserID, loanData.PairId);
      rec.Title = "Untitled";
      rec.Source = "Manual";
      logList.AddRecord((LogRecordBase) rec);
      this.condList = new ConditionLog[1]
      {
        (ConditionLog) rec
      };
      this.DialogResult = DialogResult.OK;
    }

    private void importCondition(bool importAll)
    {
      if (this.displayingImportCondition)
        return;
      try
      {
        this.displayingImportCondition = true;
        this.isImportCondtions = true;
        List<ConditionLog> conditionLogList1 = new List<ConditionLog>();
        List<ConditionLog> conditionLogList2;
        if (importAll)
        {
          conditionLogList2 = new ImportConditionFactory(this.loanDataMgr.LoanData).GetAllConditionsToImport();
        }
        else
        {
          ImportConditionFactory conditionFactory = new ImportConditionFactory(ConditionType.Sell, this.loanDataMgr.LoanData);
          using (Form importConditionForm = conditionFactory.GetImportConditionForm())
          {
            int num = (int) importConditionForm.ShowDialog((IWin32Window) this);
          }
          conditionLogList2 = conditionFactory.GetResult();
        }
        SellConditionLog[] allConditions = this.loanDataMgr.LoanData.GetLogList().GetAllConditions(ConditionType.Sell) as SellConditionLog[];
        if (conditionLogList2 != null && conditionLogList2.Count > 0)
        {
          this.condList = conditionLogList2.ToArray();
          foreach (StandardConditionLog rec in conditionLogList2)
          {
            Dictionary<string, string> providerUrnDictionany = this.providerUrnToDictionany(rec.ProviderURN);
            StandardConditionLog standardConditionLog = (StandardConditionLog) null;
            if (allConditions != null && !string.IsNullOrEmpty(providerUrnDictionany["partner"]) && !string.IsNullOrEmpty(providerUrnDictionany["condition"]))
              standardConditionLog = (StandardConditionLog) ((IEnumerable<SellConditionLog>) allConditions).FirstOrDefault<SellConditionLog>((Func<SellConditionLog, bool>) (item => this.providerUrnToDictionany(item.ProviderURN)["partner"].Equals(providerUrnDictionany["partner"]) && this.providerUrnToDictionany(item.ProviderURN)["condition"].Equals(providerUrnDictionany["condition"])));
            if (standardConditionLog == null)
            {
              string title = rec.Title;
              string description = rec.Description;
              string category = rec.Category;
              string priorTo = ((SellConditionLog) rec).PriorTo;
              rec.Title = string.Empty;
              rec.Description = string.Empty;
              rec.Category = string.Empty;
              ((SellConditionLog) rec).PriorTo = string.Empty;
              List<CommentEntry> commentEntryList = new List<CommentEntry>();
              foreach (CommentEntry comment in (CollectionBase) rec.Comments)
                commentEntryList.Add(comment);
              rec.Comments.Clear();
              this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) rec);
              rec.Title = title;
              rec.Description = description;
              rec.Category = category;
              ((SellConditionLog) rec).PriorTo = priorTo;
              if (((SellConditionLog) rec).SetStatusFulfilled)
                ((SellConditionLog) rec).MarkAsFulfilled(DateTime.Now, Session.UserID);
              else if (((SellConditionLog) rec).SetStatusRequested)
                rec.MarkAsRequested(DateTime.Now, Session.UserID);
              else if (((SellConditionLog) rec).SetStatusRerequested)
                rec.MarkAsRerequested(DateTime.Now, Session.UserID);
              else if (((SellConditionLog) rec).SetStatusReceived)
                rec.MarkAsReceived(DateTime.Now, Session.UserID);
              else if (((SellConditionLog) rec).SetStatusReviewed)
                ((SellConditionLog) rec).MarkAsReviewed(DateTime.Now, Session.UserID);
              else if (((SellConditionLog) rec).SetStatusRejected)
                ((SellConditionLog) rec).MarkAsRejected(DateTime.Now, Session.UserID);
              else if (((SellConditionLog) rec).SetStatusCleared)
                ((SellConditionLog) rec).MarkAsCleared(DateTime.Now, Session.UserID);
              else if (((SellConditionLog) rec).SetStatusWaived)
                ((SellConditionLog) rec).MarkAsWaived(DateTime.Now, Session.UserID);
              foreach (CommentEntry entry in commentEntryList)
              {
                if (!string.IsNullOrWhiteSpace(entry.Comments))
                  rec.Comments.Add(entry);
              }
              allConditions = this.loanDataMgr.LoanData.GetLogList().GetAllConditions(ConditionType.Sell) as SellConditionLog[];
            }
            else
            {
              standardConditionLog.TrackChangeForCondition("Condition update received from " + rec.Source);
              standardConditionLog.Title = rec.Title;
              standardConditionLog.Description = rec.Description;
              standardConditionLog.Source = rec.Source;
              standardConditionLog.PairId = rec.PairId;
              standardConditionLog.ProviderURN = rec.ProviderURN;
              standardConditionLog.Category = rec.Category;
              if (rec.GetType() == typeof (SellConditionLog))
              {
                if (((SellConditionLog) rec).SetStatusFulfilled)
                  ((SellConditionLog) standardConditionLog).MarkAsFulfilled(DateTime.Now, Session.UserID);
                else if (((SellConditionLog) rec).SetStatusRequested)
                  standardConditionLog.MarkAsRequested(DateTime.Now, Session.UserID);
                else if (((SellConditionLog) rec).SetStatusRerequested)
                  standardConditionLog.MarkAsRerequested(DateTime.Now, Session.UserID);
                else if (((SellConditionLog) rec).SetStatusReceived)
                  standardConditionLog.MarkAsReceived(DateTime.Now, Session.UserID);
                else if (((SellConditionLog) rec).SetStatusReviewed)
                  ((SellConditionLog) standardConditionLog).MarkAsReviewed(DateTime.Now, Session.UserID);
                else if (((SellConditionLog) rec).SetStatusRejected)
                  ((SellConditionLog) standardConditionLog).MarkAsRejected(DateTime.Now, Session.UserID);
                else if (((SellConditionLog) rec).SetStatusCleared)
                  ((SellConditionLog) standardConditionLog).MarkAsCleared(DateTime.Now, Session.UserID);
                else if (((SellConditionLog) rec).SetStatusWaived)
                  ((SellConditionLog) standardConditionLog).MarkAsWaived(DateTime.Now, Session.UserID);
                foreach (CommentEntry comment in (CollectionBase) rec.Comments)
                {
                  if (!string.IsNullOrWhiteSpace(comment.Comments))
                    standardConditionLog.Comments.Add(comment);
                }
                ((SellConditionLog) standardConditionLog).ConditionCode = ((SellConditionLog) rec).ConditionCode;
                ((SellConditionLog) standardConditionLog).PriorTo = ((SellConditionLog) rec).PriorTo;
              }
              if (this.ShowSellConditions && this.Conditions[0].ConditionType.ToString().ToLower() == "sell")
                this.condList[0] = (ConditionLog) standardConditionLog;
            }
          }
          this.DialogResult = DialogResult.OK;
          if (!importAll)
            return;
          int num = (int) MessageBox.Show("Conditions imported successfully", "Import Conditions", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
          this.DialogResult = DialogResult.Cancel;
      }
      catch
      {
        throw;
      }
      finally
      {
        this.displayingImportCondition = false;
      }
    }

    private void importEnhancedCondition(bool importAll)
    {
      if (this.loanDataMgr.Dirty && !Session.Application.GetService<ILoanConsole>().SaveLoan())
      {
        this.DialogResult = DialogResult.Cancel;
      }
      else
      {
        ImportConditionFactory conditionFactory = new ImportConditionFactory(ConditionType.Sell, this.loanDataMgr.LoanData, true, importAll);
        using (Form importConditionForm = conditionFactory.GetImportConditionForm())
        {
          int num = (int) importConditionForm.ShowDialog((IWin32Window) this);
          if (conditionFactory.Success)
          {
            this.loanDataMgr.Refresh(false);
            this.DialogResult = DialogResult.OK;
            return;
          }
        }
        this.DialogResult = DialogResult.Cancel;
      }
    }

    private Dictionary<string, string> providerUrnToDictionany(string providerUrn)
    {
      try
      {
        Dictionary<string, string> dictionary = ((IEnumerable<string>) providerUrn.Replace(":partner:", "|partner:").Replace(":Investor:", "|Investor:").Replace(":transaction:", "|transaction:").Replace(":condition:", "|condition:").Replace(":conditionId:", "|conditionId:").Split('|')).Select<string, string[]>((Func<string, string[]>) (x => x.Split(':'))).ToDictionary<string[], string, string>((Func<string[], string>) (s => s[0]), (Func<string[], string>) (s => s[1]));
        if (!dictionary.ContainsKey("partner"))
          dictionary.Add("partner", string.Empty);
        if (!dictionary.ContainsKey("condition"))
          dictionary.Add("condition", string.Empty);
        return dictionary;
      }
      catch
      {
        return new Dictionary<string, string>()
        {
          {
            "partner",
            string.Empty
          },
          {
            "condition",
            string.Empty
          }
        };
      }
    }

    private void AddSellDialog_VisibleChanged(object sender, EventArgs e)
    {
      if (this.DialogResult != DialogResult.OK || this.Visible)
        return;
      if (this.ShowSellConditions)
      {
        if (!(this.Conditions[0].ConditionType.ToString().ToLower() == "sell"))
          return;
        eFolderDialog.ShowInstance(Session.DefaultInstance);
        eFolderDialog.SelectedTabFromImport(ConditionType.Sell, this.Conditions[0]);
        SellDetailsDialog.ShowInstance(this.loanDataMgr, (SellConditionLog) this.Conditions[0]);
      }
      else
      {
        eFolderDialog.ShowInstance(Session.DefaultInstance);
        eFolderDialog.SelectedTabFromImport(this.loanDataMgr.LoanData.EnableEnhancedConditions ? ConditionType.Enhanced : ConditionType.Sell, (ConditionLog) null);
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
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.rdoImport = new RadioButton();
      this.rdoNew = new RadioButton();
      this.rdImportAll = new RadioButton();
      this.SuspendLayout();
      this.btnOK.Location = new Point(129, 138);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(205, 138);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.rdoImport.AutoSize = true;
      this.rdoImport.Location = new Point(12, 62);
      this.rdoImport.Name = "rdoImport";
      this.rdoImport.Size = new Size(168, 18);
      this.rdoImport.TabIndex = 1;
      this.rdoImport.Text = "Review and Import Conditions";
      this.rdoImport.UseVisualStyleBackColor = true;
      this.rdoNew.AutoSize = true;
      this.rdoNew.Checked = true;
      this.rdoNew.Location = new Point(12, 12);
      this.rdoNew.Name = "rdoNew";
      this.rdoNew.Size = new Size(125, 18);
      this.rdoNew.TabIndex = 0;
      this.rdoNew.TabStop = true;
      this.rdoNew.Text = "Add a new condition";
      this.rdoNew.UseVisualStyleBackColor = true;
      this.rdImportAll.AutoSize = true;
      this.rdImportAll.Location = new Point(12, 38);
      this.rdImportAll.Name = "rdImportAll";
      this.rdImportAll.Size = new Size(120, 18);
      this.rdImportAll.TabIndex = 5;
      this.rdImportAll.Text = "Import all Conditions";
      this.rdImportAll.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(292, 172);
      this.Controls.Add((Control) this.rdImportAll);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.rdoImport);
      this.Controls.Add((Control) this.rdoNew);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddSellDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Condition";
      this.VisibleChanged += new EventHandler(this.AddSellDialog_VisibleChanged);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
