// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TAX4506TInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class TAX4506TInputHandler : VOLInputHandler, IInputHandler, IFormScreen, IRefreshContents
  {
    private EllieMae.Encompass.Forms.Label label1aFName;
    private EllieMae.Encompass.Forms.Label label1aLName;
    private EllieMae.Encompass.Forms.TextBox txt1aLName;
    private EllieMae.Encompass.Forms.Label label2aFName;
    private EllieMae.Encompass.Forms.Label label2aLName;
    private EllieMae.Encompass.Forms.TextBox txt2aLName;
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
    private EllieMae.Encompass.Forms.Panel oldPanel;
    private EllieMae.Encompass.Forms.Panel pnlFrm;
    private EllieMae.Encompass.Forms.Panel pnlNew;
    private EllieMae.Encompass.Forms.Panel pnlNewLocation;
    private EllieMae.Encompass.Forms.Label formLabel;
    private EllieMae.Encompass.Forms.Panel pnlLastSection;
    private EllieMae.Encompass.Forms.GroupBox grpBoxHeader;
    private EllieMae.Encompass.Forms.Panel panelOldLayoutForLoan;
    private EllieMae.Encompass.Forms.Panel panelOldLayoutForSetting;
    private EllieMae.Encompass.Forms.Panel panelNewLayoutForLoan;
    private EllieMae.Encompass.Forms.Panel panelNewLayoutForSetting;
    private EllieMae.Encompass.Forms.CheckBox chkDefaultTQL;
    private CategoryBox catBoxAll;
    private const string ShowFewerDesigneesText = "Show Fewer Designees";
    private const string ShowMoreDesigneesText = "Show More Designees";
    private EllieMae.Encompass.Forms.Panel panel8821;
    private EllieMae.Encompass.Forms.Panel panel8821Names;
    private EllieMae.Encompass.Forms.Panel panel8821BorNames;
    private EllieMae.Encompass.Forms.Panel panel8821OtherNames;
    private EllieMae.Encompass.Forms.Panel panel8821Designee1;
    private EllieMae.Encompass.Forms.Panel panel8821Designee2;
    private EllieMae.Encompass.Forms.Panel panel8821DesigneeAdditions;
    private EllieMae.Encompass.Forms.Panel panel8821TaxInformation;
    private EllieMae.Encompass.Forms.Panel panel8821LastPanel;
    private EllieMae.Encompass.Forms.Hyperlink hyperlinkShowMore;
    private DropdownBox cboTaxFormFor;
    private DropdownBox dd8821AuthorizedSigner;
    private string[] iconControlIDs = new string[10]
    {
      "Rolodex1",
      "Rolodex3",
      "Calendar1",
      "Calendar2",
      "Calendar3",
      "Calendar4",
      "Calendar9",
      "Calendar10",
      "Calendar11",
      "Calendar12"
    };
    private List<RuntimeControl> iconControls;

    public TAX4506TInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public TAX4506TInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public TAX4506TInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public TAX4506TInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public TAX4506TInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, owner, input, htmldoc, form, property)
    {
    }

    protected override void InitHeader()
    {
      this.header = "IR";
      this.IdId = "35";
      this.nameId = "02";
      this.title = "Request for Transcript of Tax";
    }

    public string CurrentIndex => this.ind;

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
        if (this.grpBoxHeader == null)
          this.grpBoxHeader = this.currentForm.FindControl("grpBoxHeader") as EllieMae.Encompass.Forms.GroupBox;
        if (this.oldPanel == null)
          this.oldPanel = this.currentForm.FindControl("PanelOldLayout") as EllieMae.Encompass.Forms.Panel;
        if (this.pnlNew == null)
        {
          this.pnlNew = this.currentForm.FindControl("PanelNewLayout") as EllieMae.Encompass.Forms.Panel;
          this.pnlNew.Left = this.oldPanel.Left;
          this.pnlNew.Top = this.grpBoxHeader.Top + this.grpBoxHeader.Size.Height;
        }
        if (this.pnlNewLocation == null)
          this.pnlNewLocation = this.currentForm.FindControl("PanelSecion1a") as EllieMae.Encompass.Forms.Panel;
        if (this.pnlLastSection == null)
          this.pnlLastSection = this.currentForm.FindControl("PanelLastSection") as EllieMae.Encompass.Forms.Panel;
        if (this.pnlFrm == null)
          this.pnlFrm = this.currentForm.FindControl("pnlForm") as EllieMae.Encompass.Forms.Panel;
        if (this.formLabel == null)
          this.formLabel = this.currentForm.FindControl("Label59") as EllieMae.Encompass.Forms.Label;
        if (this.panelOldLayoutForLoan == null)
          this.panelOldLayoutForLoan = this.currentForm.FindControl("panelOldLayoutForLoan") as EllieMae.Encompass.Forms.Panel;
        if (this.panelOldLayoutForSetting == null)
          this.panelOldLayoutForSetting = this.currentForm.FindControl("panelOldLayoutForSetting") as EllieMae.Encompass.Forms.Panel;
        if (this.panelNewLayoutForLoan == null)
          this.panelNewLayoutForLoan = this.currentForm.FindControl("panelNewLayoutForLoan") as EllieMae.Encompass.Forms.Panel;
        if (this.panelNewLayoutForSetting == null)
          this.panelNewLayoutForSetting = this.currentForm.FindControl("panelNewLayoutForSetting") as EllieMae.Encompass.Forms.Panel;
        if (this.chkDefaultTQL == null)
          this.chkDefaultTQL = this.currentForm.FindControl("chkDefaultTQL") as EllieMae.Encompass.Forms.CheckBox;
        if (this.catBoxAll == null)
          this.catBoxAll = this.currentForm.FindControl("catBoxAll") as CategoryBox;
        if (this.inputData is IRS4506TFields)
        {
          this.iconControls = new List<RuntimeControl>();
          for (int index = 0; index < this.iconControlIDs.Length; ++index)
            this.iconControls.Add((RuntimeControl) this.currentForm.FindControl(this.iconControlIDs[index]));
        }
        this.panel8821 = this.find8821Panel("Panel8821");
        this.panel8821BorNames = this.find8821Panel("Panel8821BorNames");
        this.panel8821Names = this.find8821Panel("Panel8821Names");
        this.panel8821Designee1 = this.find8821Panel("Panel8821Designee1");
        this.panel8821Designee2 = this.find8821Panel("Panel8821Designee2");
        this.panel8821DesigneeAdditions = this.find8821Panel("Panel8821DesigneeAdditions");
        this.panel8821TaxInformation = this.find8821Panel("Panel8821TaxInformation");
        this.panel8821LastPanel = this.find8821Panel("Panel8821LastPanel");
        this.panel8821OtherNames = this.find8821Panel("Panel8821OtherNames");
        this.hyperlinkShowMore = (EllieMae.Encompass.Forms.Hyperlink) this.currentForm.FindControl("HyperlinkShowMore");
        this.cboTaxFormFor = (DropdownBox) this.currentForm.FindControl("cboTaxFormFor");
        this.dd8821AuthorizedSigner = (DropdownBox) this.currentForm.FindControl("dd8821AuthorizedSigner");
        if (this.dd8821AuthorizedSigner != null)
        {
          DropdownOption dropdownOption = new DropdownOption("Both", "Both");
          if (this.dd8821AuthorizedSigner.Options.IndexOf(dropdownOption) >= 0)
            this.dd8821AuthorizedSigner.Options.Remove(dropdownOption);
        }
        if (this.hyperlinkShowMore != null)
        {
          if (!string.IsNullOrEmpty(this.GetFieldValue(this.header + this.ind + "A027")))
            this.hyperlinkShowMore.Text = "Show Fewer Designees";
          this.hyperlinkShowMore.Click += new EventHandler(this.hyperlinkShowMore_Click);
        }
        if (this.panel8821OtherNames == null)
          return;
        this.panel8821OtherNames.Position = new Point(this.panel8821BorNames.Position.X, this.panel8821BorNames.Position.Y);
      }
      catch (Exception ex)
      {
      }
    }

    private EllieMae.Encompass.Forms.Panel find8821Panel(string controlID)
    {
      EllieMae.Encompass.Forms.Panel control = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl(controlID);
      if (control != null)
        control.BorderWidth = 0;
      return control;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (this.inputData is IRS4506TFields && !((IRS4506TFields) this.inputData).InEditMode)
        return ControlState.Disabled;
      if (id.Length == 6)
      {
        if (id.EndsWith("57") && id.StartsWith(this.header) && this.GetFieldValue(this.header + this.ind + "01") == "Other" || id.EndsWith("58") && id.StartsWith(this.header) && this.GetFieldValue(this.header + this.ind + "01") == "Other")
          controlState = ControlState.Disabled;
        else if ((id.EndsWith("65") || id.EndsWith("66")) && id.StartsWith(this.header) && this.GetFieldValue("IR" + this.ind + "01") != "Other")
          controlState = ControlState.Disabled;
        if (id.EndsWith("93"))
        {
          this.cboTaxFormFor.Options.Clear();
          this.cboTaxFormFor.Options.Add(new DropdownOption("", ""));
          this.cboTaxFormFor.Options.Add(new DropdownOption("Borrower", "Borrower"));
          this.cboTaxFormFor.Options.Add(new DropdownOption("Co-Borrower", "CoBorrower"));
          if (this.GetFieldValue(this.header + this.ind + "93") != "8821")
            this.cboTaxFormFor.Options.Add(new DropdownOption("Both", "Both"));
          this.cboTaxFormFor.Options.Add(new DropdownOption("Other", "Other"));
          this.cboTaxFormFor.Refresh();
        }
      }
      return controlState;
    }

    private void hyperlinkShowMore_Click(object sender, EventArgs e)
    {
      if (this.hyperlinkShowMore.Text == "Show More Designees")
        this.hyperlinkShowMore.Text = "Show Fewer Designees";
      else
        this.hyperlinkShowMore.Text = "Show More Designees";
      this.UpdateContents();
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      this.pnlNew.Visible = this.oldPanel.Visible = this.panel8821.Visible = true;
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.GetFieldValue(this.header + this.ind + "93") == "8821")
      {
        this.pnlNew.Visible = this.oldPanel.Visible = this.pnlLastSection.Visible = false;
        this.panel8821.Visible = true;
        this.panel8821OtherNames.Visible = this.GetFieldValue(this.header + this.ind + "01") == "Other";
        this.panel8821BorNames.Visible = !this.panel8821OtherNames.Visible;
        Point position;
        if (this.inputData != null && this.inputData is IRS4506TFields)
        {
          this.panel8821Names.Visible = this.grpBoxHeader.Visible = false;
          EllieMae.Encompass.Forms.Panel panel8821 = this.panel8821;
          int x = this.grpBoxHeader.Position.X;
          position = this.grpBoxHeader.Position;
          int y = position.Y;
          Point point = new Point(x, y);
          panel8821.Position = point;
        }
        else
        {
          this.panel8821Names.Visible = this.grpBoxHeader.Visible = true;
          EllieMae.Encompass.Forms.Panel panel8821 = this.panel8821;
          int x = this.oldPanel.Position.X;
          position = this.oldPanel.Position;
          int y = position.Y;
          Point point = new Point(x, y);
          panel8821.Position = point;
        }
        List<EllieMae.Encompass.Forms.Panel> panelList = new List<EllieMae.Encompass.Forms.Panel>();
        if (this.panel8821Names.Visible)
          panelList.Add(this.panel8821Names);
        panelList.Add(this.panel8821Designee1);
        panelList.Add(this.panel8821Designee2);
        if (this.hyperlinkShowMore.Text == "Show Fewer Designees")
        {
          this.panel8821DesigneeAdditions.Visible = true;
          panelList.Add(this.panel8821DesigneeAdditions);
        }
        else
          this.panel8821DesigneeAdditions.Visible = false;
        panelList.Add(this.panel8821TaxInformation);
        panelList.Add(this.panel8821LastPanel);
        Size size1;
        for (int index = 0; index < panelList.Count; ++index)
        {
          if (index == 0)
          {
            panelList[index].Position = new Point(0, 0);
          }
          else
          {
            EllieMae.Encompass.Forms.Panel panel = panelList[index];
            position = panelList[index - 1].Position;
            int y = position.Y;
            size1 = panelList[index - 1].Size;
            int height = size1.Height;
            Point point = new Point(0, y + height - 2);
            panel.Position = point;
          }
        }
        EllieMae.Encompass.Forms.Panel panel8821_1 = this.panel8821;
        size1 = this.pnlFrm.Size;
        int width1 = size1.Width - 2;
        position = this.panel8821LastPanel.Position;
        int y1 = position.Y;
        size1 = this.panel8821LastPanel.Size;
        int height1 = size1.Height;
        int height2 = y1 + height1;
        Size size2 = new Size(width1, height2);
        panel8821_1.Size = size2;
        CategoryBox catBoxAll = this.catBoxAll;
        size1 = this.catBoxAll.Size;
        int width2 = size1.Width;
        size1 = this.panel8821.Size;
        int height3 = size1.Height;
        size1 = this.grpBoxHeader.Size;
        int height4 = size1.Height;
        int height5 = height3 + height4 + (!(this.inputData is IRS4506TFields) ? 30 : 0);
        Size size3 = new Size(width2, height5);
        catBoxAll.Size = size3;
        EllieMae.Encompass.Forms.Panel pnlFrm = this.pnlFrm;
        size1 = this.pnlFrm.Size;
        int width3 = size1.Width;
        size1 = this.catBoxAll.Size;
        int height6 = size1.Height + 2;
        Size size4 = new Size(width3, height6);
        pnlFrm.Size = size4;
        EllieMae.Encompass.Forms.Label formLabel = this.formLabel;
        position = this.formLabel.Position;
        int x1 = position.X;
        size1 = this.pnlFrm.Size;
        int y2 = size1.Height + 4;
        Point point1 = new Point(x1, y2);
        formLabel.Position = point1;
      }
      else
      {
        if (this.panel8821 != null)
          this.panel8821.Visible = false;
        if (this.pnlLastSection != null)
          this.pnlLastSection.Visible = true;
        if (this.GetFieldValue(this.header + this.ind + "01") == "Other")
        {
          this.label1aFName.Text = "Name";
          this.label1aLName.Visible = false;
          this.txt1aLName.Visible = false;
          this.label2aFName.Text = "Name";
          this.label2aLName.Visible = false;
          this.txt2aLName.Visible = false;
          if (this.GetFieldValue(this.header + this.ind + "93") == "4506-COct2022")
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
        if (this.inputData != null && this.inputData is IRS4506TFields)
        {
          if (this.iconControls != null)
          {
            IRS4506TFields inputData = (IRS4506TFields) this.inputData;
            for (int index = 0; index < this.iconControls.Count; ++index)
              this.iconControls[index].Enabled = inputData.InEditMode;
          }
          this.chkDefaultTQL.Visible = false;
          this.grpBoxHeader.Visible = false;
          this.panelOldLayoutForLoan.Visible = false;
          this.panelNewLayoutForLoan.Visible = false;
          this.panelOldLayoutForSetting.Top = 0;
          this.panelNewLayoutForSetting.Top = 0;
          this.pnlLastSection.Visible = false;
          this.oldPanel.Top = this.pnlNew.Top = 0;
          if (!this.panel8821.Visible)
          {
            this.pnlNew.Visible = this.GetFieldValue(this.header + this.ind + "93") == "4506-COct2022";
            this.oldPanel.Visible = !this.pnlNew.Visible;
          }
          this.oldPanel.Size = new Size(this.oldPanel.Size.Width, this.panelOldLayoutForSetting.Size.Height);
          EllieMae.Encompass.Forms.Panel pnlNew = this.pnlNew;
          Size size5 = this.pnlNew.Size;
          int width4 = size5.Width;
          size5 = this.panelNewLayoutForSetting.Size;
          int height7 = size5.Height;
          Size size6 = new Size(width4, height7);
          pnlNew.Size = size6;
          CategoryBox catBoxAll = this.catBoxAll;
          int width5 = this.catBoxAll.Size.Width;
          Size size7;
          int height8;
          if (!this.pnlNew.Visible)
          {
            size7 = this.oldPanel.Size;
            height8 = size7.Height + 30;
          }
          else
          {
            size7 = this.pnlNew.Size;
            height8 = size7.Height + 30;
          }
          Size size8 = new Size(width5, height8);
          catBoxAll.Size = size8;
          EllieMae.Encompass.Forms.Panel pnlFrm = this.pnlFrm;
          size7 = this.pnlFrm.Size;
          int width6 = size7.Width;
          int height9;
          if (!this.pnlNew.Visible)
          {
            size7 = this.oldPanel.Size;
            height9 = size7.Height + 50;
          }
          else
          {
            size7 = this.pnlNew.Size;
            height9 = size7.Height + 50;
          }
          Size size9 = new Size(width6, height9);
          pnlFrm.Size = size9;
          EllieMae.Encompass.Forms.Label formLabel = this.formLabel;
          size7 = this.pnlFrm.Size;
          int num = size7.Height + 10;
          formLabel.Top = num;
        }
        else
        {
          this.oldPanel.Visible = this.GetFieldValue(this.header + this.ind + "93") != "4506-COct2022";
          this.pnlNew.Visible = !this.oldPanel.Visible;
          this.pnlLastSection.Top = (this.pnlNew.Visible ? this.pnlNew.Top + this.pnlNew.Size.Height : this.oldPanel.Top + this.oldPanel.Size.Height) + 30;
          CategoryBox catBoxAll = this.catBoxAll;
          int width7 = this.catBoxAll.Size.Width;
          Size size10;
          int height10;
          if (!this.pnlNew.Visible)
          {
            size10 = this.oldPanel.Size;
            height10 = size10.Height;
          }
          else
          {
            size10 = this.pnlNew.Size;
            height10 = size10.Height;
          }
          size10 = this.pnlLastSection.Size;
          int height11 = size10.Height;
          int num1 = height10 + height11;
          size10 = this.grpBoxHeader.Size;
          int height12 = size10.Height;
          int height13 = num1 + height12 + 30;
          Size size11 = new Size(width7, height13);
          catBoxAll.Size = size11;
          EllieMae.Encompass.Forms.Panel pnlFrm = this.pnlFrm;
          size10 = this.pnlFrm.Size;
          int width8 = size10.Width;
          size10 = this.catBoxAll.Size;
          int height14 = size10.Height + 2;
          Size size12 = new Size(width8, height14);
          pnlFrm.Size = size12;
          EllieMae.Encompass.Forms.Label formLabel = this.formLabel;
          size10 = this.pnlFrm.Size;
          int num2 = size10.Height + 4;
          formLabel.Top = num2;
        }
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id.Length == 6)
      {
        if (id.EndsWith("25"))
          id = this.header + this.ind + "25";
        else if (id.EndsWith("26"))
          id = this.header + this.ind + "26";
        else if (id.EndsWith("29"))
          id = this.header + this.ind + "29";
        else if (id.EndsWith("30"))
          id = this.header + this.ind + "30";
        else if (id.EndsWith("01") && val == "Other" && this.GetFieldValue(this.header + this.ind + "93") != "8821")
        {
          base.UpdateFieldValue(this.header + this.ind + "57", "Y");
          base.UpdateFieldValue(this.header + this.ind + "58", "Y");
        }
      }
      base.UpdateFieldValue(id, val);
      if (id.Length == 6 && id.EndsWith("93"))
      {
        this.cboTaxFormFor.Options.Clear();
        this.cboTaxFormFor.Options.Add(new DropdownOption("", ""));
        this.cboTaxFormFor.Options.Add(new DropdownOption("Borrower", "Borrower"));
        this.cboTaxFormFor.Options.Add(new DropdownOption("Co-Borrower", "CoBorrower"));
        if (this.GetFieldValue(id) != "8821")
          this.cboTaxFormFor.Options.Add(new DropdownOption("Both", "Both"));
        this.cboTaxFormFor.Options.Add(new DropdownOption("Other", "Other"));
        this.cboTaxFormFor.Refresh();
      }
      if (id.Length == 6)
      {
        if (id.EndsWith("59") && this.GetFieldValue(id) == "Y" && this.GetFieldValue(this.header + this.ind + "93") != "8821" || id.EndsWith("93") && this.GetFieldValue(id) != "8821")
        {
          this.refreshColumnValues(this.header + "0024", this.GetFieldValue(this.header + this.ind + "24"));
          this.refreshColumnValues(this.header + "0025", this.GetFieldValue(this.header + this.ind + "25"));
          this.refreshColumnValues(this.header + "0046", this.GetFieldValue(this.header + this.ind + "46"));
        }
        else
        {
          if (!id.EndsWith("93") || !(this.GetFieldValue(id) == "8821"))
            return;
          this.refreshColumnValues(this.header + "00A067", this.GetFieldValue(this.header + this.ind + "A067"));
          this.refreshColumnValues(this.header + "00A068", this.GetFieldValue(this.header + this.ind + "A068"));
          this.refreshColumnValues(this.header + "0001", this.GetFieldValue(this.header + this.ind + "01"));
        }
      }
      else
      {
        if (id.Length != 8 || !id.EndsWith("066") && !id.EndsWith("070") && !id.EndsWith("074"))
          return;
        this.refreshColumnValues(this.header + "00" + id.Substring(4), this.GetFieldValue(this.header + this.ind + id.Substring(4)));
      }
    }

    public void OrderTaxReturns(int recordIndex)
    {
      Session.Application.GetService<ILoanServices>().OrderTaxReturns(recordIndex);
    }

    public void CheckTaxReturnStatus(int recordIndex)
    {
      Session.Application.GetService<ILoanServices>().CheckTaxReturnStatus(recordIndex);
    }

    public override void ExecAction(string action)
    {
      if (!(action == "copybroInfo"))
        return;
      string fieldValue1 = this.GetFieldValue(this.header + this.ind + "01");
      string fieldValue2 = this.GetFieldValue(this.header + this.ind + "93");
      if (fieldValue1 == "Both" || fieldValue1 == "Borrower")
      {
        if (fieldValue2 == "4506-COct2022" || fieldValue2 == "8821")
        {
          this.UpdateFieldValue(this.header + this.ind + "02", this.GetFieldValue("4000"));
          if (!string.IsNullOrEmpty(this.GetFieldValue("4001")))
            this.UpdateFieldValue(this.header + this.ind + "68", this.GetFieldValue("4001").Substring(0, 1));
          else
            this.UpdateFieldValue(this.header + this.ind + "68", "");
          this.UpdateFieldValue(this.header + this.ind + "03", this.GetFieldValue("4002") + (!string.IsNullOrEmpty(this.GetFieldValue("4003")) ? " " + this.GetFieldValue("4003") : ""));
        }
        else
        {
          this.UpdateFieldValue(this.header + this.ind + "02", this.GetFieldValue("36"));
          this.UpdateFieldValue(this.header + this.ind + "03", this.GetFieldValue("37"));
        }
        this.UpdateFieldValue(this.header + this.ind + "57", "");
        this.UpdateFieldValue(this.header + this.ind + "04", this.GetFieldValue("65"));
        this.UpdateFieldValue(this.header + this.ind + "39", this.GetFieldValue("36"));
        this.UpdateFieldValue(this.header + this.ind + "40", this.GetFieldValue("37"));
        this.UpdateFieldValue(this.header + this.ind + "35", this.GetFieldValue("FR0104"));
        this.UpdateFieldValue(this.header + this.ind + "36", this.GetFieldValue("FR0106"));
        this.UpdateFieldValue(this.header + this.ind + "37", this.GetFieldValue("FR0107"));
        this.UpdateFieldValue(this.header + this.ind + "38", this.GetFieldValue("FR0108"));
        this.UpdateFieldValue(this.header + this.ind + "27", fieldValue2 != "8821" || string.IsNullOrEmpty(this.GetFieldValue("1490")) ? this.GetFieldValue("66") : this.GetFieldValue("1490"));
      }
      if (fieldValue1 == "CoBorrower")
      {
        if (fieldValue2 == "4506-COct2022" || fieldValue2 == "8821")
        {
          this.UpdateFieldValue(this.header + this.ind + "02", this.GetFieldValue("4004"));
          if (!string.IsNullOrEmpty(this.GetFieldValue("4005")))
            this.UpdateFieldValue(this.header + this.ind + "68", this.GetFieldValue("4005").Substring(0, 1));
          else
            this.UpdateFieldValue(this.header + this.ind + "68", "");
          this.UpdateFieldValue(this.header + this.ind + "03", this.GetFieldValue("4006") + (!string.IsNullOrEmpty(this.GetFieldValue("4007")) ? " " + this.GetFieldValue("4007") : ""));
        }
        else
        {
          this.UpdateFieldValue(this.header + this.ind + "02", this.GetFieldValue("68"));
          this.UpdateFieldValue(this.header + this.ind + "03", this.GetFieldValue("69"));
        }
        this.UpdateFieldValue(this.header + this.ind + "57", "");
        this.UpdateFieldValue(this.header + this.ind + "04", this.GetFieldValue("97"));
        this.UpdateFieldValue(this.header + this.ind + "39", this.GetFieldValue("68"));
        this.UpdateFieldValue(this.header + this.ind + "40", this.GetFieldValue("69"));
        this.UpdateFieldValue(this.header + this.ind + "35", this.GetFieldValue("FR0204"));
        this.UpdateFieldValue(this.header + this.ind + "36", this.GetFieldValue("FR0206"));
        this.UpdateFieldValue(this.header + this.ind + "37", this.GetFieldValue("FR0207"));
        this.UpdateFieldValue(this.header + this.ind + "38", this.GetFieldValue("FR0208"));
        this.UpdateFieldValue(this.header + this.ind + "27", fieldValue2 != "8821" || string.IsNullOrEmpty(this.GetFieldValue("1480")) ? this.GetFieldValue("98") : this.GetFieldValue("1480"));
      }
      if (fieldValue1 == "Both")
      {
        if (this.GetFieldValue(this.header + this.ind + "93") == "4506-COct2022")
        {
          this.UpdateFieldValue(this.header + this.ind + "06", this.GetFieldValue("4004"));
          if (!string.IsNullOrEmpty(this.GetFieldValue("4005")))
            this.UpdateFieldValue(this.header + this.ind + "69", this.GetFieldValue("4005").Substring(0, 1));
          else
            this.UpdateFieldValue(this.header + this.ind + "69", "");
          this.UpdateFieldValue(this.header + this.ind + "07", this.GetFieldValue("4006") + (!string.IsNullOrEmpty(this.GetFieldValue("4007")) ? " " + this.GetFieldValue("4007") : ""));
        }
        else
        {
          this.UpdateFieldValue(this.header + this.ind + "06", this.GetFieldValue("68"));
          this.UpdateFieldValue(this.header + this.ind + "07", this.GetFieldValue("69"));
        }
        this.UpdateFieldValue(this.header + this.ind + "58", "");
        this.UpdateFieldValue(this.header + this.ind + "05", this.GetFieldValue("97"));
      }
      else
      {
        this.UpdateFieldValue(this.header + this.ind + "06", "");
        this.UpdateFieldValue(this.header + this.ind + "07", "");
        this.UpdateFieldValue(this.header + this.ind + "58", "");
        this.UpdateFieldValue(this.header + this.ind + "05", "");
        this.UpdateFieldValue(this.header + this.ind + "69", "");
      }
      this.UpdateContents();
      this.SetFieldFocus("l_IRS4506X2");
    }

    protected override void SaveBeforeSwitch() => this.FlushOutCurrentField();

    public new event VerifSummaryChangedEventHandler VerifSummaryChanged;

    public new void SummaryChanged(string fieldId, string newValue)
    {
      if (this.VerifSummaryChanged == null)
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) newValue));
    }

    public override void onCalendarPopupClosed(object sender, FormClosedEventArgs e)
    {
      CalendarPopup calendarPopup = (CalendarPopup) sender;
      if (calendarPopup.DialogResult != DialogResult.OK)
        return;
      EllieMae.Encompass.Forms.Calendar tag = calendarPopup.Tag as EllieMae.Encompass.Forms.Calendar;
      if (tag.DateField != null && !this.setFieldValue(this.getActualFieldID(tag.DateField.FieldID), tag.FieldSource, calendarPopup.SelectedDate.ToString("MM/dd/yyyy")))
        return;
      if (this.executeDateSelectedEvent((RuntimeControl) tag, calendarPopup.SelectedDate))
        this.RefreshContents();
      if (this.VerifSummaryChanged == null)
        return;
      this.refreshColumnValues(tag.DateField.FieldID, calendarPopup.SelectedDate.ToString("MM/dd/yyyy"));
    }

    private string getActualFieldID(string id)
    {
      if (!id.StartsWith(this.header))
        return id;
      string str = string.Empty;
      if (id.Length == 6)
        str = id.Substring(4);
      else if (id.Length == 7)
        str = id.Substring(5);
      return this.header + this.ind + str;
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onkeyup(pEvtObj);
      if (this.VerifSummaryChanged == null)
        return;
      FieldControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as FieldControl;
      this.refreshColumnValues(controlForElement.Field.FieldID, controlForElement.Value);
    }

    public override void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onfocusout(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      this.refreshColumnValues(controlForElement.Field.FieldID, controlForElement.Value);
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      bool flag = base.onclick(pEvtObj);
      if (this.VerifSummaryChanged == null)
        return false;
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return flag;
      this.refreshColumnValues(controlForElement.Field.FieldID, controlForElement.Value);
      return flag;
    }

    private void refreshColumnValues(string fieldId, string val)
    {
      if (fieldId.Length == 6 && (fieldId.EndsWith("01") || fieldId.EndsWith("24") || fieldId.EndsWith("25") || fieldId.EndsWith("26") || fieldId.EndsWith("29") || fieldId.EndsWith("30") || fieldId.EndsWith("46") || fieldId.EndsWith("47") || fieldId.EndsWith("48") || fieldId.EndsWith("50") || fieldId.EndsWith("93")))
      {
        this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) val));
      }
      else
      {
        if (fieldId.Length != 8 || !fieldId.EndsWith("A066") && !fieldId.EndsWith("A070") && !fieldId.EndsWith("A074") && !fieldId.EndsWith("A068") && !fieldId.EndsWith("A072") && !fieldId.EndsWith("A076"))
          return;
        this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) val));
      }
    }

    public override void AddToEFolder()
    {
      if (!new eFolderAccessRights(this.session.LoanDataMgr).CanAddDocuments)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "You do not have rights to add documents to the eFolder.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        string field = this.loan.GetField(this.header + this.ind + "63");
        VerifLog[] allRecordsOfType = (VerifLog[]) this.loan.GetLogList().GetAllRecordsOfType(typeof (VerifLog));
        if (allRecordsOfType != null)
        {
          for (int index = 0; index < allRecordsOfType.Length; ++index)
          {
            if (allRecordsOfType[index].Id == field)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) null, "The document tracking list already contains this verification.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
          }
        }
        if (!this.UpdateDocTrackingForVerifs(this.header + this.ind + "97", true))
          return;
        int num3 = (int) Utils.Dialog((IWin32Window) null, "The verification has been added to document tracking list successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }
  }
}
