// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HUD1ESInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class HUD1ESInputHandler : InputHandlerBase
  {
    private Color PREPAIDCOLOR = Color.FromArgb(221, 233, 249);
    private Color UNPREPAIDCOLOR = Color.WhiteSmoke;

    public HUD1ESInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public HUD1ESInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      this.loan.Calculator.FormCalculation("HUD1ES", (string) null, (string) null);
      this.UpdateContents();
    }

    public HUD1ESInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public HUD1ESInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
      if (dataTemplate.Calculator != null)
        dataTemplate.Calculator.FormCalculation("HUD1ES", (string) null, (string) null);
      this.UpdateContents();
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (id == "HUD69")
      {
        if (this.GetField("19") != "ConstructionToPermanent")
          controlState = ControlState.Disabled;
      }
      else
        controlState = ControlState.Default;
      return controlState;
    }

    protected override void UpdateContents()
    {
      base.UpdateContents();
      foreach (EllieMae.Encompass.Forms.Control allControl in this.currentForm.GetAllControls())
      {
        try
        {
          if (allControl is FieldControl)
          {
            FieldControl fieldControl = (FieldControl) allControl;
            if (fieldControl.Field.FieldID.Length == 7)
            {
              int num1 = Utils.ParseInt((object) fieldControl.Field.FieldID.Substring(3, 2));
              int num2 = Utils.ParseInt((object) fieldControl.Field.FieldID.Substring(5, 2));
              if (num1 >= 1)
              {
                if (num1 <= 48)
                {
                  if (num2 >= 2)
                  {
                    if (num2 <= 9)
                    {
                      int num3 = num2 + 48;
                      if (this.GetFieldValue("HUD" + num1.ToString("00") + num3.ToString("00")) == "Y")
                        fieldControl.BackColor = this.PREPAIDCOLOR;
                      else if (fieldControl.BackColor == this.PREPAIDCOLOR)
                        fieldControl.BackColor = this.UNPREPAIDCOLOR;
                    }
                  }
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
        }
      }
    }

    internal override void UpdateFieldValue(string fieldId, string val)
    {
      if (fieldId == "230" || fieldId == "232")
      {
        double num = this.ToDouble(val);
        if (num != 0.0)
          val = num.ToString("N2");
      }
      base.UpdateFieldValue(fieldId, val);
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      switch (action)
      {
        case "setup":
          this.SetFieldFocus("l_661");
          break;
        case "mtginsreserv":
          if (this.inputData.IsLocked("232"))
          {
            this.SetFieldFocus("l_232");
            break;
          }
          this.SetFieldFocus("l_235");
          break;
      }
    }
  }
}
