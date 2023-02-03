import { useState } from "react";
import { invoke } from "@tauri-apps/api/tauri";
import { open } from "@tauri-apps/api/dialog";
import "./App.css";

function App() {
  const [tickets, setTickets] = useState([""]);

  async function openDialog() {
    /**1. @tauri-apps/api/dialog#openで選択したファイルのパスを取得。 */
    const path = await open({ multiple: false });
    if (typeof path === "string") {
      /**2. create_ticketコマンドを実行。String[]で受け取ったチケット一覧をセット。 */
      setTickets(await invoke<string[]>("create_ticket", { path: path }));
    }
  }

  const ticketsEl = tickets.map((ticket) => <p>{ticket}</p>);

  return (
    <div className="container">
      <h1>Welcome to Tauri!</h1>

      <div className="row">
        <button onClick={openDialog}>add tickets from file</button>
        <button onClick={() => setTickets([])}>clear tickets</button>
      </div>

      {ticketsEl}
    </div>
  );
}

export default App;
