### Getting Started...

## Project Folder Structure

project/
├── src/
│
├── components/
│   ├── CafePage.js
│   ├── EmployeePage.js
│   └── common/
│		├── EmployeeDialog.js
│		├── CafeDailog.js
│		├── ConfirmDailog.js
│		└── ReusableTextField.js
│
├── services/
│    └── app.js
│
├── util/
│    └── validation.js
├── App.js
└── index.js



## Creating a new React app

### npx create-react-app sample-crud-react

## Available Scripts

    In the project directory, you can run:

### npm install
    Installs all dependencies listed in package.json

### npm install react react-scripts react-dom react-router-dom 
    Installs core React libraries:

    react: Main React library

    react-dom: DOM-specific methods for React

    react-router-dom: Routing in React apps

    react-scripts: Scripts and configuration used by Create React App

### npm install @mui/material @mui/icons-material @emotion/react @emotion/styled @mui/x-date-pickers dayjs ag-grid-react 

    Installs UI libraries:

    @mui/material: MUI components (Material-UI)

    @mui/icons-material: MUI icon set

    @emotion/react & @emotion/styled: Emotion CSS-in-JS for MUI styling

    @mui/x-date-pickers: MUI’s extended DatePicker & DateTimePicker components

    dayjs: Lightweight date library used with the pickers

    ag-grid-react: AG Grid table/grid component for React

### npm install axios@1.3.5 

    Installs Axios, a popular HTTP client for API requests.

### npm install ajv@8
    Installs AJV (Another JSON Schema Validator).

### npm cache clean --force
    clean the cache of the installed packages

The page will reload when you make changes.\
You may also see any lint errors in the console.

### npm start
    Starts the development server.

###  npm run build
    Bundles the app into static files for production.

###  npm test
    Starts the test runner.

###  npm run eject
    Removes this tool and copies build dependencies, configuration files
    and scripts into the app directory. If you do this, you can’t go back!

### Deployment

### `npm run build` fails to minify

