// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.IncomeControl
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
  public class IncomeControl : UserControl
  {
    private LoanData loan;
    private AIQIncomeData aiqIncomeData;
    private int loanVOEIndex = -1;
    private bool forCoborrower;
    private int aiqVOEIndex = -1;
    private GenericFormInputHandler aiqIncomeForm;
    private Dictionary<string, int> encompassAIQIndexMapping = new Dictionary<string, int>();
    private BorrowerPair borrowerPairForVOE;
    public Control isSelectedControl;
    private IContainer components;
    private Panel panel2;
    private PictureBox endDateWarning;
    private TextBox endDateEncompaass;
    private TextBox endDateAiq;
    private Label endDateLabel;
    private PictureBox pictureBox4;
    private TextBox textBox7;
    private TextBox textBox8;
    private Label label10;
    private PictureBox pictureBox10;
    private TextBox textBox19;
    private TextBox textBox20;
    private Label label16;
    private PictureBox pictureBox9;
    private TextBox textBox17;
    private TextBox textBox18;
    private Label label15;
    private PictureBox pictureBox8;
    private TextBox textBox15;
    private TextBox textBox16;
    private Label label14;
    private PictureBox pictureBox7;
    private TextBox textBox13;
    private TextBox textBox14;
    private Label label13;
    private PictureBox pictureBox6;
    private TextBox textBox11;
    private TextBox textBox12;
    private Label label12;
    private PictureBox pictureBox5;
    private TextBox textBox9;
    private TextBox textBox10;
    private Label label11;
    private Label labelEmployerName;
    private CheckBox isSelected;

    public IncomeControl(
      int loanVOEIndex,
      bool forCoborrower,
      LoanData loan,
      Sessions.Session session,
      int aiqVOEIndex,
      AIQIncomeData aiqIncomeData,
      BorrowerPair borrowerPairForVOE)
    {
      this.loanVOEIndex = loanVOEIndex;
      this.forCoborrower = forCoborrower;
      this.loan = loan;
      this.aiqIncomeData = aiqIncomeData;
      this.aiqVOEIndex = aiqVOEIndex;
      this.borrowerPairForVOE = borrowerPairForVOE;
      this.InitializeComponent();
      this.aiqIncomeForm = new GenericFormInputHandler((IHtmlInput) loan, this.Controls, session);
      this.initForm();
      this.isSelectedControl = (Control) this.isSelected;
    }

    private void initForm()
    {
      this.aiqIncomeForm.SetVeriFieldIDinTag("BE", this.forCoborrower ? "CE" : "BE", this.loanVOEIndex);
      string borrowerKey = this.forCoborrower ? "C" : "B";
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (borrowerPairs[index].Id == this.borrowerPairForVOE.Id)
        {
          borrowerKey = (index + 1).ToString() + borrowerKey;
          break;
        }
      }
      AIQEmploymentData employment = this.aiqIncomeData.getEmploymentList(borrowerKey)[this.aiqVOEIndex];
      this.aiqIncomeForm.SetAIQValuesToForm(false, "INCOME", employment);
      this.populateEmployerName(employment);
      this.matchAndDisplayData(employment);
      if (employment.currentOrPrior == "Current")
        this.hideEndDate();
      this.isSelected.Tag = (object) employment;
    }

    private void populateEmployerName(AIQEmploymentData aiqEmp)
    {
      string str = "";
      if (aiqEmp.linkedEncompassVOEBlockNumber > 0)
        str = this.loan.GetField((this.forCoborrower ? "CE" : "BE") + aiqEmp.linkedEncompassVOEBlockNumber.ToString("00") + "02", this.borrowerPairForVOE);
      if (aiqEmp.linkedEncompassVOEBlockNumber <= 0 || str == "")
        str = aiqEmp.employerFullname;
      this.labelEmployerName.Text = str;
    }

    private void matchAndDisplayData(AIQEmploymentData aiqEmp)
    {
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      this.loan.SetBorrowerPair(this.borrowerPairForVOE);
      this.aiqIncomeForm.SetVeriFieldIDinTag(this.forCoborrower ? "CE" : "BE", this.forCoborrower ? "CE" : "BE", aiqEmp.linkedEncompassVOEBlockNumber);
      this.aiqIncomeForm.SetAIQValuesToForm(true, "INCOME");
      this.compareAIQAndEncompassData();
      if (aiqEmp != null && aiqEmp.currentOrPrior == "Current")
        this.hideEndDate();
      if (aiqEmp.linkedEncompassVOEBlockNumber < 1)
        this.clearAllRedFlags();
      this.loan.SetBorrowerPair(currentBorrowerPair);
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

    private void compareAIQAndEncompassData()
    {
      Control.ControlCollection controls = this.Controls["panel2"].Controls;
      foreach (Control control1 in (ArrangedElementCollection) controls)
      {
        if (control1 is PictureBox)
        {
          if (!(control1.Name == "collapse") && !(control1.Name == "expand"))
          {
            string prefix = "BE";
            string str = "";
            string fieldId = "";
            if (control1.Tag != null)
              str = control1.Tag.ToString();
            if (this.forCoborrower)
              prefix = "CE";
            fieldId = str.Substring(str.Length - 2);
            Control control2 = controls.Cast<Control>().FirstOrDefault<Control>((Func<Control, bool>) (x => x is TextBox && x.Tag.ToString().StartsWith(prefix) && x.Tag.ToString().EndsWith(fieldId)));
            Control control3 = controls.Cast<Control>().FirstOrDefault<Control>((Func<Control, bool>) (x => x is TextBox && x.Tag.ToString().StartsWith("AIQ") && x.Tag.ToString().EndsWith(fieldId)));
            if (control2 != null && control3 != null)
              control1.Visible = control2.Text != control3.Text;
          }
        }
      }
    }

    private void hideEndDate()
    {
      foreach (Control control in this.Controls["panel2"]?.Controls.Cast<Control>().AsEnumerable<Control>().Where<Control>((Func<Control, bool>) (x => x.Name.StartsWith("endDate"))))
      {
        if (control != null)
          control.Visible = false;
      }
    }

    private void isSelected_CheckedChanged(object sender, EventArgs e)
    {
      AIQEmploymentData tag = (AIQEmploymentData) this.isSelected.Tag;
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
      this.panel2 = new Panel();
      this.isSelected = new CheckBox();
      this.endDateWarning = new PictureBox();
      this.endDateEncompaass = new TextBox();
      this.endDateAiq = new TextBox();
      this.endDateLabel = new Label();
      this.pictureBox4 = new PictureBox();
      this.textBox7 = new TextBox();
      this.textBox8 = new TextBox();
      this.label10 = new Label();
      this.pictureBox10 = new PictureBox();
      this.textBox19 = new TextBox();
      this.textBox20 = new TextBox();
      this.label16 = new Label();
      this.pictureBox9 = new PictureBox();
      this.textBox17 = new TextBox();
      this.textBox18 = new TextBox();
      this.label15 = new Label();
      this.pictureBox8 = new PictureBox();
      this.textBox15 = new TextBox();
      this.textBox16 = new TextBox();
      this.label14 = new Label();
      this.pictureBox7 = new PictureBox();
      this.textBox13 = new TextBox();
      this.textBox14 = new TextBox();
      this.label13 = new Label();
      this.pictureBox6 = new PictureBox();
      this.textBox11 = new TextBox();
      this.textBox12 = new TextBox();
      this.label12 = new Label();
      this.pictureBox5 = new PictureBox();
      this.textBox9 = new TextBox();
      this.textBox10 = new TextBox();
      this.label11 = new Label();
      this.labelEmployerName = new Label();
      this.panel2.SuspendLayout();
      ((ISupportInitialize) this.endDateWarning).BeginInit();
      ((ISupportInitialize) this.pictureBox4).BeginInit();
      ((ISupportInitialize) this.pictureBox10).BeginInit();
      ((ISupportInitialize) this.pictureBox9).BeginInit();
      ((ISupportInitialize) this.pictureBox8).BeginInit();
      ((ISupportInitialize) this.pictureBox7).BeginInit();
      ((ISupportInitialize) this.pictureBox6).BeginInit();
      ((ISupportInitialize) this.pictureBox5).BeginInit();
      this.SuspendLayout();
      this.panel2.Controls.Add((Control) this.isSelected);
      this.panel2.Controls.Add((Control) this.endDateWarning);
      this.panel2.Controls.Add((Control) this.endDateEncompaass);
      this.panel2.Controls.Add((Control) this.endDateAiq);
      this.panel2.Controls.Add((Control) this.endDateLabel);
      this.panel2.Controls.Add((Control) this.pictureBox4);
      this.panel2.Controls.Add((Control) this.textBox7);
      this.panel2.Controls.Add((Control) this.textBox8);
      this.panel2.Controls.Add((Control) this.label10);
      this.panel2.Controls.Add((Control) this.pictureBox10);
      this.panel2.Controls.Add((Control) this.textBox19);
      this.panel2.Controls.Add((Control) this.textBox20);
      this.panel2.Controls.Add((Control) this.label16);
      this.panel2.Controls.Add((Control) this.pictureBox9);
      this.panel2.Controls.Add((Control) this.textBox17);
      this.panel2.Controls.Add((Control) this.textBox18);
      this.panel2.Controls.Add((Control) this.label15);
      this.panel2.Controls.Add((Control) this.pictureBox8);
      this.panel2.Controls.Add((Control) this.textBox15);
      this.panel2.Controls.Add((Control) this.textBox16);
      this.panel2.Controls.Add((Control) this.label14);
      this.panel2.Controls.Add((Control) this.pictureBox7);
      this.panel2.Controls.Add((Control) this.textBox13);
      this.panel2.Controls.Add((Control) this.textBox14);
      this.panel2.Controls.Add((Control) this.label13);
      this.panel2.Controls.Add((Control) this.pictureBox6);
      this.panel2.Controls.Add((Control) this.textBox11);
      this.panel2.Controls.Add((Control) this.textBox12);
      this.panel2.Controls.Add((Control) this.label12);
      this.panel2.Controls.Add((Control) this.pictureBox5);
      this.panel2.Controls.Add((Control) this.textBox9);
      this.panel2.Controls.Add((Control) this.textBox10);
      this.panel2.Controls.Add((Control) this.label11);
      this.panel2.Controls.Add((Control) this.labelEmployerName);
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(661, 218);
      this.panel2.TabIndex = 101;
      this.isSelected.AutoSize = true;
      this.isSelected.Location = new Point(595, 5);
      this.isSelected.Name = "isSelected";
      this.isSelected.Size = new Size(15, 14);
      this.isSelected.TabIndex = 139;
      this.isSelected.UseVisualStyleBackColor = true;
      this.isSelected.CheckedChanged += new EventHandler(this.isSelected_CheckedChanged);
      this.endDateWarning.Image = (Image) Resources.IncompleteAlert;
      this.endDateWarning.Location = new Point(542, 49);
      this.endDateWarning.Name = "endDateWarning";
      this.endDateWarning.Size = new Size(23, 18);
      this.endDateWarning.SizeMode = PictureBoxSizeMode.CenterImage;
      this.endDateWarning.TabIndex = 138;
      this.endDateWarning.TabStop = false;
      this.endDateWarning.Tag = (object) "BE0014";
      this.endDateWarning.Visible = false;
      this.endDateEncompaass.Location = new Point(185, 49);
      this.endDateEncompaass.Name = "endDateEncompaass";
      this.endDateEncompaass.ReadOnly = true;
      this.endDateEncompaass.Size = new Size(159, 20);
      this.endDateEncompaass.TabIndex = 137;
      this.endDateEncompaass.Tag = (object) "BE0014";
      this.endDateAiq.Location = new Point(361, 49);
      this.endDateAiq.Name = "endDateAiq";
      this.endDateAiq.ReadOnly = true;
      this.endDateAiq.Size = new Size(159, 20);
      this.endDateAiq.TabIndex = 136;
      this.endDateAiq.Tag = (object) "AIQBE0014";
      this.endDateLabel.AutoSize = true;
      this.endDateLabel.Location = new Point(10, 49);
      this.endDateLabel.Name = "endDateLabel";
      this.endDateLabel.Size = new Size(52, 13);
      this.endDateLabel.TabIndex = 135;
      this.endDateLabel.Text = "End Date";
      this.pictureBox4.Image = (Image) Resources.IncompleteAlert;
      this.pictureBox4.Location = new Point(542, 27);
      this.pictureBox4.Name = "pictureBox4";
      this.pictureBox4.Size = new Size(23, 18);
      this.pictureBox4.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pictureBox4.TabIndex = 134;
      this.pictureBox4.TabStop = false;
      this.pictureBox4.Tag = (object) "BE0051";
      this.pictureBox4.Visible = false;
      this.textBox7.Location = new Point(185, 27);
      this.textBox7.Name = "textBox7";
      this.textBox7.ReadOnly = true;
      this.textBox7.Size = new Size(159, 20);
      this.textBox7.TabIndex = 133;
      this.textBox7.Tag = (object) "BE0051";
      this.textBox8.Location = new Point(361, 27);
      this.textBox8.Name = "textBox8";
      this.textBox8.ReadOnly = true;
      this.textBox8.Size = new Size(159, 20);
      this.textBox8.TabIndex = 132;
      this.textBox8.Tag = (object) "AIQBE0051";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(10, 29);
      this.label10.Name = "label10";
      this.label10.Size = new Size(55, 13);
      this.label10.TabIndex = 131;
      this.label10.Text = "Start Date";
      this.pictureBox10.Image = (Image) Resources.IncompleteAlert;
      this.pictureBox10.Location = new Point(542, 181);
      this.pictureBox10.Name = "pictureBox10";
      this.pictureBox10.Size = new Size(23, 18);
      this.pictureBox10.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pictureBox10.TabIndex = 123;
      this.pictureBox10.TabStop = false;
      this.pictureBox10.Tag = (object) "BE0071";
      this.pictureBox10.Visible = false;
      this.textBox19.Location = new Point(185, 181);
      this.textBox19.Name = "textBox19";
      this.textBox19.ReadOnly = true;
      this.textBox19.Size = new Size(159, 20);
      this.textBox19.TabIndex = 122;
      this.textBox19.Tag = (object) "BE0071";
      this.textBox20.Location = new Point(361, 181);
      this.textBox20.Name = "textBox20";
      this.textBox20.ReadOnly = true;
      this.textBox20.Size = new Size(159, 20);
      this.textBox20.TabIndex = 121;
      this.textBox20.Tag = (object) "AIQBE0071";
      this.label16.Location = new Point(11, 181);
      this.label16.Name = "label16";
      this.label16.Size = new Size(130, 37);
      this.label16.TabIndex = 120;
      this.label16.Text = "Military Variable Housing Allowance";
      this.pictureBox9.Image = (Image) Resources.IncompleteAlert;
      this.pictureBox9.Location = new Point(542, 159);
      this.pictureBox9.Name = "pictureBox9";
      this.pictureBox9.Size = new Size(23, 18);
      this.pictureBox9.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pictureBox9.TabIndex = 119;
      this.pictureBox9.TabStop = false;
      this.pictureBox9.Tag = (object) "BE0070";
      this.pictureBox9.Visible = false;
      this.textBox17.Location = new Point(185, 159);
      this.textBox17.Name = "textBox17";
      this.textBox17.ReadOnly = true;
      this.textBox17.Size = new Size(159, 20);
      this.textBox17.TabIndex = 118;
      this.textBox17.Tag = (object) "BE0070";
      this.textBox18.Location = new Point(361, 159);
      this.textBox18.Name = "textBox18";
      this.textBox18.ReadOnly = true;
      this.textBox18.Size = new Size(159, 20);
      this.textBox18.TabIndex = 117;
      this.textBox18.Tag = (object) "AIQBE0070";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(11, 159);
      this.label15.Name = "label15";
      this.label15.Size = new Size(130, 13);
      this.label15.TabIndex = 116;
      this.label15.Text = "Military Rations Allowance";
      this.pictureBox8.Image = (Image) Resources.IncompleteAlert;
      this.pictureBox8.Location = new Point(542, 137);
      this.pictureBox8.Name = "pictureBox8";
      this.pictureBox8.Size = new Size(23, 18);
      this.pictureBox8.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pictureBox8.TabIndex = 115;
      this.pictureBox8.TabStop = false;
      this.pictureBox8.Tag = (object) "BE0022";
      this.pictureBox8.Visible = false;
      this.textBox15.Location = new Point(185, 137);
      this.textBox15.Name = "textBox15";
      this.textBox15.ReadOnly = true;
      this.textBox15.Size = new Size(159, 20);
      this.textBox15.TabIndex = 114;
      this.textBox15.Tag = (object) "BE0022";
      this.textBox16.Location = new Point(361, 137);
      this.textBox16.Name = "textBox16";
      this.textBox16.ReadOnly = true;
      this.textBox16.Size = new Size(159, 20);
      this.textBox16.TabIndex = 113;
      this.textBox16.Tag = (object) "AIQBE0022";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(11, 137);
      this.label14.Name = "label14";
      this.label14.Size = new Size(67, 13);
      this.label14.TabIndex = 112;
      this.label14.Text = "Commissions";
      this.pictureBox7.Image = (Image) Resources.IncompleteAlert;
      this.pictureBox7.Location = new Point(542, 115);
      this.pictureBox7.Name = "pictureBox7";
      this.pictureBox7.Size = new Size(23, 18);
      this.pictureBox7.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pictureBox7.TabIndex = 111;
      this.pictureBox7.TabStop = false;
      this.pictureBox7.Tag = (object) "BE0021";
      this.pictureBox7.Visible = false;
      this.textBox13.Location = new Point(185, 115);
      this.textBox13.Name = "textBox13";
      this.textBox13.ReadOnly = true;
      this.textBox13.Size = new Size(159, 20);
      this.textBox13.TabIndex = 110;
      this.textBox13.Tag = (object) "BE0021";
      this.textBox14.Location = new Point(361, 115);
      this.textBox14.Name = "textBox14";
      this.textBox14.ReadOnly = true;
      this.textBox14.Size = new Size(159, 20);
      this.textBox14.TabIndex = 109;
      this.textBox14.Tag = (object) "AIQBE0021";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(11, 115);
      this.label13.Name = "label13";
      this.label13.Size = new Size(37, 13);
      this.label13.TabIndex = 108;
      this.label13.Text = "Bonus";
      this.pictureBox6.Image = (Image) Resources.IncompleteAlert;
      this.pictureBox6.Location = new Point(542, 93);
      this.pictureBox6.Name = "pictureBox6";
      this.pictureBox6.Size = new Size(23, 18);
      this.pictureBox6.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pictureBox6.TabIndex = 107;
      this.pictureBox6.TabStop = false;
      this.pictureBox6.Tag = (object) "BE0020";
      this.pictureBox6.Visible = false;
      this.textBox11.Location = new Point(185, 93);
      this.textBox11.Name = "textBox11";
      this.textBox11.ReadOnly = true;
      this.textBox11.Size = new Size(159, 20);
      this.textBox11.TabIndex = 106;
      this.textBox11.Tag = (object) "BE0020";
      this.textBox12.Location = new Point(361, 93);
      this.textBox12.Name = "textBox12";
      this.textBox12.ReadOnly = true;
      this.textBox12.Size = new Size(159, 20);
      this.textBox12.TabIndex = 105;
      this.textBox12.Tag = (object) "AIQBE0020";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(11, 93);
      this.label12.Name = "label12";
      this.label12.Size = new Size(49, 13);
      this.label12.TabIndex = 104;
      this.label12.Text = "Overtime";
      this.pictureBox5.Image = (Image) Resources.IncompleteAlert;
      this.pictureBox5.Location = new Point(542, 71);
      this.pictureBox5.Name = "pictureBox5";
      this.pictureBox5.Size = new Size(23, 18);
      this.pictureBox5.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pictureBox5.TabIndex = 103;
      this.pictureBox5.TabStop = false;
      this.pictureBox5.Tag = (object) "BE0019";
      this.pictureBox5.Visible = false;
      this.textBox9.Location = new Point(185, 71);
      this.textBox9.Name = "textBox9";
      this.textBox9.ReadOnly = true;
      this.textBox9.Size = new Size(159, 20);
      this.textBox9.TabIndex = 102;
      this.textBox9.Tag = (object) "BE0019";
      this.textBox10.Location = new Point(361, 71);
      this.textBox10.Name = "textBox10";
      this.textBox10.ReadOnly = true;
      this.textBox10.Size = new Size(159, 20);
      this.textBox10.TabIndex = 101;
      this.textBox10.Tag = (object) "AIQBE0019";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(10, 71);
      this.label11.Name = "label11";
      this.label11.Size = new Size(69, 13);
      this.label11.TabIndex = 100;
      this.label11.Text = "Base Income";
      this.labelEmployerName.AutoSize = true;
      this.labelEmployerName.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.labelEmployerName.ForeColor = Color.Black;
      this.labelEmployerName.Location = new Point(10, 5);
      this.labelEmployerName.Name = "labelEmployerName";
      this.labelEmployerName.Size = new Size(169, 15);
      this.labelEmployerName.TabIndex = 99;
      this.labelEmployerName.Text = "Monthly Income (or Loss)";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panel2);
      this.Name = nameof (IncomeControl);
      this.Size = new Size(661, 218);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      ((ISupportInitialize) this.endDateWarning).EndInit();
      ((ISupportInitialize) this.pictureBox4).EndInit();
      ((ISupportInitialize) this.pictureBox10).EndInit();
      ((ISupportInitialize) this.pictureBox9).EndInit();
      ((ISupportInitialize) this.pictureBox8).EndInit();
      ((ISupportInitialize) this.pictureBox7).EndInit();
      ((ISupportInitialize) this.pictureBox6).EndInit();
      ((ISupportInitialize) this.pictureBox5).EndInit();
      this.ResumeLayout(false);
    }
  }
}
