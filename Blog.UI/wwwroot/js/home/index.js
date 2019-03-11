document.getElementById('loginForm').addEventListener('click', () => {
    let name = document.getElementById('login-username').value;
    let password = document.getElementById('login-password').value;

    if (!name || !password) 
        return alert('something went wrong, pass/name is empty or null');

    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/user/login');
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.onload = () => {
        if (xhr.status != '200') 
            return alert('something went wrong, user/login status is not equal 200');
            
        var token = JSON.parse(xhr.responseText).access_token;
        document.cookie = 'access_token=' + token + ';';
        document.location.href = '/';
    };
    xhr.send('username=' + name + '&password=' + password);
});

document.getElementById('registerForm').addEventListener('click', () => {
    let name = document.getElementById('register-username').value;
    let password = document.getElementById('register-password').value;
    let email = document.getElementById('register-email').value;

    if (!name || !password || !email)
        return alert('name/pass/email cannot be null');

    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/user/register');
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.onload = () => {
        if (xhr.status != '200') 
            return alert('something went wrong, cannot register an user');
        return alert('correctly register');    
    };
    xhr.send('username=' + name + '&password=' + password + '&email=' + email);
});