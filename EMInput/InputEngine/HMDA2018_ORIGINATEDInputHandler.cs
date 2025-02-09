// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HMDA2018_ORIGINATEDInputHandler
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
using System.Globalization;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class HMDA2018_ORIGINATEDInputHandler : InputHandlerBase
  {
    private GovernmentInfoInputHandler governmentInfoInputHandler;
    private EllieMae.Encompass.Forms.Panel panelBorScoreLock;
    private EllieMae.Encompass.Forms.Panel panelCobScoreLock;
    private EllieMae.Encompass.Forms.Panel panelBorScoreNoLock;
    private EllieMae.Encompass.Forms.Panel panelCobScoreNoLock;
    private EllieMae.Encompass.Forms.Panel panelDenialLock;
    private EllieMae.Encompass.Forms.Panel panelDenialNoLock;
    private EllieMae.Encompass.Forms.Panel panelNewRecommend;
    private EllieMae.Encompass.Forms.Panel panelOldRecommend;
    private List<DropdownBox> field_Credit2018;
    private List<DropdownBox> field_Credit2017;
    private DropdownBox field_HMDAProfilename;
    private EllieMae.Encompass.Forms.Label labelUniversalLoanID;
    private string _hmdaProfileid;

    public HMDA2018_ORIGINATEDInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public HMDA2018_ORIGINATEDInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public HMDA2018_ORIGINATEDInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public HMDA2018_ORIGINATEDInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      try
      {
        this.panelBorScoreLock = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelBorScoreLock");
        this.panelCobScoreLock = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelCobScoreLock");
        this.panelBorScoreNoLock = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelBorScoreNoLock");
        this.panelCobScoreNoLock = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelCobScoreNoLock");
        this.panelDenialLock = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelDenialWithLock");
        this.panelDenialNoLock = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelDenialNoLock");
        this.panelNewRecommend = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelNewRecommend");
        this.panelOldRecommend = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelOldRecommend");
        this.HMDAMappingLocks();
        if (this.panelBorScoreNoLock != null && this.panelBorScoreLock != null)
        {
          this.panelBorScoreNoLock.Left = this.panelBorScoreLock.Left;
          this.panelBorScoreNoLock.Top = this.panelBorScoreLock.Top;
        }
        if (this.panelCobScoreNoLock != null && this.panelCobScoreLock != null)
        {
          this.panelCobScoreNoLock.Left = this.panelCobScoreLock.Left;
          this.panelCobScoreNoLock.Top = this.panelCobScoreLock.Top;
        }
        if (this.panelDenialLock != null && this.panelDenialNoLock != null)
        {
          this.panelDenialNoLock.Left = this.panelDenialLock.Left;
          this.panelDenialNoLock.Top = this.panelDenialLock.Top;
        }
        if (this.panelOldRecommend != null && this.panelNewRecommend != null)
        {
          this.panelOldRecommend.Left = this.panelNewRecommend.Left;
          this.panelOldRecommend.Top = this.panelNewRecommend.Top;
        }
        this.field_Credit2017 = new List<DropdownBox>();
        this.field_Credit2017.Add((DropdownBox) this.currentForm.FindControl("l_HMDAX116_noLock"));
        this.field_Credit2017.Add((DropdownBox) this.currentForm.FindControl("l_HMDAX118_noLock"));
        this.field_Credit2018 = new List<DropdownBox>();
        this.field_Credit2018.Add((DropdownBox) this.currentForm.FindControl("l_HMDAX116"));
        this.field_Credit2018.Add((DropdownBox) this.currentForm.FindControl("l_HMDAX118"));
        this.field_HMDAProfilename = (DropdownBox) this.currentForm.FindControl("hmdaprofilename");
        this.labelUniversalLoanID = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelUniversalLoanID");
        this.field_HMDAProfilename.Change += new EventHandler(this.field_HMDAProfilename_Change);
        this.fillHMDAProfileList();
        if (this.governmentInfoInputHandler == null)
          this.governmentInfoInputHandler = new GovernmentInfoInputHandler(this.currentForm, this.inputData, this.ToString());
        if (this.loan == null || this.loan.Calculator == null)
          return;
        this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.PaymentSchedule);
        if (this.loan.GetField("3296") == string.Empty && this.loan.GetField("608") == "AdjustableRate")
          this.loan.Calculator.CalcRateCap();
        this.loan.Calculator.FormCalculation("POPULATESUBJECTPROPERTYADDRESS", (string) null, (string) null);
      }
      catch (Exception ex)
      {
      }
    }

    private void fillHMDAProfileList()
    {
      List<HMDAProfile> hmdaProfile1 = Session.ConfigurationManager.GetHMDAProfile();
      if (hmdaProfile1 != null && hmdaProfile1.Count > 0)
      {
        foreach (HMDAProfile hmdaProfile2 in hmdaProfile1)
          this.field_HMDAProfilename.Options.Add(hmdaProfile2.HMDAProfileName, hmdaProfile2.HMDAProfileID.ToString());
      }
      try
      {
        this._hmdaProfileid = this.GetField("HMDA.X100");
        if (hmdaProfile1.Count == 1)
        {
          this.SetField("HMDA.X100", this.field_HMDAProfilename.Value.ToString());
          HMDAProfile hmdaProfileById = Session.ConfigurationManager.GetHMDAProfileById(Utils.ParseInt((object) this.field_HMDAProfilename.Value.ToString()));
          if (hmdaProfileById != null)
          {
            this.loan.Settings.HMDAInfo = new HMDAInformation(hmdaProfileById.HMDAProfileSetting);
            this.loan.Settings.HMDAInfo.HMDAProfileID = hmdaProfileById.HMDAProfileID.ToString();
          }
        }
        else if (hmdaProfile1.Count > 1 && string.IsNullOrEmpty(this._hmdaProfileid))
          this.field_HMDAProfilename.Options.Add("", "");
        if (!string.IsNullOrEmpty(this._hmdaProfileid))
        {
          this.field_HMDAProfilename.SelectedIndex = this.field_HMDAProfilename.Options.IndexOf(new DropdownOption(this._hmdaProfileid));
          this.loan.Calculator.FormCalculation("UPDATEHMDA2018");
        }
        else
          this.field_HMDAProfilename.SelectedIndex = this.field_HMDAProfilename.Options.IndexOf(new DropdownOption(""));
      }
      catch
      {
      }
    }

    private void field_HMDAProfilename_Change(object sender, EventArgs e)
    {
      bool flag = false;
      if (this._hmdaProfileid == this.field_HMDAProfilename.Value)
        return;
      if (string.IsNullOrEmpty(this.GetField("HMDA.X28")))
        flag = true;
      else if (DialogResult.OK == Utils.Dialog((IWin32Window) this.mainScreen, "Do you wish to update Universal Loan ID already calculated for this loan?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
      {
        flag = true;
      }
      else
      {
        try
        {
          this._hmdaProfileid = this.GetField("HMDA.X100");
          if (!string.IsNullOrEmpty(this._hmdaProfileid))
            this.field_HMDAProfilename.SelectedIndex = this.field_HMDAProfilename.Options.IndexOf(new DropdownOption(this._hmdaProfileid));
        }
        catch
        {
        }
      }
      if (!flag)
        return;
      this.SetField("HMDA.X100", this.field_HMDAProfilename.Value.ToString());
      this._hmdaProfileid = this.field_HMDAProfilename.Value.ToString();
      HMDAProfile hmdaProfileById = Session.ConfigurationManager.GetHMDAProfileById(Utils.ParseInt((object) this.field_HMDAProfilename.Value.ToString()));
      if (hmdaProfileById != null)
      {
        this.loan.Settings.HMDAInfo = new HMDAInformation(hmdaProfileById.HMDAProfileSetting);
        this.loan.Settings.HMDAInfo.HMDAProfileID = hmdaProfileById.HMDAProfileID.ToString();
      }
      this.loan.Calculator.FormCalculation("UPDATEHMDA2018");
      this.RefreshContents();
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      bool flag1 = this.GetField("1393") == "Application withdrawn" || this.GetField("1393") == "File Closed for incompleteness" || this.GetField("1393") == "Loan purchased by your institution";
      bool flag2 = flag1 || this.GetField("3840") == "Y";
      if (this.panelBorScoreLock != null)
      {
        this.panelBorScoreLock.Visible = flag1;
        if (this.field_Credit2018[0] != null)
          this.field_Credit2018[0].Visible = flag1;
      }
      if (this.panelCobScoreLock != null)
      {
        this.panelCobScoreLock.Visible = flag2;
        if (this.field_Credit2018[1] != null)
          this.field_Credit2018[1].Visible = flag2;
      }
      if (this.panelBorScoreNoLock != null)
      {
        this.panelBorScoreNoLock.Visible = !flag1;
        if (this.field_Credit2017[0] != null)
          this.field_Credit2017[0].Visible = !flag1;
      }
      if (this.panelCobScoreNoLock != null)
      {
        this.panelCobScoreNoLock.Visible = !flag2;
        if (this.field_Credit2017[1] != null)
          this.field_Credit2017[1].Visible = !flag2;
      }
      bool flag3 = this.loan != null && this.loan.Calculator != null && this.loan.Calculator.ValidateHmdaActioForDenialReasons();
      if (this.panelDenialNoLock != null)
        this.panelDenialNoLock.Visible = flag3;
      if (this.panelDenialLock != null)
        this.panelDenialLock.Visible = !flag3;
      bool flag4 = Utils.ParseInt((object) this.GetField("HMDA.X27")) < 2019;
      if (this.panelOldRecommend != null)
        this.panelOldRecommend.Visible = flag4;
      if (this.panelNewRecommend != null)
        this.panelNewRecommend.Visible = !flag4;
      this.labelUniversalLoanID.Text = this.loan == null || this.loan.Settings.HMDAInfo == null || !this.loan.Settings.HMDAInfo.HMDANuli || !(this.loan.GetField("HMDA.X113") == "Y") ? "Universal Loan ID" : "Non Universal Loan ID";
      this.governmentInfoInputHandler.SetLayout((InputHandlerBase) this);
      this.HMDAMappingLocks();
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      this.RefreshContents();
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      int num = this.GetField("HMDA.X27") == string.Empty ? 0 : (int) short.Parse(this.GetField("HMDA.X27"), NumberStyles.AllowThousands);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(id))
      {
        case 15589259:
          if (id == "HMDA.X88")
          {
            if (this.inputData.GetField("HMDA.X91") == "Y")
            {
              controlState = ControlState.Disabled;
              goto label_135;
            }
            else
              goto label_135;
          }
          else
            goto default;
        case 519214426:
          if (id == "3840")
          {
            if (this.GetField("3840") != "Y")
            {
              this.SetControlState("panelCoBorrowerEthnicity", true);
              goto label_135;
            }
            else
            {
              this.SetControlState("panelCoBorrowerEthnicity", false);
              goto label_135;
            }
          }
          else
            goto default;
        case 909199398:
          if (id == "generateuli")
          {
            if (string.IsNullOrEmpty(this.GetField("HMDA.X70")) && string.IsNullOrEmpty(this.GetField("HMDA.X106")) || string.IsNullOrEmpty(this.GetField("364")) || this.loan.IsLocked("HMDA.X28"))
            {
              controlState = ControlState.Disabled;
              goto label_135;
            }
            else
              goto label_135;
          }
          else
            goto default;
        case 1375179504:
          if (id == "4160")
            goto label_116;
          else
            goto default;
        case 1391957123:
          if (id == "4161")
            goto label_116;
          else
            goto default;
        case 1408734742:
          if (id == "4162")
            goto label_116;
          else
            goto default;
        case 1425512361:
          if (id == "4163")
            goto label_121;
          else
            goto default;
        case 1425659456:
          if (id == "4173")
            goto label_121;
          else
            goto default;
        case 1442289980:
          if (id == "4164")
            goto label_121;
          else
            goto default;
        case 1442437075:
          if (id == "4172")
            goto label_121;
          else
            goto default;
        case 1459067599:
          if (id == "4165")
            goto label_121;
          else
            goto default;
        case 1459214694:
          if (id == "4171")
            goto label_121;
          else
            goto default;
        case 1475845218:
          if (id == "4166")
            goto label_121;
          else
            goto default;
        case 1475992313:
          if (id == "4170")
            goto label_121;
          else
            goto default;
        case 1492622837:
          if (id == "4167")
            goto label_121;
          else
            goto default;
        case 1509400456:
          if (id == "4168")
            goto label_121;
          else
            goto default;
        case 1509547551:
          if (id == "4176")
          {
            controlState = !(this.GetField("HMDA.X117") == "Other credit scoring model") ? ControlState.Disabled : ControlState.Enabled;
            goto label_135;
          }
          else
            goto default;
        case 1526178075:
          if (id == "4169")
            goto label_121;
          else
            goto default;
        case 1593435646:
          if (id == "4179")
          {
            controlState = !(this.GetField("HMDA.X119") != "Other credit scoring model") ? ControlState.Enabled : ControlState.Disabled;
            goto label_135;
          }
          else
            goto default;
        case 1662811788:
          if (id == "4199")
            goto label_109;
          else
            goto default;
        case 1679589407:
          if (id == "4198")
            goto label_109;
          else
            goto default;
        case 1729922264:
          if (id == "4195")
            break;
          goto default;
        case 1735842832:
          if (id == "4252")
            goto label_120;
          else
            goto default;
        case 1746699883:
          if (id == "4194")
            break;
          goto default;
        case 1752620451:
          if (id == "4253")
            goto label_121;
          else
            goto default;
        case 1763477502:
          if (id == "4197")
            goto label_109;
          else
            goto default;
        case 1830587978:
          if (id == "4193")
            break;
          goto default;
        case 1835816998:
          if (id == "4214")
            goto label_116;
          else
            goto default;
        case 1886149855:
          if (id == "4213")
            goto label_116;
          else
            goto default;
        case 1902927474:
          if (id == "4210")
            goto label_112;
          else
            goto default;
        case 1919705093:
          if (id == "4211")
            goto label_112;
          else
            goto default;
        case 2060634396:
          if (id == "HMDA.X34")
          {
            if (this.GetFieldValue("HMDA.X21") != "Other" && this.GetFieldValue("HMDA.X22") != "Other" && this.GetFieldValue("HMDA.X23") != "Other" && this.GetFieldValue("HMDA.X33") != "Other")
            {
              controlState = ControlState.Disabled;
              goto label_135;
            }
            else
              goto label_135;
          }
          else
            goto default;
        case 2095322467:
          if (id == "HMDA.X44")
            goto label_128;
          else
            goto default;
        case 2144522491:
          if (id == "HMDA.X39")
            goto label_128;
          else
            goto default;
        case 2162432943:
          if (id == "HMDA.X40")
            goto label_128;
          else
            goto default;
        case 2195988181:
          if (id == "HMDA.X42")
            goto label_128;
          else
            goto default;
        case 2243491555:
          if (id == "761")
          {
            if (this.GetField("3941") == "Y")
            {
              ((RuntimeControl) this.currentForm.FindControl("Calendar5")).Enabled = false;
              return ControlState.Disabled;
            }
            ((RuntimeControl) this.currentForm.FindControl("Calendar5")).Enabled = true;
            goto label_135;
          }
          else
            goto default;
        case 2279876276:
          if (id == "HMDA.X49")
          {
            if (this.GetFieldValue("HMDA.X44") != "Other" && this.GetFieldValue("HMDA.X45") != "Other" && this.GetFieldValue("HMDA.X46") != "Other" && this.GetFieldValue("HMDA.X47") != "Other" && this.GetFieldValue("HMDA.X48") != "Other")
            {
              controlState = ControlState.Disabled;
              goto label_135;
            }
            else
              goto label_135;
          }
          else
            goto default;
        case 2464871370:
          if (id == "HMDA.X90")
          {
            if (this.inputData.GetField("HMDA.X91") == "Y")
            {
              controlState = ControlState.Disabled;
              goto label_135;
            }
            else
              goto label_135;
          }
          else
            goto default;
        case 3150458397:
          if (id == "1825")
          {
            if (Utils.ParseInt((object) this.GetField("HMDA.X27")) >= 2018)
            {
              controlState = ControlState.Disabled;
              goto label_135;
            }
            else
              goto label_135;
          }
          else
            goto default;
        case 3455542607:
          if (id == "HMDA.X107")
          {
            if (this.GetField("1393") == "Loan purchased by your institution" && Utils.ParseDate((object) this.GetField("745")) < Utils.ParseDate((object) "01/01/2018"))
            {
              controlState = ControlState.Disabled;
              goto label_135;
            }
            else
              goto label_135;
          }
          else
            goto default;
        case 3504228646:
          if (id == "1529")
            goto label_120;
          else
            goto default;
        case 3513013582:
          if (id == "384")
          {
            if (Utils.ParseInt((object) this.GetField("HMDA.X27")) >= 2018)
            {
              controlState = !(this.inputData.GetField("1393") == "Loan purchased by your institution") || !(Utils.ParseDate((object) this.inputData.GetField("745")) < Utils.ParseDate((object) "01/01/2018")) ? ControlState.Default : ControlState.Disabled;
              goto label_135;
            }
            else
              goto label_135;
          }
          else
            goto default;
        case 3521006265:
          if (id == "1528")
            goto label_120;
          else
            goto default;
        case 3587969646:
          if (id == "1532")
            goto label_121;
          else
            goto default;
        case 3604747265:
          if (id == "1533")
            goto label_121;
          else
            goto default;
        case 3621524884:
          if (id == "1534")
            goto label_121;
          else
            goto default;
        case 3623674640:
          if (id == "4146")
            goto label_112;
          else
            goto default;
        case 3638302503:
          if (id == "1535")
            goto label_121;
          else
            goto default;
        case 3640452259:
          if (id == "4147")
            goto label_112;
          else
            goto default;
        case 3655080122:
          if (id == "1536")
            goto label_121;
          else
            goto default;
        case 3657229878:
          if (id == "4144")
            goto label_112;
          else
            goto default;
        case 3671857741:
          if (id == "1537")
            goto label_121;
          else
            goto default;
        case 3672004836:
          if (id == "1527")
            goto label_120;
          else
            goto default;
        case 3674007497:
          if (id == "4145")
            goto label_112;
          else
            goto default;
        case 3674154592:
          if (id == "4159")
            goto label_116;
          else
            goto default;
        case 3688782455:
          if (id == "1526")
            goto label_120;
          else
            goto default;
        case 3690785116:
          if (id == "4142")
          {
            try
            {
              string field = this.GetField("HMDA.X100");
              if (!string.IsNullOrEmpty(field) && field != this._hmdaProfileid)
              {
                this._hmdaProfileid = field;
                this.field_HMDAProfilename.SelectedIndex = this.field_HMDAProfilename.Options.IndexOf(new DropdownOption(field));
              }
              if (Utils.ParseInt((object) this.GetField("1825")) >= 2020)
              {
                controlState = ControlState.Disabled;
                goto label_135;
              }
              else
                goto label_135;
            }
            catch
            {
              goto label_135;
            }
          }
          else
            goto default;
        case 3690932211:
          if (id == "4158")
            goto label_120;
          else
            goto default;
        case 3705560074:
          if (id == "1525")
            goto label_120;
          else
            goto default;
        case 3707562735:
          if (id == "4143")
          {
            bool enabled = !(this.GetField("4143") == "FaceToFace");
            this.SetControlState("chkBorEthInfoNotProvided", enabled);
            this.SetControlState("chkBorRaceInfoNotProvided", enabled);
            this.SetControlState("chkBorSexInfoNotProvided", enabled);
            controlState = ControlState.Default;
            goto label_135;
          }
          else
            goto default;
        case 3722337693:
          if (id == "1524")
            goto label_120;
          else
            goto default;
        case 3808375544:
          if (id == "4151")
            goto label_120;
          else
            goto default;
        case 3825153163:
          if (id == "4150")
            goto label_120;
          else
            goto default;
        case 3841930782:
          if (id == "4153")
            goto label_120;
          else
            goto default;
        case 3858561306:
          if (id == "4148")
            goto label_120;
          else
            goto default;
        case 3858708401:
          if (id == "4152")
            goto label_120;
          else
            goto default;
        case 3875338925:
          if (id == "4149")
            goto label_120;
          else
            goto default;
        case 3875486020:
          if (id == "4155")
            goto label_120;
          else
            goto default;
        case 3892263639:
          if (id == "4154")
            goto label_120;
          else
            goto default;
        case 3909041258:
          if (id == "4157")
            goto label_120;
          else
            goto default;
        case 3925818877:
          if (id == "4156")
            goto label_120;
          else
            goto default;
        case 4010001162:
          if (id == "4131")
          {
            bool enabled = !(this.GetField("4131") == "FaceToFace");
            this.SetControlState("chkCoBorEthInfoNotProvided", enabled);
            this.SetControlState("chkCoBorRaceInfoNotProvided", enabled);
            this.SetControlState("chkCoBorSexInfoNotProvided", enabled);
            controlState = ControlState.Default;
            goto label_135;
          }
          else
            goto default;
        case 4201608372:
          if (id == "4205")
            goto label_112;
          else
            goto default;
        case 4209052198:
          if (id == "HMDA.X50")
            goto label_128;
          else
            goto default;
        case 4251941229:
          if (id == "4206")
            goto label_116;
          else
            goto default;
        case 4260223698:
          if (id == "HMDA.X87")
          {
            if (this.inputData.GetField("HMDA.X91") == "Y")
            {
              controlState = ControlState.Disabled;
              goto label_135;
            }
            else
              goto label_135;
          }
          else
            goto default;
        case 4292940293:
          if (id == "HMDA.X55")
          {
            if (this.GetFieldValue("HMDA.X50") != "Other" && this.GetFieldValue("HMDA.X51") != "Other" && this.GetFieldValue("HMDA.X52") != "Other" && this.GetFieldValue("HMDA.X53") != "Other" && this.GetFieldValue("HMDA.X54") != "Other")
            {
              controlState = ControlState.Disabled;
              goto label_135;
            }
            else
              goto label_135;
          }
          else
            goto default;
        case 4293778936:
          if (id == "HMDA.X89")
          {
            if (this.inputData.GetField("HMDA.X91") == "Y")
            {
              controlState = ControlState.Disabled;
              goto label_135;
            }
            else
              goto label_135;
          }
          else
            goto default;
        default:
          controlState = ControlState.Default;
          goto label_135;
      }
      if (this.GetField("4193") == "Y" || this.GetField("4194") == "Y" || this.GetField("4195") == "Y")
      {
        this.SetControlState("chkborsexnotapplicable", false);
        goto label_135;
      }
      else
      {
        this.SetControlState("chkborsexnotapplicable", true);
        goto label_135;
      }
label_109:
      if (this.GetField("4197") == "Y" || this.GetField("4198") == "Y" || this.GetField("4199") == "Y")
      {
        this.SetControlState("chkcoborsexnotapplicable", false);
        goto label_135;
      }
      else
      {
        this.SetControlState("chkcoborsexnotapplicable", true);
        goto label_135;
      }
label_112:
      if (this.GetField("4205") == "Y" || this.GetField("4210") == "Y" || this.GetField("4211") == "Y" || this.GetField("4144") == "Y" || this.GetField("4145") == "Y" || this.GetField("4146") == "Y" || this.GetField("4147") == "Y")
        this.SetControlState("chkEthnicityBorrNA", false);
      else
        this.SetControlState("chkEthnicityBorrNA", true);
      controlState = ControlState.Default;
      goto label_135;
label_116:
      if (this.GetField("4206") == "Y" || this.GetField("4213") == "Y" || this.GetField("4214") == "Y" || this.GetField("4159") == "Y" || this.GetField("4160") == "Y" || this.GetField("4161") == "Y" || this.GetField("4162") == "Y")
        this.SetControlState("chkEthnicityCoBorrNA", false);
      else
        this.SetControlState("chkEthnicityCoBorrNA", true);
      controlState = ControlState.Default;
      goto label_135;
label_120:
      bool enabled1 = !(this.GetField("1524") == "Y") && !(this.GetField("1525") == "Y") && !(this.GetField("1526") == "Y") && !(this.GetField("1527") == "Y") && !(this.GetField("1528") == "Y") && !(this.GetField("4148") == "Y") && !(this.GetField("4149") == "Y") && !(this.GetField("4150") == "Y") && !(this.GetField("4151") == "Y") && !(this.GetField("4152") == "Y") && !(this.GetField("4153") == "Y") && !(this.GetField("4154") == "Y") && !(this.GetField("4155") == "Y") && !(this.GetField("4156") == "Y") && !(this.GetField("4157") == "Y") && !(this.GetField("4158") == "Y") && !(this.GetField("4252") == "Y");
      this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox41" : "CheckBox15", enabled1);
      controlState = ControlState.Default;
      goto label_135;
label_121:
      bool enabled2 = !(this.GetField("1532") == "Y") && !(this.GetField("1533") == "Y") && !(this.GetField("1534") == "Y") && !(this.GetField("1535") == "Y") && !(this.GetField("1536") == "Y") && !(this.GetField("4163") == "Y") && !(this.GetField("4164") == "Y") && !(this.GetField("4165") == "Y") && !(this.GetField("4166") == "Y") && !(this.GetField("4167") == "Y") && !(this.GetField("4168") == "Y") && !(this.GetField("4169") == "Y") && !(this.GetField("4170") == "Y") && !(this.GetField("4171") == "Y") && !(this.GetField("4172") == "Y") && !(this.GetField("4173") == "Y") && !(this.GetField("4253") == "Y");
      this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox71" : "CheckBox19", enabled2);
      controlState = ControlState.Default;
      goto label_135;
label_128:
      controlState = this.loan == null || this.loan.Calculator == null || !this.loan.Calculator.ValidateHmdaCalculation(id) || this.loan.IsLocked(id) ? ControlState.Default : ControlState.Disabled;
label_135:
      return controlState;
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "generateuli":
          this.showHMDAGenerateULIDialog();
          break;
        case "applypartialexemption":
        case "recalculatehmda":
          base.ExecAction(action);
          this.HMDAMappingLocks();
          break;
        default:
          base.ExecAction(action);
          break;
      }
    }

    private void HMDAMappingLocks()
    {
      this.ShowHmdaLock("HMDA.X39", "FieldLock40", "hmdax39NoLock");
      this.ShowHmdaLock("HMDA.X40", "FieldLock41", "hmdax40NoLock");
      this.ShowHmdaLock("HMDA.X42", "FieldLock42", "hmdax42NoLock");
      this.ShowHmdaLock("HMDA.X44", "FieldLock43", "hmdax44NoLock");
      if (this.panelNewRecommend.Visible)
        this.ShowHmdaLock("HMDA.X50", "FieldLock44", "hmdax50NoLockNew");
      else
        this.ShowHmdaLock("HMDA.X50", "FieldLock24", "hmdax50NoLockOld");
      this.ShowHmdaLock("HMDA.X86", "FieldLock37", "hmdax86NoLock");
    }

    private void showHMDAGenerateULIDialog()
    {
      using (HMDAGenerateULIDialog generateUliDialog = new HMDAGenerateULIDialog(this.loan))
      {
        if (generateUliDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        this.UpdateContents();
      }
    }
  }
}
