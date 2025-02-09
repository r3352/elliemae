// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.OtherIncomeControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class OtherIncomeControl : UserControl
  {
    private LoanData loan;
    private AIQIncomeData aiqIncomeData;
    private int loanOISIndex = -1;
    private bool forCoborrower;
    private int aiqOISIndex = -1;
    private GenericFormInputHandler aiqOtherIncomeForm;
    private Dictionary<string, int> encompassAIQIndexMapping = new Dictionary<string, int>();
    private BorrowerPair borrowerPairForOIS;
    public Control isSelectedControl;
    private IContainer components;
    private Panel panel1;
    private CheckBox isSelected;
    private Label otherIncomeLabel;
    private PictureBox pictureBox5;
    private TextBox textBox9;
    private TextBox textBox10;

    public OtherIncomeControl(
      int loanOISIndex,
      bool forCoborrower,
      LoanData loan,
      Sessions.Session session,
      int aiqOISIndex,
      AIQIncomeData aiqIncomeData,
      BorrowerPair borrowerPairForOIS)
    {
      this.loanOISIndex = loanOISIndex;
      this.forCoborrower = forCoborrower;
      this.loan = loan;
      this.aiqIncomeData = aiqIncomeData;
      this.aiqOISIndex = aiqOISIndex;
      this.borrowerPairForOIS = borrowerPairForOIS;
      this.InitializeComponent();
      this.aiqOtherIncomeForm = new GenericFormInputHandler((IHtmlInput) loan, this.Controls, session);
      this.isSelectedControl = (Control) this.isSelected;
      this.initForm();
    }

    private void initForm()
    {
      this.aiqOtherIncomeForm.SetVeriFieldIDinTag("URLAROIS", "URLAROIS", this.loanOISIndex);
      string borrowerKey = this.forCoborrower ? "C" : "B";
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (borrowerPairs[index].Id == this.borrowerPairForOIS.Id)
        {
          borrowerKey = (index + 1).ToString() + borrowerKey;
          break;
        }
      }
      AIQOtherIncomeData otherIncome = this.aiqIncomeData.getOtherIncomeList(borrowerKey)[this.aiqOISIndex];
      this.aiqOtherIncomeForm.SetAIQValuesToForm(false, "OTHERINCOME", aiqOtherIncomeData: otherIncome);
      this.matchAndDisplayData(otherIncome);
      this.populateIncomeLabel(otherIncome);
      this.isSelected.Tag = (object) otherIncome;
    }

    private void populateIncomeLabel(AIQOtherIncomeData aiqOtherIncome)
    {
      string str = "";
      FieldOptionCollection options = EncompassFields.GetField("URLAROIS0018").Options;
      if (aiqOtherIncome.linkedEncompassOISBlockNumber > 0)
      {
        string field = this.loan.GetField("URLAROIS" + aiqOtherIncome.linkedEncompassOISBlockNumber.ToString("00") + "18", this.borrowerPairForOIS);
        str = !options.ContainsValue(field) ? "Other" : options.ValueToText(field);
      }
      if (aiqOtherIncome.linkedEncompassOISBlockNumber <= 0 || str == "")
        str = !options.ContainsValue(aiqOtherIncome.incomeType) ? "Other" : options.ValueToText(aiqOtherIncome.incomeType);
      this.otherIncomeLabel.Text = str;
    }

    private void matchAndDisplayData(AIQOtherIncomeData aiqOtherIncome)
    {
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      this.loan.SetBorrowerPair(this.borrowerPairForOIS);
      this.aiqOtherIncomeForm.SetVeriFieldIDinTag("URLAROIS", "URLAROIS", aiqOtherIncome.linkedEncompassOISBlockNumber);
      this.aiqOtherIncomeForm.SetAIQValuesToForm(true, "OTHERINCOME");
      this.compareAIQAndEncompassData();
      if (aiqOtherIncome.linkedEncompassOISBlockNumber < 1)
        this.clearAllRedFlags();
      this.loan.SetBorrowerPair(currentBorrowerPair);
    }

    private void compareAIQAndEncompassData()
    {
      Control.ControlCollection controls = this.Controls["panel1"].Controls;
      foreach (Control control1 in (ArrangedElementCollection) controls)
      {
        if (control1 is PictureBox)
        {
          string prefix = "URLAROIS";
          string str = "";
          string fieldId = "";
          if (control1.Tag != null)
            str = control1.Tag.ToString();
          fieldId = str.Substring(str.Length - 2);
          Control control2 = controls.Cast<Control>().FirstOrDefault<Control>((Func<Control, bool>) (x => x is TextBox && x.Tag.ToString().StartsWith(prefix) && x.Tag.ToString().EndsWith(fieldId)));
          Control control3 = controls.Cast<Control>().FirstOrDefault<Control>((Func<Control, bool>) (x => x is TextBox && x.Tag.ToString().StartsWith("AIQ") && x.Tag.ToString().EndsWith(fieldId)));
          if (control2 != null && control3 != null)
            control1.Visible = control2.Text != control3.Text;
        }
      }
    }

    private void clearAllRedFlags()
    {
      Control.ControlCollection controls = this.Controls;
      foreach (Control control in (ArrangedElementCollection) this.Controls)
      {
        if (control is PictureBox)
          control.Visible = false;
      }
    }

    private void isSelected_CheckedChanged(object sender, EventArgs e)
    {
      AIQOtherIncomeData tag = (AIQOtherIncomeData) this.isSelected.Tag;
      if (this.isSelected.Checked)
        tag.isSelected = true;
      else
        tag.isSelected = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.isSelected = new CheckBox();
      this.otherIncomeLabel = new Label();
      this.pictureBox5 = new PictureBox();
      this.textBox9 = new TextBox();
      this.textBox10 = new TextBox();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.pictureBox5).BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.isSelected);
      this.panel1.Controls.Add((Control) this.otherIncomeLabel);
      this.panel1.Controls.Add((Control) this.pictureBox5);
      this.panel1.Controls.Add((Control) this.textBox9);
      this.panel1.Controls.Add((Control) this.textBox10);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(661, 46);
      this.panel1.TabIndex = 1;
      this.isSelected.AutoSize = true;
      this.isSelected.Location = new Point(595, 12);
      this.isSelected.Name = "isSelected";
      this.isSelected.Size = new Size(15, 14);
      this.isSelected.TabIndex = 109;
      this.isSelected.UseVisualStyleBackColor = true;
      this.isSelected.CheckedChanged += new EventHandler(this.isSelected_CheckedChanged);
      this.otherIncomeLabel.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
      this.otherIncomeLabel.ForeColor = Color.Black;
      this.otherIncomeLabel.Location = new Point(12, 9);
      this.otherIncomeLabel.Name = "otherIncomeLabel";
      this.otherIncomeLabel.Size = new Size(134, 37);
      this.otherIncomeLabel.TabIndex = 108;
      this.otherIncomeLabel.Text = "label1";
      this.pictureBox5.Image = (Image) Resources.IncompleteAlert;
      this.pictureBox5.Location = new Point(542, 9);
      this.pictureBox5.Name = "pictureBox5";
      this.pictureBox5.Size = new Size(23, 18);
      this.pictureBox5.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pictureBox5.TabIndex = 107;
      this.pictureBox5.TabStop = false;
      this.pictureBox5.Tag = (object) "URLAROIS0022";
      this.pictureBox5.Visible = false;
      this.textBox9.Location = new Point(361, 9);
      this.textBox9.Name = "textBox9";
      this.textBox9.ReadOnly = true;
      this.textBox9.Size = new Size(159, 20);
      this.textBox9.TabIndex = 106;
      this.textBox9.Tag = (object) "AIQURLAROIS0022";
      this.textBox10.Location = new Point(187, 9);
      this.textBox10.Name = "textBox10";
      this.textBox10.ReadOnly = true;
      this.textBox10.Size = new Size(159, 20);
      this.textBox10.TabIndex = 105;
      this.textBox10.Tag = (object) "URLAROIS0022";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (OtherIncomeControl);
      this.Size = new Size(661, 46);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.pictureBox5).EndInit();
      this.ResumeLayout(false);
    }
  }
}
