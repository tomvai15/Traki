import React from 'react';
import './App.css';
import Dashboard from './pages/Dashboard';
import ThemeProvider from '@mui/material/styles/ThemeProvider';
import createTheme from '@mui/material/styles/createTheme';

const mdTheme = createTheme({ 
    palette: {
        primary: {
            main: '#e4ae3f',
        },
        secondary: {
            main: '#9ab1c0',
        }
    }
});

function App() {
    return (
        <ThemeProvider theme={mdTheme}>
            <Dashboard/>
        </ThemeProvider>
    );
}

export default App;
