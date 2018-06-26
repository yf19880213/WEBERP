$(function(){
    (function () {
        var dialog1 = $(".dialog").eq(0);
        var dialog2 = $(".dialog").eq(1);
        var $first = null;
        var $me = null;
        var $count = null;
        //数量框
        
        $("#CheckType .count").click(function (){            
            if ($(this).siblings().text().length!=0){
                $count = $(this);
                $("#count").removeClass("hide");
                $("#count input").focus();
            }          
            return false;
        });
      
        //关闭数量框
        $("#count .btn").click(function () {
            var reg = /^[1-9][0-9]*$/;
            var text = $("#count input").val().trim();
            if (reg.test(text))
                $count.text(text);
            $("#count input").val("");
            $("#count").addClass("hide");
        })
        //选择框
        $("#CheckType .Choice").click(function () {
            var parents = $(this).parents(".row");
            if (parents.index() != 0) {
                var text = parents.prev().find(".col-xs-4").eq(2).text();
                if (text.trim().length == 0) return;
            }
            $first = $(this).parents(".row");
            $.ajax({
                url: "/ChaiGou/GetAllType",
                type: "post",
                success: function (data) {
                    var html = "";
                    $.each(data, function (index, item) {
                        html += '<tr>'
                            + ' <td>' + item.Id + '</td>'
                            + ' <td>' + item.text + '</td>'
                            + '</tr>';
                    })
                    dialog1.find("tbody").html($(html));
                    dialog1.removeClass("hide");
                },
                error: function (e) {
                    console.log(e);
                }
            })
        })


        //类型弹出框 1
        dialog1.delegate("tr", "click", function () {        
            $me = $(this);
            var html = "";
            dialog1.addClass("hide");
            
            $.ajax({
                url: "/ChaiGou/GetStuffByCode",
                data: { data: $me.find("td").eq(0).text() },
                success: function (data) {
                    if (data.length > 0)
                        dialog2.removeClass("hide");
                    else
                        return;
                    $.each(data, function (index, item) {
                        html += '<tr>'
                            + ' <td>' + item.Id + '</td>'
                            + ' <td>' + item.name + '</td>'
                            + '</tr>';
                    });
                    $("#StuffName").find("tbody").html($(html));
                },
                error: function (e) {
                    alert(e);
                }
            })
        })
        //类型弹出框 2
        dialog2.delegate("tr", "click", function () {
            var onoff = true;
            var td1 = $(this).find("td").eq(0).text().trim();
            var td2 = $(this).find("td").eq(1).text().trim();
            dialog2.addClass("hide");
            if ($first.prev().length != 0 && $first.prev().find(".col-xs-4").eq(0).text().trim() == td1) return;
            $.each($first.prevAll(),function (index,item) {
                if ($(item).find(".col-xs-4").eq(0).text().trim()==td1) {
                    onoff = false;
                }
            })
            if (!onoff)return;
            $first.find(".col-xs-4").eq(0).text(td1);
            $first.find(".col-xs-4").eq(1).text(td2);
           
        });
    })();

    $("#submit").click(function () {
        var onoff = true;
        var arr = [];
        var len = $("#content .row").length - 1;
        $("#content .row").each(function (index, item) {
            if ($(item).find(".col-xs-4").text().length != 0) {
                var a = {};
                a.CailiaoId = $(item).find(".col-xs-4").eq(0).text().trim();
                a.CailiaoName = $(item).find(".col-xs-4").eq(1).text().trim();
                a.Num = $(item).find(".col-xs-4").eq(2).text();
                if (a.Num == null || a.Num.trim() == "") onoff = false;
                arr.push(a);
            }       
        })
        if (arr.length == 0 || !onoff) return;           
        $.post('/ChaiGou/GetRequest', { data: arr }, function (response) {
            if (response.success == 0) {
                $(" #alert").height(53);
                setInterval(function () {
                    location.href = '/Home/Index';
                }, 1500)
                
            }
        })
    })
    //取消
    $("#cancel").click(function () {
        $.post("/ChaiGou/ChaiGouCancel", null, function (e) {
            if (e=="success") {
                location.href = "/Home/index";
            }
        });
    })
    //最下面的边框
    var len = $("#content .row").length;
    if (len % 2 != 0) {
        $("#content .row:nth-last-child(1)").css("border-bottom", "1px solid #cccccc");
    } else
        $("#content .row:nth-last-child(1)").css("border-bottom", "1px solid #d5eef0");
})