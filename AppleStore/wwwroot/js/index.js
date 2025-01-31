document.addEventListener("DOMContentLoaded", function () {
    const productCards = document.querySelectorAll(".product-card");
    productCards.forEach((card, index) => {
        setTimeout(() => card.classList.add("fade-in"), index * 200);
    });
});