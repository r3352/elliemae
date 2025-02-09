// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.GovernmentInfoInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class GovernmentInfoInputHandler
  {
    private LoanData loan;
    private IHtmlInput inputData;
    private Dictionary<string, RuntimeControl> pjtPanels = new Dictionary<string, RuntimeControl>();
    private string handlerName = "";
    private bool f1529;
    private bool f1537;
    private bool f4205;
    private bool f4206;
    private bool f4252;
    private bool f4253;
    private Form currentForm;
    private Panel Panel2018DIInstructions;
    private Panel panelBorrowerDemo;
    private Panel panelCoBorrowerDemo;
    private Panel panelBorrowerEthnicity;
    private Panel panelCoBorrowerEthnicity;
    private Panel panelBorrowerFaceToFace;
    private Panel panelCoBorrowerFaceToFace;
    private HorizontalRule borrowerHDMAHZR;
    private HorizontalRule coBorrowerHDMAHZR;
    private Panel panelForm;
    private Panel panelBorrowerCoBorrower;
    private Label formLabel;
    private Panel borrower2008;
    private CategoryBox categoryInfoForGovernment;
    private GroupBox GroupBoxLoanOriginator;
    private CategoryBox categoryBoxLoanOriginator;
    private GroupBox groupBoxInfoForGovernment;
    private GroupBox groupBoxPoolLoans;
    private CategoryBox categoryBoxFannieMae;
    private Panel panelLoanOriginationInfo;
    private VerticalRule borrowerCoBorrowerVerticalRule;

    public GovernmentInfoInputHandler(Form currentForm, IHtmlInput inputData, string handlerName)
    {
      this.inputData = inputData;
      this.handlerName = handlerName;
      if (inputData is LoanData)
        this.loan = (LoanData) inputData;
      this.currentForm = currentForm;
      this.panelBorrowerDemo = (Panel) this.currentForm.FindControl(nameof (panelBorrowerDemo));
      this.panelCoBorrowerDemo = (Panel) this.currentForm.FindControl(nameof (panelCoBorrowerDemo));
      this.panelBorrowerEthnicity = (Panel) this.currentForm.FindControl(nameof (panelBorrowerEthnicity));
      this.panelCoBorrowerEthnicity = (Panel) this.currentForm.FindControl(nameof (panelCoBorrowerEthnicity));
      this.panelBorrowerFaceToFace = (Panel) this.currentForm.FindControl(nameof (panelBorrowerFaceToFace));
      this.panelCoBorrowerFaceToFace = (Panel) this.currentForm.FindControl(nameof (panelCoBorrowerFaceToFace));
      this.borrowerHDMAHZR = (HorizontalRule) this.currentForm.FindControl(nameof (borrowerHDMAHZR));
      this.coBorrowerHDMAHZR = (HorizontalRule) this.currentForm.FindControl(nameof (coBorrowerHDMAHZR));
      this.panelForm = (Panel) this.currentForm.FindControl("pnlForm");
      this.panelBorrowerCoBorrower = (Panel) this.currentForm.FindControl(nameof (panelBorrowerCoBorrower));
      this.formLabel = (Label) this.currentForm.FindControl(nameof (formLabel));
      this.borrower2008 = (Panel) this.currentForm.FindControl("Panel2008BorrowerCoBorrower");
      this.categoryInfoForGovernment = (CategoryBox) this.currentForm.FindControl(nameof (categoryInfoForGovernment));
      this.GroupBoxLoanOriginator = (GroupBox) this.currentForm.FindControl(nameof (GroupBoxLoanOriginator));
      this.categoryBoxLoanOriginator = (CategoryBox) this.currentForm.FindControl(nameof (categoryBoxLoanOriginator));
      this.categoryBoxFannieMae = (CategoryBox) this.currentForm.FindControl(nameof (categoryBoxFannieMae));
      this.panelLoanOriginationInfo = (Panel) this.currentForm.FindControl(nameof (panelLoanOriginationInfo));
      this.borrowerCoBorrowerVerticalRule = (VerticalRule) this.currentForm.FindControl(nameof (borrowerCoBorrowerVerticalRule));
      this.Panel2018DIInstructions = (Panel) this.currentForm.FindControl(nameof (Panel2018DIInstructions));
      this.groupBoxInfoForGovernment = (GroupBox) this.currentForm.FindControl(nameof (groupBoxInfoForGovernment));
      this.groupBoxPoolLoans = (GroupBox) this.currentForm.FindControl(nameof (groupBoxPoolLoans));
    }

    public ControlState GetControlState(string id, ControlState currentState) => currentState;

    public void UpdateFieldValue(string id, string val)
    {
    }

    public string GetFieldValue(string id, string val) => string.Empty;

    public void SetSectionStatus(string id)
    {
    }

    private void setControlStateForInvisibleForm(string fieldId, string fieldVal, string controlId)
    {
      if (this.inputData.GetField(fieldId) == fieldVal)
      {
        this.SetControlState(controlId, false);
        this.SetControlState(controlId, true);
      }
      else
      {
        this.SetControlState(controlId, true);
        this.SetControlState(controlId, false);
      }
    }

    private void setControlStateForInvisibleForm(string fieldId, string controlId)
    {
      bool flag = false;
      switch (fieldId)
      {
        case "4205":
          flag = this.f4205 && this.inputData.GetField("4143") == "FaceToFace";
          break;
        case "4206":
          flag = this.f4206 && this.inputData.GetField("4131") == "FaceToFace";
          break;
        case "1529":
          flag = this.f1529 && this.inputData.GetField("4143") == "FaceToFace";
          break;
        case "1537":
          flag = this.f1537 && this.inputData.GetField("4131") == "FaceToFace";
          break;
        case "4252":
          flag = this.f4252 && this.inputData.GetField("4143") == "FaceToFace";
          break;
        case "4253":
          flag = this.f4253 && this.inputData.GetField("4131") == "FaceToFace";
          break;
      }
      if (flag)
        this.SetControlState(controlId, false);
      else
        this.SetControlState(controlId, true);
    }

    private void SetControlState(string controlID, bool enabled)
    {
      if (this.currentForm == null || !(this.currentForm.FindControl(controlID) is RuntimeControl control))
        return;
      control.Enabled = enabled;
    }

    public void SetLayout(InputHandlerBase inputHandler)
    {
      if (this.formLabel == null)
        return;
      Size size1 = new Size(0, 0);
      Size size2;
      Point position;
      if (this.inputData.GetField("4142") == "Y")
      {
        if (this.borrower2008 != null)
          this.borrower2008.Visible = false;
        if (this.panelBorrowerCoBorrower != null)
          this.panelBorrowerCoBorrower.Visible = true;
        switch (this.formLabel.Text)
        {
          case "D10033":
            if (this.categoryInfoForGovernment != null && this.GroupBoxLoanOriginator != null)
            {
              ref Size local = ref size1;
              size2 = this.panelBorrowerCoBorrower.Size;
              int height1 = 1578 + size2.Height - 33;
              local = new Size(592, height1);
              this.Panel2018DIInstructions.Position = new Point(this.borrower2008.Position.X, this.borrower2008.Position.Y);
              this.Panel2018DIInstructions.Visible = true;
              Panel borrowerCoBorrower = this.panelBorrowerCoBorrower;
              int x1 = this.Panel2018DIInstructions.Position.X;
              int y1 = this.Panel2018DIInstructions.Position.Y;
              size2 = this.Panel2018DIInstructions.Size;
              int height2 = size2.Height;
              int y2 = y1 + height2 + 10;
              Point point1 = new Point(x1, y2);
              borrowerCoBorrower.Position = point1;
              GroupBox boxLoanOriginator = this.GroupBoxLoanOriginator;
              position = this.GroupBoxLoanOriginator.Position;
              int x2 = position.X;
              position = this.borrower2008.Position;
              int y3 = position.Y;
              size2 = this.Panel2018DIInstructions.Size;
              int height3 = size2.Height;
              int num = y3 + height3;
              size2 = this.panelBorrowerCoBorrower.Size;
              int height4 = size2.Height;
              int y4 = num + height4 + 10;
              Point point2 = new Point(x2, y4);
              boxLoanOriginator.Position = point2;
              break;
            }
            break;
          case "D1003_2020P4":
            size1 = new Size(695, 4265);
            break;
          case "HMDA2018_ORIGINATED":
            if (this.categoryInfoForGovernment != null)
            {
              size1 = new Size(591, (this.formLabel.Text == "HMDA2018_ORIGINATED" ? 2126 : 1300) + (this.formLabel.Text == "HMDA2018_ORIGINATED" ? this.panelBorrowerCoBorrower.Size.Height - 100 : this.panelBorrowerCoBorrower.Size.Height - 25));
              Panel panel2018DiInstructions = this.Panel2018DIInstructions;
              position = this.borrower2008.Position;
              int x3 = position.X;
              position = this.borrower2008.Position;
              int y5 = position.Y;
              Point point3 = new Point(x3, y5);
              panel2018DiInstructions.Position = point3;
              this.Panel2018DIInstructions.Visible = true;
              Panel borrowerCoBorrower = this.panelBorrowerCoBorrower;
              position = this.Panel2018DIInstructions.Position;
              int x4 = position.X;
              position = this.Panel2018DIInstructions.Position;
              int y6 = position.Y + this.Panel2018DIInstructions.Size.Height + 10;
              Point point4 = new Point(x4, y6);
              borrowerCoBorrower.Position = point4;
              break;
            }
            break;
          case "HMDA2018_PURCHASED":
            if (this.categoryInfoForGovernment != null)
            {
              size1 = new Size(591, (this.formLabel.Text == "HMDA2018_ORIGINATED" ? 2126 : 1447) + (this.formLabel.Text == "HMDA2018_ORIGINATED" ? this.panelBorrowerCoBorrower.Size.Height - 13 : this.panelBorrowerCoBorrower.Size.Height - 25));
              Panel panel2018DiInstructions = this.Panel2018DIInstructions;
              position = this.borrower2008.Position;
              int x5 = position.X;
              position = this.borrower2008.Position;
              int y7 = position.Y;
              Point point5 = new Point(x5, y7);
              panel2018DiInstructions.Position = point5;
              this.Panel2018DIInstructions.Visible = true;
              Panel borrowerCoBorrower = this.panelBorrowerCoBorrower;
              position = this.Panel2018DIInstructions.Position;
              int x6 = position.X;
              position = this.Panel2018DIInstructions.Position;
              int y8 = position.Y + this.Panel2018DIInstructions.Size.Height + 10;
              Point point6 = new Point(x6, y8);
              borrowerCoBorrower.Position = point6;
              break;
            }
            break;
          case "HMDA2018_REPURCHASED":
            if (this.categoryInfoForGovernment != null)
            {
              size1 = new Size(591, (this.formLabel.Text == "HMDA2018_ORIGINATED" ? 2126 : 1390) + (this.formLabel.Text == "HMDA2018_ORIGINATED" ? this.panelBorrowerCoBorrower.Size.Height - 100 : this.panelBorrowerCoBorrower.Size.Height - 25));
              Panel panel2018DiInstructions = this.Panel2018DIInstructions;
              position = this.borrower2008.Position;
              int x7 = position.X;
              position = this.borrower2008.Position;
              int y9 = position.Y;
              Point point7 = new Point(x7, y9);
              panel2018DiInstructions.Position = point7;
              this.Panel2018DIInstructions.Visible = true;
              Panel borrowerCoBorrower = this.panelBorrowerCoBorrower;
              position = this.Panel2018DIInstructions.Position;
              int x8 = position.X;
              position = this.Panel2018DIInstructions.Position;
              int y10 = position.Y + this.Panel2018DIInstructions.Size.Height + 10;
              Point point8 = new Point(x8, y10);
              borrowerCoBorrower.Position = point8;
              break;
            }
            break;
          case "HMDA_DENIAL04":
            if (this.categoryInfoForGovernment != null)
            {
              ref Size local = ref size1;
              size2 = this.panelBorrowerCoBorrower.Size;
              int num = 993 + size2.Height;
              size2 = this.borrower2008.Size;
              int height5 = size2.Height;
              int height6 = num - height5;
              local = new Size(588, height6);
              Panel borrowerCoBorrower = this.panelBorrowerCoBorrower;
              position = this.borrower2008.Position;
              int x = position.X;
              position = this.borrower2008.Position;
              int y = position.Y;
              Point point = new Point(x, y);
              borrowerCoBorrower.Position = point;
              break;
            }
            break;
          case "STREAMLINED 1003":
            if (this.categoryInfoForGovernment != null && this.categoryBoxFannieMae != null && this.panelLoanOriginationInfo != null)
            {
              ref Size local = ref size1;
              int num1 = this.categoryBoxFannieMae.Position.Y + this.categoryBoxFannieMae.Size.Height + 80;
              size2 = this.panelBorrowerCoBorrower.Size;
              int height7 = size2.Height;
              int num2 = num1 + height7;
              size2 = this.borrower2008.Size;
              int height8 = size2.Height;
              int height9 = num2 - height8;
              local = new Size(591, height9);
              Panel borrowerCoBorrower = this.panelBorrowerCoBorrower;
              position = this.borrower2008.Position;
              int x = position.X;
              position = this.borrower2008.Position;
              int y = position.Y;
              Point point9 = new Point(x, y);
              borrowerCoBorrower.Position = point9;
              CategoryBox categoryBoxFannieMae = this.categoryBoxFannieMae;
              size2 = this.panelBorrowerCoBorrower.Size;
              int num3 = 3300 + size2.Height;
              size2 = this.borrower2008.Size;
              int height10 = size2.Height;
              Point point10 = new Point(0, num3 - height10);
              categoryBoxFannieMae.Position = point10;
              Panel loanOriginationInfo = this.panelLoanOriginationInfo;
              size2 = this.panelBorrowerCoBorrower.Size;
              int num4 = 263 + size2.Height;
              size2 = this.borrower2008.Size;
              int height11 = size2.Height;
              Point point11 = new Point(0, num4 - height11);
              loanOriginationInfo.Position = point11;
              break;
            }
            break;
          case "ULDD_FANNIEMAE":
            if (this.groupBoxInfoForGovernment != null && this.groupBoxPoolLoans != null)
            {
              ref Size local = ref size1;
              size2 = this.panelBorrowerCoBorrower.Size;
              int num5 = 8168 + size2.Height;
              size2 = this.borrower2008.Size;
              int height12 = size2.Height;
              int height13 = num5 - height12 + 20;
              local = new Size(670, height13);
              Panel borrowerCoBorrower = this.panelBorrowerCoBorrower;
              int x = this.borrower2008.Position.X;
              position = this.borrower2008.Position;
              int y = position.Y;
              Point point12 = new Point(x, y);
              borrowerCoBorrower.Position = point12;
              GroupBox groupBoxPoolLoans = this.groupBoxPoolLoans;
              size2 = this.panelBorrowerCoBorrower.Size;
              int num6 = 7558 + size2.Height;
              size2 = this.borrower2008.Size;
              int height14 = size2.Height;
              Point point13 = new Point(1, num6 - height14);
              groupBoxPoolLoans.Position = point13;
              break;
            }
            break;
          case "ULDD_FREDDIEMAC":
            if (this.groupBoxInfoForGovernment != null)
            {
              size1 = new Size(670, this.groupBoxInfoForGovernment.Top + this.panelBorrowerCoBorrower.Size.Height + 26);
              Panel borrowerCoBorrower = this.panelBorrowerCoBorrower;
              position = this.borrower2008.Position;
              int x = position.X;
              position = this.borrower2008.Position;
              int y = position.Y;
              Point point = new Point(x, y);
              borrowerCoBorrower.Position = point;
              break;
            }
            break;
          case "USDA_RURAL5":
            if (this.panelLoanOriginationInfo != null)
            {
              size1 = new Size(588, 1340);
              this.panelBorrowerCoBorrower.Position = new Point(0, 0);
              this.panelLoanOriginationInfo.Position = new Point(0, 800);
              break;
            }
            break;
        }
        this.panelForm.Size = new Size(size1.Width, size1.Height);
        if (this.inputData.GetField("4143") == "FaceToFace" || this.inputData.GetField("4131") == "FaceToFace" || this.formLabel.Text.Contains("HMDA") || this.formLabel.Text.Equals("D10033") || this.formLabel.Text.Equals("D1003_2020P4"))
        {
          Panel panelForm = this.panelForm;
          int width = size1.Width;
          int height15 = size1.Height;
          size2 = this.panelBorrowerFaceToFace.Size;
          int height16 = size2.Height;
          int height17 = height15 + height16;
          Size size3 = new Size(width, height17);
          panelForm.Size = size3;
          if (this.formLabel.Text.Contains("HMDA"))
            this.borrowerCoBorrowerVerticalRule.Size = new Size(3, 1260);
          else
            this.borrowerCoBorrowerVerticalRule.Size = new Size(3, 1183);
        }
        else
        {
          switch (this.formLabel.Text)
          {
            case "ULDD_FANNIEMAE":
              GroupBox groupBoxPoolLoans1 = this.groupBoxPoolLoans;
              size2 = this.panelBorrowerCoBorrower.Size;
              int num7 = 7558 + size2.Height;
              size2 = this.borrower2008.Size;
              int height18 = size2.Height;
              Point point14 = new Point(1, num7 - height18);
              groupBoxPoolLoans1.Position = point14;
              this.borrowerCoBorrowerVerticalRule.Size = new Size(3, 1000);
              break;
            case "STREAMLINED 1003":
              CategoryBox categoryBoxFannieMae1 = this.categoryBoxFannieMae;
              size2 = this.panelBorrowerCoBorrower.Size;
              int num8 = 3300 + size2.Height;
              size2 = this.borrower2008.Size;
              int height19 = size2.Height;
              Point point15 = new Point(0, num8 - height19);
              categoryBoxFannieMae1.Position = point15;
              Panel loanOriginationInfo1 = this.panelLoanOriginationInfo;
              size2 = this.panelBorrowerCoBorrower.Size;
              int num9 = 263 + size2.Height;
              size2 = this.borrower2008.Size;
              int height20 = size2.Height;
              Point point16 = new Point(0, num9 - height20);
              loanOriginationInfo1.Position = point16;
              break;
            case "USDA_RURAL5":
              Panel loanOriginationInfo2 = this.panelLoanOriginationInfo;
              position = this.panelLoanOriginationInfo.Position;
              Point point17 = new Point(position.X, 1009);
              loanOriginationInfo2.Position = point17;
              break;
          }
        }
        if (this.inputData.GetField("4143") == "FaceToFace" || this.formLabel.Text.Contains("HMDA") || this.formLabel.Text.Equals("D10033") || this.formLabel.Text.Equals("D1003_2020P4"))
        {
          this.panelBorrowerFaceToFace.Visible = true;
          this.panelBorrowerFaceToFace.Position = new Point(5, 66);
          this.panelBorrowerDemo.Position = new Point(5, 266);
          this.panelBorrowerEthnicity.Position = new Point(5, 339);
          this.borrowerHDMAHZR.Position = new Point(-1, 334);
          switch (this.formLabel.Text)
          {
            case "D10033":
              GroupBox boxLoanOriginator1 = this.GroupBoxLoanOriginator;
              position = this.GroupBoxLoanOriginator.Position;
              int x9 = position.X;
              position = this.borrower2008.Position;
              int y11 = position.Y;
              size2 = this.Panel2018DIInstructions.Size;
              int height21 = size2.Height;
              int num10 = y11 + height21;
              size2 = this.panelBorrowerCoBorrower.Size;
              int height22 = size2.Height;
              int num11 = num10 + height22 + 10;
              size2 = this.panelBorrowerFaceToFace.Size;
              int height23 = size2.Height;
              int y12 = num11 + height23;
              Point point18 = new Point(x9, y12);
              boxLoanOriginator1.Position = point18;
              break;
            case "D1003_2020P4":
              if (this.categoryBoxLoanOriginator != null)
              {
                CategoryBox boxLoanOriginator2 = this.categoryBoxLoanOriginator;
                position = this.categoryBoxLoanOriginator.Position;
                int x10 = position.X;
                size2 = this.panelBorrowerFaceToFace.Size;
                int y13 = 3885 + size2.Height;
                Point point19 = new Point(x10, y13);
                boxLoanOriginator2.Position = point19;
                break;
              }
              break;
            case "ULDD_FANNIEMAE":
              if (this.groupBoxInfoForGovernment != null && this.groupBoxPoolLoans != null)
              {
                Panel borrowerCoBorrower = this.panelBorrowerCoBorrower;
                position = this.borrower2008.Position;
                int x11 = position.X;
                position = this.borrower2008.Position;
                int y14 = position.Y;
                Point point20 = new Point(x11, y14);
                borrowerCoBorrower.Position = point20;
                GroupBox groupBoxPoolLoans2 = this.groupBoxPoolLoans;
                size2 = this.panelBorrowerCoBorrower.Size;
                int num12 = 7558 + size2.Height;
                size2 = this.borrower2008.Size;
                int height24 = size2.Height;
                int num13 = num12 - height24;
                size2 = this.panelBorrowerFaceToFace.Size;
                int height25 = size2.Height;
                Point point21 = new Point(1, num13 + height25);
                groupBoxPoolLoans2.Position = point21;
                break;
              }
              break;
            case "STREAMLINED 1003":
              if (this.categoryInfoForGovernment != null && this.categoryBoxFannieMae != null && this.panelLoanOriginationInfo != null)
              {
                CategoryBox categoryBoxFannieMae2 = this.categoryBoxFannieMae;
                size2 = this.panelBorrowerCoBorrower.Size;
                int num14 = 3300 + size2.Height;
                size2 = this.borrower2008.Size;
                int height26 = size2.Height;
                int num15 = num14 - height26;
                size2 = this.panelBorrowerFaceToFace.Size;
                int height27 = size2.Height;
                Point point22 = new Point(0, num15 + height27);
                categoryBoxFannieMae2.Position = point22;
                Panel loanOriginationInfo3 = this.panelLoanOriginationInfo;
                size2 = this.panelBorrowerCoBorrower.Size;
                int num16 = 263 + size2.Height;
                size2 = this.borrower2008.Size;
                int height28 = size2.Height;
                int num17 = num16 - height28;
                size2 = this.panelBorrowerFaceToFace.Size;
                int height29 = size2.Height;
                Point point23 = new Point(0, num17 + height29);
                loanOriginationInfo3.Position = point23;
                break;
              }
              break;
            case "USDA_RURAL5":
              Panel loanOriginationInfo4 = this.panelLoanOriginationInfo;
              position = this.panelLoanOriginationInfo.Position;
              int x12 = position.X;
              size2 = this.panelBorrowerFaceToFace.Size;
              int y15 = 1009 + size2.Height;
              Point point24 = new Point(x12, y15);
              loanOriginationInfo4.Position = point24;
              break;
          }
        }
        else
        {
          if (this.panelBorrowerFaceToFace != null)
            this.panelBorrowerFaceToFace.Visible = false;
          if (this.panelBorrowerDemo != null)
            this.panelBorrowerDemo.Position = new Point(6, 66);
          if (this.panelBorrowerEthnicity != null)
            this.panelBorrowerEthnicity.Position = new Point(6, 159);
          if (this.borrowerHDMAHZR != null)
            this.borrowerHDMAHZR.Position = new Point(-1, 146);
        }
        if (this.inputData.GetField("4131") == "FaceToFace" || this.formLabel.Text.Contains("HMDA") || this.formLabel.Text.Equals("D10033") || this.formLabel.Text.Equals("D1003_2020P4"))
        {
          this.panelCoBorrowerFaceToFace.Visible = true;
          this.panelCoBorrowerFaceToFace.Position = new Point(5, 66);
          this.panelCoBorrowerDemo.Position = new Point(5, 266);
          this.panelCoBorrowerEthnicity.Position = new Point(3, 339);
          this.coBorrowerHDMAHZR.Position = new Point(0, 334);
          switch (this.formLabel.Text)
          {
            case "D10033":
              GroupBox boxLoanOriginator = this.GroupBoxLoanOriginator;
              position = this.GroupBoxLoanOriginator.Position;
              int x13 = position.X;
              position = this.borrower2008.Position;
              int y16 = position.Y;
              size2 = this.Panel2018DIInstructions.Size;
              int height30 = size2.Height;
              int num18 = y16 + height30;
              size2 = this.panelBorrowerCoBorrower.Size;
              int height31 = size2.Height;
              int num19 = num18 + height31 + 10;
              size2 = this.panelBorrowerFaceToFace.Size;
              int height32 = size2.Height;
              int y17 = num19 + height32;
              Point point25 = new Point(x13, y17);
              boxLoanOriginator.Position = point25;
              break;
            case "ULDD_FANNIEMAE":
              if (this.groupBoxInfoForGovernment != null && this.groupBoxPoolLoans != null)
              {
                Panel borrowerCoBorrower = this.panelBorrowerCoBorrower;
                position = this.borrower2008.Position;
                int x14 = position.X;
                position = this.borrower2008.Position;
                int y18 = position.Y;
                Point point26 = new Point(x14, y18);
                borrowerCoBorrower.Position = point26;
                GroupBox groupBoxPoolLoans3 = this.groupBoxPoolLoans;
                size2 = this.panelBorrowerCoBorrower.Size;
                int num20 = 7558 + size2.Height;
                size2 = this.borrower2008.Size;
                int height33 = size2.Height;
                int num21 = num20 - height33;
                size2 = this.panelBorrowerFaceToFace.Size;
                int height34 = size2.Height;
                Point point27 = new Point(1, num21 + height34);
                groupBoxPoolLoans3.Position = point27;
                break;
              }
              break;
            case "STREAMLINED 1003":
              if (this.categoryInfoForGovernment != null && this.categoryBoxFannieMae != null && this.panelLoanOriginationInfo != null)
              {
                CategoryBox categoryBoxFannieMae3 = this.categoryBoxFannieMae;
                size2 = this.panelBorrowerCoBorrower.Size;
                int num22 = 3300 + size2.Height;
                size2 = this.borrower2008.Size;
                int height35 = size2.Height;
                int num23 = num22 - height35;
                size2 = this.panelBorrowerFaceToFace.Size;
                int height36 = size2.Height;
                Point point28 = new Point(0, num23 + height36);
                categoryBoxFannieMae3.Position = point28;
                Panel loanOriginationInfo5 = this.panelLoanOriginationInfo;
                size2 = this.panelBorrowerCoBorrower.Size;
                int num24 = 263 + size2.Height;
                size2 = this.borrower2008.Size;
                int height37 = size2.Height;
                int num25 = num24 - height37;
                size2 = this.panelBorrowerFaceToFace.Size;
                int height38 = size2.Height;
                Point point29 = new Point(0, num25 + height38);
                loanOriginationInfo5.Position = point29;
                break;
              }
              break;
            case "USDA_RURAL5":
              Panel loanOriginationInfo6 = this.panelLoanOriginationInfo;
              position = this.panelLoanOriginationInfo.Position;
              int x15 = position.X;
              size2 = this.panelBorrowerFaceToFace.Size;
              int y19 = 1009 + size2.Height;
              Point point30 = new Point(x15, y19);
              loanOriginationInfo6.Position = point30;
              break;
          }
        }
        else
        {
          if (this.panelCoBorrowerFaceToFace != null)
            this.panelCoBorrowerFaceToFace.Visible = false;
          if (this.panelCoBorrowerDemo != null)
            this.panelCoBorrowerDemo.Position = new Point(5, 66);
          if (this.panelCoBorrowerEthnicity != null)
            this.panelCoBorrowerEthnicity.Position = new Point(3, 159);
          if (this.coBorrowerHDMAHZR != null)
            this.coBorrowerHDMAHZR.Position = new Point(0, 146);
        }
        this.f1529 = this.inputData.GetField("1529") == "Y";
        this.f1537 = this.inputData.GetField("1537") == "Y";
        this.f4205 = this.inputData.GetField("4205") == "Y";
        this.f4206 = this.inputData.GetField("4206") == "Y";
        this.f4252 = this.inputData.GetField("4252") == "Y";
        this.f4253 = this.inputData.GetField("4253") == "Y";
        this.setControlStateForInvisibleForm("4205", "checkboxMexican");
        this.setControlStateForInvisibleForm("4205", "checkboxPuertoRican");
        this.setControlStateForInvisibleForm("4205", "checkboxCuban");
        this.setControlStateForInvisibleForm("4205", "checkboxOtherHispanic");
        this.setControlStateForInvisibleForm("4205", "textboxOtherHispanic");
        this.setControlStateForInvisibleForm("4206", "checkboxCoBorrowerMexican");
        this.setControlStateForInvisibleForm("4206", "checkboxCoBorrowerPuertoRican");
        this.setControlStateForInvisibleForm("4206", "checkboxCoBorrowerCuban");
        this.setControlStateForInvisibleForm("4206", "checkboxCoBorrowerOtherHispanic");
        this.setControlStateForInvisibleForm("4206", "textboxCoBorrowerOtherHispanic");
        this.setControlStateForInvisibleForm("4252", "textboxAmericanIndian");
        this.setControlStateForInvisibleForm("4253", "textboxCoBorrowerAmericanIndian");
        this.setControlStateForInvisibleForm("4252", "checkboxAsianIndian");
        this.setControlStateForInvisibleForm("4252", "checkboxChinese");
        this.setControlStateForInvisibleForm("4252", "checkboxFilipino");
        this.setControlStateForInvisibleForm("4252", "checkboxJapanese");
        this.setControlStateForInvisibleForm("4252", "checkboxKorean");
        this.setControlStateForInvisibleForm("4252", "checkboxVietnamese");
        this.setControlStateForInvisibleForm("4252", "checkboxOtherAsian");
        this.setControlStateForInvisibleForm("4252", "textboxOtherAsian");
        this.setControlStateForInvisibleForm("4253", "checkboxCoBorrowerAsianIndian");
        this.setControlStateForInvisibleForm("4253", "checkboxCoBorrowerChinese");
        this.setControlStateForInvisibleForm("4253", "checkboxCoBorrowerFilipino");
        this.setControlStateForInvisibleForm("4253", "checkboxCoBorrowerJapanese");
        this.setControlStateForInvisibleForm("4253", "checkboxCoBorrowerKorean");
        this.setControlStateForInvisibleForm("4253", "checkboxCoBorrowerVietnamese");
        this.setControlStateForInvisibleForm("4253", "checkboxCoBorrowerOtherAsian");
        this.setControlStateForInvisibleForm("4253", "textboxCoBorrowerOtherAsian");
        this.setControlStateForInvisibleForm("4252", "checkboxHawaiian");
        this.setControlStateForInvisibleForm("4252", "checkboxGuam");
        this.setControlStateForInvisibleForm("4252", "checkboxSamoan");
        this.setControlStateForInvisibleForm("4252", "checkboxOtherPacific");
        this.setControlStateForInvisibleForm("4252", "textboxOtherPacific");
        this.setControlStateForInvisibleForm("4253", "checkboxCoBorrowerHawaiian");
        this.setControlStateForInvisibleForm("4253", "checkboxCoBorrowerGuam");
        this.setControlStateForInvisibleForm("4253", "checkboxCoBorrowerSamoan");
        this.setControlStateForInvisibleForm("4253", "checkboxCoBorrowerOtherPacific");
        this.setControlStateForInvisibleForm("4253", "textboxCoBorrowerOtherPacific");
      }
      else
      {
        if (this.borrower2008 != null)
          this.borrower2008.Visible = true;
        if (this.panelBorrowerCoBorrower != null)
          this.panelBorrowerCoBorrower.Visible = false;
        switch (this.formLabel.Text)
        {
          case "D10033":
            if (this.categoryInfoForGovernment != null && this.GroupBoxLoanOriginator != null)
            {
              this.Panel2018DIInstructions = (Panel) this.currentForm.FindControl("Panel2018DIInstructions");
              this.Panel2018DIInstructions.Visible = false;
              this.GroupBoxLoanOriginator.Position = new Point(0, 298);
              this.panelForm.Size = new Size(592, 1578);
              break;
            }
            break;
          case "HMDA2018_ORIGINATED":
          case "HMDA2018_PURCHASED":
          case "HMDA2018_REPURCHASED":
            if (this.categoryInfoForGovernment != null)
            {
              this.Panel2018DIInstructions = (Panel) this.currentForm.FindControl("Panel2018DIInstructions");
              this.Panel2018DIInstructions.Visible = false;
              Panel panelForm = this.panelForm;
              size2 = this.panelForm.Size;
              int width = size2.Width;
              int top = this.categoryInfoForGovernment.Top;
              size2 = this.categoryInfoForGovernment.Size;
              int height39 = size2.Height;
              int height40 = top + height39 + 2;
              Size size4 = new Size(width, height40);
              panelForm.Size = size4;
              break;
            }
            break;
          case "HMDA_DENIAL04":
            if (this.formLabel.Text == "HMDA_DENIAL04" && this.categoryInfoForGovernment != null)
            {
              this.panelForm.Size = new Size(588, 993);
              break;
            }
            break;
          case "STREAMLINED 1003":
            if (this.formLabel.Text == "STREAMLINED 1003" && this.categoryInfoForGovernment != null && this.categoryBoxFannieMae != null && this.panelLoanOriginationInfo != null)
            {
              this.categoryBoxFannieMae.Position = new Point(0, 3300);
              this.panelLoanOriginationInfo.Position = new Point(0, 263);
              this.panelForm.Size = new Size(591, this.categoryBoxFannieMae.Position.Y + this.categoryBoxFannieMae.Size.Height + 103);
              break;
            }
            break;
          case "ULDD_FANNIEMAE":
            if (this.groupBoxInfoForGovernment != null && this.groupBoxPoolLoans != null)
            {
              this.panelForm.Size = new Size(670, this.groupBoxInfoForGovernment.Top + this.groupBoxInfoForGovernment.Size.Height + 630);
              this.groupBoxPoolLoans.Position = new Point(1, 7575);
              break;
            }
            break;
          case "ULDD_FREDDIEMAC":
            if (this.groupBoxInfoForGovernment != null)
            {
              this.panelForm.Size = new Size(670, this.groupBoxInfoForGovernment.Top + this.groupBoxInfoForGovernment.Size.Height + 10);
              break;
            }
            break;
          case "USDA_RURAL5":
            if (this.formLabel.Text == "USDA_RURAL5")
            {
              this.panelForm.Size = new Size(588, 599);
              this.panelLoanOriginationInfo.Position = new Point(0, 258);
              break;
            }
            break;
        }
      }
      if (this.formLabel == null)
        return;
      try
      {
        Label formLabel = this.formLabel;
        position = this.formLabel.Position;
        int x = position.X;
        size2 = this.panelForm.Size;
        int y = size2.Height + 9;
        Point point = new Point(x, y);
        formLabel.Position = point;
      }
      catch (Exception ex)
      {
      }
    }
  }
}
