// Decompiled with JetBrains decompiler
// Type: DirectoryServiceClient
// Assembly: Environment, Version=17.1.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 54BC7282-2405-4166-B8F8-72E1EF543E16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Environment.dll

using Elli.DirectoryServices.Contracts.Dto;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

#nullable disable
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class DirectoryServiceClient : ClientBase<IDirectoryService>, IDirectoryService
{
  public DirectoryServiceClient()
  {
  }

  public DirectoryServiceClient(string endpointConfigurationName)
    : base(endpointConfigurationName)
  {
  }

  public DirectoryServiceClient(string endpointConfigurationName, string remoteAddress)
    : base(endpointConfigurationName, remoteAddress)
  {
  }

  public DirectoryServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
    : base(endpointConfigurationName, remoteAddress)
  {
  }

  public DirectoryServiceClient(Binding binding, EndpointAddress remoteAddress)
    : base(binding, remoteAddress)
  {
  }

  public DirectoryInstanceDto AddInstance(string instanceName)
  {
    return this.Channel.AddInstance(instanceName);
  }

  public Task<DirectoryInstanceDto> AddInstanceAsync(string instanceName)
  {
    return this.Channel.AddInstanceAsync(instanceName);
  }

  public void DeleteInstanceById(int instanceId) => this.Channel.DeleteInstanceById(instanceId);

  public Task DeleteInstanceByIdAsync(int instanceId)
  {
    return this.Channel.DeleteInstanceByIdAsync(instanceId);
  }

  public DirectoryInstanceDto GetInstanceById(int instanceId)
  {
    return this.Channel.GetInstanceById(instanceId);
  }

  public Task<DirectoryInstanceDto> GetInstanceByIdAsync(int instanceId)
  {
    return this.Channel.GetInstanceByIdAsync(instanceId);
  }

  public DirectoryInstanceDto[] GetInstances() => this.Channel.GetInstances();

  public Task<DirectoryInstanceDto[]> GetInstancesAsync() => this.Channel.GetInstancesAsync();

  public void UpdateInstance(int instanceId, string instanceName)
  {
    this.Channel.UpdateInstance(instanceId, instanceName);
  }

  public Task UpdateInstanceAsync(int instanceId, string instanceName)
  {
    return this.Channel.UpdateInstanceAsync(instanceId, instanceName);
  }

  public DirectoryCategoryDto AddCategory(string categoryName)
  {
    return this.Channel.AddCategory(categoryName);
  }

  public Task<DirectoryCategoryDto> AddCategoryAsync(string categoryName)
  {
    return this.Channel.AddCategoryAsync(categoryName);
  }

  public void DeleteCategoryById(int categoryId) => this.Channel.DeleteCategoryById(categoryId);

  public Task DeleteCategoryByIdAsync(int categoryId)
  {
    return this.Channel.DeleteCategoryByIdAsync(categoryId);
  }

  public DirectoryCategoryDto GetCategoryById(int categoryId)
  {
    return this.Channel.GetCategoryById(categoryId);
  }

  public Task<DirectoryCategoryDto> GetCategoryByIdAsync(int categoryId)
  {
    return this.Channel.GetCategoryByIdAsync(categoryId);
  }

  public DirectoryCategoryDto[] GetCategories() => this.Channel.GetCategories();

  public Task<DirectoryCategoryDto[]> GetCategoriesAsync() => this.Channel.GetCategoriesAsync();

  public void UpdateCategory(int categoryId, string categoryName)
  {
    this.Channel.UpdateCategory(categoryId, categoryName);
  }

  public Task UpdateCategoryAsync(int categoryId, string categoryName)
  {
    return this.Channel.UpdateCategoryAsync(categoryId, categoryName);
  }

  public void DeleteEntryById(int entryId) => this.Channel.DeleteEntryById(entryId);

  public Task DeleteEntryByIdAsync(int entryId) => this.Channel.DeleteEntryByIdAsync(entryId);

  public DirectoryEntryDto AddEntry(
    int instanceId,
    int categoryId,
    string entryName,
    DirectoryEntryValueType valueType,
    object value)
  {
    return this.Channel.AddEntry(instanceId, categoryId, entryName, valueType, value);
  }

  public Task<DirectoryEntryDto> AddEntryAsync(
    int instanceId,
    int categoryId,
    string entryName,
    DirectoryEntryValueType valueType,
    object value)
  {
    return this.Channel.AddEntryAsync(instanceId, categoryId, entryName, valueType, value);
  }

  public DirectoryEntryDto GetEntry(string instanceName, string categoryName, string entryName)
  {
    return this.Channel.GetEntry(instanceName, categoryName, entryName);
  }

  public Task<DirectoryEntryDto> GetEntryAsync(
    string instanceName,
    string categoryName,
    string entryName)
  {
    return this.Channel.GetEntryAsync(instanceName, categoryName, entryName);
  }

  public DirectoryEntryDto GetEntryById(int entryId) => this.Channel.GetEntryById(entryId);

  public Task<DirectoryEntryDto> GetEntryByIdAsync(int entryId)
  {
    return this.Channel.GetEntryByIdAsync(entryId);
  }

  public DirectoryEntryDto[] GetEntriesInInstance(string instanceName)
  {
    return this.Channel.GetEntriesInInstance(instanceName);
  }

  public Task<DirectoryEntryDto[]> GetEntriesInInstanceAsync(string instanceName)
  {
    return this.Channel.GetEntriesInInstanceAsync(instanceName);
  }

  public DirectoryEntryDto[] GetEntriesInInstanceAndCatagory(
    string instanceName,
    string categoryName)
  {
    return this.Channel.GetEntriesInInstanceAndCatagory(instanceName, categoryName);
  }

  public Task<DirectoryEntryDto[]> GetEntriesInInstanceAndCatagoryAsync(
    string instanceName,
    string categoryName)
  {
    return this.Channel.GetEntriesInInstanceAndCatagoryAsync(instanceName, categoryName);
  }

  public DirectoryEntryDto[] GetEntries() => this.Channel.GetEntries();

  public Task<DirectoryEntryDto[]> GetEntriesAsync() => this.Channel.GetEntriesAsync();

  public void UpdateEntry(
    int entryId,
    int categoryId,
    string entryName,
    DirectoryEntryValueType valueType,
    object value)
  {
    this.Channel.UpdateEntry(entryId, categoryId, entryName, valueType, value);
  }

  public Task UpdateEntryAsync(
    int entryId,
    int categoryId,
    string entryName,
    DirectoryEntryValueType valueType,
    object value)
  {
    return this.Channel.UpdateEntryAsync(entryId, categoryId, entryName, valueType, value);
  }

  public string Ping() => this.Channel.Ping();

  public Task<string> PingAsync() => this.Channel.PingAsync();
}
