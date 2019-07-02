import React, { Component } from "react";
import './App.css';
import * as signalR from '@aspnet/signalr';
import * as uuidv4 from 'uuid/v4';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

toast.configure({
  autoClose: 2000,
  draggable: false,
  //etc you get the idea
});

class App extends Component {
  constructor(props) {
    super(props);
    
    this.uniqueKey = uuidv4();
     
    this.state = { name:"", value:"", messages:[] };
  }

  notifyUserConnected = (data) => {
    toast.success(`${data.profile.name} connected`);
  }
  
  notifyUserDisConnected = (data) => {
    toast.info(`${data.profile.name} disconnected`);
  }
  
  notifyUserDuplicated = (data) => {
    toast.error(`The unique-key is Duplication:  ${data.uniqueKey}`);
  }
  
  messageReceived = (data) =>{
    
    this.state.messages.push(data);

    this.setState({ messages: this.state.messages});
  }
  
  handleChangeMessge = (e) => { 
    this.setState({ value: e.target.value });
  }

  handleChangeName = (e) => { 
    this.setState({ name: e.target.value });
  }

  handleSend = (e) => {
    this.connection.invoke("SendAsync", { 
      body: this.state.value,
      sendToAll:true
    });
  };
  
  handleConnection = (e) => {
    this.connection = new signalR.HubConnectionBuilder()
    .withUrl(`http://localhost:7500/js/c-sharp-message-queue?uniqueKey=${this.uniqueKey}&name=${this.state.name}`, {})
    .build();

    this.connection.start().then(function () { });

    this.connection.on("NotifyUserConnected", this.notifyUserConnected);
   
    this.connection.on("NotifyUserDisConnected", this.notifyUserDisConnected);

    this.connection.on("NotifyUserDuplicated", this.notifyUserDuplicated);

    this.connection.on("MessageReceived", this.messageReceived);
  };

  render() {
    return (
      <div style={{ marginLeft:"50px", marginTop:"20px " }}>
        <input type="text" value={this.uniqueKey} readOnly="readOnly"></input>
        <input type="text" onChange={this.handleChangeName} placeholder="Display Name" value={this.state.name}></input>
        <br/><br/>
        <button onClick={this.handleConnection}>Connect</button>
        <br/><br/>
        <textarea onChange={this.handleChangeMessge} placeholder="Type your word"></textarea>
        <br/><br/>
        <button onClick={this.handleSend}>Send</button>
        <ul>
          {this.state.messages.map((item, index) => {
             return <li key={index}>
                <span>{item.fromProfile.name}: </span>
                <span>{item.body} </span>
                <span>{item.sendDate}</span>
               </li>;
          })}
        </ul>
      </div>
    );
  }
}

export default App;
