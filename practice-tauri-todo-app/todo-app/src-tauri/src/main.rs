#![cfg_attr(
    all(not(debug_assertions), target_os = "windows"),
    windows_subsystem = "windows"
)]

/**指定されたパスのファイル読み込み */
fn read_file(path: String) -> String {
    std::fs::read_to_string(path).expect("could not read file")
}

/**2. パスを受け取り、読み込んだファイルを改行で区切りVecにして返す。 */
#[tauri::command]
fn create_ticket(path: String) -> Vec<String>{
    println!("create ticket");
    let tickets = read_file(path);
    tickets.lines().map(|s|s.to_string()).collect()
}

fn main() {
    tauri::Builder::default()
        .invoke_handler(tauri::generate_handler![create_ticket])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}