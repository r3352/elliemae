// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.BorrowerStatusStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class BorrowerStatusStore
  {
    private const string className = "BorrowerStatusStore�";
    private const string xmlFileRelativePath = "Users\\BorrowerStatus.xml�";

    public static BorrowerStatus Get()
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (BorrowerStatusStore), LockType.ReadOnly))
        return BorrowerStatusStore.readFromDisk(current);
    }

    public static void Create(BorrowerStatusItem item)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (BorrowerStatusStore)))
      {
        BorrowerStatus borrowerStatus = BorrowerStatusStore.readFromDisk(current);
        borrowerStatus.Items = (BorrowerStatusItem[]) ArrayUtil.Add((Array) borrowerStatus.Items, (object) item);
        XmlDataStore.Serialize((object) borrowerStatus, BorrowerStatusStore.getFilePath(current));
      }
    }

    public static void Set(BorrowerStatusItem newField)
    {
      BorrowerStatusStore.Set(new BorrowerStatusItem[1]
      {
        newField
      });
    }

    public static void Set(BorrowerStatusItem[] newFields)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (BorrowerStatusStore)))
      {
        BorrowerStatus borrowerStatus = BorrowerStatusStore.readFromDisk(current);
        borrowerStatus.Items = newFields;
        XmlDataStore.Serialize((object) borrowerStatus, BorrowerStatusStore.getFilePath(current));
      }
    }

    public static void Update(int index, BorrowerStatusItem itemToUpdate)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (BorrowerStatusStore)))
      {
        BorrowerStatus borrowerStatus = BorrowerStatusStore.readFromDisk(current);
        foreach (BorrowerStatusItem borrowerStatusItem in borrowerStatus.Items)
        {
          if (borrowerStatusItem.index == index)
          {
            borrowerStatusItem.index = itemToUpdate.index;
            borrowerStatusItem.name = itemToUpdate.name;
            break;
          }
        }
        XmlDataStore.Serialize((object) borrowerStatus, BorrowerStatusStore.getFilePath(current));
      }
    }

    private static BorrowerStatus readFromDisk(ClientContext context)
    {
      BorrowerStatus borrowerStatus = (BorrowerStatus) context.Cache.Get(nameof (BorrowerStatusStore));
      if (borrowerStatus != null)
        return borrowerStatus;
      BorrowerStatus o = (BorrowerStatus) XmlDataStore.Deserialize(typeof (BorrowerStatus), BorrowerStatusStore.getFilePath(context));
      if (o.Items == null)
        o.Items = new BorrowerStatusItem[0];
      Array.Sort<BorrowerStatusItem>(o.Items);
      context.Cache.Put(nameof (BorrowerStatusStore), (object) o, CacheSetting.Low);
      return o;
    }

    private static string getFilePath(ClientContext context)
    {
      return context.Settings.GetDataFilePath("Users\\BorrowerStatus.xml");
    }
  }
}
