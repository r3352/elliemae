// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TrusteeDatabaseContainer
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TrusteeDatabaseContainer : Form
  {
    private TrusteeDatabaseControl trustControl;
    private IContainer components;
    private PanelEx pnlExContent;
    private Button btnSelect;
    private Button btnClose;

    public TrusteeDatabaseContainer(TrusteeRecord rec)
    {
      this.InitializeComponent();
      this.trustControl = new TrusteeDatabaseControl((IWin32Window) this, rec, false);
      this.trustControl.OnSelect += new EventHandler(this.trustControl_OnSelect);
      this.trustControl.OnSelectedItemCountChanged += new EventHandler(this.trustControl_OnSelectedItemCountChanged);
      this.pnlExContent.Controls.Add((Control) this.trustControl);
      this.trustControl_OnSelectedItemCountChanged((object) this, (EventArgs) null);
    }

    public TrusteeRecord SelectedTrustee => this.trustControl.SelectedTrustee;

    private void trustControl_OnSelect(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }

    private void trustControl_OnSelectedItemCountChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.trustControl.SelectedItemCount == 1;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlExContent = new PanelEx();
      this.btnSelect = new Button();
      this.btnClose = new Button();
      this.SuspendLayout();
      this.pnlExContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlExContent.Location = new Point(0, 0);
      this.pnlExContent.Name = "pnlExContent";
      this.pnlExContent.Size = new Size(725, 448);
      this.pnlExContent.TabIndex = 0;
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.DialogResult = DialogResult.OK;
      this.btnSelect.Location = new Point(559, 455);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 1;
      this.btnSelect.Text = "Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(640, 455);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnSelect;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(724, 484);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.pnlExContent);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (TrusteeDatabaseContainer);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Trustee List";
      this.ResumeLayout(false);
    }
  }
}
