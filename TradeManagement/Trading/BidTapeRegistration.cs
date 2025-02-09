// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.BidTapeRegistration
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using Elli.Web.Host.SSF.Context;
using Elli.Web.Host.SSF.UI;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class BidTapeRegistration : UserControl
  {
    private IContainer components;

    public BidTapeRegistration()
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      string tapeThinClientUrl = Session.SessionObjects.ConfigurationManager.GetBidTapeThinClientURL();
      string appSetting = EnConfigurationSettings.AppSettings["ThinClientBidTape.Url"];
      string str = string.IsNullOrEmpty(appSetting) ? tapeThinClientUrl : appSetting;
      SSFContext context = SSFContext.Create((LoanDataMgr) null, SSFHostType.Network, new SSFGuest()
      {
        uri = str,
        scope = "sec",
        clientId = "04gkefdw"
      });
      context.parameters = new Dictionary<string, object>()
      {
        {
          "oapiBaseUrl",
          (object) Session.StartupInfo.OAPIGatewayBaseUri
        }
      };
      SSFControl ssfControl = new SSFControl();
      ssfControl.LoadApp(context);
      this.Controls.Add((Control) ssfControl);
      ssfControl.Dock = DockStyle.Fill;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.AutoScaleMode = AutoScaleMode.Font;
    }
  }
}
