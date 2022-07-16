using Microsoft.AspNetCore.Http;
using System.Data;

namespace hospins.Infrastructure
{
    public class GridParams
    {
        public HttpContext _HTTPContext;

        /// <summary>
        /// name of table. incase of join (tblA inner join tblB ON tblA.columnA=tblB.columnB)
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Column names which will be required for page
        /// </summary>
        public string ColumnsName { get; set; }

        /// <summary>
        /// default sort column
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        /// default sort order
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// default page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// default total rows per page
        /// </summary>
        public int RecordPerPage { get; set; }

        /// <summary>
        /// where cluase for grid query
        /// </summary>
        public string WhereClause { get; set; }

        /// <summary>
        /// column list for export file
        /// </summary>
        public string ExportedColumns { get; set; }

        /// <summary>
        /// exported file name
        /// </summary>
        public string ExportedFileName { get; set; }

        public string GetData()
        {
            GridFunctions oGrid = new GridFunctions(_HTTPContext);
            return oGrid.GetJson(this);
        }

        public DataTable GetDataTable()
        {
            GridFunctions oGrid = new GridFunctions(_HTTPContext);
            return oGrid.GetDataTable(this);
        }

        public void ExportData()
        {
            GridFunctions oGrid = new GridFunctions(_HTTPContext);
            oGrid.Export(this);
        }
    }

    public class SearchGrid
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string Datatype { get; set; }
    }
}