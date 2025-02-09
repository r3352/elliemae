// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CLOSINGDISCLOSUREPAGE2InputHandler
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
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CLOSINGDISCLOSUREPAGE2InputHandler : InputHandlerBase
  {
    private string itemizationXML = string.Empty;
    private Hyperlink hyperlinkOriginationCharges;
    private EllieMae.Encompass.Forms.Panel panelBottomOriginationCharges;
    private EllieMae.Encompass.Forms.Panel paneltotalOriginationCharges;
    private Hyperlink SerBorrDidNotShopForShowMoreLines;
    private EllieMae.Encompass.Forms.Panel SerBorrDidNotShopForChargesBottom;
    private EllieMae.Encompass.Forms.Panel SerBorrDidNotShopForChargesTotal;
    private Hyperlink SerBorrDidShopForShowMoreLines;
    private EllieMae.Encompass.Forms.Panel SerBorrDidShopForChargesBottom;
    private EllieMae.Encompass.Forms.Panel SerBorrDidShopForChargesTotal;
    private Hyperlink TaxesAndOtherFeesShowMoreLines;
    private EllieMae.Encompass.Forms.Panel TaxesAndGovFeesBottom;
    private EllieMae.Encompass.Forms.Panel TaxesAndGovFeesTotal;
    private Hyperlink PrepaidsShowMoreLines;
    private EllieMae.Encompass.Forms.Panel PrepaidsBottom;
    private EllieMae.Encompass.Forms.Panel PrepaidsTotal;
    private Hyperlink OtherShowMoreLines;
    private EllieMae.Encompass.Forms.Panel OtherBottom;
    private EllieMae.Encompass.Forms.Panel OtherTotal;
    private Hyperlink InitialEscrowShowMoreLines;
    private EllieMae.Encompass.Forms.Panel InitialEscrowBottom;
    private EllieMae.Encompass.Forms.Panel InitialEscrowTotal;
    private EllieMae.Encompass.Forms.Panel InitialEscrowAggre;
    private EllieMae.Encompass.Forms.Panel TolerancePanel;
    private List<EllieMae.Encompass.Forms.Label> monthlyBiweeklyLabels;

    public CLOSINGDISCLOSUREPAGE2InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE2InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE2InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE2InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE2InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmlDoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmlDoc, form, property)
    {
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      if (this.inputData is LoanData)
      {
        if (this.itemizationXML == "")
          return;
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(this.itemizationXML);
        dictionary = new UCDXmlParser(doc).ParseXml(false);
      }
      foreach (EllieMae.Encompass.Forms.Control allControl in this.currentForm.GetAllControls())
      {
        if (allControl is FieldControl)
        {
          FieldControl fieldControl = (FieldControl) allControl;
          if (fieldControl.Field.FieldID == "")
          {
            if (this.inputData is LoanData)
            {
              if (dictionary.ContainsKey(allControl.ControlID))
              {
                string str = dictionary[allControl.ControlID];
                fieldControl.BindTo(str);
              }
            }
            else
            {
              string field = this.inputData.GetField(allControl.ControlID);
              fieldControl.BindTo(field);
            }
          }
          else
          {
            string field = this.inputData.GetField(fieldControl.Field.FieldID);
            fieldControl.BindTo(field);
          }
          fieldControl.Enabled = false;
          if (allControl is EllieMae.Encompass.Forms.TextBox && fieldControl.Field.FieldID == "" && !allControl.ControlID.Contains("DisableHover"))
            fieldControl.HoverText = allControl.ControlID;
        }
      }
      if (!(this.inputData is LoanData))
        return;
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
    }

    internal override void CreateControls()
    {
      this.hyperlinkOriginationCharges = (Hyperlink) this.currentForm.FindControl("OriginationChargesShowMoreLines");
      this.hyperlinkOriginationCharges.Click += new EventHandler(this.hyperlinkOriginationCharges_Click);
      this.panelBottomOriginationCharges = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelOriginationChargesBottom");
      this.paneltotalOriginationCharges = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelOriginationChargesTotal");
      this.showLessOriginationCharges();
      this.SerBorrDidNotShopForShowMoreLines = (Hyperlink) this.currentForm.FindControl("SerBorrDidNotShopForShowMoreLines");
      this.SerBorrDidNotShopForShowMoreLines.Click += new EventHandler(this.SerBorrDidNotShopForShowMoreLines_Click);
      this.SerBorrDidNotShopForChargesBottom = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("SerBorrDidNotShopForChargesBottom");
      this.SerBorrDidNotShopForChargesTotal = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("SerBorrDidNotShopForChargesTotal");
      this.showLessServiceDidNotShopFor();
      this.SerBorrDidShopForShowMoreLines = (Hyperlink) this.currentForm.FindControl("SerBorrDidShopForShowMoreLines");
      this.SerBorrDidShopForShowMoreLines.Click += new EventHandler(this.SerBorrDidShopForShowMoreLines_Click);
      this.SerBorrDidShopForChargesBottom = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("SerBorrDidShopForChargesBottom");
      this.SerBorrDidShopForChargesTotal = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("SerBorrDidShopForChargesTotal");
      this.showLessServiceDidShopFor();
      this.TaxesAndOtherFeesShowMoreLines = (Hyperlink) this.currentForm.FindControl("TaxesAndOtherFeesShowMoreLines");
      this.TaxesAndOtherFeesShowMoreLines.Click += new EventHandler(this.TaxesAndOtherFeesShowMoreLines_Click);
      this.TaxesAndGovFeesBottom = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("TaxesAndGovFeesBottom");
      this.TaxesAndGovFeesTotal = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("TaxesAndGovFeesTotal");
      this.showLessTaxesAndOtherFees();
      this.PrepaidsShowMoreLines = (Hyperlink) this.currentForm.FindControl("PrepaidsShowMoreLines");
      this.PrepaidsShowMoreLines.Click += new EventHandler(this.PrepaidsShowMoreLines_Click);
      this.PrepaidsBottom = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PrepaidsBottom");
      this.PrepaidsTotal = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PrepaidsTotal");
      this.showLessPrepaid();
      this.OtherShowMoreLines = (Hyperlink) this.currentForm.FindControl("OtherShowMoreLines");
      this.OtherShowMoreLines.Click += new EventHandler(this.OtherShowMoreLines_Click);
      this.OtherBottom = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("OtherBottom");
      this.OtherTotal = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("OtherTotal");
      this.showLessOther();
      this.showTolerancePanel();
      this.InitialEscrowShowMoreLines = (Hyperlink) this.currentForm.FindControl("InitialEscrowShowMoreLines");
      this.InitialEscrowShowMoreLines.Click += new EventHandler(this.InitialEscrowShowMoreLines_Click);
      this.InitialEscrowBottom = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("InitialEscrowBottom");
      this.InitialEscrowTotal = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("InitialEscrowTotal");
      this.InitialEscrowAggre = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("InitialEscrowAggre");
      this.showLessInitialEscrow();
      this.monthlyBiweeklyLabels = new List<EllieMae.Encompass.Forms.Label>();
      for (int index = 1; index <= 13; ++index)
        this.monthlyBiweeklyLabels.Add((EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelG" + (object) index));
      if (this.loan == null)
        return;
      if (this.loan.IsTemplate)
        return;
      try
      {
        if (!(this.inputData is LoanData))
          return;
        this.itemizationXML = this.loan.Calculator.GetUCD(false, true);
      }
      catch (Exception ex)
      {
      }
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.setMonthlyBiweeklyLabel(this.inputData.GetField("423") == "Biweekly");
    }

    private void setMonthlyBiweeklyLabel(bool isBiweekly)
    {
      for (int index = 0; index <= 12; ++index)
        this.monthlyBiweeklyLabels[index].Text = isBiweekly ? "bwks" : "mths";
    }

    private void InitialEscrowShowMoreLines_Click(object sender, EventArgs e)
    {
      if (this.InitialEscrowShowMoreLines.Text == "Show More Lines")
        this.showMoreInitialEscrow();
      else
        this.showLessInitialEscrow();
    }

    private void OtherShowMoreLines_Click(object sender, EventArgs e)
    {
      if (this.OtherShowMoreLines.Text == "Show More Lines")
        this.showMoreOther();
      else
        this.showLessOther();
    }

    private void PrepaidsShowMoreLines_Click(object sender, EventArgs e)
    {
      if (this.PrepaidsShowMoreLines.Text == "Show More Lines")
        this.ShowMorePrepaid();
      else
        this.showLessPrepaid();
    }

    private void TaxesAndOtherFeesShowMoreLines_Click(object sender, EventArgs e)
    {
      if (this.TaxesAndOtherFeesShowMoreLines.Text == "Show More Lines")
        this.ShowMoreTaxesAndOtherFees();
      else
        this.showLessTaxesAndOtherFees();
    }

    private void SerBorrDidShopForShowMoreLines_Click(object sender, EventArgs e)
    {
      if (this.SerBorrDidShopForShowMoreLines.Text == "Show More Lines")
        this.ShowMoreServiceDidShopFor();
      else
        this.showLessServiceDidShopFor();
    }

    private void SerBorrDidNotShopForShowMoreLines_Click(object sender, EventArgs e)
    {
      if (this.SerBorrDidNotShopForShowMoreLines.Text == "Show More Lines")
        this.ShowMoreServiceDidNotShopFor();
      else
        this.showLessServiceDidNotShopFor();
    }

    private void hyperlinkOriginationCharges_Click(object sender, EventArgs e)
    {
      if (this.hyperlinkOriginationCharges.Text == "Show More Lines")
        this.ShowMoreOriginationCharges();
      else
        this.showLessOriginationCharges();
    }

    private void showLessInitialEscrow()
    {
      this.InitialEscrowBottom.Visible = false;
      this.InitialEscrowAggre.Position = new Point(this.InitialEscrowAggre.Position.X, 204);
      this.InitialEscrowTotal.Position = new Point(this.InitialEscrowTotal.Position.X, 232);
      int maxValue = (int) sbyte.MaxValue;
      ((EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblEscrowAggre")).Text = "08";
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxG");
      EllieMae.Encompass.Forms.GroupBox groupBox = control1;
      Size size1 = control1.Size;
      int width1 = size1.Width;
      size1 = control1.Size;
      int height1 = size1.Height - maxValue;
      Size size2 = new Size(width1, height1);
      groupBox.Size = size2;
      this.InitialEscrowShowMoreLines.Text = "Show More Lines";
      foreach (string controlId in new List<string>()
      {
        "groupBoxH",
        "groupBoxI",
        "groupBoxJ"
      })
      {
        EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl(controlId);
        control2.Position = new Point(control2.Position.X, control2.Position.Y - maxValue);
      }
      EllieMae.Encompass.Forms.Panel control3 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control3;
      Size size3 = control3.Size;
      int width2 = size3.Width;
      size3 = control3.Size;
      int height2 = size3.Height - maxValue;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control4 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      control4.Position = new Point(control4.Position.X, control4.Position.Y - maxValue);
      CategoryBox control5 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox = control5;
      Size size5 = control5.Size;
      int width3 = size5.Width;
      size5 = control5.Size;
      int height3 = size5.Height - maxValue;
      Size size6 = new Size(width3, height3);
      categoryBox.Size = size6;
    }

    private void showMoreInitialEscrow()
    {
      this.InitialEscrowBottom.Visible = true;
      this.InitialEscrowBottom.Position = new Point(this.InitialEscrowBottom.Position.X, 207);
      this.InitialEscrowAggre.Position = new Point(this.InitialEscrowAggre.Position.X, 336);
      this.InitialEscrowTotal.Position = new Point(this.InitialEscrowTotal.Position.X, 359);
      this.InitialEscrowShowMoreLines.Text = "Show Less Lines";
      int maxValue = (int) sbyte.MaxValue;
      ((EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblEscrowAggre")).Text = "14";
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxG");
      EllieMae.Encompass.Forms.GroupBox groupBox = control1;
      Size size1 = control1.Size;
      int width1 = size1.Width;
      size1 = control1.Size;
      int height1 = size1.Height + maxValue;
      Size size2 = new Size(width1, height1);
      groupBox.Size = size2;
      foreach (string controlId in new List<string>()
      {
        "groupBoxH",
        "groupBoxI",
        "groupBoxJ"
      })
      {
        EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl(controlId);
        control2.Position = new Point(control2.Position.X, control2.Position.Y + maxValue);
      }
      EllieMae.Encompass.Forms.Panel control3 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control3;
      Size size3 = control3.Size;
      int width2 = size3.Width;
      size3 = control3.Size;
      int height2 = size3.Height + maxValue;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control4 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control4;
      Point position = control4.Position;
      int x = position.X;
      position = control4.Position;
      int y = position.Y + maxValue;
      Point point = new Point(x, y);
      label.Position = point;
      CategoryBox control5 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox = control5;
      Size size5 = control5.Size;
      int width3 = size5.Width;
      size5 = control5.Size;
      int height3 = size5.Height + maxValue;
      Size size6 = new Size(width3, height3);
      categoryBox.Size = size6;
    }

    private void showTolerancePanel()
    {
      int num = 65;
      this.TolerancePanel = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("tolerancePanel");
      if (this.inputData.GetField("CD3.X129") != string.Empty && this.inputData.GetField("CD3.X129") != "0.00")
      {
        this.TolerancePanel.Visible = true;
        EllieMae.Encompass.Forms.TextBox control1 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txtLenderCredit");
        EllieMae.Encompass.Forms.TextBox control2 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("toleranceTxt");
        if (Utils.ParseDouble((object) this.GetField("CD2.X2")) > 0.0)
        {
          control1.Position = new Point(87, 6);
          control2.Visible = !(control1.Visible = true);
        }
        else
          control2.Visible = !(control1.Visible = false);
      }
      else
      {
        this.TolerancePanel.Visible = false;
        EllieMae.Encompass.Forms.Label control3 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("totalBorrowerLbl");
        control3.Position = new Point(control3.Position.X, control3.Position.Y - num);
        EllieMae.Encompass.Forms.TextBox control4 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("totalBorrowerTxt");
        control4.Position = new Point(control4.Position.X, control4.Position.Y - num);
      }
    }

    private void showLessOther()
    {
      this.OtherBottom.Visible = false;
      this.OtherTotal.Position = new Point(this.OtherTotal.Position.X, 222);
      int num = 163;
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxH");
      EllieMae.Encompass.Forms.GroupBox groupBox = control1;
      Size size1 = control1.Size;
      int width1 = size1.Width;
      size1 = control1.Size;
      int height1 = size1.Height - num;
      Size size2 = new Size(width1, height1);
      groupBox.Size = size2;
      this.OtherShowMoreLines.Text = "Show More Lines";
      foreach (string controlId in new List<string>()
      {
        "groupBoxI",
        "groupBoxJ"
      })
      {
        EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl(controlId);
        control2.Position = new Point(control2.Position.X, control2.Position.Y - num);
      }
      EllieMae.Encompass.Forms.Panel control3 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control3;
      Size size3 = control3.Size;
      int width2 = size3.Width;
      size3 = control3.Size;
      int height2 = size3.Height - num;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control4 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control4;
      Point position = control4.Position;
      int x = position.X;
      position = control4.Position;
      int y = position.Y - num;
      Point point = new Point(x, y);
      label.Position = point;
      CategoryBox control5 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox = control5;
      Size size5 = control5.Size;
      int width3 = size5.Width;
      size5 = control5.Size;
      int height3 = size5.Height - num;
      Size size6 = new Size(width3, height3);
      categoryBox.Size = size6;
    }

    private void showMoreOther()
    {
      this.OtherBottom.Visible = true;
      this.OtherBottom.Position = new Point(this.OtherBottom.Position.X, 222);
      this.OtherTotal.Position = new Point(this.OtherTotal.Position.X, 385);
      this.OtherShowMoreLines.Text = "Show Less Lines";
      int num = 163;
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxH");
      EllieMae.Encompass.Forms.GroupBox groupBox = control1;
      Size size1 = control1.Size;
      int width1 = size1.Width;
      size1 = control1.Size;
      int height1 = size1.Height + num;
      Size size2 = new Size(width1, height1);
      groupBox.Size = size2;
      foreach (string controlId in new List<string>()
      {
        "groupBoxI",
        "groupBoxJ"
      })
      {
        EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl(controlId);
        control2.Position = new Point(control2.Position.X, control2.Position.Y + num);
      }
      EllieMae.Encompass.Forms.Panel control3 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control3;
      Size size3 = control3.Size;
      int width2 = size3.Width;
      size3 = control3.Size;
      int height2 = size3.Height + num;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control4 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control4;
      Point position = control4.Position;
      int x = position.X;
      position = control4.Position;
      int y = position.Y + num;
      Point point = new Point(x, y);
      label.Position = point;
      CategoryBox control5 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox = control5;
      Size size5 = control5.Size;
      int width3 = size5.Width;
      size5 = control5.Size;
      int height3 = size5.Height + num;
      Size size6 = new Size(width3, height3);
      categoryBox.Size = size6;
    }

    private void showLessPrepaid()
    {
      this.PrepaidsBottom.Visible = false;
      this.PrepaidsTotal.Position = new Point(this.PrepaidsTotal.Position.X, 264);
      int num = 401;
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxF");
      EllieMae.Encompass.Forms.GroupBox groupBox = control1;
      Size size1 = control1.Size;
      int width1 = size1.Width;
      size1 = control1.Size;
      int height1 = size1.Height - num;
      Size size2 = new Size(width1, height1);
      groupBox.Size = size2;
      this.PrepaidsShowMoreLines.Text = "Show More Lines";
      foreach (string controlId in new List<string>()
      {
        "groupBoxG",
        "groupBoxH",
        "groupBoxI",
        "groupBoxJ"
      })
      {
        EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl(controlId);
        control2.Position = new Point(control2.Position.X, control2.Position.Y - num);
      }
      EllieMae.Encompass.Forms.Panel control3 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control3;
      Size size3 = control3.Size;
      int width2 = size3.Width;
      size3 = control3.Size;
      int height2 = size3.Height - num;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control4 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control4;
      Point position = control4.Position;
      int x = position.X;
      position = control4.Position;
      int y = position.Y - num;
      Point point = new Point(x, y);
      label.Position = point;
      CategoryBox control5 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox = control5;
      Size size5 = control5.Size;
      int width3 = size5.Width;
      size5 = control5.Size;
      int height3 = size5.Height - num;
      Size size6 = new Size(width3, height3);
      categoryBox.Size = size6;
    }

    private void ShowMorePrepaid()
    {
      this.PrepaidsBottom.Visible = true;
      this.PrepaidsBottom.Position = new Point(this.PrepaidsBottom.Position.X, 264);
      this.PrepaidsTotal.Position = new Point(this.PrepaidsTotal.Position.X, 665);
      this.PrepaidsShowMoreLines.Text = "Show Less Lines";
      int num = 401;
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxF");
      EllieMae.Encompass.Forms.GroupBox groupBox = control1;
      Size size1 = control1.Size;
      int width1 = size1.Width;
      size1 = control1.Size;
      int height1 = size1.Height + num;
      Size size2 = new Size(width1, height1);
      groupBox.Size = size2;
      foreach (string controlId in new List<string>()
      {
        "groupBoxG",
        "groupBoxH",
        "groupBoxI",
        "groupBoxJ"
      })
      {
        EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl(controlId);
        control2.Position = new Point(control2.Position.X, control2.Position.Y + num);
      }
      EllieMae.Encompass.Forms.Panel control3 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control3;
      Size size3 = control3.Size;
      int width2 = size3.Width;
      size3 = control3.Size;
      int height2 = size3.Height + num;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control4 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control4;
      Point position = control4.Position;
      int x = position.X;
      position = control4.Position;
      int y = position.Y + num;
      Point point = new Point(x, y);
      label.Position = point;
      CategoryBox control5 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox = control5;
      Size size5 = control5.Size;
      int width3 = size5.Width;
      size5 = control5.Size;
      int height3 = size5.Height + num;
      Size size6 = new Size(width3, height3);
      categoryBox.Size = size6;
    }

    private void showLessTaxesAndOtherFees()
    {
      this.TaxesAndGovFeesBottom.Visible = false;
      this.TaxesAndGovFeesTotal.Position = new Point(this.SerBorrDidShopForChargesTotal.Position.X, 95);
      int num = 271;
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxE");
      EllieMae.Encompass.Forms.GroupBox groupBox = control1;
      Size size1 = control1.Size;
      int width1 = size1.Width;
      size1 = control1.Size;
      int height1 = size1.Height - num;
      Size size2 = new Size(width1, height1);
      groupBox.Size = size2;
      this.TaxesAndOtherFeesShowMoreLines.Text = "Show More Lines";
      foreach (string controlId in new List<string>()
      {
        "groupBoxF",
        "groupBoxG",
        "groupBoxH",
        "groupBoxI",
        "groupBoxJ"
      })
      {
        EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl(controlId);
        control2.Position = new Point(control2.Position.X, control2.Position.Y - num);
      }
      EllieMae.Encompass.Forms.Panel control3 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control3;
      Size size3 = control3.Size;
      int width2 = size3.Width;
      size3 = control3.Size;
      int height2 = size3.Height - num;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control4 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control4;
      Point position = control4.Position;
      int x = position.X;
      position = control4.Position;
      int y = position.Y - num;
      Point point = new Point(x, y);
      label.Position = point;
      CategoryBox control5 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox = control5;
      Size size5 = control5.Size;
      int width3 = size5.Width;
      size5 = control5.Size;
      int height3 = size5.Height - num;
      Size size6 = new Size(width3, height3);
      categoryBox.Size = size6;
    }

    private void ShowMoreTaxesAndOtherFees()
    {
      this.TaxesAndGovFeesBottom.Visible = true;
      this.TaxesAndGovFeesBottom.Position = new Point(this.TaxesAndGovFeesBottom.Position.X, 91);
      this.TaxesAndGovFeesTotal.Position = new Point(this.TaxesAndGovFeesTotal.Position.X, 360);
      this.TaxesAndOtherFeesShowMoreLines.Text = "Show Less Lines";
      int num = 271;
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxE");
      EllieMae.Encompass.Forms.GroupBox groupBox = control1;
      Size size1 = control1.Size;
      int width1 = size1.Width;
      size1 = control1.Size;
      int height1 = size1.Height + num;
      Size size2 = new Size(width1, height1);
      groupBox.Size = size2;
      foreach (string controlId in new List<string>()
      {
        "groupBoxF",
        "groupBoxG",
        "groupBoxH",
        "groupBoxI",
        "groupBoxJ"
      })
      {
        EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl(controlId);
        control2.Position = new Point(control2.Position.X, control2.Position.Y + num);
      }
      EllieMae.Encompass.Forms.Panel control3 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control3;
      Size size3 = control3.Size;
      int width2 = size3.Width;
      size3 = control3.Size;
      int height2 = size3.Height + num;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control4 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control4;
      Point position = control4.Position;
      int x = position.X;
      position = control4.Position;
      int y = position.Y + num;
      Point point = new Point(x, y);
      label.Position = point;
      CategoryBox control5 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox = control5;
      Size size5 = control5.Size;
      int width3 = size5.Width;
      size5 = control5.Size;
      int height3 = size5.Height + num;
      Size size6 = new Size(width3, height3);
      categoryBox.Size = size6;
    }

    private void ShowMoreServiceDidShopFor()
    {
      this.SerBorrDidShopForChargesBottom.Visible = true;
      this.SerBorrDidShopForChargesBottom.Position = new Point(this.SerBorrDidShopForChargesBottom.Position.X, 222);
      this.SerBorrDidShopForChargesTotal.Position = new Point(this.SerBorrDidShopForChargesTotal.Position.X, 535);
      this.SerBorrDidShopForShowMoreLines.Text = "Show Less Lines";
      int num = 313;
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxC");
      EllieMae.Encompass.Forms.GroupBox groupBox1 = control1;
      Size size1 = control1.Size;
      int width1 = size1.Width;
      size1 = control1.Size;
      int height1 = size1.Height + num;
      Size size2 = new Size(width1, height1);
      groupBox1.Size = size2;
      EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxD");
      EllieMae.Encompass.Forms.GroupBox groupBox2 = control2;
      Point position1 = control2.Position;
      int x1 = position1.X;
      position1 = control2.Position;
      int y1 = position1.Y + num;
      Point point1 = new Point(x1, y1);
      groupBox2.Position = point1;
      CategoryBox control3 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox1 = control3;
      Point position2 = control3.Position;
      int x2 = position2.X;
      position2 = control3.Position;
      int y2 = position2.Y + num;
      Point point2 = new Point(x2, y2);
      categoryBox1.Position = point2;
      EllieMae.Encompass.Forms.Panel control4 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control4;
      Size size3 = control4.Size;
      int width2 = size3.Width;
      size3 = control4.Size;
      int height2 = size3.Height + num;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control5 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control5;
      Point position3 = control5.Position;
      int x3 = position3.X;
      position3 = control5.Position;
      int y3 = position3.Y + num;
      Point point3 = new Point(x3, y3);
      label.Position = point3;
      CategoryBox control6 = (CategoryBox) this.currentForm.FindControl("CategoryBox2");
      CategoryBox categoryBox2 = control6;
      Size size5 = control6.Size;
      int width3 = size5.Width;
      size5 = control6.Size;
      int height3 = size5.Height + num;
      Size size6 = new Size(width3, height3);
      categoryBox2.Size = size6;
    }

    private void showLessServiceDidShopFor()
    {
      this.SerBorrDidShopForChargesBottom.Visible = false;
      this.SerBorrDidShopForChargesTotal.Position = new Point(this.SerBorrDidShopForChargesTotal.Position.X, 222);
      int num = 313;
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxC");
      EllieMae.Encompass.Forms.GroupBox groupBox1 = control1;
      Size size1 = control1.Size;
      int width1 = size1.Width;
      size1 = control1.Size;
      int height1 = size1.Height - num;
      Size size2 = new Size(width1, height1);
      groupBox1.Size = size2;
      this.SerBorrDidShopForShowMoreLines.Text = "Show More Lines";
      EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxD");
      EllieMae.Encompass.Forms.GroupBox groupBox2 = control2;
      Point position1 = control2.Position;
      int x1 = position1.X;
      position1 = control2.Position;
      int y1 = position1.Y - num;
      Point point1 = new Point(x1, y1);
      groupBox2.Position = point1;
      CategoryBox control3 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox1 = control3;
      Point position2 = control3.Position;
      int x2 = position2.X;
      position2 = control3.Position;
      int y2 = position2.Y - num;
      Point point2 = new Point(x2, y2);
      categoryBox1.Position = point2;
      EllieMae.Encompass.Forms.Panel control4 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control4;
      Size size3 = control4.Size;
      int width2 = size3.Width;
      size3 = control4.Size;
      int height2 = size3.Height - num;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control5 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control5;
      Point position3 = control5.Position;
      int x3 = position3.X;
      position3 = control5.Position;
      int y3 = position3.Y - num;
      Point point3 = new Point(x3, y3);
      label.Position = point3;
      CategoryBox control6 = (CategoryBox) this.currentForm.FindControl("CategoryBox2");
      CategoryBox categoryBox2 = control6;
      Size size5 = control6.Size;
      int width3 = size5.Width;
      size5 = control6.Size;
      int height3 = size5.Height - num;
      Size size6 = new Size(width3, height3);
      categoryBox2.Size = size6;
    }

    private void ShowMoreServiceDidNotShopFor()
    {
      this.SerBorrDidNotShopForChargesBottom.Visible = true;
      this.SerBorrDidNotShopForChargesBottom.Position = new Point(this.SerBorrDidNotShopForChargesBottom.Position.X, 261);
      this.SerBorrDidNotShopForChargesTotal.Position = new Point(this.SerBorrDidNotShopForChargesTotal.Position.X, 515);
      this.SerBorrDidNotShopForShowMoreLines.Text = "Show Less Lines";
      int num = 254;
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxB");
      EllieMae.Encompass.Forms.GroupBox groupBox1 = control1;
      Size size1 = control1.Size;
      int width1 = size1.Width;
      size1 = control1.Size;
      int height1 = size1.Height + num;
      Size size2 = new Size(width1, height1);
      groupBox1.Size = size2;
      EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxC");
      EllieMae.Encompass.Forms.GroupBox groupBox2 = control2;
      Point position1 = control2.Position;
      int x1 = position1.X;
      position1 = control2.Position;
      int y1 = position1.Y + num;
      Point point1 = new Point(x1, y1);
      groupBox2.Position = point1;
      EllieMae.Encompass.Forms.GroupBox control3 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxD");
      EllieMae.Encompass.Forms.GroupBox groupBox3 = control3;
      Point position2 = control3.Position;
      int x2 = position2.X;
      position2 = control3.Position;
      int y2 = position2.Y + num;
      Point point2 = new Point(x2, y2);
      groupBox3.Position = point2;
      CategoryBox control4 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox1 = control4;
      Point position3 = control4.Position;
      int x3 = position3.X;
      position3 = control4.Position;
      int y3 = position3.Y + num;
      Point point3 = new Point(x3, y3);
      categoryBox1.Position = point3;
      EllieMae.Encompass.Forms.Panel control5 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control5;
      Size size3 = control5.Size;
      int width2 = size3.Width;
      size3 = control5.Size;
      int height2 = size3.Height + num;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control6 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control6;
      Point position4 = control6.Position;
      int x4 = position4.X;
      position4 = control6.Position;
      int y4 = position4.Y + num;
      Point point4 = new Point(x4, y4);
      label.Position = point4;
      CategoryBox control7 = (CategoryBox) this.currentForm.FindControl("CategoryBox2");
      CategoryBox categoryBox2 = control7;
      Size size5 = control7.Size;
      int width3 = size5.Width;
      size5 = control7.Size;
      int height3 = size5.Height + num;
      Size size6 = new Size(width3, height3);
      categoryBox2.Size = size6;
    }

    private void showLessServiceDidNotShopFor()
    {
      this.SerBorrDidNotShopForChargesBottom.Visible = false;
      this.SerBorrDidNotShopForChargesTotal.Position = new Point(this.SerBorrDidNotShopForChargesTotal.Position.X, 261);
      int num = 254;
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxB");
      EllieMae.Encompass.Forms.GroupBox groupBox1 = control1;
      Size size1 = control1.Size;
      int width1 = size1.Width;
      size1 = control1.Size;
      int height1 = size1.Height - num;
      Size size2 = new Size(width1, height1);
      groupBox1.Size = size2;
      this.SerBorrDidNotShopForShowMoreLines.Text = "Show More Lines";
      EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxC");
      EllieMae.Encompass.Forms.GroupBox groupBox2 = control2;
      Point position1 = control2.Position;
      int x1 = position1.X;
      position1 = control2.Position;
      int y1 = position1.Y - num;
      Point point1 = new Point(x1, y1);
      groupBox2.Position = point1;
      EllieMae.Encompass.Forms.GroupBox control3 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxD");
      EllieMae.Encompass.Forms.GroupBox groupBox3 = control3;
      Point position2 = control3.Position;
      int x2 = position2.X;
      position2 = control3.Position;
      int y2 = position2.Y - num;
      Point point2 = new Point(x2, y2);
      groupBox3.Position = point2;
      CategoryBox control4 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox1 = control4;
      Point position3 = control4.Position;
      int x3 = position3.X;
      position3 = control4.Position;
      int y3 = position3.Y - num;
      Point point3 = new Point(x3, y3);
      categoryBox1.Position = point3;
      EllieMae.Encompass.Forms.Panel control5 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control5;
      Size size3 = control5.Size;
      int width2 = size3.Width;
      size3 = control5.Size;
      int height2 = size3.Height - num;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control6 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control6;
      Point position4 = control6.Position;
      int x4 = position4.X;
      position4 = control6.Position;
      int y4 = position4.Y - num;
      Point point4 = new Point(x4, y4);
      label.Position = point4;
      CategoryBox control7 = (CategoryBox) this.currentForm.FindControl("CategoryBox2");
      CategoryBox categoryBox2 = control7;
      Size size5 = control7.Size;
      int width3 = size5.Width;
      size5 = control7.Size;
      int height3 = size5.Height - num;
      Size size6 = new Size(width3, height3);
      categoryBox2.Size = size6;
    }

    private void ShowMoreOriginationCharges()
    {
      this.panelBottomOriginationCharges.Visible = true;
      this.panelBottomOriginationCharges.Position = new Point(this.panelBottomOriginationCharges.Position.X, 227);
      this.paneltotalOriginationCharges.Position = new Point(this.paneltotalOriginationCharges.Position.X, 518);
      this.hyperlinkOriginationCharges.Text = "Show Less Lines";
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxB");
      EllieMae.Encompass.Forms.GroupBox groupBox1 = control1;
      Point position1 = control1.Position;
      int x1 = position1.X;
      position1 = control1.Position;
      int y1 = position1.Y + 291;
      Point point1 = new Point(x1, y1);
      groupBox1.Position = point1;
      EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox11");
      EllieMae.Encompass.Forms.GroupBox groupBox2 = control2;
      Size size1 = control2.Size;
      int width1 = size1.Width;
      size1 = control2.Size;
      int height1 = size1.Height + 291;
      Size size2 = new Size(width1, height1);
      groupBox2.Size = size2;
      EllieMae.Encompass.Forms.GroupBox control3 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxC");
      EllieMae.Encompass.Forms.GroupBox groupBox3 = control3;
      Point position2 = control3.Position;
      int x2 = position2.X;
      position2 = control3.Position;
      int y2 = position2.Y + 291;
      Point point2 = new Point(x2, y2);
      groupBox3.Position = point2;
      EllieMae.Encompass.Forms.GroupBox control4 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxD");
      EllieMae.Encompass.Forms.GroupBox groupBox4 = control4;
      Point position3 = control4.Position;
      int x3 = position3.X;
      position3 = control4.Position;
      int y3 = position3.Y + 291;
      Point point3 = new Point(x3, y3);
      groupBox4.Position = point3;
      CategoryBox control5 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox1 = control5;
      Point position4 = control5.Position;
      int x4 = position4.X;
      position4 = control5.Position;
      int y4 = position4.Y + 291;
      Point point4 = new Point(x4, y4);
      categoryBox1.Position = point4;
      EllieMae.Encompass.Forms.Panel control6 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control6;
      Size size3 = control6.Size;
      int width2 = size3.Width;
      size3 = control6.Size;
      int height2 = size3.Height + 291;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control7 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control7;
      Point position5 = control7.Position;
      int x5 = position5.X;
      position5 = control7.Position;
      int y5 = position5.Y + 291;
      Point point5 = new Point(x5, y5);
      label.Position = point5;
      CategoryBox control8 = (CategoryBox) this.currentForm.FindControl("CategoryBox2");
      CategoryBox categoryBox2 = control8;
      Size size5 = control8.Size;
      int width3 = size5.Width;
      size5 = control8.Size;
      int height3 = size5.Height + 291;
      Size size6 = new Size(width3, height3);
      categoryBox2.Size = size6;
    }

    private void showLessOriginationCharges()
    {
      this.panelBottomOriginationCharges.Visible = false;
      this.paneltotalOriginationCharges.Position = new Point(this.paneltotalOriginationCharges.Position.X, 227);
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxB");
      EllieMae.Encompass.Forms.GroupBox groupBox1 = control1;
      Point position1 = control1.Position;
      int x1 = position1.X;
      position1 = control1.Position;
      int y1 = position1.Y - 291;
      Point point1 = new Point(x1, y1);
      groupBox1.Position = point1;
      EllieMae.Encompass.Forms.GroupBox control2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox11");
      EllieMae.Encompass.Forms.GroupBox groupBox2 = control2;
      Size size1 = control2.Size;
      int width1 = size1.Width;
      size1 = control2.Size;
      int height1 = size1.Height - 291;
      Size size2 = new Size(width1, height1);
      groupBox2.Size = size2;
      this.hyperlinkOriginationCharges.Text = "Show More Lines";
      EllieMae.Encompass.Forms.GroupBox control3 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxC");
      EllieMae.Encompass.Forms.GroupBox groupBox3 = control3;
      Point position2 = control3.Position;
      int x2 = position2.X;
      position2 = control3.Position;
      int y2 = position2.Y - 291;
      Point point2 = new Point(x2, y2);
      groupBox3.Position = point2;
      EllieMae.Encompass.Forms.GroupBox control4 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBoxD");
      EllieMae.Encompass.Forms.GroupBox groupBox4 = control4;
      Point position3 = control4.Position;
      int x3 = position3.X;
      position3 = control4.Position;
      int y3 = position3.Y - 291;
      Point point3 = new Point(x3, y3);
      groupBox4.Position = point3;
      CategoryBox control5 = (CategoryBox) this.currentForm.FindControl("CategoryBoxOtherCosts");
      CategoryBox categoryBox1 = control5;
      Point position4 = control5.Position;
      int x4 = position4.X;
      position4 = control5.Position;
      int y4 = position4.Y - 291;
      Point point4 = new Point(x4, y4);
      categoryBox1.Position = point4;
      EllieMae.Encompass.Forms.Panel control6 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      EllieMae.Encompass.Forms.Panel panel = control6;
      Size size3 = control6.Size;
      int width2 = size3.Width;
      size3 = control6.Size;
      int height2 = size3.Height - 291;
      Size size4 = new Size(width2, height2);
      panel.Size = size4;
      EllieMae.Encompass.Forms.Label control7 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDPage2");
      EllieMae.Encompass.Forms.Label label = control7;
      Point position5 = control7.Position;
      int x5 = position5.X;
      position5 = control7.Position;
      int y5 = position5.Y - 291;
      Point point5 = new Point(x5, y5);
      label.Position = point5;
      CategoryBox control8 = (CategoryBox) this.currentForm.FindControl("CategoryBox2");
      CategoryBox categoryBox2 = control8;
      Size size5 = control8.Size;
      int width3 = size5.Width;
      size5 = control8.Size;
      int height3 = size5.Height - 291;
      Size size6 = new Size(width3, height3);
      categoryBox2.Size = size6;
    }
  }
}
