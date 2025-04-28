let sidebar = document.querySelector(".sidebar");
let sidebarBtn = document.querySelector(".sidebarBtn");
let homeSection = document.querySelector(".home-section");
let productTable = document.querySelector('.table-section');

sidebarBtn.onclick = function () {
    sidebar.classList.toggle("active");
    homeSection.classList.toggle("sidebar-coolapsed");
    productTable.classList.toggle("table-active");

};
