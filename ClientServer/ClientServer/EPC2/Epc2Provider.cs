// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EPC2.Epc2Provider
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.EPC2
{
  [Serializable]
  public class Epc2Provider
  {
    public string Id { get; set; }

    public string PartnerId { get; set; }

    public string ProductName { get; set; }

    public string Status { get; set; }

    public VendorPlatform VendorPlatform { get; set; }

    public string InterfaceUrl { get; set; }

    public string HostAdapterUrl { get; set; }

    public List<string> Categories { get; set; }

    public List<string> Workflows { get; set; }

    public string ListingName { get; set; }

    public List<Credential> Credentials { get; set; }

    public int ExtensionLimit { get; set; }

    public List<string> Applications { get; set; }

    public List<EllieMae.EMLite.ClientServer.EPC2.AdditionalLinks> AdditionalLinks { get; set; }
  }
}
