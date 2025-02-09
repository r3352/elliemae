// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.RolesComparer
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.Common;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class RolesComparer : IComparer
  {
    int IComparer.Compare(object x, object y)
    {
      return string.Compare(((RoleSummaryInfo) x).RoleName, ((RoleSummaryInfo) y).RoleName, false);
    }
  }
}
