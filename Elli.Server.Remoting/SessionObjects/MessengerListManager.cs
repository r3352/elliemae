// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.MessengerListManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server;
using System;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class MessengerListManager : SessionBoundObject, IMessengerListManager
  {
    private const string className = "MessengerListManager";

    public MessengerListManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (MessengerListManager).ToLower());
      return this;
    }

    public virtual void AddGroup(string groupName)
    {
      this.onApiCalled(nameof (MessengerListManager), nameof (AddGroup), new object[1]
      {
        (object) groupName
      });
      try
      {
        IMDBAccessor.AddGroup(this.Session.UserID, groupName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MessengerListManager), ex);
      }
    }

    public virtual void AddContact(
      string groupName,
      string contactUserid,
      string firstName,
      string lastName,
      string reqMsg)
    {
      this.onApiCalled(nameof (MessengerListManager), nameof (AddContact), new object[5]
      {
        (object) groupName,
        (object) contactUserid,
        (object) firstName,
        (object) lastName,
        (object) reqMsg
      });
      try
      {
        IMDBAccessor.AddContact(this.Session.UserID, groupName, contactUserid, firstName, lastName, reqMsg);
        if (this.Session.UserID == contactUserid)
          return;
        SessionInfo[] allSessionsForUser = this.Session.Context.Sessions.GetAllSessionsForUser(contactUserid, true);
        if (allSessionsForUser == null || allSessionsForUser.Length == 0)
          return;
        this.Session.Context.Sessions.SendMessage((Message) new IMControlMessage(this.Session.UserID, contactUserid, IMMessage.MessageType.RequestAddToList, reqMsg), allSessionsForUser[0].SessionID, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MessengerListManager), ex);
      }
    }

    public virtual void DeleteGroup(string groupName)
    {
      this.onApiCalled(nameof (MessengerListManager), nameof (DeleteGroup), new object[1]
      {
        (object) groupName
      });
      try
      {
        IMDBAccessor.DeleteGroup(this.Session.UserID, groupName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MessengerListManager), ex);
      }
    }

    public virtual void DeleteContact(string groupName, string contactUserid)
    {
      this.onApiCalled(nameof (MessengerListManager), nameof (DeleteContact), new object[2]
      {
        (object) groupName,
        (object) contactUserid
      });
      try
      {
        IMDBAccessor.DeleteContact(this.Session.UserID, groupName, contactUserid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MessengerListManager), ex);
      }
    }

    public virtual void RenameGroup(string groupName, string newGroupName)
    {
      this.onApiCalled(nameof (MessengerListManager), nameof (RenameGroup), new object[2]
      {
        (object) groupName,
        (object) newGroupName
      });
      try
      {
        IMDBAccessor.RenameGroup(this.Session.UserID, groupName, newGroupName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MessengerListManager), ex);
      }
    }

    public virtual void MoveContact(string contactUserid, string fromGroup, string toGroup)
    {
      this.onApiCalled(nameof (MessengerListManager), nameof (MoveContact), new object[3]
      {
        (object) contactUserid,
        (object) fromGroup,
        (object) toGroup
      });
      try
      {
        IMDBAccessor.MoveContact(this.Session.UserID, contactUserid, fromGroup, toGroup);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MessengerListManager), ex);
      }
    }

    public virtual MessengerListInfo GetMessengerList()
    {
      this.onApiCalled(nameof (MessengerListManager), nameof (GetMessengerList), Array.Empty<object>());
      try
      {
        return IMDBAccessor.GetMessengerList(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MessengerListManager), ex);
        return (MessengerListInfo) null;
      }
    }

    public virtual bool ContainsGroup(string groupName)
    {
      this.onApiCalled(nameof (MessengerListManager), nameof (ContainsGroup), new object[1]
      {
        (object) groupName
      });
      try
      {
        return IMDBAccessor.ContainsGroup(this.Session.UserID, groupName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MessengerListManager), ex);
        return false;
      }
    }

    public virtual bool ContainsContact(string contactUserid)
    {
      this.onApiCalled(nameof (MessengerListManager), nameof (ContainsContact), new object[1]
      {
        (object) contactUserid
      });
      try
      {
        return IMDBAccessor.ContainsContact(this.Session.UserID, contactUserid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MessengerListManager), ex);
        return false;
      }
    }

    public virtual IMControlMessage[] GetStoredIMMessages()
    {
      this.onApiCalled(nameof (MessengerListManager), nameof (GetStoredIMMessages), Array.Empty<object>());
      try
      {
        return IMDBAccessor.GetStoredIMMessages(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MessengerListManager), ex);
        return (IMControlMessage[]) null;
      }
    }
  }
}
