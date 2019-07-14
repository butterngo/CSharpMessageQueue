import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from "react-redux";
import './index.css';
import App from './container/App';
import ConfigureStore from "./configure-store";

function render() {

    let store = ConfigureStore();

    ReactDOM.render(
      <Provider store={store}>
          <App />
      </Provider>,
      document.getElementById("root")
    );
  }
  
  render();