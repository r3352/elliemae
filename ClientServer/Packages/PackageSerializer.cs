// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Packages.PackageSerializer
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Packages
{
  public class PackageSerializer
  {
    private SessionObjects sessionObjects;

    public PackageSerializer(SessionObjects sessionObjects) => this.sessionObjects = sessionObjects;

    public void Serialize(Stream serializationStream, object graph)
    {
      StreamWriter streamWriter = new StreamWriter(serializationStream, Encoding.Default);
      streamWriter.Write(this.Serialize(graph));
      streamWriter.Flush();
    }

    public string Serialize(object graph)
    {
      PackageSerializationInfo serializationInfo = new PackageSerializationInfo(this.sessionObjects);
      serializationInfo.AddValue("root", graph);
      return serializationInfo.ToString();
    }

    public object Deserialize(Stream serializationStream, Type valueType)
    {
      using (StreamReader streamReader = new StreamReader(serializationStream, Encoding.Default))
        return this.Deserialize(streamReader.ReadToEnd(), valueType);
    }

    public object Deserialize(string xmlData, Type valueType)
    {
      return new PackageSerializationInfo(this.sessionObjects, xmlData).GetValue("root", valueType);
    }
  }
}
