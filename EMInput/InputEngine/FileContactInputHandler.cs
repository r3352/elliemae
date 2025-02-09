// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FileContactInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FileContactInputHandler : InputHandlerBase
  {
    private IWin32Window owner;

    public FileContactInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public FileContactInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public FileContactInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public FileContactInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
      this.owner = owner;
    }

    public event EventHandler ContactFieldsChanged;

    public void ContactChanged(RxContactInfo info)
    {
      if (this.ContactFieldsChanged != null)
        this.ContactFieldsChanged((object) info, (EventArgs) null);
      if (this.GetField("VEND.X993") == "Y")
        this.loan.Calculator.FormCalculation("VEND.X993", "VEND.X993", "Y");
      if (!(this.GetField("VEND.X994") == "Y"))
        return;
      this.loan.Calculator.FormCalculation("VEND.X994", "VEND.X994", "Y");
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (this.loan == null || this.loan.Calculator == null)
        return;
      if ((id == "VEND.X133" || id == "VEND.X134" || id == "VEND.X135" || id == "VEND.X136" || id == "VEND.X137" || id == "VEND.X936" || id == "VEND.X139" || id == "VEND.X739" || id == "VEND.X141" || id == "VEND.X140") && this.GetField("VEND.X993") == "Y")
        this.loan.Calculator.FormCalculation("VEND.X993", "VEND.X993", "Y");
      else if ((id == "VEND.X144" || id == "VEND.X145" || id == "VEND.X146" || id == "VEND.X147" || id == "VEND.X148" || id == "VEND.X937" || id == "VEND.X150" || id == "VEND.X928" || id == "VEND.X152" || id == "VEND.X151") && this.GetField("VEND.X994") == "Y")
      {
        this.loan.Calculator.FormCalculation("VEND.X994", "VEND.X994", "Y");
      }
      else
      {
        if (!(id == "VEND.X655") && !(id == "VEND.X656") && !(id == "VEND.X657") && !(id == "VEND.X658") && !(id == "VEND.X659") && !(id == "VEND.X662") && !(id == "VEND.X675") && !(id == "VEND.X663") && !(id == "VEND.X668") && !(id == "VEND.X676") && !(id == "VEND.X670") && !(id == "VEND.X669") || !(this.GetField("VEND.X654") == "Y"))
          return;
        this.loan.Calculator.FormCalculation("VEND.X654", "VEND.X654", "Y");
      }
    }
  }
}
