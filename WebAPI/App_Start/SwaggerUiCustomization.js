// ReSharper disable PossiblyUnassignedProperty
$("#explore").off();

$("#explore").click(function () {
    var cred = $("#input_apiKey")[0].value;
    var credentials = cred.split(":"); //username:password expected
    //  var baseUrl = $("#api_info > div.info_license > a").attr('href') + '/connect/';
    var tokenUrl = "http://localhost/WebAPICrypto/api/user?username=teste&password=123&api_key=teste%3A123";
    var clientId = "ro.device";
    var clientSecret = "amico-account";
    var xhr = new XMLHttpRequest();
    xhr.onload = function (e) {
        var responseData = JSON.parse(xhr.response);
        var bearerToken = "Bearer " + responseData.access_token;

        window.swaggerUi.api.clientAuthorizations.add('Authorization', new SwaggerClient.ApiKeyAuthorization("Authorization", bearerToken, "header"));

        window.swaggerUi.api.clientAuthorizations.remove("api_key");
        console.log(responseData);
        alert("Something happenned");
    }
    xhr.open("POST", tokenUrl);
    var data = {
        username: credentials[0],
        password: credentials[1],
        grant_type: "password",
        scope: "amico-chat-api amico-account-api"
    };
    var body = "";
    for (var key in data) {
        if (body.length) {
            body += "&";
        }
        body += key + "=";
        body += encodeURIComponent(data[key]);
    }
    xhr.setRequestHeader("Authorization", "Basic " + btoa(clientId + ":" + clientSecret));
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xhr.send(body);
});
// ReSharper restore PossiblyUnassignedProperty