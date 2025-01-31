document.addEventListener("DOMContentLoaded", function () {
    const container = document.querySelector(".container");
    container.style.opacity = "0";
    container.style.transform = "translateY(20px)";

    setTimeout(() => {
        container.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
        container.style.opacity = "1";
        container.style.transform = "translateY(0)";
    }, 0);

    const image = document.querySelector(".col-md-6 img");
    image.style.opacity = "0";
    image.style.transform = "translateY(20px)";

    setTimeout(() => {
        image.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
        image.style.opacity = "1";
        image.style.transform = "translateY(0)";
    }, 100);

    const text = document.querySelector(".col-md-6 h1");
    text.style.opacity = "0";
    text.style.transform = "translateY(20px)";

    setTimeout(() => {
        text.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
        text.style.opacity = "1";
        text.style.transform = "translateY(0)";
    }, 200);

    const description = document.querySelector(".col-md-6 p");
    description.style.opacity = "0";
    description.style.transform = "translateY(20px)";

    setTimeout(() => {
        description.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
        description.style.opacity = "1";
        description.style.transform = "translateY(0)";
    }, 300);

    const price = document.querySelector(".col-md-6 h3");
    price.style.opacity = "0";
    price.style.transform = "translateY(20px)";

    setTimeout(() => {
        price.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
        price.style.opacity = "1";
        price.style.transform = "translateY(0)";
    }, 400);

    const buttons = document.querySelectorAll(".col-md-6 .btn");
    buttons.forEach((button, index) => {
        button.style.opacity = "0";
        button.style.transform = "translateY(20px)";

        setTimeout(() => {
            button.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
            button.style.opacity = "1";
            button.style.transform = "translateY(0)";
        }, 500 + index * 100);
    });
});
