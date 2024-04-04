using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NMCAPI.Models;
using static NMCAPI.Models.NMC_Class;

namespace NMCAPI.Controllers
{
    public class NMCDATAController : ApiController
    {

		[HttpGet]
		public IHttpActionResult MyId(int ModuleCode, String FromDate, String ToDate)
		{

			string config = ConfigurationManager.ConnectionStrings["ServerConnectionoutsideDB"].ConnectionString;
			using (SqlConnection sqlcon = new SqlConnection(config))
			{
				SqlDataAdapter da = new SqlDataAdapter();
				SqlCommand cmd = new SqlCommand("dbo.GET_SN_NDHM_OP_IP_DETAILS", sqlcon);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@MODULWCODE", ModuleCode);
				cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
				cmd.Parameters.AddWithValue("@TODATE", ToDate);
				sqlcon.Open();
				cmd.ExecuteNonQuery();
				DataTable dt = new DataTable();
				SqlDataReader sdr = cmd.ExecuteReader();
				da.SelectCommand = cmd;
				DataSet ds = new DataSet();
				sqlcon.Close();
				da.Fill(ds);

				ds.Tables[0].TableName = "HospitalDetails";
				ds.Tables[1].TableName = "AppointmentCount";
				ds.Tables[2].TableName = "AppointmentList";

				//--------------------------------Hospital Details------------------------//
				List<dynamic> lstHospitalDetails = new List<dynamic>();
				if (ds.Tables[0].Rows.Count > 0)
				{
					HospitalDetails hospitalDetails = new HospitalDetails();
					hospitalDetails.from_date = ds.Tables[0].Rows[0]["fromDate"].ToString();
					hospitalDetails.to_date = ds.Tables[0].Rows[0]["toDate"].ToString();
					hospitalDetails.hf_id_hmis = Convert.ToInt32(ds.Tables[0].Rows[0]["hfidHMIS"]);
					hospitalDetails.hf_id_abdm = ds.Tables[0].Rows[0]["hfidABDM"].ToString();
					lstHospitalDetails.Add(hospitalDetails);
				}

				//--------------------------------Appointment Count Details------------------------//
				List<dynamic> lstapointmentCounts = new List<dynamic>();
				if (ds.Tables[1].Rows.Count > 0)
				{
					ApointmentCount apointmentCount = new ApointmentCount();
					if (ModuleCode == 01)
					{
						apointmentCount.Total_Appointment_Count = Convert.ToInt32(ds.Tables[1].Rows[0]["opd_count"]);
					}
					else
					{
						apointmentCount.Total_Appointment_Count = Convert.ToInt32(ds.Tables[1].Rows[0]["ipd_count"]);

					}
					lstapointmentCounts.Add(apointmentCount);
				}

				//--------------------------------Patient Details------------------------//
				List<dynamic> lstpatienDetails = new List<dynamic>();
				if (ds.Tables[2].Rows.Count > 0)
				{
					for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
					{
						PatienDetails patienDetails = new PatienDetails();
						patienDetails.patient_name = ds.Tables[2].Rows[i]["patient_name"].ToString();
						patienDetails.patient_age = Convert.ToInt32(ds.Tables[2].Rows[i]["patient_age"]);
						patienDetails.address = ds.Tables[2].Rows[i]["address"].ToString();
						patienDetails.patient_abha_id = ds.Tables[2].Rows[i]["patient_abha_id"].ToString();
						patienDetails.patient_identification_proof = ds.Tables[2].Rows[i]["patient_identification_proof"].ToString();
						patienDetails.patient_identification_number = ds.Tables[2].Rows[i]["patient_identification_number"].ToString();
						patienDetails.patient_mobile_number = ds.Tables[2].Rows[i]["patient_mobile_number"].ToString();
						patienDetails.transaction_type = ds.Tables[2].Rows[i]["transaction_type"].ToString();
						patienDetails.uhid_number = ds.Tables[2].Rows[i]["uhid_number"].ToString();
						patienDetails.department_visited_name = ds.Tables[2].Rows[i]["department_visited_name"].ToString();
						patienDetails.department_visited_code = ds.Tables[2].Rows[i]["department_visited_code"].ToString();
						patienDetails.datetime_of_transaction = ds.Tables[2].Rows[i]["datetime_of_transaction"].ToString();
						lstpatienDetails.Add(patienDetails);
					}
				}

				//var result = lstHospitalDetails.Concat(lstpatienDetails).ToList();
				var result = lstHospitalDetails.Concat(lstapointmentCounts).Concat(lstpatienDetails).ToList();

				if (result.Count > 0)
				{
					return Ok(result);

				}
				else
				{
					return Ok("No Data Found");
				}


			}

		}
	}
}
