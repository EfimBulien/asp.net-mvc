document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    form.style.opacity = "0";
    form.style.transform = "translateY(20px)";

    setTimeout(() => {
        form.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
        form.style.opacity = "1";
        form.style.transform = "translateY(0)";
    }, 0);
    
    /*
    const signUpLink = document.querySelector("div");
    signUpLink.style.opacity = "0";
    signUpLink.style.transform = "translateY(20px)";

    setTimeout(() => {
        signUpLink.style.transition = "opacity 0.3s ease-in-out, transform 0.3s ease-in-out";
        signUpLink.style.opacity = "1";
        signUpLink.style.transform = "translateY(0)";
    }, 300);*/
});
