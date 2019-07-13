import { createStore, combineReducers } from 'redux';
import * as csharpQueue from './csharp-message-queue-factory';
import ChatStore from "./store/chat-store";

export default function ConfigureStore() {
    
    const reducers = combineReducers({ChatStore});

    const store = createStore(reducers);

    csharpQueue.factory.subscribe((type, data) =>{
      switch(type){
        case csharpQueue.MESSAGE_RECEIVED:
          store.dispatch({ type: data.label, params: data });
          break;
        default: store.dispatch({ type: type, params: data });
      }
      
    });
    
    return store;
}

