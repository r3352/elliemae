// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MCAWREFIInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class MCAWREFIInputHandler : InputHandlerBase
  {
    public MCAWREFIInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public MCAWREFIInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public MCAWREFIInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public MCAWREFIInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "enerfyimprove":
        case "mcawtotalcc":
          base.ExecAction(action);
          this.SetFieldFocus("l_26");
          break;
        case "mipff":
          base.ExecAction(action);
          this.SetFieldFocus("l_356");
          break;
        case "baseincome":
          BorIncomeDialog borIncomeDialog = new BorIncomeDialog(this.loan);
          if (borIncomeDialog.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            if (borIncomeDialog.CopyVOE)
            {
              double num;
              string val1;
              if (borIncomeDialog.BorBaseIncome == 0.0)
              {
                val1 = "";
              }
              else
              {
                num = borIncomeDialog.BorBaseIncome;
                val1 = num.ToString();
              }
              this.UpdateFieldValue("101", val1);
              string val2;
              if (borIncomeDialog.BorOverTime == 0.0)
              {
                val2 = "";
              }
              else
              {
                num = borIncomeDialog.BorOverTime;
                val2 = num.ToString();
              }
              this.UpdateFieldValue("102", val2);
              string val3;
              if (borIncomeDialog.BorBonus == 0.0)
              {
                val3 = "";
              }
              else
              {
                num = borIncomeDialog.BorBonus;
                val3 = num.ToString();
              }
              this.UpdateFieldValue("103", val3);
              string val4;
              if (borIncomeDialog.BorCommissions == 0.0)
              {
                val4 = "";
              }
              else
              {
                num = borIncomeDialog.BorCommissions;
                val4 = num.ToString();
              }
              this.UpdateFieldValue("104", val4);
              string val5;
              if (borIncomeDialog.BorOthers == 0.0)
              {
                val5 = "";
              }
              else
              {
                num = borIncomeDialog.BorOthers;
                val5 = num.ToString();
              }
              this.UpdateFieldValue("107", val5);
              string val6;
              if (borIncomeDialog.CobBaseIncome == 0.0)
              {
                val6 = "";
              }
              else
              {
                num = borIncomeDialog.CobBaseIncome;
                val6 = num.ToString();
              }
              this.UpdateFieldValue("110", val6);
              string val7;
              if (borIncomeDialog.CobOverTime == 0.0)
              {
                val7 = "";
              }
              else
              {
                num = borIncomeDialog.CobOverTime;
                val7 = num.ToString();
              }
              this.UpdateFieldValue("111", val7);
              string val8;
              if (borIncomeDialog.CobBonus == 0.0)
              {
                val8 = "";
              }
              else
              {
                num = borIncomeDialog.CobBonus;
                val8 = num.ToString();
              }
              this.UpdateFieldValue("112", val8);
              string val9;
              if (borIncomeDialog.CobCommissions == 0.0)
              {
                val9 = "";
              }
              else
              {
                num = borIncomeDialog.CobCommissions;
                val9 = num.ToString();
              }
              this.UpdateFieldValue("113", val9);
              string val10;
              if (borIncomeDialog.CobOthers == 0.0)
              {
                val10 = "";
              }
              else
              {
                num = borIncomeDialog.CobOthers;
                val10 = num.ToString();
              }
              this.UpdateFieldValue("116", val10);
            }
            else
            {
              double num = borIncomeDialog.BorBaseIncome;
              if (num > 0.0)
                this.UpdateFieldValue("101", num != 0.0 ? num.ToString() : "");
              num = borIncomeDialog.CobBaseIncome;
              if (num > 0.0)
                this.UpdateFieldValue("110", num != 0.0 ? num.ToString() : "");
            }
            this.UpdateContents();
          }
          this.SetFieldFocus("l_101");
          break;
        case "eem":
          try
          {
            this.loan.Calculator.FormCalculation("MCAWREFI", "1228", "");
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          this.UpdateContents();
          break;
      }
    }
  }
}
