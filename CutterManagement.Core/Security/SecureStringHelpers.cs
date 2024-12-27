using System.Runtime.InteropServices;
using System.Security;

namespace CutterManagement.Core
{
    public static class SecureStringHelpers
    {
        public static string Decrypt(this SecureString secureString)
        {
            IntPtr bstr = IntPtr.Zero;
            string decryptedValue;

            try
            {
                // Resolve secure-string to plain text
                bstr = Marshal.SecureStringToBSTR(secureString);
                // Get the resolved text
                decryptedValue = Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                // free BSTR and zero out plain text from memory
                if (bstr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(bstr);
                }
            }

            // Return plain text
            return decryptedValue;
        }
    }
}
