using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


//important add this
using System.Linq.Dynamic;

using System.Data.Objects.SqlClient;
using jGrid.Helpers;
using MI.WebUI.Infrastructure;
using MI.Domain.jGridHelpers;
using MI.Domain.Models;
using Massive;
using System.Dynamic;
namespace MI.WebUI.Controllers
{
    public class PlacementsController : Controller
    {


        DynamicModel tbl = new Placements();




        public PlacementsController()
        {

        }


        public ActionResult Index()
        {
            return View();
        }


        public JsonResult DynamicGridData(string sidx, string sord, int page, int rows, bool _search, string filters)
        {

            //note: a view is used here. This is because the we want to display the PUBLICATION_NETWORK field from the PUBLICATION_NETWORKS table
            //OID_PUBLISHER_ID field of the  MEDIA_CAT_DIGITAL_PLCMNT table is hooked up to a combo box: ref method PublishersCombo() which feeds the combo box
            //if just the data from the MEDIA_CAT_DIGITAL_PLCMNT table was to be dispayed you could simply pass in the Placements class like so:
            //SearchHelper sh = new SearchHelper(new Placements(), ShapeGridRow);

            //display a view
            DynamicModel myview = new View_Placements_Publications();
            jgridDisplayHelper sh = new jgridDisplayHelper(myview, ShapeGridRow);

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = tbl.Count(); //repository.TimeCards.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);



            JsonResult mydata = sh.GetSearchResults(sidx, sord, page, rows, _search, filters);

            return mydata;


        }

        //this is a delagate passed in to the jgridDisplayHelper. Each grid will want to display different columns-- you define the shape( ie what fields you want displayed) of the grid row here.

        private static List<string> ShapeGridRow(dynamic result)
        {
            List<string> fields_to_display = new List<string>() { "OID_MEDIA_CAT_DIGITAL_PLCMNT", "MEDIA_GENRE", "MEDIA_PLAN_NAME", "OID_PUBLISHER_ID", "PUBLICATION_NETWORK", "PLACEMENT_NAME", "PLACEMENT_CODE", "BEGIN_DATE", "END_DATE", "COST" };
            List<string> field_values = new List<string>();

            foreach (string s in fields_to_display)
            {
                string myvalue = getFieldValue(result, s);
                field_values.Add(myvalue);
            }

            return field_values;
        }

        //this is a delagate passed in to the SearchHelper. Each grid will want to display different columns-- you define the shape of the grid row here.
        //private static List<string> xxShapeGridRow(dynamic result)
        //{
        //    List<string> s = new List<string>();


        //    string a = "";
        //    string b = "";
        //    string c = "";
        //    string d = "";
        //    string e = "";
        //    string f = "";
        //    string g = "";
        //    string h = "";
        //    string i = "";
        //    string j = "";

        //    if (result.OID_MEDIA_CAT_DIGITAL_PLCMNT != null)
        //    {
        //        a = result.OID_MEDIA_CAT_DIGITAL_PLCMNT.ToString();

        //    }

        //    if (result.MEDIA_GENRE.ToString() != null)
        //    {
        //        b = result.MEDIA_GENRE.ToString().ToString();

        //    }

        //    if (result.MEDIA_PLAN_NAME != null)
        //    {
        //        c = result.MEDIA_PLAN_NAME.ToString();

        //    }

        //    if (result.OID_PUBLISHER_ID != null)
        //    {
        //        d = result.OID_PUBLISHER_ID.ToString();

        //    }

        //    if (result.PUBLICATION_NETWORK != null)
        //    {
        //        e = result.PUBLICATION_NETWORK.ToString();

        //    }

        //    if (result.PLACEMENT_NAME != null)
        //    {
        //        f = result.PLACEMENT_NAME.ToString();

        //    }
        //    if (result.PLACEMENT_CODE != null)
        //    {
        //        g = result.PLACEMENT_CODE.ToString();

        //    }

        //    if (result.BEGIN_DATE != null)
        //    {
        //        h = Formatdate((result.BEGIN_DATE.ToString()));

        //    }

        //    if (result.END_DATE != null)
        //    {
        //        i = Formatdate(result.END_DATE.ToString());

        //    }
        //    if (result.COST != null)
        //    {
        //        j = result.COST.ToString();

        //    }




        //    s = new List<string>() { a, b, c, d, e, f, g, h, i, j };

        //    return s;
        //}
        //------ Sub Grid------------
        public JsonResult SubGridData(string id)
        {
            string s = @"
                 SELECT     MEDIA_STATISTICS.*
                FROM         MEDIA_CAT_DIGITAL_PLCMNT INNER JOIN
                 MEDIA_STATISTICS ON MEDIA_CAT_DIGITAL_PLCMNT.PLACEMENT_CODE 
                 = MEDIA_STATISTICS.PLACEMENT_CODE
                and OID_MEDIA_CAT_DIGITAL_PLCMNT =  ";

            s += id.ToString();


            IEnumerable<dynamic> results = tbl.Query(s);

            DynamicModel myview = new View_Placements_Publications();
            jgridDisplayHelper sh = new jgridDisplayHelper(ShapeSubGridRow);

            JsonResult myJasonResults = sh.GetJson(results,0, 0, 0);


            return myJasonResults;

          
        }

      


        //this is a delagate passed in to the jgridDisplayHelper. Each grid will want to display different columns-- you define the shape( ie what fields you want displayed) of the grid row here.
        private static List<string> ShapeSubGridRow(dynamic result)
        {
            List<string> fields_to_display = new List<string>() { "PLACEMENT_CODE", "CLICKS", "IMPRESSIONS", "NETWORK" };
            List<string> field_values = new List<string>();

            foreach (string s in fields_to_display)
            {
                string myvalue = getFieldValue(result, s);
                field_values.Add(myvalue);
            }

            return field_values;
        }

        //Sub grid end ---------------

        [HttpPost]

        public ActionResult Update(int id, FormCollection formCollection)
        {
            var model = tbl.CreateFrom(formCollection);

            try
            {

                tbl.Update(model, id);

            }
            catch (Exception x)
            {
                TempData["Error"] = "There was a problem editing this record";

            }


            return null;

        }

        [HttpPost]

        public ActionResult Insert(dynamic viewModel, FormCollection formCollection)
        {
            var model = tbl.CreateFrom(formCollection);

            try
            {

                tbl.Insert(model);

            }
            catch (Exception x)
            {
                TempData["Error"] = "There was a problem adding this record";

            }

            return null;

        }
        [HttpPost]
        public string Delete(string id)
        {

            try
            {
              
                tbl.Delete(id);
               
            }
            catch
            {
                TempData["Error"] = "There was a problem deleting this record";
            }

            return "true";
        }


        //---------combos-------

        public ActionResult PublishersCombo()
        {
            Dictionary<int, string> pubs = new Dictionary<int, string>();

            Publishers tbl = new Publishers();
            dynamic all = tbl.All();
            foreach (dynamic pub in all)
            {
                //use this if you want to store the key and display descriptive name
                pubs.Add(pub.OID_PUBLICATION_NETWORK_LU, pub.PUBLICATION_NETWORK);

                //displays and stores descriptive  name 
                // pubs.Add(pub.PUBLICATION_NETWORK, pub.PUBLICATION_NETWORK);
            }



            return PartialView("SelectPartial", pubs);
        }

       

        //------helpers------------------
        public static string getFieldValue(dynamic myexpando, string fieldname)
        {
            string ret = "";

            foreach (KeyValuePair<string, object> pair in myexpando)
            {
                string val = "";
                if (pair.Value != null)
                {
                    val = pair.Value.ToString();
                    if (pair.Key == fieldname)
                    {
                        ret = val;

                        //format dates
                        if ( fieldname.ToLower().Contains("date"))
                        {
                            ret = Formatdate(val);
                        }
                    }
                }

            }

            return ret;

        }
       
        public static string Formatdate(string dtin)
        {
            string dtret = dtin;
            DateTime dt;
            if (dtin != null)
            {
                if (DateTime.TryParse(dtin.ToString(), out dt))
                {
                    dtret = String.Format("{0:M/d/yyyy}", dt);
                }
            }


            return dtret;

        }

    }//class


}//ns
