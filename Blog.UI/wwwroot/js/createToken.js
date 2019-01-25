var loginForm = document.getElementById('loginForm');
loginForm.addEventListener('click', () => {
    var login = document.getElementById('user-login').value;
    var password = document.getElementById('password-login').value;
    var loginRequest = new XMLHttpRequest();
    loginRequest.open('POST', '/user/login', true);
    loginRequest.setRequestHeader('Content-Type', 'application/json');
    loginRequest.send(JSON.stringify({ Name: login, Password: password }));
    loginRequest.onload = () => {
        if (loginRequest.status != '200') {
            return alert('Something went wrong try again');
        }
        var json = JSON.parse(loginRequest.responseText);
        window.localStorage.clear();
        window.localStorage.setItem('Token', json.token);
        window.localStorage.setItem('Id', json.id);
        // ???
        var token = window.localStorage.getItem('Token');
        var id = window.localStorage.getItem('Id');
        var xhr = new XMLHttpRequest();
        xhr.open('GET', '/user/' + id, true);
        xhr.setRequestHeader('Authorization', 'Bearer ' + token);
        xhr.send();

    };
});

