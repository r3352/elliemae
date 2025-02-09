// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.IFeeManagementRecord
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>Interface for the FeeManagementRecord class</summary>
  /// <exclude />
  [Guid("B7DFA440-D517-480E-B076-6BC048DF1272")]
  public interface IFeeManagementRecord
  {
    int ID { get; }

    string Name { get; }

    string MaventFeeName { get; }

    string FeeSource { get; }

    bool For800 { get; }

    bool For900 { get; }

    bool For1000 { get; }

    bool For1100 { get; }

    bool For1200 { get; }

    bool For1300 { get; }

    bool ForPC { get; }
  }
}
