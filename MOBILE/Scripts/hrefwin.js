//禁止用户回退
$(document).ready(function () {
    if (window.history && window.history.pushState) {
        $(window).on('popstate', function () {
            window.history.pushState('forward', null, '#');
            window.history.forward(1);
        });
    }
    window.history.pushState('forward', null, '#'); //在IE中必须得有这两行
    window.history.forward(1);
});
//点击切换
$(function () {
    $("#wrapBox").on("click", ".row .radioBtn", function () {        
        if ($(this).index() == 1) {
            var $parents = $(this).parents(".bigmu");
            var id = $parents.find(".danhao").text().trim();
            var text = "通过";
            $(this).parents(".row").next().find("input").val("通过");                    
              $parents.stop().hide(1000);
            Grant(id,text);
            $(this).find("i").css("background-image", "url('../images/radio_select.png')");
            $(this).siblings().find("i").css("background-image", "url('../images/radio_normal.png')");
          
        } else if ($(this).index() == 2) {
            var bgurl = $(this).find("i").css("backgroundImage").split("\"")[1];
            if (bgurl.indexOf('select') != -1) {
                $(this).parents(".bigmu").stop().hide(1000);
                var $parent = $(this).parents(".bigmu");
                var Id = $parent.find(".danhao").text();
                var text = $(this).val().trim();
                UnGrant(Id, text);
            } else {
                $(this).find("i").css("background-image", "url('../images/radio_select.png')");
                $(this).siblings().find("i").css("background-image", "url('../images/radio_normal.png')");
                $(this).parents(".row").next().find("input").focus();
            }
           

        }
    })
    function Grant(id,text) {
        $.post("/Examine/PassById", {Id:id,text: text }, function (context) {
            if (context == 'success') {
                Emerage();
            }
        })
    };
    function UnGrant(id,text) {
        $.post("/Examine/UnPassById", { id:id,text: text }, function (context) {
            if (context == 'success') {
                Emerage();
            }
        })
    };
    function Emerage() {
        $.post("/ChaiGou/GetLoadingCountEximine", null, function (data) {
            count = data;
            $(".loading").text(data);
            console.clear();
        })
        $.post("/ChaiGou/GetFinishCount", null, function (data) {
            $(".finishing").text(data);
        })
        $("#granted").html("");
    }

    $(".conclusion").click(function () {
        if ($(this).text().indexOf(0) != -1) return;
        $.post("/Examine/Conclusion", null, function (e) {
            if (e == null) return;
            var html = '<div class="table-responsive">'
                +' <table class="table table-bordered"> '
                    +'  <thead> '
                +' <tr> '
                +' <th> 名称</th> '
                        +'<th> 结果</th> '
                        +' </tr> '
                            +'</thead> '
                    +'<tbody>';
            $.each(e, function (index, item) {
                var str = item.auditoring == 1 ? "通过" : "没通过";
                html += '<tr>'
                    + '<td> ' + item.masterId+'</td> '
                    + '<td>' + str+'</td>'
                            +'</tr>';
            })
            html += " </tbody> </table></div>";
            $("#granted").html(html);
        })
    })






















    Date.prototype.toLocaleString = function () {
        return this.getFullYear() + "/" + (this.getMonth() + 1) + "/" + this.getDate()  ;
    };

});

