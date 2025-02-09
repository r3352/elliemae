// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.Wizard.WizardBase
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI.Wizard
{
  public class WizardBase : Form
  {
    private Panel pnlWizard;
    private SizeF panelAutoScaleBaseSize;
    private System.ComponentModel.Container components;
    private WizardItem currentItem;
    protected Button btnNext;
    protected Button btnCancel;
    protected Panel pnlFooter;
    protected Button btnBack;
    protected GroupBox groupBox1;

    public event CancelEventHandler ItemChanging;

    public event EventHandler ItemChanged;

    public event CancelEventHandler Cancelling;

    public event CancelEventHandler Finishing;

    public WizardBase()
    {
      this.InitializeComponent();
      this.panelAutoScaleBaseSize = new SizeF(6f, 13f);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlWizard = new Panel();
      this.pnlFooter = new Panel();
      this.btnBack = new Button();
      this.btnNext = new Button();
      this.btnCancel = new Button();
      this.groupBox1 = new GroupBox();
      this.pnlFooter.SuspendLayout();
      this.SuspendLayout();
      this.pnlWizard.Dock = DockStyle.Fill;
      this.pnlWizard.Location = new Point(0, 0);
      this.pnlWizard.Name = "pnlWizard";
      this.pnlWizard.Size = new Size(496, 314);
      this.pnlWizard.TabIndex = 3;
      this.pnlFooter.Controls.Add((Control) this.btnBack);
      this.pnlFooter.Controls.Add((Control) this.btnNext);
      this.pnlFooter.Controls.Add((Control) this.btnCancel);
      this.pnlFooter.Dock = DockStyle.Bottom;
      this.pnlFooter.Location = new Point(0, 316);
      this.pnlFooter.Name = "pnlFooter";
      this.pnlFooter.Size = new Size(496, 42);
      this.pnlFooter.TabIndex = 5;
      this.btnBack.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnBack.Location = new Point(256, 12);
      this.btnBack.Name = "btnBack";
      this.btnBack.TabIndex = 5;
      this.btnBack.Text = "< &Back";
      this.btnBack.Click += new EventHandler(this.btnBack_Click);
      this.btnNext.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnNext.Location = new Point(335, 12);
      this.btnNext.Name = "btnNext";
      this.btnNext.TabIndex = 4;
      this.btnNext.Text = "&Next >";
      this.btnNext.Click += new EventHandler(this.btnNext_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(414, 12);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.groupBox1.Dock = DockStyle.Bottom;
      this.groupBox1.Location = new Point(0, 314);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(496, 2);
      this.groupBox1.TabIndex = 7;
      this.groupBox1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.btnNext;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(496, 358);
      this.Controls.Add((Control) this.pnlWizard);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.pnlFooter);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = nameof (WizardBase);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "(Title)";
      this.Closing += new CancelEventHandler(this.WizardBase_Closing);
      this.pnlFooter.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public WizardItem Current
    {
      get => this.currentItem;
      set => this.setCurrentItem(value);
    }

    private void controlsChangeHandler(object sender, EventArgs e)
    {
      this.btnBack.Text = this.currentItem.BackLabel;
      this.btnBack.Enabled = this.currentItem.BackEnabled;
      this.btnBack.Visible = this.currentItem.BackVisible;
      this.btnNext.Text = this.currentItem.NextLabel;
      this.btnNext.Enabled = this.currentItem.NextEnabled;
      this.btnCancel.Text = this.currentItem.CancelLabel;
      this.btnCancel.Visible = this.currentItem.CancelEnabled;
    }

    public bool BackButtonVisible
    {
      get => this.btnBack.Visible;
      set => this.btnBack.Visible = value;
    }

    public virtual void Next() => this.ChangePanel(this.currentItem.Next());

    public virtual void Back() => this.ChangePanel(this.currentItem.Back());

    public virtual void Cancel() => this.ChangePanel(this.currentItem.Cancel());

    public void EnableNextButton(bool enable) => this.btnNext.Enabled = enable;

    public bool IsNextButtonEnabled() => this.btnNext.Enabled;

    protected virtual void OnItemChanging(CancelEventArgs e)
    {
      if (this.ItemChanging == null)
        return;
      this.ItemChanging((object) this, e);
    }

    protected virtual void OnItemChanged()
    {
      if (this.ItemChanged == null)
        return;
      this.ItemChanged((object) this, EventArgs.Empty);
    }

    protected virtual void OnCancelling(CancelEventArgs e)
    {
      if (this.Cancelling == null)
        return;
      this.Cancelling((object) this, e);
    }

    protected virtual void OnFinishing(CancelEventArgs e)
    {
      if (this.Finishing == null)
        return;
      this.Finishing((object) this, e);
    }

    protected virtual void ChangePanel(WizardItem newPanel)
    {
      CancelEventArgs e = new CancelEventArgs(false);
      if (newPanel == WizardItem.Finished)
      {
        this.OnFinishing(e);
        if (e.Cancel)
          return;
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
      else if (newPanel == WizardItem.Cancelled)
      {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
      }
      else
      {
        if (newPanel == null)
          return;
        this.OnItemChanging(e);
        if (e.Cancel)
          return;
        this.setCurrentItem(newPanel);
      }
    }

    private void btnBack_Click(object sender, EventArgs e) => this.Back();

    private void btnNext_Click(object sender, EventArgs e) => this.Next();

    private void btnCancel_Click(object sender, EventArgs e) => this.Cancel();

    private void setCurrentItem(WizardItem item)
    {
      if (this.currentItem != null)
      {
        this.currentItem.ControlsChange -= new EventHandler(this.controlsChangeHandler);
        this.pnlWizard.Controls.Remove((Control) this.currentItem);
      }
      this.currentItem = item;
      this.currentItem.Dock = DockStyle.Fill;
      if (this.currentItem.AutoScaleDimensions.Equals((object) new SizeF(0.0f, 0.0f)))
        this.currentItem.AutoScaleDimensions = this.panelAutoScaleBaseSize;
      this.currentItem.AutoScaleMode = AutoScaleMode.Font;
      this.currentItem.ControlsChange += new EventHandler(this.controlsChangeHandler);
      this.pnlWizard.Controls.Add((Control) this.currentItem);
      this.controlsChangeHandler((object) this.currentItem, (EventArgs) null);
      this.currentItem.Focus();
      this.OnItemChanged();
    }

    private void WizardBase_Closing(object sender, CancelEventArgs e)
    {
      if (DialogResult.Cancel != this.DialogResult)
        return;
      this.OnCancelling(e);
    }
  }
}
