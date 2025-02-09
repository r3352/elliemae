// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.D1003_2020P3InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class D1003_2020P3InputHandler : InputHandlerBase
  {
    protected string header;
    protected string ind = "00";
    private EllieMae.Encompass.Forms.CheckBox chkVOLLinked1;
    private EllieMae.Encompass.Forms.CheckBox chkVOLLinked2;
    private EllieMae.Encompass.Forms.CheckBox chkVOLLinked3;
    private EllieMae.Encompass.Forms.CheckBox chkVOLLinked4;
    private EllieMae.Encompass.Forms.CheckBox chkVOLLinked5;
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<ImageButton> assetDeleteButtons;
    private List<ImageButton> otherAssetDeleteButtons;
    private List<ImageButton> LiabilityDeleteButtons;
    private List<ImageButton> otherLiabilitDeleteButtons;
    private List<ImageButton> vomDeleteButtons;
    private string sourceEnable = string.Empty;
    private string sourceDisable = string.Empty;
    private Dictionary<string, bool> controlWithPermission = new Dictionary<string, bool>();

    public D1003_2020P3InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public D1003_2020P3InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public D1003_2020P3InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public D1003_2020P3InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (this.allFieldsAreReadonly)
        return ControlState.Disabled;
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "FM0156":
          if (this.loan.GetField("FM0155") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FM0157":
          if (this.loan.GetField("FM0158") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FM0228":
          if (this.loan.GetField("1825") == "2020" && this.loan.GetField("FM0128") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FM0256":
          if (this.loan.GetField("FM0255") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FM0257":
          if (this.loan.GetField("FM0258") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FM0328":
          if (this.loan.GetField("1825") == "2020" && this.loan.GetField("FM0128") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FM0356":
          if (this.loan.GetField("FM0355") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FM0357":
          if (this.loan.GetField("FM0358") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X110":
          if (this.VOMDoesNotApply("CoBorrower"))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X51":
          if (this.VOOADoesNotApply("Borrower"))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X52":
          if (this.VOOADoesNotApply("CoBorrower"))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X59":
          if (this.VOLDoesNotApply("Borrower"))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X60":
          if (this.VOLDoesNotApply("CoBorrower"))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X63":
          if (this.VOOLDoesNotApply("Borrower"))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X64":
          if (this.VOOLDoesNotApply("CoBorrower"))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X69":
          if (this.VOMDoesNotApply("Borrower"))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLAROA0104":
          if (this.loan.GetField("URLAROA0102") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLAROA0204":
          if (this.loan.GetField("URLAROA0202") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLAROA0304":
          if (this.loan.GetField("URLAROA0302") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLAROL0104":
          if (this.loan.GetField("URLAROL0102") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLAROL0204":
          if (this.loan.GetField("URLAROL0202") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLAROL0304":
          if (this.loan.GetField("URLAROL0302") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "attachliens01":
          if (this.FormIsForTemplate || string.IsNullOrEmpty(this.loan.GetField("FM0143")))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "attachliens02":
          if (this.FormIsForTemplate || string.IsNullOrEmpty(this.loan.GetField("FM0243")))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "attachliens03":
          if (this.FormIsForTemplate || string.IsNullOrEmpty(this.loan.GetField("FM0343")))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "importliab":
        case "ocredit":
        case "vcredit":
        case "vod":
        case "vol":
        case "vom":
        case "vooa":
        case "vool":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      this.ind = id.Substring(2, 2);
      if (!(id == "FM" + this.ind + "28"))
        return;
      if (val == "Y")
      {
        this.loan.Calculator.UpdateAccountName("11");
      }
      else
      {
        base.UpdateFieldValue("FM" + this.ind + "04", "");
        base.UpdateFieldValue("FM" + this.ind + "06", "");
        base.UpdateFieldValue("FM" + this.ind + "07", "");
        base.UpdateFieldValue("FM" + this.ind + "08", "");
      }
      if (!(this.loan.GetField("FM0128") != "Y") || !(this.loan.GetField("1825") == "2020"))
        return;
      int numberOfMortgages = this.loan.GetNumberOfMortgages();
      for (int i = 2; i <= numberOfMortgages; ++i)
      {
        if (this.loan.GetField("FM" + i.ToString("00") + "28") == "Y")
        {
          this.loan.MoveMortgageToTop(i);
          this.RefreshLoanContents();
          break;
        }
      }
    }

    private bool VOOADoesNotApply(string type)
    {
      return this.controlDoesNotApply(this.loan.GetNumberOfOtherAssets(), "URLAROA", "01", type);
    }

    private bool VOLDoesNotApply(string type)
    {
      return this.controlDoesNotApply(this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp(), "FL", "15", type);
    }

    private bool VOOLDoesNotApply(string type)
    {
      return this.controlDoesNotApply(this.loan.GetNumberOfOtherLiability(), "URLAROL", "01", type);
    }

    private bool VOMDoesNotApply(string type)
    {
      return this.controlDoesNotApply(this.loan.GetNumberOfMortgages(), "FM", "46", type);
    }

    private bool controlDoesNotApply(int recCount, string prefixID, string borTypeID, string type)
    {
      int num = recCount;
      for (int index = 1; index <= num; ++index)
      {
        string field = this.inputData.GetField(prefixID + index.ToString("00") + borTypeID);
        if (field == "Both" || field == type)
          return true;
      }
      return false;
    }

    internal override void CreateControls()
    {
      try
      {
        this.chkVOLLinked1 = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("CheckBox17");
        this.chkVOLLinked2 = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("CheckBox18");
        this.chkVOLLinked3 = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("CheckBox19");
        this.chkVOLLinked4 = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("CheckBox20");
        this.chkVOLLinked5 = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("CheckBox21");
        this.panelLocalAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 1; index <= 3; ++index)
        {
          this.panelLocalAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr" + (object) index));
          this.panelForeignAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr" + (object) index));
          if (this.panelForeignAddresses[index - 1] != null && this.panelLocalAddresses[index - 1] != null)
            this.panelForeignAddresses[index - 1].Position = this.panelLocalAddresses[index - 1].Position;
        }
        this.selectCountryButtons = new List<StandardButton>();
        for (int index = 1; index <= 3; ++index)
          this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_FM0" + (object) index + "57"));
        this.assetDeleteButtons = new List<ImageButton>();
        this.assetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Asset0"));
        this.assetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisAsset0"));
        this.assetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Asset1"));
        this.assetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisAsset1"));
        this.assetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Asset2"));
        this.assetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisAsset2"));
        this.assetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Asset3"));
        this.assetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisAsset3"));
        this.assetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Asset4"));
        this.assetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisAsset4"));
        if (this.assetDeleteButtons.Count >= 2)
        {
          this.sourceEnable = this.assetDeleteButtons[0].Source;
          this.sourceDisable = this.assetDeleteButtons[1].Source;
        }
        this.otherAssetDeleteButtons = new List<ImageButton>();
        this.otherAssetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("OtherAsset0"));
        this.otherAssetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisOtherAsset0"));
        this.otherAssetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("OtherAsset1"));
        this.otherAssetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisOtherAsset1"));
        this.otherAssetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("OtherAsset2"));
        this.otherAssetDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisOtherAsset2"));
        this.LiabilityDeleteButtons = new List<ImageButton>();
        this.LiabilityDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Liability0"));
        this.LiabilityDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisLiability0"));
        this.LiabilityDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Liability1"));
        this.LiabilityDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisLiability1"));
        this.LiabilityDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Liability2"));
        this.LiabilityDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisLiability2"));
        this.LiabilityDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Liability3"));
        this.LiabilityDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisLiability3"));
        this.LiabilityDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Liability4"));
        this.LiabilityDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisLiability4"));
        this.otherLiabilitDeleteButtons = new List<ImageButton>();
        this.otherLiabilitDeleteButtons.Add((ImageButton) this.currentForm.FindControl("OtherLiability0"));
        this.otherLiabilitDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisOtherLiability0"));
        this.otherLiabilitDeleteButtons.Add((ImageButton) this.currentForm.FindControl("OtherLiability1"));
        this.otherLiabilitDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisOtherLiability1"));
        this.otherLiabilitDeleteButtons.Add((ImageButton) this.currentForm.FindControl("OtherLiability2"));
        this.otherLiabilitDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisOtherLiability2"));
        this.vomDeleteButtons = new List<ImageButton>();
        this.vomDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Property0"));
        this.vomDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisProperty0"));
        this.vomDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Property1"));
        this.vomDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisProperty1"));
        this.vomDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Property2"));
        this.vomDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisProperty2"));
        if (this.assetDeleteButtons.Count >= 10)
          this.PopulateControlPermission(this.assetDeleteButtons, 10);
        if (this.otherAssetDeleteButtons.Count >= 6)
          this.PopulateControlPermission(this.otherAssetDeleteButtons, 6);
        if (this.LiabilityDeleteButtons.Count >= 10)
          this.PopulateControlPermission(this.LiabilityDeleteButtons, 10);
        if (this.otherLiabilitDeleteButtons.Count >= 6)
          this.PopulateControlPermission(this.otherLiabilitDeleteButtons, 6);
        if (this.vomDeleteButtons.Count < 6)
          return;
        this.PopulateControlPermission(this.vomDeleteButtons, 6);
      }
      catch (Exception ex)
      {
      }
    }

    private void PopulateControlPermission(List<ImageButton> buttons, int Count)
    {
      string empty = string.Empty;
      for (int index = 0; index < Count; index += 2)
      {
        BizRule.FieldAccessRight fieldAccessRights = this.session.LoanDataMgr.GetFieldAccessRights("Button_" + buttons[index].Action);
        this.controlWithPermission.Add(buttons[index].Action, fieldAccessRights == BizRule.FieldAccessRight.ViewOnly);
      }
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.UpdateVOLLinkCheckboxes();
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButtons != null && this.selectCountryButtons.Count > 0)
      {
        for (int index = 0; index < this.selectCountryButtons.Count; ++index)
        {
          if (this.selectCountryButtons[index] != null)
          {
            bool flag = this.GetField("FM0" + (object) (index + 1) + "58") == "Y";
            this.selectCountryButtons[index].Visible = flag;
            this.panelLocalAddresses[index].Visible = !flag;
            this.panelForeignAddresses[index].Visible = flag;
          }
        }
      }
      if (this.loan != null && this.loan.IsInFindFieldForm)
      {
        for (int index = 0; index < this.assetDeleteButtons.Count; index += 2)
          this.assetDeleteButtons[index].Source = this.sourceEnable;
        for (int index = 0; index < this.otherAssetDeleteButtons.Count; index += 2)
          this.otherAssetDeleteButtons[index].Source = this.sourceEnable;
        for (int index = 0; index < this.LiabilityDeleteButtons.Count; index += 2)
          this.LiabilityDeleteButtons[index].Source = this.sourceEnable;
        for (int index = 0; index < this.otherLiabilitDeleteButtons.Count; index += 2)
          this.otherLiabilitDeleteButtons[index].Source = this.sourceEnable;
        for (int index = 0; index < this.vomDeleteButtons.Count; index += 2)
          this.vomDeleteButtons[index].Source = this.sourceEnable;
      }
      else
      {
        if (this.assetDeleteButtons != null && this.assetDeleteButtons.Count >= 10)
          this.SwitchImageControl(this.assetDeleteButtons, 8, this.loan.GetNumberOfDeposits());
        if (this.otherAssetDeleteButtons != null && this.otherAssetDeleteButtons.Count >= 6)
          this.SwitchImageControl(this.otherAssetDeleteButtons, 4, this.loan.GetNumberOfOtherAssets());
        if (this.LiabilityDeleteButtons != null && this.LiabilityDeleteButtons.Count >= 10)
          this.SwitchImageControl(this.LiabilityDeleteButtons, 8, this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp());
        if (this.otherLiabilitDeleteButtons != null && this.otherLiabilitDeleteButtons.Count >= 6)
          this.SwitchImageControl(this.otherLiabilitDeleteButtons, 4, this.loan.GetNumberOfOtherLiability());
        if (this.vomDeleteButtons == null || this.vomDeleteButtons.Count < 6)
          return;
        this.SwitchImageControl(this.vomDeleteButtons, 4, this.loan.GetNumberOfMortgages());
      }
    }

    internal void SwitchImageControl(List<ImageButton> imageButton, int totalIndex, int totalCount)
    {
      int num = 1;
      for (int index = 0; index <= totalIndex; index += 2)
      {
        bool result = totalCount >= num++;
        this.EnableDisableControl(imageButton[index], this.controlWithPermission, result, this.sourceEnable, this.sourceDisable);
      }
    }

    internal void UpdateVOLLinkCheckboxes()
    {
      this.chkVOLLinked1.Checked = !string.IsNullOrEmpty(this.inputData.GetField("FL0125"));
      this.chkVOLLinked2.Checked = !string.IsNullOrEmpty(this.inputData.GetField("FL0225"));
      this.chkVOLLinked3.Checked = !string.IsNullOrEmpty(this.inputData.GetField("FL0325"));
      this.chkVOLLinked4.Checked = !string.IsNullOrEmpty(this.inputData.GetField("FL0425"));
      if (!string.IsNullOrEmpty(this.inputData.GetField("FL0525")))
        this.chkVOLLinked5.Checked = true;
      else
        this.chkVOLLinked5.Checked = false;
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 595349256:
          if (!(action == "Property0"))
            return;
          goto label_43;
        case 612126875:
          if (!(action == "Property1"))
            return;
          goto label_43;
        case 625538672:
          if (!(action == "Liability4"))
            return;
          goto label_41;
        case 628904494:
          if (!(action == "Property2"))
            return;
          goto label_43;
        case 692649148:
          if (!(action == "Liability0"))
            return;
          goto label_41;
        case 709426767:
          if (!(action == "Liability1"))
            return;
          goto label_41;
        case 726204386:
          if (!(action == "Liability2"))
            return;
          goto label_41;
        case 742982005:
          if (!(action == "Liability3"))
            return;
          goto label_41;
        case 1991246971:
          if (!(action == "OtherAsset2"))
            return;
          goto label_40;
        case 2008024590:
          if (!(action == "OtherAsset1"))
            return;
          goto label_40;
        case 2024802209:
          if (!(action == "OtherAsset0"))
            return;
          goto label_40;
        case 2825478808:
          if (!(action == "Asset3"))
            return;
          break;
        case 2842256427:
          if (!(action == "Asset2"))
            return;
          break;
        case 2859034046:
          if (!(action == "Asset1"))
            return;
          break;
        case 2875811665:
          if (!(action == "Asset0"))
            return;
          break;
        case 2942922141:
          if (!(action == "Asset4"))
            return;
          break;
        case 3852682476:
          if (!(action == "OtherLiability0"))
            return;
          goto label_42;
        case 3869460095:
          if (!(action == "OtherLiability1"))
            return;
          goto label_42;
        case 3886237714:
          if (!(action == "OtherLiability2"))
            return;
          goto label_42;
        default:
          return;
      }
      this.RemoveVerifRecords(D1003_2020P3InputHandler.VerifType.Asset, int.Parse(action.Substring(5)));
      return;
label_40:
      this.RemoveVerifRecords(D1003_2020P3InputHandler.VerifType.OtherAsset, int.Parse(action.Substring(10)));
      return;
label_41:
      this.RemoveVerifRecords(D1003_2020P3InputHandler.VerifType.Liability, int.Parse(action.Substring(9)));
      return;
label_42:
      this.RemoveVerifRecords(D1003_2020P3InputHandler.VerifType.OtherLiability, int.Parse(action.Substring(14)));
      return;
label_43:
      this.RemoveVerifRecords(D1003_2020P3InputHandler.VerifType.Property, int.Parse(action.Substring(8)));
    }

    private void RemoveVerifRecords(D1003_2020P3InputHandler.VerifType verifType, int index)
    {
      if (Utils.Dialog((IWin32Window) this.session.MainForm, "Are you sure you want to delete this record from the verification?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      switch (verifType)
      {
        case D1003_2020P3InputHandler.VerifType.Asset:
          this.loan.RemoveDepositAt(index);
          break;
        case D1003_2020P3InputHandler.VerifType.OtherAsset:
          this.loan.RemoveOtherAssetAt(index);
          break;
        case D1003_2020P3InputHandler.VerifType.Liability:
          this.loan.RemoveLiabilityAt(index);
          break;
        case D1003_2020P3InputHandler.VerifType.OtherLiability:
          this.loan.RemoveOtherLiabilityAt(index);
          break;
        case D1003_2020P3InputHandler.VerifType.Property:
          this.loan.RemoveMortgageAt(index);
          break;
      }
      this.RefreshContents();
    }

    private enum VerifType
    {
      Asset,
      OtherAsset,
      Liability,
      OtherLiability,
      Property,
    }
  }
}
