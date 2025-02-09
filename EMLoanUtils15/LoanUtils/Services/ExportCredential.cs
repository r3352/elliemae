// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Services.ExportCredential
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Services
{
  public class ExportCredential
  {
    private static string sw = Tracing.SwOutsideLoan;
    private List<ePassCredential> emnUsersProvider;
    private ProductPricingSetting currentActiveSetting;
    private SessionObjects sessionObject;
    private string accessToken = string.Empty;
    private string category;

    private ReauthenticateOnUnauthorised reauthenticateOnUnauthorised { get; set; }

    public ExportCredential(
      SessionObjects sessionObject,
      ProductPricingSetting currentActiveSetting,
      string accessToken)
    {
      this.accessToken = accessToken;
      this.category = "PRODUCTPRICING";
      this.sessionObject = sessionObject;
      this.reauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(this.sessionObject.StartupInfo.ServerInstanceName, this.sessionObject.StartupInfo.SessionID, this.sessionObject.StartupInfo.OAPIGatewayBaseUri, new RetrySettings());
      List<ePassCredential> source = sessionObject.ConfigurationManager.GetePassCredential("Product and Pricing");
      string title = "";
      if (currentActiveSetting != null && currentActiveSetting.IsEPPS)
        title = "Encompass Product and Pricing Service";
      this.emnUsersProvider = source.Where<ePassCredential>((Func<ePassCredential, bool>) (emn => emn.Title == title)).ToList<ePassCredential>();
      this.currentActiveSetting = currentActiveSetting;
    }

    public bool IsExportEnabled
    {
      get
      {
        return this.emnUsersProvider != null && this.emnUsersProvider.Count > 0 && this.currentActiveSetting != null && this.currentActiveSetting.IsEPPS && this.currentActiveSetting.VendorPlatform == VendorPlatform.EPC2 && !this.currentActiveSetting.IsExportUserFinished;
      }
    }

    public int ExportIndex { get; set; }

    public List<ServiceSetupResult> ServiceSetups { get; set; }

    public int TotlaCount
    {
      get
      {
        return this.emnUsersProvider.OrderBy<ePassCredential, string>((Func<ePassCredential, string>) (x => x.UserId)).GroupBy<ePassCredential, string>((Func<ePassCredential, string>) (x => x.UserId)).ToList<IGrouping<string, ePassCredential>>().Count;
      }
    }

    public string ServiceSetupName
    {
      get => this.currentActiveSetting.PartnerName + " Exported Service Setup";
    }

    public ServiceSetupResult ActiveServiceSetupForProvider
    {
      get
      {
        if (this.ServiceSetups == null)
          this.ServiceSetups = Task.Run<List<ServiceSetupResult>>((Func<Task<List<ServiceSetupResult>>>) (async () => await Epc2ServiceClient.GetServiceSetupAsync(this.sessionObject, this.accessToken, this.category, ""))).Result;
        return this.ServiceSetups.Where<ServiceSetupResult>((Func<ServiceSetupResult, bool>) (s => s.IsActive && s.ProviderId == this.currentActiveSetting.ProviderID)).FirstOrDefault<ServiceSetupResult>();
      }
    }

    public bool HasDuplicateServiceSetup
    {
      get
      {
        if (this.ServiceSetups == null)
          this.ServiceSetups = Task.Run<List<ServiceSetupResult>>((Func<Task<List<ServiceSetupResult>>>) (async () => await Epc2ServiceClient.GetServiceSetupAsync(this.sessionObject, this.accessToken, this.category, ""))).Result;
        return this.ServiceSetups.Any<ServiceSetupResult>((Func<ServiceSetupResult, bool>) (s => s.Name == this.ServiceSetupName));
      }
    }

    public async Task<ServiceSetupResult> ExportUsers(
      Epc2Provider provider,
      ServiceSetupResult activeServiceSetup,
      bool isNewServicerSetupActive)
    {
      List<Task> tasks = new List<Task>();
      Task<ServiceSetupResult> serviceSetupTask = (Task<ServiceSetupResult>) null;
      this.ExportIndex = 0;
      this.reauthenticateOnUnauthorised.Execute((Action<EllieMae.EMLite.ClientServer.Authentication.AccessToken>) (accessToken =>
      {
        this.ExportServiceCredential(tasks, accessToken.TypeAndToken, this.emnUsersProvider, provider);
        serviceSetupTask = this.ExportServiceSetup(tasks, accessToken.TypeAndToken, this.emnUsersProvider, provider, activeServiceSetup, isNewServicerSetupActive);
      }));
      await Task.WhenAll((IEnumerable<Task>) tasks);
      this.sessionObject.ConfigurationManager.ProductPricingExportUser(provider.Id);
      return serviceSetupTask.Result;
    }

    private Task<ServiceSetupResult> ExportServiceSetup(
      List<Task> tasks,
      string accessToken,
      List<ePassCredential> emnUsers,
      Epc2Provider provider,
      ServiceSetupResult activeServiceSetup,
      bool isNewServicerSetupActive)
    {
      if (activeServiceSetup != null & isNewServicerSetupActive)
      {
        Task task = (Task) Epc2ServiceClient.UpdateServiceSetupAsync(this.sessionObject, accessToken, new ServiceSetupResult()
        {
          Id = activeServiceSetup.Id,
          IsActive = false
        });
        tasks.Add(task);
      }
      Task<ServiceSetupResult> serviceSetupAsync = Epc2ServiceClient.CreateServiceSetupAsync(this.sessionObject, accessToken, this.PopulateServiceSetup(emnUsers, provider, isNewServicerSetupActive));
      tasks.Add((Task) serviceSetupAsync);
      return serviceSetupAsync;
    }

    private async Task<bool> ExportServiceCredential(
      List<Task> tasks,
      string accessToken,
      List<ePassCredential> emnUsers,
      Epc2Provider provider)
    {
      ServiceCredentialResponse credentialResponse = Task.Run<List<ServiceCredentialResponse>>((Func<Task<List<ServiceCredentialResponse>>>) (async () => await Epc2ServiceClient.GetServiceCredentialAsync(this.sessionObject, accessToken, this.category, provider.Id))).Result.FirstOrDefault<ServiceCredentialResponse>((Func<ServiceCredentialResponse, bool>) (x => !string.IsNullOrWhiteSpace(x.Id)));
      string id = credentialResponse?.Id;
      if (string.IsNullOrWhiteSpace(id))
      {
        ServiceCredentialRequest request = new ServiceCredentialRequest()
        {
          Category = this.category,
          Description = provider.ListingName + " Exported Credentials",
          Name = provider.ListingName + " Exported Credentials",
          PartnerId = provider.PartnerId,
          ProductName = provider.ProductName,
          ProviderId = provider.Id
        };
        id = Task.Run<ServiceCredentialResponse>((Func<Task<ServiceCredentialResponse>>) (async () => await Epc2ServiceClient.CreateServiceCredentialAsync(this.sessionObject, accessToken, request))).Result.Id;
      }
      bool flag = false;
      if (true)
        flag = this.CreateOrUpdateUserCredentials(accessToken, id, provider, (IEnumerable<ePassCredential>) emnUsers, credentialResponse?.UserCredentials);
      else
        tasks.Add((Task) this.CreateOrUpdateUserCredentialsAsync(accessToken, id, provider, emnUsers, credentialResponse?.UserCredentials));
      await Task.WhenAll((IEnumerable<Task>) tasks);
      return true;
    }

    private async Task<List<UserCredentialResponse>> CreateOrUpdateUserCredentialsAsync(
      string accessToken,
      string credentialId,
      Epc2Provider provider,
      List<ePassCredential> emnUsers,
      List<UserCredential> epciUserCredentials)
    {
      List<IGrouping<string, ePassCredential>> list1 = emnUsers.OrderBy<ePassCredential, string>((Func<ePassCredential, string>) (x => x.UserId)).GroupBy<ePassCredential, string>((Func<ePassCredential, string>) (x => x.UserId)).ToList<IGrouping<string, ePassCredential>>();
      List<Task<UserCredentialResponse>> taskList = new List<Task<UserCredentialResponse>>();
      foreach (IGrouping<string, ePassCredential> grouping in list1)
      {
        IGrouping<string, ePassCredential> user = grouping;
        if (!string.IsNullOrWhiteSpace(user.Key))
        {
          List<ePassCredential> list2 = user.ToList<ePassCredential>();
          AuthorizedUser authorizedUser = new AuthorizedUser()
          {
            EntityId = user.Key,
            EntityType = "User",
            EntityName = list2[0].Name,
            EntityUri = "/users/" + user.Key
          };
          IDictionary<string, object> dynamicCred = (IDictionary<string, object>) new ExpandoObject();
          foreach (ePassCredential ePassCredential in list2.Where<ePassCredential>((Func<ePassCredential, bool>) (row => !string.IsNullOrWhiteSpace(row.Attribute))).Where<ePassCredential>((Func<ePassCredential, bool>) (row => !dynamicCred.ContainsKey(row.Attribute))))
          {
            string key = ePassCredential.Attribute.Equals("LoginName", StringComparison.CurrentCultureIgnoreCase) ? "username" : ePassCredential.Attribute;
            dynamicCred.Add(key, (object) ePassCredential.Value);
          }
          List<UserCredential> source = epciUserCredentials;
          UserCredential userCredential1 = source != null ? source.FirstOrDefault<UserCredential>((Func<UserCredential, bool>) (x => x.AuthorizedUsers.Any<AuthorizedUser>((Func<AuthorizedUser, bool>) (y => y.EntityId.Equals(user.Key, StringComparison.OrdinalIgnoreCase))))) : (UserCredential) null;
          if (userCredential1 != null && !string.IsNullOrWhiteSpace(userCredential1.Id))
          {
            for (int index = 0; index < userCredential1.AuthorizedUsers.Count; ++index)
            {
              if (userCredential1.AuthorizedUsers[index].EntityId.Equals(user.Key, StringComparison.OrdinalIgnoreCase))
              {
                userCredential1.AuthorizedUsers[index] = authorizedUser;
                break;
              }
            }
            UserCredentialRequest userCredential2 = new UserCredentialRequest()
            {
              Name = userCredential1.Name,
              Description = userCredential1.Description,
              AuthorizedUsers = userCredential1.AuthorizedUsers,
              Credential = (ExpandoObject) dynamicCred
            };
            taskList.Add(Epc2ServiceClient.UpdateUserCredentialAsync(this.sessionObject, accessToken, userCredential2, credentialId, userCredential1.Id));
          }
          else
          {
            UserCredentialRequest userCredential3 = new UserCredentialRequest()
            {
              Name = provider.ListingName + " User Credentials " + user.Key,
              Description = provider.ListingName + " User Credentials " + user.Key,
              AuthorizedUsers = new List<AuthorizedUser>()
              {
                authorizedUser
              },
              Credential = (ExpandoObject) dynamicCred,
              PartnerId = provider.PartnerId,
              ProductName = provider.ProductName,
              ProviderId = provider.Id
            };
            taskList.Add(Epc2ServiceClient.CreateUserCredentialAsync(this.sessionObject, accessToken, userCredential3, credentialId));
          }
        }
      }
      return ((IEnumerable<UserCredentialResponse>) await Task.WhenAll<UserCredentialResponse>((IEnumerable<Task<UserCredentialResponse>>) taskList)).ToList<UserCredentialResponse>();
    }

    private bool CreateOrUpdateUserCredentials(
      string accessToken,
      string credentialId,
      Epc2Provider product,
      IEnumerable<ePassCredential> emnUsers,
      List<UserCredential> epciUserCredentials)
    {
      foreach (IGrouping<string, ePassCredential> grouping in emnUsers.OrderBy<ePassCredential, string>((Func<ePassCredential, string>) (x => x.UserId)).GroupBy<ePassCredential, string>((Func<ePassCredential, string>) (x => x.UserId)).ToList<IGrouping<string, ePassCredential>>())
      {
        IGrouping<string, ePassCredential> user = grouping;
        if (!string.IsNullOrWhiteSpace(user.Key))
        {
          List<ePassCredential> list = user.ToList<ePassCredential>();
          AuthorizedUser authorizedUser = new AuthorizedUser()
          {
            EntityId = user.Key,
            EntityType = "User",
            EntityName = list[0].Name,
            EntityUri = "/users/" + user.Key
          };
          IDictionary<string, object> dynamicCred = (IDictionary<string, object>) new ExpandoObject();
          foreach (ePassCredential ePassCredential in list.Where<ePassCredential>((Func<ePassCredential, bool>) (row => !string.IsNullOrWhiteSpace(row.Attribute))).Where<ePassCredential>((Func<ePassCredential, bool>) (row => !dynamicCred.ContainsKey(row.Attribute))))
          {
            string key = ePassCredential.Attribute.Equals("LoginName", StringComparison.CurrentCultureIgnoreCase) ? "username" : ePassCredential.Attribute;
            dynamicCred.Add(key, (object) ePassCredential.Value);
          }
          UserCredential epciUser = epciUserCredentials != null ? epciUserCredentials.FirstOrDefault<UserCredential>((Func<UserCredential, bool>) (x => x.AuthorizedUsers.Any<AuthorizedUser>((Func<AuthorizedUser, bool>) (y => y.EntityId.Equals(user.Key, StringComparison.OrdinalIgnoreCase))))) : (UserCredential) null;
          if (epciUser != null && !string.IsNullOrWhiteSpace(epciUser.Id))
          {
            for (int index = 0; index < epciUser.AuthorizedUsers.Count; ++index)
            {
              if (epciUser.AuthorizedUsers[index].EntityId.Equals(user.Key, StringComparison.OrdinalIgnoreCase))
              {
                epciUser.AuthorizedUsers[index] = authorizedUser;
                break;
              }
            }
            UserCredentialRequest request = new UserCredentialRequest()
            {
              Name = epciUser.Name,
              Description = epciUser.Description,
              AuthorizedUsers = epciUser.AuthorizedUsers,
              Credential = (ExpandoObject) dynamicCred
            };
            UserCredentialResponse result = Task.Run<UserCredentialResponse>((Func<Task<UserCredentialResponse>>) (async () => await Epc2ServiceClient.UpdateUserCredentialAsync(this.sessionObject, accessToken, request, credentialId, epciUser.Id))).Result;
            ++this.ExportIndex;
          }
          else
          {
            UserCredentialRequest request = new UserCredentialRequest()
            {
              Name = product.ListingName + " User Credentials " + user.Key,
              Description = product.ListingName + " User Credentials " + user.Key,
              AuthorizedUsers = new List<AuthorizedUser>()
              {
                authorizedUser
              },
              Credential = (ExpandoObject) dynamicCred,
              PartnerId = product.PartnerId,
              ProductName = product.ProductName,
              ProviderId = product.Id
            };
            UserCredentialResponse result = Task.Run<UserCredentialResponse>((Func<Task<UserCredentialResponse>>) (async () => await Epc2ServiceClient.CreateUserCredentialAsync(this.sessionObject, accessToken, request, credentialId))).Result;
            ++this.ExportIndex;
          }
        }
      }
      return true;
    }

    private ServiceSetupResult PopulateServiceSetup(
      List<ePassCredential> emnUsers,
      Epc2Provider provider,
      bool isNewServicerSetupActive)
    {
      return new ServiceSetupResult()
      {
        PartnerId = provider.PartnerId,
        ProductName = provider.ProductName,
        Category = "PRODUCTPRICING",
        Name = this.ServiceSetupName,
        Description = this.ServiceSetupName,
        IsActive = isNewServicerSetupActive,
        OrderType = OrderTypeEnum.MANUAL,
        Scope = ScopeEnum.Loan,
        AuthorizedUsers = this.GetAuthorizedUsers((IEnumerable<ePassCredential>) emnUsers).Values.ToList<AuthorizedUser>(),
        ProviderId = provider.Id
      };
    }

    private Dictionary<string, AuthorizedUser> GetAuthorizedUsers(
      IEnumerable<ePassCredential> userCredentials)
    {
      Dictionary<string, AuthorizedUser> authorizedUsers = new Dictionary<string, AuthorizedUser>();
      foreach (IGrouping<string, ePassCredential> source in userCredentials.OrderBy<ePassCredential, string>((Func<ePassCredential, string>) (x => x.UserId)).GroupBy<ePassCredential, string>((Func<ePassCredential, string>) (x => x.UserId)).ToList<IGrouping<string, ePassCredential>>())
      {
        if (!authorizedUsers.ContainsKey(source.Key))
        {
          ePassCredential ePassCredential = source.ToList<ePassCredential>().FirstOrDefault<ePassCredential>((Func<ePassCredential, bool>) (x => !string.IsNullOrWhiteSpace(x.UserId)));
          if (ePassCredential != null)
          {
            AuthorizedUser authorizedUser = new AuthorizedUser()
            {
              EntityId = ePassCredential.UserId,
              EntityType = "User",
              EntityName = ePassCredential.Name,
              EntityUri = "/users/" + ePassCredential.UserId
            };
            authorizedUsers.Add(source.Key, authorizedUser);
          }
        }
      }
      return authorizedUsers;
    }
  }
}
