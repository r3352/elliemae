// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ChangeCircumstanceSettings
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ChangeCircumstanceSettings
  {
    public List<string> LineNumbers = new List<string>();
    public bool IsItemLinesUpdated;

    public int optionId { get; set; }

    public string Description { get; set; }

    public string Code { get; set; }

    public string Comment { get; set; }

    public int Reason { get; set; }

    public int optionOrder { get; set; }

    public string CocType { get; set; }
  }
}
