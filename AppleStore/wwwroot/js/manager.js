document.addEventListener("DOMContentLoaded", function () {
    const jumbotron = document.querySelector(".jumbotron");
    jumbotron.style.opacity = "0";
    jumbotron.style.transform = "translateY(20px)";

    setTimeout(() => {
        jumbotron.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
        jumbotron.style.opacity = "1";
        jumbotron.style.transform = "translateY(0)";
    }, 0);

    const charts = document.querySelectorAll(".row canvas");
    charts.forEach((chart, index) => {
        chart.style.opacity = "0";
        chart.style.transform = "translateY(20px)";

        setTimeout(() => {
            chart.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
            chart.style.opacity = "1";
            chart.style.transform = "translateY(0)";
        }, 300 + index * 100);
    });

    const exportButtons = document.querySelectorAll(".export-buttons .btn");
    exportButtons.forEach((button, index) => {
        button.style.opacity = "0";
        button.style.transform = "translateY(20px)";

        setTimeout(() => {
            button.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
            button.style.opacity = "1";
            button.style.transform = "translateY(0)";
        }, 600 + index * 100);
    });
});
