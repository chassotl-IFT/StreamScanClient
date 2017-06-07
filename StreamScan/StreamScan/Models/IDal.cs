using QueriesManager.Bean;
using StreamScanCommon.Infos;
using System.Collections.Generic;

namespace StreamScan.Models
{
    public interface IDal
    {
        //Account
        string Login(Login login);
        //Enterprises
        List<Enterprise> GetEnterprises();
        MySqlReturn InsertEnterprise(Enterprise enterprise);
        MySqlReturn UpdateEnterprise(Enterprise enterprise);
        MySqlReturn DeleteEnterprise(int id);
        //Facilities
        List<Facility> GetFacilities(int enterprise);
        MySqlReturn InsertFacility(Facility facility, int enterprise);
        MySqlReturn UpdateFacility(Facility facility, int enterprise);
        MySqlReturn DeleteFacility(int id);
        //Machines
        Dictionary<int, List<Machine>> GetEnterpriseMachines(int enterprise);
        List<Machine> GetFacilityMachines(int facility);
        MySqlReturn InsertMachine(int facility, Info machine);
        MySqlReturn UpdateMachine(int id, Info machine);
    }
}