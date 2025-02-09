// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.IncomeBorrowerUIControl
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
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class IncomeBorrowerUIControl : UserControl
  {
    private LoanData loan;
    private AIQIncomeData aiqIncomeData;
    private bool forCoborrower;
    private Dictionary<string, int> encompassAIQIndexMapping = new Dictionary<string, int>();
    private BorrowerPair borrowerPair;
    private int borrowerPairIndex = -1;
    private Sessions.Session session;
    private int height;
    private int totalControls;
    public List<Control> isSelectedControls = new List<Control>();
    private IContainer components;
    private Panel BorrowerHeaderLabel;
    private Label BorrowerNameLabel;
    private Panel panelBorrowerIncome;
    private PictureBox collapse;
    private PictureBox expand;

    public int CurrentHeight
    {
      get => this.height;
      set => this.height = value;
    }

    public int TotalControls => this.totalControls;

    public IncomeBorrowerUIControl(
      bool forCoborrower,
      LoanData loan,
      Sessions.Session session,
      AIQIncomeData aiqIncomeData,
      BorrowerPair borrowerPair,
      int borrowerPairIndex)
    {
      this.forCoborrower = forCoborrower;
      this.loan = loan;
      this.aiqIncomeData = aiqIncomeData;
      this.borrowerPair = borrowerPair;
      this.borrowerPairIndex = borrowerPairIndex;
      this.session = session;
      this.InitializeComponent();
      this.initForm();
    }

    private void initForm()
    {
      this.Dock = DockStyle.Top;
      this.loan.SetBorrowerPair(this.borrowerPair);
      string borrowerKey = (this.borrowerPairIndex + 1).ToString() + (this.forCoborrower ? (object) "C" : (object) "B");
      IList<AIQEmploymentData> employmentList = this.aiqIncomeData.getEmploymentList(borrowerKey);
      int count1 = employmentList != null ? employmentList.Count : 0;
      for (int loanVOEIndex = 1; loanVOEIndex <= count1; ++loanVOEIndex)
      {
        IncomeControl incomeControl = new IncomeControl(loanVOEIndex, this.forCoborrower, this.loan, this.session, loanVOEIndex - 1, this.aiqIncomeData, this.borrowerPair);
        incomeControl.Location = new Point(0, this.CurrentHeight);
        incomeControl.Width = this.panelBorrowerIncome.Width;
        this.panelBorrowerIncome.Controls.Add((Control) incomeControl);
        this.CurrentHeight += incomeControl.Height;
        ++this.totalControls;
        this.isSelectedControls.Add(incomeControl.isSelectedControl);
      }
      IList<AIQPropertyRentalData> propertyRentalList = this.aiqIncomeData.getPropertyRentalList(borrowerKey);
      int count2 = propertyRentalList != null ? propertyRentalList.Count : 0;
      for (int loanVOMIndex = 1; loanVOMIndex <= count2; ++loanVOMIndex)
      {
        RentalPropertyControl rentalPropertyControl = new RentalPropertyControl(loanVOMIndex, this.forCoborrower, this.loan, this.session, loanVOMIndex - 1, this.aiqIncomeData, this.borrowerPair);
        rentalPropertyControl.Location = new Point(0, this.CurrentHeight);
        rentalPropertyControl.Width = this.panelBorrowerIncome.Width;
        this.panelBorrowerIncome.Controls.Add((Control) rentalPropertyControl);
        this.CurrentHeight += rentalPropertyControl.Height;
        ++this.totalControls;
        if (rentalPropertyControl.isSelectedControl.Tag is AIQPropertyRentalData && !((AIQPropertyRentalData) rentalPropertyControl.isSelectedControl.Tag).ignoreForImport)
          this.isSelectedControls.Add(rentalPropertyControl.isSelectedControl);
      }
      IList<AIQOtherIncomeData> otherIncomeList = this.aiqIncomeData.getOtherIncomeList(borrowerKey);
      int count3 = otherIncomeList != null ? otherIncomeList.Count : 0;
      for (int loanOISIndex = 1; loanOISIndex <= count3; ++loanOISIndex)
      {
        OtherIncomeControl otherIncomeControl = new OtherIncomeControl(loanOISIndex, this.forCoborrower, this.loan, this.session, loanOISIndex - 1, this.aiqIncomeData, this.borrowerPair);
        otherIncomeControl.Location = new Point(0, this.CurrentHeight);
        otherIncomeControl.Width = this.panelBorrowerIncome.Width;
        this.panelBorrowerIncome.Controls.Add((Control) otherIncomeControl);
        this.CurrentHeight += otherIncomeControl.Height;
        ++this.totalControls;
        this.isSelectedControls.Add(otherIncomeControl.isSelectedControl);
      }
      this.populateBorrowerNameLabel();
    }

    private void populateBorrowerNameLabel()
    {
      BorrowerPair borrowerPair = this.borrowerPair;
      if (this.forCoborrower)
        this.BorrowerNameLabel.Text = borrowerPair.CoBorrower.FirstName + " " + borrowerPair.CoBorrower.LastName;
      else
        this.BorrowerNameLabel.Text = borrowerPair.Borrower.FirstName + " " + borrowerPair.Borrower.LastName;
    }

    private void collapse_Click(object sender, EventArgs e)
    {
      this.panelBorrowerIncome.Visible = false;
    }

    private void expand_Click(object sender, EventArgs e)
    {
      this.panelBorrowerIncome.Visible = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.BorrowerHeaderLabel = new Panel();
      this.BorrowerNameLabel = new Label();
      this.panelBorrowerIncome = new Panel();
      this.collapse = new PictureBox();
      this.expand = new PictureBox();
      this.BorrowerHeaderLabel.SuspendLayout();
      ((ISupportInitialize) this.collapse).BeginInit();
      ((ISupportInitialize) this.expand).BeginInit();
      this.SuspendLayout();
      this.BorrowerHeaderLabel.BackColor = Color.FromArgb(224, 224, 224);
      this.BorrowerHeaderLabel.BorderStyle = BorderStyle.FixedSingle;
      this.BorrowerHeaderLabel.Controls.Add((Control) this.expand);
      this.BorrowerHeaderLabel.Controls.Add((Control) this.collapse);
      this.BorrowerHeaderLabel.Controls.Add((Control) this.BorrowerNameLabel);
      this.BorrowerHeaderLabel.Dock = DockStyle.Top;
      this.BorrowerHeaderLabel.Location = new Point(0, 0);
      this.BorrowerHeaderLabel.Name = "BorrowerHeaderLabel";
      this.BorrowerHeaderLabel.Size = new Size(661, 24);
      this.BorrowerHeaderLabel.TabIndex = 0;
      this.BorrowerNameLabel.AutoSize = true;
      this.BorrowerNameLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.BorrowerNameLabel.Location = new Point(67, 4);
      this.BorrowerNameLabel.Name = "BorrowerNameLabel";
      this.BorrowerNameLabel.Size = new Size(0, 13);
      this.BorrowerNameLabel.TabIndex = 2;
      this.panelBorrowerIncome.AutoSize = true;
      this.panelBorrowerIncome.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.panelBorrowerIncome.Dock = DockStyle.Top;
      this.panelBorrowerIncome.Location = new Point(0, 24);
      this.panelBorrowerIncome.Name = "panelBorrowerIncome";
      this.panelBorrowerIncome.Size = new Size(661, 0);
      this.panelBorrowerIncome.TabIndex = 2;
      this.collapse.Image = (Image) Resources.collapse;
      this.collapse.Location = new Point(8, 3);
      this.collapse.Name = "collapse";
      this.collapse.Padding = new Padding(3, 0, 0, 0);
      this.collapse.Size = new Size(28, 23);
      this.collapse.TabIndex = 5;
      this.collapse.TabStop = false;
      this.collapse.Click += new EventHandler(this.collapse_Click);
      this.expand.Image = (Image) Resources.expand;
      this.expand.Location = new Point(36, 3);
      this.expand.Name = "expand";
      this.expand.Padding = new Padding(3, 0, 0, 0);
      this.expand.Size = new Size(28, 23);
      this.expand.TabIndex = 6;
      this.expand.TabStop = false;
      this.expand.Click += new EventHandler(this.expand_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.panelBorrowerIncome);
      this.Controls.Add((Control) this.BorrowerHeaderLabel);
      this.Name = nameof (IncomeBorrowerUIControl);
      this.Size = new Size(661, 280);
      this.BorrowerHeaderLabel.ResumeLayout(false);
      this.BorrowerHeaderLabel.PerformLayout();
      ((ISupportInitialize) this.collapse).EndInit();
      ((ISupportInitialize) this.expand).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
