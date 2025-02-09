// Decompiled with JetBrains decompiler
// Type: Elli.DirectoryServices.Contracts.Dto.DirectoryEntryValueTypeDto
// Assembly: Elli.DirectoryServices.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A20C9E9A-C80F-4187-B071-520874129AC0
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.DirectoryServices.Contracts.dll

using System.Runtime.Serialization;

#nullable disable
namespace Elli.DirectoryServices.Contracts.Dto
{
  [DataContract(Name = "DirectoryEntryValueType")]
  public enum DirectoryEntryValueTypeDto
  {
    [EnumMember] String,
    [EnumMember] Int,
    [EnumMember] DateTime,
  }
}
