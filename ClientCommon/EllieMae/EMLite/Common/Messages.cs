// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Messages
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class Messages
  {
    private static Hashtable messages = new Hashtable();

    static Messages()
    {
      Messages.messages.Add((object) "AlreadySentToProcessing", (object) "You cannot download the loan '{0}' because it has already been sent to processing.");
      Messages.messages.Add((object) "DeleteLoanConfirmation", (object) "Are you sure you want to delete loan '{0}'?");
      Messages.messages.Add((object) "MoveLoanConfirmation", (object) "Are you sure you want to move loan '{0}'?");
      Messages.messages.Add((object) "PermanentlyDeleteLoanConfirmation", (object) "Are you sure you want to permanently delete loan '{0}'?");
      Messages.messages.Add((object) "LoanNotExist", (object) "The selected loan does not exist. Please rebuild your pipeline.");
      Messages.messages.Add((object) "ReadOnlyLoan", (object) "The loan is read only. Do you still want to open it in read-only mode?");
      Messages.messages.Add((object) "JedHelpLoadingError", (object) "An error occurred in loading help. Please contact your system administrator.");
      Messages.messages.Add((object) "VersionUpdateInProgess", (object) "Currently, a version update is in progress.");
      Messages.messages.Add((object) "VersionUpdateInitiated", (object) "Version update is successfully initiated.");
      Messages.messages.Add((object) "ClientVersionUpdate", (object) "A newer version of Encompass is available. Encompass will now restart to update to the new version.");
      Messages.messages.Add((object) "ReadRightOnly", (object) "You have read only rights to this loan. Do you want to open it in read only mode?");
      Messages.messages.Add((object) "OpenDownLoad", (object) "This loan is locked because {0} has downloaded it. Do you want to open it in read only mode?");
      Messages.messages.Add((object) "OpenWork", (object) "This loan is locked because {0} is currently editing it. Do you want to open it in read only mode?");
    }

    public static string GetMessage(string key, params object[] args)
    {
      return string.Format((string) Messages.messages[(object) key], args);
    }
  }
}
