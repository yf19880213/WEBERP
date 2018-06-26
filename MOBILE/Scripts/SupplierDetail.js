$.post("/Supplier/GetDetail", null, function (e) {
    var html = "";
    $.each(e, function (index,item) {
        html += "<tr><td>" + item.CailiaoName + "</td><td>" + item.Num+"</td><td></td></tr>";
    })
    $("#mainTable tbody").html(html);
})
$("#mainTable tbody").on("click", "tr", function () {
    $(".dialog1 tbody").html("");
    var text = $(this).find("td").eq(0).text().trim();
    GetData("/Supplier/GetUnitByNameFirst", text);
    GetData("/Supplier/GetUnitByNameFirstOther", text);
    if (!$(".dialog1").hasClass("hide")) return;
    if (!isNaN(Number(text.substring(0, 1)))) {
        text = text.substring(text.length - 2, text.length);
    } else
        text = text.substring(0,2);
    GetData("/Supplier/GetUnitByName",text);
    GetData("/Supplier/GetUnitByNameOther", text);
    if (!$(".dialog1").hasClass("hide")) return;
        text = text.substring(0, 1);
     GetData("/Supplier/GetUnitByName", text);
     GetData("/Supplier/GetUnitByNameOther", text);
})

function GetData(url, text) {
    $.ajax({
        type: "post",
        url: url,
        data: {name:text},
        async: false,
        success: function (data) {
            if (data != null && data.length > 0) {
                var html = "";
                $.each(data, function (index, item) {
                    var totalPrice = item.金额;
                    var num = item.数量 == 0 ? 1 : item.数量;
                    if (totalPrice == 0) return false;
                    var unit = (totalPrice / num).toFixed(2);
                    html += "<tr><td>" + item.ID + "</td><td>" + item.材料名称 + "</td><td>" + unit + "</td></tr>"
                })
                if (html != "") {
                    $(".dialog1 tbody").prepend($(html));
                    $(".dialog1").removeClass("hide");
                }
            }
        }
    });
}

$(".dialog1").on("click", "tbody td,.btn", function () {
    $(".dialog1").addClass("hide");
})
