import * as signalR from "@microsoft/signalr";
import React, { useEffect, useRef, useState } from "react";

const SERVER_URL = "http://localhost:5096/chatHub";

function App() {
  const connection = useRef();

  const [userMessageList, setUserMessageList] = useState([]);
  const [userInput, setUserInput] = useState('');
  const [sendCounter, setSendCounter] = useState(0);
  const [sendDisabled, setSendDisabled] = useState(true);

  function userChangeHandler(e) { setUserInput(e.target.value); }
  function submitMessage(e) {
    e.preventDefault();
    const message = sendCounter.toString();
    connection.current
      .invoke("SendMessage", userInput, message)
      .catch(console.error);
    setSendCounter(state => state + 1);
  };

  useEffect(() => {
    connection.current = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(SERVER_URL)
      .build();

    connection.current.on("ReceiveMessage", function (user, message) {
      const newUserMessage = { user, message };
      setUserMessageList(list => [...list, newUserMessage]);
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
        <button disabled={sendDisabled}>Send Message</button>
      </form>
      <hr />
      <ul id="messagesList">
        {userMessageList.map((item, i) => {
          const { user, message } = item;
          const text = `user "${user}" says: "${message}"`;
          return <li key={i}>{text}</li>
        })}
      </ul>
    </div>
  );
}

export default App;
