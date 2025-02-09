// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.UserLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class UserLink : PipelineImageLink
  {
    private UserInfo user;
    private string userId;
    private string userName;

    public UserLink(Control parentControl, PipelineElementData data)
      : base(parentControl, data, (Element) new TextElement(string.Concat(data.GetValue())), (Image) Resources.business_contact, (Image) Resources.business_contact_over)
    {
      this.userName = string.Concat(data.GetValue());
      this.userId = string.Compare(data.FieldName, "CurrentLoanAssociate.FullName", true) != 0 ? string.Concat(data.PipelineInfo.GetField(data.FieldName.Replace("Name", "ID"))) : string.Concat(data.PipelineInfo.GetField("CurrentLoanAssociate.UserID"));
      if (!(this.userId == ""))
        return;
      this.userId = this.userName;
    }

    public UserLink(Control parentControl, string userId, string userName)
      : base(parentControl, (Image) Resources.business_contact, (Image) Resources.business_contact_over)
    {
      this.userId = userId;
      this.userName = userName;
    }

    protected override void OnLinkClicked(object sender, EventArgs e)
    {
      Point position = Cursor.Position;
      this.createContextMenu()?.Show(position);
    }

    private ContextMenuStrip createContextMenu()
    {
      ContextMenuStrip contextMenu = new ContextMenuStrip();
      contextMenu.ShowImageMargin = false;
      ObjectWithImage dataSource = new ObjectWithImage(this.userName, (Image) Resources.business_contact_icon_menu);
      contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) dataSource, ToolStripMenuItemEx.ToolStripItemType.Header));
      this.user = Session.OrganizationManager.GetUser(this.userId);
      if (this.user == (UserInfo) null)
      {
        contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) new FormattedText("User information not available"), ToolStripMenuItemEx.ToolStripItemType.Label));
      }
      else
      {
        if (this.user.Phone != "")
          contextMenu.Items.Add((ToolStripItem) this.createPhoneLinkMenuItem(this.user.Phone, PhoneLink.ContactType.WorkPhone));
        if (this.user.CellPhone != "")
          contextMenu.Items.Add((ToolStripItem) this.createPhoneLinkMenuItem(this.user.CellPhone, PhoneLink.ContactType.CellPhone));
        if (this.user.Email != "")
          contextMenu.Items.Add((ToolStripItem) this.createEmailLinkMenuItem(this.user.Email));
      }
      return contextMenu;
    }

    private ToolStripMenuItem createPhoneLinkMenuItem(
      string phoneNumber,
      PhoneLink.ContactType phoneType)
    {
      ToolStripMenuItemEx phoneLinkMenuItem = new ToolStripMenuItemEx((object) new PhoneLink(phoneNumber, phoneType, false), ToolStripMenuItemEx.ToolStripItemType.Clickable);
      phoneLinkMenuItem.Click += new EventHandler(this.onPhoneMenuClick);
      return (ToolStripMenuItem) phoneLinkMenuItem;
    }

    private ToolStripMenuItem createEmailLinkMenuItem(string emailAddr)
    {
      ToolStripMenuItemEx emailLinkMenuItem = new ToolStripMenuItemEx((object) new ObjectWithImage(emailAddr, (Image) Resources.email_icon_menu), ToolStripMenuItemEx.ToolStripItemType.Clickable);
      emailLinkMenuItem.Click += new EventHandler(this.onEmailMenuClick);
      return (ToolStripMenuItem) emailLinkMenuItem;
    }

    private void onEmailMenuClick(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<ILoanEditor>().StartConversation(new ConversationLog(DateTime.Now, Session.UserID)
      {
        IsEmail = true,
        Name = this.user.FullName,
        Email = this.user.Email
      });
    }

    private void onPhoneMenuClick(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<ILoanEditor>().StartConversation(new ConversationLog(DateTime.Now, Session.UserID)
      {
        IsEmail = false,
        Name = this.user.FullName,
        Phone = string.Concat(((ToolStripMenuItemEx) sender).DataSource)
      });
    }
  }
}
