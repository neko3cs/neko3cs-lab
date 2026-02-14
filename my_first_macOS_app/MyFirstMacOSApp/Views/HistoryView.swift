//
//  HistoryView.swift
//  MyFirstMacOSApp
//
//  Created by neko3cs on 2026/02/14.
//

import SwiftUI
import SwiftData

struct HistoryListView: View {
    @Environment(\.modelContext) private var modelContext
    var history: [HistoryItem]
    
    var body: some View {
        List {
            ForEach(history) { item in
                HStack {
                    Image(systemName: "calendar")
                        .foregroundStyle(.blue)
                    Text(item.timestamp.formatted(date: .numeric, time: .standard))
                }
                .transition(.asymmetric(insertion: .move(edge: .leading).combined(with: .opacity), removal: .opacity))
            }
            .onDelete(perform: deleteItems)
        }
        .navigationTitle("履歴")
        .toolbar {
            ToolbarItem(placement: .primaryAction) {
                Button(role: .destructive) {
                    resetHistory()
                } label: {
                    Label("すべて削除", systemImage: "trash")
                }
                .disabled(history.isEmpty)
            }
        }
        .overlay {
            if history.isEmpty {
                ContentUnavailableView("履歴なし", systemImage: "clock", description: Text("カウントするとここに表示されます"))
            }
        }
    }
    
    private func resetHistory() {
        withAnimation {
            try? modelContext.delete(model: HistoryItem.self)
            try? modelContext.save()
        }
    }
    private func deleteItems(offsets: IndexSet) {
        withAnimation {
            for index in offsets {
                modelContext.delete(history[index])
                try? modelContext.save()
            }
        }
    }
}
