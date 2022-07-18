var where = "1=1";
$(document).ready(function () {
    BindGrid();
    $("#tblSubCategory").on("click", ".role-edit", function () {
        OpenPopup('modalSubCategory', 'contentSubCategory', addSubCategoryURL + '/' + $(this).attr("data-id"), 'frmSubCategory', null, reloadGrid);
    });
    $("#tblSubCategory").on("click", ".role-delete", function () {
        Delete(this, deleteSubCategoryURL, 'Please confirm if you want to delete SubCategory?', 'Yes', 'No', reloadGrid);
    });
});

function reloadGrid() {
    var columns = [
        //{
        //    "title": '<div class="checkbox-fade fade-in-default"><label><input type="checkbox" id="flowcheckall" name="flowcheckall" onchange="chkAll(this,\'tblSubCategory\')" value=""><span class="cr"><i class="cr-icon icofont icofont-ui-check txt-primary"></i></span></label></div>',
        //    'searchable': false,
        //    'IsHideFromSelection': true,
        //    /*'visible': (isDeleteSubCategory == "True") ? true : false,*/
        //    'orderable': false,
        //    'className': 'text-center remove-pr tw-fixed',
        //    "render": function (data, type, full, meta) {
        //        return '<div class="checkbox-fade fade-in-default"><label><input type="checkbox" ChekBoxType ="DeleteCheckBox" onchange="chkChild(this,\'tblSubCategory\');" name="chk_' + full.SubCategoryId + '" value = "' + full.SubCategoryId + '"><span class="cr"><i class="cr-icon icofont icofont-ui-check txt-primary"></i></span></label></div>';
        //    }
        //},
        { "title": "SubCategory", "data": "Name", "sWidth": "35%" },
        { "title": "Category", "data": "Category", "sWidth": "35%" },
        { "title": "Sort Order", "data": "SortOrder" },
        {
            'title': 'Status',
            'orderable': false,
            /*'visible': (isEditSubCategory == "True") ? true : false,*/
            'className': 'text-center tw-fixed all',
            'render': function (data, type, full, meta) {
                if (full.IsActive == 1) {
                    return '<div class="d-inline-block align-items-center justify-content-center"><label class="switch small-switch switch-danger mb-0"><input type="checkbox" checked="" data-id="' + full.SubCategoryId + '" onchange="ChangeStatus(this, \'' + statusSubCategoryURL + '\' ,\'Are you sure want to change this record status?\',\'Yes\',\'No\', reloadGrid)"><span class="switch-slider"></span></label></div>';
                }
                else {
                    return '<div class="d-inline-block align-items-center justify-content-center"><label class="switch small-switch switch-danger mb-0"><input type="checkbox" data-id="' + full.SubCategoryId + '" onchange="ChangeStatus(this, \'' + statusSubCategoryURL + '\' ,\'Are you sure want to change this record status?\',\'Yes\',\'No\', reloadGrid)"><span class="switch-slider"></span></label></div>';
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
                html += '<button class="btn btn-sm btn-primary role-edit" data-id="' + full.SubCategoryId + '" data-tooltip="true" title="' + title +'"><i class="fa fa-pencil-square-o"></i></button>&nbsp;';
                html += '<button class="btn btn-sm btn-primary role-delete" data-id="' + full.SubCategoryId + '" data-tooltip="true" title="Delete"><i class="fa fa-trash"></i></button>';
                return html;
            }
        }
    ];
    $('#tblSubCategory').createGrid({
        Columns: columns,
        Mode: 'SubCategory',
        SortColumn: '1',
        IsAddShow: true,
        OnAddLabel: "Add New SubCategory",
        OnAdd: function () {
            OpenPopup('modalSubCategory', 'contentSubCategory', addSubCategoryURL, 'frmSubCategory', null, reloadGrid);
            return false;
        },
        IsDeleteShow: true,
        OnDeleteLabel: "Delete",
        OnDelete: function () {
            DeleteGridData(this, 'tblSubCategory', deleteSubCategoryURL, 'SubCategory', reloadGrid);
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


