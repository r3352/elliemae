// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LODETAILEDLOCKREQUESTInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class LODETAILEDLOCKREQUESTInputHandler : InputHandlerBase
  {
    private string relockPriceCache_LockNumOfDays = "";
    private string relockPriceCache_LockExpirationDate = "";
    private bool isRelockFromExtension;
    private EllieMae.Encompass.Forms.Label label21;
    private string[] rateOptions = new string[0];
    private string[] priceOptions = new string[0];
    private string[] marginOptions = new string[0];
    private bool hideBR = true;
    private bool hideBP = true;
    private bool hideBM = true;
    private bool hideLE = true;
    private bool hideReLockFees = true;
    private bool hideCPA = true;
    private ImageButton picZoomInOver;
    private ImageButton picZoomOutOver;
    private ImageButton picZoomIn;
    private ImageButton picZoomOut;
    private ImageButton picBRZoomOut;
    private ImageButton picBRZoomIn;
    private ImageButton picBPZoomOut;
    private ImageButton picBPZoomIn;
    private ImageButton picLEZoomOut;
    private ImageButton picLEZoomIn;
    private ImageButton picReLockZoomOut;
    private ImageButton picReLockZoomIn;
    private ImageButton picCPAZoomOut;
    private ImageButton picCPAZoomIn;
    private ImageButton picBMZoomOut;
    private ImageButton picBMZoomIn;
    private CategoryBox boxForm;
    private EllieMae.Encompass.Forms.Panel plForm;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private VerticalRule verticalLine;
    private EllieMae.Encompass.Forms.Form formContainer;
    private EllieMae.Encompass.Forms.Panel plBRRight10;
    private EllieMae.Encompass.Forms.Panel plBRRight20;
    private EllieMae.Encompass.Forms.Panel plBRRightTotal;
    private EllieMae.Encompass.Forms.Panel plBPRight10;
    private EllieMae.Encompass.Forms.Panel plBPRight20;
    private EllieMae.Encompass.Forms.Panel plBPRightTotal;
    private EllieMae.Encompass.Forms.Panel plLERight10;
    private EllieMae.Encompass.Forms.Panel plLERight20;
    private EllieMae.Encompass.Forms.Panel plReLockFeeRight10;
    private EllieMae.Encompass.Forms.Panel plReLockFeeRight20;
    private EllieMae.Encompass.Forms.Panel plCPARight10;
    private EllieMae.Encompass.Forms.Panel plCPARight20;
    private EllieMae.Encompass.Forms.Panel plBMRight10;
    private EllieMae.Encompass.Forms.Panel plBMRight20;
    private EllieMae.Encompass.Forms.Panel plBMRightTotal;
    private EllieMae.Encompass.Forms.Panel plBRLeft10;
    private EllieMae.Encompass.Forms.Panel plBRLeft20;
    private EllieMae.Encompass.Forms.Panel plBRLeftTotal;
    private EllieMae.Encompass.Forms.Panel plBPLeft10;
    private EllieMae.Encompass.Forms.Panel plBPLeft20;
    private EllieMae.Encompass.Forms.Panel plBPLeftTotal;
    private EllieMae.Encompass.Forms.Panel plLELeft10;
    private EllieMae.Encompass.Forms.Panel plLELeft20;
    private EllieMae.Encompass.Forms.Panel plReLockFeeLeft10;
    private EllieMae.Encompass.Forms.Panel plReLockFeeLeft20;
    private EllieMae.Encompass.Forms.Panel plCPALeft10;
    private EllieMae.Encompass.Forms.Panel plCPALeft20;
    private EllieMae.Encompass.Forms.Panel plBMLeft10;
    private EllieMae.Encompass.Forms.Panel plBMLeft20;
    private EllieMae.Encompass.Forms.Panel plBMLeftTotal;
    private EllieMae.Encompass.Forms.Panel plRightComment;
    private EllieMae.Encompass.Forms.Panel plLeftComment;

    public LODETAILEDLOCKREQUESTInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public LODETAILEDLOCKREQUESTInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      this.UpdateReLockFields();
      this.label21 = this.currentForm.FindControl("Label21") as EllieMae.Encompass.Forms.Label;
      if ((!(this.GetFieldValue("2626") == "Banked - Retail") || !(bool) Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"]) && (!(this.GetFieldValue("2626") == "Banked - Wholesale") || !(bool) Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]))
        return;
      this.label21.Text = "(Par pricing is 0.00)\r\n";
    }

    public LODETAILEDLOCKREQUESTInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public LODETAILEDLOCKREQUESTInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
      this.hideBR = !(this.GetFieldValue("2420") != "") && !(this.GetFieldValue("2421") != "") && !(this.GetFieldValue("2058") != "") && !(this.GetFieldValue("2059") != "");
      this.hideBP = !(this.GetFieldValue("2122") != "") && !(this.GetFieldValue("2123") != "") && !(this.GetFieldValue("2066") != "") && !(this.GetFieldValue("2067") != "");
      this.hideBM = !(this.GetFieldValue("2656") != "") && !(this.GetFieldValue("2657") != "") && !(this.GetFieldValue("2699") != "") && !(this.GetFieldValue("2700") != "");
      this.label21 = this.currentForm.FindControl("Label21") as EllieMae.Encompass.Forms.Label;
      if (dataTemplate != null && (dataTemplate.GetField("2626") == "Banked - Retail" && (bool) Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"] || dataTemplate.GetField("2626") == "Banked - Wholesale" && (bool) Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]))
        this.label21.Text = "(Par pricing is 0.00)\r\n";
      this.zoomPanels();
    }

    internal override void CreateControls()
    {
      object[] allSecondaryFields = this.session.ConfigurationManager.GetAllSecondaryFields();
      if (allSecondaryFields != null && allSecondaryFields.Length >= 6)
      {
        if (allSecondaryFields[0] != null)
          this.rateOptions = (string[]) allSecondaryFields[0];
        if (allSecondaryFields[0] != null)
          this.priceOptions = (string[]) allSecondaryFields[1];
        if (allSecondaryFields[0] != null)
          this.marginOptions = (string[]) allSecondaryFields[2];
      }
      this.picZoomInOver = this.currentForm.FindControl("btnZoomInOver") as ImageButton;
      this.picZoomOutOver = this.currentForm.FindControl("btnZoomOutOver") as ImageButton;
      this.picZoomIn = this.currentForm.FindControl("btnZoomIn") as ImageButton;
      this.picZoomOut = this.currentForm.FindControl("btnZoomOut") as ImageButton;
      this.picBRZoomOut = this.currentForm.FindControl("btnBRZoomOut") as ImageButton;
      this.picBRZoomIn = this.currentForm.FindControl("btnBRZoomIn") as ImageButton;
      this.picBPZoomOut = this.currentForm.FindControl("btnBPZoomOut") as ImageButton;
      this.picBPZoomIn = this.currentForm.FindControl("btnBPZoomIn") as ImageButton;
      this.picLEZoomOut = this.currentForm.FindControl("btnLEZoomOut") as ImageButton;
      this.picLEZoomIn = this.currentForm.FindControl("btnLEZoomIn") as ImageButton;
      this.picReLockZoomOut = this.currentForm.FindControl("btnReLockZoomOut") as ImageButton;
      this.picReLockZoomIn = this.currentForm.FindControl("btnReLockZoomIn") as ImageButton;
      this.picCPAZoomOut = this.currentForm.FindControl("btnCPAZoomOut") as ImageButton;
      this.picCPAZoomIn = this.currentForm.FindControl("btnCPAZoomIn") as ImageButton;
      this.picBMZoomOut = this.currentForm.FindControl("btnBMZoomOut") as ImageButton;
      this.picBMZoomIn = this.currentForm.FindControl("btnBMZoomIn") as ImageButton;
      this.plBRRight10 = this.currentForm.FindControl("PanelBRRight10") as EllieMae.Encompass.Forms.Panel;
      this.plBRRight20 = this.currentForm.FindControl("PanelBRRight20") as EllieMae.Encompass.Forms.Panel;
      this.plBRRightTotal = this.currentForm.FindControl("PanelBRRightTotal") as EllieMae.Encompass.Forms.Panel;
      this.plBPRight10 = this.currentForm.FindControl("PanelBPRight10") as EllieMae.Encompass.Forms.Panel;
      this.plBPRight20 = this.currentForm.FindControl("PanelBPRight20") as EllieMae.Encompass.Forms.Panel;
      this.plBPRightTotal = this.currentForm.FindControl("PanelBPRightTotal") as EllieMae.Encompass.Forms.Panel;
      this.plLERight10 = this.currentForm.FindControl("PanelLERight10") as EllieMae.Encompass.Forms.Panel;
      this.plLERight20 = this.currentForm.FindControl("PanelLERight20") as EllieMae.Encompass.Forms.Panel;
      this.plReLockFeeRight10 = this.currentForm.FindControl("PanelReLockRight10") as EllieMae.Encompass.Forms.Panel;
      this.plReLockFeeRight20 = this.currentForm.FindControl("PanelReLockRight20") as EllieMae.Encompass.Forms.Panel;
      this.plCPARight10 = this.currentForm.FindControl("PanelCPARight10") as EllieMae.Encompass.Forms.Panel;
      this.plCPARight20 = this.currentForm.FindControl("PanelCPARight20") as EllieMae.Encompass.Forms.Panel;
      this.plBMRight10 = this.currentForm.FindControl("PanelBMRight10") as EllieMae.Encompass.Forms.Panel;
      this.plBMRight20 = this.currentForm.FindControl("PanelBMRight20") as EllieMae.Encompass.Forms.Panel;
      this.plBMRightTotal = this.currentForm.FindControl("PanelBMRightTotal") as EllieMae.Encompass.Forms.Panel;
      this.plBRLeft10 = this.currentForm.FindControl("PanelBRLeft10") as EllieMae.Encompass.Forms.Panel;
      this.plBRLeft20 = this.currentForm.FindControl("PanelBRLeft20") as EllieMae.Encompass.Forms.Panel;
      this.plBRLeftTotal = this.currentForm.FindControl("PanelBRLeftTotal") as EllieMae.Encompass.Forms.Panel;
      this.plBPLeft10 = this.currentForm.FindControl("PanelBPLeft10") as EllieMae.Encompass.Forms.Panel;
      this.plBPLeft20 = this.currentForm.FindControl("PanelBPLeft20") as EllieMae.Encompass.Forms.Panel;
      this.plBPLeftTotal = this.currentForm.FindControl("PanelBPLeftTotal") as EllieMae.Encompass.Forms.Panel;
      this.plLELeft10 = this.currentForm.FindControl("PanelLELeft10") as EllieMae.Encompass.Forms.Panel;
      this.plLELeft20 = this.currentForm.FindControl("PanelLELeft20") as EllieMae.Encompass.Forms.Panel;
      this.plReLockFeeLeft10 = this.currentForm.FindControl("PanelReLockLeft10") as EllieMae.Encompass.Forms.Panel;
      this.plReLockFeeLeft20 = this.currentForm.FindControl("PanelReLockLeft20") as EllieMae.Encompass.Forms.Panel;
      this.plCPALeft10 = this.currentForm.FindControl("PanelCPALeft10") as EllieMae.Encompass.Forms.Panel;
      this.plCPALeft20 = this.currentForm.FindControl("PanelCPALeft20") as EllieMae.Encompass.Forms.Panel;
      this.plBMLeft10 = this.currentForm.FindControl("PanelBMLeft10") as EllieMae.Encompass.Forms.Panel;
      this.plBMLeft20 = this.currentForm.FindControl("PanelBMLeft20") as EllieMae.Encompass.Forms.Panel;
      this.plBMLeftTotal = this.currentForm.FindControl("PanelBMLeftTotal") as EllieMae.Encompass.Forms.Panel;
      this.plRightComment = this.currentForm.FindControl("PanelRightComment") as EllieMae.Encompass.Forms.Panel;
      this.plLeftComment = this.currentForm.FindControl("PanelLeftComment") as EllieMae.Encompass.Forms.Panel;
      this.boxForm = this.currentForm.FindControl("CategoryBoxForm") as CategoryBox;
      this.plForm = this.currentForm.FindControl("pnlForm") as EllieMae.Encompass.Forms.Panel;
      this.labelFormName = this.currentForm.FindControl("LabelFormName") as EllieMae.Encompass.Forms.Label;
      this.verticalLine = this.currentForm.FindControl("VerticalRule1") as VerticalRule;
      this.formContainer = this.currentForm.FindControl("Form1") as EllieMae.Encompass.Forms.Form;
      this.hideBR = false;
      this.hideBP = false;
      this.hideBM = false;
      this.hideLE = false;
      this.hideReLockFees = false;
      this.hideCPA = false;
      this.zoomPanels();
    }

    private void zoomPanels()
    {
      int x1 = this.plBRLeft10.Position.X;
      int x2 = this.plBRRight10.Position.X;
      int y1 = this.plBRRight10.Position.Y + this.plBRRight10.Size.Height;
      Size size1;
      if (this.hideBR)
      {
        this.plBRRight20.Visible = false;
        this.plBRLeft20.Visible = false;
        this.picBRZoomOut.Visible = false;
        this.picBRZoomIn.Visible = true;
      }
      else
      {
        this.plBRRight20.Visible = true;
        this.plBRLeft20.Visible = true;
        int num = y1;
        size1 = this.plBRRight20.Size;
        int height = size1.Height;
        y1 = num + height;
        this.picBRZoomOut.Visible = true;
        this.picBRZoomIn.Visible = false;
      }
      this.plBRRightTotal.Position = new Point(x2, y1);
      this.plBRLeftTotal.Position = new Point(x1, y1);
      int num1 = y1;
      size1 = this.plBRRightTotal.Size;
      int height1 = size1.Height;
      int y2 = num1 + height1;
      this.plBPRight10.Position = new Point(x2, y2);
      this.plBPLeft10.Position = new Point(x1, y2);
      int num2 = y2;
      size1 = this.plBPRight10.Size;
      int height2 = size1.Height;
      int y3 = num2 + height2;
      if (this.hideBP)
      {
        this.plBPRight20.Visible = false;
        this.plBPLeft20.Visible = false;
        this.picBPZoomOut.Visible = false;
        this.picBPZoomIn.Visible = true;
      }
      else
      {
        this.plBPRight20.Visible = true;
        this.plBPLeft20.Visible = true;
        this.plBPRight20.Position = new Point(x2, y3);
        this.plBPLeft20.Position = new Point(x1, y3);
        int num3 = y3;
        size1 = this.plBPRight20.Size;
        int height3 = size1.Height;
        y3 = num3 + height3;
        this.picBPZoomOut.Visible = true;
        this.picBPZoomIn.Visible = false;
      }
      this.plLERight10.Position = new Point(x2, y3);
      this.plLELeft10.Position = new Point(x1, y3);
      int num4 = y3;
      size1 = this.plLERight10.Size;
      int height4 = size1.Height;
      int y4 = num4 + height4;
      if (this.hideLE)
      {
        this.plLERight20.Visible = false;
        this.plLELeft20.Visible = false;
        this.picLEZoomOut.Visible = false;
        this.picLEZoomIn.Visible = true;
      }
      else
      {
        this.plLERight20.Visible = true;
        this.plLELeft20.Visible = true;
        this.plLERight20.Position = new Point(x2, y4);
        this.plLELeft20.Position = new Point(x1, y4);
        int num5 = y4;
        size1 = this.plLERight20.Size;
        int height5 = size1.Height;
        y4 = num5 + height5;
        this.picLEZoomOut.Visible = true;
        this.picLEZoomIn.Visible = false;
      }
      this.plReLockFeeRight10.Position = new Point(x2, y4);
      this.plReLockFeeLeft10.Position = new Point(x1, y4);
      int num6 = y4;
      size1 = this.plReLockFeeRight10.Size;
      int height6 = size1.Height;
      int y5 = num6 + height6;
      if (this.hideReLockFees)
      {
        this.plReLockFeeRight20.Visible = false;
        this.plReLockFeeLeft20.Visible = false;
        this.picReLockZoomOut.Visible = false;
        this.picReLockZoomIn.Visible = true;
      }
      else
      {
        this.plReLockFeeRight20.Visible = true;
        this.plReLockFeeLeft20.Visible = true;
        this.plReLockFeeRight20.Position = new Point(x2, y5);
        this.plReLockFeeLeft20.Position = new Point(x1, y5);
        int num7 = y5;
        size1 = this.plReLockFeeRight20.Size;
        int height7 = size1.Height;
        y5 = num7 + height7;
        this.picReLockZoomOut.Visible = true;
        this.picReLockZoomIn.Visible = false;
      }
      this.plCPARight10.Position = new Point(x2, y5);
      this.plCPALeft10.Position = new Point(x1, y5);
      int num8 = y5;
      size1 = this.plCPARight10.Size;
      int height8 = size1.Height;
      int y6 = num8 + height8;
      if (this.hideCPA)
      {
        this.plCPARight20.Visible = false;
        this.plCPALeft20.Visible = false;
        this.picCPAZoomOut.Visible = false;
        this.picCPAZoomIn.Visible = true;
      }
      else
      {
        this.plCPARight20.Visible = true;
        this.plCPALeft20.Visible = true;
        this.plCPARight20.Position = new Point(x2, y6);
        this.plCPALeft20.Position = new Point(x1, y6);
        int num9 = y6;
        size1 = this.plCPARight20.Size;
        int height9 = size1.Height;
        y6 = num9 + height9;
        this.picCPAZoomOut.Visible = true;
        this.picCPAZoomIn.Visible = false;
      }
      this.plBPRightTotal.Position = new Point(x2, y6);
      this.plBPLeftTotal.Position = new Point(x1, y6);
      int num10 = y6;
      size1 = this.plBPRightTotal.Size;
      int height10 = size1.Height;
      int y7 = num10 + height10;
      this.plBMRight10.Position = new Point(x2, y7);
      this.plBMLeft10.Position = new Point(x1, y7);
      int num11 = y7;
      size1 = this.plBMRight10.Size;
      int height11 = size1.Height;
      int y8 = num11 + height11;
      if (this.hideBM)
      {
        this.plBMRight20.Visible = false;
        this.plBMLeft20.Visible = false;
        this.picBMZoomOut.Visible = false;
        this.picBMZoomIn.Visible = true;
      }
      else
      {
        this.plBMRight20.Visible = true;
        this.plBMLeft20.Visible = true;
        this.plBMRight20.Position = new Point(x2, y8);
        this.plBMLeft20.Position = new Point(x1, y8);
        int num12 = y8;
        size1 = this.plBMRight20.Size;
        int height12 = size1.Height;
        y8 = num12 + height12;
        this.picBMZoomOut.Visible = true;
        this.picBMZoomIn.Visible = false;
      }
      this.plBMRightTotal.Position = new Point(x2, y8);
      this.plBMLeftTotal.Position = new Point(x1, y8);
      int num13 = y8;
      size1 = this.plBMRightTotal.Size;
      int height13 = size1.Height;
      int y9 = num13 + height13;
      this.plRightComment.Position = new Point(x2, y9);
      this.plLeftComment.Position = new Point(x1, y9);
      int num14 = y9;
      size1 = this.plRightComment.Size;
      int height14 = size1.Height;
      int num15 = num14 + height14;
      CategoryBox boxForm = this.boxForm;
      size1 = this.boxForm.Size;
      Size size2 = new Size(size1.Width, num15 + 24);
      boxForm.Size = size2;
      EllieMae.Encompass.Forms.Panel plForm = this.plForm;
      size1 = this.plForm.Size;
      int width1 = size1.Width;
      size1 = this.boxForm.Size;
      int height15 = size1.Height + 10;
      Size size3 = new Size(width1, height15);
      plForm.Size = size3;
      VerticalRule verticalLine = this.verticalLine;
      size1 = this.verticalLine.Size;
      int width2 = size1.Width;
      size1 = this.boxForm.Size;
      int height16 = size1.Height - 20;
      Size size4 = new Size(width2, height16);
      verticalLine.Size = size4;
      EllieMae.Encompass.Forms.Label labelFormName = this.labelFormName;
      int x3 = this.labelFormName.Position.X;
      size1 = this.plForm.Size;
      int y10 = size1.Height + 4;
      Point point = new Point(x3, y10);
      labelFormName.Position = point;
      this.UpdateContents(true);
      this.RefreshContents();
    }

    public override void onmousedown(mshtml.IHTMLEventObj pEvtObj)
    {
      string id = pEvtObj.srcElement.id;
      if (id.StartsWith("PickListRate"))
      {
        PickList controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as PickList;
        if (controlForElement.Options.Count != 0 || this.rateOptions.Length == 0)
          return;
        for (int index = 0; index < this.rateOptions.Length; ++index)
          controlForElement.Options.Add(this.rateOptions[index]);
        controlForElement.BoundControl = (FieldControl) this.currentForm.FindControl(controlForElement.ControlID.Replace("PickList", "TextBox"));
        controlForElement.ItemSelected += new ItemSelectedEventHandler(this.onRateListItemSelected);
      }
      else if (id.StartsWith("PickListPrice"))
      {
        PickList controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as PickList;
        if (controlForElement.Options.Count != 0 || this.priceOptions.Length == 0)
          return;
        for (int index = 0; index < this.priceOptions.Length; ++index)
          controlForElement.Options.Add(this.priceOptions[index]);
        controlForElement.BoundControl = (FieldControl) this.currentForm.FindControl(controlForElement.ControlID.Replace("PickList", "TextBox"));
        controlForElement.ItemSelected += new ItemSelectedEventHandler(this.onPriceListItemSelected);
      }
      else
      {
        if (!id.StartsWith("PickListMargin"))
          return;
        PickList controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as PickList;
        if (controlForElement.Options.Count != 0 || this.marginOptions.Length == 0)
          return;
        for (int index = 0; index < this.marginOptions.Length; ++index)
          controlForElement.Options.Add(this.marginOptions[index]);
        controlForElement.BoundControl = (FieldControl) this.currentForm.FindControl(controlForElement.ControlID.Replace("PickList", "TextBox"));
        controlForElement.ItemSelected += new ItemSelectedEventHandler(this.onMarginListItemSelected);
      }
    }

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      switch (pEvtObj.srcElement.id)
      {
        case "btnBPZoomOut":
          this.picBPZoomOut.Source = this.picZoomOutOver.Source;
          break;
        case "btnBRZoomOut":
          this.picBRZoomOut.Source = this.picZoomOutOver.Source;
          break;
        case "btnBMZoomOut":
          this.picBMZoomOut.Source = this.picZoomOutOver.Source;
          break;
        case "btnLEZoomOut":
          this.picLEZoomOut.Source = this.picZoomOutOver.Source;
          break;
        case "btnReLockZoomOut":
          this.picReLockZoomOut.Source = this.picZoomOutOver.Source;
          break;
        case "btnCPAZoomOut":
          this.picCPAZoomOut.Source = this.picZoomOutOver.Source;
          break;
        case "btnBPZoomIn":
          this.picBPZoomIn.Source = this.picZoomInOver.Source;
          break;
        case "btnBRZoomIn":
          this.picBRZoomIn.Source = this.picZoomInOver.Source;
          break;
        case "btnBMZoomIn":
          this.picLEZoomIn.Source = this.picZoomInOver.Source;
          break;
        case "btnLEZoomIn":
          this.picBMZoomIn.Source = this.picZoomInOver.Source;
          break;
        case "btnReLockZoomIn":
          this.picReLockZoomIn.Source = this.picZoomInOver.Source;
          break;
        case "btnCPAZoomIn":
          this.picCPAZoomIn.Source = this.picZoomInOver.Source;
          break;
      }
      base.onmouseenter(pEvtObj);
    }

    public override void onmouseleave(mshtml.IHTMLEventObj pEvtObj)
    {
      switch (pEvtObj.srcElement.id)
      {
        case "btnBPZoomOut":
          this.picBPZoomOut.Source = this.picZoomOut.Source;
          break;
        case "btnBRZoomOut":
          this.picBRZoomOut.Source = this.picZoomOut.Source;
          break;
        case "btnBMZoomOut":
          this.picBMZoomOut.Source = this.picZoomOut.Source;
          break;
        case "btnLEZoomOut":
          this.picLEZoomOut.Source = this.picZoomOut.Source;
          break;
        case "btnReLockZoomOut":
          this.picReLockZoomOut.Source = this.picZoomOut.Source;
          break;
        case "btnCPAZoomOut":
          this.picCPAZoomOut.Source = this.picZoomOut.Source;
          break;
        case "btnBPZoomIn":
          this.picBPZoomIn.Source = this.picZoomIn.Source;
          break;
        case "btnBRZoomIn":
          this.picBRZoomIn.Source = this.picZoomIn.Source;
          break;
        case "btnBMZoomIn":
          this.picBMZoomIn.Source = this.picZoomIn.Source;
          break;
        case "btnLEZoomIn":
          this.picLEZoomIn.Source = this.picZoomIn.Source;
          break;
        case "btnReLockZoomIn":
          this.picReLockZoomIn.Source = this.picZoomIn.Source;
          break;
        case "btnCPAZoomIn":
          this.picCPAZoomIn.Source = this.picZoomIn.Source;
          break;
      }
    }

    private void onRateListItemSelected(object sender, ItemSelectedEventArgs e)
    {
      EllieMae.Encompass.Forms.TextBox control = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl(((EllieMae.Encompass.Forms.Control) sender).ControlID.Replace("PickListRate", "TextBoxRate"));
      control.Text = e.SelectedItem.Value;
      control.Focus();
    }

    private void onPriceListItemSelected(object sender, ItemSelectedEventArgs e)
    {
      EllieMae.Encompass.Forms.TextBox control = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl(((EllieMae.Encompass.Forms.Control) sender).ControlID.Replace("PickListPrice", "TextBoxPrice"));
      control.Text = e.SelectedItem.Value;
      control.Focus();
    }

    private void onMarginListItemSelected(object sender, ItemSelectedEventArgs e)
    {
      EllieMae.Encompass.Forms.TextBox control = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl(((EllieMae.Encompass.Forms.Control) sender).ControlID.Replace("PickListMargin", "TextBoxMargin"));
      control.Text = e.SelectedItem.Value;
      control.Focus();
    }

    public override void ExecAction(string action)
    {
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 31976832:
          if (!(action == "lockextensionzoomout"))
            break;
          this.hideLE = true;
          this.zoomPanels();
          break;
        case 76333550:
          if (!(action == "basemarginzoomin"))
            break;
          this.hideBM = false;
          this.zoomPanels();
          break;
        case 117381350:
          if (!(action == "cpazoomout"))
            break;
          this.hideCPA = true;
          this.zoomPanels();
          break;
        case 836556322:
          if (!(action == "baseratezoomin"))
            break;
          this.hideBR = false;
          this.zoomPanels();
          break;
        case 1185976910:
          if (!(action == "copycurrentlock"))
            break;
          for (int index = 2036; index <= 2087; ++index)
            this.UpdateFieldValue((index + 56).ToString(), this.GetFieldValue(index.ToString()), true);
          for (int index = 2516; index <= 2549; ++index)
            this.UpdateFieldValue((index - 102).ToString(), this.GetFieldValue(index.ToString()), true);
          for (int index = 2690; index <= 2732; ++index)
            this.UpdateFieldValue((index - 43).ToString(), this.GetFieldValue(index.ToString()), true);
          for (int index = 3434; index <= 3453; ++index)
            this.UpdateFieldValue((index + 20).ToString(), this.GetFieldValue(index.ToString()), true);
          for (int index = 4416; index <= 4435; ++index)
            this.UpdateFieldValue((index - 160).ToString(), this.GetFieldValue(index.ToString()), true);
          for (int index = 4436; index <= 4455; ++index)
            this.UpdateFieldValue((index - 100).ToString(), this.GetFieldValue(index.ToString()), true);
          this.UpdateFieldValue("2144", this.GetFieldValue("2035"), true);
          this.UpdateFieldValue("2088", this.GetFieldValue("2034"), true);
          this.UpdateFieldValue("2089", this.GetFieldValue("2145"), true);
          this.UpdateFieldValue("2090", this.GetFieldValue("2146"), true);
          this.UpdateFieldValue("2091", this.GetFieldValue("2147"), true);
          this.UpdateFieldValue("3254", this.GetFieldValue("3255"), true);
          this.UpdateFieldValue("3847", this.GetFieldValue("3848"), true);
          this.UpdateFieldValue("3872", this.GetFieldValue("3873"), true);
          this.UpdateFieldValue("3874", this.GetFieldValue("3875"), true);
          if (this.loan == null || this.loan.LinkedData == null)
            break;
          Cursor.Current = Cursors.WaitCursor;
          this.loan.SyncPiggyBackFiles((string[]) null, true, false, (string) null, (string) null, false);
          Cursor.Current = Cursors.Default;
          this.RefreshContents();
          break;
        case 1474802063:
          if (!(action == "clearfields"))
            break;
          for (int index = 2092; index <= 2143; ++index)
            this.UpdateFieldValue(index.ToString(), "", true);
          for (int index = 2414; index <= 2447; ++index)
            this.UpdateFieldValue(index.ToString(), "", true);
          for (int index = 2647; index <= 2689; ++index)
            this.UpdateFieldValue(index.ToString(), "", true);
          for (int index = 3454; index <= 3473; ++index)
            this.UpdateFieldValue(index.ToString(), "", true);
          for (int index = 4256; index <= 4275; ++index)
            this.UpdateFieldValue(index.ToString(), "", true);
          for (int index = 4336; index <= 4355; ++index)
            this.UpdateFieldValue(index.ToString(), "", true);
          this.UpdateFieldValue("2144", "", true);
          this.UpdateFieldValue("2088", "", true);
          this.UpdateFieldValue("2089", "", true);
          this.UpdateFieldValue("2090", "", true);
          this.UpdateFieldValue("2091", "", true);
          this.UpdateFieldValue("3254", "", true);
          this.UpdateFieldValue("3847", "", true);
          this.UpdateFieldValue("3872", "", true);
          this.UpdateFieldValue("3874", "", true);
          if (this.loan == null || this.loan.LinkedData == null)
            break;
          Cursor.Current = Cursors.WaitCursor;
          this.loan.SyncPiggyBackFiles((string[]) null, true, false, (string) null, (string) null, false);
          Cursor.Current = Cursors.Default;
          this.RefreshContents();
          break;
        case 1509603543:
          if (!(action == "relockzoomin"))
            break;
          this.hideReLockFees = false;
          this.zoomPanels();
          break;
        case 1883646779:
          if (!(action == "baseratezoomout"))
            break;
          this.hideBR = true;
          this.zoomPanels();
          break;
        case 2770411901:
          if (!(action == "basepricezoomin"))
            break;
          this.hideBP = false;
          this.zoomPanels();
          break;
        case 3224674269:
          if (!(action == "cpazoomin"))
            break;
          this.hideCPA = false;
          this.zoomPanels();
          break;
        case 3343827718:
          if (!(action == "basepricezoomout"))
            break;
          this.hideBP = true;
          this.zoomPanels();
          break;
        case 3496665820:
          if (!(action == "relockzoomout"))
            break;
          this.hideReLockFees = true;
          this.zoomPanels();
          break;
        case 3631701647:
          if (!(action == "basemarginzoomout"))
            break;
          this.hideBM = true;
          this.zoomPanels();
          break;
        case 3884394771:
          if (!(action == "lockextensionzoomin"))
            break;
          this.hideLE = false;
          this.zoomPanels();
          break;
      }
    }

    private void UpdateReLockFields()
    {
      EllieMae.Encompass.Forms.TextBox control1 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_2090");
      EllieMae.Encompass.Forms.TextBox control2 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("TextBoxRelockLockDays");
      EllieMae.Encompass.Forms.TextBox control3 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_2091");
      EllieMae.Encompass.Forms.TextBox control4 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("TextBoxRelockExpirationDate");
      if (LockUtils.IsRelock(this.GetFieldValue("3841")))
      {
        this.DisplayReLockFields(true);
        LockRequestLog confirmedLockForRelocks = LoanLockUtils.GetPreviousConfirmedLockForRelocks(this.session.LoanDataMgr);
        if (confirmedLockForRelocks != null)
        {
          Hashtable lockRequestSnapshot = confirmedLockForRelocks.GetLockRequestSnapshot();
          if (Utils.IsDate(lockRequestSnapshot[(object) "3364"]))
          {
            this.isRelockFromExtension = true;
            this.relockPriceCache_LockExpirationDate = lockRequestSnapshot[(object) "3364"].ToString();
            int num = Utils.ParseInt(lockRequestSnapshot[(object) "2150"], 0) + Utils.ParseInt(lockRequestSnapshot[(object) "3431"], 0);
            this.relockPriceCache_LockNumOfDays = num == 0 ? "" : num.ToString();
          }
        }
        if (!this.isRelockFromExtension)
          return;
        control2.Visible = true;
        control1.Visible = false;
        control4.Visible = true;
        control3.Visible = false;
        control2.Top = control1.Top;
        control2.Left = control1.Left;
        control2.Size = control1.Size;
        control4.Top = control3.Top;
        control4.Left = control3.Left;
        control4.Size = control3.Size;
        control2.Text = this.relockPriceCache_LockNumOfDays;
        control4.Text = this.relockPriceCache_LockExpirationDate;
      }
      else
        this.DisplayReLockFields(false);
    }

    private void DisplayReLockFields(bool relock)
    {
      EllieMae.Encompass.Forms.TextBox control1 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_2090");
      EllieMae.Encompass.Forms.TextBox control2 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("TextBoxRelockLockDays");
      EllieMae.Encompass.Forms.TextBox control3 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_2091");
      EllieMae.Encompass.Forms.TextBox control4 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("TextBoxRelockExpirationDate");
      if (relock)
      {
        this.SetFieldReadOnly("l_2088");
        this.SetFieldReadOnly("l_2089");
        this.SetFieldReadOnly("l_2090");
        this.SetFieldReadOnly("l_2091");
        this.SetFieldReadOnly("l_3254");
        this.SetFieldDisabled("Calendar1");
        this.SetFieldDisabled("Calendar2");
        this.SetFieldDisabled("Calendar3");
        this.SetFieldDisabled("btncopy");
        this.SetFieldDisabled("btnclear");
      }
      else
      {
        this.SetFieldAvailable("l_2088");
        this.SetFieldAvailable("l_2089");
        this.SetFieldAvailable("l_2090");
        this.SetFieldAvailable("l_2091");
        this.SetFieldAvailable("l_3254");
        this.SetFieldEnabled("Calendar1");
        this.SetFieldEnabled("Calendar2");
        this.SetFieldEnabled("Calendar3");
        this.SetFieldEnabled("btncopy");
        this.SetFieldEnabled("btnclear");
        control2.Visible = false;
        control1.Visible = true;
        control1.Enabled = true;
        control4.Visible = false;
        control3.Visible = true;
        control3.Enabled = true;
      }
    }
  }
}
