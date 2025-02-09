// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.POPUP_ADJUSTMENTSSELLERCREDITSUCDInputHandler
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
  public class POPUP_ADJUSTMENTSSELLERCREDITSUCDInputHandler : InputHandlerBase
  {
    private List<object> allContainers;
    private EllieMae.Encompass.Forms.Panel panelM;
    private EllieMae.Encompass.Forms.Panel panelN;
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private ImageHandler imageHandler;
    private bool formLoading;

    public POPUP_ADJUSTMENTSSELLERCREDITSUCDInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public POPUP_ADJUSTMENTSSELLERCREDITSUCDInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public POPUP_ADJUSTMENTSSELLERCREDITSUCDInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public POPUP_ADJUSTMENTSSELLERCREDITSUCDInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public POPUP_ADJUSTMENTSSELLERCREDITSUCDInputHandler(
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
      this.panelM = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelM");
      this.panelN = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN");
      this.allContainers = new List<object>();
      this.allContainers.Add((object) (CategoryBox) this.currentForm.FindControl("boxM3"));
      this.allContainers.Add((object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxM3All"));
      this.allContainers.Add((object) new object[5]
      {
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelM8"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelM8Fix"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelM8Dynamic"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelM8L"),
        (object) (Hyperlink) this.currentForm.FindControl("HyperlinkM8")
      });
      this.allContainers.Add((object) (CategoryBox) this.currentForm.FindControl("boxM9"));
      this.allContainers.Add((object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxM9All"));
      this.allContainers.Add((object) (CategoryBox) this.currentForm.FindControl("boxM12"));
      this.allContainers.Add((object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxM12All"));
      this.allContainers.Add((object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelM12Fix"));
      this.allContainers.Add((object) new object[5]
      {
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelM16"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelM16Fix"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelM16Dynamic"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelM16L"),
        (object) (Hyperlink) this.currentForm.FindControl("HyperlinkM16")
      });
      this.allContainers.Add((object) (CategoryBox) this.currentForm.FindControl("boxN6"));
      this.allContainers.Add((object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxN6All"));
      this.allContainers.Add((object) new object[5]
      {
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN7"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN7Fix"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN7Dynamic"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN7L"),
        (object) (Hyperlink) this.currentForm.FindControl("HyperlinkN7")
      });
      this.allContainers.Add((object) (CategoryBox) this.currentForm.FindControl("boxN9"));
      this.allContainers.Add((object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxN9All"));
      this.allContainers.Add((object) new object[5]
      {
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN13"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN13Fix"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN13Dynamic"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN13L"),
        (object) (Hyperlink) this.currentForm.FindControl("HyperlinkN13")
      });
      this.allContainers.Add((object) (CategoryBox) this.currentForm.FindControl("boxN14"));
      this.allContainers.Add((object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxN14All"));
      this.allContainers.Add((object) (CategoryBox) this.currentForm.FindControl("boxN17"));
      this.allContainers.Add((object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("boxN17All"));
      this.allContainers.Add((object) new object[5]
      {
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN19"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN19Fix"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN19Dynamic"),
        (object) (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelN19L"),
        (object) (Hyperlink) this.currentForm.FindControl("HyperlinkN19")
      });
      for (int index = 0; index < this.allContainers.Count; ++index)
      {
        if (this.allContainers[index] is Array)
          ((Hyperlink) ((object[]) this.allContainers[index])[4]).Click += new EventHandler(this.showMoreLines_Click);
      }
      this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      this.labelFormName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelFormName");
      this.formLoading = true;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (this.inputData is DisclosedLEHandler || this.inputData is DisclosedCDHandler)
        return ControlState.Disabled;
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "L81":
          if (this.GetField("CD3.X445") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "L86":
          if (this.GetField("CD3.X469") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "L90":
          if (this.GetField("CD3.X493") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "CD3.X24":
          if (this.GetField("CD3.X517") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "CD3.X26":
          if (this.GetField("CD3.X541") == "Y")
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
      for (int index = 1; index <= 7; ++index)
        this.SetControlState("ImageButton" + (object) index, true);
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (!this.dateRangeValidation(id, val, 95, 96, 0) || !this.dateRangeValidation(id, val, 101, 102, 0) || !this.dateRangeValidation(id, val, 107, 108, 0) || !this.dateRangeValidation(id, val, 159, 160, 0) || !this.dateRangeValidation(id, val, 165, 166, 0) || !this.dateRangeValidation(id, val, 171, 172, 0) || !this.dateRangeValidation(id, val, 598, 598, 5) || !this.dateRangeValidation(id, val, 688, 688, 5) || !this.dateRangeValidation(id, val, 778, 778, 5) || !this.dateRangeValidation(id, val, 868, 868, 5) || !this.dateRangeValidation(id, val, 958, 1043, 5) || !this.dateRangeValidation(id, val, 1234, 1235, 5) || !this.dateRangeValidation(id, val, 1324, 1325, 5) || !this.dateRangeValidation(id, val, 1414, 1499, 5))
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
        for (int index = 0; index < this.allContainers.Count; ++index)
        {
          if (this.allContainers[index] is Array)
          {
            object[] allContainer = (object[]) this.allContainers[index];
            ((ContentControl) allContainer[0]).BackColor = Color.White;
            ((ContentControl) allContainer[2]).BackColor = Color.White;
          }
        }
        for (int index = 0; index < this.allContainers.Count; ++index)
        {
          if (this.allContainers[index] is Array)
            ((RuntimeControl) ((object[]) this.allContainers[index])[2]).Visible = false;
        }
        this.rearrangeLayout();
      }
      this.formLoading = false;
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (!action.StartsWith("switch"))
        return;
      string str = action.Substring(6);
      for (int index = 0; index < this.allContainers.Count; ++index)
      {
        if (this.allContainers[index] is EllieMae.Encompass.Forms.Panel)
        {
          EllieMae.Encompass.Forms.Panel allContainer = (EllieMae.Encompass.Forms.Panel) this.allContainers[index];
          if (allContainer.ControlID.EndsWith(str + "All"))
          {
            allContainer.Visible = !allContainer.Visible;
            break;
          }
        }
      }
      this.rearrangeLayout();
    }

    private void showMoreLines_Click(object sender, EventArgs e)
    {
      Hyperlink hyperlink = (Hyperlink) sender;
      if (hyperlink.Text == "Show More Lines")
        hyperlink.Text = "Show Less Lines";
      else if (hyperlink.Text == "Show Less Lines")
        hyperlink.Text = "Show More Lines";
      string str = hyperlink.ControlID.Substring(9);
      for (int index = 0; index < this.allContainers.Count; ++index)
      {
        if (this.allContainers[index] is Array)
        {
          object[] allContainer = (object[]) this.allContainers[index];
          if (((EllieMae.Encompass.Forms.Control) allContainer[0]).ControlID.EndsWith(str))
          {
            EllieMae.Encompass.Forms.Panel panel = (EllieMae.Encompass.Forms.Panel) allContainer[2];
            panel.Visible = !panel.Visible;
            break;
          }
        }
      }
      this.rearrangeLayout();
    }

    private void rearrangeLayout()
    {
      int height1 = 26;
      int num1 = this.panelM.Top + this.panelM.Size.Height;
      Size size1;
      for (int index1 = 0; index1 < this.allContainers.Count; ++index1)
      {
        if (this.allContainers[index1] is CategoryBox)
        {
          CategoryBox allContainer1 = (CategoryBox) this.allContainers[index1];
          EllieMae.Encompass.Forms.Panel allContainer2 = (EllieMae.Encompass.Forms.Panel) this.allContainers[index1 + 1];
          if (allContainer2.Visible)
          {
            int height2 = allContainer1.ControlID == "boxM3" ? 326 : (allContainer1.ControlID == "boxM12" ? 120 : (allContainer1.ControlID == "boxN6" ? 54 : (allContainer1.ControlID == "boxN9" ? 120 : (allContainer1.ControlID == "boxN17" ? 76 : 25))));
            for (int index2 = index1 + 2; index2 < this.allContainers.Count; ++index2)
            {
              if (this.allContainers[index2] is Array)
              {
                object[] allContainer3 = (object[]) this.allContainers[index2];
                EllieMae.Encompass.Forms.Panel panel1 = (EllieMae.Encompass.Forms.Panel) allContainer3[0];
                EllieMae.Encompass.Forms.Panel panel2 = (EllieMae.Encompass.Forms.Panel) allContainer3[1];
                EllieMae.Encompass.Forms.Panel panel3 = (EllieMae.Encompass.Forms.Panel) allContainer3[2];
                EllieMae.Encompass.Forms.Panel panel4 = (EllieMae.Encompass.Forms.Panel) allContainer3[3];
                EllieMae.Encompass.Forms.Panel panel5 = panel4;
                int top1 = panel2.Top;
                size1 = panel2.Size;
                int height3 = size1.Height;
                int num2 = top1 + height3;
                int num3;
                if (!panel3.Visible)
                {
                  num3 = 0;
                }
                else
                {
                  size1 = panel3.Size;
                  num3 = size1.Height;
                }
                int num4 = num2 + num3;
                panel5.Top = num4;
                EllieMae.Encompass.Forms.Panel panel6 = panel1;
                size1 = panel1.Size;
                int width = size1.Width;
                int top2 = panel4.Top;
                size1 = panel4.Size;
                int height4 = size1.Height;
                int height5 = top2 + height4;
                Size size2 = new Size(width, height5);
                panel6.Size = size2;
                panel1.Top = height2;
                int num5 = height2;
                size1 = panel1.Size;
                int height6 = size1.Height;
                height2 = num5 + height6;
              }
              else if (this.allContainers[index2] is CategoryBox)
              {
                index1 = index2 - 1;
                break;
              }
            }
            if (height2 > 25)
            {
              EllieMae.Encompass.Forms.Panel panel = allContainer2;
              size1 = allContainer2.Size;
              Size size3 = new Size(size1.Width, height2);
              panel.Size = size3;
            }
            CategoryBox categoryBox = allContainer1;
            size1 = allContainer1.Size;
            int width1 = size1.Width;
            size1 = allContainer2.Size;
            int height7 = size1.Height + 25;
            Size size4 = new Size(width1, height7);
            categoryBox.Size = size4;
            allContainer1.Top = num1;
            int num6 = num1;
            size1 = allContainer1.Size;
            int height8 = size1.Height;
            num1 = num6 + height8;
          }
          else
          {
            CategoryBox categoryBox = allContainer1;
            size1 = allContainer1.Size;
            Size size5 = new Size(size1.Width, height1);
            categoryBox.Size = size5;
            allContainer1.Top = num1;
            num1 += height1;
          }
          if (allContainer1.ControlID == "boxM12")
          {
            int num7 = num1 + 1;
            this.panelN.Top = num7;
            int num8 = num7;
            size1 = this.panelN.Size;
            int height9 = size1.Height;
            num1 = num8 + height9;
          }
        }
      }
      EllieMae.Encompass.Forms.Panel pnlForm = this.pnlForm;
      size1 = this.pnlForm.Size;
      Size size6 = new Size(size1.Width, num1 + 10);
      pnlForm.Size = size6;
      EllieMae.Encompass.Forms.Label labelFormName = this.labelFormName;
      size1 = this.pnlForm.Size;
      int num9 = size1.Height + 10;
      labelFormName.Top = num9;
    }
  }
}
