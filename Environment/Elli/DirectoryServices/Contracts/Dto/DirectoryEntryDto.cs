// Decompiled with JetBrains decompiler
// Type: Elli.DirectoryServices.Contracts.Dto.DirectoryEntryDto
// Assembly: Environment, Version=17.1.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 54BC7282-2405-4166-B8F8-72E1EF543E16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Environment.dll

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.DirectoryServices.Contracts.Dto
{
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "DirectoryEntryDto", Namespace = "http://schemas.datacontract.org/2004/07/Elli.DirectoryServices.Contracts.Dto")]
  [KnownType(typeof (DirectoryInstanceDto))]
  [KnownType(typeof (DirectoryInstanceDto[]))]
  [KnownType(typeof (DirectoryCategoryDto))]
  [KnownType(typeof (DirectoryCategoryDto[]))]
  [KnownType(typeof (DirectoryEntryValueType))]
  [KnownType(typeof (DirectoryEntryDto[]))]
  public class DirectoryEntryDto : IExtensibleDataObject
  {
    private ExtensionDataObject extensionDataField;
    private int CategoryIdField;
    private string CategoryNameField;
    private int IdField;
    private int InstanceIdField;
    private string InstanceNameField;
    private DateTime LastModifiedDateField;
    private string NameField;
    private object ValueField;
    private DirectoryEntryValueType ValueTypeField;

    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public int CategoryId
    {
      get => this.CategoryIdField;
      set => this.CategoryIdField = value;
    }

    [DataMember]
    public string CategoryName
    {
      get => this.CategoryNameField;
      set => this.CategoryNameField = value;
    }

    [DataMember]
    public int Id
    {
      get => this.IdField;
      set => this.IdField = value;
    }

    [DataMember]
    public int InstanceId
    {
      get => this.InstanceIdField;
      set => this.InstanceIdField = value;
    }

    [DataMember]
    public string InstanceName
    {
      get => this.InstanceNameField;
      set => this.InstanceNameField = value;
    }

    [DataMember]
    public DateTime LastModifiedDate
    {
      get => this.LastModifiedDateField;
      set => this.LastModifiedDateField = value;
    }

    [DataMember]
    public string Name
    {
      get => this.NameField;
      set => this.NameField = value;
    }

    [DataMember]
    public object Value
    {
      get => this.ValueField;
      set => this.ValueField = value;
    }

    [DataMember]
    public DirectoryEntryValueType ValueType
    {
      get => this.ValueTypeField;
      set => this.ValueTypeField = value;
    }
  }
}
