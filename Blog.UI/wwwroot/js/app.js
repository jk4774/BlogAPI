export function makeRequest(methodName, url, onloadCallback, objectToSend, alertMessage) {
    var xhr = new XMLHttpRequest();
    xhr.open(methodName, url);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.onload = () => {
        if (xhr != '200')
            return alert(alertMessage);
        onloadCallback();
    }; 
    xhr.send(objectToSend);
}

export function validation (value) {
    !value ? false : true;
}  


