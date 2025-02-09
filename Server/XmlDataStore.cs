// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.XmlDataStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class XmlDataStore
  {
    private const string className = "XmlDataStore�";

    public static object Deserialize(Type objType, string filename)
    {
      BinaryObject binaryObject = (BinaryObject) null;
      using (DataFile latestVersion = FileStore.GetLatestVersion(filename))
      {
        if (latestVersion.Exists)
          binaryObject = latestVersion.GetData();
      }
      try
      {
        if (binaryObject != null)
        {
          object obj = new XmlSerializer().Deserialize(binaryObject.AsStream(), objType);
          binaryObject.Dispose();
          return obj;
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (XmlDataStore), "Error deserializing xml file: " + filename + ": " + (object) ex);
      }
      finally
      {
        binaryObject?.Dispose();
      }
      return XmlDataStore.CreateNew(objType);
    }

    public static object CreateNew(Type objType)
    {
      return objType.GetConstructor(Type.EmptyTypes).Invoke((object[]) null);
    }

    public static void Serialize(object obj, string filename)
    {
      try
      {
        XmlSerializer xmlSerializer = new XmlSerializer();
        ByteStream byteStream = new ByteStream(false);
        xmlSerializer.Serialize((Stream) byteStream, obj);
        using (BinaryObject data = new BinaryObject((Stream) byteStream, false))
        {
          using (DataFile dataFile = FileStore.CheckOut(filename, MutexAccess.Write))
            dataFile.CheckIn(data);
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (XmlDataStore), "Error saving xml file " + filename + ": " + (object) ex);
        Err.Reraise(nameof (XmlDataStore), ex);
      }
    }
  }
}
