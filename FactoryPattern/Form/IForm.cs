using Microsoft.AspNetCore.Mvc;

namespace WithAuthintication

{
    public interface IForm
    {
        public void GetEmail(string emails);
        public void ResetPassword(string emails, string password);
    }
}
