using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneratSettings;
namespace B_Layer
{
    public class clsDataBussinesLayer
    {

        public static DataTable GetAllDataBases()
        {
            return clsDataAccessLAyer.GetAllDataBasses();
        }
        public static DataTable GetAllTables(string DataBase)
        {
            return clsDataAccessLAyer.GetAlltables(DataBase);
        }
         public static string GeneratCodForTable(string TabelName)
        {
           return  clsDataAccessLAyer.GeneratDataAccess(TabelName);
        }  
        public static string GeneratBussenss(string TabelName)
        {
           return clsGeneratBussinessLayer.GeneratDataBussiness(TabelName);
        }
        public static void SetConnectionString(string ConnectionString)
        {
            clsDataAccessLAyer.SetStringConnection(ConnectionString);
        }
      static  public string GeneratSettinges()
        {
         return clsGeneratSettings.GeneratSettinges() ;
        }


 
    }
}
