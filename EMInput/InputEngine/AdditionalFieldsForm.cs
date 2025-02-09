// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AdditionalFieldsForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AdditionalFieldsForm : UserControl, IRefreshContents
  {
    private FieldQuickEditor editor;
    private string[] additionalFields;
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer groupContainer1;

    public AdditionalFieldsForm(Sessions.Session session, LoanData loan, string[] additionalFields)
    {
      this.session = session;
      this.additionalFields = additionalFields;
      this.InitializeComponent();
      this.editor = new FieldQuickEditor(this.session, loan, FieldQuickEditorMode.ForRequest, false);
      this.editor.RefreshFieldList(additionalFields, false);
      this.groupContainer1.Controls.Add((Control) this.editor);
      this.Height = this.editor.MaxHeight + 30;
    }

    public void RefreshContents() => this.editor.RefreshFieldList(this.additionalFields, false);

    public void RefreshLoanContents() => this.RefreshContents();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.SuspendLayout();
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(556, 603);
      this.groupContainer1.TabIndex = 0;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (AdditionalFieldsForm);
      this.Size = new Size(556, 603);
      this.ResumeLayout(false);
    }
  }
}
