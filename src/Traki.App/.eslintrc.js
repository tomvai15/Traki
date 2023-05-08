module.exports = {
    'env': {
        'browser': true,
        'es2021': true
    },
    'extends': [
        'eslint:recommended',
        'plugin:react/recommended',
        'plugin:react-hooks/recommended',
        'plugin:@typescript-eslint/eslint-recommended',
        'plugin:@typescript-eslint/recommended',
        'plugin:@typescript-eslint/recommended-requiring-type-checking',
    ],
    'overrides': [
    ],
    'parser': '@typescript-eslint/parser',
    'parserOptions': {
        'project': './tsconfig.json',
    },
    'plugins': [
        'react',
        'react-hooks',
        '@typescript-eslint'
    ],
    'rules': {
        'react-hooks/exhaustive-deps': 'off',
        '@typescript-eslint/no-misused-promises': 'off',
        '@typescript-eslint/no-floating-promises': 'off',
        'indent': [
            'error', 
            2, 
            { 'SwitchCase': 1 }
        ],
        'linebreak-style': [
            'error',
            'unix'
        ],
        'quotes': [
            'error', 
            'single', 
            { 'avoidEscape': true }
        ],
        'semi': [
            'error',
            'always'
        ],
        'no-empty-function': 'off',
    },
    'settings': {
        'react': {
          'version': 'detect',
        },
    },
};
