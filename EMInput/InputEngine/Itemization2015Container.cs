// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Itemization2015Container
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class Itemization2015Container : UserControl, IOnlineHelpTarget, IRefreshContents
  {
    private LoanData loan;
    private LoanScreen loanScreen;
    private REGZGFE_2015InputHandler inputHandler;
    private Sessions.Session session;
    private string currentFormName;
    private IContainer components;
    private ToolTip toolTip1;
    private ComboBox cboHighlight;
    private Label label2;
    private ComboBox cboViews;
    private Label label1;
    private Button btnCollapse;
    private Button btnExpand;
    private Panel panelScreen;
    private GradientPanel gradientPanel2;
    private GradientPanel gradientPanel1;

    public event EventHandler FormLoaded;

    public string CurrentFormName => this.currentFormName;

    public Itemization2015Container(
      Sessions.Session session,
      IWin32Window parentWindow,
      LoanData loan)
    {
      this.session = session;
      this.loan = loan;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.loanScreen = new LoanScreen(this.session, parentWindow, (IHtmlInput) loan);
      this.loanScreen.LoadForm(new InputFormInfo("REGZGFE_2015HTML", "2015 Itemization"));
      this.loanScreen.HideFormTitle();
      this.loanScreen.RemoveBorder();
      this.loanScreen.FormLoaded += new EventHandler(this.loanScreen_FormLoaded);
      this.panelScreen.Controls.Add((Control) this.loanScreen);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      if (disposing)
      {
        if (this.loanScreen != null)
        {
          this.loanScreen.Unload();
          this.loanScreen.Dispose();
        }
        this.toolTip1.Dispose();
        this.cboHighlight.Dispose();
        this.label1.Dispose();
        this.label2.Dispose();
        this.cboViews.Dispose();
        this.btnCollapse.Dispose();
        this.btnExpand.Dispose();
        this.panelScreen.Dispose();
        this.gradientPanel1.Dispose();
        this.gradientPanel2.Dispose();
        this.toolTip1 = (ToolTip) null;
        this.cboHighlight = (ComboBox) null;
        this.label1 = (Label) null;
        this.label2 = (Label) null;
        this.cboViews = (ComboBox) null;
        this.btnCollapse = (Button) null;
        this.btnExpand = (Button) null;
        this.panelScreen = (Panel) null;
        this.gradientPanel1 = (GradientPanel) null;
        this.gradientPanel2 = (GradientPanel) null;
      }
      base.Dispose(disposing);
    }

    private void loanScreen_FormLoaded(object sender, EventArgs e)
    {
      this.inputHandler = (REGZGFE_2015InputHandler) this.loanScreen.GetInputHandler();
    }

    private void btnExpand_Click(object sender, EventArgs e)
    {
      this.inputHandler.RefreshLayout(true, false, (string) null);
      if (this.FormLoaded == null)
        return;
      this.FormLoaded((object) this, e);
    }

    public IInputHandler GetInputHandler() => (IInputHandler) this.inputHandler;

    private void btnCollapse_Click(object sender, EventArgs e)
    {
      this.inputHandler.RefreshLayout(false, true, (string) null);
    }

    private void cboHighlight_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.inputHandler.HighlightLines(this.cboHighlight.Text, (string) null);
    }

    private void cboViews_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.inputHandler.ChangeView(this.cboViews.Text);
    }

    public void RefreshContents() => this.loanScreen.RefreshContents();

    public void RefreshLoanContents() => this.loanScreen.RefreshLoanContents();

    public void SelectAllFields()
    {
      if (this.loanScreen == null)
        return;
      this.loanScreen.SelectAllFields();
    }

    public void DeselectAllFields()
    {
      if (this.loanScreen == null)
        return;
      this.loanScreen.DeselectAllFields();
    }

    public InputFormInfo GetCurrentFormInfo()
    {
      return new InputFormInfo("REGZGFE_2015", "2015 Itemization");
    }

    public void RemoveQuickLinks()
    {
    }

    public InputFormInfo GetCurrentChildFormInfo() => this.GetCurrentFormInfo();

    public string GetHelpTargetName() => "2015 Itemization";

    public void ClearFeeDetailsPopup()
    {
      if (this.inputHandler == null)
        this.inputHandler = (REGZGFE_2015InputHandler) this.loanScreen.GetInputHandler();
      this.inputHandler.CloseAllPopupWindows();
    }

    public void ClearCurrentFieldObject()
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.ClearCurrentFieldObject();
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Itemization2015Container));
      this.toolTip1 = new ToolTip(this.components);
      this.btnCollapse = new Button();
      this.btnExpand = new Button();
      this.cboHighlight = new ComboBox();
      this.label2 = new Label();
      this.cboViews = new ComboBox();
      this.label1 = new Label();
      this.panelScreen = new Panel();
      this.gradientPanel2 = new GradientPanel();
      this.gradientPanel1 = new GradientPanel();
      this.gradientPanel2.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.btnCollapse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCollapse.Image = (Image) componentResourceManager.GetObject("btnCollapse.Image");
      this.btnCollapse.Location = new Point(757, 5);
      this.btnCollapse.Name = "btnCollapse";
      this.btnCollapse.Size = new Size(38, 23);
      this.btnCollapse.TabIndex = 3;
      this.btnCollapse.Text = "   ";
      this.btnCollapse.UseVisualStyleBackColor = true;
      this.btnCollapse.Click += new EventHandler(this.btnCollapse_Click);
      this.btnExpand.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExpand.Image = (Image) componentResourceManager.GetObject("btnExpand.Image");
      this.btnExpand.Location = new Point(717, 5);
      this.btnExpand.Name = "btnExpand";
      this.btnExpand.Size = new Size(38, 23);
      this.btnExpand.TabIndex = 2;
      this.btnExpand.Text = "    ";
      this.btnExpand.UseVisualStyleBackColor = true;
      this.btnExpand.Click += new EventHandler(this.btnExpand_Click);
      this.cboHighlight.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboHighlight.FormattingEnabled = true;
      this.cboHighlight.Items.AddRange(new object[7]
      {
        (object) "",
        (object) "APR Fees",
        (object) "Section 32 Fees",
        (object) "Did shop for",
        (object) "Did not shop for",
        (object) "POC Seller",
        (object) "POC Buyer"
      });
      this.cboHighlight.Location = new Point(63, 5);
      this.cboHighlight.Name = "cboHighlight";
      this.cboHighlight.Size = new Size(186, 21);
      this.cboHighlight.TabIndex = 1;
      this.cboHighlight.SelectedIndexChanged += new EventHandler(this.cboHighlight_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(6, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(51, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Highlight:";
      this.cboViews.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboViews.FormattingEnabled = true;
      this.cboViews.Items.AddRange(new object[3]
      {
        (object) "Standard",
        (object) "Comparison",
        (object) "Paid By / Paid To"
      });
      this.cboViews.Location = new Point(63, 5);
      this.cboViews.Name = "cboViews";
      this.cboViews.Size = new Size(186, 21);
      this.cboViews.TabIndex = 1;
      this.cboViews.SelectedIndexChanged += new EventHandler(this.cboViews_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(38, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Views:";
      this.panelScreen.Dock = DockStyle.Fill;
      this.panelScreen.Location = new Point(0, 31);
      this.panelScreen.Name = "panelScreen";
      this.panelScreen.Size = new Size(800, 303);
      this.panelScreen.TabIndex = 14;
      this.gradientPanel2.BackColor = Color.WhiteSmoke;
      this.gradientPanel2.Borders = AnchorStyles.Bottom;
      this.gradientPanel2.Controls.Add((Control) this.btnCollapse);
      this.gradientPanel2.Controls.Add((Control) this.btnExpand);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.WhiteSmoke;
      this.gradientPanel2.GradientColor2 = Color.WhiteSmoke;
      this.gradientPanel2.Location = new Point(0, 0);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(800, 31);
      this.gradientPanel2.TabIndex = 13;
      this.gradientPanel1.BackColor = Color.WhiteSmoke;
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.cboViews);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.WhiteSmoke;
      this.gradientPanel1.GradientColor2 = Color.WhiteSmoke;
      this.gradientPanel1.GradientPaddingColor = Color.Gainsboro;
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(798, 31);
      this.gradientPanel1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelScreen);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Name = nameof (Itemization2015Container);
      this.Size = new Size(800, 334);
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
