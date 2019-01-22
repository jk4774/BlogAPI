if (window.localStorage.getItem('Token') === null) {
    return alert('You are not logged');
}
var id = '1';
var authorizationRequest = new XMLHttpRequest();
authorizationRequest.open('GET', '/user/' + id, true);
authorizationRequest.setRequestHeader('Authorization', window.localStorage.getItem('token'));
authorizationRequest.send();
window.location.href = '/user/' + id;
console.log('asdf');
