$(document).ready(function () {
    document.getElementById("wrap").addEventListener("scroll", function () {
        var translate = "translate(0," + this.scrollTop + "px)";
        this.querySelector("thead").style.transform = translate;
    });
    ko.applyBindings(suburbViewModel());
    
});

function suburbViewModel() {
    var self = this;
    self.SearchStatus = ko.observable(true);
    self.Search = ko.observable("Please wait. Searching Open homes...");
    self.home = ko.observable(getHomes());
    function getHomes() {
        var data1 = localStorage.getItem('Region');
        var data2 = localStorage.getItem('District');
        var data3 = localStorage.getItem('Suburb');
        var isSuburb = data3.includes("(0)"); // If suburb contains "0" then no need to invoke search open homes

        //var data2 = Request["Region"]; //Query string
        if (!isSuburb) {
            $.ajax({
                type: "GET",
                url: "/Result/GetHomeDetails?region=" + data1 + "&district=" + data2 + "&suburb=" + data3,
                success: function (returndata) {
                    self.SearchStatus(false);
                    self.home(returndata);
                }
            });
        }
        else {
            self.Search = "No Open homes found.";
        }
        
    }
}