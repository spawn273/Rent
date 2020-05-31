import decodeJwt from 'jwt-decode';
import { useSelector, useDispatch } from 'react-redux';
import authProvider from '../../authProvider/authProvider';

authProvider.checkAuth = () => {
    let permissions = JSON.parse(localStorage.getItem('permissions'));
    if (permissions && permissions.role === 'admin') {
        return Promise.resolve();
    }
    return Promise.reject();
};

export default authProvider;
