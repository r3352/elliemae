// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ContactGroupsLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ContactGroupsLink : ImageLink
  {
    private int groupCount;
    private Rectangle imageRectangle = Rectangle.Empty;
    private IPropertyDictionary dataHash;
    private EllieMae.EMLite.ContactUI.ContactType contactType;
    private int contactID;
    private const int imageSpacing = 10;

    public ContactGroupsLink(int groupCount, EventHandler clickHandler)
      : base((Element) new FormattedText("(" + (object) groupCount + ")", EncompassFonts.Normal1.ForeColor), (Image) Resources.borrower_contact_group, (Image) Resources.borrower_contact_group_over, clickHandler)
    {
      this.groupCount = groupCount;
      this.GetImageLink();
    }

    public ContactGroupsLink(
      string criterialName,
      IPropertyDictionary dataHash,
      EllieMae.EMLite.ContactUI.ContactType contactType)
      : base((Element) new FormattedText("(" + (object) Utils.ParseInt(dataHash[criterialName], 0) + ")", EncompassFonts.Normal1.ForeColor), (Image) Resources.borrower_contact_group, (Image) Resources.borrower_contact_group_over, (EventHandler) null)
    {
      this.dataHash = dataHash;
      this.contactType = contactType;
      this.contactID = Utils.ParseInt(dataHash["Contact.ContactID"], -1);
      this.groupCount = Utils.ParseInt(dataHash[criterialName], 0);
      this.Click += new EventHandler(this.ContactGroupsLink_Click);
      this.GetImageLink();
    }

    private void ContactGroupsLink_Click(object sender, EventArgs e)
    {
      Point position = Cursor.Position;
      this.createGroupsPopup(Session.ContactGroupManager.GetContactGroupsForContact(this.contactType, this.contactID))?.Show(position);
    }

    private ContextMenuStrip createGroupsPopup(ContactGroupInfo[] contactGroups)
    {
      ContextMenuStrip groupsPopup = new ContextMenuStrip();
      groupsPopup.ShowImageMargin = false;
      groupsPopup.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) "Contact Groups", ToolStripMenuItemEx.ToolStripItemType.Header));
      int num = 1;
      foreach (ContactGroupInfo contactGroup in contactGroups)
      {
        if (this.contactType == EllieMae.EMLite.ContactUI.ContactType.PublicBiz || !(contactGroup.UserId != Session.UserID))
          groupsPopup.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) string.Format("{0}. {1}", (object) num++, (object) contactGroup.GroupName), ToolStripMenuItemEx.ToolStripItemType.Label));
      }
      return groupsPopup;
    }

    public void GetImageLink()
    {
      switch (this.contactType)
      {
        case EllieMae.EMLite.ContactUI.ContactType.Borrower:
          this.NormalImage = (Image) Resources.borrower_contact_group;
          this.HotImage = (Image) Resources.borrower_contact_group_over;
          break;
        case EllieMae.EMLite.ContactUI.ContactType.BizPartner:
        case EllieMae.EMLite.ContactUI.ContactType.PublicBiz:
          this.NormalImage = (Image) Resources.business_contact_group;
          this.HotImage = (Image) Resources.business_contact_group_over;
          break;
      }
    }
  }
}
