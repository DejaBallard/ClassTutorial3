using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Gallery3SelfHost
{
    public class clsGalleryController : System.Web.Http.ApiController
    {
        public List<string> GetArtistNames()
        {
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT Name FROM Artist", null);
            List<string> lcNames = new List<string>();
            foreach (DataRow dr in lcResult.Rows)
                lcNames.Add((string)dr[0]);
            return lcNames;
        }
    }
}
