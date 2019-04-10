using System;
using System.Collections.Generic;
using System.Data;

namespace Gallery3SelfHost
{
    /// <summary>
    /// API controller to be accessed from the "web". in this case its a local port 60064
    /// </summary>
    public class GalleryController : System.Web.Http.ApiController
    {
        /// <summary>
        /// Gets all names from the artist table. this will be used by the GET protocol
        /// </summary>
        /// <returns>A list of all the names in the artist table</returns>
        public List<string> GetArtistNames()
        {

            DataTable lcResult = clsDbConnection.GetDataTable("SELECT Name FROM Artist", null);
            List<string> lcNames = new List<string>();
            foreach (DataRow dr in lcResult.Rows)
                lcNames.Add((string)dr[0]);
            return lcNames;
        }

        /// <summary>
        /// Gets all data about one existing artist. This uses the GET protocol
        /// </summary>
        /// <param name="Name">String name of the artist you want to search for</param>
        /// <returns>returns a clsArtist(from DTO.cs) filled with the data returned from the database. else null</returns>
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

        /// <summary>
        /// Gets the artists work linked with the artist you selected.
        /// </summary>
        /// <param name="Name">string of the artists name</param>
        /// <returns>Multiple clsAllwork(from DTO.cs) filled with data retrieved from the database</returns>
        private List<clsAllWork> GetArtistWork(string Name)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1);
            par.Add("Name", Name); DataTable lcResult = clsDbConnection.GetDataTable("SELECT * FROM Work WHERE ArtistName = @Name", par);
            List<clsAllWork> lcWorks = new List<clsAllWork>();
            foreach (DataRow dr in lcResult.Rows)
                lcWorks.Add(dataRow2AllWork(dr));
            return lcWorks;
        }

        /// <summary>
        /// Deletes artist from the database and all artwork linked with the artist
        /// </summary>
        /// <param name="ArtistName">string of the artists name</param>
        /// <returns>if it was able to delete the data or not, depending on how many rows were effected by the SQL statement</returns>
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

        /// <summary>
        /// Puts the artist details into a dictionary for the SQL statement
        /// </summary>
        /// <param name="prName">string name of the artist</param>
        /// <returns></returns>
        private Dictionary<string, object> prepareDeleteArtistParameters(string prName)
        {
            Dictionary<string,object> par = new Dictionary<string, object>();
            par.Add("ArtistName",prName);
            return par;
        }

        /// <summary>
        /// Converts all the data from the database into the correct datatypes for the clsallwork( from DTO.cs)
        /// </summary>
        /// <param name="prDataRow">the data row from the SQL statement it returned</param>
        /// <returns>a completed clsAllWork class</returns>
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

        /// <summary>
        /// Updates the existing artist details in the datbase. uses the PUT protocol
        /// </summary>
        /// <param name="prArtist">updated clsArtist(from DTO.cs) you want to update to the database</param>
        /// <returns>row count of if it added or not. depending on the number result. will return a messagebox</returns>
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

        /// <summary>
        /// Inserting a new artist into the databse. uses the post protocol
        /// </summary>
        /// <param name="prArtist">newly filled clsArtist(from DTO.cs) to be formated into SQL and inserted</param>
        /// <returns>row count of if it works or not, or if their is an exception error</returns>
        public string PostArtist(clsArtist prArtist)
        {//insert
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

        /// <summary>
        /// turning the class into a format which the database can use
        /// </summary>
        /// <param name="prArtist">clsArtist(from DTO.cs) that you are turning into a dictonary</param>
        /// <returns>Dictionary of the artist data, to be used in the SQL statement</returns>
        private Dictionary<string, object> prepareArtistParameters(clsArtist prArtist)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(3);
            par.Add("Name", prArtist.Name);
            par.Add("Speciality", prArtist.Speciality);
            par.Add("Phone", prArtist.Phone);
            return par;

        }

        /// <summary>
        /// Inserting the artwork into the database
        /// </summary>
        /// <param name="prWork">Taking the newly made clsAllWork(from DTO.cs) is being prepared for SQL insertion</param>
        /// <returns>row count of how many rows were effected by the SQL statemet</returns>
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

        /// <summary>
        /// Updating the database of the selected work
        /// </summary>
        /// <param name="prWork">newly updated work data</param>
        /// <returns>row count of if the SQL statement worked</returns>
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

        /// <summary>
        /// Deleting the artwork from the database
        /// </summary>
        /// <param name="WorkName">Name of the artwork</param>
        /// <param name="ArtistName">name of the artist</param>
        /// <returns>row count of if the deletion worked</returns>
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
        /// <summary>
        /// Adding the strings into a dictionary for the SQL statement
        /// </summary>
        /// <param name="prWork">name of the Artwork</param>
        /// <param name="prArtist">name of the artist name</param>
        /// <returns>the dictionary of the two parameters </returns>
        private Dictionary<string, object> prepareDeleteWorkParameters(string prWork, string prArtist)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(2);
            par.Add("WorkName", prWork);
            par.Add("ArtistName", prArtist);
            return par;
        }

        /// <summary>
        /// Adding strings into a dictionary for the SQL statement
        /// </summary>
        /// <param name="prWork">name of the artwork</param>
        /// <returns>the dictionary of the parameter</returns>
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

