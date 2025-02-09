// Decompiled with JetBrains decompiler
// Type: Elli.DirectoryServices.Contracts.Services.IDirectoryService
// Assembly: Elli.DirectoryServices.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A20C9E9A-C80F-4187-B071-520874129AC0
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.DirectoryServices.Contracts.dll

using Elli.DirectoryServices.Contracts.Dto;
using System.Collections.Generic;
using System.ServiceModel;

#nullable disable
namespace Elli.DirectoryServices.Contracts.Services
{
  [ServiceContract(Namespace = "http://www.elliemae.com/encompass/directory")]
  public interface IDirectoryService
  {
    [OperationContract]
    DirectoryInstanceDto AddInstance(string instanceName);

    [OperationContract]
    void DeleteInstanceById(int instanceId);

    [OperationContract]
    DirectoryInstanceDto GetInstanceById(int instanceId);

    [OperationContract]
    IEnumerable<DirectoryInstanceDto> GetInstances();

    [OperationContract]
    void UpdateInstance(int instanceId, string instanceName);

    [OperationContract]
    DirectoryCategoryDto AddCategory(string categoryName);

    [OperationContract]
    void DeleteCategoryById(int categoryId);

    [OperationContract]
    DirectoryCategoryDto GetCategoryById(int categoryId);

    [OperationContract]
    IEnumerable<DirectoryCategoryDto> GetCategories();

    [OperationContract]
    void UpdateCategory(int categoryId, string categoryName);

    [OperationContract]
    void DeleteEntryById(int entryId);

    [OperationContract]
    DirectoryEntryDto AddEntry(
      int instanceId,
      int categoryId,
      string entryName,
      DirectoryEntryValueTypeDto valueType,
      object value);

    [OperationContract]
    DirectoryEntryDto GetEntry(string instanceName, string categoryName, string entryName);

    [OperationContract]
    DirectoryEntryDto GetEntryById(int entryId);

    [OperationContract]
    IEnumerable<DirectoryEntryDto> GetEntriesInInstance(string instanceName);

    [OperationContract]
    IEnumerable<DirectoryEntryDto> GetEntries();

    [OperationContract]
    void UpdateEntry(
      int entryId,
      int categoryId,
      string entryName,
      DirectoryEntryValueTypeDto valueType,
      object value);

    [OperationContract]
    string Ping();
  }
}
