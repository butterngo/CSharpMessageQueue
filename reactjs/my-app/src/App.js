import React, { Component } from "react";
import './App.css';
import * as signalR from '@aspnet/signalr';
class App extends Component {
  constructor(props) {
    super(props);
    
  }

   toUTF8Array(str) {
    let utf8 = [];
    for (let i = 0; i < str.length; i++) {
        let charcode = str.charCodeAt(i);
        if (charcode < 0x80) utf8.push(charcode);
        else if (charcode < 0x800) {
            utf8.push(0xc0 | (charcode >> 6),
                      0x80 | (charcode & 0x3f));
        }
        else if (charcode < 0xd800 || charcode >= 0xe000) {
            utf8.push(0xe0 | (charcode >> 12),
                      0x80 | ((charcode>>6) & 0x3f),
                      0x80 | (charcode & 0x3f));
        }
        // surrogate pair
        else {
            i++;
            // UTF-16 encodes 0x10000-0x10FFFF by
            // subtracting 0x10000 and splitting the
            // 20 bits of 0x0-0xFFFFF into two halves
            charcode = 0x10000 + (((charcode & 0x3ff)<<10)
                      | (str.charCodeAt(i) & 0x3ff));
            utf8.push(0xf0 | (charcode >>18),
                      0x80 | ((charcode>>12) & 0x3f),
                      0x80 | ((charcode>>6) & 0x3f),
                      0x80 | (charcode & 0x3f));
        }
    }
    return utf8;
}
  componentDidMount(){
    
    
    const connection = new signalR.HubConnectionBuilder()
		.withUrl("http://localhost:7500/c-sharp-message-queue", {})
		.build();
connection.start().then(function () {
    console.log("connected");
});
    
   /* var bytes = this.toUTF8Array("testvu");
    console.log(bytes);*/

   /* this.connection.on("send", data => {
      console.log(data);
    });
   
   */
  }

  render() {
    return (
      <div>My name is butter</div>
    );
  }
}

export default App;
