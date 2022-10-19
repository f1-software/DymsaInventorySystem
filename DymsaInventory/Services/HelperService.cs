
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace DymsaInventory.Services
{
    public interface IHelperService
    {
        string DbQuery(string query);
    }

    public class HelperService : IHelperService
    {
        public IConfiguration config { get; set; } = null!;
        public HelperService(IConfiguration config)
        {
            this.config = config;
        }

        public string DbQuery(string query)
        {
            var connstring = config.GetConnectionString("SomeeConnection");
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connstring))
            {
                SqlCommand objSqlCommand = new SqlCommand(query, con);
                objSqlCommand.CommandType = CommandType.Text;
                SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);
                try
                {
                    objSqlDataAdapter.Fill(dt);
                    string jsonresult = JsonConvert.SerializeObject(dt);
                    return jsonresult;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
