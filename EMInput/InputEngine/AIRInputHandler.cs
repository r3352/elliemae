// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AIRInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Forms;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AIRInputHandler
  {
    private LoanData loan;
    private IHtmlInput inputData;
    private Form currentForm;
    private CategoryBox categoryBoxAIR;
    private CategoryBox categoryBoxAIRforConstruction;
    private GroupBox groupAIR;
    private GroupBox constructionAir;
    private CategoryBox categoryBoxAP;
    private Panel formPanel;
    private VerticalRule apTableDivider;

    public AIRInputHandler(Form currentForm, IHtmlInput inputData)
    {
      this.inputData = inputData;
      if (inputData is LoanData)
        this.loan = (LoanData) inputData;
      this.currentForm = currentForm;
      try
      {
        this.categoryBoxAIR = (CategoryBox) this.currentForm.FindControl("table_AIR");
        this.categoryBoxAIRforConstruction = (CategoryBox) this.currentForm.FindControl("AIRforConstruction");
        this.groupAIR = (GroupBox) this.currentForm.FindControl("group_AIR");
        this.constructionAir = (GroupBox) this.currentForm.FindControl(nameof (constructionAir));
        this.categoryBoxAP = (CategoryBox) this.currentForm.FindControl("CategoryBox4");
        this.formPanel = (Panel) this.currentForm.FindControl("pnlForm");
        this.apTableDivider = (VerticalRule) this.currentForm.FindControl(nameof (apTableDivider));
      }
      catch (Exception ex)
      {
      }
    }

    public string GetFieldValue(string id, string val)
    {
      return id == "3" || id == "689" || id == "1699" || id == "NEWHUD.X555" || id == "697" || id == "695" || id == "2625" ? Utils.FormatLEAndCDPercentageValue(val) : val;
    }

    public void SetSectionStatus()
    {
      if (this.categoryBoxAIR != null)
      {
        this.categoryBoxAIR.Visible = this.inputData.GetField("608") == "AdjustableRate";
        this.categoryBoxAIRforConstruction.Visible = false;
      }
      if (this.groupAIR != null)
      {
        this.groupAIR.Visible = this.inputData.GetField("608") == "AdjustableRate";
        if (this.constructionAir != null)
          this.constructionAir.Visible = false;
      }
      if (this.categoryBoxAIRforConstruction != null && this.inputData.GetField("19") == "ConstructionToPermanent" && this.inputData.GetField("608") == "Fixed" && Utils.ToDouble(this.inputData.GetField("1677")) < Utils.ToDouble(this.inputData.GetField("3")))
      {
        this.categoryBoxAIR.Visible = false;
        this.categoryBoxAIRforConstruction.Visible = true;
        this.categoryBoxAIRforConstruction.Position = this.categoryBoxAIR.Position;
      }
      if (this.constructionAir != null && this.inputData.GetField("19") == "ConstructionToPermanent" && this.inputData.GetField("608") == "Fixed" && Utils.ToDouble(this.inputData.GetField("1677")) < Utils.ToDouble(this.inputData.GetField("3")))
      {
        this.groupAIR.Visible = false;
        this.constructionAir.Visible = true;
        this.constructionAir.Position = this.groupAIR.Position;
      }
      if (this.formPanel == null || this.categoryBoxAP == null || this.categoryBoxAIR == null || this.categoryBoxAIRforConstruction == null || this.apTableDivider == null)
        return;
      if (!this.categoryBoxAIR.Visible && !this.categoryBoxAIRforConstruction.Visible)
        this.categoryBoxAP.Size = new Size(this.formPanel.Size.Width - 2, this.categoryBoxAP.Size.Height);
      else
        this.categoryBoxAP.Size = new Size(365, this.categoryBoxAP.Size.Height);
      this.apTableDivider.Visible = this.categoryBoxAIR.Visible || this.categoryBoxAIRforConstruction.Visible;
    }
  }
}
