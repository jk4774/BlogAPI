var loginForm = document.getElementById('loginForm');
loginForm.addEventListener('click', function () {
    console.log('test');
    var login = document.getElementById('user-login').value;
    var password = document.getElementById('password-login').value;
    var loginRequest = new XMLHttpRequest();
    loginRequest.open('POST', '/user/login', true);
    loginRequest.setRequestHeader('Content-Type', 'application/json');
    loginRequest.send(JSON.stringify({ Name: login, Password: password }));
    loginRequest.onload = () => {
        if (loginRequest.status == '404') {
            return alert('Something went wrong try again');
        }
        var token = JSON.parse(loginRequest.responseText).token;
        var id = JSON.parse(loginRequest.responseText).id;
        document.cookie = 'Authorization=Bearer ' + token + ';';
        //var authorizationRequest = new XMLHttpRequest();
        /*authorizationRequest.open('GET', '/user/' + id, true);
        authorizationRequest.setRequestHeader('Authorization', 'Bearer ' + token);
        authorizationRequest.send();*/
        window.location.href = '/user/' + id;
        console.log('test2' + token + ' ' + id);
    };
});