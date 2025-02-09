// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.GFESettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class GFESettings : SettingsUserControl
  {
    private System.ComponentModel.Container components;
    private CheckBox chkBoxItemization;
    private CheckBox chkBoxLender;
    private CheckBox chkBoxBroker;
    private CheckBox chkBoxLenderEx;
    private CheckBox chkBoxBrokerEx;
    private GroupContainer groupContainer1;
    private GFEPrintingDefault access;

    public GFESettings(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.access = (GFEPrintingDefault) Session.ServerManager.GetServerSetting("Printing.GFE");
      this.reset();
    }

    private void reset()
    {
      this.resetAll();
      this.partialAccess();
      this.setDirtyFlag(false);
    }

    private void resetAll()
    {
      this.chkBoxBroker.Checked = false;
      this.chkBoxBrokerEx.Checked = false;
      this.chkBoxLender.Checked = false;
      this.chkBoxLenderEx.Checked = false;
      this.chkBoxItemization.Checked = false;
    }

    private void partialAccess()
    {
      if (this.isSavedAccess(16, this.access))
        this.chkBoxBrokerEx.Checked = true;
      if (this.isSavedAccess(1, this.access))
        this.chkBoxLenderEx.Checked = true;
      if (this.isSavedAccess(2, this.access))
        this.chkBoxBroker.Checked = true;
      if (this.isSavedAccess(4, this.access))
        this.chkBoxLender.Checked = true;
      if (!this.isSavedAccess(8, this.access))
        return;
      this.chkBoxItemization.Checked = true;
    }

    private bool isSavedAccess(int bitmask, GFEPrintingDefault access)
    {
      return Convert.ToBoolean((int) ((GFEPrintingDefault) bitmask & access));
    }

    private void enableSaveReset(object sender, EventArgs e) => this.setDirtyFlag(true);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.chkBoxItemization = new CheckBox();
      this.chkBoxLender = new CheckBox();
      this.chkBoxBroker = new CheckBox();
      this.chkBoxLenderEx = new CheckBox();
      this.chkBoxBrokerEx = new CheckBox();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.chkBoxItemization.Location = new Point(10, 119);
      this.chkBoxItemization.Name = "chkBoxItemization";
      this.chkBoxItemization.Size = new Size(232, 16);
      this.chkBoxItemization.TabIndex = 21;
      this.chkBoxItemization.Text = "GFE - Itemization of Amount Financed";
      this.chkBoxItemization.CheckedChanged += new EventHandler(this.enableSaveReset);
      this.chkBoxLender.Location = new Point(10, 97);
      this.chkBoxLender.Name = "chkBoxLender";
      this.chkBoxLender.Size = new Size(192, 16);
      this.chkBoxLender.TabIndex = 20;
      this.chkBoxLender.Text = "GFE Lender";
      this.chkBoxLender.CheckedChanged += new EventHandler(this.enableSaveReset);
      this.chkBoxBroker.Location = new Point(10, 75);
      this.chkBoxBroker.Name = "chkBoxBroker";
      this.chkBoxBroker.Size = new Size(192, 16);
      this.chkBoxBroker.TabIndex = 3;
      this.chkBoxBroker.Text = "GFE Broker";
      this.chkBoxBroker.CheckedChanged += new EventHandler(this.enableSaveReset);
      this.chkBoxLenderEx.Location = new Point(10, 53);
      this.chkBoxLenderEx.Name = "chkBoxLenderEx";
      this.chkBoxLenderEx.Size = new Size(168, 16);
      this.chkBoxLenderEx.TabIndex = 2;
      this.chkBoxLenderEx.Text = "GFE Lender Expanded";
      this.chkBoxLenderEx.CheckedChanged += new EventHandler(this.enableSaveReset);
      this.chkBoxBrokerEx.Location = new Point(10, 31);
      this.chkBoxBrokerEx.Name = "chkBoxBrokerEx";
      this.chkBoxBrokerEx.Size = new Size(256, 16);
      this.chkBoxBrokerEx.TabIndex = 1;
      this.chkBoxBrokerEx.Text = "GFE Broker Expanded (recommended)";
      this.chkBoxBrokerEx.CheckedChanged += new EventHandler(this.enableSaveReset);
      this.groupContainer1.Controls.Add((Control) this.chkBoxBrokerEx);
      this.groupContainer1.Controls.Add((Control) this.chkBoxItemization);
      this.groupContainer1.Controls.Add((Control) this.chkBoxLenderEx);
      this.groupContainer1.Controls.Add((Control) this.chkBoxLender);
      this.groupContainer1.Controls.Add((Control) this.chkBoxBroker);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(457, 279);
      this.groupContainer1.TabIndex = 22;
      this.groupContainer1.Text = "GFE Print Selection";
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (GFESettings);
      this.Size = new Size(457, 279);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public override void Reset()
    {
      this.reset();
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      if (!this.IsDirty)
      {
        this.Dispose();
      }
      else
      {
        Session.ServerManager.UpdateServerSetting("Printing.GFE", (object) ((this.chkBoxBrokerEx.Checked ? 16 : 0) | (this.chkBoxBroker.Checked ? 2 : 0) | (this.chkBoxLenderEx.Checked ? 1 : 0) | (this.chkBoxLender.Checked ? 4 : 0) | (this.chkBoxItemization.Checked ? 8 : 0)));
        this.access = (GFEPrintingDefault) Session.ServerManager.GetServerSetting("Printing.GFE");
        this.setDirtyFlag(false);
      }
    }
  }
}
