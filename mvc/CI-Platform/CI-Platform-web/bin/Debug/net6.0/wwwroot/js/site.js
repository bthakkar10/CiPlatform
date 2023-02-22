// Hide Navbar on scroll down
//var prevScrollpos = window.pageYOffset;
//window.onscroll = function () {
//    var currentScrollPos = window.pageYOffset;
//    if (prevScrollpos > currentScrollPos) {
//        document.getElementById('navbar').style.top = '0';
//    } else {
//        document.getElementById('navbar').style.top = '-80px'; /* width horizontal navbar */
//    }
//    prevScrollpos = currentScrollPos;

//}

//const mainNavigation = document.querySelector('.main-navigation');
//const overlay = mainNavigation.querySelector('.overlay');
//const toggler = mainNavigation.querySelector('.top-nav-toggle');

//const openSideNav = () => mainNavigation.classList.add('active');
//const closeSideNav = () => mainNavigation.classList.remove('active');
//toggler.addEventListener('click', openSideNav);
//overlay.addEventListener('click', closeSideNav);

//var clear = document.getElementById('clear-all-filter').setAttribute('style', 'display:none');


//document.getElementById('navbarTogglerDemo02").onclick = function () {
//    document.getElementById("navbarTogglerDemo02").style.display = "none";
//}


var closebtns = document.getElementById("clear-all-filter");
    var i;

    for (i = 0; i < closebtns.length; i++) {
        closebtns[i].addeventlistener("click", function () {
            this.parentelement.style.display = 'none';
        });
}

    function show_grid(){
    document.getElementById("grid-view").style.display = "block";
    document.getElementById("list-view").style.display = "none";
}
    function show_list(){
        document.getElementById("list-view").style.display = "block";
    document.getElementById("grid-view").style.display = "none";
}
    function for_grid(){
        document.getElementById("grid-view").style.display = "block";
    document.getElementById("list-view").style.display = "none";
}





