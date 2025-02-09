// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.IDataExchange
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>Represents the interface for the DataObject object.</summary>
  /// <exclude />
  [Guid("423F7C56-F3B7-4676-B7C4-0CCD5AD86E7B")]
  public interface IDataExchange
  {
    int PostDataToUser(string userId, object data);

    int PostDataToSession(string sessionId, object data);

    int PostDataToAll(object data);

    DataObject GetCustomDataObject(string filePath);

    void SaveCustomDataObject(string filePath, DataObject dataObj);

    void AppendToCustomDataObject(string filePath, DataObject dataObj);

    void DeleteCustomDataObject(string filePath);
  }
}
