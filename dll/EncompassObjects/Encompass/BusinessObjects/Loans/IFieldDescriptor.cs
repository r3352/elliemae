// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IFieldDescriptor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("68FA83BF-9A89-4327-85DE-5ECEFF037E27")]
  public interface IFieldDescriptor
  {
    string FieldID { get; }

    LoanFieldFormat Format { get; }

    string Description { get; }

    int MaxLength { get; }

    bool MultiInstance { get; }

    FieldOptions Options { get; }

    bool IsCustomField { get; }

    bool IsNumeric();

    string FormatValue(string v);

    string UnformatValue(string v);

    object ConvertToNativeType(string v);

    string GetFieldInstanceID(object instanceIndexOrSpecifier);

    FieldDescriptor GetInstanceDescriptor(object instanceIndexOrSpecifier);

    bool IsDateValued();

    MultiInstanceSpecifierType InstanceSpecifierType { get; }

    object InstanceSpecifier { get; }

    bool IsInstance { get; }

    FieldDescriptor ParentDescriptor { get; }

    bool IsVirtualField { get; }

    bool RequiresExclusiveLock { get; }
  }
}
