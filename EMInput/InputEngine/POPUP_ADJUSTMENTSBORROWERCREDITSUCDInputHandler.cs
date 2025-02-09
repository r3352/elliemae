// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.POPUP_ADJUSTMENTSBORROWERCREDITSUCDInputHandler
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

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class POPUP_ADJUSTMENTSBORROWERCREDITSUCDInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel panelK;
    private CategoryBox boxK4;
    private CategoryBox boxK57;
    private CategoryBox boxK810;
    private CategoryBox boxK1115;
    private EllieMae.Encompass.Forms.Panel boxK4Detail;
    private EllieMae.Encompass.Forms.Panel boxK57Detail;
    private EllieMae.Encompass.Forms.Panel boxK810Detail;
    private EllieMae.Encompass.Forms.Panel boxK1115Detail;
    private EllieMae.Encompass.Forms.Panel panelL;
    private CategoryBox boxL4;
    private CategoryBox boxL67;
    private CategoryBox boxL811;
    private CategoryBox boxL1214;
    private CategoryBox boxL1517;
    private CategoryBox boxUcdTotal;
    private CategoryBox boxTotal;
    private EllieMae.Encompass.Forms.Panel boxL4Detail;
    private EllieMae.Encompass.Forms.Panel boxL67Detail;
    private EllieMae.Encompass.Forms.Panel boxL811Detail;
    private EllieMae.Encompass.Forms.Panel boxL1214Detail;
    private EllieMae.Encompass.Forms.Panel boxL1517Detail;
    private EllieMae.Encompass.Forms.Panel boxK4Expand;
    private EllieMae.Encompass.Forms.Panel boxK5Expand;
    private EllieMae.Encompass.Forms.Panel boxL15Expand;
    private Hyperlink HyperlinkK4;
    private Hyperlink HyperlinkK5;
    private Hyperlink HyperlinkL15;
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private List<EllieMae.Encompass.Forms.ContainerControl[]> containers;
    private ImageHandler imageHandler;
    private bool formLoading;

    public POPUP_ADJUSTMENTSBORROWERCREDITSUCDInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public POPUP_ADJUSTMENTSBORROWERCREDITSUCDInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public POPUP_ADJUSTMENTSBORROWERCREDITSUCDInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public POPUP_ADJUSTMENTSBORROWERCREDITSUCDInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public POPUP_ADJUSTMENTSBORROWERCREDITSUCDInputHandler(
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
      this.imageHandler = new ImageHandler(this.currentForm);
      this.panelK = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelK");
      this.boxK4 = (CategoryBox) this.currentForm.FindControl("boxK4");
      this.boxK57 = (CategoryBox) this.currentForm.FindControl("boxK57");
      this.boxK810 = (CategoryBox) this.currentForm.FindControl("boxK810");
      this.boxK1115 = (CategoryBox) this.currentForm.FindControl("boxK1115");
      this.boxK4Detail = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxK4Detail");
      this.boxK57Detail = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxK57Detail");
      this.boxK810Detail = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxK810Detail");
      this.boxK1115Detail = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxK1115Detail");
      this.panelL = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelL");
      this.boxL4 = (CategoryBox) this.currentForm.FindControl("boxL4");
      this.boxL67 = (CategoryBox) this.currentForm.FindControl("boxL67");
      this.boxL811 = (CategoryBox) this.currentForm.FindControl("boxL811");
      this.boxL1214 = (CategoryBox) this.currentForm.FindControl("boxL1214");
      this.boxL1517 = (CategoryBox) this.currentForm.FindControl("boxL1517");
      this.boxUcdTotal = (CategoryBox) this.currentForm.FindControl("boxUcdTotal");
      this.boxTotal = (CategoryBox) this.currentForm.FindControl("boxTotal");
      this.boxL4Detail = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxL4Detail");
      this.boxL67Detail = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxL67Detail");
      this.boxL811Detail = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxL811Detail");
      this.boxL1214Detail = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxL1214Detail");
      this.boxL1517Detail = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxL1517Detail");
      this.boxK4Expand = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxK4Expand");
      this.boxK5Expand = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxK5Expand");
      this.boxL15Expand = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxL15Expand");
      this.boxK4Expand.BackColor = this.boxK5Expand.BackColor = this.boxL15Expand.BackColor = Color.White;
      this.HyperlinkK4 = (Hyperlink) this.currentForm.FindControl("HyperlinkK4");
      this.HyperlinkK5 = (Hyperlink) this.currentForm.FindControl("HyperlinkK5");
      this.HyperlinkL15 = (Hyperlink) this.currentForm.FindControl("HyperlinkL15");
      this.HyperlinkK4.Click += new EventHandler(this.showMoreLines_Click);
      this.HyperlinkK5.Click += new EventHandler(this.showMoreLines_Click);
      this.HyperlinkL15.Click += new EventHandler(this.showMoreLines_Click);
      this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      this.labelFormName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelFormName");
      this.containers = new List<EllieMae.Encompass.Forms.ContainerControl[]>();
      this.containers.Add(new EllieMae.Encompass.Forms.ContainerControl[3]
      {
        (EllieMae.Encompass.Forms.ContainerControl) this.boxK4,
        (EllieMae.Encompass.Forms.ContainerControl) this.boxK4Detail,
        (EllieMae.Encompass.Forms.ContainerControl) this.boxK4Expand
      });
      this.containers.Add(new EllieMae.Encompass.Forms.ContainerControl[3]
      {
        (EllieMae.Encompass.Forms.ContainerControl) this.boxK57,
        (EllieMae.Encompass.Forms.ContainerControl) this.boxK57Detail,
        (EllieMae.Encompass.Forms.ContainerControl) this.boxK5Expand
      });
      this.containers.Add(new EllieMae.Encompass.Forms.ContainerControl[3]
      {
        (EllieMae.Encompass.Forms.ContainerControl) this.boxK810,
        (EllieMae.Encompass.Forms.ContainerControl) this.boxK810Detail,
        null
      });
      this.containers.Add(new EllieMae.Encompass.Forms.ContainerControl[3]
      {
        (EllieMae.Encompass.Forms.ContainerControl) this.boxK1115,
        (EllieMae.Encompass.Forms.ContainerControl) this.boxK1115Detail,
        null
      });
      this.containers.Add(new EllieMae.Encompass.Forms.ContainerControl[3]
      {
        (EllieMae.Encompass.Forms.ContainerControl) this.panelL,
        null,
        null
      });
      this.containers.Add(new EllieMae.Encompass.Forms.ContainerControl[3]
      {
        (EllieMae.Encompass.Forms.ContainerControl) this.boxL4,
        (EllieMae.Encompass.Forms.ContainerControl) this.boxL4Detail,
        null
      });
      this.containers.Add(new EllieMae.Encompass.Forms.ContainerControl[3]
      {
        (EllieMae.Encompass.Forms.ContainerControl) this.boxL67,
        (EllieMae.Encompass.Forms.ContainerControl) this.boxL67Detail,
        null
      });
      this.containers.Add(new EllieMae.Encompass.Forms.ContainerControl[3]
      {
        (EllieMae.Encompass.Forms.ContainerControl) this.boxL811,
        (EllieMae.Encompass.Forms.ContainerControl) this.boxL811Detail,
        null
      });
      this.containers.Add(new EllieMae.Encompass.Forms.ContainerControl[3]
      {
        (EllieMae.Encompass.Forms.ContainerControl) this.boxL1214,
        (EllieMae.Encompass.Forms.ContainerControl) this.boxL1214Detail,
        null
      });
      this.containers.Add(new EllieMae.Encompass.Forms.ContainerControl[3]
      {
        (EllieMae.Encompass.Forms.ContainerControl) this.boxL1517,
        (EllieMae.Encompass.Forms.ContainerControl) this.boxL1517Detail,
        (EllieMae.Encompass.Forms.ContainerControl) this.boxL15Expand
      });
      this.containers.Add(new EllieMae.Encompass.Forms.ContainerControl[3]
      {
        (EllieMae.Encompass.Forms.ContainerControl) this.boxUcdTotal,
        null,
        null
      });
      this.containers.Add(new EllieMae.Encompass.Forms.ContainerControl[3]
      {
        (EllieMae.Encompass.Forms.ContainerControl) this.boxTotal,
        null,
        null
      });
      this.formLoading = true;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (this.inputData is DisclosedLEHandler || this.inputData is DisclosedCDHandler)
        return ControlState.Disabled;
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "L88":
          if (this.GetField("CD3.X225") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "CD3.X2":
          if (this.GetField("CD3.X229") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "CD3.X9":
          if (this.GetField("CD3.X288") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "CD3.X13":
          if (this.GetField("CD3.X312") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "CD3.X15":
          if (this.GetField("CD3.X316") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "CD3.X17":
          if (this.GetField("CD3.X320") == "Y")
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

    public override void onmouseout(mshtml.IHTMLEventObj pEvtObj)
    {
      this.imageHandler.OnMouseOut(pEvtObj);
      base.onmouseout(pEvtObj);
    }

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      this.imageHandler.OnMouseEnter(pEvtObj);
      base.onmouseenter(pEvtObj);
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      bool flag = base.onclick(pEvtObj);
      this.imageHandler.OnMouseClick(pEvtObj);
      return flag;
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.refreshIcons();
    }

    protected override void UpdateContents(bool refreshAllFields)
    {
      base.UpdateContents(refreshAllFields);
      this.refreshIcons();
    }

    private void refreshIcons()
    {
      if (!(this.inputData is DisclosedCDHandler))
        return;
      for (int index = 1; index <= 9; ++index)
        this.SetControlState("ImageButton" + (object) index, true);
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (!this.dateRangeValidation(id, val, 92, 93, 0) || !this.dateRangeValidation(id, val, 98, 99, 0) || !this.dateRangeValidation(id, val, 104, 105, 0) || !this.dateRangeValidation(id, val, 156, 157, 0) || !this.dateRangeValidation(id, val, 162, 163, 0) || !this.dateRangeValidation(id, val, 168, 169, 0) || !this.dateRangeValidation(id, val, 264, 276, 3) || !this.dateRangeValidation(id, val, 347, 350, 3) || !this.dateRangeValidation(id, val, 355, 440, 5))
        return;
      base.UpdateFieldValue(id, val);
    }

    private bool dateRangeValidation(
      string id,
      string val,
      int fieldStart,
      int fieldEnd,
      int interval)
    {
      if (val == "" || val == "//")
        return true;
      int num1 = -1;
      int num2 = -1;
      if (interval == 0)
      {
        if ("L" + (object) fieldStart == id || "L" + (object) fieldEnd == id)
        {
          num1 = fieldStart;
          num2 = fieldEnd;
        }
      }
      else
      {
        for (int index = fieldStart; index <= fieldEnd; index += interval)
        {
          if ("CD3.X" + (object) index == id || "CD3.X" + (object) (index + 1) == id)
          {
            num1 = index;
            num2 = index + 1;
            break;
          }
        }
      }
      if (num1 == -1 || num2 == -1)
        return true;
      DateTime date1;
      DateTime date2;
      if (interval == 0 && "L" + (object) num1 == id || interval > 0 && "CD3.X" + (object) num1 == id)
      {
        date1 = Utils.ParseDate((object) val);
        date2 = Utils.ParseDate((object) this.inputData.GetField((interval == 0 ? (object) "L" : (object) "CD3.X").ToString() + (object) num2));
      }
      else
      {
        date1 = Utils.ParseDate((object) this.inputData.GetField((interval == 0 ? (object) "L" : (object) "CD3.X").ToString() + (object) num1));
        date2 = Utils.ParseDate((object) val);
      }
      if (date1 == DateTime.MinValue || date2 == DateTime.MinValue || !(date1.Date > date2.Date))
        return true;
      int num3 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The date range between Date From and Date Through is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.formLoading)
      {
        this.boxL4Detail.Visible = this.boxL67Detail.Visible = this.boxL811Detail.Visible = this.boxL1214Detail.Visible = this.boxL1517Detail.Visible = this.boxK4Detail.Visible = this.boxK57Detail.Visible = this.boxK810Detail.Visible = this.boxK1115Detail.Visible = true;
        this.boxK4Expand.Visible = this.boxK5Expand.Visible = this.boxL15Expand.Visible = false;
      }
      this.rearrangeLayout((string) null);
      this.formLoading = false;
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (!action.StartsWith("switch"))
        return;
      this.rearrangeLayout(action);
    }

    private void showMoreLines_Click(object sender, EventArgs e)
    {
      Hyperlink hyperlink = (Hyperlink) sender;
      if (hyperlink.Text == "Show More Lines")
      {
        if (hyperlink.ControlID == "HyperlinkK4")
          this.rearrangeLayout("switchK4DetailOn");
        else if (hyperlink.ControlID == "HyperlinkK5")
          this.rearrangeLayout("switchK5DetailOn");
        else if (hyperlink.ControlID == "HyperlinkL15")
          this.rearrangeLayout("switchL15DetailOn");
        hyperlink.Text = "Show Less Lines";
      }
      else
      {
        if (!(hyperlink.Text == "Show Less Lines"))
          return;
        if (hyperlink.ControlID == "HyperlinkK4")
          this.rearrangeLayout("switchK4DetailOff");
        else if (hyperlink.ControlID == "HyperlinkK5")
          this.rearrangeLayout("switchK5DetailOff");
        else if (hyperlink.ControlID == "HyperlinkL15")
          this.rearrangeLayout("switchL15DetailOff");
        hyperlink.Text = "Show More Lines";
      }
    }

    private void rearrangeLayout(string actionID)
    {
      int num1 = 26;
      int num2 = 20;
      int num3 = this.panelK.Top + this.panelK.Size.Height;
      switch (actionID)
      {
        case "switchK11":
          this.boxK1115Detail.Visible = !this.boxK1115Detail.Visible;
          break;
        case "switchK4":
          this.boxK4Detail.Visible = !this.boxK4Detail.Visible;
          break;
        case "switchK4DetailOff":
          this.boxK4Expand.Visible = false;
          break;
        case "switchK4DetailOn":
          this.boxK4Expand.Visible = true;
          break;
        case "switchK5":
          this.boxK57Detail.Visible = !this.boxK57Detail.Visible;
          break;
        case "switchK5DetailOff":
          this.boxK5Expand.Visible = false;
          break;
        case "switchK5DetailOn":
          this.boxK5Expand.Visible = true;
          break;
        case "switchK8":
          this.boxK810Detail.Visible = !this.boxK810Detail.Visible;
          break;
        case "switchL12":
          this.boxL1214Detail.Visible = !this.boxL1214Detail.Visible;
          break;
        case "switchL15":
          this.boxL1517Detail.Visible = !this.boxL1517Detail.Visible;
          break;
        case "switchL15DetailOff":
          this.boxL15Expand.Visible = !this.boxL15Expand.Visible;
          break;
        case "switchL15DetailOn":
          this.boxL15Expand.Visible = !this.boxL15Expand.Visible;
          break;
        case "switchL4":
          this.boxL4Detail.Visible = !this.boxL4Detail.Visible;
          break;
        case "switchL6":
          this.boxL67Detail.Visible = !this.boxL67Detail.Visible;
          break;
        case "switchL8":
          this.boxL811Detail.Visible = !this.boxL811Detail.Visible;
          break;
      }
      Size size1;
      for (int index = 0; index < this.containers.Count; ++index)
      {
        if (this.containers[index][0].ControlID != "panelL" && this.containers[index][1] != null)
        {
          if (this.containers[index][2] != null && this.containers[index][1].Visible)
          {
            if (this.containers[index][2].Visible)
            {
              EllieMae.Encompass.Forms.ContainerControl containerControl = this.containers[index][1];
              size1 = this.containers[index][1].Size;
              int width = size1.Width;
              int top = this.containers[index][2].Top;
              size1 = this.containers[index][2].Size;
              int height1 = size1.Height;
              int height2 = top + height1 + num2;
              Size size2 = new Size(width, height2);
              containerControl.Size = size2;
            }
            else
            {
              EllieMae.Encompass.Forms.ContainerControl containerControl = this.containers[index][1];
              size1 = this.containers[index][1].Size;
              Size size3 = new Size(size1.Width, this.containers[index][2].Top + num2);
              containerControl.Size = size3;
            }
            if (this.containers[index][0].ControlID == "boxK4")
            {
              Hyperlink hyperlinkK4 = this.HyperlinkK4;
              size1 = this.containers[index][1].Size;
              int num4 = size1.Height - num2;
              hyperlinkK4.Top = num4;
            }
            else if (this.containers[index][0].ControlID == "boxK57")
            {
              Hyperlink hyperlinkK5 = this.HyperlinkK5;
              size1 = this.containers[index][1].Size;
              int num5 = size1.Height - num2;
              hyperlinkK5.Top = num5;
            }
            else if (this.containers[index][0].ControlID == "boxL1517")
            {
              Hyperlink hyperlinkL15 = this.HyperlinkL15;
              size1 = this.containers[index][1].Size;
              int num6 = size1.Height - num2;
              hyperlinkL15.Top = num6;
            }
          }
          EllieMae.Encompass.Forms.ContainerControl containerControl1 = this.containers[index][0];
          size1 = this.containers[index][0].Size;
          int width1 = size1.Width;
          int height;
          if (!this.containers[index][1].Visible)
          {
            height = num1;
          }
          else
          {
            size1 = this.containers[index][1].Size;
            height = size1.Height + num1;
          }
          Size size4 = new Size(width1, height);
          containerControl1.Size = size4;
        }
        else if (this.containers[index][0].ControlID == "panelL" || this.containers[index][1] == null)
          ++num3;
        this.containers[index][0].Top = num3;
        if (this.containers[index][0].ControlID != "panelL" || this.containers[index][1] == null)
        {
          int num7 = num3;
          size1 = this.containers[index][0].Size;
          int height = size1.Height;
          num3 = num7 + height;
        }
        else
        {
          int num8 = num3;
          int num9;
          if (!this.containers[index][1].Visible)
          {
            num9 = num1;
          }
          else
          {
            size1 = this.containers[index][1].Size;
            num9 = size1.Height + num1;
          }
          num3 = num8 + num9;
        }
      }
      EllieMae.Encompass.Forms.Panel pnlForm = this.pnlForm;
      size1 = this.pnlForm.Size;
      Size size5 = new Size(size1.Width, num3 + 10);
      pnlForm.Size = size5;
      EllieMae.Encompass.Forms.Label labelFormName = this.labelFormName;
      size1 = this.pnlForm.Size;
      int num10 = size1.Height + 10;
      labelFormName.Top = num10;
    }
  }
}
