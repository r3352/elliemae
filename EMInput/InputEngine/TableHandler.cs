// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TableHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class TableHandler
  {
    private LoanData loanData;
    private string[] escrowFields;
    private string[] titleFields;
    private string titleType = string.Empty;
    private Sessions.Session session = Session.DefaultInstance;
    private double escrowFee;
    private string escrowTableName = string.Empty;

    internal TableHandler(LoanData loanData, string scrID, Sessions.Session session)
    {
      this.loanData = loanData;
      this.session = session;
      switch (scrID)
      {
        case "SECTION32":
        case "REGZGFE":
          this.escrowFields = new string[2]
          {
            "ESCROW_TABLE",
            "387"
          };
          this.titleFields = new string[2]
          {
            "TITLE_TABLE",
            "385"
          };
          break;
        case "RE88395":
          this.escrowFields = new string[2]
          {
            "ESCROW_TABLE",
            "RE88395.X74"
          };
          this.titleFields = new string[2]
          {
            "TITLE_TABLE",
            "RE88395.X83"
          };
          break;
        case "2010_Escrow":
          this.escrowFields = new string[2]
          {
            "ESCROW_TABLE",
            "NEWHUD.X808"
          };
          break;
        case "2010_LenderTitle":
          this.titleFields = new string[2]
          {
            "TITLE_TABLE",
            "NEWHUD.X639"
          };
          this.titleType = "Lender";
          break;
        case "2010_OwnerTitle":
          this.titleFields = new string[2]
          {
            "2010TITLE_TABLE",
            "NEWHUD.X572"
          };
          this.titleType = "Owner";
          break;
      }
    }

    internal bool LookUpTable(string tableType)
    {
      bool flag = false;
      string simpleField = this.loanData != null ? this.loanData.GetSimpleField("19") : "";
      switch (tableType)
      {
        case "Escrow":
          flag = simpleField.IndexOf("Refinance") <= -1 ? this.lookUpTableFee("Escrow Fee (Purchase)") : this.lookUpTableFee("Escrow Fee (Refinance)");
          break;
        case "Title":
          flag = simpleField.IndexOf("Refinance") <= -1 ? this.lookUpTableFee("Title Fee (Purchase)") : this.lookUpTableFee("Title Fee (Refinance)");
          break;
      }
      return flag;
    }

    private bool lookUpTableFee(string tableType)
    {
      TableFeeListBase tableFeeListBase = (TableFeeListBase) null;
      switch (tableType)
      {
        case "Escrow Fee (Purchase)":
          tableFeeListBase = (TableFeeListBase) this.session.GetSystemSettings(typeof (TblEscrowPurList));
          break;
        case "Escrow Fee (Refinance)":
          tableFeeListBase = (TableFeeListBase) this.session.GetSystemSettings(typeof (TblEscrowRefiList));
          break;
        case "Title Fee (Purchase)":
          tableFeeListBase = (TableFeeListBase) this.session.GetSystemSettings(typeof (TblTitlePurList));
          break;
        case "Title Fee (Refinance)":
          tableFeeListBase = (TableFeeListBase) this.session.GetSystemSettings(typeof (TblTitleRefiList));
          break;
      }
      if (this.loanData == null || this.loanData.IsTemplate)
        return this.selectTableList(tableType);
      string empty = string.Empty;
      for (int i = 0; i < tableFeeListBase.Count; ++i)
      {
        TableFeeListBase.FeeTable tableAt = tableFeeListBase.GetTableAt(i);
        if (tableAt.UseThis && (!tableType.StartsWith("Title Fee") || !(this.titleType != tableAt.FeeType)))
        {
          this.escrowFee = this.loanData.Calculator.CalcTitleEscrowRate(tableAt);
          this.escrowTableName = tableAt.TableName;
          int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The default table '" + this.escrowTableName + "' will be used for calculation.");
          if (this.loanData.Use2010RESPA && tableType.StartsWith("Escrow"))
            return true;
          this.loanData.SetCurrentField(tableType.StartsWith("Escrow") ? this.escrowFields[0] : this.titleFields[0], this.escrowTableName);
          this.loanData.SetCurrentField(tableType.StartsWith("Escrow") ? this.escrowFields[1] : this.titleFields[1], this.escrowFee.ToString("N2"));
          if (this.loanData.Use2015RESPA && this.escrowFields != null && this.escrowFields.Length > 1)
            this.loanData.Calculator.Calculate2015FeeDetails(this.escrowFields[1]);
          return true;
        }
      }
      return this.selectTableList(tableType);
    }

    private bool selectTableList(string tableType)
    {
      using (TableListDialog tableListDialog = new TableListDialog(tableType, this.titleType, this.session))
      {
        if (tableListDialog.ShowDialog((IWin32Window) this.session.MainForm) == DialogResult.OK)
        {
          this.escrowTableName = tableListDialog.SelectedTableName;
          if (this.loanData == null)
          {
            this.escrowFee = 0.0;
            return true;
          }
          this.escrowFee = this.loanData.Calculator.CalcTitleEscrowRate(tableListDialog.SelectedFeeTable);
          if (tableType.IndexOf("Title") > -1)
          {
            this.loanData.SetCurrentField(this.titleFields[0], this.escrowTableName);
            this.loanData.SetCurrentField(this.titleFields[1], this.escrowFee.ToString("N2"));
            if (this.loanData.Use2015RESPA)
              this.loanData.Calculator.Calculate2015FeeDetails(this.titleFields[1]);
          }
          else if (!this.loanData.Use2010RESPA)
          {
            this.loanData.SetCurrentField(this.escrowFields[0], this.escrowTableName);
            this.loanData.SetCurrentField(this.escrowFields[1], this.escrowFee.ToString("N2"));
            if (this.loanData.Use2015RESPA)
              this.loanData.Calculator.Calculate2015FeeDetails(this.escrowFields[1]);
          }
          return true;
        }
      }
      return false;
    }

    internal double EscrowFee => this.escrowFee;

    internal string EscrowTableName => this.escrowTableName;
  }
}
