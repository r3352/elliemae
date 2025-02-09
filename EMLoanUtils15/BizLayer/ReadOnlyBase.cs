// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BizLayer.ReadOnlyBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace EllieMae.EMLite.BizLayer
{
  [Serializable]
  public abstract class ReadOnlyBase : ICloneable
  {
    public object Clone()
    {
      MemoryStream serializationStream = new MemoryStream();
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      binaryFormatter.Serialize((Stream) serializationStream, (object) this);
      serializationStream.Position = 0L;
      return binaryFormatter.Deserialize((Stream) serializationStream);
    }
  }
}
