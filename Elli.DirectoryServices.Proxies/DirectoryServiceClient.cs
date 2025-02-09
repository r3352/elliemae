// Decompiled with JetBrains decompiler
// Type: Elli.DirectoryServices.Proxies.DirectoryServiceClient
// Assembly: Elli.DirectoryServices.Proxies, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6212B77A-36B3-4956-81C4-E4F08EF9D4F9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.DirectoryServices.Proxies.dll

using Elli.DirectoryServices.Contracts.Dto;
using Elli.DirectoryServices.Contracts.Services;
using System.Collections.Generic;
using System.ServiceModel;

#nullable disable
namespace Elli.DirectoryServices.Proxies
{
  public class DirectoryServiceClient : IDirectoryService
  {
    private const string DirectoryServiceEndpointName = "BasicHttpBinding_IDirectoryService";
    private readonly IDirectoryService _directoryServiceChannel;

    public DirectoryServiceClient()
    {
      this._directoryServiceChannel = new ChannelFactory<IDirectoryService>("BasicHttpBinding_IDirectoryService").CreateChannel();
    }

    public DirectoryServiceClient(string endpointAddress)
    {
      this._directoryServiceChannel = new ChannelFactory<IDirectoryService>("BasicHttpBinding_IDirectoryService").CreateChannel(new EndpointAddress(endpointAddress));
    }

    public DirectoryInstanceDto AddInstance(string instanceName)
    {
      return this._directoryServiceChannel.AddInstance(instanceName);
    }

    public void DeleteInstanceById(int instanceId)
    {
      this._directoryServiceChannel.DeleteInstanceById(instanceId);
    }

    public DirectoryInstanceDto GetInstanceById(int instanceId)
    {
      return this._directoryServiceChannel.GetInstanceById(instanceId);
    }

    public IEnumerable<DirectoryInstanceDto> GetInstances()
    {
      return this._directoryServiceChannel.GetInstances();
    }

    public void UpdateInstance(int instanceId, string instanceName)
    {
      this._directoryServiceChannel.UpdateInstance(instanceId, instanceName);
    }

    public DirectoryCategoryDto AddCategory(string categoryName)
    {
      return this._directoryServiceChannel.AddCategory(categoryName);
    }

    public void DeleteCategoryById(int categoryId)
    {
      this._directoryServiceChannel.DeleteCategoryById(categoryId);
    }

    public DirectoryCategoryDto GetCategoryById(int categoryId)
    {
      return this._directoryServiceChannel.GetCategoryById(categoryId);
    }

    public IEnumerable<DirectoryCategoryDto> GetCategories()
    {
      return this._directoryServiceChannel.GetCategories();
    }

    public void UpdateCategory(int categoryId, string categoryName)
    {
      this._directoryServiceChannel.UpdateCategory(categoryId, categoryName);
    }

    public DirectoryEntryDto AddEntry(
      int instanceId,
      int categoryId,
      string entryName,
      DirectoryEntryValueTypeDto valueType,
      object value)
    {
      return this._directoryServiceChannel.AddEntry(instanceId, categoryId, entryName, valueType, value);
    }

    public void DeleteEntryById(int entryId)
    {
      this._directoryServiceChannel.DeleteEntryById(entryId);
    }

    public DirectoryEntryDto GetEntry(string instanceName, string categoryName, string entryName)
    {
      return this._directoryServiceChannel.GetEntry(instanceName, categoryName, entryName);
    }

    public DirectoryEntryDto GetEntryById(int entryId)
    {
      return this._directoryServiceChannel.GetEntryById(entryId);
    }

    public IEnumerable<DirectoryEntryDto> GetEntriesInInstance(string instanceName)
    {
      return this._directoryServiceChannel.GetEntriesInInstance(instanceName);
    }

    public IEnumerable<DirectoryEntryDto> GetEntries()
    {
      return this._directoryServiceChannel.GetEntries();
    }

    public void UpdateEntry(
      int entryId,
      int categoryId,
      string entryName,
      DirectoryEntryValueTypeDto valueType,
      object value)
    {
      this._directoryServiceChannel.UpdateEntry(entryId, categoryId, entryName, valueType, value);
    }

    public string Ping() => this._directoryServiceChannel.Ping();
  }
}
