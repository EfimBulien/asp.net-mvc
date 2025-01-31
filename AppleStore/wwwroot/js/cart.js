document.addEventListener("DOMContentLoaded", function () {
    const tableRows = document.querySelectorAll(".table tbody tr");

    tableRows.forEach((row, index) => {
        setTimeout(() => {
            row.classList.add("fade-in");
        }, index * 200);
    });

    const tableContainer = document.querySelector(".table-responsive");
    setTimeout(() => {
        tableContainer.classList.add("fade-in");
    }, 0);

    const total = document.querySelector(".total");
    setTimeout(() => {
        total.classList.add("fade-in");
    }, 0);

    const actions = document.querySelector(".actions");
    setTimeout(() => {
        actions.classList.add("fade-in");
    }, 0);

    const alertMessage = document.querySelector(".alert");
    if (alertMessage) {
        setTimeout(() => {
            alertMessage.classList.add("fade-in");
        }, 0);
    }
});