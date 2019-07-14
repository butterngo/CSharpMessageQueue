import * as signalR from '@aspnet/signalr';
import * as actionTypes from './action_type';

var _connection = null, _store = null, _func = null;

export const factory = {
    
	start: (options) => {
      _connection = new signalR.HubConnectionBuilder()
      .withUrl(`http://localhost:7500/js/c-sharp-message-queue?uniqueKey=${options.uniqueKey}&name=${options.name}`, {})
      .build();

      _connection.start().then(() => {
        
        _connection.on("NotifyUserConnected", (data) => _func(actionTypes.CSHARP_NOTIFY_USER_CONNECTED, data));
    
        _connection.on("NotifyUserDisConnected", (data) => _func(actionTypes.CSHARP_NOTIFY_USER_DIS_CONNECTED, data));
  
        _connection.on("NotifyUserDuplicated", (data) => _func(actionTypes.CSHARP_NOTIFY_USER_DUPLICATED, data));
  
        _connection.on("MessageReceived", (data) => _func(actionTypes.CSHARP_MESSAGE_RECEIVED, data));

       });

	},
    
	stop:() => _connection.stop(error => console.log(error)),
    
	subscribe: (func) => {
        if(typeof func === "function") _func = func;

        else throw "The parameter must be a function.";
    },
	
	publish:(params) => _connection.invoke("SendAsync", params),
};