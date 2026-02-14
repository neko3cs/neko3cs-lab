//
//  CounterView.swift
//  MyFirstMacOSApp
//
//  Created by neko3cs on 2026/02/14.
//

import SwiftUI

struct CounterView: View {
    let count: Int
    let onAdd: () -> Void
    let onReset: () -> Void
    
    var body: some View {
        VStack(spacing: 20) {
            Text("現在のカウント").font(.headline)
            
            NumberDisplay(value: count)
            
            HStack(spacing: 15) {
                Button(action: onAdd) {
                    Label("カウントアップ", systemImage: "plus.circle.fill")
                }
                .buttonStyle(.borderedProminent)
                .controlSize(.large)
                
                Button(role: .destructive, action: onReset) {
                    Label("リセット", systemImage: "arrow.counterclockwise")
                }
                .buttonStyle(.bordered)
                .controlSize(.large)
                .disabled(count == 0)
            }
        }
        .navigationTitle("カウンター")
    }
}

#Preview {
    CounterView(count: 10, onAdd: {}, onReset: {})
}
