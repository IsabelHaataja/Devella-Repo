/* NavMenu */
function setupNavbarScrollHandler() {
    const navbar = document.getElementById("mainNavbar");
    if (!navbar) {
        console.error("Navbar element not found.");
        return;
    }

    let lastScrollTop = 0;

    window.addEventListener("scroll", function () {
        const scrollTop = window.scrollY;

        if (scrollTop > lastScrollTop) {
            navbar.classList.add("navbar-hidden");
        } else {
            navbar.classList.remove("navbar-hidden");
        }

        lastScrollTop = scrollTop;
    });
}

window.getSelectedOptions = function (selectElement) {
    return Array.from(selectElement.selectedOptions).map(option => option.value);
}
