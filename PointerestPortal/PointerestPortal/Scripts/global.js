function getToken() {
    var token = sessionStorage.getItem('tokenKey');
    if (token) {
        return token;
    }
    return null;
}

function createHeaders() {
    var token = getToken();
    if (token) {
        var headers = {};
        headers.Authorization = 'Bearer ' + token;
        return headers;
    }
}

function getQueryVariable(variable) {

    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return false;
}