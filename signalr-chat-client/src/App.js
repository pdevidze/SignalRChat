import * as signalR from "@microsoft/signalr";
import React, { useEffect, useRef, useState } from "react";

const SERVER_URL = "http://localhost:5096/chatHub";

function App() {
  const connection = useRef();

  const [messageList, setMessageList] = useState([]);
  const [userInput, setUserInput] = useState('');
  const [messageInput, setMessageInput] = useState('');
  const [sendDisabled, setSendDisabled] = useState(true);

  function userChangeHandler(e) { setUserInput(e.target.value); }
  function messageChangeHandler(e) { setMessageInput(e.target.value); }
  function submitMessage(e) {
    e.preventDefault();
    connection.current
      .invoke("SendMessage", userInput, messageInput)
      .catch(console.error);
  };

  useEffect(() => {
    connection.current = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(SERVER_URL)
      .build();

    connection.current.on("ReceiveMessage", function (user, message) {
      const newMessage = `${user} says: ${message}`;
      setMessageList(list => [...list, newMessage]);
    });

    connection.current.on("WorkerPrinted", function (message) {
      const newMessage = `WORKER printed: ${message}`;
      setMessageList(list => [...list, newMessage]);
    });

    connection.current.start()
      .then(() => setSendDisabled(false))
      .catch(console.error);
  }, []);

  return (
    <div className="App">
      <form onSubmit={submitMessage}>
        <label htmlFor="user">User:</label>
        <input id="user" value={userInput} onChange={userChangeHandler} />
        <label htmlFor="message">Message:</label>
        <input id="message" value={messageInput} onChange={messageChangeHandler} />
        <button disabled={sendDisabled}>Send Message</button>
      </form>
      <hr />
      <ul id="messagesList">
        {messageList.map((text, i) => {
          return <li key={i}>{text}</li>
        })}
      </ul>
    </div>
  );
}

export default App;
