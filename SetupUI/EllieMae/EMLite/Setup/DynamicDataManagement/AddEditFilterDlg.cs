// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.AddEditFilterDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class AddEditFilterDlg : Form
  {
    private ReportFieldDefs fieldDefs;
    private ReportFieldDef fieldDef;
    private IContainer components;
    private Label label1;
    private TextBox txtField;
    private StandardIconButton btnFieldList;
    private Label label2;
    private TextBox txtDescription;
    private Button okBtn;
    private Button cancelBtn;

    public AddEditFilterDlg(ReportFieldDefs fieldDefs)
    {
      this.InitializeComponent();
      this.fieldDefs = fieldDefs;
      this.txtField.KeyDown += new KeyEventHandler(this.txtField_KeyDown);
      this.txtField.Validating += new CancelEventHandler(this.txtField_Validating);
    }

    public ReportFieldDef FieldDef => this.fieldDef;

    private void btnFieldList_Click(object sender, EventArgs e)
    {
      using (ReportFieldSelector reportFieldSelector = new ReportFieldSelector(this.fieldDefs, false, true))
      {
        if (reportFieldSelector.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.fieldDef = reportFieldSelector.SelectedField;
        this.loadField(this.fieldDef);
      }
    }

    private void loadField(ReportFieldDef fieldDef)
    {
      this.txtField.Text = fieldDef.FieldID;
      this.txtDescription.Text = fieldDef.Description;
    }

    private void txtField_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.txtField.ReadOnly || e.KeyCode != Keys.Return)
        return;
      this.txtField_Validating(sender, new CancelEventArgs());
    }

    private void txtField_Validating(object sender, CancelEventArgs e)
    {
      string fieldId = this.txtField.Text.Trim();
      if (fieldId == "")
      {
        this.txtDescription.Text = "";
      }
      else
      {
        this.fieldDef = this.fieldDefs.GetFieldByID(fieldId);
        if (this.fieldDef == null)
        {
          ILoanManager loanManager = (ILoanManager) null;
          FieldDefinition fieldDefinition = DDM_FieldAccess_Utils.GetFieldDefinition(fieldId, loanManager);
          if (fieldDefinition != null)
            this.fieldDef = (ReportFieldDef) new LoanReportFieldDef(fieldDefinition);
        }
        if (this.fieldDef == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + fieldId + "' is not a valid Field ID.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          e.Cancel = true;
        }
        else
        {
          this.loadField(this.fieldDef);
          this.txtDescription.Focus();
        }
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.txtField.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select a field for this filter", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.fieldDef == null)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The value '" + this.txtField.Text + "' is not a valid Field ID.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
      this.label1 = new Label();
      this.txtField = new TextBox();
      this.btnFieldList = new StandardIconButton();
      this.label2 = new Label();
      this.txtDescription = new TextBox();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      ((ISupportInitialize) this.btnFieldList).BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(29, 13);
      this.label1.TabIndex = 15;
      this.label1.Text = "Field";
      this.txtField.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtField.Location = new Point(78, 6);
      this.txtField.Name = "txtField";
      this.txtField.Size = new Size(192, 20);
      this.txtField.TabIndex = 16;
      this.btnFieldList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFieldList.BackColor = Color.Transparent;
      this.btnFieldList.Location = new Point(276, 6);
      this.btnFieldList.MouseDownImage = (Image) null;
      this.btnFieldList.Name = "btnFieldList";
      this.btnFieldList.Size = new Size(16, 16);
      this.btnFieldList.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFieldList.TabIndex = 28;
      this.btnFieldList.TabStop = false;
      this.btnFieldList.Click += new EventHandler(this.btnFieldList_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 39);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 29;
      this.label2.Text = "Description";
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Location = new Point(78, 36);
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.txtDescription.Size = new Size(214, 20);
      this.txtDescription.TabIndex = 30;
      this.okBtn.Location = new Point(128, 66);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 31;
      this.okBtn.Text = "OK";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.CausesValidation = false;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(217, 66);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 32;
      this.cancelBtn.Text = "Cancel";
      this.cancelBtn.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(304, 94);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnFieldList);
      this.Controls.Add((Control) this.txtField);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtDescription);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddEditFilterDlg);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add/Edit Search Filter";
      ((ISupportInitialize) this.btnFieldList).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
