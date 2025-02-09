// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LockSnapshotRecaptureStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Classes;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class LockSnapshotRecaptureStore
  {
    private static string FileName
    {
      get => ClientContext.GetCurrent(false).Settings.LoansDir + "\\LockSnapshotRecapture.txt";
    }

    public void Save(LockSnapshotRecapture lockSnapshotRecapture)
    {
      string json = lockSnapshotRecapture.ToJson();
      try
      {
        if (!File.Exists(LockSnapshotRecaptureStore.FileName))
        {
          using (StreamWriter text = File.CreateText(LockSnapshotRecaptureStore.FileName))
            text.WriteLine(json);
        }
        using (StreamWriter streamWriter = File.AppendText(LockSnapshotRecaptureStore.FileName))
          streamWriter.WriteLine(json);
      }
      catch (Exception ex)
      {
      }
    }

    public List<LockSnapshotRecapture> GetList()
    {
      List<LockSnapshotRecapture> list = new List<LockSnapshotRecapture>();
      if (File.Exists(LockSnapshotRecaptureStore.FileName))
      {
        using (StreamReader streamReader = File.OpenText(LockSnapshotRecaptureStore.FileName))
        {
          while (!streamReader.EndOfStream)
            list.Add(new LockSnapshotRecapture(streamReader.ReadLine()));
        }
      }
      return list;
    }
  }
}
