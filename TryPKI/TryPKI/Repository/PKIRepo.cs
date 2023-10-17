using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TryPKI.Repository
{
    public class PKIRepo: ConnRepo
    {
        public string GetThumbprints(int ClientId)
        {
            string Thumbprint = "";
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select ThumbPrint_PKI from CLIENT_MAST  where CLIENT_ID=@CLient_Id";
                using (SqlConnection conn1 = Connection())
                {
                    cmd.Connection = conn1;
                    cmd.CommandTimeout = 30;
                    cmd.Parameters.Add("@Client_Id", SqlDbType.Int).Value = ClientId;
                    using (SqlDataReader sd = cmd.ExecuteReader())
                    {
                        if (sd.HasRows)
                        {
                            while (sd.Read())
                            {
                                Thumbprint = sd["ThumbPrint_PKI"]==null?"": sd["ThumbPrint_PKI"].ToString();
                            }
                        }
                    }
                }
            }
            return Thumbprint;
        }
    }
}
