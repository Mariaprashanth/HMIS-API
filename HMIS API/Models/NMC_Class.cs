using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMCAPI.Models
{
	public class NMC_Class
	{

		public class PatienDetails
		{
			public String patient_name { get; set; }
			public int patient_age { get; set; }
			public String address { get; set; }
			public String patient_abha_id { get; set; }
			public String patient_identification_proof { get; set; }
			public String patient_identification_number { get; set; }
			public String patient_mobile_number { get; set; }
			public String transaction_type { get; set; }
			public String uhid_number { get; set; }
			public String department_visited_name { get; set; }
			public String department_visited_code { get; set; }
			public String datetime_of_transaction { get; set; }

		}
		public class HospitalDetails
		{
			public String from_date { get; set; }
			public String to_date { get; set; }
			public int hf_id_hmis { get; set; }
			public String hf_id_abdm { get; set; }

		}
		public class ApointmentCount
		{
			
			public int Total_Appointment_Count { get; set; }

		}

	}
}