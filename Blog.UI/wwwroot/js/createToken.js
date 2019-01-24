var loginForm = document.getElementById('loginForm');
loginForm.addEventListener('click', () => {
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
        var json = JSON.parse(loginRequest.responseText);
        window.localStorage.setItem('Authorization', 'Bearer ' + json.token);
        window.localStorage.setItem('Id', json.id);
    };
});

