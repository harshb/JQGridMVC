using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MI.Domain.Models;
using System.Dynamic;
using MI.Tests;
using jGrid.Helpers;
using System.IO;
using MI.Domain.jGridHelpers;

namespace MI.Tests
{

    [TestFixture]
    public class TestFixtureClass: TestBase
    {
        public delegate List<string> DelegateShapeGridRow(dynamic inString);
        jgridDisplayHelper sh = new jgridDisplayHelper(new Placements(), ShapeGridRow);
        Placements tbl = new Placements();

        [Test]
        public void TestName()
        {

           
           // dynamic x = tbl.All().Count();
            IEnumerable<dynamic> results = tbl.Paged(currentPage: 2, pageSize: 20).Items;

            //dynamic d = new ExpandoObject();
            //d = results.FirstOrDefault();
            //string desc = d.Description;

            foreach (dynamic result in results)
            {
                string desc = result.Description;
            }
           
        }

        
        [Test]
        public void test_search()
        {

            //Workdate~desc~1~20~True~{"groupOp":"AND","rules":[{"field":"TimeCard2Client","op":"eq","data":"10001868"},{"field":"Hours","op":"eq","data":"8"}]}
            //Workdate~desc~1~20~True~{"groupOp":"OR","rules":[{"field":"Hours","op":"eq","data":"8"},{"field":"Hours","op":"eq","data":"5"}]}
           
            string sidx ="WorkDate";
            string sord = "desc";
            int page = 1;
            int rows = 20;
            bool _search = false;
            string filters = "{\"groupOp\":\"AND\",\"rules\":[{\"field\":\"TimeCard2Client\",\"op\":\"eq\",\"data\":\"10001868\"},{\"field\":\"Hours\",\"op\":\"eq\",\"data\":\"8\"}]}";

            
            //IEnumerable<dynamic>  ret = BuildSql(sidx, sord, page, rows, _search, filters);
           
           sh.GetSearchResults(sidx, sord, page, rows, _search, filters);

            this.IsPending();

        }

        //----Test shape-----
        //Func does not work with dynamic hence have to use delegate
        
      

        [Test]
        public void test_shape()
        {
            IEnumerable<dynamic> results = tbl.Paged(where: "objid > 1", currentPage: 1, pageSize: 20).Items;

            ExpandoObject eo = new ExpandoObject();

            string a = "Description";

            // Use delegate instance to call UppercaseString method
            
            //string name = "Dakota";
            //string myshape = convertMethod(name);
           // List<string> myGridData = convertMethod(results);
            List<string> myGridData = new List<string>();
            //Func<string, List<string>> convertMethod = ShapeGridRow;
            DelegateShapeGridRow convertMethod = ShapeGridRow;
            foreach (dynamic result in results)
            {dynamic desc = result;
                 myGridData = convertMethod(result);
                
            }


            this.IsPending();

        }

        private static List<string> ShapeGridRow(dynamic result)
        {
           List<string> s=  new List<string>();
          
            s =  new List<string>() { result.autoid.ToString(), result.Objid.ToString(), result.WorkDate.ToString(), result.TimeCard2ServiceCode.ToString(), result.TimeCard2Client.ToString(), result.TimeCard2Employee.ToString(), result.Hours.ToString(), result.Amount.ToString(), result.Locked.ToString(), result.Description.ToString(), result.Created.ToString(), result.TimeCard2Creator.ToString(), result.TimeCard2EmpRate.ToString(), result.Code };
           

            return s;
        }
        
        //-----Test shape ends -----

        [Test]
        public void test_getdataType()
        {
            sh.GetDataType("Code");
            this.IsPending();

        }

        public void dodebug(String s)
        {

            if (s.Trim() != "")
            {
                string path = @"C:\debug.txt";
                StreamWriter SW = new StreamWriter(path, true);
                SW.WriteLine(s);
                SW.Flush();
                SW.Close();
            }
        }
        
    }//
}
