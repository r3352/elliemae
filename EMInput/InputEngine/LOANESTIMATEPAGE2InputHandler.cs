// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LOANESTIMATEPAGE2InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LOANESTIMATEPAGE2InputHandler : InputHandlerBase
  {
    private AIRInputHandler airInputHandler;
    private APInputHandler apInputHandler;
    private EllieMae.Encompass.Forms.Panel pnlCalculatingCashToCloseSV;
    private EllieMae.Encompass.Forms.Panel pnlCalculatingCashToCloseAlt;
    private EllieMae.Encompass.Forms.Panel pnlSubCalculatingCashToCloseSV;
    private EllieMae.Encompass.Forms.CheckBox chkIncludePayoffs;
    private EllieMae.Encompass.Forms.Button btnPayoffs;
    private EllieMae.Encompass.Forms.CheckBox chkItemizeServices;
    private List<EllieMae.Encompass.Forms.Label> monthlyBiweeklyLabels;
    private string itemizationXML = string.Empty;
    private FieldReformatOnUIHandler fieldReformatOnUIHandler;

    public LOANESTIMATEPAGE2InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGE2InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGE2InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGE2InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGE2InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmlDoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmlDoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      try
      {
        this.fieldReformatOnUIHandler = new FieldReformatOnUIHandler(this.inputData);
        this.btnPayoffs = (EllieMae.Encompass.Forms.Button) this.currentForm.FindControl("btnPayoffs");
        this.pnlCalculatingCashToCloseSV = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlCalculatingCashToCloseSV");
        this.pnlCalculatingCashToCloseAlt = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlCalculatingCashToCloseAlt");
        this.pnlSubCalculatingCashToCloseSV = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlSubCalculatingCashToCloseSV");
        this.chkItemizeServices = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chkItemizeServices");
        this.chkIncludePayoffs = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chkIncludePayoffs");
        this.airInputHandler = new AIRInputHandler(this.currentForm, this.inputData);
        this.airInputHandler.SetSectionStatus();
        this.apInputHandler = new APInputHandler(this.currentForm, this.inputData);
        this.apInputHandler.SetSectionStatus();
        this.monthlyBiweeklyLabels = new List<EllieMae.Encompass.Forms.Label>();
        for (int index = 1; index <= 16; ++index)
          this.monthlyBiweeklyLabels.Add((EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelG" + (object) index));
      }
      catch (Exception ex)
      {
      }
      if (this.inputData is DisclosedLEHandler || this.loan != null && this.loan.IsTemplate)
        this.btnPayoffs.Enabled = false;
      if (this.loan != null)
      {
        try
        {
          if (this.inputData is LoanData)
          {
            if (this.loan.Calculator != null)
              this.itemizationXML = this.loan.Calculator.GetUCD(true, true);
          }
        }
        catch (Exception ex)
        {
        }
      }
      if (this.loan == null || this.loan.Calculator == null)
        return;
      this.loan.Calculator.FormCalculation("LOANESTIMATEPAGE2", "", "");
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      if (this.inputData is LoanData)
      {
        XmlDocument doc = new XmlDocument();
        if (this.itemizationXML != "")
          doc.LoadXml(this.itemizationXML);
        dictionary = new UCDXmlParser(doc).ParseXml(true);
      }
      foreach (EllieMae.Encompass.Forms.Control allControl in this.currentForm.GetAllControls())
      {
        switch (allControl)
        {
          case FieldControl _:
            FieldControl fieldControl = (FieldControl) allControl;
            if (fieldControl.Field.FieldID == "")
            {
              if (this.inputData is LoanData)
              {
                if (dictionary.ContainsKey(allControl.ControlID))
                {
                  string str = dictionary[allControl.ControlID];
                  fieldControl.BindTo(str);
                }
              }
              else
              {
                string field = this.inputData.GetField(allControl.ControlID);
                fieldControl.BindTo(field);
              }
            }
            else
            {
              string str = this.RoundorTruncateFieldValue(fieldControl.Field.FieldID, FieldSource.CurrentLoan);
              fieldControl.BindTo(str);
            }
            if (this.inputData is DisclosedLEHandler)
            {
              fieldControl.Enabled = false;
              this.SetStatusCashToCloseSection(this.inputData.GetField("LE2.X28"));
            }
            if (allControl is EllieMae.Encompass.Forms.TextBox && fieldControl.Field.FieldID == "")
            {
              fieldControl.HoverText = allControl.ControlID;
              break;
            }
            break;
          case FieldLock _:
            if (this.inputData is DisclosedLEHandler)
            {
              (allControl as RuntimeControl).Enabled = false;
              break;
            }
            break;
        }
      }
      if (!(this.inputData is LoanData))
        return;
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.chkItemizeServices == null)
        return;
      if (dictionary != null && dictionary.ContainsKey("LEServicesBorrowerDidShopFor14A1") && dictionary["LEServicesBorrowerDidShopFor14A1"].ToString() != "" && (dictionary["LEServicesBorrowerDidShopFor14A1"].ToString() == "See attached page for additional items you can shop for" || dictionary["LEServicesBorrowerDidShopFor14A1"].ToString() == "Additional Charges"))
        this.chkItemizeServices.Enabled = true;
      else
        this.chkItemizeServices.Enabled = false;
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.setMonthlyBiweeklyLabel(this.inputData.GetField("423") == "Biweekly");
    }

    private void setMonthlyBiweeklyLabel(bool isBiweekly)
    {
      for (int index = 0; index <= 7; ++index)
        this.monthlyBiweeklyLabels[index].Text = isBiweekly ? "bwks" : "mths";
      for (int index = 8; index <= 15; ++index)
        this.monthlyBiweeklyLabels[index].Text = isBiweekly ? "per bwk for" : "per mth for";
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (this.inputData is DisclosedLEHandler)
        return ControlState.Disabled;
      this.getControlState(ctrl, id, ControlState.Enabled);
      return !(id == "CD4.X31") ? ControlState.Default : (!(this.inputData.GetField("608") != "AdjustableRate") ? ControlState.Enabled : ControlState.Disabled);
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      if (id == "LE2.X28")
        this.SetStatusCashToCloseSection(this.inputData.GetField(id));
      if (id == "CD4.X25" || id == "CD4.X27" || id == "NEWHUD.X6")
        this.apInputHandler.SetSectionStatus();
      string val = this.airInputHandler.GetFieldValue(id, base.GetFieldValue(id, fieldSource));
      if ((id == "LE2.X1" || id == "LE2.X2" || id == "L128" || id == "LE2.X3" || id == "NEWHUD2.X23" || id == "LE2.X4" || id == "LE2.X25" || id == "2" || id == "LE2.X26") && base.GetFieldValue(id, fieldSource) != "")
        val = Utils.RemoveEndingZeros(Decimal.Parse(Utils.RemoveEndingZeros(base.GetFieldValue(id, fieldSource), true)).ToString("N"));
      return this.fieldReformatOnUIHandler.GetFieldValue(id, val);
    }

    internal string RoundorTruncateFieldValue(string id, FieldSource fieldSource)
    {
      if ((id == "LE2.X1" || id == "LE2.X2" || id == "L128" || id == "LE2.X3" || id == "NEWHUD2.X23" || id == "LE2.X4" || id == "LE2.X25" || id == "2" || id == "LE2.X26") && base.GetFieldValue(id, fieldSource) != "")
        return Utils.RemoveEndingZeros(Decimal.Parse(Utils.RemoveEndingZeros(base.GetFieldValue(id, fieldSource), true)).ToString("N"));
      string fieldValue = this.airInputHandler.GetFieldValue(id, base.GetFieldValue(id, fieldSource));
      return this.fieldReformatOnUIHandler.GetFieldValue(id, fieldValue);
    }

    public void SetStatusCashToCloseSection(string useAlternate)
    {
      if (useAlternate == "Y")
      {
        this.pnlCalculatingCashToCloseAlt.Visible = true;
        this.pnlCalculatingCashToCloseSV.Visible = false;
        this.pnlCalculatingCashToCloseAlt.Position = new Point(0, 0);
        this.pnlCalculatingCashToCloseSV.Position = new Point(400, 0);
      }
      else
      {
        this.pnlCalculatingCashToCloseAlt.Visible = false;
        this.pnlCalculatingCashToCloseSV.Visible = true;
        this.pnlCalculatingCashToCloseSV.Position = new Point(0, 0);
        this.pnlCalculatingCashToCloseAlt.Position = new Point(400, 0);
        if (this.inputData.GetField("19") == "Purchase")
        {
          this.chkIncludePayoffs.Visible = true;
          this.pnlSubCalculatingCashToCloseSV.Position = new Point(5, 82);
        }
        else
        {
          this.chkIncludePayoffs.Visible = false;
          this.pnlSubCalculatingCashToCloseSV.Position = new Point(5, 47);
        }
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      switch (id)
      {
        case "LE2.X28":
          this.SetControlState("l_LE2X1", this.inputData.IsLocked(id));
          this.SetControlState("I_AltLE2X1", this.inputData.IsLocked(id));
          this.SetStatusCashToCloseSection(val);
          break;
        case "LE2.X32":
          try
          {
            if (!(this.inputData is LoanData) || this.loan.Calculator == null)
              break;
            this.itemizationXML = this.loan.Calculator.GetUCD(true, true);
            break;
          }
          catch (Exception ex)
          {
            break;
          }
      }
    }

    public override void ExecAction(string action) => base.ExecAction(action);

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      string fieldId = controlForElement.Field.FieldID;
      if (!(fieldId == "LE2.X1") && !(fieldId == "LE2.X2") && !(fieldId == "L128") && !(fieldId == "LE2.X3") && !(fieldId == "NEWHUD2.X23") && !(fieldId == "LE2.X4") && !(fieldId == "LE2.X25") && !(fieldId == "2") && !(fieldId == "LE2.X26"))
        return;
      bool needsUpdate = false;
      string str = Utils.FormatInput(controlForElement.Value, FieldFormat.INTEGER, ref needsUpdate);
      if (!needsUpdate)
        return;
      controlForElement.BindTo(str);
    }
  }
}
