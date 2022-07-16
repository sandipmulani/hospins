namespace hospins.Repository.Models
{
    public class CommonColumn
    {
        public string Mode { get; set; }
        public string TableName { get; set; }
        public string ColumnsName { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public int PageNumber { get; set; }
        public int RecordPerPage { get; set; }
        public string WhereClause { get; set; }
    }
}