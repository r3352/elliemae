// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.ClientIDInfoForm
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public class ClientIDInfoForm : Form
  {
    private IContainer components;
    private Label lblMsg;
    private Button btnCancel;
    private Button btnContinue;
    private Label lblMsg2;

    public ClientIDInfoForm()
    {
      this.InitializeComponent();
      this.ActiveControl = (Control) this.btnCancel;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ClientIDInfoForm));
      this.lblMsg = new Label();
      this.btnCancel = new Button();
      this.btnContinue = new Button();
      this.lblMsg2 = new Label();
      this.SuspendLayout();
      this.lblMsg.Location = new Point(12, 11);
      this.lblMsg.Name = "lblMsg";
      this.lblMsg.Size = new Size(537, 47);
      this.lblMsg.TabIndex = 2;
      this.lblMsg.Text = componentResourceManager.GetString("lblMsg.Text");
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(474, 117);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnContinue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnContinue.DialogResult = DialogResult.OK;
      this.btnContinue.Location = new Point(393, 117);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(75, 23);
      this.btnContinue.TabIndex = 1;
      this.btnContinue.Text = "Continue";
      this.btnContinue.UseVisualStyleBackColor = true;
      this.lblMsg2.Location = new Point(12, 62);
      this.lblMsg2.Name = "lblMsg2";
      this.lblMsg2.Size = new Size(537, 49);
      this.lblMsg2.TabIndex = 3;
      this.lblMsg2.Text = componentResourceManager.GetString("lblMsg2.Text");
      this.AcceptButton = (IButtonControl) this.btnContinue;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(561, 152);
      this.Controls.Add((Control) this.lblMsg2);
      this.Controls.Add((Control) this.btnContinue);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblMsg);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (ClientIDInfoForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass SmartClient Client IDs";
      this.ResumeLayout(false);
    }
  }
}
