$(document).ready(function () {
    ko.applyBindings(suburbViewModel());
});

function suburbViewModel() {
    var self = this;
    self.SearchStatus = ko.observable(true);
    self.Search = ko.observable("Please wait. Searching Regions, Districts and Suburbs...");
    self.SearchRegions = ko.observable(false);
    self.regions = ko.observableArray();
    self.selectedSuburb = ko.observable();
    self.selectedRegion = ko.observable();
    self.selectedDistrict = ko.observable();
    loadRegions();

    function loadRegions() {
        $.ajax({
            type: "GET",
            url: "/Result/NewRegions",
            success: function (returndata) {
                var obj = JSON.parse(returndata);
                self.SearchStatus(false);
                self.SearchRegions(true);
                self.regions(obj);
            }
        });
    }

    self.searchSuburb = function () {
        var reg = ko.toJSON(self.selectedRegion);
        var region = JSON.parse(reg);
        var RegionName = region.RegionName;
        var dist = ko.toJSON(self.selectedDistrict);
        var district = JSON.parse(dist);
        var DistrictName = district.DistrictName;
        var sub = ko.toJSON(self.selectedSuburb);
        var suburb = JSON.parse(sub);
        var SuburbName = suburb.SuburbName;
        //var data = JSON.stringify({ Region: RegionName, District: DistrictName, Suburb: SuburbName });
        //var parsedData = JSON.parse(data);
        localStorage.setItem("Region", RegionName);
        localStorage.setItem("District", DistrictName);
        localStorage.setItem("Suburb", SuburbName);
        
        window.location.pathname = "/Result/Index";

        //var data = ko.toJSON(self.selectedSuburb);
        //var data1 = JSON.parse(data);
        /*$.ajax({
            type: "GET",
            url: "/Result/Index?name=" + data,
            success: function () {
                window.location.replace("/Result/Index?name=" + data);
            }
        });*/
    };
}