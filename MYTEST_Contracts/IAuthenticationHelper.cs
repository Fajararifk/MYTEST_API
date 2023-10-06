using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTEST_Contracts
{
    public interface IAuthenticationHelper
    {
        int? GetConnectedUserId();
        bool HasPermission(string action);
        bool HasRole(string role);
    }
}
