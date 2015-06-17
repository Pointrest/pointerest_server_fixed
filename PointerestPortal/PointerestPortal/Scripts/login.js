$(function() {

    var gestore;

    $('#login-form-link').click(function (e) {
        
        showLoginForm($(this),e);
    });
    $('#register-form-link').click(function (e) {
        
        showRegisterForm($(this),e);
    });

    // handle register form
    $('#register-submit').on('click', function (e) {

        e.preventDefault()

        var password = $('#registerPassword').val();
        var confirmPassword = $('#confirmPassword').val();
        var email = $('#email').val();

        if (password == confirmPassword) {

            var registerModel = {
                "Email": email,
                "Password": password,
                "ConfirmPassword": confirmPassword
            };

            $.post("api/Account/register", registerModel, function () {

                gestore = {
                    "Username": email,
                    "Password": password,
                    "Nome": $('#nome').val(),
                    "Cognome": $('#cognome').val()
                };

                $.post("api/gestore", gestore, function () {
                    $('.form-group > input').val('');
                    showLoginForm(null, null);
                });
            }); 
        }  
    });

    // handle login form
    $('#login-submit').on('click', function (e) {

        e.preventDefault();

        var loginData = {
            'grant_type': 'password',
            'username': $('#username').val(),
            'password': $('#password').val()
        };

        $.ajax({
            type: 'POST',
            url: '/Token',
            data: loginData
        }).done(function (data) {
            sessionStorage.setItem("tokenKey", data.access_token);
            window.location.href = "/manage?username=" + loginData.username;
        }).fail(function () {
            alert('Login error');
        });
    });
});

function showLoginForm(obj,e) {

    $("#login-form").delay(100).fadeIn(100);
    $("#register-form").fadeOut(100);
    $('#register-form-link').removeClass('active');

    if (obj == null) {
        $('#login-form-link').addClass('active');
    } else {
        $(obj).addClass('active');
    }

    if(e != null)
        e.preventDefault();
}

function showRegisterForm(obj, e) {

    $("#register-form").delay(100).fadeIn(100);
    $("#login-form").fadeOut(100);
    $('#login-form-link').removeClass('active');
    $(obj).addClass('active');
    e.preventDefault();
}
