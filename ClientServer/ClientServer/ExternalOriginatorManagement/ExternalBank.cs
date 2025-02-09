// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOriginatorManagement.ExternalBank
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ExternalOriginatorManagement
{
  [Serializable]
  public class ExternalBank
  {
    public virtual int BankID { get; set; }

    public virtual string BankName { get; set; }

    public virtual string Address { get; set; }

    public virtual string Address1 { get; set; }

    public virtual string City { get; set; }

    public virtual string State { get; set; }

    public virtual string Zip { get; set; }

    public virtual string ContactName { get; set; }

    public virtual string ContactEmail { get; set; }

    public virtual string ContactPhone { get; set; }

    public virtual string ContactFax { get; set; }

    public virtual string ABANumber { get; set; }

    public virtual DateTime DateAdded { get; set; }

    public virtual string TimeZone { get; set; }
  }
}
