var where = "1=1";
$(document).ready(function () {
    BindGrid();
    $("#tblDesignation").on("click", ".role-edit", function () {
        OpenPopup('modalDesignation', 'contentDesignation', addDesignationURL + '/' + $(this).attr("data-id"), 'frmDesignation', null, reloadGrid);
    });
    $("#tblDesignation").on("click", ".role-delete", function () {
        Delete(this, deleteDesignationURL, 'Please confirm if you want to delete Designation?', 'Yes', 'No', reloadGrid);
    });

});

function reloadGrid() {
    var columns = [
        { "title": "Designation", "data": "Name", "sWidth": "45%" },
        { "title": "Sort Order", "data": "SortOrder" },
        {
            'title': 'Status',
            'orderable': false,
            /*'visible': (isEditDesignation == "True") ? true : false,*/
            'className': 'text-center tw-fixed all',
            'render': function (data, type, full, meta) {
                if (full.IsActive == 1) {
                    return '<div class="d-inline-block align-items-center justify-content-center"><label class="switch small-switch switch-danger mb-0"><input type="checkbox" checked="" data-id="' + full.DesignationId + '" onchange="ChangeStatus(this, \'' + statusDesignationURL + '\' ,\'Are you sure want to change this record status?\',\'Yes\',\'No\', reloadGrid)"><span class="switch-slider"></span></label></div>';
                }
                else {
                    return '<div class="d-inline-block align-items-center justify-content-center"><label class="switch small-switch switch-danger mb-0"><input type="checkbox" data-id="' + full.DesignationId + '" onchange="ChangeStatus(this, \'' + statusDesignationURL + '\' ,\'Are you sure want to change this record status?\',\'Yes\',\'No\', reloadGrid)"><span class="switch-slider"></span></label></div>';
                }
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
                html += '<button class="btn btn-sm btn-primary role-edit" data-id="' + full.DesignationId + '" data-tooltip="true" title="' + title +'"><i class="fa fa-pencil-square-o"></i></button>&nbsp;';
                html += '<button class="btn btn-sm btn-primary role-delete" data-id="' + full.DesignationId + '" data-tooltip="true" title="Delete"><i class="fa fa-trash"></i></button>';
                return html;
            }
        }
    ];
    $('#tblDesignation').createGrid({
        Columns: columns,
        Mode: 'Designation',
        SortColumn: '1',
        IsAddShow: true,
        OnAddLabel: "Add New Designation",
        OnAdd: function () {
            OpenPopup('modalDesignation', 'contentDesignation', addDesignationURL, 'frmDesignation', null, reloadGrid);
            return false;
        },
        IsDeleteShow: true,
        OnDeleteLabel: "Delete",
        OnDelete: function () {
            DeleteGridData(this, 'tblDesignation', deleteDesignationURL, 'Designation', reloadGrid);
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


