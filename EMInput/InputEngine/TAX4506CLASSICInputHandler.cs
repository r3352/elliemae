// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TAX4506CLASSICInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TAX4506CLASSICInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Label label1aFName;
    private EllieMae.Encompass.Forms.Label label1aLName;
    private EllieMae.Encompass.Forms.TextBox txt1aLName;
    private EllieMae.Encompass.Forms.Label label2aFName;
    private EllieMae.Encompass.Forms.Label label2aLName;
    private EllieMae.Encompass.Forms.TextBox txt2aLName;

    public TAX4506CLASSICInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public TAX4506CLASSICInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public TAX4506CLASSICInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public TAX4506CLASSICInputHandler(
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
        if (this.label1aFName == null)
          this.label1aFName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1aFName");
        if (this.label1aLName == null)
          this.label1aLName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1aLName");
        if (this.txt1aLName == null)
          this.txt1aLName = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt1aLName");
        if (this.label2aFName == null)
          this.label2aFName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label2aFName");
        if (this.label2aLName == null)
          this.label2aLName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label2aLName");
        if (this.txt2aLName != null)
          return;
        this.txt2aLName = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt2aLName");
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (id == "IRS4506.X1")
      {
        if (this.GetFieldValue(id) == "Other")
        {
          this.label1aFName.Text = "Name";
          this.label1aLName.Visible = false;
          this.txt1aLName.Visible = false;
          this.label2aFName.Text = "Name";
          this.label2aLName.Visible = false;
          this.txt2aLName.Visible = false;
        }
        else
        {
          this.label1aFName.Text = "First Name";
          this.label1aLName.Visible = true;
          this.txt1aLName.Visible = true;
          this.label2aFName.Text = "First Name";
          this.label2aLName.Visible = true;
          this.txt2aLName.Visible = true;
        }
      }
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (id == "IRS4506.X57" && this.GetFieldValue("IRS4506.X1") == "Other" || id == "IRS4506.X58" && this.GetFieldValue("IRS4506.X1") == "Other")
        controlState = ControlState.Disabled;
      else if (id == "IRS4506.X32")
        controlState = ControlState.Disabled;
      else if (id == "IRS4506.X63" && this.GetFieldValue("IRS4506.X1") != "Other")
        controlState = ControlState.Disabled;
      else if (id == "IRS4506.X64" && this.GetFieldValue("IRS4506.X1") != "Other")
        controlState = ControlState.Disabled;
      return controlState;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if ((id == "IRS4506.X25" || id == "IRS4506.X26" || id == "IRS4506.X29" || id == "IRS4506.X30" || id == "IRS4506.X53" || id == "IRS4506.X54" || id == "IRS4506.X55" || id == "IRS4506.X56") && val != string.Empty)
      {
        if (val != null)
        {
          try
          {
            val = Convert.ToDateTime(val).ToString("MM/dd/yyyy");
          }
          catch
          {
          }
        }
      }
      base.UpdateFieldValue(id, val);
      if (!(id == "IRS4506.X1"))
        return;
      if (val == "Other")
      {
        base.UpdateFieldValue("IRS4506.X2", (this.GetField("IRS4506.X2") + " " + this.GetField("IRS4506.X3")).Trim());
        base.UpdateFieldValue("IRS4506.X6", (this.GetField("IRS4506.X6") + " " + this.GetField("IRS4506.X7")).Trim());
        base.UpdateFieldValue("IRS4506.X3", "");
        base.UpdateFieldValue("IRS4506.X7", "");
        base.UpdateFieldValue("IRS4506.X57", "Y");
        base.UpdateFieldValue("IRS4506.X58", "Y");
      }
      else
      {
        if (!(val != string.Empty))
          return;
        string field1 = this.GetField("IRS4506.X2");
        if (this.GetField("IRS4506.X3") == string.Empty && field1.LastIndexOf(" ") > -1)
        {
          base.UpdateFieldValue("IRS4506.X2", field1.Substring(0, field1.LastIndexOf(" ")));
          base.UpdateFieldValue("IRS4506.X3", field1.Substring(field1.LastIndexOf(" ") + 1));
        }
        string field2 = this.GetField("IRS4506.X6");
        if (!(this.GetField("IRS4506.X7") == string.Empty) || field2.LastIndexOf(" ") <= -1)
          return;
        base.UpdateFieldValue("IRS4506.X6", field2.Substring(0, field2.LastIndexOf(" ")));
        base.UpdateFieldValue("IRS4506.X7", field2.Substring(field2.LastIndexOf(" ") + 1));
      }
    }

    public override void ExecAction(string action)
    {
      if (!(action == "copybroInfo"))
        return;
      string fieldValue = this.GetFieldValue("IRS4506.X1");
      if (fieldValue == "Both" || fieldValue == "Borrower")
      {
        if (this.GetFieldValue("IRS4506.X93") == "4506-COct2022")
        {
          this.UpdateFieldValue("IRS4506.X2", this.GetFieldValue("4000"));
          if (!string.IsNullOrEmpty(this.GetFieldValue("4001")))
            this.UpdateFieldValue("IRS4506.X68", this.GetFieldValue("4001").Substring(0, 1));
          else
            this.UpdateFieldValue("IRS4506.X68", "");
          this.UpdateFieldValue("IRS4506.X3", this.GetFieldValue("4002"));
        }
        else
        {
          this.UpdateFieldValue("IRS4506.X2", this.GetFieldValue("36"));
          this.UpdateFieldValue("IRS4506.X3", this.GetFieldValue("37"));
        }
        this.UpdateFieldValue("IRS4506.X57", "");
        this.UpdateFieldValue("IRS4506.X4", this.GetFieldValue("65"));
        this.UpdateFieldValue("IRS4506.X39", this.GetFieldValue("36"));
        this.UpdateFieldValue("IRS4506.X40", this.GetFieldValue("37"));
        this.UpdateFieldValue("IRS4506.X35", this.GetFieldValue("FR0104"));
        this.UpdateFieldValue("IRS4506.X36", this.GetFieldValue("FR0106"));
        this.UpdateFieldValue("IRS4506.X37", this.GetFieldValue("FR0107"));
        this.UpdateFieldValue("IRS4506.X38", this.GetFieldValue("FR0108"));
        this.UpdateFieldValue("IRS4506.X27", this.GetFieldValue("66"));
      }
      if (fieldValue == "CoBorrower")
      {
        if (this.GetFieldValue("IRS4506.X93") == "4506-COct2022")
        {
          this.UpdateFieldValue("IRS4506.X2", this.GetFieldValue("4004"));
          if (!string.IsNullOrEmpty(this.GetFieldValue("4005")))
            this.UpdateFieldValue("IRS4506.X68", this.GetFieldValue("4005").Substring(0, 1));
          else
            this.UpdateFieldValue("IRS4506.X68", "");
          this.UpdateFieldValue("IRS4506.X3", this.GetFieldValue("4006"));
        }
        else
        {
          this.UpdateFieldValue("IRS4506.X2", this.GetFieldValue("68"));
          this.UpdateFieldValue("IRS4506.X3", this.GetFieldValue("69"));
        }
        this.UpdateFieldValue("IRS4506.X57", "");
        this.UpdateFieldValue("IRS4506.X4", this.GetFieldValue("97"));
        this.UpdateFieldValue("IRS4506.X39", this.GetFieldValue("68"));
        this.UpdateFieldValue("IRS4506.X40", this.GetFieldValue("69"));
        this.UpdateFieldValue("IRS4506.X35", this.GetFieldValue("FR0204"));
        this.UpdateFieldValue("IRS4506.X36", this.GetFieldValue("FR0206"));
        this.UpdateFieldValue("IRS4506.X37", this.GetFieldValue("FR0207"));
        this.UpdateFieldValue("IRS4506.X38", this.GetFieldValue("FR0208"));
        this.UpdateFieldValue("IRS4506.X27", this.GetFieldValue("98"));
      }
      if (fieldValue == "Both")
      {
        if (this.GetFieldValue("IRS4506.X93") == "4506-COct2022")
        {
          this.UpdateFieldValue("IRS4506.X6", this.GetFieldValue("4004"));
          if (!string.IsNullOrEmpty(this.GetFieldValue("4005")))
            this.UpdateFieldValue("IRS4506.X69", this.GetFieldValue("4005").Substring(0, 1));
          else
            this.UpdateFieldValue("IRS4506.X69", "");
          this.UpdateFieldValue("IRS4506.X7", this.GetFieldValue("4006"));
        }
        else
        {
          this.UpdateFieldValue("IRS4506.X6", this.GetFieldValue("68"));
          this.UpdateFieldValue("IRS4506.X7", this.GetFieldValue("69"));
        }
        this.UpdateFieldValue("IRS4506.X58", "");
        this.UpdateFieldValue("IRS4506.X5", this.GetFieldValue("97"));
      }
      else
      {
        this.UpdateFieldValue("IRS4506.X6", "");
        this.UpdateFieldValue("IRS4506.X7", "");
        this.UpdateFieldValue("IRS4506.X58", "");
        this.UpdateFieldValue("IRS4506.X5", "");
        this.UpdateFieldValue("IRS4506.X69", "");
      }
      this.UpdateContents();
      this.SetFieldFocus("l_IRS4506X2");
    }
  }
}
