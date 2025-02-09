// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PriceAdjustmentFilterDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PriceAdjustmentFilterDlg : Form
  {
    private Sessions.Session session;
    private LoanReportFieldDefs fieldDefs;
    private IContainer components;
    private AdvancedSearchControl advSearchControl;
    private Button btnApply;
    private Button btnCancel;
    private Label label1;
    private TextBox txtPriceAdjustment;

    public PriceAdjustmentFilterDlg(TradePriceAdjustment tradeAdj)
      : this(tradeAdj, Session.DefaultInstance)
    {
    }

    public PriceAdjustmentFilterDlg(TradePriceAdjustment tradeAdj, Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.advSearchControl.AllowDatabaseFieldsOnly = true;
      this.advSearchControl.AllowDynamicOperators = false;
      this.advSearchControl.FieldDefs = (ReportFieldDefs) this.getFieldDefinitions();
      FieldFilterList filters;
      if (tradeAdj != null)
      {
        filters = new FieldFilterList(tradeAdj.CriterionList);
        this.txtPriceAdjustment.Text = tradeAdj.PriceAdjustment.ToString("0.000");
      }
      else
      {
        filters = new FieldFilterList();
        this.txtPriceAdjustment.Text = "0.000";
      }
      this.advSearchControl.SetCurrentFilter(filters);
    }

    public FieldFilterList GetCurrentFilterList() => this.advSearchControl.GetCurrentFilter();

    public Decimal PriceAdjustment => Decimal.Parse(this.txtPriceAdjustment.Text);

    private LoanReportFieldDefs getFieldDefinitions()
    {
      if (this.fieldDefs == null)
        this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(this.session, LoanReportFieldFlags.LoanDataFieldsInDatabase);
      return this.fieldDefs;
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
      this.txtPriceAdjustment.Text = this.txtPriceAdjustment.Text.Trim();
      if (this.txtPriceAdjustment.Text == "")
        this.txtPriceAdjustment.Text = "0.000";
      if (!Utils.IsDecimal((object) this.txtPriceAdjustment.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a valid value for price adjustment.");
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.advSearchControl = new AdvancedSearchControl();
      this.btnApply = new Button();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.txtPriceAdjustment = new TextBox();
      this.SuspendLayout();
      this.advSearchControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.advSearchControl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.advSearchControl.Location = new Point(13, 39);
      this.advSearchControl.Name = "advSearchControl";
      this.advSearchControl.Size = new Size(671, 239);
      this.advSearchControl.TabIndex = 0;
      this.advSearchControl.Title = "Filters";
      this.btnApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnApply.Location = new Point(528, 295);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new Size(75, 23);
      this.btnApply.TabIndex = 1;
      this.btnApply.Text = "Apply";
      this.btnApply.UseVisualStyleBackColor = true;
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(609, 295);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(86, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Price Adjustment";
      this.txtPriceAdjustment.Location = new Point(105, 10);
      this.txtPriceAdjustment.MaxLength = 14;
      this.txtPriceAdjustment.Name = "txtPriceAdjustment";
      this.txtPriceAdjustment.Size = new Size(100, 20);
      this.txtPriceAdjustment.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(696, 334);
      this.Controls.Add((Control) this.txtPriceAdjustment);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnApply);
      this.Controls.Add((Control) this.advSearchControl);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (PriceAdjustmentFilterDlg);
      this.Text = "Add/Edit Search Filter";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
