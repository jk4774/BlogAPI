import { validation, makeRequest } from '../app'
document.getElementById('deleteUser')
    .addEventListener('click', 
        makeRequest('DELETE', '/user/delete/', () => { window.location.href = '/'; }, null, 'Cannot delete, something went wrong' ));
