using System.Security;

namespace CutterManagement.Core
{
    public interface ISecurePassword : IDisposable
    {
        public SecureString Password { get; }
    }
}
