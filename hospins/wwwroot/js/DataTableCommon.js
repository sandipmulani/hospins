var settings;
var self;
(function ($) {
    if ($.fn.DataTable.isDataTable(this)) {
        $(this).dataTable().fnDestroy();
    }
    $.fn.createGrid = function (options) {
        var defaults = { Url: dataTableUrl, PagerInfo: true,LengthChange: true, SearchParams: {}, RecordPerPage: 10, DataType: 'POST', Columns: [], Mode: '', FixClause: '', SortColumn: '0', SortOrder: 'asc', ExportIcon: true, ColumnSelection: true, IsAddShow: true, IsDeleteShow : false, IsPaging: true, OnAdd: fnAdd,OnDelete:fnDelete, IsShowFilter: false, OnAddLabel: '',OnDeleteLabel: '', FixedRightColumns: 0, FixedLeftColumns: 0, GrdLabels: JSON.stringify({ Show: "Showing", To: "to", Of: "of", Entries: "entries", Search: "Search", First: "first", Last: "last", Next: "next", Previous: "previous", SortAsc: "activate to sort column ascending", SortDesc: "activate to sort column descending", Add: "Add", ExportTo: "Export To ", Excel: "Excel", Pdf: "Pdf", Csv: "Csv", Word: "Word" }), 
        DrawCallback: fn_drawCallback, RowCallback: fn_rowCallback,CreatedRow: fn_createdRow };
        settings = $.extend({}, defaults, options);

        $.fn.DataTable.ext.pager.numbers_length = 5;

        self = this;
        var labels = JSON.parse(settings.GrdLabels);
        $(this).DataTable({
            "processing": true,
            "bStateSave": true,
            "destroy": true,
            "oLanguage": { "sProcessing": "", "sInfo": labels.Show + " _START_ " + labels.To + " _END_ " + labels.Of + " _TOTAL_ " + labels.Entries, "sLengthMenu": labels.Show + " _MENU_ " + labels.Entries, "sSearch": labels.Search, "sPaginate": { "first": labels.First, "last": labels.Last, "next": labels.Next, "previous": labels.Previous }, "sAria": { "sortAscending": ": " + labels.SortAsc, "sortDescending": ": " + labels.SortDesc } },
            "serverSide": true,
            "ajax": {
                "type": settings.DataType, "url": settings.Url,
                "data": {
                    'SearchParams': settings.SearchParams,
                    'mode': settings.Mode,
                    'FixClause': settings.FixClause,
                    '__RequestVerificationToken': GetAntiForgeryToken()
                },
                'dataType': 'json'
                //'async' : false
            },
            "columns": settings.Columns,
            "searching": settings.IsShowFilter,
            "bPaginate": settings.IsPaging, 
            "lengthChange": settings.LengthChange,
            "pageLength": settings.RecordPerPage,
            "scrollX": true,
            "scrollCollapse": true,
            // "scrollY": "300px",            
            "fixedColumns": {
                rightColumns: settings.FixedRightColumns,
                leftColumns: settings.FixedLeftColumns
            },
            // "fixedColumns": true,
            
            // "responsive": true,
            //     "columnDefs": [{
            //         responsivePriority: 0,
            //         targets: -0
            //     },
            //     {
            //         responsivePriority: 1,
            //         targets: -1
            //     },
            //     {
            //         responsivePriority: 2,
            //         targets: -2
            //     }
            // ],
            "oLanguage": {
                "oPaginate": {
                  sNext: '<i class="fa fa-chevron-right">',
                  sPrevious: '<i class="fa fa-chevron-left">'  
                }
            },
            
            "bInfo": settings.PagerInfo ? true : false,
            //lengthMenu: [[5,10,25,50,100],['5','10','25','50','100']],
            "pagingType": "simple_numbers",
            "order": [[settings.SortColumn, '' + settings.SortOrder + '']],
            //dom: 'lfrti<"toolbar">p',
            dom: '<"col-md-5 col-sm-12 normal-left padd-left-right-0 m-center datatable-drop"l><"col-md-7 col-sm-12 m-center normal-left padd-left-right-0 dataTablesEntriesInfo">rt i<"toolbar">p',
            "drawCallback": function (settings) {
                checkboxItem = [];
                $('#flowcheckall').prop('checked', false);
                $("#btnDelete").fadeOut("1000");

                if (fn_drawCallback)
                    setTimeout(function () {
                        fn_drawCallback(settings.DrawCallback);
                     //   fn_autoAdjustColumn();
                        //$(window).resize();
                    }, 300);
                    // Start check letter table td text increase add class 
                    // $("table.table-bordered tr td").each(function(){
                    //     if ( $(this).text().length > 50 ) {
                    //         $(this).addClass('line-break');
                    //         $(this).addClass('line-break-active');
                    //     }
                    //     else {
                    //         $(this).removeClass('line-break-active');
                    //     }
                    // });
                    // End check letter table td text increase add class 

            },
            "rowCallback": function (row, data) {
                settings.RowCallback();
            },
            "createdRow": function (row, data, dataIndex) {
                settings.CreatedRow(row, data, dataIndex);
            },
            initComplete: function () {
                var table = $('#' + $(this).attr("id") + '').DataTable();
                var s = '';
                var d = '';
                var s1 = '';
                if (settings.ExportIcon && table.data().count() != 0) {
                    s1 += "<iframe width='0' style='display:none;' height='0' name='exportFrame' id='exportFrame'></iframe>&nbsp;&nbsp;&nbsp;<a href='javascript:void(0)' onclick=\"Export(1,'" + $(this).attr("id") + "')\" data-tooltip=\"true\" title=\"" + labels.ExportTo + " " + labels.Excel + "\" class='btn btn-sm btn-primary'><i class='fa fa-file-excel-o'></i></a>\
                        <a href='javascript:void(0)' onclick=\"Export(4,'" + $(this).attr("id") + "')\" data-tooltip=\"true\" title=\"" + labels.ExportTo + " " + labels.Word + "\" class='btn btn-sm btn-primary'><i class='fa fa-file-word-o'></i></a>";

                    // <a onclick=\"Export(2,'" + $(this).attr("id") + "')\" data-tooltip=\"true\" title=\"" + labels.ExportTo + " " + labels.Pdf + "\" class='dt-button buttons-collection buttons-colvis btn btn-white btn-primary btn-bold'><i class='fa fa-file-pdf-o bigger-110 red'></i></a>
                }
                
                if (settings.IsDeleteShow) {
                    d += "<a href=\"javascript:void(0);\" id=\"btnDelete\" class=\"btn btn-primary btn-bg mb-xs-5 mr-2\" style=\"display:none;\"><i class=\"far fa-trash-alt text-size-12\"></i> " + settings.OnDeleteLabel + "</a>"
                }

                if (settings.IsAddShow) {
                    s += "<a href=\"javascript:void(0);\" id=\"btnAddNew\" class=\"btn btn-primary btn-bg normal-right mb-xs-5\" ><i class=\"fa fa-plus text-size-12\"></i> " + settings.OnAddLabel + "</a>"
                }
                
                if (settings.ColumnSelection) {
                    if (settings.ExportIcon && table.data().count() != 0) {
                        s1 += '&nbsp;<div class="dropup" style="display: inline-block;" ><a href=\"javascript:void(0)"\ data-tooltip=\"true\" title="Columns" id="dLabel1" class="btn btn-sm btn-primary" data-uib-tooltip="Columns" ><i class="fa fa-columns"></i></a>';
                        s1 += '<ul class="dropdown-menu column-selection" aria-labelledby="dLabel1">';
                        for (var i = 0; i < table.columns().count(); i++) {
                            var c = table.column(i).visible() == false ? "" : "checked";
                            if (!($(table.column(i).header()).text().match("Id")) && table.columns().column(i).context[0].aoColumns[i].IsHideFromSelection != true) {
                                s1 += "<li class=\"mt-checkbox-list\" style=\"display:flex;\"><div class=\"checkbox-fade fade-in-default\"><label><input id=\"chk_select_column_" + i + "\" type=\"checkbox\" " + c + " onchange=\"ShowHideColumn(this,'" + $(this).attr("id") + "');\" coldata='" + $(table.column(i).header()).text() + "' /><span class=\"cr checkmark mright-5\"><i class=\"cr-icon icofont icofont-ui-check txt-primary\"></i></span><span class=\"checkbox-content\">" + $(table.column(i).header()).text() + "</span></label></div></li>";
                            }
                        }
                        s1 += "</ul></div>";
                    }
                }
                if(~~table.data().count() != 0) {
                    s1 += '&nbsp;<a href="javascript:void(0)" onclick=\"fn_hard_reload_grid(\'' + $(self).attr("id") +'\')\" data-tooltip="true" title="" class="btn btn-sm btn-primary" data-original-title="Reset"><i class="fa fa-undo"></i></a>';
                }

                $("#" + $(this).attr("id") + "").parent().parent().parent().find("div.toolbar").html(s1);
                $("#" + $(this).attr("id") + "").parent().parent().parent().find("div.dataTables_length").prepend(d);
                $("#" + $(this).attr("id") + "").parent().parent().parent().find(".dataTablesEntriesInfo").append(s);
                
                
                $("#dLabel1").on("click", function (event) {
                    event.stopPropagation();
                    $(".column-selection").slideToggle("fast");
                    $(this).tooltip('hide');
                });

                $(".column-selection").on("click", function (event) {
                    event.stopPropagation();
                });

                $(document).on("click", function () {
                    $(".column-selection").hide();
                });

                // $('[data-tooltip="true"]').tooltip({
                //     container: 'body',
                //     placement: 'top'
                // });
                $("#btnAddNew").on("click", function () {
                    debugger
                    $(this).tooltip('hide');
                    settings.OnAdd();
                });
                
                $("#btnDelete").on("click",function(){
                    settings.OnDelete();
                });
                
                if($("#dLabel1").length>0)
                {
                    $(".mt-checkbox-list .checkmark").on("click",function(e){
                        e.stopPropagation();
                    });
                }

                $("#" + $(this).attr("id") + " tr th").each(function (e, index) {
                    $("#" + $(self).attr("id") + " tr td:nth-child(" + (e + 1) + ")").attr('title', $(this).text());;
                });
                if (settings.DrawCallback) {
                    settings.DrawCallback(settings);
                }
                
                //added by sandip 03-03-2020 for feching issue after storing table history
                $("#" + $(this).attr("id") + "").on('init.dt', function (evt, settings) { 
                    if (settings && settings.fnRecordsTotal() == 0) {     
                        console.log("fire");          
                        setTimeout(function(){
                            self.dataTable().fnPageChange(0);
                        },500);
                    }
                });
            }
        });
        
        $('div.dataTables_filter').addClass('xs-mb-5');
        $('div.dataTables_filter input').addClass('form-control input-sm input-small input-inline');
        $('div.dataTables_length select').addClass('form-control input-sm input-xsmall input-inline');
        $("#" + $(this).attr("id") + "").on('order.dt', function () {
            if (settings.DrawCallback) {
                setTimeout(function () {
                    settings.DrawCallback(settings);
                }, 300);
            }
        });
        
        $("#" + $(this).attr("id") + "").on('page.dt', function () {
            //for uncheck checkall btn when page change
            $('#flowcheckall').prop('checked',false);
            if (settings.DrawCallback) {
                setTimeout(function () {
                    settings.DrawCallback(settings);
                }, 300);
            }
        });
        $("#" + $(this).attr("id") + "").on('search.dt', function () {
            if (settings.DrawCallback) {
                setTimeout(function () {
                    settings.DrawCallback(settings);
                }, 300);
            }
        });
    }
}(jQuery));

function HideLoading() {
    $(".dataTables_processing").hide();
}

function fn_drawCallback() {
    // console.log("innerdiv");
    // $("#tbl tr td").wrapInner("<div class='div-wrapper'></div>");
    // fn_autoAdjustColumn();
}

function fn_rowCallback() {

}

function fn_createdRow() {

}

function fn_autoAdjustColumn(){
    //$($.fn.dataTable.tables(true)).DataTable().columns.adjust();
    //$($.fn.dataTable.tables(true)).DataTable().fixedColumns().relayout();
    
    $('[data-tooltip="true"]').tooltip({
        container: 'body',
        placement: 'auto'
    });
}

function fn_hard_reload_grid(gid){
    localStorage.removeItem('DataTables_'+ gid +"_"+window.location.pathname);
    window.location.reload();
}

function ShowHideColumn(obj, tbl) {
    var table = $('#' + tbl.trim() + '').DataTable();
    for (var i = 0; i < table.columns().count(); i++) {
        if ($(table.column(i).header()).text() == $(obj).attr("coldata")) {
            table.column(i).visible(obj.checked)
        }
    }
    fn_autoAdjustColumn();
}


function fnAdd() {

}

function fnDelete(){

}

function Export(obj, tbl) {
    var table = $('#' + tbl.trim() + '').DataTable();
    var type = 'excel';
    if (obj === 2)
        type = 'pdf';
    else if (obj === 4)
        type = 'word';
    var winName = 'MyWindow';
    var winURL = dataTableExportUrl;
    var params1 = jQuery('#' + tbl + '').DataTable().ajax.params();
    var params = { 'SearchParams': params1.SearchParams, 'search[value]': params1.search.value, 'order[0][column]': params1.order[0].column, 'order[0][dir]': params1.order[0].dir, 'start': "-2", 'mode': params1.mode, 'FixClause': params1.FixClause, 'type': type, 'columns': params1.columns, '__RequestVerificationToken': GetAntiForgeryToken() };
    
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", winURL);
    form.setAttribute("target", winName);

    var filter = Array();
    filter.push(params1.SearchParams[0]);
    input = document.createElement('input');
    input.type = 'hidden';
    input.name = 'filters';
    input.value = JSON.stringify(params1.SearchParams);
    form.appendChild(input);
    
    for (var i in params) {
        if (params.hasOwnProperty(i)) {
            var input;
            if (i === "columns") {
                input = document.createElement('input');
                input.type = 'hidden';
                input.name = 'Columns';
                var cols = "";
                for (c = 0; c < table.columns().count(); c++) {
                    if (table.column(c).visible() != false && isNaN(table.column(c).dataSrc()))
                        cols += cols == "" ? table.column(c).dataSrc() + " [" + $(table.column(c).header()).text() + "]" : "," + table.column(c).dataSrc() + " [" + $(table.column(c).header()).text() + "]";
                    //if (table.column(c).visible() != false && isNaN($(table.column(c).header()).text()))
                    //    cols += cols == "" ? $(table.column(c).header()).text() : "," + $(table.column(c).header()).text();
                }
                input.value = cols;
                form.appendChild(input);
            }
            else {

                if (typeof params[i] == "object") {
                    for (var j in params[i]) {
                        input = document.createElement('input');
                        input.type = 'hidden';
                        input.name = "SearchParams[" + j + "]";
                        input.value = JSON.stringify(params[i][j]);
                        form.appendChild(input);
                    }
                }
                else {
                    input = document.createElement('input');
                    input.type = 'hidden';
                    input.name = i;
                    //input.value = typeof params[i] == "object" ? JSON.stringify(params[i]) : params[i];
                    input.value = params[i];
                    form.appendChild(input);
                }
            }
        }
    }
    
    document.body.appendChild(form);
    form.target = "exportFrame";
    form.submit();
    document.body.removeChild(form);
}

// CSRF (XSRF) security
function GetAntiForgeryToken() {
    var tokenInput = $('input[name=__RequestVerificationToken]');
    if (tokenInput.length) {
        return tokenInput.val();
    }
    return '';
};
$(document).ready(function(){
    $(window).resize(function(){
         fn_autoAdjustColumn();
    });
});
