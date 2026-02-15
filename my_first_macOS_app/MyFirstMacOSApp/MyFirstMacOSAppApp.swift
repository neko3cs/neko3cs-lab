//
//  MyFirstMacOSAppApp.swift
//  MyFirstMacOSApp
//
//  Created by neko3cs on 2026/02/14.
//

import SwiftUI
import SwiftData

@main
struct MyFirstMacOSAppApp: App {
    @AppStorage("isDarkMode") private var isDarkMode = true

    var body: some Scene {
        WindowGroup {
            ContentView()
        }
        // SwiftDataの保存対象を指定。配下のViewで@Queryが使用可能になる
        .modelContainer(for: HistoryItem.self)
        .defaultSize(width: 800, height: 500)
        // コンテンツのサイズ制約をウィンドウに適用
        .windowResizability(.contentSize)

        Settings {
            Form {
                Section("表示設定") {
                    Toggle("ダークモード固定（仮）", isOn: $isDarkMode)
                }
            }
            .formStyle(.grouped)
            .frame(width: 350, height: 250)
            .navigationTitle("設定")
        }

        MenuBarExtra("MyFirstMacOSApp", systemImage: "star.fill") {
            Button("終了") {
                NSApplication.shared.terminate(nil)
            }
            .keyboardShortcut("q")
        }
    }
}
