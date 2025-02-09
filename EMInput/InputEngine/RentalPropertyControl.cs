// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.RentalPropertyControl
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
  public class RentalPropertyControl : UserControl
  {
    private LoanData loan;
    private AIQIncomeData aiqIncomeData;
    private int loanVOMIndex = -1;
    private bool forCoborrower;
    private int aiqVOMIndex = -1;
    private GenericFormInputHandler aiqPropRentalForm;
    private Dictionary<string, int> encompassAIQIndexMapping = new Dictionary<string, int>();
    private BorrowerPair borrowerPairForVOM;
    public Control isSelectedControl;
    private IContainer components;
    private Panel panel1;
    private PictureBox pictureBox5;
    private TextBox textBox9;
    private TextBox textBox10;
    private Label label11;
    private Label RentalPropertyLabel;
    private CheckBox isSelected;
    private Label subjectPropertyLabel;

    public RentalPropertyControl(
      int loanVOMIndex,
      bool forCoborrower,
      LoanData loan,
      Sessions.Session session,
      int aiqVOMIndex,
      AIQIncomeData aiqIncomeData,
      BorrowerPair borrowerPairForVOM)
    {
      this.loanVOMIndex = loanVOMIndex;
      this.forCoborrower = forCoborrower;
      this.loan = loan;
      this.aiqIncomeData = aiqIncomeData;
      this.aiqVOMIndex = aiqVOMIndex;
      this.borrowerPairForVOM = borrowerPairForVOM;
      this.InitializeComponent();
      this.aiqPropRentalForm = new GenericFormInputHandler((IHtmlInput) loan, this.Controls, session);
      this.isSelectedControl = (Control) this.isSelected;
      this.initForm();
    }

    private void initForm()
    {
      this.aiqPropRentalForm.SetVeriFieldIDinTag("FM", "FM", this.loanVOMIndex);
      string borrowerKey = this.forCoborrower ? "C" : "B";
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (borrowerPairs[index].Id == this.borrowerPairForVOM.Id)
        {
          borrowerKey = (index + 1).ToString() + borrowerKey;
          break;
        }
      }
      AIQPropertyRentalData propertyRental = this.aiqIncomeData.getPropertyRentalList(borrowerKey)[this.aiqVOMIndex];
      this.aiqPropRentalForm.SetAIQValuesToForm(false, "PROPERTYRENTAL", aiqProp: propertyRental);
      this.matchAndDisplayData(propertyRental);
      this.populateLabels(propertyRental);
      if (propertyRental.ignoreForImport)
        this.isSelected.Visible = false;
      this.isSelected.Tag = (object) propertyRental;
    }

    private void populateLabels(AIQPropertyRentalData aiqProp)
    {
      string str = "";
      if (aiqProp.linkedEncompassVOMBlockNumber > 0)
        str = this.loan.GetField("FM" + aiqProp.linkedEncompassVOMBlockNumber.ToString("00") + "50", this.borrowerPairForVOM);
      if (aiqProp.linkedEncompassVOMBlockNumber <= 0 || str == "")
        str = aiqProp.addressLineText;
      this.RentalPropertyLabel.Text = str;
      if (aiqProp.belongsToBoth)
        this.RentalPropertyLabel.Text += " (Jointly Owned)";
      if (!aiqProp.isSubjectProperty)
        return;
      this.subjectPropertyLabel.Visible = true;
    }

    private void matchAndDisplayData(AIQPropertyRentalData aiqProp)
    {
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      this.loan.SetBorrowerPair(this.borrowerPairForVOM);
      this.aiqPropRentalForm.SetVeriFieldIDinTag("FM", "FM", aiqProp.linkedEncompassVOMBlockNumber);
      this.aiqPropRentalForm.SetAIQValuesToForm(true, "PROPERTYRENTAL");
      this.compareAIQAndEncompassData();
      if (aiqProp.linkedEncompassVOMBlockNumber < 1)
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
      Control.ControlCollection controls = this.Controls["panel1"].Controls;
      foreach (Control control1 in (ArrangedElementCollection) controls)
      {
        if (control1 is PictureBox)
        {
          string prefix = "FM";
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

    private void isSelected_CheckedChanged(object sender, EventArgs e)
    {
      AIQPropertyRentalData tag = (AIQPropertyRentalData) this.isSelected.Tag;
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
      this.RentalPropertyLabel = new Label();
      this.pictureBox5 = new PictureBox();
      this.textBox9 = new TextBox();
      this.textBox10 = new TextBox();
      this.label11 = new Label();
      this.subjectPropertyLabel = new Label();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.pictureBox5).BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.subjectPropertyLabel);
      this.panel1.Controls.Add((Control) this.isSelected);
      this.panel1.Controls.Add((Control) this.RentalPropertyLabel);
      this.panel1.Controls.Add((Control) this.pictureBox5);
      this.panel1.Controls.Add((Control) this.textBox9);
      this.panel1.Controls.Add((Control) this.textBox10);
      this.panel1.Controls.Add((Control) this.label11);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(661, 57);
      this.panel1.TabIndex = 0;
      this.isSelected.AutoSize = true;
      this.isSelected.Location = new Point(595, 8);
      this.isSelected.Name = "isSelected";
      this.isSelected.Size = new Size(15, 14);
      this.isSelected.TabIndex = 109;
      this.isSelected.UseVisualStyleBackColor = true;
      this.isSelected.CheckedChanged += new EventHandler(this.isSelected_CheckedChanged);
      this.RentalPropertyLabel.AutoSize = true;
      this.RentalPropertyLabel.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
      this.RentalPropertyLabel.ForeColor = Color.Black;
      this.RentalPropertyLabel.Location = new Point(12, 4);
      this.RentalPropertyLabel.Name = "RentalPropertyLabel";
      this.RentalPropertyLabel.Size = new Size(47, 15);
      this.RentalPropertyLabel.TabIndex = 108;
      this.RentalPropertyLabel.Text = "label1";
      this.pictureBox5.Image = (Image) Resources.IncompleteAlert;
      this.pictureBox5.Location = new Point(542, 23);
      this.pictureBox5.Name = "pictureBox5";
      this.pictureBox5.Size = new Size(23, 18);
      this.pictureBox5.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pictureBox5.TabIndex = 107;
      this.pictureBox5.TabStop = false;
      this.pictureBox5.Tag = (object) "FM0020";
      this.pictureBox5.Visible = false;
      this.textBox9.Location = new Point(361, 23);
      this.textBox9.Name = "textBox9";
      this.textBox9.ReadOnly = true;
      this.textBox9.Size = new Size(159, 20);
      this.textBox9.TabIndex = 106;
      this.textBox9.Tag = (object) "AIQFM0020";
      this.textBox10.Location = new Point(187, 23);
      this.textBox10.Name = "textBox10";
      this.textBox10.ReadOnly = true;
      this.textBox10.Size = new Size(159, 20);
      this.textBox10.TabIndex = 105;
      this.textBox10.Tag = (object) "FM0020";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(12, 23);
      this.label11.Name = "label11";
      this.label11.Size = new Size(106, 13);
      this.label11.TabIndex = 104;
      this.label11.Text = "Gross Rental Income";
      this.subjectPropertyLabel.Location = new Point(12, 39);
      this.subjectPropertyLabel.Name = "subjectPropertyLabel";
      this.subjectPropertyLabel.Size = new Size(98, 17);
      this.subjectPropertyLabel.TabIndex = 110;
      this.subjectPropertyLabel.Text = "(Subject Property)";
      this.subjectPropertyLabel.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (RentalPropertyControl);
      this.Size = new Size(661, 57);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.pictureBox5).EndInit();
      this.ResumeLayout(false);
    }
  }
}
