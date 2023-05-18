using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BondInn
{
    public interface IAuth
    {
        Task<string> LoginWithEmailAndPassword(string email, string password);

        Task<string> SignUpWIthEmailAndPassword(string email, string password);

        bool SignOut();

        bool IsSignIn();
    }
}
