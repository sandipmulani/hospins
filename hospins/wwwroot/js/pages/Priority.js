var where = "1=1";
$(document).ready(function () {
    BindGrid();
    $("#tblPriority").on("click", ".role-edit", function () {
        OpenPopup('modalPriority', 'contentPriority', addPriorityURL + '/' + $(this).attr("data-id"), 'frmPriority', null, reloadGrid);
    });
    $("#tblPriority").on("click", ".role-delete", function () {
        Delete(this, deletePriorityURL, 'Please confirm if you want to delete Priority?', 'Yes', 'No', reloadGrid);
    });

});

function reloadGrid() {
    var columns = [
        //{
        //    "title": '<div class="checkbox-fade fade-in-default"><label><input type="checkbox" id="flowcheckall" name="flowcheckall" onchange="chkAll(this,\'tblPriority\')" value=""><span class="cr"><i class="cr-icon icofont icofont-ui-check txt-primary"></i></span></label></div>',
        //    'searchable': false,
        //    'IsHideFromSelection': true,
        //    /*'visible': (isDeletePriority == "True") ? true : false,*/
        //    'orderable': false,
        //    'className': 'text-center remove-pr tw-fixed',
        //    "render": function (data, type, full, meta) {
        //        return '<div class="checkbox-fade fade-in-default"><label><input type="checkbox" ChekBoxType ="DeleteCheckBox" onchange="chkChild(this,\'tblPriority\');" name="chk_' + full.PriorityId + '" value = "' + full.PriorityId + '"><span class="cr"><i class="cr-icon icofont icofont-ui-check txt-primary"></i></span></label></div>';
        //    }
        //},
        { "title": "Priority", "data": "Name", "sWidth": "45%" },
        { "title": "Sort Order", "data": "SortOrder" },
        {
            'title': 'Status',
            'orderable': false,
            /*'visible': (isEditPriority == "True") ? true : false,*/
            'className': 'text-center tw-fixed all',
            'render': function (data, type, full, meta) {
                if (full.IsActive == 1) {
                    return '<div class="d-inline-block align-items-center justify-content-center"><label class="switch small-switch switch-danger mb-0"><input type="checkbox" checked="" data-id="' + full.PriorityId + '" onchange="ChangeStatus(this, \'' + statusPriorityURL + '\' ,\'Are you sure want to change this record status?\',\'Yes\',\'No\', reloadGrid)"><span class="switch-slider"></span></label></div>';
                }
                else {
                    return '<div class="d-inline-block align-items-center justify-content-center"><label class="switch small-switch switch-danger mb-0"><input type="checkbox" data-id="' + full.PriorityId + '" onchange="ChangeStatus(this, \'' + statusPriorityURL + '\' ,\'Are you sure want to change this record status?\',\'Yes\',\'No\', reloadGrid)"><span class="switch-slider"></span></label></div>';
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
                html += '<button class="btn btn-sm btn-primary role-edit" data-id="' + full.PriorityId + '" data-tooltip="true" title="' + title +'"><i class="fa fa-pencil-square-o"></i></button>&nbsp;';
                html += '<button class="btn btn-sm btn-primary role-delete" data-id="' + full.PriorityId + '" data-tooltip="true" title="Delete"><i class="fa fa-trash"></i></button>';
                return html;
            }
        }
    ];
    $('#tblPriority').createGrid({
        Columns: columns,
        Mode: 'Priority',
        SortColumn: '1',
        IsAddShow: true,
        OnAddLabel: "Add New Priority",
        OnAdd: function () {
            OpenPopup('modalPriority', 'contentPriority', addPriorityURL, 'frmPriority', null, reloadGrid);
            return false;
        },
        IsDeleteShow: true,
        OnDeleteLabel: "Delete",
        OnDelete: function () {
            DeleteGridData(this, 'tblPriority', deletePriorityURL, 'Priority', reloadGrid);
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


