// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LoanBalancingWorksheet
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LoanBalancingWorksheet : CustomUserControl, IOnlineHelpTarget, IRefreshContents
  {
    private const string className = "LoanBalancingWorksheet";
    protected static string sw = Tracing.SwOutsideLoan;
    private Label label2;
    private Label label5;
    private System.ComponentModel.Container components;
    private LoanData loan;
    private TextBox boxDebits;
    private TableContainer tableContainer1;
    private GridView gridDebit;
    private TableContainer tableContainer2;
    private GridView gridCredit;
    private TextBox boxCredits;

    public LoanBalancingWorksheet(LoanData loan)
    {
      this.Dock = DockStyle.Fill;
      this.loan = loan;
      this.InitializeComponent();
      this.initForm();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.label2 = new Label();
      this.boxDebits = new TextBox();
      this.label5 = new Label();
      this.boxCredits = new TextBox();
      this.tableContainer1 = new TableContainer();
      this.gridDebit = new GridView();
      this.tableContainer2 = new TableContainer();
      this.gridCredit = new GridView();
      this.tableContainer1.SuspendLayout();
      this.tableContainer2.SuspendLayout();
      this.SuspendLayout();
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(109, 372);
      this.label2.Name = "label2";
      this.label2.Size = new Size(72, 14);
      this.label2.TabIndex = 6;
      this.label2.Text = "Total Debits";
      this.boxDebits.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.boxDebits.BackColor = Color.WhiteSmoke;
      this.boxDebits.Font = new Font("Arial", 8.25f);
      this.boxDebits.Location = new Point(186, 369);
      this.boxDebits.Name = "boxDebits";
      this.boxDebits.ReadOnly = true;
      this.boxDebits.Size = new Size(132, 20);
      this.boxDebits.TabIndex = 3;
      this.boxDebits.TextAlign = HorizontalAlignment.Right;
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(103, 372);
      this.label5.Name = "label5";
      this.label5.Size = new Size(78, 14);
      this.label5.TabIndex = 9;
      this.label5.Text = "Total Credits";
      this.boxCredits.BackColor = Color.WhiteSmoke;
      this.boxCredits.Font = new Font("Arial", 8.25f);
      this.boxCredits.Location = new Point(186, 369);
      this.boxCredits.Name = "boxCredits";
      this.boxCredits.ReadOnly = true;
      this.boxCredits.Size = new Size(132, 20);
      this.boxCredits.TabIndex = 6;
      this.boxCredits.TextAlign = HorizontalAlignment.Right;
      this.tableContainer1.Controls.Add((Control) this.gridDebit);
      this.tableContainer1.Controls.Add((Control) this.boxDebits);
      this.tableContainer1.Controls.Add((Control) this.label2);
      this.tableContainer1.Location = new Point(3, 3);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(324, 392);
      this.tableContainer1.TabIndex = 4;
      this.tableContainer1.Text = "Debits";
      this.gridDebit.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Debit Description";
      gvColumn1.Width = 250;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Amount";
      gvColumn2.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn2.Width = 72;
      this.gridDebit.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridDebit.Dock = DockStyle.Fill;
      this.gridDebit.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gridDebit.Location = new Point(1, 26);
      this.gridDebit.Name = "gridDebit";
      this.gridDebit.Size = new Size(322, 340);
      this.gridDebit.TabIndex = 7;
      this.tableContainer2.Controls.Add((Control) this.gridCredit);
      this.tableContainer2.Controls.Add((Control) this.label5);
      this.tableContainer2.Controls.Add((Control) this.boxCredits);
      this.tableContainer2.Location = new Point(329, 3);
      this.tableContainer2.Name = "tableContainer2";
      this.tableContainer2.Size = new Size(324, 392);
      this.tableContainer2.TabIndex = 5;
      this.tableContainer2.Text = "Credits";
      this.gridCredit.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "";
      gvColumn3.Width = 45;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.Text = "Credit Description";
      gvColumn4.Width = 200;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.SpringToFit = true;
      gvColumn5.Text = "Amount";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 77;
      this.gridCredit.Columns.AddRange(new GVColumn[3]
      {
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gridCredit.Dock = DockStyle.Fill;
      this.gridCredit.Font = new Font("Arial", 8.25f);
      this.gridCredit.Location = new Point(1, 26);
      this.gridCredit.Name = "gridCredit";
      this.gridCredit.Size = new Size(322, 340);
      this.gridCredit.TabIndex = 10;
      this.Controls.Add((Control) this.tableContainer2);
      this.Controls.Add((Control) this.tableContainer1);
      this.Name = nameof (LoanBalancingWorksheet);
      this.Size = new Size(682, 420);
      this.tableContainer1.ResumeLayout(false);
      this.tableContainer1.PerformLayout();
      this.tableContainer2.ResumeLayout(false);
      this.tableContainer2.PerformLayout();
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      double num1 = 0.0;
      this.gridDebit.Items.Clear();
      this.gridDebit.BeginUpdate();
      if (this.loan.Use2015RESPA)
      {
        double num2 = Utils.ParseDouble((object) this.loan.GetField("4083"));
        if (num2 > 0.0)
        {
          this.gridDebit.Items.Add(new GVItem("Lender Credits")
          {
            SubItems = {
              (object) num2.ToString("N2")
            }
          });
          num1 += num2;
        }
      }
      double num3 = Utils.ParseDouble((object) this.loan.GetField("2"));
      this.gridDebit.Items.Add(new GVItem("Total Loan Amount")
      {
        SubItems = {
          (object) num3.ToString("N2")
        }
      });
      double amount = num1 + num3;
      this.gridDebit.EndUpdate();
      this.boxDebits.Text = amount.ToString("N2");
      string empty = string.Empty;
      double num4 = 0.0;
      this.gridCredit.Items.Clear();
      this.gridCredit.BeginUpdate();
      bool flag1 = this.loan.GetField("NEWHUD.X1139") == "Y";
      bool flag2 = this.loan.GetField("LE2.X28") == "Y";
      List<GFEItem> gfeItemList = this.loan.Use2010RESPA || this.loan.Use2015RESPA ? GFEItemCollection.GFEItems2010 : GFEItemCollection.GFEItems;
      for (int index = 0; index < gfeItemList.Count; ++index)
      {
        GFEItem gfeItemObject = gfeItemList[index];
        if ((this.loan.Use2015RESPA || gfeItemObject.LineNumber >= 100 && gfeItemObject.LineNumber != 701 && gfeItemObject.LineNumber != 702 && gfeItemObject.LineNumber != 703 && gfeItemObject.LineNumber != 704) && (!this.loan.Use2015RESPA || !GFEItemCollection.Excluded2015GFEItem.Contains(gfeItemObject.LineNumber)) && (!(this.loan.Use2015RESPA & flag2) || gfeItemObject.LineNumber >= 520 || gfeItemObject.LineNumber >= 21 && gfeItemObject.LineNumber <= 45) && (!this.loan.Use2015RESPA || flag2 || gfeItemObject.LineNumber < 21 || gfeItemObject.LineNumber > 45) && this.loan.Calculator.CalcFundingBalancingWorksheet((object) gfeItemObject, ref empty, ref amount))
        {
          GVItem gvItem;
          if (gfeItemObject.LineNumber == 802 && (this.loan.Use2010RESPA || this.loan.Use2015RESPA) && this.loan.GetField("NEWHUD.X713") == "Origination Charge")
          {
            if (flag1)
            {
              GFEItem gfeItem = gfeItemObject.Clone();
              gfeItem.LineNumber = 801;
              if (gfeItem.ComponentID == "a")
                gfeItem.ComponentID = "s";
              if (gfeItem.ComponentID == "b")
                gfeItem.ComponentID = "t";
              if (gfeItem.ComponentID == "c")
                gfeItem.ComponentID = "u";
              if (gfeItem.ComponentID == "d")
                gfeItem.ComponentID = "v";
              if (gfeItem.ComponentID == "e")
                gfeItem.ComponentID = "w";
              if (gfeItem.ComponentID == "f")
                gfeItem.ComponentID = "x";
              if (gfeItem.ComponentID == "g")
                gfeItem.ComponentID = "y";
              if (gfeItem.ComponentID == "h")
                gfeItem.ComponentID = "z";
              gvItem = new GVItem(gfeItem.LineNumber.ToString() + gfeItem.ComponentID + ".");
              if (gfeItem.LineNumber == 802 && gfeItem.ComponentID == "e" && this.loan.Use2015RESPA)
                gvItem.SubItems.Add((object) (this.loan.GetField("NEWHUD2.X928") + " % of Loan Amount (Points)"));
              else if (gfeItem.Description.Length <= 4 || gfeItem.Description.StartsWith("NEWHUD.X") || gfeItem.Description.StartsWith("CD3.X"))
                gvItem.SubItems.Add((object) this.loan.GetField(gfeItem.Description));
              else
                gvItem.SubItems.Add((object) gfeItem.Description);
            }
            else
            {
              gvItem = new GVItem("801s.");
              if (this.loan.GetField("NEWHUD.X715") == "Include Origination Credit")
                gvItem.SubItems.Add((object) "Origination Credit");
              else
                gvItem.SubItems.Add((object) "Origination Points");
            }
          }
          else
          {
            gvItem = !this.loan.Use2015RESPA || gfeItemObject.LineNumber >= 520 ? new GVItem(gfeItemObject.LineNumber.ToString() + gfeItemObject.ComponentID + ".") : new GVItem(FundingFee.GetCDLineID(gfeItemObject.LineNumber));
            if (gfeItemObject.LineNumber == 802 && gfeItemObject.ComponentID == "e" && this.loan.Use2015RESPA)
              gvItem.SubItems.Add((object) (this.loan.GetField("NEWHUD2.X928") + " % of Loan Amount (Points)"));
            else if (gfeItemObject.Description.Length <= 4 || gfeItemObject.Description.StartsWith("NEWHUD.X") || gfeItemObject.Description.StartsWith("NEWHUD2.X") || gfeItemObject.Description.StartsWith("CD3.X"))
              gvItem.SubItems.Add((object) this.loan.GetField(gfeItemObject.Description));
            else if (this.loan.Use2015RESPA)
            {
              string taxStampIndicator = "";
              if (gfeItemObject.LineNumber == 1204 || gfeItemObject.LineNumber == 1205)
                taxStampIndicator = gfeItemObject.LineNumber == 1204 ? this.loan.GetField("4855") : this.loan.GetField("4856");
              string newFeeDescription = UCDXmlExporterBase.GetNewFeeDescription(gfeItemObject.LineNumber, gfeItemObject.Description, taxStampIndicator);
              gvItem.SubItems.Add((object) FundingFee.GetFeeDescription2015(gfeItemObject.LineNumber.ToString() + gfeItemObject.ComponentID + ".", newFeeDescription));
            }
            else
              gvItem.SubItems.Add((object) gfeItemObject.Description);
          }
          gvItem.SubItems.Add((object) amount.ToString("N2"));
          this.gridCredit.Items.Add(gvItem);
          num4 += amount;
        }
      }
      amount = Utils.ParseDouble((object) this.loan.GetField("1990"));
      if (amount != 0.0)
      {
        this.gridCredit.Items.Add(new GVItem("")
        {
          SubItems = {
            (object) "Wire Transfer Amount",
            (object) amount.ToString("N2")
          }
        });
        num4 += amount;
      }
      this.gridCredit.EndUpdate();
      this.boxCredits.Text = num4.ToString("N2");
    }

    public string GetHelpTargetName() => nameof (LoanBalancingWorksheet);

    public void RefreshContents() => this.initForm();

    public void RefreshLoanContents() => this.RefreshContents();
  }
}
