// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.FilePermission
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System.Collections;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class FilePermission
  {
    public static bool ExistsSecurityGroup(
      string directoryName,
      WellKnownSidType wksid,
      bool errorDefault)
    {
      bool flag = false;
      try
      {
        DirectorySecurity accessControl = new DirectoryInfo(directoryName).GetAccessControl();
        SecurityIdentifier securityIdentifier = new SecurityIdentifier(wksid, (SecurityIdentifier) null);
        System.Type targetType = typeof (SecurityIdentifier);
        foreach (AuthorizationRule accessRule in (ReadOnlyCollectionBase) accessControl.GetAccessRules(true, true, targetType))
        {
          if (accessRule.IdentityReference.Value == securityIdentifier.Value)
          {
            flag = true;
            break;
          }
        }
      }
      catch
      {
        flag = errorDefault;
      }
      return flag;
    }

    public static void AddSecurityGroup(string directoryName, WellKnownSidType wksid)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
      DirectorySecurity accessControl = directoryInfo.GetAccessControl();
      SecurityIdentifier identity = new SecurityIdentifier(wksid, (SecurityIdentifier) null);
      accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference) identity, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
      directoryInfo.SetAccessControl(accessControl);
    }

    public static void RemoveSecurityGroup(string directoryName, WellKnownSidType wksid)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
      DirectorySecurity accessControl = directoryInfo.GetAccessControl();
      SecurityIdentifier identity = new SecurityIdentifier(wksid, (SecurityIdentifier) null);
      accessControl.PurgeAccessRules((IdentityReference) identity);
      directoryInfo.SetAccessControl(accessControl);
    }
  }
}
