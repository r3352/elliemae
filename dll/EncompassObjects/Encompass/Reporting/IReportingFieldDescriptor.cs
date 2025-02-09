// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Reporting.IReportingFieldDescriptor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Reporting
{
  [Guid("0FE8FE3B-9F19-41dd-9D9E-A3BD91B92129")]
  public interface IReportingFieldDescriptor
  {
    string FieldID { get; }

    int BorrowerPair { get; }

    string CanonicalName { get; }

    string TableName { get; }

    string ColumnName { get; }

    bool Auditable { get; }

    string Description { get; }

    string FieldType { get; }

    int FieldSize { get; }
  }
}
