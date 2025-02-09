// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SelectOtherEffectiveDateForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SelectOtherEffectiveDateForm : Form
  {
    private Sessions.Session session;
    private LoanReportFieldDefs fieldDefs;
    private IContainer components;
    private Button btnOK;
    private TextBox txtFieldID;
    private Label label1;
    private Label label2;
    private StandardIconButton btnFieldList;
    private Button btnCancel;
    private Label labelDescription;

    public SelectOtherEffectiveDateForm(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.labelDescription.Text = "";
      this.txtFieldID_TextChanged((object) null, (EventArgs) null);
    }

    private void btnFieldList_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.fieldDefs == null)
        this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(this.session, LoanReportFieldFlags.LoanDataFieldsInDatabase);
      using (ReportFieldSelector reportFieldSelector = new ReportFieldSelector((ReportFieldDefs) this.fieldDefs, new EllieMae.EMLite.ClientServer.Reporting.FieldTypes[1]
      {
        EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate
      }, true, true, this.session))
      {
        Cursor.Current = Cursors.Default;
        if (reportFieldSelector.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          this.txtFieldID.Text = reportFieldSelector.SelectedField.FieldID;
          this.labelDescription.Text = reportFieldSelector.SelectedField.FieldDefinition.Description;
        }
      }
      Cursor.Current = Cursors.Default;
    }

    public string SelectedFIeldID => this.txtFieldID.Text;

    public string SelectedFIeldDescription => this.labelDescription.Text;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.labelDescription.Text == "" && DDM_FieldAccess_Utils.GetFieldDefinition(this.txtFieldID.Text.Trim(), this.session.LoanManager) == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The field ID is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void txtFieldID_Leave(object sender, EventArgs e)
    {
      FieldDefinition fieldDefinition = DDM_FieldAccess_Utils.GetFieldDefinition(this.txtFieldID.Text.Trim(), this.session.LoanManager);
      if (fieldDefinition != null)
      {
        this.txtFieldID.Text = fieldDefinition.FieldID;
        if (fieldDefinition.Format != FieldFormat.DATE && fieldDefinition.Format != FieldFormat.DATETIME && fieldDefinition.Format != FieldFormat.MONTHDAY)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The field ID is invalid , you must choose a Date Field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtFieldID.Text = string.Empty;
          return;
        }
      }
      this.labelDescription.Text = fieldDefinition != null ? fieldDefinition.Description : "";
    }

    private void txtFieldID_TextChanged(object sender, EventArgs e)
    {
      if (this.txtFieldID.Text.Trim() == "")
        this.labelDescription.Text = "";
      this.btnOK.Enabled = this.txtFieldID.Text.Trim().Length > 0;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.txtFieldID = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.btnFieldList = new StandardIconButton();
      this.btnCancel = new Button();
      this.labelDescription = new Label();
      ((ISupportInitialize) this.btnFieldList).BeginInit();
      this.SuspendLayout();
      this.btnOK.Location = new Point(163, 81);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.txtFieldID.Location = new Point(94, 19);
      this.txtFieldID.Name = "txtFieldID";
      this.txtFieldID.Size = new Size(206, 20);
      this.txtFieldID.TabIndex = 0;
      this.txtFieldID.TextChanged += new EventHandler(this.txtFieldID_TextChanged);
      this.txtFieldID.Leave += new EventHandler(this.txtFieldID_Leave);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(26, 22);
      this.label1.Name = "label1";
      this.label1.Size = new Size(29, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Field";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(26, 48);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Description";
      this.btnFieldList.BackColor = Color.Transparent;
      this.btnFieldList.Location = new Point(303, 20);
      this.btnFieldList.MouseDownImage = (Image) null;
      this.btnFieldList.Name = "btnFieldList";
      this.btnFieldList.Size = new Size(16, 16);
      this.btnFieldList.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFieldList.TabIndex = 28;
      this.btnFieldList.TabStop = false;
      this.btnFieldList.Click += new EventHandler(this.btnFieldList_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(244, 81);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.labelDescription.AutoSize = true;
      this.labelDescription.Location = new Point(92, 48);
      this.labelDescription.Name = "labelDescription";
      this.labelDescription.Size = new Size(66, 13);
      this.labelDescription.TabIndex = 31;
      this.labelDescription.Text = "(Description)";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(342, 119);
      this.Controls.Add((Control) this.labelDescription);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnFieldList);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtFieldID);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectOtherEffectiveDateForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Effective Date";
      ((ISupportInitialize) this.btnFieldList).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
