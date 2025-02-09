// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILockRequestFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("70486CFE-A950-4dc6-A5D1-81572EA93553")]
  public interface ILockRequestFields
  {
    LockRequestField this[string fieldId] { get; }

    FieldDescriptors Descriptors { get; }

    void Recalculate();

    void CommitChanges();
  }
}
