var where = "1=1";
$(document).ready(function () {
    BindGrid();
    $("#tblSalaryType").on("click", ".role-edit", function () {
        OpenPopup('modalSalaryType', 'contentSalaryType', addSalaryTypeURL + '/' + $(this).attr("data-id"), 'frmSalaryType', null, reloadGrid);
    });
    $("#tblSalaryType").on("click", ".role-delete", function () {
        Delete(this, deleteSalaryTypeURL, 'Please confirm if you want to delete SalaryType?', 'Yes', 'No', reloadGrid);
    });

});

function reloadGrid() {
    var columns = [
        //{
        //    "title": '<div class="checkbox-fade fade-in-default"><label><input type="checkbox" id="flowcheckall" name="flowcheckall" onchange="chkAll(this,\'tblSalaryType\')" value=""><span class="cr"><i class="cr-icon icofont icofont-ui-check txt-primary"></i></span></label></div>',
        //    'searchable': false,
        //    'IsHideFromSelection': true,
        //    /*'visible': (isDeleteSalaryType == "True") ? true : false,*/
        //    'orderable': false,
        //    'className': 'text-center remove-pr tw-fixed',
        //    "render": function (data, type, full, meta) {
        //        return '<div class="checkbox-fade fade-in-default"><label><input type="checkbox" ChekBoxType ="DeleteCheckBox" onchange="chkChild(this,\'tblSalaryType\');" name="chk_' + full.SalaryTypeId + '" value = "' + full.SalaryTypeId + '"><span class="cr"><i class="cr-icon icofont icofont-ui-check txt-primary"></i></span></label></div>';
        //    }
        //},
        { "title": "Salary Type", "data": "Name", "sWidth": "45%" },
        { "title": "Sort Order", "data": "SortOrder" },
        {
            'title': 'Status',
            'orderable': false,
            /*'visible': (isEditSalaryType == "True") ? true : false,*/
            'className': 'text-center tw-fixed all',
            'render': function (data, type, full, meta) {
                if (full.IsActive == 1) {
                    return '<div class="d-inline-block align-items-center justify-content-center"><label class="switch small-switch switch-danger mb-0"><input type="checkbox" checked="" data-id="' + full.SalaryTypeId + '" onchange="ChangeStatus(this, \'' + statusSalaryTypeURL + '\' ,\'Are you sure want to change this record status?\',\'Yes\',\'No\', reloadGrid)"><span class="switch-slider"></span></label></div>';
                }
                else {
                    return '<div class="d-inline-block align-items-center justify-content-center"><label class="switch small-switch switch-danger mb-0"><input type="checkbox" data-id="' + full.SalaryTypeId + '" onchange="ChangeStatus(this, \'' + statusSalaryTypeURL + '\' ,\'Are you sure want to change this record status?\',\'Yes\',\'No\', reloadGrid)"><span class="switch-slider"></span></label></div>';
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
                html += '<button class="btn btn-sm btn-primary role-edit" data-id="' + full.SalaryTypeId + '" data-tooltip="true" title="' + title +'"><i class="fa fa-pencil-square-o"></i></button>&nbsp;';
                html += '<button class="btn btn-sm btn-primary role-delete" data-id="' + full.SalaryTypeId + '" data-tooltip="true" title="Delete"><i class="fa fa-trash"></i></button>';
                return html;
            }
        }
    ];
    $('#tblSalaryType').createGrid({
        Columns: columns,
        Mode: 'SalaryType',
        SortColumn: '1',
        IsAddShow: true,
        OnAddLabel: "Add New Salary Type",
        OnAdd: function () {
            OpenPopup('modalSalaryType', 'contentSalaryType', addSalaryTypeURL, 'frmSalaryType', null, reloadGrid);
            return false;
        },
        IsDeleteShow: true,
        OnDeleteLabel: "Delete",
        OnDelete: function () {
            DeleteGridData(this, 'tblSalaryType', deleteSalaryTypeURL, 'SalaryType', reloadGrid);
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


