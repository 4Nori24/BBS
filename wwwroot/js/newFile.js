<script>
    console.log("home.js loaded");

    <script>
        document.addEventListener("DOMContentLoaded", function () {}
        const rows = document.querySelectorAll("#postTable tbody tr");

        rows.forEach(row => {row.addEventListener("click", function() {
            rows.forEach(r => r.classList.remove("selected"));
            this.classList.add("selected");
            selectRow(this);
        })};
        });
    });
    </script>
</script>;
