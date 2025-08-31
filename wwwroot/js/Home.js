console.log("Home.js loaded");

document.addEventListener("DOMContentLoaded", function () {
    const rows = document.querySelectorAll("#postTable tbody tr");

    rows.forEach(row => {
        // PC対応
        row.addEventListener("click", () => handleRowSelect(row));
        // スマホ対応
        row.addEventListener("touchstart", () => handleRowSelect(row));
    });

    function handleRowSelect(row) {
        rows.forEach(r => r.classList.remove("selected"));
        row.classList.add("selected");
        selectRow(row);
    }

    function selectRow(row) {
        const cells = row.getElementsByTagName("td");

        document.getElementById("selectedPostNo").value = cells[0].textContent.trim();
        document.getElementById("selectedTitle").value = cells[1].textContent.trim();
        document.getElementById("selectedCategory").value = cells[2].textContent.trim();
        document.getElementById("selectedContributor").value = cells[3].textContent.trim();
        document.getElementById("selectedDate").value = cells[4].textContent.trim();
        document.getElementById("selectedContent").value = cells[5].textContent.trim();
    }
});