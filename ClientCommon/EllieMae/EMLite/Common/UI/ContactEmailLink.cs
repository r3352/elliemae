// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ContactEmailLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ContactEmailLink : EmailLink
  {
    private int contactID;
    private string emailLink = "";

    public ContactEmailLink(
      string criterialName,
      IPropertyDictionary dataHash,
      EventHandler eventHandler)
      : base(string.Concat(dataHash[criterialName]), EncompassFonts.Normal1.ForeColor, eventHandler)
    {
      try
      {
        this.contactID = Utils.ParseInt(dataHash["Contact.ContactID"], -1);
      }
      catch
      {
      }
      try
      {
        this.emailLink = string.Concat(dataHash[criterialName]);
      }
      catch
      {
      }
    }

    public string EmailAddress => this.emailLink;

    public override Rectangle Draw(ItemDrawArgs e)
    {
      return this.emailLink == "" ? Rectangle.Empty : base.Draw(e);
    }
  }
}
