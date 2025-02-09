// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TriggerAssignmentEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TriggerAssignmentEditor : Form
  {
    private FieldSettings fieldSettings;
    private TriggerAssignment assignmentItem;
    private IContainer components;
    private Label label1;
    private TextBox txtFieldID;
    private Label label2;
    private TextBox txtValue;
    private CheckBox chkEvaluate;
    private Button btnOK;
    private Button btnCancel;

    public TriggerAssignmentEditor(FieldSettings fieldSettings, TriggerAssignment item)
    {
      this.InitializeComponent();
      this.fieldSettings = fieldSettings;
      this.assignmentItem = item;
      if (item == null)
        return;
      this.loadAssignmentItem();
    }

    public TriggerAssignment TriggerAssignment => this.assignmentItem;

    private void loadAssignmentItem()
    {
      this.txtFieldID.Text = this.assignmentItem.FieldID;
      this.txtValue.Text = this.assignmentItem.Expression;
      this.chkEvaluate.Checked = this.assignmentItem.Evaluate;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      FieldDefinition field = EncompassFields.GetField(this.txtFieldID.Text.Trim(), this.fieldSettings);
      if (field == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The value '" + this.txtFieldID.Text + "' is not a valid Field ID.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (!field.AllowEdit)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The field '" + field.FieldID + "' is read-only and cannot be modified.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (!this.chkEvaluate.Checked && FieldReplacementRegex.ParseDependentFields(this.txtValue.Text).Length != 0)
        {
          switch (Utils.Dialog((IWin32Window) this, "It appears that the value you entered may be an expression that references other field values. Would you like Encompass to evaluate this expression as a custom calculation so field references will be replaced by values from the loan file?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
          {
            case DialogResult.Cancel:
              return;
            case DialogResult.Yes:
              this.chkEvaluate.Checked = true;
              break;
          }
        }
        if (this.chkEvaluate.Checked && !this.validateExpression(this.txtValue.Text))
          return;
        this.assignmentItem = new TriggerAssignment(field.FieldID, this.txtValue.Text, this.chkEvaluate.Checked);
        this.DialogResult = DialogResult.OK;
      }
    }

    private bool validateExpression(string expression)
    {
      Cursor.Current = Cursors.WaitCursor;
      using (RuntimeContext context = RuntimeContext.Create())
      {
        try
        {
          new CalculationBuilder().CreateImplementation(new CustomCalculation(expression), context);
          return true;
        }
        catch (CompileException ex)
        {
          if (EnConfigurationSettings.GlobalSettings.Debug)
          {
            ErrorDialog.Display((Exception) ex);
            return false;
          }
          int num = (int) MessageBox.Show((IWin32Window) this, "Validation failed: the expression contains errors or is not a valid formula", "Trigger Assignment Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        finally
        {
          Cursor.Current = Cursors.Default;
        }
      }
    }

    private void TriggerAssignmentEditor_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
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
      this.txtFieldID = new TextBox();
      this.label2 = new Label();
      this.txtValue = new TextBox();
      this.chkEvaluate = new CheckBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(20, 22);
      this.label1.Name = "label1";
      this.label1.Size = new Size(79, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Assign To Field";
      this.txtFieldID.Location = new Point(110, 18);
      this.txtFieldID.Name = "txtFieldID";
      this.txtFieldID.Size = new Size(106, 20);
      this.txtFieldID.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(20, 46);
      this.label2.Name = "label2";
      this.label2.Size = new Size(34, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Value";
      this.txtValue.AcceptsReturn = true;
      this.txtValue.AcceptsTab = true;
      this.txtValue.Location = new Point(110, 42);
      this.txtValue.Multiline = true;
      this.txtValue.Name = "txtValue";
      this.txtValue.Size = new Size(246, 50);
      this.txtValue.TabIndex = 3;
      this.chkEvaluate.AutoSize = true;
      this.chkEvaluate.Location = new Point(110, 95);
      this.chkEvaluate.Name = "chkEvaluate";
      this.chkEvaluate.Size = new Size(235, 17);
      this.chkEvaluate.TabIndex = 4;
      this.chkEvaluate.Text = "Evaluate expression as a custom calculation";
      this.chkEvaluate.UseVisualStyleBackColor = true;
      this.btnOK.Location = new Point(204, 142);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(282, 142);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(379, 179);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.chkEvaluate);
      this.Controls.Add((Control) this.txtValue);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtFieldID);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TriggerAssignmentEditor);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add/Edit Assignment";
      this.KeyDown += new KeyEventHandler(this.TriggerAssignmentEditor_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
