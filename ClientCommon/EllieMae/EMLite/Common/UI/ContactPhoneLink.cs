// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ContactPhoneLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ContactPhoneLink : PhoneImageLink
  {
    private int contactID;
    private string phoneNumber = "";
    private PhoneImageLink.PhoneType phoneType;

    public ContactPhoneLink(
      string criterialName,
      IPropertyDictionary dataHash,
      EventHandler eventHandler,
      PhoneImageLink.PhoneType phoneType)
      : base(string.Concat(dataHash[criterialName]), EncompassFonts.Normal1.ForeColor, eventHandler, phoneType)
    {
      this.phoneType = phoneType;
      try
      {
        this.contactID = Utils.ParseInt(dataHash["Contact.ContactID"], -1);
      }
      catch
      {
      }
      try
      {
        this.phoneNumber = string.Concat(dataHash[criterialName]);
      }
      catch
      {
      }
    }

    public string PhoneNumber => this.phoneNumber;

    public PhoneImageLink.PhoneType ContactPhoneType => this.phoneType;

    public override Rectangle Draw(ItemDrawArgs e)
    {
      return this.phoneNumber == "" ? Rectangle.Empty : base.Draw(e);
    }
  }
}
