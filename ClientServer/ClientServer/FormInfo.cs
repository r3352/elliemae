// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FormInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FormInfo
  {
    private string name;
    private OutputFormType type;
    private PrintForm.MergeLocationValues _MergeLocation;
    private MergeParamValues _MergeParams = new MergeParamValues();

    public FormInfo(string name, OutputFormType type)
    {
      this.name = name;
      this.type = type;
    }

    public string Name => this.name;

    public OutputFormType Type => this.type;

    public PrintForm.MergeLocationValues MergeLocation
    {
      get => this._MergeLocation;
      set => this._MergeLocation = value;
    }

    public MergeParamValues MergeParams
    {
      get => this._MergeParams;
      set => this._MergeParams = new MergeParamValues(value);
    }
  }
}
