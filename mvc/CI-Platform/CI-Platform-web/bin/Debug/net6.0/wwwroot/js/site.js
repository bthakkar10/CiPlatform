

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





