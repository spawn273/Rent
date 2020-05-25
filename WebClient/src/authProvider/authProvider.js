import decodeJwt from 'jwt-decode';
import { useSelector, useDispatch } from 'react-redux';

const authProvider = {
    login: ({ username, password }) =>  {
        const request = new Request(`${process.env.REACT_APP_API_URL}/Authorization/token`, {
            method: 'POST',
            body: JSON.stringify({ email: username, password }),
            headers: new Headers({ 'Content-Type': 'application/json' }),
        });
        return fetch(request)
            .then(response => {
                if (response.status < 200 || response.status >= 300) {
                    throw new Error(response.statusText);
                }
                return response.json();
            })
            .then(({ token }) => {
                const decodedToken = decodeJwt(token);
                localStorage.setItem('token', token);
                
                const permissions = {
                    role: decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
                    shop: decodedToken.shop
                };

                localStorage.setItem('permissions', JSON.stringify(permissions));
            });
    },
    
    logout: () => {
        localStorage.removeItem('token');
        localStorage.removeItem('permissions');
        return Promise.resolve();
    },

    checkAuth: () => {
        return localStorage.getItem('token') ? Promise.resolve() : Promise.reject();
    },
    
    getPermissions: () => {
        const permissions = JSON.parse(localStorage.getItem('permissions'));
        const perm = {
            ...permissions,
            isMyShop: (shopId) => {
                return shopId == permissions.shop;
            }
        };

        return Promise.resolve(perm);
        // return role ? Promise.resolve(role) : Promise.reject();
    }
};

export default authProvider;
