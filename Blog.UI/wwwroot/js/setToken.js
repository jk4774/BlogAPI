var loginForm = document.getElementById('loginForm');
loginForm.addEventListener('click', () => {
    var name = document.getElementById('username').value;
    var password = document.getElementById('password').value;
    if (!name || !password) {
        return alert('something went wrong, pass/name is empty or null');
    }
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/user/login');
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send('username=' + name + '&password=' + password);
    xhr.onload = () => {
        if (xhr.status != '200') {
            return alert('something went wrong, user/login status is not equal 200');
        }
        var id = JSON.parse(xhr.responseText).id;
        var token = JSON.parse(xhr.responseText).access_token;
        document.cookie = 'access_token=' + token + ';';
        document.location.href = '/';
    };
});
