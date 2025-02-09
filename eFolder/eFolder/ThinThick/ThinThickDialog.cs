// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ThinThick.ThinThickDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.eFolder.Conditions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.ThinThick
{
  public class ThinThickDialog : Form
  {
    private const string className = "ThinThickDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private ThinThickType thinThickType;
    private ThinThickControl microApp;
    private IContainer components;
    private Panel pnlMicroApp;

    public ThinThickDialog(LoanDataMgr loanDataMgr, string url, ThinThickType type)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.thinThickType = type;
      this.setWindow();
      this.microApp = new ThinThickControl(loanDataMgr, url, type);
      this.pnlMicroApp.Controls.Add((Control) this.microApp);
      this.microApp.Dock = DockStyle.Fill;
      this.microApp.WebHostUnloaded += new EventHandler(this.microApp_WebHostUnloaded);
      this.microApp.RefreshContents();
    }

    private void setWindow()
    {
      switch (this.thinThickType)
      {
        case ThinThickType.eClose:
          this.Text = "eClose";
          this.setWindowSize(0.95);
          break;
        case ThinThickType.ReviewAndImport:
          this.Text = "Import Conditions";
          this.setImportWindowSize(false);
          break;
        case ThinThickType.ImportAll:
          this.Text = "Import Conditions";
          this.setImportWindowSize(true);
          break;
        case ThinThickType.DeliverConditionResponses:
          this.Text = "Deliver Condition Responses";
          this.setImportWindowSize(false);
          break;
        case ThinThickType.ConditionDeliveryStatus:
          this.Text = "Condition Delivery Status";
          this.setImportWindowSize(false);
          break;
        case ThinThickType.eSignPackages:
          this.Text = "eSign Pipeline";
          this.setWindowSize(0.95);
          break;
        case ThinThickType.eConsent:
          this.Text = "eConsent";
          this.setWindowSize(0.95);
          break;
      }
    }

    private void setWindowSize(double sizeFactor) => this.setWindowSize(sizeFactor, sizeFactor);

    private void setWindowSize(double widthFactor, double heightFactor)
    {
      if (Form.ActiveForm != null)
      {
        Form form = Form.ActiveForm;
        while (form.Owner != null)
          form = form.Owner;
        this.Width = Convert.ToInt32((double) form.Width * widthFactor);
        this.Height = Convert.ToInt32((double) form.Height * heightFactor);
      }
      else
      {
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Width = Convert.ToInt32((double) workingArea.Width * widthFactor);
        workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Height = Convert.ToInt32((double) workingArea.Height * heightFactor);
      }
    }

    private void setImportWindowSize(bool importAll)
    {
      ImportConditionFactory conditionFactory = new ImportConditionFactory(ConditionType.Enhanced, this.loanDataMgr.LoanData, true, importAll);
      this.setWindowSize(conditionFactory.Width, conditionFactory.Height);
    }

    private void microApp_WebHostUnloaded(object source, EventArgs e)
    {
      Tracing.Log(ThinThickDialog.sw, TraceLevel.Verbose, nameof (ThinThickDialog), "UnloadHandler Triggered");
      if (!this.IsHandleCreated)
        return;
      this.BeginInvoke((Delegate) (() => this.Close()));
    }

    private void ThinThickDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.thinThickType != ThinThickType.eClose)
        return;
      if (!this.microApp.CanCloseWebHost())
        e.Cancel = true;
      if (e.Cancel || this.OwnedForms.Length == 0)
        return;
      List<Form> formList = new List<Form>((IEnumerable<Form>) this.OwnedForms);
      foreach (Form form in formList)
      {
        Tracing.Log(ThinThickDialog.sw, nameof (ThinThickDialog), TraceLevel.Error, "Closing OwnedForm: " + form.Text);
        form.Close();
        Tracing.Log(ThinThickDialog.sw, nameof (ThinThickDialog), TraceLevel.Error, "Disposing OwnedForm");
        form.Dispose();
      }
      formList.Clear();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ThinThickDialog));
      this.pnlMicroApp = new Panel();
      this.SuspendLayout();
      this.pnlMicroApp.BackColor = SystemColors.Control;
      this.pnlMicroApp.Dock = DockStyle.Fill;
      this.pnlMicroApp.Location = new Point(0, 0);
      this.pnlMicroApp.Margin = new Padding(2);
      this.pnlMicroApp.Name = "pnlMicroApp";
      this.pnlMicroApp.Size = new Size(1174, 546);
      this.pnlMicroApp.TabIndex = 6;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1174, 546);
      this.Controls.Add((Control) this.pnlMicroApp);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(2, 3, 2, 3);
      this.MinimizeBox = false;
      this.Name = nameof (ThinThickDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "eClosing";
      this.FormClosing += new FormClosingEventHandler(this.ThinThickDialog_FormClosing);
      this.ResumeLayout(false);
    }
  }
}
