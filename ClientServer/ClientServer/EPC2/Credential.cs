// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EPC2.Credential
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.EPC2
{
  [Serializable]
  public class Credential
  {
    public string Id { get; set; }

    public string Type { get; set; }

    public string Title { get; set; }

    public int Minimum { get; set; }

    public int Maximum { get; set; }

    public bool Secret { get; set; }

    public string Scope { get; set; }

    public bool Required { get; set; }
  }
}
