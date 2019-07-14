import React, { Component } from "react";
import './App.css';
import * as uuidv4 from 'uuid/v4';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import {connect} from 'react-redux';
import * as actionTypes from '../action_type';

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

  componentWillReceiveProps(nextprops) {
    if(nextprops.ChatStore.toastMessage !=="") toast.success(nextprops.ChatStore.toastMessage);
  }

  handleChangeMessge = (e) => { 
    this.setState({ value: e.target.value });
  }

  handleChangeName = (e) => { 
    this.setState({ name: e.target.value });
  }

  handleSend = (e) => this.props.dispatch({
    type:actionTypes.CSHARP_PUBLISH_MESSAGE,
    params:{ 
      body: this.state.value,
      label: actionTypes.CHAT_MESSAGE,
      sendToAll:true
    }
  })
  
  handleConnection = (e) => this.props.dispatch({
    type:actionTypes.CSHARP_START_CONNECTION,
    params:{ 
      uniqueKey: this.uniqueKey,
       name: this.state.name
      }
  })

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
          {this.props.ChatStore.messages.map((item, index) => {
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

const mapStateToProps = state => state;

export default connect(mapStateToProps)(App);
