$.post("/Supplier/GetAllDanhao", null, function (data) {
    var html = "";
    $.each(data, function (index,item) {
        html += "<tr><td>" + item.danhao + "</td><td>" + item.name+"</td></tr>";
    })
    $("#mainTable tbody").html(html);

})
$("tbody").on("click", "tr", function () {
    var text = $(this).find("td").eq(0).text().trim();
    location.href = "/Supplier/SupplierDetail?masterId=" + text;
})