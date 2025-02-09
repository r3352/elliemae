// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MbsPoolsSetup
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MbsPoolsSetup : SettingsUserControl
  {
    private bool oriValue;
    private IContainer components;
    private GroupContainer groupContainer1;
    private CheckBox ckboxEnableMbsPool;

    public MbsPoolsSetup(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.InitialPageValue();
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      Session.ServerManager.UpdateServerSetting("Trade.EnableMbsPool", (object) this.ckboxEnableMbsPool.Checked);
      this.setDirtyFlag(false);
    }

    private void InitialPageValue()
    {
      this.oriValue = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableMbsPool"]);
      this.ckboxEnableMbsPool.Checked = this.oriValue;
    }

    private void ckboxEnableMbsPool_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(this.ckboxEnableMbsPool.Checked != this.oriValue);
    }

    public override void Reset()
    {
      this.ckboxEnableMbsPool.Checked = this.oriValue;
      this.setDirtyFlag(false);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.ckboxEnableMbsPool = new CheckBox();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.ckboxEnableMbsPool);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(602, 427);
      this.groupContainer1.TabIndex = 8;
      this.groupContainer1.Text = "MBS Pools Setting";
      this.ckboxEnableMbsPool.AutoSize = true;
      this.ckboxEnableMbsPool.Location = new Point(16, 42);
      this.ckboxEnableMbsPool.Name = "ckboxEnableMbsPool";
      this.ckboxEnableMbsPool.Size = new Size(242, 17);
      this.ckboxEnableMbsPool.TabIndex = 0;
      this.ckboxEnableMbsPool.Text = "Enable MBS Pools Tab in Trade management";
      this.ckboxEnableMbsPool.UseVisualStyleBackColor = true;
      this.ckboxEnableMbsPool.CheckedChanged += new EventHandler(this.ckboxEnableMbsPool_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (MbsPoolsSetup);
      this.Size = new Size(602, 427);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
