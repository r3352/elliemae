// Decompiled with JetBrains decompiler
// Type: Elli.DirectoryServices.Contracts.Dto.DirectoryEntryValueType
// Assembly: Environment, Version=17.1.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 54BC7282-2405-4166-B8F8-72E1EF543E16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Environment.dll

using System.CodeDom.Compiler;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.DirectoryServices.Contracts.Dto
{
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "DirectoryEntryValueType", Namespace = "http://schemas.datacontract.org/2004/07/Elli.DirectoryServices.Contracts.Dto")]
  public enum DirectoryEntryValueType
  {
    [EnumMember] String,
    [EnumMember] Int,
    [EnumMember] DateTime,
  }
}
