// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.ClosingDocumentDataHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class ClosingDocumentDataHandler : WinFormInputHandler
  {
    private DocEngineFieldData docFieldData;
    private bool readOnly;
    private bool isDirty;
    private List<SwitchedControl> fieldControls = new List<SwitchedControl>();

    public ClosingDocumentDataHandler(
      LoanData loan,
      DocEngineFieldData docFieldData,
      bool readOnly)
      : base((IHtmlInput) loan)
    {
      this.docFieldData = docFieldData;
      this.readOnly = readOnly;
    }

    public bool IsDirty => this.isDirty;

    public override void Attach(System.Windows.Forms.Control formControl, ToolTip toolTip)
    {
      this.prepareSwitchedControls(formControl, toolTip);
      base.Attach(formControl, toolTip);
      this.isDirty = false;
    }

    public void ApplyDocEngineFieldValues(DocEngineFieldData fieldData)
    {
      this.docFieldData = fieldData;
      foreach (SwitchedControl fieldControl in this.fieldControls)
      {
        if (fieldControl.SwitchState == SwitchButtonState.Off)
          this.PopulateControls((System.Windows.Forms.Control) fieldControl);
      }
    }

    private void prepareSwitchedControls(System.Windows.Forms.Control ctrl, ToolTip toolTip)
    {
      if (ctrl is SwitchedControl)
      {
        this.prepareSwitchedControl(ctrl as SwitchedControl, toolTip);
      }
      else
      {
        if (ctrl.Controls.Count <= 0)
          return;
        foreach (System.Windows.Forms.Control control in (ArrangedElementCollection) ctrl.Controls)
          this.prepareSwitchedControls(control, toolTip);
      }
    }

    private void prepareSwitchedControl(SwitchedControl ctrl, ToolTip toolTip)
    {
      ctrl.AutoEnableDisable = true;
      ctrl.SwitchClick += new EventHandler(this.onSwitchClick);
      ctrl.SwitchToolTipOn = "Currently displaying loan data. Toggle to clear the loan field and use the ICE Mortgage Technology Doc Engine data.";
      ctrl.SwitchToolTipOff = "Currently displaying ICE Mortgage Technology Doc Engine data. Toggle to override this field and save your value to the loan.";
      ctrl.ToolTip = toolTip;
      string fieldId = this.GetFieldID((System.Windows.Forms.Control) ctrl);
      if (fieldId != "")
      {
        foreach (System.Windows.Forms.Control childDataControl in ctrl.GetChildDataControls())
        {
          if (this.GetFieldID(childDataControl) == "")
            this.SetFieldID(childDataControl, fieldId);
        }
        ctrl.SwitchState = ((LoanData) this.InputData).GetDocFieldUserOverride(fieldId) ? SwitchButtonState.On : SwitchButtonState.Off;
      }
      this.fieldControls.Add(ctrl);
    }

    public override bool IsFieldControl(System.Windows.Forms.Control c)
    {
      return c is SwitchedYesNo || base.IsFieldControl(c);
    }

    protected override void InitializeFieldControl(System.Windows.Forms.Control ctrl)
    {
      base.InitializeFieldControl(ctrl);
      ctrl.TextChanged += new EventHandler(this.onFieldValueChanged);
      switch (ctrl)
      {
        case ComboBox _:
          ((ComboBox) ctrl).SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
          break;
        case System.Windows.Forms.RadioButton _:
          ((System.Windows.Forms.RadioButton) ctrl).CheckedChanged += new EventHandler(this.onFieldValueChanged);
          break;
        case System.Windows.Forms.CheckBox _:
          ((System.Windows.Forms.CheckBox) ctrl).CheckedChanged += new EventHandler(this.onFieldValueChanged);
          break;
      }
    }

    private void onFieldValueChanged(object sender, EventArgs e) => this.isDirty = true;

    private void onSwitchClick(object sender, EventArgs e)
    {
      SwitchedControl switchedControl = (SwitchedControl) sender;
      if (this.IsFieldControl((System.Windows.Forms.Control) switchedControl))
        this.applyOverrideToControl((System.Windows.Forms.Control) switchedControl, switchedControl.SwitchState == SwitchButtonState.On);
      else
        this.applyOverrideToControls((IEnumerable) switchedControl.GetChildDataControls(), switchedControl.SwitchState == SwitchButtonState.On);
      this.isDirty = true;
    }

    private void applyOverrideToControls(IEnumerable controls, bool isOverridden)
    {
      foreach (System.Windows.Forms.Control control in controls)
      {
        if (this.IsFieldControl(control))
          this.applyOverrideToControl(control, isOverridden);
        else if (control.Controls.Count > 0)
          this.applyOverrideToControls((IEnumerable) control.Controls, isOverridden);
      }
    }

    private void applyOverrideToControl(System.Windows.Forms.Control ctrl, bool isOverridden)
    {
      string fieldId = this.GetFieldID(ctrl);
      if (isOverridden)
      {
        string field = this.InputData.GetField(fieldId);
        if (!(field != ""))
          return;
        this.SetControlValue(ctrl, field);
      }
      else
      {
        string settingsValue = this.docFieldData.GetSettingsValue(fieldId);
        this.SetControlValue(ctrl, settingsValue);
      }
    }

    private SwitchedControl getParentSwitchedControl(System.Windows.Forms.Control ctrl)
    {
      System.Windows.Forms.Control parentSwitchedControl = ctrl;
      while (true)
      {
        switch (parentSwitchedControl)
        {
          case null:
          case SwitchedControl _:
            goto label_3;
          default:
            parentSwitchedControl = parentSwitchedControl.Parent;
            continue;
        }
      }
label_3:
      return (SwitchedControl) parentSwitchedControl;
    }

    public override void CommitValues()
    {
      base.CommitValues();
      this.isDirty = false;
    }

    protected override void OnDataBind(System.Windows.Forms.Control ctrl, DataBindEventArgs e)
    {
      SwitchedControl parentSwitchedControl = this.getParentSwitchedControl(ctrl);
      if (parentSwitchedControl != null && parentSwitchedControl.SwitchState == SwitchButtonState.Off)
        e.Value = this.docFieldData.GetSettingsValue(this.GetFieldID(ctrl));
      base.OnDataBind(ctrl, e);
      if (!this.readOnly)
        return;
      ctrl.Enabled = false;
    }

    protected override void OnDataCommit(System.Windows.Forms.Control ctrl, DataCommitEventArgs e)
    {
      if (this.readOnly)
        return;
      SwitchedControl parentSwitchedControl = this.getParentSwitchedControl(ctrl);
      if (parentSwitchedControl != null)
      {
        ((LoanData) this.InputData).SetDocFieldUserOverride(this.GetFieldID(ctrl), parentSwitchedControl.SwitchState == SwitchButtonState.On);
        if (parentSwitchedControl.SwitchState == SwitchButtonState.Off)
          e.Value = "";
      }
      base.OnDataCommit(ctrl, e);
    }
  }
}
