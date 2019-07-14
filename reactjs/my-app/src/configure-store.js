import { createStore, applyMiddleware, combineReducers } from 'redux';
import * as csharpQueue from './csharp-message-queue-factory';
import * as actionTypes from './action_type';
import ChatStore from "./store/chat-store";

export default function ConfigureStore() {
    
    const reducers = combineReducers({ChatStore});
    
    const middlewares = applyMiddleware(csharpInvokeMiddleware);

    const store = createStore(reducers, middlewares);

    csharpQueue.factory.subscribe((type, data) =>{
      switch(type){
        case actionTypes.CSHARP_MESSAGE_RECEIVED:
          store.dispatch({ type: data.label, params: data });
          break;
        default: store.dispatch({ type: type, params: data });
      }
      
    });
    
    return store;
}

function csharpInvokeMiddleware(store) {
  return (next) => async (action) => {
    switch (action.type) {
      case actionTypes.CSHARP_START_CONNECTION: 
          csharpQueue.factory.start(action.params);
          break;   
      case actionTypes.CSHARP_STOP_CONNECTION: 
           csharpQueue.factory.stop();
          break;   
      case actionTypes.CSHARP_PUBLISH_MESSAGE: 
           csharpQueue.factory.publish(action.params);
          break;   
      default: return next(action);
      }
  }
}
