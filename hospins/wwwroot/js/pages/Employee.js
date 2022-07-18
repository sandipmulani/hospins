var where = "1=1";
$(document).ready(function () {
    BindGrid();
    $("#tblEmployee").on("click", ".role-delete", function () {
        Delete(this, deleteEmployeeURL, 'Please confirm if you want to delete Employee?', 'Yes', 'No', reloadGrid);
    });

});

function reloadGrid() {
    var columns = [
        { "title": "SL", "data": "EmployeeId" },
        { "title": "Employee", "data": "Name", "sWidth": "15%" },
        { "title": "Designation", "data": "Designation" },
        { "title": "Phone", "data": "Mobile" },
        { "title": "Email", "data": "Email" },
        {
            'title': 'Picture',
            'IsHideFromSelection': true,
            'searchable': false,
            'orderable': false,
            'className': 'text-center',
            'render': function (data, type, full, meta) {
                var html = '';
                html += `<img src="EmployeeDoc/${full.Picture}" height="60px" width="80px"></img>`;
                return html;
            }
        },
        {
            'title': 'Action',
            'IsHideFromSelection': true,
            'searchable': false,
            'orderable': false,
            'className': 'text-center action-btn2',
            'render': function (data, type, full, meta) {
                var title = "Edit";
                var icon = "edit";
                var html = '';
                html += '<a class="btn btn-sm btn-primary" href="' + addEmployeeURL + '/' + full.EmployeeId + '" data-tooltip="true" title="' + title +'"><i class="fa fa-pencil-square-o"></i></a>&nbsp;';
                html += '<a class="btn btn-sm btn-primary role-delete" data-id="' + full.EmployeeId + '" data-tooltip="true" title="Delete"><i class="fa fa-trash"></i></button>';
                return html;
            }
        }
    ];
    $('#tblEmployee').createGrid({
        Columns: columns,
        Mode: 'Employee',
        SortColumn: '1',
        IsAddShow: true,
        OnAddLabel: "Add New Employee",
        OnAdd: function () {
            window.location.href = addEmployeeURL;
            return false;
        },
        IsDeleteShow: true,
        OnDeleteLabel: "Delete",
        OnDelete: function () {
            DeleteGridData(this, 'tblEmployee', deleteEmployeeURL, 'Employee', reloadGrid);
            return false;
        },
        SortOrder: 'asc',
        FixClause: where,
        SearchParams: filterArr
        //,FixedRightColumns: 2
    });
}

// for clear form
function filterCallBack() {
    reloadGrid();
}


