//
//  SettingsView.swift
//  MyFirstMacOSApp
//
//  Created by opencode on 2026/02/14.
//

import SwiftUI

struct SettingsView: View {
    @AppStorage("isDarkMode") private var isDarkMode = true
    
    var body: some View {
        Form {
            Section("表示設定") {
                Toggle("ダークモード固定（仮）", isOn: $isDarkMode)
            }
        }
        .formStyle(.grouped)
        .frame(width: 350, height: 250)
        .navigationTitle("設定")
    }
}

#Preview {
    SettingsView()
}
