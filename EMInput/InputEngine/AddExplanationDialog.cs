// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddExplanationDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AddExplanationDialog : Form
  {
    private IHtmlInput inputData;
    private string sectionID;
    private bool isDirty;
    private bool printOptionChanged;
    private LoanData loan;
    private PopupBusinessRules popupRules;
    private Sessions.Session session;
    private string fieldID;
    private string fieldIDPrint;
    private IContainer components;
    private Button btnSave;
    private Button btnCancel;
    private TextBox txtExplanation;
    private ToolTip fieldToolTip;
    private CheckBox printOptionCheckBox;

    public AddExplanationDialog(IHtmlInput inputData, string sectionID, Sessions.Session session)
    {
      this.inputData = inputData;
      this.sectionID = sectionID;
      this.session = session;
      if (this.inputData is LoanData)
        this.loan = (LoanData) this.inputData;
      this.InitializeComponent();
      this.Text = this.Text + " - Section " + this.sectionID;
      switch (this.sectionID)
      {
        case "A":
          this.fieldID = "URLA.X216";
          this.fieldIDPrint = "URLA.X247";
          break;
        case "A (Coborrower)":
          this.fieldID = "URLA.X271";
          this.fieldIDPrint = "URLA.X285";
          break;
        case "B":
          this.fieldID = "URLA.X217";
          this.fieldIDPrint = "URLA.X248";
          break;
        case "B (Coborrower)":
          this.fieldID = "URLA.X272";
          this.fieldIDPrint = "URLA.X286";
          break;
        case "C":
          this.fieldID = "URLA.X218";
          this.fieldIDPrint = "URLA.X249";
          break;
        case "C (Coborrower)":
          this.fieldID = "URLA.X273";
          this.fieldIDPrint = "URLA.X287";
          break;
        case "D":
          this.fieldID = "URLA.X219";
          this.fieldIDPrint = "URLA.X250";
          break;
        case "D (Coborrower)":
          this.fieldID = "URLA.X274";
          this.fieldIDPrint = "URLA.X288";
          break;
        case "D2":
          this.fieldID = "URLA.X235";
          this.fieldIDPrint = "URLA.X251";
          break;
        case "D2 (Coborrower)":
          this.fieldID = "URLA.X275";
          this.fieldIDPrint = "URLA.X289";
          break;
        case "E":
          this.fieldID = "URLA.X220";
          this.fieldIDPrint = "URLA.X252";
          break;
        case "E (Coborrower)":
          this.fieldID = "URLA.X276";
          this.fieldIDPrint = "URLA.X290";
          break;
        case "F":
          this.fieldID = "URLA.X221";
          this.fieldIDPrint = "URLA.X253";
          break;
        case "F (Coborrower)":
          this.fieldID = "URLA.X277";
          this.fieldIDPrint = "URLA.X291";
          break;
        case "G":
          this.fieldID = "URLA.X222";
          this.fieldIDPrint = "URLA.X254";
          break;
        case "G (Coborrower)":
          this.fieldID = "URLA.X278";
          this.fieldIDPrint = "URLA.X292";
          break;
        case "H":
          this.fieldID = "URLA.X223";
          this.fieldIDPrint = "URLA.X255";
          break;
        case "H (Coborrower)":
          this.fieldID = "URLA.X279";
          this.fieldIDPrint = "URLA.X293";
          break;
        case "I":
          this.fieldID = "URLA.X224";
          this.fieldIDPrint = "URLA.X256";
          break;
        case "I (Coborrower)":
          this.fieldID = "URLA.X280";
          this.fieldIDPrint = "URLA.X294";
          break;
        case "J":
          this.fieldID = "URLA.X225";
          this.fieldIDPrint = "URLA.X257";
          break;
        case "J (Coborrower)":
          this.fieldID = "URLA.X281";
          this.fieldIDPrint = "URLA.X295";
          break;
        case "K":
          this.fieldID = "URLA.X226";
          this.fieldIDPrint = "URLA.X258";
          break;
        case "K (Coborrower)":
          this.fieldID = "URLA.X282";
          this.fieldIDPrint = "URLA.X296";
          break;
        case "L":
          this.fieldID = "URLA.X227";
          this.fieldIDPrint = "URLA.X259";
          break;
        case "L (Coborrower)":
          this.fieldID = "URLA.X283";
          this.fieldIDPrint = "URLA.X297";
          break;
        case "M":
          this.fieldID = "URLA.X228";
          this.fieldIDPrint = "URLA.X260";
          break;
        case "M (Coborrower)":
          this.fieldID = "URLA.X284";
          this.fieldIDPrint = "URLA.X298";
          break;
      }
      this.initForm();
      this.setBusinessRule();
      this.isDirty = false;
    }

    private void initForm()
    {
      if (this.fieldID == null)
        return;
      this.txtExplanation.Text = this.inputData.GetField(this.fieldID);
      this.printOptionCheckBox.Checked = this.inputData.GetField(this.fieldIDPrint) == "Y";
      this.fieldToolTip.SetToolTip((Control) this.txtExplanation, this.fieldID);
      this.fieldToolTip.SetToolTip((Control) this.printOptionCheckBox, this.fieldIDPrint);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.isDirty && this.fieldID != null && !PopupBusinessRules.RequiredFieldRuleCheck(this.fieldID, this.inputData) || this.printOptionChanged && this.fieldIDPrint != null && !PopupBusinessRules.RequiredFieldRuleCheck(this.fieldIDPrint, this.inputData))
        return;
      if (this.isDirty && this.fieldID != null)
        this.inputData.SetField(this.fieldID, this.txtExplanation.Text);
      if (this.printOptionChanged && this.fieldIDPrint != null)
        this.inputData.SetField(this.fieldIDPrint, this.printOptionCheckBox.Checked ? "Y" : "N");
      this.DialogResult = DialogResult.OK;
    }

    private void txtExplanation_TextChanged(object sender, EventArgs e)
    {
      this.isDirty = true;
      this.printOptionCheckBox.Checked = !string.IsNullOrEmpty(this.txtExplanation.Text) && this.printOptionCheckBox.Checked;
    }

    private void printOptionCheckBox_Changed(object sender, EventArgs e)
    {
      this.printOptionChanged = true;
    }

    private void setBusinessRule()
    {
      if (this.loan == null || this.fieldID == null)
        return;
      ResourceManager resources = new ResourceManager(typeof (AddExplanationDialog));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), this.session);
      this.popupRules.SetBusinessRules((object) this.txtExplanation, this.fieldID);
      this.popupRules.SetBusinessRules((object) this.printOptionCheckBox, this.fieldIDPrint);
    }

    private void field_Enter(object sender, EventArgs e)
    {
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(sender is TextBox ? this.fieldID : this.fieldIDPrint);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.txtExplanation = new TextBox();
      this.fieldToolTip = new ToolTip(this.components);
      this.printOptionCheckBox = new CheckBox();
      this.SuspendLayout();
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.BackColor = SystemColors.Control;
      this.btnSave.Location = new Point(634, 548);
      this.btnSave.Margin = new Padding(5, 6, 5, 6);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(137, 44);
      this.btnSave.TabIndex = 19;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(784, 548);
      this.btnCancel.Margin = new Padding(5, 6, 5, 6);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(137, 44);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "&Cancel";
      this.txtExplanation.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtExplanation.Location = new Point(11, 17);
      this.txtExplanation.Margin = new Padding(5, 6, 5, 6);
      this.txtExplanation.Multiline = true;
      this.txtExplanation.Name = "txtExplanation";
      this.txtExplanation.ScrollBars = ScrollBars.Both;
      this.txtExplanation.Size = new Size(909, 511);
      this.txtExplanation.TabIndex = 18;
      this.txtExplanation.TextChanged += new EventHandler(this.txtExplanation_TextChanged);
      this.txtExplanation.Enter += new EventHandler(this.field_Enter);
      this.printOptionCheckBox.AutoSize = true;
      this.printOptionCheckBox.Location = new Point(53, 548);
      this.printOptionCheckBox.Name = "printOptionCheckBox";
      this.printOptionCheckBox.Size = new Size(266, 29);
      this.printOptionCheckBox.TabIndex = 21;
      this.printOptionCheckBox.Text = "Print explanation on URLA";
      this.printOptionCheckBox.UseVisualStyleBackColor = true;
      this.printOptionCheckBox.Enter += new EventHandler(this.field_Enter);
      this.printOptionCheckBox.CheckStateChanged += new EventHandler(this.printOptionCheckBox_Changed);
      this.AutoScaleDimensions = new SizeF(11f, 24f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(932, 614);
      this.Controls.Add((Control) this.printOptionCheckBox);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtExplanation);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddExplanationDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Explanation";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
