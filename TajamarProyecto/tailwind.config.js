module.exports = {
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml'
    ],
    theme: {
        extend: {
            colors: {
                primary: {
                    100: '#d4eaf7',
                    200: '#b6ccd8',
                    300: '#3b3c3d',
                },
                accent: {
                    100: '#71c4ef',
                    200: '#00668c',
                },
                text: {
                    100: '#1d1c1c',
                    200: '#313d44',
                },
                bg: {
                    100: '#fffefb',
                    200: '#f5f4f1',
                    300: '#cccbc8',
                },
                primaryDark: {
                    100: '#0c6a8a',
                    200: '#00445f',
                    300: '#003954',
                },
                accentDark: {
                    100: '#007b9b',
                    200: '#00445f',
                },
                textDark: {
                    100: '#e6e6e6',
                    200: '#b3b3b3',
                },
                redDark: {
                    100: '#8c0e00',
                    200: '#5f0400',
                },
                bgDark: {
                    100: '#c8c6bc',
                    200: '#a8a599',
                    300: '#7f7d6c',
                },
            },
            screens: {
                'sm': '640px',
                'md': '768px',
                'lg': '1024px',
                'xl': '1280px',
                '2xl': '1536px',
                'custom': '900px', // Nuevo breakpoint personalizado
            },
        },
    },
    plugins: [],
}