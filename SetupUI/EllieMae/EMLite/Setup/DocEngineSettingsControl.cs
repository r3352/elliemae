// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DocEngineSettingsControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DocEngineSettingsControl : SettingsUserControl
  {
    private Sessions.Session session;
    private PlanCodeManagementControl planCodeCtl;
    private StackingOrderMgmtControl stackingOrderCtl;
    private IContainer components;
    private TabControl tbSettings;
    private TabPage tpPlanCodes;
    private TabPage tpStackingOrders;

    public DocEngineSettingsControl(SetUpContainer container, Sessions.Session session)
      : base(container)
    {
      this.session = session;
      this.InitializeComponent();
      this.planCodeCtl = new PlanCodeManagementControl(container, session, DocumentOrderType.Opening);
      this.planCodeCtl.Dock = DockStyle.Fill;
      this.tpPlanCodes.Controls.Add((Control) this.planCodeCtl);
      this.stackingOrderCtl = new StackingOrderMgmtControl((SetUpContainer) null, session, DocumentOrderType.Opening);
      this.stackingOrderCtl.Dock = DockStyle.Fill;
      this.tpStackingOrders.Controls.Add((Control) this.stackingOrderCtl);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tbSettings = new TabControl();
      this.tpPlanCodes = new TabPage();
      this.tpStackingOrders = new TabPage();
      this.tbSettings.SuspendLayout();
      this.SuspendLayout();
      this.tbSettings.Controls.Add((Control) this.tpPlanCodes);
      this.tbSettings.Controls.Add((Control) this.tpStackingOrders);
      this.tbSettings.Dock = DockStyle.Fill;
      this.tbSettings.Location = new Point(5, 5);
      this.tbSettings.Name = "tbSettings";
      this.tbSettings.SelectedIndex = 0;
      this.tbSettings.Size = new Size(728, 548);
      this.tbSettings.TabIndex = 0;
      this.tpPlanCodes.Location = new Point(4, 23);
      this.tpPlanCodes.Name = "tpPlanCodes";
      this.tpPlanCodes.Padding = new Padding(3);
      this.tpPlanCodes.Size = new Size(720, 521);
      this.tpPlanCodes.TabIndex = 0;
      this.tpPlanCodes.Text = "Plan Codes & Settings";
      this.tpPlanCodes.UseVisualStyleBackColor = true;
      this.tpStackingOrders.Location = new Point(4, 23);
      this.tpStackingOrders.Name = "tpStackingOrders";
      this.tpStackingOrders.Padding = new Padding(3);
      this.tpStackingOrders.Size = new Size(732, 533);
      this.tpStackingOrders.TabIndex = 1;
      this.tpStackingOrders.Text = "Stacking Orders";
      this.tpStackingOrders.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.BorderStyle = BorderStyle.FixedSingle;
      this.Controls.Add((Control) this.tbSettings);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (DocEngineSettingsControl);
      this.Padding = new Padding(5);
      this.Size = new Size(738, 558);
      this.tbSettings.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
