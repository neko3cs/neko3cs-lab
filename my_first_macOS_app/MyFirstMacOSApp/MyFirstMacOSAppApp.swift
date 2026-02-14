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
    var body: some Scene {
        WindowGroup {
            ContentView()
        }
        .modelContainer(for: HistoryItem.self)
        .defaultSize(width: 800, height: 500)
        // 最小サイズを指定してレイアウト崩れを防ぐ
        .windowResizability(.contentSize)

        Settings {
            SettingsView()
        }

        MenuBarExtra("MyFirstMacOSApp", systemImage: "star.fill") {
            Button("終了") {
                NSApplication.shared.terminate(nil)
            }
            .keyboardShortcut("q")
        }
    }
}
