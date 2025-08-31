console.log("Home.js loaded");

    document.addEventListener("DOMContentLoaded", function () {
    const rows = document.querySelectorAll("#postTable tbody tr");

    rows.forEach(row => {
        row.addEventListener("click", function () {
            rows.forEach(r => r.classList.remove("selected"));
            this.classList.add("selected");
            selectRow(this);
        });
    });
});

    // selectRow を JSファイル内に定義する
    function selectRow(row) {
    const cells = row.getElementsByTagName("td");

    document.getElementById("selectedPostNo").value = cells[0].innerText;
    document.getElementById("selectedTitle").value = cells[1].innerText;
    document.getElementById("selectedCategory").value = cells[2].innerText;
    document.getElementById("selectedContributor").value = cells[3].innerText;
    document.getElementById("selectedDate").value = cells[4].innerText;
    document.getElementById("selectedContent").value = cells[5].innerText;
}

    });
});
