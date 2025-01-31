document.addEventListener("DOMContentLoaded", function () {
    const productCards = document.querySelectorAll(".product-card");
    productCards.forEach((card, index) => {
        setTimeout(() => card.classList.add("fade-in"), index * 200);
    });

    const searchForm = document.querySelector(".glass-search");
    setTimeout(() => searchForm.classList.add("fade-in"), 0);
});