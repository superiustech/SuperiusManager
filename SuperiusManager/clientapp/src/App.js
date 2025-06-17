import React, { useEffect, useCallback, useMemo, memo } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import SemAcessoPage from './pages/SemAcessoPage';
import Layout from './components/layout/Layout';
import { AuthProvider, useAuth } from './components/common/AuthContext';
import ProtectedRoute from './components/common/ProtectedRoute';
import './styles/css/site.css';
import './styles/App.css';
import 'bootstrap/dist/css/bootstrap.min.css';

const AuthenticatedApp = memo(() => (
    <Layout>
        <Routes>
            {/*Dashboard*/}
            {/*<Route path="/manager/dashboard" element={<ProtectedRoute funcionalidade={funcionalidades.VISUALIZAR_DASHBOARD}> <DashboardPage /> </ProtectedRoute>} />*/}

            {/* Redirecionamento para login se acessar qualquer rota sem estar logado */}
            <Route path="/administrador/sem-acesso" element={<ProtectedRoute><SemAcessoPage /></ProtectedRoute>} />
            <Route path="*" element={<Navigate to="/administrador/login" replace />} />
        </Routes>
    </Layout>
));

const PublicRoutes = memo(() => (
    <Routes>
        <Route path="/administrador/login" element={<LoginPage />} />
        <Route path="*" element={<Navigate to="/administrador/login" replace />} />
    </Routes>
));

const AppWrapper = memo(() => {
    const { isAuthenticated } = useAuth();
    return (
        <div className="app-container">
            {isAuthenticated() ? <AuthenticatedApp /> : <PublicRoutes />}
        </div>
    );
});
function App() {
    useEffect(() => {
        document.documentElement.lang = 'pt-BR';
        document.documentElement.charset = 'UTF-8';
    }, []);

    return (
        <Router>
            <AuthProvider>
                <AppWrapper />
            </AuthProvider>
        </Router>
    );
}

export default App;