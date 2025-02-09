// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TAX4506TCLASSICInputHandler
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
  public class TAX4506TCLASSICInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Label label1aFName;
    private EllieMae.Encompass.Forms.Label label1aLName;
    private EllieMae.Encompass.Forms.TextBox txt1aLName;
    private EllieMae.Encompass.Forms.Label label2aFName;
    private EllieMae.Encompass.Forms.Label label2aLName;
    private EllieMae.Encompass.Forms.TextBox txt2aLName;
    private EllieMae.Encompass.Forms.Panel oldPanel;
    private EllieMae.Encompass.Forms.Panel pnlFrm;
    private EllieMae.Encompass.Forms.Panel pnlNew;
    private EllieMae.Encompass.Forms.Panel pnlNewLocation;
    private EllieMae.Encompass.Forms.Label formLabel;
    private EllieMae.Encompass.Forms.Label label1aFNameNew;
    private EllieMae.Encompass.Forms.Label label1aLNameNew;
    private EllieMae.Encompass.Forms.TextBox txt1aLNameNew;
    private EllieMae.Encompass.Forms.Label label1aMNameNew;
    private EllieMae.Encompass.Forms.TextBox txt1aMNameNew;
    private EllieMae.Encompass.Forms.Label label2aFNameNew;
    private EllieMae.Encompass.Forms.Label label2aLNameNew;
    private EllieMae.Encompass.Forms.TextBox txt2aLNameNew;
    private EllieMae.Encompass.Forms.Label label2aMNameNew;
    private EllieMae.Encompass.Forms.TextBox txt2aMNameNew;
    private EllieMae.Encompass.Forms.Label label1cFNameNew;
    private EllieMae.Encompass.Forms.Label label1cLNameNew;
    private EllieMae.Encompass.Forms.TextBox txt1cLNameNew;
    private EllieMae.Encompass.Forms.Label label1cMNameNew;
    private EllieMae.Encompass.Forms.TextBox txt1cMNameNew;
    private EllieMae.Encompass.Forms.Label label2cFNameNew;
    private EllieMae.Encompass.Forms.Label label2cLNameNew;
    private EllieMae.Encompass.Forms.TextBox txt2cLNameNew;
    private EllieMae.Encompass.Forms.Label label2cMNameNew;
    private EllieMae.Encompass.Forms.TextBox txt2cMNameNew;
    private EllieMae.Encompass.Forms.Panel pnlLastSection;

    public TAX4506TCLASSICInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public TAX4506TCLASSICInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public TAX4506TCLASSICInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public TAX4506TCLASSICInputHandler(
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
        if (this.label1aFNameNew == null)
          this.label1aFNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1aFNameNew");
        if (this.label1aLNameNew == null)
          this.label1aLNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1aLNameNew");
        if (this.txt1aLNameNew == null)
          this.txt1aLNameNew = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt1aLNameNew");
        if (this.label1aMNameNew == null)
          this.label1aMNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1aMNameNew");
        if (this.txt1aMNameNew == null)
          this.txt1aMNameNew = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt1aMNameNew");
        if (this.label2aFName == null)
          this.label2aFName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label2aFName");
        if (this.label2aLName == null)
          this.label2aLName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label2aLName");
        if (this.txt2aLName == null)
          this.txt2aLName = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt2aLName");
        if (this.label2aFNameNew == null)
          this.label2aFNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label2aFNameNew");
        if (this.label2aLNameNew == null)
          this.label2aLNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label2aLNameNew");
        if (this.txt2aLNameNew == null)
          this.txt2aLNameNew = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt2aLNameNew");
        if (this.label2aMNameNew == null)
          this.label2aMNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label2aMNameNew");
        if (this.txt2aMNameNew == null)
          this.txt2aMNameNew = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt2aMNameNew");
        if (this.label1aFNameNew == null)
          this.label1aFNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1aFNameNew");
        if (this.label1aLNameNew == null)
          this.label1aLNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1aLNameNew");
        if (this.txt1aLNameNew == null)
          this.txt1aLNameNew = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt1aLNameNew");
        if (this.label1aMNameNew == null)
          this.label1aMNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1aMNameNew");
        if (this.label1cFNameNew == null)
          this.label1cFNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1cFNameNew");
        if (this.label1cLNameNew == null)
          this.label1cLNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1cLNameNew");
        if (this.txt1cLNameNew == null)
          this.txt1cLNameNew = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt1cLNameNew");
        if (this.label1cMNameNew == null)
          this.label1cMNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1cMNameNew");
        if (this.txt1cMNameNew == null)
          this.txt1cMNameNew = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt1cMNameNew");
        if (this.label2cFNameNew == null)
          this.label2cFNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label2cFNameNew");
        if (this.label2cLNameNew == null)
          this.label2cLNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label2cLNameNew");
        if (this.txt2cLNameNew == null)
          this.txt2cLNameNew = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt2cLNameNew");
        if (this.label2cMNameNew == null)
          this.label2cMNameNew = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label2cMNameNew");
        if (this.txt2cMNameNew == null)
          this.txt2cMNameNew = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt2cMNameNew");
        if (this.txt1aMNameNew == null)
          this.txt1aMNameNew = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt1aMNameNew");
        if (this.oldPanel == null)
          this.oldPanel = this.currentForm.FindControl("PanelOldLayout") as EllieMae.Encompass.Forms.Panel;
        if (this.pnlNew == null)
          this.pnlNew = this.currentForm.FindControl("PanelNewLayout") as EllieMae.Encompass.Forms.Panel;
        if (this.pnlNewLocation == null)
          this.pnlNewLocation = this.currentForm.FindControl("PanelSecion1a") as EllieMae.Encompass.Forms.Panel;
        if (this.pnlLastSection == null)
          this.pnlLastSection = this.currentForm.FindControl("PanelLastSection") as EllieMae.Encompass.Forms.Panel;
        if (this.pnlFrm == null)
          this.pnlFrm = this.currentForm.FindControl("pnlForm") as EllieMae.Encompass.Forms.Panel;
        if (this.formLabel != null)
          return;
        this.formLabel = this.currentForm.FindControl("Label59") as EllieMae.Encompass.Forms.Label;
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (id == "IRS4506.X57" && this.GetFieldValue("IRS4506.X1") == "Other" || id == "IRS4506.X58" && this.GetFieldValue("IRS4506.X1") == "Other")
        controlState = ControlState.Disabled;
      else if (id == "IRS4506.X63" && this.GetFieldValue("IRS4506.X1") != "Other")
        controlState = ControlState.Disabled;
      else if (id == "IRS4506.X64" && this.GetFieldValue("IRS4506.X1") != "Other")
        controlState = ControlState.Disabled;
      return controlState;
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.GetFieldValue("IRS4506.X1") == "Other")
      {
        this.label1aFName.Text = "Name";
        this.label1aLName.Visible = false;
        this.txt1aLName.Visible = false;
        this.label2aFName.Text = "Name";
        this.label2aLName.Visible = false;
        this.txt2aLName.Visible = false;
        if (this.GetFieldValue("IRS4506.X93") == "4506-COct2022")
        {
          this.label1aFNameNew.Text = "Name";
          this.label1aLNameNew.Visible = false;
          this.txt1aLNameNew.Visible = false;
          this.label1aMNameNew.Visible = false;
          this.txt1aMNameNew.Visible = false;
          this.label2aFNameNew.Text = "Name";
          this.label2aLNameNew.Visible = false;
          this.txt2aLNameNew.Visible = false;
          this.label2aMNameNew.Visible = false;
          this.txt2aMNameNew.Visible = false;
          this.label2cFNameNew.Text = "Name";
          this.label2cLNameNew.Visible = false;
          this.txt2cLNameNew.Visible = false;
          this.label2cMNameNew.Visible = false;
          this.txt2cMNameNew.Visible = false;
          this.label1cFNameNew.Text = "Name";
          this.label1cLNameNew.Visible = false;
          this.txt1cLNameNew.Visible = false;
          this.label1cMNameNew.Visible = false;
          this.txt1cMNameNew.Visible = false;
        }
      }
      else
      {
        this.label1aFName.Text = "First Name";
        this.label1aLName.Visible = true;
        this.txt1aLName.Visible = true;
        this.label2aFName.Text = "First Name";
        this.label2aLName.Visible = true;
        this.txt2aLName.Visible = true;
        this.label1aFNameNew.Text = "First Name";
        this.label1aLNameNew.Visible = true;
        this.txt1aLNameNew.Visible = true;
        this.label2aFNameNew.Text = "First Name";
        this.label2aLNameNew.Visible = true;
        this.txt2aLNameNew.Visible = true;
        this.label1aMNameNew.Visible = true;
        this.txt1aMNameNew.Visible = true;
        this.label2aMNameNew.Visible = true;
        this.txt2aMNameNew.Visible = true;
        this.label2cFNameNew.Text = "First Name";
        this.label2cLNameNew.Visible = true;
        this.txt2cLNameNew.Visible = true;
        this.label2cMNameNew.Visible = true;
        this.txt2cMNameNew.Visible = true;
        this.label1cFNameNew.Text = "First Name";
        this.label1cLNameNew.Visible = true;
        this.txt1cLNameNew.Visible = true;
        this.label1cMNameNew.Visible = true;
        this.txt1cMNameNew.Visible = true;
      }
      if (this.GetFieldValue("IRS4506.X93") == "4506-COct2022")
      {
        this.oldPanel.Visible = false;
        this.pnlNew.Visible = true;
        this.pnlNew.Left = this.pnlNewLocation.Left;
        this.pnlNew.Top = this.pnlNewLocation.Top + 50;
        this.pnlLastSection.Top = this.pnlNew.Top + this.pnlNew.Size.Height;
        this.pnlFrm.Size = new Size(this.pnlFrm.Size.Width, this.pnlLastSection.Top + this.pnlLastSection.Size.Height);
        this.formLabel.Top = this.pnlFrm.Size.Height + 15;
      }
      else
      {
        this.oldPanel.Visible = true;
        this.pnlNew.Visible = false;
        this.pnlLastSection.Top = this.oldPanel.Top + this.oldPanel.Size.Height + 60;
        this.pnlFrm.Size = new Size(this.pnlFrm.Size.Width, this.pnlLastSection.Top + this.pnlLastSection.Size.Height);
        this.formLabel.Top = this.pnlFrm.Size.Height + 15;
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if ((id == "IRS4506.X25" || id == "IRS4506.X26" || id == "IRS4506.X29" || id == "IRS4506.X30") && val != string.Empty)
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
