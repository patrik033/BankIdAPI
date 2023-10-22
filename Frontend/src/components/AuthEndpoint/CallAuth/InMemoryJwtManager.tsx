const InMemoryJwtManager = () => {
    let inMemoryJwt: null | string;
    const getToken = () => inMemoryJwt;

    const setToken = (token: null | null) => {
        inMemoryJwt = token;
        return true;
    };

    const deleteToken = () => {
        inMemoryJwt = null;
        return true;
    };

    return{
        getToken,
        setToken,
        deleteToken
    };
};

export default InMemoryJwtManager();