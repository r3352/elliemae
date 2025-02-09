// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LoanXDBCompareDialog
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class LoanXDBCompareDialog : Form
  {
    private Button btnOK;
    private GVColumn columnHeader2;
    private GVColumn columnHeader3;
    private GVColumn columnHeader4;
    private GVColumn columnHeader1;
    private GridView listViewCompare;
    private GVColumn columnHeader5;
    private System.ComponentModel.Container components;
    private GVColumn columnHeader6;
    private GVColumn columnHeader7;
    private GVColumn columnHeader8;
    private bool useERDB;

    public LoanXDBCompareDialog(bool useERDB, GridView currentView)
    {
      this.useERDB = useERDB;
      this.InitializeComponent();
      this.initCompare(currentView);
      this.listViewCompare.Sort(0, SortOrder.Ascending);
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
      this.listViewCompare = new GridView();
      this.columnHeader1 = new GVColumn();
      this.columnHeader2 = new GVColumn();
      this.columnHeader7 = new GVColumn();
      this.columnHeader3 = new GVColumn();
      this.columnHeader4 = new GVColumn();
      this.columnHeader5 = new GVColumn();
      this.columnHeader6 = new GVColumn();
      this.columnHeader8 = new GVColumn();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(516, 356);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "&Close";
      this.listViewCompare.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listViewCompare.Columns.AddRange(new GVColumn[8]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader7,
        this.columnHeader3,
        this.columnHeader4,
        this.columnHeader5,
        this.columnHeader6,
        this.columnHeader8
      });
      this.listViewCompare.FullRowSelect = true;
      this.listViewCompare.Location = new Point(12, 8);
      this.listViewCompare.Name = "listViewCompare";
      this.listViewCompare.Size = new Size(580, 336);
      this.listViewCompare.TabIndex = 1;
      this.columnHeader1.Text = "Status";
      this.columnHeader1.Width = 70;
      this.columnHeader2.Text = "Field ID";
      this.columnHeader2.Width = 90;
      this.columnHeader7.Text = "Pair";
      this.columnHeader7.Width = 36;
      this.columnHeader3.Text = "Description";
      this.columnHeader3.Width = 139;
      this.columnHeader4.Text = "Field Type";
      this.columnHeader4.Width = 70;
      this.columnHeader5.Text = "Column ID";
      this.columnHeader5.Width = 80;
      this.columnHeader6.Text = "Table Name";
      this.columnHeader6.Width = 100;
      this.columnHeader8.Text = "Use Index";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(604, 389);
      this.Controls.Add((Control) this.listViewCompare);
      this.Controls.Add((Control) this.btnOK);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (LoanXDBCompareDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Preview of Changes";
      this.ResumeLayout(false);
    }

    private void initCompare(GridView currentView)
    {
      this.listViewCompare.Items.Clear();
      Font font = this.listViewCompare.Font;
      LoanXDBTableList loanXdbTableList = Session.LoanManager.GetLoanXDBTableList(this.useERDB);
      Hashtable hashtable = (Hashtable) null;
      if (loanXdbTableList != null)
        hashtable = loanXdbTableList.GetFields();
      for (int nItemIndex = 0; nItemIndex < currentView.Items.Count; ++nItemIndex)
      {
        LoanXDBField tag = (LoanXDBField) currentView.Items[nItemIndex].Tag;
        if (!hashtable.ContainsKey((object) tag.FieldIDWithCoMortgagor))
        {
          this.displayChangedField(tag, LoanXDBField.FieldStatus.New);
        }
        else
        {
          LoanXDBField loanXdbField = (LoanXDBField) hashtable[(object) tag.FieldIDWithCoMortgagor];
          tag.TableName = loanXdbField.TableName;
          if (tag.FieldSizeToString != loanXdbField.FieldSizeToString || tag.UseIndex != loanXdbField.UseIndex || tag.Description != loanXdbField.Description)
          {
            this.displayChangedField(tag, LoanXDBField.FieldStatus.Updated);
            hashtable.Remove((object) tag.FieldIDWithCoMortgagor);
          }
          else
          {
            this.displayUnchangedField(tag);
            hashtable.Remove((object) tag.FieldIDWithCoMortgagor);
          }
        }
      }
      foreach (DictionaryEntry dictionaryEntry in hashtable)
        this.displayChangedField((LoanXDBField) dictionaryEntry.Value, LoanXDBField.FieldStatus.Removed);
    }

    private void displayChangedField(LoanXDBField dbField, LoanXDBField.FieldStatus status)
    {
      GVItem gvItem = new GVItem();
      Color black = Color.Black;
      Color color;
      switch (status)
      {
        case LoanXDBField.FieldStatus.New:
          color = Color.Blue;
          gvItem.Text = "Added";
          break;
        case LoanXDBField.FieldStatus.Removed:
          color = Color.Red;
          gvItem.Text = "Removed";
          break;
        default:
          color = Color.DarkGreen;
          gvItem.Text = "Modified";
          break;
      }
      gvItem.ForeColor = color;
      gvItem.SubItems[1].Text = dbField.FieldID;
      gvItem.SubItems[1].SortValue = (object) this.getFieldIDSortKey(dbField.FieldID);
      FieldDefinition field = EncompassFields.GetField(dbField.FieldID);
      if (field != null && field.Category == FieldCategory.Common || dbField.FieldID.ToLower().StartsWith("cx."))
      {
        gvItem.SubItems[2].Text = "";
      }
      else
      {
        int pair = dbField.ComortgagorPair;
        if (pair < 1)
          pair = 1;
        gvItem.SubItems[2].Text = LoanXDBManager.ToPairOrder(pair);
      }
      gvItem.SubItems[3].Text = dbField.Description;
      gvItem.SubItems[4].Text = dbField.FieldType != LoanXDBTableList.TableTypes.IsDate ? (dbField.FieldType != LoanXDBTableList.TableTypes.IsNumeric ? "String" : "Numeric") : "Date";
      gvItem.SubItems[5].Text = dbField.ColumnName;
      gvItem.SubItems[6].Text = dbField.TableName;
      gvItem.SubItems[7].Text = dbField.UseIndex ? "Y" : "N";
      this.listViewCompare.Items.Add(gvItem);
    }

    private string getFieldIDSortKey(string fieldId)
    {
      return Utils.IsInt((object) fieldId) ? "A" + (object) Utils.ParseInt((object) fieldId) : "B" + fieldId;
    }

    private void displayUnchangedField(LoanXDBField dbField)
    {
      GVItem gvItem = new GVItem("");
      gvItem.SubItems[0].SortValue = (object) "ZZZZ";
      gvItem.SubItems[1].Text = dbField.FieldID;
      gvItem.SubItems[1].SortValue = (object) this.getFieldIDSortKey(dbField.FieldID);
      FieldDefinition field = EncompassFields.GetField(dbField.FieldID);
      if (field != null && field.Category == FieldCategory.Common || dbField.FieldID.ToLower().StartsWith("cx."))
      {
        gvItem.SubItems[2].Text = "";
      }
      else
      {
        int pair = dbField.ComortgagorPair;
        if (pair < 1)
          pair = 1;
        gvItem.SubItems[2].Text = LoanXDBManager.ToPairOrder(pair);
      }
      gvItem.SubItems[3].Text = dbField.Description;
      gvItem.SubItems[4].Text = dbField.FieldType != LoanXDBTableList.TableTypes.IsDate ? (dbField.FieldType != LoanXDBTableList.TableTypes.IsNumeric ? "String" : "Numeric") : "Date";
      gvItem.SubItems[5].Text = dbField.ColumnName;
      gvItem.SubItems[6].Text = dbField.TableName;
      gvItem.SubItems[7].Text = dbField.UseIndex ? "Y" : "N";
      this.listViewCompare.Items.Add(gvItem);
    }
  }
}
