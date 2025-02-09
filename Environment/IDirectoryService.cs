// Decompiled with JetBrains decompiler
// Type: IDirectoryService
// Assembly: Environment, Version=17.1.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 54BC7282-2405-4166-B8F8-72E1EF543E16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Environment.dll

using Elli.DirectoryServices.Contracts.Dto;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

#nullable disable
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://www.elliemae.com/encompass/directory", ConfigurationName = "IDirectoryService")]
public interface IDirectoryService
{
  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/AddInstance", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/AddInstanceResponse")]
  DirectoryInstanceDto AddInstance(string instanceName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/AddInstance", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/AddInstanceResponse")]
  Task<DirectoryInstanceDto> AddInstanceAsync(string instanceName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/DeleteInstanceById", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/DeleteInstanceByIdResponse")]
  void DeleteInstanceById(int instanceId);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/DeleteInstanceById", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/DeleteInstanceByIdResponse")]
  Task DeleteInstanceByIdAsync(int instanceId);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetInstanceById", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetInstanceByIdResponse")]
  DirectoryInstanceDto GetInstanceById(int instanceId);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetInstanceById", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetInstanceByIdResponse")]
  Task<DirectoryInstanceDto> GetInstanceByIdAsync(int instanceId);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetInstances", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetInstancesResponse")]
  DirectoryInstanceDto[] GetInstances();

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetInstances", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetInstancesResponse")]
  Task<DirectoryInstanceDto[]> GetInstancesAsync();

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/UpdateInstance", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/UpdateInstanceResponse")]
  void UpdateInstance(int instanceId, string instanceName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/UpdateInstance", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/UpdateInstanceResponse")]
  Task UpdateInstanceAsync(int instanceId, string instanceName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/AddCategory", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/AddCategoryResponse")]
  DirectoryCategoryDto AddCategory(string categoryName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/AddCategory", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/AddCategoryResponse")]
  Task<DirectoryCategoryDto> AddCategoryAsync(string categoryName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/DeleteCategoryById", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/DeleteCategoryByIdResponse")]
  void DeleteCategoryById(int categoryId);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/DeleteCategoryById", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/DeleteCategoryByIdResponse")]
  Task DeleteCategoryByIdAsync(int categoryId);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetCategoryById", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetCategoryByIdResponse")]
  DirectoryCategoryDto GetCategoryById(int categoryId);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetCategoryById", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetCategoryByIdResponse")]
  Task<DirectoryCategoryDto> GetCategoryByIdAsync(int categoryId);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetCategories", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetCategoriesResponse")]
  DirectoryCategoryDto[] GetCategories();

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetCategories", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetCategoriesResponse")]
  Task<DirectoryCategoryDto[]> GetCategoriesAsync();

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/UpdateCategory", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/UpdateCategoryResponse")]
  void UpdateCategory(int categoryId, string categoryName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/UpdateCategory", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/UpdateCategoryResponse")]
  Task UpdateCategoryAsync(int categoryId, string categoryName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/DeleteEntryById", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/DeleteEntryByIdResponse")]
  void DeleteEntryById(int entryId);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/DeleteEntryById", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/DeleteEntryByIdResponse")]
  Task DeleteEntryByIdAsync(int entryId);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/AddEntry", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/AddEntryResponse")]
  [ServiceKnownType(typeof (DirectoryInstanceDto))]
  [ServiceKnownType(typeof (DirectoryInstanceDto[]))]
  [ServiceKnownType(typeof (DirectoryCategoryDto))]
  [ServiceKnownType(typeof (DirectoryCategoryDto[]))]
  [ServiceKnownType(typeof (DirectoryEntryValueType))]
  [ServiceKnownType(typeof (DirectoryEntryDto))]
  [ServiceKnownType(typeof (DirectoryEntryDto[]))]
  DirectoryEntryDto AddEntry(
    int instanceId,
    int categoryId,
    string entryName,
    DirectoryEntryValueType valueType,
    object value);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/AddEntry", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/AddEntryResponse")]
  Task<DirectoryEntryDto> AddEntryAsync(
    int instanceId,
    int categoryId,
    string entryName,
    DirectoryEntryValueType valueType,
    object value);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntry", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntryResponse")]
  DirectoryEntryDto GetEntry(string instanceName, string categoryName, string entryName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntry", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntryResponse")]
  Task<DirectoryEntryDto> GetEntryAsync(string instanceName, string categoryName, string entryName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntryById", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntryByIdResponse")]
  DirectoryEntryDto GetEntryById(int entryId);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntryById", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntryByIdResponse")]
  Task<DirectoryEntryDto> GetEntryByIdAsync(int entryId);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntriesInInstance", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntriesInInstanceResponse")]
  DirectoryEntryDto[] GetEntriesInInstance(string instanceName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntriesInInstance", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntriesInInstanceResponse")]
  Task<DirectoryEntryDto[]> GetEntriesInInstanceAsync(string instanceName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntriesInInstanceAndCatagory", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntriesInInstanceAndCatagoryResponse")]
  DirectoryEntryDto[] GetEntriesInInstanceAndCatagory(string instanceName, string categoryName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntriesInInstanceAndCatagory", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntriesInInstanceAndCatagoryResponse")]
  Task<DirectoryEntryDto[]> GetEntriesInInstanceAndCatagoryAsync(
    string instanceName,
    string categoryName);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntries", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntriesResponse")]
  DirectoryEntryDto[] GetEntries();

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntries", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/GetEntriesResponse")]
  Task<DirectoryEntryDto[]> GetEntriesAsync();

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/UpdateEntry", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/UpdateEntryResponse")]
  [ServiceKnownType(typeof (DirectoryInstanceDto))]
  [ServiceKnownType(typeof (DirectoryInstanceDto[]))]
  [ServiceKnownType(typeof (DirectoryCategoryDto))]
  [ServiceKnownType(typeof (DirectoryCategoryDto[]))]
  [ServiceKnownType(typeof (DirectoryEntryValueType))]
  [ServiceKnownType(typeof (DirectoryEntryDto))]
  [ServiceKnownType(typeof (DirectoryEntryDto[]))]
  void UpdateEntry(
    int entryId,
    int categoryId,
    string entryName,
    DirectoryEntryValueType valueType,
    object value);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/UpdateEntry", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/UpdateEntryResponse")]
  Task UpdateEntryAsync(
    int entryId,
    int categoryId,
    string entryName,
    DirectoryEntryValueType valueType,
    object value);

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/Ping", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/PingResponse")]
  string Ping();

  [OperationContract(Action = "http://www.elliemae.com/encompass/directory/IDirectoryService/Ping", ReplyAction = "http://www.elliemae.com/encompass/directory/IDirectoryService/PingResponse")]
  Task<string> PingAsync();
}
