var where = "1=1";
$(document).ready(function () {
    BindGrid();
    $("#tblLogbook").on("click", ".role-delete", function () {
        Delete(this, deleteLogbookURL, 'Please confirm if you want to delete Logbook?', 'Yes', 'No', reloadGrid);
    });

});

function reloadGrid() {
    var columns = [
        { "title": "SL", "data": "LogbookId" },
        { "title": "Logbook", "data": "Name", "sWidth": "15%" },
        { "title": "AssisgnTo", "data": "AssisgnTo" },
        { "title": "Submit", "data": "CreatedBy" },
        { "title": "Log Check", "data": "CreatedDate" },
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
                html += '<a class="btn btn-sm btn-primary" href="' + addLogbookURL + '/' + full.LogbookId + '" data-tooltip="true" title="' + title +'"><i class="fa fa-pencil-square-o"></i></a>&nbsp;';
                html += '<a class="btn btn-sm btn-primary role-delete" data-id="' + full.LogbookId + '" data-tooltip="true" title="Delete"><i class="fa fa-trash"></i></button>';
                return html;
            }
        }
    ];
    $('#tblLogbook').createGrid({
        Columns: columns,
        Mode: 'Logbook',
        SortColumn: '1',
        IsAddShow: true,
        OnAddLabel: "Add New Logbook",
        OnAdd: function () {
            window.location.href = addLogbookURL;
            return false;
        },
        IsDeleteShow: true,
        OnDeleteLabel: "Delete",
        OnDelete: function () {
            DeleteGridData(this, 'tblLogbook', deleteLogbookURL, 'Logbook', reloadGrid);
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


