// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.AddEditFilterDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Reporting;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class AddEditFilterDialog : Form
  {
    private const string className = "AddEditLoanFilterDialog";
    private static string sw = Tracing.SwCommon;
    private IContainer components;
    private FieldFilterEditor ctlEditor;
    private Button btnCancel;
    private Button btnOK;

    public AddEditFilterDialog(ReportFieldDefs fieldDefs)
    {
      this.InitializeComponent();
      this.ctlEditor.FieldDefinitions = fieldDefs;
    }

    public AddEditFilterDialog(ReportFieldDefs fieldDefs, bool isDDM)
      : this(fieldDefs)
    {
      this.ctlEditor.DDMSetting = isDDM;
    }

    public bool AllowDatabaseFieldsOnly
    {
      get => this.ctlEditor.AllowDatabaseFieldsOnly;
      set => this.ctlEditor.AllowDatabaseFieldsOnly = value;
    }

    public bool AllowDynamicOperators
    {
      get => this.ctlEditor.AllowDynamicOperators;
      set => this.ctlEditor.AllowDynamicOperators = value;
    }

    public bool AllowVirtualFields
    {
      get => this.ctlEditor.AllowVirtualFields;
      set => this.ctlEditor.AllowVirtualFields = value;
    }

    public FieldFilter SelectedFilter
    {
      get => this.ctlEditor.CurrentFilter;
      set => this.ctlEditor.CurrentFilter = value;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.ctlEditor.ValidateContents())
        return;
      this.ctlEditor.CommitChanges();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddEditFilterDialog));
      this.ctlEditor = new FieldFilterEditor();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.ctlEditor.AllowDatabaseFieldsOnly = true;
      this.ctlEditor.AllowDynamicOperators = true;
      this.ctlEditor.AllowVirtualFields = true;
      this.ctlEditor.CurrentFilter = (FieldFilter) componentResourceManager.GetObject("ctlEditor.CurrentFilter");
      this.ctlEditor.FieldDefinitions = (ReportFieldDefs) null;
      this.ctlEditor.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlEditor.Location = new Point(7, 8);
      this.ctlEditor.Name = "ctlEditor";
      this.ctlEditor.Size = new Size(301, 183);
      this.ctlEditor.TabIndex = 1;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.CausesValidation = false;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(231, 202);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(148, 202);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(315, 233);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.ctlEditor);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddEditFilterDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add/Edit Search Filter";
      this.ResumeLayout(false);
    }
  }
}
