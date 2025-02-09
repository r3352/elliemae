// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LOCompensationInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LOCompensationInputHandler
  {
    private EllieMae.Encompass.Forms.Form currentForm;
    private IHtmlInput dataObject;
    private InputHandlerBase currentHandler;
    private LOCompensationSetting loCompensationSetting;
    private Image[] image801s;
    private Image imageOver;
    private string imgFile = string.Empty;
    private string imgOverFile = string.Empty;
    public static bool RefreshScreenRequired;

    public LOCompensationInputHandler(
      LOCompensationSetting loCompensationSetting,
      IHtmlInput dataObject,
      EllieMae.Encompass.Forms.Form currentForm,
      InputHandlerBase currentHandler)
    {
      this.dataObject = dataObject;
      this.currentForm = currentForm;
      this.currentHandler = currentHandler;
      this.loCompensationSetting = loCompensationSetting;
      this.image801s = new Image[19];
      for (int index = 0; index < 19; ++index)
        this.image801s[index] = (Image) this.currentForm.FindControl("Image801" + ((char) (97 + index)).ToString());
      this.imageOver = (Image) this.currentForm.FindControl("Image801Over");
      if (this.imageOver != null)
      {
        this.imgOverFile = this.imageOver.Source;
        this.imgFile = this.image801s[0].Source;
      }
      this.RefreshContents();
    }

    public void RefreshContents()
    {
      if (this.loCompensationSetting.LOAction == LOCompensationSetting.LOActions.NoAction || !this.loCompensationSetting.EnableLOCompensationRule(this.dataObject))
        return;
      if (this.image801s != null)
      {
        try
        {
          for (int index = 0; index < 19; ++index)
          {
            if (this.image801s[index] != null)
            {
              if (this.loCompensationSetting.IsLineItemEnabled(((char) (97 + index)).ToString() ?? "", this.dataObject))
              {
                this.image801s[index].Visible = true;
                switch (index)
                {
                  case 4:
                    this.image801s[index].HTMLElement.title = "Broker Fees is an LO Compensation field, paid by Borrower and paid to Broker. You can only enter Broker Fees or Broker Compensation.";
                    continue;
                  case 5:
                    this.image801s[index].HTMLElement.title = "Broker Compensation is an LO Compensation field, paid by Lender and paid to Broker. You can only enter Broker Fees or Broker Compensation.";
                    continue;
                  default:
                    this.image801s[index].HTMLElement.title = "This is an LO compensation field. It should comply with LO Compensation Rule.";
                    continue;
                }
              }
              else
                this.image801s[index].Visible = false;
            }
          }
        }
        catch (Exception ex)
        {
          return;
        }
      }
      this.imageOver = (Image) this.currentForm.FindControl("Image801Over");
      if (this.imageOver == null)
        return;
      this.imgOverFile = this.imageOver.Source;
      this.imgFile = this.image801s[0].Source;
    }

    public void ShowToolTips(mshtml.IHTMLEventObj pEvtObj)
    {
      RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
      if (!controlForElement.ControlID.StartsWith("Image801"))
        return;
      ((Image) controlForElement).Source = this.imgOverFile;
    }

    public void HideToolTips(mshtml.IHTMLEventObj pEvtObj)
    {
      RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
      if (!controlForElement.ControlID.StartsWith("Image801"))
        return;
      ((Image) controlForElement).Source = this.imgFile;
    }

    public static bool CheckLOCompRuleConfliction(
      LOCompensationSetting loCompensationSetting,
      IHtmlInput inputDate,
      string id,
      string newVal,
      string origValue,
      bool updateGFE)
    {
      LOCompensationInputHandler.RefreshScreenRequired = false;
      if (loCompensationSetting == null || loCompensationSetting.LOAction == LOCompensationSetting.LOActions.NoAction || !loCompensationSetting.HasViolation(inputDate, id, newVal))
        return true;
      LOCompensationInputHandler.RefreshScreenRequired = true;
      using (LOCompensationViolationDialog compensationViolationDialog = new LOCompensationViolationDialog(inputDate, id, newVal, loCompensationSetting, updateGFE))
      {
        if (compensationViolationDialog.ShowDialog((IWin32Window) Session.MainForm) == DialogResult.OK)
          return true;
        if (id != null)
        {
          inputDate.SetField(id, origValue);
          if (inputDate is LoanData)
          {
            LoanData loanData = (LoanData) inputDate;
            if (id == "NEWHUD.X1718" && origValue != "Y")
              loanData.Calculator.FormCalculation("CLEAR801LOCOMP", id, origValue);
            loanData.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
          }
        }
        return false;
      }
    }
  }
}
