using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A_Layer;
using GeneratSettings;
namespace B_Layer
{
    public class clsDataBussinesLayer
    {
        public static void SetApplicationName(string AppName)
        {
            clsGeneralUtils.ApplicationName = AppName;
        }
        public static DataTable GetAllDataBases()
        {
            return clsGeneralUtils.GetAllDataBasses();
        }
        public static DataTable GetAllTables(string DataBase)
        {
            return clsGeneralUtils.GetAlltables(DataBase);
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
            clsGeneralUtils.SetStringConnection(ConnectionString);
        }
      static  public string GeneratSettinges()
        {
         return clsGeneratSettings.GeneratSettinges() ;
        }


 
    }
}
