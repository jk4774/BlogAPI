var loginForm = document.getElementById('loginForm');
loginForm.addEventListener('click', () => {
    var name = document.getElementById('username').value;
    var pass = document.getElementById('password').value;
    if (!name || !pass) {
        return alert('something went wrong pass/username is empty/null');
    }
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/user/login');
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send('username=' + name + '&password=' + pass);
    xhr.onload = () => {
        if (xhr.status != '200') {
            return alert('something went wrong');
        }
        var token = JSON.parse(xhr.responseText).access_token;
        document.cookie = 'access_token=' + token + ';';
        document.location.href = '/user/1';
    };
});
