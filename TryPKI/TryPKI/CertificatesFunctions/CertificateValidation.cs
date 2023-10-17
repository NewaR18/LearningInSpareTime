using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TryPKI.Repository;

namespace TryPKI.CertificatesFunctions
{
    public class CertificateValidation
    {
        public bool ValidateCertificate(X509Certificate2 clientCertificate)
        {
            PKIRepo _repo=new PKIRepo();
            string ThumbPrintOfClient = _repo.GetThumbprints(463);
            if (ThumbPrintOfClient==clientCertificate.Thumbprint)
            {
                return true;
            }
            return false;
        }
    }
}
