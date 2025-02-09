// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.RelatedLoanMatchDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class RelatedLoanMatchDialog : Form
  {
    private IContainer components;
    private Label label1;
    private RadioButton radAnyOriginated;
    private RadioButton radLastOriginated;
    private RadioButton radLastClosed;
    private Button btnOK;
    private RadioButton radAnyClosed;

    public RelatedLoanMatchDialog() => this.InitializeComponent();

    public RelatedLoanMatchType SelectedMatchType
    {
      get
      {
        if (this.radAnyOriginated.Checked)
          return RelatedLoanMatchType.AnyOriginated;
        if (this.radAnyClosed.Checked)
          return RelatedLoanMatchType.AnyClosed;
        return this.radLastOriginated.Checked ? RelatedLoanMatchType.LastOriginated : RelatedLoanMatchType.LastClosed;
      }
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
      this.radAnyOriginated = new RadioButton();
      this.radLastOriginated = new RadioButton();
      this.radLastClosed = new RadioButton();
      this.btnOK = new Button();
      this.radAnyClosed = new RadioButton();
      this.SuspendLayout();
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(342, 22);
      this.label1.TabIndex = 0;
      this.label1.Text = "Which loans should be considered for this search?";
      this.radAnyOriginated.AutoSize = true;
      this.radAnyOriginated.Checked = true;
      this.radAnyOriginated.Location = new Point(35, 39);
      this.radAnyOriginated.Name = "radAnyOriginated";
      this.radAnyOriginated.Size = new Size(212, 18);
      this.radAnyOriginated.TabIndex = 1;
      this.radAnyOriginated.TabStop = true;
      this.radAnyOriginated.Text = "All loans associated with each contact";
      this.radAnyOriginated.UseVisualStyleBackColor = true;
      this.radLastOriginated.AutoSize = true;
      this.radLastOriginated.Location = new Point(35, 79);
      this.radLastOriginated.Name = "radLastOriginated";
      this.radLastOriginated.Size = new Size(237, 18);
      this.radLastOriginated.TabIndex = 2;
      this.radLastOriginated.Text = "Each contact's most recently originated loan";
      this.radLastOriginated.UseVisualStyleBackColor = true;
      this.radLastClosed.AutoSize = true;
      this.radLastClosed.Location = new Point(35, 99);
      this.radLastClosed.Name = "radLastClosed";
      this.radLastClosed.Size = new Size(239, 18);
      this.radLastClosed.TabIndex = 3;
      this.radLastClosed.Text = "Each contact's most recently completed loan";
      this.radLastClosed.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(273, 152);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.radAnyClosed.AutoSize = true;
      this.radAnyClosed.Location = new Point(35, 59);
      this.radAnyClosed.Name = "radAnyClosed";
      this.radAnyClosed.Size = new Size(274, 18);
      this.radAnyClosed.TabIndex = 5;
      this.radAnyClosed.Text = "Only completed loans associated with each contact";
      this.radAnyClosed.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnOK;
      this.ClientSize = new Size(357, 183);
      this.Controls.Add((Control) this.radAnyClosed);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.radLastClosed);
      this.Controls.Add((Control) this.radLastOriginated);
      this.Controls.Add((Control) this.radAnyOriginated);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RelatedLoanMatchDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Advanced Search";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
