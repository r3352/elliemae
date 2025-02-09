// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.UNDERWRITERSUMMARYInputHandler
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
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class UNDERWRITERSUMMARYInputHandler : InputHandlerBase
  {
    private List<FieldLock> fieldCreditLocks;
    private List<DropdownBox> field_Credit2018;
    private List<DropdownBox> field_Credit2017;
    private CategoryBox hmdaCategoryBox;
    private EllieMae.Encompass.Forms.Panel UWpnlForm;
    private EllieMae.Encompass.Forms.Label lblUwLabel;
    private EllieMae.Encompass.Forms.Button btnFundingImport;

    public UNDERWRITERSUMMARYInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public UNDERWRITERSUMMARYInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public UNDERWRITERSUMMARYInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public UNDERWRITERSUMMARYInputHandler(
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
        this.btnFundingImport = (EllieMae.Encompass.Forms.Button) this.currentForm.FindControl("btnFundingImport");
      }
      catch (Exception ex)
      {
      }
      if (this.btnFundingImport != null)
        this.btnFundingImport.Visible = this.session.UserInfo.IsAdministrator() || this.session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ToolsTab_FundingImportWS) && (bool) this.session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ToolsTab_FundingImportWS];
      try
      {
        this.fieldCreditLocks = new List<FieldLock>();
        this.fieldCreditLocks.Add((FieldLock) this.currentForm.FindControl("flock_4174"));
        this.fieldCreditLocks.Add((FieldLock) this.currentForm.FindControl("flock_4175"));
        this.fieldCreditLocks.Add((FieldLock) this.currentForm.FindControl("flock_4177"));
        this.fieldCreditLocks.Add((FieldLock) this.currentForm.FindControl("flock_4178"));
        this.field_Credit2017 = new List<DropdownBox>();
        this.field_Credit2017.Add((DropdownBox) this.currentForm.FindControl("l_4174_2017"));
        this.field_Credit2017.Add((DropdownBox) this.currentForm.FindControl("l_4175_2017"));
        this.field_Credit2017.Add((DropdownBox) this.currentForm.FindControl("l_4177_2017"));
        this.field_Credit2017.Add((DropdownBox) this.currentForm.FindControl("l_4178_2017"));
        this.field_Credit2018 = new List<DropdownBox>();
        this.field_Credit2018.Add((DropdownBox) this.currentForm.FindControl("l_4174"));
        this.field_Credit2018.Add((DropdownBox) this.currentForm.FindControl("l_4175"));
        this.field_Credit2018.Add((DropdownBox) this.currentForm.FindControl("l_4177"));
        this.field_Credit2018.Add((DropdownBox) this.currentForm.FindControl("l_4178"));
        this.UWpnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.hmdaCategoryBox = (CategoryBox) this.currentForm.FindControl("CategoryBoxHMDAFactors");
        this.lblUwLabel = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label112");
        for (int index = 0; index < this.field_Credit2017.Count; ++index)
        {
          this.field_Credit2017[index].Position = new Point(this.field_Credit2018[index].Left, this.field_Credit2018[index].Top);
          DropdownBox dropdownBox = this.field_Credit2017[index];
          Size size1 = this.field_Credit2018[index].Size;
          int width = size1.Width;
          size1 = this.field_Credit2018[index].Size;
          int height = size1.Height;
          Size size2 = new Size(width, height);
          dropdownBox.Size = size2;
        }
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "101":
        case "102":
        case "103":
        case "104":
        case "107":
        case "108":
        case "11":
        case "110":
        case "111":
        case "112":
        case "113":
        case "116":
        case "117":
        case "importfundingdetails":
          return controlState;
        case "2982":
          if (Utils.ParseInt((object) this.GetField("1177")) <= 0)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "364":
          if (this.FormIsForTemplate || InputHandlerBase.LockLoanNumber)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "4176":
          controlState = !(this.GetFieldValue("4175") == "Other credit scoring model") ? ControlState.Disabled : ControlState.Enabled;
          goto case "101";
        case "4179":
          controlState = !(this.GetFieldValue("4178") == "Other credit scoring model") ? ControlState.Disabled : ControlState.Enabled;
          goto case "101";
        case "4458":
          string fieldValue = this.GetFieldValue("4457");
          if (string.IsNullOrEmpty(fieldValue) || fieldValue == "//")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "762":
          if (this.GetField("3941") == "Y")
          {
            ((RuntimeControl) this.currentForm.FindControl("Calendar2")).Enabled = false;
            return ControlState.Disabled;
          }
          ((RuntimeControl) this.currentForm.FindControl("Calendar2")).Enabled = true;
          goto case "101";
        case "baseincome":
          if (this.GetField("1825") == "2020")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        default:
          controlState = ControlState.Default;
          goto case "101";
      }
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (!(action == "mtginsreserv"))
        return;
      if (this.inputData.IsLocked("232"))
        this.SetFieldFocus("l_232");
      else
        this.SetFieldFocus("l_L268");
    }

    protected override void UpdateContents()
    {
      base.UpdateContents();
      this.setLayout();
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      this.setLayout();
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.setLayout();
    }

    private void setLayout()
    {
      if (this.fieldCreditLocks == null)
        return;
      bool flag1 = Utils.ParseInt((object) this.GetFieldValue("HMDA.X27")) >= 2018 && (this.GetField("1393") == "File Closed for incompleteness" || this.GetField("1393") == "Loan purchased by your institution" || this.GetField("1393") == "Application withdrawn");
      bool flag2 = flag1 || this.GetField("3840") == "Y";
      for (int index = 0; index < this.fieldCreditLocks.Count - 2; ++index)
      {
        if (this.fieldCreditLocks[index] != null)
          this.fieldCreditLocks[index].Visible = flag1;
      }
      for (int index = 2; index < this.fieldCreditLocks.Count; ++index)
      {
        if (this.fieldCreditLocks[index] != null)
          this.fieldCreditLocks[index].Visible = flag2;
      }
      for (int index = 0; index < this.field_Credit2017.Count - 2; ++index)
      {
        if (this.field_Credit2017[index] != null)
          this.field_Credit2017[index].Visible = !flag1;
      }
      for (int index = 2; index < this.field_Credit2017.Count; ++index)
      {
        if (this.field_Credit2017[index] != null)
          this.field_Credit2017[index].Visible = !flag2;
      }
      for (int index = 0; index < this.field_Credit2018.Count - 2; ++index)
      {
        if (this.field_Credit2018[index] != null)
          this.field_Credit2018[index].Visible = flag1;
      }
      for (int index = 2; index < this.field_Credit2018.Count; ++index)
      {
        if (this.field_Credit2018[index] != null)
          this.field_Credit2018[index].Visible = flag2;
      }
      if (this.hmdaCategoryBox == null)
        return;
      bool flag3 = Utils.ParseInt((object) this.GetFieldValue("HMDA.X27")) >= 2018;
      this.hmdaCategoryBox.Visible = flag3;
      if (flag3)
        return;
      EllieMae.Encompass.Forms.Panel uwpnlForm = this.UWpnlForm;
      Size size1 = this.UWpnlForm.Size;
      int width = size1.Width;
      size1 = this.UWpnlForm.Size;
      int height1 = size1.Height;
      size1 = this.hmdaCategoryBox.Size;
      int height2 = size1.Height;
      int height3 = height1 - height2;
      Size size2 = new Size(width, height3);
      uwpnlForm.Size = size2;
      this.lblUwLabel.Position = new Point(this.UWpnlForm.Left, this.UWpnlForm.Bottom + 7);
    }
  }
}
