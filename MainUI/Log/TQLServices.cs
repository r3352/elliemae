// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.TQLServices
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.LoanServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class TQLServices : UserControl, IOnlineHelpTarget
  {
    private const string className = "TQLServices";
    private IContainer components;

    public TQLServices()
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      if (!EpassLogin.LoginRequired(true))
        return;
      try
      {
        if (!LoanServiceManager.TQLClientInstalled)
          return;
        Control tqlClientControl = LoanServiceManager.GetTQLClientControl(string.Empty);
        tqlClientControl.Dock = DockStyle.Fill;
        this.Controls.Add(tqlClientControl);
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\TQLServices\\TQLServices.cs", ".ctor", 52);
        int num = (int) Utils.Dialog((IWin32Window) null, "The following error occurred when trying to create TQL Client Service Control :\n\n" + ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public string GetHelpTargetName() => nameof (TQLServices);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (TQLServices);
      this.Size = new Size(645, 680);
      this.ResumeLayout(false);
    }
  }
}
