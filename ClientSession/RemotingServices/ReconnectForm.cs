// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.ReconnectForm
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.SimpleCache;
using EllieMae.EMLite.eFolder.Files;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class ReconnectForm : Form
  {
    private bool isHTTP;
    private Sessions.Session session;
    private bool result;
    private static int randomDelayMin = -1;
    private static int randomDelayMax = -1;
    private IContainer components;
    private Label label1;
    private Label label2;

    public ReconnectForm(bool shuReconnect)
      : this(shuReconnect, -1)
    {
    }

    public ReconnectForm(bool shuReconnect, int waitTime)
    {
      this.InitializeComponent();
      if (shuReconnect)
      {
        if (waitTime > 0)
          this.label1.Text = string.Format("Encompass server is about to install updates in {0} seconds. Please wait...", (object) waitTime);
        else
          this.label1.Text = "Encompass server is installing server hot updates. Please wait...";
      }
      else
        this.label1.Text = "Lost connection to Encompass server. Trying to reconnect. Please wait...";
      this.isHTTP = Session.DefaultInstance.ServerIdentity.IsHttp;
      this.session = Session.DefaultInstance;
      new Thread(new ParameterizedThreadStart(this.threadStart))
      {
        IsBackground = true
      }.Start((object) waitTime);
    }

    private void hide()
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new MethodInvoker(this.hide));
      }
      else
      {
        this.DialogResult = this.result ? DialogResult.Yes : DialogResult.No;
        this.Hide();
      }
    }

    private void threadStart(object arg)
    {
      int num = (int) arg;
      if (num > 0)
        Thread.Sleep(num * 1000);
      else if (num < 0)
        this.result = this.reconnect();
      this.hide();
    }

    private void randomDelay()
    {
      try
      {
        if (ReconnectForm.randomDelayMin < 0 && ReconnectForm.randomDelayMax < 0)
        {
          string attribute = SmartClientUtils.GetAttribute(this.session.CompanyInfo.ClientID, "16.3.1", "AutoReconnectRandomDelay");
          if (string.IsNullOrWhiteSpace(attribute))
          {
            ReconnectForm.randomDelayMin = ReconnectForm.randomDelayMax = 0;
          }
          else
          {
            string[] strArray = attribute.Split(',');
            if (strArray.Length != 0)
            {
              try
              {
                ReconnectForm.randomDelayMin = Convert.ToInt32(strArray[0]);
              }
              catch
              {
                ReconnectForm.randomDelayMin = 0;
              }
            }
            if (strArray.Length > 1)
            {
              try
              {
                ReconnectForm.randomDelayMax = Convert.ToInt32(strArray[1]);
              }
              catch
              {
                ReconnectForm.randomDelayMax = 0;
              }
            }
          }
        }
        if (ReconnectForm.randomDelayMin < 0 || ReconnectForm.randomDelayMax <= 0 || ReconnectForm.randomDelayMax < ReconnectForm.randomDelayMin)
          return;
        Thread.Sleep(new Random().Next(ReconnectForm.randomDelayMin, ReconnectForm.randomDelayMax) * 1000);
      }
      catch
      {
        ReconnectForm.randomDelayMin = ReconnectForm.randomDelayMax = 0;
      }
    }

    private string getAuthCode()
    {
      return new OAuth2(Session.DefaultInstance.StartupInfo.OAPIGatewayBaseUri, Session.StartupInfo.SSFClientId, Session.StartupInfo.SSFClientSecret, new RetrySettings(Session.SessionObjects), CacheItemRetentionPolicy.NoRetention).GenerateGuestApplicationAuthCode(Session.DefaultInstance.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, this.session.CompanyInfo.ClientID);
    }

    private bool reconnect()
    {
      string remoteServer = this.session.RemoteServer;
      string userId = this.session.UserID;
      string password = this.session.Password;
      string withoutExtension = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
      string sessionId = this.session.SessionID;
      int num1 = 10;
      int num2 = num1;
      while (num2 > 0)
      {
        try
        {
          if (this.isHTTP)
            Thread.Sleep(10000);
          this.randomDelay();
          string authCode = this.getAuthCode();
          Session.Start(remoteServer, userId, "", withoutExtension, false, (string) null, authCode);
          if (Session.LoanDataMgr != null)
          {
            Session.LoanDataMgr.ResetSessionObjects(Session.DefaultInstance.SessionObjects);
            BackgroundAttachmentDialog.CreateInstance(Session.ISession, Session.StartupInfo, true);
          }
          return true;
        }
        catch (Exception ex)
        {
          if (num2 == 10)
            MetricsFactory.IncrementCounter("LoanErrorIncCounter", new SFxTag("ErrorType=", "LostConnectivity"), (SFxTag) new SFxUiTag(), new SFxTag("ExceptionType", ex.GetType().ToString()));
          --num2;
          if (num2 == 0)
          {
            if (Utils.Dialog((IWin32Window) this, "Unable to connect to Encompass server. Try again?\r\n\r\nIf you click No, you may lose your changes.", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
              return false;
            num2 = num1;
          }
          else
            Thread.Sleep(3000);
        }
      }
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ReconnectForm));
      this.label1 = new Label();
      this.label2 = new Label();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(355, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Lost connection to Encompass server. Trying to reconnect.  Please wait...";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 37);
      this.label2.Name = "label2";
      this.label2.Size = new Size(323, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Closing this window will lose the chance to reconnect to the server.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(383, 65);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ReconnectForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Reconnect to Encompass Server";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
