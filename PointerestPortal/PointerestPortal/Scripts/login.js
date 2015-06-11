$(function() {

    var gestore;

    $('#login-form-link').click(function (e) {
        $("#login-form").delay(100).fadeIn(100);
        $("#register-form").fadeOut(100);
        $('#register-form-link').removeClass('active');
        $(this).addClass('active');
        e.preventDefault();
    });
    $('#register-form-link').click(function (e) {
        $("#register-form").delay(100).fadeIn(100);
        $("#login-form").fadeOut(100);
        $('#login-form-link').removeClass('active');
        $(this).addClass('active');
        e.preventDefault();
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

                alert("registered gestore");

                gestore = {
                    "Username": email,
                    "Password": password,
                    "Nome": $('#nome').val(),
                    "Cognome": $('#cognome').val()
                };

                $.post("api/gestore", gestore, function() {

                    alert("posted gestore");
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
            //self.user(data.userName);
            alert('Login ok');
            sessionStorage.setItem("tokenKey", data.access_token);
            window.location.href = "/manage?username=" + loginData.username;
        }).fail(function () {
            alert('Login error');
        });
    });
});

