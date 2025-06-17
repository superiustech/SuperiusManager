import axios from 'axios';

const baseApiUrl = `${window.location.origin}/api`;
const getAuthToken = () => localStorage.getItem('authToken'); 
const axiosWithToken = (baseURL) => {
    const instance = axios.create({ baseURL });
    instance.interceptors.request.use(
        (config) => {
            const token = getAuthToken();
            if (token) {
                config.headers.Authorization = `Bearer ${token}`;
            }
            return config;
        },
        (error) => Promise.reject(error)
    );
    return instance;
};

export default {
    autenticacao: {
        baseURL: `${baseApiUrl}/Autenticacao`,
        endpoints: {
            token: "/Token",
            login: "/Login"
        }
    },
    dashboard: {
        baseURL: `${baseApiUrl}/Dashboard`,
        axios: axiosWithToken(`${baseApiUrl}/Dashboard`),
        endpoints: {
            dashboardResumo: "/ResumoDashboard",
            produtosPorEstoques: "/ProdutosPorEstoques"
        }
    }
};
