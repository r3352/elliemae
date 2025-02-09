// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.NotificationTemplatesSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ePass.eFolder;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.LoanUtils.Authentication;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.ThinThick;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class NotificationTemplatesSetupControl : UserControl
  {
    private LoadWebPageControl loadWebPageControl;
    private IContainer components;

    public NotificationTemplatesSetupControl()
    {
      this.InitializeComponent();
      if (!NotificationTemplateRestClient.MigrateEConsentTemplates(Session.SessionObjects).Success && Session.SessionObjects.StartupInfo.OtpSupport)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "We were unable to pull your HTML email templates for eConsent. Please relaunch Notification Templates.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      AccessToken accessToken = this.getAccessToken();
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      webPageParams.Add("oapiBaseUrl", (object) Session.StartupInfo.OAPIGatewayBaseUri);
      if (accessToken != null)
        webPageParams.Add("authToken", (object) accessToken.Token);
      string appSetting = EnConfigurationSettings.AppSettings["NotificationTemplates.Url"];
      string webPageURL = string.IsNullOrEmpty(appSetting) ? Session.SessionObjects.StartupInfo.NotificationTemplatesUrl : appSetting;
      this.loadWebPageControl = new LoadWebPageControl(webPageURL, webPageParams, "sc", webPageURL + "host.html?r=", new Elli.Web.Host.PostMessageHandler(this.PostMessageHandler));
      this.Controls.Add((Control) this.loadWebPageControl);
      this.loadWebPageControl.Dock = DockStyle.Fill;
    }

    public AccessToken getAccessToken()
    {
      AccessToken accessToken = (AccessToken) null;
      try
      {
        accessToken = new OAuth2Utils(Session.ISession, Session.StartupInfo).GetAccessToken("sc", true);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(this.GetType().Name, "NotificationTemplatesSetupControl: GetAccessToken error: " + (object) ex);
      }
      return accessToken;
    }

    public string PostMessageHandler(string payload)
    {
      TraceLog.WriteVerbose(this.GetType().Name, "NotificationTemplatesSetupControl: PostMessage: " + payload);
      string str = string.Empty;
      try
      {
        ThinThickPostMessage thickPostMessage = JsonConvert.DeserializeObject<ThinThickPostMessage>(payload);
        if (thickPostMessage.action == "AddEmailTemplateFields")
        {
          ThinThickRaiseMessage thickRaiseMessage = new ThinThickRaiseMessage()
          {
            type = "FieldsAdded",
            payload = new ThinThickRaiseMessagePayload()
          };
          using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(Session.DefaultInstance, thickPostMessage.fieldIds, true, string.Empty, true, true))
          {
            if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
              thickRaiseMessage.payload.fieldIds = ruleFindFieldDialog.SelectedRequiredFields;
          }
          str = JsonConvert.SerializeObject((object) thickRaiseMessage);
        }
        return str;
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(this.GetType().Name, "NotificationTemplatesSetupControl: PostMessage failed: " + (object) ex);
        return "NotificationTemplatesSetupControl: PostMessage failed: " + (object) ex;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Name = nameof (NotificationTemplatesSetupControl);
      this.Size = new Size(648, 565);
      this.ResumeLayout(false);
    }
  }
}
