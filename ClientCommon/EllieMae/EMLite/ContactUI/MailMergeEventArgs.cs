// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.MailMergeEventArgs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class MailMergeEventArgs : EventArgs
  {
    private int[] contactIds;
    private ContactType contactType;

    public MailMergeEventArgs(int[] contactIds, ContactType contactType)
    {
      this.contactIds = contactIds;
      this.contactType = contactType;
    }

    public MailMergeEventArgs(int contactId, ContactType contactType)
    {
      this.contactIds = new int[1]{ contactId };
      this.contactType = contactType;
    }

    public int[] ContactIDs => this.contactIds;

    public ContactType ContactType => this.contactType;
  }
}
