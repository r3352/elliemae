// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DisclosureTrackingForms
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DisclosureTrackingForms : Form
  {
    private List<FormItemInfo> formList;
    private AddPrintFormControl mainControl;
    private IContainer components;
    private Panel pnlWorkArea;
    private Button btnOk;
    private Button btnCancel;

    public DisclosureTrackingForms(List<FormItemInfo> formList)
    {
      this.InitializeComponent();
      this.formList = formList;
      this.initialPageSetting();
    }

    private void initialPageSetting()
    {
      this.mainControl = new AddPrintFormControl(Session.DefaultInstance, true, true, (LoanData) null, false);
      this.mainControl.Dock = DockStyle.Fill;
      this.mainControl.AddPreSelectedForm(this.formList.ToArray(), true);
      this.pnlWorkArea.Controls.Add((Control) this.mainControl);
    }

    public List<FormItemInfo> FormList => this.mainControl.GetSelectedFormItems();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DisclosureTrackingForms));
      this.pnlWorkArea = new Panel();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.pnlWorkArea.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlWorkArea.Location = new Point(12, 12);
      this.pnlWorkArea.Name = "pnlWorkArea";
      this.pnlWorkArea.Size = new Size(580, 324);
      this.pnlWorkArea.TabIndex = 0;
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Location = new Point(436, 387);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(517, 387);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(604, 422);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.pnlWorkArea);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (DisclosureTrackingForms);
      this.Text = nameof (DisclosureTrackingForms);
      this.ResumeLayout(false);
    }
  }
}
