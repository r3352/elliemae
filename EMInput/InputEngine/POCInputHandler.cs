// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.POCInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class POCInputHandler
  {
    private LoanData loan;
    private EllieMae.Encompass.Forms.Form currentForm;
    private InputHandlerBase currentHandler;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private EllieMae.Encompass.Forms.Panel panel1400;
    private EllieMae.Encompass.Forms.Panel panelForm;
    private EllieMae.Encompass.Forms.Image imageSellerPaidWarning;
    private EllieMae.Encompass.Forms.Image imageSellerPaidWarning1000;
    private EllieMae.Encompass.Forms.Image imageForPOC;
    private EllieMae.Encompass.Forms.Image imageForPOC4Affiliate;
    private EllieMae.Encompass.Forms.Panel panelPOC;
    private EllieMae.Encompass.Forms.Label lblPOCAmt;
    private EllieMae.Encompass.Forms.Label lblPaidBy;
    private EllieMae.Encompass.Forms.Label lblPTCAmt;
    private EllieMae.Encompass.Forms.Label lblPTCPaidBy;
    private EllieMae.Encompass.Forms.Label lblBorPay;
    private EllieMae.Encompass.Forms.Label lblFinanced;
    private EllieMae.Encompass.Forms.Label lblAffiliate;
    private EllieMae.Encompass.Forms.Label lblAffiliateAmt;
    private FieldLock fieldLock819;
    private bool isEntryShowed;
    private IHtmlInput inputData;
    private Sessions.Session session = Session.DefaultInstance;
    private bool for2015;

    public POCInputHandler(IHtmlInput inputData, EllieMae.Encompass.Forms.Form currentForm, bool for2015)
    {
      this.inputData = inputData;
      if (this.inputData is LoanData)
        this.loan = (LoanData) this.inputData;
      this.currentForm = currentForm;
      this.for2015 = for2015;
      this.fieldLock819 = (FieldLock) this.currentForm.FindControl("FieldLock819");
    }

    public POCInputHandler(
      IHtmlInput inputData,
      EllieMae.Encompass.Forms.Form currentForm,
      InputHandlerBase currentHandler,
      Sessions.Session session)
    {
      this.inputData = inputData;
      this.session = session;
      if (this.inputData is LoanData)
        this.loan = (LoanData) this.inputData;
      this.currentForm = currentForm;
      this.currentHandler = currentHandler;
      this.panelForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      if (this.loan == null && this.currentHandler is REGZGFE_2010InputHandler)
      {
        if (this.panel1400 == null)
        {
          this.panel1400 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("Panel1400");
          if (this.panel1400 != null)
            this.panel1400.Visible = false;
        }
        if (this.panel1400 != null)
          this.panelForm.Size = new Size(this.panelForm.Size.Width, this.panelForm.Size.Height - this.panel1400.Size.Height);
        if (this.labelFormName == null && this.panel1400 != null)
        {
          this.labelFormName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelFormName");
          this.labelFormName.Top -= this.panel1400.Size.Height;
        }
      }
      this.imageSellerPaidWarning = (EllieMae.Encompass.Forms.Image) this.currentForm.FindControl(nameof (imageSellerPaidWarning));
      this.imageSellerPaidWarning1000 = (EllieMae.Encompass.Forms.Image) this.currentForm.FindControl(nameof (imageSellerPaidWarning1000));
      this.imageForPOC = (EllieMae.Encompass.Forms.Image) this.currentForm.FindControl("imgPOC");
      this.imageForPOC4Affiliate = (EllieMae.Encompass.Forms.Image) this.currentForm.FindControl("imgPOC4Affiliate");
      this.panelPOC = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl(nameof (panelPOC));
      this.lblPOCAmt = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl(nameof (lblPOCAmt));
      this.lblPaidBy = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl(nameof (lblPaidBy));
      this.lblPTCAmt = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl(nameof (lblPTCAmt));
      this.lblPTCPaidBy = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl(nameof (lblPTCPaidBy));
      this.lblBorPay = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl(nameof (lblBorPay));
      this.lblFinanced = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl(nameof (lblFinanced));
      this.lblAffiliate = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelAffiliate");
      this.lblAffiliateAmt = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl(nameof (lblAffiliateAmt));
      this.fieldLock819 = (FieldLock) this.currentForm.FindControl("FieldLock819");
    }

    public void TurnOnSellerPaidWarning(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.for2015)
        return;
      string attribute = (string) pEvtObj.srcElement.getAttribute("emid");
      if (!HUDGFE2010Fields.ALLSELLERFIELDS.Contains(attribute))
        return;
      EllieMae.Encompass.Forms.Image image = attribute == "596" || attribute == "563" || attribute == "595" || attribute == "L270" || attribute == "597" || attribute == "1632" || attribute == "598" || attribute == "599" || attribute == "NEWHUD.X1714" ? this.imageSellerPaidWarning1000 : this.imageSellerPaidWarning;
      if ((this.imageSellerPaidWarning == null || image != this.imageSellerPaidWarning) && this.imageSellerPaidWarning != null && this.imageSellerPaidWarning.Visible)
        this.imageSellerPaidWarning.Visible = false;
      if ((this.imageSellerPaidWarning1000 == null || image != this.imageSellerPaidWarning1000) && this.imageSellerPaidWarning1000 != null && this.imageSellerPaidWarning1000.Visible)
        this.imageSellerPaidWarning1000.Visible = false;
      if (image == null)
        return;
      RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
      if (!image.Visible)
        image.Visible = true;
      image.Left = controlForElement.AbsolutePosition.X - image.Size.Width + 3;
      image.Top = controlForElement.AbsolutePosition.Y - image.Size.Height + 3;
    }

    public void TurnOffSellerPaidWarning()
    {
      if (this.for2015)
        return;
      if (this.imageSellerPaidWarning != null && this.imageSellerPaidWarning.Visible)
        this.imageSellerPaidWarning.Visible = false;
      if (this.imageSellerPaidWarning1000 == null || !this.imageSellerPaidWarning1000.Visible)
        return;
      this.imageSellerPaidWarning1000.Visible = false;
    }

    public void TurnOnPOCToolTip(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.for2015)
        return;
      if (this.isEntryShowed)
      {
        this.isEntryShowed = false;
      }
      else
      {
        if (this.imageForPOC == null)
          return;
        string attribute;
        try
        {
          attribute = (string) pEvtObj.srcElement.getAttribute("emid");
          if (!attribute.StartsWith("POPT.X"))
            return;
        }
        catch (Exception ex)
        {
          return;
        }
        RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
        bool flag = false;
        if ((attribute == "POPT.X54" || attribute == "POPT.X55") && this.currentHandler.GetField(attribute == "POPT.X54" ? "NEWHUD.X804" : "NEWHUD.X805") == "Affiliate")
          flag = true;
        if (flag)
        {
          if (!this.imageForPOC4Affiliate.Visible)
          {
            this.imageForPOC4Affiliate.Visible = true;
            this.panelPOC.Visible = true;
            EllieMae.Encompass.Forms.Image forPoC4Affiliate1 = this.imageForPOC4Affiliate;
            Point absolutePosition = controlForElement.AbsolutePosition;
            int num1 = absolutePosition.X - this.imageForPOC4Affiliate.Size.Width + 3;
            forPoC4Affiliate1.Left = num1;
            EllieMae.Encompass.Forms.Image forPoC4Affiliate2 = this.imageForPOC4Affiliate;
            absolutePosition = controlForElement.AbsolutePosition;
            int num2 = absolutePosition.Y - this.imageForPOC4Affiliate.Size.Height + 4;
            forPoC4Affiliate2.Top = num2;
            this.panelPOC.Left = this.imageForPOC4Affiliate.Left + 1;
            this.panelPOC.Top = this.imageForPOC4Affiliate.Top + 3;
            this.panelPOC.Size = new Size(this.panelPOC.Size.Width, 185);
          }
        }
        else if (!this.imageForPOC.Visible)
        {
          this.imageForPOC.Visible = true;
          this.panelPOC.Visible = true;
          EllieMae.Encompass.Forms.Image imageForPoc1 = this.imageForPOC;
          Point absolutePosition = controlForElement.AbsolutePosition;
          int num3 = absolutePosition.X - this.imageForPOC.Size.Width + 3;
          imageForPoc1.Left = num3;
          EllieMae.Encompass.Forms.Image imageForPoc2 = this.imageForPOC;
          absolutePosition = controlForElement.AbsolutePosition;
          int num4 = absolutePosition.Y - this.imageForPOC.Size.Height + 4;
          imageForPoc2.Top = num4;
          this.panelPOC.Left = this.imageForPOC.Left + 1;
          this.panelPOC.Top = this.imageForPOC.Top + 3;
          this.panelPOC.Size = new Size(this.panelPOC.Size.Width, 165);
        }
        string[] strArray = (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) attribute];
        double num5 = strArray[0] != string.Empty ? Utils.ParseDouble((object) this.currentHandler.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT])) : 0.0;
        this.lblPOCAmt.Text = num5 != 0.0 ? "$ " + num5.ToString("N2") : "";
        this.lblPaidBy.Text = strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY] != string.Empty ? this.currentHandler.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]) : "";
        if (this.lblPaidBy.Text == string.Empty && num5 != 0.0)
          this.lblPaidBy.Text = "Borrower";
        if (Utils.ParseDouble((object) this.currentHandler.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT])) != 0.0)
        {
          this.lblPTCAmt.Text = "$ " + this.currentHandler.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]);
          this.lblPTCPaidBy.Text = this.currentHandler.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]);
          if (this.lblPTCPaidBy.Text == string.Empty)
            this.lblPTCPaidBy.Text = "Borrower";
        }
        else
        {
          this.lblPTCAmt.Text = "";
          this.lblPTCPaidBy.Text = "";
        }
        double num6 = Utils.ParseDouble((object) this.currentHandler.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY]));
        this.lblBorPay.Text = "$ " + num6.ToString("N2");
        this.lblFinanced.Text = this.currentHandler.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_FINANCED]) == "Y" ? "Yes" : "No";
        if (this.lblAffiliateAmt == null)
          return;
        if (flag)
        {
          this.lblAffiliate.Visible = true;
          this.lblAffiliateAmt.Visible = true;
          num6 = Utils.ParseDouble((object) this.currentHandler.GetField(attribute == "POPT.X54" ? "NEWHUD.X1724" : "NEWHUD.X1725"));
          if (num6 != 0.0)
            this.lblAffiliateAmt.Text = "$ " + num6.ToString("N2");
          else
            this.lblAffiliateAmt.Text = "";
        }
        else
        {
          this.lblAffiliate.Visible = false;
          this.lblAffiliateAmt.Visible = false;
          this.lblAffiliateAmt.Text = "";
        }
      }
    }

    public void TurnOffPOCToolTip()
    {
      if (this.for2015)
        return;
      if (this.imageForPOC != null && this.imageForPOC.Visible)
      {
        this.imageForPOC.Visible = false;
        this.panelPOC.Visible = false;
      }
      if (this.imageForPOC4Affiliate == null || !this.imageForPOC4Affiliate.Visible)
        return;
      this.imageForPOC4Affiliate.Visible = false;
      this.panelPOC.Visible = false;
    }

    public bool IsPOCFieldClicked(string currentFieldID)
    {
      if (this.for2015)
        return false;
      return currentFieldID == "NEWHUD.X804" || currentFieldID == "NEWHUD.X805" || HUDGFE2010Fields.POCPTCFIELDS.ContainsKey((object) currentFieldID);
    }

    public void TurnOnPOCEntryBox(string currentFieldID, mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.for2015 || !currentFieldID.StartsWith("POPT.X") && currentFieldID != "NEWHUD.X804" && currentFieldID != "NEWHUD.X805")
        return;
      if (currentFieldID == "NEWHUD.X804" || currentFieldID == "NEWHUD.X805")
      {
        if (string.Compare((this.currentForm.FindControlForElement(pEvtObj.srcElement) as DropdownBox).Value, "Affiliate", true) != 0)
          return;
        this.setPOCEntry(currentFieldID == "NEWHUD.X804" ? "POPT.X54" : "POPT.X55", pEvtObj.screenX, pEvtObj.screenY - 80);
      }
      else
      {
        EllieMae.Encompass.Forms.CheckBox controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as EllieMae.Encompass.Forms.CheckBox;
        controlForElement.Checked = this.inputData.GetField(currentFieldID) == "Y";
        controlForElement.Blur();
        this.setPOCEntry(currentFieldID, pEvtObj.screenX, pEvtObj.screenY);
      }
    }

    private void setPOCEntry(string currentFieldID, int entryPosX, int entryPosY)
    {
      if (this.for2015)
        return;
      string id = ((string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) currentFieldID])[HUDGFE2010Fields.PTCPOCINDEX_BORPAID];
      if (id == "NEWHUD.X15" && this.inputData.GetField("NEWHUD.X1139") != "Y" && this.inputData.GetField("1663") != string.Empty)
        id = "1663";
      if (Utils.ParseDouble((object) this.inputData.GetField(id)) == 0.0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The borrower paid fee cannot be blank if POC/PTC function is used.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        using (POCDialog pocDialog = new POCDialog(currentFieldID, this.inputData))
        {
          pocDialog.Location = new Point(entryPosX - pocDialog.Width, entryPosY - pocDialog.Height);
          if (pocDialog.ShowDialog((IWin32Window) this.session.MainForm) == DialogResult.OK)
          {
            if (this.loan != null && !this.loan.IsTemplate && this.session.LoanDataMgr.SystemConfiguration != null && this.session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting != null)
              LOCompensationInputHandler.CheckLOCompRuleConfliction(this.session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting, (IHtmlInput) this.loan, (string) null, (string) null, (string) null, true);
            if (this.inputData is LoanData)
              this.loan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
            else if (pocDialog.DirtyFields != null && pocDialog.DirtyFields.Count > 0)
              ((REGZGFE_2010InputHandler) this.currentHandler).UpdateClosingCostLoan(pocDialog.DirtyFields);
            this.currentHandler.RefreshContents();
          }
        }
        this.isEntryShowed = true;
      }
    }

    public void SetLine819Status()
    {
      this.SetFieldLock819Status();
      if (this.loan == null || this.fieldLock819.Visible)
        return;
      string field = this.loan.GetField("NEWHUD.X1301");
      this.loan.RemoveLock("NEWHUD.X1301");
      this.loan.SetField("NEWHUD.X1301", field);
    }

    public void SetFieldLock819Status()
    {
      if (this.fieldLock819 == null)
        return;
      try
      {
        this.fieldLock819.Visible = this.loan != null && this.loan.GetField("1172") == "FarmersHomeAdministration";
      }
      catch (Exception ex)
      {
        return;
      }
      if (this.loan != null && this.loan.IsLocked("NEWHUD.X1301"))
        this.fieldLock819.DisplayImage(true);
      else
        this.fieldLock819.DisplayImage(false);
    }

    public ControlState GetLine819Status(string id)
    {
      if (this.loan != null && this.loan.GetField("1172") == "FarmersHomeAdministration")
      {
        if (this.fieldLock819 != null)
          this.fieldLock819.Enabled = true;
        switch (id)
        {
          case "NEWHUD.X1301":
            return ControlState.Default;
          case "NEWHUD.X1299":
            return this.loan.IsLocked("NEWHUD.X1301") ? ControlState.Enabled : ControlState.Disabled;
        }
      }
      return ControlState.Enabled;
    }
  }
}
