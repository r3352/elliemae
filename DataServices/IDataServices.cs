// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataServices.IDataServices
// Assembly: DataServices, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 227B0203-DF45-468D-9C1B-FA6CED472E23
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataServices.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.DataServices
{
  public interface IDataServices
  {
    IBam Bam { get; set; }

    bool UpdateReport(Hashtable data);

    bool DataValidation();

    Hashtable GetData();

    bool Compare(Hashtable compareData);
  }
}
