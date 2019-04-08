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
                    WorksList = GetArtistWork(Name),
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
        public string DeleteArtist(string ArtistName)
        {
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                "DELETE FROM Artist WHERE Name = @ArtistName",
                prepareDeleteArtistParameters(ArtistName));
                if (lcRecCount == 1)
                    return "Artist:" + ArtistName + " has been removed";
                else
                    return "Unexpected artist update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        private Dictionary<string, object> prepareDeleteArtistParameters(string prName)
        {
            Dictionary<string,object> par = new Dictionary<string, object>();
            par.Add("ArtistName",prName);
            return par;
        }
        private clsAllWork dataRow2AllWork(DataRow prDataRow)
        {
            return new clsAllWork()
            {
                ArtistName = Convert.ToString(prDataRow["ArtistName"]),
                Name = Convert.ToString(prDataRow["Name"]),
                Date = Convert.ToDateTime(prDataRow["Date"]),
                Value = Convert.ToDecimal(prDataRow["Value"]),
                WorkType = Convert.ToChar(prDataRow["WorkType"]),
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
                    return "Artist: "+prArtist.Name+" has been updated";
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
                    return "Artist: "+prArtist.Name+" has been added";
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

        public string PostArtWork(clsAllWork prWork)
        { // insert
            try
            {
                int lcRecCount = clsDbConnection.Execute("INSERT INTO Work " +
                    "(WorkType, Name, Date, Value, Width, Height, Type, Weight, Material, ArtistName) " +
                    "VALUES (@WorkType, @Name, @Date, @Value, @Width, @Height, @Type, @Weight, @Material, @ArtistName)",
                    prepareWorkParameters(prWork));
                if (lcRecCount == 1) return "Artwork: "+prWork.Name+" has been added";
                else return "Unexpected artwork insert count: " + lcRecCount;
            }
            catch (Exception ex)
            { return ex.GetBaseException().Message; }
        }

        public string PutArtWork(clsAllWork prWork)
        { // insert
            try
            {
                int lcRecCount = clsDbConnection.Execute("UPDATE Work SET WorkType = @WorkType, Date = @Date, Value = @Value, Width = @Width, Height = @Height, Type = @Type, Weight = @Weight, Material = @Material WHERE ArtistName = @ArtistName AND Name = @Name ",
                    prepareWorkParameters(prWork));
                if (lcRecCount == 1) return "Artwork: "+prWork.ArtistName+" has updated";
                else return "Unexpected artwork insert count: " + lcRecCount;
            }
            catch (Exception ex)
            { return ex.GetBaseException().Message; }
        }

        public string DeleteArtwork(string WorkName, string ArtistName)
        {
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                "DELETE FROM Work WHERE Name = @WorkName AND ArtistName = @ArtistName",
                prepareDeleteWorkParameters(WorkName, ArtistName));
                if (lcRecCount == 1)
                    return "Artwork: "+WorkName+" has been removed from "+ArtistName;
                else
                    return "Unexpected artist update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }
        private Dictionary<string, object> prepareDeleteWorkParameters(string prWork, string prArtist)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(2);
            par.Add("WorkName", prWork);
            par.Add("ArtistName", prArtist);
            return par;
        }
        private Dictionary<string, object> prepareWorkParameters(clsAllWork prWork)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(10);
            par.Add("Name", prWork.Name);
            par.Add("Date", prWork.Date);
            par.Add("Value", prWork.Value);
            par.Add("WorkType", prWork.WorkType);
            par.Add("Height", prWork.Height);
            par.Add("Width", prWork.Width);
            par.Add("Type", prWork.Type);
            par.Add("Weight", prWork.Weight);
            par.Add("Material", prWork.Material);
            par.Add("ArtistName", prWork.ArtistName);
            // Etc: your turn: 
            return par;
        }


    }


}

