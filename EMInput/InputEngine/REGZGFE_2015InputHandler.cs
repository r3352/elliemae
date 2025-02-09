// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.REGZGFE_2015InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
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
  public class REGZGFE_2015InputHandler : REGZGFE_2010InputHandler
  {
    private EllieMae.Encompass.Forms.Form form;
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private EllieMae.Encompass.Forms.Panel grpLegend;
    private List<EllieMae.Encompass.Forms.Panel> panelSections;
    private List<EllieMae.Encompass.Forms.Panel> panelSectionDetails;
    private List<ImageButton> panelSummarySwitches;
    private List<EllieMae.Encompass.Forms.Panel> panelSellerColumns;
    private EllieMae.Encompass.Forms.Panel panelSummary;
    private List<QuickEntryPopupDialog> detailDialogs;
    private EllieMae.Encompass.Forms.GroupBox grpTitleBorrower;
    private ImageButton imagePlus;
    private ImageButton imagePlusOver;
    private ImageButton imageMinus;
    private ImageButton imageMinusOver;
    private ImageButton imagePop;
    private ImageButton imagePopOver;
    private ImageButton imageHighlight;
    private ImageButton imageHighlightOver;
    private ImageButton imageVideo;
    private ImageButton imageVideoOver;
    private EllieMae.Encompass.Forms.Panel panel1000Monthly;
    private EllieMae.Encompass.Forms.Panel panel1000Biweekly;
    private EllieMae.Encompass.Forms.Label[] section1000UnitLables;
    private EllieMae.Encompass.Forms.Panel CPC;
    private EllieMae.Encompass.Forms.Panel CPCExpanded;
    private EllieMae.Encompass.Forms.Panel panel1300;
    private EllieMae.Encompass.Forms.Label labelItemM7;
    private EllieMae.Encompass.Forms.Panel pnlItemM7;
    private EllieMae.Encompass.Forms.Panel pnlISectionMVersion1;
    private EllieMae.Encompass.Forms.Panel pnlISectionMVersion2;
    private EllieMae.Encompass.Forms.Panel pnlISectionN;
    private EllieMae.Encompass.Forms.Label labelSectionLversion1;
    private EllieMae.Encompass.Forms.Label labelSectionLversion2;
    private EllieMae.Encompass.Forms.Label labelM11;
    private EllieMae.Encompass.Forms.GroupBox grpDetailofTransaction;
    private EllieMae.Encompass.Forms.Panel pnlItemINoHeloc;
    private EllieMae.Encompass.Forms.Panel pnlItemIHeloc;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFees;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFeesNew;
    private int totalHeighSections;
    private EllieMae.Encompass.Forms.CheckBox itemizedCheckBox;
    private bool firstLoading = true;
    private string highlightField = string.Empty;
    private FieldLock lock_1851;
    private FieldLock lock2_1851;
    private EllieMae.Encompass.Forms.TextBox input_f_1851;
    private EllieMae.Encompass.Forms.TextBox input2_f_1851;
    private mshtml.IHTMLEventObj clickedpEvtObj;

    public REGZGFE_2015InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(mainScreen, htmldoc, form, property)
    {
    }

    public REGZGFE_2015InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public REGZGFE_2015InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZGFE_2015InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZGFE_2015InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmlDoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, owner, htmlInput, htmlDoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      base.CreateControls();
      if (this.session != null && this.session.LoanDataMgr != null)
      {
        this.session.LoanDataMgr.AfterDDMApplied -= new EventHandler(this.loanDataMgr_AfterDDMApplied);
        this.session.LoanDataMgr.AfterDDMApplied += new EventHandler(this.loanDataMgr_AfterDDMApplied);
      }
      try
      {
        this.form = (EllieMae.Encompass.Forms.Form) this.currentForm.FindControl("Form1");
        this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.labelFormName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelFormName");
        this.grpTitleBorrower = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("grpTitleBorrower");
        this.imagePlus = (ImageButton) this.currentForm.FindControl("imagePlus");
        this.imagePlusOver = (ImageButton) this.currentForm.FindControl("imagePlusOver");
        this.imageMinus = (ImageButton) this.currentForm.FindControl("imageMinus");
        this.imageMinusOver = (ImageButton) this.currentForm.FindControl("imageMinusOver");
        this.imagePop = (ImageButton) this.currentForm.FindControl("imagePop");
        this.imagePopOver = (ImageButton) this.currentForm.FindControl("imagePopOver");
        this.imageHighlight = (ImageButton) this.currentForm.FindControl("imageHighlight");
        this.imageHighlightOver = (ImageButton) this.currentForm.FindControl("imageHighlightOver");
        this.imageVideo = (ImageButton) this.currentForm.FindControl("imageVideo");
        this.imageVideoOver = (ImageButton) this.currentForm.FindControl("imageVideoOver");
        this.grpLegend = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("grpLegend");
        this.CPC = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelCPC");
        this.CPCExpanded = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelCPCExpanded");
        this.panel1300 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panel1300");
        this.panelSections = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelSectionDetails = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 700; index <= 1400; index += 100)
        {
          this.panelSections.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panel" + (object) index));
          this.panelSectionDetails.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panel" + (object) index + "Expand"));
        }
        this.panelSectionDetails.Add(this.CPCExpanded);
        this.panelSellerColumns = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 700; index <= 1300; index += 100)
          this.panelSellerColumns.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSeller" + (object) index));
        this.panelSummary = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSummary");
        this.panelSummarySwitches = new List<ImageButton>();
        for (int index = 700; index <= 1400; index += 100)
          this.panelSummarySwitches.Add((ImageButton) this.currentForm.FindControl("btnSwitch" + (object) index));
        this.panelSummarySwitches.Add((ImageButton) this.currentForm.FindControl("btnSwitchCPC"));
        this.panel1000Monthly = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panel1000Monthly");
        this.panel1000Biweekly = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panel1000Biweekly");
        this.panel1000Biweekly.Position = new Point(this.panel1000Monthly.Left, this.panel1000Monthly.Top);
        this.section1000UnitLables = new EllieMae.Encompass.Forms.Label[9];
        for (int index = 0; index < 9; ++index)
          this.section1000UnitLables[index] = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("l_" + (object) (1002 + index) + "Unit");
        this.pnlItemINoHeloc = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlItemINoHeloc");
        this.pnlItemIHeloc = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlItemIHeloc");
        this.pnlItemIHeloc.Left = 1;
        this.pnlItemIHeloc.Top = this.pnlItemINoHeloc.Top;
        this.grpDetailofTransaction = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("grpDetailofTransaction");
        this.pnlItemM7 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlItemM7");
        this.labelItemM7 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label_M7");
        this.labelM11 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label315");
        this.pnlISectionMVersion1 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("MSectionVersion1");
        this.pnlISectionMVersion2 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("MSectionVersion2");
        this.pnlISectionN = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("SectionN");
        this.labelSectionLversion1 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("SectionLRespaVersion1");
        this.labelSectionLversion2 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("SectionLRespaVersion2");
        if (this.pnlISectionMVersion1 != null && this.pnlISectionMVersion2 != null)
          this.pnlISectionMVersion2.Position = this.pnlISectionMVersion1.Position;
        if (this.labelSectionLversion1 != null && this.labelSectionLversion2 != null)
          this.labelSectionLversion2.Position = this.labelSectionLversion1.Position;
        this.pnlBorPaidFees = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesPanel");
        this.pnlBorPaidFeesNew = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesNewPanel");
        this.itemizedCheckBox = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("CheckBox301");
        this.lock_1851 = (FieldLock) this.currentForm.FindControl("lock_1851");
        this.lock2_1851 = (FieldLock) this.currentForm.FindControl("lock2_1851");
        this.input_f_1851 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("input_f_1851");
        this.input2_f_1851 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("input2_f_1851");
      }
      catch (Exception ex)
      {
      }
      if (this.inputData is DisclosedItemizationHandler)
        this.RefreshLayout(false, true, (string) null);
      else
        this.RefreshLayout(true, false, (string) null);
      this.ChangeView((string) null);
      if (this.detailDialogs != null)
        this.detailDialogs.Clear();
      this.detailDialogs = new List<QuickEntryPopupDialog>();
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      this.RefreshFeeDetails();
      if (!(this.highlightField != string.Empty))
        return;
      this.HighlightLines(this.highlightField, id);
    }

    internal void RefreshFeeDetails()
    {
      if (this.detailDialogs == null)
        return;
      for (int index = 0; index < this.detailDialogs.Count; ++index)
        this.detailDialogs[index].RefreshContents();
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (action.StartsWith("switch"))
        this.RefreshLayout(false, false, action);
      else if (action.StartsWith("zoomin"))
        this.showDetails(action);
      else if (action.StartsWith("video"))
        this.showVideo(action);
      else if (action == "recordingfee" && this.loan != null)
        this.loan.Calculator.Calculate2015FeeDetails("390", "SYS.X355");
      else if (this.loan != null && (action == "localtax" || action == "cityfee"))
        this.loan.Calculator.Calculate2015FeeDetails("647", "SYS.X357");
      else if (this.loan != null && (action == "statetax" || action == "statefee"))
        this.loan.Calculator.Calculate2015FeeDetails("648", "SYS.X359");
      else if (this.loan != null && action == "userfee1")
        this.loan.Calculator.Calculate2015FeeDetails("374", "SYS.X361");
      else if (this.loan != null && action == "userfee2")
      {
        this.loan.Calculator.Calculate2015FeeDetails("1641", "SYS.X363");
      }
      else
      {
        if (this.loan == null || !(action == "userfee3"))
          return;
        this.loan.Calculator.Calculate2015FeeDetails("1644", "SYS.X365");
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      this.firstLoading = false;
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      try
      {
        this.updateDetailOfTransctionLayout();
      }
      catch (Exception ex)
      {
      }
    }

    private void updateDetailOfTransctionLayout()
    {
      if (this.inputData is ClosingCost)
      {
        this.panelSummary.Visible = false;
        this.CPC.Top = this.panel1300.Bottom + 2;
        this.pnlForm.Size = new Size(this.pnlForm.Size.Width, this.CPC.Top + 2 + this.CPC.Size.Height);
        this.labelFormName.Top = this.pnlForm.Size.Height + 6;
      }
      else
      {
        if (this.inputData.GetField("1172") == "HELOC")
        {
          this.pnlItemIHeloc.Visible = true;
          this.pnlItemINoHeloc.Visible = false;
        }
        else
        {
          this.pnlItemIHeloc.Visible = false;
          this.pnlItemINoHeloc.Visible = true;
        }
        if (this.inputData.GetField("4796") == "Y")
        {
          if (!this.firstLoading)
          {
            this.pnlISectionMVersion2.Visible = true;
            this.pnlISectionMVersion1.Visible = false;
          }
          this.pnlISectionN.Position = new Point(0, 816);
          this.enableOrDisableDropdownPanels(this.pnlBorPaidFeesNew, this.pnlBorPaidFees);
          this.labelSectionLversion2.Visible = true;
          this.labelSectionLversion1.Visible = false;
        }
        else
        {
          if (!this.firstLoading)
          {
            this.pnlISectionMVersion1.Visible = true;
            this.pnlISectionMVersion2.Visible = false;
          }
          this.pnlISectionN.Position = new Point(0, 672);
          this.enableOrDisableDropdownPanels(this.pnlBorPaidFees, this.pnlBorPaidFeesNew);
          this.labelSectionLversion2.Visible = false;
          this.labelSectionLversion1.Visible = true;
        }
        if (!(this.inputData.GetField("1825") == "2020"))
          this.itemizedCheckBox.Visible = false;
        else
          this.itemizedCheckBox.Visible = true;
        EllieMae.Encompass.Forms.GroupBox detailofTransaction = this.grpDetailofTransaction;
        int width1 = this.grpDetailofTransaction.Size.Width;
        int top1 = this.pnlISectionN.Top;
        Size size1 = this.pnlISectionN.Size;
        int height1 = size1.Height;
        int height2 = top1 + height1;
        Size size2 = new Size(width1, height2);
        detailofTransaction.Size = size2;
        EllieMae.Encompass.Forms.Panel panelSummary = this.panelSummary;
        size1 = this.panelSummary.Size;
        int width2 = size1.Width;
        int top2 = this.grpDetailofTransaction.Top;
        size1 = this.grpDetailofTransaction.Size;
        int height3 = size1.Height;
        int height4 = top2 + height3;
        Size size3 = new Size(width2, height4);
        panelSummary.Size = size3;
        this.panelSummary.Top = this.totalHeighSections + 1;
        this.CPC.Top = this.panelSummary.Bottom + 20;
        EllieMae.Encompass.Forms.Panel pnlForm = this.pnlForm;
        size1 = this.pnlForm.Size;
        int width3 = size1.Width;
        int top3 = this.panelSummary.Top;
        size1 = this.panelSummary.Size;
        int height5 = size1.Height;
        int num1 = top3 + height5;
        int num2;
        if (!this.CPC.Visible)
        {
          num2 = 0;
        }
        else
        {
          size1 = this.CPC.Size;
          num2 = size1.Height;
        }
        int height6 = num1 + num2 + 30;
        Size size4 = new Size(width3, height6);
        pnlForm.Size = size4;
        EllieMae.Encompass.Forms.Label labelFormName = this.labelFormName;
        size1 = this.pnlForm.Size;
        int num3 = size1.Height + 6;
        labelFormName.Top = num3;
      }
    }

    public void RefreshLayout(bool showAll, bool hideAll, string actionID)
    {
      bool flag1 = this.GetFieldValue("423") == "Biweekly";
      string fieldValue = this.GetFieldValue("19");
      bool flag2 = fieldValue == "ConstructionToPermanent" || fieldValue == "ConstructionOnly";
      if (flag1)
      {
        this.panel1000Monthly.Visible = false;
        this.panel1000Biweekly.Visible = true;
        this.panel1000Biweekly.Enabled = true;
      }
      else
      {
        this.panel1000Monthly.Visible = true;
        this.panel1000Biweekly.Visible = false;
        this.panel1000Monthly.Enabled = true;
      }
      for (int index = 0; index < 9; ++index)
        this.section1000UnitLables[index].Text = flag1 ? "bwks @ $" : "mths @ $";
      for (int index = 0; index < this.panelSummarySwitches.Count; ++index)
      {
        if (hideAll || actionID != null && this.panelSummarySwitches[index].Action == actionID && (this.panelSummarySwitches[index].Source == this.imageMinus.Source || this.panelSummarySwitches[index].Source == this.imageMinusOver.Source))
        {
          if (this.panelSummarySwitches[index].Source == this.imageMinus.Source)
            this.panelSummarySwitches[index].Source = this.imagePlus.Source;
          else if (this.panelSummarySwitches[index].Source == this.imageMinusOver.Source)
            this.panelSummarySwitches[index].Source = this.imagePlusOver.Source;
        }
        else if (showAll || actionID != null && this.panelSummarySwitches[index].Action == actionID && (this.panelSummarySwitches[index].Source == this.imagePlus.Source || this.panelSummarySwitches[index].Source == this.imagePlusOver.Source))
        {
          if (this.panelSummarySwitches[index].Source == this.imagePlus.Source)
            this.panelSummarySwitches[index].Source = this.imageMinus.Source;
          else if (this.panelSummarySwitches[index].Source == this.imagePlusOver.Source)
            this.panelSummarySwitches[index].Source = this.imageMinusOver.Source;
        }
      }
      int num1 = this.grpLegend.Top + this.grpLegend.Size.Height;
      Size size1;
      for (int index = 0; index < this.panelSummarySwitches.Count; ++index)
      {
        if (this.inputData is ClosingCost && this.panelSummarySwitches[index].ControlID != "btnSwitchCPC" && this.panelSections[index].ControlID == "panel1400")
        {
          this.panelSections[index].Visible = false;
        }
        else
        {
          if (this.panelSummarySwitches[index].ControlID != "btnSwitchCPC")
            this.panelSections[index].Top = num1;
          if (this.panelSummarySwitches[index].Source == this.imageMinus.Source || this.panelSummarySwitches[index].Source == this.imageMinusOver.Source)
          {
            this.panelSectionDetails[index].Visible = true;
            if (this.panelSummarySwitches[index].ControlID == "btnSwitchCPC")
            {
              if (flag2 || this.inputData is ClosingCost)
              {
                EllieMae.Encompass.Forms.Panel cpc = this.CPC;
                size1 = this.CPC.Size;
                int width = size1.Width;
                size1 = this.CPCExpanded.Size;
                int height = size1.Height + 51;
                Size size2 = new Size(width, height);
                cpc.Size = size2;
              }
              else
              {
                this.CPC.Size = new Size(0, 0);
                this.CPC.Visible = false;
              }
            }
            else
            {
              EllieMae.Encompass.Forms.Panel panelSection = this.panelSections[index];
              size1 = this.panelSections[index].Size;
              int width = size1.Width;
              size1 = this.panelSectionDetails[index].Size;
              int height1 = size1.Height + 51;
              Size size3 = new Size(width, height1);
              panelSection.Size = size3;
              int num2 = num1;
              size1 = this.panelSections[index].Size;
              int height2 = size1.Height;
              num1 = num2 + height2;
            }
          }
          else
          {
            this.panelSectionDetails[index].Visible = false;
            if (this.panelSummarySwitches[index].ControlID == "btnSwitchCPC")
            {
              if (flag2 || this.inputData is ClosingCost)
              {
                EllieMae.Encompass.Forms.Panel cpc = this.CPC;
                size1 = this.CPC.Size;
                Size size4 = new Size(size1.Width, 51);
                cpc.Size = size4;
              }
              else
              {
                this.CPC.Size = new Size(0, 0);
                this.CPC.Visible = false;
              }
            }
            else
            {
              EllieMae.Encompass.Forms.Panel panelSection = this.panelSections[index];
              size1 = this.panelSections[index].Size;
              Size size5 = new Size(size1.Width, 51);
              panelSection.Size = size5;
              int num3 = num1;
              size1 = this.panelSections[index].Size;
              int height = size1.Height;
              num1 = num3 + height;
            }
          }
        }
      }
      this.totalHeighSections = num1;
      if (this.inputData is ClosingCost)
      {
        this.panelSummary.Visible = false;
        this.CPC.Top = this.panel1300.Bottom + 2;
        EllieMae.Encompass.Forms.Panel pnlForm = this.pnlForm;
        size1 = this.pnlForm.Size;
        int width = size1.Width;
        int num4 = num1 + 2;
        size1 = this.CPC.Size;
        int height3 = size1.Height;
        int height4 = num4 + height3;
        Size size6 = new Size(width, height4);
        pnlForm.Size = size6;
        EllieMae.Encompass.Forms.Label labelFormName = this.labelFormName;
        size1 = this.pnlForm.Size;
        int num5 = size1.Height + 6;
        labelFormName.Top = num5;
      }
      else
        this.updateDetailOfTransctionLayout();
      if (this.loan == null)
        return;
      if (this.loan.LinkedData != null && (this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked))
      {
        this.lock_1851.Visible = this.lock2_1851.Visible = true;
        if (this.lock_1851.ControlToLock == null)
        {
          this.lock_1851.ControlToLock = (EllieMae.Encompass.Forms.Control) this.input_f_1851;
          if (!this.loan.IsLocked("1851"))
            this.input_f_1851.Enabled = false;
          this.lock_1851.DisplayImage(this.loan.IsLocked("1851"));
        }
        if (this.lock2_1851.ControlToLock != null)
          return;
        this.lock2_1851.ControlToLock = (EllieMae.Encompass.Forms.Control) this.input2_f_1851;
        if (!this.loan.IsLocked("1851"))
          this.input2_f_1851.Enabled = false;
        this.lock2_1851.DisplayImage(this.loan.IsLocked("1851"));
      }
      else
      {
        if (this.loan.IsLocked("1851"))
          this.loan.RemoveLock("1851");
        this.lock_1851.Visible = this.lock2_1851.Visible = false;
        if (this.lock_1851.ControlToLock != null)
          this.lock_1851.ControlToLock = (EllieMae.Encompass.Forms.Control) null;
        this.input_f_1851.Enabled = true;
        if (this.lock2_1851.ControlToLock != null)
          this.lock2_1851.ControlToLock = (EllieMae.Encompass.Forms.Control) null;
        this.input2_f_1851.Enabled = true;
      }
    }

    public void ChangeView(string viewMode)
    {
      for (int index = 0; index < this.panelSellerColumns.Count; ++index)
      {
        this.panelSellerColumns[index].Top = 0;
        this.panelSellerColumns[index].Left = this.grpTitleBorrower.Left + this.grpTitleBorrower.Size.Width + 1;
        this.panelSellerColumns[index].Visible = viewMode == null || viewMode == "Standard";
      }
    }

    public void HighlightLines(string highlightField, string fieldInFocus)
    {
      if (this.highlightField == highlightField)
        return;
      this.highlightField = highlightField;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      string empty6 = string.Empty;
      string empty7 = string.Empty;
      string empty8 = string.Empty;
      for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
      {
        string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
        string str1 = strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER];
        if (str1.StartsWith("0"))
          str1 = str1.Substring(1);
        string str2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID];
        string str3 = strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID];
        string id1 = strArray[HUDGFE2010Fields.PTCPOCINDEX_APR];
        string id2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP];
        string id3 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT];
        string id4 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT];
        string id5 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT];
        if (fieldInFocus == null || !(id1 != fieldInFocus) || !(str3 != fieldInFocus) || !(id2 != fieldInFocus) || !(id3 != fieldInFocus) || !(id4 != fieldInFocus) || !(id5 != fieldInFocus))
        {
          ImageButton control;
          try
          {
            control = (ImageButton) this.currentForm.FindControl("btnPop" + str1 + str2);
            if (control == null)
            {
              if (fieldInFocus != null)
                break;
              continue;
            }
          }
          catch (Exception ex)
          {
            if (fieldInFocus != null)
              break;
            continue;
          }
          control.Source = !(highlightField == string.Empty) ? (!(highlightField == "APR") || !(this.inputData.GetField(id1) == "Y") ? (!(highlightField == "Did shop for") || !(id2 != "") || !(this.inputData.GetField(id2) == "Y") ? (!(highlightField == "Did not shop for") || !(id2 == "") && !(this.inputData.GetField(id2) != "Y") ? (!(highlightField == "POC Seller") || !(id3 != "") || !(this.inputData.GetField(id3) != "") ? (!(highlightField == "POC Buyer") || !(id4 != "") || !(this.inputData.GetField(id4) != "") ? (!(highlightField == "Section 32 Fees") || !(id5 != "") || !(this.inputData.GetField(id5) != "") ? this.imagePop.Source : this.imageHighlight.Source) : this.imageHighlight.Source) : this.imageHighlight.Source) : this.imageHighlight.Source) : this.imageHighlight.Source) : this.imageHighlight.Source) : this.imagePop.Source;
          if (fieldInFocus != null)
            break;
        }
      }
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      this.clickedpEvtObj = pEvtObj;
      return base.onclick(pEvtObj);
    }

    private void showDetails(string actionID)
    {
      if (this.detailDialogs != null && this.detailDialogs.Count > 0)
      {
        foreach (QuickEntryPopupDialog detailDialog in this.detailDialogs)
        {
          if (detailDialog.HUDLineNo == actionID.Replace("zoomin", ""))
          {
            detailDialog.Location = new Point(this.clickedpEvtObj.screenX, this.clickedpEvtObj.screenY + 18);
            detailDialog.Focus();
            return;
          }
        }
      }
      QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog(this.inputData is ClosingCost ? (IHtmlInput) this.ccLoan : this.inputData, "Fee Details", new InputFormInfo("REGZGFE_2015_Details", "REGZGFE_2015_Details"), 660, 650, FieldSource.CurrentLoan, "Fee_Details", this.session, (object) actionID.Replace("zoomin", ""));
      entryPopupDialog.HUDLineNo = actionID.Replace("zoomin", "");
      if (entryPopupDialog.HUDLineNo == "2001" || entryPopupDialog.HUDLineNo == "2002" || entryPopupDialog.HUDLineNo == "2003" || entryPopupDialog.HUDLineNo == "2004")
        entryPopupDialog.Text = "Fee Details - PC" + entryPopupDialog.HUDLineNo.Substring(3, 1);
      else
        entryPopupDialog.Text = "Fee Details - " + (entryPopupDialog.HUDLineNo == "802x" || entryPopupDialog.HUDLineNo == "803x" || entryPopupDialog.HUDLineNo == "1101x" ? entryPopupDialog.HUDLineNo.Substring(0, entryPopupDialog.HUDLineNo.Length - 1) : entryPopupDialog.HUDLineNo);
      entryPopupDialog.FormBorderStyle = FormBorderStyle.Sizable;
      entryPopupDialog.ShowIcon = false;
      entryPopupDialog.MaximizeBox = false;
      entryPopupDialog.StartPosition = FormStartPosition.Manual;
      entryPopupDialog.Location = new Point(this.clickedpEvtObj.screenX, this.clickedpEvtObj.screenY + 18);
      entryPopupDialog.FormClosing += new FormClosingEventHandler(this.quickPage_FormClosing);
      entryPopupDialog.OkClicked += new EventHandler(this.quickPage_OkClicked);
      entryPopupDialog.LoanScreenLoaded += new EventHandler(this.quickPage_LoanScreenLoaded);
      entryPopupDialog.OnFieldChanged += new EventHandler(this.quickPage_OnFieldChanged);
      entryPopupDialog.Disposed += new EventHandler(this.quickPage_Disposed);
      entryPopupDialog.ButtonClicked += new EventHandler(this.quickPage_OnButtonClicked);
      entryPopupDialog.OnDialogDeactivated += new EventHandler(this.quickPage_OnDialogDeactivated);
      entryPopupDialog.Location = new Point(this.clickedpEvtObj.screenX + 10, (SystemInformation.VirtualScreen.Height - entryPopupDialog.Height) / 2);
      entryPopupDialog.Show((IWin32Window) this.session.MainForm);
      entryPopupDialog.BringToFront();
      this.detailDialogs.Add(entryPopupDialog);
    }

    private void showVideo(string actionID)
    {
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(actionID))
      {
        case 225416445:
          if (!(actionID == "video1000"))
            break;
          WebViewer.OpenURL("http://help.icemortgagetechnology.com/GA/video_2015Itemization_1000.html", "2015 Itemization Input Form - Section 1000", 1046, 732);
          break;
        case 337628969:
          if (!(actionID == "video700"))
            break;
          WebViewer.OpenURL("http://help.icemortgagetechnology.com/GA/video_2015Itemization_700.html", "2015 Itemization Input Form - Section 700", 1046, 732);
          break;
        case 451084476:
          if (!(actionID == "video800"))
            break;
          WebViewer.OpenURL("http://help.icemortgagetechnology.com/GA/video_2015Itemization_800.html", "2015 Itemization Input Form - Section 800", 1046, 732);
          break;
        case 817580220:
          if (!(actionID == "video1300"))
            break;
          WebViewer.OpenURL("http://help.icemortgagetechnology.com/GA/video_2015Itemization_1300.html", "2015 Itemization Input Form - Section 1300", 1046, 732);
          break;
        case 2396532434:
          if (!(actionID == "video1100"))
            break;
          WebViewer.OpenURL("http://help.icemortgagetechnology.com/GA/video_2015Itemization_1100.html", "2015 Itemization Input Form - Section 1100", 1046, 732);
          break;
        case 2595724135:
          if (!(actionID == "video900"))
            break;
          WebViewer.OpenURL("http://help.icemortgagetechnology.com/GA/video_2015Itemization_900.html", "2015 Itemization Input Form - Section 900", 1046, 732);
          break;
        case 2962219879:
          if (!(actionID == "video1200"))
            break;
          WebViewer.OpenURL("http://help.icemortgagetechnology.com/GA/video_2015Itemization_1200.html", "2015 Itemization Input Form - Section 1200", 1046, 732);
          break;
        case 3525580017:
          if (!(actionID == "video1400"))
            break;
          WebViewer.OpenURL("http://help.icemortgagetechnology.com/GA/video_2015Itemization_1400.html", "2015 Itemization Input Form - Section 1400", 1046, 732);
          break;
      }
    }

    private void quickPage_Disposed(object sender, EventArgs e)
    {
      if (!(((QuickEntryPopupDialog) sender).HUDLineNo != ""))
        return;
      this.RunCalculation((string) null, (string) null);
      this.RefreshContents();
    }

    private void quickPage_OkClicked(object sender, EventArgs e)
    {
      QuickEntryPopupDialog entryPopupDialog = (QuickEntryPopupDialog) sender;
      bool flag = entryPopupDialog.Text.Trim().StartsWith("Fee Details - ");
      entryPopupDialog.Disposed -= new EventHandler(this.quickPage_Disposed);
      entryPopupDialog.Close();
      entryPopupDialog.Dispose();
      if (!flag)
        return;
      this.RunCalculation((string) null, (string) null);
      this.RefreshContents();
    }

    private void quickPage_LoanScreenLoaded(object sender, EventArgs e)
    {
      QuickEntryPopupDialog entryPopupDialog = (QuickEntryPopupDialog) sender;
      if (this.inputData is ClosingCost)
        ((REGZGFE_2015_DETAILSInputHandler) entryPopupDialog.GetInputHandler()).OnRefreshed += new EventHandler(this.feeDetailsInputHandler_OnRefreshed);
      if (!(entryPopupDialog.HUDLineNo == "1007") && !(entryPopupDialog.HUDLineNo == "1008") && !(entryPopupDialog.HUDLineNo == "1009") && !(entryPopupDialog.HUDLineNo == "907") && !(entryPopupDialog.HUDLineNo == "908") && !(entryPopupDialog.HUDLineNo == "909") && !(entryPopupDialog.HUDLineNo == "910") && !(entryPopupDialog.HUDLineNo == "911") && !(entryPopupDialog.HUDLineNo == "912"))
        return;
      entryPopupDialog.Height = 710;
    }

    private void feeDetailsInputHandler_OnRefreshed(object sender, EventArgs e)
    {
      this.RefreshContents();
    }

    private void quickPage_OnFieldChanged(object sender, EventArgs e)
    {
      this.RefreshContents(true);
      this.HighlightLines(this.highlightField, (string) sender);
    }

    private void quickPage_OnDialogDeactivated(object sender, EventArgs e)
    {
      if (this.inputData is ClosingCost)
        this.SetTemplate();
      this.RunCalculation((string) null, (string) null);
      if (this.inputData is LoanData && this.loan.IsTemplate)
      {
        this.RefreshContents();
      }
      else
      {
        this.session.Application.GetService<ILoanEditor>()?.RefreshContents();
        this.RefreshContents();
      }
    }

    private void quickPage_OnButtonClicked(object sender, EventArgs e)
    {
      if (this.ccLoan != null)
        this.SetTemplate();
      this.RefreshContents();
      this.HighlightLines(this.highlightField, (string) null);
    }

    private void quickPage_FormClosing(object sender, FormClosingEventArgs e)
    {
      QuickEntryPopupDialog entryPopupDialog = (QuickEntryPopupDialog) sender;
      IInputHandler inputHandler = entryPopupDialog.GetInputHandler();
      if (inputHandler is REGZGFE_2015_DETAILSInputHandler)
        ((InputHandlerBase) inputHandler).Dispose();
      this.detailDialogs.Remove(entryPopupDialog);
    }

    public override void onmouseout(mshtml.IHTMLEventObj pEvtObj)
    {
      RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
      if (!(controlForElement is ImageButton))
        return;
      ImageButton imageButton = (ImageButton) controlForElement;
      if (imageButton.Source == this.imageHighlightOver.Source)
        imageButton.Source = this.imageHighlight.Source;
      else if (imageButton.Source == this.imageVideoOver.Source)
        imageButton.Source = this.imageVideo.Source;
      else if (imageButton.Source == this.imagePopOver.Source)
        imageButton.Source = this.imagePop.Source;
      else if (imageButton.Source == this.imagePlusOver.Source)
      {
        imageButton.Source = this.imagePlus.Source;
      }
      else
      {
        if (!(imageButton.Source == this.imageMinusOver.Source))
          return;
        imageButton.Source = this.imageMinus.Source;
      }
    }

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
      if (controlForElement is ImageButton)
      {
        ImageButton imageButton = (ImageButton) controlForElement;
        if (imageButton.Source == this.imageHighlight.Source)
          imageButton.Source = this.imageHighlightOver.Source;
        else if (imageButton.Source == this.imageVideo.Source)
          imageButton.Source = this.imageVideoOver.Source;
        else if (imageButton.Source == this.imagePop.Source)
          imageButton.Source = this.imagePopOver.Source;
        else if (imageButton.Source == this.imagePlus.Source)
          imageButton.Source = this.imagePlusOver.Source;
        else if (imageButton.Source == this.imageMinus.Source)
          imageButton.Source = this.imageMinusOver.Source;
      }
      base.onmouseenter(pEvtObj);
    }

    public void CloseAllPopupWindows()
    {
      if (this.detailDialogs == null)
        return;
      while (this.detailDialogs.Count > 0)
        this.detailDialogs[0].Close();
      this.detailDialogs.Clear();
      this.detailDialogs = (List<QuickEntryPopupDialog>) null;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      return base.GetControlState(ctrl, id);
    }

    private void loanDataMgr_AfterDDMApplied(object sender, EventArgs e)
    {
      this.RefreshFeeDetails();
    }

    public override void Dispose()
    {
      if (this.session != null && this.session.LoanDataMgr != null)
        this.session.LoanDataMgr.AfterDDMApplied -= new EventHandler(this.loanDataMgr_AfterDDMApplied);
      this.form.Dispose();
      this.pnlForm.Dispose();
      base.Dispose();
    }
  }
}
