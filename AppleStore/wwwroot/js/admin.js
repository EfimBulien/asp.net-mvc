document.addEventListener("DOMContentLoaded", function () {
    const card = document.querySelector(".card");
    card.style.opacity = "0";
    card.style.transform = "translateY(20px)";

    setTimeout(() => {
        card.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
        card.style.opacity = "1";
        card.style.transform = "translateY(0)";
    }, 0);

    const links = document.querySelectorAll(".list-group-item");
    links.forEach((link, index) => {
        link.style.opacity = "0";
        link.style.transform = "translateY(20px)";

        setTimeout(() => {
            link.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
            link.style.opacity = "1";
            link.style.transform = "translateY(0)";
        }, 100 + index * 100);
    });

    const footerButton = document.querySelector(".card-footer");
    footerButton.style.opacity = "0";
    footerButton.style.transform = "translateY(20px)";

    setTimeout(() => {
        footerButton.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
        footerButton.style.opacity = "1";
        footerButton.style.transform = "translateY(0)";
    }, 800);
});
