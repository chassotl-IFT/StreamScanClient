using System.Collections.Generic;

namespace StreamScan.Models {
	public interface IDal  {
		List<Enterprise> GetEnterprises();
		List<Facility> GetFacilities(int enterprise);
	}
}