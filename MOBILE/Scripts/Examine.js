$.post("/Examine/GetAllUnExamine", null, function (data) {

    Date.prototype.toLocaleString = function () {
        return this.getFullYear() + "/" + (this.getMonth() + 1) + "/" + this.getDate() ;
    };
    var html = "";
    $.each(data, function (index, item) {
        var date =Number( item.caigouLate.split("(")[1].split(")")[0]);
        date = new Date(date).toLocaleString();
        var response = item.response == null ? "" : item.response;
        html += '<div class="bigmu">'
            + '<div class="row">'
            + '<div class="col-xs-3  textleft">申请单号</div><div class="col-xs-9 danhao textleft">' + item.masterId + '</div>'
            + '</div>'
            + ' <div class="row">'
            + ' <div class="col-xs-3  textleft">申请人</div>'
            + '<div class="col-xs-3 applyperson textleft">' + item.name + '</div>'
            + '<div class="col-xs-3 ">申请日期</div>'
            + ' <div class="col-xs-3 date textleft">' + date + '</div>'
            + ' </div>'
            //+ ' <div class="row">'
            //+ ' <div class="col-xs-3 ">申请部门</div>'
            //+ ' <div class="col-xs-3 department textleft">' + item.department + '</div>'
            //+ ' <div class="col-xs-3">负责人</div>'
            //+ ' <div class="col-xs-3 response textleft">' + response + '</div>'
            //+ ' </div>'
            + ' <div class="table-responsive">'
            + '<table class="table table-bordered table-hover">'
            + '<thead><tr><th>名称</th><th>数量</th></tr></thead>'
            + '<tbody></tbody>'
            + '</table>'
            + '</div> '
            + '<div class="row  examine">'
            + ' <div class="col-xs-3 ">审核</div>'
            + '<div class="col-xs-3 radioBtn"><i></i><big>通过</big></div>'
            + '<div class="col-xs-3 radioBtn"><i></i><big>不通过</big></div>'
            + '</div> '
            + '<div class="row argue">'
            + '<div class="col-xs-3 ">审核意见</div>'
            + ' <div class="col-xs-9"><input class="form-control"/></div>'
            + '</div>'
            + '</div>';
        console.clear();
    })
    $("#wrapBox").html(html);
    $(".bigmu").each(function (index,element) {       
        GetOption(element);
    })
})
function GetOption(element) {
    var text = $(element).find(".danhao").text().trim();
    console.log(text);
    $.post("/Examine/ExaminRecordDetail", { masterId: text }, function (data) {
        var option = "";
        $.each(data, function (index,item) {
            option += "<tr><td>" + item.cailiaoName + "</td><td>" + item.num+"</td></tr>";
        })
        $(element).find("tbody").html(option);
      
    })

   







}
