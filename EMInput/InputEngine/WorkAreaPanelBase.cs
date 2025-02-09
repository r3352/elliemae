// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.WorkAreaPanelBase
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.UI;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class WorkAreaPanelBase : Panel, IOnlineHelpTarget
  {
    protected GradientPanel titlePanel;
    protected BorderPanel contentPanel;
    protected EMHelpLink emHelpLink1;
    protected Label titleLbl;
    protected Label alertLbl;
    private Color originalTitleColor1;
    private Color originalTitleColor2;
    private string originalTitle = string.Empty;
    private string helpTopic;

    public WorkAreaPanelBase()
    {
      this.InitializeComponent();
      this.originalTitleColor1 = this.titlePanel.GradientColor1;
      this.originalTitleColor2 = this.titlePanel.GradientColor2;
      this.emHelpLink1.Visible = false;
    }

    private void InitializeComponent()
    {
      this.titlePanel = new GradientPanel();
      this.titleLbl = new Label();
      this.alertLbl = new Label();
      this.contentPanel = new BorderPanel();
      this.emHelpLink1 = new EMHelpLink();
      this.titlePanel.SuspendLayout();
      this.SuspendLayout();
      this.titlePanel.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.titlePanel.Controls.Add((Control) this.titleLbl);
      this.titlePanel.Controls.Add((Control) this.emHelpLink1);
      this.titlePanel.Controls.Add((Control) this.alertLbl);
      this.titlePanel.Dock = DockStyle.Top;
      this.titlePanel.Location = new Point(0, 0);
      this.titlePanel.Name = "titlePanel";
      this.titlePanel.Size = new Size(464, 26);
      this.titlePanel.TabIndex = 1;
      this.titleLbl.BackColor = Color.Transparent;
      this.titleLbl.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.titleLbl.ForeColor = SystemColors.ControlText;
      this.titleLbl.Location = new Point(8, 6);
      this.titleLbl.Name = "titleLbl";
      this.titleLbl.Size = new Size(400, 26);
      this.titleLbl.AutoSize = true;
      this.titleLbl.TabIndex = 0;
      this.titleLbl.Text = "Form Title";
      this.titleLbl.TextAlign = ContentAlignment.MiddleLeft;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.Location = new Point(400, 6);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.DisplayOption = HelpLinkDisplayOption.IconOnly;
      this.emHelpLink1.Size = new Size(16, 16);
      this.emHelpLink1.TabIndex = 9;
      this.emHelpLink1.Visible = false;
      this.alertLbl.BackColor = Color.Transparent;
      this.alertLbl.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.alertLbl.ForeColor = SystemColors.ControlText;
      this.alertLbl.Location = new Point(408, 6);
      this.alertLbl.Name = "alertLbl";
      this.alertLbl.Size = new Size(417, 26);
      this.alertLbl.AutoSize = true;
      this.alertLbl.TabIndex = 5;
      this.alertLbl.Text = "";
      this.alertLbl.TextAlign = ContentAlignment.MiddleLeft;
      this.alertLbl.Visible = false;
      this.contentPanel.Dock = DockStyle.Fill;
      this.contentPanel.Location = new Point(0, 26);
      this.contentPanel.Name = "contentPanel";
      this.contentPanel.Size = new Size(464, 374);
      this.contentPanel.TabIndex = 2;
      this.Controls.Add((Control) this.contentPanel);
      this.Controls.Add((Control) this.titlePanel);
      this.Dock = DockStyle.Fill;
      this.Name = "panel1";
      this.Size = new Size(464, 400);
      this.titlePanel.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public void SetTitle(string title) => this.SetTitle(title, (Control) null);

    public void RemoveTitle()
    {
      this.titlePanel.Controls.Clear();
      this.titlePanel.Visible = false;
    }

    public void AddAlertToTitle(string alertMessage)
    {
      this.alertLbl.ForeColor = AppColors.AlertRed;
      this.alertLbl.Text = alertMessage;
      if (this.emHelpLink1.Visible)
        this.alertLbl.Left = this.titleLbl.Left + this.titleLbl.Width + 1 + this.emHelpLink1.Width + 1;
      else
        this.alertLbl.Left = this.titleLbl.Left + this.titleLbl.Width + 1;
      this.alertLbl.Visible = true;
      this.titlePanel.Controls.Add((Control) this.alertLbl);
    }

    public void RemoveAlertFromTitle()
    {
      this.alertLbl.Visible = false;
      this.alertLbl.Text = "";
      this.titlePanel.Controls.Remove((Control) this.alertLbl);
    }

    public void SetTitleAlert(bool alert, string alertMessage)
    {
      this.titlePanel.BackColor = AppColors.AlertRed;
      this.titleLbl.Text = alert ? alertMessage : this.originalTitle;
      this.titleLbl.ForeColor = alert ? Color.White : Color.Black;
      this.alertLbl.Visible = false;
    }

    public void ShowBorder()
    {
      this.contentPanel.Borders = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.contentPanel.BorderStyle = BorderStyle.FixedSingle;
    }

    public void RemoveBorder()
    {
      this.contentPanel.Borders = AnchorStyles.None;
      this.contentPanel.BorderStyle = BorderStyle.None;
    }

    public void RemoveScrollBar() => this.AutoScroll = false;

    public void SetTitle(string title, Control navControl)
    {
      this.titlePanel.Controls.Clear();
      this.titlePanel.Controls.Add((Control) this.titleLbl);
      if (navControl != null)
      {
        this.titlePanel.Controls.Add(navControl);
        navControl.Dock = DockStyle.Right;
      }
      this.titleLbl.Text = title;
      this.originalTitle = title;
      this.helpTopic = (string) null;
      this.alertLbl.Visible = false;
    }

    public void SetTitleOnly(string title)
    {
      this.titleLbl.Text = title;
      this.originalTitle = title;
      this.helpTopic = (string) null;
      this.alertLbl.Visible = false;
    }

    public void AddHelpIconToTitle(string helpLink)
    {
      this.emHelpLink1.HelpTag = helpLink;
      this.emHelpLink1.Left = this.titleLbl.Left + this.titleLbl.Width + 1;
      this.emHelpLink1.Visible = true;
      this.titlePanel.Controls.Add((Control) this.emHelpLink1);
    }

    public void SetHelpTopic(string helpTopic)
    {
      if (string.IsNullOrEmpty(JedHelp.GetMapId(helpTopic)))
        return;
      this.helpTopic = helpTopic;
    }

    public virtual string GetHelpTargetName()
    {
      return this.helpTopic == null ? this.originalTitle : this.helpTopic;
    }
  }
}
