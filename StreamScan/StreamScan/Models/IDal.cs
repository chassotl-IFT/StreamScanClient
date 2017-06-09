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
        Enterprise GetEnterprise(int enterprise);
        MySqlReturn InsertEnterprise(Enterprise enterprise);
        MySqlReturn UpdateEnterprise(Enterprise enterprise);
        MySqlReturn DeleteEnterprise(int id);
        //Facilities
        List<Facility> GetFacilities(int enterprise);
        Facility GetFacility(int facility);
        MySqlReturn InsertFacility(Facility facility);
        MySqlReturn UpdateFacility(Facility facility);
        MySqlReturn DeleteFacility(int id);
        //Machines
        Dictionary<int, List<Machine>> GetEnterpriseMachines(int enterprise);
        List<Machine> GetFacilityMachines(int facility);
        MySqlReturn InsertMachine(int facility, Info machine);
        MySqlReturn UpdateMachine(int version, int systemId, Info machine);
    }
}