// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddPrintFormContainer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddPrintFormContainer : Form
  {
    private AddPrintFormControl printFormControl;
    private Sessions.Session session;
    private FormInfo[] selectedForms;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private Panel panelContainer;

    public AddPrintFormContainer(Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.printFormControl = new AddPrintFormControl(this.session, true, false, (LoanData) null, false);
      this.panelContainer.Controls.Add((Control) this.printFormControl);
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      List<FormInfo> selectedForms = this.printFormControl.GetSelectedForms();
      if (selectedForms.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an output form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.selectedForms = selectedForms.ToArray();
        this.DialogResult = DialogResult.OK;
      }
    }

    public FormInfo[] SelectedForms => this.selectedForms;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dialogButtons1 = new DialogButtons();
      this.panelContainer = new Panel();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 402);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.OKText = "&Add";
      this.dialogButtons1.Size = new Size(710, 44);
      this.dialogButtons1.TabIndex = 0;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.panelContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelContainer.Location = new Point(10, 12);
      this.panelContainer.Name = "panelContainer";
      this.panelContainer.Size = new Size(690, 388);
      this.panelContainer.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(710, 446);
      this.Controls.Add((Control) this.panelContainer);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (AddPrintFormContainer);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Print Forms";
      this.ResumeLayout(false);
    }
  }
}
