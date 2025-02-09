// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.IDataObject
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects
{
  /// <summary>Represents the interface for the DataObject object.</summary>
  /// <exclude />
  [Guid("21795A57-3B14-4eb6-8078-29C267A59E08")]
  public interface IDataObject
  {
    int Size { get; }

    byte[] Data { get; }

    void Load(byte[] objectData);

    void LoadFile(string filePath);

    void SaveToDisk(string filePath);
  }
}
