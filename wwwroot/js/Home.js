console.log("Home.js loaded");

function toggleReplyCard(id) {
    const reply = document.getElementById(id);
    reply.classList.toggle("show");
}

document.addEventListener("DOMContentLoaded", function () {
    const cards = document.querySelectorAll(".post-card");

    cards.forEach(card => {
        card.addEventListener("click", () => handleCardSelect(card));
        card.addEventListener("touchstart", () => handleCardSelect(card));
    });

    function handleCardSelect(card) {
        cards.forEach(c => c.classList.remove("selected"));
        card.classList.add("selected");
        selectCard(card);
    }

    function selectCard(card) {
        // Razor側で data-* 属性を埋め込んでおくと安全
        document.getElementById("selectedPostNo").value = card.dataset.postno || "";
        document.getElementById("selectedTitle").value = card.dataset.title || "";
        document.getElementById("selectedCategory").value = card.dataset.category || "";
        document.getElementById("selectedContributor").value = card.dataset.contributor || "";
        document.getElementById("selectedDate").value = card.dataset.date || "";
        document.getElementById("selectedContent").value = card.dataset.content || "";
    }

    const btnSearch = document.getElementById("btn-search");
    const searchCondition = document.getElementById("search-condition");

    console.log("btnSearch:", btnSearch);
    console.log("searchCondition:", searchCondition);



    if (btnSearch && searchCondition) {
        btnSearch.addEventListener("click", function () {
            searchCondition.classList.toggle("show");
        });
    }
});

