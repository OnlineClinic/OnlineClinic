
function fillState(State, city, Country) {
    $('#' + State).empty();
    $('#' + city).empty();
    $.ajax({
        type: 'GET',
        url: '/Home/Getstate',
        dataType: 'json',
        data: { countyid: Country },

        success: function (data) {
            $('#' + State).append($("<option></option>").val(0).html('Select State'));
            $('#' + city).append($("<option></option>").val(0).html('Select City'));
            $.each(data, function (key, value) {

                $('#' + State).append($("<option></option>").val(value.Id).html(value.StateName));
            });

        },
        error: function (ex) {
        }
    });

}
function fillCity(City, state) {
    $('#' + City).empty();
    $.ajax({
        type: 'GET',
        url: '/Home/GetCity',
        dataType: 'json',
        data: { StateId: state },
        success: function (data) {
            $('#' + City).append($("<option></option>").val(0).html('Select City'));
            $.each(data, function (key, value) {
                $('#' + City).append($("<option></option>").val(value.LookUpCityId).html(value.LookUpCityName));
            });

        },
        error: function (ex) {
        }
    });

}
