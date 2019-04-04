using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Gallery3SelfHost
{
    public class GalleryController : System.Web.Http.ApiController
    {
        public List<string> GetArtistNames()
        {
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT Name FROM Artist", null);
            List<string> lcNames = new List<string>();
            foreach (DataRow dr in lcResult.Rows)
                lcNames.Add((string)dr[0]);
            return lcNames;
        }


        public clsArtist GetArtist(string Name)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1);
            par.Add("Name", Name);
            DataTable lcResult =
            clsDbConnection.GetDataTable("SELECT * FROM Artist WHERE Name = @Name", par);
            if (lcResult.Rows.Count > 0)
                return new clsArtist()
                {
                    Name = (string)lcResult.Rows[0]["Name"],
                    Speciality = (string)lcResult.Rows[0]["Speciality"],
                    Phone = (string)lcResult.Rows[0]["Phone"],
                    WorksList = GetArtistWork(Name)
                };
            else
                return null;
        }

        private List<clsAllWork> GetArtistWork(string Name)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1);
            par.Add("Name", Name); DataTable lcResult = clsDbConnection.GetDataTable("SELECT * FROM Work WHERE ArtistName = @Name", par);
            List<clsAllWork> lcWorks = new List<clsAllWork>();
            foreach (DataRow dr in lcResult.Rows)
                lcWorks.Add(dataRow2AllWork(dr));
            return lcWorks;
        }

        private clsAllWork dataRow2AllWork(DataRow prDataRow)
        {
            return new clsAllWork()
            {
                Name = Convert.ToString(prDataRow["Name"]),
                Date = Convert.ToDateTime(prDataRow["Date"]),
                Value = Convert.ToDecimal(prDataRow["Value"]),
                Height = prDataRow["Height"] is DBNull ? (float?)null : Convert.ToSingle(prDataRow["Height"]),
                Width = prDataRow["Width"] is DBNull ? (float?)null : Convert.ToSingle(prDataRow["Width"]),
                Type = Convert.ToString(prDataRow["Type"]),
                Material = Convert.ToString(prDataRow["Material"]),
                Weight = prDataRow["Weight"] is DBNull ? (float?)null : Convert.ToSingle(prDataRow["Weight"])
            };
        }

        public string PutArtist(clsArtist prArtist)
        { // update
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                "UPDATE Artist SET Speciality = @Speciality, Phone = @Phone WHERE Name = @Name",
                prepareArtistParameters(prArtist));
                if (lcRecCount == 1)
                    return "One artist updated";
                else
                    return "Unexpected artist update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        public string PostArtist(clsArtist prArtist)
        {
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                "INSERT INTO Artist(Name,Phone,Speciality)Values(@Name,@Phone,@Speciality);",
                prepareArtistParameters(prArtist));
                if (lcRecCount == 1)
                    return "One artist Inserted";
                else
                    return "Unexpected artist update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        private Dictionary<string, object> prepareArtistParameters(clsArtist prArtist)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(3);
            par.Add("Name", prArtist.Name);
            par.Add("Speciality", prArtist.Speciality);
            par.Add("Phone", prArtist.Phone);
            return par;

        }
    }
}

