// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.CustomFormDetail
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class CustomFormDetail
  {
    private string source;
    private ForBorrowerType intendedFor;

    public CustomFormDetail(string source, ForBorrowerType intendedFor)
    {
      this.source = source;
      this.intendedFor = intendedFor;
    }

    public string Source
    {
      get => this.source;
      set => this.source = value;
    }

    public ForBorrowerType IntendedFor
    {
      get => this.intendedFor;
      set => this.intendedFor = value;
    }
  }
}
