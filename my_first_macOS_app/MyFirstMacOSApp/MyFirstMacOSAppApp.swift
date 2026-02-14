//
//  MyFirstMacOSApp.swift
//  MyFirstMacOSApp
//
//  Created by neko3cs on 2026/02/14.
//

import SwiftUI
import SwiftData

@main
struct MyFirstMacOSApp: App {
    var body: some Scene {
        WindowGroup {
            ContentView()
        }
        .modelContainer(for: HistoryItem.self)
        .defaultSize(width: 800, height: 500)
        .windowResizability(.contentSize)

        Settings {
            VStack(spacing: 20) {
                Text("アプリの設定").font(.title)
                Toggle("ダークモード固定（仮）", isOn: .constant(true))
            }
            .frame(width: 300, height: 200)
        }

        MenuBarExtra("MyFirstMacOSApp", systemImage: "star.fill") {
            Button("終了") { NSApplication.shared.terminate(nil) }
        }
    }
}
