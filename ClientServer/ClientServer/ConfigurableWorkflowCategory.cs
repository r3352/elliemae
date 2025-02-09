// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ConfigurableWorkflowCategory
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ConfigurableWorkflowCategory
  {
    public string GUID { get; set; }

    public string CategoryName { get; set; }

    public int SequenceNum { get; set; }

    public List<ConfigurableWorkflowSubCategory> SubCategories { get; set; }

    public ConfigurableWorkflowCategory()
    {
      this.SubCategories = new List<ConfigurableWorkflowSubCategory>();
    }
  }
}
